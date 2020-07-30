using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.Linq;

namespace Incentex.DAL.SqlRepository
{
    public class ReportRepository : RepositoryBase
    {
        #region Spend Summary Report
        public List<GetSalesYearlWiseResult> GetSalesYearWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, String OrderStatus, String PriceLevel)
        {
            var result = (from order in db.SelectSalesReport(PriceLevel)
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.OrderStatus.ToUpper() != "ORDER PENDING"
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderAmount = order.CalculatedOrderAmount + order.SalesTax + order.ShippingAmount,
                              OrderStatus = order.OrderStatus
                          }).ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 451);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }

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
            if (!String.IsNullOrEmpty(OrderStatus))
                result = result.Where(r => r.OrderStatus == OrderStatus).ToList();

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
            public Decimal? CAIE_MOASOrderAmount { get; set; }
            public String WorkGroupIDs { get; set; }
            public String BaseStationIDs { get; set; }
            public String PriceLevelIDs { get; set; }
        }

        public List<GetSalesWorkgroupWiseResult> GetSalesWorkgroupWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, String OrderStatus, String PriceLevel)
        {
            var result = (from order in db.SelectSalesReport(PriceLevel)
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.OrderStatus.ToUpper() != "ORDER PENDING"
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderAmount = order.OrderAmount + order.SalesTax + order.ShippingAmount - (order.CorporateDiscount != null ? order.CorporateDiscount : 0),
                              MOASOrderAmount = (order.MOASOrderAmount != null ? (order.MOASOrderAmount + order.SalesTax + order.ShippingAmount - (order.CorporateDiscount != null ? order.CorporateDiscount : 0)) : 0),
                              OrderStatus = order.OrderStatus,
                              PaymentOption = order.PaymentOption
                          }).ToList();

            String workgroupIds = "", baseStationIds = "";
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 451);
                if (objReportAccessRight != null)
                {
                    workgroupIds = objReportAccessRight.WorkgroupID;
                    baseStationIds = objReportAccessRight.BaseStationID;
                    String storeIds = objReportAccessRight.StoreID;
                    if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds) || String.IsNullOrEmpty(storeIds))
                        return null;

                    Int64[] wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] sIds = storeIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }

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
            if (!String.IsNullOrEmpty(OrderStatus))
                result = result.Where(r => r.OrderStatus == OrderStatus).ToList();

            var newresult = (from info in result
                             group info by new { info.WorkgroupID } into newInfo
                             select new GetSalesWorkgroupWiseResult
                             {
                                 Workgroup = db.INC_Lookups.SingleOrDefault(w => w.iLookupID == newInfo.Key.WorkgroupID).sLookupName,
                                 OrderAmount = newInfo.Where(x => x.PaymentOption != (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS).Sum(s => s.OrderAmount),
                                 CAIE_MOASOrderAmount = newInfo.Where(x => x.PaymentOption == (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS).Sum(s => (s.MOASOrderAmount != null ? s.MOASOrderAmount : s.OrderAmount)),
                                 WorkGroupIDs = workgroupIds,
                                 BaseStationIDs = baseStationIds
                             });

            return newresult.ToList();
        }

        public class GetSalesStationWiseResult
        {
            public Int64? CurrentBaseStationID { get; set; }
            public String BaseStation { get; set; }
            public Decimal? OrderAmount { get; set; }
            public Decimal? WaitingOrderAmount { get; set; }
            public Decimal? CAIE_MOASOrderAmount { get; set; }
            public Decimal? CAIE_WaitingMOASOrderAmount { get; set; }
            public String WorkGroupIDs { get; set; }
            public String BaseStationIDs { get; set; }
            public String PriceLevelIDs { get; set; }
        }

        public List<GetSalesStationWiseResult> GetSalesStationWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, String OrderStatus, String PriceLevel)
        {
            var result = (from order in db.SelectSalesReport(PriceLevel)
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where companyemp.BaseStation != null && companyemp.BaseStation != 0
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderAmount = order.OrderAmount + order.SalesTax + order.ShippingAmount - (order.CorporateDiscount != null ? order.CorporateDiscount : 0),
                              MOASOrderAmount = (order.MOASOrderAmount != null ? (order.MOASOrderAmount + order.MOASSalesTax + order.ShippingAmount - (order.CorporateDiscount != null ? order.CorporateDiscount : 0)) : 0),
                              OrderStatus = order.OrderStatus,
                              PaymentOption = order.PaymentOption
                          }).ToList();
            String workgroupIds = "", baseStationIds = "";
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 451);
                if (objReportAccessRight != null)
                {
                    workgroupIds = objReportAccessRight.WorkgroupID;
                    baseStationIds = objReportAccessRight.BaseStationID;
                    String storeIds = objReportAccessRight.StoreID;
                    if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds) || String.IsNullOrEmpty(storeIds))
                        return null;

                    Int64[] wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] sIds = storeIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }

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
            if (!String.IsNullOrEmpty(OrderStatus))
                result = result.Where(r => r.OrderStatus == OrderStatus).ToList();


            var newresult = (from info in result
                             group info by new { info.BaseStation } into newInfo
                             select new GetSalesStationWiseResult
                             {
                                 CurrentBaseStationID = newInfo.Key.BaseStation,
                                 BaseStation = db.INC_BasedStations.SingleOrDefault(w => w.iBaseStationId == newInfo.Key.BaseStation).sBaseStation,
                                 OrderAmount = newInfo.Where(x => x.OrderStatus.ToUpper() != "ORDER PENDING" && x.PaymentOption != (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS).Sum(s => s.OrderAmount),
                                 WaitingOrderAmount = newInfo.Where(x => x.OrderStatus.ToUpper() == "ORDER PENDING" && x.PaymentOption != (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS).Sum(s => s.OrderAmount),
                                 CAIE_MOASOrderAmount = newInfo.Where(x => x.OrderStatus.ToUpper() != "ORDER PENDING" && x.PaymentOption == (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS).Sum(s => (s.MOASOrderAmount != null ? s.MOASOrderAmount : s.OrderAmount)),
                                 CAIE_WaitingMOASOrderAmount = newInfo.Where(x => x.OrderStatus.ToUpper() == "ORDER PENDING" && x.PaymentOption == (Int64)Incentex.DAL.Common.DAEnums.PaymentOptions.MOAS).Sum(s => (s.MOASOrderAmount != null ? s.MOASOrderAmount : s.OrderAmount)),
                                 WorkGroupIDs = workgroupIds,
                                 BaseStationIDs = baseStationIds
                             });

            return newresult.ToList();
        }

        public class GetSalesCompanyVSEmployeeWiseResult
        {
            public String Month { get; set; }
            public Decimal? OrderAmount { get; set; }
        }

        public List<GetSalesCompanyVSEmployeeWiseResult> GetSalesCompanyWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, String OrderStatus, String PriceLevel)
        {
            var result = (from order in db.SelectSalesReport(PriceLevel) as IEnumerable<SelectSalesReportResult>
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          join lookup in db.INC_Lookups as IEnumerable<INC_Lookup> on order.PaymentOption equals lookup.iLookupID
                          where order.OrderStatus.ToUpper() != "ORDER PENDING" &&
                          (lookup.sLookupName == null ||
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
                              OrderAmount = order.CreditUsed != null && order.CreditAmt != 0 && (lookup.sLookupName == "Employee Payroll Deduct" || lookup.sLookupName == "Credit Card" || lookup.sLookupName == "Personal Credit Card") ? order.CreditAmt : (order.CalculatedOrderAmount + order.SalesTax + order.ShippingAmount),
                              OrderStatus = order.OrderStatus
                          }).ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 451);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStationID != null && x.BaseStationID != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStationID))).ToList();
                }
                else
                    return null;
            }

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
            if (!String.IsNullOrEmpty(OrderStatus))
                result = result.Where(r => r.OrderStatus == OrderStatus).ToList();

            var newresult = (from info in result
                             group info by new { Convert.ToDateTime(info.OrderDate).Month } into newInfo
                             select new GetSalesCompanyVSEmployeeWiseResult
                             {
                                 Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(newInfo.Key.Month),
                                 OrderAmount = newInfo.Sum(s => s.OrderAmount)
                             });

            return newresult.ToList();
        }

        public List<GetSalesCompanyVSEmployeeWiseResult> GetSalesEmployeeWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, String OrderStatus, String PriceLevel)
        {
            var result = (from order in db.SelectSalesReport(PriceLevel) as IEnumerable<SelectSalesReportResult>
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          join lookup in db.INC_Lookups as IEnumerable<INC_Lookup> on order.PaymentOption equals lookup.iLookupID
                          where order.OrderStatus.ToUpper() != "ORDER PENDING" &&
                          (lookup.sLookupName == "Employee Payroll Deduct" || lookup.sLookupName == "Credit Card" || lookup.sLookupName == "Personal Credit Card")
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStationID = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderAmount = order.CreditUsed != null && order.CreditAmt != 0 ? ((order.OrderAmount + order.ShippingAmount + order.SalesTax) - order.CreditAmt) : (order.OrderAmount + order.ShippingAmount + order.SalesTax),
                              OrderStatus = order.OrderStatus
                          }).ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 451);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStationID != null && x.BaseStationID != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStationID))).ToList();
                }
                else
                    return null;
            }

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
            if (!String.IsNullOrEmpty(OrderStatus))
                result = result.Where(r => r.OrderStatus == OrderStatus).ToList();

            var newresult = (from info in result
                             group info by new { Convert.ToDateTime(info.OrderDate).Month } into newInfo
                             select new GetSalesCompanyVSEmployeeWiseResult
                             {
                                 Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(newInfo.Key.Month),
                                 OrderAmount = newInfo.Sum(s => s.OrderAmount)
                             });

            return newresult.ToList();
        }

        public List<GetSalesStationWiseResult> GetSalesByReportTag(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, String OrderStatus, String PriceLevel, Int64 ReportTagID)
        {
            var result = (from order in db.SelectSalesByReportTag(PriceLevel, ReportTagID)
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where companyemp.BaseStation != null && companyemp.BaseStation != 0
                          select new
                          {
                              StoreID = order.StoreID,
                              OrderDate = order.OrderDate,
                              WorkgroupID = order.WorkgroupId,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              OrderAmount = order.OrderAmount + order.SalesTax + order.ShippingAmount - order.Corporatediscount,
                              OrderStatus = order.OrderStatus
                          }).ToList();
            String workgroupIds = "", baseStationIds = "";
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 451);
                if (objReportAccessRight != null)
                {
                    workgroupIds = objReportAccessRight.WorkgroupID;
                    baseStationIds = objReportAccessRight.BaseStationID;
                    String storeIds = objReportAccessRight.StoreID;
                    if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds) || String.IsNullOrEmpty(storeIds))
                        return null;

                    Int64[] wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] sIds = storeIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }

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
            if (!String.IsNullOrEmpty(OrderStatus))
                result = result.Where(r => r.OrderStatus == OrderStatus).ToList();

            var newresult = (from info in result
                             group info by new { info.BaseStation } into newInfo
                             select new GetSalesStationWiseResult
                             {
                                 BaseStation = db.INC_BasedStations.SingleOrDefault(w => w.iBaseStationId == newInfo.Key.BaseStation).sBaseStation,
                                 OrderAmount = newInfo.Sum(s => s.OrderAmount),
                                 WorkGroupIDs = workgroupIds,
                                 BaseStationIDs = baseStationIds
                             });

            return newresult.ToList();
        }

        #endregion

        #region Employee Information Report
        public class GetEmployeeWiseResult
        {
            public String Text { get; set; }
            public Int64? Value { get; set; }
        }

        public List<GetEmployeeWiseResult> GetEmployeeStationWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from companyemp in db.CompanyEmployees
                          join userinfo in db.UserInformations on companyemp.UserInfoID equals userinfo.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          where userinfo.WLSStatusId == 135 && userinfo.IsDeleted == false && companyemp.BaseStation != null && companyemp.BaseStation != 0
                          select new
                          {
                              StoreID = companystore.StoreID,
                              CreatedDate = userinfo.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID
                          }).ToList();
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 453);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
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

        public List<GetEmployeeWiseResult> GetEmployeeWorkgroupWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from companyemp in db.CompanyEmployees
                          join userinfo in db.UserInformations on companyemp.UserInfoID equals userinfo.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          where userinfo.WLSStatusId == 135 && userinfo.IsDeleted == false
                          select new
                          {
                              StoreID = companystore.StoreID,
                              CreatedDate = userinfo.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID
                          }).ToList();
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 453);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
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

        public List<GetEmployeeWiseResult> GetEmployeeGenderWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from companyemp in db.CompanyEmployees
                          join userinfo in db.UserInformations on companyemp.UserInfoID equals userinfo.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          where userinfo.WLSStatusId == 135 && userinfo.IsDeleted == false
                          select new
                          {
                              StoreID = companystore.StoreID,
                              CreatedDate = userinfo.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID
                          }).ToList();
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 453);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
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

        public List<GetEmployeeWiseResult> GetEmployeeStatusWise(Int64? UserInfoID, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from companyemp in db.CompanyEmployees
                          join userinfo in db.UserInformations on companyemp.UserInfoID equals userinfo.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          where userinfo.IsDeleted == false
                          select new
                          {
                              StoreID = companystore.StoreID,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              Status = userinfo.WLSStatusId
                          }).ToList();
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 453);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
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

        public List<GetProductSizeWiseResult> GetProductSalesSizeWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, Int64? GarmentTypeID)
        {
            var result = (from order in db.SelectProductReportForSize()
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
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

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
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

        public List<GetProductSizeWiseResult> GetProductSalesColorWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from order in db.SelectProductReportForColor()
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.ItemColorID != null
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

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
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

        public List<GetTopFiftyItemByRevenueResult> GetTopFiftyItemByRevenue(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from order in db.SelectTopTenMasterItemByRevenue()
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.MasterItemID != null
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

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
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

        public class GetBackOrderItemsResult
        {
            public String Text { get; set; }
            public Int64? Value { get; set; }
            public Int64? Value1 { get; set; }
        }

        public List<GetBackOrderItemsResult> GetBackOrderItemsReport(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from order in db.SelectBackOrderInventoryReport()
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.MasterItemID != null
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

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
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
                             select new GetBackOrderItemsResult
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

        public List<GetInventoryUsageReportResult> GetInventoryUsageReport(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, Int64 MasterItemID)
        {
            var result = (from order in db.SelectInventoryUsageReport()
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          where order.MasterItemID != null && order.MasterItemID == MasterItemID
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

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
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

        public class GetSummaryProductReviewResult
        {
            public Int64 StoreProductID { get; set; }
            public String MasterItemNumber { get; set; }
            public String ItemNumber { get; set; }
            public String ProductDescription { get; set; }
            public String Vendor { get; set; }
            public Decimal? Level1 { get; set; }
            public Decimal? Level2 { get; set; }
            public Decimal? Level3 { get; set; }
            public Decimal? Level4 { get; set; }
            public Int32? RowNumber { get; set; }
            public Int32? TotalNumber { get; set; }
            public Int64 StoreID { get; set; }
            public String WorkgroupName { get; set; }
            public String StoreName { get; set; }
        }

        public List<GetSummaryProductReviewResult> GetSummaryProductReviewReport(Int64? UserInfoID, Int64? storeID, Int64? workgroupID, Int64? genderID)
        {
            var result = (from storeProduct in db.StoreProducts
                          join pItem in db.ProductItems on storeProduct.StoreProductID equals pItem.ProductId
                          join lookup in db.INC_Lookups on pItem.MasterStyleID equals lookup.iLookupID
                          join piPricing in db.ProductItemPricings on pItem.ProductItemID equals piPricing.ProductItemID
                          join supplier in db.Suppliers on storeProduct.SupplierId equals supplier.SupplierID
                          join supplierUserInfo in db.UserInformations on supplier.UserInfoID equals supplierUserInfo.UserInfoID
                          where storeProduct.StatusID == 135 && pItem.ItemNumberStatusID == 135
                          select new
                          {
                              StoreProductID = storeProduct.StoreProductID,
                              MasterItemNumber = lookup.sLookupName,
                              ItemNumber = pItem.ItemNumber,
                              ProductDescription = storeProduct.ProductDescrption,
                              Vendor = supplierUserInfo.FirstName + " " + supplierUserInfo.LastName,
                              Level1 = piPricing.Level1,
                              Level2 = piPricing.Level2,
                              Level3 = piPricing.Level3,
                              Level4 = piPricing.Level4,
                              WorkgroupID = storeProduct.WorkgroupID,
                              GenderID = storeProduct.GarmentTypeID,
                              StoreID = storeProduct.StoreId
                          }).ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
                if (objReportAccessRight != null)
                {
                    String workgroupIds = objReportAccessRight.WorkgroupID;
                    String storeIds = objReportAccessRight.StoreID;
                    if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(storeIds))
                        return null;

                    Int64[] wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] sIds = storeIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && wIds.Contains(x.WorkgroupID)).ToList();
                }
                else
                    return null;
            }
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            return (from info in result
                    select new GetSummaryProductReviewResult
                    {
                        StoreProductID = info.StoreProductID,
                        MasterItemNumber = info.MasterItemNumber,
                        ItemNumber = info.ItemNumber,
                        ProductDescription = info.ProductDescription,
                        Vendor = info.Vendor.ToString(),
                        Level1 = info.Level1,
                        Level2 = info.Level2,
                        Level3 = info.Level3,
                        Level4 = info.Level4,
                        StoreID = info.StoreID,
                        StoreName = new CompanyStoreRepository().GetBYStoreId((Int32)info.StoreID).FirstOrDefault().CompanyName,
                        WorkgroupName = db.INC_Lookups.Where(x => x.iLookupID == info.WorkgroupID).FirstOrDefault().sLookupName,
                        TotalNumber = result.Where(x => x.MasterItemNumber == info.MasterItemNumber).Count(),
                        RowNumber = result.Where(x => x.MasterItemNumber == info.MasterItemNumber).ToList().IndexOf(info) + 1
                    }).ToList();
        }

        public class GetItemswithBackordersResult
        {
            public String ItemNumber { get; set; }
            public String Description { get; set; }
            public Int32? CurrentStock { get; set; }
            public Int32? OnOrder { get; set; }
            public Int64? BackOrdered { get; set; }
            public DateTime? ExpectedDate { get; set; }
        }

        public List<GetItemswithBackordersResult> GetItemswithBackorders(Int64? UserInfoID, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = db.SelectItemswithBackordersReport().Where(x => x.ItemNumber != null && ((x.Qty - x.ShipQty) > 0 || x.ShipQty == null)).ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupId) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }

            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupId == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            return (from info in result
                    group info by new { info.ItemNumber, info.Description, info.ToArriveOn, info.OnOrder, info.OnHand } into newInfo
                    select new GetItemswithBackordersResult
                    {
                        ItemNumber = newInfo.Key.ItemNumber,
                        Description = newInfo.Key.Description,
                        CurrentStock = newInfo.Key.OnHand,
                        OnOrder = newInfo.Key.OnOrder,
                        BackOrdered = newInfo.Sum(x => x.Qty) - newInfo.Sum(x => x.ShipQty),
                        ExpectedDate = newInfo.Key.ToArriveOn
                    }).Where(x => x.CurrentStock - x.BackOrdered < 0).ToList();
        }

        public List<SelectSalesByInventoryStatusResult> GetProductSalesInventoryStatusWise(Int64? UserInfoID, Int64? storeID, Int64? workgroupID, Int64? GarmentTypeID, Int64 InventoryStatusID)
        {
            var result = db.SelectSalesByInventoryStatus(InventoryStatusID).ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 452);
                if (objReportAccessRight != null)
                {
                    String workgroupIds = objReportAccessRight.WorkgroupID;
                    String storeIds = objReportAccessRight.StoreID;
                    if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(storeIds))
                        return null;

                    Int64[] wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] sIds = storeIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && wIds.Contains(x.WorkgroupID)).ToList();
                }
                else
                    return null;
            }
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (GarmentTypeID != null)
                result = result.Where(r => r.GarmentTypeID == GarmentTypeID).ToList();

            return result;
        }


        #endregion

        #region Anniversary Credits Report
        public class GetAnniversaryCredit
        {
            public String Text { get; set; }
            public Decimal? Value { get; set; }
        }

        public class GetAnniversaryCreditEmployee
        {
            public String EmployeeID { get; set; }
            public String Name { get; set; }
            public DateTime? HirerdDate { get; set; }
            public DateTime? LastIssueDate { get; set; }
            public Decimal? LastIssueAmount { get; set; }
            public Decimal? TotalIssueAmount { get; set; }
            public Decimal? TotalUsedAmount { get; set; }
            public Decimal? CreditAmtToApplied { get; set; }
            public Decimal? CreditAmtToAExpired { get; set; }
            public List<DateTime?> TransactionDate { get; set; }
        }

        public List<GetAnniversaryCreditEmployee> GetAnniversaryCreditEmployeeWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, String ReportType)
        {
            var result = (from ledger in db.EmployeeLedgers
                          join userinfo in db.UserInformations on ledger.UserInfoId equals userinfo.UserInfoID
                          join companyemp in db.CompanyEmployees on userinfo.UserInfoID equals companyemp.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          join anniversaryProgram in db.AnniversaryCreditPrograms on new { StoreId = companystore.StoreID, WorkgroupID = companyemp.WorkgroupID } equals new { StoreId = anniversaryProgram.StoreId, WorkgroupID = anniversaryProgram.WorkgroupID }
                          where userinfo.WLSStatusId == 135 && userinfo.IsDeleted == false
                          select new
                          {
                              StoreID = companystore.StoreID,
                              Name = userinfo.FirstName + " " + userinfo.LastName,
                              TransactionDate = ledger.TransactionDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              CreditAmtToApplied = companyemp.CreditAmtToApplied,
                              EmployeeID = companyemp.EmployeeID,
                              HirerdDate = companyemp.HirerdDate,
                              UserInfoID = userinfo.UserInfoID,
                              TransactionType = ledger.TransactionType,
                              AmountCreditDebit = ledger.AmountCreditDebit,
                              TransactionAmount = ledger.TransactionAmount,
                              Notes = ledger.Notes,
                              EmployeeProgramStatus = companyemp.ProgramActive,
                              GlobalProgramStatus = anniversaryProgram.EmployeeStatus
                          }).ToList();
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 454);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
            if (fromDate != null)
                result = result.Where(r => r.TransactionDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.TransactionDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();
            if (ReportType == "Credits Issued by Month")
                result = result.Where(r => r.Notes == "Issued Anniversary Credit").ToList();
            else
                result = result.Where(x => x.EmployeeProgramStatus == "Active" && x.GlobalProgramStatus == 135).ToList();

            return (from info in result
                    group info by new { info.UserInfoID } into newInfo
                    select new GetAnniversaryCreditEmployee
                    {
                        EmployeeID = newInfo.FirstOrDefault().EmployeeID,
                        Name = newInfo.FirstOrDefault().Name,
                        HirerdDate = newInfo.FirstOrDefault().HirerdDate,
                        LastIssueDate = newInfo.Where(x => x.TransactionType == "AnniversaryCredits" && x.AmountCreditDebit == "Credit").Count() > 0 ? newInfo.Where(x => x.TransactionType == "AnniversaryCredits" && x.AmountCreditDebit == "Credit").OrderByDescending(x => x.TransactionDate).FirstOrDefault().TransactionDate : null,
                        LastIssueAmount = newInfo.Where(x => x.TransactionType == "AnniversaryCredits" && x.AmountCreditDebit == "Credit").Count() > 0 ? newInfo.Where(x => x.TransactionType == "AnniversaryCredits" && x.AmountCreditDebit == "Credit").OrderByDescending(x => x.TransactionDate).FirstOrDefault().TransactionAmount : null,
                        TotalIssueAmount = newInfo.Where(x => x.TransactionType == "AnniversaryCredits" && x.AmountCreditDebit == "Credit").Sum(x => x.TransactionAmount),
                        TotalUsedAmount = newInfo.Where(x => x.TransactionType == "OrderConfirm" && x.AmountCreditDebit == "Debit").Sum(x => x.TransactionAmount),
                        CreditAmtToApplied = newInfo.FirstOrDefault().CreditAmtToApplied,
                        TransactionDate = newInfo.Where(x => x.TransactionType == "AnniversaryCredits" && x.AmountCreditDebit == "Credit").Count() > 0 ? newInfo.Where(x => x.TransactionType == "AnniversaryCredits" && x.AmountCreditDebit == "Credit").Select(x => x.TransactionDate).ToList() : null
                    }).ToList();
        }

        public List<GetAnniversaryCredit> GetAnniversaryCreditWorkgroupWise(Int64? UserInfoID, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from companyemp in db.CompanyEmployees
                          join userinfo in db.UserInformations on companyemp.UserInfoID equals userinfo.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          join anniversaryProgram in db.AnniversaryCreditPrograms on new { StoreId = companystore.StoreID, WorkgroupID = companyemp.WorkgroupID } equals new { StoreId = anniversaryProgram.StoreId, WorkgroupID = anniversaryProgram.WorkgroupID }
                          where userinfo.WLSStatusId == 135 && userinfo.IsDeleted == false && companyemp.ProgramActive == "Active" && companyemp.CreditAmtToApplied != 0 && anniversaryProgram != null && anniversaryProgram.EmployeeStatus == 135
                          select new
                          {
                              StoreID = companystore.StoreID,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              AnniversaryCredit = companyemp.CreditAmtToApplied
                          }).ToList();
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 454);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
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

            var newresult = (from info in result
                             group info by new { info.WorkgroupID } into newInfo
                             select new GetAnniversaryCredit
                             {
                                 Text = db.INC_Lookups.SingleOrDefault(w => w.iLookupID == newInfo.Key.WorkgroupID).sLookupName,
                                 Value = newInfo.Sum(x => x.AnniversaryCredit)
                             });

            return newresult.ToList();
        }

        public List<GetAnniversaryCredit> GetAnniversaryCreditStationWise(Int64? UserInfoID, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from companyemp in db.CompanyEmployees
                          join userinfo in db.UserInformations on companyemp.UserInfoID equals userinfo.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          join anniversaryProgram in db.AnniversaryCreditPrograms on new { StoreId = companystore.StoreID, WorkgroupID = companyemp.WorkgroupID } equals new { StoreId = anniversaryProgram.StoreId, WorkgroupID = anniversaryProgram.WorkgroupID }
                          where userinfo.WLSStatusId == 135 && userinfo.IsDeleted == false && companyemp.BaseStation != null && companyemp.BaseStation != 0 && companyemp.ProgramActive == "Active" && companyemp.CreditAmtToApplied != 0 && anniversaryProgram != null && anniversaryProgram.EmployeeStatus == 135
                          select new
                          {
                              StoreID = companystore.StoreID,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              AnniversaryCredit = companyemp.CreditAmtToApplied
                          }).ToList();
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 454);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
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

            var newresult = (from info in result
                             group info by new { info.BaseStation } into newInfo
                             select new GetAnniversaryCredit
                             {
                                 Text = db.INC_BasedStations.SingleOrDefault(w => w.iBaseStationId == newInfo.Key.BaseStation).sBaseStation,
                                 Value = newInfo.Sum(x => x.AnniversaryCredit)
                             });

            return newresult.ToList();
        }

        public List<GetAnniversaryCredit> GetAnniversaryCreditIssuedMonthWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, String Notes)
        {
            var result = (from ledger in db.EmployeeLedgers
                          join userinfo in db.UserInformations on ledger.UserInfoId equals userinfo.UserInfoID
                          join companyemp in db.CompanyEmployees on userinfo.UserInfoID equals companyemp.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          where userinfo.WLSStatusId == 135 && userinfo.IsDeleted == false && ledger.TransactionType == "AnniversaryCredits" && ledger.AmountCreditDebit == "Credit"
                          select new
                          {
                              StoreID = companystore.StoreID,
                              TransactionDate = ledger.TransactionDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              AnniversaryCredit = ledger.TransactionAmount,
                              Notes = ledger.Notes
                          }).ToList();
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 454);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
            if (fromDate != null)
                result = result.Where(r => r.TransactionDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.TransactionDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();
            if (Notes != null)
                result = result.Where(r => r.Notes == Notes).ToList();

            var newresult = (from info in result
                             group info by new { Convert.ToDateTime(info.TransactionDate).Month } into newInfo
                             select new GetAnniversaryCredit
                             {
                                 Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(newInfo.Key.Month),
                                 Value = newInfo.Sum(x => x.AnniversaryCredit)
                             });

            return newresult.ToList();
        }

        public List<GetAnniversaryCredit> GetTopFiftyEmployeeByAnniversaryCreditWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from companyemp in db.CompanyEmployees
                          join userinfo in db.UserInformations on companyemp.UserInfoID equals userinfo.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          join anniversaryProgram in db.AnniversaryCreditPrograms on new { StoreId = companystore.StoreID, WorkgroupID = companyemp.WorkgroupID } equals new { StoreId = anniversaryProgram.StoreId, WorkgroupID = anniversaryProgram.WorkgroupID }
                          where userinfo.WLSStatusId == 135 && userinfo.IsDeleted == false && companyemp.ProgramActive == "Active" && companyemp.CreditAmtToApplied != 0 && anniversaryProgram != null && anniversaryProgram.EmployeeStatus == 135
                          select new
                          {
                              StoreID = companystore.StoreID,
                              CreatedDate = userinfo.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              AnniversaryCredit = companyemp.CreditAmtToApplied,
                              Name = userinfo.FirstName + " " + userinfo.LastName
                          }).ToList();
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 454);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
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
                             select new GetAnniversaryCredit
                             {
                                 Text = info.Name,
                                 Value = info.AnniversaryCredit
                             }).OrderByDescending(x => x.Value).Take(50);

            return newresult.ToList();
        }

        public List<GetAnniversaryCredit> GetAnniversaryCreditsAtAGlanceWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from ledger in db.EmployeeLedgers
                          join userinfo in db.UserInformations on ledger.UserInfoId equals userinfo.UserInfoID
                          join companyemp in db.CompanyEmployees on userinfo.UserInfoID equals companyemp.UserInfoID
                          join companystore in db.CompanyStores on userinfo.CompanyId equals companystore.CompanyID
                          join anniversaryProgram in db.AnniversaryCreditPrograms on new { StoreId = companystore.StoreID, WorkgroupID = companyemp.WorkgroupID } equals new { StoreId = anniversaryProgram.StoreId, WorkgroupID = anniversaryProgram.WorkgroupID }
                          where userinfo.IsDeleted == false && companyemp.ProgramActive == "Active" && anniversaryProgram != null && anniversaryProgram.EmployeeStatus == 135
                          select new
                          {
                              StoreID = companystore.StoreID,
                              TransactionDate = ledger.TransactionDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStation = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              EmployeeStatus = userinfo.WLSStatusId,
                              EmployeeUpdateDate = userinfo.UpdatedDate,
                              CreditAmtToApplied = companyemp.CreditAmtToApplied,
                              CreditAmtToExpired = companyemp.CreditAmtToExpired,
                              CreditExpiredOn = companyemp.CreditExpireOn,
                              TransactionType = ledger.TransactionType,
                              AmountCreditDebit = ledger.AmountCreditDebit,
                              TransactionAmount = ledger.TransactionAmount
                          }).ToList();
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 454);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
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

            List<GetAnniversaryCredit> objGetAnniversaryCreditList = new List<GetAnniversaryCredit>();

            var TransactionResult = result.Where(x => x.EmployeeStatus == 135).ToList();
            if (fromDate != null)
                TransactionResult = TransactionResult.Where(r => r.TransactionDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                TransactionResult = TransactionResult.Where(r => r.TransactionDate.Value.Date <= toDate).ToList();

            GetAnniversaryCredit objGetAnniversaryCredit = new GetAnniversaryCredit();
            objGetAnniversaryCredit.Text = "Credits Issued";
            objGetAnniversaryCredit.Value = TransactionResult.Where(x => x.TransactionType == "AnniversaryCredits" && x.AmountCreditDebit == "Credit").Sum(x => x.TransactionAmount);
            objGetAnniversaryCreditList.Add(objGetAnniversaryCredit);

            objGetAnniversaryCredit = new GetAnniversaryCredit();
            objGetAnniversaryCredit.Text = "Credits Used";
            objGetAnniversaryCredit.Value = TransactionResult.Where(x => x.TransactionType == "OrderConfirm" && x.AmountCreditDebit == "Debit").Sum(x => x.TransactionAmount);
            objGetAnniversaryCreditList.Add(objGetAnniversaryCredit);

            objGetAnniversaryCredit = new GetAnniversaryCredit();
            objGetAnniversaryCredit.Text = "Credit Balances";
            objGetAnniversaryCredit.Value = result.Where(x => x.EmployeeStatus == 135).Sum(x => x.CreditAmtToApplied);
            objGetAnniversaryCreditList.Add(objGetAnniversaryCredit);

            //This is for getting data that whose credit apply within this period
            var ExpiredCreditResult = result.Where(x => x.EmployeeStatus == 135 && !String.IsNullOrEmpty(x.CreditExpiredOn)).ToList();
            if (fromDate != null)
                ExpiredCreditResult = ExpiredCreditResult.Where(r => Convert.ToDateTime(r.CreditExpiredOn) >= fromDate).ToList();
            if (toDate != null)
                ExpiredCreditResult = ExpiredCreditResult.Where(r => Convert.ToDateTime(r.CreditExpiredOn) <= toDate).ToList();
            objGetAnniversaryCredit = new GetAnniversaryCredit();
            objGetAnniversaryCredit.Text = "Expired Credits";
            objGetAnniversaryCredit.Value = ExpiredCreditResult.Sum(x => x.CreditAmtToExpired);
            objGetAnniversaryCreditList.Add(objGetAnniversaryCredit);

            //This is for getting data that whose deactiavetd during this time period
            var DeactivatedUsersResult = result.Where(x => x.EmployeeStatus == 136).ToList();
            if (fromDate != null)
                DeactivatedUsersResult = DeactivatedUsersResult.Where(r => r.EmployeeUpdateDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                DeactivatedUsersResult = DeactivatedUsersResult.Where(r => r.EmployeeUpdateDate.Value.Date <= toDate).ToList();
            objGetAnniversaryCredit = new GetAnniversaryCredit();
            objGetAnniversaryCredit.Text = "Deactivated Users";
            objGetAnniversaryCredit.Value = DeactivatedUsersResult.Sum(x => x.CreditAmtToApplied);
            objGetAnniversaryCreditList.Add(objGetAnniversaryCredit);

            return objGetAnniversaryCreditList;
        }
        #endregion

        #region Order Management Report
        public class GetOrderAtAGlanceWiseResult
        {
            public String Status { get; set; }
            public Int32 Count { get; set; }
        }

        public List<GetOrderAtAGlanceWiseResult> GetOrderAtAGlanceWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
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
                              OrderStatus = order.OrderStatus
                          }).ToList();

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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
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

        public List<GetEmployeePayrollDeductWiseResult> GetEmployeePayrollDeductWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from order in db.Orders
                          join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
                          join userinfo in db.UserInformations on order.UserId equals userinfo.UserInfoID
                          where order.OrderStatus.ToUpper() != "CANCELED" && order.OrderStatus.ToUpper() != "DELETED" && order.OrderStatus.ToUpper() != "ORDER PENDING" && order.IsPaid == true && order.PaymentOption == 165
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }
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

        public List<SelectOrderDropShipSupplierReportResult> GetOrderDropShipSupplierReport(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = db.SelectOrderDropShipSupplierReport().ToList();

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

            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupId == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();

            return result;
        }

        public List<SelectOrderSummaryViewReportResult> GetOrderSummaryViewReport(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, String OrderNumber, String FirstName, String LastName, String StatusView)
        {
            var result = db.SelectOrderSummaryViewReport().ToList();

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

            if (fromDate != null)
                result = result.Where(r => r.OrderDate.Value.Date >= fromDate).ToList();
            if (toDate != null)
                result = result.Where(r => r.OrderDate.Value.Date <= toDate).ToList();
            if (storeID != null)
                result = result.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                result = result.Where(r => r.WorkgroupId == workgroupID).ToList();
            if (stationID != null)
                result = result.Where(r => r.BaseStation == stationID).ToList();
            if (genderID != null)
                result = result.Where(r => r.GenderID == genderID).ToList();
            if (OrderNumber != null)
                result = result.Where(r => r.OrderNumber == OrderNumber).ToList();
            if (FirstName != null)
                result = result.Where(r => r.FirstName.ToLower().Contains(FirstName.ToLower())).ToList();
            if (LastName != null)
                result = result.Where(r => r.LastName.ToLower().Contains(LastName.ToLower())).ToList();

            if (StatusView != "0")
            {
                if (StatusView == "1")
                    result = result.Where(r => (r.QuantityOrdered - (r.QuantityShipped != null ? r.QuantityShipped : 0)) > 0 && r.Inventory <= 0).ToList();
                if (StatusView == "2")
                    result = result.Where(r => (r.QuantityOrdered - (r.QuantityShipped != null ? r.QuantityShipped : 0)) > 0 && r.Inventory <= 0 && r.BackOrderedUntil == null).ToList();
            }
            return result;
        }

        public List<Company> GetCompanyList()
        {
            var objCompanyList = (from o in db.Orders
                                  where o.StoreID != null
                                  join cs in db.CompanyStores on o.StoreID equals cs.StoreID
                                  join c in db.Companies on cs.CompanyID equals c.CompanyID
                                  select c
                                  ).Distinct().OrderBy(o => o.CompanyName).ToList();

            //if (fromDate != null)
            //    result = result.Where(r => r.o.OrderDate.Value.Date >= fromDate).ToList();
            //if (toDate != null)
            //    result = result.Where(r => r.o.OrderDate.Value.Date <= toDate).ToList();
            //if (storeID != null)
            //    result = result.Where(r => r.o.StoreID == storeID).ToList();
            //if (workgroupID != null)
            //    result = result.Where(r => r.o.WorkgroupId == workgroupID).ToList();
            //if (stationID != null)
            //    result = result.Where(r => r.o.BaseStation == stationID).ToList();
            //if (OrderNumber != null)
            //    result = result.Where(r => r.o.OrderNumber == OrderNumber).ToList();

            //var objCompanyList = (from r in result
            //                     select r.c).ToList();
            return objCompanyList;
        }
        #endregion

        #region Service Level Scorecard Report
        public class GetAverageShipTimeWiseResult
        {
            public String Days { get; set; }
            public Int32 Count { get; set; }
        }

        public List<GetAverageShipTimeWiseResult> GetAverageShipTimeWise(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
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
                    String storeIds = objReportAccessRight.StoreID;
                    if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds) || String.IsNullOrEmpty(storeIds))
                        return null;

                    Int64[] wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] sIds = storeIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
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

        public List<GetPurchasesVSReturnsResult> GetPurchasesVSReturns(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var resultPurchase = (from order in db.SelectProductReportForSize()
                                  join companyemp in db.CompanyEmployees on order.UserId equals companyemp.UserInfoID
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
                                where order.OrderStatus.ToUpper() != "CANCELED" && order.OrderStatus.ToUpper() != "DELETED" && order.OrderStatus.ToUpper() != "ORDER PENDING" && order.OrderFor == "ShoppingCart"
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
                                          where order.OrderStatus.ToUpper() != "CANCELED" && order.OrderStatus.ToUpper() != "DELETED" && order.OrderStatus.ToUpper() != "ORDER PENDING" && order.OrderFor == "IssuanceCart"
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
            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
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
                    resultPurchase = resultPurchase.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                    resultReturn = resultReturn.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
                }
                else
                    return null;
            }

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

        public List<GetHelpTicketsbyLocationResult> GetHelpTicketsbyLocation(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from ticket in db.ServiceTickets
                          join userInfo in db.UserInformations on ticket.ContactID equals userInfo.UserInfoID
                          join companyemp in db.CompanyEmployees on userInfo.UserInfoID equals companyemp.UserInfoID
                          join companyStore in db.CompanyStores on userInfo.CompanyId equals companyStore.CompanyID
                          where companyemp.BaseStation != null && companyemp.BaseStation != 0
                          select new
                          {
                              StoreID = companyStore.StoreID,
                              TicketDate = ticket.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStationID = companyemp.BaseStation,
                              GenderID = companyemp.GenderID
                          }).ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStationID))).ToList();
                }
                else
                    return null;
            }

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

        public List<GetHelpTicketsbyWorkgroupResult> GetHelpTicketsbyWorkgroup(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from ticket in db.ServiceTickets
                          join userInfo in db.UserInformations on ticket.ContactID equals userInfo.UserInfoID
                          join companyemp in db.CompanyEmployees on userInfo.UserInfoID equals companyemp.UserInfoID
                          join companyStore in db.CompanyStores on userInfo.CompanyId equals companyStore.CompanyID
                          where ticket.IsDeleted == false
                          select new
                          {
                              StoreID = companyStore.StoreID,
                              TicketDate = ticket.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStationID = companyemp.BaseStation,
                              GenderID = companyemp.GenderID
                          }).ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStationID))).ToList();
                }
                else
                    return null;
            }

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

        public List<GetHelpTicketsUsersvsTotalEmployeesResult> GetHelpTicketsUsersvsTotalEmployees(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var resultEmployee = (from userInfo in db.UserInformations
                                  join companyemp in db.CompanyEmployees on userInfo.UserInfoID equals companyemp.UserInfoID
                                  join companyStore in db.CompanyStores on userInfo.CompanyId equals companyStore.CompanyID
                                  select new
                                  {
                                      UserInfoID = userInfo.UserInfoID,
                                      StoreID = companyStore.StoreID,
                                      WorkgroupID = companyemp.WorkgroupID,
                                      BaseStationID = companyemp.BaseStation,
                                      GenderID = companyemp.GenderID
                                  }).ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
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
                    resultEmployee = resultEmployee.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStationID))).ToList();
                }
                else
                    return null;
            }

            if (storeID != null)
                resultEmployee = resultEmployee.Where(r => r.StoreID == storeID).ToList();
            if (workgroupID != null)
                resultEmployee = resultEmployee.Where(r => r.WorkgroupID == workgroupID).ToList();
            if (stationID != null)
                resultEmployee = resultEmployee.Where(r => r.BaseStationID == stationID).ToList();
            if (genderID != null)
                resultEmployee = resultEmployee.Where(r => r.GenderID == genderID).ToList();

            var resultTicket = db.ServiceTickets.AsEnumerable().Join(resultEmployee, c => c.ContactID, ci => ci.UserInfoID, (c, ci) => c.ContactID).GroupBy(x => x.Value).ToList();

            List<GetHelpTicketsUsersvsTotalEmployeesResult> objGetHelpTicketsUsersvsTotalEmployeesList = new List<GetHelpTicketsUsersvsTotalEmployeesResult>();
            GetHelpTicketsUsersvsTotalEmployeesResult objGetHelpTicketsUsersvsTotalEmployeesResult = new GetHelpTicketsUsersvsTotalEmployeesResult();

            objGetHelpTicketsUsersvsTotalEmployeesResult.Value = resultEmployee.Count();
            objGetHelpTicketsUsersvsTotalEmployeesResult.Value1 = resultTicket.Count();
            objGetHelpTicketsUsersvsTotalEmployeesList.Add(objGetHelpTicketsUsersvsTotalEmployeesResult);

            return objGetHelpTicketsUsersvsTotalEmployeesList;
        }

        public class GetTop25EmployeesSubmittingTicketsResult
        {
            public String CompanyName { get; set; }
            public String User { get; set; }
            public String Email { get; set; }
            public Int64? TicketCount { get; set; }
        }

        public List<GetTop25EmployeesSubmittingTicketsResult> GetTop25EmployeesSubmittingTickets(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from ticket in db.ServiceTickets
                          join userInfo in db.UserInformations on ticket.ContactID equals userInfo.UserInfoID
                          join companyemp in db.CompanyEmployees on userInfo.UserInfoID equals companyemp.UserInfoID
                          join companyStore in db.CompanyStores on userInfo.CompanyId equals companyStore.CompanyID
                          join company in db.Companies on userInfo.CompanyId equals company.CompanyID
                          select new
                          {
                              UserID = userInfo.UserInfoID,
                              Name = userInfo.FirstName + " " + userInfo.LastName,
                              StoreID = companyStore.StoreID,
                              CompanyName = company.CompanyName,
                              Email = userInfo.LoginEmail,
                              TicketDate = ticket.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStationID = companyemp.BaseStation,
                              GenderID = companyemp.GenderID
                          }).ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStationID))).ToList();
                }
                else
                    return null;
            }

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
                             group info by new { info.UserID } into newInfo
                             select new GetTop25EmployeesSubmittingTicketsResult
                             {
                                 CompanyName = newInfo.FirstOrDefault().CompanyName,
                                 User = newInfo.FirstOrDefault().Name,
                                 Email = newInfo.FirstOrDefault().Email,
                                 TicketCount = newInfo.Count()
                             });

            return newresult.ToList();
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
                    String storeIds = objReportAccessRight.StoreID;
                    if (String.IsNullOrEmpty(workgroupIds) || String.IsNullOrEmpty(baseStationIds) || String.IsNullOrEmpty(storeIds))
                        return null;

                    Int64[] wIds = workgroupIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] bIds = baseStationIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    Int64[] sIds = storeIds.Split(',').Select(x => Int64.Parse(x)).ToArray();
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && x.BaseStation != null && x.BaseStation != 0 && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStation))).ToList();
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
        /// added by prashant 12/12/12
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

        public class GetHelpTicketsbyTypeResult
        {
            public String Type { get; set; }
            public Int64? TicketCount { get; set; }
        }
        public List<GetHelpTicketsbyTypeResult> GetHelpTicketsbyType(Int64? UserInfoID, DateTime? fromDate, DateTime? toDate, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID)
        {
            var result = (from ticket in db.ServiceTickets
                          join userInfo in db.UserInformations on ticket.ContactID equals userInfo.UserInfoID
                          join companyemp in db.CompanyEmployees on userInfo.UserInfoID equals companyemp.UserInfoID
                          join companyStore in db.CompanyStores on userInfo.CompanyId equals companyStore.CompanyID
                          //join lk in db.INC_Lookups on ticket.TypeOfRequestID equals lk.iLookupID
                          where ticket.TypeOfRequestID != null && ticket.TypeOfRequestID != 0 && ticket.IsDeleted == false
                          select new
                          {
                              StoreID = companyStore.StoreID,
                              TicketDate = ticket.CreatedDate,
                              WorkgroupID = companyemp.WorkgroupID,
                              BaseStationID = companyemp.BaseStation,
                              GenderID = companyemp.GenderID,
                              TypeOfRequest = ticket.TypeOfRequestID
                          }).ToList();

            //This is for IE filter
            if (UserInfoID != null)
            {
                ReportAccessRight objReportAccessRight = db.ReportAccessRights.FirstOrDefault(r => r.UserInfoID == UserInfoID && r.ParentReportID == 455);
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
                    result = result.Where(x => sIds.Contains(Convert.ToInt64(x.StoreID)) && wIds.Contains(x.WorkgroupID) && bIds.Contains(Convert.ToInt64(x.BaseStationID))).ToList();
                }
                else
                    return null;
            }

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
            return result;
        }
        #endregion

        public List<SelectBackOrderReportResult> GetBackOrderReport(Int64? UserInfoID, DateTime? FromDate, DateTime? ToDate, Int64? StoreID, Int64? WorkgroupID, Int64? BaseStationID, Int64? GenderID)
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

        public class GetYearVsPreviousYearReportResult
        {
            public Int32 Month { get; set; }
            public Int32 Year { get; set; }
            public Decimal? OrderAmount { get; set; }
            public Decimal? MOASOrderAmount { get; set; }
            public String WorkGroupIDs { get; set; }
            public String BaseStationIDs { get; set; }
            public String PriceLevelIDs { get; set; }
        }
        public class MonthYear
        {
            public Int32 Month { get; set; }
            public String MonthName { get; set; }
            public Int32 Year { get; set; }
        }
        public List<GetYearVsPreviousYearReportResult> GetYearVsPresiousYearReportResult(Int64? UserInfoID, Int64? storeID, Int64? workgroupID, Int64? stationID, Int64? genderID, String OrderStatus, String PriceLevel)
        {
            var result = (from report in db.SelectYearVsPresiousYearReportResult(PriceLevel, UserInfoID, storeID, workgroupID, stationID, genderID, OrderStatus, 451)
                          select new
                          {
                              OrderAmount = report.OrderAmount,
                              Month = report.MonthID,
                              Year = report.YearID,
                              WorkGroupIDs = report.WorkGroupIDs,
                              BaseStationIDs = report.BaseStationIDs
                          }).Select(n => new GetYearVsPreviousYearReportResult
                              {
                                  OrderAmount = n.OrderAmount,
                                  Month = Convert.ToInt32(n.Month),
                                  Year = Convert.ToInt32(n.Year),
                                  WorkGroupIDs = n.WorkGroupIDs,
                                  BaseStationIDs = n.BaseStationIDs
                              }).ToList();
            return result;
        }


        public List<GetDatePeriodValueForSearchResult> GetDatePeriodValues()
        {
            var result = db.GetDatePeriodValueForSearch().ToList();
            return result;
        }
        /// <summary>
        /// Get Details Uniform Issuance Policy
        /// </summary>
        /// <param name="UniformIssuancePolicyID"></param>
        /// <returns></returns>
        public List<GetUniformIssuancePolicyForUsersResult> GetUniformIssuancePolicyForUsers(Int64 UniformIssuancePolicyID)
        {
            return db.GetUniformIssuancePolicyForUsers(UniformIssuancePolicyID).ToList();
        }
        public List<GetPendingOrdersListResult> GetPendingOrderList(DateTime? FromDate, DateTime? ToDate, Int64? ApproverUserInfoID, Int64? StoreID)
        {
            return db.GetPendingOrdersList(FromDate, ToDate, ApproverUserInfoID, StoreID).ToList();
        }
    }
}