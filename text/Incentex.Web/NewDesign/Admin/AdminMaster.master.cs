using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class NewDesign_Admin_AdminMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

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
        Session.RemoveAll();
        Response.Redirect("~/login.aspx");
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
