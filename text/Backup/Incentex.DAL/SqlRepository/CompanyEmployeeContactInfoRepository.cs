using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class CompanyEmployeeContactInfoRepository : RepositoryBase
    {
        IQueryable<CompanyEmployeeContactInfo> GetAllQuery()
        {
            IQueryable<CompanyEmployeeContactInfo> qry = from C in db.CompanyEmployeeContactInfos
                                                         select C;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoId"></param>
        /// <returns></returns>
        public List<CompanyEmployeeContactInfo> GetAllTitles(Int64 UserInfoId)
        {
            List<CompanyEmployeeContactInfo> obj = db.CompanyEmployeeContactInfos.Where(s => s.UserInfoID == UserInfoId && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString()).ToList();
            return obj;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoId"></param>
        /// <returns></returns>
        public CompanyEmployeeContactInfo GetFirstBillingDetailByUserInfoID(Int64 userInfoID)
        {
            return db.CompanyEmployeeContactInfos.FirstOrDefault(s => s.UserInfoID == userInfoID && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString() && s.OrderType == "MySetting");
        }

        /// <summary>
        /// Update BY : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoId"></param>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public CompanyEmployeeContactInfo GetBillingDetailIssuanceAddress(Int64 userInfoId, Int64 orderId)
        {
            return db.CompanyEmployeeContactInfos.FirstOrDefault(le => le.UserInfoID == userInfoId && le.OrderID == orderId && le.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString() && le.OrderType == "IssuancePolicy");
        }

        /// <summary>
        /// Update By ; Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoId"></param>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public CompanyEmployeeContactInfo GetBillingDetailAddress(Int64 UserInfoId, Int64 OrderId)
        {
            //CompanyEmployeeContactInfo obj = GetAllQuery().Where(s => s.UserInfoID == UserInfoId && s.OrderID == OrderId && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString() && s.OrderType == "ShoppingCart").ToList().SingleOrDefault();

            CompanyEmployeeContactInfo obj = (from C in db.CompanyEmployeeContactInfos
                                              where C.UserInfoID == UserInfoId &&
                                                    C.OrderID == OrderId &&
                                                    C.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString() &&
                                                    C.OrderType == "ShoppingCart"
                                              select C).FirstOrDefault();

            return obj;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        private CompanyEmployeeContactInfo GetBillingDetailByOrderID(Int64 orderId)
        {
            return db.CompanyEmployeeContactInfos.FirstOrDefault(le => le.OrderID == orderId && le.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString());
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoId"></param>
        /// <param name="CompanyContactid"></param>
        /// <returns></returns>
        public CompanyEmployeeContactInfo GetShippingDetailById(Int64 userInfoID, Int64 companyContactInfoID)
        {
            return db.CompanyEmployeeContactInfos.FirstOrDefault(le => le.UserInfoID == userInfoID && le.CompanyContactInfoID == companyContactInfoID && le.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString());
        }

        public CompanyEmployeeContactInfo GetBillingDetailById(Int64 userInfoID, Int64 companyContactInfoID)
        {
            return db.CompanyEmployeeContactInfos.FirstOrDefault(le => le.UserInfoID == userInfoID && le.CompanyContactInfoID == companyContactInfoID && le.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString());
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoId"></param>
        /// <param name="OrderId"></param>
        /// <param name="ContactInfoType"></param>
        /// <param name="OrderType"></param>
        /// <returns></returns>
        public CompanyEmployeeContactInfo GetShippingDetailAddress(Int64 UserInfoId, Int64 OrderId, string ContactInfoType, string OrderType)
        {
            //CompanyEmployeeContactInfo obj = GetAllQuery().Where(s => s.UserInfoID == UserInfoId && s.OrderID == OrderId && s.ContactInfoType == ContactInfoType && s.OrderType == OrderType).ToList().SingleOrDefault();
            CompanyEmployeeContactInfo obj = (from C in db.CompanyEmployeeContactInfos
                                              where C.UserInfoID == UserInfoId &&
                                                    C.OrderID == OrderId &&
                                                    C.ContactInfoType == ContactInfoType &&
                                                    C.OrderType == OrderType
                                              select C).SingleOrDefault();

            return obj;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public CompanyEmployeeContactInfo GetShippingDetailByOrderId(Int64 OrderId)
        {
            //CompanyEmployeeContactInfo obj = GetAllQuery().Where(s => s.OrderID == OrderId && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString()).ToList().SingleOrDefault();
            CompanyEmployeeContactInfo obj = (from C in db.CompanyEmployeeContactInfos
                                              where C.OrderID == OrderId &&
                                                    C.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString()
                                              select C).FirstOrDefault();

            return obj;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="CompanyContactid"></param>
        /// <returns></returns>
        public CompanyEmployeeContactInfo GetBillingDetailsByID(Int64 CompanyContactid)
        {
            //CompanyEmployeeContactInfo obj = GetAllQuery().Where(s => s.CompanyContactInfoID == CompanyContactid && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString()).ToList().SingleOrDefault();
            CompanyEmployeeContactInfo obj = (from C in db.CompanyEmployeeContactInfos
                                              where C.CompanyContactInfoID == CompanyContactid &&
                                                    C.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString()
                                              select C).FirstOrDefault();
            return obj;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="CompanyContactid"></param>
        /// <returns></returns>
        public CompanyEmployeeContactInfo GetShippingDetailsByID(Int64 CompanyContactid)
        {
            //CompanyEmployeeContactInfo obj = GetAllQuery().Where(s => s.CompanyContactInfoID == CompanyContactid && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString()).ToList().SingleOrDefault();
            CompanyEmployeeContactInfo obj = (from C in db.CompanyEmployeeContactInfos
                                              where C.CompanyContactInfoID == CompanyContactid &&
                                                    C.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString()
                                              select C).FirstOrDefault();
            return obj;
        }

        public List<CompanyEmployeeContactInfo> GetSavedShippingAddressesForUser(Int64 UserInfoId, Incentex.DAL.SqlRepository.CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType SortExp, DAEnums.SortOrderType SortOrder)
        {
            List<CompanyEmployeeContactInfo> obj = new List<CompanyEmployeeContactInfo>();
            switch (SortExp)
            {
                case CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Company:
                    obj = GetAllQuery().Where(s => s.UserInfoID == UserInfoId && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString() && s.CompanyName != null && s.CompanyName != String.Empty && (s.OrderType == "MySetting" || (s.OrderType == "AdditionalStation" && s.IsAdditionalStationActive == true))).OrderBy(o => o.CompanyName).ToList();
                    break;
                case CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Title:
                    obj = GetAllQuery().Where(s => s.UserInfoID == UserInfoId && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString() && s.Title != null && s.Title != String.Empty && (s.OrderType == "MySetting" || (s.OrderType == "AdditionalStation" && s.IsAdditionalStationActive == true))).OrderBy(o => o.Title).ToList();
                    break;
                case CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Email:
                    obj = GetAllQuery().Where(s => s.UserInfoID == UserInfoId && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString() && s.Email != null && s.Email != String.Empty && (s.OrderType == "MySetting" || (s.OrderType == "AdditionalStation" && s.IsAdditionalStationActive == true))).OrderBy(o => o.Email).ToList();
                    break;

            }

            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                obj.Reverse();
            }

            return obj;
        }

        public List<CompanyEmployeeContactInfo> GetSavedBillingAddressesForUser(Int64 UserInfoId, Incentex.DAL.SqlRepository.CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType SortExp, DAEnums.SortOrderType SortOrder)
        {
            List<CompanyEmployeeContactInfo> lstBillingAddresses = new List<CompanyEmployeeContactInfo>();
            switch (SortExp)
            {
                case CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Company:
                    lstBillingAddresses = GetAllQuery().Where(s => s.UserInfoID == UserInfoId && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString() && s.CompanyName != null && s.CompanyName != String.Empty && s.OrderType == "MySetting").OrderBy(o => o.CompanyName).ToList();
                    break;
                case CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Title:
                    lstBillingAddresses = GetAllQuery().Where(s => s.UserInfoID == UserInfoId && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString() && s.Title != null && s.Title != String.Empty && s.OrderType == "MySetting").OrderBy(o => o.Title).ToList();
                    break;
                case CompanyEmployeeContactInfoRepository.ShippingInfoSortExpType.Email:
                    lstBillingAddresses = GetAllQuery().Where(s => s.UserInfoID == UserInfoId && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Billing.ToString() && s.Email != null && s.Email != String.Empty && s.OrderType == "MySetting").OrderBy(o => o.Email).ToList();
                    break;
            }

            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                lstBillingAddresses.Reverse();
            }

            return lstBillingAddresses;
        }

        /// <summary>
        /// update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <returns></returns>
        public List<CompanyEmployeeContactInfo> GetAditionalStationShippingDetailsByUserInfoID(Int64 UserInfoID)
        {
            //return GetAllQuery().Where(s => s.UserInfoID == UserInfoID && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString() && s.OrderType == "AdditionalStation").ToList();
            return (from C in db.CompanyEmployeeContactInfos
                    where C.UserInfoID == UserInfoID &&
                          C.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString() &&
                          C.OrderType == "AdditionalStation"
                    select C).ToList();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        public Boolean CheckForShippingStationAvailabelByUserInfoID(Int64 UserInfoID, string station)
        {
            //if (GetAllQuery().Where(s => s.UserInfoID == UserInfoID && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString() && s.OrderType == "AdditionalStation" && s.Station == station).Count() > 0)
            //    return true;
            //else
            //    return false;


            return (from C in db.CompanyEmployeeContactInfos
                    where C.UserInfoID == UserInfoID &&
                          C.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString() &&
                          C.OrderType == "AdditionalStation" &&
                          C.Station == station
                    select C).Count() > 0;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="CompanyContactInfoID"></param>
        /// <returns></returns>
        public CompanyEmployeeContactInfo GetShippingDetailsByContactInfoID(Int64 CompanyContactInfoID)
        {
            //CompanyEmployeeContactInfo obj = GetAllQuery().Where(s => s.CompanyContactInfoID == CompanyContactInfoID && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString()).FirstOrDefault();

            CompanyEmployeeContactInfo obj = (from C in db.CompanyEmployeeContactInfos
                                              where C.CompanyContactInfoID == CompanyContactInfoID &&
                                                    C.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString()
                                              select C).FirstOrDefault();
            return obj;
        }

        public enum ShippingInfoSortExpType
        {
            Company,
            Title,
            Email
        }

        public List<StationAdditionalWorkgroup> GetAdditionalWorkgroupByCompanyContactInfoID(Int64 CompanyContactInfoID)
        {
            var qry = from w in db.CompanyEmployeeStationWorkgroups
                      join look in db.INC_Lookups on w.WorkgroupID equals look.iLookupID
                      where w.CompanyContactInfoID == CompanyContactInfoID
                      select new StationAdditionalWorkgroup { WorkgroupID = w.WorkgroupID, WorkgroupName = look.sLookupName, TotalEmployee = w.TotalEmployee, StationWorkgroupID = w.StationWorkgroupID };
            return qry.ToList();
        }

        public CompanyEmployeeStationWorkgroup GetCompanyEmployeeStationWorkgroupByID(Int64 StationWorkgroupID)
        {
            return db.CompanyEmployeeStationWorkgroups.SingleOrDefault(c => c.StationWorkgroupID == StationWorkgroupID);
        }

        public Boolean CheckForStationWorkgroupAvailabelByContactInfoID(Int64 ContactInfoID, Int64 workgroupID)
        {
            if (db.CompanyEmployeeStationWorkgroups.Where(w => w.CompanyContactInfoID == ContactInfoID && w.WorkgroupID == workgroupID).Count() > 0)
                return true;
            else
                return false;
        }

        public class StationAdditionalWorkgroup
        {
            public Int64 WorkgroupID { get; set; }
            public string WorkgroupName { get; set; }
            public Int64 TotalEmployee { get; set; }
            public Int64 StationWorkgroupID { get; set; }
        }

        /// <summary>
        /// added by Prashant - 7th Dec
        /// </summary>
        /// <param name="BaseStationID"></param>
        /// <returns></returns>
        public CompanyEmployeeContactInfo GetShippingDetailsByBaseStationID(Int64 BaseStationID)
        {
            CompanyEmployeeContactInfo obj = GetAllQuery().Where(s => s.BaseStationID == BaseStationID && s.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString()).FirstOrDefault();
            return obj;
        }
        /// <summary>
        /// Delete  Company Employee Contact Info By companyContactInfoID
        /// </summary>
        /// <param name="companyContactInfoID"></param>
        public void DeleteAddressByID(Int64 companyContactInfoID)
        {
            CompanyEmployeeContactInfo objCEC = db.CompanyEmployeeContactInfos.FirstOrDefault(le => le.CompanyContactInfoID == companyContactInfoID && le.ContactInfoType == DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString());
            this.Delete(objCEC);
            this.SubmitChanges();
        }
    }
}
