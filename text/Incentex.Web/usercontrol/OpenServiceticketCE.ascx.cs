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
using System.IO;
using System.Text;
using System.Configuration;
using System.Data;

public partial class usercontrol_OpenServiceticketCE : System.Web.UI.UserControl
{
    ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
    ServiceTicket objServiceTicket = new ServiceTicket();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (IncentexGlobal.CurrentMember != null)
            {
                txtLoginEmail.Text = IncentexGlobal.CurrentMember.LoginEmail;
                txtLoginEmail.ReadOnly = true;
                FillServiceTicketReason();
                mpOpenServiceTicketCE.Show();
                txtSubject.Focus();
            }
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

    protected void btnSubmitTicketCE_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToBoolean(Session["ostCEControl"]))
            {
                Session.Remove("ostCEControl");
                if (IncentexGlobal.CurrentMember != null)
                {
                    Int64 ServiceTicketID = 0;

                    Int64? OwnerID;

                    String NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);

                    ServiceTicketID = objSerTicRep.InsertTicket(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.CompanyId, IncentexGlobal.CurrentMember.Usertype, Convert.ToString(txtSubject.Text.Trim()), Convert.ToString(txtQuestion.Text.Trim()), Convert.ToInt64(ddlServiceTicketReason.SelectedValue), null, out OwnerID);

                    #region For Note Insertion

                    objSerTicRep.InsertTicketNote(ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, "Opened Support Ticket.", NoteFor, "IEActivity");

                    if (!String.IsNullOrEmpty(txtQuestion.Text.Trim()))
                    {
                        NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs);
                        objSerTicRep.InsertTicketNote(ServiceTicketID, IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(txtQuestion.Text.Trim()), NoteFor, null);
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

                    UserInformationRepository objUserRepo = new UserInformationRepository();
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

                        MessageBody.Replace("{CloseTicket}", String.Empty);

                        objCommonMail.SendServiceTicketReciept(ServiceTicketID, Convert.ToString(IncentexGlobal.CurrentMember.UserInfoID), "Support Ticket Center", 1, IncentexGlobal.CurrentMember.Email, MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(txtSubject.Text.Trim()));
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

                        String CloseTicket = @"<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'><tr><td>
                                    <a href='" + ConfigurationSettings.AppSettings["siteurl"]
                                        + "admin/ServiceTicketCenter/ChangeStatus.aspx?ID="
                                        + ServiceTicketID + "&uid=" + objOwner.UserInfoID
                                        + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/closeticket.png' alt='close ticket' border='0'/></a><td></tr></table>";

                        MessageBody.Replace("{CloseTicket}", CloseTicket);

                        objCommonMail.SendServiceTicketReciept(ServiceTicketID, Convert.ToString(objOwner.UserInfoID), "Support Ticket Center", 1, objOwner.Email, MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(txtSubject.Text.Trim()));
                    }

                    #endregion

                    #endregion
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        txtQuestion.Text = "";
        mpOpenServiceTicketCE.Hide();
    }

    /// <summary>
    /// For filling the support ticket owner dropdown
    /// </summary>
    private void FillServiceTicketReason()
    {
        try
        {
            LookupDA sServiceSupportStatus = new LookupDA();
            LookupBE sServiceSupportStatusBE = new LookupBE();
            sServiceSupportStatusBE.SOperation = "selectall";
            sServiceSupportStatusBE.iLookupCode = "SupportTicketReason";
            DataSet ds = new DataSet();
            ds = sServiceSupportStatus.LookUp(sServiceSupportStatusBE);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            dt.DefaultView.Sort = "sLookupName";
            ddlServiceTicketReason.DataSource = dt.DefaultView.ToTable();
            ddlServiceTicketReason.DataValueField = "iLookupID";
            ddlServiceTicketReason.DataTextField = "sLookupName";
            ddlServiceTicketReason.DataBind();
            ddlServiceTicketReason.Items.Insert(0, new ListItem("-Select Support Ticket Reason-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}
