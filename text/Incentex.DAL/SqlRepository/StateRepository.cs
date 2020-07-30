using System;
using System.Collections.Generic;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class StateRepository : RepositoryBase
    {
        IQueryable<INC_State> GetAllQuery()
        {
            IQueryable<INC_State> qry = from s in db.INC_States
                                        orderby s.sStatename
                                        select s;
            return qry;
        }

        public List<INC_State> GetByCountryId(Int64 countryID)
        {
            return db.INC_States.Where(S => S.iCountryID == countryID).ToList();
        }

        public INC_State GetById(Int64 stateID)
        {
            return db.INC_States.FirstOrDefault(le => le.iStateID == stateID);
        }

        public String GetStateProvinceCodebyStateId(Int64 stateID)
        {
            return db.INC_StateProvinceCodes.FirstOrDefault(le => le.StateID == stateID).ProvinceCode;
        }

        public TaxRatesForCanada GetTaxRateForCanadaByStateID(Int64 stateID)
        {
            return db.TaxRatesForCanadas.FirstOrDefault(le => le.StateID == stateID);
        }

        public INC_State GetByCountryIDAndStateCode(Int64 countryID, String stateCode)
        {
            return db.INC_States.FirstOrDefault(le => le.iCountryID == countryID && le.sCode == stateCode);
        }
    }
}