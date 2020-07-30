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
using Incentex.DAL;
using System.Collections.Generic;
using Incentex.DAL.Common;

public partial class MyAccount_CompanyProgram_ViewLedgerDetails : PageBase
{
    PagedDataSource pds = new PagedDataSource();
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
    EmployeeLedgerRepository.LedgerSortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = EmployeeLedgerRepository.LedgerSortExpType.LedgerId;
            }
            return (EmployeeLedgerRepository.LedgerSortExpType)ViewState["SortExp"];
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
                ViewState["SortOrder"] = DAEnums.SortOrderType.Asc;
            }
            return (DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                CheckLogin();
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("SubId"));
                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/Company/Employee/SpecialProgram.aspx?id=" + this.CompanyId + "&SubId=" + this.EmployeeId;
                ((Label)Master.FindControl("lblPageHeading")).Text = "View Transaction Log";
                BindTransactionDetails();
            }
        }

    }
    public void BindTransactionDetails()
    {
        List<EmployeeLedgerResult> objEmplHis = new List<EmployeeLedgerResult>();
        EmployeeLedgerRepository objLedgerHistory = new EmployeeLedgerRepository();
        objEmplHis = objLedgerHistory.GetEmployeeLedgerByUserInfoId(new CompanyEmployeeRepository().GetById(this.EmployeeId).UserInfoID, this.SortExp, this.SortOrder);
        if (objEmplHis.Count == 0)
        {
            lblMsg.Text = "No Transactions found";
            dvPaging.Visible = false;
            return;
        }
        else
        {
            dvPaging.Visible = true;
            //if (objEmplHis.Where(e => e.AmountCreditDebit.ToLower() == "credit").Count() > 0)
            //{
            //    BindCreditType(false);
            //}
            //else
            //{
            //    BindCreditType(true);
            //}

        }

        pds.DataSource = objEmplHis;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gvEmployeeTransactionHistory.DataSource = pds;
        gvEmployeeTransactionHistory.DataBind();

        doPaging();
    }
    protected void BindCreditType(bool IsStartingCreditApplicable)
    {
        ddlCreditType.ClearSelection();
        ddlCreditType.Items.Clear();
        ListItem objItem = new ListItem();
        objItem.Text = "--Select--";
        objItem.Value = "0";
        ddlCreditType.Items.Add(objItem);
        if (IsStartingCreditApplicable)
        {
            objItem = new ListItem();
            objItem.Text = "Starting";
            objItem.Value = "1";
            ddlCreditType.Items.Add(objItem);
        }
        objItem = new ListItem();
        objItem.Text = "Anniversary";
        objItem.Value = "2";
        ddlCreditType.Items.Add(objItem);
    }

    protected void gvEmployeeTransactionHistory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {

            case "Sorting":

                if (this.SortExp.ToString() == e.CommandArgument.ToString())
                {
                    if (this.SortOrder == DAEnums.SortOrderType.Asc)
                        this.SortOrder = DAEnums.SortOrderType.Desc;
                    else
                        this.SortOrder = DAEnums.SortOrderType.Asc;
                }
                else
                {
                    this.SortOrder = DAEnums.SortOrderType.Asc;
                    this.SortExp = (EmployeeLedgerRepository.LedgerSortExpType)Enum.Parse(typeof(EmployeeLedgerRepository.LedgerSortExpType), e.CommandArgument.ToString());
                }
                BindTransactionDetails();
                break;

            case "View":
                if (e.CommandArgument != "")
                {
                    Response.Redirect("~/OrderManagement/OrderDetail.aspx?id=" + e.CommandArgument.ToString() + "&from=lgr");
                }
                break;
        }
    }
    protected void gvEmployeeTransactionHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((LinkButton)e.Row.FindControl("lblOrderNumber")).Text != "---")
            {
                Order objOrder = new OrderConfirmationRepository().GetByOrderID(Convert.ToInt64(((Label)e.Row.FindControl("lblORderId")).Text));
                if (objOrder != null)
                {
                    if (objOrder.OrderNumber == ((LinkButton)e.Row.FindControl("lblOrderNumber")).Text)
                    {
                        if (new OrderConfirmationRepository().GetByOrderNo(((LinkButton)e.Row.FindControl("lblOrderNumber")).Text).CreditUsed == "Anniversary")
                        {
                            ((LinkButton)e.Row.FindControl("lblOrderNumber")).ToolTip = "Anniversary Credit Used";

                        }
                        else if (new OrderConfirmationRepository().GetByOrderNo(((LinkButton)e.Row.FindControl("lblOrderNumber")).Text).CreditUsed == "Previous")
                        {
                            ((LinkButton)e.Row.FindControl("lblOrderNumber")).ToolTip = "Starting Credit Used";
                        }
                        else
                        {
                            ((LinkButton)e.Row.FindControl("lblOrderNumber")).ToolTip = "";
                        }
                    }
                }
                else
                {
                    ((LinkButton)e.Row.FindControl("lblOrderNumber")).Text = "---";
                }


            }
            if (((Label)e.Row.FindControl("lblUpdatedBy")).Text == String.Empty)
            {
                ((Label)e.Row.FindControl("lblUpdatedBy")).Text = "Auto";
            }
            if (((Incentex.DAL.SqlRepository.EmployeeLedgerResult)(e.Row.DataItem)).Note != null)
            {
                ((Label)e.Row.FindControl("lblTransactionType")).ToolTip = ((Incentex.DAL.SqlRepository.EmployeeLedgerResult)(e.Row.DataItem)).TransactionType + "  & Note >> " + ((Incentex.DAL.SqlRepository.EmployeeLedgerResult)(e.Row.DataItem)).Note;
            }
            else
            {
                ((Label)e.Row.FindControl("lblTransactionType")).ToolTip = ((Incentex.DAL.SqlRepository.EmployeeLedgerResult)(e.Row.DataItem)).TransactionType;
            }



        }
    }
    private void doPaging()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PageIndex");
        dt.Columns.Add("PageText");

        for (int i = 0; i < pds.PageCount; i++)
        {
            DataRow dr = dt.NewRow();
            dr[0] = i;
            dr[1] = i + 1;
            dt.Rows.Add(dr);
        }

        lstPaging.DataSource = dt;
        lstPaging.DataBind();
    }
    protected void lstPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindTransactionDetails();
            //bindGridView();
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

    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindTransactionDetails();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindTransactionDetails();
    }
    protected void lnkAddCredits_Click(object sender, EventArgs e)
    {
        ddlCreditType.SelectedIndex = 0;
        lblMessage.Text = string.Empty;
        modal.Show();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ddlCreditType.SelectedItem.Value == "0")
        {

            lblMessage.Text = "Please select Credits";
            modal.Show();
            return;
        }
        if (txtCrAmount.Text == string.Empty)
        {
            lblMessage.Text = "Please enter credit amount";
            modal.Show();
            return;
        }

        try
        {
            //Update value in creditstarting credit or anniversary credit
            CompanyEmployee objCompanyEmployee = new CompanyEmployee();
            CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
            objCompanyEmployee = objEmpRepo.GetById(EmployeeId);


            #region EmployeeLedger
            EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
            EmployeeLedger objEmplLedger = new EmployeeLedger();
            EmployeeLedger transaction = new EmployeeLedger();

            objEmplLedger.UserInfoId = new CompanyEmployeeRepository().GetById(this.EmployeeId).UserInfoID;
            objEmplLedger.CompanyEmployeeId = this.EmployeeId;
            if (ddlCreditType.SelectedItem.Value == "1")
            {
                // objCompanyEmployee.StratingCreditAmount = objCompanyEmployee.StratingCreditAmount + Convert.ToDecimal(txtCrAmount.Text);
                objCompanyEmployee.StratingCreditAmount = Convert.ToDecimal(txtCrAmount.Text);
                objCompanyEmployee.StartingCreditUpdatedDate = System.DateTime.Now;
                objCompanyEmployee.CreditAmtToApplied = objCompanyEmployee.CreditAmtToApplied + Convert.ToDecimal(txtCrAmount.Text);
                objCompanyEmployee.CreditAmtToExpired = objCompanyEmployee.CreditAmtToExpired + Convert.ToDecimal(txtCrAmount.Text);

                objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.STCR.ToString(); ;
                objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.StartingCredits.ToString();
            }
            else if (ddlCreditType.SelectedItem.Value == "2")
            {

                objCompanyEmployee.CreditAmtToApplied = objCompanyEmployee.CreditAmtToApplied + Convert.ToDecimal(txtCrAmount.Text);
                objCompanyEmployee.CreditAmtToExpired = objCompanyEmployee.CreditAmtToExpired + Convert.ToDecimal(txtCrAmount.Text);

                objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ANCR.ToString(); ;
                objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.AnniversaryCredits.ToString();
            }

            objEmplLedger.TransactionAmount = Convert.ToDecimal(txtCrAmount.Text);
            objEmplLedger.AmountCreditDebit = "Credit";
            objEmplLedger.Notes = txtMultiLine.Text;


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
                objEmplLedger.CurrentBalance = Convert.ToDecimal(txtCrAmount.Text);
            }

            objEmplLedger.TransactionDate = System.DateTime.Now;
            objEmplLedger.UpdateById = IncentexGlobal.CurrentMember.UserInfoID;

            //Save in table
            objEmpRepo.SubmitChanges();

            //Save in ledger
            objLedger.Insert(objEmplLedger);
            objLedger.SubmitChanges();

            //Bind
            BindTransactionDetails();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
        //Starting Credits Add
            #endregion

        ddlCreditType.SelectedValue = "0";
        txtCrAmount.Text = "";
        txtMultiLine.Text = "";
    }
}
