using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
   public class IncentexEmpProductReturnReceivedRepository : RepositoryBase 
    {
        IQueryable<IncentexEmpProductReturnReceived> GetAllQuery()
        {
            IQueryable<IncentexEmpProductReturnReceived> qry = from C in db.IncentexEmpProductReturnReceiveds
                                            select C;
            return qry;
        }

       /// <summary>
       /// Update By : Gaurang Pathak
       /// </summary>
       /// <param name="OrderID"></param>
       /// <returns></returns>
        public List<IncentexEmpProductReturnReceived> GetByOrderID(Int64 OrderID)
        {
            //return GetAllQuery().Where(C => C.OrderID == OrderID).ToList();

            return (from C in db.IncentexEmpProductReturnReceiveds
                    where C.OrderID == OrderID
                    select C).ToList(); 

        }

       /// <summary>
       /// Update By : Gaurang Pathak
       /// </summary>
       /// <param name="ProductReturnID"></param>
       /// <returns></returns>
        public List<IncentexEmpProductReturnReceived> GetByProductReturnID(Int64 ProductReturnID)
        {
            //return GetAllQuery().Where(C => C.ProductReturnId == ProductReturnID).ToList();

            return (from C in db.IncentexEmpProductReturnReceiveds
                    where C.ProductReturnId == ProductReturnID
                    select C).ToList(); 
        }

       /// <summary>
       /// Update By : Gaurang Pathak
       /// </summary>
       /// <param name="ReceivedID"></param>
       /// <returns></returns>
        public IncentexEmpProductReturnReceived GetById(Int64 ReceivedID)
        {
            //IncentexEmpProductReturnReceived objNew = GetSingle(GetAllQuery().Where(C => C.ReceivedId == ReceivedID).ToList());

            IncentexEmpProductReturnReceived objNew = (from C in db.IncentexEmpProductReturnReceiveds
                                                       where C.ReceivedId == ReceivedID
                                                       select C).FirstOrDefault(); 

            return objNew;
        }

        public List<IncentexEmpProductReturnReceived> GetAll()
        {
            return GetAllQuery().ToList();
        }
    }
}
