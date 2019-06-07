using SONEPAR.SAPMIGRATION.WSS.DATAACCESS;
using SONEPAR.SAPMIGRATION.WSS.HELPER;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Document;
using SONEPAR.SAPMIGRATION.WSS.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SONEPAR.SAPMIGRATION.WSS.LOGIC
{
    public class SalesQuotationLogic : ConnectionLogic
    {
        public void Process()
        {
            var documents = new DocumentLogic().GetPendingDocumentsFromDb(AppSource.WebCotiza);
            foreach (var item in documents)
            {
                Process(item.Item1, item.Item2);
            }
        }
        private void Process(MigrationDocument migrationDocument, List<MigrationDocumentLine> migrationDocumentLines)
        {
            try
            {
                //Setea bd SAP a trabajar
                SetCurrentDataBase(migrationDocument.C_DestinationSapDb);

                //Convierte documento de bd en documento de transferencia a sAP
                var sapDocumentModel = ConvertToSapDocumentModel(migrationDocument, migrationDocumentLines);

                //Agrega el documento a SAP y actualiza datos de migración
                AddUpdateDocument(sapDocumentModel, ref migrationDocument);

            }
            catch (Exception ex) { new DocumentLogic().SetLogInformation(ex, ref migrationDocument); }
            finally { GetMigrationDataContext().SaveChanges(); }
        }

        private void AddUpdateDocument(DocumentViewModel model, ref MigrationDocument migrationDocument)
        {
            new SalesQuotationDataAccess().AddUpdateDocument(GetCompany(), model);
            var docEntry = migrationDocument.DocEntry;
            //Actualiza Datos de migración en base de datos intermedia
            migrationDocument.DocNum = GetSapDataContext().ORDR.FirstOrDefault(x => x.DocEntry == docEntry).DocNum;
            migrationDocument.C_MigrationDate = DateTime.Now;
            migrationDocument.C_MigrationState = (int)MigrationState.Success;
            GetMigrationDataContext().Entry(migrationDocument);
        }

        private DocumentViewModel ConvertToSapDocumentModel(MigrationDocument migrationDocument, List<MigrationDocumentLine> migrationDocumentLines)
        {
            var model = (DocumentViewModel)ReflectionHelper.CopyAToB(migrationDocument, typeof(DocumentViewModel), false, typeof(MigrationDocument));
            model.IsUpdate = migrationDocument.C_IsUpdate;
            foreach (var item in migrationDocumentLines)
            {
                var sapDocumentLine = ReflectionHelper.CopyAToB(item, typeof(DocumentLineViewModel), false, typeof(MigrationDocumentLine));
                model.DocumentLines.Add(sapDocumentLine);
            }
            return model;
        }

    }
}
