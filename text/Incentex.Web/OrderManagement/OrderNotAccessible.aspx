<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderNotAccessible.aspx.cs" Inherits="OrderManagement_OrderNotAccessible"
     %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="black_round_box">
        <div class="black2_round_top">
            <span></span>
        </div>
        <div class="black2_round_middle">
            <div class="form_pad">
                <div class="pro_search_pad" style="width: 900px;">
                    <h1>
                         Incentex Admin ( User : <asp:Label ID="lblUserName" runat="server"></asp:Label> ) is viewing this order.
                         <br />
                         <br />
                         Please check back after sometime..
                    </h1>
                </div>
            </div>
        </div>
        <div class="black2_round_bottom">
            <span></span>
        </div>
        <div class="alignnone">
            &nbsp;</div>
    </div>
</asp:Content>
