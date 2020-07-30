using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class ProductItemDetailsRepository : RepositoryBase
    {
        IQueryable<ProductItem> GetAllQuery()
        {
            IQueryable<ProductItem> qry = from C in db.ProductItems
                                          where C.IsDeleted == false
                                          select C;
            return qry;
        }

        public List<ProductItem> GetAllProductItem(Int32 iMasteritemId)
        {
            return db.ProductItems.Where(C => C.MasterStyleID == iMasteritemId && C.IsDeleted == false).ToList();
        }

        /// <summary>
        /// Get All Product Item by iMasteritemId, StoreProductID
        /// </summary>
        /// <param name="iMasteritemId"></param>
        /// <param name="storeproductID"></param>
        /// <returns></returns>
        public List<ProductItem> GetAllProductItem(Int32 iMasteritemId, Int64 storeproductID)
        {
            return db.ProductItems.Where(C => C.MasterStyleID == iMasteritemId && C.ProductId == storeproductID && C.IsDeleted == false).ToList();
        }
        /// <summary>
        /// Get Size based on StoreProductID
        /// </summary>
        /// <param name="storeProductID"></param>
        /// <returns></returns>
        public List<SizeItemResult> GetALLSize(Int64 storeProductID)
        {
            // only active for active
            var listSize = (from pi in db.ProductItems
                            join ls in db.INC_Lookups on pi.ItemSizeID equals ls.iLookupID
                            where pi.ItemNumberStatusID == 135 && pi.ProductId == storeProductID && pi.IsDeleted == false
                            select new SizeItemResult
                            {
                                ItemSizeID = ls.iLookupID,
                                SizeName = ls.sLookupName
                            }).ToList<SizeItemResult>();
            return listSize;

        }

        public class SizeItemResult
        {
            public Int64 ItemSizeID { get; set; }
            public String SizeName { get; set; }
        }


        /// <summary>
        /// This method is used to set and get property 
        /// of field for which we show in geidview.
        /// Nagmani 08/10/2010
        /// </summary>
        public class ProductItemResult
        {
            public System.Int32 ProductItemId

            { get; set; }
            public System.Int32 ProductId

            { get; set; }
            public System.String ProductStyle

            { get; set; }
            public System.String ItemNumber

            { get; set; }
            public System.String ItemColor

            { get; set; }
            public System.String ItemSize

            { get; set; }
            public System.Int32 MasterStyleID

            { get; set; }
            public System.String MasterStyleName

            { get; set; }
            public System.Int32 ItemNumberStatusID

            { get; set; }
            public System.String IconPath

            { get; set; }
            public System.String ItemImage

            { get; set; }

            public System.Int32 SizePriority

            { get; set; }

            public String ItemQuantity { get; set; }

            public String ItemPrice { get; set; }

        }

        /// <summary>
        /// Reterieve all the records from
        /// from the storeproduct table
        /// on storeid parameter.
        /// Nagmani 08/10/2010
        /// </summary>
        /// <returns></returns>
        public List<ProductItemResult> ProductItemDetails(Int32 iStoreProduct)
        {
            return (from stprd in db.GetTable<ProductItem>()
                    join l1 in db.GetTable<INC_Lookup>()
                    on stprd.ItemColorID equals l1.iLookupID
                    join l2 in db.GetTable<INC_Lookup>() on stprd.ItemSizeID equals l2.iLookupID into ank
                    from l2 in ank.DefaultIfEmpty()
                    join l3 in db.GetTable<INC_Lookup>()
                    on stprd.ProductStyleID equals l3.iLookupID
                    join l4 in db.GetTable<INC_Lookup>() on stprd.Soldby equals l4.iLookupID into N
                    from l4 in N.DefaultIfEmpty()
                    join l5 in db.GetTable<INC_Lookup>()
                    on stprd.ProductStyleID equals l5.iLookupID
                    join l6 in db.GetTable<INC_Lookup>()
                    on stprd.MasterStyleID equals l6.iLookupID
                    join l7 in db.GetTable<INC_Lookup>()
                    on stprd.ItemNumberStatusID equals l7.iLookupID
                    where stprd.ProductId == iStoreProduct && stprd.IsDeleted == false
                    orderby stprd.ProductItemID ascending
                    select new ProductItemResult
                    {
                        ProductItemId = Convert.ToInt32(stprd.ProductItemID),
                        ProductId = Convert.ToInt32(stprd.ProductId),
                        ItemImage = stprd.ItemImage,
                        ProductStyle = l3.sLookupName,
                        ItemNumber = stprd.ItemNumber,
                        ItemColor = l1.sLookupName,
                        ItemSize = l2 == null ? "(No Size)" : l2.sLookupName,
                        MasterStyleID = Convert.ToInt32(l6.iLookupID),
                        MasterStyleName = l6.sLookupName,
                        ItemNumberStatusID = Convert.ToInt32(stprd.ItemNumberStatusID),
                        IconPath = l7.sLookupIcon,
                        SizePriority = Convert.ToInt32(stprd.SizePriority),
                    }).ToList<ProductItemResult>();
        }

        /// <summary>
        /// GetById()
        /// Reterieve   single Reord of Productitem.
        /// </summary>
        /// Nagmani 08/10/2010
        ///<param name="ProductItemID"></param>
        public ProductItem GetById(Int64 ProductItemID)
        {
            return db.ProductItems.FirstOrDefault(C => C.ProductItemID == ProductItemID && C.IsDeleted == false);
        }

        public ProductItem GetByProductId(Int32 ProductId)
        {
            return GetSingle(db.ProductItems.Where(C => C.ProductId == ProductId).ToList());
        }
        /// <summary>
        /// Get Product Item Details By StoreProductID
        /// </summary>
        /// <param name="StoreProductID"></param>
        /// <returns></returns>
        public List<GetProductItemDetailsByStoreProductIDResult> GetProductItemDetailsByStoreProductID(Int64 StoreProductID, Int64 UserInfoID)
        {
            return db.GetProductItemDetailsByStoreProductID(StoreProductID, UserInfoID).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StoreProductID"></param>
        /// <param name="ItemSizeID"></param>
        /// <returns></returns>
        public List<GetSoldByDetailsResult> GetSoldByDetails(Int64 StoreProductID, Int64 ItemSizeID)
        {
            return db.GetSoldByDetails(StoreProductID, ItemSizeID).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="SubCategoryID"></param>
        /// <param name="GarmentType"></param>
        /// <param name="UserTypeID"></param>
        /// <param name="EmployeeTypeID"></param>
        /// <param name="ClimateSettingID"></param>
        /// <returns></returns>
        public List<GetStoreProductsForUserResult> GetStoreProductsForUser(Int64 CompanyID, Int64 SubCategoryID, String GarmentType, Int64? UserTypeID, Int64? EmployeeTypeID, Int64? ClimateSettingID)
        {
            return db.GetStoreProductsForUser(CompanyID, SubCategoryID, GarmentType, UserTypeID, EmployeeTypeID, ClimateSettingID).ToList();
        }

        public List<GetUserProductCategoryAccessResult> GetUserProductCategoryAccess(Int64 UserInfoID)
        {
            return db.GetUserProductCategoryAccess(UserInfoID).ToList();
        }
        /// <summary>
        /// Delete a ProductItem table record by ProductItemID
        /// </summary>
        /// <param name="ProductItemid"></param>
        public void DeleteProductItem(Int64 ProductItemID, Int64? DeletedBy)
        {

            ProductItem objItem = db.ProductItems.FirstOrDefault(le => le.ProductItemID == ProductItemID && le.IsDeleted == false);
            try
            {
                if (objItem != null)
                {
                    // To delete records from ProductItemPricing
                    DeleteProductItemPricing(objItem.ProductItemID, DeletedBy);

                    //Delete Product Item Inventory
                    DeleteProductItemInventory(objItem.ProductItemID, DeletedBy);

                    objItem.IsDeleted = true;
                    objItem.DeletedBy = DeletedBy;
                    objItem.DeletedDate = DateTime.Now;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DeleteProductItemInventory(Int64 ProductItemId, Int64? DeletedBy)
        {
            ProductItemInventory matchedInvetory = db.ProductItemInventories.FirstOrDefault(le => le.ProductItemID == ProductItemId);

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

        private void DeleteProductItemPricing(Int64 ProductItemId, Int64? DeletedBy)
        {
            ProductItemPricing matchedPricing = db.ProductItemPricings.FirstOrDefault(le => le.ProductItemID == ProductItemId);

            try
            {
                if (matchedPricing != null)
                {
                    matchedPricing.IsDeleted = true;
                    matchedPricing.DeletedBy = DeletedBy;
                    matchedPricing.DeletedDate = DateTime.Now;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetProductItemDetails()
        /// Reterieve  single Reord of ProductItem table
        /// using the parameter
        /// </summary>
        /// Nagmani 08/09/2010
        /// <param name="iProductItemID"></param>
        /// <param name="itemNumber"></param>
        public ProductItemResult GetProductItemDetails(Int32 iProductStyleID, String itemNumber)
        {
            return (from e in db.GetTable<ProductItem>()
                    join l1 in db.GetTable<INC_Lookup>()
                    on e.ItemColorID equals l1.iLookupID
                    join l2 in db.GetTable<INC_Lookup>()
                    on e.ItemSizeID equals l2.iLookupID
                    where (e.MasterStyleID == iProductStyleID && e.ItemNumber == itemNumber && e.IsDeleted == false)
                    select new ProductItemResult
                    {
                        ItemColor = l1.sLookupName,
                        ItemSize = l2.sLookupName
                    }).SingleOrDefault();
        }

        public List<SelectStyleNumberResult> CheckStyle(Int32? ProductID, Int32? MasterStyleId, Int32? ProductItemID, String mode)
        {
            return db.SelectStyleNumber(ProductID, MasterStyleId, ProductItemID, mode).ToList();
        }

        /// <summary>
        /// CheckItemNuberDuplication()
        /// Check the itemmumber duuplication
        /// on the style id , productitemid and productid
        /// using the parameter
        /// </summary>
        /// Nagmani 08/09/2010
        /// <param name="MasterStyleId"></param>
        /// <param name="mode"></param>
        /// <param name="ProductId"></param>
        /// <param name="ProductItmeId"></param>
        /// <param name="ItemNumber"></param>
        public Int32 CheckDuplicate(String ItemNumber, Int32? ProductItmeId, Int32? ProductId, Int32? MasterStyleId, Int32? ProductStyleID, String mode)
        {
            return (Int32)db.INC_ItemNumberDuplicate(ItemNumber, ProductItmeId, ProductId, MasterStyleId, ProductStyleID, mode).SingleOrDefault().IsDuplicate;
        }

        public List<SelectMasterItemStyleDuplicateResult> CheckDuplicateMasterItem(Int32? ProductID, Int32? MasterStyleId, Int32? ProductItemID, String mode)
        {
            return db.SelectMasterItemStyleDuplicate(ProductID, MasterStyleId, ProductItemID, mode).ToList();
        }

        public ProductItem GetStoreProductMasterItemNo(Int32 StoreProductId)
        {
            return GetSingle(db.ProductItems.Where(C => C.ProductId == StoreProductId).ToList());
        }

        //IsDeleted not applicable
        public ProductItem GetRecord(Int64 StoreProductId, Int64 MasterStyleId, String ItemNumber)
        {
            return GetSingle(db.ProductItems.Where(C => C.ProductId == StoreProductId && C.MasterStyleID == MasterStyleId && C.ItemNumber == ItemNumber).ToList());
        }

        public List<SelectProductIDResult> GetProductId(Int64 MasterStyleId, Int64 MyIssuanceCartID, Int64 @ItemSizeId, Int64 StoreId)
        {
            return db.SelectProductID(MasterStyleId, @ItemSizeId, StoreId, MyIssuanceCartID).ToList();
        }

        public void UpdateProductItemsDetailsByMasterItemStyle(Int32? productID, Int32? masterStyleID, Int32? productStyleID, Int32? oldproductStyleID)
        {
            try
            {
                db.UpdateProductItemsStyleByMasterStyleID(productID, masterStyleID, productStyleID, oldproductStyleID);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :" + ex.ToString());
            }
        }

        public void UpdateProductItemsSizeOrderPriority(Int32 productItemId, Int32 sizePriority)
        {
            try
            {
                db.UpdateProductItemsSizePriority(productItemId, sizePriority);
            }
            catch (Exception ex)
            {
                Console.Write("Error : " + ex.ToString());
            }
        }

        public Int64 GetMasterItemByItemNumber(String itemNumber)
        {
            return (GetAllQuery().Where(a => a.ItemNumber == itemNumber).SingleOrDefault()).MasterStyleID;
        }

        /// <summary>
        /// Nagmani Kumar
        /// </summary>
        /// <param name="itemNumber"></param>
        /// <param name="Sizeid"></param>
        /// <param name="Colorid"></param>
        /// <returns></returns>
        public Int64 GetMasterItemByItemNumberNew(String itemNumber, Int32 Sizeid, Int32 Colorid, Int32 ProductItemID)
        {
            return (GetAllQuery().Where(a => a.ItemNumber == itemNumber && a.ItemSizeID == Sizeid && a.ItemColorID == Colorid && a.ProductItemID == ProductItemID).SingleOrDefault()).MasterStyleID;
        }

        public ProductItem GetAllRecordByItemNumber(String itemNumber)
        {
            return GetSingle(GetAllQuery().Where(a => a.ItemNumber == itemNumber).ToList());
        }

        public List<ItemNumberClass> GetAllRelatedItems(Int32 iMasteritemId)
        {
            return (from a in db.ProductItems
                    join b in db.INC_Lookups on a.MasterStyleID equals b.iLookupID
                    where (a.MasterStyleID == iMasteritemId)
                    select new ItemNumberClass
                    {
                        _productitemid = a.ProductItemID,
                        _itemnumber = a.ItemNumber
                    }).ToList<ItemNumberClass>();
        }

        public class ItemNumberClass
        {
            public Int64 _productitemid { get; set; }
            public String _itemnumber { get; set; }
        }

        /// <summary>
        /// Update Itemno Status
        /// </summary>
        /// <param name="ItemProductiD"></param>
        /// <param name="ItemnoStatusID"></param>
        public void UpdateStatus(Int32 ProductItemiD, Int32 ItemNoStatusID)
        {
            db.Update_ItemNumberStatus(ProductItemiD, ItemNoStatusID);
        }

        /// <summary>
        /// Get Product ItemId By ItemNumber
        /// </summary>
        /// <param name="itemNumber"></param>
        /// <returns></returns>
        public Int64 GetProductItemIdByItemNumber(String itemNumber, Int64 StoreID, Int64 WorkgroupID, Int64 SupplierID)
        {
            try
            {
                return (from pi in db.ProductItems
                        join sp in db.StoreProducts on pi.ProductId equals sp.StoreProductID
                        where sp.StoreId == StoreID && sp.WorkgroupID == WorkgroupID && sp.SupplierId == SupplierID && pi.ItemNumber == itemNumber &&
                        pi.IsDeleted == false
                        select pi).SingleOrDefault().ProductItemID;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        /// <summary>
        /// Update Inventory By Magaya
        /// </summary>
        /// <param name="ProductItemiD"></param>
        /// <param name="Inventory"></param>
        public void UpdateInventoryByMagaya(Int32 ProductItemId, Int32 Inventory)
        {
            db.UpDateInventorybyMagaya(ProductItemId, Inventory);
        }

        public List<ProductItemInventory> GetProductItemInventoryByItemNumberAndSupplierID(String ItemNumber, Int64 SupplierID)
        {
            return (from pi in db.ProductItems
                    join sp in db.StoreProducts on pi.ProductId equals sp.StoreProductID
                    join piInventory in db.ProductItemInventories on pi.ProductItemID equals piInventory.ProductItemID
                    where sp.SupplierId == SupplierID && pi.ItemNumber.ToUpper() == ItemNumber.ToUpper() && pi.IsDeleted == false
                    select piInventory).ToList();
        }        

        public class ProductSearchResult
        {
            public Int64 StoreProductID { get; set; }
            public String MasterItemNumber { get; set; }
            public String ItemNumber { get; set; }
            public String ProductDescription { get; set; }
            public Decimal? Level1 { get; set; }
            public Decimal? Level2 { get; set; }
            public Decimal? Level3 { get; set; }
            public Decimal? Level4 { get; set; }
            public Int64 StoreID { get; set; }
            public String WorkgroupName { get; set; }
            public String StoreName { get; set; }

            public Decimal? Closeout { get; set; }
            public DateTime? DelieveryDate { get; set; }
            public Int32? OnOrder { get; set; }
            public Int32? OnHand { get; set; }
            public String AllowBackOrder { get; set; }
            public String Vendor { get; set; }
            public Int32? MonthUsage { get; set; }
        }

        public List<GetSearchProductAsPerReportResult> GetProductSearchResult(Int64? storeID, Int64? workgroupID, String MasterItemNumber, String ItemNumber, Decimal? Price, String Description, Int32 reportFlag)
        {
            return db.GetSearchProductAsPerReport(storeID, workgroupID, MasterItemNumber, ItemNumber, Price, Description, reportFlag).ToList();            
        }

        public Int32 GetMonthUsage(Int64 MasterStyleID, Int64 StoreProductID, String ItemNumber)
        {
            return ((from order in db.Orders
                     join myShop in db.MyShoppinCarts on order.OrderID equals myShop.OrderID
                     where order.OrderStatus.ToUpper() != "CANCELED" && order.OrderStatus.ToUpper() != "DELETED" && order.OrderStatus.ToUpper() != "ORDER PENDING" && order.IsPaid == true
                     && order.OrderFor == "ShoppingCart" && myShop.ItemNumber == ItemNumber && myShop.MasterItemNo == MasterStyleID && myShop.StoreProductID == StoreProductID
                     select new { CartID = myShop.MyShoppingCartID }).
                            Union(from order in db.Orders
                                  join myIssuance in db.MyIssuanceCarts on order.OrderID equals myIssuance.OrderID
                                  where order.OrderStatus.ToUpper() != "CANCELED" && order.OrderStatus.ToUpper() != "DELETED" && order.OrderStatus.ToUpper() != "ORDER PENDING" && order.IsPaid == true
                                  && order.OrderFor == "IssuanceCart" && myIssuance.ItemNumber == ItemNumber && myIssuance.MasterItemID == MasterStyleID && myIssuance.StoreProductID == StoreProductID
                                  select new { CartID = myIssuance.MyIssuanceCartID })).Count();

        }

        /// <summary>
        /// Get AllowBackOrderID from StoreProduct masterlavel
        /// </summary>
        /// <param name="StoreProductID"></param>
        /// <returns></returns>
        public Int64 GetProductBackOrderId(Int64 StoreProductID)
        {
            return (from s in db.StoreProducts where s.StoreProductID == StoreProductID select s.AllowBackOrderID.Value).SingleOrDefault(); 
        }

        /// <summary>
        /// Get AllowBackOrderID from ProductItem  itemlavel
        /// </summary>
        /// <param name="StoreProductID"></param>
        /// <param name="ItemNumber"></param>
        /// <returns></returns>
        public Int64? GetProductItemBackOrderId(Int64 StoreProductID, String ItemNumber)
        {
            return (from i in db.ProductItems
                    where i.ProductId == StoreProductID && i.ItemNumber == ItemNumber
                    && i.IsDeleted == false
                    select i.AllowBackOrderID).SingleOrDefault();  
        }

         /// <summary>
        /// Ckeck back order funcationality at master lavel as well as Item lavel
        /// </summary>
        /// <param name="AllowBackOrderID"></param>
        /// <param name="ItemAllowBackOrderID"></param>
        /// <param name="intInventory"></param>
        /// <param name="intQuantity"></param>
        /// <returns></returns>
        public Boolean CheckAllowBackOrderID(Int64 AllowBackOrderID, Int64 ItemAllowBackOrderID, Int32 intInventory, Int32 intQuantity)
        {
            LookupRepository objLookupRepo = new LookupRepository();
            INC_Lookup objLook = new INC_Lookup();
            Boolean result = true;

            objLook = objLookupRepo.GetById(AllowBackOrderID);
            if (objLook != null && intInventory < intQuantity && objLook.Val1 == "Backorder's set at item level") //Check at master level
            {
                objLook = ItemAllowBackOrderID > 0 ? objLookupRepo.GetById(ItemAllowBackOrderID) : new INC_Lookup();
                if (objLook.sLookupName == null || objLook.Val1 == "No") //Check at Item level
                {
                    result = false;
                }
            }
            else if (intInventory < intQuantity && objLook.Val1 == "No") //Check at master level
            {
                result = false;
            }

            return result;
        }

        public string GetStatusForAllowBackOrderAndInventoryLevel(Int64 EnteredQuantity, Int64 ProductItemID, String returnmessage)
        {
            db.CheckForAllowBackOrderAndInventoryLevel(EnteredQuantity, ProductItemID, ref returnmessage);
            return returnmessage;
        }
    }
}