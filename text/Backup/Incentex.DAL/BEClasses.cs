using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Configuration;
using System.Reflection;

namespace Incentex.DAL
{
    public partial class CompanyStation
    {
        /*
        public string sCountryName { get; set; }
        public string sStatename { get; set; }
        public string sCityName { get; set; }
         */
    }

    public partial class StoreDocument
    {
        public string Workgroup { get; set; }
        public string Department { get; set; }
    }

    //public partial class SelectOrderDetailsResult
    //{
    //    private int _SupplierId;
    //    public int SupplierId
    //    {
    //        get
    //        {
    //            return this._SupplierId;
    //        }
    //        set
    //        {
    //            if ((this._SupplierId != value))
    //            {
    //                this._SupplierId = value;
    //            }
    //        }
    //    }
    //}

    public class PaymentInfo
    {
        public string OrderNumber { get; set; }
        public decimal OrderAmountToPay { get; set; }
        //public decimal TaxAmount { get; set; }
        //public decimal SalesTax { get; set; }
        //public decimal ShippingAmount { get; set; }
        //Billing Details
        public string B_FirstName { get; set; }
        public string B_LastName { get; set; }
        public string B_StreetAddress1 { get; set; }
        public string B_StreetAddress2 { get; set; }
        public string B_State { get; set; }
        public string B_City { get; set; }
        public string B_Zipcode { get; set; }
        public string B_Email { get; set; }
        public string B_CompanyName { get; set; }
        public string B_PhoneNumber { get; set; }
        //Shipping Details
        public string S_FirstName { get; set; }
        public string S_LastName { get; set; }
        public string S_StreetAddress1 { get; set; }
        public string S_StreetAddress2 { get; set; }
        public string S_State { get; set; }
        public string S_City { get; set; }
        public string S_Zipcode { get; set; }
        public string S_Email { get; set; }
        public string S_CompanyName { get; set; }
        public string S_PhoneNumber { get; set; }


        //Card Detail
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string CardVerification { get; set; }
        public string ExpiresOnMonth { get; set; }
        public string ExpiresOnYear { get; set; }





        public string Country { get; set; }
    }

    public class selectmenuaccess
    {
        public string mainmenusetting { get; set; }
        public string employeeuniform { get; set; }
        public string additonalinfo { get; set; }
        public string companystore { get; set; }
        public string uniform { get; set; }
        public string supplies { get; set; }

    }

    public class selectstoresetting
    {
        public string userstoreoption { get; set; }
        public string paymentoption { get; set; }
        public string checkoutinformation { get; set; }
    }

    public partial class IncentexBEDataContext
    {
        public IncentexBEDataContext()
            : base(ConfigurationManager.ConnectionStrings["conn"].ConnectionString)
        {
            OnCreated();
        }

        //[Function(Name = "dbo.GetServiceTicketActivities")]
        [Function(Name = "dbo.GetServiceTicketActivitiesOnly")]
        [ResultType(typeof(GetServiceTicketActivitiesResult))]
        [ResultType(typeof(FUN_GetServiceTicketNoteResult))]
        public IMultipleResults GetServiceTicketActivities([Parameter(Name = "CompanyID", DbType = "BigInt")] System.Nullable<long> companyID, [Parameter(Name = "ContactID", DbType = "BigInt")] System.Nullable<long> contactID, [Parameter(Name = "OpenedByID", DbType = "BigInt")] System.Nullable<long> openedByID, [Parameter(Name = "StatusID", DbType = "BigInt")] System.Nullable<long> statusID, [Parameter(Name = "OwnerID", DbType = "BigInt")] System.Nullable<long> ownerID, [Parameter(Name = "TicketName", DbType = "NVarChar(150)")] string ticketName, [Parameter(Name = "TicketNumber", DbType = "NVarChar(10)")] string ticketNumber, [Parameter(Name = "DateNeeded", DbType = "NVarChar(10)")] string dateNeeded, [Parameter(Name = "SupplierID", DbType = "BigInt")] System.Nullable<long> supplierID, [Parameter(Name = "TypeOfRequestID", DbType = "BigInt")] System.Nullable<long> typeOfRequestID, [Parameter(Name = "KeyWord", DbType = "NVarChar(50)")] string keyWord, [Parameter(Name = "SubOwnerID", DbType = "BigInt")] System.Nullable<long> subOwnerID, [Parameter(Name = "FromDate", DbType = "NVarChar(10)")] string fromDate, [Parameter(Name = "ToDate", DbType = "NVarChar(10)")] string toDate, [Parameter(Name = "SpecificNoteFor", DbType = "NVarChar(100)")] string specificNoteFor, [Parameter(Name = "UserInfoID", DbType = "BigInt")] System.Nullable<long> userInfoID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), companyID, contactID, openedByID, statusID, ownerID, ticketName, ticketNumber, dateNeeded, supplierID, typeOfRequestID, keyWord, subOwnerID, fromDate, toDate, specificNoteFor, userInfoID);
            return (IMultipleResults)(result.ReturnValue);
        }

        [Function(Name = "dbo.GetServiceTicketActivitiesNotes")]
        [ResultType(typeof(FUN_GetServiceTicketNoteResult))]
        public IMultipleResults GetServiceTicketActivitiesNotes([Parameter(Name = "SpecificNoteFor", DbType = "NVarChar(100)")] string specificNoteFor, [Parameter(Name = "UserInfoID", DbType = "BigInt")] System.Nullable<long> userInfoID, [Parameter(Name = "TicketID", DbType = "BigInt")] System.Nullable<long> TicketID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), specificNoteFor, userInfoID, TicketID);
            return (IMultipleResults)(result.ReturnValue);
        }


        //[Function(Name = "dbo.SelectServiceTicketsBySearchCriteria")]
        [Function(Name = "dbo.SelectServiceTicketsOnlyBySearchCriteria")]
        public ISingleResult<ServiceTicketsSearchResult> SelectServiceTicketsBySearchCriteria([Parameter(Name = "CompanyID", DbType = "BigInt")] System.Nullable<long> companyID, [Parameter(Name = "ContactID", DbType = "BigInt")] System.Nullable<long> contactID, [Parameter(Name = "OpenedByID", DbType = "BigInt")] System.Nullable<long> openedByID, [Parameter(Name = "StatusID", DbType = "BigInt")] System.Nullable<long> statusID, [Parameter(Name = "OwnerID", DbType = "BigInt")] System.Nullable<long> ownerID, [Parameter(Name = "TicketName", DbType = "NVarChar(150)")] string ticketName, [Parameter(Name = "TicketNumber", DbType = "VarChar(10)")] string ticketNumber, [Parameter(Name = "DateNeeded", DbType = "VarChar(10)")] string dateNeeded, [Parameter(Name = "SupplierID", DbType = "BigInt")] System.Nullable<long> supplierID, [Parameter(Name = "TypeOfRequestID", DbType = "BigInt")] System.Nullable<long> typeOfRequestID, [Parameter(Name = "KeyWord", DbType = "NVarChar(50)")] string keyWord, [Parameter(Name = "SubOwnerID", DbType = "BigInt")] System.Nullable<long> subOwnerID, [Parameter(Name = "FromDate", DbType = "VarChar(10)")] string fromDate, [Parameter(Name = "ToDate", DbType = "VarChar(10)")] string toDate, [Parameter(Name = "UserInfoID", DbType = "BigInt")] System.Nullable<long> userInfoID, [Parameter(Name = "NoActivity", DbType = "Bit")] System.Nullable<bool> noActivity)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), companyID, contactID, openedByID, statusID, ownerID, ticketName, ticketNumber, dateNeeded, supplierID, typeOfRequestID, keyWord, subOwnerID, fromDate, toDate, userInfoID, noActivity);
            return ((ISingleResult<ServiceTicketsSearchResult>)(result.ReturnValue));
        }

        
        [Function(Name = "dbo.SelectUnReadNotesBySearchCriteria")]
        public ISingleResult<ServiceTicketsSearchResult>SelectUnReadNotesBySearchCriteria([Parameter(Name = "UserInfoID", DbType = "BigInt")] System.Nullable<long> userInfoID, [Parameter(Name = "ServiceTicketID", DbType = "BigInt")] System.Nullable<long> ServiceTicketID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userInfoID, ServiceTicketID);
            return ((ISingleResult<ServiceTicketsSearchResult>)(result.ReturnValue));
        }


        [Function(Name = "dbo.SelectServiceTicketsBySearchCriteriaForCA")]
        public ISingleResult<ServiceTicketsSearchResult> SelectServiceTicketsBySearchCriteriaForCA([Parameter(Name = "cAUserInfoID", DbType = "BigInt")] System.Nullable<long> cAUserInfoID, [Parameter(Name = "TicketName", DbType = "NVarChar(150)")] string ticketName, [Parameter(Name = "TicketNumber", DbType = "VarChar(10)")] string ticketNumber, [Parameter(Name = "WorkGroupID", DbType = "BigInt")] System.Nullable<long> workGroupID, [Parameter(Name = "ContactID", DbType = "BigInt")] System.Nullable<long> contactID, [Parameter(Name = "StatusID", DbType = "BigInt")] System.Nullable<long> statusID, [Parameter(Name = "OwnerID", DbType = "BigInt")] System.Nullable<long> ownerID, [Parameter(Name = "KeyWord", DbType = "NVarChar(50)")] string keyWord, [Parameter(Name = "AllowedToSearch", DbType = "NVarChar(50)")] string allowedToSearch)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), cAUserInfoID, ticketName, ticketNumber, workGroupID, contactID, statusID, ownerID, keyWord, allowedToSearch);
            return ((ISingleResult<ServiceTicketsSearchResult>)(result.ReturnValue));
        }

        //[Function(Name = "dbo.SelectServiceTicketAssociatedWithIEs")]
        [Function(Name = "dbo.SelectServiceTicketOnlyAssociatedWithIEs")]
        public ISingleResult<ServiceTicketsSearchResult> SelectServiceTicketAssociatedWithIEs([Parameter(Name = "UserInfoId", DbType = "BigInt")] System.Nullable<long> userInfoId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userInfoId);
            return ((ISingleResult<ServiceTicketsSearchResult>)(result.ReturnValue));
        }

        [Function(Name = "dbo.SelectServiceTicketRecentlyUpdated")]
        public ISingleResult<ServiceTicketsSearchResult> SelectRecentlyUpdatedServiceTicket([Parameter(Name = "UserInfoId", DbType = "BigInt")] System.Nullable<long> userInfoId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userInfoId);
            return ((ISingleResult<ServiceTicketsSearchResult>)(result.ReturnValue));
        }

        [Function(Name = "dbo.SelectServiceTicketHistoryPerEmployee")]
        public ISingleResult<SelectServiceTicketHistoryPerEmployeeResult> SelectServiceTicketHistoryPerEmployee([Parameter(Name = "UserInfoId", DbType = "BigInt")] System.Nullable<long> userInfoId, [Parameter(Name = "QuickView", DbType = "Bit")] System.Nullable<Boolean> quickView)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userInfoId, quickView);
            return ((ISingleResult<SelectServiceTicketHistoryPerEmployeeResult>)(result.ReturnValue));
        }

        [Function(Name = "dbo.SelectServiceTicketForCACERecentlyUpdated")]
        public ISingleResult<SelectServiceTicketHistoryPerEmployeeResult> SelectRecentlyUpdatedServiceTicketForCACE([Parameter(Name = "UserInfoId", DbType = "BigInt")] System.Nullable<long> userInfoId)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userInfoId);
            return ((ISingleResult<SelectServiceTicketHistoryPerEmployeeResult>)(result.ReturnValue));
        }

        [Function(Name = "dbo.SelectShippedQuantityBySupplier")]
        public ISingleResult<SelectShippedQuantityResult> SelectShippedQuantityBySupplier([Parameter(Name = "SupplierID", DbType = "BigInt")] System.Nullable<long> supplierID, [Parameter(Name = "OrderID", DbType = "BigInt")] System.Nullable<long> orderID)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), supplierID, orderID);
            return ((ISingleResult<SelectShippedQuantityResult>)(result.ReturnValue));
        }
    }

    [Serializable]
    public partial class ServiceTicketsSearchResult
    {
        private long _ServiceTicketID;

        private string _ServiceTicketNumber;

        private string _ServiceTicketName;

        private System.Nullable<long> _ServiceTicketOwnerID;

        private System.Nullable<System.DateTime> _DatePromised;

        private string _ServiceTicketDetails;

        private System.Nullable<long> _TicketStatusID;

        private System.Nullable<long> _CompanyID;

        private System.Nullable<long> _ContactID;

        private System.Nullable<long> _CreatedBy;

        private System.Nullable<System.DateTime> _CreatedDate;

        private System.Nullable<long> _UpdatedBy;

        private System.Nullable<System.DateTime> _UpdatedDate;

        private string _ContactName;

        private string _StartDateNTime;

        private string _TicketStatus;

        private string _OpenedByName;

        private string _UpdateByName;

        private string _UpdatedDateNTime;

        private string _CompanyName;

        private string _OwnerName;

        private string _DatePromisedFormatted;

        private string _ContactEmail;

        private string _ContactTelephone;

        private System.Nullable<System.DateTime> _EndDate;

        private string _EndDateNTime;

        private string _FirstName;

        private string _LastName;

        private string _SupplierName;

        private System.Nullable<long> _SupplierID;

        private System.Nullable<long> _TypeOfRequestID;

        private string _TypeOfRequest;

        private string _TicketEmail;

        private System.Nullable<int> _UnReadCount;

        private int _UnReadNotesExists;

        private Int32 _IsFlagged;

        private string _SupplierEmail;

        private string _SupplierTelephone;

        private System.Nullable<long> _WorkGroupID;

        private string _WorkGroup;

        private Int32 _IsSupplierTicket;

        private System.Nullable<long> _BaseStationID;

        private string _BaseStation;

        public ServiceTicketsSearchResult()
        {
        }

        [Column(Storage = "_ServiceTicketID", DbType = "BigInt NOT NULL")]
        public long ServiceTicketID
        {
            get
            {
                return this._ServiceTicketID;
            }
            set
            {
                if ((this._ServiceTicketID != value))
                {
                    this._ServiceTicketID = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketNumber", DbType = "NVarChar(100)")]
        public string ServiceTicketNumber
        {
            get
            {
                return this._ServiceTicketNumber;
            }
            set
            {
                if ((this._ServiceTicketNumber != value))
                {
                    this._ServiceTicketNumber = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketName", DbType = "NVarChar(150)")]
        public string ServiceTicketName
        {
            get
            {
                return this._ServiceTicketName;
            }
            set
            {
                if ((this._ServiceTicketName != value))
                {
                    this._ServiceTicketName = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketOwnerID", DbType = "BigInt")]
        public System.Nullable<long> ServiceTicketOwnerID
        {
            get
            {
                return this._ServiceTicketOwnerID;
            }
            set
            {
                if ((this._ServiceTicketOwnerID != value))
                {
                    this._ServiceTicketOwnerID = value;
                }
            }
        }

        [Column(Storage = "_DatePromised", DbType = "DateTime")]
        public System.Nullable<System.DateTime> DatePromised
        {
            get
            {
                return this._DatePromised;
            }
            set
            {
                if ((this._DatePromised != value))
                {
                    this._DatePromised = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketDetails", DbType = "NVarChar(2500)")]
        public string ServiceTicketDetails
        {
            get
            {
                return this._ServiceTicketDetails;
            }
            set
            {
                if ((this._ServiceTicketDetails != value))
                {
                    this._ServiceTicketDetails = value;
                }
            }
        }

        [Column(Storage = "_TicketStatusID", DbType = "BigInt")]
        public System.Nullable<long> TicketStatusID
        {
            get
            {
                return this._TicketStatusID;
            }
            set
            {
                if ((this._TicketStatusID != value))
                {
                    this._TicketStatusID = value;
                }
            }
        }

        [Column(Storage = "_CompanyID", DbType = "BigInt")]
        public System.Nullable<long> CompanyID
        {
            get
            {
                return this._CompanyID;
            }
            set
            {
                if ((this._CompanyID != value))
                {
                    this._CompanyID = value;
                }
            }
        }

        [Column(Storage = "_ContactID", DbType = "BigInt")]
        public System.Nullable<long> ContactID
        {
            get
            {
                return this._ContactID;
            }
            set
            {
                if ((this._ContactID != value))
                {
                    this._ContactID = value;
                }
            }
        }

        [Column(Storage = "_CreatedBy", DbType = "BigInt")]
        public System.Nullable<long> CreatedBy
        {
            get
            {
                return this._CreatedBy;
            }
            set
            {
                if ((this._CreatedBy != value))
                {
                    this._CreatedBy = value;
                }
            }
        }

        [Column(Storage = "_CreatedDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> CreatedDate
        {
            get
            {
                return this._CreatedDate;
            }
            set
            {
                if ((this._CreatedDate != value))
                {
                    this._CreatedDate = value;
                }
            }
        }

        [Column(Storage = "_UpdatedBy", DbType = "BigInt")]
        public System.Nullable<long> UpdatedBy
        {
            get
            {
                return this._UpdatedBy;
            }
            set
            {
                if ((this._UpdatedBy != value))
                {
                    this._UpdatedBy = value;
                }
            }
        }

        [Column(Storage = "_UpdatedDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> UpdatedDate
        {
            get
            {
                return this._UpdatedDate;
            }
            set
            {
                if ((this._UpdatedDate != value))
                {
                    this._UpdatedDate = value;
                }
            }
        }

        [Column(Storage = "_ContactName", DbType = "NVarChar(201)")]
        public string ContactName
        {
            get
            {
                return this._ContactName;
            }
            set
            {
                if ((this._ContactName != value))
                {
                    this._ContactName = value;
                }
            }
        }

        [Column(Storage = "_StartDateNTime", DbType = "VarChar(41)")]
        public string StartDateNTime
        {
            get
            {
                return this._StartDateNTime;
            }
            set
            {
                if ((this._StartDateNTime != value))
                {
                    this._StartDateNTime = value;
                }
            }
        }

        [Column(Storage = "_TicketStatus", DbType = "NVarChar(100)")]
        public string TicketStatus
        {
            get
            {
                return this._TicketStatus;
            }
            set
            {
                if ((this._TicketStatus != value))
                {
                    this._TicketStatus = value;
                }
            }
        }

        [Column(Storage = "_OpenedByName", DbType = "NVarChar(201)")]
        public string OpenedByName
        {
            get
            {
                return this._OpenedByName;
            }
            set
            {
                if ((this._OpenedByName != value))
                {
                    this._OpenedByName = value;
                }
            }
        }

        [Column(Storage = "_UpdateByName", DbType = "NVarChar(201)")]
        public string UpdateByName
        {
            get
            {
                return this._UpdateByName;
            }
            set
            {
                if ((this._UpdateByName != value))
                {
                    this._UpdateByName = value;
                }
            }
        }

        [Column(Storage = "_UpdatedDateNTime", DbType = "VarChar(41)")]
        public string UpdatedDateNTime
        {
            get
            {
                return this._UpdatedDateNTime;
            }
            set
            {
                if ((this._UpdatedDateNTime != value))
                {
                    this._UpdatedDateNTime = value;
                }
            }
        }

        [Column(Storage = "_CompanyName", DbType = "NVarChar(100)")]
        public string CompanyName
        {
            get
            {
                return this._CompanyName;
            }
            set
            {
                if ((this._CompanyName != value))
                {
                    this._CompanyName = value;
                }
            }
        }

        [Column(Storage = "_OwnerName", DbType = "NVarChar(201)")]
        public string OwnerName
        {
            get
            {
                return this._OwnerName;
            }
            set
            {
                if ((this._OwnerName != value))
                {
                    this._OwnerName = value;
                }
            }
        }

        [Column(Storage = "_DatePromisedFormatted", DbType = "VarChar(30)")]
        public string DatePromisedFormatted
        {
            get
            {
                return this._DatePromisedFormatted;
            }
            set
            {
                if ((this._DatePromisedFormatted != value))
                {
                    this._DatePromisedFormatted = value;
                }
            }
        }

        [Column(Storage = "_ContactEmail", DbType = "NVarChar(50)")]
        public string ContactEmail
        {
            get
            {
                return this._ContactEmail;
            }
            set
            {
                if ((this._ContactEmail != value))
                {
                    this._ContactEmail = value;
                }
            }
        }

        [Column(Storage = "_ContactTelephone", DbType = "NVarChar(100)")]
        public string ContactTelephone
        {
            get
            {
                return this._ContactTelephone;
            }
            set
            {
                if ((this._ContactTelephone != value))
                {
                    this._ContactTelephone = value;
                }
            }
        }

        [Column(Storage = "_EndDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> EndDate
        {
            get
            {
                return this._EndDate;
            }
            set
            {
                if ((this._EndDate != value))
                {
                    this._EndDate = value;
                }
            }
        }

        [Column(Storage = "_EndDateNTime", DbType = "VarChar(41)")]
        public string EndDateNTime
        {
            get
            {
                return this._EndDateNTime;
            }
            set
            {
                if ((this._EndDateNTime != value))
                {
                    this._EndDateNTime = value;
                }
            }
        }

        [Column(Storage = "_FirstName", DbType = "NVarChar(100)")]
        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                if ((this._FirstName != value))
                {
                    this._FirstName = value;
                }
            }
        }

        [Column(Storage = "_LastName", DbType = "NVarChar(100)")]
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                if ((this._LastName != value))
                {
                    this._LastName = value;
                }
            }
        }

        [Column(Storage = "_SupplierName", DbType = "VarChar(100)")]
        public string SupplierName
        {
            get
            {
                return this._SupplierName;
            }
            set
            {
                if ((this._SupplierName != value))
                {
                    this._SupplierName = value;
                }
            }
        }

        [Column(Storage = "_SupplierID", DbType = "BigInt")]
        public System.Nullable<long> SupplierID
        {
            get
            {
                return this._SupplierID;
            }
            set
            {
                if ((this._SupplierID != value))
                {
                    this._SupplierID = value;
                }
            }
        }

        [Column(Storage = "_TypeOfRequestID", DbType = "BigInt")]
        public System.Nullable<long> TypeOfRequestID
        {
            get
            {
                return this._TypeOfRequestID;
            }
            set
            {
                if ((this._TypeOfRequestID != value))
                {
                    this._TypeOfRequestID = value;
                }
            }
        }

        [Column(Storage = "_TypeOfRequest", DbType = "NVarChar(100)")]
        public string TypeOfRequest
        {
            get
            {
                return this._TypeOfRequest;
            }
            set
            {
                if ((this._TypeOfRequest != value))
                {
                    this._TypeOfRequest = value;
                }
            }
        }

        [Column(Storage = "_TicketEmail", DbType = "NVarChar(100)")]
        public string TicketEmail
        {
            get
            {
                return this._TicketEmail;
            }
            set
            {
                if ((this._TicketEmail != value))
                {
                    this._TicketEmail = value;
                }
            }
        }

        [Column(Storage = "_UnReadCount", DbType = "Int")]
        public System.Nullable<int> UnReadCount
        {
            get
            {
                return this._UnReadCount;
            }
            set
            {
                if ((this._UnReadCount != value))
                {
                    this._UnReadCount = value;
                }
            }
        }

        [Column(Storage = "_UnReadNotesExists", DbType = "Int NOT NULL")]
        public int UnReadNotesExists
        {
            get
            {
                return this._UnReadNotesExists;
            }
            set
            {
                if ((this._UnReadNotesExists != value))
                {
                    this._UnReadNotesExists = value;
                }
            }
        }

        [Column(Storage = "_IsFlagged", DbType = "Int NOT NULL")]
        public Int32 IsFlagged
        {
            get
            {
                return this._IsFlagged;
            }
            set
            {
                if ((this._IsFlagged != value))
                {
                    this._IsFlagged = value;
                }
            }
        }

        [Column(Storage = "_SupplierEmail", DbType = "NVarChar(50)")]
        public string SupplierEmail
        {
            get
            {
                return this._SupplierEmail;
            }
            set
            {
                if ((this._SupplierEmail != value))
                {
                    this._SupplierEmail = value;
                }
            }
        }

        [Column(Storage = "_SupplierTelephone", DbType = "NVarChar(100)")]
        public string SupplierTelephone
        {
            get
            {
                return this._SupplierTelephone;
            }
            set
            {
                if ((this._SupplierTelephone != value))
                {
                    this._SupplierTelephone = value;
                }
            }
        }

        [Column(Storage = "_WorkGroupID", DbType = "BigInt")]
        public System.Nullable<long> WorkGroupID
        {
            get
            {
                return this._WorkGroupID;
            }
            set
            {
                if ((this._WorkGroupID != value))
                {
                    this._WorkGroupID = value;
                }
            }
        }

        [Column(Storage = "_WorkGroup", DbType = "NVarChar(100)")]
        public string WorkGroup
        {
            get
            {
                return this._WorkGroup;
            }
            set
            {
                if ((this._WorkGroup != value))
                {
                    this._WorkGroup = value;
                }
            }
        }

        [Column(Storage = "_IsSupplierTicket", DbType = "Int NOT NULL")]
        public Int32 IsSupplierTicket
        {
            get
            {
                return this._IsSupplierTicket;
            }
            set
            {
                if ((this._IsSupplierTicket != value))
                {
                    this._IsSupplierTicket = value;
                }
            }
        }

        [Column(Storage = "_BaseStationID", DbType = "BigInt")]
        public System.Nullable<long> BaseStationID
        {
            get
            {
                return this._BaseStationID;
            }
            set
            {
                if ((this._BaseStationID != value))
                {
                    this._BaseStationID = value;
                }
            }
        }

        [Column(Storage = "_BaseStation", DbType = "NVarChar(500)")]
        public string BaseStation
        {
            get
            {
                return this._BaseStation;
            }
            set
            {
                if ((this._BaseStation != value))
                {
                    this._BaseStation = value;
                }
            }
        }
    }

    [Serializable]
    public partial class SelectServiceTicketHistoryPerEmployeeResult
    {
        private System.Nullable<long> _CompanyID;

        private string _CompanyName;

        private string _ContactEmail;

        private System.Nullable<long> _ContactID;

        private string _ContactName;

        private string _ContactTelephone;

        private System.Nullable<long> _CreatedBy;

        private System.Nullable<System.DateTime> _CreatedDate;

        private System.Nullable<System.DateTime> _DatePromised;

        private string _DatePromisedFormatted;

        private System.Nullable<System.DateTime> _EndDate;

        private string _EndDateNTime;

        private string _FirstName;

        private string _LastName;

        private string _OpenedByName;

        private string _OwnerName;

        private string _ServiceTicketDetails;

        private long _ServiceTicketID;

        private string _ServiceTicketName;

        private string _ServiceTicketNumber;

        private System.Nullable<long> _ServiceTicketOwnerID;

        private string _StartDateNTime;

        private System.Nullable<long> _SupplierID;

        private string _SupplierName;

        private string _TicketEmail;

        private string _TicketStatus;

        private System.Nullable<long> _TicketStatusID;

        private string _TypeOfRequest;

        private System.Nullable<long> _TypeOfRequestID;

        private string _UpdateByName;

        private System.Nullable<long> _UpdatedBy;

        private System.Nullable<System.DateTime> _UpdatedDate;

        private string _UpdatedDateNTime;

        private System.Nullable<int> _UnReadCount;

        private int _UnReadNotesExists;

        private Int32 _IsFlagged;

        private string _SupplierEmail;

        private string _SupplierTelephone;

        private System.Nullable<long> _WorkGroupID;

        private string _WorkGroup;

        private Int32 _IsSupplierTicket;

        private System.Nullable<long> _BaseStationID;

        private string _BaseStation;

        public SelectServiceTicketHistoryPerEmployeeResult()
        {
        }

        [Column(Storage = "_CompanyID", DbType = "BigInt")]
        public System.Nullable<long> CompanyID
        {
            get
            {
                return this._CompanyID;
            }
            set
            {
                if ((this._CompanyID != value))
                {
                    this._CompanyID = value;
                }
            }
        }

        [Column(Storage = "_CompanyName", DbType = "NVarChar(100)")]
        public string CompanyName
        {
            get
            {
                return this._CompanyName;
            }
            set
            {
                if ((this._CompanyName != value))
                {
                    this._CompanyName = value;
                }
            }
        }

        [Column(Storage = "_ContactEmail", DbType = "NVarChar(50)")]
        public string ContactEmail
        {
            get
            {
                return this._ContactEmail;
            }
            set
            {
                if ((this._ContactEmail != value))
                {
                    this._ContactEmail = value;
                }
            }
        }

        [Column(Storage = "_ContactID", DbType = "BigInt")]
        public System.Nullable<long> ContactID
        {
            get
            {
                return this._ContactID;
            }
            set
            {
                if ((this._ContactID != value))
                {
                    this._ContactID = value;
                }
            }
        }

        [Column(Storage = "_ContactName", DbType = "NVarChar(201)")]
        public string ContactName
        {
            get
            {
                return this._ContactName;
            }
            set
            {
                if ((this._ContactName != value))
                {
                    this._ContactName = value;
                }
            }
        }

        [Column(Storage = "_ContactTelephone", DbType = "NVarChar(100)")]
        public string ContactTelephone
        {
            get
            {
                return this._ContactTelephone;
            }
            set
            {
                if ((this._ContactTelephone != value))
                {
                    this._ContactTelephone = value;
                }
            }
        }

        [Column(Storage = "_CreatedBy", DbType = "BigInt")]
        public System.Nullable<long> CreatedBy
        {
            get
            {
                return this._CreatedBy;
            }
            set
            {
                if ((this._CreatedBy != value))
                {
                    this._CreatedBy = value;
                }
            }
        }

        [Column(Storage = "_CreatedDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> CreatedDate
        {
            get
            {
                return this._CreatedDate;
            }
            set
            {
                if ((this._CreatedDate != value))
                {
                    this._CreatedDate = value;
                }
            }
        }

        [Column(Storage = "_DatePromised", DbType = "DateTime")]
        public System.Nullable<System.DateTime> DatePromised
        {
            get
            {
                return this._DatePromised;
            }
            set
            {
                if ((this._DatePromised != value))
                {
                    this._DatePromised = value;
                }
            }
        }

        [Column(Storage = "_DatePromisedFormatted", DbType = "VarChar(30)")]
        public string DatePromisedFormatted
        {
            get
            {
                return this._DatePromisedFormatted;
            }
            set
            {
                if ((this._DatePromisedFormatted != value))
                {
                    this._DatePromisedFormatted = value;
                }
            }
        }

        [Column(Storage = "_EndDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> EndDate
        {
            get
            {
                return this._EndDate;
            }
            set
            {
                if ((this._EndDate != value))
                {
                    this._EndDate = value;
                }
            }
        }

        [Column(Storage = "_EndDateNTime", DbType = "VarChar(41)")]
        public string EndDateNTime
        {
            get
            {
                return this._EndDateNTime;
            }
            set
            {
                if ((this._EndDateNTime != value))
                {
                    this._EndDateNTime = value;
                }
            }
        }

        [Column(Storage = "_FirstName", DbType = "NVarChar(100)")]
        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                if ((this._FirstName != value))
                {
                    this._FirstName = value;
                }
            }
        }

        [Column(Storage = "_LastName", DbType = "NVarChar(100)")]
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                if ((this._LastName != value))
                {
                    this._LastName = value;
                }
            }
        }

        [Column(Storage = "_OpenedByName", DbType = "NVarChar(201)")]
        public string OpenedByName
        {
            get
            {
                return this._OpenedByName;
            }
            set
            {
                if ((this._OpenedByName != value))
                {
                    this._OpenedByName = value;
                }
            }
        }

        [Column(Storage = "_OwnerName", DbType = "NVarChar(201)")]
        public string OwnerName
        {
            get
            {
                return this._OwnerName;
            }
            set
            {
                if ((this._OwnerName != value))
                {
                    this._OwnerName = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketDetails", DbType = "NVarChar(2500)")]
        public string ServiceTicketDetails
        {
            get
            {
                return this._ServiceTicketDetails;
            }
            set
            {
                if ((this._ServiceTicketDetails != value))
                {
                    this._ServiceTicketDetails = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketID", DbType = "BigInt NOT NULL")]
        public long ServiceTicketID
        {
            get
            {
                return this._ServiceTicketID;
            }
            set
            {
                if ((this._ServiceTicketID != value))
                {
                    this._ServiceTicketID = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketName", DbType = "NVarChar(150)")]
        public string ServiceTicketName
        {
            get
            {
                return this._ServiceTicketName;
            }
            set
            {
                if ((this._ServiceTicketName != value))
                {
                    this._ServiceTicketName = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketNumber", DbType = "NVarChar(100)")]
        public string ServiceTicketNumber
        {
            get
            {
                return this._ServiceTicketNumber;
            }
            set
            {
                if ((this._ServiceTicketNumber != value))
                {
                    this._ServiceTicketNumber = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketOwnerID", DbType = "BigInt")]
        public System.Nullable<long> ServiceTicketOwnerID
        {
            get
            {
                return this._ServiceTicketOwnerID;
            }
            set
            {
                if ((this._ServiceTicketOwnerID != value))
                {
                    this._ServiceTicketOwnerID = value;
                }
            }
        }

        [Column(Storage = "_StartDateNTime", DbType = "VarChar(41)")]
        public string StartDateNTime
        {
            get
            {
                return this._StartDateNTime;
            }
            set
            {
                if ((this._StartDateNTime != value))
                {
                    this._StartDateNTime = value;
                }
            }
        }

        [Column(Storage = "_SupplierID", DbType = "BigInt")]
        public System.Nullable<long> SupplierID
        {
            get
            {
                return this._SupplierID;
            }
            set
            {
                if ((this._SupplierID != value))
                {
                    this._SupplierID = value;
                }
            }
        }

        [Column(Storage = "_SupplierName", DbType = "VarChar(100)")]
        public string SupplierName
        {
            get
            {
                return this._SupplierName;
            }
            set
            {
                if ((this._SupplierName != value))
                {
                    this._SupplierName = value;
                }
            }
        }

        [Column(Storage = "_TicketEmail", DbType = "NVarChar(100)")]
        public string TicketEmail
        {
            get
            {
                return this._TicketEmail;
            }
            set
            {
                if ((this._TicketEmail != value))
                {
                    this._TicketEmail = value;
                }
            }
        }

        [Column(Storage = "_TicketStatus", DbType = "NVarChar(100)")]
        public string TicketStatus
        {
            get
            {
                return this._TicketStatus;
            }
            set
            {
                if ((this._TicketStatus != value))
                {
                    this._TicketStatus = value;
                }
            }
        }

        [Column(Storage = "_TicketStatusID", DbType = "BigInt")]
        public System.Nullable<long> TicketStatusID
        {
            get
            {
                return this._TicketStatusID;
            }
            set
            {
                if ((this._TicketStatusID != value))
                {
                    this._TicketStatusID = value;
                }
            }
        }

        [Column(Storage = "_TypeOfRequest", DbType = "NVarChar(100)")]
        public string TypeOfRequest
        {
            get
            {
                return this._TypeOfRequest;
            }
            set
            {
                if ((this._TypeOfRequest != value))
                {
                    this._TypeOfRequest = value;
                }
            }
        }

        [Column(Storage = "_TypeOfRequestID", DbType = "BigInt")]
        public System.Nullable<long> TypeOfRequestID
        {
            get
            {
                return this._TypeOfRequestID;
            }
            set
            {
                if ((this._TypeOfRequestID != value))
                {
                    this._TypeOfRequestID = value;
                }
            }
        }

        [Column(Storage = "_UpdateByName", DbType = "NVarChar(201)")]
        public string UpdateByName
        {
            get
            {
                return this._UpdateByName;
            }
            set
            {
                if ((this._UpdateByName != value))
                {
                    this._UpdateByName = value;
                }
            }
        }

        [Column(Storage = "_UpdatedBy", DbType = "BigInt")]
        public System.Nullable<long> UpdatedBy
        {
            get
            {
                return this._UpdatedBy;
            }
            set
            {
                if ((this._UpdatedBy != value))
                {
                    this._UpdatedBy = value;
                }
            }
        }

        [Column(Storage = "_UpdatedDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> UpdatedDate
        {
            get
            {
                return this._UpdatedDate;
            }
            set
            {
                if ((this._UpdatedDate != value))
                {
                    this._UpdatedDate = value;
                }
            }
        }

        [Column(Storage = "_UpdatedDateNTime", DbType = "VarChar(41)")]
        public string UpdatedDateNTime
        {
            get
            {
                return this._UpdatedDateNTime;
            }
            set
            {
                if ((this._UpdatedDateNTime != value))
                {
                    this._UpdatedDateNTime = value;
                }
            }
        }

        [Column(Storage = "_UnReadCount", DbType = "Int")]
        public System.Nullable<int> UnReadCount
        {
            get
            {
                return this._UnReadCount;
            }
            set
            {
                if ((this._UnReadCount != value))
                {
                    this._UnReadCount = value;
                }
            }
        }

        [Column(Storage = "_UnReadNotesExists", DbType = "Int NOT NULL")]
        public int UnReadNotesExists
        {
            get
            {
                return this._UnReadNotesExists;
            }
            set
            {
                if ((this._UnReadNotesExists != value))
                {
                    this._UnReadNotesExists = value;
                }
            }
        }

        [Column(Storage = "_IsFlagged", DbType = "Int NOT NULL")]
        public Int32 IsFlagged
        {
            get
            {
                return this._IsFlagged;
            }
            set
            {
                if ((this._IsFlagged != value))
                {
                    this._IsFlagged = value;
                }
            }
        }

        [Column(Storage = "_SupplierEmail", DbType = "NVarChar(50)")]
        public string SupplierEmail
        {
            get
            {
                return this._SupplierEmail;
            }
            set
            {
                if ((this._SupplierEmail != value))
                {
                    this._SupplierEmail = value;
                }
            }
        }

        [Column(Storage = "_SupplierTelephone", DbType = "NVarChar(100)")]
        public string SupplierTelephone
        {
            get
            {
                return this._SupplierTelephone;
            }
            set
            {
                if ((this._SupplierTelephone != value))
                {
                    this._SupplierTelephone = value;
                }
            }
        }

        [Column(Storage = "_WorkGroupID", DbType = "BigInt")]
        public System.Nullable<long> WorkGroupID
        {
            get
            {
                return this._WorkGroupID;
            }
            set
            {
                if ((this._WorkGroupID != value))
                {
                    this._WorkGroupID = value;
                }
            }
        }

        [Column(Storage = "_WorkGroup", DbType = "NVarChar(100)")]
        public string WorkGroup
        {
            get
            {
                return this._WorkGroup;
            }
            set
            {
                if ((this._WorkGroup != value))
                {
                    this._WorkGroup = value;
                }
            }
        }

        [Column(Storage = "_IsSupplierTicket", DbType = "Int NOT NULL")]
        public Int32 IsSupplierTicket
        {
            get
            {
                return this._IsSupplierTicket;
            }
            set
            {
                if ((this._IsSupplierTicket != value))
                {
                    this._IsSupplierTicket = value;
                }
            }
        }

        [Column(Storage = "_BaseStationID", DbType = "BigInt")]
        public System.Nullable<long> BaseStationID
        {
            get
            {
                return this._BaseStationID;
            }
            set
            {
                if ((this._BaseStationID != value))
                {
                    this._BaseStationID = value;
                }
            }
        }

        [Column(Storage = "_BaseStation", DbType = "NVarChar(500)")]
        public string BaseStation
        {
            get
            {
                return this._BaseStation;
            }
            set
            {
                if ((this._BaseStation != value))
                {
                    this._BaseStation = value;
                }
            }
        }
    }

    public partial class GetServiceTicketActivitiesResult
    {
        private long _ServiceTicketID;

        private string _ServiceTicketNumber;

        private string _ServiceTicketName;

        private System.Nullable<long> _ServiceTicketOwnerID;

        private System.Nullable<System.DateTime> _DatePromised;

        private string _ServiceTicketDetails;

        private System.Nullable<long> _TicketStatusID;

        private System.Nullable<long> _CompanyID;

        private System.Nullable<long> _ContactID;

        private System.Nullable<long> _CreatedBy;

        private System.Nullable<System.DateTime> _CreatedDate;

        private System.Nullable<long> _UpdatedBy;

        private System.Nullable<System.DateTime> _UpdatedDate;

        private string _ContactName;

        private string _StartDateNTime;

        private string _TicketStatus;

        private string _OpenedByName;

        private string _UpdateByName;

        private string _UpdatedDateNTime;

        private string _CompanyName;

        private string _OwnerName;

        private string _DatePromisedFormatted;

        private string _ContactEmail;

        private string _ContactTelephone;

        private System.Nullable<System.DateTime> _EndDate;

        private string _EndDateNTime;

        private string _FirstName;

        private string _LastName;

        private string _SupplierName;

        private System.Nullable<long> _SupplierID;

        private System.Nullable<long> _TypeOfRequestID;

        private string _TypeOfRequest;

        private string _TicketEmail;

        private string _SupplierEmail;

        private string _SupplierTelephone;

        private System.Nullable<long> _WorkGroupID;

        private string _WorkGroup;

        private int _IsSupplierTicket;

        private int _IsFlagged;

        private System.Nullable<long> _BaseStationID;

        private string _BaseStation;

        public GetServiceTicketActivitiesResult()
        {
        }

        [Column(Storage = "_ServiceTicketID", DbType = "BigInt NOT NULL")]
        public long ServiceTicketID
        {
            get
            {
                return this._ServiceTicketID;
            }
            set
            {
                if ((this._ServiceTicketID != value))
                {
                    this._ServiceTicketID = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketNumber", DbType = "NVarChar(100)")]
        public string ServiceTicketNumber
        {
            get
            {
                return this._ServiceTicketNumber;
            }
            set
            {
                if ((this._ServiceTicketNumber != value))
                {
                    this._ServiceTicketNumber = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketName", DbType = "NVarChar(150)")]
        public string ServiceTicketName
        {
            get
            {
                return this._ServiceTicketName;
            }
            set
            {
                if ((this._ServiceTicketName != value))
                {
                    this._ServiceTicketName = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketOwnerID", DbType = "BigInt")]
        public System.Nullable<long> ServiceTicketOwnerID
        {
            get
            {
                return this._ServiceTicketOwnerID;
            }
            set
            {
                if ((this._ServiceTicketOwnerID != value))
                {
                    this._ServiceTicketOwnerID = value;
                }
            }
        }

        [Column(Storage = "_DatePromised", DbType = "DateTime")]
        public System.Nullable<System.DateTime> DatePromised
        {
            get
            {
                return this._DatePromised;
            }
            set
            {
                if ((this._DatePromised != value))
                {
                    this._DatePromised = value;
                }
            }
        }

        [Column(Storage = "_ServiceTicketDetails", DbType = "NVarChar(2500)")]
        public string ServiceTicketDetails
        {
            get
            {
                return this._ServiceTicketDetails;
            }
            set
            {
                if ((this._ServiceTicketDetails != value))
                {
                    this._ServiceTicketDetails = value;
                }
            }
        }

        [Column(Storage = "_TicketStatusID", DbType = "BigInt")]
        public System.Nullable<long> TicketStatusID
        {
            get
            {
                return this._TicketStatusID;
            }
            set
            {
                if ((this._TicketStatusID != value))
                {
                    this._TicketStatusID = value;
                }
            }
        }

        [Column(Storage = "_CompanyID", DbType = "BigInt")]
        public System.Nullable<long> CompanyID
        {
            get
            {
                return this._CompanyID;
            }
            set
            {
                if ((this._CompanyID != value))
                {
                    this._CompanyID = value;
                }
            }
        }

        [Column(Storage = "_ContactID", DbType = "BigInt")]
        public System.Nullable<long> ContactID
        {
            get
            {
                return this._ContactID;
            }
            set
            {
                if ((this._ContactID != value))
                {
                    this._ContactID = value;
                }
            }
        }

        [Column(Storage = "_CreatedBy", DbType = "BigInt")]
        public System.Nullable<long> CreatedBy
        {
            get
            {
                return this._CreatedBy;
            }
            set
            {
                if ((this._CreatedBy != value))
                {
                    this._CreatedBy = value;
                }
            }
        }

        [Column(Storage = "_CreatedDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> CreatedDate
        {
            get
            {
                return this._CreatedDate;
            }
            set
            {
                if ((this._CreatedDate != value))
                {
                    this._CreatedDate = value;
                }
            }
        }

        [Column(Storage = "_UpdatedBy", DbType = "BigInt")]
        public System.Nullable<long> UpdatedBy
        {
            get
            {
                return this._UpdatedBy;
            }
            set
            {
                if ((this._UpdatedBy != value))
                {
                    this._UpdatedBy = value;
                }
            }
        }

        [Column(Storage = "_UpdatedDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> UpdatedDate
        {
            get
            {
                return this._UpdatedDate;
            }
            set
            {
                if ((this._UpdatedDate != value))
                {
                    this._UpdatedDate = value;
                }
            }
        }

        [Column(Storage = "_ContactName", DbType = "NVarChar(201)")]
        public string ContactName
        {
            get
            {
                return this._ContactName;
            }
            set
            {
                if ((this._ContactName != value))
                {
                    this._ContactName = value;
                }
            }
        }

        [Column(Storage = "_StartDateNTime", DbType = "VarChar(38)")]
        public string StartDateNTime
        {
            get
            {
                return this._StartDateNTime;
            }
            set
            {
                if ((this._StartDateNTime != value))
                {
                    this._StartDateNTime = value;
                }
            }
        }

        [Column(Storage = "_TicketStatus", DbType = "NVarChar(100)")]
        public string TicketStatus
        {
            get
            {
                return this._TicketStatus;
            }
            set
            {
                if ((this._TicketStatus != value))
                {
                    this._TicketStatus = value;
                }
            }
        }

        [Column(Storage = "_OpenedByName", DbType = "NVarChar(201)")]
        public string OpenedByName
        {
            get
            {
                return this._OpenedByName;
            }
            set
            {
                if ((this._OpenedByName != value))
                {
                    this._OpenedByName = value;
                }
            }
        }

        [Column(Storage = "_UpdateByName", DbType = "NVarChar(201)")]
        public string UpdateByName
        {
            get
            {
                return this._UpdateByName;
            }
            set
            {
                if ((this._UpdateByName != value))
                {
                    this._UpdateByName = value;
                }
            }
        }

        [Column(Storage = "_UpdatedDateNTime", DbType = "VarChar(38)")]
        public string UpdatedDateNTime
        {
            get
            {
                return this._UpdatedDateNTime;
            }
            set
            {
                if ((this._UpdatedDateNTime != value))
                {
                    this._UpdatedDateNTime = value;
                }
            }
        }

        [Column(Storage = "_CompanyName", DbType = "NVarChar(100)")]
        public string CompanyName
        {
            get
            {
                return this._CompanyName;
            }
            set
            {
                if ((this._CompanyName != value))
                {
                    this._CompanyName = value;
                }
            }
        }

        [Column(Storage = "_OwnerName", DbType = "NVarChar(201)")]
        public string OwnerName
        {
            get
            {
                return this._OwnerName;
            }
            set
            {
                if ((this._OwnerName != value))
                {
                    this._OwnerName = value;
                }
            }
        }

        [Column(Storage = "_DatePromisedFormatted", DbType = "VarChar(30)")]
        public string DatePromisedFormatted
        {
            get
            {
                return this._DatePromisedFormatted;
            }
            set
            {
                if ((this._DatePromisedFormatted != value))
                {
                    this._DatePromisedFormatted = value;
                }
            }
        }

        [Column(Storage = "_ContactEmail", DbType = "NVarChar(50)")]
        public string ContactEmail
        {
            get
            {
                return this._ContactEmail;
            }
            set
            {
                if ((this._ContactEmail != value))
                {
                    this._ContactEmail = value;
                }
            }
        }

        [Column(Storage = "_ContactTelephone", DbType = "NVarChar(100)")]
        public string ContactTelephone
        {
            get
            {
                return this._ContactTelephone;
            }
            set
            {
                if ((this._ContactTelephone != value))
                {
                    this._ContactTelephone = value;
                }
            }
        }

        [Column(Storage = "_EndDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> EndDate
        {
            get
            {
                return this._EndDate;
            }
            set
            {
                if ((this._EndDate != value))
                {
                    this._EndDate = value;
                }
            }
        }

        [Column(Storage = "_EndDateNTime", DbType = "VarChar(38)")]
        public string EndDateNTime
        {
            get
            {
                return this._EndDateNTime;
            }
            set
            {
                if ((this._EndDateNTime != value))
                {
                    this._EndDateNTime = value;
                }
            }
        }

        [Column(Storage = "_FirstName", DbType = "NVarChar(100)")]
        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                if ((this._FirstName != value))
                {
                    this._FirstName = value;
                }
            }
        }

        [Column(Storage = "_LastName", DbType = "NVarChar(100)")]
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                if ((this._LastName != value))
                {
                    this._LastName = value;
                }
            }
        }

        [Column(Storage = "_SupplierName", DbType = "VarChar(100)")]
        public string SupplierName
        {
            get
            {
                return this._SupplierName;
            }
            set
            {
                if ((this._SupplierName != value))
                {
                    this._SupplierName = value;
                }
            }
        }

        [Column(Storage = "_SupplierID", DbType = "BigInt")]
        public System.Nullable<long> SupplierID
        {
            get
            {
                return this._SupplierID;
            }
            set
            {
                if ((this._SupplierID != value))
                {
                    this._SupplierID = value;
                }
            }
        }

        [Column(Storage = "_TypeOfRequestID", DbType = "BigInt")]
        public System.Nullable<long> TypeOfRequestID
        {
            get
            {
                return this._TypeOfRequestID;
            }
            set
            {
                if ((this._TypeOfRequestID != value))
                {
                    this._TypeOfRequestID = value;
                }
            }
        }

        [Column(Storage = "_TypeOfRequest", DbType = "NVarChar(100)")]
        public string TypeOfRequest
        {
            get
            {
                return this._TypeOfRequest;
            }
            set
            {
                if ((this._TypeOfRequest != value))
                {
                    this._TypeOfRequest = value;
                }
            }
        }

        [Column(Storage = "_TicketEmail", DbType = "NVarChar(100)")]
        public string TicketEmail
        {
            get
            {
                return this._TicketEmail;
            }
            set
            {
                if ((this._TicketEmail != value))
                {
                    this._TicketEmail = value;
                }
            }
        }

        [Column(Storage = "_SupplierEmail", DbType = "NVarChar(50)")]
        public string SupplierEmail
        {
            get
            {
                return this._SupplierEmail;
            }
            set
            {
                if ((this._SupplierEmail != value))
                {
                    this._SupplierEmail = value;
                }
            }
        }

        [Column(Storage = "_SupplierTelephone", DbType = "NVarChar(100)")]
        public string SupplierTelephone
        {
            get
            {
                return this._SupplierTelephone;
            }
            set
            {
                if ((this._SupplierTelephone != value))
                {
                    this._SupplierTelephone = value;
                }
            }
        }

        [Column(Storage = "_WorkGroupID", DbType = "BigInt")]
        public System.Nullable<long> WorkGroupID
        {
            get
            {
                return this._WorkGroupID;
            }
            set
            {
                if ((this._WorkGroupID != value))
                {
                    this._WorkGroupID = value;
                }
            }
        }

        [Column(Storage = "_WorkGroup", DbType = "NVarChar(100)")]
        public string WorkGroup
        {
            get
            {
                return this._WorkGroup;
            }
            set
            {
                if ((this._WorkGroup != value))
                {
                    this._WorkGroup = value;
                }
            }
        }

        [Column(Storage = "_IsSupplierTicket", DbType = "Int NOT NULL")]
        public int IsSupplierTicket
        {
            get
            {
                return this._IsSupplierTicket;
            }
            set
            {
                if ((this._IsSupplierTicket != value))
                {
                    this._IsSupplierTicket = value;
                }
            }
        }

        [Column(Storage = "_IsFlagged", DbType = "Int NOT NULL")]
        public int IsFlagged
        {
            get
            {
                return this._IsFlagged;
            }
            set
            {
                if ((this._IsFlagged != value))
                {
                    this._IsFlagged = value;
                }
            }
        }

        [Column(Storage = "_BaseStationID", DbType = "BigInt")]
        public System.Nullable<long> BaseStationID
        {
            get
            {
                return this._BaseStationID;
            }
            set
            {
                if ((this._BaseStationID != value))
                {
                    this._BaseStationID = value;
                }
            }
        }

        [Column(Storage = "_BaseStation", DbType = "NVarChar(500)")]
        public string BaseStation
        {
            get
            {
                return this._BaseStation;
            }
            set
            {
                if ((this._BaseStation != value))
                {
                    this._BaseStation = value;
                }
            }
        }
    }

    public partial class PAASOrderLinesResult
    {
        private string _VendorProductID;

        private string _DisplayQtyOrdered;

        private string _DisplayUnit;

        public PAASOrderLinesResult()
        {
        }

        [Column(Storage = "_VendorProductID", DbType = "NVarChar(50)")]
        public string VendorProductID
        {
            get
            {
                return this._VendorProductID;
            }
            set
            {
                if ((this._VendorProductID != value))
                {
                    this._VendorProductID = value;
                }
            }
        }

        [Column(Storage = "_DisplayQtyOrdered", DbType = "NVarChar(50)")]
        public string DisplayQtyOrdered
        {
            get
            {
                return this._DisplayQtyOrdered;
            }
            set
            {
                if ((this._DisplayQtyOrdered != value))
                {
                    this._DisplayQtyOrdered = value;
                }
            }
        }

        [Column(Storage = "_DisplayUnit", DbType = "NVarChar(100)")]
        public string DisplayUnit
        {
            get
            {
                return this._DisplayUnit;
            }
            set
            {
                if ((this._DisplayUnit != value))
                {
                    this._DisplayUnit = value;
                }
            }
        }
    }

    //added by Prashant - 22/11/2012
    public class SelectRecipentsForReplyTo
    {
        public long OrderID;
        public long UserInfoID;
        public string FirstName;
        public string LastName;
        public string Email;
        public long Usertype;
    }

    //added by Prashant - 29/03/2013
    public class GSEUserDetails
    {
        public long UserID;
        public long CurrentUserType;
        public string FirstName;
        public string LastName;
        public string LoginEmail;
        public string BaseStationIds;
        public long? CompanyID;
        public long? VendorID;
        public long VenoderEmployeeID;
        public long MenuPrivilegeID;
        public string AssociateCustomerID;
        public bool IsCustomer;
    }
}