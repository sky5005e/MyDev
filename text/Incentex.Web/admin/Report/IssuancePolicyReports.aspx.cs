using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Web.UI.DataVisualization.Charting;

public partial class admin_Report_IssuancePolicyReports : PageBase
{
    #region  Page Variable's 
    Int64 StoreID
    {
        get { return Convert.ToInt64(ViewState["StoreID"]); }
        set { this.ViewState["StoreID"] = value; }

    }

    DateTime FromDate
    {
        get
        {
            if (ViewState["FromDate"] == null)
                return DateTime.Now;
            else
                return Convert.ToDateTime(ViewState["FromDate"]);
        }
        set
        {
            ViewState["FromDate"] = value;
        }
    }

    DateTime ToDate
    {
        get
        {
            if (ViewState["ToDate"] == null)
                return DateTime.Now;
            else
                return Convert.ToDateTime(ViewState["ToDate"]);
        }
        set
        {
            ViewState["ToDate"] = value;
        }
    }

    Int64[] arUID
    {
        get { return (Int64[])HttpContext.Current.Session["arUID"]; }
        set { HttpContext.Current.Session["arUID"] = value; }
    }
    public Int64 _threshold = 0;


    #endregion 
    #region  Page Event's
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Order Management System";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Issuance Policy Report";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Report/ReportDashBoard.aspx";

            //this is for setting search criteria
            if (!String.IsNullOrEmpty(Request.QueryString["FromDate"]))
                this.FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            if (!String.IsNullOrEmpty(Request.QueryString["ToDate"]))
                this.ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            if (!String.IsNullOrEmpty(Request.QueryString["StoreID"]))
                this.StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);

            BindChart();
        }
    }

    protected void ChrtStorePolicyName_Click(object sender, ImageMapEventArgs e)
    {
        this.StoreID = new CompanyStoreRepository().GetStoreIDByCompanyName(e.PostBackValue);
        BindChart();
    }

    protected void chrtIssuancePolicyName_Click(object sender, ImageMapEventArgs e)
    {
        String IssuanceName = e.PostBackValue;
        if (IssuanceName.Contains("Other"))
            Response.Redirect("UniformPolicyReports.aspx");
        else
        {
            Int64 UniformIssuancePolicyID = new UniformIssuancePolicyRepository().GetUniformIssuancePolicyIDbyName(IssuanceName);
            Response.Redirect("UniformPolicyReports.aspx?UID=" + UniformIssuancePolicyID);
        }
       
    }

    protected void chrtIssuancePolicyName_DataBound(object sender, EventArgs e)
    {

        chrtIssuancePolicyName.Series["Series1"].Url = "130";
    }
    #endregion
    #region  Page Method's
    private void BindChart()
    {
        try
        {
            UniformIssuancePolicyRepository objUniRepo = new UniformIssuancePolicyRepository();
            if (this.StoreID > 0)
            {
                List<GetIssuancePoliciesForReportResult> listResult = objUniRepo.GetIssuancePoliciesForReport(StoreID);
                // Get Count for other values in chart.
                Int64? OtherSum = listResult.Where(c => c.TotalCount < 500).Sum(s => s.TotalCount);
                // Allocate the ID's of Other Count
                arUID = listResult.Where(c => c.TotalCount < 500).Select(s => s.UniformIssuancePolicyID).ToArray();
                // Get Threshold values
                _threshold = GetThresholdValue(OtherSum, Convert.ToInt64(listResult.Sum(q => q.TotalCount)));

                // Set  Chart Propertities
                SetChartProperties(chrtIssuancePolicyName);
                // Bind Chart
                chrtIssuancePolicyName.DataSource = listResult;
                chrtIssuancePolicyName.DataBind();

                SetDivVisibility(true, false);
            }
            else
            {
                ChrtStorePolicyName.DataSource = objUniRepo.GetAllIssuancePolicyCount();
                ChrtStorePolicyName.DataBind();
                SetDivVisibility(false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    /// <summary>
    /// Show/Hide
    /// </summary>
    /// <param name="IsShowPolicy"></param>
    /// <param name="IsShowStore"></param>
    private void SetDivVisibility(Boolean IsShowPolicy, Boolean IsShowStore)
    {
        dvchrtIssuancePolicyName.Visible = IsShowPolicy;
        dvStorePolicyName.Visible = IsShowStore;
    }
    /// <summary>
    /// Set the Properties of Chart
    /// </summary>
    /// <param name="chartMain">Chart Name</param>
    private void SetChartProperties(Chart chartMain)
    {
        chartMain.Series["Series1"].ToolTip = "#VALY";
        chartMain.Series["Series1"].CustomProperties = "DrawingStyle=Pie,PieDrawingStyle=Concave,PieLabelStyle=Outside,PieLineColor=Gray,LabelsRadialLineSize=1, LabelsHorizontalLineSize=0,CollectedThreshold=" 
            + _threshold + ",CollectedLabel=Other,CollectedSliceExploded=True,CollectedColor=Green,CollectedToolTip=Other : #VALY";
    }
    /// <summary>
    /// Get Threshold Value
    /// </summary>
    /// <param name="ortherSum"></param>
    /// <param name="totalCount"></param>
    /// <returns></returns>
    private Int64 GetThresholdValue(Int64? ortherSum, Int64 totalCount)
    {
        Decimal _trhsval = Convert.ToDecimal((Convert.ToDecimal(ortherSum) / Convert.ToDecimal(totalCount)) * 10);
        if (Math.Round(_trhsval) < _trhsval)
            return Convert.ToInt32(Math.Round(_trhsval)) + 1;
        else
            return Convert.ToInt32(Math.Round(_trhsval));
    }

    public class ChartThreshold
    {
        public Int64 TotalCount { get; set; }
    }
    #endregion


}
