using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Linq;

public partial class admin_Report_ReportDashBoard : PageBase
{
    #region Data Members
    LookupRepository objLookupRepository = new LookupRepository();
    StoreProductRepository objStoreProductRepository = new StoreProductRepository();
    ReportAccessRightsRepository objReportAccessRightsRepository = new ReportAccessRightsRepository();
    Boolean IsFromIndexPage
    {
        get
        {
            if (ViewState["IsFromIndexPage"] == null)
            {
                ViewState["IsFromIndexPage"] = false;
            }
            return Convert.ToBoolean(ViewState["IsFromIndexPage"]);
        }
        set
        {
            ViewState["IsFromIndexPage"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["IsFromIndexPage"]) && Request.QueryString["IsFromIndexPage"] == "true")
            {
                IsFromIndexPage = true;
                base.MenuItem = "Summary Order View";
            }
            else
            {
                IsFromIndexPage = false;
                base.MenuItem = "Executive Planning Reports";
            }
            
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>"; ;
            ((Label)Master.FindControl("lblPageHeading")).Text = "Management Reporting Filter Criteria";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/index.aspx";
            //txtFromDate.Text = "01/01/" + DateTime.Now.Year;
            FillPeriod();
            ddlPeriod.Items.Insert(5, new ListItem("Current Year", System.Data.Linq.SqlClient.SqlMethods.DateDiffDay(Convert.ToDateTime("01/01/" + DateTime.Now.Year), System.DateTime.Now).ToString()));//Add new item for current year            
            
            
            bindReportDashboard();
            FillCompanyStore();

            if (IsFromIndexPage)
            {
                ddlParentReport.Items.FindByText("Order Management").Selected = true;
                ddlParentReport_SelectedIndexChanged(null, null);
                ddlChildReport.Items.FindByText("Summary Order View").Selected = true;
                ddlChildReport_SelectedIndexChanged(null, null);
                trParentReport.Visible = false;
                trSubReport.Visible = false;
            }
        }
    }

    protected void ddlParentReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlChildReport.Items.Clear();
        List<INC_Lookup> objlist = new List<INC_Lookup>();
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IsFromIndexPage == true)
            objlist = objLookupRepository.GetByLookup(ddlParentReport.SelectedItem.Text);
        else
            objlist = objReportAccessRightsRepository.GetChildReportByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(ddlParentReport.SelectedItem.Value));

        if (objlist != null)
        {
            ddlChildReport.DataSource = objlist.OrderBy(x => x.sLookupName);
            ddlChildReport.DataValueField = "iLookupID";
            ddlChildReport.DataTextField = "sLookupName";
            ddlChildReport.DataBind();
        }
        ddlChildReport.Items.Insert(0, new ListItem("-Select-", "0"));
        FillCompanyStore();
        ddlMasterItem.Items.Clear();
        trMasterItem.Visible = false;
        trOrderNumber.Visible = false;
        trFirstName.Visible = false;
        trLastName.Visible = false;
        trPeriod.Visible = true;
        ddlPeriod.SelectedValue = "0";
        ddlPeriod_SelectedIndexChanged(null, null);
    }

    protected void ddlChildReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlChildReport.SelectedItem.Text == "Item History Snapshot")
        {
            Int64? storeID = null;
            if (ddlCompanyStore.SelectedIndex != 0)
                storeID = Convert.ToInt64(ddlCompanyStore.SelectedValue);

            ddlMasterItem.Items.Clear();
            List<INC_Lookup> objlist = objStoreProductRepository.GetActiveMasterItems(storeID).OrderBy(x => x.sLookupName).ToList();
            if (objlist != null)
            {
                ddlMasterItem.DataSource = objlist;
                ddlMasterItem.DataValueField = "iLookupID";
                ddlMasterItem.DataTextField = "sLookupName";
                ddlMasterItem.DataBind();
            }
            ddlMasterItem.Items.Insert(0, new ListItem("-Select Master Item-", "0"));
            trMasterItem.Visible = true;
            trOrderNumber.Visible = false;
            trFirstName.Visible = false;
            trLastName.Visible = false;
        }
        else
        {
            if (ddlChildReport.SelectedItem.Text == "Summary View of Credits")
                ddlPeriod.SelectedValue = "";
            else
                ddlPeriod.SelectedValue = "0";
            ddlPeriod_SelectedIndexChanged(null, null);

            if (ddlChildReport.SelectedItem.Text == "Credits by Station" || ddlChildReport.SelectedItem.Text == "Credits by Workgroup" || ddlChildReport.SelectedItem.Text == "Summary Product Review" || ddlChildReport.SelectedItem.Text == "Items with Backorders" || ddlChildReport.SelectedItem.Text == "Employment Status" || ddlChildReport.SelectedItem.Text == "Customer Owned Inventory" || ddlChildReport.SelectedItem.Text == "Current Vs Previous Year Sales")
                trPeriod.Visible = false;
            else
                trPeriod.Visible = true;

            ddlMasterItem.Items.Clear();
            trMasterItem.Visible = false;

            if (ddlChildReport.SelectedItem.Text == "Summary Order View")
            {
                trOrderNumber.Visible = true;
                trFirstName.Visible = true;
                trLastName.Visible = true;
                trStatusView.Visible = true;
                trReportView.Visible = true;
            }
            else
            {
                trOrderNumber.Visible = false;
                trFirstName.Visible = false;
                trLastName.Visible = false;
                trStatusView.Visible = false;
                trReportView.Visible = false;
            }
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
        if (txtOrderNumber.Text.Trim() != "" || txtFirstName.Text.Trim() != "" || txtLastName.Text.Trim() != "")
        {
            ddlPeriod.SelectedValue = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
        }
        if (ddlReportView.SelectedValue == "2")
        {
            Response.Redirect("SummaryOrderCompanyViewReport.aspx?StatusView=" + ddlStatusView.SelectedValue);
        }
        else
        {
            if (ddlParentReport.SelectedItem.Text.Contains("Spend Summary"))
                Response.Redirect("SalesSummaryReport.aspx?SubReport=" + ddlChildReport.SelectedItem.Text + "&StoreID=" + ddlCompanyStore.SelectedItem.Value + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&Period=" + PeriodValue);
            else if (ddlParentReport.SelectedItem.Text.Contains("Employee Information"))
                Response.Redirect("EmployeeDataReport.aspx?SubReport=" + ddlChildReport.SelectedItem.Text + "&StoreID=" + ddlCompanyStore.SelectedItem.Value + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&Period=" + PeriodValue);
            else if (ddlParentReport.SelectedItem.Text.Contains("Product Planning"))
                Response.Redirect("ProductPlanningReport.aspx?SubReport=" + ddlChildReport.SelectedItem.Text + "&StoreID=" + ddlCompanyStore.SelectedItem.Value + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&MasterItemID=" + ddlMasterItem.SelectedValue + "&Period=" + PeriodValue);
            else if (ddlParentReport.SelectedItem.Text.Contains("Anniversary Credits"))
                Response.Redirect("AnniversaryCreditsReport.aspx?SubReport=" + ddlChildReport.SelectedItem.Text + "&StoreID=" + ddlCompanyStore.SelectedItem.Value + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&Period=" + PeriodValue);
            else if (ddlParentReport.SelectedItem.Text.Contains("Order Management") && ddlChildReport.SelectedItem.Text.Contains("Summary Order View"))
                Response.Redirect("SummaryOrderViewReport.aspx?StoreID=" + ddlCompanyStore.SelectedItem.Value + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&Period=" + PeriodValue + "&OrderNumber=" + txtOrderNumber.Text.Trim() + "&FirstName=" + txtFirstName.Text.Trim() + "&LastName=" + txtLastName.Text.Trim() + "&StatusView=" + ddlStatusView.SelectedValue + (IsFromIndexPage == true ? "&IsFromIndexPage=true" : ""));
            else if (ddlParentReport.SelectedItem.Text.Contains("Order Management") && ddlChildReport.SelectedItem.Text.Contains("Uniform Issuance Report"))
                Response.Redirect("IssuancePolicyReports.aspx?SubReport=" + ddlChildReport.SelectedItem.Text + "&StoreID=" + ddlCompanyStore.SelectedItem.Value + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&Period=" + PeriodValue);
            else if (ddlParentReport.SelectedItem.Text.Contains("Order Management"))
                Response.Redirect("OrderManagementReport.aspx?SubReport=" + ddlChildReport.SelectedItem.Text + "&StoreID=" + ddlCompanyStore.SelectedItem.Value + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&Period=" + PeriodValue);
            else if (ddlParentReport.SelectedItem.Text.Contains("Service Level Scorecard"))
                Response.Redirect("ServiceLevelScorecardReport.aspx?SubReport=" + ddlChildReport.SelectedItem.Text + "&StoreID=" + ddlCompanyStore.SelectedItem.Value + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&Period=" + PeriodValue);
        }
    }

    #endregion

    #region Methods
    private void FillCompanyStore()
    {
        try
        {
            ddlCompanyStore.Items.Clear();
            List<SelectCompanyStoreNameResult> objlist = new List<SelectCompanyStoreNameResult>();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IsFromIndexPage == true)
            {
                OrderConfirmationRepository objLookRep = new OrderConfirmationRepository();
                objlist = objLookRep.GetCompanyStoreName();
            }
            else
            {
                if (ddlParentReport.SelectedIndex > 0)
                    objlist = objReportAccessRightsRepository.GetStoreByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(ddlParentReport.SelectedValue));
            }
            if (objlist != null)
                objlist = objlist.OrderBy(x => x.CompanyName).ToList();

            ddlCompanyStore.DataSource = objlist;
            ddlCompanyStore.DataValueField = "StoreID";
            ddlCompanyStore.DataTextField = "CompanyName";
            ddlCompanyStore.DataBind();
            ddlCompanyStore.Items.Insert(0, new ListItem("-Select Store-", "0"));
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
    }
    public void bindReportDashboard()
    {
        List<INC_Lookup> objlist = new List<INC_Lookup>();
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IsFromIndexPage == true)
            objlist = objLookupRepository.GetByLookup("ReportDashboard");
        else
            objlist = objReportAccessRightsRepository.GetParentReportByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);

        if (objlist != null)
        {
            ddlParentReport.DataSource = objlist.OrderBy(x => x.sLookupName);
            ddlParentReport.DataValueField = "iLookupID";
            ddlParentReport.DataTextField = "sLookupName";
            ddlParentReport.DataBind();
            ddlParentReport.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlChildReport.Items.Insert(0, new ListItem("-Select-", "0"));

            //As per Ken instruction set default report to this
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            {
                ddlParentReport.Items.FindByText("Spend Summary").Selected = true;
                ddlParentReport_SelectedIndexChanged(null, null);
                ddlChildReport.Items.FindByText("Spend By Station Location").Selected = true;
                ddlChildReport_SelectedIndexChanged(null, null);
            }
        }
    }
    #endregion
}
