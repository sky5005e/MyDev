using System;
using System.Collections.Generic;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class SAPBillingAddressRepository : RepositoryBase
    {
        public SAPBillingAddress GetByID(Int64 BillingAddressID)
        {
            return db.SAPBillingAddresses.FirstOrDefault(le => le.BillingAddressID == BillingAddressID);
        }

        public List<SAPBillingAddressCustom> GetByStoreAndWorkGroupID(Int64 StoreID, Int64 WorkGroupID)
        {
            var result = (from s in db.SAPBillingAddresses
                          join c in db.INC_Countries on s.CountryID equals c.iCountryID into countryLeft
                          from sc in countryLeft.DefaultIfEmpty()
                          join b in db.INC_BasedStations on s.BaseStationId equals b.iBaseStationId into baseLeft
                          from bs in baseLeft.DefaultIfEmpty()
                          where s.StoreID == StoreID && s.WorkGroupID == WorkGroupID
                          select new SAPBillingAddressCustom
                          {
                              BillingAddressID = s.BillingAddressID,
                              FirstName = s.FirstName,
                              LastName = s.LastName,
                              Country = sc.sCountryName,
                              CompanyName = s.CompanyName,
                              BaseStation = bs.sBaseStation,
                              SAPBillToCode = s.SAPBillToCode
                          }).ToList();

            return result;
        }

        public void DeleteByID(Int64 BillingAddressID)
        {
            SAPBillingAddress objAddress = GetByID(BillingAddressID);

            if (objAddress != null)
            {
                this.Delete(objAddress);
                this.SubmitChanges();
            }
        }

        public bool CheckIsExist(Int64 StoreID, Int64 WorkGroupID, Int64 BasestationID, Int64 BillingAddressID)
        {
            return (from s in db.SAPBillingAddresses
                    where s.StoreID == StoreID &&
                          s.WorkGroupID == WorkGroupID &&
                          s.BaseStationId == BasestationID &&
                          s.BillingAddressID != BillingAddressID
                    select s.BillingAddressID).Count() > 0;
        }
    }

    public class SAPBillingAddressCustom
    {
        public long BillingAddressID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public string BaseStation { get; set; }

        public string CompanyName { get; set; }

        public string SAPBillToCode { get; set; }
    }
}