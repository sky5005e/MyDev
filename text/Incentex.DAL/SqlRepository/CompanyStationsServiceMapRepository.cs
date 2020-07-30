using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class CompanyStationsServiceMapRepository : RepositoryBase
    {
        IQueryable<CompanyStationsServiceMap> GetAllQuery()
        {
            IQueryable<CompanyStationsServiceMap> qry = from q in db.CompanyStationsServiceMaps
                                                        select q;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="StationId"></param>
        /// <returns></returns>
        public List<CompanyStationsServiceMap> GetByStationId(Int64 StationId)
        {
            //IQueryable<CompanyStationsServiceMap> qry = GetAllQuery().Where( s => s.StationID == StationId );
            //List<CompanyStationsServiceMap> objList = qry.ToList();

            List<CompanyStationsServiceMap> objList = (from q in db.CompanyStationsServiceMaps
                                                       where q.StationID == StationId
                                                       select q).ToList();
            return objList;
        }


        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="StationId"></param>
        /// <param name="LookupID"></param>
        /// <returns></returns>
        public CompanyStationsServiceMap GetByStationIdAndLookupID(Int64 StationId, Int64 LookupID)
        {
            //IQueryable<CompanyStationsServiceMap> qry = GetAllQuery().Where(s => s.StationID == StationId && s.LookupID == LookupID);
            //CompanyStationsServiceMap obj = GetSingle(qry.ToList());

            CompanyStationsServiceMap obj = (from q in db.CompanyStationsServiceMaps
                                             where q.StationID == StationId &&
                                             q.LookupID == LookupID
                                             select q).FirstOrDefault();

            return obj;
        }

        public void Delete(Int64 StationID)
        {
            List<CompanyStationsServiceMap> objList = GetAllQuery().Where(m => m.StationID == StationID).ToList();
            db.CompanyStationsServiceMaps.DeleteAllOnSubmit(objList);
            db.SubmitChanges();
        }

    }

}
