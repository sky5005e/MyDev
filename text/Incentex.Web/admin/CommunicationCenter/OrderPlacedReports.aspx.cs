using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Web.UI.DataVisualization.Charting;

public partial class admin_CommunicationCenter_OrderPlacedReports : PageBase
{
    #region Data Members

    LookupRepository objLookupRepos = new LookupRepository();
    TodayEmailsRepository objTodayRepo = new TodayEmailsRepository();
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
            base.MenuItem = "Order Placed Reports";
            base.ParentMenuID = 29;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>"; ;
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Order Placed Reports";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/CommunicationCenter/CampaignSelection.aspx?IsOrderPlaced=1";

            if (!String.IsNullOrEmpty(Request.QueryString["cid"]))
                this.CompanyID = Convert.ToInt64(Request.QueryString["cid"]);

            if (!String.IsNullOrEmpty(Request.QueryString["uid"]))
                this.UserInfoID = Convert.ToInt64(Request.QueryString["uid"]);

            if (!String.IsNullOrEmpty(Request.QueryString["dt"]))
                this.Date = Convert.ToDateTime(Request.QueryString["dt"]);

            if (!String.IsNullOrEmpty(Request.QueryString["nday"]))
                this.NumberOfDays = Convert.ToInt32(Request.QueryString["nday"]);
            if (!String.IsNullOrEmpty(Request.QueryString["rptop"]))
                this.ReportOption = Convert.ToInt32(Request.QueryString["rptop"]);

            // Call
            List<TodayEmailsRepository.GetTodaysSentEmail> objResult = objTodayRepo.GetAllOrderPlacedList(CompanyID, UserInfoID, Date, NumberOfDays);
            if (objResult.Count > 0)
            {
                dvChartsTodaysMails.Visible = true;
                Int64? _maxInterval = objResult.Max(y => y.Count);
                SetChartProperties(ChartsTodaysComm, _maxInterval, Date, NumberOfDays, this.ReportOption);
                ChartsTodaysComm.DataSource = objResult;
                ChartsTodaysComm.DataBind();
            }

        }


    }
    #endregion 

    #region Page Method's

    /// <summary>
    /// Set the Properties of Chart
    /// </summary>
    /// <param name="chartMain">Chart Name</param>
    /// <param name="MaxInterval">Maximum value of list to set the Yaxis Interval</param>
    private void SetChartProperties(Chart chartMain, Int64? MaxInterval, DateTime dt, Int32 nday, Int32 rptOption)
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

        chartMain.Series["Series1"].Url = "Userlist.aspx?dt=" + dt.Date.ToShortDateString() + "&AxisSeries=" + xValue + "&nday=" + nday + "&rptOp=" + rptOption + "&cid=" + this.CompanyID + "&uid=" + this.UserInfoID + "&IsOrderPlaced=1";


    }
    #endregion 
}
