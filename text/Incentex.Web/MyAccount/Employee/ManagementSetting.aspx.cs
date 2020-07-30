using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using System.Reflection;

public partial class admin_Company_Employee_ManagementSetting : PageBase
{
    string selectedmanagementmenuoption = "";
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                CheckLogin();
                BindValues();
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("SubId"));
                IncentexGlobal.ManageID = 7;
                if (this.EmployeeId == 0)
                {
                    Response.Redirect("~/MyAccount/Employee/BasicInformation.aspx?Id=" + this.CompanyId);
                }

                menucontrol.PopulateMenu(0, 3, this.CompanyId, this.EmployeeId,true);
                
                /*Company objCompany = new CompanyRepository().GetById(this.CompanyId);
                
                if (objCompany == null)
                {
                    Response.Redirect("~/admin/Company/Employee/ViewEmployee.aspx");
                }*/
                ((Label)Master.FindControl("lblPageHeading")).Text = "Management Settings";
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
            BindMenu();
            DisplayData(sender, e);
        }
    }

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

    //bind dropdown for the workgroup, department, gender, 
    public void BindValues()
    {
        //For workgroup
        LookupRepository objLookupRepo = new LookupRepository();
        ddlWorkgroup.DataSource = objLookupRepo.GetByLookup("Workgroup");
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Department
        ddlDepartment.DataSource = objLookupRepo.GetByLookup("Department");
        ddlDepartment.DataValueField = "iLookupID";
        ddlDepartment.DataTextField = "sLookupName";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-Select-", "0"));     

        // For Regoin
        ddlRegion.DataSource = objLookupRepo.GetByLookup("Region");
        ddlRegion.DataValueField = "iLookupID";
        ddlRegion.DataTextField = "sLookupName";
        ddlRegion.DataBind();
        ddlRegion.Items.Insert(0, new ListItem("-Select-", "0"));

        
    }

    /// <summary>
    /// Fills the based station on country.
    /// </summary>
    /// <param name="countryid">The countryid.</param>
    public void FillBasedStationOnCountry()
    {
        IncentexBEDataContext db = new IncentexBEDataContext();
        var list = db.INC_BasedStations.Select(i => new { i.sBaseStation, i.iCountryID, i.sBaseStationIcon, i.iBaseStationId }).ToList();

        DataTable dsBaseStation = LINQToDataTable(list);//sBaseStation.GetBaseStaionByCountry(sBSBe);
        if (dsBaseStation.Rows.Count > 0)//&& dsBaseStation.Tables[0].Rows.Count > 0)
        {
            ddlBaseStation.DataSource = list;
            ddlBaseStation.DataValueField = "iBaseStationId";
            ddlBaseStation.DataTextField = "sBaseStation";
            ddlBaseStation.DataBind();
            ddlBaseStation.Items.Insert(0, new ListItem("-Select Sase Station-", "0"));
        }
      
    }

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
            if (objCompanyEmployee.isCompanyAdmin)
            {
                isAdmin.Visible = true;
                isEmployee.Visible = false;
                UserInformation objUserInfo = new UserInformationRepository().GetById(objCompanyEmployee.UserInfoID);
                FillBasedStationOnCountry();//(objUserInfo.CountryId.Value  == null ? -1 : objUserInfo.CountryId.Value)) ;
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


               ddlDepartment.SelectedValue = objCompanyEmployee.ManagementControlForDepartment.ToString();
               ddlRegion.SelectedValue = objCompanyEmployee.ManagementControlForRegion.ToString();
               ddlWorkgroup.SelectedValue = objCompanyEmployee.ManagementControlForWorkgroup.ToString();
               ddlBaseStation.SelectedValue = objCompanyEmployee.ManagementControlForStaionlocation.ToString();
            }
            else
            {
                isAdmin.Visible = false;
                isEmployee.Visible = true;
                
            }




        }
    }
    public void BindMenu()
    {
        MenuPrivilegeRepository objMenu = new MenuPrivilegeRepository();
        List<INC_MenuPrivilege> objList = objMenu.GetFrontMenu(DAEnums.MenuTypes.FrontEnd.ToString(), "Company" + " " + DAEnums.CompanyEmployeeTypes.Admin.ToString());
        dtlMenus.DataSource = objList;
        dtlMenus.DataBind();

    }


    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            //Delete from EmployeeMenuaccessfirst
            CompanyEmpMenuAccessRepository objCmpMenuAccesRepos = new CompanyEmpMenuAccessRepository();
            CompanyEmpMenuAccess objCmpMenuAccessfordel = new CompanyEmpMenuAccess();

            List<CompanyEmpMenuAccess> lst = objCmpMenuAccesRepos.getMenuByUserType(EmployeeId, "Company Admin");

            foreach (CompanyEmpMenuAccess l in lst)
            {
                objCmpMenuAccesRepos.Delete(l);
                objCmpMenuAccesRepos.SubmitChanges();
            }


            //insert into map table
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
            objCompanyEmployee.ManagementControlForDepartment = Convert.ToInt64(ddlDepartment.SelectedValue);
            objCompanyEmployee.ManagementControlForRegion = Convert.ToInt64(ddlRegion.SelectedValue);
            objCompanyEmployee.ManagementControlForWorkgroup = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            if(ddlBaseStation.SelectedValue!= "")
                objCompanyEmployee.ManagementControlForStaionlocation = Convert.ToInt64(ddlBaseStation.SelectedValue); 

            objEmpRepo.SubmitChanges();

            //Redirection
            Response.Redirect("ShoppingSetting.aspx?Id=" + this.CompanyId + "&SubId=" + this.EmployeeId);
        }
        catch (Exception ex)
        {
            lblMsg.Text = objcomm.Callvalidationmessage(Server.MapPath("../../JS/JQuery/validationMessages/commonValidationMsg.xml"), "ProcessError", "");
            ErrHandler.WriteError(ex);
        }
    }
}