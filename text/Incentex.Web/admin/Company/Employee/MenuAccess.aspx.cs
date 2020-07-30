using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Company_Employee_MenuAccess : PageBase
{
    #region Data Members

    IncentexBEDataContext db;

    LookupDA sEU = new LookupDA();
    LookupBE sEUBE = new LookupBE();

    SubCatogeryRepository objSubCatogeryRepository = new SubCatogeryRepository();

    String selectedpreferences = "";
    String selectedemployeeuniform = "";
    String seelctedadditonalinfo = "";
    String selectedcompanystore = "";
    String selecteduniform = "";
    String selectedsupplies = "";
    String selectedBasedStations = "";
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

            if (Request.QueryString.Count > 0)
            {
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("SubId"));

                if (this.EmployeeId == 0)
                {
                    Response.Redirect("~/admin/Company/Employee/BasicInformation.aspx?Id=" + this.CompanyId);
                }

                menucontrol.PopulateMenu(2, 2, this.CompanyId, this.EmployeeId, true);

                ((Label)Master.FindControl("lblPageHeading")).Text = "Menu Access";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Company/Employee/ViewEmployee.aspx?id=" + this.CompanyId;

                //set Search Page Return URL
                if (IncentexGlobal.SearchPageReturnURL != null)
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.SearchPageReturnURL;
                }
            }
            else
            {
                Response.Redirect("~/admin/Company/Employee/ViewEmployee.aspx");
            }

            BindData();
            DisplayData(sender, e);
            SetPreferences();
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        Boolean IsError = false;

        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            //Delete from EmployeeMenuaccessfirst
            CompanyEmpMenuAccessRepository objCmpMenuAccesRepos = new CompanyEmpMenuAccessRepository();
            CompanyEmpMenuAccess objCmpMenuAccessfordel = new CompanyEmpMenuAccess();

            List<CompanyEmpMenuAccess> lst = objCmpMenuAccesRepos.getMenuByUserType(EmployeeId, "Company Admin,Company Employee");

            foreach (CompanyEmpMenuAccess l in lst)
            {
                objCmpMenuAccesRepos.Delete(l);
                objCmpMenuAccesRepos.SubmitChanges();
            }

            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
            LookupRepository objreposlook = new LookupRepository();
            INC_Lookup objlookid = new INC_Lookup();

            //Menu Access
            Int64 lookupid = 0;
            foreach (DataListItem dt in dtlMenus.Items)
            {
                if (((CheckBox)dt.FindControl("chkdtlMenus")).Checked == true)
                {
                    CompanyEmpMenuAccessRepository objCmpMenuAccesRep = new CompanyEmpMenuAccessRepository();
                    CompanyEmpMenuAccess objCmpMenuAccess = new CompanyEmpMenuAccess();
                    objCmpMenuAccess.MenuPrivilegeID = Convert.ToInt64(((HiddenField)dt.FindControl("hdnMenuAccess")).Value);
                    objCmpMenuAccess.CompanyEmployeeID = EmployeeId;
                    objCmpMenuAccesRep.Insert(objCmpMenuAccess);
                    objCmpMenuAccesRep.SubmitChanges();

                    //Add Nagmani For Special Program Active and Inactive when checked the issuance polciy 
                    Label lblMenus = (Label)dt.FindControl("lblMenus");

                    if (lblMenus.Text.Trim() == "My Issuance Policy")
                        lookupid = Convert.ToInt64(objreposlook.GetIdByLookupNameNLookUpCode("Active", "Status"));
                    //End
                }
                else
                {
                    //Add Nagmani For Special Program Active and Inactive when checked the issuance polciy 
                    Label lblMenus = (Label)dt.FindControl("lblMenus");

                    if (lblMenus.Text.Trim() == "My Issuance Policy")
                        lookupid = Convert.ToInt64(objreposlook.GetIdByLookupNameNLookUpCode("InActive", "Status"));
                    //End
                }
            }

            objCompanyEmployee = objEmpRepo.GetById(EmployeeId);

            //User Preferences
            foreach (DataListItem dt in dtlPreferences.Items)
            {
                if (((CheckBox)dt.FindControl("chkPreferences")).Checked == true)
                {
                    if (selectedpreferences == "")
                        selectedpreferences = ((HiddenField)dt.FindControl("hdnPreferences")).Value;
                    else
                        selectedpreferences = selectedpreferences + "," + ((HiddenField)dt.FindControl("hdnPreferences")).Value;
                }
            }
            objCompanyEmployee.Preferences = selectedpreferences;
            objEmpRepo.SubmitChanges();

            //Employee Uniform
            foreach (DataListItem dtEmp in dtEmpUniform.Items)
            {
                if (((CheckBox)dtEmp.FindControl("chkEmpUniform")).Checked == true)
                {
                    if (selectedemployeeuniform == "")
                        selectedemployeeuniform = ((HiddenField)dtEmp.FindControl("hdnEmpUni")).Value;
                    else
                        selectedemployeeuniform = selectedemployeeuniform + "," + ((HiddenField)dtEmp.FindControl("hdnEmpUni")).Value;
                }
            }

            objCompanyEmployee.EmpIssuancePolicyStatus = lookupid;
            objCompanyEmployee.EmployeeUniformAccess = selectedemployeeuniform;
            objEmpRepo.SubmitChanges();

            //addition info
            foreach (DataListItem dt in dtAddiInfo.Items)
            {
                if (((CheckBox)dt.FindControl("chkAddiInfo")).Checked == true)
                {
                    if (seelctedadditonalinfo == "")
                        seelctedadditonalinfo = ((HiddenField)dt.FindControl("hdnAddiInfo")).Value;
                    else
                        seelctedadditonalinfo = seelctedadditonalinfo + "," + ((HiddenField)dt.FindControl("hdnAddiInfo")).Value;
                }
            }

            objCompanyEmployee.AdditionInfoAccess = seelctedadditonalinfo;
            objEmpRepo.SubmitChanges();

            //Company store
            foreach (DataListItem dt in dtCompanyStore.Items)
            {
                if (((CheckBox)dt.FindControl("chkCompanyStore")).Checked == true)
                {
                    if (selectedcompanystore == "")
                        selectedcompanystore = ((HiddenField)dt.FindControl("hdnCompanyStore")).Value;
                    else
                        selectedcompanystore = selectedcompanystore + "," + ((HiddenField)dt.FindControl("hdnCompanyStore")).Value;
                }
            }

            objCompanyEmployee.CompanyStoreAccess = selectedcompanystore;
            objEmpRepo.SubmitChanges();

            //Uniform Purchasing
            foreach (DataListItem dt in dtUniPurchasing.Items)
            {
                if (((CheckBox)dt.FindControl("chkUniPurchasing")).Checked == true)
                {
                    if (selecteduniform == "")
                        selecteduniform = ((HiddenField)dt.FindControl("hdnUniformPurchasing")).Value;
                    else
                        selecteduniform = selecteduniform + "," + ((HiddenField)dt.FindControl("hdnUniformPurchasing")).Value;
                }
            }

            objCompanyEmployee.UniformPurchasingAccess = selecteduniform;
            objEmpRepo.SubmitChanges();

            //Supplies
            foreach (DataListItem dts in dtSupplies.Items)
            {
                if (((CheckBox)dts.FindControl("chkSupplies")).Checked == true)
                {
                    if (selectedsupplies == "")
                        selectedsupplies = ((HiddenField)dts.FindControl("hdnSupplies")).Value;
                    else
                        selectedsupplies = selectedsupplies + "," + ((HiddenField)dts.FindControl("hdnSupplies")).Value;
                }
            }

            objCompanyEmployee.SuppliesAccess = selectedsupplies;
            objEmpRepo.SubmitChanges();

            //Base stations
            foreach (DataListItem dt in dtBaseStations.Items)
            {
                if (((CheckBox)dt.FindControl("chkBaseStationsName")).Checked == true)
                {
                    if (selectedBasedStations == "")
                        selectedBasedStations = ((HiddenField)dt.FindControl("hdnBaseStationID")).Value;
                    else
                        selectedBasedStations = selectedBasedStations + "," + ((HiddenField)dt.FindControl("hdnBaseStationID")).Value;
                }
            }

            objCompanyEmployee.BaseStationsAccess = selectedBasedStations;
            objEmpRepo.SubmitChanges();


            //Other Features
            objCompanyEmployee.DisplayTotalOrderAmount = chkDispTotalOrderAmount.Checked;
            objEmpRepo.SubmitChanges();

            db = new IncentexBEDataContext();
            CompanyEmployee objCE = db.CompanyEmployees.FirstOrDefault(le => le.CompanyEmployeeID == this.EmployeeId);
            List<UserPreference> lstPrevPref = db.UserPreferences.Where(le => le.UserInfoID == objCE.UserInfoID).ToList();

            if (lstPrevPref != null && lstPrevPref.Count > 0)
            {
                db.UserPreferences.DeleteAllOnSubmit(lstPrevPref);
                db.SubmitChanges();
            }

            foreach (DataListItem repPref in dlPreferences.Items)
            {
                HiddenField hdnKeyID = (HiddenField)repPref.FindControl("hdnKeyID");
                HiddenField hdnValueID = (HiddenField)repPref.FindControl("hdnValueID");
                DropDownList ddlPreference = (DropDownList)repPref.FindControl("ddlPreference");

                if (!String.IsNullOrEmpty(hdnKeyID.Value) && !String.IsNullOrEmpty(hdnValueID.Value) && !String.IsNullOrEmpty(ddlPreference.SelectedValue))
                {
                    UserPreference objNewPref = new UserPreference();
                    objNewPref.PreferenceID = Convert.ToInt64(hdnKeyID.Value);
                    objNewPref.PreferenceValueID = Convert.ToInt64(ddlPreference.SelectedValue);
                    objNewPref.UserInfoID = objCE.UserInfoID;
                    db.UserPreferences.InsertOnSubmit(objNewPref);
                }
            }

            db.SubmitChanges();
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
            IsError = true;
        }

        if (!IsError)
            Response.Redirect("MenuAccess.aspx?Id=" + this.CompanyId + "&SubId=" + this.EmployeeId);
    }

    protected void dlPreferences_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HiddenField hdnKeyID = (HiddenField)e.Item.FindControl("hdnKeyID");
            HiddenField hdnValueID = (HiddenField)e.Item.FindControl("hdnValueID");
            DropDownList ddlPreference = (DropDownList)e.Item.FindControl("ddlPreference");

            if (!String.IsNullOrEmpty(hdnKeyID.Value))
            {
                Int64 PreferenceID = Convert.ToInt64(hdnKeyID.Value);
                db = new IncentexBEDataContext();
                List<PreferenceValue> lstPreference = db.PreferenceValues.Where(le => le.PreferenceID == PreferenceID).ToList();
                ddlPreference.DataSource = lstPreference;
                ddlPreference.DataValueField = "PreferenceValueID";
                ddlPreference.DataTextField = "Display";
                ddlPreference.DataBind();

                if (!String.IsNullOrEmpty(hdnValueID.Value))
                    ddlPreference.SelectedValue = hdnValueID.Value;
            }
        }
    }

    #endregion

    #region Methods

    void DisplayData(object sender, EventArgs e)
    {
        CheckBox chk;
        HiddenField lblId;
        if (this.EmployeeId != 0)
        {
            CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetById(this.EmployeeId);
            if (objCompanyEmployee == null)
            {
                return;
            }

            UserInformation objUserInfo = new UserInformationRepository().GetById(objCompanyEmployee.UserInfoID);
            lblUserFullName.Text = objUserInfo.FirstName + " " + objUserInfo.LastName;

            //Get menu from empmenuaccess table
            List<CompanyEmpMenuAccess> lstMenuAccess = new CompanyEmpMenuAccessRepository().GetMenusByEmployeeId(this.EmployeeId);

            foreach (DataListItem dtM in dtlMenus.Items)
            {
                chk = dtM.FindControl("chkdtlMenus") as CheckBox;
                lblId = dtM.FindControl("hdnMenuAccess") as HiddenField;
                HtmlGenericControl dvChk = dtM.FindControl("menuspan") as HtmlGenericControl;

                foreach (CompanyEmpMenuAccess objMenu in lstMenuAccess)
                {
                    if (objMenu.MenuPrivilegeID.ToString().Equals(lblId.Value))
                    {
                        chk.Checked = true;
                        dvChk.Attributes.Add("class", "custom-checkbox_checked");
                        break;
                    }
                }
            }

            //Get data from companyemployee table
            if (objCompanyEmployee == null)
            {
                return;
            }

            //User Preferences
            if (objCompanyEmployee.Preferences != null)
            {
                String[] Ids = objCompanyEmployee.Preferences.Split(',');

                foreach (DataListItem dt in dtlPreferences.Items)
                {
                    chk = dt.FindControl("chkPreferences") as CheckBox;
                    lblId = dt.FindControl("hdnPreferences") as HiddenField;
                    HtmlGenericControl dvChk = dt.FindControl("spanPreferences") as HtmlGenericControl;

                    foreach (String i in Ids)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }
                    }
                }
            }

            //Employee Uniform
            if (objCompanyEmployee.EmployeeUniformAccess != null)
            {
                String[] Ids = objCompanyEmployee.EmployeeUniformAccess.Split(',');

                foreach (DataListItem dt in dtEmpUniform.Items)
                {
                    chk = dt.FindControl("chkEmpUniform") as CheckBox;
                    lblId = dt.FindControl("hdnEmpUni") as HiddenField;
                    HtmlGenericControl dvChk = dt.FindControl("menuemployeeuni") as HtmlGenericControl;

                    foreach (String i in Ids)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }
                    }
                }
            }

            // Additional Information
            if (objCompanyEmployee.AdditionInfoAccess != null)
            {
                String[] additioninfoIds = objCompanyEmployee.AdditionInfoAccess.Split(',');

                foreach (DataListItem dtaddiinfo in dtAddiInfo.Items)
                {
                    chk = dtaddiinfo.FindControl("chkAddiInfo") as CheckBox;
                    lblId = dtaddiinfo.FindControl("hdnAddiInfo") as HiddenField;
                    HtmlGenericControl dvChk = dtaddiinfo.FindControl("addinfospan") as HtmlGenericControl;

                    foreach (String i in additioninfoIds)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }
                    }
                }
            }

            // Company Store
            if (objCompanyEmployee.CompanyStoreAccess != null)
            {
                String[] companystoreIds = objCompanyEmployee.CompanyStoreAccess.Split(',');

                foreach (DataListItem dtcmpnystore in dtCompanyStore.Items)
                {
                    chk = dtcmpnystore.FindControl("chkCompanyStore") as CheckBox;
                    lblId = dtcmpnystore.FindControl("hdnCompanyStore") as HiddenField;
                    HtmlGenericControl dvChk = dtcmpnystore.FindControl("companystorespan") as HtmlGenericControl;

                    foreach (String i in companystoreIds)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }
                    }
                }
            }

            // Uniform Purchasing
            if (objCompanyEmployee.UniformPurchasingAccess != null)
            {
                String[] UniformPurchasingIds = objCompanyEmployee.UniformPurchasingAccess.Split(',');

                foreach (DataListItem dtuniformpurhcasing in dtUniPurchasing.Items)
                {
                    chk = dtuniformpurhcasing.FindControl("chkUniPurchasing") as CheckBox;
                    lblId = dtuniformpurhcasing.FindControl("hdnUniformPurchasing") as HiddenField;
                    HtmlGenericControl dvChk = dtuniformpurhcasing.FindControl("uniformspan") as HtmlGenericControl;

                    foreach (String i in UniformPurchasingIds)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }
                    }
                }
            }

            // Supplies
            if (objCompanyEmployee.SuppliesAccess != null)
            {
                String[] suppliesIds = objCompanyEmployee.SuppliesAccess.Split(',');

                foreach (DataListItem dtsupplies in dtSupplies.Items)
                {
                    chk = dtsupplies.FindControl("chkSupplies") as CheckBox;
                    lblId = dtsupplies.FindControl("hdnSupplies") as HiddenField;
                    HtmlGenericControl dvChk = dtsupplies.FindControl("supplliespan") as HtmlGenericControl;

                    foreach (String i in suppliesIds)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }
                    }
                }
            }

            //other Features - added by prashant jan - 2013
            if (objCompanyEmployee.DisplayTotalOrderAmount != null)
            { 
                chkDispTotalOrderAmount.Checked = Convert.ToBoolean(objCompanyEmployee.DisplayTotalOrderAmount);
                if (Convert.ToBoolean(objCompanyEmployee.DisplayTotalOrderAmount))
                {
                    displaytotalSpan.Attributes.Remove("class");
                    displaytotalSpan.Attributes.Add("class", "custom-checkbox_checked alignleft");
                }
            }

            // Base Stations 
            if (objCompanyEmployee.BaseStationsAccess != null)
            {
                String[] BaseStationsIds = objCompanyEmployee.BaseStationsAccess.Split(',');

                foreach (DataListItem dtBaseStationsAccess in dtBaseStations.Items)
                {
                    chk = dtBaseStationsAccess.FindControl("chkBaseStationsName") as CheckBox;
                    lblId = dtBaseStationsAccess.FindControl("hdnBaseStationID") as HiddenField;
                    HtmlGenericControl dvChk = dtBaseStationsAccess.FindControl("BaseStationsNamespan") as HtmlGenericControl;

                    foreach (String i in BaseStationsIds)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }
                    }
                }
            }
        }
    }

    public void BindData()
    {
        sEUBE.SOperation = "selectall";
        sEUBE.iLookupCode = "User Preferences";
        DataSet dsPR = sEU.LookUp(sEUBE);
        dtlPreferences.DataSource = dsPR;
        dtlPreferences.DataBind();

        dtAddiInfo.DataSource = new LookupRepository().GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.AdditionalInfo);
        dtAddiInfo.DataBind();

        dtEmpUniform.DataSource = objSubCatogeryRepository.GetAllSubCategory(1);
        dtEmpUniform.DataBind();

        dtCompanyStore.DataSource = objSubCatogeryRepository.GetAllSubCategory(3);
        dtCompanyStore.DataBind();

        dtUniPurchasing.DataSource = objSubCatogeryRepository.GetAllSubCategory(4);
        dtUniPurchasing.DataBind();

        dtSupplies.DataSource = objSubCatogeryRepository.GetAllSubCategory(2);
        dtSupplies.DataBind();


        MenuPrivilegeRepository objMenu = new MenuPrivilegeRepository();
        List<INC_MenuPrivilege> objList = objMenu.GetFrontMenu(DAEnums.MenuTypes.FrontEnd.ToString(), "Company" + " " + DAEnums.CompanyEmployeeTypes.Admin.ToString() + "," + "Company" + " " + DAEnums.CompanyEmployeeTypes.Employee.ToString());
        dtlMenus.DataSource = objList;
        dtlMenus.DataBind();

        BaseStationRepository objRepoBaseStations = new BaseStationRepository();
        dtBaseStations.DataSource = objRepoBaseStations.GetAllBaseStation().OrderBy(s => s.sBaseStation).ToList();
        dtBaseStations.DataBind();
    }

    private void SetPreferences()
    {
        CompanyEmployee objCE = new CompanyEmployeeRepository().GetById(this.EmployeeId);
        UserInformationRepository objUserRepo = new UserInformationRepository();

        UserInformation objUser = objUserRepo.GetById(objCE.UserInfoID);
        List<FUN_GetUserPreferenceResult> lstPref = new PreferenceRepository().GetUserPreferences(objUser.UserInfoID, objUser.Usertype).ToList();

        if (lstPref != null && lstPref.Count > 0)
        {
            dlPreferences.DataSource = lstPref;
            dlPreferences.DataBind();
        }
    }

    #endregion
}