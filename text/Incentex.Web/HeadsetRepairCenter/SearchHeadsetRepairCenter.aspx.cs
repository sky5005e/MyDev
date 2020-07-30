using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;

public partial class HeadsetRepairCenter_SearchHeadsetRepairCenter : PageBase
{
    #region Data Member's
    CompanyRepository objcompany = new CompanyRepository();
    UserInformationRepository objcompanyEmployee = new UserInformationRepository();
    LookupRepository objLookRep = new LookupRepository();
    HeadsetRepairCenterRepository objHeadset = new HeadsetRepairCenterRepository();
    #endregion

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

            BindDropdown();

            ((Label)Master.FindControl("lblPageHeading")).Text = "Headset Repair Center";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/HeadsetRepairCenter/HeadsetRepairCenter.aspx";

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) ||
                IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
            {
                trCompany.Visible = false;
                trContact.Visible = false;

                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/TrackingCenter.aspx";
            }
        }
    }

    protected void BindDropdown()
    {
        ddlContact.Items.Insert(0, new ListItem("-Select-", "0"));

        #region Company
        ddlCompany.DataSource = objcompany.GetAllCompany();
        ddlCompany.DataValueField = "CompanyID";
        ddlCompany.DataTextField = "CompanyName";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion

        #region Status
        ddlStatus.DataSource = objLookRep.GetByLookup("HeadsetStatus");
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));
        #endregion
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlContact.DataSource = objcompanyEmployee.GetAllUser(Convert.ToInt64(ddlCompany.SelectedValue));
        ddlContact.DataValueField = "UserInfoID";
        ddlContact.DataTextField = "UserName";
        ddlContact.DataBind();
        ddlContact.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    protected void lnkBtnSearch_Click(object sender, EventArgs e)
    {
        long Company = Convert.ToInt64(ddlCompany.SelectedValue);
        long Contact = trContact.Visible ? Convert.ToInt64(ddlContact.SelectedValue) : IncentexGlobal.CurrentMember.UserInfoID;
        long Status = Convert.ToInt64(ddlStatus.SelectedValue);    
        string RepairNumber = (!string.IsNullOrEmpty(txtrepairnumber.Text) && txtrepairnumber.Text.Length > 2) ? txtrepairnumber.Text.Trim().Substring(2, txtrepairnumber.Text.Length - 2) : string.Empty;

        Response.Redirect("ListofHeadsetRepairCenter.aspx?Company=" + Company + "&Contact=" + Contact + "&RepairNumber=" + RepairNumber + "&Status=" + Status + "");
    }
    #endregion 
}
