using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class OrderManagement_OrderDetailForPrint : PageBase
{
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
    OrderConfirmationRepository OrderRepos = new OrderConfirmationRepository();
    OrderDocumentRepository objOrderDocRepos = new OrderDocumentRepository();
    Order objOrder = new Order();

    ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
    LookupRepository objLookRep = new LookupRepository();

    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
    CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();
    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();
    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Order Detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "   ";
            ((LinkButton)Master.FindControl("btnLogout")).Visible = false;
            ((HtmlImage)Master.FindControl("imgHomeBtn")).Visible = false;
            ((HtmlImage)Master.FindControl("imgSupportTickets")).Visible = false;
            ((HtmlImage)Master.FindControl("imgOpenServiceTicket")).Visible = false;

            if (Request.QueryString["Id"] != null)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString["Id"]);
                setOrderStatus();
            }

            BindAddress();
            BindOrderStatus();
            BindParentRepeaterSupplierAddress();
        }
    }
    private void setOrderStatus()
    {
        Order o = new OrderConfirmationRepository().GetByOrderID(this.OrderID);
        this.OrderStatus = o.OrderStatus;
        this.UserID = Convert.ToInt64(o.UserId);
    }

    //Added on 16 Apr
    private void BindAddress()
    {
        try
        {
            List<SelectOrderAddressResult> obj = new List<SelectOrderAddressResult>();
            obj = OrderRepos.GetOrderAddress(Convert.ToInt32(this.OrderID));
            if (obj.Count > 0)
            {
                lblOrderNo.Text = obj[0].OrderNumber;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    private void BindOrderStatus()
    {
        try
        {
            //Added on 24 Mar 11
            OrderConfirmationRepository objOrderConfir = new OrderConfirmationRepository();
            Order objOrder = objOrderConfir.GetByOrderID(this.OrderID);
            lblShipping.Text = Convert.ToDecimal(objOrder.ShippingAmount).ToString();
            lblSalesTax.Text = (objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)).ToString();
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
                lblOrderTotal.Text = Convert.ToString(Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.MOASSalesTax != null && objOrder.MOASSalesTax != 0 ? Convert.ToDecimal(objOrder.MOASSalesTax) : Convert.ToDecimal(objOrder.SalesTax)) + Convert.ToDecimal(objOrder.ShippingAmount));
            }
            lblOrderedDate.Text = Convert.ToDateTime(objOrder.OrderDate).ToString("MM/dd/yyyy");
            lblOrderBy.Text = objOrder.ReferenceName.ToString();
            txtOrderNotesForCECA.Text = objOrder.SpecialOrderInstruction;
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
            {
                trCreditType.Visible = false;
            }

            lblOrderStatus.Text = objOrder.OrderStatus.ToString();

            LookupRepository objLookRep = new LookupRepository();

            //Start Update Nagmani Change 31-May-2011

            // objBillingInfo = objCmpEmpContRep.GetBillingDetailById((Int64)objOrder.UserId);
            // BindBillingAddress();

            // objShippingInfo = objCmpEmpContRep.GetShippingDetailById((Int64)objOrder.UserId, objOrder.ShippingInfromationid);
            // BindShippingAddress();

            if (objOrder.OrderFor == "IssuanceCart")
            {
                //Comapny Pays
                objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                BindBillingAddress();

                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                BindShippingAddress();

            }
            else
            {
                objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                BindBillingAddress();

                objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                BindShippingAddress();

            }

            //End Nagmani Change 31-May-2011


        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void BindBillingAddress()
    {
        if (objBillingInfo != null)
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
            //lblBEmail.Text = objBillingInfo.Email;
            lblBName.Text = objBillingInfo.BillingCO + " " + objBillingInfo.Manager;
            //lblBPhone.Text = objBillingInfo.Telephone;
        }

    }
    protected void BindShippingAddress()
    {
        if (objShippingInfo != null)
        {
            lblSAddress.Text = objShippingInfo.Address;
            lblSAddress2.Text = objShippingInfo.Address2;
            lblSStreet.Text = objShippingInfo.Street;
            if (objShippingInfo.CityID != null && objShippingInfo.StateID != null)
                lblSCity.Text = objCity.GetById((Int64)objShippingInfo.CityID).sCityName + "," + objState.GetById((Int64)objShippingInfo.StateID).sStatename + " " + objShippingInfo.ZipCode;
            lblSCompany.Text = objShippingInfo.CompanyName;
            if (objShippingInfo.CountryID != null)
                lblSCountry.Text = objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName;
            lblSName.Text = objShippingInfo.Name + " " + objShippingInfo.Fax;
            lblSPhoneNumber.Text = objShippingInfo.Telephone;
            lblSEmailAddress.Text = objShippingInfo.Email;
        }
    }
    //End

    /// <summary>
    /// Nagmani Kumar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void parentRepeater_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            if (this.OrderStatus.ToUpper() == "CANCELED")
            {
                GridView gvOrderDetail = ((GridView)(e.Item.FindControl("gvOrderDetail")));
                ((LinkButton)e.Item.FindControl("lnkSaveOrderDetails")).Visible = false;
            }
        }
        /*try
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {

                PlaceHolder PHolderddlFileType = (PlaceHolder)e.Item.FindControl("PHolderddlFileType");
                PlaceHolder PlaceHolderddlFileTypeSupplier = (PlaceHolder)e.Item.FindControl("PlaceHolderddlFileTypeSupplier");
                //Check Here Supplier and Incentex Admin for File type dropdownlist item show
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                {
                    PHolderddlFileType.Visible = false;
                    PlaceHolderddlFileTypeSupplier.Visible = true;
                }
                else
                {
                    PHolderddlFileType.Visible = true;
                    PlaceHolderddlFileTypeSupplier.Visible = false;
                }
            

             


            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }*/
    }

    private void BindParentRepeaterSupplierAddress()
    {
        try
        {
            List<SelectSupplierAddressResult> obj = new List<SelectSupplierAddressResult>();
            List<SelectSupplierAddressBySupplierIdResult> objSupplier = new List<SelectSupplierAddressBySupplierIdResult>();
            List<Object> objMainRecord = new List<Object>();

            objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(this.OrderID));
            if (objOrder != null)
            {
                String[] MyShoppingcart;
                MyShoppingcart = objOrder.MyShoppingCartID.Split(',');

                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                {
                    //Login User's SupplierId will assign here..
                    Supplier objSup = new SupplierRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                    objSupplier = OrderRepos.GetSupplierAddressBySupplier(objOrder.OrderFor, this.OrderID, objSup.SupplierID);
                    if (objSupplier.Count > 0)
                    {
                        parentRepeater.DataSource = objSupplier;
                        parentRepeater.DataBind();
                    }

                }
                else
                {
                    obj = OrderRepos.GetSupplierAddress(objOrder.OrderFor, this.OrderID);
                    if (obj.Count > 0)
                    {
                        parentRepeater.DataSource = obj;
                        parentRepeater.DataBind();
                    }

                }


            }
            foreach (RepeaterItem repeaterItem in parentRepeater.Items)
            {
                //
                List<SelectShipToAddressByOrderIDResult> objShipToAddress = OrderRepos.GetShipToAddressByOrderId(this.OrderID);

                objOrder = OrderRepos.GetByOrderID(Convert.ToInt32(this.OrderID));
                //Start Nagmani Change 31-May-2011
                if (objOrder.OrderFor == "IssuanceCart")
                {
                    objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                }
                else
                {
                    objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                }
                //End Nagmani Change 31-May-2011

                ((Label)repeaterItem.FindControl("lblShipContaName")).Text = objShippingInfo.Name + " " + objShippingInfo.Fax;
                ((Label)repeaterItem.FindControl("lblShipToAddress1")).Text = objShippingInfo.CompanyName;
                ((Label)repeaterItem.FindControl("lblShipToAddress2")).Text = objShippingInfo.Address;
                if (objShippingInfo.CityID != null && objShippingInfo.StateID != null)
                    ((Label)repeaterItem.FindControl("lblShipZipCode")).Text = objCity.GetById((Int64)objShippingInfo.CityID).sCityName + "," + objState.GetById((Int64)objShippingInfo.StateID).sStatename + " " + objShippingInfo.ZipCode;
                if (objShippingInfo.CountryID != null)
                    ((Label)repeaterItem.FindControl("lblCountryName")).Text = objCountry.GetById((Int64)objShippingInfo.CountryID).sCountryName;


                if (objOrder != null)
                {

                    ((HiddenField)(repeaterItem.FindControl("hdnOrderFor"))).Value = objOrder.OrderFor;
                    HiddenField hdnCartID = (HiddenField)(repeaterItem.FindControl("hdnMyShoppingCartId"));
                    HiddenField hdnSuppleriD = (HiddenField)(repeaterItem.FindControl("hdnSupplierId"));
                    List<SelectOrderDetailsResult> obj1 = new List<SelectOrderDetailsResult>();
                    //Add Here For MyShoppingcartiD
                    List<Object> objRecord = new List<Object>();

                    String[] MyShoppingcart;

                    MyShoppingcart = objOrder.MyShoppingCartID.Split(',');
                    for (Int32 i = 0; i < MyShoppingcart.Count(); i++)
                    {
                        obj1 = OrderRepos.GetDetailOrder(Convert.ToInt32(MyShoppingcart[i]), Convert.ToInt32(hdnSuppleriD.Value), objOrder.OrderFor);

                        if (obj1.Count > 0)
                        {

                            for (Int32 z = 0; z < obj1.Count; z++)
                            {
                                obj1[z].SupplierId = Convert.ToInt32(hdnSuppleriD.Value);
                                objRecord.Add(obj1[z]);
                                objMainRecord.Add(obj1[z]);

                            }
                        }
                    }

                    if (objRecord.Count > 0)
                    {
                        ((GridView)(repeaterItem.FindControl("gvOrderDetail"))).DataSource = objRecord;
                        ((GridView)(repeaterItem.FindControl("gvOrderDetail"))).DataBind();
                    }


                    GridView gvOrderDetail = ((GridView)(repeaterItem.FindControl("gvOrderDetail")));
                    //Fill Shipper Dropdownlist
                    foreach (GridViewRow t in gvOrderDetail.Rows)
                    {
                        Label txtQtyOrder = (Label)t.FindControl("txtQtyOrder");
                        Label lblItemNumber = (Label)t.FindControl("lblItemNumber");
                        Label txtQtyOrderRemaining = (Label)t.FindControl("txtQtyOrderRemaining");
                        HiddenField MyShoppingCart = (HiddenField)t.FindControl("hdnMyShoppingCartiD");
                        CountTotalShippedQuantiyResult objTotalShipped = new CountTotalShippedQuantiyResult();
                        TextBox txtQtyShipped = (TextBox)t.FindControl("txtQtyShipped");
                        objTotalShipped = objShipOrderRepos.GetQtyShippedTotal(Convert.ToInt32(MyShoppingCart.Value), lblItemNumber.Text, this.OrderID);
                        if (!(String.IsNullOrEmpty(objTotalShipped.ToString())))
                        {
                            txtQtyOrderRemaining.Text = Convert.ToString((Convert.ToInt32(txtQtyOrder.Text)) - (Convert.ToInt32(objTotalShipped.ShipQuantity)));
                        }

                        if (txtQtyOrderRemaining.Text == "0")
                        {
                            txtQtyShipped.Enabled = false;
                            ((TextBox)t.FindControl("txtBackOrderDate")).Enabled = false;
                        }
                        else
                        {
                            txtQtyShipped.Enabled = true;
                            ((TextBox)t.FindControl("txtBackOrderDate")).Enabled = true;
                        }


                        List<ShipingOrder> objOrderList = new ShipOrderRepository().GetShippingOrders(this.OrderID, Convert.ToInt32(MyShoppingCart.Value), lblItemNumber.Text);
                        if (objOrderList.Count > 0)
                        {
                            //Build the Shipping Order Link
                            ((HyperLink)t.FindControl("hypEditShippmentOrder")).Visible = true;
                            ((HyperLink)t.FindControl("hypEditShippmentOrder")).NavigateUrl = "~/OrderManagement/EditShipOrder.aspx?Id=" + this.OrderID + "&ShoppingCartID=" + MyShoppingCart.Value + "&ItemNumber=" + lblItemNumber.Text;
                        }
                        else
                        {
                            ((HyperLink)t.FindControl("hypEditShippmentOrder")).Visible = false;
                        }
                    }
                }

            }
            gvMainOrderDetail.DataSource = objMainRecord;
            gvMainOrderDetail.DataBind();

            DisplaySupplierOrderNotes();


        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
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
            if (IncentexGlobal.CurrentMember.Usertype == (Int64)Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee)
            {
                if (lblProductItemID != null && txtQtyOrder != null && IncentexGlobal.CurrentMember.Usertype != (Int64)Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee)
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


        }

    }
    //Created By Ankit on 21 Mar 11
    public void DisplaySupplierOrderNotes()
    {
        try
        {
            foreach (RepeaterItem repeaterItem in parentRepeater.Items)
            {
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
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    //End
    protected void gvOrderDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gvOrderDetail_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((Image)e.Row.FindControl("imgColor")).ImageUrl = "../admin/Incentex_Used_Icons/" + new LookupRepository().GetIconByLookupName(((SelectOrderDetailsResult)(e.Row.DataItem)).Color);
        }
    }
}
