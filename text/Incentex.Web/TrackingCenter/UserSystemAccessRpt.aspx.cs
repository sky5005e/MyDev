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

public partial class TrackingCenter_UserSystemAccessRpt : PageBase
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
            dvtotalnumber.Visible = false;
            DateTime now = DateTime.Now;
            string year = now.Year.ToString();
            string month = now.Month.ToString();
            string day = now.Day.ToString();
            txtStartDate.Text = month + "/" + day + "/" + year;
            txtEndDate.Text = month + "/" + day + "/" + year;
            if (Request.QueryString["sdate"] != null) { BindGird(); }


        }

    }
    /// <summary>
    ///  after entering both start and end date through the stored procedure get the result of the user who accessed the system 
    /// </summary>
    
    protected void lnkBtnReportUserAccess_Click(object sender, EventArgs e)
    {
        try
        { 


            StartDateForSysAcc = txtStartDate.Text;
            EndDateForSysAcc = txtEndDate.Text;


            if ((Convert.ToDateTime(StartDateForSysAcc) < Convert.ToDateTime(EndDateForSysAcc)) || (Convert.ToDateTime(StartDateForSysAcc) == Convert.ToDateTime(EndDateForSysAcc)))
            {

                LblError.Visible = false;
                UserTrackingRepo ObjTracRepo = new UserTrackingRepo();
                List<UserAccessTrackingResult> List = ObjTracRepo.AccessUserList(Convert.ToDateTime(StartDateForSysAcc), Convert.ToDateTime(EndDateForSysAcc), this.SortExp, this.SortOrder);
                DataTable dt = ListToDataTable(List);
                DataView dv = dt.DefaultView;
                //gv.DataSource = List;
                pds.DataSource = dv;
                pds.AllowPaging = true;
                pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
                pds.CurrentPageIndex = CurrentPage;
                lnkbtnNext.Enabled = !pds.IsLastPage;
                gv.DataSource = pds;
                gv.DataBind();
                if (List.Count == 0)
                {

                    lstPaging.Visible = false;
                    lnkbtnNext.Visible = false;
                    lnkbtnPrevious.Visible = false;
                    pager.Visible = false;
                }
                else
                {

                    lstPaging.Visible = true;
                    lnkbtnNext.Visible = true;
                    lnkbtnPrevious.Visible = true;
                    pager.Visible = true;
                }
                doPaging();
                dvtotalnumber.Visible = true;
                LblTotalUser.Text = Convert.ToString(List.Count);
            }
            else
            {
                LblError.Visible = true;
                LblError.Text = " Start Date Must be less then or eqaul to End date ";
            }
        }
        catch (Exception ex)
        {

            ex = null;
            LblError.Visible = true;
            LblError.Text = " Please enter both the dates  ";

        }
    }
    
    /// <summary>
    ///  Row Command for the sorting logic and as well as redirect to next page for the detail
    /// </summary>
    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Sorting":

                if (this.SortExp.ToString() == e.CommandArgument.ToString())
                {
                    if (this.SortOrder == DAEnums.SortOrderType.Asc)
                        this.SortOrder = DAEnums.SortOrderType.Desc;
                    else
                        this.SortOrder = DAEnums.SortOrderType.Asc;
                }
                else
                {
                    this.SortOrder = DAEnums.SortOrderType.Asc;
                    this.SortExp = (UserTrackingRepo.UserAccessTrackingSortExpType)Enum.Parse(typeof(UserTrackingRepo.UserAccessTrackingSortExpType), e.CommandArgument.ToString());
                }
                BindGird();
                break;

        }
        if (e.CommandName == "Inform")
        {
            userinfoid = (Convert.ToInt32(e.CommandArgument));
            Response.Redirect("ViewAccessInfo.aspx?uid=" + userinfoid + "&sdate=" + this.txtStartDate.Text + "&edate=" + this.txtEndDate.Text);
            //gvmenu.DataSource = ObjUserTrackRepo.GetUserByUserinfoid(userinfoid,Convert.ToDateTime(txtStartDate.Text),Convert.ToDateTime(txtEndDate.Text));
            //gvmenu.DataBind();
            //modal.Show();

        }
    }
    
    /// <summary>
    /// if the user status is 1 then it will show as online.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
           
            if (((HiddenField)e.Row.FindControl("hdnUserStatus")).Value == "1" && string.IsNullOrEmpty(((DataRowView)(e.Row.DataItem)).Row.ItemArray[6].ToString()))
            {
                if (Convert.ToInt32(((DataRowView)(e.Row.DataItem)).Row.ItemArray[7]) <= 20)
                {
                    ((Label)e.Row.FindControl("lblFirstName")).Style.Add("color", "black");
                    ((Label)e.Row.FindControl("lblFirstName")).BackColor = System.Drawing.Color.Green;
                }
            }
            userinfoid = Convert.ToInt32(((Label)e.Row.FindControl("lblUID")).Text);
            var objlist = ObjUserTrackRepo.GetPucrchaeCount(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(userinfoid));
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            if (objlist.Count > 0)
            {
                lblStatus.Text = objlist[0].PurchaseCount.ToString();
            }
            else
            {
                lblStatus.Text = "0";
            }
          
        }
    }

    #region Paging
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        gv.PageIndex = CurrentPage;
        BindGird();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        gv.PageIndex = CurrentPage;
        BindGird();
    }
    
    protected void lstPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    private void doPaging()
    {
        try
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            int CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + PagerSize;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - PagerSize;

            }

            if (pds.PageCount < ToPg)
                ToPg = pds.PageCount;

            for (int i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            lstPaging.DataSource = dt;
            lstPaging.DataBind();

        }
        catch (Exception)
        { }

    }
    
    protected void lstPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGird();
            //bindGridView();
        }
    }
    #endregion

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
    
    /// <summary>
    /// in this grid it will bind all the record of the CA/CE/IE . if they purchase the come with green (right sign)
    /// if not then come red( cross sign)
    /// </summary>
    public void BindGird()
    {
        UserTrackingRepo ObjRepo = new UserTrackingRepo();
        // check with query string start 

        if ((Request.QueryString["sdate"] != null))
        {
            StartDateForSysAcc = (Request.QueryString["sdate"].ToString());
        }
        else
        {
            StartDateForSysAcc = txtStartDate.Text;
        }

        if ((Request.QueryString["edate"] != null))
        {
            EndDateForSysAcc = (Request.QueryString["edate"].ToString());
        }
        else
        {
            EndDateForSysAcc = txtEndDate.Text;
        }

        //this.uid = Convert.ToInt32(Request.QueryString["uid"]);
        //this.sd = Convert.ToDateTime(Request.QueryString["sdate"]);
        //this.ed = Convert.ToDateTime(Request.QueryString["edate"]);

        // check with query string End
        List<UserAccessTrackingResult> objList = ObjRepo.AccessUserList(Convert.ToDateTime(StartDateForSysAcc), Convert.ToDateTime(EndDateForSysAcc), this.SortExp, this.SortOrder);
        DataTable dt = ListToDataTable(objList);
        DataView dv = dt.DefaultView;
        pds.DataSource = dv;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gv.DataSource = pds;
        gv.DataBind();
        //doPaging();

    }
    

}
