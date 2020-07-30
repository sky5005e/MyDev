using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using System.Data;

public partial class admin_Report_ItemWithBackOrderedGridReport : PageBase
{
    #region Data Members
    PagedDataSource pds = new PagedDataSource();
    ReportRepository objReportRepository = new ReportRepository();
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
                return 5;
            else
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();

            ((Label)Master.FindControl("lblPageHeading")).Text = "Items with Backorders Report";
            if (Request.QueryString["ChartSubReport"] != null)
            {
                String ReturnUrl = string.Empty;
                ReturnUrl = "~/admin/Report/ProductPlanningReport.aspx";

                //Now combine all querystring with returnurl
                for (int i = 0; i < Request.QueryString.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString[i]))
                    {
                        if (i == 0)
                            ReturnUrl += "?SubReport";
                        else
                            ReturnUrl += "&" + Request.QueryString.AllKeys[i];

                        ReturnUrl += "=" + Request.QueryString[i].ToString();
                    }
                }
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = ReturnUrl;
            }
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Report/ReportDashboard.aspx";
            BindGrid();
        }
    }

    #region GridView Events
    protected void gvItemswithBackorders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Sort"))
        {
            if (this.ViewState["SortExp"] == null)
            {
                this.ViewState["SortExp"] = e.CommandArgument.ToString();
                this.ViewState["SortOrder"] = "ASC";
            }
            else
            {
                if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
                {
                    if (this.ViewState["SortOrder"].ToString() == "ASC")
                        this.ViewState["SortOrder"] = "DESC";
                    else
                        this.ViewState["SortOrder"] = "ASC";

                }
                else
                {
                    this.ViewState["SortOrder"] = "ASC";
                    this.ViewState["SortExp"] = e.CommandArgument.ToString();
                }
            }
        }
        BindGrid();

    }
    #endregion
    #endregion

    #region Methods
    public void BindGrid()
    {
        Int64? StoreID = null;
        Int64? WorkgroupID = null;
        Int64? GenderID = null;
        Int64? BaseStationID = null;
        Int64? UserInfoID = null;

        if (!string.IsNullOrEmpty(Request.QueryString["StoreID"]))
            StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
        if (!string.IsNullOrEmpty(Request.QueryString["Workgroup"]))
            WorkgroupID = Convert.ToInt64(Request.QueryString["Workgroup"]);
        if (!string.IsNullOrEmpty(Request.QueryString["BaseStation"]))
            BaseStationID = Convert.ToInt64(Request.QueryString["BaseStation"]);
        if (!string.IsNullOrEmpty(Request.QueryString["Gender"]))
            GenderID = Convert.ToInt64(Request.QueryString["Gender"]);
        if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;

        DataView myDataView = new DataView();
        List<ReportRepository.GetItemswithBackordersResult> objResult = objReportRepository.GetItemswithBackorders(UserInfoID, StoreID, WorkgroupID, BaseStationID, GenderID);

        if (objResult.Count == 0)
            pagingtable.Visible = false;
        else
        {
            lblTotalItems.Text = Convert.ToString(objResult.Count);
            lblTotalValues.Text = Convert.ToString(objResult.Sum(x => x.BackOrdered));
            pagingtable.Visible = true;
        }

        DataTable dataTable = ListToDataTable(objResult);
        myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = 1000;//Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gvItemswithBackorders.DataSource = pds;
        gvItemswithBackorders.DataBind();
        doPaging();
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }
    #endregion

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGrid();
        }
    }
    protected void dtlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
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
                ToPg = ToPg + 5;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - 5;

            }

            if (pds.PageCount < ToPg)
            {
                ToPg = pds.PageCount;
            }

            for (int i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();

        }
        catch (Exception)
        { }
    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindGrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGrid();
    }
    #endregion
}
