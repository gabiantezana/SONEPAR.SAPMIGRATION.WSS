using SAPbobsCOM;
using SONEPAR.SAPMIGRATION.WSS.EXCEPTION;
using SONEPAR.SAPMIGRATION.WSS.HELPER;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.BusinessPartner;
using System;
using System.Reflection;

namespace SONEPAR.SAPMIGRATION.WSS.DATAACCESS
{
    public class BusinessPartnerDataAccess
    {
        Boolean save = false;

        public void AddUpdateBusinessPartner(Company company, BusinessPartnerViewModel model)
        {
            BusinessPartners businessPartner = company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
            if (model.IsUpdate)
                businessPartner.GetByKey(model.CardCode);

            businessPartner.CardCode = model.CardCode;
            businessPartner.CardName = model.CardName;
            businessPartner.CardType = model.CardType;
            businessPartner.FederalTaxID = model.FederalTaxID;
            businessPartner.GroupCode = model.GroupCode;
            businessPartner.Currency = model.Currency;
            businessPartner.PayTermsGrpCode = model.PayTermsGrpCode;

            model.ContactEmployees.ForEach(x => AddUpdateContactEmployee(x, ref businessPartner));
            model.BPAddresses.ForEach(x => AddUpdateBPAddress(x, ref businessPartner));

            SetUserFields(model, ref businessPartner);

            if (model.IsUpdate)
                save = businessPartner.Update() == ConstantHelper.SuccessSaveSap;
            else
                save = businessPartner.Add() == ConstantHelper.SuccessSaveSap;

            if (!save)
                throw new SapException();

        }

        private void AddUpdateContactEmployee(ContactEmployeeViewModel model, ref BusinessPartners businessPartner)
        {
            if (model.IsUpdate)
                businessPartner.ContactEmployees.SetCurrentLine(model.LineNum.Value);

            businessPartner.ContactEmployees.Name = model.Name;
            businessPartner.ContactEmployees.Phone1 = model.Phone1;
            businessPartner.ContactEmployees.Phone2 = model.Phone2;
            businessPartner.ContactEmployees.MobilePhone = model.MobilePhone;
            businessPartner.ContactEmployees.E_Mail = model.E_Mail;
            businessPartner.ContactEmployees.Gender = model.Gender;

            if (!model.IsUpdate)
                businessPartner.ContactEmployees.Add();

        }

        private void AddUpdateBPAddress(BPAddressViewModel model, ref BusinessPartners businessPartner)
        {
            if (model.IsUpdate)
                businessPartner.Addresses.SetCurrentLine(model.LineNum.Value);

            businessPartner.Addresses.AddressType = model.AddressType;
            businessPartner.Addresses.AddressName = model.AddressName;
            businessPartner.Addresses.Street = model.Street;
            businessPartner.Addresses.Country = model.Country;
            businessPartner.Addresses.State = model.State;
            businessPartner.Addresses.City = model.City;
            businessPartner.Addresses.Block = model.Block;

            if (!model.IsUpdate)
                businessPartner.Addresses.Add();
        }

        private void SetUserFields(BusinessPartnerViewModel model, ref BusinessPartners businessPartner)
        {
            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                if (property.Name.StartsWith(ConstantHelper.UserFieldNameStarsWith))
                    businessPartner.UserFields.Fields.Item(property.Name).Value = property.GetValue(model, null);
            }
        }

        public Boolean ExistBusinessPartner(Company company, String cardCode, Boolean throwIfNull = false)
        {
            Boolean exists = false;
            BusinessPartners businessPartner = company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
            exists = businessPartner.GetByKey(cardCode);
            if (!exists && throwIfNull)
                throw new SapException("Business Partner does not exist. CardCode: " + cardCode);
            return exists;

        }

        public Int32? GetBPAddressLineNum(Company company, String cardCode, String addressName)
        {
            BusinessPartners businessPartner = company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
            businessPartner.GetByKey(cardCode);
            BPAddresses bpAdresses = businessPartner.Addresses;

            if (bpAdresses != null)
                if (bpAdresses.Count > 0)
                    for (int i = 0; i < bpAdresses.Count; i++)
                    {
                        bpAdresses.SetCurrentLine(i);
                        if (bpAdresses.AddressName.Equals(addressName))
                            return i;
                    }
            return null;
        }

        public Int32? GetContactEmployeeLineNum(Company company, String cardCode, String contactEmployeeName)
        {
            BusinessPartners businessPartner = company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
            businessPartner.GetByKey(cardCode);
            ContactEmployees BusinessPartnerContactEmployees = businessPartner.ContactEmployees;

            if (BusinessPartnerContactEmployees != null)
                if (BusinessPartnerContactEmployees.Count > 0)
                    for (int i = 0; i < BusinessPartnerContactEmployees.Count; i++)
                    {
                        BusinessPartnerContactEmployees.SetCurrentLine(i);
                        if (BusinessPartnerContactEmployees.Name.Equals(contactEmployeeName))
                            return i;
                    }
            return null;
        }

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
    }
}
