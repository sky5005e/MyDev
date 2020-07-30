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
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
public partial class admin_CommunicationCenter_CampaignSelection : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Communications Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Communications Center";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
            lnkCreateCampaign.NavigateUrl = "~/admin/CommunicationCenter/CreateCampaign.aspx";
            lnkViewCampaign.NavigateUrl = "~/admin/CommunicationCenter/ViewCamp.aspx";
            lnkPendingMOASOrders.NavigateUrl = "~/admin/CommunicationCenter/ViewPendingMOASOrders.aspx?req=order";
            lnkPendingUsers.NavigateUrl = "~/admin/CommunicationCenter/ViewPendingMOASOrders.aspx?req=user";
            lnkTodayEmails.NavigateUrl = "~/admin/CommunicationCenter/ViewTodaysMail.aspx";
            lnkSystemEmailTemplates.NavigateUrl = "~/admin/CommunicationCenter/SystemEmailTemplates.aspx";
            lnkViewReports.NavigateUrl = "~/admin/CommunicationCenter/ReportDashBoard.aspx";
            lnkPendingShoppingCart.NavigateUrl = "~/admin/CommunicationCenter/ViewPendingShoppingCart.aspx";
            lnkOrdersPlacedReports.NavigateUrl = "~/admin/CommunicationCenter/ReportDashBoard.aspx?IsOrderPlaced=1";

            // For Bounce
            String urlWithParamsforBounce = "ViewTemplates.aspx?mailStatus=0&campid=1&compid=0";
            lnkBouncedDetails.Attributes.Add("OnClick", "window.open('" + urlWithParamsforBounce + "','PopupWindow','width=650,height=650,scrollbars=yes')");
        
        }
    }
}
