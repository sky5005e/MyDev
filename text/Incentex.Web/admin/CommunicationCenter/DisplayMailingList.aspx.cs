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
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL.Common;

public partial class admin_CommunicationCenter_DisplayMailingList : PageBase
{
    Campaign ObjCamp = new Campaign();
    CampignRepo ObjRepo = new CampignRepo();
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        ((Label)Master.FindControl("lblPageHeading")).Text = "Send Mail";
        ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
        ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CampaignSelection.aspx";
        if(!IsPostBack)
        {
            int Count = Convert.ToInt32(Session["SendMail"]);
            Label1.Text = "mailing has been sent and the number of emails we have sent: " + Count.ToString();
            ObjCamp = ObjRepo.GetDetailFromCampID(Convert.ToInt32(Session["CampID"]));
            ObjCamp.IsComplete = true;
            ObjRepo.SubmitChanges();
        }

    }
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateCampaignStep5.aspx");
    }
}
