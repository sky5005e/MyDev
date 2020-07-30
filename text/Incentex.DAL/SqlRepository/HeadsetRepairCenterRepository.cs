using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incentex.DAL.Common;

namespace Incentex.DAL.SqlRepository
{
    public class HeadsetRepairCenterRepository : RepositoryBase
    {
        #region Common Functions

        public List<HeadsetRepairCenterCustom> GetHeadsetRepairCenterBysearch(long? Company, long? Contact, string RepairNumber, long? Status)
        {
            var result = (from e in db.HeadsetRepairCenterMasters
                          join c in db.Companies on e.CompanyID equals c.CompanyID
                          join con in db.UserInformations on e.ContactID equals con.UserInfoID
                          where (((Company != null && Company != 0) ? e.CompanyID == Company : true) &&
                                ((Contact != null && Contact != 0) ? e.ContactID == Contact : true) &&
                                ((Status != null && Status != 0) ? e.Status == Status : true) &&
                                (!string.IsNullOrEmpty(RepairNumber) ? e.RepairNumber.Value.ToString().ToLower() == RepairNumber.ToLower() : true)) &&
                                e.IsDeleted == false 
                          select new HeadsetRepairCenterCustom
                          {
                              HeadsetRepairID = e.HeadsetRepairID,
                              RepairNumber = e.RepairNumber.Value,
                              CompanyName = c.CompanyName,
                              ContactName = con.FirstName + " " + con.LastName,
                              Date = e.Date.Value,
                              OrderTrackingNumber = e.OrderTrackingNumber,
                              IsCustomerApprovedQuote = e.IsCustomerApprovedQuote.HasValue ? e.IsCustomerApprovedQuote.Value.ToString() : string.Empty,
                              Requestquotebeforerepair = e.RequestQuoteBeforeRepair.Value  
                          }).ToList();
            return result;

        }

        public HeadsetRepairCenterMaster GetHeadsetRepairCenterById(long HeadsetRepairID)
        {
            return (from e in db.HeadsetRepairCenterMasters
                    where e.HeadsetRepairID == HeadsetRepairID
                    select e).SingleOrDefault();
        }

        public HeadsetRepairCenterCustom GetHeadsetRepaircustomByRepairNumber(long RepairNumber)
        {
            return (from e in db.HeadsetRepairCenterMasters
                    join b in db.INC_Lookups on e.HeadsetBrandID equals b.iLookupID
                    join c in db.Companies on e.CompanyID equals c.CompanyID 
                    from il in db.INC_Lookups.Where(t => t.iLookupID == e.Status.Value).DefaultIfEmpty()
                    from v in db.HeadsetRepairVendors.Where(t=>t.VendorID == e.VendorID).DefaultIfEmpty()     
                    join user in db.UserInformations on e.ContactID equals user.UserInfoID
                    from con in db.CompanyEmployeeContactInfos.Where(t=>t.CompanyContactInfoID == e.ReturnHeadsetToID).DefaultIfEmpty()  
                    from city in db.INC_Cities.Where(t=>t.iCityID == con.CityID).DefaultIfEmpty()
                    from state in db.INC_States.Where(t=>t.iStateID == con.StateID).DefaultIfEmpty()    
                    where e.RepairNumber == RepairNumber
                    select new HeadsetRepairCenterCustom
                    {
                        HeadsetRepairID = e.HeadsetRepairID,
                        ContactID = e.ContactID.Value,
                        ContactName = user.FirstName + " " + user.LastName,
                        CompanyName = c.CompanyName, 
                        RepairNumber = e.RepairNumber.Value,
                        Date = e.Date.Value,
                        TotalHeadset = e.TotalHeadset.Value,
                        HeadsetBrandName = b.sLookupName,
                        Status = il != null ? il.iLookupID.ToString() : string.Empty,
                        OrderTrackingNumber = e.OrderTrackingNumber,
                        IsCustomerApprovedQuote = e.IsCustomerApprovedQuote.HasValue ? e.IsCustomerApprovedQuote.Value.ToString() : string.Empty,
                        Requestquotebeforerepair = e.RequestQuoteBeforeRepair.Value,
                        RepairQuoteAmount = e.RepairQuoteAmount.HasValue ? e.RepairQuoteAmount.Value : 0,
                        EstimatedLeadTime = e.EstimatedLeadTime,
                        VendorID = e.VendorID.HasValue ? e.VendorID.Value : 0,
                        Vendorcompany = v !=null ? v.VendorName : string.Empty,
                        retCompany = con != null ? con.CompanyName : string.Empty,
                        retContactName = con != null ? con.Name : string.Empty,
                        retAddress1 = con != null ? con.Address : string.Empty,
                        retAddress2 = con != null ? con.Address2 : string.Empty,
                        retCity = con != null ? city.sCityName  : string.Empty,
                        retState = con != null ? state.sStatename : string.Empty,
                        retZip = con != null ? con.ZipCode : string.Empty,
                        retTelephone = con != null ? con.Telephone : string.Empty,   
                        trackingtosupplier = e.TrackingtoSupplierNumber,
                        trackingfromsupplier = e.TrackingfromSupplierNumber,
                        trackingtoclient = e.TrackingtoClientNumber,
                        IsUPSShippertoSupplier = e.IsUPSShippertoSupplier.HasValue ? e.IsUPSShippertoSupplier.Value.ToString() : string.Empty,     
                        IsUPSShipperfromSupplier = e.IsUPSShipperfromSupplier.HasValue ? e.IsUPSShipperfromSupplier.Value.ToString() : string.Empty,
                        IsUPSShippertoClient  = e.IsUPSShippertoClient.HasValue ? e.IsUPSShippertoClient.Value.ToString() : string.Empty      
                    }).SingleOrDefault();
        }

        public String TrailingNotes(Int64 ForeinKey, string NoteFor, string SpecificNoteFor, String NewLineChar)
        {
            String TrailingNotes = String.Empty;

            List<NoteDetail> objList = new List<NoteDetail>();
            objList = db.NoteDetails.Where(n => n.NoteFor == NoteFor && n.ForeignKey == ForeinKey).OrderByDescending(n => n.NoteID).ToList();

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

        public void DeleteHeadsetRepairCenter(long HeadsetRepairCenterID , long UserID)
        {
            HeadsetRepairCenterMaster obj = GetHeadsetRepairCenterById(HeadsetRepairCenterID);
            if (obj != null)
            {
                obj.IsDeleted = true;
                obj.DeletedBy = UserID;
                obj.DeletedDate = System.DateTime.Now;   
                base.SubmitChanges();
            }
        }

        /// <summary>
        ///  Manage HeadsetRepair Email
        /// </summary>
        /// <param name="lstHeadsetRepairEmailID"></param>
        /// <param name="noteID"></param>
        public void InsertHeadsetRepairCenterManageEmail(List<long> lstHeadsetRepairEmailID, long noteID, long hdnHeadsetRepairID)
        {
            //Delete HeadsetRepair Email
            List<HeadsetRepairCenterManageEmail> lstmail = (from m in db.HeadsetRepairCenterManageEmails
                                                            where m.HeadsetRepairID == hdnHeadsetRepairID
                                                            select m).ToList();

            db.HeadsetRepairCenterManageEmails.DeleteAllOnSubmit(lstmail);
            base.SubmitChanges();

            if (lstHeadsetRepairEmailID != null && lstHeadsetRepairEmailID.Count > 0)
            {
                foreach (long item in lstHeadsetRepairEmailID)
                {
                    HeadsetRepairCenterManageEmail objEmail = new HeadsetRepairCenterManageEmail();
                    objEmail.NoteID = noteID;
                    objEmail.UserInfoID = item;
                    objEmail.HeadsetRepairID = hdnHeadsetRepairID;

                    db.HeadsetRepairCenterManageEmails.InsertOnSubmit(objEmail);
                    base.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// Get All Incentex Employee excluding Super Admin
        /// </summary>
        /// <returns></returns>
        public List<IEListResultsCustom> GetAllEmployeeCustome(long HeadsetRepairID)
        {
            return
            (from ie in db.IncentexEmployees
             join u in db.UserInformations on ie.UserInfoID equals u.UserInfoID
             from hEmail in db.HeadsetRepairCenterManageEmails.Where(t => t.UserInfoID == u.UserInfoID && t.HeadsetRepairID == HeadsetRepairID).DefaultIfEmpty()
             where ie.MemberRole != "Super Admin" && u.IsDeleted == false
             select new IEListResultsCustom
             {
                 EmployeeName = (u.FirstName + " " + u.LastName),
                 Email = u.Email,
                 UserInfoID = u.UserInfoID,
                 IsChecked = hEmail != null ? true : false
             }).ToList<IEListResultsCustom>();
        }

        public List<IEListResultsCustom> GetHeadsetRepairCenterManageEmail(long HeadsetRepairID)
        {
            return (from e in db.HeadsetRepairCenterManageEmails
                    join u in db.UserInformations on e.UserInfoID equals u.UserInfoID
                    where e.HeadsetRepairID == HeadsetRepairID
                    select new IEListResultsCustom
                    {
                        EmployeeName = (u.FirstName + " " + u.LastName),
                        Email = u.Email,
                        UserInfoID = u.UserInfoID
                    }).ToList();
        }

        /// <summary>
        /// Get Max Repair No
        /// </summary>
        /// <returns></returns>
        public Int64? GetMaxRepairNo()
        {
            return db.HeadsetRepairCenterMasters.Max(x => (Int64?)x.RepairNumber);
        }

        public void UpdateOrderTrackingNumber(long HeadsetRepairID, string OrderTrackingNumber)
        {
            HeadsetRepairCenterMaster obj = GetHeadsetRepairCenterById(HeadsetRepairID);

            if (obj != null)
            {
                obj.OrderTrackingNumber = OrderTrackingNumber;
                base.SubmitChanges();
            }
        }

        public void UpdateHeaderRepairCenter(HeadsetRepairCenterMaster objHeadsetRepair)
        {
            HeadsetRepairCenterMaster obj = GetHeadsetRepairCenterById(objHeadsetRepair.HeadsetRepairID);

            if (obj != null)
            {
                obj.CompanyID = objHeadsetRepair.CompanyID;
                obj.ContactID = objHeadsetRepair.ContactID;
                obj.HeadsetBrandID = objHeadsetRepair.HeadsetBrandID;
                obj.TotalHeadset = objHeadsetRepair.TotalHeadset;
                obj.RequestQuoteBeforeRepair = objHeadsetRepair.RequestQuoteBeforeRepair;

                obj.UpdatedBy = objHeadsetRepair.UpdatedBy;
                obj.UpdatedDate = System.DateTime.Now;

                if (objHeadsetRepair.ReturnHeadsetToID > 0)
                    obj.ReturnHeadsetToID = objHeadsetRepair.ReturnHeadsetToID.Value;
                else
                    obj.ReturnHeadsetToID = null;

                if (objHeadsetRepair.Status > 0)
                    obj.Status = objHeadsetRepair.Status;
                else
                    obj.Status = null;

                base.SubmitChanges();
            }

        }

        public void InsertHeadsetQuote(long HeadsetRepairID, long RepairQuoteAmount, string EstimatedLeadTime)
        {
            HeadsetRepairCenterMaster obj = GetHeadsetRepairCenterById(HeadsetRepairID);
            if (obj != null)
            {
                obj.RepairQuoteAmount = RepairQuoteAmount;
                obj.EstimatedLeadTime = EstimatedLeadTime;

                base.SubmitChanges();
            }
        }

        /// <summary>
        /// Get All HeadsetRepair Vendor
        /// </summary>
        /// <returns></returns>
        public List<HeadsetRepairVendor> GetAllHeadsetRepairVendor()
        {
            return (from v in db.HeadsetRepairVendors select v).ToList();
        }

        /// <summary>
        /// Get HeadsetRepair Vendor By VendorID  
        /// </summary>
        /// <returns></returns>
        public HeadsetRepairVendor GetHeadsetRepairVendorByID(long vendorID)
        {
            return (from v in db.HeadsetRepairVendors
                    where v.VendorID == vendorID && v.IsDeleted == false
                    select v).SingleOrDefault();
        }

        public void UpdateHeadsetRepairVenoderdetails(long HeadsetRepairID, long VendorID, long UserID)
        {
            HeadsetRepairCenterMaster obj = GetHeadsetRepairCenterById(HeadsetRepairID);
            if (obj != null)
            {
                if (VendorID > 0)
                    obj.VendorID = VendorID;
                else
                    obj.VendorID = null;

                obj.UpdatedBy = UserID;
                obj.UpdatedDate = System.DateTime.Now;
                base.SubmitChanges();
            }
        }

        public void InsertHeadsetTrackingInformation(long HeadsetrepairID , HeadsetRepairCenterMaster objheadset)
        {
            HeadsetRepairCenterMaster obj = GetHeadsetRepairCenterById(HeadsetrepairID);
            
            if (obj != null)
            {
                obj.TrackingtoSupplierNumber = objheadset.TrackingtoSupplierNumber;
                obj.TrackingfromSupplierNumber = objheadset.TrackingfromSupplierNumber;
                obj.TrackingtoClientNumber = objheadset.TrackingtoClientNumber;

                obj.IsUPSShippertoSupplier = objheadset.IsUPSShippertoSupplier;
                obj.IsUPSShipperfromSupplier = objheadset.IsUPSShipperfromSupplier;
                obj.IsUPSShippertoClient = objheadset.IsUPSShippertoClient;

                base.SubmitChanges();  
            }
        }

        #endregion
    }

    #region Extension Class
    public class HeadsetRepairCenterCustom
    {
        public long HeadsetRepairID { get; set; }

        public Int64 RepairNumber { get; set; }

        public string OrderTrackingNumber { get; set; }

        public Int64 CompanyID { get; set; }

        public string CompanyName { get; set; }

        public Int64 ContactID { get; set; }

        public string ContactName { get; set; }

        public Int64 HeadsetBrandID { get; set; }

        public string HeadsetBrandName { get; set; }

        public int TotalHeadset { get; set; }

        public DateTime Date { get; set; }

        public Int64 CreateBy { get; set; }

        public string Status { get; set; }

        public string IsCustomerApprovedQuote { get; set; }

        public long RepairQuoteAmount { get; set; }

        public string EstimatedLeadTime { get; set; }

        public bool Requestquotebeforerepair { get; set; }

        public long VendorID { get; set; }

        public string Vendorcompany { get; set; }

        public string retCompany { get; set; }

        public string retContactName {get;set;}

        public string retAddress1 { get; set; }

        public string retAddress2 { get; set; }

        public string retCity { get; set; }

        public string retState { get; set; }

        public string retZip { get; set; }

        public string retTelephone { get; set; }

        public string trackingtosupplier { get; set; }

        public string trackingfromsupplier { get; set; }

        public string trackingtoclient { get; set; }

        public string IsUPSShippertoSupplier { get; set; }

        public string IsUPSShipperfromSupplier { get; set; }

        public string IsUPSShippertoClient { get; set; }
    }

    public class IEListResultsCustom
    {
        public string EmployeeName { get; set; }

        public string Email { get; set; }

        public long UserInfoID { get; set; }

        public bool IsChecked { get; set; }
    }
    #endregion
}
