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

public partial class ProductReturnManagement_ProductReturnView : PageBase
{
    PagedDataSource pds = new PagedDataSource();
    ProductReturnRepository objPrdReturnRepos = new ProductReturnRepository();
    ReturnProduct objReturnPrd = new ReturnProduct();
    UserInformationRepository objUserInfoRepos = new UserInformationRepository();
    OrderConfirmationRepository ordConRepos = new OrderConfirmationRepository();
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Return Details";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/ProductReturnManagement/MYProductReturns.aspx";
           
            //Check Here for CA/CE to Inserted Return Quantity
            long userid = IncentexGlobal.CurrentMember.UserInfoID;
            UserInformation obj = new UserInformation();
            obj = objUserInfoRepos.GetById(userid);
            if (obj.Usertype == 3 || obj.Usertype == 4)
            {
                BidGridView();
                lnkReturnExchange.Visible = true;
            }
            else
            {
                lnkReturnExchange.Visible = false;
            }

        }
    }
    /// <summary>
    /// BindGriedView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BidGridView()
    {
        //Here Data Show in Gridview Who has Returned That Order Number
        //Means Uerewise Data Show Here
        List<ReturnProduct> objList = new List<ReturnProduct>();
        objList = objPrdReturnRepos.GetByUserId(IncentexGlobal.CurrentMember.UserInfoID);
        //gvProductReturn.DataSource = objList;
        //gvProductReturn.DataBind();
        DataView myDataView = new DataView();
        DataTable dataTable = ListToDataTable(objList);
        myDataView = RemoveDuplicate(dataTable).DefaultView;
        //myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gvProductReturn.DataSource = pds;
        gvProductReturn.DataBind();
        doPaging();
        if (gvProductReturn.Rows.Count > 0)
        {
            pagingtable.Visible = true;
            lblmsg.Text = "";
        }
        else
        {
            pagingtable.Visible = false;
            //lblmsg.Text = "No Record Found!";
            string myStringVariable = string.Empty;
            myStringVariable = "No Record Found!";
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                    
        }
        
    }
    protected void gvProductReturn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnOrderID = (HiddenField)e.Row.FindControl("hdnOrderID");
            HyperLink lblReturnOrder = (HyperLink)e.Row.FindControl("lblReturnOrder");
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            Label lblReferenceName = (Label)e.Row.FindControl("lblReferenceName");
            Order objNew = ordConRepos.GetByOrderID(Convert.ToInt64(hdnOrderID.Value));
            if (objNew!=null)
            {
                lblReturnOrder.Text = "RA " + objNew.OrderNumber;
            }
            OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
            OrderConfirmationRepository objOrderConfir = new OrderConfirmationRepository();
            Order objOrder = objOrderConfir.GetByOrderID(Convert.ToInt64(hdnOrderID.Value));
            if (!(string.IsNullOrEmpty(objOrder.ReferenceName)))
            {
                lblReferenceName.Text = objOrder.ReferenceName;
            }
            else
            {
                lblReferenceName.Text = "&nbsp;";
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField ProductReturnId = (HiddenField)e.Row.FindControl("hdnProductReturnId");
            ((HyperLink)e.Row.FindControl("lblReturnOrder")).NavigateUrl = "~/ProductReturnManagement/OrderProductReturns.aspx?OrderId=" + ((HiddenField)e.Row.FindControl("hdnOrderID")).Value.ToString() + "&ProductReturnId=" + ProductReturnId.Value + "&Status=" + ((Label)e.Row.FindControl("lblStatus")).Text.ToString();
        }

        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            if (this.ViewState["SortOrder"].ToString() == "ASC")
                ImgSort.ImageUrl = "";
            else
                ImgSort.ImageUrl = "";


            switch (this.ViewState["SortExp"].ToString())
            {
                case "ReturnOrder":
                    PlaceHolder placeholderplaceholderOrderNumber = (PlaceHolder)e.Row.FindControl("placeholderReturnOrder");

                    break;
                case "ReturnQty":
                    PlaceHolder placeholderReturnQty = (PlaceHolder)e.Row.FindControl("placeholderReturnQty");

                    break;
                case "ReferenceName":
                    PlaceHolder placeholderReferenceName = (PlaceHolder)e.Row.FindControl("placeholderReferenceName");

                    break;
                case "SubmitDate":
                    PlaceHolder placeholderWorkgroupName = (PlaceHolder)e.Row.FindControl("placeholderSubmitDate");

                    break;
                
                case "Status":
                    PlaceHolder placeholderStatus = (PlaceHolder)e.Row.FindControl("placeholderStatus");

                    break;


            }

        }

    }
    protected void lnkReturnExchange_Click(object sender, EventArgs e)
    {
       // Response.Redirect("ProductReturn.aspx");
    }
    protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    protected void DataList2_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BidGridView();
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

        DataList2.DataSource = dt;
        DataList2.DataBind();
    }
    protected void gvProductReturn_RowCommand(object sender, GridViewCommandEventArgs e)
    {

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

        }
    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BidGridView();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BidGridView();
    }
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
    private DataTable RemoveDuplicate(DataTable dtInput)
    {

        DataTable dtTemp = dtInput.Clone();

        foreach (DataRow drInput in dtInput.Rows)
        {

            if (dtTemp.Rows.Count == 0)
            {

                DataRow dr = dtTemp.NewRow();

                dr["CreatedBy"] = drInput["CreatedBy"].ToString();
                //dr["UpdatedBy"] = drInput["UpdatedBy"].ToString();
                dr["ItemNumber"] = drInput["ItemNumber"].ToString();
                dr["MyShoppingCartID"] = drInput["MyShoppingCartID"].ToString();

                dr["OrderId"] = drInput["OrderId"].ToString();
                dr["PackageId"] = drInput["PackageId"].ToString();

                dr["ProductReturnId"] = drInput["ProductReturnId"].ToString();
                dr["Reason"] = drInput["Reason"].ToString();
                dr["ShippId"] = drInput["ShippId"].ToString();
                dr["Status"] = drInput["Status"].ToString();
                dr["SubmitDate"] = drInput["SubmitDate"].ToString();
                dr["TrackingNumber"] = drInput["TrackingNumber"].ToString();
               
                dtTemp.Rows.Add(dr);

            }

            else
            {

                bool xIsExist = false;

                for (int idxT = 0; idxT < dtTemp.Rows.Count; idxT++)
                {

                    if (drInput["OrderId"].ToString().Equals(dtTemp.Rows[idxT]["OrderId"].ToString()))
                    {

                        xIsExist = true;

                    }

                }



                if (!xIsExist)
                {

                    DataRow dr = dtTemp.NewRow();

                    dr["CreatedBy"] = drInput["CreatedBy"].ToString();
                   // dr["UpdatedBy"] = drInput["UpdatedBy"].ToString();
                    dr["ItemNumber"] = drInput["ItemNumber"].ToString();
                    dr["MyShoppingCartID"] = drInput["MyShoppingCartID"].ToString();

                    dr["OrderId"] = drInput["OrderId"].ToString();
                    dr["PackageId"] = drInput["PackageId"].ToString();

                    dr["ProductReturnId"] = drInput["ProductReturnId"].ToString();
                    dr["Reason"] = drInput["Reason"].ToString();
                    dr["ShippId"] = drInput["ShippId"].ToString();
                    dr["Status"] = drInput["Status"].ToString();
                    dr["SubmitDate"] = drInput["SubmitDate"].ToString();
                    dr["TrackingNumber"] = drInput["TrackingNumber"].ToString();
                  

                    dtTemp.Rows.Add(dr);

                }

            }

        }



        return dtTemp;



    }
    
}
