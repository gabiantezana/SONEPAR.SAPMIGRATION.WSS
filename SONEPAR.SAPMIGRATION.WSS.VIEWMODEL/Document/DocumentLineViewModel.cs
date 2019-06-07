using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Document
{
    public class DocumentLineViewModel
    {

        public Int32 LineNum { get; set; }
        public String ItemCode { get; set; }
        public Int32 Quantity { get; set; }

        public int? SalesPersonCode { get; set; }
        public Double UnitPrice { get; set; }
        public Double Price { get; set; }
        public String TaxCode { get; set; }
        public String WarehouseCode { get; set; }
     
        public Double DisccountPercent { get; set; }
        public Double LineTotal { get; set; }
        public string Currency { get; set; }
        public string ProjectCode { get; set; }
        public string ShipToDescription { get; set; }

        #region ApplicationProperties

        public Boolean IsUpdate { get; set; }

        #endregion

        #region Userfields

        public string U_EXX_FE_TPVU { get; set; }
        public string U_EXX_FE_TAIGV { get; set; }
        public string U_RX_MARCA { get; set; }
        public double U_RX_STKDIS { get; set; }
        public double U_RX_STKRSV { get; set; }
        public double U_RX_STOKCD { get; set; }
        public double U_RX_PRIADD { get; set; }
        public double U_RX_DSCADD { get; set; }
        public string U_RX_ACCPED { get; set; }

        #endregion

    }
}
