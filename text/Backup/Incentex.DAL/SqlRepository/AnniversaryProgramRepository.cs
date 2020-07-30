using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class AnniversaryProgramRepository : RepositoryBase
    {
        IQueryable<AnniversaryCreditProgram> GetAllQuery()
        {
            IQueryable<AnniversaryCreditProgram> qry = from a in db.AnniversaryCreditPrograms
                                                       select a;
            return qry;
        }

        /// <summary>
        /// Update By : Gaurang Pathak 
        /// </summary>
        /// <param name="AnniversaryCreditProgramID"></param>
        /// <returns></returns>
        public AnniversaryCreditProgram GetById(Int64 AnniversaryCreditProgramID)
        {
            //AnniversaryCreditProgram objAnniversaryProgram = GetSingle(GetAllQuery().Where(C => C.AnniversaryCreditProgramID== AnniversaryCreditProgramID).ToList());
            AnniversaryCreditProgram objAnniversaryProgram = (from a in db.AnniversaryCreditPrograms
                                                              where a.AnniversaryCreditProgramID == AnniversaryCreditProgramID
                                                              select a).SingleOrDefault();
            return objAnniversaryProgram;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="WorkgroupID"></param>
        /// <param name="StoreID"></param>
        /// <returns></returns>
        public AnniversaryCreditProgram GetProgByStoreIdWorkgroupId(Int64 WorkgroupID, Int64 StoreID)
        {
            //AnniversaryCreditProgram objAnnCredProg = GetSingle(GetAllQuery().Where(W => W.WorkgroupID == WorkgroupID && W.StoreId == StoreID).ToList());
            AnniversaryCreditProgram objAnnCredProg = (from a in db.AnniversaryCreditPrograms
                                                       where a.WorkgroupID == WorkgroupID && a.StoreId == StoreID
                                                       select a).FirstOrDefault();
            return objAnnCredProg;
        }

        public SelectEmployeeStoreIdResult GetEmpStoreId(Int64 UserInfoID, Int64 WorkgroupID)
        {
            return db.SelectEmployeeStoreId(UserInfoID, WorkgroupID).SingleOrDefault();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        public List<AnniversaryCreditProgramResults> GetCompanyProgramsByStoreId(Int64 StoreId)
        {
            //return GetCompanyPrograms().Where(s => s.StoreId == StoreId).ToList();
            var qry = (from d in db.AnniversaryCreditPrograms
                       join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                       join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                       join icon in db.INC_Lookups on d.EmployeeStatus equals icon.iLookupID
                       where d.StoreId == StoreId
                       select new AnniversaryCreditProgramResults
                       {
                           AnniversaryCreditProgramID = d.AnniversaryCreditProgramID,
                           StoreId = d.StoreId,
                           WorkgroupId = d.WorkgroupID,
                           DepartmentId = d.DepartmentID,
                           Workgroup = il.sLookupName,
                           Department = dept.sLookupName,
                           StandardCreditAmount = (decimal)d.StandardCreditAmount,
                           IssueCreditIn = (int)d.IssueCreditIn,
                           CreditExpiresIn = (int)d.CreditExpiresIn,
                           IconPath = icon.sLookupIcon,
                           EmployeeStatus = (int)d.EmployeeStatus
                       }).ToList();

            return qry;
        }

        public IQueryable<AnniversaryCreditProgramResults> GetCompanyPrograms()
        {
            var qry = (from d in db.AnniversaryCreditPrograms
                       join il in db.INC_Lookups on d.WorkgroupID equals il.iLookupID
                       join dept in db.INC_Lookups on d.DepartmentID equals dept.iLookupID
                       join icon in db.INC_Lookups on d.EmployeeStatus equals icon.iLookupID
                       select new AnniversaryCreditProgramResults
                       {
                           AnniversaryCreditProgramID = d.AnniversaryCreditProgramID,
                           StoreId = d.StoreId,
                           WorkgroupId = d.WorkgroupID,
                           DepartmentId = d.DepartmentID,
                           Workgroup = il.sLookupName,
                           Department = dept.sLookupName,
                           StandardCreditAmount = (decimal)d.StandardCreditAmount,
                           IssueCreditIn = (int)d.IssueCreditIn,
                           CreditExpiresIn = (int)d.CreditExpiresIn,
                           IconPath = icon.sLookupIcon,
                           EmployeeStatus = (int)d.EmployeeStatus
                       }
                     );

            return qry;


        }
        public class AnniversaryCreditProgramResults
        {
            public Int64 AnniversaryCreditProgramID { get; set; }
            public Int64 StoreId { get; set; }
            public Int64 WorkgroupId { get; set; }
            public Int64 DepartmentId { get; set; }
            public string Workgroup { get; set; }
            public string Department { get; set; }
            public decimal StandardCreditAmount { get; set; }
            public int IssueCreditIn { get; set; }
            public int CreditExpiresIn { get; set; }
            public string IconPath { get; set; }
            public Int64 EmployeeStatus { get; set; }

        }

        public void DeleteProgram(Int64 AnniversaryProgramId)
        {
            try
            {
                AnniversaryCreditProgram objProgram = GetById(AnniversaryProgramId);
                Delete(objProgram);
                SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public enum CompanyAnniversarySortExpType
        {
            OrderID,
            EmployeeID,
            HirerdDate,
            FullName,
            sBaseStation,
            CreditBalance,
            Status,
            OrderNumber,
            ReferenceName,
            TotalAmount,
            PaymentMethod,
            OrderDate,
            OrderStatus,
            StartingCreditAmount,
            AnniversaryCreditAmount

        }

        public List<selectcompanyanneversaryprogResult> GetCompanyEmployeeAnniversaryCredit(Int64 companyid, Int64 workgroupid, Incentex.DAL.SqlRepository.AnniversaryProgramRepository.CompanyAnniversarySortExpType SortExp, DAEnums.SortOrderType SortOrder, string employeeId, string FirstName, string LastName, Int64? stationcode, Int64? employeestatus)
        {
            var qry = db.selectcompanyanneversaryprog(companyid, workgroupid, employeeId, FirstName, LastName, employeestatus, stationcode).ToList();
            switch (SortExp)
            {
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.EmployeeID:
                    qry = qry.OrderBy(s => s.EmployeeID).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.HirerdDate:
                    qry = qry.OrderBy(s => s.HirerdDate).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.FullName:
                    qry = qry.OrderBy(s => s.FullName).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.sBaseStation:
                    qry = qry.OrderBy(s => s.sBaseStation).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.CreditBalance:
                    qry = qry.OrderBy(s => s.CreditBalance).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.Status:
                    qry = qry.OrderBy(s => s.Status).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.AnniversaryCreditAmount:
                    qry = qry.OrderBy(s => s.AnniversaryCreditAmount).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.StartingCreditAmount:
                    qry = qry.OrderBy(s => s.StartingCreditAmount).ToList();
                    break;
            }

            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                qry.Reverse();
            }
            return qry;


        }

        //Created on 21 Jan by Ankit
        public SelectAnniversaryCreditProgramPerEmployeeResult GetCompanyEmployeeAnniversaryCreditDetails(Int64 UserInfoId)
        {
            var qry = db.SelectAnniversaryCreditProgramPerEmployee(UserInfoId);
            return qry.SingleOrDefault();


        }

        public List<SelectAnniversaryOrderPerEmployeeResult> GetAnniversaryOrderHistoryByEmployee(Int64 UserInfoId)
        {
            var qry = db.SelectAnniversaryOrderPerEmployee(UserInfoId);
            return qry.ToList<SelectAnniversaryOrderPerEmployeeResult>();
        }

        public List<SelectOrderHistoryPerEmployeeResult> GetOrderHistoryByEmployee(Int64 UserInfoId, Incentex.DAL.SqlRepository.AnniversaryProgramRepository.CompanyAnniversarySortExpType SortExp, DAEnums.SortOrderType SortOrder, string From)
        {
            var qry = db.SelectOrderHistoryPerEmployee(UserInfoId, From).ToList();
            switch (SortExp)
            {
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.OrderID:
                    qry = qry.OrderBy(s => s.OrderID).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.OrderNumber:
                    qry = qry.OrderBy(s => s.OrderNumber).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.ReferenceName:
                    qry = qry.OrderBy(s => s.ReferenceName).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.TotalAmount:
                    qry = qry.OrderBy(s => s.TotalAmount).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.PaymentMethod:
                    qry = qry.OrderBy(s => s.PaymentMethod).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.OrderDate:
                    qry = qry.OrderBy(s => s.OrderDate).ToList();
                    break;
                case AnniversaryProgramRepository.CompanyAnniversarySortExpType.OrderStatus:
                    qry = qry.OrderBy(s => s.OrderStatus).ToList();
                    break;
            }
            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                qry.Reverse();
            }
            return qry;
        }

        //End

        public void UpdateStatus(int anniversaryProgramid, int EmployeeStatus)
        {

            {
                var obj = db.Update_ProgramStatus(anniversaryProgramid, EmployeeStatus);

            }
        }

        /// <summary>
        /// Function to get list of users who are having the anniversary date of the annivesray credit, on the current day(today),
        /// or users who had an expiry date of previous anniversary credit, the day before current day(yesterday)
        /// </summary>
        /// <returns>
        /// List of users who are having the anniversary date of the annivesray credit, on the current day(today),
        /// or users who had an expiry date of previous anniversary credit, the day before current day(yesterday)
        /// </returns>
        public List<GetUsersToApplyAnniversaryCreditResult> GetUsersToApplyAnniversaryCredit()
        {
            return db.GetUsersToApplyAnniversaryCredit().ToList();
        }

        /// <summary>
        /// Function to get list of users who are part of anniversary credit programs but their anniversary date
        /// or expiry date is not set
        /// </summary>
        /// <returns>
        /// List of users who are part of anniversary credit programs but their anniversary date
        /// or expiry date is not set
        /// </returns>
        public List<GetUsersToSetAnniversaryNExpiryDatesResult> GetUsersToSetAnniversaryNExpiryDates()
        {
            return db.GetUsersToSetAnniversaryNExpiryDates().ToList();
        }
    }
}
