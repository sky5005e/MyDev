using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class RegistrationRepository : RepositoryBase
    {
        IQueryable<Inc_Registration> GetAllQuery()
        {
            IQueryable<Inc_Registration> qry = from r in db.Inc_Registrations
                                               where (r.status ?? "").ToLower() != "rejected"
                                               select r;
            return qry;
        }

        public Inc_Registration GetByRegistrationID(Int64 RegistrationID)
        {
            return db.Inc_Registrations.FirstOrDefault(le => le.iRegistraionID == RegistrationID);
        }
        /// <summary>
        /// Get Pending User By RegistrationID 
        /// </summary>
        /// <param name="RegistrationID"></param>
        /// <returns></returns>
        public GetPendingUsersByRegistrationIDResult GetPendingUserByRegistrationID(Int64 RegistrationID)
        {
            return db.GetPendingUsersByRegistrationID(RegistrationID).FirstOrDefault();
        }

        public List<RegistrationSearchResults> GetAllUsersByWorkGroup(Int64 companyId, Int64 workgroupID, Incentex.DAL.SqlRepository.RegistrationRepository.UsersSortExpType SortExp, DAEnums.SortOrderType SortOrder)
        {
            var qry = (from r in db.Inc_Registrations
                       join bs in db.INC_BasedStations on r.iBasestationId equals bs.iBaseStationId into rbs
                       from leftjointbl in rbs.DefaultIfEmpty()
                       where (r.iCompanyName == companyId && r.iWorkgroupId == workgroupID && r.status == "pending")
                       select new RegistrationSearchResults
                       {
                           iRegistraionID = r.iRegistraionID,
                           BaseStationId = r.iBasestationId,
                           BaseStationName = (leftjointbl == null ? String.Empty : leftjointbl.sBaseStation),
                           DateRequestSubmitted = r.DateRequestSubmitted,
                           sEmailAddress = r.sEmailAddress,
                           sEmployeeId = r.sEmployeeId,
                           sFirstName = r.sFirstName,
                           sLastName = r.sLastName,
                           EmployeeType = r.iEmployeeTypeID != null ? (db.INC_Lookups.FirstOrDefault(inc => inc.iLookupID == r.iEmployeeTypeID).sLookupName) : "",
                           sTelephoneNumber = r.sTelephoneNumber,
                           status = r.status
                       }
                       ).ToList<RegistrationSearchResults>();

            switch (SortExp)
            {
                case RegistrationRepository.UsersSortExpType.sFirstName:
                    qry = qry.OrderBy(s => s.sFirstName).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.sEmployeeId:
                    qry = qry.OrderBy(s => s.sEmployeeId).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.sMobileNumber:
                    qry = qry.OrderBy(s => s.sMobileNumber).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.status:
                    qry = qry.OrderBy(s => s.status).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.BaseStationName:
                    qry = qry.OrderBy(s => s.BaseStationName).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.EmployeeType:
                    qry = qry.OrderBy(s => s.EmployeeType).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.DateRequestSubmitted:
                    qry = qry.OrderBy(s => s.DateRequestSubmitted).ToList();
                    break;

            }

            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                qry.Reverse();
            }

            return qry;
        }

        /// <summary>
        /// Gets the pending users count by work group and companyid.
        /// Add by mayur on 12-jan-2012 for display total pending user for company admin.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <param name="workgroupid">The workgroupid.</param>
        /// <returns></returns>
        public Int64 GetPendingUsersCountByWorkGroup(Int64 companyId, Int64 workgroupid)
        {
            var qry = (from r in db.Inc_Registrations
                       where (r.iCompanyName == companyId && r.iWorkgroupId == workgroupid && r.status == "pending")
                       select r).ToList<Inc_Registration>();

            return qry.Count;
        }
        /// <summary>
        /// To Approve Pending user 
        /// </summary>
        /// <param name="RegistrationID"></param>
        /// <param name="UserInfoID"></param>
        public void ApprovePendingUser(Int64 RegistrationID, Int64 UserInfoID, Boolean IsthirdParty)
        {
            db.ApprovePendingUser(RegistrationID, UserInfoID, IsthirdParty);
        }
        public List<RegistrationSearchResults> GetAllPendingUsers(Incentex.DAL.SqlRepository.RegistrationRepository.UsersSortExpType SortExp, DAEnums.SortOrderType SortOrder, Int64 CompanyID)
        {
            var qry = (from r in db.Inc_Registrations
                       from leftjointbl in db.INC_BasedStations.Where(s => s.iBaseStationId == r.iBasestationId).DefaultIfEmpty()
                       //join bs in db.INC_BasedStations on r.iBasestationId equals bs.iBaseStationId into rbs
                       //from leftjointbl in rbs.DefaultIfEmpty()
                       join c in db.Companies on r.iCompanyName equals c.CompanyID
                       where (r.status == "pending" && r.iCompanyName == CompanyID)
                       select new RegistrationSearchResults
                       {
                           iRegistraionID = r.iRegistraionID,
                           BaseStationId = r.iBasestationId,
                           BaseStationName = (leftjointbl == null ? String.Empty : leftjointbl.sBaseStation),
                           DateRequestSubmitted = r.DateRequestSubmitted,
                           sEmailAddress = r.sEmailAddress,
                           sEmployeeId = r.sEmployeeId,
                           sFirstName = r.sFirstName,
                           sLastName = r.sLastName,
                           EmployeeType = r.iEmployeeTypeID != null ? (db.INC_Lookups.FirstOrDefault(inc => inc.iLookupID == r.iEmployeeTypeID).sLookupName) : "",
                           sTelephoneNumber = r.sTelephoneNumber,
                           status = r.status,
                           CompanyName = c.CompanyName,
                           iWorkGroupID = r.iWorkgroupId
                       }
                       ).ToList<RegistrationSearchResults>();

            switch (SortExp)
            {
                case RegistrationRepository.UsersSortExpType.sFirstName:
                    qry = qry.OrderBy(s => s.sFirstName).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.sEmailAddress:
                    qry = qry.OrderBy(s => s.sEmailAddress).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.sEmployeeId:
                    qry = qry.OrderBy(s => s.sEmployeeId).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.sMobileNumber:
                    qry = qry.OrderBy(s => s.sMobileNumber).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.status:
                    qry = qry.OrderBy(s => s.status).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.sTelephoneNumber:
                    qry = qry.OrderBy(s => s.sTelephoneNumber).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.BaseStationName:
                    qry = qry.OrderBy(s => s.BaseStationName).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.EmployeeType:
                    qry = qry.OrderBy(s => s.EmployeeType).ToList();
                    break;
                case RegistrationRepository.UsersSortExpType.DateRequestSubmitted:
                    qry = qry.OrderBy(s => s.DateRequestSubmitted).ToList();
                    break;
            }

            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                qry.Reverse();
            }

            return qry;
        }

        public class SearchResults
        {

            public String FullName { get; set; }
            public String Contact { get; set; }
            public String Email { get; set; }
            public String Telephone { get; set; }
            public String CompanyName { get; set; }
            public String Mobile { get; set; }
        }

        public String[] InsertIntoUserInformation(Int64 loggedinuserid, Int64 registrationid)
        {
            try
            {
                String[] val = new String[2];

                String b = null;
                Int64? uid = 0;
                Int32 a = db.SelectInsertInUserinfo(loggedinuserid, registrationid, ref b, ref uid);
                val[0] = b;
                val[1] = uid.ToString();
                return val;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void InsertIntoCompanyEmployeeMenuAccess(Int64 userid)
        {
            try
            {
                db.SelectInsertInCompanyEmployee(userid);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void UpdateValueToReject(Int64 registrationid)
        {
            try
            {
                db.UpdateRegistrationToReject(registrationid);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public Int64? getworkgroupbyregistrationid(Int64 regid)
        {
            return GetByRegistrationID(regid).iWorkgroupId;
        }

        public enum UsersSortExpType
        {

            sFirstName,
            sEmployeeId,
            sEmailAddress,
            sTelephoneNumber,
            sMobileNumber,
            status,
            BaseStationName,
            EmployeeType,
            DateRequestSubmitted,
            EmployeeName,
            CompanyName,
            WorkGroup
        }


        public class RegistrationSearchResults
        {

            public Int64 iCompanyId { get; set; }
            public Int64 iRegistraionID { get; set; }
            public String sEmployeeId { get; set; }
            public String sFirstName { get; set; }
            public String sLastName { get; set; }
            public String sTelephoneNumber { get; set; }
            public String sEmailAddress { get; set; }
            public DateTime? DateRequestSubmitted { get; set; }
            public Int64? BaseStationId { get; set; }
            public String BaseStationName { get; set; }
            public String EmployeeType { get; set; }
            public String sMobileNumber { get; set; }
            public String status { get; set; }
            public String CompanyName { get; set; }
            public Int64? iWorkGroupID { get; set; }
        }

        public Boolean CheckEmailExistence(String email, Int64 registrationID)
        {
            Inc_Registration objRegistration = db.Inc_Registrations.FirstOrDefault(le => le.sEmailAddress == email && (le.status ?? "").ToLower() != "rejected" && le.iRegistraionID != registrationID);
            return objRegistration == null;
        }

        public Boolean CheckEmployeeIDExistence(String employeeID, Int64 companyID, Int64 registrationID)
        {
            Inc_Registration objRegistration = db.Inc_Registrations.FirstOrDefault(le => le.sEmployeeId == employeeID && le.iCompanyName == companyID && (le.status ?? "").ToLower() != "rejected" && le.iRegistraionID != registrationID);
            return objRegistration == null;
        }
    }
}