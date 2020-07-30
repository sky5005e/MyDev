using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace Incentex.DAL.SqlRepository
{
    public class AssetMgtRepository : RepositoryBase
    {
        #region EquipmentMaster

        IQueryable<EquipmentMaster> GetAllQuery()
        {
            IQueryable<EquipmentMaster> qry = from c in db.EquipmentMasters
                                              orderby c.EquipmentMasterID
                                              select c;
            return qry;
        }

        public IQueryable<EquipmentMaster> GetAll()
        {
            var ListView = GetAllQuery();
            return ListView;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <returns></returns>
        public EquipmentMaster GetAllEquipments()
        {
            // IQueryable<EquipmentMaster> qry = GetAllQuery();
            //EquipmentMaster obj = GetSingle(qry.ToList());

            EquipmentMaster obj = (from c in db.EquipmentMasters
                                   orderby c.EquipmentMasterID
                                   select c).FirstOrDefault();
            return obj;
        }

        public List<GetEquipmentsResult> GetEquipmentsDetail(string EquipmentTypeID, string EquipmentID, string BaseStationID, string Status, string EquipmentMasterID, string Flagged, string GSEDepartmentID, Int64 CompanyID, Int64 VEUserInfoID)
        {
            var objlist = db.GetEquipments(EquipmentTypeID, EquipmentID, BaseStationID, Status, EquipmentMasterID, Flagged, GSEDepartmentID, VEUserInfoID).ToList();
            if (CompanyID != 0)
            {
                objlist = objlist.Where(q => q.CompanyID == CompanyID).ToList();
            }
            //if (WorkgroupID != 0)
            //{
            //    objlist = objlist.Where(q => q.WorkgroupID == WorkgroupID).ToList();
            //}
            return objlist.ToList<GetEquipmentsResult>();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public int GetEquipmentCount(Int64 EquipmentMasterID)
        {
            //IQueryable<EquipmentMaster> qry = GetAllQuery().Where(e => e.EquipmentMasterID == EquipmentMasterID);
            //return qry.Count();

            int qry = (from c in db.EquipmentMasters
                       where c.EquipmentMasterID == EquipmentMasterID
                       select c.EquipmentMasterID).Count();
            return qry;
        }

        public void Delete(Int64 EquipmentMasterID)
        {
            var matched = (from c in db.GetTable<EquipmentMaster>()
                           where c.EquipmentMasterID == Convert.ToInt64(EquipmentMasterID)
                           select c).SingleOrDefault();
            try
            {
                if (matched != null)
                {
                    db.EquipmentMasters.DeleteOnSubmit(matched);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public EquipmentMaster GetById(Int64 EquipmentMasterID)
        {
            //IQueryable<EquipmentMaster> qry = GetAllQuery().Where(s => s.EquipmentMasterID == EquipmentMasterID);
            //EquipmentMaster obj = GetSingle(qry.ToList());

            EquipmentMaster obj = (from c in db.EquipmentMasters
                                   where c.EquipmentMasterID == EquipmentMasterID
                                   select c).FirstOrDefault();
            return obj;
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentTypeID"></param>
        /// <param name="EquipmentID"></param>
        /// <param name="BaseStationID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public EquipmentMaster GetSearchResult(Int64 EquipmentTypeID, string EquipmentID, Int64 BaseStationID, Int64 Status)
        {
            //IQueryable<EquipmentMaster> qry = GetAllQuery();

            //if (EquipmentTypeID != null)
            //{
            //    qry = qry.Where(s => s.EquipmentTypeID == EquipmentTypeID);
            //}

            //if (EquipmentID != "")
            //{
            //    qry = qry.Where(s => s.EquipmentID == EquipmentID);
            //}
            //if (BaseStationID != null)
            //{
            //    qry = qry.Where(s => s.BaseStationID == BaseStationID);
            //}
            //if (Status != null)
            //{
            //    qry = qry.Where(s => s.Status == Status);
            //}
            //EquipmentMaster obj = GetSingle(qry.ToList());

            EquipmentMaster obj = (from c in db.EquipmentMasters
                                   orderby c.EquipmentMasterID
                                   where (EquipmentTypeID != null ? (c.EquipmentTypeID == EquipmentTypeID) : true) &&
                                         (EquipmentID != "" ? (c.EquipmentID == EquipmentID) : true) &&
                                         (BaseStationID != null ? (c.BaseStationID == BaseStationID) : true) &&
                                         (Status != null ? (c.Status == Status) : true)
                                   select c).FirstOrDefault();
            return obj;
        }
        #endregion
        #region EquipmentMaintenanceCostDetail

        /// <summary>
        /// Get Equipment Maintenance Cost Detail
        /// </summary>
        /// <param name="ProductItemiD"></param>
        /// <param name="Inventory"></param>
        IQueryable<EquipmentMaintenanceCostDetail> GetAllMaintenanceCostQuery()
        {
            IQueryable<EquipmentMaintenanceCostDetail> qry = from c in db.EquipmentMaintenanceCostDetails
                                                             orderby c.EquipmentMaintenanceCostID
                                                             select c;
            return qry;
        }

        /// <summary>
        /// Get Data from Equipment Maintenence Cost Detail Table using EquipmentMasterID
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        //public List<EquipmentMaintenanceCostDetail> GetEquipMaintenanceCostById(Int64 EquipmentMasterID)
        //{
        //    List<EquipmentMaintenanceCostDetail> qry = GetAllMaintenanceCostQuery().Where(s => s.EquipmentMasterID == EquipmentMasterID).ToList<EquipmentMaintenanceCostDetail>();
        //    var List = qry.ToList<EquipmentMaintenanceCostDetail>();
        //    return List;
        //}


        /// <summary>
        /// Get Data from Equipment Maintenence Cost Detail Table using EquipMaintenanceCostID
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public EquipmentMaintenanceCostDetail GetByEquipMaintenanceCostId(Int64 EquipMaintenanceCostID)
        {
            //IQueryable<EquipmentMaintenanceCostDetail> qry = GetAllMaintenanceCostQuery().Where(s => s.EquipmentMaintenanceCostID == EquipMaintenanceCostID);
            //EquipmentMaintenanceCostDetail obj = GetSingle(qry.ToList());

            EquipmentMaintenanceCostDetail obj = (from c in db.EquipmentMaintenanceCostDetails
                                                  orderby c.EquipmentMaintenanceCostID
                                                  where c.EquipmentMaintenanceCostID == EquipMaintenanceCostID
                                                  select c).FirstOrDefault();
            return obj;
        }

        /// <summary>
        /// Get DATA By EquipMaintenanceCostId to Show Vendor
        /// </summary>
        /// <param name="EquipMaintenanceCostID"></param>
        /// <returns></returns>
        public List<GetEquipmentMaintenanceDetailResult> GetEquipMaintenanceCostById(Int64 EquipmentMasterID)
        {

            var objlist = db.GetEquipmentMaintenanceDetail(EquipmentMasterID).ToList();

            return objlist.ToList<GetEquipmentMaintenanceDetailResult>();


            //IQueryable<EquipmentMaintenanceCostDetail> qry = GetAllMaintenanceCostQuery().Where(s => s.EquipmentMaintenanceCostID == EquipMaintenanceCostID);
            //EquipmentMaintenanceCostDetail obj = GetSingle(qry.ToList());
            //return obj;
        }

        public void DeleteMaintinanceCost(Int64 EquipmentMaintenanceCostID)
        {
            var matched = (from c in db.GetTable<EquipmentMaintenanceCostDetail>()
                           where c.EquipmentMaintenanceCostID == Convert.ToInt64(EquipmentMaintenanceCostID)
                           select c).SingleOrDefault();
            try
            {
                if (matched != null)
                {
                    db.EquipmentMaintenanceCostDetails.DeleteOnSubmit(matched);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Get DATA By CompanyID to Show Pending Invoice
        /// </summary>
        /// <param name="EquipMaintenanceCostID"></param>
        /// <returns></returns>
        public List<GetEquipPendingInvoiceResult> GetEquipPendingInvoiceByCmpID(Int64 CompanyID)
        {

            var objlist = db.GetEquipPendingInvoice(CompanyID).ToList();

            return objlist.ToList<GetEquipPendingInvoiceResult>();


            //IQueryable<EquipmentMaintenanceCostDetail> qry = GetAllMaintenanceCostQuery().Where(s => s.EquipmentMaintenanceCostID == EquipMaintenanceCostID);
            //EquipmentMaintenanceCostDetail obj = GetSingle(qry.ToList());
            //return obj;
        }

        #endregion
        #region EquipmentWeeklyMaintenance

        /// <summary>
        /// Get Weekly Equipment Maintenance Detail
        /// </summary>
        /// <param name="ProductItemiD"></param>
        /// <param name="Inventory"></param>
        IQueryable<EquipmentWeeklyMaintenance> GetAllWeeklyMaintenance()
        {
            IQueryable<EquipmentWeeklyMaintenance> qry = from c in db.EquipmentWeeklyMaintenances
                                                         orderby c.EquipmentWeeklyMaintinanceID
                                                         select c;
            return qry;
        }

        /// <summary>
        /// Get Data from Weekly Equipment Maintenence Table using EquipmentMasterID last inserted will come first
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public List<EquipmentWeeklyMaintenance> GetEquipWeeklyMaintenance(Int64 EquipmentMasterID)
        {
            //List<EquipmentWeeklyMaintenance> qry = GetAllWeeklyMaintenance().Where(s => s.EquipmentMasterID == EquipmentMasterID).ToList<EquipmentWeeklyMaintenance>();
            //var List = qry.OrderByDescending(o => o.EquipmentWeeklyMaintinanceID).ToList<EquipmentWeeklyMaintenance>();

            var List = (from c in db.EquipmentWeeklyMaintenances
                        orderby c.EquipmentWeeklyMaintinanceID
                        where c.EquipmentMasterID == EquipmentMasterID
                        select c).OrderByDescending(o => o.EquipmentWeeklyMaintinanceID).ToList();

            return List;
        }

        /// <summary>
        /// Get Data from Weekly Equipment Maintenence Table using EquipmentWeeklyMaintinanceID
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public EquipmentWeeklyMaintenance GetByEquipWeeklyMaintenanceId(Int64 EquipmentWeeklyMaintinanceID)
        {
            //IQueryable<EquipmentWeeklyMaintenance> qry = GetAllWeeklyMaintenance().Where(s => s.EquipmentWeeklyMaintinanceID == EquipmentWeeklyMaintinanceID);
            //EquipmentWeeklyMaintenance obj = GetSingle(qry.ToList());

            EquipmentWeeklyMaintenance obj = (from c in db.EquipmentWeeklyMaintenances
                                              orderby c.EquipmentWeeklyMaintinanceID
                                              where c.EquipmentWeeklyMaintinanceID == EquipmentWeeklyMaintinanceID
                                              select c).FirstOrDefault();
            return obj;
        }

        public void DeleteWeeklyMaintenance(Int64 EquipmentWeeklyMaintinanceID)
        {
            var matched = (from c in db.GetTable<EquipmentWeeklyMaintenance>()
                           where c.EquipmentWeeklyMaintinanceID == Convert.ToInt64(EquipmentWeeklyMaintinanceID)
                           select c).SingleOrDefault();
            try
            {
                if (matched != null)
                {
                    db.EquipmentWeeklyMaintenances.DeleteOnSubmit(matched);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region EquipmentImages

        /// <summary>
        /// Get All Equipment Images
        /// </summary>
        /// <param name="ProductItemiD"></param>
        /// <param name="Inventory"></param>
        IQueryable<EquipmentImage> GetAllEquipmentImages()
        {
            IQueryable<EquipmentImage> qry = from c in db.EquipmentImages
                                             orderby c.EquipmentImageID
                                             select c;
            return qry;
        }

        /// <summary>
        /// Get Equipment Images By Id
        /// </summary>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public List<EquipmentImage> GetEquipmentImagesById(Int64 EquipmentMasterID, bool IsAssetImage)
        {
            // return GetAllEquipmentImages().Where(s => s.EquipmentMasterID == EquipmentMasterID && s.IsAssetImage == IsAssetImage).ToList();

            return (from c in db.EquipmentImages
                    orderby c.EquipmentImageID
                    where c.EquipmentMasterID == EquipmentMasterID &&
                          c.IsAssetImage == IsAssetImage
                    select c).ToList();
        }


        public void DeleteEquipmentImage(Int64 EquipmentImageID)
        {
            var matched = (from c in db.GetTable<EquipmentImage>()
                           where c.EquipmentImageID == EquipmentImageID
                           select c).SingleOrDefault();
            try
            {
                if (matched != null)
                {
                    db.EquipmentImages.DeleteOnSubmit(matched);
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

            }

        }
        #endregion
        #region JobCode

        /// <summary>
        /// GetAllQuery()
        /// Return all the record from the JobCode table
        /// Nagmani 08/10/2010
        /// </summary>
        /// <returns></returns>
        IQueryable<EquipmentJobCodeLookup> GetAllJobCodeQuery()
        {
            IQueryable<EquipmentJobCodeLookup> qry = from C in db.EquipmentJobCodeLookups.Where(c => c.ParentJobCode == null)
                                                     select C;
            return qry;
        }

        /// <summary>
        /// GetAllJobCode()
        /// Return List of the record from the JobCode table
        /// Nagmani 08/10/2010
        /// </summary>
        /// <returns></returns>
        public List<EquipmentJobCodeLookup> GetAllJobCode()
        {
            return db.EquipmentJobCodeLookups.Where(c => c.ParentJobCode == null).ToList();
        }

        public EquipmentJobCodeLookup GetJobCodeByID(Int64 JobCodeID)
        {
            return db.EquipmentJobCodeLookups.SingleOrDefault(c => c.JobCodeID == JobCodeID);
        }

        public List<EquipmentJobCodeLookup> CheckDuplication(Int64 JobCodeID, String JobCodeName)
        {
            if (JobCodeID != 0)
                return db.EquipmentJobCodeLookups.Where(c => c.JobCodeID != JobCodeID && c.JobCodeName == JobCodeName).ToList();
            else
                return db.EquipmentJobCodeLookups.Where(c => c.JobCodeName == JobCodeName).ToList();
        }

        /// <summary>
        /// GetLookupJobCodeQuery()
        /// Return List of the SubJobCode record from the Inc_Lookup table
        /// Nagmani 08/10/2010
        /// </summary>
        /// <returns></returns>
        IQueryable<EquipmentJobCodeLookup> GetLookupJobCodeQuery()
        {
            IQueryable<EquipmentJobCodeLookup> qry = from C in db.EquipmentJobCodeLookups.Where(c => c.ParentJobCode != null)
                                                     select C;
            return qry;
        }

        /// <summary>
        /// GetAllSubJobCode
        /// Return List of the record from the JobCode table
        /// Nagmani 08/10/2010
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <returns></returns>
        /// <param name="JobCodeName"></param>
        public List<EquipmentJobCodeLookup> GetAllSubJobCode(int JobCodeID)
        {
            //IQueryable<EquipmentJobCodeLookup> qry = GetLookupJobCodeQuery();
            //List<EquipmentJobCodeLookup> objList = qry.ToList();
            //var subJobCode = (from c in objList where c.JobCodeID == JobCodeID select c);

            var subJobCode = (from C in db.EquipmentJobCodeLookups
                              where C.ParentJobCode != null &&
                                    C.JobCodeID == JobCodeID
                              select C).ToList();


            return subJobCode.ToList();
        }
        #endregion
        #region JobSubCode
        IQueryable<EquipmentJobCodeLookup> GetAllSubJobCodeQuery()
        {
            IQueryable<EquipmentJobCodeLookup> qry = from C in db.EquipmentJobCodeLookups.Where(c => c.ParentJobCode != null)
                                                     select C;
            return qry;
        }


        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="iJobCodeid"></param>
        /// <returns></returns>
        public List<EquipmentJobCodeLookup> GetAllSubJobCodeDetail(Int64 iJobCodeid)
        {
            //IQueryable<EquipmentJobCodeLookup> qry = GetAllSubJobCodeQuery();
            //List<EquipmentJobCodeLookup> objList = qry.ToList();
            //var subJobCode = (from c in objList where c.ParentJobCode == iJobCodeid select c);

            return (from C in db.EquipmentJobCodeLookups where C.ParentJobCode == iJobCodeid select C).OrderBy(c => c.JobCodeName).ToList();
        }

        public List<EquipmentJobCodeLookup> GetAllSubJobCodeByJobCodeName(String JobCodeName)
        {
            return (from sc in db.EquipmentJobCodeLookups
                    join c in db.EquipmentJobCodeLookups on sc.JobCodeID equals c.JobCodeID
                    where c.JobCodeName == JobCodeName
                    select sc).ToList();
        }

        /// <summary>
        /// Update By : Gaurang Pathak
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public EquipmentJobCodeLookup GetByName(string Name)
        {
            //IQueryable<EquipmentJobCodeLookup> qry = GetAllSubJobCodeQuery().Where(s => s.JobCodeName.Equals(Name));
            //EquipmentJobCodeLookup objSubJobCode = qry.SingleOrDefault();

            EquipmentJobCodeLookup objSubJobCode = (from C in db.EquipmentJobCodeLookups
                                                    where C.ParentJobCode != null &&
                                                     (C.JobCodeName.Equals(Name))
                                                    select C).FirstOrDefault();
            return objSubJobCode;
        }

        public EquipmentJobCodeLookup GetSubJobCodeByID(Int64 SubJobCodeID)
        {
            return db.EquipmentJobCodeLookups.SingleOrDefault(c => c.JobCodeID == SubJobCodeID);
        }

        public List<EquipmentJobCodeLookup> CheckDuplication(Int64 JobCodeID, Int64 SubJobCodeID, String SubJobCodeName)
        {
            if (JobCodeID != 0 && SubJobCodeID != 0)
                return db.EquipmentJobCodeLookups.Where(c => c.ParentJobCode == JobCodeID && c.JobCodeID != SubJobCodeID && c.JobCodeName == SubJobCodeName).ToList();
            else
                return db.EquipmentJobCodeLookups.Where(c => c.ParentJobCode == JobCodeID && c.JobCodeName == SubJobCodeName).ToList();
        }
        #endregion
        #region PartsHistory
        public List<EquipmentRepairOrder> GetRepairOrderByID(Int64 EquipmentMasterID)
        {
            var ReparOrder = (from c in db.GetTable<EquipmentRepairOrder>()
                              where c.EquipmentMasterID == EquipmentMasterID
                              select c);
            return ReparOrder.ToList<EquipmentRepairOrder>();
        }
        #endregion
        #region ReplyEmailNotes
        //Get GSE Asset Mgt Email List
        public List<GetGSEUsersResult> GetGSEUsers()
        {
            return db.GetGSEUsers().ToList();
            //var objlist = db.GetGSEUsers().ToList();           
            //return objlist.ToList<GetGSEUsersResult>();
        }
        /// <summary>
        /// Insert Reply Emai lNotes
        /// </summary>
        /// <param name="EquipmentID"></param>
        /// <param name="UserInfoID"></param>
        /// <param name="Contents"></param>
        /// <param name="NoteFor"></param>
        /// <param name="SpecificNoteFor"></param>
        public void InsertReplyEmailNotes(Int64 EquipmentMasterID, Int64? UserInfoID, String Contents, String NoteFor, String SpecificNoteFor)
        {
            try
            {
                //string strNoteHistory = "";
                string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.AssetManagement);

                NoteDetail objComNot = new NoteDetail();
                NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();


                objComNot.Notecontents = Contents;
                objComNot.NoteFor = NoteFor;
                objComNot.SpecificNoteFor = SpecificNoteFor;
                objComNot.ForeignKey = EquipmentMasterID;
                objComNot.CreateDate = System.DateTime.Now;
                objComNot.CreatedBy = UserInfoID;
                objComNot.UpdateDate = System.DateTime.Now;
                objComNot.UpdatedBy = UserInfoID;
                objCompNoteHistRepos.Insert(objComNot);
                objCompNoteHistRepos.SubmitChanges();
            }
            catch (Exception)
            {


            }
        }
        #endregion
        #region TextMessage

        public void TextMsg(Int64 userInfoID, String txtBody)
        {
            try
            {
                WebClient client = new WebClient();

                string MobileNo = db.UserInformations.FirstOrDefault(r => r.UserInfoID == userInfoID && r.IsDeleted == false).Mobile;
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                // Local Settings
                //client.QueryString.Add("user", "saurabh");
                //client.QueryString.Add("password", "ARKGMRSPDZQTQD");
                //client.QueryString.Add("api_id", "3394406");
                // Live Settings
                string user = ConfigurationSettings.AppSettings["ClickatelUser"];
                string password = ConfigurationSettings.AppSettings["ClickatelPassword"];
                string api_id = ConfigurationSettings.AppSettings["ClickatelApi_id"];
                string MO = ConfigurationSettings.AppSettings["ClickatelMO"];
                string from = ConfigurationSettings.AppSettings["ClickatelFrom"];

                client.QueryString.Add("user", user);
                client.QueryString.Add("password", password);
                client.QueryString.Add("api_id", api_id);
                client.QueryString.Add("MO", MO);
                client.QueryString.Add("from", from);


                client.QueryString.Add("to", MobileNo);
                client.QueryString.Add("text", txtBody);
                string baseurl = "http://api.clickatell.com/http/sendmsg";
                Stream data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
            }
            catch (Exception)
            { }
        }
        #endregion

        #region DropDownMgt
        public List<EquipmentLookup> GetItemFrmEquipmentLookup(String ILookupCode)
        {
            List<EquipmentLookup> qry = (from C in db.EquipmentLookups.Where(c => c.ParentiLookupID == null && c.iLookupCode == ILookupCode)
                                         select C).OrderBy(p => p.sLookupName).ToList();
            return qry;
        }
        public List<EquipmentLookup> GetSubItemFrmEquipmentLookup(String ILookupCode, Int64 ParentiLookupID)
        {
            List<EquipmentLookup> qry = (from C in db.EquipmentLookups.Where(c => c.ParentiLookupID == ParentiLookupID && c.iLookupCode == ILookupCode)
                                         select C).ToList();
            return qry;
        }

        public List<EquipmentLookup> CheckDuplicatLookup(String SLookupName, String ILookupCode)
        {
            return db.EquipmentLookups.Where(c => c.iLookupCode != ILookupCode && c.sLookupName == SLookupName).ToList();
        }
        public EquipmentLookup GetLookupByID(Int64 IlookupID)
        {
            return db.EquipmentLookups.Where(c => c.iLookupID == IlookupID).FirstOrDefault();
        }


        #endregion


        /// <summary>
        /// Added By : Prshant Kanakhara 4th April 2013
        /// </summary>
        /// <returns></returns>
        public List<UserType> GetGSEAssetManagemenUserType()
        {
            List<UserType> lstUserTypes = db.UserTypes.Where(x => x.UserTypeFor == "GSEAssetManagement").ToList();
            return lstUserTypes;
        }

        /// <summary>
        /// Check whether Super Admin has been assigned to the company or not?
        /// </summary>
        /// <param name="EquipmentVendorID"></param>
        /// <param name="IsCustomer"></param>
        /// <returns></returns>
        public bool IsSuperAdminDefined(Int64? EquipmentVendorID, bool IsCustomer)
        {
            bool flag = false;
            if (IsCustomer)
            {
                var obj = (from e in db.EquipmentVendorEmployees
                           join u in db.UserInformations on e.UserInfoID equals u.UserInfoID
                           where e.VendorID == @EquipmentVendorID && u.Usertype == (long)Incentex.DAL.Common.DAEnums.UserTypes.CustomerSuperAdmin
                           select e).FirstOrDefault();
                if (obj != null)
                    flag = true;
            }
            else
            {
                var obj = (from e in db.EquipmentVendorEmployees
                           join u in db.UserInformations on e.UserInfoID equals u.UserInfoID
                           where e.VendorID == @EquipmentVendorID && u.Usertype == (long)Incentex.DAL.Common.DAEnums.UserTypes.VendorSuperAdmin
                           select e).FirstOrDefault();
                if (obj != null)
                    flag = true;
            }
            return flag;
        }


        /// <summary>
        /// Get all assets for GSE Asset Management module
        /// Created for the New Integration of Design on 14th June 2013
        /// </summary>
        /// <param name="EquipmentTypeID"></param>
        /// <param name="EquipmentID"></param>
        /// <param name="BaseStationID"></param>
        /// <param name="Status"></param>
        /// <param name="EquipmentMasterID"></param>
        /// <param name="Flagged"></param>
        /// <param name="GSEDepartmentID"></param>
        /// <param name="CompanyID"></param>
        /// <param name="VEUserInfoID"></param>
        /// <returns></returns>
        public List<GetAssetsResult> GetAssetsList(Int64? EquipmentTypeID, string EquipmentID, Int64? BaseStationID, Int64? Status, Int64? GSEDepartmentID, Int64 CompanyID, string Keyword)
        {
            return db.GetAssets(EquipmentTypeID, EquipmentID, BaseStationID, Status, GSEDepartmentID, CompanyID, Keyword).ToList();
        }


        public List<GetAssetWarrantyListOrByIDResult> GetAssetWarranty(Int64 AssetID, Int64? WarrantyID)
        {
            return db.GetAssetWarrantyListOrByID(AssetID, WarrantyID).ToList();
        }


        public List<GetAssetWarrantyClaimResult> GetAssetWarrantyClaimByID(Int64? WarrantyID, Int64? ClaimID)
        {
            return db.GetAssetWarrantyClaim(WarrantyID, ClaimID).ToList();
        }


        public List<InsertAndGetAllDropDownOptionsResult> InsertAndGetAllDropDownOption(string LookUpCode, string LookUpName, string Flag)
        {
            return db.InsertAndGetAllDropDownOptions(LookUpCode, LookUpName, Flag).ToList();
        }

        public string InsertIntoEquipmentFieldMaster(Int64 EquipmentMasterID, string FieldMasterName, Int64 EquipmentTypeID, Int64? CompanyID, string ControlType, bool ApplyToAll, string SectionName)
        {
            return db.InsertIntoFieldMaster(EquipmentMasterID, FieldMasterName, EquipmentTypeID, CompanyID, ControlType, ApplyToAll, SectionName).FirstOrDefault().InsertTransactionStatus;
        }

        public List<GetAssetSpecificationsByAssetMasterIDCompanyIDResult> GetAssetSpecificationsByAssetMasterIDCompanyID(Int64 EquipmentMasterID, Int64? CompanyID, string SectionName)
        {
            return db.GetAssetSpecificationsByAssetMasterIDCompanyID(EquipmentMasterID, CompanyID, SectionName).ToList();
        }

        /// <summary>
        /// Created By : Prashant
        /// </summary>
        /// <param name="FieldMasterID"></param>
        /// <param name="EquipmentMasterID"></param>
        /// <returns></returns>
        public EquipmentFieldDetail GetFieldDetailById(Int64 FieldDetailID)
        {
            EquipmentFieldDetail obj = (from c in db.EquipmentFieldDetails
                                        where c.FieldDetailID == FieldDetailID
                                        select c).FirstOrDefault();
            return obj;
        }

        public int DeleteFieldDetailFieldMasterByID(String FieldDetailIdMasterIDLookUpCode)
        {
            return db.DeleteFromFieldMasterFieldDetailByID(FieldDetailIdMasterIDLookUpCode);
        }

        public Int64? GetIdByLookupNameNLookUpCode(String LookupName, String LookupCode)
        {
            Int64? LookupId = (from q in db.EquipmentLookups
                               where q.sLookupName == LookupName && q.iLookupCode == LookupCode
                               select q.iLookupID).FirstOrDefault();

            return LookupId;
        }

        public Int64? GetBaseStationIDByName(String BaseStationName)
        {
            Int64? LookupId = (from q in db.INC_BasedStations
                               where q.sBaseStation == BaseStationName
                               select q.iBaseStationId).FirstOrDefault();

            return LookupId;
        }

        public List<EquipmentFile> GetEquipmentFilesByEquipmentMasterID(Int64 EquipmentMasterID, string strFileFor)
        {
            return (from q in db.EquipmentFiles
                    where q.EquipmentMasterID == EquipmentMasterID && q.FileFor == strFileFor
                    select q).ToList();
        }

        public void DeleteEquipmentFile(Int64 EquipmentFileID)
        {
            var matched = (from e in db.EquipmentFiles
                           where e.EquipmentFileID == EquipmentFileID
                           select e).FirstOrDefault();
            if (matched != null)
            {
                db.EquipmentFiles.DeleteOnSubmit(matched);
            }
            db.SubmitChanges();
        }

        public List<GetAssetIDByAssetTypesResult> GetAssetIDByAssetType(Int64? AssetType, Int64 CompanyID, Int64 BaseStationID)
        {
            return db.GetAssetIDByAssetTypes(AssetType, CompanyID, BaseStationID).ToList();
        }

        public void DeleteWarrantyClaimByID(Int64 WarrantyID)
        {
            db.DeleteWarrantyAndClaimByID(WarrantyID);
        }

        public void UnflaggedAssets(Int64 EquipmentMasterID, Int64 UserInfoID, Incentex.DAL.Common.DAEnums.NoteForType strNoteFor)
        {
            string strNoteForName = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(strNoteFor);
            db.UnflaggedAssetsAndClearNotes(EquipmentMasterID, UserInfoID, strNoteForName);
        }
        public void FlagAssetsAndAddNote(Int64 EquipmentMasterID, Int64 UserInfoID, Incentex.DAL.Common.DAEnums.NoteForType strNoteFor, string strNoteContents)
        {
            string strNoteForName = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(strNoteFor);
            db.FlaggedAssetsAndAddNotes(EquipmentMasterID, UserInfoID, strNoteForName, strNoteContents);
        }
        public List<INC_Lookup> GetEquipmentTypesByBaseStation(Int64? BaseStationID)
        {
            if (BaseStationID > 0)
                return (from r in db.EquipmentMasters
                        join lt in db.INC_Lookups on r.EquipmentTypeID equals lt.iLookupID
                        where r.BaseStationID == BaseStationID
                        select lt).Distinct().OrderBy(lt => lt.sLookupName).ToList();
            else
                return (from r in db.EquipmentMasters
                        join lt in db.INC_Lookups on r.EquipmentTypeID equals lt.iLookupID
                        select lt).Distinct().OrderBy(lt => lt.sLookupName).ToList();

        }

        public List<GetAssetListForSearchDropDownsResult> GetSearchDropDownsLists(Int64 CompanyID, Int64? AssetType, Int64? BaseStationID, String AssetID, Int64? StatusID)
        {
            return db.GetAssetListForSearchDropDowns(CompanyID, AssetType, BaseStationID, AssetID, StatusID).ToList();
        }

        public String TrailingNotes(String ReceivedBy, String NoteFor, String SpecificNoteFor, String NewLineChar)
        {
            String TrailingNotes = String.Empty;

            List<NoteDetail> objList = new List<NoteDetail>();
            objList = db.NoteDetails.Where(n => n.ReceivedBy == ReceivedBy && n.NoteFor == NoteFor && n.SpecificNoteFor == SpecificNoteFor).OrderByDescending(n => n.NoteID).ToList();

            if (objList.Count > 0)
            {
                StringBuilder sbTrailingNotes = new StringBuilder();
                foreach (NoteDetail obj in objList)
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

        public List<SelectRecipentsForReplyTo> GetRecipentsFromAssetID(Int64 CreatedUserID)
        {
            List<SelectRecipentsForReplyTo> recipentslist = new List<SelectRecipentsForReplyTo>();
            recipentslist = (from u in db.UserInformations
                             where u.UserInfoID == CreatedUserID
                             select new SelectRecipentsForReplyTo
                             {
                                 FirstName = u.FirstName,
                                 LastName = u.LastName,
                                 Email = u.LoginEmail,
                                 UserInfoID = u.UserInfoID,
                                 Usertype = u.Usertype
                             }).ToList();
            return recipentslist;
        }

        public InsertNotesForSendMailResult InsertIntoNotesForSendEmail(String NoteFor, String SpecificNoteFor, Int64 EquipmentMasterID, String ForeignKey, String NoteContents, String UserEmail, String ModuleName, Int64 CreatedBy)
        {
            return db.InsertNotesForSendMail(NoteFor, SpecificNoteFor, EquipmentMasterID, ForeignKey, NoteContents, UserEmail, ModuleName, CreatedBy).ToList().FirstOrDefault();
        }

        public List<NoteResult> GetNotesForHistoryTab()
        {
            //GetNotesForAssetManagement
            var lst = (from n in db.NoteDetails
                       join u in db.UserInformations on n.CreatedBy equals u.UserInfoID
                       select new NoteResult
                       {
                           Name = u.FirstName + ' ' + u.LastName,
                           CreatedDate = n.CreateDate,
                           Description = n.Notecontents
                       }).ToList();
            return lst;
        }
        
    }
    public class NoteResult
    {
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Description { get; set; }
    }
   
}

