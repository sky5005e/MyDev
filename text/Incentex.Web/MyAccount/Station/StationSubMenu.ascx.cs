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

public partial class admin_Company_Station_StationSubMenu : System.Web.UI.UserControl
{

    public enum StationInfoType
    {
        MainInfo_1 = 1,
        ManagerInfo_2 = 2,
        AdminInfo_3 = 3,
        ServiceInfo_4 = 4,
        AdditionalInfo_5 = 5
    }

    public StationInfoType StationInfo
    {
        get
        {
            if (ViewState["StationInfo"] == null)
            {
                ViewState["StationInfo"] = StationInfoType.MainInfo_1; 
            }
            return (StationInfoType)ViewState["StationInfo"];
        }
        set
        {
            ViewState["StationInfo"] = value;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        lnkAdditionalInfo.CssClass="";
        lnkAdminInfo.CssClass="";
        lnkMainInfo.CssClass="";
        lnkManagerInfo.CssClass="";
        lnkServiceInfo.CssClass="";

        switch(this.StationInfo)
        {
            case StationInfoType.MainInfo_1:
                lnkMainInfo.CssClass = "current";
                break;
            case StationInfoType.ManagerInfo_2:
                lnkManagerInfo.CssClass = "current";
                break;
            case StationInfoType.AdminInfo_3:
                lnkAdminInfo.CssClass = "current";
                break;
            case StationInfoType.ServiceInfo_4:
                lnkServiceInfo.CssClass = "current";
                break;
            case StationInfoType.AdditionalInfo_5:
                lnkAdditionalInfo.CssClass = "current";
                break;

        }
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
}
