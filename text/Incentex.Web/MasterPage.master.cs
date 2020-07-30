using System;
using System.Configuration;
using System.Web.UI;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MasterPage : System.Web.UI.MasterPage
{
    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblCopyRightYear.Text = DateTime.Now.Year.ToString();
            UpdateUserHistoryForTracking();
            lblSystemDate.Text = System.DateTime.Now.Date.ToString("MMMM dd, yyyy");

            if (IncentexGlobal.CurrentMember != null && (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee)) /*&& !Request.Url.ToString().Contains("index.aspx")*/)
            {
                //for visible video training button based on page open.
                VisibleVideoTrainingModule();

                //for visible Remote Support button
                VisibleRemoteSupport();

                aSearchServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "MyAccount/TrackServiceTicket.aspx?qv=1";

                if (Session["CoupaID"] != null)
                    aOpenServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "index.aspx?st=3";
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                    aOpenServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "index.aspx?st=1";
                else
                    aOpenServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "index.aspx?st=2";
            }
            else
                aOpenServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "login.aspx?st=1";

            if (IncentexGlobal.IndexPageLink != null)
                imgHomeBtn.Attributes.Add("onclick", "javascript:window.location='" + IncentexGlobal.IndexPageLink + "';return false;");

            imgOpenServiceTicket.Src = ConfigurationSettings.AppSettings["siteurl"] + "Images/ticket.png";
            imgSupportTickets.Src = ConfigurationSettings.AppSettings["siteurl"] + "Images/search.png";
            imgRemoteSupport.Src = ConfigurationSettings.AppSettings["siteurl"] + "Images/remoteSupport_button.png";
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        // for tracking center logout time in table(UserTracking)
        if (IncentexGlobal.CurrentMember != null)
            UdpateUserLoginForTC();

        IncentexGlobal.CurrentMember = null;
        IncentexGlobal.GSEMgtCurrentMember = null;
        IncentexGlobal.IsIEFromStore = false;
        IncentexGlobal.IsIEFromStoreTestMode = false;
        Session.RemoveAll();
        Response.Redirect("~/login.aspx");
    }

    protected void imgbtnPlayVideo_Click(object sender, ImageClickEventArgs e)
    {
        VideoTraining objVideoTraining = new VideoTraining();
        objVideoTraining = new VideoTrainingRepository().GetVideoByStoreIDWorkgroupIDUserTypeAndUrl(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.Usertype, Request.Url.ToString());
        if (objVideoTraining != null)
        {
            usercontrol_TrainingVideo ur = LoadControl("~/usercontrol/TrainingVideo.ascx") as usercontrol_TrainingVideo;
            ur.VideoTitle = objVideoTraining.VideoTitle;
            if (!string.IsNullOrEmpty(objVideoTraining.VideoName))
                ur.VideoName = objVideoTraining.VideoName;
            else
                ur.VideoYouTubID = objVideoTraining.YouTubeVideoID;

            pnlLoadVideoUserControl.Controls.Add(ur);
        }
    }

    #endregion

    #region Methods

    // this function for the insert records into userpagehistorytable
    public void UpdateUserHistoryForTracking()
    {
        // this is for enter the value in page history table(start)(Parth)
        UserPageHistoryTracking objhistbl = new UserPageHistoryTracking();
        UserTrackingRepo objTrackinRepo = new UserTrackingRepo();

        if (Session["trackID"] != null)
        {
            objhistbl.UserTrackingID = Convert.ToInt32(Session["trackID"]);
            objhistbl.PagesName = Request.Url.AbsoluteUri;
            objhistbl.DateTimePage = DateTime.Now;
            objTrackinRepo.Insert(objhistbl);
            objTrackinRepo.SubmitChanges();
        }
    }

    public void UdpateUserLoginAndActivity()
    {
        UserActivityRepository objUAct = new UserActivityRepository();
        UserLoginActivity objUser = new UserLoginActivity();
        objUser = objUAct.GetByUserId(IncentexGlobal.CurrentMember.UserInfoID);

        if (objUser != null)
        {
            //User Has logged in once
            objUser.LogOutTime = System.DateTime.Now.TimeOfDay.ToString();
            objUser.LoginStatus = false;
            objUAct.SubmitChanges();
        }
    }

    /// <summary>
    /// Visibles the video training module for open video popup.
    /// </summary>
    protected void VisibleVideoTrainingModule()
    {
        //get data for video training based on user information and page url
        VideoTraining objVideoTraining = new VideoTraining();
        objVideoTraining = new VideoTrainingRepository().GetVideoByStoreIDWorkgroupIDUserTypeAndUrl(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.Usertype, Request.Url.ToString());
        if (objVideoTraining != null)
        {
            imgbtnPlayVideo.Visible = true;
        }
        else
        {
            imgbtnPlayVideo.Visible = false;
        }
    }

    protected void VisibleRemoteSupport()
    {
        try
        {
            CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
            if (objCompanyEmployee != null && objCompanyEmployee.Preferences != null)
            {
                string[] Ids = objCompanyEmployee.Preferences.Split(',');
                foreach (string i in Ids)
                {
                    if (i.Equals(new LookupRepository().GetIdByLookupNameNLookUpCode("Remote Support", "User Preferences").ToString()))
                    {
                        aRemoteSupport.Visible = true;
                        break;
                    }
                }
            }
        }
        catch { }
    }

    // This is for the logout for the USERtracking system.
    public void UdpateUserLoginForTC()
    {

        UserTrackingRepo objTrackinRepo = new UserTrackingRepo();
        UserTracking objTrackinTbl = new UserTracking();
        DateTime LogoutTime = Convert.ToDateTime(System.DateTime.Now.TimeOfDay.ToString());

        objTrackinRepo.SetLogout(Convert.ToInt32(Session["trackID"]), LogoutTime);


    }

    #endregion
}
