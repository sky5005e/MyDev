using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class IssuanceCompanyAddressRepository : RepositoryBase
    {
        IQueryable<IssuanceCompanyAddress> GetAllQuery()
        {
            IQueryable<IssuanceCompanyAddress> qry = from c in db.IssuanceCompanyAddresses
                                                     select c;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="IssuanceCompanyAddressId"></param>
        /// <returns></returns>
        public IssuanceCompanyAddress GetById(Int64 IssuanceCompanyAddressId)
        {
            //IssuanceCompanyAddress obj = GetSingle(GetAllQuery().Where(s => s.IssuanceCompanyAddressId == IssuanceCompanyAddressId).ToList());

            IssuanceCompanyAddress obj = (from c in db.IssuanceCompanyAddresses
                                          where c.IssuanceCompanyAddressId == IssuanceCompanyAddressId
                                          select c).SingleOrDefault();

            return obj;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UniformIssuancePolicyID"></param>
        /// <returns></returns>
        public IssuanceCompanyAddress GetByIssuanceId(Int64 UniformIssuancePolicyID)
        {
            //IssuanceCompanyAddress obj = GetSingle(GetAllQuery().Where(s => s.UniformIssuancePolicyID == UniformIssuancePolicyID).ToList());

            IssuanceCompanyAddress obj = (from c in db.IssuanceCompanyAddresses
                                          where c.UniformIssuancePolicyID == UniformIssuancePolicyID
                                          select c).FirstOrDefault();

            return obj;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UniformIssuancePolicyID"></param>
        /// <returns></returns>
        public IssuanceCompanyAddress GetByUniformIssuancePolicyId(Int64 UniformIssuancePolicyID)
        {
            //IssuanceCompanyAddress objUniformIssuancePolicyItem = GetSingle(GetAllQuery().Where(u => u.UniformIssuancePolicyID == UniformIssuancePolicyID).ToList());

            IssuanceCompanyAddress objUniformIssuancePolicyItem = (from c in db.IssuanceCompanyAddresses
                                                                   where c.UniformIssuancePolicyID == UniformIssuancePolicyID
                                                                   select c).FirstOrDefault();

            return objUniformIssuancePolicyItem;
        }

        public string UpdateIssuanceAddress(Int64 IssuanceCompanyAddressId, Int64 uniformIssuancePolicyId)
        {
            db.UpdateIssuanceAddress(IssuanceCompanyAddressId, uniformIssuancePolicyId);
            db.SubmitChanges();
            return "updated";
        }

        public string DeleteAddress(Int64 uniformIssuancePolicyId)
        {
            db.DeleteIssuanceCompanyAddress(uniformIssuancePolicyId);
            db.SubmitChanges();
            return "Deleted";
        }
    }
}
