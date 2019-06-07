using SONEPAR.SAPMIGRATION.WSS.MODEL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Linq;

namespace SONEPAR.SAPMIGRATION.WSS.DATAACCESS.SqlConnections
{
    public class ConnectionDataAccess
    {
        public static List<SAPModelEntities> SAPModelEntities { get; set; } = new List<SAPModelEntities>();
        public static DB_SAPMIGRATIONEntities DB_SAPMIGRATIONEntities { get; set; } = new DB_SAPMIGRATIONEntities();
        public static SAPModelEntities GetEntity(string dataBaseName)
        {
            return SAPModelEntities.FirstOrDefault(x => x.Database.Connection.Database == dataBaseName) ?? throw new Exception("No se encontró una conexión para la base de datos especificada");
        }
        public static void AddNewEntity(string connectionstring)
        {
            var entity = new SAPModelEntities();
            //var connection = new SqlConnectionStringBuilder(entity.Database.Connection.ConnectionString);
            //connection.InitialCatalog = "YOUR_PREFIX_FROMSOMEWHERE";
            var connectionString = string.Empty;
            try { connectionString = new EntityConnectionStringBuilder(connectionstring).ProviderConnectionString; }
            catch { }

            if (!string.IsNullOrEmpty(connectionString))
            {
                entity.Database.Connection.ConnectionString = connectionString;
                SAPModelEntities.Add(entity);
            }
        }
    }
}
