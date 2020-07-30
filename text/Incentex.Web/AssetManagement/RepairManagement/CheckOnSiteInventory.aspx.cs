using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Data;
using System.IO;
using System.Text;
using System.Configuration;
using commonlib.Common;
public partial class AssetManagement_RepairManagement_CheckOnSiteInventory : PageBase
{
    #region DataMembers
    PagedDataSource pds = new PagedDataSource();
    CompanyEmployee objCmpnyInfo = new CompanyEmployee();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    UserInformation objuser = new UserInformation();
    UserInformationRepository objuserrepos = new UserInformationRepository();
    AssetRepairMgtRepository objRepairMgtRepository = new AssetRepairMgtRepository();
    Int64 RepaiOrderID
    {
        get
        {
            if (ViewState["RepaiOrderID"] == null)
            {
                ViewState["RepaiOrderID"] = 0;
            }
            return Convert.ToInt64(ViewState["RepaiOrderID"]);
        }
        set
        {
            ViewState["RepaiOrderID"] = value;
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
    Int64 BaseStationID
    {
        get
        {
            if (ViewState["BaseStationID"] == null)
            {
                ViewState["BaseStationID"] = 0;
            }
            return Convert.ToInt64(ViewState["BaseStationID"]);
        }
        set
        {
            ViewState["BaseStationID"] = value;
        }
    }
    Int64 ProductCategoryID
    {
        get
        {
            if (ViewState["ProductCategoryID"] == null)
            {
                ViewState["ProductCategoryID"] = 0;
            }
            return Convert.ToInt64(ViewState["ProductCategoryID"]);
        }
        set
        {
            ViewState["ProductCategoryID"] = value;
        }
    }
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
        try
        {
            CheckLogin();
            

            if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    if (string.IsNullOrEmpty(Request.QueryString["id"]) == false)
                    {
                        RepaiOrderID = Convert.ToInt64(Request.QueryString["id"]);
                    }
                }
                ((Label)Master.FindControl("lblPageHeading")).Text = "Search Equipment Inventory";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/RepairManagement/RepairProfile.aspx?Page=CheckOnSiteInventory&RepairOrderId=" + this.RepaiOrderID;
                lblMsg.Text = "";
                BindValues();
                pagingtable.Visible = false;
                lnkBtnPickInventory.Visible = false;
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    ddlCompany.SelectedValue = Convert.ToString(IncentexGlobal.CurrentMember.CompanyId);
                    ddlCompany.Enabled = false;
                }
                else
                {
                    ddlCompany.Items.Insert(0, new ListItem("-Select-", "0"));
                    ddlCompany.Enabled = true;
                }
            }
        }
        catch (Exception)
        {

        }
    }
    protected void lnkBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {

            this.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            this.BaseStationID = Convert.ToInt64(ddlBaseStation.SelectedValue);
            this.ProductCategoryID = Convert.ToInt64(ddlProductCategory.SelectedValue);
            bindgrid();
            //Response.Redirect("~/AssetManagement/Inventory/InventoryResult.aspx?CompanyID=" + CompanyID + "&BaseStationID=" + BaseStationID + "&ProductCategoryID=" + ProductCategoryID + "&ProductSCategoryID=" + ProductSCategoryID + "&PartNumber=" + PartNumber + "&MFGPartNumber=" + MFGPartNumber);
        }
        catch (Exception)
        {

        }
    }
    /// <summary>
    /// Row Command Event of Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEquipment_RowCommand(object sender, GridViewCommandEventArgs e)
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

            bindgrid();
        }
        catch (Exception)
        {

        }
    }
    /// <summary>
    /// Row Databound event of a grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEquipment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
            {
                Image ImgSort = new Image();
                switch (this.ViewState["SortExp"].ToString())
                {
                    case "CurrentInvenory":
                        PlaceHolder placeholderInStock = (PlaceHolder)e.Row.FindControl("placeholderInStock");
                        break;
                    case "PartNumber":
                        PlaceHolder placeholderPartNumber = (PlaceHolder)e.Row.FindControl("placeholderPartNumber");
                        break;
                    case "ProductDescription":
                        PlaceHolder placeholderProductDescription = (PlaceHolder)e.Row.FindControl("placeholderProductDescription");
                        break;

                }

            }
        }
        catch (Exception)
        {


        }
    }
    protected void lnkBtnPickInventory_Click(object sender, EventArgs e)
    {
        Int64 ErrCount = 0;
        foreach (GridViewRow t in gvEquipment.Rows)
        {

            TextBox txtPickQty = (TextBox)t.FindControl("txtPickQty");
            Label lblInStock = (Label)t.FindControl("lblInStock");
            Label lblEquipmentInventoryID = (Label)t.FindControl("lblEquipmentInventoryID");
            Label lblPartNumber = (Label)t.FindControl("lblPartNumber");
            Label lblProductDescription = (Label)t.FindControl("lblProductDescription");
            Label lblVendorID = (Label)t.FindControl("lblVendorID");
            Int64 FinalInventory = 0;
            Int64 PickQty = Convert.ToInt64(txtPickQty.Text.Trim());
            Int64 InStock = string.IsNullOrEmpty(lblInStock.Text.Trim()) ? 0 : Convert.ToInt64(lblInStock.Text.Trim());
            Int64 InventoryID = Convert.ToInt64(lblEquipmentInventoryID.Text.Trim());
            String PartNumber = lblPartNumber.Text.Trim();
            String ProductDescription = lblProductDescription.Text.Trim();
            Int64 VendorID = string.IsNullOrEmpty(lblVendorID.Text.Trim()) ? 0 : Convert.ToInt64(lblVendorID.Text.Trim());
            if (PickQty != 0)
            {
                if (string.IsNullOrEmpty(InStock.ToString()) == false)
                {
                    if (InStock != 0)
                    {

                        if (InStock >= PickQty)
                        {
                            FinalInventory = InStock - PickQty;
                            objRepairMgtRepository.UpdateInventoryParts(Convert.ToString(FinalInventory), PickQty, this.RepaiOrderID, InventoryID, IncentexGlobal.CurrentMember.UserInfoID, PartNumber, ProductDescription, VendorID);                           
                        }

                        else
                        {
                            ErrCount = ErrCount + 1;
                        }
                    }
                    else
                    {
                        ErrCount = ErrCount + 1;
                    }
                }
                else
                {
                    ErrCount = ErrCount + 1;
                }
            }
           

        }
        if (ErrCount==0)
        {
            Response.Redirect("RepairProfile.aspx?Page=CheckOnSiteInventory&RepairOrderId="+this.RepaiOrderID);
        }
        else
        {
            lblMsg.Text = "Unable to Pick Inventory";
            bindgrid();
        }

    }   
    #endregion
    #region Methods
    public void BindValues()
    {
        try
        {

            //get Base Stations
            CompanyRepository objRepo = new CompanyRepository();
            List<INC_BasedStation> objBaseStationList = new List<INC_BasedStation>();
            objBaseStationList = objRepo.GetAllBaseStationResult();
            objBaseStationList = objBaseStationList.OrderBy(p => p.sBaseStation).ToList();
            Common.BindDDL(ddlBaseStation, objBaseStationList, "sBaseStation", "iBaseStationId", "-Select All BaseSation-");

            //get company
            CompanyRepository objCompanyRepo = new CompanyRepository();
            List<Company> objCompanyList = new List<Company>();
            objCompanyList = objCompanyRepo.GetAllCompany();
            Common.BindDDL(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "-Select Company-");

            //get Product Category
            AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
            List<EquipmentProductCategoryLookup> objCategoryList = new List<EquipmentProductCategoryLookup>();
            objCategoryList = objInventoryRepository.GetAllProductCategory();
            Common.BindDDL(ddlProductCategory, objCategoryList, "ProductCategoryName", "ProductCategoryID", "-Select Product Category-");

        }
        catch (Exception)
        {


        }
    }
    public void bindgrid()
    {
        DataView myDataView = new DataView();

        DataTable dt = new DataTable();
        AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
        try
        {

            dt = ListToDataTable(objInventoryRepository.GetInventoryResult(this.CompanyID, this.BaseStationID, this.ProductCategoryID, 0, null, null));
            if (dt.Rows.Count == 0)
            {
                pagingtable.Visible = false;
                lnkBtnPickInventory.Visible = false;
            }
            else
            {               
                pagingtable.Visible = true;
                lnkBtnPickInventory.Visible = true;
            }

            myDataView = dt.DefaultView;
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

            gvEquipment.DataSource = pds;
            gvEquipment.DataBind();
            doPaging();

        }
        catch (Exception)
        {


        }
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            //table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

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
    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindgrid();
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

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();

        }
        catch (Exception)
        { }

    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        bindgrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindgrid();
    }
    #endregion
}
