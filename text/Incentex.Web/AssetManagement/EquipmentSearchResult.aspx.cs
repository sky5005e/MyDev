using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;

public partial class AssetManagement_EquipmentSearchResult : PageBase
{
    #region DataMembers   
    CompanyEmployee objCmpnyInfo = new CompanyEmployee();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    UserInformation objuser = new UserInformation();
    UserInformationRepository objuserrepos = new UserInformationRepository();
    Int64 CompanyID = 0;
    Int64 WorkgroupID = 0;
    string EquipmentTypeID = null;
    string EquipmentID = null;
    string CurrentLocationID = null;
    string EquipmentStatus = null;

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
                ((Label)Master.FindControl("lblPageHeading")).Text = "Manage Equipment";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/SearchEquipment.aspx";

                if (Request.QueryString.Count > 0)
                {

                    if (Request.QueryString["EquipmentTypeID"] != "" && Request.QueryString["EquipmentTypeID"] != null)
                    {
                        EquipmentTypeID = Convert.ToString(Request.QueryString["EquipmentTypeID"]);
                    }
                    if (Request.QueryString["EquipmentID"] != "" && Request.QueryString["EquipmentID"] != null)
                    {
                        EquipmentID = Convert.ToString(Request.QueryString["EquipmentID"]);
                    }
                    if (Request.QueryString["CurrentLocationID"] != "" && Request.QueryString["CurrentLocationID"] != null)
                    {
                        CurrentLocationID = Convert.ToString(Request.QueryString["CurrentLocationID"]);
                    }
                    if (Request.QueryString["EquipmentStatus"] != "" && Request.QueryString["EquipmentStatus"] != null)
                    {
                        EquipmentStatus = Convert.ToString(Request.QueryString["EquipmentStatus"]);
                    }
                    if (Request.QueryString["CompanyID"] != "" && Request.QueryString["CompanyID"] != null)
                    {
                        CompanyID = Convert.ToInt64(Request.QueryString["CompanyID"]);
                    }
                    else
                    {
                        CompanyID = 0;
                    }
                    if (Request.QueryString["WorkgroupID"] != "" && Request.QueryString["WorkgroupID"] != null)
                    {
                        WorkgroupID = Convert.ToInt64(Request.QueryString["WorkgroupID"].ToString());
                    }
                    else
                    {
                        WorkgroupID = 0;
                    }

                }
                bindgrid();
            }
        }
        catch (Exception)
        {


        }
    }
    #endregion
    #region Methods
    public void bindgrid()
    {
        DataView myDataView = new DataView();

        DataTable dt = new DataTable();
        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        try
        {


            dt = ListToDataTable(objAssetMgtRepository.GetEquipmentsDetail(null, null, null, null, null, null,null, CompanyID,0));//0 for VEUserInfoID
            if (dt.Rows.Count == 0)
            {
                pagingtable.Visible = false;

            }
            else
            {
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
