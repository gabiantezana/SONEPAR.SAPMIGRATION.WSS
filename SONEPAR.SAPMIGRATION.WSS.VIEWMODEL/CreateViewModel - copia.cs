using SAPbobsCOM;
using SAPWS.EXCEPTION;
using SAPWS.HELPER;
using SAPWS.VIEWMODEL.BusinessPartner;
using SAPWS.VIEWMODEL.Document;
using SAPWS.VIEWMODEL.Payment;
using SAPWS.XMLMODEL.BusinessPartner;
using SAPWS.XMLMODEL.Document;
using SAPWS.XMLMODEL.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAPWS.VIEWMODEL
{
    public static class CreateViewModel
    {
        public static BusinessPartnerViewModel CreateBusinessPartnerViewModel(BusinessPartnerXMLModel xmlModel)
        {
            BusinessPartnerViewModel model = new BusinessPartnerViewModel();

            model.CardCode = xmlModel.CardCode;
            model.CardName = xmlModel.CardName;
            model.CardType = (BoCardTypes)ParseStringToEnum(typeof(BoCardTypes), xmlModel.CardType);
            model.Currency = xmlModel.Currency;
            model.FederalTaxID = xmlModel.FederalTaxID;
            model.GroupCode = xmlModel.GroupCode;
            model.PayTermsGrpCode = xmlModel.PayTermsGrpCode;
            model.U_BPP_BPTD = xmlModel.U_BPP_BPTD;
            model.U_BPP_BPTP = xmlModel.U_BPP_BPTP;

            foreach (var item in xmlModel.BPAddresses)
            {
                BPAddressViewModel bpAddressModel = new BPAddressViewModel();
                bpAddressModel.AddressName = item.AddressName;
                bpAddressModel.AddressType = (BoAddressType)ParseStringToEnum(typeof(BoAddressType), item.AddressType);
                bpAddressModel.Block = item.Block;
                bpAddressModel.City = item.City;
                bpAddressModel.Country = item.Country;
                bpAddressModel.State = item.State;
                bpAddressModel.Street = item.Street;

                model.BPAddresses.Add(bpAddressModel);
            }
            foreach (var item in xmlModel.ContactEmployees)
            {
                ContactEmployeeViewModel contactEmployeeModel = new ContactEmployeeViewModel();
                contactEmployeeModel.E_Mail = item.E_Mail;
                contactEmployeeModel.Gender = (BoGenderTypes)ParseStringToEnum(typeof(BoGenderTypes), item.Gender);
                contactEmployeeModel.MobilePhone = item.MobilePhone;
                contactEmployeeModel.Name = item.Name;
                contactEmployeeModel.Phone1 = item.Phone1;
                contactEmployeeModel.Phone2 = item.Phone2;

                model.ContactEmployees.Add(contactEmployeeModel);
            }

            return model;
        }

        public static DocumentViewModel CreateDocumentViewModel(ApplicationDocumentType applicationDocumentType, DocumentXMLModel xmlModel)
        {
            try
            {
                DocumentViewModel model = new DocumentViewModel();
                model = (DocumentViewModel)ReflectionHelper.CopyAToB(xmlModel, model);

                DocumentViewModel model = new DocumentViewModel();
                {
                    model.ObjectType = GetObjectType(applicationDocumentType);
                    model.DocumentSubType = GetDocumentSubType(applicationDocumentType, xmlModel);

                    model.CardCode = xmlModel.CardCode;
                    model.Comments = xmlModel.Comments;
                    model.DocCurrency = xmlModel.DocCurrency;
                    model.DocDate = ConvertHelper.ToDate(xmlModel.DocDate);
                    model.DocDueDate = ConvertHelper.ToDate(xmlModel.DocDueDate);
                    model.TaxDate = ConvertHelper.ToDate(xmlModel.TaxDate);
                    model.DocDueDate = ConvertHelper.ToDate(xmlModel.DocDueDate);

                    if (!String.IsNullOrEmpty(xmlModel.U_BPP_SDocDate))
                        model.U_BPP_SDocDate = ConvertHelper.ToDate(xmlModel.U_BPP_SDocDate);

                    model.DocEntry = xmlModel.DocEntry;
                    model.PaymentGroupCode = xmlModel.PaymentGroupCode;
                    model.TotalDiscount = xmlModel.TotalDiscount;
                    model.TotalDiscountFC = xmlModel.TotalDiscountFC;
                    model.U_BPP_MDCD = xmlModel.U_BPP_MDCD;
                    model.U_BPP_MDCO = xmlModel.U_BPP_MDCO;
                    model.U_BPP_MDSD = xmlModel.U_BPP_MDSD;
                    model.U_BPP_MDSO = xmlModel.U_BPP_MDSO;
                    model.U_BPP_MDTD = xmlModel.U_BPP_MDTD;
                    model.U_BPP_MDTO = xmlModel.U_BPP_MDTO;
                    model.U_BPP_MDTS = xmlModel.U_BPP_MDTS;
                    model.U_MSS_TIDA = xmlModel.U_MSS_TIDA;
                    model.U_MSS_TVTA = xmlModel.U_MSS_TVTA;

                    foreach (var item in xmlModel.DocumentLines)
                    {
                        DocumentLineViewModel documentLine = new DocumentLineViewModel();
                        documentLine.BaseEntry = item.BaseEntry;
                        documentLine.BaseLine = item.BaseLine;
                        documentLine.BaseType = item.BaseType;
                        documentLine.CostingCode = item.CostingCode;
                        documentLine.CostingCode2 = item.CostingCode2;
                        documentLine.CostingCode3 = item.CostingCode3;
                        documentLine.ItemCode = item.ItemCode;
                        documentLine.LineNum = item.LineNum;
                        documentLine.Price = item.Price;
                        documentLine.Quantity = item.Quantity;
                        documentLine.TaxCode = item.TaxCode;
                        documentLine.UnitPrice = item.UnitPrice;
                        documentLine.WarehouseCode = item.WarehouseCode;

                        documentLine.DocEntry = item.DocEntry;
                        model.DocumentLines.Add(documentLine);
                    }
                }

                typeof(DocumentViewModel).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(y => y.PropertyType == typeof(String)).ToList().ForEach(z => z.SetValue(model, z.GetValue(model, null) ?? String.Empty));

                return model;
            }
            catch (Exception ex)
            {
                throw new CustomException("Error parsing xml data: " + ex.Message);
            }
        }

        public static PaymentViewModel CreatePaymentViewModel(ApplicationDocumentType applicationDocumentType, PaymentXMLModel xmlModel)
        {
            PaymentViewModel model = new PaymentViewModel();
            model.DocType = (BoRcptTypes)ParseStringToEnum(typeof(BoRcptTypes), xmlModel.DocType);
            model.DocDate = ConvertHelper.ToDate(xmlModel.DocDate);
            model.TaxDate = ConvertHelper.ToDate(xmlModel.TaxDate);
            model.DueDate = ConvertHelper.ToDate(xmlModel.DueDate);
            model.CardCode = xmlModel.CardCode;
            model.DocCurrency = xmlModel.DocCurrency;
            model.DocRate = xmlModel.DocRate;
            model.CounterReference = xmlModel.CounterReference;
            model.Remarks = xmlModel.Remarks;
            model.CashSum = xmlModel.CashSum;

            foreach (PaymentInvoiceXMLModel item in xmlModel.PaymentInvoices)
            {
                PaymentInvoiceViewModel paymentInvoice = new PaymentInvoiceViewModel();
                paymentInvoice.DocEntry = item.DocEntry;
                paymentInvoice.SumApplied = item.SumApplied;
                paymentInvoice.InvoiceType = item.InvoiceType;
                paymentInvoice.InstallmentId = item.InstallmentId;
                paymentInvoice.DistributionRule = item.DistributionRule;
                paymentInvoice.DistributionRule2 = item.DistributionRule2;
                paymentInvoice.DistributionRule3 = item.DistributionRule3;
            }

            return model;

        }

        private static BoObjectTypes GetObjectType(ApplicationDocumentType applicationDocumentType)
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

        private static object ParseStringToEnum(Type type, String valueToParse)
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
    }
}
