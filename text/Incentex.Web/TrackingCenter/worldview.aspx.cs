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
using System.Xml.Linq;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.Collections.Generic;
using System.Xml;
using Artem.Google.UI;
using System.Text;

public partial class TrackingCenter_worldview : PageBase
{
    UserTrackingRepo objRepo = new UserTrackingRepo();
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

            ((Label)Master.FindControl("lblPageHeading")).Text = "World View";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/TrackingCenter/ReportSelection.aspx";
            DateTime now = DateTime.Now;
            string year = now.Year.ToString();
            string month = now.Month.ToString();
            string day = now.Day.ToString();
            txtStartDateip.Text = month + "/" + day + "/" + year;
            txtEndDateip.Text = month + "/" + day + "/" + year;
            //GoogleMap1.Visible = false;


        }
    }
    // this function is for the bind map using the ip address
    public void bindmap()
    {

        List<Incentex.DAL.SqlRepository.UserTrackingRepo.getip> objList = new List<Incentex.DAL.SqlRepository.UserTrackingRepo.getip>();
        objList = objRepo.Getallip(Convert.ToDateTime(txtStartDateip.Text), Convert.ToDateTime(txtEndDateip.Text));
        if (objList.Count > 0)
        {
            GoogleMarker objMarker;
            for (int ipcount = 0; ipcount < objList.Count; ipcount++)
            {
                GeoDetailsByIP latlong;
                if (objList[ipcount].IPAddress.ToString().Contains(','))
                {
                    string[] obj = objList[ipcount].IPAddress.Split(',');
                    latlong = Common.GetLatLong(obj[0]);
                }
                else
                {
                    latlong = Common.GetLatLong(objList[ipcount].IPAddress);
                }

                objMarker = new GoogleMarker(latlong.latitude, latlong.longitude);
                GoogleMap1.Markers.Add(objMarker);
                StringBuilder Info = new StringBuilder();
                Info.Append("<b style='font-size:medium'>Address:</b>: ");
                Info.AppendLine("<br/>");
                Info.AppendLine("<br/>");
                Info.Append("<b>Country</b>: " + latlong.countryName);
                Info.AppendLine("<br/>");
                Info.Append("<b>Region</b>: " + latlong.regionName);
                Info.AppendLine("<br/>");
                Info.Append("<b>City</b>: " + latlong.cityName);
                Info.AppendLine("<br/>");
                if (latlong.zipCode != "-")
                {
                    Info.Append("<b>ZipCode</b>: " + latlong.zipCode);
                }
                Label lbl = new Label();
                lbl.Text = Info.ToString();

                //objMarker.InfoContent.Attributes.Add("style", "foreColor:#1155CC");
                //objMarker.Shadow=
                objMarker.InfoContent.ForeColor = System.Drawing.Color.FromArgb(17, 85, 204);
                objMarker.InfoContent.Controls.Add(lbl);
                objMarker.OpenInfoBehaviour = OpenInfoBehaviour.Click;
                GoogleMap1.Center = new LatLng(latlong.latitude, latlong.longitude);

            }

        }

        modal.Show();
    }
    protected void lnkBtnReportWorldWide_Click(object sender, EventArgs e)
    {

        bindmap();
    }
}
