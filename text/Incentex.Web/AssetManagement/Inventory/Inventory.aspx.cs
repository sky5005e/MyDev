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
public partial class AssetManagement_Inventory_Inventory : PageBase
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

                ((Label)Master.FindControl("lblPageHeading")).Text = "Inventory";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/Inventory/ItemInfo.aspx";

                Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.EquipmentInventory;
                if (Request.QueryString.Count > 0)
                {
                    this.EquipmentInventoryID = Convert.ToInt64(Request.QueryString.Get("Id"));
                }

                menuControl.PopulateMenu(2, 0, this.EquipmentInventoryID, 0, false);
                lblMsg.Text = "";
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
            objInventoryMaster.CurrentInvenory = txtCurrentInventory.Text.Trim();
            objInventoryMaster.ReOrderLevel = txtReOrderLevel.Text.Trim();
            objInventoryMaster.SupplierInventory = txtSupplierInventory.Text;
            objInventoryMaster.OnOrderSupplier = txtOnOrderSupplier.Text;
            if (txtExpDeliveryDate.Text!="")
            {
                objInventoryMaster.ExpectedDeliveryDate = Convert.ToDateTime(txtExpDeliveryDate.Text.Trim());     
            }           
            objInventoryRepository.SubmitChanges();
            Response.Redirect("Location.aspx?Id=" + this.EquipmentInventoryID);
        }
        catch (Exception)
        {

            lblMsg.Text = "Data can't saved";
        }
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
                txtCurrentInventory.Text = Convert.ToString(objInventoryMaster.CurrentInvenory);
                txtReOrderLevel.Text = Convert.ToString(objInventoryMaster.ReOrderLevel);
                txtSupplierInventory.Text = Convert.ToString(objInventoryMaster.SupplierInventory);
                txtOnOrderSupplier.Text= Convert.ToString(objInventoryMaster.OnOrderSupplier);
                txtExpDeliveryDate.Text= Convert.ToString(objInventoryMaster.ExpectedDeliveryDate);
            }
        }
        catch (Exception)
        {

        }
    }
}
