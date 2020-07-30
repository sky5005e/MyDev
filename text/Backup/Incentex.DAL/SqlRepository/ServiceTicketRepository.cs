using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class ServiceTicketRepository : RepositoryBase
    {
        private Boolean TicketInsertionDone1 = true;
        private Boolean TicketInsertionDone2 = true;
        private Boolean NoteInsertionDone = true;

        private readonly Object SyncRootTicket1 = new Object();
        private readonly Object SyncRootTicket2 = new Object();
        private readonly Object SyncRootNote = new Object();

        public List<vw_ServiceTicket> GetAll()
        {
            return db.vw_ServiceTickets.Where(le => le.IsDeleted == false).ToList();
        }

        public vw_ServiceTicket GetFirstByID(Int64 ServiceTicketID)
        {
            return db.vw_ServiceTickets.FirstOrDefault(le => le.ServiceTicketID == ServiceTicketID && le.IsDeleted == false);
        }

        public ServiceTicket GetByTicketID(Int64 ServiceTicketID)
        {
            return db.ServiceTickets.FirstOrDefault(le => le.ServiceTicketID == ServiceTicketID && le.IsDeleted == false);
        }

        /// <summary>
        /// Get Service Ticket Details 
        /// </summary>
        /// <param name="ServiceTicketID"></param>
        /// <param name="NoteFor"></param>
        /// <param name="SpecificNoteFor"></param>
        /// <returns></returns>
        public List<GetTicketDetailsForCACEResult> GetTicketDetailsForCACE(Int64 ServiceTicketID, String NoteFor, String SpecificNoteFor)
        {
            return db.GetTicketDetailsForCACE(ServiceTicketID, NoteFor, SpecificNoteFor).ToList();
        }


        //Delete By Id
        public void DeleteById(Int64 ServiceTicketID, Int64? UserInfoID)
        {
            //DeleteNoteReadFlags(ServiceTicketID);
            //DeleteNoteRecipientsByServiceTicketID(ServiceTicketID);
            //NotesHistoryRepository objNoteRepo = new NotesHistoryRepository();
            //objNoteRepo.Delete(ServiceTicketID, DAEnums.NoteForType.ServiceTicketCAs);
            //objNoteRepo.Delete(ServiceTicketID, DAEnums.NoteForType.ServiceTicketIEs);
            //objNoteRepo.Delete(ServiceTicketID, DAEnums.NoteForType.ServiceTicketSupps);
            DeleteSubOwnerByServiceTicketID(ServiceTicketID, UserInfoID);
            //DeleteTicketFlags(ServiceTicketID);

            ServiceTicket objServiceTicket = db.ServiceTickets.FirstOrDefault(le => le.ServiceTicketID == ServiceTicketID);
            try
            {
                if (objServiceTicket != null)
                {
                    objServiceTicket.IsDeleted = true;
                    objServiceTicket.DeletedBy = UserInfoID;
                    objServiceTicket.DeletedDate = DateTime.Now;
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectServiceTicketHistoryPerEmployeeResult> SelectServiceTicketHistoryByEmployee(Int64? UserInfoId, Boolean? QuickView)
        {
            return db.SelectServiceTicketHistoryPerEmployee(UserInfoId, QuickView).ToList();
        }

        public List<SelectServiceTicketHistoryPerEmployeeResult> SelectRecentlyUpdatedServiceTicketForCACE(Int64? UserInfoID)
        {
            return db.SelectRecentlyUpdatedServiceTicketForCACE(UserInfoID).ToList();
        }

        public List<GetServiceTicketSubOwnerDetailsResult> GetSubOwnersByTicketID(Int64 ServiceTicketID)
        {
            return db.GetServiceTicketSubOwnerDetails(ServiceTicketID).ToList();
        }

        public void DeleteSubOwnerTicketIDNUserID(Int64 TicketID, Int64? SubOwnerID, Int64? UserInfoID)
        {
            DeleteTodoByServiceTicketIDNSubOwnerID(TicketID, SubOwnerID, UserInfoID);

            ServiceTicketSubOwnerDetail objSubOwnerDetail = db.ServiceTicketSubOwnerDetails.FirstOrDefault(le => le.ServiceTicketID == TicketID && le.SubOwnerID == SubOwnerID && le.IsDeleted == false);
            try
            {
                if (objSubOwnerDetail != null)
                {
                    objSubOwnerDetail.IsDeleted = true;
                    objSubOwnerDetail.DeletedBy = UserInfoID;
                    objSubOwnerDetail.DeletedDate = DateTime.Now;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteSubOwnerByServiceTicketID(Int64 ServiceTicketID, Int64? UserInfoID)
        {
            DeleteTodoByServiceTicketID(ServiceTicketID, UserInfoID);

            List<ServiceTicketSubOwnerDetail> lstSubOwnerDetail = db.ServiceTicketSubOwnerDetails.Where(le => le.ServiceTicketID == ServiceTicketID).ToList();
            try
            {
                if (lstSubOwnerDetail != null)
                {
                    foreach (ServiceTicketSubOwnerDetail objSubOwner in lstSubOwnerDetail)
                    {
                        objSubOwner.IsDeleted = true;
                        objSubOwner.DeletedBy = UserInfoID;
                        objSubOwner.DeletedDate = DateTime.Now;
                    }

                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ServiceTicketToDoDetail> GetTodoListByTicketAndOwnerID(Int64 TicketID, Int64 OwnerID)
        {
            return db.ServiceTicketToDoDetails.Where(le => le.ServiceTicketID == TicketID && le.ToDoOwnerID == OwnerID && le.IsDeleted == false).ToList();
        }

        public void DeleteTodoByToDoID(Int64 ToDoID, Int64? UserInfoID)
        {
            ServiceTicketToDoDetail objTodo = db.ServiceTicketToDoDetails.FirstOrDefault(le => le.ToDoID == ToDoID);
            try
            {
                if (objTodo != null)
                {
                    objTodo.IsDeleted = true;
                    objTodo.DeletedBy = UserInfoID;
                    objTodo.DeletedDate = DateTime.Now;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTodoBySubOwnerDetailID(Int64 SubOwnerDetailID, Int64? UserInfoID)
        {
            List<ServiceTicketToDoDetail> lstToDo = db.ServiceTicketToDoDetails.Where(le => le.SubOwnerDetailID == SubOwnerDetailID).ToList();
            try
            {
                if (lstToDo != null)
                {
                    foreach (ServiceTicketToDoDetail objTodo in lstToDo)
                    {
                        objTodo.IsDeleted = true;
                        objTodo.DeletedBy = UserInfoID;
                        objTodo.DeletedDate = DateTime.Now;
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTodoByServiceTicketID(Int64 ServiceTicketID, Int64? UserInfoID)
        {
            List<ServiceTicketToDoDetail> lstToDo = db.ServiceTicketToDoDetails.Where(le => le.ServiceTicketID == ServiceTicketID).ToList();
            try
            {
                if (lstToDo != null)
                {
                    foreach (ServiceTicketToDoDetail objTodo in lstToDo)
                    {
                        objTodo.IsDeleted = true;
                        objTodo.DeletedBy = UserInfoID;
                        objTodo.DeletedDate = DateTime.Now;
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTodoByServiceTicketIDNSubOwnerID(Int64 ServiceTicketID, Int64? SubOwnerID, Int64? UserInfoID)
        {
            List<ServiceTicketToDoDetail> lstToDo = db.ServiceTicketToDoDetails.Where(le => le.ServiceTicketID == ServiceTicketID && le.ToDoOwnerID == SubOwnerID).ToList();
            try
            {
                if (lstToDo != null)
                {
                    foreach (ServiceTicketToDoDetail objTodo in lstToDo)
                    {
                        objTodo.IsDeleted = true;
                        objTodo.DeletedBy = UserInfoID;
                        objTodo.DeletedDate = DateTime.Now;
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void DeleteNoteRecipientsByServiceTicketID(Int64 ServiceTicketID)
        //{
        //    List<ServiceTicketNoteRecipientsDetail> objNoteRecipientList = db.ServiceTicketNoteRecipientsDetails.Where(le => le.ServiceTicketID == ServiceTicketID).ToList();
        //    try
        //    {
        //        if (objNoteRecipientList != null)
        //        {
        //            db.ServiceTicketNoteRecipientsDetails.DeleteAllOnSubmit(objNoteRecipientList);
        //            db.SubmitChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void DeleteNoteReadFlags(Int64 ServiceTicketID)
        //{
        //    List<ServiceTicketNoteReadDetail> objNoteReadFlagList = db.ServiceTicketNoteReadDetails.Where(le => le.ServiceTicketID == ServiceTicketID).ToList();
        //    try
        //    {
        //        if (objNoteReadFlagList != null)
        //        {
        //            db.ServiceTicketNoteReadDetails.DeleteAllOnSubmit(objNoteReadFlagList);
        //            db.SubmitChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void DeleteTicketFlags(Int64 ServiceTicketID)
        //{
        //    List<ServiceTicketFlagDetail> lstTicketFlags = db.ServiceTicketFlagDetails.Where(le => le.ServiceTicketID == ServiceTicketID).ToList();
        //    try
        //    {
        //        if (lstTicketFlags != null)
        //        {
        //            db.ServiceTicketFlagDetails.DeleteAllOnSubmit(lstTicketFlags);
        //            db.SubmitChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void DeleteTicketFlag(Int64 TicketFlagID)
        {
            try
            {
                ServiceTicketFlagDetail objFlag = db.ServiceTicketFlagDetails.FirstOrDefault(le => le.TicketFlagID == TicketFlagID);
                if (objFlag != null)
                {
                    db.ServiceTicketFlagDetails.DeleteOnSubmit(objFlag);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTicketFlag(Int64 ServiceTicketID, Int64 UserInfoID)
        {
            try
            {
                ServiceTicketFlagDetail objFlag = db.ServiceTicketFlagDetails.FirstOrDefault(le => le.ServiceTicketID == ServiceTicketID && le.CreatedBy == UserInfoID);
                if (objFlag != null)
                {
                    db.ServiceTicketFlagDetails.DeleteOnSubmit(objFlag);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ServiceTicketToDoDetail GetToDoByToDoID(Int64 ToDoID)
        {
            return db.ServiceTicketToDoDetails.FirstOrDefault(le => le.ToDoID == ToDoID && le.IsDeleted == false);
        }

        private List<GetServiceTicketEmailNotificationPartiesResult> GetEmailNotificationParties(Int64? CompanyID, Int64 TicketID)
        {
            return db.GetServiceTicketEmailNotificationParties(CompanyID, TicketID).ToList();
        }

        public List<GetServiceTicketOpenedByResult> GetServiceTicketOpenedByFromCompanyID(Int64 CompanyID)
        {
            return db.GetServiceTicketOpenedBy(CompanyID).ToList();
        }

        public List<GetServiceTicketNoteRecipientsResult> GetServiceTicketNoteRecipients(Int64 ServiceTicketID)
        {
            return db.GetServiceTicketNoteRecipients(ServiceTicketID).ToList();
        }

        public List<GetCompanyEmployeeContactByCompanyIDResult> GetServiceTicketContactByCompanyID(Int64 companyID, Int64 userInfoID)
        {
            return db.GetCompanyEmployeeContactByCompanyID(companyID, userInfoID).ToList();
        }

        public List<GetServiceTicketContactByCACompanyAndWorkGroupResult> GetServiceTicketContactByCACompanyAndWorkGroup(Int64 UserInfoID)
        {
            return db.GetServiceTicketContactByCACompanyAndWorkGroup(UserInfoID).ToList();
        }

        public List<GetServiceTicketWorkGroupsForCAResult> GetServiceTicketWorkGroupsForCA(Int64 UserInfoID)
        {
            return db.GetServiceTicketWorkGroupsForCA(UserInfoID).ToList();
        }

        public IMultipleResults GetServiceTicketActivities(Int64? CompanyID, Int64? ContactID, Int64? OpenedByID, Int64? StatusID, Int64? OwnerID, String TicketName, String TicketNumber, String DateNeeded, Int64? SupplierID, Int64? TypeOfRequestID, String KeyWord, Int64? SubOwnerID, String FromDate, String ToDate, String SpecificNoteFor, Int64? UserInfoID)
        {
            return db.GetServiceTicketActivities(CompanyID, ContactID, OpenedByID, StatusID, OwnerID, TicketName, TicketNumber, DateNeeded, SupplierID, TypeOfRequestID, KeyWord, SubOwnerID, FromDate, ToDate, SpecificNoteFor, UserInfoID);
        }

        public IMultipleResults GetServiceTicketActivitiesNotes(String SpecificNoteFor, Int64? UserInfoID, Int64 TicketID)
        {
            return db.GetServiceTicketActivitiesNotes(SpecificNoteFor, UserInfoID, TicketID);
        }

        public List<ServiceTicketsSearchResult> SelectUnReadNotesBySearchCriteria(Int64? UserInfoID, Int64? ServiceTicketID)
        {
            return db.SelectUnReadNotesBySearchCriteria(UserInfoID, ServiceTicketID).ToList();
        }

        public List<ServiceTicketsSearchResult> SelectServiceTicketsBySearchCriteria(Int64? CompanyID, Int64? ContactID, Int64? OpenedByID, Int64? StatusID, Int64? OwnerID, String TicketName, String TicketNumber, String DateNeeded, Int64? SupplierID, Int64? TypeOfRequestID, String KeyWord, Int64? SubOwnerID, String FromDate, String ToDate, Int64? UserInfoID, Boolean? NoActivity)
        {
            return db.SelectServiceTicketsBySearchCriteria(CompanyID, ContactID, OpenedByID, StatusID, OwnerID, TicketName, TicketNumber, DateNeeded, SupplierID, TypeOfRequestID, KeyWord, SubOwnerID, FromDate, ToDate, UserInfoID, NoActivity).ToList();
        }

        public List<ServiceTicketsSearchResult> SelectServiceTicketsBySearchCriteriaForCA(Int64 CAUserInfoID, String TicketName, String TicketNumber, Int64? WorkGroupID, Int64? ContactID, Int64? StatusID, Int64? OwnerID, String KeyWord, String AllowedToSearch)
        {
            return db.SelectServiceTicketsBySearchCriteriaForCA(CAUserInfoID, TicketName, TicketNumber, WorkGroupID, ContactID, StatusID, OwnerID, KeyWord, AllowedToSearch).ToList();
        }

        public List<ServiceTicketsSearchResult> SelectServiceTicketAssociatedWithIEs(Int64? UserInfoID)
        {
            return db.SelectServiceTicketAssociatedWithIEs(UserInfoID).ToList();
        }

        public List<ServiceTicketsSearchResult> SelectRecentlyUpdatedServiceTicket(Int64? UserInfoID)
        {
            return db.SelectRecentlyUpdatedServiceTicket(UserInfoID).ToList();
        }

        public Int64 InsertTicket(Int64? UserInfoID, Int64? CompanyID, Int64? UserType, String TicketName, String Question, Int64? ReasonID, String Email, out Int64? OwnerID)
        {
            lock (SyncRootTicket1)
            {
                try
                {
                    if (!TicketInsertionDone1)
                    {
                        Monitor.Wait(SyncRootTicket1);
                    }

                    LookupRepository objLookupRepo = new LookupRepository();
                    UserInformationRepository objUserRepo = new UserInformationRepository();

                    ServiceTicket objServiceTicket = new ServiceTicket();

                    TicketInsertionDone1 = false;

                    objServiceTicket.CreatedBy = UserInfoID;
                    objServiceTicket.CreatedDate = DateTime.Now;
                    objServiceTicket.CompanyID = CompanyID;
                    objServiceTicket.ContactID = UserInfoID;
                    objServiceTicket.DatePromised = System.DateTime.Now;

                    if (Question.Length > 2500)
                    {
                        Question = Question.Substring(0, 2500);
                    }

                    objServiceTicket.ReasonID = ReasonID;
                    objServiceTicket.ServiceTicketDetails = Question;
                    objServiceTicket.ServiceTicketName = TicketName;

                    //all tickets other than company employee tickets are directed to Amanda Steed
                    if (UserType != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                    {
                        String DefaultOwner = Convert.ToString(ConfigurationManager.AppSettings["SupportTicketDefaultOwner"]);                       
                        objServiceTicket.ServiceTicketOwnerID = objUserRepo.GetUserInfoIDOfIcentexEmployeeByName(DefaultOwner.Substring(0, DefaultOwner.IndexOf(' ')), DefaultOwner.Substring(DefaultOwner.IndexOf(' ') + 1));
                    }
                    //all company employee tickets are directed to Kerri Tropf
                    else
                    {
                        String DefaultOwner = Convert.ToString(ConfigurationManager.AppSettings["SupportTicketForEmployeesDefaultOwner"]);
                        objServiceTicket.ServiceTicketOwnerID = objUserRepo.GetUserInfoIDOfIcentexEmployeeByName(DefaultOwner.Substring(0, DefaultOwner.IndexOf(' ')), DefaultOwner.Substring(DefaultOwner.IndexOf(' ') + 1));
                    }

                    objServiceTicket.TicketStatusID = objLookupRepo.GetIdByLookupNameNLookUpCode("Open", "ServiceTicketStatus");
                    objServiceTicket.TicketEmail = Email;

                    if (UserInfoID != null && UserInfoID > 0)
                    {
                        CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetByUserInfoId(Convert.ToInt64(UserInfoID));
                        if (objCompanyEmployee != null)
                        {
                            objServiceTicket.WorkGroupID = objCompanyEmployee.WorkgroupID;
                            objServiceTicket.BaseStationID = objCompanyEmployee.BaseStation;
                        }
                    }

                    this.Insert(objServiceTicket);
                    this.SubmitChanges();
                    objServiceTicket.ServiceTicketNumber = Convert.ToString(objServiceTicket.ServiceTicketID);
                    this.SubmitChanges();

                    OwnerID = objServiceTicket.ServiceTicketOwnerID;

                    InsertNotificationParties(objServiceTicket.ServiceTicketID, objServiceTicket.CompanyID, objServiceTicket.CreatedBy);
                    return objServiceTicket.ServiceTicketID;
                }
                finally
                {
                    TicketInsertionDone1 = true;
                    Monitor.Pulse(SyncRootTicket1);
                }
            }
        }

        public Int64 InsertTicket(Int64? UserInfoID, Int64? CompanyID, Int64? ContactID, Int64? SupplierID, Int64? OwnerID, String DatePromised, String TicketName, String Question)
        {
            lock (SyncRootTicket2)
            {
                try
                {
                    if (!TicketInsertionDone2)
                    {
                        Monitor.Wait(SyncRootTicket2);
                    }

                    LookupRepository objLookupRepo = new LookupRepository();
                    ServiceTicket objServiceTicket = new ServiceTicket();

                    TicketInsertionDone2 = false;

                    objServiceTicket.CreatedBy = UserInfoID;
                    objServiceTicket.CreatedDate = DateTime.Now;
                    objServiceTicket.CompanyID = CompanyID;
                    objServiceTicket.ContactID = ContactID;

                    if (!String.IsNullOrEmpty(DatePromised))
                    {
                        objServiceTicket.DatePromised = DateTime.ParseExact(DatePromised,
                                                        "MM/dd/yyyy",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.AssumeLocal);
                    }

                    if (Question.Length > 2500)
                    {
                        Question = Question.Substring(0, 2500);
                    }

                    objServiceTicket.ServiceTicketDetails = Question;
                    objServiceTicket.ServiceTicketName = TicketName;
                    objServiceTicket.ServiceTicketOwnerID = OwnerID;
                    objServiceTicket.SupplierID = SupplierID;
                    objServiceTicket.TicketStatusID = objLookupRepo.GetIdByLookupNameNLookUpCode("Open", "ServiceTicketStatus");

                    if (ContactID != null && ContactID > 0)
                    {
                        CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetByUserInfoId(Convert.ToInt64(ContactID));
                        if (objCompanyEmployee != null)
                        {
                            objServiceTicket.WorkGroupID = objCompanyEmployee.WorkgroupID;
                            objServiceTicket.BaseStationID = objCompanyEmployee.BaseStation;
                        }
                    }

                    this.Insert(objServiceTicket);
                    this.SubmitChanges();
                    objServiceTicket.ServiceTicketNumber = Convert.ToString(objServiceTicket.ServiceTicketID);
                    this.SubmitChanges();

                    InsertNotificationParties(objServiceTicket.ServiceTicketID, objServiceTicket.CompanyID, objServiceTicket.CreatedBy);

                    return objServiceTicket.ServiceTicketID;
                }
                finally
                {
                    TicketInsertionDone2 = true;
                    Monitor.Pulse(SyncRootTicket2);
                }
            }
        }

        private void InsertNotificationParties(Int64 TicketID, Int64? CompanyID, Int64? UserInfoID)
        {
            List<GetServiceTicketEmailNotificationPartiesResult> lstParties = GetEmailNotificationParties(CompanyID, TicketID);
            foreach (GetServiceTicketEmailNotificationPartiesResult objParty in lstParties)
            {
                ServiceTicketNoteRecipientsDetail objSTNRD = new ServiceTicketNoteRecipientsDetail();
                objSTNRD.CreatedBy = UserInfoID;
                objSTNRD.CreatedDate = DateTime.Now;
                objSTNRD.ServiceTicketID = TicketID;
                objSTNRD.SubscriptionFlag = false;
                objSTNRD.UserInfoID = objParty.UserInfoID;

                this.Insert(objSTNRD);
                this.SubmitChanges();
            }
        }

        public void InsertTicketNote(Int64 ServiceTicketID, Int64? UserInfoID, String Contents, String NoteFor, String SpecificNoteFor)
        {
            lock (SyncRootNote)
            {
                try
                {
                    if (!NoteInsertionDone)
                    {
                        Monitor.Wait(SyncRootNote);
                    }

                    NoteDetail objComNot = new NoteDetail();

                    NoteInsertionDone = false;

                    objComNot.Notecontents = Contents;
                    objComNot.NoteFor = NoteFor;
                    objComNot.SpecificNoteFor = SpecificNoteFor;
                    objComNot.ForeignKey = ServiceTicketID;
                    objComNot.CreateDate = System.DateTime.Now;
                    objComNot.CreatedBy = UserInfoID;
                    objComNot.UpdateDate = System.DateTime.Now;
                    objComNot.UpdatedBy = UserInfoID;

                    this.Insert(objComNot);
                    this.SubmitChanges();

                    ServiceTicket objServiceTicket = this.GetByTicketID(ServiceTicketID);
                    LookupRepository objLookupRepo = new LookupRepository();
                    if (objServiceTicket != null && objServiceTicket.TicketStatusID == objLookupRepo.GetIdByLookupNameNLookUpCode("Closed", "ServiceTicketStatus"))
                    {
                        objServiceTicket.TicketStatusID = objLookupRepo.GetIdByLookupNameNLookUpCode("Open", "ServiceTicketStatus");
                        this.SubmitChanges();
                    }

                    if (SpecificNoteFor != "IEActivity")
                    {
                        List<GetServiceTicketUsersForNoteFlagResult> lstUsersForNoteFlag = new List<GetServiceTicketUsersForNoteFlagResult>();
                        if (!String.IsNullOrEmpty(SpecificNoteFor))
                        {
                            lstUsersForNoteFlag = GetUsersForNoteFlag(ServiceTicketID).Where(le => le.UserInfoID != UserInfoID && (le.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin) || le.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))).ToList();
                        }
                        else
                        {
                            lstUsersForNoteFlag = GetUsersForNoteFlag(ServiceTicketID).Where(le => le.UserInfoID != UserInfoID).ToList();
                        }

                        foreach (GetServiceTicketUsersForNoteFlagResult User in lstUsersForNoteFlag)
                        {
                            ServiceTicketNoteReadDetail objReadFlag = new ServiceTicketNoteReadDetail();
                            objReadFlag.CreatedBy = UserInfoID;
                            objReadFlag.CreatedDate = System.DateTime.Now;
                            objReadFlag.FlagFor = User.UserInfoID;
                            objReadFlag.NoteID = objComNot.NoteID;
                            objReadFlag.ReadFlag = false;
                            objReadFlag.ServiceTicketID = ServiceTicketID;
                            this.Insert(objReadFlag);
                            this.SubmitChanges();
                        }
                    }
                }
                finally
                {
                    NoteInsertionDone = true;
                    Monitor.Pulse(SyncRootNote);
                }
            }
        }

        public List<GetServiceTicketUsersForNoteFlagResult> GetUsersForNoteFlag(Int64 TicketID)
        {
            return db.GetServiceTicketUsersForNoteFlag(TicketID).ToList();
        }

        public List<FUN_GetServiceTicketNoteResult> GetUnreadNotes(Int64 ServiceTicketID, Int64? UserInfoID)
        {
            List<FUN_GetServiceTicketNoteResult> lstUnReadNotes = new List<FUN_GetServiceTicketNoteResult>();
            List<ServiceTicketNoteReadDetail> objNoteReadDetails = db.ServiceTicketNoteReadDetails.Where(le => le.ServiceTicketID == ServiceTicketID && le.FlagFor == UserInfoID && le.ReadFlag == false).ToList();
            foreach (ServiceTicketNoteReadDetail objRead in objNoteReadDetails)
            {
                FUN_GetServiceTicketNoteResult objUnReadNote = db.FUN_GetServiceTicketNote(UserInfoID).FirstOrDefault(le => le.NoteID == objRead.NoteID);
                if (objUnReadNote != null)
                    lstUnReadNotes.Add(objUnReadNote);
            }

            return lstUnReadNotes;
        }

        public ServiceTicketNoteRecipientsDetail GetNoteRecipient(Int64 RecipientsDetailID)
        {
            return db.ServiceTicketNoteRecipientsDetails.FirstOrDefault(le => le.RecipientsDetailID == RecipientsDetailID);
        }

        public ServiceTicketNoteRecipientsDetail GetNoteRecipient(Int64 TicketID, Int64? UserInfoID)
        {
            return db.ServiceTicketNoteRecipientsDetails.FirstOrDefault(le => le.ServiceTicketID == TicketID && UserInfoID == null ? le.UserInfoID.Equals(null) : le.UserInfoID == UserInfoID);
        }

        public ServiceTicketFlagDetail GetTicketFlag(Int64 TicketID, Int64 UserInfoID)
        {
            return db.ServiceTicketFlagDetails.FirstOrDefault(le => le.ServiceTicketID == TicketID && le.CreatedBy == UserInfoID);
        }

        public List<vw_ServiceTicketNoteRecipient> GetNoteRecipientsByTicketID(Int64 TicketID)
        {
            return db.vw_ServiceTicketNoteRecipients.Where(le => le.ServiceTicketID == TicketID).ToList();
        }

        public void UpdateNoteReadFlag(Int64 ServiceTicketID, Int64? UserInfoID, List<FUN_GetServiceTicketNoteResult> lstNotes)
        {
            foreach (FUN_GetServiceTicketNoteResult objNote in lstNotes)
            {
                ServiceTicketNoteReadDetail objRead = db.ServiceTicketNoteReadDetails.FirstOrDefault(le => le.ServiceTicketID == ServiceTicketID && le.FlagFor == UserInfoID && le.NoteID == objNote.NoteID);
                objRead.ReadFlag = true;
            }
            this.SubmitChanges();
        }

        public String TrailingNotes(Int64 TicketID, Int32 NoteType, Boolean? OnlyInternalNotes, String NewLineChar)
        {
            String TrailingNotes = String.Empty;

            List<NoteDetail> objList = new List<NoteDetail>();

            if (NoteType == 2)
            {
                if (OnlyInternalNotes == null)
                {
                    objList = db.NoteDetails.Where(le => le.ForeignKey == TicketID && le.NoteFor == "ServiceTicketIEs" && (le.SpecificNoteFor == "IEInternalNotes" || le.SpecificNoteFor == "IEActivity")).OrderByDescending(le => le.ForeignKey).ThenByDescending(le => le.NoteID).ToList();
                }
                else if (Convert.ToBoolean(OnlyInternalNotes))
                {
                    objList = db.NoteDetails.Where(le => le.ForeignKey == TicketID && le.NoteFor == "ServiceTicketIEs" && le.SpecificNoteFor == "IEInternalNotes").OrderByDescending(le => le.ForeignKey).ThenByDescending(le => le.NoteID).ToList();
                    //added on 29th Jan 2013 by prashant
                    //Merge Notes posted from Summary Order View
                    OrderConfirmationRepository objOrderRepos = new OrderConfirmationRepository();
                    NotesHistoryRepository objNotesRepos = new NotesHistoryRepository();
                    var order = objOrderRepos.GetOrderByAssociateTicketID(TicketID);
                    if (order != null)
                    {
                        var objOrderNote = db.NoteDetails.Where(le => le.ForeignKey == order.OrderID && le.NoteFor == DAEnums.GetNoteForTypeName(DAEnums.NoteForType.OrderDetailsIEs) && le.ForeignKey == order.OrderID && le.SpecificNoteFor == "IEInternalNotes").OrderByDescending(le => le.CreateDate).ToList();
                        objList.AddRange(objOrderNote);
                    }
                    //added on 29th Jan 2013 by prashant
                    //Merge Notes posted from Summary Order View
                }
                else if (!Convert.ToBoolean(OnlyInternalNotes))
                {
                    objList = db.NoteDetails.Where(le => le.ForeignKey == TicketID && (le.NoteFor == "ServiceTicketCAs" || le.NoteFor == "ServiceTicketSupps" || le.NoteFor == "ServiceTicketIEs") && (le.SpecificNoteFor == null || le.SpecificNoteFor == "IEInternalNotes")).OrderByDescending(le => le.ForeignKey).ThenByDescending(le => le.NoteID).ToList();
                    //added on 28th Jan 2013 by prashant
                    //Merge Notes posted from Summary Order View
                    OrderConfirmationRepository objOrderRepos = new OrderConfirmationRepository();
                    NotesHistoryRepository objNotesRepos = new NotesHistoryRepository();
                    var order = objOrderRepos.GetOrderByAssociateTicketID(TicketID);
                    if (order != null)
                    {
                        var objOrderNote = db.NoteDetails.Where(le => le.ForeignKey == order.OrderID && (le.NoteFor == DAEnums.GetNoteForTypeName(DAEnums.NoteForType.CACE) || (le.NoteFor == DAEnums.GetNoteForTypeName(DAEnums.NoteForType.OrderDetailsIEs) && le.SpecificNoteFor == "IEInternalNotes")) && le.ForeignKey == order.OrderID).OrderByDescending(le => le.CreateDate).ToList();
                        objList.AddRange(objOrderNote);
                    }
                    //added on 28th Jan 2013 by prashant
                    //Merge Notes posted from Summary Order View
                }
            }
            else if (NoteType == 1)
            {
                objList = db.NoteDetails.Where(le => le.ForeignKey == TicketID && le.NoteFor == "ServiceTicketCAs" && le.SpecificNoteFor.Equals(null)).OrderByDescending(le => le.ForeignKey).ThenByDescending(le => le.NoteID).ToList();

                //added on 28th Jan 2013 by prashant
                //Merge Notes posted from Summary Order View
                OrderConfirmationRepository objOrderRepos = new OrderConfirmationRepository();
                NotesHistoryRepository objNotesRepos = new NotesHistoryRepository();
                var order = objOrderRepos.GetOrderByAssociateTicketID(TicketID);
                if (order != null)
                {
                    var objOrderNote = db.NoteDetails.Where(le => le.ForeignKey == order.OrderID && le.NoteFor == DAEnums.GetNoteForTypeName(DAEnums.NoteForType.CACE) && le.ForeignKey == order.OrderID).OrderByDescending(le => le.CreateDate).ToList();
                    objList.AddRange(objOrderNote);
                }
                //added on 28th Jan 2013 by prashant
                //Merge Notes posted from Summary Order View
            }
            else if (NoteType == 3)
            {
                objList = db.NoteDetails.Where(le => le.ForeignKey == TicketID && le.NoteFor == "ServiceTicketSupps" && le.SpecificNoteFor.Equals(null)).OrderByDescending(le => le.ForeignKey).ThenByDescending(le => le.NoteID).ToList();
            }



            if (objList.Count > 0)
            {
                StringBuilder sbTrailingNotes = new StringBuilder();
                foreach (NoteDetail obj in objList.OrderByDescending(le => le.CreateDate).ToList())
                {
                    sbTrailingNotes.Append(obj.Notecontents);
                    sbTrailingNotes.Append(NewLineChar + NewLineChar);
                    UserInformationRepository objUserRepo = new UserInformationRepository();
                    UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);
                    if (objUser != null)
                    {
                        sbTrailingNotes.Append(objUser.FirstName + " " + objUser.LastName + "   ");
                    }
                    sbTrailingNotes.Append(Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy"));
                    sbTrailingNotes.Append(" @ " + Convert.ToDateTime(obj.CreateDate).ToString("HH:mm") + NewLineChar);
                    sbTrailingNotes.Append("______________________________________________________________________________");
                    sbTrailingNotes.Append(NewLineChar + NewLineChar);
                }

                TrailingNotes = sbTrailingNotes.ToString();
            }

            return TrailingNotes;
        }

        public List<GetServiceTicketOwnersForCAResult> GetServiceTicketOwnersForCA(Int64 UserInfoID)
        {
            return db.GetServiceTicketOwnersForCA(UserInfoID).ToList();
        }

        public Int64 GetUserInfoIDByCompanyEmployeeID(Int64 CompanyEmployeeID)
        {
            return Convert.ToInt64(db.FUN_GetUserInfoIDByCompanyEmployeeID(CompanyEmployeeID));
        }

        public Int32 GetServiceTicketUpdatedCountCACE(Int64 UserInfoID)
        {
            Int32 UpdatedCount = 0;
            GetServiceTicketUpdatedCountCACEResult objCount = db.GetServiceTicketUpdatedCountCACE(UserInfoID).FirstOrDefault();
            if (objCount != null)
                UpdatedCount = Convert.ToInt32(objCount.UpdatedTicketsCount);
            return UpdatedCount;
        }

        public Int32 GetServiceTicketUpdatedCountIESA(Int64 UserInfoID)
        {
            Int32 UpdatedCount = 0;
            GetServiceTicketUpdatedCountIESAResult objCount = db.GetServiceTicketUpdatedCountIESA(UserInfoID).FirstOrDefault();
            if (objCount != null)
                UpdatedCount = Convert.ToInt32(objCount.UpdatedTicketsCount);
            return UpdatedCount;
        }
    }
}