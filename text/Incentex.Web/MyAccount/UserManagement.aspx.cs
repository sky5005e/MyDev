using System;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_UserManagement : PageBase
{
    DataSet ds = new DataSet();
    CompanyEmployeeDA objCeDa = new CompanyEmployeeDA();
    CompanyEmployeeBE objCE = new CompanyEmployeeBE();

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {   
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            
            #region Functionality of viewing WLS sotr front

            //Check if admin is back from the store front
            if (Request.QueryString["IsFromStoreFront"] != null)
            {
                if (Request.QueryString.Get("IsFromStoreFront").ToString() == "1")
                {
                    IncentexGlobal.IsIEFromStore = false;
                    IncentexGlobal.CurrentMember = null;
                    IncentexGlobal.IsIEFromStoreTestMode = false;
                    if (IncentexGlobal.AdminUser != null)
                    {
                        IncentexGlobal.CurrentMember = IncentexGlobal.AdminUser;
                        IncentexGlobal.AdminUser = null;
                        IncentexGlobal.IndexPageLink = null;
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
            }
            #endregion

            if (IncentexGlobal.IsIEFromStore)
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";

            ((Label)Master.FindControl("lblPageHeading")).Text = "User Management";
            lnkViewEmpl.NavigateUrl = "~/MyAccount/Employee/ViewEmployee.aspx?id=" + IncentexGlobal.CurrentMember.CompanyId;
            lnkViewStations.NavigateUrl = "~/MyAccount/Station/ViewStation.aspx?id=" + IncentexGlobal.CurrentMember.CompanyId;
            lnkSearchUsers.NavigateUrl = "~/MyAccount/SearchUsers.aspx?id=" + IncentexGlobal.CurrentMember.CompanyId;
            lnkPendinUsers.NavigateUrl = "~/MyAccount/ViewPendingUsers.aspx?id=" + IncentexGlobal.CurrentMember.CompanyId;
            lnkStoreManagement.NavigateUrl = "~/MyAccount/Store/News.aspx";
            BindEmployee();
        }        
    }

    protected void lnkbtnViewUserStoreFront_Click(Object sender, EventArgs e)
    {
        if (ddlStorefrontUser.SelectedIndex != 0)
        {
            if (ddlStorefrontAccessType.SelectedItem.Value == "Test")
                IncentexGlobal.IsIEFromStoreTestMode = true;
            Response.Redirect("~/index.aspx?id=true&subid=" + ddlStorefrontUser.SelectedItem.Value.ToString() + "&storeid=" + new CompanyStoreRepository().GetStoreIDByCompanyId((Int64)IncentexGlobal.CurrentMember.CompanyId).ToString());
        }
        else
        {
            lblMessage.Text = "Please select user";
            modal.Show();
        }
    }

    private void BindEmployee()
    {
        objCE.Operations = "selectcompanyemployeefullname";
        objCE.CompanyId = (Int64)IncentexGlobal.CurrentMember.CompanyId;
        objCE.WorkgroupID = new CompanyEmployeeRepository().GetByUserInfoId((Int64)IncentexGlobal.CurrentMember.UserInfoID).WorkgroupID;
        ds = objCeDa.CompanyEmployee(objCE);
        ddlStorefrontUser.DataSource = ds;
        ddlStorefrontUser.DataValueField = "CompanyEmployeeID";
        ddlStorefrontUser.DataTextField = "FullName";
        ddlStorefrontUser.DataBind();
        ddlStorefrontUser.Items.Insert(0, new ListItem("-select user-", "0"));
    }

    protected void ddlStorefrontUser_SelectedIndexChanged(Object sender, EventArgs e)
    {
        if (ddlStorefrontUser.SelectedIndex != 0)
        {
            UserInformation objUserInformation = new UserInformationRepository().GetById(new CompanyEmployeeRepository().GetById(Convert.ToInt64(ddlStorefrontUser.SelectedItem.Value)).UserInfoID);
            lblLogin.Text = objUserInformation.LoginEmail;
            lblPassword.Text = objUserInformation.Password;
        }
        modal.Show();
    }
}