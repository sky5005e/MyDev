<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PaymentFail.aspx.cs" Inherits="Payment_paymentfail" Title="Payment Failed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad building_pad">
        <div class="forgot_box">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="black_middle">
                <div class="clearfix">
                    <asp:Label ID="lblMessage" runat="server" Font-Size="16px" ForeColor="Red"></asp:Label>
                </div>
                <div class="alignnone">
                </div>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
    </div>
</asp:Content>
