using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class WorldwideProspectsRepository : RepositoryBase
    {
        #region Common Functions

        public List<WorldwideprospectCustom> GetAllworldwideprospectBysearch(string company, string contact, string Email, long businesstype,
            long country)
        {
            return (from w in db.WorldWideProspects
                    join l in db.INC_Lookups on w.BusinessTypeID equals l.iLookupID
                    join c in db.INC_Countries on w.CountryID equals c.iCountryID
                    where w.IsDeleted == false &&
                          ((!string.IsNullOrEmpty(company) ? w.CompanyName.Contains(company) : true) &&
                           (!string.IsNullOrEmpty(contact) ? w.ContactName.Contains(contact) : true) &&
                           (!string.IsNullOrEmpty(Email) ? w.Address.Contains(Email) : true) &&
                           (businesstype > 0 ? w.BusinessTypeID == businesstype : true) &&
                           (country > 0 ? w.CountryID == country : true)) 
                    select new WorldwideprospectCustom
                    {
                        ProspectID = w.ProspectID,
                        CompanyName = w.CompanyName,
                        ContactName = w.ContactName,
                        BusinessType = l.sLookupName,
                        Email = w.Email, 
                        Country = c.sCountryName
                    }).ToList();
        }

        public WorldWideProspect GetworldwideprospectByID(long ProspectID)
        {
            return (from w in db.WorldWideProspects where w.ProspectID == ProspectID && w.IsDeleted == false  select w).SingleOrDefault();
        }

        public void Deleteprospect(long ProspectID, long UserID)
        {
            WorldWideProspect obj = GetworldwideprospectByID(ProspectID);

            if (obj != null)
            {
                obj.IsDeleted = true;
                obj.DeletedBy = UserID;
                obj.DeletedDate = System.DateTime.Now;
                base.SubmitChanges();
            }
        }

        public void updateprospect(WorldWideProspect objprospect)
        {
            WorldWideProspect newobjprospect = GetworldwideprospectByID(objprospect.ProspectID);
            
            newobjprospect.CompanyName = objprospect.CompanyName;
            newobjprospect.ContactName = objprospect.ContactName;
            newobjprospect.Email = objprospect.Email;
            newobjprospect.BusinessTypeID = objprospect.BusinessTypeID;
            newobjprospect.CountryID = objprospect.CountryID;
            newobjprospect.UpdatedBy = objprospect.UpdatedBy;
            newobjprospect.UpdatedDate = objprospect.UpdatedDate;   

            base.SubmitChanges();  
        }
        
        #endregion

        #region Extension Class
        public class WorldwideprospectCustom
        {
            public long ProspectID { get; set; }

            public string CompanyName { get; set; }

            public string ContactName { get; set; }

            public string Address { get; set; }

            public string Email { get; set; }

            public string BusinessType { get; set; }

            public string Country { get; set; }
        }
        #endregion
    }
}
