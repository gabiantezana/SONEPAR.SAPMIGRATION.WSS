using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SONEPAR.SAPMIGRATION.WSS.HELPER
{
    public class ConstantHelper
    {
        public static class MailConstant
        {
            public static string Subject = "TEST ENVÍO OC";
            public static string PathImagesResources = @"Resources/Images/";
        }

        public static string LOG_DATE_FORMAT = "dd/MM/yyyy hh:mm:ss";

        public const Int32 SuccessSaveSap = 0;

        public static class APPCONFIGKEY
        {
            public const string SmtpHost = "SmtpHost";
            public const string SmtpPort = "SmtpPort";
            public const string mailFromAddress = "mailFromAddress";
            public const string mailFromPassword = "mailFromPassword";
            public const string mailFromName = "mailFromName";
            public const string EnableSSL = "EnableSSL";
        }

        #region Parameter
        public static class Parameter
        {
            public static class Connection
            {
                public static string CompanyServer = "CompanyServer";
                public static string LicenseServer = "LicenseServer";
                public static string DBServerType = "DBServerType";
                public static string DBUserName = "DBUserName";
                public static string DBUserPass = "DBUserPass";
                public static string CompanyDB = "CompanyDB";
                public static string B1UserName = "B1UserName";
                public static string B1UserPass = "B1UserPass";
                public static string languaje = "languaje";
            }
        }

        public static string ParametersName = "name";
        public static string ParameterValue = "value";
        public static string ParameterPath = "../Parameters/ConnectionParameters.json";

        #endregion

        public const string UserFieldNameStarsWith = "U_";
        public const string DateFormat = "yyyyMMdd";

        public static class Moneda
        {
            public const string USD = "USD";
            public const string SOL = "SOL";
        }

        public static class DocType
        {
            public const string Factura = "01";
            public const string Boleta = "03";
        }


        public static string XMLConnectionFileName = "//ConnectionParameters//ConnectionParameters.xml";

        public static int INT_EMPTY = -1;

        public static class InventoryDocumentType
        {
            public const string fo_GoodsReceipt = "fo_GoodsReceipt";
            public const string fo_GoodsIssue = "fo_GoodsIssue";
        }

        public static string SuccessMessage = "Successful operation.";

        public static class MetodoPago
        {
            public const int Efectivo = -1;
        }
    }
}
