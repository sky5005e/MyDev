using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HeadsetRepairCenter_HeadsetRepairCenter : PageBase
{
    #region Event
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
            base.MenuItem = "Headset Repair Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Headset Repair Center";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";

            lnkCreateHeadsetRepairCenter.NavigateUrl = "~/HeadsetRepairCenter/AddHeadsetRepairCenter.aspx";
            lnkSearchHeadsetRepairCenter.NavigateUrl = "~/HeadsetRepairCenter/SearchHeadsetRepairCenter.aspx";

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) ||
              IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
            {
                lnkAddHeadsetRepairVendor.Visible = true; 
                lnkAddHeadsetRepairVendor.NavigateUrl = "~/HeadsetRepairCenter/HeadsetRepairVendor.aspx";
            }

            if (!String.IsNullOrEmpty(Request.QueryString["req"]) && Request.QueryString["req"] == "1")
                lblMsg.Text = "Records sucessfully Created";

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) ||
             IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
            {
                Response.Redirect("~/HeadsetRepairCenter/AddHeadsetRepairCenter.aspx");  
            }
        }
    }

    #endregion
}
