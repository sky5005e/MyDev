using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;

public partial class ErrorPage : System.Web.UI.Page
{
    #region Data Member
    IncentexBEDataContext db = new IncentexBEDataContext();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.UrlReferrer != null)
                lblErrorPage.Text = Request.UrlReferrer.AbsolutePath;
            else
                lblErrorPage.Text = "Could not identify error page";
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            System.Web.HttpBrowserCapabilities browser = Request.Browser;
            string OSVal = GetOSName(hdnOS.Value);

            db.Page404Errors.InsertOnSubmit(new Incentex.DAL.Page404Error()
            {
                UserInfoID = Common.UserID,
                CompanyID = Common.CompID,
                ErrorPageName = lblErrorPage.Text,
                Browser = browser.Browser,
                OS = OSVal,
                Resolution = hdnResolution.Value,
                Notes = txtNotes.Text,
                ErrorDate = DateTime.Now                
            });
            db.SubmitChanges();
            Response.Redirect("~/NewDesign/Admin/Index.aspx", false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private string GetOSName(string UserAgentDetails)
    {
        UserAgentDetails = UserAgentDetails.Substring(UserAgentDetails.IndexOf('(') + 1, UserAgentDetails.IndexOf(')') - UserAgentDetails.IndexOf('(') - 1);
        switch (UserAgentDetails)
        {
            case "Windows CE":
            case "Windows 95":
            case "Windows 98": return "Windows CE";
            case "Windows 98; Win 9x 4.90": return "Windows Millennium Edition (Windows Me)";
            case "Windows NT 4.0": return "Microsoft Windows NT 4.0";
            case "Windows NT 5.0": return "Windows 2000";
            case "Windows NT 5.01": return "Windows 2000, Service Pack 1 (SP1)";
            case "Windows NT 5.1": return "Windows XP";
            case "Windows NT 5.2": return "Windows Server 2003; Windows XP x64 Edition";
            case "Windows NT 6.0": return "Windows Vista";
            case "Windows NT 6.1": return "Windows 7";
            case "Windows NT 6.2": return "Windows 8";
            case "Windows NT 6.3": return "Windows 8.1";
            default:
                {
                    if (UserAgentDetails.Split(';').Length > 1) return UserAgentDetails.Split(';')[0];
                    else return "unknown";
                }
        }
    }
}
