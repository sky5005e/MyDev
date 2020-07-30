using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Data;
using System.Reflection;
using System.Configuration;

public partial class admin_CommunicationCenter_ViewTodaysMail :PageBase
{
    #region Data Members

    CompanyRepository objCompanyRepos = new CompanyRepository();
    TodayEmailsRepository objRepo = new TodayEmailsRepository();
    Incentex.DAL.Common.DAEnums.SortOrderType SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
            }
            return (Incentex.DAL.Common.DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }

    String SiteURL
    {
        get
        {
            return Convert.ToString(ConfigurationSettings.AppSettings["siteurl"]);
        }
    }
    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Todays Sent Emails";
            base.ParentMenuID = 29;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CampaignSelection.aspx";
            ((Label)Master.FindControl("lblPageHeading")).Text = "Today's Emails";
            BindGridView();
        }
    }
    protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Int64 userID = Convert.ToInt64(((Label)e.Row.FindControl("lblUserInfoID")).Text);
            GridView gridViewNested = (GridView)e.Row.FindControl("nestedGridView");
            List<TodayEmailsRepository.GetDetailsTodayMails> objList = objRepo.GetAllDetailsofTodaysMail(DateTime.Now, userID);
            gridViewNested.DataSource = objList;
            gridViewNested.DataBind();

        }
    }
    protected void nestedGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow row = (GridViewRow)e.Row;
        if (row.RowType == DataControlRowType.DataRow)
        {
            //Now Want to open Popup window from hyper link
            HiddenField hdnTempID = (HiddenField)row.FindControl("hdnTempID");
            Int64 mailID = Convert.ToInt64(hdnTempID.Value);
            String urlpath = new TodayEmailsRepository().GetMessagePathID(mailID);//"ViewTemplates.aspx?mailID=" + mailID;
            urlpath = urlpath.Replace("~/", SiteURL);
            HyperLink hypViewTemp = (HyperLink)row.FindControl("hypViewTemp");
            hypViewTemp.Attributes.Add("OnClick", "window.open('" + urlpath + "','PopupWindow','width=650,height=650, scrollbars=yes')");


        }
    }
    #endregion

    #region Methods
   

    private void BindGridView()
    {
        List<TodayEmailsRepository.GetUserofTodayMails> objListDistinct = objRepo.GetAllUserList(DateTime.Now);
        // To get Distinct value from Generic List using LINQ
        // Create an Equality Comprarer Intance
        IEqualityComparer<TodayEmailsRepository.GetUserofTodayMails> customComparer =  new Common.PropertyComparer<TodayEmailsRepository.GetUserofTodayMails>("UserInfoID");
        IEnumerable<TodayEmailsRepository.GetUserofTodayMails> distinctUser = objListDistinct.Distinct(customComparer); 
        grdView.DataSource = distinctUser;
        grdView.DataBind();
        lblCount.Text = String.Format("Total number of users : {0} <br/>  Total number of emails sent: {1}", distinctUser.Count().ToString(), objListDistinct.Count().ToString());
    }

    #endregion
}
