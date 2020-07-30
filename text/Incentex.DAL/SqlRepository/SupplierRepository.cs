using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class SupplierRepository : RepositoryBase
    {

        public IQueryable<Supplier> GetAllQuery()
        {
            IQueryable<Supplier> qry = from s in db.Suppliers
                                       select s;
            return qry;
        }


        public enum SupplierSortExpType
        {
            FirstName,
            CompanyWebsite,
            sCountryName,      
            Telephone
        }

        


        public IEnumerable GetSupplierList(SupplierSortExpType SortExp, DAEnums.SortOrderType SortOrder)
        {
            var qry = from u in db.UserInformations
                      join suppliers in db.Suppliers on u.UserInfoID equals suppliers.UserInfoID
                      join inc_countries in db.INC_Countries on new { CountryId = (u.CountryId.Value ==null ? -1 : u.CountryId.Value) } equals new { CountryId = inc_countries.iCountryID }
                      where u.IsDeleted == false
                      //join companies in db.Companies on new { CompanyId = suppliers.CompanyId } equals new { CompanyId = companies.CompanyID }
                      select new
                      {
                          suppliers.SupplierID,
                          suppliers.UserInfoID,
                          //suppliers.CompanyId,
                          suppliers.CompanyWebsite,
                          suppliers.SupplierClassificationID,
                          suppliers.SupplierSetupDate,
                          suppliers.SupplierTypeID,
                          suppliers.BankName,
                          suppliers.BannkContactPerson,
                          suppliers.BankAddress,
                          suppliers.BankCountryID,
                          suppliers.BankStateID,
                          suppliers.BankCityID,
                          suppliers.BankZip,
                          suppliers.BankFax,
                          suppliers.BankTelephone,
                          suppliers.BankMobile,
                          suppliers.BankEmail,
                          suppliers.BankAccountName,
                          suppliers.BankAccountNumber,
                          suppliers.BankRoutingNumber,
                          suppliers.BankEmailABA,
                          suppliers.FactoryHoursInformation,
                          suppliers.AnnualPriceOfferReviewDate,
                          suppliers.SupplierAccountNumber,
                          suppliers.InternalAccountNumber,
                          suppliers.SupplierBirthDay,
                          suppliers.SpouseName,
                          suppliers.ChildrenName,
                          suppliers.AccessToWorldLinkTrackingCenter,
                          suppliers.AccessToEditOrMakeChangesToPurchaseOrders,
                          suppliers.GeneralQualitySystemCompliances,
                          suppliers.Department,
                          u.FirstName,
                          u.LastName,
                          inc_countries.sCountryName,
                          CountryId = (System.Int64?)u.CountryId,
                          u.Title,
                          u.Telephone
                      };

            var List = qry.ToList();

            

            switch(SortExp)
            {
                case SupplierSortExpType.CompanyWebsite:
                    //List = (SortOrder == DAEnums.SortOrderType.Asc) ? qry.OrderBy(s => s.CompanyWebsite).ToList() : qry.OrderByDescending(s => s.CompanyWebsite).ToList(); 
                    List = qry.OrderBy(s => s.CompanyWebsite).ToList();
                    break;
                case SupplierSortExpType.FirstName:
                    List = qry.OrderBy(s => s.FirstName).ToList();
                    break;
                case SupplierSortExpType.sCountryName:
                    List = qry.OrderBy(s => s.sCountryName).ToList();
                    break;
                case SupplierSortExpType.Telephone:
                    List = qry.OrderBy(s => s.Telephone).ToList();
                    break;
            }


            if(SortOrder == DAEnums.SortOrderType.Desc )
            {
                List.Reverse();
            }

         

            return List;
        }

        /// <summary>
        /// Get supplier by id
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        public Supplier GetById(Int64 SupplierID )
        {
            IQueryable<Supplier> qry = GetAllQuery().Where( s => s.SupplierID == SupplierID);
            Supplier obj = GetSingle(qry.ToList());
            return obj;
        }

        public Supplier GetByUserInfoId(Int64 UserInfoID)
        {
            IQueryable<Supplier> qry = GetAllQuery().Where(s => s.UserInfoID == UserInfoID);
            Supplier obj = GetSingle(qry.ToList());
            return obj;
        }
        

        public IEnumerable GetAllsupplier()
        {
            var qry = from u in db.UserInformations
                      join suppliers in db.Suppliers on u.UserInfoID equals suppliers.UserInfoID
                      join inc_countries in db.INC_Countries on new { CountryId = (u.CountryId.Value == null ? -1:u.CountryId.Value)  } equals new { CountryId = inc_countries.iCountryID }
                      //join companies in db.Companies on new { CompanyId = suppliers.CompanyId } equals new { CompanyId = companies.CompanyID }
                      where u.IsDeleted == false //&& companies.CompanyID == CompanyId
                      select new
                      {
                          suppliers.SupplierID,
                        FirstName =  u.FirstName + " " + u.LastName
                          
                          
                      };

            var List = qry.ToList();

            return List;
        }

        public List<SupplierDetails> GetAllSupplierList(SupplierSortExpType SortExp, DAEnums.SortOrderType SortOrder, Int64 SupplierId)
        {

            var qry = (from u in db.UserInformations
                      join suppliers in db.Suppliers on u.UserInfoID equals suppliers.UserInfoID
                      join inc_countries in db.INC_Countries on new { CountryId = (u.CountryId.Value == null ? -1 : u.CountryId.Value) } equals new { CountryId = inc_countries.iCountryID }
                      join city in db.INC_Cities on new { cityid = (u.CityId.Value == null ? -1 : u.CityId.Value) } equals new { cityid = city.iCityID }
                      join state in db.INC_States on new { stateid = (u.StateId.Value == null ? -1 : u.StateId.Value) } equals new { stateid = state.iStateID }
                      where u.IsDeleted == false
                      select new SupplierDetails
                      {
                          SupplierID = suppliers.SupplierID,
                          FullName = u.FirstName + " " + u.LastName,
                          CountryName = inc_countries.sCountryName,
                          UserInfoID = suppliers.UserInfoID,
                          CompanyID = suppliers.CompanyId,
                          CompanyName = suppliers.CompanyName,
                          Address1 = u.Address1,
                          Address2 = u.Address2,
                          City = city.sCityName,
                          State = state.sStatename,
                          ZipCode = u.ZipCode,
                          Telephone = u.Telephone,
                          Fax = u.Fax,
                          DecoratingServicesID = suppliers.DecoratingServicesID
                      }).ToList();

            if (SupplierId != 0)
                qry = qry.Where(s => s.SupplierID == SupplierId).ToList();

            switch (SortExp)
            {
                case SupplierSortExpType.FirstName:
                    qry = qry.OrderBy(s => s.FullName).ToList();
                    break;
                case SupplierSortExpType.sCountryName:
                    qry = qry.OrderBy(s => s.CountryName).ToList();
                    break;
            }


            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                qry.Reverse();
            }

            return qry.ToList();
        }
        public List<SearchResults> GetSupplierId()
        {
            return (from u in db.UserInformations
                    join suppliers in db.Suppliers on u.UserInfoID equals suppliers.UserInfoID
                    join inc_countries in db.INC_Countries on new { CountryId = (u.CountryId.Value == null ? -1 : u.CountryId.Value) } equals new { CountryId = inc_countries.iCountryID }
                    select new SearchResults
                    {
                        SupplierID = suppliers.SupplierID,
                        FirstName = u.FirstName + " " + u.LastName

                    }).ToList<SearchResults>();

                   
        }
        public class SearchResults
        {

            public Int64 SupplierID { get; set; }
            public String FirstName { get; set; }
        }

        /// <summary>
        /// Delete supplier and related data
        /// </summary>
        /// <param name="SupplierID"></param>
        public void Delete(Int64 SupplierID)
        {
            Supplier obj = GetById(SupplierID);

            if(obj != null)
            {
              

                //delete from documents which are of 3 types

                
                LookupRepository objLookupRepository = new LookupRepository();
 

                DocumentRepository objDocumentRepository = new DocumentRepository();

                //delete for SupplierListVacationAndSupplierClosing 
                Int64 DocumentTypeId = objLookupRepository.GetSupplierDocumentLookUpId(LookupRepository.SupplierDocumentsType.SupplierListVacationAndSupplierClosing);
                objDocumentRepository.Delete(DocumentTypeId, obj.SupplierID);

                //delete for SupplierMasterPriceOfferList
                DocumentTypeId = objLookupRepository.GetSupplierDocumentLookUpId(LookupRepository.SupplierDocumentsType.SupplierMasterPriceOfferList);
                objDocumentRepository.Delete(DocumentTypeId, obj.SupplierID);


                //delete for SupplierQualityCertificates
                DocumentTypeId = objLookupRepository.GetSupplierDocumentLookUpId(LookupRepository.SupplierDocumentsType.SupplierQualityCertificates);
                objDocumentRepository.Delete(DocumentTypeId, obj.SupplierID);


                //delete from Notes
                NotesHistoryRepository objNotesHistoryRepository = new NotesHistoryRepository();
                objNotesHistoryRepository.Delete(obj.SupplierID, DAEnums.NoteForType.Supplier); 


                //delete supplier 
                base.Delete(obj);
                base.SubmitChanges();

                //delete from user info table
                UserInformationRepository objUserInfoRepository = new UserInformationRepository();
                UserInformation objUserInfo = objUserInfoRepository.GetById(obj.UserInfoID);

                if (objUserInfo != null)
                {
                    objUserInfoRepository.Delete(objUserInfo);
                    objUserInfoRepository.SubmitChanges();
                }
            }

        }

        public List<GetSupplierOrdersResult> GetSupplierOrder(Int64 OrderID, Int64 SupplierID)
        {
               return db.GetSupplierOrders(OrderID, SupplierID).ToList();
        }
        //Saurabh
        public Supplier GetSinglSupplierid(String Name)
        {
            return db.Suppliers.FirstOrDefault(le => le.CompanyName == Name);
        }

        public UserInformation GetUserInformationBySupplierID(Int64 SupplierID)
        {
            return (from supplier in db.Suppliers
                    join user in db.UserInformations on supplier.UserInfoID equals user.UserInfoID
                    where supplier.SupplierID == SupplierID
                    select user).FirstOrDefault();
        }
    }


    public class SupplierDetails
    {
        public Int64 SupplierID { get; set; }
        public Int64 UserInfoID { get; set; }
        public String FullName { get; set; }
        public String CountryName { get; set; }
        public Int64 CompanyID { get; set; }
        public String CompanyName { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String ZipCode { get; set; }
        public String Telephone { get; set; }
        public String Fax { get; set; }
        public String DecoratingServicesID { get; set; }

    }


}
