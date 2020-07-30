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
using System.Globalization;

public partial class Admin_IssuanceActivated : PageBase
{

    #region DataMembers
    PagedDataSource pds = new PagedDataSource();    
    #endregion

    #region Page Level Variables
    string CurrModule = "Marketing Services";
    string CurrSubMenu = "Issuance Activated";
    #endregion

    #region Properties & Fields

    public Int64? CompanyID
    {
        get
        {
            if (this.ViewState["CompanyID"] == null || Convert.ToString(this.ViewState["CompanyID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["CompanyID"]);
        }
        set
        {
            this.ViewState["CompanyID"] = value;
        }
    }

    public Int64? WorkgroupID
    {
        get
        {
            if (this.ViewState["WorkgroupID"] == null || Convert.ToString(this.ViewState["WorkgroupID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["WorkgroupID"]);
        }
        set
        {
            this.ViewState["WorkgroupID"] = value;
        }
    }        

    public String FromDate
    {
        get
        {
            if (Convert.ToString(this.ViewState["FromDate"]) == string.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["FromDate"]);
        }
        set
        {
            this.ViewState["FromDate"] = value;
        }
    }

    public String ToDate
    {
        get
        {
            if (Convert.ToString(this.ViewState["ToDate"]) == string.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["ToDate"]);
        }
        set
        {
            this.ViewState["ToDate"] = value;
        }
    }    

    public Int16? DateRange
    {
        get
        {
            if (Convert.ToInt16(this.ViewState["DateRange"]) == 0)
                return null;
            else
                return Convert.ToInt16(this.ViewState["DateRange"]);
        }
        set
        {
            this.ViewState["DateRange"] = value;
        }
    }

    public String KeyWord
    {
        get
        {
            return Convert.ToString(this.ViewState["KeyWord"]);
        }
        set
        {
            this.ViewState["KeyWord"] = value;
        }
    }

    public String CurrentPagingStatus
    {
        get
        {
            if (Session["CurrentPagingStatus"] == null)
                return "VIEW ALL";
            return Convert.ToString(Session["CurrentPagingStatus"]);
        }
        set
        {
            Session["CurrentPagingStatus"] = value;
        }
    }

    public String FilterSortExp
    {
        get
        {
            return Convert.ToString(Session["FilterSortExp"]);
        }
        set
        {
            Session["FilterSortExp"] = value;
        }
    }
    public String FilterSortOrder
    {
        get
        {
            return Convert.ToString(Session["FilterSortOrder"]);
        }
        set
        {
            Session["FilterSortOrder"] = value;
        }
    }

    public Int32 NoOfRecordsToDisplay
    {
        get
        {
            if (Convert.ToString(this.ViewState["NoOfRecordsToDisplay"]) == "")
                return Convert.ToInt32(Application["NEWPAGESIZE"]);
            else
                return Convert.ToInt32(this.ViewState["NoOfRecordsToDisplay"]);
        }
        set
        {
            this.ViewState["NoOfRecordsToDisplay"] = value;
        }
    }

    public Int32 PageIndex
    {
        get
        {
            if (Convert.ToString(this.ViewState["PageIndex"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["PageIndex"]);
        }
        set
        {
            this.ViewState["PageIndex"] = value;
        }
    }

    public String SortColumn
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortColumn"]) == String.Empty)
                return "ActivationDate";
            else
                return Convert.ToString(this.ViewState["SortColumn"]);
        }
        set
        {
            this.ViewState["SortColumn"] = value;
        }
    }

    public String SortDirection
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortDirection"]) == String.Empty)
                return "ASC";
            else
                return Convert.ToString(this.ViewState["SortDirection"]);
        }
        set
        {
            this.ViewState["SortDirection"] = value;
        }
    }

    public Int32 TotalPages
    {
        get
        {
            if (Convert.ToString(this.ViewState["TotalPages"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["TotalPages"]);
        }
        set
        {
            this.ViewState["TotalPages"] = value;
        }
    }

    public Int32 NoOfPagesToDisplay
    {
        get
        {
            if (Convert.ToString(this.ViewState["NoOfPagesToDisplay"]) == "")
                return Convert.ToInt32(Application["NEWPAGERSIZE"]);
            else
                return Convert.ToInt32(this.ViewState["NoOfPagesToDisplay"]);
        }
        set
        {
            this.ViewState["NoOfPagesToDisplay"] = value;
        }
    }

    public Int32 FromPage
    {
        get
        {
            if (Convert.ToString(this.ViewState["FromPage"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["FromPage"]);
        }
        set
        {
            this.ViewState["FromPage"] = value;
        }
    }

    public Int32 ToPage
    {
        get
        {
            if (Convert.ToString(this.ViewState["ToPage"]) == "")
                return this.NoOfPagesToDisplay;
            else
                return Convert.ToInt32(this.ViewState["ToPage"]);
        }
        set
        {
            this.ViewState["ToPage"] = value;
        }
    }    
    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                base.StartStopwatch();

                BindSearchDropDowns();

                base.EndStopwatch("Page Load", CurrModule, CurrSubMenu);
            }
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
    protected void gvIssuanceActivated_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Sort")
            {
                if (this.SortColumn == null)
                {
                    this.SortColumn = Convert.ToString(e.CommandArgument);
                    this.SortDirection = "ASC";
                }
                else
                {
                    if (this.SortColumn == Convert.ToString(e.CommandArgument))
                    {
                        if (this.SortDirection == "ASC")
                            this.SortDirection = "DESC";
                        else
                            this.SortDirection = "ASC";
                    }
                    else
                    {
                        this.SortDirection = "ASC";
                        this.SortColumn = Convert.ToString(e.CommandArgument);
                    }
                }

                BindIssuanceActivatedGrid(true);
            }
            else if (e.CommandName == "IssuanceProgramName")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('system-data-popup', 'warranty-content');", true);
            }
            else if (e.CommandName == "FirstName")
            {
                Response.Redirect("~/NewDesign/UserPages/index.aspx?id=true&subid=" + e.CommandArgument.ToString().Split('-')[0] + "&storeid=" + e.CommandArgument.ToString().Split('-')[1]);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('system-data-popup', 'warranty-content');", true);   
                //Response.Redirect("~/index.aspx?id=true&subid=" + e.CommandArgument.ToString().Split('-')[0] + "&storeid=" + e.CommandArgument.ToString().Split('-')[1], true);
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
    protected void btnSearchIssuanceData_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            this.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            this.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            if (ddlDate.SelectedValue == "0")
            {
                this.FromDate = this.ToDate = string.Empty;
            }
            else if (ddlDate.SelectedValue == "1")
            {
                this.FromDate = this.ToDate = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else if (ddlDate.SelectedValue == "2")
            {
                this.FromDate = this.ToDate = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy");
            }
            else if (ddlDate.SelectedValue == "3")
            {
                this.FromDate = DateTime.Now.AddDays(-7).ToString("MM/dd/yyyy");
                this.ToDate = DateTime.Now.ToString("MM/dd/yyyy");                
            }
            else if (ddlDate.SelectedValue == "4")
            {
                this.FromDate = txtFromDate.Text;
                this.ToDate = txtToDate.Text;
            }
            

            this.KeyWord = String.Empty;
            this.NoOfRecordsToDisplay = Convert.ToInt32(Application["NEWPAGESIZE"]);
            this.PageIndex = 1;

            btnSearchGrid.Enabled = true;
            btnSearchGrid.CssClass += " postback";
            txtSearchGrid.Text = String.Empty;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                this.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);

            BindIssuanceActivatedGrid(false);

            base.EndStopwatch("Search", CurrModule, CurrSubMenu);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnSearchGrid_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            this.KeyWord = txtSearchGrid.Text;
            BindIssuanceActivatedGrid(false);

            base.EndStopwatch("Search by Keyword", CurrModule, CurrSubMenu);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkPrevious_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex -= 1;
            BindIssuanceActivatedGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkNext_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex += 1;
            BindIssuanceActivatedGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }


    protected void lnkViewAllSD_Click(object sender, EventArgs e)
    {
        try
        {
            //this.PageIndexSD = 1;
            //this.NoOfRecordsToDisplaySD = 0;
            BindSystemDataGrid(false);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('system-data-popup', 'warranty-content');", true);   
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkPreviousSD_Click(Object sender, EventArgs e)
    {
        try
        {
            //this.PageIndexSD -= 1;
            BindSystemDataGrid(false);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('system-data-popup', 'warranty-content');", true);   
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkNextSD_Click(Object sender, EventArgs e)
    {
        try
        {
            //this.PageIndexSD += 1;
            BindSystemDataGrid(false);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('system-data-popup', 'warranty-content');", true);   
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }


    protected void lnkViewAll_Click(object sender, EventArgs e)
    {
        try
        {
            this.PageIndex = 1;
            this.NoOfRecordsToDisplay = 0;
            BindIssuanceActivatedGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods
    
    protected void BindIssuanceActivatedGrid(Boolean isForSorting)
    {
        try
        {
            IssuanceActivatedRepository ObjSARepo = new IssuanceActivatedRepository();
            List<GetIssuancePolicyDetailsResult> lstSAResult = ObjSARepo.GetIssuancePolicyDetails(this.CompanyID, this.WorkgroupID, "A", this.FromDate, this.ToDate, this.KeyWord, this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);

            if (lstSAResult != null && lstSAResult.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstSAResult[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;

                lstSAResult = lstSAResult.Select(x => new GetIssuancePolicyDetailsResult
                {
                    TotalRecords = x.TotalRecords,
                    FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.FirstName.Split(' ')[0]) + " " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.FirstName.Split(' ')[1]),
                    IssuanceProgramName = x.IssuanceProgramName,
                    UserInfoID = x.UserInfoID,
                    CompanyId = x.CompanyId,
                    ExpirationDate = x.ExpirationDate,
                    EligibleDate = x.EligibleDate,
                    WorkgroupID = x.WorkgroupID,                    
                    UniformIssuancePolicyID = x.UniformIssuancePolicyID,
                    RowNum = x.RowNum
                }).ToList();
            }


            gvIssuanceActivated.DataSource = lstSAResult;
            gvIssuanceActivated.DataBind();

            if (!isForSorting)
            {
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gvIssuanceActivated.Rows.Count > 0)
            {
                totalcount_em.InnerText = lstSAResult[0].TotalRecords + " results";
                pagingtable.Visible = true;
            }
            else
            {
                totalcount_em.InnerText = "no results";
                pagingtable.Visible = false;
            }

            totalcount_em.Visible = true;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindSystemDataGrid(Boolean isForSorting)
    {
        try
        {            
            //SystemAccessRepository objRepo = new SystemAccessRepository();
            //List<GetSystemAccessSubDetailsResult> ObjList = objRepo.SearchSystemData(this.UserIdForSystemData, this.FromDate, this.ToDate, this.NoOfRecordsToDisplaySD, this.PageIndexSD, this.SortColumnSD, this.SortDirectionSD);

            //if (ObjList != null && ObjList.Count > 0)
            //{
            //    if (this.NoOfRecordsToDisplaySD > 0)
            //        this.TotalPagesSD = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(ObjList[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplaySD)));
            //    else
            //        this.TotalPagesSD = 1;
            //}
            

            //gvSystemData.DataSource = ObjList;
            //gvSystemData.DataBind();

            //decimal TotHours = Convert.ToDecimal(ObjList[0].AverageLoginTime.Split(':')[0]);
            //decimal TotMins = Convert.ToDecimal(ObjList[0].AverageLoginTime.Split(':')[1]);
            //decimal AvgTime = ((TotHours * 60) + TotMins) / Convert.ToDecimal(ObjList[0].TotalRecords);
            //TimeSpan Total=TimeSpan.FromMinutes(Convert.ToDouble(AvgTime));
            //lblAverageTime.Text = Total.Hours + ":" + Total.Minutes;

            //if (!isForSorting)
            //{
            //    GeneratePagingSD();
            //    //lnkPreviousSD.Enabled = this.PageIndexSD > 1;
            //    //lnkNextSD.Enabled = this.PageIndexSD < this.TotalPagesSD;
            //}

            //if (gvSystemData.Rows.Count > 0)
            //{
            //    //totalcount_em.InnerText = lstSAResult[0].TotalRecords + " results";
            //    pagingTableSD.Visible = true;
            //}
            //else
            //{
            //    //totalcount_em.InnerText = "no results";
            //    pagingTableSD.Visible = false;
            //}

            //totalcount_em.Visible = true;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindSearchDropDowns()
    {
        try
        {
            IncentexBEDataContext db = new IncentexBEDataContext();

            //Bind Company Names
            CompanyRepository objCompanyRepository = new CompanyRepository();
            List<Company> objCompanyList = new List<Company>();

            objCompanyList = objCompanyRepository.GetCompaniesWithIssuance();
            Common.BindDropDown(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "- Company -");            

            LookupRepository objLookupRepository = new LookupRepository();
            List<INC_Lookup> objList = new List<INC_Lookup>();
            String LookUpCode = string.Empty;

            //Bind Workgroups            
            objList = objLookupRepository.GetWorkgroupWithIssuance();
            Common.BindDropDown(ddlWorkgroup, objList, "sLookupName", "iLookupID", "- Workgroup -");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion

    #region Paging in the System Access GridView

    private void GeneratePaging()
    {
        try
        {
            Int32 tCurrentPg = this.PageIndex;
            Int32 tToPg = this.ToPage;
            Int32 tFrmPg = this.FromPage;

            if (tCurrentPg > tToPg)
            {
                tFrmPg = tToPg + 1;
                tToPg += this.NoOfPagesToDisplay;
            }

            if (tCurrentPg < tFrmPg)
            {
                tToPg = tFrmPg - 1;
                tFrmPg -= this.NoOfPagesToDisplay;
            }

            //if (this.TotalPages > tToPg)
            //    tToPg = this.TotalPages;

            if (tToPg > this.TotalPages)
                tToPg = this.TotalPages;

            this.ToPage = tToPg;
            this.FromPage = tFrmPg;

            DataTable dt = new DataTable();
            dt.Columns.Add("Index");

            for (Int32 i = tFrmPg; i <= tToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Index"] = i;
                dt.Rows.Add(dr);
            }

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }    

    protected void dtlPagingSD_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ChangePage")
            {
                //this.PageIndexSD = Convert.ToInt32(e.CommandArgument);
                BindSystemDataGrid(false);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('system-data-popup', 'warranty-content');", true);   
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ChangePage")
            {
                this.PageIndex = Convert.ToInt32(e.CommandArgument);
                BindIssuanceActivatedGrid(false);                
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
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
            if (this.ViewState["AssetsCurrentPage"] == null)
                return 0;
            else
                return Convert.ToInt16(this.ViewState["AssetsCurrentPage"].ToString());
        }
        set
        {
            this.ViewState["AssetsCurrentPage"] = value;
        }
    }

    public int AssetsFrmPg
    {
        get
        {
            if (this.ViewState["AssetsFrmPg"] == null)
                return 1;
            else
                return Convert.ToInt16(this.ViewState["AssetsFrmPg"].ToString());
        }
        set
        {
            this.ViewState["AssetsFrmPg"] = value;
        }
    }

    public int AssetsToPg
    {
        get
        {
            if (this.ViewState["AssetsToPg"] == null)
                return 5;
            else
                return Convert.ToInt16(this.ViewState["AssetsToPg"].ToString());
        }
        set
        {
            this.ViewState["AssetsToPg"] = value;
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
    #endregion    
}
