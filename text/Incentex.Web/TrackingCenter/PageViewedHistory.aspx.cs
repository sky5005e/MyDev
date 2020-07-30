using System;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DA;

public partial class TrackingCenter_PageViewedHistory : PageBase
{
    #region Properties
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
    public string Pageurl
    {
        get
        {
            if (this.ViewState["Pageurl"] == null)
                return null;
            else
                return (this.ViewState["Pageurl"].ToString());
        }
        set
        {
            this.ViewState["Pageurl"] = value;
        }
    }
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
    PagedDataSource pds = new PagedDataSource();
    #endregion

    #region event Handlers & Methods
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
            
            ((Label)Master.FindControl("lblPageHeading")).Text = "Page Viewed History";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/TrackingCenter/ReportSelection.aspx";

            if (Request.QueryString["sdate"] != null) { BindGird(); }
        }
    }

    protected void gvPageViewedHistory_RowCommand1(object sender, GridViewCommandEventArgs e)
    {
        try
        {


            //Label lblID = e.Row.FindControl("lblID") as Label;
            if (e.CommandName == "Sorting")
            {

                LinkButton lnkUserList;
                GridViewRow row;
                row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                lnkUserList = (LinkButton)(gvPageViewedHistory.Rows[row.RowIndex].FindControl("lnkUserList"));
                //string pageurl = lnkUserList.ToolTip;
                string pageurl = lnkUserList.ToolTip;
                this.sdate = Convert.ToDateTime(txtStartPageview.Text);
                this.edate = Convert.ToDateTime(txtEndPageview.Text);
                Session["PageNameUrl"] = pageurl;
                Response.Redirect("PageAccessList.aspx?Pageurl=" + pageurl + "&sdate=" + Convert.ToDateTime(this.sdate) + "&edate=" + Convert.ToDateTime(this.edate));

            }
        }
        catch (Exception ex)
        {
            ex = null;
        }
    }

    protected void lnkBtnPageview_Click(object sender, EventArgs e)
    {
        //gvPageViewedHistory
        try
        {
            DateTime StartDate = Convert.ToDateTime(txtStartPageview.Text);
            DateTime EndDate = Convert.ToDateTime(txtEndPageview.Text);
            if ((StartDate < EndDate) || (StartDate == EndDate))
            {
                LblError.Visible = false;
                BindGird();

            }
        }
        catch (Exception ex)
        {

            ex = null;
            LblError.Visible = true;
            LblError.Text = "Please enter both the dates";
        }

    }

    /// <summary>
    /// entering both the dates user can get pagewised history.top ten pages 
    /// </summary>
    public void BindGird()
    {
        DataView mydataview = new DataView();
        UserTrackingDA ObjDA = new UserTrackingDA();
        DataSet DsTracking = new DataSet();
        DsTracking = ObjDA.GetTop10PagesViewed1(Convert.ToDateTime(txtStartPageview.Text), Convert.ToDateTime(txtEndPageview.Text), Convert.ToInt32(TxtNosOfRecord.Text));
        mydataview = DsTracking.Tables[0].DefaultView;
        pds.DataSource = mydataview;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gvPageViewedHistory.DataSource = pds;
        gvPageViewedHistory.DataBind();
        if (DsTracking.Tables.Count == 0)
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
    
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        gvPageViewedHistory.PageIndex = CurrentPage;
        BindGird();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        gvPageViewedHistory.PageIndex = CurrentPage;
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
