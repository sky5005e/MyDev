using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class PageLoadRepository : RepositoryBase
    {
        public void AddPageLoadDetails(Int64? UserID, Int64? CompanyID, String PageName, String PageEvent, Int64? LoadTime, String ModuleName, String SubMenu, String BrowserName, String DeviceName)
        {
            db.LoadTimeDetails.InsertOnSubmit(new LoadTimeDetail()
            {
                UserID = UserID,
                CompanyID = CompanyID,
                PageName = PageName,
                PageEvent = PageEvent,
                LoadTime = LoadTime,
                CreatedDate = DateTime.Now,
                ModuleName = ModuleName,
                SubMenu = SubMenu,
                BrowserName = BrowserName,
                DeviceName = DeviceName
            });
            db.SubmitChanges();
        }

        public List<GetLoadTimeDetailsResult> GetLoadTimeData(Int64? CompanyId, Int64? UserId, Int64? WorkgroupID, Int64? FromLoadTime, Int64? ToLoadTime, String ModuleName, String SubMenuName, String BrowserName, String DeviceName, String PageEventName, string FromDate, string ToDate, string keyWord, Int32 PageSize, Int32 PageIndex, string SortColumn, string SortDirection)
        {
            return db.GetLoadTimeDetails(CompanyId, UserId, WorkgroupID, FromLoadTime, ToLoadTime, ModuleName, SubMenuName, BrowserName, DeviceName, PageEventName, FromDate, ToDate, keyWord, PageSize, PageIndex, SortColumn, SortDirection).ToList();
        }

        public List<GetLoadTimeSubDetailsResult> GetLoadTimeDetailsData(Int64? CompanyId, Int64? UserId, Int64? WorkgroupID, Int64? FromLoadTime, Int64? ToLoadTime, String ModuleName, String SubMenuName, String BrowserName, String DeviceName, String PageEventName, string FromDate, string ToDate, string keyWord, Int32 PageSize, Int32 PageIndex, string SortColumn, string SortDirection)
        {
            return db.GetLoadTimeSubDetails(CompanyId, UserId, WorkgroupID, FromLoadTime, ToLoadTime, ModuleName, SubMenuName, BrowserName, DeviceName, PageEventName, FromDate, ToDate, keyWord, PageSize, PageIndex, SortColumn, SortDirection).ToList();
        }

        public List<MediaHelpVidPrnt> GetModules()
        {
            return db.MediaHelpVidPrnts.OrderBy(x => x.ModuleName).ToList();
        }

        public List<MediaHelpVidSub> GetSubMenu(Int64 ModuleId)
        {
            return db.MediaHelpVidSubs.Where(x => x.MediaHelpVidPrntID == ModuleId && x.PageType != "Pop-up").OrderBy(x => x.ModuleName).ToList();
        }

        public List<BrowserNames> GetBrowsers()
        {
            return db.LoadTimeDetails.Select(x => new { x.BrowserName }).Distinct()
                .Where(x => x.BrowserName != string.Empty && x.BrowserName != null)
                .Select(x => new BrowserNames()
                {
                    BrowserName = x.BrowserName
                })
                .OrderBy(x => x.BrowserName)
                .ToList();
        }

        public List<DeviceNames> GetDeviceNames()
        {
            return db.LoadTimeDetails.Select(x => new { x.DeviceName }).Distinct()
                .Where(x => x.DeviceName != string.Empty && x.DeviceName != null)
                .Select(x => new DeviceNames()
                {
                    DeviceName = x.DeviceName
                })
                .OrderBy(x => x.DeviceName)
                .ToList();
        }

        public List<EventNames> GetEventNames()
        {
            return db.LoadTimeDetails.Select(x => new { x.PageEvent }).Distinct()
                .Where(x => x.PageEvent != string.Empty && x.PageEvent != null)
                .Select(x => new EventNames()
                {
                    EventName = x.PageEvent
                })
                .OrderBy(x => x.EventName)
                .ToList();
        }

    }

    public class BrowserNames
    {
        public string BrowserName { get; set; }
    }

    public class DeviceNames
    {
        public string DeviceName { get; set; }
    }

    public class EventNames
    {
        public string EventName { get; set; }
    }
}
