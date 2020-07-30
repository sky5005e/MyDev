using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
   public class SupplierDataAccessRepository:RepositoryBase
    {
        IQueryable<SupplierEmpDataAccess> GetAllQuery()
        {
            IQueryable<SupplierEmpDataAccess> qry = from d in db.SupplierEmpDataAccesses
                                               select d;
            return qry;
        }
        public List<SupplierEmpDataAccess> GetDataBySupplierEmpId(Int64 SupplierEmpId)
        {
            return GetAllQuery().Where(s => s.SupplierEmployeeID == SupplierEmpId).ToList();
        }
    }
}
