using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class CompanyEmpManageEmailRepository : RepositoryBase
    {
        IQueryable<CompanyEmpManageEmail> GetAllQuery()
        {
            IQueryable<CompanyEmpManageEmail> qry = from e in db.CompanyEmpManageEmails
                                                    select e;
            return qry;
        }

        public List<CompanyEmpManageEmail> GetEmailRightsByUserInfoID(Int64 UserInfoID)
        {
            //return GetAllQuery().Where(s => s.UserInfoID == UserInfoID).ToList();

            return (from e in db.CompanyEmpManageEmails
                    where e.UserInfoID == UserInfoID
                    select e).ToList();
        }
    }
}
