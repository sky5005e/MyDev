/// <summary>
/// Page created by mayur on 23-dec-2011 for design changes and remove some extrafields
/// Also add functionality of schedular event
/// Add companyemail,Employee Type,fax and extension field on page
/// remove Third Party Employee fields, Manager Order Approval System fields, Order Notifications fields, Return/Exchange Notifications fields, Shipment Notifications fields
/// </summary>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_Employee_BasicInformation : PageBase
{
    UserInformationRepository objUserInformationRepository = new UserInformationRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    INC_Lookup objLook = new INC_Lookup();
    LookupRepository objLookupRepos = new LookupRepository();
    CompanyRepository objCompanyRepos = new CompanyRepository();
    List<Company> objComplist = new List<Company>();

    EmailTemplateBE objEmailBE = new EmailTemplateBE();
    EmailTemplateDA objEmailDA = new EmailTemplateDA();
    DataSet dsEmailTemplate;

    DataSet ds;
    CountryDA objCountry = new CountryDA();
    StateDA objState = new StateDA();
    CityDA objCity = new CityDA();
    Common objcomm = new Common();
    Int64 CompanyId
    {
        get
        {
            if (ViewState["CompanyId"] == null)
            {
                ViewState["CompanyId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyId"]);
        }
        set
        {
            ViewState["CompanyId"] = value;
        }
    }

    Int64 EmployeeId
    {
        get
        {
            if (ViewState["EmployeeId"] == null)
            {
                ViewState["EmployeeId"] = 0;
            }
            return Convert.ToInt64(ViewState["EmployeeId"]);
        }
        set
        {
            ViewState["EmployeeId"] = value;
        }
    }
    string WorkgroupName
    {
        get
        {
            if (ViewState["WorkgroupName"] == null)
            {
                ViewState["WorkgroupName"] = 0;
            }
            return Convert.ToString(ViewState["WorkgroupName"]);
        }
        set
        {
            ViewState["WorkgroupName"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Common.SetModuleMenu(Incentex.DAL.Common.DAEnums.ManageID.ManageCompany); 
            Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.ManageCompany;
            ///hi

            if (Request.QueryString.Count > 0)
            {
                CheckLogin();
                BindValues();
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("SubId"));
                if (Request.QueryString.Get("wg") != null)
                {
                    //Get Lookup name by ID
                    this.WorkgroupName = Request.QueryString.Get("wg");
                    INC_Lookup objLookup = new LookupRepository().GetById(Convert.ToInt64(this.WorkgroupName));
                    txtWorkgroup.Text = objLookup.sLookupName;
                }
                IncentexGlobal.ManageID = 7;
                //populatesubmenu();
                menucontrol.PopulateMenu(0, 0, this.CompanyId, this.EmployeeId, true);
                txtActivatedBy.Text = IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName;
                txtDateRequestSubmitted.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                Company objCompany = new CompanyRepository().GetById(this.CompanyId);
                if (objCompany == null)
                {
                    Response.Redirect("~/MyAccount/Employee/ViewEmployee.aspx");
                }
                ((Label)Master.FindControl("lblPageHeading")).Text = "Basic Information";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/Employee/ViewEmployee.aspx?id=" + this.CompanyId;

                //set Search Page Return URL
                if (IncentexGlobal.SearchPageReturnURLFrontSide != null)
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.SearchPageReturnURLFrontSide;
                }

            }
            else
            {
                Response.Redirect("~/MyAccount/Employee/ViewEmployee.aspx");
            }

            FillCountry();
            FillEmployeeTitle();
            bindEmployyeType();
            FillEmployeeType();
            DisplayData();
        }
    }

    public void FillEmployeeTitle()
    {
        LookupRepository objTitleLookRep = new LookupRepository();
        string strPayment = "EmployeeTitle ";
        ddlTitle.DataSource = objTitleLookRep.GetByLookup(strPayment);
        ddlTitle.DataValueField = "iLookupID";
        ddlTitle.DataTextField = "sLookupName";
        ddlTitle.DataBind();
        ddlTitle.Items.Insert(0, new ListItem("-select title-", "0"));
    }

    //Newly added by mayur on 30 dec-2011
    public void FillEmployeeType()
    {
        LookupRepository objTitleLookRep = new LookupRepository();
        string strPayment = "EmployeeType ";
        ddlEmployeeTypeLast.DataSource = objTitleLookRep.GetByLookup(strPayment);
        ddlEmployeeTypeLast.DataValueField = "iLookupID";
        ddlEmployeeTypeLast.DataTextField = "sLookupName";
        ddlEmployeeTypeLast.DataBind();
        ddlEmployeeTypeLast.Items.Insert(0, new ListItem("-select employee type-", "0"));
    }

    //bind dropdown for the workgroup, department, gender, 
    public void BindValues()
    {
        //For Product Status 
        LookupRepository objLookupRepo = new LookupRepository();
        ddlStatus.DataSource = objLookupRepo.GetByLookup("Status");
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Department
        ddlDepartment.DataSource = objLookupRepo.GetByLookup("Department");
        ddlDepartment.DataValueField = "iLookupID";
        ddlDepartment.DataTextField = "sLookupName";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Gender
        ddlGender.DataSource = objLookupRepo.GetByLookup("Gender");
        ddlGender.DataValueField = "iLookupID";
        ddlGender.DataTextField = "sLookupName";
        ddlGender.DataBind();
        ddlGender.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Regoin
        ddlRegion.DataSource = objLookupRepo.GetByLookup("Region");
        ddlRegion.DataValueField = "iLookupID";
        ddlRegion.DataTextField = "sLookupName";
        ddlRegion.DataBind();
        ddlRegion.Items.Insert(0, new ListItem("-Select-", "0"));

        //BaseStation
    }

    void DisplayData()
    {
        hdnCompanyID.Value = Convert.ToString(this.CompanyId);

        if (this.EmployeeId != 0)
        {
            lnkApplychanges.Visible = true;
            CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetById(this.EmployeeId);
            if (objCompanyEmployee == null)
            {
                return;
            }
            //Get and set data from userinformation table
            UserInformation objUserInfo = objUserInformationRepository.GetById(objCompanyEmployee.UserInfoID);
            hdnUserInfoID.Value = Convert.ToString(objUserInfo.UserInfoID);
            hdnRegistrationID.Value = Convert.ToString(objUserInfo.RegistrationID);

            #region AnniversaryDate
            //Set Anniversary Date Added on 27 Jan 2011
            AnniversaryProgramRepository objCEProgram = new AnniversaryProgramRepository();
            SelectAnniversaryCreditProgramPerEmployeeResult objCE = objCEProgram.GetCompanyEmployeeAnniversaryCreditDetails(objCompanyEmployee.UserInfoID);
            // txtAnniversaryDate.Text = Convert.ToDateTime(objCE.NewHiredDate).ToString("MM/dd/yyyy");

            if (Convert.ToDateTime(objCE.NewHiredDate).ToString("MM/dd/yyyy") != "01/01/0001")
            {
                txtAnniversaryDate.Text = Convert.ToDateTime(objCE.NewHiredDate).ToString("MM/dd/yyyy");
            }
            else
            {
                txtAnniversaryDate.Text = "";
            }

            //End
            #endregion

            //Get & set Lookup name by ID
            this.WorkgroupName = objCompanyEmployee.WorkgroupID.ToString();
            INC_Lookup objLookup = new LookupRepository().GetById(Convert.ToInt64(this.WorkgroupName));
            txtWorkgroup.Text = objLookup.sLookupName;

            //lblFullName.Text = objUserInfo.FirstName +" "+objUserInfo.LastName;
            txtFirstName.Text = objUserInfo.FirstName;
            txtMiddleName.Text = objUserInfo.MiddleName;
            txtLastName.Text = objUserInfo.LastName;
            txtAdress1.Text = objUserInfo.Address1;
            txtAddress2.Text = objUserInfo.Address2;
            ddlStatus.SelectedValue = objUserInfo.WLSStatusId != null ? objUserInfo.WLSStatusId.ToString() : "0"; //change by mayur for set 0 if null

            ddlCountry.SelectedValue = objUserInfo.CountryId.ToString();
            ddlCountry_SelectedIndexChanged(null, null);

            ddlState.SelectedValue = objUserInfo.StateId.ToString();
            ddlState_SelectedIndexChanged(null, null);
            ddlCity.SelectedValue = objUserInfo.CityId.ToString();
            if (objCompanyEmployee.EmployeeTitleId.ToString() != string.Empty)
            {
                ddlTitle.Items.FindByValue(objCompanyEmployee.EmployeeTitleId.ToString()).Selected = true;
            }
            if (objCompanyEmployee.EmployeeTypeID != null)
            {
                ddlEmployeeTypeLast.Items.FindByValue(objCompanyEmployee.EmployeeTypeID.ToString()).Selected = true;
            }
            ddlDepartment.SelectedValue = objCompanyEmployee.DepartmentID != null ? objCompanyEmployee.DepartmentID.ToString() : "0"; //change by mayur for set 0 if null
            //txtWorkgroup.Text = this.WorkgroupName;
            //hdnWorkgroup.Value = objCompanyEmployee.WorkgroupID.ToString();
            ddlGender.SelectedValue = objCompanyEmployee != null ? objCompanyEmployee.GenderID.ToString() : "0";//change by mayur for set 0 if null
            ddlRegion.SelectedValue = objCompanyEmployee.RegionID != null ? objCompanyEmployee.RegionID.ToString() : "0";//change by mayur for set 0 if null
            ddlBaseStation.SelectedValue = objCompanyEmployee.BaseStation != null ? objCompanyEmployee.BaseStation.ToString() : "0";//change by mayur for set 0 if null

            txtZip.Text = objUserInfo.ZipCode;
            txtTelephone.Text = objUserInfo.Telephone;
            txtExtension.Text = objUserInfo.Extension;
            txtMobile.Text = objUserInfo.Mobile;
            txtFax.Text = objUserInfo.Fax;
            txtEmail.Text = objUserInfo.Email;
            txtCompanyEmail.Text = objCompanyEmployee.CompanyEmail;
            txtLoginEmail.Text = objUserInfo.LoginEmail;
            //
            ViewState["LoginEmail"] = objUserInfo.LoginEmail;
            //
            txtPassword.Text = objUserInfo.Password;
            if (objCompanyEmployee.isCompanyAdmin)
            {
                ddlEmployeeType.Items.FindByText(Incentex.DA.DAEnums.CompanyEmployeeTypes.Admin.ToString()).Selected = true;
                trMOASPayment.Visible = true;
            }
            else
            {
                //add by mayur for third party supplier employee on 10-may-2012
                trMOASPayment.Visible = false;
                if (objUserInfo.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
                {
                    ddlEmployeeType.Items.FindByText(Incentex.DA.DAEnums.CompanyEmployeeTypes.XSE.ToString()).Selected = true;
                    trSupplierCompanyName.Visible = true;
                    txtSupplierCompanyName.Text = objCompanyEmployee.ThirdPartySupplierCompanyName;
                }
                else
                    ddlEmployeeType.Items.FindByText(Incentex.DA.DAEnums.CompanyEmployeeTypes.Employee.ToString()).Selected = true;
            }

            //ViewState["DOH"] = txtDateOfHired.Text = Convert.ToDateTime(objCompanyEmployee.HirerdDate).ToString("MM/dd/yyyy");
            if (Convert.ToDateTime(objCompanyEmployee.HirerdDate).ToString("MM/dd/yyyy") != "01/01/0001")
            {
                ViewState["DOH"] = txtDateOfHired.Text = Convert.ToDateTime(objCompanyEmployee.HirerdDate).ToString("MM/dd/yyyy");
            }
            else
            {
                txtDateOfHired.Text = "";
            }
            txtEmployeeId.Text = objCompanyEmployee.EmployeeID;
            ViewState["EmployeeNumber"] = objCompanyEmployee.EmployeeID;
            //if(objCompanyEmployee.UploadImage != null)
            hdPriPhoto.Value = objCompanyEmployee.UploadImage;

            txtActivatedBy.Text = IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName;
            if (objCompanyEmployee.StoreActivatedDate != null)
                txtDateActivated.Text = DateTime.Parse(objCompanyEmployee.StoreActivatedDate.ToString()).ToString("MM/dd/yyyy");

            ddlMOASPayment.SelectedValue = objCompanyEmployee.IsMOASApprover.ToString();

            if (txtDateRequestSubmitted.Text != "")
                txtDateRequestSubmitted.Text = objCompanyEmployee.DateRequestSubmitted;
            else
                txtDateRequestSubmitted.Text = System.DateTime.Now.ToString("MM/dd/yyyy");

            DisplayNotes();
        }

    }
    public void bindEmployyeType()
    {
        Dictionary<int, string> ht = Common.GetEnumForBind(typeof(Incentex.DA.DAEnums.CompanyEmployeeTypes));

        ddlEmployeeType.DataSource = ht;
        ddlEmployeeType.DataTextField = "value";
        ddlEmployeeType.DataValueField = "key";
        ddlEmployeeType.DataBind();
        ddlEmployeeType.Items.Insert(0, new ListItem("-select-", "0"));



    }
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
        }
        finally
        {
            ds.Dispose();
            objCountry = null;
        }
    }
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

                        //ddlCity.Items.Clear();
                        //ddlCity.Items.Add(new ListItem("-select city-", "0"));
                    }
                }
                FillBasedStationOnCountry(Convert.ToInt64(ddlCountry.SelectedItem.Value));

            }
            if (ddlCountry.SelectedValue == "0" && ddlState.SelectedValue == "0")
            {
                ddlCity.Items.Remove(new ListItem("-Other-", "-Other-"));

            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        finally
        {
            //ds.Dispose();
            //objState = null;
        }
    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlState.SelectedIndex <= 0)
            {
                ddlCity.Enabled = false;
                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));
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
                    }
                }
            }

            if (ddlState.SelectedIndex > 0)
            {
                //DrpCity.Items.Add(new ListItem("-Other-", "-1"));
                ddlCity.Items.Insert(1, "-Other-");
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        finally
        {
            //ds.Dispose();
            //objCity = null;
        }
    }
    protected void ddlEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEmployeeType.SelectedItem.Text == Incentex.DA.DAEnums.CompanyEmployeeTypes.XSE.ToString())
        {
            trSupplierCompanyName.Visible = true;
            trMOASPayment.Visible = false;
        }
        else if (ddlEmployeeType.SelectedItem.Text == Incentex.DA.DAEnums.CompanyEmployeeTypes.Admin.ToString())
        {
            trSupplierCompanyName.Visible = false;
            trMOASPayment.Visible = true;
        }
        else
        {
            trSupplierCompanyName.Visible = false;
            trMOASPayment.Visible = false;
        }
    }
    public void FillBasedStationOnCountry(long countryid)
    {
        BaseStationDA sBaseStation = new BaseStationDA();
        BasedStationBE sBSBe = new BasedStationBE();

        sBSBe.SOperation = "getBaseStationbyCounty";
        sBSBe.iCountryID = countryid;
        DataSet dsBaseStation = sBaseStation.GetBaseStaionByCountry(sBSBe);
        if (dsBaseStation.Tables.Count > 0 && dsBaseStation.Tables[0].Rows.Count > 0)
        {
            ddlBaseStation.DataSource = dsBaseStation.Tables[0];
            ddlBaseStation.DataValueField = "iBaseStationId";
            ddlBaseStation.DataTextField = "sBaseStation";
            ddlBaseStation.DataBind();
            ddlBaseStation.Items.Insert(0, new ListItem("-select basestation-", "0"));
        }
    }
    protected void lnkApplychanges_Click(object sender, EventArgs e)
    {
        try
        {
            UserInformation objUserInfo = new UserInformation();
            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
            if (this.EmployeeId != 0)
            {
                objCompanyEmployee = objCompanyEmployeeRepository.GetById(this.EmployeeId);
                objUserInfo = objUserInformationRepository.GetById(objCompanyEmployee.UserInfoID);

                if (ViewState["LoginEmail"] != null)
                {
                    if (ViewState["LoginEmail"].ToString() != txtLoginEmail.Text)
                    {
                        if (!objUserInformationRepository.CheckEmailExistence(txtLoginEmail.Text.Trim(), objCompanyEmployee.UserInfoID))
                        {
                            lblMsg.Text = "Login Email already exist ...";
                            return;
                        }
                    }
                }
                else if (!objUserInformationRepository.CheckEmailExistence(txtLoginEmail.Text.Trim(), objCompanyEmployee.UserInfoID))
                {
                    lblMsg.Text = "Login Email already exist ...";
                    return;
                }

                objUserInfo.LoginEmail = txtLoginEmail.Text;
                objUserInfo.Password = txtPassword.Text;
                objUserInfo.WLSStatusId = Convert.ToInt32(ddlStatus.SelectedValue);
                objUserInformationRepository.SubmitChanges();
                ViewState["LoginEmail"] = txtLoginEmail.Text;
                objLook = objLookupRepos.GetById(Convert.ToInt32(ddlStatus.SelectedValue));
                if (objLook.sLookupName == "Active")
                {
                    //after Active mail sent to employee
                    sendApprovalEmail(txtLoginEmail.Text, txtPassword.Text, objUserInfo.FirstName + " " + objUserInfo.LastName, objUserInfo.UserInfoID);


                }
                else
                {
                    //after Inactive mail sent to employee
                    sendInApprovalEmail(txtLoginEmail.Text, objUserInfo.FirstName + " " + objUserInfo.LastName, objUserInfo.UserInfoID);
                }
            }
        }
        catch (Exception ex)
        {

            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);

        }

    }
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 CityID = 0;
            //Start Add City when Other Selection form city dropdownlist"
            if (PnlCityOther.Visible == true && ddlCity.SelectedValue == "-Other-")
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

            UserInformation objUserInfo = new UserInformation();
            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
            if (this.EmployeeId != 0)
            {
                objCompanyEmployee = objCompanyEmployeeRepository.GetById(this.EmployeeId);
                objUserInfo = objUserInformationRepository.GetById(objCompanyEmployee.UserInfoID);
            }
            if (ViewState["LoginEmail"] != null)
            {
                if (ViewState["LoginEmail"].ToString() != txtLoginEmail.Text)
                {
                    if (!objUserInformationRepository.CheckEmailExistence(txtLoginEmail.Text.Trim(), objCompanyEmployee.UserInfoID))
                    {
                        lblMsg.Text = "Login Email already exist ...";
                        return;
                    }
                }
            }
            else if (!objUserInformationRepository.CheckEmailExistence(txtLoginEmail.Text.Trim(), objCompanyEmployee.UserInfoID))
            {
                lblMsg.Text = "Login Email already exist ...";
                return;
            }

            //check for employee id exist or not
            if (ViewState["EmployeeNumber"] != null)
            {
                if (ViewState["EmployeeNumber"].ToString() != txtEmployeeId.Text.Trim())
                {
                    if (!objCompanyEmployeeRepository.CheckEmployeeIDExistence(txtEmployeeId.Text.Trim(), this.CompanyId, objCompanyEmployee.UserInfoID))
                    {
                        lblMsg.Text = "Employee # already exist ...";
                        return;
                    }
                    else if (!new RegistrationRepository().CheckEmployeeIDExistence(txtEmployeeId.Text.Trim(), this.CompanyId, objCompanyEmployee.UserInfoID))
                    {
                        lblMsg.Text = "Employee # already exist in registration requests...";
                        return;
                    }
                }
            }
            else if (!objCompanyEmployeeRepository.CheckEmployeeIDExistence(txtEmployeeId.Text.Trim(), this.CompanyId, objCompanyEmployee.UserInfoID))
            {
                lblMsg.Text = "Employee # already exist ...";
                return;
            }
            else if (!new RegistrationRepository().CheckEmployeeIDExistence(txtEmployeeId.Text.Trim(), this.CompanyId, objCompanyEmployee.UserInfoID))
            {
                lblMsg.Text = "Employee # already exist in registration requests...";
                return;
            }

            objUserInfo.FirstName = txtFirstName.Text;
            objUserInfo.LastName = txtLastName.Text;
            objUserInfo.MiddleName = txtMiddleName.Text;
            objUserInfo.Address1 = txtAdress1.Text;
            objUserInfo.WLSStatusId = Convert.ToInt32(ddlStatus.SelectedValue);
            objUserInfo.Address2 = txtAddress2.Text;
            objUserInfo.CountryId = Convert.ToInt64(ddlCountry.SelectedItem.Value);
            objUserInfo.StateId = Convert.ToInt64(ddlState.SelectedItem.Value);
            objUserInfo.CityId = CityID;
            objUserInfo.ZipCode = txtZip.Text;
            objUserInfo.Telephone = txtTelephone.Text;
            objUserInfo.Extension = txtExtension.Text;
            objUserInfo.Mobile = txtMobile.Text;
            objUserInfo.Fax = txtFax.Text;
            objUserInfo.Email = txtEmail.Text;
            objUserInfo.LoginEmail = txtLoginEmail.Text;
            objUserInfo.Password = txtPassword.Text;
            objUserInfo.CreatedDate = System.DateTime.Now;
            objUserInfo.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objUserInfo.CompanyId = this.CompanyId;
            objUserInfo.Usertype = Convert.ToInt64(ddlEmployeeType.SelectedItem.Value);
            if (this.EmployeeId == 0)
            {
                objUserInformationRepository.Insert(objUserInfo);
            }
            else
                objUserInfo.UpdatedDate = DateTime.Now;

            objUserInformationRepository.SubmitChanges();

            //insert record in the userinfo table and then insert other details in company employee table
            if (this.EmployeeId != 0)
            {
                objCompanyEmployee = objCompanyEmployeeRepository.GetById(this.EmployeeId);
            }

            objCompanyEmployee.UserInfoID = objUserInfo.UserInfoID;
            objCompanyEmployee.CompanyEmail = txtCompanyEmail.Text;
            //
            if (ddlEmployeeType.SelectedItem.Text == Incentex.DA.DAEnums.CompanyEmployeeTypes.Admin.ToString())
            {
                objCompanyEmployee.isCompanyAdmin = true;
                objCompanyEmployee.IsMOASApprover = ddlMOASPayment.SelectedValue == "True" ? true : false;
                //objCompanyEmployee.ManagementControlForWorkgroup = Convert.ToInt64(hdnWorkgroup.Value);
                objCompanyEmployee.ManagementControlForDepartment = Convert.ToInt64(ddlDepartment.SelectedValue);
                objCompanyEmployee.ManagementControlForRegion = Convert.ToInt64(ddlRegion.SelectedValue);
                objCompanyEmployee.ManagementControlForStaionlocation = Convert.ToInt64(ddlBaseStation.SelectedValue);

                //Remove MOAS as payment option
                if (this.EmployeeId != 0 && ddlMOASPayment.SelectedValue != "True")
                {
                    if (objCompanyEmployee.Paymentoption != null)
                    {
                        List<string> lstSelectedPaymentOptions = new List<string>(objCompanyEmployee.Paymentoption.Split(','));
                        string MOASID = new LookupRepository().GetIdByLookupNameNLookUpCode("MOAS", "WLS Payment Option").ToString();
                        for (int i = 0; i < lstSelectedPaymentOptions.Count; i++)
                        {
                            if (MOASID == lstSelectedPaymentOptions[i])
                                lstSelectedPaymentOptions.RemoveAt(i);
                        }

                        objCompanyEmployee.Paymentoption = string.Join(",", lstSelectedPaymentOptions.ToArray());
                    }
                    objCompanyEmployee.MOASEmailAddresses = null;
                }
            }
            else
            {
                objCompanyEmployee.isCompanyAdmin = false;
                objCompanyEmployee.IsMOASApprover = false;
                objCompanyEmployee.ManagementControlForWorkgroup = null;
                objCompanyEmployee.ManagementControlForDepartment = null;
                objCompanyEmployee.ManagementControlForRegion = null;
                objCompanyEmployee.ManagementControlForStaionlocation = null;
                //Remove menu right also
                if (this.EmployeeId != 0)
                {
                    CompanyEmpMenuAccessRepository objCmpMenuAccesRepos = new CompanyEmpMenuAccessRepository();
                    List<CompanyEmpMenuAccess> lst = objCmpMenuAccesRepos.getMenuByUserType(this.EmployeeId, "Company Admin");

                    foreach (CompanyEmpMenuAccess l in lst)
                    {
                        objCmpMenuAccesRepos.Delete(l);
                        objCmpMenuAccesRepos.SubmitChanges();
                    }

                    //Remove Bulk Order feature for employee
                    if (objCompanyEmployee.Userstoreoption != null)
                    {
                        List<string> lstSelectedStoreOption = new List<string>(objCompanyEmployee.Userstoreoption.Split(','));
                        string BulkOrderID = new LookupRepository().GetIdByLookupNameNLookUpCode("Bulk Order", "UserStoreOptions").ToString();
                        string NameToEngraveID = new LookupRepository().GetIdByLookupNameNLookUpCode("Name to Engrave", "UserStoreOptions").ToString();
                        for (int i = 0; i < lstSelectedStoreOption.Count; i++)
                        {
                            if (BulkOrderID == lstSelectedStoreOption[i] || NameToEngraveID == lstSelectedStoreOption[i])
                            {
                                lstSelectedStoreOption.RemoveAt(i);
                                i--;
                            }
                        }
                        objCompanyEmployee.Userstoreoption = string.Join(",", lstSelectedStoreOption.ToArray());
                    }
                }
            }

            //
            if (hdPriPhoto.Value != "")
                objCompanyEmployee.UploadImage = hdPriPhoto.Value;
            else
                objCompanyEmployee.UploadImage = null;

            if (!string.IsNullOrEmpty(txtDateOfHired.Text))
                objCompanyEmployee.HirerdDate = Convert.ToDateTime(txtDateOfHired.Text);

            objCompanyEmployee.EmployeeID = txtEmployeeId.Text;
            objCompanyEmployee.RegionID = Convert.ToInt64(ddlRegion.SelectedValue);
            objCompanyEmployee.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
            if (ddlBaseStation.SelectedValue == "0")
                objCompanyEmployee.BaseStation = null;
            else
                objCompanyEmployee.BaseStation = Convert.ToInt64(ddlBaseStation.SelectedValue);
            objCompanyEmployee.GenderID = Convert.ToInt64(ddlGender.SelectedValue);
            objCompanyEmployee.WorkgroupID = Convert.ToInt64(this.WorkgroupName);
            //objCompanyEmployee.WorkgroupID = Convert.ToInt64(hdnWorkgroup.Value);

            objCompanyEmployee.StoreActivatedBy = txtActivatedBy.Text;

            objCompanyEmployee.StoreActivatedDate = Convert.ToDateTime(txtDateActivated.Text);
            objCompanyEmployee.DateRequestSubmitted = txtDateRequestSubmitted.Text;

            //Newly Added on 15 feb by Ankit
            if (ddlTitle.SelectedIndex != 0)
            {
                objCompanyEmployee.EmployeeTitleId = Convert.ToInt64(ddlTitle.SelectedItem.Value);
            }
            else
            {
                objCompanyEmployee.EmployeeTitleId = null;
            }

            //Newly Added on 30 dec-2011 by Mayur
            if (ddlEmployeeTypeLast.SelectedIndex != 0)
            {
                objCompanyEmployee.EmployeeTypeID = Convert.ToInt64(ddlEmployeeTypeLast.SelectedValue);
            }
            else
            {
                objCompanyEmployee.EmployeeTypeID = null;
            }
            //End

            if (trSupplierCompanyName.Visible == true)
                objCompanyEmployee.ThirdPartySupplierCompanyName = txtSupplierCompanyName.Text.Trim();
            else
                objCompanyEmployee.ThirdPartySupplierCompanyName = null;

            if (this.EmployeeId == 0)
            {
                objCompanyEmployeeRepository.Insert(objCompanyEmployee);

            }
            objCompanyEmployeeRepository.SubmitChanges();

            #region anniversary region

            //Newly added on 26 aug 2011
            //Now Update the Anniversary Credits
            if (this.EmployeeId != 0)
            {
                if (checkifhireddateischanged())
                {
                    //Update Anniversary Credit Details
                    //If new hire date is there in the cycle..
                    AnniversaryProgramRepository objUserData = new AnniversaryProgramRepository();
                    SelectAnniversaryCreditProgramPerEmployeeResult objUserDataResult = objUserData.GetCompanyEmployeeAnniversaryCreditDetails(objUserInfo.UserInfoID);
                    //If Applicable for the credit program or not!
                    if (objUserDataResult.AmountFromProgram != null)
                    {

                        objCompanyEmployee.CreditAmtToApplied = objUserDataResult.AmountFromProgram;
                        objCompanyEmployee.NextCreditAppliedOn = Convert.ToDateTime(objUserDataResult.IssueCreditOn).AddYears(1).ToShortDateString();
                        if (((DateTime)objUserDataResult.CreditExpireAfter).Year.ToString() == "9999")
                        {
                            objCompanyEmployee.CreditExpireOn = "---";
                            objCompanyEmployee.CreditAmtToExpired = 0;
                        }
                        else
                        {
                            objCompanyEmployee.CreditExpireOn = Convert.ToDateTime(objUserDataResult.CreditExpireAfter).AddYears(1).ToShortDateString();
                            objCompanyEmployee.CreditAmtToExpired = objUserDataResult.AmountFromProgram;
                        }
                        objCompanyEmployeeRepository.SubmitChanges();
                    }
                    //End
                }
            }
            #endregion

            this.EmployeeId = objCompanyEmployee.CompanyEmployeeID;
            Response.Redirect("SpecialProgram.aspx?Id=" + this.CompanyId + "&SubId=" + this.EmployeeId);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }
    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        modalAddnotes.Show();
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }
    void DisplayNotes()
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        List<NoteDetail> objList = objRepo.GetByForeignKeyId(this.EmployeeId, Incentex.DAL.Common.DAEnums.NoteForType.CompanyEmployee);
        txtNoteHistory.Text = string.Empty;
        foreach (NoteDetail obj in objList)
        {
            txtNoteHistory.Text += "Created Date : " + Convert.ToDateTime(obj.CreateDate).ToString("dd-MMM-yyyy") + "\n";
            txtNoteHistory.Text += "Created Time : " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";

            UserInformation objUser = objUserInformationRepository.GetById(obj.CreatedBy);

            if (objUser != null)
            {
                txtNoteHistory.Text += "Created By : " + objUser.FirstName + " " + objUser.LastName + "\n";
            }


            txtNoteHistory.Text += "Note : " + obj.Notecontents + "\n";
            txtNoteHistory.Text += "---------------------------------------------------------------------------\n";


        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNote.Text))
        {
            return;
        }
        try
        {
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            NoteDetail obj = new NoteDetail()
            {
                ForeignKey = this.EmployeeId,
                Notecontents = txtNote.Text
                ,
                NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CompanyEmployee)
                ,
                CreatedBy = IncentexGlobal.CurrentMember.UserInfoID
                ,
                CreateDate = DateTime.Now
                ,
                UpdateDate = DateTime.Now
                ,
                UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID
            };

            objRepo.Insert(obj);
            objRepo.SubmitChanges();
            DisplayNotes();
            txtNote.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }

    }
    private bool checkifhireddateischanged()
    {
        if (ViewState["DOH"] != null)
        {
            if (ViewState["DOH"].ToString() != txtDateOfHired.Text)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCity.SelectedValue == "-Other-")
        {
            PnlCityOther.Visible = true;

        }
        else
        {
            PnlCityOther.Visible = false;
        }
    }

    #region Send email method
    private void sendInApprovalEmail(string psEmailAddress, string psUserName, Int64 piUserInfoID)
    {

        try
        {
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Employee Inactivation";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                string sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                string sToadd = psEmailAddress;
                string sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                string sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{fullname}", psUserName);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                string smtphost = Application["SMTPHOST"].ToString();
                int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                string smtpUserID = Application["SMTPUSERID"].ToString();
                string smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(piUserInfoID, "Basic Information", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    private void sendApprovalEmail(string psEmailAddress, string psPassword, string psUserName, Int64 piUserInfoID)
    {

        try
        {
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "employee activation";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                string sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                string sToadd = psEmailAddress;
                string sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                string sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{fullname}", psUserName);
                messagebody.Replace("{password}", psPassword);
                messagebody.Replace("{email}", psEmailAddress);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                objComplist = objCompanyRepos.GetByCompanyId(this.CompanyId);
                if (objComplist.Count > 0)
                {
                    messagebody.Replace("{storename}", objComplist[0].CompanyName);
                }

                string smtphost = Application["SMTPHOST"].ToString();
                int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                string smtpUserID = Application["SMTPUSERID"].ToString();
                string smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(piUserInfoID, "Basic Information", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion

    [WebMethod]
    public static String CheckDuplicateEmail(String email, String userInfoID, String registrationID)
    {
        String methodResponse = String.Empty;

        try
        {
            if (!String.IsNullOrEmpty(email))
            {
                if (new UserInformationRepository().CheckEmailExistence(email, !String.IsNullOrEmpty(userInfoID) ? Convert.ToInt64(userInfoID) : 0))
                {
                    if (new RegistrationRepository().CheckEmailExistence(email, !String.IsNullOrEmpty(registrationID) ? Convert.ToInt64(registrationID) : 0))
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
    public static String CheckDuplicateEmployeeID(String employeeID, String companyID, String userInfoID, String registrationID)
    {
        String methodResponse = String.Empty;

        try
        {
            if (!String.IsNullOrEmpty(employeeID) && !String.IsNullOrEmpty(companyID))
            {
                if (new CompanyEmployeeRepository().CheckEmployeeIDExistence(employeeID, Convert.ToInt64(companyID), !String.IsNullOrEmpty(userInfoID) ? Convert.ToInt64(userInfoID) : 0))
                {
                    if (new RegistrationRepository().CheckEmployeeIDExistence(employeeID, Convert.ToInt64(companyID), !String.IsNullOrEmpty(registrationID) ? Convert.ToInt64(registrationID) : 0))
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
}
