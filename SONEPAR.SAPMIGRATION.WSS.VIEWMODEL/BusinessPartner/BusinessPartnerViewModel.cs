using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.BusinessPartner
{
    public class BusinessPartnerViewModel
    {
        public BusinessPartnerViewModel()
        {
            this.ContactEmployees = new List<ContactEmployeeViewModel>();
            this.BPAddresses = new List<BPAddressViewModel>();
        }
        public String CardCode { get; set; }
        public String CardName { get; set; }
        public BoCardTypes CardType { get; set; }
        public String FederalTaxID { get; set; }
        public Int32 GroupCode { get; set; }
        public String Currency { get; set; }
        public String U_BPP_BPTP { get; set; }
        public String U_BPP_BPTD { get; set; }
        public Int32 PayTermsGrpCode { get; set; }
        public List<ContactEmployeeViewModel> ContactEmployees { get; set; }
        public List<BPAddressViewModel> BPAddresses { get; set; }

        #region AplicationProperties

        public Boolean IsUpdate { get; set; }

        #endregion
    }






}
