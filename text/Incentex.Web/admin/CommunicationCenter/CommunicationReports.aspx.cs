using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Web.UI.DataVisualization.Charting;

public partial class admin_CommunicationCenter_CommunicationReports : PageBase
{
    #region Data Members

    LookupRepository objLookupRepos = new LookupRepository();
    TodayEmailsRepository objTodayRepo = new TodayEmailsRepository();
    CampignRepo objCampRepo = new CampignRepo();
    Int64 CompanyID
    {
        get
        {
            if (ViewState["CompanyID"] == null)
            {
                ViewState["CompanyID"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyID"]);
        }
        set
        {
            ViewState["CompanyID"] = value;
        }
    }
    Int64 UserInfoID
    {
        get
        {
            if (ViewState["UserInfoID"] == null)
            {
                ViewState["UserInfoID"] = 0;
            }
            return Convert.ToInt64(ViewState["UserInfoID"]);
        }
        set
        {
            ViewState["UserInfoID"] = value;
        }
    }
    Int32 NumberOfDays
    {
        get
        {
            if (ViewState["NumberOfDays"] == null)
            {
                ViewState["NumberOfDays"] = 0;
            }
            return Convert.ToInt32(ViewState["NumberOfDays"]);
        }
        set
        {
            ViewState["NumberOfDays"] = value;
        }
    }
    Int32 ReportOption
    {
        get
        {
            if (ViewState["ReportOption"] == null)
            {
                ViewState["ReportOption"] = 0;
            }
            return Convert.ToInt32(ViewState["ReportOption"]);
        }
        set
        {
            ViewState["ReportOption"] = value;
        }
    }
    DateTime Date
    {
        get
        {
            if (ViewState["Date"] == null)
                return DateTime.Now;
            else
                return Convert.ToDateTime(ViewState["Date"]);
        }
        set
        {
            ViewState["Date"] = value;
        }
    }
    #endregion
    

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Reports Dashboard";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/ReportDashBoard.aspx";
            FillCompanyStore();
            if (!String.IsNullOrEmpty(Request.QueryString["cid"]))
                this.CompanyID = Convert.ToInt64(Request.QueryString["cid"]);

            if (!String.IsNullOrEmpty(Request.QueryString["uid"]))
                this.UserInfoID = Convert.ToInt64(Request.QueryString["uid"]);

            if (!String.IsNullOrEmpty(Request.QueryString["dt"]))
                this.Date = Convert.ToDateTime(Request.QueryString["dt"]);

            if (!String.IsNullOrEmpty(Request.QueryString["nday"]))
                this.NumberOfDays = Convert.ToInt32(Request.QueryString["nday"]);
            if(!String.IsNullOrEmpty(Request.QueryString["rptop"]))
                this.ReportOption = Convert.ToInt32(Request.QueryString["rptop"]);

            if (ReportOption == 0)
            {
                List<TodayEmailsRepository.GetTodaysSentEmail> objResult = objTodayRepo.GetAllTodaysEmailSent(CompanyID, UserInfoID, Date, NumberOfDays);
                if (objResult.Count > 0)
                {
                    dvChartsTodaysMails.Visible = true;
                    Int64? _maxInterval = objResult.Max(y => y.Count);
                    SetChartProperties(ChartsTodaysComm, _maxInterval, Date, NumberOfDays, this.ReportOption);
                    ChartsTodaysComm.DataSource = objResult;
                    ChartsTodaysComm.DataBind();
                }
            }
            else if (ReportOption == 1)
            {
                List<TodayEmailsRepository.GetTodaysSentEmail> objResult = objTodayRepo.GetAllWeekEmailSent(CompanyID, UserInfoID, Date, NumberOfDays);
                if (objResult.Count > 0)
                {
                    Int64? _maxInterval = objResult.Max(y => y.Count);
                    SetChartProperties(ChartbyDays, _maxInterval, Date, NumberOfDays, this.ReportOption);
                    ChartbyDays.DataSource = objResult;
                    ChartbyDays.DataBind();
                    dvChartsByDays.Visible = true;
                }
            }
            else if (ReportOption == 2)
            {
                List<CampignRepo.GetTodaysReadEmail> objResult = objCampRepo.GetAllTodaysEmailRead(CompanyID, UserInfoID, Date, NumberOfDays);
                if (objResult.Count > 0)
                {
                    dvChartsTodaysMails.Visible = true;
                    Int64? _maxInterval = objResult.Max(y => y.Count);
                    SetChartProperties(ChartsTodaysComm, _maxInterval, Date, NumberOfDays, this.ReportOption);
                    ChartsTodaysComm.DataSource = objResult;
                    ChartsTodaysComm.DataBind();
                }
            }
            else if (ReportOption == 3)
            {
                List<CampignRepo.GetTodaysReadEmail> objResult = objCampRepo.GetAllWeekEmailRead(CompanyID, UserInfoID, Date, NumberOfDays);
                if (objResult.Count > 0)
                {
                    Int64? _maxInterval = objResult.Max(y => y.Count);
                    SetChartProperties(ChartbyDays, _maxInterval,Date,NumberOfDays,this.ReportOption);
                    ChartbyDays.DataSource = objResult;
                    ChartbyDays.DataBind();
                    dvChartsByDays.Visible = true;
                }
            }
        }
    }

    protected void lnkGeneratesReports_Click(object sender, EventArgs e)
    {
        DateTime SearchDate = DateTime.Now;
        Int64 CompanyID = 0;
        if (txtFromDate.Text != "")
            SearchDate = Convert.ToDateTime(txtFromDate.Text.Trim());

        if (ddlCompanyStore.SelectedIndex > 0)
            CompanyID = Convert.ToInt64(ddlCompanyStore.SelectedValue);

        if (ddlReportOptions.SelectedValue == "0")// For sent mail by hours
        {
            List<TodayEmailsRepository.GetTodaysSentEmail> objResult = objTodayRepo.GetAllTodaysEmailSent(CompanyID, SearchDate);
            if (objResult.Count > 0)
            {
                Int64? _maxInterval = objResult.Max(y => y.Count);
                SetChartProperties(ChartsTodaysComm, _maxInterval, SearchDate, 0, Convert.ToInt32(ddlReportOptions.SelectedValue));
                ChartsTodaysComm.DataSource = objResult;
                ChartsTodaysComm.DataBind();
                dvChartsTodaysMails.Visible = true;
                dvChartsByDays.Visible = false;
                lblMsg.Text = "Below chart display all Email's Sent by Hours  :" + SearchDate.Date.ToShortDateString();
            }
            else
            {
                dvChartsTodaysMails.Visible = false;
                dvChartsByDays.Visible = false;
                lblMsg.Text = "There is no records founded with this criteria  :" + SearchDate.Date.ToShortDateString(); 
            }
        }
        else if (ddlReportOptions.SelectedValue == "1")// For sent mail by days
        {
            // To get the First date of week (ie Monday)
            int diff = DayOfWeek.Monday - SearchDate.DayOfWeek;
            DateTime mondaydate = SearchDate.AddDays(diff);

            List<TodayEmailsRepository.GetTodaysSentEmail> objResult = objTodayRepo.GetAllWeekEmailSent(CompanyID, mondaydate);
            if (objResult.Count > 0)
            {
                Int64? _maxInterval = objResult.Max(y => y.Count);
                SetChartProperties(ChartbyDays, _maxInterval, SearchDate, 0, Convert.ToInt32(ddlReportOptions.SelectedValue));
                ChartbyDays.DataSource = objResult;
                ChartbyDays.DataBind();
                dvChartsByDays.Visible = true;
                dvChartsTodaysMails.Visible = false;
                lblMsg.Text = "Below chart display all Email's Sent by week  :" + SearchDate.Date.ToShortDateString(); 
            }
            else
            {
                dvChartsByDays.Visible = false;
                dvChartsTodaysMails.Visible = false;
                lblMsg.Text = "There is no records founded with this criteria  :" + SearchDate.Date.ToShortDateString(); 
            }
            
        }
        else if (ddlReportOptions.SelectedValue == "2")// For viewed mail by hours
        {

            List<CampignRepo.GetTodaysReadEmail> objResult = objCampRepo.GetAllTodaysEmailRead(CompanyID, SearchDate);
            if (objResult.Count > 0)
            {
                Int64? _maxInterval = objResult.Max(y => y.Count);
                SetChartProperties(ChartsTodaysComm, _maxInterval, SearchDate, 0, Convert.ToInt32(ddlReportOptions.SelectedValue));
                ChartsTodaysComm.DataSource = objResult;
                ChartsTodaysComm.DataBind();
                dvChartsTodaysMails.Visible = true;
                dvChartsByDays.Visible = false;
                lblMsg.Text = "Below chart display all Email's opened by Hours  :" + SearchDate.Date.ToShortDateString();
            }
            else
            {
                dvChartsTodaysMails.Visible = false;
                dvChartsByDays.Visible = false;
                lblMsg.Text = "There is no records founded with this criteria  :" + SearchDate.Date.ToShortDateString();
            }

        }
        else if (ddlReportOptions.SelectedValue == "3")// For viewed mail by days
        {
            // To get the First date of week (ie Monday)
            int diff = DayOfWeek.Monday - SearchDate.DayOfWeek;
            DateTime mondaydate = SearchDate.AddDays(diff);

            List<CampignRepo.GetTodaysReadEmail> objResult = objCampRepo.GetAllWeekEmailRead(CompanyID, mondaydate);
            if (objResult.Count > 0)
            {
                Int64? _maxInterval = objResult.Max(y => y.Count);
                SetChartProperties(ChartbyDays, _maxInterval,SearchDate,0,Convert.ToInt32(ddlReportOptions.SelectedValue));
                ChartbyDays.DataSource = objResult;
                ChartbyDays.DataBind();
                dvChartsByDays.Visible = true;
                dvChartsTodaysMails.Visible = false;
                lblMsg.Text = "Below chart display all Email's Sent by week  :" + SearchDate.Date.ToShortDateString();
            }
            else
            {
                dvChartsByDays.Visible = false;
                dvChartsTodaysMails.Visible = false;
                lblMsg.Text = "There is no records founded with this criteria  :" + SearchDate.Date.ToShortDateString();
            }
        }
        ResetControls();
    }
    #endregion

    #region Methods
    private void ResetControls()
    {
        ddlCompanyStore.SelectedIndex = 0;
        txtFromDate.Text = null;
    }
    private void FillCompanyStore()
    {
        //get company
        List<Company> objcomList = new List<Company>();
        CompanyRepository objRepo = new CompanyRepository();
        objcomList = objRepo.GetAllCompany().OrderBy(c => c.CompanyName).ToList();
        Common.BindDDL(ddlCompanyStore, objcomList, "CompanyName", "CompanyID", "-Select Company-");
    }
    /// <summary>
    /// Set the Properties of Chart
    /// </summary>
    /// <param name="chartMain">Chart Name</param>
    /// <param name="MaxInterval">Maximum value of list to set the Yaxis Interval</param>
    private void SetChartProperties(Chart chartMain,Int64? MaxInterval,DateTime dt, Int32 nday, Int32 rptOption)
    {
        Int32 Interval = 5;
        if (MaxInterval < 10)
            Interval = 5;
        else if (MaxInterval > 10 && MaxInterval < 50)
            Interval = 10;
        else if (MaxInterval > 50 && MaxInterval < 100)
            Interval = 25;
        else if (MaxInterval > 100 && MaxInterval < 500)
            Interval = 50;
        else if (MaxInterval > 500 && MaxInterval < 1000)
            Interval = 150;
        else if (MaxInterval > 1000 && MaxInterval < 1500)
            Interval = 300;
        else if (MaxInterval > 1500 && MaxInterval < 2500)
            Interval = 400;
        else if (MaxInterval > 2500)
            Interval = 500;
        // show an Y label series
        chartMain.ChartAreas[0].AxisY.Interval = Interval;
        chartMain.ChartAreas[0].AxisY.IsStartedFromZero = true;
        // Only to pass 
        String xValue = chartMain.Series["Series1"].ToolTip = "#VALX";
        chartMain.Series["Series1"].ToolTip = "#VALY";

        chartMain.Series["Series1"].Url = "Userlist.aspx?dt=" + dt.Date.ToShortDateString() + "&AxisSeries=" + xValue + "&nday=" + nday + "&rptOp=" + rptOption + "&cid=" + this.CompanyID + "&uid=" + this.UserInfoID;
        

    }
    #endregion

}
