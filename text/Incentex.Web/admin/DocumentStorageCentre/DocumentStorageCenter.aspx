<%@ Page Title="Document Storage Center" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="DocumentStorageCenter.aspx.cs" Inherits="admin_DocumentStoregeCentre_DocumentStorageCenter" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad" style="padding-left: 180px;">
        <div class="spacer10">
        </div>
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" Style="padding-left: 35px;"></asp:Label>
        <div class="spacer10">
        </div>
        <div id="dvDocument" runat="server">
            <asp:HyperLink ID="lnkAddDocumentStorage" runat="server" title="Add Document Storage" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Incentex-adddocumentstorage.png" /> 
                <span>
                   Add Document Storage
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkSearchDocumentStorage" runat="server" title="Search Document Storage" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Incentex-searchdocumentstorage.png" /> 
                <span>
                   Search Document Storage  
                </span>
            </asp:HyperLink>
        </div>
        <div>
           <asp:HyperLink ID="lnkAddDocumentStorageType" runat="server" title="Add Document Storage Type" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Incentex-adddocumentstorage.png" /> 
                <span>
                   Add Document Storage Type  
                </span>
            </asp:HyperLink>
        </div>
    </div>
</asp:Content>
