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
public partial class AssetManagement_Reports_SpendByAssetPage : PageBase
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
    String EquipmentID
    {
        get
        {
            if (ViewState["EquipmentID"] == null)
            {
                ViewState["EquipmentID"] = 0;
            }
            return Convert.ToString(ViewState["EquipmentID"]);
        }
        set
        {
            ViewState["EquipmentID"] = value;
        }
    }
    Int64 EquiTypeIDView
    {
        get
        {
            if (ViewState["EquiTypeIDView"] == null)
            {
                ViewState["EquiTypeIDView"] = 0;
            }
            return Convert.ToInt64(ViewState["EquiTypeIDView"]);
        }
        set
        {
            ViewState["EquiTypeIDView"] = value;
        }
    }
    string FrmDate = null;
    string ToDate = null;      
    string GSEDepartmentID = null;
    string EquipmentStatus = null;

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

                CheckLogin();
                ((Label)Master.FindControl("lblPageHeading")).Text = "Search Equipment";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                

                //------Getting Data From Query String

                if (Request.QueryString.Count > 0)
                {
                   
                    if (Request.QueryString["EquipmentTypeID"] != "" && Request.QueryString["EquipmentTypeID"] != null && Request.QueryString["EquipmentTypeID"] != "0")
                    {
                        EquipmentTypeID = Convert.ToInt64(Request.QueryString["EquipmentTypeID"]);
                    }
                    if (Request.QueryString["EquipmentID"] != "" && Request.QueryString["EquipmentID"] != null)
                    {
                        EquipmentID = Convert.ToString(Request.QueryString["EquipmentID"]);
                    }
                    if (Request.QueryString["BaseStationID"] != "" && Request.QueryString["BaseStationID"] != null && Request.QueryString["BaseStationID"] != "0")
                    {
                        BaseStationID = Convert.ToInt64(Request.QueryString["BaseStationID"]);
                    }
                    if (Request.QueryString["FDate"] != "" && Request.QueryString["FDate"] != null)
                    {
                        FrmDate = Convert.ToString(Request.QueryString["FDate"]);
                    }
                    if (Request.QueryString["TDate"] != "" && Request.QueryString["TDate"] != null)
                    {
                        ToDate = Convert.ToString(Request.QueryString["TDate"]);
                    }
                    if (Request.QueryString["EquiTypeIDView"] != "" && Request.QueryString["EquiTypeIDView"] != null && Request.QueryString["EquiTypeIDView"] != "0")
                    {
                        EquiTypeIDView = Convert.ToInt64(Request.QueryString["EquiTypeIDView"]);
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

                bindgrid();

                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/Reports/SpendByAsset.aspx?EquipmentTypeID=" + this.EquiTypeIDView + "&BaseStationID=" + this.BaseStationID + "&EquipmentID=" + EquipmentID + "&FromDate=" + Convert.ToString(Convert.ToDateTime(this.FrmDate).ToString("MM/dd/yyyy")) + "&ToDate=" + Convert.ToString(Convert.ToDateTime(this.ToDate).ToString("MM/dd/yyyy"));
            }
        }
        catch (Exception)
        {


        }
    }
    //protected void btnAddEquipment_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("AddEquipment.aspx");
    //}


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
                    case "EquipmentID":
                        PlaceHolder placeholderEquipmentID = (PlaceHolder)e.Row.FindControl("placeholderEquipmentID");
                        break;
                    case "EquiType":
                        PlaceHolder placeholderEquiType = (PlaceHolder)e.Row.FindControl("placeholderEquiType");
                        break;
                    case "BaseStation":
                        PlaceHolder placeholderBaseStation = (PlaceHolder)e.Row.FindControl("placeholderBaseStationID");
                        break;
                    case "Spend":
                        PlaceHolder placeholderSpend = (PlaceHolder)e.Row.FindControl("placeholderSpend");
                        break;

                }

            }
        }
        catch (Exception)
        {


        }
    }

    

    #endregion
    #region Methods
   
    /// <summary>
    /// Equipment Status Report
    /// </summary>
    public void bindgrid()
    {
        DataView myDataView = new DataView();

        DataTable dt = new DataTable();
        AssetReportRepository objReportRepository = new AssetReportRepository();

        try
        {
            if (this.VEUserInfoID!=0)
                dt = ListToDataTable(objReportRepository.GetAssetSpendDetailVE(Convert.ToDateTime(FrmDate), Convert.ToDateTime(ToDate), this.CompanyID, this.EquipmentTypeID, this.BaseStationID, this.VEUserInfoID));    
            else
                dt = ListToDataTable(objReportRepository.GetAssetSpendDetail(Convert.ToDateTime(FrmDate), Convert.ToDateTime(ToDate), this.CompanyID, this.EquipmentTypeID, this.BaseStationID));    
            
            if (dt.Rows.Count == 0)
            {
                dvTotalRecords.Visible = false;
                pagingtable.Visible = false;               
            }
            else
            {
                dvTotalRecords.Visible = true;
                LblTotalRecords.Text = Convert.ToString(dt.Rows.Count);
                pagingtable.Visible = true;                
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
