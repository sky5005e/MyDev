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
using System.IO;
using System.Text;
using commonlib.Common;
public partial class UserStatusChange_ChangeStatus : PageBase
{
    #region Data Members
    CompanyRepository objCompanyRepo = new CompanyRepository();
    LookupRepository objLookRep = new LookupRepository();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    DataSet ds;
    CompanyEmployeeBE objCE = new CompanyEmployeeBE();
    CompanyEmployeeDA objCeDa = new CompanyEmployeeDA();
    UserInformationRepository objUserInforepo = new UserInformationRepository();
    UniformIssuancePolicyRepository objUniformIssuancePolicyRepository = new UniformIssuancePolicyRepository();
    MyIssuanceCartRepository objMyIssRepo = new MyIssuanceCartRepository();
    ChangeStatusRepository objChangeStatusRepo = new ChangeStatusRepository();
    SubCatogeryRepository objSubCateRepo = new SubCatogeryRepository();
    // All selected Department 
    String NewEmployeeAccessId
    {
        get { return Convert.ToString(ViewState["NewEmployeeAccessId"]); }
        set { ViewState["NewEmployeeAccessId"] = value; }
    }
    // Pilot Emp Type
    String PilotEmpType
    {
        get { return Convert.ToString(ViewState["PilotEmpType"]); }
        set { ViewState["PilotEmpType"] = value; }
    }
    #endregion
    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Change User Status";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";

            if (!IsPostBack)
            {
                lblMsg.Text = "";
                BindValues();

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                {

                    ddlCompany.SelectedValue = Convert.ToString(IncentexGlobal.CurrentMember.CompanyId);
                    ddlCompany.Enabled = false;
                }
                else
                {
                    ddlCompany.SelectedIndex = 0;
                    ddlCompany.Enabled = true;
                }
                dvPolMsg.Visible = false;
            }
        }
        catch (Exception)
        {


        }
    }
    protected void ddlWorkgroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWorkgroup.SelectedIndex != 0)
            FillEmployees();
    }
    protected void lnkBtnNext1_Click(object sender, EventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            dvFirst.Visible = false;
            dvSecond.Visible = true;
            dvThird.Visible = false;
            this.NewEmployeeAccessId = null;
            txtEmployee.Text = ddlEmployee.SelectedItem.Text;
            UserInformation objUserInfo = objUserInforepo.GetById(Convert.ToInt64(ddlEmployee.SelectedValue));
            ddlEmployeeStatus.SelectedValue = objUserInfo.WLSStatusId != null ? objUserInfo.WLSStatusId.ToString() : "0";

            CompanyEmployee objEmpType = objCmpnyEmpRepo.GetByUserInfoId(Convert.ToInt64(ddlEmployee.SelectedValue));
            
            ddlNewWorkgroup.SelectedValue = Convert.ToString(objEmpType.WorkgroupID);
            if (objEmpType.EmployeeTypeID != null)
            {
                ddlEmployeeType.SelectedValue = Convert.ToString(objEmpType.EmployeeTypeID);
                if (ddlEmployeeType.SelectedItem.Text == "First Officer")
                    this.PilotEmpType = "First Officer";
                else
                    this.PilotEmpType = "No";
            }
            this.NewEmployeeAccessId = objEmpType.EmployeeUniformAccess;
        }
        catch (Exception)
        {
        }
    }
    
    protected void lnkBtnNext2_Click(object sender, EventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            dvFirst.Visible = false;
            dvSecond.Visible = false;
            dvThird.Visible = true;
            if (!String.IsNullOrEmpty(hdnListofEmployeeUniformID.Value))
            {
                this.NewEmployeeAccessId = Convert.ToString(this.hdnListofEmployeeUniformID.Value);
                this.NewEmployeeAccessId = this.NewEmployeeAccessId.Remove(NewEmployeeAccessId.Length - 1);
            }
           
            txtEmployeeUniform.Text = objChangeStatusRepo.GetAlldepartmentList(NewEmployeeAccessId);
            txtEmployeeUniform.ToolTip = txtEmployeeUniform.Text;
            txtConfEmployee.Text = txtEmployee.Text;
            txtConfWorkgroup.Text = ddlNewWorkgroup.SelectedItem.Text;
            txtConfEmployeeType.Text = ddlEmployeeType.SelectedItem.Text;
            txtConfIssuancePolicy.Text = ddlIssuancePolicy.SelectedItem.Text;
            txtConfEmployeeStatus.Text = ddlEmployeeStatus.SelectedItem.Text;
            dvUniformIssuancePolicies.Visible = true;
            BindIssuancePolicies(Convert.ToInt64(ddlEmployee.SelectedValue), Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId),Convert.ToInt64(ddlNewWorkgroup.SelectedValue),Convert.ToInt64(ddlEmployeeType.SelectedValue));
        }
        catch (Exception)
        {


        }
    }
    protected void lnkActivate_Click(object sender, EventArgs e)
    {
        dvPolMsg.Visible = true;
        bool IsChecked = false;
        foreach (GridViewRow gvItem in gvUniIssuancePolicy.Rows)
        {
            CheckBox chkchild = (CheckBox)gvItem.FindControl("chkchild");
            Label lblID = (Label)gvItem.FindControl("lblID");
            if (chkchild != null && chkchild.Checked)
            {
                objChangeStatusRepo.SaveRecordsForReActivatedPolicy(Convert.ToInt64(ddlEmployee.SelectedValue), Convert.ToInt64(lblID.Text), IncentexGlobal.CurrentMember.UserInfoID);
                chkchild.Checked = false;
                IsChecked = true;
            }
        }
        if (IsChecked)
        {
           // sendVerificationEmail(objUserInfo.LoginEmail, FullName, Convert.ToInt64(ddlEmployee.SelectedValue), GetAllIssuancePoliciesName());
            lblmsgPolicy.Text = "Policies are re-activated successfully";
        }
        else
            lblmsgPolicy.Text = "Please select policy to re-activate";

    }
   
    protected void lnkProcess_Click(object sender, EventArgs e)
    {
        try
        {
            objChangeStatusRepo.UpdateUserStatus(Convert.ToInt64(ddlEmployee.SelectedValue), Convert.ToChar(ddlIssuancePolicy.SelectedValue), Convert.ToInt64(ddlEmployeeType.SelectedValue), Convert.ToInt64(ddlEmployeeStatus.SelectedValue), IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(ddlNewWorkgroup.SelectedValue), this.NewEmployeeAccessId);
            UserInformation objUserInfo = objUserInforepo.GetById(Convert.ToInt64(ddlEmployee.SelectedValue));

            String FullName = objUserInfo.FirstName + " " + objUserInfo.LastName;
            String _policiesName = GetAllPublishIssuancePoliciesName();
            Boolean IsFOupgCaptaion = false;
            if(ddlCompany.SelectedItem.Text == "Spirit Airlines" && this.PilotEmpType =="First Officer" &&  txtConfEmployeeType.Text == "Captain ")
                IsFOupgCaptaion = true;
            else
                IsFOupgCaptaion = false;
            sendVerificationEmail(objUserInfo.LoginEmail, FullName, Convert.ToInt64(ddlEmployee.SelectedValue), _policiesName, IsFOupgCaptaion);
            lblMsg.Text = "Process Complete";
            FillWorkgroup();
            dvFirst.Visible = true;
            dvSecond.Visible = false;
            dvThird.Visible = false;
            ResetControl();
        }
        catch (Exception)
        {
            lblMsg.Text = "Please Check all Entries";
        }
    }
    protected void lnkBtnBack1_Click(object sender, EventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            dvFirst.Visible = true;
            dvSecond.Visible = false;
            dvThird.Visible = false;
            ResetControl();
        }
        catch (Exception)
        {

        }
    }
    protected void lnkBtnBack2_Click(object sender, EventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            dvFirst.Visible = false;
            dvSecond.Visible = true;
            dvThird.Visible = false;
            ResetControl();
        }
        catch (Exception)
        {

        }
    }
    protected void gvUniIssuancePolicy_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblID = (Label)e.Row.FindControl("lblID");
            Label lblOrderStatus = (Label)e.Row.FindControl("lblOrderStatus");
            CheckBox chkchild = (CheckBox)e.Row.FindControl("chkchild");
            // Check If this policy is already re-activated and still pending for order.
            //Boolean IsAllowForActivation = objChangeStatusRepo.IsUserReActivatedPolicyExist(Convert.ToInt64(ddlEmployee.SelectedValue), Convert.ToInt64(lblID.Text));
            //if (IsAllowForActivation)
            //    chkchild.Visible = false;

            Boolean IsOrderPlaced = objMyIssRepo.IsOrderPlacedByUser(Convert.ToInt64(lblID.Text), Convert.ToInt64(ddlEmployee.SelectedValue));
            if (IsOrderPlaced)
                lblOrderStatus.Text = "Order Placed";
            else
                lblOrderStatus.Text = "Not used";

            HiddenField hdnPolicyExpiryDate = (HiddenField)e.Row.FindControl("hdnPolicyExpiryDate"); //CreditExpireDate
            DateTime PolicyExpiryDate = DateTime.Now.AddDays(-1);// Refer last day.
            if (!String.IsNullOrEmpty(hdnPolicyExpiryDate.Value))
                PolicyExpiryDate = Convert.ToDateTime(hdnPolicyExpiryDate.Value);
            Label lblPolicyStatus = (Label)e.Row.FindControl("lblPolicyStatus");
            if (PolicyExpiryDate < DateTime.Now)
                lblPolicyStatus.Text = "Expire";
            else
                lblPolicyStatus.Text = "Active";

        }
    }
    #endregion
    #region Methods
    /// <summary>
    /// Get All Polcies name from Gridview
    /// </summary>
    /// <returns></returns>
    private String GetAllPublishIssuancePoliciesName()
    {
        Boolean IsChecked = true;//
        String policyName = String.Empty;
        if (gvUniIssuancePolicy.Rows.Count > 0)
        {
            policyName = "<BR>New Policies are :<ul>";
            foreach (GridViewRow gvItem in gvUniIssuancePolicy.Rows)
            {
                CheckBox chkchild = (CheckBox)gvItem.FindControl("chkchild");
                Label lblID = (Label)gvItem.FindControl("lblID");
                Label lblIssuanceProgramName = (Label)gvItem.FindControl("lblIssuanceProgramName");
                if (chkchild != null && chkchild.Checked)
                {
                    if (IsChecked)
                    {
                        objChangeStatusRepo.UpdateExistingPublishIssuancePolicy(Convert.ToInt64(ddlEmployee.SelectedValue));
                        IsChecked = false;
                    }
                    objChangeStatusRepo.SaveRecordsForUserPublishIssuancePolicy(Convert.ToInt64(ddlEmployee.SelectedValue), Convert.ToInt64(lblID.Text), IncentexGlobal.CurrentMember.UserInfoID);
                    if (!String.IsNullOrEmpty(lblIssuanceProgramName.Text))
                    {
                        policyName += "<li>" + lblIssuanceProgramName.Text + "</li>";
                    }
                }
            }
            policyName += "</ul>";
        }
        return policyName;
    }
    public void BindValues()
    {
        try
        {
            //get company

            List<Company> objCompanyList = new List<Company>();
            objCompanyList = objCompanyRepo.GetAllCompany();
            Common.BindDDL(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "-Select Company-");

            //get workgroup
            FillWorkgroup();

            //get Status
            LookupRepository objLookRep = new LookupRepository();
            ddlEmployeeStatus.DataSource = objLookRep.GetByLookup("status");
            ddlEmployeeStatus.DataValueField = "iLookupID";
            ddlEmployeeStatus.DataTextField = "sLookupName";
            ddlEmployeeStatus.DataBind();

            BindEmployeeUniformAccess();
            //Employee Type           
            FillEmployeeType();


        }
        catch (Exception)
        {


        }
    }
    private void BindEmployeeUniformAccess()
    {
        //get EmployeeUniformAccess
        List<SubCategory> objSubcateList = objSubCateRepo.GetAllSubCategory(1).OrderBy(o => o.SubCategoryName).ToList();
        DataTable dt = Common.ListToDataTable(objSubcateList);
        this.drpMulti.DataSource = dt;
        this.drpMulti.DataTextField = "SubCategoryName";
        this.drpMulti.DataValueField = "SubCategoryID";
        this.drpMulti.DataBind();
    }
    private void FillWorkgroup()
    {
        try
        {
            List<INC_Lookup> objINC_LookupList = new List<INC_Lookup>();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            {
                CompanyEmployee objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

                List<AdditionalWorkgroup> additionaWorkgroupList = new AdditionalWorkgroupRepository().GetManageDetailName(Convert.ToInt32(objCmpnyInfo.CompanyEmployeeID), Convert.ToInt32(IncentexGlobal.CurrentMember.CompanyId));
                List<Int64> workgroupList = new List<Int64>();
                workgroupList.Add((Int64)objCmpnyInfo.ManagementControlForWorkgroup);
                for (int i = 0; i < additionaWorkgroupList.Count; i++)
                    workgroupList.Add((Int64)additionaWorkgroupList[i].WorkgroupID);

                objINC_LookupList = objLookRep.GetByLookupWorkgroupList("Workgroup", workgroupList);
            }
            else
                objINC_LookupList = objLookRep.GetByLookup("Workgroup");

            //Fill Workgroup dropdown
            ddlWorkgroup.DataSource = objINC_LookupList;
            ddlWorkgroup.DataTextField = "sLookupName";
            ddlWorkgroup.DataValueField = "iLookupID";
            ddlWorkgroup.DataBind();
            ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));

            //Fill New Workgroup dropdown
            ddlNewWorkgroup.DataSource = objINC_LookupList;
            ddlNewWorkgroup.DataTextField = "sLookupName";
            ddlNewWorkgroup.DataValueField = "iLookupID";
            ddlNewWorkgroup.DataBind();
            ddlNewWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));

                        
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    private void FillEmployees()
    {
        try
        {
            List<UserInformationRepository.AllUser> objEmployee = new List<UserInformationRepository.AllUser>();
            Int64 CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            Int64 WorkGrpID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            objEmployee = objUserInforepo.GetEmployeesByWorkGrp(CompanyID, WorkGrpID);
            Common.BindDDL(ddlEmployee, objEmployee, "UserName", "UserInfoID", "-Select Employee-");
        }
        catch (Exception)
        { }
    }
    private void ResetControl()
    {
        Control hdnHsiv = drpMulti.FindControl("hsiv");
        HiddenField uchdn = (HiddenField)hdnHsiv;
        uchdn.Value = "";
        this.NewEmployeeAccessId = null;
        this.hdnListofEmployeeUniformID = null;
        dvUniformIssuancePolicies.Visible = false;
        dvUniformIssuancePolicies.Visible = false;
        gvUniIssuancePolicy.DataSource = null;
        gvUniIssuancePolicy.DataBind();
        drpMulti.DataSource = null;
        drpMulti.DataBind();
        BindEmployeeUniformAccess();
    }
    private void BindIssuancePolicies(Int64 userID,Int64 companyId,Int64 workgroupID, Int64 empTypeID)
    {
        INC_Lookup obklookup = new INC_Lookup();

        List<AdditionalWorkgroup> objAddWorkgroupList = new List<AdditionalWorkgroup>();
        AdditionalWorkgroupRepository objAddWorkgroupRepos = new AdditionalWorkgroupRepository();

        CompanyEmployee objCmpnyInfo = new CompanyEmployee();
        CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
        objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(userID);
        if (objCmpnyInfo != null)
        {
            //Check Here Status of Issuance Polciy in Special Program Active or Inactive
            int ActiveInactive = Convert.ToInt32(objCmpnyInfo.EmpIssuancePolicyStatus);
            obklookup = objLookRep.GetById(ActiveInactive);
            if (obklookup != null)
            {
                if (obklookup.sLookupName == "Active")
                {
                    obklookup = objLookRep.GetById(objCmpnyInfo.GenderID);
                    long strgendertype = obklookup.iLookupID;
                    if (strgendertype != 0)
                    {
                        List<UniformIssuancePolicy> objList = objUniformIssuancePolicyRepository.GetByCompanyIdandWorkgroupId(companyId, workgroupID, strgendertype, empTypeID);
                        List<UniformIssuancePolicy> objAddTotalList = new List<UniformIssuancePolicy>();
                        int addWorkgroupID = 0;
                        objAddWorkgroupList = objAddWorkgroupRepos.GetWorkgroupName(Convert.ToInt32(userID), Convert.ToInt32(companyId));

                        if (objAddWorkgroupList.Count > 0)
                        {
                            foreach (var p in objAddWorkgroupList)
                            {
                                addWorkgroupID = Convert.ToInt32(p.WorkgroupID);
                                List<UniformIssuancePolicy> objUIP = objUniformIssuancePolicyRepository.GetByCompanyIdandWorkgroupId(companyId, addWorkgroupID, strgendertype, empTypeID);
                                if (objUIP.Count > 0)
                                {
                                    objAddTotalList.AddRange(objUIP);
                                }
                            }
                        }

                        objList.AddRange(objAddTotalList);
                        gvUniIssuancePolicy.DataSource = objList;
                        gvUniIssuancePolicy.DataBind();
                    }
                }
            }
        }
    }
    public void FillEmployeeType()
    {
        LookupRepository objTitleLookRep = new LookupRepository();
        string strPayment = "EmployeeType ";
        ddlEmployeeType.DataSource = objTitleLookRep.GetByLookup(strPayment);
        ddlEmployeeType.DataValueField = "iLookupID";
        ddlEmployeeType.DataTextField = "sLookupName";
        ddlEmployeeType.DataBind();
        ddlEmployeeType.Items.Insert(0, new ListItem("-select employee type-", "0"));
    }
    private void sendVerificationEmail(String UserEmail, String UserName, Int64 userInfoID, String polices, Boolean IsUPGPilottoCaptain)
    {
        try
        {
            string sFrmadd = IncentexGlobal.CurrentMember.LoginEmail;
            string sToadd = UserEmail.Trim();            
            string sFrmname = IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName;
            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();
            string sSubject = "User Status Changed";
            string sBody = "Your User Status Changed to: </BR> Workgroup: " + ddlNewWorkgroup.SelectedItem.Text + " </BR> Employee Type: " + ddlEmployeeType.SelectedItem.Text + " </BR> Employee Status: " + ddlEmployeeStatus.SelectedItem.Text + " </BR> Issuance Status: " + ddlIssuancePolicy.SelectedItem.Text +" </BR>" +polices;

            String eMailTemplate = String.Empty;

            StreamReader _StreamReader;
            if (!IsUPGPilottoCaptain)
                _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/SimpleMailFormat.htm"));
            else
                _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/CaptainUpgrade.html"));

            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
            MessageBody.Replace("{MessageBody}", sBody.ToString());
            MessageBody.Replace("{Sender}", sFrmname);
            //Live server Message
            new CommonMails().SendMail(userInfoID, "Emp Changes Status", sFrmadd, sToadd, sSubject, MessageBody.ToString(), Common.DisplyName, Common.SMTPHost, Common.SMTPPort, false, true);
            //Local testing email settings
            //if (HttpContext.Current.Request.IsLocal)
            //    new CommonMails().SendEmail4Local(1092,"Test Changes Status","surendar.yadav@indianic.com", sSubject, MessageBody.ToString(),"incentextest6@gmail.com", "test6incentex", true);

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    #endregion
}
