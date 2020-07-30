using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class OrderManagement_EditShipOrder : PageBase
{
    #region Property
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
    Int64 ShoppingCartId
    {
        get
        {
            if (ViewState["ShoppingCartId"] == null)
            {
                ViewState["ShoppingCartId"] = 0;
            }
            return Convert.ToInt64(ViewState["ShoppingCartId"]);
        }
        set
        {
            ViewState["ShoppingCartId"] = value;
        }
    }
    Int64 TotalShippedQuantity
    {
        get
        {
            if (ViewState["TotalShippedQuantity"] == null)
            {
                ViewState["TotalShippedQuantity"] = 0;
            }
            return Convert.ToInt64(ViewState["TotalShippedQuantity"]);
        }
        set
        {
            ViewState["TotalShippedQuantity"] = value;
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
    ShipOrderRepository obj = new ShipOrderRepository();
    OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
    List<SelectSizeColorDescriptionResult> ObjSize = new List<SelectSizeColorDescriptionResult>();
    Order objOrder = new Order();
    ShipingOrder objShiporder = new ShipingOrder();
    #endregion

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            CheckLogin();
          
            //Bind Menu
            menucontrol.PopulateMenu(0, 0, this.OrderID, 0, false);
            //End

            this.OrderID = Convert.ToInt64(Request.QueryString["id"]);
            setOrderStatus();
            this.ShoppingCartId = Convert.ToInt64(Request.QueryString["ShoppingCartID"]);
            this.ItemNumber = Request.QueryString["ItemNumber"].ToString();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Edit Shipment Order Detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.OrderReturnURL;
            bindGridView();
        }
    }

    #region grid events
    /// <summary>
    /// Row command event of a grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvShippedOrderDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //Saving shipment Details
            if (e.CommandName == "SaveShippingDetails")
            {
                foreach (GridViewRow gr in gvShippedOrderDetail.Rows)
                {
                    if (e.CommandArgument.ToString() == (((Label)gr.FindControl("lblShipId")).Text))
                    {
                        Int32 ShipId = Convert.ToInt32((((Label)gr.FindControl("lblShipId")).Text));
                        Int64 oldshipvalue = Convert.ToInt64(((Label)gr.FindControl("lblOldShippedValue")).Text);
                        Int64 newShipvalue = Convert.ToInt64(((TextBox)gr.FindControl("txtQtyShipped")).Text);

                        objShiporder = obj.GetById(ShipId);
                        objShiporder.ShipQuantity = Convert.ToInt64(((TextBox)gr.FindControl("txtQtyShipped")).Text);
                        objShiporder.ShipingDate = Convert.ToDateTime(((TextBox)gr.FindControl("txtBackOrderDate")).Text);
                        objShiporder.RemaingQutOrder = Convert.ToInt64(((Label)gr.FindControl("txtQtyOrder")).Text) - newShipvalue;
                        obj.SubmitChanges();

                    }
                }
               
            }
            if (e.CommandName == "deleteshiporder")
            {
                foreach (GridViewRow gr in gvShippedOrderDetail.Rows)
                {
                    if (e.CommandArgument.ToString() == (((Label)gr.FindControl("lblShipId")).Text))
                    {
                        Int32 ShipId = Convert.ToInt32((((Label)gr.FindControl("lblShipId")).Text));
                        objShiporder = obj.GetById(ShipId);
                        obj.Delete(objShiporder);
                        obj.SubmitChanges();
                        //End

                    }
                }
            }
            bindGridView();
            //End
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

        }
    }
    /// <summary>
    /// Row Data bound event of a grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvShippedOrderDetail_RowDataBound(Object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //Get necessary data here and bind it to the required fields like Color,Size and the description which are not bringing from bind grid
            HiddenField hdnOrderid;
            hdnOrderid = (HiddenField)e.Row.FindControl("hdnOrderID");
            Label lblItemNumber = (Label)e.Row.FindControl("lblItemNumber");
            Label lblColor = (Label)e.Row.FindControl("lblColor");
            Label lblSize = (Label)e.Row.FindControl("lblSize");
            Label lblDescription = (Label)e.Row.FindControl("lblDescription");
            HiddenField hdnMyShoppingCartID = (HiddenField)e.Row.FindControl("hdnMyShoppingCartId");
            objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(hdnOrderid.Value));
            if (objOrder != null)
            {

                ObjSize = obj.GetSizeColorDetails(Convert.ToInt32(hdnMyShoppingCartID.Value), Convert.ToInt32(this.OrderID), objOrder.OrderFor, lblItemNumber.Text);
                if (ObjSize.Count > 0)
                {
                    lblColor.Text = ObjSize[0].Color;
                    lblSize.Text = ObjSize[0].Size;
                    lblDescription.Text = ObjSize[0].ProductDescrption.ToString().Length > 15 ? ObjSize[0].ProductDescrption.ToString().Substring(0, 15) + "..." : ObjSize[0].ProductDescrption.ToString();
                }
                ((Image)e.Row.FindControl("imgColor")).ImageUrl = "../admin/Incentex_Used_Icons/" + new LookupRepository().GetIconByLookupName(lblColor.Text);
            }
            //End



        }
    }
    /// <summary>
    /// Row Editing event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvShippedOrderDetail_RowEditing(Object sender, GridViewEditEventArgs e)
    {
        bindGridView();
    }
    #endregion

  
    #region Functions
    /// <summary>
    /// Bind the Grid View
    /// </summary>
    /// 
    private void setOrderStatus()
    {
        Order o = new OrderConfirmationRepository().GetByOrderID(this.OrderID);
        this.OrderStatus = o.OrderStatus;
        if (this.OrderStatus.ToUpper() == "CANCELED")
        {
            lnkSaveOrderDetails.Visible = false;            
        }
    }

    public void bindGridView()
    {
        try
        {
            List<ShipingOrder> objOrderList = obj.GetShippingOrders(this.OrderID, this.ShoppingCartId, this.ItemNumber);
            if (objOrderList.Count > 0)
            {
                gvShippedOrderDetail.DataSource = objOrderList;
                gvShippedOrderDetail.DataBind();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }


    }
    protected void lnkSaveOrderDetails_Click(Object sender, EventArgs e)
    {
        try
        {
            //Below condition will not allow shipping quantity as a Zero..
            foreach (GridViewRow grv in gvShippedOrderDetail.Rows)
            {
                if (Convert.ToInt64(((TextBox)grv.FindControl("txtQtyShipped")).Text) == 0)
                {
                    lblmsg.Text = "You can not enter 0 as a shipped quantity.Please enter values again or you can delete the shiping order too!!!";
                    bindGridView();
                    return;
                }
            }
            //End

            ShipingOrder objShipOrder = new ShipingOrder();
            ShipOrderRepository objShipOrderRepository = new ShipOrderRepository();
            Int64 QuantityOrdered = 0;
            this.TotalShippedQuantity = 0;
            foreach (GridViewRow grv in gvShippedOrderDetail.Rows)
            {
                QuantityOrdered = Convert.ToInt64(((Label)grv.FindControl("txtQtyOrder")).Text);
                this.TotalShippedQuantity = this.TotalShippedQuantity + Convert.ToInt64(((TextBox)grv.FindControl("txtQtyShipped")).Text);
            }
            if (this.TotalShippedQuantity > QuantityOrdered)
            {
                lblmsg.Text = "Please check back again Shipped Quantity..It can not be more than the Quantity Ordered!!<br/>Sum of all the Shipped values can not be more than " + QuantityOrdered;
                return;
            }
            else
            {
                //Update Shipping Quantity to Zero First.
                foreach (GridViewRow grv in gvShippedOrderDetail.Rows)
                {

                    Int32 ShipId = Convert.ToInt32((((Label)grv.FindControl("lblShipId")).Text));
                    objShiporder = objShipOrderRepository.GetById(ShipId);
                    objShiporder.ShipQuantity = 0;
                    objShipOrderRepository.SubmitChanges();
                }

                //Assign Values Again
                foreach (GridViewRow grv in gvShippedOrderDetail.Rows)
                {
                    //Add Nagmani 20-Jan-2012
                    Int32 intRemainingQty = 0;
                    //End
                    CountTotalShippedQuantiyResult objTotalShipped = new CountTotalShippedQuantiyResult();
                    objTotalShipped = objShipOrderRepository.GetQtyShippedTotal((Int32)this.ShoppingCartId, this.ItemNumber, this.OrderID);
                    Int32 ShipId = Convert.ToInt32((((Label)grv.FindControl("lblShipId")).Text));
                    objShiporder = objShipOrderRepository.GetById(ShipId);
                    objShiporder.ShipQuantity = Convert.ToInt64(((TextBox)grv.FindControl("txtQtyShipped")).Text);
                    objShiporder.RemaingQutOrder = Convert.ToInt64(((Label)grv.FindControl("txtQtyOrder")).Text) - Convert.ToInt64(((TextBox)grv.FindControl("txtQtyShipped")).Text) - (Convert.ToInt32(objTotalShipped.ShipQuantity));
                    //Add Nagmani 20-Jan-2012
                    intRemainingQty = Convert.ToInt32(((Label)grv.FindControl("txtQtyOrder")).Text) - Convert.ToInt32(((TextBox)grv.FindControl("txtQtyShipped")).Text) - (Convert.ToInt32(objTotalShipped.ShipQuantity));
                    //End
                    if (((TextBox)grv.FindControl("txtBackOrderDate")).Text != "")
                    {
                        objShiporder.BackOrderUntil = Convert.ToDateTime(((TextBox)grv.FindControl("txtBackOrderDate")).Text);
                    }
                    else
                    {
                        objShiporder.BackOrderUntil = null;
                    }

                    //Add Nagmani 20-Jan-2012 Supplier Status in Shipping Status field in shipping Order table
                    if (intRemainingQty == 0)
                    {
                        objShiporder.ShippingOrderStatus = "Shipped Complete";
                    }
                    else
                    {
                        objShiporder.ShippingOrderStatus = "Partial Shipped";
                    }

                    //End
                    objShipOrderRepository.SubmitChanges();
                }
                lblmsg.Text = String.Empty;
            }
            Response.Redirect("~/OrderManagement/OrderDetail.aspx?id="+this.OrderID);
            //bindGridView();
           
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    #endregion

}
