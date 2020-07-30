using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace Incentex.DAL.SqlRepository
{
    public class ArtWorkRepository : RepositoryBase
    {
        IQueryable<CompanyArtwork> GetAllQuery()
        {
            IQueryable<CompanyArtwork> qry = from a in db.CompanyArtworks
                                             select a;
            return qry;
        }

        public List<CompanyArtWorkResults> getUniqueFileCategories()
        {
            IQueryable<CompanyArtWorkResults> list = (from a in db.CompanyArtworks
                                                      join f in db.INC_Lookups
                                                      on a.FileCategoryId equals f.iLookupID
                                                      select new CompanyArtWorkResults
                                                      {
                                                          filecategoryid = a.FileCategoryId,
                                                          FileCategoryName = f.sLookupName
                                                      }
                        );

            list.GroupBy(a => a.filecategoryid);

            return list.ToList<CompanyArtWorkResults>();


        }

        public List<CompanyArtWorkResults> GetAllArts(Int64 CustomerId, Int64 FileCategoryId, string FileName)
        {
            return (from a in db.CompanyArtworks
                    join i in db.INC_Lookups
                    on a.FileCategoryId equals i.iLookupID
                    where
                    (
                          a.FileCategoryId == FileCategoryId
                          && a.CustomerId == CustomerId
                          && SqlMethods.Like(a.ThumbImageOName, "%" + FileName + "%")
                    )
                    select new CompanyArtWorkResults
                    {
                        ArtWorkId = a.ArtworkId,
                        filecategoryid = a.FileCategoryId,
                        FileCategoryName = i.sLookupName,
                        LargerImageSName = a.LargerImageSName,
                        ThumbImageSName = a.ThumbImageSName,
                        LargerImageOName = a.LargerImageOName,
                        ThumbImageOName = a.ThumbImageOName,
                    }).ToList<CompanyArtWorkResults>();
        }

        /// <summary>
        /// Update By : Gaurang Pathak 
        /// </summary>
        /// <param name="Artworkid"></param>
        /// <returns></returns>
        public CompanyArtwork GetById(Int64 Artworkid)
        {
            //return GetAllQuery().Where(a => a.ArtworkId == Artworkid).SingleOrDefault();

            return (from a in db.CompanyArtworks
                    where a.ArtworkId == Artworkid
                    select a).SingleOrDefault();
        }

        public ArtworkLibrary GetArtworkById(Int64 Artworkid)
        {
            return db.ArtworkLibraries.Where(a => a.ArtworkID == Artworkid).SingleOrDefault();
        }

        public List<ArtworkDecoratingLibrary> GetArtworkLibrarybyCompanyID(Int64 CompanyID)
        {
            return db.ArtworkDecoratingLibraries.Where(q => q.CompanyID == CompanyID).ToList();
        }

        public List<ArtworkDecoratingLibrary> GetArtworkLibrarybyArtDecoratingID(String artID)
        {
            String[] list = artID.Split(',');
            List<ArtworkDecoratingLibrary> objList = new List<ArtworkDecoratingLibrary>();
            foreach (var item in list)
            {
                var qry = (from n in db.ArtworkDecoratingLibraries
                           where n.ArtworkDecoratingID == Convert.ToInt64(item) 
                           select n).ToList();
                objList.AddRange(qry);
            }
            return objList;

        }
        /// <summary>
        /// Set  
        /// </summary>
        /// <returns></returns>
        public Boolean HasRandomArtDesignNumber(Int64 artDesignNumber)
        {
            var obj = (from n in db.ArtworkLibraries
                       where n.ArtDesignNumber == artDesignNumber
                       select n).ToList();
            if (obj.Count > 0)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Check Existence of  Art work Number
        /// </summary>
        /// <returns></returns>
        public Boolean HasRandomArtworkNumber(String artDesignNumber)
        {
            var obj = (from n in db.ArtworkDecoratingLibraries
                       where n.ArtworkNumber == artDesignNumber
                       select n).ToList();
            if (obj.Count > 0)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Delete a artwork Item record by ID
        /// </summary>
        /// <param name="artworkID"></param>
        public void DeleteArtworkLibraryById(int artworkID)
        {
            var matchedId = (from a in db.GetTable<ArtworkLibrary>()
                             where a.ArtworkID == artworkID
                             select a).SingleOrDefault();
            try
            {
                if (matchedId != null)
                {
                    db.ArtworkLibraries.DeleteOnSubmit(matchedId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ArtWorkList> GetAllArtWorkFiles(Int64 artworkID)
        {
            var fileName = (from i in db.ArtworkLibraries
                            where i.ArtworkID == artworkID
                            select i.ArtworkFiles).ToArray();
            List<String> stringList = new List<String>();
            foreach (var items in fileName)
            {
                String[] list = items.Split(',');
                stringList.AddRange(list.Select(s => s));
            }
            List<ArtWorkList> objlist = new List<ArtWorkList>();
            foreach (var item in stringList)
            {
                var qry = (from a in db.ArtworkLibraries
                           join i in db.INC_Lookups on a.ArtworkFor equals i.iLookupID
                           where a.ArtworkID == a.ArtworkID && a.ArtworkFiles.Contains(item)
                           select new ArtWorkList
                           {
                               ArtworkFor = i.sLookupName,
                               ArtworkName = a.ArtworkName,
                               ArtWorkId = a.ArtworkID,
                               ArtFileName = item.ToString()
                           }).ToList();
                objlist.AddRange(qry.ToList());
            }
            return objlist;

        }

        /// <summary>
        /// Get All Artwork Details
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="ArtworkName"></param>
        /// <param name="ArtworkForID"></param>
        /// <param name="ArtworkDesign"></param>
        /// <returns></returns>
        public List<AllArtWorkLibrary> GetAllArtworkLibrary(Int64 companyID, String ArtworkName)
        {
            var qry = (from a in db.ArtworkDecoratingLibraries
                       join c in db.Companies on a.CompanyID equals c.CompanyID
                       where (companyID != 0 ? c.CompanyID == companyID : true) &&
                             (!String.IsNullOrEmpty(ArtworkName) ? a.ArtworkName == ArtworkName : true)     
                       select new AllArtWorkLibrary
                       {
                           ArtworkID = a.ArtworkDecoratingID,
                           CompanyName = c.CompanyName,
                           CompanyID = c.CompanyID,
                           ArtworkName = a.ArtworkName,
                           ArtDesignNumber = a.ArtworkNumber,
                       }).ToList();

            //if (companyID != 0)
            //    qry = qry.Where(a => a.CompanyID == companyID).ToList();
            //if (!String.IsNullOrEmpty(ArtworkName))
            //    qry = qry.Where(a => a.ArtworkName == ArtworkName).ToList();

            return qry.ToList<AllArtWorkLibrary>();
        }


        /// <summary>
        /// Get All Artwork Details
        /// update By : Gaurang Pathak
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="ArtworkName"></param>
        /// <param name="ArtworkForID"></param>
        /// <param name="ArtworkDesign"></param>
        /// <returns></returns>
        public List<AllArtWorkLibrary> GetAllArtworkLibrary(Int64 companyID, String ArtworkName, Int64 ArtworkForID, Int64 ArtworkDesign)
        {
            var qry = (from a in db.ArtworkLibraries
                       join c in db.Companies on a.CompanyID equals c.CompanyID
                       where (companyID != 0 ? c.CompanyID == companyID : true) &&
                             (!String.IsNullOrEmpty(ArtworkName) ? a.ArtworkName == ArtworkName : true) &&
                             (ArtworkForID != 0 ? a.ArtworkFor == ArtworkForID : true) &&   
                             (ArtworkDesign != 0 ? a.ArtDesignNumber.Value.ToString() == ArtworkDesign.ToString() : true) 
                       select new AllArtWorkLibrary
                       {
                           ArtworkID = a.ArtworkID,
                           CompanyName = c.CompanyName,
                           CompanyID = c.CompanyID,
                           ArtworkName = a.ArtworkName,
                           ArtDesignNumber = a.ArtDesignNumber.ToString(),
                           ArtworkFor = a.ArtworkFor
                       }).ToList();

            //if (companyID != 0)
            //    qry = qry.Where(a => a.CompanyID == companyID).ToList();
            //if (!String.IsNullOrEmpty(ArtworkName))
            //    qry = qry.Where(a => a.ArtworkName == ArtworkName).ToList();
            //if (ArtworkForID != 0)
            //    qry = qry.Where(a => a.ArtworkFor == ArtworkForID).ToList();
            //if (ArtworkDesign != 0)
            //    qry = qry.Where(a => a.ArtDesignNumber == ArtworkDesign.ToString()).ToList();

            return qry.ToList<AllArtWorkLibrary>();
        }

    }

    public class CompanyArtWorkResults
    {
        public long? filecategoryid { get; set; }
        public string FileCategoryName { get; set; }
        public long ArtWorkId { get; set; }
        public string LargerImageSName { get; set; }
        public string ThumbImageSName { get; set; }
        public string LargerImageOName { get; set; }
        public string ThumbImageOName { get; set; }

    }
    public class ArtWorkList
    {
        public Int64 ArtWorkId { get; set; }
        public String ArtFileName { get; set; }
        public String ArtworkFor { get; set; }
        public String ArtworkName { get; set; }

    }
    public class AllArtWorkLibrary
    {
        public Int64 ArtworkID { get; set; }
        public Int64 CompanyID { get; set; }
        public String CompanyName { get; set; }
        public String ArtworkName { get; set; }
        public Int64? ArtworkFor { get; set; }
        public String ArtDesignNumber { get; set; }

    }

}
