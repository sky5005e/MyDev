using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.Collections.Generic;

public partial class TrackingCenter_SystemAccessByHour : PageBase
{
    #region Properties
    public int CurrentPage
    {
        get
        {
            if (this.ViewState["CurrentPage"] == null)
                return 0;
            else
                return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }
    public int PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt16(this.ViewState["PagerSize"].ToString());
        }
        set
        {
            this.ViewState["PagerSize"] = value;
        }
    }
    public int FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt16(this.ViewState["FrmPg"].ToString());
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }
    public int ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return PagerSize;
            else
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }
    UserTrackingRepo.UserAccessTrackingSortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = UserTrackingRepo.UserAccessTrackingSortExpType.CompanyName;
            }
            return (UserTrackingRepo.UserAccessTrackingSortExpType)ViewState["SortExp"];
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }
    DAEnums.SortOrderType SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = DAEnums.SortOrderType.Asc;
            }
            return (DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }
    PagedDataSource pds = new PagedDataSource();
    UserTrackingRepo ObjUserTrackRepo = new UserTrackingRepo();
    CampignRepo ObjRepo = new CampignRepo();
    string StartDateForSysAcc = string.Empty;
    string EndDateForSysAcc = string.Empty;
    int userinfoid = 0;
    #endregion

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

            ((Label)Master.FindControl("lblPageHeading")).Text = "User Access System Report";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/TrackingCenter/ReportSelection.aspx";           
            DateTime now = DateTime.Now;
            string year = now.Year.ToString();
            string month = now.Month.ToString();
            string day = now.Day.ToString();
            txtStartDate.Text = month + "/" + day + "/" + year;
            txtEndDate.Text = month + "/" + day + "/" + year;
           // if (Request.QueryString["sdate"] != null) { BindGird(); }
            BindValues();
        }

    }
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            List<CampignRepo.SearchCampUser> objList = null;
            if (ddlCompany.SelectedValue == "8")// Here 8 for Company  Incentex of Vero Beach, LLC
                objList = ObjRepo.GetIncentexEmp(0, 0).OrderBy(u => u.FullName).ToList();
            else
                objList = ObjRepo.GetCompanyEmp(Convert.ToInt64(ddlCompany.SelectedValue), 0, 0).OrderBy(u => u.FullName).ToList();

            ddlEmployee.DataSource = objList;
            ddlEmployee.DataValueField = "UserInfoID";
            ddlEmployee.DataTextField = "FullName";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("-Select All Employee-", "0"));

        }
        catch (Exception)
        {

            throw;
        }
    }
   
    protected void lnkBtnReportUserAccess_Click(object sender, EventArgs e)
    {
        try
        {
            StartDateForSysAcc = txtStartDate.Text;
            EndDateForSysAcc = txtEndDate.Text;
            if ((Convert.ToDateTime(StartDateForSysAcc) < Convert.ToDateTime(EndDateForSysAcc)) || (Convert.ToDateTime(StartDateForSysAcc) == Convert.ToDateTime(EndDateForSysAcc)))
            {
                LblError.Visible = false;
                Int64 CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
                Int64 UserInfoID=0;
                if(!string.IsNullOrEmpty(ddlEmployee.SelectedValue))
                    UserInfoID = Convert.ToInt64(ddlEmployee.SelectedValue); 
                List<GetSystemAccessDetailResult> objResult = ObjUserTrackRepo.GetSystemAccess(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), UserInfoID);
                chrtStatusReport.DataSource = objResult;                            
                //chrtStatusReport.DataSource = objResult.Where(x=>x.AC!=0).ToList();
                chrtStatusReport.DataBind();
                dvReport.Visible = true;
                
            }
            else
            {
                LblError.Visible = true;
                LblError.Text = " Start Date Must be less then or eqaul to End date ";
                dvReport.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ex = null;
            LblError.Visible = true;
            LblError.Text = " Please enter both the dates  ";
        }
    }

    protected void chrtStatusReport_Click(object sender, ImageMapEventArgs e)
    {
        try
        {          
            Int64 AccessCount = Convert.ToInt64(e.PostBackValue);
            if (AccessCount != 0)
            {
                Response.Redirect("SystemAccessByHourDetail.aspx?Hour=" + AccessCount + "&StartDate=" + txtStartDate.Text + "&EndDate=" + txtEndDate.Text + "&UserInfoID=" + ddlEmployee.SelectedValue);
            }


        }
        catch (Exception)
        { }

    }

    public void BindValues()
    {
        //get company
        CompanyRepository objCompRepo = new CompanyRepository();
        List<Company> objcomList = new List<Company>();
        objcomList = objCompRepo.GetAllCompany().OrderBy(c => c.CompanyName).ToList();
        Common.BindDDL(ddlCompany, objcomList, "CompanyName", "CompanyID", "-Select Company-");
    }

    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

}
