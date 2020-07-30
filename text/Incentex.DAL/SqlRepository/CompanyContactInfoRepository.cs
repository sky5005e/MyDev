using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class CompanyContactInfoRepository : RepositoryBase
    {
        IQueryable<CompanyContactInfo> GetAllQuery()
        {
            IQueryable<CompanyContactInfo> qry = from C in db.CompanyContactInfos
                                                 select C;
            return qry;
        }

        /// <summary>
        /// GetById
        /// Reterieve  List of comapnyContactinfo by CompanyContactinfoId
        /// Update By : Gaurang Pathak
        /// </summary>
        /// Nagmani 08/09/2010
        /// <param name="CompanyContactinfoId"></param>
        public List<CompanyContactInfo> GetByCompanyId(Int64 CompanyContactinfoId)
        {
            //List<CompanyContactInfo> objList = GetAllQuery().Where(s => s.CompanyContactInfoID == CompanyContactinfoId).ToList();
            List<CompanyContactInfo> objList = (from C in db.CompanyContactInfos
                                                where C.CompanyContactInfoID == CompanyContactinfoId
                                                select C).ToList();
            return objList;
        }

        /// <summary>
        /// GetById
        /// Reterieve  a comapnyContactinfo by Company ID And AccountType
        /// </summary>
        /// Nagmani 08/09/2010
        /// Update By : Gaurang Pathak
        /// <param name="customerID"></param>
        /// <param name="AccountType"></param>
        public CompanyContactInfo GetById(Int64 CompanyId, String AccountType)
        {
            //CompanyContactInfo obj = GetSingle(GetAllQuery().Where(C => C.CompanyID == CompanyId && C.ContactType == AccountType).ToList());
            CompanyContactInfo obj = (from C in db.CompanyContactInfos
                                      where C.CompanyID == CompanyId && 
                                            C.ContactType == AccountType
                                      select C).FirstOrDefault(); 
            return obj;
        }

        /// <summary>
        /// Delete a comapnyContactinfo by Company ID
        /// </summary>
        /// <param name="customerID"></param>
        public void DeleteCompanyContact(string ComapnyID, string AccoutType)
        {
            var matchedCompany = (from c in db.GetTable<CompanyContactInfo>()
                                  where c.CompanyID == Convert.ToInt32(ComapnyID) && c.ContactType == AccoutType
                                  select c).SingleOrDefault();
            try
            {
                if (matchedCompany != null)
                {
                    db.CompanyContactInfos.DeleteOnSubmit(matchedCompany);

                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Reterive  CompanyContactInfo by Company ID And ContactType
        /// </summary>
        /// GetSingleCompanyContactInfo()
        /// Nagmani 08/09/2010
        /// <param name="Company ID"></param>
        /// <param name="ContactType"></param>
        public CompanyContactInfo GetSingleCompanyContactInfo(int ComapnyID, string ContactType)
        {

            return (from e in db.GetTable<CompanyContactInfo>()

                    where (e.CompanyID == ComapnyID && e.ContactType == ContactType)

                    select e).SingleOrDefault<CompanyContactInfo>();

        }
    }
}
