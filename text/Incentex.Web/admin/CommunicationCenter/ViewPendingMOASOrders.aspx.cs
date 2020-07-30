using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;

public partial class admin_CommunicationCenter_ViewPendingMOASOrders : PageBase
{
    #region Data Members

    CompanyRepository objCompanyRepos = new CompanyRepository();
    OrderMOASSystemRepository objRepo = new OrderMOASSystemRepository();
    Incentex.DAL.Common.DAEnums.SortOrderType SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
            }
            return (Incentex.DAL.Common.DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }
    /// <summary>
    /// To check whether request if for user pending or order pending
    /// Set true when requested for pending order 
    /// </summary>
    Boolean IsOrderReq
    {
        get
        {
            if (ViewState["IsOrderReq"] == null)
            {
                ViewState["IsOrderReq"] = 0;
            }
            return Convert.ToBoolean(ViewState["IsOrderReq"]);
        }
        set
        {
            ViewState["IsOrderReq"] = value;
        }
    }
    #endregion

    Decimal TotalDollarValue = 0M;
    Int64 TotalPendings = 0;

    #region Event Handlers

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["req"]) && Request.QueryString["req"] == "order")
            {
                base.MenuItem = "Pending MOAS Orders";
                ((Label)Master.FindControl("lblPageHeading")).Text = "View Pending MOAS Orders";
                this.IsOrderReq = true;
            }
            else if (Request.QueryString["req"] == "user")
            {
                base.MenuItem = "Pending Users";
                ((Label)Master.FindControl("lblPageHeading")).Text = "View Pending Users";
                this.IsOrderReq = false;
            }
            else
                Response.Redirect("CampaignSelection.aspx");

            base.ParentMenuID = 29;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CampaignSelection.aspx";

            bindRepeater();
            if (this.IsOrderReq)
                lblCount.Text = String.Format("Total Pending MOAS Orders : {0} <br/>  Total Value of MOAS Orders: ${1}", TotalPendings.ToString(), TotalDollarValue.ToString("0,0.00"));
            else
                lblCount.Text = String.Format("Total Pending Users : {0}", TotalPendings.ToString());
        }
    }

    protected void dtlCompany_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            // For MOAS Pending Order
            if (IsOrderReq)
            {
                List<OrderMOASSystemRepository.SelectAdminMOASOrdersPending> objList = objRepo.GetAllAdminUserList(Convert.ToInt64(((HiddenField)e.Item.FindControl("hdnCompanyId")).Value), "Order Pending");
                if (objList.Count > 0)
                {
                    HiddenField hdnIamExpanded = (HiddenField)e.Item.FindControl("hdnIamExpanded");
                    HiddenField hdnCompanyId = (HiddenField)e.Item.FindControl("hdnCompanyId");
                    String[] ProductExpandedStatuses = Convert.ToString(Session["ProductExpandedStatuses"]).Split(',');

                    if (ProductExpandedStatuses.Contains(hdnCompanyId.Value) || ProductExpandedStatuses.Contains(hdnCompanyId.Value + "|true"))
                        hdnIamExpanded.Value = "true";

                    ((GridView)e.Item.FindControl("grdView")).DataSource = objList;
                    ((GridView)e.Item.FindControl("grdView")).DataBind();
                    ((GridView)e.Item.FindControl("grdView")).Columns[4].Visible = true;//Show TotalDollar Value column
                    ((Label)e.Item.FindControl("LblGrandTotal")).Text = (objList.Sum(x => x.TotalDollarValue)).ToString("0,0.00");
                    TotalDollarValue = TotalDollarValue + objList.Sum(x => x.TotalDollarValue);
                    ((HtmlControl)e.Item.FindControl("dvTotalDollarValue")).Visible = true;
                    ((GridView)e.Item.FindControl("grdView")).Columns[6].Visible = false;// To hide it when Page is call from Pending MOAS Order
                }
                else
                {
                    ((Label)e.Item.FindControl("lblMsg")).Text = "No Company Admin Assigned to above pending users.";
                }
            }
            else
            {
                List<OrderMOASSystemRepository.SelectAdminMOASOrdersPending> objListSP = objRepo.GetAllAdminPendingUserList(Convert.ToInt64(((HiddenField)e.Item.FindControl("hdnCompanyId")).Value));
                if (objListSP.Count > 0)
                {
                    HiddenField hdnIamExpanded = (HiddenField)e.Item.FindControl("hdnIamExpanded");
                    HiddenField hdnCompanyId = (HiddenField)e.Item.FindControl("hdnCompanyId");
                    String[] ProductExpandedStatuses = Convert.ToString(Session["ProductExpandedStatuses"]).Split(',');

                    if (ProductExpandedStatuses.Contains(hdnCompanyId.Value) || ProductExpandedStatuses.Contains(hdnCompanyId.Value + "|true"))
                        hdnIamExpanded.Value = "true";

                    ((GridView)e.Item.FindControl("grdView")).DataSource = objListSP;
                    ((GridView)e.Item.FindControl("grdView")).DataBind();
                    ((GridView)e.Item.FindControl("grdView")).Columns[5].Visible = false;//Hide TotalDollar Value column
                    ((GridView)e.Item.FindControl("grdView")).Columns[6].Visible = true;// To show it when Page is call from Pending User
                    ((GridView)e.Item.FindControl("grdView")).Columns[4].Visible = false;// To hide it when Page is call from Pending User
                }
                else
                {
                    ((Label)e.Item.FindControl("lblMsg")).Text = "No Company Admin Assigned to above pending users.";
                }

            }
        }

    }

    protected void grdView_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblUserInfoID = (Label)e.Row.FindControl("lblUserInfoID");
            List<OrderMOASSystemRepository.SelectOrderDetails> objList = objRepo.GetOrderDetialsByUserInfoID(Convert.ToInt64(lblUserInfoID.Text), "Order Pending");
            GridView gridViewNested = (GridView)e.Row.FindControl("nestedGridView");
            gridViewNested.DataSource = objList;
            gridViewNested.DataBind();
        }
    }


    protected void lnkBtnEmails_Click(Object sender, EventArgs e)
    {
        Boolean IsChecked = false;
        //Loop through GridView rows to find checked rows 
        foreach (RepeaterItem repeaterItem in dtlCompany.Items)
        {
            GridView grdView = (GridView)repeaterItem.FindControl("grdView");
            for (Int32 i = 0; i < grdView.Rows.Count; i++)
            {
                CheckBox chkSelectUser = (CheckBox)grdView.Rows[i].Cells[1].FindControl("chkSelectUser");
                if (chkSelectUser != null)
                {
                    if (chkSelectUser.Checked)
                    {
                        Label lblUserInfoID = (Label)grdView.Rows[i].Cells[0].FindControl("lblUserInfoID");
                        Label lblEmail = (Label)grdView.Rows[i].Cells[0].FindControl("lblEmail");
                        Label lblTotal = (Label)grdView.Rows[i].Cells[0].FindControl("lblTotal");
                        Label lblFirstName = (Label)grdView.Rows[i].Cells[0].FindControl("lblFirstName");
                        sendVerificationEmail(Convert.ToInt32(lblTotal.Text), Convert.ToInt64(lblUserInfoID.Text), lblEmail.Text, lblFirstName.Text, true);
                        chkSelectUser.Checked = false;
                        IsChecked = true;
                    }
                }
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

    #region Methods
    private void sendVerificationEmail(Int32 Xpending, Int64 UserInfoID, String UserEmail, String UserName, Boolean Isorder)
    {
        try
        {
            String sFrmadd = "support@world-link.us.com";
            String sToadd = UserEmail.Trim();
            String sSubject = null;
            String sFrmname = null;
            if (IsOrderReq)
            {
                sSubject = "Incentex Message - You have " + Xpending + " Pending Orders Waiting for your Review";
                sFrmname = "Incentex Order Processing Team";
            }
            else
            {
                sSubject = Xpending + " Pending Users";
                sFrmname = "Incentex Management";
            }

            String smtphost = Application["SMTPHOST"].ToString();
            Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            String smtpUserID = Application["SMTPUSERID"].ToString();
            String smtppassword = Application["SMTPPASSWORD"].ToString();

            String eMailTemplate = String.Empty;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/SimpleMailFormat.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            String body = null;
            if (IsOrderReq)
            {
                body = "This email message is to inform you of " + Xpending + " pending order(s) for your review in the system.<br/><br/>";
                body += "Please note you can click on the button below called \"View Pending Orders\" and it will bring you directly to your approval page to review these orders.<br/>";

                //Set View Button
                String buttonText = "<br/>";
                buttonText += "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                buttonText += "<tr>";
                buttonText += "<td>";
                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MyAccount/OrderManagement/MyPendingOrders.aspx?IsFormEmail=" + true + "&UserInfoID=" + UserInfoID + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/view_pending_btn.png' alt='View Pending Orders' border='0'/></a>";
                buttonText += "<td>";
                buttonText += "</tr>";
                buttonText += "</table><br/>";
                body += buttonText;

                body += "Thank you for your business!";
            }
            else
            {
                body = "This email is to notify you that you have " + Xpending + " users pending requesting your review. Please login to the system to review www.incentex.com<br/><br/>";
                body += "Thank you";
            }

            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
            MessageBody.Replace("{MessageBody}", body);
            MessageBody.Replace("{Sender}", sFrmname);
            //
            if (HttpContext.Current.Request.IsLocal)
            {
                new CommonMails().SendEmail4Local(UserInfoID, "Testing", "surendar.yadav@indianic.com", sSubject, MessageBody.ToString(), "incentextest6@gmail.com", "test6incentex", true);
            }
            else
                new CommonMails().SendMail(UserInfoID, null, sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, smtphost, smtpport, false, true);

        }
        catch (Exception ex)
        {
        }

    }
    private void StoreProductExpandedStatuses()
    {
        if (dtlCompany.Items.Count > 0)
        {
            String ExpandedStatuses = String.Empty;
            foreach (RepeaterItem repItem in dtlCompany.Items)
            {
                HiddenField hdnCompanyId = (HiddenField)repItem.FindControl("hdnCompanyId");
                HiddenField hdnIamExpanded = (HiddenField)repItem.FindControl("hdnIamExpanded");

                if (hdnIamExpanded != null && hdnCompanyId != null)
                {
                    if (Convert.ToBoolean(hdnIamExpanded.Value))
                    {
                        if (String.IsNullOrEmpty(ExpandedStatuses))
                            ExpandedStatuses = hdnCompanyId.Value;
                        else
                            ExpandedStatuses += "," + hdnCompanyId.Value;
                    }
                }
            }

            Session["ProductExpandedStatuses"] = ExpandedStatuses;
        }
    }

    public void bindRepeater()
    {
        StoreProductExpandedStatuses();
        List<OrderMOASSystemRepository.SelectCompanyPending> objList;
        if (IsOrderReq)// For pending orders 
        {
            objList = objRepo.GetCompanyPendingList("Order Pending");
            TotalPendings = objList.Sum(x => x.TotalPending);
        }
        else // for Pending users
        {
            objList = objRepo.GetCompanyPendingList("pending");
            TotalPendings = objList.Sum(x => x.TotalPending);
        }
        dtlCompany.DataSource = objList;
        dtlCompany.DataBind();
    }
    #endregion
}
