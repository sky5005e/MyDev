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
using System.IO;
using System.Text;

public partial class Admin_EmailMarketing : PageBase
{

    #region DataMembers
    PagedDataSource pds = new PagedDataSource();
    #endregion

    #region Page Level Variables
    string CurrModule = "Marketing Services";
    string CurrSubMenu = "Email Marketing";
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

    public Int64? UserId
    {
        get
        {
            if (this.ViewState["UserId"] == null || Convert.ToString(this.ViewState["UserId"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["UserId"]);
        }
        set
        {
            this.ViewState["UserId"] = value;
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

    public String ModuleName
    {
        get
        {
            if (Convert.ToString(this.ViewState["ModuleName"]) == string.Empty || Convert.ToString(this.ViewState["ModuleName"]) == "- Module -")
                return null;
            else
                return Convert.ToString(this.ViewState["ModuleName"]);
        }
        set
        {
            this.ViewState["ModuleName"] = value;
        }
    }

    public String EmailTitle
    {
        get
        {
            if (Convert.ToString(this.ViewState["EmailTitle"]) == string.Empty || this.ViewState["EmailTitle"].ToString() == "- Title -")
                return null;
            else
                return Convert.ToString(this.ViewState["EmailTitle"]);
        }
        set
        {
            this.ViewState["EmailTitle"] = value;
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

    public String FromDateSent
    {
        get
        {
            if (Convert.ToString(this.ViewState["FromDateSent"]) == string.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["FromDateSent"]);
        }
        set
        {
            this.ViewState["FromDateSent"] = value;
        }
    }

    public String ToDateSent
    {
        get
        {
            if (Convert.ToString(this.ViewState["ToDateSent"]) == string.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["ToDateSent"]);
        }
        set
        {
            this.ViewState["ToDateSent"] = value;
        }
    }

    public String FromDateView
    {
        get
        {
            if (Convert.ToString(this.ViewState["FromDateView"]) == string.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["FromDateView"]);
        }
        set
        {
            this.ViewState["FromDateView"] = value;
        }
    }

    public String ToDateView
    {
        get
        {
            if (Convert.ToString(this.ViewState["ToDateView"]) == string.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["ToDateView"]);
        }
        set
        {
            this.ViewState["ToDateView"] = value;
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

    public String SortColumn
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortColumn"]) == String.Empty)
                return "CreatedDate";
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

    class TempClass
    {
        public String TempName { get; set; }
        public String Para { get; set; }
    }

    /// <summary>
    /// Row Command Event of Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEmailMarketing_RowCommand(object sender, GridViewCommandEventArgs e)
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

                BindEmailMarketingGrid(true);
            }
            else if (e.CommandName == "CustomerName")
            {
                //this.MyShoppingCartID = Convert.ToInt32(e.CommandArgument);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('system-data-popup', 'warranty-content');", true);   
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvEmailHistory_RowCommand(object sender, GridViewCommandEventArgs e)
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

                BindHistoryGrid(true);
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
    protected void btnSearchAbdnCartDate_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            this.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            this.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            this.EmailTitle = ddlEmailTitle.SelectedItem.Text;
            this.ModuleName = ddlModule.SelectedItem.Text;

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
            this.ToPage = this.NoOfPagesToDisplay;
            this.FromPage = 1;

            btnSearchGrid.Enabled = true;
            btnSearchGrid.CssClass += " postback";
            txtSearchGrid.Text = String.Empty;

            BindEmailMarketingGrid(false);

            base.EndStopwatch("Search", CurrModule, CurrSubMenu);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnSearchEmailHistory_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            gvEmailMarketing.Visible = pagingtable.Visible = false;
            gvEmailHistory.Visible = pagingtableH.Visible = true;

            this.CompanyID = Convert.ToInt64(ddlCompanyH.SelectedValue);
            this.UserId = Convert.ToInt64(ddlUserH.SelectedValue);
            //Sent Date 
            if (ddlSentDateH.SelectedValue == "0")
            {
                this.FromDateSent = this.ToDateSent = string.Empty;
            }
            else if (ddlSentDateH.SelectedValue == "1")
            {
                this.FromDateSent = this.ToDateSent = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else if (ddlSentDateH.SelectedValue == "2")
            {
                this.FromDateSent = this.ToDateSent = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy");
            }
            else if (ddlSentDateH.SelectedValue == "3")
            {
                this.FromDateSent = DateTime.Now.AddDays(-7).ToString("MM/dd/yyyy");
                this.ToDateSent = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else if (ddlSentDateH.SelectedValue == "4")
            {
                this.FromDateSent = txtFromDateSentH.Text;
                this.ToDateSent = txtToDateSentH.Text;
            }

            //View Date 
            if (ddlViewDateH.SelectedValue == "0")
            {
                this.FromDateView = this.ToDateView = string.Empty;
            }
            else if (ddlViewDateH.SelectedValue == "1")
            {
                this.FromDateView = this.ToDateView = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else if (ddlViewDateH.SelectedValue == "2")
            {
                this.FromDateView = this.ToDateView = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy");
            }
            else if (ddlViewDateH.SelectedValue == "3")
            {
                this.FromDateView = DateTime.Now.AddDays(-7).ToString("MM/dd/yyyy");
                this.ToDateView = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else if (ddlViewDateH.SelectedValue == "4")
            {
                this.FromDateView = txtFromDateViewH.Text;
                this.ToDateView = txtToDateViewH.Text;
            }

            this.KeyWord = txtSearchGrid.Text;
            this.PageIndex = 1;
            this.ToPage = this.NoOfPagesToDisplay;
            this.FromPage = 1;

            btnSearchGrid.Enabled = true;
            btnSearchGrid.CssClass += " postback";
            txtSearchGrid.Text = String.Empty;

            BindHistoryGrid(false);

            base.EndStopwatch("Search Email History", CurrModule, CurrSubMenu);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnSendTemplate_click(object sender, EventArgs e)
    {
        try
        {
            Int64? CompanyId = null, WorkgroupId = null;
            if (ddlCompanySE.SelectedIndex != 0) CompanyId = Convert.ToInt64(ddlCompanySE.SelectedValue);
            if (ddlWorkgroupSE.SelectedIndex != 0) WorkgroupId = Convert.ToInt64(ddlWorkgroupSE.SelectedValue);

            if (rbSendIndividually.Checked)
            {
                if (rbSendLater.Checked)
                {
                    EmailMarketingRepository ObjEmailRepo = new EmailMarketingRepository();

                    ObjEmailRepo.AddSendLaterData(Convert.ToDateTime(txtDateTime.Text), txtCustomerEmailID.Text, string.Empty, CompanyId, WorkgroupId, TxtEmailText.Text);
                }
                else
                {
                    sendTemplateEmails(Common.UserID, txtCustomerEmailID.Text, Common.UserName, CurrModule, CurrSubMenu);
                }
            }
            else
            {
                EmailMarketingRepository objEmailRepo = new EmailMarketingRepository();
                //List<UserInformation> ObjUsers = objEmailRepo.GetUsersWithWorkgroup(Convert.ToInt64(ddlWorkgroupSE.SelectedValue));
                if (rbSendLater.Checked)
                {
                    objEmailRepo.AddSendLaterData(Convert.ToDateTime(txtDateTime.Text), txtCustomerEmailID.Text, string.Empty, CompanyId, WorkgroupId, TxtEmailText.Text);
                }
                else
                {
                    List<UserInformation> ObjUsers = objEmailRepo.GetUsersWithWorkgroup(Convert.ToInt64(ddlWorkgroupSE.SelectedValue));
                    foreach (var item in ObjUsers)
                    {
                        sendTemplateEmails(Common.UserID, item.Email, Common.UserName, CurrModule, CurrSubMenu);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnSendEmail_click(object sender, EventArgs e)
    {
        try
        {
            TxtEmailText.Text = string.Empty;
            txtCustomerEmailID.Text = string.Empty;
            ddlCompanySE.SelectedIndex = 0;
            ddlWorkgroupSE.Items.Clear();
            ddlWorkgroupSE.Items.Add(new ListItem("- Workgroup -", "0"));
            txtDateTime.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('email-marketing-popup', 'warranty-content');", true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }        
    }

    protected void btnEmailHistory_click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            if (btnEmailHistory.InnerText == "Email Search")
            {
                btnEmailHistory.InnerText = "History";
                divEmailFilter.Style.Add("display", "block");
                divHistoryFilter.Style.Add("display", "none");                
            }
            else
            {
                btnEmailHistory.InnerText = "Email Search";
                divEmailFilter.Style.Add("display", "none");
                divHistoryFilter.Style.Add("display", "block");
            }
            gvEmailHistory.Visible = gvEmailMarketing.Visible = pagingtableH.Visible = pagingtable.Visible = false;
            totalcount_em.InnerText = string.Empty;

            base.EndStopwatch("Email History Filter", CurrModule, CurrSubMenu);
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
        }
    }

    protected void ddlCompanySE_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompanySE.SelectedValue != "0")
            FillWorkgroupsSE(Convert.ToInt64(ddlCompanySE.SelectedValue));
        else
        {
            ddlWorkgroupSE.Items.Clear();
            ddlWorkgroupSE.Items.Add(new ListItem() { Text = "- Workgroup -", Value = "0" });
        }
    }

    protected void FillWorkgroupsSE(Int64 CompanyID)
    {
        try
        {
            LookupRepository objLookupRepository = new LookupRepository();
            List<INC_Lookup> objList = new List<INC_Lookup>();

            //Bind Workgroups
            objList = objLookupRepository.GetWorkgroupByCompany(CompanyID);
            Common.BindDropDown(ddlWorkgroupSE, objList, "sLookupName", "iLookupID", "- Workgroup -");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('email-marketing-popup', 'warranty-content');", true);   
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
            if (gvEmailHistory.Visible) BindHistoryGrid(false);
            else if (gvEmailMarketing.Visible) BindEmailMarketingGrid(false);

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
            BindEmailMarketingGrid(false);
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
            BindEmailMarketingGrid(false);
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
            BindEmailMarketingGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkPreviousH_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex -= 1;
            BindHistoryGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkNextH_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex += 1;
            BindHistoryGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkViewAllH_Click(object sender, EventArgs e)
    {
        try
        {
            this.PageIndex = 1;
            this.NoOfRecordsToDisplay = 0;
            BindHistoryGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods

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

    protected void BindSearchDropDowns()
    {
        try
        {
            IncentexBEDataContext db = new IncentexBEDataContext();

            //Bind Company Names
            CompanyRepository objCompanyRepository = new CompanyRepository();
            List<Company> objCompanyList = new List<Company>();
            
            objCompanyList = objCompanyRepository.GetIECompany();
            Common.BindDropDown(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "- Company -");
            Common.BindDropDown(ddlCompanySE, objCompanyList, "CompanyName", "CompanyID", "- Company -");
            Common.BindDropDown(ddlCompanyH, objCompanyList, "CompanyName", "CompanyID", "- Company -");

            if (!(IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee)))
            {
                ddlCompany.SelectedValue = Common.CompID.Value.ToString();
                ddlCompanySE.SelectedValue = Common.CompID.Value.ToString();
                ddlCompany.Enabled = false;
                ddlCompanySE.Enabled = false;
            }

            //Bind Email Title
            EmailMarketingRepository objEmailMarkRepo = new EmailMarketingRepository();
            List<EmailTitle> lstTitle = objEmailMarkRepo.GetEmailTitle();
            Common.BindDropDown(ddlEmailTitle, lstTitle, "EmailSubject", "EmailSubject", "- Title -");

            //Bind Module
            List<ModuleNames> lstModules = objEmailMarkRepo.GetModuleName();
            Common.BindDropDown(ddlModule, lstModules, "ModuleName", "ModuleName", "- Module -");

            ddlWorkgroup.Items.Add(new ListItem() { Text = "- Workgroup -", Value = "0" });

            //Bind User Names
            var objUsers = (from usr in db.UserInformations where usr.IsDeleted == false select new { FirstName = usr.FirstName + " " + usr.LastName, usr.UserInfoID }).Distinct().OrderBy(x => x.FirstName).ToList();
            ddlUserH.DataSource = objUsers;
            ddlUserH.DataTextField = "FirstName";
            ddlUserH.DataValueField = "UserInfoID";
            ddlUserH.DataBind();
            ddlUserH.Items.Insert(0, new ListItem("- User -", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindHistoryGrid(Boolean isForSorting)
    {
        try
        {
            EmailMarketingRepository ObjEmailMarkRepo = new EmailMarketingRepository();
            List<GetEmailHisoryResult> lstHistoryResult = ObjEmailMarkRepo.GetHistoryDetails(this.CompanyID, this.UserId, this.FromDateSent, this.ToDateSent, this.FromDateView, this.ToDateView, this.KeyWord, this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);

            if (lstHistoryResult != null && lstHistoryResult.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstHistoryResult[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;

                lstHistoryResult = lstHistoryResult.Select(x => new GetEmailHisoryResult
                {
                    TotalRecords = x.TotalRecords.Value,
                    SentTo = x.SentTo,
                    CompanyName = x.CompanyName,
                    Sender = x.Sender,
                    SentDate = x.SentDate,
                    ViewedDate = x.ViewedDate
                }).ToList();
            }


            gvEmailHistory.DataSource = lstHistoryResult;
            gvEmailHistory.DataBind();

            if (!isForSorting)
            {
                GeneratePagingHistory();
                lnkPreviousH.Enabled = this.PageIndex > 1;
                lnkNextH.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gvEmailHistory.Rows.Count > 0)
            {
                totalcount_em.InnerText = lstHistoryResult[0].TotalRecords + " results";
                pagingtableH.Visible = true;
            }
            else
            {
                totalcount_em.InnerText = "no results";
                pagingtableH.Visible = false;
            }

            totalcount_em.Visible = true;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindEmailMarketingGrid(Boolean isForSorting)
    {
        try
        {
            gvEmailMarketing.Visible = pagingtable.Visible = true;
            gvEmailHistory.Visible = pagingtableH.Visible = false;

            EmailMarketingRepository ObjEmailMarkRepo = new EmailMarketingRepository();
            List<GetEmailDetailsResult> lstEmailMarkResult = ObjEmailMarkRepo.GetEmailDetails(this.CompanyID, null, this.WorkgroupID, this.ModuleName, this.EmailTitle, this.FromDate, this.ToDate, this.KeyWord, this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);

            if (lstEmailMarkResult != null && lstEmailMarkResult.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstEmailMarkResult[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;

                lstEmailMarkResult = lstEmailMarkResult.Select(x => new GetEmailDetailsResult
                {
                    TotalRecords = x.TotalRecords.Value,
                    Workgroup = x.Workgroup,
                    CompanyName = x.CompanyName != null ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.CompanyName) : x.CompanyName,
                    FirstName = x.FirstName != null ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.FirstName.Split(' ')[0]) + " " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.FirstName.Split(' ')[1]) : x.FirstName,
                    CreatedDate = x.CreatedDate,
                    RowNum = x.RowNum.Value,
                    ModuleName = x.ModuleName,
                    Title = x.Title
                }).ToList();
            }


            gvEmailMarketing.DataSource = lstEmailMarkResult;
            gvEmailMarketing.DataBind();

            if (!isForSorting)
            {
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gvEmailMarketing.Rows.Count > 0)
            {
                totalcount_em.InnerText = lstEmailMarkResult[0].TotalRecords + " results";
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
    private void sendTemplateEmails(Int64 UserInfoID, String UserEmail, String UserName, String ModuleName, String MenuName)
    {
        try
        {
            EmailMarketingRepository objEmailMarkRepo = new EmailMarketingRepository();
            //EmailTemplete ObjEmailTemp = objEmailMarkRepo.GetEmailTemplate(Convert.ToInt64(ddlEmailTemplate.SelectedValue));

            string sFrmadd = "support@world-link.us.com";
            string sToadd = UserEmail.Trim();
            string sSubject = "Email Template";
            //string sFrmname = ObjEmailTemp.TempName;
            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();


            System.Net.WebClient MyWebClient = new System.Net.WebClient();
            String eMailTemplate = TxtEmailText.Text;// MyWebClient.DownloadString(Server.MapPath("~/UploadedImages/EmailTempletes/") + ObjEmailTemp.TempFile);

            if (HttpContext.Current.Request.IsLocal)
            {
                new CommonMails().SendEmail4Local(1092, "Testing", "navin.valera@indianic.com", sSubject, eMailTemplate, "incentextest6@gmail.com", "test6incentex", true, CurrModule, CurrSubMenu);
            }
            else
                new CommonMails().SendMail(UserInfoID, "Email Template", sFrmadd, sToadd, sSubject, eMailTemplate, "Incentex", smtphost, smtpport, false, true);


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
    
    private void GeneratePagingHistory()
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

            dtlPagingH.DataSource = dt;
            dtlPagingH.DataBind();
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
                BindEmailMarketingGrid(false);                
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dtlPagingH_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ChangePage")
            {
                this.PageIndex = Convert.ToInt32(e.CommandArgument);
                BindHistoryGrid(false);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion    
}
