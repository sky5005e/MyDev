using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
public partial class UserManagement_UserManagement : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "User Management";
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
            ((Label)Master.FindControl("lblPageHeading")).Text = "User Management";
            BindUserManageDetail();
        }
    }

    public void BindUserManageDetail()
    {
        try
        {
            ManageDetailsRepository objManageRepos = new ManageDetailsRepository();
            List<INC_ManageDetail> objManage = new List<INC_ManageDetail>();
            objManage = objManageRepos.GetManageDetailName(15);
            dtUserManage.DataSource = objManage;
            dtUserManage.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void dtUserManage_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "GO")
            {
                HiddenField hdnID, hdnsManagename;

                if (e.CommandName.Equals("GO"))
                {
                    hdnsManagename = (HiddenField)e.Item.FindControl("hdnsManagename");
                    hdnID = (HiddenField)e.Item.FindControl("hdnManageID");
                    int imangeid = Convert.ToInt32(hdnID.Value);
                    if (hdnsManagename.Value == "Manage Company")
                    {
                        IncentexGlobal.ManageID = 1;
                        Response.Redirect("~/admin/Company/ViewCompany.aspx?id=" + imangeid);
                    }
                    if (hdnsManagename.Value == "Manage Supplier")
                    {
                        IncentexGlobal.ManageID = 2;
                        Response.Redirect("~/admin/Supplier/ViewSupplier.aspx?id=" + imangeid);
                    }
                    if (hdnsManagename.Value == "Manage Incentex Employee")
                    {
                        IncentexGlobal.ManageID = 3;
                        Response.Redirect("../Admin/IncentexEmployee/ViewIncentexEmployee.aspx?id=" + imangeid);
                    }
                    if (hdnsManagename.Value == "Search Users")
                    {
                        IncentexGlobal.ManageID = 3;
                        Response.Redirect("SearchUsers.aspx");
                    }
                    if (hdnsManagename.Value == "View Pending Users")
                    {

                        Response.Redirect("ViewPendingUsers.aspx");
                    }
                    if (hdnsManagename.Value == "Manage Ship To")
                    {
                        Response.Redirect("../Admin/ManagedShipAddress.aspx");
                    }
                }


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
