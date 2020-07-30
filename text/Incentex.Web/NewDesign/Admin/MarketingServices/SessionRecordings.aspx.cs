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
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

public partial class Admin_SessionRecordings : PageBase
{

    #region DataMembers
    PagedDataSource pds = new PagedDataSource();
    CompanyRepository objComRepo = new CompanyRepository();
    UserInformationRepository objUserRepo = new UserInformationRepository();
    #endregion

    #region Page Level Variables
    string CurrModule = "Marketing Services";
    string CurrSubMenu = "Session Recordings";
    Int32 TotalSession = 0;
    #endregion

    #region Properties & Fields

    public Boolean IsPaggingEnabled
    {
        get
        {
            if (this.ViewState["IsPaggingEnabled"] == null) return false;
            else return Convert.ToBoolean(this.ViewState["IsPaggingEnabled"]);
        }
        set
        {
            this.ViewState["IsPaggingEnabled"] = value;
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

    public IEnumerable<Common.RecordingList> SessionRecordingData
    {
        get
        {
            if (this.ViewState["SessionRecordingData"] == null) return null;
            else return (IEnumerable<Common.RecordingList>)this.ViewState["SessionRecordingData"];
        }
        set
        {
            this.ViewState["SessionRecordingData"] = value;
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
                return "LoadTime";
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
                return "DESC";
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
    protected void gvSessionRecoding_RowCommand(object sender, GridViewCommandEventArgs e)
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

                BindSessionRecodingsGrid(true, false);
            }
            else if (e.CommandName == "Details")
            {
                lblHisotryTitle.Text = "Details";
                IEnumerable<Common.RecordingList> RecordingList = (IEnumerable<Common.RecordingList>)Session["SessionRecordingData"];

                RecordingList = RecordingList.Where(x => x.Id == e.CommandArgument.ToString()).ToList();
                gvSessionDetails.DataSource = RecordingList;
                gvSessionDetails.DataBind();

                DataTable dt = new DataTable();
                dt.Columns.Add("Index");

                DataRow dr = dt.NewRow();
                dr["Index"] = 1;
                dt.Rows.Add(dr);

                dtlPagingSD.DataSource = dt;
                dtlPagingSD.DataBind();
                pagingTableSD.Visible = true;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('session-details-popup', 'warranty-content');", true);
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
    protected void btnSearchSessionRecording_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();
            Session["SessionRecordingData"] = null;

            this.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            //this.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);

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

            BindSessionRecodingsGrid(false, false);

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
            BindSessionRecodingsGrid(false, false);

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
            this.IsPaggingEnabled = true; 
            BindSessionRecodingsGrid(false, true);
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
            this.IsPaggingEnabled = true; 
            BindSessionRecodingsGrid(false, true);
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
            this.IsPaggingEnabled = true; 
            BindSessionRecodingsGrid(false, true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }          

    #endregion    

    #region Methods       

    protected void BindSessionRecodingsGrid(Boolean isForSorting, Boolean IsForPagging)
    {
        try
        {
            PagedDataSource pds = new PagedDataSource();
            IEnumerable<Common.RecordingList> RecordingList;

            if (Session["SessionRecordingData"] != null)
            {
                RecordingList = (IEnumerable<Common.RecordingList>)Session["SessionRecordingData"];
            }
            else
            {
                // To get Distinct value from Generic List using LINQ
                // Create an Equality Comprarer Intance
                List<Common.RecordingList> objList = GetFinalRecordingList(Convert.ToDateTime(this.FromDate), Convert.ToDateTime(this.ToDate), this.CompanyID, this.UserID, this.KeyWord);
                IEqualityComparer<Common.RecordingList> customComparer = new Common.PropertyComparer<Common.RecordingList>("Id");
                RecordingList = objList.Distinct(customComparer);

                
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(RecordingList.Count()) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;

                Session["SessionRecordingData"] = RecordingList;
            }

            if (ddlCompany.SelectedValue != "0")
            {
                RecordingList = RecordingList.Where(x => x.CompanyName.Contains(ddlCompany.SelectedItem.Text)).ToList();
            }
            if (ddlUser.SelectedValue != "0")
            {
                RecordingList = RecordingList.Where(x => x.UserName == ddlUser.SelectedItem.Text).ToList();
            }

            if (isForSorting)
            {
                if (this.SortColumn == "CompanyName") RecordingList = RecordingList.ToList().OrderBy(x => x.CompanyName).ToList();
                else if (SortColumn == "UserName") RecordingList = RecordingList.ToList().OrderBy(x => x.UserName).ToList();
                else if (SortColumn == "VisitLength") RecordingList = RecordingList.ToList().OrderBy(x => x.VisitLength).ToList();
                else if (SortColumn == "Browser") RecordingList = RecordingList.ToList().OrderBy(x => x.Browser).ToList();
                else if (SortColumn == "OS") RecordingList = RecordingList.ToList().OrderBy(x => x.OS).ToList();
                else if (SortColumn == "OS") RecordingList = RecordingList.ToList().OrderBy(x => x.OS).ToList();
            }
            
            //Session["SessionRecordingData"] = RecordingList;

            if (RecordingList.Count() > 0)
            {
                totalcount_em.InnerText = RecordingList.Count() + " results";
                pagingtable.Visible = true;
            }
            else
            {
                totalcount_em.InnerText = "no results";
                pagingtable.Visible = false;
            }

            totalcount_em.Visible = true;

            RecordingList = RecordingList.Skip((this.PageIndex - 1) * this.NoOfRecordsToDisplay).Take(this.NoOfRecordsToDisplay).ToList();

            if (this.KeyWord != string.Empty)
            {
                RecordingList = RecordingList.Where(x => x.CompanyName.ToUpper().Contains(this.KeyWord.ToUpper()) || x.Browser.ToUpper().Contains(this.KeyWord.ToUpper()) || x.UserName.ToUpper().Contains(this.KeyWord.ToUpper())).ToList();
            }

            DataView myDataView = new DataView();
            DataTable dataTable = Common.ListToDataTable(RecordingList);
            if (dataTable.Rows.Count > 0)
            {
                myDataView = dataTable.DefaultView;
                pds.DataSource = myDataView;

                pds.DataSource = myDataView;
                pds.PageSize = this.NoOfRecordsToDisplay;
                pds.AllowPaging = true;
                pds.CurrentPageIndex = this.PageIndex;
                pds.AllowCustomPaging = true;
                pds.VirtualCount = RecordingList.Count();

                gvSessionRecoding.DataSource = pds;
                gvSessionRecoding.DataBind();
            }
            else
            {
                gvSessionRecoding.DataSource = null;
                gvSessionRecoding.DataBind();
            }

            if (!isForSorting)
            {
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
    }

    private List<Common.RecordingList> GetFinalRecordingList(DateTime FromDate, DateTime ToDate, Int64? compID, Int64? UserID, String searchBy)
    {
        List<Common.RecordingList> objFinalRecordingList = new List<Common.RecordingList>();
        string url = ConfigurationSettings.AppSettings["MouseFlowURL"];
        string email = ConfigurationSettings.AppSettings["MouseFlowEmail"];
        string apiToken = ConfigurationSettings.AppSettings["MouseFlowAPIToken"];
        string RecordingURL = url + "/getrecordings?email=" + email + "&token=" + apiToken + "&website=6400d321-64cf-4fcb-a60d-a9e5ecdd0ee2&datefrom=" + FromDate.Date.ToString("yyyy-MM-dd") + "&dateto=" + ToDate.Date.ToString("yyyy-MM-dd");
        String pageviewsURL = url + "/getpageviews?email=" + email + "&token=" + apiToken + "&website=6400d321-64cf-4fcb-a60d-a9e5ecdd0ee2&datefrom=" + FromDate.Date.ToString("yyyy-MM-dd") + "&dateto=" + ToDate.Date.ToString("yyyy-MM-dd");

        var qry = (from pl in GetPageViewList(pageviewsURL)
                   join rl in GetRecordingList(RecordingURL) on pl.Session equals rl.Id
                   select new Common.RecordingList
                   {
                       UserName = (pl.UserName == null ? "No User" : pl.UserName),
                       CompanyName = (pl.CompanyName == null ? "No Company" : pl.CompanyName),
                       Id = rl.Id,
                       EntryUrl = rl.EntryUrl,
                       URL = pl.PageTitle,
                       MaxScrolledPercentage = pl.MaxScrolledPercentage,
                       Browser = rl.Browser,
                       IP = rl.IP,
                       Country = rl.Country,
                       CountryCode = rl.CountryCode,
                       VisitLength = rl.VisitLength,
                       LastUpdated = rl.LastUpdated,
                       OS = rl.OS,
                       ImageSrc = "~/admin/Incentex_Used_Icons/CountryFlagsIcon/" + rl.CountryCode + ".png",
                       PopUrl = "http://account.c.mouseflow.com/my-account/recordings/play?session=" + rl.Id + "&email=" + email + "&token=" + apiToken
                   }).ToList();
        
        if (compID > 0)
        {
            String companyid = "cid_" + compID;
            qry = qry.Where(q => q.EntryUrl.Contains(companyid)).ToList();
        }
        if (UserID > 0)
        {
            String userinfoid = "uid_" + UserID;
            qry = qry.Where(q => q.EntryUrl.Contains(userinfoid)).ToList();
        }
        if (!String.IsNullOrEmpty(searchBy))
        {
            qry = qry.Where(q => q.URL.Contains(searchBy)).ToList();
        }

        objFinalRecordingList = qry.ToList();
        return objFinalRecordingList;
    }

    private List<Common.Pageviews> GetPageViewList(String pageviewsURL)
    {
        List<Common.Pageviews> objPageViewList = new List<Common.Pageviews>();
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pageviewsURL);
        request.Method = "GET";
        request.ContentType = "application/json";
        try
        {
            WebResponse webResponse = request.GetResponse();
            using (Stream webStream = webResponse.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        JObject objjson = JObject.Parse(response);

                        var postTitles = (from p in objjson["PageViewList"].Children()
                                          select p).ToList();

                        for (Int32 i = 0; i < postTitles.Count; i++)
                        {
                            Common.Pageviews obj = new Common.Pageviews();
                            obj.Id = (String)objjson["PageViewList"][i]["Id"];
                            obj.Session = (String)objjson["PageViewList"][i]["Session"];
                            obj.PageTitle = (String)objjson["PageViewList"][i]["URL"];

                            String myString = obj.PageTitle;
                            String[] arrayString = myString.Split('/');
                            String Cutted = String.Empty;
                            foreach (var item in arrayString)
                            {
                                if (item.Contains("cid_"))
                                    Cutted = item.ToString();
                            }
                            //string Cutted = myString.Substring(myString.LastIndexOf("/") + 1);

                            string[] id = Cutted.Split('&');
                            if (id.Count() > 0)
                            {
                                Int64 cid = Convert.ToInt64(id[0].Substring(id[0].ToString().LastIndexOf("_") + 1));
                                Int64? uid = Convert.ToInt64(id[1].Substring(id[1].ToString().LastIndexOf("_") + 1));
                                //string er = id[2].Substring(id[2].ToString().LastIndexOf("_") + 1);
                                //string cart = id[3].Substring(id[3].ToString().LastIndexOf("_") + 1);
                                string companyName = objComRepo.GetByCompanyId(cid).Select(q => q.CompanyName).FirstOrDefault();
                                if (!String.IsNullOrEmpty(companyName))
                                    obj.CompanyName = companyName;
                                else
                                    obj.CompanyName = null;

                                UserInformation objUser = objUserRepo.GetById(uid);

                                if (objUser != null)
                                    obj.UserName = objUser.FirstName + " " + objUser.LastName;
                                else
                                    obj.UserName = null;
                            }

                            obj.Browser = (String)objjson["PageViewList"][i]["Browser"];
                            obj.IP = (String)objjson["PageViewList"][i]["IP"];
                            obj.Country = (String)objjson["PageViewList"][i]["Country"];
                            obj.CountryCode = ((String)objjson["PageViewList"][i]["CountryCode"]).ToUpper();
                            obj.MaxScrolledPercentage = (Int32)objjson["PageViewList"][i]["MaxScrolledPercentage"];
                            objPageViewList.Add(obj);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }

        foreach (var item in objPageViewList)
        {
            Boolean IsAlreadySetUser = objPageViewList.Where(i => i.Session == item.Session).Select(q => q.IsSetUserName).FirstOrDefault();
            Boolean IsAlreadySetCompany = objPageViewList.Where(i => i.Session == item.Session).Select(q => q.IsSetCompanyName).FirstOrDefault();
            if (!String.IsNullOrEmpty(item.CompanyName) && !String.IsNullOrEmpty(item.UserName) && !IsAlreadySetUser && !IsAlreadySetCompany)
            {
                string cname = item.CompanyName;
                objPageViewList.Where(i => i.Session == item.Session).ToList().ForEach(c => c.CompanyName = cname);
                string uname = item.UserName;
                objPageViewList.Where(i => i.Session == item.Session).ToList().ForEach(c => c.UserName = uname);

                objPageViewList.Where(i => i.Session == item.Session).ToList().ForEach(c => c.IsSetCompanyName = true);
                objPageViewList.Where(i => i.Session == item.Session).ToList().ForEach(c => c.IsSetUserName = true);
            }
            else if (!String.IsNullOrEmpty(item.CompanyName) && !IsAlreadySetCompany)
            {
                string cname = item.CompanyName;
                objPageViewList.Where(i => i.Session == item.Session).ToList().ForEach(c => c.CompanyName = cname);
                objPageViewList.Where(i => i.Session == item.Session).ToList().ForEach(c => c.IsSetCompanyName = true);
            }
            else if (!String.IsNullOrEmpty(item.UserName) && !IsAlreadySetUser)
            {
                string uname = item.UserName;
                objPageViewList.Where(i => i.Session == item.Session).ToList().ForEach(c => c.UserName = uname);
                objPageViewList.Where(i => i.Session == item.Session).ToList().ForEach(c => c.IsSetUserName = true);
            }
        }

        return objPageViewList;
    }

    private List<Common.RecordingList> GetRecordingList(String RecordingURL)
    {
        List<Common.RecordingList> objRecordingList = new List<Common.RecordingList>();
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(RecordingURL);
        request.Method = "GET";
        request.ContentType = "application/json";
        try
        {
            WebResponse webResponse = request.GetResponse();
            using (Stream webStream = webResponse.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        JObject objjson = JObject.Parse(response);

                        var postTitles = (from p in objjson["RecordingList"].Children()
                                          select p).ToList();

                        for (Int32 i = 0; i < postTitles.Count; i++)
                        {
                            Common.RecordingList obj = new Common.RecordingList();
                            obj.Id = (String)objjson["RecordingList"][i]["Id"];
                            obj.EntryUrl = (String)objjson["RecordingList"][i]["EntryUrl"];
                            obj.Browser = (String)objjson["RecordingList"][i]["Browser"];
                            obj.IP = (String)objjson["RecordingList"][i]["IP"];
                            obj.Country = (String)objjson["RecordingList"][i]["Country"];
                            obj.CountryCode = ((String)objjson["RecordingList"][i]["CountryCode"]).ToUpper();
                            obj.VisitLength = TimeSpan.FromSeconds((Int32)objjson["RecordingList"][i]["VisitLength"]);
                            Int32 days = Convert.ToInt32((Convert.ToDateTime(DateTime.Now.Date) - ((DateTime)objjson["RecordingList"][i]["LastUpdated"]).Date).TotalDays);

                            if (days > 0)
                                obj.LastUpdated = days.ToString() + " days ago";
                            else if (days == 0)
                                obj.LastUpdated = "Todays";
                            else
                                obj.LastUpdated = "0";

                            obj.OS = (String)objjson["RecordingList"][i]["OS"];
                            objRecordingList.Add(obj);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        return objRecordingList;
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

            //ddlWorkgroup.Items.Add(new ListItem() { Text = "- Workgroup -", Value = "0" });
            //Bind User Names
            var objUsers = (from usr in db.UserInformations where usr.IsDeleted == false select new { FirstName = usr.FirstName + " " + usr.LastName, usr.UserInfoID }).Distinct().OrderBy(x => x.FirstName).ToList();
            ddlUser.DataSource = objUsers;
            ddlUser.DataTextField = "FirstName";
            ddlUser.DataValueField = "UserInfoID";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("- User -", "0"));
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
                BindSessionRecodingsGrid(false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion    
}
