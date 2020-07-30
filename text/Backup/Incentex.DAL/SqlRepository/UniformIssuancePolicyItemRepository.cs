/* Project Name : Incentex 
 * Module Name : Uniform Isuance Program 
 * Description : This page is for data access logic for UniformIssuancePolicyItem table
 * ----------------------------------------------------------------------------------------- 
 * DATE | ID/ISSUE| AUTHOR | REMARKS 
 * ----------------------------------------------------------------------------------------- 
 * 23-Oct-2010 | 1 | Amit Trivedi | Design and Coding
 * ----------------------------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Incentex.DAL.SqlRepository
{
    public class UniformIssuancePolicyItemRepository : RepositoryBase
    {
        
        /// <summary>
        /// Get all record linq query 
        /// </summary>
        /// <returns></returns>
        IQueryable<UniformIssuancePolicyItem> GetAllQuery()
        {
            IQueryable<UniformIssuancePolicyItem> query = from u in db.UniformIssuancePolicyItems
                                                           select u;
            return query;
        }
        /// <summary>
        /// Get by Id
        /// </summary>
        /// <param name="UniformIssuancePolicyStyleID"></param>
        /// <returns></returns>
        public UniformIssuancePolicyItem GetById(Int64 UniformIssuancePolicyItemID)
        {
            return db.UniformIssuancePolicyItems.Where(q => q.UniformIssuancePolicyItemID == UniformIssuancePolicyItemID).FirstOrDefault();
        }
        public class UniformIssuancePolicyItemResult
        {
            public System.Int64 UniformIssuancePolicyItemID

            { get; set; }
            public System.Int64 UniformIssuancePolicyID

            { get; set; }
          
            public System.String AssociationbudgetAmt

            { get; set; }
            public System.String sLookupName

            { get; set; }
            public System.Int32 Issuance

            { get; set; }


            public System.Int64 RankId

            { get; set; }
            public System.Int64 AssociationIssuanceType

            { get; set; }

            public System.Int64 MasterItemId

            { get; set; }
            public System.Int64 PaymentOption

            { get; set; }
            public System.String IssuanceMasterItmewise

            { get; set; }
            public System.String FirstName

            { get; set; }
            public System.String LastName

            { get; set; }
            public System.String Address1

            { get; set; }
            public System.String Address2

            { get; set; }
            public System.Int64 CountryId

            { get; set; }
            public System.Int64 StateId

            { get; set; }
            public System.Int64 CityId

            { get; set; }
             public System.Int64 CompanyId

            { get; set; }
            
            public System.String ZipCode

            { get; set; }
            public System.String Telephone

            { get; set; }
                 public System.String Email

            { get; set; }
                 public System.String Mobile

            { get; set; }
                 public System.String MasterItemName

                 { get; set; }
                 public System.String CompanyName

                 { get; set; }
                 public System.String CountryName

                 { get; set; }
                 public System.String StateName

                 { get; set; }
                 public System.String CityName

                 { get; set; }
                 public System.String FullName

                 { get; set; }

        }
        /// <summary>
        /// get by UniformIssuancePolicyID
        /// </summary>
        /// <param name="UniformIssuancePolicyID"></param>
        /// <returns></returns>
        public List<UniformIssuancePolicyItem> GetByUniformIssuancePolicyID(long UniformIssuancePolicyID)
        {
            //List<UniformIssuancePolicyItem> objList =  //GetAllQuery().Where(u => u.UniformIssuancePolicyID == UniformIssuancePolicyID).ToList();
            //return objList;
            var qry = from uipi in db.UniformIssuancePolicyItems
                      join sp in db.StoreProducts on new { StoreProductID = Convert.ToInt64(uipi.StoreProductid) } equals new { StoreProductID = sp.StoreProductID }
                      where sp.StatusID == 135 && sp.IsDeleted == false && uipi.UniformIssuancePolicyID == UniformIssuancePolicyID
                      select uipi;

            return qry.ToList<UniformIssuancePolicyItem>();
        }

        public List<GetUniformIssuancePolicyItemsResult> GetUniformIssuancePolicyItems(Int64 UniformIssuancePolicyID)
        {
            return db.GetUniformIssuancePolicyItems(UniformIssuancePolicyID).ToList();
        }
        public List<UniformIssuancePolicyItem> GetByUniformIssuancePolicyIDAndNewGroup(long UniformIssuancePolicyID,string NewGroup)
        {
            List<UniformIssuancePolicyItem> objList = GetAllQuery().Where(u => u.UniformIssuancePolicyID == UniformIssuancePolicyID && u.NEWGROUP == NewGroup).ToList();
            return objList;
        }

        public List<IPItem> GetIssuancePolicyItemDetails(Int64 companyID, Int64 workgroupID, String categoryType, String garmentType)
        {
            var lstItemsDetials = db.GetIssuancePolicyItemDetails(companyID, workgroupID, categoryType, garmentType).ToList();
            List<IPItem>  AddedItems = (from ip in lstItemsDetials
                          select new IPItem
                          {
                              StoreProductID = ip.StoreProductID,
                              MasterItemID = ip.MasterItemID,
                              MasterItem = ip.MasterItem,
                              StyleID = ip.StyleID,
                              Style = ip.Style,
                              Summary = ip.Summary,
                              ProductDescription = ip.ProductDescrption,
                              ProductImage = ip.ProductImage
                          }).ToList();
            return AddedItems;
        }
        /// <summary>
        /// Delete By UniformIssuancePolicyID
        /// <param name="UniformIssuancePolicyID"></param>
        public void DeleteByUniformIssuancePolicyID(Int64 UniformIssuancePolicyID)
        {
            List<UniformIssuancePolicyItem> objList = GetAllQuery().Where( u => u.UniformIssuancePolicyID == UniformIssuancePolicyID).ToList();
            db.UniformIssuancePolicyItems.DeleteAllOnSubmit(objList);
            SubmitChanges();        
        }
        /// <summary>
        /// Check Duplication for Policy Note association
        /// Nagmani 28/01/2011
        /// </summary>
        /// <param name="AssociationIssuanceType "></param>
        /// <param name="UniformIssuancePolicyID"></param>
        /// <returns></returns>
        public int CheckDuplicate(long UniformIssuancePolicyID,long AssociationIssuanceType)
        {
           return (int)db.CheckAssociationIssuancePolicyNote(UniformIssuancePolicyID, AssociationIssuanceType).SingleOrDefault().IsDuplicate;

        }
        public int CheckDuplicate(string CompanyName, int? CompanyId, string mode)
        {
            return (int)db.INC_SelectCompanyDuplicate(CompanyName, CompanyId, mode).SingleOrDefault().IsDuplicate;
        }
        public List<CheckAddressOfBillingResult> CheckDuplicateAddress(Int64 UniformIssuancePolicyID)
        {
            List<CheckAddressOfBillingResult> obj = new List<CheckAddressOfBillingResult>();
            return obj = db.CheckAddressOfBilling(Convert.ToInt16(UniformIssuancePolicyID)).ToList();
        }
        public List<SelectUniformIssuancePolicyProgramResult> GetCreditProgram(Int64 UniformIssuancePolicyID)
        {
            List<SelectUniformIssuancePolicyProgramResult> obj = new List<SelectUniformIssuancePolicyProgramResult>();
            return obj = db.SelectUniformIssuancePolicyProgram(Convert.ToInt16(UniformIssuancePolicyID)).ToList();
        }
        public List<SelectBillingAddressResult> GetBillingAddress(Int64 UniformIssuancePolicyID)
        {
            List<SelectBillingAddressResult> obj = new List<SelectBillingAddressResult>();
            return obj = db.SelectBillingAddress(Convert.ToInt16(UniformIssuancePolicyID)).ToList();
        }
        public List<SelectShippingAddressResult>GetShippingAddress(Int64 UniformIssuancePolicyID)
        {
            List<SelectShippingAddressResult> obj = new List<SelectShippingAddressResult>();
            return obj = db.SelectShippingAddress(Convert.ToInt16(UniformIssuancePolicyID)).ToList();
        }
        public void UpdateBillingAddress(int UniformIssuancePolicyID,string Address1,string Address2,string email,string mobile,string telephone,string zipCode,Int64 countryid,Int64 stateid,Int64 cityid,string firstName,string lastname,int CompanyID,string SAddress1,string SAddress2,string Semail,string Smobile,string Stelephone,string SzipCode,Int64 Scountryid,Int64 Sstateid,Int64 Scityid,string SfirstName,string Slastname,int SCompanyID,string SCompanyName,string BCompanyName)
        {
            db.UpdateBillingAddress(UniformIssuancePolicyID,Address1,Address2,email,mobile,telephone,zipCode,countryid,stateid,cityid,firstName,lastname, CompanyID, SAddress1, SAddress2, Semail, Smobile, Stelephone, SzipCode, Scountryid, Sstateid, Scityid, SfirstName, Slastname, SCompanyID,SCompanyName,BCompanyName);
            db.SubmitChanges();
        }
        public List<SelectTailoringOptionActiveInActiveResult>GetTailoringActiveInActive(int MasterItemID, int ItemSizeID,int storeproductid)
        {
            List<SelectTailoringOptionActiveInActiveResult> objList;
            objList = db.SelectTailoringOptionActiveInActive(MasterItemID, ItemSizeID, storeproductid).ToList();
            return objList;

        }
        public int CheckMasterItemDuplicate(string SingleAssociation ,Int64 UniformIssuancePolicyID, string NEWGROUP, int MasterItemId, Int64 StoreProductID)
        {
            return (int)db.CheckduplicationMasterItemId(SingleAssociation, UniformIssuancePolicyID, NEWGROUP, MasterItemId, StoreProductID).SingleOrDefault().Ispresent;
        }
        public int CheckGroupName(Int64 UniformIssuancePolicyID, string NEWGROUP, int issuance)
        {
            return (int)db.CheckGroupName(UniformIssuancePolicyID, NEWGROUP, issuance).SingleOrDefault().Ispresent;
        }
        public List<SELECTITEMNUMBERONSIZEMASTERNOResult> GETITEMNUMBER(int MasterItemID, int ItemSizeID,int storeproductid)
        {
            List<SELECTITEMNUMBERONSIZEMASTERNOResult> objList;
            objList = db.SELECTITEMNUMBERONSIZEMASTERNO(MasterItemID, ItemSizeID, storeproductid).ToList();
            return objList;

        }
        public String GetItemNumber(Int32 MasterItemID, Int32 ItemSizeID, Int32 storeproductid)
        {
            var qry = (from q in db.ProductItems
                       where q.MasterStyleID == MasterItemID && q.ItemSizeID == ItemSizeID 
                           && q.ProductId == storeproductid && q.ItemNumberStatusID == 135 && q.IsDeleted == false
                       select q.ItemNumber).FirstOrDefault();
            return qry;
        }

        public ProductItem GetProductItemIDandItemNumber(Int32 MasterItemID, Int32 ItemSizeID, Int32 storeproductid)
        {
            return db.ProductItems.Where(q => q.MasterStyleID == MasterItemID && q.ItemSizeID == ItemSizeID
                           && q.ProductId == storeproductid && q.ItemNumberStatusID == 135 && q.IsDeleted == false).FirstOrDefault();
        }

        public List<SelectWeatherTypeIdResult> GetWeatherTypeid(int BaseStationid)
        {
            List<SelectWeatherTypeIdResult> obj = new List<SelectWeatherTypeIdResult>();
            return obj = db.SelectWeatherTypeId(BaseStationid).ToList();

        } 
    }

    [Serializable]
    public class IPItem
    {
       
        public Int64 StoreProductID { get; set; }
        public Int64 MasterItemID { get; set; }
        public String MasterItem { get; set; }
        public Int64 StyleID { get; set; }
        public String Style { get; set; }
        public String Summary { get; set; }
        public String ProductDescription { get; set; }
        public String ProductImage { get; set; }
        //public Int64 UniformIssuancePolicyItemID { get; set; }
        //public Int64 Issuance { get; set; }
        //public Int64 AssociationIssuanceType { get; set; }
        //public Int64 EmployeeTypeID { get; set; }
        //public Int64 WeatherTypeID { get; set; }
        //public String ItemNumber { get; set; }

    }
}
