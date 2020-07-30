using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Data;

public partial class AssetManagement_RepairManagement_ViewOpenRepairOrders : PageBase
{
    #region DataMembers
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
    Int64 VEUserInfoID
    {
        get
        {
            if (ViewState["VEUserInfoID"] == null)
            {
                ViewState["VEUserInfoID"] = 0;
            }
            return Convert.ToInt64(ViewState["VEUserInfoID"]);
        }
        set
        {
            ViewState["VEUserInfoID"] = value;
        }
    }
    Int64 EquipmentTypeID
    {
        get
        {
            if (ViewState["EquipmentTypeID"] == null)
            {
                ViewState["EquipmentTypeID"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentTypeID"]);
        }
        set
        {
            ViewState["EquipmentTypeID"] = value;
        }
    }
    Int64 RepairOrderID
    {
        get
        {
            if (ViewState["RepairOrderID"] == null)
            {
                ViewState["RepairOrderID"] = 0;
            }
            return Convert.ToInt64(ViewState["RepairOrderID"]);
        }
        set
        {
            ViewState["RepairOrderID"] = value;
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
    Int64 EquipmentStatusID
    {
        get
        {
            if (ViewState["EquipmentStatusID"] == null)
            {
                ViewState["EquipmentStatusID"] = 0;
            }
            return Convert.ToInt64(ViewState["EquipmentStatusID"]);
        }
        set
        {
            ViewState["EquipmentStatusID"] = value;
        }
    }
    String FromDate
    {
        get
        {
            if (ViewState["FromDate"] == null)
            {
                ViewState["FromDate"] = 0;
            }
            return ViewState["FromDate"].ToString();
        }
        set
        {
            ViewState["FromDate"] = value;
        }
    }
    String ToDate
    {
        get
        {
            if (ViewState["ToDate"] == null)
            {
                ViewState["ToDate"] = 0;
            }
            return ViewState["ToDate"].ToString();
        }
        set
        {
            ViewState["ToDate"] = value;
        }
    }
    String IsBillingComplete
    {
        get
        {
            if (ViewState["IsBillingComplete"] == null)
            {
                ViewState["IsBillingComplete"] = "false";
            }
            return ViewState["IsBillingComplete"].ToString();
        }
        set
        {
            ViewState["IsBillingComplete"] = value;
        }
    }
    String BaseStationWise
    {
        get
        {
            if (ViewState["BaseStationWise"] == null)
            {
                ViewState["BaseStationWise"] = "false";
            }
            return ViewState["BaseStationWise"].ToString();
        }
        set
        {
            ViewState["BaseStationWise"] = value;
        }
    }
    AssetRepairMgtRepository objAssetRepairRepository = new AssetRepairMgtRepository();
    CompanyEmployee objCmpnyInfo = new CompanyEmployee();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    UserInformation objuser = new UserInformation();
    UserInformationRepository objuserrepos = new UserInformationRepository();
    CompanyStoreRepository objCompStoreRepos = new CompanyStoreRepository();
    CompanyStore objcomstore = new CompanyStore();
    PagedDataSource pds = new PagedDataSource();
    DataSet ds = new DataSet();

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
                base.MenuItem = "Repair Order Management";
                base.ParentMenuID = 46;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
                else
                    base.SetAccessRights(true, true, true, true);

                if (!base.CanView)
                {
                    base.RedirectToUnauthorised();
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Open Repair Orders";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";

              
                if (Request.QueryString.Count > 0)
                {

                    if (!String.IsNullOrEmpty(Request.QueryString["EquipmentTypeID"]))
                    {
                        EquipmentTypeID = Convert.ToInt64(Request.QueryString["EquipmentTypeID"]);
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["RepairOrderId"]))
                    {
                        RepairOrderID = Convert.ToInt64(Request.QueryString["RepairOrderId"]);
                    }
                    if (Request.QueryString["BaseStationID"] != "" && Request.QueryString["BaseStationID"] != null)
                    {
                        BaseStationID = Convert.ToInt64(Request.QueryString["BaseStationID"]);
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["EquipmentStatus"]))
                    {
                        EquipmentStatusID = Convert.ToInt64(Request.QueryString["EquipmentStatus"]);
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["ToDate"]))
                    {
                        ToDate = Request.QueryString["ToDate"].ToString();
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["FromDate"]))
                    {
                        FromDate = Request.QueryString["FromDate"].ToString();
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["CompanyID"]))
                    {
                        CompanyID = Convert.ToInt64(Request.QueryString["CompanyID"]);
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["IsBillingComplete"]))
                    {
                        IsBillingComplete = Convert.ToString(Request.QueryString["IsBillingComplete"]);
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["bs"]))
                    {
                        BaseStationWise = Convert.ToString(Request.QueryString["bs"]);
                    }

                }

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    CompanyID = Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId);
                }
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                {
                    VEUserInfoID = Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID);
                }
                if (IsBillingComplete == "true" || IsBillingComplete == "True")
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/RepairManagement/SearchPastRepairOrders.aspx";
                else
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/RepairManagement/RepairManagementIndex.aspx";
                BindGrid();

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
    /// Row Command Event of Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvRepairOrder_RowCommand(object sender, GridViewCommandEventArgs e)
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

            BindGrid();
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
    protected void gvRepairOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
            {
                Image ImgSort = new Image();
                switch (this.ViewState["SortExp"].ToString())
                {
                    case "EquipmentID":
                        PlaceHolder placeholderEquipmentID = (PlaceHolder)e.Row.FindControl("placeholderEquipmentID");
                        break;
                    case "EquiType":
                        PlaceHolder placeholderEquiType = (PlaceHolder)e.Row.FindControl("placeholderEquiType");
                        break;
                    case "BaseStation":
                        PlaceHolder placeholderBaseStation = (PlaceHolder)e.Row.FindControl("placeholderBaseStationID");
                        break;
                    case "Status":
                        PlaceHolder placeholderStatus = (PlaceHolder)e.Row.FindControl("placeholderStatus");
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

        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        try
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            string NotDeleted = "";

            bool IsAnySelected = false;
            foreach (GridViewRow gr in gvRepairOrder.Rows)
            {
                CheckBox chkDelete = gr.FindControl("chkDelete") as CheckBox;
                Label lblID = gr.FindControl("lblRepairOrderID") as Label;

                HyperLink lnkEditSupp = gr.FindControl("lnkRepairOrderID") as HyperLink;

                if (chkDelete.Checked)
                {                   
                    objAssetRepairRepository.DeleteRepairOrder(Convert.ToInt64(lblID.Text));
                    IsAnySelected = true;                   
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
                    lblmsg.Text = NotDeleted + " can not be deleted as they contains 1 or more records.";
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

        objAssetMgtRepository.SubmitChanges();
        BindGrid();
    }

    #endregion
    #region Methods
    public void BindGrid()
    {
        DataView myDataView = new DataView();

        DataTable dt = new DataTable();
        
        UserInformation objuser = new UserInformation();
        UserInformationRepository objuserrepos = new UserInformationRepository();
        try
        {
            if(BaseStationWise=="false")
            dt = ListToDataTable(objAssetRepairRepository.GetRepairOrderListBy(this.CompanyID, this.EquipmentTypeID, this.RepairOrderID, this.BaseStationID, this.EquipmentStatusID, this.FromDate.ToString(), this.ToDate.ToString(), IsBillingComplete));
            else
            dt = ListToDataTable(objAssetRepairRepository.GetRepairOrderByBaseStation(this.CompanyID, IncentexGlobal.CurrentMember.UserInfoID));            
            if (dt.Rows.Count == 0)
            {
                dvTotalRecords.Visible = false;
                pagingtable.Visible = false;
                btnDelete.Visible = false;
                lblmsg.Text = "No records found";
            }
            else
            {
                dvTotalRecords.Visible = true;
                LblTotalRecords.Text = Convert.ToString(dt.Rows.Count);
                pagingtable.Visible = true;
                btnDelete.Visible = true;
            }
            //gvEquipment.DataSource = dt;
            //gvEquipment.DataBind();

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

            gvRepairOrder.DataSource = pds;
            gvRepairOrder.DataBind();
            doPaging();

        }
        catch (Exception)
        {
            dvTotalRecords.Visible = false;
            pagingtable.Visible = false;
            btnDelete.Visible = false;

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
        BindGrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindGrid();
    }
    #endregion
}




