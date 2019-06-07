using SONEPAR.SAPMIGRATION.WSS.EXCEPTION;
using SONEPAR.SAPMIGRATION.WSS.HELPER;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Document;
using System;
using System.Linq;

namespace SONEPAR.SAPMIGRATION.WSS.LOGIC
{
    public class ApplicationLogic
    {
        public void Initizalize()
        {
            new ConnectionLogic().InitializeConnections();
        }

        public void RunProccess()
        {
            MigrateObjects();
            SendPendingMails();
        }

        private void MigrateObjects()
        {
            var applicationLog = new ApplicationLog();
            try
            {
                new SalesOrderLogic().Process();
                new SalesQuotationLogic().Process();
            }
            catch (Exception ex)
            {
                applicationLog.FinishDate = DateTime.Now;
                throw;
            }
        }
        private void SendPendingMails(bool regenerateMailBody = false)
        {

        }
    }
}
