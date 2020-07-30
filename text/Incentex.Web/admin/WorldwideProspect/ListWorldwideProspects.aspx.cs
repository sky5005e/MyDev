using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using System.Data;

public partial class WorldwideProspect_ListWorldwideProspects : PageBase
{

    #region Data Member's
    long Country
    {
        get
        {
            if (ViewState["Country"] != null)
                return Convert.ToInt64(ViewState["Country"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["Country"] = value;
        }
    }
    long BusinessType
    {
        get
        {
            if (ViewState["BusinessType"] != null)
                return Convert.ToInt64(ViewState["BusinessType"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["BusinessType"] = value;
        }
    }
    String CompanyName
    {
        get
        {
            if (ViewState["CompanyName"] != null)
                return ViewState["CompanyName"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["CompanyName"] = value;
        }
    }
    String ContactName
    {
        get
        {
            if (ViewState["ContactName"] != null)
                return ViewState["ContactName"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["ContactName"] = value;
        }
    }
    String Email
    {
        get
        {
            if (ViewState["Email"] != null)
                return ViewState["Email"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["Email"] = value;
        }
    }


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

    WorldwideProspectsRepository objProspectRepair = new WorldwideProspectsRepository();
    #endregion

    #region Event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Worldwide Prospects";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Worldwide Prospects";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/WorldwideProspect/SearchProspects.aspx";


            if (!String.IsNullOrEmpty(Request.QueryString["Company"]))
                this.CompanyName = Convert.ToString(Request.QueryString["Company"]);
            if (!String.IsNullOrEmpty(Request.QueryString["Contact"]))
                this.ContactName = Convert.ToString(Request.QueryString["Contact"]);
            if (!String.IsNullOrEmpty(Request.QueryString["Email"]))
                this.Email = Convert.ToString(Request.QueryString["Email"]);
            if (!string.IsNullOrEmpty(Request.QueryString["BusinessType"]))
                this.BusinessType = Convert.ToInt64(Request.QueryString["BusinessType"]);
            if (!string.IsNullOrEmpty(Request.QueryString["Country"]))
                this.Country = Convert.ToInt64(Request.QueryString["Country"]);

            BindGrid();
        }
    }

    public void grdView_RowCommand(object sender, GridViewCommandEventArgs e)
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

            BindGrid();
        }
        else if (e.CommandName == "DeleteProspect")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            objProspectRepair.Deleteprospect(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);
            lblmsg.Text = "Record deleted successfully";
            BindGrid();
        }
        else if (e.CommandName == "CompanyName")
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            Response.Redirect("AddProspects.aspx?ProspectID=" + e.CommandArgument + "&Company=" + this.CompanyName + "&Contact=" + this.ContactName + "&Email=" + this.Email + "&BusinessType=" + this.BusinessType + "&Country=" + this.Country);
        }
    }

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGrid();
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

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();

        }
        catch (Exception)
        { }
    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindGrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGrid();
    }
    #endregion

    #endregion

    #region Methods

    public void BindGrid()
    {
        DataView myDataView = new DataView();
        List<Incentex.DAL.SqlRepository.WorldwideProspectsRepository.WorldwideprospectCustom> list = objProspectRepair.GetAllworldwideprospectBysearch(CompanyName, ContactName, Email, BusinessType, Country);


        if (list.Count == 0)
        {
            pagingtable.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
        }


        DataTable dataTable = ListToDataTable(list);
        myDataView = dataTable.DefaultView;

        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }


        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = 50; //Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;


        grdView.DataSource = pds;
        grdView.DataBind();

        doPaging();
    }

    /// <summary>
    /// Converting from List to Datatables
    /// </summary>
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
