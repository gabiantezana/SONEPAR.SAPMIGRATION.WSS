using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Company
{
    public class RootObject
    {
        public List<CompanyViewModel> Companies { get; set; }
    }
    public class CompanyViewModel
    {
        public Boolean? XMLAsString { get; set; }
        public String Server { get; set; }
        public String LicenseServer { get; set; }
        public BoDataServerTypes DbServerType { get; set; }
        public String DbUserName { get; set; }
        public String DbPassword { get; set; }
        public String CompanyDB { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public BoSuppLangs language { get; set; }
        public Boolean UseTrusted { get; set; }
        public Boolean Connected { get; set; }

        public CompanyViewModel()
        {
            XMLAsString = null;
            LicenseServer = String.Empty;
            DbUserName = String.Empty;
            DbPassword = String.Empty;
            CompanyDB = String.Empty;
            UserName = String.Empty;
            Password = String.Empty;
        }
        /*
        oCompany.XMLAsString = true;
        oCompany.Server = System.Configuration.ConfigurationSettings.AppSettings["CompanyServer"];
        oCompany.LicenseServer = System.Configuration.ConfigurationSettings.AppSettings["LicenseServer"];
        oCompany.DbServerType = getBoDataServerType(System.Configuration.ConfigurationSettings.AppSettings["DBServerType"]);
        oCompany.DbUserName = System.Configuration.ConfigurationSettings.AppSettings["DBUserName"];
        oCompany.DbPassword = System.Configuration.ConfigurationSettings.AppSettings["DBUserPass"];
        oCompany.CompanyDB = System.Configuration.ConfigurationSettings.AppSettings["CompanyDB"];
        oCompany.UserName = System.Configuration.ConfigurationSettings.AppSettings["B1UserName"];
        oCompany.Password = System.Configuration.ConfigurationSettings.AppSettings["B1UserPass"];
        oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English;
        oCompany.UseTrusted = false;*/

    }
}
