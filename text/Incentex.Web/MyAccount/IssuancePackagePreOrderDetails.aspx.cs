using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Text;
using System.Web.UI.HtmlControls;
using Incentex.DAL.Common;

public partial class MyAccount_IssuancePackagePreOrderDetails : PageBase
{
    #region Properties
    /// <summary>
    ///  Policy ID
    /// </summary>
    Int64 PolicyID
    {
        get
        {
            return Convert.ToInt32(ViewState["PID"]);
        }
        set
        {
            ViewState["PID"] = value;
        }
    }
    /// <summary>
    /// Policy Workgroup ID
    /// </summary>
    Int64 PolicyWorkgroupID
    {
        get
        {
            return Convert.ToInt32(ViewState["PolicyWorkgroupID"]);
        }
        set
        {
            ViewState["PolicyWorkgroupID"] = value;
        }
    }
    /// <summary>
    /// Name Bars/Formats
    /// </summary>
    String NameFormat
    {
        get
        {
            return Convert.ToString(ViewState["NameFormat"]);
        }
        set
        {
            ViewState["NameFormat"] = value;
        }
    }
    /// <summary>
    /// FontFormat
    /// </summary>
    String FontFormat
    {
        get
        {
            return Convert.ToString(ViewState["FontFormat"]);
        }
        set
        {
            ViewState["FontFormat"] = value;
        }
    }
    /// <summary>
    /// Final Name Style
    /// </summary>
    String FinalNameBarStyle
    {
        get
        {
            return Convert.ToString(ViewState["FinalNameBarStyle"]);
        }
        set
        {
            ViewState["FinalNameBarStyle"] = value;
        }
    }
    /// <summary>
    /// Final Name Bar Style For GroupAssociation
    /// </summary>
    String FinalNameBarStyleForGroupAssociation
    {
        get
        {
            return Convert.ToString(ViewState["FinalNameBarStyleForGroupAssociation"]);
        }
        set
        {
            ViewState["FinalNameBarStyleForGroupAssociation"] = value;
        }
    }
    /// <summary>
    /// Final NameBar Style For Budget Association
    /// </summary>
    string FinalNameBarStyleForBudgetAssociation
    {
        get
        {
            return Convert.ToString(ViewState["FinalNameBarStyleForBudgetAssociation"]);
        }
        set
        {
            ViewState["FinalNameBarStyleForBudgetAssociation"] = value;
        }
    }
    /// <summary>
    /// Price Level
    /// </summary>
    Int32 priceLevel
    {
        get
        {
            return Convert.ToInt32(ViewState["priceLevel"]);
        }
        set
        {
            ViewState["priceLevel"] = value;
        }
    }

    /// <summary>
    /// When Policy is active on next cycle period then no need to check my issuance cart item
    /// </summary>
    Boolean AllowonNextCycle
    {
        get
        {
            return Convert.ToBoolean(ViewState["AllowonNextCycle"]);
        }
        set
        {
            ViewState["AllowonNextCycle"] = value;
        }
    }

    /// <summary>
    /// PriceShow
    /// </summary>
    Boolean IsPriceShow
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsPriceShow"]);
        }
        set
        {
            ViewState["IsPriceShow"] = value;
        }
    }
    /// <summary>
    /// Set true when every thing is ok then we will proceed to Checkout Page
    /// </summary>
    Boolean IsProceed2CheckOut
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsProceed2CheckOut"]);
        }
        set
        {
            ViewState["IsProceed2CheckOut"] = value;
        }
    }
    /// <summary>
    /// Is Complete Purchase
    /// This will set as per Issuance policy and Eployee Rank Type.
    /// </summary>
    Boolean IsCompletePurchase
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsCompletePurchase"]);
        }
        set
        {
            ViewState["IsCompletePurchase"] = value;
        }
    }
    /// <summary>
    /// Is Open Forum Purchase Purchase
    /// </summary>
    Boolean IsOpenForumPurchase
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsOpenForumPurchase"]);
        }
        set
        {
            ViewState["IsOpenForumPurchase"] = value;
        }
    }
    /// <summary>
    /// Is Policy ReActivated
    /// </summary>
    Boolean IsPolicyReActivated
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsPolicyReActivated"]);
        }
        set
        {
            ViewState["IsPolicyReActivated"] = value;
        }
    }
    /// <summary>
    /// SingleTotal
    /// </summary>
    Decimal SingleTotal
    {
        get
        {
            return Convert.ToDecimal(ViewState["SingleTotal"]);
        }
        set
        {
            ViewState["SingleTotal"] = value;
        }
    }
    /// <summary>
    /// GroupTotal
    /// </summary>
    Decimal GroupTotal
    {
        get
        {
            return Convert.ToDecimal(ViewState["GroupTotal"]);
        }
        set
        {
            ViewState["GroupTotal"] = value;
        }
    }
    /// <summary>
    /// Group Budget Total
    /// </summary>
    Decimal GroupBudgetTotal
    {
        get
        {
            return Convert.ToDecimal(ViewState["GroupBudgetTotal"]);
        }
        set
        {
            ViewState["GroupBudgetTotal"] = value;
        }
    }
    /// <summary>
    /// String MyIssuanceCart ID's
    /// </summary>
    String MyIssuanceCartIDs
    {
        get
        {
            return Convert.ToString(ViewState["MyIssuanceCartIDs"]);
        }
        set
        {
            ViewState["MyIssuanceCartIDs"] = value;
        }
    }
    String FName = string.Empty;
    String LName = string.Empty;
    Int64? EmployeeTypeID = null;
    Int64? WorkgroupID = null;
    //
    #endregion
    #region Objects/Variables
    UniformIssuancePolicy objPolicy = new UniformIssuancePolicy();
    UniformIssuancePolicyRepository objPolicyRepo = new UniformIssuancePolicyRepository();
    UniformIssuancePolicyItemRepository objPolicyItemRepo = new UniformIssuancePolicyItemRepository();
    UniformIssuancePolicyItemRepository.UniformIssuancePolicyItemResult objPolicyItem = new UniformIssuancePolicyItemRepository.UniformIssuancePolicyItemResult();
    List<UniformIssuancePolicyItem> objPolicyItemResult = new List<UniformIssuancePolicyItem>();
    MyIssuanceCartRepository objMyIssuanceCartRepo = new MyIssuanceCartRepository();
    List<MyIssuanceCart> objMyIssuanceCart = new List<MyIssuanceCart>();
    List<MyIssuanceCartRepository.GetMyIssuanceCartDetails> objIssuanceList = new List<MyIssuanceCartRepository.GetMyIssuanceCartDetails>();
    LookupRepository objLookupRepo = new LookupRepository();
    INC_Lookup objLook = new INC_Lookup();
    ProductItemDetailsRepository objProdItemDetRepo = new ProductItemDetailsRepository();
    List<ProductItem> objProdItem = new List<ProductItem>();
    CompanyEmployeeRepository objCmpEmpRepo = new CompanyEmployeeRepository();
    CompanyEmployee objCmpEmp = new CompanyEmployee();
    OrderConfirmationRepository objOrderConfirmRepost = new OrderConfirmationRepository();
    List<Order> objOrderlist = new List<Order>();
    List<SelectWeatherTypeIdResult> objWeatherlsit = new List<SelectWeatherTypeIdResult>();
    TailoringMeasurementsRepository objRep = new TailoringMeasurementsRepository();
    List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
    List<ProductItemPricing> objPricing = new List<ProductItemPricing>();
    ChangeStatusRepository objChangeStatusRepo = new ChangeStatusRepository();
    Decimal leftQty = 0M;
    string[] MyWeatherlist = null;
    String stringWeather = "0";
    Int64? IssuanceTypeID = 0;
    Decimal? TotalAmount = 0;
    #endregion
    #region Page Load Event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Issuance Package";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/MyIssuancePolicy.aspx";
            if (!String.IsNullOrEmpty(Request.QueryString["PID"]))
                this.PolicyID = Convert.ToInt64(Request.QueryString["PID"]);
            if (!String.IsNullOrEmpty(Request.QueryString["PolicyWorkgroupID"]))
                PolicyWorkgroupID = Convert.ToInt64(Request.QueryString["PolicyWorkgroupID"]);
            SetPolicyLevelProperties();
            DisplayData();
            List<MyIssuanceCart> lstNotOrdered = objMyIssuanceCartRepo.GetUnOrderedCart(IncentexGlobal.CurrentMember.UserInfoID);
            objMyIssuanceCartRepo.DeleteAll(lstNotOrdered);
            objMyIssuanceCartRepo.SubmitChanges();
        }
    }
    #endregion
    #region Page Events
    protected void btnCheckOut_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsProcessCheckQtySuccess())
            {
                PerformProcessCheckOut();
                if (SingleTotal != 0)
                    TotalAmount += SingleTotal;
                if (GroupTotal != 0)
                    TotalAmount += GroupTotal;
                if (GroupBudgetTotal != 0)
                    TotalAmount += GroupBudgetTotal;

                Session["TotalAmount"] = TotalAmount;
                Session["MyIssuanceCartIDs"] = this.MyIssuanceCartIDs;
                Session["NotAnniversaryCreditAmount"] = null;
                if (IsProceed2CheckOut)
                    Response.Redirect("~/My Cart/CheckOutSteps.aspx?prog=ui");
            }
        }
        catch (System.Threading.ThreadAbortException tex)
        {
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// Ddl Single SelectedIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in rptSingleAssociation.Items)
        {
            HiddenField hdnStoreProductid = (HiddenField)item.FindControl("hdnStoreProductid");
            HiddenField hdnProductItemIDSingle = (HiddenField)item.FindControl("hdnProductItemIDSingle");
            DropDownList ddlSize = (DropDownList)item.FindControl("ddlSize");
            Label lblPrice = (Label)item.FindControl("lblPrice");
            HiddenField hdnMasterItemId = (HiddenField)item.FindControl("hdnMasterItemId");
            objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(Convert.ToInt32(hdnMasterItemId.Value), Convert.ToInt64(ddlSize.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
            if (objPricing.Count > 0)
            {
                if (priceLevel == 1)
                    lblPrice.Text = Convert.ToString(objPricing[0].Level1);
                else if (priceLevel == 2)
                    lblPrice.Text = Convert.ToString(objPricing[0].Level2);
                else
                    lblPrice.Text = Convert.ToString(objPricing[0].Level3);
            }
            HiddenField hdnItemNumber = (HiddenField)item.FindControl("hdnItemNumber");
            ProductItem objProduct = objPolicyItemRepo.GetProductItemIDandItemNumber(Convert.ToInt32(hdnMasterItemId.Value), Convert.ToInt32(ddlSize.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
            hdnItemNumber.Value = objProduct.ItemNumber;
            hdnProductItemIDSingle.Value = Convert.ToString(objProduct.ProductItemID);

        }
    }
    /// <summary>
    /// Ddl Group SelectedIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSizeGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in rptGroupAssociation.Items)
        {
            Repeater rptchild = (Repeater)item.FindControl("rptChildGroupAssociation");
            foreach (RepeaterItem childitem in rptchild.Items)
            {
                HiddenField hdnStoreProductid = (HiddenField)childitem.FindControl("hdnStoreProductid");
                HiddenField hdnProductItemIDGroup = (HiddenField)childitem.FindControl("hdnProductItemIDGroup");
                DropDownList ddlSizeGroup = (DropDownList)childitem.FindControl("ddlSizeGroup");
                Label lblPriceGroup = (Label)childitem.FindControl("lblPriceGroup");
                HiddenField hdnMasterItemIdGroup = (HiddenField)childitem.FindControl("hdnMasterItemIdGroup");
                objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(Convert.ToInt32(hdnMasterItemIdGroup.Value), Convert.ToInt64(ddlSizeGroup.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                if (objPricing.Count > 0)
                {
                    if (priceLevel == 1)
                        lblPriceGroup.Text = Convert.ToString(objPricing[0].Level1);
                    else if (priceLevel == 2)
                        lblPriceGroup.Text = Convert.ToString(objPricing[0].Level2);
                    else
                        lblPriceGroup.Text = Convert.ToString(objPricing[0].Level3);
                }
                HiddenField hdnItemNumberGroup = (HiddenField)childitem.FindControl("hdnItemNumberGroup");
                ProductItem objProduct = objPolicyItemRepo.GetProductItemIDandItemNumber(Convert.ToInt32(hdnMasterItemIdGroup.Value), Convert.ToInt32(ddlSizeGroup.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
                hdnItemNumberGroup.Value = objProduct.ItemNumber;
                hdnProductItemIDGroup.Value = Convert.ToString(objProduct.ProductItemID);

            }
        }
    }
    /// <summary>
    /// Ddl Group Budget SelectedIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSizeGrpB_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in rptGroupBudgetAssociation.Items)
        {
            Repeater rptchild = (Repeater)item.FindControl("rptChildGroupBudgetAssociation");
            foreach (RepeaterItem childitem in rptchild.Items)
            {
                HiddenField hdnStoreProductid = (HiddenField)childitem.FindControl("hdnStoreProductid");
                HiddenField hdnProductItemIDGrpBudget = (HiddenField)childitem.FindControl("hdnProductItemIDGrpBudget");
                DropDownList ddlSizeGrpB = (DropDownList)childitem.FindControl("ddlSizeGrpB");
                Label lblGroupBudgetPrice = (Label)childitem.FindControl("lblGroupBudgetPrice");
                HiddenField hdnMasterItemIdGrpB = (HiddenField)childitem.FindControl("hdnMasterItemIdGrpB");
                objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(Convert.ToInt32(hdnMasterItemIdGrpB.Value), Convert.ToInt64(ddlSizeGrpB.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                if (objPricing.Count > 0)
                {
                    if (priceLevel == 1)
                        lblGroupBudgetPrice.Text = Convert.ToString(objPricing[0].Level1);
                    else if (priceLevel == 2)
                        lblGroupBudgetPrice.Text = Convert.ToString(objPricing[0].Level2);
                    else
                        lblGroupBudgetPrice.Text = Convert.ToString(objPricing[0].Level3);
                }
                HiddenField hdnBudgetItemNumber = (HiddenField)childitem.FindControl("hdnBudgetItemNumber");
                ProductItem objProduct = objPolicyItemRepo.GetProductItemIDandItemNumber(Convert.ToInt32(hdnMasterItemIdGrpB.Value), Convert.ToInt32(ddlSizeGrpB.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
                hdnBudgetItemNumber.Value = objProduct.ItemNumber;
                hdnProductItemIDGrpBudget.Value = Convert.ToString(objProduct.ProductItemID);

            }
        }
    }
    #endregion
    #region Single Association Type
    protected void rptSingleAssociation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                #region Set Images
                HiddenField hdnEmployeetype = (HiddenField)e.Item.FindControl("hdnEmployeetype");
                HiddenField hdnWeathertype = (HiddenField)e.Item.FindControl("hdnWeathertype");
                objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                if (objCmpEmp != null && Convert.ToInt32(hdnEmployeetype.Value) != 0 &&
                    hdnWeathertype.Value == "0" && (objCmpEmp.EmployeeTypeID != Convert.ToInt64(hdnEmployeetype.Value)))
                {
                    e.Item.Visible = false;
                }
                HiddenField hdnLookupIcon = (HiddenField)e.Item.FindControl("hdnLookupIcon");
                HiddenField hdnProductImage = (HiddenField)e.Item.FindControl("hdnProductImage");
                HiddenField hdnlargerimagename = (HiddenField)e.Item.FindControl("hdnlargerimagename");
                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Single Item Association");
                List<SelectUniformIssuanceImageResult> obj = new List<SelectUniformIssuanceImageResult>();
                obj = objPolicyRepo.GetIssuanceProductImage(Convert.ToInt16(IssuanceTypeID), Convert.ToInt64(((UniformIssuancePolicyItem)(e.Item.DataItem)).MasterItemId), Convert.ToInt32(PolicyID));
                if (obj.Count > 0)
                {
                    hdnLookupIcon.Value = Convert.ToString(obj[0].ProductImageID);
                    hdnlargerimagename.Value = obj[0].LargerProductImage;
                    hdnProductImage.Value = obj[0].ProductImage;
                    HtmlImage htnlImageSplash = ((HtmlImage)e.Item.FindControl("imgSplashImage"));
                    HtmlAnchor htnlAnchor = ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv"));
                    htnlAnchor.HRef = "~/UploadedImages/ProductImages/" + hdnlargerimagename.Value;
                    htnlImageSplash.Src = "~/UploadedImages/ProductImages/Thumbs/" + hdnProductImage.Value;
                }
                #endregion
                HiddenField hdnUniformIssuancePolicyItemID = (HiddenField)e.Item.FindControl("hdnUniformIssuancePolicyItemID");
                HiddenField hdnMasterItemId = (HiddenField)e.Item.FindControl("hdnMasterItemId");
                HiddenField hdnStoreProductid = (HiddenField)e.Item.FindControl("hdnStoreProductid");
                HiddenField hdnProductItemIDSingle = (HiddenField)e.Item.FindControl("hdnProductItemIDSingle");
                HiddenField hdnSupplierIDSingle = (HiddenField)e.Item.FindControl("hdnSupplierIDSingle");
                hdnSupplierIDSingle.Value = Convert.ToString(new StoreProductRepository().GetStoreProductSupplierID(Convert.ToInt64(hdnStoreProductid.Value)));
                #region Show Price Label
                if (IsPriceShow)
                {
                    lblPrice.Visible = true;
                    tdSinglePrice.Visible = true;
                }
                else
                {
                    lblPrice.Visible = false;
                    tdSinglePrice.Visible = false;
                }
                #endregion
                #region TxtQty  / Validation
                TextBox txtQty = (TextBox)e.Item.FindControl("txtQty");
                HiddenField hdnRemainingValue = (HiddenField)e.Item.FindControl("hdnRemainingValue");
                Decimal Qty = PurchasedAmtQty(Convert.ToInt64(objPolicyItemResult[e.Item.ItemIndex].AssociationIssuanceType), Convert.ToInt64(hdnUniformIssuancePolicyItemID.Value), "single", null);
                hdnRemainingValue.Value = Convert.ToString(objPolicyItemResult[e.Item.ItemIndex].Issuance - Convert.ToInt32(Qty));
                String JavaScriptValidation = "CheckNum('" + txtQty.ClientID + "','" + hdnRemainingValue.Value + "','" + IsCompletePurchase + "');";
                txtQty.Attributes.Add("onchange", JavaScriptValidation);
                #endregion
                #region Load Issuance and Balance
                Label lblIssuance = (Label)e.Item.FindControl("lblIssuance");
                Label lblBalance = (Label)e.Item.FindControl("lblBalance");
                Label lblPurchase = (Label)e.Item.FindControl("lblPurchase");
                lblBalance.Text = (objPolicyItemResult[e.Item.ItemIndex].Issuance - Qty).ToString();
                lblPurchase.Text = Qty.ToString();
                #endregion
                #region Size Dropdown Fill
                objProdItem = objProdItemDetRepo.GetAllProductItem(Convert.ToInt32(hdnMasterItemId.Value), Convert.ToInt64(hdnStoreProductid.Value));
                hdnProductItemIDSingle.Value = Convert.ToString(objProdItem[0].ProductItemID);
                #region For Name Bars
                string Stylename = objLookupRepo.GetById(objProdItem[0].ProductStyleID).sLookupName.ToString();
                string ProdcutStyleName = objLookupRepo.GetById(objProdItem[0].ItemSizeID).sLookupName.ToString();
                if (Stylename == "Name Bars")
                {
                    if (ProdcutStyleName == "No Size")
                    {
                        this.NameFormat = objProdItem[0].NameFormatForNameBars;
                        this.FontFormat = objProdItem[0].FontFormatForNameBars;
                        this.FName = IncentexGlobal.CurrentMember.FirstName;
                        this.LName = IncentexGlobal.CurrentMember.LastName;
                        this.FinalNameBarStyle = SetNameFontFormat();
                        /*Employee Title */
                        objCmpEmp = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                        if (objCmpEmp.EmployeeTitleId != null)
                            this.FinalNameBarStyle = this.FinalNameBarStyle + "," + objLookupRepo.GetById((long)objCmpEmp.EmployeeTitleId).sLookupName.ToString();
                        /*End*/
                        Panel pnlNameBarsForSingleAssociation = (Panel)e.Item.FindControl("pnlNameBarsForSingleAssociation");
                        if (this.FinalNameBarStyle != string.Empty)
                        {
                            pnlNameBarsForSingleAssociation.Visible = true;
                            if (this.FinalNameBarStyle.Contains(','))
                            {
                                string[] EmployeeTitle = (this.FinalNameBarStyle.Split(','));
                                ((Label)e.Item.FindControl("lblNameTobeEngravedForSingleAssociation")).Text = EmployeeTitle[0];
                                ((Label)e.Item.FindControl("lblEmplTitleForSingleAssociation")).Text = EmployeeTitle[1];
                            }
                            else
                                ((Label)e.Item.FindControl("lblNameTobeEngravedForSingleAssociation")).Text = this.FinalNameBarStyle;
                        }
                        else
                            pnlNameBarsForSingleAssociation.Visible = false;

                    }
                }

                #endregion

                DropDownList ddlSize = (DropDownList)e.Item.FindControl("ddlSize");
                BindDdlSize(ddlSize, Convert.ToInt64(hdnStoreProductid.Value), objProdItem);
                objLook = objLookupRepo.GetById(Convert.ToInt32(hdnMasterItemId.Value));
                Label lblItemNumber = (Label)e.Item.FindControl("lblItemNumber");
                lblItemNumber.Text = objLook.sLookupName;
                HiddenField hdnItemNumber = (HiddenField)e.Item.FindControl("hdnItemNumber");
                hdnItemNumber.Value = objPolicyItemRepo.GetItemNumber(Convert.ToInt32(hdnMasterItemId.Value), Convert.ToInt32(ddlSize.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
                #region Set link of sizing Chart
                HyperLink lnkSizingChart = (HyperLink)e.Item.FindControl("lnkSizingChart");
                List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
                objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(Convert.ToInt64(hdnMasterItemId.Value), Convert.ToInt64(hdnStoreProductid.Value));
                if (objCount.Count > 0 && objCount[0].TailoringMeasurementChart != "")
                {
                    lnkSizingChart.Visible = true;
                    lnkSizingChart.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringMeasurementChart;
                }
                else
                {
                    lnkSizingChart.Visible = false;
                    lnkSizingChart.NavigateUrl = "";
                }
                #endregion
                #region Set Item Pricing
                objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(Convert.ToInt32(hdnMasterItemId.Value), Convert.ToInt64(ddlSize.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                if (objPricing.Count > 0)
                {
                    if (priceLevel == 1)
                        lblPrice.Text = Convert.ToString(objPricing[0].Level1);
                    else if (priceLevel == 2)
                        lblPrice.Text = Convert.ToString(objPricing[0].Level2);
                    else
                        lblPrice.Text = Convert.ToString(objPricing[0].Level3);
                }
                #endregion
                #region CHECK TAILORING OPTION TRUE FALASE
                Panel pnlTailoring = (Panel)e.Item.FindControl("pnlTailoring");
                List<SelectTailoringOptionActiveInActiveResult> objTailoring;
                objTailoring = objPolicyItemRepo.GetTailoringActiveInActive(Convert.ToInt32(hdnMasterItemId.Value), int.Parse(ddlSize.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
                if (objTailoring.Count > 0)
                {
                    if (objTailoring[0].sLookupName == "Active")
                    {
                        if (!string.IsNullOrEmpty(objTailoring[0].TailoringRunCharge))
                            ((Label)e.Item.FindControl("lblRunCharge")).Text = objTailoring[0].TailoringRunCharge.ToString();

                        pnlTailoring.Visible = true;
                    }
                    else
                        pnlTailoring.Visible = false;
                }
                #endregion

                #endregion
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    #endregion
    #region Group Association Type
    protected void rptGroupAssociation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnGroupName = (HiddenField)e.Item.FindControl("hdnGroupName");
                Repeater rptChild = (Repeater)e.Item.FindControl("rptChildGroupAssociation");
                Label lblGroupItemAssociation = (Label)e.Item.FindControl("lblGroupItemAssociation");
                Label tdGroupPrice = (Label)e.Item.FindControl("tdGroupPrice");
                BindData(rptChild, "Create Group Association", true, hdnGroupName.Value, lblGroupItemAssociation, dvGroup, pnlGroup);
                #region Show Price Label
                if (IsPriceShow)
                    tdGroupPrice.Visible = true;
                else
                    tdGroupPrice.Visible = false;
                #endregion
                #region Load Issuance and Balance
                Label lblIssuanceGroup = (Label)e.Item.FindControl("lblIssuanceGroup");
                Label lblBalanceGroup = (Label)e.Item.FindControl("lblBalanceGroup");
                Label lblPurchaseGroup = (Label)e.Item.FindControl("lblPurchaseGroup");
                lblPurchaseGroup.Text = Convert.ToString(leftQty);
                lblBalanceGroup.Text = Convert.ToString(Convert.ToInt32(lblIssuanceGroup.Text) - (Convert.ToInt32(lblPurchaseGroup.Text)));
                #endregion
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void rptChildGroupAssociation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                #region Set Images
                HiddenField hdnEmployeetype = (HiddenField)e.Item.FindControl("hdnEmployeetype");
                HiddenField hdnWeathertype = (HiddenField)e.Item.FindControl("hdnWeathertype");
                objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                if (objCmpEmp != null && Convert.ToInt32(hdnEmployeetype.Value) != 0 &&
                    hdnWeathertype.Value == "0" && (objCmpEmp.EmployeeTypeID != Convert.ToInt64(hdnEmployeetype.Value)))
                {
                    e.Item.Visible = false;
                }
                HiddenField hdnLookupIcon = (HiddenField)e.Item.FindControl("hdnLookupIcon");
                HiddenField hdnProductImage = (HiddenField)e.Item.FindControl("hdnProductImage");
                HiddenField hdnlargerimagename = (HiddenField)e.Item.FindControl("hdnlargerimagename");
                IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Group Association");
                List<SelectUniformIssuanceImageResult> obj = new List<SelectUniformIssuanceImageResult>();
                obj = objPolicyRepo.GetIssuanceProductImage(Convert.ToInt16(IssuanceTypeID), Convert.ToInt64(((UniformIssuancePolicyItem)(e.Item.DataItem)).MasterItemId), Convert.ToInt32(PolicyID));
                if (obj.Count > 0)
                {
                    hdnLookupIcon.Value = Convert.ToString(obj[0].ProductImageID);
                    hdnlargerimagename.Value = obj[0].LargerProductImage;
                    hdnProductImage.Value = obj[0].ProductImage;
                    HtmlImage htnlImageSplash = ((HtmlImage)e.Item.FindControl("imgSplashImage"));
                    HtmlAnchor htnlAnchor = ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv"));
                    htnlAnchor.HRef = "~/UploadedImages/ProductImages/" + hdnlargerimagename.Value;
                    htnlImageSplash.Src = "~/UploadedImages/ProductImages/Thumbs/" + hdnProductImage.Value;
                }
                #endregion
                HiddenField hdnUniformIssuancePolicyItemIDGroup = (HiddenField)e.Item.FindControl("hdnUniformIssuancePolicyItemIDGroup");
                HiddenField hdnMasterItemIdGroup = (HiddenField)e.Item.FindControl("hdnMasterItemIdGroup");
                HiddenField hdnStoreProductid = (HiddenField)e.Item.FindControl("hdnStoreProductid");
                HiddenField hdnProductItemIDGroup = (HiddenField)e.Item.FindControl("hdnProductItemIDGroup");
                HiddenField hdnSupplierIDGroup = (HiddenField)e.Item.FindControl("hdnSupplierIDGroup");
                hdnSupplierIDGroup.Value = Convert.ToString(new StoreProductRepository().GetStoreProductSupplierID(Convert.ToInt64(hdnStoreProductid.Value)));
                Label lblPriceGroup = (Label)e.Item.FindControl("lblPriceGroup");
                #region Show Price Label
                if (IsPriceShow)
                    lblPriceGroup.Visible = true;
                else
                    lblPriceGroup.Visible = false;
                #endregion
                #region TxtQty Group / Validation
                TextBox txtQtyGroup = (TextBox)e.Item.FindControl("txtQtyGroup");
                HiddenField hdnRemainingValueGroup = (HiddenField)e.Item.FindControl("hdnRemainingValueGroup");
                Decimal Qty = PurchasedAmtQty(Convert.ToInt64(objPolicyItemResult[e.Item.ItemIndex].AssociationIssuanceType), Convert.ToInt64(hdnUniformIssuancePolicyItemIDGroup.Value), "group", objPolicyItemResult[e.Item.ItemIndex].NEWGROUP);
                leftQty = Qty;// This will set to Parent Repeater Balance left lbl.
                hdnRemainingValueGroup.Value = Convert.ToString(objPolicyItemResult[e.Item.ItemIndex].Issuance - Convert.ToInt32(Qty));
                String JavaScriptValidation = "CheckNum('" + txtQtyGroup.ClientID + "','" + hdnRemainingValueGroup.Value + "','False');";// Group Items are allow to order (sum of items of this group) upto to issuance qty.
                txtQtyGroup.Attributes.Add("onchange", JavaScriptValidation);
                #endregion

                #region Size Dropdown Fill

                objProdItem = objProdItemDetRepo.GetAllProductItem(Convert.ToInt32(hdnMasterItemIdGroup.Value), Convert.ToInt64(hdnStoreProductid.Value));
                hdnProductItemIDGroup.Value = Convert.ToString(objProdItem[0].ProductItemID);
                #region For Name Bars
                string Stylename = objLookupRepo.GetById(objProdItem[0].ProductStyleID).sLookupName.ToString();
                string ProdcutStyleName = objLookupRepo.GetById(objProdItem[0].ItemSizeID).sLookupName.ToString();
                if (Stylename == "Name Bars")
                {
                    if (ProdcutStyleName == "No Size")
                    {
                        this.NameFormat = objProdItem[0].NameFormatForNameBars;
                        this.FontFormat = objProdItem[0].FontFormatForNameBars;
                        this.FName = IncentexGlobal.CurrentMember.FirstName;
                        this.LName = IncentexGlobal.CurrentMember.LastName;
                        this.FinalNameBarStyle = SetNameFontFormat();
                        /*Employee Title */
                        objCmpEmp = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                        if (objCmpEmp.EmployeeTitleId != null)
                            this.FinalNameBarStyle = this.FinalNameBarStyle + "," + objLookupRepo.GetById((long)objCmpEmp.EmployeeTitleId).sLookupName.ToString();
                        /*End*/
                        Panel pnlNameBarsForGroupAssociation = (Panel)e.Item.FindControl("pnlNameBarsForGroupAssociation");
                        if (this.FinalNameBarStyle != string.Empty)
                        {
                            pnlNameBarsForGroupAssociation.Visible = true;
                            if (this.FinalNameBarStyle.Contains(','))
                            {
                                string[] EmployeeTitle = (this.FinalNameBarStyle.Split(','));
                                ((Label)e.Item.FindControl("lblNameTobeEngravedForGroupAssociation")).Text = EmployeeTitle[0];
                                ((Label)e.Item.FindControl("lblEmplTitleForGroupAssociation")).Text = EmployeeTitle[1];
                            }
                            else
                                ((Label)e.Item.FindControl("lblNameTobeEngravedForGroupAssociation")).Text = this.FinalNameBarStyle;
                        }
                        else
                            pnlNameBarsForGroupAssociation.Visible = false;

                    }
                }

                #endregion

                DropDownList ddlSizeGroup = (DropDownList)e.Item.FindControl("ddlSizeGroup");
                BindDdlSize(ddlSizeGroup, Convert.ToInt64(hdnStoreProductid.Value), objProdItem);
                #endregion
                objLook = objLookupRepo.GetById(Convert.ToInt32(hdnMasterItemIdGroup.Value));
                Label lblItemNumberGroup = (Label)e.Item.FindControl("lblItemNumberGroup");
                lblItemNumberGroup.Text = objLook.sLookupName;
                HiddenField hdnItemNumberGroup = (HiddenField)e.Item.FindControl("hdnItemNumberGroup");
                hdnItemNumberGroup.Value = objPolicyItemRepo.GetItemNumber(Convert.ToInt32(hdnMasterItemIdGroup.Value), Convert.ToInt32(ddlSizeGroup.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
                #region Set link of sizing Chart
                HyperLink lnkSizingChartGroup = (HyperLink)e.Item.FindControl("lnkSizingChartGroup");
                List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
                objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(Convert.ToInt64(hdnMasterItemIdGroup.Value), Convert.ToInt64(hdnStoreProductid.Value));
                if (objCount.Count > 0 && objCount[0].TailoringMeasurementChart != "")
                {
                    lnkSizingChartGroup.Visible = true;
                    lnkSizingChartGroup.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringMeasurementChart;
                }
                else
                {
                    lnkSizingChartGroup.Visible = false;
                    lnkSizingChartGroup.NavigateUrl = "";
                }
                #endregion
                #region Set Item Pricing
                objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(Convert.ToInt32(hdnMasterItemIdGroup.Value), Convert.ToInt64(ddlSizeGroup.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                if (objPricing.Count > 0)
                {
                    if (priceLevel == 1)
                        lblPriceGroup.Text = Convert.ToString(objPricing[0].Level1);
                    else if (priceLevel == 2)
                        lblPriceGroup.Text = Convert.ToString(objPricing[0].Level2);
                    else
                        lblPriceGroup.Text = Convert.ToString(objPricing[0].Level3);

                }
                #endregion
                #region CHECK TAILORING OPTION TRUE FALASE
                Panel pnlTailoringGroup = (Panel)e.Item.FindControl("pnlTailoringGroup");
                List<SelectTailoringOptionActiveInActiveResult> objTailoring;
                objTailoring = objPolicyItemRepo.GetTailoringActiveInActive(Convert.ToInt32(hdnMasterItemIdGroup.Value), int.Parse(ddlSizeGroup.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
                if (objTailoring.Count > 0)
                {
                    if (objTailoring[0].sLookupName == "Active")
                    {
                        if (!string.IsNullOrEmpty(objTailoring[0].TailoringRunCharge))
                            ((Label)e.Item.FindControl("lblRunCharge")).Text = objTailoring[0].TailoringRunCharge.ToString();

                        pnlTailoringGroup.Visible = true;
                    }
                    else
                        pnlTailoringGroup.Visible = false;
                }
                #endregion


            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    #endregion
    #region Group Budget Association Type
    protected void rptGroupBudgetAssociation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnGroupName = (HiddenField)e.Item.FindControl("hdnGroupBName");
                Repeater rptChild = (Repeater)e.Item.FindControl("rptChildGroupBudgetAssociation");
                Label lblGroupItemAssociationBudget = (Label)e.Item.FindControl("lblGroupItemAssociationBudget");
                Label tdBudgetPrice = (Label)e.Item.FindControl("tdBudgetPrice");
                BindData(rptChild, "Create Group Association with Budget", true, hdnGroupName.Value, lblGroupItemAssociationBudget, dvGroupBudget, pnlGroupBudget);
                #region Show Price Label
                if (IsPriceShow)
                    tdBudgetPrice.Visible = true;
                else
                    tdBudgetPrice.Visible = false;
                #endregion
                #region Load Issuance and Balance
                Label lblGroupBudgetAmount = (Label)e.Item.FindControl("lblGroupBudgetAmount");
                Label lblGroupBudgetAmountLeft = (Label)e.Item.FindControl("lblGroupBudgetAmountLeft");
                Label lblBudgetUsed = (Label)e.Item.FindControl("lblBudgetUsed");
                lblBudgetUsed.Text = Convert.ToString(leftQty);
                lblGroupBudgetAmountLeft.Text = Convert.ToString(Convert.ToDecimal(lblGroupBudgetAmount.Text) - (Convert.ToDecimal(lblBudgetUsed.Text)));
                #endregion

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void rptChildGroupBudgetAssociation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                #region Set Images
                HiddenField hdnEmployeetype = (HiddenField)e.Item.FindControl("hdnEmployeetype");
                HiddenField hdnWeathertype = (HiddenField)e.Item.FindControl("hdnWeathertype");
                objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
                if (objCmpEmp != null && Convert.ToInt32(hdnEmployeetype.Value) != 0 &&
                    hdnWeathertype.Value == "0" && (objCmpEmp.EmployeeTypeID != Convert.ToInt64(hdnEmployeetype.Value)))
                {
                    e.Item.Visible = false;
                }
                HiddenField hdnLookupIcon = (HiddenField)e.Item.FindControl("hdnLookupIcon");
                HiddenField hdnProductImage = (HiddenField)e.Item.FindControl("hdnProductImage");
                HiddenField hdnlargerimagename = (HiddenField)e.Item.FindControl("hdnlargerimagename");

                IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Group Association with Budget");
                List<SelectUniformIssuanceImageResult> obj = new List<SelectUniformIssuanceImageResult>();
                obj = objPolicyRepo.GetIssuanceProductImage(Convert.ToInt16(IssuanceTypeID), Convert.ToInt64(((UniformIssuancePolicyItem)(e.Item.DataItem)).MasterItemId), Convert.ToInt32(PolicyID));
                if (obj.Count > 0)
                {
                    hdnLookupIcon.Value = Convert.ToString(obj[0].ProductImageID);
                    hdnlargerimagename.Value = obj[0].LargerProductImage;
                    hdnProductImage.Value = obj[0].ProductImage;
                    HtmlImage htnlImageSplash = ((HtmlImage)e.Item.FindControl("imgSplashImage"));
                    HtmlAnchor htnlAnchor = ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv"));
                    htnlAnchor.HRef = "~/UploadedImages/ProductImages/" + hdnlargerimagename.Value;
                    htnlImageSplash.Src = "~/UploadedImages/ProductImages/Thumbs/" + hdnProductImage.Value;
                }
                #endregion
                HiddenField hdnUniformIssuancePolicyItemIDGrpB = (HiddenField)e.Item.FindControl("hdnUniformIssuancePolicyItemIDGrpB");
                HiddenField hdnMasterItemIdGrpB = (HiddenField)e.Item.FindControl("hdnMasterItemIdGrpB");
                Label lblGroupBudgetPrice = (Label)e.Item.FindControl("lblGroupBudgetPrice");
                HiddenField hdnStoreProductid = (HiddenField)e.Item.FindControl("hdnStoreProductid");
                HiddenField hdnProductItemIDGrpBudget = (HiddenField)e.Item.FindControl("hdnProductItemIDGrpBudget");
                HiddenField hdnSupplierIDGrpBudget = (HiddenField)e.Item.FindControl("hdnSupplierIDGrpBudget");
                hdnSupplierIDGrpBudget.Value = Convert.ToString(new StoreProductRepository().GetStoreProductSupplierID(Convert.ToInt64(hdnStoreProductid.Value)));
                #region Show Price Label
                if (IsPriceShow)
                    lblGroupBudgetPrice.Visible = true;
                else
                    lblGroupBudgetPrice.Visible = false;
                #endregion

                #region TxtQty  / Validation
                TextBox txtQtyGrpB = (TextBox)e.Item.FindControl("txtQtyGrpB");
                HiddenField hdnRemainingValueGrpB = (HiddenField)e.Item.FindControl("hdnRemainingValueGrpB");
                Decimal Qty = PurchasedAmtQty(Convert.ToInt64(objPolicyItemResult[e.Item.ItemIndex].AssociationIssuanceType), Convert.ToInt64(hdnUniformIssuancePolicyItemIDGrpB.Value), "groupbudget", objPolicyItemResult[e.Item.ItemIndex].NEWGROUP);
                leftQty = Qty;// This will set to Parent Repeater Balance left lbl.
                hdnRemainingValueGrpB.Value = Convert.ToString(Convert.ToDecimal(objPolicyItemResult[e.Item.ItemIndex].AssociationbudgetAmt) - Qty);
                String JavaScriptValidation = "CheckNum('" + txtQtyGrpB.ClientID + "','" + hdnRemainingValueGrpB.Value + "','False');";// Group Items are allow to order (sum of items of this group) upto to issuance qty.
                txtQtyGrpB.Attributes.Add("onchange", JavaScriptValidation);
                #endregion
                #region Size Dropdown Fill

                objProdItem = objProdItemDetRepo.GetAllProductItem(Convert.ToInt32(hdnMasterItemIdGrpB.Value), Convert.ToInt64(hdnStoreProductid.Value));
                hdnProductItemIDGrpBudget.Value = Convert.ToString(objProdItem[0].ProductItemID);
                #region For Name Bars
                string Stylename = objLookupRepo.GetById(objProdItem[0].ProductStyleID).sLookupName.ToString();
                string ProdcutStyleName = objLookupRepo.GetById(objProdItem[0].ItemSizeID).sLookupName.ToString();
                if (Stylename == "Name Bars")
                {
                    if (ProdcutStyleName == "No Size")
                    {
                        this.NameFormat = objProdItem[0].NameFormatForNameBars;
                        this.FontFormat = objProdItem[0].FontFormatForNameBars;
                        this.FName = IncentexGlobal.CurrentMember.FirstName;
                        this.LName = IncentexGlobal.CurrentMember.LastName;
                        this.FinalNameBarStyle = SetNameFontFormat();
                        /*Employee Title */
                        objCmpEmp = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                        if (objCmpEmp.EmployeeTitleId != null)
                            this.FinalNameBarStyle = this.FinalNameBarStyle + "," + objLookupRepo.GetById((long)objCmpEmp.EmployeeTitleId).sLookupName.ToString();
                        /*End*/
                        Panel pnlNameBarsForBudgetAssociation = (Panel)e.Item.FindControl("pnlNameBarsForBudgetAssociation");
                        if (this.FinalNameBarStyle != string.Empty)
                        {
                            pnlNameBarsForBudgetAssociation.Visible = true;
                            if (this.FinalNameBarStyle.Contains(','))
                            {
                                string[] EmployeeTitle = (this.FinalNameBarStyle.Split(','));
                                ((Label)e.Item.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text = EmployeeTitle[0];
                                ((Label)e.Item.FindControl("lblEmplTitleForBudgetAssociation")).Text = EmployeeTitle[1];
                            }
                            else
                                ((Label)e.Item.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text = this.FinalNameBarStyle;
                        }
                        else
                            pnlNameBarsForBudgetAssociation.Visible = false;
                    }
                }

                #endregion

                DropDownList ddlSizeGrpB = (DropDownList)e.Item.FindControl("ddlSizeGrpB");
                BindDdlSize(ddlSizeGrpB, Convert.ToInt64(hdnStoreProductid.Value), objProdItem);
                #endregion
                objLook = objLookupRepo.GetById(Convert.ToInt32(hdnMasterItemIdGrpB.Value));
                Label lblItemNumberGrpB = (Label)e.Item.FindControl("lblItemNumberGrpB");
                lblItemNumberGrpB.Text = objLook.sLookupName;
                HiddenField hdnBudgetItemNumber = (HiddenField)e.Item.FindControl("hdnBudgetItemNumber");
                hdnBudgetItemNumber.Value = objPolicyItemRepo.GetItemNumber(Convert.ToInt32(hdnMasterItemIdGrpB.Value), Convert.ToInt32(ddlSizeGrpB.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
                #region Set link of sizing Chart
                HyperLink lnkGroupBudgetSizingChart = (HyperLink)e.Item.FindControl("lnkGroupBudgetSizingChart");
                List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
                objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(Convert.ToInt64(hdnMasterItemIdGrpB.Value), Convert.ToInt64(hdnStoreProductid.Value));
                if (objCount.Count > 0 && objCount[0].TailoringMeasurementChart != "")
                {
                    lnkGroupBudgetSizingChart.Visible = true;
                    lnkGroupBudgetSizingChart.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringMeasurementChart;
                }
                else
                {
                    lnkGroupBudgetSizingChart.Visible = false;
                    lnkGroupBudgetSizingChart.NavigateUrl = "";
                }
                #endregion
                #region Set Item Pricing
                objPricing = objMyIssuanceCartRepo.GetL1L2L3ByMasterItemIdandItemSizeId(Convert.ToInt32(hdnMasterItemIdGrpB.Value), Convert.ToInt64(ddlSizeGrpB.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                if (objPricing.Count > 0)
                {
                    if (priceLevel == 1)
                    {
                        lblGroupBudgetPrice.Text = Convert.ToString(objPricing[0].Level1);
                    }
                    else if (priceLevel == 2)
                    {
                        lblGroupBudgetPrice.Text = Convert.ToString(objPricing[0].Level2);
                    }
                    else
                    {
                        lblGroupBudgetPrice.Text = Convert.ToString(objPricing[0].Level3);
                    }

                }
                #endregion
                #region CHECK TAILORING OPTION TRUE FALASE
                Panel pnlTailoringGroupBudget = (Panel)e.Item.FindControl("pnlTailoringGroupBudget");
                List<SelectTailoringOptionActiveInActiveResult> objTailoring;
                objTailoring = objPolicyItemRepo.GetTailoringActiveInActive(Convert.ToInt32(hdnMasterItemIdGrpB.Value), int.Parse(ddlSizeGrpB.SelectedValue), Convert.ToInt32(hdnStoreProductid.Value));
                if (objTailoring.Count > 0)
                {
                    if (objTailoring[0].sLookupName == "Active")
                    {
                        if (!string.IsNullOrEmpty(objTailoring[0].TailoringRunCharge))
                            ((Label)e.Item.FindControl("lblRunCharge")).Text = objTailoring[0].TailoringRunCharge.ToString();

                        pnlTailoringGroupBudget.Visible = true;
                    }
                    else
                        pnlTailoringGroupBudget.Visible = false;
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion
    #region Page Method's
    private void DisplayData()
    {
        try
        {
            // For Sinlge Item associations
            BindData(this.rptSingleAssociation, "Create Single Item Association", false, null, this.lblSingleItemAssociation, dvSingle, pnlSingle);
            // For Group Associations
            BindData(this.rptGroupAssociation, "Create Group Association", true, null, null, dvGroup, pnlGroup);
            // For Group Budget Associations
            BindData(this.rptGroupBudgetAssociation, "Create Group Association with Budget", true, null, null, dvGroupBudget, pnlGroupBudget);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// Bind Size drop down List
    /// </summary>
    /// <param name="ddlsize"></param>
    /// <param name="StoreProductid"></param>
    /// <param name="listProductItem"></param>
    private void BindDdlSize(DropDownList ddlsize, Int64 StoreProductid, List<ProductItem> listProductItem)
    {
        if (ddlsize != null)
        {
            try
            {
                ddlsize.DataSource = objProdItemDetRepo.GetALLSize(StoreProductid).ToList();
                ddlsize.DataValueField = "ItemSizeID";
                ddlsize.DataTextField = "SizeName";
                ddlsize.DataBind();
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }
    }
    /// <summary>
    /// Set Name Format to Upper/Lower
    /// </summary>
    /// <returns></returns>
    private string SetNameFontFormat()
    {
        string FinalNameFormat = string.Empty;
        if (this.NameFormat.ToString() != string.Empty)
        {
            if (this.NameFormat == "First Name")
            {
                if (this.FontFormat == "All Caps")
                    FinalNameFormat = this.FName.ToUpper();
                else if (this.FontFormat == "Upper and Lower Case")
                    FinalNameFormat = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.FName.ToLower());
                else
                    FinalNameFormat = this.FName;
            }
            else if (this.NameFormat == "Last Name")
            {
                if (this.FontFormat == "All Caps")
                    FinalNameFormat = this.LName.ToUpper();
                else if (this.FontFormat == "Upper and Lower Case")
                    FinalNameFormat = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.LName.ToLower());
                else
                    FinalNameFormat = this.LName;

            }
            else if (this.NameFormat == "First Initial.LastName")
            {
                if (this.FontFormat == "All Caps")
                    FinalNameFormat = (this.FName.Substring(0, 1).ToString() + "." + this.LName.ToString()).ToUpper();
                else if (this.FontFormat == "Upper and Lower Case")
                    FinalNameFormat = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.FName.Substring(0, 1).ToString() + "." + LName.ToString().ToLower());
                else
                    FinalNameFormat = (this.FName.Substring(0, 1).ToString() + "." + this.LName.ToString()).ToUpper();

            }
        }
        else
        {
            if (this.FontFormat == "All Caps")
                FinalNameFormat = (this.FName + " " + this.LName).ToUpper();
            else if (this.FontFormat == "Upper and Lower Case")
                FinalNameFormat = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase((this.FName + " " + this.LName));
            else
                FinalNameFormat = (this.FName + " " + this.LName);
        }
        return FinalNameFormat;
    }
    /// <summary>
    /// Get weather list value into string with comma.
    /// </summary>
    /// <param name="BaseStationID">CE base StationId</param>
    /// <returns></returns>
    private String GetWeatherList(Int32 BaseStationID)
    {
        string strweather = "0";
        //Select Weather from Inc_BaseStation using CE base Station
        objWeatherlsit = objPolicyItemRepo.GetWeatherTypeid(BaseStationID);
        if (objWeatherlsit.Count > 0 && objWeatherlsit[0].WEATHERTYPE != null)
        {
            MyWeatherlist = objWeatherlsit[0].WEATHERTYPE.Split(',');
            for (int i = 0; i < MyWeatherlist.Count(); i++)
            {
                if (i == 0)
                    strweather = "'" + Convert.ToString(MyWeatherlist[i]) + "'";
                else
                    strweather += ",'" + Convert.ToString(MyWeatherlist[i]) + "'";
            }
        }
        return strweather;
    }
    private Boolean IsProcessCheckQtySuccess()
    {
        lblGroupBudgetMsg.Visible = false;
        lblSingleMsg.Visible = false;
        lblGroupMsg.Visible = false;
        Boolean isSuccess = true;
        #region Check items qty's
        #region Single
        foreach (RepeaterItem itemS in rptSingleAssociation.Items)
        {
            Int32 StotalQty = 0;
            Int32 StotalLeftQty = 0;
            TextBox txtQty = (TextBox)itemS.FindControl("txtQty");
            HiddenField hdnRemainingValue = (HiddenField)itemS.FindControl("hdnRemainingValue");
            DropDownList ddlSize = (DropDownList)itemS.FindControl("ddlSize");
            HiddenField hdnMasterItemId = (HiddenField)itemS.FindControl("hdnMasterItemId");
            HiddenField hdnStoreProductid = (HiddenField)itemS.FindControl("hdnStoreProductid");
            if (!String.IsNullOrEmpty(txtQty.Text))
                StotalQty += Convert.ToInt32(txtQty.Text);
            if (!String.IsNullOrEmpty(hdnRemainingValue.Value))
                StotalLeftQty += Convert.ToInt32(hdnRemainingValue.Value);
            if (IsCompletePurchase && StotalQty < StotalLeftQty)
            {
                lblSingleMsg.Visible = true;
                lblSingleMsg.Text = "Please enter quantity equal to total balance left";
                isSuccess = false;
                return isSuccess;
            }
            if (StotalQty > StotalLeftQty)
            {
                lblSingleMsg.Visible = true;
                lblSingleMsg.Text = "Please enter quantity equal to total balance left";
                isSuccess = false;
                return isSuccess;
            }
            // Check for Inventory level
            int Inventory = 0;
            if (!String.IsNullOrEmpty(txtQty.Text))
            {
                List<ProductItemInventory> ItemBackorder = objMyIssuanceCartRepo.GrtBackOrder1(Convert.ToInt64(hdnMasterItemId.Value), Convert.ToInt64(ddlSize.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                if (ItemBackorder.Count > 0)
                    Inventory = Convert.ToInt32(ItemBackorder[0].Inventory);
                if (Inventory != 0 && Inventory < StotalLeftQty && !GetBackOrderStatus(Convert.ToInt64(hdnStoreProductid.Value)))
                {
                    lblSingleMsg.Visible = true;
                    lblSingleMsg.Text = "Inventory level is low. You can not process order.";
                    isSuccess = false;
                    return isSuccess;
                }
            }

        }
        #endregion
        #region Group
        foreach (RepeaterItem itemG in rptGroupAssociation.Items)
        {
            Int32 GtotalQty = 0;
            Int32 GtotalLeftQty = 0;
            Repeater rptchild = (Repeater)itemG.FindControl("rptChildGroupAssociation");
            foreach (RepeaterItem itemCG in rptchild.Items)
            {
                TextBox txtQtyGroup = (TextBox)itemCG.FindControl("txtQtyGroup");
                HiddenField hdnRemainingValueGroup = (HiddenField)itemCG.FindControl("hdnRemainingValueGroup");
                DropDownList ddlSizeGroup = (DropDownList)itemCG.FindControl("ddlSizeGroup");
                HiddenField hdnMasterItemIdGroup = (HiddenField)itemCG.FindControl("hdnMasterItemIdGroup");
                HiddenField hdnStoreProductid = (HiddenField)itemCG.FindControl("hdnStoreProductid");

                if (!String.IsNullOrEmpty(txtQtyGroup.Text))
                    GtotalQty += Convert.ToInt32(txtQtyGroup.Text);
                if (!String.IsNullOrEmpty(hdnRemainingValueGroup.Value))
                    GtotalLeftQty = Convert.ToInt32(hdnRemainingValueGroup.Value);
                // Check for Inventory level
                int Inventory = 0;
                if (!String.IsNullOrEmpty(txtQtyGroup.Text))
                {
                    List<ProductItemInventory> ItemBackorder = objMyIssuanceCartRepo.GrtBackOrder1(Convert.ToInt64(hdnMasterItemIdGroup.Value), Convert.ToInt64(ddlSizeGroup.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                    if (ItemBackorder.Count > 0)
                        Inventory = Convert.ToInt32(ItemBackorder[0].Inventory);
                    if (Inventory != 0 && Inventory < Convert.ToInt32(txtQtyGroup.Text) && !GetBackOrderStatus(Convert.ToInt64(hdnStoreProductid.Value)))
                    {
                        lblGroupMsg.Visible = true;
                        lblGroupMsg.Text = "Inventory level is low. You can not process order.";
                        isSuccess = false;
                        return isSuccess;
                    }
                }
            }
            if (IsCompletePurchase && GtotalQty != 0 && (GtotalQty < GtotalLeftQty || GtotalQty > GtotalLeftQty))
            {
                lblGroupMsg.Visible = true;
                lblGroupMsg.Text = "Combined Quantity must be equal to balance left";
                isSuccess = false;
                return isSuccess;
            }
            if (GtotalQty > GtotalLeftQty)
            {
                lblGroupMsg.Visible = true;
                lblGroupMsg.Text = "Combined Quantity cannot be more than balance left";
                isSuccess = false;
                return isSuccess;
            }


        }
        #endregion
        #region Group Budget
        foreach (RepeaterItem itemGB in rptGroupBudgetAssociation.Items)
        {
            Decimal GBtotalAmt = 0M;
            Decimal GBtotalLeftAmt = 0M;
            Repeater rptchildGB = (Repeater)itemGB.FindControl("rptChildGroupBudgetAssociation");
            foreach (RepeaterItem itemCGB in rptchildGB.Items)
            {
                TextBox txtQtyGrpB = (TextBox)itemCGB.FindControl("txtQtyGrpB");
                Label lblGroupBudgetPrice = (Label)itemCGB.FindControl("lblGroupBudgetPrice");
                HiddenField hdnRemainingValueGrpB = (HiddenField)itemCGB.FindControl("hdnRemainingValueGrpB");
                DropDownList ddlSizeGrpB = (DropDownList)itemCGB.FindControl("ddlSizeGrpB");
                HiddenField hdnMasterItemIdGrpB = (HiddenField)itemCGB.FindControl("hdnMasterItemIdGrpB");
                HiddenField hdnStoreProductid = (HiddenField)itemCGB.FindControl("hdnStoreProductid");
                if (!String.IsNullOrEmpty(txtQtyGrpB.Text))
                    GBtotalAmt += Convert.ToDecimal(Convert.ToInt32(txtQtyGrpB.Text) * Convert.ToDecimal(lblGroupBudgetPrice.Text));
                if (!String.IsNullOrEmpty(hdnRemainingValueGrpB.Value))
                    GBtotalLeftAmt = Convert.ToDecimal(hdnRemainingValueGrpB.Value);

                // Check for Inventory level
                int Inventory = 0;
                if (!String.IsNullOrEmpty(txtQtyGrpB.Text))
                {
                    List<ProductItemInventory> ItemBackorder = objMyIssuanceCartRepo.GrtBackOrder1(Convert.ToInt64(hdnMasterItemIdGrpB.Value), Convert.ToInt64(ddlSizeGrpB.SelectedValue), Convert.ToInt64(hdnStoreProductid.Value));
                    if (ItemBackorder.Count > 0)
                        Inventory = Convert.ToInt32(ItemBackorder[0].Inventory);
                    if (Inventory != 0 && Inventory < Convert.ToInt32(txtQtyGrpB.Text) && !GetBackOrderStatus(Convert.ToInt64(hdnStoreProductid.Value)))
                    {
                        lblGroupMsg.Visible = true;
                        lblGroupMsg.Text = "Inventory level is low. You can not process order.";
                        isSuccess = false;
                        return isSuccess;
                    }
                }
            }
            if (GBtotalAmt != 0 && IsCompletePurchase && (GBtotalAmt < GBtotalLeftAmt || GBtotalAmt > GBtotalLeftAmt))
            {
                lblGroupBudgetMsg.Visible = true;
                lblGroupBudgetMsg.Text = "Amount must be equal to budget amount";
                isSuccess = false;
                return isSuccess;
            }
            if (GBtotalAmt > GBtotalLeftAmt)
            {
                lblGroupBudgetMsg.Visible = true;
                lblGroupBudgetMsg.Text = "Amount can not be more than budget left respectively";
                isSuccess = false;
                return isSuccess;
            }
        }
        #endregion
        #endregion
        return isSuccess;
    }
    /// <summary>
    /// Get BackOrder Status by Store Product ID
    /// </summary>
    /// <param name="storeProductID"></param>
    /// <returns></returns>
    private Boolean GetBackOrderStatus(Int64 storeProductID)
    {
        Boolean IsAllowBackOrder = false;
        StoreProduct objStoreproduct = new StoreProductRepository().GetById(storeProductID);
        if (objStoreproduct != null)
            objLook = objLookupRepo.GetById(Convert.ToInt64(objStoreproduct.AllowBackOrderID));
        if (objLook != null && objLook.sLookupName == "Yes")
            IsAllowBackOrder = true;
        return IsAllowBackOrder;
    }
    private void PerformProcessCheckOut()
    {
        IsProceed2CheckOut = false;
        #region Process Single Item Association
        foreach (RepeaterItem item in rptSingleAssociation.Items)
        {
            TextBox txtQty = (TextBox)item.FindControl("txtQty");
            if (!String.IsNullOrEmpty(txtQty.Text) && Convert.ToInt32(txtQty.Text) > 0)
            {
                DropDownList ddlSize = (DropDownList)item.FindControl("ddlSize");
                Label lblPrice = (Label)item.FindControl("lblPrice");
                Decimal pricevalue = 0M;
                if (!String.IsNullOrEmpty(lblPrice.Text))
                    pricevalue = Convert.ToDecimal(lblPrice.Text);
                HiddenField hdnMasterItemId = (HiddenField)item.FindControl("hdnMasterItemId");
                HiddenField hdnUniformIssuancePolicyItemID = (HiddenField)item.FindControl("hdnUniformIssuancePolicyItemID");
                HiddenField hdnStoreProductid = (HiddenField)item.FindControl("hdnStoreProductid");
                HiddenField hdnProductItemIDSingle = (HiddenField)item.FindControl("hdnProductItemIDSingle");
                TextBox txtTailoring = (TextBox)item.FindControl("txtSingleTailoringlength");
                HiddenField hdnItemNumber = (HiddenField)item.FindControl("hdnItemNumber");
                HiddenField hdnSupplierIDSingle = (HiddenField)item.FindControl("hdnSupplierIDSingle");
                Label lblRunCharge = ((Label)item.FindControl("lblRunCharge"));
                IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Single Item Association");
                String BackOrderStatus = null;
                string NameToBeEngravedForSingle = null;
                if (Convert.ToString(((Label)item.FindControl("lblNameTobeEngravedForSingleAssociation")).Text) != string.Empty)
                {
                    if (Convert.ToString(((Label)item.FindControl("lblEmplTitleForSingleAssociation")).Text) != string.Empty)
                    {
                        NameToBeEngravedForSingle = Convert.ToString(((Label)item.FindControl("lblNameTobeEngravedForSingleAssociation")).Text) + "," + Convert.ToString(((Label)item.FindControl("lblEmplTitleForSingleAssociation")).Text);
                    }
                    else
                    {
                        NameToBeEngravedForSingle = Convert.ToString(((Label)item.FindControl("lblNameTobeEngravedForSingleAssociation")).Text);
                    }
                }
                if (GetBackOrderStatus(Convert.ToInt64(hdnStoreProductid.Value)))
                    BackOrderStatus = "Y";

                InsertDetailsintoMyIssuanceCart(Convert.ToInt64(hdnUniformIssuancePolicyItemID.Value), Convert.ToInt64(IssuanceTypeID), Convert.ToInt64(hdnMasterItemId.Value), Convert.ToInt64(ddlSize.SelectedValue),
                    Convert.ToInt32(txtQty.Text), NameToBeEngravedForSingle, lblRunCharge.Text, Convert.ToInt64(hdnStoreProductid.Value), txtTailoring.Text, BackOrderStatus, pricevalue, this.priceLevel, hdnItemNumber.Value, "single",
                    Convert.ToInt64(hdnProductItemIDSingle.Value), Convert.ToInt64(hdnSupplierIDSingle.Value));
            }
        }
        #endregion
        #region Process Group Item Association
        foreach (RepeaterItem itemG in rptGroupAssociation.Items)
        {
            Repeater rptchild = (Repeater)itemG.FindControl("rptChildGroupAssociation");
            foreach (RepeaterItem itemCG in rptchild.Items)
            {
                TextBox txtQtyGroup = (TextBox)itemCG.FindControl("txtQtyGroup");
                if (!String.IsNullOrEmpty(txtQtyGroup.Text) && Convert.ToInt32(txtQtyGroup.Text) > 0)
                {
                    DropDownList ddlSizeGroup = (DropDownList)itemCG.FindControl("ddlSizeGroup");
                    Label lblPriceGroup = (Label)itemCG.FindControl("lblPriceGroup");
                    Decimal pricevalue = 0M;
                    if (!String.IsNullOrEmpty(lblPriceGroup.Text))
                        pricevalue = Convert.ToDecimal(lblPriceGroup.Text);
                    HiddenField hdnMasterItemIdGroup = (HiddenField)itemCG.FindControl("hdnMasterItemIdGroup");
                    HiddenField hdnUniformIssuancePolicyItemIDGroup = (HiddenField)itemCG.FindControl("hdnUniformIssuancePolicyItemIDGroup");
                    HiddenField hdnStoreProductid = (HiddenField)itemCG.FindControl("hdnStoreProductid");
                    HiddenField hdnProductItemIDGroup = (HiddenField)itemCG.FindControl("hdnProductItemIDGroup");
                    TextBox txtTailoringlengthGroup = (TextBox)itemCG.FindControl("txtTailoringlengthGroup");
                    HiddenField hdnItemNumberGroup = (HiddenField)itemCG.FindControl("hdnItemNumberGroup");
                    HiddenField hdnSupplierIDGroup = (HiddenField)itemCG.FindControl("hdnSupplierIDGroup");
                    IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Group Association");
                    String BackOrderStatus = null;
                    string NameToBeEngravedForGroup = null;
                    if (Convert.ToString(((Label)itemCG.FindControl("lblNameTobeEngravedForGroupAssociation")).Text) != string.Empty)
                    {
                        if (Convert.ToString(((Label)itemCG.FindControl("lblEmplTitleForGroupAssociation")).Text) != string.Empty)
                        {
                            NameToBeEngravedForGroup = Convert.ToString(((Label)itemCG.FindControl("lblNameTobeEngravedForGroupAssociation")).Text) + "," + Convert.ToString(((Label)itemCG.FindControl("lblEmplTitleForGroupAssociation")).Text);
                        }
                        else
                        {
                            NameToBeEngravedForGroup = Convert.ToString(((Label)itemCG.FindControl("lblNameTobeEngravedForGroupAssociation")).Text);
                        }
                    }
                    if (GetBackOrderStatus(Convert.ToInt64(hdnStoreProductid.Value)))
                        BackOrderStatus = "Y";

                    InsertDetailsintoMyIssuanceCart(Convert.ToInt64(hdnUniformIssuancePolicyItemIDGroup.Value), Convert.ToInt64(IssuanceTypeID), Convert.ToInt64(hdnMasterItemIdGroup.Value), Convert.ToInt64(ddlSizeGroup.SelectedValue),
                        Convert.ToInt32(txtQtyGroup.Text), NameToBeEngravedForGroup, null, Convert.ToInt64(hdnStoreProductid.Value), txtTailoringlengthGroup.Text, BackOrderStatus, pricevalue, this.priceLevel, hdnItemNumberGroup.Value, "group",
                    Convert.ToInt64(hdnProductItemIDGroup.Value), Convert.ToInt64(hdnSupplierIDGroup.Value));
                }
            }
        }
        #endregion
        #region Process Group Budget Item Association
        foreach (RepeaterItem itemGB in rptGroupBudgetAssociation.Items)
        {
            Repeater rptchildGB = (Repeater)itemGB.FindControl("rptChildGroupBudgetAssociation");
            foreach (RepeaterItem itemCGB in rptchildGB.Items)
            {
                TextBox txtQtyGrpB = (TextBox)itemCGB.FindControl("txtQtyGrpB");
                if (!String.IsNullOrEmpty(txtQtyGrpB.Text) && Convert.ToInt32(txtQtyGrpB.Text) > 0)
                {
                    DropDownList ddlSizeGrpB = (DropDownList)itemCGB.FindControl("ddlSizeGrpB");
                    Label lblGroupBudgetPrice = (Label)itemCGB.FindControl("lblGroupBudgetPrice");
                    Decimal pricevalue = 0M;
                    if (!String.IsNullOrEmpty(lblGroupBudgetPrice.Text))
                        pricevalue = Convert.ToDecimal(lblGroupBudgetPrice.Text);
                    HiddenField hdnMasterItemIdGrpB = (HiddenField)itemCGB.FindControl("hdnMasterItemIdGrpB");
                    HiddenField hdnUniformIssuancePolicyItemIDGrpB = (HiddenField)itemCGB.FindControl("hdnUniformIssuancePolicyItemIDGrpB");
                    HiddenField hdnStoreProductid = (HiddenField)itemCGB.FindControl("hdnStoreProductid");
                    HiddenField hdnProductItemIDGrpBudget = (HiddenField)itemCGB.FindControl("hdnProductItemIDGrpBudget");
                    HiddenField hdnSupplierIDGrpBudget = (HiddenField)itemCGB.FindControl("hdnSupplierIDGrpBudget");
                    TextBox txtBudgetTailoringlength = (TextBox)itemCGB.FindControl("txtBudgetTailoringlength");
                    HiddenField hdnBudgetItemNumber = (HiddenField)itemCGB.FindControl("hdnBudgetItemNumber");
                    IssuanceTypeID = objLookupRepo.GetIdByLookupName("Create Group Association with Budget");
                    String BackOrderStatus = null;
                    string NameToBeEngravedForGroupB = null;
                    if (Convert.ToString(((Label)itemCGB.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text) != string.Empty)
                    {
                        if (Convert.ToString(((Label)itemCGB.FindControl("lblEmplTitleForBudgetAssociation")).Text) != string.Empty)
                        {
                            NameToBeEngravedForGroupB = Convert.ToString(((Label)itemCGB.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text) + "," + Convert.ToString(((Label)itemCGB.FindControl("lblEmplTitleForBudgetAssociation")).Text);
                        }
                        else
                        {
                            NameToBeEngravedForGroupB = Convert.ToString(((Label)itemCGB.FindControl("lblNameTobeEngravedForBudgetAssociation")).Text);
                        }
                    }
                    if (GetBackOrderStatus(Convert.ToInt64(hdnStoreProductid.Value)))
                        BackOrderStatus = "Y";

                    InsertDetailsintoMyIssuanceCart(Convert.ToInt64(hdnUniformIssuancePolicyItemIDGrpB.Value), Convert.ToInt64(IssuanceTypeID), Convert.ToInt64(hdnMasterItemIdGrpB.Value), Convert.ToInt64(ddlSizeGrpB.SelectedValue),
                        Convert.ToInt32(txtQtyGrpB.Text), NameToBeEngravedForGroupB, null, Convert.ToInt64(hdnStoreProductid.Value), txtBudgetTailoringlength.Text, BackOrderStatus, pricevalue, this.priceLevel, hdnBudgetItemNumber.Value, "groupbudget",
                    Convert.ToInt64(hdnProductItemIDGrpBudget.Value), Convert.ToInt64(hdnSupplierIDGrpBudget.Value));
                }
            }
        }
        #endregion
        IsProceed2CheckOut = true;
    }
    /// <summary>
    /// Insert this detials into MyIssuance Cart 
    /// </summary>
    /// <param name="UniformIssuancePolicyItemID"></param>
    /// <param name="IssuanceTypeID"></param>
    /// <param name="MasterItemId"></param>
    /// <param name="sizeID"></param>
    /// <param name="qty"></param>
    /// <param name="NameToBeEngraved"></param>
    /// <param name="strRunCharge"></param>
    /// <param name="StoreProductID"></param>
    /// <param name="Tailoring"></param>
    /// <param name="BackOrderStatus"></param>
    /// <param name="Price"></param>
    /// <param name="pricelvl"></param>
    /// <param name="ItemNumber"></param>
    /// <param name="strFor"></param>
    private void InsertDetailsintoMyIssuanceCart(Int64 UniformIssuancePolicyItemID, Int64 IssuanceTypeID, Int64 MasterItemId, Int64 sizeID, Int32 qty, String NameToBeEngraved, String strRunCharge, Int64 StoreProductID, String Tailoring, String BackOrderStatus, Decimal Price, Int32 pricelvl, String ItemNumber, String strFor, Int64 ProductItemID, Int64 SupplierID)
    {
        try
        {
            MyIssuanceCart objCart = new MyIssuanceCart();
            objCart.UniformIssuancePolicyItemID = UniformIssuancePolicyItemID;
            objCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
            objCart.UniformIssuanceType = IssuanceTypeID;
            objCart.MasterItemID = MasterItemId;
            objCart.ItemSizeID = sizeID;
            objCart.Qty = qty;
            objCart.Rate = Price;
            //Update by prashant
            objCart.MOASRate = Price;
            objCart.MOASPriceLevel = pricelvl;
            //Update by prashant
            objCart.NameToBeEngraved = NameToBeEngraved;
            if (!String.IsNullOrEmpty(strRunCharge))
                objCart.RunCharge = strRunCharge;
            if (!String.IsNullOrEmpty(Tailoring))
                objCart.TailoringLength = strRunCharge;
            if (!String.IsNullOrEmpty(txtShippingDate.Text))
                objCart.ShippingDate = Convert.ToDateTime(txtShippingDate.Text);
            objCart.BackOrderStatus = BackOrderStatus;
            objCart.StoreProductID = StoreProductID;
            objCart.PriceLevel = pricelvl;
            objCart.ItemNumber = ItemNumber;
            objCart.CreatedDate = DateTime.Now;
            objCart.UniformIssuancePolicyID = this.PolicyID;
            if (ProductItemID > 0)
                objCart.ProductItemID = ProductItemID;
            if (SupplierID > 0)
                objCart.SupplierID = SupplierID;

            objMyIssuanceCartRepo.Insert(objCart);
            objMyIssuanceCartRepo.SubmitChanges();

            if (!String.IsNullOrEmpty(MyIssuanceCartIDs))
                MyIssuanceCartIDs += "," + objCart.MyIssuanceCartID.ToString();
            else
                MyIssuanceCartIDs = objCart.MyIssuanceCartID.ToString();

            if (strFor == "single")
                SingleTotal += Convert.ToDecimal(Price * objCart.Qty);
            else if (strFor == "group")
                GroupTotal += Convert.ToDecimal(Price * objCart.Qty);
            else if (strFor == "groupbudget")
                GroupBudgetTotal += Convert.ToDecimal(Price * objCart.Qty);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    /// <summary>
    /// Bind Repeater as per the parameters
    /// </summary>
    /// <param name="rpt">Repeater Name</param>
    /// <param name="AssociationsType"></param>
    /// <param name="IsGroup">True/False</param>
    /// <param name="GroupName">Name of Group only in Group case</param>
    /// <param name="lblItemAssociation"> Display association type name</param>
    /// <param name="dv">Div Name to show/hide</param>
    /// <param name="pnl">Panel name to show/hide</param>
    private void BindData(Repeater rpt, String AssociationsType, Boolean IsGroup, String GroupName, Label lblItemAssociation, HtmlControl dv, Panel pnl)
    {
        try
        {
            //Get Employeetype Id by user ID
            objCmpEmp = objCmpEmpRepo.GetByUserInfoId(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
            if (objCmpEmp != null)
            {
                IssuanceTypeID = objLookupRepo.GetIdByLookupName(AssociationsType);
                if (objCmpEmp.EmployeeTypeID != null && objCmpEmp.BaseStation != null)
                {
                    stringWeather = GetWeatherList(Convert.ToInt32(objCmpEmp.BaseStation));
                    objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (stringWeather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == objCmpEmp.EmployeeTypeID || j.EmployeeTypeid == 0)).ToList();
                }
                else if (objCmpEmp.EmployeeTypeID == null && objCmpEmp.BaseStation != null)
                {
                    stringWeather = GetWeatherList(Convert.ToInt32(objCmpEmp.BaseStation));
                    objPolicyItemResult = objPolicyItemRepo.GetByUniformIssuancePolicyID(this.PolicyID).Where(j => j.AssociationIssuanceType == IssuanceTypeID && (stringWeather.Contains(j.WeatherTypeid.ToString()) || j.WeatherTypeid == 0) && (j.EmployeeTypeid == 0)).ToList();
                }
            }
            if (objPolicyItemResult.Count > 0)
            {
                if (IsGroup)
                {
                    if (String.IsNullOrEmpty(Convert.ToString(GroupName)))
                    {
                        // To get Distinct value from Generic List using LINQ
                        // Create an Equality Comprarer Intance
                        IEqualityComparer<UniformIssuancePolicyItem> customComparer = new Common.PropertyComparer<UniformIssuancePolicyItem>("NEWGROUP");
                        IEnumerable<UniformIssuancePolicyItem> distinctList = objPolicyItemResult.Distinct(customComparer);
                        objPolicyItemResult = distinctList.ToList();
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(GroupName)))
                        objPolicyItemResult = objPolicyItemResult.Where(q => q.NEWGROUP == Convert.ToString(GroupName)).ToList();
                }
                if (objPolicyItemResult.Count > 0)
                {
                    dv.Visible = true;
                    pnl.Visible = true;
                    if (objPolicyItemResult[0].AssociationIssuancePolicyNote != "" && lblItemAssociation != null)
                        lblItemAssociation.Text = objPolicyItemResult[0].AssociationIssuancePolicyNote;
                }
                else
                {
                    dv.Visible = false;
                    pnl.Visible = false;
                }

                rpt.DataSource = objPolicyItemResult;
                rpt.DataBind();
            }
            else
            {
                dv.Visible = false;
                pnl.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// Check/ Set Current policy properties
    /// </summary>
    /// <returns>Null</returns>
    private void SetPolicyLevelProperties()
    {
        objPolicy = objPolicyRepo.GetById(this.PolicyID);
        if (objPolicy != null)
        {
            // Set Price Level
            this.priceLevel = Convert.ToInt32(objPolicy.PricingLevel);
            DateTime? _PolicyEligibleDate = null;
            DateTime? _PolicyExpireDate = null;
            // Set Eligible and and Expire date
            DateTime? HiredDate = null;
            objCmpEmp = objCmpEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
            EmployeeTypeID = objCmpEmp.EmployeeTypeID;
            WorkgroupID = objCmpEmp.WorkgroupID;
            HiredDate = objCmpEmp.HirerdDate;
            if (Convert.ToBoolean(objPolicy.IsDateOfHiredTicked) && objPolicy.NumberOfMonths > 0 && objPolicy.CreditExpireNumberOfMonths > 0)
            {
                DateTime dtnow = DateTime.Now;//Convert.ToDateTime("12/12/2015");
                DateTime dt = Convert.ToDateTime(HiredDate).AddMonths(Convert.ToInt32(objPolicy.NumberOfMonths));
                Double monthsDiff = (dtnow.Subtract(dt).TotalDays / 30) / Convert.ToDouble(objPolicy.NumberOfMonths);
                Int32 mlv = Math.Abs(Math.Abs((Int32)Math.Round(monthsDiff, 0)) - 1);// We can multiply this value with number of month;

                DateTime _NewDate = Convert.ToDateTime(HiredDate).AddMonths(Convert.ToInt32(objPolicy.NumberOfMonths) * mlv);

                _PolicyEligibleDate = Convert.ToDateTime(_NewDate);
                _PolicyExpireDate = Convert.ToDateTime(_NewDate).AddMonths(Convert.ToInt32(objPolicy.CreditExpireNumberOfMonths));

                if (_PolicyEligibleDate < dtnow && dtnow < _PolicyExpireDate)
                    AllowonNextCycle = true;

                //_PolicyEligibleDate = Convert.ToDateTime(HiredDate);     //_PolicyEligibleDate = Convert.ToDateTime(HiredDate).AddMonths(Convert.ToInt32(objPolicy.NumberOfMonths));
                //_PolicyExpireDate = Convert.ToDateTime(HiredDate).AddMonths(Convert.ToInt32(objPolicy.CreditExpireNumberOfMonths));

            }
            else
            {
                _PolicyEligibleDate = Convert.ToDateTime(objPolicy.EligibleDate);
                _PolicyExpireDate = Convert.ToDateTime(objPolicy.CreditExpireDate);
            }

            lblEligible.Text = Convert.ToDateTime(_PolicyEligibleDate).ToShortDateString();
            lblExpire.Text = Convert.ToDateTime(_PolicyExpireDate).ToShortDateString();

            // Set price Show
            objLook = objLookupRepo.GetById(Convert.ToInt64(objPolicy.PRICEFOR));
            if (Convert.ToString(objPolicy.SHOWPRICE) == "Y" && (objLook.sLookupName == "Both" || objLook.sLookupName == "Customer Employee" || objLook.sLookupName == "Customer Admin"))
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                    IsPriceShow = true;
            }
            else
                IsPriceShow = false;

            // Set Is Complete Purchase True
            if (objPolicy.CompletePurchase == 'Y')
                IsCompletePurchase = true;
            else if (objPolicy.CompletePurchase == 'O')
            {
                IsCompletePurchase = true;
                IsOpenForumPurchase = true;
            }
            else if (objPolicy.CompletePurchase == 'N')
                IsCompletePurchase = false;
            else
                IsCompletePurchase = false;
            //If CE is allow for partial issuance then override IsCompletePurchase = false, 
            //without consider Issuance policy complete purchase required type value and allow for partial issuance
            if (objCmpEmp.IssuancePolicyStatus == 'P')
                IsCompletePurchase = false;

            // Check for Group name of policy and Allow to order only for different group
            // Also If current policy and already order policy are same then allow for order.
            objIssuanceList = objMyIssuanceCartRepo.GetUniformIssuancePolicyDetails(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));
            Boolean IsGroupAllowPolicy = true;//default allow
            for (Int32 i = 0; i < objIssuanceList.Count; i++)
            {
                // If the purchase date is between eligible and expire date then User will not allow to purchase this policy
                if (objIssuanceList[i].UniformIssuancePolicyId == objPolicy.UniformIssuancePolicyID && _PolicyEligibleDate < objIssuanceList[i].CreatedDate && objIssuanceList[i].CreatedDate < _PolicyExpireDate)
                    AllowonNextCycle = false;

                if (objIssuanceList[i].GroupName == objPolicy.GroupName && objIssuanceList[i].WorkGroupID == objPolicy.WorkgroupID && objIssuanceList[i].UniformIssuancePolicyId != objPolicy.UniformIssuancePolicyID)
                {
                    IsGroupAllowPolicy = false;// here group name and WorkGroupID are same  , So not allow for Place order. 
                    break;
                }
                else
                    IsGroupAllowPolicy = true;// Allow for Order.
            }
            // Check whether this policy is re-activated for current user.
            IsPolicyReActivated = objChangeStatusRepo.IsUserReActivatedPolicyExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID), this.PolicyID);
            // If CE Issuance Policy  Status is not partial then
            //Check here this User Had taken any Issuance Policy then He can not take this issuance policy further
            Boolean IsSamePolicyPurchased = false;
            if (objPolicy.CompletePurchase != 'O' && !AllowonNextCycle)
            {
                objIssuanceList = objIssuanceList.Where(q => q.UniformIssuancePolicyId == this.PolicyID).ToList();
                if (objIssuanceList.Count > 0)
                    IsSamePolicyPurchased = true;
            }
            //  objOrderlist = objOrderConfirmRepost.CheckUserIDExist(Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID));

            // For Eligible Date of Policy. If Date of Eligible is greater then current date then 
            if (_PolicyEligibleDate > DateTime.Now && _PolicyExpireDate >= DateTime.Now)
            {
                btnCheckOut.Visible = false;
                lblNoRecord.Text = "Your Policy is not yet eligible: " + lblEligible.Text;
                dvNoRecord.Visible = true;
            }
            else if (_PolicyExpireDate <= DateTime.Now)
            {
                btnCheckOut.Visible = false;
                lblNoRecord.Text = "This policy is expired";
                dvNoRecord.Visible = true;
            }
            else if (PolicyWorkgroupID != objCmpEmp.WorkgroupID && IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(DAEnums.UserTypes.CompanyEmployee))
            {
                //Check Here For CA/CE Can See Other Workgroup Package.
                //But They can view other issuance packages for the other workgroups but not place orders for the secondary workgroups.
                btnCheckOut.Visible = false;
                lblNoRecord.Text = "You can not place orders for the secondary workgroups packages.";
                dvNoRecord.Visible = true;
            }
            else if (!IsGroupAllowPolicy && !IsPolicyReActivated)
            {
                btnCheckOut.Visible = false;
                lblNoRecord.Text = "You are not allow to place order for this policy. Since you had already placed order for same group.";
                dvNoRecord.Visible = true;
            }

            if (IsGroupAllowPolicy && IsSamePolicyPurchased && !IsPolicyReActivated)
            {
                btnCheckOut.Visible = false;
                lblNoRecord.Text = "You have already purchased this issuance policy.";
                dvNoRecord.Visible = true;
            }

        }

    }
    /// <summary>
    /// Check for order in myissuance table by UniformIssuancePolicyItemID
    /// </summary>
    /// <param name="IssuanceTypeID"></param>
    /// <param name="UniformIssuancePolicyItemID"></param>
    /// <param name="strFor"></param>
    /// <param name="GroupName"></param>
    /// <returns>Qty</returns>
    private Decimal PurchasedAmtQty(Int64 IssuanceTypeID, Int64 UniformIssuancePolicyItemID, String strFor, String GroupName)
    {
        Decimal? AmtQty = 0M;
        if (!IsOpenForumPurchase && !IsPolicyReActivated && !AllowonNextCycle)
        {
            objIssuanceList = objMyIssuanceCartRepo.GetIssuancePolicyItemDetails(this.PolicyID, IncentexGlobal.CurrentMember.UserInfoID);

            if (objIssuanceList != null && objIssuanceList.Count > 0)
            {
                if (strFor == "single")
                    AmtQty = objIssuanceList.Where(q => q.UniformIssuancePolicyItemID == UniformIssuancePolicyItemID && q.UniformIssuanceType == IssuanceTypeID).Sum(s => s.Qty);
                else if (strFor == "group")
                    AmtQty = objIssuanceList.Where(q => q.UniformIssuanceType == IssuanceTypeID && q.GroupName == GroupName).Sum(s => s.Qty);
                else if (strFor == "groupbudget")
                    AmtQty = objIssuanceList.Where(q => q.UniformIssuanceType == IssuanceTypeID && q.GroupName == GroupName).Sum(s => (s.Qty * s.Rate));
            }
        }
        return Convert.ToDecimal(AmtQty);
    }
    #endregion
}
