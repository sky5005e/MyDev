using System;
using System.Collections.Generic;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class MyIssuanceCartRepository : RepositoryBase
    {
        IQueryable<MyIssuanceCart> GetAllQuery()
        {
            IQueryable<MyIssuanceCart> Query = from I in db.MyIssuanceCarts
                                               select I;
            return Query;
        }

        public MyIssuanceCart GetById(Int64 MyIssuanceCartID, Int64 UserInfoID)
        {
            return db.MyIssuanceCarts.FirstOrDefault(I => I.MyIssuanceCartID == MyIssuanceCartID && I.UserInfoID == UserInfoID);
        }

        public List<MyIssuanceCart> GetByPolicyItemId(Int64 UniformIssuancePolicyItemID, Int64 UserInfoID)
        {
            return db.MyIssuanceCarts.Where(i => i.UniformIssuancePolicyItemID == UniformIssuancePolicyItemID && i.UserInfoID == UserInfoID).ToList();
        }

        public List<MyIssuanceCart> GetByUserIdandAssociationType(Int64 UserInfoId, Int64 AssociationTypeId)
        {
            return db.MyIssuanceCarts.Where(w => w.UserInfoID == UserInfoId && w.UniformIssuanceType == AssociationTypeId).ToList();
        }

        public List<MyIssuanceCart> GetCurrentOrderProducts(String CartID)
        {
            List<MyIssuanceCart> objCart = new List<MyIssuanceCart>();
            try
            {
                foreach (String item in CartID.Split(','))
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        MyIssuanceCart objSingleProd = db.MyIssuanceCarts.FirstOrDefault(le => le.MyIssuanceCartID == Convert.ToInt64(item));
                        if (objSingleProd != null)
                            objCart.Add(objSingleProd);
                    }
                }
            }
            catch
            {
            }

            return objCart;
        }

        public List<MyIssuanceCart> GetUnOrderedCart(Int64 userInfoID)
        {
            return db.MyIssuanceCarts.Where(le => le.UserInfoID == userInfoID && le.OrderID == null && (le.OrderStatus == null || le.OrderStatus == String.Empty)).ToList();
        }

        public List<SelectMyIssuanceProductItemsResult> GetMyIssuanceProdList(Int64? orderID, Int64? userInfoID, Boolean isOrdered)
        {
            return db.SelectMyIssuanceProductItems(orderID, userInfoID, isOrdered).ToList();
        }

        public Decimal? GetPriceByMasterItemIdandItemSizeId(Int64 MasterItemID, Int64 ItemSizeID, Int64 storeproductid)
        {
            return (from pi in db.ProductItems
                    join pr in db.ProductItemPricings on pi.ProductItemID equals pr.ProductItemID
                    where pi.MasterStyleID == MasterItemID && pi.ItemSizeID == ItemSizeID && pi.ProductId == storeproductid
                    select pr.Level2).FirstOrDefault();
        }

        public List<ProductItemPricing> GetL1L2L3ByMasterItemIdandItemSizeId(Int64 MasterItemID, Int64 ItemSizeID, Int64 storeproductid)
        {
            return (from pi in db.ProductItems
                    join pr in db.ProductItemPricings on pi.ProductItemID equals pr.ProductItemID
                    where pi.MasterStyleID == MasterItemID && pi.ItemSizeID == ItemSizeID && pi.ProductId == storeproductid
                    select pr).ToList();
        }

        public List<StoreProduct> GetBackorder(Int64 MasterItemID, Int64 ItemSizeID, Int64 storeproductid)
        {
            return (
                from pi in db.StoreProducts
                join p in db.ProductItems on pi.StoreProductID equals p.ProductId
                where p.MasterStyleID == MasterItemID && p.ItemSizeID == ItemSizeID && p.ItemNumberStatusID != 136 && pi.StoreProductID == storeproductid
                select pi).ToList();
        }

        public List<ProductItemInventory> GrtBackOrder1(Int64 MasterItemID, Int64 ItemSizeID, Int64 storeproductid)
        {
            return (from pi in db.ProductItemInventories

                    join pr in db.ProductItemPricings on pi.ProductItemID equals pr.ProductItemID
                    join p in db.ProductItems on pi.ProductItemID equals p.ProductItemID
                    where p.MasterStyleID == MasterItemID && p.ItemSizeID == ItemSizeID && p.ItemNumberStatusID != 136 && p.ProductId == storeproductid
                    select pi).ToList();
        }

        public void DeleteOrderConfirmation(Int32 Orderid)
        {
            db.DeleteOrderConfirmation(Orderid);
            db.SubmitChanges();
        }

        public void DeleteMYIssuanceCart(Int32 MyIssuancecartId)
        {
            db.DeleteMyIssuanceCartId(MyIssuancecartId);
            db.SubmitChanges();
        }

        public MyIssuanceCart GetByIssuanceCartId(Int64 MyIssuanceCartID)
        {
            return db.MyIssuanceCarts.FirstOrDefault(I => I.MyIssuanceCartID == MyIssuanceCartID);
        }

        public List<GetMyIssuanceCartDetails> GetIssuancePolicyItemDetails(Int64 UniformIssuancePolicyID, Int64 UserInfoID)
        {
            var qry = (from mi in db.MyIssuanceCarts
                       join up in db.UniformIssuancePolicyItems on mi.UniformIssuancePolicyItemID equals up.UniformIssuancePolicyItemID
                       where mi.OrderStatus == "Submitted" && mi.UserInfoID == UserInfoID && mi.UniformIssuancePolicyID == UniformIssuancePolicyID
                       select new GetMyIssuanceCartDetails
                       {
                           Qty = mi.Qty,
                           Rate = mi.Rate,
                           GroupName = up.NEWGROUP,
                           UniformIssuancePolicyId = mi.UniformIssuancePolicyID,
                           UniformIssuancePolicyItemID = up.UniformIssuancePolicyItemID,
                           UserInfoID = mi.UserInfoID,
                           UniformIssuanceType = mi.UniformIssuanceType,
                           CreatedDate = mi.CreatedDate,
                           OrderStatus = mi.OrderStatus
                       }).ToList();

            return qry.ToList();
        }

        public List<GetMyIssuanceCartDetails> GetUniformIssuancePolicyDetails(Int64 UserInfoID)
        {
            var qry = (from mi in db.MyIssuanceCarts
                       join up in db.UniformIssuancePolicies on mi.UniformIssuancePolicyID equals up.UniformIssuancePolicyID
                       where mi.OrderStatus == "Submitted" && mi.UserInfoID == UserInfoID
                       select new GetMyIssuanceCartDetails
                       {
                           GroupName = up.GroupName,
                           WorkGroupID = up.WorkgroupID,
                           UniformIssuancePolicyId = mi.UniformIssuancePolicyID,
                           UserInfoID = mi.UserInfoID,
                           OrderStatus = mi.OrderStatus
                       }).Distinct();

            return qry.ToList();
        }

        public Boolean IsOrderPlacedByUser(Int64 policyID, Int64 userInfoID)
        {
            Boolean IsPlacedOrder = false;
            var qry = (from mi in db.MyIssuanceCarts
                       where mi.OrderStatus == "Submitted" && mi.UserInfoID == userInfoID && mi.UniformIssuancePolicyID == policyID
                       select mi).ToList().Distinct();
            if (qry.Count() > 0)
                IsPlacedOrder = true;
            else
                IsPlacedOrder = false;
            return IsPlacedOrder;
        }

        public class GetMyIssuanceCartDetails
        {
            public Int64 UserInfoID { get; set; }
            public Int64 WorkGroupID { get; set; }
            public Int64 UniformIssuancePolicyItemID { get; set; }
            public Int64? UniformIssuancePolicyId { get; set; }
            public Int64 UniformIssuanceType { get; set; }
            public Int32? Qty { get; set; }
            public Decimal? Rate { get; set; }
            public String GroupName { get; set; }
            public String OrderStatus { get; set; }
            public DateTime? CreatedDate { get; set; }
        }

        public List<GetUniIssPolItemsByOrderIDResult> GetUniformIssuancePolicyItemsByOrderIDForSAP(Int64 OrderID)
        {
            return db.GetUniIssPolItemsByOrderID(OrderID).ToList();
        }

        public List<MyIssuanceCart> GetIssuanceCartByOrderID(Int64 OrderID)
        {
            return db.MyIssuanceCarts.Where(le => le.OrderID == OrderID).ToList();
        }

        public List<MyIssuanceCartDetails> GetUniformIssuancePolicyDetailsByOrderID(Int64 OrderID)
        {
            var qry = (from mi in db.MyIssuanceCarts
                       join up in db.UniformIssuancePolicies on mi.UniformIssuancePolicyID equals up.UniformIssuancePolicyID
                       join uipi in db.UniformIssuancePolicyItems on mi.UniformIssuancePolicyItemID equals uipi.UniformIssuancePolicyItemID
                       join sp in db.StoreProducts on mi.StoreProductID equals sp.StoreProductID
                       join p in db.ProductItems on new { StoreProductID = (mi.StoreProductID ?? 0), mi.ItemNumber, mi.MasterItemID } equals new { StoreProductID = p.ProductId, p.ItemNumber, MasterItemID = p.MasterStyleID }
                       where mi.OrderStatus == "Submitted" && mi.OrderID == OrderID
                       select new MyIssuanceCartDetails
                       {
                           GroupName = up.GroupName,
                           WorkGroupID = sp.WorkgroupID,
                           UniformIssuancePolicyId = mi.UniformIssuancePolicyID,
                           UserInfoID = mi.UserInfoID,
                           OrderStatus = mi.OrderStatus,
                           ItemNumber = mi.ItemNumber,
                           ProductDescrption = sp.ProductDescrption,
                           Quantity = mi.Qty
                       }).Distinct();

            return qry.ToList();
        }

        public class MyIssuanceCartDetails
        {
            public Int64 UserInfoID { get; set; }
            public Int64 WorkGroupID { get; set; }
            public Int64 UniformIssuancePolicyItemID { get; set; }
            public Int64? UniformIssuancePolicyId { get; set; }
            public Int64 UniformIssuanceType { get; set; }
            public Int32? Quantity { get; set; }
            public Decimal? Rate { get; set; }
            public String GroupName { get; set; }
            public String OrderStatus { get; set; }
            public DateTime? CreatedDate { get; set; }
            public String ItemNumber { get; set; }

            //Name of this property is having a spelling mistake.
            //But its done purposefully. Don't dare to change it.
            public String ProductDescrption { get; set; }
        }

        public List<GetIssuancePolicyItemQuantityAlreadyOrderedResult> GetIssuancePolicyItemQuantityAlreadyOrdered(Int64 UserInfoID, Int64 IssuancePolicyID, Int64? OrderID)
        {
            return db.GetIssuancePolicyItemQuantityAlreadyOrdered(UserInfoID, IssuancePolicyID, OrderID).ToList();
        }

        public List<GetIssuancePolicyBySearchCriteriaResult> GetIssuancePolicyBySearchCriteria(Int64? companyID, Int64? workgroupID, Int64? garmentTypeID, Int64? IssuanceStatus, String keyWord, Int32? pageSize, Int32? pageIndex, String sortColumn, String sortDirection)
        {
            return db.GetIssuancePolicyBySearchCriteria(companyID, workgroupID, garmentTypeID, IssuanceStatus, keyWord, pageSize, pageIndex, sortColumn, sortDirection).ToList();
        }
    }
}