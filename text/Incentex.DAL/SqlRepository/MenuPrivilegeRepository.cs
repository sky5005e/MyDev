using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class MenuPrivilegeRepository : RepositoryBase
    {
        IQueryable<INC_MenuPrivilege> GetAllQuery()
        {
            IQueryable<INC_MenuPrivilege> qry = from c in db.INC_MenuPrivileges
                                                select c;
            return qry;
        }
        public List<INC_MenuPrivilege> GetFrontMenu(string menutype, string usertype)
        {
            return db.INC_MenuPrivileges.WithFrontMenus(menutype, usertype).ToList();
        }
        //Get Incentex Employee's Data Rights.
        public List<INC_MenuPrivilege> GetBackMenuByEmployeeID(string menutype, string usertype,string MenuPriviledge)
        {

            return db.INC_MenuPrivileges.Where(m =>m.sMenuType == menutype && m.sUserType == usertype).WithEmployeeTypes(MenuPriviledge).ToList();
            /*return (
              from inc_menuprivileges in db.INC_MenuPrivileges
              where
              inc_menuprivileges.sMenuType == "BackEnd" &&
              inc_menuprivileges.sUserType == "Incentex Employee" &&

              (from incempmenuaccesses in db.IncEmpMenuAccesses
               where
               incempmenuaccesses.IncentexEmployeeID == 5
               select new
               {incempmenuaccesses.MenuPrivilegeID}
               ).Contains( new { inc_menuprivileges.iMenuPrivilegeID })
              select new
              {
                  inc_menuprivileges.iMenuPrivilegeID,
                  inc_menuprivileges.sDescription,
                  inc_menuprivileges.sMenuType,
                  inc_menuprivileges.sUserType
              }
          ).ToList();*/
        }
        /// <summary>
        /// Get Company Employee Menus by UserInfoId (Front - side)
        /// </summary>
        /// <param name="UserInfoId"></param>
        /// <returns></returns>
        public List<SelectFrontMenuByCompanyEmployeeResult> GetMenusByUserInformationId(Int64 UserInfoId)
        {
            var obj = (from a in db.SelectFrontMenuByCompanyEmployee(UserInfoId)
                       select a);
            return obj.ToList();
        }

        /// <summary>
        /// Get Company Second Top Navigation Menus for Company Admin(Front - side)
        /// </summary>
        /// <param name="UserInfoId"></param>
        /// <returns></returns>
        public List<SelectFrontSecondMenuByCompanyEmployeeResult> GetSecondTopMenusByUserInformationId(Int64 UserInfoId)
        {
            var obj = (from a in db.SelectFrontSecondMenuByCompanyEmployee(UserInfoId)
                       select a);
            return obj.ToList();
        }

        public List<GetUserAssignedSubCategoryByCategoryIDAndUserInfoIDResult> GetUserAssignedSubCategoryByCategoryIDAndUserInfoID(Int64 UserInfoID, Int64 CategoryID)
        {
            return db.GetUserAssignedSubCategoryByCategoryIDAndUserInfoID(UserInfoID, CategoryID).ToList();
        }

        public List<SelectAddiInfoAccessByCompanyEmployeeResult> GetAddiInfoByUserInformationId(Int64 UserInfoId)
        {
            var obj = (from a in db.SelectAddiInfoAccessByCompanyEmployee(UserInfoId)
                       select a);
            return obj.ToList();
        }
        
        //Created on 25 Feb
        public INC_MenuPrivilege GetMenuByMenuType(string menutype, string usertype,string description)
        {
            return GetAllQuery().Where(s => s.sDescription == description && s.sMenuType == menutype && s.sUserType == usertype).SingleOrDefault();
        }

        //Created on 26 Jan 2012 by Devraj Gadhavi to get menuprivilegeid by description of the menu
        public long GetMenuPrivilegeIDByDescription(String sDescription)
        {
            return db.INC_MenuPrivileges.FirstOrDefault(le => le.sDescription == sDescription).iMenuPrivilegeID;
        }

    }
    public static class INC_MenuPrivilegeExtension
    {
        public static IQueryable<INC_MenuPrivilege> WithFrontMenus(this IQueryable<INC_MenuPrivilege> qry, string menuType, string userType)
        {
            return from m in qry
                   where (m.sMenuType == menuType && m.sUserType == userType)
                   select m;
        }

        public static IQueryable<INC_MenuPrivilege> WithEmployeeTypes(this IQueryable<INC_MenuPrivilege> qry, string MenuPrivildgeType)
        {
            string[] MenuPrivildgeTypeid = MenuPrivildgeType.Split(',');

            return from m in qry
                   where MenuPrivildgeTypeid.Contains(m.iMenuPrivilegeID.ToString())
                   select m;
        }
    }
}
