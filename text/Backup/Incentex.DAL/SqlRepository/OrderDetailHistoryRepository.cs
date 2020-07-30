using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class OrderDetailHistoryRepository : RepositoryBase
    {
        IQueryable<OrderDetailPageHistory> GetAllQuery()
        {
            IQueryable<OrderDetailPageHistory> qry = from a in db.OrderDetailPageHistories
                                                     select a;
            return qry;
        }
        public OrderDetailPageHistory GetByOrderId(Int64 OrderId)
        {
            return GetAllQuery().Where(a => a.OrderId == OrderId).SingleOrDefault();
        }
        public OrderDetailPageHistory GetByUserId(Int64 UserId, Int64 OrderId)
        {
            return GetAllQuery().Where(a => a.UserId == UserId && a.OrderId == OrderId).SingleOrDefault();
        }

        public List<OrderDetails> GetOrderDetailsByOrderID(Int64 OrderID)
        {
            return (from ms in db.MyShoppinCarts
                    join ord in db.Orders on ms.OrderID equals ord.OrderID
                    where ord.OrderID == OrderID
                    select new OrderDetails
                    {
                        MyShoppingCartID = ms.MyShoppingCartID,
                        UserInfoID = ms.UserInfoID,
                        OrderID = ord.OrderID,
                        StoreProductID = ms.StoreProductID,
                        ProductDescription = ms.ProductDescrption,
                        OrderStatus = ord.OrderStatus,
                        Size = ms.Size,
                        Quantity = ms.Quantity,
                        ItemNumber = ms.ItemNumber
                    }).ToList();
        }

        public List<OrderUserInfo> GetOrderUserInfoByOrderID(Int64 orderID)
        {
            return (from ord in db.Orders
                    join ui in db.UserInformations on ord.UserId equals ui.UserInfoID
                    where ord.OrderID == orderID
                    select new OrderUserInfo
                    {
                        OrderID = ord.OrderID,
                        UserInfoId = ui.UserInfoID,
                        FirstName = ui.FirstName,
                        LastName = ui.LastName,
                        OrderNumber = ord.OrderNumber
                    }).ToList();
        }

        public List<OrderDetailPageHistory> GetAllOrders()
        {
            return GetAllQuery().ToList();
        }
        public List<ListUsers> GetAllUsersAccessingTheOrders(Int64 OrderId)
        {
            return (from a in db.OrderDetailPageHistories
                    join u in db.UserInformations on a.UserId equals u.UserInfoID
                    where a.UserId != null && a.OrderId == OrderId && u.IsDeleted == false
                    select new ListUsers
                     {
                         userid = u.UserInfoID,
                         username = u.FirstName + " " + u.LastName,
                         orderid = (long)a.OrderId
                     }).ToList<ListUsers>();
        }
        public class ListUsers
        {
            public string username { get; set; }
            public Int64 userid { get; set; }
            public Int64 orderid { get; set; }
        }
        [Serializable]
        public class OrderDetails
        {
            public Int64 MyShoppingCartID { get; set; }
            public Int64? UserInfoID { get; set; }
            public Int64 OrderID { get; set; }
            public Int64? StoreProductID { get; set; }
            public String ProductDescription { get; set; }
            public String OrderStatus { get; set; }
            public String Size { get; set; }
            public String Quantity { get; set; }
            public String ItemNumber { get; set; }

        }

        public class OrderUserInfo
        {
            public Int64 UserInfoId { get; set; }
            public String FirstName { get; set; }
            public String LastName { get; set; }
            public Int64 OrderID { get; set; }
            public String OrderNumber { get; set; }

        }

        [Serializable]
        public class OrderItemDetails
        {
            public Int64 MyShoppingCartID { get; set; }
            public Int64? UserInfoID { get; set; }
            public Int64 OrderID { get; set; }
            public Int64? StoreProductID { get; set; }
            public String ProductDescription { get; set; }
            public String OrderStatus { get; set; }
            public String Size { get; set; }
            public String Quantity { get; set; }
            public String ItemNumber { get; set; }
            public long? MasterItemNumber { get; set; }
            public long? PaymentOptions { get; set; }
            public String UnitPrice { get; set; }
            public int? PriceLevel { get; set; }
            public decimal? CorporateDiscount { get; set; }

        }
        //added by Prashant - 4th Dec 2012
        public List<MyShoppinCart> GetShoppinCartDetailsByOrderID(Int64 OrderID)
        {
            return (from ms in db.MyShoppinCarts
                        join ord in db.Orders on ms.OrderID equals ord.OrderID
                        where ord.OrderID == OrderID
                        select ms).ToList();
        }
        public List<MyIssuanceCart> GetIssuanceCartDetailsByOrderID(Int64 OrderID)
        {
            return (from ms in db.MyIssuanceCarts
                    join ord in db.Orders on ms.OrderID equals ord.OrderID
                    join lncSize in db.INC_Lookups on ms.ItemSizeID equals lncSize.iLookupID
                    where ord.OrderID == OrderID
                    select ms).ToList();
        }
    }
}

