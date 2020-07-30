using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;
namespace Incentex.DAL.SqlRepository
{
    public class CampHistory : RepositoryBase
    {

        IQueryable<CampaignMailHistory> GetAllQuery()
        {
            IQueryable<CampaignMailHistory> qry = from c in db.CampaignMailHistories
                                                  select c;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="campid"></param>
        /// <returns></returns>
        public CampaignMailHistory GetByIdUserCamp(Int32 userinfo, Int32 campid)
        {
            //CampaignMailHistory obj = GetSingle(GetAllQuery().Where(s => s.UserInfoID == userinfo && s.CampID == campid).ToList());

            CampaignMailHistory obj = (from c in db.CampaignMailHistories
                                       where c.UserInfoID == userinfo && c.CampID == campid
                                       select c).FirstOrDefault();


            return obj;
        }
    }
}
