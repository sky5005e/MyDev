<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="Menu.aspx.cs" Inherits="admin_Home2" Title="World-Link System" %>
<%@ Register Src="~/admin/UserControl/ScheduledEvent.ascx" TagName="ScheduledUserControl"
    TagPrefix="se" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad" style="padding-left: 200px">
        <se:ScheduledUserControl runat="server" ID="ucScheduledEvent" />
        <div class="btn_width_small">
            <asp:HyperLink ID="lnkWLSC" class="gredient_btnMainPage" title="World-Link System Control"
                runat="server">
                <img src="Incentex_Used_Icons/World-Link System Controls.png" alt="World-Link System Control" />
                <span>
               
                    World-Link System Control
                
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkSupplierPartner" class="gredient_btnMainPage" title="Supplier Partners"
                runat="server">
                <img src="Incentex_Used_Icons/Supplier Partners.png"  alt="Supplier Partners" />
                <span>
                   Supplier Partners
                </span>
            </asp:HyperLink>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
