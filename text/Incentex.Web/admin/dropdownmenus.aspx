<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="dropdownmenus.aspx.cs" Inherits="admin_dropdownmenus" Title="World-Link System - DropdownMenu"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div class="btn_width worldlink_btn">
            <div>
                <asp:LinkButton ID="MasterItemNumber" runat="server" class="gredient_btn" Text="MasterItemNumber"
                    OnClick="MasterItemNumber_Click"><span><strong>Master Item Number</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ItemSize" runat="server" class="gredient_btn" Text="Item Size"
                    OnClick="ItemSize_Click"><span><strong>Item Size</strong></span></asp:LinkButton>
                <asp:LinkButton ID="Department" runat="server" Text="Department" class="gredient_btn pad_none"
                    OnClick="Department_Click"><span><strong>Department</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ProductionStatus" runat="server" class="gredient_btn" OnClick="ProductionStatus_Click"><span><strong>Production Status</strong></span></asp:LinkButton>
                <asp:LinkButton ID="EmployeeRankPilotsOnly" runat="server" class="gredient_btn" OnClick="EmployeeRankPilotsOnly_Click"><span><strong>Employee Rank (Pilots Only)</strong></span></asp:LinkButton>
                <asp:LinkButton ID="FirstReminder" runat="server" class="gredient_btn pad_none" Text="1st Reminder"
                    OnClick="FirstReminder_Click"><span><strong>1st Reminder</strong></span></asp:LinkButton>
                <asp:LinkButton ID="GeneralStatus" runat="server" class="gredient_btn" Text="General Status"
                    OnClick="GeneralStatus_Click"><span><strong>General Status</strong></span></asp:LinkButton>
                <asp:LinkButton ID="EmploymentStatus" runat="server" class="gredient_btn" Text="Employment Status"
                    OnClick="EmploymentStatus_Click"><span><strong>Employment Status</strong></span></asp:LinkButton>
                <asp:LinkButton ID="SecondReminders" runat="server" class="gredient_btn pad_none"
                    Text="2nd Reminders" OnClick="SecondReminders_Click"><span><strong>2nd Reminder</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ProofStatus" runat="server" class="gredient_btn" Text="Proof Status"
                    OnClick="ProofStatus_Click"><span><strong>Proof Status</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ShippingMethod" runat="server" class="gredient_btn" Text="Shipping Method"
                    OnClick="ShippingMethod_Click"><span><strong>Shipping Method</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ThirdReminder" runat="server" class="gredient_btn pad_none" Text="3rd Reminder"
                    OnClick="ThirdReminder_Click"><span><strong>3rd Reminder</strong></span></asp:LinkButton>
                <asp:LinkButton ID="DecoratingMethod" runat="server" class="gredient_btn" Text="Decorating Method"
                    OnClick="DecoratingMethod_Click"><span><strong>Decorating Method</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ItemsToBePolybagged" runat="server" class="gredient_btn" Text="Items to be poly bagged"
                    OnClick="ItemsToBePolybagged_Click"><span><strong>Items to be poly bagged</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ExpirationByMonths" runat="server" class="gredient_btn pad_none"
                    Text="Expiration by Months" OnClick="ExpirationByMonths_Click"><span><strong>Expiration by Months</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ManagingShipmentBy" runat="server" class="gredient_btn" Text="Managing Shipment by"
                    OnClick="ManagingShipmentBy_Click"><span><strong>Managing Shipment by</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ItemsToHaveSizeStickers" runat="server" class="gredient_btn"
                    Text="Items to have size stickers" OnClick="ItemsToHaveSizeStickers_Click"><span><strong>Items to have size stickers</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ExpirationByDate" runat="server" class="gredient_btn pad_none"
                    Text="Expiration by Date" OnClick="ExpirationByDate_Click"><span><strong>Expiration by Date</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ConsolidatedShipment" runat="server" class="gredient_btn" Text="Consolidated Shipment"
                    OnClick="ConsolidatedShipment_Click"><span><strong>Consolidated Shipment</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ItemToBePackagedUsingCardboardInsert" runat="server" class="gredient_btn"
                    Text="Item to be packaged using cardboard insert" OnClick="ItemToBePackagedUsingCardboardInsert_Click"><span><strong>Item to be packaged using cardboard insert</strong></span></asp:LinkButton>
                <asp:LinkButton ID="AssociateWithFileCategory" runat="server" class="gredient_btn pad_none"
                    Text="Associate with File Category" OnClick="AssociateWithFileCategory_Click"><span><strong>Associate with File Category</strong></span></asp:LinkButton>
                <asp:LinkButton ID="Workgroup" runat="server" class="gredient_btn" OnClick="Workgroup_Click"><span><strong>Workgroup</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ItemToBePackagedUsingPlasticClips" runat="server" class="gredient_btn"
                    Text="Item to be packaged using plastic clips" OnClick="ItemToBePackagedUsingPlasticClips_Click"><span><strong>Item to be packaged using plastic clips</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ProductCategory" runat="server" class="gredient_btn pad_none"
                    Text="Product Category" OnClick="ProductCategory_Click"><span><strong>Product Category</strong></span></asp:LinkButton>
                <asp:LinkButton ID="BaseStation" runat="server" class="gredient_btn" Text="Base Stattion"
                    OnClick="BaseStation_Click"><span><strong>Based Station</strong></span></asp:LinkButton>
                <asp:LinkButton ID="Gender" runat="server" class="gredient_btn" Text="Gender" OnClick="Gender_Click"><span><strong>Gender</strong></span></asp:LinkButton>
                <asp:LinkButton ID="Rank" runat="server" class="gredient_btn pad_none" Text="Rank"
                    OnClick="Rank_Click"><span><strong>Rank</strong></span></asp:LinkButton>
                <asp:LinkButton ID="Priority" runat="server" class="gredient_btn" OnClick="Priority_Click"> <span><strong>Priority</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ItemColor" runat="server" class="gredient_btn" Text="ItemColor"
                    OnClick="ItemColor_Click"><span><strong>Item Color</strong></span></asp:LinkButton>
                <asp:LinkButton ID="NumberOfMonths" runat="server" class="gredient_btn pad_none"
                    OnClick="NumberOfMonths_Click"><span><strong>Number Of Months</strong></span></asp:LinkButton>
                <asp:LinkButton ID="SoldBy" runat="server" class="gredient_btn" Text="SoldBy" OnClick="SoldBy_Click"><span><strong>Sold By</strong></span></asp:LinkButton>
                <asp:LinkButton ID="StyleNo" runat="server" class="gredient_btn" Text="StyleNo" OnClick="StyleNo_Click"><span><strong>Style No</strong></span></asp:LinkButton>
                <asp:LinkButton ID="GarmentType" runat="server" class="gredient_btn pad_none" Text="Garment Type"
                    OnClick="GarmentType_Click"><span><strong>Garment Type</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkIssuancePolicy" runat="server" class="gredient_btn" Text="Association for Issuance policy"
                    OnClick="lnkIssuancePolicy_Click"><span><strong>Association for Issuance policy</strong></span></asp:LinkButton>
                <asp:LinkButton ID="Region" runat="server" class="gredient_btn" Text="Region" OnClick="lnkRegion_Click"><span><strong>Region</strong></span></asp:LinkButton>
                <asp:LinkButton ID="EmployeeTitle" runat="server" class="gredient_btn pad_none" Text="Employee Title"
                    OnClick="EmployeeTitle_Click"><span><strong>Employee Title</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkCity" runat="server" class="gredient_btn" Text="Add City"
                    OnClick="lnkCity_Click"><span><strong>City</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkFileCategory" runat="server" class="gredient_btn" Text="File Category"
                    OnClick="lnkFileCategory_Click"><span><strong>File Category</strong></span></asp:LinkButton>
                <asp:LinkButton ID="EmployeeType" runat="server" class="gredient_btn pad_none" Text="Employee Type"
                    OnClick="EmployeeType_Click"><span><strong>Employee Type</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkState" runat="server" class="gredient_btn" Text="Add State"
                    OnClick="lnkState_Click"><span><strong>State</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ServiceSupportIssue" runat="server" class="gredient_btn" Text="Support Ticket Issue"
                    OnClick="ServiceSupportIssue_Click"><span><strong>Support Ticket Issue</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ServiceTicketStatus" runat="server" class="gredient_btn pad_none"
                    Text="Support Ticket Status" OnClick="ServiceTicketStatus_Click"><span><strong>Support Ticket Status</strong></span></asp:LinkButton>
                <asp:LinkButton ID="TypeOfRequest" runat="server" class="gredient_btn" Text="Type Of Request"
                    OnClick="TypeOfRequest_Click"><span><strong>Type Of Request</strong></span></asp:LinkButton>
                <asp:LinkButton ID="EquipmentType" runat="server" class="gredient_btn" Text="Equipment Type"
                    OnClick="EquipmentType_Click">
                     <span><strong>Equipment Type</strong></span></asp:LinkButton>
                <asp:LinkButton ID="Brand" runat="server" class="gredient_btn  pad_none" Text="Brand"
                    OnClick="Brand_Click">
                     <span><strong>Brand</strong></span></asp:LinkButton>
                <asp:LinkButton ID="FuelType" runat="server" class="gredient_btn" Text="Fuel Type"
                    OnClick="FuelType_Click">
                     <span><strong>Fuel Type</strong></span></asp:LinkButton>
                <asp:LinkButton ID="PurchasedFrom" runat="server" class="gredient_btn" Text="Purchased From"
                    OnClick="PurchasedFrom_Click">
                     <span><strong>Purchased From</strong></span></asp:LinkButton>
                <asp:LinkButton ID="GSEDepartment" runat="server" class="gredient_btn pad_none" Text="GSE Department"
                    OnClick="GSEDepartment_Click">
                     <span><strong>GSE Department</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ChargeTo" runat="server" class="gredient_btn" Text="Charge To"
                    OnClick="ChargeTo_Click">
                     <span><strong>Charge To</strong></span></asp:LinkButton>
                <asp:LinkButton ID="EquipmentStatus" runat="server" class="gredient_btn" Text="Equipment Status"
                    OnClick="EquipmentStatus_Click">
                     <span><strong>Equipment Status</strong></span></asp:LinkButton>
                <%--<asp:LinkButton ID="lnkLookupEntry" runat="server" class="gredient_btn pad_none" Text="Lookup Management"
                    OnClick="lnkLookupEntry_Click">
                     <span><strong>Lookup Management</strong></span></asp:LinkButton>--%>
                <asp:LinkButton ID="Supplies" runat="server" class="gredient_btn pad_none" Text="Supplies"
                    OnClick="Supplies_Click">
                     <span><strong>Supplies</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkCategory" runat="server" class="gredient_btn" PostBackUrl="~/admin/vLookupCategory.aspx"
                    Text="Category">
                     <span><strong>Category</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkSubCategory" runat="server" class="gredient_btn" Text="SubCategory"
                    PostBackUrl="~/admin/vLookupSubCategory.aspx">
                     <span><strong>SubCategory</strong></span></asp:LinkButton>
                <asp:LinkButton ID="Material" runat="server" class="gredient_btn pad_none" Text="Material"
                    OnClick="Material_Click">
                     <span><strong>Material</strong></span></asp:LinkButton>
                <asp:LinkButton ID="OrderFor" runat="server" class="gredient_btn" Text="Order For"
                    OnClick="OrderFor_Click">
                     <span><strong>Order For</strong></span></asp:LinkButton>
                <asp:LinkButton ID="SupportTicketReason" runat="server" class="gredient_btn" Text="Support Ticket Reason"
                    OnClick="lnkSupportTicketReason_Click"><span><strong>Support Ticket Reason</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ReasonForReplacement" runat="server" class="gredient_btn pad_none"
                    Text="Reason For Replacement" OnClick="ReasonForReplacement_Click"><span><strong>Reason For Replacement</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ReportTag" runat="server" class="gredient_btn" Text="Report Tag"
                    OnClick="ReportTag_Click">
                     <span><strong>Report Tag</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ItemType" runat="server" class="gredient_btn" Text="Item Type"
                    OnClick="ItemType_Click">
                     <span><strong>Item Type</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkAdditionalInfo" runat="server" class="gredient_btn pad_none"
                    Text="Additional Information" ToolTip="Additional Information" OnClick="lnkAdditionalInfo_Click"><span><strong>Additional Information</strong></span></asp:LinkButton>
                <asp:LinkButton ID="HeadsetBrand" runat="server" class="gredient_btn" Text="Headset Brand"
                    OnClick="lnkHeadsetBrand_Click"><span><strong>Headset Brand</strong></span></asp:LinkButton>
                <asp:LinkButton ID="HeadsetStatus" runat="server" class="gredient_btn" Text="Headset Repair Status"
                    OnClick="lnkHeadsetStatus_Click"><span><strong>Headset Repair Status</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkReasonForReturn" runat="server" class="gredient_btn pad_none"
                    ToolTip="ReasonCode" Text="Reason For Return" OnClick="lnkReasonForReturn_Click"><span><strong>Reason For Return</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkBusinessType" runat="server" class="gredient_btn" ToolTip="BusinessType"
                    Text="BusinessType" OnClick="lnkBusinessType_Click"><span><strong>Business Type</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkOrderfor" runat="server" class="gredient_btn" ToolTip="Purchase Order for"
                    Text="Purchase Order for" OnClick="lnkOrderfor_Click"><span><strong>Purchase Order for</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkOverseasVendor" runat="server" class="gredient_btn pad_none"
                    ToolTip="Overseas Vendor" Text="Purchase Overseas Vendor" OnClick="lnkOverseasVendor_Click"><span><strong>Overseas Vendor</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkOrderSentby" runat="server" class="gredient_btn" ToolTip="Purchase Order Sent by"
                    Text="Purchase Order Sent by" OnClick="lnkOrderSentby_Click"><span><strong>Purchase Order Sent by</strong></span></asp:LinkButton>
                <asp:LinkButton ID="lnkOrderStatus" runat="server" class="gredient_btn" ToolTip="Purchase Order Status"
                    Text="Purchase Order Status" OnClick="lnkOrderStatus_Click"><span><strong>Purchase Order Status</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ArtworkCreatedBy" runat="server" class="gredient_btn pad_none"
                    ToolTip="Artwork Created By User" Text="Artwork Created By User" OnClick="ArtworkCreatedBy_Click"><span><strong>Artwork Created By</strong></span></asp:LinkButton>
                <asp:LinkButton ID="GarmentSizeApply" runat="server" class="gredient_btn" ToolTip="Garment Size Apply for Artwork"
                    Text="Garment Size Apply for Artwork" OnClick="GarmentSizeApply_Click">
                     <span><strong>Garment Size Apply</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ProductType" runat="server" class="gredient_btn" ToolTip="Product Type for Artwork"
                    Text="Product Type for Artwork" OnClick="ProductType_Click">
                     <span><strong>Product Type</strong></span></asp:LinkButton>
                <%-- <asp:LinkButton ID="DecoratingMethod" runat="server" class="gredient_btn" Text="Decorating Method"
                    OnClick="DecoratingMethod_Click">
                     <span><strong>Decorating Method for artwork</strong></span></asp:LinkButton>--%>
                <asp:LinkButton ID="Decorator" runat="server" class="gredient_btn pad_none" ToolTip="Decorator for Artwork"
                    Text="Decorator for Artwork" OnClick="Decorator_Click">
                     <span><strong>Decorator for artwork</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ImprintLocations" runat="server" class="gredient_btn" ToolTip="Imprint Locations Artwork"
                    Text="Imprint Locations Artwork" OnClick="ImprintLocations_Click">
                     <span><strong>Imprint Locations artwork</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ProductGLCode" runat="server" class="gredient_btn" ToolTip="Product GL-Codes"
                    Text="Product GL-Codes" OnClick="ProductGLCode_Click">
                     <span><strong>Product GL-Codes</strong></span></asp:LinkButton>
                <asp:LinkButton ID="ClimateSetting" runat="server" class="gredient_btn pad_none"
                    ToolTip="Climate Setting" Text="Climate Setting" OnClick="ClimateSetting_Click">
                     <span><strong>Climate Setting</strong></span></asp:LinkButton>
                <asp:LinkButton ID="BackOrderManagement" runat="server" class="gredient_btn pad_none"
                    ToolTip=" Back Order Management" Text=" Back Order Management" OnClick="BackOrderManagement_Click">
                     <span><strong>Back Order Management</strong></span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
