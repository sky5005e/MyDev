using System;
using System.Collections.Generic;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class OrderMOASSystemRepository : RepositoryBase
    {
        /// <summary>
        /// Gets by order ID.
        /// </summary>
        /// <param name="OrderID">The order ID.</param>
        /// <returns></returns>
        public List<OrderMOASSystem> GetByOrderID(Int64 OrderID)
        {
            return db.OrderMOASSystems.Where(o => o.OrderID == OrderID).ToList();
        }

        /// <summary>
        /// Gets by order ID and status.
        /// </summary>
        /// <param name="OrderID">The order ID.</param>
        /// <param name="Status">The status.</param>
        /// <returns></returns>
        public List<OrderMOASSystem> GetByOrderIDAndStatus(Int64 OrderID, String Status)
        {
            return db.OrderMOASSystems.Where(o => o.OrderID == OrderID && o.Status == Status).OrderBy(o => o.Priority).ToList();
        }

        public OrderMOASSystem GetByOrderIDAndManagerUserInfoID(Int64 OrderID, Int64 ManagerUserInfoID)
        {
            return db.OrderMOASSystems.FirstOrDefault(o => o.OrderID == OrderID && o.ManagerUserInfoID == ManagerUserInfoID);
        }

        public List<SelectCompanyPending> GetCompanyPendingList(String status)
        {

            List<SelectCompanyPending> qry = null;
            if (status.ToUpper() == "ORDER PENDING")
            {
                qry =db.Orders.Where(model=>model.OrderStatus==status && model.IsPaid==true).Join(
                db.OrderMOASSystems.Where(model => model.Status == status).Join(
                db.UserInformations.Where(model => model.WLSStatusId == 135 && model.IsDeleted == false).Join(
                db.Companies, UserInfo => UserInfo.CompanyId, CompanyInfo => CompanyInfo.CompanyID,
                (UserInfo, CompanyInfo) => new { UserInfo, CompanyInfo }), OrderMO => OrderMO.ManagerUserInfoID,
                Group => Group.UserInfo.UserInfoID,
                (OrderMo, Group) => new {OrderMo,Group}),OrderInfo=> OrderInfo.OrderID,UserDetailsInfo=>UserDetailsInfo.OrderMo.OrderID,
                (OrderDetailsInfo,UserDetailsInfo)=>new {OrderDetailsInfo,UserDetailsInfo})
                .GroupBy(model => model.UserDetailsInfo.Group.CompanyInfo.CompanyName)
                    .Select(model => new SelectCompanyPending()
                    {
                        CompanyID = model.Select(x => x.UserDetailsInfo.Group.CompanyInfo.CompanyID).FirstOrDefault(),
                        CompanyName = model.Select(x => x.UserDetailsInfo.Group.CompanyInfo.CompanyName).FirstOrDefault(),
                        TotalPending = model.Count()
                    }).ToList();
            }
            else if (status == "pending")
            {
                qry = db.Inc_Registrations.Where(model => model.status == status).Join(
                        db.Companies, RegisInfo => RegisInfo.iCompanyName, CompanyInfo => CompanyInfo.CompanyID,
                        (RegisInfo, CompanyInfo) => new { RegisInfo, CompanyInfo }).GroupBy(model => model.CompanyInfo.CompanyName)
                    .Select(model => new SelectCompanyPending()
                    {
                        CompanyID = model.Select(x => x.CompanyInfo.CompanyID).FirstOrDefault(),
                        CompanyName = model.Select(x => x.CompanyInfo.CompanyName).FirstOrDefault(),
                        TotalPending = model.Count()
                    }).ToList();
            }
            return qry;
        }

        public List<SelectAdminMOASOrdersPending> GetAllAdminUserList(Int64 CompanyId, String status)
        {
            List<SelectAdminMOASOrdersPending> qry = db.Orders.Where(model=>model.OrderStatus==status && model.IsPaid==true).Join(
                db.OrderMOASSystems.Where(model => model.Status == status).Join(
                db.UserInformations.Where(model => model.WLSStatusId == 135 && model.IsDeleted == false).Join(
                db.Companies.Where(model => model.CompanyID == CompanyId), UserInfo => UserInfo.CompanyId, CompanyInfo => CompanyInfo.CompanyID,
                (UserInfo, CompanyInfo) => new { UserInfo, CompanyInfo }), OrderMO => OrderMO.ManagerUserInfoID,
                Group => Group.UserInfo.UserInfoID,
                (OrderMo, Group) => new {OrderMo,Group}),OrderInfo=> OrderInfo.OrderID,UserDetailsInfo=>UserDetailsInfo.OrderMo.OrderID,
                (OrderDetailsInfo,UserDetailsInfo)=>new {OrderDetailsInfo,UserDetailsInfo})
                .GroupBy(model => model.UserDetailsInfo.Group.UserInfo.FirstName)
                .Select(model => new SelectAdminMOASOrdersPending()
                {
                    UserInfoID = model.Select(x => x.UserDetailsInfo.Group.UserInfo.UserInfoID).FirstOrDefault(),
                    FirstName = model.Select(x => x.UserDetailsInfo.Group.UserInfo.FirstName).FirstOrDefault(),
                    LastName = model.Select(x => x.UserDetailsInfo.Group.UserInfo.LastName).FirstOrDefault(),
                    LoginEmail = model.Select(x => x.UserDetailsInfo.Group.UserInfo.LoginEmail).FirstOrDefault(),
                    FullName = model.Select(x => x.UserDetailsInfo.Group.UserInfo.FirstName).FirstOrDefault() + " " + model.Select(x => x.UserDetailsInfo.Group.UserInfo.LastName).FirstOrDefault(),
                    TotalPending = model.Count(),
                    TotalDollarValue = model.Sum(x => x.OrderDetailsInfo.OrderAmount).Value + model.Sum(x => x.OrderDetailsInfo.SalesTax).Value + model.Sum(x => x.OrderDetailsInfo.ShippingAmount).Value
                }).ToList();
            return qry;

        }

        public List<SelectOrderDetails> GetOrderDetialsByUserInfoID(Int64 manageID, String status)
        {
            var qry = (from ord in db.Orders
                       join oms in db.OrderMOASSystems on ord.OrderID equals oms.OrderID
                       join ui in db.UserInformations on ord.UserId equals ui.UserInfoID
                       where ord.IsPaid == true && ord.OrderStatus == status && oms.Status == status && oms.ManagerUserInfoID == manageID
                       && ui.IsDeleted == false
                       select new SelectOrderDetails
                       {
                           OrderID = ord.OrderID,
                           UserInfoID = ui.UserInfoID,
                           OrderBy = ui.FirstName + " " + ui.LastName,
                           OrderNumber = ord.OrderNumber
                       }).ToList();
            return qry.ToList <SelectOrderDetails>();
        }
        public List<SelectAdminMOASOrdersPending> GetAllAdminPendingUserList(Int64 CompanyId)
        {
            var list = (from u in db.GetPendingUserListByCompanyId(CompanyId).ToList()
                        select new SelectAdminMOASOrdersPending
                        {
                            UserInfoID = u.UserInfoID,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            LoginEmail = u.LoginEmail,
                            FullName = u.FullName,
                            TotalPending = Convert.ToInt64(u.TotalPending)
                        }).ToList();
            return list.ToList<SelectAdminMOASOrdersPending>();
        }
       
        public class SelectCompanyPending
        {
            public Int64 CompanyID { get; set; }
            public String CompanyName { get; set; }
            public Int64 TotalPending { get; set; }
        }

        public class SelectOrderDetails
        {
            public Int64 OrderID { get; set; }
            public Int64? UserInfoID { get; set; }
            public String OrderBy { get; set; }
            public String OrderNumber { get; set; }
        }
        public class SelectAdminMOASOrdersPending
        {
            public Int64 UserInfoID { get; set; }
            public String FirstName { get; set; }
            public String LastName { get; set; }
            public String LoginEmail { get; set; }
            public String FullName { get; set; }
            public Int64 TotalPending { get; set; }
            public Decimal TotalDollarValue { get; set; }
        }

    }
}
