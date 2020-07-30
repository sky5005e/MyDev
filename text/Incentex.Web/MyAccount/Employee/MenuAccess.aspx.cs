using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Company_Employee_MenuAccess : PageBase
{
    #region Data Members

    LookupDA sEU = new LookupDA();
    LookupBE sEUBE = new LookupBE();

    String selectedpreferences = "";
    String selectedemployeeuniform = "";
    String seelctedadditonalinfo = "";
    String selectedcompanystore = "";
    String selecteduniform = "";
    String selectedsupplies = "";

    SubCatogeryRepository objSubCatogeryRepository = new SubCatogeryRepository();

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
        CheckLogin();

        if (!IsPostBack)
        {            
            if (Request.QueryString.Count > 0)
            {
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("SubId"));

                IncentexGlobal.ManageID = 7;
                if (this.EmployeeId == 0)
                    Response.Redirect("~/MyAccount/Employee/BasicInformation.aspx?Id=" + this.CompanyId);

                menucontrol.PopulateMenu(0, 2, this.CompanyId, this.EmployeeId, true);

                ((Label)Master.FindControl("lblPageHeading")).Text = "Menu Access";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/Employee/ViewEmployee.aspx?id=" + this.CompanyId;

                //set Search Page Return URL
                if (IncentexGlobal.SearchPageReturnURLFrontSide != null)
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.SearchPageReturnURLFrontSide;
            }
            else
                Response.Redirect("~/MyAccount/Employee/ViewEmployee.aspx");

            BindData();
            DisplayData(sender, e);
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        Boolean IsError = false;

        try
        {
            //Delete from EmployeeMenuaccessfirst
            CompanyEmpMenuAccessRepository objCmpMenuAccesRepos = new CompanyEmpMenuAccessRepository();
            CompanyEmpMenuAccess objCmpMenuAccessfordel = new CompanyEmpMenuAccess();

            List<CompanyEmpMenuAccess> lst = objCmpMenuAccesRepos.getMenuByUserType(EmployeeId, "Company Admin,Company Employee");

            foreach (CompanyEmpMenuAccess l in lst)
            {
                objCmpMenuAccesRepos.Delete(l);
                objCmpMenuAccesRepos.SubmitChanges();
            }

            //Menu Access
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

            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
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
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
            IsError = true;
        }

        if (!IsError)
            Response.Redirect("ManagementSetting.aspx?Id=" + this.CompanyId + "&SubId=" + this.EmployeeId);
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
                return;

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
    }

    #endregion
}