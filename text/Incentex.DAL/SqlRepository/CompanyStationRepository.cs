using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class CompanyStationRepository : RepositoryBase
    {

        IQueryable<CompanyStation> GetAllQuery()
        {
            IQueryable<CompanyStation> qry = from companystations in db.CompanyStations
                                             join inc_countries in db.INC_Countries on new { CountryId = companystations.CountryId } equals new { CountryId = inc_countries.iCountryID }
                                             join inc_states in db.INC_States on new { StateId = companystations.StateId } equals new { StateId = inc_states.iStateID }
                                             join inc_cities in db.INC_Cities on new { CityId = companystations.CityId } equals new { CityId = inc_cities.iCityID }
                                             select companystations;
            return qry;
        }

        /*
        IEnumerable GetQry()
        {
            var qry1 = from companystations in db.CompanyStations
                       join inc_countries in db.INC_Countries on new { CountryId = companystations.CountryId } equals new { CountryId = inc_countries.iCountryID }
                       join inc_states in db.INC_States on new { StateId = companystations.StateId } equals new { StateId = inc_states.iStateID }
                       join inc_cities in db.INC_Cities on new { CityId = companystations.CityId } equals new { CityId = inc_cities.iCityID }
                       select new
                       {
                           companystations.StationID,
                           companystations.CompanyID,
                           companystations.StationCode,
                           companystations.AirportName,
                           companystations.Address,
                           companystations.CountryId,
                           companystations.StateId,
                           companystations.CityId,
                           companystations.Telephone,
                           companystations.Fax,
                           companystations.OperatingSinceDate,
                           companystations.StationSetupDate,
                           companystations.StationCostCenter,
                           companystations.StationNumber,
                           companystations.ThirdPartyCompanyName,
                           companystations.ThirdPartyOperartinSince,
                           companystations.ThirdPartyCorporateContact,
                           companystations.ThirdPartyContractTerms,
                           companystations.StationServices,
                           companystations.SeasonalWeather,
                           companystations.StationIdInput,
                           companystations.Active,
                           inc_cities.sCityName,
                           inc_countries.sCountryName,
                           inc_states.sStatename
                       };

            return qry1;
        }
        */

        public enum StationSortExpType
        {
            StationCode,
            sCountryName,

        }

        public IEnumerable GetByCompanyId(Int64 CompanyId, StationSortExpType SortExp, DAEnums.SortOrderType SortOrder)
        {
           // List<CompanyStation> objList = GetAllQuery().Where(s => s.CompanyID == CompanyId).ToList();
        
           //IQueryable qry1 = GetQry()

            var qry = (from companystations in db.CompanyStations

                       join inc_countries in db.INC_Countries on new { CountryId = companystations.CountryId } equals new { CountryId = inc_countries.iCountryID }
                       join inc_states in db.INC_States on new { StateId = companystations.StateId } equals new { StateId = inc_states.iStateID }
                       join inc_cities in db.INC_Cities on new { CityId = companystations.CityId } equals new { CityId = inc_cities.iCityID }
                       
                       
                       where companystations.CompanyID == CompanyId
                       select new
                       {
                           companystations.StationID,
                           companystations.CompanyID,
                           companystations.StationCode,
                           companystations.AirportName,
                           companystations.Address,
                           companystations.CountryId,
                           companystations.StateId,
                           companystations.CityId,
                           companystations.Telephone,
                           companystations.Fax,
                           companystations.OperatingSinceDate,
                           companystations.StationSetupDate,
                           companystations.StationCostCenter,
                           companystations.StationNumber,
                           companystations.ThirdPartyCompanyName,
                           companystations.ThirdPartyOperartinSince,
                           companystations.ThirdPartyCorporateContact,
                           companystations.ThirdPartyContractTerms,
                           companystations.StationServices,
                           companystations.SeasonalWeather,
                           companystations.StationIdInput,
                           companystations.Active,
                           inc_cities.sCityName,
                           inc_countries.sCountryName,
                           inc_states.sStatename,
                           companystations.Zip,

                       });
             
            var List = qry.ToList();

            switch (SortExp)
            {
                case StationSortExpType.StationCode:
                    //List = (SortOrder == DAEnums.SortOrderType.Asc) ? qry.OrderBy(s => s.CompanyWebsite).ToList() : qry.OrderByDescending(s => s.CompanyWebsite).ToList(); 
                    List = qry.OrderBy(s => s.StationCode).ToList();
                    break;
                case StationSortExpType.sCountryName:
                    //List = (SortOrder == DAEnums.SortOrderType.Asc) ? qry.OrderBy(s => s.CompanyWebsite).ToList() : qry.OrderByDescending(s => s.CompanyWebsite).ToList(); 
                    List = qry.OrderBy(s => s.sCountryName).ToList();
                    break;

            }

            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                List.Reverse();
            }

            return List;
        }

        /// <summary>
        /// Gets the company station by company ID.
        /// </summary>
        /// <param name="CompanyID">The company ID.</param>
        /// <returns></returns>
        public List<CompanyStation> GetCompanyStationByCompanyID(Int64 CompanyID)
        {
            return db.CompanyStations.Where(cs => cs.CompanyID == CompanyID).ToList<CompanyStation>();
        }

        public List<SelectCompanyStationStatusResult> SelectStationStatus(int Active)
        {

            try
            {

                var obj = db.SelectCompanyStationStatus(Active);
                return obj.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public CompanyStation GetById(Int64 StationId)
        {
            CompanyStation obj = GetSingle(GetAllQuery().Where(s => s.StationID == StationId).ToList());
            return obj;
        }

        public void Delete(Int64 StationId)
        {
            CompanyStation obj = GetById(StationId);

            if(obj != null)
            {
            //delete from company station user

                CompanyStationUserInformationRepository objCompanyUserRepo = new CompanyStationUserInformationRepository();
                objCompanyUserRepo.Delete(obj.StationID);

            //delete from Notes
                NotesHistoryRepository objNotesHistoryRepository = new NotesHistoryRepository();
                objNotesHistoryRepository.Delete(obj.StationID, Incentex.DAL.Common.DAEnums.NoteForType.Station);

             //delete from CompanyStationUserInformation

                CompanyStationsServiceMapRepository objCompanyStationsServiceMapRepository = new CompanyStationsServiceMapRepository();
                objCompanyStationsServiceMapRepository.Delete(obj.StationID);

            //delete record

                Delete(obj);
                SubmitChanges();

            }

        }

    }//class
}
