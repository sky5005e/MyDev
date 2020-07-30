using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DAL;
using System.Linq;
using Incentex.DAL.SqlRepository;
using System.Data.Linq.SqlClient;
using Incentex.BE;

public partial class admin_CompanyStore_Product_ProductSearchResult : PageBase
{
    #region Data Member
    PagedDataSource pds = new PagedDataSource();
    ProductItemDetailsRepository objProductItemDetailsRepository = new ProductItemDetailsRepository();
    //Set default Substring length for the Product Description
    public int DescSubStringLength = 60;
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
         if (!IsPostBack)
        {
            base.MenuItem = "Product Search";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Search Result";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/Product/ProductSearch.aspx";

             BindGriedView();
        }
    }
    protected void gvProduct_RowCommand(object sender, GridViewCommandEventArgs e)
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
                BindGriedView();
            }
            if (e.CommandName.Equals("Detail"))
            {
                GridViewRow grdRow = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                HiddenField hdnStoreID = (HiddenField)grdRow.FindControl("hdnStoreID");
                HiddenField hdnWorkgroupName = (HiddenField)grdRow.FindControl("hdnWorkgroupName");
                Session["WorkgroupName"] = hdnWorkgroupName.Value;
                Session["WorkGroupNameToDisplay"] = hdnWorkgroupName.Value;
                IncentexGlobal.ProductReturnURL = Request.Url.ToString();
                Response.Redirect("~/admin/CompanyStore/Product/General.aspx?SubId=" + e.CommandArgument + "&Id=" + hdnStoreID.Value);
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
    #endregion

    #region Method
    private void BindGriedView()
    {
        string MasterItemNumber = null;
        string ItemNumber = null;
        decimal? Price = null;
        Int64? StoreID = null;
        Int64? WorkgroupID = null;
        string Description = null;
        string ReportViewType = null;
        int reportflag = 0;

        if (!string.IsNullOrEmpty(Request.QueryString["MasterNo"]))
            MasterItemNumber = Request.QueryString["MasterNo"].ToLower();
        if (!string.IsNullOrEmpty(Request.QueryString["ItemNumber"]))
            ItemNumber = Request.QueryString["ItemNumber"].ToLower();
        if (!string.IsNullOrEmpty(Request.QueryString["Price"]))
            Price = Convert.ToDecimal(Request.QueryString["Price"]);
        if (!string.IsNullOrEmpty(Request.QueryString["StoreID"]))
            StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
        if (!string.IsNullOrEmpty(Request.QueryString["WorkgroupID"]))
            WorkgroupID = Convert.ToInt64(Request.QueryString["WorkgroupID"]);
        if (!string.IsNullOrEmpty(Request.QueryString["Description"]))
            Description = Request.QueryString["Description"].ToLower();
        if (!string.IsNullOrEmpty(Request.QueryString["ReportViewType"]))
            ReportViewType = Request.QueryString["ReportViewType"].ToLower();

        if (ReportViewType == "inventory management report")
            reportflag = 1;

        bool CompanyColumnFlag = true;
        

        if (StoreID != null && StoreID != 0)
        {
            CompanyColumnFlag = false;
            gvProduct.Columns[0].Visible = false;
            tdCompany.Visible = true;
            DescSubStringLength = 70;//Set the Substring length for the Product Description 
        }
        else
        {
            CompanyColumnFlag = true;
            gvProduct.Columns[0].Visible = true;
            tdCompany.Visible = false;
            gvProduct.Columns[2].HeaderStyle.Width = Unit.Percentage(25); // Set Product Description Width No Company is Selected
            DescSubStringLength = 35;//Set the Substring length for the Product Description 
        }

        //if (CompanyColumnFlag)
        //{
        //    gvProduct.Columns[2].HeaderStyle.Width = Unit.Percentage(25);
        //}
        List<GetSearchProductAsPerReportResult> objResult = objProductItemDetailsRepository.GetProductSearchResult(StoreID, WorkgroupID, MasterItemNumber, ItemNumber, Price, Description, reportflag);

        if (ReportViewType == "vendor setup view")
        {
            gvProduct.Columns[3].Visible = true;//Vendor
            if (CompanyColumnFlag)
            {
                DescSubStringLength = 30; //Set the Substring length for the Product Description
            }
        }
        else if (ReportViewType == "pricing setup report")
        {
            gvProduct.Columns[4].Visible = true;//L1
            gvProduct.Columns[5].Visible = true;//L2
            gvProduct.Columns[6].Visible = true;//L3
            gvProduct.Columns[7].Visible = true;//L4
            gvProduct.Columns[8].Visible = true;//Close Out
            gvProduct.Columns[1].HeaderStyle.Width = Unit.Percentage(20); // Change the width of the Item Number Column
            if (CompanyColumnFlag)
            {
                DescSubStringLength = 30; //Set the Substring length for the Product Description
            }
        }
        else if (ReportViewType == "back order settings report")
        {
            gvProduct.Columns[9].Visible = true; // Allow Back Order
            if (CompanyColumnFlag)
            {
                gvProduct.Columns[2].HeaderStyle.Width = Unit.Percentage(34);
                DescSubStringLength = 50; //Set the Substring length for the Product Description 
            }
            else
            {
                DescSubStringLength = 82; //Set the Substring length for the Product Description 
            }
        }
        else if (ReportViewType == "inventory management report")
        {
            gvProduct.Columns[10].Visible = true;//12 Month Usage
            gvProduct.Columns[11].Visible = true;// Stock On Hand
            gvProduct.Columns[12].Visible = true;// Stock On Order
            gvProduct.Columns[13].Visible = true;// Expected Delievery Date
            gvProduct.Columns[1].HeaderStyle.Width = Unit.Percentage(22); // Change the width of the Item Number Column
            if (CompanyColumnFlag)
            {
                DescSubStringLength = 30; //Set the Substring length for the Product Description
            }
        }

        if (objResult.Count > 0)
        {
            DataView myDataView = new DataView();
            DataTable dataTable = ListToDataTable(objResult);
            myDataView = dataTable.DefaultView;
            lblRecords.Text = objResult.Count.ToString();
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
            gvProduct.DataSource = pds;
            gvProduct.DataBind();
            doPaging();
            pagingtable.Visible = true;

            if(!CompanyColumnFlag)
                lblCompanyName.Text = Convert.ToString(dataTable.Rows[0]["StoreName"]);
            

            
        }
        else
        {
            gvProduct.DataSource = null;
            gvProduct.DataBind();
            pagingtable.Visible = false;
            tdCompany.Visible = false;
            tdReords.Visible = false;
        }
    }
    #endregion

    #region Pagging
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

            DataList2.DataSource = dt;
            DataList2.DataBind();

        }
        catch (Exception)
        { }

    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindGriedView();
        
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
       
            BindGriedView();
       
    }
    protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
          
                BindGriedView();
        
        }
    }
    protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);
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
