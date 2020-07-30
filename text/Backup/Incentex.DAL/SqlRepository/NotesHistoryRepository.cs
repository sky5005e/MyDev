using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class NotesHistoryRepository : RepositoryBase
    {
        public NoteDetail GetById(Int64 ForeignKey, String NoteFor)
        {
            return db.NoteDetails.FirstOrDefault(C => C.ForeignKey == ForeignKey && C.NoteFor == NoteFor);
        }

        public List<NoteDetail> GetByForeignKeyId(Int64 ForeignKey, DAEnums.NoteForType NoteFor)
        {
            string NoteForName = DAEnums.GetNoteForTypeName(NoteFor);
            List<NoteDetail> objNotes = db.NoteDetails.Where(C => C.ForeignKey == ForeignKey && C.NoteFor == NoteForName).OrderByDescending(n => n.NoteID).OrderByDescending(n => n.CreateDate).ToList();
            return objNotes;
        }

        public List<NoteDetail> GetNotesForCACEId(Int64 ForeignKey, DAEnums.NoteForType NoteFor, string SpecificNotefor)
        {
            string NoteForName = DAEnums.GetNoteForTypeName(NoteFor);
            List<NoteDetail> objNotes = db.NoteDetails.Where(C => C.ForeignKey == ForeignKey && C.NoteFor == NoteForName && C.SpecificNoteFor == SpecificNotefor).OrderByDescending(n => n.NoteID).OrderByDescending(n => n.CreateDate).ToList();
            return objNotes;
        }

        public List<NoteDetail> GetNotesForCACEPerOrderId(Int64 ForeignKey, DAEnums.NoteForType NoteFor)
        {
            string NoteForName = DAEnums.GetNoteForTypeName(NoteFor);
            return db.NoteDetails.Where(C => C.ForeignKey == ForeignKey && C.NoteFor == NoteForName && (C.SpecificNoteFor == null || C.SpecificNoteFor == "" || C.SpecificNoteFor != "IEInternalNotes")).OrderByDescending(n => n.NoteID).OrderByDescending(n => n.CreateDate).ToList();
        }

        public List<NoteDetail> GetNotesForIEPerOrderId(Int64 ForeignKey, DAEnums.NoteForType NoteFor, string SpecificNotefor, int FlagForPurchaseOrder)
        {
            string NoteForName = DAEnums.GetNoteForTypeName(NoteFor);
            var result = db.NoteDetails.Where(C => C.ForeignKey == ForeignKey && C.NoteFor == NoteForName && C.SpecificNoteFor == SpecificNotefor).OrderByDescending(n => n.NoteID).ToList();
            if (FlagForPurchaseOrder == 1)
            {
                var purchaseOrderNotes = db.GetPurchaseOrderNotes(2, DAEnums.NoteForType.PurchaseOrderDetailsIEs.ToString(), ForeignKey).Select(x => new NoteDetail
                    {
                        CreateDate = x.CreateDate,
                        CreatedBy = x.CreatedBy,
                        ForeignKey = x.ForeignKey,
                        Notecontents = x.Notecontents,
                        NoteFor = x.NoteFor,
                        NoteID = x.NoteID,
                        ReceivedBy = x.ReceivedBy,
                        SpecificNoteFor = x.SpecificNoteFor,
                        UpdateDate = x.UpdateDate,
                        UpdatedBy = x.UpdatedBy
                    }).ToList();
                if (purchaseOrderNotes.Count > 0)
                {
                    result.AddRange(purchaseOrderNotes);
                }
            }
            return result;
        }

        public List<NoteDetail> GetNotesForSuppliersPerOrderId(DAEnums.NoteForType NoteFor, string SpecificNotefor, int FlagForPurchaseOrderNote)
        {
            string NoteForName = DAEnums.GetNoteForTypeName(NoteFor);
            var result = db.NoteDetails.Where(C => C.NoteFor == NoteForName && C.SpecificNoteFor == SpecificNotefor).OrderByDescending(n => n.NoteID).ToList();
            if (FlagForPurchaseOrderNote == 1)
            {
                var purchaseOrderNotes = db.GetPurchaseOrderNotes(1, DAEnums.NoteForType.SupplierPurchaseOrder.ToString(), Convert.ToInt64(SpecificNotefor)).Select(x => new NoteDetail
                {
                    CreateDate = x.CreateDate,
                    CreatedBy = x.CreatedBy,
                    ForeignKey = x.ForeignKey,
                    Notecontents = x.Notecontents,
                    NoteFor = x.NoteFor,
                    NoteID = x.NoteID,
                    ReceivedBy = x.ReceivedBy,
                    SpecificNoteFor = x.SpecificNoteFor,
                    UpdateDate = x.UpdateDate,
                    UpdatedBy = x.UpdatedBy
                }).ToList();
                if (purchaseOrderNotes.Count > 0)
                {
                    result.AddRange(purchaseOrderNotes);
                }
            }
            return result;
        }

        /// To get the details of CampainNote by using userID
        public List<CampaignNote> GetCampaignDetailsByEmpID(Int64 userID)
        {
            List<CampaignNote> objCampNote = (from c in db.CampaignNotes
                                              where c.UserInfoID == userID
                                              select c).ToList();
            return objCampNote;
        }

        /// List a NoteDetail by Company ID
        public List<NoteDetail> GetByNoteId(Int64 CompanyId, String NoteFor)
        {
            return db.NoteDetails.Where(C => C.ForeignKey == CompanyId && C.NoteFor == NoteFor).ToList();
        }

        /// Delete a NotaDetail Record by Company ID
        public void DeleteCompanyNotice(string ComapnyID)
        {
            var matchedNote = (from c in db.GetTable<NoteDetail>()
                               where c.ForeignKey == Convert.ToInt32(ComapnyID)
                               select c);
            try
            {
                if (matchedNote != null)
                {
                    foreach (var notedetail in matchedNote)
                    {
                        db.NoteDetails.DeleteOnSubmit(notedetail);
                    }

                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(Int64 ForeignKey, DAEnums.NoteForType NoteFor)
        {
            string NoteForName = DAEnums.GetNoteForTypeName(NoteFor);
            List<NoteDetail> objNotes = db.NoteDetails.Where(C => C.ForeignKey == ForeignKey && C.NoteFor == NoteForName).OrderByDescending(n => n.NoteID).OrderByDescending(n => n.CreateDate).ToList();

            db.NoteDetails.DeleteAllOnSubmit(objNotes);
            db.SubmitChanges();
        }


        //added by prashant 20th nov 2012
        public List<NoteDetail> GetNotesSendtoCustomer(DAEnums.NoteForType NoteFor, Int64 ForeignKey, string SpecificNotefor)
        {
            string NoteForName = DAEnums.GetNoteForTypeName(NoteFor);
            return db.NoteDetails.Where(C => C.NoteFor == NoteForName && C.SpecificNoteFor == SpecificNotefor && C.ForeignKey == ForeignKey).OrderByDescending(n => n.NoteID).ToList();
        }

        //added by prashant 21 Jan 2013
        public NoteDetail GetByNoteID(Int64 NoteID)
        {
            return db.NoteDetails.FirstOrDefault(C => C.NoteID == NoteID);
        }


        //added by prashant 21 Jan 2013
        public Int64 GetUserIdByNoteID(Int64 NoteID)
        {
            return (from n in db.NoteDetails
                    join s in db.Suppliers on n.ForeignKey equals s.SupplierID
                    select s.UserInfoID).FirstOrDefault();

        }

        //added by prashant 24 jan 2013
        public List<FUN_GetServiceTicketNoteResult> GetCustomerNotesByOrderID(Int64 OrderID, Int64 ServiceTicketID)
        {
            string NoteForName = DAEnums.GetNoteForTypeName(DAEnums.NoteForType.CACE);
            //var result = db.NoteDetails.Where(C => C.NoteFor == NoteForName && C.ForeignKey == OrderID).OrderByDescending(n => n.NoteID).ToList();
            var result = (from n in db.NoteDetails
                          join u in db.UserInformations on n.CreatedBy equals u.UserInfoID
                          where n.NoteFor == NoteForName && n.ForeignKey == OrderID
                          select new
                          {
                              FirstName = u.FirstName,
                              LastName = u.LastName,
                              NoteFor = NoteForName,
                              Notecontents = n.Notecontents,
                              CreateDate = n.CreateDate,
                              NoteID = n.NoteID,
                              ServiceTicketID = ServiceTicketID
                          }).ToList().Select(x => new FUN_GetServiceTicketNoteResult
                          {
                              FirstName = x.FirstName,
                              LastName = x.LastName,
                              NoteFor = NoteForName,
                              Notecontents = x.Notecontents,
                              CreateDate = x.CreateDate,
                              DateNTime = x.CreateDate.HasValue ? x.CreateDate.Value.ToString("MM/dd/yyyy") + "@" + x.CreateDate.Value.ToString("HH:mm") : "",
                              NoteID = x.NoteID,
                              ServiceTicketID = ServiceTicketID
                          }).ToList();
            return result;
        }

        //added by prashant 21 Jan 2013
        public List<NoteDetailResult> GetByNoteForAndForeignKey(Int64 ForeignKey, String NoteFor)
        {
            return (from n in db.NoteDetails
                    join u in db.UserInformations on n.CreatedBy equals u.UserInfoID
                    where n.NoteFor == NoteFor && n.ForeignKey == ForeignKey
                    select new
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        NoteFor = NoteFor,
                        Notecontents = n.Notecontents,
                        CreateDate = n.CreateDate,
                        NoteID = n.NoteID,
                    }).ToList().Select(x => new NoteDetailResult
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        NoteFor = NoteFor,
                        Notecontents = x.Notecontents,
                        CreateDate = x.CreateDate,
                        DateNTime = x.CreateDate.HasValue ? x.CreateDate.Value.ToString("ddd, MMM d, yyyy, hh:mm ") : "",
                        //Thu, Jan 21, 2013, 8:11 am
                        NoteID = x.NoteID,
                    }).OrderByDescending(x => x.NoteID).ToList();
        }

        public class NoteDetailResult
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string NoteFor { get; set; }
            public string Notecontents { get; set; }
            public DateTime? CreateDate { get; set; }
            public Int64 NoteID { get; set; }
            public string DateNTime { get; set; }
        }

        //added by prashant 21 Jan 2013
        public List<NoteDetailResult> GetFlaggedNotes(Int64 ForeignKey, String NoteFor)
        {
            return (from n in db.NoteDetails
                    join u in db.UserInformations on n.CreatedBy equals u.UserInfoID
                    join e in db.EquipmentMasters on n.ForeignKey equals e.EquipmentMasterID
                    where n.NoteFor == NoteFor && n.ForeignKey == ForeignKey && e.Flagged == true && n.SpecificNoteFor == "Flagged"
                    select new
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        NoteFor = NoteFor,
                        Notecontents = n.Notecontents,
                        CreateDate = n.CreateDate,
                        NoteID = n.NoteID,
                    }).ToList().Select(x => new NoteDetailResult
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        NoteFor = NoteFor,
                        Notecontents = x.Notecontents,
                        CreateDate = x.CreateDate,
                        DateNTime = x.CreateDate.HasValue ? x.CreateDate.Value.ToString("ddd, MMM d, yyyy, hh:mm tt") : "",
                        //Thu, Jan 21, 2013, 8:11 am
                        NoteID = x.NoteID,
                    }).OrderByDescending(x => x.NoteID).ToList();
        }
    }
}
