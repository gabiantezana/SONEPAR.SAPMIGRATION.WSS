using SAPbobsCOM;
using SAPWS.DATAACCESS;
using SAPWS.EXCEPTION;
using SAPWS.HELPER;
using SAPWS.VIEWMODEL.BusinessPartner;
using SAPWS.XMLMODEL.BusinessPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SAPWS.VIEWMODEL;

namespace SAPWS.LOGIC
{
    public class BusinessPartnerLogic : ConnectionLogic
    {
        public void AddUpdateBusinessPartner(Company company, string xml)
        {
            BusinessPartnerViewModel model = CreateViewModel.GenerateViewModel(SerializeHelper.XMLToObject(xml, typeof(BusinessPartnerXMLModel)));

            FillBusinessPartnerFields(company, ref model);
            model.BPAddresses.ForEach(x => FillBusinessPartnerFields(company, model, ref x));
            model.ContactEmployees.ForEach(x => FillBusinessPartnerFields(company, model, ref x));

            new BusinessPartnerDataAccess().AddUpdateBusinessPartner(company, model);
        }

        private void FillBusinessPartnerFields(Company company, ref BusinessPartnerViewModel model)
        {
            model.IsUpdate = new BusinessPartnerDataAccess().ExistBusinessPartner(company, model.CardCode);
        }

        private void FillBusinessPartnerFields(Company company, BusinessPartnerViewModel parentModel, ref BPAddressViewModel model)
        {
            model.LineNum = new BusinessPartnerDataAccess().GetBPAddressLineNum(company, parentModel.CardCode, model.AddressName);
            if (model.LineNum.HasValue)
                model.IsUpdate = true;
        }

        private void FillBusinessPartnerFields(Company company, BusinessPartnerViewModel parentModel, ref ContactEmployeeViewModel model)
        {
            model.LineNum = new BusinessPartnerDataAccess().GetContactEmployeeLineNum(company, parentModel.CardCode, model.Name);
            if (model.LineNum.HasValue)
                model.IsUpdate = true;
        }
    }

}
