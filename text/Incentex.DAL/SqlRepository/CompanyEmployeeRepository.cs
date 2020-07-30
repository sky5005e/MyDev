using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class CompanyEmployeeRepository : RepositoryBase
    {
        public IQueryable<CompanyEmployee> GetAllQuery()
        {
            IQueryable<CompanyEmployee> qry = from c in db.CompanyEmployees
                                              select c;
            return qry;
        }

        public CompanyEmployee GetById(Int64 EmployeeId)
        {
            return db.CompanyEmployees.FirstOrDefault(s => s.CompanyEmployeeID == EmployeeId);
        }

        /// <summary>
        /// Date Created: 24-Dec-2010 by Shehzad
        /// Return all the payment options in checkout page
        /// for the given user
        /// </summary>
        /// <param name="EmployeeId">Sets the Employee Id</param>
        /// <returns>List of all payment options</returns>
        public List<CompanyEmployeePaymentOptionsResult> GetAllPaymentsOptions(Int64 EmployeeId)
        {
            return db.CompanyEmployeePaymentOptions(EmployeeId).ToList();
        }

        public void DeleteCompanyEmployeeById(Int64 CompanyEmployeeId)
        {
            CompanyEmployee objCE = GetById(CompanyEmployeeId);
            Delete(objCE);
            SubmitChanges();
        }

        /// <summary>
        /// Created by Shehzad 17-Jan-2011
        /// Gets the Employee record by its User Information ID
        /// </summary>
        /// <param name="UserInfoId">User Id</param>
        /// <returns>Single record from the Company Employee table</returns>
        public CompanyEmployee GetByUserInfoId(Int64 UserInfoId)
        {
            return db.CompanyEmployees.FirstOrDefault(le => le.UserInfoID == UserInfoId);
        }

        public List<SelectAnniversaryCreditProgramResult> GetAnniversaryProgram(int userinfoid, int StoreId)
        {
            return db.SelectAnniversaryCreditProgram(userinfoid, StoreId).ToList();
        }


        public Boolean DeleteUser(Int64 EmployeeId, Int64? DeletedBy)
        {
            CompanyEmployee objEmpl = GetById(EmployeeId);
            UserInformationRepository objUserRepo = new UserInformationRepository();
            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
            CompanyEmpMenuAccessRepository objCmpMenuAccesRepos = new CompanyEmpMenuAccessRepository();
            CompanyEmpManageEmailRepository objCompanyEmpManageEmailRepository = new CompanyEmpManageEmailRepository();
            NotesHistoryRepository objNotesHistoryRepository = new NotesHistoryRepository();
            if (objEmpl != null)
            {
                if (db.OrderMOASSystems.Where(x => x.ManagerUserInfoID == objEmpl.UserInfoID).Count() == 0 && db.Orders.Where(x => x.UserId == objEmpl.UserInfoID).Count() == 0)
                {
                    //delete from note history table
                    //objNotesHistoryRepository.Delete(objEmpl.CompanyEmployeeID, Incentex.DAL.Common.DAEnums.NoteForType.CompanyEmployee);

                    //Delete from EmployeeMenuaccess
                    //List<CompanyEmpMenuAccess> lst = objCmpMenuAccesRepos.getMenuByUserType(objEmpl.CompanyEmployeeID);
                    //foreach (CompanyEmpMenuAccess l in lst)
                    //{
                    //    objCmpMenuAccesRepos.Delete(l);
                    //    objCmpMenuAccesRepos.SubmitChanges();
                    //}

                    //Delete from CompanyEmpManageEmail
                    //List<CompanyEmpManageEmail> lstCompanyEmpManageEmail = objCompanyEmpManageEmailRepository.GetEmailRightsByUserInfoID(objEmpl.UserInfoID);
                    //foreach (CompanyEmpManageEmail l in lstCompanyEmpManageEmail)
                    //{
                    //    objCompanyEmpManageEmailRepository.Delete(l);
                    //    objCompanyEmpManageEmailRepository.SubmitChanges();
                    //}

                    //delete from company employee table
                    //objEmpRepo.DeleteCompanyEmployeeById(objEmpl.CompanyEmployeeID);

                    //delete from user information table
                    objUserRepo.DeleteUsersById(objEmpl.UserInfoID, DeletedBy);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public List<UserInformation> GetUsersByWorkgroupId(Int64 WorkgroupId, Int64 CompanyId)
        {
            return
                (from u in db.UserInformations
                 join companyemployees in db.CompanyEmployees on u.UserInfoID equals companyemployees.UserInfoID
                 where companyemployees.WorkgroupID == WorkgroupId && u.CompanyId == CompanyId
                 && u.IsDeleted == false
                 select u).ToList();
            //return GetAllQuery().Where(s => s.WorkgroupID == WorkgroupId).ToList();

        }

        public List<CompanyEmployee> GetByWorkgroupID(Int64 WorkgroupId, Int64 CompanyId)
        {
            return
                (from companyemployees in db.CompanyEmployees
                 join u in db.UserInformations on companyemployees.UserInfoID equals u.UserInfoID
                 where companyemployees.WorkgroupID == WorkgroupId && u.CompanyId == CompanyId && u.IsDeleted == false
                 select companyemployees).ToList();
        }


        public List<UserInformation> GetCAByWorkgroupId(Int64 WorkgroupId, Int64 CompanyId)
        {
            return
                (from u in db.UserInformations
                 join companyemployees in db.CompanyEmployees on u.UserInfoID equals companyemployees.UserInfoID
                 where companyemployees.WorkgroupID == WorkgroupId && u.CompanyId == CompanyId && companyemployees.isCompanyAdmin == true && u.WLSStatusId == 135
                 && u.IsDeleted == false
                 select u).ToList();


        }

        public List<UserInformation> GetCAByCompanyID(Int64 CompanyId)
        {
            return
                (from u in db.UserInformations
                 join companyemployees in db.CompanyEmployees on u.UserInfoID equals companyemployees.UserInfoID
                 where u.CompanyId == CompanyId && companyemployees.isCompanyAdmin == true && u.WLSStatusId == 135
                 && u.IsDeleted == false
                 select u).ToList();
        }

        public class MOASUserWithPriority
        {
            public string UserInfoID { get; set; }
            public string Priority { get; set; }
        }

        public Boolean CheckEmployeeIDExistence(String employeeID, Int64 companyID, Int64 userInfoID)
        {
            Int64? WLSStatusID = new LookupRepository().GetIdByLookupNameNLookUpCode("Active", "Status");

            UserInformation objUser = (from u in db.UserInformations
                                       join companyemployees in db.CompanyEmployees on u.UserInfoID equals companyemployees.UserInfoID
                                       where companyemployees.EmployeeID == employeeID && u.CompanyId == companyID
                                       && u.IsDeleted == false && u.WLSStatusId == WLSStatusID && u.UserInfoID != userInfoID
                                       select u).FirstOrDefault<UserInformation>();

            return objUser == null;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoId"></param>
        /// <param name="strID"></param>
        /// <returns></returns>
        public CompanyEmployee GetByUserInfoIdForMail(Int64 UserInfoId, long strID)
        {
            //CompanyEmployee obj = GetSingle(GetAllQuery().Where(s => s.UserInfoID == UserInfoId).ToList());

            CompanyEmployee obj = (from c in db.CompanyEmployees
                                   where c.UserInfoID == UserInfoId
                                   select c).FirstOrDefault();

            if (strID == 1)
            {
                obj.MailFlag = Convert.ToBoolean(0);
            }
            else
            {
                obj.MailFlag = Convert.ToBoolean(1);
            }
            Insert(obj);
            SubmitChanges();
            return obj;
        }

        //public CompanyEmployee InsertCampID(int Userinfoid)
        //{

        //    CompanyEmployee obj = GetSingle(GetAllQuery().Where(s => s.UserInfoID == Userinfoid).ToList());
        //    obj.CampaignID = db.Campaigns(
        //    Insert(obj);
        //    SubmitChanges();
        //    return obj;

        //}
        public void UpdateStatus(int Userid, bool MailFlag)
        {
            var obj = db.Update_MailFlagStatus(Userid, MailFlag);
        }

        public Boolean FUN_IsMOASWithCostCenterCode(Int64 UserInfoID)
        {
            return Convert.ToBoolean(db.FUN_IsMOASWithCostCenterCode(UserInfoID));
        }

        public List<GetEmployeesForReplacementUniformsByCAIDResult> GetEmployeesByBaseStationAndWorkGroupOfCA(Int64 CAUserInfoID)
        {
            return db.GetEmployeesForReplacementUniformsByCAID(CAUserInfoID).ToList();
        }

        public Boolean FUN_IsReason4ReplacementRequired(Int64 UserInfoID)
        {
            return Convert.ToBoolean(db.FUN_IsReason4ReplacementRequired(UserInfoID));
        }

        /// <summary>
        /// Added by Prashant April 2013
        /// To get the MOAS Approver List for CE
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <param name="CompanyId"></param>
        /// <param name="UniformIssuancePolicyID"></param>
        /// <returns></returns>
        public List<GetMOASApproverForCEResult> GetMOASApprover(Int64 UserInfoID, Int64? CompanyId, Int64? UniformIssuancePolicyID)
        {
            var result = db.GetMOASApproverForCE(UserInfoID, CompanyId, UniformIssuancePolicyID).ToList();
            return result;
        }

        public string GetWorkGroupMOASApproverLevel(Int64? WorkGroupID, Int64? CompanyID)
        {
            return db.FUN_GetWorkGroupPreferenceValue(WorkGroupID, CompanyID, "MOASApproverSettings");
        }

        public List<GetPendingUsersListForAdminResult> GetPendingUsersList(Int64 UserInfoID)
        {
            var result = db.GetPendingUsersListForAdmin(UserInfoID).ToList();
            return result;
        }

        /// <summary>
        /// Get Pending Users List For Admin 
        /// </summary>
        /// <param name="UserInfoID">UserInfoID</param>
        /// <param name="SortExp">The sort exp.</param>
        /// <param name="SortOrder">The sort order.</param>
        /// <returns></returns>
        public List<GetPendingUsersListForAdminResult> GetPendingUsersList(Int64 UserInfoID, Incentex.DAL.SqlRepository.RegistrationRepository.UsersSortExpType SortExp, DAEnums.SortOrderType SortOrder)
        {
            List<GetPendingUsersListForAdminResult> qry = db.GetPendingUsersListForAdmin(UserInfoID).ToList();

            switch (SortExp)
            {
                case RegistrationRepository.UsersSortExpType.CompanyName:
                    qry = qry.OrderBy(s => s.CompanyName).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.EmployeeName:
                    qry = qry.OrderBy(s => s.CompanyName).ThenBy(s => s.EmployeeName).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.WorkGroup:
                    qry = qry.OrderBy(s => s.CompanyName).ThenBy(s => s.EmployeeName).ThenBy(s => s.WorkGroup).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.BaseStationName:
                    qry = qry.OrderBy(s => s.CompanyName).ThenBy(s => s.EmployeeName).ThenBy(s => s.WorkGroup).ThenBy(s => s.BaseStation).ToList();
                    break;
            }
            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                qry.Reverse();
            }
            return qry;
        }

        public List<GetEmployeesBySearchCriteriaResult> GetEmployeesBySearchCriteria(String firstName, String lastName, Int64? workGroupID, Int64? baseStationID, Int64? systemStatusID, Int64? systemAccessID, String email, Int64? companyID, String keyWord, Int32? pageSize, Int32? pageIndex, String sortColumn, String sortDirection)
        {
            return db.GetEmployeesBySearchCriteria(firstName, lastName, workGroupID, baseStationID, systemStatusID, systemAccessID, email, companyID, keyWord, pageSize, pageIndex, sortColumn, sortDirection).ToList();
        }

        public List<GetUserApproverResult> GetUserApprover(Int64 UserInfoID, Int64? CompanyId, Int64? WorkGroupID, Int64? BaseStationID)
        {
            return db.GetUserApprover(UserInfoID, CompanyId, WorkGroupID, BaseStationID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ViewPendingUsers)).ToList();
        }

        public List<GetUserPaymentOptionsResult> GetUserPaymentOptions(Int64 userInfoID)
        {
            return db.GetUserPaymentOptions(userInfoID).ToList();
        }

        public List<UserPaymentOption> SetUserPaymentOptions(Int64 userInfoID)
        {
            return db.UserPaymentOptions.Where(le => le.UserInfoID == userInfoID).ToList();
        }

        public List<GetUserManageEmailSettingsResult> GetUserManageEmailSettings(Int64 userInfoID)
        {
            return db.GetUserManageEmailSettings(userInfoID).ToList();
        }

        public List<GetUserShippingMethodsResult> GetUserShippingMethods(Int64 userInfoID)
        {
            return db.GetUserShippingMethods(userInfoID).ToList();
        }

        public List<GetUserCategoryAccessResult> GetUserCategoryAccess(Int64 userInfoID)
        {
            return db.GetUserCategoryAccess(userInfoID).ToList();
        }

        public List<UserCategoryAccess> SetUserCategoryAccess(Int64 userInfoID)
        {
            return db.UserCategoryAccesses.Where(le => le.UserInfoID == userInfoID).ToList();
        }

        public List<GetUserMOASApproversResult> GetUserMOASApprovers(Int64 userInfoID)
        {
            return db.GetUserMOASApprovers(userInfoID).ToList();
        }

        public List<UserMOASApprover> SetUserMOASApprovers(Int64 userInfoID)
        {
            return db.UserMOASApprovers.Where(le => le.UserInfoID == userInfoID).ToList();
        }

        public UserMOASPaymentOptionFor SetUserMOASPaymentOptionFor(Int64 userInfoID)
        {
            return db.UserMOASPaymentOptionFors.FirstOrDefault(le => le.UserInfoID == userInfoID);
        }

        public List<GetUserStationsResult> GetUserStations(Int64 userInfoID)
        {
            return db.GetUserStations(userInfoID).ToList();
        }

        public List<UserStation> SetUserStations(Int64 userInfoID)
        {
            return db.UserStations.Where(le => le.UserInfoID == userInfoID).ToList();
        }

        public List<GetUserStationWorkGroupResult> GetUserStationWorkGroup(Int64 userInfoID)
        {
            return db.GetUserStationWorkGroup(userInfoID).ToList();
        }

        public List<UserStationWorkGroup> SetUserStationWorkGroup(Int64 userInfoID)
        {
            return db.UserStationWorkGroups.Where(le => le.UserInfoID == userInfoID).ToList();
        }

        public List<GetMismatchBetweenCreditBalanceAndLedgerBalanceResult> GetMismatchBetweenCreditBalanceAndLedgerBalance()
        {
            return db.GetMismatchBetweenCreditBalanceAndLedgerBalance().ToList();
        }

        public List<GetUserStationPrivilegesResult> GetUserStationPrivileges(Int64 userInfoID)
        {
            return db.GetUserStationPrivileges(userInfoID).ToList();
        }

        public List<UserStationPrivilege> SetUserStationPrivilege(Int64 userInfoID)
        {
            return db.UserStationPrivileges.Where(le => le.UserInfoID == userInfoID).ToList();
        }

        public List<GetUserReportsResult> GetUserReports(Int64 userInfoID)
        {
            return db.GetUserReports(userInfoID).ToList();
        }

        public List<UserReport> SetUserReports(Int64 userInfoID)
        {
            return db.UserReports.Where(le => le.UserInfoID == userInfoID).ToList();
        }

        public List<GetUserSubReportsResult> GetUserSubReports(Int64 userInfoID)
        {
            return db.GetUserSubReports(userInfoID).ToList();
        }

        public List<UserSubReport> SetUserSubReports(Int64 userInfoID)
        {
            return db.UserSubReports.Where(le => le.UserInfoID == userInfoID).ToList();
        }

        public List<GetUserReportWorkGroupsResult> GetUserReportWorkGroups(Int64 userInfoID)
        {
            return db.GetUserReportWorkGroups(userInfoID).ToList();
        }

        public List<UserReportWorkGroup> SetUserReportWorkGroups(Int64 userInfoID)
        {
            return db.UserReportWorkGroups.Where(le => le.UserInfoID == userInfoID).ToList();
        }

        public List<GetUserReportStationsResult> GetUserReportStations(Int64 userInfoID)
        {
            return db.GetUserReportStations(userInfoID).ToList();
        }

        public List<UserReportStation> SetUserReportStations(Int64 userInfoID)
        {
            return db.UserReportStations.Where(le => le.UserInfoID == userInfoID).ToList();
        }

        public List<GetUserReportPriceLevelsResult> GetUserReportPriceLevels(Int64 userInfoID)
        {
            return db.GetUserReportPriceLevels(userInfoID).ToList();
        }

        public List<UserReportPriceLevel> SetUserReportPriceLevels(Int64 userInfoID)
        {
            return db.UserReportPriceLevels.Where(le => le.UserInfoID == userInfoID).ToList();
        }
    }
}