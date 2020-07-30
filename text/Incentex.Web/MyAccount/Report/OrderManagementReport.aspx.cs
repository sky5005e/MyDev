using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class MyAccount_Report_OrderManagementReport : PageBase
{
    #region Data Members
    ReportForEmployeeRepository objReportForEmployeeRepository = new ReportForEmployeeRepository();
    ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();
    LookupRepository objLookupRepos = new LookupRepository();
    PagedDataSource pds = new PagedDataSource();
    String SubReport
    {
        get
        {
            if (ViewState["SubReport"] == null)
            {
                ViewState["SubReport"] = null;
            }
            return Convert.ToString(ViewState["SubReport"]);
        }
        set
        {
            ViewState["SubReport"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Service Level Scorecard Dashboard";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/report/ReportDashBoard.aspx";
            FillBasedStation();
            FillWorkgroup();
            FillGender();
            FillPeriod();
            ddlPeriod.Items.Insert(5, new ListItem("Current Year", System.Data.Linq.SqlClient.SqlMethods.DateDiffDay(Convert.ToDateTime("01/01/" + DateTime.Now.Year), System.DateTime.Now).ToString()));//Add new item for current year

            //this is for setting search criteria
            if (Request.QueryString["FromDate"] != null)
                txtFromDate.Text = Request.QueryString["FromDate"];
            if (Request.QueryString["ToDate"] != null)
                txtToDate.Text = Request.QueryString["ToDate"];
            if (string.IsNullOrEmpty(Request.QueryString["Period"]))
            {
                trFromDate.Visible = true;
                trToDate.Visible = true;
                ddlPeriod.SelectedValue = "99999";
            }
            else
            {
                ddlPeriod.SelectedValue = Request.QueryString["Period"];
                if (Convert.ToInt64(ddlPeriod.SelectedValue) < 367)
                {
                    txtFromDate.Text = DateTime.Now.AddDays(-Convert.ToInt64(ddlPeriod.SelectedValue)).ToString("MM/dd/yyyy");
                    txtToDate.Text = string.Empty;
                }
                else
                {
                    txtFromDate.Text = "01/01/" + ddlPeriod.SelectedValue;
                    txtToDate.Text = "12/31/" + ddlPeriod.SelectedValue;
                }
            }
            GenerateChart();
        }
    }
    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        if (ddlPeriod.SelectedValue == "99999")//This is for date range
        {
            trFromDate.Visible = true;
            trToDate.Visible = true;
        }
        else
        {
            if (Convert.ToInt64(ddlPeriod.SelectedValue) < 367)
                txtFromDate.Text = DateTime.Now.AddDays(-Convert.ToInt64(ddlPeriod.SelectedValue)).ToString("MM/dd/yyyy");
            else
            {
                txtFromDate.Text = "01/01/" + ddlPeriod.SelectedValue;
                txtToDate.Text = "12/31/" + ddlPeriod.SelectedValue;
            }
            trFromDate.Visible = false;
            trToDate.Visible = false;
        }
        GenerateChart();
    }
    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {
        GenerateChart();
    }
    protected void chkEDPReceived_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkEDPChecked = (CheckBox)sender;
        HiddenField hdnOrderID = (HiddenField)chkEDPChecked.Parent.Parent.Parent.FindControl("hdnOrderID");
        if (hdnOrderID != null)
        {
            OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
            Order objOrder = objOrderConfirmationRepository.GetByOrderID(Convert.ToInt64(hdnOrderID.Value));
            objOrder.IsEDPReceived = chkEDPChecked.Checked ? true : false;
            objOrderConfirmationRepository.SubmitChanges();
        }
        GenerateChart();
    }
    protected void gvEmployeePayrollDeduct_RowCommand(object sender, GridViewCommandEventArgs e)
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
        if (e.CommandName.Equals("OrderDetail"))
        {
            IncentexGlobal.ManageID = 9;
            IncentexGlobal.OrderReturnURL = Request.Url.ToString() + "&WorkGroup=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : "") + "&StationCode=" + (ddlBasestation.SelectedValue != "0" ? ddlBasestation.SelectedValue : "");
            Response.Redirect("~/OrderManagement/OrderDetail.aspx?Id=" + e.CommandArgument);
        }
    }
    protected void gvEmployeePayrollDeduct_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkEDPReceived = (CheckBox)e.Row.FindControl("chkEDPReceived");
            HtmlControl spnEDPReceived = (HtmlControl)e.Row.FindControl("spnEDPReceived");
            HiddenField hdnEDPReceived = (HiddenField)e.Row.FindControl("hdnEDPReceived");
            if (hdnEDPReceived != null && !string.IsNullOrEmpty(hdnEDPReceived.Value))
            {
                if (Convert.ToBoolean(hdnEDPReceived.Value) == true)
                {
                    chkEDPReceived.Checked = true;
                    spnEDPReceived.Attributes.Add("class", "custom-checkbox_checked");
                }
                else
                {
                    chkEDPReceived.Checked = false;
                    spnEDPReceived.Attributes.Add("class", "custom-checkbox");
                }
            }
        }
    }
    #endregion

    #region Methods

    private void FillBasedStation()
    {
        List<INC_BasedStation> basestationList = objReportAccessRightsRepository.GetBaseStationByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 2221);
        if (basestationList != null)
        {
            ddlBasestation.DataSource = basestationList.OrderBy(x => x.sBaseStation);
            ddlBasestation.DataValueField = "iBaseStationId";
            ddlBasestation.DataTextField = "sBaseStation";
            ddlBasestation.DataBind();
        }
        ddlBasestation.Items.Insert(0, new ListItem("-Select Basestation-", "0"));
    }

    private void FillWorkgroup()
    {
        List<INC_Lookup> WorkgroupList = objReportAccessRightsRepository.GetWorkgroupByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 2221);
        if (WorkgroupList != null)
        {
            ddlWorkgroup.DataSource = WorkgroupList.OrderBy(x => x.sLookupName);
            ddlWorkgroup.DataValueField = "iLookupID";
            ddlWorkgroup.DataTextField = "sLookupName";
            ddlWorkgroup.DataBind();
        }
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select Workgroup-", "0"));
    }

    private void FillGender()
    {
        ddlGender.DataSource = objLookupRepos.GetByLookup("Gender").OrderBy(x => x.sLookupName);
        ddlGender.DataValueField = "iLookupID";
        ddlGender.DataTextField = "sLookupName";
        ddlGender.DataBind();
        ddlGender.Items.Insert(0, new ListItem("-Select Gender-", "0"));
    }
    private void FillPeriod()
    {
        ddlPeriod.ClearSelection();
        ddlPeriod.Items.Clear();
        ddlPeriod.DataSource = Common.BindPeriodDropDownItems();
        ddlPeriod.DataValueField = "Value";
        ddlPeriod.DataTextField = "Text";
        ddlPeriod.DataBind();
    }
    protected void GenerateChart()
    {
        DateTime? FromDate = null;
        DateTime? ToDate = null;
        Int64? StoreID = null;
        Int64? WorkgroupID = null;
        Int64? GenderID = null;
        Int64? BaseStationID = null;

        if (txtFromDate.Text != "")
            FromDate = Convert.ToDateTime(txtFromDate.Text.Trim());
        if (txtToDate.Text != "")
            ToDate = Convert.ToDateTime(txtToDate.Text.Trim());
        if (ddlWorkgroup.SelectedValue != "0")
            WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
        if (ddlBasestation.SelectedValue != "0")
            BaseStationID = Convert.ToInt64(ddlBasestation.SelectedValue);
        if (ddlGender.SelectedValue != "0")
            GenderID = Convert.ToInt64(ddlGender.SelectedValue);
        StoreID = new CompanyStoreRepository().GetStoreIDByCompanyId((Int64)IncentexGlobal.CurrentMember.CompanyId);


        if (Request.QueryString["SubReport"] != null)
        {
            SubReport = Request.QueryString["SubReport"].ToString();
        }
        if (SubReport != null)
        {
            if (SubReport == "Order Status Snapshot")
            {
                List<ReportForEmployeeRepository.GetOrderAtAGlanceWiseResult> objResult = objReportForEmployeeRepository.GetOrderAtAGlanceWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    objResult = objResult.Where(x => x.Status != "Canceled" && x.Status != "Deleted").ToList();
                    chrtOrderAtAGlance.DataSource = objResult;
                    chrtOrderAtAGlance.DataBind();
                }
                dvOrderAtAGlance.Visible = true;
            }
            else if (SubReport == "Employee Payroll Deduct")
            {
                List<ReportForEmployeeRepository.GetEmployeePayrollDeductWiseResult> objResult = objReportForEmployeeRepository.GetEmployeePayrollDeductWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);

                if (objResult != null && objResult.Count > 0)
                {
                    DataView myDataView = new DataView();
                    DataTable dataTable = ListToDataTable(objResult);
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
                    gvEmployeePayrollDeduct.DataSource = pds;
                    gvEmployeePayrollDeduct.DataBind();

                    pagingtable.Visible = true;
                    lblRecordCounter.Text = objResult.Count.ToString();
                    doPaging();
                }
                else
                {
                    pagingtable.Visible = false;
                }
                dvEmployeePayrollDeduct.Visible = true;
            }
            else if (SubReport == "Back Order Report")
            {
                List<SelectBackOrderReportResult> objResult = objReportForEmployeeRepository.GetBackOrderReport(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);

                if (objResult != null && objResult.Count > 0)
                {
                    //Getting Order Detail only 
                    var orderResult = (from result in objResult
                                       select new
                                       {
                                           OrderID = result.OrderID,
                                           OrderNumber = result.OrderNumber,
                                           CompanyName = result.CompanyName,
                                           OrderDate = result.OrderDate,
                                           Name = result.Name
                                       }).Distinct().ToList();

                    rpBackOrderReport.DataSource = orderResult;
                    rpBackOrderReport.DataBind();

                    lblBackOrderCount.Text = orderResult.Count.ToString();

                    foreach (RepeaterItem repeaterItem in rpBackOrderReport.Items)
                    {
                        HiddenField hdnOrderID = (HiddenField)repeaterItem.FindControl("hdnOrderID");

                        //Get item detail based on order id
                        var orderItemResult = (from result in objResult
                                               where result.OrderID == Convert.ToInt64(hdnOrderID.Value) && result.MyShoppingCartID != null
                                               select new
                                               {
                                                   ItemNumber = result.ItemNumber,
                                                   QuantityOrdered = result.QuantityOrdered,
                                                   Description = result.ProductDescrption,
                                                   UnitPrice = result.UnitPrice,
                                                   Status = result.Status != null ? result.Status : false,
                                                   MyShoppingCartID = result.MyShoppingCartID,
                                                   BackOrderedUntil = result.BackOrderedUntil,
                                                   IsShippedComplete = (((result.QuantityOrdered - (result.QuantityShipped != null ? result.QuantityShipped : 0)) > 0) ? false : true),
                                                   Inventory = result.Inventory,
                                                   QuantityShipped = result.QuantityShipped
                                               }).OrderBy(x => x.QuantityShipped >= x.QuantityOrdered).ThenBy(x => x.QuantityShipped != 0).ThenBy(x => x.Inventory).ToList();
                        var backorderresult = orderItemResult.Where(o => o.Inventory != null && o.Inventory <= 0 && !o.IsShippedComplete).ToList();
                        GridView grdItemDetail = ((GridView)(repeaterItem.FindControl("grdItemDetail")));
                        grdItemDetail.DataSource = backorderresult;
                        grdItemDetail.DataBind();

                    }
                }
                dvBackOrder.Visible = true;
            }

        }
    }
    #endregion

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
}
