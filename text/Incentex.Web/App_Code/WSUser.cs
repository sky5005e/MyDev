using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Data;
using Newtonsoft.Json.Linq;
using System.IO;


using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;





/// <summary>
/// Summary description for WSUser
/// </summary>
[WebService(Namespace = "https://www.world-link.us.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WSUser : System.Web.Services.WebService
{

    public WSUser()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    #region Check User Session
    /// <summary>
    /// This is only to check session is exist or not from ajax call
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public Boolean IsUserSessionExist()
    {
        if (IncentexGlobal.CurrentMember == null)
            return false;
        else
            return true;
    }

    /// <summary>
    /// This is only to check session is exist or not from ajax call
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String SystemExit()
    {
        String msg = String.Empty;
        try
        {
            // for tracking center logout time in table(UserTracking)
            if (IncentexGlobal.CurrentMember != null)
            {
                UserTrackingRepo objTrackinRepo = new UserTrackingRepo();
                UserTracking objTrackinTbl = new UserTracking();
                DateTime LogoutTime = Convert.ToDateTime(System.DateTime.Now.TimeOfDay.ToString());

                objTrackinRepo.SetLogout(Convert.ToInt32(Session["trackID"]), LogoutTime);
            }

            IncentexGlobal.CurrentMember = null;
            IncentexGlobal.GSEMgtCurrentMember = null;
            IncentexGlobal.IsIEFromStore = false;
            IncentexGlobal.IsIEFromStoreTestMode = false;
            Session.RemoveAll();
            msg = "success";
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
            msg = "Error";
        }
        return msg;
    }
    #endregion

    #region User Index page's
    /// <summary>
    /// Get All Notification Details
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public HCItemsResult GetNotificationDetails()
    {
        return HeaderItem.GetAllNotifications(IncentexGlobal.CurrentMember.UserInfoID, Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
    }

    /// <summary>
    /// GetAllCartItems for which is not order
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public HCItemsResult GetAllCartItems()
    {
        return HeaderItem.GetPendingCartItems(IncentexGlobal.CurrentMember.UserInfoID);
    }
    /// <summary>
    /// Return Help Vidoe URL
    /// </summary>
    /// <param name="VideoType"></param>
    /// <param name="ModuleName"></param>
    /// <param name="PageOrPopupTitle"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String GetHelpVideoOrDoc(String Type, String ModuleName, String PageOrPopupTitle)
    {
        MediaRepository objmedia = new MediaRepository();
        String strVideoUrl = String.Empty;
        Int64? Userid = null;
        Int64? Companyid=null;

        if (Type != "login pop-up" && PageOrPopupTitle != "Become a Member")
        {
            Userid = IncentexGlobal.CurrentMember.UserInfoID;
            Companyid = IncentexGlobal.CurrentMember.CompanyId;
        }
        
        GetMediaHelpVideoOrDocResult ObjPageVideo = objmedia.GetMediaHelpVideoOrDoc(Type, ModuleName, PageOrPopupTitle, Userid, Companyid);
        if (ObjPageVideo != null)
        {
            if (Type.ToLower() == "help video" || Type.ToLower() == "login pop-up")
                strVideoUrl = ObjPageVideo.Vimeourl;
            else
                strVideoUrl = ObjPageVideo.OriginalFileName;

            MediaFile objfile = objmedia.GetFileById(ObjPageVideo.mediafileid);
            if (objfile != null)
            {
                objfile.View = objfile.View == null ? 1 : objfile.View + 1;
                objmedia.SubmitChanges();
            }
        }
       // else// For Testing : if there will be no vidoe then use below Vimeo Link URL
         //   strVideoUrl = "http://vimeo.com/user19227861/review/72414822/f9ce776c88";
        return strVideoUrl;
    }


    /// <summary>
    /// Return Help Vidoe URL.. 
    /// </summary>
    /// <param name="VideoType"></param>
    /// <param name="ModuleName"></param>
    /// <param name="PageOrPopupTitle"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String GetHelpVideoOrDocLink(String Type, String ModuleName, String PageOrPopupTitle)
    {
        MediaRepository objmedia = new MediaRepository();
        String strVideoUrl = String.Empty;
        Int64? Userid = null;
        Int64? Companyid = null;

        if (Type != "login pop-up" && PageOrPopupTitle != "Become a Member")
        {
            Userid = IncentexGlobal.CurrentMember.UserInfoID;
            Companyid = IncentexGlobal.CurrentMember.CompanyId;
        }

        GetMediaHelpVideoOrDocResult ObjPageVideo = objmedia.GetMediaHelpVideoOrDoc(Type, ModuleName, PageOrPopupTitle, Userid, Companyid);
        if (ObjPageVideo != null)
        {
            if (Type.ToLower() == "help video" || Type.ToLower() == "login pop-up")
                strVideoUrl = ObjPageVideo.Vimeourl;
            else
                strVideoUrl = ObjPageVideo.OriginalFileName;

           
        }
        // else// For Testing : if there will be no vidoe then use below Vimeo Link URL
        //   strVideoUrl = "http://vimeo.com/user19227861/review/72414822/f9ce776c88";
        return strVideoUrl;
    }




    /// <summary>
    /// Return Master Item Detail
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public List<string> GetMasterItemDetail(string prefixText)
    {
        LookupRepository ObjRepos=new LookupRepository();
        var result = ObjRepos.GetByLookupCode(DAEnums.LookupCodeType.MasterItemNumber).Where(s => s.sLookupName.StartsWith(prefixText)).OrderBy(x=>x.sLookupName).Select(x=>x.sLookupName).ToList();
        return result;
        // LookupBE sLookupBE = new LookupBE();
    }


    /// <summary>
    /// Return Master Item Detail
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public List<string> GetMediaSearchKeywords(string prefixText)
    {
        MediaRepository objmedia = new MediaRepository();
       var result = objmedia.MediaSearch().Where(x=>x.items.ToLower().StartsWith(prefixText.ToLower())).Select(x=>x.items).ToList();
       return result;
    }


    /// <summary>
    /// Currency Conversion
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="fromCurrency"></param>
    /// <param name="toCurrency"></param>
    /// <returns></returns>
    [WebMethod]
    public String CurrencyConversion(Decimal amount, String fromCurrency, String toCurrency)
    {
        String NetAmount = String.Empty;
        try
        {
            CountryRepository objCX = new CountryRepository();
            CountryCurrency objCCurrency = objCX.CountryCurrencySymbol(toCurrency);
            WebClient web = new WebClient();
            String url = string.Format("http://rate-exchange.appspot.com/currency?from={0}&to={1}&q={2}", fromCurrency, toCurrency, amount);
            //string url = string.Format("http://www.google.com/ig/calculator?hl=en&q={0}{1}=?{2}", amount, fromCurrency, toCurrency);// for google
            string response = web.DownloadString(url);
            JObject objjson = JObject.Parse(response);
            // {"to": "INR", "rate": 63.048200000000001, "from": "USD", "v": 28371.690000000002}
            // for google
            //MatchCollection matches = Regex.Matches(objjson["v"], @"[+-]?\d+(\.\d+)?");// For google
            // result {lhs: "1 U.S. dollar",rhs: "56.679703 Indian rupees",error: "",icc: true}// for google
            //MatchCollection matches = Regex.Matches((String)objjson["rhs"], @"[+-]?\d+(\.\d+)?");// For google
            String Rate = objjson["v"].ToString();
            //foreach (Match m in matches)
            //    Rate += m.Value;

            Decimal TotalAmount = Convert.ToDecimal(Rate);
            NetAmount = objCCurrency.CountryCurrencySymbol + " " + TotalAmount.ToString("0.00") + " " + objCCurrency.CountryCurrencyName;

        }
        catch (Exception ex)
        {
            NetAmount = "oops error occur";
            ErrHandler.WriteError(ex, false);
        }
        return NetAmount;
    }


    #region RSS Feed
    /// <summary>
    /// Get RSS Feed
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public HCItemsResult GetRSSFeeds(String index)
    {
        return HeaderItem.GetRSSFeedDetails(Convert.ToInt32(index));
    }


    /// <summary>
    /// Get RSS Feed
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public HCItemsResult GetRSSFeedsOnPageLoad()
    {
        return HeaderItem.GetRSSFeedDetailsOnPageLoad();
    }
    #endregion

    /// <summary>
    /// Get Product Details by Sold by selection
    /// </summary>
    /// <param name="StoreProductID"></param>
    /// <param name="ItemSizeID"></param>
    /// <returns></returns>
    [WebMethod()]
    public List<GetSoldByDetailsResult> GetSoldBy(String StoreProductID, String ItemSizeID)
    {
        ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
        List<GetSoldByDetailsResult> objList = new List<GetSoldByDetailsResult>();
        try
        {
            objList = objRepository.GetSoldByDetails(Convert.ToInt64(StoreProductID), Convert.ToInt64(ItemSizeID));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
        }
        return objList;
    }

    /// <summary>
    /// To get products details by storeproductID
    /// </summary>
    /// <param name="StoreProductID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public List<GetProductItemDetailsByStoreProductIDResult> GetProductDetails(String StoreProductID)
    {
        ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
        List<GetProductItemDetailsByStoreProductIDResult> objList = new List<GetProductItemDetailsByStoreProductIDResult>();
        try
        {
            objList = objRepository.GetProductItemDetailsByStoreProductID(Convert.ToInt64(StoreProductID), IncentexGlobal.CurrentMember.UserInfoID).ToList();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
        }
        return objList;
    }

    /// <summary>
    /// Get items details by StoreProductID and ItemsizeID
    /// </summary>
    /// <param name="StoreProductID"></param>
    /// <param name="itemSizeID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public GetProductItemDetailsByStoreProductIDResult GetProductItemDetails(String StoreProductID, String itemSizeID, String soldBy)
    {
        ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
        GetProductItemDetailsByStoreProductIDResult obj = new GetProductItemDetailsByStoreProductIDResult();
        try
        {
            List<GetProductItemDetailsByStoreProductIDResult> objList = objRepository.GetProductItemDetailsByStoreProductID(Convert.ToInt64(StoreProductID), IncentexGlobal.CurrentMember.UserInfoID).ToList();
            if (objList.Count > 0)
            {
                if (!String.IsNullOrEmpty(soldBy))
                    obj = objList.Where(q => q.ItemSizeID == Convert.ToInt64(itemSizeID) && q.Soldby == Convert.ToInt64(soldBy)).FirstOrDefault();
                else
                    obj = objList.Where(q => q.ItemSizeID == Convert.ToInt64(itemSizeID)).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
        }
        return obj;
    }

    /// <summary>
    /// Add to cart
    /// </summary>
    /// <param name="StoreProductID"></param>
    /// <param name="itemSizeID"></param>
    /// <param name="soldBy"></param>
    /// <param name="Quantity"></param>
    /// <param name="Price"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public Int64 AddProductToCart(String StoreProductID, String itemSizeID, String soldBy, String Quantity, String Price)
    {
        Int64 shoppingCartID = 0;
        try
        {
            ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
            GetProductItemDetailsByStoreProductIDResult objItemToAdd = new GetProductItemDetailsByStoreProductIDResult();
            MyShoppingCartRepository shoppingRepo = new MyShoppingCartRepository();
            List<GetProductItemDetailsByStoreProductIDResult> objList = objRepository.GetProductItemDetailsByStoreProductID(Convert.ToInt64(StoreProductID), IncentexGlobal.CurrentMember.UserInfoID).ToList();
            if (objList.Count > 0)
            {
                if (!String.IsNullOrEmpty(soldBy))
                    objItemToAdd = objList.Where(q => q.ItemSizeID == Convert.ToInt64(itemSizeID) && q.Soldby == Convert.ToInt64(soldBy)).FirstOrDefault();
                else
                    objItemToAdd = objList.Where(q => q.ItemSizeID == Convert.ToInt64(itemSizeID)).FirstOrDefault();
            }
            if (objItemToAdd != null)
            {
                MyShoppinCart objShoppingCart = new MyShoppinCart();

                objShoppingCart.CategoryID = Convert.ToInt64(objItemToAdd.CategoryID);
                objShoppingCart.CompanyID = Convert.ToInt64(objItemToAdd.CompanyID);
                objShoppingCart.ItemNumber = Convert.ToString(objItemToAdd.ItemNumber);
                objShoppingCart.ProductDescrption = Convert.ToString(objItemToAdd.ProductDescrption);
                objShoppingCart.Quantity = Convert.ToString(Quantity);
                objShoppingCart.Size = Convert.ToString(objItemToAdd.ItemSize);
                objShoppingCart.StoreID = Convert.ToInt64(objItemToAdd.StoreID);
                objShoppingCart.StoreProductID = Convert.ToInt64(objItemToAdd.StoreProductID);
                objShoppingCart.SubCategoryID = Convert.ToInt64(objItemToAdd.SubCategoryID);
                //objShoppingCart.TailoringLength = txtDesiredLength.Text;
                objShoppingCart.UnitPrice = Convert.ToString(Price);
                Int32 PriceLevel = 0;
                if ((Boolean)objItemToAdd.IsCloseOut)
                    PriceLevel = 5;
                else if (objItemToAdd.Level3PricingStatus != null && objItemToAdd.Level3PricingStatus == 135 && DateTime.Now.Date <= objItemToAdd.Level3PricingEndDate && DateTime.Now.Date >= objItemToAdd.Level3PricingStartDate)
                    PriceLevel = 3;
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                    PriceLevel = 1;
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                    PriceLevel = 2;
                else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
                    PriceLevel = 4;

                objShoppingCart.PriceLevel = PriceLevel;
                // START MOAS
                objShoppingCart.MOASPriceLevel = PriceLevel;
                objShoppingCart.MOASUnitPrice = Convert.ToString(Price);
                // END MOAS
                objShoppingCart.ProductItemID = Convert.ToInt64(objItemToAdd.ProductItemID);
                objShoppingCart.WorkgroupID = Convert.ToInt64(objItemToAdd.WorkgroupID);
                objShoppingCart.Inventory = Convert.ToString(objItemToAdd.Inventory);
                objShoppingCart.ProductImageID = Convert.ToString(objItemToAdd.StoreProductImageID);
                objShoppingCart.MasterItemNo = Convert.ToInt64(objItemToAdd.MasterItemNo);
                objShoppingCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                //ItemSizeID and ItemSoldbyId item ItemColorID
                objShoppingCart.ItemSizeID = Convert.ToInt64(objItemToAdd.ItemSizeID);
                objShoppingCart.ItemColorID = Convert.ToInt64(objItemToAdd.ItemColorID);
                objShoppingCart.ItemSoldByID = Convert.ToInt64(objItemToAdd.Soldby);
                objShoppingCart.SupplierID = Convert.ToInt64(objItemToAdd.SupplierId);
                //// If Coupa User
                //if (!String.IsNullOrEmpty(CoupaID) && !String.IsNullOrEmpty(BuyerCookie))
                //{
                //    objShoppingCart.BuyerCookie = Convert.ToString(BuyerCookie);
                //    objShoppingCart.IsCoupaOrder = true;
                //    objShoppingCart.IsCoupaOrderSubmitted = false;
                //}
                // Add Created Date on MyShopping Cart Table
                objShoppingCart.CreatedDate = DateTime.Now;
                // Added SoldbyName field in MyShoppingCart Table.
                objShoppingCart.SoldbyName = Convert.ToString(objItemToAdd.SoldbyName);
                objShoppingCart.IsOrdered = false;
                shoppingRepo.Insert(objShoppingCart);
                shoppingRepo.SubmitChanges();
                shoppingCartID = objShoppingCart.MyShoppingCartID;
            }
        }
        catch (Exception ex)
        {
            shoppingCartID = 0;
            ErrHandler.WriteError(ex);
        }
        return shoppingCartID;
    }
    /// <summary>
    /// Update Cart items by MyShoppingCartID
    /// </summary>
    /// <param name="StoreProductID"></param>
    /// <param name="itemSizeID"></param>
    /// <param name="soldBy"></param>
    /// <param name="Quantity"></param>
    /// <param name="Price"></param>
    /// <param name="MyShoppingCartID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String UpdateProductToCart(String StoreProductID, String itemSizeID, String soldBy, String Quantity, String Price, String MyShoppingCartID)
    {
        String msg = String.Empty;
        try
        {
            ProductItemDetailsRepository objRepository = new ProductItemDetailsRepository();
            GetProductItemDetailsByStoreProductIDResult objItemToAdd = new GetProductItemDetailsByStoreProductIDResult();
            MyShoppingCartRepository shoppingRepo = new MyShoppingCartRepository();
            List<GetProductItemDetailsByStoreProductIDResult> objList = objRepository.GetProductItemDetailsByStoreProductID(Convert.ToInt64(StoreProductID), IncentexGlobal.CurrentMember.UserInfoID).ToList();
            if (objList.Count > 0)
            {
                if (!String.IsNullOrEmpty(soldBy))
                    objItemToAdd = objList.Where(q => q.ItemSizeID == Convert.ToInt64(itemSizeID) && q.Soldby == Convert.ToInt64(soldBy)).FirstOrDefault();
                else
                    objItemToAdd = objList.Where(q => q.ItemSizeID == Convert.ToInt64(itemSizeID)).FirstOrDefault();
            }
            if (objItemToAdd != null)
            {
                MyShoppinCart objShoppingCart = new MyShoppinCart();
                if (!String.IsNullOrEmpty(MyShoppingCartID))
                    objShoppingCart = shoppingRepo.GetById(Convert.ToInt32(MyShoppingCartID), IncentexGlobal.CurrentMember.UserInfoID);
                if (objShoppingCart != null)
                {
                    objShoppingCart.CategoryID = Convert.ToInt64(objItemToAdd.CategoryID);
                    objShoppingCart.CompanyID = Convert.ToInt64(objItemToAdd.CompanyID);
                    objShoppingCart.ItemNumber = Convert.ToString(objItemToAdd.ItemNumber);
                    objShoppingCart.ProductDescrption = Convert.ToString(objItemToAdd.ProductDescrption);
                    objShoppingCart.Quantity = Convert.ToString(Quantity);
                    objShoppingCart.Size = Convert.ToString(objItemToAdd.ItemSize);
                    objShoppingCart.StoreID = Convert.ToInt64(objItemToAdd.StoreID);
                    objShoppingCart.StoreProductID = Convert.ToInt64(objItemToAdd.StoreProductID);
                    objShoppingCart.SubCategoryID = Convert.ToInt64(objItemToAdd.SubCategoryID);
                    //objShoppingCart.TailoringLength = txtDesiredLength.Text;
                    objShoppingCart.UnitPrice = Convert.ToString(Price);
                    Int32 PriceLevel = 0;
                    if ((Boolean)objItemToAdd.IsCloseOut)
                        PriceLevel = 5;
                    else if (objItemToAdd.Level3PricingStatus != null && objItemToAdd.Level3PricingStatus == 135 && DateTime.Now.Date <= objItemToAdd.Level3PricingEndDate && DateTime.Now.Date >= objItemToAdd.Level3PricingStartDate)
                        PriceLevel = 3;
                    else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                        PriceLevel = 1;
                    else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                        PriceLevel = 2;
                    else if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
                        PriceLevel = 4;

                    objShoppingCart.PriceLevel = PriceLevel;
                    // START MOAS
                    objShoppingCart.MOASPriceLevel = PriceLevel;
                    objShoppingCart.MOASUnitPrice = Convert.ToString(Price);
                    // END MOAS

                    objShoppingCart.ProductItemID = Convert.ToInt64(objItemToAdd.ProductItemID);
                    objShoppingCart.WorkgroupID = Convert.ToInt64(objItemToAdd.WorkgroupID);
                    objShoppingCart.Inventory = Convert.ToString(objItemToAdd.Inventory);
                    objShoppingCart.ProductImageID = Convert.ToString(objItemToAdd.StoreProductImageID);
                    objShoppingCart.MasterItemNo = Convert.ToInt64(objItemToAdd.MasterItemNo);
                    objShoppingCart.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;
                    //ItemSizeID and ItemSoldbyId item ItemColorID
                    objShoppingCart.ItemSizeID = Convert.ToInt64(objItemToAdd.ItemSizeID);
                    objShoppingCart.ItemColorID = Convert.ToInt64(objItemToAdd.ItemColorID);
                    objShoppingCart.ItemSoldByID = Convert.ToInt64(objItemToAdd.Soldby);
                    //// If Coupa User
                    //if (!String.IsNullOrEmpty(CoupaID) && !String.IsNullOrEmpty(BuyerCookie))
                    //{
                    //    objShoppingCart.BuyerCookie = Convert.ToString(BuyerCookie);
                    //    objShoppingCart.IsCoupaOrder = true;
                    //    objShoppingCart.IsCoupaOrderSubmitted = false;
                    //}
                    // Add Created Date on MyShopping Cart Table
                    objShoppingCart.CreatedDate = DateTime.Now;
                    // Added SoldbyName field in MyShoppingCart Table.
                    objShoppingCart.SoldbyName = Convert.ToString(objItemToAdd.SoldbyName);
                    objShoppingCart.IsOrdered = false;
                    shoppingRepo.SubmitChanges();
                }
                msg = "Item updated to cart";
            }
        }
        catch (Exception ex)
        {
            msg = "oops error occur";
            ErrHandler.WriteError(ex);
        }
        return msg;
    }

    /// <summary>
    /// Remove Items from Cart
    /// </summary>
    /// <param name="StoreProductID"></param>
    /// <param name="ProductItemID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String RemoveItemFromCart(String MyShoppingCartID)
    {
        String msg = String.Empty;
        try
        {
            MyShoppingCartRepository shoppingRepo = new MyShoppingCartRepository();
            List<MyShoppinCart> objList = shoppingRepo.GetPendingCartItemsList(IncentexGlobal.CurrentMember.UserInfoID);
            // For Single Item Remove 
            // Here it will return always single cart item list
            if (!String.IsNullOrEmpty(MyShoppingCartID))
                objList = objList.Where(q => q.MyShoppingCartID == Convert.ToInt64(MyShoppingCartID)).ToList();

            shoppingRepo.DeleteAll(objList);
            shoppingRepo.SubmitChanges();
            msg = "Item successfully deleted from cart";

        }
        catch (Exception ex)
        {
            msg = "oops error occur";
            ErrHandler.WriteError(ex);
        }
        return msg;
    }

    #endregion

    #region Header - Notification's
    #region Pending User's
    /// <summary>
    /// Get Pending User By RegistrationID
    /// </summary>
    /// <param name="RegisID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public GetPendingUsersByRegistrationIDResult PopolateUserInfo(String RegisID)
    {
        GetPendingUsersByRegistrationIDResult objUser = new GetPendingUsersByRegistrationIDResult();
        try
        {
            objUser = new RegistrationRepository().GetPendingUserByRegistrationID(Convert.ToInt64(RegisID));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
        }
        return objUser;

    }
    /// <summary>
    /// To Approve User's
    /// </summary>
    /// <param name="RegistrationID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String ApproveUser(String RegistrationID)
    {
        String msg = String.Empty;
        try
        {
            UserInformationRepository objUserRepo = new UserInformationRepository();
            RegistrationRepository objRegisRepo = new RegistrationRepository();
            Inc_Registration objRegis = objRegisRepo.GetByRegistrationID(Convert.ToInt64(RegistrationID));

            if (objRegis != null && objUserRepo.CheckEmailExistence(objRegis.sEmailAddress, 0))
            {
                objRegisRepo.ApprovePendingUser(objRegis.iRegistraionID, IncentexGlobal.CurrentMember.UserInfoID,false);
                SendUserApprovalEmail(objRegis.sEmailAddress, objRegis.Password);
                msg = "User Approved";
            }
            else
            {
                msg = "User Already Exist";
            }
        }
        catch (Exception ex)
        {
            msg = "oops error occur";
            ErrHandler.WriteError(ex);
        }
        return msg;
    }

    /// <summary>
    /// Check for user Email Existance in System
    /// </summary>
    /// <param name="RegistrationID"></param>
    /// <returns></returns>
    [WebMethod]
    public String IsEmailExist(String Email)
    {
        String msg = String.Empty;
        try
        {
            if (new UserInformationRepository().CheckEmailExistence(Email, 0))
            {
                msg = "Email already exist";
            }
        }
        catch (Exception ex)
        {
            msg = "oops error occur";
            ErrHandler.WriteError(ex);
        }
        return msg;
    }
    /// <summary>
    /// To Reject Pending User
    /// </summary>
    /// <param name="RegistrationID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String RejectUser(String RegistrationID)
    {
        String msg = String.Empty;
        try
        {
            RegistrationRepository objRepo = new RegistrationRepository();
            Inc_Registration objRegis = objRepo.GetByRegistrationID(Convert.ToInt64(RegistrationID));
            if (objRegis != null)
            {
                objRegis.status = "rejected";
                objRegis.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;
                String emailAddress = objRegis.sEmailAddress;
                String _userName = objRegis.sFirstName + " " + objRegis.sLastName;
                objRepo.SubmitChanges();
                SendUserRejectedEmail(emailAddress, _userName);
                msg = "User Rejected";
            }
        }
        catch (Exception ex)
        {
            msg = "oops error occur";
            ErrHandler.WriteError(ex);
        }
        return msg;
    }
    /// <summary>
    /// Send User Approval Email
    /// </summary>
    /// <param name="EmailAddress"></param>
    /// <param name="password"></param>
    private void SendUserApprovalEmail(String EmailAddress, String password)
    {
        try
        {
            UserInformation objUserInformation = new UserInformationRepository().GetByLoginEmail(EmailAddress);

            //For Send mail
            INC_EmailTemplate objEmail = new EmailRepository().GetEmailTemplatesByName("NewEmployeeActivation");
            //Get Email Content
            if (objEmail != null)
            {
                String sFrmadd = objEmail.sFromAddress;
                String sToadd = EmailAddress;
                String sSubject = objEmail.sSubject;
                String sFrmname = objEmail.sFromName;
                StringBuilder messagebody = new StringBuilder(objEmail.sTemplateContent); // From Template table

                ManageEmailRepository objManageEmailRepo = new ManageEmailRepository();//Email Authorization
                messagebody.Replace("{fullname}", objUserInformation.FirstName + " " + objUserInformation.LastName);
                messagebody.Replace("{password}", password);
                messagebody.Replace("{email}", objUserInformation.LoginEmail);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                Company objComplist = new CompanyRepository().GetById(Convert.ToInt64(objUserInformation.CompanyId));
                if (objComplist != null)
                {
                    messagebody.Replace("{CompanyName}", objComplist.CompanyName);
                    messagebody.Replace("{Date}", DateTime.Now.ToShortDateString());
                }

                String smtphost = CommonMails.SMTPHost;
                Int32 smtpport = CommonMails.SMTPPort;
                //Email Management
                if (objManageEmailRepo.CheckEmailAuthentication(objUserInformation.UserInfoID, Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.ApprovedUsers)) == true)
                {
                    //Local testing email settings
                    if (HttpContext.Current.Request.IsLocal)
                        new CommonMails().SendEmail4Local(1092, "Employee Approved", "surendar.yadav@indianic.com", sSubject, messagebody.ToString(), "incentextest6@gmail.com", "test6incentex", true);
                    else
                        new CommonMails().SendMail(objUserInformation.UserInfoID, "Employee Approved", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
                }
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Send User Rejected Email
    /// </summary>
    /// <param name="EmailAddress"></param>
    /// <param name="password"></param>
    private void SendUserRejectedEmail(String EmailAddress, String UserName)
    {
        try
        {
            //For Send mail
            INC_EmailTemplate objEmail = new EmailRepository().GetEmailTemplatesByName("NewEmployeeRejection");
            //Get Email Content
            if (objEmail != null)
            {
                String sFrmadd = objEmail.sFromAddress;
                String sToadd = EmailAddress;
                String sSubject = objEmail.sSubject;
                String sFrmname = objEmail.sFromName;
                StringBuilder messagebody = new StringBuilder(objEmail.sTemplateContent); // From Template table
                messagebody.Replace("{fullname}", UserName);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                Company objComplist = new CompanyRepository().GetById(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
                if (objComplist != null)
                {
                    messagebody.Replace("{ApproverCompany}", objComplist.CompanyName);
                    messagebody.Replace("{ApproverName}", Convert.ToString(IncentexGlobal.CurrentMember.FirstName + " " + IncentexGlobal.CurrentMember.LastName));
                }

                String smtphost = CommonMails.SMTPHost;
                Int32 smtpport = CommonMails.SMTPPort;
                //Local testing email settings
                if (HttpContext.Current.Request.IsLocal)
                    new CommonMails().SendEmail4Local(1092, "Employee Rejected", "surendar.yadav@indianic.com", sSubject, messagebody.ToString(), "incentextest6@gmail.com", "test6incentex", true);
                else
                    new CommonMails().SendMail(0, "Employee Rejected", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);

            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion

    #region Pending Order's
    /// <summary>
    /// To Approve Order's
    /// </summary>
    /// <param name="OrderID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String ApproveOrder(String OrderID)
    {
        String msg = String.Empty;
        try
        {
          String userName =  new OrderApproval().ApproveOrder(Convert.ToInt64(OrderID), IncentexGlobal.CurrentMember.UserInfoID);
          msg = userName + "'s order has been approved for processing.";// In this text – the persons last name is to be followed by ‘s
           // msg = "Order Approved";
        }
        catch (Exception ex)
        {
            msg = "oops error occur";
            ErrHandler.WriteError(ex);
        }
        return msg;
    }
    /// <summary>
    /// To Reject Order's
    /// </summary>
    /// <param name="OrderID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String RejectOrder(String OrderID, String ReasonMsg)
    {
        String msg = String.Empty;
        try
        {

            UserInformationRepository objUserRepo = new UserInformationRepository();
            OrderConfirmationRepository objRepos = new OrderConfirmationRepository();
            Order objCancelOrder = objRepos.GetByOrderID(Convert.ToInt64(OrderID));
            UserInformation objCancelUser = objUserRepo.GetById(Convert.ToInt64(objCancelOrder.UserId));
            MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
            objCancelOrder.OrderStatus = "Canceled";
            objCancelOrder.UpdatedDate = DateTime.Now;
            objRepos.SubmitChanges();

            //start changes added by mayur for maintain history of manager cancel order on 6-feb-2012
            OrderMOASSystemRepository objOrderMOASSystemRepository = new OrderMOASSystemRepository();
            OrderMOASSystem objOrderMOASSystem = objOrderMOASSystemRepository.GetByOrderIDAndManagerUserInfoID(objCancelOrder.OrderID, IncentexGlobal.CurrentMember.UserInfoID);
            objOrderMOASSystem.Status = "Canceled";
            objOrderMOASSystem.DateAffected = DateTime.Now;
            objOrderMOASSystemRepository.SubmitChanges();
            //end changes added by mayur for maintain history of manager cancel order on 6-feb-2012

            new OrderApproval().AddNoteHistory("Canceled", objCancelOrder.OrderID, IncentexGlobal.CurrentMember.UserInfoID, ReasonMsg);

            #region Give back the credit amount he has used for this order
            if (objCancelOrder.CreditUsed != null)
            {
                CompanyEmployeeRepository objCmnyEmp = new CompanyEmployeeRepository();
                CompanyEmployee cmpEmpl = objCmnyEmp.GetByUserInfoId((Int64)objCancelOrder.UserId);
                if (objCancelOrder.CreditUsed == "Previous")
                {
                    cmpEmpl.StratingCreditAmount = cmpEmpl.StratingCreditAmount + objCancelOrder.CreditAmt;
                    objCmnyEmp.SubmitChanges();
                }
                else if (objCancelOrder.CreditUsed == "Anniversary")
                {
                    cmpEmpl.CreditAmtToApplied = cmpEmpl.CreditAmtToApplied + objCancelOrder.CreditAmt;
                    objCmnyEmp.SubmitChanges();
                }
                else
                {

                }
            }
            #endregion

            #region update inventory and transfer order to shopping cart
            List<Order> obj = objRepos.GetShoppingCartId(objCancelOrder.OrderNumber);
            if (obj.Count > 0)
            {
                foreach (var cartID in obj[0].MyShoppingCartID.ToString().Split(','))
                {
                    if (objCancelOrder.OrderFor == "ShoppingCart")
                    {
                        //Shopping cart
                        MyShoppinCart objShoppingcart = new MyShoppinCart();
                        ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                        ProductItem objProductItem = new ProductItem();
                        objShoppingcart = objShoppingCartRepository.GetById(Convert.ToInt32(cartID), (Int64)objCancelOrder.UserId);
                        if (objShoppingcart != null)
                        {
                            objProductItem = objProItemRepos.GetRecord(Convert.ToInt64(objShoppingcart.StoreProductID), Convert.ToInt64(objShoppingcart.MasterItemNo), objShoppingcart.ItemNumber);
                            //Update Inventory Here 
                            //Call here upDate Procedure
                            String strProcess = "Shopping";
                            String strMessage = objRepos.IncreaseDescreaseInventory(objProductItem.ProductItemID, -Convert.ToInt32(objShoppingcart.Quantity), strProcess);
                        }

                    }
                    else
                    {
                        //Issuance
                        MyIssuanceCartRepository objIssuanceRepos = new MyIssuanceCartRepository();
                        MyIssuanceCart objIssuance = new MyIssuanceCart();
                        ProductItemDetailsRepository objProItemRepos = new ProductItemDetailsRepository();
                        //End 

                        objIssuance = objIssuanceRepos.GetById(Convert.ToInt32(cartID), (Int64)objCancelOrder.UserId);
                        if (objIssuance != null)
                        {
                            List<SelectProductIDResult> objList = new List<SelectProductIDResult>();
                            CompanyEmployeeRepository objcmpemprepo = new CompanyEmployeeRepository();
                            AnniversaryProgramRepository objacprepo = new AnniversaryProgramRepository();
                            Int64 storeid = objacprepo.GetEmpStoreId((Int64)objCancelOrder.UserId, objcmpemprepo.GetByUserInfoId((Int64)objCancelOrder.UserId).WorkgroupID).StoreID;
                            objList = objProItemRepos.GetProductId(objIssuance.MasterItemID, Convert.ToInt64(cartID), Convert.ToInt64(objIssuance.ItemSizeID), Convert.ToInt64(storeid));
                            //Update Inventory Here 
                            //Call here upDate Procedure
                            for (Int32 i = 0; i < objList.Count; i++)
                            {
                                String strProcess = "UniformIssuance";
                                String strMessage = objRepos.IncreaseDescreaseInventory(Convert.ToInt64(objList[i].ProductItemID), -Convert.ToInt32(objIssuance.Qty), strProcess);
                            }
                        }
                    }
                }
            }
            #endregion
            msg = "Order Rejected";
        }
        catch (Exception ex)
        {
            msg = "oops error occur";
            ErrHandler.WriteError(ex);
        }
        return msg;
    }


    #endregion

    #region My Message's

    /// <summary>
    /// Get All message details
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public HCItemsResult GetMessageDetails(String ServiceTicketID)
    {
        return HeaderItem.GetAllMessage(Convert.ToInt64(ServiceTicketID), "ServiceTicketCAs", null);
    }

    #endregion

    /// <summary>
    /// Message Mark as Read
    /// </summary>
    /// <param name="EquipmentMasterID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String MessageMarkRead(String ServiceTicketID)
    {
        String msg = String.Empty;
        try
        {
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            Int64 TicketID = Convert.ToInt64(ServiceTicketID);
            objSerTicRep.UpdateNoteReadFlag(TicketID, IncentexGlobal.CurrentMember.UserInfoID, objSerTicRep.GetUnreadNotes(TicketID, IncentexGlobal.CurrentMember.UserInfoID).ToList());
            msg = "Message mark as Read";
        }
        catch (Exception ex)
        {
            msg = "oops error occur";
            ErrHandler.WriteError(ex);
        }
        return msg;
    }

    /// <summary>
    /// Save Notes
    /// </summary>
    /// <param name="ServiceTicketID"></param>
    /// <param name="NoteMsg"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String SaveMessage(String ServiceTicketID, String NoteMsg)
    {
        String msg = String.Empty;
        try
        {
            Int64 TicketID = Convert.ToInt64(ServiceTicketID);
            ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
            String NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs);
            // Save notes
            objSerTicRep.InsertTicketNote(TicketID, IncentexGlobal.CurrentMember.UserInfoID, Convert.ToString(NoteMsg.Trim()), NoteFor, null);
            //Mark as read 
            objSerTicRep.UpdateNoteReadFlag(TicketID, IncentexGlobal.CurrentMember.UserInfoID, objSerTicRep.GetUnreadNotes(TicketID, IncentexGlobal.CurrentMember.UserInfoID).ToList());
            SendEmailToCECAIE(Convert.ToInt64(ServiceTicketID));

            msg = "Message Save Successfully";
        }
        catch (Exception ex)
        {
            msg = "oops error occur";
            ErrHandler.WriteError(ex);
        }
        return msg;
    }
    /// <summary>
    /// Send Email to Ticket CECAIE
    /// </summary>
    /// <param name="ServiceTicketID"></param>
    private void SendEmailToCECAIE(Int64 ServiceTicketID)
    {
        ServiceTicketRepository objSerTicRep = new ServiceTicketRepository();
        vw_ServiceTicket objServiceTicket = objSerTicRep.GetFirstByID(ServiceTicketID);

        if (objServiceTicket != null)
        {
            String eMailTemplate = String.Empty;
            String sSubject = objServiceTicket.ServiceTicketName + " - Support Ticket - " + objServiceTicket.ServiceTicketNumber;

            String sReplyToadd = Common.ReplyTo;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/ServiceTicketNote.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            String TrailingNotesIE = new ServiceTicketRepository().TrailingNotes(objServiceTicket.ServiceTicketID, 2, false, "<br/>");
            String TrailingNotes = new ServiceTicketRepository().TrailingNotes(objServiceTicket.ServiceTicketID, 1, false, "<br/>");

            List<vw_ServiceTicketNoteRecipient> lstRecipients = new List<vw_ServiceTicketNoteRecipient>();
            lstRecipients = new ServiceTicketRepository().GetNoteRecipientsByTicketID(objServiceTicket.ServiceTicketID).Where(le => le.SubscriptionFlag == true).ToList();
            foreach (vw_ServiceTicketNoteRecipient recipient in lstRecipients)
            {
                //Email Management
                if (new ManageEmailRepository().CheckEmailAuthentication(Convert.ToInt64(recipient.UserInfoID), Convert.ToInt64(Incentex.DAL.Common.DAEnums.ManageEmail.SupportTickets)) == true)
                {
                    String UserName = Convert.ToString(recipient.LastName) + " " + Convert.ToString(recipient.FirstName);
                    StringBuilder MessageBody = new StringBuilder(eMailTemplate);
                    MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                    MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
                    MessageBody.Replace("{TicketNo}", Convert.ToString(objServiceTicket.ServiceTicketID));
                    MessageBody.Replace("{NoteSentOn}", System.DateTime.Now.ToString());
                    MessageBody.Replace("{Sender}", "");
                    MessageBody.Replace("{Note}", recipient.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || recipient.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) ? TrailingNotesIE : TrailingNotes);
                    MessageBody.Replace("{CloseTicket}", String.Empty);

                    String ReplyToAddress = sReplyToadd.Substring(0, sReplyToadd.IndexOf('@')) + "+tn" + Convert.ToString(objServiceTicket.ServiceTicketID) + "un" + Convert.ToString(recipient.UserInfoID) + "nt1en" + sReplyToadd.Substring(sReplyToadd.IndexOf('@'), sReplyToadd.Length - sReplyToadd.IndexOf('@'));
                    new CommonMails().SendMailWithReplyTo(Convert.ToInt64(recipient.UserInfoID), "Support Ticket Center", Common.EmailFrom, Convert.ToString(recipient.Email), sSubject, MessageBody.ToString(), Common.DisplyName, ReplyToAddress, Common.UserName, Common.Password, Common.SMTPHost, Common.SMTPPort.ToString(), "Normal", Common.SSL, true);
                }
            }
        }
    }
    #region My Flagged Asset's
    /// <summary>
    ///  To set Flag/Unflag
    /// </summary>
    /// <param name="EquipmentMasterID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public String RemoveFlag(String EquipmentMasterID)
    {
        String msg = String.Empty;
        try
        {
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            objAssetMgtRepository.UnflaggedAssets(Convert.ToInt64(EquipmentMasterID), IncentexGlobal.CurrentMember.UserInfoID, Incentex.DAL.Common.DAEnums.NoteForType.FlagAssets);
            msg = "Flag Removed";
        }
        catch (Exception ex)
        {
            msg = "oops error occur";
            ErrHandler.WriteError(ex);
        }
        return msg;
    }
    #endregion

    #endregion

}

