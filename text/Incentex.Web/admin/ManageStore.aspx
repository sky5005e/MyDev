<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ManageStore.aspx.cs" Inherits="admin_ManageStore" Title="Untitled Page" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="manuControl" runat="server" /> 
<table>
<tr>
<td>
<asp:Label  Text="Under Construction" runat="server" BorderColor="Black" BorderStyle="Solid" Font-Size="XX-Large" Font-Bold="True"></asp:Label>
</td>
</tr>
</table>
</asp:Content>


