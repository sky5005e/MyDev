using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class CompanyDocumentRepository : RepositoryBase
    {
        IQueryable<Document> GetAllQuery()
        {
            IQueryable<Document> qry = from c in db.Documents
                                       orderby c.DocumentId
                                       select c;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <returns></returns>
        public Document GetById(Int64 DocumentId)
        {
            //Document objDocument = GetSingle(GetAllQuery().Where(C => C.DocumentId == DocumentId).ToList());

            Document objDocument = (from c in db.Documents
                                    where c.DocumentId == DocumentId
                                    select c).SingleOrDefault();


            return objDocument;
        }

        public class CompanyDocumentResult
        {
            public System.Int64 DocumentId

            { get; set; }
            public System.String sLookupName

            { get; set; }

            public System.String FileName

            { get; set; }
            public System.String iLookupCode

            { get; set; }

        }
        // public List<CompanyDocumentResult> CompanyDcoDetails(Int32 CompanyID)
        //{



        /* var query = (from ords in db.GetTable<Document>()

                         join dets in db.GetTable<INC_Lookup>()
                         on ords.DocumentTypeID equals dets.iLookupID
                         where ords.ForeignKey == CompanyID
                         select new CompanyDocumentResult
                         {
                             FileName = ords.FileName,
                             DocumentId = ords.DocumentId,
                             sLookupName = dets.sLookupName,
                             iLookupCode=dets.iLookupCode
                         }).ToList<CompanyDocumentResult>();
         var matches = (from c in query

                        where c.iLookupCode == "Document"

                        select c).ToList<CompanyDocumentResult>();

         return matches;*/


        //}
        public List<CompanyDocumentResult> GetDocumentDetails(Int32 Companyid)
        {
            return (from doc in db.GetTable<Document>()
                    join lok in db.GetTable<INC_Lookup>()
                    on doc.DocumentTypeID equals lok.iLookupID
                    where doc.ForeignKey == Companyid && lok.iLookupCode == "Document"
                    select new CompanyDocumentResult
                    {
                        FileName = doc.FileName,
                        DocumentId = doc.DocumentId,
                        sLookupName = lok.sLookupName,
                        iLookupCode = lok.iLookupCode
                    }).ToList<CompanyDocumentResult>();

        }
       
        /// <summary>
        /// Delete a Document Record by Document ID
        /// </summary>
        /// <param name="Document ID"></param>
        public void DeleteCompanyDocument(string DocumentID)
        {
            var matchedDocument = (from c in db.GetTable<Document>()
                                   where c.DocumentId == Convert.ToInt32(DocumentID)
                                   select c).SingleOrDefault();
            try
            {
                if (matchedDocument != null)
                {
                    db.Documents.DeleteOnSubmit(matchedDocument);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        /// <summary>
        /// CheckDuplicaetDocument()
        /// Check the CompanyDuplication]
        /// Nagmani Kumar 16/09/2010
        /// </summary>
        /// <param name="documnetName"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public int CheckDuplicaetDocument(string documnetName, int companyid)
        {
            return (from com in db.Documents
                    where com.FileName == documnetName && com.ForeignKey == companyid
                    select com).Count();
        }

    }
}