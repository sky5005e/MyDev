<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="DefaultSettings.aspx.cs" Inherits="admin_CompanyStore_DefaultSettings"
    Title="Store Workgroup >> Default Settings" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style type="text/css">
        .basic_link .manage_link a, .header_bg
        {
        	text-align:left;
        }
        .basic_link img
        {
        	margin:0 4px;
        } 
</style>
    <script type="text/javascript" language="javascript">
            $().ready(function() {
                   
               });
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div class="employee_name">
            Work Group:
            <asp:Label ID="lblWorkGroup" runat="server" Text=""></asp:Label>
        </div>
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <h4>
            Default Pricing for MOAS Payment</h4>
        <div class="form_table">
            <table class="dropdown_pad">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%">Select Price Level :</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlPriceLevel" onchange="pageLoad(this,value);" runat="server">
                                        <asp:ListItem Text="-- Select Price --" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="L1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="L2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="L3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="L4" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                                <div id="dvPriceLevel">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkbtnSave" class="gredient_btn" runat="server" OnClick="lnkbtnSave_Click"><span><strong>Save</strong></span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
