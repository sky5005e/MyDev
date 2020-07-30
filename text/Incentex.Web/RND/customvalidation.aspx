<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="customvalidation.aspx.cs" Inherits="RND_customvalidation" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
<asp:Label ID="lblfirstname" runat="server">Firstname</asp:Label>
<asp:TextBox ID="txtFirstname" runat="server">
</asp:TextBox>
<asp:RequiredFieldValidator ID="rdfFirstname" runat="server" ControlToValidate="txtFirstname">
</asp:RequiredFieldValidator>
<asp:Button ID="btnsubmit" runat="server" Text="submit custom" />
</div>
</asp:Content>

