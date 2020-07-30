/// <summary>
/// Display pending user by workgroupwise changes done by mayur on 18-jan-2012
/// Totalworkgroup = main workgroup + additional workgroup
/// Approve/reject multiple employee is also add by mayur on 6=feb-2012
/// </summary>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using SAP_API;
using SAP_WebService = SAP_API.ipostep_vP0010000106in_WCSX_comsapb1ivplatformruntime_INB_WS_CALL_SYNC_XPT_INB_WS_CALL_SYNC_XPTipo_proc_Service;

public partial class MyAccount_ViewPendingUsers : PageBase
{
    #region Data Members

    List<Incentex.DAL.SqlRepository.RegistrationRepository.RegistrationSearchResults> objRegList = new List<Incentex.DAL.SqlRepository.RegistrationRepository.RegistrationSearchResults>();
    RegistrationRepository objRegistrationRepo = new RegistrationRepository();
    Common objcomm = new Common();
    EmailTemplateBE objEmailBE = new EmailTemplateBE();
    EmailTemplateDA objEmailDA = new EmailTemplateDA();
    RegistrationBE objRegistrationBE = new RegistrationBE();
    RegistrationDA objRegistrationDA = new RegistrationDA();
    DataSet dsEmaiUser, dsEmailTemplate;
    Common objcommm = new Common();
    CompanyRepository objCompanyRepos = new CompanyRepository();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    List<Company> objComplist = new List<Company>();

    RegistrationRepository.UsersSortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = SupplierRepository.SupplierSortExpType.FirstName;
            }
            return (RegistrationRepository.UsersSortExpType)ViewState["SortExp"];
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }

    Incentex.DAL.Common.DAEnums.SortOrderType SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
            }
            return (Incentex.DAL.Common.DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }

    #endregion

    #region Event Handlers

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = Convert.ToString(Request.UrlReferrer);
            ((Label)Master.FindControl("lblPageHeading")).Text = "View Pending Users";
            bindgrid();
        }
    }

    protected void gvPendingUserList_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMsgGlobal.Text = String.Empty;
        if (e.CommandName == "Sort")
        {
            if (this.SortExp.ToString() == e.CommandArgument.ToString())
            {
                if (this.SortOrder == Incentex.DAL.Common.DAEnums.SortOrderType.Asc)
                    this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Desc;
                else
                    this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
            }
            else
            {
                this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                this.SortExp = (RegistrationRepository.UsersSortExpType)Enum.Parse(typeof(RegistrationRepository.UsersSortExpType), e.CommandArgument.ToString());
            }
            bindgrid();
        }
        else if (e.CommandName == "Detail")
        {
            Response.Redirect("~/MyAccount/RegistrationDetails.aspx?ID=" + Convert.ToString(e.CommandArgument));
        }
    }

    protected void lnkbtnApproveUser_Click(Object sender, EventArgs e)
    {
        try
        {
            UserInformationRepository objUserRepo = new UserInformationRepository();

            Int32 ApprovedUsersCount = 0;
            Int32 ExistingUsersCount = 0;

            foreach (GridViewRow grv in gvPendingUserList.Rows)
            {
                if (((CheckBox)grv.FindControl("chkSelectUser")).Checked)
                {
                    HiddenField hdnEmail = (HiddenField)(grv.FindControl("hdnEmail"));
                    if (objUserRepo.CheckEmailExistence(hdnEmail.Value.Trim(), 0))
                    {
                        Label lblRegistrationId = (Label)grv.FindControl("lblRegistrationId");
                        String[] generatedpwd = objRegistrationRepo.InsertIntoUserInformation(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(lblRegistrationId.Text));

                        //Now assign global menu logic
                        Int64 uid = Convert.ToInt64(generatedpwd[1]);
                        objRegistrationRepo.InsertIntoCompanyEmployeeMenuAccess(uid);
                        //End

                        //Comment out below code for email
                        sendApprovalEmail(hdnEmail.Value, generatedpwd[0]);
                        //SendToAllIE
                        //sendIEEmail(hdnEmail.Value, generatedpwd[0]);//Commented this code by mayur on 13-march-2012 Due to client update
                        //End

                        ApprovedUsersCount++;

                        //new SAPOperations().SubmitUserToSAP(uid, hdnEmail.Value.Trim());
                    }
                    else
                        ExistingUsersCount++;
                }
            }

            bindgrid();

            if (ApprovedUsersCount > 0 && ExistingUsersCount == 0)
                lblMsgGlobal.Text = ApprovedUsersCount + " Company employee(s) approved successfully...";
            else if (ApprovedUsersCount == 0 && ExistingUsersCount > 0)
                lblMsgGlobal.Text = ExistingUsersCount + " user(s) already exists with same email address...";
            else if (ApprovedUsersCount > 0 && ExistingUsersCount > 0)
                lblMsgGlobal.Text = ApprovedUsersCount + " Company employee(s) approved successfully...<br/>" + ExistingUsersCount + " user(s) already exists with same email address...";
        }
        catch (Exception ex)
        {
            lblMsgGlobal.Text = objcomm.Callvalidationmessage(Server.MapPath("~/JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkBtnRejectUser_Click(Object sender, EventArgs e)
    {
        try
        {
            Int32 RejectedUsersCount = 0;

            foreach (GridViewRow grv in gvPendingUserList.Rows)
            {
                if (((CheckBox)grv.FindControl("chkSelectUser")).Checked == true)
                {
                    Label lblRegistrationId = (Label)grv.FindControl("lblRegistrationId");
                    objRegistrationRepo.UpdateValueToReject(Convert.ToInt64(lblRegistrationId.Text));
                }
            }

            bindgrid();

            lblMsgGlobal.Text = RejectedUsersCount + " Company employee(s) rejected successfully..";
        }
        catch (Exception ex)
        {
            lblMsgGlobal.Text = objcomm.Callvalidationmessage(Server.MapPath("~/JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnApproveAndApplyPassword_Click(Object sender, EventArgs e)
    {
        try
        {
            UserInformationRepository objUserRepo = new UserInformationRepository();

            Int32 ApprovedUsersCount = 0;
            Int32 ExistingUsersCount = 0;

            foreach (GridViewRow grv in gvPendingUserList.Rows)
            {
                if (((CheckBox)grv.FindControl("chkSelectUser")).Checked)
                {
                    HiddenField hdnEmail = (HiddenField)(grv.FindControl("hdnEmail"));
                    if (objUserRepo.CheckEmailExistence(hdnEmail.Value.Trim(), 0))
                    {

                        Label lblRegistrationId = (Label)grv.FindControl("lblRegistrationId");
                        String[] generatedpwd = objRegistrationRepo.InsertIntoUserInformation(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(lblRegistrationId.Text));

                        Int64 uid = Convert.ToInt64(generatedpwd[1]);

                        //change password to 'abc123' to selected user
                        new UserInformationRepository().UpdatePassword(uid, "abc123");

                        //Now assign global menu logic
                        objRegistrationRepo.InsertIntoCompanyEmployeeMenuAccess(uid);
                        //End                        

                        //Comment out below code for email
                        sendApprovalEmail(hdnEmail.Value, "abc123");
                        //SendToAllIE
                        //sendIEEmail(hdnEmail.Value, "abc123"); //Commented this code by mayur on 13-march-2012 Due to client update
                        //End

                        ApprovedUsersCount++;

                        //new SAPOperations().SubmitUserToSAP(uid, hdnEmail.Value.Trim());
                    }
                    else
                    {
                        ExistingUsersCount++;
                    }
                }
            }

            bindgrid();

            if (ApprovedUsersCount > 0 && ExistingUsersCount == 0)
                lblMsgGlobal.Text = ApprovedUsersCount + " Company employee(s) approved successfully...";
            else if (ApprovedUsersCount == 0 && ExistingUsersCount > 0)
                lblMsgGlobal.Text = ExistingUsersCount + " user(s) already exists with same email address...";
            else if (ApprovedUsersCount > 0 && ExistingUsersCount > 0)
                lblMsgGlobal.Text = ApprovedUsersCount + " Company employee(s) approved successfully...<br/>" + ExistingUsersCount + " user(s) already exists with same email address...";
        }
        catch (Exception ex)
        {
            lblMsgGlobal.Text = objcomm.Callvalidationmessage(Server.MapPath("~/JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods

    public void bindgrid()
    {
        //updated by Prashant May-June 2013
        CompanyEmployeeRepository objCompanyEmployeeRepos = new CompanyEmployeeRepository();
        var objPendingUserList = objCompanyEmployeeRepos.GetPendingUsersList(IncentexGlobal.CurrentMember.UserInfoID).ToList();
        gvPendingUserList.DataSource = objPendingUserList;
        gvPendingUserList.DataBind();
        dvButtonControler.Visible = true;

        if (objPendingUserList.Count <= 0)
            dvButtonControler.Visible = false;
        //updated by Prashant May-June 2013
    }

    /// <summary>
    /// Displays the total pending user of this workgroup.
    /// Add by mayur on 18-jan-2012
    /// </summary>
    /// <param name="workgroupID">The workgroup ID.</param>
    /// <returns></returns>
    public String DisplayTotal(String workgroupID)
    {
        return objRegistrationRepo.GetAllUsersByWorkGroup((Int64)IncentexGlobal.CurrentMember.CompanyId, Convert.ToInt64(workgroupID), this.SortExp, this.SortOrder).Count().ToString();
    }

    private void sendApprovalEmail(String EmailAddress, String password)
    {
        try
        {
            UserInformation objUserInformation = new UserInformationRepository().GetByLoginEmail(EmailAddress);

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "employee activation";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = EmailAddress;
                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{fullname}", objUserInformation.FirstName + " " + objUserInformation.LastName);
                messagebody.Replace("{password}", password);
                messagebody.Replace("{email}", objUserInformation.LoginEmail);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                objComplist = objCompanyRepos.GetByCompanyId(Convert.ToInt64(objUserInformation.CompanyId));

                if (objComplist.Count > 0)
                    messagebody.Replace("{storename}", objComplist[0].CompanyName);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ApprovedUsers)) == true)
                    new CommonMails().SendMail(objUserInformation.UserInfoID, "Employee Approved", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendIEEmail(String EmailAddressFor, String password)
    {
        List<UserInformation> objAdminList = new List<UserInformation>();
        objAdminList = new UserInformationRepository().GetEmailInformation();

        if (objAdminList.Count > 0)
            for (Int32 i = 0; i < objAdminList.Count; i++)
                sendApprovalEmailIE(EmailAddressFor, objAdminList[i].Email, password, objAdminList[i].FirstName + " " + objAdminList[i].LastName, objAdminList[i].UserInfoID);
    }

    private void sendApprovalEmailIE(String EmailAddressFor, String ToEmailAddress, String password, String FullName, Int64 UserInfoID)
    {
        try
        {
            UserInformation objUserInformation = new UserInformationRepository().GetByLoginEmail(EmailAddressFor);

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "employee activation";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = ToEmailAddress;
                Int64 sToUserInfoID = UserInfoID;
                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{fullname}", FullName);
                messagebody.Replace("{password}", password);
                messagebody.Replace("{email}", objUserInformation.LoginEmail);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                objComplist = objCompanyRepos.GetByCompanyId(Convert.ToInt64(objUserInformation.CompanyId));

                if (objComplist.Count > 0)
                    messagebody.Replace("{storename}", objComplist[0].CompanyName);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ApprovedUsers)) == true)
                    new CommonMails().SendMail(sToUserInfoID, "Employee Approved", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SendEmailToAllCA(String EmailAddressFor, String password, Int64 wgid, Int64 companyid)
    {
        List<UserInformation> objCA = new List<UserInformation>();
        CompanyEmployeeRepository objRep = new CompanyEmployeeRepository();
        objCA = objRep.GetCAByWorkgroupId(wgid, companyid);

        foreach (UserInformation eachCA in objCA)
        {
            if (objManageEmailRepo.CheckEmailAuthentication(eachCA.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ApprovedUsers)) == true)
                sendApprovalEmailCA(EmailAddressFor, eachCA.Email, password, eachCA.FirstName + " " + eachCA.LastName, eachCA.UserInfoID);
        }
    }

    private void sendApprovalEmailCA(String EmailAddressFor, String ToEmailAddress, String password, String FullName, Int64 UserInfoID)
    {
        try
        {
            UserInformation objUserInformation = new UserInformationRepository().GetByLoginEmail(EmailAddressFor);

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "employee activation";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = ToEmailAddress;
                Int64 sToUserInfoID = UserInfoID;
                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{fullname}", FullName);
                messagebody.Replace("{password}", password);
                messagebody.Replace("{email}", objUserInformation.LoginEmail);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                objComplist = objCompanyRepos.GetByCompanyId(Convert.ToInt64(objUserInformation.CompanyId));

                if (objComplist.Count > 0)
                    messagebody.Replace("{storename}", objComplist[0].CompanyName);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                //Email Management
                new CommonMails().SendMail(sToUserInfoID, "Employee Approved", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}