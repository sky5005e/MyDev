<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="GenerateSalesOrdersCSV.aspx.cs" Inherits="admin_Supplier_GenerateSalesOrdersCSV"
    Title="World-Link System - Generate Sales Orders (CSV)" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsgList" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div>
            <div style="float: left; padding-left: 125px;">
                <asp:LinkButton ID="lnkDownloadCSV" runat="server" class="gredient_btn1" ToolTip="Download Sales Orders (CSV)"
                    OnClick="lnkDownloadCSV_Click">
                    <span>
                        <img src="../Incentex_Used_Icons/sales_order_btn.png" alt="DSO" />Download Sales Orders (CSV)</span></asp:LinkButton>
            </div>
            <div style="float: right; padding-right: 125px;">
                <asp:LinkButton ID="lnkPastDownloads" runat="server" class="gredient_btn1" ToolTip="View Past Downloads"
                    OnClick="lnkPastDownloads_Click">
                                                        <span><img src="../Incentex_Used_Icons/past_download_btn.png" alt="VPD" />View Past Downloads</span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
