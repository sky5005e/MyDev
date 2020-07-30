using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class GlobalBillingAddressRepository : RepositoryBase
    {
        public GlobalBillingAddress GetByID(Int64 BillingAddressID)
        {
            return db.GlobalBillingAddresses.FirstOrDefault(le => le.BillingAddressID == BillingAddressID);
        }

        public GlobalBillingAddress GetByStoreAndWorkGroupID(Int64 StoreID, Int64 WorkGroupID)
        {
            return db.GlobalBillingAddresses.FirstOrDefault(le => le.StoreID == StoreID && le.WorkGroupID == WorkGroupID);
        }

        public void DeleteByID(Int64 BillingAddressID)
        {
            GlobalBillingAddress objAddress = GetByID(BillingAddressID);
            this.Delete(objAddress);
            this.SubmitChanges();
        }

        public void DeleteByStoreAndWorkGroupID(Int64 StoreID, Int64 WorkGroupID)
        {
            GlobalBillingAddress objAddress = GetByStoreAndWorkGroupID(StoreID, WorkGroupID);
            this.Delete(objAddress);
            this.SubmitChanges();
        }
    }
}