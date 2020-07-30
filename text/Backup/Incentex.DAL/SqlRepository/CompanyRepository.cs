using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Incentex.DAL.SqlRepository
{
    public class CompanyRepository : RepositoryBase
    {
        public IQueryable<Company> GetAllQuery()
        {
            IQueryable<Company> qry = from C in db.Companies
                                      select C;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <returns></returns>
        public List<Company> GetAllCompany()
        {
            //IQueryable<Company> qry = GetAllQuery().OrderBy(t => t.CompanyName);

            List<Company> objList = (from C in db.Companies
                                     orderby C.CompanyName
                                     select C).ToList();
            return objList;
        }

        public List<Company> GetIECompany()
        {
            List<Company> objList = (from c in db.Companies
                                     select c)
                                     .OrderBy(x => x.CompanyName)
                                     .ToList();

            return objList;
        }

        public List<Company> GetCompaniesWithIssuance()
        {
            List<Company> objList = (from C in db.Companies                                     
                                     join UI in db.UserInformations on C.CompanyID equals UI.CompanyId
                                     join CE in db.CompanyEmployees on UI.UserInfoID equals CE.UserInfoID
                                     join UIP in db.UniformIssuancePolicies on CE.WorkgroupID equals UIP.WorkgroupID
                                     orderby C.CompanyName
                                     select C)
                                     .Distinct()
                                     .OrderBy(x => x.CompanyName)
                                     .ToList();

            return objList;
        }

        public DataTable GetCompany()
        {
            DataTable dt = new DataTable();
            var list = from c in db.Companies
                       orderby c.CompanyName
                       select new { c.CompanyID, c.CompanyName };
            dt = ListToDataTable(list);
            return dt;
        }
        public static DataTable ListToDataTable<T>(IEnumerable<T> list)
        {
            var dt = new DataTable();
            foreach (var info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (var t in list)
            {
                var row = dt.NewRow();
                foreach (var info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        IQueryable<INC_BasedStation> GetAllBaseStation1()
        {
            IQueryable<INC_BasedStation> qry = from C in db.INC_BasedStations
                                               select C;
            return qry;
        }

        public List<INC_BasedStation> GetAllBaseStationResult()
        {
            IQueryable<INC_BasedStation> qry = GetAllBaseStation1();
            List<INC_BasedStation> objList = qry.ToList();
            return objList;
        }


        /// <summary>
        /// GetById
        /// Reterieve  List single Reord of Company by companyId
        /// </summary>
        /// Nagmani 08/09/2010
        /// Update By : Gaurang Pathak
        /// <param name="companyId"></param>
        public List<Company> GetByCompanyId(Int64 CompanyId)
        {
            //List<Company> objList = GetAllQuery().Where(s => s.CompanyID == CompanyId).ToList();
            List<Company> objList = (from C in db.Companies
                                     where C.CompanyID == CompanyId
                                     select C).ToList();
            return objList;
        }

        /// <summary>
        /// GetById
        /// Reterieve  Table single Reord of Company by companyId
        /// </summary>
        /// <param name="companyId"></param>
        public Company GetById(Int64 CompanyId)
        {
            return db.Companies.Where(c => c.CompanyID == CompanyId).FirstOrDefault();
        }

        /// <summary>
        /// TO get User ID by company employee ID
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public Int64 GetUserInfoIDByEmpId(Int64 empID)
        {
            var ID = (from u in db.UserInformations
                      join c in db.CompanyEmployees on u.UserInfoID equals c.UserInfoID
                      where c.CompanyEmployeeID == empID && u.IsDeleted == false
                      select u.UserInfoID).FirstOrDefault();
            Int64 userID = Convert.ToInt64(ID);
            return userID;
        }

        public class CompanyContactBillingResult
        {
            public System.Int64 CompanyID

            { get; set; }
            public System.String CompanyName

            { get; set; }
            public System.Int64 StationID

            { get; set; }
            public System.String Country

            { get; set; }
            public System.String Zip

            { get; set; }
            public System.String Telephone
            { get; set; }
            public System.Int32 NoOfEmp
            { get; set; }

        }

        //public List<CompanyContactBillingResult> CompanyAndDetails()
        //{

        //    return (from ords in db.GetTable<Company>()

        //            joi n dets in db.GetTable<INC_Country>()
        //            on ords.CountryId equals dets.iCountryID
        //            orderby ords.CompanyID ascending
        //            select new CompanyContactBillingResult

        //            {

        //                CompanyID = ords.CompanyID,
        //                CompanyName = ords.CompanyName,
        //                Country = dets.sCountryName,
        //                Telephone = ords.Telephone

        //            }

        //            ).ToList<CompanyContactBillingResult>();



        //}
        public List<CompanyContactBillingResult> CompanyAndDetails()
        {
            //IncentexBEDataContext dc = new IncentexBEDataContext();
            List<CompanyContactBillingResult> objList = new List<CompanyContactBillingResult>();
            objList = (from ords in db.GetTable<Company>()

                       join dets in db.GetTable<INC_Country>()
                       on ords.CountryId equals dets.iCountryID
                       orderby ords.CompanyID ascending
                       select new CompanyContactBillingResult

                       {

                           CompanyID = ords.CompanyID,
                           CompanyName = ords.CompanyName,
                           Country = dets.sCountryName,
                           Telephone = ords.Telephone

                       }

                    ).ToList<CompanyContactBillingResult>();

            foreach (CompanyContactBillingResult obj in objList)
            {
                obj.NoOfEmp = GetTotalEmployeeByCompanyID((int)obj.CompanyID);
                obj.StationID = GetTotalStationByCompanyId((int)obj.CompanyID);
            }
            return objList;
        }
     
        /// <summary>
        /// GetSingleCompany
        /// Reterieve  Table single Reord of Company by companyId
        /// </summary>
        /// Nagmani 08/09/2010
        /// <param name="companyId"></param>
        public Company GetSingleCompany(int ComapnyID)
        {
            return (from e in db.GetTable<Company>()
                    where (e.CompanyID == ComapnyID)
                    select e).SingleOrDefault<Company>();
        }
        
        /// <summary>
        /// Delete a comapny by Company ID
        /// </summary>
        /// <param name="customerID"></param>
        public void DeleteCompany(string ComapnyID)
        {

            var matchedCompany = (from c in db.GetTable<Company>()
                                  where c.CompanyID == Convert.ToInt32(ComapnyID)
                                  select c).SingleOrDefault();
            try
            {
                if (matchedCompany != null)
                {
                    db.Companies.DeleteOnSubmit(matchedCompany);

                }
                db.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        //update by mayur rathod on 3rd-Jan-2012
        public int GetTotalStationByCompanyId(int CompanyID)
        {
            var matches =
            (from od in db.CompanyEmployeeContactInfos
             join ui in db.UserInformations on od.UserInfoID equals ui.UserInfoID
             where ui.CompanyId == CompanyID && od.OrderType == "AdditionalStation" && od.ContactInfoType == Incentex.DAL.Common.DAEnums.CompanyEmployeeContactInfo.Shipping.ToString()
             && ui.IsDeleted == false
             group od by new { od.Station } into newod
             select newod.Key).Count();
            if (matches != null)
            {
                return matches;
            }
            else
                return 0;
        }

        //Updated by Ankit Shah on 27 Nov 2010
        public int GetTotalEmployeeByCompanyID(int CompanyID)
        {
            var matches =
                (from od in db.GetTable<CompanyEmployee>()
                 join d in db.UserInformations
                          on od.UserInfoID equals d.UserInfoID
                 where d.CompanyId == CompanyID && (d.Usertype == 3 || d.Usertype == 4)
                 && d.IsDeleted == false
                 select od.CompanyEmployeeID).Count();
            if (matches != null)
            {
                return matches;
            }
            else
                return 0;
        }
 
        public IQueryable BindCompany(int startRowIndex, int maximumRows)
        {

            var query = from com in db.Companies
                        join d in db.INC_Countries
                            on com.CountryId equals d.iCountryID
                        select new
                        {

                            CompanyID = com.CompanyID,
                            CompanyName = com.CompanyName,
                            Country = d.sCountryName,
                            Telephone = com.Telephone
                        };

            return query.Skip(startRowIndex).Take(maximumRows);
        }
        
        public int GetCompanyCount()
        {
            return (from com in db.Companies
                    select com).Count();

        }
        
        public List<INC_Lookup> GetLookupDocumentDetails(string strLookupCode)
        {
            return (from look in db.GetTable<INC_Lookup>()
                    where look.iLookupCode == strLookupCode
                    select look
                    ).ToList();
        }
        
        /// <summary>
        /// CheckDuplicaetDocument()
        /// Check the CompanyDuplication]
        /// Nagmani Kumar 16/09/2010
        /// </summary>
        /// <param name="documnetName"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public int CheckDuplicaetDocument(string documnetName, int companyid)
        {
            return (from com in db.Documents
                    where com.FileName == documnetName && com.ForeignKey == companyid
                    select com).Count();
        }
        
        public int CheckDuplicate(string CompanyName, int? CompanyId, string mode)
        {
            return (int)db.INC_SelectCompanyDuplicate(CompanyName, CompanyId, mode).SingleOrDefault().IsDuplicate;
        }

        public List<Company> GetCompanyNotCompanyStore(string companyIds)
        {

            return GetAllQuery().WithStoreIds(companyIds).ToList();

        }
       
        public List<GetAllBaseStationsResult> GetAllBaseStation()
        {
            return db.GetAllBaseStations().ToList();
        }
        //objBaseStation = DBConcurrencyException GetAllBaseStations();

        public void BulkUploadCustomerEmployee()
        {
            db.selectinsertbulkdatafromexcel();
        }


    }

    public static class CompanyExtension
    {
        public static IQueryable<Company> WithStoreIds(this IQueryable<Company> qry, string CompanyIds)
        {
            string[] companyid = CompanyIds.Split(',');

            return from m in qry
                   where !(companyid.Contains(m.CompanyID.ToString()))
                   select m;
        }
    }


}

