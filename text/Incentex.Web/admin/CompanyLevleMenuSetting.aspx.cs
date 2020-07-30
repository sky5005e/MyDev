using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyLevleMenuSetting : PageBase
{
    Int64 WorkGroupId
    {
        get
        {
            if (ViewState["WorkGroupId"] == null)
            {
                ViewState["WorkGroupId"] = 0;
            }
            return Convert.ToInt64(ViewState["WorkGroupId"]);
        }
        set
        {
            ViewState["WorkGroupId"] = value;
        }
    }

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

    String selectedMainMenu = "";
    String selectedemployeeuniform = "";
    String seelctedadditonalinfo = "";
    String selectedcompanystore = "";
    String selecteduniform = "";
    String selectedsupplies = "";

    SubCatogeryRepository objSubCatogeryRepository = new SubCatogeryRepository();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {            
            this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
            this.WorkGroupId = Convert.ToInt64(Request.QueryString.Get("workgroupid"));
            ((Label)Master.FindControl("lblPageHeading")).Text = "Gloabal Setting For Menu Access";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyLevelGlobalSetting.aspx?id=" + this.CompanyId;

            BindData();
        }
    }

    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("CompanyLevelGlobalSetting.aspx?id="+this.CompanyId);
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        selectmenuaccess a = new selectmenuaccess();

        //Main menu setting
        foreach (DataListItem dtMen in dtlMenus.Items)
        {
            if (((CheckBox)dtMen.FindControl("chkdtlMenus")).Checked == true)
            {
                if (selectedMainMenu == "")
                    selectedMainMenu = ((HiddenField)dtMen.FindControl("hdnMenuAccess")).Value;
                else
                    selectedMainMenu = selectedMainMenu + "," + ((HiddenField)dtMen.FindControl("hdnMenuAccess")).Value;
            }
        }

        a.mainmenusetting = selectedMainMenu;  

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

        a.employeeuniform = selectedemployeeuniform;

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

        a.additonalinfo = seelctedadditonalinfo;

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

        a.companystore = selectedcompanystore;  

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

        a.uniform = selecteduniform;  


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

        a.supplies = selectedsupplies;
        
        //Assign session for the other page..
        Session["MenuAccess"] = a;
        
        Response.Redirect("CompanyLevelStoreSetting.aspx?id=" + this.CompanyId + "&workgroupid=" + this.WorkGroupId);
    }

    public void BindData()
    {
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
}