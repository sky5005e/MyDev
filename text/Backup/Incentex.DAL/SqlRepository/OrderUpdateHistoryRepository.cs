using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class OrderUpdateHistoryRepository : RepositoryBase
    {
        public OrderUpdateHistory GetPreviousUpdateByOrderID(Int64 OrderID)
        {
            return db.OrderUpdateHistories.Where(le => le.OrderID == OrderID).OrderByDescending(le => le.CreatedDate).FirstOrDefault();
        }
    }
}