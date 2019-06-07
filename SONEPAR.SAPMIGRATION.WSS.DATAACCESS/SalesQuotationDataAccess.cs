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

    public class SalesQuotationDataAccess
    {
        private Boolean Save { get; set; }

        #region AddUpdateDocument
        public void AddUpdateDocument(Company company, DocumentViewModel model)
        {
            Documents document = company.GetBusinessObject((BoObjectTypes)(model.ObjectType));
            if (model.IsUpdate)
                if (!document.GetByKey(model.DocEntry))
                    throw new Exception("No se encontró el documento con docEntry" + model.DocEntry);

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
            for (int i = 0; i < document.Lines.Count; i++)
            {
                document.Lines.SetCurrentLine(i);
                if (document.Lines.LineNum == model.LineNum)
                {
                    var unitPrice = document.Lines.UnitPrice;

                    document.Lines.ItemCode = model.ItemCode;
                    document.Lines.UnitPrice = unitPrice;
                    document.Lines.Price = unitPrice;
                    break;
                }
            }
        }
        #endregion
    }
}
