using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;


public partial class admin_CommunicationCenter_ReportDashBoard : PageBase
{
    #region Data Members
    CompanyRepository objCompanyRepo = new CompanyRepository();
    UserInformationRepository objUserRepo = new UserInformationRepository();
    /// <summary>
    /// Set true when request is for Order Placed Report
    /// </summary>
    Boolean IsOrderPlaced
    {
        get
        {
            if (ViewState["IsOrderPlaced"] == null)
            {
                ViewState["IsOrderPlaced"] = 0;
            }
            return Convert.ToBoolean(ViewState["IsOrderPlaced"]);
        }
        set
        {
            ViewState["IsOrderPlaced"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "View Reports";
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
            ((Label)Master.FindControl("lblPageHeading")).Text = "Reporting Filter Criteria";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/CommunicationCenter/CampaignSelection.aspx";
            FillCompanyStore();
            ddlEmployee.Items.Insert(0, new ListItem("-Select-", "0"));
            if (!String.IsNullOrEmpty(Request.QueryString["IsOrderPlaced"]) && Request.QueryString["IsOrderPlaced"] == "1")
                this.IsOrderPlaced = true;

            BindReportOtion(IsOrderPlaced);
        }

    }
    protected void ddlDateRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDateRange.SelectedValue == "Range")//This is for date range
        {
            trFromDate.Visible = true;
            trToDate.Visible = true;
        }
        else
        {
            txtFromDate.Text = null;
            txtToDate.Text = null;
            trFromDate.Visible = false;
            trToDate.Visible = false;
        }
    }
    protected void lnkRunReport_Click(object sender, EventArgs e)
    {
        Int32 nday = 0;
        DateTime dt = DateTime.Now;
        if (ddlDateRange.SelectedValue == "Range" && txtToDate.Text != "" && txtFromDate.Text != "")
        {
            nday = Convert.ToInt32((Convert.ToDateTime(txtToDate.Text) - Convert.ToDateTime(txtFromDate.Text)).TotalDays);
            dt = Convert.ToDateTime(txtFromDate.Text);
        }
        else
        {
            nday = Convert.ToInt32(ddlDateRange.SelectedValue);
            dt = DateTime.Now.AddDays(-nday);
            if (ddlDateRange.SelectedValue == "1")
                nday = 0;// To get yesteday records
        }
        if (IsOrderPlaced)
            Response.Redirect("OrderPlacedReports.aspx?cid=" + ddlCompanyStore.SelectedValue + "&uid=" + ddlEmployee.SelectedValue + "&dt=" + dt + "&nday=" + nday + "&rptop=" + ddlReportOptions.SelectedValue);
        else
            Response.Redirect("CommunicationReports.aspx?cid=" + ddlCompanyStore.SelectedValue + "&uid=" + ddlEmployee.SelectedValue + "&dt=" + dt + "&nday=" + nday + "&rptop=" + ddlReportOptions.SelectedValue);
    }

    protected void ddlCompanyStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompanyStore.SelectedValue != "0")
        {
            FillEmployee(Convert.ToInt64(ddlCompanyStore.SelectedValue));
        }
    }
   
    #endregion

    #region Methods
    private void BindReportOtion(Boolean IsOrderPlacedReport)
    {


        if (IsOrderPlacedReport)
        {
            ddlReportOptions.Items.Insert(0, new ListItem("Orders placed by Hour", "0"));
        }
        else
        {
            ddlReportOptions.Items.Insert(0, new ListItem("Emails Sent by Hour", "0"));
            ddlReportOptions.Items.Insert(1, new ListItem("Emails Sent by Day", "1"));
            ddlReportOptions.Items.Insert(2, new ListItem("Emails Opened by Hour", "2"));
            ddlReportOptions.Items.Insert(3, new ListItem("Emails Opened by Day", "3"));
        }
    }

    private void FillCompanyStore()
    {
        //get company Name
        List<Company> objcomList = new List<Company>();
        objcomList = objCompanyRepo.GetAllCompany().OrderBy(c => c.CompanyName).ToList();
        Common.BindDDL(ddlCompanyStore, objcomList, "CompanyName", "CompanyID", "-Select-");
    }
    private void FillEmployee(Int64 compID)
    {
        // Get Employee Name
        List<UserInformationRepository.AllUser> objUserList = objUserRepo.GetAllUser(compID).OrderBy(u=>u.UserName).ToList();
        Common.BindDDL(ddlEmployee, objUserList, "UserName", "UserInfoID", "-Select-");
    }
    #endregion
}
