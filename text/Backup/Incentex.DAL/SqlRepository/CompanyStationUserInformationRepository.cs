using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class CompanyStationUserInformationRepository : RepositoryBase
    {
        IQueryable<CompanyStationUserInformation> GetAllQuery()
        {
            IQueryable<CompanyStationUserInformation> qry = from u in db.CompanyStationUserInformations
                                                            select u;
            return qry;
        }
        
        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary> 
        /// <param name="StationId"></param>
        /// <param name="UserType">manager | admin</param>
        /// <returns></returns>
        public CompanyStationUserInformation GetByStationId(Int64 StationId,string UserType)
        {
            //CompanyStationUserInformation obj = GetSingle(GetAllQuery().Where(u => u.StationID == StationId && u.StationUserType.Equals(UserType)).ToList());

            CompanyStationUserInformation obj = (from u in db.CompanyStationUserInformations
                                                 where u.StationID == StationId &&
                                                 u.StationUserType.Equals(UserType)
                                                 select u).FirstOrDefault();

            return obj;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="CompanyStationUserInformationID"></param>
        /// <returns></returns>
        public CompanyStationUserInformation GetById(Int64 CompanyStationUserInformationID)
        {
            //CompanyStationUserInformation obj = GetSingle(GetAllQuery().Where( u => u.CompanyStationUserInformationID == CompanyStationUserInformationID).ToList());
            CompanyStationUserInformation obj = (from u in db.CompanyStationUserInformations
                                                 where u.CompanyStationUserInformationID == CompanyStationUserInformationID
                                                 select u).FirstOrDefault(); 

            return obj;
        }

        public void Delete(Int64 StationId)
        {
            List<CompanyStationUserInformation> objList = GetAllQuery().Where(u => u.StationID == StationId).ToList();

            db.CompanyStationUserInformations.DeleteAllOnSubmit(objList);
            db.SubmitChanges();

        }
    }
}
