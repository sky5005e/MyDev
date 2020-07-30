using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;

public partial class admin_PurchaseOrderManagement_SearchPurchaseOrders : PageBase 
{
    #region Data Member's
    LookupRepository objLookRep = new LookupRepository();
    SupplierRepository objSupplier = new SupplierRepository();
    PurchaseOrderManagmentRepository objordermanagmentRep = new PurchaseOrderManagmentRepository();
    #endregion

    #region Event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Purchase Order Management";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Purchase Order Management";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/PurchaseOrderManagement.aspx";

            this.BindDropdown();
        }
    }

    protected void ddlvendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<StoreProductCustom> objlst = objordermanagmentRep.GetAllMasterItemBysupplierID(Convert.ToInt64(ddlvendor.SelectedValue));
        ddlMasterItem.DataSource = objlst;
        ddlMasterItem.DataValueField = "storeproductid";
        ddlMasterItem.DataTextField = "summary";
        ddlMasterItem.DataBind();
        ddlMasterItem.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void BindDropdown()
    {
        ddlMasterItem.Items.Insert(0, new ListItem("-Select-", "0"));

        #region Vendor
        ddlvendor.DataSource = objSupplier.GetAllsupplier();
        ddlvendor.DataValueField = "SupplierID";
        ddlvendor.DataTextField = "FirstName";
        ddlvendor.DataBind();
        ddlvendor.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion
    }

    protected void lnkBtnSearch_Click(object sender, EventArgs e)
    {
        long VendorID = ddlvendor.SelectedIndex > 0 ? Convert.ToInt64(ddlvendor.SelectedValue) : 0;
        long MasterItemID = ddlMasterItem.SelectedIndex > 0 ? Convert.ToInt64(ddlMasterItem.SelectedValue) : 0;
        string OrderNumber = txtOrderNumber.Text.Trim();

        Response.Redirect("PurchaseOrderList.aspx?vendorID=" + VendorID + "&MasterItemID=" + MasterItemID + "&OrderNumber=" + OrderNumber);  
    }
    #endregion
    
}
