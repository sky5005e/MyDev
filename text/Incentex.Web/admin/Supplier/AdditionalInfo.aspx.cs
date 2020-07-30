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

public partial class admin_Supplier_AdditionalInfo : PageBase
{
    #region Properties

    Int64 SupplierId
    {
        get
        {
            if (ViewState["SupplierId"] == null)
            {
                ViewState["SupplierId"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierId"]);
        }
        set
        {
            ViewState["SupplierId"] = value;
        }
    }

    #endregion
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Additional Supplier Info";
            base.ParentMenuID = 18;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Additional Info";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to Supplier listing</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Supplier/ViewSupplier.aspx";
          
            if (Request.QueryString.Count > 0)
            {
                this.SupplierId = Convert.ToInt64(Request.QueryString.Get("Id"));
                manuControl.PopulateMenu(4, 0, this.SupplierId, 0, false);
            }
            else
            {
                Response.Redirect("~/admin/Supplier/ViewSupplier.aspx");
            }


            if (this.SupplierId == 0)
            {
                Response.Redirect("~/admin/Supplier/MainCompanyContact.aspx?Id=0");
            }
        }
    }
}
