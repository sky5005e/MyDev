using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Text;
using System.Configuration;

public partial class controller_SendEmailScheduler : System.Web.UI.Page
{
    
    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            EmailMarketingRepository ObjEmailRepo = new EmailMarketingRepository();

            List<SendLaterEmail> ObjTemplate = ObjEmailRepo.GetScheduledEmails();
            if (ObjTemplate.Count > 0)
            {
                foreach (var item in ObjTemplate)
                {
                    string sFrmadd = "support@world-link.us.com";
                    string sToadd = string.Empty;
                    string sSubject = "Incentex - Pending Items in Shopping Cart";
                    string sFrmname = "Incentex Order Processing Team";
                    string smtphost = Application["SMTPHOST"].ToString();
                    int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                    string smtpUserID = Application["SMTPUSERID"].ToString();
                    string smtppassword = Application["SMTPPASSWORD"].ToString();

                    EmailMarketingRepository objEmailMarkRepo = new EmailMarketingRepository();
                    EmailTemplete ObjEmailTemp = objEmailMarkRepo.GetEmailTemplate(item.TemplateID);

                    System.Net.WebClient MyWebClient = new System.Net.WebClient();
                    String eMailTemplate = MyWebClient.DownloadString(Server.MapPath("~/UploadedImages/EmailTempletes/") + ObjEmailTemp.TempFile);

                    if (!string.IsNullOrEmpty(item.ToAddress))
                    {
                        if (HttpContext.Current.Request.IsLocal)
                        {
                            new CommonMails().SendEmail4Local(1092, "Testing", "navin.valera@indianic.com", sSubject, eMailTemplate, "incentextest6@gmail.com", "test6incentex", true, "Email Marketing", "Email Scheduler");
                        }
                        else
                            new CommonMails().SendMail(Common.UserID, "Email Template", sFrmadd, sToadd, sSubject, eMailTemplate, sFrmname, smtphost, smtpport, false, true);
                    }
                    else
                    {
                        List<UserInformation> LstUsers = objEmailMarkRepo.GetUsersWithWorkgroup(item.WorkgroupID.Value);
                        foreach (var usr in LstUsers)
                        {
                            if (HttpContext.Current.Request.IsLocal)
                            {
                                new CommonMails().SendEmail4Local(1092, "Testing", "navin.valera@indianic.com", sSubject, eMailTemplate, "incentextest6@gmail.com", "test6incentex", true, "Email Marketing", "Email Scheduler");
                            }
                            else
                                new CommonMails().SendMail(Common.UserID, "Email Template", sFrmadd, usr.Email, sSubject, eMailTemplate, sFrmname, smtphost, smtpport, false, true);
                        }
                    }
                    ObjEmailRepo.UpdateSentStatus(item.SendLaterID);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion

    #region Methods
    private void sendTemplateEmails(Int64 UserInfoID, String UserEmail, String UserName, String ModuleName, String MenuName)
    {

    } 
    #endregion
}
