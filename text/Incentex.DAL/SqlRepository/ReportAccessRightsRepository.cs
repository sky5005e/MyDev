using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class ReportAccessRightsRepository : RepositoryBase
    {
        public ReportAccessRight GetByUserInfoIDAndParentReportID(Int64 UserInfoID , Int64 ParentReportID)
        {
            return db.ReportAccessRights.SingleOrDefault(a=>a.UserInfoID == UserInfoID && a.ParentReportID == ParentReportID);
        }

        public List<INC_Lookup> GetParentReportByUserInfoID(Int64 UserInfoID)
        {
            return (from reportRight in db.ReportAccessRights
                    join look in db.INC_Lookups on reportRight.ParentReportID equals look.iLookupID
                    where reportRight.UserInfoID == UserInfoID
                    select look
                        ).ToList();
        }

        public List<INC_Lookup> GetChildReportByUserInfoIDAndParentReportID(Int64 UserInfoID, Int64 ParentReportID)
        {
            string[] childList = db.ReportAccessRights.FirstOrDefault(r=>r.UserInfoID == UserInfoID && r.ParentReportID == ParentReportID).ChildReportID.Split(',');
            
            return db.INC_Lookups.Where(look => childList.Contains(look.iLookupID.ToString())).ToList();
        }

        public List<INC_Lookup> GetWorkgroupByUserInfoIDAndParentReportID(Int64 UserInfoID, Int64 ParentReportID)
        {
            ReportAccessRight objResult = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == ParentReportID);
            if (objResult != null && objResult.WorkgroupID !=null)
                return db.INC_Lookups.Where(look => objResult.WorkgroupID.Split(',').Contains(look.iLookupID.ToString())).ToList();
            else
                return null;
        }

        public List<INC_Lookup> GetPriceListByUserInfoIDAndParentReportID(Int64 UserInfoID, Int64 ParentReportID)
        {
            ReportAccessRight objResult = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == ParentReportID);

            if (objResult != null && objResult.PriceLevel != null)
                return db.INC_Lookups.Where(look => objResult.PriceLevel.Split(',').Contains(look.iLookupID.ToString())).ToList();
            else
                return null;
        }

        public List<INC_BasedStation> GetBaseStationByUserInfoIDAndParentReportID(Int64 UserInfoID, Int64 ParentReportID)
        {
            ReportAccessRight objResult = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == ParentReportID);
            if (objResult!=null && objResult.BaseStationID != null)
                return db.INC_BasedStations.Where(b => objResult.BaseStationID.Split(',').Contains(b.iBaseStationId.ToString())).ToList();
            else
                return null;
        }

        public List<SelectCompanyStoreNameResult> GetStoreByUserInfoIDAndParentReportID(Int64 UserInfoID, Int64 ParentReportID)
        {
            ReportAccessRight objResult = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == ParentReportID);
            if (objResult != null && objResult.StoreID != null)
                return new OrderConfirmationRepository().GetCompanyStoreName().Where(store => objResult.StoreID.Split(',').Contains(store.StoreID.ToString())).ToList();
            else
                return null;
        }
    }
}
