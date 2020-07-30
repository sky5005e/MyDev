using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class StoreProductImageRepository : RepositoryBase
    {
        IQueryable<StoreProductImage> GetAllQuery()
        {
            IQueryable<StoreProductImage> qry = from d in db.StoreProductImages
                                                where d.IsDeleted == false
                                                select d;
            return qry;
        }

        public List<StoreProductImage> GetStoreProductImagesdById(Int64 StoreProductID, Int64 MasterItemNo)
        {
            if (MasterItemNo == 0)
                return db.StoreProductImages.Where(s => s.StoreProductID == StoreProductID && s.ProductImageActive == 1 && s.IsDeleted == false).ToList();
            else
                return db.StoreProductImages.Where(s => s.StoreProductID == StoreProductID && s.StyleId == MasterItemNo && s.ProductImageActive == 1 && s.IsDeleted == false).ToList();
        }

        //IsDeleted not applicable
        public List<StoreProductImage> GetStoreProductImagesByIdForShoppingCart(Int64 StoreProductID, Int64 MasterItemNo)
        {
            if (MasterItemNo == 0)
                return db.StoreProductImages.Where(s => s.StoreProductID == StoreProductID && s.ProductImageActive == 1 && s.IsDeleted == false).ToList();
            else
                return db.StoreProductImages.Where(s => s.StoreProductID == StoreProductID && s.StyleId == MasterItemNo && s.ProductImageActive == 1 && s.IsDeleted == false).ToList();
        }

        public List<StoreProductImage> GetStoreProductImages(Int32 StoreProductId, Int32 MasterItemNo)
        {
            if (MasterItemNo == 0)
                return db.StoreProductImages.Where(s => s.StoreProductID == StoreProductId && s.IsDeleted == false).ToList();
            else
                return db.StoreProductImages.Where(s => s.StoreProductID == StoreProductId && s.StyleId == MasterItemNo && s.IsDeleted == false).ToList();
        }

        public void Delete(Int64 StoreProductImageID, Int64? DeletedBy)
        {
            StoreProductImage ProductImage = db.StoreProductImages.FirstOrDefault(le => le.StoreProductImageId == StoreProductImageID && le.IsDeleted == false);
            try
            {
                if (ProductImage != null)
                {
                    ProductImage.IsDeleted = true;
                    ProductImage.DeletedBy = DeletedBy;
                    ProductImage.DeletedDate = DateTime.Now;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(String StoreProductImageidID, Int32 StoreProductImageActive)
        {
            try
            {
                var obj = db.UpdateProductImage(Convert.ToInt32(StoreProductImageidID), StoreProductImageActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateBeforeActive(Int32 active, Int32 status, Int64 storeProductID)
        {
            try
            {
                var obj = db.UpdateBeforeActive(active, status, storeProductID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get StoreProduct Image
        /// </summary>
        /// amit
        /// <param name="StoreId"></param>
        /// <param name="MasterItemId"></param>
        /// <returns></returns>
        public List<StoreProductImage> GetStoreProductImage(Int64 StoreId, Int64 MasterItemId)
        {
            List<StoreProductImage> objStoreProductImageList = (from sp in db.StoreProducts
                                                                join si in db.StoreProductImages
                                                                on sp.StoreProductID equals si.StoreProductID
                                                                where sp.StoreId == StoreId && si.StyleId == MasterItemId && sp.IsDeleted == false && si.IsDeleted == false
                                                                select si).ToList();
            return objStoreProductImageList;
        }

        /// <summary>
        /// Nagmani Kumar 
        /// 16-Feb-2012
        /// Replacement Of GetStoreProductImage Function (Because Upper function is not suitable on store wise
        /// </summary>
        /// <param name="StoreId"></param>
        /// <param name="MasterItemId"></param>
        /// <param name="storeproductid"></param>
        /// <returns></returns>
        public List<StoreProductImage> GetStoreProductImageNew(Int64 StoreId, Int64 MasterItemId, Int64 storeproductid)
        {
            List<StoreProductImage> objStoreProductImageList = (from sp in db.StoreProducts
                                                                join si in db.StoreProductImages
                                                                on sp.StoreProductID equals si.StoreProductID
                                                                where sp.StoreId == StoreId && si.StyleId == MasterItemId && si.StoreProductID == storeproductid
                                                                && sp.IsDeleted == false && si.IsDeleted == false
                                                                select si).ToList();
            return objStoreProductImageList;
        }

        public List<INC_Lookup> GetMasterItemImage(Int64 MasterItemId)
        {
            List<INC_Lookup> objLookupImage = (from sp in db.INC_Lookups
                                               where sp.iLookupID == MasterItemId
                                               select sp).ToList();
            return objLookupImage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// Added by amit as on 10-Dec-2010
        /// <param name="CompanyId"></param>
        /// <param name="MasterItemId"></param>
        /// <returns></returns>
        public List<StoreProductImage> GetStoreProductImageByCompany(Int64 CompanyId, Int64 MasterItemId)
        {
            var qry = from si in db.StoreProductImages
                      join sp in db.StoreProducts on si.StoreProductID equals sp.StoreProductID
                      join cs in db.CompanyStores on sp.StoreId equals cs.StoreID
                      where cs.CompanyID == CompanyId && si.StyleId == MasterItemId
                      && si.IsDeleted == false && sp.IsDeleted == false
                      select si;

            return qry.ToList();
        }

        public List<SelectProductImageMasterItemStyleDuplicateResult> CheckDuplicateMasterItem(Int32? ProductID, Int32? MasterStyleId, Int32? StoreProductImageID, Int32 Storeid, String mode)
        {
            return db.SelectProductImageMasterItemStyleDuplicate(ProductID, MasterStyleId, StoreProductImageID, Storeid, mode).ToList();
        }

        public List<StoreProductImage> GetStoreProductImagesdByMasterID(Int64 MasterItemID, Int64 StoreProductID)
        {
            return db.StoreProductImages.Where(s => s.StyleId == MasterItemID && s.StoreProductID == StoreProductID && s.IsDeleted == false).ToList();
        }

        public List<StoreProductImage> GetStoreProductImagesByStoreProductId(Int64 StoreProductID)
        {
            return db.StoreProductImages.Where(s => s.StoreProductID == StoreProductID && s.IsDeleted == false).ToList();
        }

        public List<StoreProductImage> GetStoreProductImagesByStoreProductImageID(Int64 StoreProductImageID)
        {
            return db.StoreProductImages.Where(s => s.StoreProductImageId == StoreProductImageID && s.IsDeleted == false).ToList();
        }

        public List<StoreProductImage> GetStoreProductImagesdByMasterIDAndProductIamgeisActive(Int64 MasterItemID, Int64 StoreProductID)
        {
            return db.StoreProductImages.Where(s => s.StyleId == MasterItemID && s.StoreProductID == StoreProductID && s.ProductImageActive == 1 && s.IsDeleted == false).ToList();
        }
    }
}