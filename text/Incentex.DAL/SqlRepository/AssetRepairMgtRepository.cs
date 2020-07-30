using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace Incentex.DAL.SqlRepository
{
    public class AssetRepairMgtRepository : RepositoryBase
    {

        public EquipmentRepairOrder GetRepairOrderByID(Int64 RepairOrderID)
        {
            EquipmentRepairOrder obj = (from r in db.EquipmentRepairOrders
                                        where r.RepairOrderID == RepairOrderID
                                        select r).SingleOrDefault();
            return obj;
        }

        #region Create RepairOrder
        /// <summary>
        /// Get Max Repair No
        /// </summary>
        /// <returns></returns>
        public Int64? GetMaxRepairNo()
        {
            //if (db.EquipmentRepairOrders.Count()>0 )
            return db.EquipmentRepairOrders.Max(x => (Int64?)x.AutoRepairNumber);
            //else
            //return null;
        }
    
        /// <summary>
        /// Get Base Station List for Vendor Employee
        /// </summary>
        /// <returns></returns>
        public List<INC_BasedStation> GetBaseStationFromVendorEmployeeBaseSattion()
        {
            var test = (from i in db.EquipmentVendorEmployees
                        join j in db.EquipmentVendorMasters on i.VendorID equals j.EquipmentVendorID
                        where i.BaseStationID.Length > 0 && j.IsCustomer == false
                        select i.BaseStationID).ToArray();
            List<Int64> intList = new List<Int64>();
            foreach (var items in test)
            {
                String[] list = items.Split(',');
                intList.AddRange(list.Select(s => Int64.Parse(s)));
            }
            var qry = (from b in db.INC_BasedStations
                       where (from l in intList
                              select l).Contains(b.iBaseStationId)
                       select b).ToList();
            return qry.ToList<INC_BasedStation>();

        }
      
        /// <summary>
        /// Get Base Station List for Customer Employee
        /// </summary>
        /// <returns></returns>
        public List<INC_BasedStation> GetBaseStationFromCustomerEmployeeBaseSattion()
        {
            var test = (from i in db.EquipmentVendorEmployees
                        join j in db.EquipmentVendorMasters on i.VendorID equals j.EquipmentVendorID
                        where i.BaseStationID.Length > 0 && j.IsCustomer == true
                        select i.BaseStationID).ToArray();
            List<Int64> intList = new List<Int64>();
            foreach (var items in test)
            {
                String[] list = items.Split(',');
                intList.AddRange(list.Select(s => Int64.Parse(s)));
            }
            var qry = (from b in db.INC_BasedStations
                       where (from l in intList
                              select l).Contains(b.iBaseStationId)
                       select b).ToList();
            return qry.ToList<INC_BasedStation>();

        }
        #endregion

        /// <summary>
        /// get all details of Equipment Billing Repair Hours
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <returns></returns>
        public List<GetEquipmentBillingRepairHourInfo> GetEquipmentBillingRepairHourDetails(Int64 repairOrderID)
        {

            var qry = (from bi in db.EquipmentBillingRepairHours
                       join u in db.UserInformations on bi.UserInfoID equals u.UserInfoID
                       where u.IsDeleted == false &&
                            (repairOrderID != 0 ? (bi.RepairOrderID.HasValue ? bi.RepairOrderID.Value == repairOrderID : true) : true)
                       select new GetEquipmentBillingRepairHourInfo
                       {
                           BillingRepairHoursID = bi.BillingRepairHoursID,
                           RepairOrderID = bi.RepairOrderID,
                           MechanicName = u.FirstName + " " + u.LastName,
                           HoursWorked = bi.HoursWorked,
                           CompanyID = bi.CompanyID,
                           CreatedDate = bi.CreatedDate,
                           UserInfoID = u.UserInfoID,
                           SummaryWorkPerformed = bi.SummaryWorkPerformed
                       }).ToList();

            //if (repairOrderID != 0)
            //{
            //    qry = qry.Where(q => (q.RepairOrderID == null ? q.RepairOrderID == null : q.RepairOrderID == repairOrderID)).ToList();
            //}
            return qry.ToList<GetEquipmentBillingRepairHourInfo>();
        }

        /// <summary>
        /// get all details of Equipment Billing Parts Ordered
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <returns></returns>
        public List<GetEquipmentBillingPartsOrderedInfo> GetEquipmentBillingPartsOrderedDetails(Int64 repairOrderID)
        {
            var qry = (from bi in db.EquipmentBillingPartsOrdereds
                       join v in db.EquipmentVendorMasters on bi.VendorID equals v.EquipmentVendorID into leftJoinTable
                       from objResult in leftJoinTable.DefaultIfEmpty()
                       where (repairOrderID != 0 ? (bi.RepairOrderID.HasValue ? bi.RepairOrderID.Value == repairOrderID : true) : true)
                       select new GetEquipmentBillingPartsOrderedInfo
                       {
                           BillingPartsOrderedID = bi.BillingPartsOrderedID,
                           RepairOrderID = bi.RepairOrderID,
                           VendorName = objResult.EquipmentVendorName,
                           Quantity = bi.Quantity,
                           VendorID = bi.VendorID,
                           CreatedDate = bi.CreatedDate,
                           UserInfoID = bi.UserInfoID,
                           PartNumber = bi.PartNumber,
                           SummaryDescriptions = bi.SummaryDescriptions
                       }).ToList();
            //if (repairOrderID != 0)
            //{
            //    qry = qry.Where(q => (q.RepairOrderID == null ? q.RepairOrderID == null : q.RepairOrderID == repairOrderID)).ToList();
            //}

            return qry.ToList<GetEquipmentBillingPartsOrderedInfo>();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="repairOrderId"></param>
        /// <param name="baseStationId"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public List<EquipmentOnSiteInventory> GetOnSiteInventoryInfo(Int64 repairOrderId, Int64 baseStationId, Int64 categoryID)
        {
            var qry = (from i in db.EquipmentOnSiteInventories
                       where (i.RepairOrderID == repairOrderId) &&
                         (baseStationId != 0 ? i.BaseStationID == baseStationId : true) &&
                         (categoryID != 0 ? i.CategoryID == categoryID : true)
                       select i).ToList();

            //if (baseStationId != 0)
            //{
            //    qry = qry.Where(q => q.BaseStationID == baseStationId).ToList();
            //}
            //if (categoryID != 0)
            //{
            //    qry = qry.Where(q => q.CategoryID == categoryID).ToList();
            //}
            return qry.ToList();
        }
        public List<EquipmentRepairOrder> GetEquipmentRepairOrderDetails()
        {
            List<EquipmentRepairOrder> list = (from er in db.EquipmentRepairOrders
                                               select er).ToList();
            return list;
        }

        public List<GetEquipmentMaster> GetEquipmentMasterbyEquipmentTypeID(Int64 equipmentTypeID)
        {
            List<GetEquipmentMaster> list = (from em in db.EquipmentMasters
                                             where em.EquipmentTypeID == equipmentTypeID
                                             select new GetEquipmentMaster
                                             {
                                                 EquipmentID = em.EquipmentID,
                                                 EquipmentMasterID = em.EquipmentMasterID,
                                                 EquipmentTypeID = em.EquipmentTypeID
                                             }).ToList();
            return list;

        }

        public List<RepairOrderList> GetRepairOrderListBy(Int64 CompanyID, Int64 EquipmentTypeID, Int64 RepairOrderID, Int64 BaseStationID, Int64 RepairStatusID, String FromDate, String ToDate, string IsBillingComplete)
        {

            var qry = (from er in db.EquipmentRepairOrders
                       join em in db.EquipmentMasters on er.EquipmentMasterID equals em.EquipmentMasterID
                       join l1 in db.INC_Lookups on em.EquipmentTypeID equals l1.iLookupID
                       join l2 in db.EquipmentLookups on er.RepairStatusID equals l2.iLookupID into leftJoinTable
                       from StatusResult in leftJoinTable.DefaultIfEmpty()
                       join bs in db.INC_BasedStations on em.BaseStationID equals bs.iBaseStationId
                       join el in db.EquipmentLookups on er.RepairReason equals el.iLookupID
                       join l3 in db.INC_Lookups on em.BrandID equals l3.iLookupID into erp1
                       from sub1 in erp1.DefaultIfEmpty()
                       join l4 in db.INC_Lookups on em.FuelTypeID equals l4.iLookupID into erp2
                       from sub2 in erp2.DefaultIfEmpty()

                       select new RepairOrderList
                       {
                           AutoRepairNumber = er.AutoRepairNumber,
                           RepairOrderID = er.RepairOrderID,
                           EquipmentMasterID = er.EquipmentMasterID,
                           CompanyID = er.CompanyID,
                           EquipmentID = em.EquipmentID,
                           EquipmentTypeID = em.EquipmentTypeID,
                           RepairStatus = StatusResult.sLookupName,
                           RepairStatusID = StatusResult.iLookupID,
                           EquipmetType = l1.sLookupName,
                           CreatedDate = er.CreatedDate,
                           BaseStation = bs.sBaseStation,
                           BaseStationID = bs.iBaseStationId,
                           ProblemDescription = er.ProblemDescription,
                           ReturnedDate = er.RequestedServiceDate,
                           RepairReason = el.sLookupName,
                           ModalYear = em.ModelYear,
                           SerialNo = em.SerialNumber,
                           Brand = (sub1 == null ? String.Empty : sub1.sLookupName),
                           Fuel = (sub2 == null ? String.Empty : sub2.sLookupName),
                           IsBillingComplete = er.IsBillingComplete,
                           VendorRepairID = er.VendorRepairID
                       }).ToList();

            if (CompanyID != 0)
            {
                qry = qry.Where(q => q.CompanyID == CompanyID).ToList();
            }
            if (EquipmentTypeID != 0)
            {
                qry = qry.Where(q => q.EquipmentTypeID == EquipmentTypeID).ToList();
            }
            if (RepairOrderID != 0)
            {
                qry = qry.Where(q => q.RepairOrderID == RepairOrderID).ToList();
            }
            if (BaseStationID != 0)
            {
                qry = qry.Where(q => q.BaseStationID == BaseStationID).ToList();
            }
            if (RepairStatusID != 0)
            {
                qry = qry.Where(q => q.RepairStatusID == RepairStatusID).ToList();
            }
            if (FromDate != "0")
            {
                qry = qry.Where(q => q.CreatedDate.Value.Date >= Convert.ToDateTime(FromDate)).ToList();
            }
            if (ToDate != "0")
            {
                qry = qry.Where(q => q.CreatedDate.Value.Date <= Convert.ToDateTime(ToDate)).ToList();
            }
            if (IsBillingComplete != null)
            {
                qry = qry.Where(q => q.IsBillingComplete == Convert.ToBoolean(IsBillingComplete)).ToList();
            }
            return qry.ToList<RepairOrderList>();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="userInfoID"></param>
        /// <returns></returns>
        public List<RepairOrderList> GetRepairOrderByBaseStation(Int64 CompanyID, Int64 userInfoID)
        {

            EquipmentVendorEmployee objEquipmentVendorEmployee = db.EquipmentVendorEmployees.FirstOrDefault(r => r.UserInfoID == userInfoID);
            if (objEquipmentVendorEmployee != null)
            {
                var qry = (from er in db.EquipmentRepairOrders
                           join em in db.EquipmentMasters on er.EquipmentMasterID equals em.EquipmentMasterID
                           join l1 in db.INC_Lookups on em.EquipmentTypeID equals l1.iLookupID
                           join l2 in db.EquipmentLookups on er.RepairStatusID equals l2.iLookupID into leftJoinTable
                           from StatusResult in leftJoinTable.DefaultIfEmpty()
                           join bs in db.INC_BasedStations on em.BaseStationID equals bs.iBaseStationId
                           join el in db.EquipmentLookups on er.RepairReason equals el.iLookupID
                           join l3 in db.INC_Lookups on em.BrandID equals l3.iLookupID into erp1
                           from sub1 in erp1.DefaultIfEmpty()
                           join l4 in db.INC_Lookups on em.FuelTypeID equals l4.iLookupID into erp2
                           from sub2 in erp2.DefaultIfEmpty()
                           where objEquipmentVendorEmployee.BaseStationID.Split(',').Contains(Convert.ToString(em.BaseStationID)) &&
                           er.IsBillingComplete == false &&
                           (CompanyID != 0 ? er.CompanyID == CompanyID : true)
                           select new RepairOrderList
                           {
                               AutoRepairNumber = er.AutoRepairNumber,
                               RepairOrderID = er.RepairOrderID,
                               EquipmentMasterID = er.EquipmentMasterID,
                               CompanyID = er.CompanyID,
                               EquipmentID = em.EquipmentID,
                               EquipmentTypeID = em.EquipmentTypeID,
                               RepairStatus = StatusResult.sLookupName,
                               RepairStatusID = StatusResult.iLookupID,
                               EquipmetType = l1.sLookupName,
                               CreatedDate = er.CreatedDate,
                               BaseStation = bs.sBaseStation,
                               BaseStationID = bs.iBaseStationId,
                               ProblemDescription = er.ProblemDescription,
                               ReturnedDate = er.RequestedServiceDate,
                               RepairReason = el.sLookupName,
                               ModalYear = em.ModelYear,
                               SerialNo = em.SerialNumber,
                               Brand = (sub1 == null ? String.Empty : sub1.sLookupName),
                               Fuel = (sub2 == null ? String.Empty : sub2.sLookupName),
                               IsBillingComplete = er.IsBillingComplete,
                               VendorRepairID = er.VendorRepairID
                           }).ToList();

                //if (CompanyID != 0)
                //{
                //    qry = qry.Where(q => q.CompanyID == CompanyID).ToList();
                //}
                return qry.ToList<RepairOrderList>();
            }
            else
                return null;
        }

        /// <summary>
        /// Get RepairOrder List By EquipmentMasterID
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentTypeID"></param>
        /// <returns></returns>
        public List<RepairOrderList> GetRepairOrderByEquipmentMasterID(Int64 equipmentMasterID)
        {

            var qry = (from er in db.EquipmentRepairOrders
                       join em in db.EquipmentMasters on er.EquipmentMasterID equals em.EquipmentMasterID
                       join l1 in db.INC_Lookups on em.EquipmentTypeID equals l1.iLookupID
                       join l2 in db.EquipmentLookups on er.RepairStatusID equals l2.iLookupID into leftJoinTable
                       from StatusResult in leftJoinTable.DefaultIfEmpty()
                       join bs in db.INC_BasedStations on em.BaseStationID equals bs.iBaseStationId
                       join el in db.EquipmentLookups on er.RepairReason equals el.iLookupID
                       join l3 in db.INC_Lookups on em.BrandID equals l3.iLookupID into erp1
                       from sub1 in erp1.DefaultIfEmpty()
                       join l4 in db.INC_Lookups on em.FuelTypeID equals l4.iLookupID into erp2
                       from sub2 in erp2.DefaultIfEmpty()
                       where (equipmentMasterID != 0 ? er.EquipmentMasterID == equipmentMasterID : true)
                       select new RepairOrderList
                       {
                           AutoRepairNumber = er.AutoRepairNumber,
                           RepairOrderID = er.RepairOrderID,
                           EquipmentMasterID = er.EquipmentMasterID,
                           CompanyID = er.CompanyID,
                           EquipmentID = em.EquipmentID,
                           EquipmentTypeID = em.EquipmentTypeID,
                           RepairStatus = StatusResult.sLookupName,
                           RepairStatusID = StatusResult.iLookupID,
                           EquipmetType = l1.sLookupName,
                           CreatedDate = er.CreatedDate,
                           BaseStation = bs.sBaseStation,
                           BaseStationID = bs.iBaseStationId,
                           ProblemDescription = er.ProblemDescription,
                           ReturnedDate = er.RequestedServiceDate,
                           RepairReason = el.sLookupName,
                           ModalYear = em.ModelYear,
                           SerialNo = em.SerialNumber,
                           Brand = (sub1 == null ? String.Empty : sub1.sLookupName),
                           Fuel = (sub2 == null ? String.Empty : sub2.sLookupName),
                           IsBillingComplete = er.IsBillingComplete,
                           VendorRepairID = er.VendorRepairID
                       }).ToList();


            //if (equipmentMasterID != 0)
            //{
            //    qry = qry.Where(q => q.EquipmentMasterID == equipmentMasterID).ToList();
            //}

            return qry.ToList<RepairOrderList>();
        }
        public List<GetVenderUserListInfo> GetVendorUserListbyBaseStationID(List<String> intList)
        {

            var qry = (from ev in db.EquipmentVendorEmployees
                       join ui in db.UserInformations on ev.UserInfoID equals ui.UserInfoID
                       join evm in db.EquipmentVendorMasters on ev.VendorID equals evm.EquipmentVendorID
                       where evm.IsCustomer == false && ui.IsDeleted == false
                       select new GetVenderUserListInfo
                       {
                           UserInfoID = ev.UserInfoID,
                           FullName = ui.FirstName + " " + ui.LastName,
                           VendorEmployeeID = ev.VendorEmployeeID,
                           LoginEmail = ui.LoginEmail,
                           baseStations = ev.BaseStationID
                       }).ToList();
            List<GetVenderUserListInfo> list = new List<GetVenderUserListInfo>();

            foreach (GetVenderUserListInfo item in qry)
            {
                bool Flg = false;
                for (int i = 0; i <= intList.Count - 1; i++)
                {
                    string pattern = intList[i].ToString();
                    if (item.baseStations.Contains(pattern))
                    {
                        if (Flg == false)
                        {
                            var newresult = (from e in qry
                                             where e.UserInfoID == item.UserInfoID
                                             select e).ToList();
                            list.AddRange(newresult);
                            Flg = true;
                        }

                    }

                }
            }
            //for (int i = 0; i <= intList.Count - 1; i++)
            //{
            //    string pattern = intList[i].ToString();
            //    var newresult = (from e in qry
            //                     where e.baseStations.Contains(pattern)
            //                     select e).ToList();
            //    list.AddRange(newresult);
            //}

            return list.Distinct().ToList();

        }
        public List<GetVenderUserListInfo> GetCustomerUserListbyBaseStationID(List<String> intList)
        {

            var qry = (from ev in db.EquipmentVendorEmployees
                       join ui in db.UserInformations on ev.UserInfoID equals ui.UserInfoID
                       join evm in db.EquipmentVendorMasters on ev.VendorID equals evm.EquipmentVendorID
                       where evm.IsCustomer == true && ui.IsDeleted == false
                       select new GetVenderUserListInfo
                       {
                           UserInfoID = ev.UserInfoID,
                           FullName = ui.FirstName + " " + ui.LastName,
                           VendorEmployeeID = ev.VendorEmployeeID,
                           LoginEmail = ui.LoginEmail,
                           baseStations = ev.BaseStationID
                       }).ToList();
            List<GetVenderUserListInfo> list = new List<GetVenderUserListInfo>();

            foreach (GetVenderUserListInfo item in qry)
            {
                bool Flg = false;
                for (int i = 0; i <= intList.Count - 1; i++)
                {
                    string pattern = intList[i].ToString();
                    if (item.baseStations.Contains(pattern))
                    {
                        if (Flg == false)
                        {
                            var newresult = (from e in qry
                                             where e.UserInfoID == item.UserInfoID
                                             select e).ToList();
                            list.AddRange(newresult);
                            Flg = true;
                        }

                    }

                }
            }
            //for (int i = 0; i < intList.Count - 1; i++)
            //{
            //    string pattern = intList[i].ToString();
            //    var newresult = (from e in qry
            //                     where e.baseStations.Contains(pattern)
            //                     select e).ToList();
            //    list.AddRange(newresult);
            //}

            return list.Distinct().ToList();

        }
        //public List<GetVenderUserListInfo> GetCompanyUserList(Int64 emailID)
        //{

        //    var qry = (from eve in db.EquipmentVendorEmployees
        //               join ui in db.UserInformations on eve.UserInfoID equals ui.UserInfoID
        //               join evm in db.EquipmentVendorMasters on eve.VendorID equals evm.EquipmentVendorID
        //               join eme in db.EquipmentManageEmails on eve.UserInfoID equals eme.UserInfoID
        //               join bs in db.INC_BasedStations on evm.BaseStationID equals bs.iBaseStationId
        //               where eme.EquipmentEmailID == emailID && ui.IsDeleted == false
        //               select new GetVenderUserListInfo
        //               {
        //                   UserInfoID = eve.UserInfoID,
        //                   FullName = ui.FirstName + " " + ui.LastName,
        //                   VendorEmployeeID = eve.VendorEmployeeID,
        //                   LoginEmail = ui.LoginEmail,
        //                   baseStations = eve.BaseStationID,
        //                   baseStationName = bs.sBaseStation
        //               }).ToList();

        //    return qry.ToList<GetVenderUserListInfo>();
        //}
        public class GetEquipmentBillingRepairHourInfo
        {
            public Int64 BillingRepairHoursID { get; set; }
            public Int64? RepairOrderID { get; set; }
            public Int64? CompanyID { get; set; }
            public Int64? UserInfoID { get; set; }
            public String MechanicName { get; set; }
            public Double? HoursWorked { get; set; }
            public DateTime? CreatedDate { get; set; }
            public String SummaryWorkPerformed { get; set; }
        }
        public class GetEquipmentBillingPartsOrderedInfo
        {
            public Int64 BillingPartsOrderedID { get; set; }
            public Int64? RepairOrderID { get; set; }
            public Int64? VendorID { get; set; }
            public Int64? UserInfoID { get; set; }
            public String VendorName { get; set; }
            public Int64? Quantity { get; set; }
            public DateTime? CreatedDate { get; set; }
            public String PartNumber { get; set; }
            public String SummaryDescriptions { get; set; }
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="ilookupID"></param>
        /// <param name="ilookupCode"></param>
        /// <returns></returns>
        public List<EquipmentLookup> GetRepairStatusInfo(Int64 ilookupID, String ilookupCode)
        {
            var qry = (from e in db.EquipmentLookups
                       where (ilookupID != 0 ? e.iLookupID == ilookupID : true) &&
                             (ilookupCode != null ? e.iLookupCode == ilookupCode : true)
                       select e).ToList();

            //if (ilookupID != 0)
            //{
            //    qry = qry.Where(q => q.iLookupID == ilookupID).ToList();
            //}
            //if (ilookupCode != null)
            //{
            //    qry = qry.Where(q => q.iLookupCode == ilookupCode).ToList();
            //}

            return qry.ToList();
        }
        public List<EquipmentLookup> GetRepairReasonInfo(Int64 ilookupID, String ilookupCode)
        {
            var qry = (from e in db.EquipmentLookups
                       where (ilookupID != 0 ? e.iLookupID == ilookupID : true) &&
                             (ilookupCode != null ? e.iLookupCode == ilookupCode : true)
                       select e).ToList();

            //if (ilookupID != 0)
            //{
            //    qry = qry.Where(q => q.iLookupID == ilookupID).ToList();
            //}
            //if (ilookupCode != null)
            //{
            //    qry = qry.Where(q => q.iLookupCode == ilookupCode).ToList();
            //}

            return qry.ToList();
        }
        public EquipmentLookup GetRepairReasonInfobyID(Int64 reasonRepairID)
        {
            return db.EquipmentLookups.SingleOrDefault(c => c.iLookupID == reasonRepairID);
        }
        public EquipmentLookup GetRepairStatusInfobyID(Int64 statusRepairID)
        {
            return db.EquipmentLookups.SingleOrDefault(c => c.iLookupID == statusRepairID);
        }

        public List<EquipmentLookup> CheckDuplication(String RepairReasonName)
        {
            return db.EquipmentLookups.Where(c => c.sLookupName == RepairReasonName).ToList(); ;
        }
        //public EquipmentRepairOrder GetDetailFromCampID(Int64 repairID)
        //{

        //    EquipmentRepairOrder obj = (from r in db.EquipmentRepairOrders
        //                                where r.RepairOrderID == repairID
        //                                select r).ToList().SingleOrDefault();
        //    return obj;

        //}

        public class GetEquipmentMaster
        {
            public Int64 EquipmentMasterID { get; set; }
            public String EquipmentID { get; set; }
            public Int64? EquipmentTypeID { get; set; }
        }

        public class RepairOrderList
        {
            public Int64 AutoRepairNumber { get; set; }
            public Int64 RepairOrderID { get; set; }
            public Int64? EquipmentMasterID { get; set; }
            public Int64? CompanyID { get; set; }
            public String EquipmentID { get; set; }
            public Int64? EquipmentTypeID { get; set; }
            public String ProblemDescription { get; set; }
            public String RepairReason { get; set; }
            public String BaseStation { get; set; }
            public Int64 BaseStationID { get; set; }
            public String EquipmetType { get; set; }
            public String RepairStatus { get; set; }
            public Int64? RepairStatusID { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? ReturnedDate { get; set; }
            public Int32? ModalYear { get; set; }
            public String SerialNo { get; set; }
            public String Brand { get; set; }
            public String Fuel { get; set; }
            public bool IsBillingComplete { get; set; }
            public String VendorRepairID { get; set; }
        }

        public class GetVenderUserListInfo
        {
            public Int64? UserInfoID { get; set; }
            public Int64 VendorEmployeeID { get; set; }
            public String LoginEmail { get; set; }
            public String FullName { get; set; }
            public String baseStations { get; set; }
            public String baseStationName { get; set; }
        }

        public Int64? GetEquipMasterIDFrmRepairOrderID(Int64 RepairOrderID)
        {
            Int64? EquipmentMasterID = db.EquipmentRepairOrders.FirstOrDefault(x => x.RepairOrderID == RepairOrderID).EquipmentMasterID;
            return EquipmentMasterID;
        }

        #region Create Repair Order Dropdown Relation
        public class GetEquipmentTypeResult
        {
            public Int64? EquipmentTypeID { get; set; }
            public String EquipmentTypeName { get; set; }
        }
        public List<GetEquipmentTypeResult> GetEquipmentTypebyCompany(Int64 companyID)
        {
            //List<GetEquipmentTypeResult> list = (from em in db.EquipmentMasters
            //                                     join IL in db.INC_Lookups on em.EquipmentTypeID equals IL.iLookupID
            //                                     where IL.iLookupCode == "EquipmentType"
            var list = (from IL in db.INC_Lookups
                        join EM in db.EquipmentMasters on IL.iLookupID equals EM.EquipmentTypeID
                        where IL.iLookupCode == "EquipmentType"
                        select new
                          {
                              EquipmentTypeID = EM.EquipmentTypeID,
                              EquipmentTypeName = IL.sLookupName,
                              CompanyID = EM.CompanyID
                          }).ToList();
            if (companyID != 0)
                list = list.Where(r => r.CompanyID == companyID).ToList();
            var newresult = (from info in list
                             group info by new { info.EquipmentTypeID, info.EquipmentTypeName } into newInfo
                             select new GetEquipmentTypeResult
                             {
                                 EquipmentTypeID = newInfo.Key.EquipmentTypeID,
                                 EquipmentTypeName = newInfo.Key.EquipmentTypeName
                             }).ToList();
            return newresult;
        }

        //public List<GetEquipmentStatusResult> GetEquipmentStatusbyEquipmentType(Int64 equipmentTypeID,Int64 companyID)
        //{
        //    var list = (from IL in db.INC_Lookups
        //                join EM in db.EquipmentMasters on IL.iLookupID equals EM.Status
        //                where IL.iLookupCode == "EquipmentStatus"
        //                                     select new 
        //                                     {
        //                                         EquipmentStatusID = EM.Status,
        //                                         EquipmentStatusName = IL.sLookupName,
        //                                         EquipmentTypeID = EM.EquipmentTypeID,
        //                                         CompanyID=EM.CompanyID
        //                                     }).ToList();
        //    if (companyID != 0)
        //        list = list.Where(r => r.CompanyID == companyID).ToList();
        //    if (equipmentTypeID != 0)
        //        list = list.Where(r => r.EquipmentTypeID == equipmentTypeID).ToList();
        //    var newresult = (from info in list
        //                     group info by new { info.EquipmentStatusID, info.EquipmentStatusName } into newInfo
        //                     select new GetEquipmentStatusResult
        //                     {
        //                         EquipmentStatusID = newInfo.Key.EquipmentStatusID,
        //                         EquipmentStatusName = newInfo.Key.EquipmentStatusName
        //                     }).ToList();
        //    return newresult;
        //}
        public class GetEquipmentIDResult
        {
            public Int64 EquipmentMasterID { get; set; }
            public Int64? EquipmentTypeID { get; set; }
            public String EquipmentID { get; set; }
            public Int64? CompanyID { get; set; }
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="equipmentTypeID"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public List<GetEquipmentIDResult> GetEquipmentIDbyEquipmentType(Int64 equipmentTypeID, Int64 companyID)
        {
            List<GetEquipmentIDResult> list = (from em in db.EquipmentMasters
                                               where (companyID != 0 ? (em.CompanyID == companyID) : true) &&
                                                     (equipmentTypeID != 0 ? (em.EquipmentTypeID == equipmentTypeID) : true)
                                               select new GetEquipmentIDResult
                                                   {
                                                       EquipmentTypeID = em.EquipmentTypeID,
                                                       EquipmentMasterID = em.EquipmentMasterID,
                                                       CompanyID = em.CompanyID,
                                                       EquipmentID = em.EquipmentID
                                                   }).ToList();
            //if (companyID != 0)
            //    list = list.Where(r => r.CompanyID == companyID).ToList();
            //if (equipmentTypeID != 0)
            //    list = list.Where(r => r.EquipmentTypeID == equipmentTypeID).ToList();

            return list;
        }

        public String GetEquipmentStatusbyEquipmentID(Int64 equipmentmasterID)
        {
            var EquipStatus = (from em in db.EquipmentMasters
                               where em.EquipmentMasterID == equipmentmasterID
                               select em.Status).FirstOrDefault();
            return Convert.ToString(EquipStatus);
        }
        #endregion

        #region CheckonSiteInventory
        public void UpdateInventoryParts(String finalInventory, Int64 partsQuantity, Int64 repairOrderID, Int64 equipmentInventoryID, Int64 userInfoID, String partNumber, String description, Int64 vendorID)
        {
            try
            {


                //Edit EquipmentInventoryMaster
                EquipmentInventoryMaster objInventoryMaster = new EquipmentInventoryMaster();
                IQueryable<EquipmentInventoryMaster> qry = from c in db.EquipmentInventoryMasters.Where(a => a.EquipmentInventoryID == equipmentInventoryID)
                                                           select c;
                objInventoryMaster = GetSingle(qry.ToList());
                objInventoryMaster.CurrentInvenory = finalInventory;
                db.SubmitChanges();
                //Insert Equipmentbillingpartsordered

                EquipmentBillingPartsOrdered objpartsordered = new EquipmentBillingPartsOrdered();
                AssetRepairMgtRepository objRepairRepo = new AssetRepairMgtRepository();
                objpartsordered.RepairOrderID = repairOrderID;
                objpartsordered.UserInfoID = userInfoID;
                objpartsordered.Quantity = partsQuantity;
                objpartsordered.PartNumber = partNumber;
                objpartsordered.SummaryDescriptions = description;
                objpartsordered.CreatedDate = DateTime.Now;
                objpartsordered.EquipmentInventoryID = equipmentInventoryID;
                objpartsordered.VendorID = vendorID;
                objRepairRepo.Insert(objpartsordered);
                objRepairRepo.SubmitChanges();
            }
            catch (Exception)
            {

            }

        }

        public void DeletePartsOrdered(Int64 BillingPartsOrderedID)
        {
            var matched = (from c in db.GetTable<EquipmentBillingPartsOrdered>()
                           where c.BillingPartsOrderedID == BillingPartsOrderedID
                           select c).SingleOrDefault();
            try
            {
                if (matched != null)
                {
                    db.EquipmentBillingPartsOrdereds.DeleteOnSubmit(matched);
                }
                if (matched.EquipmentInventoryID != null)
                {
                    EquipmentInventoryMaster objInventoryMaster = new EquipmentInventoryMaster();
                    IQueryable<EquipmentInventoryMaster> qry = from c in db.EquipmentInventoryMasters.Where(a => a.EquipmentInventoryID == matched.EquipmentInventoryID)
                                                               select c;
                    objInventoryMaster = GetSingle(qry.ToList());
                    Int64 CurrentInventory = Convert.ToInt64(objInventoryMaster.CurrentInvenory);
                    Int64 PartsOrdered = Convert.ToInt64(matched.Quantity);
                    Int64 FinalInventory = CurrentInventory + PartsOrdered;
                    objInventoryMaster.CurrentInvenory = Convert.ToString(FinalInventory);
                    db.SubmitChanges();
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        public GetEquipRepairCustomerResult DisplayCustomerInfo(Int64 RepairOrderID)
        {
            return db.GetEquipRepairCustomer(RepairOrderID).FirstOrDefault();
        }

        public void DeleteRepairOrder(Int64 repairOrderID)
        {
            EquipmentRepairOrder objRepairOrder = GetRepairOrderByID(repairOrderID);
            if (objRepairOrder != null)
            {
                //Delete Repair Hours
                var hrs = (from h in db.GetTable<EquipmentBillingRepairHour>()
                           where h.RepairOrderID == repairOrderID
                           select h);
                if (hrs != null)
                {
                    db.EquipmentBillingRepairHours.DeleteAllOnSubmit(hrs);
                }
                //Delete Parts Ordered
                var parts = (from p in db.GetTable<EquipmentBillingPartsOrdered>()
                             where p.RepairOrderID == repairOrderID
                             select p);
                if (parts != null)
                {
                    db.EquipmentBillingPartsOrdereds.DeleteAllOnSubmit(parts);
                }
                db.EquipmentRepairOrders.DeleteOnSubmit(objRepairOrder);
                db.SubmitChanges();
            }
        }

        public Int64 GetRepairOrderStatus(String repairStatus)
        {
            var objStatusID = (from e in db.EquipmentLookups

                               where e.iLookupCode == "RepairStatus" && SqlMethods.Like(e.sLookupName, "%" + repairStatus + "%")
                               select e.iLookupID).FirstOrDefault();
            return Convert.ToInt64(objStatusID);
        }
        public Int64 GetEquipmentStatus()
        {
            var objStatusID = (from e in db.INC_Lookups
                               where e.iLookupCode == "EquipmentStatus" && SqlMethods.Like(e.sLookupName, "Active")
                               select e.iLookupID).FirstOrDefault();
            return Convert.ToInt64(objStatusID);
        }
        public void UpdateEquipmentStatus(Int64 equipmentMasterID, Int64 equipmentStatusID)
        {
            EquipmentMaster EquipMaster = (from e in db.EquipmentMasters
                                           where e.EquipmentMasterID == equipmentMasterID
                                           select e).ToList().FirstOrDefault();
            EquipMaster.Status = equipmentStatusID;
            db.SubmitChanges();
        }

        public List<EquipmentVendorEmployee> GetVendorEmployeeByBaseStation(Int64 equipmentMasterID)
        {
            Int64? BaseStationID = db.EquipmentMasters.Where(x => x.EquipmentMasterID == equipmentMasterID).FirstOrDefault().BaseStationID;

            string ResBaseStationID = Convert.ToString(BaseStationID);
            List<EquipmentVendorEmployee> lst = (from e in db.EquipmentVendorEmployees
                                                 join m in db.EquipmentVendorMasters on e.VendorID equals m.EquipmentVendorID
                                                 where e.BaseStationID.Contains(ResBaseStationID) && m.IsCustomer == true
                                                 select e).ToList();

            return lst;
        }

        public List<GetEquipmentMaintenanceDetailResult> GetEquipMaintenanceForRepair(Int64 EquipmentMasterID)
        {
            var objlist = db.GetEquipmentMaintenanceDetail(EquipmentMasterID).ToList();
            objlist = objlist.Where(x => x.IsRepaired == true).ToList();
            return objlist.ToList<GetEquipmentMaintenanceDetailResult>();
        }
    }
}
