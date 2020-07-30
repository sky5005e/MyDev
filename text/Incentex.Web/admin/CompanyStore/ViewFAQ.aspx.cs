using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using Incentex.BE;
using Incentex.DA;

public partial class admin_CompanyStore_ViewFAQ : PageBase
{
    CompanyStoreDocumentRepository objRep = new CompanyStoreDocumentRepository();

    Int64 CompanyID;
    Int64 UserInfoID;
    
    #region Page Load Event

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Redirect user to login page if session gets expires
            CheckLogin();

            //Assign Page Header and return URL 
            ((Label)Master.FindControl("lblPageHeading")).Text = "FAQ";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";

            //Retrieve UserID and CompanyID
            //UserInfoID = Int64.Parse(Request.QueryString["u"]);
            //CompanyID = Int64.Parse(Request.QueryString["c"]);
            UserInfoID = Int64.Parse(Session["UserInfoID"].ToString());
            CompanyID = Int64.Parse(Session["CompanyID"].ToString());

            //Load News
            LoadFAQ();
        }
    }

    #endregion

    #region Misc Functions

    private void LoadFAQ()
    {
        try
        {
            rptFAQ.DataSource = objRep.GetAllFAQsByUser(UserInfoID, CompanyID);
            rptFAQ.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    #endregion
}
