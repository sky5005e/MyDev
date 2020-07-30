using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.IO;

public partial class Products_ProductDetail : PageBase
{
    #region Properties

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

    Int32 MyShoppingCartID
    {
        get
        {
            if (ViewState["MyShoppingCartID"] == null)
            {
                ViewState["MyShoppingCartID"] = 0;
            }
            return Convert.ToInt32(ViewState["MyShoppingCartID"]);
        }
        set
        {
            ViewState["MyShoppingCartID"] = value;
        }
    }

    Int64? StoreProductID
    {
        get
        {
            if (ViewState["StoreProductID"] == null)
            {
                ViewState["StoreProductID"] = 0;
            }
            return Convert.ToInt32(ViewState["StoreProductID"]);
        }
        set
        {
            ViewState["StoreProductID"] = value;
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

    String Runcharge
    {
        get
        {
            if (ViewState["Runcharge"] == null)
            {
                ViewState["Runcharge"] = "";
            }
            return ViewState["Runcharge"].ToString();
        }
        set
        {
            ViewState["Runcharge"] = value;
        }

    }

    /// <summary>
    /// Set True when page is First Time Load
    /// </summary>
    Boolean IsFirstTimePageLoad
    {
        get
        {
            if (ViewState["IsFirstTimePageLoad"] == null)
            {
                ViewState["IsFirstTimePageLoad"] = false;
            }
            return (Boolean)ViewState["IsFirstTimePageLoad"];
        }
        set
        {
            ViewState["IsFirstTimePageLoad"] = value;
        }

    }

    /// <summary>
    /// Set True when there is more color
    /// </summary>
    Boolean IsColorCountMore
    {
        get
        {
            if (ViewState["IsColorCountMore"] == null)
            {
                ViewState["IsColorCountMore"] = false;
            }
            return (Boolean)ViewState["IsColorCountMore"];
        }
        set
        {
            ViewState["IsColorCountMore"] = value;
        }

    }
    /// <summary>
    /// Here we will set Session CoupaID 
    /// </summary>
    String CoupaID
    {
        get { return Convert.ToString(Session["CoupaID"]); }
    }
    /// <summary>
    /// Set True when Product is CloseOut Product
    /// </summary>
    Boolean IsCloseOutProduct
    {
        get
        {
            if (ViewState["IsCloseOutProduct"] == null)
            {
                ViewState["IsCloseOutProduct"] = false;
            }
            return (Boolean)ViewState["IsCloseOutProduct"];
        }
        set
        {
            ViewState["IsCloseOutProduct"] = value;
        }
    }
    /// <summary>
    /// To set Enter buyer Cookie
    /// </summary>
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
    /// <summary>
    /// Set LargeImage path
    /// </summary>
    String LargeImagePath
    {
        get
        {
            if (ViewState["LargeImagePath"] == null)
            {
                ViewState["LargeImagePath"] = "";
            }
            return ViewState["LargeImagePath"].ToString();
        }
        set
        {
            ViewState["LargeImagePath"] = value;
        }

    }

    /// <summary>
    /// Master lavel AllowBackOrderID
    /// </summary>
    Int64 AllowBackOrderID
    {
        get
        {
            if (ViewState["AllowBackOrderID"] == null)
            {
                ViewState["AllowBackOrderID"] = 0;
            }
            return Convert.ToInt64(ViewState["AllowBackOrderID"]);
        }
        set
        {
            ViewState["AllowBackOrderID"] = value;
        }
    }

    /// <summary>
    /// Item lavel AllowBackOrderID
    /// </summary>
    Int64? ItemAllowBackOrderID
    {
        get
        {
            if (ViewState["ItemAllowBackOrderID"] == null)
            {
                ViewState["ItemAllowBackOrderID"] = 0;
            }
            return Convert.ToInt64(ViewState["ItemAllowBackOrderID"]);
        }
        set
        {
            ViewState["ItemAllowBackOrderID"] = value;
        }
    }

    Int32 Inventory
    {
        get
        {
            if (ViewState["Inventory"] == null)
            {
                ViewState["Inventory"] = 0;
            }
            return Convert.ToInt32(ViewState["Inventory"]);
        }
        set
        {
            ViewState["Inventory"] = value;
        }
    }

    String FName = String.Empty;
    String LName = String.Empty;

    StoreProductRepository objStoreProductRepository = new StoreProductRepository();
    StoreProductImageRepository objStoreProductImageRepository = new StoreProductImageRepository();
    MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
    MyShoppinCart objShoppingCart = new MyShoppinCart();
    Int32 intDuplicate = 0;
    INC_Lookup objLook = new INC_Lookup();
    LookupRepository objLookupRepo = new LookupRepository();
    SubCatogeryRepository objSubCatogeryRepository = new SubCatogeryRepository();
    ProductItemDetailsRepository objproductitem = new ProductItemDetailsRepository();

    #endregion

    #region Page Load Event

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Product Detail";
            ((HtmlImage)Master.FindControl("imgShoppingCart")).Visible = true;

            this.StoreProductID = Convert.ToInt64(Request.QueryString.Get("StoreProductId"));
            this.MasterItemNo = Convert.ToInt64(Request.QueryString.Get("MasterItemNo"));
            this.SubCatID = Convert.ToInt64(Request.QueryString["SubCat"]);
            INC_Lookup objlook = new INC_Lookup();
            LookupRepository objlookuprepos = new LookupRepository();

            if (Session["ProductListUrl"] != null)
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = Session["ProductListUrl"].ToString();
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";

            trL3.Visible = false;
            dvL3Msg.Visible = false;
            // Set Buyer Cookie  For Coupa Order
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
            // Set Color
            BindDropdownColor();
            // Set Page as per Size on DDL.
            BindDropdownSize(null);
            // 
            IsFirstTimePageLoad = true;
            ddlColor_SelectedIndexChanged(sender, e);
            // 
            if (!IsColorCountMore)
                ddlSize_SelectedIndexChanged(sender, e);
            else
            {
                // when Size is changed
                if (ddlSize.SelectedValue != "" && ddlColor.SelectedValue != "")
                {
                    List<SP_GetStoreProductBySubCatIDResult> objListsubId = GetProductsBySubCatID();
                    List<SP_GetStoreProductBySubCatIDResult> obj = (from p in objListsubId
                                                                    where p.ItemSizeID == Convert.ToInt64(ddlSize.SelectedValue)
                                                                    && p.ItemColorID == Convert.ToInt64(ddlColor.SelectedValue)
                                                                    select p).ToList();


                    //Set AllowBackOrderID
                    if (obj.Count > 0)
                    {
                        this.AllowBackOrderID = obj[0].AllowBackOrderID.Value;
                        this.ItemAllowBackOrderID = obj[0].ItemAllowBackOrderID;
                        this.Inventory = obj[0].Inventory.HasValue ? obj[0].Inventory.Value : 0;
                    }
                }
            }

            bindAllProductImages();

            //Fucntion To hide Price row for 
            SetUserStoreOptions();

            SetBulkOrderAndNameToEngraveFeature();
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Sets the bulk order feature for company admin based on access.
    /// </summary>
    protected void SetBulkOrderAndNameToEngraveFeature()
    {
        String userStoreOptions = new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).Userstoreoption;
        if (!String.IsNullOrEmpty(userStoreOptions))
        {
            String[] userstore = userStoreOptions.Split(',');
            foreach (String eachuserStore in userstore)
            {
                if (new LookupRepository().GetById(Convert.ToInt32(eachuserStore)).sLookupName.Trim() == "Bulk Order")
                {
                    lnkbtnBulkOrder.Visible = true;
                }
                else if (new LookupRepository().GetById(Convert.ToInt32(eachuserStore)).sLookupName.Trim() == "Name to Engrave")
                {
                    txtExample.Enabled = true;
                }
            }
        }
    }

    //Functio to Check if user is allowed to see the price or not 
    //Created on 2-Mar-11 by Ankit..
    public void SetUserStoreOptions()
    {
        Boolean DoNotShowPriceToUser = false;
        String userStoreOptions = new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).Userstoreoption;
        if (!String.IsNullOrEmpty(userStoreOptions))
        {
            if (userStoreOptions.Contains(','))
            {
                String[] userstore = userStoreOptions.Split(',');
                foreach (String eachuserStore in userstore)
                {
                    if (new LookupRepository().GetById(Convert.ToInt32(eachuserStore)).sLookupName.Trim() == "Do Not View Any Prices")
                    {
                        DoNotShowPriceToUser = true;
                        break;
                    }
                    else
                    {
                        DoNotShowPriceToUser = false;
                    }
                }
            }
            else
            {
                if (new LookupRepository().GetById(Convert.ToInt32(userStoreOptions)).sLookupName.Trim() == "Do Not View Any Prices")
                {
                    DoNotShowPriceToUser = true;
                }
                else
                {
                    DoNotShowPriceToUser = false;
                }
            }
        }

        if (DoNotShowPriceToUser)
        {
            lnkAddToCart.Visible = false;
            trL3Price.Visible = false;
            trL3.Visible = false;
        }
    }

    /// <summary>
    ///  To set DDL Size
    /// </summary>
    private void BindDropdownSize(List<SP_GetStoreProductBySubCatIDResult> objList)
    {
        if (this.SubCatID != null && this.SubCatID != 0)
        {
            if (objList != null && objList.Count > 0)
            {
                var SizeList = (from s in objList
                                where s.ItemColorID == Convert.ToInt64(ddlColor.SelectedValue)
                                select new { s.ItemSizeID, s.ItemSize, s.SizePriority }).Distinct().OrderBy(o => o.SizePriority).ToList();
                Common.BindDDL(ddlSize, SizeList, "ItemSize", "ItemSizeID", "");
                if (SizeList.Count == 1)
                    ddlSize.Enabled = false;
                else
                    ddlSize.Enabled = true;
            }
            else
            {
                List<SP_GetStoreProductBySubCatIDResult> objListsubId = GetProductsBySubCatID();
                var SizeList = (from s in objListsubId
                                where s.ItemColorID == Convert.ToInt64(ddlColor.SelectedValue)
                                select new { s.ItemSizeID, s.ItemSize, s.SizePriority }).Distinct().OrderBy(o => o.SizePriority).ToList();
                Common.BindDDL(ddlSize, SizeList, "ItemSize", "ItemSizeID", "");
            }
        }

    }
    /// <summary>
    /// Bind Color
    /// </summary>
    public void BindDropdownColor()
    {
        if (this.SubCatID != null && this.SubCatID != 0)
        {
            List<SP_GetStoreProductBySubCatIDResult> objListsubId = GetProductsBySubCatID();
            var ColorList = (from c in objListsubId
                             select new { c.ItemColorID, c.ItemColorName, c.ItemColorImage }).Distinct().ToList();
            Common.BindDDL(ddlColor, ColorList, "ItemColorName", "ItemColorID", "");
            if (ColorList.Count == 1)
                ddlColor.Enabled = false;
            else
                ddlColor.Enabled = true;
        }
    }

    /// <summary>
    /// Bind Material DDL as per change selection of DDL Size or DDL Orderby
    /// </summary>
    /// <param name="objListsubId"></param>
    private void BindMaterial(List<SP_GetStoreProductBySubCatIDResult> objList)
    {
        if (objList.Count > 0 && (Boolean)objList[0].MaterialStatus)
        {
            var materialList = (from ms in objList
                                select new { ms.MaterialStyleID, ms.MaterialStyle }).Distinct().ToList();
            foreach (var items in materialList)
            {
                if (!String.IsNullOrEmpty(items.MaterialStyleID.ToString()))
                {
                    Common.BindDDL(ddlMaterialStyle, materialList, "MaterialStyle", "MaterialStyleID", "");
                    if (materialList.Count == 1)
                        ddlMaterialStyle.Enabled = false;
                    else
                        ddlMaterialStyle.Enabled = true;
                }
                else
                {
                    ddlMaterialStyle.ClearSelection();
                }
            }
        }
    }

    ///<summary>
    /// Bind Orderby DDL as per change selection of DDL Size or DDL Orderby
    /// </summary>
    /// <param name="objListsubId"></param>
    private void BindOrderby(List<SP_GetStoreProductBySubCatIDResult> objList)
    {
        var orderList = (from o in objList
                         select new { o.Soldby, o.SoldbyName }).Distinct().ToList();
        foreach (var items in orderList)
        {
            if (!String.IsNullOrEmpty(items.Soldby.ToString()))
            {
                Common.BindDDL(ddlOrderby, orderList, "SoldbyName", "Soldby", "");
                if (orderList.Count == 1)
                    ddlOrderby.Enabled = false;
                else
                    ddlOrderby.Enabled = true;
            }
            else
            {
                ddlOrderby.ClearSelection();
            }
        }
    }

    #region Get Products List using emp workgroup and subcategory

    List<SP_GetStoreProductBySubCatIDResult> GetProductsBySubCatID()
    {
        List<SP_GetStoreProductBySubCatIDResult> objList = new List<SP_GetStoreProductBySubCatIDResult>();
        CompanyEmployee objCmpnyInfo = new CompanyEmployee();
        CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
        objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        var qry = objStoreProductRepository.GetStoreProductBySubCatID((Int64)IncentexGlobal.CurrentMember.CompanyId, this.SubCatID);
        qry = qry.Where(a => a.StoreProductID == this.StoreProductID).ToList();
        objList = qry;
        return objList;
    }

    List<SP_GetStoreProductForBulkOrderResult> GetProductsBulkOrder()
    {
        List<SP_GetStoreProductForBulkOrderResult> objList = new List<SP_GetStoreProductForBulkOrderResult>();
        StoreProductRepository objstprodrepos = new StoreProductRepository();
        StoreProduct objStore = new StoreProduct();
        objStore = objstprodrepos.GetById(Convert.ToInt64(this.StoreProductID));

        if (objStore != null)
        {
            Int64 productworkgroupid = (objStore.WorkgroupID);

            var qry = objStoreProductRepository.GetStoreProductItemsForBulkOrder((Int64)IncentexGlobal.CurrentMember.CompanyId, productworkgroupid);
            qry = qry.Where(a => a.StoreProductID == this.StoreProductID).ToList();
            objList = qry;
        }
        return objList;
    }

    #endregion

    #region Function to get all the product images
    public void bindAllProductImages()
    {
        List<StoreProductImage> objStoreProductImage = new List<StoreProductImage>();
        StoreProductImageRepository objRepos = new StoreProductImageRepository();
        if (!String.IsNullOrEmpty(hfMasterItemNumber.Value))
        {
            objStoreProductImage = objRepos.GetStoreProductImagesdByMasterID(Convert.ToInt64(hfMasterItemNumber.Value), Convert.ToInt64(hfProductItemId.Value));
            objStoreProductImage = objStoreProductImage.OrderByDescending(a => a.ProductImageActive).ToList();
            dtProductImages.DataSource = objStoreProductImage;
            dtProductImages.DataBind();
        }
    }
    /// <summary>
    /// For Sub items Images
    /// </summary>
    /// <param name="objList"></param>
    private void BindSubItemImages(List<SP_GetStoreProductBySubCatIDResult> objList)
    {
        if (objList.Count > 0 && objList[0].ItemImage != null && objList[0].ItemImage.Length > 0)
        {
            dtSubitemsImages.DataSource = objList.Take(1);// show First image
            dtSubitemsImages.DataBind();
            dtSubitemsImages.Visible = true;
            dtProductImages.Visible = false;
        }
        else
        {
            bindAllProductImages();// When there is no subitem images
            dtSubitemsImages.Visible = false;
            dtProductImages.Visible = true;
        }
    }
    #endregion

    #region If No tailoring or measurement is uploaded from the admin side then set Both PDF links to true or false.
    public void SetTailoringMeasurementCount(Int64 MasterItemNo, Int64 StoreProductID)
    {
        TailoringMeasurementsRepository objRep = new TailoringMeasurementsRepository();
        List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
        objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(MasterItemNo, StoreProductID);
        if (objCount.Count > 0)
        {
            if (objCount[0].TailoringMeasurementChart != "")
            {
                lnkMeasurementChart.Visible = true;
                lnkMeasurementChart.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringMeasurementChart;
            }
            else
            {
                lnkMeasurementChart.Visible = false;
                lnkMeasurementChart.NavigateUrl = "";
            }

        }
        else
        {
            lnkMeasurementChart.Visible = false;
            lnkMeasurementChart.NavigateUrl = "";
        }
    }
    public void SetTailoringGuidelineCount(Int64 MasterItemNo, Int64 StoreProductID)
    {
        TailoringMeasurementsRepository objRep = new TailoringMeasurementsRepository();
        List<ProductItemTailoringMeasurement> objCount = new List<ProductItemTailoringMeasurement>();
        objCount = objRep.GetTailoringMeasurementByStoreProductIDAndMasterItemID(MasterItemNo, StoreProductID);
        if (objCount.Count > 0)
        {
            if (objCount[0].TailoringGuidelines != "")
            {
                lnkTailoringGuidelines.Visible = true;
                lnkTailoringGuidelines.NavigateUrl = "../UploadedImages/TailoringMeasurement/" + objCount[0].TailoringGuidelines;
                //pnlTailoring.Visible = true;
            }
            else
            {
                lnkTailoringGuidelines.Visible = false;
                lnkTailoringGuidelines.NavigateUrl = "";
                //pnlTailoring.Visible = false;
            }

        }
        else
        {
            lnkTailoringGuidelines.Visible = false;
            lnkTailoringGuidelines.NavigateUrl = "";
            //pnlTailoring.Visible = false;
        }
    }
    #endregion

    private void SetNameFontFormat()
    {
        if (txtNameFormat.Text.ToString() != String.Empty)
        {
            if (txtNameFormat.Text == "First Name")
            {
                if (txtFontFormat.Text == "All Caps")
                {
                    txtExample.Text = this.FName.ToUpper();
                }
                else if (txtFontFormat.Text == "Upper and Lower Case")
                {
                    txtExample.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.FName.ToLower());
                }
                else
                {
                    txtExample.Text = this.FName;
                }
            }
            else if (txtNameFormat.Text == "Last Name")
            {
                if (txtFontFormat.Text == "All Caps")
                {
                    txtExample.Text = this.LName.ToUpper();
                }
                else if (txtFontFormat.Text == "Upper and Lower Case")
                {
                    txtExample.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.LName.ToLower());
                }
                else
                {
                    txtExample.Text = this.LName;
                }
            }
            else if (txtNameFormat.Text == "First Initial.LastName")
            {
                if (txtFontFormat.Text == "All Caps")
                {
                    txtExample.Text = (this.FName.Substring(0, 1).ToString() + "." + this.LName.ToString()).ToUpper();
                }
                else if (txtFontFormat.Text == "Upper and Lower Case")
                {
                    txtExample.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.FName.Substring(0, 1).ToString() + "." + LName.ToString().ToLower());
                }
                else
                {
                    txtExample.Text = (this.FName.Substring(0, 1).ToString() + "." + this.LName.ToString()).ToUpper();
                }
            }
        }
        else
        {
            if (txtFontFormat.Text == "All Caps")
            {
                txtExample.Text = (this.FName + " " + this.LName).ToUpper();
            }
            else if (txtFontFormat.Text == "Upper and Lower Case")
            {
                txtExample.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase((this.FName + " " + this.LName));
            }
            else
            {
                txtExample.Text = (this.FName + " " + this.LName);
            }
        }
    }

    private void AddNameBartoCart()
    {
        try
        {
            CompanyEmployee objCmpnyInfo = new CompanyEmployee();
            CompanyEmployeeRepository objCompEmp = new CompanyEmployeeRepository();
            objCmpnyInfo = objCompEmp.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
            txtOrderQty.Text = "1";
            //Insert into MyShoppingcart Table
            if (this.MyShoppingCartID != 0)
                objShoppingCart = objShoppingCartRepository.GetById(MyShoppingCartID, IncentexGlobal.CurrentMember.UserInfoID);
            objShoppingCart.FilePath = ExcelDoc.Value;
            objShoppingCart.CategoryID = Convert.ToInt64(ViewState["CategoryID"]);
            objShoppingCart.CompanyID = Convert.ToInt64(ViewState["CompanyID"]);
            objShoppingCart.ItemNumber = ViewState["ItemNumber"].ToString();
            objShoppingCart.ProductDescrption = lblDescription.Text;
            objShoppingCart.Quantity = txtOrderQty.Text;
            objShoppingCart.Size = ddlSize.SelectedItem.Text;
            objShoppingCart.StoreID = Convert.ToInt64(ViewState["StoreID"]);
            objShoppingCart.StoreProductID = Convert.ToInt64(ViewState["StoreProductID"]);
            objShoppingCart.SubCategoryID = Convert.ToInt64(ViewState["SubCategoryID"]);
            objShoppingCart.TailoringLength = txtDesiredLength.Text;
            objShoppingCart.UnitPrice = txtL3.Visible == true ? txtL3.Text : txtPrice.Text; // Change by Shehzad 11-Jan-2011
            objShoppingCart.MOASUnitPrice = txtL3.Visible == true ? txtL3.Text : txtPrice.Text; // Change by Prashant 29-Jun -2013
            objShoppingCart.WorkgroupID = Convert.ToInt64(ViewState["WorkgroupID"]);
            objShoppingCart.Inventory = Convert.ToString(ViewState["Inventory"]);
            objShoppingCart.ProductImageID = ViewState["StoreProductImageId"].ToString();
            objShoppingCart.MasterItemNo = Convert.ToInt64(ViewState["MasterItemNo"]);
            objShoppingCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
            // Add Created Date on MyShopping Cart Table
            objShoppingCart.CreatedDate = DateTime.Now;
            // Added SoldbyName field in MyShoppingCart Table.
            objShoppingCart.SoldbyName = ddlOrderby.SelectedItem.Text;
            // Added MaterialStyle field in MyShoppingCart Table.
            if (ddlMaterialStyle.SelectedIndex != -1)
            {
                objShoppingCart.MaterialStyle = ddlMaterialStyle.SelectedItem.Text;
            }
            else
            {
                objShoppingCart.MaterialStyle = null;
            }

            if (IsCloseOutProduct)
            {
                objShoppingCart.PriceLevel = 5;
                objShoppingCart.MOASPriceLevel = 5;
            }
            else if (txtL3.Visible == true)
            {
                objShoppingCart.PriceLevel = 3;
                objShoppingCart.MOASPriceLevel = 3;
            }
            else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
            {
                objShoppingCart.PriceLevel = 2;
                objShoppingCart.MOASPriceLevel = 2;
            }
            else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            {
                objShoppingCart.PriceLevel = 1;
                objShoppingCart.MOASPriceLevel = 1;
            }
            else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
            {
                objShoppingCart.PriceLevel = 4;
                objShoppingCart.MOASPriceLevel = 4;
            }

            if (this.Runcharge != null)
            {
                objShoppingCart.RunCharge = Convert.ToString(this.Runcharge);
            }


            objShoppingCart.NameToBeEngraved = null;

            //End

            //Shehzad Start 5-Jan-2011
            objShoppingCart.IsOrdered = false;
            //shehzad end

            if (Session["ShowInventoryLevel"].ToString() == "Yes")
            {
                objShoppingCart.ShowInventoryLevel = "Yes";
            }
            else
            {
                objShoppingCart.ShowInventoryLevel = "No";
            }

            intDuplicate = objShoppingCartRepository.CheckDuplicate(Convert.ToInt32(ViewState["StoreProductID"]), ddlSize.SelectedItem.Text, IncentexGlobal.CurrentMember.UserInfoID, ddlOrderby.SelectedItem.Text);

            if (intDuplicate == 0)
            {
                //Check Here Inventory level is less than the Enter order quantity and is Allowbackoer is Yes OR No.
                Int32 BackOrder = Convert.ToInt32(ViewState["AllowBAckOrder"]);
                Int32 Inventory = Convert.ToInt32(ViewState["Inventory"]);
                objLook = objLookupRepo.GetById(BackOrder);
                if (objLook != null)
                {
                    if (Inventory < Int32.Parse(txtOrderQty.Text) && objLook.sLookupName == "Yes")
                    {
                        if (this.MyShoppingCartID == 0)
                        {
                            objShoppingCartRepository.Insert(objShoppingCart);
                            objShoppingCartRepository.SubmitChanges();
                            this.MyShoppingCartID = Convert.ToInt32(objShoppingCart.MyShoppingCartID);
                        }
                        else
                        {
                            objShoppingCartRepository.SubmitChanges();
                            this.MyShoppingCartID = Convert.ToInt32(objShoppingCart.MyShoppingCartID);
                        }
                        lblMsg.Text = "";
                        Response.Redirect("~/My Cart/MyShoppinCart.aspx?SubCat=" + this.SubCatID + "&MasterItemNo=" + this.MasterItemNo + "&StoreProductId=" + this.StoreProductID);
                    }

                    else if (Inventory < Int32.Parse(txtOrderQty.Text) && objLook.sLookupName == "No")
                    {
                        String myStringVariable = String.Empty;
                        myStringVariable = "Please enter Order Quantity less than or equal to Inventory left: " + Session["ShowInventoryNo"];
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);

                        txtOrderQty.Text = "";
                        SetFocus(txtOrderQty);
                        return;
                    }
                    else //NA
                    {
                        if (this.MyShoppingCartID == 0)
                        {
                            objShoppingCartRepository.Insert(objShoppingCart);
                            objShoppingCartRepository.SubmitChanges();
                            this.MyShoppingCartID = Convert.ToInt32(objShoppingCart.MyShoppingCartID);
                        }
                        else
                        {
                            objShoppingCartRepository.SubmitChanges();
                            this.MyShoppingCartID = Convert.ToInt32(objShoppingCart.MyShoppingCartID);
                        }

                        lblMsg.Text = "";
                        Response.Redirect("~/My Cart/MyShoppinCart.aspx?SubCat=" + this.SubCatID + "&MasterItemNo=" + this.MasterItemNo + "&StoreProductId=" + this.StoreProductID);
                    }
                }
            }
            else
            {
                String myStringVariable = String.Empty;
                myStringVariable = "Item already exists in your cart";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            if (!ex.Message.Contains("Input String was not in a correct format"))
                ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Events

    /*Added By Ankit on 7th Jan
    Which was single image ealier i have replaced with an Datalist control
    in this control the image which is selected from admin side is displaying and then 
    after clicking on it other related image will display..*/
    protected void dtProductImages_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (((HiddenField)e.Item.FindControl("hdnimagestatus")).Value.ToString() == "1")
                {
                    HiddenField hdndocumentname = (HiddenField)e.Item.FindControl("hdndocumentname");
                    HiddenField hdnlargerimagename = (HiddenField)e.Item.FindControl("hdnlargerimagename");
                    if (hdndocumentname.Value != null && hdndocumentname.Value != "")
                    {
                        ((HtmlImage)e.Item.FindControl("imgSplashImage")).Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + hdndocumentname.Value + "&_twidth=145&_theight=198";
                    }
                    else
                    {
                        ((HtmlImage)e.Item.FindControl("imgSplashImage")).Src = "~/UploadedImages/ProductImages/ProductDefault.jpg";
                    }
                    if (hdnlargerimagename.Value != null && hdnlargerimagename.Value != "")
                    {
                        ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/" + hdnlargerimagename.Value;
                        // Here set Large Image path for Subitems Images
                        LargeImagePath = hdnlargerimagename.Value;
                    }
                    else
                    {
                        ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/ProductDefault.jpg";
                    }
                }
                else
                {
                    ((HtmlImage)e.Item.FindControl("imgSplashImage")).Attributes.Add("style", "border-style:none;");
                    ((HtmlImage)e.Item.FindControl("imgSplashImage")).Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value + "&_twidth=1&_theight=1";
                    ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/" + ((HiddenField)e.Item.FindControl("hdnlargerimagename")).Value;
                }

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dtSubitemsImages_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((HtmlImage)e.Item.FindControl("imgSubItemsImage")).Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + ((HiddenField)e.Item.FindControl("hdnSubItemImages")).Value + "&_twidth=145&_theight=198";
                if (!String.IsNullOrEmpty(LargeImagePath))
                    ((HtmlAnchor)e.Item.FindControl("SubprettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/" + LargeImagePath;
                else
                    ((HtmlAnchor)e.Item.FindControl("SubprettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/ProductDefault.jpg";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void ddlSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        trL3.Visible = false;
        dvL3Msg.Visible = false;
        trInventory.Visible = false;

        if (ddlSize.SelectedValue != "0" && ddlSize.SelectedValue != "")
        {
            #region search

            if (this.SubCatID != null && this.SubCatID != 0)
            {
                IsFirstTimePageLoad = false;
                #region Size is changed

                List<SP_GetStoreProductBySubCatIDResult> objListsubId = GetProductsBySubCatID();

                // when Size is changed
                if (ddlSize.SelectedValue != "" && ddlColor.SelectedValue != "")
                {
                    object QueryList = (from p in objListsubId
                                        where p.ItemSizeID == Convert.ToInt64(ddlSize.SelectedValue)
                                        && p.ItemColorID == Convert.ToInt64(ddlColor.SelectedValue)
                                        select p).ToList();
                    List<SP_GetStoreProductBySubCatIDResult> obj = QueryList as List<SP_GetStoreProductBySubCatIDResult>;
                    if (obj.Count > 0 && (Boolean)obj[0].MaterialStatus)
                    {
                        BindMaterial(obj);
                        ddlMaterialStyle_SelectedIndexChanged(sender, e);
                    }
                    else
                    {
                        BindOrderby(obj);
                    }

                    if (!IsFirstTimePageLoad)
                        BindSubItemImages(obj);


                    //Set AllowBackOrderID
                    if (obj.Count > 0)
                    {
                        this.AllowBackOrderID = obj[0].AllowBackOrderID.Value;
                        this.ItemAllowBackOrderID = obj[0].ItemAllowBackOrderID;
                        this.Inventory = obj[0].Inventory.HasValue ? obj[0].Inventory.Value : 0;
                    }
                }
                else
                {
                    BindOrderby(objListsubId);
                    BindMaterial(objListsubId);
                }

                #endregion

                GetProductDetailsBysubCatID();
            }

            #endregion
        }

        SetUserStoreOptions();
    }

    protected void ddlColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlColor.SelectedValue != "0" && ddlColor.SelectedValue != "")
        {
            #region search
            if (this.SubCatID != null && this.SubCatID != 0)
            {
                IsFirstTimePageLoad = false;
                #region Size is changed
                List<SP_GetStoreProductBySubCatIDResult> objListsubId = GetProductsBySubCatID();

                // when Size is changed
                if (ddlColor.SelectedValue != "")
                {
                    object QueryList = (from p in objListsubId
                                        where p.ItemColorID == Convert.ToInt64(ddlColor.SelectedValue)
                                        select p).ToList();
                    List<SP_GetStoreProductBySubCatIDResult> obj = QueryList as List<SP_GetStoreProductBySubCatIDResult>;
                    // 
                    if (obj.Count > 1)
                    {
                        IsColorCountMore = true;
                    }
                    else
                        IsColorCountMore = false;
                    //
                    if (obj.Count > 0 && (Boolean)obj[0].SizeOff)
                    {
                        BindDropdownSize(obj);
                        ddlSize_SelectedIndexChanged(sender, e);
                    }
                    else if (obj.Count > 0 && (Boolean)obj[0].MaterialStatus)
                    {
                        BindMaterial(obj);
                        ddlMaterialStyle_SelectedIndexChanged(sender, e);
                    }
                    else
                    {
                        BindOrderby(obj);
                    }

                    if (!IsFirstTimePageLoad)
                        BindSubItemImages(obj);
                }
                #endregion
                GetProductDetailsBysubCatID();

            }
            #endregion
        }
    }

    protected void ddlOrderby_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOrderby.SelectedValue != "0" && ddlOrderby.SelectedValue != "")
        {
            #region search

            if (this.SubCatID != null && this.SubCatID != 0)
            {
                GetProductDetailsBysubCatID();
            }

            #endregion
        }
    }

    protected void ddlMaterialStyle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMaterialStyle.SelectedValue != "0" && ddlMaterialStyle.SelectedValue != "")
        {
            #region search
            if (this.SubCatID != null && this.SubCatID != 0)
            {
                #region MaterialStyle is changed
                List<SP_GetStoreProductBySubCatIDResult> objListsubId = GetProductsBySubCatID();

                // when Material is changed
                if (ddlColor.SelectedValue != "" && ddlSize.SelectedValue != "" && ddlMaterialStyle.SelectedValue != "")
                {
                    object QueryList = (from p in objListsubId
                                        where p.ItemColorID == Convert.ToInt64(ddlColor.SelectedValue)
                                        && p.ItemSizeID == Convert.ToInt64(ddlSize.SelectedValue)
                                        && p.MaterialStyleID == Convert.ToInt64(ddlMaterialStyle.SelectedValue)
                                        select p).ToList();
                    List<SP_GetStoreProductBySubCatIDResult> obj = QueryList as List<SP_GetStoreProductBySubCatIDResult>;
                    if (obj.Count > 0)
                    {
                        BindOrderby(obj);
                        BindSubItemImages(obj);
                    }
                }
                else
                {
                    BindOrderby(objListsubId);

                }
                #endregion
                GetProductDetailsBysubCatID();
            }
            #endregion
        }
    }

    private void GetProductDetailsBysubCatID()
    {
        List<SP_GetStoreProductBySubCatIDResult> objList = GetProductsBySubCatID();
        object query;
        MarketingToolRepository objMarketingToolRepository = new MarketingToolRepository();
        Int64? PriceLevelID = null;
        // when size offered is not hide and 
        //both dropdownlist is selected value not equal to zero and also if there 
        //is only single vaule in ddlOrderby
        //All dropdownlist is selected value not equal to zero 
        // TTTT
        if (ddlColor.SelectedValue != "" && ddlSize.SelectedValue != "" && ddlMaterialStyle.SelectedValue != "" && ddlOrderby.SelectedValue != "")
        {
            query = (from p in objList
                     where p.ItemColorID == Convert.ToInt64(ddlColor.SelectedValue)
                         && p.ItemSizeID == Convert.ToInt64(ddlSize.SelectedValue)
                         && p.MaterialStyleID == Convert.ToInt64(ddlMaterialStyle.SelectedValue)
                         && p.Soldby == Convert.ToInt64(ddlOrderby.SelectedValue)
                     select p).Distinct().ToList();
        }//TTTF
        else if (ddlColor.SelectedValue != "" && ddlSize.SelectedValue != "" && ddlMaterialStyle.SelectedValue != "")
        {
            query = (from p in objList
                     where p.ItemColorID == Convert.ToInt64(ddlColor.SelectedValue)
                         && p.ItemSizeID == Convert.ToInt64(ddlSize.SelectedValue)
                         && p.MaterialStyleID == Convert.ToInt64(ddlMaterialStyle.SelectedValue)
                     select p).Distinct().ToList();
        }//TTFT
        else if (ddlColor.SelectedValue != "" && ddlSize.SelectedValue != "" && ddlMaterialStyle.SelectedValue == "" && ddlOrderby.SelectedValue != "")
        {
            query = (from p in objList
                     where p.ItemColorID == Convert.ToInt64(ddlColor.SelectedValue)
                         && p.Soldby == Convert.ToInt64(ddlOrderby.SelectedValue)
                         && p.ItemSizeID == Convert.ToInt64(ddlSize.SelectedValue)
                     select p).Distinct().ToList();
        }
        else// for firstime when page is load if there is more than single vaule in ddlOrderby
        {
            query = (from p in objList
                     where p.ItemColorID == Convert.ToInt64(ddlColor.SelectedValue)
                         && p.ItemSizeID == Convert.ToInt64(ddlSize.SelectedValue)
                     select p).Distinct().ToList();
        }
        List<SP_GetStoreProductBySubCatIDResult> obj = query as List<SP_GetStoreProductBySubCatIDResult>;

        if (obj.Count > 0)
        {
            hfMasterItemNumber.Value = obj[0].MasterItemNo.ToString();
            hfProductItemId.Value = obj[0].StoreProductID.ToString();

            lblItemNumber.Text = obj[0].MasterItemName;
            lblDescription.Text = obj[0].ProductDescrption;

            //Added By Ankit For Name Bars on 3 Feb 11
            if (obj[0].ProductStyle == "Name Bars")
            {
                //Set Name and Font Region
                trSizeOffered.Visible = false;
                this.FName = IncentexGlobal.CurrentMember.FirstName;
                this.LName = IncentexGlobal.CurrentMember.LastName;

                /*Employee Title added on 15 Feb*/
                CompanyEmployee objCompanyEmployee = new CompanyEmployee();
                CompanyEmployeeRepository objEmpRepo = new CompanyEmployeeRepository();
                LookupRepository objLookupRepoForTitle = new LookupRepository();
                objCompanyEmployee = objEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

                if (objCompanyEmployee.EmployeeTitleId != null)
                {
                    dvEmpTitle.Visible = true;
                    txtEmployeeTitle.Text = objLookupRepoForTitle.GetById((Int64)objCompanyEmployee.EmployeeTitleId).sLookupName.ToString();
                }
                else
                {
                    dvEmpTitle.Visible = false;
                }

                dvNameFormat.Visible = false;
                dvFontFormat.Visible = false;
                dvEnterName.Visible = true;
                dvUpload.Visible = true;
                if (obj[0].FontFormatForNameBars != "-select-")
                {
                    txtFontFormat.Text = obj[0].FontFormatForNameBars;
                }
                if (obj[0].NameFormatForNameBars != "-select-")
                {
                    txtNameFormat.Text = obj[0].NameFormatForNameBars;
                }

                SetNameFontFormat();
            }
            else
            {
                trSizeOffered.Visible = true;
                dvNameFormat.Visible = false;
                dvFontFormat.Visible = false;
                dvEmpTitle.Visible = false;
                dvEnterName.Visible = false;
                dvUpload.Visible = false;
            }
            //End

            #region Show/Hide Tailoring Guideline Option
            if (obj[0].TailoringStatusName != null && obj[0].TailoringStatusName.ToLower() == "active")
            {
                if (!String.IsNullOrEmpty(obj[0].RunCharge))
                {
                    this.Runcharge = obj[0].RunCharge.ToString();
                }
                SetTailoringGuidelineCount(this.MasterItemNo, obj[0].StoreProductID);
            }
            else
            {
                lnkTailoringGuidelines.Visible = false;
                lnkTailoringGuidelines.NavigateUrl = "";
            }
            #endregion

            #region Show/Hide Tailoring Measurement
            if (obj[0].TailoringMeasurementName != null && obj[0].TailoringMeasurementName.ToLower() == "active")
                SetTailoringMeasurementCount(this.MasterItemNo, obj[0].StoreProductID);
            else
            {
                lnkMeasurementChart.Visible = false;
                lnkMeasurementChart.NavigateUrl = "";
            }
            #endregion

            // Shehzad start 11-Jan-2011
            LookupRepository objLookupRepo = new LookupRepository();

            // Add by shehzad.
            // Show new logo if New Product Until is active and eligible
            if (obj[0].NewProductUntil != null)
            {
                if (obj[0].NewProductUntil >= DateTime.Now)
                    imgNew.Visible = true;
                else
                    imgNew.Visible = false;
            }
            else
                imgNew.Visible = false;

            

            // IF Product is Close out Product and Set True to IsCloseOutProduct 
            if ((Boolean)obj[0].IsCloseOut)
            {
                txtPrice.Text = obj[0].CloseOutPrice.ToString();
                IsCloseOutProduct = true;

                imgSale.Visible = false;
                txtL3.Visible = false;
                dvL3Msg.Visible = false;
                trL3Price.Visible = true;
            }
            else if (obj[0].Level3PricingStatus != null)
            {
                if (objLookupRepo.GetById((Int64)(obj[0].Level3PricingStatus)).sLookupName.ToLower() == "active" && obj[0].Level3PricingStatus != null)
                {
                    if (DateTime.Now.Date <= obj[0].Level3PricingEndDate && DateTime.Now.Date >= obj[0].Level3PricingStartDate)
                    {
                        imgSale.Visible = true;
                        trL3.Visible = true;
                        dvL3Msg.Visible = true;
                        txtL3.Visible = true;
                        txtL3.Text = obj[0].Level3.ToString();
                        lblDiscountExpDate.Text = Convert.ToDateTime(obj[0].Level3PricingEndDate).ToShortDateString();
                        trL3Price.Visible = false;
                    }
                    else
                    {
                        imgSale.Visible = false;
                        txtL3.Visible = false;
                        dvL3Msg.Visible = false;
                        trL3Price.Visible = true;
                    }
                }
                else
                {
                    imgSale.Visible = false;
                    txtL3.Visible = false;
                    dvL3Msg.Visible = false;
                    trL3Price.Visible = true;
                }
            }
            else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            {
                txtPrice.Text = obj[0].Level1.ToString();
                PriceLevelID = objMarketingToolRepository.GetPriceLevelID(Convert.ToString(Incentex.DAL.Common.DAEnums.PriceLevelName.L1));

                imgSale.Visible = false;
                txtL3.Visible = false;
                dvL3Msg.Visible = false;
                trL3Price.Visible = true;
            }
            else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
            {
                txtPrice.Text = obj[0].Level2.ToString();
                PriceLevelID = objMarketingToolRepository.GetPriceLevelID(Convert.ToString(Incentex.DAL.Common.DAEnums.PriceLevelName.L2));

                imgSale.Visible = false;
                txtL3.Visible = false;
                dvL3Msg.Visible = false;
                trL3Price.Visible = true;
            }
            else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
            {
                txtPrice.Text = obj[0].Level4.ToString();
                PriceLevelID = objMarketingToolRepository.GetPriceLevelID(Convert.ToString(Incentex.DAL.Common.DAEnums.PriceLevelName.L4));

                imgSale.Visible = false;
                txtL3.Visible = false;
                dvL3Msg.Visible = false;
                trL3Price.Visible = true;
            }

            if ((Boolean)obj[0].ColorOff)
                trColor.Visible = true;
            else
                trColor.Visible = false;

            if ((Boolean)obj[0].SizeOff)
                trSizeOffered.Visible = true;
            else
                trSizeOffered.Visible = false;
            // For Material Style 
            if ((Boolean)obj[0].MaterialStatus)
                trMaterialStyle.Visible = true;
            else
                trMaterialStyle.Visible = false;

            if (obj[0].ItemDescription != null)
                lblItemDescriptions.Text = obj[0].ItemDescription.ToString();
            else
                lblItemDescriptions.Visible = false;

            // Color 19-june-2012
            imgColor.Src = "../admin/Incentex_Used_Icons/" + objLookupRepo.GetById((Int64)obj[0].ItemColorID).sLookupIcon;

            // Shehzad End
            if (obj[0].Soldby != null)
                trOrderBy.Visible = true;
            else
                trOrderBy.Visible = false;

            //inventory
            String strDAteArrive = "1900-01-01";
            if (obj[0].ToArriveOn != Convert.ToDateTime(strDAteArrive))
            {
                trInventory.Visible = true;
                txtInventoryDate.Text = Convert.ToDateTime(obj[0].ToArriveOn).ToShortDateString();
            }
            else
            {
                trInventory.Visible = false;
            }

            if (!String.IsNullOrEmpty(obj[0].ShowInventoryLevelInStoreID.ToString()))
            {
                objLook = objLookupRepo.GetById((Int64)(obj[0].ShowInventoryLevelInStoreID));
                Session["ShowInventoryLevel"] = objLook.sLookupName;
                if (objLook.sLookupName == "Yes")
                {
                    trInventoryNo.Visible = true;
                    lblInventoryNo.Text = obj[0].Inventory.ToString();
                }
                else
                {
                    trInventoryNo.Visible = false;
                    lblInventoryNo.Text = String.Empty;
                    Session["ShowInventoryNo"] = obj[0].Inventory.ToString();
                }
            }
            else
            {
                trInventoryNo.Visible = false;
                lblInventoryNo.Text = String.Empty;
            }

            //Add Nagmani
            ViewState["ProductItemID"] = obj[0].ProductItemID;
            ViewState["SupplierID"] = obj[0].SupplierId;
            ViewState["ItemNumber"] = obj[0].ItemNumber;
            ViewState["Inventory"] = obj[0].Inventory;
            ViewState["MasterItemNo"] = obj[0].MasterItemNo;
            ViewState["StoreID"] = obj[0].StoreID;
            ViewState["StoreProductID"] = obj[0].StoreProductID;
            ViewState["SubCategoryID"] = obj[0].SubCategoryID;
            ViewState["CategoryID"] = obj[0].CategoryID;
            ViewState["WorkgroupID"] = obj[0].WorkgroupID;
            ViewState["CompanyID"] = obj[0].CompanyID;
            ViewState["AllowBAckOrder"] = obj[0].AllowBackOrderID;

            //End Nagmani
            // display image
            List<StoreProductImage> objImageList = objStoreProductImageRepository.GetStoreProductImagesdById((Int32)obj[0].StoreProductID, (Int32)obj[0].MasterItemNo);

            if (objImageList.Count > 0)
            {
                ViewState["StoreProductImageId"] = objImageList[0].StoreProductImageId;
            }

            //Saurabh --Discount

            Int64? DiscountOffer = objMarketingToolRepository.CheckDiscountValue(Convert.ToInt64(ViewState["StoreID"]), Convert.ToInt64(ViewState["WorkgroupID"]), PriceLevelID);
            if (DiscountOffer != 0 && DiscountOffer != null)
            {
                if (txtL3.Visible)
                {
                    decimal DiscountedL3 = objMarketingToolRepository.GetPriceAfterDiscount(txtL3.Text, Convert.ToInt64(DiscountOffer));
                    dvL3.InnerHtml = "<span style='text-decoration:line-through'> " + txtL3.Text + " |</span>";
                    txtL3.Text = Convert.ToString(DiscountedL3);
                    imgSale.Visible = true;
                }
                else
                {
                    decimal DiscountedPrice = objMarketingToolRepository.GetPriceAfterDiscount(txtPrice.Text, Convert.ToInt64(DiscountOffer));
                    dvPrice.InnerHtml = "<span style='text-decoration:line-through'> " + txtPrice.Text + " |</span>";
                    txtPrice.Text = Convert.ToString(DiscountedPrice);
                    imgSale.Visible = true;
                }

            }
            else
            {
                imgSale.Visible = false;
            }

            //----
        }
    }

    /// Insert the Record in MYShoppingCart Table.
    /// This table record shown in the My Shopping cart page.
    /// When click on lnkAddToCart button click.
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkAddToCart_Click(object sender, EventArgs e)
    {
        try
        {
            if (objproductitem.CheckAllowBackOrderID(this.AllowBackOrderID, this.ItemAllowBackOrderID.Value, this.Inventory, Convert.ToInt32(txtOrderQty.Text)))
            {
                CompanyEmployee objCmpnyInfo = new CompanyEmployee();
                CompanyEmployeeRepository objCompEmp = new CompanyEmployeeRepository();
                objCmpnyInfo = objCompEmp.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

                //Insert into MyShoppingcart Table
                if (this.MyShoppingCartID != 0)
                    objShoppingCart = objShoppingCartRepository.GetById(MyShoppingCartID, IncentexGlobal.CurrentMember.UserInfoID);

                objShoppingCart.CategoryID = Convert.ToInt64(ViewState["CategoryID"]);
                objShoppingCart.CompanyID = Convert.ToInt64(ViewState["CompanyID"]);
                objShoppingCart.ItemNumber = Convert.ToString(ViewState["ItemNumber"]);
                objShoppingCart.ProductDescrption = lblDescription.Text;
                objShoppingCart.Quantity = txtOrderQty.Text;
                objShoppingCart.Size = ddlSize.SelectedItem.Text;
                objShoppingCart.StoreID = Convert.ToInt64(ViewState["StoreID"]);
                objShoppingCart.StoreProductID = Convert.ToInt64(ViewState["StoreProductID"]);
                objShoppingCart.SubCategoryID = Convert.ToInt64(ViewState["SubCategoryID"]);
                objShoppingCart.TailoringLength = txtDesiredLength.Text;
                objShoppingCart.UnitPrice = txtL3.Visible == true ? txtL3.Text : txtPrice.Text; // Change by Shehzad 11-Jan-2011
                objShoppingCart.MOASUnitPrice = txtL3.Visible == true ? txtL3.Text : txtPrice.Text; // Change by Prashant 20-Jun-2013
                objShoppingCart.WorkgroupID = Convert.ToInt64(ViewState["WorkgroupID"]);
                objShoppingCart.Inventory = Convert.ToString(ViewState["Inventory"]);
                objShoppingCart.ProductImageID = Convert.ToString(ViewState["StoreProductImageId"]);
                objShoppingCart.MasterItemNo = Convert.ToInt64(ViewState["MasterItemNo"]);
                objShoppingCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                objShoppingCart.ProductItemID = Convert.ToInt64(ViewState["ProductItemID"]);
                objShoppingCart.SupplierID = Convert.ToInt64(ViewState["SupplierID"]);
                // If Coupa User
                if (!String.IsNullOrEmpty(CoupaID) && !String.IsNullOrEmpty(BuyerCookie))
                {
                    objShoppingCart.BuyerCookie = Convert.ToString(BuyerCookie);
                    objShoppingCart.IsCoupaOrder = true;
                    objShoppingCart.IsCoupaOrderSubmitted = false;
                }
                // Add Created Date on MyShopping Cart Table
                objShoppingCart.CreatedDate = DateTime.Now;
                // Added SoldbyName field in MyShoppingCart Table.
                objShoppingCart.SoldbyName = ddlOrderby.SelectedItem.Text;
                // Added MaterialStyle field in MyShoppingCart Table.
                if (ddlMaterialStyle.SelectedIndex != -1)
                {
                    objShoppingCart.MaterialStyle = ddlMaterialStyle.SelectedItem.Text;
                }
                else
                {
                    objShoppingCart.MaterialStyle = null;
                }


                if (IsCloseOutProduct)
                {
                    objShoppingCart.PriceLevel = 5;
                    objShoppingCart.MOASPriceLevel = 5;
                }
                else if (txtL3.Visible == true)
                {
                    objShoppingCart.PriceLevel = 3;
                    objShoppingCart.MOASPriceLevel = 3;
                }
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                {
                    objShoppingCart.PriceLevel = 2;
                    objShoppingCart.MOASPriceLevel = 2;
                }
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                {
                    objShoppingCart.PriceLevel = 1;
                    objShoppingCart.MOASPriceLevel = 1;
                }
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
                {
                    objShoppingCart.PriceLevel = 4;
                    objShoppingCart.MOASPriceLevel = 4;
                }

                if (this.Runcharge != null)
                {
                    objShoppingCart.RunCharge = Convert.ToString(this.Runcharge);
                }

                //Ankit Start 5-Feb-2011
                if (txtExample.Text != String.Empty)
                {
                    if (txtEmployeeTitle.Text != String.Empty)
                    {
                        objShoppingCart.NameToBeEngraved = txtExample.Text + "," + txtEmployeeTitle.Text;
                    }
                    else
                    {
                        objShoppingCart.NameToBeEngraved = txtExample.Text;
                    }

                }
                else
                {
                    objShoppingCart.NameToBeEngraved = null;
                }
                //End

                //Shehzad Start 5-Jan-2011
                objShoppingCart.IsOrdered = false;
                //shehzad end

                if (Convert.ToString(Session["ShowInventoryLevel"]) == "Yes")
                {
                    objShoppingCart.ShowInventoryLevel = "Yes";
                }
                else
                {
                    objShoppingCart.ShowInventoryLevel = "No";
                }
                if (!String.IsNullOrEmpty(BuyerCookie))
                    intDuplicate = objShoppingCartRepository.CheckDuplicateForCoupa(Convert.ToInt32(ViewState["StoreProductID"]), ddlSize.SelectedItem.Text, IncentexGlobal.CurrentMember.UserInfoID, ddlOrderby.SelectedItem.Text, this.BuyerCookie);
                else
                    intDuplicate = objShoppingCartRepository.CheckDuplicate(Convert.ToInt32(ViewState["StoreProductID"]), ddlSize.SelectedItem.Text, IncentexGlobal.CurrentMember.UserInfoID, ddlOrderby.SelectedItem.Text);

                if (intDuplicate == 0)
                {
                    //Check Here Inventory level is less than the Enter order quantity and is Allowbackoer is Yes OR No.
                    Int32 BackOrder = Convert.ToInt32(ViewState["AllowBAckOrder"]);
                    Int32 Inventory = Convert.ToInt32(ViewState["Inventory"]);
                    objLook = objLookupRepo.GetById(BackOrder);
                    if (objLook != null)
                    {
                        if (Inventory < Int32.Parse(txtOrderQty.Text) && objLook.sLookupName == "Yes")
                        {
                            if (this.MyShoppingCartID == 0)
                            {
                                objShoppingCartRepository.Insert(objShoppingCart);
                                objShoppingCartRepository.SubmitChanges();
                                this.MyShoppingCartID = Convert.ToInt32(objShoppingCart.MyShoppingCartID);
                            }
                            else
                            {
                                objShoppingCartRepository.SubmitChanges();
                                this.MyShoppingCartID = Convert.ToInt32(objShoppingCart.MyShoppingCartID);
                            }
                            lblMsg.Text = "";
                            Response.Redirect("~/My Cart/MyShoppinCart.aspx?SubCat=" + this.SubCatID + "&MasterItemNo=" + this.MasterItemNo + "&StoreProductId=" + this.StoreProductID, false);
                        }

                        else if (Inventory < Int32.Parse(txtOrderQty.Text) && objLook.sLookupName == "No")
                        {
                            String myStringVariable = String.Empty;
                            myStringVariable = "Please enter Order Quantity less than or equal to Inventory left: " + Session["ShowInventoryNo"];
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);

                            txtOrderQty.Text = "";
                            SetFocus(txtOrderQty);
                            return;
                        }
                        else //NA
                        {
                            if (this.MyShoppingCartID == 0)
                            {
                                objShoppingCartRepository.Insert(objShoppingCart);
                                objShoppingCartRepository.SubmitChanges();
                                this.MyShoppingCartID = Convert.ToInt32(objShoppingCart.MyShoppingCartID);
                            }
                            else
                            {
                                objShoppingCartRepository.SubmitChanges();
                                this.MyShoppingCartID = Convert.ToInt32(objShoppingCart.MyShoppingCartID);
                            }

                            lblMsg.Text = "";
                            Response.Redirect("~/My Cart/MyShoppinCart.aspx?SubCat=" + this.SubCatID + "&MasterItemNo=" + this.MasterItemNo + "&StoreProductId=" + this.StoreProductID, false);
                        }
                    }
                }
                else
                {
                    String myStringVariable = String.Empty;
                    myStringVariable = "Item already exists in your cart";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Please enter Order Quantity less than or equal to Inventory left for item size :" + ddlSize.SelectedItem.Text + "');", true);
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            if (!ex.Message.Contains("Input String was not in a correct format"))
                ErrHandler.WriteError(ex);
        }
    }

    protected void lnkbtnBulkOrder_Click(object sender, EventArgs e)
    {
        Int32 employeetypeid = 0;
        CompanyEmployee objCmpnyInfo = new CompanyEmployee();
        CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
        objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        if (objCmpnyInfo.EmployeeTypeID != null)
        {
            employeetypeid = Convert.ToInt32(objCmpnyInfo.EmployeeTypeID);
        }

        mpeBulkOrder.Show();
        List<SP_GetStoreProductForBulkOrderResult> objList = GetProductsBulkOrder();

        grdBulkOrder.DataSource = objList;
        grdBulkOrder.DataBind();
    }

    protected void grdBulkOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Start to display inventory or not
            HiddenField hdnShowInventoryLevelInStoreID = (HiddenField)e.Row.FindControl("hdnShowInventoryLevelInStoreID");
            Label lblInStock = (Label)e.Row.FindControl("lblInStock");
            if (!String.IsNullOrEmpty(hdnShowInventoryLevelInStoreID.Value))
            {
                objLook = objLookupRepo.GetById(Convert.ToInt64(hdnShowInventoryLevelInStoreID.Value));
                if (objLook.sLookupName != "Yes")
                    lblInStock.Text = "&nbsp;";
            }
            else
                lblInStock.Text = "&nbsp;";

            //To arrive on display or not
            Label lblNewStockArrivingOn = (Label)e.Row.FindControl("lblNewStockArrivingOn");
            //inventory
            String strDAteArrive = "1900-01-01";
            if (Convert.ToDateTime(lblNewStockArrivingOn.Text) != Convert.ToDateTime(strDAteArrive))
                lblNewStockArrivingOn.Text = Convert.ToDateTime(lblNewStockArrivingOn.Text).ToShortDateString();
            else
                lblNewStockArrivingOn.Text = "&nbsp;";

            //Stock to Arrive
            Label lblStockToArrive = (Label)e.Row.FindControl("lblStockToArrive");
            //inventory
            if (lblStockToArrive.Text == "0" || lblStockToArrive.Text == "")
                lblStockToArrive.Text = "&nbsp;";

        }
    }
    public bool CheckFileName(string filename, string filepath)
    {
        //deleting thumb file physically

        if (File.Exists(filepath + filename))
        {
            return false;
        }
        return true;
    }
    protected void lnkbtnBulkOrderAddToCart_Click(object sender, EventArgs e)
    {
        if (dvUpload.Visible)
        {
            //--Saurabh-Upload Excel for Name Bar Bulk Upload
            string sFilePath = null;
            String filename = String.Empty;
            Common objcommon = new Common();
            if (Request.Files.Count > 0)
            {

                if (Request.Files.Count > 0)
                {
                    if (((float)Request.Files[0].ContentLength / 1048576) > 2)
                    {
                        lblMsg.Text = "The file you are uploading is more than 2MB.";
                        return;
                    }
                    if (!String.IsNullOrEmpty(ExcelDoc.Value))
                    {
                        filename = ExcelDoc.Value;
                        sFilePath = Server.MapPath("../UploadedImages/NameBarFile/") + ExcelDoc.Value;
                        //objcommon.DeleteImageFromFolder(ExcelDoc.Value, Server.MapPath("../UploadedImages/NameBarFile/"));
                        if (CheckFileName(ExcelDoc.Value, Server.MapPath("../UploadedImages/NameBarFile/")))
                        {
                            Request.Files[0].SaveAs(sFilePath);
                        }
                        else
                        {
                            lblMsg.Text = "Same File Name Exist. Please Rename the New File and Upload Again";
                            return;
                        }
                        AddNameBartoCart();
                    }

                }
            }
            //--     
        }
        //This is for Generate error message if quantity is less then the inventory
        String StrMessage = "";
        foreach (GridViewRow grv in grdBulkOrder.Rows)
        {
            if (((TextBox)grv.FindControl("txtQuantity")).Text != "")
            {
                try
                {
                    Int32 intQuantity = Convert.ToInt32(((TextBox)grv.FindControl("txtQuantity")).Text);
                    Int32 intInventory = Convert.ToInt32(((HiddenField)grv.FindControl("hdnInventory")).Value);

                    //Check Here Inventory level is less than the Enter order quantity and is Allowbackorder is Yes OR No.
                    Int64 BackOrder = !string.IsNullOrEmpty(((HiddenField)grv.FindControl("hdnAllowBackOrderID")).Value) ? Convert.ToInt32(((HiddenField)grv.FindControl("hdnAllowBackOrderID")).Value) : 0;
                    Int64 MasterItemBackOrder = Convert.ToInt32(((HiddenField)grv.FindControl("hdnMasterAllowBackOrderID")).Value);

                    objLook = objLookupRepo.GetById(MasterItemBackOrder);

                    if (objLook != null)
                    {
                        //if (intInventory < intQuantity && objLook.sLookupName == "Backorder's set at item level") //Check at master level
                        //{
                        //    objLook = BackOrder > 0 ? objLookupRepo.GetById(BackOrder) : new INC_Lookup();
                        //    if (objLook.sLookupName == null || objLook.sLookupName == "No") //Check at Item level
                        //    {
                        //        if (string.IsNullOrEmpty(StrMessage))
                        //            StrMessage = "Please enter Order Quantity less than or equal to Inventory left for bellow item size : \\n ";

                        //        StrMessage += ((Label)grv.FindControl("lblSize")).Text + " \\n ";
                        //    }
                        //}
                        //else if (intInventory < intQuantity && objLook.sLookupName == "No") //Check at master level
                        //{
                        //    if (string.IsNullOrEmpty(StrMessage))
                        //        StrMessage = "Please enter Order Quantity less than or equal to Inventory left for bellow item size : \\n ";

                        //    StrMessage += ((Label)grv.FindControl("lblSize")).Text + " \\n ";
                        //}

                        if (!objproductitem.CheckAllowBackOrderID(MasterItemBackOrder, BackOrder, intInventory, intQuantity))
                        {
                            if (string.IsNullOrEmpty(StrMessage))
                                StrMessage = "Please enter Order Quantity less than or equal to Inventory left for bellow item size : \\n ";

                            StrMessage += ((Label)grv.FindControl("lblSize")).Text + " \\n ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex);
                }
            }
        }

        if (StrMessage != "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + StrMessage + "');", true);
            return;
        }

        foreach (GridViewRow grv in grdBulkOrder.Rows)
        {
            if (((TextBox)grv.FindControl("txtQuantity")).Text != "")
            {
                try
                {
                    CompanyEmployee objCmpnyInfo = new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
                    objShoppingCart = new MyShoppinCart();
                    //Insert into MyShoppingcart Table

                    objShoppingCart.CategoryID = Convert.ToInt64(((HiddenField)grv.FindControl("hdnCategoryID")).Value);
                    objShoppingCart.CompanyID = Convert.ToInt64(((HiddenField)grv.FindControl("hdnCompanyID")).Value);
                    objShoppingCart.ItemNumber = ((HiddenField)grv.FindControl("hdnItemNumber")).Value;
                    objShoppingCart.ProductDescrption = ((HiddenField)grv.FindControl("hdnProductDescrption")).Value;
                    objShoppingCart.Quantity = ((TextBox)grv.FindControl("txtQuantity")).Text;
                    objShoppingCart.Size = ((Label)grv.FindControl("lblSize")).Text;
                    objShoppingCart.StoreID = Convert.ToInt64(((HiddenField)grv.FindControl("hdnStoreID")).Value);
                    objShoppingCart.StoreProductID = Convert.ToInt64(((HiddenField)grv.FindControl("hdnStoreProductID")).Value);
                    objShoppingCart.SubCategoryID = Convert.ToInt64(((HiddenField)grv.FindControl("hdnSubCategoryID")).Value);
                    objShoppingCart.TailoringLength = txtDesiredLength.Text;
                    objShoppingCart.WorkgroupID = Convert.ToInt64(((HiddenField)grv.FindControl("hdnWorkgroupID")).Value);
                    objShoppingCart.Inventory = ((HiddenField)grv.FindControl("hdnInventory")).Value;
                    objShoppingCart.MasterItemNo = Convert.ToInt64(((HiddenField)grv.FindControl("hdnMasterItemNo")).Value);
                    objShoppingCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                    objShoppingCart.ProductItemID = Convert.ToInt64(((HiddenField)grv.FindControl("ProductItemID")).Value);
                    objShoppingCart.SupplierID = Convert.ToInt64(((HiddenField)grv.FindControl("hdnSupplierID")).Value);
                    // If Coupa User
                    if (!String.IsNullOrEmpty(CoupaID) && !String.IsNullOrEmpty(BuyerCookie))
                    {
                        objShoppingCart.BuyerCookie = Convert.ToString(BuyerCookie);
                        objShoppingCart.IsCoupaOrder = true;
                        objShoppingCart.IsCoupaOrderSubmitted = false;
                    }
                    objShoppingCart.CreatedDate = DateTime.Now;
                    if (((HiddenField)grv.FindControl("hdnSoldByName")).Value != "")
                        objShoppingCart.SoldbyName = ((HiddenField)grv.FindControl("hdnSoldByName")).Value;

                    List<StoreProductImage> objImageList = objStoreProductImageRepository.GetStoreProductImagesdById(Convert.ToInt32(((HiddenField)grv.FindControl("hdnStoreProductID")).Value), Convert.ToInt32(((HiddenField)grv.FindControl("hdnMasterItemNo")).Value));

                    if (objImageList.Count > 0)
                        objShoppingCart.ProductImageID = objImageList[0].StoreProductImageId.ToString();

                    //this is for item pricing based on level selected for that item
                    HiddenField Level3PricingStatus = (HiddenField)grv.FindControl("hdnLevel3PricingStatus");

                    //  Set the Price level to CloseOutPrice if Product is CloseOut Product
                    if (IsCloseOutProduct)
                    {
                        objShoppingCart.PriceLevel = 5;
                        objShoppingCart.UnitPrice = ((HiddenField)grv.FindControl("hdnCloseOutPrice")).Value;
                        objShoppingCart.MOASUnitPrice = ((HiddenField)grv.FindControl("hdnCloseOutPrice")).Value;
                        objShoppingCart.MOASPriceLevel = 5;
                    }
                    else if (!String.IsNullOrEmpty(Level3PricingStatus.Value))
                    {
                        if (objLookupRepo.GetById(Convert.ToInt64(Level3PricingStatus.Value)).sLookupName.ToLower() == "active")
                        {
                            if (DateTime.Now.Date <= Convert.ToDateTime(((HiddenField)grv.FindControl("hdnLevel3PricingEndDate")).Value) && DateTime.Now.Date >= Convert.ToDateTime(((HiddenField)grv.FindControl("hdnLevel3PricingStartDate")).Value))
                            {
                                objShoppingCart.PriceLevel = 3;
                                objShoppingCart.MOASPriceLevel = 3;
                                objShoppingCart.UnitPrice = ((HiddenField)grv.FindControl("hdnLevel3")).Value;
                                objShoppingCart.MOASUnitPrice = ((HiddenField)grv.FindControl("hdnLevel3")).Value;
                            }
                        }
                    }
                    else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                    {
                        objShoppingCart.PriceLevel = 1;
                        objShoppingCart.MOASPriceLevel = 1;
                        objShoppingCart.UnitPrice = ((HiddenField)grv.FindControl("hdnLevel1")).Value;
                        objShoppingCart.MOASUnitPrice = ((HiddenField)grv.FindControl("hdnLevel1")).Value;
                    }
                    else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                    {
                        objShoppingCart.PriceLevel = 2;
                        objShoppingCart.MOASPriceLevel = 2;
                        objShoppingCart.UnitPrice = ((HiddenField)grv.FindControl("hdnLevel2")).Value;
                        objShoppingCart.MOASUnitPrice = ((HiddenField)grv.FindControl("hdnLevel2")).Value;
                    }
                    else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
                    {
                        objShoppingCart.PriceLevel = 4;
                        objShoppingCart.MOASPriceLevel = 4;
                        objShoppingCart.UnitPrice = ((HiddenField)grv.FindControl("hdnLevel4")).Value;
                        objShoppingCart.MOASUnitPrice = ((HiddenField)grv.FindControl("hdnLevel4")).Value;
                    }

                    HiddenField TailoringStatusName = (HiddenField)grv.FindControl("hdnTailoringStatusName");
                    HiddenField hdnRunCharge = (HiddenField)grv.FindControl("hdnRunCharge");

                    if (!String.IsNullOrEmpty(TailoringStatusName.Value))
                    {
                        if (TailoringStatusName.Value.ToLower() == "active")
                        {
                            if (!String.IsNullOrEmpty(hdnRunCharge.Value))
                                objShoppingCart.RunCharge = hdnRunCharge.Value;
                        }
                    }

                    txtFontFormat.Text = String.Empty;
                    txtNameFormat.Text = String.Empty;

                    HiddenField hdnNameFormatForNameBars = (HiddenField)grv.FindControl("hdnNameFormatForNameBars");
                    HiddenField hdnFontFormatForNameBars = (HiddenField)grv.FindControl("hdnFontFormatForNameBars");

                    if (hdnFontFormatForNameBars.Value != "-select-")
                    {
                        txtFontFormat.Text = hdnFontFormatForNameBars.Value;
                    }
                    if (hdnNameFormatForNameBars.Value != "-select-")
                    {
                        txtNameFormat.Text = hdnNameFormatForNameBars.Value;
                    }

                    if (txtExample.Text != String.Empty)
                    {
                        if (txtEmployeeTitle.Text != String.Empty)
                        {
                            objShoppingCart.NameToBeEngraved = txtExample.Text + "," + txtEmployeeTitle.Text;
                        }
                        else
                        {
                            objShoppingCart.NameToBeEngraved = txtExample.Text;
                        }

                    }
                    else
                    {
                        objShoppingCart.NameToBeEngraved = null;
                    }

                    objShoppingCart.IsOrdered = false;
                    objShoppingCart.IsBulkOrder = true;

                    HiddenField hdnShowInventoryLevelInStoreID = (HiddenField)grv.FindControl("hdnShowInventoryLevelInStoreID");
                    if (!String.IsNullOrEmpty(hdnShowInventoryLevelInStoreID.Value))
                    {
                        objLook = objLookupRepo.GetById(Convert.ToInt64(hdnShowInventoryLevelInStoreID.Value));
                        if (objLook.sLookupName == "Yes")
                        {
                            objShoppingCart.ShowInventoryLevel = "Yes";
                        }
                        else
                        {
                            objShoppingCart.ShowInventoryLevel = "No";
                        }
                    }
                    if (!String.IsNullOrEmpty(BuyerCookie))
                        intDuplicate = objShoppingCartRepository.CheckDuplicateForCoupa(Convert.ToInt32(objShoppingCart.StoreProductID), objShoppingCart.Size, IncentexGlobal.CurrentMember.UserInfoID, objShoppingCart.SoldbyName, this.BuyerCookie);
                    else
                        intDuplicate = objShoppingCartRepository.CheckDuplicate(Convert.ToInt32(objShoppingCart.StoreProductID), objShoppingCart.Size, IncentexGlobal.CurrentMember.UserInfoID, objShoppingCart.SoldbyName);
                    if (intDuplicate == 0)
                    {
                        objShoppingCartRepository.Insert(objShoppingCart);
                        objShoppingCartRepository.SubmitChanges();
                    }
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex);
                }
            }
        }

        mpeBulkOrder.Hide();
        Response.Redirect("~/My Cart/MyShoppinCart.aspx?SubCat=" + this.SubCatID + "&MasterItemNo=" + this.MasterItemNo + "&StoreProductId=" + this.StoreProductID);
    }
    #endregion
}