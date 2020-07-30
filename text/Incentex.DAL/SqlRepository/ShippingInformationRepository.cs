using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class ShippingInformationRepository: RepositoryBase
    {
        IQueryable<ShippingInformation> GetAllQuery()
        {
            IQueryable<ShippingInformation> qry = from C in db.ShippingInformations
                                                 select C;
            return qry;
        }

        public ShippingInformation GetShippingById(Int64 ShoppingInformationId)
        {
            ShippingInformation obj = GetSingle(GetAllQuery().Where(s => s.ShippingInfromationid == ShoppingInformationId).ToList());
            return obj;
        }
    }
}
