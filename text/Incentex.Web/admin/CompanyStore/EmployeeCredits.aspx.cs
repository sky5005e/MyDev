/* Project Name : Incentex 
 * Description : The purpose of this page is to display \ edit credit limit for company employee
 * ----------------------------------------------------------------------------------------- 
 * DATE | ID/ISSUE| AUTHOR | REMARKS 
 * ----------------------------------------------------------------------------------------- 
 * 26-Oct-2010 | 1 | Amit Trivedi | Design and Coding
 * ----------------------------------------------------------------------------------------- */


using System;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;


public partial class admin_CompanyStore_EmployeeCredits : PageBase
{
    #region Properties

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
                return 5;
            else
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }
    PagedDataSource pds = new PagedDataSource();
    DataSet ds;
    CompanyEmployeeBE objCE = new CompanyEmployeeBE();
    CompanyEmployeeDA objCeDa = new CompanyEmployeeDA();
 

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        
        CheckLogin();
        if(!IsPostBack)
        {
            //Common.SetModuleMenu(Incentex.DAL.Common.DAEnums.ManageID.CompanyStore);
            Common.MenuModuleId = Incentex.DAL.Common.DAEnums.ManageID.CompanyStore; 
 
            if (Request.QueryString.Count > 0)
            {
                CheckLogin();

                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                //this.CompanyId = 2;
                Company objCompany = new CompanyRepository().GetById(this.CompanyId);
                if (objCompany == null)
                {
                   // Response.Redirect("~/admin/Company/ViewCompany.aspx");
                }
                //((HtmlGenericControl)menucontrol.FindControl("dvSubMenu")).Visible = false;
                
                // ((Label)Master.FindControl("lblPageHeading")).Text = "Employee Listing";
                ((Label)Master.FindControl("lblPageHeading")).Text = objCompany.CompanyName + " Employees Credit";
                //((Label)Master.FindControl("lblPageHeading")).Text = "Company Record ";

                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/ViewCompanyStore.aspx";

                bindgrid();
            }
            else
            {
                Response.Redirect("~/admin/CompanyStore/ViewCompanyStore.aspx");
            }
        }
    }


    #region Methods
    #endregion

    private void bindgrid()
    {
        try
        {
            DataView myDataView = new DataView();
            objCE.Operations = "selectcompanyemployeeforcredit";
            objCE.CompanyId = this.CompanyId;
            ds = objCeDa.CompanyEmployee(objCE);
            if (ds.Tables[0].Rows.Count == 0)
            {
                dvPaging.Visible = false;
                lnkSave.Visible = false;
                //lnkBtnDelete.Visible = false;
            }
            else
            {
                dvPaging.Visible = true;
                lnkSave.Visible = true;
                //lnkBtnDelete.Visible = true;
            }



            myDataView = ds.Tables[0].DefaultView;
            if (this.ViewState["SortExp"] != null)
            {
                myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
            }
            pds.DataSource = myDataView;
            pds.AllowPaging = true;
            pds.PageSize = 50; // client needs list of 50
            pds.CurrentPageIndex = CurrentPage;
            lnkbtnNext.Enabled = !pds.IsLastPage;
            lnkbtnPrevious.Enabled = !pds.IsFirstPage;
            gvEmployee.DataSource = pds;
            gvEmployee.DataBind();

            doPaging();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

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
                ToPg = ToPg + 5;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - 5;

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

            dtlViewEmployee.DataSource = dt;
            dtlViewEmployee.DataBind();

        }
        catch (Exception)
        { }
    }

    private string getSortDirectionString(SortDirection sortDireciton)
    {
        string newSortDirection = String.Empty;
        if (sortDireciton == SortDirection.Ascending)
        {
            newSortDirection = "ASC";
        }
        else
        {
            newSortDirection = "DESC";
        }

        return newSortDirection;
    }

    #region Events

    protected void gvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            if (this.ViewState["SortOrder"].ToString() == "ASC")
                ImgSort.ImageUrl = "";
            else
                ImgSort.ImageUrl = "";


            switch (this.ViewState["SortExp"].ToString())
            {
                case "FullName":
                    PlaceHolder placeholderEmployeeName = (PlaceHolder)e.Row.FindControl("placeholderEmployeeName");
                    //placeholderEmployeeName.Controls.Add(ImgSort);
                    break;
                case "Country":
                    PlaceHolder placeholderCountry = (PlaceHolder)e.Row.FindControl("placeholderCountry");
                    // placeholderCountry.Controls.Add(ImgSort);
                    break;
                case "State":
                    PlaceHolder placeholderState = (PlaceHolder)e.Row.FindControl("placeholderState");
                    //placeholderState.Controls.Add(ImgSort);
                    break;
                case "Telephone":
                    PlaceHolder placeholdercontactnumber = (PlaceHolder)e.Row.FindControl("placeholdercontactnumber");
                    //placeholdercontactnumber.Controls.Add(ImgSort);
                    break;


            }

        }//if

        if(e.Row.RowType == DataControlRowType.DataRow )
        {
            DataRow drItem = ((DataRowView)e.Row.DataItem).Row;
            
            TextBox txtStratingCreditAmount = e.Row.FindControl("txtStratingCreditAmount") as TextBox;
            HiddenField hdnStartingCredit = e.Row.FindControl("hdnStartingCredit") as HiddenField;
            HiddenField hdnAnniversary = e.Row.FindControl("hdnAnniversary") as HiddenField;

            if (drItem["StratingCreditAmount"].ToString() == "" )
            {
                drItem["StratingCreditAmount"] = "0.00";
            }

            txtStratingCreditAmount.Text = drItem["StratingCreditAmount"].ToString();
            hdnStartingCredit.Value = drItem["StratingCreditAmount"].ToString();
            //Start
            long userinfoId =new CompanyEmployeeRepository().GetById(Convert.ToInt64(((Label)e.Row.FindControl("lblCompanyEmployeeID")).Text)).UserInfoID;
            SelectAnniversaryCreditProgramPerEmployeeResult objCE = new AnniversaryProgramRepository().GetCompanyEmployeeAnniversaryCreditDetails(userinfoId);
            if (objCE.AnniversaryCreditBalance.ToString() != "")
            {
             
                ((TextBox)e.Row.FindControl("txtAnniversaryCreditAmount")).Text = objCE.AnniversaryCreditBalance.ToString();
                 hdnAnniversary.Value = objCE.AnniversaryCreditBalance.ToString();
            }
            else
            {

                ((TextBox)e.Row.FindControl("txtAnniversaryCreditAmount")).Text ="0.00";
                hdnAnniversary.Value = "0";
            }
           //End

            //Start
            if (new OrderConfirmationRepository().GetUserIdsWithInCompletedOrders(userinfoId))
            {
                //Orderid is there in the incomplete order so unlock here
                ((TextBox)e.Row.FindControl("txtAnniversaryCreditAmount")).Enabled = true;
                ((LinkButton)e.Row.FindControl("lnkSaveAnnive")).Visible = true;
                
            }
            else
            {
                //Keep locked textbox here
                ((TextBox)e.Row.FindControl("txtAnniversaryCreditAmount")).Enabled = false;
                ((LinkButton)e.Row.FindControl("lnkSaveAnnive")).Visible = false;
            }

            //End
        }
    }
    protected void gvEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblMsg.Text = "";
        if (e.CommandName.Equals("Sort"))
        {
            if (this.ViewState["SortExp"] == null)
            {
                this.ViewState["SortExp"] = e.CommandArgument.ToString();
                this.ViewState["SortOrder"] = "ASC";
            }
            else
            {
                if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
                {
                    if (this.ViewState["SortOrder"].ToString() == "ASC")
                        this.ViewState["SortOrder"] = "DESC";
                    else
                        this.ViewState["SortOrder"] = "ASC";

                }
                else
                {
                    this.ViewState["SortOrder"] = "ASC";
                    this.ViewState["SortExp"] = e.CommandArgument.ToString();
                }
            }

            bindgrid();
        }
        if (e.CommandName == "vieweditces")
        {
            
            Response.Redirect("BasicInformation.aspx?Id=" + this.CompanyId + "&SubId=" + e.CommandArgument.ToString() + "&PP=EC");
        }
        else if (e.CommandName == "EditEmp")
        {
            Response.Redirect("~/admin/Company/Employee/BasicInformation.aspx?Id=" + this.CompanyId + "&SubId=" + e.CommandArgument + "&PP=EC");
        }
        else if (e.CommandName == "saveAnniversary")
        {
            if (((TextBox)((LinkButton)e.CommandSource).NamingContainer.FindControl("txtAnniversaryCreditAmount")).Text != null)
            {
              CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
              CompanyEmployee objEmployee = objCompanyEmployeeRepository.GetById(Convert.ToInt64(e.CommandArgument.ToString()));
              objEmployee.CreditAmtToApplied = Common.GetDecimal(((TextBox)((LinkButton)e.CommandSource).NamingContainer.FindControl("txtAnniversaryCreditAmount")).Text);
              objEmployee.CreditAmtToExpired = Common.GetDecimal(((TextBox)((LinkButton)e.CommandSource).NamingContainer.FindControl("txtAnniversaryCreditAmount")).Text);
              objCompanyEmployeeRepository.SubmitChanges();

             
              #region EmployeeLedger
              HiddenField hdnAnniversary = ((HiddenField)((LinkButton)e.CommandSource).NamingContainer.FindControl("hdnAnniversary"));
              TextBox txtAnniversaryCreditAmount = ((TextBox)((LinkButton)e.CommandSource).NamingContainer.FindControl("txtAnniversaryCreditAmount"));
              Label lblCompanyEmployeeID = ((Label)((LinkButton)e.CommandSource).NamingContainer.FindControl("lblCompanyEmployeeID"));
              if (txtAnniversaryCreditAmount.Text != "0.00")
              {
                  //If value is changed then add record only.
                  if (hdnAnniversary.Value != txtAnniversaryCreditAmount.Text)
                  {
                      EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                      EmployeeLedger objEmplLedger = new EmployeeLedger();
                      EmployeeLedger transaction = new EmployeeLedger();

                      objEmplLedger.UserInfoId = objEmployee.UserInfoID;
                      objEmplLedger.CompanyEmployeeId = Convert.ToInt64(lblCompanyEmployeeID.Text);
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
    }

    protected void gvEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployee.PageIndex = e.NewPageIndex;
        bindgrid();
    }

    protected void lnkBtnAddEmployee_Click(object sender, EventArgs e)
    {
        Common.MenuModuleId = null;
        //PP = previous page , EC = employee credit
        Response.Redirect("~/admin/Company/Employee/BasicInformation.aspx?Id=" + this.CompanyId + "&SubId=0" + "&PP=EC");

    }

    #region Paging

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        CurrentPage += 1;
        bindgrid();
    }

    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        CurrentPage -= 1;
        bindgrid();
    }

    protected void dtlViewEmployee_ItemCommand(object source, DataListCommandEventArgs e)
    {
        lblMsg.Text = "";
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindgrid();
        }
    }
    protected void dtlViewEmployee_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    #endregion

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        if(gvEmployee.Rows.Count == 0 )
        {
            return;
        }

        CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
        try
        {
            foreach (GridViewRow gr in gvEmployee.Rows)
            {
                if (((CheckBox)gr.FindControl("chkSelectUser")).Checked == true)
                {
                    TextBox txtStratingCreditAmount = gr.FindControl("txtStratingCreditAmount") as TextBox;
                    Label lblCompanyEmployeeID = gr.FindControl("lblCompanyEmployeeID") as Label;
                    HiddenField hdnStartingCredit = gr.FindControl("hdnStartingCredit") as HiddenField;

                    if (string.IsNullOrEmpty(txtStratingCreditAmount.Text))
                    {
                        txtStratingCreditAmount.Text = "0";
                    }
                    CompanyEmployee objEmployee = objCompanyEmployeeRepository.GetById(Convert.ToInt64(lblCompanyEmployeeID.Text));
                    objEmployee.StratingCreditAmount = Common.GetDecimal(txtStratingCreditAmount.Text);
                    objEmployee.StartingCreditUpdatedDate = DateTime.Now;
                    objEmployee.CreditAmtToApplied = Convert.ToDecimal(objEmployee.CreditAmtToApplied) + Common.GetDecimal(txtStratingCreditAmount.Text);
                    objEmployee.CreditAmtToExpired = Convert.ToDecimal(objEmployee.CreditAmtToExpired) + Common.GetDecimal(txtStratingCreditAmount.Text);

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
            }
            lblMsg.Text = "Employee Credits are updated";
            objCompanyEmployeeRepository.SubmitChanges();
            bindgrid();
        }
        catch(Exception ex)
        {
            lblMsg.Text = "Error in saving record";
            ErrHandler.WriteError(ex);
        }
        

    }

    #endregion




}
