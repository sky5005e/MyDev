using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace Incentex.DAL.SqlRepository
{
    public class TodayEmailsRepository : RepositoryBase
    {

      
        public List<GetUserofTodayMails> GetAllUserList(DateTime dt)
        {

             var qry = (from te in db.TodaysEmailDetails
                       join ui in db.UserInformations on te.UserInfoID equals ui.UserInfoID
                        join c in db.Companies on ui.CompanyId equals c.CompanyID
                        where te.CreatedDate.Value.Date == dt.Date && ui.IsDeleted == false
                        select new GetUserofTodayMails
                       {
                           UserInfoID = ui.UserInfoID,
                           FullName = ui.FirstName + " " + ui.LastName,
                           FirstName = ui.FirstName,
                           LastName = ui.LastName,
                           LoginEmail = ui.LoginEmail,
                           CompanyName = c.CompanyName,
                           dateTime = te.CreatedDate
                       }).ToList().Distinct();
             return qry.ToList<GetUserofTodayMails>();
        }

        public List<GetDetailsTodayMails> GetAllDetailsofTodaysMail(DateTime dt, Int64 UserInfoID)
        {

            var qry = (from te in db.TodaysEmailDetails
                       join ui in db.UserInformations on te.UserInfoID equals ui.UserInfoID
                       join c in db.Companies on ui.CompanyId equals c.CompanyID
                       where te.CreatedDate.Value.Date == dt.Date && ui.IsDeleted == false
                       select new GetDetailsTodayMails
                       {
                           mailID = te.MailID,
                           UserInfoID = ui.UserInfoID,
                           FullName = ui.FirstName + " " + ui.LastName,
                           FirstName = ui.FirstName,
                           LastName = ui.LastName,
                           LoginEmail = ui.LoginEmail,
                           CompanyName = c.CompanyName,
                           dateTime = te.CreatedDate
                       }).ToList();

            if (UserInfoID != 0)
            {
                qry = qry.Where(c => c.UserInfoID == UserInfoID).ToList();
            }
            return qry.ToList<GetDetailsTodayMails>();
        }
        /// <summary>
        /// Get Todays emails sent result by todays date
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<GetTodaysSentEmail> GetAllTodaysEmailSent(Int64 CompanyID,DateTime dt)
        {
            var query = (from te in db.TodaysEmailDetails
                       join ui in db.UserInformations on te.UserInfoID equals ui.UserInfoID
                       where te.CreatedDate.Value.Date == dt.Date && ui.IsDeleted == false
                       select new
                       {
                           te.MailID,
                           ui.CompanyId,
                           te.CreatedDate
                       }).ToList();

            if (CompanyID != 0)
                query = query.Where(c => c.CompanyId == CompanyID).ToList();

            List<GetTodaysSentEmail> objGetTodaysSentEmailList = new List<GetTodaysSentEmail>();
            GetTodaysSentEmail objGetTodaysSentEmailResult = new GetTodaysSentEmail();
            // Here get the result by hours.
            if (query.Count > 0)
            {
                for (int i = 1; i <= 24; i++)
                {
                    objGetTodaysSentEmailResult = new GetTodaysSentEmail();
                    objGetTodaysSentEmailResult.Count = (query.Where(c => c.CreatedDate.Value.TimeOfDay.TotalHours > i - 1 && c.CreatedDate.Value.TimeOfDay.TotalHours <= i)).Count();
                    if (i <= 11)
                        objGetTodaysSentEmailResult.Hours = String.Format("{0}AM", i);
                    else if (i == 12)
                        objGetTodaysSentEmailResult.Hours = String.Format("{0}PM", i);
                    else if (i > 12 && i < 24)
                        objGetTodaysSentEmailResult.Hours = String.Format("{0}PM", i - 12);
                    else
                        objGetTodaysSentEmailResult.Hours = String.Format("{0}AM", (i - 24).ToString("00"));

                    objGetTodaysSentEmailList.Add(objGetTodaysSentEmailResult);
                }
            }
                return objGetTodaysSentEmailList;
           

        }
        /// <summary>
        /// Here this method will return the Order Placed list as per the filter criteria.
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserInfoID"></param>
        /// <param name="date"></param>
        /// <param name="nDays"></param>
        /// <returns></returns>
        public List<GetTodaysSentEmail> GetAllOrderPlacedList(Int64 CompanyID, Int64 UserInfoID, DateTime date, Int32 nDays)
        {
            List<GetTodaysSentEmail> objOrderPlacedList = new List<GetTodaysSentEmail>();
            List<List<GetTodaysSentEmail>> objGetAllOrderPlacedList = new List<List<GetTodaysSentEmail>>();
            for (int day = 0; day <= nDays; day++)
            {
                // Set Date
                DateTime dt = date.AddDays(day);

                var query = (from ord in db.Orders
                             join ui in db.UserInformations on ord.UserId equals ui.UserInfoID
                             where ord.OrderDate.Value.Date == dt.Date && ui.IsDeleted == false
                             select new
                             {
                                 ord.OrderID,
                                 ui.CompanyId,
                                 ui.UserInfoID,
                                 ord.OrderDate
                             }).ToList();

                if (CompanyID != 0)
                    query = query.Where(q => q.CompanyId == CompanyID).ToList();

                if (UserInfoID != 0)
                    query = query.Where(q => q.UserInfoID == UserInfoID).ToList();

                List<GetTodaysSentEmail> objList = new List<GetTodaysSentEmail>();
                GetTodaysSentEmail objGetOrderPlacedResult = new GetTodaysSentEmail();
                // Here get the result by hours.
                if (query.Count > 0)
                {
                    for (int i = 1; i <= 24; i++)
                    {
                        objGetOrderPlacedResult = new GetTodaysSentEmail();
                        objGetOrderPlacedResult.Count = (query.Where(o => o.OrderDate.Value.TimeOfDay.TotalHours > i - 1 && o.OrderDate.Value.TimeOfDay.TotalHours <= i)).Count();
                        if (i <= 11)
                            objGetOrderPlacedResult.Hours = String.Format("{0}AM", i);
                        else if (i == 12)
                            objGetOrderPlacedResult.Hours = String.Format("{0}PM", i);
                        else if (i > 12 && i < 24)
                            objGetOrderPlacedResult.Hours = String.Format("{0}PM", i - 12);
                        else
                            objGetOrderPlacedResult.Hours = String.Format("{0}AM", (i - 24).ToString("00"));

                        objList.Add(objGetOrderPlacedResult);
                    }
                }
                // At Last add the above List
                objGetAllOrderPlacedList.Add(objList);

            }

            for (int j = 0; j < 24; j++)// This loop is used to get the records by hours of day.
            {
                GetTodaysSentEmail obj = new GetTodaysSentEmail();
                Int64? count = 0;
                String Hours = String.Empty;
                for (int i = 0; i <= objGetAllOrderPlacedList.Count - 1; i++)// This loop is used to get the records by hours form ie 0-1AM of  All days.
                {
                    if (objGetAllOrderPlacedList[i].Count > 0)
                    {
                        count += objGetAllOrderPlacedList[i][j].Count;
                        Hours = objGetAllOrderPlacedList[i][j].Hours;
                    }
                }
                obj.Count = count;
                obj.Hours = Hours;
                objOrderPlacedList.Add(obj);
            }
            return objOrderPlacedList;


        }

        public List<GetTodaysSentEmail> GetAllTodaysEmailSent(Int64 CompanyID, Int64 UserInfoID, DateTime date, Int32 nDays)
        {
            List<GetTodaysSentEmail> objGetTodaysSentList = new List<GetTodaysSentEmail>();
            List<List<GetTodaysSentEmail>> objGetTodaysSentEmailList = new List<List<GetTodaysSentEmail>>();
            for (int day = 0; day <= nDays; day++)
            {
                // Set Date
                DateTime  dt = date.AddDays(day);

                var query = (from te in db.TodaysEmailDetails
                             join ui in db.UserInformations on te.UserInfoID equals ui.UserInfoID
                             where te.CreatedDate.Value.Date == dt.Date && ui.IsDeleted == false
                             select new
                             {
                                 te.MailID,
                                 ui.CompanyId,
                                 ui.UserInfoID,
                                 te.CreatedDate
                             }).ToList();

                if (CompanyID != 0)
                    query = query.Where(q => q.CompanyId == CompanyID).ToList();

                if (UserInfoID != 0)
                    query = query.Where(q => q.UserInfoID == UserInfoID).ToList();

                List<GetTodaysSentEmail> objList = new List<GetTodaysSentEmail>();
                GetTodaysSentEmail objGetTodaysSentEmailResult = new GetTodaysSentEmail();
                // Here get the result by hours.
                if (query.Count > 0)
                {
                    for (int i = 1; i <= 24; i++)
                    {
                        objGetTodaysSentEmailResult = new GetTodaysSentEmail();
                        objGetTodaysSentEmailResult.Count = (query.Where(c => c.CreatedDate.Value.TimeOfDay.TotalHours > i - 1 && c.CreatedDate.Value.TimeOfDay.TotalHours <= i)).Count();
                        if (i <= 11)
                            objGetTodaysSentEmailResult.Hours = String.Format("{0}AM", i);
                        else if (i == 12)
                            objGetTodaysSentEmailResult.Hours = String.Format("{0}PM", i);
                        else if (i > 12 && i < 24)
                            objGetTodaysSentEmailResult.Hours = String.Format("{0}PM", i - 12);
                        else
                            objGetTodaysSentEmailResult.Hours = String.Format("{0}AM", (i - 24).ToString("00"));

                        objList.Add(objGetTodaysSentEmailResult);                       
                    }
                }
                // At Last add the above List
                objGetTodaysSentEmailList.Add(objList);

            }

            for (int j = 0; j < 24; j++)// This loop is used to get the records by hours of day.
            {
                GetTodaysSentEmail obj = new GetTodaysSentEmail();
                Int64? count = 0;
                String Hours = String.Empty;
                for (int i = 0; i <= objGetTodaysSentEmailList.Count - 1; i++)// This loop is used to get the records by hours form ie 0-1AM of  All days.
                {
                    if (objGetTodaysSentEmailList[i].Count > 0)
                    {
                        count += objGetTodaysSentEmailList[i][j].Count;
                        Hours = objGetTodaysSentEmailList[i][j].Hours;
                    }
                }
                obj.Count = count;
                obj.Hours = Hours;
                objGetTodaysSentList.Add(obj);
            }
            return objGetTodaysSentList;


        }


        public List<GetTodaysSentEmail> GetAllWeekEmailSent(Int64 CompanyID, Int64 UserInfoID, DateTime date, Int32 nDays)
        {
            List<GetTodaysSentEmail> objSetTodaysSentList = new List<GetTodaysSentEmail>();
            for (int day = 0; day <= nDays; day++)
            {
                // Set Date
                DateTime dt = date.AddDays(day);

                var query = (from te in db.TodaysEmailDetails
                             join ui in db.UserInformations on te.UserInfoID equals ui.UserInfoID
                             where te.CreatedDate.Value.Date == dt.Date && ui.IsDeleted == false
                             select new
                             {
                                 te.MailID,
                                 ui.CompanyId,
                                 ui.UserInfoID,
                                 te.CreatedDate
                             }).ToList();

                if (CompanyID != 0)
                    query = query.Where(q => q.CompanyId == CompanyID).ToList();

                if (UserInfoID != 0)
                    query = query.Where(q => q.UserInfoID == UserInfoID).ToList();
                // Here get the result by day.
                if (query.Count > 0)
                {

                    GetTodaysSentEmail objGetTodaysSentEmailResult = new GetTodaysSentEmail();
                    objGetTodaysSentEmailResult.Count = (query.Where(c => c.CreatedDate.Value.Date == dt.Date)).Count();
                    objGetTodaysSentEmailResult.Hours = String.Format("{0}", dt.Date.DayOfWeek.ToString());
                    objSetTodaysSentList.Add(objGetTodaysSentEmailResult);
                }
            }
            // Return List 
            List<GetTodaysSentEmail> objGetTodaysSentList = new List<GetTodaysSentEmail>();
            // Always Week Start From Monday

            for (int i = 0; i < DayofWeek().Count; i++)// This loop is used to get the records by day.
            {
                // Get day of week.
                String day = DayofWeek()[i].ToString();
                var newResult = (from d in objSetTodaysSentList
                                 where d.Hours == day
                                 select d).ToList();
                // Now Add this to return List
                GetTodaysSentEmail obj = new GetTodaysSentEmail();
                Int64? count = 0;
                String Hours = String.Empty;
                foreach (var item in newResult)
                {
                    count += item.Count;
                    Hours = item.Hours;                    
                }
                obj.Count = count;
                obj.Hours = day;
                objGetTodaysSentList.Add(obj);
            }
            return objGetTodaysSentList;


        }

        public List<GetUserforMails> GetAllUserInfobyBarGraph(Int32 rptOption, Int64 CompanyID, Int64 UserInfoID, DateTime frmdate, DateTime toDate, Int32 XvalueofHour, String WeekDays)
        {
            if (rptOption == 0 || rptOption == 1)
            {
                var query = (from te in db.TodaysEmailDetails
                             join ui in db.UserInformations on te.UserInfoID equals ui.UserInfoID
                             join c in db.Companies on ui.CompanyId equals c.CompanyID
                             where te.CreatedDate.Value.Date >= frmdate.Date && te.CreatedDate.Value.Date <= toDate.Date && ui.IsDeleted == false
                             select new GetUserforMails
                             {
                                 CompanyId = c.CompanyID,
                                 MailID = te.MailID,
                                 UserInfoID = ui.UserInfoID,
                                 CompanyName = c.CompanyName,
                                 FullName = ui.FirstName + " " + ui.LastName,
                                 LoginEmail = ui.LoginEmail,
                                 DateTime = te.CreatedDate
                             }).ToList();
                // Get the value of that time span
                if (XvalueofHour != 0)
                    query = query.Where(q => q.DateTime.Value.TimeOfDay.TotalHours > XvalueofHour - 1 && q.DateTime.Value.TimeOfDay.TotalHours <= XvalueofHour).ToList();
                // Get the Value of that week days
                if (WeekDays != "" && WeekDays != null)
                    query = query.Where(q => q.DateTime.Value.DayOfWeek.ToString() == WeekDays).ToList();

                if (CompanyID != 0)
                    query = query.Where(q => q.CompanyId == CompanyID).ToList();

                if (UserInfoID != 0)
                    query = query.Where(q => q.UserInfoID == UserInfoID).ToList();

                return query.ToList<GetUserforMails>();
            }
            else
            {
                var query = (from cmh in db.CampaignMailHistories
                             join ui in db.UserInformations on cmh.UserInfoID equals ui.UserInfoID
                             join c in db.Companies on ui.CompanyId equals c.CompanyID
                             where cmh.ReadDate.Value.Date >= frmdate.Date && cmh.ReadDate.Value.Date <= toDate.Date
                             && cmh.CampMailStatus == 2 && ui.IsDeleted == false
                             select new GetUserforMails
                             {
                                 CompanyId = c.CompanyID,
                                 MailID = cmh.CMSId,
                                 UserInfoID = ui.UserInfoID,
                                 CompanyName = c.CompanyName,
                                 FullName = ui.FirstName + " " + ui.LastName,
                                 LoginEmail = ui.LoginEmail,
                                 DateTime = cmh.ReadDate
                             }).ToList();
                // Get the value of that time span
                if (XvalueofHour != 0)
                    query = query.Where(q => q.DateTime.Value.TimeOfDay.TotalHours > XvalueofHour - 1 && q.DateTime.Value.TimeOfDay.TotalHours <= XvalueofHour).ToList();
                // Get the Value of that week days
                if (WeekDays != "" && WeekDays != null)
                    query = query.Where(q => q.DateTime.Value.DayOfWeek.ToString() == WeekDays).ToList();

                if (CompanyID != 0)
                    query = query.Where(q => q.CompanyId == CompanyID).ToList();

                if (UserInfoID != 0)
                    query = query.Where(q => q.UserInfoID == UserInfoID).ToList();

                return query.ToList<GetUserforMails>();
            }


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
        /// Get Todays emails sent result by week day's
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="dtMonday"></param>
        /// <returns></returns>
        public List<GetTodaysSentEmail> GetAllWeekEmailSent(Int64 CompanyID, DateTime dtMonday)
        {
            var query = (from te in db.TodaysEmailDetails
                         join ui in db.UserInformations on te.UserInfoID equals ui.UserInfoID
                         where te.CreatedDate.Value.Date >= dtMonday.Date && ui.IsDeleted == false
                         select new
                         {
                             te.MailID,
                             ui.CompanyId,
                             te.CreatedDate
                         }).ToList();

            if (CompanyID != 0)
                query = query.Where(c => c.CompanyId == CompanyID).ToList();

            List<GetTodaysSentEmail> objGetTodaysSentEmailList = new List<GetTodaysSentEmail>();
            GetTodaysSentEmail objGetTodaysSentEmailResult = new GetTodaysSentEmail();
            if (query.Count > 0)
            {
                // Here get the result by day.
                for (int i = 1; i <= 7; i++)
                {
                    objGetTodaysSentEmailResult = new GetTodaysSentEmail();
                    objGetTodaysSentEmailResult.Count = (query.Where(c => c.CreatedDate.Value.Date >= dtMonday.Date.AddDays(i - 1) && c.CreatedDate.Value.Date < dtMonday.Date.AddDays(i))).Count();
                    objGetTodaysSentEmailResult.Hours = String.Format("{0}", dtMonday.Date.AddDays(i - 1).DayOfWeek.ToString());
                    objGetTodaysSentEmailList.Add(objGetTodaysSentEmailResult);
                }
            }
            return objGetTodaysSentEmailList;
        }

        public Dictionary<Int64, String> GetAllTemplatesFromDB()
        {
            Dictionary<Int64, String> objDict = new Dictionary<Int64, String>();
            var qry = db.INC_EmailTemplates.Select(i=> new {i.iTemplateID, i.sTemplateName}).ToList();
            foreach (var items in qry)
            {
                objDict.Add(items.iTemplateID, items.sTemplateName);
            }
            return objDict;
        }

        public String GetEmailTemplatesByID(Int64 iTemplateID)
        {
            return db.INC_EmailTemplates.Where(i => i.iTemplateID == iTemplateID).FirstOrDefault().sTemplateContent;

        }

        public INC_EmailTemplate GetEmailTemplatesByTempName(String TempName)
        {
            return db.INC_EmailTemplates.Where(q => q.sTemplateName == TempName).FirstOrDefault();
        }

        public INC_EmailTemplate GetTemplatesDetailsByID(Int64 tempID)
        {
            return db.INC_EmailTemplates.Where(q => q.iTemplateID == tempID).FirstOrDefault();
        }

        public String GetMessagePathID(Int64 mailID)
        {
            return db.TodaysEmailDetails.Where(t => t.MailID == mailID).FirstOrDefault().FilePath;
        }

        public List<TodaysEmailDetail> TodaysEmailDetailList()
        {
            return db.TodaysEmailDetails.ToList();
        }

        public TodaysEmailDetail TodaysEmailDetailByID(Int64 mailID)
        {
            return db.TodaysEmailDetails.Where(t => t.MailID == mailID).FirstOrDefault();
        }

        public List<GetUserforMails> GetEmailsbyOrderID(Int64 OrderID)
        {
            return (from Email in db.TodaysEmailDetails
                        join User in db.UserInformations on Email.UserInfoID equals User.UserInfoID
                        where Email.OrderID == OrderID
                        select new GetUserforMails{
                            UserInfoID = User.UserInfoID,
                            MailID = Email.MailID,
                            FullName = User.FirstName + " " + User.LastName,
                            DateTime = Email.CreatedDate
                        }).ToList();
        }

        public class GetUserofTodayMails
        {
            public Int64 UserInfoID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string LoginEmail { get; set; }
            public String FullName { get; set; }
            public String CompanyName { get; set; }
            public DateTime? dateTime { get; set; }            
        }

        public class GetUserforMails
        {   
            public Int64 UserInfoID { get; set; }
            public Int64 MailID { get; set; }
            public string LoginEmail { get; set; }
            public String FullName { get; set; }
            public String CompanyName { get; set; }
            public DateTime? DateTime { get; set; }
            public Int64 CompanyId { get; set; }
        }

        public class GetDetailsTodayMails
        {
            public Int64 mailID { get; set; }
            public Int64 UserInfoID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string LoginEmail { get; set; }
            public String FullName { get; set; }
            public String BaseStation { get; set; }
            public String CompanyName { get; set; }
            public DateTime? dateTime { get; set; }
        }

        public class GetTodaysSentEmail
        {
            public Int64? Count { get; set; }
            public String Hours { get; set; }
        }
    }
}