using System;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class CompanyStore_ViewNews : PageBase
{
    CompanyStoreDocumentRepository objRep = new CompanyStoreDocumentRepository();

    Int64 CompanyID;
    Int64 UserInfoID;

    #region Page Load Event

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Redirect user to login page if session gets expires
            CheckLogin();

            //Assign Page Header and return URL 
            ((Label)Master.FindControl("lblPageHeading")).Text = "Recent News";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            if (IncentexGlobal.IsIEFromStore)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            }

            //Retrieve UserID and CompanyID
            //UserInfoID = Int64.Parse(Request.QueryString["u"]);
            //CompanyID = Int64.Parse(Request.QueryString["c"]);
            UserInfoID = Int64.Parse(Session["UserInfoID"].ToString());
            CompanyID = Int64.Parse(Session["CompanyID"].ToString());

            //Load News
            LoadNews();
        }
    }

    #endregion

    #region Misc Functions

    private void LoadNews()
    {
        try
        {
            //get workgroup by userinformationid
            CompanyEmployee objCmpnyInfo = new CompanyEmployee();
            CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
            objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

            //Get news by workgroup and companyid
            rptNews.DataSource = objRep.GetAllNewsByWorkgroupId(objCmpnyInfo.WorkgroupID, CompanyID);
            rptNews.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    /// <summary>
    /// Created Date: 21-Dec-2010 by Shehzad
    /// Gets the path for the each news in Repeater
    /// </summary>
    /// <param name="value">Gets the sender</param>
    /// <returns></returns>
    protected string SetNews(object value)
    {
        string strNews;
        if (value != null) //Set Path + PDF File name
        {
            strNews = "../UploadedImages/CompanyStoreDocuments/" + value.ToString();
            return strNews;
        }
        return string.Empty;
    }
    
    #endregion


}
