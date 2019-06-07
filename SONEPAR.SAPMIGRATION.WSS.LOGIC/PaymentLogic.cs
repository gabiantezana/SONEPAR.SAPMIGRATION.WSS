using SAPbobsCOM;
using SAPWS.DATAACCESS;
using SAPWS.EXCEPTION;
using SAPWS.HELPER;
using SAPWS.VIEWMODEL;
using SAPWS.VIEWMODEL.Payment;
using SAPWS.XMLMODEL.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPWS.LOGIC
{
    public class PaymentLogic : ConnectionLogic
    {
        public void AddUpdatePayment(Company company, ApplicationDocumentType documentType, String xml)
        {
            PaymentViewModel model = CreateViewModel.GenerateViewModel(SerializeHelper.XMLToObject(xml, typeof(PaymentXMLModel)));

            SetDocumentProperties(company, ref model);
            model.PaymentInvoices.ForEach(paymentInvoice => SetDocumentProperties(company, model, ref paymentInvoice));

            new PaymentDataAccess().AddUpdatePayment(company, model);
        }

        public void SetDocumentProperties(Company company, ref PaymentViewModel model)
        {
            SetDocumentProperties_CashAccount(company, ref model);
        }

        public void SetDocumentProperties(Company company, PaymentViewModel parentModel, ref PaymentInvoiceViewModel model)
        {

        }

        public void SetDocumentProperties_CashAccount(Company company, ref PaymentViewModel model)
        {
            model.CashAccount = GetCashAccount(company, model, true);
        }

        public String GetCashAccount(Company company, PaymentViewModel model, Boolean throwIfNull)
        {
            String cashAccount = String.Empty;
            var response = new PaymentDataAccess().GetCashAccount(company, model.U_MSS_MPSA, model.DocCurrency);
            if (response != null)
                if (response.Count > 0)
                    if (response.Item(0) != null)
                        if (!String.IsNullOrEmpty(response.Item(0).Value))
                            return response.Item(0).Value;
            if (throwIfNull)
                throw new CustomException("Cash account for payment does not found in database.");
            else
                return cashAccount;
        }


    }
}
