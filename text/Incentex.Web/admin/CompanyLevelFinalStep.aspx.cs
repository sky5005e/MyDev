using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyLevelFinalStep : PageBase
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
            ((Label)Master.FindControl("lblPageHeading")).Text = "Gloabal Setting";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Company/Employee/ViewEmployee.aspx?id=" + this.CompanyId;
            List<selectusersuploadfromexcelsheetResult> a = new UserInformationRepository().GetNewUserInformation(this.CompanyId, this.WorkGroupId);
            noofusers.Text = a.Count.ToString();
            lblworkgroup.Text = new LookupRepository().GetById(this.WorkGroupId).sLookupName;
        }
    }

    protected void lnkBtnSave_Click(object sender, EventArgs e)
    {
        Boolean IsError = false;

        try
        {
            selectmenuaccess menuaccess = (selectmenuaccess)Session["MenuAccess"];
            selectstoresetting storesetting = (selectstoresetting)Session["selectstoresetting"];

            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
            AnniversaryProgramRepository objAnniversaryProgramRepo = new AnniversaryProgramRepository();
            AnniversaryCreditProgram objProgram = new AnniversaryCreditProgram();

            this.CompanyStoreId = new CompanyStoreRepository().GetStoreIDByCompanyId(this.CompanyId);
            objProgram = objAnniversaryProgramRepo.GetProgByStoreIdWorkgroupId(this.WorkGroupId, this.CompanyStoreId);

            List<selectusersuploadfromexcelsheetResult> a = new UserInformationRepository().GetNewUserInformation(this.CompanyId, this.WorkGroupId);

            foreach (selectusersuploadfromexcelsheetResult objuser in a)
            {
                objCompanyEmployee = objEmpRepo.GetById(objuser.CompanyEmployeeID);

                #region MenuAccess
                //Delete from EmployeeMenuaccessfirst
                CompanyEmpMenuAccessRepository objCmpMenuAccesRepos = new CompanyEmpMenuAccessRepository();
                CompanyEmpMenuAccess objCmpMenuAccessfordel = new CompanyEmpMenuAccess();

                List<CompanyEmpMenuAccess> lst = objCmpMenuAccesRepos.getMenuByUserType(objuser.CompanyEmployeeID, "Company Admin,Company Employee");

                foreach (CompanyEmpMenuAccess l in lst)
                {
                    objCmpMenuAccesRepos.Delete(l);
                    objCmpMenuAccesRepos.SubmitChanges();
                }

                //Menu Access
                String[] mainmenu = !String.IsNullOrEmpty(menuaccess.mainmenusetting) ? menuaccess.mainmenusetting.Split(',') : new String[] {};

                foreach (String permenu in mainmenu)
                {
                    CompanyEmpMenuAccessRepository objCmpMenuAccesRep = new CompanyEmpMenuAccessRepository();
                    CompanyEmpMenuAccess objCmpMenuAccess = new CompanyEmpMenuAccess();
                    objCmpMenuAccess.MenuPrivilegeID = Convert.ToInt64(permenu);
                    objCmpMenuAccess.CompanyEmployeeID = objuser.CompanyEmployeeID;
                    objCmpMenuAccesRep.Insert(objCmpMenuAccess);
                    objCmpMenuAccesRep.SubmitChanges();
                }

                // Menu

                objCompanyEmployee.EmployeeUniformAccess = menuaccess.employeeuniform;
                objCompanyEmployee.AdditionInfoAccess = menuaccess.additonalinfo;
                objCompanyEmployee.CompanyStoreAccess = menuaccess.companystore;
                objCompanyEmployee.UniformPurchasingAccess = menuaccess.uniform;
                objCompanyEmployee.SuppliesAccess = menuaccess.supplies;

                #endregion

                #region StoreSetting

                objCompanyEmployee.Userstoreoption = storesetting.userstoreoption;
                objCompanyEmployee.Paymentoption = storesetting.paymentoption;
                objCompanyEmployee.Checkoutinformation = storesetting.checkoutinformation;

                #endregion

                #region AnniCredit

                //Add data foreach users..
                //Using Creaded SP Just For calculation Purpose.
                AnniversaryProgramRepository objUserData = new AnniversaryProgramRepository();
                SelectAnniversaryCreditProgramPerEmployeeResult objUserDataResult = objUserData.GetCompanyEmployeeAnniversaryCreditDetails(objuser.UserInfoID);

                if (String.IsNullOrEmpty(objCompanyEmployee.CreditAmtToApplied.ToString()))
                {
                    if (objUserDataResult.IssueCreditOn <= System.DateTime.Now.Date)
                    {
                        objCompanyEmployee.CreditAmtToApplied = objProgram.StandardCreditAmount;
                        objCompanyEmployee.NextCreditAppliedOn = Convert.ToDateTime(objUserDataResult.IssueCreditOn).AddYears(1).ToShortDateString();

                        if (((DateTime)objUserDataResult.CreditExpireAfter).Year.ToString() == "9999")
                        {
                            objCompanyEmployee.CreditExpireOn = "---";
                            objCompanyEmployee.CreditAmtToExpired = 0;
                        }
                        else
                        {
                            objCompanyEmployee.CreditExpireOn = Convert.ToDateTime(objUserDataResult.CreditExpireAfter).AddYears(1).ToShortDateString();
                            objCompanyEmployee.CreditAmtToExpired = objProgram.StandardCreditAmount;
                        }
                    }
                    else if (objUserDataResult.IssueCreditOn > System.DateTime.Now.Date)
                    {
                        objCompanyEmployee.CreditAmtToApplied = objProgram.StandardCreditAmount;
                        objCompanyEmployee.NextCreditAppliedOn = Convert.ToDateTime(objUserDataResult.IssueCreditOn).ToShortDateString();
                        if (((DateTime)objUserDataResult.CreditExpireAfter).Year.ToString() == "9999")
                        {
                            objCompanyEmployee.CreditExpireOn = "---";
                            objCompanyEmployee.CreditAmtToExpired = 0;
                        }
                        else
                        {
                            objCompanyEmployee.CreditExpireOn = objUserDataResult.CreditExpireAfter.ToString();
                            objCompanyEmployee.CreditAmtToExpired = objProgram.StandardCreditAmount;
                        }
                    }
                }

                #endregion

                #region UniformIssuance

                if (Convert.ToString(Session["uniformissuancepolicystatusid"]) != "")
                    objCompanyEmployee.EmpIssuancePolicyStatus = Convert.ToInt64(Session["uniformissuancepolicystatusid"]);
                else
                    objCompanyEmployee.EmpIssuancePolicyStatus = null;

                #endregion

                objCompanyEmployee.isfromexcelupload = false;
                objEmpRepo.SubmitChanges();

                //remove all the session here
                Session.Remove("uniformissuancepolicystatusid");
                Session.Remove("MenuAccess");
                Session.Remove("selectstoresetting");
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            IsError = true;
        }

        if (!IsError)
            Response.Redirect("~/admin/Company/ViewCompany.aspx?id=" + this.CompanyId);
    }

    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("CompanyLevelPrograms.aspx?id=" + this.CompanyId + "&workgroupid=" + this.WorkGroupId);
    }
}