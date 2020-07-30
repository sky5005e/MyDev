using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class PendingOrderRepository : RepositoryBase
    {
        public List<GetPendingOrdersResult> GetPendingOrders(Int64? CompanyId, Int64? WorkgroupId, Int64? StationId, String NameOfCustomer, String NameOfApprover, string keyWord, Int32 PageSize, Int32 PageIndex, string SortColumn, string SortDirection)
        {
            return db.GetPendingOrders(CompanyId, StationId, WorkgroupId, null, NameOfCustomer, NameOfApprover, keyWord, PageSize, PageIndex, SortColumn, SortDirection).ToList();
        }
    }
}
