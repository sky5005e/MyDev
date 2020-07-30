using System;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class CompanyStore_ViewFAQ : PageBase
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
            if (IncentexGlobal.IsIEFromStore)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            }

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
            //get workgroup by userinformationid
            CompanyEmployee objCmpnyInfo = new CompanyEmployee();
            CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
            objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

            //Get news by workgroup and companyid
            rptFAQ.DataSource = objRep.GetAllFAQsByUser(objCmpnyInfo.WorkgroupID, CompanyID);
            rptFAQ.DataBind();

            //foreach (RepeaterItem item in rptFAQ.Items)
            //{
            //    AjaxControlToolkit.CollapsiblePanelExtender objCollPnlExt = new AjaxControlToolkit.CollapsiblePanelExtender();

            //    objCollPnlExt = (AjaxControlToolkit.CollapsiblePanelExtender)item.FindControl("CollapsiblePanelExtender1");
            //    objCollPnlExt.Collapsed = true;
            //}
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    #endregion

}
