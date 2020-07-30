using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Web.UI.DataVisualization.Charting;
using Incentex.DAL.Common;

public partial class admin_Report_ServiceLevelScorecardReport : PageBase
{
    #region Data Members
    ReportRepository objReportRepository = new ReportRepository();
    LookupRepository objLookupRepos = new LookupRepository();
    ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();
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
    String WorkGroupIDs
    {
        get
        {
            if (ViewState["WorkGroupIDs"] == null)
            {
                ViewState["WorkGroupIDs"] = null;
            }
            return Convert.ToString(ViewState["WorkGroupIDs"]);
        }
        set
        {
            ViewState["WorkGroupIDs"] = value;
        }
    }
    String BaseStationIDs
    {
        get
        {
            if (ViewState["BaseStationIDs"] == null)
            {
                ViewState["BaseStationIDs"] = null;
            }
            return Convert.ToString(ViewState["BaseStationIDs"]);
        }
        set
        {
            ViewState["BaseStationIDs"] = value;
        }
    }
    String PriceLevelIDs
    {
        get
        {
            if (ViewState["PriceLevelIDs"] == null)
            {
                ViewState["PriceLevelIDs"] = null;
            }
            return Convert.ToString(ViewState["PriceLevelIDs"]);
        }
        set
        {
            ViewState["PriceLevelIDs"] = value;
        }
    }
    BaseStationRepository objBaseStatRepo = new BaseStationRepository();
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Service Level Scorecard Dashboard";
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
            GenerateChart();
        }
    }
    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        if (ddlPeriod.SelectedValue == "")//This is for date range
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

    protected void chrtShipCompleteReportChart_Click(object sender, ImageMapEventArgs e)
    {
        string strDays = e.PostBackValue;
        if (strDays == "Days: 0-1")
            strDays = "1";
        else if (strDays == "Days: 2-5")
            strDays = "5";
        else if (strDays == "Days: 6-7")
            strDays = "7";
        else if (strDays == "Days: 8-14")
            strDays = "14";
        else
            strDays = "15";

        if (!string.IsNullOrEmpty(strDays))
        {
            Response.Redirect("~/OrderManagement/OrderView.aspx?ChartSubReport=" + SubReport + "&StoreId=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedItem.Value : "") + "&ToDate=" + txtToDate.Text.Trim() + "&FromDate=" + txtFromDate.Text.Trim() + "&Email=&OrderNumber=&OrdeStatus=&WorkGroup=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : "") + "&FName=&LName=&EmployeeCode=&StationCode=&PaymentType=&DaysRange=" + strDays + "&WorkGroupAccess=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : this.WorkGroupIDs) + "&BSA=" + this.BaseStationIDs);
        }
    }
    protected void chrtHelptickets_Click(object sender, ImageMapEventArgs e)
    {
        long? CompanyID = 0, BaseStationID = 0, WorkgroupID = 0, TypeID = 0;

        if (ddlCompanyStore.SelectedIndex > 0)
        {
            CompanyStoreRepository objCompanyStoreRepository = new CompanyStoreRepository();
            CompanyID = objCompanyStoreRepository.GetBYStoreId(ddlCompanyStore.SelectedIndex).SingleOrDefault().CompanyID;

        }

        if (SubReport == "Help Tickets by Location")
        {
            BaseStationRepository objBaseStation = new BaseStationRepository();
            BaseStationID = objBaseStation.GetByName(e.PostBackValue.ToString()).iBaseStationId;

        }
        if (SubReport == "Help Tickets by Workgroup")
        {
            LookupRepository objLookUpRepository = new LookupRepository();
            WorkgroupID = objLookUpRepository.GetIdByLookupNameNLookUpCode(e.PostBackValue.ToString(), DAEnums.LookupCodeType.Workgroup.ToString());

        }
        if (SubReport == "Help Tickets by Type")
        {
            LookupRepository objLookUpRepository = new LookupRepository();
            TypeID = objLookUpRepository.GetIdByLookupNameNLookUpCode(e.PostBackValue.ToString(), DAEnums.LookupCodeType.TypeOfRequest.ToString());
            if (ddlBasestation.SelectedValue != "0")
                BaseStationID = Convert.ToInt64(ddlBasestation.SelectedValue);
            if (ddlWorkgroup.SelectedValue != "0")
                WorkgroupID= Convert.ToInt64(ddlWorkgroup.SelectedValue);
        }

        Response.Redirect("~/admin/Report/ServiceTicketList.aspx?ChartSubReport=" + SubReport + "&com=" + (CompanyID > 0 ? CompanyID.ToString() : "") + "&ToDate=" + txtToDate.Text.Trim() + "&FromDate=" + txtFromDate.Text.Trim() + "&Email=&OrderNumber=&OrdeStatus=&wg=" + (WorkgroupID != 0 ? Convert.ToString(WorkgroupID) : "") + "&FName=&LName=&EmployeeCode=&bs=" + (BaseStationID != 0 ? Convert.ToString(BaseStationID) : "") + "&tor=" + (TypeID != 0 ? Convert.ToString(TypeID) : ""));
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
                objCompanyList = objReportAccessRightsRepository.GetStoreByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 455);

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
            basestationList = objReportAccessRightsRepository.GetBaseStationByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 455);

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
            WorkgroupList = objReportAccessRightsRepository.GetWorkgroupByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 455);

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
            if (SubReport == "Ship Time for In-Stock Merchandise")
            {
                List<ReportRepository.GetAverageShipTimeWiseResult> objResult = objReportRepository.GetAverageShipTimeWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtAverageShipTime.DataSource = objResult;
                    chrtAverageShipTime.DataBind();
                    lblDisplayText.Text = "Total Orders : " + objResult.Sum(x => x.Count);

                    try
                    {
                        for (int i = 0; i < chrtAverageShipTime.Series["Series1"].Points.Count; i++)
                        {
                            if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtAverageShipTime.Series["Series1"].Points[i])).AxisLabel == "Waiting to Ship")
                                chrtAverageShipTime.Series["Series1"].Points[i].Color = System.Drawing.Color.Red;
                        }
                    }
                    catch
                    {
                    }
                }
                dvAverageShipTime.Visible = true;
            }
            else if (SubReport == "Returns vs. Orders Placed")
            {
                /// <summary>
                /// This report will be a bar graph coming out from the side like the planning reports. 
                /// two different bars for each size one bar will show the total number of units sold and the next bar will show the total number of pieces that where returned for that item number. 
                /// The sold bar will be blue and the returns will be red.
                /// All of our customers must use our return module to return items to us so you will be able to pull the data from there for the returns.
                /// On the bottom of the page show a grand total for example: Sold 10,000 units vs. 250 Returns = 2.5%
                /// </summary>

                List<ReportRepository.GetPurchasesVSReturnsResult> objResult = objReportRepository.GetPurchasesVSReturns(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    objResult = objResult.OrderBy(x => x.Value).Where(x => x.Text != "No Size" && x.Text != "One Size").ToList();
                    chrtPurchasesVSReturns.Series["SeriesForPurchase"].Points.DataBindXY(objResult, "Text", objResult, "Value");
                    chrtPurchasesVSReturns.Series["SeriesForReturn"].Points.DataBindXY(objResult, "Text", objResult, "Value1");

                    Decimal? totalSales = objResult.Sum(x => x.Value);
                    Decimal? totalReturns = objResult.Sum(x => x.Value1);
                    try
                    {
                        lblDisplayText.Text = "Sold " + totalSales + " units vs. " + totalReturns + " Returns = " + Math.Round(Convert.ToDecimal(totalReturns / totalSales) * 100, 2) + "%";
                    }
                    catch { lblDisplayText.Text = ""; }
                }
                dvPurchasesVSReturns.Visible = true;
            }
            else if (SubReport == "Returns vs. Units Purchased")
            {
                /// <summary>
                /// The units vs. returns report will have only two bar graphs coming out from the side. 
                /// One will be blue for the total units sold for all items and the bar below it will be red for the total units returned.  
                /// On the bottom of the report will show the total number of units and returns and then the %.
                /// </summary>
                List<ReportRepository.GetPurchasesVSReturnsResult> objResult = objReportRepository.GetPurchasesVSReturns(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    Decimal? totalSales = objResult.Sum(x => x.Value);
                    Decimal? totalReturns = objResult.Sum(x => x.Value1);

                    DataPoint dataPointSold = new DataPoint();
                    dataPointSold.SetValueY(totalSales);

                    DataPoint dataPointReturn = new DataPoint();
                    dataPointReturn.SetValueY(totalReturns);

                    chrtUnitsVSReturnsSummary.Series["SeriesTotalOrder"].Points.Add(dataPointSold);
                    chrtUnitsVSReturnsSummary.Series["SeriesTotalReturn"].Points.Add(dataPointReturn);

                    try
                    {
                        lblDisplayText.Text = "Sold " + totalSales + " units vs. " + totalReturns + " Returns = " + Math.Round(Convert.ToDecimal(totalReturns / totalSales) * 100, 2) + "%";
                    }
                    catch { lblDisplayText.Text = ""; }
                }
                dvUnitsVSReturnsSummary.Visible = true;
            }
            else if (SubReport == "Help Tickets by Location")
            {
                List<ReportRepository.GetHelpTicketsbyLocationResult> objResult = objReportRepository.GetHelpTicketsbyLocation(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtHelpTicketsbyLocation.DataSource = objResult;
                    chrtHelpTicketsbyLocation.DataBind();
                    lblDisplayText.Text = "Total Tickets : " + objResult.Sum(x => x.TicketCount);
                }
                dvHelpTicketsbyLocation.Visible = true;
            }
            else if (SubReport == "Help Tickets by Workgroup")
            {
                List<ReportRepository.GetHelpTicketsbyWorkgroupResult> objResult = objReportRepository.GetHelpTicketsbyWorkgroup(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtHelpTicketsbyWorkgroup.DataSource = objResult;
                    chrtHelpTicketsbyWorkgroup.DataBind();
                    lblDisplayText.Text = "Total Tickets : " + objResult.Sum(x => x.TicketCount);
                }
                dvHelpTicketsbyWorkgroup.Visible = true;
            }
            else if (SubReport == "Help Tickets Users vs. Total Employees")
            {
                List<ReportRepository.GetHelpTicketsUsersvsTotalEmployeesResult> objResult = objReportRepository.GetHelpTicketsUsersvsTotalEmployees(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtHelpTicketsUsersvsTotalEmployees.Series["SeriesTotalEmployee"].Points.DataBindY(objResult, "Value");
                    chrtHelpTicketsUsersvsTotalEmployees.Series["SeriesTotalTicketEmployee"].Points.DataBindY(objResult, "Value1");

                    Decimal? totalEmployee = objResult.Sum(x => x.Value);
                    Decimal? totalTicketEmployee = objResult.Sum(x => x.Value1);
                    try
                    {
                        lblDisplayText.Text = "Total Employee " + totalEmployee + " vs. " + totalTicketEmployee + " Total Ticket Employee = " + Math.Round(Convert.ToDecimal(totalTicketEmployee / totalEmployee) * 100, 2) + "%";
                    }
                    catch { lblDisplayText.Text = ""; }
                }
                dvHelpTicketsUsersvsTotalEmployees.Visible = true;
            }
            else if (SubReport == "Top 25 Employees Submitting Tickets")
            {
                List<ReportRepository.GetTop25EmployeesSubmittingTicketsResult> objResult = objReportRepository.GetTop25EmployeesSubmittingTickets(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    Decimal? totalTicket = objResult.Sum(x => x.TicketCount);
                    objResult = objResult.OrderByDescending(x => x.TicketCount).Take(25).ToList();
                    Decimal? total25EmployeeTicket = objResult.Sum(x => x.TicketCount);

                    gvTop25EmployeesSubmittingTickets.DataSource = objResult;
                    gvTop25EmployeesSubmittingTickets.DataBind();

                    lblDisplayText.Text = "<br/>Total Tickets : " + totalTicket + " vs. Top 25 Employee Tickets : " + total25EmployeeTicket + " = " + Math.Round(Convert.ToDecimal(total25EmployeeTicket / totalTicket) * 100, 2) + "%";
                }
                dvTop25EmployeesSubmittingTickets.Visible = true;
            }
            else if (SubReport == "Ship Complete Report")
            {
                int TotalOrderPlaced, TotalShipComplete;
                List<ReportRepository.GetShipCompleteCountDayWiseResult> objResult = objReportRepository.GetShipCompleteCountDayWise(out TotalOrderPlaced, UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtShipCompleteReportChart.DataSource = objResult;
                    chrtShipCompleteReportChart.DataBind();
                    TotalShipComplete = objResult.Sum(x => x.Count);
                    lblDisplayText.Text = "Total Orders : " + TotalOrderPlaced;
                    if(TotalOrderPlaced != 0) 
                        lblDisplayText.Text += "<br/> Total Ship Complete : " + TotalShipComplete + " (" + ((TotalShipComplete * 100) / TotalOrderPlaced) + "%)";
                    else
                        lblDisplayText.Text += "<br/> Total Ship Complete : 0)";
                }
                dvShipCompleteReport.Visible = true;
            }
            else if (SubReport == "Help Tickets by Type")
            {
                List<ReportRepository.GetHelpTicketsbyTypeResult> objResult = objReportRepository.GetHelpTicketsbyType(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtHelpticketsByType.DataSource = objResult;
                    chrtHelpticketsByType.DataBind();
                    lblDisplayText.Text = "Total Tickets : " + objResult.Sum(x => x.TicketCount);
                }
                dvHelpTicketsbyType.Visible = true;
            }
        }
    }

    #endregion
}
