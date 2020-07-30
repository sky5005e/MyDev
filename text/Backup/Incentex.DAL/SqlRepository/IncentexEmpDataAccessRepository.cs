using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class IncentexEmpDataAccessRepository : RepositoryBase
    {
        IQueryable<IncEmpDataAccess> GetAllQuery()
        {
            IQueryable<IncEmpDataAccess> qry = from d in db.IncEmpDataAccesses
                                               select d;
            return qry;
        }

        public List<IncEmpDataAccess> GetDataByEmployeeId(Int64 EmployeeId)
        {
            //return GetAllQuery().Where(s => s.IncentexEmployeeID == EmployeeId).ToList();

            return (from d in db.IncEmpDataAccesses
                    where d.IncentexEmployeeID == EmployeeId
                    select d).ToList();
        }
    }
}
