using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Incentex.DAL.SqlRepository
{
    public class CompanyEmpMenuAccessRepository : RepositoryBase
    {
        IQueryable<CompanyEmpMenuAccess> GetAllQuery()
        {
            IQueryable<CompanyEmpMenuAccess> qry = from c in db.CompanyEmpMenuAccesses
                                                   select c;
            return qry;
        }
        public List<CompanyEmpMenuAccess> GetMenusByEmployeeId(Int64 EmployeeId)
        {
            //return GetAllQuery().Where(s => s.CompanyEmployeeID == EmployeeId).ToList();

            return (from c in db.CompanyEmpMenuAccesses
                    where c.CompanyEmployeeID == EmployeeId
                    select c).ToList();
        }

        public List<CompanyEmpMenuAccess> getMenuByUserType(Int64 EmployeeId, string UserType)
        {
            return (from menuaccess in db.CompanyEmpMenuAccesses
                    join menuprivilege in db.INC_MenuPrivileges on new { MenuPrivilegeId = menuaccess.MenuPrivilegeID } equals new { MenuPrivilegeId = menuprivilege.iMenuPrivilegeID }
                    where (menuaccess.CompanyEmployeeID == EmployeeId && menuprivilege.sUserType == UserType)
                    select menuaccess
                     ).ToList();

        }

        public List<CompanyEmpMenuAccess> getMenuByUserType(Int64 EmployeeId)
        {
            return (from menuaccess in db.CompanyEmpMenuAccesses
                    join menuprivilege in db.INC_MenuPrivileges on new { MenuPrivilegeId = menuaccess.MenuPrivilegeID } equals new { MenuPrivilegeId = menuprivilege.iMenuPrivilegeID }
                    where (menuaccess.CompanyEmployeeID == EmployeeId)
                    select menuaccess
                     ).ToList();
        }
    }
}
