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

public partial class AssetManagement_DropDownMenu : PageBase
{
    string strIDValue = null;
    string strText = null;
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
        if (!IsPostBack)
        {
            base.MenuItem = "Drop Down Menu";
            base.ParentMenuID = 46;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Drop Down Menu";
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
    /// <summary>
    /// When click on the linkbutton we get the ToolTip value and its id and redirect to vlookupdropdown
    /// and store its id as ilookUpCode and ToolTip value as its Text into the db
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>   
    /// ******************************************************
    protected void lnkbtnDropDown_Click(object sender, EventArgs e)
    {
        LinkButton lnkbtnDropDown = (LinkButton)sender;
        strText = Common.Encryption(lnkbtnDropDown.ToolTip);
        strIDValue = lnkbtnDropDown.ID;
        Response.Redirect("vLookupDropdown.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
}
