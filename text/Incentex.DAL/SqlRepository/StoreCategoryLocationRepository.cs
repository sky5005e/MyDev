using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class StoreCategoryLocationRepository : RepositoryBase
    {
        public List<StoreCategoryLocationResults> GetLocationByStoreID(Int64 StoreID)
        {
            return (from location in db.StoreCategoryLocations
                    join c in db.Categories on location.CategoryID equals c.CategoryID
                    where location.StoreID == StoreID
                    select new StoreCategoryLocationResults()
                    {
                        StoreCategoryLocationID = location.StoreCategoryLocationID,
                        StoreID = location.StoreID,
                        Location = location.Location,
                        CategoryID = location.CategoryID,
                        CategoryName = c.CategoryName
                    }).ToList();
        }
        public List<StoreCategoryLocation> GetCategoryLocationByStoreID(Int64 StoreID)
        {
            return db.StoreCategoryLocations.Where(s => s.StoreID == StoreID).ToList();
        }
        public class StoreCategoryLocationResults
        {
            public long StoreCategoryLocationID { get; set; }
            public long StoreID { get; set; }
            public string Location { get; set; }
            public long CategoryID { get; set; }
            public string CategoryName { get; set; }
        }
    }
}
