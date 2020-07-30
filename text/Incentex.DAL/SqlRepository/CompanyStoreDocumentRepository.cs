using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{

    public class CompanyStoreDocumentRepository : RepositoryBase
    {
        #region Common Functions
        IQueryable<StoreDocument> GetAllQuery()
        {
            IQueryable<StoreDocument> qry = from d in db.StoreDocuments
                                            select d;
            return qry;
        }

        public StoreDocument GetById(Int64 StoreDocumentId)
        {
            //StoreDocument objStore = GetSingle(GetAllQuery().Where(C => C.StoreDocumentID == StoreDocumentId).ToList());

            StoreDocument objStore = (from d in db.StoreDocuments
                                      where d.StoreDocumentID == StoreDocumentId
                                      select d).SingleOrDefault();
            return objStore;
        }

        /// <summary>
        /// Shehzad 17-Jan-01
        /// Gets the Store Id for the current employee
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <param name="WorkgroupID"></param>
        /// <returns></returns>
        public SelectEmployeeStoreIdResult GetEmpStoreId(Int64 UserInfoID, Int64 WorkgroupID)
        {
            return db.SelectEmployeeStoreId(UserInfoID, WorkgroupID).SingleOrDefault();
        }

        public List<StoreDocument> GetSplashImagesdById(Int64 WorkgroupId, Int64 StoreId)
        {
            //return GetAllQuery().Where(s => s.StoreId == StoreId && s.WorkgroupID == WorkgroupId && s.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.SplashImage.ToString()).ToList();

            return (from d in db.StoreDocuments
                    where d.StoreId == StoreId &&
                          d.WorkgroupID == WorkgroupId &&
                          d.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.SplashImage.ToString()
                    select d).ToList();
        }

        //Delete By Id
        public void DeleteCompanyStoreDocument(string StoreDocumentID)
        {
            var matchedDocument = (from c in db.GetTable<StoreDocument>()
                                   where c.StoreDocumentID == Convert.ToInt64(StoreDocumentID)
                                   select c).SingleOrDefault();
            try
            {
                if (matchedDocument != null)
                {
                    db.StoreDocuments.DeleteOnSubmit(matchedDocument);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Get all company store documents by storeid
        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        public List<StoreDocument> GetAllDocumentsByStore(Int64 StoreId)
        {
            //return GetAllQuery().Where(s => s.StoreId == StoreId).ToList();

            return (from d in db.StoreDocuments
                    where d.StoreId == StoreId
                    select d).ToList();
        }

        #endregion

        #region GuideLine Manuals

        //GuideLine Manuals
        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        public List<StoreDocumentExtension> GetAllGuideLineManuals(Int64 StoreId)
        {
            //return GetAllGuideLineManual().Where(s => s.StoreId == StoreId && s.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.GuideLineManuals.ToString()).ToList();

            return (from d in db.StoreDocuments
                    join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                    join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                    where d.StoreId == StoreId &&
                          d.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.GuideLineManuals.ToString()
                    select new StoreDocumentExtension
                    {
                        StoreId = d.StoreId,
                        DocuemntFor = d.DocuemntFor,
                        Workgroup = il.sLookupName,
                        WorkgroupId = d.WorkgroupID,
                        Department = dept.sLookupName,
                        DepartmentId = d.DepartmentID,
                        DocumentName = d.DocumentName,
                        StoreDocumentId = d.StoreDocumentID
                    }).ToList();
        }


        public IQueryable<StoreDocumentExtension> GetAllGuideLineManual()
        {
            var qry = (from d in db.StoreDocuments
                       join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                       join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                       select new StoreDocumentExtension
                       {
                           StoreId = d.StoreId,
                           DocuemntFor = d.DocuemntFor,
                           Workgroup = il.sLookupName,
                           WorkgroupId = d.WorkgroupID,
                           Department = dept.sLookupName,
                           DepartmentId = d.DepartmentID,
                           DocumentName = d.DocumentName,
                           StoreDocumentId = d.StoreDocumentID
                       }
                       );
            return qry;
            //.Where(s => s.StoreId == StoreId && s.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.GuideLineManuals.ToString()).ToList<StoreDocumentExtension>()
        }

        public class StoreDocumentExtension
        {

            public Int64 WorkgroupId { get; set; }
            public Int64 DepartmentId { get; set; }
            public string Workgroup { get; set; }
            public string Department { get; set; }
            public string DocumentName { get; set; }
            public Int64 StoreDocumentId { get; set; }
            public Int64 StoreId { get; set; }
            public string DocuemntFor { get; set; }
        }
        #endregion

        #region News
        //News

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        public List<NewsExtension> GetAllNewsByStoreId(Int64 StoreId)
        {
            //return GetAllNews().Where(s => s.StoreId == StoreId && s.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.News.ToString()).ToList();

            return (from d in db.StoreDocuments
                    join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                    join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                    join cs in db.CompanyStores on d.StoreId equals cs.StoreID
                    //join ce in db.CompanyEmployees on d.WorkgroupID equals ce.WorkgroupID
                    where d.StoreId == StoreId &&
                          d.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.News.ToString()
                    select new NewsExtension
                    {
                        StoreId = d.StoreId,
                        NewsTitle = d.NewsTitle,
                        NewsPostDate = Convert.ToDateTime(d.NewsPostDate),
                        NewsEndDate = Convert.ToDateTime(d.NewsEndDate),
                        DocuemntFor = d.DocuemntFor,
                        Workgroup = il.sLookupName,
                        WorkgroupId = d.WorkgroupID,
                        Department = dept.sLookupName,
                        DepartmentId = d.DepartmentID,
                        StoreDocumentId = d.StoreDocumentID,
                        //UserInfoID = ce.UserInfoID,
                        CompanyID = cs.CompanyID,
                        NewsTitleDes = d.NewsTitleDes
                    }).ToList();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="WorkgroupID"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public List<NewsExtension> GetAllNewsByWorkgroupId(Int64 WorkgroupID, Int64 CompanyID)
        {
            //return GetAllNews().Where(w => w.WorkgroupId == WorkgroupID && w.CompanyID == CompanyID && w.NewsEndDate > DateTime.Today && w.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.News.ToString()).ToList();

            return (from d in db.StoreDocuments
                    join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                    join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                    join cs in db.CompanyStores on d.StoreId equals cs.StoreID
                    //join ce in db.CompanyEmployees on d.WorkgroupID equals ce.WorkgroupID
                    where d.WorkgroupID == WorkgroupID &&
                          cs.CompanyID == CompanyID &&
                          d.NewsEndDate > DateTime.Today &&
                          d.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.News.ToString()
                    select new NewsExtension
                    {
                        StoreId = d.StoreId,
                        NewsTitle = d.NewsTitle,
                        NewsPostDate = Convert.ToDateTime(d.NewsPostDate),
                        NewsEndDate = Convert.ToDateTime(d.NewsEndDate),
                        DocuemntFor = d.DocuemntFor,
                        Workgroup = il.sLookupName,
                        WorkgroupId = d.WorkgroupID,
                        Department = dept.sLookupName,
                        DepartmentId = d.DepartmentID,
                        StoreDocumentId = d.StoreDocumentID,
                        //UserInfoID = ce.UserInfoID,
                        CompanyID = cs.CompanyID,
                        NewsTitleDes = d.NewsTitleDes
                    }).ToList();
        }

        ////Created On 4 Jan 2012 by Ankit
        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="WorkgroupID"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public List<NewsExtension> GetAllNewsByWorkgroupIdForCA(Int64 WorkgroupID, Int64 CompanyID)
        {
            //return GetAllNews().Where(w => w.WorkgroupId == WorkgroupID && w.CompanyID == CompanyID && w.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.News.ToString()).ToList();

            return (from d in db.StoreDocuments
                    join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                    join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                    join cs in db.CompanyStores on d.StoreId equals cs.StoreID
                    //join ce in db.CompanyEmployees on d.WorkgroupID equals ce.WorkgroupID
                    where d.WorkgroupID == WorkgroupID &&
                          cs.CompanyID == CompanyID &&
                          d.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.News.ToString()
                    select new NewsExtension
                    {
                        StoreId = d.StoreId,
                        NewsTitle = d.NewsTitle,
                        NewsPostDate = Convert.ToDateTime(d.NewsPostDate),
                        NewsEndDate = Convert.ToDateTime(d.NewsEndDate),
                        DocuemntFor = d.DocuemntFor,
                        Workgroup = il.sLookupName,
                        WorkgroupId = d.WorkgroupID,
                        Department = dept.sLookupName,
                        DepartmentId = d.DepartmentID,
                        StoreDocumentId = d.StoreDocumentID,
                        //UserInfoID = ce.UserInfoID,
                        CompanyID = cs.CompanyID,
                        NewsTitleDes = d.NewsTitleDes
                    }).ToList();

        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="WorkgroupID"></param>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        public List<StoreDocumentExtension> GetAllGuideLineManualsByWgIDForCA(Int64 WorkgroupID, Int64 StoreId)
        {
            //return GetAllGuideLineManual().Where(s => s.WorkgroupId == WorkgroupID && s.StoreId == StoreId && s.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.GuideLineManuals.ToString()).ToList();

            return (from d in db.StoreDocuments
                    join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                    join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                    where d.WorkgroupID == WorkgroupID &&
                          d.StoreId == StoreId &&
                          d.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.GuideLineManuals.ToString()
                    select new StoreDocumentExtension
                    {
                        StoreId = d.StoreId,
                        DocuemntFor = d.DocuemntFor,
                        Workgroup = il.sLookupName,
                        WorkgroupId = d.WorkgroupID,
                        Department = dept.sLookupName,
                        DepartmentId = d.DepartmentID,
                        DocumentName = d.DocumentName,
                        StoreDocumentId = d.StoreDocumentID
                    }).ToList();
        }

        public List<ContactExtension> GetAllContactsByWorkgroupIDForCA(Int64 WorkgroupID, Int64 StoreId)
        {
            return GetAllContact().Where(w => w.WorkgroupId == WorkgroupID && w.StoreId == StoreId && w.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.Contact.ToString()).ToList();
        }
        //End

        public IQueryable<NewsExtension> GetAllNews()
        {
            var qry = (from d in db.StoreDocuments
                       join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                       join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                       join cs in db.CompanyStores on d.StoreId equals cs.StoreID
                       //join ce in db.CompanyEmployees on d.WorkgroupID equals ce.WorkgroupID
                       select new NewsExtension
                       {
                           StoreId = d.StoreId,
                           NewsTitle = d.NewsTitle,
                           NewsPostDate = Convert.ToDateTime(d.NewsPostDate),
                           NewsEndDate = Convert.ToDateTime(d.NewsEndDate),
                           DocuemntFor = d.DocuemntFor,
                           Workgroup = il.sLookupName,
                           WorkgroupId = d.WorkgroupID,
                           Department = dept.sLookupName,
                           DepartmentId = d.DepartmentID,
                           StoreDocumentId = d.StoreDocumentID,
                           //UserInfoID = ce.UserInfoID,
                           CompanyID = cs.CompanyID,
                           NewsTitleDes = d.NewsTitleDes
                       }
                       );
            return qry;

        }

        public class NewsExtension
        {

            public Int64 WorkgroupId { get; set; }
            public Int64 DepartmentId { get; set; }
            public string Workgroup { get; set; }
            public string Department { get; set; }
            public string NewsTitle { get; set; }
            //public string NewsPostDate { get; set; }
            public DateTime NewsPostDate { get; set; }
            public DateTime NewsEndDate { get; set; }
            public string DocuemntFor { get; set; }
            public Int64 StoreDocumentId { get; set; }
            public Int64 StoreId { get; set; }
            public Int64 UserInfoID { get; set; }
            public Int64 CompanyID { get; set; }
            public string NewsTitleDes { get; set; }
        }

        #endregion

        #region FAQ
        //FAQ

        public List<FAQExtension> GetAllFAQsByStoreId(Int64 StoreId)
        {
            return GetAllFAQ().Where(s => s.StoreId == StoreId && s.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.FAQ.ToString()).ToList();
        }

        /// <summary>
        /// Shehzad Jan-17-2011
        /// Gets all FAQs for a workgroup in a store
        /// </summary>
        /// <param name="StoreId"></param>
        /// <param name="WorkgroupId"></param>
        /// <returns></returns>
        public List<FAQExtension> GetFAQsByWorkgroupId(Int64 StoreId, Int64 WorkgroupId)
        {
            return GetAllFAQ().Where(w => w.WorkgroupId == WorkgroupId && w.StoreId == StoreId && w.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.FAQ.ToString()).ToList();
        }

        public List<FAQExtension> GetAllFAQsByUser(Int64 WorkgroupID, Int64 CompanyID)
        {
            return GetAllFAQ().Where(w => w.WorkgroupId == WorkgroupID && w.CompanyID == CompanyID && w.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.FAQ.ToString()).ToList();
        }

        public IQueryable<FAQExtension> GetAllFAQ()
        {
            var qry = (from d in db.StoreDocuments
                       join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                       join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                       join cs in db.CompanyStores on d.StoreId equals cs.StoreID
                       //join ce in db.CompanyEmployees on d.WorkgroupID equals ce.WorkgroupID
                       select new FAQExtension
                       {
                           StoreId = d.StoreId,
                           FaqQuestion = d.FaqQuestion,
                           FaqAnswer = d.FaqAnswer,
                           DocuemntFor = d.DocuemntFor,
                           Workgroup = il.sLookupName,
                           WorkgroupId = d.WorkgroupID,
                           Department = dept.sLookupName,
                           DepartmentId = d.DepartmentID,
                           StoreDocumentId = d.StoreDocumentID,
                           //UserInfoID = ce.UserInfoID,
                           CompanyID = cs.CompanyID

                       }
                       );
            return qry;

        }

        public class FAQExtension
        {
            public Int64 WorkgroupId { get; set; }
            public Int64 DepartmentId { get; set; }
            public string Workgroup { get; set; }
            public string Department { get; set; }
            public string FaqQuestion { get; set; }
            public string FaqAnswer { get; set; }
            public Int64 StoreDocumentId { get; set; }
            public Int64 StoreId { get; set; }
            public string DocuemntFor { get; set; }
            public Int64 UserInfoID { get; set; }
            public Int64 CompanyID { get; set; }
        }


        #endregion

        #region Contact
        //Contact

        public List<ContactExtension> GetAllContactByStoreId(Int64 StoreId)
        {
            return GetAllContact().Where(s => s.StoreId == StoreId && s.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.Contact.ToString()).ToList();
        }

        /// <summary>
        /// Created Date: 21-Dec-2010 by Shehzad
        /// Gets all Contacts for a company to show in front end
        /// </summary>
        /// <param name="WorkgroupId">Get contact by this param</param>
        /// <returns>List of Contacts</returns>
        public List<ContactExtension> GetAllContactsByWorkgroupID(Int64 WorkgroupID, Int64 CompanyID)
        {
            return GetAllContact().Where(w => w.WorkgroupId == WorkgroupID && w.CompanyID == CompanyID && w.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.Contact.ToString()).ToList();
        }

        public IQueryable<ContactExtension> GetAllContact()
        {
            var qry = (from d in db.StoreDocuments
                       join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                       join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                       join city in db.INC_Cities on d.ContactCityID equals city.iCityID
                       join state in db.INC_States on d.ContactStateID equals state.iStateID
                       join country in db.INC_Countries on d.ContactCountryID equals country.iCountryID
                       join cs in db.CompanyStores on d.StoreId equals cs.StoreID
                       //join ce in db.CompanyEmployees on d.WorkgroupID equals ce.WorkgroupID
                       select new ContactExtension
                       {
                           StoreId = d.StoreId,
                           DocuemntFor = d.DocuemntFor,
                           Workgroup = il.sLookupName,
                           WorkgroupId = d.WorkgroupID,
                           Department = dept.sLookupName,
                           DepartmentId = d.DepartmentID,
                           StoreDocumentId = d.StoreDocumentID,
                           FirstName = d.ContactFirstName,
                           LastName = d.ContactLastName,
                           Title = d.ContactTitle,
                           CompanyName = d.ContactCompanyName,
                           Address1 = d.ContactAddress1,
                           Address2 = d.ContactAddress2,
                           CountryId = (long)d.ContactCountryID,
                           StateId = (long)d.ContactStateID,
                           CityId = (long)d.ContactCityID,
                           Country = country.sCountryName,
                           State = state.sStatename,
                           City = city.sCityName,
                           Email = d.ContactEmail,
                           Mobile = d.ContactMobile,
                           Fax = d.ContactFax,
                           Telephone = d.ContactTelephone,
                           ZipCode = d.ContactZip,
                           Image = d.ContactUploadImage,
                           UserRoleDescription = d.ContactUserRoleDescription,
                           //UserInfoID = ce.UserInfoID,
                           CompanyID = cs.CompanyID
                       }
                       );
            return qry;

        }

        public class ContactExtension
        {

            public Int64 WorkgroupId { get; set; }
            public Int64 DepartmentId { get; set; }
            public string Workgroup { get; set; }
            public string Department { get; set; }
            public Int64 StoreDocumentId { get; set; }
            public Int64 StoreId { get; set; }
            public string DocuemntFor { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Title { get; set; }
            public string CompanyName { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public Int64 CountryId { get; set; }
            public Int64 StateId { get; set; }
            public Int64 CityId { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
            public string Telephone { get; set; }
            public string Fax { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string Image { get; set; }
            public string UserRoleDescription { get; set; }
            public Int64 UserInfoID { get; set; }
            public Int64 CompanyID { get; set; }

        }

        #endregion

        #region Tearms & Conditions

        public List<TNCExtension> GetAllTNCsByStoreId(Int64 StoreId)
        {
            return GetAllTNC().Where(s => s.StoreId == StoreId && s.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.TNC.ToString()).ToList();
        }

        /// <summary>
        /// Mayur Dec-05-2011
        /// Gets all Tearms & Conditions for a workgroup in a store
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="StoreId"></param>
        /// <param name="WorkgroupId"></param>
        /// <returns></returns>
        public List<TNCExtension> GetTNCsByWorkgroupId(Int64 StoreId, Int64 WorkgroupId)
        {
            //return GetAllTNC().Where(w => w.WorkgroupId == WorkgroupId && w.StoreId == StoreId && w.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.TNC.ToString()).ToList();

            return (from d in db.StoreDocuments
                    join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                    join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                    join cs in db.CompanyStores on d.StoreId equals cs.StoreID
                    //join ce in db.CompanyEmployees on d.WorkgroupID equals ce.WorkgroupID
                    where d.WorkgroupID == WorkgroupId &&
                          d.StoreId == StoreId &&
                          d.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.TNC.ToString()
                    select new TNCExtension
                    {
                        StoreId = d.StoreId,
                        TNCHeader = d.TNCHeader,
                        TNCContent = d.TNCContent,
                        DocuemntFor = d.DocuemntFor,
                        Workgroup = il.sLookupName,
                        WorkgroupId = d.WorkgroupID,
                        Department = dept.sLookupName,
                        DepartmentId = d.DepartmentID,
                        StoreDocumentId = d.StoreDocumentID,
                        //UserInfoID = ce.UserInfoID,
                        CompanyID = cs.CompanyID

                    }).ToList();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="WorkgroupID"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public List<TNCExtension> GetAllTNCsByUser(Int64 WorkgroupID, Int64 CompanyID)
        {
            //return GetAllTNC().Where(w => w.WorkgroupId == WorkgroupID && w.CompanyID == CompanyID && w.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.TNC.ToString()).ToList();

            return (from d in db.StoreDocuments
                    join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                    join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                    join cs in db.CompanyStores on d.StoreId equals cs.StoreID
                    //join ce in db.CompanyEmployees on d.WorkgroupID equals ce.WorkgroupID
                    where d.WorkgroupID == WorkgroupID &&
                          cs.CompanyID == CompanyID &&
                          d.DocuemntFor == Common.DAEnums.CompanyStoreDocumentFor.TNC.ToString()
                    select new TNCExtension
                    {
                        StoreId = d.StoreId,
                        TNCHeader = d.TNCHeader,
                        TNCContent = d.TNCContent,
                        DocuemntFor = d.DocuemntFor,
                        Workgroup = il.sLookupName,
                        WorkgroupId = d.WorkgroupID,
                        Department = dept.sLookupName,
                        DepartmentId = d.DepartmentID,
                        StoreDocumentId = d.StoreDocumentID,
                        //UserInfoID = ce.UserInfoID,
                        CompanyID = cs.CompanyID
                    }).ToList();
        }

        public IQueryable<TNCExtension> GetAllTNC()
        {
            var qry = (from d in db.StoreDocuments
                       join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                       join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                       join cs in db.CompanyStores on d.StoreId equals cs.StoreID
                       //join ce in db.CompanyEmployees on d.WorkgroupID equals ce.WorkgroupID
                       select new TNCExtension
                       {
                           StoreId = d.StoreId,
                           TNCHeader = d.TNCHeader,
                           TNCContent = d.TNCContent,
                           DocuemntFor = d.DocuemntFor,
                           Workgroup = il.sLookupName,
                           WorkgroupId = d.WorkgroupID,
                           Department = dept.sLookupName,
                           DepartmentId = d.DepartmentID,
                           StoreDocumentId = d.StoreDocumentID,
                           //UserInfoID = ce.UserInfoID,
                           CompanyID = cs.CompanyID

                       }
                       );
            return qry;

        }

        public class TNCExtension
        {
            public Int64 WorkgroupId { get; set; }
            public Int64 DepartmentId { get; set; }
            public string Workgroup { get; set; }
            public string Department { get; set; }
            public string TNCHeader { get; set; }
            public string TNCContent { get; set; }
            public Int64 StoreDocumentId { get; set; }
            public Int64 StoreId { get; set; }
            public string DocuemntFor { get; set; }
            public Int64 UserInfoID { get; set; }
            public Int64 CompanyID { get; set; }
        }


        #endregion

    }

}
