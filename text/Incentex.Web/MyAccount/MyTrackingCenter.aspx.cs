using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data;

public partial class MyAccount_MyTrackingCenter : PageBase
{
    AnniversaryProgramRepository.CompanyAnniversarySortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = AnniversaryProgramRepository.CompanyAnniversarySortExpType.OrderID;
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
                ViewState["SortOrder"] = Incentex.DAL.Common.DAEnums.SortOrderType.Desc;
            }
            return (Incentex.DAL.Common.DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }
    PagedDataSource pds = new PagedDataSource();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();

            if (IncentexGlobal.IsIEFromStore)
            {
                this.Title = "My Tracking Center";
                ((Label)Master.FindControl("lblPageHeading")).Text = "My Tracking Center";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    this.Title = "Track Order";
                    ((Label)Master.FindControl("lblPageHeading")).Text = "Track Order";
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/TrackingCenter.aspx";
                }
                else
                {
                    this.Title = "My Tracking Center";
                    ((Label)Master.FindControl("lblPageHeading")).Text = "My Tracking Center";
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
                }
            }
            BindTrackingCenterGrid();

        }
    }
    private void BindTrackingCenterGrid()
    {
        //Bind Anniversary Order Hisotry
        DataView myDataView = new DataView();
        AnniversaryProgramRepository o = new AnniversaryProgramRepository();
        List<SelectOrderHistoryPerEmployeeResult> objExtList = new List<SelectOrderHistoryPerEmployeeResult>();
        objExtList = o.GetOrderHistoryByEmployee(IncentexGlobal.CurrentMember.UserInfoID, this.SortExp, this.SortOrder, "Tracking");
        //gvEmployeeTrackingCenter.DataSource 
        //gvEmployeeTrackingCenter.DataBind();

        if (objExtList.Count == 0)
        {
            pagingtable.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
        }


        DataTable dataTable = ListToDataTable(objExtList);
        myDataView = dataTable.DefaultView;
        //if (this.ViewState["SortExp"] != null)
        //{
        //    myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        //}
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gvEmployeeTrackingCenter.DataSource = pds;
        gvEmployeeTrackingCenter.DataBind();
        doPaging();
    }

    //Allow NULLABLE 
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            //table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            dt.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    protected void gvEmployeeTrackingCenter_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    this.SortExp = (AnniversaryProgramRepository.CompanyAnniversarySortExpType)Enum.Parse(typeof(AnniversaryProgramRepository.CompanyAnniversarySortExpType), e.CommandArgument.ToString());
                }
                BindTrackingCenterGrid();
                break;
            case "OrderDetail":
                Response.Redirect("~/MyAccount/MyOrderDetails.aspx?OrderId=" + e.CommandArgument);
                break;
        }
    }
    protected void gvEmployeeTrackingCenter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblPaymentMethod = new Label();
            lblPaymentMethod = ((Label)e.Row.FindControl("lblPaymentMethod"));

            if (lblPaymentMethod != null)
            {
                Label lblOrderAmountView = new Label();
                lblOrderAmountView = ((Label)e.Row.FindControl("lblOrderAmountView"));
                Label lblMOASOrderAmountView = new Label();
                lblMOASOrderAmountView = ((Label)e.Row.FindControl("lblMOASOrderAmountView"));
                if (lblPaymentMethod.Text.Trim() == Convert.ToString(Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS) && (IncentexGlobal.CurrentMember.Usertype == (long)Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin || IncentexGlobal.CurrentMember.Usertype == (long)Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin || IncentexGlobal.CurrentMember.Usertype == (long)Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin || IncentexGlobal.CurrentMember.Usertype == (long)Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                {
                    if (lblOrderAmountView != null && lblMOASOrderAmountView != null && lblMOASOrderAmountView.Text != "" && Convert.ToDecimal(lblMOASOrderAmountView.Text) > 0)
                    {
                        lblOrderAmountView.Visible = false;
                        lblMOASOrderAmountView.Visible = true;
                    }
                    else
                    {
                        lblOrderAmountView.Visible = true;
                        lblMOASOrderAmountView.Visible = false;
                    }
                }
                else
                {
                    lblOrderAmountView.Visible = true;
                    lblMOASOrderAmountView.Visible = false;
                }
            }

            //if (!String.IsNullOrEmpty(((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[5].ToString()))
            //{
            //    INC_Lookup objLookup = new LookupRepository().GetById(Convert.ToInt64(((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[5]));
            //    ((Label)e.Row.FindControl("lblPaymentMethod")).Text = objLookup.sLookupName;
            //}
            //else
            //{
            //    ((Label)e.Row.FindControl("lblPaymentMethod")).Text = "Paid By Corporate";
            //}
            ((Label)e.Row.FindControl("lblOrderSubmitedDate")).Text = Convert.ToDateTime(((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[4]).ToShortDateString();
        }

    }
    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindTrackingCenterGrid();
        }
    }
    protected void dtlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
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

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();

        }
        catch (Exception)
        { }

    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindTrackingCenterGrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindTrackingCenterGrid();
    }
    #endregion
}
