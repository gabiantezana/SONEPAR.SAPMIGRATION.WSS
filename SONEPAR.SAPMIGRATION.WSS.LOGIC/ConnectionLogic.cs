using SONEPAR.SAPMIGRATION.WSS.DATAACCESS.SqlConnections;
using SONEPAR.SAPMIGRATION.WSS.MODEL;
using System;

namespace SONEPAR.SAPMIGRATION.WSS.LOGIC
{
    public class ConnectionLogic : CompanyLogic
    {
        public void SetCurrentDataBase(string _DestinationSapDb)
        {
            if (string.IsNullOrEmpty(_DestinationSapDb))
                throw new ArgumentNullException(nameof(_DestinationSapDb));
            CompanyLogic.SetCurrentCompany(_DestinationSapDb);
        }
        public DB_SAPMIGRATIONEntities GetMigrationDataContext() => ConnectionDataAccess.DB_SAPMIGRATIONEntities;

        public SAPModelEntities GetSapDataContext() => ConnectionDataAccess.GetEntity(CurrentCompanyName);
    }
}
