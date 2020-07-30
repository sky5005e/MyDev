using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL.Common;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;
public partial class controller_GSEAssetMailAlert : PageBase
{
    AssetVendorRepository objAssetVendorRepository = new AssetVendorRepository();
    AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
    EquipmentMaster objEquipmentMaster = new EquipmentMaster();
    Int64 EquipmentMasterId
    {
        get
        {
            if (ViewState["EquipmentMasterId"] == null)
            {
                ViewState["EquipmentMasterId"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentMasterId"]);
        }
        set
        {
            ViewState["EquipmentMasterId"] = value;
        }
    }
    private String EquipmentID
    {
        get
        {
            if (Convert.ToString(this.ViewState["EquipmentID"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["EquipmentID"]);
        }
        set
        {
            this.ViewState["EquipmentID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
       
        
        try
        {
            List<EquipmentMaster> objlstEquipment = new List<EquipmentMaster>();
            objlstEquipment = objAssetMgtRepository.GetAll().ToList();

            foreach (EquipmentMaster obj in objlstEquipment)
            {
                this.EquipmentMasterId = obj.EquipmentMasterID;
                this.EquipmentID = obj.EquipmentID;
                CheckExpiration();
            }

            #region Email Notification
            using (MailMessage objEmail = new MailMessage())
            {
                objEmail.Body = "The GSE Mail Alert Scheduler has run successfully for the day.";
                objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                objEmail.IsBodyHtml = true;
                objEmail.Subject = "GSE Mail Alert Scheduler - Notification";
                objEmail.To.Add(new MailAddress("testforind@gmail.com"));

                SmtpClient objSmtp = new SmtpClient();

                objSmtp.EnableSsl = Common.SSL;
                objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                objSmtp.Host = Common.SMTPHost;
                objSmtp.Port = Common.SMTPPort;

                objSmtp.Send(objEmail);
            }
            #endregion

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    public void CheckExpiration()
    {
        try
        {
            objEquipmentMaster = objAssetMgtRepository.GetById(EquipmentMasterId);
            DateTime ExpireOn = Convert.ToDateTime(objEquipmentMaster.ExpireOn, System.Globalization.CultureInfo.CurrentCulture);
            TimeSpan DateDiff = Convert.ToDateTime(objEquipmentMaster.ExpireOn).Subtract(DateTime.Now);

           
            //Insurance Start
            if (objEquipmentMaster.ExpireOn != null)
            {
                if (DateDiff.Days < 0)
                {

                    NotesOnColorChange(Convert.ToString(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.InsuranceRecords), true);
                }
                else
                {
                    if (DateDiff.Days <= 30)
                    {

                        NotesOnColorChange(Convert.ToString(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.InsuranceRecords), false);
                    }
                  
                }
            }
           
            //Insurance End
            //Registration Start
            DateDiff = Convert.ToDateTime(objEquipmentMaster.RegistrationExpiryDate).Subtract(DateTime.Now);
            if (objEquipmentMaster.RegistrationExpiryDate != null)
            {
                if (DateDiff.Days < 0)
                {
                    NotesOnColorChange(Convert.ToString(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.Registration), true);
                }
                else
                {
                    if (DateDiff.Days <= 30)
                    {
                        NotesOnColorChange(Convert.ToString(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.Registration), false);
                    }                   
                }
            }
           
            //Registration End
        }
        catch (Exception)
        {
            
          
        }
    }
    public void NotesOnColorChange(string NoteFor, bool IsExpired)
    {
        try
        {
            string NoteContent = string.Empty;
            Int64 SpecificNoteFor = 0;
            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement);

            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();
            if (NoteFor == Convert.ToString(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.InsuranceRecords))
            {
                SpecificNoteFor = Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.InsuranceRecords);
                if (IsExpired)
                {
                    objComNot.Notecontents = "Your Insurance on asset (" + Convert.ToString(this.EquipmentID) + ") has expired.";
                 }
                else
                {
                    objComNot.Notecontents = "Your Insurance on asset (" + Convert.ToString(this.EquipmentID) + ") is about to expire in 30 days.";
                }
            }
            else
            {
                SpecificNoteFor = Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.Registration);
                if (IsExpired)
                {
                    objComNot.Notecontents = "Your Registration on asset (" + Convert.ToString(this.EquipmentID) + ") has expired.";
                }
                else
                {
                    objComNot.Notecontents = "Your Registration on asset (" + Convert.ToString(this.EquipmentID) + ") is about to expire in 30 days.";                    
                }
            }
            NoteContent = objComNot.Notecontents;
            objComNot.NoteFor = strNoteFor;
            objComNot.ForeignKey = Convert.ToInt64(EquipmentMasterId);
            objComNot.SpecificNoteFor = Convert.ToString(SpecificNoteFor);
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            // strNoteHistory = txtOrderNotesHistory.Text;
            objCompNoteHistRepos.Insert(objComNot);
            objCompNoteHistRepos.SubmitChanges();
            int NoteId = (int)(objComNot.NoteID);
            SendEmailOnColorChange(NoteContent, SpecificNoteFor);

        }
        catch (Exception)
        {

        }
    }
    private void SendEmailOnColorChange(string strNoteHistory, Int64 EmailFor)
    {

        String eMailTemplate = String.Empty;
        String sSubject = string.Empty;
        if (EmailFor == Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEquipmentEmail.InsuranceRecords))
        {
            sSubject = " Insurance Alert - " + this.EquipmentID;
        }
        else
        {
            sSubject = " Registration Alert - " + this.EquipmentID;
        }


        // String sReplyToadd = Common.ReplyToGSE;
        String sReplyToadd = "replytoGSE@world-link.us.com";

        StreamReader _StreamReader;
        _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/GSEAssetMgtNote.htm"));
        eMailTemplate = _StreamReader.ReadToEnd();
        _StreamReader.Close();
        _StreamReader.Dispose();


        List<GetGSEUsersResult> lstRecipients = new List<GetGSEUsersResult>();
        lstRecipients = new AssetMgtRepository().GetGSEUsers().ToList();
        foreach (GetGSEUsersResult recipient in lstRecipients)
        {
            //Email Management
            if (objAssetVendorRepository.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), EmailFor) == true)
            //if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
            {
                String UserName = Convert.ToString(recipient.LastName) + " " + Convert.ToString(recipient.FirstName);
                StringBuilder MessageBody = new StringBuilder(eMailTemplate);
                MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
                MessageBody.Replace("{EquipmentID}", Convert.ToString(this.EquipmentID));
                MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                MessageBody.Replace("{Note}", "<br/>" + strNoteHistory);
                MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);

                using (MailMessage objEmail = new MailMessage())
                {
                    objEmail.Body = MessageBody.ToString();
                    objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                    objEmail.IsBodyHtml = true;
                    //objEmail.ReplyTo = new MailAddress(sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(this.EquipmentMasterId) + "un" + Convert.ToString(recipient.UserInfoID) + "nt1en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@')));
                    objEmail.ReplyTo = new MailAddress(sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(this.EquipmentMasterId) + "un" + Convert.ToString(recipient.UserInfoID) + "nt" + Convert.ToString(EmailFor) + "en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@')));
                    objEmail.Subject = sSubject;
                    objEmail.To.Add(new MailAddress(Convert.ToString(recipient.LoginEmail)));

                    SmtpClient objSmtp = new SmtpClient();

                    objSmtp.EnableSsl = Common.SSL;
                    objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                    objSmtp.Host = Common.SMTPHost;
                    objSmtp.Port = Common.SMTPPort;

                    objSmtp.Send(objEmail);
                }
            }
        }

    }
}
