using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class TrackingCenter_BrowserType : PageBase
{
    #region Data Members
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
    public DateTime? sdate
    {
        get
        {
            if (this.ViewState["sdate"] == null)
                return null;
            else
                return Convert.ToDateTime(this.ViewState["sdate"].ToString());
        }
        set
        {
            this.ViewState["sdate"] = value;
        }
    }
    public DateTime? edate
    {
        get
        {
            if (this.ViewState["edate"] == null)
                return null;
            else
                return Convert.ToDateTime(this.ViewState["edate"].ToString());
        }
        set
        {
            this.ViewState["edate"] = value;
        }
    }
    public string BrowserName
    {
        get
        {
            if (this.ViewState["BrowserName"] == null)
                return null;
            else
                return (this.ViewState["BrowserName"].ToString());
        }
        set
        {
            this.ViewState["BrowserName"] = value;
        }
    }
    PagedDataSource pds = new PagedDataSource();
    #endregion
    
    #region Event Handlers
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
            
            ((Label)Master.FindControl("lblPageHeading")).Text = "Browser Type Report";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/TrackingCenter/ReportSelection.aspx";
            if (Request.QueryString["sdate"] != null) { BindGird(); }
            LblError.Visible = false;
        }
    }
    
    protected void lnkBtnReportBrowserType_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime StartDate = Convert.ToDateTime(txtStartDateBrowserType.Text);
            DateTime EndDate = Convert.ToDateTime(txtEndDateBrowserType.Text);
            if ((StartDate < EndDate) || (StartDate == EndDate))
            {
                BindGird();
            }
            else 
            {
                lblmsg.Visible = true;
                LblError.Text = "End date must be grater then or equal to start date";
            }
        }
        catch (Exception ex)
        {
            
            ex=null;
            LblError.Visible = true;
            LblError.Text = " Please enter both the dates  ";

        }
    }

    protected void gvBrowserType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Sorting")
            {

                LinkButton lnkUserList;
                GridViewRow row;
                row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                lnkUserList = (LinkButton)(gvBrowserType.Rows[row.RowIndex].FindControl("lnkBrowserName"));
                string BrowserName = lnkUserList.ToolTip;
                this.sdate = Convert.ToDateTime(txtStartDateBrowserType.Text);
                this.edate = Convert.ToDateTime(txtEndDateBrowserType.Text);
                Response.Redirect("BrowserWiseUser.aspx?BrowserName=" + Convert.ToString(BrowserName) + "&sdate=" + Convert.ToDateTime(this.sdate) + "&edate=" + Convert.ToDateTime(this.edate));

            }
        }
        catch (Exception ex)
        {
            ex = null;
        }
    }
    #endregion

    #region Methods
    public void BindGird()
    {
        UserTrackingRepo ObjRepo = new UserTrackingRepo();
        List<GetBrowserTypeCountResult> objList = ObjRepo.GetBrowserConut(Convert.ToDateTime(txtStartDateBrowserType.Text), Convert.ToDateTime(txtEndDateBrowserType.Text));
        pds.DataSource = objList;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gvBrowserType.DataSource = pds;
        gvBrowserType.DataBind();
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
        gvBrowserType.PageIndex = CurrentPage;
        BindGird();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        gvBrowserType.PageIndex = CurrentPage;
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
            
        }
    }
    #endregion
}
