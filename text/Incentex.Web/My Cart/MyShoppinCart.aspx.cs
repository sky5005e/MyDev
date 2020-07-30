using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Incentex.DAL;
using Incentex.DA;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.Collections.Generic;

public partial class My_Cart_MyShoppinCart : PageBase
{
    #region Data Members

    Int64 MasterItemNo
    {
        get
        {
            if (ViewState["MasterItemNo"] == null)
            {
                ViewState["MasterItemNo"] = 0;
            }
            return Convert.ToInt64(ViewState["MasterItemNo"]);
        }
        set
        {
            ViewState["MasterItemNo"] = value;
        }
    }
    Int64? StoreProductId
    {
        get
        {
            if (ViewState["StoreProductId"] == null)
            {
                ViewState["StoreProductId"] = 0;
            }
            return Convert.ToInt32(ViewState["StoreProductId"]);
        }
        set
        {
            ViewState["StoreProductId"] = value;
        }
    }
    Int64 SubCatID
    {
        get
        {
            if (ViewState["SubCatID"] == null)
            {
                ViewState["SubCatID"] = 0;
            }
            return Convert.ToInt64(ViewState["SubCatID"]);
        }
        set
        {
            ViewState["SubCatID"] = value;
        }
    }
    /// <summary>
    /// Here we will set Session CoupaID 
    /// </summary>
    String CoupaID
    {
        get { return Convert.ToString(Session["CoupaID"]); }
    }
    String BuyerCookie
    {
        get
        {
            return Convert.ToString(ViewState["BuyerCookie"]);
        }
        set
        {
            ViewState["BuyerCookie"] = value;
        }
    }
    string strtotal = string.Empty;
    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    MyShoppinCart objShoppingCart = new MyShoppinCart();
    StoreProductRepository objStoreProductRepository = new StoreProductRepository();
    StoreProductImageRepository objStoreProductImageRepository = new StoreProductImageRepository();
    CompanyEmployee objCmpnyInfo = new CompanyEmployee();
    LookupRepository objLookUpRep = new LookupRepository();
    CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
    UserInformationRepository objUserRepo = new UserInformationRepository();
    INC_Lookup objLookup = new INC_Lookup();
    bool boolInventoryShow = false;
    string StrMessage = string.Empty;
    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        // For Coupa Order
        if (!string.IsNullOrEmpty(CoupaID))
        {
            CoupaPunchOutDetail objcp = new CoupaPunchOutDetail();
            objcp = new UserInformationRepository().GetCoupaPunchOutDetailbyID(Convert.ToInt64(CoupaID));
            if (objcp != null)
                this.BuyerCookie = objcp.BuyerCookie;
            else
                this.BuyerCookie = null;
        }
        else
            this.BuyerCookie = null;

        if (!IsPostBack)
        {
            if (Session["PID"] != null)
            {
                Session["PID"] = null;
            }
            if (Session["PaymentOption"] != null)
            {
                Session["PaymentOption"] = null;
            }
            this.SubCatID = Convert.ToInt64(Request.QueryString["SubCat"]);
            this.MasterItemNo = Convert.ToInt64(Request.QueryString.Get("MasterItemNo"));
            this.StoreProductId = Convert.ToInt64(Request.QueryString.Get("StoreProductId"));
            ((Label)Master.FindControl("lblPageHeading")).Text = "My Shopping Cart";

            if (this.SubCatID == null || this.SubCatID == 0)
            {
                if (IncentexGlobal.IsIEFromStore)
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
                else
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            }
            else
            {
                if (IncentexGlobal.IsIEFromStore)
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
                else if (Session["ProductListUrl"] != null)// To go back 
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = Session["ProductListUrl"].ToString();
                else
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Products/ProductDetail.aspx?SubCat=" + this.SubCatID + "&MasterItemNo=" + this.MasterItemNo + "&StoreProductId=" + this.StoreProductId;

            }

            BindData();
            Calculate();
        }
    }

    protected void rptMyShoppingCart_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "deleteProdcutItem")
        {
            MyShoppinCart objMyShoppinCart = objShoppingCartRepository.GetById(Convert.ToInt32(e.CommandArgument.ToString()), IncentexGlobal.CurrentMember.UserInfoID);

            if (objMyShoppinCart.IsBulkOrder == false)
            {
                //Delete normal cart item
                int result = objShoppingCartRepository.RemoveProductsFromCart(Convert.ToInt32(e.CommandArgument.ToString()), IncentexGlobal.CurrentMember.UserInfoID);
                objShoppingCartRepository.SubmitChanges();
            }
            else
            {
                //delete bulk order item
                DeleteBulkOrder(objMyShoppinCart.StoreProductID.ToString(), objMyShoppinCart.IsBulkOrder);
            }
        }
        GapBentweenBulkOrder = 0;
        StoreProductIdForBulkOrder = 0;
        BindData();
        Calculate();
    }

    int GapBentweenBulkOrder = 0;
    Int64? StoreProductIdForBulkOrder
    {
        get
        {
            if (ViewState["StoreProductIdForBulkOrder"] == null)
            {
                ViewState["StoreProductIdForBulkOrder"] = 0;
            }
            return Convert.ToInt32(ViewState["StoreProductIdForBulkOrder"]);
        }
        set
        {
            ViewState["StoreProductIdForBulkOrder"] = value;
        }
    }
    protected void rptMyShoppingCart_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Panel pnlNameBars = new Panel();
                pnlNameBars = (Panel)e.Item.FindControl("pnlNameBars");
                if (!string.IsNullOrEmpty(((SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved) && Convert.ToString(((SelectMyShoppingCartProductResult)(e.Item.DataItem)).NameToBeEngraved.Trim()) != "")
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
                {
                    pnlNameBars.Visible = false;

                }

                HiddenField hdnShowInventory = (HiddenField)e.Item.FindControl("hdnInventoryLevelShow");
                INC_Lookup objLook = new INC_Lookup();
                LookupRepository objLookupRepo = new LookupRepository();
                objLook = objLookupRepo.GetById(Convert.ToInt64(hdnShowInventory.Value));
                if (objLook.sLookupName == "No")
                {
                    ((Label)e.Item.FindControl("InventoryData")).Text = "-";
                }
                //Show Tailoring Status
                HiddenField hdnTailoringOption = (HiddenField)e.Item.FindControl("hdnTailoringOption");
                INC_Lookup objLook1 = new INC_Lookup();
                LookupRepository objLookupRepo1 = new LookupRepository();
                objLook1 = objLookupRepo1.GetById(Convert.ToInt64(hdnTailoringOption.Value));
                HiddenField hdnRunCharge = (HiddenField)e.Item.FindControl("hdnRunCharge");
                Panel pnlTailoring = (Panel)e.Item.FindControl("pnlTailoring");
                HiddenField hdnTailoringLength = (HiddenField)e.Item.FindControl("hdnTailoringLength");
                if (objLook1.sLookupName.ToLower() == "active" && hdnTailoringLength.Value != "")
                {

                    pnlTailoring.Visible = true;
                    //Added on 8-Mar-11 by Ankit
                    ((Label)e.Item.FindControl("lblRunChargeView")).Visible = true;
                    ((Label)e.Item.FindControl("lblRunCharge")).Text = Convert.ToDecimal(hdnRunCharge.Value).ToString("#,##0.00"); ;
                    //End       

                }
                else
                {
                    pnlTailoring.Visible = false;
                    //Added on 8-Mar-11 by Ankit
                    ((Label)e.Item.FindControl("lblRunChargeView")).Visible = false;
                    ((Label)e.Item.FindControl("lblRunCharge")).Text = "";
                    //End
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

                    if (StoreProductIdForBulkOrder != Convert.ToInt32(hdnStoreProductid.Value))
                    {
                        List<StoreProductImage> objImageList = objStoreProductImageRepository.GetStoreProductImagesByIdForShoppingCart(Convert.ToInt32(hdnStoreProductid.Value), Convert.ToInt32(((HiddenField)e.Item.FindControl("hdnMasterItemNo")).Value));

                        if (objImageList.Count > 0)
                        {
                            ((HtmlImage)e.Item.FindControl("imgSplashImage")).Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value + "&_twidth=145&_theight=198";
                            ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/" + ((HiddenField)e.Item.FindControl("hdnlargerimagename")).Value;
                            StoreProductIdForBulkOrder = Convert.ToInt32(hdnStoreProductid.Value);
                            GapBentweenBulkOrder = 1;
                        }
                    }
                    else //New Add nagmani
                    {
                        if (GapBentweenBulkOrder == 1)
                        {
                            GapBentweenBulkOrder = 0;
                            ((HtmlTable)e.Item.FindControl("tblCartItem")).Style.Add("margin-top", "-160px");
                        }
                        else
                            ((HtmlTable)e.Item.FindControl("tblCartItem")).Style.Add("margin-top", "-30px");
                        ((HtmlControl)e.Item.FindControl("prettyphotoDiv")).Visible = false;
                        ((HtmlGenericControl)e.Item.FindControl("dvCartItemHeader")).Visible = false;
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

    protected void lnkBtnCheckout_Click(object sender, EventArgs e)
    {
        if (CheckIfQuantityEnteredIsNULL())
        {
            lblrptMsg.Visible = true;
            lblrptMsg.Text = "You can not checkout without entering quantity,please enter quantity!";
        }
        else if (CheckIfQuantityEnteredIsZero())
        {
            lblrptMsg.Visible = true;
            lblrptMsg.Text = "You can not checkout with quantity zero,please upadate quantity!";
        }
        else
        {
            if (ApplyChanges())
            {
                if (!String.IsNullOrEmpty(CoupaID))//Check for Coupa Order
                {
                    UpdateCoupaPunchoOutDetailsbyID(Convert.ToInt64(CoupaID));
                    Response.Redirect("CoupaCheckOut.aspx");
                }
                else if (string.IsNullOrEmpty(StrMessage))
                    Response.Redirect("~/My Cart/CheckOutSteps.aspx", false);
            }

        }
    }

    protected void lnkBtnContinueShopping_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/index.aspx");
        setContinueShoppingLink();
    }

    private void setContinueShoppingLink()
    {
        //if (this.SubCatID != null && this.SubCatID != 0)
        //    Response.Redirect("~/Products/ProductList.aspx?SubCat=" + this.SubCatID + "&sc=true");
        if (Session["ProductListUrl"] != null)
            Response.Redirect(Session["ProductListUrl"].ToString());
        else
            Response.Redirect("~/Index.aspx");
    }

    protected void lnkBtnApplyChanges_Click(object sender, EventArgs e)
    {
        ApplyChanges();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Loads products and binds to repeater
    /// </summary>
    protected void BindData()
    {
        try
        {
            List<SelectMyShoppingCartProductResult> obj = objShoppingCartRepository.SelectShoppingProduct(IncentexGlobal.CurrentMember.UserInfoID);
            // If Coupa User
            if (!String.IsNullOrEmpty(CoupaID) && !String.IsNullOrEmpty(BuyerCookie))
                obj = obj.Where(q => q.BuyerCookie == this.BuyerCookie && q.IsCoupaOrder == true && q.IsCoupaOrderSubmitted == false).ToList();

            if (obj.Count > 0)
            {
                Session["MasterNo"] = obj[0].MasterItemNo;
                lblrptMsg.Text = "";
                lblrptMsg.Visible = false;
                lnkBtnCheckout.Visible = true;
                lnkBtnApplyChanges.Visible = true;
                rptMyShoppingCart.Visible = true;
                rptMyShoppingCart.DataSource = obj;
                rptMyShoppingCart.DataBind();
            }
            else
            {
                lblrptMsg.Visible = true;
                lblrptMsg.Text = "No Records Found";
                lnkBtnCheckout.Visible = false;
                lnkBtnApplyChanges.Visible = false;
                rptMyShoppingCart.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            ex.Message.ToString();
        }
    }

    /// <summary>
    /// Calculates Changes in amount based on quantity
    /// </summary>
    protected void Calculate()
    {
        try
        {
            decimal totalRunning = 0;
            decimal totalNotEligibleAnniversary = 0;
            foreach (RepeaterItem item in rptMyShoppingCart.Items)
            {
                //Added on 8-Mar-11
                Label lblRunCharge = (Label)item.FindControl("lblRunCharge");
                //End
                Label lblUnitPrice = (Label)item.FindControl("lblUnitPrice");
                TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
                Label lblTotal = (Label)item.FindControl("lblTotal");
                Label lblTotalPrice = (Label)rptMyShoppingCart.Controls[rptMyShoppingCart.Controls.Count - 1].Controls[0].FindControl("lblTotalProductPrice");
                HiddenField lblsLookupName = (HiddenField)item.FindControl("hdnAnniversary");
                //Add Nagmani 18-April-2012 
                string strtotal;
                //End
                if (lblsLookupName.Value == "No")
                {
                    if (txtQuantity.Text != "" || lblUnitPrice.Text != "")
                    {
                        if (!String.IsNullOrEmpty(lblRunCharge.Text))
                        {
                            //Update nagmani 18-04-12
                            //lblTotal.Text = ((Convert.ToDecimal(lblUnitPrice.Text) + Convert.ToDecimal(lblRunCharge.Text)) * Convert.ToDecimal(txtQuantity.Text)).ToString();
                            lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(txtQuantity.Text)).ToString();
                            strtotal = ((Convert.ToDecimal(lblUnitPrice.Text) + Convert.ToDecimal(lblRunCharge.Text)) * Convert.ToDecimal(txtQuantity.Text)).ToString();
                        }
                        else
                        {
                            lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(txtQuantity.Text)).ToString();
                            //Add nagmani 18-04-12
                            strtotal = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(txtQuantity.Text)).ToString();
                        }
                        //Update nagmani 18-04-12
                        //totalNotEligibleAnniversary += Convert.ToDecimal(lblTotal.Text);
                        totalNotEligibleAnniversary += Convert.ToDecimal(strtotal);
                    }

                }
                else
                {
                    if (txtQuantity.Text != "" || lblUnitPrice.Text != "")
                    {
                        if (!String.IsNullOrEmpty(lblRunCharge.Text))
                        {
                            //Update nagmani 18-04-12
                            //lblTotal.Text = ((Convert.ToDecimal(lblUnitPrice.Text) + Convert.ToDecimal(lblRunCharge.Text)) * Convert.ToDecimal(txtQuantity.Text)).ToString();
                            lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(txtQuantity.Text)).ToString();
                            strtotal = ((Convert.ToDecimal(lblUnitPrice.Text) + Convert.ToDecimal(lblRunCharge.Text)) * Convert.ToDecimal(txtQuantity.Text)).ToString();
                        }
                        else
                        {
                            //Add nagmani 18-04-12
                            lblTotal.Text = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(txtQuantity.Text)).ToString();
                            strtotal = (Convert.ToDecimal(lblUnitPrice.Text) * Convert.ToDecimal(txtQuantity.Text)).ToString();
                        }
                        // totalRunning += Convert.ToDecimal(lblTotal.Text);
                        totalRunning += Convert.ToDecimal(strtotal);
                    }

                }

                lblTotalPrice.Text = (totalNotEligibleAnniversary + totalRunning).ToString("#,##0.00");
                Session["TotalAmount"] = (totalNotEligibleAnniversary + totalRunning);
                Session["NotAnniversaryCreditAmount"] = totalNotEligibleAnniversary;
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    /// <summary>
    /// Applies the changes if any
    /// Deletes record if any product selected for removal from cart
    /// Updates value if quantity modified
    /// </summary>
    protected bool ApplyChanges()
    {
        try
        {
            String strInventoryMessage = String.Empty;
            Int16 Count = 0;
            ProductItemDetailsRepository objproductitem = new ProductItemDetailsRepository();
            foreach (RepeaterItem item in rptMyShoppingCart.Items)
            {
                TextBox txtQty = (TextBox)item.FindControl("txtQuantity");
                TextBox txtLength = (TextBox)item.FindControl("Textbox1");
                Label lblID = (Label)item.FindControl("lblID");

                HiddenField hdnStoreProductID = (HiddenField)item.FindControl("hdnStoreProductID");
                HiddenField hdnItemNumber = (HiddenField)item.FindControl("hdnItemNumber");
                HiddenField hdnInventory = (HiddenField)item.FindControl("hdnInventory");
                HiddenField hdnSize = (HiddenField)item.FindControl("hdnSize");


                //Check back order AllowBackOrderID
                long AllowBackOrderID = objproductitem.GetProductBackOrderId(Convert.ToInt64(hdnStoreProductID.Value));
                long? ItemAllowBackOrderID = objproductitem.GetProductItemBackOrderId(Convert.ToInt64(hdnStoreProductID.Value), hdnItemNumber.Value);
                ItemAllowBackOrderID = ItemAllowBackOrderID.HasValue ? ItemAllowBackOrderID.Value : 0;

                if (objproductitem.CheckAllowBackOrderID(AllowBackOrderID, ItemAllowBackOrderID.Value, Convert.ToInt32(hdnInventory.Value), Convert.ToInt32(txtQty.Text)))
                {
                    objShoppingCart = objShoppingCartRepository.GetById(int.Parse(lblID.Text), IncentexGlobal.CurrentMember.UserInfoID);
                    objShoppingCart.Quantity = txtQty.Text;
                    objShoppingCart.TailoringLength = txtLength.Text;
                    objShoppingCartRepository.SubmitChanges();
                }
                else
                {
                    if (string.IsNullOrEmpty(strInventoryMessage))
                        strInventoryMessage += " Please enter Order Quantity less than or equal to Inventory left for below :- ";
                    //strInventoryMessage += " \\n========================================";
                    strInventoryMessage += " \\n " + (Count + 1) + ") Item Number : " + hdnItemNumber.Value;
                    strInventoryMessage += " ; item size : " + hdnSize.Value;
                    Count++;
                }
            }

            //Checks for the selected records and removes then from cart
            foreach (RepeaterItem item in rptMyShoppingCart.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("chkRemove");
                Label lb = (Label)item.FindControl("lblID");

                if (cb != null)
                {
                    if (cb.Checked)
                    {
                        int result = objShoppingCartRepository.RemoveProductsFromCart(Convert.ToInt32(lb.Text), IncentexGlobal.CurrentMember.UserInfoID);
                        objShoppingCartRepository.SubmitChanges();
                    }
                }
            }

            if (string.IsNullOrEmpty(strInventoryMessage))
            {
                GapBentweenBulkOrder = 0;
                StoreProductIdForBulkOrder = 0;
                BindData();
                Calculate();
                return true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myalert", "alert('" + strInventoryMessage + "');", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            ex.Message.ToString();
            return false;
        }
    }

    private bool CheckIfQuantityEnteredIsZero()
    {
        bool isZero = false;
        foreach (RepeaterItem item in rptMyShoppingCart.Items)
        {
            TextBox cb = (TextBox)item.FindControl("txtQuantity");
            if (cb.Text == "0")
            {
                isZero = true;
            }
        }
        if (isZero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckIfQuantityEnteredIsNULL()
    {
        bool isZero = false;
        foreach (RepeaterItem item in rptMyShoppingCart.Items)
        {
            TextBox cb = (TextBox)item.FindControl("txtQuantity");
            if (cb.Text == string.Empty)
            {
                isZero = true;
            }
        }
        if (isZero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DeleteBulkOrder(string StoreProductID, Boolean IsBulkOrder)
    {
        foreach (RepeaterItem item in rptMyShoppingCart.Items)
        {
            if (((HiddenField)item.FindControl("hdnStoreProductID")).Value == StoreProductID && ((HiddenField)item.FindControl("hdnIsBulkOrder")).Value == "True")
            {
                Label lblID = (Label)item.FindControl("lblID");
                int result = objShoppingCartRepository.RemoveProductsFromCart(Convert.ToInt32(lblID.Text), IncentexGlobal.CurrentMember.UserInfoID);
                objShoppingCartRepository.SubmitChanges();
            }
        }
    }


    /// <summary>
    /// This method is used to update Group name of Uniform Issuance Policy
    /// </summary>
    /// <param name="UniformIssuancePolicyID"></param>
    /// <param name="groupName"></param>
    private void UpdateCoupaPunchoOutDetailsbyID(Int64 punchoutID)
    {
        CoupaPunchOutDetail obj = new CoupaPunchOutDetail();
        obj = objUserRepo.GetCoupaPunchOutDetailbyID(punchoutID);
        if (obj != null)
        {
            string xml = getStringXML(obj.PayLoadID, obj.BuyerCookie, obj.UserEmail);
            obj.CoupaPunchOutDetails = Convert.ToString(xml);
            objUserRepo.SubmitChanges();
        }
    }
    private String getStringXML(String payloadID, String buyerCookie, String userEmail)
    {
        List<SelectMyShoppingCartProductForCoupaResult> objList = new List<SelectMyShoppingCartProductForCoupaResult>();
        String sb = string.Empty;
        try
        {
            sb = @"<cXML payloadID='" + DateTime.Now.Ticks + "'  timestamp='" + DateTime.Now.ToString() + "' xml:lang='en-US' version='1.2.0.14'>" +
                 @"<Header>
                   <From>
                        <Credential domain='NetworkID'>
                            <Identity>World Link</Identity>
                        </Credential>
                   </From>
                   <To>
                      <Credential domain='NetworkId'>
                        <Identity>" + userEmail + @"</Identity>
                      </Credential>
                   </To>
                  <Sender>
                    <Credential domain='NetworkID'>
                      <Identity/>
                    </Credential>
                    <UserAgent/>
                  </Sender>
                </Header>
                <Message>
                <PunchOutOrderMessage>
                  <BuyerCookie>" + buyerCookie + @"</BuyerCookie>
                  <PunchOutOrderMessageHeader operationAllowed='edit'>
                    <Total>
                      <Money currency='USD'>" + strtotal + @"</Money>
                    </Total>
                   <Shipping>
                     <Money currency='USD'>0</Money>
                     <Description xml:lang='en-US'>Unknown</Description>
                   </Shipping>
                   <Tax>
                    <Money currency='USD'>0</Money>
                    <Description xml:lang='en-US'>Unknown</Description>
                   </Tax>
                  </PunchOutOrderMessageHeader>";
           
            objList = objShoppingCartRepository.SelectMyShoppingCartProductForCoupa(IncentexGlobal.CurrentMember.UserInfoID, buyerCookie);
            for (int i = 0; i < objList.Count; i++)
            {
                
                sb += @"<ItemIn quantity='" + objList[i].Quantity + "'>";
                sb += @"<ItemID>";
                sb += "<SupplierPartID>" + objList[i].MyShoppingCartID + "</SupplierPartID>";
                sb += "<SupplierPartAuxiliaryID>" + objList[i].StoreID + "</SupplierPartAuxiliaryID>";
                sb += @"</ItemID>";
                sb += @"<ItemDetail>";
                sb += @"<UnitPrice>";
                sb += @"<Money currency='USD'>" + objList[i].UnitPrice + "</Money>";
                sb += @"</UnitPrice>";
                sb += @"<Description xml:lang='en-US'> " + objList[i].ProductDescrption + "</Description>";
                sb += @"<UnitOfMeasure>EA</UnitOfMeasure>";
                sb += @"<Classification domain='UNSPSC'>unknown</Classification>";
                sb += @"<ManufacturerName/>";
                sb += @"<LeadTime>0</LeadTime>";
                sb += @"</ItemDetail></ItemIn>";
                // Update for order submitted to Coupa
                objShoppingCartRepository.UpdateMyShoppingCartForCoupaOrder(objList[i].MyShoppingCartID);

            }
            sb += @"</PunchOutOrderMessage></Message></cXML>";
            return sb;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            return sb;
        }


    }
    #endregion
}
