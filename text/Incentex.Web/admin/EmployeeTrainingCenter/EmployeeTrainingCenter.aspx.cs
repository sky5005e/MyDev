using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_EmployeeTrainingCenter_EmployeeTrainingCenter : PageBase
{
    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Employee Training Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Employee Training Centre";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";

            lnkAddEmployeeTraining.NavigateUrl = "~/admin/EmployeeTrainingCenter/AddEmployeeTrainingCenter.aspx";
            lnkSearchEmployeeTraining.NavigateUrl = "~/admin/EmployeeTrainingCenter/SearchEmployeeTrainingCenter.aspx";
            lnkAddEmployeeTrainingType.NavigateUrl = "~/admin/EmployeeTrainingCenter/AddEmployeeTrainingType.aspx";

            if (!String.IsNullOrEmpty(Request.QueryString["req"]) && Request.QueryString["req"] == "1")
                lblMsg.Text = "Records sucessfully added";

        }
    }
    #endregion
}
