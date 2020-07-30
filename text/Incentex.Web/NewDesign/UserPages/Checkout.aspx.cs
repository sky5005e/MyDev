using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.paypal.sdk.profiles;
using com.paypal.sdk.services;
using com.strikeiron.ws;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class UserPages_Checkout : PageBase
{
    #region Page Properties

    Decimal SubTotal
    {
        get
        {
            return Convert.ToDecimal(ViewState["SubTotal"]);
        }
        set
        {
            ViewState["SubTotal"] = value;
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

    Decimal ShippingAmount
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

    //Decimal PromoDiscountAmount
    //{
    //    get
    //    {
    //        return Convert.ToDecimal(ViewState["PromoDiscountAmount"]);
    //    }
    //    set
    //    {
    //        ViewState["PromoDiscountAmount"] = value;
    //    }
    //}

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

    Int64 WorkGroupID
    {
        get
        {
            return Convert.ToInt64(ViewState["WorkGroupID"]);
        }
        set
        {
            ViewState["WorkGroupID"] = value;
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

    String OrderFor
    {
        get
        {
            return Convert.ToString(ViewState["OrderFor"]);
        }
        set
        {
            ViewState["OrderFor"] = value;
        }
    }

    String CartIDs
    {
        get
        {
            return Convert.ToString(ViewState["CartIDs"]);
        }
        set
        {
            ViewState["CartIDs"] = value;
        }
    }

    #region Paging Properties

    Int32 NoOfRecordsToDisplay
    {
        get
        {
            return 5;
        }
        set
        {
            this.ViewState["NoOfRecordsToDisplay"] = value;
        }
    }

    Int32 PageIndex
    {
        get
        {
            if (Convert.ToString(this.ViewState["PageIndex"]) == "")
                return 0;
            else
                return Convert.ToInt32(this.ViewState["PageIndex"]);
        }
        set
        {
            this.ViewState["PageIndex"] = value;
        }
    }

    Int32 TotalPages
    {
        get
        {
            if (Convert.ToString(this.ViewState["TotalPages"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["TotalPages"]);
        }
        set
        {
            this.ViewState["TotalPages"] = value;
        }
    }

    Int32 NoOfPagesToDisplay
    {
        get
        {
            return 3;
        }
        set
        {
            this.ViewState["NoOfPagesToDisplay"] = value;
        }
    }

    Int32 FromPage
    {
        get
        {
            if (Convert.ToString(this.ViewState["FromPage"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["FromPage"]);
        }
        set
        {
            this.ViewState["FromPage"] = value;
        }
    }

    Int32 ToPage
    {
        get
        {
            if (Convert.ToString(this.ViewState["ToPage"]) == "")
                return this.NoOfPagesToDisplay;
            else
                return Convert.ToInt32(this.ViewState["ToPage"]);
        }
        set
        {
            this.ViewState["ToPage"] = value;
        }
    }

    #endregion

    FreeShipping StoreShippingDetails
    {
        get
        {
            return (FreeShipping)ViewState["StoreShippingDetails"];
        }
        set
        {
            ViewState["StoreShippingDetails"] = value;
        }
    }

    Int64 CompanyEmployeeID
    {
        get
        {
            return Convert.ToInt64(ViewState["CompanyEmployeeID"]);
        }
        set
        {
            ViewState["CompanyEmployeeID"] = value;
        }
    }

    Decimal AvailableAnniversaryCreditAmount
    {
        get
        {
            return Convert.ToDecimal(ViewState["AvailableAnniversaryCreditAmount"]);
        }
        set
        {
            ViewState["AvailableAnniversaryCreditAmount"] = value;
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

    #endregion

    #region Page Events

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (Convert.ToBoolean(Session["OrderCompleted"]) || Convert.ToBoolean(Session["OrderFailed"]))
        {
            LinkButton btnLogout = (LinkButton)this.Master.FindControl("lnklogOut");
            if (btnLogout != null && Request.Params["__EVENTTARGET"] != btnLogout.UniqueID)
                Response.Redirect("Index.aspx");
        }

        try
        {
            if (!IsPostBack)
            {
                GetInitialDetails();
                BindItems(true);
                FillDropDowns();
                ShowAnniversaryCredits(false);
            }

            rfvSAddressName.Enabled = chkAddShipping.Checked;
            rfvBAddressName.Enabled = chkAddBilling.Checked;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Control Events

    protected void repCartItems_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SelectMyShoppingCartProductResult objItem = (SelectMyShoppingCartProductResult)e.Item.DataItem;

                if (objItem != null)
                {
                    if (((List<SelectMyShoppingCartProductResult>)repCartItems.DataSource).Count == 1)
                    {
                        LinkButton lbRemoveItem = (LinkButton)e.Item.FindControl("lbRemoveItem");
                        lbRemoveItem.Visible = false;
                    }

                    Image imgProductImage = (Image)e.Item.FindControl("imgProductImage");
                    Label lblProductName = (Label)e.Item.FindControl("lblProductName");
                    Label lblProductQty = (Label)e.Item.FindControl("lblProductQty");
                    Label lblProductPrice = (Label)e.Item.FindControl("lblProductPrice");
                    Label lblProductSize = (Label)e.Item.FindControl("lblProductSize");

                    imgProductImage.ImageUrl = ConfigurationManager.AppSettings["ProductImagesThumbsPath"] + (!String.IsNullOrEmpty(objItem.ProductImage) ? objItem.ProductImage : "Thumb_ProductDefault.jpg");

                    lblProductName.Text = !String.IsNullOrEmpty(objItem.ProductDescrption1) && objItem.ProductDescrption1.Length > 15 ? objItem.ProductDescrption1.Substring(0, 12).Trim() + "..." : objItem.ProductDescrption1;
                    lblProductName.ToolTip = objItem.ProductDescrption1;

                    if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                    {
                        lblProductQty.Text = Convert.ToDecimal(objItem.UnitPrice).ToString("C2") + "<em>&nbsp;</em>QTY " + Convert.ToString(objItem.Quantity);
                        lblProductPrice.Text = Convert.ToDecimal(Convert.ToDecimal(objItem.UnitPrice) * Convert.ToDecimal(objItem.Quantity)).ToString("C2");
                    }
                    else
                    {
                        lblProductQty.Text = Convert.ToDecimal(objItem.MOASUnitPrice).ToString("C2") + "<em>&nbsp;</em>QTY " + Convert.ToString(objItem.Quantity);
                        lblProductPrice.Text = Convert.ToDecimal(Convert.ToDecimal(objItem.MOASUnitPrice) * Convert.ToDecimal(objItem.Quantity)).ToString("C2");
                    }

                    lblProductSize.Text = objItem.Size;
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void repCartItems_ItemCommand(Object sender, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "remove")
            {
                MyShoppingCartRepository objCartRepo = new MyShoppingCartRepository();
                MyShoppinCart objCartItem = objCartRepo.GetDetailsById(Convert.ToInt64(e.CommandArgument));
                objCartRepo.Delete(objCartItem);
                objCartRepo.SubmitChanges();
            }

            BindItems(true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    //protected void lbPromoApply_Click(Object sender, EventArgs e)
    //{
    //    try
    //    {
    //        this.PromotionCode = txtPromoCode.Text;
    //        this.PromoDiscountAmount = Decimal.Zero;
    //        SetTotal();
    //    }
    //    catch (Exception ex)
    //    {
    //        ErrHandler.WriteError(ex);
    //    }
    //}

    protected void ddlShippingAddressName_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(ddlShippingAddressName.SelectedValue))
            {
                Int64 shippingAddressID;
                Boolean shippingAddressSelected = Int64.TryParse(ddlShippingAddressName.SelectedValue, out shippingAddressID);

                if (shippingAddressSelected)
                {
                    CompanyEmployeeContactInfo objShippingInfo = new CompanyEmployeeContactInfoRepository().GetShippingDetailById(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(ddlShippingAddressName.SelectedValue));

                    if (objShippingInfo != null)
                    {
                        chkAddShipping.Checked = false;
                        rfvSAddressName.Enabled = chkAddShipping.Checked;

                        txtSCompany.Text = Convert.ToString(objShippingInfo.CompanyName);
                        txtSAddress1.Text = Convert.ToString(objShippingInfo.Address);
                        txtSAddress2.Text = Convert.ToString(objShippingInfo.Address2);
                        txtSFirstName.Text = Convert.ToString(objShippingInfo.Name);
                        txtSLastName.Text = Convert.ToString(objShippingInfo.Fax);

                        ddlSCountry.SelectedValue = Convert.ToString(objShippingInfo.CountryID);
                        ddlSCountry_SelectedIndexChanged(null, null);
                        ddlSState.SelectedValue = Convert.ToString(objShippingInfo.StateID);
                        ddlSState_SelectedIndexChanged(null, null);

                        if (objShippingInfo.CityID != null)
                            txtSCity.Text = new CityRepository().GetById(Convert.ToInt64(objShippingInfo.CityID)).sCityName;

                        txtSZipCode.Text = Convert.ToString(objShippingInfo.ZipCode);
                    }
                    else
                        ClearShippingAddress();
                }
            }
            else
                ClearShippingAddress();

            if (sender != null && !String.IsNullOrEmpty(ddlShippingAddressName.SelectedValue) && Convert.ToInt64(ddlShippingAddressName.SelectedValue) > 0)
                txtSFirstName.Focus();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlBillingAddressName_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(ddlBillingAddressName.SelectedValue))
            {
                Int64 billingAddressID;
                Boolean billingAddressSelected = Int64.TryParse(ddlBillingAddressName.SelectedValue, out billingAddressID);

                if (billingAddressSelected)
                {
                    CompanyEmployeeContactInfo objBillingInfo = new CompanyEmployeeContactInfoRepository().GetBillingDetailById(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(ddlBillingAddressName.SelectedValue));

                    if (objBillingInfo != null)
                    {
                        chkAddBilling.Checked = false;
                        rfvBAddressName.Enabled = chkAddBilling.Checked;

                        txtBCompany.Text = Convert.ToString(objBillingInfo.CompanyName);
                        txtBAddress1.Text = Convert.ToString(objBillingInfo.Address);
                        txtBAddress2.Text = Convert.ToString(objBillingInfo.Address2);
                        txtBFirstName.Text = Convert.ToString(objBillingInfo.BillingCO);
                        txtBLastName.Text = Convert.ToString(objBillingInfo.Manager);

                        ddlBCountry.SelectedValue = Convert.ToString(objBillingInfo.CountryID);
                        ddlBCountry_SelectedIndexChanged(null, null);
                        ddlBState.SelectedValue = Convert.ToString(objBillingInfo.StateID);
                        ddlBState_SelectedIndexChanged(null, null);

                        if (objBillingInfo.CityID != null)
                            txtBCity.Text = new CityRepository().GetById(Convert.ToInt64(objBillingInfo.CityID)).sCityName;

                        txtBZipCode.Text = Convert.ToString(objBillingInfo.ZipCode);
                    }
                    else
                        ClearBillingAddress();
                }
            }
            else
                ClearBillingAddress();

            if (sender != null && !String.IsNullOrEmpty(ddlBillingAddressName.SelectedValue) && Convert.ToInt64(ddlBillingAddressName.SelectedValue) > 0)
                txtBFirstName.Focus();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlSCountry_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            FillShippingState(ddlSCountry.SelectedValue);

            if (ddlSCountry.SelectedItem.Text == "United States")
            {
                revSZipCode.ValidationExpression = @"^\d{5}([-\s]{0,3}\d{4})?$";
            }
            else if (ddlSCountry.SelectedItem.Text == "Canada")
                revSZipCode.ValidationExpression = @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])[-\s]{0,3}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$";
            else
                revSZipCode.IsValid = true;

            if (sender != null && !String.IsNullOrEmpty(ddlSCountry.SelectedValue) && Convert.ToInt64(ddlSCountry.SelectedValue) > 0)
                ddlSState.Focus();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlSState_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(ddlSState.SelectedValue) && Convert.ToInt64(ddlSState.SelectedValue) > 0)
            {
                aceSCity.ContextKey = ddlSState.SelectedValue;

                if (sender != null)
                    txtSCity.Focus();
            }
            else
            {
                aceSCity.ContextKey = String.Empty;
                txtSCity.Text = String.Empty;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlBCountry_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            FillBillingState(ddlBCountry.SelectedValue);

            if (ddlBCountry.SelectedItem.Text == "United States")
                revBZipCode.ValidationExpression = @"^\d{5}([-\s]{0,3}\d{4})?$";
            else if (ddlBCountry.SelectedItem.Text == "Canada")
                revBZipCode.ValidationExpression = @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])[-\s]{0,3}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$";
            else
                revBZipCode.IsValid = true;

            if (sender != null && !String.IsNullOrEmpty(ddlBCountry.SelectedValue) && Convert.ToInt64(ddlBCountry.SelectedValue) > 0)
                ddlBState.Focus();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlBState_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(ddlBState.SelectedValue) && Convert.ToInt64(ddlBState.SelectedValue) > 0)
            {
                aceBCity.ContextKey = ddlBState.SelectedValue;

                if (sender != null)
                    txtBCity.Focus();
            }
            else
            {
                aceSCity.ContextKey = String.Empty;
                txtSCity.Text = String.Empty;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlAnniversaryCredits_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(ddlAnniversaryCredits.SelectedValue) && Convert.ToDecimal(ddlAnniversaryCredits.SelectedValue) > 0)
            {
                if (this.AvailableAnniversaryCreditAmount < this.SubTotal + this.ShippingAmount + ((this.SubTotal + this.ShippingAmount) * this.StrikeIronTaxRate))
                {
                    dvRemainingBalance.Visible = true;
                    dvRemainingBalance.InnerHtml = @"Remaining balance is <em>" + (this.SubTotal + this.ShippingAmount + ((this.SubTotal + this.ShippingAmount) * this.StrikeIronTaxRate) - this.AvailableAnniversaryCreditAmount).ToString("C2") + "</em> for this purchase. Select a secondary payment option.";
                    liPaymentOption.Visible = true;

                    //Removing MOAS as payment option as anniversary credits & MOAS can not be applied as a combine payment to an order.
                    ddlPaymentOption.Items.Remove(ddlPaymentOption.Items.FindByText("MOAS"));

                    if (sender != null)
                        ddlPaymentOption.Focus();
                }
                else
                {
                    dvRemainingBalance.Visible = false;
                    liPaymentOption.Visible = false;
                    ddlPaymentOption.SelectedIndex = 0;
                }
            }
            else
            {
                dvRemainingBalance.Visible = false;
                liPaymentOption.Visible = true;
                //Adding back MOAS as payment option when the user don't use anniversary credits...
                ddlPaymentOption.Items.Insert(ddlPaymentOption.Items.Count, new ListItem("MOAS", "198"));

                if (sender != null)
                    ddlPaymentOption.Focus();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlPaymentOption_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (ddlPaymentOption.SelectedValue == Convert.ToString(Convert.ToInt64(Incentex.DAL.Common.DAEnums.PaymentOptions.CostCenterCode)) || ddlPaymentOption.SelectedValue == Convert.ToString(Convert.ToInt64(Incentex.DAL.Common.DAEnums.PaymentOptions.GLCode)) || ddlPaymentOption.SelectedValue == Convert.ToString(Convert.ToInt64(Incentex.DAL.Common.DAEnums.PaymentOptions.PurchaseOrder)))
                liPaymentOptionCode.Visible = true;
            else
                liPaymentOptionCode.Visible = false;

            ShowCreditCardFields(ddlPaymentOption.SelectedValue == Convert.ToString(Convert.ToInt64(Incentex.DAL.Common.DAEnums.PaymentOptions.CreditCard)));
            ShowAnniversaryCredits(ddlPaymentOption.SelectedValue == Convert.ToString(Convert.ToInt64(Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS)));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlCreditCardType_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (ddlCreditCardType.SelectedItem.Text == "Master Card")
            {
                revCardNumber.ValidationExpression = @"^5[1-5][0-9]{14}$";
                revCardNumber.ErrorMessage = "Please enter valid master card number.";
                txtSecurityCode.MaxLength = 3;
            }
            else if (ddlCreditCardType.SelectedItem.Text == "Visa")
            {
                revCardNumber.ValidationExpression = @"^4[0-9]{12}(?:[0-9]{3})?$";
                revCardNumber.ErrorMessage = "Please enter valid visa card number.";
                txtSecurityCode.MaxLength = 3;
            }
            else if (ddlCreditCardType.SelectedItem.Text == "Discover")
            {
                revCardNumber.ValidationExpression = @"^6(?:011|5[0-9]{2})[0-9]{12}$";
                revCardNumber.ErrorMessage = "Please enter valid discover card number.";
                txtSecurityCode.MaxLength = 3;
            }
            else if (ddlCreditCardType.SelectedItem.Text == "American Express")
            {
                revCardNumber.ValidationExpression = @"^3[47][0-9]{13}$";
                revCardNumber.ErrorMessage = "Please enter valid american express card number.";
                txtSecurityCode.MaxLength = 4;
            }

            if (sender != null)
                txtCardHolderName.Focus();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lbSaveShippingAddress_Click(Object sender, EventArgs e)
    {
        try
        {
            Page.Validate("SaveShipping");
            if (Page.IsValid)
            {
                SetStrikeIronFields();
                SetTotal();

                chkAddShipping.Enabled = false;
                ddlSCountry.Enabled = false;
                ddlShippingAddressName.Enabled = false;
                ddlShippingMethod.Enabled = false;
                ddlSState.Enabled = false;

                lbSaveShippingAddress.Enabled = false;

                txtSAddress1.Attributes.Add("readonly", "readonly");
                txtSAddress1.CssClass += " disable-txt";
                txtSAddress2.Attributes.Add("readonly", "readonly");
                txtSAddress2.CssClass += " disable-txt";
                txtSAddressName.Attributes.Add("readonly", "readonly");
                txtSAddressName.CssClass += " disable-txt";
                txtSCity.Attributes.Add("readonly", "readonly");
                txtSCity.CssClass += " disable-txt";
                txtSCompany.Attributes.Add("readonly", "readonly");
                txtSCompany.CssClass += " disable-txt";
                txtSFirstName.Attributes.Add("readonly", "readonly");
                txtSFirstName.CssClass += " disable-txt";
                txtSLastName.Attributes.Add("readonly", "readonly");
                txtSLastName.CssClass += " disable-txt";
                txtSZipCode.Attributes.Add("readonly", "readonly");
                txtSZipCode.CssClass += " disable-txt";

                lbSaveShippingAddress.CssClass = lbSaveShippingAddress.CssClass.Replace(" saveshipping", "");
                lbSaveShippingAddress.Enabled = false;
                lbSaveShippingAddress.Text = "SAVED";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lbSaveBillingAddress_Click(Object sender, EventArgs e)
    {
        try
        {
            Page.Validate("SaveBilling");
            if (Page.IsValid)
            {
                chkAddBilling.Enabled = false;
                ddlBCountry.Enabled = false;
                ddlBillingAddressName.Enabled = false;
                ddlBState.Enabled = false;

                txtBAddress1.Attributes.Add("readonly", "readonly");
                txtBAddress1.CssClass += " disable-txt";
                txtBAddress2.Attributes.Add("readonly", "readonly");
                txtBAddress2.CssClass += " disable-txt";
                txtBAddressName.Attributes.Add("readonly", "readonly");
                txtBAddressName.CssClass += " disable-txt";
                txtBCity.Attributes.Add("readonly", "readonly");
                txtBCity.CssClass += " disable-txt";
                txtBCompany.Attributes.Add("readonly", "readonly");
                txtBCompany.CssClass += " disable-txt";
                txtBFirstName.Attributes.Add("readonly", "readonly");
                txtBFirstName.CssClass += " disable-txt";
                txtBLastName.Attributes.Add("readonly", "readonly");
                txtBLastName.CssClass += " disable-txt";
                txtBZipCode.Attributes.Add("readonly", "readonly");
                txtBZipCode.CssClass += " disable-txt";

                lbSaveBillingAddress.CssClass = lbSaveBillingAddress.CssClass.Replace(" savebilling", "");
                lbSaveBillingAddress.Enabled = false;
                lbSaveBillingAddress.Text = "SAVED";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lbSavePayment_Click(Object sender, EventArgs e)
    {
        try
        {
            Page.Validate("SavePayment");
            if (Page.IsValid)
            {
                ddlAnniversaryCredits.Enabled = false;
                ddlCreditCardExpirationMonth.Enabled = false;
                ddlCreditCardType.Enabled = false;
                ddlPaymentOption.Enabled = false;

                txtCardExpirationYear.Attributes.Add("readonly", "readonly");
                txtCardExpirationYear.CssClass += " disable-txt";
                txtCardHolderName.Attributes.Add("readonly", "readonly");
                txtCardHolderName.CssClass += " disable-txt";
                txtCardNumber.Attributes.Add("readonly", "readonly");
                txtCardNumber.CssClass += " disable-txt";
                txtPaymentOptionCode.Attributes.Add("readonly", "readonly");
                txtPaymentOptionCode.CssClass += " disable-txt";
                txtReferenceName.Attributes.Add("readonly", "readonly");
                txtReferenceName.CssClass += " disable-txt";
                txtSecurityCode.Attributes.Add("readonly", "readonly");
                txtSecurityCode.CssClass += " disable-txt";

                lbSavePayment.CssClass = lbSavePayment.CssClass.Replace(" savepayment", "");
                lbSavePayment.Enabled = false;
                lbSavePayment.Text = "SAVED";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lbProcessOrder_Click(Object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Boolean orderSucceed = true;
            String orderMessage = String.Empty;
            Order objOrder = new Order();

            try
            {
                if (lbSaveShippingAddress.Text != "SAVED")
                {
                    SetStrikeIronFields();
                    SetTotal();
                }

                CompanyEmployeeRepository objCompEmpRepo = new CompanyEmployeeRepository();
                CompanyEmployee objCompEmp = objCompEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

                OrderConfirmationRepository objOrderRepo = new OrderConfirmationRepository();

                #region Order Number Generation

                String strOrderNumber;

                strOrderNumber = objOrderRepo.GetMaxOrderNumber();

                // Check if OrderNumber for the workgroup is null or empty
                if (String.IsNullOrEmpty(strOrderNumber))
                    //Generate New Order Number
                    strOrderNumber = DateTime.Now.Year.ToString() + "000001";
                else
                {
                    // Check if new year?
                    if (strOrderNumber.Substring(0, 4) == DateTime.Now.Year.ToString())
                        // Same year so Increment Order Number
                        strOrderNumber = (Convert.ToInt64(strOrderNumber) + 1).ToString();
                    else
                        // New year so Generate New Order Number
                        strOrderNumber = DateTime.Now.Year.ToString() + "000001";
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

                objOrder.SalesTax = this.SalesTaxAmount;
                objOrder.OrderDate = DateTime.Now;
                objOrder.OrderAmount = this.SubTotal;

                objOrder.MOASOrderAmount = objOrder.OrderAmount;
                objOrder.MOASSalesTax = objOrder.SalesTax;

                if (!String.IsNullOrEmpty(hfMOASApproverLevelID.Value))
                    objOrder.MOASApproverLevelID = Convert.ToInt64(hfMOASApproverLevelID.Value);

                objOrder.ShippingAmount = this.ShippingAmount;
                objOrder.StrikeIronTaxRate = this.StrikeIronTaxRate;
                objOrder.StrikeIronResponseFileName = this.StrikeIronResponseFileName;

                if (ddlPaymentOption.SelectedIndex > 0)
                    objOrder.PaymentOption = Convert.ToInt64(ddlPaymentOption.SelectedValue);
                else
                    objOrder.PaymentOption = null;

                if (!String.IsNullOrEmpty(txtPaymentOptionCode.Text.Trim()))
                {
                    if (txtPaymentOptionCode.Text.Length > 60)
                        txtPaymentOptionCode.Text = txtPaymentOptionCode.Text.Trim().Substring(0, 60);

                    objOrder.PaymentOptionCode = txtPaymentOptionCode.Text.Trim();
                }

                //if (IsCoseCenterCode && ddlCostCode.SelectedIndex > 0 && lblPayByCreditCard.Text != "" && Convert.ToDecimal(lblPayByCreditCard.Text.Trim()) > 0)
                //{
                //    objOrder.PaymentOptionCode = ddlCostCode.SelectedItem.Text; //txtPaymentOptionCode.Text.Trim();
                //    objOrder.CostCenterCodeID = Convert.ToInt64(ddlCostCode.SelectedValue.ToString());
                //}

                //if (!String.IsNullOrEmpty(ddlReplacementReason.SelectedValue) && Convert.ToInt64(ddlReplacementReason.SelectedValue) > 0)
                //    objOrder.ReplacementReasonID = Convert.ToInt64(ddlReplacementReason.SelectedValue);

                objOrder.SpecialOrderInstruction = String.Empty;
                objOrder.OrderStatus = null;
                objOrder.CreatedDate = objOrder.OrderDate;
                objOrder.UpdatedDate = objOrder.OrderDate;
                objOrder.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objOrder.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                objOrder.WorkgroupId = this.WorkGroupID;
                objOrder.MyShoppingCartID = this.CartIDs;
                objOrder.StoreID = this.StoreID;

                //Get Employee Type           
                if (objCompEmp.EmployeeTypeID != null)
                    objOrder.EmployeeTypeID = objCompEmp.EmployeeTypeID;

                objOrder.IsPaid = false;

                if (ddlAnniversaryCredits.SelectedIndex > 0)
                {
                    objOrder.CreditUsed = "Anniversary";
                    objOrder.CreditAmt = Convert.ToDecimal(ddlAnniversaryCredits.SelectedValue);
                }
                else
                {
                    objOrder.CreditUsed = null;
                    objOrder.CreditAmt = 0;
                }

                //if (!String.IsNullOrEmpty(lblPromoDiscount.Text.Trim()) && Convert.ToDecimal(lblPromoDiscount.Text.Trim()) > 0)
                //    objOrder.CorporateDiscount = Decimal.Parse(lblPromoDiscount.Text.Trim());
                //else
                objOrder.CorporateDiscount = 0;

                objOrder.OrderFor = this.OrderFor;

                #endregion

                #region Save Record

                objOrderRepo.Insert(objOrder);
                objOrderRepo.SubmitChanges();
                OrderConfirmationRepository objOrderRepo2 = new OrderConfirmationRepository();
                objOrder = objOrderRepo2.GetByOrderID(objOrder.OrderID);

                #endregion

                #region Update Cart

                if (this.OrderFor == "ShoppingCart")
                {
                    MyShoppingCartRepository objCartRepo = new MyShoppingCartRepository();

                    foreach (String CartID in CartIDs.Split(','))
                    {
                        if (!String.IsNullOrEmpty(CartID))
                        {
                            MyShoppinCart objShopCart = objCartRepo.GetById(Int32.Parse(CartID), IncentexGlobal.CurrentMember.UserInfoID);
                            objShopCart.IsOrdered = true;
                            objShopCart.OrderID = Convert.ToInt64(objOrder.OrderID);
                        }
                    }

                    objCartRepo.SubmitChanges();
                }
                else
                {
                    MyIssuanceCartRepository objCartRepo = new MyIssuanceCartRepository();

                    foreach (String CartID in CartIDs.Split(','))
                    {
                        if (!String.IsNullOrEmpty(CartID))
                        {
                            MyIssuanceCart objMyIssuanceCart = objCartRepo.GetById(Int32.Parse(CartID), IncentexGlobal.CurrentMember.UserInfoID);
                            objMyIssuanceCart.OrderStatus = "Submitted";
                            objMyIssuanceCart.OrderID = Convert.ToInt64(objOrder.OrderID);
                        }
                    }

                    objCartRepo.SubmitChanges();
                }

                #endregion

                #region Update Credits to Company Employee Table

                if (ddlAnniversaryCredits.SelectedIndex > 0)
                {
                    objCompEmp.CreditAmtToApplied -= Convert.ToDecimal(ddlAnniversaryCredits.SelectedValue);
                    objCompEmp.CreditAmtToExpired -= Convert.ToDecimal(ddlAnniversaryCredits.SelectedValue);
                    objCompEmpRepo.SubmitChanges();
                }

                #endregion

                #region EmployeeLedger

                if (objOrder.CreditAmt > 0)
                {
                    ////Added for the company employee ledger
                    EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                    EmployeeLedger objEmplLedger = new EmployeeLedger();
                    EmployeeLedger transaction = new EmployeeLedger();

                    objEmplLedger.UserInfoId = objCompEmp.UserInfoID;
                    objEmplLedger.CompanyEmployeeId = objCompEmp.CompanyEmployeeID;
                    objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ORCNR.ToString(); ;
                    objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.OrderConfirm.ToString();
                    objEmplLedger.TransactionAmount = objOrder.CreditAmt;
                    objEmplLedger.AmountCreditDebit = "Debit";
                    objEmplLedger.OrderNumber = objOrder.OrderNumber;
                    objEmplLedger.OrderId = Convert.ToInt64(objOrder.OrderID);

                    //When there will be records then get the last transaction
                    transaction = objLedger.GetLastTransactionByEmplID(objCompEmp.CompanyEmployeeID);
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

                #region Saving Addresses

                CompanyEmployeeContactInfoRepository objContactInfoRepo = new CompanyEmployeeContactInfoRepository();
                CityRepository objCityRep = new CityRepository();

                #region Shipping Info

                Int64 sCityID = 0;

                INC_City objSCity = objCityRep.CheckIfExist(Convert.ToInt64(ddlSCountry.SelectedItem.Value), Convert.ToInt64(ddlSState.SelectedItem.Value), txtSCity.Text.Trim());
                if (objSCity == null)
                {
                    INC_City objNewCity = new INC_City();
                    objNewCity.iCountryID = Convert.ToInt64(ddlSCountry.SelectedItem.Value);
                    objNewCity.iStateID = Convert.ToInt64(ddlSState.SelectedItem.Value);
                    objNewCity.sCityName = txtSCity.Text.Trim();
                    objCityRep.Insert(objNewCity);
                    objCityRep.SubmitChanges();
                    sCityID = objNewCity.iCityID;
                }
                else
                    sCityID = objSCity.iCityID;

                CompanyEmployeeContactInfo objOrderShippingInfo = new CompanyEmployeeContactInfo();

                objOrderShippingInfo.Address = txtSAddress1.Text.Trim();
                objOrderShippingInfo.Address2 = txtSAddress2.Text.Trim();
                objOrderShippingInfo.CityID = sCityID;
                objOrderShippingInfo.CompanyName = txtSCompany.Text.Trim();
                objOrderShippingInfo.ContactInfoType = Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString();
                objOrderShippingInfo.CountryID = Convert.ToInt64(ddlSCountry.SelectedValue);
                objOrderShippingInfo.county = this.County;
                objOrderShippingInfo.Fax = txtSLastName.Text.Trim();
                objOrderShippingInfo.Name = txtSFirstName.Text.Trim();
                objOrderShippingInfo.OrderID = objOrder.OrderID;
                objOrderShippingInfo.OrderType = this.OrderFor == "ShoppingCart" ? "ShoppingCart" : "IssuancePolicy";
                objOrderShippingInfo.StateID = Convert.ToInt64(ddlSState.SelectedValue);
                objOrderShippingInfo.Title = ddlShippingAddressName.SelectedValue;
                objOrderShippingInfo.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                objOrderShippingInfo.ZipCode = txtSZipCode.Text.Trim();

                objContactInfoRepo.Insert(objOrderShippingInfo);
                objContactInfoRepo.SubmitChanges();

                if (chkAddShipping.Checked && !String.IsNullOrEmpty(txtSAddressName.Text.Trim()))
                {
                    CompanyEmployeeContactInfo objSaveShipping = new CompanyEmployeeContactInfo();

                    objSaveShipping.Address = txtSAddress1.Text.Trim();
                    objSaveShipping.Address2 = txtSAddress2.Text.Trim();
                    objSaveShipping.CityID = sCityID;
                    objSaveShipping.CompanyName = txtSCompany.Text.Trim();
                    objSaveShipping.ContactInfoType = Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString();
                    objSaveShipping.CountryID = Convert.ToInt64(ddlSCountry.SelectedValue);
                    objSaveShipping.county = this.County;
                    objSaveShipping.Fax = txtSLastName.Text.Trim();
                    objSaveShipping.Name = txtSFirstName.Text.Trim();
                    objSaveShipping.OrderID = null;
                    objSaveShipping.OrderType = "MySetting";
                    objSaveShipping.StateID = Convert.ToInt64(ddlSState.SelectedValue);
                    objSaveShipping.Title = txtSAddressName.Text.Trim();
                    objSaveShipping.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                    objSaveShipping.ZipCode = txtSZipCode.Text.Trim();

                    objContactInfoRepo.Insert(objSaveShipping);
                    objContactInfoRepo.SubmitChanges();
                }

                #endregion

                #region Billing Info

                Int64 bCityID = 0;

                INC_City objBCity = objCityRep.CheckIfExist(Convert.ToInt64(ddlBCountry.SelectedItem.Value), Convert.ToInt64(ddlBState.SelectedItem.Value), txtBCity.Text.Trim());
                if (objBCity == null)
                {
                    INC_City objNewCity = new INC_City();
                    objNewCity.iCountryID = Convert.ToInt64(ddlBCountry.SelectedItem.Value);
                    objNewCity.iStateID = Convert.ToInt64(ddlBState.SelectedItem.Value);
                    objNewCity.sCityName = txtBCity.Text.Trim();
                    objCityRep.Insert(objNewCity);
                    objCityRep.SubmitChanges();
                    bCityID = objNewCity.iCityID;
                }
                else
                    bCityID = objBCity.iCityID;

                CompanyEmployeeContactInfo objOrderBillingInfo = new CompanyEmployeeContactInfo();

                objOrderBillingInfo.Address = txtBAddress1.Text.Trim();
                objOrderBillingInfo.Address2 = txtBAddress2.Text.Trim();
                objOrderBillingInfo.BillingCO = txtBFirstName.Text.Trim();
                objOrderBillingInfo.CityID = bCityID;
                objOrderBillingInfo.CompanyName = txtBCompany.Text.Trim();
                objOrderBillingInfo.ContactInfoType = Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString();
                objOrderBillingInfo.CountryID = Convert.ToInt64(ddlBCountry.SelectedValue);
                objOrderBillingInfo.Manager = txtBLastName.Text.Trim();
                objOrderBillingInfo.Name = txtBFirstName.Text.Trim() + " " + txtBLastName.Text.Trim();
                objOrderBillingInfo.OrderID = objOrder.OrderID;
                objOrderBillingInfo.OrderType = this.OrderFor == "ShoppingCart" ? "ShoppingCart" : "IssuancePolicy";
                objOrderBillingInfo.StateID = Convert.ToInt64(ddlBState.SelectedValue);
                objOrderBillingInfo.Title = ddlBillingAddressName.SelectedValue;
                objOrderBillingInfo.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                objOrderBillingInfo.ZipCode = txtBZipCode.Text.Trim();

                objContactInfoRepo.Insert(objOrderBillingInfo);
                objContactInfoRepo.SubmitChanges();

                if (chkAddBilling.Checked && !String.IsNullOrEmpty(txtBAddressName.Text.Trim()))
                {
                    CompanyEmployeeContactInfo objSaveBilling = new CompanyEmployeeContactInfo();
                    objSaveBilling.Address = txtBAddress1.Text.Trim();
                    objSaveBilling.Address2 = txtBAddress2.Text.Trim();
                    objSaveBilling.BillingCO = txtBFirstName.Text.Trim();
                    objSaveBilling.CityID = bCityID;
                    objSaveBilling.CompanyName = txtBCompany.Text.Trim();
                    objSaveBilling.ContactInfoType = Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString();
                    objSaveBilling.CountryID = Convert.ToInt64(ddlBCountry.SelectedValue);
                    objSaveBilling.Manager = txtBLastName.Text.Trim();
                    objSaveBilling.Name = txtBFirstName.Text.Trim() + " " + txtBLastName.Text.Trim();
                    objSaveBilling.OrderID = null;
                    objSaveBilling.OrderType = "MySetting";
                    objSaveBilling.StateID = Convert.ToInt64(ddlBState.SelectedValue);
                    objSaveBilling.Title = txtBAddressName.Text.Trim();
                    objSaveBilling.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                    objSaveBilling.ZipCode = txtBZipCode.Text.Trim();

                    objContactInfoRepo.Insert(objSaveBilling);
                    objContactInfoRepo.SubmitChanges();
                }

                #endregion

                #endregion

                #region Order Confirm Process

                String paymentMethod = String.Empty;
                String paymentStatus = String.Empty;

                if (!String.IsNullOrEmpty(txtPaymentOptionCode.Text))
                    paymentMethod = ddlPaymentOption.SelectedItem.Text + " : " + txtPaymentOptionCode.Text;
                else
                    paymentMethod = ddlPaymentOption.SelectedItem.Text;

                if (paymentMethod == "Credit Card")
                {
                    #region Credit Card Processing

                    PaymentInfo objPaymentInfo = new PaymentInfo();

                    if (objOrder.CreditUsed != null)
                    {
                        if (objOrder.CreditAmt != 0)
                            objPaymentInfo.OrderAmountToPay = (Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax) - Convert.ToDecimal(objOrder.CorporateDiscount)) - (Convert.ToDecimal(objOrder.CreditAmt));
                        else
                            objPaymentInfo.OrderAmountToPay = (Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax) - Convert.ToDecimal(objOrder.CorporateDiscount));
                    }
                    else
                        objPaymentInfo.OrderAmountToPay = (Convert.ToDecimal(objOrder.OrderAmount) + Convert.ToDecimal(objOrder.ShippingAmount) + Convert.ToDecimal(objOrder.SalesTax) - Convert.ToDecimal(objOrder.CorporateDiscount));

                    objPaymentInfo.OrderNumber = objOrder.OrderNumber;

                    #region 1 - Store credit card detail to payment information

                    String cardType = String.Empty;

                    if (ddlCreditCardType.SelectedItem.Text == "Master Card")
                        cardType = "MasterCard";
                    else if (ddlCreditCardType.SelectedItem.Text == "Visa")
                        cardType = "Visa";
                    else if (ddlCreditCardType.SelectedItem.Text == "Discover")
                        cardType = "Discover";
                    else if (ddlCreditCardType.SelectedItem.Text == "American Express")
                        cardType = "Amex";

                    objPaymentInfo.CardNumber = txtCardNumber.Text;
                    objPaymentInfo.CardType = cardType;
                    objPaymentInfo.ExpiresOnMonth = ddlCreditCardExpirationMonth.SelectedValue;
                    objPaymentInfo.ExpiresOnYear = txtCardExpirationYear.Text.Trim();
                    objPaymentInfo.CardVerification = txtSecurityCode.Text.Trim();

                    #endregion

                    #region 2 - Store billing information to payment information

                    objPaymentInfo.B_FirstName = txtBFirstName.Text.Trim();
                    objPaymentInfo.B_LastName = txtBLastName.Text.Trim();
                    objPaymentInfo.B_CompanyName = txtBCompany.Text.Trim();
                    objPaymentInfo.B_StreetAddress1 = txtBAddress1.Text.Trim();
                    objPaymentInfo.B_StreetAddress2 = txtBAddress2.Text.Trim();
                    objPaymentInfo.B_City = txtBCity.Text.Trim();
                    objPaymentInfo.B_State = ddlBState.SelectedItem.Text;
                    objPaymentInfo.B_Zipcode = txtBZipCode.Text.Trim();
                    objPaymentInfo.B_PhoneNumber = String.Empty;
                    objPaymentInfo.B_Email = String.Empty;

                    #endregion

                    #region 3 - Store shipping information to payment info

                    objPaymentInfo.S_FirstName = txtSFirstName.Text.Trim();
                    objPaymentInfo.S_LastName = txtSLastName.Text.Trim();
                    objPaymentInfo.S_CompanyName = txtSCompany.Text.Trim();
                    objPaymentInfo.S_StreetAddress1 = txtSAddress1.Text.Trim();
                    objPaymentInfo.S_StreetAddress2 = txtSAddress2.Text.Trim();
                    objPaymentInfo.S_City = txtSCity.Text.Trim();
                    objPaymentInfo.S_State = ddlSState.SelectedItem.Text;
                    objPaymentInfo.S_Zipcode = txtSZipCode.Text.Trim();
                    objPaymentInfo.S_PhoneNumber = String.Empty;
                    objPaymentInfo.S_Email = String.Empty;

                    #endregion

                    String transactionID = DoDirectPayment(objPaymentInfo);

                    if (!String.IsNullOrEmpty(transactionID) && !transactionID.StartsWith("Error : ") && !transactionID.StartsWith("Invalid : "))
                    {
                        objOrder.PaymentTranscationNumber = transactionID;
                        objOrder.OrderStatus = "Open";
                        objOrder.IsPaid = true;
                        paymentStatus = "success";
                    }
                    else
                    {
                        if (transactionID.StartsWith("Error : "))
                            paymentStatus = "error in card processing";
                        else
                            paymentStatus = transactionID;

                        orderSucceed = false;
                    }

                    #endregion
                }
                else
                {
                    #region Update Order Status

                    if (this.OrderFor == "ShoppingCart")
                    {
                        if (paymentMethod.Contains("MOAS") || paymentMethod.Contains("Replacement Uniforms"))
                            objOrder.OrderStatus = "Order Pending";
                        else
                            objOrder.OrderStatus = "Open";
                    }
                    else
                    {
                        //when paymentMethod is moas then insert order status as 'Order Pending'.
                        if (Session["PaymentOption"] != null && (Int32)(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.MOAS) == Convert.ToInt32(Session["PaymentOption"]))
                        {
                            objOrder.OrderStatus = "Order Pending";
                            objOrder.PaymentOption = new LookupRepository().GetIdByLookupNameNLookUpCode("MOAS", "WLS Payment Option");
                        }//This is for Employee pay MOAS option
                        else if (Session["PaymentOption"] != null && (Int32)(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.EmployeePays) == Convert.ToInt32(Session["PaymentOption"]) && (paymentMethod.Contains("MOAS") || paymentMethod.Contains("Replacement Uniforms")))
                            objOrder.OrderStatus = "Order Pending";
                        else
                            objOrder.OrderStatus = "Open";
                    }

                    #endregion

                    objOrder.IsPaid = true;
                    paymentStatus = "success";
                }

                if (paymentStatus == "success")
                {
                    objOrderRepo2.SubmitChanges();

                    #region Decrease inventory

                    ProductItemDetailsRepository objProductRepo = new ProductItemDetailsRepository();

                    if (this.OrderFor == "ShoppingCart")
                    {
                        MyShoppingCartRepository objShoppingCartRepo = new MyShoppingCartRepository();

                        List<SelectMyShoppingCartProductResult> lstShoppingCartProducts = objShoppingCartRepo.SelectCurrentOrderProducts(objOrder.OrderID);

                        foreach (SelectMyShoppingCartProductResult objCartProduct in lstShoppingCartProducts)
                        {
                            if (objCartProduct != null)
                            {
                                ProductItem objProductItem = objProductRepo.GetRecord(Convert.ToInt64(objCartProduct.StoreProductID), Convert.ToInt64(objCartProduct.MasterItemNo), objCartProduct.item);
                                objOrderRepo.UpdateInventory(objCartProduct.MyShoppingCartID, objProductItem.ProductItemID, "Shopping");
                            }
                        }
                    }
                    else
                    {
                        MyIssuanceCartRepository objIssuanceCartRepo = new MyIssuanceCartRepository();
                        List<SelectMyIssuanceProductItemsResult> objFinal = objIssuanceCartRepo.GetMyIssuanceProdList(objOrder.OrderID, objOrder.UserId, true);

                        Boolean IsUpdateUserReactivatedPolicy = false;

                        foreach (SelectMyIssuanceProductItemsResult objIssuanceProduct in objFinal)
                        {
                            if (objIssuanceProduct != null)
                            {
                                if (!IsUpdateUserReactivatedPolicy)
                                {
                                    new ChangeStatusRepository().UpdateUserReActivatedIssuancePolicy(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(objIssuanceProduct.UniformIssuancePolicyID));
                                    IsUpdateUserReactivatedPolicy = true;
                                }

                                MyIssuanceCart objIssuance = objIssuanceCartRepo.GetById(objIssuanceProduct.MyIssuanceCartID, IncentexGlobal.CurrentMember.UserInfoID);
                                List<SelectProductIDResult> objList = new List<SelectProductIDResult>();
                                objList = objProductRepo.GetProductId(objIssuance.MasterItemID, objIssuanceProduct.MyIssuanceCartID, Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(this.StoreID));

                                for (Int32 i = 0; i < objList.Count; i++)
                                {
                                    objOrderRepo.UpdateInventory(objIssuanceProduct.MyIssuanceCartID, Convert.ToInt64(objList[i].ProductItemID), "UniformIssuance");
                                }

                            }
                        }
                    }

                    #endregion

                    SendEmails(objOrder, objOrderShippingInfo, objOrderBillingInfo, paymentMethod);

                    Session["OrderCompleted"] = true;

                    new SAPOperations().SubmitOrderToSAP(objOrder.OrderID);

                    if (this.StrikeIronResponseFailed)
                    {
                        String Subject = "World-Link (" + (CommonMails.Live ? "live" : "test") + ") : Stike-Iron Response failed for order # " + objOrder.OrderNumber;
                        StringBuilder Body = new StringBuilder("Strike Iron Response has failed for World-Link (" + (CommonMails.Live ? "live" : "test") + ") order # " + objOrder.OrderNumber + ".");
                        Body.Append("<br/><br/>The ship to zip code entered by the user was " + this.ShipToZipCodePassedToStrikeIron + ".");
                        Body.Append("<br/><br/>Attached is the strike Iron response in XML format.");
                        Body.Append("<br/><br/>Thanks");
                        Body.Append("<br/>Wolrd-Link System");
                        Body.Append("<br/><br/><b>Note :</b> Do not reply to this email, it has been sent from an un-monitored email address.");

                        new CommonMails().SendMailWithAttachment(0, "Stike Iron Response Failed", CommonMails.EmailFrom, Convert.ToString(ConfigurationManager.AppSettings["StrikeIronFailedNotifyList"]), Subject, Body.ToString(), CommonMails.DisplyName, CommonMails.SSL, true, Path.Combine(Common.StrikeIronResponsePath, this.StrikeIronResponseFileName), CommonMails.SMTPHost, CommonMails.SMTPPort, CommonMails.UserName, CommonMails.Password, this.StrikeIronResponseFileName, CommonMails.Live);

                        this.StrikeIronResponseFailed = false;
                    }
                }
                else
                {
                    UndoChanges(objOrder.OrderID);
                    orderMessage = paymentStatus;
                }

                #endregion
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
                Session["OrderFailed"] = true;
                orderSucceed = false;
                UndoChanges(objOrder.OrderID);
            }
            finally
            {
                ShowConfirmationPopup(orderSucceed, objOrder.OrderNumber, objOrder.OrderStatus, orderMessage);
            }
        }
    }

    //protected void btnCallStrikeIron_Click(Object sender, EventArgs e)
    //{
    //    try
    //    {
    //        SetStrikeIronFields();
    //    }
    //    catch (Exception ex)
    //    {
    //        ErrHandler.WriteError(ex);
    //    }
    //}

    #region Paging Events

    protected void dtlPaging_ItemCommand(Object sender, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ChangePage")
            {
                this.PageIndex = Convert.ToInt32(e.CommandArgument) - 1;
                BindItems(false);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dtlPaging_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        try
        {
            LinkButton lnkPaging = (LinkButton)e.Item.FindControl("lnkPaging");

            if (lnkPaging.Text == Convert.ToString(this.PageIndex + 1))
            {
                lnkPaging.Enabled = false;
                lnkPaging.Font.Bold = true;
                lnkPaging.CssClass = String.Empty;
                lnkPaging.Font.Size = new FontUnit(12);
            }
            else
            {
                lnkPaging.CssClass = "postback";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkPrevious_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex -= 1;
            BindItems(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkNext_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex += 1;
            BindItems(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #endregion

    #region Page Methods

    private void ClearPage()
    {
        try
        {
            this.aceBCity.ContextKey = String.Empty;
            this.aceCardExpirationYear.ContextKey = String.Empty;
            this.aceSCity.ContextKey = String.Empty;
            this.AvailableAnniversaryCreditAmount = 0M;
            this.CartIDs = String.Empty;
            this.chkAddBilling.Checked = true;
            this.chkAddShipping.Checked = true;
            this.CompanyEmployeeID = 0;
            this.County = String.Empty;
            this.ddlAnniversaryCredits.Items.Clear();
            this.ddlBCountry.Items.Clear();
            this.ddlBillingAddressName.Items.Clear();
            this.ddlBState.Items.Clear();
            this.ddlCreditCardExpirationMonth.Items.Clear();
            this.ddlCreditCardType.Items.Clear();
            this.ddlPaymentOption.Items.Clear();
            this.ddlSCountry.Items.Clear();
            this.ddlShippingAddressName.Items.Clear();
            this.ddlShippingMethod.Items.Clear();
            this.ddlSState.Items.Clear();
            this.dtlPaging.DataSource = null;
            this.dtlPaging.DataBind();
            //this.dvPrintOrder.Visible = true;
            //this.dvPrintOrderConfirmationContent.InnerHtml = String.Empty;
            this.dvRemainingBalance.InnerHtml = String.Empty;
            this.dvRemainingBalance.Visible = false;
            this.FromPage = 1;
            //this.h2OrderConfirmed.InnerHtml = String.Empty;
            this.hfMOASApproverLevelID.Value = String.Empty;
            this.isFreeShipping = false;
            this.IsMOASWithCostCenterCode = false;
            this.lblGrandTotal.Text = 0M.ToString("C2");
            this.lblShippingAmount.Text = 0M.ToString("C2");
            this.lblSubTotal.Text = 0M.ToString("C2");
            this.lblTax.Text = 0M.ToString("C2");
            this.liAnniversaryCredits.Visible = false;
            this.liCardDetails1.Visible = false;
            this.liCardDetails2.Visible = false;
            this.liCardDetails3.Visible = false;
            this.liCardHolderName.Visible = false;
            this.liCardNumber.Visible = false;
            this.liCreditCardType.Visible = false;
            this.liPaymentOptionCode.Visible = false;
            this.NoOfPagesToDisplay = 3;
            this.NoOfRecordsToDisplay = 5;
            this.OrderFor = "ShoppingCart";
            this.PageIndex = 0;
            this.pagingtable.Visible = false;
            //this.pOrderSentForApproval.InnerHtml = String.Empty;
            this.PromotionCode = String.Empty;
            this.repCartItems.DataSource = null;
            this.repCartItems.DataBind();
            this.SalesTaxAmount = 0M;
            this.ShippingAmount = 0M;
            this.ShipToZipCodePassedToStrikeIron = String.Empty;
            this.StoreID = 0;
            this.StoreShippingDetails = null;
            this.StrikeIronResponseFailed = false;
            this.StrikeIronResponseFileName = String.Empty;
            this.StrikeIronTaxRate = 0M;
            this.SubTotal = 0M;
            this.ToPage = 1;
            this.TotalPages = 1;
            this.txtBAddress1.Text = String.Empty;
            this.txtBAddress2.Text = String.Empty;
            this.txtBAddressName.Text = String.Empty;
            this.txtBCity.Text = String.Empty;
            this.txtBCompany.Text = String.Empty;
            this.txtBFirstName.Text = String.Empty;
            this.txtBLastName.Text = String.Empty;
            this.txtBZipCode.Text = String.Empty;
            this.txtCardExpirationYear.Text = String.Empty;
            this.txtCardHolderName.Text = String.Empty;
            this.txtCardNumber.Text = String.Empty;
            this.txtPaymentOptionCode.Text = String.Empty;
            this.txtReferenceName.Text = String.Empty;
            this.txtSAddress1.Text = String.Empty;
            this.txtSAddress2.Text = String.Empty;
            this.txtSAddressName.Text = String.Empty;
            this.txtSCity.Text = String.Empty;
            this.txtSCompany.Text = String.Empty;
            this.txtSecurityCode.Text = String.Empty;
            this.txtSFirstName.Text = String.Empty;
            this.txtSLastName.Text = String.Empty;
            this.txtSZipCode.Text = String.Empty;
            this.WorkGroupID = 0;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void GetInitialDetails()
    {
        try
        {
            this.OrderFor = "ShoppingCart";

            GetCheckoutDetailsByUserInfoIDResult objUserDetail = new UserInformationRepository().GetCheckoutDetailsByUserInfoID(IncentexGlobal.CurrentMember.UserInfoID);
            if (objUserDetail != null)
            {
                this.AvailableAnniversaryCreditAmount = Convert.ToDecimal(objUserDetail.CreditAmtToApplied);
                this.CompanyEmployeeID = Convert.ToInt64(objUserDetail.CompanyEmployeeID);
                this.IsMOASWithCostCenterCode = Convert.ToBoolean(objUserDetail.IsMOASWithCostCenterCode);
                this.StoreID = objUserDetail.StoreID;
                this.WorkGroupID = objUserDetail.WorkgroupID;

                FreeShipping objFreeShippingDetails = new FreeShipping();

                objFreeShippingDetails.IsFreeShippingActive = Convert.ToBoolean(objUserDetail.isFreeShippingActive);
                objFreeShippingDetails.IsSaleShipping = Convert.ToBoolean(objUserDetail.IsSaleShipping);
                objFreeShippingDetails.MinimumShippingAmount = Convert.ToDecimal(objUserDetail.MinimumShippingAmount);
                objFreeShippingDetails.ShippingPercentOfSale = Convert.ToDecimal(objUserDetail.ShippiingPercentOfSale);
                objFreeShippingDetails.ShippingProgramEndDate = Convert.ToDateTime(objUserDetail.ShippingProgramEndDate);
                objFreeShippingDetails.ShippingProgramFor = Convert.ToString(objUserDetail.shippingprogramfor);
                objFreeShippingDetails.ShippingProgramStartDate = Convert.ToDateTime(objUserDetail.ShippingProgramStartDate);
                objFreeShippingDetails.TotalSaleAbove = Convert.ToDecimal(objUserDetail.totalsaleabove);

                this.StoreShippingDetails = objFreeShippingDetails;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void BindItems(Boolean CalculateAmounts)
    {
        try
        {
            MyShoppingCartRepository objCartRepo = new MyShoppingCartRepository();
            List<SelectMyShoppingCartProductResult> lstCartItems = objCartRepo.SelectShoppingProduct(IncentexGlobal.CurrentMember.UserInfoID);

            if (lstCartItems.Count > 0)
            {
                this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstCartItems.Count) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));

                repCartItems.DataSource = lstCartItems.Skip(this.PageIndex * this.NoOfRecordsToDisplay).Take(this.NoOfRecordsToDisplay).ToList();
                repCartItems.DataBind();

                this.CartIDs = String.Join(",", lstCartItems.Select(le => le.MyShoppingCartID.ToString()).ToArray());

                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 0;
                lnkNext.Enabled = this.PageIndex < this.TotalPages - 1;

                if (lnkPrevious.Enabled)
                    lnkPrevious.CssClass += " postback";
                else
                    lnkPrevious.CssClass.Replace(" postback", "");

                if (lnkNext.Enabled)
                    lnkNext.CssClass += " postback";
                else
                    lnkNext.CssClass.Replace(" postback", "");

                pagingtable.Visible = true;
                lbProcessOrder.Enabled = true;
                hdnItemCount.Value = lstCartItems.Count.ToString();
            }
            else
            {
                repCartItems.DataSource = null;
                repCartItems.DataBind();
                pagingtable.Visible = false;
                lbProcessOrder.Enabled = false;
                hdnItemCount.Value = "0";
            }

            if (CalculateAmounts)
            {
                if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                    this.SubTotal = lstCartItems.Sum(le => Convert.ToDecimal(le.UnitPrice) * Convert.ToDecimal(le.Quantity));
                else
                    this.SubTotal = lstCartItems.Sum(le => Convert.ToDecimal(le.MOASUnitPrice) * Convert.ToDecimal(le.Quantity));

                SetShippingAmount();
                SetTotal();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SetShippingAmount()
    {
        try
        {
            if (this.StoreShippingDetails != null)
            {
                Boolean isShippingAmountApplicable = false;

                // Check if Free Shipping is active or not
                if (this.StoreShippingDetails.IsFreeShippingActive)
                {
                    // Check if Free Shipping program is valid (in promotion)
                    if (this.StoreShippingDetails.ShippingProgramStartDate < DateTime.Now && this.StoreShippingDetails.ShippingProgramEndDate > DateTime.Now)
                    {
                        switch (this.StoreShippingDetails.ShippingProgramFor.ToLower())
                        {
                            case "admin": //For Admin
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                                    // Check if Sale Shipping is applicable
                                    isShippingAmountApplicable = this.StoreShippingDetails.IsSaleShipping;
                                break;

                            case "employee": // For Employee
                                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                                    // Check if Sale Shipping is applicable
                                    isShippingAmountApplicable = this.StoreShippingDetails.IsSaleShipping;
                                break;

                            case "both": // For Both
                                // Check if Sale Shipping is applicable
                                isShippingAmountApplicable = this.StoreShippingDetails.IsSaleShipping;
                                break;
                            case "bothunticked":
                                isShippingAmountApplicable = false;
                                break;
                            default:
                                // Check if Sale Shipping is applicable                            
                                isShippingAmountApplicable = this.StoreShippingDetails.IsSaleShipping;
                                break;
                        }
                    }
                    else
                        // Check if Sale Shipping is applicable
                        isShippingAmountApplicable = this.StoreShippingDetails.IsSaleShipping;
                }
                else // Free Shipping Inactive so calculate Shipping charge
                    // Check if Sale Shipping is applicable
                    isShippingAmountApplicable = this.StoreShippingDetails.IsSaleShipping;

                if (isShippingAmountApplicable)
                {
                    // Compare OrderAmount with TotalSalesAbove (Shipping)
                    if (this.SubTotal <= this.StoreShippingDetails.TotalSaleAbove)
                        this.ShippingAmount = this.StoreShippingDetails.MinimumShippingAmount; // Shipping amount = only minimum shipping amount
                    else
                        this.ShippingAmount = this.StoreShippingDetails.MinimumShippingAmount + ((this.SubTotal - this.StoreShippingDetails.TotalSaleAbove) * this.StoreShippingDetails.ShippingPercentOfSale / 100.0M);
                }
                else
                {
                    this.isFreeShipping = true;
                }
            }

            this.ShippingAmount = Decimal.Round(this.ShippingAmount, 2);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SetTotal()
    {
        try
        {
            lblSubTotal.Text = this.SubTotal.ToString("C2");
            //lblPromoDiscount.Text = "(" + this.PromoDiscountAmount.ToString("C2") + ")";

            if (this.SubTotal > 0)
                lblShippingAmount.Text = this.ShippingAmount.ToString("C2");
            else
                lblShippingAmount.Text = 0M.ToString("C2");

            lblTax.Text = Convert.ToDecimal((this.SubTotal + this.ShippingAmount) * this.StrikeIronTaxRate).ToString("C2");

            if (this.SubTotal > 0)
                lblGrandTotal.Text = Convert.ToDecimal(this.SubTotal + this.ShippingAmount + ((this.SubTotal + this.ShippingAmount) * this.StrikeIronTaxRate)).ToString("C2");
            else
                lblGrandTotal.Text = 0M.ToString("C2");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillDropDowns()
    {
        try
        {
            FillCountries();
            FillPaymentOption();
            FillAddressNames();
            FillShippingMethod();
            FillAnniversaryCredits();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillCountries()
    {
        try
        {
            CountryRepository objCountryRepo = new CountryRepository();
            List<INC_Country> lstCountries = objCountryRepo.GetAll().OrderBy(le => le.sCountryName).ToList();

            ddlSCountry.DataSource = lstCountries;
            ddlSCountry.DataTextField = "sCountryName";
            ddlSCountry.DataValueField = "iCountryId";
            ddlSCountry.DataBind();

            ddlSCountry.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlSCountry.SelectedValue = ddlSCountry.Items.FindByText("United States").Value;
            ddlSCountry_SelectedIndexChanged(null, null);

            ddlBCountry.DataSource = lstCountries;
            ddlBCountry.DataTextField = "sCountryName";
            ddlBCountry.DataValueField = "iCountryId";
            ddlBCountry.DataBind();

            ddlBCountry.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlBCountry.SelectedValue = ddlBCountry.Items.FindByText("United States").Value;
            ddlBCountry_SelectedIndexChanged(null, null);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillPaymentOption()
    {
        try
        {
            CompanyEmployeeRepository objCompEmpRepo = new CompanyEmployeeRepository();
            List<GetUserPaymentOptionsResult> lstUserPaymentOptions = objCompEmpRepo.GetUserPaymentOptions(IncentexGlobal.CurrentMember.UserInfoID).OrderBy(le => le.PaymentOption).ToList();

            #region Stuff for MOAS

            List<GetMOASApproverForCEResult> lstMOASApprover = new List<GetMOASApproverForCEResult>();

            if (this.OrderFor == "ShoppingCart")
                lstMOASApprover = objCompEmpRepo.GetMOASApprover(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.CompanyId, 0);
            else
                lstMOASApprover = objCompEmpRepo.GetMOASApprover(IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.CompanyId, Convert.ToInt64(Session["PID"].ToString()));

            if (lstMOASApprover != null)
                if (lstMOASApprover.Count == 0)
                    lstUserPaymentOptions = lstUserPaymentOptions.Where(x => x.PaymentOption != "MOAS").ToList();
                else
                    hfMOASApproverLevelID.Value = Convert.ToString(lstMOASApprover.FirstOrDefault().MOASApproverLevelID);

            #endregion

            ddlPaymentOption.DataSource = lstUserPaymentOptions;
            ddlPaymentOption.DataTextField = "PaymentOption";
            ddlPaymentOption.DataValueField = "PaymentOptionID";
            ddlPaymentOption.DataBind();

            ddlPaymentOption.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillShippingMethod()
    {
        try
        {
            LookupRepository objLookupRepo = new LookupRepository();
            List<INC_Lookup> lstShippingMethods = objLookupRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.ShippingMethod).OrderBy(le => le.sLookupName).ToList();

            ddlShippingMethod.DataSource = lstShippingMethods;
            ddlShippingMethod.DataTextField = "sLookupName";
            ddlShippingMethod.DataValueField = "iLookupId";
            ddlShippingMethod.DataBind();

            ddlShippingMethod.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlShippingMethod.SelectedValue = ddlShippingMethod.Items.FindByText("Common Carrier") != null ? ddlShippingMethod.Items.FindByText("Common Carrier").Value : "0";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillAddressNames()
    {
        try
        {
            CompanyEmployeeContactInfoRepository objCompEmpContactRepo = new CompanyEmployeeContactInfoRepository();
            List<CompanyEmployeeContactInfo> lstShippingAddresses = objCompEmpContactRepo.GetSavedShippingAddressesForUser(IncentexGlobal.CurrentMember.UserInfoID, CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Title, Incentex.DAL.Common.DAEnums.SortOrderType.Asc);
            ddlShippingAddressName.DataSource = lstShippingAddresses;
            ddlShippingAddressName.DataTextField = "Title";
            ddlShippingAddressName.DataValueField = "CompanyContactInfoID";
            ddlShippingAddressName.DataBind();

            ddlShippingAddressName.Items.Insert(0, new ListItem("-Choose address or enter below-", "0"));

            List<CompanyEmployeeContactInfo> lstBillingAddresses = objCompEmpContactRepo.GetSavedBillingAddressesForUser(IncentexGlobal.CurrentMember.UserInfoID, CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Title, Incentex.DAL.Common.DAEnums.SortOrderType.Asc);
            ddlBillingAddressName.DataSource = lstBillingAddresses;
            ddlBillingAddressName.DataTextField = "Title";
            ddlBillingAddressName.DataValueField = "CompanyContactInfoID";
            ddlBillingAddressName.DataBind();

            ddlBillingAddressName.Items.Insert(0, new ListItem("-Choose address or enter below-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillShippingState(String countryID)
    {
        try
        {
            Int64 iCountryID;
            Boolean countrySelected = Int64.TryParse(countryID, out iCountryID);

            if (countrySelected)
            {
                StateRepository objStateRepo = new StateRepository();
                List<INC_State> lstStates = objStateRepo.GetByCountryId(iCountryID).OrderBy(le => le.sStatename).ToList();

                ddlSState.DataSource = lstStates;
                ddlSState.DataTextField = "sStateName";
                ddlSState.DataValueField = "iStateId";
                ddlSState.DataBind();
            }
            else
                ddlSState.Items.Clear();

            ddlSState.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillBillingState(String countryID)
    {
        try
        {
            Int64 iCountryID;
            Boolean countrySelected = Int64.TryParse(countryID, out iCountryID);

            if (countrySelected)
            {
                StateRepository objStateRepo = new StateRepository();
                List<INC_State> lstStates = objStateRepo.GetByCountryId(iCountryID).OrderBy(le => le.sStatename).ToList();

                ddlBState.DataSource = lstStates;
                ddlBState.DataTextField = "sStateName";
                ddlBState.DataValueField = "iStateId";
                ddlBState.DataBind();
            }
            else
                ddlBState.Items.Clear();

            ddlBState.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void FillAnniversaryCredits()
    {
        try
        {
            if (this.AvailableAnniversaryCreditAmount > 0)
            {
                ddlAnniversaryCredits.Items.Insert(0, new ListItem("-Select-", "0"));
                ddlAnniversaryCredits.Items.Insert(1, new ListItem("Store Credits " + this.AvailableAnniversaryCreditAmount.ToString("C2"), this.AvailableAnniversaryCreditAmount.ToString()));
                ddlAnniversaryCredits.DataBind();
            }
            else
                ddlAnniversaryCredits.Items.Clear();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void ClearShippingAddress()
    {
        try
        {
            ddlSCountry.SelectedValue = ddlSCountry.Items.FindByText("United States").Value;
            ddlSCountry_SelectedIndexChanged(null, null);
            txtSAddress1.Text = String.Empty;
            txtSAddress2.Text = String.Empty;
            txtSAddressName.Text = String.Empty;
            txtSCity.Text = String.Empty;
            txtSCompany.Text = String.Empty;
            txtSFirstName.Text = String.Empty;
            txtSLastName.Text = String.Empty;
            txtSZipCode.Text = String.Empty;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void ClearBillingAddress()
    {
        try
        {
            ddlBCountry.SelectedValue = ddlBCountry.Items.FindByText("United States").Value;
            ddlBCountry_SelectedIndexChanged(null, null);
            txtBAddress1.Text = String.Empty;
            txtBAddress2.Text = String.Empty;
            txtBAddressName.Text = String.Empty;
            txtBCity.Text = String.Empty;
            txtBCompany.Text = String.Empty;
            txtBFirstName.Text = String.Empty;
            txtBLastName.Text = String.Empty;
            txtBZipCode.Text = String.Empty;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

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
            String zipCode = txtSZipCode.Text.Trim(); //this is the ZIP code to get for which the operation will return data

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
                    this.SalesTaxAmount = Decimal.Round((this.SubTotal + Convert.ToDecimal(this.ShippingAmount)) * this.StrikeIronTaxRate, 2);
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
                    this.SalesTaxAmount = Decimal.Round((this.SubTotal + Convert.ToDecimal(this.ShippingAmount)) * this.StrikeIronTaxRate, 2);
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
                this.SalesTaxAmount = Decimal.Round((this.SubTotal + Convert.ToDecimal(this.ShippingAmount)) * this.StrikeIronTaxRate, 2);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void SetStrikeIronFields()
    {
        try
        {
            if (ddlSCountry.SelectedItem.Text.ToUpper() == "UNITED STATES")
            {
                GetSalesTax(ddlSCountry.SelectedItem.Text, ddlSState.SelectedItem.Text);
            }
            else if (ddlSCountry.SelectedItem.Text.ToUpper() == "CANADA" && ddlSState.SelectedItem.Text.ToUpper() != "ALBERTA" || ddlSState.SelectedItem.Text.ToUpper() != "NUNAVUT" || ddlSState.SelectedItem.Text.ToUpper() != "NORTHWEST TERRITORIES" || ddlSState.SelectedItem.Text.ToUpper() != "YUKON TERRITORY")
            {
                GetSalesTaxForCanada(Convert.ToInt64(ddlSState.SelectedValue));
            }
            else
            {
                this.StrikeIronTaxRate = 0M;
                this.StrikeIronResponseFailed = false;
                this.ShipToZipCodePassedToStrikeIron = String.Empty;
                this.StrikeIronResponseFileName = String.Empty;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void GeneratePaging()
    {
        try
        {
            Int32 tCurrentPg = this.PageIndex + 1;
            Int32 tToPg = this.ToPage;
            Int32 tFrmPg = this.FromPage;

            if (tCurrentPg > tToPg)
            {
                tFrmPg = tToPg + 1;
                tToPg += this.NoOfPagesToDisplay;
            }

            if (tCurrentPg < tFrmPg)
            {
                tToPg = tFrmPg - 1;
                tFrmPg -= this.NoOfPagesToDisplay;
            }

            if (tToPg > this.TotalPages)
                tToPg = this.TotalPages;

            this.ToPage = tToPg;
            this.FromPage = tFrmPg;

            DataTable dt = new DataTable();
            dt.Columns.Add("Index");

            for (Int32 i = tFrmPg - 1; i < tToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Index"] = i + 1;
                dt.Rows.Add(dr);
            }

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void ShowAnniversaryCredits(Boolean isMOASPaymentOption)
    {
        try
        {
            if (this.OrderFor == "ShoppingCart" && this.AvailableAnniversaryCreditAmount > 0 && !isMOASPaymentOption)
            {
                liAnniversaryCredits.Visible = true;
                spanAnniversaryCredits.InnerText = "Payment Method 1";
                spanPaymentMethod.InnerText = "Payment Method 2*";
            }
            else
            {
                liAnniversaryCredits.Visible = false;
                spanPaymentMethod.InnerText = "Payment Method*";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void ShowCreditCardFields(Boolean show)
    {
        try
        {
            liCardHolderName.Visible = show;
            liCardNumber.Visible = show;
            liCreditCardType.Visible = show;
            liCardDetails1.Visible = show;
            liCardDetails2.Visible = show;
            liCardDetails3.Visible = show;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private String DoDirectPayment(PaymentInfo objPaymentInfo)
    {
        String transactionID = String.Empty;

        try
        {
            if (objPaymentInfo != null)
            {
                String[] a = objPaymentInfo.ExpiresOnYear.Split('/');
                String expirymonth = Convert.ToInt32(objPaymentInfo.ExpiresOnMonth).ToString("00");
                String expirydate = objPaymentInfo.ExpiresOnYear;

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
                encoder["AMT"] = objPaymentInfo.OrderAmountToPay.ToString();
                encoder["DESC"] = "Order Number :" + objPaymentInfo.OrderNumber;
                encoder["CREDITCARDTYPE"] = objPaymentInfo.CardType;
                encoder["ACCT"] = objPaymentInfo.CardNumber;
                encoder["EXPDATE"] = expirymonth + expirydate;
                encoder["CVV2"] = objPaymentInfo.CardVerification;
                encoder["COUNTRYCODE"] = "US";
                encoder["CURRENCYCODE"] = "USD";
                encoder["IPADDRESS"] = ip;

                /*Billing Information*/
                encoder["FIRSTNAME"] = objPaymentInfo.B_FirstName;
                encoder["LASTNAME"] = objPaymentInfo.B_LastName;
                encoder["STREET"] = objPaymentInfo.B_StreetAddress1;
                encoder["CITY"] = objPaymentInfo.B_City;
                encoder["STATE"] = objPaymentInfo.B_State;
                encoder["ZIP"] = objPaymentInfo.B_Zipcode;
                encoder["SHIPTOPHONENUM"] = objPaymentInfo.B_PhoneNumber;
                encoder["EMAIL"] = objPaymentInfo.B_Email != null && objPaymentInfo.B_Email != "" ? objPaymentInfo.B_Email : IncentexGlobal.CurrentMember.LoginEmail;

                /*Shipping Information*/
                encoder["SHIPTONAME"] = (objPaymentInfo.S_FirstName + " " + objPaymentInfo.S_LastName).Length > 31 ? (objPaymentInfo.S_FirstName + " " + objPaymentInfo.S_LastName).Substring(0, 31) : (objPaymentInfo.S_FirstName + " " + objPaymentInfo.S_LastName);
                encoder["SHIPTOSTREET"] = objPaymentInfo.S_StreetAddress1 + Environment.NewLine + objPaymentInfo.S_StreetAddress2;
                encoder["SHIPTOCITY"] = objPaymentInfo.S_City;
                encoder["SHIPTOSTATE"] = objPaymentInfo.S_State;
                encoder["SHIPTOZIP"] = objPaymentInfo.S_Zipcode;
                encoder["SHIPTOPHONENUM"] = objPaymentInfo.S_PhoneNumber;

                // Execute the API operation and obtain the response.
                String pStrrequestforNvp = encoder.Encode();
                String pStresponsenvp = caller.Call(pStrrequestforNvp);

                NVPCodec decoder = new NVPCodec();
                decoder.Decode(pStresponsenvp);

                String RespMsg = decoder["ACK"];

                if (RespMsg != null && (RespMsg == "Success" || RespMsg == "SuccessWithWarning"))
                    //Need to Save the data with response message.
                    transactionID = decoder["TRANSACTIONID"];
                else
                    transactionID = "Invalid : " + decoder["L_LONGMESSAGE0"];
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            transactionID = "Error : " + ex.Message;
        }

        return transactionID;
    }

    private void UndoChanges(Int64 OrderID)
    {
        try
        {
            OrderConfirmationRepository objOdrCnfRep = new OrderConfirmationRepository();
            Order objFailedOrder = objOdrCnfRep.GetByOrderID(OrderID);

            if (objFailedOrder != null && objFailedOrder.OrderStatus != "Deleted")
            {
                CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();

                #region Undo Order Changes
                CompanyEmployee objCE = objCompanyEmployeeRepository.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

                #region Undo Cart Changes

                if (!String.IsNullOrEmpty(objFailedOrder.MyShoppingCartID))
                {
                    String[] shoppingCartArray = objFailedOrder.MyShoppingCartID.Split(',');
                    Int32[] CartIDs = shoppingCartArray.Select(x => Int32.Parse(x)).ToArray();

                    if (objFailedOrder.OrderFor == "ShoppingCart")
                    {
                        //Shopping cart
                        MyShoppingCartRepository objShoppingCartRepos = new MyShoppingCartRepository();

                        foreach (Int32 CartID in CartIDs)
                        {
                            MyShoppinCart objShoppingcart = new MyShoppinCart();
                            ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                            ProductItem objProductItem = new ProductItem();
                            objShoppingcart = objShoppingCartRepos.GetById(CartID, Convert.ToInt64(objFailedOrder.UserId));

                            if (objShoppingcart != null)
                            {
                                objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(objShoppingcart.StoreProductID), Convert.ToInt64(objShoppingcart.MasterItemNo), objShoppingcart.ItemNumber);

                                //Update Inventory Here 
                                String strProcess = "Shopping";
                                String strMessage = objOdrCnfRep.IncreaseDescreaseInventory(objProductItem.ProductItemID, -Convert.ToInt32(objShoppingcart.Quantity), strProcess);

                                objShoppingcart.IsOrdered = false;
                                objShoppingcart.OrderID = null;
                            }
                        }

                        objShoppingCartRepos.SubmitChanges();
                    }
                    else
                    {
                        //Issuance
                        MyIssuanceCartRepository objIssuanceRepos = new MyIssuanceCartRepository();

                        foreach (Int32 CartID in CartIDs)
                        {
                            MyIssuanceCart objIssuance = new MyIssuanceCart();
                            ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();

                            objIssuance = objIssuanceRepos.GetById(CartID, Convert.ToInt64(objFailedOrder.UserId));
                            if (objIssuance != null)
                            {
                                List<SelectProductIDResult> objList = new List<SelectProductIDResult>();
                                Int64 storeid = new CompanyStoreRepository().GetStoreIDByCompanyId(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
                                objList = objProItemRepos.GetProductId(objIssuance.MasterItemID, CartID, Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(storeid));
                                //Update Inventory Here
                                for (Int32 i = 0; i < objList.Count; i++)
                                {
                                    String strProcess = "UniformIssuance";
                                    String strMessage = objOdrCnfRep.IncreaseDescreaseInventory(Convert.ToInt64(objList[i].ProductItemID), -Convert.ToInt32(objIssuance.Qty), strProcess);
                                }

                                objIssuanceRepos.Delete(objIssuance);
                            }
                        }

                        objIssuanceRepos.SubmitChanges();
                    }
                }

                #endregion

                objOdrCnfRep.UpdateStatus(OrderID, "Deleted", null, DateTime.Now);

                #region Undo Credits Changes

                AnniversaryCreditProgram objACP = new AnniversaryCreditProgram();
                AnniversaryProgramRepository objAnnCreditProRep = new AnniversaryProgramRepository();

                // Undo Previous Credit Changes
                if (!String.IsNullOrEmpty(objFailedOrder.CreditUsed) && objFailedOrder.CreditUsed == "Previous" && objFailedOrder.CreditAmt > 0)
                {
                    if (objCE != null)
                    {
                        objCE.StratingCreditAmount = objCE.StratingCreditAmount + objFailedOrder.CreditAmt;
                        objCE.CreditAmtToApplied = objCE.CreditAmtToApplied + objFailedOrder.CreditAmt;
                        objCE.CreditAmtToExpired = objCE.CreditAmtToExpired + objFailedOrder.CreditAmt;
                        objCompanyEmployeeRepository.SubmitChanges();
                    }
                }

                // Undo Anniversary Credit Changes
                if (!String.IsNullOrEmpty(objFailedOrder.CreditUsed) && objFailedOrder.CreditUsed == "Anniversary" && objFailedOrder.CreditAmt > 0)
                {
                    if (objCE != null)
                    {
                        objCE.CreditAmtToApplied = objCE.CreditAmtToApplied + objFailedOrder.CreditAmt;
                        objCE.CreditAmtToExpired = objCE.CreditAmtToExpired + objFailedOrder.CreditAmt;
                        objCompanyEmployeeRepository.SubmitChanges();
                    }
                }

                #endregion

                #region EmployeeLedger

                if (objFailedOrder.CreditAmt > 0)
                {
                    ////Added for the company employee ledger
                    EmployeeLedgerRepository objLedger = new EmployeeLedgerRepository();
                    EmployeeLedger objEmplLedger = new EmployeeLedger();
                    EmployeeLedger transaction = new EmployeeLedger();

                    objEmplLedger.UserInfoId = objCE.UserInfoID;
                    objEmplLedger.CompanyEmployeeId = objCE.CompanyEmployeeID;
                    objEmplLedger.TransactionCode = Incentex.DAL.Common.DAEnums.TransactionCode.ORINAMT.ToString();
                    objEmplLedger.TransactionType = Incentex.DAL.Common.DAEnums.TransactionType.OrderInCompletedAmount.ToString();
                    objEmplLedger.TransactionAmount = objFailedOrder.CreditAmt;
                    objEmplLedger.AmountCreditDebit = "Credit";
                    objEmplLedger.OrderNumber = objFailedOrder.OrderNumber;
                    objEmplLedger.OrderId = objFailedOrder.OrderID;

                    //When there will be records then get the last transaction
                    transaction = objLedger.GetLastTransactionByEmplID(objCE.CompanyEmployeeID);
                    if (transaction != null)
                    {
                        objEmplLedger.PreviousBalance = transaction.CurrentBalance;
                        objEmplLedger.CurrentBalance = transaction.CurrentBalance + objEmplLedger.TransactionAmount;
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

                #endregion
            }

            using (MailMessage objEmail = new MailMessage())
            {
                objEmail.Body = "Order # " + Request.QueryString["no"] + " (" + Request.QueryString["id"] + ")" + " has failed on World-Link System.<br/><br/>Determine the reason & fix the bug/issue.";
                objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                objEmail.IsBodyHtml = true;
                objEmail.Subject = "Order Failed - " + Request.QueryString["no"] + " (" + Request.QueryString["id"] + ")";
                objEmail.To.Add(new MailAddress("devraj.gadhavi@indianic.com"));
                objEmail.To.Add(new MailAddress("krushna@indianic.com"));

                SmtpClient objSmtp = new SmtpClient();

                objSmtp.EnableSsl = Common.SSL;
                objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                objSmtp.Host = Common.SMTPHost;
                objSmtp.Port = Common.SMTPPort;

                objSmtp.Send(objEmail);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #region Email functions with lot of scope for code optimization, reusability etc.

    private void SendEmails(Order objOrder, CompanyEmployeeContactInfo objShippingInfo, CompanyEmployeeContactInfo objBillingInfo, String paymentMethod)
    {
        try
        {
            #region Send email function based on condition

            ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();

            //1.)Customer
            if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(objOrder.UserId), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderConfirmations)) || (IncentexGlobal.IsIEFromStoreTestMode == true && IncentexGlobal.AdminUser != null))
                sendVerificationEmail(objOrder, objBillingInfo, objShippingInfo);

            //add by mayur on 22-nov-2011 for MOAS payment method
            //start
            if (paymentMethod.Contains("MOAS") || paymentMethod.Contains("Replacement Uniforms"))
            {
                //3.)MOAS Manager
                sendEmailToMOASManager(objOrder, objOrder.OrderFor, objBillingInfo, objShippingInfo);
            }
            else
            {
                //this for not sending an email to supplier when IE/CA comming from view storefront test mode.
                if (!objOrder.ReferenceName.ToLower().Contains("test") && !IncentexGlobal.IsIEFromStoreTestMode)
                {
                    //2.)IE Admin
                    sendIEEmail(objOrder, objBillingInfo, objShippingInfo);

                    //3.)Supplier
                    sendEmailToSupplier(objOrder, objShippingInfo);

                    //add by Saurabh on 27-Feb-2012 for Magaya Integration
                    //SendDataToMagaya(objOrder, objShippingInfo, objBillingInfo);
                }
                else
                {
                    sendTestOrderEmailNotification(objOrder, objBillingInfo, objShippingInfo);
                }
            }
            //end

            //remove session value            
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
            if (paymentMethod.Contains("Replacement Uniforms"))
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

            #endregion
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendVerificationEmail(Order objOrder, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
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
                UserInformationRepository objUserInformationRepository = new UserInformationRepository();
                CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
                CompanyRepository objCompanyRepository = new CompanyRepository();
                LookupRepository objLookupRepository = new LookupRepository();

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

                if (PaymentMethod.Contains("MOAS") || PaymentMethod.Contains("Replacement Uniforms"))//start add by mayur on 24-nov-2011
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
                    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
                    List<SelectMyShoppingCartProductResult> objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

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
                    MyIssuanceCartRepository objMyIssuanceCartRepository = new MyIssuanceCartRepository();
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

                String a = NameBars(objOrder);

                if (a != null)
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + a);
                }
                else
                {
                    messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                }

                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                dvPrintOrderConfirmationContent.InnerHtml = messagebody.ToString();

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
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendTestOrderEmailNotification(Order objOrder, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
    {
        try
        {
            ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();
            List<FUN_GetTestOrderEmailReceiversResult> objAdminList = new IncentexBEDataContext().FUN_GetTestOrderEmailReceivers().ToList();
            if (objAdminList.Count > 0)
            {
                foreach (FUN_GetTestOrderEmailReceiversResult receiver in objAdminList)
                {
                    //Email Management
                    if (objManageEmailRepo.CheckEmailAuthentication(Convert.ToInt64(receiver.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderConfirmations)) == true)
                        sendVerificationEmailAdmin(objOrder, receiver.LoginEmail, receiver.FirstName, objBillingInfo, objShippingInfo, Convert.ToInt64(receiver.UserInfoID), true);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendIEEmail(Order objOrder, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
    {
        try
        {
            UserInformationRepository objUserInformationRepository = new UserInformationRepository();
            ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();
            List<UserInformation> objAdminList = objUserInformationRepository.GetEmailInformation();
            if (objAdminList.Count > 0)
            {
                for (Int32 i = 0; i < objAdminList.Count; i++)
                {
                    //Email Management
                    if (objManageEmailRepo.CheckEmailAuthentication(objAdminList[i].UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderConfirmations)) == true)
                        sendVerificationEmailAdmin(objOrder, objAdminList[i].LoginEmail, objAdminList[i].FirstName, objBillingInfo, objShippingInfo, objAdminList[i].UserInfoID, false);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendVerificationEmailAdmin(Order objOrder, String IEemailAddress, String FullName, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo, Int64 UserInfoID, Boolean IsTestOrder)
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
                UserInformationRepository objUserInformationRepository = new UserInformationRepository();
                CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
                CompanyRepository objCompanyRepository = new CompanyRepository();
                LookupRepository objLookupRepository = new LookupRepository();
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
                    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
                    List<SelectMyShoppingCartProductResult> objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

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
                    MyIssuanceCartRepository objMyIssuanceCartRepository = new MyIssuanceCartRepository();
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

                String b = NameBars(objOrder);

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

                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();

                String NameBarList = NameBarBulkFile(objOrder);
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
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendEmailToSupplier(Order objOrder, CompanyEmployeeContactInfo objShippingInfo)
    {
        try
        {
            if (objOrder != null)
            {
                OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
                List<SelectSupplierAddressResult> obj = objOrderConfirmationRepository.GetSupplierAddress(objOrder.OrderFor, objOrder.OrderID);

                foreach (SelectSupplierAddressResult repeaterItem in obj)
                {
                    sendVerificationEmailSupplier(objOrder, objOrder.MyShoppingCartID, repeaterItem.SupplierID, repeaterItem.Name, objShippingInfo, repeaterItem.CompanyName);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            throw ex;
        }
    }

    private void sendVerificationEmailSupplier(Order objOrder, String ShoppingCartID, Int64 supplierId, String fullName, CompanyEmployeeContactInfo objShippingInfo, String SupplierCompanyName)
    {
        try
        {
            //Get supplierinfo by id
            UserInformationRepository objUserInformationRepository = new UserInformationRepository();
            UserInformation objUserInfo = objUserInformationRepository.GetById(new SupplierRepository().GetById(supplierId).UserInfoID);

            if (objUserInfo != null)
            {
                ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();
                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(objUserInfo.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.OrderConfirmations)) == true)
                {
                    OrderConfirmationRepository objOrderConfirmationRepository = new OrderConfirmationRepository();
                    CompanyRepository objCompanyRepository = new CompanyRepository();
                    LookupRepository objLookupRepository = new LookupRepository();
                    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
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

                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Unit Price";
                            innermessageSupplier = innermessageSupplier + "</td>";
                            innermessageSupplier = innermessageSupplier + "<td width='10%' style='font-weight: bold; text-align: center;'>";
                            innermessageSupplier = innermessageSupplier + "Extended Price";
                            innermessageSupplier = innermessageSupplier + "</td>";

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
                        messagebody.Replace("{ShippingCost}", "");
                        messagebody.Replace("{Saletax}", "");
                        messagebody.Replace("{OrderTotal}", "$" + Convert.ToDecimal(objOrder.SalesTax + objOrder.OrderAmount).ToString());
                        messagebody.Replace("{Order Notes:}", "Order Notes :");
                        messagebody.Replace("{ShippingCostView}", "");
                        messagebody.Replace("{SalesTaxView}", "");
                        messagebody.Replace("{OrderTotalView}", "Order Total :");

                        #endregion

                        String c = NameBars(objOrder);
                        if (c != null)
                        {
                            messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />") + c);
                        }
                        else
                        {
                            messagebody.Replace("{OrderNotes}", objOrder.SpecialOrderInstruction.ToString().Replace(Environment.NewLine, "<br />"));
                        }

                        messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                        String smtphost = CommonMails.SMTPHost;//Application["SMTPHOST"].ToString();
                        Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                        String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                        String smtppassword = CommonMails.Password;// Application["SMTPPASSWORD"].ToString();


                        String NameBarList = NameBarBulkFile(objOrder);
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

    private void sendEmailToMOASManager(Order objOrder, String OrderFor, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
    {
        try
        {
            CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();

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
            ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();

            for (Int32 i = 0; i < objlstMOASApprover.Count(); i++)
            {
                OrderMOASSystem objOrderMOASSystem = new OrderMOASSystem()
                {
                    OrderID = objOrder.OrderID,
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
                        sendVerificationEmailMOASManager(objOrder, objlstMOASApprover[i].LoginEmail, objlstMOASApprover[i].FirstName, objlstMOASApprover[i].UserInfoID, objBillingInfo, objShippingInfo);
                }
            }
            //change end on 3-feb-2012 by mayur for send email priority wise
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Sends the verification email MOAS manager for approve order.
    /// Add By Mayur on 22-Nov-2011
    /// </summary>
    /// <param name="OrderNumber">The order number.</param>
    /// <param name="MOASEmailAddress">The MOAS email address.</param>
    /// <param name="FullName">The full name.</param>
    private void sendVerificationEmailMOASManager(Order objOrder, String MOASEmailAddress, String FullName, Int64 MOASUserId, CompanyEmployeeContactInfo objBillingInfo, CompanyEmployeeContactInfo objShippingInfo)
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
                UserInformationRepository objUserInformationRepository = new UserInformationRepository();
                CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
                LookupRepository objLookupRepository = new LookupRepository();
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
                    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
                    List<SelectMyShoppingCartProductResult> objSelectMyShoppingCartProductResultList = objShoppingCartRepository.SelectCurrentOrderProducts(objOrder.OrderID);

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
                    MyIssuanceCartRepository objMyIssuanceCartRepository = new MyIssuanceCartRepository();
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
                String b = NameBars(objOrder);
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

                String NameBarList = NameBarBulkFile(objOrder);
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

                new CommonMails().SendMailWithMultiAttachment(sToUserInfoID, "Order Confirmation", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, false, true, objImage, filePath, smtphost, smtpport, smtpUserID, smtppassword, NameBarList, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private String NameBars(Order objOrder)
    {
        StringBuilder strNameBars = new StringBuilder();

        try
        {
            if (objOrder.OrderFor == "IssuanceCart")
            {
                MyIssuanceCartRepository objMyIssuanceCartRepository = new MyIssuanceCartRepository();
                List<MyIssuanceCart> objIssList = objMyIssuanceCartRepository.GetIssuanceCartByOrderID(objOrder.OrderID);
                foreach (MyIssuanceCart objItem in objIssList)
                {
                    if (objItem.NameToBeEngraved != null && objItem.NameToBeEngraved != String.Empty)
                    {
                        if (objItem.NameToBeEngraved.Contains(','))
                        {
                            String[] EmployeeTitle = objItem.NameToBeEngraved.Split(',');
                            strNameBars.Append("Name to be Engraved on Name Bar:" + EmployeeTitle[0] + "\n");
                            strNameBars.Append("Employee Title" + EmployeeTitle[1] + "\n");
                        }
                        else
                        {
                            strNameBars.Append("Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n");
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
                            strNameBars.Append("Name to be Engraved on Name Bar:" + EmployeeTitle[0] + "\n");
                            strNameBars.Append("Employee Title:" + EmployeeTitle[1] + "\n");
                        }
                        else
                        {
                            strNameBars.Append("Name to be Engraved on Name Bar:" + objItem.NameToBeEngraved + "\n");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return strNameBars.ToString();
    }

    private String NameBarBulkFile(Order objOrder)
    {
        StringBuilder strNameBars = new StringBuilder();

        try
        {
            List<SelectMyShoppingCartProductResult> objMyShpList = new MyShoppingCartRepository().SelectCurrentOrderProducts(objOrder.OrderID);

            foreach (SelectMyShoppingCartProductResult objItem in objMyShpList)
            {
                if (!String.IsNullOrEmpty(objItem.FilePath))
                {
                    if (strNameBars.Length == 0)
                        strNameBars.Append(objItem.FilePath);
                    else
                        strNameBars.Append("," + objItem.FilePath);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return strNameBars.ToString();
    }

    private Boolean checkissuancepolicy(Order objOrder)
    {
        Boolean returnvalue = false;

        try
        {
            String[] firstitem = objOrder.MyShoppingCartID.Split(',');
            MyIssuanceCart objMyIssuanceCart = new MyIssuanceCartRepository().GetByIssuanceCartId(Convert.ToInt64(firstitem[0]));
            UniformIssuancePolicyItemRepository objUniformIssuancePolicyItemRepository = new UniformIssuancePolicyItemRepository();
            UniformIssuancePolicyItem objUniformIssuancePolicyItem = objUniformIssuancePolicyItemRepository.GetById(objMyIssuanceCart.UniformIssuancePolicyItemID);

            UniformIssuancePolicy objPolicy = new UniformIssuancePolicy();
            UniformIssuancePolicyRepository objPolicyRepo = new UniformIssuancePolicyRepository();
            LookupRepository objLookupRepository = new LookupRepository();
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
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return returnvalue;
    }

    #endregion

    private void GetPurchaseUpdate()
    {
        try
        {
            new UserTrackingRepo().SetPurchase(Convert.ToInt32(Session["trackID"]));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void ShowConfirmationPopup(Boolean orderSucceed, String orderNumber, String orderStatus, String orderMessage)
    {
        try
        {
            if (orderSucceed)
            {
                h2OrderConfirmed.InnerHtml = "Order Confirmed #" + orderNumber + "";
                pOrderSentForApproval.InnerHtml = "The order was sent for review and final approval.";
                pOrderSentForApproval.Visible = orderStatus == "Order Pending";

                ClearPage();
            }
            else
            {
                if (orderMessage == "error in card processing")
                {
                    h2OrderConfirmed.InnerHtml = "Error in processing credit card.";
                    pOrderSentForApproval.InnerHtml = "An error occured while processing your credit card.";
                }
                else if (orderMessage.StartsWith("Invalid : "))
                {
                    h2OrderConfirmed.InnerHtml = "Your credit card information is invalid.";
                    pOrderSentForApproval.InnerHtml = Server.UrlDecode(orderMessage.Replace("Invalid : ", ""));
                }
                else
                {
                    h2OrderConfirmed.InnerHtml = "Order Failed";
                    pOrderSentForApproval.InnerHtml = "An error occured while processing your order.";
                }

                pOrderSentForApproval.Visible = true;
                dvPrintOrder.Visible = false;
            }

            HiddenField hdnScrollY = (HiddenField)this.Master.FindControl("hdnScrollY");
            hdnScrollY.Value = String.Empty;

            mpeProcessOrderPopup.Show();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #region Web Methods

    [ScriptMethod()]
    [WebMethod]
    public static List<String> FillSCity(String prefixText, Int32 count, String contextKey)
    {
        List<String> lstSCities = new List<String>();

        try
        {
            if (!String.IsNullOrEmpty(contextKey) && Convert.ToInt64(contextKey) > 0)
                lstSCities = new CityRepository().GetByStateId(Convert.ToInt64(contextKey)).Where(le => le.sCityName.ToUpper().StartsWith(prefixText.ToUpper())).Select(le => le.sCityName).ToList<String>();
            else
                lstSCities.Add("Please first select shipping state.");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return lstSCities;
    }

    [ScriptMethod()]
    [WebMethod]
    public static List<String> FillBCity(String prefixText, Int32 count, String contextKey)
    {
        List<String> lstBCities = new List<String>();

        try
        {
            if (!String.IsNullOrEmpty(contextKey) && Convert.ToInt64(contextKey) > 0)
                lstBCities = new CityRepository().GetByStateId(Convert.ToInt64(contextKey)).Where(le => le.sCityName.ToUpper().StartsWith(prefixText.ToUpper())).Select(le => le.sCityName).ToList<String>();
            else
                lstBCities.Add("Please first select billing state.");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return lstBCities;
    }

    [ScriptMethod()]
    [WebMethod]
    public static List<String> FillCreditCardExpirationYear()
    {
        List<String> lstYears = new List<String>();

        try
        {
            Int32 currentYear = DateTime.Now.Year;
            Int32 futureYears = 15;

            for (Int32 i = futureYears; i >= 0; i--)
            {
                lstYears.Add(currentYear.ToString());
                currentYear++;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        return lstYears;
    }

    #endregion

    #endregion
}