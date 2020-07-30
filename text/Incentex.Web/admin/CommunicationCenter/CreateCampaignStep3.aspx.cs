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

public partial class admin_CommunicationCenter_CreateCampaignStep3 : PageBase
{
   
    private Int64 CompanyID
    {
        get
        {
            if (ViewState["CompanyID"] == null)
            {
                ViewState["CompanyID"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyID"]);
        }
        set
        {
            ViewState["CompanyID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Select Email Template";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            if (!String.IsNullOrEmpty(Session["cid"].ToString()))
            {
                this.CompanyID = Convert.ToInt64(Session["cid"].ToString());
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CreateCampaignStep2.aspx?cid=" + this.CompanyID;
            }
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CreateCampaignStep2.aspx";

        }
    }
   
    
    /// <summary>
    /// thisi is for the uploading the style sheet and insert the record into db.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkUploadStyleSheet_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateCampaignStep3-1.aspx");

    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateCampaignStep3-1.aspx");     
    }
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        if (this.CompanyID == 8)
            Response.Redirect("CreateCampaignStep2.aspx?cid=" + this.CompanyID);
        else
            Response.Redirect("CreateCampaignStep2.aspx");
    }
    protected void LnkQuickMsg_Click(object sender, EventArgs e)
    {
        Response.Redirect("QuickMsg.aspx");
    }
}
