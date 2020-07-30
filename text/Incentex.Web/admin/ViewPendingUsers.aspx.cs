/// <summary>
/// Display pending user by companywise changes done by mayur on 18-jan-2012
/// Approve/reject multiple employee is also add by mayur on 4-feb-2012
/// Code Optimization and clearup is done by mayur
/// </summary>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
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
    DataSet dsEmailTemplate;
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

    Int32 TotalRecords = 0;

    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "View Pending Users";
            base.ParentMenuID = 10;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/UserManagement.aspx";
            ((Label)Master.FindControl("lblPageHeading")).Text = "View Pending Users";
            bindgrid();
        }
    }

    protected void grdView_RowCommand(object sender, GridViewCommandEventArgs e)
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
            Response.Redirect("~/admin/Company/Employee/RegistrationInfo.aspx?ID=" + Convert.ToString(e.CommandArgument));
        }
    }

    protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (this.ViewState["SortExp"] != null)
            {
                switch (this.ViewState["SortExp"].ToString())
                {
                    case "sFirstName":
                        PlaceHolder placeholderFullName = (PlaceHolder)e.Row.FindControl("placeholderFullName");
                        break;
                    case "Contact":
                        PlaceHolder placeholderContract = (PlaceHolder)e.Row.FindControl("placeholderContract");
                        break;
                    case "sMobileNumber":
                        PlaceHolder placeholdersMobileNumber = (PlaceHolder)e.Row.FindControl("placeholdersMobileNumber");
                        break;
                }
            }
        }
    }

    protected void dtlStore_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HiddenField hdnStoreId = (HiddenField)e.Item.FindControl("hdnStoreId");
            List<GetStoreWorkGroupsResult> lstWorkGroups = new List<GetStoreWorkGroupsResult>();
            lstWorkGroups = new CompanyStoreRepository().GetStoreWorkGroups(Convert.ToInt64(hdnStoreId.Value)).OrderBy(le => le.WorkGroup).ToList();
            if (lstWorkGroups.Count > 0)
            {
                ((Repeater)e.Item.FindControl("dtlWorkGroup")).DataSource = lstWorkGroups;
                ((Repeater)e.Item.FindControl("dtlWorkGroup")).DataBind();
            }
            else
            {
                ((Label)e.Item.FindControl("lblMsg")).Text = "No work groups.";
            }
        }
    }

    protected void dtlWorkGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HiddenField hdnCompanyId = (HiddenField)e.Item.Parent.Parent.FindControl("hdnCompanyId");
            HiddenField hdnWorkGroupID = (HiddenField)e.Item.FindControl("hdnWorkGroupID");
            Int64? iWorkGroupID = Convert.ToInt64(hdnWorkGroupID.Value);
            objRegList = objRegistrationRepo.GetAllPendingUsers(this.SortExp, this.SortOrder, Convert.ToInt64(hdnCompanyId.Value)).Where(le => le.iWorkGroupID == iWorkGroupID).ToList();

            HtmlControl dvWorkGroup = (HtmlControl)e.Item.FindControl("dvWorkGroup");
            dvWorkGroup.Attributes.Add("total", Convert.ToString(objRegList.Count));

            if (objRegList.Count > 0)
            {
                ((GridView)e.Item.FindControl("grdView")).DataSource = objRegList;
                ((GridView)e.Item.FindControl("grdView")).DataBind();
            }
            else
            {
                ((HtmlGenericControl)e.Item.FindControl("dvButtonControler")).Visible = false;
                ((Label)e.Item.FindControl("lblMsg")).Text = "No pending users.";
            }
        }
    }

    protected void dtlWorkGroup_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        lblMsgGlobal.Text = String.Empty;

        if (e.CommandName == "Approve")
        {
            try
            {
                HiddenField hdnCompanyId = (HiddenField)e.Item.Parent.Parent.FindControl("hdnCompanyId");
                GridView grd = (GridView)e.Item.FindControl("grdView");

                UserInformationRepository objUserRepo = new UserInformationRepository();

                Int32 ApprovedUsersCount = 0;
                Int32 ExistingUsersCount = 0;

                foreach (GridViewRow grv in grd.Rows)
                {
                    if (((CheckBox)grv.FindControl("chkSelectUser")).Checked == true)
                    {
                        Label lbtnAction = (Label)(grv.FindControl("lblEmail"));
                        if (objUserRepo.CheckEmailExistence(lbtnAction.Text.Trim(), 0))
                        {
                            Label lblRegistrationId = (Label)grv.FindControl("lblRegistrationId");
                            String[] generatedpwd = objRegistrationRepo.InsertIntoUserInformation(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(lblRegistrationId.Text));

                            //Now assign global menu logic
                            Int64 uid = Convert.ToInt64(generatedpwd[1]);
                            objRegistrationRepo.InsertIntoCompanyEmployeeMenuAccess(uid);
                            //End                        

                            sendApprovalEmail(lbtnAction.Text, generatedpwd[0]);

                            //SendToAllIE
                            //sendIEEmail(lbtnAction.Text, generatedpwd[0]);//Commented this code by mayur on 13-march-2012 Due to client update
                            //End

                            //SendToCA
                            //get WorkgroupId by companyid
                            //Int64? wid = new RegistrationRepository().getworkgroupbyregistrationid(Convert.ToInt64(lblRegistrationId.Text));
                            //SendEmailToAllCA(lbtnAction.Text, generatedpwd[0], (Int64)wid, Convert.ToInt64(hdnCompanyId.Value));//Commented this code by mayur on 13-march-2012 Due to client update
                            //End
                            ApprovedUsersCount++;

                            //new SAPOperations().SubmitUserToSAP(uid, lbtnAction.Text.Trim());
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
        else if (e.CommandName == "ApproveAndApplyPassword")
        {
            try
            {
                HiddenField hdnCompanyId = (HiddenField)e.Item.Parent.Parent.FindControl("hdnCompanyId");
                GridView grd = (GridView)e.Item.FindControl("grdView");

                UserInformationRepository objUserRepo = new UserInformationRepository();

                Int32 ApprovedUsersCount = 0;
                Int32 ExistingUsersCount = 0;

                foreach (GridViewRow grv in grd.Rows)
                {
                    if (((CheckBox)grv.FindControl("chkSelectUser")).Checked == true)
                    {
                        Label lbtnAction = (Label)(grv.FindControl("lblEmail"));

                        if (objUserRepo.CheckEmailExistence(lbtnAction.Text.Trim(), 0))
                        {
                            Label lblRegistrationId = (Label)grv.FindControl("lblRegistrationId");
                            String[] generatedpwd = objRegistrationRepo.InsertIntoUserInformation(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(lblRegistrationId.Text));

                            Int64 uid = Convert.ToInt64(generatedpwd[1]);

                            //change password to 'abc123' to selected user
                            new UserInformationRepository().UpdatePassword(uid, "abc123");

                            //Now assign global menu logic
                            objRegistrationRepo.InsertIntoCompanyEmployeeMenuAccess(uid);
                            //End                        

                            sendApprovalEmail(lbtnAction.Text, "abc123");

                            //SendToAllIE
                            //sendIEEmail(lbtnAction.Text, "abc123");//Commented this code by mayur on 13-march-2012 Due to client update
                            //End

                            //SendToCA
                            //get WorkgroupId by companyid
                            //Int64? wid = new RegistrationRepository().getworkgroupbyregistrationid(Convert.ToInt64(lblRegistrationId.Text));
                            //SendEmailToAllCA(lbtnAction.Text, "abc123", (Int64)wid, Convert.ToInt64(hdnCompanyId.Value));//Commented this code by mayur on 13-march-2012 Due to client update
                            //End
                            ApprovedUsersCount++;

                            //new SAPOperations().SubmitUserToSAP(uid, lbtnAction.Text.Trim());
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
        else if (e.CommandName == "Reject")
        {
            try
            {
                HiddenField hdnCompanyId = (HiddenField)e.Item.Parent.Parent.FindControl("hdnCompanyId");
                GridView grd = (GridView)e.Item.FindControl("grdView");

                Int32 RejectedUsersCount = 0;

                foreach (GridViewRow grv in grd.Rows)
                {
                    if (((CheckBox)grv.FindControl("chkSelectUser")).Checked == true)
                    {
                        Label lblRegistrationId = (Label)grv.FindControl("lblRegistrationId");
                        objRegistrationRepo.UpdateValueToReject(Convert.ToInt64(lblRegistrationId.Text));
                        RejectedUsersCount++;
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
        else if (e.CommandName == "Delete")
        {
            try
            {
                if (!base.CanDelete)
                {
                    base.RedirectToUnauthorised();
                }

                HiddenField hdnCompanyId = (HiddenField)e.Item.Parent.Parent.FindControl("hdnCompanyId");
                GridView grd = (GridView)e.Item.FindControl("grdView");
                RegistrationRepository objRepo = new RegistrationRepository();

                Int32 DeletedUsersCount = 0;

                foreach (GridViewRow grv in grd.Rows)
                {
                    if (((CheckBox)grv.FindControl("chkSelectUser")).Checked == true)
                    {
                        Label lblRegistrationId = (Label)grv.FindControl("lblRegistrationId");
                        Inc_Registration objRegistration = objRepo.GetByRegistrationID(Convert.ToInt64(lblRegistrationId.Text));
                        objRepo.Delete(objRegistration);
                        DeletedUsersCount++;
                    }
                }

                objRepo.SubmitChanges();

                bindgrid();

                lblMsgGlobal.Text = DeletedUsersCount + " Company employee(s) deleted successfully..";
            }
            catch (Exception ex)
            {
                lblMsgGlobal.Text = objcomm.Callvalidationmessage(Server.MapPath("~/JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
                ErrHandler.WriteError(ex);
            }
        }
    }

    #endregion

    #region Methods

    public void bindgrid()
    {
        List<CompanyStoreRepository.IECompanyListResults> objStoreList = new CompanyStoreRepository().GetCompanyStore();
        dtlStore.DataSource = objStoreList;
        dtlStore.DataBind();
        lblRecords.Text = TotalRecords.ToString();
    }

    /// <summary>
    /// Displays the total pending user of this company.
    /// Add by mayur on 18-jan-2012
    /// </summary>
    /// <param name="companyID">The company ID.</param>
    /// <returns></returns>
    public String DisplayTotal(String companyID)
    {
        Int32 StoreTotal = objRegistrationRepo.GetAllPendingUsers(this.SortExp, this.SortOrder, Convert.ToInt64(companyID)).Count();
        TotalRecords += StoreTotal;
        return StoreTotal.ToString();
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
                {
                    messagebody.Replace("{storename}", objComplist[0].CompanyName);
                }

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ApprovedUsers)) == true)
                {
                    new CommonMails().SendMail(objUserInformation.UserInfoID, "Employee Approved", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
                }
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
        {
            for (Int32 i = 0; i < objAdminList.Count; i++)
            {
                sendApprovalEmailIE(EmailAddressFor, objAdminList[i].Email, password, objAdminList[i].FirstName + " " + objAdminList[i].LastName, objAdminList[i].UserInfoID);
            }
        }
        //End
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
                {
                    messagebody.Replace("{storename}", objComplist[0].CompanyName);
                }

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ApprovedUsers)) == true)
                {
                    new CommonMails().SendMail(sToUserInfoID, "Employee Approved", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
                }
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
                {
                    messagebody.Replace("{storename}", objComplist[0].CompanyName);
                }

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(sToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ApprovedUsers)) == true)
                {
                    new CommonMails().SendMail(sToUserInfoID, "Employee Approved", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
                }
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}