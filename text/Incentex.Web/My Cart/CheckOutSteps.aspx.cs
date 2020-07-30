//created by mayur for new design apply and some code modification
//created date : 7-feb-2012
//Magaya integration done by saurabh on 14-march-2012
//bulk order done by mayur on 30-march-2012
//Sales Tax done by mayur

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using com.paypal.sdk.profiles;
using com.paypal.sdk.services;
using com.strikeiron.ws;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using MagayaService;

public partial class My_Cart_CheckOutSteps : PageBase
{
    #region Data Members

    CountryDA objCountry = new CountryDA();
    StateDA objState = new StateDA();
    CityDA objCity = new CityDA();
    Common objcomm = new Common();
    LookupRepository objLookupRepository = new LookupRepository();
    CompanyRepository objCompanyRepository = new CompanyRepository();
    CompanyStoreRepository objCompanyStoreRepository = new CompanyStoreRepository();
    CompanyEmployeeContactInfoRepository objCompanyEmployeeContactInfoRepository = new CompanyEmployeeContactInfoRepository();
    AnniversaryProgramRepository objAnniversaryProgramRepository = new AnniversaryProgramRepository();
    UserInformationRepository objUserInformationRepository = new UserInformationRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    UniformIssuancePolicyRepository objUniRepos = new UniformIssuancePolicyRepository();
    UniformIssuancePolicyItemRepository objUniformIssuancePolicyItemRepository = new UniformIssuancePolicyItemRepository();
    StoreProductImageRepository objStoreProductImageRepository = new StoreProductImageRepository();
    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    MyIssuanceCartRepository objMyIssuanceCartRepository = new MyIssuanceCartRepository();
    OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
    SupplierRepository objSupplierRepository = new SupplierRepository();
    ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
    List<SelectMyShoppingCartProductResult> objSelectMyShoppingCartProductResultList;
    MarketingToolRepository objMarketingToolRepository = new MarketingToolRepository();//Promotion Code

    Int32 GapBentweenBulkOrder = 0;

    PaymentInfo objPaymentInfo { get; set; }

    String ShowPayment
    {
        get
        {
            return Convert.ToString(ViewState["ShowPayment"]);
        }
        set
        {
            ViewState["ShowPayment"] = value;
        }
    }

    String sProcess
    {
        get
        {
            return Convert.ToString(ViewState["sProcess"]);
        }
        set
        {
            ViewState["sProcess"] = value;
        }
    }

    Decimal? ShippingAmount
    {
        get
        {
            return Convert.ToDecimal(ViewState["ShippingAmount"]);
        }
        set
        {
            ViewState["ShippingAmount"] = value;
        }
    }

    Decimal SalesTaxAmount
    {
        get
        {
            return Convert.ToDecimal(ViewState["SalesTaxAmount"]);
        }
        set
        {
            ViewState["SalesTaxAmount"] = value;
        }
    }

    Decimal StrikeIronTaxRate
    {
        get
        {
            return Convert.ToDecimal(ViewState["StrikeIronTaxRate"]);
        }
        set
        {
            ViewState["StrikeIronTaxRate"] = value;
        }
    }

    Boolean isAnniversaryCreditApply
    {
        get
        {
            return Convert.ToBoolean(ViewState["isAnniversaryCreditApply"]);
        }
        set
        {
            ViewState["isAnniversaryCreditApply"] = value;
        }
    }

    Int32 WTDCSupplierID
    {
        get
        {
            return Convert.ToInt32(ViewState["WTDCSupplierID"]);
        }
        set
        {
            ViewState["WTDCSupplierID"] = value;
        }
    }

    Boolean isFreeShipping
    {
        get
        {
            return Convert.ToBoolean(ViewState["isFreeShipping"]);
        }
        set
        {
            ViewState["isFreeShipping"] = value;
        }
    }

    Int64 PriceLevelID
    {
        get
        {
            return Convert.ToInt64(ViewState["PriceLevelID"]);
        }
        set
        {
            ViewState["PriceLevelID"] = value;
        }
    }

    String PromotionCode
    {
        get
        {
            return Convert.ToString(ViewState["PromotionCode"]);
        }
        set
        {
            ViewState["PromotionCode"] = value;
        }
    }

    Int64? StoreProductId
    {
        get
        {
            return Convert.ToInt32(ViewState["StoreProductId"]);
        }
        set
        {
            ViewState["StoreProductId"] = value;
        }
    }

    String County
    {
        get
        {
            return Convert.ToString(ViewState["County"]);
        }
        set
        {
            ViewState["County"] = value;
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

    Int64 StoreID
    {
        get
        {
            return Convert.ToInt64(ViewState["StoreID"]);
        }
        set
        {
            ViewState["StoreID"] = value;
        }
    }

    Boolean IsCoseCenterCode
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsCoseCenterCode"]);
        }
        set
        {
            ViewState["IsCoseCenterCode"] = value;
        }
    }

    String StrikeIronResponseFileName
    {
        get
        {
            return Convert.ToString(ViewState["StrikeIronResponseFileName"]);
        }
        set
        {
            ViewState["StrikeIronResponseFileName"] = value;
        }
    }

    String ShipToZipCodePassedToStrikeIron
    {
        get
        {
            return Convert.ToString(ViewState["ShipToZipCodePassedToStrikeIron"]);
        }
        set
        {
            ViewState["ShipToZipCodePassedToStrikeIron"] = value;
        }
    }

    Boolean StrikeIronResponseFailed
    {
        get
        {
            return Convert.ToBoolean(ViewState["StrikeIronResponseFailed"]);
        }
        set
        {
            ViewState["StrikeIronResponseFailed"] = value;
        }
    }

    Boolean IsIssuanceShippingAddressSet
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsIssuanceShippingAddressSet"]);
        }
        set
        {
            ViewState["IsIssuanceShippingAddressSet"] = value;
        }
    }

    String PaymentOrder
    {
        get
        {
            if (ViewState["PaymentOrder"] == null)
                return null;
            else
                return Convert.ToString(ViewState["PaymentOrder"]);
        }
        set
        {
            ViewState["PaymentOrder"] = value;
        }
    }

    #endregion

    #region Event Handlers

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (Convert.ToBoolean(Session["OrderCompleted"]) || Convert.ToBoolean(Session["OrderFailed"]))
        {
            LinkButton btnLogout = (LinkButton)this.Master.FindControl("btnLogout");
            if (btnLogout != null && Request.Params["__EVENTTARGET"] != btnLogout.UniqueID)
                Response.Redirect("~/index.aspx");
        }

        if (!IsPostBack)
        {
            this.IsIssuanceShippingAddressSet = false;
            // here set flase for cart abandonment
            new Common().SetIsCartAbandonment(false);
            ((Label)Master.FindControl("lblPageHeading")).Text = "CheckOut";

            if (Request.QueryString["prog"] != null && Request.QueryString["prog"] == "ui")
                this.sProcess = Request.QueryString["prog"].ToString();
            else
                this.sProcess = "sc";

            switch (this.sProcess)
            {
                // Simple Shopping
                case "sc":
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/My Cart/MyShoppinCart.aspx";
                    ((HtmlImage)Master.FindControl("imgShoppingCart")).Visible = true;
                    lnkbtnReviewOrderCancelOrder.PostBackUrl = "~/My Cart/MyShoppinCart.aspx";
                    lnkbtnReviewOrderMyShoppingCart.PostBackUrl = "~/My Cart/MyShoppinCart.aspx";
                    lnkbtnReviewOrderMyShoppingCart.Visible = true;
                    break;

                // Uniform Issuance
                case "ui":
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/MyIssuancePolicy.aspx";
                    lnkbtnReviewOrderCancelOrder.PostBackUrl = "~/MyAccount/MyIssuancePolicy.aspx";
                    lnkbtnReviewOrderMyIssuanceCart.PostBackUrl = "~/MyAccount/MyIssuancePolicy.aspx";
                    lnkbtnReviewOrderMyIssuanceCart.Visible = true;
                    break;

                // Nothing
                default:
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
                    break;
            }

            FillDepartment();
            FillBCountry();
            FillSCountry();
            FillPaymentOption();
            FillCodtCenterCode();
            FillShippingLocations();
            FillOrderFor();
            FillYear();
            GetCartIDs();
            BindRepeaterForItem();

            if (Session["PID"] != null)
            {
                UniformIssuancePolicy objUniformIssuancePolicy = new UniformIssuancePolicy();
                objUniformIssuancePolicy = objUniRepos.GetById(Convert.ToInt64(Session["PID"]));
                if (objUniformIssuancePolicy != null)
                {
                    txtReferenceName.Text = objUniformIssuancePolicy.IssuanceProgramName;
                    txtReferenceName.Enabled = false;
                    ShowPayment = objUniformIssuancePolicy.ShowPayment.ToString();
                }
            }

            if (IncentexGlobal.IsIEFromStoreTestMode)
                txtReferenceName.Text = "TEST ORDER : ";

            DisplayData();
            GetShippingAmount();
            CalculateOrderItemDetail();
            GetCreditAmount();

            Int32 paymentoption = (Int32)(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.CompanyPays);
            if (Session["PaymentOption"] != null && paymentoption == Convert.ToInt32(Session["PaymentOption"]))
            {
                liPaymentOptions.Visible = false;
                divPaymentOptionCompany.Visible = true;
                lnkbtnMainProcessOrderNow.Visible = false;
                lnkbtnMainProcessOrderNowForCompanyPay.Visible = true;
                lblBalanceDueTip.Visible = false;
                lblPayByCreditCard.Visible = false;
                tdBalanceDue.Visible = false;
                divNotDisplayissuanceCart.Visible = false;
            }
            else if (Session["PaymentOption"] != null && (Int32)(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.MOAS) == Convert.ToInt32(Session["PaymentOption"]))
            {
                liPaymentOptions.Visible = false;
                divPaymentOptionCompany.Visible = true;
                lblBalanceDueTip.Visible = false;
                lblPayByCreditCard.Visible = false;//true
                tdBalanceDue.Visible = false;//true
                divNotDisplayissuanceCart.Visible = false;//true
                pnlMOASPayment.Visible = true;
            }
            else
            {
                liPaymentOptions.Visible = true;
                divPaymentOptionCompany.Visible = false;
                lblBalanceDueTip.Visible = true;
                lblPayByCreditCard.Visible = true;
                tdBalanceDue.Visible = true;
                divNotDisplayissuanceCart.Visible = true;
            }
            this.PromotionCode = null;

            if (IncentexGlobal.CurrentMember.CountryId == 223)
                hfIsInternationalUser.Value = "false";
            else
                hfIsInternationalUser.Value = "true";
        }
    }

    #region Repeater Events

    protected void rptMyShoppingCart_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Panel pnlNameBars = (Panel)e.Item.FindControl("pnlNameBars");
                if (!String.IsNullOrEmpty(((SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved) && Convert.ToString(((SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved.Trim()) != "")
                {
                    pnlNameBars.Visible = true;
                    if (((SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved.Contains(','))
                    {
                        String[] EmployeeTitle = ((SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved.Split(',');
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

                HiddenField hdnShowInventory = (HiddenField)e.Item.FindControl("hdnInventoryLevelShow");
                INC_Lookup objLook = new INC_Lookup();
                LookupRepository objLookupRepo = new LookupRepository();
                objLook = objLookupRepo.GetById(Convert.ToInt64(hdnShowInventory.Value));
                if (objLook.sLookupName == "No")
                {
                    //((HtmlTableCell)e.Item.FindControl("inventory")).Visible = false;
                    ((Label)e.Item.FindControl("InventoryData")).Text = "-";
                }
                //Show Tailoring Status
                HiddenField hdnTailoringOption = (HiddenField)e.Item.FindControl("hdnTailoringOption");
                INC_Lookup objLook1 = objLookupRepository.GetById(Convert.ToInt64(hdnTailoringOption.Value));
                HiddenField hdnRunCharge = (HiddenField)e.Item.FindControl("hdnRunCharge");
                Panel pnlTailoring = (Panel)e.Item.FindControl("pnlTailoring");
                HiddenField hdnTailoringLength = (HiddenField)e.Item.FindControl("hdnTailoringLength");
                if (objLook1.sLookupName.ToLower() == "active" && hdnTailoringLength.Value != "")
                {
                    pnlTailoring.Visible = true;
                    ((Label)e.Item.FindControl("lblRunChargeView")).Visible = true;
                    ((Label)e.Item.FindControl("lblRunCharge")).Text = Convert.ToDecimal(hdnRunCharge.Value).ToString("#,##0.00"); ;
                }
                else
                {
                    pnlTailoring.Visible = false;
                    ((Label)e.Item.FindControl("lblRunChargeView")).Visible = false;
                    ((Label)e.Item.FindControl("lblRunCharge")).Text = "";
                }

                Label lblProductDescription = (Label)e.Item.FindControl("lblProductDescription");
                HiddenField hdnItemNumber = (HiddenField)e.Item.FindControl("hdnItemNumber");
                HiddenField hdnIsBulkOrder = (HiddenField)e.Item.FindControl("hdnIsBulkOrder");
                HiddenField hdnStoreProductid = (HiddenField)e.Item.FindControl("hdnStoreProductid");
                if (hdnIsBulkOrder.Value == "True")
                {
                    lblProductDescription.Text = hdnItemNumber.Value;

                    // Visible false tailaring oprions for bulk order
                    pnlTailoring.Visible = false;
                    ((Label)e.Item.FindControl("lblRunChargeView")).Visible = false;
                    ((Label)e.Item.FindControl("lblRunCharge")).Text = "";

                    if (StoreProductId != Convert.ToInt32(hdnStoreProductid.Value))//
                    {
                        List<StoreProductImage> objImageList = objStoreProductImageRepository.GetStoreProductImagesByIdForShoppingCart(Convert.ToInt32(hdnStoreProductid.Value), Convert.ToInt32(((HiddenField)e.Item.FindControl("hdnMasterItemNo")).Value));

                        if (objImageList.Count > 0)
                        {
                            ((HtmlImage)e.Item.FindControl("imgSplashImage")).Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value + "&_twidth=145&_theight=198";
                            ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/" + ((HiddenField)e.Item.FindControl("hdnlargerimagename")).Value;
                            StoreProductId = Convert.ToInt32(hdnStoreProductid.Value);
                            GapBentweenBulkOrder = 1;
                        }
                    }
                    else //New Add nagmani
                    {
                        if (GapBentweenBulkOrder == 1)
                        {
                            GapBentweenBulkOrder = 0;
                            ((HtmlTable)e.Item.FindControl("tblOrderItem")).Style.Add("margin-top", "-160px");
                        }
                        else
                            ((HtmlTable)e.Item.FindControl("tblOrderItem")).Style.Add("margin-top", "-30px");
                        ((HtmlControl)e.Item.FindControl("prettyphotoDiv")).Visible = false;
                        ((HtmlGenericControl)e.Item.FindControl("dvOrderItemHeader")).Visible = false;
                    }
                }
                else
                {
                    List<StoreProductImage> objImageList = objStoreProductImageRepository.GetStoreProductImagesByIdForShoppingCart(Convert.ToInt32(hdnStoreProductid.Value), Convert.ToInt32(((HiddenField)e.Item.FindControl("hdnMasterItemNo")).Value));
                    if (objImageList.Count > 0)
                    {
                        ((HtmlImage)e.Item.FindControl("imgSplashImage")).Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value + "&_twidth=145&_theight=198";
                        ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/" + ((HiddenField)e.Item.FindControl("hdnlargerimagename")).Value;
                    }
                }
            }

            // Add by Shehzad 18-jan-2011
            // Used For each loop instead of e.itemtype loop
            foreach (RepeaterItem item in rptMyShoppingCart.Items)
            {
                HiddenField hdnAnniversary = (HiddenField)(item.FindControl("hdnAnniversary"));
                HiddenField hdnCreditMessages = (HiddenField)(item.FindControl("hdnCreditmessage"));
                Label lblAnniversary = (Label)(item.FindControl("lblAnniversary"));
                Panel pnlpnlAnniversaryCE = (Panel)(item.FindControl("pnlAnniversaryCE"));
                if (hdnAnniversary.Value == "No")
                {

                    if (hdnCreditMessages.Value == "Show - Not Eligible for Credit Use")
                    {
                        pnlpnlAnniversaryCE.Visible = true;
                        lblAnniversary.Text = "Not Eligible For Credit Use";
                    }
                    else
                    {
                        pnlpnlAnniversaryCE.Visible = false;
                        lblAnniversary.Text = "";
                    }

                }
                else
                {
                    if (hdnCreditMessages.Value == "Show - Not Eligible for Credit Use")
                    {
                        pnlpnlAnniversaryCE.Visible = true;
                        lblAnniversary.Text = "Not Eligible for Credit Use";
                    }
                    else
                    {
                        pnlpnlAnniversaryCE.Visible = false;
                        lblAnniversary.Text = "";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void rptMyIssuanceCart_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Added on 5-May-2011
                HtmlTableCell pnlInventoryToArrive = (HtmlTableCell)e.Item.FindControl("pnlInventoryDateForIssuance");

                if (Convert.ToInt32(((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).StoreProductID) < Convert.ToInt32(((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).Qty))
                {
                    CheckProductInventoryResult date = objOrderConfirmationRepository.GetProductInventoryToArriveOnDate(((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).StoreProductID, Convert.ToInt32(((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).Qty));
                    if (date.InventoryDate != "---")
                    {
                        String strDAteArrive = "1900-01-01";
                        if (Convert.ToDateTime(date.InventoryDate).ToShortDateString() != Convert.ToDateTime(strDAteArrive).ToShortDateString())
                        {
                            ((Label)e.Item.FindControl("InventoryToArriveOnDateForIssuance")).Text = Convert.ToDateTime(date.InventoryDate).ToString("MM/dd/yyyy");
                            pnlInventoryToArrive.Visible = true;
                        }
                        else
                            pnlInventoryToArrive.Visible = false;
                    }
                    else
                        pnlInventoryToArrive.Visible = false;
                    //End
                }
                else
                    pnlInventoryToArrive.Visible = false;


                HtmlTableCell pnlNameBars = new HtmlTableCell();
                HtmlTableCell pnlEmpTitle = new HtmlTableCell();
                pnlNameBars = (HtmlTableCell)e.Item.FindControl("pnlNameBarsForIssuance");

                pnlEmpTitle = (HtmlTableCell)e.Item.FindControl("pnlEmpTitleForIssuance");
                if (Convert.ToString(((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).NameToBeEngraved) != null && Convert.ToString(((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).NameToBeEngraved) != String.Empty)
                {
                    pnlNameBars.Visible = true;
                    pnlEmpTitle.Visible = true;
                    if (((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).NameToBeEngraved.Contains(','))
                    {
                        String[] EmployeeTitle = ((SelectMyIssuanceProductItemsResult)(e.Item.DataItem)).NameToBeEngraved.Split(',');
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
                        // lblIssuancbackorder.Text = "Allow for Back Order";
                    }
                    else
                    {
                        lblIssuancbackorder.Text = "";
                    }
                }

                if (lbllblTailoring.Text != String.Empty)
                    pnlTailoring.Visible = true;
                else
                    pnlTailoring.Visible = false;

                Label lblUnitPrice = (Label)e.Item.FindControl("lblUnitPrice");
                Label lblTotal = (Label)e.Item.FindControl("lblTotal");
                Label lblTotalview = (Label)e.Item.FindControl("lblTotalview");

                Label lblnitPrice = (Label)e.Item.FindControl("lblnitPrice");
                UniformIssuancePolicy objPolicy = new UniformIssuancePolicy();

                INC_Lookup objLook = new INC_Lookup();
                objPolicy = objUniRepos.GetById(Convert.ToInt64(Session["PID"].ToString()));
                if (objPolicy != null)
                {
                    objLook = objLookupRepository.GetById(Convert.ToInt64(objPolicy.PRICEFOR));

                    if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Both")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            lblUnitPrice.Visible = true;
                            lblTotal.Visible = true;
                            lblTotalview.Visible = true;
                            lblnitPrice.Visible = true;
                        }
                        else
                        {
                            lblUnitPrice.Visible = false;
                            lblTotal.Visible = false;
                            lblTotalview.Visible = false;
                            lblnitPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Employee")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                            lblUnitPrice.Visible = true;
                            lblTotal.Visible = true;
                            lblTotalview.Visible = true;
                            lblnitPrice.Visible = true;
                        }
                        else
                        {
                            lblUnitPrice.Visible = false;
                            lblTotal.Visible = false;
                            lblTotalview.Visible = false;
                            lblnitPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Admin")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            lblUnitPrice.Visible = true;
                            lblTotal.Visible = true;
                            lblTotalview.Visible = true;
                            lblnitPrice.Visible = true;
                        }
                        else
                        {
                            lblUnitPrice.Visible = false;
                            lblTotal.Visible = false;
                            lblTotalview.Visible = false;
                            lblnitPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Admin")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                            lblUnitPrice.Visible = true;
                            lblTotal.Visible = true;
                            lblTotalview.Visible = true;
                            lblnitPrice.Visible = true;
                        }
                        else
                        {
                            lblUnitPrice.Visible = false;
                            lblTotal.Visible = false;
                            lblTotalview.Visible = false;
                            lblnitPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Employee")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            lblUnitPrice.Visible = true;
                            lblTotal.Visible = true;
                            lblTotalview.Visible = true;
                            lblnitPrice.Visible = true;
                        }
                        else
                        {
                            lblUnitPrice.Visible = false;
                            lblTotal.Visible = false;
                            lblTotalview.Visible = false;
                            lblnitPrice.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Both")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                            lblUnitPrice.Visible = false;
                            lblTotal.Visible = false;
                            lblTotalview.Visible = false;
                            lblnitPrice.Visible = false;
                        }
                        else
                        {
                            lblUnitPrice.Visible = true;
                            lblTotal.Visible = true;
                            lblTotalview.Visible = true;
                            lblnitPrice.Visible = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region MultiView Events

    protected void mvCheckoutSteps_ActiveViewChanged(Object sender, EventArgs e)
    {
        if (mvCheckoutSteps.ActiveViewIndex == 1)
        {
            txtSName.Focus();

            //Updated by Prashant May 2013 ---------Starts
            //Bind Fixed Shipping Address for the Current User's Company,Workgroup,Station and Payment Option if exists
            Int64 PaymentOption = Convert.ToInt64(ddlPaymentOption.SelectedValue);
            if (this.sProcess == "ui" && Session["PaymentOption"] != null)
            {
                if (Convert.ToInt16(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.CompanyPays) == Convert.ToInt16(Session["PaymentOption"]))
                    PaymentOption = Convert.ToInt64(Incentex.DAL.Common.DAEnums.PaymentOptions.PaidByCorporate);
                else if (Convert.ToInt16(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.MOAS) == Convert.ToInt16(Session["PaymentOption"]))
                    PaymentOption = Convert.ToInt64(Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS);
            }


            GlobalShippingAddress objShippingAddress = new GlobalShippingAddress();
            GlobalShippingAddressRepository objGlobalShippingAddressRepos = new GlobalShippingAddressRepository();
            objShippingAddress = objGlobalShippingAddressRepos.GetByStoreWorkgroupStationPaymentOption(IncentexGlobal.CurrentMember.UserInfoID, this.StoreID, PaymentOption);
            if (objShippingAddress != null && (this.sProcess == "sc" || (objShippingAddress.IssuancePolicyTypeToApply != null && objShippingAddress.IssuancePolicyTypeToApply.ToLower() == "all") || (objShippingAddress.IssuancePolicyTypeToApply != null && objShippingAddress.IssuancePolicyTypeToApply.ToLower() == "domestic" && IncentexGlobal.CurrentMember.CountryId == 223)))
            {
                txtSName.Text = IncentexGlobal.CurrentMember.FirstName;
                txtSLName.Text = IncentexGlobal.CurrentMember.LastName;
                txtSCompany.Text = objShippingAddress.CompanyName;
                txtSEmail.Text = objShippingAddress.Email;
                txtSAddress.Text = objShippingAddress.ShippingAddress;
                txtSZip.Text = objShippingAddress.ZipCode;
                txtSTelephone.Text = objShippingAddress.Telephone;
                ddlSCountry.SelectedValue = Convert.ToString(objShippingAddress.CountryID);
                ddlSCountry_SelectedIndexChanged(ddlSCountry, null);
                ddlSState.SelectedValue = Convert.ToString(objShippingAddress.StateID);
                ddlSState_SelectedIndexChanged(ddlSState, null);
                ddlSCity.SelectedValue = Convert.ToString(objShippingAddress.CityID);

                //Not Enable to edit billing information
                txtSName.Attributes.Add("readonly", "readonly");
                txtSAddress.Attributes.Add("readonly", "readonly");
                txtSAddress2.Attributes.Add("readonly", "readonly");
                txtSStreet.Attributes.Add("readonly", "readonly");

                if (!String.IsNullOrEmpty(objShippingAddress.CompanyName))
                    txtSCompany.Attributes.Add("readonly", "readonly");
                else
                    txtSCompany.Attributes.Remove("readonly");

                if (!String.IsNullOrEmpty(objShippingAddress.Email))
                    txtSEmail.Attributes.Add("readonly", "readonly");
                else
                    txtSEmail.Attributes.Remove("readonly");

                txtSLName.Attributes.Add("readonly", "readonly");
                txtSZip.Attributes.Add("readonly", "readonly");

                if (!String.IsNullOrEmpty(objShippingAddress.Telephone))
                    txtSTelephone.Attributes.Add("readonly", "readonly");
                else
                    txtSTelephone.Attributes.Remove("readonly");

                ddlSCity.Enabled = false;
                ddlSState.Enabled = false;
                ddlSCountry.Enabled = false;

                //Hide the Address Change Location
                dvShippingLocation.Visible = false;
                hfIsAirportFieldRequired.Value = "true";
            }
            else
            {
                dvShippingLocation.Visible = true;
                hfIsAirportFieldRequired.Value = "false";
            }

            //Updated by Prashant May 2013 ---------Ends
        }
        else if (mvCheckoutSteps.ActiveViewIndex == 2)
        {
            #region Tax Calculation

            //tax calculation
            //Don't go by name, names can be deceiving...!!! ;)            
            if (ddlSCountry.SelectedItem.Text.ToUpper() == "UNITED STATES" || ddlSCountry.SelectedItem.Text.ToUpper() == "CANADA")
            {
                //method GetSalesTax is used, not only to calculate sales tax but also for getting the county name from the zip code...
                //method GetCreditAmount is used to diplay the calcualted taxes!!! Irony...!!!

                if (ddlSCountry.SelectedItem.Text.ToUpper() == "UNITED STATES")
                {
                    GetSalesTax(ddlSCountry.SelectedItem.Text, ddlSState.SelectedItem.Text);

                    //Taxes levied only in Florida and connecticut for The United States
                    if (ddlSState.SelectedItem.Text.ToUpper() == "FLORIDA" || ddlSState.SelectedItem.Text.ToUpper() == "CONNECTICUT")
                    {
                        GetCreditAmount();
                    }
                }
                else if (ddlSCountry.SelectedItem.Text.ToUpper() == "CANADA")
                {
                    //Taxes levied on all provinces of Canada, other than Alberta, Nunavut, Northwest Territories and Yukon Territory
                    if (ddlSState.SelectedItem.Text.ToUpper() != "ALBERTA" && ddlSState.SelectedItem.Text.ToUpper() != "NUNAVUT" && ddlSState.SelectedItem.Text.ToUpper() != "NORTHWEST TERRITORIES" && ddlSState.SelectedItem.Text.ToUpper() != "YUKON TERRITORY")
                    {
                        //GetSalesTax(ddlSCountry.SelectedItem.Text, ddlSState.SelectedItem.Text);
                        GetSalesTaxForCanada(Convert.ToInt64(ddlSState.SelectedItem.Value));
                        GetCreditAmount();
                    }
                }
            }

            lblPaymentOption.Text = ddlPaymentOption.SelectedItem.Text;

            #endregion
        }

        else if (mvCheckoutSteps.ActiveViewIndex == 3)
        {
            txtBFirstName.Focus();
        }
        else if (mvCheckoutSteps.ActiveViewIndex == 4)
        {
            #region Send email function based on condition
            if (Session["NewOrderID"] != null)
            {
                Order objOrder = new Order();
                CompanyEmployeeContactInfo objShippingInfo;
                CompanyEmployeeContactInfo objBillingInfo;
                objOrderConfirmationRepository = new OrderConfirmationRepository();
                objOrder = objOrderConfirmationRepository.GetByOrderID(Convert.ToInt64(Session["NewOrderID"]));

                //add by mayur on 22-nov-2011 for moas payment method
                //start
                String PaymentMethod = String.Empty;
                if (objOrder.PaymentOption != null)
                    PaymentMethod = new LookupRepository().GetById((Int64)objOrder.PaymentOption).sLookupName;
                //end

                //Billing
                if (objOrder.OrderFor == "ShoppingCart") // Company Pays
                {
                    objBillingInfo = objCompanyEmployeeContactInfoRepository.GetBillingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                else
                {
                    objBillingInfo = objCompanyEmployeeContactInfoRepository.GetBillingDetailIssuanceAddress((Int64)objOrder.UserId, objOrder.OrderID);
                }
                //Shipping
                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objShippingInfo = objCompanyEmployeeContactInfoRepository.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "ShoppingCart");
                }
                else
                {
                    objShippingInfo = objCompanyEmployeeContactInfoRepository.GetShippingDetailAddress((Int64)objOrder.UserId, objOrder.OrderID, Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString(), "IssuancePolicy");
                }

                //1.)Customer
                if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(objOrder.UserId), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderConfirmations)) || (IncentexGlobal.IsIEFromStoreTestMode == true && IncentexGlobal.AdminUser != null))
                    sendVerificationEmail(objOrder.OrderID, objBillingInfo, objShippingInfo);

                //add by mayur on 22-nov-2011 for MOAS payment method
                //start
                if (PaymentMethod == "MOAS" || PaymentMethod == "Replacement Uniforms")
                {
                    //3.)MOAS Manager
                    sendEmailToMOASManager(objOrder.OrderID, objOrder.OrderFor, objBillingInfo, objShippingInfo);
                }
                else
                {
                    //this for not sending an email to supplier when IE/CA comming from view storefront test mode.
                    if (!objOrder.ReferenceName.ToLower().Contains("test") && !IncentexGlobal.IsIEFromStoreTestMode)
                    {
                        //2.)IE Admin
                        sendIEEmail(objBillingInfo, objShippingInfo);

                        //3.)Supplier
                        sendEmailToSupplier(objOrder.OrderID, objShippingInfo);

                        //add by Saurabh on 27-Feb-2012 for Magaya Integration
                        //SendDataToMagaya(objOrder, objShippingInfo, objBillingInfo);
                    }
                    else
                    {
                        sendTestOrderEmailNotification(objBillingInfo, objShippingInfo);
                    }
                }
                //end

                //remove session value
                Session["NewOrderID"] = null;
                Session["PID"] = null;

                //Change the status of order to cancel when CA/IE comming from view storefront test mode and order is for issuancepolicy
                //so that user can use that policy again
                if (objOrder.OrderFor == "IssuanceCart" && IncentexGlobal.IsIEFromStoreTestMode == true)
                {
                    OrderConfirmationRepository objOrderConfirmationRepositoryForCancel = new OrderConfirmationRepository();
                    Order objOrderForCancel = objOrderConfirmationRepositoryForCancel.GetByOrderID(objOrder.OrderID);
                    objOrderForCancel.OrderStatus = "Canceled";
                    objOrderConfirmationRepositoryForCancel.SubmitChanges();
                }

                //Remove Replacement Uniforms As Payment Option
                if (PaymentMethod == "Replacement Uniforms")
                {
                    CompanyEmployeeRepository objCompEmpRepo = new CompanyEmployeeRepository();
                    CompanyEmployee objCompanyEmployee = objCompEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                    String ReplacementUniformsID = Convert.ToString(new LookupRepository().GetIdByLookupNameNLookUpCode("Replacement Uniforms", "WLS Payment Option"));
                    String PaymentOptions = objCompanyEmployee.Paymentoption;
                    objCompanyEmployee.Paymentoption = PaymentOptions.Replace(ReplacementUniformsID + ",", "").Replace("," + ReplacementUniformsID, "").Replace("," + ReplacementUniformsID + ",", "").Replace(ReplacementUniformsID, "");
                    objCompEmpRepo.SubmitChanges();
                }

                // for set purchase flag in tracking center(parth)
                GetPurchaseUpdate();
            }
            #endregion
        }
    }

    #endregion

    #region Dropdown Events

    protected void ddlShippingLocations_SelectedIndexChanged(Object sender, EventArgs e)
    {
        trSCityOther.Visible = false;
        tdBCityOtherLabel.Visible = false;
        tdBCityOtherInput.Visible = false;
        DisplayData();
    }

    protected void ddlBCountry_SelectedIndexChanged(Object sender, EventArgs e)
    {
        ddlBState.Items.Clear();
        ddlBState.Items.Add(new ListItem("-Select-", "0"));

        ddlBCity.Items.Clear();
        ddlBCity.Items.Add(new ListItem("-Select-", "0"));

        if (ddlBCountry.SelectedIndex > 0)
        {
            DataSet ds = objState.GetStateByCountryID(Convert.ToInt32(ddlBCountry.SelectedItem.Value));
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlBState.DataSource = ds;
                    ddlBState.DataValueField = "iStateID";
                    ddlBState.DataTextField = "sStateName";
                    ddlBState.DataBind();
                    ddlBState.Items.Insert(0, new ListItem("-Select-", "0"));
                }
            }
        }
    }

    protected void ddlBState_SelectedIndexChanged(Object sender, EventArgs e)
    {
        ddlBCity.Items.Clear();
        ddlBCity.Items.Add(new ListItem("-Select-", "0"));

        if (ddlBState.SelectedIndex > 0)
        {
            DataSet ds = objCity.GetCityByStateID(Convert.ToInt32(ddlBState.SelectedItem.Value));
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlBCity.DataSource = ds;
                    ddlBCity.DataValueField = "iCityID";
                    ddlBCity.DataTextField = "sCityName";
                    ddlBCity.DataBind();
                    ddlBCity.Items.Insert(0, new ListItem("-Select-", "0"));
                    ddlBCity.Items.Insert(1, new ListItem("-Other-", "-1"));
                }
            }
        }
    }

    protected void ddlBCity_SelectedIndexChanged(Object sender, EventArgs e)
    {
        if (Convert.ToInt64(ddlBCity.SelectedValue) < 0)
        {
            tdBCityOtherLabel.Visible = true;
            tdBCityOtherLabel.Visible = true;
        }
        else
        {
            tdBCityOtherLabel.Visible = false;
            tdBCityOtherInput.Visible = false;
        }
    }

    protected void ddlSCountry_SelectedIndexChanged(Object sender, EventArgs e)
    {
        ddlSState.Items.Clear();
        ddlSState.Items.Add(new ListItem("-Select-", "0"));

        ddlSCity.Items.Clear();
        ddlSCity.Items.Add(new ListItem("-Select-", "0"));

        if (ddlSCountry.SelectedIndex > 0)
        {
            DataSet ds = objState.GetStateByCountryID(Convert.ToInt32(ddlSCountry.SelectedItem.Value));
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlSState.DataSource = ds;
                    ddlSState.DataValueField = "iStateID";
                    ddlSState.DataTextField = "sStateName";
                    ddlSState.DataBind();
                    ddlSState.Items.Insert(0, new ListItem("-Select-", "0"));
                }
            }
        }
    }

    protected void ddlSState_SelectedIndexChanged(Object sender, EventArgs e)
    {
        ddlSCity.Items.Clear();
        ddlSCity.Items.Add(new ListItem("-Select-", "0"));
        if (ddlSState.SelectedIndex > 0)
        {
            DataSet ds = objCity.GetCityByStateID(Convert.ToInt32(ddlSState.SelectedItem.Value));
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlSCity.DataSource = ds;
                    ddlSCity.DataValueField = "iCityID";
                    ddlSCity.DataTextField = "sCityName";
                    ddlSCity.DataBind();
                    ddlSCity.Items.Insert(0, new ListItem("-Select-", "0"));
                    ddlSCity.Items.Insert(1, new ListItem("-Other-", "-1"));
                }
            }
        }
    }

    protected void ddlSCity_SelectedIndexChanged(Object sender, EventArgs e)
    {
        if (Convert.ToInt64(ddlSCity.SelectedValue) < 0)
            trSCityOther.Visible = true;
        else
            trSCityOther.Visible = false;
    }


    protected void ddlPaymentOption_SelectedIndexChanged(Object sender, EventArgs e)
    {
        BindTagsAsPerSelectedPaymentOption();
    }

    #endregion

    #region Link Button Events

    protected void lnkbtnReviewOrderNextStep_Click(Object sender, EventArgs e)
    {
        try
        {
            StringBuilder sbMessage = new StringBuilder();

            if (dvPaymentMethodSelection.Visible && !String.IsNullOrEmpty(ddlPaymentOption.SelectedValue) && Convert.ToInt64(ddlPaymentOption.SelectedValue) == 0)
                sbMessage.Append("<b>*</b>&ensp;Please select payment option.");

            if (sbMessage.Length > 0)
            {
                dvPaymentMessage1.Visible = true;
                lblPaymentMessage1.Text = sbMessage.ToString();
            }
            else
            {
                dvPaymentMessage1.Visible = false;
                mvCheckoutSteps.ActiveViewIndex = 1;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnMainProcessOrderNow_Click(Object sender, EventArgs e)
    {
        try
        {
            StringBuilder sbMessage = new StringBuilder();

            if (!String.IsNullOrEmpty(lblPayByCreditCard.Text.Trim()) && Convert.ToDecimal(lblPayByCreditCard.Text.Trim()) > 0)
            {
                // added by Prashant 15th march 2013--START
                if (liPaymentOptiondropdownCode.Visible && Convert.ToInt64(ddlCostCode.SelectedValue) == 0)
                    sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please select code.");
                // added by Prashant 15th march 2013--END

                // added by Gaurang 21th march 2013--START
                if (liPaymentOptionCode.Visible && String.IsNullOrEmpty(txtPaymentOptionCode.Text.Trim()))
                    sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter code.");
                // added by Gaurang 21th march 2013--END

                if (pnlCreditCardPayment.Visible)
                {
                    if (String.IsNullOrEmpty(txtCreditCardNumber.Text.Trim()))
                        sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter credit card number.");

                    if (ddlCreditCardExpirationYear.Items[0].Selected && Convert.ToInt64(ddlCreditCardExpirationMonth.SelectedValue.ToString()) < DateTime.Now.Month)
                        sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Credit Card expired");

                    if (String.IsNullOrEmpty(txtCVVNumber.Text.Trim()))
                        sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter CVV number.");

                    if (String.IsNullOrEmpty(txtCardHolderName.Text.Trim()))
                        sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter card holder name.");
                }
            }

            if (liReasonForReplacement.Visible && !String.IsNullOrEmpty(ddlReplacementReason.SelectedValue) && Convert.ToInt64(ddlReplacementReason.SelectedValue) == 0)
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please select replacement reason.");

            if (pnlMOASPayment.Visible)
            {
                sbMessage = new StringBuilder();
                if (String.IsNullOrEmpty(hfMOASApproverLevelID.Value))
                    sbMessage.Append("<b>*</b>&ensp;No Approver Found. Please contact administrator to assign approver.");
            }

            if (sbMessage.Length > 0)
            {
                dvPaymentMessage2.Visible = true;
                lblPaymentMessage2.Text = sbMessage.ToString();
            }
            else
            {
                dvPaymentMessage2.Visible = false;
                mvCheckoutSteps.ActiveViewIndex = 3;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnShippingNextStep_Click(Object sender, EventArgs e)
    {
        try
        {
            StringBuilder sbMessage = new StringBuilder();

            if (String.IsNullOrEmpty(txtSName.Text.Trim()))
                sbMessage.Append("<b>*</b>&ensp;Please enter name.");

            if (String.IsNullOrEmpty(txtSAddress.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter address.");

            if (!String.IsNullOrEmpty(ddlSCountry.SelectedValue) && Convert.ToInt64(ddlSCountry.SelectedValue) == 0)
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please select country.");

            if (!String.IsNullOrEmpty(ddlSState.SelectedValue) && Convert.ToInt64(ddlSState.SelectedValue) == 0)
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please select state.");

            if (!String.IsNullOrEmpty(ddlSCity.SelectedValue) && Convert.ToInt64(ddlSCity.SelectedValue) == 0)
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please select city.");
            else if (!String.IsNullOrEmpty(ddlSCity.SelectedValue) && Convert.ToInt64(ddlSCity.SelectedValue) < 0 && String.IsNullOrEmpty(txtSCity.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter city name.");

            if (String.IsNullOrEmpty(txtSZip.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter zip code.");
            else if (!Common.IsValidZipCode(ddlSCountry.SelectedItem.Text.Trim(), txtSZip.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter valid zip code.");

            if (String.IsNullOrEmpty(txtSTelephone.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter telephone number.");

            if (String.IsNullOrEmpty(txtSEmail.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter email.");
            else if (!Common.IsValidEmailAddress(txtSEmail.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter valid email.");

            //IncentexGlobal.CurrentMember.CountryId != 223 &&
            //updated by prashant as per client's request 7th sep 2013
            //updated by prashant as per client's request on 14th oct 2013
            if (String.IsNullOrEmpty(hfIsAirportFieldRequired.Value.Trim()) && Convert.ToBoolean(hfIsAirportFieldRequired.Value.Trim()) && String.IsNullOrEmpty(txtSAirport.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter airport.");

            if (sbMessage.Length > 0)
            {
                dvShippingMessage.Visible = true;
                lblShippingMessage.Text = sbMessage.ToString();
            }
            else
            {
                dvShippingMessage.Visible = false;
                mvCheckoutSteps.ActiveViewIndex = 2;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnBillingNextStep_Click(Object sender, EventArgs e)
    {
        Order objOrder = new Order();
        try
        {
            StringBuilder sbMessage = new StringBuilder();

            if (String.IsNullOrEmpty(txtBFirstName.Text.Trim()))
                sbMessage.Append("<b>*</b>&ensp;Please enter name.");

            if (String.IsNullOrEmpty(txtBAddressLine1.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter address.");

            if (!String.IsNullOrEmpty(ddlBCountry.SelectedValue) && Convert.ToInt64(ddlBCountry.SelectedValue) == 0)
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please select country.");

            if (!String.IsNullOrEmpty(ddlBState.SelectedValue) && Convert.ToInt64(ddlBState.SelectedValue) == 0)
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please select state.");

            if (!String.IsNullOrEmpty(ddlBCity.SelectedValue) && Convert.ToInt64(ddlBCity.SelectedValue) == 0)
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please select city.");
            else if (!String.IsNullOrEmpty(ddlBCity.SelectedValue) && Convert.ToInt64(ddlBCity.SelectedValue) < 0 && String.IsNullOrEmpty(txtBCity.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter city name.");

            if (String.IsNullOrEmpty(txtBZip.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter zip code.");
            else if (!Common.IsValidZipCode(ddlBCountry.SelectedItem.Text.Trim(), txtBZip.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter valid zip code.");

            if (String.IsNullOrEmpty(txtReferenceName.Text.Trim()))
                sbMessage.Append((sbMessage.Length > 0 ? "<br/>" : "") + "<b>*</b>&ensp;Please enter reference name.");

            if (sbMessage.Length > 0)
            {
                dvBillingMessage.Visible = true;
                lblBillingMessage.Text = sbMessage.ToString();
            }
            else
            {
                dvBillingMessage.Visible = false;

                #region Objects
                UniformIssuancePolicyItem objUniIssPolItem = new UniformIssuancePolicyItem();
                CompanyEmployee objCE = objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                CompanyEmployeeContactInfo objBillingInfo = objCompanyEmployeeContactInfoRepository.GetFirstBillingDetailByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);
                CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();

                if (ddlShippingLocations.SelectedIndex >= 0)
                    objShippingInfo = objCompanyEmployeeContactInfoRepository.GetShippingDetailById(IncentexGlobal.CurrentMember.UserInfoID, Int64.Parse(ddlShippingLocations.SelectedValue.ToString()));

                //set cartid from session
                String CartIDs = String.Empty;
                if (Session["CartIDs"] != null)
                    CartIDs = Session["CartIDs"].ToString();

                #endregion

                #region Order Number Generation

                String strOrderNumber;

                strOrderNumber = objOrderConfirmationRepository.GetMaxOrderNumber();

                // Check if OrderNumber for the workgroup is null or empty
                if (String.IsNullOrEmpty(strOrderNumber))
                    strOrderNumber = DateTime.Now.Year.ToString() + "000001"; // Generate New Order Number
                else
                {
                    // Check if new year?
                    if (strOrderNumber.Substring(0, 4) == DateTime.Now.Year.ToString())
                        strOrderNumber = (Int64.Parse(strOrderNumber) + 1).ToString(); // Same year so Increment Order Number
                    else // New year so Generate New Order Number
                        strOrderNumber = DateTime.Now.Year.ToString() + "000001";
                    //One zero Added by Devraj Gadhavi on 23/01/2012 for order number change
                }

                #endregion

                #region Set Properties

                objOrder.OrderNumber = strOrderNumber;
                objOrder.UserId = IncentexGlobal.CurrentMember.UserInfoID;

                if (IncentexGlobal.IsIEFromStoreTestMode && !txtReferenceName.Text.Trim().ToLower().Contains("test"))
                    txtReferenceName.Text = "Test Order : " + txtReferenceName.Text.Trim();

                if (!String.IsNullOrEmpty(txtReferenceName.Text) && txtReferenceName.Text.Length > 100)
                    txtReferenceName.Text = txtReferenceName.Text.Trim().Substring(0, 100);

                objOrder.ReferenceName = txtReferenceName.Text.Trim();

                objOrder.SalesTax = Decimal.Parse(lblSalesTax.Text);
                objOrder.OrderDate = DateTime.Now;
                objOrder.OrderAmount = Session["TotalAmount"] != null ? Decimal.Parse(Session["TotalAmount"].ToString()) : 0;
                //Updated by Prashant 20th June 2013, to set the default price in the MOAS Amount column
                objOrder.MOASOrderAmount = objOrder.OrderAmount;
                objOrder.MOASSalesTax = objOrder.SalesTax;
                if (!String.IsNullOrEmpty(hfMOASApproverLevelID.Value))
                    objOrder.MOASApproverLevelID = Convert.ToInt64(hfMOASApproverLevelID.Value);
                //Updated by Prashant 20th June 2013, to set the default price in the MOAS Amount column
                objOrder.ShippingAmount = Convert.ToDecimal(lblShippingAmount.Text);
                objOrder.StrikeIronTaxRate = this.StrikeIronTaxRate;
                objOrder.StrikeIronResponseFileName = this.StrikeIronResponseFileName;

                if (objBillingInfo != null)
                    objOrder.BillingInformationId = objBillingInfo.CompanyContactInfoID;

                if (objShippingInfo != null)
                    objOrder.ShippingInfromationid = objShippingInfo.CompanyContactInfoID;

                //update by prashant for Fix Shipping Address logic change May 2013
                //added the "&& lblPayByCreditCard.Text != "" && Convert.ToDecimal(lblPayByCreditCard.Text.Trim()) > 0" condition
                if (ddlPaymentOption.SelectedIndex > 0 && lblPayByCreditCard.Text != "" && Convert.ToDecimal(lblPayByCreditCard.Text.Trim()) > 0)
                    objOrder.PaymentOption = Int64.Parse(ddlPaymentOption.SelectedValue);
                else
                    objOrder.PaymentOption = null;

                if (txtPaymentOptionCode != null && txtPaymentOptionCode.Text != "" && lblPayByCreditCard.Text != "" && Convert.ToDecimal(lblPayByCreditCard.Text.Trim()) > 0)
                {
                    if (!String.IsNullOrEmpty(txtPaymentOptionCode.Text) && txtPaymentOptionCode.Text.Length > 60)
                        txtPaymentOptionCode.Text = txtPaymentOptionCode.Text.Trim().Substring(0, 60);

                    objOrder.PaymentOptionCode = txtPaymentOptionCode.Text.Trim();
                }

                /*-Chang By Gaurang 21 Mar 2013 add Cost-Center Code Dropdown instaed of textbox -*/
                if (IsCoseCenterCode && ddlCostCode.SelectedIndex > 0 && lblPayByCreditCard.Text != "" && Convert.ToDecimal(lblPayByCreditCard.Text.Trim()) > 0)
                {
                    objOrder.PaymentOptionCode = ddlCostCode.SelectedItem.Text; //txtPaymentOptionCode.Text.Trim();
                    objOrder.CostCenterCodeID = Convert.ToInt64(ddlCostCode.SelectedValue.ToString());
                }

                //update by prashant for Fix Shipping Address logic change May 2013
                if (!String.IsNullOrEmpty(ddlReplacementReason.SelectedValue) && Convert.ToInt64(ddlReplacementReason.SelectedValue) > 0)
                    objOrder.ReplacementReasonID = Convert.ToInt64(ddlReplacementReason.SelectedValue);

                objOrder.SpecialOrderInstruction = txtSpecialOrderInstruction.Text;
                objOrder.OrderStatus = null;
                objOrder.CreatedDate = null;
                objOrder.UpdatedDate = null;
                objOrder.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objOrder.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objOrder.WorkgroupId = objCE.WorkgroupID;
                objOrder.MyShoppingCartID = CartIDs;
                objOrder.StoreID = this.StoreID;

                //Get Employee Type           
                if (objCE.EmployeeTypeID != null)
                    objOrder.EmployeeTypeID = objCE.EmployeeTypeID;

                objOrder.IsPaid = false;

                if (isAnniversaryCreditApply == true)
                {
                    objOrder.CreditUsed = "Anniversary";
                    objOrder.CreditAmt = Decimal.Parse(lblAnniversaryCredit.Text) - Decimal.Parse(lblRemAnniversaryCredit.Text);
                }
                else
                {
                    objOrder.CreditUsed = null;
                    objOrder.CreditAmt = 0;
                }

                if (lblCorporateDiscount.Text != "0")//this is for checking that corporate discount is applied or not
                    objOrder.CorporateDiscount = Decimal.Parse(lblCorporateDiscount.Text.Trim());
                else
                    objOrder.CorporateDiscount = 0;

                if (this.sProcess == "sc")
                    objOrder.OrderFor = "ShoppingCart";
                else
                    objOrder.OrderFor = "IssuanceCart";

                #endregion

                #region Save Record

                objOrderConfirmationRepository.Insert(objOrder);
                objOrderConfirmationRepository.SubmitChanges();
                Session["NewOrderID"] = objOrder.OrderID;

                #endregion

                #region Update Shopping Cart

                if (this.sProcess == "sc")
                {
                    foreach (String CartID in CartIDs.Split(','))
                    {
                        if (!String.IsNullOrEmpty(CartID))
                        {
                            MyShoppinCart objShopCart = objShoppingCartRepository.GetById(Int32.Parse(CartID), IncentexGlobal.CurrentMember.UserInfoID);
                            objShopCart.IsOrdered = true;
                            objShopCart.OrderID = Convert.ToInt64(Session["NewOrderID"]);
                        }
                    }

                    objShoppingCartRepository.SubmitChanges();
                }
                else
                {
                    foreach (String CartID in CartIDs.Split(','))
                    {
                        if (!String.IsNullOrEmpty(CartID))
                        {
                            MyIssuanceCart objMyIssuanceCart = objMyIssuanceCartRepository.GetById(Int32.Parse(CartID), IncentexGlobal.CurrentMember.UserInfoID);
                            objMyIssuanceCart.OrderStatus = "Submitted";
                            objMyIssuanceCart.OrderID = Convert.ToInt64(Session["NewOrderID"]);
                        }
                    }

                    objMyIssuanceCartRepository.SubmitChanges();
                }

                #endregion

                #region EmployeeLedger

                if (objOrder.CreditAmt > 0)
                {
                    ////Added for the company employee ledger
                    EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                    EmployeeLedger objEmplLedger = new EmployeeLedger();
                    EmployeeLedger transaction = new EmployeeLedger();

                    objEmplLedger.UserInfoId = objCE.UserInfoID;
                    objEmplLedger.CompanyEmployeeId = objCE.CompanyEmployeeID;
                    objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ORCNR.ToString(); ;
                    objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.OrderConfirm.ToString();
                    objEmplLedger.TransactionAmount = objOrder.CreditAmt;
                    objEmplLedger.AmountCreditDebit = "Debit";
                    objEmplLedger.OrderNumber = objOrder.OrderNumber;
                    objEmplLedger.OrderId = Convert.ToInt64(objOrder.OrderID);

                    //When there will be records then get the last transaction
                    transaction = objLedger.GetLastTransactionByEmplID(objCE.CompanyEmployeeID);
                    if (transaction != null)
                    {
                        objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                        objEmplLedger.CurrentBalance = transaction.CurrentBalance - objEmplLedger.TransactionAmount;
                    }
                    //When first time record is added for the CE
                    else
                    {
                        objEmplLedger.PreviousBalance = 0;
                        objEmplLedger.CurrentBalance = objEmplLedger.TransactionAmount;
                    }

                    objEmplLedger.TransactionDate = System.DateTime.Now;
                    objEmplLedger.UpdateById = IncentexGlobal.CurrentMember.UserInfoID;
                    objLedger.Insert(objEmplLedger);
                    objLedger.SubmitChanges();
                    ////Starting Credits Add
                }

                #endregion

                #region Update Credits to company employee Table

                if (isAnniversaryCreditApply == true)
                {
                    if (lblAnniversaryCredit.Text != "0" || lblAnniversaryCredit.Text != "0.00")
                    {
                        //Load the Credit record
                        objCE = objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

                        if (objCE != null)
                        {
                            // Set Properties
                            objCE.CreditAmtToApplied = Decimal.Parse(lblRemAnniversaryCredit.Text);
                            objCE.CreditAmtToExpired = Decimal.Parse(lblRemAnniversaryCredit.Text);
                            objCompanyEmployeeRepository.SubmitChanges();
                        }
                    }
                }

                #endregion

                #region Set Sessions

                if (ddlPaymentOption.SelectedIndex > 0 && lblPayByCreditCard.Text != "" && Convert.ToDecimal(lblPayByCreditCard.Text.Trim()) > 0)
                {
                    if (IsCoseCenterCode && ddlCostCode.SelectedIndex > 0)  //txtPaymentOptionCode.Text != ""
                        this.PaymentOrder = ddlPaymentOption.SelectedItem.Text + " : " + ddlCostCode.SelectedItem.Text;
                    else if (!String.IsNullOrEmpty(txtPaymentOptionCode.Text))
                        this.PaymentOrder = ddlPaymentOption.SelectedItem.Text + " : " + txtPaymentOptionCode.Text;
                    else
                        this.PaymentOrder = ddlPaymentOption.SelectedItem.Text;
                }
                else
                {
                    //If nothing is selected in the payment option dropdown then set the rediction session to the "Thank You"..
                    Session["Redirection"] = "Thankyou";

                    if (isAnniversaryCreditApply == true)
                        this.PaymentOrder = "Anniversary Credit";
                    else
                        this.PaymentOrder = null;
                }

                #endregion

                #region Billing Info

                //Insert New Billing Info
                String OrderTypeBilling = null;

                if (this.sProcess == "ui")
                    OrderTypeBilling = "IssuancePolicy";
                else
                    OrderTypeBilling = "ShoppingCart";

                Int64 CityBID = 0;
                //Add new city to INC_City Table
                if (ddlBCity.SelectedValue == "-Other-")
                {
                    CityRepository objCityRep = new CityRepository();
                    Int64 countryID = Convert.ToInt64(ddlBCountry.SelectedItem.Value);
                    Int64 stateID = Convert.ToInt64(ddlBState.SelectedItem.Value);

                    INC_City objBCity = objCityRep.CheckIfExist(countryID, stateID, txtBCity.Text.Trim());
                    if (objBCity == null)
                    {
                        INC_City objBCity1 = new INC_City();
                        objBCity1.iCountryID = countryID;
                        objBCity1.iStateID = stateID;
                        objBCity1.sCityName = txtBCity.Text.Trim();
                        objCityRep.Insert(objBCity1);
                        objCityRep.SubmitChanges();
                        CityBID = objBCity1.iCityID;
                    }
                    else
                        CityBID = objBCity.iCityID;
                }
                //End
                else
                    CityBID = Convert.ToInt64(ddlBCity.SelectedValue);

                CompanyEmployeeContactInfo objBillingOrderInfo = new CompanyEmployeeContactInfo();

                objBillingOrderInfo.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                objBillingOrderInfo.BillingCO = txtBFirstName.Text.Trim();
                objBillingOrderInfo.Manager = txtBLastName.Text.Trim();
                objBillingOrderInfo.CompanyName = txtBCompany.Text.Trim();
                objBillingOrderInfo.Address = txtBAddressLine1.Text.Trim();
                objBillingOrderInfo.Address2 = txtBAddressLine2.Text.Trim();
                objBillingOrderInfo.CountryID = Convert.ToInt64(ddlBCountry.SelectedValue);
                objBillingOrderInfo.StateID = Convert.ToInt64(ddlBState.SelectedValue);
                objBillingOrderInfo.CityID = CityBID;
                objBillingOrderInfo.ZipCode = txtBZip.Text.Trim();
                objBillingOrderInfo.Telephone = String.Empty;
                objBillingOrderInfo.Email = txtBEmail.Text.Trim();
                objBillingOrderInfo.OrderType = OrderTypeBilling;
                objBillingOrderInfo.ContactInfoType = "Billing";
                objBillingOrderInfo.OrderID = Convert.ToInt64(Session["NewOrderID"]);
                //CompanyEmployeeID=null,
                objBillingOrderInfo.Name = txtBFirstName.Text.Trim() + " " + txtBLastName.Text.Trim();

                objCompanyEmployeeContactInfoRepository.Insert(objBillingOrderInfo);
                objCompanyEmployeeContactInfoRepository.SubmitChanges();

                #endregion

                #region Shipping Info

                Int64 CitySID = 0;
                //Start Add City when Other Selection form city dropdownlist"
                if (ddlSCity.SelectedValue == "-Other-")
                {
                    CityRepository objSCityRep = new CityRepository();
                    Int64 countryID = Convert.ToInt64(ddlSCountry.SelectedItem.Value);
                    Int64 stateID = Convert.ToInt64(ddlSState.SelectedItem.Value);

                    INC_City objSCity = objSCityRep.CheckIfExist(countryID, stateID, txtSCity.Text.Trim());
                    if (objSCity == null)
                    {
                        INC_City objSCity1 = new INC_City();
                        objSCity1.iCountryID = countryID;
                        objSCity1.iStateID = stateID;
                        objSCity1.sCityName = txtSCity.Text.Trim();
                        objSCityRep.Insert(objSCity1);
                        objSCityRep.SubmitChanges();
                        CitySID = objSCity1.iCityID;
                    }
                    else
                        CitySID = objSCity.iCityID;
                }
                //End
                else
                    CitySID = Convert.ToInt64(ddlSCity.SelectedValue);

                //Insert New Shipping Info
                String OrderTypeShipping = null;
                String strTitle = null;

                if (this.sProcess == "ui")
                    OrderTypeShipping = "IssuancePolicy";
                else
                    OrderTypeShipping = "ShoppingCart";

                if (ddlShippingLocations.SelectedIndex >= 0)
                    strTitle = ddlShippingLocations.SelectedValue;

                CompanyEmployeeContactInfo objShippingOrderInfo = new CompanyEmployeeContactInfo();

                objShippingOrderInfo.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                objShippingOrderInfo.Name = txtSName.Text.Trim();
                objShippingOrderInfo.CompanyName = txtSCompany.Text.Trim();
                objShippingOrderInfo.Address = txtSAddress.Text.Trim();
                objShippingOrderInfo.Address2 = txtSAddress2.Text.Trim();
                objShippingOrderInfo.Street = txtSStreet.Text.Trim();
                objShippingOrderInfo.CountryID = Convert.ToInt64(ddlSCountry.SelectedValue);
                objShippingOrderInfo.StateID = Convert.ToInt64(ddlSState.SelectedValue);
                objShippingOrderInfo.CityID = CitySID;
                objShippingOrderInfo.ZipCode = txtSZip.Text.Trim();
                objShippingOrderInfo.Telephone = txtSTelephone.Text.Trim();
                objShippingOrderInfo.Email = txtSEmail.Text.Trim();
                objShippingOrderInfo.OrderType = OrderTypeShipping;
                objShippingOrderInfo.OrderID = Convert.ToInt64(Session["NewOrderID"]);
                objShippingOrderInfo.ContactInfoType = "Shipping";
                objShippingOrderInfo.Title = strTitle;

                //Start add by mayur on 26-jan-2012
                objShippingOrderInfo.Airport = txtSAirport.Text.Trim();
                if (ddlDepartment.SelectedValue != "0")
                    objShippingOrderInfo.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
                else
                    objShippingOrderInfo.DepartmentID = null;
                //End add by mayur on 26-jan-2012

                //Start add by mayur on 07-Sept-2012
                if (ddlOrderFor.SelectedValue != "" && ddlOrderFor.SelectedValue != "0")
                    objShippingOrderInfo.OrderFor = Convert.ToInt64(ddlOrderFor.SelectedValue);
                else
                    objShippingOrderInfo.OrderFor = null;
                //End add by mayur on 07-Sept-2012

                objShippingOrderInfo.Fax = txtSLName.Text.Trim();
                objShippingOrderInfo.county = this.County;//!String.IsNullOrEmpty(txtCounty.Text.Trim()) ? txtCounty.Text.Trim() : this.County;                

                objCompanyEmployeeContactInfoRepository.Insert(objShippingOrderInfo);
                objCompanyEmployeeContactInfoRepository.SubmitChanges();

                #endregion

                #region Add To My AddressBook

                if (chkAddToMyAddressBook.Checked == true && txtAddToMyAddressBookName.Text.Trim() != "")
                {
                    CompanyEmployeeContactInfo objNewCompanyContactInfo = new CompanyEmployeeContactInfo();

                    objNewCompanyContactInfo.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                    objNewCompanyContactInfo.Name = txtSName.Text.Trim();
                    objNewCompanyContactInfo.CompanyName = txtSCompany.Text.Trim();
                    objNewCompanyContactInfo.Address = txtSAddress.Text.Trim();
                    objNewCompanyContactInfo.Address2 = txtSAddress2.Text.Trim();
                    objNewCompanyContactInfo.Street = txtSStreet.Text.Trim();
                    objNewCompanyContactInfo.CountryID = Convert.ToInt64(ddlSCountry.SelectedValue);
                    objNewCompanyContactInfo.StateID = Convert.ToInt64(ddlSState.SelectedValue);
                    objNewCompanyContactInfo.CityID = CitySID;
                    objNewCompanyContactInfo.ZipCode = txtSZip.Text.Trim();
                    objNewCompanyContactInfo.Telephone = txtSTelephone.Text.Trim();
                    objNewCompanyContactInfo.Email = txtSEmail.Text.Trim();
                    objNewCompanyContactInfo.OrderType = "MySetting";
                    objNewCompanyContactInfo.OrderID = null;
                    objNewCompanyContactInfo.ContactInfoType = "Shipping";
                    objNewCompanyContactInfo.Title = txtAddToMyAddressBookName.Text.Trim();

                    //Start add by mayur on 26-jan-2012
                    objNewCompanyContactInfo.Airport = txtSAirport.Text.Trim();
                    if (ddlDepartment.SelectedValue != "0")
                        objNewCompanyContactInfo.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
                    else
                        objNewCompanyContactInfo.DepartmentID = null;
                    //End add by mayur on 26-jan-2012

                    objNewCompanyContactInfo.Fax = txtSLName.Text.Trim();
                    objNewCompanyContactInfo.county = County;//!String.IsNullOrEmpty(txtCounty.Text.Trim()) ? txtCounty.Text.Trim() : County;

                    //
                    objCompanyEmployeeContactInfoRepository.Insert(objNewCompanyContactInfo);
                    objCompanyEmployeeContactInfoRepository.SubmitChanges();
                }

                #endregion

                String OrderNumber = OrderConfirmProcess();

                Session["OrderCompleted"] = true;

                new SAPOperations().SubmitOrderToSAP(objOrder.OrderID);

                if (this.StrikeIronResponseFailed)
                {
                    String Subject = "World-Link (" + (CommonMails.Live ? "live" : "test") + ") : Stike-Iron Response failed for order # " + OrderNumber;
                    StringBuilder Body = new StringBuilder("Strike Iron Response has failed for World-Link (" + (CommonMails.Live ? "live" : "test") + ") order # " + OrderNumber + ".");
                    Body.Append("<br/><br/>The ship to zip code entered by the user was " + this.ShipToZipCodePassedToStrikeIron + ".");
                    Body.Append("<br/><br/>Attached is the strike Iron response in XML format.");
                    Body.Append("<br/><br/>Thanks");
                    Body.Append("<br/>Wolrd-Link System");
                    Body.Append("<br/><br/><b>Note :</b> Do not reply to this email, it has been sent from an un-monitored email address.");

                    new CommonMails().SendMailWithAttachment(0, "Stike Iron Response Failed", CommonMails.EmailFrom, Convert.ToString(ConfigurationManager.AppSettings["StrikeIronFailedNotifyList"]), Subject, Body.ToString(), CommonMails.DisplyName, CommonMails.SSL, true, Path.Combine(Common.StrikeIronResponsePath, this.StrikeIronResponseFileName), CommonMails.SMTPHost, CommonMails.SMTPPort, CommonMails.UserName, CommonMails.Password, this.StrikeIronResponseFileName, CommonMails.Live);

                    this.StrikeIronResponseFailed = false;
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            Session["OrderFailed"] = true;
            Response.Redirect("OrderFailed.aspx?id=" + objOrder.OrderID + "&no=" + objOrder.OrderNumber);
        }
    }

    protected void lnkbtnApplyAnniversaryCredit_Click(Object sender, EventArgs e)
    {
        isAnniversaryCreditApply = true;
        GetCreditAmount();
        lnkbtnAnniversaryCreditsCancel.Visible = true;
        lnkbtnAnniversaryCredits.Visible = false;
    }

    protected void lnkbtnAnniversaryCreditsCancel_Click(Object sender, EventArgs e)
    {
        isAnniversaryCreditApply = false;
        GetCreditAmount();
        lnkbtnAnniversaryCreditsCancel.Visible = false;
        lnkbtnAnniversaryCredits.Visible = true;
    }

    protected void lnkbtnAddToMyAddressBookSave_Click(Object sender, EventArgs e)
    {
        mpeAddToMyAddressBook.Hide();
    }

    protected void lnkBtnSubmitPromotion_Click(Object sender, EventArgs e)
    {
        TextBox txtPromotionCode = (TextBox)rptMyShoppingCart.Controls[rptMyShoppingCart.Controls.Count - 1].Controls[0].FindControl("txtPromotionCode");
        this.PromotionCode = txtPromotionCode.Text;
        GetShippingAmount();
        CalculateOrderItemDetail();
        lblShippingAmount.Text = this.ShippingAmount.ToString();
    }

    #endregion

    #region Checkbox Events

    protected void chkBillingSameAsShipping_CheckedChanged(Object sender, EventArgs e)
    {
        if (chkBillingSameAsShipping.Checked == true)
        {
            chkBillingSameAsShipping.Checked = true;
            spnBillingSameAsShipping.Attributes.Add("class", "checkout_checkbox_checked alignleft");

            txtBFirstName.Text = txtSName.Text;
            txtBLastName.Text = txtSLName.Text;
            txtBCompany.Text = txtSCompany.Text;
            txtBAddressLine1.Text = txtSAddress.Text;
            txtBAddressLine2.Text = txtSAddress2.Text;

            ddlBCountry.SelectedValue = ddlSCountry.SelectedValue;
            ddlBCountry_SelectedIndexChanged(null, null);

            ddlBState.SelectedValue = ddlSState.SelectedValue;
            ddlBState_SelectedIndexChanged(null, null);

            ddlBCity.SelectedValue = ddlSCity.SelectedValue;
            ddlBCity_SelectedIndexChanged(null, null);
            if (ddlBCity.SelectedValue == "-Other-")
                txtBCity.Text = txtSCity.Text;
            txtBZip.Text = txtSZip.Text;
        }
        else
        {
            chkBillingSameAsShipping.Checked = false;
            spnBillingSameAsShipping.Attributes.Add("class", "checkout_checkbox alignleft");
        }
    }

    protected void chkAddToMyAddressBook_CheckedChanged(Object sender, EventArgs e)
    {
        if (chkAddToMyAddressBook.Checked == true)
        {
            chkAddToMyAddressBook.Checked = true;
            spnAddToMyAddressBook.Attributes.Add("class", "checkout_checkbox_checked alignleft");

            mpeAddToMyAddressBook.Show();
            txtAddToMyAddressBookName.Text = String.Empty;
        }
        else
        {
            chkAddToMyAddressBook.Checked = false;
            spnAddToMyAddressBook.Attributes.Add("class", "checkout_checkbox alignleft");
            txtAddToMyAddressBookName.Text = String.Empty;
        }
    }

    #endregion

    #endregion

    #region Methods

    #region Bind Dropdowns

    /// <summary>
    /// Fill Payment Option available to user
    /// </summary>
    public void FillPaymentOption()
    {
        try
        {
            List<CompanyEmployeePaymentOptionsResult> objCompanyEmployeePaymentOptionsResultList = objCompanyEmployeeRepository.GetAllPaymentsOptions(IncentexGlobal.CurrentMember.UserInfoID);

            //This is for removing moas option from payment option shopping cart when current user having no approver exists add by mayur 21-July-2012
            List<GetMOASApproverForCEResult> objlstMOASApprover = new List<GetMOASApproverForCEResult>();


            if (this.sProcess == "sc" || (this.sProcess == "ui" && Session["PaymentOption"] != null && (Int32)(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.EmployeePays) == Convert.ToInt32(Session["PaymentOption"])))
            {
                //Call the SP to get the MOAS Approver for the Current User
                objlstMOASApprover = objCompanyEmployeeRepository.GetMOASApprover(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.CompanyId, 0);
                if (objlstMOASApprover.Count == 0)
                    objCompanyEmployeePaymentOptionsResultList = objCompanyEmployeePaymentOptionsResultList.Where(x => x.sLookupName != "MOAS").ToList();
            }
            else if (this.sProcess == "ui")
            {
                //Call the SP to get the MOAS Approver for the Current User
                objlstMOASApprover = objCompanyEmployeeRepository.GetMOASApprover(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.CompanyId, Convert.ToInt64(Session["PID"].ToString()));
                if (objlstMOASApprover.Count == 0)
                    objCompanyEmployeePaymentOptionsResultList = objCompanyEmployeePaymentOptionsResultList.Where(x => x.sLookupName != "MOAS").ToList();
            }
            //End mayur 21-july-2012

            //Bind MOAS Panel Data(Approver)
            //if (objlstMOASApprover != null && objlstMOASApprover.Count > 0)
            FillMOASPanelData(objlstMOASApprover);

            ddlPaymentOption.DataSource = objCompanyEmployeePaymentOptionsResultList;
            ddlPaymentOption.DataTextField = "sLookupName";
            ddlPaymentOption.DataValueField = "iLookupID";
            ddlPaymentOption.DataBind();
            ddlPaymentOption.Items.Insert(0, new ListItem("--Select Payment Option--", "0"));
            if (IncentexGlobal.IsPlaceExchangeOrder)
            {
                if (ddlPaymentOption.Items.FindByText("Purchase Order") == null)
                {
                    String ilookupID = Convert.ToString(objLookupRepository.GetIdByLookupNameNLookUpCode("Purchase Order", "WLS Payment Option"));
                    ddlPaymentOption.Items.Insert(1, new ListItem("Purchase Order", ilookupID));
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Fills the department dropdown.
    /// </summary>
    private void FillDepartment()
    {
        this.StoreID = objCompanyStoreRepository.GetStoreIDByCompanyId((Int32)IncentexGlobal.CurrentMember.CompanyId);
        List<GetStoreDepartmentsResult> lstDepartments = objCompanyStoreRepository.GetStoreDepartments(this.StoreID).Where(le => le.Existing == 1).OrderBy(le => le.Department).ToList();
        ddlDepartment.DataSource = lstDepartments;
        ddlDepartment.DataTextField = "Department";
        ddlDepartment.DataValueField = "DepartmentID";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-select department-", "0"));

    }

    /// <summary>
    /// Fills the Title for Mutiple Shipping Addresses
    /// </summary>
    public void FillShippingLocations()
    {
        List<CompanyEmployeeContactInfo> obj = objCompanyEmployeeContactInfoRepository.GetSavedShippingAddressesForUser(IncentexGlobal.CurrentMember.UserInfoID, CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Company, Incentex.DAL.Common.DAEnums.SortOrderType.Asc);
        ddlShippingLocations.DataSource = obj;
        ddlShippingLocations.DataBind();
    }

    /// <summary>
    /// Fills the order for dropdown based on storefront setting applied to this user.
    /// </summary>
    private void FillOrderFor()
    {
        String userStoreOptions = new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).Userstoreoption;
        if (!String.IsNullOrEmpty(userStoreOptions))
        {
            String[] userstore = userStoreOptions.Split(',');
            foreach (String eachuserStore in userstore)
            {
                if (new LookupRepository().GetById(Convert.ToInt32(eachuserStore)).sLookupName.Trim() == "Required Order For DropDown")
                {
                    trOrderFor.Visible = true;
                    ddlOrderFor.DataSource = objLookupRepository.GetByLookup("OrderFor");
                    ddlOrderFor.DataValueField = "iLookupID";
                    ddlOrderFor.DataTextField = "sLookupName";
                    ddlOrderFor.DataBind();
                    ddlOrderFor.Items.Insert(0, new ListItem("-Select Order For-", "0"));
                }
            }
        }
    }

    public void FillBCountry()
    {
        try
        {
            DataSet ds = objCountry.GetCountry();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBCountry.DataSource = ds;
                ddlBCountry.DataTextField = "sCountryName";
                ddlBCountry.DataValueField = "iCountryID";
                ddlBCountry.DataBind();
                ddlBCountry.Items.Insert(0, new ListItem("-Select-", "0"));
                ddlBCountry.SelectedValue = ddlBCountry.Items.FindByText("United States").Value;

                //fill state based on country
                ddlBState.Enabled = true;
                ds = null;
                ds = objState.GetStateByCountryID(Convert.ToInt32(ddlBCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlBState.DataSource = ds;
                        ddlBState.DataValueField = "iStateID";
                        ddlBState.DataTextField = "sStateName";
                        ddlBState.DataBind();
                        ddlBState.Items.Insert(0, new ListItem("-Select-", "0"));

                        ddlBCity.Items.Clear();
                        ddlBCity.Items.Add(new ListItem("-Select-", "0"));
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        finally
        {
        }
    }

    public void FillSCountry()
    {
        try
        {
            DataSet ds = objCountry.GetCountry();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSCountry.DataSource = ds;
                ddlSCountry.DataTextField = "sCountryName";
                ddlSCountry.DataValueField = "iCountryID";
                ddlSCountry.DataBind();
                ddlSCountry.Items.Insert(0, new ListItem("-Select-", "0"));
                ddlSCountry.SelectedValue = ddlSCountry.Items.FindByText("United States").Value;

                //bind shipping state based on country selected
                ddlSState.Enabled = true;
                ds = null;
                ds = objState.GetStateByCountryID(Convert.ToInt32(ddlSCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlSState.DataSource = ds;
                        ddlSState.DataValueField = "iStateID";
                        ddlSState.DataTextField = "sStateName";
                        ddlSState.DataBind();
                        ddlSState.Items.Insert(0, new ListItem("-Select-", "0"));

                        ddlSCity.Items.Clear();
                        ddlSCity.Items.Add(new ListItem("-Select-", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        finally
        {
            objCountry = null;
        }
    }

    /// <summary>
    /// Populates the Year dropdown from current year to next 15 years
    /// </summary>
    private void FillYear()
    {
        Int32 currentYear = DateTime.Now.Year;
        Int32 futureYears = 15;

        for (Int32 i = futureYears; i >= 0; i--)
        {
            ddlCreditCardExpirationYear.Items.Add(new ListItem(currentYear.ToString(), currentYear.ToString()));
            currentYear++;
        }

        ddlCreditCardExpirationYear.Items[0].Selected = true;
    }

    /// <summary>
    /// For filling the reason for replacement dropdown
    /// </summary>
    private void FillReasonForReplacement(Boolean IsRequired)
    {
        liReasonForReplacement.Visible = IsRequired;
        if (IsRequired)
        {
            ddlReplacementReason.DataSource = new LookupRepository().GetByLookup("ReasonForReplacement");
            ddlReplacementReason.DataValueField = "iLookupID";
            ddlReplacementReason.DataTextField = "sLookupName";
            ddlReplacementReason.DataBind();
            ddlReplacementReason.Items.Insert(0, new ListItem("-Reason For Replacement-", "0"));
        }
        else
        {
            ddlReplacementReason.Items.Clear();
        }
    }

    private void FillCodtCenterCode()
    {
        StoreCostCenterCodeRepository objStorecode = new StoreCostCenterCodeRepository();
        ddlCostCode.DataSource = objStorecode.GetAllQuery(this.StoreID);
        ddlCostCode.DataValueField = "CostCenterCodeID";
        ddlCostCode.DataTextField = "Code";
        ddlCostCode.DataBind();
        ddlCostCode.Items.Insert(0, new ListItem("--select--", "0"));
    }

    #endregion

    #region Loads all the data for the given User

    private void DisplayData()
    {
        try
        {
            if (this.sProcess == "ui")
            {
                UniformIssuancePolicy objUniformIssuancePolicy = objUniRepos.GetById(Convert.ToInt64(Session["PID"]));

                if (objUniformIssuancePolicy != null)
                {
                    Session["PaymentOption"] = objUniformIssuancePolicy.PaymentOption;
                    if (objUniformIssuancePolicy.PaymentOption == 1 || objUniformIssuancePolicy.PaymentOption == 3) //this is for company pay and MOAS
                    {
                        //Hide the Payment Dropdown selection if the Issuance Policy payment method is company pay or MOAS.
                        dvPaymentMethodSelection.Visible = false;

                        //if the order is from the Issuance policy than check Its Payment Option and assign it to the dropdown.
                        //if (ddlPaymentOption.SelectedValue == "0" && Session["PaymentOption"] != null)
                        //{
                        //    if (Convert.ToString(Session["PaymentOption"]) == "3") // 3 is For MOAS
                        //    {
                        //        if (ddlPaymentOption.Items.FindByValue(Convert.ToString((long)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS)) == null)
                        //        {
                        //            ListItem objlstItem = new ListItem(Convert.ToString(Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS), Convert.ToString((long)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS));
                        //            ddlPaymentOption.Items.Add(objlstItem);
                        //        }

                        //        ddlPaymentOption.SelectedValue = Convert.ToString((long)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS);
                        //    }
                        //    else if (Convert.ToString(Session["PaymentOption"]) == "1") //1 is for Company Pay
                        //    {
                        //        if (ddlPaymentOption.Items.FindByValue(Convert.ToString((long)Incentex.DAL.Common.DAEnums.PaymentOptions.PaidByCorporate)) == null)
                        //        {
                        //            ListItem objlstItem = new ListItem(Convert.ToString(Incentex.DAL.Common.DAEnums.PaymentOptions.PaidByCorporate), Convert.ToString((long)Incentex.DAL.Common.DAEnums.PaymentOptions.PaidByCorporate));
                        //            ddlPaymentOption.Items.Add(objlstItem);
                        //        }
                        //        ddlPaymentOption.SelectedValue = Convert.ToString((long)Incentex.DAL.Common.DAEnums.PaymentOptions.PaidByCorporate);
                        //    }
                        //}



                        IssuanceCompanyAddress objIssuanceCompanyAddress = new IssuanceCompanyAddressRepository().GetByUniformIssuancePolicyId(objUniformIssuancePolicy.UniformIssuancePolicyID);
                        if (objIssuanceCompanyAddress != null)
                        {
                            this.IsIssuanceShippingAddressSet = true;
                            //billing information
                            txtBFirstName.Text = objIssuanceCompanyAddress.FirstName;
                            txtBLastName.Text = objIssuanceCompanyAddress.LastName;
                            if (!String.IsNullOrEmpty(objIssuanceCompanyAddress.BCompanyName))
                            {
                                txtBCompany.Text = objIssuanceCompanyAddress.BCompanyName;
                            }
                            else
                            {
                                CompanyEmployeeContactInfo objBillingInfo = objCompanyEmployeeContactInfoRepository.GetFirstBillingDetailByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);
                                if (objBillingInfo != null)
                                {
                                    txtBCompany.Text = objBillingInfo.CompanyName;
                                    Session["CompanyChekoutName"] = txtBCompany.Text;
                                }
                            }
                            txtBAddressLine1.Text = objIssuanceCompanyAddress.Address1;
                            txtBAddressLine2.Text = objIssuanceCompanyAddress.Address2;

                            ddlBCountry.SelectedValue = objIssuanceCompanyAddress.CountryId.ToString();
                            ddlBCountry_SelectedIndexChanged(null, null);

                            ddlBState.SelectedValue = objIssuanceCompanyAddress.StateId.ToString();
                            ddlBState_SelectedIndexChanged(null, null);

                            ddlBCity.SelectedValue = objIssuanceCompanyAddress.CityId.ToString();

                            txtBZip.Text = objIssuanceCompanyAddress.ZipCode;
                            txtBEmail.Text = objIssuanceCompanyAddress.Email;

                            //Not Enable to edit billing information
                            txtBFirstName.Enabled = false;
                            txtBAddressLine1.Enabled = false;
                            txtBAddressLine2.Enabled = false;
                            txtBCompany.Enabled = false;
                            txtBEmail.Enabled = false;
                            txtBLastName.Enabled = false;
                            txtBZip.Enabled = false;
                            ddlBCity.Enabled = false;
                            ddlBState.Enabled = false;
                            ddlBCountry.Enabled = false;
                            chkBillingSameAsShipping.Enabled = false;

                            //Fill Shipping information if show payment option is not 'N'
                            if (ShowPayment != "N")
                            {
                                if (objIssuanceCompanyAddress.SCompanyName != null)
                                    txtSCompany.Text = objIssuanceCompanyAddress.SCompanyName;

                                if (objIssuanceCompanyAddress.SAddress1 != null && objIssuanceCompanyAddress.SAddress2 != null)
                                    txtSAddress.Text = objIssuanceCompanyAddress.SAddress1 + " , " + objIssuanceCompanyAddress.SAddress2;
                                else if (objIssuanceCompanyAddress.SAddress1 != null && objIssuanceCompanyAddress.SAddress2 == null)
                                    txtSAddress.Text = objIssuanceCompanyAddress.SAddress1;
                                else if (objIssuanceCompanyAddress.SAddress1 == null && objIssuanceCompanyAddress.SAddress2 != null)
                                    txtSAddress.Text = objIssuanceCompanyAddress.SAddress2;
                                else
                                    txtSAddress.Text = "";

                                if (objIssuanceCompanyAddress.SCountryId != 0)
                                    ddlSCountry.SelectedValue = objIssuanceCompanyAddress.SCountryId.ToString();
                                ddlSCountry_SelectedIndexChanged(null, null);

                                if (objIssuanceCompanyAddress.SStateId != 0)
                                    ddlSState.SelectedValue = objIssuanceCompanyAddress.SStateId.ToString();
                                ddlSState_SelectedIndexChanged(null, null);

                                if (objIssuanceCompanyAddress.SCityId != 0)
                                    ddlSCity.SelectedValue = objIssuanceCompanyAddress.SCityId.ToString();

                                txtSZip.Text = objIssuanceCompanyAddress.SZipCode;
                                txtSTelephone.Text = objIssuanceCompanyAddress.STelephone;
                                txtSEmail.Text = objIssuanceCompanyAddress.SEmail;

                                //Enable to edit shipping information
                                txtSAddress.Enabled = false;
                                txtSAddress2.Enabled = false;
                                txtSStreet.Enabled = false;
                                txtSCompany.Enabled = false;
                                txtSEmail.Enabled = false;
                                txtSTelephone.Enabled = false;
                                txtSZip.Enabled = false;
                                //txtCounty.Enabled = true;
                                txtSAirport.Enabled = true;
                                ddlSCity.Enabled = false;
                                ddlSCountry.Enabled = false;
                                ddlSState.Enabled = false;
                                ddlShippingLocations.Enabled = false;
                                chkAddToMyAddressBook.Enabled = false;

                                if (objIssuanceCompanyAddress.FirstSName == null && objIssuanceCompanyAddress.LastSName == null && objIssuanceCompanyAddress.SAddress1 == null)
                                {
                                    GetShippingAddressBasedOnLocationDropdown();
                                }
                                else if (objIssuanceCompanyAddress.FirstSName == null && objIssuanceCompanyAddress.LastSName == null && objIssuanceCompanyAddress.SAddress1 != null)
                                {
                                    txtSCompany.Enabled = false;
                                    txtSAddress.Enabled = false;
                                    txtSAddress2.Enabled = false;
                                    txtSStreet.Enabled = false;
                                    ddlSCountry.Enabled = false;
                                    ddlSState.Enabled = false;
                                    ddlSCity.Enabled = false;
                                    txtSZip.Enabled = false;
                                    txtSTelephone.Enabled = false;
                                    txtSEmail.Enabled = false;
                                    ddlShippingLocations.Enabled = false;
                                    chkAddToMyAddressBook.Enabled = false;
                                    //txtCounty.Enabled = true;
                                    txtSAirport.Enabled = true;
                                }
                            }
                        }
                        //Here write code for Internation shipping address if user is outside of USA this will overwrite previous address
                        //there are cases where countryid is null hence check for null first, inorderto avoid errors. 
                        if (IncentexGlobal.CurrentMember.CountryId != null && IncentexGlobal.CurrentMember.CountryId != 223)//this is for overwrite existing address
                            GetInternationShippingAddress();

                    }
                    else //this is for employee pay
                    {
                        //Show the payment option dropdown if its for the Employee pay.
                        dvPaymentMethodSelection.Visible = true;
                        #region billing information
                        GetBillingDetails();
                        #endregion

                        #region Shipping information
                        GetShippingAddressBasedOnLocationDropdown();

                        //Here write code for Internation shipping address if user is outside of USA
                        //there are cases where countryid is null hence check for null first, inorderto avoid errors. 
                        if (IncentexGlobal.CurrentMember.CountryId != null && IncentexGlobal.CurrentMember.CountryId != 223)//this is for overwrite existing address
                            GetInternationShippingAddress();
                        #endregion
                    }
                }
            }
            else    //Shopping cart billing and shipping information
            {
                #region Employee Billing Information
                GetBillingDetails();
                #endregion

                #region Shipping Information
                GetShippingAddressBasedOnLocationDropdown();

                //Here write code for Internation shipping address if user is outside of USA
                //there are cases where countryid is null hence check for null first, inorderto avoid errors. 
                if (IncentexGlobal.CurrentMember.CountryId != null && IncentexGlobal.CurrentMember.CountryId != 223)//this is for overwrite existing address
                    GetInternationShippingAddress();
                #endregion
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Internation Shipping Address for user outside from USA Different for issuance policy and shopping cart

    private void GetInternationShippingAddress()
    {
        InternationalShippingAddress objInternationalShippingAddress = null;
        if (this.sProcess == "ui")
            objInternationalShippingAddress = new InternationalShippingAddressRepository().GetIssuancePolicyAddressByStoreIDAndWorkgroupID(new CompanyStoreRepository().GetStoreIDByCompanyId((Int64)IncentexGlobal.CurrentMember.CompanyId), objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).WorkgroupID);
        else
            objInternationalShippingAddress = new InternationalShippingAddressRepository().GetShoppingCartAddressByStoreIDAndWorkgroupID(new CompanyStoreRepository().GetStoreIDByCompanyId((Int64)IncentexGlobal.CurrentMember.CompanyId), objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).WorkgroupID);

        if (objInternationalShippingAddress != null)
        {
            txtSCompany.Text = objInternationalShippingAddress.CompanyName;
            txtSAddress.Text = objInternationalShippingAddress.Address;

            ddlSCountry.SelectedValue = Convert.ToString(objInternationalShippingAddress.CountryID);
            ddlSCountry_SelectedIndexChanged(null, null);

            ddlSState.SelectedValue = Convert.ToString(objInternationalShippingAddress.StateID);
            ddlSState_SelectedIndexChanged(null, null);

            ddlSCity.SelectedValue = Convert.ToString(objInternationalShippingAddress.CityID);

            //txtCounty.Text = objInternationalShippingAddress.County;
            txtSAirport.Text = objInternationalShippingAddress.Airport;
            ddlDepartment.SelectedValue = objInternationalShippingAddress.DepartmentID != null ? objInternationalShippingAddress.DepartmentID.ToString() : "0";
            txtSZip.Text = objInternationalShippingAddress.ZipCode;
            txtSTelephone.Text = objInternationalShippingAddress.Telephone;
            txtSEmail.Text = objInternationalShippingAddress.Email;

            //Desable the control
            txtSCompany.Enabled = false;
            txtSAddress.Enabled = false;
            txtSAddress2.Enabled = false;
            txtSStreet.Enabled = false;
            ddlSCountry.Enabled = false;
            ddlSState.Enabled = false;
            ddlSCity.Enabled = false;
            txtSZip.Enabled = false;
            txtSTelephone.Enabled = false;
            txtSEmail.Enabled = false;
            //txtCounty.Enabled = false;

            if (!String.IsNullOrEmpty(txtSAirport.Text.Trim()))
                txtSAirport.Enabled = false;

            ddlShippingLocations.Enabled = false;
            chkAddToMyAddressBook.Enabled = false;
        }
    }

    #endregion

    #region Shipping Detail based on shipping location dropdown

    private void GetShippingAddressBasedOnLocationDropdown()
    {
        txtSCompany.Enabled = true;
        txtSAddress.Enabled = true;
        txtSAddress2.Enabled = true;
        txtSStreet.Enabled = true;
        ddlSCountry.Enabled = true;
        ddlSState.Enabled = true;
        ddlSCity.Enabled = true;
        txtSZip.Enabled = true;
        txtSTelephone.Enabled = true;
        txtSEmail.Enabled = true;
        //txtCounty.Enabled = true;
        txtSAirport.Enabled = true;
        ddlShippingLocations.Enabled = true;
        chkAddToMyAddressBook.Enabled = true;

        if (ddlShippingLocations.SelectedIndex >= 0)
        {
            CompanyEmployeeContactInfo objShippingInfo = objCompanyEmployeeContactInfoRepository.GetShippingDetailById(IncentexGlobal.CurrentMember.UserInfoID, Int64.Parse(ddlShippingLocations.SelectedValue.ToString()));

            if (objShippingInfo != null)
            {
                txtSCompany.Text = objShippingInfo.CompanyName;
                txtSAddress.Text = objShippingInfo.Address;
                txtSAddress2.Text = objShippingInfo.Address2;
                txtSStreet.Text = objShippingInfo.Street;

                ddlSCountry.SelectedValue = Convert.ToString(objShippingInfo.CountryID);
                ddlSCountry_SelectedIndexChanged(null, null);

                ddlSState.SelectedValue = Convert.ToString(objShippingInfo.StateID);
                ddlSState_SelectedIndexChanged(null, null);

                ddlSCity.SelectedValue = Convert.ToString(objShippingInfo.CityID);

                //Start add by mayur on 26-jan-2012
                txtSAirport.Text = objShippingInfo.Airport;
                ddlDepartment.SelectedValue = objShippingInfo.DepartmentID != null ? objShippingInfo.DepartmentID.ToString() : "0";
                //End by mayur on 26-jan-2012
                txtSZip.Text = objShippingInfo.ZipCode;
                txtSTelephone.Text = objShippingInfo.Telephone;
                txtSEmail.Text = objShippingInfo.Email;
                //txtCounty.Text = objShippingInfo.county;
            }
        }
    }

    #endregion

    #region Get CartID of Order Item

    /// <summary>
    /// Gets the shopping cartid of item of order and stored it on session["cartID"].
    /// </summary>
    private void GetCartIDs()
    {
        List<MyShoppinCart> objMyShoppinCart = new List<MyShoppinCart>();
        String CartIDs = String.Empty;
        if (this.sProcess == "sc")
        {
            objMyShoppinCart = objShoppingCartRepository.GetUnorderedCartIdsByUser(IncentexGlobal.CurrentMember.UserInfoID);

            if (objMyShoppinCart.Count > 0)
            {
                if (objMyShoppinCart.Count == 1)
                {
                    foreach (var item in objMyShoppinCart)
                    {
                        CartIDs = item.MyShoppingCartID.ToString();
                    }
                }
                else
                {
                    foreach (var item in objMyShoppinCart)
                    {
                        CartIDs = CartIDs + item.MyShoppingCartID.ToString() + ",";
                    }
                    CartIDs = CartIDs.Remove(CartIDs.Length - 1);
                }
            }
        }
        else
        {
            StringBuilder sbCartIDs = new StringBuilder();

            if (!String.IsNullOrEmpty(Convert.ToString(Session["SingleCartIDs"])))
            {
                sbCartIDs.Append(Session["SingleCartIDs"].ToString() + ",");
                if (sbCartIDs.Length > 0)
                    CartIDs = sbCartIDs.ToString().Remove(sbCartIDs.Length - 1, 1);
            }

            if (!String.IsNullOrEmpty(Convert.ToString(Session["GroupCartIDs"])))
            {
                if (Convert.ToString(Session["GroupCartIDs"]).Length > 0)
                    CartIDs = Convert.ToString(Session["GroupCartIDs"]).Remove(Convert.ToString(Session["GroupCartIDs"]).Length - 1, 1);

                sbCartIDs.Append(CartIDs + ",");
                if (sbCartIDs.Length > 0)
                    CartIDs = sbCartIDs.ToString().Remove(sbCartIDs.Length - 1, 1);
            }

            if (!String.IsNullOrEmpty(Convert.ToString(Session["GroupBudgetCartIDs"])))
            {
                if (Convert.ToString(Session["GroupBudgetCartIDs"]).Length > 0)
                    CartIDs = Convert.ToString(Session["GroupBudgetCartIDs"]).Remove(Convert.ToString(Session["GroupBudgetCartIDs"]).Length - 1, 1);

                sbCartIDs.Append(CartIDs + ",");
                if (sbCartIDs.Length > 0)
                    CartIDs = sbCartIDs.ToString().Remove(sbCartIDs.Length - 1, 1);
            }
            if (!String.IsNullOrEmpty(Convert.ToString(Session["MyIssuanceCartIDs"])))
                CartIDs = Convert.ToString(Session["MyIssuanceCartIDs"]);
        }

        Session["CartIDs"] = CartIDs;
    }

    #endregion

    #region Calculate Shipping Amount

    /// <summary>
    /// Calculates the Shipping amount based on settings set in Company Store
    /// </summary>
    private void GetShippingAmount()
    {
        try
        {
            UserInformation objUser = new UserInformation();
            CompanyEmployee objCE = objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
            CompanyStore objCS = new CompanyStore();

            Boolean IsAdmin;
            String ShippingProgFor;
            Decimal TotalAmount = 0;
            Decimal? MinShipAmt = 0;
            Decimal? ShipPercent = 0;
            Decimal? TotalSaleAbove = 0;
            Decimal PromoItemValue = 0;
            // Get the User information
            objUser = objUserInformationRepository.GetById(IncentexGlobal.CurrentMember.UserInfoID);
            IsAdmin = objCE.isCompanyAdmin; // Check if User is Admin for the above Company or not

            objCS = objCompanyStoreRepository.GetById(this.StoreID);

            ShippingProgFor = objCS.shippingprogramfor; // Get the value indicating for whom the program is notapplicable

            // Check if Free Shipping is active or not
            if (objCS.isFreeShippingActive == true)
            {
                // Check if Free Shipping program is valid (in promotion)
                if (objCS.ShippingProgramStartDate < DateTime.Now && objCS.ShippingProgramEndDate > DateTime.Now)
                {
                    switch (ShippingProgFor.ToLower())
                    {
                        case "admin": // For Admin
                            if (IsAdmin == true)
                            {
                                // Check if Sale Shipping is applicable
                                if (objCS.IsSaleShipping == true)
                                {
                                    TotalAmount = Session["TotalAmount"] != null ? Decimal.Parse(Session["TotalAmount"].ToString()) : 0;
                                    //--Saurabh Check for Promotional Items
                                    PromoItemValue = CheckForPromotionCode();
                                    if (PromoItemValue != 0)
                                    {
                                        TotalAmount = TotalAmount - PromoItemValue;
                                    }
                                    MinShipAmt = objCS.MinimumShippingAmount;
                                    ShipPercent = objCS.ShippiingPercentOfSale;
                                    TotalSaleAbove = Decimal.Parse(objCS.totalsaleabove);

                                    // Compare OrderAmount with TotalSalesAbove (Shipping)
                                    if (TotalAmount <= TotalSaleAbove)
                                        this.ShippingAmount = MinShipAmt; // Shipping amount = only minimum shipping amount
                                    else
                                        this.ShippingAmount = MinShipAmt + ((TotalAmount - TotalSaleAbove) * ShipPercent / 100);
                                }
                                else // Sale Shipping is not applicable
                                {
                                    this.ShippingAmount = 0;
                                }
                                lblFreeshipping.Visible = false;
                                lblShippingAmount.Visible = true;
                            }
                            else
                            {
                                this.ShippingAmount = 0;
                                lblFreeshipping.Visible = true;
                                isFreeShipping = true;
                                lblShippingAmount.Visible = false;
                            }
                            break;

                        case "employee": // For Employee
                            if (IsAdmin == false)
                            {
                                // Check if Sale Shipping is applicable
                                if (objCS.IsSaleShipping == true)
                                {
                                    TotalAmount = Session["TotalAmount"] != null ? Decimal.Parse(Session["TotalAmount"].ToString()) : 0;
                                    //--Saurabh Check for Promotional Items
                                    PromoItemValue = CheckForPromotionCode();
                                    if (PromoItemValue != 0)
                                    {
                                        TotalAmount = TotalAmount - PromoItemValue;
                                    }
                                    MinShipAmt = objCS.MinimumShippingAmount;
                                    ShipPercent = objCS.ShippiingPercentOfSale;
                                    TotalSaleAbove = Decimal.Parse(objCS.totalsaleabove);

                                    // Compare OrderAmount with TotalSalesAbove (Shipping)
                                    if (TotalAmount <= TotalSaleAbove)
                                        this.ShippingAmount = MinShipAmt; // Shipping amount = only minimum shipping amount
                                    else
                                        this.ShippingAmount = MinShipAmt + ((TotalAmount - TotalSaleAbove) * ShipPercent / 100);
                                }
                                else // Sale Shipping is not applicable
                                {
                                    this.ShippingAmount = 0;
                                }
                                lblFreeshipping.Visible = false;
                                lblShippingAmount.Visible = true;
                            }
                            else
                            {
                                this.ShippingAmount = 0;
                                lblFreeshipping.Visible = true;
                                isFreeShipping = true;
                                lblShippingAmount.Visible = false;
                            }
                            break;

                        case "both": // For Both
                            // Check if Sale Shipping is applicable
                            if (objCS.IsSaleShipping == true)
                            {
                                TotalAmount = Session["TotalAmount"] != null ? Decimal.Parse(Session["TotalAmount"].ToString()) : 0;
                                //--Saurabh Check for Promotional Items
                                PromoItemValue = CheckForPromotionCode();
                                if (PromoItemValue != 0)
                                {
                                    TotalAmount = TotalAmount - PromoItemValue;
                                }
                                MinShipAmt = objCS.MinimumShippingAmount;
                                ShipPercent = objCS.ShippiingPercentOfSale;
                                TotalSaleAbove = Decimal.Parse(objCS.totalsaleabove);

                                // Compare OrderAmount with TotalSalesAbove (Shipping)
                                if (TotalAmount <= TotalSaleAbove)
                                    this.ShippingAmount = MinShipAmt; // Shipping amount = only minimum shipping amount
                                else
                                    this.ShippingAmount = MinShipAmt + ((TotalAmount - TotalSaleAbove) * ShipPercent / 100);
                            }
                            else // Sale Shipping is not applicable
                            {
                                this.ShippingAmount = 0;
                            }
                            lblFreeshipping.Visible = false;
                            lblShippingAmount.Visible = true;
                            break;
                        case "bothunticked":
                            this.ShippingAmount = 0;
                            lblFreeshipping.Visible = true;
                            isFreeShipping = true;
                            lblShippingAmount.Visible = false;
                            break;
                        default:
                            // Check if Sale Shipping is applicable
                            if (objCS.IsSaleShipping == true)
                            {
                                TotalAmount = Session["TotalAmount"] != null ? Decimal.Parse(Session["TotalAmount"].ToString()) : 0;
                                //--Saurabh Check for Promotional Items
                                PromoItemValue = CheckForPromotionCode();
                                if (PromoItemValue != 0)
                                {
                                    TotalAmount = TotalAmount - PromoItemValue;
                                }
                                MinShipAmt = objCS.MinimumShippingAmount;
                                ShipPercent = objCS.ShippiingPercentOfSale;
                                TotalSaleAbove = Decimal.Parse(objCS.totalsaleabove);

                                // Compare OrderAmount with TotalSalesAbove (Shipping)
                                if (TotalAmount <= TotalSaleAbove)
                                    this.ShippingAmount = MinShipAmt; // Shipping amount = only minimum shipping amount
                                else
                                    this.ShippingAmount = MinShipAmt + ((TotalAmount - TotalSaleAbove) * ShipPercent / 100);
                            }
                            else // Sale Shipping is not applicable
                            {
                                this.ShippingAmount = 0;
                            }
                            lblFreeshipping.Visible = false;
                            lblShippingAmount.Visible = true;
                            break;

                    }
                }
            }
            else // Free Shipping Inactive so calculate Shipping charge
            {
                // Check if Sale Shipping is applicable
                if (objCS.IsSaleShipping == true)
                {
                    TotalAmount = Session["TotalAmount"] != null ? Decimal.Parse(Session["TotalAmount"].ToString()) : 0;
                    //--Saurabh Check for Promotional Items
                    PromoItemValue = CheckForPromotionCode();
                    if (PromoItemValue != 0)
                    {
                        TotalAmount = TotalAmount - PromoItemValue;
                    }
                    MinShipAmt = objCS.MinimumShippingAmount;
                    ShipPercent = objCS.ShippiingPercentOfSale;
                    TotalSaleAbove = Decimal.Parse(objCS.totalsaleabove);

                    // Compare OrderAmount with TotalSalesAbove (Shipping)
                    if (TotalAmount <= TotalSaleAbove)
                        this.ShippingAmount = MinShipAmt; // Shipping amount = only minimum shipping amount
                    else
                        this.ShippingAmount = MinShipAmt + ((TotalAmount - TotalSaleAbove) * ShipPercent / 100);
                }
                else // Sale Shipping is not applicable
                {
                    this.ShippingAmount = 0;
                }
                lblFreeshipping.Visible = false;
                lblShippingAmount.Visible = true;
            }
            this.ShippingAmount = Math.Round((Decimal)this.ShippingAmount, 2);

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void CheckForPriceLevel(Int64? storeProductid, String itemNumber)
    {
        try
        {

            //Saurabh --Check For Price Level

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            {
                PriceLevelID = objMarketingToolRepository.GetPriceLevelID(Convert.ToString(Incentex.DAL.Common.DAEnums.PriceLevelName.L1));
            }
            else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
            {
                PriceLevelID = objMarketingToolRepository.GetPriceLevelID(Convert.ToString(Incentex.DAL.Common.DAEnums.PriceLevelName.L2));
            }
            else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
            {
                PriceLevelID = objMarketingToolRepository.GetPriceLevelID(Convert.ToString(Incentex.DAL.Common.DAEnums.PriceLevelName.L4));
            }
            //--Check for L3            

            if (objMarketingToolRepository.CheckForL3(storeProductid, itemNumber))
            {
                PriceLevelID = objMarketingToolRepository.GetPriceLevelID(Convert.ToString(Incentex.DAL.Common.DAEnums.PriceLevelName.L3));
            }

            //----
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private Decimal CheckForPromotionCode()
    {
        CompanyStore objCS = new CompanyStore();
        Decimal TotalPromoItemPrice = 0;
        try
        {
            //Saurabh--Promotion Code
            if (!String.IsNullOrEmpty(PromotionCode))
            {
                objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectShoppingProduct(IncentexGlobal.CurrentMember.UserInfoID);
                if (objSelectMyShoppingCartProductResultList.Count > 0)
                {
                    foreach (var item in objSelectMyShoppingCartProductResultList)
                    {
                        StoreProduct objStoreProduct = new StoreProduct();
                        //Get ProductItemID
                        Int64 ProductItemID = objMarketingToolRepository.GetProductItemID(item.StoreProductID, item.item); // Int64 ProductItemID = objMarketingToolRepository.GetProductItemID(this.StoreProductId, this.ItemNumber);
                        objStoreProduct = objMarketingToolRepository.GetByStoreProductID(item.StoreProductID);//objStoreProduct=objMarketingToolRepository.GetByStoreProductID(this.StoreProductId);
                        CheckForPriceLevel(item.StoreProductID, item.item);
                        if (ProductItemID != 0)
                        {
                            String PromoCode = objMarketingToolRepository.CheckPromotionCode(objStoreProduct.StoreId, objStoreProduct.WorkgroupID, this.PriceLevelID, ProductItemID);
                            if (PromoCode == this.PromotionCode)
                            {
                                TotalPromoItemPrice = TotalPromoItemPrice + (Convert.ToDecimal(item.UnitPrice) * Convert.ToDecimal(item.Quantity));
                            }
                        }
                    }
                }
            }
            return TotalPromoItemPrice;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            return 0;
        }
    }
    #endregion

    #region Calculate credit amount and set labels

    /// <summary>
    /// Fetch the previous credit amount and 
    /// anniversary credit amount for the employee
    /// </summary>
    private void GetCreditAmount()
    {
        try
        {
            Decimal? TotalAmount = 0;
            Decimal? AnniversaryCredit = 0;

            if (this.sProcess == "sc")
            {
                if (ddlPaymentOption.SelectedItem.Text != "MOAS" && ddlPaymentOption.SelectedItem.Text != "Replacement Uniforms")
                {
                    #region Objects/Variables


                    CompanyEmployee objCE = objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

                    //Load the Anniversay Credit record
                    AnniversaryCreditProgram objACP = new AnniversaryCreditProgram();
                    objACP = objAnniversaryProgramRepository.GetProgByStoreIdWorkgroupId(objCE.WorkgroupID, this.StoreID);

                    #endregion

                    #region Anniversary Credit Calculation

                    if (objACP != null)
                    {
                        if (objACP.EmployeeStatus != null)
                        {
                            //Check Status of Anniversary Credit Program
                            if (objLookupRepository.GetById(Int64.Parse(objACP.EmployeeStatus.ToString())).sLookupName.ToString().ToLower() == "active")
                            {
                                SelectAnniversaryCreditProgramPerEmployeeResult objCEAnniversary = new AnniversaryProgramRepository().GetCompanyEmployeeAnniversaryCreditDetails(IncentexGlobal.CurrentMember.UserInfoID);

                                //Check the Anniversay Credit is applicable and not expired
                                if (objCEAnniversary.AnniversaryCreditBalance != null && objCEAnniversary.AnniversaryCreditBalance != 0)
                                    AnniversaryCredit += objCE.CreditAmtToApplied;
                            }
                        }
                    }
                    //check that program is active and aniversary credit is greater then 0 then only anniversary credit option is enable
                    if (objCE.ProgramActive == "Active" && AnniversaryCredit > 0)
                    {
                        liAnniversaryCredits.Visible = true;
                        lblAnniversaryCreditBallance.Text = AnniversaryCredit.ToString();
                    }
                    else
                        liAnniversaryCredits.Visible = false;

                    #endregion
                }

                Decimal? PayableByCC = 0;

                #region Set Labels

                if (Session["TotalAmount"] != null)
                {
                    lblOrderTotal.Text = Session["TotalAmount"].ToString();
                    lblSalesTax.Text = this.SalesTaxAmount.ToString("#,##0.00");
                    lblShippingAmount.Text = this.ShippingAmount.ToString();

                    #region Credit Type Selection

                    if (liAnniversaryCredits.Visible == true)
                    {
                        // Anniversary Credit Selected
                        if (isAnniversaryCreditApply == true) //here check that aniversary credit apply for this order selected by user or not
                        {
                            trAnniversaryCredit.Visible = true;
                            trRemAnniversaryCredit.Visible = true;
                            lblAnniversaryCredit.Text = AnniversaryCredit.ToString();

                            if (((Decimal.Parse(Session["TotalAmount"].ToString()) + this.ShippingAmount + this.SalesTaxAmount) - AnniversaryCredit) >= 0)
                            {
                                lblRemAnniversaryCredit.Text = "0.00";
                                if (Session["NotAnniversaryCreditAmount"].ToString() != "0")
                                {
                                    Decimal? TAmount = Convert.ToDecimal(Session["TotalAmount"].ToString());
                                    Decimal? totaAnniNC = Convert.ToDecimal(Session["NotAnniversaryCreditAmount"].ToString());
                                    Decimal? FinalAmount = TAmount - totaAnniNC;
                                    lblOrderTotal.Text = Convert.ToString(FinalAmount);

                                    trPurchasesNotEligableForAnniversary.Visible = true;
                                    lblNotEligibleforAnniversarycredit.Text = Session["NotAnniversaryCreditAmount"].ToString();
                                }
                                else
                                {
                                    trPurchasesNotEligableForAnniversary.Visible = false;
                                }
                            }
                            else
                            {
                                if (Session["NotAnniversaryCreditAmount"].ToString() != "0")
                                {
                                    Decimal? TAmount = Convert.ToDecimal(Session["TotalAmount"].ToString());
                                    Decimal? totaAnniNC = Convert.ToDecimal(Session["NotAnniversaryCreditAmount"].ToString());
                                    Decimal? FinalAmount = TAmount - totaAnniNC;
                                    lblOrderTotal.Text = Convert.ToString(FinalAmount);

                                    trPurchasesNotEligableForAnniversary.Visible = true;
                                    lblNotEligibleforAnniversarycredit.Text = Session["NotAnniversaryCreditAmount"].ToString();

                                }//END NAGMANI 9fEB
                                else
                                {
                                    trPurchasesNotEligableForAnniversary.Visible = false;
                                }
                                //Start nagmani 10 Feb
                                lblRemAnniversaryCredit.Text = (AnniversaryCredit - (Decimal.Parse(lblOrderTotal.Text) + this.ShippingAmount + this.SalesTaxAmount)).ToString();
                                //End
                            }

                            if (Decimal.Parse(lblAnniversaryCredit.Text) <= 0)
                            {
                                PayableByCC += Decimal.Parse(lblOrderTotal.Text) + Decimal.Parse(lblNotEligibleforAnniversarycredit.Text != "" ? lblNotEligibleforAnniversarycredit.Text : "0") + this.ShippingAmount + Decimal.Parse(lblSalesTax.Text);
                            }
                            else if (Decimal.Parse(lblAnniversaryCredit.Text) >= (Decimal.Parse(lblOrderTotal.Text) + this.ShippingAmount + Decimal.Parse(lblSalesTax.Text)))
                            {
                                if (lblNotEligibleforAnniversarycredit.Text != "")
                                    PayableByCC = Decimal.Parse(lblNotEligibleforAnniversarycredit.Text);
                            }
                            else if ((Decimal.Parse(lblAnniversaryCredit.Text) > 0) && (Decimal.Parse(lblAnniversaryCredit.Text) < (Decimal.Parse(lblOrderTotal.Text) + this.ShippingAmount + Decimal.Parse(lblSalesTax.Text))))
                            {
                                PayableByCC = Decimal.Parse(lblNotEligibleforAnniversarycredit.Text != "" ? lblNotEligibleforAnniversarycredit.Text : "0") + (Decimal.Parse(lblOrderTotal.Text) + this.ShippingAmount + Decimal.Parse(lblSalesTax.Text) - AnniversaryCredit);
                            }
                            else
                            {
                                if (lblNotEligibleforAnniversarycredit.Text != "")
                                    PayableByCC = Decimal.Parse(lblNotEligibleforAnniversarycredit.Text);
                            }
                        }
                        // Nothing selected
                        else
                        {

                            PayableByCC = Decimal.Parse(Session["TotalAmount"].ToString()) + this.ShippingAmount + Decimal.Parse(lblSalesTax.Text);

                            trAnniversaryCredit.Visible = false;
                            trRemAnniversaryCredit.Visible = false;
                            lblBalanceDueTip.Visible = false;//add by mayur on 13-feb-2012

                            trPurchasesNotEligableForAnniversary.Visible = false;
                        }
                    }
                    else
                    {
                        trAnniversaryCredit.Visible = false;
                        trRemAnniversaryCredit.Visible = false;
                        lblBalanceDueTip.Visible = false;

                        PayableByCC = Decimal.Parse(Session["TotalAmount"].ToString()) + this.ShippingAmount + Decimal.Parse(lblSalesTax.Text);

                        trPurchasesNotEligableForAnniversary.Visible = false;
                    }

                    #endregion

                    if (Session["NotAnniversaryCreditAmount"].ToString() != "0")
                        liPaymentOptions.Visible = true;
                    else
                        liPaymentOptions.Visible = false;

                    if (PayableByCC > 0)
                    {
                        liPaymentOptions.Visible = true;
                        lblBalanceDueTip.Visible = true;
                        lblPayByCreditCard.Text = PayableByCC.ToString();
                    }
                    else
                    {
                        liPaymentOptions.Visible = false;
                        pnlCreditCardPayment.Visible = false;
                        pnlMOASPayment.Visible = false;
                        lblBalanceDueTip.Visible = false;
                        lblPayByCreditCard.Text = "0.00";
                    }
                }
                else
                {
                    lblOrderTotal.Text = "0.00";
                    lblSalesTax.Text = "0.00";
                    lblPayByCreditCard.Text = "0.00";
                    lblShippingAmount.Text = "0.00";
                    trAnniversaryCredit.Visible = false;
                    trRemAnniversaryCredit.Visible = false;
                    lblBalanceDueTip.Visible = true;
                }

                if (liPaymentOptions.Visible == false)
                {
                    liPaymentOptiondropdownCode.Visible = false; //liPaymentOptionCode.Visible = false;
                    liPaymentOptionCode.Visible = false;
                }
                else if (ddlPaymentOption.SelectedValue == Convert.ToString(Convert.ToInt64(Incentex.DAL.Common.DAEnums.PaymentOptions.CostCenterCode)) || ddlPaymentOption.SelectedValue == Convert.ToString(Convert.ToInt64(Incentex.DAL.Common.DAEnums.PaymentOptions.GLCode))
                     || ddlPaymentOption.SelectedValue == Convert.ToString(Convert.ToInt64(Incentex.DAL.Common.DAEnums.PaymentOptions.PurchaseOrder)))
                {
                    if (ddlPaymentOption.SelectedItem.Text == "Cost-Center Code")
                    {
                        liPaymentOptiondropdownCode.Visible = true; //liPaymentOptionCode.Visible = false;
                    }
                    else
                    {
                        liPaymentOptionCode.Visible = true;
                    }
                }
                else if (ddlPaymentOption.SelectedValue == Convert.ToString(Convert.ToInt64(Incentex.DAL.Common.DAEnums.PaymentOptions.CreditCard)))
                {
                    pnlCreditCardPayment.Visible = true;
                }


                if (trAnniversaryCredit.Visible == true)
                    Session["AnniversaryCredit"] = lblAnniversaryCredit.Text;
                else
                    Session["AnniversaryCredit"] = 0;
                #endregion
            }
            else
            {
                #region Get order total amount
                //TotalAmount += Session["SingleTotal"] != null ? Decimal.Parse(Session["SingleTotal"].ToString()) : 0;
                //TotalAmount += Session["GroupTotal"] != null ? Decimal.Parse(Session["GroupTotal"].ToString()) : 0;
                //TotalAmount += Session["GroupBudgetTotal"] != null ? Decimal.Parse(Session["GroupBudgetTotal"].ToString()) : 0;
                TotalAmount = Session["TotalAmount"] != null ? Decimal.Parse(Session["TotalAmount"].ToString()) : 0;
                #endregion

                #region Set Labels
                liAnniversaryCredits.Visible = false;
                //liPaymentOptions.Visible = true;

                trAnniversaryCredit.Visible = false;
                trRemAnniversaryCredit.Visible = false;

                trPurchasesNotEligableForAnniversary.Visible = false;

                lblOrderTotal.Text = TotalAmount.ToString();
                lblSalesTax.Text = this.SalesTaxAmount.ToString("#,##0.00");
                lblShippingAmount.Text = this.ShippingAmount.ToString();
                lblPayByCreditCard.Text = (TotalAmount + Decimal.Parse(lblSalesTax.Text) + this.ShippingAmount).ToString();

                #region check that show price for this policy or not
                UniformIssuancePolicy objPolicy = new UniformIssuancePolicy();
                INC_Lookup objLook = new INC_Lookup();
                objPolicy = objUniRepos.GetById(Convert.ToInt64(Session["PID"].ToString()));
                if (objPolicy != null)
                {
                    //Check that apply corporate Discount for this policy or not
                    if (objPolicy.CorporateDiscountStatus && objPolicy.CorporateAmount != null && objPolicy.CorporateAmount != 0)
                    {
                        lblCorporateDiscount.Text = Convert.ToString(objPolicy.CorporateAmount);
                        lblPayByCreditCard.Text = Convert.ToString(Decimal.Parse(lblPayByCreditCard.Text) - objPolicy.CorporateAmount);
                    }

                    objLook = objLookupRepository.GetById(Convert.ToInt64(objPolicy.PRICEFOR));

                    if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Both")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            lblShippingAmount.Visible = true;
                            tdShippingCharge.Visible = true;
                            lblOrderTotal.Visible = true;
                            tdTotalOrderAmt.Visible = true;
                            lblSalesTax.Visible = true;
                            tdSalestax.Visible = true;
                            if (lblCorporateDiscount.Text != "0")
                                trCorporateDiscount.Visible = true;
                        }
                        else
                        {
                            lblShippingAmount.Visible = false;
                            tdShippingCharge.Visible = false;
                            lblOrderTotal.Visible = false;
                            tdTotalOrderAmt.Visible = false;
                            lblSalesTax.Visible = false;
                            tdSalestax.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Employee")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                            lblShippingAmount.Visible = true;
                            tdShippingCharge.Visible = true;
                            lblOrderTotal.Visible = true;
                            tdTotalOrderAmt.Visible = true;
                            lblSalesTax.Visible = true;
                            tdSalestax.Visible = true;
                            if (lblCorporateDiscount.Text != "0")
                                trCorporateDiscount.Visible = true;
                        }
                        else
                        {
                            lblShippingAmount.Visible = false;
                            tdShippingCharge.Visible = false;
                            lblOrderTotal.Visible = false;
                            tdTotalOrderAmt.Visible = false;
                            lblSalesTax.Visible = false;
                            tdSalestax.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Admin")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            lblShippingAmount.Visible = true;
                            tdShippingCharge.Visible = true;
                            lblOrderTotal.Visible = true;
                            tdTotalOrderAmt.Visible = true;
                            lblSalesTax.Visible = true;
                            tdSalestax.Visible = true;
                            if (lblCorporateDiscount.Text != "0")
                                trCorporateDiscount.Visible = true;
                        }
                        else
                        {
                            lblShippingAmount.Visible = false;
                            tdShippingCharge.Visible = false;
                            lblOrderTotal.Visible = false;
                            tdTotalOrderAmt.Visible = false;
                            lblSalesTax.Visible = false;
                            tdSalestax.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Admin")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                            lblShippingAmount.Visible = true;
                            tdShippingCharge.Visible = true;
                            lblOrderTotal.Visible = true;
                            tdTotalOrderAmt.Visible = true;
                            lblSalesTax.Visible = true;
                            tdSalestax.Visible = true;
                            if (lblCorporateDiscount.Text != "0")
                                trCorporateDiscount.Visible = true;
                        }
                        else
                        {
                            lblShippingAmount.Visible = false;
                            tdShippingCharge.Visible = false;
                            lblOrderTotal.Visible = false;
                            tdTotalOrderAmt.Visible = false;
                            lblSalesTax.Visible = false;
                            tdSalestax.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Employee")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        {
                            lblShippingAmount.Visible = true;
                            tdShippingCharge.Visible = true;
                            lblOrderTotal.Visible = true;
                            tdTotalOrderAmt.Visible = true;
                            lblSalesTax.Visible = true;
                            tdSalestax.Visible = true;
                            if (lblCorporateDiscount.Text != "0")
                                trCorporateDiscount.Visible = true;
                        }
                        else
                        {
                            lblShippingAmount.Visible = false;
                            tdShippingCharge.Visible = false;
                            lblOrderTotal.Visible = false;
                            tdTotalOrderAmt.Visible = false;
                            lblSalesTax.Visible = false;
                            tdSalestax.Visible = false;
                        }
                    }
                    else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Both")
                    {
                        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        {
                            lblShippingAmount.Visible = false;
                            tdShippingCharge.Visible = false;
                            lblOrderTotal.Visible = false;
                            tdTotalOrderAmt.Visible = false;
                            lblSalesTax.Visible = false;
                            tdSalestax.Visible = false;
                        }
                        else
                        {
                            lblShippingAmount.Visible = true;
                            tdShippingCharge.Visible = true;
                            lblOrderTotal.Visible = true;
                            tdTotalOrderAmt.Visible = true;
                            lblSalesTax.Visible = true;
                            tdSalestax.Visible = true;
                            if (lblCorporateDiscount.Text != "0")
                                trCorporateDiscount.Visible = true;
                        }
                    }
                }
                #endregion

                #endregion
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            ex.Message.ToString();
        }
    }

    #endregion

    #region Employee Billing Information

    private void GetBillingDetails()
    {
        String StoreBillingSettting = new PreferenceRepository().GetStorePreferenceValue(this.StoreID, "StoreBillingSetting");

        if (StoreBillingSettting == "Capture Billing on Checkout")
        {
            CompanyEmployeeContactInfo objBillingInfo = objCompanyEmployeeContactInfoRepository.GetFirstBillingDetailByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);

            if (objBillingInfo != null)
            {
                txtBFirstName.Text = objBillingInfo.BillingCO;
                txtBLastName.Text = objBillingInfo.Manager;
                txtBCompany.Text = objBillingInfo.CompanyName;
                txtBAddressLine1.Text = objBillingInfo.Address;
                txtBAddressLine2.Text = objBillingInfo.Address2;

                ddlBCountry.SelectedValue = objBillingInfo.CountryID.ToString();
                ddlBCountry_SelectedIndexChanged(null, null);
                try
                {
                    ddlBState.SelectedValue = objBillingInfo.StateID.ToString();
                    ddlBState_SelectedIndexChanged(null, null);

                    ddlBCity.SelectedValue = objBillingInfo.CityID.ToString();
                    ddlBCity_SelectedIndexChanged(null, null);
                }
                catch { }
                txtBZip.Text = objBillingInfo.ZipCode;
                txtBEmail.Text = objBillingInfo.Email;
            }

            //Enabled the fields
            txtBFirstName.Enabled = true;
            txtBAddressLine1.Enabled = true;
            txtBAddressLine2.Enabled = true;
            txtBCompany.Enabled = true;
            txtBEmail.Enabled = true;
            txtBLastName.Enabled = true;
            txtBZip.Enabled = true;
            ddlBCity.Enabled = true;
            ddlBState.Enabled = true;
            ddlBCountry.Enabled = true;
            chkBillingSameAsShipping.Enabled = true;
        }
        else if (StoreBillingSettting == "Fixed Corporate Billing by Workgroup")
        {
            GlobalBillingAddress objBillingAddress = new GlobalBillingAddressRepository().GetByStoreAndWorkGroupID(this.StoreID, new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).WorkgroupID);

            if (objBillingAddress != null)
            {
                txtBFirstName.Text = objBillingAddress.FirstName;
                txtBLastName.Text = objBillingAddress.LastName;
                txtBCompany.Text = objBillingAddress.CompanyName;
                txtBAddressLine1.Text = objBillingAddress.AddressLine1;
                txtBAddressLine2.Text = objBillingAddress.AddressLine2;

                ddlBCountry.SelectedValue = objBillingAddress.CountryID.ToString();
                ddlBCountry_SelectedIndexChanged(null, null);
                try
                {
                    ddlBState.SelectedValue = objBillingAddress.StateID.ToString();
                    ddlBState_SelectedIndexChanged(null, null);

                    ddlBCity.SelectedValue = objBillingAddress.CityID.ToString();
                    ddlBCity_SelectedIndexChanged(null, null);
                }
                catch { }
                txtBZip.Text = objBillingAddress.ZipCode;
                txtBEmail.Text = objBillingAddress.Email;
            }

            //Enabled the fields
            txtBFirstName.ReadOnly = true;
            txtBAddressLine1.ReadOnly = true;
            txtBAddressLine2.ReadOnly = true;
            txtBCompany.ReadOnly = true;
            txtBEmail.ReadOnly = true;
            txtBLastName.ReadOnly = true;
            txtBZip.ReadOnly = true;
            ddlBCity.Enabled = false;
            ddlBState.Enabled = false;
            ddlBCountry.Enabled = false;
            chkBillingSameAsShipping.Enabled = false;
        }
        //End
    }

    #endregion

    #region Get Billing detail from global menu

    private void getsetBillingDetailsFromGlobalMenu()
    {
        GlobalMenuSetting objValue = new GlobalMenuSettingRepository().GetById(objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).WorkgroupID, new CompanyStoreRepository().GetStoreIDByCompanyId((Int64)IncentexGlobal.CurrentMember.CompanyId));
        if (objValue != null)
        {
            //Check if Paymnet option is ticked
            if (objValue.Paymentoption.Contains(objLookupRepository.GetIdByLookupName("Employee Payroll Deduct").ToString()))
            {
                if ((Boolean)objValue.IsGlobalBilling)
                {
                    txtBCompany.Text = objValue.CompanyName;
                    txtBFirstName.Text = objValue.FirstName;
                    txtBLastName.Text = objValue.LastName;
                    txtBAddressLine1.Text = objValue.Address;
                    //txtBAddressLine2.Text = objValue.Address2;
                    txtBZip.Text = objValue.Zip;
                    txtBEmail.Text = objValue.Email;

                    if (ddlBCountry.SelectedItem.Value != objValue.CountryId.ToString())
                    {
                        ddlBCountry.SelectedValue = objValue.CountryId.ToString();
                        ddlBCountry_SelectedIndexChanged(null, null);
                    }

                    if (ddlBState.SelectedItem.Value != objValue.StateId.ToString())
                    {
                        ddlBState.ClearSelection();
                        ddlBState.SelectedValue = objValue.StateId.ToString();
                        ddlBState_SelectedIndexChanged(null, null);
                    }

                    if (ddlBCity.SelectedItem.Value != objValue.CityId.ToString())
                    {
                        ddlBCity.ClearSelection();
                        ddlBCity.SelectedValue = objValue.CityId.ToString();
                        ddlBCity_SelectedIndexChanged(null, null);
                    }

                    //Disabled the fields
                    txtBFirstName.Enabled = false;
                    txtBAddressLine1.Enabled = false;
                    txtBAddressLine2.Enabled = false;
                    txtBCompany.Enabled = false;
                    txtBEmail.Enabled = false;
                    txtBLastName.Enabled = false;
                    txtBZip.Enabled = false;
                    ddlBCity.Enabled = false;
                    ddlBState.Enabled = false;
                    ddlBCountry.Enabled = false;
                    chkBillingSameAsShipping.Enabled = false;
                    //End
                }

            }
        }

    }

    #endregion

    #region Get Billing detail for MOAS based on store and workgroup

    private void GetMOASBillingDetail()
    {
        MOASShoppingCartAddress objValue = new MOASShoppingCartAddressRepository().GetByStoreIDWorkgroupIDAndDepartmentID(new CompanyStoreRepository().GetStoreIDByCompanyId((Int64)IncentexGlobal.CurrentMember.CompanyId), objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).WorkgroupID);
        if (objValue != null)
        {
            txtBCompany.Text = objValue.BCompanyName;
            txtBFirstName.Text = objValue.FirstBName;
            txtBLastName.Text = objValue.LastBName;
            txtBAddressLine1.Text = objValue.BAddress1;
            txtBAddressLine2.Text = objValue.BAddress2;
            txtBZip.Text = objValue.BZipCode;
            txtBEmail.Text = objValue.BEmail;

            ddlBCountry.ClearSelection();
            ddlBCountry.SelectedValue = objValue.BCountryId.ToString();
            ddlBCountry_SelectedIndexChanged(null, null);

            ddlBState.ClearSelection();
            ddlBState.SelectedValue = objValue.BStateId.ToString();
            ddlBState_SelectedIndexChanged(null, null);


            ddlBCity.ClearSelection();
            ddlBCity.SelectedValue = objValue.BCityId.ToString();
            ddlBCity_SelectedIndexChanged(null, null);

            //Disabled the fields
            txtBFirstName.Enabled = false;
            txtBAddressLine1.Enabled = false;
            txtBAddressLine2.Enabled = false;
            txtBCompany.Enabled = false;
            txtBEmail.Enabled = false;
            txtBLastName.Enabled = false;
            txtBZip.Enabled = false;
            ddlBCity.Enabled = false;
            ddlBState.Enabled = false;
            ddlBCountry.Enabled = false;
            chkBillingSameAsShipping.Enabled = false;
            //End
        }
    }

    #endregion

    #region Bind repeater control for shopping cart and issueance policy item

    private void BindRepeaterForItem()
    {
        if (this.sProcess == "sc") // Shopping Process
        {
            objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectShoppingProduct(IncentexGlobal.CurrentMember.UserInfoID);

            if (objSelectMyShoppingCartProductResultList.Count > 0)
            {
                lblrptMsg.Text = "";

                rptMyShoppingCart.Visible = true;
                rptMyShoppingCart.DataSource = objSelectMyShoppingCartProductResultList;
                rptMyShoppingCart.DataBind();
            }
            else
            {
                lblrptMsg.Text = "No Records Found";
                rptMyShoppingCart.Visible = false;
            }
            rptMyIssuanceCart.Visible = false;

            //CalculateOrderItemDetail();
        }
        else    // Uniform Issuance Process
        {
            List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssuanceCartRepository.GetMyIssuanceProdList(null, IncentexGlobal.CurrentMember.UserInfoID, false);
            rptMyShoppingCart.Visible = false;
            rptMyIssuanceCart.Visible = true;
            rptMyIssuanceCart.DataSource = objFinal;
            rptMyIssuanceCart.DataBind();

            foreach (RepeaterItem item in rptMyIssuanceCart.Items)
            {
                ((HtmlImage)item.FindControl("imgSplashImage")).Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + ((HiddenField)item.FindControl("hdndocumentname")).Value + "&_twidth=145&_theight=198";
                ((HtmlAnchor)item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/" + ((HiddenField)item.FindControl("hdnlargerimagename")).Value;

            }
            //CalculateOrderItemDetail();
        }
    }

    /// <summary>
    /// Calculates Changes in amount based on quantity
    /// </summary>
    private void CalculateOrderItemDetail()
    {
        try
        {
            Decimal totalRunning = 0;
            Decimal totalNotEligibleAnniversary = 0;
            if (this.sProcess == "sc")
            {
                foreach (RepeaterItem item in rptMyShoppingCart.Items)
                {
                    Label lblRunCharge = (Label)item.FindControl("lblRunCharge");
                    Label lblUnitPrice = (Label)item.FindControl("lblUnitPrice");
                    Label lblQuantity = (Label)item.FindControl("lblQuantity");
                    Label lblTotal = (Label)item.FindControl("lblTotal");
                    Label lblTotalPrice = (Label)rptMyShoppingCart.Controls[rptMyShoppingCart.Controls.Count - 1].Controls[0].FindControl("lblTotalProductPrice");
                    Label lblTotalShippingCharge = (Label)rptMyShoppingCart.Controls[rptMyShoppingCart.Controls.Count - 1].Controls[0].FindControl("lblTotalShippingCharge");
                    HiddenField lblsLookupName = (HiddenField)item.FindControl("hdnAnniversary");
                    //Add Nagmani 18-April-2012 
                    String strtotal;
                    //End
                    if (lblsLookupName.Value == "No")
                    {
                        if (lblQuantity.Text != "" || lblUnitPrice.Text != "")
                        {
                            if (!String.IsNullOrEmpty(lblRunCharge.Text))
                            {
                                lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                                //Update Nagmani 18-April-2012 
                                strtotal = ((Convert.ToDecimal(lblUnitPrice.Text) + Convert.ToDecimal(lblRunCharge.Text)) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                                //totalNotEligibleAnniversary += Convert.ToDecimal(lblTotal.Text);

                            }
                            else
                            {
                                lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                                //Add Nagmani 18-April-2012 
                                strtotal = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                            }
                            //Add Nagmani 18-April-2012 
                            totalNotEligibleAnniversary += Convert.ToDecimal(strtotal);
                        }
                    }
                    else
                    {
                        if (lblQuantity.Text != "" || lblUnitPrice.Text != "")
                        {
                            //Add nagmani 18-04-12
                            if (!String.IsNullOrEmpty(lblRunCharge.Text))
                            {
                                lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                                strtotal = ((Convert.ToDecimal(lblUnitPrice.Text) + Convert.ToDecimal(lblRunCharge.Text)) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                                //totalRunning += Convert.ToDecimal(lblTotal.Text);
                            }
                            else
                            {

                                //Add nagmani 18-04-12
                                lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                                strtotal = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(lblQuantity.Text)).ToString();
                            }
                            totalRunning += Convert.ToDecimal(strtotal);
                        }
                    }
                    lblTotalPrice.Text = (totalNotEligibleAnniversary + totalRunning).ToString("#,##0.00");
                    lblTotalShippingCharge.Text = this.ShippingAmount.ToString();
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
                    HtmlTableCell tdFooterTotal = (HtmlTableCell)rptMyIssuanceCart.Controls[rptMyIssuanceCart.Controls.Count - 1].Controls[0].FindControl("tdFooterTotal");
                    Label lblTotalShippingCharge = (Label)rptMyIssuanceCart.Controls[rptMyIssuanceCart.Controls.Count - 1].Controls[0].FindControl("lblTotalShippingCharge");
                    HtmlTableCell tdFooterShipping = (HtmlTableCell)rptMyIssuanceCart.Controls[rptMyIssuanceCart.Controls.Count - 1].Controls[0].FindControl("tdFooterShipping");
                    if ((lblQuantity.Text != "" || lblUnitPrice.Text != "") && lblTotal.Visible == true)
                    {
                        lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(lblQuantity.Text)).ToString();

                    }//display only when having property to display price is true
                    if (lblTotal.Visible == true)
                    {
                        Decimal TotalAmount = 0;
                        //TotalAmount += Session["SingleTotal"] != null ? Decimal.Parse(Session["SingleTotal"].ToString()) : 0;
                        //TotalAmount += Session["GroupTotal"] != null ? Decimal.Parse(Session["GroupTotal"].ToString()) : 0;
                        //TotalAmount += Session["GroupBudgetTotal"] != null ? Decimal.Parse(Session["GroupBudgetTotal"].ToString()) : 0;
                        TotalAmount = Session["TotalAmount"] != null ? Decimal.Parse(Session["TotalAmount"].ToString()) : 0;
                        lblTotalPrice.Text = TotalAmount.ToString("#,##0.00");
                        lblTotalShippingCharge.Text = this.ShippingAmount.ToString();
                    }
                    else
                    {
                        tdFooterTotal.Visible = false;
                        tdFooterShipping.Visible = false;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            ex.Message.ToString();
        }
    }

    #endregion

    #region Order confirmation process

    /// <summary>
    /// this process is for final order setup and transfer page based on payment option selected and update inventary also.
    /// </summary>
    public String OrderConfirmProcess()
    {
        String PaymentMethod = String.Empty;
        String OrderNumber = String.Empty;
        if (this.PaymentOrder != null)
        {
            PaymentMethod = this.PaymentOrder;
        }
        else
        {
            PaymentMethod = "Order and Shipping Paid by Corporate.";
        }

        MyShoppinCart objShoppingcart = new MyShoppinCart();
        ProductItem objProductItem = new ProductItem();
        ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
        MyIssuanceCart objIssuance = new MyIssuanceCart();
        UserInformation objUsrInfo = objUserInformationRepository.GetById(IncentexGlobal.CurrentMember.UserInfoID);

        //start order confirm process
        if (Session["Redirection"] != null)
        {
            if (Session["Redirection"].ToString() == "Payment")
            {
                //store all information of card in global session 'PaymentDetails'
                //Response.Redirect("PaymentDetails.aspx");
                Order objOrder = new Order();
                CompanyEmployee objCompEmp = new CompanyEmployee();
                objCompEmp = objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

                objOrder = objOrderConfirmationRepository.GetRecordByOrderId(Convert.ToInt64(Session["NewOrderID"]), objCompEmp.WorkgroupID);

                //Assign Billing & Shipping Detail & Other Credit Card Details Here
                PaymentInfo objPaymentInfo = new PaymentInfo();
                if (objOrder.CreditUsed != null)
                {
                    if (objOrder.CreditAmt != 0)
                    {
                        objPaymentInfo.OrderAmountToPay = (Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax) - Convert.ToDecimal(objOrder.CorporateDiscount)) - (Convert.ToDecimal(objOrder.CreditAmt));
                    }
                    else
                    {
                        objPaymentInfo.OrderAmountToPay = (Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax) - Convert.ToDecimal(objOrder.CorporateDiscount));
                    }
                }
                else
                {
                    objPaymentInfo.OrderAmountToPay = (Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax) - Convert.ToDecimal(objOrder.CorporateDiscount));
                }
                objPaymentInfo.OrderNumber = objOrder.OrderNumber;
                //get selected cardtype
                String CardType = String.Empty;
                if (rdbMasterCard.Checked == true)
                    CardType = "MasterCard";
                else if (rdbVisaCard.Checked == true)
                    CardType = "Visa";
                else if (rdbDiscoder.Checked == true)
                    CardType = "Discover";
                else if (rdbAmericanExpress.Checked == true)
                    CardType = "Amex";

                #region 1 - store credit card detail to payment information
                objPaymentInfo.CardNumber = txtCreditCardNumber.Text;
                objPaymentInfo.CardType = CardType;
                objPaymentInfo.ExpiresOnMonth = ddlCreditCardExpirationMonth.SelectedValue;
                objPaymentInfo.ExpiresOnYear = ddlCreditCardExpirationYear.SelectedValue;
                objPaymentInfo.CardVerification = txtCVVNumber.Text;
                #endregion

                #region 2 - store billing information to payment information
                CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfo();
                if (this.sProcess == "ui")
                {
                    //CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfoRepository().GetBillingDetailById(IncentexGlobal.CurrentMember.UserInfoID);
                    objBillingInfo = objCompanyEmployeeContactInfoRepository.GetBillingDetailIssuanceAddress(IncentexGlobal.CurrentMember.UserInfoID, objOrder.OrderID);
                }
                else
                {
                    objBillingInfo = objCompanyEmployeeContactInfoRepository.GetBillingDetailAddress(IncentexGlobal.CurrentMember.UserInfoID, objOrder.OrderID);
                }

                objPaymentInfo.B_FirstName = objBillingInfo.BillingCO;
                objPaymentInfo.B_LastName = objBillingInfo.Manager;
                objPaymentInfo.B_CompanyName = objBillingInfo.CompanyName;
                objPaymentInfo.B_StreetAddress1 = objBillingInfo.Address;
                objPaymentInfo.B_StreetAddress2 = objBillingInfo.Address2;
                objPaymentInfo.B_City = new CityRepository().GetById((Int64)objBillingInfo.CityID).sCityName;
                objPaymentInfo.B_State = new StateRepository().GetById((Int64)objBillingInfo.StateID).sStatename;
                objPaymentInfo.B_Zipcode = objBillingInfo.ZipCode;
                objPaymentInfo.B_PhoneNumber = objBillingInfo.Telephone;
                objPaymentInfo.B_Email = objBillingInfo.Email;
                #endregion

                #region 3 - store shipping information to payment info
                CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfo();
                if (this.sProcess == "ui")
                {
                    //CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailById(IncentexGlobal.CurrentMember.UserInfoID, objOrder.ShippingInfromationid);
                    objShippingInfo = objCompanyEmployeeContactInfoRepository.GetShippingDetailAddress(IncentexGlobal.CurrentMember.UserInfoID, objOrder.OrderID, "Shipping", "IssuancePolicy");
                }
                else
                {
                    objShippingInfo = objCompanyEmployeeContactInfoRepository.GetShippingDetailAddress(IncentexGlobal.CurrentMember.UserInfoID, objOrder.OrderID, "Shipping", "ShoppingCart");
                }

                objPaymentInfo.S_FirstName = objShippingInfo.Name;
                objPaymentInfo.S_LastName = objShippingInfo.Fax;
                objPaymentInfo.S_CompanyName = objShippingInfo.CompanyName;
                objPaymentInfo.S_StreetAddress1 = objShippingInfo.Address;
                objPaymentInfo.S_StreetAddress2 = objShippingInfo.Address2 + Environment.NewLine + objShippingInfo.Street;
                objPaymentInfo.S_City = new CityRepository().GetById((Int64)objShippingInfo.CityID).sCityName;
                objPaymentInfo.S_State = new StateRepository().GetById((Int64)objShippingInfo.StateID).sStatename;
                objPaymentInfo.S_Zipcode = objShippingInfo.ZipCode;
                objPaymentInfo.S_PhoneNumber = objShippingInfo.Telephone;
                objPaymentInfo.S_Email = objShippingInfo.Email;
                #endregion

                //4.Assign Value to Global Payment Class
                //If Its already Assigned then make it NULL first..
                if (IncentexGlobal.PaymentDetails != null)
                {
                    IncentexGlobal.PaymentDetails = null;
                }
                IncentexGlobal.PaymentDetails = objPaymentInfo;

                DoDirectPaymentCode();
                //Response.Redirect("~/Payment/Payflow.aspx");

                OrderNumber = objOrder.OrderNumber;
            }
            else
            {
                if (this.sProcess == "ui")
                {
                    if (Session["NewOrderID"] != null)
                    {
                        #region Update Order Table

                        //Add this by mayur for moas in issuance policy
                        objOrderConfirmationRepository = new OrderConfirmationRepository();
                        Order objOrder = objOrderConfirmationRepository.GetByOrderID(Convert.ToInt64(Session["NewOrderID"]));

                        //when paymentMethod is moas then insert order status as 'Waiting For Review'.
                        if (Session["PaymentOption"] != null && (Int32)(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.MOAS) == Convert.ToInt32(Session["PaymentOption"]))
                        {
                            objOrder.OrderStatus = "Order Pending";
                            objOrder.PaymentOption = objLookupRepository.GetIdByLookupNameNLookUpCode("MOAS", "WLS Payment Option");
                        }//This is for Employee pay MOAS option
                        else if (Session["PaymentOption"] != null && (Int32)(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.EmployeePays) == Convert.ToInt32(Session["PaymentOption"]) && (PaymentMethod == "MOAS" || PaymentMethod == "Replacement Uniforms"))
                        {
                            objOrder.OrderStatus = "Order Pending";
                        }
                        else
                        {
                            objOrder.OrderStatus = "Open";
                        }
                        objOrder.IsPaid = true;
                        try
                        {
                            objOrderConfirmationRepository.SubmitChanges();
                        }
                        catch { }
                        #endregion

                        #region Decrease the Inventroy  Here if Order is confirmed
                        List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssuanceCartRepository.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);
                        Boolean IsUpdateUserReactivatedPolicy = false;
                        foreach (SelectMyIssuanceProductItemsResult u in objFinal)
                        {
                            if (u != null)
                            {
                                if (!IsUpdateUserReactivatedPolicy)
                                {
                                    new ChangeStatusRepository().UpdateUserReActivatedIssuancePolicy(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(u.UniformIssuancePolicyID));
                                    IsUpdateUserReactivatedPolicy = true;
                                }

                                if (u.SupplierID != this.WTDCSupplierID)
                                {
                                    objIssuance = objMyIssuanceCartRepository.GetById(u.MyIssuanceCartID, objUsrInfo.UserInfoID);
                                    List<SelectProductIDResult> objList = new List<SelectProductIDResult>();
                                    objList = objProItemRepos.GetProductId(objIssuance.MasterItemID, u.MyIssuanceCartID, Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(this.StoreID));

                                    for (Int32 i = 0; i < objList.Count; i++)
                                    {
                                        String strProcess = "UniformIssuance";
                                        String strMessage = objOrderConfirmationRepository.UpdateInventory(u.MyIssuanceCartID, Convert.ToInt64(objList[i].ProductItemID), strProcess);
                                    }
                                }
                            }
                        }
                        #endregion

                        OrderNumber = objOrder.OrderNumber;
                    }
                }
                else
                {
                    if (Session["NewOrderID"] != null)
                    {
                        #region Update Order Table
                        objOrderConfirmationRepository = new OrderConfirmationRepository();
                        Order objOrder = objOrderConfirmationRepository.GetByOrderID(Convert.ToInt64(Session["NewOrderID"]));

                        //Added by mayur on 22-nov-2011 for MOAS module
                        //when paymentMethod is moas then insert order status as 'Waiting For Review'.
                        if (PaymentMethod == "MOAS" || PaymentMethod == "Replacement Uniforms")
                        {
                            objOrder.OrderStatus = "Order Pending";
                        }
                        else
                        {
                            objOrder.OrderStatus = "Open";
                        }
                        objOrder.IsPaid = true;
                        try
                        {
                            objOrderConfirmationRepository.SubmitChanges();
                        }
                        catch { }
                        #endregion

                        #region Decrease the Inventroy  Here if Order is confirmed

                        objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                        foreach (SelectMyShoppingCartProductResult u in objSelectMyShoppingCartProductResultList)
                        {
                            if (u != null)
                            {
                                if (u.SupplierID != this.WTDCSupplierID)
                                {
                                    objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(u.StoreProductID), Convert.ToInt64(u.MasterItemNo), u.item);

                                    String strProcess = "Shopping";
                                    String strMessage = objOrderConfirmationRepository.UpdateInventory(u.MyShoppingCartID, objProductItem.ProductItemID, strProcess);
                                }
                            }
                        }

                        #endregion

                        OrderNumber = objOrder.OrderNumber;
                    }
                }
                // here set true for cart abandonment when order is complete
                new Common().SetIsCartAbandonment(true);
                mvCheckoutSteps.ActiveViewIndex = 4;
            }
        }
        else
        {
            // here set true for cart abandonment when order is complete
            new Common().SetIsCartAbandonment(true);
            mvCheckoutSteps.ActiveViewIndex = 4;
        }

        return OrderNumber;
    }

    #endregion

    #region Fill the moas approver for current login user

    private void FillMOASPanelData(List<GetMOASApproverForCEResult> objlstMOASApprover)
    {
        //Updated by Prashant April 2013 (Removed the old logic for biding MOAS Approver and added SP to get the list of the same)
        //List<GetMOASApproverForCEResult> objlstMOASApprover = objCompanyEmployeeRepository.GetMOASApprover(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.CompanyId, 0);

        if (objlstMOASApprover != null && objlstMOASApprover.Count > 0)
        {
            StringBuilder strMOASManagerApproverHtml = new StringBuilder();
            for (Int32 i = 0; i < objlstMOASApprover.Count(); i++)
            {
                strMOASManagerApproverHtml.Append("<dl><dt>");
                if (objlstMOASApprover[i].ApproverLevel.ToLower() == "companylevel")
                    strMOASManagerApproverHtml.Append("<label for='name'>Level " + (i + 1) + " Approver</label>");
                else
                    strMOASManagerApproverHtml.Append("<label for='name'>Approver " + (i + 1) + "</label>");
                strMOASManagerApproverHtml.Append("</dt><dd>");
                strMOASManagerApproverHtml.Append("<div class='card_select_box'>");
                strMOASManagerApproverHtml.Append("<label style='margin-left:10px;'>" + objlstMOASApprover[i].FirstName + " " + objlstMOASApprover[i].LastName + "</label>");
                strMOASManagerApproverHtml.Append("</div>");
                strMOASManagerApproverHtml.Append("</dd></dl>");
            }

            hfMOASApproverLevelID.Value = Convert.ToString(objlstMOASApprover.FirstOrDefault().MOASApproverLevelID);
            dvMOASManagerApprover.InnerHtml = strMOASManagerApproverHtml.ToString();
            strMOASManagerApproverHtml.Remove(0, strMOASManagerApproverHtml.Length); //cleanup code
        }
        else //if no one approver is there then display text
            dvMOASManagerApprover.InnerHtml = "<div style='color:Red;font-size:15px;margin:25px;min-height:80px;'>No Approver Found. Please contact administrator to assign an approver.</div>";
        //Updated by Prashant April 2013
    }

    #endregion

    #region Anniversary credit li visible based on condition

    private void AnniversaryCreditVisibleTrueFalse()
    {
        if (this.sProcess == "sc")
        {
            if (lblAnniversaryCreditBallance.Text != "$0.00" && Convert.ToDecimal(lblAnniversaryCreditBallance.Text.Trim()) > 0)
                liAnniversaryCredits.Visible = true;
            else
                liAnniversaryCredits.Visible = false;
        }
        else
            liAnniversaryCredits.Visible = false;
    }

    #endregion

    #region For tracking page history

    private void GetPurchaseUpdate()
    {
        new UserTrackingRepo().SetPurchase(Convert.ToInt32(Session["trackID"]));
    }

    #endregion

    #region Paypal Methods

    public void DoDirectPaymentCode()
    {
        if (IncentexGlobal.PaymentDetails != null)
        {

            CityRepository objCity = new CityRepository();
            StateRepository objState = new StateRepository();
            CountryRepository objCountry = new CountryRepository();

            this.objPaymentInfo = IncentexGlobal.PaymentDetails;

            String[] a = this.objPaymentInfo.ExpiresOnYear.Split('/');
            String expirymonth = Convert.ToInt32(this.objPaymentInfo.ExpiresOnMonth).ToString("00");
            String expirydate = this.objPaymentInfo.ExpiresOnYear;

            //get ipaddress of user
            String ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!String.IsNullOrEmpty(ip))
            {
                String[] ipRange = ip.Split(',');
                ip = ipRange[0];
            }
            else
            {
                ip = Request.ServerVariables["REMOTE_ADDR"];
            }

            System.Guid uid = System.Guid.NewGuid();

            NVPCallerServices caller = new NVPCallerServices();
            IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();
            /*
             WARNING: Do not embed plaintext credentials in your application code.
             Doing so is insecure and against best practices.
             Your API credentials must be handled securely. Please consider
             encrypting them for use in any production environment, and ensure
             that only authorized individuals may view or modify them.
             */

            // Set up your API credentials, PayPal end point, API operation and version.
            /*Live Credential*/
            profile.APIUsername = "lbowman_api1.incentex.com";
            profile.APIPassword = "SY29D7VQEDERF8UN";
            profile.APISignature = "AFcWxV21C7fd0v3bYYYRCpSSRl31A72PPxEc7HPA.AivfXoRdpPmyelW";
            profile.Environment = "live";
            profile.Subject = "";

            /*Sandbox Credential*/
            //profile.APIUsername = "mayur._1329902388_biz_api1.indianic.com";
            //profile.APIPassword = "XRVFGKHRTLQ2CG33";
            //profile.APISignature = "ANKE9qAUIenRiCNEiwogMNiblbIaAHjwg450-G-55blbYB2HERgP5gxJ";
            //profile.Environment = "sandbox";
            //profile.Subject = "";

            caller.APIProfile = profile;

            NVPCodec encoder = new NVPCodec();
            encoder["VERSION"] = "64.0";
            encoder["METHOD"] = "DoDirectPayment";
            encoder["PAYMENTACTION"] = "Authorization";
            encoder["AMT"] = this.objPaymentInfo.OrderAmountToPay.ToString();
            encoder["DESC"] = "Order Number :" + this.objPaymentInfo.OrderNumber;
            encoder["CREDITCARDTYPE"] = this.objPaymentInfo.CardType;
            encoder["ACCT"] = this.objPaymentInfo.CardNumber;
            encoder["EXPDATE"] = expirymonth + expirydate;
            encoder["CVV2"] = this.objPaymentInfo.CardVerification;
            encoder["COUNTRYCODE"] = "US";
            encoder["CURRENCYCODE"] = "USD";
            encoder["IPADDRESS"] = ip;

            /*Billing Information*/
            encoder["FIRSTNAME"] = this.objPaymentInfo.B_FirstName;
            encoder["LASTNAME"] = this.objPaymentInfo.B_LastName;
            encoder["STREET"] = this.objPaymentInfo.B_StreetAddress1;
            encoder["CITY"] = this.objPaymentInfo.B_City;
            encoder["STATE"] = this.objPaymentInfo.B_State;
            encoder["ZIP"] = this.objPaymentInfo.B_Zipcode;
            encoder["SHIPTOPHONENUM"] = this.objPaymentInfo.B_PhoneNumber;
            encoder["EMAIL"] = this.objPaymentInfo.B_Email != null && this.objPaymentInfo.B_Email != "" ? this.objPaymentInfo.B_Email : IncentexGlobal.CurrentMember.LoginEmail;

            /*Shipping Information*/
            encoder["SHIPTONAME"] = (this.objPaymentInfo.S_FirstName + " " + this.objPaymentInfo.S_LastName).Length > 31 ? (this.objPaymentInfo.S_FirstName + " " + this.objPaymentInfo.S_LastName).Substring(0, 31) : (this.objPaymentInfo.S_FirstName + " " + this.objPaymentInfo.S_LastName);
            encoder["SHIPTOSTREET"] = this.objPaymentInfo.S_StreetAddress1 + Environment.NewLine + this.objPaymentInfo.S_StreetAddress2;
            encoder["SHIPTOCITY"] = this.objPaymentInfo.S_City;
            encoder["SHIPTOSTATE"] = this.objPaymentInfo.S_State;
            encoder["SHIPTOZIP"] = this.objPaymentInfo.S_Zipcode;
            encoder["SHIPTOPHONENUM"] = this.objPaymentInfo.S_PhoneNumber;

            //clear the session for PaymentDetails
            IncentexGlobal.PaymentDetails = null;

            // Execute the API operation and obtain the response.
            String pStrrequestforNvp = encoder.Encode();
            String pStresponsenvp = caller.Call(pStrrequestforNvp);

            NVPCodec decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            String RespMsg = decoder["ACK"];

            if (RespMsg != null && (RespMsg == "Success" || RespMsg == "SuccessWithWarning"))
            {
                //Need to Save the data with response message.
                CompletePaymentProcess(decoder["TRANSACTIONID"]);
                mvCheckoutSteps.ActiveViewIndex = 4;
            }
            else
            {
                Response.Redirect("~/Payment/PaymentFail.aspx?msg=" + decoder["L_LONGMESSAGE0"].Replace("\n", "<br/>"), false);
            }
        }
    }

    private void CompletePaymentProcess(String TransactionIdFromGateway)
    {
        ProductItem objProductItem = new ProductItem();
        ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
        MyIssuanceCart objIssuance = new MyIssuanceCart();

        objOrderConfirmationRepository = new OrderConfirmationRepository();

        Order objOrder = objOrderConfirmationRepository.GetByOrderID(Convert.ToInt64(Session["NewOrderID"]));
        objOrder.IsPaid = true;
        objOrder.PaymentTranscationNumber = TransactionIdFromGateway;
        objOrder.UpdatedDate = System.DateTime.Now;
        objOrder.OrderStatus = "Open";
        objOrderConfirmationRepository.SubmitChanges();

        objOrder = objOrderConfirmationRepository.GetByOrderID(Convert.ToInt64(Session["NewOrderID"]));

        //Decrease the Inventroy  Here if Order is confirmed
        if (objOrder.OrderFor == "ShoppingCart")
        {
            objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

            foreach (SelectMyShoppingCartProductResult u in objSelectMyShoppingCartProductResultList)
            {
                if (u != null)
                {
                    if (u.SupplierID != this.WTDCSupplierID)
                    {
                        objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(u.StoreProductID), Convert.ToInt64(u.MasterItemNo), u.item);

                        String strProcess = "Shopping";
                        String strMessage = objOrderConfirmationRepository.UpdateInventory(u.MyShoppingCartID, objProductItem.ProductItemID, strProcess);
                    }
                }
            }
        }
        else
        {
            List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssuanceCartRepository.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);
            foreach (SelectMyIssuanceProductItemsResult u in objFinal)
            {
                if (u != null)
                {
                    if (u.SupplierID != this.WTDCSupplierID)
                    {
                        objIssuance = objMyIssuanceCartRepository.GetById(u.MyIssuanceCartID, (Int64)objOrder.UserId);
                        List<SelectProductIDResult> objList = new List<SelectProductIDResult>();
                        objList = objProItemRepos.GetProductId(objIssuance.MasterItemID, u.MyIssuanceCartID, Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(this.StoreID));

                        for (Int32 i = 0; i < objList.Count; i++)
                        {
                            String strProcess = "UniformIssuance";
                            String strMessage = objOrderConfirmationRepository.UpdateInventory(u.MyIssuanceCartID, Convert.ToInt64(objList[i].ProductItemID), strProcess);
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Sales Tax Calculation

    private void GetSalesTax(String countryName, String stateName)
    {
        try
        {
            /// The web service definition for used in the Web Reference for this project is located at http://ws.strikeiron.com/taxdatabasic5?WSDL
            /// You can view more information about this web service here: https://strikeiron.com/ProductDetail.aspx?p=444

            /// Variables storing authentication values are declared below.  As a Registered StrikeIron user, you can authenticate
            /// to a StrikeIron web service with either a UserID/Password combination or a License Key.  If you wish to use a
            /// License Key, assign this value to the UserID field and set the Password field to null.
            /// 
            String userID = ConfigurationSettings.AppSettings["StrikeironUserID"];
            String password = ConfigurationSettings.AppSettings["StrikeironPassword"];

            /// To access the web service operations, you must declare a web service client Object.  This Object will contain
            /// all of the methods available in the web service and properties for each portion of the SOAP header.
            /// The class name for the web service client Object (assigned automatically by the Web Reference) is TaxDataBasic.
            /// 
            TaxDataBasic siService = new TaxDataBasic();

            /// StrikeIron web services accept user authentication credentials as part of the SOAP header.  .NET web service client
            /// objects represent SOAP header values as public fields.  The name of the field storing the authentication credentials
            /// is LicenseInfoValue (class type LicenseInfo).
            /// 
            LicenseInfo authHeader = new LicenseInfo();

            /// Registered StrikeIron users pass authentication credentials in the RegisteredUser section of the LicenseInfo Object.
            /// (property name: RegisteredUser; class name: RegisteredUser)
            /// 
            RegisteredUser regUser = new RegisteredUser();

            /// Assign credential values to this RegisteredUser Object
            /// 
            regUser.UserID = userID;
            regUser.Password = password;

            /// The populated RegisteredUser Object is now assigned to the LicenseInfo Object, which is then assigned to the web
            /// service client Object.
            /// 
            authHeader.RegisteredUser = regUser;
            siService.LicenseInfoValue = authHeader;

            /// Inputs for the GetTaxRateUS operation are declared below.
            /// 
            String zipCode = txtSZip.Text.Trim(); //this is the ZIP code to get for which the operation will return data

            if (countryName.ToUpper() == "UNITED STATES")
            {
                zipCode = zipCode.Contains("-") ? zipCode.Substring(0, zipCode.IndexOf("-")) : zipCode.Length == 9 ? zipCode.Substring(0, 5) : zipCode;

                /// The GetTaxRateUS operation can now be called.  The output type for this operation is SIWSOutputOfTaxRateUSAData.
                /// Note that for simplicity, there is no error handling in this sample project.  In a production environment, any
                /// web service call should be encapsulated in a try-catch block.
                /// 
                SIWsOutputOfTaxRateUSAData wsOutput = siService.GetTaxRateUS(zipCode);

                this.StrikeIronResponseFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_Strike_Iron_Response.xml";
                Common.SaveXMLFromObject(wsOutput, Path.Combine(Common.StrikeIronResponsePath, this.StrikeIronResponseFileName));

                /// The output objects of this StrikeIron web service contains two sections: ServiceStatus, which stores data
                /// indicating the success/failure status of the the web service request; and ServiceResult, which contains the
                /// actual data returne as a result of the request.
                /// 
                /// ServiceStatus contains two elements - StatusNbr: a numeric status code, and StatusDescription: a String
                /// describing the status of the output Object.  As a standard, you can apply the following assumptions for the value of
                /// StatusNbr:
                ///   200-299: Successful web service call (data found, etc...)
                ///   300-399: Nonfatal error (No data found, etc...)
                ///   400-499: Error due to invalid input
                ///   500+: Unexpected internal error; contact support@strikeiron.com
                if (wsOutput.ServiceStatus.StatusNbr >= 300)
                {
                    this.StrikeIronResponseFailed = true;
                    this.ShipToZipCodePassedToStrikeIron = zipCode;
                    return;
                }

                if (stateName.ToUpper() == "FLORIDA" || stateName.ToUpper() == "CONNECTICUT")
                {
                    this.StrikeIronTaxRate = Convert.ToDecimal(wsOutput.ServiceResult.TotalSalesTax);
                    this.SalesTaxAmount = Decimal.Round(((Session["TotalAmount"] != null ? Decimal.Parse(Session["TotalAmount"].ToString()) : 0) + Convert.ToDecimal(this.ShippingAmount)) * this.StrikeIronTaxRate, 2);
                }

                this.County = Convert.ToString(wsOutput.ServiceResult.County);
            }
            else if (countryName.ToUpper() == "CANADA")
            {
                /// Inputs for the GetTaxRateUS operation are declared below.

                /// The GetTaxRateCanada operation can now be called.  The output type for this operation is SIWsOutputOfTaxRateCanadaData.
                /// Note that for simplicity, there is no error handling in this sample project.  In a production environment, any
                /// web service call should be encapsulated in a try-catch block.
                /// 
                SIWsOutputOfTaxRateCanadaData wsOutput = siService.GetTaxRateCanada(stateName);

                this.StrikeIronResponseFileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_Strike_Iron_Response.xml";
                Common.SaveXMLFromObject(wsOutput, Path.Combine(Common.StrikeIronResponsePath, this.StrikeIronResponseFileName));

                /// The output objects of this StrikeIron web service contains two sections: ServiceStatus, which stores data
                /// indicating the success/failure status of the the web service request; and ServiceResult, which contains the
                /// actual data returne as a result of the request.
                /// 
                /// ServiceStatus contains two elements - StatusNbr: a numeric status code, and StatusDescription: a String
                /// describing the status of the output Object.  As a standard, you can apply the following assumptions for the value of
                /// StatusNbr:
                ///   200-299: Successful web service call (data found, etc...)
                ///   300-399: Nonfatal error (No data found, etc...)
                ///   400-499: Error due to invalid input
                ///   500+: Unexpected internal error; contact support@strikeiron.com
                if (wsOutput.ServiceStatus.StatusNbr >= 300)
                {
                    this.StrikeIronResponseFailed = true;
                    this.ShipToZipCodePassedToStrikeIron = zipCode;
                    return;
                }
                else
                {
                    this.StrikeIronTaxRate = Convert.ToDecimal(wsOutput.ServiceResult.GST);
                    this.SalesTaxAmount = Decimal.Round(((Session["TotalAmount"] != null ? Decimal.Parse(Session["TotalAmount"].ToString()) : 0) + Convert.ToDecimal(this.ShippingAmount)) * this.StrikeIronTaxRate, 2);
                }

                //this.County = Convert.ToString(wsOutput.ServiceResult.Abbreviation);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void GetSalesTaxForCanada(Int64 canadianStateID)
    {
        try
        {
            StateRepository objStateRepo = new StateRepository();
            TaxRatesForCanada objTaxRate = objStateRepo.GetTaxRateForCanadaByStateID(canadianStateID);

            if (objTaxRate != null)
            {
                this.StrikeIronTaxRate = Convert.ToDecimal(objTaxRate.TaxRate);
                this.SalesTaxAmount = Decimal.Round(((Session["TotalAmount"] != null ? Decimal.Parse(Session["TotalAmount"].ToString()) : 0) + Convert.ToDecimal(this.ShippingAmount)) * this.StrikeIronTaxRate, 2);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Integration with Magaya

    private void SendDataToMagaya(Order objOrder, CompanyEmployeeContactInfo objShippingInfo, CompanyEmployeeContactInfo objBillingInfo)
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
                List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssuanceCartRepository.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);
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
                String xmlTrans = beautify(createXmlDoc(objOrder.OrderID.ToString(), this.WTDCSupplierID, objShippingInfo, objBillingInfo).OuterXml);

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
            ErrHandler.WriteError(ex);
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
    public XmlDocument createXmlDoc(String OrderID, Int32 supplierid, CompanyEmployeeContactInfo objShippingInfo, CompanyEmployeeContactInfo objBillingInfo)
    {
        try
        {
            //Find UserName who had order purchased
            String strUserName = null;
            List<MyIssuanceCart> objMyIssCart = new List<MyIssuanceCart>();
            Order objOrder = new Order();
            objOrder = objOrderConfirmationRepository.GetByOrderID(Convert.ToInt64(OrderID));
            Int64 intUserId = Convert.ToInt64(objOrder.UserId);
            UserInformation objUsrInfo = objUserInformationRepository.GetById(intUserId);
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
            elem = addChildNode(xmlDoc, root, "BuyerShippingAddress");
            //addChildNode(xmlDoc, elem, "ContactName", objShippingInfo.CompanyName.ToString(), false);
            subElemt = addChildNode(xmlDoc, elem, "Street", objShippingInfo.CompanyName + " " + objShippingInfo.Address, false);
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
                Boolean showprice = checkissuancepolicy(objOrder);
                List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssuanceCartRepository.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);
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
            String a = NameBars(objOrder.OrderID);
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
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
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

    #region Send An Email To Supplier,IE and Customer Admin

    private void sendVerificationEmail(Int64 OrderID, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
    {
        try
        {
            CityRepository objCity = new CityRepository();
            StateRepository objState = new StateRepository();
            CountryRepository objCountry = new CountryRepository();
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                Order objOrder = objOrderConfirmationRepository.GetByOrderID(OrderID);
                UserInformation objUserInformation = objUserInformationRepository.GetById(objOrder.UserId);
                CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID);

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = objCompanyRepository.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();
                String sToadd = objUserInformation.LoginEmail;

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

                String PaymentMethod = String.Empty;
                if (objOrder.PaymentOption != null)
                    PaymentMethod = objLookupRepository.GetById((Int64)objOrder.PaymentOption).sLookupName;

                if (PaymentMethod == "MOAS" || PaymentMethod == "Replacement Uniforms")//start add by mayur on 24-nov-2011
                {
                    messagebody.Replace("{firstnote}", "Your requisition (" + objOrder.OrderNumber + ") has been submitted for review. Once the order is approved or if it is canceled we will notify you via email. You can also check the status of your requisition in your My Tracking Center.");
                }//end mayur
                else
                {
                    messagebody.Replace("{firstnote}", "Thank you for your business you can check the status of your order(s) any time by logging into your store <a href='http://www.world-link.us.com' title='Incentex'>here.</a>");
                }
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName + "<br/>Workgroup : " + objLookupRepository.GetById(objCompanyEmployee.WorkgroupID).sLookupName); // change by mayur on 26-March-2012
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus); //change by mayur on 22-nov-2011

                //  messagebody.Replace("{ReferenceName}", lblPurchasedBy.Text);
                messagebody.Replace("{FullName}", objUserInformation.FirstName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());
                String cr;
                Boolean creditused = true;
                //Check here for CreditUsed
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
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + PaymentMethod + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + PaymentMethod);
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
                    messagebody.Replace("{PaymentType}", PaymentMethod);
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + PaymentMethod + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
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
                // messagebody.Replace("{ShippingMethod}", "");

                //messagebody.Replace("{CreditType}", lblCreditType.Text);

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
                messagebody.Replace("{Billing Address}", "Billing Address");
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
                    messagebody.Replace("{Department1}", objLookupRepository.GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
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

                #region bind item detail
                String innermessage = "";
                Boolean GLCodeExists = false;

                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                    GLCodeExists = objSelectMyShoppingCartProductResultList.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                    {
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Quantity;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].item;
                        innermessage = innermessage + "</td>";
                        //innermessage = innermessage + "<td width='6%' style='font-weight: bold; text-align: center;'>";
                        //innermessage = innermessage + obj[i].color;
                        //innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objSelectMyShoppingCartProductResultList[i].Size;
                        innermessage = innermessage + "</td>";
                        //innermessage = innermessage + "<tr>";
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

                    messagebody.Replace("{ShippingCost}", !isFreeShipping ? objOrder.ShippingAmount.ToString() : "FREE SHIPPING.");
                    messagebody.Replace("{Saletax}", objOrder.SalesTax.ToString());
                    messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
                    messagebody.Replace("{CorporateDiscountView}", "");
                    messagebody.Replace("{CorporateDiscount}", "");
                }
                else
                {
                    Boolean showprice = checkissuancepolicy(objOrder);
                    List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssuanceCartRepository.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                    GLCodeExists = objFinal.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objFinal.Count; i++)
                    {
                        innermessage = innermessage + "<tr>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].Qty;
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='15%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].item;
                        innermessage = innermessage + "</td>";
                        //innermessage = innermessage + "<td width='6%' style='font-weight: bold; text-align: center;'>";
                        //innermessage = innermessage + objFinal[i].color;
                        //innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='13%' style='font-weight: bold; text-align: center;'>";
                        innermessage = innermessage + objFinal[i].sLookupName;
                        innermessage = innermessage + "</td>";
                        //innermessage = innermessage + "<tr>";
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
                        if (showprice)
                        {
                            innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate);
                        }
                        else
                        {
                            innermessage = innermessage + "---";
                        }
                        innermessage = innermessage + "</td>";
                        innermessage = innermessage + "<td width='8%' style='font-weight: bold;text-align: center;'>";
                        if (showprice)
                        {
                            innermessage = innermessage + "$" + Convert.ToDecimal(objFinal[i].Rate) * Convert.ToDecimal(objFinal[i].Qty);
                        }
                        else
                        {
                            innermessage = innermessage + "---";
                        }
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

                    if (showprice)
                    {
                        messagebody.Replace("{ShippingCost}", !isFreeShipping ? objOrder.ShippingAmount.ToString() : "FREE SHIPPING.");
                        messagebody.Replace("{Saletax}", objOrder.ShippingAmount.ToString());
                        messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
                        if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)//check that corporate discount is applied or not
                        {
                            messagebody.Replace("{CorporateDiscountView}", "Corporate Discount :");
                            messagebody.Replace("{CorporateDiscount}", objOrder.CorporateDiscount.ToString());
                        }
                        else
                        {
                            messagebody.Replace("{CorporateDiscountView}", "");
                            messagebody.Replace("{CorporateDiscount}", "");
                        }
                    }
                    else
                    {
                        messagebody.Replace("{ShippingCost}", "---");
                        messagebody.Replace("{Saletax}", "---");
                        messagebody.Replace("{OrderTotal}", "---");
                        messagebody.Replace("{CorporateDiscountView}", "");
                        messagebody.Replace("{CorporateDiscount}", "");
                    }
                }

                if (GLCodeExists)
                    messagebody.Replace("<td id=\"tdPD\" width=\"48%\"", "<td id=\"tdPD\" width=\"38%\"");
                else
                    messagebody.Replace(@"<td id=""tdGC"" width=""10%"" style=""font-weight: bold; text-align: center;"">
                                                    GL Code
                                                </td>", "");

                #endregion

                messagebody.Replace("{innermesaageforsupplier}", "");
                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "Shipping :");
                messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{OrderTotalView}", "Order Total :");
                String a = NameBars(objOrder.OrderID);
                if (a != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + a);
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                }

                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                //String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                //String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                //Email Management
                if (IncentexGlobal.IsIEFromStoreTestMode == true && IncentexGlobal.AdminUser != null) //This is for sending email to the user who is comming from view store front testing mode.
                {
                    new CommonMails().SendMail(IncentexGlobal.AdminUser.UserInfoID, "Order Confirmation", sFrmadd, IncentexGlobal.AdminUser.LoginEmail, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                }
                else
                {
                    new CommonMails().SendMail(objUserInformation.UserInfoID, "Order Confirmation", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                    //new CommonMails().SendEmail4Local(objUserInformation.UserInfoID, "Order Confirmation", sToadd, sSubject, messagebody.ToString(), Common.UserName, Common.Password, true, objOrder.OrderID);
                }
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private String NameBars(Int64 OrderId)
    {
        String strNameBars = String.Empty;
        Order objOrder = objOrderConfirmationRepository.GetByOrderID(OrderId);
        if (objOrder.OrderFor == "IssuanceCart")
        {
            List<MyIssuanceCart> objIssList = objMyIssuanceCartRepository.GetIssuanceCartByOrderID(objOrder.OrderID);
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

    private String NameBarBulkFile(Int64 OrderID)
    {
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

    private Boolean checkissuancepolicy(Order objOrder)
    {
        Boolean returnvalue = false;
        String[] firstitem = objOrder.MyShoppingCartID.Split(',');
        MyIssuanceCart objMyIssuanceCart = new MyIssuanceCartRepository().GetByIssuanceCartId(Convert.ToInt64(firstitem[0]));
        UniformIssuancePolicyItem objUniformIssuancePolicyItem = objUniformIssuancePolicyItemRepository.GetById(objMyIssuanceCart.UniformIssuancePolicyItemID);

        UniformIssuancePolicy objPolicy = new UniformIssuancePolicy();
        UniformIssuancePolicyRepository objPolicyRepo = new UniformIssuancePolicyRepository();
        INC_Lookup objLook = new INC_Lookup();
        objPolicy = objPolicyRepo.GetById(Convert.ToInt64(objUniformIssuancePolicyItem.UniformIssuancePolicyID));
        if (objPolicy != null)
        {
            objLook = objLookupRepository.GetById(Convert.ToInt64(objPolicy.PRICEFOR));

            if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Both")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    returnvalue = true;
                }
                else
                {
                    returnvalue = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Employee")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                {
                    returnvalue = true;
                }
                else
                {
                    returnvalue = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && objLook.sLookupName == "Customer Admin")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    returnvalue = true;
                }
                else
                {
                    returnvalue = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Admin")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                {
                    returnvalue = true;
                }
                else
                {
                    returnvalue = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Customer Employee")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    returnvalue = true;
                }
                else
                {
                    returnvalue = false;
                }
            }
            else if (Convert.ToString(objPolicy.SHOWPRICE) == "N" && objLook.sLookupName == "Both")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                {
                    returnvalue = false;
                }
                else
                {
                    returnvalue = true;
                }
            }
        }
        else
        {
            returnvalue = false;
        }

        return returnvalue;
    }

    private void sendTestOrderEmailNotification(CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
    {
        List<FUN_GetTestOrderEmailReceiversResult> objAdminList = new List<FUN_GetTestOrderEmailReceiversResult>();
        objAdminList = new IncentexBEDataContext().FUN_GetTestOrderEmailReceivers().ToList();
        if (objAdminList.Count > 0)
        {
            foreach (FUN_GetTestOrderEmailReceiversResult receiver in objAdminList)
            {
                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(receiver.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderConfirmations)) == true)
                    sendVerificationEmailAdmin(Convert.ToInt64(Session["NewOrderID"]), receiver.LoginEmail, receiver.FirstName, objBillingInfo, objShippingInfo, Convert.ToInt64(receiver.UserInfoID), true);
            }
        }
        //End
    }

    private void sendIEEmail(CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
    {
        List<UserInformation> objAdminList = new List<UserInformation>();
        objAdminList = objUserInformationRepository.GetEmailInformation();
        if (objAdminList.Count > 0)
        {
            for (Int32 i = 0; i < objAdminList.Count; i++)
            {
                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(objAdminList[i].UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderConfirmations)) == true)
                    sendVerificationEmailAdmin(Convert.ToInt64(Session["NewOrderID"]), objAdminList[i].LoginEmail, objAdminList[i].FirstName, objBillingInfo, objShippingInfo, objAdminList[i].UserInfoID, false);
            }
        }
        //End
    }

    private void sendVerificationEmailAdmin(Int64 OrderId, String IEemailAddress, String FullName, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo, Int64 UserInfoID, Boolean IsTestOrder)
    {
        try
        {
            CityRepository objCity = new CityRepository();
            StateRepository objState = new StateRepository();
            CountryRepository objCountry = new CountryRepository();
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                Order objOrder = objOrderConfirmationRepository.GetByOrderID(OrderId);
                UserInformation objUserInformation = objUserInformationRepository.GetById(objOrder.UserId);
                CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID);

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = objCompanyRepository.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber.ToString();
                String sToadd = IEemailAddress;
                Int64 sToUserInfoID = UserInfoID;

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

                String PaymentMethod = String.Empty;
                if (objOrder.PaymentOption != null)
                    PaymentMethod = objLookupRepository.GetById((Int64)objOrder.PaymentOption).sLookupName;

                messagebody.Replace("{firstnote}", "Thank you for your business you can check the status of your order(s) any time by logging into your store <a href='http://www.world-link.us.com' title='Incentex'>here.</a>");
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");

                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName + "<br/>Workgroup : " + objLookupRepository.GetById(objCompanyEmployee.WorkgroupID).sLookupName); // change by mayur on 26-March-2012
                messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);// change by mayur on 22-nov-2011
                // messagebody.Replace("{ReferenceName}", lblPurchasedBy.Text);
                messagebody.Replace("{OrderDate}", objOrder.OrderDate.ToString());
                // Set SAP Company Contact ID and Full name
                String OrderPlacedBy = String.Empty;
                String WLContactID = String.Empty;
                GetSAPCompanyCodeIDResult objSAPCompanyResult = objUserInformationRepository.GetSAPCompanyCodeID(IncentexGlobal.CurrentMember.UserInfoID);
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
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + PaymentMethod + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By " + PaymentMethod);
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
                    messagebody.Replace("{PaymentType}", PaymentMethod);
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to " + PaymentMethod + " : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
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

                //Billing Address
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

                //shipping address
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
                    messagebody.Replace("{Department1}", objLookupRepository.GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
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
                //
                #region start
                String innermessage = "";
                Boolean GLCodeExists = false;

                if (objOrder.OrderFor == "ShoppingCart")
                {
                    objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

                    GLCodeExists = objSelectMyShoppingCartProductResultList.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objSelectMyShoppingCartProductResultList.Count; i++)
                    {
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
                    List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssuanceCartRepository.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                    GLCodeExists = objFinal.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objFinal.Count; i++)
                    {
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
                String b = NameBars(objOrder.OrderID);
                if (b != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b + (IsTestOrder ? @"<br/><span style=""color:red;font-weight:bold;"">THIS IS A TEST ORDER. PLEASE DO NOT PROCESS.</span>" : ""));
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + (IsTestOrder ? @"<br/><span style=""color:red;font-weight:bold;"">THIS IS A TEST ORDER. PLEASE DO NOT PROCESS.</span>" : ""));
                }

                messagebody.Replace("{ShippingCost}", !isFreeShipping ? objOrder.ShippingAmount.ToString() : "FREE SHIPPING.");
                messagebody.Replace("{Saletax}", "$" + objOrder.SalesTax.ToString());
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
                if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)//check that corporate discount is applied or not
                {
                    messagebody.Replace("{CorporateDiscountView}", "Corporate Discount :");
                    messagebody.Replace("{CorporateDiscount}", objOrder.CorporateDiscount.ToString());
                }
                else
                {
                    messagebody.Replace("{CorporateDiscountView}", "");
                    messagebody.Replace("{CorporateDiscount}", "");
                }
                // messagebody.Replace("{PurchasesNotEligableforCreditUse}", lblNotEligibleforAnniversarycredit.Text);


                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                //--Saurabh || Name Bar Bulk Upload
                String NameBarList = NameBarBulkFile(objOrder.OrderID);
                String filePath = Server.MapPath("~/UploadedImages/NameBarFile/");
                List<UploadImage> objImage = new List<UploadImage>();
                if (NameBarList.Length > 0)
                {
                    List<String> myList = new List<String>(NameBarList.Split(','));
                    for (Int32 i = 0; i < myList.Count; i++)
                    {
                        UploadImage obj = new UploadImage();
                        obj.imageName = myList[i].ToString();
                        obj.imageOnly = myList[i].ToString();
                        objImage.Add(obj);
                    }
                }

                if (HttpContext.Current.Request.IsLocal)
                {
                    new CommonMails().SendMailWithMultiAttachment(1092, "Testing", "incentextest10@gmail.com", "surendar.yadav@indianic.com", sSubject, messagebody.ToString(), sFrmname, true, true, objImage, filePath, "smtp.gmail.com", 25, "incentextest10@gmail.com", "test10incentex", NameBarList, false);
                }
                else
                    new CommonMails().SendMailWithMultiAttachment(sToUserInfoID, "Order Confirmation", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, false, true, objImage, filePath, smtphost, smtpport, smtpUserID, smtppassword, NameBarList, true);
                //new CommonMails().SendMail(sToUserInfoID, "Order Confirmation", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);

            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendEmailToSupplier(Int64 orderid, CompanyEmployeeContactInfo objShippingInfo)
    {
        try
        {
            Order objOrder = objOrderConfirmationRepository.GetByOrderID(orderid);

            if (objOrder != null)
            {
                List<SelectSupplierAddressResult> obj = objOrderConfirmationRepository.GetSupplierAddress(objOrder.OrderFor, objOrder.OrderID);

                foreach (SelectSupplierAddressResult repeaterItem in obj)
                {
                    sendVerificationEmailSupplier(orderid, objOrder.MyShoppingCartID, repeaterItem.SupplierID, repeaterItem.Name, objShippingInfo, repeaterItem.CompanyName);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            throw ex;
        }
    }

    private void sendVerificationEmailSupplier(Int64 OrderId, String ShoppingCartID, Int64 supplierId, String fullName, CompanyEmployeeContactInfo objShippingInfo, String SupplierCompanyName)
    {
        try
        {
            //Get supplierinfo by id
            UserInformation objUserInfo = objUserInformationRepository.GetById(new SupplierRepository().GetById(supplierId).UserInfoID);

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
                        Order objOrder = objOrderConfirmationRepository.GetByOrderID(OrderId);
                        UserInformation objUserInformation = objUserInformationRepository.GetById(objOrder.UserId);

                        String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                        String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                        String sSubject = objCompanyRepository.GetById(Convert.ToInt64(objUserInformation.CompanyId)).CompanyName + " - " + objOrder.OrderNumber;
                        String sToadd = objUserInfo.LoginEmail;

                        StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                        StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());
                        messagebody.Replace("{firstnote}", "Please review the following order. If you have any questions please post them to the notes section of this order located within the order in the system.");
                        messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                        messagebody.Replace("{referencename}", objOrder.ReferenceName + "<br/>Workgroup : " + objLookupRepository.GetById((Int64)(objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID).WorkgroupID)).sLookupName); // change by mayur on 26-March-2012
                        messagebody.Replace("{OrderStatus}", objOrder.OrderStatus);

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

                        #region shipping address
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
                        #endregion

                        String innermessageSupplier = "";
                        Boolean GLCodeExists = false;

                        #region Supplier

                        List<SelectOrderDetailsForSupplierResult> lstSupplierItemsFromOrder = objOrderConfirmationRepository.GetOrderItemsForSupplier(supplierId, objOrder.OrderID);

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
                                innermessageSupplier = innermessageSupplier + "$" + Convert.ToDecimal(item.Price);
                                innermessageSupplier = innermessageSupplier + "</td>";
                                innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold;text-align: center;'>";
                                innermessageSupplier = innermessageSupplier + "$" + Convert.ToDecimal(item.Price) * Convert.ToDecimal(item.Quantity);
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
                        messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.SalesTax + objOrder.OrderAmount).ToString());

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

                        String c = NameBars(objOrder.OrderID);
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
                        String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                        String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                        //--Saurabh || Name Bar Bulk Upload
                        String NameBarList = NameBarBulkFile(objOrder.OrderID);
                        List<String> myList = new List<String>(NameBarList.Split(','));
                        String filePath = Server.MapPath("~/UploadedImages/NameBarFile/");
                        List<UploadImage> objImage = new List<UploadImage>();
                        for (Int32 i = 0; i < myList.Count; i++)
                        {
                            UploadImage obj = new UploadImage();
                            obj.imageName = myList[i].ToString();
                            obj.imageOnly = myList[i].ToString();
                            objImage.Add(obj);
                        }

                        //new CommonMails().SendEmail4Local(objUserInfo.UserInfoID, "Order Confirmation", sToadd, sSubject, messagebody.ToString(), Common.UserName, Common.Password, Common.SSL, objOrder.OrderID);
                        new CommonMails().SendMailWithMultiAttachment(objUserInfo.UserInfoID, "Order Confirmation", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, false, true, objImage, filePath, smtphost, smtpport, smtpUserID, smtppassword, NameBarList, true);
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
    /// Sends the email to MOAS managers.
    /// Add By Mayur on 22-Nov-2011
    /// </summary>
    /// <param name="orderNumber">The order number.</param>
    private void sendEmailToMOASManager(Int64 orderid, String OrderFor, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
    {
        //Updated by Prashant April 2013
        //List<CompanyEmployeeRepository.MOASUserWithPriority> objListMOASUserWithPriority = new List<CompanyEmployeeRepository.MOASUserWithPriority>();
        List<GetMOASApproverForCEResult> objlstMOASApprover = new List<GetMOASApproverForCEResult>();

        if (OrderFor == "ShoppingCart" || (OrderFor == "IssuanceCart" && Session["PaymentOption"] != null && Convert.ToInt64(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.EmployeePays) == Convert.ToInt64(Session["PaymentOption"]))) //For shopping cart MOAS
        {
            objlstMOASApprover = objCompanyEmployeeRepository.GetMOASApprover(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.CompanyId, 0);
        }
        else //For issuance policy MOAS
        {
            //this is for employee pay MOAS option
            objlstMOASApprover = objCompanyEmployeeRepository.GetMOASApprover(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.CompanyId, Convert.ToInt64(Session["PID"].ToString()));
        }

        //make order by priority and insert it into OrderMoasSystem table

        OrderMOASSystemRepository objOrderMOASSystemRepository = new OrderMOASSystemRepository();

        for (Int32 i = 0; i < objlstMOASApprover.Count(); i++)
        {
            OrderMOASSystem objOrderMOASSystem = new OrderMOASSystem()
            {
                OrderID = orderid,
                ManagerUserInfoID = Convert.ToInt64(objlstMOASApprover[i].UserInfoID),
                Priority = objlstMOASApprover[i].UserPriority != null ? Convert.ToInt64(objlstMOASApprover[i].UserPriority) : 1,
                Status = "Order Pending",
                DateAffected = null
            };
            objOrderMOASSystemRepository.Insert(objOrderMOASSystem);
            objOrderMOASSystemRepository.SubmitChanges();

            //send email to first priority manager
            if (i == 0 || (objlstMOASApprover[i].ApproverLevel.ToLower() == "stationlevel"))
            {
                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(objlstMOASApprover[i].UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderConfirmations)) == true)
                    sendVerificationEmailMOASManager(orderid, objlstMOASApprover[i].LoginEmail, objlstMOASApprover[i].FirstName, objlstMOASApprover[i].UserInfoID, objBillingInfo, objShippingInfo);
            }
        }
        //change end on 3-feb-2012 by mayur for send email priority wise
    }

    /// <summary>
    /// Sends the verification email MOAS manager for approve order.
    /// Add By Mayur on 22-Nov-2011
    /// </summary>
    /// <param name="OrderNumber">The order number.</param>
    /// <param name="MOASEmailAddress">The MOAS email address.</param>
    /// <param name="FullName">The full name.</param>
    private void sendVerificationEmailMOASManager(Int64 OrderId, String MOASEmailAddress, String FullName, Int64 MOASUserId, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
    {
        try
        {
            CityRepository objCity = new CityRepository();
            StateRepository objState = new StateRepository();
            CountryRepository objCountry = new CountryRepository();
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Placed MOAS";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                Order objOrder = objOrderConfirmationRepository.GetByOrderID(OrderId);
                UserInformation objUserInformation = objUserInformationRepository.GetById(objOrder.UserId);
                CompanyEmployee objCompanyEmployee = objCompanyEmployeeRepository.GetByUserInfoId(objUserInformation.UserInfoID);

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();
                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString().Replace("{FullName}", objUserInformation.FirstName + " " + objUserInformation.LastName).Replace("{OrderNumber}", objOrder.OrderNumber);
                String sToadd = MOASEmailAddress;
                Int64 sToUserInfoID = MOASUserId;

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                StringBuilder innermessagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString());

                messagebody.Replace("{firstnote}", "Please review the following order below that requires your attention. You can Approve, Edit or Cancel this order by clicking the buttons on the bottom of this page.");
                messagebody.Replace("{FullName}", FullName);
                messagebody.Replace("{EmployeeNumber}", objCompanyEmployee.EmployeeID);
                messagebody.Replace("{BaseStation}", objCompanyEmployee.BaseStation != null ? new BaseStationRepository().GetById(Convert.ToInt64(objCompanyEmployee.BaseStation)).sBaseStation : "");
                messagebody.Replace("{OrderNumber}", objOrder.OrderNumber);
                messagebody.Replace("{referencename}", objOrder.ReferenceName + "<br/>Workgroup : " + objLookupRepository.GetById(objCompanyEmployee.WorkgroupID).sLookupName); // change by mayur on 26-March-2012
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
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to MOAS : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
                    /*End*/
                    messagebody.Replace("{PaymentType}", cr + "& Additional Purchases Paid By MOAS");
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
                    messagebody.Replace("{PaymentType}", "MOAS");
                    /*Anniversary Credit Row*/
                    messagebody.Replace("{lblStrCrAmntAppliedLabel}", String.Empty);
                    messagebody.Replace("{lblStCrAmntApplied}", String.Empty);
                    /*End*/
                    /*Payment Option Row*/
                    messagebody.Replace("{lblPaymentMethodUsed}", "Applied to MOAS : ");
                    messagebody.Replace("{lblPaymentMethodAmount}", "$" + ((Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax)) - (objOrder.CreditAmt) - (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0 ? objOrder.CorporateDiscount : 0)).ToString());
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
                    messagebody.Replace("{Department1}", objLookupRepository.GetById(Convert.ToInt64(objShippingInfo.DepartmentID)).sLookupName);
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
                    List<SelectMyIssuanceProductItemsResult> objFinal = objMyIssuanceCartRepository.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                    GLCodeExists = objFinal.FirstOrDefault(le => le.GLCode != null) != null;

                    for (Int32 i = 0; i < objFinal.Count; i++)
                    {
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
                    messagebody.Replace(@"<td id=""tdGC"" width=""10%"" style=""font-weight: bold; text-align: center;"">GL Code</td>", "");

                #endregion

                messagebody.Replace("{Order Notes:}", "Order Notes :");
                messagebody.Replace("{ShippingCostView}", "Shipping :");
                messagebody.Replace("{SalesTaxView}", "Sales Tax :");
                messagebody.Replace("{OrderTotalView}", "Order Total :");
                messagebody.Replace("{innermesaageforsupplier}", "");
                String b = NameBars(objOrder.OrderID);
                if (b != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + b);
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                }

                messagebody.Replace("{ShippingCost}", !isFreeShipping ? objOrder.ShippingAmount.ToString() : "FREE SHIPPING.");
                messagebody.Replace("{Saletax}", "$" + objOrder.SalesTax.ToString());
                messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.ShippingAmount + objOrder.SalesTax + objOrder.OrderAmount).ToString());
                if (objOrder.CorporateDiscount != null && objOrder.CorporateDiscount != 0)//check that corporate discount is applied or not
                {
                    messagebody.Replace("{CorporateDiscountView}", "Corporate Discount :");
                    messagebody.Replace("{CorporateDiscount}", objOrder.CorporateDiscount.ToString());
                }
                else
                {
                    messagebody.Replace("{CorporateDiscountView}", "");
                    messagebody.Replace("{CorporateDiscount}", "");
                }
                // messagebody.Replace("{PurchasesNotEligableforCreditUse}", lblNotEligibleforAnniversarycredit.Text);

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
                String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();
                //--Saurabh || Name Bar Bulk Upload
                String NameBarList = NameBarBulkFile(objOrder.OrderID);
                List<String> myList = new List<String>(NameBarList.Split(','));
                String filePath = Server.MapPath("~/UploadedImages/NameBarFile/");
                List<UploadImage> objImage = new List<UploadImage>();
                for (Int32 i = 0; i < myList.Count; i++)
                {
                    UploadImage obj = new UploadImage();
                    obj.imageName = myList[i].ToString();
                    obj.imageOnly = myList[i].ToString();
                    objImage.Add(obj);
                }


                // new CommonMails().SendMail(sToUserInfoID, "Order Confirmation", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);
                new CommonMails().SendMailWithMultiAttachment(sToUserInfoID, "Order Confirmation", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, false, true, objImage, filePath, smtphost, smtpport, smtpUserID, smtppassword, NameBarList, true);


            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Fix Shipping Address Change Bind Payment Option as per user selection from the Step 1

    private void BindTagsAsPerSelectedPaymentOption()
    {
        txtPaymentOptionCode.Text = "";
        ddlCostCode.SelectedIndex = 0;
        if (ddlPaymentOption.SelectedValue == "0")
        {
            liPaymentOptionCode.Visible = false;
            liPaymentOptiondropdownCode.Visible = false;
            pnlCreditCardPayment.Visible = false;
            pnlMOASPayment.Visible = false;
            Session["Redirection"] = "Thankyou";
            GetBillingDetails();
            AnniversaryCreditVisibleTrueFalse();
            FillReasonForReplacement(false);
        }
        else if (ddlPaymentOption.SelectedItem.Text == "Credit Card")
        {
            liPaymentOptionCode.Visible = false;
            liPaymentOptiondropdownCode.Visible = false;
            pnlCreditCardPayment.Visible = true;
            pnlMOASPayment.Visible = false;
            Session["Redirection"] = "Payment";
            GetBillingDetails();
            AnniversaryCreditVisibleTrueFalse();
            FillReasonForReplacement(false);
        }
        else if (ddlPaymentOption.SelectedItem.Text == "Employee Payroll Deduct")
        {
            //Update billing Information here..Change on Billing address.created on 09-09-2011
            getsetBillingDetailsFromGlobalMenu();
            //End
            pnlCreditCardPayment.Visible = false;
            liPaymentOptionCode.Visible = false;
            liPaymentOptiondropdownCode.Visible = false;
            pnlMOASPayment.Visible = false;
            Session["Redirection"] = "Thankyou";
            AnniversaryCreditVisibleTrueFalse();
            FillReasonForReplacement(false);
        }
        else if (ddlPaymentOption.SelectedItem.Text == "MOAS" || ddlPaymentOption.SelectedItem.Text == "Replacement Uniforms") //start add by mayur on 25-nov-2011
        {
            liAnniversaryCredits.Visible = false;
            pnlMOASPayment.Visible = true;
            pnlCreditCardPayment.Visible = false;
            isAnniversaryCreditApply = false;
            Session["Redirection"] = "Thankyou";
            GetMOASBillingDetail();

            CompanyEmployeeRepository objCmpEmpRepo = new CompanyEmployeeRepository();

            if (ddlPaymentOption.SelectedItem.Text == "MOAS")
            {
                //Change for MOAS with cost center code by Devraj Gadhavi on 5th July, 2012                
                this.IsMOASWithCostCenterCode = objCmpEmpRepo.FUN_IsMOASWithCostCenterCode(IncentexGlobal.CurrentMember.UserInfoID);

                liPaymentOptionCode.Visible = false;
                if (this.IsMOASWithCostCenterCode)
                    liPaymentOptiondropdownCode.Visible = true;
                else
                    liPaymentOptiondropdownCode.Visible = false;
            }
            else if (ddlPaymentOption.SelectedItem.Text == "Replacement Uniforms")
            {
                Boolean IsReason4ReplacementRequired = objCmpEmpRepo.FUN_IsReason4ReplacementRequired(IncentexGlobal.CurrentMember.UserInfoID);
                FillReasonForReplacement(IsReason4ReplacementRequired);
            }

            //Change for MOAS with cost center code by Devraj Gadhavi on 5th July, 2012
        } //end
        else if (ddlPaymentOption.SelectedItem.Text == "Cost-Center Code")
        {
            liPaymentOptionCode.Visible = false;
            liPaymentOptiondropdownCode.Visible = true;
            pnlCreditCardPayment.Visible = false;
            pnlMOASPayment.Visible = false;
            Session["Redirection"] = "Thankyou";
            GetBillingDetails();
            AnniversaryCreditVisibleTrueFalse();
            FillReasonForReplacement(false);
        }
        else if (ddlPaymentOption.SelectedValue == Convert.ToInt64(Incentex.DAL.Common.DAEnums.PaymentOptions.PaidByCorporate).ToString())
        {
            liPaymentOptionCode.Visible = false;
            lblPaymentOptionCode.InnerText = ddlPaymentOption.SelectedItem.Text;
            liPaymentOptiondropdownCode.Visible = false;
            pnlCreditCardPayment.Visible = false;
            pnlMOASPayment.Visible = false;
            Session["Redirection"] = "Thankyou";
            GetBillingDetails();
            AnniversaryCreditVisibleTrueFalse();
            FillReasonForReplacement(false);
        }
        else
        {
            liPaymentOptionCode.Visible = true;
            lblPaymentOptionCode.InnerText = ddlPaymentOption.SelectedItem.Text;
            liPaymentOptiondropdownCode.Visible = false;
            pnlCreditCardPayment.Visible = false;
            pnlMOASPayment.Visible = false;
            Session["Redirection"] = "Thankyou";
            GetBillingDetails();
            AnniversaryCreditVisibleTrueFalse();
            FillReasonForReplacement(false);
        }

        IsCoseCenterCode = liPaymentOptiondropdownCode.Visible;
    }

    #endregion

    #endregion
}