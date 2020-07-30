using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyLevelGlobalSetting : PageBase
{
    #region Data Members
    Int64 CompanyId
    {
        get
        {
            if (ViewState["CompanyId"] == null)
            {
                ViewState["CompanyId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyId"]);
        }
        set
        {
            ViewState["CompanyId"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Company";
            base.ParentMenuID = 10;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }
   
            this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
            ((Label)Master.FindControl("lblPageHeading")).Text = "Gloabal Setting";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Company/Employee/ViewEmployee.aspx?id=" + this.CompanyId;
            FillDepartment();
            FillWorkgroup();
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        List<selectusersuploadfromexcelsheetResult> a = new UserInformationRepository().GetNewUserInformation(this.CompanyId, Convert.ToInt64(ddlWorkgroup.SelectedValue));
        if (a.Count > 0)
        {
            Response.Redirect("CompanyLevleMenuSetting.aspx?id=" + this.CompanyId + "&workgroupid=" + ddlWorkgroup.SelectedValue);
        }
        else
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = "There are no company employee for whom you can set the menu setting and store setting.";
            return;
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Fills the Department.
    /// </summary>
    private void FillDepartment()
    {
        LookupDA sDepartment = new LookupDA();
        LookupBE sDepartmentBE = new LookupBE();
        sDepartmentBE.SOperation = "selectall";
        sDepartmentBE.iLookupCode = "Department";
        ddlDepartment.DataSource = sDepartment.LookUp(sDepartmentBE);
        ddlDepartment.DataValueField = "iLookupID";
        ddlDepartment.DataTextField = "sLookupName";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-select department-", "0"));
    }

    /// <summary>
    /// Fills the workgroup.
    /// </summary>
    private void FillWorkgroup()
    {
        LookupDA sWorkgroup = new LookupDA();
        LookupBE sWorkgroupBE = new LookupBE();
        sWorkgroupBE.SOperation = "selectall";
        sWorkgroupBE.iLookupCode = "Workgroup";
        ddlWorkgroup.DataSource = sWorkgroup.LookUp(sWorkgroupBE);
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-select workgroup-", "0"));
    }
    #endregion
}
