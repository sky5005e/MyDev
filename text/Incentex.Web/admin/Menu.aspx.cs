using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL.Common;

public partial class admin_Home2 : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = " ";
            ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = false;
            //((HtmlImage)Master.FindControl("imgOpenServiceTicket")).Visible = false;
            ((Label)Master.FindControl("lblPageHeading")).Text = "World-Link System Menu";

            //((Label)Master.FindControl("lblPageHeading")).Text = "World-Link System Menu";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/login.aspx";

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.EquipmentVendorEmployee))
                Response.Redirect("~/admin/index.aspx", false);
            else
                lnkWLSC.NavigateUrl = "~/admin/index.aspx";
                lnkSupplierPartner.NavigateUrl = "~/admin/ManageSupplierPartner/ListSupplier.aspx";
        }
    }
}