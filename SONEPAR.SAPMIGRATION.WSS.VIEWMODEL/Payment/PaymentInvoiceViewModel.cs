using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Payment
{
    public class PaymentInvoiceViewModel
    {
        public Int32 LineNum { get; set; }
        public Int32 DocEntry { get; set; }
        public Double SumApplied { get; set; }
        public BoRcptInvTypes InvoiceType { get; set; }
        public Int32 InstallmentId { get; set; }
        public String DistributionRule { get; set; }
        public String DistributionRule2 { get; set; }
        public String DistributionRule3 { get; set; }

        #region SAPProperties


        #endregion

        #region AplicationProperties

        public Boolean IsUpdate { get; set; }

        #endregion
    }
}
