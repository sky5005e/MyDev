using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyStore_CompanyPrograms_AnniversaryProgram : PageBase
{
    #region Data Members
    LookupRepository objLookRep = new LookupRepository();
    AnniversaryCreditProgram objProgram = new AnniversaryCreditProgram();
    AnniversaryProgramRepository objAnniversaryProgramRepo = new AnniversaryProgramRepository();
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
    Int64 AnniversaryCreditProgramId
    {
        get
        {
            if (ViewState["AnniversaryCreditProgramId"] == null)
            {
                ViewState["AnniversaryCreditProgramId"] = 0;
            }
            return Convert.ToInt64(ViewState["AnniversaryCreditProgramId"]);
        }
        set
        {
            ViewState["AnniversaryCreditProgramId"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                CheckLogin();
                BindDropDowns();
                //
                this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.AnniversaryCreditProgramId = Convert.ToInt64(Request.QueryString.Get("SubId"));

                //
                ((Label)Master.FindControl("lblPageHeading")).Text = "Anniversary Program";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/CompanyPrograms/ViewAnniversaryPrograms.aspx?id="+this.CompanyStoreId;
                menuControl.PopulateMenu(4, 0, this.CompanyStoreId, 0, true);
                bindvalues();
            }
            DisplayData(sender, e);
            
        }
    }
    protected void lnkBtnSaveInformation_Click(object sender, EventArgs e)
    {
        try
        {
            decimal? oldvalue = 0;
            objProgram.StoreId = this.CompanyStoreId;
            if (this.AnniversaryCreditProgramId != 0)
            {
                objProgram = objAnniversaryProgramRepo.GetById(this.AnniversaryCreditProgramId);
                oldvalue = objProgram.StandardCreditAmount;
            }

            objProgram.WorkgroupID = Convert.ToInt64(ddlWorkgroup.SelectedValue);
            objProgram.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
            objProgram.StandardCreditAmount = Convert.ToDecimal(txtCreditAmount.Text);
            objProgram.IssueCreditIn = Convert.ToInt32(ddlMonths.SelectedItem.Text.ToString());
            objProgram.CreditExpiresIn = Convert.ToInt32(ddlCreditExpiresAfter.SelectedItem.Value.ToString());
            if (ddlCreditStatus.SelectedItem.Value != "0")
                objProgram.CreditStatus = Convert.ToInt64(ddlCreditStatus.SelectedItem.Value);
            else
                objProgram.CreditStatus = null;
            if (ddlRuleStatus.SelectedItem.Value != "0")
                objProgram.RuleStatus = Convert.ToInt64(ddlRuleStatus.SelectedItem.Value);
            else
                objProgram.RuleStatus = null;
            if (ddlStatus.SelectedValue != null)
            {
                objProgram.EmployeeStatus = Convert.ToInt64(ddlStatus.SelectedValue);
            }
            else
            {
                objProgram.EmployeeStatus = null;
            }
            if (chkPreviousCreditExpire.Checked)
            {
                if (txtPreviousCreditExpire.Text != "")
                {
                    objProgram.ExpirePreviousCredit = Convert.ToDateTime(txtPreviousCreditExpire.Text.ToString());
                }
                else
                {
                    objProgram.ExpirePreviousCredit = null;
                }
            }
            else
            {
                objProgram.ExpirePreviousCredit = null;
            }
            if (this.AnniversaryCreditProgramId == 0)
            {
                objProgram.CreatedDate = System.DateTime.Now;
                objAnniversaryProgramRepo.Insert(objProgram);
            }
            else
            {
                objProgram.UpdatedDate = System.DateTime.Now;
            }
            objAnniversaryProgramRepo.SubmitChanges();

            CompanyStoreRepository objCompanyStore = new CompanyStoreRepository();
            CompanyStore objStore = objCompanyStore.GetById(this.CompanyStoreId);
            //Update CreditAmountToApplied from the company employee table
            UserInformationRepository objUserInfoRep = new UserInformationRepository();
            List<UserInformationRepository.SearchResults> objEmpl = new List<UserInformationRepository.SearchResults>();
            if (this.AnniversaryCreditProgramId == 0)
            {
                objEmpl = objUserInfoRep.getCompanyUsersForAnniversaryCredit("", "", "", "", "", objStore.CompanyID, objProgram.WorkgroupID, "AddAnniversary", Convert.ToDateTime(objProgram.CreatedDate));
            }
            else
            {
                objEmpl = objUserInfoRep.getCompanyUsersForAnniversaryCredit("", "", "", "", "", objStore.CompanyID, objProgram.WorkgroupID, "EditAnniversary", Convert.ToDateTime(objProgram.CreatedDate));
            }
            foreach (UserInformationRepository.SearchResults peremployee in objEmpl)
            {

                //Using Creaded SP Just For calculation Purpose.
                AnniversaryProgramRepository objUserData = new AnniversaryProgramRepository();
                SelectAnniversaryCreditProgramPerEmployeeResult objUserDataResult = objUserData.GetCompanyEmployeeAnniversaryCreditDetails(peremployee.UserInformationId);
                CompanyEmployeeRepository objCERep = new CompanyEmployeeRepository();
                CompanyEmployee objCE = objCERep.GetById(peremployee.CompanyEmployeeId);

                if (String.IsNullOrEmpty(objCE.CreditAmtToApplied.ToString()))
                {
                    if (objUserDataResult.IssueCreditOn <= System.DateTime.Now.Date)
                    {
                        objCE.CreditAmtToApplied = objProgram.StandardCreditAmount;
                        objCE.NextCreditAppliedOn = Convert.ToDateTime(objUserDataResult.IssueCreditOn).AddYears(1).ToShortDateString();
                        if (((DateTime)objUserDataResult.CreditExpireAfter).Year.ToString() == "9999")
                        {
                            objCE.CreditExpireOn = "---";
                            objCE.CreditAmtToExpired = 0;
                        }
                        else
                        {
                            objCE.CreditExpireOn = Convert.ToDateTime(objUserDataResult.CreditExpireAfter).AddYears(1).ToShortDateString();
                            objCE.CreditAmtToExpired = objProgram.StandardCreditAmount;
                        }
                        objCERep.SubmitChanges();
                    }
                    else if (objUserDataResult.IssueCreditOn > System.DateTime.Now.Date)
                    {
                        objCE.CreditAmtToApplied = objProgram.StandardCreditAmount;
                        objCE.NextCreditAppliedOn = Convert.ToDateTime(objUserDataResult.IssueCreditOn).ToShortDateString();
                        if (((DateTime)objUserDataResult.CreditExpireAfter).Year.ToString() == "9999")
                        {
                            objCE.CreditExpireOn = "---";
                            objCE.CreditAmtToExpired = 0;
                        }
                        else
                        {
                            objCE.CreditExpireOn = objUserDataResult.CreditExpireAfter.ToString();
                            objCE.CreditAmtToExpired = objProgram.StandardCreditAmount;
                        }
                        objCERep.SubmitChanges();
                    }
                    //Created on 14th dec 2011
                    #region EmployeeLedger

                    EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                    EmployeeLedger objEmplLedger = new EmployeeLedger();
                    EmployeeLedger transaction = new EmployeeLedger();

                    objEmplLedger.UserInfoId = peremployee.UserInformationId;
                    objEmplLedger.CompanyEmployeeId = peremployee.CompanyEmployeeId;
                    objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ANCR.ToString(); ;
                    objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.AnniversaryCredits.ToString();

                    objEmplLedger.TransactionAmount = Convert.ToDecimal(objCE.CreditAmtToApplied);
                    objEmplLedger.AmountCreditDebit = "Credit";

                    //When there will be records then get the last transaction
                    transaction = objLedger.GetLastTransactionByEmplID(peremployee.CompanyEmployeeId);
                    if (transaction != null)
                    {
                        objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                        objEmplLedger.CurrentBalance = transaction.CurrentBalance + objEmplLedger.TransactionAmount;
                    }
                    //When first time record is added for the CE
                    else
                    {
                        objEmplLedger.PreviousBalance = 0;
                        objEmplLedger.CurrentBalance = Convert.ToDecimal(objCE.CreditAmtToApplied);
                    }

                    objEmplLedger.TransactionDate = System.DateTime.Now;
                    objEmplLedger.Notes = "Issued Anniversary Credit";
                    objEmplLedger.UpdateById = IncentexGlobal.CurrentMember.UserInfoID;
                    objLedger.Insert(objEmplLedger);
                    objLedger.SubmitChanges();

                    //Starting Credits Add
                    #endregion
                    //End
                }
            }



            Response.Redirect("~/admin/CompanyStore/CompanyPrograms/ViewAnniversaryPrograms.aspx?id=" + this.CompanyStoreId);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblmsg.Text = "Unable to add anniversary program";
            ErrHandler.WriteError(ex);
        }

    }
    #endregion

    #region Methods
    public void BindDropDowns()
    {

        LookupDA sLookup = new LookupDA();
        LookupBE sLookupBE = new LookupBE();
        //For Status 
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "Status";
        ddlStatus.DataSource = sLookup.LookUp(sLookupBE);
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Department
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "Department";
        ddlDepartment.DataSource = sLookup.LookUp(sLookupBE);
        ddlDepartment.DataValueField = "iLookupID";
        ddlDepartment.DataTextField = "sLookupName";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-Select-", "0"));


        // For Workgroup
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "Workgroup";
        ddlWorkgroup.DataSource = sLookup.LookUp(sLookupBE);
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));
      

    }
    void DisplayData(object sender, EventArgs e)
    {

        if (this.AnniversaryCreditProgramId != 0)
        {
            objProgram = objAnniversaryProgramRepo.GetById(this.AnniversaryCreditProgramId);
            ddlWorkgroup.SelectedValue = objProgram.WorkgroupID.ToString();
            ddlDepartment.SelectedValue = objProgram.DepartmentID.ToString();
            txtCreditAmount.Text = objProgram.StandardCreditAmount.ToString();

            if (objProgram.IssueCreditIn.ToString() != "0")
                ddlMonths.Items.FindByText(objProgram.IssueCreditIn.ToString()).Selected = true;

            if (objProgram.CreditExpiresIn.ToString() != "0")
                ddlCreditExpiresAfter.Items.FindByText(objProgram.CreditExpiresIn.ToString()).Selected = true;

            if (objProgram.CreditStatus != null)
                ddlCreditStatus.Items.FindByValue(objProgram.CreditStatus.ToString()).Selected = true;

            if (objProgram.RuleStatus != null)
                ddlRuleStatus.Items.FindByValue(objProgram.RuleStatus.ToString()).Selected = true;

            if (objProgram.EmployeeStatus != null)
                ddlStatus.SelectedValue = objProgram.EmployeeStatus.ToString();

            if (objProgram.ExpirePreviousCredit != null)
                txtPreviousCreditExpire.Text = Convert.ToDateTime(objProgram.ExpirePreviousCredit.ToString()).ToShortDateString();
        }
    }
    public void bindvalues()
    {
        ddlRuleStatus.DataSource = objLookRep.GetByLookup("Rule Status");
        ddlRuleStatus.DataValueField = "iLookupID";
        ddlRuleStatus.DataTextField = "sLookupName";
        ddlRuleStatus.DataBind();
        ddlRuleStatus.Items.Insert(0, new ListItem("-select-", "0"));

        ddlCreditStatus.DataSource = objLookRep.GetByLookup("Credits Status");
        ddlCreditStatus.DataValueField = "iLookupID";
        ddlCreditStatus.DataTextField = "sLookupName";
        ddlCreditStatus.DataBind();
        ddlCreditStatus.Items.Insert(0, new ListItem("-select-", "0"));

        //bind months
        ddlMonths.Items.Insert(0, new ListItem("-select-", "0"));
        int j = 0;
        for (int i = 1; i <= 10; i++)
        {
            ddlMonths.Items.Insert(i, new ListItem((j+12).ToString(), i.ToString()));
            j = j + 6;
        }
        ddlMonths.Attributes.Add("onchange", "pageLoad(this,value);");

        ddlCreditExpiresAfter.Items.Insert(0, new ListItem("-select-", "0"));

        for (int i = 1; i <= 36; i++)
        {
            ddlCreditExpiresAfter.Items.Insert(i, new ListItem(i.ToString(), i.ToString()));
        }
        ddlCreditExpiresAfter.Attributes.Add("onchange", "pageLoad(this,value);");
    }
    #endregion
}
