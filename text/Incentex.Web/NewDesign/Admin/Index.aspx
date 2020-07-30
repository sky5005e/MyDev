<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Admin_Index" 
Title="incentex | Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section id="container" class="asset-grid">
 
  <ul class="admin-linkslist cf">
		<li class="asset-link" data-content="asset-link-data"><a href="javascript: void(0);" title="Asset Management"><span>Asset Management</span><strong>Asset Management</strong></a></li>
		<li class="user-link" data-content="user-link-data"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/UserManagement.aspx" %>" title="User Management"><span>User Management</span><strong>User Management</strong></a></li>
		<li class="store-link" data-content="store-link-data"><a href="javascript: void(0);" title="Store Management"><span>Store Management</span><strong>Store Management</strong></a></li>
		<li class="order-link" data-content="order-link-data"><a href="javascript: void(0);" title="Order Management"><span>Order Management</span><strong>Order Management</strong></a></li>
		<li class="myorder-link last" data-content="myorder-link-data"><a href="javascript: void(0);" title="My Orders"><span>My Orders</span><strong>My Orders</strong></a></li>
	</ul>
	<div class="asset-inner asset-link-data">
		<ul class="admin-linkslist cf">
			<li class="mypeninvoices-ico"><em class="top-arrow"></em><a href="javascript: void(0);"  title="My Pending Invoices"><span>My Pending Invoices</span><strong>My Pending Invoices</strong></a></li>
			<li class="assets-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/Assets.aspx" %>" title="Assets"><span>Assets</span><strong>Assets</strong></a></li>
			<li class="reporting-ico"><a href="javascript: void(0);"  title="Reporting"><span>Reporting</span><strong>Reporting</strong></a></li>
			<li class="servicerepair-ico"><a href="javascript: void(0);"  title="Service Repair"><span>Service Repair</span><strong>Service Repair</strong></a></li>
			<li class="accounting-ico"><a href="javascript: void(0);"  title="Accounting"><span>Accounting</span><strong>Accounting</strong></a></li>
			<li class="inventory-ico"><a href="javascript: void(0);"  title="Inventory"><span>Inventory</span><strong>Inventory</strong></a></li>
			<li class="productordering-ico"><a href="javascript: void(0);"  title="Product Ordering"><span>Product Ordering</span><strong>Product Ordering</strong></a></li>
		</ul>
	</div>	
	<div class="asset-inner store-link-data">
		<ul class="admin-linkslist cf">
			<li class="basicinfo-ico"><a href="javascript: void(0);"  title="Basic Information"><span>Basic Information</span><strong>Basic Information</strong></a></li>
			<li class="issuance-ico"><a  href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/IssuancePolicy/IssuanceProgram.aspx" %>"  title="Issuance Management"><span>Issuance Management</span><strong>Issuance Management</strong></a></li>
			<li class="shippolicies-ico"><em class="top-arrow"></em><a href="javascript: void(0);"  title="Shipping Policies"><span>Shipping Policies</span><strong>Shipping Policies</strong></a></li>
			<li class="programdoc-ico"><a href="javascript: void(0);"  title="Program Documents"><span>Program Documents</span><strong>Program Documents</strong></a></li>
			<li class="stations-ico"><a href="javascript: void(0);"  title="Stations"><span>Stations</span><strong>Stations</strong></a></li>
			<li class="instore-ico"><a href="javascript: void(0);"  title="In-Store Marketing"><span>In-Store Marketing</span><strong>In-Store Marketing</strong></a></li>
			<li class="storecredits-ico"><a href="javascript: void(0);"  title="Store Credits"><span>Store Credits</span><strong>Store Credits</strong></a></li>
			<li class="productmgmt-ico"><a href="javascript: void(0);"  title="Product Management"><span>Product Management</span><strong>Product Management</strong></a></li>
			<%--<li class="shipreporting-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["siteurl"] + "UserPages/AdminReports.aspx" %>"  title="Reporting"><span>Reporting</span><strong>Reporting</strong></a></li>--%>
			<li class="msgcenter-ico"><a href="javascript: void(0);"  title="Message Center"><span>Message Center</span><strong>Message Center</strong></a></li>
			<li class="workgroupmgmt-ico"><a href="javascript: void(0);"  title="In-Store Marketing"><span>In-Store Marketing</span><strong>In-Store Marketing</strong></a></li>
			<li class="summary-ico"><a href="javascript: void(0);"  title="Store Credits"><span>Store Credits</span><strong>Store Credits</strong></a></li>
		</ul>
	</div>
	<ul class="admin-linkslist cf">
		<li class="field-link"><a href="javascript: void(0);"  title="Field &amp; Form Management"><span>Field &amp; Form Management</span><strong>Field &amp; Form <br>
			Management</strong></a></li>
		<li class="media-link" data-content="media-link-data"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "admin/DocumentStorageCenter.aspx" %>" title="Document &amp; Media Storage"><span>Document &amp; Media Storage</span><strong>Document &amp; Media <br>
			Storage</strong></a></li>
		<li class="market-link" data-content="market-link-data"><a href="javascript: void(0);"  title="Marketing Services"><span>Marketing Services</span><strong>Marketing Services</strong></a></li>
		<li class="ustore-link"><a href="javascript: void(0);"  title="User Storefront"><span>User Storefront</span><strong>User Storefront</strong></a></li>
		<li class="pro-link last"><a href="javascript: void(0);"  title="Product Search"><span>Product Search</span><strong>Product Search</strong></a></li>
	</ul>
	<div class="asset-inner media-link-data">
		<ul class="admin-linkslist cf">
			<li class="screenprinting-ico"><a href="javascript: void(0);"  title="Screen Printing"><span>Screen Printing</span><strong>Screen Printing</strong></a></li>
			<li class="embroidery-ico"><em class="top-arrow"></em><a href="javascript: void(0);"  title="Embroidery"><span>Embroidery</span><strong>Embroidery</strong></a></li>
			<li class="vmgmt-videos-ico"><a href="javascript: void(0);"  title="Videos"><span>Videos</span><strong>Videos</strong></a></li>
			<li class="corporate-ico"><a href="javascript: void(0);"  title="Corporate"><span>Corporate</span><strong>Corporate</strong></a></li>
			<li class="customers-ico"><a href="javascript: void(0);"  title="Customers"><span>Customers</span><strong>Customers</strong></a></li>
			<li class="vendors-ico"><a href="javascript: void(0);"  title="Vendors"><span>Vendors</span><strong>Vendors</strong></a></li>			
			<li class="prodomages-ico"><a href="javascript: void(0);"  title="Product Images"><span>Product Images</span><strong>Product Images</strong></a></li>
		</ul>
	</div>
	<div class="asset-inner market-link-data">
		<ul class="admin-linkslist cf">
		    <li class="assets-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/MarketingServices/Page404Errors.aspx" %>" title="404 Errors"><span>404 Errors</span><strong>404 Errors</strong></a></li>			
    		<li class="reporting-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/MarketingServices/AbandonedCarts.aspx" %>"  title="Abandoned Carts"><span>Abandoned Carts</span><strong>Abandoned Carts</strong></a></li>
    		<li class="assets-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/MarketingServices/CampaignTracking.aspx" %>" title="Campaign Tracking"><span>Campaign Tracking</span><strong>Campaign Tracking</strong></a></li>
    		<li class="productordering-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/MarketingServices/EmailMarketing.aspx" %>"  title="Email Marketing"><span>Email Marketing</span><strong>Email Marketing</strong></a></li>			
    		<li class="servicerepair-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/MarketingServices/IssuanceActivated.aspx" %>"  title="Issuance Activated"><span>Issuance Activated</span><strong>Issuance Activated</strong></a></li>
    		<li class="accounting-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/MarketingServices/IssuanceExpired.aspx" %>"  title="Issuance Expired"><span>Issuance Expired</span><strong>Issuance Expired</strong></a></li>
    		<li class="inventory-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/MarketingServices/LoadTime.aspx" %>"  title="Load Time"><span>Load Time</span><strong>Load Time</strong></a></li>
    		<li class="reporting-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/MarketingServices/PendingOrders.aspx" %>" title="Pending Orders"><span>Pending Orders</span><strong>Pending Orders</strong></a></li>
    		<li class="servicerepair-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/MarketingServices/PendingUsers.aspx" %>"  title="Pending Users"><span>Pending Users</span><strong>Pending Users</strong></a></li>
    		<li class="productordering-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/MarketingServices/SessionRecordings.aspx" %>"  title="Session Recordings"><span>Session Recordings</span><strong>Session Recordings</strong></a></li>
    		<li class="mypeninvoices-ico"><em class="top-arrow"></em><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "Admin/MarketingServices/SystemAccess.aspx" %>"  title="System Acess"><span>System Access</span><strong>System Access</strong></a></li>
		</ul>
	</div>
	<ul class="admin-linkslist cf">
	    <li class="shipreporting-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "UserPages/AdminReports.aspx" %>"  title="Reporting"><span>Reporting</span><strong>Reporting</strong></a></li>
	    <li class="shipreporting-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "MyAccount/OrderManagement/PendingOrders.aspx"%>"  title="Pending Orders"><span>Reporting</span><strong>Pending Orders</strong></a></li>
	    <li class="pendingusermgmt-ico"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "MyAccount/ViewPendingUsers.aspx"%>"  title="Pending User"><span>Reporting</span><strong>Pending Users</strong></a></li>	    
	    <li class="myorder-link last" data-content="myorder-link-data"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"] + "admin/medialocation.aspx"%>" title="My Orders"><span>My Orders</span><strong>Media Location Management</strong></a></li>
	</ul>
	
</section>
</asp:Content>

