using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class IssuanceActivatedRepository : RepositoryBase
    {
        public List<GetIssuancePolicyDetailsResult> GetIssuancePolicyDetails(Int64? CompanyId, Int64? WorkgroupId, string ReportType, string FromDate, string ToDate, string keyWord, Int32 PageSize, Int32 PageIndex, string SortColumn, string SortDirection)
        {
            return db.GetIssuancePolicyDetails(CompanyId, WorkgroupId, FromDate, ToDate, ReportType, keyWord, PageSize, PageIndex, SortColumn, SortDirection).ToList();
        }
    }
}
