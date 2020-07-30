<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UnAuthorised.aspx.cs" Inherits="admin_UnAuthorised" Title="Incentex - Unauthorised Access" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad building_pad">
        <div class="forgot_box">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="black_middle">
                <div class="clearfix">
                    <h4>
                        <b>Unauthorised...</b></h4>
                    <label style="color: #72757C; font-size: 14px;">
                        You are not authorised to execute this transaction.<br />
                        <br />
                        Contact the sytem administrator.</label>
                </div>
                <div class="alignnone">
                </div>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
    </div>
</asp:Content>
