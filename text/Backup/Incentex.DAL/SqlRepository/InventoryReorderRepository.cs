using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class InventoryReorderRepository : RepositoryBase
    {
        IQueryable<ProductItemInventory> GetAllQuery()
        {
            IQueryable<ProductItemInventory> qry = from C in db.ProductItemInventories
                                                   where C.IsDeleted == false
                                                   select C;
            return qry;
        }
        /// <summary>
        /// GetAllProductItemInventory()
        /// Reterieve  List  Record of ProductItemInventory table
        /// </summary>
        /// Nagmani 11/10/2010 
        public List<ProductItemInventory> GetAllProductItemInventory()
        {
            return GetAllQuery().ToList();
        }
        /// <summary>
        /// This method is used to set and get property 
        /// of field for which we show in geidview.
        /// Nagmani 08/10/2010
        /// </summary>
        public class ProductItemInventoryResult
        {
            public Int32 ProductItemInventoryID { get; set; }

            public Int32 ProductItemID { get; set; }

            public Int32 Inventory { get; set; }

            public Int32 ReOrderPoint { get; set; }

            public Int32 OnOrder { get; set; }

            public String ItemColor { get; set; }

            public String ItemSize { get; set; }

            public String ItemNumber { get; set; }

            public Int32 ShowInventoryLevelinStore { get; set; }

            public Int32 AllowOrderId { get; set; }

            public Int32 NotificationSystemId { get; set; }

            public DateTime ToArriveon { get; set; }

            public Int32 MasterStyleID { get; set; }

            public String Style { get; set; }

            public Int32 ProductStyle { get; set; }

            public String ProductstyleName { get; set; }

            public String MasterStyleName { get; set; }

            public String InventoryArriveOn { get; set; }
        }

        /// <summary>
        /// ProductItemInventoryDetails()
        /// Reterieve all the records from
        /// from the ProductItemInventroy table
        /// on iProductItemID parameter.
        /// Nagmani 08/10/2010
        /// </summary>
        /// <param name="iProductItemID"></param>
        /// <returns></returns>

        public List<ProductItemInventoryResult> ProductItemInventoryDetails(Int32 iProductId)
        {
            DateTime initialDate = Convert.ToDateTime("1900-01-01 00:00:00.000");            
            var qry = (from c in db.SelectProductItmeInventroyDetails(iProductId)
                       orderby c.ItemSize ascending
                       select new ProductItemInventoryResult
                       {
                           ProductItemInventoryID = Convert.ToInt32(c.ProductItemInventoryID),
                           ProductItemID = Convert.ToInt32(c.ProductItemID),
                           Inventory = Convert.ToInt32(c.Inventory),
                           ItemNumber = c.ItemNumber.ToString(),
                           ItemColor = c.ItemColor,
                           ItemSize = c.ItemSize,
                           ReOrderPoint = Convert.ToInt32(c.ReOrderPoint),
                           OnOrder = Convert.ToInt32(c.OnOrder),
                           ShowInventoryLevelinStore = Convert.ToInt32(c.ShowInventoryLevelInStoreID),
                           AllowOrderId = Convert.ToInt32(c.AllowBackOrderID),
                           NotificationSystemId = Convert.ToInt32(c.NotificationSystemID),
                           ToArriveon = Convert.ToDateTime(c.ToArriveOn),
                           MasterStyleID = Convert.ToInt32(c.MasterStyleID),
                           MasterStyleName = c.MasterStyleName,
                           ProductstyleName = c.ProductStyle,
                           InventoryArriveOn = c.ToArriveOn != null && c.ToArriveOn != initialDate ? Convert.ToString(c.ToArriveOn) : ""
                       }
                      );
            return (qry).ToList<ProductItemInventoryResult>();


        }
        /// <summary>
        /// GetById()
        /// Reterieve   single Reord of Productitem.
        /// </summary>
        /// Nagmani 08/10/2010
        ///<param name="ProductItemID"></param>
        public ProductItemInventory GetById(Int32 ProductItemInventoryID)
        {
            return db.ProductItemInventories.FirstOrDefault(le => le.ProductItemInventoryID == ProductItemInventoryID);
        }

        /// <summary>
        /// Delete a ProductItemInventory table record by ProductItemInventoryid
        /// </summary>
        /// <param name="ProductItemid"></param>
        public void DeleteProductItemInventory(Int64 ProductItemInventoryID, Int64? DeletedBy)
        {
            ProductItemInventory matchedInvetory = db.ProductItemInventories.FirstOrDefault(le => le.ProductItemInventoryID == ProductItemInventoryID);

            try
            {
                if (matchedInvetory != null)
                {
                    matchedInvetory.IsDeleted = true;
                    matchedInvetory.DeletedBy = DeletedBy;
                    matchedInvetory.DeletedDate = DateTime.Now;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetSingleProductItemInventory
        /// Reterieve  single Reord of ProductItemInventory table
        /// </summary>
        /// Nagmani 08/09/2010
        /// <param name="iProductItemInventoryID"></param>
        public ProductItemInventoryResult GetSingleProductItemInventory(Int32 iProductItemInventoryID, Int32 iProductItemID)
        {
            if (iProductItemInventoryID != 0)
            {
                var qury = (from stprd in db.GetTable<ProductItemInventory>()

                            join Id in db.GetTable<ProductItem>()
                            on stprd.ProductItemID equals Id.ProductItemID
                            //Start Add Nagmani 06-May-2011
                            join sp in db.GetTable<StoreProduct>()
                            on Id.ProductId equals sp.StoreProductID
                            //End Add Nagmani 06-May-2011
                            join l1 in db.GetTable<INC_Lookup>()
                            on Id.ItemColorID equals l1.iLookupID
                            join l2 in db.GetTable<INC_Lookup>()
                            on Id.ItemSizeID equals l2.iLookupID
                            join l3 in db.GetTable<INC_Lookup>()
                            on Id.ProductStyleID equals l3.iLookupID
                            where stprd.ProductItemInventoryID == iProductItemInventoryID && stprd.ProductItemID == iProductItemID
                            && stprd.IsDeleted == false && Id.IsDeleted == false && sp.IsDeleted == false
                            orderby stprd.ProductItemInventoryID ascending

                            select new ProductItemInventoryResult
                            {
                                ProductItemInventoryID = Convert.ToInt32(stprd.ProductItemInventoryID),
                                ProductItemID = Convert.ToInt32(stprd.ProductItemID),
                                Inventory = Convert.ToInt32(stprd.Inventory == null ? 0 : stprd.Inventory),
                                ItemNumber = Id.ItemNumber.ToString(),
                                ItemColor = l1.sLookupName,
                                ItemSize = l2.sLookupName,
                                ReOrderPoint = Convert.ToInt32(stprd.ReOrderPoint.ToString() == null ? 0 : stprd.ReOrderPoint),
                                OnOrder = Convert.ToInt32(stprd.OnOrder.ToString() == null ? 0 : stprd.OnOrder),
                                //Start Modify Nagmani 06-May-2011
                                //ShowInventoryLevelinStore = Convert.ToInt32(stprd.ShowInventoryLevelInStoreID.ToString() == null ? 0 : stprd.ShowInventoryLevelInStoreID),
                                //AllowOrderId = Convert.ToInt32(stprd.AllowBackOrderID.ToString() == null ? 0 : stprd.AllowBackOrderID),
                                //NotificationSystemId = Convert.ToInt32(stprd.NotificationSystemID.ToString() == null ? 0 : stprd.NotificationSystemID),
                                //ToArriveon = Convert.ToDateTime(stprd.ToArriveOn),
                                ShowInventoryLevelinStore = Convert.ToInt32(sp.ShowInventoryLevelInStoreID.ToString() == null ? 0 : sp.ShowInventoryLevelInStoreID),
                                AllowOrderId = Convert.ToInt32(sp.AllowBackOrderID.ToString() == null ? 0 : sp.AllowBackOrderID),
                                NotificationSystemId = Convert.ToInt32(sp.NotificationSystemID.ToString() == null ? 0 : sp.NotificationSystemID),
                                ToArriveon = Convert.ToDateTime(sp.ToArriveOn),
                                //End Modify Nagmani 06-May-2011
                                MasterStyleID = Convert.ToInt32(Id.MasterStyleID),
                                Style = l3.sLookupName,
                                ProductStyle = Convert.ToInt32(Id.ProductStyleID)
                            }

                        ).SingleOrDefault();
                return qury;
            }
            else if (iProductItemInventoryID == 0)
            {

                var qury1 = (from Id in db.GetTable<ProductItem>()
                             join l1 in db.GetTable<INC_Lookup>()
                             on Id.ItemColorID equals l1.iLookupID
                             join l2 in db.GetTable<INC_Lookup>()
                             on Id.ItemSizeID equals l2.iLookupID
                             join l3 in db.GetTable<INC_Lookup>()
                             on Id.ProductStyleID equals l3.iLookupID
                             where Id.ProductItemID == iProductItemID
                             && Id.IsDeleted == false

                             select new ProductItemInventoryResult
                             {
                                 ProductItemInventoryID = 0,
                                 ProductItemID = Convert.ToInt32(Id.ProductItemID),
                                 Inventory = 0,
                                 ItemNumber = Id.ItemNumber.ToString(),
                                 ItemColor = l1.sLookupName,
                                 ItemSize = l2.sLookupName,
                                 ReOrderPoint = 0,
                                 OnOrder = 0,
                                 ShowInventoryLevelinStore = 0,
                                 AllowOrderId = 0,
                                 NotificationSystemId = 0,
                                 ToArriveon = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                                 MasterStyleID = Convert.ToInt32(Id.MasterStyleID),
                                 Style = l3.sLookupName,
                                 ProductStyle = Convert.ToInt32(Id.ProductStyleID),
                             }).SingleOrDefault();

                return qury1;
            }

            return null;
        }

        public String GetProductDecription(Int32 ProdcutItmeId)
        {
            var qury1 = (from Id in db.GetTable<StoreProduct>()
                         join pi in db.GetTable<ProductItem>()
                         on Id.StoreProductID equals pi.ProductId
                         where pi.ProductId == ProdcutItmeId
                         && Id.IsDeleted == false && pi.IsDeleted == false
                         select Id.ProductDescrption
                        ).Distinct().SingleOrDefault();
            return qury1;
        }

        public void UpdateTable(Int32 Inventory, Int32 Reorder, Int64 ProductItemInventoryID, Int32 OnOrder)
        {
            db.UpdateInventoryReOrder(Inventory, Reorder, ProductItemInventoryID, OnOrder);
            db.SubmitChanges();
        }

        public void InsertTable(Int32 Inventory, Int32 Reorder, Int64 ProductItemID, Int32 OnOrder)
        {
            db.InsertInventoryReOrder(Inventory, Reorder, ProductItemID, OnOrder);
            db.SubmitChanges();
        }

        public List<SelectRemainingShippedQuantityResult> GetRemainingQut(Int32 OrderID, String itemnumber)
        {
            var qry = (from c in db.SelectRemainingShippedQuantity(OrderID, itemnumber)
                       select c).ToList();
            return qry;
        }
    }
}