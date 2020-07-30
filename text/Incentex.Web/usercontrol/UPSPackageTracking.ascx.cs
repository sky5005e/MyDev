using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using TrackWebReference;

public partial class usercontrol_UPSPackageTracking : System.Web.UI.UserControl
{
    #region Data Members
    public string TrackingNumber
    {
        get
        {
            if (ViewState["TrackingNumber"] == null)
            {
                ViewState["TrackingNumber"] = "";
            }
            return ViewState["TrackingNumber"].ToString();
        }
        set
        {
            ViewState["TrackingNumber"] = value;
        }

    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.TrackingNumber))
        {
            BindTrackingDetail();
        }
    }

    protected override void OnInit(EventArgs e)
    {
        Page.Init += delegate(object sender, EventArgs e_Init)
        {
            if (ToolkitScriptManager.GetCurrent(Page) == null && ScriptManager.GetCurrent(Page) == null)
            {
                ToolkitScriptManager sMgr = new ToolkitScriptManager();
                phScriptManager.Controls.AddAt(0, sMgr);
            }
        };
        base.OnInit(e);
    }

    #endregion

    #region Methods
    protected void BindTrackingDetail()
    {
        try
        {
            TrackService track = new TrackService();
            TrackRequest tr = new TrackRequest();
            UPSSecurity upss = new UPSSecurity();
            UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
            upssSvcAccessToken.AccessLicenseNumber = System.Configuration.ConfigurationSettings.AppSettings["UPSLicenceNumber"];
            upss.ServiceAccessToken = upssSvcAccessToken;
            UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
            upssUsrNameToken.Username = System.Configuration.ConfigurationSettings.AppSettings["UPSUserName"];
            upssUsrNameToken.Password = System.Configuration.ConfigurationSettings.AppSettings["UPSPassword"];
            upss.UsernameToken = upssUsrNameToken;
            track.UPSSecurityValue = upss;
            RequestType request = new RequestType();
            String[] requestOption = { "15" };
            request.RequestOption = requestOption;
            tr.Request = request;
            tr.InquiryNumber = this.TrackingNumber;
            System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicyForUPS();
            TrackResponse trackResponse = track.ProcessTrack(tr);

            //Display Information
            lblTrackingNumber.Text = trackResponse.Shipment[0].Package[0].TrackingNumber;
            lblShippedBy.Text = trackResponse.Shipment[0].Service.Description;

            if (trackResponse.Shipment[0].ShipmentAddress != null)
            {
                for (int i = 0; i < trackResponse.Shipment[0].ShipmentAddress.Length; i++)
                {
                    if (trackResponse.Shipment[0].ShipmentAddress[i].Type != null && trackResponse.Shipment[0].ShipmentAddress[i].Type.Description!=null && trackResponse.Shipment[0].ShipmentAddress[i].Type.Description == "ShipTo Address")
                    {
                        lblShipToAddress.Text = trackResponse.Shipment[0].ShipmentAddress[i].Address.AddressLine[0] != null ? trackResponse.Shipment[0].ShipmentAddress[i].Address.AddressLine[0] : "" + "," + trackResponse.Shipment[0].ShipmentAddress[i].Address.City!=null?trackResponse.Shipment[0].ShipmentAddress[i].Address.City:"" + "," + trackResponse.Shipment[0].ShipmentAddress[i].Address.StateProvinceCode!=null?trackResponse.Shipment[0].ShipmentAddress[i].Address.StateProvinceCode:"" + ", " + trackResponse.Shipment[0].ShipmentAddress[i].Address.CountryCode!=null?trackResponse.Shipment[0].ShipmentAddress[i].Address.CountryCode:"" + ", " + trackResponse.Shipment[0].ShipmentAddress[i].Address.PostalCode!=null?trackResponse.Shipment[0].ShipmentAddress[i].Address.PostalCode:"";
                    }
                }
            }

            string DateDelivered = "";
            string TimeDelivered = "";

            string htmlData = "";
            htmlData += "<table cellspacing='0' border='0' style='border-collapse: collapse;' class='orderreturn_box'>";
            htmlData += "<tr class='ord_header'><th width='30%'><span>Location</span><div class='corner'><span class='ord_headtop_cl'></span><span class='ord_headbot_cl'></span></div></th><th width='20%'><span>Date</span></th><th width='10%'><span>Time</span></th><th width='40%'><span>Activity</span></th></tr>";
            if (trackResponse.Shipment[0].Package[0] != null && trackResponse.Shipment[0].Package[0].Activity != null)
            {
                for (int i = 0; i < trackResponse.Shipment[0].Package[0].Activity.Length; i++)
                {
                    try
                    {
                        if (trackResponse.Shipment[0].Package[0].Activity[i].Status != null && trackResponse.Shipment[0].Package[0].Activity[i].Status.Description!=null && trackResponse.Shipment[0].Package[0].Activity[i].Status.Description.ToLower().Contains("delivered"))
                        {
                            if (trackResponse.Shipment[0].Package[0].Activity[i].ActivityLocation != null)
                            {
                                lblLeftAt.Text = trackResponse.Shipment[0].Package[0].Activity[i].ActivityLocation.Description!=null ? trackResponse.Shipment[0].Package[0].Activity[i].ActivityLocation.Description:"";
                                lblSignedBy.Text = trackResponse.Shipment[0].Package[0].Activity[i].ActivityLocation.SignedForByName!=null?trackResponse.Shipment[0].Package[0].Activity[i].ActivityLocation.SignedForByName:"";
                            }
                            try
                            {
                                DateDelivered = trackResponse.Shipment[0].Package[0].Activity[i].Date.Substring(4, 2) + "/" + trackResponse.Shipment[0].Package[0].Activity[i].Date.Substring(6, 2) + "/" + trackResponse.Shipment[0].Package[0].Activity[i].Date.Substring(0, 4);
                                TimeDelivered = trackResponse.Shipment[0].Package[0].Activity[i].Time.Substring(0, 2) + ":" + trackResponse.Shipment[0].Package[0].Activity[i].Time.Substring(2, 2);
                            }
                            catch { }
                            //TimeDelivered = Convert.ToString(Convert.ToInt32(trackResponse.Shipment[0].Package[0].Activity[i].Time.Substring(0, 2)) > 12 ? Convert.ToInt32(trackResponse.Shipment[0].Package[0].Activity[i].Time.Substring(0, 2)) - 12 : Convert.ToInt32(trackResponse.Shipment[0].Package[0].Activity[i].Time.Substring(0, 2))) + ":" + trackResponse.Shipment[0].Package[0].Activity[i].Time.Substring(2, 2) + " " + Convert.ToString(Convert.ToInt32(trackResponse.Shipment[0].Package[0].Activity[i].Time.Substring(0, 2)) > 12 ? "AM" : "PM");
                        }
                        htmlData += "<tr class='ord_content'>";
                        try
                        {
                            if (trackResponse.Shipment[0].Package[0].Activity[i].ActivityLocation != null && trackResponse.Shipment[0].Package[0].Activity[i].ActivityLocation.Address != null)
                                htmlData += "<td class='g_box'><span class='first'>" + trackResponse.Shipment[0].Package[0].Activity[i].ActivityLocation.Address.City + "," + trackResponse.Shipment[0].Package[0].Activity[i].ActivityLocation.Address.StateProvinceCode + "," + trackResponse.Shipment[0].Package[0].Activity[i].ActivityLocation.Address.CountryCode + "</span><div class='corner'><span class='ord_greytop_cl'></span><span class='ord_greybot_cl'></span></div></td>";
                            else
                                htmlData += "<td class='g_box'><span class='first'>&nbsp;</span><div class='corner'><span class='ord_greytop_cl'></span><span class='ord_greybot_cl'></span></div></td>";
                        }
                        catch { }
                        try
                        {
                            htmlData += "<td class='b_box'><span>" + trackResponse.Shipment[0].Package[0].Activity[i].Date.Substring(4, 2) + "/" + trackResponse.Shipment[0].Package[0].Activity[i].Date.Substring(6, 2) + "/" + trackResponse.Shipment[0].Package[0].Activity[i].Date.Substring(0, 4) + "</span></td>";
                            htmlData += "<td class='g_box'><span>" + trackResponse.Shipment[0].Package[0].Activity[i].Time.Substring(0, 2) + ":" + trackResponse.Shipment[0].Package[0].Activity[i].Time.Substring(2, 2) + "</span></td>";
                        }
                        catch { }
                        try
                        {
                            htmlData += "<td class='b_box'><span>" + trackResponse.Shipment[0].Package[0].Activity[i].Status.Description + "</span></td>";
                        }
                        catch { }
                        htmlData += "</tr>";
                    }
                    catch { }
                }
            }

            htmlData += "</table>";
            dvShipmentProgress.InnerHtml = htmlData;

            lblDeliveredOn.Text = DateDelivered + " " + TimeDelivered;

            mpePackageTracking.Show();
        }
        catch
        {
        }
    }
    #endregion
}
