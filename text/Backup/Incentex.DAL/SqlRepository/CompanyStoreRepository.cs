using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace Incentex.DAL.SqlRepository
{
    public class CompanyStoreRepository : RepositoryBase
    {
        #region Common Functions
        /// <summary>
        /// Fucntion to get all the record of a CompanyStore
        /// </summary>
        /// <returns></returns>
        IQueryable<CompanyStore> GetAllQuery()
        {
            IQueryable<CompanyStore> qry = from C in db.CompanyStores
                                           select C;
            return qry;
        }
        /// <summary>
        /// Same function as above one but it returns record in generic list
        /// </summary>
        /// <returns>List<CompanyStore></returns>
        public List<CompanyStore> GetAllCompanyStore()
        {
            return GetAllQuery().ToList();
        }
        /// <summary>
        /// Returnds single Object by companystoreId
        /// </summary>
        /// <param name="CompanyStoreId"></param>
        /// <returns>Single Object of CompanyStore</returns>
        public CompanyStore GetById(Int64 CompanyStoreId)
        {
            return db.CompanyStores.FirstOrDefault(s => s.StoreID == CompanyStoreId);
        }

        /// <summary>
        /// Returnds single Object by companyId
        /// </summary>
        /// <param name="CompanyStoreId"></param>
        /// <returns>Single Object of CompanyStore</returns>
        public CompanyStore GetByCompanyId(Int64 CompanyId)
        {
            return db.CompanyStores.FirstOrDefault(s => s.CompanyID == CompanyId);
        }

        public List<SelectCompanyNameCompanyIDResult> GetBYStoreId(Int32 StoreId)
        {
            List<SelectCompanyNameCompanyIDResult> query;
            return query = db.SelectCompanyNameCompanyID(StoreId).ToList();
        }
        /// <summary>
        /// Get All store which returns all the store with customized and required fields
        /// </summary>
        /// <returns>List<IECompanyListResults></returns>
        public List<IECompanyListResults> GetCompanyStore()
        {
            return
            (from cs in db.CompanyStores
             join c in db.Companies on cs.CompanyID equals c.CompanyID
             //join ship in db.INC_Lookups on cs.ShippingTypeID equals ship.iLookupID
             join storestatus in db.INC_Lookups on cs.StoreStatusID equals storestatus.iLookupID
             join country in db.INC_Countries on c.CountryId equals country.iCountryID
             select new IECompanyListResults
             {
                 StoreId = cs.StoreID,
                 CompanyId = c.CompanyID,
                 Company = c.CompanyName,
                 CountryId = country.iCountryID,
                 CountryName = country.sCountryName,
                 //Shipping = ship.sLookupName,
                 StoreStatus = storestatus.sLookupName,
                 SAPCompanyCode = c.SAPCompanyCode
             }
             ).ToList<IECompanyListResults>();

        }

        public IECompanyListResults GetCompanyStore(Int64 CompanyID)
        {
            return
            (from cs in db.CompanyStores
             join c in db.Companies on cs.CompanyID equals c.CompanyID
             //join ship in db.INC_Lookups on cs.ShippingTypeID equals ship.iLookupID
             join storestatus in db.INC_Lookups on cs.StoreStatusID equals storestatus.iLookupID
             join country in db.INC_Countries on c.CountryId equals country.iCountryID
             where cs.CompanyID == CompanyID
             select new IECompanyListResults
             {
                 StoreId = cs.StoreID,
                 CompanyId = c.CompanyID,
                 Company = c.CompanyName,
                 CountryId = country.iCountryID,
                 CountryName = country.sCountryName,
                 //Shipping = ship.sLookupName,
                 StoreStatus = storestatus.sLookupName,
                 SAPCompanyCode = c.SAPCompanyCode
             }
             ).FirstOrDefault();
        }

        /// <summary>
        /// Gets the name of the company store and storeid for dropdown bind.
        /// </summary>
        /// <returns></returns>
        public List<SelectCompanyStoreNameResult> GetCompanyStoreName()
        {
            return db.SelectCompanyStoreName().ToList();
        }

        /// <summary>
        /// Gets Company ID by Company Name
        /// </summary>
        /// <returns></returns>
        public Int64 GetCompanyIDByName(String comName)
        {
            return db.Companies.Where(q => q.CompanyName == comName).Select(s => s.CompanyID).FirstOrDefault();
        }
        /// <summary>
        /// Get StoreID By Company Name
        /// </summary>
        /// <param name="CompanyName"></param>
        /// <returns></returns>
        public Int64 GetStoreIDByCompanyName(String CompanyName)
        {
            return (from c in db.Companies
                    join cs in db.CompanyStores on c.CompanyID equals cs.CompanyID
                    where c.CompanyName == CompanyName
                    select cs.StoreID).FirstOrDefault();
        }

        /// <summary>
        /// Delete store by storeId
        /// </summary>
        /// <param name="StoreId"></param>
        public void DeleteStore(Int64 StoreId)
        {
            try
            {
                CompanyStore objStore = GetById(StoreId);
                Delete(objStore);
                SubmitChanges();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get splash images for front side
        public List<SelectSplashImagesByWorkGroupResult> GetSplashImagesByWorkGroup(Int64 storeId, Int64 employeeId)
        {

            var obj = (from a in db.SelectSplashImagesByWorkGroup(storeId, employeeId, "SplashImage")
                       select a);
            return obj.ToList();

        }

        //Get Guidelines Manuals for front side
        public List<SelectSplashImagesByWorkGroupResult> GetGuideLinesManualsByWorkGroup(Int64 storeId, Int64 employeeId)
        {

            var obj = (from a in db.SelectSplashImagesByWorkGroup(storeId, employeeId, "GuideLineManuals")
                       select a);
            return obj.ToList();

        }

        public List<SelectSplashImagesByCompanyEmployeeResult> GetSplashImagesByCompanyEmployee(Int64 companyId, Int64 userinfoid)
        {

            var obj = (from a in db.SelectSplashImagesByCompanyEmployee(companyId, userinfoid)
                       select a);
            return obj.ToList();

        }

        public List<SelectGuidelineByCompanyEmployeeResult> GetGuideLinesManualByCompanyEmployee(Int64 companyId, Int64 userinfoid)
        {

            var obj = (from a in db.SelectGuidelineByCompanyEmployee(companyId, userinfoid)
                       select a);
            return obj.ToList();

        }

        public Int64 GetCompanyStoreStatusByCompanyId(Int64 CompanyId)
        {
            //return GetSingle(GetAllQuery().Where(s => s.CompanyID == CompanyId).ToList()).StoreStatusID;

            return (from C in db.CompanyStores
                    where C.CompanyID == CompanyId
                    select C.StoreStatusID).FirstOrDefault();

        }
        public Int64 GetStoreIDByCompanyId(Int64 CompanyId)
        {
            return db.CompanyStores.FirstOrDefault(le => le.CompanyID == CompanyId).StoreID;
        }

        public List<GetStoreDepartmentsResult> GetStoreDepartments(Int64 StoreID)
        {
            return db.GetStoreDepartments(StoreID).ToList();
        }

        public List<GetStoreWorkGroupsResult> GetStoreWorkGroups(Int64 StoreID)
        {
            return db.GetStoreWorkGroups(StoreID).ToList();
        }

        public StoreDepartment GetStoreDepartmentByStoreNDepartmentID(Int64 StoreID, Int64 DepartmentID)
        {
            return db.StoreDepartments.FirstOrDefault(le => le.StoreID == StoreID && le.DepartmentID == DepartmentID);
        }

        public StoreWorkGroup GetStoreWorkGroupByStoreNWorkGroupID(Int64 StoreID, Int64 WorkGroupID)
        {
            return db.StoreWorkGroups.FirstOrDefault(le => le.StoreID == StoreID && le.WorkGroupID == WorkGroupID);
        }

        #endregion

        #region Extension Class
        public class IECompanyListResults
        {
            public Int64 StoreId { get; set; }
            public Int64 CompanyId { get; set; }
            public String Company { get; set; }
            public String CountryName { get; set; }
            public Int64 CountryId { get; set; }
            public String Shipping { get; set; }
            public String StoreStatus { get; set; }
            public String MasterItemName { get; set; }
            public String SAPCompanyCode { get; set; }
        }
        #endregion
    }
}