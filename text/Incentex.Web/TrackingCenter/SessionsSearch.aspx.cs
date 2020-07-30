using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class TrackingCenter_SessionsSearch : PageBase
{
    #region  Properties

    String RequestType
    {
        get
        {
            if (ViewState["RequestType"] != null)
                return ViewState["RequestType"].ToString();
            else
                return null;
        }
        set
        {
            ViewState["RequestType"] = value;
        }
    }
    CompanyRepository objRepo = new CompanyRepository();

    #endregion

    #region Page Event's

    protected void Page_Load(object sender, EventArgs e)
    {   
        if (!IsPostBack)
        {
            base.MenuItem = "Tracking Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Session Search";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/TrackingCenter/ViewVideoSession.aspx";

            if (!String.IsNullOrEmpty(Request.QueryString["req"]))
                this.RequestType = Request.QueryString["req"];
            BindValues();
        }

    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CampignRepo ObjRepo = new CampignRepo();
            List<CampignRepo.SearchCampUser> objList = null;
            if (ddlCompany.SelectedValue == "8")// Here 8 for Company  Incentex of Vero Beach, LLC
                objList = ObjRepo.GetIncentexEmp(0, 0).OrderBy(u => u.FullName).ToList();
            else
                objList = ObjRepo.GetCompanyEmp(Convert.ToInt64(ddlCompany.SelectedValue), 0, 0).OrderBy(u => u.FullName).ToList();

            ddlEmployee.DataSource = objList;
            ddlEmployee.DataValueField = "UserInfoID";
            ddlEmployee.DataTextField = "FullName";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception)
        {

            throw;
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

    protected void lnkViewSessions_Click(object sender, EventArgs e)
    {

        Int32 nday = 0;
        DateTime ToDate = DateTime.Now;
        DateTime FromDate = DateTime.Now;
        if (ddlDateRange.SelectedValue == "Range" && txtFromDate.Text != "" && txtToDate.Text != "")
        {
            ToDate = Convert.ToDateTime(txtToDate.Text);
            FromDate = Convert.ToDateTime(txtFromDate.Text);
        }
        else
        {
            nday = Convert.ToInt32(ddlDateRange.SelectedValue);
            FromDate = DateTime.Now.AddDays(-nday);
        }

        Response.Redirect("AllSessionRecords.aspx?cid=" + ddlCompany.SelectedValue + "&uid=" + ddlEmployee.SelectedValue + "&fromDate=" + FromDate + "&toDate=" + ToDate + "&req=" + RequestType);
    }

    #endregion

    #region Page Method's
    //bind dropdown for the company 
    public void BindValues()
    {
        //get company
        List<Company> objcomList = new List<Company>();
        objcomList = objRepo.GetAllCompany().OrderBy(c => c.CompanyName).ToList();
        Common.BindDDL(ddlCompany, objcomList, "CompanyName", "CompanyID", "-Select Company-");
        ddlEmployee.Items.Insert(0, new ListItem("-Select-", "0"));
    }
    #endregion
}