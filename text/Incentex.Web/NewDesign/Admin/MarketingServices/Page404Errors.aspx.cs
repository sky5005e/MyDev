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

public partial class Admin_Page404Errors : PageBase
{

    #region DataMembers
    PagedDataSource pds = new PagedDataSource();
    System.Diagnostics.Stopwatch mysw = new System.Diagnostics.Stopwatch();
    #endregion

    #region Page Level Variables
    string CurrModule = "Marketing Services";
    string CurrSubMenu = "404 Errors";
    #endregion

    #region Properties & Fields
    
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

    public String ModuleOfSystem
    {
        get
        {
            if (Convert.ToString(this.ViewState["ModuleOfSystem"]) == string.Empty || Convert.ToString(this.ViewState["ModuleOfSystem"]) == "- Module -")
            {
                return null;
            }
            else
            {
                return Convert.ToString(this.ViewState["ModuleOfSystem"]).Replace(" ", "");
            }
        }
        set
        {
            this.ViewState["ModuleOfSystem"] = value;
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

    public Int64? UserID
    {
        get
        {
            if (this.ViewState["UserID"] == null || Convert.ToString(this.ViewState["UserID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["UserID"]);
        }
        set
        {
            this.ViewState["UserID"] = value;
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
                return "FirstName";
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

    public Int32? PageErrorID
    {
        get
        {
            if (Convert.ToString(this.ViewState["PageErrorID"]) != "")
                return Convert.ToInt32(this.ViewState["PageErrorID"]);
            else
                return null;
        }
        set
        {
            this.ViewState["PageErrorID"] = value;
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
    protected void gv404Errors_RowCommand(object sender, GridViewCommandEventArgs e)
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

                Bind404ErrorPageGrid(true);
            }
            else if (e.CommandName == "ShowNotes")
            {
                this.PageErrorID = Convert.ToInt32(e.CommandArgument);
                BindNotesGrid(false);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('system-data-popup', 'warranty-content');", true);   
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gv404Errors_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label PageErrorLabel = e.Row.FindControl("lblPageWithError") as Label;
                String ErrorPage = PageErrorLabel.Text;
                if (!string.IsNullOrEmpty(ErrorPage))
                {
                    PageErrorLabel.Text = ErrorPage.Split('/')[ErrorPage.Split('/').Length - 1];
                    PageErrorLabel.ToolTip = ErrorPage;
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
    protected void btnSearchErrorDate_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            this.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            this.UserID = Convert.ToInt64(ddlUser.SelectedValue);
            this.ModuleOfSystem = ddlModuleName.SelectedItem.Text;

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

            Bind404ErrorPageGrid(false);

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
            Bind404ErrorPageGrid(false);

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
            Bind404ErrorPageGrid(false);
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
            Bind404ErrorPageGrid(false);
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
            Bind404ErrorPageGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods
    
    protected void Bind404ErrorPageGrid(Boolean isForSorting)
    {
        try
        {
            Page404ErrorRepository ObjErrRepo = new Page404ErrorRepository();
            List<Get404ErrorDetailsResult> lstErResult = ObjErrRepo.GetErrorDetails(this.CompanyID, this.UserID, this.ModuleOfSystem, this.FromDate, this.ToDate, this.KeyWord, this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);

            if (lstErResult != null && lstErResult.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstErResult[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;

                lstErResult = lstErResult.Select(x => new Get404ErrorDetailsResult
                {
                    TotalRecords = x.TotalRecords.Value,                    
                    CustomerName = x.CustomerName,
                    ErrorID = x.ErrorID,
                    ErrorDate = x.ErrorDate.Value,
                    CompanyName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.CompanyName),
                    ErrorPageName = x.ErrorPageName,
                    Browser = x.Browser,
                    OS = x.OS,
                    Resolution = x.Resolution,
                    RowNum = x.RowNum.Value
                }).ToList();
            }


            gv404Errors.DataSource = lstErResult;
            gv404Errors.DataBind();

            if (!isForSorting)
            {
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gv404Errors.Rows.Count > 0)
            {
                totalcount_em.InnerText = lstErResult[0].TotalRecords + " results";
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

    protected void BindNotesGrid(Boolean isForSorting)
    {
        try
        {            
            Page404ErrorRepository objRepo = new Page404ErrorRepository();
            List<Get404ErrorNotesResult> ObjList = objRepo.GetNotesForError(this.PageErrorID);

            if (ObjList != null && ObjList.Count > 0)
            {
                //if (this.NoOfRecordsToDisplaySD > 0)
                //    this.TotalPagesSD = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(ObjList[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplaySD)));
                //else
                //    this.TotalPagesSD = 1;
            }

            gvDetails.DataSource = ObjList;
            gvDetails.DataBind();

            //gvSystemData.DataSource = ObjList;
            //gvSystemData.DataBind();

            //decimal TotHours = Convert.ToDecimal(ObjList[0].AverageLoginTime.Split(':')[0]);
            //decimal TotMins = Convert.ToDecimal(ObjList[0].AverageLoginTime.Split(':')[1]);
            //decimal AvgTime = ((TotHours * 60) + TotMins) / Convert.ToDecimal(ObjList[0].TotalRecords);
            //TimeSpan Total = TimeSpan.FromMinutes(Convert.ToDouble(AvgTime));
            //lblAverageTime.Text = Total.Hours + ":" + Total.Minutes;

            //if (!isForSorting)
            //{
            //    GeneratePagingSD();
            //    lnkPreviousSD.Enabled = this.PageIndexSD > 1;
            //    lnkNextSD.Enabled = this.PageIndexSD < this.TotalPagesSD;
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

            objCompanyList = objCompanyRepository.GetAllCompany();
            Common.BindDropDown(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "- Company -");

            LookupRepository objLookupRepository = new LookupRepository();
            List<INC_Lookup> objList = new List<INC_Lookup>();
            String LookUpCode = string.Empty;                        

            //Bind User Names
            var objUsers = (from usr in db.UserInformations where usr.IsDeleted == false select new { FirstName = usr.FirstName + " " + usr.LastName, usr.UserInfoID }).OrderBy(x => x.FirstName).ToList();
            ddlUser.DataSource = objUsers;
            ddlUser.DataTextField = "FirstName";
            ddlUser.DataValueField = "UserInfoID";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("- Customer -", "0"));

            //Bind Modules
            PageLoadRepository PageLoadRepo = new PageLoadRepository();
            List<MediaHelpVidPrnt> objModule = PageLoadRepo.GetModules();
            Common.BindDropDown(ddlModuleName, objModule, "ModuleName", "MediaHelpVidPrntId", "- Module -");
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
    

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ChangePage")
            {
                this.PageIndex = Convert.ToInt32(e.CommandArgument);
                Bind404ErrorPageGrid(false);                
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion
}
