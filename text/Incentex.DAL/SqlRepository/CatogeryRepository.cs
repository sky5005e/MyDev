using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class CatogeryRepository : RepositoryBase
    {
        /// <summary>
        /// GetAllQuery()
        /// Return all the record from the category table
        /// Nagmani 08/10/2010
        /// </summary>
        /// <returns></returns>
        IQueryable<Category> GetAllQuery()
        {
            IQueryable<Category> qry = from C in db.Categories
                                       select C;
            return qry;
        }

        /// <summary>
        /// GetAllCategory()
        /// Return List of the record from the category table
        /// Nagmani 08/10/2010
        /// </summary>
        /// <returns></returns>
        public List<Category> GetAllCategory()
        {
            return db.Categories.ToList();
        }

        public Category GetCategoryByID(Int64 CategoryID)
        {
            return db.Categories.SingleOrDefault(c => c.CategoryID == CategoryID);
        }

        public List<Category> CheckDuplication(Int64 CategoryID, String CategoryName)
        {
            if (CategoryID != 0)
                return db.Categories.Where(c => c.CategoryID != CategoryID && c.CategoryName == CategoryName).ToList();
            else
                return db.Categories.Where(c => c.CategoryName == CategoryName).ToList();
        }

        /// <summary>
        /// GetLookupCategoryQuery()
        /// Return List of the SubCategory record from the Inc_Lookup table
        /// Nagmani 08/10/2010
        /// </summary>
        /// <returns></returns>
        IQueryable<SubCategory> GetLookupCategoryQuery()
        {
            IQueryable<SubCategory> qry = from C in db.SubCategories
                                          select C;
            return qry;
        }

        /// <summary>
        /// GetAllSubCategory
        /// Return List of the record from the category table
        /// Nagmani 08/10/2010
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <returns></returns>
        /// <param name="CatogeryName"></param>
        public List<SubCategory> GetAllSubCategory(int CatogeryID)
        {
            //IQueryable<SubCategory> qry = GetLookupCategoryQuery();
            //List<SubCategory> objList = qry.ToList();
            //var subcatogery = (from c in objList where c.CategoryID == CatogeryID select c);

            var subcatogery = (from C in db.SubCategories
                               where C.CategoryID == CatogeryID
                               select C).ToList();
            
            return subcatogery;
        }


    }
}
