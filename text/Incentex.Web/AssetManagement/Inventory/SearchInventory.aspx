<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="SearchInventory.aspx.cs" Inherits="AssetManagement_Inventory_SearchInventory"
    Title="Search Inventory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div style="text-align: center">
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
    </div>
    <div class="form_pad">
        <div class="form_table">
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <table class="dropdown_pad ">
                        <tr>
                            <td>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 38%">Company</span>
                                    <label class="dropimg_width">
                                        <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlCompany" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </label>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 38%">Base Station</span>
                                    <label class="dropimg_width">
                                        <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlBaseStation" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </label>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 38%">Product Category</span>
                                    <label class="dropimg_width">
                                        <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlProductCategory" runat="server" AutoPostBack="true" 
                                    onselectedindexchanged="ddlProductCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                        </span>
                                    </label>
                                </div>
                                <div class="form_bot_co" onselectedindexchanged="ddlProductCategory_SelectedIndexChanged">
                                    <span>&nbsp;</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 38%">Product Sub-Category</span>
                                    <label class="dropimg_width">
                                        <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlProductSCategory" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </label>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 38%">Part Number</span>
                                    <asp:TextBox ID="txtPartNumber" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 38%">MFG Part Number</span>
                                    <asp:TextBox ID="txtMFGPartNumber" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="spacer10">
                            </td>
                        </tr>
                       <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSearch" class="grey2_btn" runat="server" 
                            ToolTip="Search Basic Information" onclick="lnkBtnSearch_Click" ><span>Search</span></asp:LinkButton>
                        
                    </td>
                </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
