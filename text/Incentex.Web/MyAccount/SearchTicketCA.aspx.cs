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
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using System.Text;

public partial class MyAccount_SearchTicketCA : PageBase
{
    #region Page Properties
    ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Tickets";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/TrackingCenter.aspx";

            FillWorkGroup();
            FillServiceTicketStatus();
            FillServiceTicketOwner();
        }
    }

    #endregion

    #region Page Control Events

    /// <summary>
    /// Event for Searching Support ticket
    /// </summary>    
    protected void lnkBtnSearchNow_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/MyAccount/SearchTicketResult.aspx" + GetSearchURL());
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Event for filling contact name on selection of the workgroup
    /// </summary>    
    protected void ddlWorkGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillContact(Convert.ToInt64(ddlWorkGroup.SelectedValue));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// For filling the workgroup dropdown
    /// </summary>
    private void FillWorkGroup()
    {
        try
        {
            objSerTicRep = new ServiceTicketRepository();
            ddlWorkGroup.DataSource = objSerTicRep.GetServiceTicketWorkGroupsForCA(IncentexGlobal.CurrentMember.UserInfoID).OrderBy(le => le.WorkGroup);
            ddlWorkGroup.DataValueField = "WorkGroupID";
            ddlWorkGroup.DataTextField = "WorkGroup";
            ddlWorkGroup.DataBind();
            ddlWorkGroup.Items.Insert(0, new ListItem("-All WorkGroup-", "0"));

            FillContact(Convert.ToInt64(ddlWorkGroup.SelectedValue));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// For filling the contact name dropdown base on workgroup
    /// </summary>
    private void FillContact(Int64 WorkGroupID)
    {
        try
        {
            if (WorkGroupID > 0)
            {
                objSerTicRep = new ServiceTicketRepository();
                ddlContactName.DataSource = objSerTicRep.GetServiceTicketContactByCACompanyAndWorkGroup(IncentexGlobal.CurrentMember.UserInfoID).Where(le => le.WorkgroupID == WorkGroupID).Select(le => new { UserInfoID = le.UserInfoID, User = le.FirstName + " " + le.LastName }).OrderBy(le => le.User);
                ddlContactName.DataValueField = "UserInfoID";
                ddlContactName.DataTextField = "User";
                ddlContactName.DataBind();

                ddlContactName.Enabled = true;
            }
            else
            {
                ddlContactName.Enabled = false;
                ddlContactName.Items.Clear();
            }

            ddlContactName.Items.Insert(0, new ListItem("-All Contact-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// For filling the support ticket status dropdown
    /// </summary>
    private void FillServiceTicketStatus()
    {
        try
        {
            LookupDA sServiceSupportStatus = new LookupDA();
            LookupBE sServiceSupportStatusBE = new LookupBE();
            sServiceSupportStatusBE.SOperation = "selectall";
            sServiceSupportStatusBE.iLookupCode = "ServiceTicketStatus";
            DataSet ds = new DataSet();
            ds = sServiceSupportStatus.LookUp(sServiceSupportStatusBE);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            dt.DefaultView.Sort = "sLookupName";
            ddlServiceTicketStatus.DataSource = dt.DefaultView.ToTable();
            ddlServiceTicketStatus.DataValueField = "iLookupID";
            ddlServiceTicketStatus.DataTextField = "sLookupName";
            ddlServiceTicketStatus.DataBind();
            ddlServiceTicketStatus.Items.Insert(0, new ListItem("-Select Support Ticket Status-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// For filling the support ticket owner dropdown
    /// </summary>
    private void FillServiceTicketOwner()
    {
        try
        {
            UserInformationRepository sServiceTicketOwner = new UserInformationRepository();
            var lstServiceTicketOwner = sServiceTicketOwner.GetIncentexEmployees().Select(le => new { UserInfoID = le.UserInfoID, EmployeeName = le.FirstName + " " + le.LastName }).OrderBy(le => le.EmployeeName).ToList();

            ddlServiceTicketOwner.DataSource = lstServiceTicketOwner;
            ddlServiceTicketOwner.DataValueField = "UserInfoID";
            ddlServiceTicketOwner.DataTextField = "EmployeeName";
            ddlServiceTicketOwner.DataBind();
            ddlServiceTicketOwner.Items.Insert(0, new ListItem("-Select Support Ticket Owner-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Clear Controls
    /// </summary>
    private void ClearControls()
    {
        lblMsg.Text = String.Empty;
        ddlWorkGroup.SelectedIndex = 0;
        ddlContactName.SelectedIndex = 0;
        txtServiceTicketName.Text = "";
        txtServiceTicketNumber.Text = "";
        ddlServiceTicketStatus.SelectedIndex = 0;
    }

    private String GetSearchURL()
    {
        StringBuilder SearchURL = new StringBuilder();

        if (!String.IsNullOrEmpty(txtServiceTicketName.Text.Trim()))
        {
            SearchURL.Append("?nam=" + Convert.ToString(txtServiceTicketName.Text.Trim()));
        }

        if (!String.IsNullOrEmpty(txtServiceTicketNumber.Text.Trim()))
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?num=" + Convert.ToString(txtServiceTicketNumber.Text.Trim()));
            else
                SearchURL.Append("&num=" + Convert.ToString(txtServiceTicketNumber.Text.Trim()));
        }

        if (!String.IsNullOrEmpty(ddlWorkGroup.SelectedValue) && ddlWorkGroup.SelectedValue != "0")
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?wg=" + Convert.ToString(ddlWorkGroup.SelectedValue));
            else
                SearchURL.Append("&wg=" + Convert.ToString(ddlWorkGroup.SelectedValue));
        }

        if (!String.IsNullOrEmpty(ddlContactName.SelectedValue) && ddlContactName.SelectedValue != "0")
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?con=" + Convert.ToString(ddlContactName.SelectedValue));
            else
                SearchURL.Append("&con=" + Convert.ToString(ddlContactName.SelectedValue));
        }

        if (!String.IsNullOrEmpty(ddlServiceTicketStatus.SelectedValue) && ddlServiceTicketStatus.SelectedValue != "0")
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?sta=" + Convert.ToString(ddlServiceTicketStatus.SelectedValue));
            else
                SearchURL.Append("&sta=" + Convert.ToString(ddlServiceTicketStatus.SelectedValue));
        }

        if (!String.IsNullOrEmpty(ddlServiceTicketOwner.SelectedValue) && ddlServiceTicketOwner.SelectedValue != "0")
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?own=" + Convert.ToString(ddlServiceTicketOwner.SelectedValue));
            else
                SearchURL.Append("&own=" + Convert.ToString(ddlServiceTicketOwner.SelectedValue));
        }

        if (!String.IsNullOrEmpty(txtKeyWord.Text.Trim()))
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?kw=" + Convert.ToString(txtKeyWord.Text.Trim()));
            else
                SearchURL.Append("&kw=" + Convert.ToString(txtKeyWord.Text.Trim()));
        }

        return SearchURL.ToString();
    }

    #endregion
}
