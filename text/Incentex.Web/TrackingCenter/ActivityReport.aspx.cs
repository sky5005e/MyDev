using System;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;

public partial class TrackingCenter_ActivityReport : PageBase
{
    #region Event Handlers
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
            
            ((Label)Master.FindControl("lblPageHeading")).Text = "Activity Report";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/TrackingCenter/ReportSelection.aspx";
            PnlFetchData.Visible = false;
        }
    }
    
    /// <summary>
    /// Number of user accessed :- if same user login 10 times but it will consider as 1.
    ///Numer of user placed orders : - if same user placed orderd it will consider as 1.
    ///Number of orders placed :-  total number of purchase (100+20+50 = 170)
    protected void lnkBtnActivityReport_Click(object sender, EventArgs e)
    {
        try
        {

            PnlFetchData.Visible = true;
            //GetUserActivity
            DateTime sd = Convert.ToDateTime(txtStartDate.Text);
            DateTime ed = Convert.ToDateTime(txtEndDate.Text);
            UserTrackingRepo ObjTrackRepo = new UserTrackingRepo();
            var list = ObjTrackRepo.GetUserActivity(sd, ed);
            if (list.Count > 0)
            {
                TxtNumOfOrderPlaced.Text = Convert.ToString(list[0].NumberofOrdersPlaced);
                TxtNumOfUserAccessed.Text = Convert.ToString(list[0].NumberOfUserAccessed);
                TxtNumOfUserPlacedOrder.Text = Convert.ToString(list[0].NumberOfUsersPlacedOrders);
            }
            else
            {
                LblError.Visible = true;
                LblError.Text = " No Records Found";
            }

        }
        catch (Exception ex)
        {
            ex = null;
            LblError.Visible = true;
            LblError.Text = " Please enter both the dates  ";
        }
    }
    #endregion
}

