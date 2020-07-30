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
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL;

public partial class AssetManagement_Inventory_SearchInventory : PageBase
{
    #region DataMembers
    CompanyEmployee objCmpnyInfo = new CompanyEmployee();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    UserInformation objuser = new UserInformation();
    UserInformationRepository objuserrepos = new UserInformationRepository();
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
    Int64 BaseStationID
    {
        get
        {
            if (ViewState["BaseStationID"] == null)
            {
                ViewState["BaseStationID"] = 0;
            }
            return Convert.ToInt64(ViewState["BaseStationID"]);
        }
        set
        {
            ViewState["BaseStationID"] = value;
        }
    }
    Int64 ProductCategoryID
    {
        get
        {
            if (ViewState["ProductCategoryID"] == null)
            {
                ViewState["ProductCategoryID"] = 0;
            }
            return Convert.ToInt64(ViewState["ProductCategoryID"]);
        }
        set
        {
            ViewState["ProductCategoryID"] = value;
        }
    }
    Int64 ProductSCategoryID
    {
        get
        {
            if (ViewState["ProductSCategoryID"] == null)
            {
                ViewState["ProductSCategoryID"] = 0;
            }
            return Convert.ToInt64(ViewState["ProductSCategoryID"]);
        }
        set
        {
            ViewState["ProductSCategoryID"] = value;
        }
    }
    string PartNumber = null;   
    string MFGPartNumber = null;   
    #endregion
    #region Events
    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                MasterPageFile = "~/admin/MasterPageAdmin.master";
            }
            else
            {
                MasterPageFile = "~/MasterPage.master";
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Equipment Inventory";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";           
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/AssetMgtIndex.aspx";
           
            if (!IsPostBack)
            {
                lblMsg.Text = "";
                BindValues();

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    ddlCompany.SelectedValue = Convert.ToString(IncentexGlobal.CurrentMember.CompanyId);
                    ddlCompany.Enabled = false;
                }
                else
                {
                    ddlCompany.Items.Insert(0, new ListItem("-Select-", "0"));
                    ddlCompany.Enabled = true;
                }
            }
        }
        catch (Exception)
        {

        }
    }
    protected void lnkBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {

            this.CompanyID =  Convert.ToInt64(ddlCompany.SelectedValue);
            this.BaseStationID = Convert.ToInt64(ddlBaseStation.SelectedValue);
            this.ProductCategoryID =  Convert.ToInt64(ddlProductCategory.SelectedValue);
            this.ProductSCategoryID = ddlProductSCategory.SelectedValue != "" ? Convert.ToInt64(ddlProductSCategory.SelectedValue) : 0;
            this.PartNumber = txtPartNumber.Text != "" ? txtPartNumber.Text : null;
            this.MFGPartNumber = txtMFGPartNumber.Text != "" ? txtMFGPartNumber.Text : null;
            Response.Redirect("~/AssetManagement/Inventory/InventoryResult.aspx?CompanyID=" + CompanyID + "&BaseStationID=" + BaseStationID + "&ProductCategoryID=" + ProductCategoryID + "&ProductSCategoryID=" + ProductSCategoryID + "&PartNumber=" + PartNumber + "&MFGPartNumber=" + MFGPartNumber);
        }
        catch (Exception)
        {

        }
    }
    protected void ddlProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
             //get Sub Product Category
            AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
            List<EquipmentProductCategoryLookup> objSCategoryList = new List<EquipmentProductCategoryLookup>();
            objSCategoryList = objInventoryRepository.GetAllProductSubCategoryDetail(Convert.ToInt64(ddlProductCategory.SelectedValue));
            Common.BindDDL(ddlProductSCategory, objSCategoryList, "ProductCategoryName", "ProductCategoryID", "-Select Product Sub-Category-");
        }
        catch (Exception)
        {
            
           
        }
    }
    #endregion
    #region Methods
    public void BindValues()
    {
        try
        {
            
            //get Base Stations
            CompanyRepository objRepo = new CompanyRepository();
            List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
            objBaseStationList = objRepo.GetAllBaseStationResult();
            objBaseStationList = objBaseStationList.OrderBy(p => p.sBaseStation).ToList();
            Common.BindDDL(ddlBaseStation, objBaseStationList, "sBaseStation", "iBaseStationId", "-Select All BaseSation-");

            //get company
            CompanyRepository objCompanyRepo = new CompanyRepository();
            List<Company> objCompanyList = new List<Company>();
            objCompanyList = objCompanyRepo.GetAllCompany();
            Common.BindDDL(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "-Select Company-");

            //get Product Category
            AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
            List<EquipmentProductCategoryLookup> objCategoryList = new List<EquipmentProductCategoryLookup>();
            objCategoryList = objInventoryRepository.GetAllProductCategory();
            Common.BindDDL(ddlProductCategory, objCategoryList, "ProductCategoryName", "ProductCategoryID", "-Select Product Category-");
           
        }
        catch (Exception)
        {


        }
    }

    #endregion   
}
