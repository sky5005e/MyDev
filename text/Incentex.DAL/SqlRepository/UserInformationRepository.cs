using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace Incentex.DAL.SqlRepository
{
    public class UserInformationRepository : RepositoryBase
    {
        public IQueryable<UserInformation> GetAllQuery()
        {
            IQueryable<UserInformation> qry = from c in db.UserInformations
                                              where c.IsDeleted == false
                                              select c;
            return qry;
        }

        public UserInformation GetById(Int64? UserInfoId)
        {
            UserInformation obj = db.UserInformations.FirstOrDefault(u => u.UserInfoID == UserInfoId && u.IsDeleted == false);
            return obj;
        }

        public UserInformation AuthenticateUser(String username, String password)
        {
            Int64? WLSStatusID = new LookupRepository().GetIdByLookupNameNLookUpCode("Active", "Status");
            UserInformation obj = db.UserInformations.FirstOrDefault(u => u.LoginEmail == username && u.Password == password && (u.WLSStatusId == WLSStatusID || u.WLSStatusId == null) && u.IsDeleted == false);
            return obj;
        }

        public Boolean CheckEmailExistence(String email, Int64 userInfoID)
        {
            Int64? WLSStatusID = new LookupRepository().GetIdByLookupNameNLookUpCode("Active", "Status");
            UserInformation objUser = db.UserInformations.FirstOrDefault(u => u.LoginEmail == email && u.WLSStatusId == WLSStatusID && u.IsDeleted == false && u.UserInfoID != userInfoID);
            return objUser == null;
        }

        public GetSAPCompanyCodeIDResult GetSAPCompanyCodeID(Int64 UserInfoID)
        {
            return db.GetSAPCompanyCodeID(UserInfoID).FirstOrDefault();
        }

        /// <summary>
        /// return true if email exist
        /// amit 23Sep10
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <param name="NewEmail"></param>
        /// <returns>true if email exist</returns>
        public bool CheckUniqueEmail(Int64 UserInfoID, String NewEmail)
        {
            Int64? WLSStatusID = new LookupRepository().GetIdByLookupNameNLookUpCode("Active", "Status");
            UserInformation obj = GetById(UserInfoID);
            UserInformation objEmail = db.UserInformations.FirstOrDefault(u => u.LoginEmail == NewEmail && u.UserInfoID != UserInfoID && u.WLSStatusId == WLSStatusID && u.IsDeleted == false);

            if (obj == null)
            {
                if (objEmail != null)
                {
                    return true; // email exist
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (objEmail != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void DeleteUsersById(Int64 UserID, Int64? DeletedBy)
        {
            UserInformation objUser = GetById(UserID);
            objUser.IsDeleted = true;
            objUser.DeletedBy = DeletedBy;
            objUser.DeletedDate = DateTime.Now;
            SubmitChanges();
        }

        //Search Users
        public List<SearchResults> getUsers(String UserName, String LastName, String Telephone, String Mobile, String Email, Int32 usertype, String Companyname, String EmployeeID, String EmployeeType, String BaseStation, String WorkGroup, String Gender)
        {
            if (usertype == 3 || usertype == 4 || usertype == 7)
            {
                return (from u in db.UserInformations
                        join companies in db.Companies on new { CompanyID = Convert.ToInt64(u.CompanyId) } equals new { CompanyID = companies.CompanyID }
                        join companyemployees in db.CompanyEmployees on u.UserInfoID equals companyemployees.UserInfoID
                        join basestationtbl in db.INC_BasedStations on companyemployees.BaseStation equals basestationtbl.iBaseStationId
                        join employeetype in db.INC_Lookups on companyemployees.EmployeeTypeID equals employeetype.iLookupID into employeeGroup
                        from lastresult in employeeGroup.DefaultIfEmpty()
                        where
                        SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                        SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                            // SqlMethods.Like(u.Telephone, "%" + Telephone + "") &&
                            // SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
                        SqlMethods.Like(u.LoginEmail, "%" + Email + "%") &&
                        SqlMethods.Like(companyemployees.EmployeeID, "%" + EmployeeID + "%") &&
                        SqlMethods.Like(companies.CompanyName, "%" + Companyname + "%") &&
                        u.Usertype == usertype &&
                       ((BaseStation != null && BaseStation != "") ? basestationtbl.iBaseStationId.ToString() == BaseStation : true) &&
                            //SqlMethods.Like(basestationtbl.sBaseStation != null ? basestationtbl.sBaseStation : "", "%" + BaseStation + "%") &&
                        SqlMethods.Like(lastresult.sLookupName != null ? lastresult.sLookupName : "", "%" + EmployeeType + "%") &&
                        SqlMethods.Like(companyemployees.WorkgroupID.ToString(), WorkGroup != "" && WorkGroup != "0" ? WorkGroup : companyemployees.WorkgroupID.ToString()) &&
                        SqlMethods.Like(companyemployees.GenderID.ToString(), Gender != "" && Gender != "0" ? Gender : companyemployees.GenderID.ToString()) &&
                        u.IsDeleted == false
                        select new SearchResults
                        {
                            FullName = (u.FirstName + " " + u.LastName),
                            Contact = u.Address2 == "" ? (u.Address1 + " " + u.Address2) : u.Address1,
                            Email = u.LoginEmail,
                            UserType = u.Usertype,
                            Telephone = u.Telephone,
                            CompanyName = companies.CompanyName,
                            CompanyEmployeeId = companyemployees.CompanyEmployeeID,
                            UserInformationId = u.UserInfoID
                        }
                     ).ToList<SearchResults>();
            }
            //else if (usertype == 4 || usertype == 7)
            //{
            //    return (from u in db.UserInformations
            //            join companies in db.Companies on new { CompanyID = Convert.ToInt64(u.CompanyId) } equals new { CompanyID = companies.CompanyID }
            //            join companyemployees in db.CompanyEmployees on u.UserInfoID equals companyemployees.UserInfoID
            //            join basestationtbl in db.INC_BasedStations on companyemployees.BaseStation equals basestationtbl.iBaseStationId
            //            join employeetype in db.INC_Lookups on companyemployees.EmployeeTypeID equals employeetype.iLookupID into employeeGroup
            //            from lastresult in employeeGroup.DefaultIfEmpty()
            //            where
            //            SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
            //            SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
            //                // SqlMethods.Like(u.Telephone, "%" + Telephone + "") &&
            //                //SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
            //            SqlMethods.Like(u.LoginEmail, "%" + Email + "%") &&
            //            SqlMethods.Like(companyemployees.EmployeeID, "%" + EmployeeID + "%") &&
            //            SqlMethods.Like(companies.CompanyName, "%" + Companyname + "%") &&
            //            u.Usertype == usertype &&
            //            SqlMethods.Like(basestationtbl.sBaseStation != null ? basestationtbl.sBaseStation : "", "%" + BaseStation + "%") &&
            //            SqlMethods.Like(lastresult.sLookupName != null ? lastresult.sLookupName : "", "%" + EmployeeType + "%") &&
            //            SqlMethods.Like(companyemployees.WorkgroupID.ToString(), WorkGroup != "" && WorkGroup != "0" ? WorkGroup : companyemployees.WorkgroupID.ToString()) &&
            //            SqlMethods.Like(companyemployees.GenderID.ToString(), Gender != "" && Gender != "0" ? Gender : companyemployees.GenderID.ToString()) &&
            //            u.IsDeleted == false
            //            select new SearchResults
            //            {
            //                FullName = (u.FirstName + " " + u.LastName),
            //                Contact =  (u.Address1 + " " + u.Address2),
            //                Email = u.LoginEmail,
            //                UserType = u.Usertype,
            //                Telephone = u.Telephone,
            //                CompanyName = companies.CompanyName,
            //                CompanyEmployeeId = companyemployees.CompanyEmployeeID,
            //                UserInformationId = u.UserInfoID
            //            }
            //         ).ToList<SearchResults>();
            //}
            else if (usertype == 2)
            {
                return (from u in db.UserInformations
                        join incentexemployees in db.IncentexEmployees on u.UserInfoID equals incentexemployees.UserInfoID
                        where
                            SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                            SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                            //SqlMethods.Like(u.Telephone, "%" + Telephone + "") &&
                            //SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
                            SqlMethods.Like(u.LoginEmail, "%" + Email + "%") &&
                        u.IsDeleted == false && u.UserInfoID != 2375
                        //&&
                        //SqlMethods.Like("Incentex","%"+Companyname+"%") 
                        select new SearchResults
                        {
                            FullName = (u.FirstName + " " + u.LastName),
                            Contact = u.Address2 == "" ? (u.Address1 + " " + u.Address2) : u.Address1,
                            Email = u.LoginEmail,
                            UserType = u.Usertype,
                            Telephone = u.Telephone,
                            CompanyName = "Incentex",
                            CompanyEmployeeId = 0,
                            UserInformationId = u.UserInfoID
                        }
                 ).ToList<SearchResults>();
            }
            else if (usertype == 5)
            {
                return
                    (from u in db.UserInformations
                     join suppliers in db.Suppliers on u.UserInfoID equals suppliers.UserInfoID
                     //join companies in db.Companies on new { CompanyID = suppliers.CompanyId } equals new { CompanyID = companies.CompanyID }
                     where
                     SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                     SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                         //SqlMethods.Like(u.Telephone, "%" + Telephone + "") &&
                         //SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
                     SqlMethods.Like(u.LoginEmail, "%" + Email + "%") &&
                     SqlMethods.Like(suppliers.CompanyName, "%" + Companyname + "%") &&
                        u.IsDeleted == false
                     select new SearchResults
                     {
                         FullName = (u.FirstName + " " + u.LastName),
                         Contact = u.Address2 == "" ? (u.Address1 + " " + u.Address2) : u.Address1,
                         Email = u.LoginEmail,
                         UserType = u.Usertype,
                         Telephone = u.Telephone
                        ,
                         CompanyName = suppliers.CompanyName,
                         CompanyEmployeeId = 0,
                         UserInformationId = u.UserInfoID
                     }
                    ).ToList<SearchResults>();
            }
            else if (usertype == 6)  //SupplierEmployee
            {
                return
                    (from u in db.UserInformations
                     join supplieremployees in db.SupplierEmployees on u.UserInfoID equals supplieremployees.UserInfoID
                     join suppliers in db.Suppliers on supplieremployees.SupplierID equals suppliers.SupplierID
                     //join companies in db.Companies on new { CompanyID = suppliers.CompanyId } equals new { CompanyID = companies.CompanyID }
                     where
                     SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                     SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                         //SqlMethods.Like(u.Telephone, "%" + Telephone + "") &&
                         //SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
                     SqlMethods.Like(u.LoginEmail, "%" + Email + "%") &&
                     SqlMethods.Like(suppliers.CompanyName, "%" + Companyname + "%") &&
                        u.IsDeleted == false
                     select new SearchResults
                     {
                         FullName = (u.FirstName + " " + u.LastName),
                         Contact = u.Address2 == "" ? (u.Address1 + " " + u.Address2) : u.Address1,
                         Email = u.LoginEmail,
                         UserType = u.Usertype,
                         Telephone = u.Telephone,
                         CompanyName = suppliers.CompanyName,
                         CompanyEmployeeId = 0,
                         UserInformationId = u.UserInfoID
                     }
                    ).ToList<SearchResults>();
            }
            else if (usertype == 8)  //Equipment Vendor Employee
            {
                var result =
                    (from u in db.UserInformations
                     join eve in db.EquipmentVendorEmployees on u.UserInfoID equals eve.UserInfoID.Value
                     join evm in db.EquipmentVendorMasters on eve.VendorID.Value equals evm.EquipmentVendorID
                     join Companys in db.Companies on evm.CompanyID equals Companys.CompanyID
                     where
                     SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                     SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                     SqlMethods.Like(u.LoginEmail, "%" + Email + "%") &&
                         // SqlMethods.Like(Companys.CompanyName, "%" + Companyname + "%") &&
                        u.IsDeleted == false
                     select new SearchResults
                     {
                         FullName = (u.FirstName + " " + u.LastName),
                         Contact = u.Address2 == "" ? (u.Address1 + " " + u.Address2) : u.Address1,
                         Email = u.LoginEmail,
                         UserType = u.Usertype,
                         Telephone = u.Telephone,
                         CompanyName = Companys.CompanyName,
                         CompanyEmployeeId = 0,
                         UserInformationId = u.UserInfoID
                     }).ToList<SearchResults>();

                return result;
            }
            else
            {
                if (string.IsNullOrEmpty(Companyname))
                {
                    return ((from u in db.UserInformations
                             join companies in db.Companies on new { CompanyID = Convert.ToInt64(u.CompanyId) } equals new { CompanyID = companies.CompanyID }
                             join companyemployees in db.CompanyEmployees on u.UserInfoID equals companyemployees.UserInfoID
                             join basestationtbl in db.INC_BasedStations on companyemployees.BaseStation equals basestationtbl.iBaseStationId
                             join employeetype in db.INC_Lookups on companyemployees.EmployeeTypeID equals employeetype.iLookupID into employeeGroup
                             from lastresult in employeeGroup.DefaultIfEmpty()
                             where
                             SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                             SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                                 //SqlMethods.Like(u.Telephone, "%" + Telephone + "") &&
                                 // SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
                             SqlMethods.Like(u.LoginEmail, "%" + Email + "%") &&
                             SqlMethods.Like(companyemployees.EmployeeID, "%" + EmployeeID + "%") &&
                             SqlMethods.Like(companies.CompanyName, "%" + Companyname + "%") &&
                             ((BaseStation != null && BaseStation != "") ? basestationtbl.iBaseStationId.ToString() == BaseStation : true) &&
                                 //SqlMethods.Like(basestationtbl.sBaseStation != null ? basestationtbl.sBaseStation : "", "%" + BaseStation + "%") &&
                             SqlMethods.Like(lastresult.sLookupName != null ? lastresult.sLookupName : "", "%" + EmployeeType + "%") &&
                             SqlMethods.Like(companyemployees.WorkgroupID.ToString(), WorkGroup != "" && WorkGroup != "0" ? WorkGroup : companyemployees.WorkgroupID.ToString()) &&
                             SqlMethods.Like(companyemployees.GenderID.ToString(), Gender != "" && Gender != "0" ? Gender : companyemployees.GenderID.ToString()) &&
                        u.IsDeleted == false
                             select new SearchResults
                             {
                                 FullName = (u.FirstName + " " + u.LastName),
                                 Contact = u.Address2 == "" ? (u.Address1 + " " + u.Address2) : u.Address1,
                                 Email = u.LoginEmail,
                                 UserType = u.Usertype,
                                 Telephone = u.Telephone
                                 ,
                                 CompanyName = companies.CompanyName,
                                 CompanyEmployeeId = companyemployees.CompanyEmployeeID,
                                 UserInformationId = u.UserInfoID
                             }
                         )

                        .Union
                        (
                         from u in db.UserInformations
                         join incentexemployees in db.IncentexEmployees on u.UserInfoID equals incentexemployees.UserInfoID
                         where
                             SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                             SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                             //SqlMethods.Like(u.Telephone, "%" + Telephone + "") &&
                             //SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
                             SqlMethods.Like(u.LoginEmail, "%" + Email + "%") &&
                        u.IsDeleted == false && u.UserInfoID != 2375
                         //&& 
                         //SqlMethods.Like("Incentex","%"+Companyname+"%")

                         select new SearchResults
                         {
                             FullName = (u.FirstName + " " + u.LastName),
                             Contact = u.Address2 == "" ? (u.Address1 + " " + u.Address2) : u.Address1,
                             Email = u.LoginEmail,
                             UserType = u.Usertype,
                             Telephone = u.Telephone
                             ,
                             CompanyName = "Incentex",
                             CompanyEmployeeId = 0,
                             UserInformationId = u.UserInfoID
                         }
                         ).Union
                         (
                         from u in db.UserInformations
                         join suppliers in db.Suppliers on u.UserInfoID equals suppliers.UserInfoID
                         //join companies in db.Companies on new { CompanyID = suppliers.CompanyId } equals new { CompanyID = companies.CompanyID }
                         where
                         SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                         SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                             //SqlMethods.Like(u.Telephone, "%" + Telephone + "%") &&
                             //SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
                         SqlMethods.Like(u.LoginEmail, "%" + Email + "%") &&
                         SqlMethods.Like(suppliers.CompanyName, "%" + Companyname + "%") &&
                        u.IsDeleted == false
                         select new SearchResults
                         {
                             FullName = (u.FirstName + " " + u.LastName),
                             Contact = u.Address2 == "" ? (u.Address1 + " " + u.Address2) : u.Address1,
                             Email = u.LoginEmail,
                             UserType = u.Usertype,
                             Telephone = u.Telephone,
                             CompanyName = suppliers.CompanyName,
                             CompanyEmployeeId = 0,
                             UserInformationId = u.UserInfoID
                         }
                        )).ToList<SearchResults>();
                }
                else
                {
                    return ((from u in db.UserInformations
                             join companies in db.Companies on new { CompanyID = Convert.ToInt64(u.CompanyId) } equals new { CompanyID = companies.CompanyID }
                             join companyemployees in db.CompanyEmployees on u.UserInfoID equals companyemployees.UserInfoID
                             join basestationtbl in db.INC_BasedStations on companyemployees.BaseStation equals basestationtbl.iBaseStationId
                             join employeetype in db.INC_Lookups on companyemployees.EmployeeTypeID equals employeetype.iLookupID into employeeGroup
                             from lastresult in employeeGroup.DefaultIfEmpty()
                             where
                             SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                             SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                                 //SqlMethods.Like(u.Telephone, "%" + Telephone + "") &&
                                 // SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
                             SqlMethods.Like(u.LoginEmail, "%" + Email + "%") &&
                             SqlMethods.Like(companyemployees.EmployeeID, "%" + EmployeeID + "%") &&
                             SqlMethods.Like(companies.CompanyName, "%" + Companyname + "%") &&
                             ((BaseStation != null && BaseStation != "") ? basestationtbl.iBaseStationId.ToString() == BaseStation : true) &&
                                 //SqlMethods.Like(basestationtbl.sBaseStation != null ? basestationtbl.sBaseStation : "", "%" + BaseStation + "%") &&
                             SqlMethods.Like(lastresult.sLookupName != null ? lastresult.sLookupName : "", "%" + EmployeeType + "%") &&
                             SqlMethods.Like(companyemployees.WorkgroupID.ToString(), WorkGroup != "" && WorkGroup != "0" ? WorkGroup : companyemployees.WorkgroupID.ToString()) &&
                             SqlMethods.Like(companyemployees.GenderID.ToString(), Gender != "" && Gender != "0" ? Gender : companyemployees.GenderID.ToString()) &&
                        u.IsDeleted == false
                             select new SearchResults
                             {
                                 FullName = (u.FirstName + " " + u.LastName),
                                 Contact = u.Address2 == "" ? (u.Address1 + " " + u.Address2) : u.Address1,
                                 Email = u.LoginEmail,
                                 UserType = u.Usertype,
                                 Telephone = u.Telephone
                                 ,
                                 CompanyName = companies.CompanyName,
                                 CompanyEmployeeId = companyemployees.CompanyEmployeeID,
                                 UserInformationId = u.UserInfoID
                             }
                         )

                        .Union
                         (
                         from u in db.UserInformations
                         join suppliers in db.Suppliers on u.UserInfoID equals suppliers.UserInfoID
                         //join companies in db.Companies on new { CompanyID = suppliers.CompanyId } equals new { CompanyID = companies.CompanyID }
                         where
                         SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                         SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                             //SqlMethods.Like(u.Telephone, "%" + Telephone + "%") &&
                             //SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
                         SqlMethods.Like(u.LoginEmail, "%" + Email + "%") &&
                         SqlMethods.Like(suppliers.CompanyName, "%" + Companyname + "%") &&
                        u.IsDeleted == false
                         select new SearchResults
                         {
                             FullName = (u.FirstName + " " + u.LastName),
                             Contact = u.Address2 == "" ? (u.Address1 + " " + u.Address2) : u.Address1,
                             Email = u.LoginEmail,
                             UserType = u.Usertype,
                             Telephone = u.Telephone,
                             CompanyName = suppliers.CompanyName,
                             CompanyEmployeeId = 0,
                             UserInformationId = u.UserInfoID
                         }
                        )).ToList<SearchResults>();
                }
            }
        }
        /// <summary>
        /// Return details of Coupa by current id
        /// </summary>
        /// <param name="coupaID"></param>
        /// <returns>Coupa Details</returns>
        public CoupaPunchOutDetail GetCoupaPunchOutDetailbyID(Int64 coupaID)
        {
            return db.CoupaPunchOutDetails.Where(q => q.PunchOutID == coupaID).FirstOrDefault();
        }
        //Get Company Users only
        public List<SearchResults> getCompanyUsers(string UserName, string LastName, string Telephone, string Mobile, string Email, long companyid, List<Int64> workgroupid, string EmployeeID, string WorkGroup, string Gender)
        {
            return (from u in db.UserInformations
                    join companies in db.Companies on new { CompanyID = Convert.ToInt64(u.CompanyId) } equals new { CompanyID = companies.CompanyID }
                    join companyemployees in db.CompanyEmployees on u.UserInfoID equals companyemployees.UserInfoID
                    where
                    SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                    SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                        //SqlMethods.Like(u.Telephone, "%" + Telephone + "%") &&
                        //SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
                    SqlMethods.Like(u.LoginEmail, "%" + Email + "%") &&
                    SqlMethods.Like(companyemployees.EmployeeID, "%" + EmployeeID + "%") &&
                    u.CompanyId == companyid &&
                    workgroupid.Contains(companyemployees.WorkgroupID) &&
                    SqlMethods.Like(companyemployees.WorkgroupID.ToString(), WorkGroup != "" && WorkGroup != "0" ? WorkGroup : companyemployees.WorkgroupID.ToString()) &&
                    SqlMethods.Like(companyemployees.GenderID.ToString(), Gender != "" && Gender != "0" ? Gender : companyemployees.GenderID.ToString()) &&
                        u.IsDeleted == false
                    select new SearchResults
                    {
                        UserInformationId = u.UserInfoID,
                        CompanyEmployeeId = companyemployees.CompanyEmployeeID,
                        FullName = (u.FirstName + " " + u.LastName),
                        Contact = (u.Address1 + " " + u.Address2),
                        Email = u.LoginEmail,
                        UserType = u.Usertype,
                        Telephone = u.Telephone,
                        Mobile = u.Mobile,
                        CompanyName = db.INC_Lookups.SingleOrDefault(l => l.iLookupID == u.WLSStatusId).sLookupIcon  //add by mayur on 13-jan-2012
                    }
                 ).ToList<SearchResults>();
        }

        public List<SearchResults> getCompanyUsersForAnniversaryCredit(string UserName, string LastName, string Telephone, string Mobile, string Email, long companyid, long workgroupid, string operation, DateTime AnniversaryCreatedDate)
        {
            if (operation == "AddAnniversary")
            {
                return (from u in db.UserInformations
                        join companies in db.Companies on new { CompanyID = Convert.ToInt64(u.CompanyId) } equals new { CompanyID = companies.CompanyID }
                        join companyemployees in db.CompanyEmployees on u.UserInfoID equals companyemployees.UserInfoID
                        where
                            //SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                            //SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                            //SqlMethods.Like(u.Telephone, "%" + Telephone + "%") &&
                            //SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
                            //SqlMethods.Like(u.Email, "%" + Email + "%") &&
                        u.CompanyId == companyid &&
                        companyemployees.WorkgroupID == workgroupid &&
                        u.CreatedDate <= AnniversaryCreatedDate &&
                        u.IsDeleted == false
                        select new SearchResults
                        {
                            UserInformationId = u.UserInfoID,
                            CompanyEmployeeId = companyemployees.CompanyEmployeeID,
                            FullName = (u.FirstName + " " + u.LastName),
                            Contact = (u.Address1 + " " + u.Address2),
                            Email = u.Email,
                            UserType = u.Usertype
                            //Telephone = u.Telephone,
                            //Mobile = u.Mobile
                        }
                     ).ToList<SearchResults>();
            }
            else
            {
                return (from u in db.UserInformations
                        join companies in db.Companies on new { CompanyID = Convert.ToInt64(u.CompanyId) } equals new { CompanyID = companies.CompanyID }
                        join companyemployees in db.CompanyEmployees on u.UserInfoID equals companyemployees.UserInfoID
                        where
                            //SqlMethods.Like(u.FirstName, "%" + UserName + "%") &&
                            //SqlMethods.Like(u.LastName, "%" + LastName + "%") &&
                            //SqlMethods.Like(u.Telephone, "%" + Telephone + "%") &&
                            // SqlMethods.Like(u.Mobile, "%" + Mobile + "%") &&
                            //SqlMethods.Like(u.Email, "%" + Email + "%") &&
                        u.CompanyId == companyid &&
                        companyemployees.WorkgroupID == workgroupid &&
                        u.CreatedDate > AnniversaryCreatedDate &&
                        u.IsDeleted == false
                        select new SearchResults
                        {
                            UserInformationId = u.UserInfoID,
                            CompanyEmployeeId = companyemployees.CompanyEmployeeID,
                            FullName = (u.FirstName + " " + u.LastName),
                            Contact = (u.Address1 + " " + u.Address2),
                            Email = u.Email,
                            UserType = u.Usertype
                            //Telephone = u.Telephone,
                            //Mobile = u.Mobile
                        }
                     ).ToList<SearchResults>();
            }
        }

        public class SearchResults
        {
            public Int64 CompanyEmployeeId { get; set; }
            public Int64 UserInformationId { get; set; }
            public string FullName { get; set; }
            public string Contact { get; set; }
            public string Email { get; set; }
            public string Telephone { get; set; }
            public string CompanyName { get; set; }
            public string Mobile { get; set; }
            public Int64 UserType { get; set; }
        }

        /// <summary>
        /// EmployeeStatus 
        /// </summary>
        /// <param name="userinfoid"></param>
        /// <param name="wsStatusId"></param>
        public void UpdateStatus(int userinfoid, int wsStatusId)
        {
            {
                var obj = db.CompanyEmployee_Status(userinfoid, wsStatusId);
            }
        }

        /// <summary>
        /// UserPassword 
        /// </summary>
        /// <param name="userinfoid"></param>
        /// <param name="password"></param>
        public void UpdatePassword(long userinfoid, string password)
        {
            UserInformation objUserInformation = db.UserInformations.Single(u => u.UserInfoID == userinfoid);
            objUserInformation.Password = password;
            db.SubmitChanges();
        }

        public List<UserInformation> GetEmailInformation()
        {
            var qry = (from u in db.UserInformations
                       join incentexEmployee in db.IncentexEmployees on u.UserInfoID equals incentexEmployee.UserInfoID
                       where u.IsDeleted == false
                       select u);
            return qry.ToList();
        }

        public List<selectusersuploadfromexcelsheetResult> GetNewUserInformation(long? companyid, long? workgroupid)
        {
            return db.selectusersuploadfromexcelsheet(companyid, workgroupid).ToList();
        }

        // for getting the list of employee for the dropdown of the campaign(communication center)
        public List<UserInformation> GetByCompanyId(Int64 CompanyId)
        {
            return db.UserInformations.Where(s => s.CompanyId == CompanyId && s.IsDeleted == false).ToList();
        }

        public List<AllUser> GetAllUser(Int64 CompanyId)
        {
            if (CompanyId == 8)
            {
                return (from U in db.UserInformations
                        join IE in db.IncentexEmployees on U.UserInfoID equals IE.UserInfoID
                        where U.IsDeleted == false && U.UserInfoID != 2375
                        select new AllUser
                        {
                            UserInfoID = U.UserInfoID,
                            UserName = U.FirstName + " " + U.LastName
                        }).ToList();
            }

            else
            {
                return (from U in db.UserInformations
                        join C in db.CompanyEmployees on U.UserInfoID equals C.UserInfoID
                        where U.CompanyId == CompanyId &&
                        U.IsDeleted == false
                        select new AllUser
                       {
                           UserInfoID = U.UserInfoID,
                           UserName = U.FirstName + " " + U.LastName
                       }).ToList();
            }
        }

        public class AllUser
        {
            public Int64 UserInfoID { get; set; }
            public String UserName { get; set; }
        }

        public long GetUserInfoIDOfIcentexEmployeeByName(String FirstName, String LastName)
        {
            return db.UserInformations.FirstOrDefault(s => s.FirstName == FirstName && s.LastName == LastName && (s.Usertype == 1 || s.Usertype == 2)).UserInfoID;
        }

        public UserInformation GetByEmail(String Email)
        {
            return db.UserInformations.FirstOrDefault(le => le.Email == Email);
        }

        public List<UserInformation> GetIncentexEmployees()
        {
            return db.UserInformations.Where(le => (le.Usertype == 1 || le.Usertype == 2) && le.WLSStatusId == 135 && le.IsDeleted == false && le.UserInfoID != 2375).ToList();
        }

        public UserInformation GetUserByIncentexEmployeeID(Int64 IncentexEmployeeID)
        {
            return (from ie in db.IncentexEmployees
                    join ui in db.UserInformations on ie.UserInfoID equals ui.UserInfoID
                    where ie.IncentexEmployeeID == IncentexEmployeeID && (ui.Usertype == 1 || ui.Usertype == 2)
                    && ui.IsDeleted == false && ui.UserInfoID != 2375 && ui.WLSStatusId == 135
                    select ui).FirstOrDefault();
        }

        public UserInformation GetByLoginEmail(String LoginEmail)
        {
            return db.UserInformations.FirstOrDefault(le => le.LoginEmail == LoginEmail);
        }

        //Saurabh
        public List<AllUser> GetEmployeesByWorkGrp(long companyid, long workgroupid)
        {
            try
            {
                return (from U in db.UserInformations
                        join C in db.CompanyEmployees on U.UserInfoID equals C.UserInfoID
                        where U.CompanyId == companyid && C.WorkgroupID == workgroupid &&
                        U.IsDeleted == false
                        orderby U.FirstName + " " + U.LastName
                        select new AllUser
                        {
                            UserInfoID = U.UserInfoID,
                            UserName = U.FirstName + " " + U.LastName 
                        }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<GetCompanyEmployeesResult> GetCompanyEmployees(Int64? CompanyID, Int64? WorkGroupID, Int64? UserType)
        {
            return db.GetCompanyEmployees(CompanyID, WorkGroupID, UserType).ToList();
        }

        public GetUserDetailsForSAPResult GetUserDetailsForSAP(Int64 UserInfoID)
        {
            return db.GetUserDetailsForSAP(UserInfoID).FirstOrDefault();
        }

        public List<GetCompanyEmployeeContactByCompanyIDResult> GetCompanyEmployeeContactByCompanyID(Int64 companyID, Int64 userInfoID)
        {
            return db.GetCompanyEmployeeContactByCompanyID(companyID, userInfoID).ToList();
        }

        public String GetUniqueWorldLinkContactIDByUserInfoID(Int64 UserInfoID)
        {
            return db.FUN_GetUniqueWorldLinkContactID(UserInfoID);
        }

        public GetUserInfoForForgetPasswordByEmailResult GetUserInfoForForgetPasswordByEmail(String email)
        {
            return db.GetUserInfoForForgetPasswordByEmail(email).FirstOrDefault();
        }

        public Int32 InsertWorkGroupAcessForUser(Int64 companyID, Int64 userInfoID, Int64 createdBy, String workGroupIDs)
        {
            return db.InsertWorkGroupAccessForCA(companyID, userInfoID, createdBy, workGroupIDs);
        }

        public List<GetMOASWorkGroupAccessByUserInfoIDResult> SelectMOASWOrkGroupAccessByUserInfoID(Int64 userInfoID)
        {
            return db.GetMOASWorkGroupAccessByUserInfoID(userInfoID).ToList();
        }

        public List<MOASApproverWorkGroupAccess> GetWorkGroupAccessByUserInfoID(Int64 userInfoID)
        {
            return db.MOASApproverWorkGroupAccesses.Where(w => w.UserInfoID == userInfoID).ToList();
        }

        /// <summary>
        /// Get User Details 
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <returns></returns>
        public GetUserDetailsByUserInfoIDResult GetUserDetailsByUserInfoID(Int64 userInfoID)
        {
            return db.GetUserDetailsByUserInfoID(userInfoID).FirstOrDefault();
        }

        public GetCheckoutDetailsByUserInfoIDResult GetCheckoutDetailsByUserInfoID(Int64 userInfoID)
        {
            return db.GetCheckoutDetailsByUserInfoID(userInfoID).FirstOrDefault();
        }
    }
}