using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;


public partial class admin_Report_PendingOrderList : PageBase
{
    #region Data Members
    ReportRepository objReportRepository = new ReportRepository();
    LookupRepository objLookupRepos = new LookupRepository();
    PagedDataSource pds = new PagedDataSource();
    ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();

    List<GetPendingOrdersListResult> lstPendingOrders = new List<GetPendingOrdersListResult>();
    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Pending Orders";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Report/ReportDashBoard.aspx";
            IncentexGlobal.ManageID = 9;
            FillCompanyStore();

            //this is for setting search criteria
            if (Request.QueryString["FromDate"] != null)
                txtFromDate.Text = Request.QueryString["FromDate"];
            if (Request.QueryString["ToDate"] != null)
                txtToDate.Text = Request.QueryString["ToDate"];
            if (Request.QueryString["StoreID"] != null)
                ddlCompanyStore.SelectedValue = Request.QueryString["StoreID"];
            GenerateChart();
        }
    }
    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {
        GenerateChart();
    }

    protected void gvPendingOrders_RowCommand(object sender, GridViewCommandEventArgs e)
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
            GenerateChart();
        }
        //if (e.CommandName.Equals("OrderDetail"))
        //{
        //    IncentexGlobal.OrderReturnURL = Request.Url.ToString() + "&WorkGroup=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : "") + "&StationCode=" + (ddlBasestation.SelectedValue != "0" ? ddlBasestation.SelectedValue : "");
        //    Response.Redirect("~/OrderManagement/OrderDetail.aspx?Id=" + e.CommandArgument);
        //}
    }

    protected void gvPendingOrders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Repeater rpApprovers = (Repeater)e.Row.FindControl("rpApprovers");
            HiddenField hdnOrderID = (HiddenField)e.Row.FindControl("hdnOrderID");
            if (rpApprovers != null && hdnOrderID != null)
            {
                rpApprovers.DataSource = lstPendingOrders.Where(p => p.OrderID == Convert.ToInt64(hdnOrderID.Value)).ToList();
                rpApprovers.DataBind();
            }
        }
    }

    #endregion

    #region Methods

    private void FillCompanyStore()
    {
        try
        {
            List<SelectCompanyStoreNameResult> objCompanyList = new List<SelectCompanyStoreNameResult>();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
                objCompanyList = new OrderConfirmationRepository().GetCompanyStoreName();
            else
                objCompanyList = objReportAccessRightsRepository.GetStoreByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 2221);

            if (objCompanyList != null)
            {
                ddlCompanyStore.DataSource = objCompanyList.OrderBy(x => x.CompanyName);
                ddlCompanyStore.DataValueField = "StoreID";
                ddlCompanyStore.DataTextField = "CompanyName";
                ddlCompanyStore.DataBind();
            }
            ddlCompanyStore.Items.Insert(0, new ListItem("-Select Store-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }


    protected void GenerateChart()
    {
        DateTime? FromDate = null;
        DateTime? ToDate = null;
        Int64? StoreID = null;
        Int64? ApproverUserInfoID = null;

        if (txtFromDate.Text != "")
            FromDate = Convert.ToDateTime(txtFromDate.Text.Trim());
        if (txtToDate.Text != "")
            ToDate = Convert.ToDateTime(txtToDate.Text.Trim());
        if (ddlCompanyStore.SelectedIndex > 0)
            StoreID = Convert.ToInt64(ddlCompanyStore.SelectedValue);
        if (ddlApprovers.SelectedIndex > 0)
            ApproverUserInfoID = Convert.ToInt64(ddlApprovers.SelectedValue);



        lstPendingOrders = objReportRepository.GetPendingOrderList(FromDate, ToDate, ApproverUserInfoID, StoreID);

        if (lstPendingOrders != null && lstPendingOrders.Count > 0)
        {
            DataView myDataView = new DataView();
            DataTable dataTable = ListToDataTable(lstPendingOrders);
            myDataView = dataTable.DefaultView;
            if (this.ViewState["SortExp"] != null)
            {
                myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
            }
            pds.DataSource = myDataView;
            pds.AllowPaging = true;
            pds.PageSize = 1000; //As per Ken told Convert.ToInt32(Application["GRIDPAGING"]);
            pds.CurrentPageIndex = CurrentPage;
            lnkbtnNext.Enabled = !pds.IsLastPage;
            lnkbtnPrevious.Enabled = !pds.IsFirstPage;
            gvPendingOrders.DataSource = pds;
            gvPendingOrders.DataBind();

            pagingtable.Visible = true;
            lblRecordCounter.Text = lstPendingOrders.Count.ToString();
            doPaging();

            var lstApprovers = lstPendingOrders.Select(x => new { x.ApproverName, x.ApproverUserInfoID }).Distinct().ToList();
            ddlApprovers.Items.Clear();
            ddlApprovers.DataSource = lstApprovers;
            ddlApprovers.DataValueField = "ApproverUserInfoID";
            ddlApprovers.DataTextField = "ApproverName";
            ddlApprovers.DataBind();
            ddlApprovers.Items.Insert(0, new ListItem("- Select Approvers -", "0"));
        }
        else
        {
            gvPendingOrders.DataSource = lstPendingOrders;
            gvPendingOrders.DataBind();
            pagingtable.Visible = false;
            lblRecordCounter.Text = "0";
        }
        dvPendingOrders.Visible = true;
    }

    #region Pagging Methods
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

            DataList2.DataSource = dt;
            DataList2.DataBind();

        }
        catch (Exception)
        { }

    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        GenerateChart();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        GenerateChart();
    }
    protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            GenerateChart();
        }
    }

    protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);
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

    #endregion
}
