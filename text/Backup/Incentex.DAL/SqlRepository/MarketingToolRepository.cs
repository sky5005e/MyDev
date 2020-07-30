using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class MarketingToolRepository: RepositoryBase
    {
        //public Int64? CheckDiscountValue(Int64 companyStoreID, Int64? workgroupID, Int64? priceLevelID)
        //{
        //    GlobalPricingDiscount objDiscount = new GlobalPricingDiscount();
        //    int Count = 0;            
        //    objDiscount = (from m in db.GetTable<GlobalPricingDiscount>()
        //                   where m.StoreID == companyStoreID && DateTime.Now >= m.StartDate && DateTime.Now <= m.EndDate
        //                   select m).FirstOrDefault();
        //    if (objDiscount != null)
        //    {
        //        if (objDiscount.WorkgroupID != null)
        //        {
        //            if (objDiscount.WorkgroupID == workgroupID)
        //                Count = 1;
        //            if (objDiscount.PriceLevel != null)
        //            {
        //                if (objDiscount.PriceLevel == priceLevelID)
        //                    Count = 1;
        //            }
        //        }
        //        else if (objDiscount.PriceLevel != null)
        //        {
        //            if (objDiscount.PriceLevel == priceLevelID)
        //                Count = 1;
        //        }
        //        if (Count == 1)
        //            return objDiscount.Discount;
        //        else
        //            return 0;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        public class GetItemListResult
        {
            public string Text { get; set; }
            public Int64? Value { get; set; }
        }
        public List<GetItemListResult> GetItemList(Int64 workgroupID,Int64 companyStoreID)
        {
            var result = (from pi in db.ProductItems
                          join sp in db.StoreProducts on pi.ProductId equals sp.StoreProductID
                          where sp.WorkgroupID==workgroupID && sp.StoreId==companyStoreID && sp.StatusID==135 
                          && sp.IsDeleted==false && pi.IsDeleted==false
                          select new GetItemListResult
                          {
                             Text=pi.ItemNumber,
                             Value=pi.ProductItemID
                          }).ToList();
            return result;
        }

        public Int64? CheckDiscountValue(Int64 companyStoreID, Int64? workgroupID, Int64? priceLevelID)
        {
            Int64? discount=0;
            try
            {
                List<GlobalPricingDiscount> lstDiscount = new List<GlobalPricingDiscount>();
                int Count = 0;
                lstDiscount = (from m in db.GetTable<GlobalPricingDiscount>()
                               where m.StoreID == companyStoreID && DateTime.Today >= m.StartDate && DateTime.Today <= m.EndDate
                               select m).ToList();
                if (lstDiscount.Count != 0)
                {
                    foreach (var item in lstDiscount)
                    {
                        if (item.WorkgroupID != null)
                        {
                            if (item.WorkgroupID == workgroupID)
                            {
                                if (item.PriceLevel != null)
                                {
                                    if (item.PriceLevel == priceLevelID)
                                        Count = 1;
                                    else
                                        continue;
                                }
                                Count = 1;
                            }
                        }
                        else if (item.PriceLevel != null)
                        {
                            if (item.PriceLevel == priceLevelID)
                                Count = 1;
                        }
                        else
                        {//For All WorkgroupID & PriceLevel
                            discount = item.Discount;
                            break;
                        }
                        if (Count == 1)
                        {
                            discount = item.Discount;
                            break;
                        }
                    }
                }              

            }
            catch (Exception)
            { return 0; }
            return discount;
        }

        public Int64 GetPriceLevelID(String PriceLevelName)
        {
            Int64 PriceLevelID = 0;
            INC_Lookup objInc_lookup = new INC_Lookup();
            objInc_lookup = (from m in db.GetTable<INC_Lookup>()
                             where m.sLookupName == PriceLevelName && m.iLookupCode=="PriceLevel"
                             select m).FirstOrDefault();
            if (objInc_lookup!=null)
            {
                PriceLevelID = objInc_lookup.iLookupID;
            }
            return PriceLevelID;
        }

        public Decimal GetPriceAfterDiscount(String price, Int64 discount)
        {
            Decimal DiscountOffered = Convert.ToDecimal(price) * discount / 100;
            Decimal DiscountedPrice = Convert.ToDecimal(price) - DiscountOffered;
            return DiscountedPrice;
        }

        public bool CheckForL3(Int64? storeProductID, String itemNumber)
        {
            ProductItemPricing objL3 = new ProductItemPricing();
            Int64 ProductItemID = 0;
            var lstProdItem = (from m in db.GetTable<ProductItem>()
                               where m.ItemNumber == itemNumber && m.ProductId == storeProductID && m.IsDeleted == false
                               select m).FirstOrDefault();
            if (lstProdItem != null)
                ProductItemID = lstProdItem.ProductItemID;

            if (ProductItemID != 0)
            {
                objL3 = (from m in db.GetTable<ProductItemPricing>()
                         where m.ProductItemID == ProductItemID && DateTime.Now >= m.Level3PricingStartDate && DateTime.Now <= m.Level3PricingEndDate && m.IsDeleted == false
                         select m).FirstOrDefault();
            }
            if (objL3 != null)
                return true;
            else
                return false;



        }

        public Int64 GetProductItemID(Int64? storeProductID, String itemNumber)
        {
            Int64 ProductItemID = 0;
            var lstProdItem = (from m in db.GetTable<ProductItem>()
                               where m.ItemNumber == itemNumber && m.ProductId == storeProductID && m.IsDeleted == false
                               select m).FirstOrDefault();
            if (lstProdItem != null)
                ProductItemID = lstProdItem.ProductItemID;
            return ProductItemID;
        }

        public String CheckPromotionCode(Int64 companyStoreID, Int64? workgroupID, Int64? priceLevelID, Int64? productItemID)
        {           
            try
            {
                List<PromotionCodeDetail> lstPromotion = new List<PromotionCodeDetail>();              
                lstPromotion = (from m in db.GetTable<PromotionCodeDetail>()
                               where m.StoreID == companyStoreID && DateTime.Now >= m.StartDate && DateTime.Now <= m.EndDate
                               select m).ToList();
                if (lstPromotion.Count != 0)
                {
                    foreach (var item in lstPromotion)
                    {
                        if (item.WorkgroupID != null)
                        {
                            if (item.WorkgroupID == workgroupID)
                            {
                                if (item.PriceLevel != null)
                                {
                                    if (item.PriceLevel == priceLevelID)
                                    {
                                        if (item.ProductItemID!=null)
                                        {
                                            if (item.ProductItemID==productItemID)
                                            {
                                                 return item.PromotionCode;
                                            }
                                        }
                                        else
                                        {
                                            return item.PromotionCode;
                                        }
                                    }
                                }
                                else
                                {
                                    return item.PromotionCode;
                                }
                            }
                        }
                        else
                        {
                            return item.PromotionCode;
                        }
                       
                    }
                    return null;
                }
                else
                {
                    return null;
                }               

            }
            catch (Exception)
            { return null; }
           
        }

        public StoreProduct GetByStoreProductID(Int64? storeProductID)
        {
            return db.StoreProducts.FirstOrDefault(x => x.StoreProductID == storeProductID);
        }
    }
}
