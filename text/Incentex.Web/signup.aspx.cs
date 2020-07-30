using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class signup : System.Web.UI.Page
{
    #region Data Members

    IncentexBEDataContext db = new IncentexBEDataContext();
    Int64 CompanyID
    {
        get
        {
            if (ViewState["CompanyID"] == null)
            {
                ViewState["CompanyID"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyID"]);
        }
        set
        {
            ViewState["CompanyID"] = value;
        }
    }
    DataSet ds;
    CountryDA objCountry = new CountryDA();
    StateDA objState = new StateDA();
    CityDA objCity = new CityDA();
    string messagae = null;
    RegistrationDA objInsert = new RegistrationDA();
    Common objcomm = new Common();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Not Member Yet?";
            ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = false;
            ((HtmlImage)Master.FindControl("imgSupportTickets")).Visible = false;
            if (Session["CompanyId"] != null)
                this.CompanyID = Convert.ToInt64(Session["CompanyId"]);

            FillCountry();
            BindGender();
            bindCompany();
            FillEmployeeType();
            getsetRequiredHireDateFromGlobalMenu(sender, e);

            txtHireDate.Attributes.Add("readonly", "readonly");
        }
        Session["RandomCaptchaString"] = RandomString(5);
        hdnRandomCaptcha.Value = Convert.ToString(Session["RandomCaptchaString"]);
    }

    /// <summary>
    /// Fill the state dropdownliset on the country selection
    /// </summary>
    /// nagmani 28/07/2010
    /// <param name="sender">icountryid</param>
    /// <param name="e"></param>
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCountry.SelectedIndex <= 0)
            {
                ddlState.Enabled = false;
                ddlState.Items.Clear();
                ddlState.Items.Add(new ListItem("-select state-", "0"));

                ddlCity.Enabled = false;
                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));

            }
            else
            {
                ddlState.Enabled = true;
                ds = objState.GetStateByCountryID(Convert.ToInt32(ddlCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlState.DataSource = ds;
                        ddlState.DataValueField = "iStateID";
                        ddlState.DataTextField = "sStateName";
                        ddlState.DataBind();
                        ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

                        ddlCity.Items.Clear();
                        ddlCity.Items.Add(new ListItem("-select city-", "0"));
                        ddlCity.Items.Add(new ListItem("-Other-", "-1"));
                    }
                }
                FillBasedStationOnCountry(Convert.ToInt64(ddlCountry.SelectedItem.Value));
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            messagae = ex.Message;
        }
        finally
        {
            //ds.Dispose();
            //objState = null;
        }
    }

    /// <summary>
    /// Fill the city dropdownliset on the state selection
    /// </summary>
    /// nagmani 28/07/2010
    /// <param name="sender">istateid</param>
    /// <param name="e"></param>
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlState.SelectedIndex <= 0)
            {
                ddlCity.Enabled = false;
                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));
                ddlCity.Items.Add(new ListItem("-Other-", "-1"));
            }
            else
            {
                ddlCity.Enabled = true;
                ds = objCity.GetCityByStateID(Convert.ToInt32(ddlState.SelectedItem.Value));

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlCity.DataSource = ds;
                        ddlCity.DataValueField = "iCityID";
                        ddlCity.DataTextField = "sCityName";
                        ddlCity.DataBind();
                        ddlCity.Items.Insert(0, new ListItem("-select city-", "0"));
                        ddlCity.Items.Add(new ListItem("-Other-", "-1"));
                        // ddlCity.Items.Insert(ddlCity.Items.Count + 1,new ListItem("Other","-1"));
                    }
                    else
                    {
                        ddlCity.Items.Clear();
                        ddlCity.Items.Add(new ListItem("-select city-", "0"));

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            messagae = ex.Message;
        }
        finally
        {
            //ds.Dispose();
            //objCity = null;
        }
    }

    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCity.SelectedValue == "-1")
            PnlCityOther.Visible = true;
        else
            PnlCityOther.Visible = false;
    }

    protected void ddlWorkgroup__SelectedIndexChanged(object sender, EventArgs e)
    {
        getsetRequiredHireDateFromGlobalMenu(sender, e);
    }
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        getsetRequiredHireDateFromGlobalMenu(sender, e);

        if (!String.IsNullOrEmpty(ddlCompany.SelectedValue) && Convert.ToInt64(ddlCompany.SelectedValue) > 0)
        {
            Int64 storeID = new CompanyStoreRepository().GetStoreIDByCompanyId(Convert.ToInt64(ddlCompany.SelectedValue));

            if (Convert.ToInt64(ddlCompany.SelectedValue) > 0)
            {
                txtEmployeeId.Enabled = true;
                FillWorkGroupNDepartmentForStore(storeID);
            }
        }
        else
        {
            txtEmployeeId.Enabled = false;
            txtEmployeeId.Text = "";
            FillWorkGroupNDepartmentForStore(0);
        }
    }

    /// <summary>
    /// Insert signup details
    /// </summary>    
    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {

        Int64 UserInfoID = 0;
        try
        {

            //String challenge = Request["recaptcha_challenge_field"];
            //String reponseField = Request["recaptcha_response_field"];
            //String privateKey = "6Ld8B8wSAAAAACqW3xjDsdQ7O8hA_55M-F5BAvkF";
            //Recaptcha.RecaptchaValidator abc = new Recaptcha.RecaptchaValidator();
            //abc.Challenge = challenge;
            //abc.PrivateKey = privateKey;
            //abc.RemoteIP = Request.ServerVariables["REMOTE_ADDR"];
            //abc.Response = reponseField;
            //Recaptcha.RecaptchaResponse resp = abc.Validate();
            //if (Session["RandomCaptchaString"] != null && Convert.ToString(Session["RandomCaptchaString"]) == Convert.ToString(txtCaptcha.Text).Trim())
            //{

            //objreg.SCompanyName = "";

            Int64 CityID = 0;
            //Add new city to INC_City Table
            if (PnlCityOther.Visible == true && ddlCity.SelectedItem.Text == "-Other-")
            {
                CityRepository objCityRep = new CityRepository();
                Int64 countryID = Convert.ToInt64(ddlCountry.SelectedItem.Value);
                Int64 stateID = Convert.ToInt64(ddlState.SelectedItem.Value);

                INC_City objCity = objCityRep.CheckIfExist(countryID, stateID, txtCity.Text.Trim());
                if (objCity == null)
                {
                    INC_City objCity1 = new INC_City();
                    objCity1.iCountryID = countryID;
                    objCity1.iStateID = stateID;
                    objCity1.sCityName = txtCity.Text.Trim();
                    objCityRep.Insert(objCity1);
                    objCityRep.SubmitChanges();
                    CityID = objCity1.iCityID;
                }
                else
                    CityID = objCity.iCityID;
            }
            //End
            else
                CityID = Convert.ToInt64(ddlCity.SelectedValue);
            //End

            Inc_Registration objreg = new Inc_Registration();
            RegistrationRepository objRegistrationRepo = new RegistrationRepository();
            objreg.iCompanyName = Convert.ToInt64(ddlCompany.SelectedItem.Value);
            objreg.sFirstName = txtFirstName.Text.Trim();
            objreg.sLastName = txtLastName.Text.Trim();
            objreg.sAddress1 = txtAddress.Text.Trim();
            objreg.sAddress2 = null;// txtAddress2.Text.Trim();
            objreg.iCountryId = Int64.Parse(ddlCountry.SelectedItem.Value);
            objreg.iStateId = Int64.Parse(ddlState.SelectedItem.Value);
            objreg.iCityId = CityID;
            objreg.sZipCode = txtZip.Text.Trim();
            objreg.sEmailAddress = txtEmail.Text.Trim();
            objreg.sTelephoneNumber = txtTelephone.Text.Trim();
            objreg.sMobileNumber = null;
            objreg.sEmployeeId = txtEmployeeId.Text.Trim();
            objreg.iWorkgroupId = Int64.Parse(ddlWorkgroup.SelectedValue);
            objreg.iBasestationId = Int64.Parse(ddlBaseStation.SelectedValue);
            objreg.iGender = Int32.Parse(ddlGender.SelectedValue);
            objreg.iDepartment = null;
            //objreg.IDepartmentId = Int64.Parse(hdnDepartment.Value);
            objreg.status = "pending";

            //Newly Added on 30 dec-2011 by Mayur
            if (ddlEmployeeType.SelectedIndex != 0)
                objreg.iEmployeeTypeID = Convert.ToInt64(ddlEmployeeType.SelectedValue);
            else
                objreg.iEmployeeTypeID = null;
            //End

            objreg.DateRequestSubmitted = System.DateTime.Now;
            if (txtHireDate.Text != string.Empty)
            {
                objreg.DOH = Convert.ToDateTime(txtHireDate.Text);
            }
            else
            {
                objreg.DOH = DateTime.MinValue;
            }

            objreg.sPhoto = null;

            objRegistrationRepo.Insert(objreg);
            objRegistrationRepo.SubmitChanges();

            UserInfoID = objreg.iRegistraionID;

            if (UserInfoID < 1)
            {
                //sign up not successful. Process error.
                lblmsg.InnerText = objcomm.Callvalidationmessage(Server.MapPath("JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
                //ClearData();
            }
            else
            {
                //Send Email
                SendMail();
                //Send Email to All CA for that WG

                //Commented by mayur on 13-March-2012 Due to client update
                SendEmailToAllCA(UserInfoID);
                //uncommented by Prashant on 22nd August 2013 as per Dadra's Comment

                //lblmsg.InnerText = "You have signed up successfully";                
                //ClearData();
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (SqlException ex)
        {
            ErrHandler.WriteError(ex);
        }

        if (UserInfoID > 0)
            Response.Redirect("signupconfirmation.aspx");
    }

    #endregion

    #region Methods

    public void bindCompany()
    {
        //get company
        CompanyStoreRepository objRepo = new CompanyStoreRepository();
        List<CompanyStoreRepository.IECompanyListResults> lstStores = objRepo.GetCompanyStore().OrderBy(le => le.Company).ToList();
        Common.BindDDL(ddlCompany, lstStores, "Company", "CompanyId", "-select company-");
        if (this.CompanyID != 0)
        {
            ddlCompany.SelectedValue = this.CompanyID.ToString();
            txtEmployeeId.Enabled = true;
            CompanyStore objCompanyStore = new CompanyStoreRepository().GetByCompanyId(this.CompanyID);
            FillWorkGroupNDepartmentForStore(objCompanyStore != null ? Convert.ToInt64(objCompanyStore.StoreID) : 0);
        }
        else
        {
            txtEmployeeId.Enabled = false;
            FillWorkGroupNDepartmentForStore(0);
        }
    }

    /// <summary>
    /// Fills the based station on country.
    /// </summary>
    /// <param name="countryid">The countryid.</param>
    public void FillBasedStationOnCountry(Int64 countryid)
    {
        //get BaseStation
        ddlBaseStation.Items.Clear();

        BaseStationRepository objRepo = new BaseStationRepository();
        List<INC_BasedStation> objList = objRepo.GetAllBaseStationbyCountryID(countryid).OrderBy(b => b.sBaseStation).ToList();
        if (objList.Count > 0)
            Common.BindDDL(ddlBaseStation, objList, "sBaseStation", "iBaseStationId", "-Select Base Station-");
        else
            ddlBaseStation.Items.Insert(0, new ListItem("-No BaseStation-", "0"));

    }

    /// <summary>
    /// Fills the Workgroup.
    /// </summary>
    /// <param name="countryid">The countryid.</param>
    public void FillWorkGroupNDepartmentForStore(Int64 StoreID)
    {
        ddlWorkgroup.Items.Clear();

        CompanyStoreRepository objComStoRepo = new CompanyStoreRepository();

        List<GetStoreWorkGroupsResult> lstWorkGroups = new List<GetStoreWorkGroupsResult>();
        lstWorkGroups = objComStoRepo.GetStoreWorkGroups(StoreID).Where(le => le.Existing == 1).OrderBy(le => le.WorkGroup).ToList();

        if (lstWorkGroups.Count > 0)
            Common.BindDDL(ddlWorkgroup, lstWorkGroups, "WorkGroup", "WorkGroupID", "-Select Workgroup-");
        else
            ddlWorkgroup.Items.Insert(0, new ListItem("-No WorkGroup-", "0"));

    }

    /// <summary>
    /// ClearData()
    /// This method is used to clear the control after Submit the data in database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <CreateBy>Nagmani Kumar Date: July-26-2010</CreateBy>
    public void ClearData()
    {
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtAddress.Text = "";
        //txtAddress2.Text = "";
        txtEmail.Text = "";
        txtEmployeeId.Text = "";
        // ddlBasestation.SelectedIndex = 0;
        ddlCity.SelectedIndex = 0;
        ddlCountry.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        //ddlGender.SelectedIndex = 0;
        //ddlWorkgroup.SelectedIndex = 0;
        //ddlBasestation.SelectedIndex = 0;
        hdnRandomCaptcha.Value = "";
        Session.Remove("RandomCaptchaString");
    }

    /// <summary>
    /// FillCountry()
    /// This method is used to fill the country dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <CreateBy>Nagmani Kumar Date: July-26-2010</CreateBy>
    public void FillCountry()
    {
        try
        {
            ds = objCountry.GetCountry();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCountry.DataSource = ds;
                ddlCountry.DataTextField = "sCountryName";
                ddlCountry.DataValueField = "iCountryID";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("-select country-", "0"));
                ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;

                FillBasedStationOnCountry(Convert.ToInt64(ddlCountry.SelectedValue));

                ddlState.Enabled = true;
                ds = objState.GetStateByCountryID(Convert.ToInt32(ddlCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlState.DataSource = ds;
                        ddlState.DataValueField = "iStateID";
                        ddlState.DataTextField = "sStateName";
                        ddlState.DataBind();
                        ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

                        ddlCity.Items.Clear();
                        ddlCity.Items.Add(new ListItem("-select city-", "0"));
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            messagae = ex.Message;
        }
        finally
        {
            ds.Dispose();
            objCountry = null;
        }
    }

    /// <summary>
    /// Fills the type of the employee.
    /// </summary>
    public void FillEmployeeType()
    {
        LookupRepository objTitleLookRep = new LookupRepository();
        ddlEmployeeType.DataSource = objTitleLookRep.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.EmployeeType).OrderBy(le => le.sLookupName).ToList();
        ddlEmployeeType.DataValueField = "iLookupID";
        ddlEmployeeType.DataTextField = "sLookupName";
        ddlEmployeeType.DataBind();
        ddlEmployeeType.Items.Insert(0, new ListItem("-select employee type-", "0"));
    }
    //bind dropdown for gender, 
    public void BindGender()
    {

        LookupDA sLookup = new LookupDA();
        LookupBE sLookupBE = new LookupBE();

        // For Gender
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "Gender";
        ddlGender.DataSource = sLookup.LookUp(sLookupBE);
        ddlGender.DataValueField = "iLookupID";
        ddlGender.DataTextField = "sLookupName";
        ddlGender.DataBind();
        ddlGender.Items.Insert(0, new ListItem("-Select-", "0"));

    }

    private void SendMail()
    {
        //For Send mail
        EmailTemplateBE objEmailBE = new EmailTemplateBE();
        EmailTemplateDA objEmailDA = new EmailTemplateDA();
        DataSet dsEmailTemplate;
        //Get Email Content
        objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
        objEmailBE.STemplateName = "NewSignUp";
        dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
        if (dsEmailTemplate != null)
        {
            string sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
            string sToadd = txtEmail.Text.Trim();
            //string sToadd = "ankit.h@indianic.com";
            string sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
            string sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
            StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
            messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            messagebody.Replace("{fullname}", txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim());
            messagebody.Replace("{storename}", ddlCompany.SelectedItem.Text);

            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();

            new CommonMails().SendMail(0, "New Sign Up", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
        }
    }

    private void SendEmailToAllCA(Int64 UserInfoID)
    {
        List<GetUserApproverResult> objCA = new List<GetUserApproverResult>();
        CompanyEmployeeRepository objRep = new CompanyEmployeeRepository();
        objCA = objRep.GetUserApprover(UserInfoID, Convert.ToInt64(ddlCompany.SelectedValue), Convert.ToInt64(ddlWorkgroup.SelectedValue), Convert.ToInt64(ddlBaseStation.SelectedValue));
        foreach (GetUserApproverResult eachCA in objCA)
        {
            SendMailToAllCAForTheWorkgroup(eachCA.LoginEmail, eachCA.FirstName + " " + eachCA.LastName, eachCA.UserInfoID);
        }
    }

    private void SendMailToAllCAForTheWorkgroup(string email, string FullName, Int64 UserInfoID)
    {
        //For Send mail
        EmailTemplateBE objEmailBE = new EmailTemplateBE();
        EmailTemplateDA objEmailDA = new EmailTemplateDA();
        DataSet dsEmailTemplate;
        //Get Email Content
        objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
        objEmailBE.STemplateName = "NewSignUpForCA";
        dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
        if (dsEmailTemplate != null)
        {
            string sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
            string sToadd = email;
            Int64 sToUserInfoID = UserInfoID;
            //string sToadd = "ankit.h@indianic.com";
            string sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString() + " ( " + txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim() + " )";
            string sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
            StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
            messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            messagebody.Replace("{fullname}", FullName);
            messagebody.Replace("{First Name/Last Name} ", txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim());
            messagebody.Replace("{DateTime}", System.DateTime.Now.ToString());
            messagebody.Replace("{address}", txtAddress.Text.Trim());
            messagebody.Replace("{city}", ddlCity.SelectedItem.Text);
            messagebody.Replace("{state}", ddlState.SelectedItem.Text);
            messagebody.Replace("{Zip}", txtZip.Text.Trim());
            messagebody.Replace("{Telephone}", txtTelephone.Text.Trim());
            messagebody.Replace("{Email}", txtEmail.Text.Trim());
            messagebody.Replace("{EmmployeeID}", txtEmployeeId.Text.Trim());
            messagebody.Replace("{wg}", new LookupRepository().GetById((Convert.ToInt64(ddlWorkgroup.SelectedValue))).sLookupName);

            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();

            //Email Management
            new CommonMails().SendMail(sToUserInfoID, "New Sign Up", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
        }
    }

    void getsetRequiredHireDateFromGlobalMenu(object sender, EventArgs e)
    {
        if (ddlWorkgroup.SelectedValue != "0" && ddlCompany.SelectedIndex != 0)
        {
            GlobalMenuSetting objValue = new GlobalMenuSettingRepository().GetById(Convert.ToInt64(ddlWorkgroup.SelectedValue), new CompanyStoreRepository().GetStoreIDByCompanyId(Convert.ToInt64(ddlCompany.SelectedItem.Value)));
            if (objValue != null)
            {
                //Check if Paymnet option is ticked
                if (objValue.IsHiredDateTicked != null && (bool)objValue.IsHiredDateTicked)
                {
                    trHireDate.Visible = true;
                }
                else
                {
                    trHireDate.Visible = false;
                }
            }
        }
        else
        {
            trHireDate.Visible = false;
        }
    }

    private string RandomString(int len)
    {
        Random r = new Random();
        string str = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
        StringBuilder sb = new StringBuilder();

        while ((len--) > 0)
            sb.Append(str[(int)(r.NextDouble() * str.Length)]);

        return sb.ToString();
    }

    [WebMethod]
    public static String CheckDuplicateEmail(String email)
    {
        String methodResponse = String.Empty;

        try
        {
            if (!String.IsNullOrEmpty(email))
            {
                if (new UserInformationRepository().CheckEmailExistence(email, 0))
                {
                    if (new RegistrationRepository().CheckEmailExistence(email, 0))
                        methodResponse = "0";
                    else
                        methodResponse = "2";
                }
                else
                    methodResponse = "1";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return methodResponse;
    }

    [WebMethod]
    public static String CheckDuplicateEmployeeID(String employeeID, String companyID)
    {
        String methodResponse = String.Empty;

        try
        {
            if (!String.IsNullOrEmpty(employeeID) && !String.IsNullOrEmpty(companyID))
            {
                if (new CompanyEmployeeRepository().CheckEmployeeIDExistence(employeeID, Convert.ToInt64(companyID), 0))
                {
                    if (new RegistrationRepository().CheckEmployeeIDExistence(employeeID, Convert.ToInt64(companyID), 0))
                        methodResponse = "0";
                    else
                        methodResponse = "2";
                }
                else
                    methodResponse = "1";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return methodResponse;
    }

    #endregion
}