using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class ProductItemPriceRepository : RepositoryBase
    {
        IQueryable<ProductItemPricing> GetAllQuery()
        {
            IQueryable<ProductItemPricing> qry = from C in db.ProductItemPricings
                                                 where C.IsDeleted == false
                                                 select C;
            return qry;
        }

        /// <summary>
        /// This method is used to set and get property 
        /// of field for which we show in geidview.
        /// Nagmani 08/10/2010
        /// </summary>
        public class ProductItemPricingResult
        {
            public System.Int32 ProductItemPricingID

            { get; set; }
            public System.Int32 ProductItemID

            { get; set; }
            public System.Decimal Level1

            { get; set; }
            public System.Decimal Level2

            { get; set; }
            public System.Decimal Level3

            { get; set; }
            public System.Decimal Level4
            { get; set; }
            public System.Decimal CloseOutPrice
            { get; set; }
            public System.Int64 Level3PricingStatus

            { get; set; }
            public System.DateTime Level3PricingStartDate

            { get; set; }
            public System.DateTime Level3PricingEndDate

            { get; set; }
            public System.Int64 Level3OrderingRuleID

            { get; set; }
            public System.Int32 Level3OnSaleFlagID

            { get; set; }
            public System.Int64 Level3PreSeasonPurchase

            { get; set; }
            public System.String ItemColor

            { get; set; }
            public System.String ItemSize

            { get; set; }
            public System.String ItemNumber

            { get; set; }
            public System.Int64 MasterStyleID

            { get; set; }
            public System.String Style

            { get; set; }
            public System.Int32 ProductStyle

            { get; set; }
            public System.String ProductstyleName

            { get; set; }
            public System.String MasterStyleName

            { get; set; }

            public System.Decimal ProductCost

            { get; set; }
            public System.Int32 Pricefor

            { get; set; }
        }
        /// <summary>
        /// ProductItemPricingDetails()
        /// Reterieve all the records from
        /// from the ProductItemPricing table
        /// on iProductId parameter.
        /// Nagmani 08/10/2010
        /// </summary>
        /// <param name="iProductItemID"></param>
        /// <returns></returns>

        public List<ProductItemPricingResult> ProductItemPricingDetails(Int32 iProductId)
        {
            return (from c in db.SelectProductItmePriceDetails(iProductId)
                    select new ProductItemPricingResult
                    {
                        ProductItemPricingID = Convert.ToInt32(c.ProductItemPricingID),
                        ProductItemID = Convert.ToInt32(c.ProductItemID),

                        Level1 = c.Level1,
                        Level2 = c.Level2,
                        Level3 = c.Level3,
                        Level4 = c.Level4,
                        CloseOutPrice = c.CloseOutPrice,
                        ItemNumber = c.ItemNumber.ToString(),
                        ItemColor = c.ItemColor,
                        ItemSize = c.ItemSize,
                        Level3PricingStatus = c.Level3PricingStatus,
                        Level3OrderingRuleID = c.Level3OrderingRuleID,
                        Level3PreSeasonPurchase = c.Level3PreSeasonPurchase,
                        MasterStyleID = c.MasterStyleID,
                        MasterStyleName = c.MasterStyleName,
                        ProductstyleName = c.ProductStyle,
                        ProductCost = c.ProductCost,
                    }).ToList<ProductItemPricingResult>();
        }

        /// <summary>
        /// Delete a ProductItemInventory table record by ProductItemInventoryid
        /// </summary>
        /// <param name="ProductItemid"></param>
        public void DeleteProductItemPricing(Int64 ProductItemPriceID, Int64? DeletedBy)
        {
            ProductItemPricing matchedPricing = db.ProductItemPricings.FirstOrDefault(le => le.ProductItemPricingID == ProductItemPriceID);

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
        /// GetSingleProductItemPrice
        /// Reterieve  single Reord of ProductItemPrice table
        /// </summary>
        /// Nagmani 08/09/2010
        /// <param name="iProductItemInventoryID"></param>
        public ProductItemPricingResult GetSingleProductItemPrice(Int32 iProductItemPriceID, Int32 iProductItemId)
        {
            if (iProductItemPriceID != 0)
            {
                var qury = (from stprd in db.GetTable<ProductItemPricing>()

                            join Id in db.GetTable<ProductItem>()
                            on stprd.ProductItemID equals Id.ProductItemID
                            join l1 in db.GetTable<INC_Lookup>()
                            on Id.ItemColorID equals l1.iLookupID
                            join l2 in db.GetTable<INC_Lookup>()
                            on Id.ItemSizeID equals l2.iLookupID
                            join l3 in db.GetTable<INC_Lookup>()
                            on Id.ProductStyleID equals l3.iLookupID
                            where stprd.ProductItemPricingID == iProductItemPriceID && stprd.ProductItemID == iProductItemId &&
                            stprd.IsDeleted == false
                            orderby stprd.ProductItemPricingID ascending

                            select new ProductItemPricingResult
                            {
                                ProductItemPricingID = Convert.ToInt32(stprd.ProductItemPricingID),
                                ProductItemID = Convert.ToInt32(stprd.ProductItemID == null ? 0 : stprd.ProductItemID),
                                Level1 = Convert.ToDecimal(stprd.Level1 == null ? 0 : stprd.Level1),
                                Level2 = Convert.ToDecimal(stprd.Level2 == null ? 0 : stprd.Level2),
                                Level3 = Convert.ToDecimal(stprd.Level3 == null ? 0 : stprd.Level3),
                                Level4 = Convert.ToDecimal(stprd.Level4 == null ? 0 : stprd.Level4),
                                CloseOutPrice = Convert.ToDecimal(stprd.CloseOutPrice == null ? 0 : stprd.CloseOutPrice),
                                ItemNumber = Id.ItemNumber.ToString(),
                                ItemColor = l1.sLookupName,
                                ItemSize = l2.sLookupName,
                                Level3PricingStatus = Convert.ToInt32(stprd.Level3PricingStatus == null ? 0 : stprd.Level3PricingStatus),
                                Level3OrderingRuleID = Convert.ToInt32(stprd.Level3OrderingRuleID == null ? 0 : stprd.Level3OrderingRuleID),
                                Level3PreSeasonPurchase = Convert.ToInt32(stprd.Level3PreSeasonPurchase == null ? 0 : stprd.Level3PreSeasonPurchase),
                                Level3OnSaleFlagID = Convert.ToInt32(stprd.Level3OnSaleFlagID == null ? 0 : stprd.Level3OnSaleFlagID),
                                Level3PricingEndDate = Convert.ToDateTime(stprd.Level3PricingEndDate == null ? new DateTime(1900, 1, 1) : stprd.Level3PricingEndDate),
                                Level3PricingStartDate = Convert.ToDateTime(stprd.Level3PricingStartDate == null ? new DateTime(1900, 1, 1) : stprd.Level3PricingStartDate),
                                MasterStyleID = Convert.ToInt32(Id.MasterStyleID == null ? 0 : Id.MasterStyleID),
                                Style = l3.sLookupName,
                                ProductStyle = Convert.ToInt32(Id.ProductStyleID == null ? 0 : Id.ProductStyleID),
                                ProductCost = Convert.ToDecimal(stprd.ProductCost == null ? 0 : stprd.ProductCost),
                                Pricefor = Convert.ToInt32(stprd.Pricefor == null ? 0 : stprd.Pricefor),
                            }).SingleOrDefault();

                return qury;
            }
            else if (iProductItemPriceID == 0)
            {
                var qury1 = (from Id in db.GetTable<ProductItem>()
                             join l1 in db.GetTable<INC_Lookup>()
                             on Id.ItemColorID equals l1.iLookupID
                             join l2 in db.GetTable<INC_Lookup>()
                             on Id.ItemSizeID equals l2.iLookupID
                             join l3 in db.GetTable<INC_Lookup>()
                             on Id.ProductStyleID equals l3.iLookupID
                             where Id.ProductItemID == iProductItemId

                             select new ProductItemPricingResult
                             {
                                 ProductItemPricingID = 0,
                                 ProductItemID = Convert.ToInt32(Id.ProductItemID == null ? 0 : Id.ProductItemID),
                                 Level1 = 0,
                                 Level2 = 0,
                                 Level3 = 0,
                                 Level4 = 0,
                                 CloseOutPrice = 0,
                                 ItemNumber = Id.ItemNumber.ToString(),
                                 ItemColor = l1.sLookupName,
                                 ItemSize = l2.sLookupName,
                                 Level3PricingStatus = 0,
                                 Level3OrderingRuleID = 0,
                                 Level3PreSeasonPurchase = 0,
                                 Level3OnSaleFlagID = 0,
                                 Level3PricingEndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                                 Level3PricingStartDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                                 MasterStyleID = Convert.ToInt32(Id.MasterStyleID == null ? 0 : Id.MasterStyleID),
                                 Style = l3.sLookupName,
                                 ProductStyle = Convert.ToInt32(Id.ProductStyleID == null ? 0 : Id.ProductStyleID),
                                 ProductCost = 0,
                                 Pricefor = 0,
                             }).SingleOrDefault();

                return qury1;

            }

            return null;
        }

        /// <summary>
        /// GetById()
        /// Reterieve   single Reord of ProductitemPricing.
        /// </summary>
        /// Nagmani 08/10/2010
        ///<param name="ProductItemID"></param>
        public ProductItemPricing GetById(Int32 ProductItemPriceID)
        {
            return db.ProductItemPricings.FirstOrDefault(le => le.ProductItemPricingID == ProductItemPriceID && le.IsDeleted == false);
        }

        public ProductItemPricing GetByProductItemId(Int32 ProductItemID)
        {
            return db.ProductItemPricings.FirstOrDefault(le => le.ProductItemID == ProductItemID && le.IsDeleted == false);
        }

        public String GetProductDecription(Int32 ProdcutItmeId)
        {
            var ProductDescription = (from n in db.GetTable<StoreProduct>()
                                      where n.StoreProductID == ProdcutItmeId
                                      select n.ProductDescrption).Distinct().SingleOrDefault();
            return ProductDescription;
        }

        public void UpdatePricelevel(Int64 storeproductid, Decimal L1Price, Decimal L2Price, Decimal L3Price, Decimal L4Price, Decimal CloseOutPrice)
        {
            db.UpdatePriceLevel(storeproductid, L1Price, L2Price, L3Price, L4Price, CloseOutPrice);
        }

        public void UpdatePricelevelShopping(Int64 storeproductid, Decimal L1Price, Decimal L2Price, Decimal L3Price, Decimal L4Price, Decimal CloseOutPrice)
        {
            db.UpdatePriceLevelShopping(storeproductid, L1Price, L2Price, L3Price, L4Price, CloseOutPrice);
        }

        public void UpdateTable(Decimal Level1, Decimal Level2, Decimal Level3, Decimal Level4, Decimal CloseOutPrice, Decimal ProductCost, Int64 ProductItemPriceID)
        {
            db.UpdateProductItemPrice(Convert.ToString(Level1), Convert.ToString(Level2), Convert.ToString(Level3), Convert.ToString(Level4), Convert.ToString(CloseOutPrice), Convert.ToString(ProductCost), ProductItemPriceID);
            db.SubmitChanges();
        }

        public void InsertDate(Decimal Level1, Decimal Level2, Decimal Level3, Decimal Level4, Decimal CloseOutPrice, Decimal ProductCost, Int64 ProductItemiD)
        {
            db.InsertProductItemPrice(Convert.ToString(Level1), Convert.ToString(Level2), Convert.ToString(Level3), Convert.ToString(Level4), Convert.ToString(CloseOutPrice), Convert.ToString(ProductCost), ProductItemiD);
            db.SubmitChanges();
        }

        /// <summary>
        /// GetSingleProductItemPrice
        /// Reterieve single Reord of ProductItemPrice table
        /// </summary>
        /// Prashant 4th Dec 2012
        /// <param name="iProductItemInventoryID"></param>
        public ProductItemPricingResult GetSingleProductItemPrice(string itemNumber, int iProductID)
        {
            if (itemNumber != "")
            {
                var qury = (from stprd in db.GetTable<ProductItemPricing>()

                            join Id in db.GetTable<ProductItem>()
                            on stprd.ProductItemID equals Id.ProductItemID 
                            join l1 in db.GetTable<INC_Lookup>()
                            on Id.ItemColorID equals l1.iLookupID
                            join l2 in db.GetTable<INC_Lookup>()
                            on Id.ItemSizeID equals l2.iLookupID
                            join l3 in db.GetTable<INC_Lookup>()
                            on Id.ProductStyleID equals l3.iLookupID
                            where Id.ItemNumber == itemNumber && Id.ProductId == iProductID
                            orderby stprd.ProductItemPricingID ascending
                            select new ProductItemPricingResult
                            {
                               
                                ProductItemPricingID = Convert.ToInt32(stprd.ProductItemPricingID),
                                ProductItemID = Convert.ToInt32(stprd.ProductItemID == null ? 0 : stprd.ProductItemID),
                                Level1 = Convert.ToDecimal(stprd.Level1 == null ? 0 : stprd.Level1),
                                Level2 = Convert.ToDecimal(stprd.Level2 == null ? 0 : stprd.Level2),
                                Level3 = Convert.ToDecimal(stprd.Level3 == null ? 0 : stprd.Level3),
                                Level4 = Convert.ToDecimal(stprd.Level4 == null ? 0 : stprd.Level4),
                                CloseOutPrice = Convert.ToDecimal(stprd.CloseOutPrice == null ? 0 : stprd.CloseOutPrice),
                                ItemNumber = Id.ItemNumber.ToString(),
                                ItemColor = l1.sLookupName,
                                ItemSize = l2.sLookupName,
                                Level3PricingStatus = Convert.ToInt32(stprd.Level3PricingStatus == null ? 0 : stprd.Level3PricingStatus),
                                Level3OrderingRuleID = Convert.ToInt32(stprd.Level3OrderingRuleID == null ? 0 : stprd.Level3OrderingRuleID),
                                Level3PreSeasonPurchase = Convert.ToInt32(stprd.Level3PreSeasonPurchase == null ? 0 : stprd.Level3PreSeasonPurchase),
                                Level3OnSaleFlagID = Convert.ToInt32(stprd.Level3OnSaleFlagID == null ? 0 : stprd.Level3OnSaleFlagID),
                                Level3PricingEndDate = Convert.ToDateTime(stprd.Level3PricingEndDate == null ? new DateTime(1900, 1, 1) : stprd.Level3PricingEndDate),
                                Level3PricingStartDate = Convert.ToDateTime(stprd.Level3PricingStartDate == null ? new DateTime(1900, 1, 1) : stprd.Level3PricingStartDate),
                                MasterStyleID = Convert.ToInt32(Id.MasterStyleID == null ? 0 : Id.MasterStyleID),
                                Style = l3.sLookupName,
                                ProductStyle = Convert.ToInt32(Id.ProductStyleID == null ? 0 : Id.ProductStyleID),
                                ProductCost = Convert.ToDecimal(stprd.ProductCost == null ? 0 : stprd.ProductCost),
                                Pricefor = Convert.ToInt32(stprd.Pricefor == null ? 0 : stprd.Pricefor),

                            }

                        ).FirstOrDefault();
                return qury;
            }
            return null;
        }

        //Get Product Pricing Level from by ProductID
        public decimal GetPriceByLevelandProductItemId(Int32 ProductItemID, Int32 PricingLevel)
        {
            ProductItemPricing objProductItemPricing = db.ProductItemPricings.FirstOrDefault(le => le.ProductItemID == ProductItemID && le.IsDeleted == false);
            Decimal Price = 0;
            if (objProductItemPricing != null)
            {
                switch (PricingLevel)
                {
                    case 1:
                        Price = Convert.ToDecimal(objProductItemPricing.Level1);
                        break;
                    case 2:
                        Price = Convert.ToDecimal(objProductItemPricing.Level2);
                        break;
                    case 3:
                        Price = Convert.ToDecimal(objProductItemPricing.Level3);
                        break;
                    default:
                        Price = Convert.ToDecimal(objProductItemPricing.Level2);
                        break;
                }

            }
            return Price;
        }
    }
}