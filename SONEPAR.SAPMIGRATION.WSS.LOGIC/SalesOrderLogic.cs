using SAPbobsCOM;
using SONEPAR.SAPMIGRATION.WSS.DATAACCESS;
using SONEPAR.SAPMIGRATION.WSS.HELPER;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Document;
using SONEPAR.SAPMIGRATION.WSS.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using static SONEPAR.SAPMIGRATION.WSS.HELPER.ConstantHelper;

namespace SONEPAR.SAPMIGRATION.WSS.LOGIC
{
    public class SalesOrderLogic : ConnectionLogic
    {
        public void Process()
        {
            var documents = new DocumentLogic().GetPendingDocumentsFromDb(AppSource.AppSonemas);
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
                SetCurrentCompany(migrationDocument.C_DestinationSapDb);

                //Prepara y setea información en bd
                SetAditionalProperties(ref migrationDocument, ref migrationDocumentLines);

                //Convierte documento de bd en documento de transferencia a sAP
                var sapDocumentModel = ConvertToSapDocumentModel(migrationDocument, migrationDocumentLines);

                //Agrega el documento a SAP y actualiza datos de migración
                AddUpdateDocument(sapDocumentModel, ref migrationDocument);

                //Prepara y setea información de correo
                SetMailInfo(ref migrationDocument);
            }
            catch (Exception ex) { new DocumentLogic().SetLogInformation(ex, ref migrationDocument); }
            finally { GetMigrationDataContext().SaveChanges(); }
        }
        private DocumentViewModel ConvertToSapDocumentModel(MigrationDocument migrationDocument, List<MigrationDocumentLine> migrationDocumentLines)
        {
            var originMigrationDocument = migrationDocument;
            var model = (DocumentViewModel)ReflectionHelper.CopyAToB(migrationDocument, typeof(DocumentViewModel), false, typeof(MigrationDocument));

            model.ObjectType = (BoObjectTypes)(originMigrationDocument.C_SaveAsDraft == true ? (int)BoObjectTypes.oDrafts : originMigrationDocument.DocObjectCode);
            model.DocType = (BoDocumentTypes)originMigrationDocument.DocType;
            model.DocObjectCode = (BoObjectTypes)originMigrationDocument.DocObjectCode;
            model.DocumentSubType = (BoDocumentSubType)(originMigrationDocument.DocumentSubType ?? (int)BoDocumentSubType.bod_None);

            //--------------------------------------------VALIDA CAMPOS REQUERIDOS--------------------------------------------
            model.Series = migrationDocument.Series ?? throw new ArgumentNullException(nameof(migrationDocument.Series));
            model.DocCurrency = migrationDocument.DocCurrency ?? throw new ArgumentNullException(nameof(migrationDocument.DocCurrency));
            model.DocDate = migrationDocument.DocDate ?? throw new ArgumentNullException(nameof(migrationDocument.DocDate));
            model.DocDueDate = migrationDocument.DocDueDate ?? throw new ArgumentNullException(nameof(migrationDocument.DocDueDate));
            model.TaxDate = migrationDocument.TaxDate ?? throw new ArgumentNullException(nameof(migrationDocument.TaxDate));
            model.DueDate = migrationDocument.DueDate ?? migrationDocument.DocDueDate.Value;
            //-----------------------------------------------------------------------------------------------------------------

            foreach (var item in migrationDocumentLines)
            {
                var sapDocumentLine = ReflectionHelper.CopyAToB(item, typeof(DocumentLineViewModel), false, typeof(MigrationDocumentLine));
                model.DocumentLines.Add(sapDocumentLine);
            }

            return model;
        }
        public void AddUpdateDocument(DocumentViewModel model, ref MigrationDocument migrationDocument)
        {
            new DocumentDataAccess().AddUpdateDocument(GetCompany(), model);

            //Actualiza Datos de migración en base de datos intermedia
            var docEntry = GetLastDocEntry();
            migrationDocument.DocEntry = docEntry;
            migrationDocument.DocNum = GetSapDataContext().ORDR.FirstOrDefault(x => x.DocEntry == docEntry).DocNum;
            migrationDocument.C_MigrationDate = DateTime.Now;
            migrationDocument.C_MigrationState = (int)MigrationState.Success;
            GetMigrationDataContext().Entry(migrationDocument);
        }
        private void SetAditionalProperties(ref MigrationDocument migrationDocument, ref List<MigrationDocumentLine> migrationDocumentLines)
        {
            var _migrationDocument = migrationDocument;
            if (migrationDocument.C_MigrationState == (int)MigrationState.Success) return;
            switch (migrationDocument.C_AppSource)
            {
                case (int)AppSource.AppSonemas:
                    SetAditionalProperties_App(ref migrationDocument);
                    break;
                default:
                    throw new NotImplementedException();
            }
            migrationDocumentLines.ForEach(x => SetAditionalProperties((AppSource)_migrationDocument.C_AppSource, ref x));

            GetMigrationDataContext().Entry(migrationDocument);
        }
        private void SetAditionalProperties(AppSource appSource, ref MigrationDocumentLine migrationDocument)
        {
            switch (appSource)
            {
                case AppSource.AppSonemas:
                    SetAditionalProperties_App(ref migrationDocument);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #region Mail
        private MailModel GetMailModel(int? docEntry)
        {
            if (docEntry == null || docEntry == 0)
                return new MailModel();

            var _migrationDocument = GetSapDataContext().ORDR.FirstOrDefault(x => x.DocEntry == docEntry);
            if (_migrationDocument == null)
                throw new ArgumentNullException(nameof(_migrationDocument));

            var sapOrder = GetSapDataContext().ORDR.FirstOrDefault(x => x.DocEntry == docEntry);
            if (sapOrder == null)
                throw new ArgumentNullException(nameof(sapOrder));

            var gestor = GetSapDataContext().OSLP.FirstOrDefault(x => x.SlpCode == sapOrder.SlpCode);
            if (gestor == null)
                throw new ArgumentNullException(nameof(gestor));

            var personaContacto = GetSapDataContext().OCPR.FirstOrDefault(x => x.CntctCode == sapOrder.CntctCode);
            if (personaContacto == null)
                throw new ArgumentNullException(nameof(personaContacto));

            var sucursal = GetSapDataContext().OPRJ.FirstOrDefault(x => x.PrjCode == _migrationDocument.Project);

            MailModel mailModel = new MailModel();
            mailModel.NombreCliente = sapOrder.CardName;
            mailModel.DireccionDeEntrega = sapOrder.Address;
            mailModel.CorreoCliente = personaContacto?.E_MailL;
            mailModel.NumeroDocumentoCliente = sapOrder.LicTradNum;
            mailModel.GestorCorreo = gestor.U_RX_EMLEMP;
            mailModel.GestorNombre = gestor.SlpName;
            mailModel.GestorTelefono = gestor.U_RX_TLFEMP;
            mailModel.Moneda = sapOrder.DocCur;
            mailModel.NumeroDocumentoSAP = docEntry.ToSafeString();
            mailModel.MontoIGV = (sapOrder.DocCur == Moneda.SOL ? sapOrder.VatSum : sapOrder.VatSumFC).ToString();
            mailModel.MontoSubTotal = (sapOrder.DocCur == Moneda.SOL ? (sapOrder.DocTotal - sapOrder.VatSum) : (sapOrder.DocTotalFC - sapOrder.VatSumFC)).ToString();
            mailModel.MontoTotal = (sapOrder.DocCur == Moneda.SOL ? sapOrder.DocTotal : sapOrder.DocTotalFC).ToString();
            mailModel.SucursalNombre = sucursal?.U_SON_ZONA;


            foreach (var item in GetSapDataContext().RDR1.Where(x => x.DocEntry == docEntry))
            {
                var itemInSap = GetSapDataContext().OITM.FirstOrDefault(x => x.ItemCode == item.ItemCode);
                var lineOrderInSap = GetSapDataContext().RDR1.FirstOrDefault(x => x.DocEntry == docEntry);
                mailModel.DetalleDocumentoList.Add(new MailDocumentDetail()
                {
                    Item = lineOrderInSap.U_RX_ITEDOC.ToSafeString(),
                    Cantidad = lineOrderInSap.Quantity.ToSafeString(),
                    Categoria = (lineOrderInSap.U_RX_STKDIS >= lineOrderInSap.Quantity) ? "C" : (itemInSap.QryGroup4 == "Y") ? "I" : "T",
                    Descripcion = lineOrderInSap.Dscription.ToSafeString(),
                    FechaEntrega = lineOrderInSap.ShipDate.ToSafeString(),
                    Precio = (sapOrder.DocCur == Moneda.SOL ? lineOrderInSap.LineTotal : lineOrderInSap.TotalFrgn).ToSafeString(),
                    Referencia = lineOrderInSap.ItemCode,
                    STotal = (sapOrder.DocCur == Moneda.SOL ? lineOrderInSap.LineTotal : lineOrderInSap.TotalFrgn).ToSafeString(),
                    UM = lineOrderInSap.unitMsr,
                });
            }
            return mailModel;
        }
        private void SetMailInfo(ref MigrationDocument migrationDocument)
        {
            try
            {
                var docEntry = migrationDocument.DocEntry;
                var mailModel = GetMailModel(docEntry.Value);
                var mailContent = MailHelper.RenderHtmlFromTemplate(EmbebbedFileName.HtmlMailTemplate, mailModel, Assembly.GetExecutingAssembly());

                migrationDocument.C_MigrationMailContentBody = mailContent;
                GetMigrationDataContext().Entry(migrationDocument);
            }
            catch (Exception ex)
            {
                migrationDocument.C_MigrationObs = ConvertHelper.GetLogErrorConcatened(ex.ToSafeString(), migrationDocument.C_MigrationObs);
                GetMigrationDataContext().Entry(migrationDocument);
                throw;
            }
        }
        public void SendMail(ref MigrationDocument migrationDocument, bool regenerateMailBody)
        {
            try
            {
                if (string.IsNullOrEmpty(migrationDocument.C_MigrationMailContentBody))
                    throw new ArgumentNullException(nameof(migrationDocument.C_MigrationMailContentBody));

                if (regenerateMailBody)
                    SetMailInfo(ref migrationDocument);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = ConstantHelper.MailConstant.Subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress("gabriela.antezana@sonepar.pe"));
                mailMessage.Bcc.Add(new MailAddress("gabriela.antezana@sonepar.pe"));

                MailHelper.AddImagesToHtml(ref mailMessage, ConstantHelper.MailConstant.PathImagesResources, migrationDocument.C_MigrationMailContentBody);

                //ENVÍO
                var smtpClient = MailHelper.SetSmtpClientFromConfig(ref mailMessage);
                smtpClient.Send(mailMessage);

                //Actualiza datos de envío
                migrationDocument.C_MigrationMailWasSended = true;
                migrationDocument.C_MigrationMailDate = DateTime.Now;

                GetMigrationDataContext().Entry(migrationDocument);
            }
            catch (Exception ex)
            {
                migrationDocument.C_MigrationObs = ConvertHelper.GetLogErrorConcatened(ex.ToSafeString(), migrationDocument.C_MigrationObs);
                GetMigrationDataContext().Entry(migrationDocument);
            }
        }
        #endregion

        #region Set Aditional Properties for App
        private void SetAditionalProperties_App(ref MigrationDocument migrationDocument)
        {
            var _migrationDocument = migrationDocument;
            var businessPartner = GetSapDataContext().OCRD.FirstOrDefault(x => x.CardCode == _migrationDocument.CardCode);

            /*DOCUMENT OWNER*/
            migrationDocument.DocumentsOwner = GetSapDataContext().OHEM.FirstOrDefault(x => x.salesPrson == _migrationDocument.SalesPersonCode).empID;

            /*GUARDAR COMO BORRADOR*/
            var tienecredito = true;

            //TODO:
            //Si tiene documentos vencidos
            bool _tieneDocumentosVencidos = Convert.ToBoolean(GetSapDataContext().Database.SqlQuery<string>("_SPAPP_GetStatusExpiredDocuments '" + migrationDocument.CardCode + "'").FirstOrDefault());
            if (_tieneDocumentosVencidos)
                tienecredito = false;
            else if (migrationDocument.PaymentGroupCode != ConstantHelper.MetodoPago.Efectivo)
                tienecredito = false;
            else
            {
                if (businessPartner.GroupCode != ConstantHelper.MetodoPago.Efectivo)
                {
                    var balance = (businessPartner.CreditLine + businessPartner.Balance + businessPartner.DNotesBal + businessPartner.OrdersBal);
                    if (balance < 0)
                        tienecredito = false;
                }
            }
            migrationDocument.C_SaveAsDraft = !tienecredito;
        }
        //TODO:
        private void SetAditionalProperties_App(ref MigrationDocumentLine migrationDocumentLine)
        {
            var _migrationDocumentLine = migrationDocumentLine;
            var stocks = GetSapDataContext().OITW.Where(x => x.ItemCode == _migrationDocumentLine.ItemCode && (x.WhsCode == "PRI-FIS" || x.WhsCode == _migrationDocumentLine.WarehouseCode)).Select(x => new { x.OnHand, x.WhsCode }).ToList();
            var stockSucursal = stocks.FirstOrDefault(x => x.WhsCode == "PRI-FIS").OnHand;
            var stockAlmacenPrincipal = stocks.FirstOrDefault(x => x.WhsCode == _migrationDocumentLine.WarehouseCode).OnHand;

            migrationDocumentLine.U_RX_STKDIS = (double?)stockSucursal;
            migrationDocumentLine.U_RX_STKRSV = (double?)stockSucursal;
            migrationDocumentLine.U_RX_STOKCD = (double?)stockAlmacenPrincipal;
        }

        #endregion

    }
}
