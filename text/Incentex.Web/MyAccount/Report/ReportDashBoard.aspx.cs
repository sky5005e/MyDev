using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Linq;

public partial class MyAccount_Report_ReportDashBoard : PageBase
{
    #region Data Members
    ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>"; ;
            ((Label)Master.FindControl("lblPageHeading")).Text = "Report DashBoard";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            FillPeriod();
            ddlPeriod.Items.Insert(5, new ListItem("Current Year", System.Data.Linq.SqlClient.SqlMethods.DateDiffDay(Convert.ToDateTime("01/01/" + DateTime.Now.Year), System.DateTime.Now).ToString()));//Add new item for current year
            bindReportDashboard();
        }

    }

    protected void ddlParentReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlChildReport.Items.Clear();

        List<INC_Lookup> objlist = objReportAccessRightsRepository.GetChildReportByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(ddlParentReport.SelectedItem.Value));
        if (objlist != null)
        {

            ddlChildReport.DataSource = objlist.OrderBy(x => x.sLookupName);
            ddlChildReport.DataValueField = "iLookupID";
            ddlChildReport.DataTextField = "sLookupName";
            ddlChildReport.DataBind();
        }
        ddlChildReport.Items.Insert(0, new ListItem("-select-", "0"));
        trMasterItem.Visible = false;
        trPeriod.Visible = true;
        ddlPeriod.SelectedValue = "0";
        ddlPeriod_SelectedIndexChanged(null, null);
    }

    protected void ddlChildReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlChildReport.SelectedItem.Text == "Item History Snapshot")
        {
            ddlMasterItem.Items.Clear();
            List<INC_Lookup> objlist = new StoreProductRepository().GetActiveMasterItems(new CompanyStoreRepository().GetStoreIDByCompanyId((Int64)IncentexGlobal.CurrentMember.CompanyId));
            if (objlist != null)
            {
                ddlMasterItem.DataSource = objlist;
                ddlMasterItem.DataValueField = "iLookupID";
                ddlMasterItem.DataTextField = "sLookupName";
                ddlMasterItem.DataBind();
            }
            ddlMasterItem.Items.Insert(0, new ListItem("-select master item-", "0"));
            trMasterItem.Visible = true;
        }
        else
        {
            ddlPeriod_SelectedIndexChanged(null, null);

            if (ddlChildReport.SelectedItem.Text == "Credits by Station" || ddlChildReport.SelectedItem.Text == "Credits by Workgroup" || ddlChildReport.SelectedItem.Text == "Summary Product Review" || ddlChildReport.SelectedItem.Text == "Items with Backorders" || ddlChildReport.SelectedItem.Text == "Employment Status" || ddlChildReport.SelectedItem.Text == "Customer Owned Inventory")
                trPeriod.Visible = false;
            else
                trPeriod.Visible = true;

            ddlMasterItem.Items.Clear();
            trMasterItem.Visible = false;
        }
    }

    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
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
    }

    protected void lnkSubmitRequest_Click(object sender, EventArgs e)
    {
        string PeriodValue = ddlPeriod.SelectedValue.Trim() == "99999" ? "" : ddlPeriod.SelectedValue.Trim();

        if (ddlParentReport.SelectedItem.Text.Contains("Spend Summary"))
            Response.Redirect("SalesSummaryReport.aspx?SubReport=" + ddlChildReport.SelectedItem.Text + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&Period=" + PeriodValue);
        else if (ddlParentReport.SelectedItem.Text.Contains("Employee Information"))
            Response.Redirect("EmployeeDataReport.aspx?SubReport=" + ddlChildReport.SelectedItem.Text + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&Period=" + PeriodValue);
        else if (ddlParentReport.SelectedItem.Text.Contains("Product Planning"))
            Response.Redirect("ProductPlanningReport.aspx?SubReport=" + ddlChildReport.SelectedItem.Text + "&MasterItemID=" + ddlMasterItem.SelectedValue + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&Period=" + PeriodValue);
        else if (ddlParentReport.SelectedItem.Text.Contains("Service Level Scorecard"))
            Response.Redirect("ServiceLevelScorecardReport.aspx?SubReport=" + ddlChildReport.SelectedItem.Text + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&Period=" + PeriodValue);
        else if (ddlParentReport.SelectedItem.Text.Contains("Order Management"))
            Response.Redirect("OrderManagementReport.aspx?SubReport=" + ddlChildReport.SelectedItem.Text + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&Period=" + PeriodValue);

    }

    #endregion

    #region Methods
    private void FillPeriod()
    {
        ddlPeriod.ClearSelection();
        ddlPeriod.Items.Clear();
        ddlPeriod.DataSource = Common.BindPeriodDropDownItems();
        ddlPeriod.DataValueField = "Value";
        ddlPeriod.DataTextField = "Text";
        ddlPeriod.DataBind();
    }
    public void bindReportDashboard()
    {
        List<INC_Lookup> objlist = objReportAccessRightsRepository.GetParentReportByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);
        if (objlist != null)
        {
            ddlParentReport.DataSource = objlist.OrderBy(x => x.sLookupName);
            ddlParentReport.DataValueField = "iLookupID";
            ddlParentReport.DataTextField = "sLookupName";
            ddlParentReport.DataBind();
            ddlParentReport.Items.Insert(0, new ListItem("-select-", "0"));
            ddlChildReport.Items.Insert(0, new ListItem("-select-", "0"));
        }
    }
    #endregion
}
