/// <summary>
/// Page created by mayur on 10-jan-2012
/// This page is created due to seperate user search and search result panel
/// </summary>

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Data;
using System.Linq;

public partial class admin_SearchUserResult : PageBase
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
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Search Users";
            base.ParentMenuID = 10;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Search User Result";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/SearchUsers.aspx";

            if (Request.QueryString["ChartSubReport"] != null)//This is for back into chart page when comming from chart
            {
                String ReturnUrl = string.Empty;
                if (Request.QueryString["ChartSubReport"].Contains("Anniversary Credits"))
                    ReturnUrl = "~/admin/Report/AnniversaryCreditsReport.aspx";
                else
                    ReturnUrl = "~/admin/Report/EmployeeDataReport.aspx";

                //Now combine all querystring with returnurl
                for (int i = 0; i < Request.QueryString.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString[i]))
                    {
                        if (i == 0)
                            ReturnUrl += "?SubReport";
                        else
                            ReturnUrl += "&" + Request.QueryString.AllKeys[i];

                        ReturnUrl += "=" + Request.QueryString[i].ToString();
                    }
                }
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = ReturnUrl; 
            }
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/SearchUsers.aspx";
            
            //If querystring is not enough then redirect this to search users page again
            if (Request.QueryString != null && Request.QueryString.Count >= 10)
            {
                BindSearchGrid();
            }
            else
            {
                Response.Redirect("SearchUsers.aspx");
            }
        }
    }

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



            dtlViewUsers.DataSource = dt;
            dtlViewUsers.DataBind();

        }
        catch (Exception)
        { }
    }
    #endregion

    #region Gridview events
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
            if (((Label)e.Row.FindControl("lblCompanyEmployeeId")).Text != "0")
            {
                IncentexGlobal.SearchPageReturnURL = Request.Url.ToString();
                ((HyperLink)e.Row.FindControl("hplFullName")).NavigateUrl = "~/admin/Company/Employee/BasicInformation.aspx?Id=" + new UserInformationRepository().GetById(new CompanyEmployeeRepository().GetById(Convert.ToInt64(((Label)e.Row.FindControl("lblCompanyEmployeeId")).Text)).UserInfoID).CompanyId + "&SubId=" + ((Label)e.Row.FindControl("lblCompanyEmployeeId")).Text + "";
            }
            else
            {
                HiddenField hdnUserType = (HiddenField)e.Row.FindControl("hdnUserType");
                HiddenField hdnUserInfoID = (HiddenField)e.Row.FindControl("hdnUserInfoID");
                if (hdnUserType.Value == "1" || hdnUserType.Value == "2")
                {
                    ((HyperLink)e.Row.FindControl("hplFullName")).NavigateUrl = "~/admin/IncentexEmployee/BasicInformation.aspx?Id=" + new IncentexEmployeeRepository().GetEmployeeByUserInfoId(Convert.ToInt64(hdnUserInfoID.Value)).IncentexEmployeeID;
                }
                else if (hdnUserType.Value == "5")
                {
                    ((HyperLink)e.Row.FindControl("hplFullName")).NavigateUrl = "~/admin/Supplier/MainCompanyContact.aspx?Id=" + new SupplierRepository().GetByUserInfoId(Convert.ToInt64(hdnUserInfoID.Value)).SupplierID;
                }
            }
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
                case "CompanyName":
                    PlaceHolder placeholderComapany = (PlaceHolder)e.Row.FindControl("placeholderComapany");
                    // placeholderComapany.Controls.Add(ImgSort);
                    break;

            }

        }
    }
    #endregion

    #region Miscellaneous function
    public void BindSearchGrid()
    {
        try
        {
            DataView myDataView = new DataView();
            //List<UserInformationRepository.SearchResults> oj = new List<UserInformationRepository.SearchResults>();
            UserInformationRepository objUserInfoRepo = new UserInformationRepository();
           var Usrobj = objUserInfoRepo.getUsers(Request.QueryString["FirstName"].ToString(), Request.QueryString["LastName"].ToString(), "", "", Request.QueryString["Email"].ToString(), Convert.ToInt32(Request.QueryString["UserType"].ToString()), Request.QueryString["CompanyName"].ToString(), Request.QueryString["EmployeeID"].ToString(), Request.QueryString["EmployeeType"].ToString(), Request.QueryString["BaseStation"].ToString(), Request.QueryString["WorkGroup"].ToString(), Request.QueryString["Gender"].ToString());
           var oj = Usrobj;
            if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
           {
               oj = (from m in oj
                         where m.UserType != 1
                         select m).ToList();
           }

            //start this is for filter only getting data for company employee and company admin add by mayur
            if (!string.IsNullOrEmpty(Request.QueryString["ChartSubReport"]))
            {
                oj = oj.Where(x => x.UserType == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || x.UserType == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee)).ToList();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["Status"]) && Request.QueryString["Status"]!="0") //This is for comming from chart report
            {
                IQueryable<UserInformation> UserList = objUserInfoRepo.GetAllQuery();
                oj = (from result in oj
                     join userinfo in UserList on result.UserInformationId equals userinfo.UserInfoID
                     where userinfo.WLSStatusId == Convert.ToInt64(Request.QueryString["Status"])
                     select result).ToList();
            }
           //end

            lblRecords.Text = oj.Count.ToString();
            DataTable dataTable = ListToDataTable(oj);
            myDataView = dataTable.DefaultView;

            if (oj.Count == 0)
            {
                CurrentPage = 0;
                dvPager.Visible = false;
                lblMsg.Text = "No Records found matching your criteria..";
            }
            else
            {
                dvPager.Visible = true;
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
        catch { Response.Redirect("SearchUsers.aspx"); }

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
    #endregion
}
