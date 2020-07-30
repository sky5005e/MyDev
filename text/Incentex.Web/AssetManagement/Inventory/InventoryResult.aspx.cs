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
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL.Common;
public partial class AssetManagement_Inventory_InventoryResult : PageBase
{
    #region DataMembers
    CompanyEmployee objCmpnyInfo = new CompanyEmployee();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    UserInformation objuser = new UserInformation();
    UserInformationRepository objuserrepos = new UserInformationRepository();
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
    Int64 ProductSCategoryID
    {
        get
        {
            if (ViewState["ProductSCategoryID"] == null)
            {
                ViewState["ProductSCategoryID"] = 0;
            }
            return Convert.ToInt64(ViewState["ProductSCategoryID"]);
        }
        set
        {
            ViewState["ProductSCategoryID"] = value;
        }
    }
    string PartNumber = null;
    string MFGPartNumber = null;
    PagedDataSource pds = new PagedDataSource();
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
            if (!IsPostBack)
            {
                base.MenuItem = "Station Inventory";
                base.ParentMenuID = 46;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }
                
                ((Label)Master.FindControl("lblPageHeading")).Text = "Inventory Result";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/Inventory/SearchInventory.aspx";

                //------Getting Data From Query String

                if (Request.QueryString.Count > 0)
                {
                    this.CompanyID = Convert.ToInt64(Request.QueryString["CompanyID"]);
                    this.BaseStationID = Convert.ToInt64(Request.QueryString["BaseStationID"]);
                    this.ProductCategoryID = Convert.ToInt64(Request.QueryString["ProductCategoryID"]);
                    this.ProductSCategoryID = Convert.ToInt64(Request.QueryString["ProductSCategoryID"]);
                    if (Convert.ToString(Request.QueryString["PartNumber"])!=string.Empty)
                    {
                        this.PartNumber = Convert.ToString(Request.QueryString["PartNumber"]);    
                    }
                    if (Convert.ToString(Request.QueryString["MFGPartNumber"])!=string.Empty)
                    {
                        this.MFGPartNumber = Convert.ToString(Request.QueryString["MFGPartNumber"]);    
                    }
                    

                }

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    this.CompanyID = Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId);
                }

                bindgrid();
            }
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
                    case "Vendor":
                        PlaceHolder placeholderVendor = (PlaceHolder)e.Row.FindControl("placeholderVendor");
                        break;
                    case "PartNumber":
                        PlaceHolder placeholderPartNumber = (PlaceHolder)e.Row.FindControl("placeholderPartNumber");
                        break;
                    case "MPartNumber":
                        PlaceHolder placeholderMPartNumber = (PlaceHolder)e.Row.FindControl("placeholderMPartNumber");
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

    /// <summary>
    /// Delete button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnDelete_Click(object sender, EventArgs e)
    {

        AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();
        try
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            string NotDeleted = "";

            bool IsAnySelected = false;
            foreach (GridViewRow gr in gvEquipment.Rows)
            {
                CheckBox chkDelete = gr.FindControl("chkDelete") as CheckBox;
                Label lblID = gr.FindControl("lblEquipmentInventoryID") as Label;

                HyperLink lnkEditSupp = gr.FindControl("lnkEditSupp") as HyperLink;

                if (chkDelete.Checked)
                {
                    // check has employee
                    EquipmentInventoryMaster objInventory = new EquipmentInventoryMaster();
                    objInventory = objInventoryRepository.GetInventoryById(Convert.ToInt64(lblID.Text));
                   
                    IsAnySelected = true;

                    if (objInventory !=null)
                    {
                        // delete emp
                        objInventoryRepository.DeleteInventory(Convert.ToInt64(lblID.Text));
                    }
                    else
                    {
                        NotDeleted += lnkEditSupp.Text + ",";
                    }
                }
            }

            if (IsAnySelected)
            {
                if (string.IsNullOrEmpty(NotDeleted))
                {
                    lblmsg.Text = "Selected Records Deleted Successfully ...";
                }
                else
                {
                    NotDeleted = NotDeleted.Substring(0, NotDeleted.Length - 1);
                    lblmsg.Text = NotDeleted + " can not be deleted as they contains 1 or more Records.";
                }


            }
            else
            {
                lblmsg.Text = "Please Select Record to delete ...";
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = "Error in deleting record ...";
            ErrHandler.WriteError(ex);
        }

        objInventoryRepository.SubmitChanges();
        bindgrid();
    }

    #endregion
    #region Methods
    public void bindgrid()
    {
        DataView myDataView = new DataView();

        DataTable dt = new DataTable();
        AssetInventoryRepository objInventoryRepository = new AssetInventoryRepository();       
        try
        {

            dt = ListToDataTable(objInventoryRepository.GetInventoryResult(this.CompanyID, this.BaseStationID, this.ProductCategoryID, this.ProductSCategoryID, this.PartNumber, this.MFGPartNumber));
            if (dt.Rows.Count == 0)
            {
                dvTotalRecords.Visible = false;
                pagingtable.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                dvTotalRecords.Visible = true;
                LblTotalRecords.Text = Convert.ToString(dt.Rows.Count);
                pagingtable.Visible = true;
                btnDelete.Visible = true;
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

    /// <summary>
    /// Function which converts List to the DataTable
    /// </summary>


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
