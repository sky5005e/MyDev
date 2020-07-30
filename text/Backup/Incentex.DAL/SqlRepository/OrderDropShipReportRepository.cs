using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class OrderDropShipReportRepository : RepositoryBase
    {

        public OrderDropShipReport GetByID(Int64 DropShipReportID)
        {
            return db.OrderDropShipReports.SingleOrDefault(o => o.DropShipReportID == DropShipReportID);
        }


        public List<OrderDropShipReport> GetByOrderIDAndMyShoppingCartID(Int64 OrderID, Int64 MyShoppingCartID)
        {
            return db.OrderDropShipReports.Where(o => o.OrderID == OrderID && o.MyShoppingCartID == MyShoppingCartID).ToList();
        }
    }
}
