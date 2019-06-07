using SAPbobsCOM;
using SONEPAR.SAPMIGRATION.WSS.HELPER;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SONEPAR.SAPMIGRATION.WSS.EXCEPTION;

namespace SONEPAR.SAPMIGRATION.WSS.DATAACCESS
{
    public static class CompanyDataAccess
    {
        public static List<Company> Companies { get; set; } = new List<Company>();
        public static Company GetCommpany(string companyName)
        {
            Connect(companyName);
            return Companies.FirstOrDefault(x => x.CompanyDB == companyName);
        }

        public static void AddNewCompany(CompanyViewModel model, string companyName)
        {
            if (Companies.FirstOrDefault(x => x.CompanyDB == companyName) != null) return;

            var company = CreateNewCompanyFromModel(model);
            Companies.Add(company);

            if (!company.Connected)
                Connect(companyName);
        }
        private static Company CreateNewCompanyFromModel(CompanyViewModel model)
        {
            Company _Company = new Company
            {
                DbServerType = model.DbServerType,
                Server = model.Server,
                UseTrusted = false,
                DbUserName = model.DbUserName,
                DbPassword = model.DbPassword,
                CompanyDB = model.CompanyDB,
                UserName = model.UserName,
                Password = model.Password,
                LicenseServer = model.LicenseServer,
            };
            return _Company;
        }

        public static void Disconnect(string companyName)
        {
            try
            {
                var company = GetCommpany(companyName);
                if (company != null)
                {
                    if (company.Connected)
                        company.Disconnect();
                    company = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Connect(string companyName)
        {
            var company = Companies.FirstOrDefault(x => x.CompanyDB == companyName);
            if (company.Connected)
                return;
            int resultReturn = company.Connect();
            if (resultReturn != 0)
                throw new Exception();//TODO: Implement message
        }

        public static SapExceptionEntity GetLastSapError(string companyName)
        {
            var company = GetCommpany(companyName);
            int errorCode = default;
            String errorMessage = string.Empty;
            if (company != null)
            {
                errorCode = company.GetLastErrorCode();
                errorMessage = company.GetLastErrorDescription();
            }
            return new SapExceptionEntity { errorCode = errorCode, errorMessage = errorMessage };
        }

        public static string GetLastDocEntry(string companyName)
        {
            var company = GetCommpany(companyName);
            string sNewObjCode = String.Empty;
            try
            {
                company.GetNewObjectCode(out sNewObjCode);
                return sNewObjCode;
            }
            catch (Exception ex)
            {
                sNewObjCode = "Error in GetDocEntry()";
            }
            return sNewObjCode;
        }

    }
}
