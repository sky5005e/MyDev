using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;

public partial class admin_PurchaseOrderManagement_SearchPurchaseOpenOrders : PageBase
{
    SupplierRepository objSupplier = new SupplierRepository();
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

            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Open Order";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/VendorAccessIncOrderManegement.aspx";

            BindDropdown();
        }
    }

    private void BindDropdown()
    {
        
        ddlCompany.DataSource = objSupplier.GetAllsupplier();
        ddlCompany.DataValueField = "SupplierID";
        ddlCompany.DataTextField = "FirstName";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("-Select-", "0"));
       
    }
    protected void lnkBtnSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProductionOrderDetails.aspx?vendorID="+ddlCompany.SelectedValue+"&PONumber="+txtPONumber.Text+"&SearchDate="+txtConFirmDeliveryDate.Text+"");
    }
}
