using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL; 

public partial class HeadsetRepairCenter_HeadsetRepairVendor : PageBase
{
    #region Data Member's
    HeadsetRepairCenterRepository objHeadset = new HeadsetRepairCenterRepository();
    #endregion

    #region Events
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

            if (!base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Add Headset Repair Vendor";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/HeadsetRepairCenter/HeadsetRepairCenter.aspx";
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        HeadsetRepairVendor objvendor = new HeadsetRepairVendor();

        objvendor.VendorName = txtVendorName.Text.Trim();
        objvendor.VendorContact = txtContact.Text.Trim();
        objvendor.VendorTelephone = txtTelephone.Text.Trim();
        objvendor.VendorEmail = txtEmail.Text.Trim();
        objvendor.IsDeleted = false;
        objvendor.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objvendor.CreatedDate = System.DateTime.Now;

        objHeadset.Insert(objvendor);
        objHeadset.SubmitChanges();
        Response.Redirect("HeadsetRepairCenter.aspx?req=1");
    }
    #endregion
}
