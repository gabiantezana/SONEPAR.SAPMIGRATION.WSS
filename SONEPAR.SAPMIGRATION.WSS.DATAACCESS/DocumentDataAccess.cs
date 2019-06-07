using SAPbobsCOM;
using SONEPAR.SAPMIGRATION.WSS.EXCEPTION;
using SONEPAR.SAPMIGRATION.WSS.HELPER;
using SONEPAR.SAPMIGRATION.WSS.VIEWMODEL.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SONEPAR.SAPMIGRATION.WSS.DATAACCESS
{

    public class DocumentDataAccess
    {
        private Boolean Save { get; set; }

        #region SAPDataAccess

        #region AddUpdateDocument

        public void AddUpdateDocument(Company company, DocumentViewModel model)
        {
            Documents document = company.GetBusinessObject((BoObjectTypes)(model.ObjectType));

            document.DocObjectCode = model.DocObjectCode;
            document.DocumentSubType = model.DocumentSubType;
            document.Series = model.Series;
            document.CardCode = model.CardCode;
            document.DocDate = model.DocDate;
            document.DocDueDate = model.DocDueDate;
            document.TaxDate = model.TaxDate;
            document.DocType = model.DocType;
            document.DocCurrency = model.DocCurrency;
            document.Comments = model.Comments;
            document.Project = model.Project;
            document.Indicator = model.Indicator;
            document.ShipToCode = model.ShipToCode;
            document.Address2 = model.Address2;
            document.AgentCode = model.AgentCode;
            document.NumAtCard = model.NumAtCard;

            if (model.SalesPersonCode != null)
                document.SalesPersonCode = model.SalesPersonCode.Value;
            if (model.PaymentGroupCode != null)
                document.PaymentGroupCode = model.PaymentGroupCode.Value;
            if (model.DocumentsOwner != null)
                document.DocumentsOwner = model.DocumentsOwner.Value;
            if (model.TransportationCode != null)
                document.TransportationCode = model.TransportationCode.Value;
            if (model.ContactPersonCode != null)
                document.ContactPersonCode = model.ContactPersonCode.Value;


            SetUserFields(model, ref document);

            model.DocumentLines.ForEach(x => AddUpdateDocumentLine(x, ref document));

            if (model.IsUpdate)
                Save = document.Update() == ConstantHelper.SuccessSaveSap;
            else
                Save = document.Add() == ConstantHelper.SuccessSaveSap;
            if (!Save)
                throw new SapException();
        }

        private void AddUpdateDocumentLine(DocumentLineViewModel model, ref Documents document)
        {
            if (model.SalesPersonCode != null)
                document.Lines.SalesPersonCode = model.SalesPersonCode.Value;
            document.Lines.ItemCode = model.ItemCode;
            document.Lines.Quantity = model.Quantity;
            document.Lines.UnitPrice = model.UnitPrice;
            document.Lines.DiscountPercent = model.DisccountPercent;
            document.Lines.WarehouseCode = model.WarehouseCode;
            document.Lines.TaxCode = model.TaxCode;
            document.Lines.Currency = model.Currency;
            document.Lines.ProjectCode = model.ProjectCode;
            document.Lines.ShipToDescription = model.ShipToDescription;

            SetUserFields(model, ref document);

            document.Lines.Add();
        }

        public virtual void SetUserFields(DocumentViewModel model, ref Documents document)
        {
            foreach (PropertyInfo property in ReflectionHelper.GetSAPUserFieldsProperties(model))
            {
                try { document.UserFields.Fields.Item(property.Name).Value = property.GetValue(model, null); }
                catch (Exception ex) { { ExceptionHelper.LogException(ex); } }
            }
        }

        public virtual void SetUserFields(DocumentLineViewModel model, ref Documents document)
        {
            foreach (PropertyInfo property in ReflectionHelper.GetSAPUserFieldsProperties(model))
            {
                try { document.Lines.UserFields.Fields.Item(property.Name).Value = property.GetValue(model, null); }
                catch (Exception ex) { { ExceptionHelper.LogException(ex); } }
            }
        }

        #endregion

        #region CancellDocument

        #endregion

        #region General Methods

        public Boolean DocumentExists(Company company, BoObjectTypes documentType, Int32 docEntry, Boolean throwIfNull = false)
        {
            Documents document = GetDocumentByKey(company, documentType, docEntry, throwIfNull);
            if (document == null)
                return false;
            else
                return true;
        }

        public Documents GetDocumentByKey(Company company, BoObjectTypes documentType, Int32 docEntry, Boolean throwIfNull = false)
        {
            Documents document = company.GetBusinessObject(documentType);
            Boolean exists = document.GetByKey(docEntry);

            if (exists)
                return document;
            if (throwIfNull)
                throw new Exception("Document with docEntry: '" + docEntry + "' does not exists.");

            return null;
        }

        #endregion

        #endregion

        #region Queries




        private Fields ExecuteRecordSet(Company company, String query, Boolean throwIfNull = false)
        {
            Fields response = null;
            try
            {
                Recordset recordSet = company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordSet.DoQuery(query);
                response = recordSet.Fields;
                return response;
            }
            catch (Exception ex)
            {
                if (throwIfNull)
                    throw ex;
                return response;
            }
            finally
            {
                //TODOG: CLEAN OBJECT
            }
        }

        #endregion

    }
}
