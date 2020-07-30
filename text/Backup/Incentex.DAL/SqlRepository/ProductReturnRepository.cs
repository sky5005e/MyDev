using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
   public class ProductReturnRepository:RepositoryBase 
    {
       IQueryable<ReturnProduct> GetAllQuery()
       {
           IQueryable<ReturnProduct> qry = from C in db.ReturnProducts
                                          select C;
           return qry;
       }
       public List<ReturnProduct> GetByUserId(Int64 UserID)
       {
          // return GetAllQuery().Where(C => C.CreatedBy == UserID).ToList();
           return GetAllQuery().Where(C => C.CreatedBy == UserID).ToList();

       }
       public List<ReturnProduct> GetByOrderID(Int64 OrderID)
       {
           return GetAllQuery().Where(C => C.OrderId == OrderID).ToList();

       }
       public List<ReturnProduct> GetByProductReturnID(Int64 ProductReturnID)
       {
           return GetAllQuery().Where(C => C.ProductReturnId == ProductReturnID).ToList();

       }
       
       public ReturnProduct GetById(Int64 ProductReturnID)
       {
           ReturnProduct objNew = GetSingle(GetAllQuery().Where(C => C.ProductReturnId == ProductReturnID).ToList());

           return objNew;
       }
       public List<ReturnProductCompanyListResult> GetProductReturnCompany()
       {
           List<ReturnProductCompanyListResult> objList = new List<ReturnProductCompanyListResult>();
           return objList = db.ReturnProductCompanyList().ToList();

       }
       //Start Old Function Change Nagmani 27-04-2011
       //public List<SelectReturnProductOnCompWiseResult> GetProductReturnProduct(Int64 CompanyID)
       //{
       //    List<SelectReturnProductOnCompWiseResult> objList = new List<SelectReturnProductOnCompWiseResult>();
       //    return objList = db.SelectReturnProductOnCompWise(CompanyID).ToList();

       //}
       //New Function
       public List<SelectReturnProductOnCompWiseResult> GetProductReturnProduct(DateTime ? Todate, DateTime ? FromDate, string StoreId, string Email, string Ordernumber, string OrderStatus)
       {
           List<SelectReturnProductOnCompWiseResult> objList = new List<SelectReturnProductOnCompWiseResult>();
           return objList = db.SelectReturnProductOnCompWise(FromDate, Todate, StoreId, Email, Ordernumber, OrderStatus).ToList();
       }

       public List<ReturnProductItemsDetails> GetReturnProductList(String OrderNumber, String FirstName, String LastName, DateTime? FromDate, DateTime? ToDate)
       {
           var qry = (from rp in db.ReturnProducts
                      join so in db.ShipingOrders on rp.ShippId equals so.ShippID
                      join ord in db.Orders on rp.OrderId equals ord.OrderID
                      join ui in db.UserInformations on rp.CreatedBy equals ui.UserInfoID
                      join com in db.Companies on ui.CompanyId equals com.CompanyID
                      where ui.IsDeleted == false
                      select new ReturnProductItemsDetails
                      {
                          ProductReturnID = rp.ProductReturnId,
                          OrderID = rp.OrderId,
                          ShippID = rp.ShippId,
                          ReturnStatus = rp.Status,
                          ContactName = ui.FirstName + " " + ui.LastName,
                          FirstName = ui.FirstName,
                          LastName = ui.LastName,
                          CompanyName = com.CompanyName,
                          UserInfoID = ui.UserInfoID,
                          OrderNumber = ord.OrderNumber,
                          SubmitDate = rp.SubmitDate
                      }).ToList();

           if (!String.IsNullOrEmpty(OrderNumber))
           {
               qry = qry.Where(q => q.OrderNumber.Contains(OrderNumber)).ToList();
           }
           if (!String.IsNullOrEmpty(FirstName))
           {
               qry = qry.Where(q => q.FirstName.ToLower().Contains(FirstName.ToLower())).ToList();
           }
           if (!String.IsNullOrEmpty(LastName))
           {
               qry = qry.Where(q => q.LastName.ToLower().Contains(LastName.ToLower())).ToList();
           }
           if (ToDate != null && FromDate !=null)
           {
               qry = qry.Where(q => q.SubmitDate.Value.Date <= ToDate.Value.Date && q.SubmitDate.Value.Date >= FromDate.Value.Date).ToList();
           }
           return qry.ToList<ReturnProductItemsDetails>().OrderByDescending(q=>q.SubmitDate).ToList();

       }

       public List<GetReturnOrderDetailsResult> GetReturnOrderDetails(Int64 OrderId)
       {
           List<GetReturnOrderDetailsResult> objList = new List<GetReturnOrderDetailsResult>();
           return objList = db.GetReturnOrderDetails(OrderId).ToList();
       }
       //End Function Change Nagmani 27-04-2011
       public List<ReturnProductDetailsOnOrderIDResult> GetProductReturnOnOrderID(Int64 OrderID)
       {
           List<ReturnProductDetailsOnOrderIDResult> objList = new List<ReturnProductDetailsOnOrderIDResult>();
           return objList = db.ReturnProductDetailsOnOrderID(OrderID).ToList();
       }

       public List<Order>GetOrderDetail(Int64 OrderId)
       {
           var data = db.Orders.Where(o => o.OrderID == OrderId).ToList();
           return data;
          
       }

       public void UpdateStatus(Int64 OrderID, string Status)
       {
           db.UpdateProductReturnStaus(OrderID, Status);
           db.SubmitChanges();
       }

       public List<OrderReturnDetails> GetOrdersGroupByOrderId(Int64 OrderId)
       {
           List<OrderReturnDetails> ObjReturnProducts = new List<OrderReturnDetails>();
            var data = from s in db.ReturnProducts join
                       r in db.Orders on s.OrderId equals r.OrderID 
                      where s.OrderId==OrderId
                      group s by new { s.SubmitDate, s.OrderId,r.OrderNumber } into pgroup
                      select new { count = pgroup.Count(), pgroup.Key.OrderId, pgroup.Key.SubmitDate,pgroup.Key.OrderNumber };

           foreach(var d in data)
           {
               OrderReturnDetails ObjReturnProduct = new OrderReturnDetails();
               ObjReturnProduct.OrderID=Convert.ToInt64( d.OrderId);
               ObjReturnProduct.ProductDescription = d.SubmitDate.ToString();
               ObjReturnProduct.ItemNumber ="RA"+ d.OrderNumber;
               ObjReturnProducts.Add(ObjReturnProduct);
           }
           return ObjReturnProducts;
       }



       public List<ReturnProduct> GetOrdersGroupByOrderIdSubmiDate(Int64 OrderId,DateTime Date)
       {          
           List<ReturnProduct> ObjReturnProducts = (from s in db.ReturnProducts   
                      where s.OrderId == OrderId && s.SubmitDate == Date
                      select s).ToList();
          
           return ObjReturnProducts;
       }

       public List<OrderReturnDetails> GetAllReturnProductListByOrderID(Int64 OrderID)
       {
           var qry = (from rp in db.ReturnProducts
                      join so in db.ShipingOrders on rp.OrderId equals so.OrderID
                      where rp.OrderId == OrderID
                      select new OrderReturnDetails
                      {
                          ItemNumber = rp.ItemNumber,
                          MyShoppingCartID = so.MyShoppingCartiD,
                          OrderID = rp.OrderId,
                          OrderStatus = rp.Status,
                          ProductReturnId = rp.ProductReturnId,
                          Quantity = so.QtyOrder.ToString(),
                          ReturnQty = rp.ReturnQty.ToString(),
                          ReasonCode = rp.ReturnStatusId,
                          ReturnStatusId = rp.ReturnStatusId,
                          ShippID = rp.ShippId,
                          ShippedQty = so.ShipQuantity.ToString()

                      }).ToList();
           return qry.ToList<OrderReturnDetails>();
       }
       public List<OrderReturnDetails> GetReturnProductListByOrderID(Int64 OrderID)
       {
           var qry = (from rp in db.ReturnProducts
                      where rp.OrderId == OrderID
                      select new OrderReturnDetails
                      {
                          ItemNumber = rp.ItemNumber,
                          MyShoppingCartID = rp.MyShoppingCartID,
                          OrderID = rp.OrderId,
                          OrderStatus = rp.Status,
                          ProductReturnId = rp.ProductReturnId,
                          ReturnQty = rp.ReturnQty.ToString(),
                          ReasonCode = rp.ReturnStatusId,
                          ReturnStatusId = rp.ReturnStatusId,
                          ShippID = rp.ShippId,

                      }).ToList();
           return qry.ToList<OrderReturnDetails>();
       }
       public List<OrderReturnDetails> GetAllShippingOrderDetaislByOrderID(Int64 OrderID)
       {
           var qry = (from so in db.ShipingOrders 
                      where so.OrderID == OrderID
                      select new OrderReturnDetails
                      {
                          ItemNumber = so.ItemNumber,
                          MyShoppingCartID = so.MyShoppingCartiD,
                          OrderID = so.OrderID,
                          OrderStatus = null,
                          Quantity = so.QtyOrder.ToString(),
                          ReasonCode = null,
                          ShippID = so.ShippID,
                          ShippedQty = so.ShipQuantity.ToString()

                      }).ToList();
           return qry.ToList<OrderReturnDetails>();
       }

       [Serializable]
       public class OrderReturnDetails
       {
           public Int64? ReasonCode { get; set; }
           public Int64? MyShoppingCartID { get; set; }
           public Int64? UserInfoID { get; set; }
           public Int64? OrderID { get; set; }
           public Int64? StoreProductID { get; set; }
           public String ProductDescription { get; set; }
           public String OrderStatus { get; set; }
           public String Size { get; set; }
           public String Quantity { get; set; }
           public String ShippedQty { get; set; }
           public String ReturnQty { get; set; }
           public String ItemNumber { get; set; }
           public Int64? ShippID { get; set; }
           public Int64 ProductReturnId { get; set; }
           public Int64? ReturnStatusId { get; set; }
       }
       public class ReturnProductItemsDetails
       {
           public Int64 ProductReturnID { get; set; }
           public Int64? OrderID { get; set; }
           public String ReturnStatus { get; set; }
           public Int64? ShippID { get; set; }         
           public Int64? UserInfoID { get; set; }
           public String CompanyName { get; set; }
           public String FirstName { get; set; }
           public String LastName { get; set; }
           public String ContactName { get; set; }
           public String OrderNumber { get; set; }
           public DateTime? SubmitDate { get; set; }
       }

    }
}
