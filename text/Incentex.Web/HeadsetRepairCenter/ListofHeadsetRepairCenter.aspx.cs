using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using System.Data;

public partial class HeadsetRepairCenter_ListofHeadsetRepairCenter : PageBase
{
    #region Data Member's
    long? Company
    {
        get
        {
            if (ViewState["Company"] != null)
                return Convert.ToInt64(ViewState["Company"].ToString());
            else
                return null;
        }
        set
        {
            ViewState["Company"] = value;
        }
    }
    long? Contact
    {
        get
        {
            if (ViewState["Contact"] != null)
                return Convert.ToInt64(ViewState["Contact"].ToString());
            else
                return null;
        }
        set
        {
            ViewState["Contact"] = value;
        }
    }
    long? Status
    {
        get
        {
            if (ViewState["Status"] != null)
                return Convert.ToInt64(ViewState["Status"].ToString());
            else
                return null;
        }
        set
        {
            ViewState["Status"] = value;
        }
    }
    String RepairNumber
    {
        get
        {
            if (ViewState["RepairNumber"] != null)
                return ViewState["RepairNumber"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["RepairNumber"] = value;
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

    HeadsetRepairCenterRepository objHeadsetRepair = new HeadsetRepairCenterRepository();
    #endregion

    #region Events
    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
            {
                MasterPageFile = "~/admin/MasterPageAdmin.master";
            }
            else
            {
                MasterPageFile = "~/MasterPage.master";
            }
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Headset Repair Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Headset Repair Center";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/HeadsetRepairCenter/SearchHeadsetRepairCenter.aspx";


            if (!String.IsNullOrEmpty(Request.QueryString["Company"]))
                this.Company = Convert.ToInt64(Request.QueryString["Company"]);
            if (!String.IsNullOrEmpty(Request.QueryString["Contact"]))
                this.Contact = Convert.ToInt64(Request.QueryString["Contact"]);
            if (!String.IsNullOrEmpty(Request.QueryString["RepairNumber"]))
                this.RepairNumber = Convert.ToString(Request.QueryString["RepairNumber"]);
            if (!string.IsNullOrEmpty(Request.QueryString["Status"]))
                this.Status = Convert.ToInt64(Request.QueryString["Status"]);


            BindGrid();
        }
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
        else if (e.CommandName == "RepairNumber")
        {
            Response.Redirect("~/HeadsetRepairCenter/HeadsetRepairDetails.aspx?RepairNumberDetails=" + Convert.ToInt64(e.CommandArgument) + "&Contact=" + this.Contact + "&Company=" + this.Company + "&RepairNumber=" + RepairNumber + "&Status=" + Status);
        }
        else if (e.CommandName == "DeleteHeadsetrepair")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }


            objHeadsetRepair.DeleteHeadsetRepairCenter(Convert.ToInt64(e.CommandArgument), IncentexGlobal.CurrentMember.UserInfoID);
            lblmsg.Text = "Record deleted successfully";
            BindGrid();
        }
    }

    public void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Image img = (Image)e.Row.FindControl("imgQuote");
            Label lblquote = (Label)e.Row.FindControl("lblIsCustomerApprovedQuote");
            HiddenField lblquotebeforerepaire = (HiddenField)e.Row.FindControl("hdnRequestquotebeforerepair");

            if (lblquote != null && (!string.IsNullOrEmpty(lblquote.Text.Trim())))
            {
                if (Convert.ToBoolean(lblquote.Text.Trim()))
                {
                    img.ImageUrl = "~/admin/Incentex_Used_Icons/approve_quote.png";
                    img.ToolTip = "Quote Approved";
                }
                else
                {
                    img.ImageUrl = "~/admin/Incentex_Used_Icons/rejected_quote.png";
                    img.ToolTip = "Quote Rejected";
                }
            }
            else if (lblquotebeforerepaire != null && !Convert.ToBoolean(lblquotebeforerepaire.Value))
            {
                img.ImageUrl = "~/admin/Incentex_Used_Icons/not_required_quote.png";
                img.ToolTip = "Quote Not Required";
            }
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
        List<Incentex.DAL.SqlRepository.HeadsetRepairCenterCustom> list = objHeadsetRepair.GetHeadsetRepairCenterBysearch(this.Company, this.Contact, this.RepairNumber, this.Status);


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

    #endregion
}
