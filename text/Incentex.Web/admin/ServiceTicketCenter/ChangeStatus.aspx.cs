using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_ServiceTicketCenter_ChangeStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //hiding the top buttons & setting the date in center---------------------------------------
            ((Label)Master.FindControl("lblPageHeading")).Text = "Close Support Ticket";
            ((LinkButton)Master.FindControl("btnLogout")).Visible = false;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Visible = false;
            ((HtmlControl)Master.FindControl("imgHomeBtn")).Visible = false;
            ((HtmlControl)Master.FindControl("aOpenServiceTicket")).Visible = false;
            ((HtmlControl)Master.FindControl("aSearchServiceTicket")).Visible = false;
            ((HtmlControl)Master.FindControl("spanrealdate")).Attributes.Add("class", "date alignleft");
            ((HtmlControl)Master.FindControl("spantitle")).Style.Add("width", "38.5%");
            //------------------------------------------------------------------------------------------

            if (Request.QueryString.Count > 0 && Request.QueryString["id"] != null && Request.QueryString["uid"] != null)
            {
                lblOwnerName.Text = "Are you sure, you want to close ticket no. " + Convert.ToString(Request.QueryString["id"]) + "?";
            }
        }
    }

    /// <summary>
    /// Event for Closing Support ticket
    /// </summary>    
    protected void lnkCloseTicket_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString.Count > 0 && Request.QueryString["id"] != null && Request.QueryString["uid"] != null)
            {
                ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
                ServiceTicket objServiceTicket = objSerTicRep.GetByTicketID(Convert.ToInt64(Request.QueryString["id"]));

                if (objServiceTicket != null)
                {
                    objServiceTicket.TicketStatusID = new LookupRepository().GetIdByLookupNameNLookUpCode("Closed", "ServiceTicketStatus");
                    objServiceTicket.EndDate = DateTime.Now;
                    objServiceTicket.UpdatedBy = Convert.ToInt64(Request.QueryString["uid"]);
                    objServiceTicket.UpdatedDate = DateTime.Now;
                    objSerTicRep.SubmitChanges();

                    vw_ServiceTicket vwServiceTicket = objSerTicRep.GetFirstByID(objServiceTicket.ServiceTicketID);

                    if (vwServiceTicket != null)
                    {
                        if (vwServiceTicket.ContactID != null)
                            SendCloseTicketNotification(vwServiceTicket.ServiceTicketID, vwServiceTicket.ContactID, 1, vwServiceTicket.ContactName, vwServiceTicket.ContactEmail, vwServiceTicket.ServiceTicketNumber);
                        else if (vwServiceTicket.SupplierID != null)
                            SendCloseTicketNotification(vwServiceTicket.ServiceTicketID, vwServiceTicket.SupplierID, 3, vwServiceTicket.SupplierName, vwServiceTicket.SupplierEmail, vwServiceTicket.ServiceTicketNumber);
                        else
                            SendCloseTicketNotification(vwServiceTicket.ServiceTicketID, null, 1, "User", vwServiceTicket.TicketEmail, vwServiceTicket.ServiceTicketNumber);
                    }

                    String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);

                    NoteDetail objComNot = new NoteDetail();
                    NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();

                    objComNot.Notecontents = "Changed ticket status to : Closed.";
                    objComNot.NoteFor = strNoteFor;
                    objComNot.SpecificNoteFor = "IEActivity";
                    objComNot.ForeignKey = objServiceTicket.ServiceTicketID;
                    objComNot.CreateDate = System.DateTime.Now;
                    objComNot.CreatedBy = Convert.ToInt64(Request.QueryString["uid"]);
                    objComNot.UpdateDate = System.DateTime.Now;
                    objComNot.UpdatedBy = Convert.ToInt64(Request.QueryString["uid"]);

                    objCompNoteHistRepos.Insert(objComNot);
                    objCompNoteHistRepos.SubmitChanges();

                    lblMsg.Text = "Support ticket no. " + Convert.ToString(Request.QueryString["id"]) + " has been closed successfully.";
                    lblOwnerName.Visible = false;
                    lnkCloseTicket.Visible = false;
                }
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            lblMsg.Text = "Support ticket no. " + Convert.ToString(Request.QueryString["id"]) + " could not be closed.";
        }
    }

    private void SendCloseTicketNotification(Int64? TicketID, Int64? tUserInfoID, Int32 NoteType, String FullName, String Email, String TicketNo)
    {
        try
        {
            String eMailTemplate = String.Empty;
            String sSubject = "Your Support Ticket Has Been Closed";

            String sReplyToadd = Common.ReplyTo;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/CloseTicket.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            String sUserInfoID = tUserInfoID != null ? Convert.ToString(tUserInfoID) : "annx";

            StringBuilder MessageBody = new StringBuilder(eMailTemplate);

            MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            MessageBody.Replace("{FullName}", FullName);
            MessageBody.Replace("{TicketNo}", TicketNo);

            //using (MailMessage objEmail = new MailMessage())
            //{
            //    objEmail.Body = MessageBody.ToString();
            //    objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
            //    objEmail.IsBodyHtml = true;
            //    objEmail.ReplyTo = new MailAddress(sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + TicketID + "un" + sUserInfoID + "nt" + NoteType + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@')));
            //    objEmail.Subject = sSubject;
            //    objEmail.To.Add(new MailAddress(Email));

            //    SmtpClient objSmtp = new SmtpClient();

            //    objSmtp.EnableSsl = Common.SSL;
            //    objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
            //    objSmtp.Host = Common.SMTPHost;
            //    objSmtp.Port = Common.SMTPPort;

            //    objSmtp.Send(objEmail);
            //}

            String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + TicketID + "un" + sUserInfoID + "nt" + NoteType + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
            new CommonMails().SendMailWithReplyTo(Convert.ToInt64(sUserInfoID), "Support Ticket Center", Common.EmailFrom, Email, sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}