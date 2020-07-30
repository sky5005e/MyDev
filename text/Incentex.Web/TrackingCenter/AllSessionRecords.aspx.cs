using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Newtonsoft.Json.Linq;

public partial class TrackingCenter_AllSessionRecords : PageBase
{
    #region  Properties

    CompanyRepository objComRepo = new CompanyRepository();
    UserInformationRepository objUserRepo = new UserInformationRepository();

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

    Int64 UserInfoID
    {
        get
        {
            if (ViewState["UserInfoID"] == null)
            {
                ViewState["UserInfoID"] = 0;
            }
            return Convert.ToInt64(ViewState["UserInfoID"]);
        }
        set
        {
            ViewState["UserInfoID"] = value;
        }
    }

    String RequestType
    {
        get
        {
            if (ViewState["RequestType"] == null)
            {
                ViewState["RequestType"] = String.Empty;
            }
            return ViewState["RequestType"].ToString();
        }
        set
        {
            ViewState["RequestType"] = value;
        }
    }

    String SearchString
    {
        get
        {
            if (ViewState["SearchString"] == null)
            {
                ViewState["SearchString"] = String.Empty;
            }
            return ViewState["SearchString"].ToString();
        }
        set
        {
            ViewState["SearchString"] = value;
        }
    }

    DateTime FormDate
    {
        get
        {
            if (ViewState["FormDate"] == null)
                return DateTime.Now;
            else
                return Convert.ToDateTime(ViewState["FormDate"]);
        }
        set
        {
            ViewState["FormDate"] = value;
        }
    }

    DateTime ToDate
    {
        get
        {
            if (ViewState["ToDate"] == null)
                return DateTime.Now;
            else
                return Convert.ToDateTime(ViewState["ToDate"]);
        }
        set
        {
            ViewState["ToDate"] = value;
        }
    }

    String SearchResult
    {
        get
        {
            if (ViewState["SearchResult"] == null)
            {
                ViewState["SearchResult"] = String.Empty;
            }
            return ViewState["SearchResult"].ToString();
        }
        set
        {
            ViewState["SearchResult"] = value;
        }
    }

    Int32 TotalSession = 0;

    #endregion 

    #region Page Event's

    protected void Page_Load(object sender, EventArgs e)
    {    
         if (!IsPostBack)
         {
             base.MenuItem = "Tracking Center";
             base.ParentMenuID = 0;

             if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                 base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
             else
                 base.SetAccessRights(true, true, true, true);

             if (!base.CanView)
             {
                 base.RedirectToUnauthorised();
             }

             ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Session Report";

             if (!String.IsNullOrEmpty(Request.QueryString["cid"]))
                 this.CompanyID = Convert.ToInt64(Request.QueryString["cid"]);
             if (!String.IsNullOrEmpty(Request.QueryString["uid"]))
                 this.UserInfoID = Convert.ToInt64(Request.QueryString["uid"]);
             if (!String.IsNullOrEmpty(Request.QueryString["fromDate"]))
                 this.FormDate = Convert.ToDateTime(Request.QueryString["fromDate"]);
             if (!String.IsNullOrEmpty(Request.QueryString["toDate"]))
                 this.ToDate = Convert.ToDateTime(Request.QueryString["toDate"]);
             if (!String.IsNullOrEmpty(Request.QueryString["req"]))
                 this.RequestType = Request.QueryString["req"];

             if (RequestType == "viewerror")
             {
                 this.SearchResult = "Error Sessions";
                 this.SearchString = "er_True";
             }
             else if (RequestType == "viewcart")
             {
                 this.SearchResult = "Cart Abandonment's Sessions";
                 this.SearchString = "cart_False";
             }
             else if (RequestType == "viewtoday")
             {
                 this.SearchResult = "Today's Sessions";
                 SearchString = String.Empty;
             }
             else if (RequestType == "viewsession")
             {
                 this.SearchResult = "Sessions";
                 SearchString = String.Empty;
             }
             if (RequestType == "viewtoday")
                 ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/TrackingCenter/ViewVideoSession.aspx";
             else
             ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/TrackingCenter/SessionsSearch.aspx?req=" + this.RequestType;

             BindGridView();

             lblCount.Text = String.Format("Total Number of Sessions : {0}", TotalSession.ToString());
         }
    }

    protected void gvRecordingList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            if (this.ViewState["SortOrder"].ToString() == "ASC")
                ImgSort.ImageUrl = "";
            else
                ImgSort.ImageUrl = "";

            switch (this.ViewState["SortExp"].ToString())
            {
                case "CountryCode":
                    PlaceHolder placeholderCountryCode = (PlaceHolder)e.Row.FindControl("placeholderCountryCode");
                    break;

                //case "LastUpdated":
                //    PlaceHolder placeholderLastUpdated = (PlaceHolder)e.Row.FindControl("placeholderLastUpdated");
                //    break;

                case "CompanyName":
                    PlaceHolder placeholderCompanyName = (PlaceHolder)e.Row.FindControl("placeholderCompanyName");
                    break;
                case "UserName":
                    PlaceHolder placeholderUserName = (PlaceHolder)e.Row.FindControl("placeholderUserName");
                    break;

                case "VisitLength":
                    PlaceHolder placeholderVisitLength = (PlaceHolder)e.Row.FindControl("placeholderVisitLength");
                    break;

                //case "MaxScrolledPercentage":
                //    PlaceHolder placeholderMaxScrolledPercentage = (PlaceHolder)e.Row.FindControl("placeholderMaxScrolledPercentage");
                //    break;

                case "Browser":
                    PlaceHolder placeholderBrowser = (PlaceHolder)e.Row.FindControl("placeholderBrowser");
                    break;

                case "OS":
                    PlaceHolder placeholderOS = (PlaceHolder)e.Row.FindControl("placeholderOS");
                    break;

            }
        }
    }

    protected void gvRecordingList_RowCommand(object sender, GridViewCommandEventArgs e)
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
            BindGridView();
        }
        catch (Exception ex)
        {
        }
    }
    #endregion 

    #region Page Method's

    //bind dropdown for the company 
    public void BindGridView()
    {
        try
        {
            // To get Distinct value from Generic List using LINQ
            // Create an Equality Comprarer Intance
            List<Common.RecordingList> objList = GetFinalRecordingList(this.FormDate, this.ToDate, this.CompanyID, this.UserInfoID, this.SearchString);
            IEqualityComparer<Common.RecordingList> customComparer = new Common.PropertyComparer<Common.RecordingList>("Id");
            IEnumerable<Common.RecordingList> objListDistinct = objList.Distinct(customComparer);

            DataView myDataView = new DataView();
            DataTable dataTable = Common.ListToDataTable(objListDistinct);
            myDataView = dataTable.DefaultView;

            if (this.ViewState["SortExp"] != null)
                myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();

            TotalSession = objListDistinct.Count();
            if (objListDistinct.Count() > 0)
                lblMsgGrid.Text = "Below is result based on search criteria: " + SearchResult;
            else
                lblMsgGrid.Text = "No Record found with searc Criteria : " + SearchResult;

            gvRecordingList.DataSource = myDataView;
            gvRecordingList.DataBind();
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
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
   /// <summary>
   /// TO get Page detials List 
   /// </summary>
   /// <param name="pageviewsURL"></param>
   /// <returns></returns>
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
                                if(item.Contains("cid_"))
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
                                
                                if (objUser !=null)
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
    /// <summary>
    /// To get All records from Pageview Lsit and Recording List
    /// </summary>
    /// <param name="pageviewsURL"></param>
    /// <returns></returns>
    private List<Common.RecordingList> GetFinalRecordingList(DateTime FromDate, DateTime ToDate, Int64 compID, Int64 UserID, String searchBy)
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
                       ImageSrc = "~/admin/Incentex_Used_Icons/CountryFlagsIcon/" + rl.CountryCode+".png",
                       PopUrl = "http://account.c.mouseflow.com/my-account/recordings/play?session="+rl.Id+"&email="+email+"&token="+apiToken
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


    #endregion
}