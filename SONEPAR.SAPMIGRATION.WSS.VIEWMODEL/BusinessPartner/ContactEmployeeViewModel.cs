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
    [XmlType("ContactEmployee")]
    public class ContactEmployeeViewModel
    {
        public Int32? LineNum { get; set; }

        public String Name { get; set; }

        public String Phone1 { get; set; }

        public String Phone2 { get; set; }

        public String MobilePhone { get; set; }

        public String E_Mail { get; set; }

        public BoGenderTypes Gender { get; set; }


        #region AplicationProperties

        [XmlIgnore]
        public Boolean IsUpdate { get; set; }

        #endregion
    }
}
