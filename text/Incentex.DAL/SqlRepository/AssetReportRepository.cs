using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace Incentex.DAL.SqlRepository
{
    public class AssetReportRepository : RepositoryBase
    {
        public List<EquipmentReportType> GetAllReportType()
        {
            IQueryable<EquipmentReportType> qry = from c in db.EquipmentReportTypes
                                                  orderby c.ReportTypeID
                                                  select c;
            return qry.ToList();
        }
        public EquipmentReportAccessRight GetByVEmpIDAndReportTypeID(Int64 VendorEmployeeID, Int64 ReportTypeID)
        {
            return db.EquipmentReportAccessRights.SingleOrDefault(a => a.VendorEmployeeID == VendorEmployeeID && a.ReportTypeID == ReportTypeID);
        }
        /// <summary>
        /// Get VendorEmployeeID by UserInfoID
        /// </summary>
        /// <param name="UserInfoID"></param>
        /// <returns></returns>
        public Int64? GetVEIDbyUID(Int64 UserInfoID)
        {
            AssetVendorRepository objVendorRepo = new AssetVendorRepository();
            EquipmentVendorEmployee objVendorEmployee = objVendorRepo.GetVendorEmpByUserInfoID(UserInfoID);
            Int64? VendorEmpID = null;
            if (objVendorEmployee != null)
            {
                return VendorEmpID = Convert.ToInt64(objVendorEmployee.VendorEmployeeID);
            }
            else
            {
                return null;
            }
        }
        //Equipment Status Report
        public class GetEquipmentStatusResult
        {
            public string Text { get; set; }
            public Int64? Value { get; set; }
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="companyID"></param>
        /// <param name="equipmenttypeID"></param>
        /// <param name="stationID"></param>
        /// <param name="UserInfoID"></param>
        /// <returns></returns>
        public List<GetEquipmentStatusResult> GetEquipmentStatusVE(DateTime? fromDate, DateTime? toDate, Int64? companyID, Int64? equipmenttypeID, Int64? stationID, Int64 UserInfoID)
        {
            Int64? VendorEmpID = GetVEIDbyUID(UserInfoID);  //Get VendorEmpID               

            string equiTypeIds = db.EquipmentReportAccessRights.FirstOrDefault(r => r.VendorEmployeeID == VendorEmpID && r.ReportTypeID == 1).EquipmentTypeID;
            string baseStationIds = db.EquipmentReportAccessRights.FirstOrDefault(r => r.VendorEmployeeID == VendorEmpID && r.ReportTypeID == 1).BaseStationID;
            if (string.IsNullOrEmpty(equiTypeIds) || string.IsNullOrEmpty(baseStationIds))
                return null;

            long[] eIds = equiTypeIds.Split(',').Select(x => long.Parse(x)).ToArray();
            long[] bIds = baseStationIds.Split(',').Select(x => long.Parse(x)).ToArray();

            var result = (from EM in db.EquipmentMasters
                          join IL in db.INC_Lookups on EM.Status equals IL.iLookupID
                          where eIds.Contains(Convert.ToInt64(EM.EquipmentTypeID)) &&
                          bIds.Contains(Convert.ToInt64(EM.BaseStationID)) &&
                          (companyID != 0 ? EM.CompanyID == companyID : true) &&
                          (fromDate != null ? EM.CreatedDate >= fromDate : true) &&
                          (toDate != null ? EM.CreatedDate <= toDate : true) &&
                          (stationID != 0 ? EM.BaseStationID == stationID : true) &&
                          (equipmenttypeID != 0 ? EM.EquipmentTypeID == equipmenttypeID : true)
                          select new
                          {
                              CompanyID = EM.CompanyID,
                              EquipmentStatus = IL.iLookupID,
                              CreatedDate = EM.CreatedDate,
                              BaseStation = EM.BaseStationID,
                              EquipmentType = EM.EquipmentTypeID
                          }).ToList();

            //if (companyID != 0)
            //    result = result.Where(r => r.CompanyID == companyID).ToList();
            //if (fromDate != null)
            //    result = result.Where(r => r.CreatedDate >= fromDate).ToList();
            //if (toDate != null)
            //    result = result.Where(r => r.CreatedDate <= toDate).ToList();
            //if (stationID != 0)
            //    result = result.Where(r => r.BaseStation == stationID).ToList();
            //if (equipmenttypeID != 0)
            //    result = result.Where(r => r.EquipmentType == equipmenttypeID).ToList();

            var newresult = (from info in result
                             group info by new { info.EquipmentStatus } into newInfo
                             select new GetEquipmentStatusResult
                             {
                                 Text = db.INC_Lookups.SingleOrDefault(x => x.iLookupID == newInfo.Key.EquipmentStatus).sLookupName,
                                 Value = newInfo.Count()
                             });

            return newresult.ToList();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="companyID"></param>
        /// <param name="equipmenttypeID"></param>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public List<GetEquipmentStatusResult> GetEquipmentStatus(DateTime? fromDate, DateTime? toDate, Int64? companyID, Int64? equipmenttypeID, Int64? stationID)
        {
            var result = (from EM in db.EquipmentMasters
                          join IL in db.INC_Lookups on EM.Status equals IL.iLookupID
                          where (companyID != 0 ? EM.CompanyID == companyID : true) &&
                                (fromDate != null ? EM.CreatedDate >= fromDate : true) &&
                                (toDate != null ? EM.CreatedDate <= toDate : true) &&
                                (stationID != 0 ? EM.BaseStationID == stationID : true) &&
                                (equipmenttypeID != 0 ? EM.EquipmentTypeID == equipmenttypeID : true)
                          select new
                          {
                              CompanyID = EM.CompanyID,
                              EquipmentStatus = IL.iLookupID,
                              CreatedDate = EM.CreatedDate,
                              BaseStation = EM.BaseStationID,
                              EquipmentType = EM.EquipmentTypeID
                          }).ToList();

            //if (companyID != 0)
            //    result = result.Where(r => r.CompanyID == companyID).ToList();
            //if (fromDate != null)
            //    result = result.Where(r => r.CreatedDate >= fromDate).ToList();
            //if (toDate != null)
            //    result = result.Where(r => r.CreatedDate <= toDate).ToList();
            //if (stationID != 0)
            //    result = result.Where(r => r.BaseStation == stationID).ToList();
            //if (equipmenttypeID != 0)
            //    result = result.Where(r => r.EquipmentType == equipmenttypeID).ToList();


            var newresult = (from info in result
                             group info by new { info.EquipmentStatus } into newInfo
                             select new GetEquipmentStatusResult
                             {
                                 Text = db.INC_Lookups.SingleOrDefault(x => x.iLookupID == newInfo.Key.EquipmentStatus).sLookupName,
                                 Value = newInfo.Count()
                             });

            return newresult.ToList();
        }
        public List<INC_Lookup> GetEquiTypebyUserInfoID(Int64 UserInfoID, Int64 ReportTypeID)
        {
            //Get Vendor EmployeeID
            AssetVendorRepository objVendorRepo = new AssetVendorRepository();
            EquipmentVendorEmployee objVendorEmployee = objVendorRepo.GetVendorEmpByUserInfoID(UserInfoID);
            Int64? VendorEmpID = null;
            if (objVendorEmployee != null)
            {
                VendorEmpID = Convert.ToInt64(objVendorEmployee.VendorEmployeeID);
            }

            string EquiTypeList = db.EquipmentReportAccessRights.FirstOrDefault(r => r.VendorEmployeeID == VendorEmpID && r.ReportTypeID == ReportTypeID).EquipmentTypeID;
            if (EquiTypeList != null)
                return db.INC_Lookups.Where(look => EquiTypeList.Split(',').Contains(look.iLookupID.ToString())).ToList();
            else
                return null;
        }
        public List<INC_BasedStation> GetBaseStationbyUserInfoID(Int64 UserInfoID, Int64 ReportTypeID)
        {
            //Get VendorEmployeeID
            AssetVendorRepository objVendorRepo = new AssetVendorRepository();
            EquipmentVendorEmployee objVendorEmployee = objVendorRepo.GetVendorEmpByUserInfoID(UserInfoID);
            Int64? VendorEmpID = null;
            if (objVendorEmployee != null)
            {
                VendorEmpID = Convert.ToInt64(objVendorEmployee.VendorEmployeeID);
            }

            string BaseStationList = db.EquipmentReportAccessRights.FirstOrDefault(r => r.VendorEmployeeID == VendorEmpID && r.ReportTypeID == ReportTypeID).BaseStationID;
            if (BaseStationList != null)
                return db.INC_BasedStations.Where(b => BaseStationList.Split(',').Contains(b.iBaseStationId.ToString())).ToList();
            else
                return null;
        }
        public List<GetEquipmentsResult> GetEquipmentsDetail(string EquipmentTypeID, string EquipmentID, string BaseStationID, string Status, string EquipmentMasterID, string Flagged, string GSEDepartmentID, Int64 CompanyID, Int64 VEUserInfoID)
        {
            //Get VendorEmployeeID
            AssetVendorRepository objVendorRepo = new AssetVendorRepository();
            EquipmentVendorEmployee objVendorEmployee = objVendorRepo.GetVendorEmpByUserInfoID(VEUserInfoID);
            Int64? VendorEmpID = null;
            if (objVendorEmployee != null)
            {
                VendorEmpID = Convert.ToInt64(objVendorEmployee.VendorEmployeeID);
            }
            //Get EquipmentTypeIDs & BaseStationIDs
            string equiTypeIds = db.EquipmentReportAccessRights.FirstOrDefault(r => r.VendorEmployeeID == VendorEmpID && r.ReportTypeID == 1).EquipmentTypeID;
            string baseStationIds = db.EquipmentReportAccessRights.FirstOrDefault(r => r.VendorEmployeeID == VendorEmpID && r.ReportTypeID == 1).BaseStationID;
            if (string.IsNullOrEmpty(equiTypeIds) || string.IsNullOrEmpty(baseStationIds))
                return null;
            long[] eIds = equiTypeIds.Split(',').Select(x => long.Parse(x)).ToArray();
            long[] bIds = baseStationIds.Split(',').Select(x => long.Parse(x)).ToArray();

            var objlist = (db.GetEquipments(EquipmentTypeID, EquipmentID, BaseStationID, Status, EquipmentMasterID, Flagged, GSEDepartmentID, 0).Where(x => eIds.Contains(Convert.ToInt64(x.EquipmentTypeID)) && bIds.Contains(Convert.ToInt64(x.BaseStationID)))).ToList();
            //var objlist = (db.GetEquipments(EquipmentTypeID, EquipmentID, BaseStationID, Status, EquipmentMasterID, Flagged, GSEDepartmentID, VEUserInfoID).ToList());
            if (CompanyID != 0)
            {
                objlist = objlist.Where(q => q.CompanyID == CompanyID).ToList();
            }
            return objlist.ToList<GetEquipmentsResult>();
        }
        public Int64 GetEquiStatusIDByName(string EquipmentStatusName)
        {
            try
            {
                INC_Lookup objLookup = new INC_Lookup();
                Int64 ESID = db.INC_Lookups.Where(e => e.iLookupCode == "EquipmentStatus" && e.sLookupName == EquipmentStatusName).FirstOrDefault().iLookupID;
                return ESID;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        //Equipment Spend by Asset Result Report
        public class GetSpendbyAssetResult
        {
            public string Text { get; set; }
            public decimal? Value { get; set; }
        }


        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="companyID"></param>
        /// <param name="equipmenttypeID"></param>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public List<GetSpendbyAssetResult> GetSpendbyAsset(DateTime? fromDate, DateTime? toDate, Int64? companyID, Int64? equipmenttypeID, Int64? stationID)
        {
            var result = (from EMCD in db.EquipmentMaintenanceCostDetails
                          join EM in db.EquipmentMasters on EMCD.EquipmentMasterID equals EM.EquipmentMasterID
                          where (companyID != 0 ? EM.CompanyID == companyID : true) &&
                                (fromDate != null ? EMCD.DateofService >= fromDate : true) &&
                                (toDate != null ? EMCD.DateofService <= toDate : true) &&
                                (stationID != 0 ? EM.BaseStationID == stationID : true) &&
                                (equipmenttypeID != 0 ? EM.EquipmentTypeID == equipmenttypeID : true)
                          select new
                          {
                              CompanyID = EM.CompanyID,
                              EquipmentMasterID = EM.EquipmentMasterID,
                              Amount = EMCD.Amount,
                              DateOfService = EMCD.DateofService,
                              BaseStation = EM.BaseStationID,
                              EquipmentType = EM.EquipmentTypeID
                          }).ToList();

            //if (companyID != 0)
            //    result = result.Where(r => r.CompanyID == companyID).ToList();
            //if (fromDate != null)
            //    result = result.Where(r => r.DateOfService >= fromDate).ToList();
            //if (toDate != null)
            //    result = result.Where(r => r.DateOfService <= toDate).ToList();
            //if (stationID != 0)
            //    result = result.Where(r => r.BaseStation == stationID).ToList();
            //if (equipmenttypeID != 0)
            //    result = result.Where(r => r.EquipmentType == equipmenttypeID).ToList();

            var newresult = (from info in result
                             group info by new { info.EquipmentType } into newInfo
                             select new GetSpendbyAssetResult
                             {
                                 Text = db.INC_Lookups.SingleOrDefault(x => x.iLookupID == newInfo.Key.EquipmentType).sLookupName,
                                 Value = newInfo.Sum(x => x.Amount != null ? x.Amount : 0)
                             });

            return newresult.ToList();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="companyID"></param>
        /// <param name="equipmenttypeID"></param>
        /// <param name="stationID"></param>
        /// <param name="UserInfoID"></param>
        /// <returns></returns>
        public List<GetSpendbyAssetResult> GetSpendbyAssetVE(DateTime? fromDate, DateTime? toDate, Int64? companyID, Int64? equipmenttypeID, Int64? stationID, Int64 UserInfoID)
        {
            Int64? VendorEmpID = GetVEIDbyUID(UserInfoID);  //Get VendorEmpID    

            string equiTypeIds = db.EquipmentReportAccessRights.FirstOrDefault(r => r.VendorEmployeeID == VendorEmpID && r.ReportTypeID == 1).EquipmentTypeID;
            string baseStationIds = db.EquipmentReportAccessRights.FirstOrDefault(r => r.VendorEmployeeID == VendorEmpID && r.ReportTypeID == 1).BaseStationID;
            if (string.IsNullOrEmpty(equiTypeIds) || string.IsNullOrEmpty(baseStationIds))
                return null;

            long[] eIds = equiTypeIds.Split(',').Select(x => long.Parse(x)).ToArray();
            long[] bIds = baseStationIds.Split(',').Select(x => long.Parse(x)).ToArray();

            var result = (from EMCD in db.EquipmentMaintenanceCostDetails
                          join EM in db.EquipmentMasters on EMCD.EquipmentMasterID equals EM.EquipmentMasterID
                          where eIds.Contains(Convert.ToInt64(EM.EquipmentTypeID)) && bIds.Contains(Convert.ToInt64(EM.BaseStationID)) &&
                          (companyID != 0 ? EM.CompanyID == companyID : true) &&
                          (fromDate != null ? EMCD.DateofService >= fromDate : true) &&
                          (toDate != null ? EMCD.DateofService <= toDate : true) &&
                          (stationID != 0 ? EM.BaseStationID == stationID : true) &&
                          (equipmenttypeID != 0 ? EM.EquipmentTypeID == equipmenttypeID : true)
                          select new
                          {
                              CompanyID = EM.CompanyID,
                              EquipmentMasterID = EM.EquipmentMasterID,
                              Amount = EMCD.Amount,
                              DateOfService = EMCD.DateofService,
                              BaseStation = EM.BaseStationID,
                              EquipmentType = EM.EquipmentTypeID
                          }).ToList();

            //if (companyID != 0)
            //    result = result.Where(r => r.CompanyID == companyID).ToList();
            //if (fromDate != null)
            //    result = result.Where(r => r.DateOfService >= fromDate).ToList();
            //if (toDate != null)
            //    result = result.Where(r => r.DateOfService <= toDate).ToList();
            //if (stationID != 0)
            //    result = result.Where(r => r.BaseStation == stationID).ToList();
            //if (equipmenttypeID != 0)
            //    result = result.Where(r => r.EquipmentType == equipmenttypeID).ToList();

            var newresult = (from info in result
                             group info by new { info.EquipmentType } into newInfo
                             select new GetSpendbyAssetResult
                             {
                                 Text = db.INC_Lookups.SingleOrDefault(x => x.iLookupID == newInfo.Key.EquipmentType).sLookupName,
                                 Value = newInfo.Sum(x => x.Amount != null ? x.Amount : 0)
                             });

            return newresult.ToList();
        }
        public class GetAssetDetailResult
        {
            public Int64 EquipmentMasterID { get; set; }
            public String EquipmentID { get; set; }
            public String EquiType { get; set; }
            public String BaseStation { get; set; }
            public Decimal? Spend { get; set; }
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="companyID"></param>
        /// <param name="equipmenttypeID"></param>
        /// <param name="stationID"></param>
        /// <param name="UserInfoID"></param>
        /// <returns></returns>
        public List<GetAssetDetailResult> GetAssetSpendDetailVE(DateTime? fromDate, DateTime? toDate, Int64? companyID, Int64? equipmenttypeID, Int64? stationID, Int64 UserInfoID)
        {
            Int64? VendorEmpID = GetVEIDbyUID(UserInfoID);  //Get VendorEmpID    

            string equiTypeIds = db.EquipmentReportAccessRights.FirstOrDefault(r => r.VendorEmployeeID == VendorEmpID && r.ReportTypeID == 1).EquipmentTypeID;
            string baseStationIds = db.EquipmentReportAccessRights.FirstOrDefault(r => r.VendorEmployeeID == VendorEmpID && r.ReportTypeID == 1).BaseStationID;
            if (string.IsNullOrEmpty(equiTypeIds) || string.IsNullOrEmpty(baseStationIds))
                return null;

            long[] eIds = equiTypeIds.Split(',').Select(x => long.Parse(x)).ToArray();
            long[] bIds = baseStationIds.Split(',').Select(x => long.Parse(x)).ToArray();

            var result = (from EM in db.EquipmentMasters
                          join IL1 in db.INC_Lookups on EM.EquipmentTypeID equals IL1.iLookupID
                          join IL2 in db.INC_Lookups on EM.BaseStationID equals IL2.iLookupID
                          join EMCD in db.EquipmentMaintenanceCostDetails on EM.EquipmentMasterID equals EMCD.EquipmentMasterID
                          where eIds.Contains(Convert.ToInt64(EM.EquipmentTypeID)) &&
                                bIds.Contains(Convert.ToInt64(EM.BaseStationID)) &&
                                (companyID != 0 ? EM.CompanyID == companyID : true) &&
                                (fromDate != null ? EMCD.DateofService >= fromDate : true) &&
                                (toDate != null ? EMCD.DateofService <= toDate : true) &&
                                (stationID != 0 ? EM.BaseStationID == stationID : true) &&
                                (equipmenttypeID != 0 ? EM.EquipmentTypeID == equipmenttypeID : true)
                          select new
                          {
                              CompanyID = EM.CompanyID,
                              EquipmentMasterID = EM.EquipmentMasterID,
                              Amount = EMCD.Amount,
                              DateOfService = EMCD.DateofService,
                              BaseStationName = IL2.sLookupName,
                              EquiType = IL1.sLookupName,
                              EquipmentID = EM.EquipmentID,
                              BaseStation = EM.BaseStationID,
                              EquipmentType = EM.EquipmentTypeID
                          }).ToList();

            //if (companyID != 0)
            //    result = result.Where(r => r.CompanyID == companyID).ToList();
            //if (fromDate != null)
            //    result = result.Where(r => r.DateOfService >= fromDate).ToList();
            //if (toDate != null)
            //    result = result.Where(r => r.DateOfService <= toDate).ToList();
            //if (stationID != 0)
            //    result = result.Where(r => r.BaseStation == stationID).ToList();
            //if (equipmenttypeID != 0)
            //    result = result.Where(r => r.EquipmentType == equipmenttypeID).ToList();

            var newresult = (from info in result
                             select new GetAssetDetailResult
                             {
                                 EquipmentMasterID = info.EquipmentMasterID,
                                 EquipmentID = info.EquipmentID,
                                 EquiType = info.EquiType,
                                 BaseStation = info.BaseStationName,
                                 Spend = info.Amount
                             });

            return newresult.ToList();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="companyID"></param>
        /// <param name="equipmenttypeID"></param>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public List<GetAssetDetailResult> GetAssetSpendDetail(DateTime? fromDate, DateTime? toDate, Int64? companyID, Int64? equipmenttypeID, Int64? stationID)
        {
            var result = (from EM in db.EquipmentMasters
                          join IL1 in db.INC_Lookups on EM.EquipmentTypeID equals IL1.iLookupID
                          join IL2 in db.INC_Lookups on EM.BaseStationID equals IL2.iLookupID
                          join EMCD in db.EquipmentMaintenanceCostDetails on EM.EquipmentMasterID equals EMCD.EquipmentMasterID
                          where (companyID != 0 ? EM.CompanyID == companyID : true) &&
                                (fromDate != null ? EMCD.DateofService >= fromDate : true) &&
                                (toDate != null ? EMCD.DateofService <= toDate : true) &&
                                (stationID != 0 ? EM.BaseStationID == stationID : true) &&
                                (equipmenttypeID != 0 ? EM.EquipmentTypeID == equipmenttypeID : true)
                          select new
                          {
                              CompanyID = EM.CompanyID,
                              EquipmentMasterID = EM.EquipmentMasterID,
                              Amount = EMCD.Amount,
                              DateOfService = EMCD.DateofService,
                              BaseStationName = IL2.sLookupName,
                              EquiType = IL1.sLookupName,
                              EquipmentID = EM.EquipmentID,
                              BaseStation = EM.BaseStationID,
                              EquipmentType = EM.EquipmentTypeID
                          }).ToList();


            //if (companyID != 0)
            //    result = result.Where(r => r.CompanyID == companyID).ToList();
            //if (fromDate != null)
            //    result = result.Where(r => r.DateOfService >= fromDate).ToList();
            //if (toDate != null)
            //    result = result.Where(r => r.DateOfService <= toDate).ToList();
            //if (stationID != 0)
            //    result = result.Where(r => r.BaseStation == stationID).ToList();
            //if (equipmenttypeID != 0)
            //    result = result.Where(r => r.EquipmentType == equipmenttypeID).ToList();

            var newresult = (from info in result
                             select new GetAssetDetailResult
                             {
                                 EquipmentMasterID = info.EquipmentMasterID,
                                 EquipmentID = info.EquipmentID,
                                 EquiType = info.EquiType,
                                 BaseStation = info.BaseStationName,
                                 Spend = info.Amount
                             });

            return newresult.ToList();
        }
        public Int64 GetEquiTypeIDByName(string EquipmentTypeName)
        {
            try
            {
                INC_Lookup objLookup = new INC_Lookup();
                Int64 ETID = db.INC_Lookups.Where(e => e.iLookupCode == "EquipmentType" && e.sLookupName == EquipmentTypeName).FirstOrDefault().iLookupID;
                return ETID;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        //Equipment Parts Purchase Report
        public class PartsPurchaseResult
        {
            public string Text { get; set; }
            public decimal? Value { get; set; }
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="companyID"></param>
        /// <param name="equipmenttypeID"></param>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public List<PartsPurchaseResult> PartsPurchase(DateTime? fromDate, DateTime? toDate, Int64? companyID, Int64? equipmenttypeID, Int64? stationID)
        {
            //Get Job Sub Code ID According to purchase of parts
            var JSCodeQry = from a in db.EquipmentJobCodeLookups
                            where SqlMethods.Like(a.JobCodeName, "%" + "purchase of part" + "%")
                            select a.JobCodeID.ToString();
            string[] JSCode = JSCodeQry.ToArray();

            var result = (from EMCD in db.EquipmentMaintenanceCostDetails
                          join EM in db.EquipmentMasters on EMCD.EquipmentMasterID equals EM.EquipmentMasterID
                          join IL in db.INC_Lookups on EM.EquipmentTypeID equals IL.iLookupID
                          where JSCode.Contains(EMCD.JobSubCode) &&
                                (companyID != 0 ? EM.CompanyID == companyID : true) &&
                                (fromDate != null ? EMCD.DateofService >= fromDate : true) &&
                                (toDate != null ? EMCD.DateofService <= toDate : true) &&
                                (stationID != 0 ? EM.BaseStationID == stationID : true) &&
                                (equipmenttypeID != 0 ? EM.EquipmentTypeID == equipmenttypeID : true)
                          select new
                          {
                              CompanyID = EM.CompanyID,
                              EquipmentMasterID = EM.EquipmentMasterID,
                              Amount = EMCD.Amount,
                              DateOfService = EMCD.DateofService,
                              BaseStation = EM.BaseStationID,
                              EquipmentType = EM.EquipmentTypeID
                          }).ToList();

            //if (companyID != 0)
            //    result = result.Where(r => r.CompanyID == companyID).ToList();
            //if (fromDate != null)
            //    result = result.Where(r => r.DateOfService >= fromDate).ToList();
            //if (toDate != null)
            //    result = result.Where(r => r.DateOfService <= toDate).ToList();
            //if (stationID != 0)
            //    result = result.Where(r => r.BaseStation == stationID).ToList();
            //if (equipmenttypeID != 0)
            //    result = result.Where(r => r.EquipmentType == equipmenttypeID).ToList();

            var newresult = (from info in result
                             group info by new { info.EquipmentType } into newInfo
                             select new PartsPurchaseResult
                             {
                                 Text = db.INC_Lookups.SingleOrDefault(x => x.iLookupID == newInfo.Key.EquipmentType).sLookupName,
                                 Value = newInfo.Sum(x => x.Amount != null ? x.Amount : 0)
                             });

            return newresult.ToList();
        }

        public List<PartsPurchaseResult> PartsPurchaseVE(DateTime? fromDate, DateTime? toDate, Int64? companyID, Int64? equipmenttypeID, Int64? stationID, Int64 UserInfoID)
        {
            Int64? VendorEmpID = GetVEIDbyUID(UserInfoID);  //Get VendorEmpID    

            //Get Job Sub Code ID According to purchase of parts
            var JSCodeQry = from a in db.EquipmentJobCodeLookups
                            where SqlMethods.Like(a.JobCodeName, "%" + "purchase of part" + "%")
                            select a.JobCodeID.ToString();
            string[] JSCode = JSCodeQry.ToArray();


            string equiTypeIds = db.EquipmentReportAccessRights.FirstOrDefault(r => r.VendorEmployeeID == VendorEmpID && r.ReportTypeID == 1).EquipmentTypeID;
            string baseStationIds = db.EquipmentReportAccessRights.FirstOrDefault(r => r.VendorEmployeeID == VendorEmpID && r.ReportTypeID == 1).BaseStationID;
            if (string.IsNullOrEmpty(equiTypeIds) || string.IsNullOrEmpty(baseStationIds))
                return null;

            long[] eIds = equiTypeIds.Split(',').Select(x => long.Parse(x)).ToArray();
            long[] bIds = baseStationIds.Split(',').Select(x => long.Parse(x)).ToArray();

            var result = (from EMCD in db.EquipmentMaintenanceCostDetails
                          join EM in db.EquipmentMasters on EMCD.EquipmentMasterID equals EM.EquipmentMasterID
                          join IL in db.INC_Lookups on EM.EquipmentTypeID equals IL.iLookupID
                          join EJCL in db.EquipmentJobCodeLookups on EMCD.JobCode equals EJCL.JobCodeID
                          where eIds.Contains(Convert.ToInt64(EM.EquipmentTypeID)) &&
                                bIds.Contains(Convert.ToInt64(EM.BaseStationID)) &&
                                JSCode.Contains(EMCD.JobSubCode) &&
                                (companyID != 0 ? EM.CompanyID == companyID : true) &&
                                (fromDate != null ? EMCD.DateofService >= fromDate : true) &&
                                (toDate != null ? EMCD.DateofService <= toDate : true) &&
                                (stationID != 0 ? EM.BaseStationID == stationID : true) &&
                                (equipmenttypeID != 0 ? EM.EquipmentTypeID == equipmenttypeID : true)
                          select new
                          {
                              CompanyID = EM.CompanyID,
                              EquipmentMasterID = EM.EquipmentMasterID,
                              Amount = EMCD.Amount,
                              DateOfService = EMCD.DateofService,
                              BaseStation = EM.BaseStationID,
                              EquipmentType = EM.EquipmentTypeID,
                          }).ToList();

            //if (companyID != 0)
            //    result = result.Where(r => r.CompanyID == companyID).ToList();
            //if (fromDate != null)
            //    result = result.Where(r => r.DateOfService >= fromDate).ToList();
            //if (toDate != null)
            //    result = result.Where(r => r.DateOfService <= toDate).ToList();
            //if (stationID != 0)
            //    result = result.Where(r => r.BaseStation == stationID).ToList();
            //if (equipmenttypeID != 0)
            //    result = result.Where(r => r.EquipmentType == equipmenttypeID).ToList();

            var newresult = (from info in result
                             group info by new { info.EquipmentType } into newInfo
                             select new PartsPurchaseResult
                             {
                                 Text = db.INC_Lookups.SingleOrDefault(x => x.iLookupID == newInfo.Key.EquipmentType).sLookupName,
                                 Value = newInfo.Sum(x => x.Amount != null ? x.Amount : 0)
                             });

            return newresult.ToList();
        }

        /// <summary>
        /// Get Job Code List Result
        /// </summary>
        public class GetJobCodelResult
        {
            public Int64 JobCodeID { get; set; }
            public string JobCodeName { get; set; }
            public decimal? Amount { get; set; }
        }

        /// <summary>
        /// Get JobCode List for Part Purchased Page
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="companyID"></param>
        /// <param name="equipmenttypeID"></param>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public List<GetJobCodelResult> GetJobCodeList(DateTime? fromDate, DateTime? toDate, Int64? companyID, Int64? equipmenttypeID, Int64? stationID)
        {
            var JSCodeQry = from a in db.EquipmentJobCodeLookups
                            where SqlMethods.Like(a.JobCodeName, "%" + "purchase of part" + "%")
                            select a.JobCodeID.ToString();
            string[] JSCode = JSCodeQry.ToArray();

            var result = (from EMCD in db.EquipmentMaintenanceCostDetails
                          join EM in db.EquipmentMasters on EMCD.EquipmentMasterID equals EM.EquipmentMasterID
                          join IL in db.INC_Lookups on EM.EquipmentTypeID equals IL.iLookupID
                          join JL in db.EquipmentJobCodeLookups on EMCD.JobCode equals JL.JobCodeID
                          where JSCode.Contains(EMCD.JobSubCode) && EMCD.JobCode != null &&
                                (companyID != 0 ? EM.CompanyID == companyID : true) &&
                                (fromDate != null ? EMCD.DateofService >= fromDate : true) &&
                                (toDate != null ? EMCD.DateofService <= toDate : true) &&
                                (stationID != 0 ? EM.BaseStationID == stationID : true) &&
                                (equipmenttypeID != 0 ? EM.EquipmentTypeID == equipmenttypeID : true) 
                          select new
                          {
                              CompanyID = EM.CompanyID,
                              DateOfService = EMCD.DateofService,
                              BaseStation = EM.BaseStationID,
                              EquipmentType = EM.EquipmentTypeID,
                              JobCode = JL.JobCodeID,
                              JobCodeName = JL.JobCodeName,
                              Amount = EMCD.Amount
                          }).ToList();

            //if (companyID != 0)
            //    result = result.Where(r => r.CompanyID == companyID).ToList();
            //if (fromDate != null)
            //    result = result.Where(r => r.DateOfService >= fromDate).ToList();
            //if (toDate != null)
            //    result = result.Where(r => r.DateOfService <= toDate).ToList();
            //if (stationID != 0)
            //    result = result.Where(r => r.BaseStation == stationID).ToList();
            //if (equipmenttypeID != 0)
            //    result = result.Where(r => r.EquipmentType == equipmenttypeID).ToList();

            var newresult = (from info in result
                             group info by new { info.JobCode } into newInfo
                             select new GetJobCodelResult
                             {
                                 JobCodeID = newInfo.Key.JobCode,
                                 JobCodeName = db.EquipmentJobCodeLookups.SingleOrDefault(x => x.JobCodeID == newInfo.Key.JobCode).JobCodeName,
                                 Amount = newInfo.Sum(x => x.Amount != null ? x.Amount : 0)
                             }).Distinct().ToList();



            return newresult.ToList<GetJobCodelResult>();
        }

        public class PartsPurchaseDetailResult
        {
            public string Vendor { get; set; }
            public DateTime? DateofService { get; set; }
            public string InvoiceNumber { get; set; }
            public decimal? Amount { get; set; }
            public string File { get; set; }
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="companyID"></param>
        /// <param name="equipmenttypeID"></param>
        /// <param name="stationID"></param>
        /// <param name="jobcodeID"></param>
        /// <returns></returns>
        public List<PartsPurchaseDetailResult> GetPartsPurchaseDetail(DateTime? fromDate, DateTime? toDate, Int64? companyID, Int64? equipmenttypeID, Int64? stationID, Int64 jobcodeID)
        {
            var JSCodeQry = from a in db.EquipmentJobCodeLookups
                            where SqlMethods.Like(a.JobCodeName, "%" + "purchase of part" + "%")
                            select a.JobCodeID.ToString();
            string[] JSCode = JSCodeQry.ToArray();

            var result = (from EMCD in db.EquipmentMaintenanceCostDetails
                          join EM in db.EquipmentMasters on EMCD.EquipmentMasterID equals EM.EquipmentMasterID
                          join VM in db.EquipmentVendorMasters on EMCD.Vendor equals VM.EquipmentVendorID
                          where JSCode.Contains(EMCD.JobSubCode) && EMCD.JobCode != null &&
                                EMCD.JobCode == jobcodeID &&
                                (companyID != 0 ? EM.CompanyID == companyID : true) &&
                                (fromDate != null ? EMCD.DateofService >= fromDate : true) &&
                                (toDate != null ? EMCD.DateofService <= toDate : true) &&
                                (stationID != 0 ? EM.BaseStationID == stationID : true) &&
                                (equipmenttypeID != 0 ? EM.EquipmentTypeID == equipmenttypeID : true)
                          select new
                          {
                              CompanyID = EM.CompanyID,
                              DateOfService = EMCD.DateofService,
                              BaseStation = EM.BaseStationID,
                              EquipmentType = EM.EquipmentTypeID,
                              JobCode = EMCD.JobCode,
                              VendorName = VM.EquipmentVendorName,
                              DateofService = EMCD.DateofService,
                              InvoiveNumber = EMCD.Invoice,
                              Amount = EMCD.Amount,
                              File = EMCD.DocumentPath
                          }).ToList();

            //result = result.Where(r => r.JobCode == jobcodeID).ToList();
            //if (companyID != 0)
            //    result = result.Where(r => r.CompanyID == companyID).ToList();
            //if (fromDate != null)
            //    result = result.Where(r => r.DateOfService >= fromDate).ToList();
            //if (toDate != null)
            //    result = result.Where(r => r.DateOfService <= toDate).ToList();
            //if (stationID != 0)
            //    result = result.Where(r => r.BaseStation == stationID).ToList();
            //if (equipmenttypeID != 0)
            //    result = result.Where(r => r.EquipmentType == equipmenttypeID).ToList();

            var newresult = (from info in result
                             select new PartsPurchaseDetailResult
                             {
                                 Vendor = info.VendorName,
                                 DateofService = info.DateofService,
                                 InvoiceNumber = info.InvoiveNumber,
                                 Amount = info.Amount,
                                 File = info.File
                             }).ToList();

            return newresult.ToList<PartsPurchaseDetailResult>();
        }

    }
}
