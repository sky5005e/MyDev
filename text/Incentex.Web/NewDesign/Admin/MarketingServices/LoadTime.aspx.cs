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

public partial class Admin_LoadTime : PageBase
{

    #region DataMembers
    PagedDataSource pds = new PagedDataSource();    
    #endregion

    #region Page Level Variables
    string CurrModule = "Marketing Services";
    string CurrSubMenu = "Load Time";
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

    public Int64? UserIDDtl
    {
        get
        {
            if (this.ViewState["UserIDDtl"] == null || Convert.ToString(this.ViewState["UserIDDtl"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["UserIDDtl"]);
        }
        set
        {
            this.ViewState["UserIDDtl"] = value;
        }
    }

    public Int64? LoadTimeFrom
    {
        get
        {
            if (this.ViewState["LoadTimeFrom"] == null)
                return null;
            else
                return Convert.ToInt64(this.ViewState["LoadTimeFrom"]);
        }
        set
        {
            this.ViewState["LoadTimeFrom"] = value;
        }
    }

    public Int64? LoadTimeTo
    {
        get
        {
            if (this.ViewState["LoadTimeTo"] == null)
                return null;
            else
                return Convert.ToInt64(this.ViewState["LoadTimeTo"]);
        }
        set
        {
            this.ViewState["LoadTimeTo"] = value;
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

    public String ModuleName
    {
        get
        {
            if (Convert.ToString(this.ViewState["ModuleName"]) =="- Module -")
                return null;
            else
                return Convert.ToString(this.ViewState["ModuleName"]);
        }
        set
        {
            this.ViewState["ModuleName"] = value;
        }
    }

    public String ModuleNameDtl
    {
        get
        {
            if (Convert.ToString(this.ViewState["ModuleNameDtl"]) == "- Module -")
                return null;
            else
                return Convert.ToString(this.ViewState["ModuleNameDtl"]);
        }
        set
        {
            this.ViewState["ModuleNameDtl"] = value;
        }
    }

    public String SubMenu
    {
        get
        {
            if (Convert.ToString(this.ViewState["SubMenu"]) == "- Sub Menu -")
                return null;
            else
                return Convert.ToString(this.ViewState["SubMenu"]);
        }
        set
        {
            this.ViewState["SubMenu"] = value;
        }
    }

    public String SubMenuDtl
    {
        get
        {
            if (Convert.ToString(this.ViewState["SubMenuDtl"]) == "- Sub Menu -")
                return null;
            else
                return Convert.ToString(this.ViewState["SubMenuDtl"]);
        }
        set
        {
            this.ViewState["SubMenuDtl"] = value;
        }
    }

    public String PageEventName
    {
        get
        {
            if (Convert.ToString(this.ViewState["PageEventName"]) == "- Page Event -")
                return null;
            else
                return Convert.ToString(this.ViewState["PageEventName"]);
        }
        set
        {
            this.ViewState["PageEventName"] = value;
        }
    }

    public String BrowserName
    {
        get
        {
            if (Convert.ToString(this.ViewState["BrowserName"]) == "- Browser -")
                return null;
            else
                return Convert.ToString(this.ViewState["BrowserName"]);
        }
        set
        {
            this.ViewState["BrowserName"] = value;
        }
    }

    public String DeviceName
    {
        get
        {
            if (Convert.ToString(this.ViewState["DeviceName"]) == "- Device -")
                return null;
            else
                return Convert.ToString(this.ViewState["DeviceName"]);
        }
        set
        {
            this.ViewState["DeviceName"] = value;
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

    public Int32 PageIndexDtl
    {
        get
        {
            if (Convert.ToString(this.ViewState["PageIndexDtl"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["PageIndexDtl"]);
        }
        set
        {
            this.ViewState["PageIndexDtl"] = value;
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

    public String SortColumnDtl
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortColumnDtl"]) == String.Empty)
                return "LoadTime";
            else
                return Convert.ToString(this.ViewState["SortColumnDtl"]);
        }
        set
        {
            this.ViewState["SortColumnDtl"] = value;
        }
    }

    public String SortDirection
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortDirection"]) == String.Empty)
                return "DESC";
            else
                return Convert.ToString(this.ViewState["SortDirection"]);
        }
        set
        {
            this.ViewState["SortDirection"] = value;
        }
    }

    public String SortDirectionDtl
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortDirectionDtl"]) == String.Empty)
                return "DESC";
            else
                return Convert.ToString(this.ViewState["SortDirectionDtl"]);
        }
        set
        {
            this.ViewState["SortDirectionDtl"] = value;
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

    public Int32 TotalPagesDtl
    {
        get
        {
            if (Convert.ToString(this.ViewState["TotalPagesDtl"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["TotalPagesDtl"]);
        }
        set
        {
            this.ViewState["TotalPagesDtl"] = value;
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

    public Int32 FromPageDtl
    {
        get
        {
            if (Convert.ToString(this.ViewState["FromPageDtl"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["FromPageDtl"]);
        }
        set
        {
            this.ViewState["FromPageDtl"] = value;
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

    public Int32 ToPageDtl
    {
        get
        {
            if (Convert.ToString(this.ViewState["ToPageDtl"]) == "")
                return this.NoOfPagesToDisplay;
            else
                return Convert.ToInt32(this.ViewState["ToPageDtl"]);
        }
        set
        {
            this.ViewState["ToPageDtl"] = value;
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
    protected void gvLoadTime_RowCommand(object sender, GridViewCommandEventArgs e)
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

                BindLoadTimeGrid(true);
            }
            else if (e.CommandName == "Details")
            {
                lblHisotryTitle.Text = "Details - " + e.CommandArgument.ToString().Split('#')[3];

                this.UserIDDtl = Convert.ToInt64(e.CommandArgument.ToString().Split('#')[0]);
                this.ModuleNameDtl = e.CommandArgument.ToString().Split('#')[1];
                this.SubMenuDtl = e.CommandArgument.ToString().Split('#')[2];

                this.PageIndexDtl = 1;
                this.FromPageDtl = 1;
                this.ToPageDtl = this.NoOfPagesToDisplay;


                BindLoadTimeDetailsGrid(false);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvLTSubDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Sort")
            {
                if (this.SortColumnDtl == null)
                {
                    this.SortColumnDtl = Convert.ToString(e.CommandArgument);
                    this.SortDirectionDtl = "ASC";
                }
                else
                {
                    if (this.SortColumnDtl == Convert.ToString(e.CommandArgument))
                    {
                        if (this.SortDirectionDtl == "ASC")
                            this.SortDirectionDtl = "DESC";
                        else
                            this.SortDirectionDtl = "ASC";
                    }
                    else
                    {
                        this.SortDirectionDtl = "ASC";
                        this.SortColumnDtl = Convert.ToString(e.CommandArgument);
                    }
                }

                BindLoadTimeDetailsGrid(true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvLTSubDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string myStr = (e.Row.Cells[2].FindControl("lblPageName") as Label).Text;
                (e.Row.Cells[2].FindControl("lblPageName") as Label).Text = myStr.Split('/')[myStr.Split('/').Length - 1];
                (e.Row.Cells[2].FindControl("lblPageName") as Label).ToolTip = myStr;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvLoadTime_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    string myStr = (e.Row.Cells[2].FindControl("lblPageName") as Label).Text;
            //    (e.Row.Cells[2].FindControl("lblPageName") as Label).Text = myStr.Split('/')[myStr.Split('/').Length - 1];
            //    (e.Row.Cells[2].FindControl("lblPageName") as Label).ToolTip = myStr;
            //}
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
            this.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            this.ModuleName = ddlModuleName.SelectedItem.Text;
            this.SubMenu = ddlSubMenu.SelectedItem.Text;
            this.BrowserName = ddlBrowser.SelectedItem.Text;
            this.DeviceName = ddlDevice.SelectedItem.Text;
            this.PageEventName = ddlEventName.SelectedItem.Text;

            if (ddlLoadTime.SelectedValue == "0")
            {
                this.LoadTimeFrom = this.LoadTimeTo = null;
            }
            else if (ddlLoadTime.SelectedValue == "10 secs")
            {
                this.LoadTimeFrom = 0;
                this.LoadTimeTo = 10 * 1000; // 10 secs * 1000 (for converting milisecond)
            }
            else if (ddlLoadTime.SelectedValue == "<30 secs")
            {
                this.LoadTimeFrom = 0;
                this.LoadTimeTo = 30 * 1000; // 10 secs * 1000 (for converting milisecond)
            }
            else if (ddlLoadTime.SelectedValue == "30-60 secs")
            {
                this.LoadTimeFrom = 30 * 1000;
                this.LoadTimeTo = 60 * 1000;
            }
            else if (ddlLoadTime.SelectedValue == "1-3 mins")
            {
                this.LoadTimeFrom = 60 * 1000;
                this.LoadTimeTo = 60 * 3 * 1000;
            }
            else if (ddlLoadTime.SelectedValue == "Custom")
            {
                if (txtFormLoadTime.Text != string.Empty && txtToLoadTime.Text != string.Empty)
                {
                    this.LoadTimeFrom = Convert.ToInt64(txtFormLoadTime.Text);
                    this.LoadTimeTo = Convert.ToInt64(txtToLoadTime.Text);
                }
            }

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
            this.FromPage = 1;
            this.ToPage = this.NoOfPagesToDisplay;

            btnSearchGrid.Enabled = true;
            btnSearchGrid.CssClass += " postback";
            txtSearchGrid.Text = String.Empty;

            BindLoadTimeGrid(false);

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
            BindLoadTimeGrid(false);

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
            BindLoadTimeGrid(false);
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
            BindLoadTimeGrid(false);
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
            BindLoadTimeGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkViewAllDtl_Click(object sender, EventArgs e)
    {
        try
        {
            this.PageIndexDtl = 1;
            this.NoOfRecordsToDisplay = 0;
            BindLoadTimeDetailsGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkPreviousDtl_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndexDtl -= 1;
            BindLoadTimeDetailsGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkNextDtl_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndexDtl += 1;
            BindLoadTimeDetailsGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }    

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompany.SelectedValue != "0")
            FillWorkgroups(Convert.ToInt64(ddlCompany.SelectedValue));
        else
        {
            ddlWorkgroup.Items.Clear();
            ddlWorkgroup.Items.Add(new ListItem() { Text = "- Workgroup -", Value = "0" });
            ddlUser.Items.Clear();
            ddlUser.Items.Add(new ListItem() { Text = "- User -", Value = "0" });
        }
    }

    protected void ddlModuleName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlModuleName.SelectedValue != "0")
            FillSubMenu(Convert.ToInt64(ddlModuleName.SelectedValue));
        else
        {
            ddlSubMenu.Items.Clear();
            ddlSubMenu.Items.Add(new ListItem() { Text = "- Sub Menu -", Value = "0" });
        }
    }    

    protected void ddlWorkgroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWorkgroup.SelectedValue != "0")
            FillUsers();
        else
        {
            ddlUser.Items.Clear();
            ddlUser.Items.Add(new ListItem() { Text = "- User -", Value = "0" });
        }
    }       

    #endregion

    #region Methods

    protected void FillUsers()
    {
        try
        {
            Int64 CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            Int64 WorkgroupID  = Convert.ToInt64(ddlWorkgroup.SelectedValue);

            UserInformationRepository UIRepo = new UserInformationRepository();
            List<Incentex.DAL.SqlRepository.UserInformationRepository.AllUser> ObjUser = UIRepo.GetEmployeesByWorkGrp(CompanyID, WorkgroupID);
            Common.BindDropDown(ddlUser, ObjUser, "UserName", "UserInfoID", "- User -");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void FillSubMenu(long ModuleID)
    {
        try
        {
            PageLoadRepository PageLoadRepo = new PageLoadRepository();
            List<MediaHelpVidSub> ObjSubMenu = PageLoadRepo.GetSubMenu(ModuleID);
            Common.BindDropDown(ddlSubMenu, ObjSubMenu, "ModuleName", "MediaHelpVidSubId", "- Sub Menu -");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void FillWorkgroups(Int64 CompanyID)
    {
        try
        {
            LookupRepository objLookupRepository = new LookupRepository();
            List<INC_Lookup> objList = new List<INC_Lookup>();

            //Bind Workgroups
            objList = objLookupRepository.GetWorkgroupByCompany(CompanyID);
            Common.BindDropDown(ddlWorkgroup, objList, "sLookupName", "iLookupID", "- Workgroup -");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindLoadTimeGrid(Boolean isForSorting)
    {
        try
        {
            PageLoadRepository ObjLoadTimeRepo = new PageLoadRepository();
            List<GetLoadTimeDetailsResult> lstLoadTime = ObjLoadTimeRepo.GetLoadTimeData(this.CompanyID, this.UserID, this.WorkgroupID, this.LoadTimeFrom, this.LoadTimeTo, this.ModuleName, this.SubMenu, this.BrowserName, this.DeviceName, this.PageEventName, this.FromDate, this.ToDate, this.KeyWord, this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);

            if (lstLoadTime != null && lstLoadTime.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstLoadTime[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;

                lstLoadTime = lstLoadTime.Select(x => new GetLoadTimeDetailsResult()
                {                    
                    TotalRecords = x.TotalRecords.Value,                    
                    UserInfoID = x.UserInfoID,
                    FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.FirstName.Split(' ')[0]) + " " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.FirstName.Split(' ')[1]),
                    CompanyName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.CompanyName),
                    Workgroup = x.Workgroup,
                    RowNum = x.RowNum.Value,
                    ModuleName = x.ModuleName,
                    SubMenu = x.SubMenu,
                }).ToList();
            }

            //if (lstLoadTime.Count > 0)
            //{
            //    lstLoadTime.Add(new GetLoadTimeDetailsResult()
            //    {
            //        LoadTime = lstLoadTime[0].AvgLoadTime
            //    });
            //}

            gvLoadTime.DataSource = lstLoadTime;
            gvLoadTime.DataBind();            


            if (!isForSorting)
            {
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gvLoadTime.Rows.Count > 0)
            {
                totalcount_em.InnerText = lstLoadTime[0].TotalRecords + " results";
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

    protected void BindLoadTimeDetailsGrid(Boolean isForSorting)
    {
        try
        {
            PageLoadRepository ObjLoadTimeRepo = new PageLoadRepository();
            List<GetLoadTimeSubDetailsResult> lstLoadTime = ObjLoadTimeRepo.GetLoadTimeDetailsData(this.CompanyID, this.UserIDDtl, this.WorkgroupID, this.LoadTimeFrom, this.LoadTimeTo, this.ModuleNameDtl, this.SubMenuDtl, this.BrowserName, this.DeviceName, this.PageEventName, this.FromDate, this.ToDate, this.KeyWord, this.NoOfRecordsToDisplay, this.PageIndexDtl, this.SortColumnDtl, this.SortDirectionDtl);

            if (lstLoadTime != null && lstLoadTime.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPagesDtl = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstLoadTime[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPagesDtl = 1;

                lstLoadTime = lstLoadTime.Select(x => new GetLoadTimeSubDetailsResult()
                {
                    TotalRecords = x.TotalRecords.Value,
                    LoadTimeID = x.LoadTimeID,
                    CreatedDate = x.CreatedDate,
                    LoadTime = x.LoadTime,
                    PageName = x.PageName,
                    PageEvent = x.PageEvent,
                    BrowserName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.BrowserName),
                    DeviceName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.DeviceName),
                    AvgLoadTime = x.AvgLoadTime,                    
                    RowNum = x.RowNum.Value,
                }).ToList();
            }

            if (lstLoadTime.Count > 0)
            {
                lstLoadTime.Add(new GetLoadTimeSubDetailsResult()
                {
                    LoadTime = lstLoadTime[0].AvgLoadTime
                });
            }

            gvLTSubDetails.DataSource = lstLoadTime;
            gvLTSubDetails.DataBind();


            if (!isForSorting)
            {
                GeneratePagingDtl();
                lnkPreviousDtl.Enabled = this.PageIndexDtl > 1;
                lnkNextDtl.Enabled = this.PageIndexDtl < this.TotalPagesDtl;
            }

            if (gvLTSubDetails.Rows.Count > 0)
            {
                //totalcount_em.InnerText = lstLoadTime[0].TotalRecords + " results";
                pagingTableDtl.Visible = true;
            }
            else
            {
                //totalcount_em.InnerText = "no results";
                pagingTableDtl.Visible = false;
            }

            //totalcount_em.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('system-data-popup', 'warranty-content');", true);   
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

            //Bind Modules
            PageLoadRepository PageLoadRepo = new PageLoadRepository();
            List<MediaHelpVidPrnt> objModule = PageLoadRepo.GetModules();
            Common.BindDropDown(ddlModuleName, objModule, "ModuleName", "MediaHelpVidPrntId", "- Module -");

            ddlSubMenu.Items.Add(new ListItem() { Text = "- Sub Menu -", Value = "0" });
            ddlUser.Items.Add(new ListItem() { Text = "- User -", Value = "0" });
            ddlWorkgroup.Items.Add(new ListItem() { Text = "- Workgroup -", Value = "0" });

            //Bind Browsers
            List<BrowserNames> objBrowsers = PageLoadRepo.GetBrowsers();
            Common.BindDropDown(ddlBrowser, objBrowsers, "BrowserName", "BrowserName", "- Browser -");

            //Bind Devices
            List<DeviceNames> ObjDevice = PageLoadRepo.GetDeviceNames();
            Common.BindDropDown(ddlDevice, ObjDevice, "DeviceName", "DeviceName", "- Device -");

            //Bind Page Events
            List<EventNames> objEvents = PageLoadRepo.GetEventNames();
            Common.BindDropDown(ddlEventName, objEvents, "EventName", "EventName", "- Page Event -");
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


    private void GeneratePagingDtl()
    {
        try
        {
            Int32 tCurrentPg = this.PageIndexDtl;
            Int32 tToPg = this.ToPageDtl;
            Int32 tFrmPg = this.FromPageDtl;

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

            if (tToPg > this.TotalPagesDtl)
                tToPg = this.TotalPagesDtl;

            this.ToPageDtl = tToPg;
            this.FromPageDtl = tFrmPg;

            DataTable dt = new DataTable();
            dt.Columns.Add("Index");

            for (Int32 i = tFrmPg; i <= tToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Index"] = i;
                dt.Rows.Add(dr);
            }

            dtlPagingDtl.DataSource = dt;
            dtlPagingDtl.DataBind();
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
                BindLoadTimeGrid(false);                
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dtlPagingDtl_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ChangePage")
            {
                this.PageIndexDtl = Convert.ToInt32(e.CommandArgument);
                BindLoadTimeDetailsGrid(false);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion        
}
