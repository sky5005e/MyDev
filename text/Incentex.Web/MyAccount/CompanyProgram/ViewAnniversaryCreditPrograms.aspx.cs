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
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;

public partial class MyAccount_CompanyProgram_ViewAnniversaryCreditPrograms : PageBase
{
    #region Local Property
    decimal TotalAmount = 0;
    PagedDataSource pds = new PagedDataSource();
    const int PageSize = 10;

    public int CurrentPage
    {
        get
        {
            if (this.ViewState["CurrentPage"] == null)
                return 0;
            else
                return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }
    public int PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt16(this.ViewState["PagerSize"].ToString());
        }
        set
        {
            this.ViewState["PagerSize"] = value;
        }
    }
    public int FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt16(this.ViewState["FrmPg"].ToString());
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }
    public int ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return PagerSize;
            else
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }
    List<selectcompanyanneversaryprogResult> objPrgList = new List<selectcompanyanneversaryprogResult>();
    AnniversaryProgramRepository ObjRep = new AnniversaryProgramRepository();
    AnniversaryProgramRepository.CompanyAnniversarySortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = AnniversaryProgramRepository.CompanyAnniversarySortExpType.HirerdDate;
            }
            return (AnniversaryProgramRepository.CompanyAnniversarySortExpType)ViewState["SortExp"];
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }

    Incentex.DAL.Common.DAEnums.SortOrderType SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
            }
            return (Incentex.DAL.Common.DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            if (IncentexGlobal.IsIEFromStore)
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/CompanyProgram/SearchAnniversaryProgram.aspx";
            ((Label)Master.FindControl("lblPageHeading")).Text = "Company Anniversary Program";

            bindgrid();
        }
    }
    public void bindgrid()
    {
        CompanyEmployee objCmpnyInfo = new CompanyEmployee();
        CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
        objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

        //if (employeeId == "")
        //{
        //    objPrgList = ObjRep.GetCompanyEmployeeAnniversaryCredit((long)IncentexGlobal.CurrentMember.CompanyId, (long)objCmpnyInfo.ManagementControlForWorkgroup, this.SortExp, this.SortOrder, null);
        //}
        //else
        //{
        Int64? stCode;
        Int64? empStatus;
        string employeeid;
        if (Request.QueryString["employeeid"] != string.Empty)
        {
            employeeid = Request.QueryString["employeeid"].ToString();
        }
        else
        {
            employeeid = null;
        }
        if (Request.QueryString["stCode"].ToString() != "0")
        {
            stCode = Convert.ToInt64(Request.QueryString["stCode"].ToString());
        }
        else
        {
            stCode = null;
        }
        if (Request.QueryString["empstatus"].ToString() != "0")
        {
            empStatus = Convert.ToInt64(Request.QueryString["empstatus"].ToString());
        }
        else
        {
            empStatus = null;
        }

        objPrgList = ObjRep.GetCompanyEmployeeAnniversaryCredit((long)IncentexGlobal.CurrentMember.CompanyId, (long)objCmpnyInfo.ManagementControlForWorkgroup, this.SortExp, this.SortOrder, employeeid, Request.QueryString["FName"] != null ? Request.QueryString["FName"].ToString() : null, Request.QueryString["Lname"] != null ? Request.QueryString["Lname"].ToString() : null, stCode, empStatus);
        pds.DataSource = objPrgList;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        //}
        if (objPrgList.Count > 0)
        {

            divStats.Visible = true;
            lblRecordCount.Text = objPrgList.Count.ToString();
            lblWorkgroup.Text = objPrgList[0].sLookupName;
            gvAnniversaryPro.DataSource = pds;
            gvAnniversaryPro.DataBind();
            if (Request.QueryString["crType"].ToString() == "0")
            {
                gvAnniversaryPro.Columns[4].Visible = false;
                gvAnniversaryPro.Columns[5].Visible = false;
                gvAnniversaryPro.Columns[6].Visible = true;
            }
            else if (Request.QueryString["crType"].ToString() == "1")
            {

                gvAnniversaryPro.Columns[4].Visible = true;
                gvAnniversaryPro.Columns[5].Visible = false;
                gvAnniversaryPro.Columns[6].Visible = false;
            }
            else
            {
                gvAnniversaryPro.Columns[4].Visible = false;
                gvAnniversaryPro.Columns[5].Visible = true;
                gvAnniversaryPro.Columns[6].Visible = false;
            }
        }
        else
        {
            divStats.Visible = false;
            gvAnniversaryPro.DataSource = pds;
            gvAnniversaryPro.DataBind();
            lblMsg.Text = "No users with selected criteria!!!";
        }
        doPaging();
    }
    private void doPaging()
    {
        try
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            int CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + PagerSize;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - PagerSize;

            }

            if (pds.PageCount < ToPg)
                ToPg = pds.PageCount;

            for (int i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            lstPaging.DataSource = dt;
            lstPaging.DataBind();

        }
        catch (Exception)
        { }
    }
    protected void gvAnniversaryPro_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void gvAnniversaryPro_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {

            case "Sort":

                if (this.SortExp.ToString() == e.CommandArgument.ToString())
                {
                    if (this.SortOrder == Incentex.DAL.Common.DAEnums.SortOrderType.Asc)
                        this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Desc;
                    else
                        this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                }
                else
                {
                    this.SortOrder = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
                    this.SortExp = (AnniversaryProgramRepository.CompanyAnniversarySortExpType)Enum.Parse(typeof(AnniversaryProgramRepository.CompanyAnniversarySortExpType), e.CommandArgument.ToString());
                }
                bindgrid();
                break;

            case "ViewProfile":
                Session["backlink"] = Request.Url.ToString();
                Response.Redirect("EmployeeProfile.aspx?id=" + e.CommandArgument.ToString());
                break;

            case "ChangeStatus":
                HiddenField hdnStatusID;
                Label lblCompanyEmployeeID;

                GridViewRow row;
                row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;

                lblCompanyEmployeeID = (Label)(gvAnniversaryPro.Rows[row.RowIndex].FindControl("lblCompanyEmployeeIDView"));

                hdnStatusID = (HiddenField)(gvAnniversaryPro.Rows[row.RowIndex].FindControl("hdnStatusID"));
                if (lblCompanyEmployeeID.Text != "")
                {
                    CompanyEmployeeRepository objCompanyEmpRepos = new CompanyEmployeeRepository();
                    CompanyEmployee objEmpCop = new CompanyEmployee();
                    objEmpCop = objCompanyEmpRepos.GetById(Convert.ToInt64(lblCompanyEmployeeID.Text));
                    if (objEmpCop.UserInfoID != null)
                    {
                        UserInformationRepository objUserInfoRepos = new UserInformationRepository();
                        objUserInfoRepos.UpdateStatus(Convert.ToInt32(objEmpCop.UserInfoID), Convert.ToInt32(hdnStatusID.Value));
                        objUserInfoRepos.SubmitChanges();
                        bindgrid();
                    }
                }

                break;

            case "saveAnniversary":
                {
                    if (((TextBox)((LinkButton)e.CommandSource).NamingContainer.FindControl("lblAnniversaryCreditAmount")).Text != null)
                    {
                        CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
                        CompanyEmployee objEmployee = objCompanyEmployeeRepository.GetById(Convert.ToInt64(e.CommandArgument.ToString()));
                        objEmployee.CreditAmtToApplied = Common.GetDecimal(((TextBox)((LinkButton)e.CommandSource).NamingContainer.FindControl("lblAnniversaryCreditAmount")).Text);
                        objEmployee.CreditAmtToExpired = Common.GetDecimal(((TextBox)((LinkButton)e.CommandSource).NamingContainer.FindControl("lblAnniversaryCreditAmount")).Text);
                        objCompanyEmployeeRepository.SubmitChanges();


                        #region EmployeeLedger
                        HiddenField hdnAnniversary = ((HiddenField)((LinkButton)e.CommandSource).NamingContainer.FindControl("hdnAnniversary"));
                        TextBox txtAnniversaryCreditAmount = ((TextBox)((LinkButton)e.CommandSource).NamingContainer.FindControl("lblAnniversaryCreditAmount"));
                        Label lblCompanyEmployeID = ((Label)((LinkButton)e.CommandSource).NamingContainer.FindControl("lblCompanyEmployeeID"));
                        if (txtAnniversaryCreditAmount.Text != "0.00")
                        {
                            //If value is changed then add record only.
                            if (hdnAnniversary.Value != txtAnniversaryCreditAmount.Text)
                            {
                                EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                                EmployeeLedger objEmplLedger = new EmployeeLedger();
                                EmployeeLedger transaction = new EmployeeLedger();

                                objEmplLedger.UserInfoId = objEmployee.UserInfoID;
                                objEmplLedger.CompanyEmployeeId = Convert.ToInt64(lblCompanyEmployeID.Text);
                                objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ANCR.ToString(); ;
                                objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.AnniversaryCredits.ToString();
                                if (hdnAnniversary.Value == "")// first time
                                {
                                    objEmplLedger.TransactionAmount = Convert.ToDecimal(txtAnniversaryCreditAmount.Text);
                                    objEmplLedger.AmountCreditDebit = "Credit";
                                }
                                else if (Convert.ToDecimal(txtAnniversaryCreditAmount.Text) > Convert.ToDecimal(hdnAnniversary.Value))
                                {
                                    //Increasing starting amount e.g 100 >> 130
                                    objEmplLedger.TransactionAmount = Convert.ToDecimal(txtAnniversaryCreditAmount.Text) - Convert.ToDecimal(hdnAnniversary.Value);
                                    objEmplLedger.AmountCreditDebit = "Credit";
                                }
                                else if (Convert.ToDecimal(txtAnniversaryCreditAmount.Text) < Convert.ToDecimal(hdnAnniversary.Value))
                                {
                                    //Decreasing amount e.g. >> 120 >> 100
                                    objEmplLedger.TransactionAmount = Convert.ToDecimal(txtAnniversaryCreditAmount.Text) - Convert.ToDecimal(hdnAnniversary.Value);
                                    objEmplLedger.AmountCreditDebit = "Debit";
                                }
                                else
                                {
                                    objEmplLedger.TransactionAmount = Convert.ToDecimal(txtAnniversaryCreditAmount.Text);
                                    objEmplLedger.AmountCreditDebit = "Credit";
                                }


                                //When there will be records then get the last transaction
                                transaction = objLedger.GetLastTransactionByEmplID(objEmployee.CompanyEmployeeID);
                                if (transaction != null)
                                {
                                    objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                                    objEmplLedger.CurrentBalance = transaction.CurrentBalance + objEmplLedger.TransactionAmount;
                                }
                                //When first time record is added for the CE
                                else
                                {
                                    objEmplLedger.PreviousBalance = 0;
                                    objEmplLedger.CurrentBalance = Convert.ToDecimal(txtAnniversaryCreditAmount.Text);
                                }

                                objEmplLedger.TransactionDate = System.DateTime.Now;
                                objEmplLedger.UpdateById = IncentexGlobal.CurrentMember.UserInfoID;
                                objLedger.Insert(objEmplLedger);
                                objLedger.SubmitChanges();
                            }
                            //Starting Credits Add

                        }
                        #endregion
                    }
                }
                break;
        }
    }
    protected void gvAnniversaryPro_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal total = 0;
            //Get Company Employee Current Credit Balance
            AnniversaryProgramRepository objCEProgram = new AnniversaryProgramRepository();
            SelectAnniversaryCreditProgramPerEmployeeResult objResult = objCEProgram.GetCompanyEmployeeAnniversaryCreditDetails(Convert.ToInt64(((Label)e.Row.FindControl("lblCompanyEmployeeUserInfoID")).Text));
            ((Label)e.Row.FindControl("lblCreditBalance")).Text = objResult.CurrentBalance.ToString();
            ((TextBox)e.Row.FindControl("lblStaringCredit")).Text = objResult.StratingCreditAmount.ToString();
            ((HiddenField)e.Row.FindControl("hdnStartingCredit")).Value = objResult.StratingCreditAmount.ToString();
            ((TextBox)e.Row.FindControl("lblAnniversaryCreditAmount")).Text = objResult.AnniversaryCreditBalance.ToString();
            ((HiddenField)e.Row.FindControl("hdnAnniversary")).Value = objResult.AnniversaryCreditBalance.ToString();
            
            if (((Incentex.DAL.selectcompanyanneversaryprogResult)(e.Row.DataItem)).Status == "Active")
            {
                if (Request.QueryString["crType"].ToString() == "0")
                {
                    total = Convert.ToDecimal(((Label)e.Row.FindControl("lblCreditBalance")).Text);
                }
                else if (Request.QueryString["crType"].ToString() == "1")
                {
                    if (objResult.StratingCreditAmount == null)
                    {
                        total = 0;
                    }
                    else
                    {
                        total = Convert.ToDecimal(((TextBox)e.Row.FindControl("lblStaringCredit")).Text);
                    }
                }
                else
                {
                    if (objResult.AnniversaryCreditBalance == null)
                    {
                        total = 0;
                    }
                    else
                    {
                        total = Convert.ToDecimal(((TextBox)e.Row.FindControl("lblAnniversaryCreditAmount")).Text);
                    }
                }


            }
            TotalAmount = TotalAmount + total;
            hdnTotalValue.Value = TotalAmount.ToString();
            //Set Hire Date
            if (((Label)e.Row.FindControl("lblHiredDate")).Text == "")
            {
                ((Label)e.Row.FindControl("lblHiredDate")).Text = "---";
            }
            //Set Image PAth
            string path = "~/admin/Incentex_Used_Icons/" + ((HiddenField)e.Row.FindControl("lblIconPath")).Value;
            ((HtmlImage)e.Row.FindControl("imgLookupIcon")).Src = path;


            if (new OrderConfirmationRepository().GetUserIdsWithInCompletedOrders(Convert.ToInt64(((Label)e.Row.FindControl("lblCompanyEmployeeUserInfoID")).Text)))
            {
                //Orderid is there in the incomplete order so unlock here
                ((TextBox)e.Row.FindControl("lblAnniversaryCreditAmount")).Enabled = true;
                ((LinkButton)e.Row.FindControl("lnkSaveAnnive")).Visible = true;

            }
            else
            {
                //Keep locked textbox here
                ((TextBox)e.Row.FindControl("lblAnniversaryCreditAmount")).Enabled = false;
                ((LinkButton)e.Row.FindControl("lnkSaveAnnive")).Visible = false;
            }
        }

        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[3].Text = "Totals:";
            e.Row.Cells[3].Style.Add("style", "text-align:right");
            if (Request.QueryString["crType"].ToString() == "0")
            {
                e.Row.Cells[6].Text = hdnTotalValue.Value;
            }
            else if (Request.QueryString["crType"].ToString() == "1")
            {
                e.Row.Cells[4].Text = hdnTotalValue.Value;
            }
            else
            {
                e.Row.Cells[5].Text = hdnTotalValue.Value;
            }

            e.Row.Font.Bold = true;

        }
    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        bindgrid();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindgrid();
    }
    protected void lstPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindgrid();
        }
    }
    protected void lstPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {


        CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
        try
        {
            foreach (GridViewRow gr in gvAnniversaryPro.Rows)
            {


                TextBox txtStratingCreditAmount = gr.FindControl("lblStaringCredit") as TextBox;
                Label lblCompanyEmployeeID = gr.FindControl("lblCompanyEmployeeIDView") as Label;
                HiddenField hdnStartingCredit = gr.FindControl("hdnStartingCredit") as HiddenField;
                if (string.IsNullOrEmpty(txtStratingCreditAmount.Text))
                {
                    txtStratingCreditAmount.Text = "0";
                }
                CompanyEmployee objEmployee = objCompanyEmployeeRepository.GetById(Convert.ToInt64(lblCompanyEmployeeID.Text));
                objEmployee.StratingCreditAmount = Common.GetDecimal(txtStratingCreditAmount.Text);
                objEmployee.CreditAmtToApplied = objEmployee.CreditAmtToApplied + Common.GetDecimal(txtStratingCreditAmount.Text);
                objEmployee.CreditAmtToExpired = objEmployee.CreditAmtToExpired + Common.GetDecimal(txtStratingCreditAmount.Text);
                objEmployee.StartingCreditUpdatedDate = DateTime.Now;

                #region EmployeeLedger
                if (txtStratingCreditAmount.Text != "0.00")
                {
                    //If value is changed then add record only.
                    if (hdnStartingCredit.Value != txtStratingCreditAmount.Text)
                    {
                        EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                        EmployeeLedger objEmplLedger = new EmployeeLedger();
                        EmployeeLedger transaction = new EmployeeLedger();

                        objEmplLedger.UserInfoId = objEmployee.UserInfoID;
                        objEmplLedger.CompanyEmployeeId = Convert.ToInt64(lblCompanyEmployeeID.Text);
                        objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.STCR.ToString(); ;
                        objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.StartingCredits.ToString();
                        if (hdnStartingCredit.Value == "")// first time
                        {
                            objEmplLedger.TransactionAmount = Convert.ToDecimal(txtStratingCreditAmount.Text);
                            objEmplLedger.AmountCreditDebit = "Credit";
                        }
                        else if (Convert.ToDecimal(txtStratingCreditAmount.Text) > Convert.ToDecimal(hdnStartingCredit.Value))
                        {
                            //Increasing starting amount e.g 100 >> 130
                            objEmplLedger.TransactionAmount = Convert.ToDecimal(txtStratingCreditAmount.Text) - Convert.ToDecimal(hdnStartingCredit.Value);
                            objEmplLedger.AmountCreditDebit = "Credit";
                        }
                        else if (Convert.ToDecimal(txtStratingCreditAmount.Text) < Convert.ToDecimal(hdnStartingCredit.Value))
                        {
                            //Decreasing amount e.g. >> 120 >> 100
                            objEmplLedger.TransactionAmount = Convert.ToDecimal(txtStratingCreditAmount.Text) - Convert.ToDecimal(hdnStartingCredit.Value);
                            objEmplLedger.AmountCreditDebit = "Debit";
                        }
                        else
                        {
                            objEmplLedger.TransactionAmount = Convert.ToDecimal(txtStratingCreditAmount.Text);
                            objEmplLedger.AmountCreditDebit = "Credit";
                        }


                        //When there will be records then get the last transaction
                        transaction = objLedger.GetLastTransactionByEmplID(objEmployee.CompanyEmployeeID);
                        if (transaction != null)
                        {
                            objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                            objEmplLedger.CurrentBalance = transaction.CurrentBalance + objEmplLedger.TransactionAmount;
                        }
                        //When first time record is added for the CE
                        else
                        {
                            objEmplLedger.PreviousBalance = 0;
                            objEmplLedger.CurrentBalance = Convert.ToDecimal(txtStratingCreditAmount.Text);
                        }

                        objEmplLedger.TransactionDate = System.DateTime.Now;
                        objEmplLedger.UpdateById = IncentexGlobal.CurrentMember.UserInfoID;
                        objLedger.Insert(objEmplLedger);
                        objLedger.SubmitChanges();
                    }
                    //Starting Credits Add

                }
                #endregion
            }
            lblMsg.Text = "Employee Credits are updated";
            objCompanyEmployeeRepository.SubmitChanges();
            bindgrid();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in saving record";
            ErrHandler.WriteError(ex);
        }
    }
}
