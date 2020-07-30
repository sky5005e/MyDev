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

public partial class ProductReturnManagement_ReturnProductSubView : PageBase
{
    Int64 OrderID
    {
        get
        {
            if (ViewState["OrderId"] == null)
            {
                ViewState["OrderId"] = 0;
            }
            return Convert.ToInt64(ViewState["OrderId"]);
        }
        set
        {
            ViewState["OrderId"] = value;
        }
    }
    ProductReturnRepository objRepos = new ProductReturnRepository();
    ShipOrderRepository OrdShippOrder = new ShipOrderRepository();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Return View";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/ProductReturnManagement/ReturnProductIE.aspx";
            //((HtmlImage)Master.FindControl("imgShoppingCart")).Visible = true;
            if (Request.QueryString["OrderId"] != null)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["OrderId"]);
                BindGridview();
            }
        }
    }
    private void BindGridview()
    {
        List<ReturnProductDetailsOnOrderIDResult> objList = new List<ReturnProductDetailsOnOrderIDResult>();
        objList = objRepos.GetProductReturnOnOrderID(this.OrderID);
        if (objList.Count > 0)
        {
            gvProductReturn.DataSource = objList;
            gvProductReturn.DataBind();
            lblOrderNo.Text = "RA" + objList[0].OrderNumber;
            lblRequest.Text = objList[0].Requesting.ToString();
            lblShipper.Text = objList[0].Shipper;
            txtPrdDescription.Text = objList[0].Reason;
            lblTrackingNumber.Text = objList[0].TrackingNumber;
        }
    }
    protected void gvProductReturn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            List<ShippedReturnProductDescriptionResult> objNew = new List<ShippedReturnProductDescriptionResult>();
            HiddenField hdnOrderID = (HiddenField)e.Row.FindControl("hdnOrderID");
            HiddenField hdnitemNo = (HiddenField)e.Row.FindControl("hdnitemNo");
            HiddenField hdnShoppingCartId = (HiddenField)e.Row.FindControl("hdnShoppingCartId");
            Label lblColor = (Label)e.Row.FindControl("lblColor");
            Label lblSize = (Label)e.Row.FindControl("lblSize");
            Label lblProductDescription = (Label)e.Row.FindControl("lblProductionDescription");
            DropDownList ddlRequest = (DropDownList)e.Row.FindControl("ddlRequesting");
            objNew = OrdShippOrder.GetDesCription(Convert.ToInt32(this.OrderID), Convert.ToInt32(hdnShoppingCartId.Value), hdnitemNo.Value);
            if (objNew.Count > 0)
            {
                lblColor.Text = objNew[0].Color;
                lblSize.Text = objNew[0].Size;
                lblProductDescription.Text = objNew[0].ProductDescrption;

            }
            

        }
    }
}
