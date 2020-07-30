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

public partial class AssetManagement_Blog_BlogIndex : PageBase
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
                base.MenuItem = "Blog Center";
                base.ParentMenuID = 46;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Blog Center";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                if (Session["Usr"] == Convert.ToString(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Index.aspx";
                }
                else
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/AssetMgtIndex.aspx";
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkPostBlog_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }
            Response.Redirect("BlogDetail.aspx?FromPage=PostBlog");
        }
        catch (Exception)
        {}
    }
    protected void lnkTodayBlog_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }
            Response.Redirect("BlogDetail.aspx?FromPage=TodayBlog");
        }
        catch (Exception)
        { }
    }
    protected void lnkSearchBlog_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }
            Response.Redirect("BlogSearch.aspx");
        }
        catch (Exception)
        { }
    }

   
}
