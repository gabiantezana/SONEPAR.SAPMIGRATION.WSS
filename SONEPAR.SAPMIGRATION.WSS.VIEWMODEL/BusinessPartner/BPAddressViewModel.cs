using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.BusinessPartner
{
    [XmlType("BPAddress")]
    public class BPAddressViewModel
    {
        public Int32? LineNum { get; set; }

        public String AddressName { get; set; }

        public String Street { get; set; }

        public String Country { get; set; }

        public String State { get; set; }

        public String City { get; set; }

        public String Block { get; set; }


        #region SAPProperties

        public BoAddressType AddressType { get; set; }

        #endregion

        #region AplicationProperties

        public Boolean IsUpdate { get; set; }

        #endregion

    }
}
