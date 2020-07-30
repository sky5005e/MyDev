using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using Incentex.DAL;

public partial class controller_NoSalesOrderUpdateRequestsFromSAPYesterday : Page
{
    protected void Page_Load(Object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (DateTime.Now.AddDays(-1).DayOfWeek != DayOfWeek.Saturday && DateTime.Now.AddDays(-1).DayOfWeek != DayOfWeek.Sunday)
                {
                    IncentexBEDataContext db = new IncentexBEDataContext();
                    SAPSalesOrderUpdateRequest objYesterdaysRequests = db.SAPSalesOrderUpdateRequests.FirstOrDefault(le => le.RequestDate.Date == DateTime.Now.AddDays(-1).Date);
                    if (objYesterdaysRequests == null || (objYesterdaysRequests != null && Convert.ToInt32(objYesterdaysRequests.NoOfRequests) == 0))
                    {
                        using (MailMessage objEmail = new MailMessage())
                        {
                            StringBuilder Body = new StringBuilder("No Sales Order Update request has been received by World-Link on " + DateTime.Now.AddDays(-1).ToString("MMM dd, yyyy") + ".");
                            Body.Append("<br/><br/>Please check and resolve (if) any issue on <b>SAP</b> side.");
                            Body.Append("<br/><br/>Thanks");
                            Body.Append("<br/>World-Link System");
                            Body.Append("<br/><br/><b>Note :</b> Do not reply to this email, it has been sent from an un-monitored email address.");

                            objEmail.Body = Body.ToString();
                            objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                            objEmail.IsBodyHtml = true;
                            objEmail.Priority = MailPriority.High;
                            objEmail.Subject = "World-Link (" + (Common.Live ? "live" : "test") + ") : No Sales Order Update Request received, " + DateTime.Now.AddDays(-1).ToString("MMM dd, yyyy") + ".";
                            objEmail.To.Add(Convert.ToString(ConfigurationManager.AppSettings["SAPOperationFailedNotifyList"]));

                            SmtpClient objSmtp = new SmtpClient();

                            objSmtp.EnableSsl = Common.SSL;

                            if (!Common.Live)
                                objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);

                            objSmtp.Host = Common.SMTPHost;
                            objSmtp.Port = Common.SMTPPort;

                            objSmtp.Send(objEmail);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}