using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SONEPAR.SAPMIGRATION.WSS.HELPER
{
    public class EntityHelper
    {

    }
    public class ApplicationLog
    {
        public ApplicationLog()
        {
            StartDate = DateTime.Now;
            ItemDetail = new List<ItemLog>();
        }

        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public Int32 ItemCount { get; set; }
        public Int32 SuccessCount { get; set; }
        public Int32 ErrorCount { get; set; }
        public List<ItemLog> ItemDetail { get; set; }
    }


    public class DetailLog
    {
        public DetailLog()
        {
            ItemLog = new List<ItemLog>();
        }

        public Int32 Count { get; set; }
        public List<ItemLog> ItemLog { get; set; }
    }

    public class ItemLog
    {
        public int DocEntry { get; set; }
        //public ObjectType ObjectType { get; set; }
        public int ObjectType { get; set; }
        public State State { get; set; }
        public ApplicationErrorType ErrorType { get; set; }
        public String Message { get; set; }
    }

    public class EmailConfigurationEntity
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string FromAddress { get; set; }
        public string FromPassword { get; set; }
        public string FromName { get; set; }
    }

    public class MailModel
    {
        public string SucursalNombre { get; set; }
        public string NombreCliente { get; set; }

        public string CorreoCliente { get; set; }
        public string DireccionDeEntrega { get; set; }
        public string NumeroDocumentoCliente { get; set; }
        public string NumeroDocumentoSAP { get; set; }
        public string Moneda { get; set; }
        public string MontoSubTotal { get; set; }
        public string MontoIGV { get; set; }
        public string MontoTotal { get; set; }
        public string GestorNombre { get; set; }
        public string GestorTelefono { get; set; }
        public string GestorCorreo { get; set; }
        public List<MailDocumentDetail> DetalleDocumentoList { get; set; } = new List<MailDocumentDetail>();
    }

    public class MailDocumentDetail
    {
        public string Item { get; set; }
        public string Referencia { get; set; }
        public string Descripcion { get; set; }
        public string UM { get; set; }
        public string FechaEntrega { get; set; }
        public string Direccion { get; set; }
        public string Categoria { get; set; }
        public string Cantidad { get; set; }
        public string Precio { get; set; }
        public string STotal { get; set; }
    }

    #region ApplicationResponse
    public class ApplicationResponse
    {
        public ResponseStatus ResponseStatus { get; set; }
        public ResponseType ResponseType { get; set; }
        public Int32 ResponseCount { get; set; }
        public Response Response { get; set; }
        public String ResponseTime { get; set; }

        public ApplicationResponse()
        {
            //TODOG:
            ResponseCount = 1;
            ResponseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        }
    }

    public class Response
    {
        public Int32? Code { get; set; }
        //public Boolean IsSapException { get; set; }
        public Message Message { get; set; }
    }

    public class Message
    {
        public Message()
        {
            this.Lang = string.Empty;
        }

        public String Lang { get; set; }
        public String Text { get; set; }
        public XElement Object { get; set; }

    }

    public class Value
    {
        public Object Model { get; set; }
        public String DocEntry { get; set; }
        public String ErrorMessage { get; set; }
    }

    #endregion

}
