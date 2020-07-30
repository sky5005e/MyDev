using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class OrderManagement_OrderQtyReceived : PageBase
{
   Int64 OrderID
    {
        get
        {
            if (ViewState["OrderID"] == null)
            {
                ViewState["OrderID"] = 0;
            }
            return Convert.ToInt64(ViewState["OrderID"]);
        }
        set
        {
            ViewState["OrderID"] = value;
        }
    }
    Int64 MyShoppingCartID
    {
        get
        {
            if (ViewState["MyShoppingCartID"] == null)
            {
                ViewState["MyShoppingCartID"] = 0;
            }
            return Convert.ToInt64(ViewState["MyShoppingCartID"]);
        }
        set
        {
            ViewState["MyShoppingCartID"] = value;
        }
    }
    Int64 SupplierID
    {
        get
        {
            if (ViewState["SupplierID"] == null)
            {
                ViewState["SupplierID"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierID"]);
        }
        set
        {
            ViewState["SupplierID"] = value;
        }
    }
    String ItemNumber
    {
        get
        {
            if (ViewState["ItemNumber"] == null)
            {
                ViewState["ItemNumber"] = 0;
            }
            return Convert.ToString(ViewState["ItemNumber"]);
        }
        set
        {
            ViewState["ItemNumber"] = value;
        }
    }
    String ShippID
    {
        get
        {
            if (ViewState["ShippID"] == null)
            {
                ViewState["ShippID"] = 0;
            }
            return ViewState["ShippID"].ToString();
        }
        set
        {
            ViewState["ShippID"] = value;
        }
    }
    String OrderStatus
    {
        get
        {
            if (ViewState["OrderStatus"] == null)
            {
                ViewState["OrderStatus"] = null;
            }
            return Convert.ToString(ViewState["OrderStatus"]);
        }
        set
        {
            ViewState["OrderStatus"] = value;
        }
    }

    List<SelectShippedQuantityOrderResult> objOrderDoc = new List<SelectShippedQuantityOrderResult>();
    ShipOrderRepository obj = new ShipOrderRepository();
    List<SelectOrderDetailsResult> obj1 = new List<SelectOrderDetailsResult>();
    OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
    List<SelectSizeColorDescriptionResult> ObjSize = new List<SelectSizeColorDescriptionResult>();
    Order objOrder = new Order();
    ShipingOrder objShiporder = new ShipingOrder();

    public class TrackingNumber
    {
        public String trackingnuber { get; set; }
        public String PackageId { get; set; }
    }

    private static List<TrackingNumber> ListValuesTracking
    {
        get
        {
            if ((HttpContext.Current.Session["ListValuesTracking"]) == null)
                return null;
            else
                return (List<TrackingNumber>)HttpContext.Current.Session["ListValuesTracking"];
        }
        set
        {
            HttpContext.Current.Session["ListValuesTracking"] = value;
        }
    }

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            this.OrderID = Convert.ToInt64(Request.QueryString["OrderID"]);
            setOrderStatus();
            //((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Order Quantity Received Detail";
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Packing Slip Details";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/MyOrderDetails.aspx?OrderId=" + this.OrderID;
            //((HtmlImage)Master.FindControl("imgShoppingCart")).Visible = true;
            bindGridView();
        }
    }
    public void bindGridView()
    {
        try
        {
            //Below is not a ShipId its a PackageId..
            this.ShippID = Request.QueryString["ShippID"];

            objOrderDoc = obj.GetShippedQtyOrderDetails(ShippID,OrderID);
            if (objOrderDoc.Count > 0)
            {
                gvShippedOrderDetail.DataSource = objOrderDoc;
                gvShippedOrderDetail.DataBind();
            }
            SetValues(Convert.ToDateTime(objOrderDoc[0].ShipingDate), Convert.ToInt64(objOrderDoc[0].ShipperService), objOrderDoc[0].TrackingNo);


        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }


    }

    public void SetValues(DateTime ShipDate, Int64 ShipperId, String TrackingNumber)
    {
        String strPayment = "Shipping Type";
        /*ddlShipper.DataSource = new LookupRepository().GetByLookup(strPayment);
        ddlShipper.DataValueField = "iLookupID";
        ddlShipper.DataTextField = "sLookupName";
        ddlShipper.DataBind();
        ddlShipper.Items.Insert(0, new ListItem("-Select-", "0"));*/
        if (TrackingNumber != null)
        {
            String[] listTrackingNumbers = TrackingNumber.Split(',');
            List<TrackingNumber> objTempList = new List<TrackingNumber>();
            foreach (String s in listTrackingNumbers)
            {
                TrackingNumber item = new TrackingNumber();
                item.PackageId = this.ShippID;
                item.trackingnuber = s;
                objTempList.Add(item);
            }
            ListValuesTracking = objTempList;
            //grvTrackingNumber.DataSource = ListValuesTracking;
            //grvTrackingNumber.DataBind();
        }

        //Set Values
        txtShipDate.Text = ShipDate.ToString("MM/dd/yyyy");
        //lblShipper.Text = new LookupRepository().GetById(ShipperId).sLookupName;
        //ddlShipper.Items.FindByValue(ShipperId.ToString()).Selected = true;
        //End

    }
    private void bindtrackingnumbergrid()
    {
        //grvTrackingNumber.DataSource = ListValuesTracking;
        //grvTrackingNumber.DataBind();

    }
    protected void grvTrackingNumber_RowDeleting(Object sender, GridViewDeleteEventArgs e)
    {
        ListValuesTracking.RemoveAt(e.RowIndex);
        bindtrackingnumbergrid();
    }

    protected void gvShippedOrderDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "SaveReceivedQty")
            {
                foreach (GridViewRow gr in gvShippedOrderDetail.Rows)
                {
                    if (e.CommandArgument.ToString() == (((Label)gr.FindControl("lblShipId")).Text))
                    {
                        if (Convert.ToInt32(((Label)gr.FindControl("lblQtyShip")).Text) < Convert.ToInt64(((TextBox)gr.FindControl("txtQtyReceived")).Text))
                        {
                            lblmsg.Text = "You can not enter received more the shipping order";
                            return;
                        }
                        else
                        {
                            ShipOrderRepository objRep = new ShipOrderRepository();
                            ShipingOrder objShipingOrder = objRep.GetById(Convert.ToInt32(e.CommandArgument));
                            objShipingOrder.QuantityReceived = Convert.ToInt64(((TextBox)gr.FindControl("txtQtyReceived")).Text);
                            objRep.SubmitChanges();
                            lblmsg.Text = String.Empty;
                        }
                    }
                }
                
                bindGridView();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

        }
    }
    protected void gvShippedOrderDetail_RowDataBound(Object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //HiddenField hdnSupplierIDForEdit;
            HiddenField hdnOrderid;
            hdnOrderid = (HiddenField)e.Row.FindControl("hdnOrderID");
            Label lblItemNumber = (Label)e.Row.FindControl("lblItemNumber");
            Label lblColor = (Label)e.Row.FindControl("lblColor");
            Label lblSize = (Label)e.Row.FindControl("lblSize");
            Label lblDescription = (Label)e.Row.FindControl("lblDescription");
            HiddenField hdnMyShoppingCartID = (HiddenField)e.Row.FindControl("hdnMyShoppingCartId");
            //  hdnSupplierIDForEdit = (HiddenField)e.Row.FindControl("hdnSupplierIDForEdit");
            this.OrderID = Convert.ToInt64(Request.QueryString["OrderID"]);
            objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(hdnOrderid.Value));
            if (objOrder != null)
            {

                ObjSize = obj.GetSizeColorDetails(Convert.ToInt32(hdnMyShoppingCartID.Value), Convert.ToInt32(this.OrderID), objOrder.OrderFor, lblItemNumber.Text);
                if (ObjSize.Count > 0)
                {
                    lblColor.Text = ObjSize[0].Color;
                    lblSize.Text = ObjSize[0].Size;
                    lblDescription.Text = ObjSize[0].ProductDescrption.ToString().Length > 20 ? ObjSize[0].ProductDescrption.ToString().Substring(0, 20) + "..." : ObjSize[0].ProductDescrption.ToString();
                }
            }

            //Disable if Qty Received is alreay filled and if its same as Shipped Qty..
            if (((TextBox)e.Row.FindControl("txtQtyReceived")).Text != "")
            {
                if (Convert.ToInt32(((Label)e.Row.FindControl("lblQtyShip")).Text) == Convert.ToInt64(((TextBox)e.Row.FindControl("txtQtyReceived")).Text))
                {
                    ((TextBox)e.Row.FindControl("txtQtyReceived")).Enabled = false;
                    ((ImageButton)e.Row.FindControl("lnkbtndelete")).Visible = false;
                }
            }
            //Added on 18 Apr By Ankit
            if (String.IsNullOrEmpty(((Label)e.Row.FindControl("lblBackOrderUntil")).Text))
            {
                ((Label)e.Row.FindControl("lblBackOrderUntil")).Text = "---";
            }
            //end
            ((Image)e.Row.FindControl("imgColor")).ImageUrl = "../admin/Incentex_Used_Icons/" + new LookupRepository().GetIconByLookupName(lblColor.Text);

            if (this.OrderStatus.ToUpper() == "CANCELED")
            {
                ((ImageButton)e.Row.FindControl("lnkbtndelete")).Visible = false;
            }
        }
    }
    private void setOrderStatus()
    {
        Order o = new OrderConfirmationRepository().GetByOrderID(this.OrderID);
        this.OrderStatus = o.OrderStatus;
    }
    protected void gvShippedOrderDetail_RowEditing(Object sender, GridViewEditEventArgs e)
    {
        bindGridView();
    }

    protected void grvTrackingNumber_RowCommand(Object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "deletetrackingnumber")
        {
            ListValuesTracking.RemoveAll(delegate(TrackingNumber t) { return t.trackingnuber == e.CommandArgument.ToString(); });
            bindtrackingnumbergrid();
        }
    }
    protected void lnkSaveOrderDetails_Click(Object sender, EventArgs e)
    {
        try
        {
            //Tracking Number Logic
            
            List<ShipingOrder> objShippingOrderList = new List<ShipingOrder>();
            ShipOrderRepository objRep = new ShipOrderRepository();
            objShippingOrderList = objRep.GetByPackageId(this.ShippID);
            foreach (ShipingOrder objShiporder in objShippingOrderList)
            {
                String tranum = String.Empty;
                if (ListValuesTracking.Count > 0)
                {
                    foreach (TrackingNumber tn in ListValuesTracking)
                    {
                        if (tranum == String.Empty)
                        {
                            tranum = tn.trackingnuber;
                        }
                        else
                        {
                            tranum = tranum + "," + tn.trackingnuber;
                        }
                    }
                    objShiporder.TrackingNo = tranum;
                }
                else
                {
                    objShiporder.TrackingNo = null;
                }
                
                objShiporder.ShipingDate = Convert.ToDateTime(txtShipDate.Text);
                //objShiporder.ShipperService = Convert.ToInt64(ddlShipper.SelectedItem.Value);
                objRep.SubmitChanges();
            }
            bindGridView();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnTrackingNumber_Click(Object sender, EventArgs e)
    {

        try
        {
            List<TrackingNumber> obj = new List<TrackingNumber>();
            TrackingNumber objnumber = new TrackingNumber();
            //objnumber.trackingnuber = (txtTrackingNo.Text);
            objnumber.PackageId = this.ShippID;
            ListValuesTracking.Add(objnumber);
            //if (Session["ListVal"] != null)
            //{
            //    List<TrackingNumber> objtemp = new List<TrackingNumber>();
            //    objtemp = ListValuesTracking;
            //    objtemp.Add(objnumber);
            //    obj = objtemp;
            //    Session["ListVal"] = obj;
            //}
            //else
            //{
            //    obj.Add(objnumber);
            //    Session["ListVal"] = obj;
            //}

            bindtrackingnumbergrid();
            //txtTrackingNo.Text = String.Empty;
        }
        catch (Exception ex)
        {
            Session["ListVal"] = null;
            ListValuesTracking = null;
            ex = null;
        }


    }
}

