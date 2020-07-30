using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class UserActivityRepository : RepositoryBase
    {
        IQueryable<UserLoginActivity> GetAllQuery()
        {
            IQueryable<UserLoginActivity> qry = from u in db.UserLoginActivities
                                                where u.UserInformationId != 2375
                                                select u;
            return qry;
        }

        public UserLoginActivity GetById(int LoginActivityID)
        {
            return db.UserLoginActivities.FirstOrDefault(C => C.UserActivityId == LoginActivityID && C.UserInformationId != 2375);
        }
        public UserLoginActivity GetByUserId(Int64 UserId)
        {
            return db.UserLoginActivities.FirstOrDefault(C => C.UserInformationId == UserId && C.UserInformationId != 2375);
        }
    }
}
