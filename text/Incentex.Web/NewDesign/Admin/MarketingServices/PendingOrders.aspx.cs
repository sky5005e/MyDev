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

public partial class Admin_PendingOrders : PageBase
{

    #region DataMembers
    PagedDataSource pds = new PagedDataSource();    
    #endregion

    #region Page Level Variables
    string CurrModule = "Marketing Services";
    string CurrSubMenu = "Pending Orders";
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

    public String NameOfCustomer
    {
        get
        {
            if (Convert.ToString(this.ViewState["NameOfCustomer"]) == string.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["NameOfCustomer"]);
        }
        set
        {
            this.ViewState["NameOfCustomer"] = value;
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
                return "CompanyName";
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
    protected void gvPendingOrders_RowCommand(object sender, GridViewCommandEventArgs e)
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

                BindPendingOrdersGrid(true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void gvPendingOrders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string[] ApproveDetails = (e.Row.Cells[0].FindControl("hdnApproverDetails") as HiddenField).Value.Split(',');
                string ApproveLevel = (e.Row.Cells[0].FindControl("hdnOrderApproverLevel") as HiddenField).Value;
                if (ApproveLevel != "CompanyLevel")
                {
                    string ValToShow = string.Empty;
                    string[] ApproverName = (e.Row.Cells[5].FindControl("lblNameOfApprover") as Label).Text.Split(',');
                    string RedLabels = string.Empty;
                    for (int i = 0; i < ApproveDetails.Length; i++)
                    {
                        if (RedLabels != string.Empty) RedLabels += ", ";
                        RedLabels += "<span style='color:red;'>" + ApproverName[i] + "</span>";
                    }

                    (e.Row.Cells[5].FindControl("lblNameOfApprover") as Label).Text = RedLabels;
                }
                else
                {
                    string ValToShow = string.Empty;
                    string[] ApproverName = (e.Row.Cells[5].FindControl("lblNameOfApprover") as Label).Text.Split(',');
                    string GreenLables = string.Empty, RedLabels = string.Empty;
                    for(int i = 0; i < ApproveDetails.Length;i++) 
                    {
                        if (ApproveDetails[i].Split('#')[2] == "Open")
                        {
                            if (GreenLables != string.Empty) GreenLables += ", ";
                            GreenLables += "<span style='color:green;'>" + ApproverName[i] + "</span>";

                        }
                        else
                        {
                            if (RedLabels != string.Empty) RedLabels += ", ";
                            RedLabels += "<span style='color:red;'>" + ApproverName[i] + "</span>";
                        }
                    }
                    (e.Row.Cells[5].FindControl("lblNameOfApprover") as Label).Text = GreenLables + (RedLabels != string.Empty && GreenLables != string.Empty ? ", " : "") + RedLabels;
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
    protected void btnSearchPendingOrders_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            this.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            this.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            this.StationID = Convert.ToInt64(ddlStation.SelectedValue);
            this.NameOfCustomer = txtNameOfCustomer.Text;
            this.NameOfApprover = txtNameOfApprover.Text;

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

            BindPendingOrdersGrid(false);

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
            for (int i = 0; i < gvPendingOrders.Rows.Count; i++)
            {
                if ((gvPendingOrders.Rows[i].Cells[0].FindControl("chkSelect") as CheckBox).Checked)
                {
                    string ApproveLevel = (gvPendingOrders.Rows[i].Cells[0].FindControl("hdnOrderApproverLevel") as HiddenField).Value;
                    string[] ApproveDetails = (gvPendingOrders.Rows[i].Cells[0].FindControl("hdnApproverDetails") as HiddenField).Value.Split(',');
                    if (ApproveLevel != "CompanyLevel")
                    {
                        Email = (gvPendingOrders.Rows[i].Cells[0].FindControl("hdnEmail") as HiddenField).Value.ToString().Split(',');
                        UserInfoId = (gvPendingOrders.Rows[i].Cells[0].FindControl("hdnApproverID") as HiddenField).Value.ToString().Split(',');
                        UserName = (gvPendingOrders.Rows[i].Cells[5].FindControl("lblNameOfApprover") as Label).Text.Split(',');                        

                        for (int j = 0; j < Email.Length; j++)
                        {
                            int startindex = UserName[j].IndexOf("'>") + 2;
                            int endindex = UserName[j].IndexOf("</span>");
                            UserName[j] = UserName[j].Substring(startindex, endindex - startindex);

                            sendAlertEmail(Convert.ToInt64(UserInfoId[j]), Email[j], UserName[j]);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < ApproveDetails.Length; j++)
                        {                            
                            if (ApproveDetails[j].Split('#')[2] != "Open")
                            {
                                Email = (gvPendingOrders.Rows[i].Cells[0].FindControl("hdnEmail") as HiddenField).Value.ToString().Split(',');
                                UserInfoId = (gvPendingOrders.Rows[i].Cells[0].FindControl("hdnApproverID") as HiddenField).Value.ToString().Split(',');
                                UserName = (gvPendingOrders.Rows[i].Cells[5].FindControl("lblNameOfApprover") as Label).Text.Split(',');
                                int startindex = UserName[j].IndexOf("'>") + 2;
                                int endindex = UserName[j].IndexOf("</span>");
                                UserName[j] = UserName[j].Substring(startindex, endindex - startindex);

                                sendAlertEmail(Convert.ToInt64(UserInfoId[j]), Email[j], UserName[j]);
                                break;
                            }
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

    protected void btnSearchGrid_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            this.KeyWord = txtSearchGrid.Text;
            BindPendingOrdersGrid(false);

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
            BindPendingOrdersGrid(false);
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
            BindPendingOrdersGrid(false);
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
            BindPendingOrdersGrid(false);
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
            string sSubject = "Incentex - Pending Orders to Approve";
            string sFrmname = "Incentex Order Processing Team";
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

            body = "You have orders to approve.";

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
                new CommonMails().SendMail(UserInfoID, "Pending Orders to Approve", sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, smtphost, smtpport, false, true);


        }
        catch (Exception ex)
        {
        }

    }  

    protected void BindPendingOrdersGrid(Boolean isForSorting)
    {
        try
        {
            PendingOrderRepository ObjSARepo = new PendingOrderRepository();
            List<GetPendingOrdersResult> lstSAResult = ObjSARepo.GetPendingOrders(this.CompanyID, this.WorkgroupID, this.StationID, this.NameOfCustomer, this.NameOfApprover, this.KeyWord, this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);

            if (lstSAResult != null && lstSAResult.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstSAResult[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;

                lstSAResult = lstSAResult.Select(x => new GetPendingOrdersResult
                {
                    TotalRecords = x.TotalRecords,
                    OrderID = x.OrderID,
                    CompanyName = x.CompanyName,
                    Station = x.Station,
                    Workgroup = x.Workgroup,
                    CustomerName = x.CustomerName != null && x.CustomerName.Split(' ').Length > 0 ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.CustomerName.Split(' ')[0]) + " " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.CustomerName.Split(' ')[1]) : x.CustomerName,
                    ApproverName = x.ApproverName,
                    ApproverID = x.ApproverID,
                    OrderDate = x.OrderDate,
                    ApproverEmailID = x.ApproverEmailID,
                    ApproverDetails = x.ApproverDetails,
                    OrderApproverLevel = x.OrderApproverLevel,
                    RowNum = x.RowNum
                }).ToList();
            }

            gvPendingOrders.DataSource = lstSAResult;
            gvPendingOrders.DataBind();

            if (!isForSorting)
            {
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gvPendingOrders.Rows.Count > 0)
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
                BindPendingOrdersGrid(false);                
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
