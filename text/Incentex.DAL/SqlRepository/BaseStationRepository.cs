using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
   public class BaseStationRepository:RepositoryBase
    {
        public List<INC_BasedStation> GetAllBaseStation()
        {
            return db.INC_BasedStations.ToList();
        }
        public INC_BasedStation GetById(Int64 BaseStationId)
        {
            return db.INC_BasedStations.SingleOrDefault(s => s.iBaseStationId == BaseStationId);
        }
        public INC_BasedStation GetByName(string BaseStation)
        {
            return db.INC_BasedStations.FirstOrDefault(s => s.sBaseStation == BaseStation);
        }

        public List<INC_BasedStation> GetAllBaseStationbyCountryID(Int64 countryID)
        {
            List<INC_BasedStation> Objlist = (from b in db.INC_BasedStations
                                              where b.iCountryID == countryID
                                              select b).ToList();
            return Objlist;
        }

        public List<INC_BasedStation> GetAssociateBaseStation(String BaseStationIDs)
        {
            List<INC_BasedStation> Objlist = new List<INC_BasedStation>();

            if (BaseStationIDs != "")
            {
                String[] IdsAry = BaseStationIDs.Split(',');
                Objlist = (from b in db.INC_BasedStations
                           where IdsAry.Contains(b.iBaseStationId.ToString())
                           select b).ToList();
            }
            else
            {
                Objlist = db.INC_BasedStations.ToList();
            }
            return Objlist;
        }

        public List<INC_BasedStation> GetBaseStationFromCompany(Int64 CompanyID)
        {
            List<INC_BasedStation> objList = (from bs in db.INC_BasedStations
                                              join c in db.Companies on bs.iCountryID equals c.CountryId
                                              where c.CompanyID == CompanyID
                                              select bs)
                                              .OrderBy(x => x.sBaseStation)
                                              .ToList();
            return objList;
        }
    }
}
