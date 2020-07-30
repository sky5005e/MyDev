using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_PurchaseOrderManagement_VendorAccessIncOrderManegement : PageBase
{
    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Incentex Order Management";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Incentex Order Management";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";

            lnkTodaysProduction.NavigateUrl = "~/admin/PurchaseOrderManagement/ProductionOrderDetails.aspx?search=today";
            lnkCurrentWeeksProduction.NavigateUrl = "~/admin/PurchaseOrderManagement/ProductionOrderDetails.aspx?search=week";
            lnkSearchOpenOrdered.NavigateUrl = "~/admin/PurchaseOrderManagement/SearchPurchaseOpenOrders.aspx";
           

           

        }
    }
    #endregion
}
