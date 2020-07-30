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
using System.IO;
using AjaxControlToolkit;

public partial class admin_CommunicationCenter_ViewCamp : PageBase
{
    CampignRepo ObjCamprepo = new CampignRepo();

    protected void Page_Load(object sender, EventArgs e)
    {   
        if (!IsPostBack)
        {
            base.MenuItem = "Communications Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Campaign Details";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CampaignSelection.aspx";

            Bind_GridViewCampUser();
            lblMsgGrid.Visible = false;
        }
    }

    public void Bind_GridViewCampUser()
    {
        // right one
        //var lst =  ObjCamprerp.GetAllCampUser(Convert.ToInt32(Request.QueryString["cid"]),Convert.ToInt32(Request.QueryString["wid"]),Convert.ToInt32(Request.QueryString["did"]),Convert.ToInt32(Request.QueryString["gid"]),Convert.ToInt32(Request.QueryString["sid"]));
        //delete start
        var lst = ObjCamprepo.GetViewCampDetail();
        //delete end
        gvViewCamp.DataSource = lst;
        gvViewCamp.DataBind();
    }

    protected void gvViewCamp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "del")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            Int64 val = Convert.ToInt64(e.CommandArgument);
            //lblMsg.Text = "";
            //GridViewRow row;
            //row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            ObjCamprepo.DeleteCamp(Convert.ToInt64(val));
            //objcommon.DeleteImageFromFolder(((HiddenField)dtlManuals.Rows[row.RowIndex].FindControl("hfHiddenFile")).Value, IncentexGlobal.companystoredocuments);
            lblMsgGrid.Text = "Record deleted successfully";
            Bind_GridViewCampUser();
        }

    }

    protected void gvViewCamp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblIsComplete = (Label)e.Row.FindControl("lblIsComplete");
            Label LblCampaignName = (Label)e.Row.FindControl("LblCampaignName");
            HyperLink lnkBtnEdit = (HyperLink)e.Row.FindControl("lnkBtnEdit");
            HyperLink lnkedit = (HyperLink)e.Row.FindControl("lnkedit");
            
            if (lblIsComplete.Text=="False" || lblIsComplete.Text=="false")
            {
                LblCampaignName.Style.Add("background-color", "Red");
                LblCampaignName.Style.Add("color", "Black");
                lnkedit.Attributes.Add("style", "padding-right:40px;");
                lnkBtnEdit.Visible = true;
            }
            else
            {
                lnkedit.Attributes.Add("style", "padding-right:65px;");
                lnkBtnEdit.Visible = false;
            }
            
        }
    }
}
