<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="SearchPurchaseOrders.aspx.cs" Inherits="admin_PurchaseOrderManagement_SearchPurchaseOrders" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <style type="text/css">
        .input_label
        {
            border-right: 1px solid #1B1B1B;
            color: #F4F4F4 !important;
            display: inline-block;
            font-size: 11px;
            line-height: 34px;
            margin-right: 3px !important;
            padding-right: 10px !important;
            width: 42% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad select_box_pad" style="width: 400px;">
        <div class="form_table">
            <div class="spacer10">
            </div>
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <div class="spacer10">
            </div>
            <table>
                <tr id="trVendor" runat="server">
                    <td class="form_td">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Vendor</span><span class="custom-sel label_sel">
                                    <asp:DropDownList ID="ddlvendor" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);"
                                        OnSelectedIndexChanged="ddlvendor_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trSelectMasterItem" runat="server">
                    <td class="form_td">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Select Master Item</span> <span class="custom-sel label_sel">
                                    <asp:DropDownList ID="ddlMasterItem" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trOrderNumber" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Enter Purchase Order Number</span> <span id="dvL3" runat="server"
                                    style="color: #F4F4F4; font-size: 11px;"></span>
                                <asp:TextBox ID="txtOrderNumber" runat="server"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkBtnSearch" class="gredient_btn" runat="server" ToolTip="Search Purchase Order"
                            OnClick="lnkBtnSearch_Click"><span>Search Now</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
