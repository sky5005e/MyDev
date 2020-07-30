using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Configuration;

public partial class UserFrameItems_ShopProductItem : System.Web.UI.Page
{

    #region Page Variable's
    String SiteURL
    {
        get
        {
            return Convert.ToString(ConfigurationSettings.AppSettings["siteurl"]);
        }
    }

    Int64 StatusActive
    {
        get
        {
            return Convert.ToInt64(ViewState["StatusActive"]);
        }
        set
        {
            ViewState["StatusActive"] = value;
        }
    }

    Int64 StoreProductID
    {
        get
        {
            return Convert.ToInt64(ViewState["StoreProductID"]);
        }
        set
        {
            ViewState["StoreProductID"] = value;
        }
    }
    Int64 SizeID
    {
        get
        {
            return Convert.ToInt64(ViewState["SizeID"]);
        }
        set
        {
            ViewState["SizeID"] = value;
        }
    }
    Int64 SoldbyID
    {
        get
        {
            return Convert.ToInt64(ViewState["soldbyID"]);
        }
        set
        {
            ViewState["soldbyID"] = value;
        }
    }

    Int64 CartID
    {
        get
        {
            return Convert.ToInt64(ViewState["CartID"]);
        }
        set
        {
            ViewState["CartID"] = value;
        }
    }

    String VideoURL
    {
        get
        {
            return Convert.ToString(ViewState["VideoURL"]);
        }
        set
        {
            ViewState["VideoURL"] = value;
        }
    }

    Boolean IsFromProductPage
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsFromProductPage"]);
        }
        set
        {
            ViewState["IsFromProductPage"] = value;
        }

    }

    String dvCartRemove
    {
        get
        {
            return Convert.ToString(ViewState["dvCartRemove"]);
        }
        set
        {
            ViewState["dvCartRemove"] = value;
        }
    }

    String dvCartAdded
    {
        get
        {
            return Convert.ToString(ViewState["dvCartAdded"]);
        }
        set
        {
            ViewState["dvCartAdded"] = value;
        }
    }
    #endregion

    #region Page Even't
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Int32 qty = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["cartId"]))
                CartID = Convert.ToInt64(Request.QueryString["cartId"]);
            if (!String.IsNullOrEmpty(Request.QueryString["spID"]))
                StoreProductID = Convert.ToInt64(Request.QueryString["spID"]);
           
            if (!String.IsNullOrEmpty(Request.QueryString["sizeID"]))
                SizeID = Convert.ToInt64(Request.QueryString["sizeID"]);
            if (!String.IsNullOrEmpty(Request.QueryString["soldbyid"]))
                SoldbyID = Convert.ToInt64(Request.QueryString["soldbyid"]);
            if (!String.IsNullOrEmpty(Request.QueryString["qty"]))
                qty = Convert.ToInt32(Request.QueryString["qty"]);

            if (!String.IsNullOrEmpty(Request.QueryString["IsfromProduct"]))
                IsFromProductPage = Convert.ToBoolean(Request.QueryString["IsfromProduct"]);
            if (!String.IsNullOrEmpty(Request.QueryString["dvCartAdded"]))
                dvCartAdded = Convert.ToString(Request.QueryString["dvCartAdded"]);
            if (!String.IsNullOrEmpty(Request.QueryString["dvCartRemove"]))
                dvCartRemove = Convert.ToString(Request.QueryString["dvCartRemove"]);

            PopulateData(CartID, StoreProductID, SizeID, SoldbyID, qty);
            dvVideoContent.Visible = false;
        }
    }

    protected void lnkSaveCartItem_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtMasterPopupQty.Text != "QTY")
            {
                WSUser objWSUser = new WSUser();
                if (IsFromProductPage)
                {
                    objWSUser.AddProductToCart(Convert.ToString(StoreProductID), Convert.ToString(ddlMasterPopupSize.SelectedValue), Convert.ToString(ddlMasterPopupSoldBy.SelectedValue), Convert.ToString(txtMasterPopupQty.Text), Convert.ToString(lblMasterPopupAmount.Text));
                    this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "window.parent.SetAddedToCartItems('" + dvCartAdded + "','" + dvCartRemove + "');", true);
                }
                else
                    objWSUser.UpdateProductToCart(Convert.ToString(StoreProductID), Convert.ToString(ddlMasterPopupSize.SelectedValue), Convert.ToString(ddlMasterPopupSoldBy.SelectedValue), Convert.ToString(txtMasterPopupQty.Text), Convert.ToString(lblMasterPopupAmount.Text), Convert.ToString(CartID));
                this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "window.parent.BindCartItems();", true);
            }
            else
                this.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "window.parent.GeneralAlertMsg('please enter qty');", true);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
        }

    }

    protected void lnkMastervideoDisplay_Click(object sender, EventArgs e)
    {
        dvUpdateContent.Visible = false;
        iframeMasterVideo.Attributes.Add("src", this.VideoURL);
        dvVideoContent.Visible = true;
    }


    protected void lnkBackToProduct_Click(object sender, EventArgs e)
    {
        dvUpdateContent.Visible = true;
        iframeMasterVideo.Attributes.Add("src", String.Empty);
        dvVideoContent.Visible = false;

    }

    protected void ddlMasterPopupSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetProductDetails(null,Convert.ToInt64(ddlMasterPopupSize.SelectedValue), 0);
    }

    protected void ddlMasterPopupSoldBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetProductDetails(null,Convert.ToInt64(ddlMasterPopupSize.SelectedValue), Convert.ToInt64(ddlMasterPopupSoldBy.SelectedValue));
    }
    #endregion

    #region Page Method's
    private void PopulateData(Int64 CartID, Int64 StoreProductID, Int64 sizeID, Int64 soldbyID, Int32 qty)
    {
        StatusActive = Convert.ToInt64(new LookupRepository().GetIdByLookupNameNLookUpCode("Active", "Status"));
        ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
        List<GetProductItemDetailsByStoreProductIDResult> objList = new List<GetProductItemDetailsByStoreProductIDResult>();
        try
        {
            objList = objRepository.GetProductItemDetailsByStoreProductID(Convert.ToInt64(StoreProductID), IncentexGlobal.CurrentMember.UserInfoID).ToList();

            // To get Distinct value from Generic List using LINQ
            // Create an Equality Comprarer Intance
            IEqualityComparer<GetProductItemDetailsByStoreProductIDResult> customComparer = new Common.PropertyComparer<GetProductItemDetailsByStoreProductIDResult>("ItemSizeID");
            IEnumerable<GetProductItemDetailsByStoreProductIDResult> distinctSizeList = objList.Distinct(customComparer);
            // Bind Size 
            ddlMasterPopupSize.DataSource = distinctSizeList;
            ddlMasterPopupSize.DataTextField = "ItemSize";
            ddlMasterPopupSize.DataValueField = "ItemSizeID";
            ddlMasterPopupSize.DataBind();
            BindSoldBy(objList);

            if (!IsFromProductPage)
            {
                ddlMasterPopupSize.SelectedValue = Convert.ToString(sizeID);
                ddlMasterPopupSoldBy.SelectedValue = Convert.ToString(soldbyID);
                txtMasterPopupQty.Text = Convert.ToString(qty);
                lblbtnText.Text = "Update Cart";
            }
            else
            {
                sizeID = Convert.ToInt64(ddlMasterPopupSize.SelectedValue);
                lblbtnText.Text = "Add to Cart";
            }

            if (objList.Count > 0)
            {
                SetProductDetails(objList, sizeID, 0);
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
        }
    }


    /// <summary>
    /// Get Price Amount
    /// </summary>
    /// <param name="objprod"></param>
    /// <returns></returns>
    private String GetPriceAmount(GetProductItemDetailsByStoreProductIDResult objprod)
    {
        String Amount = String.Empty;
        // IF Product is Close out Product and Set True to IsCloseOutProduct 
        if ((Boolean)objprod.IsCloseOut)
            Amount = Convert.ToString(objprod.CloseOutPrice);
        else if (objprod.Level3PricingStatus != null && objprod.Level3PricingStatus == StatusActive && DateTime.Now.Date <= objprod.Level3PricingEndDate && DateTime.Now.Date >= objprod.Level3PricingStartDate)
            Amount = Convert.ToString(objprod.Level3PricingStatus);
        else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            Amount = Convert.ToString(objprod.Level1);
        else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
            Amount = Convert.ToString(objprod.Level2);
        else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
            Amount = Convert.ToString(objprod.Level4);

        return Amount;
    }

    protected void SetProductDetails(List<GetProductItemDetailsByStoreProductIDResult> productList,Int64 _sizedID, Int64 _soldbyID)
    {
       
        try
        {
            if (productList == null)
            {
                ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
                productList = objRepository.GetProductItemDetailsByStoreProductID(Convert.ToInt64(StoreProductID), IncentexGlobal.CurrentMember.UserInfoID).ToList();
            }
            GetProductItemDetailsByStoreProductIDResult objprod = new GetProductItemDetailsByStoreProductIDResult();
            if (_sizedID > 0 && _soldbyID > 0)
            {
                objprod = productList.Where(q => q.ItemSizeID == Convert.ToInt64(Convert.ToInt64(_sizedID)) && q.Soldby == Convert.ToInt64(Convert.ToInt64(_soldbyID))).FirstOrDefault();
                BindSoldBy(productList);
            }
            else if (_sizedID > 0)
            {
                objprod = productList.Where(q => q.ItemSizeID == Convert.ToInt64(Convert.ToInt64(_sizedID))).FirstOrDefault();
                BindSoldBy(productList);
            }
            // START - Display Images
            if (!String.IsNullOrEmpty(objprod.LargerProductImage))
            {
                String LargeImagePath = SiteURL + ConfigurationSettings.AppSettings["ProductImagesLargePath"].Remove(0, 2);
                imgMasterPopImage.ImageUrl = LargeImagePath + objprod.LargerProductImage;
            }
            else
                imgMasterPopImage.ImageUrl = SiteURL + "UploadedImages/ProductImages/ProductDefault.jpg";
            String PriceAmount = GetPriceAmount(objprod);
            lblMasterProductDescription.Text = Convert.ToString(objprod.ProductDescrption);
            lblMasterPopupColor.Text = Convert.ToString(objprod.ItemColorName);
            lblMasterPopupAmount.Text = Convert.ToString(PriceAmount);
            lblbtnMasterPopupAmount.Text = Convert.ToString(PriceAmount);
            lblMasterPopupInventory.Text = Convert.ToString(objprod.Inventory);
            lblMasterPopupItemNumber.Text = Convert.ToString(objprod.ItemNumber);
            lblMasterPopupsummary.Text = Convert.ToString(objprod.Summary);
            lblMasterPopupInventory.Text = Convert.ToString(objprod.Inventory);

            // Set Size Chart and Measurement Chart
            if (objprod.TailoringStatusName == "Active" && objprod.TailoringGuidelines != null)
            {
                lnkMasterPopupSizeChart.Attributes.Add("style", "display: block;");
                lnkMasterPopupSizeChart.Attributes.Add("href", "../UploadedImages/TailoringMeasurement/" + objprod.TailoringGuidelines);
            }

            if (objprod.TailoringMeasurementName == "Active" && objprod.TailoringMeasurementChart != null)
            {
                lnkMasterPopupMeasurement.Attributes.Add("style", "display: block;");
                lnkMasterPopupMeasurement.Attributes.Add("href", "../UploadedImages/TailoringMeasurement/" + objprod.TailoringMeasurementChart);
            }
            if (objprod.ShowProductVideo && objprod.ProductVideoUrl != null)
            {
                lnkMastervideoDisplay.Attributes.Add("style", "visibility: visible");
                VideoURL = objprod.ProductVideoUrl;
            }
            if (objprod.ShowCertification && objprod.CertificationPath != null)
            {
                lnkMastercertification.Attributes.Add("style", "visibility: visible");
                lnkMastercertification.Attributes.Add("href", "../UploadedImages/ProductCertification/" + objprod.CertificationPath);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    private void BindSoldBy(List<GetProductItemDetailsByStoreProductIDResult> objList)
    {
        // To get Distinct value from Generic List using LINQ
        // Create an Equality Comprarer Intance
        IEqualityComparer<GetProductItemDetailsByStoreProductIDResult> customComparerSoldby = new Common.PropertyComparer<GetProductItemDetailsByStoreProductIDResult>("Soldby");
        IEnumerable<GetProductItemDetailsByStoreProductIDResult> distinctSizeListSoldby = objList.Distinct(customComparerSoldby);
        ddlMasterPopupSoldBy.DataSource = distinctSizeListSoldby;
        ddlMasterPopupSoldBy.DataTextField = "SoldbyName";
        ddlMasterPopupSoldBy.DataValueField = "Soldby";
        ddlMasterPopupSoldBy.DataBind();
        if (SoldbyID > 0)
            ddlMasterPopupSoldBy.SelectedValue = Convert.ToString(SoldbyID);
    }
    #endregion
}
