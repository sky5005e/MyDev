using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Configuration;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Report_UniformPolicyReports : PageBase
{
    #region Page Varible's
    Int64 UniformIssuancePolicyID
    {
        get { return Convert.ToInt64(ViewState["UniformIssuancePolicyID"]); }
        set { this.ViewState["UniformIssuancePolicyID"] = value; }

    }
    Int64[] ar;
    #endregion 
    #region Page Event's

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Order Management System";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Issuance Policy Report";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Report/ReportDashBoard.aspx";

            //this is for setting search criteria
            if (!String.IsNullOrEmpty(Request.QueryString["UID"]))
                this.UniformIssuancePolicyID = Convert.ToInt64(Request.QueryString["UID"]);
            else
                ar = (Int64 [])HttpContext.Current.Session["arUID"];
            BindGrid();
        }
    }
    protected void lnkBtnEmails_Click(object sender, EventArgs e)
    {
        Boolean IsChecked = false;

        foreach (GridViewRow gvItem in gvUniformPolicy.Rows)
        {
            CheckBox chkItem = (CheckBox)gvItem.FindControl("chkSelectUser");

            if (chkItem != null && chkItem.Checked)
            {
                Label lblUserInfoID = (Label)gvItem.FindControl("lblUserID");
                Label lblEmail = (Label)gvItem.FindControl("lblEmail");
                Label lblName = (Label)gvItem.FindControl("lblFirstName");
                chkItem.Checked = false;
                SendVerificationEmail(Convert.ToInt64(lblUserInfoID.Text), lblEmail.Text, lblName.Text);
                IsChecked = true;
            }
        }
        if (IsChecked)
        {
            lblMsgGlobal.Text = "Mail send successfully";
        }
        else
        {
            lblMsgGlobal.Text = "Please select user to send Reminder ...";
        }

    }
    #endregion
    #region Page Method's

    private void BindGrid()
    {
        List<GetUniformIssuancePolicyForUsersResult> listUniIssPolicyForUsers = new ReportRepository().GetUniformIssuancePolicyForUsers(this.UniformIssuancePolicyID);
        if (listUniIssPolicyForUsers.Count > 0)
        {
            PopulateData(listUniIssPolicyForUsers[0]);
            gvUniformPolicy.DataSource = listUniIssPolicyForUsers;
            gvUniformPolicy.DataBind();
        }
    }

    private void PopulateData(GetUniformIssuancePolicyForUsersResult listUniIssPolicyForUsers)
    {
        lblIssuanceProgramName.Text = listUniIssPolicyForUsers.IssuanceProgramName;
        lblPolicyStatus.Text = listUniIssPolicyForUsers.PolicyStatus;
        lblPolicyDate.Text = Convert.ToDateTime(listUniIssPolicyForUsers.PolicyDate).ToShortDateString();
        lblPolicyEligibleDate.Text = Convert.ToDateTime(listUniIssPolicyForUsers.EligibleDate).ToShortDateString();
        lblExpireDate.Text = Convert.ToDateTime(listUniIssPolicyForUsers.CreditExpireDate).ToShortDateString();
    }
    /// <summary>
    /// Send Mail 
    /// </summary>
    /// <param name="UserInfoID"></param>
    /// <param name="UserEmail"></param>
    /// <param name="UserName"></param>
    private void SendVerificationEmail(Int64 UserInfoID, String UserEmail, String UserName)
    {
        try
        {
            string sFrmadd = "support@world-link.us.com";
            string sToadd = UserEmail.Trim();
            string sSubject = "Issaucnce Report";
            string sFrmname = "Incentex Order Processing Team";
            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();

            String eMailTemplate = String.Empty;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/SimpleMailFormat.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            string body = null;

            body = "This message is to inform you that you are eligible for to purchase this issueance policy.<br/><br/>";


            //Set Conformation Button

            body += "Thank you for your business!";
            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
            MessageBody.Replace("{MessageBody}", body);
            MessageBody.Replace("{Sender}", sFrmname);

            if (HttpContext.Current.Request.IsLocal)
            {
                new CommonMails().SendEmail4Local(1092, "Testing", "surendar.yadav@indianic.com", sSubject, MessageBody.ToString(), "incentextest6@gmail.com", "test6incentex", true);
            }
            else
                new CommonMails().SendMail(UserInfoID, "Issaunce Policy Report", sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, smtphost, smtpport, false, true);


        }
        catch (Exception ex)
        {
        }

    }
    #endregion

}
