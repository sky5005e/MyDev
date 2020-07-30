using System;
using System.Collections.Generic;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class CityRepository : RepositoryBase
    {
        IQueryable<INC_City> GetAllQuery()
        {
            IQueryable<INC_City> qry = from c in db.INC_Cities
                                       orderby c.sCityName
                                       select c;
            return qry;
        }

        public INC_City GetById(Int64 cityId)
        {
            return db.INC_Cities.FirstOrDefault(le => le.iCityID == cityId);
        }

        public List<INC_City> GetByStateId(Int64 stateID)
        {
            return db.INC_Cities.Where(le => le.iStateID == stateID).ToList();
        }

        public INC_City CheckIfExist(Int64 countryID, Int64 stateID, String cityName)
        {
            return db.INC_Cities.FirstOrDefault(le => le.iCountryID == countryID && le.iStateID == stateID && le.sCityName.Trim().ToUpper() == cityName.Trim().ToUpper());
        }
    }
}