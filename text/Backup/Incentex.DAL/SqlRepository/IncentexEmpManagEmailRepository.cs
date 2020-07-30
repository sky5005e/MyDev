using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
   public class IncentexEmpManagEmailRepository : RepositoryBase
    {
       IQueryable<IncEmpManageEmail> GetAllQuery()
       {
           IQueryable<IncEmpManageEmail> qry = from e in db.IncEmpManageEmails
                                                    select e;
           return qry;
       }

       /// <summary>
       /// Update By ; Gaurang Pathak
       /// </summary>
       /// <param name="UserInfoID"></param>
       /// <returns></returns>
       public List<IncEmpManageEmail> GetEmailRightsByUserInfoID(Int64 UserInfoID)
       {
           //return GetAllQuery().Where(s => s.UserInfoID == UserInfoID).ToList();

           return (from e in db.IncEmpManageEmails
                   where e.UserInfoID == UserInfoID
                   select e).ToList(); 
       }
    }
}
