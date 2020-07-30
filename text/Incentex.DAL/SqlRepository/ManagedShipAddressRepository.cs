using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class ManagedShipAddressRepository : RepositoryBase
    {
        //public IQueryable<MangedShipAddress> GetAllQuery()
        //{
        //    IQueryable<MangedShipAddress> qry = from q in db.MangedShipAddresses
        //                                 select q;

        //    return qry;
        //}
        public MangedShipAddress GetById(Int64 ManagedShipId)
        {

            return db.MangedShipAddresses.FirstOrDefault(i => i.ManagedShipId == ManagedShipId);
        }

        public MangedShipAddress GetAllRecord()
        {
            return db.MangedShipAddresses.FirstOrDefault();
        }
    }
}
