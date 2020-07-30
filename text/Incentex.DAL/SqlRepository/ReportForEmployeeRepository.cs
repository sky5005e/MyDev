using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class ReportForEmployeeRepository : RepositoryBase
    {
        #region Spend Summary Report
        public List<GetSalesYearlWiseResult> GetSalesYearWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            String workgroupIds = null;
            String baseStationIds = null;
            String priceLevelIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 451);
            if (objReportAccessRight != null)
            {
                workgroupIds = objReportAccessRight.WorkgroupID;
                baseStationIds = objReportAccessRight.BaseStationID;
                priceLevelIds = objReportAccessRight.PriceLevel;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds) || String.IsNullOrEmpty(priceLevelIds))
                    return null;
            }
            else
                return null;

            var result = (from order in db.SelectSalesReportForEmployee(workgroupIds, baseStationIds, priceLevelIds)
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderAmount = order.CalculatedOrderAmount
                          }).ToList();
            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { Convert.ToDateTime(info.OrderDate).Year } into newInfo
                             select new GetSalesYearlWiseResult
                             {
                                 year = newInfo.Key.Year,
                                 OrderAmount = newInfo.Sum(s => s.OrderAmount)
                             });

            return newresult.ToList();
        }

        public class GetSalesYearlWiseResult
        {
            public Int64 year { get; set; }
            public Decimal? OrderAmount { get; set; }
        }

        public class GetSalesWorkgroupWiseResult
        {
            public String Workgroup { get; set; }
            public Decimal? OrderAmount { get; set; }
        }

        public List<GetSalesWorkgroupWiseResult> GetSalesWorkgroupWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            String workgroupIds = null;
            String baseStationIds = null;
            String priceLevelIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 451);
            if (objReportAccessRight != null)
            {
                workgroupIds = objReportAccessRight.WorkgroupID;
                baseStationIds = objReportAccessRight.BaseStationID;
                priceLevelIds = objReportAccessRight.PriceLevel;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds) || String.IsNullOrEmpty(priceLevelIds))
                    return null;
            }
            else
                return null;

            var result = (from order in db.SelectSalesReportForEmployee(workgroupIds, baseStationIds, priceLevelIds)
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderAmount = order.OrderAmount + order.ShippingAmount + order.SalesTax - (order.CorporateDiscount != null ? order.CorporateDiscount : 0)
                          }).ToList();
            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.WorkgroupID } into newInfo
                             select new GetSalesWorkgroupWiseResult
                             {
                                 Workgroup = db.INC_Lookups.SingleOrDefault(w => w.iLookupID == newInfo.Key.WorkgroupID).sLookupName,
                                 OrderAmount = newInfo.Sum(s => s.OrderAmount)
                             });

            return newresult.ToList();
        }

        public class GetSalesStationWiseResult
        {
            public String BaseStation { get; set; }
            public Decimal? OrderAmount { get; set; }
            public Decimal? WaitingOrderAmount { get; set; }
            public Decimal? CAIE_MOASOrderAmount { get; set; }
            public Decimal? CAIE_WaitingMOASOrderAmount { get; set; }
            public String WorkGroupIDs { get; set; }
            public String BaseStationIDs { get; set; }
            public String PriceLevelIDs { get; set; }
        }

        public List<GetSalesStationWiseResult> GetSalesStationWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            String workgroupIds = null;
            String baseStationIds = null;
            String priceLevelIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 451);
            if (objReportAccessRight != null)
            {
                workgroupIds = objReportAccessRight.WorkgroupID;
                baseStationIds = objReportAccessRight.BaseStationID;
                priceLevelIds = objReportAccessRight.PriceLevel;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds) || String.IsNullOrEmpty(priceLevelIds))
                    return null;
            }
            else
                return null;

            var result = (from order in db.SelectSalesReportForEmployee(workgroupIds, baseStationIds, priceLevelIds)
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderAmount = order.OrderAmount + order.ShippingAmount + order.SalesTax - (order.CorporateDiscount != null ? order.CorporateDiscount : 0),
                              MOASOrderAmount = (order.MOASOrderAmount != null ? (order.MOASOrderAmount + order.MOASSalesTax + order.ShippingAmount - (order.CorporateDiscount != null ? order.CorporateDiscount : 0)) : 0),
                              OrderStatus = order.OrderStatus,
                              PaymentOption = order.PaymentOption
                          }).ToList();
            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.BaseStation } into newInfo
                             select new GetSalesStationWiseResult
                             {
                                 BaseStation = db.INC_BasedStations.SingleOrDefault(w => w.iBaseStationId == newInfo.Key.BaseStation).sBaseStation,
                                 OrderAmount = newInfo.Where(x => x.OrderStatus.ToUpper() != "ORDER PENDING" && x.PaymentOption != (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS).Sum(s => s.OrderAmount),
                                 WaitingOrderAmount = newInfo.Where(x => x.OrderStatus.ToUpper() == "ORDER PENDING" && x.PaymentOption != (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS).Sum(s => s.OrderAmount),
                                 CAIE_MOASOrderAmount = newInfo.Where(x => x.OrderStatus.ToUpper() != "ORDER PENDING" && x.PaymentOption == (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS).Sum(s => (s.MOASOrderAmount != null ? s.MOASOrderAmount : s.OrderAmount)),
                                 CAIE_WaitingMOASOrderAmount = newInfo.Where(x => x.OrderStatus.ToUpper() == "ORDER PENDING" && x.PaymentOption == (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS).Sum(s => (s.MOASOrderAmount != null ? s.MOASOrderAmount : s.OrderAmount)),
                             })
                             .ToList().Select(t=> new GetSalesStationWiseResult{
                                 BaseStation = t.BaseStation,
                                 OrderAmount = t.OrderAmount + t.CAIE_MOASOrderAmount,
                                 WaitingOrderAmount = t.WaitingOrderAmount + t.CAIE_WaitingMOASOrderAmount,
                                 BaseStationIDs = baseStationIds,
                                 WorkGroupIDs = workgroupIds,
                                 PriceLevelIDs = priceLevelIds                             
                             });

            return newresult.ToList();
        }

        public class GetSalesCompanyVSEmployeeWiseResult
        {
            public String Month { get; set; }
            public Decimal? OrderAmount { get; set; }
        }

        public List<GetSalesCompanyVSEmployeeWiseResult> GetSalesCompanyWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            String workgroupIds = null;
            String baseStationIds = null;
            String priceLevelIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 451);
            if (objReportAccessRight != null)
            {
                workgroupIds = objReportAccessRight.WorkgroupID;
                baseStationIds = objReportAccessRight.BaseStationID;
                priceLevelIds = objReportAccessRight.PriceLevel;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds) || String.IsNullOrEmpty(priceLevelIds))
                    return null;
            }
            else
                return null;

            var result = (from order in db.SelectSalesReportForEmployee(workgroupIds, baseStationIds, priceLevelIds) as IEnumerable<SelectSalesReportForEmployeeResult>
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          join lookup in db.INC_Lookups as IEnumerable<INC_Lookup> on order.PaymentOption equals lookup.iLookupID
                          where (lookup.sLookupName == null ||
                          (lookup.sLookupName != "Employee Payroll Deduct" && lookup.sLookupName != "Credit Card" && lookup.sLookupName != "Personal Credit Card") ||
                          (order.CreditUsed != null && order.CreditAmt != 0)
                          )
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStationID = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderAmount = order.CreditUsed != null && order.CreditAmt != 0 && (lookup.sLookupName == "Employee Payroll Deduct" || lookup.sLookupName == "Credit Card" || lookup.sLookupName == "Personal Credit Card") ? order.CreditAmt : order.CalculatedOrderAmount
                          }).ToList();
            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStationID == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { Convert.ToDateTime(info.OrderDate).Month } into newInfo
                             select new GetSalesCompanyVSEmployeeWiseResult
                             {
                                 Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(newInfo.Key.Month),
                                 OrderAmount = newInfo.Sum(s => s.OrderAmount)
                             });

            return newresult.ToList();
        }

        public List<GetSalesCompanyVSEmployeeWiseResult> GetSalesEmployeeWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            String workgroupIds = null;
            String baseStationIds = null;
            String priceLevelIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 451);
            if (objReportAccessRight != null)
            {
                workgroupIds = objReportAccessRight.WorkgroupID;
                baseStationIds = objReportAccessRight.BaseStationID;
                priceLevelIds = objReportAccessRight.PriceLevel;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds) || String.IsNullOrEmpty(priceLevelIds))
                    return null;
            }
            else
                return null;

            var result = (from order in db.SelectSalesReportForEmployee(workgroupIds, baseStationIds, priceLevelIds) as IEnumerable<SelectSalesReportForEmployeeResult>
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          join lookup in db.INC_Lookups as IEnumerable<INC_Lookup> on order.PaymentOption equals lookup.iLookupID
                          where (lookup.sLookupName == "Employee Payroll Deduct" || lookup.sLookupName == "Credit Card" || lookup.sLookupName == "Personal Credit Card")
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStationID = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderAmount = order.CreditUsed != null && order.CreditAmt != 0 ? (order.CalculatedOrderAmount - order.CreditAmt) : order.CalculatedOrderAmount
                          }).ToList();
            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStationID == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { Convert.ToDateTime(info.OrderDate).Month } into newInfo
                             select new GetSalesCompanyVSEmployeeWiseResult
                             {
                                 Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(newInfo.Key.Month),
                                 OrderAmount = newInfo.Sum(s => s.OrderAmount)
                             });

            return newresult.ToList();
        }

        public List<GetSalesStationWiseResult> GetSalesByReportTag(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, Int64 ReportTagID)
        {
            String priceLevelIds = null;
            Int64[] wIds = null;
            Int64[] bIds = null;
            String workgroupIds = "";
            String baseStationIds = "";
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 451);
            if (objReportAccessRight != null)
            {
                workgroupIds = objReportAccessRight.WorkgroupID;
                baseStationIds = objReportAccessRight.BaseStationID;
                priceLevelIds = objReportAccessRight.PriceLevel;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds) || String.IsNullOrEmpty(priceLevelIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from order in db.SelectSalesByReportTag(priceLevelIds, ReportTagID)
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where companyemp.BaseStation != null && companyemp.BaseStation != 0
                          && wIds.Contains(companyemp.WorkgroupID) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderAmount = order.OrderAmount + order.SalesTax + order.ShippingAmount - (order.Corporatediscount != null ? order.Corporatediscount : 0),
                              OrderStatus = order.OrderStatus
                          }).ToList();

            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.BaseStation } into newInfo
                             select new GetSalesStationWiseResult
                             {
                                 BaseStation = db.INC_BasedStations.SingleOrDefault(w => w.iBaseStationId == newInfo.Key.BaseStation).sBaseStation,
                                 OrderAmount = newInfo.Sum(s => s.OrderAmount),
                                 BaseStationIDs = baseStationIds,
                                 WorkGroupIDs = workgroupIds,
                                 PriceLevelIDs = priceLevelIds
                             });

            return newresult.ToList();
        }

        public Decimal GetShoppingCartAmountByPriceLevel(String shoppingCartIds, String priceList)
        {
            if (shoppingCartIds == String.Empty)
                return 0;
            if (priceList == String.Empty)
                return 0;

            String[] shoppingCartArray = shoppingCartIds.Split(',');
            Int64[] ids = shoppingCartArray.Select(x => Int64.Parse(x)).ToArray();
            String[] priceArray = priceList.Split(',');
            Int64[] price = priceArray.Select(x => Int64.Parse(x)).ToArray();
            Decimal sum = 0;

            if (db.MyShoppinCarts.Where(x => ids.Contains(x.MyShoppingCartID) && price.Contains(Convert.ToInt32(x.PriceLevel))).Count() > 0)
            {
                sum = db.MyShoppinCarts.Where(x => ids.Contains(x.MyShoppingCartID) && price.Contains(Convert.ToInt32(x.PriceLevel))).Sum(x => Convert.ToDecimal(x.Quantity) * Convert.ToDecimal(x.UnitPrice));
            }

            return sum;
        }

        public Decimal GetIssuanceCartAmountByPriceLevel(String issuanceCartIds, String priceList)
        {
            if (issuanceCartIds == String.Empty)
                return 0;
            if (priceList == String.Empty)
                return 0;

            String[] issuanceCartArray = issuanceCartIds.Split(',');
            Int64[] ids = issuanceCartArray.Select(x => Int64.Parse(x)).ToArray();
            String[] priceArray = priceList.Split(',');
            Int32[] price = priceArray.Select(x => Int32.Parse(x)).ToArray();
            Decimal sum = 0;

            if (db.MyIssuanceCarts.Where(x => ids.Contains(x.MyIssuanceCartID) && price.Contains(Convert.ToInt32(x.PriceLevel))).Count() > 0)
            {
                sum = db.MyIssuanceCarts.Where(x => ids.Contains(x.MyIssuanceCartID) && price.Contains(Convert.ToInt32(x.PriceLevel))).Sum(x => Convert.ToDecimal(x.Qty) * Convert.ToDecimal(x.Rate));
            }

            return sum;
        }
        #endregion

        #region Employee Information Report
        public class GetEmployeeWiseResult
        {
            public String Text { get; set; }
            public Int64? Value { get; set; }
        }

        public List<GetEmployeeWiseResult> GetEmployeeStationWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 453);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from companyemp in db.CompanyEmployees
                          join userinfo in db.UserInformations on companyemp.UserInfoID equals userinfo.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          where userinfo.WLSStatusId == 135 && userinfo.IsDeleted == false && companyemp.BaseStation != null && companyemp.BaseStation != 0
                            && wIds.Contains(companyemp.WorkgroupID) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = companystore.StoreID,
                              CreatedDate = userinfo.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID
                          }).ToList();
            if (fromDate != null)
                result = result.Where(r => r.CreatedDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.CreatedDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.BaseStation } into newInfo
                             select new GetEmployeeWiseResult
                             {
                                 Text = db.INC_BasedStations.SingleOrDefault(w => w.iBaseStationId == newInfo.Key.BaseStation).sBaseStation,
                                 Value = newInfo.Count()
                             });

            return newresult.ToList();
        }

        public List<GetEmployeeWiseResult> GetEmployeeWorkgroupWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 453);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from companyemp in db.CompanyEmployees
                          join userinfo in db.UserInformations on companyemp.UserInfoID equals userinfo.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          where userinfo.WLSStatusId == 135 && userinfo.IsDeleted == false && wIds.Contains(companyemp.WorkgroupID) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = companystore.StoreID,
                              CreatedDate = userinfo.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID
                          }).ToList();
            if (fromDate != null)
                result = result.Where(r => r.CreatedDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.CreatedDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.WorkgroupID } into newInfo
                             select new GetEmployeeWiseResult
                             {
                                 Text = db.INC_Lookups.SingleOrDefault(w => w.iLookupID == newInfo.Key.WorkgroupID).sLookupName,
                                 Value = newInfo.Count()
                             });

            return newresult.ToList();
        }

        public List<GetEmployeeWiseResult> GetEmployeeGenderWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 453);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from companyemp in db.CompanyEmployees
                          join userinfo in db.UserInformations on companyemp.UserInfoID equals userinfo.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          where userinfo.WLSStatusId == 135 && userinfo.IsDeleted == false && wIds.Contains(companyemp.WorkgroupID) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = companystore.StoreID,
                              CreatedDate = userinfo.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID
                          }).ToList();
            if (fromDate != null)
                result = result.Where(r => r.CreatedDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.CreatedDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.GenderID } into newInfo
                             select new GetEmployeeWiseResult
                             {
                                 Text = db.INC_Lookups.SingleOrDefault(w => w.iLookupID == newInfo.Key.GenderID).sLookupName,
                                 Value = newInfo.Count()
                             });

            return newresult.ToList();
        }

        public List<GetEmployeeWiseResult> GetEmployeeStatusWise(Int64 UserInfoID, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 453);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from companyemp in db.CompanyEmployees
                          join userinfo in db.UserInformations on companyemp.UserInfoID equals userinfo.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          where userinfo.IsDeleted == false && wIds.Contains(companyemp.WorkgroupID) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = companystore.StoreID,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              Status = userinfo.WLSStatusId
                          }).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.Status } into newInfo
                             select new GetEmployeeWiseResult
                             {
                                 Text = db.INC_Lookups.SingleOrDefault(w => w.iLookupID == newInfo.Key.Status).sLookupName,
                                 Value = newInfo.Count()
                             });

            return newresult.ToList();
        }
        #endregion

        #region Product Planning Report
        public class GetProductSizeWiseResult
        {
            public String Text { get; set; }
            public Int64? Value { get; set; }
        }

        public List<GetProductSizeWiseResult> GetProductSalesSizeWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, Int64? GarmentTypeID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from order in db.SelectProductReportForSize()
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.Size != null && wIds.Contains(order.WorkgroupId) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              Size = order.Size,
                              Qty = order.Quantity,
                              GarmentTypeID = order.GarmentTypeID
                          }).ToList();
            if (GarmentTypeID != null)
                result = result.Where(r => r.GarmentTypeID == GarmentTypeID).ToList();
            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.Size } into newInfo
                             select new GetProductSizeWiseResult
                             {
                                 Text = newInfo.Key.Size,
                                 Value = newInfo.Sum(s => Convert.ToInt64(s.Qty))
                             });

            return newresult.ToList();
        }

        public List<GetProductSizeWiseResult> GetProductSalesColorWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from order in db.SelectProductReportForColor()
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.ItemColorID != null
                          && wIds.Contains(order.WorkgroupId) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              Color = order.ItemColorID,
                              Qty = order.Quantity
                          }).ToList();
            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.Color } into newInfo
                             select new GetProductSizeWiseResult
                             {
                                 Text = db.INC_Lookups.SingleOrDefault(x => x.iLookupID == newInfo.Key.Color).sLookupName,
                                 Value = newInfo.Sum(s => Convert.ToInt64(s.Qty))
                             });

            return newresult.ToList();
        }

        public class GetTopFiftyItemByRevenueResult
        {
            public String Text { get; set; }
            public Decimal? Value { get; set; }
            public Int64? Value1 { get; set; }
        }

        public List<GetTopFiftyItemByRevenueResult> GetTopFiftyItemByRevenue(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from order in db.SelectTopTenMasterItemByRevenue()
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.MasterItemID != null && wIds.Contains(order.WorkgroupId) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              MasterItemID = order.MasterItemID,
                              MasterItemName = order.MasterItemName,
                              Quantity = order.Quantity,
                              UnitPrice = order.UnitPrice
                          }).ToList();
            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.MasterItemID } into newInfo
                             select new GetTopFiftyItemByRevenueResult
                             {
                                 Text = newInfo.First().MasterItemName,
                                 Value = newInfo.Sum(s => s.Quantity * s.UnitPrice),
                                 Value1 = newInfo.Sum(s => Convert.ToInt64(s.Quantity))
                             });

            return newresult.ToList();
        }

        public class GetBackOrderItemResult
        {
            public String Text { get; set; }
            public Int64? Value { get; set; }
            public Int64? Value1 { get; set; }
        }

        public List<GetBackOrderItemResult> GetBackOrderItemReport(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from order in db.SelectBackOrderInventoryReport()
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.MasterItemID != null
                          && wIds.Contains(order.WorkgroupId) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              MasterItemID = order.MasterItemID,
                              MasterItemName = order.MasterItemName,
                              Quantity = order.TotalQuantity,
                              ShippedQuantity = order.ShippedQuantity
                          }).ToList();
            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.MasterItemID } into newInfo
                             select new GetBackOrderItemResult
                             {
                                 Text = newInfo.First().MasterItemName,
                                 Value = newInfo.Sum(s => s.Quantity != null ? s.Quantity : 0),
                                 Value1 = newInfo.Sum(s => s.Quantity != null ? s.Quantity : 0) - newInfo.Sum(s => s.ShippedQuantity != null ? s.ShippedQuantity : 0)
                             });

            return newresult.ToList();
        }

        public class GetInventoryUsageReportResult
        {
            public String Text { get; set; }
            public Int64? OnHand { get; set; }
            public Int64? Sold { get; set; }
            public Int64? OnOrder { get; set; }
            public Int64? ReOrder { get; set; }
            public Int64? Return { get; set; }
        }

        public List<GetInventoryUsageReportResult> GetInventoryUsageReport(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, Int64 MasterItemID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from order in db.SelectInventoryUsageReport()
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.MasterItemID != null && order.MasterItemID == MasterItemID
                          && wIds.Contains(order.WorkgroupId) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              MasterItemID = order.MasterItemID,
                              MasterItemName = order.MasterItemName,
                              Size = order.Size,
                              SizeID = order.SizeID,
                              SoldQuantity = order.SoldQuantity,
                              ReturnQuantity = order.ReturnQuantity
                          }).ToList();
            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            return (from info in result
                    group info by new { info.SizeID } into newInfo
                    select new GetInventoryUsageReportResult
                    {
                        Text = newInfo.FirstOrDefault().Size,
                        OnHand = (from pInventory in db.ProductItemInventories join pItem in db.ProductItems on pInventory.ProductItemID equals pItem.ProductItemID where pItem.MasterStyleID == MasterItemID && pItem.ItemSizeID == newInfo.Key.SizeID select pInventory.Inventory).Sum(),
                        Sold = newInfo.Sum(x => x.SoldQuantity),
                        OnOrder = (from pInventory in db.ProductItemInventories join pItem in db.ProductItems on pInventory.ProductItemID equals pItem.ProductItemID where pItem.MasterStyleID == MasterItemID && pItem.ItemSizeID == newInfo.Key.SizeID select pInventory.OnOrder).Sum(),
                        ReOrder = (from pInventory in db.ProductItemInventories join pItem in db.ProductItems on pInventory.ProductItemID equals pItem.ProductItemID where pItem.MasterStyleID == MasterItemID && pItem.ItemSizeID == newInfo.Key.SizeID select pInventory.ReOrderPoint).Sum(),
                        Return = newInfo.Sum(x => x.ReturnQuantity)
                    }).ToList();
        }

        public class GetProductSalesInventoryStatusWiseResult
        {
            public String ItemNumber { get; set; }
            public String ProductDescription { get; set; }
            public Int64? OnHand { get; set; }
            public Decimal? Price { get; set; }
            public Int64? Usage { get; set; }
            public Decimal? DrawDownValue { get; set; }
            public Decimal? OnHandInventoryValue { get; set; }
        }

        public List<SelectSalesByInventoryStatusResult> GetProductSalesInventoryStatusWise(Int64 UserInfoID, Int64? storeID, Int64? workgroupID, Int64? GarmentTypeID, Int64 InventoryStatusID)
        {
            Int64[] wIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                if (String.IsNullOrEmpty(workgroupIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = db.SelectSalesByInventoryStatus(InventoryStatusID).Where(x => wIds.Contains(x.WorkgroupID)).ToList();

            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (GarmentTypeID != null)
                result = result.Where(r => r.GarmentTypeID == GarmentTypeID).ToList();

            return result;
        }


        #endregion

        #region Service Level ScoreCard Report
        public class GetAverageShipTimeWiseResult
        {
            public String Days { get; set; }
            public Int32 Count { get; set; }
        }

        public List<GetAverageShipTimeWiseResult> GetAverageShipTimeWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from order in db.Orders
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.IsPaid == true && companyemp.BaseStation != null && companyemp.BaseStation != 0
                          && wIds.Contains(companyemp.WorkgroupID) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = order.StoreID,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderDate = order.OrderDate,
                              ShipDate = db.ShipingOrders.Where(x => x.OrderID == order.OrderID && x.ShipingDate != null).OrderByDescending(x => x.ShipingDate).FirstOrDefault().ShipingDate,
                              OrderStatus = order.OrderStatus
                          }).ToList();

            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            List<GetAverageShipTimeWiseResult> objGetAverageShipTimeWiseResultList = new List<GetAverageShipTimeWiseResult>();
            GetAverageShipTimeWiseResult objGetAverageShipTimeWiseResult = new GetAverageShipTimeWiseResult();

            objGetAverageShipTimeWiseResult.Count = (result.Where(X => X.ShipDate == null && X.OrderStatus.ToUpper() == "OPEN")).Count();
            objGetAverageShipTimeWiseResult.Days = "Waiting to Ship";
            objGetAverageShipTimeWiseResultList.Add(objGetAverageShipTimeWiseResult);

            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();

            objGetAverageShipTimeWiseResult = new GetAverageShipTimeWiseResult();
            objGetAverageShipTimeWiseResult.Count = (result.Where(X => SqlMethods.DateDiffDay(X.OrderDate, X.ShipDate) == 1)).Count();
            objGetAverageShipTimeWiseResult.Days = "1 Day Ship";
            objGetAverageShipTimeWiseResultList.Add(objGetAverageShipTimeWiseResult);

            objGetAverageShipTimeWiseResult = new GetAverageShipTimeWiseResult();
            objGetAverageShipTimeWiseResult.Count = (result.Where(X => SqlMethods.DateDiffDay(X.OrderDate, X.ShipDate) == 2)).Count();
            objGetAverageShipTimeWiseResult.Days = "2 Day Ship";
            objGetAverageShipTimeWiseResultList.Add(objGetAverageShipTimeWiseResult);

            objGetAverageShipTimeWiseResult = new GetAverageShipTimeWiseResult();
            objGetAverageShipTimeWiseResult.Count = (result.Where(X => SqlMethods.DateDiffDay(X.OrderDate, X.ShipDate) == 3)).Count();
            objGetAverageShipTimeWiseResult.Days = "3 Day Ship";
            objGetAverageShipTimeWiseResultList.Add(objGetAverageShipTimeWiseResult);

            objGetAverageShipTimeWiseResult = new GetAverageShipTimeWiseResult();
            objGetAverageShipTimeWiseResult.Count = (result.Where(X => SqlMethods.DateDiffDay(X.OrderDate, X.ShipDate) == 4)).Count();
            objGetAverageShipTimeWiseResult.Days = "4 Day Ship";
            objGetAverageShipTimeWiseResultList.Add(objGetAverageShipTimeWiseResult);

            objGetAverageShipTimeWiseResult = new GetAverageShipTimeWiseResult();
            objGetAverageShipTimeWiseResult.Count = (result.Where(X => SqlMethods.DateDiffDay(X.OrderDate, X.ShipDate) == 5)).Count();
            objGetAverageShipTimeWiseResult.Days = "5 Day Ship";
            objGetAverageShipTimeWiseResultList.Add(objGetAverageShipTimeWiseResult);

            objGetAverageShipTimeWiseResult = new GetAverageShipTimeWiseResult();
            objGetAverageShipTimeWiseResult.Count = (result.Where(X => SqlMethods.DateDiffDay(X.OrderDate, X.ShipDate) == 6)).Count();
            objGetAverageShipTimeWiseResult.Days = "6 Day Ship";
            objGetAverageShipTimeWiseResultList.Add(objGetAverageShipTimeWiseResult);

            objGetAverageShipTimeWiseResult = new GetAverageShipTimeWiseResult();
            objGetAverageShipTimeWiseResult.Count = (result.Where(X => SqlMethods.DateDiffDay(X.OrderDate, X.ShipDate) == 7)).Count();
            objGetAverageShipTimeWiseResult.Days = "7 Day Ship";
            objGetAverageShipTimeWiseResultList.Add(objGetAverageShipTimeWiseResult);

            objGetAverageShipTimeWiseResult = new GetAverageShipTimeWiseResult();
            objGetAverageShipTimeWiseResult.Count = (result.Where(X => SqlMethods.DateDiffDay(X.OrderDate, X.ShipDate) == 30)).Count();
            objGetAverageShipTimeWiseResult.Days = "1 Month Ship";
            objGetAverageShipTimeWiseResultList.Add(objGetAverageShipTimeWiseResult);

            objGetAverageShipTimeWiseResult = new GetAverageShipTimeWiseResult();
            objGetAverageShipTimeWiseResult.Count = (result.Where(X => SqlMethods.DateDiffDay(X.OrderDate, X.ShipDate) == 60)).Count();
            objGetAverageShipTimeWiseResult.Days = "2 Month Ship";
            objGetAverageShipTimeWiseResultList.Add(objGetAverageShipTimeWiseResult);

            objGetAverageShipTimeWiseResult = new GetAverageShipTimeWiseResult();
            objGetAverageShipTimeWiseResult.Count = (result.Where(X => SqlMethods.DateDiffDay(X.OrderDate, X.ShipDate) > 90)).Count();
            objGetAverageShipTimeWiseResult.Days = "Over 3 Months";
            objGetAverageShipTimeWiseResultList.Add(objGetAverageShipTimeWiseResult);

            return objGetAverageShipTimeWiseResultList;
        }

        public class GetPurchasesVSReturnsResult
        {
            public String Text { get; set; }
            public Int64? Value { get; set; }
            public Int64? Value1 { get; set; }
        }

        public List<GetPurchasesVSReturnsResult> GetPurchasesVSReturns(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var resultPurchase = (from order in db.SelectProductReportForSize()
                                  join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                                  where order.Size != null && wIds.Contains(order.WorkgroupId) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                                  select new
                                  {
                                      StoreID = order.StoreID,
                                      OrderDate = order.OrderDate,
                                      WorkgroupID = order.WorkgroupId,
                                      BaseStation = companyemp.BaseStation,
                                      GenderID = companyemp.GenderID,
                                      Size = order.Size,
                                      Qty = order.Quantity,
                                      GarmentTypeID = order.GarmentTypeID
                                  }).ToList();

            var resultReturn = (from order in db.Orders
                                join orderreturn in db.ReturnProducts on order.OrderID equals orderreturn.OrderId
                                join shop in db.MyShoppinCarts on orderreturn.MyShoppingCartID equals shop.MyShoppingCartID
                                join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                                where shop.Size != null && order.OrderStatus.ToUpper() != "CANCELED" && order.OrderStatus.ToUpper() != "DELETED" && order.OrderStatus.ToUpper() != "ORDER PENDING" && order.OrderFor == "ShoppingCart"
                                && wIds.Contains(order.WorkgroupId) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                                select new
                                {
                                    StoreID = order.StoreID,
                                    OrderDate = order.OrderDate,
                                    WorkgroupID = order.WorkgroupId,
                                    BaseStation = companyemp.BaseStation,
                                    GenderID = companyemp.GenderID,
                                    Size = shop.Size,
                                    Qty = orderreturn.ReturnQty
                                }).Concat(from order in db.Orders
                                          join orderreturn in db.ReturnProducts on order.OrderID equals orderreturn.OrderId
                                          join issue in db.MyIssuanceCarts on orderreturn.MyShoppingCartID equals issue.MyIssuanceCartID
                                          join look in db.INC_Lookups on issue.ItemSizeID equals look.iLookupID
                                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                                          where look.sLookupName != null && order.OrderStatus.ToUpper() != "CANCELED" && order.OrderStatus.ToUpper() != "DELETED" && order.OrderStatus.ToUpper() != "ORDER PENDING" && order.OrderFor == "IssuanceCart"
                                          && wIds.Contains(order.WorkgroupId) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                                          select new
                                          {
                                              StoreID = order.StoreID,
                                              OrderDate = order.OrderDate,
                                              WorkgroupID = order.WorkgroupId,
                                              BaseStation = companyemp.BaseStation,
                                              GenderID = companyemp.GenderID,
                                              Size = look.sLookupName,
                                              Qty = orderreturn.ReturnQty
                                          }).ToList();
            if (fromDate != null)
            {
                resultReturn = resultReturn.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
                resultPurchase = resultPurchase.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            }
            if (toDate != null)
            {
                resultReturn = resultReturn.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
                resultPurchase = resultPurchase.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            }
            if (storeID != null)
            {
                resultReturn = resultReturn.Where(r => r.StoreID == storeID).ToList();
                resultPurchase = resultPurchase.Where(r => r.StoreID == storeID).ToList();
            }
            if (workgroupID != null)
            {
                resultReturn = resultReturn.Where(r => r.WorkgroupID == workgroupID).ToList();
                resultPurchase = resultPurchase.Where(r => r.WorkgroupID == workgroupID).ToList();
            }
            if (stationID != null)
            {
                resultReturn = resultReturn.Where(r => r.BaseStation == stationID).ToList();
                resultPurchase = resultPurchase.Where(r => r.BaseStation == stationID).ToList();
            }
            if (genderID != null)
            {
                resultReturn = resultReturn.Where(r => r.GenderID == genderID).ToList();
                resultPurchase = resultPurchase.Where(r => r.GenderID == genderID).ToList();
            }

            var newResultPurchase = (from info in resultPurchase
                                     group info by new { info.Size } into newInfo
                                     select new GetProductSizeWiseResult
                                     {
                                         Text = newInfo.Key.Size,
                                         Value = newInfo.Sum(s => Convert.ToInt64(s.Qty))
                                     });

            var newResultReturn = (from info in resultReturn
                                   group info by new { info.Size } into newInfo
                                   select new GetProductSizeWiseResult
                                   {
                                       Text = newInfo.Key.Size,
                                       Value = newInfo.Sum(s => Convert.ToInt64(s.Qty))
                                   });

            //This is for joining 2 result and create single result
            return (from objPurchase in newResultPurchase.AsEnumerable()
                    join objReturn in newResultReturn.AsEnumerable() on objPurchase.Text equals objReturn.Text into joinPurchaseReturn
                    from objReturnMain in joinPurchaseReturn.DefaultIfEmpty()
                    select new GetPurchasesVSReturnsResult
                    {
                        Text = objPurchase.Text,
                        Value = objPurchase.Value,
                        Value1 = objReturnMain != null && objReturnMain.Value != null ? objReturnMain.Value : null
                    }).Where(x => x.Value != 0 && x.Value1 != 0).ToList();

        }

        public class GetHelpTicketsbyLocationResult
        {
            public String BaseStation { get; set; }
            public Int64? TicketCount { get; set; }
        }

        public List<GetHelpTicketsbyLocationResult> GetHelpTicketsbyLocation(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from ticket in db.ServiceTickets
                          join userInfo in db.UserInformations on ticket.ContactID equals userInfo.UserInfoID
                          join companyemp in db.CompanyEmployees on userInfo.UserInfoID equals companyemp.UserInfoID
                          join companyStore in db.CompanyStores on userInfo.CompanyId equals companyStore.CompanyID
                          where companyemp.BaseStation != null && companyemp.BaseStation != 0 && ticket.IsDeleted == false
                          && wIds.Contains(companyemp.WorkgroupID) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = companyStore.StoreID,
                              TicketDate = ticket.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStationID = companyemp.BaseStation,
                              GenderID = companyemp.GenderID
                          }).ToList();

            if (fromDate != null)
                result = result.Where(r => r.TicketDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.TicketDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStationID == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.BaseStationID } into newInfo
                             select new GetHelpTicketsbyLocationResult
                             {
                                 BaseStation = db.INC_BasedStations.SingleOrDefault(w => w.iBaseStationId == newInfo.Key.BaseStationID).sBaseStation,
                                 TicketCount = newInfo.Count()
                             });

            return newresult.ToList();
        }

        public class GetHelpTicketsbyWorkgroupResult
        {
            public String Workgroup { get; set; }
            public Int64? TicketCount { get; set; }
        }

        public List<GetHelpTicketsbyWorkgroupResult> GetHelpTicketsbyWorkgroup(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from ticket in db.ServiceTickets
                          join userInfo in db.UserInformations on ticket.ContactID equals userInfo.UserInfoID
                          join companyemp in db.CompanyEmployees on userInfo.UserInfoID equals companyemp.UserInfoID
                          join companyStore in db.CompanyStores on userInfo.CompanyId equals companyStore.CompanyID
                          where wIds.Contains(companyemp.WorkgroupID) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          && ticket.IsDeleted == false
                          select new
                          {
                              StoreID = companyStore.StoreID,
                              TicketDate = ticket.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStationID = companyemp.BaseStation,
                              GenderID = companyemp.GenderID
                          }).ToList();

            if (fromDate != null)
                result = result.Where(r => r.TicketDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.TicketDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStationID == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.WorkgroupID } into newInfo
                             select new GetHelpTicketsbyWorkgroupResult
                             {
                                 Workgroup = db.INC_Lookups.SingleOrDefault(w => w.iLookupID == newInfo.Key.WorkgroupID).sLookupName,
                                 TicketCount = newInfo.Count()
                             });

            return newresult.ToList();
        }

        public class GetHelpTicketsUsersvsTotalEmployeesResult
        {
            public Int64 Value { get; set; }
            public Int64 Value1 { get; set; }
        }

        public List<GetHelpTicketsUsersvsTotalEmployeesResult> GetHelpTicketsUsersvsTotalEmployees(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var resultEmployee = (from userInfo in db.UserInformations
                                  join companyemp in db.CompanyEmployees on userInfo.UserInfoID equals companyemp.UserInfoID
                                  join companyStore in db.CompanyStores on userInfo.CompanyId equals companyStore.CompanyID
                                  where wIds.Contains(companyemp.WorkgroupID) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                                  select new
                                  {
                                      UserInfoID = userInfo.UserInfoID,
                                      StoreID = companyStore.StoreID,
                                      WorkgroupID = companyemp.WorkgroupID,
                                      BaseStationID = companyemp.BaseStation,
                                      GenderID = companyemp.GenderID
                                  }).ToList();

            if (storeID != null)
                resultEmployee = resultEmployee.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                resultEmployee = resultEmployee.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                resultEmployee = resultEmployee.Where(r => r.BaseStationID == stationID).ToList();
            if (genderID != null)
                resultEmployee = resultEmployee.Where(r => r.GenderID == genderID).ToList();

            var resultTicket = db.ServiceTickets.AsEnumerable().Where(t => t.IsDeleted == false).Join(resultEmployee, c => c.ContactID, ci => ci.UserInfoID, (c, ci) => c.ContactID).GroupBy(x => x.Value).ToList();

            List<GetHelpTicketsUsersvsTotalEmployeesResult> objGetHelpTicketsUsersvsTotalEmployeesList = new List<GetHelpTicketsUsersvsTotalEmployeesResult>();
            GetHelpTicketsUsersvsTotalEmployeesResult objGetHelpTicketsUsersvsTotalEmployeesResult = new GetHelpTicketsUsersvsTotalEmployeesResult();

            objGetHelpTicketsUsersvsTotalEmployeesResult.Value = resultEmployee.Count();
            objGetHelpTicketsUsersvsTotalEmployeesResult.Value1 = resultTicket.Count();
            objGetHelpTicketsUsersvsTotalEmployeesList.Add(objGetHelpTicketsUsersvsTotalEmployeesResult);

            return objGetHelpTicketsUsersvsTotalEmployeesList;
        }


        public class GetHelpTicketsbyTypeResult
        {
            public String Type { get; set; }
            public Int64? TicketCount { get; set; }
        }
        public List<GetHelpTicketsbyTypeResult> GetHelpTicketsbyType(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from ticket in db.ServiceTickets
                          join userInfo in db.UserInformations on ticket.ContactID equals userInfo.UserInfoID
                          join companyemp in db.CompanyEmployees on userInfo.UserInfoID equals companyemp.UserInfoID
                          join companyStore in db.CompanyStores on userInfo.CompanyId equals companyStore.CompanyID
                          //join lk in db.INC_Lookups on ticket.TypeOfRequestID equals lk.iLookupID
                          where ticket.TypeOfRequestID != null && ticket.TypeOfRequestID != 0 && ticket.IsDeleted == false
                           && wIds.Contains(companyemp.WorkgroupID) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = companyStore.StoreID,
                              TicketDate = ticket.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStationID = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              TypeOfRequest = ticket.TypeOfRequestID
                          }).ToList();

            if (fromDate != null)
                result = result.Where(r => r.TicketDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.TicketDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStationID == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            var newresult = (from info in result
                             group info by new { info.TypeOfRequest } into newInfo
                             select new GetHelpTicketsbyTypeResult
                             {
                                 Type = db.INC_Lookups.SingleOrDefault(w => w.iLookupID == newInfo.Key.TypeOfRequest).sLookupName,
                                 TicketCount = newInfo.Count()
                             });

            return newresult.ToList();
        }

        public List<SelectServiceTicketsBySearchCriteriaForReportResult> SelectServiceTicketsBySearchCriteriaForReport(Int64? CompanyID, Int64? ContactID, Int64? OpenedByID, Int64? StatusID, Int64? OwnerID, String TicketName, String TicketNumber, String DateNeeded, Int64? SupplierID, Int64? TypeOfRequestID, String KeyWord, Int64? SubOwnerID, String FromDate, String ToDate, Int64? UserInfoID, Int64? WorkgroupID, Int64? BaseStationID, Boolean? NoActivity)
        {
            var result = db.SelectServiceTicketsBySearchCriteriaForReport(CompanyID, ContactID, OpenedByID, StatusID, OwnerID, TicketName, TicketNumber, DateNeeded, SupplierID, TypeOfRequestID, KeyWord, SubOwnerID, FromDate, ToDate, UserInfoID, WorkgroupID, BaseStationID, NoActivity).ToList();
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                result = (from companyemp in result
                          where wIds.Contains(Convert.ToInt64(companyemp.WorkGroupID)) && bIds.Contains(Convert.ToInt64(companyemp.BaseStationID))
                          select companyemp).ToList();
            }
            else
                return null;



            return result;
        }

        public class GetShipCompleteCountDayWiseResult
        {
            public String Days { get; set; }
            public Int32 Count { get; set; }
        }

        public List<GetShipCompleteCountDayWiseResult> GetShipCompleteCountDayWise(out Int32 TotalOrderPlaced, Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            TotalOrderPlaced = 0;

            var result = (from order in db.Orders
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.IsPaid == true
                          select new
                          {
                              StoreID = order.StoreID,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderDate = order.OrderDate,
                              ShipDate = db.ShipingOrders.Where(x => x.OrderID == order.OrderID && x.ShipingDate != null).OrderByDescending(x => x.ShipingDate).FirstOrDefault().ShipingDate,
                              OrderStatus = order.OrderStatus
                          }).ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
                if (objReportAccessRight != null)
                {
                    String workgroupIds = objReportAccessRight.WorkgroupID;
                    String baseStationIds = objReportAccessRight.BaseStationID;
                    if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                        return null;

                    Int64[] wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    result = result.Where(x => x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }

            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            List<GetShipCompleteCountDayWiseResult> objGetShipCompleteCountDayWiseResultList = new List<GetShipCompleteCountDayWiseResult>();
            GetShipCompleteCountDayWiseResult objGetShipCompleteCountDayWiseResult = new GetShipCompleteCountDayWiseResult();

            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();

            //To Calculate the Total Order Placed 
            TotalOrderPlaced = result.Where(r => r.OrderStatus.ToUpper() == "OPEN" || r.OrderStatus.ToUpper() == "CLOSED").ToList().Count;

            //To Calculate the Total Order Placed and which are closed or Ship Complete
            result = result.Where(r => r.OrderStatus.ToUpper() == "CLOSED").ToList();

            objGetShipCompleteCountDayWiseResult = new GetShipCompleteCountDayWiseResult();
            objGetShipCompleteCountDayWiseResult.Count = (result.Where(X => GetDateDifference(X.OrderDate, X.ShipDate, 0, 1))).Count();
            objGetShipCompleteCountDayWiseResult.Days = "Days: 0-1";
            objGetShipCompleteCountDayWiseResultList.Add(objGetShipCompleteCountDayWiseResult);

            objGetShipCompleteCountDayWiseResult = new GetShipCompleteCountDayWiseResult();
            objGetShipCompleteCountDayWiseResult.Count = (result.Where(X => GetDateDifference(X.OrderDate, X.ShipDate, 2, 5))).Count();
            objGetShipCompleteCountDayWiseResult.Days = "Days: 2-5";
            objGetShipCompleteCountDayWiseResultList.Add(objGetShipCompleteCountDayWiseResult);

            objGetShipCompleteCountDayWiseResult = new GetShipCompleteCountDayWiseResult();
            objGetShipCompleteCountDayWiseResult.Count = (result.Where(X => GetDateDifference(X.OrderDate, X.ShipDate, 6, 7))).Count();
            objGetShipCompleteCountDayWiseResult.Days = "Days: 6-7";
            objGetShipCompleteCountDayWiseResultList.Add(objGetShipCompleteCountDayWiseResult);

            objGetShipCompleteCountDayWiseResult = new GetShipCompleteCountDayWiseResult();
            objGetShipCompleteCountDayWiseResult.Count = (result.Where(X => GetDateDifference(X.OrderDate, X.ShipDate, 8, 14))).Count();
            objGetShipCompleteCountDayWiseResult.Days = "Days: 8-14";
            objGetShipCompleteCountDayWiseResultList.Add(objGetShipCompleteCountDayWiseResult);

            objGetShipCompleteCountDayWiseResult = new GetShipCompleteCountDayWiseResult();
            objGetShipCompleteCountDayWiseResult.Count = (result.Where(X => GetDateDifference(X.OrderDate, X.ShipDate, 14, -1))).Count();
            objGetShipCompleteCountDayWiseResult.Days = "Days: 14-Later";
            objGetShipCompleteCountDayWiseResultList.Add(objGetShipCompleteCountDayWiseResult);


            return objGetShipCompleteCountDayWiseResultList;
        }

        /// <summary>
        /// To Skip Saturday and Sunday from Ship-Complete Day Count 
        /// added by prashant 10th Jan 2013
        /// </summary>
        /// <param name="OrderDate"></param>
        /// <param name="ShipDate"></param>
        /// <param name="FromDayCount"></param>
        /// <param name="ToDayCount"></param>
        /// <returns></returns>
        public Boolean GetDateDifference(DateTime? OrderDate, DateTime? ShipDate, Int32 FromDayCount, Int32 ToDayCount)
        {
            DateTime? TempDate = OrderDate;
            Int32 Count = 0;
            if (ShipDate != null)
            {
                if (OrderDate < ShipDate)
                {
                    while (Convert.ToDateTime(TempDate).Date != Convert.ToDateTime(ShipDate).Date)
                    {
                        if (Convert.ToDateTime(TempDate).DayOfWeek.ToString() != "Saturday" || Convert.ToDateTime(TempDate).DayOfWeek.ToString() != "Sunday")
                            Count++;
                        TempDate = TempDate.Value.AddDays(1);
                    }
                }

                if (ToDayCount != -1)
                {
                    if (Count >= FromDayCount && Count <= ToDayCount)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (Count > FromDayCount)
                        return true;
                    else
                        return false;
                }
            }
            else if (ToDayCount == -1)
            {
                //for 14 or more days shipped
                return true;
            }
            else
                return false;
        }


        #endregion

        #region Order Management Report
        public class GetOrderAtAGlanceWiseResult
        {
            public String Status { get; set; }
            public Int32 Count { get; set; }
        }

        public List<GetOrderAtAGlanceWiseResult> GetOrderAtAGlanceWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 2221);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from order in db.Orders
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.IsPaid == true && wIds.Contains(order.WorkgroupId) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              StoreID = order.StoreID,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderDate = order.OrderDate,
                              OrderStatus = order.OrderStatus
                          }).ToList();

            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            return (from info in result
                    group info by new { info.OrderStatus } into newInfo
                    select new GetOrderAtAGlanceWiseResult
                    {
                        Status = newInfo.Key.OrderStatus,
                        Count = newInfo.Count()
                    }).ToList();
        }

        public class GetEmployeePayrollDeductWiseResult
        {
            public String EmployeeName { get; set; }
            public String EmployeeNo { get; set; }
            public String OrderNumber { get; set; }
            public Int64 OrderID { get; set; }
            public Decimal? EDPAmount { get; set; }
            public Boolean IsEDPReceived { get; set; }
        }

        public List<GetEmployeePayrollDeductWiseResult> GetEmployeePayrollDeductWise(Int64 UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            Int64[] wIds = null;
            Int64[] bIds = null;
            ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 2221);
            if (objReportAccessRight != null)
            {
                String workgroupIds = objReportAccessRight.WorkgroupID;
                String baseStationIds = objReportAccessRight.BaseStationID;
                if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds))
                    return null;

                wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
            }
            else
                return null;

            var result = (from order in db.Orders
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          join userinfo in db.UserInformations on order.UserId equals userinfo.UserInfoID
                          where order.OrderStatus.ToUpper() != "CANCELED" && order.OrderStatus.ToUpper() != "DELETED" && order.OrderStatus.ToUpper() != "ORDER PENDING" && order.IsPaid == true && order.PaymentOption == 165
                          && wIds.Contains(order.WorkgroupId) && bIds.Contains(Convert.ToInt64(companyemp.BaseStation))
                          select new
                          {
                              EmployeeName = userinfo.FirstName + " " + userinfo.LastName,
                              EmployeeNo = companyemp.EmployeeID,
                              OrderID = order.OrderID,
                              OrderNumber = order.OrderNumber,
                              EDPAmount = order.CreditUsed != null && order.CreditAmt != 0 ? ((order.OrderAmount + order.ShippingAmount + order.SalesTax) - order.CreditAmt) : (order.OrderAmount + order.ShippingAmount + order.SalesTax),
                              StoreID = order.StoreID,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderDate = order.OrderDate,
                              OrderStatus = order.OrderStatus,
                              IsEDPReceived = order.IsEDPReceived
                          }).ToList();

            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            return (from info in result
                    where info.EDPAmount != 0
                    select new GetEmployeePayrollDeductWiseResult
                    {
                        EmployeeName = info.EmployeeName,
                        EmployeeNo = info.EmployeeNo,
                        OrderNumber = info.OrderNumber,
                        OrderID = info.OrderID,
                        EDPAmount = info.EDPAmount,
                        IsEDPReceived = info.IsEDPReceived
                    }).ToList();
        }
        #endregion

        public List<SelectBackOrderReportResult> GetBackOrderReport(Int64 UserInfoID, DateTime? FromDate, DateTime? ToDate, Int64? StoreID, Int64? WorkgroupID, Int64? BaseStationID, Int64? GenderID)
        {
            var result = db.SelectBackOrderReport().ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 2221);
                if (objReportAccessRight != null)
                {
                    String workgroupIds = objReportAccessRight.WorkgroupID;
                    String baseStationIds = objReportAccessRight.BaseStationID;
                    String storeIds = objReportAccessRight.StoreID;
                    if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds) || String.IsNullOrEmpty(storeIds))
                        return null;

                    Int64[] wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] sIds = storeIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && wIds.Contains(x.WorkgroupId) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }

            if (FromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= FromDate).ToList();
            if (ToDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= ToDate).ToList();
            if (StoreID != null)
                result = result.Where(r => r.StoreID == StoreID).ToList();
            if (WorkgroupID != null)
                result = result.Where(r => r.WorkgroupId == WorkgroupID).ToList();
            if (BaseStationID != null)
                result = result.Where(r => r.BaseStation == BaseStationID).ToList();
            if (GenderID != null)
                result = result.Where(r => r.GenderID == GenderID).ToList();

            return result;
        }
    }
}
