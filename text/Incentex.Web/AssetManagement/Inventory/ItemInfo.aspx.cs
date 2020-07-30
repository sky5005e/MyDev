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
public partial class AssetManagement_Inventory_ItemInfo : PageBase
{
    Int64 EquipmentInventoryID
    {
        get
        {
            if (ViewState["EquipmentInventoryID"] == null)
            {
                ViewState["EquipmentInventoryID"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentInventoryID"]);
        }
        set
        {
            ViewState["EquipmentInventoryID"] = value;
        }
    }
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
            if (!IsPostBack)
            {
                base.MenuItem = "Station Inventory";
                base.ParentMenuID = 46;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Item Info";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/Inventory/BasicInfo.aspx";

                Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.EquipmentInventory;
                if (Request.QueryString.Count > 0)
                {
                    this.EquipmentInventoryID = Convert.ToInt64(Request.QueryString.Get("Id"));
                }
               
                menuControl.PopulateMenu(1, 0, this.EquipmentInventoryID, 0, false);
                lblMsg.Text = "";
                BindValues();
                if (Request.QueryString.Count > 0)
                {
                    PopulateValues();
                }
            }
        }
        catch (Exception)
        {


        }
    }
    protected void lnkBtnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanAdd && !base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
            EquipmentInventoryMaster objInventoryMaster = new EquipmentInventoryMaster();            
            objInventoryMaster = objInventoryRepository.GetInventoryById(this.EquipmentInventoryID);           
            if (ddlCategory.SelectedValue != "0")
            {
                objInventoryMaster.CategoryID = Convert.ToInt64(ddlCategory.SelectedValue);
            }
            if (ddlSubCategory.SelectedValue != "0")
            {
                objInventoryMaster.SubCategory = Convert.ToInt64(ddlSubCategory.SelectedValue);
            }
            objInventoryMaster.PartNumber = txtPartNumber.Text.Trim();
            objInventoryMaster.MPartNumber = txtMFGPartNumber.Text.Trim();
            objInventoryMaster.ProductDescription = txtProductDescription.Text;
            objInventoryMaster.ProductCost = txtProductCost.Text.Trim();
            objInventoryRepository.SubmitChanges();            
            Response.Redirect("Inventory.aspx?Id=" + this.EquipmentInventoryID);
        }
        catch (Exception)
        {

            lblMsg.Text = "Data can't saved";
        }
    }
    public void BindValues()
    {
        try
        {

            //get Category:
            AssetInventoryRepository objRepo = new AssetInventoryRepository();
            List<EquipmentProductCategoryLookup> objCategoryList = new List<EquipmentProductCategoryLookup>();
            objCategoryList = objRepo.GetAllProductCategory();
            Common.BindDDL(ddlCategory, objCategoryList, "ProductCategoryName", "ProductCategoryID", "-Select Category-");
           

        }
        catch (Exception)
        {


        }
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        //get Sub Category:
        AssetInventoryRepository objRepo = new AssetInventoryRepository();
        List<EquipmentProductCategoryLookup> objSubCategoryList = new List<EquipmentProductCategoryLookup>();
        objSubCategoryList = objRepo.GetAllProductSubCategoryDetail(Convert.ToInt64(ddlCategory.SelectedValue));
        Common.BindDDL(ddlSubCategory, objSubCategoryList, "ProductCategoryName", "ProductCategoryID", "-Select SubCategory-");
    }
    public void PopulateValues()
    {
        try
        {
            AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
            EquipmentInventoryMaster objInventoryMaster = new EquipmentInventoryMaster();
            objInventoryMaster = objInventoryRepository.GetInventoryById(this.EquipmentInventoryID);
            if (objInventoryMaster != null)
            {
                ddlCategory.SelectedValue = Convert.ToString(objInventoryMaster.CategoryID);
                ddlSubCategory.SelectedValue = Convert.ToString(objInventoryMaster.SubCategory);
                txtPartNumber.Text = Convert.ToString(objInventoryMaster.PartNumber);
                txtMFGPartNumber.Text = Convert.ToString(objInventoryMaster.MPartNumber);
                txtProductDescription.Text = Convert.ToString(objInventoryMaster.ProductDescription);
                txtProductCost.Text = Convert.ToString(objInventoryMaster.ProductCost);
            }
        }
        catch (Exception)
        {

        }
    }
}
