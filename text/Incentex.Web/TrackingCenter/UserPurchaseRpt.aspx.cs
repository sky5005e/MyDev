using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;

public partial class TrackingCenter_UserPurchaseRpt : PageBase
{
    #region Properties

    public int CurrentPage
    {
        get
        {
            if (this.ViewState["CurrentPage"] == null)
                return 0;
            else
                return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }
    public int PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt16(this.ViewState["PagerSize"].ToString());
        }
        set
        {
            this.ViewState["PagerSize"] = value;
        }
    }
    public int FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt16(this.ViewState["FrmPg"].ToString());
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }
    public int ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return PagerSize;
            else
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }
    UserTrackingRepo.UserPurchaseTrackingSortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = UserTrackingRepo.UserPurchaseTrackingSortExpType.CompanyName;
            }
            return (UserTrackingRepo.UserPurchaseTrackingSortExpType)ViewState["SortExp"];
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }
    DAEnums.SortOrderType SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = DAEnums.SortOrderType.Asc;
            }
            return (DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }
    PagedDataSource pds = new PagedDataSource();

    #endregion

    #region Event Handlers & Methods
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Tracking Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "User Purchase Report";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/TrackingCenter/ReportSelection.aspx";
            totalnumber.Visible = false;
            lblmsg.Visible = false;
        }
    }
    
    /// <summary>
    /// by entering both the dates user can view the information IE/CA/CE .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkBtnReportUserPurchase_Click(object sender, EventArgs e)
    {
        try
        {

            DateTime StartDate = Convert.ToDateTime(txtStartDatePurchase.Text);
            DateTime EndDate = Convert.ToDateTime(txtEndDatePurchase.Text);
            if ((StartDate < EndDate) || (StartDate == EndDate))
            {
                LblError.Visible = false;
                UserTrackingRepo ObjTracRepo = new UserTrackingRepo();
                var List = ObjTracRepo.AccessUserListPurchase(StartDate, EndDate);
                pds.DataSource = List;
                pds.AllowPaging = true;
                pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
                pds.CurrentPageIndex = CurrentPage;
                lnkbtnNext.Enabled = !pds.IsLastPage;
                lnkbtnPrevious.Enabled = !pds.IsFirstPage;
                gv.DataSource = pds;
                gv.DataBind();

                if (List.Count == 0)
                {

                    lstPaging.Visible = false;
                    lnkbtnNext.Visible = false;
                    lnkbtnPrevious.Visible = false;
                    pager.Visible = false;
                }
                else
                {

                    lstPaging.Visible = true;
                    lnkbtnNext.Visible = true;
                    lnkbtnPrevious.Visible = true;
                    pager.Visible = true;
                }
                doPaging();
                totalnumber.Visible = true;
                LblTotalUser.Text = Convert.ToString(List.Count);
            }
            else
            {
                LblError.Visible = true;
                LblError.Text = " Start Date Must be less then End date ";
            }

        }
        catch (Exception ex)
        {

            ex = null;
            LblError.Visible = true;
            LblError.Text = " Please enter both the dates  ";
        }

    }
    
    /// <summary>
    /// Row command for the sorting logic and also bind the image related to purchase status.
    /// </summary>
    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Sorting":

                if (this.SortExp.ToString() == e.CommandArgument.ToString())
                {
                    if (this.SortOrder == DAEnums.SortOrderType.Asc)
                        this.SortOrder = DAEnums.SortOrderType.Desc;
                    else
                        this.SortOrder = DAEnums.SortOrderType.Asc;
                }
                else
                {
                    this.SortOrder = DAEnums.SortOrderType.Asc;
                    this.SortExp = (UserTrackingRepo.UserPurchaseTrackingSortExpType)Enum.Parse(typeof(UserTrackingRepo.UserPurchaseTrackingSortExpType), e.CommandArgument.ToString());
                }
                BindGird();
                break;

        }
        if (e.CommandName == "BindHistory")
        {
            GridViewRow row;
            row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
            if (((ImageButton)gv.Rows[row.RowIndex].FindControl("btnAddemp")).Visible==true)
            {
               // BindGird(this.uid, Convert.ToDateTime(this.sd), Convert.ToDateTime(this.ed));
                //BindGird();
                ((ImageButton)gv.Rows[row.RowIndex].FindControl("btnminusemp")).Visible = true;
                ((ImageButton)gv.Rows[row.RowIndex].FindControl("btnAddemp")).Visible = false;
                GridView gvChPageHistory = ((GridView)gv.Rows[row.RowIndex].FindControl("gvChPageHistory"));
                List<UserPageHistoryTracking> objUserPageHistoryTracking = new UserTrackingRepo().GetHistoryByUserTrackingID(Convert.ToInt16(e.CommandArgument));
                if (objUserPageHistoryTracking.Count == 0)
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "No Page history available";
                }
                else
                {
                    gvChPageHistory.DataSource = objUserPageHistoryTracking;
                    gvChPageHistory.DataBind();
                    gvChPageHistory.Visible = true;
                    lblmsg.Visible = false;

                }
            }
            else
            {
                //BindGird(this.uid, Convert.ToDateTime(this.sd), Convert.ToDateTime(this.ed));
                BindGird();
            }
        }
    }

    /// <summary>
    /// if userpurchase then image will be green yes mark else cross sign.
    /// </summary>
    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        Label lblID = e.Row.FindControl("lblID") as Label;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((HiddenField)e.Row.FindControl("hdnStatus")).Value == "True")
            {
                ((Image)e.Row.FindControl("imgNo")).Visible = false;

            }
            else
            {
                ((Image)e.Row.FindControl("imgYes")).Visible = false;

            }
        }

    }

    /// <summary>
    /// This will take start date and end date as parameter and take purchase list.
    /// </summary>
    public void BindGird()
    {
        UserTrackingRepo ObjRepo = new UserTrackingRepo();
        List<UserAccessTrackingForPurchaseResult> objList = ObjRepo.GetPurchaseUserList(Convert.ToDateTime(txtStartDatePurchase.Text), Convert.ToDateTime(txtEndDatePurchase.Text), this.SortExp, this.SortOrder);
        pds.DataSource = objList;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gv.DataSource = pds;
        gv.DataBind();
        if (objList.Count == 0)
        {

            lstPaging.Visible = false;
            lnkbtnNext.Visible = false;
            lnkbtnPrevious.Visible = false;
            pager.Visible = false;
        }
        else
        {

            lstPaging.Visible = true;
            lnkbtnNext.Visible = true;
            lnkbtnPrevious.Visible = true;
            pager.Visible = true;
        }
        doPaging();

    }
    #endregion

    #region Paging
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        gv.PageIndex = CurrentPage;
        BindGird();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        gv.PageIndex = CurrentPage;
        BindGird();
    }
    
    protected void lstPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    private void doPaging()
    {
        try
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            int CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + PagerSize;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - PagerSize;

            }

            if (pds.PageCount < ToPg)
                ToPg = pds.PageCount;

            for (int i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            lstPaging.DataSource = dt;
            lstPaging.DataBind();

        }
        catch (Exception)
        { }

    }
    
    protected void lstPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGird();
            //bindGridView();
        }
    }
    #endregion

}
