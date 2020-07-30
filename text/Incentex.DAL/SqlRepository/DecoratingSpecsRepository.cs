using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace Incentex.DAL.SqlRepository
{
    public class DecoratingSpecsRepository : RepositoryBase
    {
        /// <summary>
        /// Check Existence of  Decorating Number
        /// </summary>
        /// <returns></returns>
        public Boolean HasRandomDecoratingNumber(String Number)
        {
            var obj = (from n in db.DecoratingSpecificationsMasters
                       where n.DecoratingReferenceTag == Number
                       select n).ToList();
            if (obj.Count > 0)
                return true;
            else
                return false;

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
        public List<AllDecoratingSpecLibrary> GetAllDecoratingSpecLibrary(Int64 companyID, String JobName)
        {
            var qry = (from a in db.DecoratingSpecificationsMasters
                       join c in db.Companies on a.CompanyID equals c.CompanyID
                       where (companyID != 0 ? c.CompanyID == companyID : true) &&
                             (!String.IsNullOrEmpty(JobName) ? a.JobName == JobName : true)
                       select new AllDecoratingSpecLibrary
                       {
                           DecoratingSpecificationsID = a.DecoratingSpecificationsID,
                           CompanyName = c.CompanyName,
                           CompanyID = c.CompanyID,
                           JobName = a.JobName,
                           Date = a.CreatedDate
                       }).ToList();

            //if (companyID != 0)
            //    qry = qry.Where(a => a.CompanyID == companyID).ToList();
            //if (!String.IsNullOrEmpty(JobName))
            //    qry = qry.Where(a => a.JobName == JobName).ToList();

            return qry.ToList();
        }

        public DecoratingSpecificationsMaster GetDecoratingDetailsByID(Int64 decoID)
        {
            return db.DecoratingSpecificationsMasters.Where(a => a.DecoratingSpecificationsID == decoID).FirstOrDefault();
        }
        public List<DecoratingPartner> GetAllDecoratingPartnersByID(Int64 ID)
        {
            return db.DecoratingPartners.Where(a => a.SupplierID == ID).ToList();
        }


    }
    public class AllDecoratingSpecLibrary
    {
        public Int64 DecoratingSpecificationsID { get; set; }
        public Int64 CompanyID { get; set; }
        public String CompanyName { get; set; }
        public String JobName { get; set; }
        public DateTime? Date { get; set; }
    }
}
