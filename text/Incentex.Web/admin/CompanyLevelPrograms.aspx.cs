using System;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyLevelPrograms : PageBase
{
    Int64 WorkGroupId
    {
        get
        {
            if (ViewState["WorkGroupId"] == null)
            {
                ViewState["WorkGroupId"] = 0;
            }
            return Convert.ToInt64(ViewState["WorkGroupId"]);
        }
        set
        {
            ViewState["WorkGroupId"] = value;
        }
    }
    Int64 CompanyId
    {
        get
        {
            if (ViewState["CompanyId"] == null)
            {
                ViewState["CompanyId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyId"]);
        }
        set
        {
            ViewState["CompanyId"] = value;
        }
    }
    Int64 CompanyStoreId
    {
        get
        {
            if (ViewState["CompanyStoreId"] == null)
            {
                ViewState["CompanyStoreId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {
            this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
            this.WorkGroupId = Convert.ToInt64(Request.QueryString.Get("workgroupid"));
            ((Label)Master.FindControl("lblPageHeading")).Text = "Gloabal Setting For Special Programs";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyLevelGlobalSetting.aspx?id=" + this.CompanyId;
            getcompanyporgramfortheworkgroup();
        }
    }

    public void getcompanyporgramfortheworkgroup()
    {

        CompanyStoreRepository objCompanyStore = new CompanyStoreRepository();
        AnniversaryCreditProgram objProgram = new AnniversaryCreditProgram();
        AnniversaryProgramRepository objAnniversaryProgramRepo = new AnniversaryProgramRepository();
        this.CompanyStoreId = new CompanyStoreRepository().GetStoreIDByCompanyId(this.CompanyId);
        objProgram = objAnniversaryProgramRepo.GetProgByStoreIdWorkgroupId(this.WorkGroupId,this.CompanyStoreId);
        if (objProgram != null)
        {
            NoProgram.Text = "Standard Program value for the user who is elligible for the program will get the amount is : " + objProgram.StandardCreditAmount;
            //CompanyStore objStore = objCompanyStore.GetById(this.CompanyStoreId);
            ////Update CreditAmountToApplied from the company employee table
            //UserInformationRepository objUserInfoRep = new UserInformationRepository();
            //List<selectusersuploadfromexcelsheetResult> objEmpl = new List<selectusersuploadfromexcelsheetResult>();
            //objEmpl = objUserInfoRep.GetNewUserInformation(this.CompanyId, this.WorkGroupId);
            //foreach (selectusersuploadfromexcelsheetResult peremployee in objEmpl)
            //{
               
            //}
        }
        else
        {
            NoProgram.Text = "There is no program created for selected workgroup..Please create one";
            return;
        }
        //Get program details

        //Get users elligible for the credit amount to apply
    }

    protected void lnkBtnGetInformation_Click(object sender, EventArgs e)
    {

    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        if (ddlEmployeePolicyStatus.SelectedValue != "0")
        {
            Session["uniformissuancepolicystatusid"] = ddlEmployeePolicyStatus.SelectedValue;
        }
        Response.Redirect("CompanyLevelFinalStep.aspx?id=" + this.CompanyId + "&workgroupid=" + this.WorkGroupId.ToString());
    }
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("CompanyLevelStoreSetting.aspx?id=" + this.CompanyId + "&workgroupid=" + this.WorkGroupId);
    }
}
