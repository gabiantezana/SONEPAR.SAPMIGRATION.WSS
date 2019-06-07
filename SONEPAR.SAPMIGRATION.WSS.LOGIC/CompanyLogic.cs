using SONEPAR.SAPMIGRATION.WSS.DATAACCESS;
using SONEPAR.SAPMIGRATION.WSS.HELPER;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Company;
using System;
using SAPbobsCOM;
using SONEPAR.SAPMIGRATION.WSS.EXCEPTION;
using System.Configuration;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using SONEPAR.SAPMIGRATION.WSS.DATAACCESS.SqlConnections;

namespace SONEPAR.SAPMIGRATION.WSS.LOGIC
{
    public class CompanyLogic
    {
        public static string CurrentCompanyName { get; set; }
        //TODO: VALIDAR ORDENES DUPLICADAS
        public static void SetCurrentCompany(string _DestinationSapDb)
        {
            if (string.IsNullOrEmpty(_DestinationSapDb))
                throw new ArgumentNullException(nameof(_DestinationSapDb));
            CurrentCompanyName = _DestinationSapDb;
        }

        public void ClearCurrentCompany()
        {
            CurrentCompanyName = null;
        }
        public void InitializeConnections()
        {
            var models = GetCompanyViewModelsFromFile();
            models.ForEach(x => CompanyDataAccess.AddNewCompany(x, x.CompanyDB));

            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            if (connections.Count != 0)
                foreach (ConnectionStringSettings connection in connections)
                    ConnectionDataAccess.AddNewEntity(connection.ConnectionString);
        }

        private List<CompanyViewModel> GetCompanyViewModelsFromFile()
        {
            var jsonString = System.IO.File.ReadAllText(ConstantHelper.ParameterPath);
            var companiesModel = JsonConvert.DeserializeObject<RootObject>(jsonString);
            //CompanyViewModel model = CreateViewModel.GenerateViewModel(SerializeHelper.XMLToObject(xml, typeof(CompanyXMLModel)));
            return companiesModel.Companies.ToList();
        }


        public Company GetCompany()
        {
            return CompanyDataAccess.GetCommpany(CurrentCompanyName);
        }

        public static void Disconnect()
        {
            CompanyDataAccess.Disconnect(CurrentCompanyName);
        }

        public static SapExceptionEntity GetLastSapError()
        {
            return CompanyDataAccess.GetLastSapError(CurrentCompanyName);
        }

        public int GetLastDocEntry()
        {
            var result = CompanyDataAccess.GetLastDocEntry(CurrentCompanyName);
            return (result == null) ? default : Convert.ToInt32(result);
        }


        public string GetLastSapErrorString()
        {
            return CompanyDataAccess.GetLastSapError(CurrentCompanyName).errorCode + "- " + CompanyDataAccess.GetLastSapError(CurrentCompanyName).errorMessage;
        }
    }
}
