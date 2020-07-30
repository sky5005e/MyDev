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

public partial class admin_ServiceTicketCenter_SearchServiceTicket : PageBase
{
    #region Page Properties
    ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
    #endregion

    #region Page Events

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

            ((Label)Master.FindControl("lblPageHeading")).Text = "Search Support Ticket";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/ServiceTicketCenter.aspx";

            FillCompanyName();
            FillServiceTicketStatus();
            FillServiceTicketOwner();
            FillOpenedBy(0);
            FillSupplier();
            FillTypeOfRequest();
            chkPostedNotes.Checked = true;
            spanChkPostedNote.Attributes.Add("class", "custom-checkbox_checked alignleft");            
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
            Response.Redirect("~/Admin/ServiceTicketCenter/SearchResult.aspx" + GetSearchURL());
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Event for Searching Support ticket
    /// </summary>    
    protected void lnkActivityReport_Click(object sender, EventArgs e)
    {
        try
        {
            String SearchURL = GetSearchURL();
            if (chkPostedNotes.Checked)
            {
                if (String.IsNullOrEmpty(SearchURL.ToString()))
                    SearchURL = "?pn=1";
                else
                    SearchURL += "&pn=1";
            }

            Session.Remove("ActRepScrPos");
            Response.Redirect("~/Admin/ServiceTicketCenter/ActivityReport.aspx" + SearchURL);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Event for filling contact name on selection of the company
    /// </summary>    
    protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCompanyName.SelectedIndex <= 0)
            {
                ddlContactName.Enabled = false;
                ddlContactName.Items.Clear();
                ddlContactName.Items.Add(new ListItem("-Select Contact Name-", "0"));
                FillOpenedBy(0);
            }
            else
            {
                UserInformationRepository UsrInfRepo = new UserInformationRepository();
                ddlContactName.Enabled = true;
                ddlContactName.DataSource = UsrInfRepo.GetByCompanyId(Convert.ToInt64(ddlCompanyName.SelectedValue)).Where(le => le.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin)).Select(le => new { ContactName = le.FirstName + " " + le.LastName, UserInfoID = le.UserInfoID }).OrderBy(le => le.ContactName).ToList();
                ddlContactName.DataTextField = "ContactName";
                ddlContactName.DataValueField = "UserInfoID";
                ddlContactName.DataBind();
                ddlContactName.Items.Insert(0, new ListItem("-Select Contact Name-", "0"));
                FillOpenedBy(Convert.ToInt32(ddlCompanyName.SelectedValue));
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// For filling the company name dropdown
    /// </summary>
    private void FillCompanyName()
    {
        try
        {
            CompanyRepository objCompRep = new CompanyRepository();
            ddlCompanyName.DataSource = objCompRep.GetAllQuery().OrderBy(le => le.CompanyName).ToList();
            ddlCompanyName.DataValueField = "CompanyID";
            ddlCompanyName.DataTextField = "CompanyName";
            ddlCompanyName.DataBind();
            ddlCompanyName.Items.Insert(0, new ListItem("-All Company-", "0"));

            ddlContactName.Enabled = false;
            ddlContactName.Items.Clear();
            ddlContactName.Items.Add(new ListItem("-Select Contact Name-", "0"));
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
    /// For filling the type of request dropdown
    /// </summary>
    private void FillTypeOfRequest()
    {
        try
        {
            LookupDA sServiceSupportStatus = new LookupDA();
            LookupBE sServiceSupportStatusBE = new LookupBE();
            sServiceSupportStatusBE.SOperation = "selectall";
            sServiceSupportStatusBE.iLookupCode = "TypeOfRequest";
            DataSet ds = new DataSet();
            ds = sServiceSupportStatus.LookUp(sServiceSupportStatusBE);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            dt.DefaultView.Sort = "sLookupName";
            ddlTypeOfRequest.DataSource = dt.DefaultView.ToTable();
            ddlTypeOfRequest.DataValueField = "iLookupID";
            ddlTypeOfRequest.DataTextField = "sLookupName";
            ddlTypeOfRequest.DataBind();
            ddlTypeOfRequest.Items.Insert(0, new ListItem("-Select Type Of Request-", "0"));
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

            ddlSubOwner.DataSource = lstServiceTicketOwner;
            ddlSubOwner.DataValueField = "UserInfoID";
            ddlSubOwner.DataTextField = "EmployeeName";
            ddlSubOwner.DataBind();
            ddlSubOwner.Items.Insert(0, new ListItem("-Select Sub Owner-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// For filling the support ticket opened by dropdown
    /// </summary>
    private void FillOpenedBy(int CompanyID)
    {
        try
        {
            ddlOpenedby.Items.Clear();
            var lstServiceTicketOpenedBy = objSerTicRep.GetServiceTicketOpenedByFromCompanyID(CompanyID).Select(le => new { OpenedBy = le.FirstName + " " + le.LastName, OpenedByID = le.OpenedByID }).OrderBy(le => le.OpenedBy).ToList();

            ddlOpenedby.DataSource = lstServiceTicketOpenedBy;
            ddlOpenedby.DataValueField = "OpenedByID";
            ddlOpenedby.DataTextField = "OpenedBy";
            ddlOpenedby.DataBind();
            ddlOpenedby.Items.Insert(0, new ListItem("-Select Opened By-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// For filling the supplier drop down
    /// </summary>    
    private void FillSupplier()
    {
        try
        {
            SupplierRepository objSuppRep = new SupplierRepository();
            ddlSupplier.DataSource = objSuppRep.GetAllQuery().OrderBy(le => le.CompanyName).ToList();
            ddlSupplier.DataValueField = "UserInfoID";
            ddlSupplier.DataTextField = "CompanyName";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("-Select Supplier Name-", "0"));
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
        lblMsg.Text = string.Empty;
        ddlCompanyName.SelectedIndex = 0;
        ddlContactName.SelectedIndex = 0;
        txtServiceTicketName.Text = "";
        txtServiceTicketNumber.Text = "";
        ddlServiceTicketStatus.SelectedIndex = 0;
        ddlOpenedby.SelectedIndex = 0;        
    }

    private String GetSearchURL()
    {
        StringBuilder SearchURL = new StringBuilder();

        if (!String.IsNullOrEmpty(ddlCompanyName.SelectedValue) && ddlCompanyName.SelectedValue != "0")
        {
            SearchURL.Append("?com=" + Convert.ToString(ddlCompanyName.SelectedValue));

            if (!String.IsNullOrEmpty(ddlContactName.SelectedValue) && ddlContactName.SelectedValue != "0")
            {
                SearchURL.Append("&con=" + Convert.ToString(ddlContactName.SelectedValue));
            }
        }

        if (!String.IsNullOrEmpty(ddlOpenedby.SelectedValue) && ddlOpenedby.SelectedValue != "0")
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?ob=" + Convert.ToString(ddlOpenedby.SelectedValue));
            else
                SearchURL.Append("&ob=" + Convert.ToString(ddlOpenedby.SelectedValue));
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

        if (!String.IsNullOrEmpty(txtServiceTicketName.Text.Trim()))
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?nam=" + Convert.ToString(txtServiceTicketName.Text.Trim()));
            else
                SearchURL.Append("&nam=" + Convert.ToString(txtServiceTicketName.Text.Trim()));
        }

        if (!String.IsNullOrEmpty(txtServiceTicketNumber.Text.Trim()))
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?num=" + Convert.ToString(txtServiceTicketNumber.Text.Trim()));
            else
                SearchURL.Append("&num=" + Convert.ToString(txtServiceTicketNumber.Text.Trim()));
        }

        if (!String.IsNullOrEmpty(txtDatePromised.Text.Trim()))
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?dn=" + Convert.ToString(txtDatePromised.Text.Trim()));
            else
                SearchURL.Append("&dn=" + Convert.ToString(txtDatePromised.Text.Trim()));
        }

        if (!String.IsNullOrEmpty(ddlSupplier.SelectedValue) && ddlSupplier.SelectedValue != "0")
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?sp=" + Convert.ToString(ddlSupplier.SelectedValue));
            else
                SearchURL.Append("&sp=" + Convert.ToString(ddlSupplier.SelectedValue));
        }

        if (!String.IsNullOrEmpty(ddlTypeOfRequest.SelectedValue) && ddlTypeOfRequest.SelectedValue != "0")
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?tor=" + Convert.ToString(ddlTypeOfRequest.SelectedValue));
            else
                SearchURL.Append("&tor=" + Convert.ToString(ddlTypeOfRequest.SelectedValue));
        }

        if (!String.IsNullOrEmpty(txtKeyWord.Text.Trim()))
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?kw=" + Convert.ToString(txtKeyWord.Text.Trim()));
            else
                SearchURL.Append("&kw=" + Convert.ToString(txtKeyWord.Text.Trim()));
        }

        if (!String.IsNullOrEmpty(ddlSubOwner.SelectedValue) && ddlSubOwner.SelectedValue != "0")
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?so=" + Convert.ToString(ddlSubOwner.SelectedValue));
            else
                SearchURL.Append("&so=" + Convert.ToString(ddlSubOwner.SelectedValue));
        }

        if (!String.IsNullOrEmpty(txtFromDate.Text.Trim()))
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?fd=" + Convert.ToString(txtFromDate.Text.Trim()));
            else
                SearchURL.Append("&fd=" + Convert.ToString(txtFromDate.Text.Trim()));
        }

        if (!String.IsNullOrEmpty(txtToDate.Text.Trim()))
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?td=" + Convert.ToString(txtToDate.Text.Trim()));
            else
                SearchURL.Append("&td=" + Convert.ToString(txtToDate.Text.Trim()));
        }

        if (chkNoActivity.Checked)
        {
            if (String.IsNullOrEmpty(SearchURL.ToString()))
                SearchURL.Append("?na=1");
            else
                SearchURL.Append("&na=1");
        }

        return SearchURL.ToString();
    }

    #endregion
}