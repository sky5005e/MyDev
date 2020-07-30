using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class DataAccessRepository :RepositoryBase
    {
        IQueryable<INC_DataPrivilege> GetAllQuery()
        {
            IQueryable<INC_DataPrivilege> qry = from d in db.INC_DataPrivileges
                                                select d;
            return qry;
        }
        public List<INC_DataPrivilege> GetDataAccess()
        {
            return GetAllQuery().ToList();
        }

        public List<INC_DataPrivilege> GetDataRestrictionByEmployeeID(string DataRestriction)
        {

            return GetAllQuery().WithDataPrivilgeTypes(DataRestriction).ToList();

        }



    }
    public static class INC_DataPrivilegeExtension
    {
        public static IQueryable<INC_DataPrivilege> WithDataPrivilgeTypes(this IQueryable<INC_DataPrivilege> qry, string DataPrivildgeType)
        {
            string[] DataPrivildgeTypeid = DataPrivildgeType.Split(',');

            return from m in qry
                   where DataPrivildgeTypeid.Contains(m.iDataPrivilegeId.ToString())
                   select m;
        }
    }
}
