using SAPbobsCOM;
using SONEPAR.SAPMIGRATION.WSS.EXCEPTION;
using SONEPAR.SAPMIGRATION.WSS.HELPER;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SONEPAR.SAPMIGRATION.WSS.DATAACCESS
{
    public class PaymentDataAccess
    {
        private Boolean Save { get; set; }

        public void AddUpdatePayment(Company company, PaymentViewModel model)
        {
            Payments payment = company.GetBusinessObject(BoObjectTypes.oIncomingPayments);

            if (model.IsUpdate)
                payment.GetByKey(model.DocEntry);

            payment.DocType = model.DocType;
            payment.DocDate = model.DocDate;
            payment.DueDate = model.DueDate;
            payment.TaxDate = model.TaxDate;
            payment.DocRate = model.DocRate;
            payment.CardCode = model.CardCode;
            payment.DocCurrency = model.DocCurrency;
            payment.CounterReference = model.CounterReference;
            payment.Remarks = model.Remarks;
            payment.CashSum = model.CashSum;
            payment.CashAccount = model.CashAccount;
            payment.BankChargeAmount = model.BankChargeAmount;

            SetUserFields(model, ref payment);

            model.PaymentInvoices.ForEach(paymentInvoice => AddUpdatePaymentInvoice(paymentInvoice, ref payment));

            if (model.IsUpdate)
                Save = payment.Update() == ConstantHelper.SuccessSaveSap;
            else
                Save = payment.Add() == ConstantHelper.SuccessSaveSap;
            if (!Save)
                throw new SapException();

        }

        private void AddUpdatePaymentInvoice(PaymentInvoiceViewModel model, ref Payments payment)
        {
            payment.Invoices.DocEntry = model.DocEntry;
            payment.Invoices.SumApplied = model.SumApplied;
            payment.Invoices.InvoiceType = model.InvoiceType;
            payment.Invoices.InstallmentId = model.InstallmentId;
            payment.Invoices.DistributionRule = model.DistributionRule;
            payment.Invoices.DistributionRule2 = model.DistributionRule2;
            payment.Invoices.DistributionRule3 = model.DistributionRule3;
        }

        private void SetUserFields(PaymentViewModel model, ref Payments payment)
        {
            foreach (PropertyInfo property in ReflectionHelper.GetSAPUserFieldsProperties(model))
            {
                try { payment.UserFields.Fields.Item(property.Name).Value = property.GetValue(model, null); }
                catch (Exception ex) { ExceptionHelper.LogException(ex); }
            }
        }

        private void SetUserFields(PaymentInvoiceViewModel model, ref Payments payment)
        {
            foreach (PropertyInfo property in ReflectionHelper.GetSAPUserFieldsProperties(model))
            {
                try { payment.Invoices.UserFields.Fields.Item(property.Name).Value = property.GetValue(model, null); }
                catch (Exception ex) { ExceptionHelper.LogException(ex); }
            }
        }

        #region Queries

        private Fields ExecuteRecordSet(Company company, String query, Boolean throwIfNull = false)
        {
            Fields response = null;
            try
            {
                Recordset recordSet = company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordSet.DoQuery(query);
                response = recordSet.Fields;
                return response;
            }
            catch (Exception ex)
            {
                if (throwIfNull)
                    throw ex;
                return response;
            }
            finally
            {
                //TODOG: CLEAN OBJECT
            }
        }

        #endregion


    }
}
