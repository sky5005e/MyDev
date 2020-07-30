using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class Products_ProductList : PageBase
{
    #region Properties

    Int64 PSubCategoryID
    {
        get
        {
            return Convert.ToInt64(ViewState["PSubCategoryID"]);
        }
        set
        {
            ViewState["PSubCategoryID"] = value;
        }
    }
    /// <summary>
    /// Set true when product is close out
    /// </summary>
    Boolean IsCloseOut
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsCloseOut"]);
        }
        set
        {
            ViewState["IsCloseOut"] = value;
        }
    }

    StoreProductRepository objStoreProductRepository = new StoreProductRepository();
    StoreProductImageRepository objStoreProductImageRepository = new StoreProductImageRepository();
    SubCatogeryRepository objSubCatogeryRepository = new SubCatogeryRepository();

    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {   
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Product List";

            //Below code is to set the index link for the Incentex Employee or User himself..
            if (IncentexGlobal.IsIEFromStore)
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            else
            {
                if (Request.QueryString["sc"] == null)
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
                else
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/My Cart/MyShoppinCart.aspx";
            }
            if (Request.QueryString.Count > 0 && Request.QueryString["SubCat"] != null && Request.QueryString["SubCat"] != "")
            {
                this.PSubCategoryID = Convert.ToInt64(Request.QueryString["SubCat"]);
                ((Label)Master.FindControl("lblPageHeading")).Text = objSubCatogeryRepository.GetSubCategoryByID(this.PSubCategoryID).SubCategoryName;
            }
            if (!String.IsNullOrEmpty(Request.QueryString["IsCloseOut"]) && Request.QueryString["IsCloseOut"] == "1")
                this.IsCloseOut = true;

            // Create session for Absolute URL 
            Session["ProductListUrl"] = Request.Url.AbsoluteUri;

            DisplayData();
        }
    }

    #endregion

    #region DisplayData

    void DisplayData()
    {
        lstProductList.DataBind();

        if (lstProductList.Items.Count == 0)
            lblMsg.Visible = true;
    }

    #endregion

    #region Events

    protected void lstProductList_DataBinding(object sender, EventArgs e)
    {
        Int32 employeetypeid = 0;

        CompanyEmployee objCmpnyInfo = new CompanyEmployee();
        CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();
        objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);

        if (objCmpnyInfo.EmployeeTypeID != null)
            employeetypeid = Convert.ToInt32(objCmpnyInfo.EmployeeTypeID);

        // When PSubCategoryID is zero
        Int64? subCateID = null;
        if (this.PSubCategoryID != 0)
            subCateID = this.PSubCategoryID;

        List<SP_GetStoreProductBySubCatIDResult> objList = objStoreProductRepository.GetStoreProductBySubCatID((Int64)IncentexGlobal.CurrentMember.CompanyId, subCateID);
        // Display only Closeout products
        if (IsCloseOut)
            objList = objList.Where(s => s.IsCloseOut == true).ToList();
        else
            objList = objList.Where(s => s.IsCloseOut == false).ToList();// Display without Closeout products

        //Update By Gaurang 02 March 2013
        #region Climate Setting

        if (IncentexGlobal.CurrentMember.ClimateSettingId.HasValue)
            objList = objList.Where(t => t.ClimateSettingId.HasValue ? t.ClimateSettingId == IncentexGlobal.CurrentMember.ClimateSettingId.Value : true).ToList();
        else
            objList = objList.Where(t => t.ClimateSettingId.HasValue ? false : true).ToList();

        #endregion

        //check here compay admin then no filter check by employeetype
        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            objList = SortListingOrderByGender(objList);
        else
        {
            if (employeetypeid == 0)
                objList = SortListingOrderByGender(objList.Where(s => s.EmployeeTypeid == 0 || s.EmployeeTypeid == null).ToList());
            else
                objList = SortListingOrderByGender(objList.Where(s => s.EmployeeTypeid == 0 || s.EmployeeTypeid == null || s.EmployeeTypeid == employeetypeid).ToList());
        }

        objList = objList.OrderBy(o => o.Priority).ThenBy(o => o.ProductStyleID).ToList();

        var objProducts = (from l in objList
                           select new { l.StoreProductID, l.MasterItemNo, l.SubCategoryID, l.ProductDescrption, l.Summary }
                           ).Distinct().ToList();

        List<StoreProductLocal> objProductList = new List<StoreProductLocal>();

        foreach (var p in objProducts)
            objProductList.Add(new StoreProductLocal() { MasterItemNo = p.MasterItemNo, StoreProductID = p.StoreProductID, PSubCateID = p.SubCategoryID, ProductDescription = p.ProductDescrption, Summary = p.Summary });

        lstProductList.DataSource = objProductList;
    }

    //Sorting product list based on Gender who is logged in and viewing
    public List<SP_GetStoreProductBySubCatIDResult> SortListingOrderByGender(List<SP_GetStoreProductBySubCatIDResult> objProductList)
    {
        List<SP_GetStoreProductBySubCatIDResult> malelist = new List<SP_GetStoreProductBySubCatIDResult>();
        List<SP_GetStoreProductBySubCatIDResult> femalelist = new List<SP_GetStoreProductBySubCatIDResult>();
        List<SP_GetStoreProductBySubCatIDResult> unisexlist = new List<SP_GetStoreProductBySubCatIDResult>();
        CompanyEmployee obj = new CompanyEmployeeRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        INC_Lookup objLookup = new LookupRepository().GetById(obj.GenderID);

        foreach (SP_GetStoreProductBySubCatIDResult a in objProductList)
        {
            if (a.GarmentTypeName == "Male")
                malelist.Add(a);
            else if (a.GarmentTypeName == "Female")
                femalelist.Add(a);
            else
                unisexlist.Add(a);
        }

        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
        {
            if (objLookup.sLookupName == "Male")
                malelist.AddRange(femalelist);
            else
                femalelist.AddRange(malelist);
        }

        if (objLookup.sLookupName == "Male")
            malelist.AddRange(unisexlist);
        else
            femalelist.AddRange(unisexlist);

        if (objLookup.sLookupName == "Male")
            return malelist;
        else
            return femalelist;
    }

    protected void lstProductList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            StoreProductLocal obj = e.Item.DataItem as StoreProductLocal;

            if (obj.MasterItemNo != null && obj.StoreProductID != null)
            {
                List<StoreProductImage> objImageList = objStoreProductImageRepository.GetStoreProductImagesdById((Int32)obj.StoreProductID, (Int32)obj.MasterItemNo);

                HtmlAnchor lnkImage = (HtmlAnchor)e.Item.FindControl("lnkImage");
                HiddenField hndStoreProductImageId = (HiddenField)e.Item.FindControl("hndStoreProductImageId");
                HiddenField hdnMasteItem = (HiddenField)e.Item.FindControl("hdnMasteItem");
                HiddenField hdnPSubCateID = (HiddenField)e.Item.FindControl("hdnPSubCateID");

                lnkImage.HRef = "ProductDetail.aspx?MasterItemNo=" + hdnMasteItem.Value.ToString() + "&SubCat=" + hdnPSubCateID.Value.ToString() + "&StoreProductId=" + hndStoreProductImageId.Value.ToString();

                HtmlImage imgProduct = (HtmlImage)e.Item.FindControl("imgProduct");
                if (objImageList.Count > 0 && !String.IsNullOrEmpty(objImageList[0].ProductImage))
                    imgProduct.Src = "../UploadedImages/ProductImages/Thumbs/" + objImageList[0].ProductImage;
                else
                    imgProduct.Src = "../UploadedImages/ProductImages/ProductDefault.jpg";
            }
        }
    }

    #endregion

    class StoreProductLocal
    {
        public Int64? StoreProductID { get; set; }

        public Int64? MasterItemNo { get; set; }
        public Decimal? Level1 { get; set; }
        public String ProductDescription { get; set; }
        public String Summary { get; set; }
        public Int64? ProductItemID { get; set; }
        public Int64 PSubCateID { get; set; }
    }
}