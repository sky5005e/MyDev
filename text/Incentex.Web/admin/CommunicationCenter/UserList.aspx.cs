using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CommunicationCenter_UserList : PageBase
{
    #region Data Member's
    TodayEmailsRepository ObjTodaysRepo = new TodayEmailsRepository();
    PagedDataSource pds = new PagedDataSource();
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
                return 5;
            else
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }

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
    String XValue
    {
        get
        {
            if (ViewState["XValue"] == null)
            {
                ViewState["XValue"] = 0;
            }
            return ViewState["XValue"].ToString();
        }
        set
        {
            ViewState["XValue"] = value;
        }
    }
    Int32 Time = 0;
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
    #region Event's 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "User List";
            
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

            if (!String.IsNullOrEmpty(Request.QueryString["AxisSeries"]))
                this.XValue = Request.QueryString["AxisSeries"].ToString();
           
            if (!String.IsNullOrEmpty(Request.QueryString["IsOrderPlaced"]) && Request.QueryString["IsOrderPlaced"] == "1")
                this.IsOrderPlaced = true;

            // For Back button
            if (IsOrderPlaced)
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/OrderPlacedReports.aspx?cid=" + this.CompanyID + "&uid=" + this.UserInfoID + "&dt=" + this.Date.ToShortDateString() + "&nday=" + NumberOfDays + "&rptop=" + this.ReportOption;
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CommunicationReports.aspx?cid=" + this.CompanyID + "&uid=" + this.UserInfoID + "&dt=" + this.Date.ToShortDateString() + "&nday=" + NumberOfDays + "&rptop=" + this.ReportOption;

            // Get Time of days
            if (XValue.EndsWith("AM"))
                Time = Convert.ToInt32(XValue.Replace("AM", ""));
            else if (XValue.EndsWith("PM"))
                Time = 12 + Convert.ToInt32(XValue.Replace("PM", ""));
            

                Bind_GridUser();
        }
    }
    protected void gvUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Now Want to open Popup window from hyper link
            HiddenField hdnTempID = (HiddenField)e.Row.FindControl("hdnTempID");
            Label lblUserInfoID = (Label)e.Row.FindControl("lblUserInfoID");
            Int32 mailID = Convert.ToInt32(hdnTempID.Value);
            String urlWithParams = "ViewTemplates.aspx?mailID=" + mailID+"&rptop="+this.ReportOption +"&uid="+lblUserInfoID.Text;
            HyperLink hypViewTemp = (HyperLink)e.Row.FindControl("hypViewTemp");
            hypViewTemp.Attributes.Add("OnClick", "window.open('" + urlWithParams + "','PopupWindow','width=650,height=650, scrollbars=yes')");

        }
    }
    #endregion 

    #region Method's
    private void Bind_GridUser()
    {
        DataView myDataView = new DataView();
        DateTime toDate = Date.AddDays(NumberOfDays);

        String daysofWeek = String.Empty;
        if (XValue.EndsWith("day"))
            daysofWeek = XValue.ToString();

        List<TodayEmailsRepository.GetUserforMails> objList = ObjTodaysRepo.GetAllUserInfobyBarGraph(this.ReportOption, this.CompanyID, this.UserInfoID, this.Date, toDate, Convert.ToInt32(Time), daysofWeek);
        lblCount.Text = String.Format("Total Number of Users : {0}", objList.Count().ToString());
        if (objList.Count == 0)
        {
            pagingtable.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
        }
        DataTable dataTable = Common.ListToDataTable(objList);
        myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = 250;
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        gvUser.DataSource = pds;
        gvUser.DataBind();
        doPaging();
      


    }

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            Bind_GridUser();
        }


    }
    protected void dtlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
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
                ToPg = ToPg + 5;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - 5;

            }

            if (pds.PageCount < ToPg)
            {
                ToPg = pds.PageCount;
            }

            for (int i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }



            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();

        }
        catch (Exception)
        {
        }
    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        Bind_GridUser();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        Bind_GridUser();
    }

    #endregion
    #endregion 
}
