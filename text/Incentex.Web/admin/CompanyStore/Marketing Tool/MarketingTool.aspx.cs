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

public partial class admin_CompanyStore_Marketing_Tool_MarketingTool : PageBase
{
    #region Local Property
    Int64 CompanyStoreId
    {
        get
        {
            if (ViewState["CompanyStoreId"] == null)
            {
                ViewState["CompanyStoreId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                base.MenuItem = "Store Management Console";
                base.ParentMenuID = 0;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                if (Request.QueryString["Id"] != null)
                {
                    this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));

                    if (this.CompanyStoreId == 0)
                    {
                        Response.Redirect("~/admin/CompanyStore/GeneralStoreSetup.aspx?Id=" + this.CompanyStoreId);
                    }

                    if (this.CompanyStoreId > 0 && !base.CanEdit)
                    {
                        base.RedirectToUnauthorised();
                    }
                }
                ((Label)Master.FindControl("lblPageHeading")).Text = "Marketing Tool";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/StoreView.aspx?id=" + this.CompanyStoreId;
                menuControl.PopulateMenu(13, 0, this.CompanyStoreId, 0, false);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void lnkGlobalPricingDiscount_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/CompanyStore/Marketing Tool/GlobalPricingDiscount.aspx?id=" + this.CompanyStoreId);
    }
    protected void lnkPromotionCode_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/CompanyStore/Marketing Tool/PromotionCode.aspx?id=" + this.CompanyStoreId);
    }
}
