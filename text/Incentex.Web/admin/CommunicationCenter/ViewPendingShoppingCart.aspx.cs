using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Configuration;

public partial class admin_CommunicationCenter_ViewPendingShoppingCart : PageBase
{
    #region Data Members

    CompanyRepository objCompanyRepos = new CompanyRepository();
    MyShoppingCartRepository objRepo = new MyShoppingCartRepository();
    
    #endregion

    Decimal TotalDollarValue = 0M;
    Int64 TotalPendings = 0;
    Int64 TotalUsers = 0; 

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Pending Shopping Carts";
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
            ((Label)Master.FindControl("lblPageHeading")).Text = "View Pending Shopping carts";

            bindRepeater();

            lblCount.Text = String.Format("Total Number of Users : {0} <br/> Total Number of Items-Orders : {1} <br/>  Total Value of Items : ${2}", TotalUsers.ToString(), TotalPendings.ToString(), TotalDollarValue.ToString("0,0.00"));

        }
    }

    protected void dtlCompany_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            List<MyShoppingCartRepository.SelectShoppingCartPendingOrder> objList = objRepo.GetAllAdminUserList(Convert.ToInt64(((HiddenField)e.Item.FindControl("hdnCompanyId")).Value), false);
            if (objList.Count > 0)
            {
                HiddenField hdnIamExpanded = (HiddenField)e.Item.FindControl("hdnIamExpanded");
                HiddenField hdnCompanyId = (HiddenField)e.Item.FindControl("hdnCompanyId");
                string[] ProductExpandedStatuses = Convert.ToString(Session["ExpandedStatuses"]).Split(',');

                if (ProductExpandedStatuses.Contains(hdnCompanyId.Value) || ProductExpandedStatuses.Contains(hdnCompanyId.Value + "|true"))
                    hdnIamExpanded.Value = "true";

                ((GridView)e.Item.FindControl("grdView")).DataSource = objList;
                ((GridView)e.Item.FindControl("grdView")).DataBind();
                
                ((Label)e.Item.FindControl("LblGrandTotal")).Text = (objList.Sum(x => x.ValueofCart)).ToString("0,0.00");
                TotalDollarValue = TotalDollarValue + objList.Sum(x => x.ValueofCart);
                ((HtmlControl)e.Item.FindControl("dvTotalDollarValue")).Visible = true;
              
            }
        }

    }


   


    protected void lnkBtnEmails_Click(object sender, EventArgs e)
    {
        bool IsChecked = false;
        //Loop through GridView rows to find checked rows 
        foreach (RepeaterItem repeaterItem in dtlCompany.Items)
        {
            GridView grdView = (GridView)repeaterItem.FindControl("grdView");
            for (int i = 0; i < grdView.Rows.Count; i++)
            {
                CheckBox chkSelectUser = (CheckBox)grdView.Rows[i].Cells[1].FindControl("chkSelectUser");
                if (chkSelectUser != null)
                {
                    if (chkSelectUser.Checked)
                    {
                        Label lblUserInfoID = (Label)grdView.Rows[i].Cells[0].FindControl("lblUserInfoID");
                        Label lblEmail = (Label)grdView.Rows[i].Cells[0].FindControl("lblEmail");
                        Label lblMyShoppingCartID = (Label)grdView.Rows[i].Cells[0].FindControl("lblMyShoppingCartID");
                        Label lblFirstName = (Label)grdView.Rows[i].Cells[0].FindControl("lblFirstName");
                        sendVerificationEmail(Convert.ToInt64(lblUserInfoID.Text), lblEmail.Text, lblFirstName.Text, lblMyShoppingCartID.Text);
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
    private void sendVerificationEmail(Int64 UserInfoID, String UserEmail, String UserName, String myShoppingCartID)
    {
        try
        {
            string sFrmadd = "support@world-link.us.com";
            string sToadd = UserEmail.Trim();
            string sSubject = "Incentex - Pending Items in Shopping Cart";
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

            body = "This message is to inform you that you have items in your cart and have not completed the checkout process for these items.  If you wish to complete the purchase of these items please click on the view cart button below. Log into the system and select \"My Cart\" to complete the process.<br/><br/>";

            body += "If you would like to delete the items in your cart please click on the button \"Delete Items\" and it will clear out your shopping cart.<br/>";

            //Set Conformation Button
            string buttonText = "<br/>";
            buttonText += "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
            buttonText += "<tr>";
            buttonText += "<td>";
            buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "My Cart/MyShoppinCart.aspx'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/viewcart_order_btn.png' alt='View Cart' border='0'/></a>";
            buttonText += "<td>";
            buttonText += "<td>";
            buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "admin/CommunicationCenter/DelMyShoppingCart.aspx?Id=" + myShoppingCartID + "&UserinfoID=" + UserInfoID + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/dlt_items_btn.png' alt='Delete items' border='0'/></a>";
            buttonText += "<td>";
            buttonText += "</tr>";
            buttonText += "</table><br/>";
            body += buttonText;

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
                new CommonMails().SendMail(UserInfoID, "Pending Shopping Cart", sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, smtphost, smtpport, false, true);


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

            Session["ExpandedStatuses"] = ExpandedStatuses;
        }
    }

    public void bindRepeater()
    {
        StoreProductExpandedStatuses();
        List<MyShoppingCartRepository.SelectCompanyPending> objList = objRepo.GetCompanyPendingShoppingCart(false);
        TotalPendings = objList.Sum(x => x.TotalPendingItems);
        TotalUsers = objRepo.GetUserCount(false);
        dtlCompany.DataSource = objList;
        dtlCompany.DataBind();
    }
    #endregion
}
