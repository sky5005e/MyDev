using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class InternationalShippingAddressRepository : RepositoryBase
    {
        public List<InternationalShippingAddress> GetAll()
        {
            return db.InternationalShippingAddresses.ToList();
        }
        public InternationalShippingAddress GetByID(Int64 InternationalShippingID)
        {
            return db.InternationalShippingAddresses.SingleOrDefault(i=>i.InternationalShippingID == InternationalShippingID);
        }
        public InternationalShippingAddress GetByStoreIDAndWorkgroupID(Int64 StoreID, Int64 WorkgroupID)
        {
            return db.InternationalShippingAddresses.SingleOrDefault(i => i.StoreID == StoreID && i.WorkgroupID == WorkgroupID);
        }
        public InternationalShippingAddress GetShoppingCartAddressByStoreIDAndWorkgroupID(Int64 StoreID, Int64 WorkgroupID)
        {
            return db.InternationalShippingAddresses.SingleOrDefault(i => i.StoreID == StoreID && i.WorkgroupID == WorkgroupID && i.IsForShoppingCart == true);
        }
        public InternationalShippingAddress GetIssuancePolicyAddressByStoreIDAndWorkgroupID(Int64 StoreID, Int64 WorkgroupID)
        {
            return db.InternationalShippingAddresses.SingleOrDefault(i => i.StoreID == StoreID && i.WorkgroupID == WorkgroupID && i.IsForIssuancePolicy == true);
        }
        
    }
}
