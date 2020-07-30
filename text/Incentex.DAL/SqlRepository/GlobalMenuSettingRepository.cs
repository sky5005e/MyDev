using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class GlobalMenuSettingRepository : RepositoryBase
    {
        IQueryable<GlobalMenuSetting> GetAllQuery()
        {
            IQueryable<GlobalMenuSetting> qry = from g in db.GlobalMenuSettings
                                                select g;
            return qry;
        }
        public GlobalMenuSetting GetById(Int64 WorkgroupId, Int64 StoreId)
        {
            //GlobalMenuSetting objList = GetAllQuery().Where(a => a.WorkgroupId == WorkgroupId && a.StoreId == StoreId).ToList<GlobalMenuSetting>().SingleOrDefault();

            GlobalMenuSetting objList = (from g in db.GlobalMenuSettings
                                         where g.WorkgroupId == WorkgroupId &&
                                               g.StoreId == StoreId
                                         select g).SingleOrDefault();
            return objList;
        }
    }
}
