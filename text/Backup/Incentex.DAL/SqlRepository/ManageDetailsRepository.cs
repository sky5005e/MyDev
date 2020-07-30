using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class ManageDetailsRepository : RepositoryBase
    {
        /// <summary>
        /// Displays the full INC_ManageDetail table
        /// Nagmani Kumar 
        /// </summary>
        /// <returns></returns>
        //IQueryable<INC_ManageDetail> GetAllQuery()
        //{
        //    IQueryable<INC_ManageDetail> qry = from C in db.INC_ManageDetails
        //                                       select C;
        //    return qry;
        //}
        public List<INC_ManageDetail> GetManageDetailName(long lMenuPrivilegeID)
        {
            IQueryable<INC_ManageDetail> qry = db.INC_ManageDetails.Where(c => c.iMenuPrivilegeID == lMenuPrivilegeID);
            List<INC_ManageDetail> objList = qry.ToList();
            return objList;
        }
    }
}
