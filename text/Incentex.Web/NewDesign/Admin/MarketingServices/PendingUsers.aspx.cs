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

public partial class Admin_PendingUsers : PageBase
{

    #region DataMembers
    PagedDataSource pds = new PagedDataSource();    
    #endregion

    #region Page Level Variables
    string CurrModule = "Marketing Services";
    string CurrSubMenu = "Pending Users";
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
    
    public Int64? StationID
    {
        get
        {
            if (this.ViewState["StationID"] == null || Convert.ToString(this.ViewState["StationID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["StationID"]);
        }
        set
        {
            this.ViewState["StationID"] = value;
        }
    }

    public String NameOfPendingUser
    {
        get
        {
            if (Convert.ToString(this.ViewState["NameOfPendingUser"]) == string.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["NameOfPendingUser"]);
        }
        set
        {
            this.ViewState["NameOfPendingUser"] = value;
        }
    }

    public String NameOfApprover
    {
        get
        {
            if (Convert.ToString(this.ViewState["NameOfApprover"]) == string.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["NameOfApprover"]);
        }
        set
        {
            this.ViewState["NameOfApprover"] = value;
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
    protected void gvPendingUsers_RowCommand(object sender, GridViewCommandEventArgs e)
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

                BindPendingUsersGrid(true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }    

    protected void gvPendingUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //LinkButton lnkbtnAssetID = (LinkButton)e.Row.FindControl("lnkbtnAssetID");
                //LinkButton lnkbtnEquipmentType = (LinkButton)e.Row.FindControl("lnkbtnEquipmentType");
                //LinkButton lnkbtnBaseStation = (LinkButton)e.Row.FindControl("lnkbtnBaseStation");
                //LinkButton lnkbtnStatus = (LinkButton)e.Row.FindControl("lnkbtnStatus");
                //ScriptManager objCurrentScriptManager = ScriptManager.GetCurrent(this);

                //if (lnkbtnAssetID != null)
                //{
                //    objCurrentScriptManager.RegisterAsyncPostBackControl(lnkbtnAssetID);
                //}
                //if (lnkbtnEquipmentType != null)
                //{
                //    objCurrentScriptManager.RegisterAsyncPostBackControl(lnkbtnEquipmentType);
                //}
                //if (lnkbtnBaseStation != null)
                //{
                //    objCurrentScriptManager.RegisterAsyncPostBackControl(lnkbtnBaseStation);
                //}
                //if (lnkbtnStatus != null)
                //{
                //    objCurrentScriptManager.RegisterAsyncPostBackControl(lnkbtnStatus);

                //}
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
    protected void btnSearchPendingUsers_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            this.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            this.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            this.StationID = Convert.ToInt64(ddlStation.SelectedValue);
            this.NameOfPendingUser = txtNameOfPendingUser.Text;
            this.NameOfApprover = txtNameOfApprover.Text;
            this.FromDate = this.ToDate = string.Empty;

            this.KeyWord = String.Empty;
            this.NoOfRecordsToDisplay = Convert.ToInt32(Application["NEWPAGESIZE"]);
            this.PageIndex = 1;
            this.FromPage = 1;
            this.ToPage = this.NoOfPagesToDisplay;

            btnSearchGrid.Enabled = true;
            btnSearchGrid.CssClass += " postback";
            txtSearchGrid.Text = String.Empty;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                this.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);

            BindPendingUsersGrid(false);

            base.EndStopwatch("Search", CurrModule, CurrSubMenu);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnSend_click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            string SelectedRows = string.Empty;
            string[] Email, UserInfoId, UserName;
            for (int i = 0; i < gvPendingUsers.Rows.Count; i++)
            {
                if ((gvPendingUsers.Rows[i].Cells[0].FindControl("chkSelect") as CheckBox).Checked)
                {
                    if (!string.IsNullOrEmpty((gvPendingUsers.Rows[i].Cells[0].FindControl("hdbApproverEmailID") as HiddenField).Value))
                    {
                        Email = (gvPendingUsers.Rows[i].Cells[0].FindControl("hdbApproverEmailID") as HiddenField).Value.ToString().Split(',');
                        UserInfoId = (gvPendingUsers.Rows[i].Cells[0].FindControl("hdnApproverID") as HiddenField).Value.ToString().Split(',');
                        UserName = (gvPendingUsers.Rows[i].Cells[5].FindControl("lblNameOfApprover") as Label).Text.Split(',');
                        for (int j = 0; j < Email.Length; j++)
                        {
                            sendAlertEmail(Convert.ToInt64(UserInfoId[j]), Email[j], UserName[j]);
                        }
                    }
                }
            }
            base.EndStopwatch("Send Email", CurrModule, CurrSubMenu);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    //protected void lbHelpVideo_Click(object sender, EventArgs e)
    //{
    //    string strVideoURL = Common.GetMyHelpVideo("help video", "Document & Media Storage", "Document & Media Storage");
    //    if (strVideoURL != "")
    //    {
    //        this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "ShowVideo('" + strVideoURL + "');", true);
    //    }
    //}

    protected void btnSearchGrid_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            this.KeyWord = txtSearchGrid.Text;
            BindPendingUsersGrid(false);

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
            BindPendingUsersGrid(false);
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
            BindPendingUsersGrid(false);
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
            BindPendingUsersGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompany.SelectedValue != "0")
            FillStationsAndWorkgroups(Convert.ToInt64(ddlCompany.SelectedValue));
        else
        {
            ddlStation.Items.Clear();
            ddlStation.Items.Add(new ListItem() { Text = "- Station -", Value = "0" });
            ddlWorkgroup.Items.Clear();
            ddlWorkgroup.Items.Add(new ListItem() { Text = "- Workgroup -", Value = "0" });
        }
    }   

    #endregion

    #region Methods

    private void sendAlertEmail(Int64 UserInfoID, String UserEmail, String UserName)
    {
        try
        {
            string sFrmadd = "support@world-link.us.com";
            string sToadd = UserEmail.Trim();
            string sSubject = "Incentex - Pending Users to Approve";
            string sFrmname = "Incentex User Processing Team";
            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();

            String eMailTemplate = String.Empty;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/SimpleMailFormat.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            string body = null;

            body = "You have users to approve.";

            //body += "Thank you for your business!";
            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
            MessageBody.Replace("{MessageBody}", body);
            MessageBody.Replace("{Sender}", sFrmname);

            if (HttpContext.Current.Request.IsLocal)
            {
                new CommonMails().SendEmail4Local(1092, "Testing", "navin.valera@indianic.com", sSubject, MessageBody.ToString(), "incentextest6@gmail.com", "test6incentex", true);
            }
            else
                new CommonMails().SendMail(UserInfoID, "Pending Users to Approve", sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, smtphost, smtpport, false, true);


        }
        catch (Exception ex)
        {
        }

    }  

    protected void BindPendingUsersGrid(Boolean isForSorting)
    {
        try
        {
            PendingUsersRepository ObjSARepo = new PendingUsersRepository();
            List<GetPendingUsersResult> lstSAResult = ObjSARepo.GetPendingUsers(this.CompanyID, this.WorkgroupID, this.StationID, this.NameOfPendingUser, this.NameOfApprover, this.FromDate, this.ToDate, this.KeyWord, this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);

            if (lstSAResult != null && lstSAResult.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstSAResult[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;

                lstSAResult = lstSAResult.Select(x => new GetPendingUsersResult
                {
                    TotalRecords = x.TotalRecords,
                    CompanyName = x.CompanyName,
                    Station = x.Station,
                    Workgroup = x.Workgroup,
                    NameOfPendingUser = x.NameOfPendingUser != null && x.NameOfPendingUser.Split(' ').Length > 0 ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.NameOfPendingUser.Split(' ')[0]) + " " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.NameOfPendingUser.Split(' ')[1]) : x.NameOfPendingUser,
                    NameOfApprover = x.NameOfApprover,
                    ApproverID = x.ApproverID,                    
                    ApproverEmailID = x.ApproverEmailID,
                    RowNum = x.RowNum
                }).ToList();
            }

            gvPendingUsers.DataSource = lstSAResult;
            gvPendingUsers.DataBind();

            if (!isForSorting)
            {
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gvPendingUsers.Rows.Count > 0)
            {
                totalcount_em.InnerText = lstSAResult[0].TotalRecords + " results";
                pagingtable.Visible = true;
                btnSend.Visible = true;
            }
            else
            {
                totalcount_em.InnerText = "no results";
                pagingtable.Visible = false;
                btnSend.Visible = false;
            }

            totalcount_em.Visible = true;
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

            ddlStation.Items.Add(new ListItem() { Text = "- Station -", Value = "0" });
            ddlWorkgroup.Items.Add(new ListItem() { Text = "- Workgroup -", Value = "0" });
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void FillStationsAndWorkgroups(Int64 CompanyID)
    {
        try
        {
            BaseStationRepository objBaseStationRepo = new BaseStationRepository();
            List<INC_BasedStation> objStationList = objBaseStationRepo.GetBaseStationFromCompany(CompanyID);
            Common.BindDropDown(ddlStation, objStationList, "sBaseStation", "iBaseStationId", "- Station -");

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

    protected void FillWorkgroups(Int64 BaseStationID)
    {

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

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ChangePage")
            {
                this.PageIndex = Convert.ToInt32(e.CommandArgument);
                BindPendingUsersGrid(false);                
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
