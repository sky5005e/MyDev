using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class PendingUsersRepository : RepositoryBase
    {
        public List<GetPendingUsersResult> GetPendingUsers(Int64? CompanyId, Int64? WorkgroupId, Int64? StationId, String NameOfPendingUser, String NameOfApprover, string FromDate, string ToDate, string keyWord, Int32 PageSize, Int32 PageIndex, string SortColumn, string SortDirection)
        {
            return db.GetPendingUsers(CompanyId, WorkgroupId, StationId, NameOfPendingUser, NameOfApprover, FromDate, ToDate, keyWord, PageSize, PageIndex, SortColumn, SortDirection).ToList();
        }
    }
}
