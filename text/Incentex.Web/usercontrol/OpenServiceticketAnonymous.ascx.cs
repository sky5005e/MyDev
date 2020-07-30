using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.UI;
using AjaxControlToolkit;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class usercontrol_OpenServiceticketAnonymous : System.Web.UI.UserControl
{
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            mpOpenServiceTicketAnn.Show();
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

    protected void btnSubmitTicketAnn_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToBoolean(Session["ostAnnControl"]))
            {
                Session.Remove("ostAnnControl");

                Int64 ServiceTicketID = 0;

                Int64? OwnerID;

                Int64? UserInfoID = null;
                Int64? ContactID = null;
                Int64? CompanyID = null;
                Int64 UserType = 0;

                ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
                UserInformationRepository objUserRepo = new UserInformationRepository();

                UserInformation objUser = objUserRepo.GetByEmail(txtLoginEmail.Text.Trim());

                if (objUser != null)
                {
                    UserInfoID = objUser.UserInfoID;
                    CompanyID = (objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) && objUser.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin)) ? objUser.CompanyId : 8;
                    ContactID = objUser.UserInfoID;
                    UserType = objUser.Usertype;
                }

                String NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketIEs);

                ServiceTicketID = objSerTicRep.InsertTicket(UserInfoID, CompanyID, UserType, Convert.ToString(txtSubject.Text.Trim()), Convert.ToString(txtQuestion.Text.Trim()), null, Convert.ToString(txtLoginEmail.Text.Trim()), out OwnerID);

                #region For Note Insertion

                objSerTicRep.InsertTicketNote(ServiceTicketID, UserInfoID, "Opened Support Ticket.", NoteFor, "IEActivity");

                if (!String.IsNullOrEmpty(txtQuestion.Text.Trim()))
                {
                    NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs);
                    objSerTicRep.InsertTicketNote(ServiceTicketID, UserInfoID, Convert.ToString(txtQuestion.Text.Trim()), NoteFor, null);
                }

                #endregion

                #region For Sending Emails

                StreamReader _StreamReader;
                _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ServiceTicketNote.htm"));
                String eMailTemplate = String.Empty;
                eMailTemplate = _StreamReader.ReadToEnd();
                _StreamReader.Close();
                _StreamReader.Dispose();

                CommonMails objCommonMail = new CommonMails();
                StringBuilder MessageBody = new StringBuilder(eMailTemplate);

                UserInformation objOwner = objUserRepo.GetById(OwnerID);

                #region Send Receipt

                //Email Management                
                if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                {
                    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    MessageBody.Replace("{FullName}", objUser != null ? objUser.FirstName + " " + objUser.LastName : "User");
                    MessageBody.Replace("{TicketNo}", Convert.ToString(ServiceTicketID));
                    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                    MessageBody.Replace("{Sender}", "World-Link System");
                    MessageBody.Replace("{Note}", "<br/>" + Convert.ToString(txtQuestion.Text.Trim()));

                    MessageBody.Replace("{CloseTicket}", String.Empty);

                    objCommonMail.SendServiceTicketReciept(ServiceTicketID, Convert.ToString(UserInfoID), "Support Ticket Center", 1, Convert.ToString(txtLoginEmail.Text.Trim()), MessageBody.ToString(), Convert.ToString(Application["SMTPHOST"]), Convert.ToString(Application["SMTPPORT"]), Convert.ToString(txtSubject.Text.Trim()));
                }

                #endregion

                #region Send Alert To Owner

                //Email Management                
                if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(objOwner.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                {
                    MessageBody = new StringBuilder(eMailTemplate);
                    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    MessageBody.Replace("{FullName}", objOwner.FirstName + " " + objOwner.LastName);
                    MessageBody.Replace("{TicketNo}", Convert.ToString(ServiceTicketID));
                    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                    MessageBody.Replace("{Sender}", objUser != null ? objUser.FirstName + " " + objUser.LastName : Convert.ToString(txtLoginEmail.Text.Trim()));
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
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        txtLoginEmail.Text = "";
        txtQuestion.Text = "";
        mpOpenServiceTicketAnn.Hide();
    }
}
