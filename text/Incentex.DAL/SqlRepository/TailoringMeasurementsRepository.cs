using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class TailoringMeasurementsRepository : RepositoryBase
    {
        IQueryable<ProductItemTailoringMeasurement> GetAllQuery()
        {
            IQueryable<ProductItemTailoringMeasurement> qry = from C in db.ProductItemTailoringMeasurements
                                                              where C.IsDeleted == false
                                                              select C;
            return qry;
        }

        public class ProductItemTailoringMeasurementResult
        {
            public Int64 ProductItemTailoringMeasurementID { get; set; }
            public String TailoringGuidelines { get; set; }
            public String TailoringMeasurementChart { get; set; }
            public Int64? MasterStyleID { get; set; }
            public String MasterStyleName { get; set; }
        }

        public List<ProductItemTailoringMeasurementResult> TailoringMeasurementsChart(Int64 StoreProductID, Int64 MasterItemNo)
        {
            return (from tailoring in db.ProductItemTailoringMeasurements
                    join look in db.INC_Lookups on tailoring.MasterStyleId equals look.iLookupID
                    where tailoring.StoreProductID == StoreProductID && tailoring.MasterStyleId == MasterItemNo
                    && tailoring.IsDeleted == false
                    select new ProductItemTailoringMeasurementResult
                    {
                        ProductItemTailoringMeasurementID = tailoring.ProductItemTailoringMeasurementID,
                        TailoringMeasurementChart = tailoring.TailoringMeasurementChart,
                        TailoringGuidelines = tailoring.TailoringGuidelines,
                        MasterStyleID = tailoring.MasterStyleId,
                        MasterStyleName = look.sLookupName
                    }).ToList();
        }

        public List<ProductItemTailoringMeasurement> GetTailoringMeasurementByStoreProductIDAndMasterItemID(Int64 MasterStyleid, Int64 StoreProductID)
        {
            return db.ProductItemTailoringMeasurements.Where(C => C.MasterStyleId == MasterStyleid && C.StoreProductID == StoreProductID && C.IsDeleted == false).ToList();
        }

        /// <summary>
        /// Delete a ProductItemTailoring&measurements table record by ProductItemTailoringMeasurementID
        /// </summary>
        /// <param name="ProductItemTailoringMeasurementID"></param>
        public void DeleteProductItemTailoringMeasuremts(Int64 ProductItemTailoringMeasurementID, Int64? DeletedBy)
        {
            ProductItemTailoringMeasurement objTailoring = db.ProductItemTailoringMeasurements.FirstOrDefault(le => le.ProductItemTailoringMeasurementID == ProductItemTailoringMeasurementID);
            try
            {
                if (objTailoring != null)
                {
                    objTailoring.IsDeleted = true;
                    objTailoring.DeletedBy = DeletedBy;
                    objTailoring.DeletedDate = DateTime.Now;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ProductItemTailoringMeasurement GetByID(Int64 ProductItemTailoringMeasurementID)
        {
            return db.ProductItemTailoringMeasurements.FirstOrDefault(le => le.ProductItemTailoringMeasurementID == ProductItemTailoringMeasurementID);
        }
    }
}