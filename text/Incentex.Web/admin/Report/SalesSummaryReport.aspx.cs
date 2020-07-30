using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Text;

public partial class admin_Report_SalesSummaryReport : PageBase
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
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Spend Summary Dashboard";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Report/ReportDashBoard.aspx";
            FillCompanyStore();
            FillBasedStation();
            FillWorkgroup();
            FillGender();
            FillOrderStatus();
            FillPriceList();
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
    protected void chrtSalesByStation_Click(object sender, ImageMapEventArgs e)
    {
        INC_BasedStation objINC_BasedStation = new BaseStationRepository().GetByName(e.PostBackValue);
        if (objINC_BasedStation != null)
        {
            Response.Redirect("~/OrderManagement/OrderView.aspx?ChartSubReport=" + SubReport + "&StoreId=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedItem.Value : "") + "&ToDate=" + txtToDate.Text.Trim() + "&FromDate=" + txtFromDate.Text.Trim() + "&Email=&OrderNumber=&OrdeStatus=" + (ddlOrderStatus.SelectedIndex > 0 ? ddlOrderStatus.SelectedItem.Text : "") + "&WorkGroup=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : "") + "&FName=&LName=&EmployeeCode=&StationCode=" + objINC_BasedStation.iBaseStationId + "&PaymentType=&WorkGroupAccess=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : this.WorkGroupIDs) + "&BSA=" + objINC_BasedStation.iBaseStationId);
        }
    }
    protected void chrtSalesByPersonalProtectiveEquiptment_Click(object sender, ImageMapEventArgs e)
    {
        INC_BasedStation objINC_BasedStation = new BaseStationRepository().GetByName(e.PostBackValue);
        if (objINC_BasedStation != null)
        {
            Response.Redirect("~/OrderManagement/OrderView.aspx?ChartSubReport=" + SubReport + "&StoreId=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedItem.Value : "") + "&ToDate=" + txtToDate.Text.Trim() + "&FromDate=" + txtFromDate.Text.Trim() + "&Email=&OrderNumber=&OrdeStatus=" + (ddlOrderStatus.SelectedIndex > 0 ? ddlOrderStatus.SelectedItem.Text : "") + "&WorkGroup=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : "") + "&FName=&LName=&EmployeeCode=&StationCode=" + objINC_BasedStation.iBaseStationId + "&PaymentType=&WorkGroupAccess=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : this.WorkGroupIDs) + "&BSA=" + objINC_BasedStation.iBaseStationId);
        }
    }
    protected void chrtSalesByWorkgroupPer_Click(object sender, ImageMapEventArgs e)
    {
        Int64? workgroupID = new LookupRepository().GetIdByLookupNameNLookUpCode(e.PostBackValue, "Workgroup ");
        if (workgroupID != null)
        {
            Response.Redirect("~/OrderManagement/OrderView.aspx?ChartSubReport=" + SubReport + "&StoreId=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedItem.Value : "") + "&ToDate=" + txtToDate.Text.Trim() + "&FromDate=" + txtFromDate.Text.Trim() + "&Email=&OrderNumber=&OrdeStatus=" + (ddlOrderStatus.SelectedIndex > 0 ? ddlOrderStatus.SelectedItem.Text : "") + "&WorkGroup=" + workgroupID + "&FName=&LName=&EmployeeCode=&StationCode=" + (ddlBasestation.SelectedValue != "0" ? ddlBasestation.SelectedValue : "") + "&PaymentType=&WorkGroupAccess=" + workgroupID + "&BSA=" + (ddlBasestation.SelectedValue != "0" ? ddlBasestation.SelectedValue : this.BaseStationIDs));
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
                objCompanyList = objReportAccessRightsRepository.GetStoreByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 451);

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
            basestationList = objReportAccessRightsRepository.GetBaseStationByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 451);

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
            WorkgroupList = objReportAccessRightsRepository.GetWorkgroupByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 451);

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

    private void FillOrderStatus()
    {
        List<INC_Lookup> objINCLookupList = objLookupRepos.GetByLookup("StatusOptionOne").Where(x => x.sLookupName != "Canceled").ToList();

        ddlOrderStatus.DataSource = objINCLookupList.OrderBy(x => x.sLookupName);
        ddlOrderStatus.DataValueField = "iLookupID";
        ddlOrderStatus.DataTextField = "sLookupName";
        ddlOrderStatus.DataBind();
        ddlOrderStatus.Items.Insert(0, new ListItem("-Select Order Status-", "0"));
    }

    private void FillPriceList()
    {
        ddlPriceLevel.DataSource = objLookupRepos.GetByLookup("PriceLevel");
        ddlPriceLevel.DataValueField = "Val1";
        ddlPriceLevel.DataTextField = "sLookupName";
        ddlPriceLevel.DataBind();
        ddlPriceLevel.Items.Insert(0, new ListItem("-Select Pricing Level-", "0"));
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
        String OrderStatus = null;
        Int64? UserInfoID = null;
        String PriceLevel = string.Join(",", objLookupRepos.GetByLookup("PriceLevel").Select(x => x.Val1.ToString()).ToArray());

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
        if (ddlOrderStatus.SelectedIndex > 0)
            OrderStatus = ddlOrderStatus.SelectedItem.Text;
        if (ddlPriceLevel.SelectedIndex > 0)
            PriceLevel = ddlPriceLevel.SelectedValue;
        if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;

        if (Request.QueryString["SubReport"] != null)
        {
            SubReport = Request.QueryString["SubReport"].ToString();
        }
        if (SubReport != null)
        {
            if (SubReport == "Spend By Station Location")
            {
                List<ReportRepository.GetSalesStationWiseResult> objResult = objReportRepository.GetSalesStationWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, OrderStatus, PriceLevel);
                if (objResult != null)
                {
                    if (objResult.Count > 0)
                    {
                        this.WorkGroupIDs = objResult.FirstOrDefault().WorkGroupIDs;
                        this.BaseStationIDs = objResult.FirstOrDefault().BaseStationIDs;
                    }

                    //Temp commented for the testing of the Fusion Chart integration
                    //chrtSalesByStation.DataSource = objResult;
                    //chrtSalesByStation.DataBind();
                    //Temp commented for the testing of the Fusion Chart integration

                    //Fusion Chart Code
                    StringBuilder strXML = new StringBuilder();
                    //$strXML will be used to store the entire XML document generated
                    //Generate the chart element
                    strXML.Append("<chart  exportShowMenuItem ='1' exportEnabled='1' caption='Sales Report' subCaption='Spend By BaseStation' pieSliceDepth='30' showBorder='0' formatNumberScale='0' numberPrefix='$ ' bgColor='000000' canvasBgColor='000000' showlabels='0'>");

                    foreach (var item in objResult)
                    {
                        //strXML.AppendFormat("<set label='{0}'  value='{1}' link='{2}'></set>", item.BaseStation, item.OrderAmount, ("../../OrderManagement/OrderView.aspx?ChartSubReport=" + SubReport + "&StoreId=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedItem.Value : "") + "&ToDate=" + txtToDate.Text.Trim() + "&FromDate=" + txtFromDate.Text.Trim() + "&Email=&OrderNumber=&OrdeStatus=" + (ddlOrderStatus.SelectedIndex > 0 ? ddlOrderStatus.SelectedItem.Text : "") + "&WorkGroup=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : "") + "&FName=&LName=&EmployeeCode=&StationCode=" + item.CurrentBaseStationID + "&PaymentType=&WorkGroupAccess=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : this.WorkGroupIDs) + "&BSA=" + item.CurrentBaseStationID));

                        strXML.AppendFormat("<set label='{0}'  value='{1}'></set>", item.BaseStation, (item.OrderAmount + item.CAIE_MOASOrderAmount));

                    }


                    //Finally, close <chart> element
                    strXML.Append("</chart>");

                    //Create the chart - Pie 3D Chart with data from strXML
                    FusionCharts.SetRenderer("javascript");
                    ltrSpendByLocation.Text = FusionCharts.RenderChart("../../FusionCharts/Pie2D", "", strXML.ToString(), "SpendLocation", "900", "750", false, true, false);

                    //testChart.Text = strXML.ToString();

                    chrtSalesByStation.Visible = false;
                    //Fusion Chart Code

                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => (x.OrderAmount + x.CAIE_MOASOrderAmount)));
                    lblTotalSpend.Text += "<br/>Waiting for Approval : " + string.Format("{0:c}", objResult.Sum(x => (x.WaitingOrderAmount + x.CAIE_WaitingMOASOrderAmount)));

                }
                dvSalesByStation.Visible = true;
            }
            else if (SubReport == "Spend By Workgroup")
            {
                List<ReportRepository.GetSalesWorkgroupWiseResult> objResult = objReportRepository.GetSalesWorkgroupWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, OrderStatus, PriceLevel);
                if (objResult != null)
                {
                    if (objResult.Count > 0)
                    {
                        this.WorkGroupIDs = objResult.FirstOrDefault().WorkGroupIDs;
                        this.BaseStationIDs = objResult.FirstOrDefault().BaseStationIDs;
                    }

                    //Temp commented for the testing of the Fusion Chart integration
                    //chrtSalesByWorkgroup.DataSource = objResult;
                    //chrtSalesByWorkgroup.DataBind();
                    //Temp commented for the testing of the Fusion Chart integration
                    //Fusion Chart Code
                    StringBuilder strXML = new StringBuilder();
                    strXML.Append("<chart  exportShowMenuItem ='1' exportEnabled='1' caption='Sales Report' subCaption='Spend By WorkGroup' showBorder='0'  xAxisName='Workgroup' yAxisName='OrderAmount' numberPrefix='$ ' bgcolor='0000000' bgratio='0' bgalpha='100' canvasBgColor='000000' canvasbasecolor='000000' canvasbgratio='0' canvasbgalpha='10'  showlabels='0' color='ffffff'>");

                    foreach (var item in objResult)
                    {

                        strXML.AppendFormat("<set label='{0}'  value='{1}'></set>", item.Workgroup, (item.OrderAmount + item.CAIE_MOASOrderAmount));

                    }

                    //strXML.Append("<styles><definition>");
                    //strXML.Append("<style name='Font_1' type='font' font='Times' size='22' color='4F4F4F' bold='0' align='right' Italic='1' bgcolor='Red' bordercolor='D0D0D0' />");
                    //strXML.Append("</definition><application><apply toObject='Chart' styles='Font_1'/>");
                    //strXML.Append("</application></styles>");


                    //Finally, close <chart> element
                    strXML.Append("</chart>");

                    //Create the chart - Pie 3D Chart with data from strXML
                    FusionCharts.SetRenderer("javascript");
                    ltrSpendByWorkgroup.Text = FusionCharts.RenderChart("../../FusionCharts/Column3D", "", strXML.ToString(), "SpendLocation", "900", "750", false, true, false);

                    //testChart.Text = strXML.ToString();

                    chrtSalesByWorkgroup.Visible = false;

                    //Fusion Chart Code
                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => x.OrderAmount));
                }
                dvSalesByWorkgroup.Visible = true;
            }
            else if (SubReport == "Spend By Workgroup%")
            {
                List<ReportRepository.GetSalesWorkgroupWiseResult> objResult = objReportRepository.GetSalesWorkgroupWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, OrderStatus, PriceLevel);
                if (objResult != null)
                {
                    if (objResult.Count > 0)
                    {
                        this.WorkGroupIDs = objResult.FirstOrDefault().WorkGroupIDs;
                        this.BaseStationIDs = objResult.FirstOrDefault().BaseStationIDs;
                    }
                    chrtSalesByWorkgroupPer.DataSource = objResult;
                    chrtSalesByWorkgroupPer.DataBind();


                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => (x.OrderAmount + x.CAIE_MOASOrderAmount)));


                }
                dvSalesByWorkgroupPer.Visible = true;
            }
            else if (SubReport == "Total Program Spend Since Start Date")
            {
                List<ReportRepository.GetSalesYearlWiseResult> objResult = objReportRepository.GetSalesYearWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, OrderStatus, PriceLevel);
                if (objResult != null)
                {
                    chrtSalesByYear.DataSource = objResult;
                    chrtSalesByYear.DataBind();
                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => x.OrderAmount));
                }
                dvSalesByYear.Visible = true;
            }
            else if (SubReport == "Total Company Spend Vs. Employee Spend")
            {
                List<ReportRepository.GetSalesCompanyVSEmployeeWiseResult> objSalesCompanyWise = objReportRepository.GetSalesCompanyWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, OrderStatus, PriceLevel);
                List<ReportRepository.GetSalesCompanyVSEmployeeWiseResult> objSalesEmployeeWise = objReportRepository.GetSalesEmployeeWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, OrderStatus, PriceLevel);

                if (objSalesCompanyWise != null && objSalesEmployeeWise != null)
                {
                    chrtSalesByCompanyVSEmployee.Series["SeriesForCompany"].Points.DataBindXY(objSalesCompanyWise, "Month", objSalesCompanyWise, "OrderAmount");
                    chrtSalesByCompanyVSEmployee.Series["SeriesForEmployee"].Points.DataBindXY(objSalesEmployeeWise, "Month", objSalesEmployeeWise, "OrderAmount");

                    lblTotalSpend.Text = string.Format("{0:c}", objSalesCompanyWise.Sum(x => x.OrderAmount) + objSalesEmployeeWise.Sum(x => x.OrderAmount));
                    lblTotalSpend.Text += "<br/>" + "Total Employee Spend : " + string.Format("{0:c}", objSalesEmployeeWise.Sum(x => x.OrderAmount));
                    lblTotalSpend.Text += "<br/>" + "Total Company Spend : " + string.Format("{0:c}", objSalesCompanyWise.Sum(x => x.OrderAmount));
                }
                dvSalesByCompanyVSEmployee.Visible = true;
            }
            else if (SubReport == "Total Company Spend By Month")
            {
                List<ReportRepository.GetSalesCompanyVSEmployeeWiseResult> objResult = objReportRepository.GetSalesCompanyWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, OrderStatus, PriceLevel);
                if (objResult != null)
                {
                    chrtSalesByCompany.DataSource = objResult;
                    chrtSalesByCompany.DataBind();
                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => x.OrderAmount));
                }
                dvSalesByCompany.Visible = true;
            }
            else if (SubReport == "Personal Protective Equiptment")
            {
                Int64? ReportTagID = objLookupRepos.GetIdByLookupNameNLookUpCode("Personal Protective Equiptment", "ReportTag");
                List<ReportRepository.GetSalesStationWiseResult> objResult = objReportRepository.GetSalesByReportTag(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, OrderStatus, PriceLevel, ReportTagID == null ? 0 : Convert.ToInt64(ReportTagID));
                if (objResult != null)
                {
                    if (objResult.Count > 0)
                    {
                        this.WorkGroupIDs = objResult.FirstOrDefault().WorkGroupIDs;
                        this.BaseStationIDs = objResult.FirstOrDefault().BaseStationIDs;
                    }
                    chrtSalesByPersonalProtectiveEquiptment.DataSource = objResult;
                    chrtSalesByPersonalProtectiveEquiptment.DataBind();
                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => x.OrderAmount));
                }
                dvSalesByPersonalProtectiveEquiptment.Visible = true;
            }
            else if (SubReport == "Spanx Sales Summary")
            {
                Int64? ReportTagID = objLookupRepos.GetIdByLookupNameNLookUpCode("Spanx", "ReportTag");
                List<ReportRepository.GetSalesStationWiseResult> objResult = objReportRepository.GetSalesByReportTag(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, OrderStatus, PriceLevel, ReportTagID == null ? 0 : Convert.ToInt64(ReportTagID));
                if (objResult != null)
                {
                    chrtSalesBySpanx.DataSource = objResult;
                    chrtSalesBySpanx.DataBind();
                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => x.OrderAmount));
                }
                dvSalesBySpanx.Visible = true;
            }
            else if (SubReport == "Current Vs Previous Year Sales")
            {
                trPeriod.Visible = false;
                trFromDate.Visible = false;
                trToDate.Visible = false;
                List<ReportRepository.GetYearVsPreviousYearReportResult> objYearTotalSales = objReportRepository.GetYearVsPresiousYearReportResult(UserInfoID, StoreID, WorkgroupID, BaseStationID, GenderID, OrderStatus, PriceLevel);

                if (objYearTotalSales != null)
                {
                    if (objYearTotalSales.Count > 0)
                    {
                        this.WorkGroupIDs = objYearTotalSales.FirstOrDefault().WorkGroupIDs;
                        this.BaseStationIDs = objYearTotalSales.FirstOrDefault().BaseStationIDs;
                    }

                    //Temp commented for the testing of the Fusion Chart integration
                    //chrtSalesByWorkgroup.DataSource = objResult;
                    //chrtSalesByWorkgroup.DataBind();
                    //Temp commented for the testing of the Fusion Chart integration


                    //var PreviousYearResult = objYearTotalSales.Where(p => p.Year == (System.DateTime.Now.Year - 1)).ToList();
                    // var CurrentYearResult = objYearTotalSales.Where(p => p.Year == System.DateTime.Now.Year).ToList();

                    //Fusion Chart Code
                    StringBuilder strXML = new StringBuilder();
                    strXML.Append("<chart caption='Sales Report For " + (System.DateTime.Now.Year - 1) + " vs " + System.DateTime.Now.Year + "' showBorder='0' xAxisName='Month' yAxisName='Revenue' showValues= '0' numberPrefix='$' animation='1' bgcolor='0000000' bgratio='0' bgalpha='100' canvasBgColor='000000' canvasbasecolor='000000' canvasbgratio='0' canvasbgalpha='100' exportShowMenuItem ='1' exportEnabled='1' >");
                    strXML.Append("<categories>");
                    strXML.Append("<category label='Jan'></category>");
                    strXML.Append("<category label='Feb'></category>");
                    strXML.Append("<category label='Mar'></category>");
                    strXML.Append("<category label='Apr'></category>");
                    strXML.Append("<category label='May'></category>");
                    strXML.Append("<category label='Jun'></category>");
                    strXML.Append("<category label='Jul'></category>");
                    strXML.Append("<category label='Aug'></category>");
                    strXML.Append("<category label='Sep'></category>");
                    strXML.Append("<category label='Oct'></category>");
                    strXML.Append("<category label='Nov'></category>");
                    strXML.Append("<category label='Dec'></category>");
                    strXML.Append("</categories>");

                    for (int i = 0; i < objYearTotalSales.Count; i++)
                    {
                        if (objYearTotalSales[i].Month == 1)
                            strXML.Append("<dataset seriesName='" + objYearTotalSales[i].Year + "'  renderAs='Area'>");
                        strXML.Append("<set value='" + objYearTotalSales[i].OrderAmount + "' ></set>");
                        if (objYearTotalSales[i].Month == 12)
                            strXML.Append("</dataset>");
                    }
                    strXML.Append("</chart>");

                    lblTotalSpend.Text = string.Format("{0:c}", objYearTotalSales.Sum(x => x.OrderAmount) + objYearTotalSales.Sum(x => x.OrderAmount));
                    lblTotalSpend.Text += "<br/>" + "Total Sales " + (System.DateTime.Now.Year - 1) + " : " + string.Format("{0:c}", objYearTotalSales.Where(m => m.Year == (System.DateTime.Now.Year - 1)).Sum(x => x.OrderAmount));
                    lblTotalSpend.Text += "<br/>" + "Total Sales " + System.DateTime.Now.Year + " : " + string.Format("{0:c}", objYearTotalSales.Where(m => m.Year == System.DateTime.Now.Year).Sum(x => x.OrderAmount));

                    //Create the chart - Pie 3D Chart with data from strXML
                    FusionCharts.SetRenderer("javascript");
                    ltrYearVsPreviousYear.Text = FusionCharts.RenderChart("../../FusionCharts/MSCombi3D", "", strXML.ToString(), "YearSales", "900", "550", false, true, false);

                }


                dvSalesForCurrentVsPreviousYear.Visible = true;
            }
            //chrtSalesByStation.Series["Series1"].Points[3].CustomProperties += "Exploded=true"; this is for exploded a slice of pie chart
        }
    }

    #endregion
}
