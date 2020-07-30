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
using System.Globalization;
using System.IO;
using System.Text;

public partial class admin_ServiceTicketCenter_OpenServiceTicket : PageBase
{
    #region Page Properties

    ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
    ServiceTicketAttachmentRepository objSerTicAttRep = new ServiceTicketAttachmentRepository();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    int sizeCount = 0;

    private List<int> ListValues
    {
        get
        {

            if (ViewState["ListValues"] == null)
                return new List<int>(50);
            else
                return ((List<int>)ViewState["ListValues"]);

        }
        set
        {
            ViewState["ListValues"] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Support Ticket Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            this.Title = "Open Support Ticket";
            ((Label)Master.FindControl("lblPageHeading")).Text = "Open Support Ticket";

            if (IncentexGlobal.CurrentMember != null && (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin)))
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/ServiceTicketCenter.aspx";
                FillServiceTicketOwner();
                FillCompanyName();
                FillSupplier();
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            }

            lblMessage.Text = "No attachments.";

            txtServiceTicketName.Focus();
        }
    }

    #endregion

    #region Page Control Events

    /// <summary>
    /// Event for Saving/Opening Support ticket
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkBtnStartServicingNow_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 ServiceTicketID = 0;

            Int64? OwnerID = null;

            Int64? ContactID = null;
            Int64? CompanyID = null;
            Int64? SupplierID = null;

            UserInformationRepository objUserRepo = new UserInformationRepository();

            if (!String.IsNullOrEmpty(ddlCompanyName.SelectedValue) && Convert.ToInt64(ddlCompanyName.SelectedValue) > 0)
            {
                CompanyID = Convert.ToInt64(ddlCompanyName.SelectedValue);
                if (!String.IsNullOrEmpty(ddlContactName.SelectedValue) && Convert.ToInt64(ddlContactName.SelectedValue) > 0)
                {
                    ContactID = Convert.ToInt64(ddlContactName.SelectedValue);
                }
            }

            if (!String.IsNullOrEmpty(ddlSupplier.SelectedValue) && Convert.ToInt64(ddlSupplier.SelectedValue) > 0)
            {
                SupplierID = Convert.ToInt64(ddlSupplier.SelectedValue);
            }

            if (!String.IsNullOrEmpty(ddlServiceTicketOwner.SelectedValue) && Convert.ToInt64(ddlServiceTicketOwner.SelectedValue) > 0)
            {
                OwnerID = Convert.ToInt64(ddlServiceTicketOwner.SelectedValue);
            }

            String NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);
            Int32 NoteType = 2;

            ServiceTicketID = objSerTicRep.InsertTicket(IncentexGlobal.CurrentMember.UserInfoID, CompanyID, ContactID, SupplierID, OwnerID, Convert.ToString(txtDatePromised.Text.Trim()), Convert.ToString(txtServiceTicketName.Text.Trim()), Convert.ToString(txtServiceTicketDetails.InnerText.Trim()));

            #region For Note Insertion

            objSerTicRep.InsertTicketNote(ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, "Opened Support Ticket.", NoteFor, "IEActivity");

            if (!String.IsNullOrEmpty(txtServiceTicketDetails.InnerText.Trim()))
            {
                if (ContactID == null && SupplierID == null)
                {
                    objSerTicRep.InsertTicketNote(ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtServiceTicketDetails.InnerText.Trim()), NoteFor, "IEInternalNotes");
                }
                else
                {
                    if (ContactID != null)
                    {
                        NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs);
                        objSerTicRep.InsertTicketNote(ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtServiceTicketDetails.InnerText.Trim()), NoteFor, null);
                        NoteType = 1;
                    }
                    else if (SupplierID != null)
                    {
                        NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketSupps);
                        objSerTicRep.InsertTicketNote(ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtServiceTicketDetails.InnerText.Trim()), NoteFor, null);
                        NoteType = 3;
                    }
                }
            }

            #endregion

            if (ViewState["ListValues"] != null)
            {
                List<UploadFile> objFileList = new List<UploadFile>();
                objFileList = (List<UploadFile>)ViewState["ListValues"];

                for (int iInt = 0; iInt < objFileList.Count; iInt++)
                {
                    ServiceTicketAttachment objServiceTicketAttachemt = new ServiceTicketAttachment();
                    objServiceTicketAttachemt.AttachmentFileName = objFileList[iInt].OnlyFileName;
                    objServiceTicketAttachemt.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                    objServiceTicketAttachemt.CreatedDate = DateTime.Now;
                    objServiceTicketAttachemt.SavedFileName = objFileList[iInt].SavedFileName;
                    objServiceTicketAttachemt.ServiceTicketID = ServiceTicketID;
                    objSerTicAttRep.Insert(objServiceTicketAttachemt);
                }

                objSerTicAttRep.SubmitChanges();
            }

            #region For Sending Emails

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ServiceTicketNote.htm"));
            String eMailTemplate = String.Empty;
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            CommonMails objCommonMail = new CommonMails();
            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            String CloseTicket = String.Empty;

            UserInformation objOwner = objUserRepo.GetById(OwnerID);

            #region Send Receipt

            //Email Management                
            if (objManageEmailRepo.CheckEmailAuthentication(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
            {
                MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                MessageBody.Replace("{FullName}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);
                MessageBody.Replace("{TicketNo}", Convert.ToString(ServiceTicketID));
                MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                MessageBody.Replace("{Sender}", "World-Link System");
                MessageBody.Replace("{Note}", "<br/>" + Convert.ToString(txtServiceTicketDetails.InnerText.Trim()));

                CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                        + ServiceTicketID + "&uid=" + IncentexGlobal.CurrentMember.UserInfoID
                                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                MessageBody.Replace("{CloseTicket}", CloseTicket);

                objCommonMail.SendServiceTicketReciept(ServiceTicketID, Convert.ToString(IncentexGlobal.CurrentMember.UserInfoID), "Support Ticket Center", NoteType, IncentexGlobal.CurrentMember.Email, MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(txtServiceTicketName.Text.Trim()));
            }

            #endregion

            #region Send Alert To Owner

            //Email Management                
            if (objManageEmailRepo.CheckEmailAuthentication(objOwner.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
            {
                MessageBody = new StringBuilder(eMailTemplate);
                MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                MessageBody.Replace("{FullName}", objOwner.FirstName + " " + objOwner.LastName);
                MessageBody.Replace("{TicketNo}", Convert.ToString(ServiceTicketID));
                MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                MessageBody.Replace("{Sender}", IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName);
                MessageBody.Replace("{Note}", "<br/>" + Convert.ToString(txtServiceTicketDetails.InnerText.Trim()));

                CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                        + ServiceTicketID + "&uid=" + objOwner.UserInfoID
                                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                MessageBody.Replace("{CloseTicket}", CloseTicket);

                objCommonMail.SendServiceTicketReciept(ServiceTicketID, Convert.ToString(objOwner.UserInfoID), "Support Ticket Center", NoteType, objOwner.Email, MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(txtServiceTicketName.Text.Trim()));
            }

            #endregion

            #endregion

            ClearControls();

            lblMsg.Text = "Support ticket opened successfully...";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /* Change here start*/
    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        try
        {
            if (((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblMessage.Text = "The file you are uploading is more than 2MB.";
                modalAttachments.Show();
                return;
            }

            if (!String.IsNullOrEmpty(fpAttachment.Value))
            {
                lblMessage.Text = "";
                String SavedFileName = "serviceticket_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fpAttachment.Value;
                SavedFileName = Common.MakeValidFileName(SavedFileName);
                String filePath = Common.ServieTicketAttachmentPath + SavedFileName;

                sizeCount = sizeCount + (fpAttachment.PostedFile.ContentLength / 1024);
                ViewState["CountSize"] = Convert.ToInt32(ViewState["CountSize"]) + sizeCount;
                if (!String.IsNullOrEmpty(fpAttachment.Value))
                {
                    fpAttachment.PostedFile.SaveAs(filePath);
                }

                GenerateTableGrid(fpAttachment.Value, SavedFileName);
            }

            lblMessage.Visible = true;
            if (Convert.ToInt64(ViewState["CountSize"]) > 18000)
            {
                lblMessage.Text = "Attachment(s) must be less then 18 MB";
            }
            else
            {
                lblMessage.Text = "Attachment uploaded successfully.";
            }
            modalAttachments.Show();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void grvAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteAttachment")
        {
            GridViewRow row;
            row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            List<UploadFile> objFileList = new List<UploadFile>();
            if (ViewState["ListValues"] != null)
            {
                objFileList = (List<UploadFile>)ViewState["ListValues"];
                if (File.Exists(Common.ServieTicketAttachmentPath + objFileList[row.RowIndex].SavedFileName))
                    File.Delete(Common.ServieTicketAttachmentPath + objFileList[row.RowIndex].SavedFileName);
                objFileList.RemoveAt(row.RowIndex);
                ViewState["ListValues"] = objFileList;
            }
            grvAttachment.DataSource = objFileList;
            grvAttachment.DataBind();

            lblMessage.Visible = true;
            lblMessage.Text = "Attachment deleted successfully.";
            modalAttachments.Show();
        }
    }

    /// <summary>
    /// Event for filling contact name on selection of the company
    /// </summary>    
    protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCompanyName.SelectedIndex <= 0)
            {
                ddlContactName.Items.Clear();
                ddlContactName.Items.Add(new ListItem("-Select Contact Name-", "0"));
                ddlContactName.Enabled = false;
                ddlSupplier.Enabled = true;
            }
            else
            {
                ddlContactName.DataSource = objSerTicRep.GetServiceTicketContactByCompanyID(Convert.ToInt64(ddlCompanyName.SelectedValue), IncentexGlobal.CurrentMember.UserInfoID).Select(le => new { ContactName = le.FirstName + " " + le.LastName, UserInfoID = le.UserInfoID }).OrderBy(le => le.ContactName).ToList();
                ddlContactName.DataTextField = "ContactName";
                ddlContactName.DataValueField = "UserInfoID";
                ddlContactName.DataBind();
                ddlContactName.Items.Insert(0, new ListItem("-Select Contact Name-", "0"));
                ddlContactName.Enabled = true;
                ddlSupplier.Enabled = false;
            }
            ddlSupplier.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSupplier.SelectedIndex <= 0)
            {
                ddlCompanyName.Enabled = true;
            }
            else
            {
                ddlCompanyName.Enabled = false;
            }
            ddlCompanyName.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// For filling the support ticket owner dropdown
    /// </summary>
    private void FillServiceTicketOwner()
    {
        try
        {
            UserInformationRepository sServiceTicketOwner = new UserInformationRepository();
            var lstServiceTicketOwner = sServiceTicketOwner.GetIncentexEmployees().Select(le => new { UserInfoID = le.UserInfoID, EmployeeName = le.FirstName + " " + le.LastName }).OrderBy(le => le.EmployeeName).ToList();

            ddlServiceTicketOwner.DataSource = lstServiceTicketOwner;
            ddlServiceTicketOwner.DataValueField = "UserInfoID";
            ddlServiceTicketOwner.DataTextField = "EmployeeName";
            ddlServiceTicketOwner.DataBind();
            ddlServiceTicketOwner.Items.Insert(0, new ListItem("-Select Support Ticket Owner-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// For filling the company name dropdown
    /// </summary>
    private void FillCompanyName()
    {
        try
        {
            CompanyRepository objCompRep = new CompanyRepository();
            ddlCompanyName.DataSource = objCompRep.GetAllQuery().OrderBy(le => le.CompanyName).Where(le => le.CompanyName != "Incentex of Vero Beach, LLC").ToList();
            ddlCompanyName.DataValueField = "CompanyID";
            ddlCompanyName.DataTextField = "CompanyName";
            ddlCompanyName.DataBind();
            ddlCompanyName.Items.Insert(0, new ListItem("-Select Company Name-", "0"));
            ddlCompanyName.Items.Insert(1, new ListItem("Incentex of Vero Beach, LLC", "8"));

            ddlContactName.Enabled = false;
            ddlContactName.Items.Clear();
            ddlContactName.Items.Add(new ListItem("-Select Contact Name-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// For filling the supplier drop down
    /// </summary>    
    private void FillSupplier()
    {
        try
        {
            SupplierRepository objSuppRep = new SupplierRepository();
            ddlSupplier.DataSource = objSuppRep.GetAllQuery().OrderBy(le => le.CompanyName).ToList();
            ddlSupplier.DataValueField = "SupplierID";
            ddlSupplier.DataTextField = "CompanyName";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("-Select Supplier Name-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Clear Controls
    /// </summary>
    private void ClearControls()
    {
        if (IncentexGlobal.CurrentMember != null && (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin)))
        {
            ddlServiceTicketOwner.SelectedIndex = 0;
            ddlCompanyName.SelectedIndex = 0;
            ddlContactName.SelectedIndex = 0;

            ddlContactName.Enabled = false;
            ddlContactName.Items.Clear();
            ddlContactName.Items.Add(new ListItem("-Select Contact Name-", "0"));
        }

        txtDatePromised.Text = String.Empty;
        txtServiceTicketDetails.InnerText = String.Empty;
        txtServiceTicketName.Text = String.Empty;
        ViewState["ListValues"] = null;
        grvAttachment.DataSource = null;
        grvAttachment.DataBind();
    }

    private void GenerateTableGrid(String FileName, String SavedFileName)
    {
        List<UploadFile> objFileList = new List<UploadFile>();
        UploadFile objFile = new UploadFile();

        if (ViewState["ListValues"] != null)
        {
            objFileList = (List<UploadFile>)ViewState["ListValues"];
            objFile.OnlyFileName = FileName;
            objFile.SavedFileName = SavedFileName;
            objFileList.Add(objFile);
            ViewState["ListValues"] = objFileList;
        }
        else
        {
            objFile.OnlyFileName = FileName;
            objFile.SavedFileName = SavedFileName;
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
            atSpan.InnerHtml = "&nbsp;&nbsp;&nbsp;Attchment(s) (" + objFileList.Count + ")<img src='../../images/upload-other-icon.png' alt='' />";
            lblMessage.Visible = false;
        }
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "No attachments.";
            atSpan.InnerHtml = "&nbsp;&nbsp;&nbsp;Attchment(s) (0)<img src='../../images/upload-other-icon.png' alt='' />";
        }
    }

    #endregion
}