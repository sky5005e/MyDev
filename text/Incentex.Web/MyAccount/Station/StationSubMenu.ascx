<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StationSubMenu.ascx.cs" Inherits="admin_Company_Station_StationSubMenu" %>
<div class="spacer2"></div>
            <div class="header_bg">
					<div class="header_bgr">
						<div class="basic_link">
							<ul class="manage_link">
								<li>
								<asp:HyperLink ID="lnkMainInfo" runat="server" CssClass="current" 
								Text="Main Station Information" ToolTip="Main Station Information"
								NavigateUrl="~/admin/Company/Station/AddMainStationInfo.aspx"
								></asp:HyperLink>
								<%--<a class="current" title="Basic Information" href="#">Main Station Information</a>--%> |</li>
								<li>
								<asp:HyperLink ID="lnkManagerInfo" runat="server" 
								Text="Manager Information" ToolTip="Manager Information"
								NavigateUrl="~/admin/Company/Station/AddManagerInfo.aspx"
								></asp:HyperLink> |</li>
								<li>
								<asp:HyperLink ID="lnkAdminInfo" runat="server"  
								Text="Admin Information" ToolTip="Admin Information"
								NavigateUrl="~/admin/Company/Station/AddAdminInformation.aspx"
								></asp:HyperLink> |</li>
								<li>
								<asp:HyperLink ID="lnkServiceInfo" runat="server"  
								Text="Service" ToolTip="Service"
								NavigateUrl="~/admin/Company/Station/AddService.aspx"
								></asp:HyperLink> |
								</li>
								<li>
								<asp:HyperLink ID="lnkAdditionalInfo" runat="server"  
								Text="Additional Info" ToolTip="Additional Info"
								NavigateUrl="~/admin/Company/Station/AddAdditionalInfo.aspx"
								></asp:HyperLink>
								</li>
							</ul>
						</div>
					</div>
			</div>