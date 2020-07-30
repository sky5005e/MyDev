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
using Incentex.DAL.SqlRepository;
using System.IO;
using Incentex.BE;
using Incentex.DAL;
using System.Collections.Generic;
using System.Text;

using Incentex.DAL.Common;  

public partial class admin_CommunicationCenter_CampDetail : PageBase
{
    int sent = 0;
    int MailView = 0;
    int Bouns = 0;
    Int32 campID
    {
        get
        {
            if (ViewState["campID"] == null)
            {
                ViewState["campID"] = 0;
            }
            return Convert.ToInt32(ViewState["campID"]);
        }
        set
        {
            ViewState["campID"] = value;
        }
    }
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
            base.MenuItem = "Communications Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Campaign Details";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/ViewCamp.aspx";

            if (Request.QueryString.Count > 0)
            {
                this.campID = Convert.ToInt32(Request.QueryString["Id"]);
                this.CompanyID = Convert.ToInt32(Request.QueryString["compID"]);
                // For Viewed 
                String urlWithParamsforView = "ViewTemplates.aspx?mailStatus=2&campid=" + this.campID + "&compid=" + this.CompanyID;
                btnViewed.Attributes.Add("OnClick", "window.open('" + urlWithParamsforView + "','PopupWindow','width=650, height=650, scrollbars=yes')");
                 
                // For Bounce
                String urlWithParamsforBounce = "ViewTemplates.aspx?mailStatus=0&campid=" + this.campID + "&compid=" + this.CompanyID;
                btnBounce.Attributes.Add("OnClick", "window.open('" + urlWithParamsforBounce + "','PopupWindow','width=650,height=650,scrollbars=yes')");
        
                CampignRepo ObjRepo = new CampignRepo();
                var list = ObjRepo.CampView(campID);
                // in this function it will get the list of all the all the user and check the value ans added sent , mail view ,mail bouns
                foreach (Incentex.DAL.SqlRepository.CampignRepo.SelectViewCamp user in list)
                {
                    txtCampaignName.Text = user.Campname;
                    //TxtSendDate.Text = Convert.ToString(user.CDate);
                    TxtNumOfUser.Text = user.CampTotalmail;
                    if (user.CampMailStatus == 1)
                    {
                        sent++;
                    }
                    if (user.CampMailStatus == 2)
                    {
                        MailView++;
                    }

                    if (user.CampMailStatus == 0)
                    {
                        Bouns++;
                    }
                
                }
                TxtViewd.Text = Convert.ToString(MailView);
                TxtNotOpen.Text = Convert.ToString(sent);
                TxtBounced.Text = Convert.ToString(Bouns);
                

            }
        }

    }
}
