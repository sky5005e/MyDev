//Make company code changes by mayur on 08-05-2012

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class Vendorsignup : System.Web.UI.Page
{
    #region Data Members

    IncentexBEDataContext db = new IncentexBEDataContext();

    DataSet ds;
    CountryDA objCountry = new CountryDA();
    StateDA objState = new StateDA();
    CityDA objCity = new CityDA();
    String messagae = null;
    RegistrationBE objreg = new RegistrationBE();
    RegistrationDA objInsert = new RegistrationDA();
    Common objcomm = new Common();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
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
    Int64 DepartmentID
    {
        get
        {
            if (ViewState["DepartmentID"] == null)
            {
                ViewState["DepartmentID"] = 0;
            }
            return Convert.ToInt64(ViewState["DepartmentID"]);
        }
        set
        {
            ViewState["DepartmentID"] = value;
        }
    }
    Int64 WorkgroupID
    {
        get
        {
            if (ViewState["WorkgroupID"] == null)
            {
                ViewState["WorkgroupID"] = 0;
            }
            return Convert.ToInt64(ViewState["WorkgroupID"]);
        }
        set
        {
            ViewState["WorkgroupID"] = value;
        }
    }

    String CompanyName
    {
        get
        {
            return Session["CompanyName"].ToString();
        }
        set
        {
            Session["CompanyName"] = value;
        }
    }
    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Not Member Yet?";
            ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = false;
            ((HtmlImage)Master.FindControl("imgSupportTickets")).Visible = false;
            // to check session value
            if (Session["CompanyName"] != null)
            {
                this.CompanyName = Session["CompanyName"].ToString();
                this.txtCompanyName.Text = this.CompanyName;
            }
            FillCountry();
            BindGender();
            //FillBasedStationOnCountry();
            FillEmployeeType();

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

    /// <summary>
    /// Insert signup details
    /// </summary>    
    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {
        //This is for server side validation
        try
        {
            String strCompanyCode = txtCompanyCode.Text.Trim();
            CompanyID = Convert.ToInt64(strCompanyCode.Substring(0, strCompanyCode.IndexOf('-')));
            DepartmentID = Convert.ToInt64(strCompanyCode.Substring((strCompanyCode.IndexOf('-') + 1), strCompanyCode.IndexOf('-', (strCompanyCode.IndexOf('-') + 1)) - (strCompanyCode.IndexOf('-') + 1)));
            WorkgroupID = Convert.ToInt64(strCompanyCode.Substring((strCompanyCode.IndexOf('-', (strCompanyCode.IndexOf('-') + 1)) + 1), (strCompanyCode.Split('-').Length - 1) > 2 ? (strCompanyCode.LastIndexOf('-') - 1) - strCompanyCode.IndexOf('-', (strCompanyCode.IndexOf('-') + 1)) : strCompanyCode.Length - (strCompanyCode.LastIndexOf('-') + 1)));
            Boolean validationResult = false;
            if (new CompanyRepository().GetById(CompanyID) != null && new LookupRepository().GetById(DepartmentID) != null && new LookupRepository().GetById(DepartmentID).iLookupCode == "Department " && new LookupRepository().GetById(WorkgroupID) != null && new LookupRepository().GetById(WorkgroupID).iLookupCode == "Workgroup ")
                validationResult = true;
            if (validationResult == false)
            {
                lblmsg.InnerText = "Please enter valid company code.";
                return;
            }
        }
        catch
        {
            lblmsg.InnerText = "Please enter valid company code.";
            return;
        }

        Int32 ret = 0;

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

            objreg.SFirstName = txtFirstName.Text.Trim();
            objreg.SLastName = txtLastName.Text.Trim();
            objreg.SAddress1 = txtAddress.Text.Trim();
            //objreg.SAddress2 = txtAddress2.Text.Trim();
            objreg.ICountryId = Int64.Parse(ddlCountry.SelectedItem.Value);
            objreg.IStateId = Int64.Parse(ddlState.SelectedItem.Value);
            if (ddlCity.SelectedValue == "-1")
            {
                objreg.ICityId = Convert.ToInt64(Session["this.iCityID"]);
            }
            else
            {
                objreg.ICityId = Int64.Parse(ddlCity.SelectedItem.Value);
            }
            objreg.SZipCode = txtZip.Text.Trim();
            objreg.SEmailAddress = txtEmail.Text.Trim();
            objreg.STelephoneNumber = txtTelephone.Text.Trim();
            //objreg.SMobileNumber = txtMobilePhone.Text.Trim();
            objreg.SEmployeeId = txtEmployeeId.Text.Trim();
            objreg.IBasestationId = Int64.Parse(ddlBaseStation.SelectedValue);
            objreg.IGender = Int32.Parse(ddlGender.SelectedValue);

            objreg.iCompanyName = CompanyID;
            objreg.IWorkgroupId = WorkgroupID;
            objreg.IDepartmentId = DepartmentID;
            //if ((txtCompanyCode.Text.Trim().Split('-').Length - 1) > 2 && txtCompanyName.Text != String.Empty)
            objreg.SCompanyName = txtCompanyName.Text.Trim();

            objreg.Status = "pending";

            //Newly Added on 30 dec-2011 by Mayur
            if (ddlEmployeeType.SelectedIndex != 0)
                objreg.iEmployeeTypeID = Convert.ToInt64(ddlEmployeeType.SelectedValue);
            else
                objreg.iEmployeeTypeID = null;
            //End

            objreg.DateRequestSubmitted = System.DateTime.Now;
            if (txtHireDate.Text != String.Empty)
            {
                objreg.DateOfHired = Convert.ToDateTime(txtHireDate.Text);
            }
            else
            {
                objreg.DateOfHired = DateTime.MinValue;
            }

            ret = objInsert.Insert(objreg);

            if (ret != 1)
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
                //SendEmailToAllCA();//Commented by mayur on 13-March-2012 Due to client update
                //lblmsg.InnerText = "You have signed up successfully";                
                //ClearData();
            }
            //}
            //else
            //{
            //    lblmsg.InnerText = "Wrong verification code.";
            //}
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        if (ret == 1)
            Response.Redirect("signupconfirmation.aspx");
    }

    #endregion

    #region Methods

    /// <summary>
    /// To Return LINQ data to DataTable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="varlist"></param>
    /// <returns></returns>
    private DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
    {
        DataTable dtReturn = new DataTable();

        // column names 
        PropertyInfo[] propertyInfo = null;

        if (varlist == null) return dtReturn;

        foreach (T rec in varlist)
        {
            // Use reflection to get property names, to create table, Only first time, others will follow 
            if (propertyInfo == null)
            {
                propertyInfo = ((Type)rec.GetType()).GetProperties();
                foreach (PropertyInfo pi in propertyInfo)
                {
                    Type colType = pi.PropertyType;

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }

            DataRow dr = dtReturn.NewRow();

            foreach (PropertyInfo pi in propertyInfo)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                (rec, null);
            }

            dtReturn.Rows.Add(dr);
        }
        return dtReturn;
    }

    /// <summary>
    /// Fills the based station on country.
    /// </summary>
    /// <param name="countryid">The countryid.</param>
    public void FillBasedStationOnCountry(Int64 countryid)
    {
        //get BaseStation
        BaseStationRepository objRepo = new BaseStationRepository();
        List<INC_BasedStation> objList = objRepo.GetAllBaseStationbyCountryID(countryid).OrderBy(b => b.sBaseStation).ToList();
        if (objList.Count > 0)
            Common.BindDDL(ddlBaseStation, objList, "sBaseStation", "iBaseStationId", "-Select Base Station-");
        else
            ddlBaseStation.Items.Insert(0, new ListItem("-No BaseStation-", "0"));
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
        txtCompanyName.Text = String.Empty;
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtAddress.Text = "";
        //txtAddress2.Text = "";
        txtEmail.Text = "";
        //txtMobilePhone.Text = "";
        txtTelephone.Text = "";
        txtZip.Text = "";
        txtEmployeeId.Text = "";
        // ddlBasestation.SelectedIndex = 0;
        ddlCity.SelectedIndex = 0;
        ddlCountry.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        //ddlGender.SelectedIndex = 0;
        hdnRandomCaptcha.Value = "";
        Session.Remove("RandomCaptchaString");
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
            String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
            String sToadd = txtEmail.Text;
            //String sToadd = "ankit.h@indianic.com";
            String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
            String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
            StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
            messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            messagebody.Replace("{fullname}", txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim());
            messagebody.Replace("{storename}", new CompanyRepository().GetById(CompanyID).CompanyName);

            String smtphost = Application["SMTPHOST"].ToString();
            Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            String smtpUserID = Application["SMTPUSERID"].ToString();
            String smtppassword = Application["SMTPPASSWORD"].ToString();

            new CommonMails().SendMail(0, "New Sign Up", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
        }
    }

    private String RandomString(Int32 len)
    {
        Random r = new Random();
        String str = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
        StringBuilder sb = new StringBuilder();

        while ((len--) > 0)
            sb.Append(str[(Int32)(r.NextDouble() * str.Length)]);

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