using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;

public partial class admin_EmployeeTrainingCenter_SearchEmployeeTrainingCenter : PageBase
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

            BindDropDowns();

            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Employee Training Centre";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/EmployeeTrainingCenter/EmployeeTrainingCenter.aspx";
        }
    }

    protected void lnkBtnSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect("ListEmployeeTrainingCenter.aspx?FileName=" + txtFileName.Text.Trim() + "&Searchkeyword=" + txtSearchKeyword.Text.Trim() + "&UplodedBy=" + ddlIncentexEmp.SelectedValue.Trim() + "&DocumentType=" + ddlEmployeeTrainingType.SelectedValue.Trim() + "");
    }

    #endregion

    #region Method
    /// <summary>
    /// To Bind Drop Downs
    /// </summary>
    private void BindDropDowns()
    {
        LookupRepository objLookRep = new LookupRepository();
        ddlEmployeeTrainingType.DataSource = objLookRep.GetByLookup("EmployeeTrainingType");
        ddlEmployeeTrainingType.DataValueField = "iLookupID";
        ddlEmployeeTrainingType.DataTextField = "sLookupName";
        ddlEmployeeTrainingType.DataBind();
        ddlEmployeeTrainingType.Items.Insert(0, new ListItem("-Select-", "0"));

        IncentexEmployeeRepository objEmployee = new IncentexEmployeeRepository();
        ddlIncentexEmp.DataSource = objEmployee.GetEmployeeSAdmin();
        ddlIncentexEmp.DataValueField = "UserInfoID";
        ddlIncentexEmp.DataTextField = "EmployeeName";
        ddlIncentexEmp.DataBind();
        ddlIncentexEmp.Items.Insert(0, new ListItem("-Select-", "0"));
    }
    #endregion
}
