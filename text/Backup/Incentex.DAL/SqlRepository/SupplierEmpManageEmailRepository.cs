using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
   public class SupplierEmpManageEmailRepository : RepositoryBase
    {
       IQueryable<SupplierEmpManageEmail> GetAllQuery()
       {
           IQueryable<SupplierEmpManageEmail> qry = from e in db.SupplierEmpManageEmails
                                             select e;
           return qry;
       }
       public List<SupplierEmpManageEmail> GetEmailRightsByUserInfoID(Int64 UserInfoID)
       {
           return GetAllQuery().Where(s => s.UserInfoID == UserInfoID).ToList();
       }
    }
}
