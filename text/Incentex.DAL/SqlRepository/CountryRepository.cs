using System;
using System.Collections.Generic;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class CountryRepository : RepositoryBase
    {
        IQueryable<INC_Country> GetAllQuery()
        {
            IQueryable<INC_Country> qry = from c in db.INC_Countries
                                          orderby c.sCountryName
                                          select c;
            return qry;
        }

        public INC_Country GetById(Int64 countryID)
        {
            return db.INC_Countries.FirstOrDefault(le => le.iCountryID == countryID);
        }

        public INC_Country GetByCountryName(String countryName)
        {
            return db.INC_Countries.FirstOrDefault(le => le.sCountryName.Trim().ToUpper() == countryName.Trim().ToUpper());
        }

        public List<INC_Country> GetAll()
        {
            return GetAllQuery().ToList();
        }

        public List<CountryCurrency> GetCountryCurrencyList()
        {
            return db.CountryCurrencies.ToList();
        }

        public CountryCurrency CountryCurrencySymbol(String CCCode)
        {
            return db.CountryCurrencies.Where(q => q.CurrencyCode == CCCode).FirstOrDefault();
        }

        public String CountryCurrencyFlag(String CCCode)
        {
            return db.CountryCurrencies.Where(q => q.CurrencyCode == CCCode).FirstOrDefault().CountryCurrencyFlagIcon;
        }
    }
}