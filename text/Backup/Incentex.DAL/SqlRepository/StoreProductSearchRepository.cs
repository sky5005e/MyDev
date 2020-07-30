using System;
using System.Collections.Generic;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class StoreProductSearchRepository : RepositoryBase
    {
        /// <summary>
        /// This method is used to set and get property 
        /// of field for which we show in geidview.
        /// Nagmani 08/10/2010
        /// </summary>
        public class StoreProductSearchResult
        {
            public String CompanyName { get; set; }

            public String StoreStatus { get; set; }

            public String ProductDescrption { get; set; }

            public String ProductStatus { get; set; }

            public String MasterNo { get; set; }
        }

        /// <summary>
        /// StoreProductSearchDetails()
        /// Reterieve all the records from
        /// from the StoreProductSearchDetails table
        /// on iProductId parameter.
        /// Nagmani 08/10/2010
        /// </summary>
        /// <param name="iProductItemID"></param>
        /// <returns></returns>
        public List<StoreProductSearchResult> StoreProductSearchDetails(Int32? iCompanyId, Int32? MasterStyleid, String pItemNo, Int32? pStyleNo, String Keyword)
        {
            return (from c in db.SelectStoreProdcutSearch(iCompanyId, MasterStyleid, pItemNo, pStyleNo, Keyword)
                    select new StoreProductSearchResult
                    {
                        CompanyName = c.CompanyName,
                        StoreStatus = c.StoreStatus,
                        MasterNo = c.MasterNo,
                        ProductDescrption = c.ProductDescrption,
                        ProductStatus = c.ProductStatus
                    }).Distinct().ToList<StoreProductSearchResult>();
        }
    }
}