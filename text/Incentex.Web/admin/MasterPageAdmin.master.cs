using System;
using System.Configuration;
using System.Web.UI;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_MasterPageAdmin : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblCopyRightYear.Text = DateTime.Now.Year.ToString();
            UpdateUserHistoryForTracking();
            lblSystemDate.Text = System.DateTime.Now.Date.ToString("MMMM dd, yyyy");
            if (IncentexGlobal.CurrentMember != null)
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                {
                    aSearchServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "admin/ServiceTicketCenter/SearchResult.aspx?sp=" + Convert.ToString(IncentexGlobal.CurrentMember.UserInfoID);
                    aOpenServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "admin/index.aspx?st=2";
                }
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
                {
                    aSearchServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "admin/ServiceTicketCenter/SearchResult.aspx?ie=" + Convert.ToString(IncentexGlobal.CurrentMember.UserInfoID);
                    aOpenServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "admin/index.aspx?st=1";
                }
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SupplierEmployee))
                {
                    aSearchServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "admin/ServiceTicketCenter/SearchResult.aspx?ie=" + Convert.ToString(IncentexGlobal.CurrentMember.UserInfoID);
                    aOpenServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "admin/index.aspx?st=2";
                }
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    aSearchServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "MyAccount/TrackServiceTicket.aspx?qv=1";
                    aOpenServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "index.aspx?st=1";
                }
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                {
                    aSearchServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "MyAccount/TrackServiceTicket.aspx?qv=1";
                    aOpenServiceTicket.HRef = ConfigurationSettings.AppSettings["siteurl"] + "index.aspx?st=2";
                }

                imgOpenServiceTicket.Src = ConfigurationSettings.AppSettings["siteurl"] + "Images/ticket.png";
                imgSupportTickets.Src = ConfigurationSettings.AppSettings["siteurl"] + "Images/search.png";
            }
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {

        if (IncentexGlobal.CurrentMember != null)
        {

            //Update Value in User Activity
            // UdpateUserLoginAndActivity();
            //log out time for the UserTrackingCenter
            //UdpateUserLoginForTC();
            //End

            if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
            {
                //Update Value in User Activity
                // UdpateUserLoginAndActivity();
                //log out time for the UserTrackingCenter
                UdpateUserLoginForTC();
                //End
            }

        }

        Session.Remove("CurrentMember");
        Session.Remove("ManageID");
        Session.Remove("GSEMgtCurrentMember");
        IncentexGlobal.CurrentMember = null;
        IncentexGlobal.GSEMgtCurrentMember = null;
        IncentexGlobal.ManageID = 0;
        Session.RemoveAll();
        Response.Redirect("~/login.aspx");
    }

    protected void imgOpenServiceTicket_Click(object sender, ImageClickEventArgs e)
    {
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
        {
            Response.Redirect(ConfigurationSettings.AppSettings["siteurl"] + "admin/index.aspx?st=1", false);
        }
        else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
        {
            Response.Redirect(ConfigurationSettings.AppSettings["siteurl"] + "admin/index.aspx?st=2", false);
        }
    }

    protected void imgServiceTickets_Click(object sender, ImageClickEventArgs e)
    {
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
        {
            Response.Redirect("~/Admin/ServiceTicketCenter/SearchResult.aspx?com=&con=&ob=&sta=&own=&nam=&num=&dn=&sp=" + Convert.ToString(IncentexGlobal.CurrentMember.UserInfoID) + "&tor=&kw=");
        }
        else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
        {
            Response.Redirect("~/admin/ServiceTicketCenter/SearchServiceTicket.aspx", false);
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
    // This is for the logout for the USERtracking system.
    public void UdpateUserLoginForTC()
    {

        UserTrackingRepo objTrackinRepo = new UserTrackingRepo();
        UserTracking objTrackinTbl = new UserTracking();
        DateTime LogoutTime = Convert.ToDateTime(System.DateTime.Now.TimeOfDay.ToString());
        objTrackinRepo.SetLogout(Convert.ToInt32(Session["trackID"]), LogoutTime);

    }
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

            // this is for getting only page name from uri start
            // Uri uri = new Uri(Request.Url.AbsoluteUri);
            //string filename = Path.GetFileName(uri.LocalPath);
            //objhistbl.PagesName = filename;
            // this is for getting only page name from uri end

            objhistbl.DateTimePage = DateTime.Now;
            objTrackinRepo.Insert(objhistbl);
            objTrackinRepo.SubmitChanges();
        }
    }
}
