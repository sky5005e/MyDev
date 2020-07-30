using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_ServiceTicketCenter : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Support Ticket Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
            ((Label)Master.FindControl("lblPageHeading")).Text = "Support Ticket Center";

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                BindServiceTicketCenter();
        }
    }

    /// <summary>    
    /// Devraj 25/01/2012
    /// BindServiceTicketCenter()
    /// for binding the support ticket menu
    /// </summary>
    public void BindServiceTicketCenter()
    {
        try
        {
            MenuPrivilegeRepository objMenuPriRepos = new MenuPrivilegeRepository();
            ManageDetailsRepository objManageRepos = new ManageDetailsRepository();
            List<INC_ManageDetail> objManage = new List<INC_ManageDetail>();
            objManage = objManageRepos.GetManageDetailName(objMenuPriRepos.GetMenuPrivilegeIDByDescription("Support Ticket Center"));
            dtServiceTicketCenter.DataSource = objManage;
            dtServiceTicketCenter.DataBind();

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dtServiceTicketCenter_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Open Support Ticket")
            {
                Response.Redirect("~/admin/ServiceTicketCenter/OpenServiceTicket.aspx", false);
            }
            if (e.CommandName == "Search Support Ticket")
            {
                Response.Redirect("~/admin/ServiceTicketCenter/SearchServiceTicket.aspx", false);
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}