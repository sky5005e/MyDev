using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using SAP_API;

/// <summary>
/// Summary description for IncentexAPI
/// </summary>
[WebService(Namespace = "http://incentex.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class IncentexAPI : System.Web.Services.WebService
{
    public IncentexAPI()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    #region Fields

    private Boolean orderUpdatingDone = true;
    private Boolean shipmentUpdatingDone = true;

    private readonly Object syncRootOrder = new Object();
    private readonly Object syncRootShipment = new Object();

    #endregion

    #region Web Methods

    //[WebMethod]
    public OrderConfirmType UpdateSalesOrderByString(String SalesOrderString)
    {
        if (!String.IsNullOrEmpty(SalesOrderString))
            SaveXMLFromObject(SalesOrderString);

        OrderConfirmType objResponse = new OrderConfirmType();
        SalesOrderType SalesOrder = new SalesOrderType();

        try
        {
            XmlSerializer objXmlSerializer = new XmlSerializer(typeof(SalesOrderType));
            StringReader objReader = new StringReader(SalesOrderString);

            SalesOrder = (SalesOrderType)objXmlSerializer.Deserialize(objReader);
            objResponse = UpdateSalesOrderBySalesOrderType(SalesOrder);
        }
        catch
        {
            if (String.IsNullOrEmpty(SalesOrderString))
                objResponse.LogMessage = "Can not update order. The string parameter was received as 'null' or empty string.";
            else
                objResponse.LogMessage = "Can not update order. The string parameter does not match the xml structure required to complete this operation.";
            objResponse.LogStatus = "Failed";
            objResponse.SAPOrderID = "";
            objResponse.WordlinkOrderNumber = "";
        }

        return objResponse;
    }

    //[WebMethod]
    public OrderConfirmType UpdateSalesOrderByXmlDocument(XmlDocument SalesOrderXml)
    {
        OrderConfirmType objResponse = new OrderConfirmType();
        SalesOrderType SalesOrder = new SalesOrderType();

        try
        {
            XmlSerializer objXmlSerializer = new XmlSerializer(typeof(SalesOrderType));
            StringReader objReader = new StringReader(SalesOrderXml.InnerXml);

            SalesOrder = (SalesOrderType)objXmlSerializer.Deserialize(objReader);

            objResponse = UpdateSalesOrderBySalesOrderType(SalesOrder);
        }
        catch
        {
            if (SalesOrderXml == null)
                objResponse.LogMessage = "Can not update order. The xml parameter was received as 'null'";
            else
                objResponse.LogMessage = "Can not update order. The xml parameter does not match the xml structure required to complete this operation.";
            objResponse.LogStatus = "Failed";
            objResponse.SAPOrderID = "";
            objResponse.WordlinkOrderNumber = "";
        }

        return objResponse;
    }

    [WebMethod]
    public OrderConfirmType UpdateSalesOrderBySalesOrderType(SalesOrderType SalesOrder)
    {
        lock (syncRootOrder)
        {
            String SAPRequestFilePath = String.Empty;
            Boolean IsException = false;

            OrderConfirmType objResponse = new OrderConfirmType();

            String SAPOrderID = String.Empty;
            String WorldLinkOrderNumber = String.Empty;

            try
            {
                if (!orderUpdatingDone)
                    Monitor.Wait(syncRootOrder);

                using (IncentexBEDataContext db = new IncentexBEDataContext())
                {
                    SAPSalesOrderUpdateRequest objTodaysRequest = db.SAPSalesOrderUpdateRequests.FirstOrDefault(le => le.RequestDate.Date == DateTime.Now.Date);
                    if (objTodaysRequest != null)
                    {
                        objTodaysRequest.NoOfRequests = Convert.ToInt32(objTodaysRequest.NoOfRequests) + 1;
                    }
                    else
                    {
                        objTodaysRequest = new SAPSalesOrderUpdateRequest();
                        objTodaysRequest.RequestDate = DateTime.Now.Date;
                        objTodaysRequest.NoOfRequests = 1;
                        db.GetTable<SAPSalesOrderUpdateRequest>().InsertOnSubmit(objTodaysRequest);
                    }
                    db.SubmitChanges();
                }

                if (SalesOrder != null)
                    SAPRequestFilePath = SaveXMLFromObject(SalesOrder);

                if (SalesOrder != null && SalesOrder.BO != null && SalesOrder.BO.Documents != null && SalesOrder.BO.Documents.row != null && !String.IsNullOrEmpty(SalesOrder.BO.Documents.row.U_WL_OrderNo) && SalesOrder.BO.Document_Lines != null && SalesOrder.BO.Document_Lines.Length > 0)
                {
                    WorldLinkOrderNumber = SalesOrder.BO.Documents.row.U_WL_OrderNo.Trim();

                    OrderConfirmationRepository objOrderRepo = new OrderConfirmationRepository();

                    Order objOrder = objOrderRepo.GetByOrderNo(WorldLinkOrderNumber);

                    if (objOrder != null && !String.IsNullOrEmpty(SalesOrder.BO.Documents.row.U_StoreID) && objOrder.StoreID == Convert.ToInt64(SalesOrder.BO.Documents.row.U_StoreID))
                    {
                        //String WorldLinkContactID = new UserInformationRepository().GetUniqueWorldLinkContactIDByUserInfoID(Convert.ToInt64(objOrder.UserId));
                        //if (WorldLinkContactID == SalesOrder.BO.Documents.row.U_WL_Contact_ID)
                        //{

                        #region Order Update History

                        OrderUpdateHistoryRepository objOrderUpdateHistoryRepo = new OrderUpdateHistoryRepository();
                        OrderUpdateHistory objPreviousUpdateHistory = objOrderUpdateHistoryRepo.GetPreviousUpdateByOrderID(objOrder.OrderID);
                        OrderUpdateHistory objOrderUpdateHistory = new OrderUpdateHistory();

                        List<OrderUpdateHistoryItemDetail> lstItemHistory = new List<OrderUpdateHistoryItemDetail>();

                        objOrderUpdateHistory.CorporateDiscount = objOrder.CorporateDiscount;
                        objOrderUpdateHistory.CreatedBy = null;//As the history is being created by SAP update request
                        objOrderUpdateHistory.CreatedDate = DateTime.Now;
                        objOrderUpdateHistory.CreditAmt = objOrder.CreditAmt;
                        objOrderUpdateHistory.CreditUsed = objOrder.CreditUsed;
                        objOrderUpdateHistory.IsPaid = objOrder.IsPaid;
                        objOrderUpdateHistory.MOASOrderAmount = objOrder.MOASOrderAmount;
                        objOrderUpdateHistory.MOASSalesTax = objOrder.MOASSalesTax;
                        objOrderUpdateHistory.OrderAmount = objOrder.OrderAmount;
                        objOrderUpdateHistory.OrderDate = objOrder.OrderDate;
                        objOrderUpdateHistory.OrderFor = objOrder.OrderFor;
                        objOrderUpdateHistory.OrderID = objOrder.OrderID;
                        objOrderUpdateHistory.OrderNumber = objOrder.OrderNumber;
                        objOrderUpdateHistory.OrderStatus = objOrder.OrderStatus;
                        objOrderUpdateHistory.PAASOrderNumber = objOrder.PAASOrderNumber;
                        objOrderUpdateHistory.PaymentOption = objOrder.PaymentOption;
                        objOrderUpdateHistory.PaymentOptionCode = objOrder.PaymentOptionCode;
                        objOrderUpdateHistory.PaymentTransactionNumber = objOrder.PaymentTranscationNumber;

                        if (objPreviousUpdateHistory != null)
                            objOrderUpdateHistory.PreviousHistoryID = objPreviousUpdateHistory.HistoryID;

                        objOrderUpdateHistory.ReferenceName = objOrder.ReferenceName;
                        objOrderUpdateHistory.SalesTax = objOrder.SalesTax;
                        objOrderUpdateHistory.SAPOrderID = objOrder.SAPOrderID;
                        objOrderUpdateHistory.SendToPAAS = objOrder.SendToPAAS;
                        objOrderUpdateHistory.SentInCSVName = objOrder.SentInCSVName;
                        objOrderUpdateHistory.SentToSAP = objOrder.SentToSAP;
                        objOrderUpdateHistory.ShippingAmount = objOrder.ShippingAmount;
                        objOrderUpdateHistory.StoreID = objOrder.StoreID;
                        objOrderUpdateHistory.UpdatedByPAASDate = objOrder.UpdatedDateByPAAS;
                        objOrderUpdateHistory.UpdatedBySAPDate = objOrder.UpdatedBySAPDate;
                        objOrderUpdateHistory.UserInfoID = objOrder.UserId;
                        objOrderUpdateHistory.WorkGroupID = objOrder.WorkgroupId;

                        objOrderUpdateHistoryRepo.Insert(objOrderUpdateHistory);

                        #endregion

                        SAPOrderID = objOrder.SAPOrderID;

                        if (objOrder.OrderStatus.ToUpper() == "OPEN")
                        {
                            String PaymentOption = String.Empty;
                            String MOASPriceLevel = String.Empty;

                            if (objOrder.PaymentOption != null && Convert.ToInt64(objOrder.PaymentOption) > 0)
                                PaymentOption = new LookupRepository().GetById(Convert.ToInt64(objOrder.PaymentOption)).sLookupName;

                            //Need to add logic to update Payment option code when the new UDFs are added...
                            if (PaymentOption == "Cost-Center Code")
                                objOrder.PaymentOptionCode = Convert.ToString(SalesOrder.BO.Documents.row.U_BP_Cost_Ctr);
                            //else if (PaymentOption == "GL-Code")
                            //    objOrder.PaymentOptionCode = "";
                            else if (PaymentOption == "Purchase Order")
                                objOrder.PaymentOptionCode = Convert.ToString(SalesOrder.BO.Documents.row.U_Payments_PO_No);
                            //else if (PaymentOption == "Paid By Corporate")
                            //    objOrder.PaymentOptionCode = objOrder.PaymentOptionCode;                        
                            else if (PaymentOption == "MOAS")
                            {
                                Boolean IsMOASWithCostCenterCode = new CompanyEmployeeRepository().FUN_IsMOASWithCostCenterCode(Convert.ToInt64(objOrder.UserId));
                                if (IsMOASWithCostCenterCode)
                                    objOrder.PaymentOptionCode = Convert.ToString(SalesOrder.BO.Documents.row.U_BP_Cost_Ctr);
                            }

                            objOrder.ReferenceName = Convert.ToString(SalesOrder.BO.Documents.row.NumAtCard);

                            if (SalesOrder.BO.Documents.row.DocStatus.Trim().ToUpper() == "CANCELED")
                                objOrder.OrderStatus = "Canceled";
                            else if (SalesOrder.BO.Documents.row.DocStatus.Trim().ToUpper() == "CLOSED")
                                objOrder.OrderStatus = "Closed";

                            UserInformation objUser = new UserInformationRepository().GetById(objOrder.UserId);
                            GlobalMenuSetting objDefaultMOASPrice = new GlobalMenuSettingRepository().GetById(objOrder.WorkgroupId, Convert.ToInt64(objOrder.StoreID));

                            #region Items Logic

                            //Getting Order Items for Sales Order Line items                    
                            List<SelectOrderItemsForSAPResult> lstWLOrderItems = new OrderConfirmationRepository().GetOrderItemsForSAP(objOrder.OrderID);
                            SalesOrderTypeBORow[] arrSAPOrderItems = SalesOrder.BO.Document_Lines;

                            Boolean IsShippedItemRemoved = false;
                            Boolean IsShippedItemUpdated = false;
                            Boolean IsNewItemNotFound = false;
                            Boolean IsViolatingIssuancePolicy = false;
                            Boolean IsInventoryLow = false;

                            //checking for shipped item being removed or updated
                            foreach (SelectOrderItemsForSAPResult objShippedItem in lstWLOrderItems.Where(le => le.ShipQuantity > 0))
                            {
                                SalesOrderTypeBORow SAPOrderItem = arrSAPOrderItems.FirstOrDefault(le => le.ItemCode == objShippedItem.ItemNumber && le.U_WorkgroupID == Convert.ToString(objShippedItem.WorkgroupID));
                                if (SAPOrderItem == null)
                                {
                                    IsShippedItemRemoved = true;
                                    break;
                                }
                                else if (Convert.ToInt16(Convert.ToDecimal(SAPOrderItem.Quantity)) != Convert.ToInt16(objShippedItem.Quantity))
                                {
                                    IsShippedItemUpdated = true;
                                    break;
                                }
                            }

                            #endregion

                            if (!IsShippedItemRemoved && !IsShippedItemUpdated)
                            {
                                Boolean IsItOkToUpdateOrder = true;

                                #region Items Logic

                                MyShoppingCartRepository objShoppingCartRepo = new MyShoppingCartRepository();
                                MyIssuanceCartRepository objIssuanceCartRepo = new MyIssuanceCartRepository();

                                List<MyShoppinCart> lstSCartInsertedItems = new List<MyShoppinCart>();
                                List<MyIssuanceCart> lstICartInsertedItems = new List<MyIssuanceCart>();

                                Int64 StoreID = Convert.ToInt64(SalesOrder.BO.Documents.row.U_StoreID);
                                String NameToBeEngraved = Convert.ToString(lstWLOrderItems.FirstOrDefault(le => !String.IsNullOrEmpty(le.NameToBeEngraved)) != null ? lstWLOrderItems.FirstOrDefault(le => !String.IsNullOrEmpty(le.NameToBeEngraved)).NameToBeEngraved : "");
                                String NewShoppingCartIDs = String.Empty;
                                Decimal ItemTotal = Decimal.Zero;
                                Decimal MOASTotal = Decimal.Zero;

                                if (objOrder.OrderFor == "ShoppingCart")
                                {
                                    #region Shopping Cart

                                    Boolean IsBulkOrder = lstWLOrderItems.FirstOrDefault(le => le.IsBulkOrder == true) != null ? lstWLOrderItems.FirstOrDefault(le => le.IsBulkOrder == true).IsBulkOrder : false;

                                    foreach (SelectOrderItemsForSAPResult objItem in lstWLOrderItems)
                                    {
                                        SalesOrderTypeBORow SAPOrderItem = arrSAPOrderItems.FirstOrDefault(le => le.ItemCode == objItem.ItemNumber && le.U_WorkgroupID == Convert.ToString(objItem.WorkgroupID));

                                        if (SAPOrderItem != null)
                                        {
                                            if (String.IsNullOrEmpty(NewShoppingCartIDs))
                                                NewShoppingCartIDs = Convert.ToString(objItem.MyShoppingCartID);
                                            else
                                                NewShoppingCartIDs += "," + Convert.ToString(objItem.MyShoppingCartID);
                                        }
                                        else
                                        {
                                            MyShoppinCart objDeleteItem = objShoppingCartRepo.GetDetailsById(objItem.MyShoppingCartID);
                                            objShoppingCartRepo.Delete(objDeleteItem);
                                        }

                                        #region Order Item History

                                        OrderUpdateHistoryItemDetail objOrderItemHistory = new OrderUpdateHistoryItemDetail();

                                        objOrderItemHistory.BuyerCookie = objItem.BuyerCookie;
                                        objOrderItemHistory.CartID = objItem.MyShoppingCartID;
                                        objOrderItemHistory.CategoryID = objItem.CategoryID;
                                        objOrderItemHistory.CompanyID = objItem.CompanyID;
                                        objOrderItemHistory.CreatedBy = null;//As the history is being created by SAP update request
                                        objOrderItemHistory.CreatedDate = DateTime.Now;
                                        objOrderItemHistory.HistoryID = null;//Will get updated if order update succeeds
                                        objOrderItemHistory.IsBulkOrder = objItem.IsBulkOrder;
                                        objOrderItemHistory.IsCoupaOrder = objItem.IsCoupaOrder;
                                        objOrderItemHistory.IsCoupaOrderSubmitted = objItem.IsCoupaOrderSubmitted;
                                        objOrderItemHistory.IsIssuanceCart = false;
                                        objOrderItemHistory.ItemNumber = objItem.ItemNumber;
                                        objOrderItemHistory.MasterItemNo = objItem.MasterItemNo;
                                        objOrderItemHistory.MOASPriceLevel = Convert.ToByte(objItem.MOASPriceLevel);
                                        objOrderItemHistory.MOASUnitPrice = objItem.MOASUnitPrice;
                                        objOrderItemHistory.NameToBeEngraved = objItem.NameToBeEngraved;
                                        objOrderItemHistory.OrderID = objItem.OrderID;
                                        objOrderItemHistory.ProductDescription = objItem.ProductDescrption;
                                        objOrderItemHistory.ProductImageID = Convert.ToInt64(objItem.ProductImageID);
                                        objOrderItemHistory.Quantity = Convert.ToInt32(objItem.Quantity);
                                        objOrderItemHistory.SizeID = objItem.SizeID;
                                        objOrderItemHistory.StoreID = objItem.StoreId;
                                        objOrderItemHistory.StoreProductID = objItem.StoreProductID;
                                        objOrderItemHistory.SubCategoryID = objItem.SubCategoryID;
                                        objOrderItemHistory.UniformIssuancePolicyID = objItem.UniformIssuancePolicyID;
                                        objOrderItemHistory.UniformIssuancePolicyItemID = objItem.UniformIssuancePolicyItemID;
                                        objOrderItemHistory.UniformIssuanceType = objItem.UniformIssuanceType;
                                        objOrderItemHistory.UnitPrice = objItem.Price;
                                        objOrderItemHistory.UserInfoID = objItem.UserInfoID;
                                        objOrderItemHistory.WorkGroupID = objItem.WorkgroupID;

                                        lstItemHistory.Add(objOrderItemHistory);

                                        #endregion
                                    }

                                    foreach (SalesOrderTypeBORow objSAPItem in arrSAPOrderItems)
                                    {
                                        SelectOrderItemsForSAPResult objWLItem = lstWLOrderItems.FirstOrDefault(le => le.ItemNumber == Convert.ToString(objSAPItem.ItemCode) && le.WorkgroupID == Convert.ToInt64(objSAPItem.U_WorkgroupID));

                                        //skipping already shipped items
                                        if (objWLItem != null && objWLItem.ShipQuantity > 0)
                                        {
                                            ItemTotal += Convert.ToDecimal(objWLItem.Quantity) * Convert.ToDecimal(objWLItem.Price);
                                            MOASTotal += Convert.ToDecimal(objWLItem.Quantity) * Convert.ToDecimal(objWLItem.MOASUnitPrice);
                                            continue;
                                        }
                                        //updating existing non shipped items
                                        else if (objWLItem != null && objWLItem.ShipQuantity == 0)
                                        {
                                            MyShoppinCart objUpdateItem = objShoppingCartRepo.GetDetailsById(objWLItem.MyShoppingCartID);
                                            objUpdateItem.Quantity = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(objSAPItem.Quantity)));
                                            objUpdateItem.UpdatedBySAPDate = DateTime.Now;
                                            ItemTotal += Convert.ToDecimal(objUpdateItem.Quantity) * Convert.ToDecimal(objUpdateItem.UnitPrice);
                                            MOASTotal += Convert.ToDecimal(objUpdateItem.Quantity) * Convert.ToDecimal(objUpdateItem.MOASUnitPrice);
                                        }
                                        //inserting newly added items
                                        else if (objWLItem == null)
                                        {
                                            GetShoCarProByStoIDWorGroIDNIteNumResult objItemDetails = objShoppingCartRepo.GetUniqueShoppingCartProduct(StoreID, Convert.ToInt64(objSAPItem.U_WorkgroupID), Convert.ToString(objSAPItem.ItemCode));

                                            if (objItemDetails != null)
                                            {
                                                #region Check for inventory settings

                                                if (objItemDetails.AllowBackOrderOnStoreProduct != "Yes")
                                                {
                                                    if (objItemDetails.AllowBackOrderOnStoreProduct == "No")
                                                    {
                                                        if (Convert.ToInt32(objItemDetails.Inventory) < Convert.ToInt32(Convert.ToDecimal(objSAPItem.Quantity)))
                                                        {
                                                            IsInventoryLow = true;
                                                            break;
                                                        }
                                                    }
                                                    else//Condition to check allow back order setting at item level
                                                    {
                                                        if (objItemDetails.AllowBackOrderOnProductItem == "No")
                                                        {
                                                            if (Convert.ToInt32(objItemDetails.Inventory) < Convert.ToInt32(Convert.ToDecimal(objSAPItem.Quantity)))
                                                            {
                                                                IsInventoryLow = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }

                                                #endregion

                                                MyShoppinCart objInsertItem = new MyShoppinCart();
                                                objInsertItem.CategoryID = objItemDetails.CategoryID;
                                                objInsertItem.CompanyID = objItemDetails.CompanyID;
                                                objInsertItem.CreatedDate = DateTime.Now;
                                                objInsertItem.CreatedBySAPDate = DateTime.Now;
                                                objInsertItem.Inventory = Convert.ToString(objItemDetails.Inventory);

                                                objInsertItem.IsBulkOrder = IsBulkOrder;

                                                objInsertItem.IsOrdered = true;
                                                objInsertItem.ItemNumber = Convert.ToString(objItemDetails.ItemNumber);
                                                objInsertItem.MasterItemNo = objItemDetails.MasterItemNo;

                                                //objInsertItem.MaterialStyle = "";//can't set here in the api, as it needs to be selected by the user while placing the order from the checkout steps.
                                                objInsertItem.NameToBeEngraved = NameToBeEngraved;

                                                objInsertItem.OrderID = objOrder.OrderID;
                                                objInsertItem.ProductDescrption = Convert.ToString(objItemDetails.ProductDescrption);
                                                objInsertItem.ProductImageID = Convert.ToString(objItemDetails.ProductImageID);
                                                objInsertItem.Quantity = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(objSAPItem.Quantity)));
                                                objInsertItem.RunCharge = Convert.ToString(objItemDetails.RunCharge);
                                                objInsertItem.ShowInventoryLevel = Convert.ToString(objItemDetails.ShowInventoryLevel);
                                                objInsertItem.Size = Convert.ToString(objItemDetails.Size);
                                                objInsertItem.SoldbyName = Convert.ToString(objItemDetails.SoldByName);
                                                objInsertItem.StoreID = objItemDetails.StoreID;
                                                objInsertItem.StoreProductID = objItemDetails.StoreProductID;
                                                objInsertItem.SubCategoryID = objItemDetails.SubCategoryID;
                                                //objInsertItem.TailoringLength = "";//obsolete now, so not needed

                                                #region Pricing logic

                                                //1. Used for company admin price
                                                //2. Used for company employee price
                                                //3. Used for discounted price between the discount start date & end date
                                                //4. Used for third party supplier employee price
                                                //5. Used for close out price

                                                if (objItemDetails.IsCloseOut)
                                                {
                                                    objInsertItem.PriceLevel = 5;
                                                    objInsertItem.UnitPrice = Convert.ToString(objItemDetails.CloseOutPrice);
                                                }
                                                //For discounted price
                                                else if (objItemDetails.Level3PricingStatus != null && DateTime.Now.Date <= objItemDetails.Level3PricingEndDate && DateTime.Now.Date >= objItemDetails.Level3PricingStartDate)
                                                {
                                                    objInsertItem.PriceLevel = 3;
                                                    objInsertItem.UnitPrice = Convert.ToString(objItemDetails.Level3);
                                                }
                                                else
                                                {
                                                    if (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                                                    {
                                                        objInsertItem.PriceLevel = 1;
                                                        objInsertItem.UnitPrice = Convert.ToString(objItemDetails.Level1);
                                                    }
                                                    else if (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                                                    {
                                                        objInsertItem.PriceLevel = 2;
                                                        objInsertItem.UnitPrice = Convert.ToString(objItemDetails.Level2);
                                                    }
                                                    else if (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
                                                    {
                                                        objInsertItem.PriceLevel = 4;
                                                        objInsertItem.UnitPrice = Convert.ToString(objItemDetails.Level2);
                                                    }
                                                }

                                                #region MOAS Pricing

                                                //MOAS pricing for company admins & incentex employees in case of MOAS order only
                                                if (PaymentOption == "MOAS")
                                                {
                                                    //setting MOAS price acccording to the default setting found from the global menu settings
                                                    if (objDefaultMOASPrice != null && !String.IsNullOrEmpty(objDefaultMOASPrice.MOASPaymentPricing))
                                                    {
                                                        objInsertItem.MOASPriceLevel = Convert.ToInt32(objDefaultMOASPrice.MOASPaymentPricing);

                                                        if (Convert.ToString(objDefaultMOASPrice.MOASPaymentPricing) == "2")
                                                            objInsertItem.MOASUnitPrice = Convert.ToString(objItemDetails.Level2);
                                                        else if (Convert.ToString(objDefaultMOASPrice.MOASPaymentPricing) == "4")
                                                            objInsertItem.MOASUnitPrice = Convert.ToString(objItemDetails.Level4);
                                                        else//when any value other than 2 or 4 is found then apply default value 1.
                                                            objInsertItem.MOASUnitPrice = Convert.ToString(objItemDetails.Level1);
                                                    }
                                                    else
                                                    {
                                                        //when default setting is not found apply level1
                                                        objInsertItem.MOASPriceLevel = 1;
                                                        objInsertItem.MOASUnitPrice = Convert.ToString(objItemDetails.Level1);
                                                    }
                                                }
                                                else
                                                {
                                                    objInsertItem.MOASPriceLevel = objInsertItem.PriceLevel;
                                                    objInsertItem.MOASUnitPrice = objInsertItem.UnitPrice;
                                                }

                                                #endregion

                                                #endregion

                                                objInsertItem.UserInfoID = Convert.ToInt64(objOrder.UserId);
                                                objInsertItem.WorkgroupID = objItemDetails.WorkGroupID;

                                                objShoppingCartRepo.Insert(objInsertItem);

                                                lstSCartInsertedItems.Add(objInsertItem);

                                                ItemTotal += Convert.ToDecimal(objInsertItem.Quantity) * Convert.ToDecimal(objInsertItem.UnitPrice);
                                                MOASTotal += Convert.ToDecimal(objInsertItem.Quantity) * Convert.ToDecimal(objInsertItem.MOASUnitPrice);
                                            }
                                            else
                                            {
                                                IsItOkToUpdateOrder = false;
                                                IsNewItemNotFound = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (!IsNewItemNotFound && !IsInventoryLow)
                                    {
                                        objShoppingCartRepo.SubmitChanges();

                                        foreach (MyShoppinCart objInsertItem in lstSCartInsertedItems)
                                        {
                                            if (String.IsNullOrEmpty(NewShoppingCartIDs))
                                                NewShoppingCartIDs = Convert.ToString(objInsertItem.MyShoppingCartID);
                                            else
                                                NewShoppingCartIDs += "," + Convert.ToString(objInsertItem.MyShoppingCartID);
                                        }
                                    }
                                    else
                                    {
                                        IsItOkToUpdateOrder = false;
                                    }

                                    #endregion
                                }
                                else
                                {
                                    #region Issuance Cart

                                    //Getting issuance policy item associated with this orders issuance policy
                                    List<GetUniIssPolItemsByOrderIDResult> lstPoliItems = objIssuanceCartRepo.GetUniformIssuancePolicyItemsByOrderIDForSAP(objOrder.OrderID);

                                    //Whether the issuance policy items are found or not
                                    if (lstPoliItems.Count > 0)
                                    {
                                        List<INC_Lookup> lstAssociationTypes = new LookupRepository().GetByLookup("lnkIssuancePolicy ");
                                        Int64 SingleAssociation = lstAssociationTypes.FirstOrDefault(le => le.sLookupName == "Create Single Item Association").iLookupID;
                                        Int64 GroupQuantityAssociation = lstAssociationTypes.FirstOrDefault(le => le.sLookupName == "Create Group Association").iLookupID;
                                        Int64 GroupBudgetAssociation = lstAssociationTypes.FirstOrDefault(le => le.sLookupName == "Create Group Association with Budget").iLookupID;

                                        foreach (SalesOrderTypeBORow objSAPItem in arrSAPOrderItems)
                                        {
                                            GetUniIssPolItemsByOrderIDResult objPoliItem = lstPoliItems.FirstOrDefault(le => le.ItemNumber == Convert.ToString(objSAPItem.ItemCode) && le.WorkgroupID == Convert.ToInt64(objSAPItem.U_WorkgroupID));

                                            //Item does not belong to issuance policy
                                            if (objPoliItem == null)
                                            {
                                                IsViolatingIssuancePolicy = true;
                                                break;
                                            }
                                        }

                                        //Proceed further only if all items in the update request belong to the same issuance policy
                                        if (!IsViolatingIssuancePolicy)
                                        {
                                            Boolean IsCompletePurchaseRequired = (Convert.ToString(lstPoliItems[0].CompletePurchase).ToUpper() == "Y" || Convert.ToString(lstPoliItems[0].CompletePurchase).ToUpper() == "O") && lstPoliItems[0].CompletePurchaseAtUserLevel == 'C' ? true : false;

                                            #region Checking for Issuance Policy Violations when complete purchase is required

                                            if (IsCompletePurchaseRequired)
                                            {
                                                Boolean IsSingleAssociationPresentInPolicy = lstPoliItems.FirstOrDefault(le => le.AssociationIssuanceType == SingleAssociation) != null ? true : false;
                                                Boolean IsGroupQuantityAssociationPresentInPolicy = lstPoliItems.FirstOrDefault(le => le.AssociationIssuanceType == GroupQuantityAssociation) != null ? true : false;
                                                Boolean IsGroupBudgetAssociationPresentInPolicy = lstPoliItems.FirstOrDefault(le => le.AssociationIssuanceType == GroupBudgetAssociation) != null ? true : false;

                                                #region Check Single Association

                                                //Checking for single item association violation
                                                if (IsSingleAssociationPresentInPolicy)
                                                {
                                                    List<Int64> lstStoreProductsForSingleAssociation = lstPoliItems.Where(le => le.AssociationIssuanceType == SingleAssociation).Select(le => le.StoreProductID).Distinct().ToList();
                                                    Dictionary<Int64, Int64?> dictStoreProductsMasterItems = lstPoliItems.Where(le => le.AssociationIssuanceType == SingleAssociation).Select(le => new { StoreProductID = le.StoreProductID, MasterItemId = le.MasterItemId }).Distinct().ToDictionary(le => le.StoreProductID, le => le.MasterItemId);

                                                    Boolean IsEverySingleAssociationItemPresentOnceAndOnlyOnce = true;

                                                    foreach (Int64 StoreProductID in lstStoreProductsForSingleAssociation)
                                                    {
                                                        foreach (Int64? MasterItemId in dictStoreProductsMasterItems.Where(le => le.Key == StoreProductID).Select(le => le.Value).ToList<Int64?>())
                                                        {
                                                            Int16 ProductItemCountForCurrentStoreProductMasterItemCombination = 0;

                                                            foreach (GetUniIssPolItemsByOrderIDResult objPoliItem in lstPoliItems.Where(le => le.AssociationIssuanceType == SingleAssociation && le.StoreProductID == StoreProductID && le.MasterItemId == MasterItemId))
                                                            {
                                                                SalesOrderTypeBORow SAPOrderItem = arrSAPOrderItems.FirstOrDefault(le => le.ItemCode == objPoliItem.ItemNumber && le.U_WorkgroupID == Convert.ToString(objPoliItem.WorkgroupID));

                                                                if (SAPOrderItem != null)
                                                                    ProductItemCountForCurrentStoreProductMasterItemCombination += 1;
                                                            }

                                                            if (ProductItemCountForCurrentStoreProductMasterItemCombination != 1)
                                                            {
                                                                IsEverySingleAssociationItemPresentOnceAndOnlyOnce = false;
                                                                break;
                                                            }
                                                        }

                                                        if (!IsEverySingleAssociationItemPresentOnceAndOnlyOnce)
                                                            break;
                                                    }

                                                    if (!IsEverySingleAssociationItemPresentOnceAndOnlyOnce)
                                                        IsViolatingIssuancePolicy = true;
                                                }

                                                #endregion

                                                #region Check Group Association

                                                if (!IsViolatingIssuancePolicy && IsGroupQuantityAssociationPresentInPolicy)
                                                {
                                                    List<String> QuantityGroups = lstPoliItems.Where(le => le.AssociationIssuanceType == GroupQuantityAssociation).Select(le => le.NEWGROUP).Distinct().ToList();

                                                    foreach (String Group in QuantityGroups)
                                                    {
                                                        Int16 GroupQuantity = Convert.ToInt16(lstPoliItems.FirstOrDefault(le => le.AssociationIssuanceType == GroupQuantityAssociation && le.NEWGROUP == Group).Issuance);
                                                        Int16 RequestedQuantity = 0;

                                                        //Checking for Group Association Complete Purchase
                                                        foreach (GetUniIssPolItemsByOrderIDResult objPoliItem in lstPoliItems.Where(le => le.AssociationIssuanceType == GroupQuantityAssociation && le.NEWGROUP == Group))
                                                        {
                                                            SalesOrderTypeBORow SAPOrderItem = arrSAPOrderItems.FirstOrDefault(le => le.ItemCode == objPoliItem.ItemNumber && le.U_WorkgroupID == Convert.ToString(objPoliItem.WorkgroupID));

                                                            if (SAPOrderItem != null)
                                                                RequestedQuantity += Convert.ToInt16(Convert.ToDecimal(SAPOrderItem.Quantity));
                                                        }

                                                        if (RequestedQuantity != GroupQuantity)
                                                        {
                                                            IsViolatingIssuancePolicy = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                #endregion

                                                #region Check Group Budget Association

                                                if (!IsViolatingIssuancePolicy && IsGroupBudgetAssociationPresentInPolicy)
                                                {
                                                    List<String> BudgetGroups = lstPoliItems.Where(le => le.AssociationIssuanceType == GroupBudgetAssociation).Select(le => le.NEWGROUP).Distinct().ToList();

                                                    foreach (String Group in BudgetGroups)
                                                    {
                                                        Decimal GroupBudget = Convert.ToDecimal(lstPoliItems.FirstOrDefault(le => le.AssociationIssuanceType == GroupBudgetAssociation && le.NEWGROUP == Group).AssociationbudgetAmt);
                                                        Decimal RequestedBudget = Decimal.Zero;

                                                        //Checking for Group Association Complete Purchase
                                                        foreach (GetUniIssPolItemsByOrderIDResult objPoliItem in lstPoliItems.Where(le => le.AssociationIssuanceType == GroupBudgetAssociation && le.NEWGROUP == Group))
                                                        {
                                                            SalesOrderTypeBORow SAPOrderItem = arrSAPOrderItems.FirstOrDefault(le => le.ItemCode == objPoliItem.ItemNumber && le.U_WorkgroupID == Convert.ToString(objPoliItem.WorkgroupID));

                                                            if (SAPOrderItem != null)
                                                            {
                                                                #region Pricing logic

                                                                Decimal ItemPrice = Decimal.Zero;

                                                                //1. Used for company admin price
                                                                //2. Used for company employee price
                                                                //3. Used for discounted price between the discount start date & end date
                                                                //4. Used for third party supplier employee price
                                                                //5. Used for close out price

                                                                if (objPoliItem.IsCloseOut)
                                                                    ItemPrice = Convert.ToDecimal(objPoliItem.CloseOutPrice);
                                                                //For discounted price
                                                                else if (objPoliItem.Level3PricingStatus != null && DateTime.Now.Date <= objPoliItem.Level3PricingEndDate && DateTime.Now.Date >= objPoliItem.Level3PricingStartDate)
                                                                    ItemPrice = Convert.ToDecimal(objPoliItem.Level3);
                                                                else
                                                                {
                                                                    if (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                                                                        ItemPrice = Convert.ToDecimal(objPoliItem.Level1);
                                                                    else if (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                                                                        ItemPrice = Convert.ToDecimal(objPoliItem.Level2);
                                                                    else if (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
                                                                        ItemPrice = Convert.ToDecimal(objPoliItem.Level4);
                                                                }

                                                                #endregion

                                                                RequestedBudget += Convert.ToDecimal(SAPOrderItem.Quantity) * ItemPrice;
                                                            }
                                                        }

                                                        if (RequestedBudget > GroupBudget)
                                                        {
                                                            IsViolatingIssuancePolicy = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                #endregion
                                            }

                                            #endregion

                                            if (!IsViolatingIssuancePolicy)
                                            {
                                                foreach (SelectOrderItemsForSAPResult objItem in lstWLOrderItems)
                                                {
                                                    SalesOrderTypeBORow SAPOrderItem = arrSAPOrderItems.FirstOrDefault(le => le.ItemCode == objItem.ItemNumber && le.U_WorkgroupID == Convert.ToString(objItem.WorkgroupID));

                                                    if (SAPOrderItem != null)
                                                    {
                                                        if (String.IsNullOrEmpty(NewShoppingCartIDs))
                                                            NewShoppingCartIDs = Convert.ToString(objItem.MyShoppingCartID);
                                                        else
                                                            NewShoppingCartIDs += "," + Convert.ToString(objItem.MyShoppingCartID);
                                                    }
                                                    else
                                                    {
                                                        MyIssuanceCart objDeleteItem = objIssuanceCartRepo.GetByIssuanceCartId(objItem.MyShoppingCartID);
                                                        objIssuanceCartRepo.Delete(objDeleteItem);
                                                    }

                                                    #region Order Item History

                                                    OrderUpdateHistoryItemDetail objOrderItemHistory = new OrderUpdateHistoryItemDetail();

                                                    objOrderItemHistory.BuyerCookie = objItem.BuyerCookie;
                                                    objOrderItemHistory.CartID = objItem.MyShoppingCartID;
                                                    objOrderItemHistory.CategoryID = objItem.CategoryID;
                                                    objOrderItemHistory.CompanyID = objItem.CompanyID;
                                                    objOrderItemHistory.CreatedBy = null;////As the history is being created by SAP update request
                                                    objOrderItemHistory.CreatedDate = DateTime.Now;
                                                    objOrderItemHistory.HistoryID = null;//Will get updated if order update succeeds
                                                    objOrderItemHistory.IsBulkOrder = objItem.IsBulkOrder;
                                                    objOrderItemHistory.IsCoupaOrder = objItem.IsCoupaOrder;
                                                    objOrderItemHistory.IsCoupaOrderSubmitted = objItem.IsCoupaOrderSubmitted;
                                                    objOrderItemHistory.IsIssuanceCart = true;
                                                    objOrderItemHistory.ItemNumber = objItem.ItemNumber;
                                                    objOrderItemHistory.MasterItemNo = objItem.MasterItemNo;
                                                    objOrderItemHistory.MOASPriceLevel = Convert.ToByte(objItem.MOASPriceLevel);
                                                    objOrderItemHistory.MOASUnitPrice = objItem.MOASUnitPrice;
                                                    objOrderItemHistory.NameToBeEngraved = objItem.NameToBeEngraved;
                                                    objOrderItemHistory.OrderID = objItem.OrderID;
                                                    objOrderItemHistory.ProductDescription = objItem.ProductDescrption;
                                                    objOrderItemHistory.ProductImageID = Convert.ToInt64(objItem.ProductImageID);
                                                    objOrderItemHistory.Quantity = Convert.ToInt32(objItem.Quantity);
                                                    objOrderItemHistory.SizeID = objItem.SizeID;
                                                    objOrderItemHistory.StoreID = objItem.StoreId;
                                                    objOrderItemHistory.StoreProductID = objItem.StoreProductID;
                                                    objOrderItemHistory.SubCategoryID = objItem.SubCategoryID;
                                                    objOrderItemHistory.UniformIssuancePolicyID = objItem.UniformIssuancePolicyID;
                                                    objOrderItemHistory.UniformIssuancePolicyItemID = objItem.UniformIssuancePolicyItemID;
                                                    objOrderItemHistory.UniformIssuanceType = objItem.UniformIssuanceType;
                                                    objOrderItemHistory.UnitPrice = objItem.Price;
                                                    objOrderItemHistory.UserInfoID = objItem.UserInfoID;
                                                    objOrderItemHistory.WorkGroupID = objItem.WorkgroupID;

                                                    lstItemHistory.Add(objOrderItemHistory);

                                                    #endregion
                                                }

                                                foreach (SalesOrderTypeBORow objSAPItem in arrSAPOrderItems)
                                                {
                                                    SelectOrderItemsForSAPResult objWLItem = lstWLOrderItems.FirstOrDefault(le => le.ItemNumber == Convert.ToString(objSAPItem.ItemCode) && le.WorkgroupID == Convert.ToInt64(objSAPItem.U_WorkgroupID));

                                                    if (objWLItem != null && objWLItem.ShipQuantity > 0)
                                                    {
                                                        ItemTotal += Convert.ToDecimal(objWLItem.Quantity) * Convert.ToDecimal(objWLItem.Price);
                                                        MOASTotal += Convert.ToDecimal(objWLItem.Quantity) * Convert.ToDecimal(objWLItem.MOASUnitPrice);
                                                    }
                                                    //updating existing non shipped items
                                                    else if (objWLItem != null && objWLItem.ShipQuantity == 0)
                                                    {
                                                        MyIssuanceCart objUpdateItem = objIssuanceCartRepo.GetByIssuanceCartId(objWLItem.MyShoppingCartID);
                                                        objUpdateItem.Qty = Convert.ToInt32(Convert.ToDecimal(objSAPItem.Quantity));
                                                        objUpdateItem.UpdatedBySAPDate = DateTime.Now;
                                                        ItemTotal += Convert.ToDecimal(objUpdateItem.Qty) * Convert.ToDecimal(objUpdateItem.Rate);
                                                        MOASTotal += Convert.ToDecimal(objUpdateItem.Qty) * Convert.ToDecimal(objUpdateItem.MOASRate);
                                                    }
                                                    //inserting newly added items
                                                    else if (objWLItem == null)
                                                    {
                                                        GetUniIssPolItemsByOrderIDResult objPoliItem = lstPoliItems.FirstOrDefault(le => le.ItemNumber == Convert.ToString(objSAPItem.ItemCode) && le.WorkgroupID == Convert.ToInt64(objSAPItem.U_WorkgroupID));

                                                        #region Check for inventory settings

                                                        if (objPoliItem.AllowBackOrderOnStoreProduct != "Yes")
                                                        {
                                                            if (objPoliItem.AllowBackOrderOnStoreProduct == "No")
                                                            {
                                                                if (Convert.ToInt32(objPoliItem.Inventory) < Convert.ToInt32(Convert.ToDecimal(objSAPItem.Quantity)))
                                                                {
                                                                    IsInventoryLow = true;
                                                                    break;
                                                                }
                                                            }
                                                            else//Condition to check allow back order setting at item level
                                                            {
                                                                if (objPoliItem.AllowBackOrderOnProductItem == "No")
                                                                {
                                                                    if (Convert.ToInt32(objPoliItem.Inventory) < Convert.ToInt32(Convert.ToDecimal(objSAPItem.Quantity)))
                                                                    {
                                                                        IsInventoryLow = true;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        #endregion

                                                        MyIssuanceCart objInsertItem = new MyIssuanceCart();
                                                        objInsertItem.UniformIssuancePolicyItemID = Convert.ToInt64(objPoliItem.UniformIssuancePolicyItemID);
                                                        objInsertItem.UserInfoID = Convert.ToInt64(objOrder.UserId);
                                                        objInsertItem.UniformIssuanceType = Convert.ToInt64(objPoliItem.AssociationIssuanceType);
                                                        objInsertItem.MasterItemID = Convert.ToInt64(objPoliItem.MasterItemId);
                                                        objInsertItem.ItemSizeID = Convert.ToInt64(objPoliItem.ItemSizeID);
                                                        objInsertItem.Qty = Convert.ToInt32(Convert.ToDecimal(objSAPItem.Quantity));

                                                        objInsertItem.PriceLevel = Convert.ToInt16(objPoliItem.PricingLevel);

                                                        #region Pricing logic

                                                        //1. Used for company admin price
                                                        //2. Used for company employee price
                                                        //3. Used for discounted price between the discount start date & end date
                                                        //4. Used for third party supplier employee price
                                                        //5. Used for close out price

                                                        if (objPoliItem.IsCloseOut)
                                                            objInsertItem.Rate = Convert.ToDecimal(objPoliItem.CloseOutPrice);
                                                        //For discounted price
                                                        else if (objPoliItem.Level3PricingStatus != null && DateTime.Now.Date <= objPoliItem.Level3PricingEndDate && DateTime.Now.Date >= objPoliItem.Level3PricingStartDate)
                                                            objInsertItem.Rate = Convert.ToDecimal(objPoliItem.Level3);
                                                        else
                                                        {
                                                            if (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                                                                objInsertItem.Rate = Convert.ToDecimal(objPoliItem.Level1);
                                                            else if (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                                                                objInsertItem.Rate = Convert.ToDecimal(objPoliItem.Level2);
                                                            else if (objUser.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.ThirdPartySupplierEmployee))
                                                                objInsertItem.Rate = Convert.ToDecimal(objPoliItem.Level2);
                                                        }

                                                        #region MOAS Pricing

                                                        //MOAS pricing for company admins & incentex employees in case of MOAS order only
                                                        if (PaymentOption == "MOAS")
                                                        {
                                                            //setting MOAS price acccording to the default setting found from the global menu settings
                                                            if (objDefaultMOASPrice != null && !String.IsNullOrEmpty(objDefaultMOASPrice.MOASPaymentPricing))
                                                            {
                                                                objInsertItem.MOASPriceLevel = Convert.ToInt32(objDefaultMOASPrice.MOASPaymentPricing);

                                                                if (Convert.ToString(objDefaultMOASPrice.MOASPaymentPricing) == "2")
                                                                    objInsertItem.MOASRate = Convert.ToDecimal(objPoliItem.Level2);
                                                                else if (Convert.ToString(objDefaultMOASPrice.MOASPaymentPricing) == "4")
                                                                    objInsertItem.MOASRate = Convert.ToDecimal(objPoliItem.Level4);
                                                                else//when any value other than 2 or 4 is found then apply default value 1.
                                                                    objInsertItem.MOASRate = Convert.ToDecimal(objPoliItem.Level1);
                                                            }
                                                            else
                                                            {
                                                                //when default setting is not found apply level1
                                                                objInsertItem.MOASPriceLevel = 1;
                                                                objInsertItem.MOASRate = Convert.ToDecimal(objPoliItem.Level1);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            objInsertItem.MOASPriceLevel = objInsertItem.PriceLevel;
                                                            objInsertItem.MOASRate = objInsertItem.Rate;
                                                        }

                                                        #endregion

                                                        #endregion

                                                        if (!String.IsNullOrEmpty(objPoliItem.RunCharge))
                                                            objInsertItem.RunCharge = Convert.ToString(objPoliItem.RunCharge);

                                                        objInsertItem.StoreProductID = Convert.ToInt64(objPoliItem.StoreProductID);

                                                        objInsertItem.ItemNumber = Convert.ToString(objPoliItem.ItemNumber);
                                                        objInsertItem.CreatedDate = DateTime.Now;
                                                        objInsertItem.CreatedBySAPDate = DateTime.Now;
                                                        objInsertItem.UniformIssuancePolicyID = Convert.ToInt64(objPoliItem.UniformIssuancePolicyID);
                                                        objInsertItem.OrderID = objOrder.OrderID;
                                                        objIssuanceCartRepo.Insert(objInsertItem);

                                                        lstICartInsertedItems.Add(objInsertItem);

                                                        ItemTotal += Convert.ToDecimal(objInsertItem.Qty) * Convert.ToDecimal(objInsertItem.Rate);
                                                        MOASTotal += Convert.ToDecimal(objInsertItem.Qty) * Convert.ToDecimal(objInsertItem.MOASRate);
                                                    }
                                                }
                                            }

                                            if (!IsInventoryLow)
                                            {
                                                objIssuanceCartRepo.SubmitChanges();

                                                foreach (MyIssuanceCart objInsertItem in lstICartInsertedItems)
                                                {
                                                    if (String.IsNullOrEmpty(NewShoppingCartIDs))
                                                        NewShoppingCartIDs = Convert.ToString(objInsertItem.MyIssuanceCartID);
                                                    else
                                                        NewShoppingCartIDs += "," + Convert.ToString(objInsertItem.MyIssuanceCartID);
                                                }
                                            }
                                        }

                                        IsItOkToUpdateOrder = !IsViolatingIssuancePolicy && !IsInventoryLow;
                                    }
                                    else
                                    {
                                        //Issuance policy items are not found associated with this orders issuance cart
                                        IsItOkToUpdateOrder = false;
                                    }

                                    #endregion
                                }

                                #endregion

                                if (IsItOkToUpdateOrder)
                                {
                                    #region Shipping Address

                                    SalesOrderTypeBOAddressExtensionRow objSAPShippingAddress = SalesOrder.BO.AddressExtension.row;

                                    GetCouStaCitForSAPResult objCountryStateCityForSAP = new SAPRepository().GetCountryStateCityForSAP(objSAPShippingAddress.CountryS, objSAPShippingAddress.StateS, objSAPShippingAddress.CityS);

                                    CompanyEmployeeContactInfoRepository objContactInfoRepo = new CompanyEmployeeContactInfoRepository();
                                    OrderUpdateHistoryAddressDetail objOrderAddressHistoryDetail = new OrderUpdateHistoryAddressDetail();

                                    if (objCountryStateCityForSAP != null)
                                    {
                                        CompanyEmployeeContactInfo objShippingContact = objContactInfoRepo.GetShippingDetailByOrderId(objOrder.OrderID);

                                        #region Order Address History

                                        objOrderAddressHistoryDetail.Address1 = objShippingContact.Address;
                                        objOrderAddressHistoryDetail.Address2 = objShippingContact.Address2;
                                        objOrderAddressHistoryDetail.AddressType = objShippingContact.ContactInfoType;
                                        objOrderAddressHistoryDetail.Airport = objShippingContact.Airport;
                                        objOrderAddressHistoryDetail.CityID = objShippingContact.CityID;
                                        objOrderAddressHistoryDetail.CompanyName = objShippingContact.CompanyName;
                                        objOrderAddressHistoryDetail.CountryID = objShippingContact.CountryID;
                                        objOrderAddressHistoryDetail.DepartmentID = objShippingContact.DepartmentID;
                                        objOrderAddressHistoryDetail.Email = objShippingContact.Email;
                                        objOrderAddressHistoryDetail.FirstName = objShippingContact.Name;//Special Case
                                        objOrderAddressHistoryDetail.HistoryID = null;//Will get updated if order update succeeds
                                        objOrderAddressHistoryDetail.LastName = objShippingContact.Fax;//Special Case
                                        objOrderAddressHistoryDetail.OrderID = objShippingContact.OrderID;
                                        objOrderAddressHistoryDetail.Phone = objShippingContact.Telephone;
                                        objOrderAddressHistoryDetail.StateID = objShippingContact.StateID;
                                        objOrderAddressHistoryDetail.SuiteApt = objShippingContact.Street;//Special Case
                                        objOrderAddressHistoryDetail.UserInfoID = objShippingContact.UserInfoID;
                                        objOrderAddressHistoryDetail.ZipCode = objShippingContact.ZipCode;

                                        #endregion

                                        objShippingContact.Address = Convert.ToString(objSAPShippingAddress.Address);
                                        objShippingContact.Address2 = Convert.ToString(objSAPShippingAddress.Address2);
                                        objShippingContact.CountryID = objCountryStateCityForSAP.iCountryID;
                                        objShippingContact.StateID = objCountryStateCityForSAP.iStateID;

                                        if (objCountryStateCityForSAP.iCityID != null)
                                            objShippingContact.CityID = objCountryStateCityForSAP.iCityID;

                                        objShippingContact.county = Convert.ToString(objSAPShippingAddress.CountyS);
                                        objShippingContact.ZipCode = Convert.ToString(objSAPShippingAddress.ZipCodeS);

                                        if (Convert.ToString(objSAPShippingAddress.ShipToBuilding).IndexOf(' ') > 0)
                                        {
                                            objShippingContact.Name = Convert.ToString(objSAPShippingAddress.ShipToBuilding).Substring(0, Convert.ToString(objSAPShippingAddress.ShipToBuilding).IndexOf(' '));
                                            objShippingContact.Fax = Convert.ToString(objSAPShippingAddress.ShipToBuilding).Substring(Convert.ToString(objSAPShippingAddress.ShipToBuilding).IndexOf(' '));
                                        }
                                        else
                                        {
                                            objShippingContact.Name = Convert.ToString(objSAPShippingAddress.ShipToBuilding);
                                            objShippingContact.Fax = String.Empty;
                                        }

                                        objShippingContact.Street = Convert.ToString(objSAPShippingAddress.ShipToStreetNo);

                                        //objShippingContact.Telephone = Convert.ToString(SalesOrder.BO.Documents.row.U_Phone_No);
                                        //objShippingContact.Email = Convert.ToString(SalesOrder.BO.Documents.row.U_Email);
                                    }

                                    objContactInfoRepo.SubmitChanges();

                                    #endregion

                                    #region All amounts

                                    if (objOrder.CreditUsed == "Anniversary" && !String.IsNullOrEmpty(SalesOrder.BO.Documents.row.U_AnnivCred))
                                        objOrder.CreditAmt = Convert.ToDecimal(SalesOrder.BO.Documents.row.U_AnnivCred);

                                    if (!String.IsNullOrEmpty(SalesOrder.BO.Documents.row.DiscSum))
                                        objOrder.CorporateDiscount = Convert.ToDecimal(SalesOrder.BO.Documents.row.DiscSum);

                                    if (SalesOrder.BO.DocumentsAdditionalExpenses != null && SalesOrder.BO.DocumentsAdditionalExpenses.row != null && !String.IsNullOrEmpty(SalesOrder.BO.DocumentsAdditionalExpenses.row.LineTotal))
                                        objOrder.ShippingAmount = Convert.ToDecimal(SalesOrder.BO.DocumentsAdditionalExpenses.row.LineTotal);

                                    objOrder.OrderAmount = ItemTotal;

                                    if (PaymentOption == "MOAS")
                                    {
                                        objOrder.MOASOrderAmount = MOASTotal;
                                        objOrder.MOASSalesTax = Decimal.Round((MOASTotal + Convert.ToDecimal(objOrder.ShippingAmount)) * Convert.ToDecimal(objOrder.StrikeIronTaxRate), 2);
                                    }

                                    objOrder.SalesTax = Decimal.Round((ItemTotal + Convert.ToDecimal(objOrder.ShippingAmount)) * Convert.ToDecimal(objOrder.StrikeIronTaxRate), 2);

                                    #endregion

                                    objOrder.MyShoppingCartID = NewShoppingCartIDs;

                                    objOrder.UpdatedBySAPDate = DateTime.Now;
                                    objOrderRepo.SubmitChanges();

                                    objResponse.LogMessage = "Order updated successfully.";
                                    objResponse.LogStatus = "Success";
                                    objResponse.SAPOrderID = objOrder.SAPOrderID;
                                    objResponse.WordlinkOrderNumber = objOrder.OrderNumber;

                                    #region Finally Saving Order Update History

                                    try
                                    {
                                        //Saving Order Header History
                                        objOrderUpdateHistoryRepo.SubmitChanges();

                                        //Saving Order Address History
                                        objOrderAddressHistoryDetail.HistoryID = objOrderUpdateHistory.HistoryID;
                                        objOrderUpdateHistoryRepo.Insert(objOrderAddressHistoryDetail);

                                        //Saving Order Item History
                                        foreach (OrderUpdateHistoryItemDetail objItemHistory in lstItemHistory)
                                        {
                                            objItemHistory.HistoryID = objOrderUpdateHistory.HistoryID;
                                            objOrderUpdateHistoryRepo.Insert(objItemHistory);
                                        }

                                        objOrderUpdateHistoryRepo.SubmitChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.Source = "Error in saving order history : " + ex.Source;
                                        ErrHandler.WriteError(ex);
                                    }

                                    #endregion
                                }
                                else
                                {
                                    if (IsNewItemNotFound)
                                        objResponse.LogMessage = "Can not update order. The update request attempts to add a new item, which is not found in world-link by (StoreID + WorkGroupID + ItemNumber) combination.";
                                    else if (IsInventoryLow)
                                        objResponse.LogMessage = "Can not update order. Invenotry is low for newly added item in the update request.";
                                    else if (IsViolatingIssuancePolicy)
                                        objResponse.LogMessage = "Can not update order. The update request is violating issuance policy as currently set up for the user.";

                                    objResponse.LogStatus = "Failed";
                                    objResponse.SAPOrderID = objOrder.SAPOrderID;
                                    objResponse.WordlinkOrderNumber = objOrder.OrderNumber;
                                }
                            }
                            else
                            {
                                if (IsShippedItemRemoved)
                                    objResponse.LogMessage = "Can not update order. The update request does not contain item(s) that have been already shipped, can not remove a shipped item.";
                                else
                                    objResponse.LogMessage = "Can not update order. The update request attempts to update item(s) that have been already shipped, can not update a shipped item.";

                                objResponse.LogStatus = "Failed";
                                objResponse.SAPOrderID = objOrder.SAPOrderID;
                                objResponse.WordlinkOrderNumber = objOrder.OrderNumber;
                            }
                        }
                        else
                        {
                            objResponse.LogMessage = "Can not update order. World-Link order status is " + objOrder.OrderStatus + ".";
                            objResponse.LogStatus = "Failed";
                            objResponse.SAPOrderID = objOrder.SAPOrderID;
                            objResponse.WordlinkOrderNumber = objOrder.OrderNumber;
                        }
                        //}
                        //else
                        //{
                        //    objResponse.LogMessage = "Can not update order. Unique Contact ID from SAP does not match with Unique Contact ID in World-Link.";
                        //    objResponse.LogStatus = "Failed";
                        //    objResponse.SAPOrderID = objOrder.SAPOrderID;
                        //    objResponse.WordlinkOrderNumber = objOrder.OrderNumber;
                        //}
                    }
                    else
                    {
                        if (objOrder == null)
                            objResponse.LogMessage = "Failed to update order. Could not find the order to update.";
                        else if (String.IsNullOrEmpty(SalesOrder.BO.Documents.row.U_StoreID))
                        {
                            objResponse.LogMessage += "Failed to update order. U_StoreID was either NULL or an empty string.";
                            objResponse.WordlinkOrderNumber = WorldLinkOrderNumber;
                        }
                        else
                            objResponse.LogMessage = "Failed to update order. Store ID & World-Link Order Number passed in order header does not match in World-Link.";

                        objResponse.LogStatus = "Failed";
                        objResponse.SAPOrderID = "0";
                        objResponse.WordlinkOrderNumber = WorldLinkOrderNumber;
                    }
                }
                else
                {
                    objResponse.LogMessage = "Order not updated successfully. ";
                    objResponse.LogStatus = "Failed";

                    if (SalesOrder == null)
                        objResponse.LogMessage += "SalesOrderType Object was NULL.";
                    else if (SalesOrder.BO == null)
                        objResponse.LogMessage += "SalesOrderTypeBO Object was NULL.";
                    else if (SalesOrder.BO.Documents == null)
                        objResponse.LogMessage += "SalesOrderTypeBODocuments Object was NULL.";
                    else if (SalesOrder.BO.Documents.row == null)
                        objResponse.LogMessage += "SalesOrderTypeBODocumentsRow Object was NULL.";
                    else if (String.IsNullOrEmpty(WorldLinkOrderNumber))
                    {
                        objResponse.LogMessage += "U_WL_OrderNo was either NULL or an empty string.";
                        objResponse.WordlinkOrderNumber = WorldLinkOrderNumber;
                    }
                    else if (SalesOrder.BO.Document_Lines == null)
                    {
                        objResponse.LogMessage += "SalesOrderTypeBORow Object was NULL.";
                        objResponse.WordlinkOrderNumber = WorldLinkOrderNumber;
                    }
                    else if (SalesOrder.BO.Document_Lines.Length == 0)
                    {
                        objResponse.LogMessage += "No line items found in the update request.";
                        objResponse.WordlinkOrderNumber = WorldLinkOrderNumber;
                    }

                    objResponse.SAPOrderID = "0";
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, false);
                IsException = true;

                objResponse.LogMessage = "Failed to update order. An unexpected error occurred. Error Message : " + ex.Message;
                objResponse.LogStatus = "Failed";
                objResponse.SAPOrderID = SAPOrderID;
                objResponse.WordlinkOrderNumber = WorldLinkOrderNumber;
            }
            finally
            {
                objResponse.CreatedOn = DateTime.Now.ToString("MMddyyyy HH:mm:ss");

                String WLResponseFilePath = Path.Combine(Common.SAPUpdateOrderFailedRequestPath, WorldLinkOrderNumber + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_WL_Response.xml");

                XmlSerializer ResponseSerializer = new XmlSerializer(typeof(OrderConfirmType));
                TextWriter ResponserWriter = new StreamWriter(WLResponseFilePath);
                ResponseSerializer.Serialize(ResponserWriter, objResponse);
                ResponserWriter.Close();

                #region Email Notification

                try
                {
                    StringBuilder Body = new StringBuilder();
                    String Subject = String.Empty;

                    if (objResponse.LogStatus == "Success")
                    {
                        Subject = "World-Link (" + (Common.Live ? "live" : "test") + ") : Sales order Update succeeded - Order # " + WorldLinkOrderNumber;
                        Body.Append("Sales order Update to World-Link (" + (Common.Live ? "live" : "test") + ") has been successful for Order # " + WorldLinkOrderNumber + ".");
                    }
                    else
                    {
                        Subject = "World-Link (" + (Common.Live ? "live" : "test") + ") : Sales order Update failed - Order # " + WorldLinkOrderNumber;

                        if (!IsException)
                            Body.Append("Sales order Update to World-Link (" + (Common.Live ? "live" : "test") + ") has failed for Order # " + WorldLinkOrderNumber + ".");
                        else
                        {
                            Body.Append("An unknown error has occured in Updating Sales Order to World-Link (" + (Common.Live ? "live" : "test") + ") for Order # " + WorldLinkOrderNumber + ".");
                            Body.Append("<br/><br/>Please determine & fix the issue for smooth synchronization of Sales Orders between SAP & World-Link.");
                        }
                    }

                    Body.Append("<br/><br/>Attached are the SAP Sales Order update request xml & World-Link Response xml structures.");
                    Body.Append("<br/><br/>Thanks");
                    Body.Append("<br/>World-Link System");
                    Body.Append("<br/><br/><b>Note :</b> Do not reply to this email, it has been sent from an un-monitored email address.");

                    String EmailNotificationList = String.Empty;
                    if (objResponse.LogStatus == "Success")
                        EmailNotificationList = Convert.ToString(ConfigurationManager.AppSettings["SAPOperationSucceedNotifyList"]);
                    else
                        EmailNotificationList = Convert.ToString(ConfigurationManager.AppSettings["SAPOperationFailedNotifyList"]);

                    List<UploadImage> lstAttachments = new List<UploadImage>();

                    UploadImage objUploadRequest = new UploadImage();
                    objUploadRequest.imageOnly = SAPRequestFilePath.Substring(Common.SAPUpdateOrderFailedRequestPath.Length, SAPRequestFilePath.Length - Common.SAPUpdateOrderFailedRequestPath.Length);
                    lstAttachments.Add(objUploadRequest);

                    UploadImage objUploadResponse = new UploadImage();
                    objUploadResponse.imageOnly = WLResponseFilePath.Substring(Common.SAPUpdateOrderFailedRequestPath.Length, WLResponseFilePath.Length - Common.SAPUpdateOrderFailedRequestPath.Length);
                    lstAttachments.Add(objUploadResponse);

                    new CommonMails().SendMailWithMultiAttachment(0, "Incentex API : Update Sales Order By Sales Order Type", CommonMails.EmailFrom, EmailNotificationList, Subject, Body.ToString(), CommonMails.DisplyName, CommonMails.SSL, true, lstAttachments, Common.SAPUpdateOrderFailedRequestPath, CommonMails.SMTPHost, CommonMails.SMTPPort, CommonMails.UserName, CommonMails.Password, null, CommonMails.Live);

                    #region Commented Code
                    //using (System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage())
                    //{
                    //    objEmail.Body = Body.ToString();
                    //    objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                    //    objEmail.IsBodyHtml = true;
                    //    objEmail.Subject = Subject;

                    //    String EmailNotificationList = String.Empty;
                    //    if (objResponse.LogStatus == "Success")
                    //        EmailNotificationList = Convert.ToString(ConfigurationManager.AppSettings["SAPOperationSucceedNotifyList"]);
                    //    else
                    //        EmailNotificationList = Convert.ToString(ConfigurationManager.AppSettings["SAPOperationFailedNotifyList"]);

                    //    foreach (String objTo in EmailNotificationList.Split(','))
                    //    {
                    //        objEmail.To.Add(new MailAddress(objTo));
                    //    }

                    //    Attachment objSAPRequest = new Attachment(SAPRequestFilePath);
                    //    Attachment objWLResponse = new Attachment(WLResponseFilePath);

                    //    if (objSAPRequest != null)
                    //        objEmail.Attachments.Add(objSAPRequest);
                    //    if (objWLResponse != null)
                    //        objEmail.Attachments.Add(objWLResponse);

                    //    SmtpClient objSmtp = new SmtpClient();

                    //    objSmtp.EnableSsl = Common.SSL;
                    //    objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                    //    objSmtp.Host = Common.SMTPHost;
                    //    objSmtp.Port = Common.SMTPPort;

                    //    objSmtp.Send(objEmail);
                    //}
                    #endregion
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex, false);
                }

                #endregion

                orderUpdatingDone = true;
                Monitor.Pulse(syncRootOrder);
            }

            return objResponse;
        }
    }

    [WebMethod]
    public OrderShipmentResponse UpdateSalesOrderShipment(OrderShipmentType OrderShipment)
    {
        lock (syncRootShipment)
        {
            String SAPRequestFilePath = String.Empty;
            Boolean isException = false;

            OrderShipmentResponse objResponse = new OrderShipmentResponse();

            String SAPOrderID = String.Empty;
            String worldLinkOrderNumber = String.Empty;

            try
            {
                if (!shipmentUpdatingDone)
                    Monitor.Wait(syncRootShipment);

                if (OrderShipment != null)
                    SAPRequestFilePath = SaveXMLFromObject(OrderShipment);

                if (OrderShipment != null && OrderShipment.BO != null && OrderShipment.BO.Documents != null && OrderShipment.BO.Documents.row != null && !String.IsNullOrEmpty(OrderShipment.BO.Documents.row.U_WL_OrderNo) && OrderShipment.BO.Document_Lines != null && OrderShipment.BO.Document_Lines.Length > 0)
                {
                    worldLinkOrderNumber = OrderShipment.BO.Documents.row.U_WL_OrderNo;

                    ShipOrderRepository objShipOrderRepos = new ShipOrderRepository();
                    OrderConfirmationRepository objOrderRepo = new OrderConfirmationRepository();

                    Order objOrder = objOrderRepo.GetByOrderNo(worldLinkOrderNumber);

                    if (objOrder != null)
                    {
                        SAPOrderID = objOrder.SAPOrderID;

                        if (!String.IsNullOrEmpty(objOrder.OrderStatus) && objOrder.OrderStatus.ToUpper() == "OPEN")
                        {
                            StringBuilder objHtml = new StringBuilder();
                            String packageID = "Shipment_" + DateTime.Now.Ticks;

                            Int32 actualRemainingQty = 0;
                            Int32 alreadyShippedQty = 0;
                            Int64? shipperService = null;
                            Boolean isItemNotFound = false;
                            Boolean isDuplicateShipment = false;
                            Boolean isItOkay = true;

                            List<SelectOrderItemsForSAPResult> lstWLOrderItems = new OrderConfirmationRepository().GetOrderItemsForSAP(objOrder.OrderID);
                            List<String> lstShippingServices = new List<String>() { };

                            foreach (OrderShipmentTypeBORow objItem in OrderShipment.BO.Document_Lines)
                            {
                                if (shipperService == null || lstShippingServices.FirstOrDefault(le => le == objItem.CarrierName) == null)
                                {
                                    shipperService = new LookupRepository().GetIdByLookupNameNLookUpCode(objItem.CarrierName, "Shipping Type");

                                    #region Add New Shipper

                                    if (shipperService == 0)
                                    {
                                        INC_Lookup objNewShipper = new INC_Lookup();
                                        objNewShipper.iLookupCode = "Shipping Type";
                                        objNewShipper.sLookupName = objItem.CarrierName;
                                        objOrderRepo.Insert(objNewShipper);
                                        objOrderRepo.SubmitChanges();
                                        shipperService = objNewShipper.iLookupID;
                                    }

                                    #endregion

                                    lstShippingServices.Add(objItem.CarrierName);
                                }

                                SelectOrderItemsForSAPResult objOrderItem = lstWLOrderItems.FirstOrDefault(le => le.ItemNumber.Trim().ToUpper().Replace(" ", "") == objItem.ItemCode.ToUpper());
                                Int32 shippedQuantity = Convert.ToInt32(Convert.ToDecimal(objItem.ShippedQuantity));

                                //Check if item already process with same tracking number,item and order
                                if (objOrderItem != null && objOrderItem.MyShoppingCartID != 0 && objShipOrderRepos.CheckIfCargoAlreadyProcess(objOrder.OrderID, objOrderItem.MyShoppingCartID, objItem.TrackingNumber).Count == 0)
                                {
                                    //This is for taking total shipped qty for this item before this shipped
                                    CountTotalShippedQuantiyResult objTotalItemShippedQty = new ShipOrderRepository().GetQtyShippedTotal(Convert.ToInt32(objOrderItem.MyShoppingCartID), objItem.ItemCode, objOrder.OrderID);
                                    alreadyShippedQty = (objTotalItemShippedQty != null ? Convert.ToInt32(objTotalItemShippedQty.ShipQuantity) : 0);

                                    ShipingOrder objShiporder = new ShipingOrder();

                                    //remaining order total qty for current item
                                    actualRemainingQty = Convert.ToInt32(objOrderItem.Quantity) - alreadyShippedQty;

                                    DateTime shippingDate;
                                    Boolean isDate = DateTime.TryParseExact(objItem.ShippedDateTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out shippingDate);
                                    objShiporder.ShipingDate = isDate ? shippingDate : DateTime.Now;
                                    objShiporder.TrackingNo = objItem.TrackingNumber;
                                    objShiporder.PackageId = packageID;

                                    objShiporder.ShipperService = shipperService;
                                    objShiporder.ShipQuantity = Convert.ToInt64(Convert.ToDecimal(objItem.ShippedQuantity));
                                    objShiporder.NoOfBoxes = !String.IsNullOrEmpty(objItem.NoOfBoxes) ? Convert.ToInt32(objItem.NoOfBoxes) : 0;
                                    objShiporder.IsShipped = true;
                                    objShiporder.SupplierId = objOrderItem.SupplierID;
                                    objShiporder.UpdateBy = objOrder.UserId;
                                    objShiporder.OrderID = objOrder.OrderID;
                                    objShiporder.QtyOrder = Convert.ToInt32(objOrderItem.Quantity);
                                    objShiporder.MyShoppingCartiD = objOrderItem.MyShoppingCartID;
                                    objShiporder.ItemNumber = objItem.ItemCode;

                                    objShiporder.CreatedDate = DateTime.Now;
                                    objShiporder.ProcessedFrom = "SAP @ " + DateTime.Now.ToString();

                                    if (actualRemainingQty > 0)//some quantity remaining
                                    {
                                        if (actualRemainingQty >= shippedQuantity)//normal shipped qty case
                                        {
                                            objShiporder.IsExtra = false;
                                            objShiporder.ShipQuantity = shippedQuantity;
                                            objShiporder.RemaingQutOrder = actualRemainingQty - shippedQuantity;

                                            //change order item status based on remaining order item qty
                                            if (objShiporder.RemaingQutOrder == 0)
                                                objShiporder.ShippingOrderStatus = "Shipped Complete";
                                            else
                                                objShiporder.ShippingOrderStatus = "Partial Shipped";

                                            objShipOrderRepos.Insert(objShiporder);

                                            objHtml.Append("<tr>");
                                            objHtml.Append("<td valign='top'>" + Convert.ToString(objItem.ItemCode) + "</td><td valign='top'>" + Convert.ToInt32(objOrderItem.Quantity) + "</td><td valign='top'>" + shippedQuantity + "</td><td valign='top'></td><td valign='top'>" + objOrderItem.Size + "</td><td valign='top'>" + objOrderItem.ProductDescrption + "</td><td valign='top'>" + Convert.ToString(objItem.ShippedDateTime) + "</td><td valign='top'>" + Convert.ToString(objItem.CarrierName) + "</td><td valign='top'><a href='http://wwwapps.ups.com/WebTracking/track?trackNums=" + Convert.ToString(objItem.TrackingNumber) + "&track.x=Track'>" + Convert.ToString(objItem.TrackingNumber) + "</a></td>");
                                            objHtml.Append("</tr>");
                                        }
                                        else//more qty shipped than actual remaining
                                        {
                                            objShiporder.IsExtra = false;
                                            objShiporder.ShipQuantity = actualRemainingQty;
                                            objShiporder.RemaingQutOrder = 0;
                                            objShiporder.ShippingOrderStatus = "Shipped Complete";

                                            objShipOrderRepos.Insert(objShiporder);

                                            objHtml.Append("<tr>");
                                            objHtml.Append("<td valign='top'>" + Convert.ToString(objItem.ItemCode) + "</td><td valign='top'>" + Convert.ToInt32(objOrderItem.Quantity) + "</td><td valign='top'>" + actualRemainingQty + "</td><td valign='top'></td><td valign='top'>" + objOrderItem.Size + "</td><td valign='top'>" + objOrderItem.ProductDescrption + "</td><td valign='top'>" + Convert.ToString(objItem.ShippedDateTime) + "</td><td valign='top'>" + Convert.ToString(objItem.CarrierName) + "</td><td valign='top'><a href='http://wwwapps.ups.com/WebTracking/track?trackNums=" + Convert.ToString(objItem.TrackingNumber) + "&track.x=Track'>" + Convert.ToString(objItem.TrackingNumber) + "</a></td>");
                                            objHtml.Append("</tr>");

                                            //inserting extra qty as another record
                                            ShipingOrder objExtra = new ShipingOrder();
                                            objExtra.ShipingDate = objShiporder.ShipingDate;
                                            objExtra.TrackingNo = objShiporder.TrackingNo;
                                            objExtra.PackageId = objShiporder.PackageId;

                                            objExtra.ShipperService = objShiporder.ShipperService;
                                            objExtra.NoOfBoxes = objShiporder.NoOfBoxes;
                                            objExtra.IsShipped = objShiporder.IsShipped;
                                            objExtra.SupplierId = objShiporder.SupplierId;
                                            objExtra.OrderID = objShiporder.OrderID;
                                            objExtra.QtyOrder = objShiporder.QtyOrder;
                                            objExtra.MyShoppingCartiD = objShiporder.MyShoppingCartiD;
                                            objExtra.ItemNumber = objShiporder.ItemNumber;
                                            objExtra.ShippingOrderStatus = "Extra";
                                            objExtra.IsExtra = true;
                                            objExtra.ShipQuantity = shippedQuantity - actualRemainingQty;
                                            objExtra.RemaingQutOrder = 0;
                                            objExtra.CreatedDate = objShiporder.CreatedDate;
                                            objExtra.ProcessedFrom = objShiporder.ProcessedFrom;

                                            objShipOrderRepos.Insert(objExtra);
                                        }
                                    }
                                    else//no qty remaining, i.e. over shipped case
                                    {
                                        objShiporder.ShippingOrderStatus = "Extra";
                                        objShiporder.IsExtra = true;
                                        objShiporder.ShipQuantity = shippedQuantity;
                                        objShiporder.RemaingQutOrder = 0;

                                        objShipOrderRepos.Insert(objShiporder);
                                    }
                                }
                                else
                                {
                                    if (objOrderItem == null || (objOrderItem != null && objOrderItem.MyShoppingCartID == 0))
                                        isItemNotFound = true;
                                    else
                                        isDuplicateShipment = true;

                                    isItOkay = false;

                                    break;
                                }
                            }

                            if (isItOkay)
                            {
                                objShipOrderRepos.SubmitChanges();

                                sendShippingDetailEmail(objOrder.OrderID, objOrder.OrderNumber, objOrder.UserId, objHtml.ToString());

                                //Update order table status
                                Boolean IsOrderShippedComplete = new ShipOrderRepository().IsOrderShippedComplete(objOrder.OrderID);
                                if (IsOrderShippedComplete)
                                    new OrderConfirmationRepository().UpdatePAASOrder(objOrder.OrderID, "Closed", String.Empty);

                                objResponse.LogMessage = "Order shipment updated successfully.";
                                objResponse.LogStatus = "Success";
                            }
                            else
                            {
                                if (isItemNotFound)
                                    objResponse.LogMessage = "One or more items not found/match in World-Link.";
                                else if (isDuplicateShipment)
                                    objResponse.LogMessage = "Order shipment already updated with same tracking number for the same item.";

                                objResponse.LogStatus = "Failed";
                            }
                        }
                        else
                        {
                            objResponse.LogMessage = "Can not update order. World-Link order status is " + objOrder.OrderStatus + ".";
                            objResponse.LogStatus = "Failed";
                        }
                    }
                    else
                    {
                        objResponse.LogMessage = "Failed to update order shipment. Could not find the order to update shipment details.";
                        objResponse.LogStatus = "Failed";
                    }

                    objResponse.SAPOrderID = SAPOrderID;
                    objResponse.WorldLinkOrderNumber = worldLinkOrderNumber;
                }
                else
                {
                    objResponse.LogMessage = "Order shipment not updated successfully. ";
                    objResponse.LogStatus = "Failed";

                    if (OrderShipment == null)
                        objResponse.LogMessage += "OrderShipmentType Object was NULL.";
                    else if (OrderShipment.BO == null)
                        objResponse.LogMessage += "OrderShipmentTypeBO Object was NULL.";
                    else if (OrderShipment.BO.Documents == null)
                        objResponse.LogMessage += "OrderShipmentBODocuments Object was NULL.";
                    else if (OrderShipment.BO.Documents.row == null)
                        objResponse.LogMessage += "SalesOrderTypeBODocumentsRow Object was NULL.";
                    else if (OrderShipment.BO.Document_Lines.Length == 0)
                    {
                        objResponse.LogMessage += "No line items found in the update request.";
                        objResponse.WorldLinkOrderNumber = worldLinkOrderNumber;
                    }
                    else if (String.IsNullOrEmpty(worldLinkOrderNumber))
                    {
                        objResponse.LogMessage += "U_WL_OrderNo was either NULL or an empty string.";
                        objResponse.WorldLinkOrderNumber = worldLinkOrderNumber;
                    }

                    objResponse.SAPOrderID = "0";
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
                isException = true;

                objResponse.LogMessage = "Failed to update order shipment. An unexpected error occurred. Error Message : " + ex.Message;
                objResponse.LogStatus = "Failed";
                objResponse.SAPOrderID = SAPOrderID;
                objResponse.WorldLinkOrderNumber = worldLinkOrderNumber;
            }
            finally
            {
                objResponse.CreatedOn = DateTime.Now.ToString("MMddyyyy HH:mm:ss");

                String WLResponseFilePath = Path.Combine(Common.SAPUpdateOrderFailedRequestPath, worldLinkOrderNumber + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_WL_Response.xml");

                XmlSerializer ResponseSerializer = new XmlSerializer(typeof(OrderShipmentResponse));
                TextWriter ResponserWriter = new StreamWriter(WLResponseFilePath);
                ResponseSerializer.Serialize(ResponserWriter, objResponse);
                ResponserWriter.Close();

                #region Email Notification

                try
                {
                    StringBuilder emailBody = new StringBuilder();
                    String emailSubject = String.Empty;

                    if (objResponse.LogStatus == "Success")
                    {
                        emailSubject = "World-Link (" + (Common.Live ? "live" : "test") + ") : Sales order shipment update succeeded - Order # " + worldLinkOrderNumber;
                        emailBody.Append("Sales order shipment update to World-Link (" + (Common.Live ? "live" : "test") + ") has been successful for Order # " + worldLinkOrderNumber + ".");
                    }
                    else
                    {
                        emailSubject = "World-Link (" + (Common.Live ? "live" : "test") + ") : Sales order shipment update failed - Order # " + worldLinkOrderNumber;

                        if (!isException)
                            emailBody.Append("Sales order shipment update to World-Link (" + (Common.Live ? "live" : "test") + ") has failed for Order # " + worldLinkOrderNumber + ".");
                        else
                        {
                            emailBody.Append("An unknown error has occured in updating sales order shipment to World-Link (" + (Common.Live ? "live" : "test") + ") for Order # " + worldLinkOrderNumber + ".");
                            emailBody.Append("<br/><br/>Please determine & fix the issue for smooth synchronization of Sales Orders between SAP & World-Link.");
                        }
                    }

                    emailBody.Append("<br/><br/>Log Message : " + objResponse.LogMessage);
                    emailBody.Append("<br/><br/>Attached are the SAP Sales Order shipment update request xml & World-Link Response xml structures.");
                    emailBody.Append("<br/><br/>Thanks");
                    emailBody.Append("<br/>World-Link System");
                    emailBody.Append("<br/><br/><b>Note :</b> Do not reply to this email, it has been sent from an un-monitored email address.");

                    String emailNotificationList = String.Empty;
                    if (objResponse.LogStatus == "Success")
                        emailNotificationList = Convert.ToString(ConfigurationManager.AppSettings["SAPOperationSucceedNotifyList"]);
                    else
                        emailNotificationList = Convert.ToString(ConfigurationManager.AppSettings["SAPOperationFailedNotifyList"]);

                    List<UploadImage> lstAttachments = new List<UploadImage>();

                    UploadImage objUploadRequest = new UploadImage();
                    objUploadRequest.imageOnly = SAPRequestFilePath.Substring(Common.SAPUpdateOrderFailedRequestPath.Length, SAPRequestFilePath.Length - Common.SAPUpdateOrderFailedRequestPath.Length);
                    lstAttachments.Add(objUploadRequest);

                    UploadImage objUploadResponse = new UploadImage();
                    objUploadResponse.imageOnly = WLResponseFilePath.Substring(Common.SAPUpdateOrderFailedRequestPath.Length, WLResponseFilePath.Length - Common.SAPUpdateOrderFailedRequestPath.Length);
                    lstAttachments.Add(objUploadResponse);

                    new CommonMails().SendMailWithMultiAttachment(0, "Incentex API : Update Sales Order Shipment", CommonMails.EmailFrom, emailNotificationList, emailSubject, emailBody.ToString(), CommonMails.DisplyName, CommonMails.SSL, true, lstAttachments, Common.SAPUpdateOrderFailedRequestPath, CommonMails.SMTPHost, CommonMails.SMTPPort, CommonMails.UserName, CommonMails.Password, null, CommonMails.Live);
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex, false);
                }

                #endregion

                shipmentUpdatingDone = true;
                Monitor.Pulse(syncRootShipment);
            }

            return objResponse;
        }
    }

    [WebMethod]
    public InsertItemResponse InsertItem(InsertItemType Item)
    {
        InsertItemResponse objItemResponse = new InsertItemResponse();
        if (Item != null)
        {
            if (!String.IsNullOrEmpty(Item.Status) && !String.IsNullOrEmpty(Item.ItemCode) && !String.IsNullOrEmpty(Item.ItemDescription))
            {
                if (Item.Status.ToLower() == "add")
                {
                    #region Insert Item Logic

                    Int64 ItemID = 0;
                    SAPRepository objSAPRepository = new SAPRepository();

                    try
                    {
                        if (!objSAPRepository.IsItemDuplicate(Item.ItemCode, ""))
                        {
                            SAPItem objSAPItem = new SAPItem();
                            objSAPItem.SAPItemNumber = Item.ItemCode;
                            objSAPItem.SAPItemDescription = Item.ItemDescription;
                            objSAPItem.CreatedDate = System.DateTime.Now;
                            objSAPItem.IsActive = true;
                            objSAPItem.IsfromSAP = true;
                            objSAPItem.IsDeleted = false;

                            //Insert Item to the Database
                            objSAPRepository.Insert(objSAPItem);
                            objSAPRepository.SubmitChanges();

                            //Success Response
                            objItemResponse.LogMessage = "Item created successfully";
                            objItemResponse.LogStatus = "Success";
                            objItemResponse.WorldLinkItemID = objSAPItem.SAPItemID.ToString();
                            objItemResponse.CreatedOn = DateTime.Now.ToString("MMddyyyy HH:mm:ss");
                        }
                        else
                        {
                            //Duplicate Response
                            objItemResponse.LogMessage = "Item already exists in World-Link.";
                            objItemResponse.LogStatus = "Filtered";
                            objItemResponse.WorldLinkItemID = "0";
                            objItemResponse.CreatedOn = DateTime.Now.ToString("MMddyyyy HH:mm:ss");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Exception Response
                        ErrHandler.WriteError(ex, false);
                        XmlSerializer serializer = new XmlSerializer(typeof(SalesOrderType));
                        TextWriter textWriter = new StreamWriter(Path.Combine(Common.SAPInsertItemFailedResponsePath, Item.ItemCode + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_SAP_Request.xml"));
                        serializer.Serialize(textWriter, Item);
                        textWriter.Close();

                        objItemResponse.LogMessage = "Failed to insert item. An unexpected error occurred. Error Message : " + ex.Message;
                        objItemResponse.LogStatus = "Failed";
                        objItemResponse.WorldLinkItemID = ItemID.ToString();
                        objItemResponse.CreatedOn = DateTime.Now.ToString("MMddyyyy HH:mm:ss");

                        #region Email Notification

                        StringBuilder objBody = new StringBuilder();

                        objBody.Append("An unexpected error has occured in the SAP item insert opertaion for item number " + Item.ItemCode + ".");
                        objBody.Append("<br/>Please determine & fix the issue for smooth synchronization of Items between World-Link & SAP.");
                        objBody.Append("<br/><br/>Thanks");
                        objBody.Append("<br/>Integration Team");
                        objBody.Append("<br/><br/><b>Note :</b> Do not reply to this email, it has been sent from an un-monitored email address.");

                        using (System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage())
                        {
                            objEmail.Body = objBody.ToString();
                            objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                            objEmail.IsBodyHtml = true;
                            objEmail.Subject = "World-Link : SAP item insert failed (Item # " + Item.ItemCode + ")";
                            objEmail.To.Add(new MailAddress("prashanth.kankhara@indianic.com"));

                            SmtpClient objSmtp = new SmtpClient();

                            objSmtp.EnableSsl = Common.SSL;
                            objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                            objSmtp.Host = Common.SMTPHost;
                            objSmtp.Port = Common.SMTPPort;

                            objSmtp.Send(objEmail);
                        }
                        #endregion
                    }
                    #endregion
                }
                else if (Item.Status.ToLower() == "update")
                {
                    #region Update Item Logic

                    Int64 ItemID = 0;
                    SAPRepository objSAPRepository = new SAPRepository();

                    try
                    {
                        var UpdateItem = objSAPRepository.GetByItemCode(Item.ItemCode);
                        if (UpdateItem != null)
                        {

                            //Update the item Number when only description might have changed
                            UpdateItem.SAPItemDescription = Item.ItemDescription;
                            UpdateItem.UpdatedDate = System.DateTime.Now;
                            objSAPRepository.SubmitChanges();
                            ItemID = UpdateItem.SAPItemID;

                            //Success Response
                            objItemResponse.LogMessage = "Item updated Successfully";
                            objItemResponse.LogStatus = "Success";
                            objItemResponse.WorldLinkItemID = Convert.ToString(ItemID);
                            objItemResponse.CreatedOn = DateTime.Now.ToString("MMddyyyy HH:mm:ss");
                        }
                        else
                        {
                            //Failed Response
                            objItemResponse.LogMessage = "Failed to update item. Could not find the item to update.";
                            objItemResponse.LogStatus = "Failed";
                            objItemResponse.WorldLinkItemID = Convert.ToString(ItemID);
                            objItemResponse.CreatedOn = DateTime.Now.ToString("MMddyyyy HH:mm:ss");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Exception Response
                        ErrHandler.WriteError(ex, false);
                        XmlSerializer serializer = new XmlSerializer(typeof(SalesOrderType));
                        TextWriter textWriter = new StreamWriter(Path.Combine(Common.SAPUpdateItemFailedResponsePath, Item.ItemCode + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_SAP_Request.xml"));
                        serializer.Serialize(textWriter, Item);
                        textWriter.Close();

                        objItemResponse.LogMessage = "Failed to update item. An unexpected error occurred. Error Message : " + ex.Message;
                        objItemResponse.LogStatus = "Failed";
                        objItemResponse.WorldLinkItemID = ItemID.ToString();
                        objItemResponse.CreatedOn = DateTime.Now.ToString("MMddyyyy HH:mm:ss");

                        #region Email Notification

                        StringBuilder objBody = new StringBuilder();

                        objBody.Append("An unexpected error has occured in the SAP item insert opertaion for item number " + Item.ItemCode + ".");
                        objBody.Append("<br/>Please determine & fix the issue for smooth synchronization of Items between World-Link & SAP.");
                        objBody.Append("<br/><br/>Thanks");
                        objBody.Append("<br/>Integration Team");
                        objBody.Append("<br/><br/><b>Note :</b> Do not reply to this email, it has been sent from an un-monitored email address.");

                        using (System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage())
                        {
                            objEmail.Body = objBody.ToString();
                            objEmail.From = new MailAddress(Common.EmailFrom, Common.DisplyName);
                            objEmail.IsBodyHtml = true;
                            objEmail.Subject = "World-Link : SAP item update failed (Item # " + Item.ItemCode + ")";
                            objEmail.To.Add(new MailAddress("prashanth.kankhara@indianic.com"));

                            SmtpClient objSmtp = new SmtpClient();

                            objSmtp.EnableSsl = Common.SSL;
                            objSmtp.Credentials = new NetworkCredential(Common.UserName, Common.Password);
                            objSmtp.Host = Common.SMTPHost;
                            objSmtp.Port = Common.SMTPPort;

                            objSmtp.Send(objEmail);
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            else
            {
                objItemResponse.LogMessage = "Failed to insert item. Item Number/Item Description/Item Status was either NULL or an empty string.";
                objItemResponse.LogStatus = "Failed";
                objItemResponse.WorldLinkItemID = "0";
                objItemResponse.CreatedOn = DateTime.Now.ToString("MMddyyyy HH:mm:ss");
            }
        }
        return objItemResponse;
    }

    #endregion

    #region Methods

    private Boolean LockOrder(Int64 OrderID)
    {
        Boolean IsLocked = false;
        OrderDetailHistoryRepository objHis = new OrderDetailHistoryRepository();
        OrderDetailPageHistory objPageHistory = objHis.GetByOrderId(OrderID);

        if (objPageHistory != null)
        {
            if (objPageHistory.UserId == null)
                AddOrderDetailHistory(OrderID);
            else
                IsLocked = true;
        }
        else
        {
            AddOrderDetailHistory(OrderID);
        }

        return IsLocked;
    }

    private void AddOrderDetailHistory(Int64 OrderID)
    {
        OrderDetailHistoryRepository objHis = new OrderDetailHistoryRepository();
        OrderDetailPageHistory objPageHistory = objHis.GetByOrderId(OrderID);

        if (objPageHistory != null)
        {
            objPageHistory.UserId = null;
            objPageHistory.InDateTime = DateTime.Now;
            objHis.SubmitChanges();
        }
    }

    private String SaveXMLFromObject(Object o)
    {
        String RequestFilePath = Path.Combine(Common.SAPUpdateOrderFailedRequestPath, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_SAP_Request.xml");
        try
        {
            XmlSerializer XmlS = new XmlSerializer(o.GetType());

            StringWriter sw = new StringWriter();
            TextWriter textWriter = new StreamWriter(RequestFilePath);
            XmlS.Serialize(textWriter, o);
            textWriter.Close();
        }
        catch
        {

        }
        return RequestFilePath;
    }

    private void sendShippingDetailEmail(Int64 OrderID, String orderNumber, Int64? userInfoID, String itemDetail)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;

            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "Order Item Shipped";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                UserInformation objUserInformation = new UserInformationRepository().GetById(userInfoID);
                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = objUserInformation.LoginEmail;
                String sSubject = "Your Order Shipping Information - (" + orderNumber + ")";
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{fullname}", objUserInformation.FirstName + " " + objUserInformation.LastName);
                messagebody.Replace("{OrderNo}", orderNumber);
                messagebody.Replace("{ItemDetail}", itemDetail);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                String smtphost = CommonMails.SMTPHost; //Application["SMTPHOST"].ToString();
                Int32 smtpport = CommonMails.SMTPPort; //Convert.ToInt32(Application["SMTPPORT"]);
                //String smtpUserID = CommonMails.UserName; //Application["SMTPUSERID"].ToString();
                //String smtppassword = CommonMails.Password; //Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(objUserInformation.UserInfoID, "Sync Information From PASS", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, OrderID);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion
}