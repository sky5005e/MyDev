using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_Report_EmployeeDataReport : PageBase
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
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Employee Information Dashboard";
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
    #endregion

    #region Methods

    private void FillBasedStation()
    {
        List<INC_BasedStation> basestationList = objReportAccessRightsRepository.GetBaseStationByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 453);
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
        List<INC_Lookup> WorkgroupList = objReportAccessRightsRepository.GetWorkgroupByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 453);
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
            if (SubReport == "Employees by Station")
            {
                List<ReportForEmployeeRepository.GetEmployeeWiseResult> objResult = objReportForEmployeeRepository.GetEmployeeStationWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtEmployeeByStation.DataSource = objResult;
                    chrtEmployeeByStation.DataBind();
                }
                dvEmployeeByStation.Visible = true;
            }
            else if (SubReport == "Employee Totals by Workgroup")
            {
                List<ReportForEmployeeRepository.GetEmployeeWiseResult> objResult = objReportForEmployeeRepository.GetEmployeeWorkgroupWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    objResult = objResult.OrderByDescending(x => x.Text).ToList();
                    chrtEmployeeByWorkgroup.DataSource = objResult;
                    chrtEmployeeByWorkgroup.DataBind();
                }
                dvEmployeeByWorkgroup.Visible = true;
            }
            else if (SubReport == "Employee Totals by Gender")
            {
                List<ReportForEmployeeRepository.GetEmployeeWiseResult> objResult = objReportForEmployeeRepository.GetEmployeeGenderWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtEmployeeByGender.DataSource = objResult;
                    chrtEmployeeByGender.DataBind();

                    try
                    {
                        for (int i = 0; i < chrtEmployeeByGender.Series["Series1"].Points.Count; i++)
                        {
                            if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtEmployeeByGender.Series["Series1"].Points[i])).AxisLabel == "Male")
                                chrtEmployeeByGender.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml("#4CC7F0");
                            else
                                chrtEmployeeByGender.Series["Series1"].Points[i].Color = System.Drawing.ColorTranslator.FromHtml("#EB26E5");
                        }
                    }
                    catch
                    {
                    }
                }
                dvEmployeeByGender.Visible = true;
            }
            else if (SubReport == "Gender by Workgroup")
            {
                List<ReportForEmployeeRepository.GetEmployeeWiseResult> objEmployeeMaleWise = objReportForEmployeeRepository.GetEmployeeWorkgroupWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, 75);
                List<ReportForEmployeeRepository.GetEmployeeWiseResult> objEmployeeFemaleWise = objReportForEmployeeRepository.GetEmployeeWorkgroupWise(IncentexGlobal.CurrentMember.UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, 63);
                if (objEmployeeMaleWise != null && objEmployeeFemaleWise != null)
                {
                    objEmployeeMaleWise = objEmployeeMaleWise.OrderByDescending(x => x.Text).ToList();
                    objEmployeeFemaleWise = objEmployeeFemaleWise.OrderByDescending(x => x.Text).ToList();
                    chrtEmployeeGenderByWorkgroup.Series["SeriesForMale"].Points.DataBindXY(objEmployeeMaleWise, "Text", objEmployeeMaleWise, "Value");
                    chrtEmployeeGenderByWorkgroup.Series["SeriesForFemale"].Points.DataBindXY(objEmployeeFemaleWise, "Text", objEmployeeFemaleWise, "Value");
                }
                dvEmployeeGenderByWorkgroup.Visible = true;
            }
            else if (SubReport == "Employment Status")
            {
                trPeriod.Visible = false;
                trFromDate.Visible = false;
                trToDate.Visible = false;
                List<ReportForEmployeeRepository.GetEmployeeWiseResult> objResult = objReportForEmployeeRepository.GetEmployeeStatusWise(IncentexGlobal.CurrentMember.UserInfoID, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtEmployeeByStatus.DataSource = objResult;
                    chrtEmployeeByStatus.DataBind();

                    try
                    {
                        for (int i = 0; i < chrtEmployeeByStatus.Series["Series1"].Points.Count; i++)
                        {
                            if (((System.Web.UI.DataVisualization.Charting.DataPointCustomProperties)(chrtEmployeeByStatus.Series["Series1"].Points[i])).AxisLabel == "Active")
                                chrtEmployeeByStatus.Series["Series1"].Points[i].Color = System.Drawing.Color.Green;
                            else
                                chrtEmployeeByStatus.Series["Series1"].Points[i].Color = System.Drawing.Color.Red;
                        }
                    }
                    catch
                    {
                    }
                }
                dvEmployeeByStatus.Visible = true;
            }
        }
    }
    #endregion
}
