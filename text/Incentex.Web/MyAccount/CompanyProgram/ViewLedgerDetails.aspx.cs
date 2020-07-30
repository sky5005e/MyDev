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
            CheckLogin();

            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            if (IncentexGlobal.IsIEFromStore)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/CompanyProgram/MyAnniversaryCredits.aspx";
            }
            ((Label)Master.FindControl("lblPageHeading")).Text = "View Transaction Log";
            BindTransactionDetails();
        }

    }
    public void BindTransactionDetails()
    {
        List<EmployeeLedgerResult> objEmplHis = new List<EmployeeLedgerResult>();
        EmployeeLedgerRepository objLedgerHistory = new EmployeeLedgerRepository();
        objEmplHis = objLedgerHistory.GetEmployeeLedgerByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID, this.SortExp, this.SortOrder);
        if (objEmplHis.Count == 0)
        {
            lblMsg.Text = "No Transactions found";
            dvPaging.Visible = false;
            return;
        }
        else
        {
            dvPaging.Visible = true;
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
                Response.Redirect("OrderDetail.aspx?id=" + e.CommandArgument.ToString()+"&from=lgr");
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
                    ((LinkButton)e.Row.FindControl("lblOrderNumber")).Enabled = false;
                }
            }
            else
            {
                ((LinkButton)e.Row.FindControl("lblOrderNumber")).Text = "---";
                ((LinkButton)e.Row.FindControl("lblOrderNumber")).Enabled = false;
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
}
