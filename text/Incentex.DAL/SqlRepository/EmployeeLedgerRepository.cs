using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class EmployeeLedgerRepository : RepositoryBase
    {
        IQueryable<EmployeeLedger> GetAllQuery()
        {
            return (from c in db.EmployeeLedgers
             select c);
        }
        public List<EmployeeLedgerResult> GetEmployeeLedgerByUserInfoId(Int64 userid, LedgerSortExpType SortExp, DAEnums.SortOrderType SortOrder)
        {
            var qry = (from c in db.EmployeeLedgers
                       join u in db.UserInformations
                       on new { UpdatedByID = c.UpdateById ?? 0, IsDeleted = false }
                         equals new { UpdatedByID = u.UserInfoID, IsDeleted = u.IsDeleted } into ank
                       from a in ank.DefaultIfEmpty()
                       where c.UserInfoId == userid
                       select new EmployeeLedgerResult
                       {
                           UpdatedBy = a.FirstName + " " + a.LastName,
                           LedgerId = c.LedgerId,
                           TransactionType = c.TransactionType,
                           TransactionCode =c.TransactionCode,
                          TransactionAmount= c.TransactionAmount,
                          PreviousBalance= c.PreviousBalance,
                          CurrentBalance= c.CurrentBalance,
                          TransactionDate= c.TransactionDate,
                          OrderNumber  = c.OrderNumber,
                         AmountCreditDebit = c.AmountCreditDebit,
                         OrderID = c.OrderId,
                           Note = c.Notes
                       }).OrderByDescending(c=>c.LedgerId).ToList<EmployeeLedgerResult>();;

            switch (SortExp)
            {
                case LedgerSortExpType.AmountCreditDebit:
                    qry = qry.OrderBy(s => s.AmountCreditDebit).ToList();
                    break;
                case LedgerSortExpType.CurrentBalance:
                    qry = qry.OrderBy(s => s.CurrentBalance).ToList();
                    break;
                case LedgerSortExpType.OrderNumber:
                    qry = qry.OrderBy(s => s.OrderNumber).ToList();
                    break;
                case LedgerSortExpType.PreviousBalance:
                    qry = qry.OrderBy(s => s.PreviousBalance).ToList();
                    break;
                case LedgerSortExpType.TransactionAmount:
                    qry = qry.OrderBy(s => s.TransactionAmount).ToList();
                    break;
                case LedgerSortExpType.TransactionCode:
                    qry = qry.OrderBy(s => s.TransactionCode).ToList();
                    break;
                case LedgerSortExpType.TransactionDate:
                    qry = qry.OrderBy(s => s.TransactionDate).ToList();
                    break;
                case LedgerSortExpType.TransactionType:
                    qry = qry.OrderBy(s => s.TransactionType).ToList();
                    break;
                case LedgerSortExpType.UpdatedBy:
                    qry = qry.OrderBy(s => s.UpdatedBy).ToList();
                    break;

            }


            if (SortOrder == DAEnums.SortOrderType.Desc)
            {
                qry.Reverse();
            }

            return qry;
            


        }
     
        public List<EmployeeLedger> GetEmployeeLedgerByEmplId(Int64 eid)
        {
            //return GetAllQuery().Where(e => e.CompanyEmployeeId == eid).ToList();
            return (from c in db.EmployeeLedgers
                    where c.CompanyEmployeeId == eid
                    select c).ToList(); 

        }
        public EmployeeLedger GetLastTransactionByEmplID(Int64 eid)
        {
            //var qry = GetAllQuery().Where(e => e.CompanyEmployeeId == eid);
            //qry = qry.OrderBy(e => e.LedgerId);
            //return qry.ToList().LastOrDefault();

            return (from c in db.EmployeeLedgers
                    where c.CompanyEmployeeId == eid
                    orderby c.LedgerId descending
                    select c).FirstOrDefault();  
        }

        public enum LedgerSortExpType
        {
            UpdatedBy,
            LedgerId,
            TransactionType,
            TransactionCode,
            TransactionAmount,
            PreviousBalance,
            CurrentBalance,
            TransactionDate,
            OrderNumber,
            AmountCreditDebit

        }
    }
    public class EmployeeLedgerResult
    {
        public Int64? OrderID { get; set; }
          public string UpdatedBy{get;set;}
           public Int64  LedgerId {get;set;}
           public string TransactionType {get;set;}
           public string TransactionCode {get;set;}
           public decimal? TransactionAmount{get;set;}
           public decimal?  PreviousBalance{get;set;}
           public decimal?  CurrentBalance{get;set;}
           public DateTime? TransactionDate{get;set;}
           public string OrderNumber { get; set; }
           public string AmountCreditDebit { get; set; }
           public string Note { get; set; }

    }
}
