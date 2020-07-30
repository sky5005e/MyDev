using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class Products_SaleProductDetail : PageBase
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

    #endregion

    #region Objects/Variables

    StoreProductRepository objStoreProductRepository = new StoreProductRepository();
    StoreProductImageRepository objStoreProductImageRepository = new StoreProductImageRepository();

    #endregion

    #region Page Load Event

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Sale Product Detail";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Products/SaleProductList.aspx";

            this.StoreProductID = Convert.ToInt64(Request.QueryString.Get("StoreProductId"));
            this.MasterItemNo = Convert.ToInt64(Request.QueryString.Get("MasterItemNo"));

            trL3.Visible = false;
            dvL3Msg.Visible = false;

            DisplayData();
            ddlSize_SelectedIndexChanged(sender, e);
            bindAllProductImages();

            //Function created on 2 Mar 11
            SetUserStoreOptions();
        }
    }

    #endregion

    #region Functions
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
                        goto foundcell;
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
                    goto foundcell;
                }
                else
                {
                    DoNotShowPriceToUser = false;
                }
            }
        }

        //Set
    foundcell:
        if (DoNotShowPriceToUser)
        {
            trPrice.Visible = false;
            trL3.Visible = false;
        }

    }

    void DisplayData()
    {
        List<SP_GetStoreProductResult> objList = GetProducts();

        //bind size
        var SizeList = (from s in objList
                        select new { s.ItemSizeID, s.ItemSize }).Distinct().ToList();

        Common.BindDDL(ddlSize, SizeList, "ItemSize", "ItemSizeID", "");
    }

    /// <summary>
    /// Get Products List using emp workgroup and subcategory
    /// </summary>
    /// <returns></returns>
    List<SP_GetStoreProductResult> GetProducts()
    {
        List<SP_GetStoreProductResult> objList = new List<SP_GetStoreProductResult>();

        CompanyEmployee objCmpnyInfo = new CompanyEmployee();
        CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
        objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        var qry = objStoreProductRepository.GetSaleStoreProductItems((Int64)IncentexGlobal.CurrentMember.CompanyId, (Int64)objCmpnyInfo.WorkgroupID, (Int64)IncentexGlobal.CurrentMember.UserInfoID);
        qry = qry.Where(a => a.StoreProductID == this.StoreProductID).ToList();
        objList = qry;

        return objList;
    }

    /// <summary>
    /// Function to get all the product images
    /// </summary>
    public void bindAllProductImages()
    {
        List<StoreProductImage> objStoreProductImage = new List<StoreProductImage>();
        StoreProductImageRepository objRepos = new StoreProductImageRepository();

        objStoreProductImage = objRepos.GetStoreProductImagesdByMasterID(Convert.ToInt64(hfMasterItemNumber.Value), Convert.ToInt64(hfProductItemId.Value));
        objStoreProductImage = objStoreProductImage.OrderByDescending(a => a.ProductImageActive).ToList();
        dtProductImages.DataSource = objStoreProductImage;
        dtProductImages.DataBind();
    }

    #endregion

    #region Events

    protected void dtProductImages_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (((HiddenField)e.Item.FindControl("hdnimagestatus")).Value.ToString() == "1")
                {
                    ((HtmlImage)e.Item.FindControl("imgSplashImage")).Src = "~/controller/createthumb.aspx?_ty=ProductImages&_path=" + ((HiddenField)e.Item.FindControl("hdndocumentname")).Value + "&_twidth=145&_theight=198";
                    ((HtmlAnchor)e.Item.FindControl("prettyphotoDiv")).HRef = "~/UploadedImages/ProductImages/" + ((HiddenField)e.Item.FindControl("hdnlargerimagename")).Value;
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

    protected void ddlSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        trL3.Visible = false;
        dvL3Msg.Visible = false;
        trInventory.Visible = false;

        if (ddlSize.SelectedValue != "0")
        {

            List<SP_GetStoreProductResult> objList = GetProducts();
            List<SP_GetStoreProductResult> obj = (from p in objList
                                                  where p.ItemSizeID == Convert.ToInt64(ddlSize.SelectedValue)
                                                  select p).Distinct().ToList();

            hfMasterItemNumber.Value = obj[0].MasterItemNo.ToString();
            hfProductItemId.Value = obj[0].StoreProductID.ToString();

            lblItemNumber.Text = obj[0].MasterItemName;
            lblDescription.Text = obj[0].ProductDescrption;

            LookupRepository objLookupRepo = new LookupRepository();

            if (obj[0].Level3PricingStatus != null)
            {
                if (objLookupRepo.GetById((Int64)(obj[0].Level3PricingStatus)).sLookupName.ToLower() == "active" && obj[0].Level3PricingStatus != null)
                {
                    if (DateTime.Now.Date <= obj[0].Level3PricingEndDate && DateTime.Now.Date >= obj[0].Level3PricingStartDate)
                    {
                        imgSale.Visible = true;
                        trL3.Visible = true;
                        dvL3Msg.Visible = true;
                        txtL3.Text = obj[0].Level3.ToString();
                        lblDiscountExpDate.Text = Convert.ToDateTime(obj[0].Level3PricingEndDate).ToShortDateString();
                    }
                    else
                    {
                        imgSale.Visible = false;
                        txtL3.Visible = false;
                        dvL3Msg.Visible = false;
                    }
                }
                else
                {
                    imgSale.Visible = false;
                    txtL3.Visible = false;
                    dvL3Msg.Visible = false;
                }
            }
            else
            {
                imgSale.Visible = false;
                txtL3.Visible = false;
                dvL3Msg.Visible = false;
            }


            if (obj[0].NewProductUntil != null)
            {
                if (obj[0].NewProductUntil >= DateTime.Now)
                {
                    imgNew.Visible = true;
                }
                else
                {
                    imgNew.Visible = false;
                }
            }
            else
            {
                imgNew.Visible = false;
            }


            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                txtPrice.Text = obj[0].Level1.ToString();
            else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                txtPrice.Text = obj[0].Level2.ToString();
            else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
                txtPrice.Text = obj[0].Level4.ToString();

            // Color
            imgColor.Src = "../admin/Incentex_Used_Icons/" + objLookupRepo.GetById((Int64)obj[0].ItemColorID).sLookupIcon;
            lblColor.Text = objLookupRepo.GetById((Int64)obj[0].ItemColorID).sLookupName;

            txtOrderIn.Text = obj[0].SoldbyName;

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

            ViewState["ItemNumber"] = obj[0].ItemNumber;
            ViewState["Inventory"] = obj[0].Inventory;
            ViewState["MasterItemNo"] = obj[0].MasterItemNo;
            ViewState["StoreID"] = obj[0].StoreID;
            ViewState["StoreProductID"] = obj[0].StoreProductID;
            ViewState["SubCategoryID"] = obj[0].SubCategoryID;
            ViewState["CategoryID"] = obj[0].CategoryID;
            ViewState["WorkgroupID"] = obj[0].WorkgroupID;
            ViewState["CompanyID"] = obj[0].CompanyID;

            // display image
            List<StoreProductImage> objImageList = objStoreProductImageRepository.GetStoreProductImagesdById((Int32)obj[0].StoreProductID, (Int32)obj[0].MasterItemNo);

            if (objImageList.Count > 0)
                ViewState["StoreProductImageId"] = objImageList[0].StoreProductImageId;
        }
        SetUserStoreOptions();

    }

    #endregion
}