using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
   public class SupplierMenuAccessRepository:RepositoryBase
    {
       IQueryable<SupplierEmpMenuAccess> GetAllQuery()
       {
           IQueryable<SupplierEmpMenuAccess> qry = from e in db.SupplierEmpMenuAccesses
                                              select e;
           return qry;
       }
       public List<SupplierEmpMenuAccess> GetMenusBySupplierEmployeeID(Int64 SupplierEmployeeID)
       {
           return GetAllQuery().Where(s => s.SupplierEmployeeID == SupplierEmployeeID).ToList();
       }
       public List<SupplierEmpMenuAccessResult> GetMenusBySupplierIdWithPath(Int64 SupplierId)
       {
           return (from e in db.SupplierEmpMenuAccesses
                   join m in db.INC_MenuPrivileges
                   on e.MenuPrivilegeID equals m.iMenuPrivilegeID
                   where e.SupplierEmployeeID == SupplierId
                   select new SupplierEmpMenuAccessResult
                   {
                       IconPath = m.PageUrl,
                       SupplierEmpMenuAccessID = e.SupplierEmpMenuAccessID,
                       SupplierEmployeeID = e.SupplierEmployeeID,
                       MenuPrivilegeID = e.MenuPrivilegeID
                   }
                 ).ToList<SupplierEmpMenuAccessResult>();
       }
    }

   public class SupplierEmpMenuAccessResult
   {
       public long SupplierEmpMenuAccessID { get; set; }
       public long SupplierEmployeeID { get; set; }
       public long MenuPrivilegeID { get; set; }
       public string IconPath { get; set; }
   }
}
