using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class OrderManagement_OrderDetail : PageBase
{
    #region Data Members

    Int64 OrderID
    {
        get
        {
            return Convert.ToInt64(ViewState["OrderID"]);
        }
        set
        {
            ViewState["OrderID"] = value;
        }
    }

    Int64 UserID
    {
        get
        {
            return Convert.ToInt64(ViewState["UserID"]);
        }
        set
        {
            ViewState["UserID"] = value;
        }
    }

    String OrderStatus
    {
        get
        {
            return Convert.ToString(ViewState["OrderStatus"]);
        }
        set
        {
            ViewState["OrderStatus"] = value;
        }
    }

    Decimal MOASTotalAmountForCAIE
    {
        get
        {
            return Convert.ToDecimal(ViewState["MOASTotalAmountForCAIE"]);
        }
        set
        {
            ViewState["MOASTotalAmountForCAIE"] = value;
        }
    }

    Int32 TotalItemInOrder
    {
        get
        {
            return Convert.ToInt32(ViewState["TotalItemInOrder"]);
        }
        set
        {
            ViewState["TotalItemInOrder"] = value;
        }
    }

    Int64 PaymentOption
    {
        get
        {
            return Convert.ToInt64(ViewState["PaymentOption"]);
        }
        set
        {
            ViewState["PaymentOption"] = value;
        }
    }


    LookupRepository objLookRep = new LookupRepository();
    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
    CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();
    OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
    CityRepository objCity = new CityRepository();
    CountryRepository objCountry = new CountryRepository();
    StateRepository objState = new StateRepository();
    Order objOrder = new Order();
    List<SelectOrderDetailsResult> objMainRecord = new List<SelectOrderDetailsResult>();
    #endregion

    #region Event Handlers

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Order Management System";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            CheckOrderDetailEntry();

            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Order Detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.OrderReturnURL;

            if (Request.QueryString["Id"] != null)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["Id"]);
                hdORderID.Value = this.OrderID.ToString();
                SetOrderStatus();
            }

            //Bind Menu
            menucontrol.PopulateMenu(0, 0, this.OrderID, 0, false);
            //End

            SetBillingShipping();
            BindParentRepeaterSupplierAddress();
            BindOrderStatus();
            NameBarFileLink();
            if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                ddlStatus.Enabled = true;
            else
                ddlStatus.Enabled = false;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                ddlStatus.Enabled = false;
            else
                ddlStatus.Enabled = true;

            if (this.OrderStatus.ToUpper() == "ORDER PENDING")//add by mayur for not change order status when is waiting for review
            {
                ddlStatus.Enabled = false;
            }
        }
    }

    #region Dropdown Control Events

    protected void ddlStatus_SelectedIndexChanged(Object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        OrderConfirmationRepository objOrdRepos = new OrderConfirmationRepository();

        objOrdRepos.UpdateStatus(Convert.ToInt64(this.OrderID), ddlStatus.SelectedItem.Text);

        #region Making a note of who changed the order status

        NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();
        NoteDetail objComNot = new NoteDetail();
        objComNot.Notecontents = "Changed order status from '" + this.OrderStatus + "' to '" + ddlStatus.SelectedItem.Text + "'.";
        objComNot.NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.OrderDetailsIEs);
        objComNot.ForeignKey = this.OrderID;
        objComNot.SpecificNoteFor = "IEInternalNotes";
        objComNot.CreateDate = System.DateTime.Now;
        objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objComNot.UpdateDate = System.DateTime.Now;
        objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
        objCompNoteHistRepos.Insert(objComNot);
        objCompNoteHistRepos.SubmitChanges();

        #endregion

        if (ddlStatus.SelectedItem.Text == "Canceled")
        {
            /*If order gets cancled then give back the credit amount he has used*/
            Order ordDetails = new OrderConfirmationRepository().GetByOrderID(Convert.ToInt64(this.OrderID));
            OrderConfirmationRepository objRepos = new OrderConfirmationRepository();

            if (ordDetails.CreditUsed != null)
            {
                CompanyEmployeeRepository objCmnyEmp = new CompanyEmployeeRepository();
                CompanyEmployee cmpEmpl = objCmnyEmp.GetByUserInfoId((Int64)ordDetails.UserId);

                if (ordDetails.CreditUsed == "Previous")
                {
                    cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + ordDetails.CreditAmt;
                    cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + ordDetails.CreditAmt;
                    cmpEmpl.CreditAmtToExpired = cmpEmpl.CreditAmtToExpired + ordDetails.CreditAmt;
                    objCmnyEmp.SubmitChanges();
                }
                else if (ordDetails.CreditUsed == "Anniversary")
                {
                    cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + ordDetails.CreditAmt;
                    cmpEmpl.CreditAmtToExpired = cmpEmpl.CreditAmtToExpired + ordDetails.CreditAmt;
                    objCmnyEmp.SubmitChanges();
                }

                #region EmployeeLedger
                //Added for the company employee ledger
                EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                EmployeeLedger objEmplLedger = new EmployeeLedger();
                EmployeeLedger transaction = new EmployeeLedger();
                //End

                objEmplLedger.UserInfoId = cmpEmpl.UserInfoID;
                objEmplLedger.CompanyEmployeeId = cmpEmpl.CompanyEmployeeID;
                objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ORCANIN.ToString(); ;
                objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.OderCancelFromIncentex.ToString();
                objEmplLedger.TransactionAmount = ordDetails.CreditAmt;
                objEmplLedger.AmountCreditDebit = "Credit";
                objEmplLedger.OrderNumber = ordDetails.OrderNumber;
                objEmplLedger.OrderId = ordDetails.OrderID;
                transaction = objLedger.GetLastTransactionByEmplID(cmpEmpl.CompanyEmployeeID);
                if (transaction != null)
                {
                    objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                    objEmplLedger.CurrentBalance = transaction.CurrentBalance + objEmplLedger.TransactionAmount;
                }


                objEmplLedger.TransactionDate = System.DateTime.Now;
                objEmplLedger.UpdateById = IncentexGlobal.CurrentMember.UserInfoID;
                objLedger.Insert(objEmplLedger);
                objLedger.SubmitChanges();

                //Starting Credits Add
                #endregion
            }

            List<Order> obj = objRepos.GetShoppingCartId(ordDetails.OrderNumber);
            if (obj.Count > 0)
            {
                String[] a;

                a = obj[0].MyShoppingCartID.ToString().Split(',');

                foreach (String u in a)
                {
                    if (ordDetails.OrderFor == "ShoppingCart")
                    {
                        //Shopping cart
                        MyShoppingCartRepository objShoppingCartRepos = new MyShoppingCartRepository();
                        MyShoppinCart objShoppingcart = new MyShoppinCart();
                        ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                        ProductItem objProductItem = new ProductItem();
                        objShoppingcart = objShoppingCartRepos.GetById(Convert.ToInt32(u), (Int64)ordDetails.UserId);

                        objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(objShoppingcart.StoreProductID), Convert.ToInt64(objShoppingcart.MasterItemNo), objShoppingcart.ItemNumber);
                        //Update Inventory Here 
                        //Call here upDate Procedure
                        String strProcess = "Shopping";
                        String strMessage = objRepos.IncreaseDescreaseInventory(objProductItem.ProductItemID, -Convert.ToInt32(objShoppingcart.Quantity), strProcess);
                    }
                    else
                    {
                        //Issuance
                        MyIssuanceCartRepository objIssuanceRepos = new MyIssuanceCartRepository();
                        MyIssuanceCart objIssuance = new MyIssuanceCart();
                        ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                        //End 

                        objIssuance = objIssuanceRepos.GetById(Convert.ToInt32(u), (Int64)ordDetails.UserId);
                        List<SelectProductIDResult> objList = objProItemRepos.GetProductId(objIssuance.MasterItemID, Convert.ToInt64(u), Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(ordDetails.StoreID));
                        //Update Inventory Here 
                        //Call here upDate Procedure
                        for (Int32 i = 0; i < objList.Count; i++)
                        {
                            String strProcess = "UniformIssuance";
                            String strMessage = objRepos.IncreaseDescreaseInventory(Convert.ToInt64(objList[i].ProductItemID), -Convert.ToInt32(objIssuance.Qty), strProcess);
                        }
                    }
                }
            }

            SetOrderStatus();
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

    #endregion

    #region Repeater Control Events

    protected void parentRepeater_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {

            HiddenField hdnSupplierId = (HiddenField)e.Item.FindControl("hdnSupplierId");
            HtmlContainerControl trPAASOrderNumber = (HtmlContainerControl)e.Item.FindControl("trPAASOrderNumber");
            Label lblPAASOrderNumber = (Label)e.Item.FindControl("lblPAASOrderNumber");

            Supplier PAAS = new SupplierRepository().GetSinglSupplierid("Pilot Freight Services");
            if (PAAS != null && !String.IsNullOrEmpty(hdnSupplierId.Value) && Convert.ToInt64(hdnSupplierId.Value) == PAAS.SupplierID && (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.UserInfoID == PAAS.UserInfoID))
            {
                trPAASOrderNumber.Visible = true;
                lblPAASOrderNumber.Text = objOrder.PAASOrderNumber;
            }
            else
            {
                trPAASOrderNumber.Visible = false;
            }

            if (objOrder.OrderFor == "IssuanceCart")
            {
                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
            }
            else
            {
                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
            }

            //End Nagmani Change 31-May-2011
            if (objShippingInfo != null)
            {
                ((Label)e.Item.FindControl("lblShipContaName")).Text = objShippingInfo.Name + " " + objShippingInfo.Fax;
                ((Label)e.Item.FindControl("lblShipToAddress1")).Text = objShippingInfo.CompanyName;
                ((Label)e.Item.FindControl("lblShipToAddress2")).Text = objShippingInfo.Address;
                ((Label)e.Item.FindControl("lblShipToAddress3")).Text = objShippingInfo.Address2;
                ((Label)e.Item.FindControl("lblShipToStreet")).Text = objShippingInfo.Street;
                if (objShippingInfo.CityID != null && objShippingInfo.StateID != null)
                    ((Label)e.Item.FindControl("lblShipZipCode")).Text = objCity.GetById((Int64)objShippingInfo.CityID).sCityName + "," + objState.GetById((Int64)objShippingInfo.StateID).sStatename + " " + objShippingInfo.ZipCode;
                if (objShippingInfo.CountryID != null)
                    ((Label)e.Item.FindControl("lblCountryName")).Text = objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName;
            }

            ((HiddenField)(e.Item.FindControl("hdnOrderFor"))).Value = objOrder.OrderFor;
            HiddenField hdnCartID = (HiddenField)(e.Item.FindControl("hdnMyShoppingCartId"));
            HiddenField hdnSuppleriD = (HiddenField)(e.Item.FindControl("hdnSupplierId"));
            List<SelectOrderDetailsResult> obj1 = new List<SelectOrderDetailsResult>();

            List<SelectOrderDetailsResult> objRecord = new List<SelectOrderDetailsResult>();

            String[] MyShoppingcart = objOrder.MyShoppingCartID.Split(',');
            for (Int32 i = 0; i < MyShoppingcart.Count(); i++)
            {
                obj1 = OrderRepos.GetDetailOrder(Convert.ToInt32(MyShoppingcart[i]), Convert.ToInt32(hdnSuppleriD.Value), objOrder.OrderFor);

                if (obj1.Count > 0)
                {
                    objRecord.AddRange(obj1.ToList());
                    objMainRecord.AddRange(obj1.ToList());
                }
            }

            if (objRecord.Count > 0)
            {
                GridView gvOrderDetail = ((GridView)(e.Item.FindControl("gvOrderDetail")));

                gvOrderDetail.DataSource = objRecord;
                gvOrderDetail.DataBind();

                if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) && IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                    gvOrderDetail.Columns[gvOrderDetail.Columns.Count - 1].Visible = false;
            }

            if (this.OrderStatus.ToUpper() == "CANCELED")
            {
                GridView gvOrderDetail = ((GridView)(e.Item.FindControl("gvOrderDetail")));
                ((LinkButton)e.Item.FindControl("lnkSaveOrderDetails")).Visible = false;
            }

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            {
                LinkButton lnkSaveOrderDetails = (LinkButton)e.Item.FindControl("lnkSaveOrderDetails");
                LinkButton lnkAddHisotry = (LinkButton)e.Item.FindControl("lnkAddHisotry");
                Panel pnlOrderHistory = (Panel)e.Item.FindControl("pnlOrderHistory");
                lnkSaveOrderDetails.Visible = false;
                lnkAddHisotry.Visible = false;
                pnlOrderHistory.Visible = false;
            }
            else
            {
                LinkButton lnkSaveOrderDetails = (LinkButton)e.Item.FindControl("lnkSaveOrderDetails");
                LinkButton lnkAddHisotry = (LinkButton)e.Item.FindControl("lnkAddHisotry");
                Panel pnlOrderHistory = (Panel)e.Item.FindControl("pnlOrderHistory");
                lnkSaveOrderDetails.Visible = true;
                lnkAddHisotry.Visible = true;
                pnlOrderHistory.Visible = true;
            }
        }
    }

    protected void parentRepeater_ItemCommand(Object source, RepeaterCommandEventArgs e)
    {
        try
        {
            //Insert new Record and Update Record in ShipOrder Table
            if (e.CommandName == "OrderDetails")
            {
                GridView gvOrderDetail = (GridView)e.Item.FindControl("gvOrderDetail");
                HiddenField hdnSupplierId = (HiddenField)e.Item.FindControl("hdnSupplierId");

                Int64 QtyShipped = 0;
                Int64? MyShoppingCartID = null;
                ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
                Boolean IsBackOrderedChanged = false;

                Order objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(this.OrderID));

                foreach (GridViewRow t in gvOrderDetail.Rows)
                {
                    Int32 intRemainingQty = 0;
                    Label lblItemNumber = (Label)t.FindControl("lblItemNumber");
                    TextBox txtBackOrderDate = (TextBox)t.FindControl("txtBackOrderDate");
                    Label lblColor = (Label)t.FindControl("lblColor");
                    Label lblSize = (Label)t.FindControl("lblSize");

                    HiddenField hdnBackOrderedUntil = (HiddenField)t.FindControl("hdnBackOrderedUntil");
                    HiddenField hdnMyShoppingCartiD = (HiddenField)t.FindControl("hdnMyShoppingCartiD");
                    TextBox txtQtyShipped = (TextBox)t.FindControl("txtQtyShipped");
                    Label txtQtyOrder = (Label)t.FindControl("txtQtyOrder");
                    Label txtQtyOrderRemaining = (Label)t.FindControl("txtQtyOrderRemaining");
                    Label lblAlreadyShipped = (Label)t.FindControl("lblAlreadyShipped");

                    if (Convert.ToInt64(hdnMyShoppingCartiD.Value) > 0)
                        MyShoppingCartID = Convert.ToInt64(hdnMyShoppingCartiD.Value);

                    if (!String.IsNullOrEmpty(txtBackOrderDate.Text.Trim()) && txtBackOrderDate.Text.Trim() != Convert.ToString(hdnBackOrderedUntil.Value))
                    {
                        if (objOrder.OrderFor == "ShoppingCart")
                        {
                            MyShoppingCartRepository objCartRepo = new MyShoppingCartRepository();
                            MyShoppinCart objCart = objCartRepo.GetById(Convert.ToInt32(MyShoppingCartID), Convert.ToInt64(objOrder.UserId));
                            if (objCart != null)
                            {
                                objCart.BackOrderedUntil = Convert.ToDateTime(txtBackOrderDate.Text.Trim());
                                objCartRepo.SubmitChanges();
                            }
                        }
                        else
                        {
                            MyIssuanceCartRepository objCartRepo = new MyIssuanceCartRepository();
                            MyIssuanceCart objCart = objCartRepo.GetById(Convert.ToInt32(MyShoppingCartID), Convert.ToInt64(objOrder.UserId));
                            if (objCart != null)
                            {
                                objCart.BackOrderedUntil = Convert.ToDateTime(txtBackOrderDate.Text.Trim());
                                objCartRepo.SubmitChanges();
                            }
                        }

                        IsBackOrderedChanged = true;
                    }

                    if (!String.IsNullOrEmpty(txtQtyOrderRemaining.Text.Trim()) && txtQtyOrderRemaining.Text.Trim() != "0" && !String.IsNullOrEmpty(txtQtyShipped.Text.Trim()) && txtQtyShipped.Text.Trim() != "---")
                    {
                        QtyShipped = Convert.ToInt64(txtQtyShipped.Text.Trim());

                        ShipingOrder objShiporder = new ShipingOrder();
                        objShiporder.OrderID = this.OrderID;
                        objShiporder.MyShoppingCartiD = MyShoppingCartID;
                        objShiporder.ShipQuantity = QtyShipped;

                        if (!String.IsNullOrEmpty(txtQtyOrder.Text))
                        {
                            CountTotalShippedQuantiyResult objTotalShipped = new CountTotalShippedQuantiyResult();
                            objTotalShipped = objShipOrderRepos.GetQtyShippedTotal(Convert.ToInt32(MyShoppingCartID), lblItemNumber.Text, this.OrderID);
                            objShiporder.RemaingQutOrder = Convert.ToInt64(Convert.ToInt64(txtQtyOrder.Text) - QtyShipped - (Convert.ToInt32(objTotalShipped.ShipQuantity)));
                            intRemainingQty = Convert.ToInt32(Convert.ToInt64(txtQtyOrder.Text) - QtyShipped - (Convert.ToInt32(objTotalShipped.ShipQuantity)));
                        }

                        objShiporder.SupplierId = Convert.ToInt64(hdnSupplierId.Value);
                        objShiporder.IsShipped = false;

                        objShiporder.UpdateBy = IncentexGlobal.CurrentMember.UserInfoID;
                        objShiporder.ItemNumber = lblItemNumber.Text;
                        objShiporder.QtyOrder = Convert.ToInt64(txtQtyOrder.Text);
                        objShiporder.ShippingOrderStatus = null;
                        objShiporder.ShipperService = null;
                        objShiporder.NoOfBoxes = null;

                        //if (!String.IsNullOrEmpty(txtBackOrderDate.Text))
                        //    objShiporder.BackOrderUntil = Convert.ToDateTime(txtBackOrderDate.Text);

                        if (intRemainingQty == 0)
                            objShiporder.ShippingOrderStatus = "Shipped Complete";
                        else
                            objShiporder.ShippingOrderStatus = "Partial Shipped";

                        objShiporder.ProcessedFrom = "Order Detail Page";
                        objShiporder.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                        objShiporder.CreatedDate = DateTime.Now;

                        //Here Insert ShipOrder Record
                        objShipOrderRepos.Insert(objShiporder);
                    }
                }

                objShipOrderRepos.SubmitChanges();

                if (IsBackOrderedChanged)
                    SendBackOrderNotificationEmail(objOrder.OrderID, objOrder.OrderNumber, objOrder.ReferenceName, objOrder.OrderDate, objOrder.OrderStatus, objOrder.OrderFor, objOrder.UserId);

                BindParentRepeaterSupplierAddress();

                Boolean IsOrderShippedComplete = new ShipOrderRepository().IsOrderShippedComplete(objOrder.OrderID);
                if (IsOrderShippedComplete)
                    new OrderConfirmationRepository().UpdatePAASOrder(objOrder.OrderID, "Closed", String.Empty);

                BindOrderStatus();
            }

            if (e.CommandName == "AddNotes")
            {
                ModalPopupExtender modalAddnotes = (ModalPopupExtender)e.Item.FindControl("modal  Addnotes");
                modalAddnotes.Show();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Gridview Control Events

    protected void gvMainOrderDetail_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();

            Label lblProductionDescription = (Label)e.Row.FindControl("lblProductionDescription");
            Label txtQtyOrder = (Label)e.Row.FindControl("txtQtyOrder");
            //updated by prashant on 28th Feb 2013
            Label lblMOASPrice = (Label)e.Row.FindControl("lblMOASPrice");
            Label lblMOASExtendedPrice = (Label)e.Row.FindControl("lblMOASExtendedPrice");
            Label lblPrice = (Label)e.Row.FindControl("lblPrice");
            Label lblExtendedPrice = (Label)e.Row.FindControl("lblExtendedPrice");
            Label lblProductItemID = (Label)e.Row.FindControl("lblProductItemID");
            if (lblProductItemID != null && txtQtyOrder != null && this.PaymentOption == (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS)
            {
                if (this.OrderStatus.ToLower() == "order pending")
                {
                    var result = OrderRepos.GetMOASPriceByUserIDProductItemID(this.UserID, Convert.ToInt64(lblProductItemID.Text));
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
                    lblPrice.Visible = false;
                    lblExtendedPrice.Visible = false;
                    lblMOASPrice.Visible = true;
                    lblMOASExtendedPrice.Visible = true;
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
                txtQtyOrderRemaining.Text = Convert.ToString((Convert.ToInt32(txtQtyOrder.Text)) - (Convert.ToInt32(objTotalShipped.ShipQuantity)));

            txtQtyShipped.Text = Convert.ToInt64(objTotalShipped.ShipQuantity) == 0 ? "---" : objTotalShipped.ShipQuantity.ToString();
            ((Image)e.Row.FindControl("imgColor")).ImageUrl = "~/admin/Incentex_Used_Icons/" + new LookupRepository().GetIconByLookupName(((SelectOrderDetailsResult)(e.Row.DataItem)).Color);

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                lblProductionDescription.Text = Convert.ToString(lblProductionDescription.ToolTip).Length > 30 ? Convert.ToString(lblProductionDescription.ToolTip).Substring(0, 30).Trim() + "..." : (Convert.ToString(lblProductionDescription.ToolTip) + "&nbsp;");
            else
                lblProductionDescription.Text = Convert.ToString(lblProductionDescription.ToolTip).Length > 25 ? Convert.ToString(lblProductionDescription.ToolTip).Substring(0, 25).Trim() + "..." : (Convert.ToString(lblProductionDescription.ToolTip) + "&nbsp;");

            //Hide delete button added on 13 July 2011

            if (objTotalShipped.ShipQuantity == null)
                ((LinkButton)e.Row.FindControl("hypDeleteShippedItem")).Visible = true;
            else
            {
                //If item is not shipped but shipped from the first tab of the page!
                List<ShipingOrder> objShipOrderCount = new ShipOrderRepository().GetShippingOrdersItemWise(this.OrderID, Convert.ToInt64(((LinkButton)e.Row.FindControl("hypDeleteShippedItem")).CommandArgument));
                if (objShipOrderCount.Count > 0)
                    ((LinkButton)e.Row.FindControl("hypDeleteShippedItem")).Visible = false;
                else
                    ((LinkButton)e.Row.FindControl("hypDeleteShippedItem")).Visible = true;
            }
        }
    }

    protected void gvMainOrderDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteOrderItem")
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

            Int64 CartId;
            CartId = Convert.ToInt64(e.CommandArgument.ToString());
            Decimal ItemAmount = 0;
            Decimal TransacationAmount = 0;

            try
            {
                #region Inventory
                OrderConfirmationRepository objOrderItem = new OrderConfirmationRepository();
                Order ordDetails = objOrderItem.GetByOrderID(Convert.ToInt64(this.OrderID));
                OrderConfirmationRepository objRepos = new OrderConfirmationRepository();

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

                        String strMessage = objRepos.IncreaseDescreaseInventory(objProductItem.ProductItemID, -Convert.ToInt32(((Label)gvMainOrderDetail.Rows[a.RowIndex].FindControl("txtQtyOrder")).Text), strProcess);
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
                        String strMessage = objRepos.IncreaseDescreaseInventory(Convert.ToInt64(objList[i].ProductItemID), -Convert.ToInt32(((Label)gvMainOrderDetail.Rows[a.RowIndex].FindControl("txtQtyOrder")).Text), strProcess);
                    }
                }

                #endregion

                #region Get Item Amount
                if (hdnOrderType.Value == "ShoppingCart")
                {

                    MyShoppingCartRepository objMyshoppingCartRep = new MyShoppingCartRepository();
                    MyShoppinCart objMyshoppingCart = objMyshoppingCartRep.GetDetailsById(CartId);
                    //Get Item Amount
                    ItemAmount = Convert.ToDecimal(objMyshoppingCart.UnitPrice) * Convert.ToDecimal(objMyshoppingCart.Quantity);
                }
                else
                {

                    MyIssuanceCartRepository objIssuanceCartRep = new MyIssuanceCartRepository();
                    MyIssuanceCart objMyIssuanceCart = objIssuanceCartRep.GetByIssuanceCartId(CartId);
                    //Get Item Amount
                    ItemAmount = Convert.ToDecimal(objMyIssuanceCart.Rate) * Convert.ToDecimal(objMyIssuanceCart.Qty);

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
                        /*New code done on 30-Dec-2011*/
                        if (ItemAmount < ordDetails.CreditAmt)
                        {
                            if (this.TotalItemInOrder == 1)
                            {
                                TransacationAmount = Convert.ToDecimal(ItemAmount + ordDetails.ShippingAmount);
                                cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + TransacationAmount;

                                //Update Order Amount again
                                ordDetails.CreditAmt = ordDetails.CreditAmt - TransacationAmount;
                                ordDetails.OrderAmount = 0;
                                ordDetails.ShippingAmount = 0;
                                objOrderItem.SubmitChanges();
                                //END
                            }
                            else
                            {
                                Decimal[] itemline = CheckIfLastItemInOrder(Convert.ToInt64(e.CommandArgument.ToString()));
                                //Below line lies in logic with as details
                                //I am subtracting OLD total order amount to new total order amount
                                TransacationAmount = Convert.ToDecimal((ordDetails.OrderAmount + ordDetails.ShippingAmount) - (itemline[0] + itemline[1]));
                                cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + TransacationAmount;
                                //END

                                //Update Order Amount again
                                ordDetails.CreditAmt = ordDetails.CreditAmt - TransacationAmount;
                                ordDetails.OrderAmount = itemline[0];
                                ordDetails.ShippingAmount = itemline[1];
                                objOrderItem.SubmitChanges();
                                //END
                            }
                        }
                        else
                        {

                            if (this.TotalItemInOrder == 1)
                            {
                                if (ordDetails.CreditAmt != 0)
                                {
                                    //Both is used
                                    if (ordDetails.PaymentOption != null)
                                    {
                                        TransacationAmount = Convert.ToDecimal(ordDetails.CreditAmt);
                                        cmpEmpl.StratingCreditAmount = TransacationAmount;
                                        ordDetails.CreditAmt = ordDetails.CreditAmt - TransacationAmount;
                                    }
                                    else if (ordDetails.PaymentOption == null)
                                    {
                                        TransacationAmount = Convert.ToDecimal(ItemAmount + ordDetails.ShippingAmount);
                                        cmpEmpl.StratingCreditAmount = TransacationAmount;
                                        ordDetails.CreditAmt = ordDetails.CreditAmt - TransacationAmount;
                                    }


                                }
                                //Update Order Amount again
                                ordDetails.OrderAmount = 0;
                                ordDetails.ShippingAmount = 0;
                                objOrderItem.SubmitChanges();
                                //END
                            }
                            else
                            {
                                Decimal[] itemline = CheckIfLastItemInOrder(Convert.ToInt64(e.CommandArgument.ToString()));
                                //Update Order Amount again
                                ordDetails.CreditAmt = ordDetails.CreditAmt - TransacationAmount;
                                ordDetails.OrderAmount = itemline[0];
                                ordDetails.ShippingAmount = itemline[1];
                                objOrderItem.SubmitChanges();
                                //END

                                TransacationAmount = (Decimal)ordDetails.CreditAmt;
                                cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + ordDetails.CreditAmt;
                            }
                        }

                        objCmnyEmp.SubmitChanges();

                    }
                    else if (ordDetails.CreditUsed == "Anniversary")
                    {
                        /*New code done on 30-Dec-2011*/
                        if (ItemAmount < ordDetails.CreditAmt)
                        {
                            if (this.TotalItemInOrder == 1)
                            {
                                TransacationAmount = Convert.ToDecimal(ItemAmount + ordDetails.ShippingAmount);
                                cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + TransacationAmount;


                                //Update Order Amount again
                                ordDetails.CreditAmt = ordDetails.CreditAmt - TransacationAmount;
                                ordDetails.OrderAmount = 0;
                                ordDetails.ShippingAmount = 0;
                                objOrderItem.SubmitChanges();
                                //END
                            }
                            else
                            {
                                Decimal[] itemline = CheckIfLastItemInOrder(Convert.ToInt64(e.CommandArgument.ToString()));
                                //Below line lies in logic with as details
                                //I am subtracting OLD total order amount to new total order amount
                                TransacationAmount = Convert.ToDecimal((ordDetails.OrderAmount + ordDetails.ShippingAmount) - (itemline[0] + itemline[1]));
                                cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + TransacationAmount;
                                //END

                                //Update Order Amount again
                                ordDetails.CreditAmt = ordDetails.CreditAmt - TransacationAmount;
                                ordDetails.OrderAmount = itemline[0];
                                ordDetails.ShippingAmount = itemline[1];
                                objOrderItem.SubmitChanges();
                                //END
                            }
                        }
                        else
                        {
                            if (this.TotalItemInOrder == 1)
                            {
                                if (ordDetails.CreditAmt != 0)
                                {
                                    //Both is used
                                    if (ordDetails.PaymentOption != null)
                                    {
                                        TransacationAmount = Convert.ToDecimal(ordDetails.CreditAmt);
                                        cmpEmpl.CreditAmtToApplied = TransacationAmount;
                                        ordDetails.CreditAmt = ordDetails.CreditAmt - TransacationAmount;
                                    }
                                    else if (ordDetails.PaymentOption == null)
                                    {
                                        TransacationAmount = Convert.ToDecimal(ItemAmount + ordDetails.ShippingAmount);
                                        cmpEmpl.CreditAmtToApplied = TransacationAmount;
                                        ordDetails.CreditAmt = ordDetails.CreditAmt - TransacationAmount;
                                    }
                                }
                                //Update Order Amount again
                                ordDetails.OrderAmount = 0;
                                ordDetails.ShippingAmount = 0;
                                objOrderItem.SubmitChanges();
                                //END
                            }
                            else
                            {
                                Decimal[] itemline = CheckIfLastItemInOrder(Convert.ToInt64(e.CommandArgument.ToString()));
                                TransacationAmount = Convert.ToDecimal((ordDetails.OrderAmount + ordDetails.ShippingAmount) - (itemline[0] + itemline[1]));
                                //Update Order Amount again
                                ordDetails.CreditAmt = ordDetails.CreditAmt - TransacationAmount;
                                ordDetails.OrderAmount = itemline[0];
                                ordDetails.ShippingAmount = itemline[1];
                                objOrderItem.SubmitChanges();
                                //END

                                TransacationAmount = (Decimal)ordDetails.CreditAmt;
                                cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + ordDetails.CreditAmt;
                            }
                        }

                        objCmnyEmp.SubmitChanges();

                    }

                    #region EmployeeLedger
                    if (TransacationAmount != 0)
                    {
                        //Added for the company employee ledger
                        EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                        EmployeeLedger objEmplLedger = new EmployeeLedger();
                        EmployeeLedger transaction = new EmployeeLedger();
                        //End

                        objEmplLedger.UserInfoId = cmpEmpl.UserInfoID;
                        objEmplLedger.CompanyEmployeeId = cmpEmpl.CompanyEmployeeID;
                        objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ORITDEL.ToString(); ;
                        objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.OrderItemDeleted.ToString();
                        objEmplLedger.TransactionAmount = TransacationAmount;


                        objEmplLedger.AmountCreditDebit = "Credit";
                        objEmplLedger.OrderNumber = ordDetails.OrderNumber;
                        objEmplLedger.OrderId = ordDetails.OrderID;
                        transaction = objLedger.GetLastTransactionByEmplID(cmpEmpl.CompanyEmployeeID);
                        if (transaction != null)
                        {
                            objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                            objEmplLedger.CurrentBalance = transaction.CurrentBalance + objEmplLedger.TransactionAmount;
                        }


                        objEmplLedger.TransactionDate = System.DateTime.Now;
                        objEmplLedger.UpdateById = IncentexGlobal.CurrentMember.UserInfoID;
                        objLedger.Insert(objEmplLedger);
                        objLedger.SubmitChanges();
                    }
                    //Starting Credits Add
                    #endregion
                }
                #region Update orderamount
                /*Updated on 30 Dec 2011
                //Update values in the Order table like order amount (subtract value)
                if (ordDetails.CreditUsed != null)
                {
                    Order updateOrder = OrderRepos.GetByOrderID(this.OrderID);
                    //Below will check if the last item is deleted
                    if (updateOrder.MyShoppingCartID.Contains(','))
                    {
                        updateOrder.OrderAmount = updateOrder.OrderAmount - ItemAmount;
                    }
                    //Last item in the cart
                    else
                    {
                        updateOrder.OrderAmount = updateOrder.OrderAmount - ItemAmount;
                        updateOrder.ShippingAmount = 0;
                    }

                    OrderRepos.SubmitChanges();
                }*/
                #endregion

                //Update values again(Page Load again)
                if (this.TotalItemInOrder == 1)
                {
                    //Check if its last item
                    OrderConfirmationRepository objO = new OrderConfirmationRepository();
                    Order o = objO.GetByOrderID(this.OrderID);
                    o.OrderStatus = "Canceled";
                    objO.SubmitChanges();
                    //End
                }

                #region Delete Items
                //Check order type
                //If its Shoping cart then delete from MyshoppingCart else delete from myIssuancecart
                //Delete Item from the shopping or issuance cart before that get value of Qua and Price from the table
                if (hdnOrderType.Value == "ShoppingCart")
                {

                    MyShoppingCartRepository objMyshoppingCartRep = new MyShoppingCartRepository();
                    MyShoppinCart objMyshoppingCart = objMyshoppingCartRep.GetDetailsById(CartId);
                    objMyshoppingCartRep.Delete(objMyshoppingCart);
                    objMyshoppingCartRep.SubmitChanges();
                }
                else
                {

                    MyIssuanceCartRepository objIssuanceCartRep = new MyIssuanceCartRepository();
                    MyIssuanceCart objMyIssuanceCart = objIssuanceCartRep.GetByIssuanceCartId(CartId);

                    objIssuanceCartRep.Delete(objMyIssuanceCart);
                    objIssuanceCartRep.SubmitChanges();

                }
                #endregion

                SetOrderStatus();
                SetBillingShipping();
                BindOrderStatus();
                BindParentRepeaterSupplierAddress();
                //End
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else if (e.CommandName == "EditOrderItem")
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            GridViewRow a = ((GridViewRow)((LinkButton)e.CommandSource).Parent.Parent);
            Response.Redirect("OrderItemEditQty.aspx?Id=" + this.OrderID + "&ShoppingCartId=" + e.CommandArgument.ToString() + "&OrderType=" + hdnOrderType.Value + "&SupplierId=" + ((HiddenField)gvMainOrderDetail.Rows[a.RowIndex].FindControl("hdnSupplierId")).Value);
        }
    }

    protected void gvOrderDetail_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((Image)e.Row.FindControl("imgColor")).ImageUrl = "../admin/Incentex_Used_Icons/" + new LookupRepository().GetIconByLookupName(((SelectOrderDetailsResult)(e.Row.DataItem)).Color);

            Label txtQtyOrder = (Label)e.Row.FindControl("txtQtyOrder");
            Label lblItemNumber = (Label)e.Row.FindControl("lblItemNumber");
            Label txtQtyOrderRemaining = (Label)e.Row.FindControl("txtQtyOrderRemaining");
            HiddenField MyShoppingCart = (HiddenField)e.Row.FindControl("hdnMyShoppingCartiD");
            CountTotalShippedQuantiyResult objTotalShipped = new CountTotalShippedQuantiyResult();
            TextBox txtQtyShipped = (TextBox)e.Row.FindControl("txtQtyShipped");
            Label lblAlreadyShipped = (Label)e.Row.FindControl("lblAlreadyShipped");
            TextBox txtBackOrderDate = (TextBox)e.Row.FindControl("txtBackOrderDate");

            objTotalShipped = objShipOrderRepos.GetQtyShippedTotal(Convert.ToInt32(MyShoppingCart.Value), lblItemNumber.Text, this.OrderID);

            if (!(String.IsNullOrEmpty(objTotalShipped.ToString())))
                txtQtyOrderRemaining.Text = Convert.ToString((Convert.ToInt32(txtQtyOrder.Text)) - (Convert.ToInt32(objTotalShipped.ShipQuantity)));

            lblAlreadyShipped.Text = Convert.ToInt64(objTotalShipped.ShipQuantity) == 0 ? "---" : objTotalShipped.ShipQuantity.ToString();
            txtQtyShipped.Text = "---";

            if (txtQtyOrderRemaining.Text == "0")
            {
                txtQtyShipped.Enabled = false;
                txtBackOrderDate.Enabled = false;
            }
            else
            {
                txtQtyShipped.Enabled = true;
                txtBackOrderDate.Enabled = true;
            }

            List<ShipingOrder> objOrderList = new ShipOrderRepository().GetShippingOrders(this.OrderID, Convert.ToInt32(MyShoppingCart.Value), lblItemNumber.Text);

            if (objOrderList.Count > 0)
            {
                //Build the Shipping Order Link
                ((HyperLink)e.Row.FindControl("hypEditShippmentOrder")).Visible = true;
                ((HyperLink)e.Row.FindControl("hypEditShippmentOrder")).NavigateUrl = "~/OrderManagement/EditShipOrder.aspx?Id=" + this.OrderID + "&ShoppingCartID=" + MyShoppingCart.Value + "&ItemNumber=" + lblItemNumber.Text;
            }
            else
            {
                ((HyperLink)e.Row.FindControl("hypEditShippmentOrder")).Visible = false;
            }
        }
    }

    #endregion

    #region Button Control Events

    protected void lnkButtonSaveHistory_Click(Object sender, EventArgs e)
    {
        try
        {
            String strNoteHistory = "";
            foreach (RepeaterItem repeaterItem in parentRepeater.Items)
            {
                TextBox txtNote = (TextBox)repeaterItem.FindControl("txtOrderNotesHistory");
                HiddenField hdnSupplierid = (HiddenField)repeaterItem.FindControl("hdnSupplierId");

                //NoteHistory for Supplier ORder
                String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.SupplierOrder);

                NoteDetail objComNot = new NoteDetail();
                NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();

                if (Convert.ToInt64(hdnSupplierid.Value) != 0)
                {
                    objComNot.Notecontents = txtNote.Text;
                    objComNot.NoteFor = strNoteFor;
                    objComNot.ForeignKey = Convert.ToInt64(hdnSupplierid.Value);
                    objComNot.SpecificNoteFor = Convert.ToString(this.OrderID);
                    objComNot.CreateDate = System.DateTime.Now;
                    objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                    objComNot.UpdateDate = System.DateTime.Now;
                    objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

                    if (!(String.IsNullOrEmpty(txtNote.Text)))
                    {
                        strNoteHistory = txtNote.Text;
                        objCompNoteHistRepos.Insert(objComNot);
                        objCompNoteHistRepos.SubmitChanges();
                        Int32 NoteId = (Int32)(objComNot.NoteID);
                        txtNote.Text = "";
                    }
                }
            }

            SendVerificationEmail(strNoteHistory, this.OrderID);
            DisplaySupplierOrderNotes();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnEditBilling_Click(Object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

        SetBillingAddress();
        modalEditBilling.Show();
    }

    protected void btnEditShipping_Click(Object sender, EventArgs e)
    {
        if (!base.CanEdit)
        {
            base.RedirectToUnauthorised();
        }

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
            ClearBillingDetails();
            //Bind Updated Details again
            BindOrderStatus();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #endregion

    #region Methods

    private void CheckOrderDetailEntry()
    {
        OrderDetailHistoryRepository objHis = new OrderDetailHistoryRepository();
        OrderDetailPageHistory objPageHistory = objHis.GetByOrderId(Convert.ToInt64(Request.QueryString["Id"]));

        if (objPageHistory != null)
        {
            if (objPageHistory.UserId == null)
            {
                //Access ON
                AddOrderDetailHistory();
            }
            else if (objPageHistory.UserId == IncentexGlobal.CurrentMember.UserInfoID)
            {
                //user have refresh the page and give it ON
                //Access ON
            }
            else
            {
                //Access OFF
                Response.Redirect("~/OrderManagement/OrderNotAccessible.aspx?OrderId=" + Convert.ToInt64(Request.QueryString["Id"]));
                //return;
            }
        }
        else
        {
            AddOrderDetailHistory();
        }
    }

    private void AddOrderDetailHistory()
    {
        OrderDetailHistoryRepository objHis = new OrderDetailHistoryRepository();
        OrderDetailPageHistory objPageHistory = objHis.GetByOrderId(Convert.ToInt64(Request.QueryString["Id"]));

        if (objPageHistory != null)
        {
            objPageHistory.UserId = IncentexGlobal.CurrentMember.UserInfoID;
            objPageHistory.InDateTime = DateTime.Now;
            objHis.SubmitChanges();
        }
    }

    private void SetOrderStatus()
    {
        Order o = new OrderConfirmationRepository().GetByOrderID(this.OrderID);
        if (o != null)
        {
            this.OrderStatus = o.OrderStatus;
            this.UserID = Convert.ToInt64(o.UserId);
            this.PaymentOption = Convert.ToInt64(o.PaymentOption);
            if (this.OrderStatus.ToUpper() == "CANCELED")
            {
                ddlStatus.Enabled = false;
                btnEditBilling.Enabled = false;
                btnEditShipping.Enabled = false;
            }
            else
            {
                ddlStatus.Enabled = true;
                btnEditBilling.Enabled = true;
                btnEditShipping.Enabled = true;
            }
        }
    }

    private void BindOrderStatus()
    {
        try
        {
            //Added on 24 Mar 11
            OrderConfirmationRepository objOrderConfir = new OrderConfirmationRepository();
            Order objOrder = objOrderConfir.GetByOrderID(this.OrderID);
            hdnOrderType.Value = objOrder.OrderFor;
            lblShipping.Text = Convert.ToDecimal(objOrder.ShippingAmount).ToString();
            lblStrikeIronSalesTax.Text = Convert.ToDecimal(objOrder.StrikeIronTaxRate).ToString();
            lblSalesTax.Text = (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)).ToString();
            //lblOrderTotal.Text = Convert.ToString(Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.SalesTax) + Convert.ToDecimal(objOrder.ShippingAmount));
            if (objOrder.PaymentOption == (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS && (IncentexGlobal.CurrentMember.Usertype == (Int64)Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin || IncentexGlobal.CurrentMember.Usertype == (Int64)Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin || IncentexGlobal.CurrentMember.Usertype == (Int64)Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            {
                if (objOrder.OrderStatus.ToLower() == "order pending")
                {
                    Decimal SalesTaxValue = Convert.ToDecimal(lblSalesTax.Text);
                    if (lblStrikeIronSalesTax.Text != "")
                    {
                        SalesTaxValue = Decimal.Round((MOASTotalAmountForCAIE + Convert.ToDecimal(lblShipping.Text)) * Convert.ToDecimal(lblStrikeIronSalesTax.Text), 2);
                        lblSalesTax.Text = SalesTaxValue.ToString("f2");
                    }
                    lblOrderTotal.Text = Convert.ToString(Convert.ToDecimal(MOASTotalAmountForCAIE) + SalesTaxValue + Convert.ToDecimal(lblShipping.Text));
                }
                else
                    lblOrderTotal.Text = Convert.ToString(Convert.ToDecimal(objOrder.MOASOrderAmount) + (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + Convert.ToDecimal(objOrder.ShippingAmount));
            }
            else
            {
                lblOrderTotal.Text = Convert.ToString(Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.SalesTax) + Convert.ToDecimal(objOrder.ShippingAmount));
            }

            if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)
            {
                trCorporateDiscount.Visible = true;
                lblCorporateDiscount.Text = objOrder.CorporateDiscount.ToString();
            }

            TimeZoneInfo newYorkTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime DisplayTime = TimeZoneInfo.ConvertTime(Convert.ToDateTime(objOrder.OrderDate), newYorkTimeZone);
            lblOrderedDate.Text = DisplayTime.ToString("MM/dd/yyyy HH:mm:ss");

            lblOrderBy.Text = objOrder.ReferenceName.ToString();

            //Get and Bind if there is NameBars is used in the order
            String nb = NameBars();

            if (nb == String.Empty)
                txtOrderNotesForCECA.Text = objOrder.SpecialOrderInstruction;
            else
            {
                if (objOrder.SpecialOrderInstruction != "")
                    txtOrderNotesForCECA.Text = objOrder.SpecialOrderInstruction + "\n\n\n" + nb;
                else
                    txtOrderNotesForCECA.Text = objOrder.SpecialOrderInstruction + nb;
            }

            if (objOrder.PaymentOption != null)
            {
                INC_Lookup objLookup = new LookupRepository().GetById((Int64)objOrder.PaymentOption);
                lblPaymentMethod.Text = objLookup.sLookupName + (!String.IsNullOrEmpty(objOrder.PaymentOptionCode) ? " : " + objOrder.PaymentOptionCode : "");
            }
            else
                lblPaymentMethod.Text = "Paid By Corporate";
            //End Added

            //Added on 18 Arp By Ankit
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
                trCreditType.Visible = false;

            lblOrderStatus.Text = objOrder.OrderStatus.ToString();

            LookupRepository objLookRep = new LookupRepository();
            String strPayment = "StatusOptionOne";
            ddlStatus.DataSource = objLookRep.GetByLookup(strPayment);
            ddlStatus.DataValueField = "iLookupID";
            ddlStatus.DataTextField = "sLookupName";
            ddlStatus.DataBind();
            ddlStatus.Items.FindByText(lblOrderStatus.Text).Selected = true;

            if (objOrder.OrderFor == "IssuanceCart")
            {
                //Comapny Pays
                objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);

                if (objBillingInfo != null)
                    BindBillingAddress();

                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");

                if (objShippingInfo != null)
                    BindShippingAddress();
            }
            else
            {
                objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);

                if (objBillingInfo != null)
                    BindBillingAddress();

                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");

                if (objShippingInfo != null)
                    BindShippingAddress();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindBillingAddress()
    {
        lblBAddress1.Text = objBillingInfo.Address;
        lblBAddress2.Text = objBillingInfo.Address2;
        if (objBillingInfo.CityID != null && objBillingInfo.StateID != null)
            lblBCity.Text = objCity.GetById((Int64)objBillingInfo.CityID).sCityName + "," + objState.GetById((Int64)objBillingInfo.StateID).sStatename + " " + objBillingInfo.ZipCode;
        lblBCompany.Text = objBillingInfo.CompanyName;
        lblBPhoneNumber.Text = objBillingInfo.Telephone;
        lblBEmailAddress.Text = objBillingInfo.Email;
        if (objBillingInfo.CountryID != null)
            lblBCountry.Text = objCountry.GetById((Int64)objBillingInfo.CountryID).sCountryName;
        if (!String.IsNullOrEmpty(objBillingInfo.BillingCO))
            lblBName.Text = objBillingInfo.BillingCO + " " + objBillingInfo.Manager;
        else
            lblBName.Text = objBillingInfo.Name;
        hfBillingInfoID.Value = objBillingInfo.CompanyContactInfoID.ToString();
    }

    private void SetBillingShipping()
    {
        if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
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
        else
        {
            btnEditBilling.Visible = false;
            btnEditShipping.Visible = false;
        }
    }

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
        if (objBillingInfo.CountryID != null)
            drpCountry.Items.FindByValue(objBillingInfo.CountryID.ToString()).Selected = true;


        //Bind State
        if (objBillingInfo.CountryID != null)
        {
            Common.BindState(drpState, (Int64)objBillingInfo.CountryID);
            drpState.Items.FindByValue(objBillingInfo.StateID.ToString()).Selected = true;
        }
        else
            Common.BindState(drpState, 0);

        //bind City
        if (objBillingInfo.StateID != null)
        {
            Common.BindCity(drpCity, (Int64)objBillingInfo.StateID);
            drpCity.Items.FindByValue(objBillingInfo.CityID.ToString()).Selected = true;
        }
        else
            Common.BindCity(drpCity, 0);

        txtZipcodeEdit.Text = objBillingInfo.ZipCode;
        if (!String.IsNullOrEmpty(objBillingInfo.BillingCO))
            txtBillingFnameEdit.Text = objBillingInfo.BillingCO;
        else
            txtBillingFnameEdit.Text = objBillingInfo.Name;
        if (!String.IsNullOrEmpty(objBillingInfo.Manager))
            txtBillingLnameEdit.Text = objBillingInfo.Manager;
        else
            txtBillingLnameEdit.Text = objBillingInfo.Name;
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
        txtBillingAddressEdit.MaxLength = 35;
        //dvAddress2Edit.Visible = false;
        dvStreetEdit.Visible = false;
    }

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
        if (objShippingInfo.CountryID != null)
            drpCountry.Items.FindByValue(objShippingInfo.CountryID.ToString()).Selected = true;

        //Bind State
        if (objShippingInfo.CountryID != null)
        {
            Common.BindState(drpState, (Int64)objShippingInfo.CountryID);
            drpState.Items.FindByValue(objShippingInfo.StateID.ToString()).Selected = true;
        }
        else
            Common.BindState(drpState, 0);

        //bind City
        if (objShippingInfo.StateID != null)
        {
            Common.BindCity(drpCity, (Int64)objShippingInfo.StateID);
            drpCity.Items.FindByValue(objShippingInfo.CityID.ToString()).Selected = true;
        }
        else
            Common.BindCity(drpCity, 0);

        txtZipcodeEdit.Text = objShippingInfo.ZipCode;
        txtBillingFnameEdit.Text = objShippingInfo.Name;
        txtBillingLnameEdit.Text = objShippingInfo.Fax;
        txtBillingCompanyNameEdit.Text = objShippingInfo.CompanyName;
        txtBillingAddressEdit.Text = objShippingInfo.Address;
        txtBillingAddress2Edit.Text = objShippingInfo.Address2;
        txtBillingStreetEdit.Text = objShippingInfo.Street;
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

    protected void BindShippingAddress()
    {
        lblSAddress.Text = objShippingInfo.Address;
        lblSAddress2.Text = objShippingInfo.Address2;
        lblSStreet.Text = objShippingInfo.Street;
        if (objBillingInfo.CityID != null && objBillingInfo.StateID != null)
            lblSCity.Text = objCity.GetById((Int64)objShippingInfo.CityID).sCityName + "," + objState.GetById((Int64)objShippingInfo.StateID).sStatename + " " + objShippingInfo.ZipCode;
        lblSCompany.Text = objShippingInfo.CompanyName;
        if (objBillingInfo.CountryID != null)
            lblSCountry.Text = objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName;
        lblSName.Text = objShippingInfo.Name + " " + objShippingInfo.Fax;
        lblSPhoneNumber.Text = objShippingInfo.Telephone;
        lblSEmailAddress.Text = objShippingInfo.Email;

        if (objShippingInfo.OrderFor != null)
            lblSOrderFor.Text = objLookRep.GetById(Convert.ToInt64(objShippingInfo.OrderFor)).sLookupName;

        hfShippingInfoID.Value = objShippingInfo.CompanyContactInfoID.ToString();
    }

    private void BindParentRepeaterSupplierAddress()
    {
        try
        {
            objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(this.OrderID));

            if (objOrder != null)
            {
                lblOrderNo.Text = objOrder.OrderNumber;

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                {
                    //Login User's SupplierId will assign here..
                    Supplier objSup = new SupplierRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                    List<SelectSupplierAddressBySupplierIdResult> objSelectSupplierAddressBySupplierIdResult = OrderRepos.GetSupplierAddressBySupplier(objOrder.OrderFor, this.OrderID, objSup.SupplierID);
                    if (objSelectSupplierAddressBySupplierIdResult.Count > 0)
                    {
                        parentRepeater.DataSource = objSelectSupplierAddressBySupplierIdResult;
                        parentRepeater.DataBind();
                    }
                }
                else
                {
                    List<SelectSupplierAddressResult> objSelectSupplierAddressResult = OrderRepos.GetSupplierAddress(objOrder.OrderFor, this.OrderID);
                    if (objSelectSupplierAddressResult.Count > 0)
                    {
                        parentRepeater.DataSource = objSelectSupplierAddressResult;
                        parentRepeater.DataBind();
                    }
                }

                //Bind Order Summary Grid
                gvMainOrderDetail.DataSource = objMainRecord;
                gvMainOrderDetail.DataBind();

                if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) && IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                {
                    gvMainOrderDetail.Columns[gvMainOrderDetail.Columns.Count - 1].Visible = false;
                    gvMainOrderDetail.Columns[gvMainOrderDetail.Columns.Count - 2].Visible = false;

                    if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                    {
                        //gvMainOrderDetail.Columns[3].Visible = false;
                        gvMainOrderDetail.Columns[4].Visible = false;
                        gvMainOrderDetail.Columns[gvMainOrderDetail.Columns.Count - 3].Visible = true;
                    }
                }
                else
                    gvMainOrderDetail.Columns[gvMainOrderDetail.Columns.Count - 3].Visible = false;

                this.TotalItemInOrder = objMainRecord.Count;
                DisplaySupplierOrderNotes();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public void DisplaySupplierOrderNotes()
    {
        try
        {
            foreach (RepeaterItem repeaterItem in parentRepeater.Items)
            {
                TextBox txtOrderNotesForCECA = (TextBox)repeaterItem.FindControl("txtNotesHistory");
                txtOrderNotesForCECA.Text = String.Empty;
                HiddenField hdnSupplierid = ((HiddenField)(repeaterItem.FindControl("hdnSupplierid")));
                NotesHistoryRepository objRepo = new NotesHistoryRepository();
                List<NoteDetail> objList = objRepo.GetNotesForCACEId(Convert.ToInt64(hdnSupplierid.Value), Incentex.DAL.Common.DAEnums.NoteForType.SupplierOrder, Convert.ToString(this.OrderID));

                foreach (NoteDetail obj in objList)
                {
                    txtOrderNotesForCECA.Text += "Created Date : " + Convert.ToDateTime(obj.CreateDate).ToString("dd-MMM-yyyy") + "\n";
                    txtOrderNotesForCECA.Text += "Created Time : " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";
                    UserInformationRepository objUserRepo = new UserInformationRepository();
                    UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);

                    if (objUser != null)
                    {
                        txtOrderNotesForCECA.Text += "Created By : " + objUser.FirstName + " " + objUser.LastName + "\n";
                    }

                    txtOrderNotesForCECA.Text += "Note : " + obj.Notecontents + "\n";
                    txtOrderNotesForCECA.Text += "---------------------------------------------";
                    txtOrderNotesForCECA.Text += "\n";
                }

                //Get and Bind if there is NameBars is used in the order
                String nb = NameBars();

                if (nb != String.Empty)
                    txtOrderNotesForCECA.Text += nb;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SendVerificationEmail(String strNotes, Int64 OrderID)
    {
        try
        {
            Order objOrder = OrderRepos.GetByOrderID(this.OrderID);
            UserInformation objInfo = new UserInformationRepository().GetById(Convert.ToInt64(objOrder.UserId));

            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;

            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "IE NoteHistory";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

            String sToadd = null;

            if (dsEmailTemplate != null)
            {
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();

                if (objInfo != null)
                    sToadd = objInfo.LoginEmail;

                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{OrderNo}", objOrder.OrderNumber);
                messagebody.Replace("{Commentssection}", strNotes);
                messagebody.Replace("{fullname}", objInfo.FirstName);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(objInfo.UserInfoID, "Order Detail", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SendBackOrderNotificationEmail(Int64 OrderID, String OrderNumber, String ReferenceName, DateTime? OrderDate, String OrderStatus, String OrderFor, Int64? UserInfoID)
    {
        List<SelectBackOrderedItemsByOrderIDResult> lstBackOrderedItems = new OrderConfirmationRepository().SelectBackOrderedItemsByOrderID(OrderID, OrderFor);
        if (lstBackOrderedItems.Count > 0)
        {
            UserInformation objUser = new UserInformationRepository().GetById(Convert.ToInt64(UserInfoID));
            if (objUser != null)
            {
                EmailTemplateBE objEmailBE = new EmailTemplateBE();
                EmailTemplateDA objEmailDA = new EmailTemplateDA();
                DataSet dsEmailTemplate;

                //Get Email Content
                objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
                objEmailBE.STemplateName = "BackOrder";
                dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);

                if (dsEmailTemplate != null)
                {
                    String sFrmadd = Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"]);
                    String sToadd = objUser.LoginEmail;
                    //String sToadd = "devraj.gadhavi@indianic.com";

                    String sSubject = Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sSubject"]) + " - Order # " + OrderNumber;
                    String sFrmname = Convert.ToString(dsEmailTemplate.Tables[0].Rows[0]["sFromName"]);

                    String eMailTemplate = String.Empty;

                    StreamReader _StreamReader;
                    _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/BackOrderedUntil.htm"));
                    eMailTemplate = _StreamReader.ReadToEnd();
                    _StreamReader.Close();
                    _StreamReader.Dispose();

                    StringBuilder messagebody = new StringBuilder(eMailTemplate);
                    messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    messagebody.Replace("{FullName}", objUser.FirstName + " " + objUser.LastName);
                    messagebody.Replace("{OrderNumber}", OrderNumber);
                    messagebody.Replace("{referencename}", ReferenceName);
                    messagebody.Replace("{OrderDate}", OrderDate != null ? Convert.ToDateTime(OrderDate).ToString("MM/dd/yyyy") : "");
                    messagebody.Replace("{OrderStatus}", OrderStatus);

                    StringBuilder innermessage = new StringBuilder();

                    foreach (SelectBackOrderedItemsByOrderIDResult BackOrderedItem in lstBackOrderedItems)
                    {
                        innermessage.Append("<tr>");
                        innermessage.Append("<td width='20%' style='font-weight: bold; text-align: center;'>");
                        innermessage.Append(BackOrderedItem.RemainingQty);
                        innermessage.Append("</td>");
                        innermessage.Append("<td width='55%' style='font-weight: bold; text-align: center;'>");
                        innermessage.Append(BackOrderedItem.ProductDescrption);
                        innermessage.Append("</td>");
                        innermessage.Append("<td width='25%' style='font-weight: bold; text-align: center;'>");
                        innermessage.Append(BackOrderedItem.BackOrderedUntil != null ? Convert.ToDateTime(BackOrderedItem.BackOrderedUntil).ToString("MM/dd/yyyy") : "");
                        innermessage.Append("</td>");
                        innermessage.Append("</tr>");
                        innermessage.Append("<tr>");
                        innermessage.Append("<td colspan='7'>");
                        innermessage.Append("<hr />");
                        innermessage.Append("</td>");
                        innermessage.Append("</tr>");
                    }

                    messagebody.Replace("{innermessage}", innermessage.ToString());

                    String smtphost = Application["SMTPHOST"].ToString();
                    Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                    String smtpUserID = Application["SMTPUSERID"].ToString();
                    String smtppassword = Application["SMTPPASSWORD"].ToString();

                    new CommonMails().SendMail(objUser.UserInfoID, "Back Order Notification", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, OrderID);
                }
            }
        }
    }

    private void ClearBillingDetails()
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

    private Decimal[] CheckIfLastItemInOrder(Int64 CartId)
    {
        Decimal[] arr = new Decimal[2];
        Decimal itemprice = 0;
        Decimal itemtotalprice = 0;
        Order objOrderItems = new OrderConfirmationRepository().GetByOrderID(this.OrderID);
        List<String> objItems = objOrderItems.MyShoppingCartID.Split(',').ToList<String>();
        objItems.Remove(CartId.ToString());

        if (objOrderItems.OrderFor == "IssuanceCart")
        {
            List<MyIssuanceCart> objIssList = new MyIssuanceCartRepository().GetIssuanceCartByOrderID(objOrderItems.OrderID);

            foreach (MyIssuanceCart objItem in objIssList)
            {
                itemprice = Convert.ToDecimal(objItem.Qty) * Convert.ToDecimal(objItem.Rate);
                itemtotalprice = itemtotalprice + itemprice;
            }
        }
        else
        {
            List<SelectMyShoppingCartProductResult> objMyShpList = new MyShoppingCartRepository().SelectCurrentOrderProducts(objOrderItems.OrderID);

            foreach (SelectMyShoppingCartProductResult objItem in objMyShpList)
            {
                itemprice = Convert.ToDecimal(objItem.Quantity) * Convert.ToDecimal(objItem.UnitPrice);
                itemtotalprice = itemtotalprice + itemprice;
            }
        }

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
        TotalSaleAbove = Convert.ToDecimal(objCS.totalsaleabove);

        if (itemtotalprice <= TotalSaleAbove)
            ShippingAmount = MinShipAmt; // Shipping amount = only minimum shipping amount
        else
            ShippingAmount = MinShipAmt + ((itemtotalprice - TotalSaleAbove) * ShipPercent / 100);

        arr[0] = Convert.ToDecimal(itemtotalprice);
        arr[1] = Convert.ToDecimal(ShippingAmount);
        return arr;
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
                        strNameBars += "Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n";
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
                        strNameBars += "Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n";
                }
            }
        }

        return strNameBars.ToString();
    }

    private String NameBarBulkFile(Int64 OrderID)
    {
        OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
        String strNameBars = String.Empty;
        Order objOrder = objOrderConfirmationRepository.GetByOrderID(OrderID);
        List<SelectMyShoppingCartProductResult> objMyShpList = new MyShoppingCartRepository().SelectCurrentOrderProducts(objOrder.OrderID);
        foreach (SelectMyShoppingCartProductResult objItem in objMyShpList)
        {
            if (!String.IsNullOrEmpty(objItem.FilePath))
            {
                if (String.IsNullOrEmpty(strNameBars))
                    strNameBars = objItem.FilePath;
                else
                    strNameBars += "," + objItem.FilePath;
            }
        }
        return strNameBars.ToString();
    }
    private void NameBarFileLink()
    {
        try
        {
            //--Saurabh || Name Bar Bulk Upload
            String NameBarList = NameBarBulkFile(OrderID);
            dvNameBarFile.Visible = false;
            if (!String.IsNullOrEmpty(NameBarList))
            {
                dvNameBarFile.Visible = true;
                List<String> myList = new List<String>(NameBarList.Split(','));
                String filePath = Server.MapPath("~/Images/excel_small.png");
                HyperLink DynLink = new HyperLink();
                for (Int32 i = 0; i < myList.Count; i++)
                {
                    DynLink.ID = myList[i];
                    DynLink.Text = myList[i];
                    DynLink.NavigateUrl = "~/UploadedImages/NameBarFile/" + myList[i];
                    DynLink.Target = "_blank";
                    DynLink.ImageUrl = "~/Images/excel_small.png";
                    PlaceHolder1.Controls.Add(DynLink);
                }
            }

        }
        catch (Exception)
        { }
    }
    public String DisplayTotal(String supplierid)
    {
        List<SelectOrderDetailsResult> obj1 = new List<SelectOrderDetailsResult>();
        Order objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(this.OrderID));
        List<SelectOrderDetailsResult> objMainRecord = new List<SelectOrderDetailsResult>();

        if (objOrder != null)
        {
            List<SelectOrderDetailsResult> objRecord = new List<SelectOrderDetailsResult>();
            String[] MyShoppingcart;
            MyShoppingcart = objOrder.MyShoppingCartID.Split(',');

            for (Int32 i = 0; i < MyShoppingcart.Count(); i++)
            {
                obj1 = OrderRepos.GetDetailOrder(Convert.ToInt32(MyShoppingcart[i]), Convert.ToInt32(supplierid), objOrder.OrderFor);
                if (obj1.Count > 0)
                {
                    objRecord.AddRange(obj1.ToList());
                }
            }

            if (objRecord.Count > 0)
                objMainRecord = objRecord;
        }

        return objMainRecord.Count.ToString();
    }

    #endregion
}