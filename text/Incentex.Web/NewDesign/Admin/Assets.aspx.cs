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

public partial class Admin_Assets : PageBase
{

    #region DataMembers
    PagedDataSource pds = new PagedDataSource();
    #endregion

    #region Properties & Fields

    public String CompanyStore
    {
        get
        {
            if (Convert.ToString(this.Session["CompanyStore"]) == String.Empty)
                return null;
            else
                return Convert.ToString(this.Session["CompanyStore"]);
        }
        set
        {
            this.Session["CompanyStore"] = value;
        }
    }

    public Int64? DepartmentID
    {
        get
        {
            if (Convert.ToInt64(this.Session["DepartmentID"]) == 0)
                return null;
            else
                return Convert.ToInt64(this.Session["DepartmentID"]);
        }
        set
        {
            this.Session["DepartmentID"] = value;
        }
    }

    public Int64? BaseStationID
    {
        get
        {
            if (Convert.ToInt64(this.Session["BaseStationID"]) == 0)
                return null;
            else
                return Convert.ToInt64(this.Session["BaseStationID"]);
        }
        set
        {
            this.Session["BaseStationID"] = value;
        }
    }

    public Int64? AssetTypeID
    {
        get
        {
            if (Convert.ToInt64(this.Session["AssetTypeID"]) == 0)
                return null;
            else
                return Convert.ToInt64(this.Session["AssetTypeID"]);
        }
        set
        {
            this.Session["AssetTypeID"] = value;
        }
    }

    public Int64? StatusID
    {
        get
        {
            if (Convert.ToInt64(this.Session["StatusID"]) == 0)
                return null;
            else
                return Convert.ToInt64(this.Session["StatusID"]);
        }
        set
        {
            this.Session["StatusID"] = value;
        }
    }

    public String AssetID
    {
        get
        {
            if (Convert.ToString(this.Session["AssetID"]) == String.Empty || Convert.ToString(this.Session["AssetID"]) == "0")
                return null;
            else
                return Convert.ToString(this.Session["AssetID"]);
        }
        set
        {
            this.Session["AssetID"] = value;
        }
    }

    public String Keyword
    {
        get
        {
            return Convert.ToString(this.Session["Keyword"]);
        }
        set
        {
            this.Session["Keyword"] = value;
        }
    }

    public String AssetsPagingStatus
    {
        get
        {
            if (Session["AssetsPagingStatus"] == null)
                return "VIEW ALL";
            return Convert.ToString(Session["AssetsPagingStatus"]);
        }
        set
        {
            Session["AssetsPagingStatus"] = value;
        }
    }

    public String AssetsSortExp
    {
        get
        {
            return Convert.ToString(Session["AssetsSortExp"]);
        }
        set
        {
            Session["AssetsSortExp"] = value;
        }
    }
    public String AssetsSortOrder
    {
        get
        {
            return Convert.ToString(Session["AssetsSortOrder"]);
        }
        set
        {
            Session["AssetsSortOrder"] = value;
        }
    }

    private Int32 PagerSize
    {
        get
        {
            if (Convert.ToString(this.ViewState["PagerSize"]) == "")
                return Convert.ToInt32(Application["NEWPAGESIZE"]);
            else
                return Convert.ToInt32(this.ViewState["PagerSize"]);
        }
    }

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindSearchDropDowns();
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["se"])))
            {
                if (Session["BaseStationID"] != null || Session["AssetTypeID"] != null || !String.IsNullOrEmpty(Convert.ToString(Session["AssetID"])) || Session["DepartmentID"] != null || Session["StatusID"] != null || !String.IsNullOrEmpty(Convert.ToString(Session["Keyword"])))
                {
                    BindAssetsGrid();
                    ddlBaseStation.SelectedValue = Convert.ToString(this.BaseStationID);
                    ddlAssetType.SelectedValue = Convert.ToString(this.AssetTypeID);
                    ddlAssetID.SelectedValue = Convert.ToString(this.AssetID);
                    ddlDepartments.SelectedValue = Convert.ToString(this.DepartmentID);
                    ddlStatus.SelectedValue = Convert.ToString(this.StatusID);
                    txtSearchGo.Value = Convert.ToString(this.Keyword);
                }
            }
            else
            {
                this.CompanyStore = Convert.ToString(txtCompanyStore.Text.Trim());
                this.DepartmentID = Convert.ToInt64(ddlDepartments.SelectedValue);
                this.AssetTypeID = Convert.ToInt64(ddlAssetType.SelectedValue);
                this.AssetID = Convert.ToString(ddlAssetID.SelectedValue);
                this.BaseStationID = Convert.ToInt64(ddlBaseStation.SelectedValue);
                this.StatusID = Convert.ToInt64(ddlStatus.SelectedValue);

                //Session["AssetsCurrentPage"] = thi;
                //Session["AssetsToPg"] = null;
                //Session["AssetsFrmPg "] = null;

            }

        }
    }

    /// <summary>
    /// Row Command Event of Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAssets_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Sort"))
            {
                if (this.AssetsSortExp == String.Empty)
                {
                    this.AssetsSortExp = e.CommandArgument.ToString();
                    this.AssetsSortOrder = "ASC";
                }
                else
                {
                    if (this.AssetsSortExp == e.CommandArgument.ToString())
                    {
                        if (this.AssetsSortOrder == "ASC")
                            this.AssetsSortOrder = "DESC";
                        else
                            this.AssetsSortOrder = "ASC";
                    }
                    else
                    {
                        this.AssetsSortOrder = "ASC";
                        this.AssetsSortExp = e.CommandArgument.ToString();
                    }
                }
            }
            BindAssetsGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }



    protected void gvAssets_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                LinkButton lnkbtnAssetID = (LinkButton)e.Row.FindControl("lnkbtnAssetID");
                LinkButton lnkbtnEquipmentType = (LinkButton)e.Row.FindControl("lnkbtnEquipmentType");
                LinkButton lnkbtnBaseStation = (LinkButton)e.Row.FindControl("lnkbtnBaseStation");
                LinkButton lnkbtnStatus = (LinkButton)e.Row.FindControl("lnkbtnStatus");
                ScriptManager objCurrentScriptManager = ScriptManager.GetCurrent(this);

                if (lnkbtnAssetID != null)
                {
                    objCurrentScriptManager.RegisterAsyncPostBackControl(lnkbtnAssetID);
                }
                if (lnkbtnEquipmentType != null)
                {
                    objCurrentScriptManager.RegisterAsyncPostBackControl(lnkbtnEquipmentType);
                }
                if (lnkbtnBaseStation != null)
                {
                    objCurrentScriptManager.RegisterAsyncPostBackControl(lnkbtnBaseStation);
                }
                if (lnkbtnStatus != null)
                {
                    objCurrentScriptManager.RegisterAsyncPostBackControl(lnkbtnStatus);

                }
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Search event 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearchAsset_Click(object sender, EventArgs e)
    {
        try
        {
            txtSearchGo.Value = "";
            this.CompanyStore = Convert.ToString(txtCompanyStore.Text.Trim());
            this.DepartmentID = Convert.ToInt64(ddlDepartments.SelectedValue);
            this.AssetTypeID = Convert.ToInt64(ddlAssetType.SelectedValue);
            this.AssetID = Convert.ToString(ddlAssetID.SelectedValue);
            this.BaseStationID = Convert.ToInt64(ddlBaseStation.SelectedValue);
            this.StatusID = Convert.ToInt64(ddlStatus.SelectedValue);
            this.Keyword = txtSearchGo.Value.Trim();
            Session["AssetsCurrentPage"] = null;
            Session["AssetsToPg"] = null;
            Session["AssetsFrmPg "] = null;
            BindAssetsGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            this.Keyword = txtSearchGo.Value.Trim();
            //this.CompanyStore = Convert.ToString(txtCompanyStore.Text.Trim());
            //this.DepartmentID = Convert.ToInt64(ddlDepartments.SelectedValue);
            //this.AssetTypeID = Convert.ToInt64(ddlAssetType.SelectedValue);
            //this.AssetID = Convert.ToString(txtAssetID.Text.Trim());
            //this.BaseStationID = Convert.ToInt64(ddlBaseStation.SelectedValue);
            //this.StatusID = Convert.ToInt64(ddlStatus.SelectedValue);
            BindAssetsGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnDownload_Click(object sender, EventArgs e)
    {
        //try
        //{
        String path = Server.MapPath("~/UploadedImages/AssetManagement/ExcelTemplate/Bulk_Upload_Asset_Sample.xlsx");
        //String path = Server.MapPath("~/UploadedImages/employeePhoto/empphoto_101201112234708.jpg");
        System.IO.FileInfo file = new System.IO.FileInfo(path);
        if (file.Exists)
        {
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.End();

        }
        else
        {
            Response.Write("This file does not exist.");
        }
        //}
        //catch (Exception ex)
        //{
        //   // ErrHandler.WriteError(ex);
        //}
    }

    protected void lnkbtnUploadBulkAsset_Click(object sender, EventArgs e)
    {
        try
        {
            //HttpFileCollection hfc = Request.Files;
            //for (int i = 0; i < hfc.Count; i++)
            //{
            //    HttpPostedFile hpf = hfc[i];
            //    if (hpf.ContentLength > 0)
            //    {
            //        string strFileType = Common.GetFileType(hpf.ContentType);
            //    }
            //}

            if (fuBulkAssetFile.HasFile)
            {
                String strFolderPath = Server.MapPath("../../UploadedImages/AssetManagement/Temp");
                Common.SaveDocument(strFolderPath, fuBulkAssetFile.FileName, true, fuBulkAssetFile.PostedFile);
                String strFileName = strFolderPath + @"\" + fuBulkAssetFile.FileName;

                AssetBulkUpload blkObj = new AssetBulkUpload();

                blkObj.SetAsset(strFileName, Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
                this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "GeneralAlertMsg('Bulk Assets uploaded successfully.');", true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlBaseStation_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<GetAssetListForSearchDropDownsResult> objDropDownsList = GetMasterList(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), null, Convert.ToInt64(ddlBaseStation.SelectedValue), null, null);

        //Bind Asset Type
        objDropDownsList = objDropDownsList.OrderBy(p => p.EquipmentType).ToList();
        BindDropDownWithSelectedValue(ddlAssetType, objDropDownsList.Select(x => new { x.EquipmentTypeID, x.EquipmentType }).Distinct().ToList(), "EquipmentType", "EquipmentTypeID", "- Asset Type -", ddlAssetType.SelectedValue);

        //Bind Asset ID
        objDropDownsList = objDropDownsList.OrderBy(p => p.EquipmentID).ToList();
        BindDropDownWithSelectedValue(ddlAssetID, objDropDownsList.Select(x => new { x.EquipmentID }).Distinct().ToList(), "EquipmentID", "EquipmentID", "- Asset ID -", ddlAssetID.SelectedValue);

        //Bind Status
        objDropDownsList = objDropDownsList.OrderBy(p => p.AssetStatus).ToList();
        BindDropDownWithSelectedValue(ddlStatus, objDropDownsList.Select(x => new { x.AssetStatusID, x.AssetStatus }).Distinct().ToList(), "AssetStatus", "AssetStatusID", "- Status -", ddlStatus.SelectedValue);

    }

    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<GetAssetListForSearchDropDownsResult> objDropDownsList = GetMasterList(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), Convert.ToInt64(ddlAssetType.SelectedValue), Convert.ToInt64(ddlBaseStation.SelectedValue), null, null);

        ////Bind BaseStation
        //objDropDownsList = objDropDownsList.OrderBy(p => p.BaseStation).ToList();
        //BindDropDownWithSelectedValue(ddlBaseStation, objDropDownsList.Select(x => new { x.BaseStationID, x.BaseStation }).Distinct().ToList(), "BaseStation", "BaseStationID", "- Station -", ddlBaseStation.SelectedValue);

        //Bind Asset ID
        objDropDownsList = objDropDownsList.OrderBy(p => p.EquipmentID).ToList();
        BindDropDownWithSelectedValue(ddlAssetID, objDropDownsList.Select(x => new { x.EquipmentID }).Distinct().ToList(), "EquipmentID", "EquipmentID", "- Asset ID -", ddlAssetID.SelectedValue);

        //Bind Status
        objDropDownsList = objDropDownsList.OrderBy(p => p.AssetStatus).ToList();
        BindDropDownWithSelectedValue(ddlStatus, objDropDownsList.Select(x => new { x.AssetStatusID, x.AssetStatus }).Distinct().ToList(), "AssetStatus", "AssetStatusID", "- Status -", ddlStatus.SelectedValue);

    }

    protected void ddlAssetID_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<GetAssetListForSearchDropDownsResult> objDropDownsList = GetMasterList(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), Convert.ToInt64(ddlAssetType.SelectedValue), Convert.ToInt64(ddlBaseStation.SelectedValue), ddlAssetID.SelectedValue, null);

        ////Bind BaseStation
        //objDropDownsList = objDropDownsList.OrderBy(p => p.BaseStation).ToList();
        //BindDropDownWithSelectedValue(ddlBaseStation, objDropDownsList.Select(x => new { x.BaseStationID, x.BaseStation }).Distinct().ToList(), "BaseStation", "BaseStationID", "- Station -", ddlBaseStation.SelectedValue);

        ////Bind Asset Type
        //objDropDownsList = objDropDownsList.OrderBy(p => p.EquipmentType).ToList();
        //BindDropDownWithSelectedValue(ddlAssetType, objDropDownsList.Select(x => new { x.EquipmentTypeID, x.EquipmentType }).Distinct().ToList(), "EquipmentType", "EquipmentTypeID", "- Asset Type -", ddlAssetType.SelectedValue);

        //Bind Status
        objDropDownsList = objDropDownsList.OrderBy(p => p.AssetStatus).ToList();
        BindDropDownWithSelectedValue(ddlStatus, objDropDownsList.Select(x => new { x.AssetStatusID, x.AssetStatus }).Distinct().ToList(), "AssetStatus", "AssetStatusID", "- Status -", ddlStatus.SelectedValue);

    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<GetAssetListForSearchDropDownsResult> objDropDownsList = GetMasterList(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), null, null, null, Convert.ToInt64(ddlStatus.SelectedValue));

        //Bind BaseStation
        objDropDownsList = objDropDownsList.OrderBy(p => p.BaseStation).ToList();
        BindDropDownWithSelectedValue(ddlBaseStation, objDropDownsList.Select(x => new { x.BaseStationID, x.BaseStation }).Distinct().ToList(), "BaseStation", "BaseStationID", "- Station -", ddlBaseStation.SelectedValue);

        //Bind Asset Type
        objDropDownsList = objDropDownsList.OrderBy(p => p.EquipmentType).ToList();
        BindDropDownWithSelectedValue(ddlAssetType, objDropDownsList.Select(x => new { x.EquipmentTypeID, x.EquipmentType }).Distinct().ToList(), "EquipmentType", "EquipmentTypeID", "- Asset Type -", ddlAssetType.SelectedValue);


        //Bind Asset ID
        objDropDownsList = objDropDownsList.OrderBy(p => p.EquipmentID).ToList();
        BindDropDownWithSelectedValue(ddlAssetID, objDropDownsList.Select(x => new { x.EquipmentID }).Distinct().ToList(), "EquipmentID", "EquipmentID", "- Asset ID -", ddlAssetID.SelectedValue);



    }

    protected void lnkViewAll_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkView = (LinkButton)sender;
            if (lnkView.Text.Trim().ToUpper() == "VIEW ALL")
            {
                lnkView.Text = "VIEW PAGING";
                AssetsPagingStatus = "VIEW PAGING";
            }
            else
            {
                lnkView.Text = "VIEW ALL";
                AssetsPagingStatus = "VIEW ALL";
            }
            BindAssetsGrid();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods

    protected void BindAssetsGrid()
    {

        try
        {
            DataView myDataView = new DataView();

            DataTable dt = new DataTable();
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            UserInformation objuser = new UserInformation();
            UserInformationRepository objuserrepos = new UserInformationRepository();

            dt = Common.ListToDataTable(objAssetMgtRepository.GetAssetsList(this.AssetTypeID, this.AssetID, this.BaseStationID, this.StatusID, this.DepartmentID, Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), this.Keyword));
            if (dt.Rows.Count == 0)
            {
                //dvTotalRecords.Visible = false;
                pagingtable.Visible = false;
                //btnDelete.Visible = false;
                totalcount_em.Visible = true;
                totalcount_em.InnerText = "0 Result";
            }
            else
            {

                //dvTotalRecords.Visible = true;
                pagingtable.Visible = true;
                //btnDelete.Visible = true;
                totalcount_em.Visible = true;
                totalcount_em.InnerText = dt.Rows.Count + " Result" + (dt.Rows.Count > 1 ? "s" : "");
            }

            myDataView = dt.DefaultView;
            if (this.AssetsSortExp != string.Empty)
            {
                myDataView.Sort = this.AssetsSortExp + " " + this.AssetsSortOrder;
            }

            pds.DataSource = myDataView;
            if (AssetsPagingStatus.ToUpper() == "VIEW ALL")
                pds.AllowPaging = true;
            else
                pds.AllowPaging = false;
            //Page Size has been set as per the ken's Request on 12th August 2013
            pds.PageSize = this.PagerSize;
            pds.CurrentPageIndex = AssetsCurrentPage;
            lnkbtnNext.Visible = !pds.IsLastPage;
            lnkbtnPrevious.Visible = !pds.IsFirstPage;

            gvAssests.DataSource = pds;
            gvAssests.DataBind();
            doPaging();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected List<GetAssetListForSearchDropDownsResult> GetMasterList(Int64 CompanyID, Int64? AssetType, Int64? BaseStationID, String AssetID, Int64? StatusID)
    {
        AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
        List<GetAssetListForSearchDropDownsResult> objDropDownsList = objAssetMgtRepository.GetSearchDropDownsLists(CompanyID, AssetType, BaseStationID, AssetID, StatusID);
        return objDropDownsList;
    }

    protected void BindSearchDropDowns()
    {
        try
        {
            LookupRepository objLookupRepository = new LookupRepository();
            List<INC_Lookup> objList = new List<INC_Lookup>();
            String LookUpCode = string.Empty;

            //Bind Departments
            LookUpCode = "GSEDepartment";
            objList = objLookupRepository.GetByLookup(LookUpCode);
            objList = objList.OrderBy(p => p.sLookupName).ToList();
            Common.BindDropDown(ddlDepartments, objList, "sLookupName", "iLookupID", "- Department -");

            List<GetAssetListForSearchDropDownsResult> objDropDownsList = GetMasterList(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), null, null, null, null);

            //Bind BaseStation
            objDropDownsList = objDropDownsList.OrderBy(p => p.BaseStation).ToList();
            BindDropDownWithSelectedValue(ddlBaseStation, objDropDownsList.Select(x => new { x.BaseStationID, x.BaseStation }).Distinct().ToList(), "BaseStation", "BaseStationID", "- Station -", "0");

            //Bind Asset Type
            objDropDownsList = objDropDownsList.OrderBy(p => p.EquipmentType).ToList();
            BindDropDownWithSelectedValue(ddlAssetType, objDropDownsList.Select(x => new { x.EquipmentTypeID, x.EquipmentType }).Distinct().ToList(), "EquipmentType", "EquipmentTypeID", "- Asset Type -", "0");

            //Bind Asset ID
            objDropDownsList = objDropDownsList.OrderBy(p => p.EquipmentID).ToList();
            BindDropDownWithSelectedValue(ddlAssetID, objDropDownsList.Select(x => new { x.EquipmentID }).Distinct().ToList(), "EquipmentID", "EquipmentID", "- Asset ID -", "0");

            //Bind Status
            objDropDownsList = objDropDownsList.OrderBy(p => p.AssetStatus).ToList();
            BindDropDownWithSelectedValue(ddlStatus, objDropDownsList.Select(x => new { x.AssetStatusID, x.AssetStatus }).Distinct().ToList(), "AssetStatus", "AssetStatusID", "- Status -", "0");

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Bind dropdown and add onchange attribute
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ddl"></param>
    /// <param name="list"></param>
    /// <param name="DataTextField"></param>
    /// <param name="DataValueField"></param>
    /// <param name="FirstListItem"></param>
    public static void BindDropDownWithSelectedValue<T>(DropDownList ddl, List<T> list, string DataTextField, string DataValueField, string FirstListItem, string SelectedValue) where T : class
    {
        ddl.Items.Clear();
        ddl.ClearSelection();
        if (list.Count > 0)
        {
            ddl.DataSource = list;
            ddl.DataTextField = DataTextField;
            ddl.DataValueField = DataValueField;
            ddl.DataBind();
        }
        if (!string.IsNullOrEmpty(FirstListItem))
            ddl.Items.Insert(0, new ListItem(FirstListItem, "0"));

        if (ddl.Items.FindByValue(SelectedValue) != null)
            ddl.SelectedValue = SelectedValue;
        else
            ddl.SelectedValue = "0";
    }

    #endregion

    #region Paging in the Assets GridView

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            AssetsCurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindAssetsGrid();
        }


    }

    protected void dtlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(AssetsCurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    public int AssetsCurrentPage
    {
        get
        {
            if (this.Session["AssetsCurrentPage"] == null)
                return 0;
            else
                return Convert.ToInt16(this.Session["AssetsCurrentPage"].ToString());
        }
        set
        {
            this.Session["AssetsCurrentPage"] = value;
        }
    }

    public int AssetsFrmPg
    {
        get
        {
            if (this.Session["AssetsFrmPg"] == null)
                return 1;
            else
                return Convert.ToInt16(this.Session["AssetsFrmPg"].ToString());
        }
        set
        {
            this.Session["AssetsFrmPg"] = value;
        }
    }

    public int AssetsToPg
    {
        get
        {
            if (this.Session["AssetsToPg"] == null)
                return 5;
            else
                return Convert.ToInt16(this.Session["AssetsToPg"].ToString());
        }
        set
        {
            this.Session["AssetsToPg"] = value;
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

            if (CurrentPg > AssetsToPg)
            {
                AssetsFrmPg = AssetsToPg + 1;
                AssetsToPg = AssetsToPg + 5;
            }
            if (CurrentPg < AssetsFrmPg)
            {
                AssetsToPg = AssetsFrmPg - 1;
                AssetsFrmPg = AssetsFrmPg - 5;

            }

            if (pds.PageCount < AssetsToPg)
                AssetsToPg = pds.PageCount;

            for (int i = AssetsFrmPg - 1; i < AssetsToPg; i++)
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
        AssetsCurrentPage -= 1;
        BindAssetsGrid();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        AssetsCurrentPage += 1;
        BindAssetsGrid();
    }

    #endregion
}
