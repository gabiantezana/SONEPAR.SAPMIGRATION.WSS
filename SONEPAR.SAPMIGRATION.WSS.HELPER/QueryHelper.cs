using System;
using SAPbobsCOM;
using YAMBOLY.GESTIONRUTAS.HELPER;

namespace SONEPAR.SAPMIGRATION.WSS.HELPER
{
    public class QueryConstructorHelper
    {
        #region  InternalMethods

        private BoDataServerTypes dbSAPServerType { get; set; }
        public QueryConstructorHelper(BoDataServerTypes dbServerType)
        {
            this.dbSAPServerType = dbServerType;
        }
        private String SQLQuery;
        private String HanaQuery;

        #endregion

        #region Queries


        #endregion

        public  string GetStringContent(EmbebbedFileName embebbedFileName)
        {
            var query = string.Empty;
            var fileString = XMLHelper.GetXMLString(System.Reflection.Assembly.GetCallingAssembly(), embebbedFileName);
            return fileString;
        }
    }
}
