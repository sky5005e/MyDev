using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class OrderNoteRecipientDetailRepository : RepositoryBase
    {
      
        public List<OrderNoteReceipientDetail> GetIEListByOrderID(long OrderID,long NoteID)
        {
            return (from e in db.OrderNoteReceipientDetails
                    join ie in db.IncentexEmployees on e.UserInfoId equals ie.UserInfoID
                    where e.OrderId == OrderID && e.SubscriptionFlag == true && ie.MemberRole != "Super Admin"
                          && e.NoteID == NoteID
                    select e).ToList();
        }

        public List<IEListOrderNote> GetIEList(long OrderID)
        {
            var IEList = (from e in db.OrderNoteReceipientDetails
                          join ie in db.IncentexEmployees on e.UserInfoId equals ie.UserInfoID
                          join u in db.UserInformations on ie.UserInfoID equals u.UserInfoID
                          where u.IsDeleted == false && e.OrderId == OrderID && u.Usertype == Convert.ToInt64(Common.DAEnums.UserTypes.IncentexAdmin)
                          && ie.MemberRole != "Super Admin"
                          select new IEListOrderNote
                          {
                              NoteReciepientID = e.NoteReciepientID,
                              EmployeeName = u.FirstName + ' ' + u.LastName,
                              LoginEmail = u.LoginEmail,
                              UserInfoID = u.UserInfoID,
                              SubscriptionFlag = e.SubscriptionFlag
                          }).ToList();

            if (IEList.Count == 0)
            {
                IEList = (from ie in db.IncentexEmployees
                          join u in db.UserInformations on ie.UserInfoID equals u.UserInfoID
                          join country in db.INC_Countries on new { iCountryID = (u.CountryId.Value == null ? -1 : u.CountryId.Value) } equals new { iCountryID = country.iCountryID }
                          join state in db.INC_States on new { iStateID = (u.StateId.Value == null ? -1 : u.StateId.Value) } equals new { iStateID = state.iStateID }
                          join city in db.INC_Cities on new { iCityID = (u.CityId.Value == null ? -1 : u.CityId.Value) } equals new { iCityID = city.iCityID }
                          where ie.MemberRole != "Super Admin" && u.IsDeleted == false
                          select new IEListOrderNote
                          {
                              NoteReciepientID = 0,
                              EmployeeName = u.FirstName + ' ' + u.LastName,
                              LoginEmail = u.LoginEmail,
                              UserInfoID = u.UserInfoID,
                              SubscriptionFlag = false
                          }).ToList();
            }
            return IEList;
        }

        public void InsertIEListIntoOrderNoteRecipient(long OrderID, long NoteID, long UserInfoID,long CreatedBy)
        {

            OrderNoteReceipientDetail objOrderNoteRecipient = new OrderNoteReceipientDetail();
            objOrderNoteRecipient.NoteID = NoteID;
            objOrderNoteRecipient.OrderId = OrderID;
            objOrderNoteRecipient.UserInfoId = UserInfoID;
            objOrderNoteRecipient.SubscriptionFlag = true;
            objOrderNoteRecipient.CreatedBy = CreatedBy;
            objOrderNoteRecipient.CreatedDate = DateTime.Now;
            db.OrderNoteReceipientDetails.InsertOnSubmit(objOrderNoteRecipient);
            base.SubmitChanges();
        }
    }

    public class IEListOrderNote
    {
        public long NoteReciepientID { get; set; }

        public string EmployeeName { get; set; }

        public string LoginEmail { get; set; }

        public long UserInfoID { get; set; }

        public bool? SubscriptionFlag { get; set; }
    }
}
