using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class ShipOrderRepository : RepositoryBase
    {
        public CountTotalShippedQuantiyResult GetQtyShippedTotal(Int32 MyShoppingCartID, String ItemNumber, Int64? OrderID)
        {
            return db.CountTotalShippedQuantiy(MyShoppingCartID, ItemNumber, OrderID).FirstOrDefault();
        }

        public Boolean IsOrderShippedComplete(Int64 OrderID)
        {
            return Convert.ToBoolean(db.FUN_IsOrderShippedCompleteByOrderID(OrderID));
        }

        public List<SelectShippedQuantityOrderResult> GetShippedQtyOrderDetails(String ShippID, Int64 OrderID)//,Int32 MyShoppingCartID)
        {
            return db.SelectShippedQuantityOrder(ShippID, OrderID).ToList();//, MyShoppingCartID).ToList();
        }

        public List<SelectShippedQuantityResult> GetShippedQuantity(Int64 OrderID)//,Int32 MyShoppingCartID)
        {
            return db.SelectShippedQuantity(OrderID).ToList();//, MyShoppingCartID).ToList();
        }

        public List<GetOrderPastShipmentDetailsResult> GetOrderPastShipments(Int64 OrderID, Int64? UserInfoID)
        {
            return db.GetOrderPastShipmentDetails(OrderID, UserInfoID).ToList();
        }

        public List<GetShippedOrderQuantityResult> GetShippeQuantityForPackingSlip(Int64 OrderID, String PackageID, Int64 UserInfoID)
        {
            return db.GetShippedOrderQuantity(OrderID, PackageID, UserInfoID).ToList();
        }

        public List<SelectShippedQuantityResult> GetShippedQuantityBySupplier(Int64 OrderID, Int64 SupplierID)//,Int32 MyShoppingCartID)
        {
            return db.SelectShippedQuantityBySupplier(SupplierID, OrderID).ToList();//, MyShoppingCartID).ToList();
        }

        public List<SelectSizeColorDescriptionResult> GetSizeColorDetails(Int32 MyShoppingCartID, Int32 OrderId, String OrderFor, String ItemNumber)//,Int32 MyShoppingCartID)
        {
            return db.SelectSizeColorDescription(MyShoppingCartID, OrderId, OrderFor, ItemNumber).ToList();//, MyShoppingCartID).ToList();
        }

        IQueryable<ShipingOrder> GetAllQuery()
        {
            IQueryable<ShipingOrder> qry = from C in db.ShipingOrders
                                           select C;
            return qry;
        }

        public ShipingOrder GetById(Int32 Shippid)
        {
            return db.ShipingOrders.FirstOrDefault(C => C.ShippID == Shippid);
        }

        public List<ShipingOrder> GetByPackageId(String PackageId)
        {
            return db.ShipingOrders.Where(C => C.PackageId == PackageId).ToList();
        }

        public void DeleteOrderShipped(Int32 ShippId)
        {
            var matchedStore = (from c in db.GetTable<ShipingOrder>()
                                where c.ShippID == ShippId
                                select c).SingleOrDefault();
            try
            {
                if (matchedStore != null)
                {
                    db.ShipingOrders.DeleteOnSubmit(matchedStore);
                }

                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpDateShipOrdered(DateTime ShipDate, String TrackingNO, Int64 ShipperService, Int64 shippId)
        {
            db.UpdateShipOrder(ShipDate, TrackingNO, ShipperService, shippId);
        }

        public List<ShipingOrder> GetShippingOrders(Int64 OrderId, Int64 ShoppingCartId, String ItemNumber)
        {
            return db.ShipingOrders.Where(C => C.OrderID == OrderId && C.MyShoppingCartiD == ShoppingCartId && C.ItemNumber == ItemNumber).ToList();
        }

        public List<ShipingOrder> GetAllShippOrder(Int64 OrderID)
        {
            return db.ShipingOrders.Where(C => C.OrderID == OrderID).ToList();
        }

        public List<ShipingOrder> CheckReturnStatus(Int64 OrderID)
        {
            return db.ShipingOrders.Where(C => C.OrderID == OrderID).ToList();
        }

        public List<ShipingOrder> GetShippAll(Int64 ShippID)
        {
            return db.ShipingOrders.Where(C => C.ShippID == ShippID).ToList();
        }

        public List<ShippedReturnProductDescriptionResult> GetDesCription(Int32 OrderID, Int32 MyShoppingCartID, String ItemNumber)
        {
            return db.ShippedReturnProductDescription(OrderID, MyShoppingCartID, ItemNumber).ToList();
        }

        public void UpDateShipOrderReturnStatus(Int32 shippId, String ReturnStatus)
        {
            db.UPdateShippOrderReturnStatus(shippId, ReturnStatus);
        }

        public List<ShipingOrder> GetOrderSummary(Int64 OrderID)
        {
            return db.ShipingOrders.Where(le => le.OrderID == OrderID).ToList();
        }

        public List<SelectNotShippedOrdersByOrderResult> GetSelectNotShippedOrdersByOrder(Int64 OrderID, Int64 SupplierId)//,Int32 MyShoppingCartID)
        {
            List<SelectNotShippedOrdersByOrderResult> objOrderDoc = new List<SelectNotShippedOrdersByOrderResult>();
            if (SupplierId == 0)
            {
                objOrderDoc = db.SelectNotShippedOrdersByOrder(OrderID, null).ToList();
            }
            else
            {
                objOrderDoc = db.SelectNotShippedOrdersByOrder(OrderID, SupplierId).ToList();
            }
            return objOrderDoc;
        }

        public List<ShipingOrder> GetShippingOrdersItemWise(Int64 OrderId, Int64 ShoppingCartId)
        {
            return db.ShipingOrders.Where(C => C.OrderID == OrderId && C.MyShoppingCartiD == ShoppingCartId && C.IsShipped == true).ToList();
        }

        public List<ShipingOrder> CheckIfCargoAlreadyProcess(Int64 OrderId, Int64 ShoppingCartId, String TrackingNumber)
        {
            return db.ShipingOrders.Where(s => s.OrderID == OrderId && s.MyShoppingCartiD == ShoppingCartId && s.TrackingNo == TrackingNumber).ToList();
        }
    }
}