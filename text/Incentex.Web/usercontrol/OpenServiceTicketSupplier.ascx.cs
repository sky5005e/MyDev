using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Configuration;

public partial class usercontrol_OpenServiceTicketSupplier : System.Web.UI.UserControl
{
    #region UserControl Properties

    ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
    ServiceTicketAttachmentRepository objSerTicAttRep = new ServiceTicketAttachmentRepository();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization

    #endregion

    #region UserControl Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IncentexGlobal.CurrentMember != null)
        {
            if (!IsPostBack)
            {
                FillServiceTicketOwner();
            }
            mpOpenServiceTicketSP.Show();
        }
    }

    protected override void OnInit(EventArgs e)
    {
        Page.Init += delegate(object sender, EventArgs e_Init)
        {
            if (ToolkitScriptManager.GetCurrent(Page) == null && ScriptManager.GetCurrent(Page) == null)
            {
                ToolkitScriptManager sMgr = new ToolkitScriptManager();
                phScriptManager.Controls.AddAt(0, sMgr);
            }
        };
        base.OnInit(e);
    }

    #endregion

    #region Control Events

    protected void btnSubmitTicketSP_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToBoolean(Session["ostSPControl"]))
            {
                Session.Remove("ostSPControl");

                Int64 ServiceTicketID = 0;

                Int64? OwnerID = null;
                Int64? SupplierID = new SupplierRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).SupplierID;

                UserInformationRepository objUserRepo = new UserInformationRepository();

                if (!String.IsNullOrEmpty(ddlServiceTicketOwner.SelectedValue) && Convert.ToInt64(ddlServiceTicketOwner.SelectedValue) > 0)
                {
                    OwnerID = Convert.ToInt64(ddlServiceTicketOwner.SelectedValue);
                }

                String NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);

                ServiceTicketID = objSerTicRep.InsertTicket(IncentexGlobal.CurrentMember.UserInfoID, null, null, SupplierID, OwnerID, Convert.ToString(txtDatePromised.Text.Trim()), Convert.ToString(txtServiceTicketName.Text.Trim()), Convert.ToString(txtQuestion.Text.Trim()));

                #region For Note Insertion

                objSerTicRep.InsertTicketNote(ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, "Opened Support Ticket.", NoteFor, "IEActivity");

                if (!String.IsNullOrEmpty(txtQuestion.Text.Trim()))
                {
                    NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketSupps);
                    objSerTicRep.InsertTicketNote(ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtQuestion.Text.Trim()), NoteFor, null);
                }

                #endregion

                #region Saving Attachments

                if (Request.Files.Count > 0)
                {
                    HttpFileCollection Attachments = Request.Files;
                    for (int i = 0; i < Attachments.Count; i++)
                    {
                        HttpPostedFile Attachment = Attachments[i];
                        if (Attachment.ContentLength > 0 && !String.IsNullOrEmpty(Attachment.FileName))
                        {
                            String SavedFileName = "serviceticket_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Attachment.FileName;
                            SavedFileName = Common.MakeValidFileName(SavedFileName);
                            String filePath = Common.ServieTicketAttachmentPath + SavedFileName;
                            Attachment.SaveAs(filePath);

                            ServiceTicketAttachment objServiceTicketAttachemt = new ServiceTicketAttachment();
                            objServiceTicketAttachemt.AttachmentFileName = Attachment.FileName;
                            objServiceTicketAttachemt.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                            objServiceTicketAttachemt.CreatedDate = DateTime.Now;
                            objServiceTicketAttachemt.SavedFileName = SavedFileName;
                            objServiceTicketAttachemt.ServiceTicketID = ServiceTicketID;
                            objSerTicAttRep.Insert(objServiceTicketAttachemt);
                        }
                    }

                    objSerTicAttRep.SubmitChanges();
                }

                #endregion

                #region For Sending Email

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
                    MessageBody.Replace("{Note}", "<br/>" + Convert.ToString(txtQuestion.Text.Trim()));

                    CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                        + ServiceTicketID + "&uid=" + IncentexGlobal.CurrentMember.UserInfoID
                                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                    MessageBody.Replace("{CloseTicket}", CloseTicket);

                    objCommonMail.SendServiceTicketReciept(ServiceTicketID, Convert.ToString(IncentexGlobal.CurrentMember.UserInfoID), "Support Ticket Center", 1, IncentexGlobal.CurrentMember.Email, MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(txtServiceTicketName.Text.Trim()));
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
                    MessageBody.Replace("{Note}", "<br/>" + Convert.ToString(txtQuestion.Text.Trim()));

                    CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                        + ServiceTicketID + "&uid=" + objOwner.UserInfoID
                                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                    MessageBody.Replace("{CloseTicket}", CloseTicket);

                    objCommonMail.SendServiceTicketReciept(ServiceTicketID, Convert.ToString(objOwner.UserInfoID), "Support Ticket Center", 1, objOwner.Email, MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(txtServiceTicketName.Text.Trim()));
                }

                #endregion

                #endregion
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        ClearControls();
    }

    #endregion

    #region UserControl Methods

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
    /// Clear Controls
    /// </summary>
    private void ClearControls()
    {
        ddlServiceTicketOwner.SelectedIndex = 0;

        txtDatePromised.Text = String.Empty;
        txtServiceTicketName.Text = String.Empty;
        txtQuestion.Text = "";
        mpOpenServiceTicketSP.Hide();
    }

    #endregion
}