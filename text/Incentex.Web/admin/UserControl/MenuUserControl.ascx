<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuUserControl.ascx.cs"
    Inherits="admin_UserControl_MenuUserControl" %>
<div class="spacer2">
</div>
<div class="header_bg">
    <div class="header_bgr clearfix">
        <div class="basic_link">
            <asp:Menu ID="MainMenu" runat="server" Orientation="Horizontal">
                <StaticSelectedStyle CssClass="current" />
                <DynamicSelectedStyle CssClass="current" />
            </asp:Menu>
        </div>
    </div>
</div>
<div class="spacer2">
</div>
<div class="header_bg" id="dvMainSecondMenu" runat="server" visible="false">
    <div class="header_bgr clearfix">
        <div class="basic_link">
            <asp:Menu ID="MainSecondMenu" runat="server" Orientation="Horizontal">
                <StaticSelectedStyle CssClass="current" />
                <DynamicSelectedStyle CssClass="current" />
            </asp:Menu>
        </div>
    </div>
</div>
<div class="spacer2">
</div>
<div class="header_bg" id="dvSubMenu" runat="server">
    <div class="header_bgr clearfix">
        <div class="basic_link">
            <asp:Menu CssClass="manage_link" runat="server" OnMenuItemClick="SubMainMenu_MenuItemClick"
                Orientation="Horizontal" ID="SubMainMenu">
                <StaticSelectedStyle CssClass="current" />
                <DynamicSelectedStyle CssClass="current" />
            </asp:Menu>
        </div>
    </div>
</div>
<div class="spacer2">
</div>
<%-- Start Add Subsecondmenu by mayur on 3rd-jan-2012 --%>
<div class="header_bg" id="dvSubSecondMenu" runat="server" visible="false">
    <div class="header_bgr clearfix">
        <div class="basic_link">
            <asp:Menu CssClass="manage_link" runat="server" Orientation="Horizontal" ID="SubSecondMainMenu"
                Width="90%">
                <StaticSelectedStyle CssClass="current" />
                <DynamicSelectedStyle CssClass="current" />
            </asp:Menu>
        </div>
    </div>
</div>
<%-- End Add Subsecondmenu by mayur on 3rd-jan-2012 --%>
