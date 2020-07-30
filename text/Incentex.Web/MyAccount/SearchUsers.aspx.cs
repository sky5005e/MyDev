/// <summary>
/// Page seperated by mayur on 13-jan-2012
/// SearchUsers.aspx and SearchUserResult.aspx this are two page for complete user search
/// </summary>

using System;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;

public partial class admin_searchusers : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Users";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/UserManagement.aspx";
            FillWorkgroup();
        }
    }

    #region control events
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect("SearchUserResult.aspx?id=" + Request.QueryString["id"].ToString() + "&FirstName=" + txtFirstName.Text.Trim() + "&LastName=" + txtLastname.Text.Trim() + "&Email=" + txtEmailAddress.Text.Trim() + "&EmployeeID=" + txtEmployeeId.Text.Trim() + "&WorkGroup=" + hdnWorkgroup.Value + "&Gender=" + hdnGender.Value);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtFirstName.Text = "";
        txtLastname.Text = "";
        txtEmailAddress.Text = "";
        txtEmployeeId.Text = "";
        hdnGender.Value = "";
        hdnWorkgroup.Value = "";
    }
    #endregion

    #region Method
    protected void FillWorkgroup()
    {
        //Get Company Employee Information to get the workgroup for which he is admin
        CompanyEmployee objCmpnyInfo = new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        List<AdditionalWorkgroup> additionaWorkgroupList = new AdditionalWorkgroupRepository().GetManageDetailName(Convert.ToInt32(objCmpnyInfo.CompanyEmployeeID), Convert.ToInt32(IncentexGlobal.CurrentMember.CompanyId));

        List<Int64> workgroupList = new List<Int64>();
        workgroupList.Add((Int64)objCmpnyInfo.ManagementControlForWorkgroup);
        for (int i = 0; i < additionaWorkgroupList.Count; i++)
        {
            workgroupList.Add((Int64)additionaWorkgroupList[i].WorkgroupID);
        }

        List<INC_Lookup> objLookList = new LookupRepository().GetByLookupWorkgroupList("Workgroup ", workgroupList);

        string strData = "";
        strData += "<select tabindex='8' id='Workgroup' name='Workgroup'>";
        strData += "<option value='0'>-select workgroup-</option>";
        for (int i = 0; i < objLookList.Count; i++)
        {
            string path = "../../Admin/Incentex_Used_Icons/" + objLookList[i].sLookupIcon;

            strData += "<option value='" + objLookList[i].iLookupID + "' title='" + path + "'>" + objLookList[i].sLookupName + "</option>";
        }
        strData += "</select>";

        ddlWorkgroup.InnerHtml = strData;
    }
    #endregion
}
