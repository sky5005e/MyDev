using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class OrderConfirmationRepository : RepositoryBase
    {
        IQueryable<Order> GetAllQuery()
        {
            IQueryable<Order> qry = from a in db.Orders
                                    select a;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="WorkgroupId"></param>
        /// <returns></returns>
        public Order GetRecordByOrderId(Int64 OrderId, Int64 WorkgroupId)
        {

            //Order objOrder = GetSingle(GetAllQuery().Where(O => O.OrderID == OrderId && O.WorkgroupId == WorkgroupId).ToList());
            Order objOrder = (from a in db.Orders
                              where a.OrderID == OrderId && a.WorkgroupId == WorkgroupId
                              select a).FirstOrDefault();
            return objOrder;
        }

        public String GetMaxOrderNumber()
        {
            return Convert.ToString(db.GetMaxOrderNumber());
        }

        public Int32 RemoveOrderById(Int64 OrderId, Int64 WorkgroupId)
        {
            try
            {
                Order objOrder = GetRecordByOrderId(OrderId, WorkgroupId);
                Delete(objOrder);
                SubmitChanges();
                return 0;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return -1;
            }
        }

        public Int32 RemoveOrderConfirmation(Int64 OrderId)
        {
            try
            {
                Order objOrder = new Order();
                objOrder.OrderID = OrderId;
                Delete(objOrder);
                SubmitChanges();
                return 0;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return -1;
            }
        }

        /// <summary>
        /// Gets the record in the Order table by Order Number
        /// </summary>
        /// Updated By : Gaurang Pathak
        /// <param name="OrderNumber"></param>
        /// <returns>Single record from the Order Table</returns>
        public Order GetByOrderID(Int64 OrderId)
        {
            //Order objOrder = GetSingle(GetAllQuery().Where(O => O.OrderID == OrderId).ToList());
            Order objOrder = (from a in db.Orders
                              where a.OrderID == OrderId
                              select a).FirstOrDefault();
            return objOrder;
        }

        /// <summary>
        /// Check Coupa Order Id Exist in Order table or not
        /// </summary>
        /// <param name="CoupaOrderID"></param>
        /// <returns>Order Table</returns>
        public Order GetOrderDetailsByCoupaOrderID(String CoupaOrderID)
        {
            return db.Orders.Where(q => q.CoupaOrderID == CoupaOrderID).FirstOrDefault();
        }

        /// <summary>
        ///Select MyShoppingCartid On the OrderNumber basis
        /// </summary>
        /// <param name="OrderNumber">Order Number</param>
        /// Nagmani Kumar 31/01/2011
        public List<Order> GetShoppingCartId(String strOrderNumber)
        {
            List<Order> obj = new List<Order>();
            obj = (from q in db.Orders where q.OrderNumber == strOrderNumber select q).ToList();
            return obj;
        }

        public List<Order> GetShoppingCartIdByOId(Int64 Orderid)
        {
            List<Order> obj = new List<Order>();
            obj = (from q in db.Orders where q.OrderID == Orderid select q).ToList();
            return obj;
        }

        /// <summary>
        ///Update the Inventroy record in table storProductInventroy
        ///after order confirmation of item.
        /// </summary>
        /// <param name="OrderNumber">Order Number</param>
        /// Nagmani Kumar 01/02/2011
        public String UpdateInventory(Int64 MyShoppingCartId, Int64 ProductItemId, String strProcess)
        {
            Object obj;
            if (strProcess == "Shopping")
            {
                obj = db.UpDateInventoryAfterShopping(MyShoppingCartId, ProductItemId);
                return "Update Inventory";
            }
            else
            {
                obj = db.UpDateInventoryAfterIssuanceProgram(MyShoppingCartId, ProductItemId);
                return "Update Inventory";
            }
        }

        /// <summary>
        ///Select Order details of Company Employee
        /// </summary>
        /// <param name="CompanyiD"></param>
        /// Nagmani Kumar 24/02/2011
        public List<SelectCompanyOrderDetailsResult> GetCompanyOrderDetail(Int64 CompanyId)
        {
            return db.SelectCompanyOrderDetails(CompanyId).ToList();
        }

        /// <summary>
        ///Select Order Address of Employee
        /// </summary>
        /// <param name="OrderId"></param>
        /// Nagmani Kumar 01/03/2011
        public List<SelectOrderAddressResult> GetOrderAddress(Int32 OrderId)
        {
            return db.SelectOrderAddress(OrderId).ToList();
        }

        /// <summary>
        ///Select Supplier Address
        /// </summary>
        /// <param name="Shoppingcartid"></param>
        /// <param name="Orderfor"></param>
        /// Nagmani Kumar 01/03/2011
        public List<SelectSupplierAddressResult> GetSupplierAddress(String ShoppingFor, Int64 OrderId)
        {
            return db.SelectSupplierAddress(ShoppingFor, OrderId).ToList();
        }

        /// <summary>
        ///Select OrderDetails
        /// </summary>
        /// <param name="Shoppingcartid"></param>
        /// <param name="Orderfor"></param>
        /// Nagmani Kumar 02/03/2011
        public List<SelectOrderDetailsResult> GetDetailOrder(Int32 MyShoppingCartID, Int32 SupplierID, String ShopingFor)
        {
            return db.SelectOrderDetails(MyShoppingCartID, SupplierID, ShopingFor).ToList();
        }

        public List<SelectOrderDetailsForSupplierResult> GetOrderItemsForSupplier(Int64 SupplierID, Int64 OrderID)
        {
            return db.SelectOrderDetailsForSupplier(SupplierID, OrderID).ToList();
        }

        public List<SelectOrderItemsResult> GetOrderItems(Int64 OrderID)
        {
            return db.SelectOrderItems(OrderID).ToList();
        }

        public List<SelectOrderItemsForSAPResult> GetOrderItemsForSAP(Int64 OrderID)
        {
            return db.SelectOrderItemsForSAP(OrderID).ToList();
        }

        public void UpdateStatus(Int64 OrderID, String Status)
        {
            UpdateStatus(OrderID, Status, null, null);
        }

        public void UpdateStatus(Int64 OrderID, String Status, Int64? DeletedBy, DateTime? DeletedDate)
        {
            db.UpdateOrderStaus(OrderID, Status, DeletedBy, DeletedDate);
            db.SubmitChanges();
        }

        public void UpdatePAASOrder(Int64 OrderID, String Status, String PAASOrderNumber)
        {
            Order objOrder = db.Orders.FirstOrDefault(le => le.OrderID == OrderID);
            if (objOrder != null)
            {
                objOrder.OrderStatus = Status;
                objOrder.PAASOrderNumber = PAASOrderNumber;
                objOrder.UpdatedDateByPAAS = DateTime.Now;
                objOrder.UpdatedDate = DateTime.Now;
            }
            db.SubmitChanges();
        }

        public List<SelectSupplierAddressBySupplierIdResult> GetSupplierAddressBySupplier(String ShoppingFor, Int64 OrderId, Int64 SupplierId)
        {
            return db.SelectSupplierAddressBySupplierId(ShoppingFor, OrderId, SupplierId).ToList();
        }

        public List<SelectShipToAddressByOrderIDResult> GetShipToAddressByOrderId(Int64 OrderId)
        {
            return db.SelectShipToAddressByOrderID(OrderId).ToList();
        }

        public List<GetOrdersToBeReturnedByUserInfoIDResult> GetOrderNo(Int64 userid)
        {
            return db.GetOrdersToBeReturnedByUserInfoID(userid).ToList();
        }

        public void UpdateMyIssuanceOrderStatus(Int64 MyIssuanceCartiD, String OrderStatus)
        {
            db.UpdateMyIssuanceCartOrderStatus(MyIssuanceCartiD, OrderStatus);
            db.SubmitChanges();
        }

        public List<SelectCompanyStoreNameResult> GetCompanyStoreName()
        {
            return db.SelectCompanyStoreName().ToList();
        }

        public List<SelectCompanyOrderDetailsViewResult> GetCompanyOrderDetailView(DateTime? fromDate, DateTime? toDate, String storeID, String emailAddress, String orderNumber, String orderStatus, String firstName, String lastName, String employeeCode, String stationCode, String workGroup, String paymentType, Boolean? SAPImportStatus, DateTime? hireDateBefore, String orderPlaced, String baseStationsAccess)
        {
            return db.SelectCompanyOrderDetailsView(fromDate, toDate, storeID, emailAddress, orderNumber, orderStatus, firstName, lastName, employeeCode, stationCode, workGroup, paymentType, SAPImportStatus, hireDateBefore, orderPlaced, baseStationsAccess).ToList();
        }

        public List<SelectOrderBySearchCriteriaForSupplierResult> GetSupplierOrders(DateTime? fromDate, DateTime? toDate, String storeID, String emailAddress, String orderNumber, String orderStatus, String firstName, String lastName, String employeeCode, String stationCode, String workGroup, String paymentType, DateTime? hireDateBefore, String orderPlaced, String baseStationsAccess, Int64 supplierID)
        {
            return db.SelectOrderBySearchCriteriaForSupplier(fromDate, toDate, storeID, emailAddress, orderNumber, orderStatus, firstName, lastName, employeeCode, stationCode, workGroup, paymentType, hireDateBefore, orderPlaced, baseStationsAccess, supplierID).ToList();
        }

        public CheckProductInventoryResult GetProductInventoryToArriveOnDate(Int64? ProductID, Int32 QtyEntered)
        {
            return db.CheckProductInventory(ProductID, QtyEntered).SingleOrDefault();
        }

        public List<Order> CheckUserIDWorkgroupIDEmpIDExist(Int64 userid, Int64? employeeTypeID, Int64? workgroupID)
        {
            List<Order> obj = new List<Order>();
            obj = (from q in db.Orders where q.OrderStatus != null && q.UserId == userid && q.OrderStatus.ToUpper() != "CANCELED" && q.OrderStatus.ToUpper() != "DELETED" && q.OrderFor == "IssuanceCart" && q.EmployeeTypeID == employeeTypeID && q.WorkgroupId == workgroupID select q).ToList();
            return obj;

        }

        public List<Order> CheckUserIDExist(Int64 userid)
        {
            return (from q in db.Orders where q.OrderStatus != null && q.UserId == userid && q.OrderStatus.ToUpper() != "CANCELED" && q.OrderStatus.ToUpper() != "DELETED" && q.OrderFor == "IssuanceCart" select q).ToList();
        }

        public Order GetByOrderNo(String OrderNumber)
        {
            //return GetAllQuery().Where(o => o.OrderNumber == OrderNumber).ToList().SingleOrDefault();
            return (from a in db.Orders
                    where a.OrderNumber == OrderNumber
                    select a).SingleOrDefault();
        }

        public SelectPAASOrderByWebOrderNoOrCustomerPOResult GetByWebOrderNoORCustomerPONo(String WebOrderNo, String CustomerPONo)
        {
            return db.SelectPAASOrderByWebOrderNoOrCustomerPO(WebOrderNo, CustomerPONo).FirstOrDefault();
        }

        public String AddInventory(Int64 MyShoppingCartId, Int64 ProductItemId, String strProcess)
        {
            Object obj;
            if (strProcess == "Shopping")
            {
                obj = db.IncreaseInventory(MyShoppingCartId, ProductItemId);
                return "Added Inventory";
            }
            else
            {
                obj = db.increaseInventoryForIssuance(MyShoppingCartId, ProductItemId);
                return "Added Inventory";
            }
        }

        public List<SelectOrderAssociationReportResult> GetOrderAssociationReport(String MasterItemNo, String ItemNo)
        {
            if (MasterItemNo == "")
            {
                MasterItemNo = null;
            }
            if (ItemNo == "")
            {
                ItemNo = null;
            }
            return db.SelectOrderAssociationReport(MasterItemNo, ItemNo).ToList();
        }

        public String IncreaseDescreaseInventory(Int64 ProductItemId, Int32 EnteredQty, String returnmessage)
        {
            db.IncDecInventory(EnteredQty, ProductItemId, ref returnmessage);
            return returnmessage;
        }

        public void UpdateOrderWiseUsers()
        {
            db.GetUsersForOrderHistory();
        }

        public Boolean GetUserIdsWithInCompletedOrders(Int64? UserID)
        {
            var IsExist = (from c in db.Orders
                           where c.IsPaid == false && c.CreditUsed != null && c.CreditUsed != "0" && c.UserId == UserID && c.CreditUsed == "Anniversary" && c.OrderStatus == null
                           group c by c.UserId into aa
                           select aa);

            if (IsExist.ToList().Count == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Gets my pending orders.
        /// </summary>
        /// <param name="UserInfoId">The user info id.</param>
        /// <param name="SortExp">The sort exp.</param>
        /// <param name="SortOrder">The sort order.</param>
        /// <returns></returns>
        public List<GetUserPendingOrdersToApproveResult> GetMyPendingOrders(Int64 UserInfoId, Incentex.DAL.SqlRepository.AnniversaryProgramRepository.CompanyAnniversarySortExpType SortExp, DAEnums.SortOrderType SortOrder)
        {
            List<GetUserPendingOrdersToApproveResult> qry = db.GetUserPendingOrdersToApprove(UserInfoId).ToList();

            switch (SortExp)
            {
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.OrderNumber:
                    qry = qry.OrderBy(s => s.OrderNumber).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.ReferenceName:
                    qry = qry.OrderBy(s => s.ReferenceName).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.TotalAmount:
                    qry = qry.OrderBy(s => s.TotalAmount).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.PaymentMethod:
                    qry = qry.OrderBy(s => s.PaymentMethod).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.OrderDate:
                    qry = qry.OrderBy(s => s.OrderDate).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.OrderStatus:
                    qry = qry.OrderBy(s => s.OrderStatus).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.FullName:
                    qry = qry.OrderBy(s => s.UserName).ToList();
                    break;
            }
            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                qry.Reverse();
            }
            return qry;
        }

        public List<Order> GetOrderForMOASReminderSystem()
        {
            return (from order in db.Orders as IEnumerable<Order>
                    join lookup in db.INC_Lookups on order.PaymentOption equals lookup.iLookupID
                    where !String.IsNullOrEmpty(order.OrderStatus) && order.OrderStatus.ToUpper() == "ORDER PENDING"
                    && order.PaymentOption != null && lookup.sLookupName == "MOAS"
                    && order.IsPaid == true
                    && order.OrderDate.Value.AddDays(Convert.ToInt16(new AppSettingRepository().GetbyName("MOASREMINDERDAY").sSettingValue)).Date <= DateTime.Now.Date
                    select order
                    ).ToList<Order>();
        }

        public List<Order> GetInCompleteOrders()
        {
            return (from order in db.Orders
                    where order.IsPaid == false && order.OrderStatus == null
                    orderby order.OrderNumber descending
                    select order).ToList<Order>();
        }

        public List<SelectBackOrderedItemsByOrderIDResult> SelectBackOrderedItemsByOrderID(Int64 OrderID, String OrderFor)
        {
            return db.SelectBackOrderedItemsByOrderID(OrderID, OrderFor).ToList();
        }

        public UserInformation GetUserInformationByOrderID(Int64 OrderID)
        {
            return (from order in db.Orders
                    join user in db.UserInformations on order.UserId equals user.UserInfoID
                    where order.OrderID == OrderID
                    select user).FirstOrDefault();
        }

        public String TrailingNotes(Int64 ForeinKey, String NoteFor, String SpecificNoteFor, String NewLineChar)
        {
            String TrailingNotes = String.Empty;

            List<NoteDetail> objList = new List<NoteDetail>();
            objList = db.NoteDetails.Where(n => n.NoteFor == NoteFor && n.SpecificNoteFor == SpecificNoteFor).OrderByDescending(n => n.NoteID).ToList();
            if (ForeinKey != 0)
            {
                objList = objList.Where(n => n.ForeignKey == ForeinKey).ToList();
            }

            if (objList.Count > 0)
            {
                StringBuilder sbTrailingNotes = new StringBuilder();
                foreach (NoteDetail obj in objList)
                {
                    sbTrailingNotes.Append(obj.Notecontents);
                    sbTrailingNotes.Append(NewLineChar + NewLineChar);
                    UserInformationRepository objUserRepo = new UserInformationRepository();
                    UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);
                    if (objUser != null)
                    {
                        sbTrailingNotes.Append(objUser.FirstName + " " + objUser.LastName + "   ");
                    }
                    sbTrailingNotes.Append(Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy"));
                    sbTrailingNotes.Append(" @ " + Convert.ToDateTime(obj.CreateDate).ToString("HH:mm") + NewLineChar);
                    sbTrailingNotes.Append("______________________________________________________________________________");
                    sbTrailingNotes.Append(NewLineChar + NewLineChar);
                }

                TrailingNotes = sbTrailingNotes.ToString();
            }

            return TrailingNotes;
        }

        public List<SelectRecipentsForReplyTo> GetRecipentsFromOrderID(Int64 UserInfoID, Int64 AdminUserID, Int64 CreatedUserID, Int64 SupplierID)
        {
            List<SelectRecipentsForReplyTo> recipentslist = new List<SelectRecipentsForReplyTo>();

            if (SupplierID == 0)
            {
                recipentslist = (from u in db.UserInformations
                                 where u.UserInfoID == CreatedUserID || u.UserInfoID == UserInfoID
                                 select new SelectRecipentsForReplyTo
                                 {
                                     FirstName = u.FirstName,
                                     LastName = u.LastName,
                                     //Temp Code
                                     //Email = u.Email,
                                     Email = u.LoginEmail,
                                     UserInfoID = u.UserInfoID,
                                     Usertype = u.Usertype
                                 }).ToList();
                //RecepientType = 0 means Admin is selected for the customer Messge(Note) NOTE: Only for NoteType 3
                if (AdminUserID != 0)
                {
                    SelectRecipentsForReplyTo objAdminInfo = (from u in db.UserInformations
                                                              where u.UserInfoID == AdminUserID
                                                              select new SelectRecipentsForReplyTo
                                                              {
                                                                  FirstName = u.FirstName,
                                                                  LastName = u.LastName,
                                                                  //Temp Code
                                                                  //Email = u.Email,
                                                                  Email = u.LoginEmail,
                                                                  UserInfoID = u.UserInfoID,
                                                                  Usertype = 0
                                                              }).FirstOrDefault();
                    recipentslist.Add(objAdminInfo);
                }
            }
            else
            {
                recipentslist = (from u in db.UserInformations
                                 where u.UserInfoID == CreatedUserID || u.UserInfoID == SupplierID
                                 select new SelectRecipentsForReplyTo
                                 {
                                     FirstName = u.FirstName,
                                     LastName = u.LastName,
                                     Email = u.LoginEmail,
                                     UserInfoID = u.UserInfoID,
                                     Usertype = u.Usertype
                                 }).ToList();
            }
            return recipentslist;
        }

        /// <summary>
        /// added by prashant for the Orders by the number of days taken to shipped 
        /// </summary>
        /// <param name="Todate"></param>
        /// <param name="FromDate"></param>
        /// <param name="StoreId"></param>
        /// <param name="Email"></param>
        /// <param name="Ordernumber"></param>
        /// <param name="OrderStatus"></param>
        /// <param name="FName"></param>
        /// <param name="LName"></param>
        /// <param name="EmployeeCode"></param>
        /// <param name="StationCode"></param>
        /// <param name="workgroup"></param>
        /// <param name="PaymentType"></param>
        /// <param name="PAASOrderNumber"></param>
        /// <param name="UpdatedByPAAS"></param>
        /// <param name="HireDateBefore"></param>
        /// <param name="OrderPlaced"></param>
        /// <returns></returns>
        public List<SelectShippedCompleteOrdersResult> GetShippedCompleteOrders(DateTime? fromDate, DateTime? toDate, String storeID, String emailAddress, String orderNumber, String orderStatus, String firstName, String lastName, String employeeCode, String stationCode, String workGroup, String paymentType, DateTime? hireDateBefore, String orderPlaced)
        {
            return db.SelectShippedCompleteOrders(fromDate, toDate, storeID, emailAddress, orderNumber, orderStatus, firstName, lastName, employeeCode, stationCode, workGroup, paymentType, hireDateBefore, orderPlaced).ToList();
        }

        /// <summary>
        /// Get UnAssociate Order For Service Ticket 
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <returns></returns>
        public List<Order> GetUnAssociateOrderForTicket()
        {
            var result = (from ord in db.Orders
                          where ord.IsPaid == true && ord.AssociateServiceTicketID == null
                          select ord).ToList();

            return result;
        }

        public Order GetOrderByAssociateTicketID(Int64 ServiceTicketID)
        {
            var result = (from ord in db.Orders
                          where ord.AssociateServiceTicketID == ServiceTicketID
                          select ord).FirstOrDefault();

            return result;
        }

        public void UpdateOrderSetAssociateTicket(Int64 OrderID, Int64 ServiceTicketID)
        {
            var result = (from ord in db.Orders
                          where ord.OrderID == OrderID
                          select ord).FirstOrDefault();

            if (result != null)
            {
                result.AssociateServiceTicketID = ServiceTicketID;
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Get MOAS Default Price else get Product Items Price Level if MOAS Default Price is null
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <param name="ProductItemID"></param>
        /// <returns></returns>
        public GetMOASPriceByUserIDProductItemIDResult GetMOASPriceByUserIDProductItemID(Int64 UserInfoID, Int64 ProductItemID)
        {
            return db.GetMOASPriceByUserIDProductItemID(UserInfoID, ProductItemID).FirstOrDefault();
        }

        public GetOrderDetailsForSAPResult GetOrderDetailsForSAP(Int64 OrderID)
        {
            return db.GetOrderDetailsForSAP(OrderID).FirstOrDefault();
        }

        public GetOrderBillNShipDetailByOrderIDResult GetOrderBillingNShippingDetail(Int64 OrderID)
        {
            return db.GetOrderBillNShipDetailByOrderID(OrderID).FirstOrDefault();
        }

        public List<GetSuppliersForOrderResult> GetSuppliersByOrderID(Int64 OrderID, Int64 UserInfoID)
        {
            return db.GetSuppliersForOrder(OrderID, UserInfoID).ToList();
        }

        public List<GetOrderShippingDetailsResult> GetOrderShippingDetails(Int64 OrderID, Int64 UserInfoID, Int64? SupplierID)
        {
            return db.GetOrderShippingDetails(OrderID, UserInfoID, SupplierID).ToList();
        }

        public SelectOrderDetailsResult GetOrderItemsDetail(Int32 MyShoppingIssuanceCartID, Int32 SupplierID, String ShopingFor)
        {
            return db.SelectOrderDetails(MyShoppingIssuanceCartID, SupplierID, ShopingFor).FirstOrDefault();
        }
        /// <summary>
        /// Get All MOAS Order Details For CA- Approver
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="OrderFor"></param>
        /// <param name="OrderStatus"></param>
        /// <returns></returns>
        public List<GetMOASPendingOrderDetailsResult> GetMOASPendingOrderDetails(Int64 OrderID)
        {
            return db.GetMOASPendingOrderDetails(OrderID).ToList();
        }
        /// <summary>
        /// Delete Item from MOAS Order
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="cartItemID"></param>
        /// <param name="MOASApproverID"></param>
        public void DeleteItemFromMOASOrder(Int64 orderID, Int64 cartItemID, Int64 MOASApproverID)
        {
            db.DELItemFromMOASOrder(orderID, cartItemID, MOASApproverID);
        }

        /// <summary>
        /// Edit Item From MOAS Order Result
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="cartID_qty"></param>
        /// <param name="MOASApproverID"></param>
        /// <param name="NewTotalAmount"></param>
        /// <returns>OK - When Processed Successfully Else return Cart Item ID where Error Ocurr</returns>
        public EditItemFromMOASOrderResult EditItemFromMOASOrder(Int64 orderID, String cartID_qty, Int64 MOASApproverID, Decimal NewTotalAmount)
        {
            return db.EditItemFromMOASOrder(orderID, cartID_qty, MOASApproverID, NewTotalAmount).FirstOrDefault();
        }
    }
}
