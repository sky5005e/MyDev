using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class MyShoppingCartRepository : RepositoryBase
    {
        IQueryable<MyShoppinCart> GetAllQuery()
        {
            IQueryable<MyShoppinCart> qry = from C in db.MyShoppinCarts
                                            select C;
            return qry;
        }

        public MyShoppinCart GetById(Int64 MyShoppingCartID, Int64 UserInfoID)
        {
            return db.MyShoppinCarts.FirstOrDefault(C => C.MyShoppingCartID == MyShoppingCartID && C.UserInfoID == UserInfoID);
        }

        public MyShoppinCart GetDetailsById(Int64 MyShoppingCartID)
        {
            return db.MyShoppinCarts.FirstOrDefault(C => C.MyShoppingCartID == MyShoppingCartID);
        }

        public List<MyShoppinCart> GetPendingCartItemsList(Int64 UserInfoID)
        {
            return db.MyShoppinCarts.Where(MSC => MSC.UserInfoID == UserInfoID && MSC.IsOrdered == false).ToList();
        }

        /// <summary>
        /// Changed by shehzad 5-jan-2011
        /// Gets only those items that are unordered
        /// </summary>
        /// <param name="WorkgroupId"></param>
        /// <param name="UserInfoId"></param>
        /// <returns></returns>
        public List<SelectMyShoppingCartProductResult> SelectShoppingProduct(Int64 userInfoID)
        {
            return db.SelectMyShoppingCartProduct(null, userInfoID, false).ToList();
        }
        /// <summary>
        /// Get MyCart Item Details By OrderID Result
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="OrderFor"></param>
        /// <param name="OrderStatus"></param>
        /// <returns></returns>
        public List<GetMyCartItemDetailsByOrderIDResult> GetMyCartItemDetailsByOrderID(Int64 OrderID, String OrderFor, String OrderStatus)
        {
            return db.GetMyCartItemDetailsByOrderID(OrderID, OrderFor, OrderStatus).ToList();
        }
        /// <summary>
        /// Get Total Amount of Pending Items in Cart by UserInfoID
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <param name="IsOrdered"></param>
        /// <returns></returns>
        public Decimal GetTotalAmountofPendingItemsinCart(Int64 UserInfoID, Boolean IsOrdered)
        {
            return Convert.ToDecimal(db.GetTotalPendingCartItemAmount(UserInfoID, IsOrdered).FirstOrDefault().TotalAmount);
        }
        public void UpdateMyShoppingCartForCoupaOrder(Int64 MyShoppingCartID)
        {
            MyShoppinCart obj = new MyShoppinCart();
            obj = GetDetailsById(MyShoppingCartID);
            if (obj != null)
            {
                obj.IsCoupaOrderSubmitted = true;
                this.SubmitChanges();
            }
        }

        public List<SelectMyShoppingCartProductForCoupaResult> SelectMyShoppingCartProductForCoupa(Int64 UserInfoID, String BuyerCookie)
        {
            try
            {
                return db.SelectMyShoppingCartProductForCoupa(null, Convert.ToString(UserInfoID), null,BuyerCookie, false).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Shehzad 5-Jan-2011
        /// Gets All Unordered Item from My Shopping Cart table for User
        /// </summary>
        /// <param name="UserInfoID">User Id</param>
        /// <returns>List of My Shopping Card Ids</returns>
        public List<MyShoppinCart> GetUnorderedCartIdsByUser(Int64 UserInfoID)
        {
            return db.MyShoppinCarts.Where(U => U.UserInfoID == UserInfoID && U.IsOrdered == false).ToList();
        }
        /// <summary>
        /// Gets Shopping Cart Items based on MyShoppingCartId
        /// </summary>
        /// <param name="CartID"></param>
        /// <returns>May return Single or Multiple Record</returns>
        public List<SelectMyShoppingCartProductResult> SelectCurrentOrderProducts(Int64 orderID)
        {
            return db.SelectMyShoppingCartProduct(orderID, null, true).ToList();
        }

        public int CheckDuplicate(int? StoreProductId, string Size, Int64 UserInfoID, String soldByName)
        {
            return (int)db.SelectMyShoppingProductDuplication(StoreProductId, Size, UserInfoID, soldByName).FirstOrDefault().IsDuplicate;
        }

        public int CheckDuplicateForCoupa(int? StoreProductId, string Size, Int64 UserInfoID, String soldByName,String BuyerCookie)
        {
            var qry = (from ms in db.MyShoppinCarts
                       where ms.StoreProductID == StoreProductId && ms.Size == Size && ms.UserInfoID == UserInfoID &&
                       ms.SoldbyName == soldByName && ms.BuyerCookie == BuyerCookie && ms.IsCoupaOrderSubmitted == false
                       select ms).ToList();
            if (qry.Count > 0)
                return 1;
            else
                return 0;
        }

        public int RemoveProductsFromCart(Int64 ShoppingCartId, Int64 UserInfoId)
        {
            try
            {
                MyShoppinCart objCart = GetById(ShoppingCartId, UserInfoId);
                Delete(objCart);
                SubmitChanges();
                return 0;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return -1;
            }
        }

        public List<SelectCompanyPending> GetCompanyPendingShoppingCart(Boolean status)
        {
            List<SelectCompanyPending> qry = db.MyShoppinCarts.Where(model => model.IsOrdered == status).Join(
               db.UserInformations.Where(model => model.WLSStatusId == 135 && model.IsDeleted == false).Join(
               db.Companies, UserInfo => UserInfo.CompanyId, CompanyInfo => CompanyInfo.CompanyID,
               (UserInfo, CompanyInfo) => new { UserInfo, CompanyInfo }), ShoppingInfo => ShoppingInfo.UserInfoID,
               Group => Group.UserInfo.UserInfoID,
               (ShoppingInfo, UserInfo) => new { ShoppingInfo, UserInfo })
               .GroupBy(model => model.UserInfo.CompanyInfo.CompanyName)
                   .Select(model => new SelectCompanyPending()
                   {
                       CompanyID = model.Select(x => x.UserInfo.CompanyInfo.CompanyID).FirstOrDefault(),
                       CompanyName = model.Select(x => x.UserInfo.CompanyInfo.CompanyName).FirstOrDefault(),
                       TotalPendingItems = model.Count(),
                       TotalPending = GetUserCountbyCompany(status, model.Select(x => x.UserInfo.CompanyInfo.CompanyID).FirstOrDefault())
                   }).ToList();

            return qry;
        }

        public Int64 GetUserCount(Boolean status)
        {
            return (from s in db.MyShoppinCarts
                    join u in db.UserInformations on s.UserInfoID equals u.UserInfoID
                    where s.IsOrdered == status && u.WLSStatusId == 135 && s.Quantity != "" && s.UnitPrice != "" && u.IsDeleted == false
                    select new
                    {
                        u.UserInfoID
                    }).GroupBy(u=>u.UserInfoID).Distinct().Count();

        }
        public Int64 GetUserCountbyCompany(Boolean status,Int64 CompanyID)
        {
            return (from s in db.MyShoppinCarts
                    join u in db.UserInformations on s.UserInfoID equals u.UserInfoID
                    where s.IsOrdered == status && u.WLSStatusId == 135 && s.Quantity != "" && s.UnitPrice != "" 
                    && u.CompanyId == CompanyID && u.IsDeleted == false
                    select new
                    {
                        u.UserInfoID
                    }).GroupBy(u => u.UserInfoID).Distinct().Count();

        }
        public List<SelectShoppingCartPendingOrder> GetAllAdminUserList(Int64 CompanyId, Boolean status)
        {
            List<SelectShoppingCartPendingOrder> objShoppingCartPendingOrderList = new List<SelectShoppingCartPendingOrder>();
            // Here get the Distinct UserInfoID from MyShoppinCarts by companyID
            var distinctUser = (from s in db.MyShoppinCarts
                                join u in db.UserInformations on s.UserInfoID equals u.UserInfoID
                                join c in db.Companies on s.CompanyID equals c.CompanyID
                                where s.IsOrdered == status && u.WLSStatusId == 135 && c.CompanyID == CompanyId
                                && s.Quantity != "" && s.UnitPrice != "" && u.IsDeleted == false
                                select new
                                {
                                    UserInfoID = u.UserInfoID
                                }).GroupBy(u => u.UserInfoID).Distinct().ToList();

            // Here get the All the records of pending ordered form MyShoppinCarts by companyID
            var qry = (from s in db.MyShoppinCarts
                       join u in db.UserInformations on s.UserInfoID equals u.UserInfoID
                       join c in db.Companies on s.CompanyID equals c.CompanyID
                       join w in db.INC_Lookups on s.WorkgroupID equals w.iLookupID
                       where s.IsOrdered == status && u.WLSStatusId == 135 && c.CompanyID == CompanyId
                       && s.Quantity != "" && s.UnitPrice != "" && u.IsDeleted == false
                       select new SelectShoppingCartPendingOrder
                       {
                           UserInfoID = u.UserInfoID,
                           MyShoppingID = s.MyShoppingCartID.ToString(),
                           LoginEmail = u.LoginEmail,
                           FullName = u.FirstName + " " + u.LastName,
                           WorkGroupName = w.sLookupName,
                           CreatedDate = s.CreatedDate,
                           ValueofCart = Convert.ToDecimal(s.Quantity) * Convert.ToDecimal(s.UnitPrice)
                       }).ToList();

            for(int i =0; i <distinctUser.Count; i++)
            {
                SelectShoppingCartPendingOrder obj = new SelectShoppingCartPendingOrder();
                Decimal TotalValueCart = 0M;
                String MyShoppingID = String.Empty;
                // Here get only the details of current userinfoid 
                var newresult = (from q in qry
                                where q.UserInfoID == distinctUser[i].Key
                                select q).ToList();
                for (int j = 0; j < newresult.Count; j++)
                {
                    obj.UserInfoID = newresult[j].UserInfoID;
                    obj.LoginEmail = newresult[j].LoginEmail;
                    obj.FullName = newresult[j].FullName;
                    obj.WorkGroupName = newresult[j].WorkGroupName;
                    TotalValueCart += newresult[j].ValueofCart;
                    obj.CreatedDate = newresult[j].CreatedDate;
                    MyShoppingID += newresult[j].MyShoppingID + ",";
                }
                obj.MyShoppingID = MyShoppingID;
                obj.ValueofCart = TotalValueCart;
                objShoppingCartPendingOrderList.Add(obj);
            }

            return objShoppingCartPendingOrderList.OrderBy(u => u.FullName).ToList();


        }

        public class SelectShoppingCartPendingOrder
        {
            public Int64 UserInfoID { get; set; }
            public String MyShoppingID { get; set; }
            public String LoginEmail { get; set; }
            public String FullName { get; set; }
            public String WorkGroupName { get; set; }
            public Decimal ValueofCart { get; set; }
            public DateTime? CreatedDate { get; set; }
        }

        public class SelectCompanyPending
        {
            public Int64 CompanyID { get; set; }
            public string CompanyName { get; set; }
            public Int64 TotalPending { get; set; }
            public Int64 TotalPendingItems { get; set; }
        }

        public GetShoCarProByStoIDWorGroIDNIteNumResult GetUniqueShoppingCartProduct(Int64 StoreID, Int64 WorkGroupID, String ItemNumber)
        {
            return db.GetShoCarProByStoIDWorGroIDNIteNum(StoreID, WorkGroupID, ItemNumber).FirstOrDefault();
        }

        public List<MyShoppinCart> GetShoppingCartByOrderID(Int64 OrderID)
        {
            return db.MyShoppinCarts.Where(le => le.OrderID == OrderID).ToList();
        }
    }
}