using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_PurchaseOrderManagement_ProductionOrderDetails : PageBase
{
    #region Data Member's
    long VendorID
    {
        get
        {
            if (ViewState["VendorID"] != null)
                return Convert.ToInt64(ViewState["VendorID"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["VendorID"] = value;
        }
    }
    long MasterItemID
    {
        get
        {
            if (ViewState["MasterItemID"] != null)
                return Convert.ToInt64(ViewState["MasterItemID"].ToString());
            else
                return 0;
        }
        set
        {
            ViewState["MasterItemID"] = value;
        }
    }
    string PONumber
    {
        get
        {
            if (ViewState["PONumber"] != null)
                return ViewState["PONumber"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["PONumber"] = value;
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
    /// <summary>
    /// Set Date as Per Request
    /// </summary>
    DateTime SearchDate
    {
        get
        {
            if (ViewState["SearchDate"] == null)
                ViewState["SearchDate"] = DateTime.Now;

            return Convert.ToDateTime(ViewState["SearchDate"]);
        }
        set
        {
            ViewState["SearchDate"] = value;
        }
    }

    PurchaseOrderManagmentRepository objOrderRep = new PurchaseOrderManagmentRepository();
    #endregion
    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Purchase Order Management";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Purchase Order Management";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/VendorAccessIncOrderManegement.aspx";

            if (!String.IsNullOrEmpty(Request.QueryString["search"]))
                SetSearchDate(Request.QueryString["search"]);

            if (!String.IsNullOrEmpty(Request.QueryString["SearchDate"]))
                this.SearchDate = Convert.ToDateTime(Request.QueryString["SearchDate"]);

            if (!String.IsNullOrEmpty(Request.QueryString["PONumber"]))
                this.PONumber = Convert.ToString(Request.QueryString["PONumber"]);
            if (!String.IsNullOrEmpty(Request.QueryString["vendorID"]))
                this.VendorID = Convert.ToInt64(Request.QueryString["vendorID"]);


            BindGridView();
        }
    }

    public void grdViewTP_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Label lblFileSize = (Label)e.Row.FindControl("lblFileSize");
        HiddenField hdnextension = (HiddenField)e.Row.FindControl("hdnextension");
        System.Web.UI.HtmlControls.HtmlImage viewimg = (System.Web.UI.HtmlControls.HtmlImage)e.Row.FindControl("viewimg");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (hdnextension.Value.ToLower() == ".pdf")
                viewimg.Src = "~/Images/FileType/pdf.png";
            else if (hdnextension.Value.ToLower() == ".jpg" || hdnextension.Value.ToLower() == ".jpeg")
                viewimg.Src = "~/Images/FileType/jpg.png";
            else if (hdnextension.Value.ToLower() == ".doc" || hdnextension.Value.ToLower() == ".xls" || hdnextension.Value.ToLower() == ".xlsx")
                viewimg.Src = "~/Images/FileType/doc.png";
        }
    }

    protected void grdViewTP_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
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
            }
            BindGridView();
        }
        catch (Exception ex)
        {
        }
    }
    #endregion 
    #region Page Methods
    private void BindGridView()
    {
        DataView myDataView = new DataView();
        List<SearchPurchaseOrder> list = objOrderRep.GetPurchaseOrderDetailsBySearch(this.VendorID, this.MasterItemID, this.PONumber);

        SupplierEmployee objSup = new SupplierEmployeeRepository().GetEmployeeByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        if (objSup != null && list.Count > 0 && this.VendorID == 0 && String.IsNullOrEmpty(this.PONumber))
        {
            list = list.Where(q => q.VendorID == objSup.SupplierID && q.StartProductionDate >= SearchDate.Date).ToList();
        }
       
        if (list.Count == 0)
        {
            pagingtable.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
        }


        DataTable dataTable = Common.ListToDataTable(list);
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

        grdViewTP.DataSource = pds;
        grdViewTP.DataBind();

        doPaging();
    }

    private void SetSearchDate(String request)
    {
        if (request == "week")
        {
            // To get the First date of week (ie Monday)
            int diff = DayOfWeek.Monday - DateTime.Now.DayOfWeek;
            SearchDate = SearchDate.AddDays(diff);
        }
        else
        {
            SearchDate = DateTime.Now;
        }
    }
    #endregion 
    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGridView();
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
        BindGridView();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGridView();
    }
    #endregion

   
}
