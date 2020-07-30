using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class AssetInventoryRepository : RepositoryBase
    {
        #region BasicInfo
        IQueryable<EquipmentInventoryMaster> GetAllInventoryDetail()
        {
            IQueryable<EquipmentInventoryMaster> qry = from c in db.EquipmentInventoryMasters
                                                       orderby c.EquipmentInventoryID
                                                       select c;
            return qry;
        }

        /// <summary>
        /// Update By :Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentInventoryID"></param>
        /// <returns></returns>
        public EquipmentInventoryMaster GetInventoryById(Int64 EquipmentInventoryID)
        {
            //IQueryable<EquipmentInventoryMaster> qry = GetAllInventoryDetail().Where(s => s.EquipmentInventoryID == EquipmentInventoryID);
            //EquipmentInventoryMaster obj = GetSingle(qry.ToList());

            EquipmentInventoryMaster obj = (from c in db.EquipmentInventoryMasters
                                            orderby c.EquipmentInventoryID
                                            where c.EquipmentInventoryID == EquipmentInventoryID
                                            select c).SingleOrDefault();

            return obj;
        }
        public Int64 GetMaxInventoryID()
        {
            Int64 max =
              (from EquipmentInventoryMaster in db.EquipmentInventoryMasters
               select EquipmentInventoryMaster.EquipmentInventoryID).Max();
            return max;
        }
        #endregion
        //#region ItemInfo
        //  public List<EquipmentProductCategoryLookup> GetAllProductCategory()
        //  {
        //      return db.EquipmentProductCategoryLookups.Where(c => c.ParentProductCategoryID == null).ToList();
        //  }
        //  public List<EquipmentProductCategoryLookup> GetAllProductSubCategory(Int64 ParentProductCategoryID)
        //  {
        //      List<EquipmentProductCategoryLookup> objList = db.EquipmentProductCategoryLookups.Where(c => c.ParentProductCategoryID != null && c.ParentProductCategoryID == ParentProductCategoryID).ToList();

        //      return objList;
        //  }
        //#endregion
        #region Document
        IQueryable<EquipmentInventoryDocument> GetAllInventoryDoc()
        {
            IQueryable<EquipmentInventoryDocument> qry = from c in db.EquipmentInventoryDocuments
                                                         orderby c.InventoryDocumentID
                                                         select c;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentInventoryID"></param>
        /// <returns></returns>
        public List<EquipmentInventoryDocument> GetInventoryAttachment(Int64 EquipmentInventoryID)
        {
            //IQueryable<EquipmentInventoryDocument> qry = GetAllInventoryDoc().Where(s => s.EquipmentInventoryID == EquipmentInventoryID && s.IsImage == false);
            //var objlist = qry.ToList();

            var objlist = (from c in db.EquipmentInventoryDocuments
                           orderby c.InventoryDocumentID
                           where c.EquipmentInventoryID == EquipmentInventoryID && c.IsImage == false
                           select c).ToList();
            return objlist;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentInventoryID"></param>
        /// <returns></returns>
        public List<EquipmentInventoryDocument> GetInventoryImage(Int64 EquipmentInventoryID)
        {
            //IQueryable<EquipmentInventoryDocument> qry = GetAllInventoryDoc().Where(s => s.EquipmentInventoryID == EquipmentInventoryID && s.IsImage == true);
            //var objlist = qry.ToList();

            var objlist = (from c in db.EquipmentInventoryDocuments
                           orderby c.InventoryDocumentID
                           where c.EquipmentInventoryID == EquipmentInventoryID && c.IsImage == true
                           select c).ToList();

            return objlist;
        }
        public void DeleteInventoryDoc(Int64 InventoryDocumentID)
        {
            var matched = (from c in db.GetTable<EquipmentInventoryDocument>()
                           where c.InventoryDocumentID == Convert.ToInt64(InventoryDocumentID)
                           select c).SingleOrDefault();
            try
            {
                if (matched != null)
                {
                    db.EquipmentInventoryDocuments.DeleteOnSubmit(matched);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }

        }
        #endregion
        #region InventoryResult
        public void DeleteInventory(Int64 EquipmentInventoryID)
        {
            var matched = (from c in db.GetTable<EquipmentInventoryMaster>()
                           where c.EquipmentInventoryID == EquipmentInventoryID
                           select c).SingleOrDefault();
            try
            {
                if (matched != null)
                {
                    db.EquipmentInventoryMasters.DeleteOnSubmit(matched);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }

        }

        public List<GetEquipInventoryResult> GetInventoryResult(Int64 CompanyID, Int64 BaseStationID, Int64 ProductCategoryID, Int64 ProductSCategoryID, string PartNumber, string MFGPartNumber)
        {
            var objlist = db.GetEquipInventory().ToList();
            if (CompanyID != 0)
            {
                objlist = objlist.Where(c => c.CompanyID == CompanyID).ToList();
            }
            if (BaseStationID != 0)
            {
                objlist = objlist.Where(c => c.BaseStationID == BaseStationID).ToList();
            }
            if (ProductCategoryID != 0)
            {
                objlist = objlist.Where(c => c.CategoryID == ProductCategoryID).ToList();
            }
            if (ProductSCategoryID != 0)
            {
                objlist = objlist.Where(c => c.SubCategory == ProductSCategoryID).ToList();
            }
            if (!string.IsNullOrEmpty(PartNumber))
            {
                objlist = objlist.Where(c => c.PartNumber == PartNumber).ToList();
            }
            if (!string.IsNullOrEmpty(MFGPartNumber))
            {
                objlist = objlist.Where(c => c.MPartNumber == MFGPartNumber).ToList();
            }

            return objlist;
        }
        #endregion
        #region Product Category

        /// <summary>
        /// GetAllProductCategory()
        /// Return List of the record from the ProductCategory table
        /// </summary>
        /// <returns></returns>
        public List<EquipmentProductCategoryLookup> GetAllProductCategory()
        {
            return db.EquipmentProductCategoryLookups.Where(c => c.ParentProductCategoryID == null).ToList();
        }

        public EquipmentProductCategoryLookup GetProductCategoryByID(Int64 ProductCategoryID)
        {
            return db.EquipmentProductCategoryLookups.SingleOrDefault(c => c.ProductCategoryID == ProductCategoryID);
        }

        public List<EquipmentProductCategoryLookup> CheckDuplication(Int64 ProductCategoryID, String ProductCategoryName)
        {
            if (ProductCategoryID != 0)
                return db.EquipmentProductCategoryLookups.Where(c => c.ProductCategoryID != ProductCategoryID && c.ProductCategoryName == ProductCategoryName).ToList();
            else
                return db.EquipmentProductCategoryLookups.Where(c => c.ProductCategoryName == ProductCategoryName).ToList();
        }

        /// <summary>
        /// GetLookupProductCategoryQuery()
        /// Return List of the SubProductCategory record from the Inc_Lookup table
        /// Nagmani 08/10/2010
        /// </summary>
        /// <returns></returns>
        IQueryable<EquipmentProductCategoryLookup> GetLookupProductCategoryQuery()
        {
            IQueryable<EquipmentProductCategoryLookup> qry = from C in db.EquipmentProductCategoryLookups.Where(c => c.ParentProductCategoryID != null)
                                                             select C;
            return qry;
        }
        /// <summary>
        /// GetAllSubProductCategory
        /// Return List of the record from the ProductCategory table
        /// Nagmani 08/10/2010
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <returns></returns>
        /// <param name="ProductCategoryName"></param>
        public List<EquipmentProductCategoryLookup> GetAllSubProductCategory(int ProductCategoryID)
        {
            //IQueryable<EquipmentProductCategoryLookup> qry = GetLookupProductCategoryQuery();
            //List<EquipmentProductCategoryLookup> objList = qry.ToList();
            //var subProductCategory = (from c in objList where c.ProductCategoryID == ProductCategoryID select c);

            var subProductCategory = (from C in db.EquipmentProductCategoryLookups
                                      where C.ParentProductCategoryID != null &&
                                            C.ProductCategoryID == ProductCategoryID
                                      select C);

            return subProductCategory.ToList();
        }
        #endregion
        #region ProductSubCategory
        IQueryable<EquipmentProductCategoryLookup> GetAllProductSubCategoryQuery()
        {
            IQueryable<EquipmentProductCategoryLookup> qry = from C in db.EquipmentProductCategoryLookups.Where(c => c.ParentProductCategoryID != null)
                                                             select C;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="iProductCategoryid"></param>
        /// <returns></returns>
        public List<EquipmentProductCategoryLookup> GetAllProductSubCategoryDetail(Int64 iProductCategoryid)
        {
            //IQueryable<EquipmentProductCategoryLookup> qry = GetAllProductSubCategoryQuery();
            //List<EquipmentProductCategoryLookup> objList = qry.ToList();
            //var ProductSubCategory = (from c in objList where c.ParentProductCategoryID == iProductCategoryid select c);

            var ProductSubCategory = (from C in db.EquipmentProductCategoryLookups
                                      where C.ParentProductCategoryID != null &&
                                      C.ParentProductCategoryID == iProductCategoryid
                                      select C);

            return ProductSubCategory.ToList();
        }

        public List<EquipmentProductCategoryLookup> GetAllProductSubCategoryByProductCategoryName(String ProductCategoryName)
        {
            return (from sc in db.EquipmentProductCategoryLookups
                    join c in db.EquipmentProductCategoryLookups on sc.ProductCategoryID equals c.ProductCategoryID
                    where c.ProductCategoryName == ProductCategoryName
                    select sc).ToList();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public EquipmentProductCategoryLookup GetByName(string Name)
        {
            //IQueryable<EquipmentProductCategoryLookup> qry = GetAllProductSubCategoryQuery().Where(s => s.ProductCategoryName.Equals(Name));
            //EquipmentProductCategoryLookup objProductSubCategory = qry.SingleOrDefault();

            EquipmentProductCategoryLookup objProductSubCategory = (from C in db.EquipmentProductCategoryLookups
                                                                    where C.ParentProductCategoryID != null &&
                                                                    C.ProductCategoryName.Equals(Name)
                                                                    select C).SingleOrDefault();
            return objProductSubCategory;
        }

        public EquipmentProductCategoryLookup GetProductSubCategoryByID(Int64 ProductSubCategoryID)
        {
            return db.EquipmentProductCategoryLookups.SingleOrDefault(c => c.ProductCategoryID == ProductSubCategoryID);
        }

        public List<EquipmentProductCategoryLookup> CheckDuplication(Int64 ProductCategoryID, Int64 ProductSubCategoryID, String ProductSubCategoryName)
        {
            if (ProductCategoryID != 0 && ProductSubCategoryID != 0)
                return db.EquipmentProductCategoryLookups.Where(c => c.ParentProductCategoryID == ProductCategoryID && c.ProductCategoryID != ProductSubCategoryID && c.ProductCategoryName == ProductSubCategoryName).ToList();
            else
                return db.EquipmentProductCategoryLookups.Where(c => c.ParentProductCategoryID == ProductCategoryID && c.ProductCategoryName == ProductSubCategoryName).ToList();
        }
        #endregion
    }
}
