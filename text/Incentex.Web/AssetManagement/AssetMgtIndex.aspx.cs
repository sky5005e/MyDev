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

public partial class AssetManagement_AssetMgtIndex : PageBase
{
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
                base.MenuItem = "GSE Asset Management";
                base.ParentMenuID = 0;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Asset Management System";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void lnkManageEquipment_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddEquipment.aspx");
    }
    protected void lnkSearchEquipment_Click(object sender, EventArgs e)
    {
        Response.Redirect("SearchEquipment.aspx");
    }
    protected void lnkFlaggedAssets_Click(object sender, EventArgs e)
    {
        Response.Redirect("FlaggedAssets.aspx");
    }
    protected void lnkUserManagement_Click(object sender, EventArgs e)
    {
        Response.Redirect("VendorList.aspx");
    }
    protected void LnkDropDownMenu_Click(object sender, EventArgs e)
    {
        Response.Redirect("DropDownMenu.aspx");
    }
    protected void lnkPendingInvoices_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingInvoice.aspx");
    }
    protected void lnkInventory_Click(object sender, EventArgs e)
    {
        Response.Redirect("Inventory/InventoryIndex.aspx");
    }
    protected void lnkFieldManagement_Click(object sender, EventArgs e)
    {
        Response.Redirect("FieldMgt/FieldMgtIndex.aspx");
    }
    protected void lnkPlanningReports_Click(object sender, EventArgs e)
    {
        Response.Redirect("Reports/ReportDashBoard.aspx");
    }
    protected void lnkRepairOrderManagement_Click(object sender, EventArgs e)
    {
        Response.Redirect("RepairManagement/RepairManagementIndex.aspx");
    }
    protected void lnkBlogCenter_Click(object sender, EventArgs e)
    {
        Response.Redirect("Blog/BlogIndex.aspx");
    }
}
