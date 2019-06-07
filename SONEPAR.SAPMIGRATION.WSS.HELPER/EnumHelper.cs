using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SONEPAR.SAPMIGRATION.WSS.HELPER
{
    class EnumHelper
    {
    }

    public enum Marca
    {
        Amp = 0,
        Dirome = 1,
        Sonepar = 2,
        Vyf = 3,
    }

    public enum ApplicationErrorType
    {
        EnvioASap = 1,
        Internal = 2,
        Unhandled = 3,
    }
    public enum ObjectType
    {
        StockTransfer = 1,
        GoodsReceipt = 2
    }

    public enum State
    {
        Pendiente = 1,
        Exitoso = 2,
        Error = 3
    }


    public enum ResponseStatus
    {
        Success = 0,
        Error = 1
    }

    public enum ResponseType
    {
        Message = 1,
        Object = 2,
    }

    public enum ResponseObjectType
    {
        None = 0,
        DocEntry = 1,
        Dynamic = 2
    }

    public enum ResponseTypes
    {
        XMLResponse = 1,
        ObjectResponse = 2,
        XMLObjectResponse = 3,
        NoResponse = 4
    }

    public enum MessageTypes
    {
        None = 0,
        Success = 1,
        Errors = 2,
        Warnings = 3,
        Information = 4
    }

    public enum ApplicationDocumentType
    {

        Invoice = 1,
        CreditNote = 2,
        DebitMemo = 3,
        IncomingPayments = 4,
        InventoryAjusted = 5,
        SalesOrder = 6,
    }

    public enum _DocumentType
    {
        Boleta = 1,
        Factura = 2,
        NotaCredito = 3,
        NotaDebito = 4
    }

    #region Connections

    public enum ConnectionName
    {
        AppConnection = 0,
        SqlSapConnection = 1,
    }

    public enum DBServerType
    {
        MySql = 0,
        Sql = 1,
    }

    #endregion

    public enum EmbebbedFileName
    {
        Query_AppGetSalesOrders = 0,
        HtmlMailTemplate = 1,

    }

    public enum AppSource //Debe tener el mismo id que el origen 
    {
        AppSonemas = 1,
        WebCotiza = 2,
    }

    public enum MigrationState
    {
        Error = 0,
        Success = 1,
    }
}
