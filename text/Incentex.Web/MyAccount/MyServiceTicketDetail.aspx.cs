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
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Net;

public partial class MyAccount_MyServiceTicketDetail : PageBase
{
    #region Page Properties

    ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
    ServiceTicketAttachmentRepository objSerTicAttRep = new ServiceTicketAttachmentRepository();
    Int32 sizeCount = 0;

    Int64 lServiceTicketID
    {
        get
        {
            if (ViewState["ServiceTicketID"] == null)
            {
                ViewState["ServiceTicketID"] = 0;
            }
            return Convert.ToInt64(ViewState["ServiceTicketID"]);
        }
        set
        {
            ViewState["ServiceTicketID"] = value;
        }
    }

    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();

            ((Label)Master.FindControl("lblPageHeading")).Text = "My Support Ticket Detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.SearchTicketURL != null ? IncentexGlobal.SearchTicketURL.Contains("ex=1") ? IncentexGlobal.SearchTicketURL : IncentexGlobal.SearchTicketURL.Contains("?") ? IncentexGlobal.SearchTicketURL + "&ex=1" : IncentexGlobal.SearchTicketURL + "?ex=1" : "~/MyAccount/TrackServiceTicket.aspx";

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
            {
                vw_ServiceTicket objServiceTicket = new vw_ServiceTicket();

                lServiceTicketID = Convert.ToInt64(Request.QueryString["id"]);
                objServiceTicket = objSerTicRep.GetFirstByID(lServiceTicketID);

                if (objServiceTicket != null)
                {
                    lblServiceTicketName.Text = Convert.ToString(objServiceTicket.ServiceTicketName);
                    lblServiceTicketNumber.Text = Convert.ToString(objServiceTicket.ServiceTicketNumber);
                    lblStartDate.Text = Convert.ToString(objServiceTicket.StartDateNTime);
                    lblEndDate.Text = Convert.ToString(objServiceTicket.DatePromisedFormatted);

                    if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                    {
                        lblContact.Text = Convert.ToString(objServiceTicket.ContactName);
                        lblReason.InnerText = "Contact :";
                    }
                    else
                    {
                        lblContact.Text = Convert.ToString(objServiceTicket.Reason);
                        lblReason.InnerText = "Reason :";
                    }

                    lblStatus.Text = Convert.ToString(objServiceTicket.TicketStatus);
                    DisplayNotes();

                    List<ServiceTicketAttachment> objAttachmentList = new List<ServiceTicketAttachment>() { };
                    objAttachmentList = objSerTicAttRep.GetByServiceTicektId(this.lServiceTicketID);

                    List<UploadFile> objFileList = new List<UploadFile>();

                    foreach (ServiceTicketAttachment objAttachment in objAttachmentList)
                    {
                        UploadFile objFile = new UploadFile();
                        if (ViewState["ListValues"] != null)
                        {
                            objFileList = (List<UploadFile>)ViewState["ListValues"];
                            objFile.OnlyFileName = Convert.ToString(objAttachment.AttachmentFileName);
                            objFile.SavedFileName = Convert.ToString(objAttachment.SavedFileName);
                            objFile.AttachmentID = Convert.ToInt64(objAttachment.AttachmentID);
                            objFileList.Add(objFile);
                            ViewState["ListValues"] = objFileList;
                        }
                        else
                        {
                            objFile.OnlyFileName = Convert.ToString(objAttachment.AttachmentFileName);
                            objFile.SavedFileName = Convert.ToString(objAttachment.SavedFileName);
                            objFile.AttachmentID = Convert.ToInt64(objAttachment.AttachmentID);
                            objFileList.Add(objFile);
                            ViewState["ListValues"] = objFileList;
                        }
                    }

                    //For marking all unread notes as read
                    new ServiceTicketRepository().UpdateNoteReadFlag(objServiceTicket.ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, new ServiceTicketRepository().GetUnreadNotes(objServiceTicket.ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID).ToList());
                }
            }
        }

        BindFilesFromViewState();
    }

    #endregion

    #region Page Control Events

    protected void lnkButton_Click(object sender, EventArgs e)
    {
        try
        {
            String NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs);
            objSerTicRep.InsertTicketNote(this.lServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtNote.Text.Trim()), NoteFor, null);
            SendEmailToCECAIE();
            txtNote.Text = "";
            DisplayNotes();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(fpAttachment.Value))
        {
            lblAttachmentMsg.Text = "";
            String SavedFileName = "serviceticket_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fpAttachment.Value;
            SavedFileName = Common.MakeValidFileName(SavedFileName);
            String filePath = Common.ServieTicketAttachmentPath + SavedFileName;

            sizeCount = sizeCount + (fpAttachment.PostedFile.ContentLength / 1024);
            ViewState["CountSize"] = Convert.ToInt32(ViewState["CountSize"]) + sizeCount;
            if (!string.IsNullOrEmpty(fpAttachment.Value))
            {
                fpAttachment.PostedFile.SaveAs(filePath);
            }

            ServiceTicketAttachment objServiceTicketAttachemt = new ServiceTicketAttachment();
            objServiceTicketAttachemt.AttachmentFileName = fpAttachment.Value;
            objServiceTicketAttachemt.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objServiceTicketAttachemt.CreatedDate = DateTime.Now;
            objServiceTicketAttachemt.SavedFileName = SavedFileName;
            objServiceTicketAttachemt.ServiceTicketID = this.lServiceTicketID;
            objSerTicAttRep.Insert(objServiceTicketAttachemt);
            objSerTicAttRep.SubmitChanges();

            GenerateTableGrid(fpAttachment.Value, SavedFileName, objServiceTicketAttachemt.AttachmentID);

            lblAttachmentMsg.Visible = true;
            if (Convert.ToInt64(ViewState["CountSize"]) > 18000)
            {
                lblAttachmentMsg.Text = "Attachment(s) must be less then 18 MB";
            }
            else
            {
                lblAttachmentMsg.Text = "Attachment uploaded successfully.";
            }
        }

        modalAttachments.Show();
    }

    protected void grvAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow row;
        row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
        List<UploadFile> objFileList = new List<UploadFile>();

        if (ViewState["ListValues"] != null)
        {
            objFileList = (List<UploadFile>)ViewState["ListValues"];

            if (File.Exists(Common.ServieTicketAttachmentPath + objFileList[row.RowIndex].SavedFileName))
            {
                if (e.CommandName == "DeleteAttachment")
                {
                    //File.Delete(Common.ServieTicketAttachmentPath + objFileList[row.RowIndex].SavedFileName);

                    objSerTicAttRep.DeleteByAttachmentID(objFileList[row.RowIndex].AttachmentID, IncentexGlobal.CurrentMember.UserInfoID);
                    objSerTicAttRep.SubmitChanges();
                    LogAction("Deleted an attachment : " + objFileList[row.RowIndex].OnlyFileName, "IEActivity");
                    objFileList.RemoveAt(row.RowIndex);
                    ViewState["ListValues"] = objFileList;
                    BindFilesFromViewState();
                    modalAttachments.Show();
                }

                if (e.CommandName == "download")
                {
                    String filePath = Common.ServieTicketAttachmentPath + objFileList[row.RowIndex].SavedFileName;
                    DownloadFile(filePath, objFileList[row.RowIndex].OnlyFileName);
                }
            }
            else
            {
                BindFilesFromViewState();
                lblAttachmentMsg.Visible = true;
                lblAttachmentMsg.Text = "File not found.";
                modalAttachments.Show();
            }
        }
    }

    #endregion

    #region Page Methods

    public void DisplayNotes()
    {
        try
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            txtOrderNotesForCECA.Text = objSerTicRep.TrailingNotes(this.lServiceTicketID, 1, false, "\n");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void GenerateTableGrid(String FileName, String SavedFileName, long AttachmentID)
    {
        List<UploadFile> objFileList = new List<UploadFile>();
        UploadFile objFile = new UploadFile();

        if (ViewState["ListValues"] != null)
        {
            objFileList = (List<UploadFile>)ViewState["ListValues"];
            objFile.OnlyFileName = FileName;
            objFile.SavedFileName = SavedFileName;
            objFile.AttachmentID = AttachmentID;
            objFileList.Add(objFile);
            ViewState["ListValues"] = objFileList;
        }
        else
        {
            objFile.OnlyFileName = FileName;
            objFile.SavedFileName = SavedFileName;
            objFile.AttachmentID = AttachmentID;
            objFileList.Add(objFile);
            ViewState["ListValues"] = objFileList;
        }

        BindFilesFromViewState();
    }

    private void BindFilesFromViewState()
    {
        List<UploadFile> objFileList = new List<UploadFile>();
        if (ViewState["ListValues"] != null)
        {
            objFileList = (List<UploadFile>)ViewState["ListValues"];
            grvAttachment.DataSource = objFileList;
            grvAttachment.DataBind();
            atSpan.InnerText = "Attachment(s) (" + objFileList.Count + ")";
            lblAttachmentMsg.Visible = false;
        }
        else
        {
            lblAttachmentMsg.Visible = true;
            lblAttachmentMsg.Text = "No attachments.";
            atSpan.InnerText = "Attachment(s) (0)";
        }
    }

    protected void DownloadFile(string filepath, string displayFileName)
    {
        System.IO.Stream iStream = null;
        string type = "";
        string ext = Path.GetExtension(filepath);

        // Buffer to read 10K bytes in chunk:
        byte[] buffer = new Byte[10000];

        // Total bytes to read:
        long dataToRead;

        // Identify the file name.
        string filename = System.IO.Path.GetFileName(filepath);

        try
        {
            // Open the file.
            iStream = new System.IO.FileStream(filepath, System.IO.FileMode.Open,
            System.IO.FileAccess.Read, System.IO.FileShare.Read);

            // Total bytes to read:
            dataToRead = iStream.Length;
            if (ext != null)
            {
                switch (ext.ToLower())
                {
                    case ".pdf":
                        type = "Application/pdf";
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".gif":
                    case ".tiff":
                        type = "image/gif";
                        break;
                    case ".png":
                        type = "image/png";
                        break;
                    case ".bmp":
                        type = "image/bmp";
                        break;
                    case ".ai":
                        type = "application/illustrator";
                        break;
                    case ".epf":
                        type = "application/postscript";
                        break;
                }
            }

            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + displayFileName + "\"");
            Response.ContentType = type;
            Response.WriteFile(filepath);
            Response.End();
        }
        catch (Exception ex)
        {
            // Trap the error, if any.
            Response.Write("Error : " + ex.Message);
        }
        finally
        {
            if (iStream != null)
            {
                //Close the file.
                iStream.Close();
            }
        }
    }

    private Int64 LogAction(String Content, String SpecificNoteFor)
    {
        String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs);

        NoteDetail objComNot = new NoteDetail();
        NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();

        objComNot.Notecontents = Content;
        objComNot.NoteFor = strNoteFor;
        objComNot.SpecificNoteFor = SpecificNoteFor;
        objComNot.ForeignKey = lServiceTicketID;
        objComNot.CreateDate = System.DateTime.Now;
        objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objComNot.UpdateDate = System.DateTime.Now;
        objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

        objCompNoteHistRepos.Insert(objComNot);
        objCompNoteHistRepos.SubmitChanges();

        return objComNot.NoteID;
    }

    private void SendEmailToCECAIE()
    {
        ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
        vw_ServiceTicket objServiceTicket = objSerTicRep.GetFirstByID(Convert.ToInt64(this.lServiceTicketID));

        if (objServiceTicket != null)
        {
            String eMailTemplate = String.Empty;
            String sSubject = objServiceTicket.ServiceTicketName + " - Support Ticket - " + objServiceTicket.ServiceTicketNumber;

            String sReplyToadd = Common.ReplyTo;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ServiceTicketNote.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            String TrailingNotesIE = new ServiceTicketRepository().TrailingNotes(objServiceTicket.ServiceTicketID, 2, false, "<br/>");
            String TrailingNotes = new ServiceTicketRepository().TrailingNotes(objServiceTicket.ServiceTicketID, 1, false, "<br/>");

            List<vw_ServiceTicketNoteRecipient> lstRecipients = new List<vw_ServiceTicketNoteRecipient>();
            lstRecipients = new ServiceTicketRepository().GetNoteRecipientsByTicketID(objServiceTicket.ServiceTicketID).Where(le => le.SubscriptionFlag == true).ToList();
            foreach (vw_ServiceTicketNoteRecipient recipient in lstRecipients)
            {
                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                {
                    String UserName = Convert.ToString(recipient.LastName) + " " + Convert.ToString(recipient.FirstName);
                    StringBuilder MessageBody = new StringBuilder(eMailTemplate);
                    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
                    MessageBody.Replace("{TicketNo}", Convert.ToString(objServiceTicket.ServiceTicketID));
                    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                    MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);
                    MessageBody.Replace("{Note}", recipient.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || recipient.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) ? TrailingNotesIE : TrailingNotes);
                    MessageBody.Replace("{CloseTicket}", String.Empty);

                    String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(objServiceTicket.ServiceTicketID) + "un" + Convert.ToString(recipient.UserInfoID) + "nt1en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                    new CommonMails().SendMailWithReplyTo(Convert.ToInt64(recipient.UserInfoID), "Support Ticket Center", Common.EmailFrom, Convert.ToString(recipient.Email), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
                }
            }
        }
    }

    #endregion
}