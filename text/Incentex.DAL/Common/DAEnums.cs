using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incentex.DAL.Common
{
    public class DAEnums
    {

        public enum YesNoType
        {
            Yes = 1,
            No = 0
        }

        #region Lookup Code

        public enum LookupCodeType
        {
            StationAdditionalInfo,
            StationService,
            SupplierClassifications,
            SupplierGeneralQualitySystemCompliances,
            SupplierType,
            EmployeeType,
            DocumentFor,
            Status,
            SupplierDocuments,
            SupplierEmployeeBenefits,
            SupplierEmployeePayPeriods,
            IncentexEmployeeBenefits,
            IncentexEmployeePayPeriods,
            IncentexEmployeeSetupDocument,
            IncentexEmployeeAdditionalDocument,
            EmployeeRankPilotsOnly,
            StyleNo,
            MasterItemNumber,
            RANK,
            ItemSize,
            Workgroup,
            Gender,
            Department,
            Region,
            AdditionalInfo,
            TypeOfRequest,
            InventoryStatus,
            BackOrderManagement,
            ShippingMethod,
            EmployeeTitle
        }


        public static String GetLookupCodeName(LookupCodeType code)
        {
            String Name = "";

            switch (code)
            {
                case LookupCodeType.StationAdditionalInfo:
                    Name = "StationAdditionalInfo";
                    break;
                case LookupCodeType.StationService:
                    Name = "StationService";
                    break;
                case LookupCodeType.SupplierClassifications:
                    Name = "SupplierClassifications";
                    break;
                case LookupCodeType.SupplierGeneralQualitySystemCompliances:
                    Name = "SupplierGeneralQualitySystemCompliances";
                    break;
                case LookupCodeType.SupplierType:
                    Name = "SupplierType";
                    break;
                case LookupCodeType.EmployeeType:
                    Name = "EmployeeType";
                    break;
                case LookupCodeType.DocumentFor:
                    Name = "Company";
                    break;
                case LookupCodeType.Status:
                    Name = "Status";
                    break;
                case LookupCodeType.SupplierDocuments:
                    Name = "SupplierDocuments";
                    break;
                case LookupCodeType.SupplierEmployeeBenefits:
                    Name = "SupplierEmployeeBenefits";
                    break;
                case LookupCodeType.SupplierEmployeePayPeriods:
                    Name = "SupplierEmployeePayPeriods";
                    break;
                case LookupCodeType.IncentexEmployeeBenefits:
                    Name = "IncentexEmployeeBenefits";
                    break;
                case LookupCodeType.IncentexEmployeePayPeriods:
                    Name = "IncentexEmployeePayPeriods";
                    break;
                case LookupCodeType.IncentexEmployeeSetupDocument:
                    Name = "IncentexEmployeeSetupDocument";
                    break;
                case LookupCodeType.IncentexEmployeeAdditionalDocument:
                    Name = "IncentexEmployeeAdditionalDocument";
                    break;
                case LookupCodeType.EmployeeRankPilotsOnly:
                    Name = "EmployeeRankPilotsOnly";
                    break;
                case LookupCodeType.StyleNo:
                    Name = "StyleNo";
                    break;
                case LookupCodeType.MasterItemNumber:
                    Name = "MasterItemNumber";
                    break;
                case LookupCodeType.RANK:
                    Name = "RANK";
                    break;
                case LookupCodeType.ItemSize:
                    Name = "ItemSize";
                    break;
                case LookupCodeType.Workgroup:
                    Name = "Workgroup";
                    break;
                case LookupCodeType.Gender:
                    Name = "Gender";
                    break;
                case LookupCodeType.Department:
                    Name = "Department ";
                    break;
                case LookupCodeType.Region:
                    Name = "Region";
                    break;
                case LookupCodeType.AdditionalInfo:
                    Name = "Additional Information";
                    break;
                case LookupCodeType.ShippingMethod:
                    Name = "Shipping Method";
                    break;
                default:
                    Name = code.ToString();
                    break;
            }

            return Name;
        }

        public enum status
        {
            Active = 135,
            InActive = 136
        }

        #endregion

        #region Document

        public enum DocumentForType
        {
            Supplier,
            IncentexEmployee,
            SupplierEmployee
        }


        #endregion

        public enum SortOrderType
        {
            Asc,
            Desc
        }

        public enum NoteForType
        {
            Station,
            CompanyEmployee,
            Company,
            CompanyAccount,
            SupplierEmployee,
            Add,
            Edit,
            Supplier,
            IncentexEmployee,
            StoreProduct,
            Workgroup,
            CACE,
            OrderProductReturns,
            SupplierOrder,
            OrderDetailsIEs,
            ServiceTicketIEs,
            ServiceTicketCAs,
            ServiceTicketSupps,
            AssetManagement,
            BackOrderedUntil,
            HeadsetRepairCenter,
            HeadsetRepairVendor,
            SupplierPurchaseOrder,
            PurchaseOrderDetailsIEs,
            FlagAssets,
            AssetWarrantyClaim,
            LeaseAsset,
            PurchaseAsset,
            AssetImagesVideos,
            AssetManuals,
            AssetWarranty
        }

        public static String GetNoteForTypeName(NoteForType NoteFor)
        {
            String Name = "";
            switch (NoteFor)
            {
                case NoteForType.Station:
                    Name = "Station";
                    break;
                case NoteForType.CompanyEmployee:
                    Name = "CompanyEmployee";
                    break;
                case NoteForType.CompanyAccount:
                    Name = "CompanyAccount";
                    break;
                case NoteForType.Company:
                    Name = "Company";
                    break;
                case NoteForType.Add:
                    Name = "Add";
                    break;
                case NoteForType.Edit:
                    Name = "Edit";
                    break;
                case NoteForType.Supplier:
                    Name = "Supplier";
                    break;
                case NoteForType.IncentexEmployee:
                    Name = "IncentexEmployee";
                    break;
                case NoteForType.StoreProduct:
                    Name = "StoreProduct";
                    break;
                case NoteForType.Workgroup:
                    Name = "52";
                    break;
                case NoteForType.CACE:
                    Name = "CACE";
                    break;
                case NoteForType.OrderProductReturns:
                    Name = "OrderProductReturns";
                    break;
                case NoteForType.SupplierOrder:
                    Name = "SupplierOrder";
                    break;
                case NoteForType.OrderDetailsIEs:
                    Name = "OrderDetailsIEs";
                    break;
                case NoteForType.ServiceTicketIEs:
                    Name = "ServiceTicketIEs";
                    break;
                case NoteForType.ServiceTicketCAs:
                    Name = "ServiceTicketCAs";
                    break;
                case NoteForType.ServiceTicketSupps:
                    Name = "ServiceTicketSupps";
                    break;
                case NoteForType.AssetManagement:
                    Name = "AssetManagement";
                    break;
                case NoteForType.BackOrderedUntil:
                    Name = "BackOrderedUntil";
                    break;
                case NoteForType.HeadsetRepairCenter:
                    Name = "HeadsetRepairCenter";
                    break;
                case NoteForType.HeadsetRepairVendor:
                    Name = "HeadsetRepairVendor";
                    break;
                case NoteForType.SupplierPurchaseOrder:
                    Name = "SupplierPurchaseOrder";
                    break;
                case NoteForType.PurchaseOrderDetailsIEs:
                    Name = "PurchaseOrderDetailsIEs";
                    break;
                case NoteForType.FlagAssets:
                    Name = "FlagAssets";
                    break;
                case NoteForType.AssetWarrantyClaim:
                    Name = "AssetWarrantyClaim";
                    break;

                case NoteForType.AssetManuals:
                    Name = "AssetManuals";
                    break;
                case NoteForType.AssetWarranty:
                    Name = "AssetWarranty";
                    break;
                case NoteForType.LeaseAsset:
                    Name = "LeaseAsset";
                    break;
                case NoteForType.PurchaseAsset:
                    Name = "PurchaseAsset";
                    break;
                case NoteForType.AssetImagesVideos:
                    Name = "AssetImagesVideos";
                    break;
            }
            return Name;
        }

        public enum UserTypes
        {
            SuperAdmin = 1,
            IncentexAdmin = 2,
            CompanyAdmin = 3,
            CompanyEmployee = 4,
            Supplier = 5,
            SupplierEmployee = 6,
            ThirdPartySupplierEmployee = 7,
            EquipmentVendorEmployee = 8,
            CustomerSuperAdmin = 9,
            CustomerAdmin = 10,
            CustomerEmployee = 11,
            VendorSuperAdmin = 12,
            VendorAdmin = 13,
            VendorEmployee = 14,
            FederalAviationAssociation = 15
        }

        public static String GetUserTypeName(Int64? UserType)
        {
            String UserTypeName = "";
            switch (UserType)
            {
                case 1:
                    UserTypeName = "Super Admin";
                    break;
                case 2:
                    UserTypeName = "Incentex Employee";
                    break;
                case 3:
                    UserTypeName = "Company Admin";
                    break;
                case 4:
                    UserTypeName = "Company Employee";
                    break;
                case 5:
                    UserTypeName = "Supplier";
                    break;
                case 6:
                    UserTypeName = "Supplier Employee";
                    break;
                default:
                    UserTypeName = "Anonymous";
                    break;
            }
            return UserTypeName;
        }

        //Add Here Enum type for the Manageid
        //Add By Nagmani
        public enum ManageID
        {
            ManageCompany = 1,
            ManageSupplier = 2,
            ManageIncentexEmployee = 3,
            SearchUsers = 4,
            CompanyStore = 5,
            CompanyProduct = 6,
            EquipmentVendorEmployee = 10,
            EquipmentVendor = 11,
            EquipmentInventory = 12
        }

        public enum CompanyEmployeeTypes
        {
            Admin = 3,
            Employee = 4
        }

        public enum CompanyStoreDocumentFor
        {
            SplashImage,
            GuideLineManuals,
            News,
            FAQ,
            Contact,
            TNC
        }

        /// <summary>
        ///  Used in Uniform Idduance Program Step - 4 
        /// </summary>
        public enum EmployeeActveRule
        {
            ProgramStop = 1,
            ProgramResume = 2
        }

        public enum OrderStatus
        {
            Received,
            InProcessing
        }

        public static String GetEmployeeActveRuleName(EmployeeActveRule Rule)
        {
            String Name = "";
            switch (Rule)
            {
                case EmployeeActveRule.ProgramResume:
                    Name = "Program Resume";
                    break;
                case EmployeeActveRule.ProgramStop:
                    Name = "Program Stop";
                    break;
            }

            return Name;
        }

        public enum TransactionType
        {
            StartingCredits,
            AnniversaryCredits,
            OrderConfirm,
            OrderReturn,
            OrderCancelFromConfirmation,
            OderCancelFromIncentex,
            OrderItemDeleted,
            OrderInCompletedAmount

        }

        public enum TransactionCode
        {
            STCR,
            ANCR,
            ORCNR,
            ORRTN,
            ORCAN,
            ORCANIN,
            ORITDEL,
            ORINAMT

        }

        #region Uniform Issuance Payment Option

        public enum UniformIssuancePaymentOption
        {
            CompanyPays = 1,
            EmployeePays = 2,
            MOAS = 3
        }

        public static String GetUniformIssuancePaymentOptionName(UniformIssuancePaymentOption Option)
        {
            String Name = "";
            switch (Option)
            {
                case UniformIssuancePaymentOption.CompanyPays:
                    Name = "Company Pays";
                    break;
                case UniformIssuancePaymentOption.EmployeePays:
                    Name = "Employee Pays - PD";
                    break;
                case UniformIssuancePaymentOption.MOAS:
                    Name = "MOAS";
                    break;
            }
            return Name;
        }

        public enum CompanyEmployeeContactInfo
        {
            Billing,
            Shipping
        }

        #endregion

        #region Email Management System
        public enum ManageEmail
        {
            OrderConfirmations = 1,
            OrderNotes = 2,
            ReturnConfirmations = 3,
            ReturnNotes = 4,
            SupportTickets = 5,
            ViewPendingUsers = 6,
            ApprovedUsers = 7,
            MOAS = 8,
            ReturnNotificationsAccountingRelated = 9
        }
        #endregion

        #region Equipment Email Management System
        public enum ManageEquipmentEmail
        {
            InsuranceRecords = 1,
            InspectionRecords = 2,
            MaintenanceRelatedCostes = 3,
            Registration = 4,
            WeeklyMaintenanceRecords = 5,
            EquipmentImage = 6,
            ApprovedInvoice = 7
        }
        #endregion

        #region PriceLevels
        public enum PriceLevelName
        {
            L1,
            L2,
            L3,
            L4,
            CloseOutPrice
        }
        #endregion

        public enum PaymentOptions
        {
            CreditCard = 161,
            CostCenterCode = 162,
            GLCode = 163,
            PurchaseOrder = 164,
            EmployeePayrollDeduct = 165,
            PaidByCorporate = 171,
            MOAS = 198,
            ReplacementUniforms = 212
        }

        public enum OrderFor
        {
            ShoppingCart,
            IssuanceCart
        }

        public static String GetUserTypeFor(Int64? UserType)
        {
            if (UserType > 8 && UserType < 16)
                return "GSEAssetManagement";
            else
                return "";
        }
    }

    #region Return Status for Return management
    public sealed class ProductReturnStatusConsts
    {
        private ProductReturnStatusConsts() { }
        public const String RetAcc = "Return Accepted";
        public const String RetAccRepackFee = "Return Accepted with $2.75 Re-Packaging Fee";
        public const String RetDecHem = "Return Declined - Items Hemmed";
        public const String RetDecVisSig = "Return Declined - Visible Signs of Wear";
        public const String RetAccManDef = "Return Accepted - Manufactures Defect";
    }
    #endregion
}