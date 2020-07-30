using System;
using System.Collections.Generic;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class PreferenceRepository : RepositoryBase
    {
        public List<PreferenceValue> GetPreferenceValuesByPreferenceID(Int64? PreferenceID)
        {
            return db.PreferenceValues.Where(le => le.PreferenceID == PreferenceID).ToList();
        }

        public List<FUN_GetUserPreferenceResult> GetUserPreferences(Int64? UserInfoID, Int64? UserTypeID)
        {
            return db.FUN_GetUserPreference(UserInfoID, UserTypeID).ToList();
        }

        public String GetUserPreferenceValue(Int64? UserInfoID, Int64? UserTypeID, String PreferenceKey)
        {
            return db.FUN_GetUserPreferenceValue(UserInfoID, UserTypeID, PreferenceKey);
        }

        public List<UserPreference> GetUserPreferences(Int64? UserInfoID)
        {
            return db.UserPreferences.Where(le => le.UserInfoID == UserInfoID).ToList();
        }

        public List<FUN_GetStorePreferenceResult> GetStorePreferences(Int64? StoreID)
        {
            return db.FUN_GetStorePreference(StoreID).ToList();
        }

        public String GetStorePreferenceValue(Int64? StoreID, String PreferenceKey)
        {
            return db.FUN_GetStorePreferenceValue(StoreID, PreferenceKey);
        }

        public List<StorePreference> GetStorerPreferences(Int64? StoreID)
        {
            return db.StorePreferences.Where(le => le.StoreID == StoreID).ToList();
        }

        //added by Prashant April 2013
        public List<FUN_GetWorkGroupPreferenceResult> GetWorkGroupPreferences(Int64? WorkGroupID, Int64? StoreID)
        {
            return db.FUN_GetWorkGroupPreference(WorkGroupID, StoreID).ToList();
        }

        //public String GetWorkGroupPreferenceValue(Int64? WorkGroupID,Int64? CompanyID, String PreferenceKey)
        //{
        //    return db.FUN_GetWorkGroupPreferenceValue(WorkGroupID,CompanyID,PreferenceKey);
        //}

        public WorkGroupPreference GetWorkGroupPreferenceByWorkGroupID(Int64? WorkGroupID,Int64? StoreID, Int64? PreferenceID)
        {
            return db.WorkGroupPreferences.FirstOrDefault(w => w.WorkGroupID == WorkGroupID && w.PreferenceID == PreferenceID && w.StoreID == StoreID);
        }

        public List<PreferenceValue> GetByPreferenceKey(String PreferenceFor)
        {
            return (from p in db.Preferences
                    join pv in db.PreferenceValues on p.PreferenceID equals pv.PreferenceID
                    where p.PreferenceFor == PreferenceFor
                    select pv).ToList();
        }

        public String GetPreferenceValuesByPreferenceValueID(Int64? PreferenceValueID)
        {
            return db.PreferenceValues.FirstOrDefault(le => le.PreferenceValueID == PreferenceValueID).Value;
        }
        //added by Prashant April 2013
    }
}