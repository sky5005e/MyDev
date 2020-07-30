/// <summary>
/// Page created by mayur on 23-dec-2011 for design changes and remove some extrafields
/// Also add functionality of schedular event
/// Add companyemail,Employee Type,fax and extension field on page
/// remove Third Party Employee fields, Manager Order Approval System fields, Order Notifications fields, Return/Exchange Notifications fields, Shipment Notifications fields
/// IsMOASApprover add by mayur on 30-july-2012
/// Convert html dropdown to asp.net dropdown by mayur on 30-August-2012
/// </summary>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Company_Employee_BasicInformation : PageBase
{
    #region Data Members

    LookupRepository objLookupRepos = new LookupRepository();
    CompanyRepository objCompanyRepos = new CompanyRepository();
    List<Company> objComplist = new List<Company>();
    EmailTemplateBE objEmailBE = new EmailTemplateBE();
    EmailTemplateDA objEmailDA = new EmailTemplateDA();
    CountryDA objCountry = new CountryDA();
    StateDA objState = new StateDA();
    CityDA objCity = new CityDA();
    Common objcomm = new Common();
    DataSet dsEmailTemplate;
    DataSet ds;

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

    Int64 EmployeeID
    {
        get
        {
            if (ViewState["EmployeeID"] == null)
            {
                ViewState["EmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["EmployeeID"]);
        }
        set
        {
            ViewState["EmployeeID"] = value;
        }
    }

    Int64 UserID
    {
        get
        {
            if (ViewState["UserID"] == null)
            {
                ViewState["UserID"] = 0;
            }
            return Convert.ToInt64(ViewState["UserID"]);
        }
        set
        {
            ViewState["UserID"] = value;
        }
    }

    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Employees";
            base.ParentMenuID = 11;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.ManageCompany;
            String SAPCompanyCode = String.Empty;

            if (Request.QueryString.Count > 0)
            {
                this.CompanyID = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.EmployeeID = Convert.ToInt64(Request.QueryString.Get("SubId"));

                //set three textboxes
                txtActivatedBy.Text = IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName;
                txtDateRequestSubmitted.Text = System.DateTime.Now.ToString("MM/dd/yyyy");

                menucontrol.PopulateMenu(2, 0, this.CompanyID, this.EmployeeID, true);
                Company objCompany = new CompanyRepository().GetById(this.CompanyID);

                if (objCompany == null)
                    Response.Redirect("~/admin/Company/Employee/ViewEmployee.aspx");

                ((Label)Master.FindControl("lblPageHeading")).Text = "Basic Information";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Company/Employee/ViewEmployee.aspx?id=" + this.CompanyID;

                // set previous page
                if (!String.IsNullOrEmpty(Request.QueryString.Get("PP")))
                {
                    //PP = previous page , EC = employee credit page
                    if (Request.QueryString.Get("PP") == "EC")
                        ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/EmployeeCredits.aspx?Id=" + this.CompanyID;
                }

                //set Search Page Return URL
                if (IncentexGlobal.SearchPageReturnURL != null)
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.SearchPageReturnURL;

                SAPCompanyCode = Convert.ToString(objCompany.SAPCompanyCode);
            }
            else
            {
                Response.Redirect("~/admin/Company/Employee/ViewEmployee.aspx");
            }

            FillCountry();
            FillEmployeeTitle();
            bindEmployyeType();
            FillEmployeeType();
            FillGender();
            FillDepartment();
            FillWorkgroup();
            FillRegion();
            FillEmployeeStatus();
            FillClimateSetting();
            DisplayData(SAPCompanyCode);
            bindGridView();
        }
    }

    #region Drop Down Events

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
                    }
                }

                FillBasedStationOnCountry(Convert.ToInt64(ddlCountry.SelectedItem.Value));
            }

            if (ddlCountry.SelectedValue == "0" && ddlState.SelectedValue == "0")
                ddlCity.Items.Remove(new ListItem("-Other-", "-Other-"));
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
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
                ddlCity.Items.Insert(1, "-Other-");
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
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
            tr_MOASStationLevelApprover.Visible = false;
        }
        else if (ddlEmployeeType.SelectedItem.Text == Incentex.DA.DAEnums.CompanyEmployeeTypes.Admin.ToString())
        {
            trSupplierCompanyName.Visible = false;
            trMOASPayment.Visible = true;
            tr_MOASStationLevelApprover.Visible = true;
        }
        else
        {
            trSupplierCompanyName.Visible = false;
            trMOASPayment.Visible = false;
            tr_MOASStationLevelApprover.Visible = false;
        }
    }

    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCity.SelectedValue == "-Other-")
            PnlCityOther.Visible = true;
        else
            PnlCityOther.Visible = false;
    }

    #endregion

    #region Datalist Events

    protected void dtlEvents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            switch (this.ViewState["SortExp"].ToString())
            {
                case "EventName":
                    PlaceHolder placeholderEventName = (PlaceHolder)e.Row.FindControl("placeholderEventName");
                    break;
                case "EventDate":
                    PlaceHolder placeholderEventDate = (PlaceHolder)e.Row.FindControl("placeholderEventDate");
                    break;
                case "EventTime":
                    PlaceHolder placeholderEventTimee = (PlaceHolder)e.Row.FindControl("placeholderEventTime");
                    break;
                case "EventReminder":
                    PlaceHolder placeholderEventReminder = (PlaceHolder)e.Row.FindControl("placeholderEventReminder");
                    break;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((Label)e.Row.FindControl("lblEventTime")).Text.ToString() != "")
                ((Label)e.Row.FindControl("lblEventTime")).Text = DateTime.Parse(((Label)e.Row.FindControl("lblEventTime")).Text.ToString()).ToString("hh:mm tt");
        }
    }

    protected void dtlEvents_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblEventMsg.Text = String.Empty;
        if (e.CommandName.Equals("Sort"))
        {
            if (this.ViewState["SortExp"] == null)
            {
                this.ViewState["SortExp"] = e.CommandArgument.ToString();
                this.ViewState["SortOrder"] = "ASC";
            }
            else
            {
                if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
                {
                    if (this.ViewState["SortOrder"].ToString() == "ASC")
                        this.ViewState["SortOrder"] = "DESC";
                    else
                        this.ViewState["SortOrder"] = "ASC";
                }
                else
                {
                    this.ViewState["SortOrder"] = "ASC";
                    this.ViewState["SortExp"] = e.CommandArgument.ToString();
                }
            }
        }
        else if (e.CommandName == "EditEvent")
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }
            Response.Redirect("~/admin/Company/Employee/ScheduledEvent.aspx?ID=" + this.CompanyID + "&SubID=" + this.EmployeeID + "&EventID=" + e.CommandArgument.ToString());
        }
        else if (e.CommandName == "DeleteEvent")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            new CompanyEmployeeEventsRepository().DeleteById(Convert.ToInt64(e.CommandArgument));
            lblEventMsg.Text = "Record deleted successfully...";
        }

        DisplayScheduledEvents();
    }

    #endregion

    #region Gridview Events

    protected void gvTemplatesList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            if (this.ViewState["SortOrder"].ToString() == "ASC")
                ImgSort.ImageUrl = "";
            else
                ImgSort.ImageUrl = "";
            switch (this.ViewState["SortExp"].ToString())
            {
                case "NoteContents":
                    PlaceHolder placeholderTempName = (PlaceHolder)e.Row.FindControl("placeholderTempName");
                    break;

                case "ViewTemp":
                    PlaceHolder placeholderViewTemp = (PlaceHolder)e.Row.FindControl("placeholderViewTemp");
                    break;
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Now Want to open Popup window from hyper link
            Label lblTempid = (Label)e.Row.FindControl("lblTempID");
            Int32 tid = 0;
            if (!String.IsNullOrEmpty(lblTempid.Text))
                tid = Convert.ToInt32(lblTempid.Text);
            else
                tid = 0;

            String urlWithParams = "../../CommunicationCenter/ViewTemplates.aspx?tid=" + tid + "&id=" + this.UserID;
            LinkButton hypViewTemp = (LinkButton)e.Row.FindControl("hypViewTemp");
            hypViewTemp.Attributes.Add("OnClick", "window.open('" + urlWithParams + "','PopupWindow','width=650,height=650,scrollbars=yes')");
        }
    }

    #endregion

    #region Link Button Events

    protected void lnkbtnAddEvent_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        Response.Redirect("~/admin/Company/Employee/ScheduledEvent.aspx?ID=" + this.CompanyID + "&SubID=" + this.EmployeeID);
    }

    protected void lnkApplychanges_Click(object sender, EventArgs e)
    {
        try
        {
            UserInformation objUserInfo = new UserInformation();
            UserInformationRepository objUserRepo = new UserInformationRepository();
            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
            if (this.EmployeeID != 0)
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                objCompanyEmployee = objEmpRepo.GetById(this.EmployeeID);
                objUserInfo = objUserRepo.GetById(objCompanyEmployee.UserInfoID);

                if (ViewState["LoginEmail"] != null)
                {
                    if (ViewState["LoginEmail"].ToString() != txtLoginEmail.Text)
                    {
                        if (!objUserRepo.CheckEmailExistence(txtLoginEmail.Text.Trim(), this.UserID))
                        {
                            lblMsg.Text = "Login Email already exist ...";
                            return;
                        }
                    }
                }
                else if (!objUserRepo.CheckEmailExistence(txtLoginEmail.Text.Trim(), this.UserID))
                {
                    lblMsg.Text = "Login Email already exist ...";
                    return;
                }

                objUserInfo.LoginEmail = txtLoginEmail.Text;
                objUserInfo.Password = txtPassword.Text;
                objUserInfo.WLSStatusId = Convert.ToInt32(ddlEmployeeStatus.SelectedValue);
                objUserRepo.SubmitChanges();
                ViewState["LoginEmail"] = txtLoginEmail.Text;
                INC_Lookup objLook = objLookupRepos.GetById(Convert.ToInt32(ddlEmployeeStatus.SelectedValue));
                if (objLook.sLookupName == "Active")
                {
                    //after Active mail sent to employee
                    sendApprovalEmail(txtLoginEmail.Text, txtPassword.Text, objUserInfo.FirstName + " " + objUserInfo.LastName);
                }
                else
                {
                    //after Inactive mail sent to employee
                    sendInApprovalEmail(txtLoginEmail.Text, objUserInfo.FirstName + " " + objUserInfo.LastName);
                }
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {

            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            String strAssignedWorkGroup = String.Empty;
            if (Convert.ToInt64(ddlEmployeeType.SelectedItem.Value) == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) && chkIsStaionLevelMOASApprover.Checked)
            {
                foreach (DataListItem item in dtWorkGroups.Items)
                {
                    CheckBox chkWorkGroups = (CheckBox)item.FindControl("chkWorkGroups");
                    if (chkWorkGroups != null && chkWorkGroups.Checked)
                    {
                        HiddenField hdnWorkGroupID = (HiddenField)item.FindControl("hdnWorkGroupID");
                        if (strAssignedWorkGroup != "")
                            strAssignedWorkGroup += ",";
                        strAssignedWorkGroup += Convert.ToString(hdnWorkGroupID.Value);
                    }
                }
                //if (String.IsNullOrEmpty(strAssignedWorkGroup))
                //{
                //    lblMsg.Text = "Please select atleast one workgroup for Station Level Approver";
                //    return;
                //}
                //else
                //    lblMsg.Text = "";
            }

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
            UserInformationRepository objUserRepo = new UserInformationRepository();
            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();

            if (this.EmployeeID != 0)
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                objCompanyEmployee = objEmpRepo.GetById(this.EmployeeID);
                objUserInfo = objUserRepo.GetById(objCompanyEmployee.UserInfoID);
            }
            else
            {
                if (!base.CanAdd)
                {
                    base.RedirectToUnauthorised();
                }
            }

            if (ViewState["LoginEmail"] != null)
            {
                if (ViewState["LoginEmail"].ToString() != txtLoginEmail.Text)
                {
                    if (!objUserRepo.CheckEmailExistence(txtLoginEmail.Text.Trim(), this.UserID))
                    {
                        lblMsg.Text = "Login Email already exist ...";
                        return;
                    }
                }
            }
            else if (!objUserRepo.CheckEmailExistence(txtLoginEmail.Text.Trim(), this.UserID))
            {
                lblMsg.Text = "Login Email already exist ...";
                return;
            }

            //check for employee id exist or not
            if (ViewState["EmployeeNumber"] != null)
            {
                if (ViewState["EmployeeNumber"].ToString() != txtEmployeeId.Text.Trim())
                {
                    if (!new CompanyEmployeeRepository().CheckEmployeeIDExistence(txtEmployeeId.Text.Trim(), this.CompanyID, this.UserID))
                    {
                        lblMsg.Text = "Employee # already exist ...";
                        return;
                    }
                    else if (!new RegistrationRepository().CheckEmployeeIDExistence(txtEmployeeId.Text.Trim(), this.CompanyID, this.UserID))
                    {
                        lblMsg.Text = "Employee # already exist in registration requests...";
                        return;
                    }
                }
            }
            else if (!new CompanyEmployeeRepository().CheckEmployeeIDExistence(txtEmployeeId.Text.Trim(), this.CompanyID, this.UserID))
            {
                lblMsg.Text = "Employee # already exist ...";
                return;
            }
            else if (!new RegistrationRepository().CheckEmployeeIDExistence(txtEmployeeId.Text.Trim(), this.CompanyID, this.UserID))
            {
                lblMsg.Text = "Employee # already exist in registration requests...";
                return;
            }

            objUserInfo.FirstName = txtFirstName.Text.Trim();
            objUserInfo.LastName = txtLastName.Text.Trim();
            objUserInfo.MiddleName = txtMiddleName.Text.Trim();
            objUserInfo.Address1 = txtAdress1.Text.Trim();
            objUserInfo.WLSStatusId = Convert.ToInt32(ddlEmployeeStatus.SelectedValue);
            objUserInfo.Address2 = txtAddress2.Text.Trim();
            objUserInfo.CountryId = Convert.ToInt64(ddlCountry.SelectedItem.Value);
            objUserInfo.StateId = Convert.ToInt64(ddlState.SelectedItem.Value);
            objUserInfo.CityId = CityID;
            objUserInfo.ZipCode = txtZip.Text.Trim();
            objUserInfo.Telephone = txtTelephone.Text.Trim();
            objUserInfo.Extension = txtExtension.Text.Trim();
            objUserInfo.Mobile = txtMobile.Text.Trim();
            objUserInfo.Fax = txtFax.Text.Trim();
            objUserInfo.Email = txtEmail.Text.Trim();
            objUserInfo.LoginEmail = txtLoginEmail.Text.Trim();
            objUserInfo.Password = txtPassword.Text.Trim();
            objUserInfo.CreatedDate = System.DateTime.Now;
            objUserInfo.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objUserInfo.CompanyId = this.CompanyID;
            objUserInfo.Usertype = Convert.ToInt64(ddlEmployeeType.SelectedItem.Value);

            if (this.EmployeeID == 0)
                objUserRepo.Insert(objUserInfo);
            else
                objUserInfo.UpdatedDate = DateTime.Now;

            if (ddlClimateSetting.SelectedIndex > 0)
                objUserInfo.ClimateSettingId = Convert.ToInt64(ddlClimateSetting.SelectedValue);
            else
                objUserInfo.ClimateSettingId = null;


            objUserRepo.SubmitChanges();


            //Delete if workgroup available on aditional workgroup table
            //create by mayur on 12-jan-2012
            if (this.EmployeeID != 0)
                new AdditionalWorkgroupRepository().DeleteByEmployeeIDCompanyIDAndWorkgroupID(this.EmployeeID, this.CompanyID, Convert.ToInt64(ddlWorkgroup.SelectedValue));

            //insert record in the userinfo table and then insert other details in company employee table
            if (this.EmployeeID != 0)
                objCompanyEmployee = objEmpRepo.GetById(this.EmployeeID);

            objCompanyEmployee.UserInfoID = objUserInfo.UserInfoID;
            objCompanyEmployee.CompanyEmail = txtCompanyEmail.Text;
            //

            if (ddlEmployeeType.SelectedItem.Text == Incentex.DA.DAEnums.CompanyEmployeeTypes.Admin.ToString())
            {
                objCompanyEmployee.isCompanyAdmin = true;
                objCompanyEmployee.IsMOASApprover = ddlMOASPayment.SelectedValue == "True" ? true : false;
                objCompanyEmployee.ManagementControlForWorkgroup = Convert.ToInt64(ddlWorkgroup.SelectedValue);
                objCompanyEmployee.ManagementControlForDepartment = Convert.ToInt64(ddlDepartment.SelectedValue);

                if (!String.IsNullOrEmpty(ddlRegion.SelectedValue) && Convert.ToInt64(ddlRegion.SelectedValue) > 0)
                    objCompanyEmployee.ManagementControlForRegion = Convert.ToInt64(ddlRegion.SelectedValue);
                else
                    objCompanyEmployee.ManagementControlForRegion = null;

                objCompanyEmployee.ManagementControlForStaionlocation = Convert.ToInt64(ddlBasestation.SelectedValue);

                //Remove MOAS as payment option
                if (this.EmployeeID != 0 && ddlMOASPayment.SelectedValue != "True")
                {
                    if (objCompanyEmployee.Paymentoption != null)
                    {
                        List<String> lstSelectedPaymentOptions = new List<String>(objCompanyEmployee.Paymentoption.Split(','));
                        String MOASID = new LookupRepository().GetIdByLookupNameNLookUpCode("MOAS", "WLS Payment Option").ToString();
                        for (Int32 i = 0; i < lstSelectedPaymentOptions.Count; i++)
                        {
                            if (MOASID == lstSelectedPaymentOptions[i])
                                lstSelectedPaymentOptions.RemoveAt(i);
                        }

                        objCompanyEmployee.Paymentoption = String.Join(",", lstSelectedPaymentOptions.ToArray());
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
                if (this.EmployeeID != 0)
                {
                    CompanyEmpMenuAccessRepository objCmpMenuAccesRepos = new CompanyEmpMenuAccessRepository();
                    List<CompanyEmpMenuAccess> lst = objCmpMenuAccesRepos.getMenuByUserType(this.EmployeeID, "Company Admin");

                    foreach (CompanyEmpMenuAccess l in lst)
                    {
                        objCmpMenuAccesRepos.Delete(l);
                        objCmpMenuAccesRepos.SubmitChanges();
                    }

                    //Remove Bulk Order feature for employee
                    if (objCompanyEmployee.Userstoreoption != null)
                    {
                        List<String> lstSelectedStoreOption = new List<String>(objCompanyEmployee.Userstoreoption.Split(','));
                        String BulkOrderID = new LookupRepository().GetIdByLookupNameNLookUpCode("Bulk Order", "UserStoreOptions").ToString();
                        String NameToEngraveID = new LookupRepository().GetIdByLookupNameNLookUpCode("Name to Engrave", "UserStoreOptions").ToString();

                        for (Int32 i = 0; i < lstSelectedStoreOption.Count; i++)
                        {
                            if (BulkOrderID == lstSelectedStoreOption[i] || NameToEngraveID == lstSelectedStoreOption[i])
                            {
                                lstSelectedStoreOption.RemoveAt(i);
                                i--;
                            }
                        }

                        objCompanyEmployee.Userstoreoption = String.Join(",", lstSelectedStoreOption.ToArray());
                    }
                }
            }

            //
            if (hdPriPhoto.Value != "")
                objCompanyEmployee.UploadImage = hdPriPhoto.Value;
            else
                objCompanyEmployee.UploadImage = null;

            if (!String.IsNullOrEmpty(txtDateOfHired.Text))
                objCompanyEmployee.HirerdDate = Convert.ToDateTime(txtDateOfHired.Text);

            objCompanyEmployee.EmployeeID = txtEmployeeId.Text;

            if (!String.IsNullOrEmpty(ddlRegion.SelectedValue) && Convert.ToInt64(ddlRegion.SelectedValue) > 0)
                objCompanyEmployee.RegionID = Convert.ToInt64(ddlRegion.SelectedValue);
            else
                objCompanyEmployee.RegionID = null;

            objCompanyEmployee.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);

            if (ddlBasestation.SelectedIndex == 0)
                objCompanyEmployee.BaseStation = null;
            else
                objCompanyEmployee.BaseStation = Convert.ToInt64(ddlBasestation.SelectedValue);

            objCompanyEmployee.GenderID = Convert.ToInt64(ddlGender.SelectedValue);
            objCompanyEmployee.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            objCompanyEmployee.StoreActivatedBy = txtActivatedBy.Text;
            objCompanyEmployee.StoreActivatedDate = Convert.ToDateTime(txtDateActivated.Text);
            objCompanyEmployee.DateRequestSubmitted = txtDateRequestSubmitted.Text;

            //added by amit on 14-Dec-2010

            LookupRepository objLookupRepository = new LookupRepository();
            INC_Lookup objLookUp = objLookupRepository.GetById(Convert.ToInt64(ddlEmployeeStatus.SelectedValue));

            if (objLookUp != null)
            {
                if (objLookUp.sLookupName.Equals("Active"))
                {
                    objCompanyEmployee.LastActiveDate = DateTime.Now;
                }
                else
                {
                    objCompanyEmployee.LastInActiveDate = DateTime.Now;
                }
            }

            //Newly Added on 15 feb by Ankit
            if (ddlTitle.SelectedIndex != 0)
            {
                objCompanyEmployee.EmployeeTitleId = Convert.ToInt64(ddlTitle.SelectedItem.Value);
            }
            else
            {
                objCompanyEmployee.EmployeeTitleId = null;
            }
            //End

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

            //added by Prashant april 2013
            if (Convert.ToInt64(ddlEmployeeType.SelectedItem.Value) == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) && chkIsStaionLevelMOASApprover.Checked)
            {
                objCompanyEmployee.IsMOASStationLevelApprover = chkIsStaionLevelMOASApprover.Checked;
                //Insert data into the database
                if (chkIsStaionLevelMOASApprover.Checked && strAssignedWorkGroup != hdnAssigedWorkGroup.Value)
                {
                    objUserRepo.InsertWorkGroupAcessForUser(this.CompanyID, objUserInfo.UserInfoID, IncentexGlobal.CurrentMember.UserInfoID, strAssignedWorkGroup);
                }
                else if (!chkIsStaionLevelMOASApprover.Checked)
                {
                    List<MOASApproverWorkGroupAccess> objWorkGroupAccessList = objUserRepo.GetWorkGroupAccessByUserInfoID(objUserInfo.UserInfoID);
                    objUserRepo.DeleteAll(objWorkGroupAccessList);
                    objUserRepo.SubmitChanges();
                }
            }
            else
                objCompanyEmployee.IsMOASStationLevelApprover = null;
            //added by Prashant april 2013

            if (this.EmployeeID == 0)
                objEmpRepo.Insert(objCompanyEmployee);

            objEmpRepo.SubmitChanges();

            #region anniversary region

            //Newly added on 26 aug 2011
            //Now Update the Anniversary Credits
            if (this.EmployeeID != 0)
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

                        objEmpRepo.SubmitChanges();
                    }
                    //End
                }
            }

            #endregion

            this.EmployeeID = objCompanyEmployee.CompanyEmployeeID;
            Response.Redirect("SpecialProgram.aspx?Id=" + this.CompanyID + "&SubId=" + this.EmployeeID);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        modalAddnotes.Show();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(txtNote.Text))
        {
            return;
        }
        try
        {
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            NoteDetail obj = new NoteDetail()
            {
                ForeignKey = this.EmployeeID,
                Notecontents = txtNote.Text,
                NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CompanyEmployee),
                CreatedBy = IncentexGlobal.CurrentMember.UserInfoID,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID
            };

            objRepo.Insert(obj);
            objRepo.SubmitChanges();
            DisplayNotes();
            txtNote.Text = String.Empty;
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #endregion

    #region Methods

    #region Bind Dropdown Lists

    private void FillEmployeeTitle()
    {
        ddlTitle.DataSource = objLookupRepos.GetByLookup("EmployeeTitle ");
        ddlTitle.DataValueField = "iLookupID";
        ddlTitle.DataTextField = "sLookupName";
        ddlTitle.DataBind();
        ddlTitle.Items.Insert(0, new ListItem("-select title-", "0"));
    }

    private void FillEmployeeType()
    {
        ddlEmployeeTypeLast.DataSource = objLookupRepos.GetByLookup("EmployeeType ");
        ddlEmployeeTypeLast.DataValueField = "iLookupID";
        ddlEmployeeTypeLast.DataTextField = "sLookupName";
        ddlEmployeeTypeLast.DataBind();
        ddlEmployeeTypeLast.Items.Insert(0, new ListItem("-select employee type-", "0"));
    }

    private void FillGender()
    {
        ddlGender.DataSource = objLookupRepos.GetByLookup("Gender");
        ddlGender.DataValueField = "iLookupID";
        ddlGender.DataTextField = "sLookupName";
        ddlGender.DataBind();
        ddlGender.Items.Insert(0, new ListItem("-select gender-", "0"));
    }

    private void FillDepartment()
    {
        ddlDepartment.DataSource = objLookupRepos.GetByLookup("Department");
        ddlDepartment.DataValueField = "iLookupID";
        ddlDepartment.DataTextField = "sLookupName";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-select department-", "0"));
    }

    private void FillWorkgroup()
    {
        var objWorkGroupList = objLookupRepos.GetByLookup("Workgroup");
        ddlWorkgroup.DataSource = objWorkGroupList;
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-select workgroup-", "0"));

    }

    private void FillRegion()
    {
        ddlRegion.DataSource = objLookupRepos.GetByLookup("Region");
        ddlRegion.DataValueField = "iLookupID";
        ddlRegion.DataTextField = "sLookupName";
        ddlRegion.DataBind();
        ddlRegion.Items.Insert(0, new ListItem("-select region-", "0"));
    }

    private void FillEmployeeStatus()
    {
        ddlEmployeeStatus.DataSource = objLookupRepos.GetByLookup("Status");
        ddlEmployeeStatus.DataValueField = "iLookupID";
        ddlEmployeeStatus.DataTextField = "sLookupName";
        ddlEmployeeStatus.DataBind();
        ddlEmployeeStatus.Items.Insert(0, new ListItem("-select employee status-", "0"));
    }

    private void FillCountry()
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
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
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

    private void FillClimateSetting()
    {
        ddlClimateSetting.DataSource = objLookupRepos.GetByLookup("ClimateSetting");
        ddlClimateSetting.DataValueField = "iLookupID";
        ddlClimateSetting.DataTextField = "sLookupName";
        ddlClimateSetting.DataBind();
        ddlClimateSetting.Items.Insert(0, new ListItem("-Select Climate-", "0"));
    }

    private void bindEmployyeType()
    {
        Dictionary<Int32, String> ht = Common.GetEnumForBind(typeof(Incentex.DA.DAEnums.CompanyEmployeeTypes));

        ddlEmployeeType.DataSource = ht;
        ddlEmployeeType.DataTextField = "value";
        ddlEmployeeType.DataValueField = "key";
        ddlEmployeeType.DataBind();
        ddlEmployeeType.Items.Insert(0, new ListItem("-select-", "0"));
    }

    private void FillBasedStationOnCountry(Int64 countryid)
    {
        BaseStationDA sBaseStation = new BaseStationDA();
        BasedStationBE sBSBe = new BasedStationBE();
        ddlBasestation.Items.Clear();
        sBSBe.SOperation = "getBaseStationbyCounty";
        sBSBe.iCountryID = countryid;
        String sOptions = String.Empty;
        DataSet dsBaseStation = sBaseStation.GetBaseStaionByCountry(sBSBe);
        if (dsBaseStation.Tables.Count > 0 && dsBaseStation.Tables[0].Rows.Count > 0)
        {
            ddlBasestation.DataSource = dsBaseStation.Tables[0];
            ddlBasestation.DataValueField = "iBaseStationId";
            ddlBasestation.DataTextField = "sBaseStation";
            ddlBasestation.DataBind();
            ddlBasestation.Items.Insert(0, new ListItem("-select basestation-", "0"));
        }
        else
            ddlBasestation.Items.Insert(0, new ListItem("-select basestation-", "0"));
    }

    #endregion

    private void DisplayData(String SAPCompanyCode)
    {
        hdnCompanyID.Value = Convert.ToString(this.CompanyID);

        if (this.EmployeeID != 0)
        {
            lnkApplychanges.Visible = true;
            lnkbtnAddEvent.Visible = true;
            CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetById(this.EmployeeID);

            if (objCompanyEmployee == null)
                return;

            //Get and set data from userinformation table
            UserInformation objUserInfo = new UserInformationRepository().GetById(objCompanyEmployee.UserInfoID);
            hdnUserInfoID.Value = Convert.ToString(objUserInfo.UserInfoID);
            hdnRegistrationID.Value = Convert.ToString(objUserInfo.RegistrationID);

            #region AnniversaryDate

            //Set Anniversary Date Added on 27 Jan 2011
            AnniversaryProgramRepository objCEProgram = new AnniversaryProgramRepository();
            SelectAnniversaryCreditProgramPerEmployeeResult objCE = objCEProgram.GetCompanyEmployeeAnniversaryCreditDetails(objCompanyEmployee.UserInfoID);

            if (Convert.ToDateTime(objCE.NewHiredDate).ToString("MM/dd/yyyy") != "01/01/0001")
                txtAnniversaryDate.Text = Convert.ToDateTime(objCE.NewHiredDate).ToString("MM/dd/yyyy");
            else
                txtAnniversaryDate.Text = "";
            //End

            #endregion

            txtFirstName.Text = objUserInfo.FirstName;
            txtLastName.Text = objUserInfo.LastName;
            txtMiddleName.Text = objUserInfo.MiddleName;
            txtAdress1.Text = objUserInfo.Address1;
            txtAddress2.Text = objUserInfo.Address2;
            ddlEmployeeStatus.SelectedValue = objUserInfo.WLSStatusId != null ? objUserInfo.WLSStatusId.ToString() : "0";

            ddlCountry.SelectedValue = objUserInfo.CountryId.ToString();
            ddlCountry_SelectedIndexChanged(null, null);

            ddlState.SelectedValue = objUserInfo.StateId.ToString();
            ddlState_SelectedIndexChanged(null, null);
            ddlCity.SelectedValue = objUserInfo.CityId.ToString();

            if (objCompanyEmployee.EmployeeTitleId.ToString() != String.Empty)
                ddlTitle.Items.FindByValue(objCompanyEmployee.EmployeeTitleId.ToString()).Selected = true;

            if (objCompanyEmployee.EmployeeTypeID != null)
                ddlEmployeeTypeLast.Items.FindByValue(objCompanyEmployee.EmployeeTypeID.ToString()).Selected = true;

            ddlGender.SelectedValue = objCompanyEmployee.GenderID != null ? objCompanyEmployee.GenderID.ToString() : "0";
            ddlDepartment.SelectedValue = objCompanyEmployee.DepartmentID != null ? objCompanyEmployee.DepartmentID.ToString() : "0";
            ddlWorkgroup.SelectedValue = objCompanyEmployee.WorkgroupID != null ? objCompanyEmployee.WorkgroupID.ToString() : "0";
            ddlBasestation.SelectedValue = objCompanyEmployee.BaseStation != null ? objCompanyEmployee.BaseStation.ToString() : "0";
            ddlRegion.SelectedValue = objCompanyEmployee.RegionID != null ? objCompanyEmployee.RegionID.ToString() : "0";

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
                tr_MOASStationLevelApprover.Visible = true;
            }
            else
            {
                //add by mayur for third party supplier employee on 10-may-2012
                trMOASPayment.Visible = false;
                tr_MOASStationLevelApprover.Visible = false;
                if (objUserInfo.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
                {
                    ddlEmployeeType.Items.FindByText(Incentex.DA.DAEnums.CompanyEmployeeTypes.XSE.ToString()).Selected = true;
                    trSupplierCompanyName.Visible = true;
                    txtSupplierCompanyName.Text = objCompanyEmployee.ThirdPartySupplierCompanyName;
                }
                else
                    ddlEmployeeType.Items.FindByText(Incentex.DA.DAEnums.CompanyEmployeeTypes.Employee.ToString()).Selected = true;
            }

            if (Convert.ToDateTime(objCompanyEmployee.HirerdDate).ToString("MM/dd/yyyy") != "01/01/0001")
                ViewState["DOH"] = txtDateOfHired.Text = Convert.ToDateTime(objCompanyEmployee.HirerdDate).ToString("MM/dd/yyyy");
            else
                txtDateOfHired.Text = "";

            txtEmployeeId.Text = objCompanyEmployee.EmployeeID;
            ViewState["EmployeeNumber"] = objCompanyEmployee.EmployeeID;

            hdPriPhoto.Value = objCompanyEmployee.UploadImage;
            txtActivatedBy.Text = objCompanyEmployee.StoreActivatedBy;

            if (objUserInfo.CreatedDate != null)
                txtDateActivated.Text = Convert.ToDateTime(objUserInfo.CreatedDate).ToString("MM/dd/yyyy");

            ddlMOASPayment.SelectedValue = objCompanyEmployee.IsMOASApprover.ToString();

            if (txtDateRequestSubmitted.Text != "")
                txtDateRequestSubmitted.Text = objCompanyEmployee.DateRequestSubmitted;
            else
                txtDateRequestSubmitted.Text = System.DateTime.Now.ToString("MM/dd/yyyy");

            if (objUserInfo.ClimateSettingId.HasValue)
                ddlClimateSetting.SelectedValue = objUserInfo.ClimateSettingId.Value.ToString();
            else
                ddlClimateSetting.SelectedValue = null;

            txtWorldLinkContactID.Text = (!String.IsNullOrEmpty(SAPCompanyCode) ? SAPCompanyCode : "No_Code") + "_" + objUserInfo.UserInfoID;
            txtWorldLinkContactID.ToolTip = String.IsNullOrEmpty(SAPCompanyCode) ? "No SAP Customer Code is set for this store" : null;

            chkIsStaionLevelMOASApprover.Checked = objCompanyEmployee.IsMOASStationLevelApprover != null ? Convert.ToBoolean(objCompanyEmployee.IsMOASStationLevelApprover) : false;
            //if(chkIsStaionLevelMOASApprover.Checked)
            //    Span_MoasAprover.Attributes.Add("class", "custom-checkbox_checked alignleft workgroupapprover");
            //else
            //    Span_MoasAprover.Attributes.Add("class", "custom-checkbox alignleft workgroupapprover");

            DisplayScheduledEvents();
            DisplayNotes();
            BindMOASWorkGroupAccessList(objUserInfo.UserInfoID);
        }
    }

    protected void BindMOASWorkGroupAccessList(Int64 UserInfoID)
    {
        UserInformationRepository objUserInfoRepo = new UserInformationRepository();
        List<GetMOASWorkGroupAccessByUserInfoIDResult> objResult = objUserInfoRepo.SelectMOASWOrkGroupAccessByUserInfoID(UserInfoID);
        dtWorkGroups.DataSource = objResult;
        dtWorkGroups.DataBind();
        hdnAssigedWorkGroup.Value = String.Join(",", objResult.Where(x => x.IsAssigned == 1).Select(x => x.iLookupID.ToString()).ToArray());
    }

    protected void DisplayScheduledEvents()
    {
        DataView myDataView = new DataView();
        PagedDataSource pds = new PagedDataSource();
        CompanyEmployeeEventsRepository objCompanyEmployeeEventsRep = new CompanyEmployeeEventsRepository();
        List<CompanyEmployeeEvent> objCompanyEmployeeEvents = objCompanyEmployeeEventsRep.GetActiveEventByCompanyEmployeeId(this.EmployeeID);
        DataTable dataTable = ListToDataTable(objCompanyEmployeeEvents);
        myDataView = dataTable.DefaultView;

        if (this.ViewState["SortExp"] != null)
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();

        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        dtlEvents.DataSource = pds;
        dtlEvents.DataBind();
    }

    private static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();

        foreach (var info in typeof(T).GetProperties())
            dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));

        foreach (var t in list)
        {
            var row = dt.NewRow();

            foreach (var info in typeof(T).GetProperties())
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;

            dt.Rows.Add(row);
        }

        return dt;
    }

    private void DisplayNotes()
    {
        //UserinfoID from Company Employee Table
        CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetById(this.EmployeeID);
        if (objCompanyEmployee == null)
            return;

        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        List<NoteDetail> objList = objRepo.GetByForeignKeyId(objCompanyEmployee.UserInfoID, Incentex.DAL.Common.DAEnums.NoteForType.CompanyEmployee);
        txtNoteHistory.Text = String.Empty;

        foreach (NoteDetail obj in objList)
        {
            txtNoteHistory.Text += "Created Date : " + Convert.ToDateTime(obj.CreateDate).ToString("dd-MMM-yyyy") + "\n";
            txtNoteHistory.Text += "Created Time : " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";

            UserInformationRepository objUserRepo = new UserInformationRepository();
            UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);

            if (objUser != null)
                txtNoteHistory.Text += "Created By : " + objUser.FirstName + " " + objUser.LastName + "\n";

            txtNoteHistory.Text += "Note : " + obj.Notecontents + "\n";
            txtNoteHistory.Text += "---------------------------------------------------------------------------\n";
        }
    }

    private Boolean checkifhireddateischanged()
    {
        if (ViewState["DOH"] != null)
        {
            if (ViewState["DOH"].ToString() != txtDateOfHired.Text)
                return true;
            else
                return false;
        }

        return false;
    }

    /// <summary>
    /// This is for bind compain letter attachment gridview
    /// </summary>
    private void bindGridView()
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        UserID = objCompanyRepos.GetUserInfoIDByEmpId(this.EmployeeID);
        var objList = objRepo.GetCampaignDetailsByEmpID(UserID);

        if (objList.Count > 0)
        {
            gvTemplatesList.DataSource = objList;
            gvTemplatesList.DataBind();
        }
        else
            dvCampaign.Visible = false;
    }

    #region Send email method

    private void sendInApprovalEmail(String psEmailAddress, String psUserName)
    {
        try
        {
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Employee Inactivation";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = psEmailAddress;
                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{fullname}", psUserName);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(this.UserID, "Basic Information", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendApprovalEmail(String psEmailAddress, String psPassword, String psUserName)
    {
        try
        {
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "employee activation";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = psEmailAddress;
                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{fullname}", psUserName);
                messagebody.Replace("{password}", psPassword);
                messagebody.Replace("{email}", psEmailAddress);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                objComplist = objCompanyRepos.GetByCompanyId(this.CompanyID);

                if (objComplist.Count > 0)
                    messagebody.Replace("{storename}", objComplist[0].CompanyName);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(this.UserID, "Basic Information", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
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

    #endregion
}