using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class DocumentRepository:RepositoryBase
    {
        /// <summary>
        /// get all query
        /// added by amit 17Sep10
        /// </summary>
        /// <returns></returns>
        IQueryable<Document> GetAllQuery()
        {
            IQueryable<Document> qry = from d in db.Documents
                                       select d;
            return qry;
        }


        /// <summary>
        /// Get Document by type
        /// added by amit 17Sep10
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="DocumentTypeID"></param>
        /// <returns></returns>
        public List<Document> GetByDocumentTypeId(Int64 DocumentTypeID, Int64 ForeignKey)
        {
            //IQueryable<Document> qry = GetAllQuery().Where(d => d.DocumentTypeID == DocumentTypeID);
            //qry = qry.Where(d => d.ForeignKey == ForeignKey);
            //List<Document> objList = qry.ToList();


            List<Document> objList = (from d in db.Documents
                                      where d.DocumentTypeID == DocumentTypeID &&
                                            d.ForeignKey == ForeignKey 
                                      select d).ToList(); 
           
            return objList;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="DocumentTypeID"></param>
        /// <param name="OriginalFileName"></param>
        /// <returns></returns>
        public Document GetByTypeAndName(Int64 DocumentTypeID, string OriginalFileName)
        {
                //IQueryable<Document> qry = GetAllQuery().Where(d => d.DocumentTypeID == DocumentTypeID);
            //qry = qry.Where(d => d.OriginalFileName.Equals(OriginalFileName) );
            //Document obj = GetSingle(qry.ToList());

            Document obj = (from d in db.Documents
                            where d.DocumentTypeID == DocumentTypeID &&
                                  d.OriginalFileName.Equals(OriginalFileName)
                            select d).FirstOrDefault(); 
            return obj;
        }
        

        
        /// <summary>
        /// Delete document
        /// added by amit 17Sep10
        /// </summary>
        /// <param name="DocumentId"></param>
        public void Delete(Int64 DocumentId)
        {
            Document obj = GetSingle(GetAllQuery().Where(d => d.DocumentId == DocumentId).ToList());

            if(obj != null)
            {
                base.Delete(obj);
                base.SubmitChanges();
            }
        }

        /// <summary>
        /// Delete document
        /// added by amit 17Sep10
        /// </summary>
        /// <param name="DocumentTypeID"></param>
        /// <param name="ForeignKey"></param>
        public void Delete(Int64 DocumentTypeID,Int64 ForeignKey)
        {
            IQueryable<Document> qry = GetAllQuery().Where(d => d.DocumentTypeID == DocumentTypeID);
            qry = qry.Where(d => d.ForeignKey == ForeignKey);

            db.Documents.DeleteAllOnSubmit(qry.ToList());
            db.SubmitChanges();
        }

        public void DeleteEmployeeDocuments(string DocFor, Int64 ForeignKey)
        {
            IQueryable<Document> qry = GetAllQuery().Where(d => d.DocumentFor == DocFor);
            qry = qry.Where(d => d.ForeignKey == ForeignKey);

            db.Documents.DeleteAllOnSubmit(qry.ToList());
            db.SubmitChanges();
        }

    }


}
