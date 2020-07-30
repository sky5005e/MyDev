using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class AppSettingRepository : RepositoryBase
    {
        IQueryable<INC_AppSetting> GetAllQuery()
        {
            IQueryable<INC_AppSetting> qry = from a in db.INC_AppSettings
                                             select a;
            return qry;
        }

        public INC_AppSetting GetbyName(string settingName)
        {
            //return GetAllQuery().Where(a => a.sSettingName == settingName).SingleOrDefault();
            return (from a in db.INC_AppSettings
                    where a.sSettingName == settingName
                    select a).SingleOrDefault();
        }
    }
}
