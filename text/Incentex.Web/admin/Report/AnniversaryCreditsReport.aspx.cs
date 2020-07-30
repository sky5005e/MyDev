using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Web.UI.DataVisualization.Charting;

public partial class admin_Report_AnniversaryCreditsReport : PageBase
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
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Anniversary Credit Dashboard";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Report/ReportDashBoard.aspx";
            FillCompanyStore();
            FillBasedStation();
            FillWorkgroup();
            FillPeriod();
            FillGender();
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
    protected void chrtAnniversaryCreditByWorkgroup_Click(object sender, ImageMapEventArgs e)
    {
        Int64? workgroupID = new LookupRepository().GetIdByLookupNameNLookUpCode(e.PostBackValue, "Workgroup ");
        //Response.Redirect("~/admin/SearchUserResult.aspx?ChartSubReport=" + SubReport + "&UserType=0&CompanyName=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedItem.Text : "") + "&FirstName=&LastName=&Email=&EmployeeID=&EmployeeType=&BaseStation=" + basestation + "&WorkGroup=" + workgroupID + "&Gender=" + ddlGender.SelectedValue);
        Response.Redirect("~/admin/Report/AnniversaryCreditEmployeeWiseReport.aspx?ChartSubReport=" + SubReport + "&StoreID=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedValue : "") + "&BaseStation=" + (ddlBasestation.SelectedIndex>0?ddlBasestation.SelectedValue : "") + "&WorkGroup=" + workgroupID + "&Gender=" + (ddlGender.SelectedIndex > 0 ? ddlGender.SelectedValue : ""));
    }
    protected void chrtAnniversaryCreditsIssuedByMonth_Click(object sender, ImageMapEventArgs e)
    {
        Response.Redirect("~/admin/Report/AnniversaryCreditEmployeeWiseReport.aspx?ChartSubReport=" + SubReport + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&StoreID=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedValue : "") + "&BaseStation=" + (ddlBasestation.SelectedIndex > 0 ? ddlBasestation.SelectedValue : "") + "&WorkGroup=" + (ddlWorkgroup.SelectedIndex > 0 ? ddlWorkgroup.SelectedValue : "") + "&Gender=" + (ddlGender.SelectedIndex > 0 ? ddlGender.SelectedValue : "") + "&Month=" + e.PostBackValue);
    }
    protected void chrtAnniversaryCreditByBasestation_Click(object sender, ImageMapEventArgs e)
    {
        string baseStation = "";
        baseStation = new BaseStationRepository().GetByName(e.PostBackValue).iBaseStationId.ToString();
        //Response.Redirect("~/admin/SearchUserResult.aspx?ChartSubReport=" + SubReport + "&UserType=0&CompanyName=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedItem.Text : "") + "&FirstName=&LastName=&Email=&EmployeeID=&EmployeeType=&BaseStation=" + e.PostBackValue + "&WorkGroup=" + ddlWorkgroup.SelectedValue + "&Gender=" + ddlGender.SelectedValue);
        Response.Redirect("~/admin/Report/AnniversaryCreditEmployeeWiseReport.aspx?ChartSubReport=" + SubReport + "&StoreID=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedValue : "") + "&BaseStation=" + baseStation + "&WorkGroup=" + (ddlWorkgroup.SelectedIndex > 0 ? ddlWorkgroup.SelectedValue : "") + "&Gender=" + (ddlGender.SelectedIndex > 0 ? ddlGender.SelectedValue : ""));
    }
    protected void chrtAnniversaryCreditsAtAGlance_Click(object sender, ImageMapEventArgs e)
    {
        Response.Redirect("~/admin/Report/AnniversaryCreditEmployeeWiseReport.aspx?ChartSubReport=" + SubReport + "&FromDate=" + txtFromDate.Text.Trim() + "&ToDate=" + txtToDate.Text.Trim() + "&StoreID=" + (ddlCompanyStore.SelectedIndex > 0 ? ddlCompanyStore.SelectedValue : "") + "&BaseStation=" + (ddlBasestation.SelectedIndex > 0 ? ddlBasestation.SelectedValue : "") + "&WorkGroup=" + (ddlWorkgroup.SelectedIndex > 0 ? ddlWorkgroup.SelectedValue : "") + "&Gender=" + (ddlGender.SelectedIndex > 0 ? ddlGender.SelectedValue : "") + "&Glance=" + e.PostBackValue);
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
                objCompanyList = objReportAccessRightsRepository.GetStoreByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 454);

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
            basestationList = objReportAccessRightsRepository.GetBaseStationByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 454);

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
            WorkgroupList = objReportAccessRightsRepository.GetWorkgroupByUserInfoIDAndParentReportID(IncentexGlobal.CurrentMember.UserInfoID, 454);

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
            if (SubReport == "Credits by Workgroup")
            {
                trFromDate.Visible = false;
                trToDate.Visible = false;
                trPeriod.Visible = false;
                List<ReportRepository.GetAnniversaryCredit> objResult = objReportRepository.GetAnniversaryCreditWorkgroupWise(UserInfoID, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtAnniversaryCreditByWorkgroup.DataSource = objResult.OrderByDescending(x => x.Text).ToList();
                    chrtAnniversaryCreditByWorkgroup.DataBind();

                    lblTotalAnniversaryCredit.Text = string.Format("{0:c}", objResult.Sum(x => x.Value));
                }
                dvAnniversaryCreditByWorkgroup.Visible = true;
            }
            else if (SubReport == "Credits by Station")
            {
                trFromDate.Visible = false;
                trToDate.Visible = false;
                trPeriod.Visible = false;

                List<ReportRepository.GetAnniversaryCredit> objResult = objReportRepository.GetAnniversaryCreditStationWise(UserInfoID, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtAnniversaryCreditByBasestation.DataSource = objResult.OrderByDescending(x => x.Text).ToList();
                    chrtAnniversaryCreditByBasestation.DataBind();

                    lblTotalAnniversaryCredit.Text = string.Format("{0:c}", objResult.Sum(x => x.Value));
                }
                dvAnniversaryCreditByBasestation.Visible = true;
            }
            else if (SubReport == "Top 50 Credit Balances")
            {
                List<ReportRepository.GetAnniversaryCredit> objResult = objReportRepository.GetTopFiftyEmployeeByAnniversaryCreditWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtTopFiftyEmployeeByAnniversaryCredit.DataSource = objResult;
                    chrtTopFiftyEmployeeByAnniversaryCredit.DataBind();

                    lblTotalAnniversaryCredit.Text = string.Format("{0:c}", objResult.Sum(x => x.Value));
                }
                dvTopFiftyEmployeeByAnniversaryCredit.Visible = true;
            }
            else if (SubReport == "Summary View of Credits")
            {
                List<ReportRepository.GetAnniversaryCredit> objResult = objReportRepository.GetAnniversaryCreditsAtAGlanceWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID);
                if (objResult != null)
                {
                    chrtAnniversaryCreditsAtAGlance.DataSource = objResult.OrderByDescending(x => x.Text).ToList();
                    chrtAnniversaryCreditsAtAGlance.DataBind();

                    lblTotalAnniversaryCredit.Text = string.Format("{0:c}", objResult.Sum(x => x.Value));
                }
                dvAnniversaryCreditsAtAGlance.Visible = true;
            }
            else if (SubReport == "Credits Issued by Month")
            {
                List<ReportRepository.GetAnniversaryCredit> objResult = objReportRepository.GetAnniversaryCreditIssuedMonthWise(UserInfoID, FromDate, ToDate, StoreID, WorkgroupID, BaseStationID, GenderID, "Issued Anniversary Credit");
                if (objResult != null)
                {
                    chrtAnniversaryCreditsIssuedByMonth.DataSource = objResult.OrderByDescending(x => x.Text).ToList();
                    chrtAnniversaryCreditsIssuedByMonth.DataBind();

                    lblTotalAnniversaryCredit.Text = string.Format("{0:c}", objResult.Sum(x => x.Value));
                }
                dvAnniversaryCreditsIssuedByMonth.Visible = true;
            }
        }
    }

    #endregion
}
