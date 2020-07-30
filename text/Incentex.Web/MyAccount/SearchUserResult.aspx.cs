/// <summary>
/// Page created by mayur on 13-jan-2012
/// This page is created due to seperate user search and search result panel
/// </summary>

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Web.UI.HtmlControls;

public partial class MyAccount_SearchUserResult : PageBase
{
    #region Local Variable declaration
    PagedDataSource pds = new PagedDataSource();
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
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            
            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Users";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";

            if (Request.QueryString != null && Request.QueryString.Count==7)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/SearchUsers.aspx?id=" + Request.QueryString["id"].ToString();
                BindSearchGrid();
            }
        }
    }

    #region gridview events
    protected void grdView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Sort"))
        {
            if (this.ViewState["SortExp"] == null)
            {
                this.ViewState["SortExp"] = e.CommandArgument.ToString();
                this.ViewState["SortOrder"] = "ASC";
            }
            else
            {
                if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
                {
                    if (this.ViewState["SortOrder"].ToString() == "ASC")
                        this.ViewState["SortOrder"] = "DESC";
                    else
                        this.ViewState["SortOrder"] = "ASC";

                }
                else
                {
                    this.ViewState["SortOrder"] = "ASC";
                    this.ViewState["SortExp"] = e.CommandArgument.ToString();
                }
            }

            BindSearchGrid();
        }
    }
    protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            IncentexGlobal.SearchPageReturnURLFrontSide = Request.Url.ToString();
            ((HyperLink)e.Row.FindControl("lblFullName")).NavigateUrl = "~/MyAccount/Employee/BasicInformation.aspx?Id=" + Request.QueryString["id"].ToString() + "&SubId=" + ((Label)e.Row.FindControl("lblCompanyEmployeeId")).Text + "";

            string path = "~/admin/Incentex_Used_Icons/" + ((HiddenField)e.Row.FindControl("hdnLookupIcon")).Value;
            ((HtmlImage)e.Row.FindControl("imgLookupIconView")).Src = path;
        }
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            if (this.ViewState["SortOrder"].ToString() == "ASC")
                ImgSort.ImageUrl = "";
            else
                ImgSort.ImageUrl = "";


            switch (this.ViewState["SortExp"].ToString())
            {
                case "FullName":
                    PlaceHolder placeholderFullName = (PlaceHolder)e.Row.FindControl("placeholderFullName");
                    // placeholderFullName.Controls.Add(ImgSort);
                    break;
                case "Contact":
                    PlaceHolder placeholderContract = (PlaceHolder)e.Row.FindControl("placeholderContract");
                    // placeholderContract.Controls.Add(ImgSort);
                    break;
                case "Email":
                    PlaceHolder placeholderEmail = (PlaceHolder)e.Row.FindControl("placeholderEmail");
                    // placeholderEmail.Controls.Add(ImgSort);
                    break;
                case "Telephone":
                    PlaceHolder placeholderTelephone = (PlaceHolder)e.Row.FindControl("placeholderTelephone");
                    // placeholderTelephone.Controls.Add(ImgSort);
                    break;
                case "Mobile":
                    PlaceHolder placeholderMobile = (PlaceHolder)e.Row.FindControl("placeholderMobile");
                    // placeholderComapany.Controls.Add(ImgSort);
                    break;
            }

        }
    }
    #endregion

    #region Functions for Paging
    protected void dtlViewUsers_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    protected void dtlViewUsers_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindSearchGrid();
        }
    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindSearchGrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindSearchGrid();
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

            dtlViewUsers.DataSource = dt;
            dtlViewUsers.DataBind();

        }
        catch (Exception)
        { }
    }
    #endregion

    #region Miscellaneous function
    public void BindSearchGrid()
    {
        try
        {
            DataView myDataView = new DataView();
            List<UserInformationRepository.SearchResults> oj = new List<UserInformationRepository.SearchResults>();
            UserInformationRepository objUserInfoRepo = new UserInformationRepository();
            CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
            CompanyEmployee objCmpnyInfo = new CompanyEmployee();
            //Get Company Employee Information to get the workgroup for which he is admin
            objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
            List<AdditionalWorkgroup> additionaWorkgroupList = new AdditionalWorkgroupRepository().GetManageDetailName(Convert.ToInt32(objCmpnyInfo.CompanyEmployeeID), Convert.ToInt32(IncentexGlobal.CurrentMember.CompanyId));

            List<Int64> workgroupList = new List<Int64>();
            workgroupList.Add((Int64)objCmpnyInfo.ManagementControlForWorkgroup);
            for (int i = 0; i < additionaWorkgroupList.Count; i++)
            {
                workgroupList.Add((Int64)additionaWorkgroupList[i].WorkgroupID);
            }

            if (objCmpnyInfo.ManagementControlForWorkgroup!=null)
                oj = objUserInfoRepo.getCompanyUsers(Request.QueryString["FirstName"].ToString(), Request.QueryString["LastName"].ToString(), "", "", Request.QueryString["Email"].ToString(), Convert.ToInt64(Request.QueryString.Get("Id")), workgroupList, Request.QueryString["EmployeeID"].ToString(),Request.QueryString["WorkGroup"].ToString(),Request.QueryString["Gender"].ToString());

            //total records for this search
            lblRecords.Text = oj.Count.ToString();

            DataTable dataTable = ListToDataTable(oj);
            myDataView = dataTable.DefaultView;

            if (oj.Count == 0)
            {
                CurrentPage = 0;
                //dvPager.Visible = false;
                lblMsg.Text = "No Records found matching your criteria..";
            }
            else
            {
                //dvPager.Visible = true;
                lblMsg.Text = string.Empty;
            }

            if (this.ViewState["SortExp"] != null)
            {
                myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
            }
            pds.DataSource = myDataView;
            pds.AllowPaging = true;
            pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
            pds.CurrentPageIndex = CurrentPage;
            lnkbtnNext.Enabled = !pds.IsLastPage;
            lnkbtnPrevious.Enabled = !pds.IsFirstPage;
            grdView.DataSource = pds;
            grdView.DataBind();

            doPaging();

        }
        catch
        {
            Response.Redirect("SearchUsers.aspx?id=" + Request.QueryString["id"].ToString());
        }
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null);
            }
            dt.Rows.Add(row);
        }
        return dt;
    }
    #endregion
}
