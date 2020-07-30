using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.SqlRepository
{
    public class StoreCostCenterCodeRepository : RepositoryBase
    {
        public List<StoreCostCenterCode> GetAllQuery(long StoreID)
        {
            return (from c in db.StoreCostCenterCodes
                    orderby c.CostCenterCodeID 
                    where c.StoreID == StoreID
                    select c).ToList();
        }

        public StoreCostCenterCode GetById(Int64 CostCenterCodeID)
        {
            StoreCostCenterCode objCiry = (from c in db.StoreCostCenterCodes
                                           where c.CostCenterCodeID == CostCenterCodeID
                                           select c).SingleOrDefault();

            return objCiry;
        }

        public void UpdateCostCenterCode(long CostCenterCodeID,long StoreID, string Code)
        {
            StoreCostCenterCode objCostCenterCode = GetById(CostCenterCodeID);

            if (objCostCenterCode != null)
            {
                objCostCenterCode.Code = Code; 
                db.SubmitChanges();
            }
        }

        public bool DeleteCostCenterCode(long CostCenterCodeID)
        {
            Boolean flag = false;
            flag = (from o in db.Orders where o.CostCenterCodeID == CostCenterCodeID select o.OrderID).Count() > 0;
            if (!flag)
            {
                StoreCostCenterCode objcostcentercode = GetById(CostCenterCodeID);
                this.db.StoreCostCenterCodes.DeleteOnSubmit(objcostcentercode);
                this.SubmitChanges();
            }

            return flag;
        } 

        public StoreCostCenterCode CheckIfExist(Int64 CostCenterCodeID,Int64 StoreID, string value)
        {
            var result = db.StoreCostCenterCodes.FirstOrDefault(C => C.CostCenterCodeID != CostCenterCodeID && C.StoreID == StoreID && C.Code == value);
            return result; 
        }

        public List<StoreCostCenterCode> GetByCompanyID(Int64 CompanyID)
        {
            return (from c in db.StoreCostCenterCodes
                                           join cmp in db.CompanyStores on c.StoreID equals cmp.StoreID
                                           where cmp.CompanyID == CompanyID
                                           select c).ToList();
        }
    }
}
