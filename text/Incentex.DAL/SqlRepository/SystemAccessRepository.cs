using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class SystemAccessRepository : RepositoryBase
    {
        public List<GetSystemAccessDetailsResult> SearchSystemAccessDetails(Int64? CompanyId, Int64? UserId, Int64? WorkgroupId, Int64? StationId, string FromDate, string ToDate, string keyWord, Int32 PageSize, Int32 PageIndex, string SortColumn, string SortDirection)
        {
            return db.GetSystemAccessDetails(CompanyId, UserId, WorkgroupId, StationId, FromDate, ToDate, keyWord, PageSize, PageIndex, SortColumn, SortDirection).ToList();
        }

        public List<GetSystemAccessSubDetailsResult> SearchSystemData(Int64? UserId, string FromDate, string ToDate, Int32 PageSize, Int32 PageIndex, string SortColumn, string SortDirection)
        {
            return db.GetSystemAccessSubDetails(UserId, FromDate, ToDate, PageSize, PageIndex, SortColumn, SortDirection).ToList();
        }
    }
}
