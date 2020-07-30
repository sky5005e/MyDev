using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class AbandonedCartRepository : RepositoryBase
    {
        public List<GetAbandonedCartsResult> GetAbandonedCart(Int64? CompanyID, string Location, Int64? UserInfoID,string FromDate, string ToDate, string keyWord, Int32 PageSize, Int32 PageIndex, string SortColumn, string SortDirection)
        {
            return db.GetAbandonedCarts(CompanyID, Location, UserInfoID, FromDate, ToDate, keyWord, PageSize, PageIndex, SortColumn, SortDirection).ToList();
        }
    }
}
