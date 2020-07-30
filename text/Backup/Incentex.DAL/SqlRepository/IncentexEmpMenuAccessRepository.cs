using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
   public class IncentexEmpMenuAccessRepository :RepositoryBase
    {

       IQueryable<IncEmpMenuAccess> GetAllQuery()
       {
           IQueryable<IncEmpMenuAccess> qry = from e in db.IncEmpMenuAccesses
                                                  select e;
           return qry;
       }

       /// <summary>
       /// Update By : Gaurang Pathak
       /// </summary>
       /// <param name="EmployeeId"></param>
       /// <returns></returns>
       public List<IncEmpMenuAccess> GetMenusByEmployeeId(Int64 EmployeeId)
       {
           //return GetAllQuery().Where(s => s.IncentexEmployeeID == EmployeeId).ToList();

           return (from e in db.IncEmpMenuAccesses
                   where e.IncentexEmployeeID == EmployeeId
                   select e).ToList(); 
       }

       public List<IncEmpMenuAccessResult> GetMenusByEmployeeIdWithPath(Int64 EmployeeId)
       {
               return (from e in db.IncEmpMenuAccesses
                 join m in db.INC_MenuPrivileges
                 on e.MenuPrivilegeID equals m.iMenuPrivilegeID
                 where e.IncentexEmployeeID == EmployeeId
                 select new IncEmpMenuAccessResult
                 {
                     IconPath = m.PageUrl,
                     IncEmpMenuAccessID = e.IncEmpMenuAccessID,
                     IncentexEmployeeID = e.IncentexEmployeeID,
                     MenuPrivilegeID = e.MenuPrivilegeID
                 }
                 ).ToList<IncEmpMenuAccessResult>();

                                                  
       }


    }
   public class IncEmpMenuAccessResult
   {
       public long IncEmpMenuAccessID { get; set; }
       public long IncentexEmployeeID { get; set; }
       public long MenuPrivilegeID { get; set; }
       public string IconPath { get; set; }
   }
}
