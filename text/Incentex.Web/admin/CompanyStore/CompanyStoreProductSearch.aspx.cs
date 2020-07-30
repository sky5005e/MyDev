using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyStore_CompanyStoreProductSearch : PageBase
{
    LookupRepository objLookRep = new LookupRepository();
    ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            FillCompanyStore();
            FillMasterItemNumber();
            FillStyleNumber();
            BindGriedview();
            lblMsg.Text = "";
            ((Label)Master.FindControl("lblPageHeading")).Text = "Comapny Store Product Search";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";

        }
    }
    /// <summary>
    /// FillCompanyStore()
    /// Fill the dropdown Company store
    /// from company table.
    /// Nagmani 22/10/2010
    /// </summary>
    private void FillCompanyStore()
    {
        CompanyRepository objRepo = new CompanyRepository();
        List<Company> objList = new List<Company>();
        objList = objRepo.GetAllCompany();
        if (objList != null)
        {
             ddlComapnyStore.DataTextField = "CompanyName";
             ddlComapnyStore.DataValueField = "CompanyID";
             ddlComapnyStore.DataSource = objList;
             ddlComapnyStore.DataBind();
             ddlComapnyStore.Items.Insert(0, new ListItem("-select-", "0"));
           
           
        }
    }
    /// <summary>
    ///Fill the Master Item Number dropdownlist
    ///from lookup table
    ///FillMasterItemNumber()
    /// Nagmani 11/10/2010
    /// </summary>
    public void FillMasterItemNumber()
    {
        try
        {
            string strStatus = "MasterItemNumber";
            ddlMasterItemNo.DataSource = objLookRep.GetByLookup(strStatus);
            ddlMasterItemNo.DataValueField = "iLookupID";
            ddlMasterItemNo.DataTextField = "sLookupName";
            ddlMasterItemNo.DataBind();
            ddlMasterItemNo.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// Fill the Item No on the selection
    /// of Master item no slection crieteria
    /// </summary>
    /// <param name="masteritemno"></param>
    protected void ddlMasterItemNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMasterItemNo.SelectedIndex > 0)
        {
            FillItemNumber(Convert.ToInt32(ddlMasterItemNo.SelectedValue));
        }
    }
    /// <summary>
    ///FillItemNumber()
    ///Fill the ItemNumber dropdownlist]
    ///from the table ProductItem
    /// Nagmani 11/10/2010
    /// </summary>
    private void FillItemNumber(int iMasterItemNo)
    {
        try
        {
            List<ProductItem> objlist = new List<ProductItem>();
            objlist = objRepository.GetAllProductItem(iMasterItemNo);
            if (objlist != null)
            {
                ddlItemNo.DataSource = objlist;
                ddlItemNo.DataValueField = "ProductStyleID";
                ddlItemNo.DataTextField = "ItemNumber";
                ddlItemNo.DataBind();
                ddlItemNo.Items.Insert(0, new ListItem("-select-", "0"));
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    ///Fill the Style Number dropdownlist
    ///from lookup table
    ///FillStyleNumber()
    /// Nagmani 11/10/2010
    /// </summary>
    public void FillStyleNumber()
    {
        try
        {
            string strStatus = "StyleNo";
            ddlStyleNo.DataSource = objLookRep.GetByLookup(strStatus);
            ddlStyleNo.DataValueField = "iLookupID";
            ddlStyleNo.DataTextField = "sLookupName";
            ddlStyleNo.DataBind();
            ddlStyleNo.Items.Insert(0, new ListItem("-select-", "0"));
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    public void BindGriedview()
    {
        try
        {
        int? companyid=null;
        int? MasterStyleId=null;
        int? StyleNO=null;
        string Itmeno=null;
            string keyword=null;
                if(ddlComapnyStore.SelectedIndex>0)
                {
                    companyid=Convert.ToInt32(ddlComapnyStore.SelectedValue);
                }
        if(ddlMasterItemNo.SelectedIndex>0)
        {
           MasterStyleId=Convert.ToInt32(ddlMasterItemNo.SelectedValue) ;
        }
        if(ddlStyleNo.SelectedIndex>0)
        {
            StyleNO=Convert.ToInt32(ddlStyleNo.SelectedValue);
        }
        if(ddlItemNo.SelectedIndex>0)
        {
            Itmeno=ddlItemNo.SelectedItem.Text;
        }
        if(txtProductName.Text!="")
        {
            keyword=txtProductName.Text;
        }
        List<StoreProductSearchRepository.StoreProductSearchResult> oad = new StoreProductSearchRepository().StoreProductSearchDetails(companyid, MasterStyleId, Itmeno, StyleNO, keyword);
            if (oad.Count == 0)
                {
                      CurrentPage = 0;
                     dvPager.Visible = false;
                     divPaging.Visible = false;
                     lblMsg.Text = "No Records found matching your criteria..";
                }
                else
                {
                    dvPager.Visible = true;
                    divPaging.Visible = true;
                    lblMsg.Text = string.Empty;

                }
                DataView myDataView = new DataView();
                DataTable dataTable = ListToDataTable(oad);
                myDataView = dataTable.DefaultView;
                if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]); ;
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        grdStoreProductSearch.DataSource = pds;
        grdStoreProductSearch.DataBind();
        doPaging();
            }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
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
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindGriedview();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGriedview();
    }
    protected void dtlViewUsers_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindGriedview();
        }
    }
    protected void dtlViewUsers_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    protected void grdStoreProductSearch_RowCommand(object sender, GridViewCommandEventArgs e)
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

            BindGriedview();
        }
        if (e.CommandName == "Edit")  // or Edit
        {

            LinkButton lbtnAction;
            GridViewRow row;
            row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            lbtnAction = (LinkButton)(grdStoreProductSearch.Rows[row.RowIndex].FindControl("hypStyle"));
            Response.Redirect("~/admin/CompanyStore/Product/ViewStoreProduct.aspx");
        }

    }
    protected void grdStoreProductSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            if (this.ViewState["SortOrder"].ToString() == "ASC")
                ImgSort.ImageUrl = "";
            else
                ImgSort.ImageUrl = "";


            switch (this.ViewState["SortExp"].ToString())
            {
                case "MasterNo":
                    PlaceHolder placeholderMasterItemNo = (PlaceHolder)e.Row.FindControl("placeholderMasterNo");
                    
                    break;
                case "ProductStatus":
                    PlaceHolder placeholderProductStatus = (PlaceHolder)e.Row.FindControl("placeholderProductStatus");
                    
                    break;
              
                case "CompanyName":
                    PlaceHolder placeholderComapany = (PlaceHolder)e.Row.FindControl("placeholderComapany");
                    
                    break;
                case "ProductDescrption":
                    PlaceHolder placeholderProductDescrption = (PlaceHolder)e.Row.FindControl("placeholderProductDescrption");
                    
                    break;
                case "StoreStatus":
                    PlaceHolder placeholderStatus = (PlaceHolder)e.Row.FindControl("placeholderStoreStatus");

                    break;

            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((HiddenField)e.Row.FindControl("hdnStatusID")).Value == "Active")
            {
                ((HtmlImage)e.Row.FindControl("imgLookupIcon")).Src = "~/admin/Incentex_Used_Icons/EmploymentStatus _Active-Status.png";
            }
            else
            {
                ((HtmlImage)e.Row.FindControl("imgLookupIcon")).Src = "~/admin/Incentex_Used_Icons/EmploymentStatus _Deactivated.png";
            }
            //string path = "~/admin/Incentex_Used_Icons/" + ((HiddenField)e.Row.FindControl("hdnLookupIcon")).Value;
            //((HtmlImage)e.Row.FindControl("imgLookupIcon")).Src = path;
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        BindGriedview();
        lblMsg.Text = "";
        ddlStyleNo.SelectedIndex = 0;
        ddlMasterItemNo.SelectedIndex=0;
        ddlComapnyStore.SelectedIndex = 0;
        txtProductName.Text = "";
        ddlItemNo.SelectedIndex = 0;
       
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGriedview();
    }
}
    

    

