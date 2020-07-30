using System;
using System.Collections.Generic;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{

    public class StoreProductRepository : RepositoryBase
    {
        public List<StoreProduct> GetAllStoreProduct(Int64 iStoreID)
        {
            return db.StoreProducts.Where(le => le.StoreId == iStoreID && le.IsDeleted == false).ToList();
        }

        public StoreProduct GetById(Int64 StoreProductID)
        {
            return db.StoreProducts.FirstOrDefault(le => le.StoreProductID == StoreProductID && le.IsDeleted == false);
        }


        /// <summary>
        /// Get StoreproductID
        /// </summary>
        /// Surendra 
        /// <param name="CategoryID"></param>
        /// <param name="SubCategoryID"></param>
        /// <param name="WorkGroupID"></param>
        /// <returns></returns>
        public List<StoreProduct> GetStoreProducts(Int64 CategoryID, Int64 SubCategoryID, Int64? WorkGroupID)
        {
            if (WorkGroupID != null && WorkGroupID != 0)
            {
                return db.StoreProducts.Where(le => le.CategoryID == CategoryID && le.SubCategoryID == SubCategoryID && le.WorkgroupID == WorkGroupID && le.IsDeleted == false).ToList();
            }
            else
            {
                return db.StoreProducts.Where(le => le.CategoryID == CategoryID && le.SubCategoryID == SubCategoryID && le.IsDeleted == false).ToList();
            }
        }

        public List<Select_StoreProductDetailsResult> StoreProductDetails(Int64 StoreID, Int64 WorkGroupID)
        {
            return db.Select_StoreProductDetails(StoreID, WorkGroupID).ToList();            
        }

        /// <summary>
        /// Delete a Storeproduct table record by StroeProdcutID
        /// </summary>
        /// <param name="customerID"></param>
        public void DeleteStoreProduct(Int64 StoreProductID, Int64? DeletedBy)
        {
            StoreProduct matchedProduct = db.StoreProducts.FirstOrDefault(le => le.StoreProductID == StoreProductID);

            try
            {
                if (matchedProduct != null)
                {
                    // Delete product details
                    DeleteProductItemDetails(StoreProductID, DeletedBy);
                    // Delete Product Images
                    DeleteStoreProductImagesById(StoreProductID, DeletedBy);
                    // Delete Tailoring and Measurement chart
                    DeleteStoreProductTailoringById(StoreProductID, DeletedBy);

                    matchedProduct.IsDeleted = true;
                    matchedProduct.DeletedBy = DeletedBy;
                    matchedProduct.DeletedDate = DateTime.Now;
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

        private void DeleteProductItemDetails(Int64 StoreProductID, Int64? DeletedBy)
        {
            List<ProductItem> lstItems = db.ProductItems.Where(p => p.ProductId == StoreProductID).ToList();

            foreach (ProductItem Item in lstItems)
            {
                // To delete records from ProductItemPricing
                DeleteProductItemPricing(Item.ProductItemID, DeletedBy);

                //Delete Product Item Inventory
                DeleteProductItemInventory(Item.ProductItemID, DeletedBy);

                // Delete Product Items
                Item.IsDeleted = true;
                Item.DeletedBy = DeletedBy;
                Item.DeletedDate = DateTime.Now;
            }

            if (lstItems.Count > 0)
                db.SubmitChanges();
        }

        private void DeleteStoreProductImagesById(Int64 StoreProductID, Int64? DeletedBy)
        {
            List<StoreProductImage> lstProductImages = db.StoreProductImages.Where(le => le.StoreProductID == StoreProductID).ToList();

            foreach (StoreProductImage ProductImage in lstProductImages)
            {
                ProductImage.IsDeleted = true;
                ProductImage.DeletedBy = DeletedBy;
                ProductImage.DeletedDate = DateTime.Now;            
            }

            if (lstProductImages.Count > 0)
                db.SubmitChanges();
        }

        private void DeleteStoreProductTailoringById(Int64 StoreProductID, Int64? DeletedBy)
        {

            ProductItemTailoringMeasurement objTailoring = db.ProductItemTailoringMeasurements.FirstOrDefault(le => le.StoreProductID == StoreProductID);

            if (objTailoring != null)
            {
                objTailoring.IsDeleted = true;
                objTailoring.DeletedBy = DeletedBy;
                objTailoring.DeletedDate = DateTime.Now;
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Get all MasterItems By StoreId
        /// </summary>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        public List<INC_Lookup> GetAllMasterItems(Int64? StoreID)
        {
            List<StoreProduct> result = db.StoreProducts.ToList();
            if (StoreID != null)
                result = result.Where(x => x.StoreId == StoreID).ToList();

            List<Int64> StoreMasterItemIdList = (from sp in result
                                                 join pi in db.ProductItems
                                                 on sp.StoreProductID equals pi.ProductId
                                                 where pi.IsDeleted == false
                                                 select pi.MasterStyleID).Distinct().ToList();

            List<INC_Lookup> MasterItems = (from l in db.INC_Lookups
                                            where StoreMasterItemIdList.Contains(l.iLookupID)
                                            select l).ToList();

            return MasterItems;
        }

        /// <summary>
        /// Get only active MasterItems By StoreId
        /// </summary>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        public List<INC_Lookup> GetActiveMasterItems(Int64? StoreID)
        {
            List<StoreProduct> result = db.StoreProducts.ToList();
            if (StoreID != null)
                result = result.Where(x => x.StoreId == StoreID).ToList();

            List<Int64> StoreMasterItemIdList = (from sp in result
                                                 join pi in db.ProductItems
                                                 on sp.StoreProductID equals pi.ProductId
                                                 where sp.StatusID != 136 && pi.ItemNumberStatusID != 136 && pi.IsDeleted == false
                                                 select pi.MasterStyleID).Distinct().ToList();

            List<INC_Lookup> MasterItems = (from l in db.INC_Lookups
                                            where StoreMasterItemIdList.Contains(l.iLookupID)
                                            select l).ToList();

            return MasterItems;
        }

        /// <summary>
        /// UpdateStatus a status
        /// </summary>
        /// <param name="StoreProductID"></param>
        /// <param name="StatusID"></param>
        public void UpdateStatus(Int32 StoreProductID, Int64 StatusID)
        {
            db.Update_StoreStatus(StoreProductID, StatusID);
        }

        public List<SP_GetStoreProductForBulkOrderResult> GetStoreProductItemsForBulkOrder(Int64 CompanyID, Int64 WorkGroupID)
        {
            return db.SP_GetStoreProductForBulkOrder(CompanyID, WorkGroupID).Where(le => le.Status == "Active" && le.MasterItemNo != null).ToList();
        }

        public Int64? GetStoreProductSupplierID(Int64 StoreProductID)
        {
            return db.StoreProducts.FirstOrDefault(le => le.StoreProductID == StoreProductID && le.IsDeleted == false).SupplierId;
        }


        public List<SP_GetStoreProductResult> GetStoreProductItems(Int64 CompanyID, Int64 WorkGroupID, Int64? UserInfoID)
        {
            return db.SP_GetStoreProduct(CompanyID, WorkGroupID, UserInfoID).Where(le => le.Status == "Active").ToList();
        }
        /// <summary>
        /// To get all StoreProductID,ProductDescrption, Summary,  CategoryName, SubCategoryName
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="workGroupId"></param>
        /// <returns></returns>
        public List<DataExcel> GetStoreIDforDataExcel(Int64 storeID, Int64 workGroupId)
        {
            if (workGroupId == 0)// when no workgroup is selected.
            {
                return (from s in db.StoreProducts
                        join c in db.Categories on s.CategoryID equals c.CategoryID
                        join sc in db.SubCategories on s.SubCategoryID equals sc.SubCategoryID
                        where s.StoreId == storeID && s.IsDeleted == false
                        select new DataExcel
                            {
                                StoreProductID = s.StoreProductID,
                                ProductDescrption = s.ProductDescrption,
                                Summary = s.Summary,
                                CategoryName = c.CategoryName,
                                SubCategoryName = sc.SubCategoryName
                            }).ToList<DataExcel>();
            }
            else
            {
                return (from s in db.StoreProducts
                        join c in db.Categories on s.CategoryID equals c.CategoryID
                        join sc in db.SubCategories on s.SubCategoryID equals sc.SubCategoryID
                        where s.StoreId == storeID && s.WorkgroupID == workGroupId && s.IsDeleted == false
                        select new DataExcel
                        {
                            StoreProductID = s.StoreProductID,
                            ProductDescrption = s.ProductDescrption,
                            Summary = s.Summary,
                            CategoryName = c.CategoryName,
                            SubCategoryName = sc.SubCategoryName
                        }).ToList<DataExcel>();
            }

        }
        /// <summary>
        ///  TO display records in excel
        /// </summary>
        public class DataExcel
        {
            public Int64 StoreProductID { get; set; }
            public String ProductDescrption { get; set; }
            public String Summary { get; set; }
            public String CategoryName { get; set; }
            public String SubCategoryName { get; set; }
        }

        public List<SP_GetStoreProductBySubCatIDResult> GetStoreProductBySubCatID(Int64 CompanyId, Int64? SubCatId)
        {
            return db.SP_GetStoreProductBySubCatID(CompanyId, SubCatId).ToList();
        }

        /// <summary>
        /// Shehzad 17-Jan-2011
        /// Gets all new Products
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="WorkGroupID"></param>
        /// <returns>All the new products</returns>
        public List<SP_GetStoreProductResult> GetNewStoreProductItems(Int64 CompanyID, Int64 WorkGroupID, Int64? UserInfoID)
        {
            return db.SP_GetStoreProduct(CompanyID, WorkGroupID, UserInfoID).Where(s => s.Status == "Active" && s.NewProductUntil >= DateTime.Now).ToList();
        }

        /// <summary>
        /// Ankit 25-Jan-2011
        /// Gets all Sale Products
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="WorkGroupID"></param>
        /// <returns>All the sale products</returns>
        public List<SP_GetStoreProductResult> GetSaleStoreProductItems(Int64 CompanyID, Int64 WorkGroupID, Int64? UserInfoID)
        {
            LookupRepository objLookupRepo = new LookupRepository();

            return db.SP_GetStoreProduct(CompanyID, WorkGroupID, UserInfoID).Where(s => s.Status == "Active" && s.Level3PricingStatus != null 
                && objLookupRepo.GetById((Int64)(s.Level3PricingStatus)).sLookupName.ToLower() == "active"
                && s.Level3PricingStartDate <= DateTime.Now.Date && s.Level3PricingEndDate >= DateTime.Now.Date).ToList();
        }

        public List<SelectGetStoreIdResult> GetAllStoreID()
        {   
            return db.SelectGetStoreId().ToList();
        }
    }
}