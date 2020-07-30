using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_OrderDetail : PageBase
{
    #region Properties

    string PSubCategoryName
    {
        get
        {
            if (Session["SubCat"] == null)
            {
                Session["SubCat"] = "";
            }
            return Session["SubCat"].ToString();
        }
        set
        {
            Session["SubCat"] = value;
        }
    }

    string sProcess
    {
        get
        {
            if (ViewState["sProcess"] == null)
            {
                ViewState["sProcess"] = null;
            }
            return ViewState["sProcess"].ToString();
        }
        set
        {
            ViewState["sProcess"] = value;
        }
    }
    int storeProductId
    {
        get
        {
            if (ViewState["storeProductId"] == null)
            {
                ViewState["storeProductId"] = 0;
            }
            return Convert.ToInt32(ViewState["storeProductId"]);
        }
        set
        {
            ViewState["storeProductId"] = value;
        }
    }
    #endregion

    #region Objects/Variables

    StoreProductImageRepository objStoreProductImageRepository = new StoreProductImageRepository();
    LookupRepository objLookUpRep = new LookupRepository();

    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    MyShoppinCart objShoppingCart = new MyShoppinCart();
    List<SelectMyShoppingCartProductResult> obj;

    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    CompanyEmployee objCmpnyInfo = new CompanyEmployee();

    UserInformationRepository objUsrInfoRepo = new UserInformationRepository();
    UserInformation objUsrInfo = new UserInformation();

    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
    CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();

    CityRepository objCity = new CityRepository();
    StateRepository objState = new StateRepository();
    CountryRepository objCountry = new CountryRepository();

    CompanyRepository objCmpRepo = new CompanyRepository();
    UniformIssuancePolicyItem objUniIssPolItem = new UniformIssuancePolicyItem();
    UniformIssuancePolicyItemRepository objUniIssPolItemRepo = new UniformIssuancePolicyItemRepository();

    MyIssuanceCartRepository objMyIssCartRepo = new MyIssuanceCartRepository();
    List<MyIssuanceCart> objMyIssCart = new List<MyIssuanceCart>();
    LookupRepository objLookupRepo = new LookupRepository();

    ProductItemDetailsRepository objProdItemDetRepo = new ProductItemDetailsRepository();
    List<ProductItem> objProdItem = new List<ProductItem>();
    OrderConfirmationRepository objRepos = new OrderConfirmationRepository();

    decimal totalRunning = 0;
    decimal shippingamount = 0;
    decimal salestax = 0;
    int ShippingAddressId = 0;
    string PaymentMethod = string.Empty;

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


    #endregion

    #region Page Load Event
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                this.OrderID = Convert.ToInt64(Request.QueryString.Get("Id"));
                BindOrderDetails(this.OrderID);
            }
            //try
            //{
            //    if (Session["Process"] != null)
            //    {
            //        if (Session["Process"].ToString() == "sc")
            //        {
            //            this.sProcess = "sc";
            //        }
            //        else
            //        {
            //            this.sProcess = "ui";
            //        }
            //    }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Order Detail";
            if (Request.QueryString.Count == 2)
            {
                if (Request.QueryString["from"].ToString() == "ep")
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/CompanyProgram/EmployeeProfile.aspx";
                }
                else
                {
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/CompanyProgram/ViewLedgerDetails.aspx";
                }
            }
            else
            {

                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/CompanyProgram/MyAnniversaryCredits.aspx";

            }


            //    BindVariables();

            //    if (this.sProcess == "ui" && Session["PolicyItemID"] != null) // Company Pays
            //    {
            //        objUniIssPolItem = objUniIssPolItemRepo.GetById(long.Parse(Session["PolicyItemID"].ToString()));
            //        BindCmpBillingAddress();
            //    }
            //    else    // Employee Pays
            //    {
            //        objBillingInfo = objCmpEmpContRep.GetBillingDetailById(IncentexGlobal.CurrentMember.UserInfoID);
            //        `
            //    }

            //    objShippingInfo = objCmpEmpContRep.GetShippingDetailById(IncentexGlobal.CurrentMember.UserInfoID, ShippingAddressId);
            //    BindShippingAddress();

            //    objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

            //    if (this.sProcess == "sc") // Shopping Process
            //    {
            //        obj = objShoppingCartRepository.SelectCurrentOrderProducts(Session["CartIDs"].ToString());
            //        rptMyIssuanceCart.Visible = false;
            //        rptMyShoppingCart.Visible = true;
            //    }
            //    else    // Uniform Issuance Process
            //    {
            //        objMyIssCart = objMyIssCartRepo.GetCurrentOrderProducts(Session["CartIDs"].ToString());
            //        long? storeid;

            //        CompanyEmployeeRepository objcmpemprepo = new CompanyEmployeeRepository();
            //        AnniversaryProgramRepository objacprepo = new AnniversaryProgramRepository();

            //        storeid = objacprepo.GetEmpStoreId(IncentexGlobal.CurrentMember.UserInfoID, objcmpemprepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).WorkgroupID).StoreID;

            //        //foreach (MyIssuanceCart item in objMyIssCart)
            //        //{
            //        //    ProductItem objSingleItem = new ProductItem();
            //        //    objSingleItem = objProdItemDetRepo.GetAllProductItem(Convert.ToInt32(item.MasterItemID)).Where(s=>s.ItemSizeID == item.ItemSizeID).FirstOrDefault();
            //        //    objProdItem.Add(objSingleItem);
            //        //}
            //        List<SelectMyIssuanceProductItemsResult> objFinal;
            //        objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objMyIssCart, storeid);
            //        rptMyShoppingCart.Visible = false;
            //        rptMyIssuanceCart.Visible = true;
            //        rptMyIssuanceCart.DataSource = objFinal;
            //        rptMyIssuanceCart.DataBind();
            //        foreach (RepeaterItem item in rptMyIssuanceCart.Items)
            //        {
            //            ((HtmlImage)item.FindControl("imgSplashImage")).Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + ((HiddenField)item.FindControl("hdndocumentname")).Value + "&_twidth=145&_theight=198";
            //            ((HtmlAnchor)item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/" + ((HiddenField)item.FindControl("hdnlargerimagename")).Value;
            //        }
            //    }

            //    BindMiscDetails();

            //    // Bind Repeater
            //    if (obj.Count > 0 && rptMyShoppingCart.Visible == true)
            //    {
            //        rptMyShoppingCart.DataSource = obj;
            //        rptMyShoppingCart.DataBind();
            //        ShowHideTailoringOption();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ex.Message.ToString();
            //}
        }
    }
    protected void Calculate()
    {
        try
        {
            decimal totalRunning = 0;
            decimal totalNotEligibleAnniversary = 0;
            if (this.sProcess == "sc")
            {
                foreach (RepeaterItem item in rptMyShoppingCart.Items)
                {
                    Label lblUnitPrice = (Label)item.FindControl("lblUnitPrice");
                    Label lblQuantity = (Label)item.FindControl("lblQuantity");
                    Label lblTotal = (Label)item.FindControl("lblTotal");
                    Label lblTotalPrice = (Label)rptMyShoppingCart.Controls[rptMyShoppingCart.Controls.Count - 1].Controls[0].FindControl("lblTotalProductPrice");
                    HiddenField lblsLookupName = (HiddenField)item.FindControl("hdnAnniversary");
                    if (lblsLookupName.Value == "No")
                    {
                        if (lblQuantity.Text != "" || lblUnitPrice.Text != "")
                        {
                            lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                            totalNotEligibleAnniversary += Convert.ToDecimal(lblTotal.Text);
                        }

                    }
                    else
                    {
                        if (lblQuantity.Text != "" || lblUnitPrice.Text != "")
                        {
                            lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                            totalRunning += Convert.ToDecimal(lblTotal.Text);
                        }
                    }
                    lblTotalPrice.Text = totalRunning.ToString("#,##0.00");
                    Session["TotalAmount"] = (totalRunning);
                    Session["NotAnniversaryCreditAmount"] = totalNotEligibleAnniversary;

                }
            }
            else
            {
                foreach (RepeaterItem item in rptMyIssuanceCart.Items)
                {
                    Label lblUnitPrice = (Label)item.FindControl("lblUnitPrice");
                    Label lblQuantity = (Label)item.FindControl("lblQuantity");
                    Label lblTotal = (Label)item.FindControl("lblTotal");
                    Label lblTotalPrice = (Label)rptMyIssuanceCart.Controls[rptMyIssuanceCart.Controls.Count - 1].Controls[0].FindControl("lblTotalProductPrice");

                    if (lblQuantity.Text != "" || lblUnitPrice.Text != "")
                    {
                        lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                        totalRunning += Convert.ToDecimal(lblTotal.Text);
                    }

                    lblTotalPrice.Text = totalRunning.ToString("#,##0.00");
                    Session["TotalAmount"] = totalRunning;
                    Session["NotAnniversaryCreditAmount"] = null;
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
    #endregion

    public void BindOrderDetails(Int64 OrderId)
    {
        OrderConfirmationRepository objOrderConfir = new OrderConfirmationRepository();
        Order objOrder = objOrderConfir.GetByOrderID(OrderId);
        lblOrderId.Text = objOrder.OrderNumber;
        lblOrderDate.Text = objOrder.OrderDate.ToString();
        lblPurchasedBy.Text = objOrder.ReferenceName != null ? Convert.ToString(objOrder.ReferenceName) : "---";

        //Bind Order Notes : Created on 3-Mar-11 By Ankit
        lblOrderNotes.Text = objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />");
        //End Bind

        if (objOrder.OrderFor == "IssuanceCart")
        {
            //Comapny Pays
            objBillingInfo = objCmpEmpContRep.GetBillingDetailIssuanceAddress((long)objOrder.UserId, objOrder.OrderID);
            if (objBillingInfo != null)
            {
                BindBillingAddress();
            }

            objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((long)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
            if (objShippingInfo != null)
            {
                BindShippingAddress();
            }


        }
        else
        {
            objBillingInfo = objCmpEmpContRep.GetBillingDetailAddress((long)objOrder.UserId, objOrder.OrderID);
            if (objBillingInfo != null)
            {
                BindBillingAddress();
            }

            objShippingInfo = objCmpEmpContRep.GetShippingDetailAddress((long)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
            if (objShippingInfo != null)
            {
                BindShippingAddress();
            }

        }


        ////Fill Billing
        //objBillingInfo = objCmpEmpContRep.GetBillingDetailById((long)objOrder.UserId);
        //BindBillingAddress();

        //objShippingInfo = objCmpEmpContRep.GetShippingDetailById((long)objOrder.UserId,(long)objOrder.ShippingInfromationid);
        //BindShippingAddress();

        if (objOrder.PaymentOption != null)
        {
            INC_Lookup objLookup = objLookUpRep.GetById((long)objOrder.PaymentOption);
            lblPaymentMethod.Text = objLookup.sLookupName + (!string.IsNullOrEmpty(objOrder.PaymentOptionCode) ? " : " + objOrder.PaymentOptionCode : "");
        }
        else
        {
            lblPaymentMethod.Text = "No Method";
        }

        //string[] a = objOrder.MyShoppingCartID.ToString().Split(',');
        if (objOrder.OrderFor == "IssuanceCart")
        {
            List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);
            rptMyShoppingCart.Visible = false;
            rptMyIssuanceCart.Visible = true;
            rptMyIssuanceCart.DataSource = objFinal;
            rptMyIssuanceCart.DataBind();
            foreach (RepeaterItem item in rptMyIssuanceCart.Items)
            {
                ((HtmlImage)item.FindControl("imgSplashImage")).Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + ((HiddenField)item.FindControl("hdndocumentname")).Value + "&_twidth=145&_theight=198";
                ((HtmlAnchor)item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/" + ((HiddenField)item.FindControl("hdnlargerimagename")).Value;
            }

        }
        else
        {
            obj = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);
            rptMyShoppingCart.DataSource = obj;
            rptMyShoppingCart.DataBind();
        }



        if (objOrder.CreditUsed == "Previous")
        {
            lblCreditType.Text = "Starting Credits";
        }
        else if (objOrder.CreditUsed == "Anniversary")
        {
            lblCreditType.Text = "Anniversary Credits";
        }
        else
        {
            lblCreditType.Text = "No Credit Type";
        }
        if (objOrder.CreditAmt.ToString() != "0" || objOrder.CreditAmt != null)
        {
            trstartingCredit.Visible = true;
            lblStCrAmntApplied.Text = objOrder.CreditAmt.ToString();
        }
        //lblSalesTax.Text = objOrder.SalesTax.ToString();
        if (objOrder.PaymentOption != null)
        {
            lblPaymentMethodUsed.Text = lblPaymentMethod.Text + " Amount :";
            lblPaymentMethodAmount.Text = ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt)).ToString();
        }
        else
        {
            trpaymentmethod.Visible = false;
        }
        lblOrderTotal.Text = (Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)).ToString();

        //Condition Added on 4-Mar-2011
        if (objOrder.CreditUsed == "Previous")
        {
            lblStrCrAmntAppliedLabel.Text = "Starting Credits" + " Applied :";
        }
        else if (objOrder.CreditUsed == "Anniversary")
        {
            lblStrCrAmntAppliedLabel.Text = "Anniversary Credits" + " Applied :";
        }
        else
        {
            lblStrCrAmntAppliedLabel.Text = "No Credit Type Applied";
        }
    }

    protected void BindBillingAddress()
    {
        lblBAddress.Text = objBillingInfo.Address;
        lblBCity.Text = objCity.GetById((long)objBillingInfo.CityID).sCityName;
        lblBCompany.Text = objBillingInfo.CompanyName;
        lblBCountry.Text = objCountry.GetById((long)objBillingInfo.CountryID).sCountryName;
        lblBEmail.Text = objBillingInfo.Email;
        lblBName.Text = objBillingInfo.Name;
        lblBPhone.Text = objBillingInfo.Telephone;
        lblBState.Text = objState.GetById((long)objBillingInfo.StateID).sStatename;
        lblBZip.Text = objBillingInfo.ZipCode;
    }
    ///// <summary>
    ///// Binds the Billing Address of the Company
    ///// </summary>
    //protected void BindCmpBillingAddress()
    //{
    //    lblBName.Text = objUniIssPolItem.FirstName + " " + objUniIssPolItem.LastName;
    //    lblBCompany.Text = objCmpRepo.GetById((long)objUniIssPolItem.CompanyId).CompanyName;
    //    lblBAddress.Text = objUniIssPolItem.Address1 + " " + objUniIssPolItem.Address2;
    //    lblBCountry.Text = objCountry.GetById((long)objUniIssPolItem.CountryId).sCountryName;
    //    lblBState.Text = objState.GetById((long)objUniIssPolItem.StateId).sStatename;
    //    lblBCity.Text = objCity.GetById((long)objUniIssPolItem.CityId).sCityName;
    //    lblBZip.Text = objUniIssPolItem.ZipCode;
    //    lblBPhone.Text = objUniIssPolItem.Telephone;
    //    lblBEmail.Text = objUniIssPolItem.Email;
    //}
    ///// <summary>
    ///// Binds the Shipping Address Selected by User in Checkout Page
    ///// </summary>
    protected void BindShippingAddress()
    {
        lblSAddress.Text = objShippingInfo.Address;
        lblSAddress2.Text = objShippingInfo.Address2;
        lblSStreet.Text = objShippingInfo.Street;
        lblSCity.Text = objCity.GetById((long)objShippingInfo.CityID).sCityName;
        lblSCompany.Text = objShippingInfo.CompanyName;
        lblSCountry.Text = objCountry.GetById((long)objShippingInfo.CountryID).sCountryName;
        lblSEmail.Text = objShippingInfo.Email;
        lblSName.Text = objShippingInfo.Name;
        lblSPhone.Text = objShippingInfo.Telephone;
        lblSState.Text = objState.GetById((long)objShippingInfo.StateID).sStatename;
        lblSZip.Text = objShippingInfo.ZipCode;
    }
    //#endregion

    //#region Events

    ///// <summary>
    ///// Binds the detail of Products selected by user
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    int GapBentweenBulkOrder = 0;
    protected void rptMyShoppingCart_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Add Nagmani

                Label lblProductDescription = (Label)e.Item.FindControl("lblProductDescription");
                HiddenField hdnProductDescrption = (HiddenField)e.Item.FindControl("hdnProductDescrption");
                HiddenField hdnItemNumber = (HiddenField)e.Item.FindControl("hdnItemNumber");
                HiddenField hdnIsBulkOrder = (HiddenField)e.Item.FindControl("hdnIsBulkOrder");
                HiddenField hdnStoreProductid = (HiddenField)e.Item.FindControl("hdnStoreProductid");
                hdnBulkOrdernew.Value = hdnIsBulkOrder.Value;
                hdnStroreproductnewChange.Value = hdnStoreProductid.Value;
                Panel pnlNameBars = (Panel)e.Item.FindControl("pnlNameBars");
                if (!String.IsNullOrEmpty(Convert.ToString((((Incentex.DAL.SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved))) && Convert.ToString((((Incentex.DAL.SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved.Trim())) != "")
                {

                    if (Convert.ToString(((SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved) != null && Convert.ToString(((SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved) != string.Empty)
                    {
                        pnlNameBars.Visible = true;
                        if (((SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved.Contains(','))
                        {
                            //((Label)e.Item.FindControl("lblNameTobeEngraved")).Text = ((SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved;
                            string[] EmployeeTitle = ((SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved.Split(',');
                            ((Label)e.Item.FindControl("lblNameTobeEngraved")).Text = EmployeeTitle[0];
                            ((Label)e.Item.FindControl("lblEmplTitle")).Text = EmployeeTitle[1];
                        }
                        else
                        {
                            ((Label)e.Item.FindControl("lblEmplTitleView")).Visible = false;
                            ((Label)e.Item.FindControl("lblEmplTitle")).Visible = false;

                            ((Label)e.Item.FindControl("lblNameTobeEngraved")).Text = ((SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved;

                        }
                    }
                    else
                        pnlNameBars.Visible = false;
                }

                //tailarin options
                if (obj[e.Item.ItemIndex].TailoringLength != "")
                    ((Panel)e.Item.FindControl("pnlTailoring")).Visible = true;
                else
                    ((Panel)e.Item.FindControl("pnlTailoring")).Visible = false;

                //Add Nagmani
                if (hdnIsBulkOrder.Value == "True")
                {
                    lblProductDescription.Text = hdnItemNumber.Value;
                    pnlNameBars.Visible = false;// Visible false namebars for bulk order

                    // Visible false tailaring oprions for bulk order
                    ((Panel)e.Item.FindControl("pnlTailoring")).Visible = false;
                    ((Label)e.Item.FindControl("lblRunCharge")).Text = "";
                }
                else
                {
                    lblProductDescription.Text = hdnProductDescrption.Value;
                }


                if (((HiddenField)e.Item.FindControl("hdnMasterItemNo")).Value != null && hdnStroreproductnewChange.Value != null)
                {
                    //Add Nagmani check Bulkorder

                    if (hdnBulkOrdernew.Value == "True")
                    {
                        if (storeProductId != Convert.ToInt32(hdnStroreproductnewChange.Value))//
                        {
                            List<StoreProductImage> objImageList = objStoreProductImageRepository.GetStoreProductImagesdById(Convert.ToInt32(hdnStroreproductnewChange.Value), Convert.ToInt32(((HiddenField)e.Item.FindControl("hdnMasterItemNo")).Value));

                            if (objImageList.Count > 0)
                            {
                                ((HtmlImage)e.Item.FindControl("imgSplashImage")).Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value + "&_twidth=145&_theight=198";
                                ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/" + ((HiddenField)e.Item.FindControl("hdnlargerimagename")).Value;
                                storeProductId = Convert.ToInt32(hdnStroreproductnewChange.Value);
                                GapBentweenBulkOrder = 1;
                            }
                        }
                        else //New Add nagmani
                        {
                            if (GapBentweenBulkOrder == 1)
                            {
                                GapBentweenBulkOrder = 0;
                                ((HtmlTable)e.Item.FindControl("tblBulkOrderItem")).Style.Add("margin-top", "-152px");
                            }
                            ((HtmlControl)e.Item.FindControl("prettyphotoDiv")).Visible = false;
                            ((HtmlGenericControl)e.Item.FindControl("dvBulkOrderHeader")).Visible = false;
                        }
                    }
                    else
                    {
                        List<StoreProductImage> objImageList = objStoreProductImageRepository.GetStoreProductImagesdById(Convert.ToInt32(hdnStroreproductnewChange.Value), Convert.ToInt32(((HiddenField)e.Item.FindControl("hdnMasterItemNo")).Value));
                        if (objImageList.Count > 0)
                        {
                            ((HtmlImage)e.Item.FindControl("imgSplashImage")).Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value + "&_twidth=145&_theight=198";
                            ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/" + ((HiddenField)e.Item.FindControl("hdnlargerimagename")).Value;

                        }
                    }
                }



            }
            //Calculate();


            // Used For each loop instead of e.itemtype loop
            foreach (RepeaterItem item in rptMyShoppingCart.Items)
            {

                //Get Total Amount
                Label lblUnitPrice = (Label)item.FindControl("lblUnitPrice");
                Label lblQuantity = (Label)item.FindControl("lblQuantity");
                Label lblTotal = (Label)item.FindControl("lblTotal");
                Label lblTotalPrice = (Label)rptMyShoppingCart.Controls[rptMyShoppingCart.Controls.Count - 1].Controls[0].FindControl("lblTotalProductPrice");
                if (lblQuantity.Text != "" || lblUnitPrice.Text != "")
                {
                    lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                }

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void rptMyIssuanceCart_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlTableCell pnlNameBars = new HtmlTableCell();
                HtmlTableCell pnlEmpTitle = new HtmlTableCell();
                pnlNameBars = (HtmlTableCell)e.Item.FindControl("pnlNameBarsForIssuance");

                pnlEmpTitle = (HtmlTableCell)e.Item.FindControl("pnlEmpTitleForIssuance");
                if (Convert.ToString(((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).NameToBeEngraved) != null && Convert.ToString(((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).NameToBeEngraved) != string.Empty)
                {
                    pnlNameBars.Visible = true;
                    pnlEmpTitle.Visible = true;
                    if (((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).NameToBeEngraved.Contains(','))
                    {
                        string[] EmployeeTitle = ((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).NameToBeEngraved.Split(',');
                        ((Label)e.Item.FindControl("lblNameTobeEngravedForIssuance")).Text = EmployeeTitle[0];
                        ((Label)e.Item.FindControl("lblEmplTitleForIssuance")).Text = EmployeeTitle[1];
                    }
                    else
                    {
                        ((Label)e.Item.FindControl("lblNameTobeEngravedForIssuance")).Text = ((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).NameToBeEngraved;
                    }
                }
                else
                {
                    pnlNameBars.Visible = false;
                    pnlEmpTitle.Visible = false;
                }

                //
                Label lblIssuancbackorder = (Label)e.Item.FindControl("lblIssuanceBackorder");
                HiddenField hdnbackorder = (HiddenField)e.Item.FindControl("hdnIsbackorderStatus");
                Panel pnlTailoring = (Panel)e.Item.FindControl("pnlIssuanceTailoring");
                Label lbllblTailoring = (Label)e.Item.FindControl("lblTailoringLength");
                if (hdnbackorder.Value != null)
                {
                    if (hdnbackorder.Value == "Y")
                    {
                        lblIssuancbackorder.Text = "Allow for Back Order";
                    }
                    else
                    {
                        lblIssuancbackorder.Text = "";
                    }
                }
                if (lbllblTailoring != null)
                {
                    if (lbllblTailoring.Text != string.Empty)
                    {
                        pnlTailoring.Visible = true;
                    }
                    else
                    {
                        pnlTailoring.Visible = false;
                    }
                }

                Label lblUnitPrice = (Label)e.Item.FindControl("lblUnitPriceIssuance");
                Label lblQuantity = (Label)e.Item.FindControl("lblQuantityIssuance");
                Label lblTotal = (Label)e.Item.FindControl("lblTotalIssuance");
                Label lblTotalPrice = (Label)e.Item.FindControl("lblTotalProductPrice");
                if (lblQuantity.Text != "" || lblUnitPrice.Text != "")
                {
                    lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

    }

}
