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

public partial class admin_Company_Employee_ManagementSetting : PageBase
{
    #region Data Members

    LookupRepository objLookupRepos = new LookupRepository();

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

    Common objcomm = new Common();

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

                menucontrol.PopulateMenu(2, 3, this.CompanyId, this.EmployeeId, true);

                ((Label)Master.FindControl("lblPageHeading")).Text = "Management Settings";
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
            BindMenu();
            FillDepartment();
            FillWorkgroup();
            FillRegion();
            DisplayData();            
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        Boolean Success = false;
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            //Delete from EmployeeMenuaccessfirst
            CompanyEmpMenuAccessRepository objCmpMenuAccesRepos = new CompanyEmpMenuAccessRepository();
            CompanyEmpMenuAccess objCmpMenuAccessfordel = new CompanyEmpMenuAccess();

            List<CompanyEmpMenuAccess> lst = objCmpMenuAccesRepos.getMenuByUserType(EmployeeId, "Company Admin");

            foreach (CompanyEmpMenuAccess l in lst)
            {
                objCmpMenuAccesRepos.Delete(l);
                objCmpMenuAccesRepos.SubmitChanges();
            }

            //insert into menu table
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
                }
            }

            //Update Management control for following
            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
            objCompanyEmployee = objEmpRepo.GetById(EmployeeId);

            if (ddlDepartment.SelectedValue != "0")
                objCompanyEmployee.ManagementControlForDepartment = Convert.ToInt64(ddlDepartment.SelectedValue);
            else
                objCompanyEmployee.ManagementControlForDepartment = null;

            if (ddlWorkgroup.SelectedValue != "0")
                objCompanyEmployee.ManagementControlForWorkgroup = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            else
                objCompanyEmployee.ManagementControlForWorkgroup = null;

            if (ddlBasestation.SelectedValue != "0")
                objCompanyEmployee.ManagementControlForStaionlocation = Convert.ToInt64(ddlBasestation.SelectedValue);
            else
                objCompanyEmployee.ManagementControlForStaionlocation = null;

            if (ddlRegion.SelectedValue != "0")
                objCompanyEmployee.ManagementControlForRegion = Convert.ToInt64(ddlRegion.SelectedValue);
            else
                objCompanyEmployee.ManagementControlForRegion = null;

            objEmpRepo.SubmitChanges();

            Success = true;
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }

        //Redirection
        if (Success)
            Response.Redirect("UserReports.aspx?Id=" + this.CompanyId + "&SubId=" + this.EmployeeId);
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
                List<PreferenceValue> lstPreference = new PreferenceRepository().GetPreferenceValuesByPreferenceID(PreferenceID);
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
        ddlWorkgroup.DataSource = objLookupRepos.GetByLookup("Workgroup");
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

    private void FillBasedStationOnCountry(long countryid)
    {
        BaseStationDA sBaseStation = new BaseStationDA();
        BasedStationBE sBSBe = new BasedStationBE();

        sBSBe.SOperation = "getBaseStationbyCounty";
        sBSBe.iCountryID = countryid;
        string sOptions = string.Empty;
        DataSet dsBaseStation = sBaseStation.GetBaseStaionByCountry(sBSBe);
        if (dsBaseStation.Tables.Count > 0 && dsBaseStation.Tables[0].Rows.Count > 0)
        {
            ddlBasestation.DataSource = dsBaseStation.Tables[0];
            ddlBasestation.DataValueField = "iBaseStationId";
            ddlBasestation.DataTextField = "sBaseStation";
            ddlBasestation.DataBind();
            ddlBasestation.Items.Insert(0, new ListItem("-select basestation-", "0"));
        }
    }

    private void BindMenu()
    {
        MenuPrivilegeRepository objMenu = new MenuPrivilegeRepository();
        List<INC_MenuPrivilege> objList = objMenu.GetFrontMenu(DAEnums.MenuTypes.FrontEnd.ToString(), "Company" + " " + DAEnums.CompanyEmployeeTypes.Admin.ToString());
        dtlMenus.DataSource = objList;
        dtlMenus.DataBind();

    }

    void DisplayData()
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

            if (objCompanyEmployee.isCompanyAdmin)
            {
                isAdmin.Visible = true;
                isEmployee.Visible = false;
                UserInformation objUserInfo = new UserInformationRepository().GetById(objCompanyEmployee.UserInfoID);
                FillBasedStationOnCountry(Convert.ToInt64(objUserInfo.CountryId));
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

                ddlDepartment.SelectedValue = objCompanyEmployee.ManagementControlForDepartment != null ? objCompanyEmployee.ManagementControlForDepartment.ToString() : "0";
                ddlRegion.SelectedValue = objCompanyEmployee.ManagementControlForRegion != null ? objCompanyEmployee.ManagementControlForRegion.ToString() : "0";
                ddlWorkgroup.SelectedValue = objCompanyEmployee.ManagementControlForWorkgroup != null ? objCompanyEmployee.ManagementControlForWorkgroup.ToString() : "0";
                ddlBasestation.SelectedValue = objCompanyEmployee.ManagementControlForStaionlocation != null ? objCompanyEmployee.ManagementControlForStaionlocation.ToString() : "0";
            }
            else
            {
                isAdmin.Visible = false;
                isEmployee.Visible = true;
            }
        }
    }

    #endregion
}