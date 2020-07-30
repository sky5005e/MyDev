using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_Company_Employee_SpecialProgram2 : PageBase
{
    #region Data Members
    LookupRepository objLookupRepos = new LookupRepository();
    CompanyEmployee objCompanyEmployee = new CompanyEmployee();
    CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
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
    Int64 EmployeeId
    {
        get
        {
            if (ViewState["EmployeeId"] == null)
            {
                ViewState["EmployeeId"] = 0;
            }
            return Convert.ToInt64(ViewState["EmployeeId"]);
        }
        set
        {
            ViewState["EmployeeId"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Employees";
            base.ParentMenuID = 11;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.QueryString.Count > 0)
            {
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("SubId"));
                //populatesubmenu();

                if (this.EmployeeId == 0)
                {
                    Response.Redirect("~/admin/Company/Employee/BasicInformation.aspx?Id=" + this.CompanyId);
                }
                //Nagmani Start
                //Get the Employee Workgroup Id
                // if Work group is PilotGroup then show the Rank Status
                //Dropdownlist
                if (this.EmployeeId != 0)
                {
                    objCompanyEmployee = objEmpRepo.GetById(EmployeeId);
                    if (objCompanyEmployee.WorkgroupID == Convert.ToInt64(Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.Workgroup)))
                    {
                        divRank.Visible = true;
                    }
                    else
                    {
                        divRank.Visible = false;
                    }
                }
                //Nagmani End
                menucontrol.PopulateMenu(2, 1, this.CompanyId, this.EmployeeId, true);
                ((Label)Master.FindControl("lblPageHeading")).Text = "Special Program";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Company/Employee/ViewEmployee.aspx?id=" + this.CompanyId;

                //set Search Page Return URL
                if (IncentexGlobal.SearchPageReturnURL != null)
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.SearchPageReturnURL;
                }
            }
            else
            {
                Response.Redirect("~/admin/Company/Employee/ViewEmployee.aspx");
            }
            FillRank();
            FillEmployeePolicyStatus();
            DisplayData(sender, e);
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            objCompanyEmployee = objEmpRepo.GetById(EmployeeId);
            if (chkCreditProgram.Checked)
            {
                objCompanyEmployee.StratingCreditAmount = Convert.ToDecimal(txtCurrentCreditAmount.Text);                
                objCompanyEmployee.CreditAmtToApplied = Convert.ToDecimal(objCompanyEmployee.CreditAmtToApplied) + Common.GetDecimal(txtCurrentCreditAmount.Text);
                objCompanyEmployee.CreditAmtToExpired = Convert.ToDecimal(objCompanyEmployee.CreditAmtToExpired) + Common.GetDecimal(txtCurrentCreditAmount.Text);
                objCompanyEmployee.StartingCreditUpdatedDate = System.DateTime.Now;
                //Start Add Nagmani 16-Jan-2012
                objCompanyEmployee.ProgramActive = "Active";
                //End Add Nagmani 16-Jan-2012
                #region EmployeeLedger
                EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                EmployeeLedger objEmplLedger = new EmployeeLedger();
                EmployeeLedger transaction = new EmployeeLedger();

                objEmplLedger.UserInfoId = objCompanyEmployee.UserInfoID;
                objEmplLedger.CompanyEmployeeId = objCompanyEmployee.CompanyEmployeeID;
                objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.STCR.ToString(); ;
                objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.StartingCredits.ToString();

                objEmplLedger.TransactionAmount = Convert.ToDecimal(txtCurrentCreditAmount.Text);
                objEmplLedger.AmountCreditDebit = "Credit";

                //When there will be records then get the last transaction
                transaction = objLedger.GetLastTransactionByEmplID(EmployeeId);
                if (transaction != null)
                {
                    objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                    objEmplLedger.CurrentBalance = transaction.CurrentBalance + objEmplLedger.TransactionAmount;
                }
                //When first time record is added for the CE
                else
                {
                    objEmplLedger.PreviousBalance = 0;
                    objEmplLedger.CurrentBalance = Convert.ToDecimal(txtCurrentCreditAmount.Text);
                }

                objEmplLedger.TransactionDate = System.DateTime.Now;
                objEmplLedger.UpdateById = IncentexGlobal.CurrentMember.UserInfoID;
                objLedger.Insert(objEmplLedger);
                objLedger.SubmitChanges();

                ////Starting Credits Add
                #endregion

            }
            else
            {
                //Start Update Nagmani 16-Jan-2012
                objCompanyEmployee.ProgramActive = "InActive";
                objCompanyEmployee.StartingCreditUpdatedDate = System.DateTime.Now;
            }

            if (chkUniformIssuance.Checked)
            {
                if (ddlEmployeePolicyStatus.SelectedValue != "0")
                    objCompanyEmployee.EmpIssuancePolicyStatus = Convert.ToInt64(ddlEmployeePolicyStatus.SelectedValue);
                else
                    objCompanyEmployee.EmpIssuancePolicyStatus = null;

                if (divRank.Visible == true)
                {
                    if (ddlRank.SelectedValue != "0")
                        objCompanyEmployee.RankId = Convert.ToInt32(ddlRank.SelectedValue);
                    else
                        objCompanyEmployee.RankId = null;
                }
                else
                    objCompanyEmployee.RankId = null;
            }
            else
            {
                objCompanyEmployee.RankId = null;
                objCompanyEmployee.EmpIssuancePolicyStatus = null;
            }

            //End Nagmani 

            objEmpRepo.SubmitChanges();

            Response.Redirect("MenuAccess.aspx?Id=" + this.CompanyId + "&SubId=" + this.EmployeeId);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex);
        }

    }

    protected void lnkTransactionLog_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewLedgerDetails.aspx?Id=" + this.CompanyId + "&Subid=" + this.EmployeeId);
    }
    #endregion

    #region Methods
    private void FillRank()
    {
        ddlRank.DataSource = objLookupRepos.GetByLookup("Rank");
        ddlRank.DataValueField = "iLookupID";
        ddlRank.DataTextField = "sLookupName";
        ddlRank.DataBind();
        ddlRank.Items.Insert(0, new ListItem("-Select Rank-", "0"));
    }
    private void FillEmployeePolicyStatus()
    {
        ddlEmployeePolicyStatus.DataSource = objLookupRepos.GetByLookup("Status");
        ddlEmployeePolicyStatus.DataValueField = "iLookupID";
        ddlEmployeePolicyStatus.DataTextField = "sLookupName";
        ddlEmployeePolicyStatus.DataBind();
        ddlEmployeePolicyStatus.Items.Insert(0, new ListItem("-Select Employee Issuance Policy Status-", "0"));
    }
    void DisplayData(object sender, EventArgs e)
    {
        try
        {
            //Add nagmani take here storeid using employeeid from table Companystore
            CompanyStoreRepository objComstoreRepos = new CompanyStoreRepository();
            UserInformationRepository objUserRepos = new UserInformationRepository();
            long intStoreId;
            long userinfoid;
            intStoreId = objComstoreRepos.GetStoreIDByCompanyId(CompanyId);

            //End Nagmani
            if (this.EmployeeId != 0)
            {
                CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetById(this.EmployeeId);

                if (objCompanyEmployee == null)
                {
                    return;
                }

                hdnStartingCredit.Value = Convert.ToString(objCompanyEmployee.StratingCreditAmount);

                if (objCompanyEmployee.RankId != null)
                {
                    ddlRank.SelectedValue = objCompanyEmployee.RankId.ToString();
                }
                if (objCompanyEmployee.EmpIssuancePolicyStatus != null)
                {
                    chkUniformIssuance.Checked = true;
                    menuspan1.Attributes.Add("class", "custom-checkbox_checked");
                    ddlEmployeePolicyStatus.SelectedValue = objCompanyEmployee.EmpIssuancePolicyStatus.ToString();

                }
                else
                {
                    menuspan1.Attributes.Add("class", "custom-checkbox alignleft");
                    chkUniformIssuance.Checked = false;

                }

                UserInformation objUserInfo = new UserInformationRepository().GetById(objCompanyEmployee.UserInfoID);
                lblUserFullName.Text = objUserInfo.FirstName + " " + objUserInfo.LastName;
                userinfoid = (objUserInfo.UserInfoID);
                List<SelectAnniversaryCreditProgramResult> objAnniversary;
                objAnniversary = objEmpRepo.GetAnniversaryProgram(Convert.ToInt32(userinfoid), Convert.ToInt32(intStoreId));
                //0
                txtCurrentCreditAmount.Text = objCompanyEmployee.StratingCreditAmount.ToString();


                //Added on 8th April

                //Add Nagmani Kumar 16-Jan-2012
                if (objCompanyEmployee.ProgramActive != null)
                {
                    hdnProgramActive.Value = objCompanyEmployee.ProgramActive.ToString();
                }

                if (objCompanyEmployee.ProgramActive == "Active")
                {
                    if (txtCurrentCreditAmount.Text != "")
                    {
                        chkCreditProgram.Checked = true;
                        menuspan.Attributes.Add("class", "custom-checkbox_checked");
                    }
                    else
                    {
                        menuspan.Attributes.Add("class", "custom-checkbox alignleft");
                        chkCreditProgram.Checked = false;
                    }
                }
                else
                //Start Nagmani 16-Jan-2012
                {
                    menuspan.Attributes.Add("class", "custom-checkbox alignleft");
                    chkCreditProgram.Checked = false;
                }
                //End Nagmani

                //End
                lblCreditAmtExpiresOn.Text = objCompanyEmployee.StratingCreditAmount.ToString();
                if (objAnniversary.Count != 0)
                {
                    //Create By Ankit on 10 Feb 11
                    AnniversaryProgramRepository objCEProgram = new AnniversaryProgramRepository();
                    SelectAnniversaryCreditProgramPerEmployeeResult objCE = objCEProgram.GetCompanyEmployeeAnniversaryCreditDetails(objCompanyEmployee.UserInfoID);

                    //1
                    //5
                    if (objCE.StartingCreditExpireDate != null)
                    {
                        if (((DateTime)objCE.StartingCreditExpireDate).Year.ToString() != "0001")
                        {
                            lblCreditExpiresOn.Text = Convert.ToDateTime(objCE.StartingCreditExpireDate).ToShortDateString();
                        }
                        else
                        {
                            lblCreditExpiresOn.Text = "---";
                        }
                    }
                    else
                    {
                        lblCreditExpiresOn.Text = "---";
                    }


                    //2

                    lblCreditAmountAppliedTo.Text = objCE.AmountFromProgram.ToString();
                    if (objCE.AnniversaryCreditBalance != null)
                    {
                        hdnCreditAmtToBeApplied.Value = objCE.AnniversaryCreditBalance.ToString();
                    }

                    //3
                    //commented by mayur on 23-March-2012
                    //if (txtCurrentCreditAmount.Text != null)
                    //{

                    //    lblCreditAmtExpiresOn.Text = (Convert.ToDecimal(txtCurrentCreditAmount.Text) + Convert.ToDecimal(hdnCreditAmtToBeApplied.Value)).ToString();
                    //}
                    //else
                    //{
                    lblCreditAmtExpiresOn.Text = (Convert.ToDecimal(hdnCreditAmtToBeApplied.Value)).ToString();
                    //}

                    //4
                    if (objCE.NextCreditAmountToApplyDate != null)
                    {
                        lblNextCreditAppliedOn.Text = Convert.ToDateTime(objCE.NextCreditAmountToApplyDate).ToShortDateString();
                    }
                    if (objCE.NextCreditAmountToApplyDate != null)
                    {
                        hdnNextCreditAppliedOn.Value = Convert.ToDateTime(objCE.NextCreditAmountToApplyDate).ToShortDateString();
                    }

                    //End

                }


            }
            else
            {

            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion
}
