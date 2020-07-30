using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Web.UI.DataVisualization.Charting;
using System.Data;

public partial class MyAccount_Report_ProductPlanningReport : PageBase
{
    #region Data Members
    ReportForEmployeeRepository objReportForEmployeeRepository = new ReportForEmployeeRepository();
    ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();
    LookupRepository objLookupRepos = new LookupRepository();
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
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Planning Dashboard";
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
    protected void gvCustomerOwnedInventory_RowCommand(object sender, GridViewCommandEventArgs e)
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
    }
    protected void gvCustomerOwnedInventory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnTotalNumber = (HiddenField)e.Row.FindControl("hdnTotalNumber");
            HiddenField hdnRowNumber = (HiddenField)e.Row.FindControl("hdnRowNumber");
            if (!string.IsNullOrEmpty(hdnTotalNumber.Value) && !string.IsNullOrEmpty(hdnRowNumber.Value) && hdnTotalNumber.Value == hdnRowNumber.Value)
            {
                e.Row.Attributes.CssStyle.Add("border-bottom", "1px solid grey");
            }
        }
    }
    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {
        GenerateChart();
    }
    #endregion

    #region Methods

    private void FillBasedStation()
    {
        List<INC_BasedStation> basestationList = objReportAccessRightsRepository.GetBaseStationByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 452);
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
        List<INC_Lookup> WorkgroupList = objReportAccessRightsRepository.GetWorkgroupByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 452);
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
        if (Request.QueryString["SubReport"] != null && Request.QueryString["SubReport"] == "Customer Owned Inventory")
            ddlGender.DataSource = objLookupRepos.GetByLookup("GarmentType ").OrderBy(x => x.sLookupName);
        else
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
            if (SubReport == "Purchase History by Size")
            {
                /// <summary>
                /// This report is dislay the total number of unit sold based on size.
                /// </summary>
                List<ReportForEmployeeRepository.GetProductSizeWiseResult> objResult = objReportForEmployeeRepository.GetProductSalesSizeWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, null);
                if (objResult != null)
                {
                    objResult = objResult.Where(x => x.Text != "No Size" && x.Text != "One Size").ToList();
                    chrtProductSalesByFemaleSize.DataSource = objResult.OrderBy(x => x.Value);
                    chrtProductSalesByFemaleSize.DataBind();
                }
                dvProductSalesByFemaleSize.Visible = true;

            }
            else if (SubReport == "Purchase History by Color")
            {
                /// <summary>
                /// this report display item size report based on color unit sold
                /// </summary>
                List<ReportForEmployeeRepository.GetProductSizeWiseResult> objResult = objReportForEmployeeRepository.GetProductSalesColorWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtProductSalesByColor.DataSource = objResult;
                    chrtProductSalesByColor.DataBind();

                    try
                    {
                        for (int i = 0; i < chrtProductSalesByColor.Series["Series1"].Points.Count; i++)
                        {
                            if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Navy Blue")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.Color.Navy;
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Bright Red")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml("#D5000F");
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Royal Blue")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.Color.RoyalBlue;
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Bright Yellow")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml("#FFC600");
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "White")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.Color.White;
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Bright Orange")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml("#FF8300");
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Black")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml("#4A4A4B");
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Matte Silver")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.Color.Silver;
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Periwinkle Blue")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml("#5B4EBA");
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Brown")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.Color.Brown;
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Light Blue")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.Color.LightSkyBlue;
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Gray")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.Color.Gray;
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Lime Green")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.Color.LimeGreen;
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Midnight Blue")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.Color.MidnightBlue;
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Nude")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml("#C9DAAD");
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Brass")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml("#CD9575");
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Clear")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                            else if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtProductSalesByColor.Series["Series1"].Points[i])).AxisLabel == "Pattern")
                                chrtProductSalesByColor.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml("#775A91");
                        }
                    }
                    catch
                    {
                    }
                }
                dvProductSalesByColor.Visible = true;
            }
            else if (SubReport == "Top 50 Items Purchased")
            {
                /// <summary>
                /// This report will show a listing of the top 50 selling items by revenue based on Master Item level data.
                /// This report will have two bars for each master item it will have a green bar to show the total revenue for the item and a blue bar for the total units sold.
                /// </summary>

                List<ReportForEmployeeRepository.GetTopFiftyItemByRevenueResult> objResult = objReportForEmployeeRepository.GetTopFiftyItemByRevenue(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    objResult = objResult.OrderByDescending(x => x.Value).Take(50).OrderBy(x => x.Value).ToList();
                    chrtTopFiftyItemsByRevenue.Series["SeriesRevenue"].Points.DataBindXY(objResult, "Text", objResult, "Value");
                    chrtTopFiftyItemsByRevenue.Series["SeriesUnit"].Points.DataBindXY(objResult, "Text", objResult, "Value1");
                }
                dvTopFiftyItemsByRevenue.Visible = true;
            }
            else if (SubReport == "Back Orders Items")
            {
                /// <summary>
                /// This report will have two bars coming out from the side for each master item. 
                /// The first bar will be blue and will show the total units sold for that master item. 
                /// Then the second bar will be red and will show the total number of units for that master item that have been ordered but not shipped.
                /// This report will show a long list of all the master item numbers that are active.
                /// </summary>
                List<ReportForEmployeeRepository.GetBackOrderItemResult> objResult = objReportForEmployeeRepository.GetBackOrderItemReport(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    objResult = objResult.OrderBy(x => x.Value).ToList();
                    chrtBackOrderInventoryReport.Series["SeriesTotalOrder"].Points.DataBindXY(objResult, "Text", objResult, "Value");
                    chrtBackOrderInventoryReport.Series["SeriesNotShipped"].Points.DataBindXY(objResult, "Text", objResult, "Value1");
                }
                dvBackOrderInventoryReport.Visible = true;
            }
            else if (SubReport == "Item History Snapshot")
            {
                /// <summary>
                ///This will be a bar graph coming out from the side. 
                ///Since we are selecting the master item number the report will show each item by size. 
                ///Each item will have four bars: (Blue) for current stock, (Red) for the last 12 months usage, 
                ///(Purple) will show items on back-order which would be items not shipped. 
                ///(Yellow) for re-order level, (Green) for units on order.
                /// </summary>

                List<ReportForEmployeeRepository.GetInventoryUsageReportResult> objResult = objReportForEmployeeRepository.GetInventoryUsageReport(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, Convert.ToInt64(Request.QueryString["MasterItemID"]));
                if (objResult != null)
                {
                    objResult = objResult.OrderBy(x => x.Sold).ToList();
                    chrtInventoryByItem.Series["SeriesOnHand"].Points.DataBindXY(objResult, "Text", objResult, "OnHand");
                    chrtInventoryByItem.Series["SeriesSold"].Points.DataBindXY(objResult, "Text", objResult, "Sold");
                    chrtInventoryByItem.Series["SeriesOnOrder"].Points.DataBindXY(objResult, "Text", objResult, "OnOrder");
                    chrtInventoryByItem.Series["SeriesReOrder"].Points.DataBindXY(objResult, "Text", objResult, "ReOrder");
                    chrtInventoryByItem.Series["SeriesReturn"].Points.DataBindXY(objResult, "Text", objResult, "Return");
                }
                dvInventoryByItem.Visible = true;
            }
            else if (SubReport == "Customer Owned Inventory")
            {
                trFromDate.Visible = false;
                trToDate.Visible = false;
                trPeriod.Visible = false;
                
                List<SelectSalesByInventoryStatusResult> objResult = objReportForEmployeeRepository.GetProductSalesInventoryStatusWise(IncentexGlobal.CurrentMember.UserInfoID, StoreID, WorkgroupID, GenderID, 245);

                if (objResult != null && objResult.Count > 0)
                {
                    DataView myDataView = new DataView();
                    DataTable dataTable = ListToDataTable(objResult);
                    myDataView = dataTable.DefaultView;
                    if (this.ViewState["SortExp"] != null)
                    {
                        myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
                    }
                    gvCustomerOwnedInventory.DataSource = myDataView;
                    gvCustomerOwnedInventory.DataBind();
                    lblTotalOnHand.Text = objResult.Sum(x => x.OnHand).ToString();
                    lblTotalPrice.Text = objResult.Sum(x => x.Price).ToString();
                    lblTotalUsage.Text = objResult.Sum(x => x.Usage).ToString();
                    lblTotalDrawDownValue.Text = objResult.Sum(x => x.DrawDownValue).ToString();
                    lblTotalOnHandValue.Text = objResult.Sum(x => x.OnHandInventoryValue).ToString();
                }
                else
                {
                    gvCustomerOwnedInventory.DataSource = null;
                    gvCustomerOwnedInventory.DataBind();
                    lblTotalOnHand.Text = "0";
                    lblTotalPrice.Text = "0";
                    lblTotalUsage.Text = "0";
                    lblTotalDrawDownValue.Text = "0";
                    lblTotalOnHandValue.Text = "0";
                }
                dvCustomerOwnedInventory.Visible = true;
            }
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
