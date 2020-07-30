using System;
using System.Collections.Generic;
using System.Linq;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class UserTrackingRepo : RepositoryBase
    {
        public UserTracking GetById(int LoginActivityID)
        {
            return db.UserTrackings.FirstOrDefault(C => C.UserInfoID == LoginActivityID && C.UserInfoID != 2375);
        }

        public UserTracking GetByUserId(Int64 UserId)
        {
            return db.UserTrackings.FirstOrDefault(C => C.UserInfoID == UserId && C.UserInfoID != 2375);
        }

        public List<UserTracking> GetUserByDate(DateTime Sd, DateTime Ed)
        {
            var list = (from c in db.UserTrackings
                        where c.LoginTime >= Sd && c.LoginTime <= Ed && c.UserInfoID != 2375
                        select c);

            return list.ToList();
        }

        // delete
        public List<UserTracking> GetUserByUserinfoid(int Userinfoid, DateTime sd, DateTime ed)
        {
            var list = (from c in db.UserTrackings
                        where c.UserInfoID == Userinfoid && c.LoginTime >= sd && c.LoginTime <= c.LoginTime
                        orderby c.LoginTime descending
                        select c);

            return list.ToList();
        }

        // this for the taking the list of user whose are accessed the system.
        public List<UserAccessTrackingResult> AccessUserList(DateTime Sd, DateTime Ed, UserAccessTrackingSortExpType SortExp, DAEnums.SortOrderType SortOrder)
        {
            List<UserAccessTrackingResult> qry = db.UserAccessTracking(Sd, Ed).Where(le => le.UserInfoID != 2375).ToList();
            var List = qry.ToList<UserAccessTrackingResult>();
            switch (SortExp)
            {
                case UserAccessTrackingSortExpType.CompanyName:
                    List = qry.OrderBy(s => s.CompanyName).ToList();
                    break;

                case UserAccessTrackingSortExpType.Name:
                    List = qry.OrderBy(s => s.Name).ToList();
                    break;
                //case UserAccessTrackingSortExpType.LoginTime:
                //    List = qry.OrderBy(s => s.LoginTime).ToList();
                //    break;
                case UserAccessTrackingSortExpType.LoginCount:
                    List = qry.OrderBy(s => s.LoginCount).ToList();
                    break;
            }

            if (SortOrder == DAEnums.SortOrderType.Desc)
                List.Reverse();

            return List;
        }

        public List<UserAccessTrackingForPurchaseResult> AccessUserListPurchase(DateTime Sd, DateTime Ed)
        {
            return db.UserAccessTrackingForPurchase(Sd, Ed).ToList();
        }

        public List<PurchaseCountResult> GetPucrchaeCount(DateTime Sd, DateTime Ed, int uid)
        {
            return db.PurchaseCount(Sd, Ed, uid).ToList();
        }

        // if user comes on thank you page flag will set to true
        public UserTracking SetPurchase(Int32 Usertrackid)
        {
            UserTracking obj = (from c in db.UserTrackings
                                where c.UserTrackID == Usertrackid
                                select c).ToList().SingleOrDefault();
            obj.Isupdate = true;
            SubmitChanges();
            return obj;

        }

        // for set logout time
        public UserTracking SetLogout(Int32 usetrackid, DateTime logout)
        {

            UserTracking obj = (from c in db.UserTrackings
                                where c.UserTrackID == Convert.ToInt32(usetrackid)
                                select c).ToList().SingleOrDefault();
            obj.LogoutTime = logout;
            obj.UserStatus = false;
            db.SubmitChanges();
            return obj;
        }

        public enum UserAccessTrackingSortExpType
        {
            CompanyName,
            Name,
            LoginTime,
            LoginCount
        }

        public enum UserPurchaseTrackingSortExpType
        {
            CompanyName,
            Name,
            LoginTime,
            LoginCount,
            Isupdate
        }

        public enum UserPageviewd
        {
            PagesViewed,
            PageName
        }

        public List<UserAccessTrackingForPurchaseResult> GetPurchaseUserList(DateTime Sd, DateTime Ed, UserPurchaseTrackingSortExpType SortExp, DAEnums.SortOrderType SortOrder)
        {
            List<UserAccessTrackingForPurchaseResult> qry = AccessUserListPurchase(Sd, Ed).ToList<UserAccessTrackingForPurchaseResult>();
            var List = qry.ToList<UserAccessTrackingForPurchaseResult>();
            switch (SortExp)
            {
                case UserPurchaseTrackingSortExpType.CompanyName:
                    List = qry.OrderBy(s => s.CompanyName).ToList();
                    break;

                case UserPurchaseTrackingSortExpType.Name:
                    List = qry.OrderBy(s => s.Name).ToList();
                    break;
                case UserPurchaseTrackingSortExpType.LoginTime:
                    List = qry.OrderBy(s => s.LoginTime).ToList();
                    break;
                case UserPurchaseTrackingSortExpType.LoginCount:
                    List = qry.OrderBy(s => s.LoginCount).ToList();
                    break;
                case UserPurchaseTrackingSortExpType.Isupdate:
                    List = qry.OrderBy(s => s.Isupdate).ToList();
                    break;
            }

            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                List.Reverse();
            }

            return List;
        }

        public GetLastLoginTimeResult GetLastDateTimeByUserid(Int32? userid)
        {
            return db.GetLastLoginTime(userid).ToList().FirstOrDefault();
        }

        // for the gird of menu
        public List<UserTracking> GetUsersByuid(int Uid)
        {
            var list = (from c in db.UserTrackings
                        where c.UserInfoID != 2375
                        select c);
            return list.ToList();
        }

        // for getting all the ipaddress which are not same
        public List<getip> Getallip(DateTime sdip, DateTime edip)
        {
            var list = (from c in db.UserTrackings
                        where c.LoginTime >= sdip && c.LoginTime <= edip && c.UserInfoID != 2375
                        select new getip { IPAddress = c.IPAddress }).Distinct();
            return list.ToList();
        }

        public class getip
        {
            public String IPAddress
            {
                get;
                set;
            }
        }

        public List<UserPageHistoryTracking> GetHistoryByUserTrackingID(int UserTrackID)
        {
            return (from ut in db.UserTrackings
                    join ht in db.UserPageHistoryTrackings on ut.UserTrackID equals ht.UserTrackingID
                    where ut.UserTrackID == UserTrackID && ut.UserInfoID != 2375
                    select ht
                        ).ToList();
        }

        // this is for the status column in user access report  dispaly total digits of purchasing products.
        public List<UserTracking> GetPurcgaseNumber(int UserInfoId, DateTime Sdate, DateTime Edate)
        {
            return (from ut in db.UserTrackings
                    where ut.UserInfoID == UserInfoId && ut.LoginTime >= Sdate.Date && ut.LogoutTime <= Edate.Date && ut.UserInfoID != 2375
                    select ut
                        ).ToList();
        }

        // this Sp for the user activity report
        public List<UserActivityReportResult> GetUserActivity(DateTime Sd, DateTime Ed)
        {
            return db.UserActivityReport(Sd, Ed).ToList();
        }

        // This sp for the getting the list of the user whose access link
        public List<GetUserWiseAccessListResult> GetUserAccessList(DateTime Sd, DateTime Ed, string Link)
        {
            return db.GetUserWiseAccessList(Sd, Ed, Link).ToList();
        }

        // This sp for the getting the list of the user whose access link(delete if not work nm)
        private List<GetUserWiseAccessListResult> GetUserAccessList1(DateTime Sd, DateTime Ed, string Link)
        {
            return db.GetUserWiseAccessList(Sd, Ed, Link).ToList();
        }

        // This sp for the getting browser count by passing the parameter two dates
        public List<GetBrowserTypeCountResult> GetBrowserConut(DateTime Sd, DateTime Ed)
        {
            return db.GetBrowserTypeCount(Sd, Ed).ToList();
        }

        //This sp for the getting the user list browser wise
        public List<GetBrowserWiseUserNameResult> GetUserBrowserWise(string BrowserName, DateTime Sd, DateTime Ed)
        {
            return db.GetBrowserWiseUserName(BrowserName, Sd, Ed).ToList();
        }

        //Report for System Access by Hour
        public List<GetSystemAccessDetailResult> GetSystemAccess(DateTime? fromDate, DateTime? toDate, Int64? userInfoID)
        {
            return db.GetSystemAccessDetail(fromDate, toDate, userInfoID).ToList();
        }

        public List<GetSystemAccessCountResult> GetAccessCount(DateTime? fromDate, DateTime? toDate, Int64? userInfoID, Int64 hr)
        {
            return db.GetSystemAccessCount(fromDate, toDate, userInfoID, hr).ToList();
        }
    }
}