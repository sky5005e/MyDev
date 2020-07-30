using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace Incentex.DAL.SqlRepository
{
    public class CampignRepo : RepositoryBase
    {
        public IQueryable<SelectCampUser> GetAllCampUser(Int32 CompanyID, Int32 WorkgroupID, Int32 DeptID, Int32 GenderID, Int32 StationID)
        {

            var ObjCampUser = (from User in db.UserInformations
                               join CE in db.CompanyEmployees on User.UserInfoID equals CE.UserInfoID
                               where User.CompanyId == CompanyID && User.IsDeleted == false
                               && CE.WorkgroupID == WorkgroupID
                               && CE.DepartmentID == DeptID
                               && CE.GenderID == GenderID
                               && CE.BaseStation == StationID
                               && CE.MailFlag == false
                               && SqlMethods.Like(User.LoginEmail, "%test%")
                               && User.UserInfoID != 1262
                               select new SelectCampUser
                              {
                                  FirstName = User.FirstName,
                                  LastName = User.LastName,
                                  Email = User.Email,
                                  UserInfoID = User.UserInfoID,
                                  CompanyEmployeeId = CE.CompanyEmployeeID
                              });

            return ObjCampUser;
        }


        public List<SelectViewCamp> CampView(Int32 Cid)
        {
            var List = (from Cmp in db.Campaigns
                        join CmpHs in db.CampaignMailHistories
                        on Cmp.CampaignID equals CmpHs.CampID into p
                        from a in p.DefaultIfEmpty()
                        where Cmp.CampaignID == Cid
                        select new SelectViewCamp
                        {
                            CampId = Cmp.CampaignID,
                            Campname = Cmp.Name,
                            CampDate = Convert.ToDateTime(Cmp.CDate),
                            CampTotalmail = Convert.ToString(Cmp.TotalMail),
                            CampMailStatus = a.CampMailStatus,
                            UserinfoId = a.UserInfoID

                        }).ToList();
            //var List = (from Cmp in db.Campaigns

            //            from CmpHs in db.CampaignMailHistories.Where(a => a.CampID == Cmp.CampaignID).DefaultIfEmpty()

            //            select new SelectViewCamp

            //            {

            //                CampId = Cmp.CampaignID,
            //                Campname = Cmp.Name,
            //                CampDate = Convert.ToDateTime(Cmp.CDate),
            //                CampTotalmail = Convert.ToString(Cmp.TotalMail),
            //                CampMailStatus = CmpHs.CampMailStatus,
            //                UserinfoId = CmpHs.UserInfoID

            //            }).ToList();

            return List;

        }

        /// <summary>
        /// Delete a Temp Item record by ID
        /// </summary>
        /// <param name="ProductItemid"></param>
        public void DeleteTempItem(int tempID)
        {

            var matchedId = (from e in db.GetTable<EmailTemplete>()
                             where e.TempID == tempID
                             select e).SingleOrDefault();
            try
            {
                if (matchedId != null)
                {
                    var matchedcamNote = (from c in db.GetTable<CampaignNote>()
                                          where c.TempID == Convert.ToInt64(tempID)
                                          select c).ToList();
                    foreach (var camnnote in matchedcamNote)
                    {
                        db.CampaignNotes.DeleteOnSubmit(camnnote);
                    }
                    db.SubmitChanges();

                    db.EmailTempletes.DeleteOnSubmit(matchedId);

                }
                db.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public List<SearchCampUser> GetUnDeliverableEmails(Int64 compID)
        {
            if (compID == 8)
            {
                var qry = from U in db.UserInformations
                          join IE in db.IncentexEmployees on U.UserInfoID equals IE.UserInfoID
                          where IE.MailFlag == false && U.IsDeleted == false
                          select new SearchCampUser
                          {
                              UserInfoID = U.UserInfoID,
                              FullName = U.FirstName + " " + U.LastName,
                              FirstName = U.FirstName,
                              LastName = U.LastName,
                              LoginEmail = U.LoginEmail,
                              MailFlag = IE.MailFlag,
                              CompanyEmployeeID = IE.IncentexEmployeeID,
                          };
                return qry.ToList<SearchCampUser>();
            }
            else
            {
                var qry = from U in db.UserInformations
                          join C in db.CompanyEmployees on U.UserInfoID equals C.UserInfoID
                          where C.MailFlag == false && U.IsDeleted == false
                          select new SearchCampUser
                          {
                              UserInfoID = U.UserInfoID,
                              FirstName = U.FirstName,
                              LastName = U.LastName,
                              LoginEmail = U.LoginEmail,
                              MailFlag = C.MailFlag,
                              CompanyEmployeeID = C.CompanyEmployeeID,
                              CompanyID = U.CompanyId,
                          };
                return qry.ToList<SearchCampUser>();
            }

        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userTypeID"></param>
        /// <returns></returns>
        public List<SearchCampUser> GetIncentexEmp(Int64 userID, Int64 userTypeID)
        {
            var qry = from U in db.UserInformations
                      join IE in db.IncentexEmployees on U.UserInfoID equals IE.UserInfoID
                      where U.IsDeleted == false && U.WLSStatusId == 135 &&
                            (userID != 0 ? U.UserInfoID == userID : true) &&
                            (userTypeID != 0 ? U.Usertype == userTypeID : true)
                      select new SearchCampUser
                      {
                          UserInfoID = U.UserInfoID,
                          UserTypeID = U.Usertype,
                          FullName = U.FirstName + " " + U.LastName,
                          FirstName = U.FirstName,
                          LastName = U.LastName,
                          LoginEmail = U.LoginEmail,
                          MailFlag = IE.MailFlag,
                          CompanyEmployeeID = IE.IncentexEmployeeID,
                      };

            //if (userID != 0)
            //{
            //    qry = qry.Where(q => q.UserInfoID == userID);
            //}
            //if (userTypeID != 0)
            //{
            //    qry = qry.Where(q => q.UserTypeID == userTypeID);
            //}
            return qry.ToList<SearchCampUser>();
        }

        public List<SearchCampUser> GetWordwideprospects()
        {
            var qry = (from w in db.WorldWideProspects
                       where w.IsDeleted == false
                       select new SearchCampUser
                       {
                           UserInfoID = w.ProspectID,
                           MailFlag = w.MailFlag,
                           FullName = w.ContactName,
                           FirstName = w.ContactName,
                           LastName = w.CompanyName,
                           LoginEmail = w.Email
                       }).ToList();

            return qry;
        }

        public List<SearchCampUser> GetCompanyEmp(Int64 CompanyID, Int64 userID, Int64 userTypeID)
        {
            var qry = from U in db.UserInformations
                      join C in db.CompanyEmployees on U.UserInfoID equals C.UserInfoID
                      where U.IsDeleted == false &&
                            (CompanyID != 0 ? U.CompanyId == CompanyID : true) &&
                            (userID != 0 ? U.UserInfoID == userID : true) &&
                            (userTypeID != 0 ? U.Usertype == userTypeID : true)
                      select new SearchCampUser
                      {
                          UserInfoID = U.UserInfoID,
                          UserTypeID = U.Usertype,
                          FullName = U.FirstName + " " + U.LastName,
                          CompanyID = U.CompanyId,
                      };

            //if (CompanyID != 0)
            //{
            //    qry = qry.Where(q => q.CompanyID == CompanyID);
            //}
            //if (userID != 0)
            //{
            //    qry = qry.Where(q => q.UserInfoID == userID);
            //}
            //if (userTypeID != 0)
            //{
            //    qry = qry.Where(q => q.UserTypeID == userTypeID);
            //}

            return qry.ToList<SearchCampUser>();
        }

        /// <summary>
        /// To get the user details as per companyID
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public List<SearchCampUser> GetBounceDetails(Int64 CompanyID)
        {
            if (CompanyID == 8)
            {
                var qry = from U in db.UserInformations
                          join IE in db.IncentexEmployees on U.UserInfoID equals IE.UserInfoID
                          where U.IsDeleted == false
                          select new SearchCampUser
                          {
                              UserInfoID = U.UserInfoID,
                              FullName = U.FirstName + " " + U.LastName,
                              FirstName = U.FirstName,
                              LastName = U.LastName,
                              LoginEmail = U.LoginEmail,
                              CompanyID = U.CompanyId,
                              MailFlag = IE.MailFlag,
                          };
                return qry.ToList<SearchCampUser>();
            }
            else
            {
                var qry = from U in db.UserInformations
                          join C in db.CompanyEmployees on U.UserInfoID equals C.UserInfoID
                          where U.IsDeleted == false
                          select new SearchCampUser
                          {
                              UserInfoID = U.UserInfoID,
                              FullName = U.FirstName + " " + U.LastName,
                              FirstName = U.FirstName,
                              LastName = U.LastName,
                              LoginEmail = U.LoginEmail,
                              CompanyID = U.CompanyId,
                              MailFlag = C.MailFlag,
                          };
                return qry.ToList<SearchCampUser>();
            }
        }
        /// <summary>
        /// Get User InfoId and ComapnyID from CampaignMailHistory table by campId
        /// </summary>
        /// <param name="campID"></param>
        /// <returns></returns>
        public List<SearchCampUser> GetCampMailHistoryUserID(Int64 campID, int mailStatus, Int64 CompID)
        {
            // Get All List of user Id by campID
            var list = from C in db.Campaigns
                       join H in db.CampaignMailHistories on C.CampaignID equals H.CampID
                       where H.CampID == campID && H.CampMailStatus == mailStatus
                       select new
                       {
                           H.UserInfoID,
                           C.CompanyID,
                       };


            // Now perform join from above values
            if (list != null)
            {
                if (CompID == 8)
                {
                    var qry = from U in db.UserInformations
                              join l in list on U.UserInfoID equals l.UserInfoID
                              join IE in db.IncentexEmployees on U.UserInfoID equals IE.UserInfoID
                              where U.IsDeleted == false
                              select new SearchCampUser
                              {
                                  UserInfoID = U.UserInfoID,
                                  FullName = U.FirstName + " " + U.LastName,
                                  FirstName = U.FirstName,
                                  LastName = U.LastName,
                                  LoginEmail = U.LoginEmail,
                                  CompanyID = l.CompanyID,
                                  MailFlag = IE.MailFlag,
                              };
                    return qry.ToList<SearchCampUser>();
                }
                else
                {
                    var qry = from U in db.UserInformations
                              join l in list on U.UserInfoID equals l.UserInfoID
                              join C in db.CompanyEmployees on U.UserInfoID equals C.UserInfoID
                              where U.IsDeleted == false
                              select new SearchCampUser
                              {
                                  UserInfoID = U.UserInfoID,
                                  FullName = U.FirstName + " " + U.LastName,
                                  FirstName = U.FirstName,
                                  LastName = U.LastName,
                                  LoginEmail = U.LoginEmail,
                                  CompanyID = l.CompanyID,
                                  MailFlag = C.MailFlag,
                              };
                    return qry.ToList<SearchCampUser>();
                }

            }
            else
                return null;



        }


        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="DepartmentID"></param>
        /// <param name="WorkGroupID"></param>
        /// <param name="GenderID"></param>
        /// <param name="BaseStationID"></param>
        /// <param name="CountryID"></param>
        /// <param name="userID"></param>
        /// <param name="userTypeID"></param>
        /// <param name="listExcludeCompanies"></param>
        /// <returns></returns>
        public List<SearchCampUser> GetCampUserFinal(Int64 CompanyID, Int64 DepartmentID, Int64 WorkGroupID, Int64 GenderID, Int64 BaseStationID, Int64 CountryID, Int64 userID, Int64 userTypeID, List<ExcluideCompanies> listExcludeCompanies)
        {

            var qry = (from U in db.UserInformations
                       join C in db.CompanyEmployees on U.UserInfoID equals C.UserInfoID
                       where U.IsDeleted == false && U.WLSStatusId == 135 &&
                             (CompanyID != 0 ? U.CompanyId == CompanyID : true) &&
                             (DepartmentID != 0 ? (C.DepartmentID.HasValue ? C.DepartmentID.Value == DepartmentID : false) : true) &&
                             (WorkGroupID != 0 ? C.WorkgroupID == WorkGroupID : true) &&
                             (GenderID != 0 ? C.GenderID == GenderID : true) &&
                             (BaseStationID != 0 ? C.BaseStation == BaseStationID : true) &&
                             (CountryID != 0 ? (U.CountryId.HasValue ? U.CountryId.Value == CountryID : false) : true) &&
                             (userID != 0 ? U.UserInfoID == userID : true) &&
                             (userTypeID != 0 ? U.Usertype == userTypeID : true)
                       select new SearchCampUser
                       {
                           UserInfoID = U.UserInfoID,
                           UserTypeID = U.Usertype,
                           FirstName = U.FirstName,
                           LastName = U.LastName,
                           LoginEmail = U.LoginEmail,
                           MailFlag = C.MailFlag,
                           CompanyEmployeeID = C.CompanyEmployeeID,
                           CompanyID = U.CompanyId,
                           DepartmentID = (C.DepartmentID.Value == null ? -1 : C.DepartmentID.Value),
                           WorkgroupID = C.WorkgroupID,
                           GenderID = C.GenderID,
                           BaseStation = C.BaseStation,
                           CountryId = (U.CountryId.Value == null ? -1 : U.CountryId.Value),
                       }).ToList();

            //if (CompanyID != 0)
            //{
            //    qry = qry.Where(q => q.CompanyID == CompanyID).ToList();
            //}

            //if (DepartmentID != 0)
            //{
            //    qry = qry.Where(q => q.DepartmentID == DepartmentID).ToList();
            //}

            //if (WorkGroupID != 0)
            //{
            //    qry = qry.Where(q => q.WorkgroupID == WorkGroupID).ToList();
            //}

            //if (GenderID != 0)
            //{
            //    qry = qry.Where(q => q.GenderID == GenderID).ToList();
            //}

            //if (BaseStationID != 0)
            //{
            //    qry = qry.Where(q => q.BaseStation == BaseStationID).ToList();
            //}

            //if (CountryID != 0)
            //{
            //    qry = qry.Where(q => q.CountryId == CountryID).ToList();
            //}
            //if (userID != 0)
            //{
            //    qry = qry.Where(q => q.UserInfoID == userID).ToList();
            //}
            //if (userTypeID != 0)
            //{
            //    qry = qry.Where(q => q.UserTypeID == userTypeID).ToList();
            //}

            if (listExcludeCompanies != null)
            {
                foreach (var item in listExcludeCompanies)
                {
                    qry = qry.Where(q => q.CompanyID != item.CompanyID).ToList();
                }

            }
            return qry.ToList<SearchCampUser>();
        }

        public class SearchCampUser
        {
            public Int64 UserInfoID { get; set; }
            public Int64 UserTypeID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string LoginEmail { get; set; }
            public bool? MailFlag { get; set; }
            public Int64? CompanyEmployeeID { get; set; }
            public Int64? CompanyID { get; set; }
            public Int64 DepartmentID { get; set; }
            public Int64 WorkgroupID { get; set; }
            public Int64 GenderID { get; set; }
            public Int64? BaseStation { get; set; }
            public Int64 CountryId { get; set; }
            public String FullName { get; set; }
        }

        public IQueryable<SelectCampUser1> GetViewCampDetail()
        {
            var ListViewCamp = (from Emp in db.Campaigns
                                select new SelectCampUser1
                                {
                                    CampaignID = Emp.CampaignID,
                                    CDate = (Convert.ToDateTime(Emp.CDate)).ToShortDateString(),
                                    Name = Emp.Name,
                                    TotalMail = Convert.ToInt32(Emp.TotalMail),
                                    IsComplete = Emp.IsComplete,
                                    CompanyID = Emp.CompanyID

                                });
            return ListViewCamp;

        }

        public List<EmailTemplete> GetAllTemp()
        {
            var List = (from ls in db.EmailTempletes
                        select ls);
            return List.ToList();

        }

        /// <summary>
        /// Get Todays emails read result
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<GetTodaysReadEmail> GetAllTodaysEmailRead(Int64 CompanyID, DateTime dt)
        {
            var query = (from te in db.CampaignMailHistories
                         join ui in db.UserInformations on te.UserInfoID equals ui.UserInfoID
                         where te.ReadDate.Value.Date == dt.Date && ui.IsDeleted == false
                         && te.CampMailStatus == 2
                         && (CompanyID != 0 ? ui.CompanyId == CompanyID : true)
                         select new
                         {
                             te.CMSId,
                             te.ReadDate,
                             ui.CompanyId
                         }).ToList();

            //if (CompanyID != 0)
            //    query = query.Where(c => c.CompanyId == CompanyID).ToList();

            List<GetTodaysReadEmail> objGetTodaysReadEmailList = new List<GetTodaysReadEmail>();
            GetTodaysReadEmail objGetTodaysReadEmailResult = new GetTodaysReadEmail();
            if (query.Count > 0)
            {
                for (int i = 1; i <= 24; i++)
                {
                    objGetTodaysReadEmailResult = new GetTodaysReadEmail();

                    objGetTodaysReadEmailResult.Count = (query.Where(c => c.ReadDate.Value.TimeOfDay.TotalHours > i - 1 && c.ReadDate.Value.TimeOfDay.TotalHours <= i)).Count();
                    if (i <= 11)
                        objGetTodaysReadEmailResult.Hours = String.Format("{0}AM", i);
                    else if (i == 12)
                        objGetTodaysReadEmailResult.Hours = String.Format("{0}PM", i);
                    else if (i > 12 && i < 24)
                        objGetTodaysReadEmailResult.Hours = String.Format("{0}PM", i - 12);
                    else
                        objGetTodaysReadEmailResult.Hours = String.Format("{0}AM", (i - 24).ToString("00"));

                    objGetTodaysReadEmailList.Add(objGetTodaysReadEmailResult);
                }
            }
            return objGetTodaysReadEmailList;

        }

        /// <summary>
        /// Uodate By : GAurang Pathak
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserInfoID"></param>
        /// <param name="date"></param>
        /// <param name="nDays"></param>
        /// <returns></returns>
        public List<GetTodaysReadEmail> GetAllTodaysEmailRead(Int64 CompanyID, Int64 UserInfoID, DateTime date, Int32 nDays)
        {
            List<GetTodaysReadEmail> objGetTodaysReadList = new List<GetTodaysReadEmail>();
            List<List<GetTodaysReadEmail>> objGetTodaysReadEmailList = new List<List<GetTodaysReadEmail>>();
            for (int day = 0; day <= nDays; day++)
            {
                // Set Date
                DateTime dt = date.AddDays(day);

                var query = (from te in db.CampaignMailHistories
                             join ui in db.UserInformations on te.UserInfoID equals ui.UserInfoID
                             where te.ReadDate.Value.Date == dt.Date
                             && te.CampMailStatus == 2 && ui.IsDeleted == false 
                             && (CompanyID != 0 ? ui.CompanyId == CompanyID : true)
                             && (UserInfoID != 0? ui.UserInfoID == UserInfoID : true)
                             select new
                             {
                                 te.CMSId,
                                 ui.UserInfoID,
                                 te.ReadDate,
                                 ui.CompanyId
                             }).ToList();

                //if (CompanyID != 0)
                //    query = query.Where(q => q.CompanyId == CompanyID).ToList();

                //if (UserInfoID != 0)
                //    query = query.Where(q => q.UserInfoID == UserInfoID).ToList();

                List<GetTodaysReadEmail> objList = new List<GetTodaysReadEmail>();
                GetTodaysReadEmail objGetTodaysReadEmailResult = new GetTodaysReadEmail();
                // Here get the result by hours.
                if (query.Count > 0)
                {
                    for (int i = 1; i <= 24; i++)
                    {
                        objGetTodaysReadEmailResult = new GetTodaysReadEmail();
                        objGetTodaysReadEmailResult.Count = (query.Where(c => c.ReadDate.Value.TimeOfDay.TotalHours > i - 1 && c.ReadDate.Value.TimeOfDay.TotalHours <= i)).Count();
                        if (i <= 11)
                            objGetTodaysReadEmailResult.Hours = String.Format("{0}AM", i);
                        else if (i == 12)
                            objGetTodaysReadEmailResult.Hours = String.Format("{0}PM", i);
                        else if (i > 12 && i < 24)
                            objGetTodaysReadEmailResult.Hours = String.Format("{0}PM", i - 12);
                        else
                            objGetTodaysReadEmailResult.Hours = String.Format("{0}AM", (i - 24).ToString("00"));

                        objList.Add(objGetTodaysReadEmailResult);
                    }
                }
                // At Last add the above List
                objGetTodaysReadEmailList.Add(objList);

            }

            for (int j = 0; j < 24; j++)// This loop is used to get the records by hours of day.
            {
                GetTodaysReadEmail obj = new GetTodaysReadEmail();
                Int64? count = 0;
                String Hours = String.Empty;
                for (int i = 0; i <= objGetTodaysReadEmailList.Count - 1; i++)// This loop is used to get the records by hours form ie 0-1AM of  All days.
                {
                    if (objGetTodaysReadEmailList[i].Count > 0)
                    {
                        count += objGetTodaysReadEmailList[i][j].Count;
                        Hours = objGetTodaysReadEmailList[i][j].Hours;
                    }
                }
                obj.Count = count;
                obj.Hours = Hours;
                objGetTodaysReadList.Add(obj);
            }
            return objGetTodaysReadList;


        }

        /// <summary>
        /// Update By : Gaurang pathak
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserInfoID"></param>
        /// <param name="date"></param>
        /// <param name="nDays"></param>
        /// <returns></returns>
        public List<GetTodaysReadEmail> GetAllWeekEmailRead(Int64 CompanyID, Int64 UserInfoID, DateTime date, Int32 nDays)
        {
            List<GetTodaysReadEmail> objSetTodaysReadList = new List<GetTodaysReadEmail>();
            for (int day = 0; day <= nDays; day++)
            {
                // Set Date
                DateTime dt = date.AddDays(day);

                var query = (from te in db.CampaignMailHistories
                             join ui in db.UserInformations on te.UserInfoID equals ui.UserInfoID
                             where te.ReadDate.Value.Date == dt.Date
                             && te.CampMailStatus == 2 && ui.IsDeleted == false &&
                             (CompanyID != 0 ? ui.CompanyId == CompanyID : true) &&
                             (UserInfoID != 0 ? ui.UserInfoID == UserInfoID : true)
                             select new
                             {
                                 te.CMSId,
                                 ui.UserInfoID,
                                 te.ReadDate,
                                 ui.CompanyId
                             }).ToList();

                //if (CompanyID != 0)
                //    query = query.Where(q => q.CompanyId == CompanyID).ToList();

                //if (UserInfoID != 0)
                //    query = query.Where(q => q.UserInfoID == UserInfoID).ToList();
              
                
                // Here get the result by day.
                if (query.Count > 0)
                {

                    GetTodaysReadEmail objGetTodaysReadEmailResult = new GetTodaysReadEmail();
                    objGetTodaysReadEmailResult.Count = (query.Where(c => c.ReadDate.Value.Date == dt.Date)).Count();
                    objGetTodaysReadEmailResult.Hours = String.Format("{0}", dt.Date.DayOfWeek.ToString());
                    objSetTodaysReadList.Add(objGetTodaysReadEmailResult);
                }
            }
            // Return List 
            List<GetTodaysReadEmail> objGetTodaysReadList = new List<GetTodaysReadEmail>();
            // Always Week Start From Monday

            for (int i = 0; i < DayofWeek().Count; i++)// This loop is used to get the records by day.
            {
                // Get day of week.
                String day = DayofWeek()[i].ToString();
                var newResult = (from d in objSetTodaysReadList
                                 where d.Hours == day
                                 select d).ToList();
                // Now Add this to return List
                GetTodaysReadEmail obj = new GetTodaysReadEmail();
                Int64? count = 0;
                String Hours = String.Empty;
                foreach (var item in newResult)
                {
                    count += item.Count;
                    Hours = item.Hours;
                }
                obj.Count = count;
                obj.Hours = day;
                objGetTodaysReadList.Add(obj);
            }
            return objGetTodaysReadList;


        }
        private List<String> DayofWeek()
        {
            List<String> WeekDays = new List<String>();
            WeekDays.Add(DayOfWeek.Monday.ToString());
            WeekDays.Add(DayOfWeek.Tuesday.ToString());
            WeekDays.Add(DayOfWeek.Wednesday.ToString());
            WeekDays.Add(DayOfWeek.Thursday.ToString());
            WeekDays.Add(DayOfWeek.Friday.ToString());
            WeekDays.Add(DayOfWeek.Saturday.ToString());
            WeekDays.Add(DayOfWeek.Sunday.ToString());
            return WeekDays;
        }
        /// <summary>
        /// Get Todays emails read result by week day's
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="dtMonday"></param>
        /// <returns></returns>
        public List<GetTodaysReadEmail> GetAllWeekEmailRead(Int64 CompanyID, DateTime dtMonday)
        {
            var query = (from te in db.CampaignMailHistories
                         join ui in db.UserInformations on te.UserInfoID equals ui.UserInfoID
                         where te.ReadDate.Value.Date >= dtMonday.Date
                         && te.CampMailStatus == 2 && ui.IsDeleted == false
                         && (CompanyID != 0 ? ui.CompanyId == CompanyID : true)
                         select new
                         {
                             te.CMSId,
                             te.ReadDate,
                             ui.CompanyId
                         }).ToList();

            //if (CompanyID != 0)
            //    query = query.Where(c => c.CompanyId == CompanyID).ToList();

            List<GetTodaysReadEmail> objGetTodaysSentEmailList = new List<GetTodaysReadEmail>();
            GetTodaysReadEmail objGetTodaysSentEmailResult = new GetTodaysReadEmail();
            if (query.Count > 0)
            {
                // Here get the result by day.
                for (int i = 1; i <= 7; i++)
                {
                    objGetTodaysSentEmailResult = new GetTodaysReadEmail();
                    objGetTodaysSentEmailResult.Count = (query.Where(c => c.ReadDate.Value.Date >= dtMonday.Date.AddDays(i - 1) && c.ReadDate.Value.Date < dtMonday.Date.AddDays(i))).Count();
                    objGetTodaysSentEmailResult.Hours = String.Format("{0}", dtMonday.Date.AddDays(i - 1).DayOfWeek.ToString());
                    objGetTodaysSentEmailList.Add(objGetTodaysSentEmailResult);
                }
            }
            return objGetTodaysSentEmailList;
        }
        public Campaign CountMail(Int64 mailcount, int campid)
        {

            Campaign obj = (from c in db.Campaigns
                            where c.CampaignID == Convert.ToInt32(campid)
                            select c).ToList().SingleOrDefault();

            if (obj != null && mailcount != 0)
                obj.TotalMail = Convert.ToInt32(mailcount);


            SubmitChanges();

            return obj;

        }
        public Campaign SetTempID(Int64 Tempid, int campid)
        {

            Campaign obj = (from c in db.Campaigns
                            where c.CampaignID == Convert.ToInt32(campid)
                            select c).ToList().SingleOrDefault();

            obj.TempID = Convert.ToInt32(Tempid);

            SubmitChanges();

            return obj;

        }
        /// <summary>
        /// Get the MessageBody of open Email
        /// </summary>
        /// <param name="mailID"></param>
        /// <returns></returns>
        public String GetMessageContentsByID(Int64 mailID, Int64 UserInfoID)
        {
            String msg = String.Empty;
            var qry = (from cmh in db.CampaignMailHistories
                       join cn in db.CampaignNotes on cmh.CampID equals cn.CampaignID
                       where cmh.CMSId == mailID && cn.UserInfoID == UserInfoID
                       && cmh.CampMailStatus == 2
                       select new
                       {
                           mailID = cmh.CMSId,
                           uid = cn.UserInfoID,
                           MessageBody = cn.MessageBody
                       }).ToList();
            foreach (var items in qry)
            {
                msg = items.MessageBody.ToString();
            }
            return msg;
        }
        public Campaign SaveAttachment(Int32 Campid, string path)
        {
            Campaign lst = (from s in db.Campaigns
                            where s.CampaignID == Campid
                            select s).ToList().SingleOrDefault();

            lst.Attachment = path;
            SubmitChanges();
            return lst;

        }
        public EmailTemplete FindDuplicate(string Email)
        {

            var count = (from l in db.EmailTempletes
                         where l.TempName.Contains(Email)
                         select l);
            return count.SingleOrDefault();


        }
        public class SelectCampUser
        {
            public Int64 UserInfoID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public DateTime CDate { get; set; }
            public Int64 TotalEmail { get; set; }
            public Int64 CompanyEmployeeId { get; set; }
        }
        public class GetTodaysReadEmail
        {
            public Int64? Count { get; set; }
            public String Hours { get; set; }
        }

        public class ExcluideCompanies
        {
            public Int64 CompanyID { get; set; }
        }
        // this class is for view the camp
        public class SelectViewCamp
        {
            public Int64 CampId { get; set; }
            public string Campname { get; set; }
            public DateTime CampDate { get; set; }
            public string CampTotalmail { get; set; }
            public DateTime? CDate { get; set; }
            public Int64? CampMailStatus { get; set; }
            public Int64? UserinfoId { get; set; }
        }
        public class InserEmailTemp
        {
            public string TemapName { get; set; }
        }


        public class SelectCampUser1
        {
            public Int64 CampaignID { get; set; }
            public Int64? CompanyID { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string CDate { get; set; }
            public Int64 TotalMail { get; set; }
            public bool IsComplete { get; set; }
        }
        public void DeleteCamp(Int64 Campid)
        {
            var matchedCamp = (from c in db.GetTable<Campaign>()
                               where c.CampaignID == Convert.ToInt64(Campid)
                               select c).SingleOrDefault();
            try
            {
                if (matchedCamp != null)
                {
                    var matchedcamNote = (from c in db.GetTable<CampaignNote>()
                                          where c.CampaignID == Convert.ToInt64(Campid)
                                          select c).ToList();
                    foreach (var camnnote in matchedcamNote)
                    {
                        db.CampaignNotes.DeleteOnSubmit(camnnote);
                    }
                    db.SubmitChanges();

                    db.Campaigns.DeleteOnSubmit(matchedCamp);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Campaign GetDetailFromCampID(Int32 cid)
        {

            Campaign obj = (from c in db.Campaigns
                            where c.CampaignID == Convert.ToInt32(cid)
                            select c).ToList().SingleOrDefault();
            return obj;

        }



        public object GetViewCampDetail(int Cid)
        {
            throw new NotImplementedException();
        }

        public Campaign SetTotalMail1(int Cid)
        {

            Campaign obj = (from c in db.Campaigns
                            where c.CampaignID == Convert.ToInt32(Cid)
                            select c).ToList().SingleOrDefault();

            obj.TotalMail = 1;

            SubmitChanges();


            return obj;

        }

        // this for the set the original value of total count for final 5 step 

        public Campaign SetMailCount(Int32 campid, Int32 mailcount)
        {

            Campaign obj = (from c in db.Campaigns
                            where c.CampaignID == Convert.ToInt32(campid)
                            select c).ToList().SingleOrDefault();

            obj.TotalMail = Convert.ToInt32(mailcount);

            SubmitChanges();

            return obj;

        }



    }






}
