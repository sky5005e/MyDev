using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class SupplierEmployeeRepository : RepositoryBase
    {
        IQueryable<SupplierEmployee> GetAllQuery()
        {
            IQueryable<SupplierEmployee> qry = from c in db.SupplierEmployees
                                               select c;
            return qry;
        }
        public SupplierEmployee GetById(Int64 SupplierEmployeeID)
        {
            SupplierEmployee obj = GetSingle(GetAllQuery().Where(s => s.SupplierEmployeeID == SupplierEmployeeID).ToList());
            return obj;
        }
        public SupplierEmployee GetEmployeeByUserInfoId(Int64 UserInfoId)
        {
            SupplierEmployee obj = GetSingle(GetAllQuery().Where(s => s.UserInfoID == UserInfoId).ToList());
            return obj;
        }

        public List<SupplierEmployee> GetBySupplierId(Int64 SupplierId)
        {
            IQueryable<SupplierEmployee> qry = GetAllQuery().Where(e => e.SupplierID == SupplierId).OrderBy(e => e.SupplierEmployeeID);
            return qry.ToList();
        }



        public int GetCountSupplierId(Int64 SupplierId)
        {
            IQueryable<SupplierEmployee> qry = GetAllQuery().Where(e => e.SupplierID == SupplierId).OrderBy(e => e.SupplierEmployeeID);
            return qry.Count();
        }


        /// <summary>
        /// GetById
        /// Reterieve  a SupplierEmployee by Supplier ID And SupplierEmployeeid
        /// </summary>
        /// Nagmani 14/09/2010
        /// <param name="SupplierdD"></param>
        /// <param name="SupplierEmployeeiD"></param>
        /// <param name="AccountType"></param>
        public SupplierEmployee GetBySupplierAndSuppEmpId(Int64 SupplierdD, Int64 SupplierEmployeeiD)
        {
            SupplierEmployee obj = GetSingle(GetAllQuery().Where(C => C.SupplierEmployeeID == SupplierEmployeeiD && C.SupplierID == SupplierdD).ToList());
            return obj;
        }

        /// <summary>
        /// GetById
        /// Reterieve  List of SupplierEmployee by SupplierCompanyID
        /// </summary>
        /// Nagmani 14/09/2010
        /// <param name="SupplierEmployeeiD"></param>

        //public List<SupplierEmployee> GetBySupplierEmployeeId(Int64 SupplierEmployeeiD)
        //{
        //    List<SupplierEmployee> objList = GetAllQuery().Where(s => s.SupplierEmployeeID == SupplierEmployeeiD).ToList();
        //    return objList;
        //}

        /// <summary>
        /// SupplierEmpDetails()
        /// Reterive All supplier employee
        /// from table SupplierEmployee.
        /// Nagmani 15/09/2010
        /// </summary>
        /// <returns></returns>

        public List<SupplierEmployee> SupplierEmpDetails(int supplierid)
        {
            return (from ords in db.GetTable<SupplierEmployee>()
                    where ords.SupplierID == supplierid
                    orderby ords.SupplierEmployeeID ascending
                    select ords
                    ).ToList();

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
            public long SupplierEmployeeID { get; set; }
            public long UserInfoID { get; set; }
        }

        public List<IEListResults> GetAllSupplierEmployee(int supplierid)
        {
            return
            (from ie in db.SupplierEmployees
             join u in db.UserInformations on ie.UserInfoID equals u.UserInfoID
             join country in db.INC_Countries on new { iCountryID = (u.CountryId.Value == null ? -1 : u.CountryId.Value) } equals new { iCountryID = country.iCountryID }
             join state in db.INC_States on new { iStateID = (u.StateId.Value == null ? -1 : u.StateId.Value) } equals new { iStateID = state.iStateID }
             join city in db.INC_Cities on new { iCityID = (u.CityId.Value == null ? -1 : u.CityId.Value) } equals new { iCityID = city.iCityID }
             where ie.SupplierID == supplierid && u.IsDeleted == false
             select new IEListResults
             {
                 EmployeeName = (u.FirstName + " " + u.LastName),
                 Country = country.sCountryName,
                 State = state.sStatename,
                 City = city.sCityName,
                 Mobile = u.Mobile,
                 IsDirectEmployee = (ie.isDirectEmployee == 1) ? "Direct Company Employee" : "Independent Contractor",
                 SupplierEmployeeID = ie.SupplierEmployeeID,
                 FirstName = u.FirstName,
                 LastName = u.LastName,
                 UserInfoID = u.UserInfoID
             }).ToList<IEListResults>();
        }


        /// <summary>
        /// Delete a SupplierEmployee by SupplierEmployeeID
        /// </summary>
        /// Nagmani kumar 15/09/2010
        /// <param name="SupplierEmployeeID"></param>
        public void DeleteSupplierEmployee(string SupplierEmployeeID)
        {
            var matchedSupEmp = (from c in db.GetTable<SupplierEmployee>()
                                 where c.SupplierEmployeeID == Convert.ToInt32(SupplierEmployeeID)
                                 select c).SingleOrDefault();
            try
            {
                if (matchedSupEmp != null)
                {
                    db.SupplierEmployees.DeleteOnSubmit(matchedSupEmp);

                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void Delete(Int64 SupplierEmployeeID, Int64? DeletedBy)
        {
            SupplierEmployee objEmpl = GetById(SupplierEmployeeID);

            if (objEmpl != null)
            {
                //delete for Document
                SupplierDocumentRepository objDocumentRepository = new SupplierDocumentRepository();
                objDocumentRepository.DeleteEmployeeDocuments(Incentex.DAL.Common.DAEnums.DocumentForType.SupplierEmployee.ToString(), objEmpl.SupplierEmployeeID);

                //Delete from NoteHistory
                NotesHistoryRepository objNotesHistoryRepository = new NotesHistoryRepository();
                objNotesHistoryRepository.Delete(objEmpl.SupplierEmployeeID, Incentex.DAL.Common.DAEnums.NoteForType.SupplierEmployee);

                //Delete from Menu Access
                //Delete from IncentexEmpMenuAccess
                SupplierMenuAccessRepository objCmpMenuAccesRepos = new SupplierMenuAccessRepository();
                SupplierEmpMenuAccess objCmpMenuAccessfordel = new SupplierEmpMenuAccess();

                List<SupplierEmpMenuAccess> lst = objCmpMenuAccesRepos.GetMenusBySupplierEmployeeID(objEmpl.SupplierEmployeeID);

                foreach (SupplierEmpMenuAccess l in lst)
                {
                    objCmpMenuAccesRepos.Delete(l);
                    objCmpMenuAccesRepos.SubmitChanges();
                }

                //Delete from Data Access
                SupplierDataAccessRepository objCmpDataAccesRepos = new SupplierDataAccessRepository();
                SupplierEmpDataAccess objCmpDataAccessfordel = new SupplierEmpDataAccess();

                List<SupplierEmpDataAccess> lstData = objCmpDataAccesRepos.GetDataBySupplierEmpId(objEmpl.SupplierEmployeeID);

                foreach (SupplierEmpDataAccess l in lstData)
                {
                    objCmpDataAccesRepos.Delete(l);
                    objCmpDataAccesRepos.SubmitChanges();
                }

                //Delete from main table
                DeleteSupplierEmployee(Convert.ToString(objEmpl.SupplierEmployeeID));

                //Delete from user information
                UserInformationRepository objUserRepo = new UserInformationRepository();
                objUserRepo.DeleteUsersById(Convert.ToInt64(objEmpl.UserInfoID), DeletedBy);

                SubmitChanges();
            }

        }
        public int CheckDuplicate(string EmployeeName, int? SupplierId, int? SupplierEmployeeId, string mode)
        {
            return (int)db.INC_SelectSupplierEmployeeDuplicate(EmployeeName, SupplierId, SupplierEmployeeId, mode).SingleOrDefault().IsDuplicate;
        }

        public Supplier GetSupplierBySupplierEmployeeUserInfoID(Int64 UserInfoID)
        {
            Int64 SupplierID = db.SupplierEmployees.FirstOrDefault(le => le.UserInfoID == UserInfoID).SupplierID;
            return db.Suppliers.FirstOrDefault(le => le.SupplierID == SupplierID);
        }
    }
}

