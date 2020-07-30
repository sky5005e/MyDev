using System;
using System.Collections.Generic;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class SAPRepository : RepositoryBase
    {
        public Boolean IsItemDuplicate(String CurrentItemNumber, String PreviousItemNumber)
        {
            if (PreviousItemNumber == "")
            {
                SAPItem objDuplicate = db.SAPItems.FirstOrDefault(le => le.SAPItemNumber == CurrentItemNumber && le.IsActive == true && le.IsDeleted == false);
                return objDuplicate != null;
            }
            else
            {
                SAPItem objDuplicate = db.SAPItems.FirstOrDefault(le => le.SAPItemNumber == CurrentItemNumber && le.SAPItemNumber != PreviousItemNumber && le.IsActive == true && le.IsDeleted == false);
                return objDuplicate != null;
            }
        }

        public SAPItem GetByItemCode(String ItemNumber)
        {
            return db.SAPItems.FirstOrDefault(le => le.SAPItemNumber == ItemNumber && le.IsActive == true && le.IsDeleted == false);
        }

        public GetCouStaCitForSAPResult GetCountryStateCityForSAP(String CountryCode, String StateCode, String CityName)
        {
            return db.GetCouStaCitForSAP(CountryCode, StateCode, CityName).FirstOrDefault();
        }

        public List<GetOrdersToBeRetransmittedToSAPResult> GetOrdersToBeRetransmittedToSAP()
        {
            return db.GetOrdersToBeRetransmittedToSAP().ToList();
        }

        public SAPSetting GetSAPSettingByID(Int64 SAPSettingID)
        {
            return db.SAPSettings.FirstOrDefault(le => le.SettingID == SAPSettingID);
        }

        public List<SAPSetting> GetSAPSettings()
        {
            return db.SAPSettings.ToList();
        }

        public SAPSetting GetSAPSettingBySettingKey(String SettingKey)
        {
            return db.SAPSettings.FirstOrDefault(le => le.SettingKey == SettingKey);
        }
    }
}