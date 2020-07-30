/// <summary>
/// Module Name : MOAS(Manager order approval system)
/// Description : This page is for edit order detail for order comming from moas.
/// Created : Mayur on 18-nov-2011
/// </summary>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using MagayaService;

public partial class MyAccount_OrderManagement_EditOrderDetail : PageBase
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

    Int32 WTDCSupplierID
    {
        get
        {
            if (ViewState["WTDCSupplierID"] == null)
            {
                ViewState["WTDCSupplierID"] = new SupplierRepository().GetSinglSupplierid("WTDC").SupplierID;
            }
            return Convert.ToInt32(ViewState["WTDCSupplierID"]);
        }
        set
        {
            ViewState["WTDCSupplierID"] = value;
        }
    }

    Boolean IsMOASWithCostCenterCode
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsMOASWithCostCenterCode"]);
        }
        set
        {
            ViewState["IsMOASWithCostCenterCode"] = value;
        }
    }

    OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
    UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
    CompanyRepository objCmpRepo = new CompanyRepository();
    Order objOrder = new Order();
    MyIssuanceCartRepository objMyIssCartRepo = new MyIssuanceCartRepository();
    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
    CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    OrderMOASSystemRepository objOrderMOASSystemRepository = new OrderMOASSystemRepository();
    List<SelectMyShoppingCartProductResult> objSelectMyShoppingCartProductResultList;
    LookupRepository objLookupRepository = new LookupRepository();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();

    IncentexBEDataContext db = new IncentexBEDataContext();


    Decimal MOASTotalAmountForCAIE
    {
        get
        {
            if (ViewState["MOASTotalAmountForCAIE"] == null)
                return 0;
            return Convert.ToDecimal(ViewState["MOASTotalAmountForCAIE"]);
        }
        set
        {
            ViewState["MOASTotalAmountForCAIE"] = value;
        }
    }
    #endregion

    #region Event Handlers
    protected void Page_Load(Object sender, EventArgs e)
    {
        if (IncentexGlobal.CurrentMember == null)
        {
            Server.Transfer("~/login.aspx");
        }

        if (!IsPostBack)
        {
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Edit Order Detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/OrderManagement/MyPendingOrders.aspx";

            if (Request.QueryString["Id"] != null)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["Id"]);
                hdORderID.Value = this.OrderID.ToString();

                SetBillingShipping();
                BindOrderDetails();
                BindOrderItems();
            }
        }
    }

    protected void gvMainOrderDetail_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label txtQtyOrder = (Label)e.Row.FindControl("txtQtyOrder");
            //updated by prashant on 28th Feb 2013
            Label lblMOASPrice = (Label)e.Row.FindControl("lblMOASPrice");
            Label lblMOASExtendedPrice = (Label)e.Row.FindControl("lblMOASExtendedPrice");
            Label lblPrice = (Label)e.Row.FindControl("lblPrice");
            Label lblExtendedPrice = (Label)e.Row.FindControl("lblExtendedPrice");
            Label lblProductItemID = (Label)e.Row.FindControl("lblProductItemID");

            if (lblProductItemID != null && txtQtyOrder != null)
            {
                var result = OrderRepos.GetMOASPriceByUserIDProductItemID(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(lblProductItemID.Text));
                if (result != null)
                {
                    switch (result.MOASPaymentPricing)
                    {
                        case "1":
                            lblMOASPrice.Text = Convert.ToDecimal(result.Level1Price).ToString("f2");
                            lblMOASExtendedPrice.Text = Convert.ToDecimal(result.Level1Price * Convert.ToInt32(txtQtyOrder.Text)).ToString("f2");
                            break;
                        case "2":
                            lblMOASPrice.Text = Convert.ToDecimal(result.Level2Price).ToString("f2");
                            lblMOASExtendedPrice.Text = Convert.ToDecimal(result.Level2Price * Convert.ToInt32(txtQtyOrder.Text)).ToString("f2");
                            break;
                        case "3":
                            lblMOASPrice.Text = Convert.ToDecimal(result.Level3Price).ToString("f2");
                            lblMOASExtendedPrice.Text = Convert.ToDecimal(result.Level3Price * Convert.ToInt32(txtQtyOrder.Text)).ToString("f2");
                            break;
                        case "4":
                            lblMOASPrice.Text = Convert.ToDecimal(result.Level4Price).ToString("f2");
                            lblMOASExtendedPrice.Text = Convert.ToDecimal(result.Level4Price * Convert.ToInt32(txtQtyOrder.Text)).ToString("f2");
                            break;
                        default:
                            lblMOASPrice.Text = Convert.ToDecimal(result.Level1Price).ToString("f2");
                            lblMOASExtendedPrice.Text = Convert.ToDecimal(result.Level1Price * Convert.ToInt32(txtQtyOrder.Text)).ToString("f2");
                            break;
                    }
                    MOASTotalAmountForCAIE += Convert.ToDecimal(lblMOASExtendedPrice.Text);
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
            else
            {
                lblPrice.Visible = true;
                lblExtendedPrice.Visible = true;
                lblMOASPrice.Visible = false;
                lblMOASExtendedPrice.Visible = false;
            }

            //updated by prashant on 28th Feb 2013
            Label lblItemNumber = (Label)e.Row.FindControl("lblItemNumber");
            Label txtQtyOrderRemaining = (Label)e.Row.FindControl("txtQtyOrderRemaining");
            HiddenField MyShoppingCart = (HiddenField)e.Row.FindControl("hdnMyShoppingCartiD");
            CountTotalShippedQuantiyResult objTotalShipped = new CountTotalShippedQuantiyResult();
            Label txtQtyShipped = (Label)e.Row.FindControl("txtQtyShipped");
            objTotalShipped = objShipOrderRepos.GetQtyShippedTotal(Convert.ToInt32(MyShoppingCart.Value), lblItemNumber.Text, this.OrderID);
            if (!(String.IsNullOrEmpty(objTotalShipped.ToString())))
            {
                txtQtyOrderRemaining.Text = Convert.ToString((Convert.ToInt32(txtQtyOrder.Text)) - (Convert.ToInt32(objTotalShipped.ShipQuantity)));
            }

            ((Label)e.Row.FindControl("txtQtyShipped")).Text = objTotalShipped.ShipQuantity == null ? "---" : objTotalShipped.ShipQuantity.ToString();
            ((Image)e.Row.FindControl("imgColor")).ImageUrl = "~/admin/Incentex_Used_Icons/" + new LookupRepository().GetIconByLookupName(((SelectOrderDetailsResult)(e.Row.DataItem)).Color);

            //Hide delete button added on 13 July 2011

            if (objTotalShipped.ShipQuantity == null)
            {
                //((LinkButton)e.Row.FindControl("hypDeleteShippedItem")).Visible = false;
                ((LinkButton)e.Row.FindControl("hypDeleteShippedItem")).Visible = true;
                //((LinkButton)e.Row.FindControl("hypEditShippedItem")).Visible = true;

            }
            else
            {
                //If item is not shipped but shipped from the first tab of the page!
                List<ShipingOrder> objShipOrderCount = new ShipOrderRepository().GetShippingOrdersItemWise(this.OrderID, Convert.ToInt64(((LinkButton)e.Row.FindControl("hypDeleteShippedItem")).CommandArgument));
                if (objShipOrderCount.Count > 0)
                {
                    ((LinkButton)e.Row.FindControl("hypDeleteShippedItem")).Visible = false;
                    //((LinkButton)e.Row.FindControl("hypEditShippedItem")).Visible = false;
                }
                else
                {
                    //((LinkButton)e.Row.FindControl("hypDeleteShippedItem")).Visible = false;
                    ((LinkButton)e.Row.FindControl("hypDeleteShippedItem")).Visible = true;
                    //((LinkButton)e.Row.FindControl("hypEditShippedItem")).Visible = true;
                }
            }


        }

    }

    protected void gvMainOrderDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteOrderItem")
        {
            Int64 CartId;
            CartId = Convert.ToInt64(e.CommandArgument.ToString());
            Decimal ItemAmount = 0;
            Decimal MOASItemAmount = 0;
            try
            {
                #region Inventory
                OrderConfirmationRepository objOrderItem = new OrderConfirmationRepository();
                Order ordDetails = objOrderItem.GetByOrderID(Convert.ToInt64(this.OrderID));

                GridViewRow a = ((GridViewRow)((LinkButton)e.CommandSource).Parent.Parent);
                if (ordDetails.OrderFor == "ShoppingCart")
                {
                    //Shopping cart
                    MyShoppingCartRepository objShoppingCartRepos = new MyShoppingCartRepository();
                    MyShoppinCart objShoppingcart = new MyShoppinCart();
                    ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                    ProductItem objProductItem = new ProductItem();
                    objShoppingcart = objShoppingCartRepos.GetById(Convert.ToInt32(CartId), (Int64)ordDetails.UserId);
                    if (objShoppingcart != null)
                    {
                        objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(objShoppingcart.StoreProductID), Convert.ToInt64(objShoppingcart.MasterItemNo), objShoppingcart.ItemNumber);
                        //Update Inventory Here 
                        //Call here upDate Procedure
                        String strProcess = "Shopping";


                        String strMessage = OrderRepos.IncreaseDescreaseInventory(objProductItem.ProductItemID, -Convert.ToInt32(((Label)gvMainOrderDetail.Rows[a.RowIndex].FindControl("txtQtyOrder")).Text), strProcess);
                    }

                }
                else
                {
                    //Issuance
                    MyIssuanceCartRepository objIssuanceRepos = new MyIssuanceCartRepository();
                    MyIssuanceCart objIssuance = new MyIssuanceCart();
                    ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                    //End 

                    objIssuance = objIssuanceRepos.GetById(Convert.ToInt32(CartId), (Int64)ordDetails.UserId);
                    List<SelectProductIDResult> objList = objProItemRepos.GetProductId(objIssuance.MasterItemID, Convert.ToInt64(CartId), Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(ordDetails.StoreID));
                    //Update Inventory Here 
                    //Call here upDate Procedure
                    for (Int32 i = 0; i < objList.Count; i++)
                    {
                        String strProcess = "UniformIssuance";
                        String strMessage = OrderRepos.IncreaseDescreaseInventory(Convert.ToInt64(objList[i].ProductItemID), -Convert.ToInt32(((Label)gvMainOrderDetail.Rows[a.RowIndex].FindControl("txtQtyOrder")).Text), strProcess);

                    }
                }
                #endregion

                #region Delete Order

                //Check order type
                //If its Shoping cart then delete from MyshoppingCart else delete from myIssuancecart
                //Delete Item from the shopping or issuance cart before that get value of Qua and Price from the table
                if (hdnOrderType.Value == "ShoppingCart")
                {

                    MyShoppingCartRepository objMyshoppingCartRep = new MyShoppingCartRepository();
                    MyShoppinCart objMyshoppingCart = objMyshoppingCartRep.GetDetailsById(CartId);
                    //Get Item Amount
                    ItemAmount = Convert.ToDecimal(objMyshoppingCart.UnitPrice) * Convert.ToDecimal(objMyshoppingCart.Quantity);
                    if (objMyshoppingCart.MOASUnitPrice != null && objOrder.PaymentOption == (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS)
                    {
                        MOASItemAmount = Convert.ToDecimal(objMyshoppingCart.MOASUnitPrice) * Convert.ToDecimal(objMyshoppingCart.Quantity);
                    }
                    else
                    {
                        MOASItemAmount = ItemAmount;
                    }
                    //Delete
                    objMyshoppingCartRep.Delete(objMyshoppingCart);
                    objMyshoppingCartRep.SubmitChanges();
                }
                else
                {

                    MyIssuanceCartRepository objIssuanceCartRep = new MyIssuanceCartRepository();
                    MyIssuanceCart objMyIssuanceCart = objIssuanceCartRep.GetByIssuanceCartId(CartId);
                    //Get Item Amount
                    ItemAmount = Convert.ToDecimal(objMyIssuanceCart.Rate) * Convert.ToDecimal(objMyIssuanceCart.Qty);
                    if (objMyIssuanceCart.MOASRate != null && objOrder.PaymentOption == (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS)
                    {
                        MOASItemAmount = Convert.ToDecimal(objMyIssuanceCart.MOASRate) * Convert.ToDecimal(objMyIssuanceCart.Qty);
                    }
                    //Delete
                    objIssuanceCartRep.Delete(objMyIssuanceCart);
                    objIssuanceCartRep.SubmitChanges();

                }
                #endregion

                #region CreditAmount

                //Give credit back to the user if its used
                /*If order gets cancled then give back the credit amount he has used*/
                if (ordDetails.CreditUsed != null)
                {
                    CompanyEmployeeRepository objCmnyEmp = new CompanyEmployeeRepository();
                    CompanyEmployee cmpEmpl = objCmnyEmp.GetByUserInfoId((Int64)ordDetails.UserId);
                    if (ordDetails.CreditUsed == "Previous")
                    {
                        if (ItemAmount < ordDetails.CreditAmt)
                        {
                            cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + ItemAmount + ordDetails.ShippingAmount;
                            ordDetails.CreditAmt = ordDetails.CreditAmt - cmpEmpl.StratingCreditAmount;
                        }
                        else
                        {
                            cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + ordDetails.CreditAmt;
                        }

                        objCmnyEmp.SubmitChanges();

                        ordDetails.CreditAmt = ordDetails.CreditAmt - cmpEmpl.StratingCreditAmount;
                        objOrderItem.SubmitChanges();
                    }
                    else if (ordDetails.CreditUsed == "Anniversary")
                    {

                        if (ItemAmount < ordDetails.CreditAmt)
                        {
                            cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + ItemAmount + ordDetails.ShippingAmount;
                        }
                        else
                        {
                            cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + ordDetails.CreditAmt;
                        }
                        objCmnyEmp.SubmitChanges();

                        ordDetails.CreditAmt = ordDetails.CreditAmt - cmpEmpl.CreditAmtToApplied;
                        objOrderItem.SubmitChanges();
                    }
                    else
                    {

                    }
                }
                #region Update orderamount

                //Update values in the Order table like order amount (subtract value)
                if (ordDetails.CreditUsed != null)
                {
                    Order updateOrder = OrderRepos.GetByOrderID(this.OrderID);
                    //Below will check if the last item is deleted
                    if (updateOrder.MyShoppingCartID.Contains(','))
                    {
                        updateOrder.OrderAmount = updateOrder.OrderAmount - ItemAmount;
                        if (updateOrder.MOASOrderAmount != null && updateOrder.MOASOrderAmount > 0)
                        {
                            updateOrder.MOASOrderAmount = updateOrder.MOASOrderAmount - MOASItemAmount;
                        }
                    }
                    //Last item in the cart
                    else
                    {
                        updateOrder.OrderAmount = updateOrder.OrderAmount - MOASItemAmount;
                        updateOrder.ShippingAmount = 0;
                    }

                    OrderRepos.SubmitChanges();
                }
                #endregion
                #endregion

                //Update values again(Page Load again)
                SetBillingShipping();
                BindOrderDetails();
                BindOrderItems();
                //End
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else if (e.CommandName == "EditOrderItem")
        {
            GridViewRow a = ((GridViewRow)((LinkButton)e.CommandSource).Parent.Parent);
            Response.Redirect("EditOrderItemQty.aspx?Id=" + this.OrderID + "&ShoppingCartId=" + e.CommandArgument.ToString() + "&OrderType=" + hdnOrderType.Value + "&SupplierId=" + ((HiddenField)gvMainOrderDetail.Rows[a.RowIndex].FindControl("hdnSupplierId")).Value);
        }
    }

    protected void drpCountry_SelectedIndexChanged(Object sender, EventArgs e)
    {
        Common.BindState(drpState, Convert.ToInt64(drpCountry.SelectedValue));
        drpState_SelectedIndexChanged(sender, e);
        modalEditBilling.Show();

    }

    protected void drpState_SelectedIndexChanged(Object sender, EventArgs e)
    {
        Common.BindCity(drpCity, Convert.ToInt64(drpState.SelectedValue));
        modalEditBilling.Show();
    }

    protected void btnEditBilling_Click(Object sender, EventArgs e)
    {
        SetBillingAddress();
        modalEditBilling.Show();

    }

    protected void btnEditShipping_Click(Object sender, EventArgs e)
    {
        SetShippingAddress();
        modalEditBilling.Show();
    }

    protected void btnSaveBilling_Click(Object sender, EventArgs e)
    {
        try
        {
            if (btnSaveBilling.Text == "Edit Billing")
            {
                CompanyEmployeeContactInfoRepository objCmpEmpBilling = new CompanyEmployeeContactInfoRepository();
                objBillingInfo = objCmpEmpBilling.GetBillingDetailsByID((Convert.ToInt64(hfBillingInfoID.Value.ToString())));

                //Edit Billing Details here
                objBillingInfo.ZipCode = txtZipcodeEdit.Text.Trim();
                objBillingInfo.BillingCO = txtBillingFnameEdit.Text.Trim();
                objBillingInfo.Manager = txtBillingLnameEdit.Text.Trim();
                objBillingInfo.CompanyName = txtBillingCompanyNameEdit.Text.Trim();
                objBillingInfo.Address = txtBillingAddressEdit.Text.Trim();
                objBillingInfo.Address2 = txtBillingAddress2Edit.Text.Trim();
                objBillingInfo.Email = txtEmailAdrressEdit.Text.Trim();
                objBillingInfo.Telephone = txtPhoneNumberEdit.Text.Trim();
                objBillingInfo.CountryID = Convert.ToInt64(drpCountry.SelectedItem.Value.ToString());
                objBillingInfo.StateID = Convert.ToInt64(drpState.SelectedItem.Value.ToString());
                objBillingInfo.CityID = Convert.ToInt64(drpCity.SelectedItem.Value);
                objCmpEmpBilling.SubmitChanges();
            }
            else
            {
                CompanyEmployeeContactInfoRepository objCmpEmpShipping = new CompanyEmployeeContactInfoRepository();
                objShippingInfo = objCmpEmpShipping.GetShippingDetailsByID((Convert.ToInt64(hfShippingInfoID.Value.ToString())));
                //Edit Shipping details here
                objShippingInfo.ZipCode = txtZipcodeEdit.Text.Trim();
                objShippingInfo.Name = txtBillingFnameEdit.Text.Trim();
                objShippingInfo.Fax = txtBillingLnameEdit.Text.Trim();
                objShippingInfo.CompanyName = txtBillingCompanyNameEdit.Text.Trim();
                objShippingInfo.Address = txtBillingAddressEdit.Text.Trim();
                objShippingInfo.Address2 = txtBillingAddress2Edit.Text.Trim();
                objShippingInfo.Street = txtBillingStreetEdit.Text.Trim();
                objShippingInfo.Email = txtEmailAdrressEdit.Text.Trim();
                objShippingInfo.Telephone = txtPhoneNumberEdit.Text.Trim();
                objShippingInfo.CountryID = Convert.ToInt64(drpCountry.SelectedItem.Value.ToString());
                objShippingInfo.StateID = Convert.ToInt64(drpState.SelectedItem.Value.ToString());
                objShippingInfo.CityID = Convert.ToInt64(drpCity.SelectedItem.Value);
                objCmpEmpShipping.SubmitChanges();
            }
            //Clear the fields
            clearBillingDetails();
            //Bind Updated Details again
            BindOrderDetails();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void imgbtnApproveOrder_Click(Object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Order objOrder = OrderRepos.GetByOrderID(Convert.ToInt64(this.OrderID));
        UserInformation objUser = objUsrInfoRepo.GetById(Convert.ToInt64(objOrder.UserId));

        OrderMOASSystem objOrderMOASSystem = objOrderMOASSystemRepository.GetByOrderIDAndManagerUserInfoID(objOrder.OrderID, IncentexGlobal.CurrentMember.UserInfoID);
        objOrderMOASSystem.Status = "Open";
        objOrderMOASSystem.DateAffected = DateTime.Now;
        objOrderMOASSystemRepository.SubmitChanges();

        //updated on 4th dec by prashant
        //updated on prashant april 2013
        PreferenceRepository objPreferenceRepository = new PreferenceRepository();
        String ApproverLevel = objPreferenceRepository.GetPreferenceValuesByPreferenceValueID(objOrder.MOASApproverLevelID);
        if (!String.IsNullOrEmpty(ApproverLevel) && ApproverLevel.ToLower() == "companylevel")
        {

            List<OrderMOASSystem> objOrderMOASSystemList = objOrderMOASSystemRepository.GetByOrderIDAndStatus(objOrder.OrderID, "Order Pending");

            if (objOrderMOASSystemList != null && objOrderMOASSystemList.Count > 0)
            {
                AddNoteHistory("Approved", objOrder.OrderID);

                UserInformation objUserInformation = new UserInformation();
                objUserInformation = objUsrInfoRepo.GetById(objOrderMOASSystemList[0].ManagerUserInfoID);
                sendVerificationEmailMOASManager(objOrder.OrderID, objUserInformation.LoginEmail, objUserInformation.FirstName, objUserInformation.UserInfoID);
            }
            else
            {
                UpdateOrderAndSendConfirmation(objOrder, objUser);

            }
        }
        else
        {
            //Update the Received status for the order (If the MOAS Approver level is set at Station Level
            //Hence, Only one Approver needs to approve the order.    
            UpdateOrderAndSendConfirmation(objOrder, objUser);
        }
        Response.Redirect("~/MyAccount/OrderManagement/MyPendingOrders.aspx?action=approve");
    }

    protected void lnkbtnCancelNow_Click(Object sender, EventArgs e)
    {
        try
        {
            Order objCancelOrder = OrderRepos.GetByOrderID(Convert.ToInt64(this.OrderID));
            UserInformation objCancelUser = objUsrInfoRepo.GetById(Convert.ToInt64(objCancelOrder.UserId));

            objCancelOrder.OrderStatus = "Canceled";
            objCancelOrder.UpdatedDate = DateTime.Now;
            OrderRepos.SubmitChanges();

            //start changes added by mayur for maintain history of manager cancel order on 6-feb-2012
            OrderMOASSystem objOrderMOASSystem = objOrderMOASSystemRepository.GetByOrderIDAndManagerUserInfoID(objCancelOrder.OrderID, IncentexGlobal.CurrentMember.UserInfoID);
            objOrderMOASSystem.Status = "Canceled";
            objOrderMOASSystem.DateAffected = DateTime.Now;
            objOrderMOASSystemRepository.SubmitChanges();
            //end changes added by mayur for maintain history of manager cancel order on 6-feb-2012

            AddNoteHistory("Canceled", objCancelOrder.OrderID);

            #region Give back the credit amount he has used for this order
            if (objCancelOrder.CreditUsed != null)
            {
                CompanyEmployeeRepository objCmnyEmp = new CompanyEmployeeRepository();
                CompanyEmployee cmpEmpl = objCmnyEmp.GetByUserInfoId((Int64)objCancelOrder.UserId);
                if (objCancelOrder.CreditUsed == "Previous")
                {
                    cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + objCancelOrder.CreditAmt;
                    objCmnyEmp.SubmitChanges();
                }
                else if (objCancelOrder.CreditUsed == "Anniversary")
                {
                    cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + objCancelOrder.CreditAmt;
                    objCmnyEmp.SubmitChanges();
                }
                else
                {

                }
            }
            #endregion

            #region update inventory and transfer order to shopping cart
            List<Order> obj = OrderRepos.GetShoppingCartId(objCancelOrder.OrderNumber);
            if (obj.Count > 0)
            {
                String[] a;

                a = obj[0].MyShoppingCartID.ToString().Split(',');

                foreach (String u in a)
                {
                    if (objCancelOrder.OrderFor == "ShoppingCart")
                    {
                        //Shopping cart
                        MyShoppinCart objShoppingcart = new MyShoppinCart();
                        ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                        ProductItem objProductItem = new ProductItem();
                        objShoppingcart = objShoppingCartRepository.GetById(Convert.ToInt32(u), (Int64)objCancelOrder.UserId);

                        objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(objShoppingcart.StoreProductID), Convert.ToInt64(objShoppingcart.MasterItemNo), objShoppingcart.ItemNumber);
                        //Update Inventory Here 
                        //Call here upDate Procedure
                        String strProcess = "Shopping";
                        String strMessage = OrderRepos.IncreaseDescreaseInventory(objProductItem.ProductItemID, -Convert.ToInt32(objShoppingcart.Quantity), strProcess);

                    }
                    else
                    {
                        //Issuance
                        MyIssuanceCart objIssuance = new MyIssuanceCart();
                        ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                        //End 

                        objIssuance = objMyIssCartRepo.GetById(Convert.ToInt32(u), (Int64)objCancelOrder.UserId);
                        List<SelectProductIDResult> objList = objProItemRepos.GetProductId(objIssuance.MasterItemID, Convert.ToInt64(u), Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(objCancelOrder.StoreID));
                        //Update Inventory Here 
                        //Call here upDate Procedure
                        for (Int32 i = 0; i < objList.Count; i++)
                        {
                            String strProcess = "UniformIssuance";
                            String strMessage = OrderRepos.IncreaseDescreaseInventory(Convert.ToInt64(objList[i].ProductItemID), -Convert.ToInt32(objIssuance.Qty), strProcess);
                            //String strMessage = objRepos.UpdateInventory(Convert.ToInt64(u), Convert.ToInt64(objList[i].ProductItemID), strProcess);
                        }
                    }
                }
            }
            #endregion

            sendVerificationEmail("Canceled", objCancelOrder.OrderID, objCancelUser.LoginEmail, objCancelUser.FirstName); //send email to CE

            if (!objCancelOrder.ReferenceName.ToLower().Contains("test") && !IncentexGlobal.IsIEFromStoreTestMode)
            {
                sendIEEmail("Canceled", objCancelOrder.OrderID);//send email to IE
            }
            else
            {
                sendTestOrderEmailNotification("Canceled", objCancelOrder.OrderID);
            }

            Response.Redirect("~/MyAccount/OrderManagement/MyPendingOrders.aspx?action=cancel");
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Binds the order details.
    /// </summary>
    private void BindOrderDetails()
    {
        try
        {
            OrderConfirmationRepository objOrderConfir = new OrderConfirmationRepository();
            Order objOrder = objOrderConfir.GetByOrderID(this.OrderID);
            lblOrderNo.Text = objOrder.OrderNumber;
            hdnOrderType.Value = objOrder.OrderFor;
            this.OrderStatus = objOrder.OrderStatus;
            if (this.OrderStatus.ToUpper() == "CANCELED")
            {
                btnEditBilling.Enabled = false;
                btnEditShipping.Enabled = false;
            }
            else
            {
                btnEditBilling.Enabled = true;
                btnEditShipping.Enabled = true;
            }

            this.IsMOASWithCostCenterCode = new CompanyEmployeeRepository().FUN_IsMOASWithCostCenterCode(Convert.ToInt64(objOrder.UserId));

            lblShipping.Text = Convert.ToDecimal(objOrder.ShippingAmount).ToString();
            lblStrikeIronSalesTax.Text = Convert.ToDecimal(objOrder.StrikeIronTaxRate).ToString();
            lblSalesTax.Text = (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)).ToString();
            lblOrderTotal.Text = Convert.ToString(Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.SalesTax) + Convert.ToDecimal(objOrder.ShippingAmount));
            lblOrderedDate.Text = Convert.ToDateTime(objOrder.OrderDate).ToString("MM/dd/yyyy");
            lblOrderBy.Text = objOrder.ReferenceName.ToString();

            //Get and Bind if there is NameBars is used in the order
            String nb = NameBars();
            if (nb == String.Empty)
            {
                txtOrderNotesForCECA.Text = objOrder.SpecialOrderInstruction;
            }
            else
            {
                if (objOrder.SpecialOrderInstruction != "")
                {
                    txtOrderNotesForCECA.Text = objOrder.SpecialOrderInstruction + "\n\n\n" + nb;
                }
                else
                {
                    txtOrderNotesForCECA.Text = objOrder.SpecialOrderInstruction + nb;
                }
            }
            if (objOrder.PaymentOption != null)
            {
                INC_Lookup objLookup = new LookupRepository().GetById((Int64)objOrder.PaymentOption);
                lblPaymentMethod.Text = objLookup.sLookupName + (!String.IsNullOrEmpty(objOrder.PaymentOptionCode) ? " : " + objOrder.PaymentOptionCode : "");
            }
            else
            {
                lblPaymentMethod.Text = "Paid By Corporate";
            }
            //End Added

            if (objOrder.CreditUsed == "Previous")
            {
                lblCreditType.Text = "Starting Credits";
                trCreditType.Visible = true;
            }
            else if (objOrder.CreditUsed == "Anniversary")
            {
                lblCreditType.Text = "Anniversary Credits";
                trCreditType.Visible = true;
            }
            else
            {
                trCreditType.Visible = false;
            }


            lblOrderStatus.Text = objOrder.OrderStatus.ToString();

            if (objOrder.OrderFor == "IssuanceCart")
            {
                //Comapny Pays
                objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                if (objBillingInfo != null)
                {
                    BindBillingAddress();
                }

                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                if (objShippingInfo != null)
                {
                    BindShippingAddress();
                }
            }
            else
            {
                objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                if (objBillingInfo != null)
                {
                    BindBillingAddress();
                }

                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                if (objShippingInfo != null)
                {
                    BindShippingAddress();
                }

            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Binds the billing address of order.
    /// </summary>
    protected void BindBillingAddress()
    {
        lblBAddress1.Text = objBillingInfo.Address;
        lblBAddress2.Text = objBillingInfo.Address2;
        lblBCity.Text = objCity.GetById((Int64)objBillingInfo.CityID).sCityName + "," + objState.GetById((Int64)objBillingInfo.StateID).sStatename + " " + objBillingInfo.ZipCode;
        lblBCompany.Text = objBillingInfo.CompanyName;
        lblBPhoneNumber.Text = objBillingInfo.Telephone;
        lblBEmailAddress.Text = objBillingInfo.Email;
        lblBCountry.Text = objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName;
        //lblBEmail.Text = objBillingInfo.Email;
        lblBName.Text = objBillingInfo.BillingCO + " " + objBillingInfo.Manager;
        hfBillingInfoID.Value = objBillingInfo.CompanyContactInfoID.ToString();
        //lblBPhone.Text = objBillingInfo.Telephone;
    }

    /// <summary>
    /// Binds the shipping address of order.
    /// </summary>
    protected void BindShippingAddress()
    {
        lblSAddress.Text = objShippingInfo.Address;
        lblSAddress2.Text = objShippingInfo.Address2;
        lblSStreet.Text = objShippingInfo.Street;
        lblSCity.Text = objCity.GetById((Int64)objShippingInfo.CityID).sCityName + "," + objState.GetById((Int64)objShippingInfo.StateID).sStatename + " " + objShippingInfo.ZipCode;
        lblSCompany.Text = objShippingInfo.CompanyName;
        lblSCountry.Text = objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName;
        lblSName.Text = objShippingInfo.Name + " " + objShippingInfo.Fax;
        lblSPhoneNumber.Text = objShippingInfo.Telephone;
        lblSEmailAddress.Text = objShippingInfo.Email;
        hfShippingInfoID.Value = objShippingInfo.CompanyContactInfoID.ToString();
    }

    /// <summary>
    /// Visible true/false billing/shipping button based on conditions
    /// </summary>
    private void SetBillingShipping()
    {
        List<ShipingOrder> objList = new ShipOrderRepository().GetAllShippOrder(this.OrderID);
        objList = objList.Where((delegate(ShipingOrder s) { return s.IsShipped == true; })).ToList();
        if (objList.Count > 0)
        {
            btnEditBilling.Visible = false;
            btnEditShipping.Visible = false;
        }
        else
        {
            btnEditBilling.Visible = true;
            btnEditShipping.Visible = true;
        }
    }

    /// <summary>
    /// Set the billing address of order for edit billing address popup control.
    /// </summary>
    protected void SetBillingAddress()
    {
        btnSaveBilling.Text = "Edit Billing";
        OrderConfirmationRepository objOrderConfirEdit = new OrderConfirmationRepository();
        Order objOrderEdit = objOrderConfirEdit.GetByOrderID(this.OrderID);

        if (objOrderEdit.OrderFor == "IssuanceCart")
        {
            //Comapny Pays
            objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrderEdit.UserId, objOrderEdit.OrderID);
        }
        else
        {
            objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrderEdit.UserId, objOrderEdit.OrderID);
        }

        //Bind Country
        Common.BindCountry(drpCountry);
        drpCountry.Items.FindByValue(objBillingInfo.CountryID.ToString()).Selected = true;

        //Bind State
        Common.BindState(drpState, (Int64)objBillingInfo.CountryID);
        drpState.Items.FindByValue(objBillingInfo.StateID.ToString()).Selected = true;

        //bind City
        Common.BindCity(drpCity, (Int64)objBillingInfo.StateID);
        drpCity.Items.FindByValue(objBillingInfo.CityID.ToString()).Selected = true;

        txtZipcodeEdit.Text = objBillingInfo.ZipCode;

        txtBillingFnameEdit.Text = objBillingInfo.BillingCO;
        txtBillingLnameEdit.Text = objBillingInfo.Manager;
        txtBillingCompanyNameEdit.Text = objBillingInfo.CompanyName;
        txtBillingAddressEdit.Text = objBillingInfo.Address;
        txtBillingAddress2Edit.Text = objBillingInfo.Address2;
        txtEmailAdrressEdit.Text = objBillingInfo.Email;
        txtPhoneNumberEdit.Text = objBillingInfo.Telephone;

        dvHolderFacebook.Style.Remove("top");
        dvHolderFacebook.Style.Add("top", "15%");
        dvContent.Style.Remove("height");
        dvContent.Style.Add("height", "440px");
        lblAddressEdit.InnerText = "Address 1 :";
        txtBillingAddressEdit.MaxLength = 0;
        //dvAddress2Edit.Visible = false;
        dvStreetEdit.Visible = false;
    }

    /// <summary>
    /// Sets the shipping address of order for edit shipping address popup control.
    /// </summary>
    protected void SetShippingAddress()
    {

        btnSaveBilling.Text = "Edit Shipping";

        OrderConfirmationRepository objOrderConfirEdit = new OrderConfirmationRepository();
        Order objOrderEdit = objOrderConfirEdit.GetByOrderID(this.OrderID);

        if (objOrderEdit.OrderFor == "IssuanceCart")
        {
            objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrderEdit.UserId, objOrderEdit.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
        }
        else
        {
            objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrderEdit.UserId, objOrderEdit.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
        }

        //Bind Country
        Common.BindCountry(drpCountry);
        drpCountry.Items.FindByValue(objShippingInfo.CountryID.ToString()).Selected = true;

        //Bind State
        Common.BindState(drpState, (Int64)objShippingInfo.CountryID);
        drpState.Items.FindByValue(objShippingInfo.StateID.ToString()).Selected = true;

        //bind City
        Common.BindCity(drpCity, (Int64)objShippingInfo.StateID);
        drpCity.Items.FindByValue(objShippingInfo.CityID.ToString()).Selected = true;

        txtZipcodeEdit.Text = objShippingInfo.ZipCode;
        txtBillingFnameEdit.Text = objShippingInfo.Name;
        txtBillingLnameEdit.Text = objShippingInfo.Fax;
        txtBillingCompanyNameEdit.Text = objShippingInfo.CompanyName;
        txtBillingAddressEdit.Text = objShippingInfo.Address;
        txtBillingAddress2Edit.Text = objShippingInfo.Address2;
        txtEmailAdrressEdit.Text = objShippingInfo.Email;
        txtPhoneNumberEdit.Text = objShippingInfo.Telephone;

        dvHolderFacebook.Style.Remove("top");
        dvHolderFacebook.Style.Add("top", "15%");
        dvContent.Style.Remove("height");
        dvContent.Style.Add("height", "440px");
        lblAddressEdit.InnerText = "Address 1 :";
        txtBillingAddressEdit.MaxLength = 35;
        dvAddress2Edit.Visible = true;
        dvStreetEdit.Visible = true;
    }

    /// <summary>
    /// Binds the order items on the gvMainOrderDetail gridview.
    /// </summary>
    protected void BindOrderItems()
    {
        try
        {
            List<SelectSupplierAddressResult> obj = new List<SelectSupplierAddressResult>();
            objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(this.OrderID));
            if (objOrder != null)
            {
                String[] MyShoppingcart;
                MyShoppingcart = objOrder.MyShoppingCartID.Split(',');
                obj = OrderRepos.GetSupplierAddress(objOrder.OrderFor, this.OrderID);
            }
            List<SelectOrderDetailsResult> objRecord = new List<SelectOrderDetailsResult>();

            foreach (SelectSupplierAddressResult repeaterItem in obj)
            {

                List<SelectOrderDetailsResult> obj1 = new List<SelectOrderDetailsResult>();
                String[] MyShoppingcart;
                MyShoppingcart = objOrder.MyShoppingCartID.Split(',');
                for (Int32 i = 0; i < MyShoppingcart.Count(); i++)
                {
                    obj1 = OrderRepos.GetDetailOrder(Convert.ToInt32(MyShoppingcart[i]), Convert.ToInt32(repeaterItem.SupplierID), objOrder.OrderFor);
                    if (obj1.Count > 0)
                    {
                        objRecord.AddRange(obj1.ToList());
                    }
                }
            }

            if (objRecord.Count > 0)
            {
                gvMainOrderDetail.DataSource = objRecord;
                gvMainOrderDetail.DataBind();

                if (MOASTotalAmountForCAIE > 0)
                {
                    Decimal SalesTaxValue = Decimal.Round((MOASTotalAmountForCAIE + Convert.ToDecimal(lblShipping.Text)) * Convert.ToDecimal(lblStrikeIronSalesTax.Text), 2);
                    lblSalesTax.Text = SalesTaxValue.ToString("f2");
                    lblOrderTotal.Text = Convert.ToString(Convert.ToDecimal(MOASTotalAmountForCAIE) + SalesTaxValue + Convert.ToDecimal(lblShipping.Text));
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Clears the billing details control of billing address popup.
    /// </summary>
    private void clearBillingDetails()
    {
        txtZipcodeEdit.Text = String.Empty;
        txtBillingFnameEdit.Text = String.Empty;
        txtBillingLnameEdit.Text = String.Empty;
        txtBillingCompanyNameEdit.Text = String.Empty;
        txtBillingAddressEdit.Text = String.Empty;
        txtEmailAdrressEdit.Text = String.Empty;
        txtPhoneNumberEdit.Text = String.Empty;
        drpCountry.SelectedIndex = 0;
        drpState.SelectedIndex = 0;
        drpCity.SelectedIndex = 0;
    }

    private String NameBars()
    {
        String strNameBars = String.Empty;
        Order objOrder = new OrderConfirmationRepository().GetByOrderID(this.OrderID);
        if (objOrder.OrderFor == "IssuanceCart")
        {
            List<MyIssuanceCart> objIssList = new MyIssuanceCartRepository().GetIssuanceCartByOrderID(objOrder.OrderID);
            foreach (MyIssuanceCart objItem in objIssList)
            {
                if (objItem.NameToBeEngraved != null && objItem.NameToBeEngraved != String.Empty)
                {
                    if (objItem.NameToBeEngraved.Contains(','))
                    {
                        String[] EmployeeTitle = objItem.NameToBeEngraved.Split(',');
                        strNameBars += "Name to be Engraved on Name Bar:" + EmployeeTitle[0] + "\n";
                        strNameBars += "Employee Title" + EmployeeTitle[1] + "\n";
                    }
                    else
                    {
                        strNameBars += "Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n";
                    }
                }
            }
        }
        else
        {
            List<SelectMyShoppingCartProductResult> objMyShpList = new MyShoppingCartRepository().SelectCurrentOrderProducts(objOrder.OrderID);
            foreach (SelectMyShoppingCartProductResult objItem in objMyShpList)
            {
                if (objItem.NameToBeEngraved != null && objItem.NameToBeEngraved != String.Empty)
                {
                    if (objItem.NameToBeEngraved.Contains(','))
                    {
                        String[] EmployeeTitle = objItem.NameToBeEngraved.Split(',');
                        strNameBars += "Name to be Engraved on Name Bar:" + EmployeeTitle[0] + "\n";
                        strNameBars += "Employee Title:" + EmployeeTitle[1] + "\n";
                    }
                    else
                    {
                        strNameBars += "Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n";
                    }
                }
            }
        }

        return strNameBars.ToString();

    }

    /// <summary>
    /// Sends the verification email.
    /// </summary>
    /// <param name="strStatus">New order status.</param>
    /// <param name="OrderNumber">The order number.</param>
    /// /// <param name="ToAdd">Email To address.</param>
    private void sendVerificationEmail(String strStatus, Int64 OrderID, String ToAdd, String FullName)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                Order objOrder = OrderRepos.GetByOrderID(OrderID);
                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetByUserInfoId(objUserInformation.UserInfoID);

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();
                String sToadd = ToAdd;

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

                String PaymentMethod = String.Empty;
                if (objOrder.PaymentOption != null)
                    PaymentMethod = objLookupRepository.GetById((Int64)objOrder.PaymentOption).sLookupName;

                String strFirstNote = "";
                if (strStatus == "Canceled")
                {
                    strFirstNote = "Your ordered has been review by your manager and at this time has been cancelled.";
                    strFirstNote += "<br/><br/><strong>Managers Reason for Cancelling: </strong>";
                    strFirstNote += txtCancelReason.Text;
                }
                else if (strStatus == "Approved")
                    strFirstNote = "You order has been reviewed by your manager and is approved. This order has been successfully submitted into our order processing system.";

                messagebody.Replace("{firstnote}", strFirstNote);
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());
                //Check here for CreditUsed
                String cr;
                Boolean creditused = true;
                if (objOrder.CreditUsed != "0")
                {
                    if (String.IsNullOrEmpty(objOrder.CreditUsed))
                    {
                        creditused = false;
                    }
                    else
                    {
                        creditused = true;
                    }
                }
                else
                {
                    creditused = false;
                }

                //Both Option have used
                if (objOrder.PaymentOption != null && creditused)
                {

                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits Paid by Corporate ";
                        /*Strating Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", objOrder.CreditAmt.ToString());
                        /*End*/
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits Paid by Corporate ";
                        /*Anniversary Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                        /*End*/

                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                }
                //Only Starting or Anniversary
                else if (objOrder.PaymentOption == null && creditused)
                {
                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr);
                }
                //Only Payment Option
                else if (objOrder.PaymentOption != null && !creditused)
                {
                    messagebody.Replace("{PaymentType}", new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                }
                else
                {
                    messagebody.Replace("{PaymentType}", "Paid By Corporate");
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/

                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                }
                messagebody.Replace("{Payment Method :}", "Payment Method :");

                #region PaymentOptionCode
                //For displaying payment option code
                if (!String.IsNullOrEmpty(PaymentMethod))
                {
                    if (PaymentMethod == "Purchase Order")
                    {
                        messagebody.Replace("{PaymentOptionCodeLabel}", "Purchase Order Name :");
                        messagebody.Replace("{PaymentOptionCodeValue}", objOrder.PaymentOptionCode);
                    }
                    else if (PaymentMethod == "Cost-Center Code" || (PaymentMethod == "MOAS" && this.IsMOASWithCostCenterCode))
                    {
                        messagebody.Replace("{PaymentOptionCodeLabel}", "Cost Center Code :");
                        messagebody.Replace("{PaymentOptionCodeValue}", objOrder.PaymentOptionCode);
                    }
                    else
                    {
                        messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
                        messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);
                    }
                }
                else
                {
                    messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
                    messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);
                }
                #endregion

                #region Billing Address
                if (objOrder.OrderFor == "ShoppingCart") // Company Pays
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                else
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                messagebody.Replace("{Billing Address}", "Bill To:");
                messagebody.Replace("{NameView}", "Name :");
                messagebody.Replace("{CompanyNameView}", "Company Name :");
                messagebody.Replace("{AddressView}", "Address :");
                messagebody.Replace("{CityView}", "City :");
                messagebody.Replace("{StateView}", "State :");
                messagebody.Replace("{ZipView}", "Zip :");
                messagebody.Replace("{CountryView}", "Country :");
                messagebody.Replace("{PhoneView}", "Phone :");
                messagebody.Replace("{EmailView}", "Email :");


                messagebody.Replace("{Name}", objBillingInfo.BillingCO + " " + objBillingInfo.Manager);
                messagebody.Replace("{CompanyName}", objBillingInfo.CompanyName);
                messagebody.Replace("{Address1}", objBillingInfo.Address + "<br/>" + objBillingInfo.Address2);
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{City}", objCity.GetById((Int64)objBillingInfo.CityID).sCityName);
                messagebody.Replace("{State}", objState.GetById((Int64)objBillingInfo.StateID).sStatename);
                messagebody.Replace("{Zip}", objBillingInfo.ZipCode);
                messagebody.Replace("{Country}", objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone}", objBillingInfo.Telephone);
                messagebody.Replace("{Email}", objBillingInfo.Email);
                #endregion

                #region shipping address
                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                }
                else
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                }
                messagebody.Replace("{Shipping Address}", "Ship To:");
                messagebody.Replace("{AirportView1}", "Airport :");
                messagebody.Replace("{DepartmentView1}", "Department :");
                messagebody.Replace("{NameView1}", "Name :");
                messagebody.Replace("{CompanyNameView1}", "Company Name :");
                messagebody.Replace("{AddressView1}", "Address :");
                messagebody.Replace("{CityView1}", "City :");
                messagebody.Replace("{CountyView1}", "County :");
                messagebody.Replace("{StateView1}", "State :");
                messagebody.Replace("{ZipView1}", "Zip :");
                messagebody.Replace("{CountryView1}", "Country :");
                messagebody.Replace("{PhoneView1}", "Phone :");
                messagebody.Replace("{EmailView1}", "Email :");

                messagebody.Replace("{Airport1}", objShippingInfo.Airport);
                if (objShippingInfo.DepartmentID != null)
                    messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
                else
                    messagebody.Replace("{Department1}", "");
                messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
                messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
                messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
                messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
                messagebody.Replace("{County1}", objShippingInfo.county);
                messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
                messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
                messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
                messagebody.Replace("{Email1}", objShippingInfo.Email);

                // WL ContactID Info
                messagebody.Replace("{OrderPlacedBy}", String.Empty);
                messagebody.Replace("{WLContactID}", String.Empty);
                #endregion

                #region start
                String innermessage = "";
                Boolean GLCodeExists = false;

                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                    GLCodeExists = objSelectMyShoppingCartProductResultList.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objSelectMyShoppingCartProductResultList[i].NameToBeEngraved))
                        {
                            p = objSelectMyShoppingCartProductResultList[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Quantity;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Size;
                        innermessage = innermessage + "</td>";

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].GLCode;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice) * Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].Quantity);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }
                    messagebody.Replace("{innermessage}", innermessage);
                }
                else
                {
                    List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                    GLCodeExists = objFinal.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objFinal.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objFinal[i].NameToBeEngraved))
                        {
                            p = objFinal[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].Qty;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].sLookupName;
                        innermessage = innermessage + "</td>";

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].GLCode;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate) * Convert.ToDecimal(objFinal[i].Qty);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }

                    messagebody.Replace("{innermessage}", innermessage);

                }

                if (GLCodeExists)
                    messagebody.Replace("<td id=\"tdPD\" width=\"48%\"", "<td id=\"tdPD\" width=\"38%\"");
                else
                    messagebody.Replace(@"<td id=""tdGC"" width=""10%"" style=""font-weight: bold; text-align: center;"">
                                                    GL Code
                                                </td>", "");

                #endregion

                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "Shipping :");
                messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{OrderTotalView}", "Order Total :");
                messagebody.Replace("{innermesaageforsupplier}", "");
                String b = NameBars();
                if (b != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b);
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                }

                messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
                messagebody.Replace("{Saletax}", "$" + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)).ToString());
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + objOrder.OrderAmount).ToString());
                if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)//check that corporate discount is applied or not
                {
                    messagebody.Replace("{CorporateDiscountView}", "Corporate Discount");
                    messagebody.Replace("{CorporateDiscount}", objOrder.CorporateDiscount.ToString());
                }
                else
                {
                    messagebody.Replace("{CorporateDiscountView}", "");
                    messagebody.Replace("{CorporateDiscount}", "");
                }
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                //String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                //String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                if (new ManageEmailRepository().CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.MOAS)) == true)
                {
                    new CommonMails().SendMail(objUserInformation.UserInfoID, "MOAS From Edit Order Detail", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Sends the email to incentex admin.
    /// </summary>
    private void sendIEEmail(String strStatus, Int64 OrderID)
    {
        List<UserInformation> objAdminList = new List<UserInformation>();
        objAdminList = new UserInformationRepository().GetEmailInformation();
        if (objAdminList.Count > 0)
        {
            //* ---------- Change for optimization By gaurang 16 MAR 2013 Start-----------------*/
            Order objOrder = OrderRepos.GetByOrderID(OrderID);
            UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
            CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetByUserInfoId(objUserInformation.UserInfoID);
            //* ---------- Change for optimization By gaurang 16 MAR 2013 END-----------------*/

            for (Int32 i = 0; i < objAdminList.Count; i++)
            {
                sendVerificationEmailAdmin(strStatus, OrderID, objAdminList[i].LoginEmail, objAdminList[i].FirstName, objAdminList[i].UserInfoID, false, objUserInformation, objCompanyEmployee);
            }
        }
    }

    /// <summary>
    /// Sends the email to incentex admin.
    /// </summary>
    private void sendTestOrderEmailNotification(String strStatus, Int64 OrderID)
    {
        List<FUN_GetTestOrderEmailReceiversResult> objAdminList = new List<FUN_GetTestOrderEmailReceiversResult>();
        objAdminList = new IncentexBEDataContext().FUN_GetTestOrderEmailReceivers().ToList();
        if (objAdminList.Count > 0)
        {
            //* ---------- Change for optimization By gaurang 16 MAR 2013 Start-----------------*/
            Order objOrder = OrderRepos.GetByOrderID(OrderID);
            UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
            CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetByUserInfoId(objUserInformation.UserInfoID);
            //* ---------- Change for optimization By gaurang 16 MAR 2013 END-----------------*/

            foreach (FUN_GetTestOrderEmailReceiversResult receiver in objAdminList)
            {
                sendVerificationEmailAdmin(strStatus, OrderID, receiver.LoginEmail, receiver.FirstName, Convert.ToInt64(receiver.UserInfoID), true, objUserInformation, objCompanyEmployee);
            }
        }
    }

    /// <summary>
    /// Sends the verification email to incentex admin.
    /// </summary>
    /// <param name="strStatus">New order status.</param>
    /// <param name="OrderNumber">The order number.</param>
    /// /// <param name="ToAdd">receiver email address.</param>
    /// /// /// <param name="ToAdd">full name of receiver.</param>
    private void sendVerificationEmailAdmin(String strStatus, Int64 OrderID, String ToAdd, String FullName, Int64 ToUserInfoID, Boolean IsTestOrder, UserInformation objUserInformation, CompanyEmployee objCompanyEmployee)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                //Order objOrder = OrderRepos.GetByOrderID(OrderID);
                //UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                //CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetByUserInfoId(objUserInformation.UserInfoID);

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();
                String sToadd = ToAdd;

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

                String PaymentMethod = String.Empty;
                if (objOrder.PaymentOption != null)
                    PaymentMethod = objLookupRepository.GetById((Int64)objOrder.PaymentOption).sLookupName;

                messagebody.Replace("{firstnote}", "Thank you for your business you can check the status of order(s) any time by logging into your store <a href='http://www.world-link.us.com' title='Incentex'>here.</a>");
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());

                // Set SAP Company Contact ID and Full name
                String OrderPlacedBy = String.Empty;
                String WLContactID = String.Empty;
                GetSAPCompanyCodeIDResult objSAPCompanyResult = objUsrInfoRepo.GetSAPCompanyCodeID(objUserInformation.UserInfoID);
                if (objSAPCompanyResult != null)
                {
                    OrderPlacedBy = String.Format("Order Placed By: {0}", objSAPCompanyResult.FullName);
                    WLContactID = String.Format("World-Link Contact ID : {0}", objSAPCompanyResult.SAPCompanyCodeID);
                }
                //Check here for CreditUsed
                String cr;
                Boolean creditused = true;
                if (objOrder.CreditUsed != "0")
                {
                    if (String.IsNullOrEmpty(objOrder.CreditUsed))
                    {
                        creditused = false;
                    }
                    else
                    {
                        creditused = true;
                    }
                }
                else
                {
                    creditused = false;
                }

                //Both Option have used
                if (objOrder.PaymentOption != null && creditused)
                {

                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits Paid by Corporate ";
                        /*Strating Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", objOrder.CreditAmt.ToString());
                        /*End*/
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits Paid by Corporate ";
                        /*Anniversary Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                        /*End*/

                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                }
                //Only Starting or Anniversary
                else if (objOrder.PaymentOption == null && creditused)
                {
                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr);
                }
                //Only Payment Option
                else if (objOrder.PaymentOption != null && !creditused)
                {
                    messagebody.Replace("{PaymentType}", new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                }
                else
                {
                    messagebody.Replace("{PaymentType}", "Paid By Corporate");
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/

                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                }
                messagebody.Replace("{Payment Method :}", "Payment Method :");

                #region PaymentOptionCode
                //For displaying payment option code
                if (!String.IsNullOrEmpty(PaymentMethod))
                {
                    if (PaymentMethod == "Purchase Order")
                    {
                        messagebody.Replace("{PaymentOptionCodeLabel}", "Purchase Order Name :");
                        messagebody.Replace("{PaymentOptionCodeValue}", objOrder.PaymentOptionCode);
                    }
                    else if (PaymentMethod == "Cost-Center Code" || (PaymentMethod == "MOAS" && this.IsMOASWithCostCenterCode))
                    {
                        messagebody.Replace("{PaymentOptionCodeLabel}", "Cost Center Code :");
                        messagebody.Replace("{PaymentOptionCodeValue}", objOrder.PaymentOptionCode);
                    }
                    else
                    {
                        messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
                        messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);
                    }
                }
                else
                {
                    messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
                    messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);
                }
                #endregion

                #region Billing Address
                if (objOrder.OrderFor == "ShoppingCart") // Company Pays
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                else
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                messagebody.Replace("{Billing Address}", "Bill To:");
                messagebody.Replace("{NameView}", "Name :");
                messagebody.Replace("{CompanyNameView}", "Company Name :");
                messagebody.Replace("{AddressView}", "Address :");
                messagebody.Replace("{CityView}", "City :");
                messagebody.Replace("{StateView}", "State :");
                messagebody.Replace("{ZipView}", "Zip :");
                messagebody.Replace("{CountryView}", "Country :");
                messagebody.Replace("{PhoneView}", "Phone :");
                messagebody.Replace("{EmailView}", "Email :");


                messagebody.Replace("{Name}", objBillingInfo.BillingCO + " " + objBillingInfo.Manager);
                messagebody.Replace("{CompanyName}", objBillingInfo.CompanyName);
                messagebody.Replace("{Address1}", objBillingInfo.Address + "<br/>" + objBillingInfo.Address2);
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{City}", objCity.GetById((Int64)objBillingInfo.CityID).sCityName);
                messagebody.Replace("{State}", objState.GetById((Int64)objBillingInfo.StateID).sStatename);
                messagebody.Replace("{Zip}", objBillingInfo.ZipCode);
                messagebody.Replace("{Country}", objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone}", objBillingInfo.Telephone);
                messagebody.Replace("{Email}", objBillingInfo.Email);
                #endregion

                #region shipping address
                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                }
                else
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                }
                messagebody.Replace("{Shipping Address}", "Ship To:");
                messagebody.Replace("{AirportView1}", "Airport :");
                messagebody.Replace("{DepartmentView1}", "Department :");
                messagebody.Replace("{NameView1}", "Name :");
                messagebody.Replace("{CompanyNameView1}", "Company Name :");
                messagebody.Replace("{AddressView1}", "Address :");
                messagebody.Replace("{CityView1}", "City :");
                messagebody.Replace("{CountyView1}", "County :");
                messagebody.Replace("{StateView1}", "State :");
                messagebody.Replace("{ZipView1}", "Zip :");
                messagebody.Replace("{CountryView1}", "Country :");
                messagebody.Replace("{PhoneView1}", "Phone :");
                messagebody.Replace("{EmailView1}", "Email :");

                messagebody.Replace("{Airport1}", objShippingInfo.Airport);
                if (objShippingInfo.DepartmentID != null)
                    messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
                else
                    messagebody.Replace("{Department1}", "");
                messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
                messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
                messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
                messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
                messagebody.Replace("{County1}", objShippingInfo.county);
                messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
                messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
                messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
                messagebody.Replace("{Email1}", objShippingInfo.Email);

                // WL ContactID Info
                messagebody.Replace("{OrderPlacedBy}", OrderPlacedBy);
                messagebody.Replace("{WLContactID}", WLContactID);
                #endregion

                #region start
                String innermessage = "";
                Boolean GLCodeExists = false;

                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                    GLCodeExists = objSelectMyShoppingCartProductResultList.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objSelectMyShoppingCartProductResultList[i].NameToBeEngraved))
                        {
                            p = objSelectMyShoppingCartProductResultList[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Quantity;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Size;
                        innermessage = innermessage + "</td>";

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].GLCode;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)).ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)) * Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].Quantity);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }
                    messagebody.Replace("{innermessage}", innermessage);
                }
                else
                {
                    List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                    GLCodeExists = objFinal.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objFinal.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objFinal[i].NameToBeEngraved))
                        {
                            p = objFinal[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].Qty;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].sLookupName;
                        innermessage = innermessage + "</td>";

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].GLCode;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)).ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)) * Convert.ToDecimal(objFinal[i].Qty);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }

                    messagebody.Replace("{innermessage}", innermessage);

                }

                if (GLCodeExists)
                    messagebody.Replace("<td id=\"tdPD\" width=\"48%\"", "<td id=\"tdPD\" width=\"38%\"");
                else
                    messagebody.Replace(@"<td id=""tdGC"" width=""10%"" style=""font-weight: bold; text-align: center;"">
                                                    GL Code
                                                </td>", "");
                #endregion

                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "Shipping :");
                messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{OrderTotalView}", "Order Total :");
                messagebody.Replace("{innermesaageforsupplier}", "");
                String b = NameBars();
                if (b != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b + (IsTestOrder ? @"<br/><span style=""color:red;font-weight:bold;"">THIS IS A TEST ORDER. PLEASE DO NOT PROCESS.</span>" : ""));
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + (IsTestOrder ? @"<br/><span style=""color:red;font-weight:bold;"">THIS IS A TEST ORDER. PLEASE DO NOT PROCESS.</span>" : ""));
                }

                messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
                messagebody.Replace("{Saletax}", "$" + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)).ToString());
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + (objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount))).ToString());
                if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)//check that corporate discount is applied or not
                {
                    messagebody.Replace("{CorporateDiscountView}", "Corporate Discount");
                    messagebody.Replace("{CorporateDiscount}", objOrder.CorporateDiscount.ToString());
                }
                else
                {
                    messagebody.Replace("{CorporateDiscountView}", "");
                    messagebody.Replace("{CorporateDiscount}", "");
                }
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                //String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                //String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                if (new ManageEmailRepository().CheckEmailAuthentication(ToUserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.MOAS)) == true)
                {
                    new CommonMails().SendMail(ToUserInfoID, "MOAS From Edit Order Detail", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Sends the email to supplier.
    /// </summary>
    /// <param name="ordernumber">The ordernumber.</param>
    private void sendEmailToSupplier(Int64 orderid, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
    {
        try
        {
            Order objOrder = OrderRepos.GetByOrderID(orderid);
            if (objOrder != null)
            {
                List<SelectSupplierAddressResult> obj = OrderRepos.GetSupplierAddress(objOrder.OrderFor, objOrder.OrderID);

                //* ---------- Change for optimization By gaurang 16 MAR 2013 Start-----------------*/
                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                //* ---------- Change for optimization By gaurang 16 MAR 2013 END-----------------*/


                foreach (SelectSupplierAddressResult repeaterItem in obj)
                {
                    sendVerificationEmailSupplier(orderid, objOrder.MyShoppingCartID, repeaterItem.SupplierID, repeaterItem.Name, objShippingInfo, repeaterItem.CompanyName, objUserInformation);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Sends the verification email supplier.
    /// </summary>
    /// <param name="OrderNumber">The order number.</param>
    /// <param name="ShoppingCartID">The shopping cart ID.</param>
    /// <param name="supplierId">The supplier id.</param>
    /// <param name="fullName">The full name.</param>
    private void sendVerificationEmailSupplier(Int64 OrderId, String ShoppingCartID, Int64 supplierId, String fullName, CompanyEmployeeContactInfo objShippingInfo, String SupplierCompanyName, UserInformation objUserInformation)
    {
        try
        {
            //Get supplierinfo by id
            UserInformation objUserInfo = objUsrInfoRepo.GetById(new SupplierRepository().GetById(supplierId).UserInfoID);

            if (objUserInfo != null)
            {
                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(objUserInfo.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderConfirmations)) == true)
                {

                    CityRepository objCity = new CityRepository();
                    StateRepository objState = new StateRepository();
                    CountryRepository objCountry = new CountryRepository();
                    EmailTemplateBE objEmailBE = new EmailTemplateBE();
                    EmailTemplateDA objEmailDA = new EmailTemplateDA();
                    DataSet dsEmailTemplate;

                    //Get Email Content
                    objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
                    objEmailBE.STemplateName = "Order Placed Supplier";
                    dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
                    if (dsEmailTemplate != null)
                    {
                        //Find UserName who had order purchased
                        //Order objOrder = OrderRepos.GetByOrderID(OrderId);
                        //UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);

                        String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                        String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                        String sSubject = objCmpRepo.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber;
                        String sToadd = objUserInfo.LoginEmail;

                        StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                        StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());
                        messagebody.Replace("{firstnote}", "Please review the following order below if you have any questions please post them to the notes section of this order located within the order in the system.");
                        messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                        messagebody.Replace("{referencename}", objOrder.ReferenceName + "<br/>Workgroup : " + objLookupRepository.GetById((Int64)(objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID).WorkgroupID)).sLookupName); // change by mayur on 26-March-2012
                        messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                        //messagebody.Replace("{ReferenceName}", lblPurchasedBy.Text);
                        messagebody.Replace("{FullName}", fullName);
                        messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());

                        String PaymentMethod = String.Empty;
                        if (objOrder.PaymentOption != null)
                            PaymentMethod = objLookupRepository.GetById((Int64)objOrder.PaymentOption).sLookupName;

                        //Added on 20 Sep 2011
                        //Check here for CreditUsed
                        String cr;
                        Boolean creditused = true;
                        if (objOrder.CreditUsed != "0")
                        {
                            if (String.IsNullOrEmpty(objOrder.CreditUsed))
                            {
                                creditused = false;
                            }
                            else
                            {
                                creditused = true;
                            }
                        }
                        else
                        {
                            creditused = false;
                        }

                        //Both Option have used
                        if (objOrder.PaymentOption != null && creditused)
                        {

                            if (objOrder.CreditUsed == "Previous")
                            {
                                cr = "Starting " + "Credits Paid by Corporate ";
                            }
                            else
                            {
                                cr = "Anniversary " + "Credits Paid by Corporate ";
                            }
                            messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + PaymentMethod);
                        }
                        //Only Starting or Anniversary
                        else if (objOrder.PaymentOption == null && creditused)
                        {
                            if (objOrder.CreditUsed == "Previous")
                            {
                                cr = "Starting " + "Credits - Paid by Corporate ";
                            }
                            else
                            {
                                cr = "Anniversary " + "Credits - Paid by Corporate ";
                            }
                            messagebody.Replace("{PaymentType}", cr);
                        }
                        //Only Payment Option
                        else if (objOrder.PaymentOption != null && !creditused)
                        {
                            messagebody.Replace("{PaymentType}", PaymentMethod);
                        }
                        else
                        {
                            messagebody.Replace("{PaymentType}", "Paid By Corporate");
                        }
                        messagebody.Replace("{Payment Method :}", "Payment Method :");

                        //For displaying payment option code
                        messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
                        messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);

                        //End
                        //messagebody.Replace("{CreditType}", "---");

                        #region shipping address
                        messagebody.Replace("{Shipping Address}", "Ship To:");
                        messagebody.Replace("{AirportView1}", "Airport :");
                        messagebody.Replace("{DepartmentView1}", "Department :");
                        messagebody.Replace("{NameView1}", "Name :");
                        messagebody.Replace("{CompanyNameView1}", "Company Name :");
                        messagebody.Replace("{AddressView1}", "Address :");
                        // messagebody.Replace("{Address2}", lblBadr.Text);
                        messagebody.Replace("{CityView1}", "City :");
                        messagebody.Replace("{CountyView1}", "County :");
                        messagebody.Replace("{StateView1}", "State :");
                        messagebody.Replace("{ZipView1}", "Zip :");
                        messagebody.Replace("{CountryView1}", "Country :");
                        messagebody.Replace("{PhoneView1}", "Phone :");
                        messagebody.Replace("{EmailView1}", "Email :");

                        messagebody.Replace("{Airport1}", objShippingInfo.Airport);
                        if (objShippingInfo.DepartmentID != null)
                            messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
                        else
                            messagebody.Replace("{Department1}", "");
                        messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
                        messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
                        messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
                        //messagebody.Replace("{Address22}", lbl.Text);
                        messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
                        messagebody.Replace("{County1}", objShippingInfo.county);
                        messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
                        messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
                        messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
                        messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
                        messagebody.Replace("{Email1}", objShippingInfo.Email);
                        #endregion

                        String innermessageSupplier = "";
                        Boolean GLCodeExists = false;

                        #region Supplier

                        List<SelectOrderDetailsForSupplierResult> lstSupplierItemsFromOrder = OrderRepos.GetOrderItemsForSupplier(supplierId, objOrder.OrderID);

                        if (lstSupplierItemsFromOrder.Count > 0)
                        {
                            GLCodeExists = lstSupplierItemsFromOrder.FirstOrDefault(le => le.GLCode != null && le.GLCode != "") != null;
                            Int16 ColSpan = 9;

                            if (GLCodeExists)
                                ColSpan = 10;

                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                            innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td align='center' colspan='2'  style='background-color:Gray;font-weight:bold;color:Black;'>";
                            innermessageSupplier = innermessageSupplier + SupplierCompanyName;
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";
                            innermessageSupplier = innermessageSupplier + "</table>";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";
                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                            innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: left;'>";
                            innermessageSupplier = innermessageSupplier + "Ordered";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Item#";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Size";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Color";
                            innermessageSupplier = innermessageSupplier + "</td>";

                            if (!GLCodeExists)
                            {
                                innermessageSupplier = innermessageSupplier + "<td width='35%' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "Description";
                                innermessageSupplier = innermessageSupplier + "</td>";
                            }
                            else
                            {
                                innermessageSupplier = innermessageSupplier + "<td width='25%' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "Description";
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "GL Code";
                                innermessageSupplier = innermessageSupplier + "</td>";
                            }

                            //Add Nagmani 18-Jan-2012
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Unit Price";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Extended Price";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            //End

                            innermessageSupplier = innermessageSupplier + "</table>";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";
                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                            innermessageSupplier = innermessageSupplier + "<hr />";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";

                            foreach (SelectOrderDetailsForSupplierResult item in lstSupplierItemsFromOrder)
                            {
                                innermessageSupplier = innermessageSupplier + "<tr>";
                                innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                                innermessageSupplier = innermessageSupplier + "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                                innermessageSupplier = innermessageSupplier + "<tr>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: left;'>";
                                innermessageSupplier = innermessageSupplier + item.Quantity;
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='15%' height='50' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + item.ItemNumber;
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + item.Size;
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + item.Color;
                                innermessageSupplier = innermessageSupplier + "</td>";

                                if (!GLCodeExists)
                                {
                                    innermessageSupplier = innermessageSupplier + "<td width='35%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + item.ProductDescrption;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                }
                                else
                                {
                                    innermessageSupplier = innermessageSupplier + "<td width='25%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + item.ProductDescrption;
                                    innermessageSupplier = innermessageSupplier + "</td>";

                                    innermessageSupplier = innermessageSupplier + "<td width='10%' height='50' style='font-weight: bold; text-align: center;'>";
                                    innermessageSupplier = innermessageSupplier + item.GLCode;
                                    innermessageSupplier = innermessageSupplier + "</td>";
                                }

                                innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "$" + (item.MOASPrice != null ? Convert.ToDecimal(item.MOASPrice) : Convert.ToDecimal(item.Price)).ToString();
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "$" + (item.MOASPrice != null ? Convert.ToDecimal(item.MOASPrice) : Convert.ToDecimal(item.Price)) * Convert.ToDecimal(item.Quantity);
                                innermessageSupplier = innermessageSupplier + "</td>";


                                innermessageSupplier = innermessageSupplier + "</tr>";
                                innermessageSupplier = innermessageSupplier + "</table>";
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "</tr>";
                            }

                            innermessageSupplier = innermessageSupplier + "<tr>";
                            innermessageSupplier = innermessageSupplier + "<td colspan='" + ColSpan + "'>";
                            innermessageSupplier = innermessageSupplier + "<hr />";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "</tr>";
                        }

                        messagebody.Replace("{innermesaageforsupplier}", innermessageSupplier);

                        //Update Nagmani 18-Jan-2012
                        messagebody.Replace("{ShippingCost}", "");
                        messagebody.Replace("{Saletax}", "");
                        // messagebody.Replace("{OrderTotal}", "");

                        //messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
                        messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal((objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + (objOrder.MOASOrderAmount != null ? objOrder.MOASOrderAmount : objOrder.OrderAmount)).ToString());

                        //End Nagmani
                        messagebody.Replace(" {Order Notes:}", "Order Notes :");
                        messagebody.Replace("{ShippingCostView}", "");
                        messagebody.Replace("{SalesTaxView}", "");
                        //Update Nagmani 18-Jan-2012
                        //  messagebody.Replace("{OrderTotalView}", "");
                        messagebody.Replace("{OrderTotalView}", "Order Total :");
                        //End Nagmani 

                        //End
                        #endregion

                        String c = NameBars();
                        if (c != null)
                        {
                            messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + c);
                        }
                        else
                        {
                            messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                        }

                        //  messagebody.Replace("{PurchasesNotEligableforCreditUse}", lblNotEligibleforAnniversarycredit.Text);


                        messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                        String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                        Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                        //String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                        //String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                        new CommonMails().SendMail(objUserInfo.UserInfoID, "MOAS From Edit Order Detail", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Sends the verification email MOAS manager for approve order.
    /// Add By Mayur on 6-feb-2012
    /// </summary>
    /// <param name="OrderNumber">The order number.</param>
    /// <param name="MOASEmailAddress">The MOAS email address.</param>
    /// <param name="FullName">The full name.</param>
    private void sendVerificationEmailMOASManager(Int64 OrderId, String MOASEmailAddress, String FullName, Int64 MOASUserId)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed MOAS";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                Order objOrder = OrderRepos.GetByOrderID(OrderId);
                UserInformation objUserInformation = objUsrInfoRepo.GetById(objOrder.UserId);
                CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetByUserInfoId(objUserInformation.UserInfoID);

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString().Replace("{FullName}", objUserInformation.FirstName + " " + objUserInformation.LastName).Replace("{OrderNumber}", objOrder.OrderNumber);
                String sToadd = MOASEmailAddress;

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

                messagebody.Replace("{firstnote}", "Please review the following order below that requires your attention. You can Approve, Edit or Cancel this order by clicking the buttons on the bottom of this page.");
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName);
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());

                #region Check here for CreditUsed
                String cr;
                Boolean creditused = true;
                if (objOrder.CreditUsed != "0")
                {
                    if (String.IsNullOrEmpty(objOrder.CreditUsed))
                    {
                        creditused = false;
                    }
                    else
                    {
                        creditused = true;
                    }
                }
                else
                {
                    creditused = false;
                }
                #endregion

                #region PaymentType and PaymentMethod
                //Both Option have used
                if (objOrder.PaymentOption != null && creditused)
                {

                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits Paid by Corporate ";
                        /*Strating Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", objOrder.CreditAmt.ToString());
                        /*End*/
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits Paid by Corporate ";
                        /*Anniversary Credit Row*/
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits :");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                        /*End*/

                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                }
                //Only Starting or Anniversary
                else if (objOrder.PaymentOption == null && creditused)
                {
                    if (objOrder.CreditUsed == "Previous")
                    {
                        cr = "Starting " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Starting Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    else
                    {
                        cr = "Anniversary " + "Credits - Paid by Corporate ";
                        messagebody.Replace("{lblStrCrAmntAppliedLabel}", "Applied to Anniversary Credits:");
                        messagebody.Replace("{lblStCrAmntApplied}", "$" + objOrder.CreditAmt.ToString());
                    }
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr);
                }
                //Only Payment Option
                else if (objOrder.PaymentOption != null && !creditused)
                {
                    messagebody.Replace("{PaymentType}", new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName);
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + (((objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount)) + Convert.ToDecimal(objOrder.ShippingAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax))) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                }
                else
                {
                    messagebody.Replace("{PaymentType}", "Paid By Corporate");
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/

                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", String.Empty);
                    messagebody.Replace("{lblPaymentMethodAmount}", String.Empty);
                    /*End*/
                }
                messagebody.Replace("{Payment Method :}", "Payment Method :");
                #endregion

                #region PaymentOptionCode
                //For displaying payment option code
                if (this.IsMOASWithCostCenterCode)
                {
                    messagebody.Replace("{PaymentOptionCodeLabel}", "Cost Center Code :");
                    messagebody.Replace("{PaymentOptionCodeValue}", objOrder.PaymentOptionCode);
                }
                else
                {
                    messagebody.Replace("{PaymentOptionCodeLabel}", String.Empty);
                    messagebody.Replace("{PaymentOptionCodeValue}", String.Empty);
                }
                #endregion

                #region Billing Address
                if (objOrder.OrderFor == "ShoppingCart") // Company Pays
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                else
                {
                    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                messagebody.Replace("{Billing Address}", "Bill To:");
                messagebody.Replace("{NameView}", "Name :");
                messagebody.Replace("{CompanyNameView}", "Company Name :");
                messagebody.Replace("{AddressView}", "Address :");
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{CityView}", "City :");
                messagebody.Replace("{StateView}", "State :");
                messagebody.Replace("{ZipView}", "Zip :");
                messagebody.Replace("{CountryView}", "Country :");
                messagebody.Replace("{PhoneView}", "Phone :");
                messagebody.Replace("{EmailView}", "Email :");

                messagebody.Replace("{Name}", objBillingInfo.BillingCO + " " + objBillingInfo.Manager);
                messagebody.Replace("{CompanyName}", objBillingInfo.CompanyName);
                messagebody.Replace("{Address1}", objBillingInfo.Address + "<br/>" + objBillingInfo.Address2);
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{City}", objCity.GetById((Int64)objBillingInfo.CityID).sCityName);
                messagebody.Replace("{State}", objState.GetById((Int64)objBillingInfo.StateID).sStatename);
                messagebody.Replace("{Zip}", objBillingInfo.ZipCode);
                messagebody.Replace("{Country}", objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone}", objBillingInfo.Telephone);
                messagebody.Replace("{Email}", objBillingInfo.Email);
                #endregion

                #region shipping address
                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                }
                else
                {
                    objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                }
                messagebody.Replace("{Shipping Address}", "Ship To:");
                messagebody.Replace("{AirportView1}", "Airport :");
                messagebody.Replace("{DepartmentView1}", "Department :");
                messagebody.Replace("{NameView1}", "Name :");
                messagebody.Replace("{CompanyNameView1}", "Company Name :");
                messagebody.Replace("{AddressView1}", "Address :");
                // messagebody.Replace("{Address2}", lblBadr.Text);
                messagebody.Replace("{CityView1}", "City :");
                messagebody.Replace("{CountyView1}", "County :");
                messagebody.Replace("{StateView1}", "State :");
                messagebody.Replace("{ZipView1}", "Zip :");
                messagebody.Replace("{CountryView1}", "Country :");
                messagebody.Replace("{PhoneView1}", "Phone :");
                messagebody.Replace("{EmailView1}", "Email :");

                messagebody.Replace("{Airport1}", objShippingInfo.Airport);
                if (objShippingInfo.DepartmentID != null)
                    messagebody.Replace("{Department1}", new LookupRepository().GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
                else
                    messagebody.Replace("{Department1}", "");
                messagebody.Replace("{Name1}", objShippingInfo.Name + " " + objShippingInfo.Fax);
                messagebody.Replace("{CompanyName1}", objShippingInfo.CompanyName);
                messagebody.Replace("{Address11}", objShippingInfo.Address + "<br/>" + objShippingInfo.Address2 + "<br/>" + objShippingInfo.Street);
                messagebody.Replace("{City1}", objCity.GetById((Int64)objShippingInfo.CityID).sCityName);
                messagebody.Replace("{County1}", objShippingInfo.county);
                messagebody.Replace("{State1}", objState.GetById((Int64)objShippingInfo.StateID).sStatename);
                messagebody.Replace("{Zip1}", objShippingInfo.ZipCode);
                messagebody.Replace("{Country1}", objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName);
                messagebody.Replace("{Phone1}", objShippingInfo.Telephone);
                messagebody.Replace("{Email1}", objShippingInfo.Email);
                #endregion

                #region order product detail
                String innermessage = "";
                Boolean GLCodeExists = false;

                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                    GLCodeExists = objSelectMyShoppingCartProductResultList.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objSelectMyShoppingCartProductResultList[i].NameToBeEngraved))
                        {
                            p = objSelectMyShoppingCartProductResultList[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Quantity;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Size;
                        innermessage = innermessage + "</td>";

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].ProductDescrption1;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].GLCode;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)).ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objSelectMyShoppingCartProductResultList[i].MOASUnitPrice != null ? Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].MOASUnitPrice) : Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].UnitPrice)) * Convert.ToDecimal(objSelectMyShoppingCartProductResultList[i].Quantity);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }
                    messagebody.Replace("{innermessage}", innermessage);
                }
                else
                {
                    List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                    GLCodeExists = objFinal.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objFinal.Count; i++)
                    {
                        String[] p = new String[1];
                        if (!String.IsNullOrEmpty(objFinal[i].NameToBeEngraved))
                        {
                            p = objFinal[i].NameToBeEngraved.Split(',');
                        }
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].Qty;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].item;
                        innermessage = innermessage + "</td>";

                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].sLookupName;
                        innermessage = innermessage + "</td>";

                        if (!GLCodeExists)
                        {
                            innermessage = innermessage + "<td width='48%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                        }
                        else
                        {
                            innermessage = innermessage + "<td width='38%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                            innermessage = innermessage + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                            innermessage = innermessage + objFinal[i].ProductDescrption;
                            innermessage = innermessage + "</td>";
                        }

                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)).ToString();
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        innermessage = innermessage + "$" + (objFinal[i].MOASRate != null ? Convert.ToDecimal(objFinal[i].MOASRate) : Convert.ToDecimal(objFinal[i].Rate)) * Convert.ToDecimal(objFinal[i].Qty);
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";
                        innermessage = innermessage + "<tr>";

                        if (GLCodeExists)
                            innermessage = innermessage + "<td colspan='8'>";
                        else
                            innermessage = innermessage + "<td colspan='7'>";

                        innermessage = innermessage + "<hr />";
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "</tr>";

                    }

                    messagebody.Replace("{innermessage}", innermessage);

                }

                if (GLCodeExists)
                    messagebody.Replace("<td id=\"tdPD\" width=\"48%\"", "<td id=\"tdPD\" width=\"38%\"");
                else
                    messagebody.Replace(@"<td id=""tdGC"" width=""10%"" style=""font-weight: bold; text-align: center;"">
                                                    GL Code
                                                </td>", "");

                #endregion

                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "Shipping :");
                messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{OrderTotalView}", "Order Total :");
                messagebody.Replace("{innermesaageforsupplier}", "");
                String b = NameBars();
                if (b != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b);
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                }

                messagebody.Replace("{ShippingCost}", objOrder.ShippingAmount.ToString());
                messagebody.Replace("{Saletax}", "$" + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)).ToString());
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + (objOrder.MOASOrderAmount != null && objOrder.MOASOrderAmount != 0 ? Convert.ToDecimal(objOrder.MOASOrderAmount) : Convert.ToDecimal(objOrder.OrderAmount))).ToString());

                if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)//check that corporate discount is applied or not
                {
                    messagebody.Replace("{CorporateDiscountView}", "Corporate Discount");
                    messagebody.Replace("{CorporateDiscount}", objOrder.CorporateDiscount.ToString());
                }
                else
                {
                    messagebody.Replace("{CorporateDiscountView}", "");
                    messagebody.Replace("{CorporateDiscount}", "");
                }
                //messagebody.Replace("{PurchasesNotEligableforCreditUse}", lblNotEligibleforAnniversarycredit.Text);


                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                //Set Conformation Button
                String buttonText = "";
                buttonText += "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
                buttonText += "<tr>";
                buttonText += "<td>";
                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MOASOrderStatus.aspx?UserId=" + MOASUserId + "&OrderId=" + objOrder.OrderID + "&Status=Approve'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/approve_order_btn.png' alt='Approve Order' border='0'/></a>";
                buttonText += "<td>";
                buttonText += "<td>";
                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MOASOrderStatus.aspx?UserId=" + MOASUserId + "&OrderId=" + objOrder.OrderID + "&Status=Cancel'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/cancel_order_btn.png' alt='Cancel Order' border='0'/></a>";
                buttonText += "<td>";
                buttonText += "<td>";
                buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "MyAccount/OrderManagement/EditOrderDetail.aspx?Id=" + objOrder.OrderID + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/edit_order_btn.png' alt='Edit Order' border='0'/></a>";
                buttonText += "<td>";
                buttonText += "</tr>";
                buttonText += "</table>";

                messagebody.Replace("{ConformationButton}", buttonText);

                String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                //String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                //String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                if (new ManageEmailRepository().CheckEmailAuthentication(MOASUserId, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.MOAS)) == true)
                {
                    new CommonMails().SendMail(MOASUserId, "MOAS From Edit Order Detail", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Add the note history of order.
    /// </summary>
    /// <param name="strStatus">The order status.</param>
    /// <param name="orderID">The order ID.</param>
    protected void AddNoteHistory(String strStatus, Int64 orderID)
    {
        String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CACE);

        NoteDetail objComNot = new NoteDetail();
        NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();

        String strNoteContents = "";
        strNoteContents = "Action Taken: " + strStatus;

        if (strStatus == "Canceled")
        {
            strNoteContents += Environment.NewLine;
            strNoteContents += "Reason for Cancelling: " + txtCancelReason.Text;
        }

        objComNot.Notecontents = strNoteContents;
        objComNot.NoteFor = strNoteFor;
        objComNot.ForeignKey = orderID;
        objComNot.CreateDate = System.DateTime.Now;
        objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objComNot.UpdateDate = System.DateTime.Now;
        objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

        objCompNoteHistRepos.Insert(objComNot);
        objCompNoteHistRepos.SubmitChanges();
    }

    #region Integration with Magaya

    private void SendDataToMagaya(Order objOrder)
    {
        api_session_error Result;
        List<MyIssuanceCart> objMyIssCart = new List<MyIssuanceCart>();
        String user = ConfigurationSettings.AppSettings["MagayaUser"];
        String pass = ConfigurationSettings.AppSettings["MagayaPassword"];
        Int32 AccKey;
        Int32 flag = 0x20;
        String Err_Desc;
        try
        {
            #region Check that supplier for WTDC only
            //Order objOrder1 = new Order();
            Int32 fispresnt = 0;
            if (objOrder.OrderFor == "ShoppingCart")
            {
                objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                {
                    if (objSelectMyShoppingCartProductResultList[i].SupplierID == this.WTDCSupplierID)
                    {
                        fispresnt = 1;
                        break;
                    }
                }
            }
            else
            {
                List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                for (Int32 i = 0; i < objFinal.Count; i++)
                {
                    if (objFinal[i].SupplierID == this.WTDCSupplierID)
                    {
                        fispresnt = 1;
                        break;
                    }
                }
            }
            #endregion

            #region If supplier is for WTDC then only this will process
            if (fispresnt == 1)
            {
                CSSoapService cSSoapService = new CSSoapService();
                String xmlTrans = beautify(createXmlDoc(objOrder.OrderID.ToString(), this.WTDCSupplierID).OuterXml);
                //API Functions
                Result = cSSoapService.StartSession(user, pass, out AccKey);
                //Result = cSSoapService.SetTransaction(AccKey, TransType, flag, xmlTrans, out Err_Desc);
                String szNumber = "";
                Result = cSSoapService.SubmitSalesOrder(AccKey, xmlTrans, flag, out szNumber, out Err_Desc);

                OrderConfirmationRepository objOrderConfirmationRepositoryMagaya = new OrderConfirmationRepository();
                Order ObjOrderMagaya = objOrderConfirmationRepositoryMagaya.GetByOrderID(objOrder.OrderID);
                if (Result == api_session_error.no_error)
                    ObjOrderMagaya.SendToMagaya = "Success";
                else
                    ObjOrderMagaya.SendToMagaya = "Fail";
                objOrderConfirmationRepositoryMagaya.SubmitChanges();

                Result = cSSoapService.EndSession(AccKey);
            }
            #endregion
        }
        catch (Exception ex)
        {
            if (ex.Message == "Unable to connect to the remote server")
            {
                OrderConfirmationRepository objOrderConfirmationRepositoryMagaya = new OrderConfirmationRepository();
                Order ObjOrderMagaya = objOrderConfirmationRepositoryMagaya.GetByOrderID(objOrder.OrderID);
                ObjOrderMagaya.SendToMagaya = "Fail";
                objOrderConfirmationRepositoryMagaya.SubmitChanges();
            }
        }
    }

    /// <summary>
    /// - Indent a XML 
    /// </summary>
    /// <param name="xml">Source XML String</param>
    /// <returns>Indented XML String</returns>
    public String beautify(String xml)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        MemoryStream MS = new MemoryStream();
        XmlTextWriter xmlWriter = new XmlTextWriter(MS, null);
        xmlWriter.Formatting = Formatting.Indented;
        xmlWriter.Indentation = 4;

        xmlDoc.Save(xmlWriter);

        return ByteArrayToStr(MS.ToArray());
    }

    /// <summary>
    ///  - Encoding  Byte's Array into String with UTF8 code
    /// </summary>
    /// <param name="bytes">Source Byte Array</param>
    /// <returns>Encoded String</returns>
    public String ByteArrayToStr(Byte[] bytes)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        return encoding.GetString(bytes);
    }

    /// <summary>
    /// - Create XML to send a Transaction
    /// </summary>
    /// <returns>DocXML with WareHouse</returns>
    public XmlDocument createXmlDoc(String OrderID, Int32 supplierid)
    {
        try
        {
            //Find UserName who had order purchased
            String strUserName = null;
            List<MyIssuanceCart> objMyIssCart = new List<MyIssuanceCart>();
            Order objOrder = new Order();
            objOrder = new OrderConfirmationRepository().GetByOrderID(Convert.ToInt64(OrderID));
            Int64 intUserId = Convert.ToInt64(objOrder.UserId);
            UserInformation objUsrInfo = objUsrInfoRepo.GetById(intUserId);
            if (objUsrInfo != null)
            {
                strUserName = objUsrInfo.FirstName + " " + objUsrInfo.MiddleName + " " + objUsrInfo.LastName;
            }
            //End

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\"?>" +
                             "<SalesOrder/> ");
            XmlElement root = xmlDoc.DocumentElement;
            XmlElement subElemt = null;
            XmlElement subsubElemt = null;
            XmlElement item;
            XmlElement elem = addChildNode(xmlDoc, root, "CreatedOn", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"), false);
            elem = addChildNode(xmlDoc, root, "CreatedByName", objUsrInfo.FirstName, false);
            elem = addChildNode(xmlDoc, root, "Number", objOrder.OrderNumber, false);

            //-----BuyerShippingAddress
            if (objOrder.OrderFor == "ShoppingCart")
            {
                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
            }
            else
            {
                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
            }
            elem = addChildNode(xmlDoc, root, "BuyerShippingAddress");
            //addChildNode(xmlDoc, elem, "ContactName", objShippingInfo.CompanyName.ToString(), false);
            subElemt = addChildNode(xmlDoc, elem, "Street", objShippingInfo.CompanyName + " " + objShippingInfo.Address + " " + objShippingInfo.Address2 + " " + objShippingInfo.Street, false);
            subElemt = addChildNode(xmlDoc, elem, "City", new CityRepository().GetById((Int64)objShippingInfo.CityID).sCityName, false);
            subElemt = addChildNode(xmlDoc, elem, "State", new StateRepository().GetById((Int64)objShippingInfo.StateID).sStatename, false);
            subElemt = addChildNode(xmlDoc, elem, "ZipCode", objShippingInfo.ZipCode, false);
            subElemt = addChildNode(xmlDoc, elem, "Country", new CountryRepository().GetById((Int64)objShippingInfo.CountryID).sCountryName, "Code", "US", false);
            //------

            //-------Seller Address
            elem = addChildNode(xmlDoc, root, "SellerName", "Incentex", false);
            elem = addChildNode(xmlDoc, root, "SellerAddress");
            addChildNode(xmlDoc, elem, "Street", "648 Ocean Road", false);
            subElemt = addChildNode(xmlDoc, elem, "City", "Vero Beach", false);
            subElemt = addChildNode(xmlDoc, elem, "State", "FL", false);
            subElemt = addChildNode(xmlDoc, elem, "ZipCode", "32963", false);
            subElemt = addChildNode(xmlDoc, elem, "Country", "UNITED STATES", "Code", "US", false);
            //------------

            elem = addChildNode(xmlDoc, root, "BuyerName", objShippingInfo.Name, false);

            //------BuyerBillingAddress
            //if (objOrder.OrderFor == "ShoppingCart") // Company Pays
            //{
            //    objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
            //}
            //else
            //{
            //    objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
            //}
            //elem = addChildNode(xmlDoc, root, "BuyerBillingAddress");
            //addChildNode(xmlDoc, elem, "Street", objBillingInfo.Address + " " + objBillingInfo.Address2, false);
            //subElemt = addChildNode(xmlDoc, elem, "City", new CityRepository().GetById((Int64)objBillingInfo.CityID).sCityName, false);
            //subElemt = addChildNode(xmlDoc, elem, "State", new StateRepository().GetById((Int64)objBillingInfo.StateID).sStatename, false);
            //subElemt = addChildNode(xmlDoc, elem, "ZipCode", objBillingInfo.ZipCode, false);
            //subElemt = addChildNode(xmlDoc, elem, "Country", new CountryRepository().GetById((Int64)objBillingInfo.CountryID).sCountryName, "Code", "US", false);
            //------

            elem = addChildNode(xmlDoc, root, "ModeOfTransportation", "<Description>Air</Description>" +
                                                       "<Method>Air</Method>",
                                                       "Code", "40", true);
            //----------Item

            if (objOrder.OrderFor == "ShoppingCart")
            {
                elem = addChildNode(xmlDoc, root, "Items");
                objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);
                for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                {
                    if (objSelectMyShoppingCartProductResultList[i].SupplierID == supplierid)
                    {
                        //Item node start
                        subElemt = addChildNode(xmlDoc, elem, "Item");
                        item = addChildNode(xmlDoc, subElemt, "PartNumber", objSelectMyShoppingCartProductResultList[i].item.Replace(" ", ""), false);
                        item = addChildNode(xmlDoc, subElemt, "Pieces", Convert.ToString(Convert.ToInt32(objSelectMyShoppingCartProductResultList[i].Quantity)), false);
                        item = addChildNode(xmlDoc, subElemt, "Description", objSelectMyShoppingCartProductResultList[i].ProductDescrption1, false);

                        //Item Defination Start
                        subsubElemt = addChildNode(xmlDoc, subElemt, "ItemDefinition");
                        item = addChildNode(xmlDoc, subsubElemt, "PartNumber", objSelectMyShoppingCartProductResultList[i].item.Replace(" ", ""), false);
                        item = addChildNode(xmlDoc, subsubElemt, "Description", objSelectMyShoppingCartProductResultList[i].ProductDescrption1, false);
                        subElemt.AppendChild(subsubElemt);

                        //----------Sales Charge Node Start
                        subsubElemt = addChildNode(xmlDoc, subElemt, "SalesCharge");
                        item = addChildNode(xmlDoc, subsubElemt, "Type", "Standard", false);
                        item = addChildNode(xmlDoc, subsubElemt, "ExchangeRate", "1.00", false);
                        item = addChildNode(xmlDoc, subsubElemt, "PriceInCurrency", objSelectMyShoppingCartProductResultList[i].UnitPrice, "Currency", "USD", false);
                        item = addChildNode(xmlDoc, subsubElemt, "Price", objSelectMyShoppingCartProductResultList[i].UnitPrice, "Currency", "USD", false);
                        item = addChildNode(xmlDoc, subsubElemt, "Quantity", Convert.ToString(Convert.ToInt32(objSelectMyShoppingCartProductResultList[i].Quantity)), false);
                        Double UnitPrice = Convert.ToDouble(objSelectMyShoppingCartProductResultList[i].UnitPrice);
                        Int32 qty = Convert.ToInt32(Convert.ToInt32(objSelectMyShoppingCartProductResultList[i].Quantity));
                        String Amt = Convert.ToString(UnitPrice * qty);
                        item = addChildNode(xmlDoc, subsubElemt, "AmountInCurrency", Amt, "Currency", "USD", false);
                        item = addChildNode(xmlDoc, subsubElemt, "Amount", Amt, "Currency", "USD", false);
                        item = addChildNode(xmlDoc, subsubElemt, "Currency", "<Name>United States Dollar</Name>" +
                                                        "<ExchangeRate>1.00</ExchangeRate>" + "<DecimalPlaces>2</DecimalPlaces>"
                                                        + "<IsHomeCurrency>true</IsHomeCurrency>",
                                                        "Code", "USD", true);
                        subElemt.AppendChild(subsubElemt);

                        item = addChildNode(xmlDoc, subElemt, "SalesOrderNumber", objOrder.OrderNumber, false);
                        elem.AppendChild(subElemt);
                    }
                }
            }
            else
            {
                elem = addChildNode(xmlDoc, root, "Items");
                List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                for (Int32 i = 0; i < objFinal.Count; i++)
                {
                    if (objFinal != null && objFinal[i] != null)
                    {
                        if (objFinal[i].SupplierID == supplierid)
                        {
                            //Item node start
                            subElemt = addChildNode(xmlDoc, elem, "Item");
                            item = addChildNode(xmlDoc, subElemt, "PartNumber", objFinal[i].item.Replace(" ", ""), false);
                            item = addChildNode(xmlDoc, subElemt, "Pieces", Convert.ToString(Convert.ToInt32(objFinal[i].Qty)), false);
                            item = addChildNode(xmlDoc, subElemt, "Description", objFinal[i].ProductDescrption, false);

                            //Item Defination Start
                            subsubElemt = addChildNode(xmlDoc, subElemt, "ItemDefinition");
                            item = addChildNode(xmlDoc, subsubElemt, "PartNumber", objFinal[i].item.Replace(" ", ""), false);
                            item = addChildNode(xmlDoc, subsubElemt, "Description", objFinal[i].ProductDescrption, false);
                            subElemt.AppendChild(subsubElemt);

                            //----------Sales Charge Node Start
                            subsubElemt = addChildNode(xmlDoc, subElemt, "SalesCharge");
                            item = addChildNode(xmlDoc, subsubElemt, "Type", "Standard", false);
                            item = addChildNode(xmlDoc, subsubElemt, "ExchangeRate", "1.00", false);
                            item = addChildNode(xmlDoc, subsubElemt, "PriceInCurrency", Convert.ToString(objFinal[i].Rate), "Currency", "USD", false);
                            item = addChildNode(xmlDoc, subsubElemt, "Price", Convert.ToString(objFinal[i].Rate), "Currency", "USD", false);
                            item = addChildNode(xmlDoc, subsubElemt, "Quantity", Convert.ToString(Convert.ToInt32(objFinal[i].Qty)), false);
                            Double UnitPrice = Convert.ToDouble(objFinal[i].Rate);
                            Int32 qty = Convert.ToInt32(Convert.ToInt32(objFinal[i].Qty));
                            String Amt = Convert.ToString(UnitPrice * qty);
                            item = addChildNode(xmlDoc, subsubElemt, "AmountInCurrency", Amt, "Currency", "USD", false);
                            item = addChildNode(xmlDoc, subsubElemt, "Amount", Amt, "Currency", "USD", false);
                            item = addChildNode(xmlDoc, subsubElemt, "Currency", "<Name>United States Dollar</Name>" +
                                                            "<ExchangeRate>1.00</ExchangeRate>" + "<DecimalPlaces>2</DecimalPlaces>"
                                                            + "<IsHomeCurrency>true</IsHomeCurrency>",
                                                            "Code", "USD", true);
                            subElemt.AppendChild(subsubElemt);

                            item = addChildNode(xmlDoc, subElemt, "SalesOrderNumber", objOrder.OrderNumber, false);
                            elem.AppendChild(subElemt);
                        }
                    }
                }
            }

            root.AppendChild(elem);
            String a = NameBars();
            if (a != null)
            {
                elem = addChildNode(xmlDoc, root, "Notes", objOrder.SpecialOrderInstruction.Replace(Environment.NewLine, "<br />") + a, false);
            }
            else
            {
                elem = addChildNode(xmlDoc, root, "Notes", objOrder.SpecialOrderInstruction.Replace(Environment.NewLine, "<br />"), false);
            }

            root.AppendChild(elem);

            root.SetAttribute("xmlns", "http://www.magaya.com/XMLSchema/V1");

            return xmlDoc;
        }
        catch (Exception)
        {

            throw;
        }
    }

    #region Add Child Node

    /// <summary>
    /// - Add empty Child node To XMLDocument into XmlNode   
    /// </summary>
    /// <param name="xmlDoc">XML Doucument</param>
    /// <param name="parentNode">Parent Node </param>
    /// <param name="tag">Node Tag</param>
    /// <returns>XmlElement</returns>
    public XmlElement addChildNode(XmlDocument xmlDoc, XmlNode parentNode, String tag)
    {

        XmlElement elem = xmlDoc.CreateElement(tag);
        parentNode.AppendChild(elem);
        return elem;

    }

    /// <summary>
    /// - Add Child node To XMLDocument into XmlNode with Value and Atribute
    /// </summary>
    /// <param name="xmlDoc">XML Doucumen</param>
    /// <param name="parentNode">Parent Node</param>
    /// <param name="tag">Node Tag</param>
    /// <param name="value">Node Value</param>
    /// <param name="attrTag">Attribute Tag</param>
    /// <param name="attrValue">Attribute Value</param>
    /// <param name="isXML">Body Kind (XML or Text)</param>
    /// <returns>XmlElement</returns>
    public XmlElement addChildNode(XmlDocument xmlDoc, XmlNode parentNode, String tag, String value, String attrTag, String attrValue, Boolean isXML)
    {
        XmlElement elem = addChildNode(xmlDoc, parentNode, tag, value, isXML);
        if ((elem != null) && (attrTag.Trim() != ""))
            elem.SetAttribute(attrTag, attrValue);
        return elem;

    }

    /// <summary>
    /// - Add Child node To XMLDocument into XmlNode with Value
    /// </summary>
    /// <param name="xmlDoc">XML Doucumen</param>
    /// <param name="parentNode">Parent Node</param>
    /// <param name="tag">Node Tag</param>
    /// <param name="value">Node Value</param>
    /// <param name="isXML">Body Kind (XML or Text)</param>
    /// <returns>XmlElement</returns>
    public XmlElement addChildNode(XmlDocument xmlDoc, XmlNode parentNode, String tag, String value, Boolean isXML)
    {
        if (value != null)
        {
            XmlElement elem = addChildNode(xmlDoc, parentNode, tag);
            if (isXML)
                elem.InnerXml = value;
            else
                elem.InnerText = value;
            return elem;
        } return null;
    }

    #endregion

    #endregion


    /// <summary>
    /// Added By Prashant April 2013,
    /// To Update the Order and Send the Order Approval and Received Notification to IE,Admin, Supplier
    /// </summary>
    /// <param name="objOrder"></param>
    /// <param name="objUser"></param>
    protected void UpdateOrderAndSendConfirmation(Order objOrder, UserInformation objUser)
    {

        //update price level if different from the global selection and send confirmation mail to the IE
        Int32 MOASDefaultPriceLevel = 0;

        var objDefaultPriceLevel = db.GlobalMenuSettings.FirstOrDefault(a => a.WorkgroupId == objOrder.WorkgroupId && a.StoreId == objOrder.StoreID);
        if (objDefaultPriceLevel != null)
            MOASDefaultPriceLevel = Convert.ToInt32(objDefaultPriceLevel.MOASPaymentPricing);
        Decimal TotalOrderAmount = 0;


        //Update the Price Rate if Default Price Level for the workgroup is set

        if (objOrder.PaymentOption == (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS)
        {
            OrderDetailHistoryRepository objOrderDetailsRepo = new OrderDetailHistoryRepository();
            ProductItemPriceRepository objProductItemPriceRepo = new ProductItemPriceRepository();
            if (objOrder.OrderFor == Convert.ToString(Incentex.DAL.Common.DAEnums.OrderFor.ShoppingCart))
            {
                List<MyShoppinCart> objShoppingCart = objOrderDetailsRepo.GetShoppinCartDetailsByOrderID(objOrder.OrderID);
                foreach (var item in objShoppingCart)
                {
                    //Update the Price Rate if Default Price Level for the workgroup is different from the current pricelevel
                    if ((MOASDefaultPriceLevel != 0 && item.PriceLevel != MOASDefaultPriceLevel) || MOASDefaultPriceLevel == 0)
                    {

                        Incentex.DAL.SqlRepository.ProductItemPriceRepository.ProductItemPricingResult objProductPricing = objProductItemPriceRepo.GetSingleProductItemPrice(item.ItemNumber, (Int32)item.StoreProductID);

                        //db.MyShoppinCarts.Attach(item);

                        Decimal ItemPrice = objProductPricing.Level1;
                        if (MOASDefaultPriceLevel == 1)
                            ItemPrice = objProductPricing.Level1;
                        else if (MOASDefaultPriceLevel == 2)
                            ItemPrice = objProductPricing.Level2;
                        else if (MOASDefaultPriceLevel == 3)
                            ItemPrice = objProductPricing.Level3;
                        else if (MOASDefaultPriceLevel == 4)
                            ItemPrice = objProductPricing.Level4;

                        item.MOASPriceLevel = MOASDefaultPriceLevel == 0 ? 1 : MOASDefaultPriceLevel;
                        item.MOASUnitPrice = Convert.ToString(ItemPrice);
                        TotalOrderAmount += Decimal.Round((ItemPrice * Convert.ToInt32(item.Quantity)), 2);
                    }
                }
                objOrderDetailsRepo.SubmitChanges();
            }
            else if (objOrder.OrderFor == Convert.ToString(Incentex.DAL.Common.DAEnums.OrderFor.IssuanceCart))
            {
                List<MyIssuanceCart> objIssuanceCart = objOrderDetailsRepo.GetIssuanceCartDetailsByOrderID(objOrder.OrderID);
                foreach (var item in objIssuanceCart)
                {
                    if ((MOASDefaultPriceLevel != 0 && item.PriceLevel != MOASDefaultPriceLevel) || MOASDefaultPriceLevel == 0)
                    {
                        Incentex.DAL.SqlRepository.ProductItemPriceRepository.ProductItemPricingResult objProductPricing = objProductItemPriceRepo.GetSingleProductItemPrice(item.ItemNumber, (Int32)item.StoreProductID);

                        //db.MyIssuanceCarts.Attach(item);

                        Decimal ItemPrice = objProductPricing.Level1;
                        if (MOASDefaultPriceLevel == 1)
                            ItemPrice = objProductPricing.Level1;
                        else if (MOASDefaultPriceLevel == 2)
                            ItemPrice = objProductPricing.Level2;
                        else if (MOASDefaultPriceLevel == 3)
                            ItemPrice = objProductPricing.Level3;
                        else if (MOASDefaultPriceLevel == 4)
                            ItemPrice = objProductPricing.Level4;

                        item.MOASPriceLevel = MOASDefaultPriceLevel == 0 ? 1 : MOASDefaultPriceLevel;
                        item.MOASRate = ItemPrice;
                        TotalOrderAmount += Decimal.Round((ItemPrice * Convert.ToInt32(item.Qty)), 2);
                    }
                }
                objOrderDetailsRepo.SubmitChanges();

            }
        }

        if (TotalOrderAmount != 0)
        {
            objOrder.MOASOrderAmount = TotalOrderAmount;
            objOrder.MOASSalesTax = Decimal.Round((TotalOrderAmount + Convert.ToDecimal(objOrder.ShippingAmount)) * Convert.ToDecimal(objOrder.StrikeIronTaxRate), 2);
        }

        objOrder.OrderStatus = "Open";
        objOrder.UpdatedDate = DateTime.Now;
        OrderRepos.SubmitChanges();

        AddNoteHistory("Approved", objOrder.OrderID);

        sendVerificationEmail("Approved", objOrder.OrderID, objUser.LoginEmail, objUser.FirstName);//send email to CE

        if (!objOrder.ReferenceName.ToLower().Contains("test") && !IncentexGlobal.IsIEFromStoreTestMode)
        {
            //Billing
            if (objOrder.OrderFor == "ShoppingCart") // Company Pays
            {
                objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
            }
            else
            {
                objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
            }
            //Shipping
            if (objOrder.OrderFor == "ShoppingCart")
            {
                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
            }
            else
            {
                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
            }

            sendIEEmail("Approved", objOrder.OrderID);//send email to IE

            sendEmailToSupplier(objOrder.OrderID, objBillingInfo, objShippingInfo);//send order to supplier                   

            new SAPOperations().SubmitOrderToSAP(objOrder.OrderID);

            //add by Mayur on 29-April-2012 for Magaya Integration
            //SendDataToMagaya(objOrder);
        }
        else
        {
            sendTestOrderEmailNotification("Approved", objOrder.OrderID);
        }
    }



    #endregion
}
