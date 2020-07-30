using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class OrderDocumentRepository : RepositoryBase
    {
        IQueryable<OrderDocument> GetAllQuery()
        {
            IQueryable<OrderDocument> qry = from c in db.OrderDocuments
                                       orderby c.FileName
                                       select c;
            return qry;
        }


        public List<SelectDocumentOrderInvoiceResult> GetDocumnet(int SupplierId,int OrderID,string orderFor,string usertype)//,int MyShoppingCartID)
        {
            List<SelectDocumentOrderInvoiceResult> objOrderDoc = new List<SelectDocumentOrderInvoiceResult>();
            if (SupplierId == 0)
            {
                objOrderDoc = db.SelectDocumentOrderInvoice(null, OrderID, orderFor, usertype).ToList();//, MyShoppingCartID).ToList();
            }
            else
            {
                objOrderDoc = db.SelectDocumentOrderInvoice(SupplierId, OrderID, orderFor, usertype).ToList();//, MyShoppingCartID).ToList();
            }

            return objOrderDoc;
        }


        public void DeleteOrderDocument(int DocumentId)
        {

            var matchedStore = (from c in db.GetTable<OrderDocument>()
                                where c.OrderDocumentID == DocumentId
                                select c).SingleOrDefault();
            try
            {
                if (matchedStore != null)
                {
                    db.OrderDocuments.DeleteOnSubmit(matchedStore);
                }
                db.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
    }
}
