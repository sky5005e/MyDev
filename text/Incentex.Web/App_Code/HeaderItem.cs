using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.ServiceModel.Syndication;
using System.Xml;


using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Globalization;


/// <summary>
/// Summary description for HeaderItem
/// </summary>
public class HeaderItem
{
    public HeaderItem()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// Return Details for Pending Shopping Cart Items
    /// </summary>
    /// <param name="UserInfoID"></param>
    /// <returns></returns>
    public static HCItemsResult GetPendingCartItems(Int64 UserInfoID)
    {
        HCItemsResult objCart = new HCItemsResult();
        try
        {
            StringBuilder CartItem = new StringBuilder();

            MyShoppingCartRepository objShoppingCartRepository = new MyShoppingCartRepository();
            List<SelectMyShoppingCartProductResult> objList = objShoppingCartRepository.SelectShoppingProduct(UserInfoID);
            Decimal TotalAmount = objShoppingCartRepository.GetTotalAmountofPendingItemsinCart(UserInfoID, false);

            CartItem.Append(@"<div id=""header-cart"">");
            CartItem.Append(@"<ul class=""cf header-cart-listing"">");
            String SiteURL = ConfigurationSettings.AppSettings["siteurl"];
            String ThumbImagePath = SiteURL + ConfigurationSettings.AppSettings["ProductImagesThumbsPath"].Remove(0, 2);
            // String LargeImagePath = SiteURL + ConfigurationSettings.AppSettings["ProductImagesLargePath"].Remove(0, 2);
            foreach (var item in objList)
            {
                String imagesrc = ThumbImagePath + (!String.IsNullOrEmpty(item.ProductImage) ? item.ProductImage : "Thumb_ProductDefault.jpg");
                // String Largeimagesrc = LargeImagePath + (!String.IsNullOrEmpty(item.LargerProductImage) ? item.LargerProductImage : "ProductDefault.jpg");
                String Summary = !String.IsNullOrEmpty(item.ProductDescrption1) && item.ProductDescrption1.Length > 15 ? item.ProductDescrption1.Substring(0, 15) : item.ProductDescrption1;

                //Start Cart Item List
                //CartItem.Append(@"<div onclick=""ShowItem('" + item.StoreProductID + "','" + item.MyShoppingCartID + "','" + Largeimagesrc + "','" + item.ItemSizeID + "','" + item.ItemSoldByID + "','" + item.Quantity + @"');"">");
                CartItem.Append(@"<li><a class=""cart-listlink"" href=""javascript: void(0);"" onclick=""ShowItem('" + item.StoreProductID + "','" + item.MyShoppingCartID + "','" + item.ItemSizeID + "','" + item.ItemSoldByID + "','" + item.Quantity + @"');"" ontouchstart=""ShowItem('" + item.StoreProductID + "','" + item.MyShoppingCartID + "','" + item.ItemSizeID + "','" + item.ItemSoldByID + "','" + item.Quantity + @"');"">");
                CartItem.Append(@"<span class=""cart-price"">$" + item.UnitPrice + "</span>");
                CartItem.Append(@"<span href=""javascript: void(0);"" class=""cart-img"">");
                CartItem.Append(@"<img width=""28"" height=""28"" alt=""cart"" src=""" + imagesrc + @"""></span>&nbsp;&nbsp;&nbsp;");
                CartItem.Append(@"<span class=""cart-title"" title=""" + item.ProductDescrption1 + "\">" + Summary + "</span>");
                CartItem.Append(@"<span class=""cart-desc"">" + item.Size + @",&nbsp;QTY&nbsp;" + item.Quantity + " </span>");
                CartItem.Append(@"</a></li>");
                //CartItem.Append(@"</div>");
                //END Cart Item List
            }

            CartItem.Append(@"</ul> </div>");
            CartItem.Append(@"<div class=""header-cartsubtotal cf""><strong>Subtotal</strong> <span>" + TotalAmount.ToString("C2") + "</span></div>");
            CartItem.Append(@"<div class=""header-cart-btn cf""><a href=""javascript: void(0);"" class=""alignleft"" title=""Change Currency"" onclick=""ShowCurrencyPopup('" + TotalAmount.ToString("0.00") + @"');"" ontouchstart=""ShowCurrencyPopup('" + TotalAmount.ToString("0.00") + @"');"">Change Currency</a> <a ontouchstart=""ExecuteAnchorHref('" + SiteURL + "NewDesign/UserPages/Checkout.aspx" + @"');"" onclick=""ExecuteAnchorHref('" + SiteURL + "NewDesign/UserPages/Checkout.aspx" + @"');"" class=""alignright"" title=""Checkout"">Checkout</a> </div>");
            CartItem.Append(@"<div class=""header-close-btn cf""><a href=""javascript: void(0);"" class=""close-cart"" title=""Clear Cart"" onclick=""RemoveCartItemsFromMaster();"" ontouchstart=""RemoveCartItemsFromMaster();"">Clear Cart</a></div>");

            // Set html and Count to objCart 
            objCart.HItemsHtml = CartItem.ToString();
            objCart.TotalNumber = TotalAmount.ToString("C2");
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
        }
        return objCart;
    }

    /// <summary>
    /// Get Notifications details
    /// </summary>
    /// <param name="UserInfoID"></param>
    /// <param name="CompanyID"></param>
    /// <returns></returns>
    public static HCItemsResult GetAllNotifications(Int64 UserInfoID, Int64 CompanyID)
    {
        string webpath = ConfigurationSettings.AppSettings["NewDesignSiteurl"].ToString();
        HCItemsResult objNotifications = new HCItemsResult();
        try
        {
            StringBuilder NotificationHtml = new StringBuilder();
            CompanyEmployeeRepository objCompanyEmployeeRepos = new CompanyEmployeeRepository();
            OrderConfirmationRepository objConfirmRepos = new OrderConfirmationRepository();
            AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
            // Get All Recent Ticket
            List<SelectServiceTicketHistoryPerEmployeeResult> objlistTickets = new ServiceTicketRepository().SelectRecentlyUpdatedServiceTicketForCACE(UserInfoID).ToList();
            Int64 RecentMessageCount = objlistTickets.Count;
            // Get All Flagged Asset 
            Int64 VEUserInfoID = 0;
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.EquipmentVendorEmployee))
                VEUserInfoID = Convert.ToInt64(IncentexGlobal.CurrentMember.UserInfoID);

            List<GetEquipmentsResult> listFlaggedAsset = objAssetMgtRepository.GetEquipmentsDetail(null, null, null, null, null, "1", null, CompanyID, VEUserInfoID).OrderBy(o => o.EquipmentID).ToList(); 
            Int64 FlaggedAssetMessageCount = listFlaggedAsset.Count;
            // For pending order's 
            NotificationHtml.Append(@"<div class=""notificationSubnav"">");

            // For CA Only
            Int64 OrderPendingCount = 0;
            Int64 PendingUserCount = 0;
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
            {
                // Get All Pending User for CA
                List<GetPendingUsersListForAdminResult> objUserList = objCompanyEmployeeRepos.GetPendingUsersList(UserInfoID).OrderBy(s => s.BaseStation).ThenBy(s => s.EmployeeName).ThenBy(s => s. WorkGroup).ToList();
                PendingUserCount = objUserList.Count;
                // Get All Pending Order for CA
                List<GetUserPendingOrdersToApproveResult> objOrderPendingList = new List<GetUserPendingOrdersToApproveResult>();
                objOrderPendingList = objConfirmRepos.GetMyPendingOrders(UserInfoID, AnniversaryProgramRepository.CompanyAnniversarySortExpType.OrderID, Incentex.DAL.Common.DAEnums.SortOrderType.Desc).OrderBy(s => s.BaseStation).ToList();
                OrderPendingCount = objOrderPendingList.Count;

                #region Pending Order's
                NotificationHtml.Append(@"<div class=""notifyHeader""><div class=""notification-header""><a href=""javascript:;""><span>" + OrderPendingCount + @"</span>Pending Orders</a></div>");
                if (OrderPendingCount > 0)
                {
                    NotificationHtml.Append(@"<div class=""notifyHeader1""><ul class=""cf"">");
                    foreach (var oItem in objOrderPendingList)
                    {
                        //It should be like "02987 - Susan Smith - Flight Attendant - FLL - $257.95"
                        // Revision Station Code-Contact Name-Workgroup-Amount
                        String _orderDetails = oItem.BaseStation.Substring(0, 3) + " - " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oItem.UserName.ToLower()) + " - " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oItem.WorkGroup.ToLower()) + " - " + Convert.ToDecimal(oItem.TotalAmount).ToString("C2");
                        //oItem.OrderNumber.Substring(Math.Max(0, oItem.OrderNumber.Length - 5)) + " - " + oItem.UserName + " - " + oItem.WorkGroup + " - " + oItem.BaseStation.Substring(0, 3) + " - " + oItem.OrderAmount.ToString("C2");
                        NotificationHtml.Append(@"<li><div class=""row-link cf""><a onclick=""ExecuteAnchorHref('" + webpath + "MyAccount/OrderManagement/PendingOrders.aspx" + @"');"" ontouchstart=""ExecuteAnchorHref('" + webpath + "MyAccount/OrderManagement/PendingOrders.aspx" + @"');"">" + _orderDetails + @"</a><a href=""javascript: void(0);"" class=""check-link"" title=""Approve Order"" onclick=""ApprovalOrderScript('" + oItem.OrderID + @"');"" ontouchstart=""ApprovalOrderScript('" + oItem.OrderID + @"');"">&nbsp;</a><a href=""javascript:;"" class=""cross-link"" title=""Cancel Order"" onclick=""ShowReasonForCancelPopup('" + oItem.OrderID + @"');"" ontouchstart=""ShowReasonForCancelPopup('" + oItem.OrderID + @"');"">&nbsp;</a></div></li>");
                    }
                    NotificationHtml.Append(@"</ul></div>");
                }
                NotificationHtml.Append(@"</div>");
                #endregion
                // For pending User's
                #region Pending User's
                NotificationHtml.Append(@"<div class=""notifyHeader""><div class=""notification-header""><a href=""javascript:;""><span>" + PendingUserCount + @"</span>Pending Users</a></div>");
                if (PendingUserCount > 0)
                {
                    NotificationHtml.Append(@"<div class=""notifyHeader1""><ul class=""cf"">");
                    foreach (var uItem in objUserList)
                    {
                        //It should be like "Request Date - Name - Workgroup - Station"
                        // Revision : Station-Contact Name-Workgroup
                        String _userDetails = uItem.BaseStation.Substring(0, 3) + " - " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(uItem.EmployeeName.ToLower()) + " - " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(uItem.WorkGroup.ToLower());
                        //Convert.ToDateTime(uItem.RequestedDate).ToShortDateString() + " - " + uItem.EmployeeName + " - " + uItem.WorkGroup + " - " + uItem.BaseStation.Substring(0, 3);
                        NotificationHtml.Append(@"<li><div class=""row-link cf""><a href=""javascript: void(0);"" onclick=""ShowUserBasicInfo('" + uItem.iRegistraionID + @"');"" ontouchstart=""ShowUserBasicInfo('" + uItem.iRegistraionID + @"');"">" + _userDetails + @"</a>&nbsp;<a href=""javascript:;"" class=""check-link"" onclick=""ApprovalUserScript('" + uItem.iRegistraionID + @"','true');"" ontouchstart=""ApprovalUserScript('" + uItem.iRegistraionID + @"','true');"">&nbsp;</a><a href=""javascript:;"" class=""cross-link"" onclick=""ApprovalUserScript('" + uItem.iRegistraionID + @"','false');"" ontouchstart=""ApprovalUserScript('" + uItem.iRegistraionID + @"','false');"">&nbsp;</a></div></li>");
                    }
                    NotificationHtml.Append(@"</ul></div>");
                }
                NotificationHtml.Append(@"</div>");
                #endregion
            }
            // For Message's
            #region Message's
            NotificationHtml.Append(@"<div class=""notifyHeader""><div class=""notification-header""><a href=""javascript:;""><span>" + RecentMessageCount + @"</span>Messages</a></div>");
            if (RecentMessageCount > 0)
            {
                NotificationHtml.Append(@"<div class=""notifyHeader1""><ul class=""cf"">");
                foreach (var mItem in objlistTickets)
                {
                    String mostRecentMsg = String.Empty;
                    NoteDetail objNote = new NotesHistoryRepository().GetByForeignKeyId(Convert.ToInt64(mItem.ServiceTicketID), Incentex.DAL.Common.DAEnums.NoteForType.ServiceTicketCAs).FirstOrDefault();
                    if (objNote != null)
                        mostRecentMsg = objNote.Notecontents;

                    //It should be like "Message # - Company - Name -  Station - Status"
                    // Revision  : Sender Name - Message Name - Update - 01/01/2013
                    String _msgDetails = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mItem.OwnerName.ToLower()) + " - " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mItem.ServiceTicketName.ToLower()) + " - Update" + Convert.ToDateTime(mItem.UpdatedDate).ToShortDateString();
                        //mItem.ServiceTicketNumber + " - " + mItem.CompanyName + " - " + mItem.OwnerName + " - " + mItem.BaseStation.Substring(0, 3) + " - " + mItem.TicketStatus;
                    NotificationHtml.Append(@"<li><div class=""row-link cf""><a href=""javascript:;"" title=""" + mostRecentMsg + @""" onclick=""ShowMessageInfo('" + mItem.ServiceTicketID + @"');"" ontouchstart=""ShowMessageInfo('" + mItem.ServiceTicketID + @"');"">" + _msgDetails + @"</a>&nbsp;<a href=""javascript:;"" class=""cross-link"" onclick=""CloseMessageInfo('" + mItem.ServiceTicketID + @"');"" ontouchstart=""CloseMessageInfo('" + mItem.ServiceTicketID + @"');"">&nbsp;</a></div></li>");
                }
                NotificationHtml.Append(@"</ul></div>");
            }
            NotificationHtml.Append(@"</div>");
            #endregion
            // For Flagged Asset's
            #region Flagged Asset's
            NotificationHtml.Append(@"<div class=""notifyHeader""><div class=""notification-header""><a href=""javascript:;""><span>" + FlaggedAssetMessageCount + @"</span>Flagged Assets</a></div>");
            if (FlaggedAssetMessageCount > 0)
            {
                NotificationHtml.Append(@"<div class=""notifyHeader1""><ul class=""cf"">");
                foreach (var fItem in listFlaggedAsset)
                {
                    String mostRecentNote = String.Empty;
                    NoteDetail objNote = new NotesHistoryRepository().GetByForeignKeyId(Convert.ToInt64(fItem.EquipmentMasterID), Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement).FirstOrDefault();
                    if (objNote != null)
                        mostRecentNote = objNote.Notecontents;

                    //It should be like "Asset #  - Asset Type - Station - Status  - X (to remove)"
                    // Revision : Asset ID - Asset Type
                    String strBaseStation = String.Empty;
                    if(!String.IsNullOrEmpty(fItem.BaseStation))
                        strBaseStation = fItem.BaseStation.Substring(0, 3);

                    String _assetDetails = fItem.EquipmentID + " - " + fItem.EquiType + " - " + strBaseStation;// :   + " - " + fItem.Status;
                    NotificationHtml.Append(@"<li><div class=""row-link cf""><a href=""javascript:void(0);"" onclick='ShowFlaggedAssetsNotePopup(" +fItem.EquipmentMasterID + @");'  title=""" + mostRecentNote + "\">" + _assetDetails + @"</a><a href=""javascript:;"" class=""cross-link"" onclick=""FlagRemoval('" + fItem.EquipmentMasterID + @"');"" ontouchstart=""FlagRemoval('" + fItem.EquipmentMasterID + @"');"">&nbsp;</a></div></li>");
                }
                NotificationHtml.Append(@"</ul></div>");
            }
            NotificationHtml.Append(@"</div>");
            #endregion
            NotificationHtml.Append(@"<div>");

            // Set html and Count to objNotifications 
            objNotifications.HItemsHtml = NotificationHtml.ToString();
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                objNotifications.TotalNumber = Convert.ToString(OrderPendingCount + PendingUserCount + RecentMessageCount + FlaggedAssetMessageCount);
            else
                objNotifications.TotalNumber = Convert.ToString(RecentMessageCount + FlaggedAssetMessageCount);
            
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
        }
        return objNotifications;

    }

    /// <summary>
    /// Get All Support/Ticket Message's 
    /// </summary>
    /// <param name="UserInfoID"></param>
    /// <returns></returns>
    public static HCItemsResult GetAllMessage(Int64 ServiceTicketID, String NoteFor, String SpecificNoteFor)
    {

        HCItemsResult objMsg = new HCItemsResult();
        try
        {
            StringBuilder MessageHtmlInfo = new StringBuilder();
            List<GetTicketDetailsForCACEResult> objlistTickets = new ServiceTicketRepository().GetTicketDetailsForCACE(ServiceTicketID, NoteFor, SpecificNoteFor).OrderByDescending(s => s.NoteID).Take(1).ToList();
            if (objlistTickets != null)
            {
                // Message Content Start 
                MessageHtmlInfo.Append(@"<ul class=""message-content-block cf"">");
                MessageHtmlInfo.Append(@"<li><span class=""msg-title"">" + objlistTickets[0].ServiceTicketName + @"</span><div class=""msg-no"">");
                MessageHtmlInfo.Append(@"<a href=""javascript: void(0);"" class=""small-btn"" title=""CLOSED"">" + objlistTickets[0].TicketStatus + @"</a><span>#" + objlistTickets[0].ServiceTicketID + @"</span></div></li>");
                MessageHtmlInfo.Append(@"<li><span class=""msg-client""><strong>Client Name :</strong>" + objlistTickets[0].OpenedByName + @"</span><span class=""msg-date"">Submitted " + Convert.ToDateTime(objlistTickets[0].CreatedDate).ToShortDateString() + @"</span></li>");
                MessageHtmlInfo.Append(@"<li><span class=""msg-client""><strong>Client Support Agent :</strong>" + objlistTickets[0].OwnerName + @"</span><span class=""msg-date"">Resolved " + Convert.ToDateTime(objlistTickets[0].UpdatedDate).ToShortDateString() + @"</span></li>");
                MessageHtmlInfo.Append(@"</ul>");
                MessageHtmlInfo.Append(@"<div id=""boxscroll"">");
                MessageHtmlInfo.Append(@"<div class=""pop-message-listing"">");
                MessageHtmlInfo.Append(@"<ul class=""cf"">");
                Int32 i = 1;
                foreach (var item in objlistTickets)
                {
                    if (i % 2 == 0)
                        MessageHtmlInfo.Append(@"<li class=""customer-chat"">");
                    else
                        MessageHtmlInfo.Append(@"<li class=""user-chat"">");

                    MessageHtmlInfo.Append(@"<h5 class=""cf""><span>" + item.NoteUpdatedBy + ":</span><em>" + Convert.ToDateTime(item.CreateDate).ToShortTimeString() + "</em></h5>");
                    MessageHtmlInfo.Append(@"<p>" + item.NoteContents + "</p>");
                    MessageHtmlInfo.Append(@"</li>");
                    i++;
                }
                MessageHtmlInfo.Append(@"</ul></div></div>");
            }

            //Message Content Ends 

            // Set html objMsg
            objMsg.HItemsHtml = MessageHtmlInfo.ToString();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
        }
        return objMsg;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private static List<RSSFeedItems> GetRssFeedList()
    {
        XmlReader reader = XmlReader.Create(@"http://www.airportsinternational.com/feed");
        SyndicationFeed feed = SyndicationFeed.Load(reader);
        reader.Close();
        List<RSSFeedItems> list = new List<RSSFeedItems>();
        Int32 index = 0;
        foreach (SyndicationItem item in feed.Items)
        {
            RSSFeedItems objRSS = new RSSFeedItems();
            objRSS.index = ++index;
            objRSS.Title = item.Title.Text;
            objRSS.Summary = item.Summary.Text;
            objRSS.ItemUrl = item.Id;
            objRSS.PublishedDate = item.PublishDate.DateTime.ToShortDateString();
            list.Add(objRSS);
        }

        return list;

    }
    /// <summary>
    /// Get RSS Feed Details
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static HCItemsResult GetRSSFeedDetails(Int32 _index)
    {
        HCItemsResult objRSSFeed = new HCItemsResult();
        try
        {
            Int32 _max = _index * 2;
            Int32 _min = _max - 1;
            List<RSSFeedItems> list = GetRssFeedList().Where(q => q.index >= _min && q.index <= _max).ToList();
            StringBuilder sbFeed = new StringBuilder();

            sbFeed.Append(@"<ul class=""cf"">");
            foreach (var item in list)
            {
                sbFeed.Append(@"<li><a href=""javascript: void(0);"" onclick=""ShowFeedComments('" + item.ItemUrl + @"');"">" + item.Title + @"</a></li>");
            }
            sbFeed.Append(@"</ul>");
            // Assign it to  objRSSFeed
            objRSSFeed.HItemsHtml = sbFeed.ToString();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
            return null;
        }
        return objRSSFeed;
    }

    /// <summary>
    /// Get RSS Feed Details
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static HCItemsResult GetRSSFeedDetailsOnPageLoad()
    {
        HCItemsResult objRSSFeed = new HCItemsResult();
        try
        {
            List<RSSFeedItems> list = GetRssFeedList().ToList();
            StringBuilder sbFeed = new StringBuilder();
            sbFeed.Append(@"<ul id=""movingUL""  class=""ticker"">");
            Int32 i = 0;
            while (i < list.Count)
            {
                sbFeed.Append(@"<li><a href=""javascript: void(0);"" onclick=""ShowFeedComments('" + list[i].ItemUrl + @"');"">" + list[i].Title + @"</a></li>");
                sbFeed.Append(@"<li><a href=""javascript: void(0);"" onclick=""ShowFeedComments('" + list[i + 1].ItemUrl + @"');"">" + list[i + 1].Title + @"</a></li>");
                i = i + 2;
            }
            //foreach (var item in list)
            //{
            //    sbFeed.Append(@"<li><div><a href=""javascript: void(0);"" onclick=""ShowFeedComments('" + item.ItemUrl + @"');"">" + item.Title + @"</a></div></li>");
            //}
            sbFeed.Append(@"</ul>");
            // Assign it to  objRSSFeed
            objRSSFeed.HItemsHtml = sbFeed.ToString();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex, false);
            return null;
        }
        return objRSSFeed;
    }
}
public class HCItemsResult
{
    public String HItemsHtml { get; set; }
    public String TotalNumber { get; set; }
}

public class RSSFeedItems
{
    public Int32 index { get; set; }
    public String Title { get; set; }
    public String Summary { get; set; }
    public String ItemUrl { get; set; }
    public String PublishedDate { get; set; }
}

