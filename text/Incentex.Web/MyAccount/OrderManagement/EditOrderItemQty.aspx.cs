/// <summary>
/// Module Name : MOAS(Manager order approval system)
/// Description : This page is for edit MOAS order quantity
/// Created : Mayur on 25-nov-2011
/// </summary>

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_OrderManagement_EditOrderItemQty : PageBase
{
    #region Data Members
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
    String OrderFor
    {
        get
        {
            if (ViewState["OrderFor"] == null)
            {
                ViewState["OrderFor"] = null;
            }
            return Convert.ToString(ViewState["OrderFor"]);
        }
        set
        {
            ViewState["OrderFor"] = value;
        }
    }
    String ShoppingCartNumber
    {
        get
        {
            if (ViewState["ShoppingCartNumber"] == null)
            {
                ViewState["ShoppingCartNumber"] = null;
            }
            return Convert.ToString(ViewState["ShoppingCartNumber"]);
        }
        set
        {
            ViewState["ShoppingCartNumber"] = value;
        }
    }
    Int32 SupplierId
    {
        get
        {
            if (ViewState["SupplierId"] == null)
            {
                ViewState["SupplierId"] = null;
            }
            return Convert.ToInt32(ViewState["SupplierId"]);
        }
        set
        {
            ViewState["SupplierId"] = value;
        }
    }
    Decimal OriginalItemValue
    {
        get
        {
            if (ViewState["OriginalItemValue"] == null)
            {
                ViewState["OriginalItemValue"] = null;
            }
            return Convert.ToDecimal(ViewState["OriginalItemValue"]);
        }
        set
        {
            ViewState["OriginalItemValue"] = value;
        }
    }
    Decimal OriginalMOASItemValue
    {
        get
        {
            if (ViewState["OriginalMOASItemValue"] == null)
            {
                ViewState["OriginalMOASItemValue"] = null;
            }
            return Convert.ToDecimal(ViewState["OriginalMOASItemValue"]);
        }
        set
        {
            ViewState["OriginalMOASItemValue"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Order Item (Increase/ Decrease Quantity)";

            if (Request.QueryString["Id"] != null)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["Id"].ToString());
                this.OrderFor = Request.QueryString["OrderType"].ToString();
                this.ShoppingCartNumber = Request.QueryString["ShoppingCartId"].ToString();
                this.SupplierId = Convert.ToInt32(Request.QueryString["SupplierId"].ToString());
                getItem(this.OrderID, this.OrderFor, this.ShoppingCartNumber, SupplierId);
            }
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/OrderManagement/EditOrderDetail.aspx?Id=" + this.OrderID;
        }

    }

    protected void gvMainOrderDetail_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label txtQtyOrder = (Label)e.Row.FindControl("txtQtyOrder");
            Label lblItemNumber = (Label)e.Row.FindControl("lblItemNumber");
            Label txtQtyOrderRemaining = (Label)e.Row.FindControl("txtQtyOrderRemaining");
            HiddenField MyShoppingCart = (HiddenField)e.Row.FindControl("hdnMyShoppingCartiD");
            CountTotalShippedQuantiyResult objTotalShipped = new CountTotalShippedQuantiyResult();
            Label txtQtyShipped = (Label)e.Row.FindControl("txtQtyShipped");
            objTotalShipped = new ShipOrderRepository().GetQtyShippedTotal(Convert.ToInt32(MyShoppingCart.Value), lblItemNumber.Text, this.OrderID);
            if (!(String.IsNullOrEmpty(objTotalShipped.ToString())))
            {
                txtQtyOrderRemaining.Text = Convert.ToString((Convert.ToInt32(txtQtyOrder.Text)) - (Convert.ToInt32(objTotalShipped.ShipQuantity)));
            }

            ((Label)e.Row.FindControl("txtQtyShipped")).Text = objTotalShipped.ShipQuantity == null ? "0" : objTotalShipped.ShipQuantity.ToString();
            ((Image)e.Row.FindControl("imgColor")).ImageUrl = "~/admin/Incentex_Used_Icons/" + new LookupRepository().GetIconByLookupName(((SelectOrderDetailsResult)(e.Row.DataItem)).Color);


            Label lblMOASPrice = (Label)e.Row.FindControl("lblMOASPrice");
            Label lblMOASExtendedPrice = (Label)e.Row.FindControl("lblMOASExtendedPrice");
            Label lblPrice = (Label)e.Row.FindControl("lblPrice");
            Label lblExtendedPrice = (Label)e.Row.FindControl("lblExtendedPrice");
            if (lblMOASPrice.Text != "0.00")
            {
                lblPrice.Visible = false;
                lblExtendedPrice.Visible = false;
                lblMOASPrice.Visible = true;
                lblMOASExtendedPrice.Visible = true;
            }
            else
            {
                lblPrice.Visible = true;
                lblExtendedPrice.Visible = true;
                lblMOASPrice.Visible = false;
                lblMOASExtendedPrice.Visible = false;
            }
        }

    }

    protected void lnkSaveOrderDetails_Click(Object sender, EventArgs e)
    {
        try
        {
            Decimal NewPrice = 0;
            Decimal NewMOASprice = 0;
            Decimal CreditAmountToAdd = 0;
            Decimal CreditMOASAmountToAdd = 0;
            Int32 EnteredOrderedQuantity = 0;
            Int32 QtyShipped = 0;
            Int32 OriginalOrder = 0;
            Int32 QryRemaining = 0;
            //Get Order Details
            OrderConfirmationRepository objOrderRep = new OrderConfirmationRepository();
            Order ordDetails = objOrderRep.GetByOrderID(Convert.ToInt64(this.OrderID));

            #region GetCreditAmount

            foreach (GridViewRow grv in gvMainOrderDetail.Rows)
            {
                //It is foreach loop but there will be only one item in the grid everytime..
                if (String.IsNullOrEmpty(((TextBox)grv.FindControl("txtQtyCurrent")).Text))
                {
                    lblmsg.Text = "Please enter some value in the textbox field to increase or decrease the quantity!";
                    return;
                }
                EnteredOrderedQuantity = Convert.ToInt32(((TextBox)grv.FindControl("txtQtyCurrent")).Text);
                QtyShipped = Convert.ToInt32(((Label)grv.FindControl("txtQtyShipped")).Text);
                OriginalOrder = Convert.ToInt32(((Label)grv.FindControl("txtQtyOrder")).Text);
                QryRemaining = Convert.ToInt32(((Label)grv.FindControl("txtQtyOrderRemaining")).Text);

                //Check if its in decreasing mode and if quantiy ordered is same as quantity shipped
                //Return from here.
                //All decreasing logic with different case goes here!

                if (EnteredOrderedQuantity <= 0)
                {
                    lblmsg.Text = "Quantity should be greater than zero.";
                    getItem(this.OrderID, this.OrderFor, this.ShoppingCartNumber, SupplierId);
                    return;
                }
                else if (EnteredOrderedQuantity < QtyShipped)
                {
                    lblmsg.Text = "You can not decrease the quantity as everything ordered has been shipped";
                    getItem(this.OrderID, this.OrderFor, this.ShoppingCartNumber, SupplierId);
                    return;
                }
                else
                    lblmsg.Text = String.Empty;
                //End

                //extendedprice = Convert.ToDecimal(((Label)grv.FindControl("lblPrice")).Text) * (Convert.ToDecimal(((TextBox)grv.FindControl("txtQtyCurrent")).Text) + Convert.ToDecimal(((Label)grv.FindControl("txtQtyOrder")).Text) - Convert.ToDecimal(((Label)grv.FindControl("txtQtyShipped")).Text));
                if (EnteredOrderedQuantity > 0)
                {
                    //Increase Qty
                    NewPrice = Convert.ToDecimal(((Label)grv.FindControl("lblPrice")).Text) * (Convert.ToDecimal(EnteredOrderedQuantity));
                    NewMOASprice = Convert.ToDecimal(((Label)grv.FindControl("lblMOASPrice")).Text) * (Convert.ToDecimal(EnteredOrderedQuantity));
                    CreditAmountToAdd = Convert.ToDecimal(((Label)grv.FindControl("lblPrice")).Text) * (Convert.ToDecimal(OriginalOrder)) - NewPrice;
                    CreditMOASAmountToAdd = Convert.ToDecimal(((Label)grv.FindControl("lblMOASPrice")).Text) * (Convert.ToDecimal(OriginalOrder)) - NewMOASprice;
                }
            }

            #endregion

            #region Inventory
            String strMessage = String.Empty;

            foreach (GridViewRow grv in gvMainOrderDetail.Rows)
            {
                HiddenField hdnProductItemID = (HiddenField)grv.FindControl("hdnProductItemID");
                if (hdnProductItemID != null)
                {
                    ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                    strMessage = objProItemRepos.GetStatusForAllowBackOrderAndInventoryLevel(EnteredOrderedQuantity, Convert.ToInt64(hdnProductItemID.Value), strMessage);
                }

            }
            if (strMessage == "Low Inventory")
            {
                lblmsg.Text = "Low Inventory,Please Increase inventory first!!";
                return;
            }
            else
                lblmsg.Text = String.Empty;

            #endregion

            #region CreditAmount
            //IF its anniversary / starting credit then check the amount for the order's user.
            //If its not enough then return and give message to employee
            Decimal? creditamount = 0;
            //Check if credit is used 

            if (!string.IsNullOrEmpty(ordDetails.CreditUsed) && Convert.ToDecimal(ordDetails.CreditAmt) > 0)
            {
                CompanyEmployeeRepository objCmnyEmp = new CompanyEmployeeRepository();
                CompanyEmployee cmpEmpl = objCmnyEmp.GetByUserInfoId((Int64)ordDetails.UserId);
                if (ordDetails.CreditUsed == "Previous")
                {
                    creditamount = cmpEmpl.StratingCreditAmount;
                }
                else if (ordDetails.CreditUsed == "Anniversary")
                {
                    creditamount = cmpEmpl.CreditAmtToApplied;
                }


                //check if creditamount if greater then amount
                if (CreditAmountToAdd < creditamount || EnteredOrderedQuantity < OriginalOrder)
                {
                    //deduct from balance
                    if (ordDetails.CreditUsed == "Previous")
                        cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + CreditAmountToAdd;

                    cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + CreditAmountToAdd;
                    cmpEmpl.CreditAmtToExpired = cmpEmpl.CreditAmtToExpired + CreditAmountToAdd;


                    objCmnyEmp.SubmitChanges();

                    #region EmployeeLedger
                    EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                    EmployeeLedger objEmplLedger = new EmployeeLedger();
                    EmployeeLedger transaction = new EmployeeLedger();

                    objEmplLedger.UserInfoId = cmpEmpl.UserInfoID;
                    objEmplLedger.CompanyEmployeeId = cmpEmpl.CompanyEmployeeID;
                    if (ordDetails.CreditUsed == "Previous")
                    {
                        objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.STCR.ToString(); ;
                        objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.StartingCredits.ToString();
                    }
                    else
                    {
                        objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ANCR.ToString();
                        objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.AnniversaryCredits.ToString();
                    }

                    if (EnteredOrderedQuantity > OriginalOrder)
                    {
                        objEmplLedger.AmountCreditDebit = "Debit";
                        objEmplLedger.TransactionAmount = -CreditAmountToAdd;
                    }
                    else
                    {
                        objEmplLedger.AmountCreditDebit = "Credit";
                        objEmplLedger.TransactionAmount = CreditAmountToAdd;
                    }

                    objEmplLedger.OrderNumber = new OrderConfirmationRepository().GetByOrderID(this.OrderID).OrderNumber;
                    objEmplLedger.OrderId = this.OrderID;


                    //When there will be records then get the last transaction
                    transaction = objLedger.GetLastTransactionByEmplID(cmpEmpl.CompanyEmployeeID);
                    if (transaction != null)
                    {
                        objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                        if (CreditAmountToAdd < 0)
                        {
                            objEmplLedger.CurrentBalance = transaction.CurrentBalance - objEmplLedger.TransactionAmount;
                        }
                        else
                        {
                            objEmplLedger.CurrentBalance = transaction.CurrentBalance + objEmplLedger.TransactionAmount;
                        }
                    }
                    //When first time record is added for the CE
                    else
                    {
                        objEmplLedger.PreviousBalance = 0;
                        objEmplLedger.CurrentBalance = CreditAmountToAdd;
                    }

                    objEmplLedger.TransactionDate = System.DateTime.Now;
                    objEmplLedger.UpdateById = IncentexGlobal.CurrentMember.UserInfoID;
                    objLedger.Insert(objEmplLedger);
                    objLedger.SubmitChanges();

                    //Starting Credits Add
                    #endregion
                }
                else
                {
                    lblmsg.Text = "Beyond the Anniversary/Previous credit so please decrease the QTY";
                    //If credit cant applied then revert back 

                    //End
                    getItem(this.OrderID, this.OrderFor, this.ShoppingCartNumber, SupplierId);
                    return;
                }
                //goto ts;
            }

            #endregion

            //Update Values in order table as well as shopping cart tables

            #region Update Order
            //Update Order Total amount
            ordDetails.OrderAmount = ordDetails.OrderAmount - CreditAmountToAdd;
            ordDetails.MOASOrderAmount = ordDetails.MOASOrderAmount - CreditMOASAmountToAdd;
            //Done on 2-Jan-2012
            ordDetails.CreditAmt = ordDetails.CreditAmt - CreditAmountToAdd;
            ordDetails.ShippingAmount = calculateShippingChargeAgain((Decimal)ordDetails.OrderAmount);
            ordDetails.SalesTax = Decimal.Round(Convert.ToDecimal((ordDetails.OrderAmount + Convert.ToDecimal(ordDetails.ShippingAmount)) * ordDetails.StrikeIronTaxRate), 2);
            ordDetails.MOASSalesTax = Decimal.Round(Convert.ToDecimal((ordDetails.MOASOrderAmount + Convert.ToDecimal(ordDetails.ShippingAmount)) * ordDetails.StrikeIronTaxRate), 2);
            //End
            objOrderRep.SubmitChanges();

            #endregion

            #region UpdateQty In ShoppingCart

            //Upadate Qty in Shoppingcart table either in Myshoppingcart or MyIssuanceCart
            if (this.OrderFor == "ShoppingCart")
            {
                //Update in MyshoppingCart
                MyShoppingCartRepository objRep = new MyShoppingCartRepository();

                MyShoppinCart objMyShpppingCart = objRep.GetDetailsById(Convert.ToInt64(this.ShoppingCartNumber));
                objMyShpppingCart.Quantity = Convert.ToString(EnteredOrderedQuantity);
                objRep.SubmitChanges();
            }
            else
            {
                //Update In MyIssuanceCart
                MyIssuanceCartRepository objIssRep = new MyIssuanceCartRepository();
                MyIssuanceCart objMyIssuance = objIssRep.GetByIssuanceCartId(Convert.ToInt64(this.ShoppingCartNumber));
                objMyIssuance.Qty = EnteredOrderedQuantity;
                objIssRep.SubmitChanges();

            }
            #endregion
            
            //BindAgain
            getItem(this.OrderID, this.OrderFor, this.ShoppingCartNumber, SupplierId);

            lblmsg.Text = "Your changes have been saved.";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods

    private Decimal calculateShippingChargeAgain(Decimal TotalAmount)
    {
        try
        {
            Decimal[] arr = new Decimal[2];
            String a = String.Empty;
            Decimal itemprice = 0;
            Decimal itemtotalprice = 0;
            OrderConfirmationRepository objRep = new OrderConfirmationRepository();
            Order objOrderItems = objRep.GetByOrderID(this.OrderID);

            //Count Shipping on the amount 
            CompanyStore objCS = new CompanyStore();
            CompanyStoreRepository objCompStoreRep = new CompanyStoreRepository();
            objCS = objCompStoreRep.GetById((Int64)objOrderItems.StoreID);

            Decimal? MinShipAmt = 0;
            Decimal? ShipPercent = 0;
            Decimal? TotalSaleAbove = 0;
            Decimal? ShippingAmount = 0;
            MinShipAmt = objCS.MinimumShippingAmount;
            ShipPercent = objCS.ShippiingPercentOfSale;
            TotalSaleAbove = Decimal.Parse(objCS.totalsaleabove);
            if (TotalAmount <= TotalSaleAbove)
                ShippingAmount = MinShipAmt; // Shipping amount = only minimum shipping amount
            else
                ShippingAmount = MinShipAmt + ((TotalAmount - TotalSaleAbove) * ShipPercent / 100);

            arr[0] = Convert.ToDecimal(TotalAmount);
            arr[1] = Convert.ToDecimal(ShippingAmount);
            return arr[1];
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /// <summary>
    /// Gets the item from shopping cart and bind it to the grid.
    /// </summary>
    /// <param name="orderid">The orderid.</param>
    /// <param name="orderstatus">The orderstatus.</param>
    /// <param name="shoppingcartnumber">The shoppingcartnumber.</param>
    /// <param name="SupplierID">The supplier ID.</param>
    private void getItem(Int64 orderid, String orderstatus, String shoppingcartnumber, Int32 SupplierID)
    {
        OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
        List<SelectOrderDetailsResult> objDetailResult = OrderRepos.GetDetailOrder(Convert.ToInt32(shoppingcartnumber), SupplierID, orderstatus);

        gvMainOrderDetail.DataSource = objDetailResult;
        gvMainOrderDetail.DataBind();

        //Get Item value
        OriginalItemValue = Convert.ToDecimal(objDetailResult[0].Price) * Convert.ToDecimal(objDetailResult[0].Quantity);
        OriginalMOASItemValue = Convert.ToDecimal(objDetailResult[0].MOASItemPrice) * Convert.ToDecimal(objDetailResult[0].Quantity);
    }
    #endregion
}
