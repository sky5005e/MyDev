using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class EmailMarketingRepository : RepositoryBase
    {
        public List<GetEmailDetailsResult> GetEmailDetails(Int64? CompanyId, Int64? UserId, Int64? WorkgroupId, String ModuleName, String EmailTitle, string FromDate, string ToDate, string keyWord, Int32 PageSize, Int32 PageIndex, string SortColumn, string SortDirection)
        {
            return db.GetEmailDetails(CompanyId, null, WorkgroupId, ModuleName, EmailTitle, FromDate, ToDate, keyWord, PageSize, PageIndex, SortColumn, SortDirection).ToList();
        }

        public List<GetEmailHisoryResult> GetHistoryDetails(Int64? CompanyId, Int64? UserId, string FromDateSent, string ToDateSent, string FromDateView, string ToDateView, string keyWord, Int32 PageSize, Int32 PageIndex, string SortColumn, string SortDirection)
        {
            return db.GetEmailHisory(CompanyId, UserId, FromDateSent, ToDateSent, FromDateView, ToDateView, keyWord, PageSize, PageIndex, SortColumn, SortDirection).ToList();
        }

        public List<EmailTitle> GetEmailTitle()
        {
            return db.TodaysEmailDetails.Select(x => new { x.EmailSubject })
                .Distinct()
                .Select(x => new EmailTitle()
                {
                    EmailSubject = x.EmailSubject
                })
                .OrderBy(x => x.EmailSubject)
                .ToList();
        }

        public List<ModuleNames> GetModuleName()
        {
            return db.TodaysEmailDetails.Select(x => new { x.ModuleName })
                .Distinct()
                .Where(x => x.ModuleName != null && x.ModuleName != string.Empty)
                .Select(x => new ModuleNames()
                {
                    ModuleName = x.ModuleName
                })
                .OrderBy(x => x.ModuleName)
                .ToList();
        }        

        public List<UserInformation> GetUsersWithWorkgroup(Int64 WorkgorupID)
        {
            return db.UserInformations.Join(db.CompanyEmployees.Where(x => x.WorkgroupID == WorkgorupID), ui => ui.UserInfoID, ce => ce.UserInfoID, (ui, ce) => ui).ToList();
        }

        public void AddSendLaterData(DateTime EmailSendDateTime, String ToAddress, String ToName, Int64? CompanyId, Int64? WorkgroupId, String MailBody)
        {
            db.SendLaterEmails.InsertOnSubmit(new SendLaterEmail()
            {
                EmailSendDate = EmailSendDateTime,
                ToAddress = ToAddress,
                ToName = ToName,
                CompanyID = CompanyId,
                WorkgroupID = WorkgroupId,
                EmailBody = MailBody
            });
            db.SubmitChanges();
        }

        public List<SendLaterEmail> GetScheduledEmails()
        {
            return db.SendLaterEmails.Where(x => (x.SentStatus != "Sent Successfully" || x.SentStatus == null) && x.EmailSendDate < DateTime.Now).ToList();
        }

        public void UpdateSentStatus(Int64 SendLaterID)
        {
            SendLaterEmail objEmail = db.SendLaterEmails.Where(x => x.SendLaterID == SendLaterID).SingleOrDefault();
            objEmail.SentStatus = "Sent Successfully";
            db.SubmitChanges();
        }

        public EmailTemplete GetEmailTemplate(Int64 TempID)
        {
            return db.EmailTempletes.Where(x => x.TempID == TempID).SingleOrDefault();
        }
    }

    public class EmailTitle
    {
        public String EmailSubject { get; set; }
    }

    public class ModuleNames
    {
        public String ModuleName { get; set; }
    }
}
