using SONEPAR.SAPMIGRATION.WSS.EXCEPTION;
using SONEPAR.SAPMIGRATION.WSS.HELPER;
using System;
using System.Collections.Generic;
using System.Linq;
using SONEPAR.SAPMIGRATION.WSS.MODEL;

namespace SONEPAR.SAPMIGRATION.WSS.LOGIC
{
    public class DocumentLogic : ConnectionLogic
    {
        public List<Tuple<MigrationDocument, List<MigrationDocumentLine>>> GetPendingDocumentsFromDb(AppSource appSource)
        {
            var documentList = new List<Tuple<MigrationDocument, List<MigrationDocumentLine>>>();

            //Obtiene documentos pendientes en bruto de la base de datos
            var documentHeaderList = GetMigrationDataContext().MigrationDocument.Where(x => (x.C_MigrationState == null || x.C_MigrationState == (int)MigrationState.Error) && x.C_AppSource == (int)appSource).ToList();
            foreach (var documentHeader in documentHeaderList)
            {
                var documentLines = GetMigrationDataContext().MigrationDocumentLine.Where(x => x.C_ParentExternalId == documentHeader.C_ExternalId).ToList();
                documentList.Add(Tuple.Create(documentHeader, documentLines));
            }

            return documentList;
        }
        public List<MigrationDocument> GetPendingDocumentsForMail()
        {
            //Obtiene documentos pendientes en bruto de la base de datos
            var documentHeaderList = GetMigrationDataContext().MigrationDocument.Where(x => x.C_MigrationMailWasSended == null
                                                                                            || x.C_MigrationMailWasSended == false).ToList();
            return documentHeaderList;
        }

        public void SetLogInformation(Exception ex, ref MigrationDocument migrationDocument)
        {
            MigrationState? migrationState = null;
            ApplicationErrorType? applicationErrorType = null;
            string errorMessage = null;

            if (ex.GetType() == typeof(SapException))
            {
                migrationState = MigrationState.Error;
                applicationErrorType = ApplicationErrorType.EnvioASap;
                errorMessage = GetLastSapErrorString();
            }
            else if (ex.GetType() == typeof(CustomException))
            {
                migrationState = MigrationState.Error;
                applicationErrorType = ApplicationErrorType.Internal;
                errorMessage = ex.ToString();
            }
            else
            {
                migrationState = MigrationState.Error;
                applicationErrorType = ApplicationErrorType.Unhandled;
                errorMessage = ex.ToString();
            }

            if (migrationState != null)
                migrationDocument.C_MigrationState = (int)migrationState;

            if (applicationErrorType != null)
                migrationDocument.C_MigrationErrorType = (int)applicationErrorType;

            if (errorMessage != null)
                migrationDocument.C_MigrationErrorMessage = ConvertHelper.GetLogErrorConcatened(errorMessage, migrationDocument.C_MigrationErrorMessage);

            GetMigrationDataContext().Entry(migrationDocument);
            GetMigrationDataContext().SaveChanges();
        }
        #region Util
        #endregion



    }
}
