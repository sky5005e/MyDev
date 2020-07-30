using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class GlobalShippingAddressRepository : RepositoryBase
    {
        public GlobalShippingAddress GetByStoreWorkgroupStationPaymentOption(Int64 UserInfoID,Int64 StoreID, Int64 PaymentOptionTypeID)
        {
            var result = (from g in db.GlobalShippingAddresses
                          join gc in db.GlobalShippingAddressPaymentTypes on g.ShippingAddressID equals gc.ShippingAddressID
                          join ce in db.CompanyEmployees on new { WorkGroupID = (g.WorkGroupID ?? 0), BaseStationID = g.BaseStationID } equals new { WorkGroupID = ce.WorkgroupID, BaseStationID = ce.BaseStation }
                          where ce.UserInfoID == UserInfoID && gc.PaymentOptionTypeID == PaymentOptionTypeID && g.StoreID == StoreID
                          && g.StatusID == 135 && gc.StatusID == 135 && gc.IsDeleted != true && g.IsDeleted != true
                          select g).FirstOrDefault();

            return result;
        }

        public string InsertGlobalShippingAddressDetails(Int64 ShippingAddressID, Int64 StoreID, Int64 WorkGroupID, Int64 BaseStationID, string ShippingAddress, Int64 CityID, Int64 StateID, Int64 CountryID, string CompanyName, string email, string Mobile, string Telephone, string ZipCode, string IssuancePolicyTypeToApply, Int64 StatusID, Int64 UserInfoID, string strPaymentOptionTypeIDs)
        {
            return db.InsertGlobalShippingAddress(ShippingAddressID, StoreID, WorkGroupID, BaseStationID, ShippingAddress, CityID, StateID, CountryID, CompanyName, email, Mobile, Telephone, ZipCode, IssuancePolicyTypeToApply, StatusID, UserInfoID, strPaymentOptionTypeIDs).FirstOrDefault().InsertUpdateStatus;
        }

        public List<GetShippingAddressByStoreIDWorkGroupIDResult> GetShippingAddressDetails(Int64 StoreID, Int64 WorkGroupID)
        {
            return db.GetShippingAddressByStoreIDWorkGroupID(StoreID, WorkGroupID, null).ToList();
        }

        public GetShippingAddressByStoreIDWorkGroupIDResult GetByID(Int64 StoreID, Int64 WorkGroupID, Int64? ShippingAddressID)
        {
            return db.GetShippingAddressByStoreIDWorkGroupID(StoreID, WorkGroupID, ShippingAddressID).FirstOrDefault();
        }
       
        public int DeleteShippingAddress(Int64 ShippingAddressID, Int64 UserInfoID)
        {
            return db.DeleteShippingAddress(ShippingAddressID,UserInfoID);
        }
    }
}
