using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class BillingInformationRepository : RepositoryBase
    {

        IQueryable<BillingInformation> GetAllQuery()
        {
            IQueryable<BillingInformation> qry = from C in db.BillingInformations
                                                 select C;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="BillingInformationId"></param>
        /// <returns></returns>
        public BillingInformation GetBillingById(Int64 BillingInformationId)
        {
            //BillingInformation obj = GetSingle(GetAllQuery().Where(s => s.BillingInformationId == BillingInformationId).ToList());

            BillingInformation obj = (from C in db.BillingInformations
                                      where C.BillingInformationId == BillingInformationId
                                      select C).SingleOrDefault();
            return obj;

        }

    }
}
