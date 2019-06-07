using SAPbobsCOM;
using SONEPAR.SAPMIGRATION.WSS.HELPER;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Document
{
    public class DocumentViewModel
    {
        public DocumentViewModel()
        {
            this.DocumentLines = new List<DocumentLineViewModel>();
        }

        public string InternalId { get; set; }//Para query de Neil

        #region XMLProperties

        public string Address2 { get; set; }
        public int? TransportationCode { get; set; }

        public string NumAtCard { get; set; }
        public string AgentCode { get; set; }

        public int? ContactPersonCode { get; set; }
        public Int32 DocEntry { get; set; }
        public String CardCode { get; set; }
        public String DocCurrency { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime DueDate { get; set; } //Error Paiva
        public DateTime TaxDate { get; set; }
        public int? PaymentGroupCode { get; set; }
        public String Comments { get; set; }
        public double? TotalDiscount { get; set; }
        public double? TotalDiscountFC { get; set; }

        public int? DocumentsOwner { get; set; }

        public String Project { get; set; }
        public int? SalesPersonCode { get; set; }
        public string Indicator { get; set; }
        public String ShipToCode { get; set; }

        #region UserFields

        public String U_RX_ORIREQ { get; set; }
        public String U_EXX_NRODEPDE { get; set; }

     
        #endregion

        public List<DocumentLineViewModel> DocumentLines { get; set; }


        #endregion


        #region SAPProperties 

        public BoObjectTypes DocObjectCode { get; set; }
        public BoObjectTypes ObjectType { get; set; }
        public BoDocumentTypes DocType { get; set; }
        public BoDocumentSubType DocumentSubType { get; set; }
        public Int32 Series { get; set; }
        public double? DiscountPercent { get; set; }

        #endregion

        #region ApplicationProperties

        public Boolean IsUpdate { get; set; }


        #endregion

    }




}
