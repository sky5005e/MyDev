using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Report_ProductPlanningReport : PageBase
{
    #region Data Members
    ReportRepository objReportRepository = new ReportRepository();
    LookupRepository objLookupRepos = new LookupRepository();
    ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();
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
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Planning Dashboard";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Report/ReportDashBoard.aspx";
            FillCompanyStore();
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
            if (Request.QueryString["StoreID"] != null)
                ddlCompanyStore.SelectedValue = Request.QueryString["StoreID"];
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
            if (!string.IsNullOrEmpty(Request.QueryString["SubReport"]) && Request.QueryString["SubReport"] == "Summary Product Review")
            {
                if (!string.IsNullOrEmpty(Request.QueryString["WorkGroup"]))
                    ddlWorkgroup.SelectedValue = Request.QueryString["WorkGroup"];
                if (!string.IsNullOrEmpty(Request.QueryString["BaseStation"]))
                    ddlBasestation.SelectedValue = Request.QueryString["BaseStation"];
                if (!string.IsNullOrEmpty(Request.QueryString["Gender"]))
                    ddlGender.SelectedValue = Request.QueryString["Gender"];
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
    protected void gvSummaryProductReview_RowCommand(object sender, GridViewCommandEventArgs e)
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
        if (e.CommandName.Equals("Detail"))
        {
            GridViewRow grdRow = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            HiddenField hdnStoreID = (HiddenField)grdRow.FindControl("hdnStoreID");
            HiddenField hdnWorkgroupName = (HiddenField)grdRow.FindControl("hdnWorkgroupName");
            Session["WorkgroupName"] = hdnWorkgroupName.Value;
            Session["WorkGroupNameToDisplay"] = hdnWorkgroupName.Value;
            IncentexGlobal.ProductReturnURL = "~/admin/Report/ProductPlanningReport.aspx?SubReport=" + SubReport + "&WorkGroup=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : "") + "&BaseStation=" + (ddlBasestation.SelectedValue != "0" ? ddlBasestation.SelectedValue : "" + "&StoreID=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedValue : "") + "&Gender=" + (ddlGender.SelectedIndex > 0 ? ddlGender.SelectedValue : ""));
            Response.Redirect("~/admin/CompanyStore/Product/General.aspx?SubId=" + e.CommandArgument + "&Id=" + hdnStoreID.Value);
        }
    }
    protected void gvSummaryProductReview_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void chrtItemswithBackorders_Click(object sender, ImageMapEventArgs e)
    {
        Response.Redirect("~/admin/Report/ItemWithBackOrderedGridReport.aspx?ChartSubReport=" + SubReport + "&StoreID=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedValue : "") + "&BaseStation=" + (ddlBasestation.SelectedIndex > 0 ? ddlBasestation.SelectedValue : "") + "&WorkGroup=" + (ddlWorkgroup.SelectedIndex > 0 ? ddlWorkgroup.SelectedValue : "") + "&Gender=" + (ddlGender.SelectedIndex > 0 ? ddlGender.SelectedValue : ""));
    }
    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {
        GenerateChart();
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
                objCompanyList = objReportAccessRightsRepository.GetStoreByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 452);

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

    private void FillBasedStation()
    {
        List<INC_BasedStation> basestationList = new List<INC_BasedStation>();
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            basestationList = new BaseStationRepository().GetAllBaseStation().ToList();
        else
            basestationList = objReportAccessRightsRepository.GetBaseStationByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 452);

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
        List<INC_Lookup> WorkgroupList = new List<INC_Lookup>();
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            WorkgroupList = objLookupRepos.GetByLookup("Workgroup").ToList();
        else
            WorkgroupList = objReportAccessRightsRepository.GetWorkgroupByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 452);

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
        if (Request.QueryString["SubReport"] != null && (Request.QueryString["SubReport"] == "Summary Product Review" || Request.QueryString["SubReport"] == "Customer Owned Inventory"))
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
        Int64? UserInfoID = null;

        if (txtFromDate.Text != "")
            FromDate = Convert.ToDateTime(txtFromDate.Text.Trim());
        if (txtToDate.Text != "")
            ToDate = Convert.ToDateTime(txtToDate.Text.Trim());
        if (ddlCompanyStore.SelectedIndex > 0)
            StoreID = Convert.ToInt64(ddlCompanyStore.SelectedValue);
        if (ddlWorkgroup.SelectedValue != "0")
            WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
        if (ddlBasestation.SelectedValue != "0")
            BaseStationID = Convert.ToInt64(ddlBasestation.SelectedValue);
        if (ddlGender.SelectedValue != "0")
            GenderID = Convert.ToInt64(ddlGender.SelectedValue);
        if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;

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
                List<ReportRepository.GetProductSizeWiseResult> objResult = objReportRepository.GetProductSalesSizeWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, null);
                if (objResult != null)
                {
                    objResult = objResult.OrderBy(x => x.Value).Where(x => x.Text != "No Size" && x.Text != "One Size").ToList();
                    chrtProductSalesByFemaleSize.DataSource = objResult;
                    chrtProductSalesByFemaleSize.DataBind();
                    lblTotalUnits.Text = "Total Units : " + Convert.ToString(objResult.Sum(x => x.Value));
                }
                dvProductSalesByFemaleSize.Visible = true;
            }
            else if (SubReport == "Purchase History by Color")
            {
                /// <summary>
                /// this report display item size report based on color unit sold
                /// </summary>
                List<ReportRepository.GetProductSizeWiseResult> objResult = objReportRepository.GetProductSalesColorWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtProductSalesByColor.DataSource = objResult;
                    chrtProductSalesByColor.DataBind();
                    lblTotalUnits.Text = "Total Units : " + Convert.ToString(objResult.Sum(x => x.Value));
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

                List<ReportRepository.GetTopFiftyItemByRevenueResult> objResult = objReportRepository.GetTopFiftyItemByRevenue(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    objResult = objResult.OrderByDescending(x => x.Value).Take(50).OrderBy(x => x.Value).ToList();
                    chrtTopFiftyItemsByRevenue.Series["SeriesRevenue"].Points.DataBindXY(objResult, "Text", objResult, "Value");
                    chrtTopFiftyItemsByRevenue.Series["SeriesUnit"].Points.DataBindXY(objResult, "Text", objResult, "Value1");
                    lblTotalUnits.Text = "Total Units : " + objResult.Sum(x => x.Value1) + " &emsp;&emsp;&emsp;&emsp;Total Revenue : " + string.Format("{0:c}", objResult.Sum(x => x.Value));
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
                List<ReportRepository.GetBackOrderItemsResult> objResult = objReportRepository.GetBackOrderItemsReport(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    objResult = objResult.OrderBy(x => x.Value).ToList();
                    chrtBackOrderInventoryReport.Series["SeriesTotalOrder"].Points.DataBindXY(objResult, "Text", objResult, "Value");
                    chrtBackOrderInventoryReport.Series["SeriesNotShipped"].Points.DataBindXY(objResult, "Text", objResult, "Value1");
                    lblTotalUnits.Text = "Total Units : " + objResult.Sum(x => x.Value) + " &emsp;&emsp;&emsp;&emsp;Not Shipped : " + objResult.Sum(x => x.Value1);
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

                List<ReportRepository.GetInventoryUsageReportResult> objResult = objReportRepository.GetInventoryUsageReport(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, Convert.ToInt64(Request.QueryString["MasterItemID"]));
                if (objResult != null)
                {
                    objResult = objResult.OrderBy(x => x.Sold).ToList();
                    chrtInventoryByItem.Series["SeriesOnHand"].Points.DataBindXY(objResult, "Text", objResult, "OnHand");
                    chrtInventoryByItem.Series["SeriesSold"].Points.DataBindXY(objResult, "Text", objResult, "Sold");
                    chrtInventoryByItem.Series["SeriesOnOrder"].Points.DataBindXY(objResult, "Text", objResult, "OnOrder");
                    chrtInventoryByItem.Series["SeriesReOrder"].Points.DataBindXY(objResult, "Text", objResult, "ReOrder");
                    chrtInventoryByItem.Series["SeriesReturn"].Points.DataBindXY(objResult, "Text", objResult, "Return");
                    lblTotalUnits.Text = "Total Inventory : " + objResult.Sum(x => x.OnHand) + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Total Sold : " + objResult.Sum(x => x.Sold) + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Back Order : " + objResult.Sum(x => x.Return) + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; On-Order : " + objResult.Sum(x => x.OnOrder) + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Re-Order : " + objResult.Sum(x => x.ReOrder);
                }
                dvInventoryByItem.Visible = true;
            }
            else if (SubReport == "Summary Product Review")
            {
                trBaseStation.Visible = false;
                trFromDate.Visible = false;
                trToDate.Visible = false;
                trPeriod.Visible = false;
                List<ReportRepository.GetSummaryProductReviewResult> objResult = objReportRepository.GetSummaryProductReviewReport(UserInfoID, StoreID, WorkgroupID,GenderID);

                if (objResult != null && objResult.Count > 0)
                {
                    objResult = objResult.OrderBy(x => x.MasterItemNumber).ToList();
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
                    gvSummaryProductReview.DataSource = pds;
                    gvSummaryProductReview.DataBind();

                    pagingtable.Visible = true;
                    doPaging();
                }
                else
                {
                    gvSummaryProductReview.DataSource = null;
                    gvSummaryProductReview.DataBind();
                    pagingtable.Visible = false;
                }
                dvSummaryProductReview.Visible = true;
            }
            else if (SubReport == "Items with Backorders")
            {
                trFromDate.Visible = false;
                trToDate.Visible = false;
                trPeriod.Visible = false;

                List<ReportRepository.GetItemswithBackordersResult> objResult = objReportRepository.GetItemswithBackorders(UserInfoID, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    List<GetItemswithBackordersResult> objGetItemswithBackordersResultList = new List<GetItemswithBackordersResult>();
                    GetItemswithBackordersResult objGetItemswithBackordersResult = new GetItemswithBackordersResult();
                    objGetItemswithBackordersResult.Text = "In-Stock";
                    objGetItemswithBackordersResult.Value = objResult.Sum(x => x.CurrentStock);
                    objGetItemswithBackordersResultList.Add(objGetItemswithBackordersResult);

                    objGetItemswithBackordersResult = new GetItemswithBackordersResult();
                    objGetItemswithBackordersResult.Text = "Backordered";
                    objGetItemswithBackordersResult.Value = objResult.Sum(x => x.BackOrdered);
                    objGetItemswithBackordersResultList.Add(objGetItemswithBackordersResult);

                    chrtItemswithBackorders.DataSource = objGetItemswithBackordersResultList;
                    chrtItemswithBackorders.DataBind();
                }
                dvItemswithBackorders.Visible = true;
            }
            else if (SubReport == "Customer Owned Inventory")
            {
                trBaseStation.Visible = false;
                trFromDate.Visible = false;
                trToDate.Visible = false;
                trPeriod.Visible = false;
                List<SelectSalesByInventoryStatusResult> objResult = objReportRepository.GetProductSalesInventoryStatusWise(UserInfoID, StoreID, WorkgroupID, GenderID,245);

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
                    lblRecordCounter.Text = objResult.Count().ToString();
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
                    lblRecordCounter.Text = "0";
                }
                dvCustomerOwnedInventory.Visible = true;
            }
        }
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

    #region Page Level Classes
    public class GetItemswithBackordersResult
    {
        public string Text { get; set; }
        public Int64? Value { get; set; }
    }
    #endregion

    #endregion
}
