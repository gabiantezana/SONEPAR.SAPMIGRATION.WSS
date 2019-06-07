using SAPbobsCOM;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.BusinessPartner;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Company;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Document;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Payment;
using System;

namespace SONEPAR.SAPMIGRATION.WSS.VIEWMODEL
{
    public static class CreateViewModel
    {
        #region Company

        public static CompanyViewModel GenerateViewModel(CompanyXMLModel xmlModel)
        {
            CompanyViewModel model = ReflectionHelper.CopyAToB(xmlModel, typeof(CompanyViewModel), true);

            model.DbServerType = GetSAPEnum(typeof(BoDataServerTypes), xmlModel.DbServerType);
            model.language = String.IsNullOrEmpty(xmlModel.language)? BoSuppLangs.ln_English_Sg : GetSAPEnum(typeof(BoSuppLangs), xmlModel.language);

            return model;
        }

        #endregion

        #region BusinessPartner

        public static BusinessPartnerViewModel GenerateViewModel(BusinessPartnerXMLModel xmlModel)
        {
            BusinessPartnerViewModel model = ReflectionHelper.CopyAToB(xmlModel, typeof(BusinessPartnerViewModel), true);

            model.CardType = GetSAPEnum(typeof(BoCardTypes), xmlModel.CardType);

            xmlModel.BPAddresses.ForEach(bpAddressXMLModel => model.BPAddresses.Add(GenerateViewModel(bpAddressXMLModel)));
            xmlModel.ContactEmployees.ForEach(contactEmployeeXMLModel => model.ContactEmployees.Add(GenerateViewModel(contactEmployeeXMLModel)));

            return model;
        }

        private static BPAddressViewModel GenerateViewModel(BPAddressXMLModel xmlModel)
        {
            BPAddressViewModel bpAddressModel = ReflectionHelper.CopyAToB(xmlModel, typeof(BPAddressViewModel), true);

            bpAddressModel.AddressType = GetSAPEnum(typeof(BoAddressType), xmlModel.AddressType);
            return bpAddressModel;
        }

        private static ContactEmployeeViewModel GenerateViewModel(ContactEmployeeXMLModel xmlModel)
        {
            ContactEmployeeViewModel model = ReflectionHelper.CopyAToB(xmlModel, typeof(ContactEmployeeViewModel), true);
            model.Gender = GetSAPEnum(typeof(BoGenderTypes), xmlModel.Gender);

            return model;
        }

        #endregion

        #region Document

        private static DocumentLineViewModel GenerateViewModel(DocumentLineXMLModel xmlModel)
        {
            DocumentLineViewModel model = ReflectionHelper.CopyAToB(xmlModel, typeof(DocumentLineViewModel), true);
            return model;
        }

        #endregion

        #region Payments

        public static PaymentViewModel GenerateViewModel(PaymentXMLModel xmlModel)
        {
            PaymentViewModel model = ReflectionHelper.CopyAToB(xmlModel, typeof(PaymentViewModel), true);

            model.DocType = GetSAPEnum(typeof(BoRcptTypes), xmlModel.DocType);
            model.DocDate = ConvertHelper.ToDate(xmlModel.DocDate);
            model.TaxDate = ConvertHelper.ToDate(xmlModel.TaxDate);
            model.DueDate = ConvertHelper.ToDate(xmlModel.DueDate);

            xmlModel.PaymentInvoices.ForEach(paymentInvoiceXmlModel => model.PaymentInvoices.Add(GenerateViewModel(paymentInvoiceXmlModel)));

            return model;

        }

        public static PaymentInvoiceViewModel GenerateViewModel(PaymentInvoiceXMLModel xmlModel)
        {
            PaymentInvoiceViewModel model = ReflectionHelper.CopyAToB(xmlModel, typeof(PaymentInvoiceViewModel), true);
            model.InvoiceType = GetSAPEnum(typeof(BoRcptInvTypes), xmlModel.InvoiceType);

            return model;
        }
        #endregion

        #region General Methods

        private static BoObjectTypes GetObjectType(ApplicationDocumentType applicationDocumentType, DocumentXMLModel xmlModel = null)
        {
            BoObjectTypes objectType = BoObjectTypes.oInvoices;

            switch (applicationDocumentType)
            {
                case ApplicationDocumentType.Invoice:
                case ApplicationDocumentType.DebitMemo:
                    objectType = BoObjectTypes.oInvoices;
                    break;

                case ApplicationDocumentType.CreditNote:
                    objectType = BoObjectTypes.oCreditNotes;
                    break;

                case ApplicationDocumentType.IncomingPayments:
                    objectType = BoObjectTypes.oIncomingPayments;
                    break;

                case ApplicationDocumentType.InventoryAjusted:
                    String objectTypeToSearch = xmlModel?.DocumentType;
                    switch (objectTypeToSearch)
                    {
                        case ConstantHelper.InventoryDocumentType.fo_GoodsIssue:
                            objectTypeToSearch = BoObjectTypes.oInventoryGenEntry.ToString();
                            break;
                        case ConstantHelper.InventoryDocumentType.fo_GoodsReceipt:
                            objectTypeToSearch = BoObjectTypes.oInventoryGenExit.ToString();
                            break;
                    }
                    objectType = GetSAPEnum(typeof(BoObjectTypes), objectTypeToSearch);
                    break;
            }

            return objectType;
        }

        private static BoDocumentSubType GetDocumentSubType(ApplicationDocumentType applicationDocumentType, DocumentXMLModel model)
        {
            BoDocumentSubType documentSubType = BoDocumentSubType.bod_None;

            if (applicationDocumentType == ApplicationDocumentType.Invoice && model.U_BPP_MDTD == ConstantHelper.DocType.Boleta)
                documentSubType = BoDocumentSubType.bod_Bill;

            else if (applicationDocumentType == ApplicationDocumentType.DebitMemo)
                documentSubType = BoDocumentSubType.bod_DebitMemo;

            return documentSubType;
        }

        private static dynamic GetSAPEnum(Type type, String valueToParse)
        {
            try
            {
                var response = Enum.Parse(type, valueToParse);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing: '" + valueToParse + "' to " + type.Name);
            }
        }

        #endregion
    }
}
