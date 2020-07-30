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
using System.Collections.Generic;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;


public partial class admin_Company_Employee_SpecialProgram2 : PageBase
{
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

    CompanyEmployee objCompanyEmployee = new CompanyEmployee();
    CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                CheckLogin();

                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("SubId"));
                IncentexGlobal.ManageID = 7;
                //populatesubmenu();

                if (this.EmployeeId == 0)
                {
                    Response.Redirect("~/MyAccount/Employee/BasicInformation.aspx?Id=" + this.CompanyId);
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
                menucontrol.PopulateMenu(0, 1, this.CompanyId, this.EmployeeId, true);
                /* Company objCompany = new CompanyRepository().GetById(this.CompanyId);
                 if (objCompany == null)
                 {
                     Response.Redirect("~/admin/Company/Employee/ViewEmployee.aspx");
                 }*/
                ((Label)Master.FindControl("lblPageHeading")).Text = "Special Program";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/Employee/ViewEmployee.aspx?id=" + this.CompanyId;


                //set Search Page Return URL
                if (IncentexGlobal.SearchPageReturnURLFrontSide != null)
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.SearchPageReturnURLFrontSide;
                }
            }
            else
            {
                Response.Redirect("~/admin/Company/Employee/ViewEmployee.aspx");
            }
            bindEmployyeType();
            DisplayData(sender, e);
        }
    }

    public void bindEmployyeType()
    {
        //Dictionary<int, string> ht = Common.GetEnumForBind(typeof(DAEnums.CreditExpiryInMonths));

        //ddlCreditExpiresIn.DataSource = ht;
        //ddlCreditExpiresIn.DataTextField = "value";
        //ddlCreditExpiresIn.DataValueField = "key";
        //ddlCreditExpiresIn.DataBind();
        /*
        ddlCreditExpiresIn.Items.Insert(0, "Select");
        ddlCreditExpiresIn.Items.Insert(1, new ListItem("30"));
        ddlCreditExpiresIn.Items.Insert(2, new ListItem("60"));
        ddlCreditExpiresIn.Items.Insert(3, new ListItem("90"));
        ddlCreditExpiresIn.Items.Insert(4, new ListItem("120"));
        ddlCreditExpiresIn.Items.Insert(5, new ListItem("150"));*/

    }
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {

            objCompanyEmployee = objEmpRepo.GetById(EmployeeId);
            if (chkCreditProgram.Checked)
            {
                objCompanyEmployee.StratingCreditAmount = Convert.ToDecimal(txtCurrentCreditAmount.Text);
                objCompanyEmployee.CreditAmtToApplied = Convert.ToDecimal(objCompanyEmployee.CreditAmtToApplied) + Common.GetDecimal(txtCurrentCreditAmount.Text);
                objCompanyEmployee.CreditAmtToExpired = Convert.ToDecimal(objCompanyEmployee.CreditAmtToExpired) + Common.GetDecimal(txtCurrentCreditAmount.Text);
                objCompanyEmployee.StartingCreditUpdatedDate = System.DateTime.Now;


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
                objCompanyEmployee.StratingCreditAmount = null;
                objCompanyEmployee.StartingCreditUpdatedDate = System.DateTime.Now;
            }

            if (chkUniformIssuance.Checked)
            {


                if (hdnProductStatus.Value != null)
                {
                    objCompanyEmployee.EmpIssuancePolicyStatus = Convert.ToInt64(hdnProductStatus.Value);
                }
                else
                {
                    objCompanyEmployee.EmpIssuancePolicyStatus = null;
                }

                if (divRank.Visible == true)
                {
                    if (hdnRank.Value != null)
                    {
                        objCompanyEmployee.RankId = Convert.ToInt32(hdnRank.Value);
                    }
                    else
                    {
                        objCompanyEmployee.RankId = null;
                    }
                }
                else
                {
                    objCompanyEmployee.RankId = null;
                }

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
                    hdnRank.Value = objCompanyEmployee.RankId.ToString();
                }
                if (objCompanyEmployee.EmpIssuancePolicyStatus != null)
                {
                    chkUniformIssuance.Checked = true;
                    menuspan1.Attributes.Add("class", "custom-checkbox_checked");
                    hdnProductStatus.Value = objCompanyEmployee.EmpIssuancePolicyStatus.ToString();

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
                    hdnCreditAmtToBeApplied.Value = objCE.AnniversaryCreditBalance.ToString();
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
                    lblNextCreditAppliedOn.Text = Convert.ToDateTime(objCE.NextCreditAmountToApplyDate).ToShortDateString();
                    hdnNextCreditAppliedOn.Value = Convert.ToDateTime(objCE.NextCreditAmountToApplyDate).ToShortDateString();
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
    protected void lnkTransactionLog_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewLedgerDetails.aspx?Id=" + this.CompanyId + "&Subid=" + this.EmployeeId);
    }
}
