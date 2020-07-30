using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Data;

public partial class MyAccount_Report_SalesSummaryReport : PageBase
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
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Sales Summary Dashboard";
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

    protected void chrtSalesByStation_Click(object sender, ImageMapEventArgs e)
    {
        INC_BasedStation objINC_BasedStation = new BaseStationRepository().GetByName(e.PostBackValue);
        Int64? StoreID = new CompanyStoreRepository().GetStoreIDByCompanyId((Int64)IncentexGlobal.CurrentMember.CompanyId);
        if (objINC_BasedStation != null)
        {
            Response.Redirect("~/OrderManagement/OrderView.aspx?ChartSubReport=" + SubReport + "&StoreId=" + StoreID + "&ToDate=" + txtToDate.Text.Trim() + "&FromDate=" + txtFromDate.Text.Trim() + "&Email=&OrderNumber=&OrdeStatus=" + "" + "&WorkGroup=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : "") + "&FName=&LName=&EmployeeCode=&StationCode=" + objINC_BasedStation.iBaseStationId + "&PaymentType=&page=myaccount&WorkGroupAccess=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : this.WorkGroupIDs) + "&BSA=" + objINC_BasedStation.iBaseStationId);
        }
    }
    protected void chrtSalesByPersonalProtectiveEquiptment_Click(object sender, ImageMapEventArgs e)
    {
        INC_BasedStation objINC_BasedStation = new BaseStationRepository().GetByName(e.PostBackValue);
        Int64? StoreID = new CompanyStoreRepository().GetStoreIDByCompanyId((Int64)IncentexGlobal.CurrentMember.CompanyId);
        if (objINC_BasedStation != null)
        {
            Response.Redirect("~/OrderManagement/OrderView.aspx?ChartSubReport=" + SubReport + "&StoreId=" + StoreID + "&ToDate=" + txtToDate.Text.Trim() + "&FromDate=" + txtFromDate.Text.Trim() + "&Email=&OrderNumber=&OrdeStatus=" + "" + "&WorkGroup=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : "") + "&FName=&LName=&EmployeeCode=&StationCode=" + objINC_BasedStation.iBaseStationId + "&PaymentType=&page=myaccount&WorkGroupAccess=" + (ddlWorkgroup.SelectedValue != "0" ? ddlWorkgroup.SelectedValue : this.WorkGroupIDs) + "&BSA=" + objINC_BasedStation.iBaseStationId);
        }
    }
    #endregion

    #region Methods

    private void FillBasedStation()
    {
        List<INC_BasedStation> basestationList = objReportAccessRightsRepository.GetBaseStationByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 451);
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
        List<INC_Lookup> WorkgroupList = objReportAccessRightsRepository.GetWorkgroupByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 451);
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
            if (SubReport == "Spend By Station Location")
            {
                var objResult = objReportForEmployeeRepository.GetSalesStationWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null && objResult.Count > 0)
                {
                    this.WorkGroupIDs = objResult.FirstOrDefault().WorkGroupIDs;
                    this.BaseStationIDs = objResult.FirstOrDefault().BaseStationIDs;
                    
                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => x.OrderAmount));
                    //lblTotalSpend.Text += "<br/>" + "Total Station : " + string.Format("{0:c}", objResult.Count());
                    dvTotalSpend.Visible = true;
                }
                else
                {
                    lblTotalSpend.Text = string.Format("{0:c}", "0.00");
                    dvTotalSpend.Visible = false;
                }
                chrtSalesByStation.DataSource = objResult;
                chrtSalesByStation.DataBind();
                dvSalesByStation.Visible = true;
            }
            else if (SubReport == "Spend By Workgroup")
            {
                var objResult = objReportForEmployeeRepository.GetSalesWorkgroupWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => x.OrderAmount));
                    dvTotalSpend.Visible = true;
                }
                else
                {
                    lblTotalSpend.Text = string.Format("{0:c}", "0.00");
                    dvTotalSpend.Visible = false;
                }

                chrtSalesByWorkgroup.DataSource = objResult;
                chrtSalesByWorkgroup.DataBind();

                dvSalesByWorkgroup.Visible = true;
            }
            else if (SubReport == "Spend By Workgroup%")
            {
                var objResult = objReportForEmployeeRepository.GetSalesWorkgroupWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                chrtSalesByWorkgroupPer.DataSource = objResult;
                chrtSalesByWorkgroupPer.DataBind();
                if (objResult != null)
                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => x.OrderAmount));
                dvSalesByWorkgroupPer.Visible = true;
                dvTotalSpend.Visible = false;
            }
            else if (SubReport == "Total Program Spend Since Start Date")
            {
                var objResult = objReportForEmployeeRepository.GetSalesYearWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                chrtSalesByYear.DataSource = objResult;
                chrtSalesByYear.DataBind();
                if (objResult != null)
                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => x.OrderAmount));
                dvSalesByYear.Visible = true;
                dvTotalSpend.Visible = false;
            }
            else if (SubReport == "Total Company Spend Vs. Employee Spend")
            {
                List<ReportForEmployeeRepository.GetSalesCompanyVSEmployeeWiseResult> objSalesCompanyWise = objReportForEmployeeRepository.GetSalesCompanyWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                List<ReportForEmployeeRepository.GetSalesCompanyVSEmployeeWiseResult> objSalesEmployeeWise = objReportForEmployeeRepository.GetSalesEmployeeWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);


                if (objSalesCompanyWise != null && objSalesEmployeeWise !=null)
                {
                    chrtSalesByCompanyVSEmployee.Series["SeriesForCompany"].Points.DataBindXY(objSalesCompanyWise, "Month", objSalesCompanyWise, "OrderAmount");
                    chrtSalesByCompanyVSEmployee.Series["SeriesForEmployee"].Points.DataBindXY(objSalesEmployeeWise, "Month", objSalesEmployeeWise, "OrderAmount");

                    lblTotalSpend.Text = string.Format("{0:c}", objSalesCompanyWise.Sum(x => x.OrderAmount) + objSalesEmployeeWise.Sum(x => x.OrderAmount));
                    lblTotalSpend.Text += "<br/>" + "Total Employee Spend : " + string.Format("{0:c}", objSalesEmployeeWise.Sum(x => x.OrderAmount));
                    lblTotalSpend.Text += "<br/>" + "Total Company Spend : " + string.Format("{0:c}", objSalesCompanyWise.Sum(x => x.OrderAmount));
                }
                dvSalesByCompanyVSEmployee.Visible = true;
                dvTotalSpend.Visible = false;
            }
            else if (SubReport == "Total Company Spend By Month")
            {
                List<ReportForEmployeeRepository.GetSalesCompanyVSEmployeeWiseResult> objResult = objReportForEmployeeRepository.GetSalesCompanyWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                chrtSalesByCompany.DataSource = objResult;
                chrtSalesByCompany.DataBind();
                if (objResult != null)
                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => x.OrderAmount));
                dvSalesByCompany.Visible = true;
                dvTotalSpend.Visible = false;
            }
            else if (SubReport == "Personal Protective Equiptment")
            {
                Int64? ReportTagID = objLookupRepos.GetIdByLookupNameNLookUpCode("Personal Protective Equiptment", "ReportTag");
                List<ReportForEmployeeRepository.GetSalesStationWiseResult> objResult = objReportForEmployeeRepository.GetSalesByReportTag(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, ReportTagID == null ? 0 : Convert.ToInt64(ReportTagID));
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
                dvTotalSpend.Visible = false;
            }
            else if (SubReport == "Spanx Sales Summary")
            {
                Int64? ReportTagID = objLookupRepos.GetIdByLookupNameNLookUpCode("Spanx", "ReportTag");
                List<ReportForEmployeeRepository.GetSalesStationWiseResult> objResult = objReportForEmployeeRepository.GetSalesByReportTag(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, ReportTagID == null ? 0 : Convert.ToInt64(ReportTagID));
                if (objResult != null)
                {
                    chrtSalesBySpanx.DataSource = objResult;
                    chrtSalesBySpanx.DataBind();
                    lblTotalSpend.Text = string.Format("{0:c}", objResult.Sum(x => x.OrderAmount));
                }
                dvSalesBySpanx.Visible = true;
                dvTotalSpend.Visible = false;
            }
        }
    }

    #endregion
}
