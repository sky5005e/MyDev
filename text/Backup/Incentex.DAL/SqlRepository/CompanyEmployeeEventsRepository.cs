using System;
using System.Collections.Generic;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class CompanyEmployeeEventsRepository : RepositoryBase
    {
        public CompanyEmployeeEvent GetById(Int64 EventID)
        {
            return db.CompanyEmployeeEvents.FirstOrDefault(e => e.EventID == EventID);
        }

        public List<CompanyEmployeeEvent> GetActiveEventByCompanyEmployeeId(Int64 CompanyEmployeeID)
        {
            return db.CompanyEmployeeEvents.Where(e => e.CompanyEmployeeID == CompanyEmployeeID && e.IsActive==true).ToList();
        }

        public List<CompanyEmployeeEvent> GetTodayActiveEvents(Int64 UserInfoID)
        {
            return db.CompanyEmployeeEvents.Where(e => (e.CreatedBy == UserInfoID || e.AdditionalIEID.Contains("," + UserInfoID.ToString()) || e.AdditionalIEID.Contains(UserInfoID.ToString() + ",")) && e.IsActive == true && e.EventDate == DateTime.Now).ToList();
        }

        //Delete By Id
        public void DeleteById(Int64 EventID)
        {
            var matchedEvent = db.CompanyEmployeeEvents.SingleOrDefault(e => e.EventID == EventID);
            try
            {
                if (matchedEvent != null)
                {
                    db.CompanyEmployeeEvents.DeleteOnSubmit(matchedEvent);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
