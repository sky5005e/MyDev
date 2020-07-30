using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL.Common;

public partial class UserPages_AdminReports : System.Web.UI.Page
{

    #region Data members
    DateTime? FromDate
    {
        get
        {
            if (ViewState["FromDate"] == null)
                return null;
            return Convert.ToDateTime(ViewState["FromDate"]);
        }
        set
        {
            ViewState["FromDate"] = value;
        }
    }

    DateTime? ToDate
    {
        get
        {
            if (ViewState["ToDate"] == null)
                return null;
            return Convert.ToDateTime(ViewState["ToDate"]);
        }
        set
        {
            ViewState["ToDate"] = value;
        }
    }

    Int64? WorkGroupID
    {
        get
        {
            if (ViewState["WorkGroupID"] == null)
                return null;
            return Convert.ToInt64(ViewState["WorkGroupID"]);
        }
        set
        {
            ViewState["WorkGroupID"] = value;
        }
    }

    Int64? BaseStationID
    {
        get
        {
            if (ViewState["BaseStationID"] == null)
                return null;
            return Convert.ToInt64(ViewState["BaseStationID"]);
        }
        set
        {
            ViewState["BaseStationID"] = value;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindSearchDropDowns();
            FillPeriod();
            ddlPeriod.Items.Insert(5, new ListItem("Current Year", System.Data.Linq.SqlClient.SqlMethods.DateDiffDay(Convert.ToDateTime("01/01/" + DateTime.Now.Year), System.DateTime.Now).ToString()));//Add new item for current year
            txtFromDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
        }
    }

    #region Events
    protected void btnGenerateReport_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(txtFromDate.Text.Trim()))
                FromDate = Convert.ToDateTime(txtFromDate.Text.Trim());
            else
                FromDate = null;
            if (!String.IsNullOrEmpty(txtToDate.Text.Trim()))
                ToDate = Convert.ToDateTime(txtToDate.Text.Trim());
            else
                ToDate = null;
            if (ddlWorkgroup.SelectedValue != "0")
                WorkGroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            else
                WorkGroupID = null;
            if (ddlBaseStation.SelectedValue != "0")
                BaseStationID = Convert.ToInt64(ddlBaseStation.SelectedValue);
            else
                BaseStationID = null;

            if (ddlReport.SelectedItem.Text.ToLower() == "spend by station")
                BindSpendByLocation();
            else if (ddlReport.SelectedItem.Text.ToLower() == "spend by workgroup")
                BindSpendByWorkGroup();

            Report_title_span.InnerText = ddlReport.SelectedItem.Text;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlPeriod.SelectedValue == "99999")//This is for date range
            {
                fromDate_li.Visible = true;
                toDate_li.Visible = true;
            }
            else
            {
                if (Convert.ToInt64(ddlPeriod.SelectedValue) < 367)
                {
                    txtFromDate.Text = DateTime.Now.AddDays(-Convert.ToInt64(ddlPeriod.SelectedValue)).ToString("MM/dd/yyyy");
                    txtToDate.Text = "";
                }
                else
                {
                    txtFromDate.Text = "01/01/" + ddlPeriod.SelectedValue;
                    txtToDate.Text = "12/31/" + ddlPeriod.SelectedValue;
                }
                fromDate_li.Visible = false;
                toDate_li.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion

    #region Methods

    protected void BindSearchDropDowns()
    {
        try
        {
            LookupRepository objLookupRepository = new LookupRepository();
            List<INC_Lookup> objList = new List<INC_Lookup>();
            String LookUpCode = string.Empty;

            //Bind Equipment Types
            LookUpCode = "Reports";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlReport, objList, "sLookupName", "iLookupID", "- Select Report -");


            //Bind Equipment Types
            LookUpCode = "Workgroup";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDDL(ddlWorkgroup, objList, "sLookupName", "iLookupID", "- Select Workgroup -");

            //Bind Location/Base Stations
            CompanyRepository objRepo = new CompanyRepository();
            List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
            objBaseStationList = objRepo.GetAllBaseStationResult();
            objBaseStationList = objBaseStationList.OrderBy(p => p.sBaseStation).ToList();
            Common.BindDDL(ddlBaseStation, objBaseStationList, "sBaseStation", "iBaseStationId", "- Select Base Station -");

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillPeriod()
    {
        ddlPeriod.ClearSelection();
        ddlPeriod.Items.Clear();
        ddlPeriod.DataSource = Common.BindPeriodDropDownItems();
        ddlPeriod.DataValueField = "Value";
        ddlPeriod.DataTextField = "Text";
        ddlPeriod.DataBind();

        ddlPeriod.Items.Insert(0, new ListItem("- Select Date Range -", "0"));
    }

    protected void BindSpendByLocation()
    {
        ReportForEmployeeRepository objReportForEmployeeRepository = new ReportForEmployeeRepository();
        ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();
      
        var objResult = objReportForEmployeeRepository.GetSalesStationWise(IncentexGlobal.CurrentMember.UserInfoID, this.FromDate, this.ToDate, null, this.WorkGroupID, this.BaseStationID, null);
        
        //Spend By Location
        if (objResult != null && objResult.Count > 0)
        {
            this.WorkGroupIDs = objResult.FirstOrDefault().WorkGroupIDs;
            this.BaseStationIDs = objResult.FirstOrDefault().BaseStationIDs;

            lblTotalSpend.Text = objResult.Sum(x => x.OrderAmount).Value.ToString("c2");
            //lblTotalSpend.Text += "<br/>" + "Total Station : " + string.Format("{0:c}", objResult.Count());
            dvTotalSpend.Visible = true;
            nodata_msg.Visible = false;
        }
        else
        {
            lblTotalSpend.Text = "0.00";
            dvTotalSpend.Visible = false;
            nodata_msg.Visible = true;
        }
        PieChart.DataSource = objResult;
        PieChart.Series["Series1"].XValueMember = "BaseStation";
        PieChart.Series["Series1"].YValueMembers = "OrderAmount";
        PieChart.DataBind();
    }

    protected void BindSpendByWorkGroup()
    {
        ReportForEmployeeRepository objReportForEmployeeRepository = new ReportForEmployeeRepository();
        ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();

        var objResult = objReportForEmployeeRepository.GetSalesWorkgroupWise(IncentexGlobal.CurrentMember.UserInfoID, this.FromDate, this.ToDate, null, this.WorkGroupID, this.BaseStationID, null);

        //Spend By WorkGroup
        if (objResult != null && objResult.Count > 0)
        {
            lblTotalSpend.Text = objResult.Sum(x => x.OrderAmount).Value.ToString("c2");
            dvTotalSpend.Visible = true;
            nodata_msg.Visible = false;
        }
        else
        {
            lblTotalSpend.Text = "0.00";
            dvTotalSpend.Visible = false;
            nodata_msg.Visible = true;
        }
        
        PieChart.DataSource = objResult;
        PieChart.Series["Series1"].XValueMember = "Workgroup";
        PieChart.Series["Series1"].YValueMembers = "OrderAmount";
        PieChart.DataBind();
    }

    #endregion
}
