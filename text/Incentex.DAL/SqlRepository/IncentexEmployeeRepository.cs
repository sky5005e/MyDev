using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class IncentexEmployeeRepository : RepositoryBase
    {
        IQueryable<IncentexEmployee> GetAllQuery()
        {
            IQueryable<IncentexEmployee> qry = from ie in db.IncentexEmployees                                               
                                               select ie;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public IncentexEmployee GetById(Int64 EmployeeId)
        {
            return db.IncentexEmployees.FirstOrDefault(s => s.IncentexEmployeeID == EmployeeId);
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoId"></param>
        /// <returns></returns>
        public IncentexEmployee GetEmployeeByUserInfoId(Int64 UserInfoId)
        {
            return db.IncentexEmployees.FirstOrDefault(s => s.UserInfoID == UserInfoId);
        }
        
        /// <summary>
        /// Get All Incentex Employee including Super Admin
        /// </summary>
        /// <returns></returns>
        public List<IEListResults> GetEmployeeSAdmin()
        {
            return
            (from ie in db.IncentexEmployees
             join u in db.UserInformations on ie.UserInfoID equals u.UserInfoID
             join country in db.INC_Countries on new { iCountryID = (u.CountryId.Value == null ? -1 : u.CountryId.Value) } equals new { iCountryID = country.iCountryID }
             join state in db.INC_States on new { iStateID = (u.StateId.Value == null ? -1 : u.StateId.Value) } equals new { iStateID = state.iStateID }
             join city in db.INC_Cities on new { iCityID = (u.CityId.Value == null ? -1 : u.CityId.Value) } equals new { iCityID = city.iCityID }
             where u.IsDeleted == false && u.UserInfoID != 2375
             select new IEListResults
             {
                 EmployeeName = (u.FirstName + " " + u.LastName),
                 Country = country.sCountryName,
                 State = state.sStatename,
                 City = city.sCityName,
                 Mobile = u.Mobile,
                 IsDirectEmployee = (ie.isDirectEmployee == 1) ? "Direct Company Employee" : "Independent Contractor",
                 IncentexEmployeeID = ie.IncentexEmployeeID,
                 FirstName = u.FirstName,
                 LastName = u.LastName,
                 UserInfoID = u.UserInfoID
             }).ToList<IEListResults>();
        }
        
        /// <summary>
        /// Get All Incentex Employee excluding Super Admin
        /// </summary>
        /// <returns></returns>
        public List<IEListResults> GetAllEmployee()
        {
            return
            (from ie in db.IncentexEmployees
             join u in db.UserInformations on ie.UserInfoID equals u.UserInfoID
             join country in db.INC_Countries on new { iCountryID = (u.CountryId.Value == null ? -1 :u.CountryId.Value) } equals new { iCountryID = country.iCountryID }
             join state in db.INC_States on new { iStateID = (u.StateId.Value == null ? -1 : u.StateId.Value) } equals new { iStateID = state.iStateID }
             join city in db.INC_Cities on new { iCityID = (u.CityId.Value == null ? -1 : u.CityId.Value) } equals new { iCityID = city.iCityID }
             where ie.MemberRole != "Super Admin" && u.IsDeleted == false
             select new IEListResults
             {
                 EmployeeName = (u.FirstName + " " + u.LastName),
                 Country = country.sCountryName,
                 State = state.sStatename,
                 City = city.sCityName,
                 Mobile = u.Mobile,
                 IsDirectEmployee = (ie.isDirectEmployee == 1) ? "Direct Company Employee" : "Independent Contractor",
                 IncentexEmployeeID=ie.IncentexEmployeeID,
                 FirstName = u.FirstName,
                 LastName = u.LastName,
                 Email = u.Email, 
                 UserInfoID=u.UserInfoID
             }).ToList<IEListResults>();
        }

        public class IEListResults
        {
            public string EmployeeName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Mobile { get; set; }
            public string IsDirectEmployee { get; set; }
            public long IncentexEmployeeID { get; set; }
            public long UserInfoID { get; set; }
            public string Email { get; set; }
        }

        public IncentexEmployee DeleteIncentexEmployeeById(Int64 EmployeeId)
        {
            IncentexEmployee objCE = GetById(EmployeeId);
            Delete(objCE);
            SubmitChanges();
            return objCE;
        }

        public void Delete(Int64 EmployeeId, Int64? DeletedBy)
        {
            IncentexEmployee objEmpl = GetById(EmployeeId);

            if (objEmpl != null)
            {
                //delete for Document
                DocumentRepository objDocumentRepository = new DocumentRepository();
                objDocumentRepository.DeleteEmployeeDocuments(Incentex.DAL.Common.DAEnums.DocumentForType.IncentexEmployee.ToString(), objEmpl.IncentexEmployeeID);

                //Delete from NoteHistory
                NotesHistoryRepository objNotesHistoryRepository = new NotesHistoryRepository();
                objNotesHistoryRepository.Delete(objEmpl.IncentexEmployeeID, Incentex.DAL.Common.DAEnums.NoteForType.IncentexEmployee);

                //Delete from Menu Access
                //Delete from IncentexEmpMenuAccess
                IncentexEmpMenuAccessRepository objCmpMenuAccesRepos = new IncentexEmpMenuAccessRepository();
                IncEmpMenuAccess objCmpMenuAccessfordel = new IncEmpMenuAccess();

                List<IncEmpMenuAccess> lst = objCmpMenuAccesRepos.GetMenusByEmployeeId(objEmpl.IncentexEmployeeID);

                foreach (IncEmpMenuAccess l in lst)
                {
                    objCmpMenuAccesRepos.Delete(l);
                    objCmpMenuAccesRepos.SubmitChanges();
                }

                //Delete from Data Access
                IncentexEmpDataAccessRepository objCmpDataAccesRepos = new IncentexEmpDataAccessRepository();
                IncEmpDataAccess objCmpDataAccessfordel = new IncEmpDataAccess();

                List<IncEmpDataAccess> lstData = objCmpDataAccesRepos.GetDataByEmployeeId(objEmpl.IncentexEmployeeID);

                foreach (IncEmpDataAccess l in lstData)
                {
                    objCmpDataAccesRepos.Delete(l);
                    objCmpDataAccesRepos.SubmitChanges();
                }

                //Delete from main table
                DeleteIncentexEmployeeById(objEmpl.IncentexEmployeeID);

                //Delete from user information
                UserInformationRepository objUserRepo = new UserInformationRepository();
                objUserRepo.DeleteUsersById(objEmpl.UserInfoID, DeletedBy);

                SubmitChanges();
            }

        }

        public List<GetEmailSettingsForIncentexEmployeesByIncentexEmployeeIDResult> GetEmailSettingsByEmployeeID(Int64 IncentexEmployeeID)
        {
            return db.GetEmailSettingsForIncentexEmployeesByIncentexEmployeeID(IncentexEmployeeID).ToList();
        }
    }
}
