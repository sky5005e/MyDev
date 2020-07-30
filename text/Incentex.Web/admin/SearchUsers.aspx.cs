/// <summary>
/// Page seperated by mayur on 10-jan-2012
/// SearchUsers.aspx and SearchUserResult.aspx this are two page for complete user search
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_searchusers : PageBase
{
    LookupRepository objLookupRepos = new LookupRepository();

    protected void Page_Load(object sender, EventArgs e)
    {
        // this is for the default search
        ddlUserType.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + btnSearch.ClientID + "');");
        txtEmployeeId.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + btnSearch.ClientID + "');");
        txtFirstName.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + btnSearch.ClientID + "');");
        txtLastname.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + btnSearch.ClientID + "');");
        ddlCompanyStore.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + btnSearch.ClientID + "');");
        txtEmailAddress.Attributes.Add("onkeypress", "javascript:return clickButton(event,'" + btnSearch.ClientID + "');");

        if (!IsPostBack)
        {
            base.MenuItem = "Search Users";
            base.ParentMenuID = 10;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            FillCompanyStore();
            BindUserTypes();
            FillBasedStation();
            FillEmployeeType();
            FillWorkgroup();
            FillGender();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Users";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/UserManagement.aspx";
            txtEmployeeId.Enabled = true;
        }
    }

    #region control events
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string userType = "0";
        string employeeType = "";
        string companyName = "";
        if (ddlUserType.SelectedItem.Value != "All Users")
            userType = ddlUserType.SelectedItem.Value;
        if (ddlCompanyStore.SelectedValue != "0")
            companyName = ddlCompanyStore.SelectedItem.Text;
        if (ddlEmployeeType.SelectedValue != "0")
            employeeType = ddlEmployeeType.SelectedItem.Text;

        Response.Redirect("SearchUserResult.aspx?UserType=" + userType + "&CompanyName=" + companyName + "&FirstName=" + txtFirstName.Text.Trim() + "&LastName=" + txtLastname.Text.Trim() + "&Email=" + txtEmailAddress.Text.Trim() + "&EmployeeID=" + txtEmployeeId.Text.Trim() + "&EmployeeType=" + employeeType + "&BaseStation=" + ddlBasestation.SelectedValue + "&WorkGroup=" + ddlWorkgroup.SelectedValue + "&Gender=" + ddlGender.SelectedValue,true);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtFirstName.Text = "";
        txtLastname.Text = "";
        ddlCompanyStore.SelectedIndex = 0;
        txtEmailAddress.Text = "";
        ddlUserType.SelectedIndex = 0;
        txtEmployeeId.Text = string.Empty;
        ddlEmployeeType.SelectedIndex = 0;
        ddlBasestation.SelectedIndex = 0;
        ddlGender.SelectedIndex = 0;
        ddlWorkgroup.SelectedIndex = 0;
    }
    protected void ddlUsertType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlUserType.SelectedItem.Text == "Company Admin" || ddlUserType.SelectedItem.Text == "Company Employee" || ddlUserType.SelectedItem.Text == "ThirdPartySupplierEmployee" || ddlUserType.SelectedItem.Text == "All Users")
        {
            txtEmployeeId.Enabled = true;
        }
        else
        {
            txtEmployeeId.Text = string.Empty;
            txtEmployeeId.Enabled = false;
        }
    }
    #endregion

    #region Miscellaneous function
    private void BindUserTypes()
    {
        UserTypeRepository objRep = new UserTypeRepository();
        List<UserType> objList = objRep.GetUserTypes();
        ddlUserType.DataSource = objList;
        ddlUserType.DataTextField = "UserType1";
        ddlUserType.DataValueField = "UserTypeID";
        ddlUserType.DataBind();
        ddlUserType.Items.Insert(0, new ListItem("All Users"));
    }

    /// <summary>
    /// Fills the company store.
    /// Added by mayur on 9-jan-2012
    /// </summary>
    private void FillCompanyStore()
    {
        try
        {   
            OrderConfirmationRepository objLookRep = new OrderConfirmationRepository();
            ddlCompanyStore.DataSource = objLookRep.GetCompanyStoreName().OrderBy(le => le.CompanyName);
            ddlCompanyStore.DataValueField = "StoreID";
            ddlCompanyStore.DataTextField = "CompanyName";
            ddlCompanyStore.DataBind();
            ddlCompanyStore.Items.Insert(0, new ListItem("All Company", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Fills the based station.
    /// </summary>
    public void FillBasedStation()
    {
        BaseStationDA sBaseStation = new BaseStationDA();
        BasedStationBE sBSBe = new BasedStationBE();

        sBSBe.SOperation = "Selectall";
        DataSet dsBaseStation = sBaseStation.LookUpBaseStation(sBSBe);
        if (dsBaseStation.Tables.Count > 0 && dsBaseStation.Tables[0].Rows.Count > 0)
        {
            ddlBasestation.DataSource = dsBaseStation.Tables[0];
            ddlBasestation.DataValueField = "iBaseStationId";
            ddlBasestation.DataTextField = "sBaseStation";
            ddlBasestation.DataBind();
            ddlBasestation.Items.Insert(0, new ListItem("-select basestation-", ""));
        }
    }

    /// <summary>
    /// Fills the type of the employee.
    /// </summary>
    public void FillEmployeeType()
    {
        LookupRepository objTitleLookRep = new LookupRepository();
        string strPayment = "EmployeeType";
        ddlEmployeeType.DataSource = objTitleLookRep.GetByLookup(strPayment);
        ddlEmployeeType.DataValueField = "iLookupID";
        ddlEmployeeType.DataTextField = "sLookupName";
        ddlEmployeeType.DataBind();
        ddlEmployeeType.Items.Insert(0, new ListItem("-select employee type-", "0"));
    }

    private void FillWorkgroup()
    {
        ddlWorkgroup.DataSource = objLookupRepos.GetByLookup("Workgroup");
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-select workgroup-", "0"));
    }
    
    private void FillGender()
    {
        ddlGender.DataSource = objLookupRepos.GetByLookup("Gender");
        ddlGender.DataValueField = "iLookupID";
        ddlGender.DataTextField = "sLookupName";
        ddlGender.DataBind();
        ddlGender.Items.Insert(0, new ListItem("-select gender-", "0"));
    }
    #endregion
}
