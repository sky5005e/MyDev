using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
   public class SubCatogeryRepository:RepositoryBase
    {
        IQueryable<SubCategory> GetAllQuery()
        {
            IQueryable<SubCategory> qry = from C in db.SubCategories
                                       select C;
            return qry;
        }

        public List<SubCategory> GetAllSubCategory(Int32 iCatogeryid)
        {
            return db.SubCategories.Where(s => s.CategoryID == iCatogeryid).OrderBy(le => le.SubCategoryName).ToList();
        }

        public List<SubCategory> GetAllSubCategoryByCategoryName(String CategoryName)
        {
            return (from sc in db.SubCategories
                    join c in db.Categories on sc.CategoryID equals c.CategoryID
                    where c.CategoryName == CategoryName
                    select sc).ToList();
        }

        public SubCategory GetByName(String Name)
        {
            IQueryable<SubCategory> qry = GetAllQuery().Where(s => s.SubCategoryName.Equals(Name));
            SubCategory objSubCategory = qry.SingleOrDefault();
            return objSubCategory;
        }
       
        public SubCategory GetSubCategoryByID(Int64 SubCategoryID)
        {
            return db.SubCategories.SingleOrDefault(c => c.SubCategoryID == SubCategoryID);
        }

        public List<SubCategory> CheckDuplication(Int64 CategoryID,Int64 SubCategoryID, String SubCategoryName)
        {
            if (CategoryID != 0 && SubCategoryID!=0)
                return db.SubCategories.Where(c => c.CategoryID == CategoryID && c.SubCategoryID != SubCategoryID && c.SubCategoryName == SubCategoryName).ToList();
            else
                return db.SubCategories.Where(c => c.CategoryID == CategoryID && c.SubCategoryName == SubCategoryName).ToList();
        }
    }
}
