using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class AssetVendorRepository : RepositoryBase
    {
        #region VendorManagement

        /// <summary>
        /// Get All Vendor
        /// </summary>
        IQueryable<EquipmentVendorMaster> GetAllVendorDetail()
        {
            IQueryable<EquipmentVendorMaster> qry = from c in db.EquipmentVendorMasters
                                                    orderby c.EquipmentVendorID
                                                    select c;
            return qry;
        }

        /// <summary>
        /// Get Vendor by Id
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public EquipmentVendorMaster GetEquipmentVendorById(Int64 EquipmentVendorID)
        {
            //IQueryable<EquipmentVendorMaster> qry = GetAllVendorDetail().Where(s => s.EquipmentVendorID == EquipmentVendorID);
            //EquipmentVendorMaster obj = GetSingle(qry.ToList());

            EquipmentVendorMaster obj = (from c in db.EquipmentVendorMasters
                                         where c.EquipmentVendorID == EquipmentVendorID
                                         select c).SingleOrDefault();

            return obj;
        }

        /// <summary>
        /// Get Vendor List by Id
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public GetGSECompanyByIDResult GetEquipmentVendorListById(Int64 EquipmentVendorID)
        {
            //return GetAllVendorDetail().Where(s => s.EquipmentVendorID == EquipmentVendorID).ToList();
            return db.GetGSECompanyByID(EquipmentVendorID).FirstOrDefault();
            //return (from c in db.EquipmentVendorMasters
            //        orderby c.EquipmentVendorID
            //        where c.EquipmentVendorID == EquipmentVendorID
            //        select c).FirstOrDefault();
        }

        /// <summary>
        /// Get Vendor Detail
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public List<EquipmentVendorMaster> GetAllEquipmentVendor()
        {
            IQueryable<EquipmentVendorMaster> qry = GetAllVendorDetail();
            List<EquipmentVendorMaster> objList = qry.ToList();
            return objList;
        }

        public void DeleteVendor(Int64 EquipmentVendorID)
        {
            var matched = (from c in db.GetTable<EquipmentVendorMaster>()
                           where c.EquipmentVendorID == EquipmentVendorID
                           select c).SingleOrDefault();
            try
            {
                if (matched != null)
                {
                    db.EquipmentVendorMasters.DeleteOnSubmit(matched);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }

        }

        public List<GetEquipVendorDetailResult> GetVendorDetailBySP(Int64 CompanyID)
        {
            var objlist = db.GetEquipVendorDetail(CompanyID).ToList();

            return objlist.ToList<GetEquipVendorDetailResult>();

        }

        public List<EquipmentVendorDocument> GetVendorDoc(Int64 VendorID)
        {
            IQueryable<EquipmentVendorDocument> qry = from c in db.EquipmentVendorDocuments
                                                      where c.DocumentFor == "Vendor"
                                                      orderby c.DocumentId
                                                      select c;
            List<EquipmentVendorDocument> objList = qry.ToList();
            return objList;

        }

        public void DeleteVendorDoc(Int64 DocumentId)
        {
            var matched = (from c in db.GetTable<EquipmentVendorDocument>()
                           where c.DocumentId == DocumentId
                           select c).SingleOrDefault();
            try
            {
                if (matched != null)
                {
                    db.EquipmentVendorDocuments.DeleteOnSubmit(matched);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region VendorEmployeeManagement

        /// <summary>
        /// Get All Vendor Employee
        /// </summary>
        IQueryable<EquipmentVendorEmployee> GetAllVendorEmployeeDetail()
        {
            IQueryable<EquipmentVendorEmployee> qry = from c in db.EquipmentVendorEmployees
                                                      orderby c.VendorEmployeeID
                                                      select c;
            return qry;
        }

        /// <summary>
        /// Get Vendor Employee list by Id
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public List<EquipmentVendorEmployee> GetVendorEmpListById(Int64 VendorEmployeeID)
        {
            //return GetAllVendorEmployeeDetail().Where(s => s.VendorEmployeeID == VendorEmployeeID).ToList();

            return (from c in db.EquipmentVendorEmployees
                    orderby c.VendorEmployeeID
                    where c.VendorEmployeeID == VendorEmployeeID
                    select c).ToList();

        }

        /// <summary>
        /// Get Vendor Employee by Id
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public EquipmentVendorEmployee GetVendorEmpById(Int64 VendorEmployeeID)
        {
            //IQueryable<EquipmentVendorEmployee> qry = GetAllVendorEmployeeDetail().Where(s => s.VendorEmployeeID == VendorEmployeeID);
            //EquipmentVendorEmployee obj = GetSingle(qry.ToList());

            EquipmentVendorEmployee obj = (from c in db.EquipmentVendorEmployees
                                           where c.VendorEmployeeID == VendorEmployeeID
                                           select c).FirstOrDefault();

            return obj;
        }

        /// <summary>
        /// Get Vendor Employee by UserInfoID
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public EquipmentVendorEmployee GetVendorEmpByUserInfoID(Int64 UserInfoID)
        {
            //IQueryable<EquipmentVendorEmployee> qry = GetAllVendorEmployeeDetail().Where(s => s.UserInfoID == UserInfoID);
            //EquipmentVendorEmployee obj = GetSingle(qry.ToList());

            EquipmentVendorEmployee obj = (from c in db.EquipmentVendorEmployees
                                           where c.UserInfoID == UserInfoID
                                           select c).FirstOrDefault();
            return obj;
        }

        /// <summary>
        /// Get all Vendor Employee in list
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public List<EquipmentVendorEmployee> GetAllVendorEmployee()
        {
            IQueryable<EquipmentVendorEmployee> qry = GetAllVendorEmployeeDetail();
            List<EquipmentVendorEmployee> objList = qry.ToList();
            return objList;
        }


        public void DeleteVendorEmployee(Int64 VendorEmployeeID)
        {
            try
            {

                EquipmentVendorEmployee objEmpl = GetVendorEmpById(VendorEmployeeID);
                UserInformationRepository objUserRepo = new UserInformationRepository();
                UserInformation objUI = objUserRepo.GetById(objEmpl.UserInfoID);
                Int64 UserInfoID = objUI.UserInfoID;
                AssetMgtRepository objAssetMgtRepo = new AssetMgtRepository();
                //Delete from EquipmentMenuAccess
                var menu = (from m in db.GetTable<EquipmentMenuAccess>()
                            where m.VendorEmployeeID == VendorEmployeeID
                            select m);
                if (menu != null)
                {
                    db.EquipmentMenuAccesses.DeleteAllOnSubmit(menu);
                }
                db.SubmitChanges();
                //Delete from EquipmentManageEmail
                var Email = (from e in db.GetTable<EquipmentManageEmail>()
                             where e.UserInfoID == UserInfoID
                             select e);
                if (Email != null)
                {
                    db.EquipmentManageEmails.DeleteAllOnSubmit(Email);
                }
                db.SubmitChanges();
                //Delete from EquipmentManageDropdown
                var Drop = (from d in db.GetTable<EquipmentManageDropDown>()
                            where d.UserInfoID == UserInfoID
                            select d);
                if (Drop != null)
                {
                    db.EquipmentManageDropDowns.DeleteAllOnSubmit(Drop);
                }
                db.SubmitChanges();
                //Delete from NoteDetail
                var Notes = (from n in db.GetTable<NoteDetail>()
                             where n.CreatedBy == UserInfoID
                             select n);
                if (Notes != null)
                {
                    db.NoteDetails.DeleteAllOnSubmit(Notes);
                }
                db.SubmitChanges();
                //Delete from ReportAccessRights
                var Reports = (from r in db.GetTable<EquipmentReportAccessRight>()
                               where r.VendorEmployeeID == VendorEmployeeID
                               select r);
                if (Reports != null)
                {
                    db.EquipmentReportAccessRights.DeleteAllOnSubmit(Reports);
                }
                db.SubmitChanges();
                //Delete from EquipmentVendorEmployee
                var matched = (from c in db.GetTable<EquipmentVendorEmployee>()
                               where c.VendorEmployeeID == VendorEmployeeID
                               select c).SingleOrDefault();
                if (matched != null)
                {
                    db.EquipmentVendorEmployees.DeleteOnSubmit(matched);
                }
                db.SubmitChanges();
                //Delete from UserInformation
                var usr = (from u in db.GetTable<UserInformation>()
                           where u.UserInfoID == UserInfoID
                           select u).SingleOrDefault();
                if (usr != null)
                {
                    db.UserInformations.DeleteOnSubmit(usr);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }

        }

        //HELP
        //public void Delete(Int64 EmployeeId)
        //{
        //    EquipmentVendorEmployee objEmpl = GetVendorEmpById(EmployeeId);
        //    UserInformationRepository objUserRepo = new UserInformationRepository();
        //    UserInformation objUI = objUserRepo.GetById(objEmpl.UserInfoID);
        //    CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();

        //    NotesHistoryRepository objNotesHistoryRepository = new NotesHistoryRepository();
        //    if (objEmpl != null)
        //    {
        //        //delete from note history table
        //        objNotesHistoryRepository.Delete(objEmpl.CompanyEmployeeID, Incentex.DAL.Common.DAEnums.NoteForType.CompanyEmployee);

        //        //Delete from EmployeeMenuaccess
        //        CompanyEmpMenuAccessRepository objCmpMenuAccesRepos = new CompanyEmpMenuAccessRepository();
        //        CompanyEmpMenuAccess objCmpMenuAccessfordel = new CompanyEmpMenuAccess();
        //        List<CompanyEmpMenuAccess> lst = objCmpMenuAccesRepos.getMenuByUserType(objEmpl.CompanyEmployeeID);
        //        foreach (CompanyEmpMenuAccess l in lst)
        //        {
        //            objCmpMenuAccesRepos.Delete(l);
        //            objCmpMenuAccesRepos.SubmitChanges();
        //        }

        //        //delete from company employee table
        //        objEmpRepo.DeleteCompanyEmployeeById(objEmpl.CompanyEmployeeID);

        //        //delete from user information table
        //        objUserRepo.DeleteUsersById(objEmpl.UserInfoID);

        //        //Delete(objUI);
        //        //SubmitChanges();
        //    }
        //}
        public List<GetEquipVendorEmployeeResult> GetVendorEmployeeBySP(Int64 VendorID)
        {

            var objlist = db.GetEquipVendorEmployee(VendorID).ToList();

            return objlist.ToList<GetEquipVendorEmployeeResult>();

        }

        public List<EquipmentVendorDocument> GetVendorEmpDoc(Int64 VendorEmployeeID)
        {

            IQueryable<EquipmentVendorDocument> qry = from c in db.EquipmentVendorDocuments
                                                      where c.DocumentFor == "VendorEmployee"
                                                      orderby c.DocumentId
                                                      select c;
            List<EquipmentVendorDocument> objList = qry.ToList();
            return objList;

        }
        #endregion

        #region MenuAccess
        IQueryable<EquipmentMenuAccess> GetAllMenuAccess()
        {
            IQueryable<EquipmentMenuAccess> qry = from e in db.EquipmentMenuAccesses
                                                  select e;
            return qry;
        }

        /// <summary>
        /// Update BY : Gaurang Pathak
        /// </summary>
        /// <param name="VendorEmployeeID"></param>
        /// <returns></returns>
        public List<EquipmentMenuAccess> GetMenusByVendorEmployeeID(Int64 VendorEmployeeID)
        {
            //return GetAllMenuAccess().Where(s => s.VendorEmployeeID == VendorEmployeeID).ToList();
            return (from e in db.EquipmentMenuAccesses
                    where e.VendorEmployeeID == VendorEmployeeID
                    select e).ToList();
        }

        public List<VendorEmpMenuAccessResult> GetMenusByVendorEmpIdWithPath(Int64 VendorEmployeeID)
        {
            return (from e in db.EquipmentMenuAccesses
                    join m in db.INC_MenuPrivileges
                    on e.MenuPrivilegeID equals m.iMenuPrivilegeID
                    where e.VendorEmployeeID == VendorEmployeeID
                    select new VendorEmpMenuAccessResult
                    {
                        IconPath = m.PageUrl,
                        VendorEmpMenuAccessID = e.MenuAccessID,
                        VendorEmployeeID = e.VendorEmployeeID,
                        MenuPrivilegeID = e.MenuPrivilegeID
                    }
                  ).ToList<VendorEmpMenuAccessResult>();
        }
        public class VendorEmpMenuAccessResult
        {
            public long VendorEmpMenuAccessID { get; set; }
            public long VendorEmployeeID { get; set; }
            public long MenuPrivilegeID { get; set; }
            public string IconPath { get; set; }
        }
        #endregion

        #region ManageEmail
        IQueryable<EquipmentEmail> GetAllEquipmentEmail()
        {
            IQueryable<EquipmentEmail> qry = from e in db.EquipmentEmails
                                             select e;
            return qry;
        }

        public List<EquipmentEmail> GetEmailControl()
        {
            IQueryable<EquipmentEmail> qry = GetAllEquipmentEmail();
            return qry.ToList<EquipmentEmail>();
        }

        IQueryable<EquipmentManageEmail> GetAllEquipmentEmailDetail()
        {
            IQueryable<EquipmentManageEmail> qry = from e in db.EquipmentManageEmails
                                                   select e;
            return qry;
        }


        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <returns></returns>
        public List<EquipmentManageEmail> GetEmailRightsByUserInfoID(Int64 UserInfoID)
        {
            //return GetAllEquipmentEmailDetail().Where(s => s.UserInfoID == UserInfoID).ToList();
            return (from e in db.EquipmentManageEmails
                    where e.UserInfoID == UserInfoID
                    select e).ToList();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <param name="ManageEmailID"></param>
        /// <returns></returns>
        public bool CheckEmailAuthentication(Int64 UserInfoID, Int64 ManageEmailID)
        {
            //bool Result = false;
            //IQueryable<EquipmentManageEmail> qryI = GetAllEquipmentEmailDetail();
            //qryI = qryI.Where(s => s.UserInfoID == UserInfoID && s.EquipmentEmailID == ManageEmailID);
            //EquipmentManageEmail objI = GetSingle(qryI.ToList());
            //return Result = objI != null ? true : false;

            return (from e in db.EquipmentManageEmails
                    where e.UserInfoID == UserInfoID && e.EquipmentEmailID == ManageEmailID
                    select e).Count() > 0;
        }

        #endregion

        #region ManageDropDown
        IQueryable<EquipmentDropDown> GetAllEquipmentDropDown()
        {
            IQueryable<EquipmentDropDown> qry = from e in db.EquipmentDropDowns
                                                select e;
            return qry;
        }

        public List<EquipmentDropDown> GetDropDownControl()
        {
            IQueryable<EquipmentDropDown> qry = GetAllEquipmentDropDown();
            return qry.ToList<EquipmentDropDown>();
        }

        IQueryable<EquipmentManageDropDown> GetAllEquipmentDropDownDetail()
        {
            IQueryable<EquipmentManageDropDown> qry = from e in db.EquipmentManageDropDowns
                                                      select e;
            return qry;
        }

        public List<GetEquipDropdownbyUserInfoIDResult> GetDropDownsByUserInfoID(Int64 UserInfoID)
        {
            var objlist = db.GetEquipDropdownbyUserInfoID(UserInfoID).ToList();
            return objlist.ToList<GetEquipDropdownbyUserInfoIDResult>();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <returns></returns>
        public List<EquipmentManageDropDown> GetDropDownRightsByUserInfoID(Int64 UserInfoID)
        {
            //return GetAllEquipmentDropDownDetail().Where(s => s.UserInfoID == UserInfoID).ToList();

            return (from e in db.EquipmentManageDropDowns
                    where e.UserInfoID == UserInfoID
                    select e).ToList();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <param name="ManageDropDownID"></param>
        /// <returns></returns>
        public bool CheckDropDownAuthentication(Int64 UserInfoID, Int64 ManageDropDownID)
        {
            //bool Result = false;
            //IQueryable<EquipmentManageDropDown> qryI = GetAllEquipmentDropDownDetail();
            //qryI = qryI.Where(s => s.UserInfoID == UserInfoID && s.EquipmentDropDownID == ManageDropDownID);
            //EquipmentManageDropDown objI = GetSingle(qryI.ToList());
            //return Result = objI != null ? true : false;

            return (from e in db.EquipmentManageDropDowns
                    where e.UserInfoID == UserInfoID && e.EquipmentDropDownID == ManageDropDownID
                    select e).Count() > 0;
        }

        /// <summary>
        /// Country ID for Base Stations in Report Access
        /// </summary>
        /// <param name="VendorEmployeeID"></param>
        /// <returns></returns>
        public Int64 GetVendorEmpCountrybyID(Int64 VendorEmployeeID)
        {
            Int64? UserInfoID = db.EquipmentVendorEmployees.SingleOrDefault(s => s.VendorEmployeeID == VendorEmployeeID).UserInfoID;
            Int64? CountryID = db.UserInformations.SingleOrDefault(u => u.UserInfoID == UserInfoID && u.IsDeleted == false).CountryId;
            return Convert.ToInt64(CountryID);
        }
        #endregion

        #region UserSetting
        public List<EquipmentUserSetting> GetUserSettingMenu()
        {
            IQueryable<EquipmentUserSetting> qry = from e in db.EquipmentUserSettings.OrderBy(x => x.UserSettingID)
                                                   select e;
            return qry.ToList();
        }
        public List<EquipmentUserSettingDetail> GetUserSettingByID(Int64 VendorEmployeeID)
        {
            IQueryable<EquipmentUserSettingDetail> qry = from e in db.EquipmentUserSettingDetails
                                                         select e;
            return qry.Where(s => s.VendorEmployeeID == VendorEmployeeID).ToList();
        }
        public bool IsAuthentic(Int64 UserInfoID)
        {
            try
            {
                Int64 VendorEmpID = db.EquipmentVendorEmployees.FirstOrDefault(x => x.UserInfoID == UserInfoID).VendorEmployeeID;

                var matched = (from c in db.GetTable<EquipmentUserSettingDetail>()
                               where c.VendorEmployeeID == VendorEmpID
                               select c).FirstOrDefault();
                if (matched != null)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return true;
            }

        }
        #endregion

        /// <summary>
        /// Created by Prashant - 29th March 2013 for GSE New Logic
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <param name="VendorEmployeeID"></param>
        /// <returns></returns>
        public List<INC_MenuPrivilege> GetMenusByVendorEmployeeIDUserInfoID(Int64 UserInfoID, string MenuType, string UserType)
        {

            List<INC_MenuPrivilege> obj = (from c in db.EquipmentMenuAccesses
                                           join m in db.INC_MenuPrivileges on c.MenuPrivilegeID equals m.iMenuPrivilegeID
                                           join d in db.EquipmentVendorEmployees on c.VendorEmployeeID equals d.VendorEmployeeID
                                           join u in db.UserInformations on d.UserInfoID equals u.UserInfoID
                                           where d.UserInfoID == UserInfoID && m.sMenuType == MenuType && m.sUserType == UserType
                                           select m
                                       ).ToList();
            return obj;
        }

        /// <summary>
        /// Get AssociateCustomer for the Vendor
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <returns></returns>
        public List<EquipmentVendorMaster> GetCustomerCompany(Int64? EquipmentVendorID)
        {
            var result = db.EquipmentVendorMasters.Where(e => e.IsCustomer == true).ToList();
            return result;
        }

        public List<EquipmentAssociateCustomerForVendor> GetAssociateCustomerByVendorID(Int64? EquipmentVendorID)
        {
            var result = db.EquipmentAssociateCustomerForVendors.Where(e => e.EquipmentVendorID == EquipmentVendorID).ToList();
            return result;
        }

        public List<GetGSECompanyListResult> GetCompanyList(Int64? EquipmentVendorID, Int64? CompanyID,bool IsEmployee)
        {
            var objlist = db.GetGSECompanyList(EquipmentVendorID, CompanyID,IsEmployee).ToList();

            return objlist.ToList<GetGSECompanyListResult>();

        }
        public GSEUserDetails GetGSEUserDetailsByUserInfoID(Int64 UserInfoID)
        {
            //IQueryable<EquipmentVendorEmployee> qry = GetAllVendorEmployeeDetail().Where(s => s.UserInfoID == UserInfoID);
            //EquipmentVendorEmployee obj = GetSingle(qry.ToList());

            var obj = (from c in db.EquipmentVendorMasters
                       join e in db.EquipmentVendorEmployees on c.EquipmentVendorID equals e.VendorID
                       join u in db.UserInformations on e.UserInfoID equals u.UserInfoID
                       where e.UserInfoID == UserInfoID
                       select new GSEUserDetails
                       {
                           UserID = u.UserInfoID,
                           CurrentUserType = u.Usertype,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           LoginEmail = u.LoginEmail,
                           BaseStationIds = e.BaseStationID,
                           CompanyID = c.CompanyID,
                           VendorID = c.EquipmentVendorID,
                           VenoderEmployeeID = e.VendorEmployeeID,
                           IsCustomer = c.IsCustomer,
                           AssociateCustomerID = string.Join(",", (from a in db.EquipmentAssociateCustomerForVendors
                                                                   where a.EquipmentVendorID == c.EquipmentVendorID
                                                                   select (a.AssociateCustomerID.HasValue ? a.AssociateCustomerID.Value.ToString() : "")).ToList().ToArray())
                       }).FirstOrDefault();
            return obj;
        }
        
        
    }

}