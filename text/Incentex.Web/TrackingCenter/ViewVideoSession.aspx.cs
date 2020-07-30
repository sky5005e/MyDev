using System;
using System.Web.UI.WebControls;

public partial class TrackingCenter_ViewVideoSession : PageBase
{
    #region Page Method's

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

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Session Recordings";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/TrackingCenter/ReportSelection.aspx";
            lnkViewTodaySessions.NavigateUrl = "~/TrackingCenter/AllSessionRecords.aspx?req=viewtoday";
            lnkErrorMessageSessions.NavigateUrl = "~/TrackingCenter/SessionsSearch.aspx?req=viewerror";
            lnkSessionReport.NavigateUrl = "~/TrackingCenter/SessionsSearch.aspx?req=viewsession";
            lnkViewCartAbandonments.NavigateUrl = "~/TrackingCenter/SessionsSearch.aspx?req=viewcart";
        }
    }

    #endregion
}