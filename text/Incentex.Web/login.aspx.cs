using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;
using System.Web.Services;
using Incentex.BE;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

public partial class Login : System.Web.UI.Page
{
    #region Page Variables
    CompanyStoreRepository objStoreRepo = new CompanyStoreRepository();

    #endregion
    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        txtPassword.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + btnLogin.ClientID + "');");
        //Check for Coupa User
        if (!String.IsNullOrEmpty(Request.QueryString["Key"]) && Request.QueryString["Key"] == "cp" && Request.QueryString["ID"] != "0")
        {
            Session["CoupaID"] = Convert.ToString(Request.QueryString["ID"]);
            CheckForCoupa();
        }

        if (!IsPostBack)
        {
            txtEmail.Focus();
            if (IncentexGlobal.CurrentMember != null)
                RedirectFunction();

            HttpCookie objEmailCookie = Request.Cookies["Email"];
            HttpCookie objPasswordCookie = Request.Cookies["Password"];
            String Email = "";
            String Password = "";

            if (objEmailCookie != null)
            {
                Email = objEmailCookie.Value.ToString();
                txtEmail.Text = Email.Trim();
                txtPassword.Focus();
            }

            if (objPasswordCookie != null)
                Password = objPasswordCookie.Value.ToString();

            if (objEmailCookie != null && objPasswordCookie != null)
            {
                chkRememberMe.Checked = true;
                txtEmail.Text = Email.Trim();
                txtPassword.Attributes.Add("value", Password);
                txtPassword.Focus();
            }

            txtEmail.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + btnLogin.ClientID + "');");
            txtPassword.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + btnLogin.ClientID + "');");

            if (Request.Url.OriginalString != null && Request.Url.OriginalString.ToLower().Contains("editorderdetail.aspx"))
                hdnUrlReferrer.Value = Request.Url.OriginalString;

            BindDropDown();

            // Set the CompanyList 
            List<String> companyList = objStoreRepo.GetCompanyStore().OrderBy(le => le.Company).Select(s => s.Company).ToList();
            //Convert to Array as We need to return Array
            hdnCompanyList.Value = String.Join(",", companyList.ToArray<string>());
        }

    }
    #endregion
    #region Page Method

    //bind dropdown
    public void BindDropDown()
    {
        try
        {
            // For base satations
            CompanyRepository objRepo = new CompanyRepository();
            List<INC_BasedStation> objbsList = new List<INC_BasedStation>();
            objbsList = objRepo.GetAllBaseStationResult().OrderBy(b => b.sBaseStation).ToList();
            Common.BindDDL(ddlBaseStation, objbsList, "sBaseStation", "iBaseStationId", "- Station -");

            LookupRepository objLookRep = new LookupRepository();
            //For gender
            ddlGender.DataSource = objLookRep.GetByLookup("Gender");
            ddlGender.DataValueField = "iLookupID";
            ddlGender.DataTextField = "sLookupName";
            ddlGender.DataBind();
            ddlGender.Items.Insert(0, new ListItem("- Gender -", "0"));
            // For Workgroup
            ddlWorkgroup.DataSource = objLookRep.GetByLookup("Workgroup").OrderBy(s => s.sLookupName).ToList();
            ddlWorkgroup.DataValueField = "iLookupID";
            ddlWorkgroup.DataTextField = "sLookupName";
            ddlWorkgroup.DataBind();
            ddlWorkgroup.Items.Insert(0, new ListItem("- Workgroup -", "0"));

            //For Employee Title
            ddlEmployeeTitle.DataSource = objLookRep.GetByLookup("EmployeeTitle");
            ddlEmployeeTitle.DataValueField = "iLookupID";
            ddlEmployeeTitle.DataTextField = "sLookupName";
            ddlEmployeeTitle.DataBind();
            ddlEmployeeTitle.Items.Insert(0, new ListItem("- Position -", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public void RedirectFunction()
    {
        LookupRepository obj = new LookupRepository();
        INC_Lookup objLookup = new INC_Lookup();
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.IncentexAdmin))
        {
            objLookup = obj.GetById((Int64)IncentexGlobal.CurrentMember.WLSStatusId);
            if (objLookup.sLookupName == "Active")
                Response.Redirect("NewDesign/Admin/Dashboard.aspx", false);
            else
            {
                Response.Redirect("loginfailed.aspx?success=fail", false);
                Session.Remove("CurrentMember");
                Session.Remove("GSEMgtCurrentMember");
                IncentexGlobal.CurrentMember = null;
                IncentexGlobal.GSEMgtCurrentMember = null;
            }
        }
        else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.ThirdPartySupplierEmployee))
        {
            objLookup = obj.GetById(objStoreRepo.GetCompanyStoreStatusByCompanyId((Int64)IncentexGlobal.CurrentMember.CompanyId));

            if (objLookup.sLookupName == "Open")
            {
                objLookup = obj.GetById((Int64)IncentexGlobal.CurrentMember.WLSStatusId);

                if (objLookup.sLookupName == "Active")
                {
                    // Response.Redirect("index.aspx", false);
                    Response.Redirect("NewDesign/UserPages/Index.aspx", true);
                }
                else
                {
                    Response.Redirect("loginfailed.aspx?success=fail", false);
                    Session.Remove("CurrentMember");
                    Session.Remove("GSEMgtCurrentMember");
                    IncentexGlobal.CurrentMember = null;
                    IncentexGlobal.GSEMgtCurrentMember = null;
                }
            }
            else if (objLookup.sLookupName == "Updating")
            {
                Response.Redirect("loginfailed.aspx?success=storeupdating", false);
                Session.Remove("CurrentMember");
                Session.Remove("GSEMgtCurrentMember");
                IncentexGlobal.CurrentMember = null;
                IncentexGlobal.GSEMgtCurrentMember = null;
            }
            else
            {
                Response.Redirect("loginfailed.aspx?success=storeclosed", false);
                Session.Remove("CurrentMember");
                Session.Remove("GSEMgtCurrentMember");
                IncentexGlobal.CurrentMember = null;
                IncentexGlobal.GSEMgtCurrentMember = null;
            }
        }
        else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.Supplier))
        {
            Response.Redirect("NewDesign/Admin/Dashboard.aspx", true);
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        LookupRepository obj = new LookupRepository();
        INC_Lookup objLookup = new INC_Lookup();
        lblMessage.Text = "";
        HttpCookie hcEmail;
        HttpCookie hcPassword;
        try
        {
            if (txtEmail.Text.Trim() != "" && txtPassword.Text.Trim() != "")
            {
                UserInformation objUser = new UserInformation();
                UserInformationRepository objUserRepository = new UserInformationRepository();
                objUser = objUserRepository.AuthenticateUser(txtEmail.Text.Trim(), txtPassword.Text.Trim());

                if (objUser != null)
                {
                    GSEUserDetails objGSEUserDetail = new GSEUserDetails();
                    AssetVendorRepository objAssetMgt = new AssetVendorRepository();
                    objGSEUserDetail = objAssetMgt.GetGSEUserDetailsByUserInfoID(objUser.UserInfoID);

                    IncentexGlobal.GSEMgtCurrentMember = objGSEUserDetail;
                    IncentexGlobal.CurrentMember = objUser;
                    //
                    if (chkRememberMe.Checked)
                    {
                        hcEmail = new HttpCookie("Email", Convert.ToString(txtEmail.Text.Trim()));
                        hcPassword = new HttpCookie("Password", Convert.ToString(txtPassword.Text.Trim()));
                        hcEmail.Expires = DateTime.Now.AddDays(30);
                        hcPassword.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(hcEmail);
                        Response.Cookies.Add(hcPassword);
                    }
                    else
                    {
                        Response.Cookies["Email"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
                    }

                    if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.EquipmentVendorEmployee))
                    {
                        objLookup = obj.GetById((Int64)IncentexGlobal.CurrentMember.WLSStatusId);
                        if (objLookup.sLookupName == "Active")
                        {
                            // for UserTracking System
                            UdpateUserLoginDetailTC();
                            //End
                            Response.Redirect("NewDesign/Admin/Dashboard.aspx", false);
                            //Response.Redirect("admin/Menu.aspx", false);
                        }
                        else
                        {
                            Session.Remove("CurrentMember");
                            Session.Remove("GSEMgtCurrentMember");
                            IncentexGlobal.CurrentMember = null;
                            IncentexGlobal.GSEMgtCurrentMember = null;
                            Response.Redirect("NewDesign/loginfailed.aspx?success=fail", false);
                        }
                    }
                    else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.ThirdPartySupplierEmployee))
                    {
                        objLookup = obj.GetById(objStoreRepo.GetCompanyStoreStatusByCompanyId((Int64)IncentexGlobal.CurrentMember.CompanyId));
                        if (objLookup.sLookupName == "Open")
                        {
                            objLookup = obj.GetById((Int64)IncentexGlobal.CurrentMember.WLSStatusId);
                            if (objLookup.sLookupName == "Active")
                            {
                                // for UserTracking System
                                UdpateUserLoginDetailTC();
                                //End
                                //start add by mayur 1-dec-2011
                                if (!String.IsNullOrEmpty(hdnUrlReferrer.Value))
                                    Response.Redirect(hdnUrlReferrer.Value, false);
                                else //end add by mayur 1-dec-2011
                                    //Response.Redirect("index.aspx", false);
                                    Response.Redirect("NewDesign/UserPages/Index.aspx", false);
                            }
                            else
                            {
                                Response.Redirect("loginfailed.aspx?success=fail", false);
                                Session.Remove("CurrentMember");
                                Session.Remove("GSEMgtCurrentMember");
                                IncentexGlobal.CurrentMember = null;
                                IncentexGlobal.GSEMgtCurrentMember = null;
                            }
                        }
                        else if (objLookup.sLookupName == "Updating")
                        {
                            Response.Redirect("NewDesign/loginfailed.aspx?success=storeupdating&uid=" + IncentexGlobal.CurrentMember.CompanyId, false);
                            Session.Remove("CurrentMember");
                            Session.Remove("GSEMgtCurrentMember");
                            IncentexGlobal.CurrentMember = null;
                            IncentexGlobal.GSEMgtCurrentMember = null;
                        }
                        else
                        {
                            Response.Redirect("NewDesign/loginfailed.aspx?success=storeclosed&uid=" + IncentexGlobal.CurrentMember.CompanyId, false);
                            Session.Remove("CurrentMember");
                            IncentexGlobal.CurrentMember = null;
                            IncentexGlobal.GSEMgtCurrentMember = null;
                        }
                    }
                    else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.Supplier))
                    {
                        Response.Redirect("NewDesign/Admin/Dashboard.aspx", false);
                    }
                    else
                    {
                        //Update User Activity Table
                        UdpateUserLoginAndActivity();
                        // for UserTracking System
                        UdpateUserLoginDetailTC();
                        //End

                        Response.Redirect("NewDesign/Admin/Dashboard.aspx", false);
                    }
                }
                else
                {
                    hcEmail = new HttpCookie("Email", Convert.ToString(txtEmail.Text.Trim()));
                    hcEmail.Expires = DateTime.Now.AddDays(30);
                    hcPassword = new HttpCookie("Password", "");
                    Response.Cookies.Add(hcEmail);
                    Response.Cookies.Add(hcPassword);
                    Response.Redirect("NewDesign/loginfailed.aspx", false);
                }
            }
            else
            {
                if (txtEmail.Text.Trim() == "")
                {

                    //error message for email field required
                    lblMessage.Text = "Oops, try again!";//objcomm.Callvalidationmessage(Server.MapPath("JS/JQuery/validationMessages/commonValidationMsg.xml"), "Required", "login email");
                }
                else if (txtPassword.Text.Trim() == "")
                {
                    //error message for password field required
                    lblMessage.Text = "Oops, try again!";// objcomm.Callvalidationmessage(Server.MapPath("JS/JQuery/validationMessages/commonValidationMsg.xml"), "Required", "password");
                }
            }
        }
        catch (Exception ex)
        {
            IncentexGlobal.CurrentMember = null;
            IncentexGlobal.GSEMgtCurrentMember = null;
            ErrHandler.WriteError(ex);
        }
    }

    public void UdpateUserLoginAndActivity()
    {
        UserActivityRepository objUAct = new UserActivityRepository();
        UserLoginActivity objUser = new UserLoginActivity();
        objUser = objUAct.GetByUserId(IncentexGlobal.CurrentMember.UserInfoID);
        if (objUser != null)
        {
            //User Has logged in once
            objUser.LastLoginDate = System.DateTime.Now;
            objUser.LoginTime = System.DateTime.Now.TimeOfDay.ToString();
            objUser.LogOutTime = null;
            objUser.LoginCount = objUser.LoginCount + 1;
            objUser.LoginStatus = true;
            String userip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (userip == null)
                userip = Request.ServerVariables["REMOTE_ADDR"];

            objUser.IPAddress = userip;
            objUAct.SubmitChanges();
        }
        else
        {
            UserActivityRepository objUActAdd = new UserActivityRepository();
            UserLoginActivity objUserAdd = new UserLoginActivity();
            objUserAdd.UserInformationId = IncentexGlobal.CurrentMember.UserInfoID;
            objUserAdd.UserType = IncentexGlobal.CurrentMember.Usertype;

            if (IncentexGlobal.CurrentMember.CompanyId != null)
                objUserAdd.CompanyId = (Int64)IncentexGlobal.CurrentMember.CompanyId;

            objUserAdd.LastLoginDate = System.DateTime.Now;
            objUserAdd.LoginTime = System.DateTime.Now.TimeOfDay.ToString();
            objUserAdd.LoginCount = 1;
            objUserAdd.LoginStatus = true;
            String userip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (userip == null)
                userip = Request.ServerVariables["REMOTE_ADDR"];

            objUserAdd.IPAddress = userip;
            objUActAdd.Insert(objUserAdd);
            objUActAdd.SubmitChanges();
            //First Time Activity
        }
    }

    // For the tracking Center user tracking  insert data into UserTracking table[Created by :- Parth Date :- 28/12/2011]
    public void UdpateUserLoginDetailTC()
    {
        UserTrackingRepo objTrackinRepo = new UserTrackingRepo();
        UserTracking objTrackinTbl = new UserTracking();

        objTrackinTbl.UserInfoID = Convert.ToInt32(IncentexGlobal.CurrentMember.UserInfoID);
        objTrackinTbl.LoginTime = Convert.ToDateTime(System.DateTime.Now.TimeOfDay.ToString());
        objTrackinTbl.LogoutTime = null;
        objTrackinTbl.LoginCount = 1;
        objTrackinTbl.Isupdate = false;
        objTrackinTbl.UserStatus = true;
        System.Web.HttpBrowserCapabilities browser = Request.Browser;

        objTrackinTbl.OS = GetOSName(hdnOS.Value);
        objTrackinTbl.Resolution = hdnResolution.Value;

        String s = browser.Browser;
        objTrackinTbl.BrowserName = s;
        String userip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (userip == null)
            userip = Request.ServerVariables["REMOTE_ADDR"];

        objTrackinTbl.IPAddress = userip;
        objTrackinTbl.BrowserVersion = browser.Version;
        objTrackinRepo.Insert(objTrackinTbl);
        objTrackinRepo.SubmitChanges();

        Session["trackID"] = objTrackinTbl.UserTrackID;
    }

    private string GetOSName(string UserAgentDetails)
    {
        UserAgentDetails = UserAgentDetails.Substring(UserAgentDetails.IndexOf('(') + 1, UserAgentDetails.IndexOf(')') - UserAgentDetails.IndexOf('(') - 1);
        switch (UserAgentDetails)
        {
            case "Windows CE": 
            case "Windows 95":
            case "Windows 98": return "Windows CE";
            case "Windows 98; Win 9x 4.90": return "Windows Millennium Edition (Windows Me)";
            case "Windows NT 4.0": return "Microsoft Windows NT 4.0";
            case "Windows NT 5.0": return "Windows 2000";
            case "Windows NT 5.01": return "Windows 2000, Service Pack 1 (SP1)";
            case "Windows NT 5.1": return "Windows XP";
            case "Windows NT 5.2": return "Windows Server 2003; Windows XP x64 Edition";
            case "Windows NT 6.0": return "Windows Vista";
            case "Windows NT 6.1": return "Windows 7";
            case "Windows NT 6.2": return "Windows 8";
            case "Windows NT 6.3": return "Windows 8.1";
            default:
                {
                    if (UserAgentDetails.Split(';').Length > 1) return UserAgentDetails.Split(';')[0];
                    else return "unknown";
                }
        }
    }

    protected void lnkSubmitRegistration_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 CompanyID = new CompanyStoreRepository().GetCompanyIDByName(Convert.ToString(txtCompany.Text));
            String FullName = txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim();
            DateTime hireDate = Convert.ToDateTime(txtDateOfHire.Text);
            Inc_Registration objRegis = new Inc_Registration();
            UserInformationRepository objUserRepo = new UserInformationRepository();
            objRegis.iCompanyName = Convert.ToInt64(CompanyID);
            objRegis.sFirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFirstName.Text.Trim().ToLower());
            objRegis.sLastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLastName.Text.Trim().ToLower());
            objRegis.sEmailAddress = txtEmailAddress.Text.Trim();
            objRegis.iWorkgroupId = Int64.Parse(ddlWorkgroup.SelectedValue);
            objRegis.iBasestationId = Int64.Parse(ddlBaseStation.SelectedValue);
            objRegis.iGender = Int32.Parse(ddlGender.SelectedValue);
            objRegis.status = "pending";
            objRegis.DateRequestSubmitted = System.DateTime.Now;
            objRegis.DOH = hireDate;
            objRegis.sEmployeeId = Convert.ToString(txtEmployeeID.Text);
            objRegis.Password = Convert.ToString(txtPwdRegistration.Text);
            objRegis.EmployeeTitleID = Convert.ToInt64(ddlEmployeeTitle.SelectedValue);

            objUserRepo.Insert(objRegis);
            objUserRepo.SubmitChanges();
            Int64 registraionID = objRegis.iRegistraionID;
            SendMail();
            SendEmailToAllCA(registraionID, CompanyID);

            this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowLoginPopup('register-block','" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(FullName.ToLower()) + "');", true);
            ResetControl();
        }
        catch (SqlException ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    /// <summary>
    /// Reset all Control
    /// </summary>
    private void ResetControl()
    {
        txtCompany.Text = String.Empty;
        txtLastName.Text = String.Empty;
        txtFirstName.Text = String.Empty;
        ddlWorkgroup.SelectedIndex = 0;
        ddlBaseStation.SelectedIndex = 0;
        ddlGender.SelectedIndex = 0;
        txtEmployeeID.Text = String.Empty;
        txtEmailAddress.Text = String.Empty;
        txtPwdRegistration.Text = String.Empty;
        txtDateOfHire.Text = String.Empty;
        ddlEmployeeTitle.SelectedIndex = 0;
    }

    private void SendMail()
    {
        INC_EmailTemplate objEmail = new EmailRepository().GetEmailTemplatesByName("NewSignUp");
        //Get Email Content
        if (objEmail != null)
        {
            String sFrmadd = objEmail.sFromAddress;
            String sToadd = txtEmail.Text.Trim();
            String sSubject = objEmail.sSubject;
            String sFrmname = objEmail.sFromName;
            StringBuilder messagebody = new StringBuilder(objEmail.sTemplateContent); // From Template table

            messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            messagebody.Replace("{fullname}", txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim());
            messagebody.Replace("{storename}", txtCompany.Text);

            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();
            if (HttpContext.Current.Request.IsLocal)
            {
                new CommonMails().SendEmail4Local(1092, "New Sign Up", "surendar.yadav@indianic.com", sSubject, messagebody.ToString(), "incentextest6@gmail.com", "test6incentex", true);
            }
            else
                new CommonMails().SendMail(0, "New Sign Up", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
        }
    }

    private void SendEmailToAllCA(Int64 registraionID, Int64 companyID)
    {
        List<GetUserApproverResult> objCA = new List<GetUserApproverResult>();
        CompanyEmployeeRepository objRep = new CompanyEmployeeRepository();
        objCA = objRep.GetUserApprover(0, companyID, Convert.ToInt64(ddlWorkgroup.SelectedValue), Convert.ToInt64(ddlBaseStation.SelectedValue));
        foreach (GetUserApproverResult eachCA in objCA)
        {
            SendMailToAllCAForTheWorkgroup(eachCA.LoginEmail, eachCA.FirstName + " " + eachCA.LastName, eachCA.UserInfoID, registraionID);
        }
    }

    private void SendMailToAllCAForTheWorkgroup(string email, string FullName, Int64 UserInfoID, Int64 registraionID)
    {
        INC_EmailTemplate objEmail = new EmailRepository().GetEmailTemplatesByName("NewSignUpRequestForCA");
        //Get Email Content
        if (objEmail != null)
        {
            String sFrmadd = objEmail.sFromAddress;
            String sToadd = email;
            String sSubject = objEmail.sSubject;
            String sFrmname = objEmail.sFromName;
            StringBuilder messagebody = new StringBuilder(objEmail.sTemplateContent); // From Template table
            Int64 sToUserInfoID = UserInfoID;

            messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            messagebody.Replace("{fullname}", FullName);
            messagebody.Replace("{First Name/Last Name}", txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim());
            messagebody.Replace("{DateTime}", System.DateTime.Now.ToShortDateString());
            messagebody.Replace("{employeeID}", txtEmployeeID.Text.Trim());
            messagebody.Replace("{baseStation}", ddlBaseStation.SelectedItem.Text);
            messagebody.Replace("{workgroup}", ddlWorkgroup.SelectedItem.Text);
            messagebody.Replace("{position}", ddlEmployeeTitle.SelectedItem.Text);
            messagebody.Replace("{address}", String.Empty);//No input address
            messagebody.Replace("{Email}", txtEmailAddress.Text.Trim());
            //Set Conformation Button
            String buttonText = "";
            String siteURL = ConfigurationSettings.AppSettings["NewDesignSiteurl"];
            buttonText += "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
            buttonText += "<tr>";
            buttonText += "<td>";
            buttonText += "<a href='" + siteURL + "MyAccount/ViewPendingUsers.aspx?UserId=" + sToUserInfoID + "&RegisID=" + registraionID + "&Status=Approve'><img src='" + siteURL + "StaticContents/img/btn_approve.png' alt='Approve User' border='0'/></a>";
            buttonText += "<td>";
            buttonText += "<td>";
            buttonText += "<a href='" + siteURL + "MyAccount/ViewPendingUsers.aspx?UserId=" + sToUserInfoID + "&RegisID=" + registraionID + "&Status=Reject'><img src='" + siteURL + "StaticContents/img/btn_deny.png' alt='Reject User' border='0'/></a>";
            buttonText += "<td>";
            buttonText += "<td>";
            buttonText += "<a href='" + siteURL + "MyAccount/ViewPendingUsers.aspx?UserId=" + sToUserInfoID + "&RegisID=" + registraionID + "&Status=EditApproveProfile'><img src='" + siteURL + "StaticContents/img/btn_editprofile.png' alt='Edit User' border='0'/></a>";
            buttonText += "<td>";
            buttonText += "</tr>";
            buttonText += "</table>";

            messagebody.Replace("{ConfirmationButton}", buttonText);

            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();

            //Email Management
            if (HttpContext.Current.Request.IsLocal)
            {
                new CommonMails().SendEmail4Local(1092, "New Sign Up", "surendar.yadav@indianic.com", sSubject, messagebody.ToString(), "incentextest6@gmail.com", "test6incentex", true);
            }
            else
                new CommonMails().SendMail(sToUserInfoID, "New Sign Up", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
        }
    }

    public void CheckForCoupa()
    {
        UserInformation objUser = new UserInformation();
        UserInformationRepository objUserRepository = new UserInformationRepository();
        objUser = objUserRepository.AuthenticateUser("michael.wagner@asig.com", "abc123");
        //objUser = objUserRepository.AuthenticateUser("incentextest10@gmail.com", "abc123");
        if (objUser != null)
        {
            IncentexGlobal.CurrentMember = objUser;
            UdpateUserLoginDetailTC();
        }

        Response.Redirect("NewDesign/UserPages/Index.aspx", true);
    }
    #endregion
}