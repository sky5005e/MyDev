using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;

using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_ViewPendingUsers : PageBase
{
    #region Data Members
    RegistrationRepository.UsersSortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = RegistrationRepository.UsersSortExpType.EmployeeName;
            }
            return (RegistrationRepository.UsersSortExpType)ViewState["SortExp"];
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }

    Incentex.DAL.Common.DAEnums.SortOrderType SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
            }
            return (Incentex.DAL.Common.DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }
    PagedDataSource pds = new PagedDataSource();
    /// <summary>
    /// Set true when user click from his/her email button view pending shopping cart.
    /// </summary>
    Boolean IsFormEmail
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsFormEmail"]);
        }
        set
        {
            ViewState["IsFormEmail"] = value;
        }
    }
    Int64 UserInfoID
    {
        get
        {
            return Convert.ToInt64(ViewState["UserInfoID"]);
        }
        set
        {
            ViewState["UserInfoID"] = value;
        }
    }

    /// <summary>
    /// Set this value when view all is clicked.
    /// </summary>
    private Boolean IsViewAllTotal
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsViewAllTotal"]);
        }
        set
        {
            ViewState["IsViewAllTotal"] = value;
        }
    }



    #endregion

    #region Page Event's

    protected void Page_Load(object sender, EventArgs e)
    {
        // Top - Link 
        base.SetTopLinkScript("admin-link");
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["UserId"]))
                this.UserInfoID = Convert.ToInt64(Request.QueryString["UserId"]);
            else
                this.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;

            if (!String.IsNullOrEmpty(Request.QueryString["RegisID"])&& !String.IsNullOrEmpty(Request.QueryString["Status"]))
            {
                String _registrationID = Convert.ToString(Request.QueryString["RegisID"]);
                String _status = Convert.ToString(Request.QueryString["Status"]);
                ProcessRequest(_status, _registrationID);
            }
            BindPendingUsersGrid();
        }
    }

    protected void gvPendingUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblUserName = (Label)e.Row.FindControl("lblUserName");
            HiddenField hdnUser = (HiddenField)e.Row.FindControl("hdnUser");
            if (lblUserName != null && !String.IsNullOrEmpty(hdnUser.Value))
                lblUserName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Convert.ToString(hdnUser.Value).ToLower());
        }
    }

    protected void gvPendingUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Sorting":
                    if (this.SortExp.ToString() == e.CommandArgument.ToString())
                    {
                        if (this.SortOrder == Incentex.DAL.Common.DAEnums.SortOrderType.Asc)
                            this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Desc;
                        else
                            this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                    }
                    else
                    {
                        this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                        this.SortExp = (RegistrationRepository.UsersSortExpType)Enum.Parse(typeof(RegistrationRepository.UsersSortExpType), e.CommandArgument.ToString());
                    }
                    BindPendingUsersGrid();
                    break;

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Method's

    private void ProcessRequest(String status, String registrationID)
    {
        try
        {
            switch (status)
            {
                case "Approve":
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "GeneralAlertMsg('" + new WSUser().ApproveUser(registrationID) + "');", true); 
                    break;

                case "Reject":
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "GeneralAlertMsg('" + new WSUser().RejectUser(registrationID) + "');", true);
                    break;
                case "EditApproveProfile":
                    ClientScript.RegisterStartupScript(this.GetType(), "EditProfile", "ShowUserBasicInfo('" + registrationID + "')", true);
                    break;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    /// <summary>
    /// Approve all pending user 
    /// </summary>
    private void ApproveALL()
    {
        try
        {
            foreach (GridViewRow mGrid in gvPendingUsers.Rows)
            {
                Label lblRegistraionID = (Label)mGrid.FindControl("lblRegistraionID");
                new WSUser().ApproveUser(Convert.ToString(lblRegistraionID.Text));
            }
            BindPendingUsersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Only Approve selected user's
    /// </summary>
    private void ApproveSelected()
    {
        try
        {
            Boolean IsAnyRowChecked = false;
            foreach (GridViewRow mGrid in gvPendingUsers.Rows)
            {
                CheckBox chkSelectOrder = (CheckBox)mGrid.FindControl("chkSelectOrder");
                Label lblRegistraionID = (Label)mGrid.FindControl("lblRegistraionID");
                if (chkSelectOrder.Checked && !String.IsNullOrEmpty(lblRegistraionID.Text))
                {
                    IsAnyRowChecked = true;
                    new WSUser().ApproveUser(Convert.ToString(lblRegistraionID.Text));
                }
            }
            if (!IsAnyRowChecked)
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "GeneralAlertMsg('Please select order to approve!! ');", true);

            BindPendingUsersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void CancelALL(String ReasonMsg)
    {
        try
        {
            foreach (GridViewRow mGrid in gvPendingUsers.Rows)
            {
                Label lblRegistraionID = (Label)mGrid.FindControl("lblRegistraionID");
                new WSUser().RejectUser(Convert.ToString(lblRegistraionID.Text));

            }
            BindPendingUsersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// Binds the pending users grid.
    /// Bind Paging datasource and set paging property
    /// </summary>
    private void BindPendingUsersGrid()
    {
        CompanyEmployeeRepository objCompanyEmployeeRepos = new CompanyEmployeeRepository();
        var objPendingUserList = objCompanyEmployeeRepos.GetPendingUsersList(IncentexGlobal.CurrentMember.UserInfoID, this.SortExp, this.SortOrder).ToList();

        if (objPendingUserList.Count == 0)
            pagingtable.Visible = false;
        else
            pagingtable.Visible = true;

        pds.DataSource = objPendingUserList;
        pds.AllowPaging = true;
        if (IsViewAllTotal)
        {
            CurrentPage = 0;
            pds.PageSize = objPendingUserList.Count;
        }
        else
            pds.PageSize = 15;//Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gvPendingUsers.DataSource = pds;
        gvPendingUsers.DataBind();
        doPaging();
        // Display Total Count 
        lblPendingCount.Text = String.Format("Total Pending Orders : {0}", Convert.ToString(objPendingUserList.Count));
    }

    #endregion

    #region Paging


    protected void dtlPaging_ItemCommand(Object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "lnkbtnPaging")
            {
                this.IsViewAllTotal = false;
                CurrentPage = Convert.ToInt16(e.CommandArgument);
                BindPendingUsersGrid();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dtlPaging_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");

            if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
            {
                lnkbtnPage.Enabled = false;
                lnkbtnPage.Font.Bold = true;
                lnkbtnPage.CssClass = String.Empty;
                lnkbtnPage.Font.Size = new FontUnit(12);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public Int32 CurrentPage
    {
        get
        {
            return Convert.ToInt32(ViewState["CurrentPage"]);
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }

    public Int32 FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt32(ViewState["FrmPg"]);
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }

    public Int32 ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return 3;
            else
                return Convert.ToInt32(ViewState["ToPg"]);
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }

    private void doPaging()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            Int32 CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + 3;
            }
            else if (!IsViewAllTotal && CurrentPg == ToPg)// this will set if when user will click page index
            {
                ToPg = 3;
            }

            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - 3;
            }

            if (pds.PageCount < ToPg)
                ToPg = pds.PageCount;

            for (Int32 i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }
            // Bottom
            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkViewAll_Click(Object sender, EventArgs e)
    {
        try
        {
            this.IsViewAllTotal = true;
            BindPendingUsersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnPrevious_Click(Object sender, EventArgs e)
    {
        try
        {
            this.IsViewAllTotal = false;
            CurrentPage -= 1;
            BindPendingUsersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnNext_Click(Object sender, EventArgs e)
    {
        try
        {
            this.IsViewAllTotal = false;
            CurrentPage += 1;
            BindPendingUsersGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}
