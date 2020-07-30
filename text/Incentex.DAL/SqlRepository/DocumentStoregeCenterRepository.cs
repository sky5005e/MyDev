using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class DocumentStoregeCenterRepository:RepositoryBase
    {
        #region Common Functions
        /// <summary>
        /// Get Document Storege Type 
        /// added by Gaurang 1Nov2012
        /// </summary>
        /// <returns></returns>
        public List<INC_Lookup> GetDocumentStoregeType()
        {
            List<INC_Lookup> qry = (from q in db.INC_Lookups
                                    where q.iLookupCode == "DocumentStorageType"
                                    select q).ToList();

            return qry;
        }

        /// <summary>
        /// Get all Document Storege Center by Search 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="uplodedby"></param>
        /// <param name="searchkeyword"></param>
        /// <returns></returns>
        public List<DocumentStoregeCentreCustom> GetDocumentStoregecenterBySearch(string filename, string uplodedby, string searchkeyword, long? DocumentType)
        {
            List<DocumentStoregeCentreCustom> qry = (from q in db.DocumentStorageCentres
                                                     join u in db.UserInformations on q.UplodedBy equals u.UserInfoID
                                                     from objFinaljoinLastViewBy in db.UserInformations.Where(t=>t.UserInfoID == q.LastViewBy).DefaultIfEmpty()     
                                                     where (!string.IsNullOrEmpty(filename) ? q.FileName.Contains(filename) : true) &&
                                                           (!string.IsNullOrEmpty(searchkeyword) ? q.SearchKeyWord.Contains(searchkeyword) : true) &&
                                                           (!string.IsNullOrEmpty(uplodedby) ? ((u.FirstName.Contains(uplodedby) || u.LastName.Contains(uplodedby)) || ((u.FirstName + " " + u.LastName).ToLower() == uplodedby.ToLower())) : true) &&
                                                           (DocumentType != null  ? q.DocumentType == DocumentType  : true)
                                                     select new DocumentStoregeCentreCustom
                                                     {
                                                         DocumentStorageID = q.DocumentStorageID,
                                                         FileName = q.FileName,
                                                         UplodedDate = q.UplodedDate,
                                                         UplodedBy = u.LastName!=null ? (u.FirstName + " " + u.LastName) : u.FirstName,
                                                         FileSize = (q.FileSize.Value /1024)/1024,    //Convert Bytes to MB
                                                         OriginalFileName = q.OriginalFileName, 
                                                         extension = q.extension, 
                                                         LastViewBy = (objFinaljoinLastViewBy != null && objFinaljoinLastViewBy.FirstName != null && objFinaljoinLastViewBy.LastName != null) ? (objFinaljoinLastViewBy.FirstName + " " + objFinaljoinLastViewBy.LastName) : string.Empty
                                                     }).ToList<DocumentStoregeCentreCustom>();

            return qry;
        }

        public DocumentStorageCentre GetDocumentStoregeCenterById(long DocumentStoregeID)
        {
            return (from d in db.DocumentStorageCentres
                    where d.DocumentStorageID == DocumentStoregeID
                    select d).SingleOrDefault(); 
        }

        public void UpdateDocumentStoregeCenter(long UpdateBy , long DocumentstoregeID)
        {
            DocumentStorageCentre objdocument = GetDocumentStoregeCenterById(DocumentstoregeID);
            if (objdocument != null)
            {
                objdocument.LastViewBy = UpdateBy;
                base.SubmitChanges();    
            }
        }

        public int CheckDocumentTypeExistence(string DocumentType, long TrainingTypeID)
        {
            return (from l in db.INC_Lookups
                    where l.sLookupName.ToLower() == DocumentType.ToLower() && l.iLookupCode.Contains("DocumentStorageType")
                          && l.iLookupID != TrainingTypeID 
                    select l.iLookupID).Count();  
        }

        public void DeleteDocumentStorageCenter(long DocumentStorageID)
        {
            DocumentStorageCentre obj = GetDocumentStoregeCenterById(DocumentStorageID);
            if (obj != null)
            {
                db.DocumentStorageCentres.DeleteOnSubmit(obj);
                base.SubmitChanges();  
            }
        }

        #endregion

        #region Extension Class
        public class DocumentStoregeCentreCustom
        {
            public long DocumentStorageID { get; set; }

            public string FileName { get; set; }

            public string SearchKeyWord { get; set; }

            public string Email { get; set; }

            public decimal FileSize { get; set; }

            public string OriginalFileName { get; set; }

            public long DocumentType { get; set; }

            public System.DateTime UplodedDate { get; set; }

            public string UplodedBy { get; set; }

            public string LastViewBy { get; set; }

            public string extension { get; set; }
        }
        #endregion
    }
}
