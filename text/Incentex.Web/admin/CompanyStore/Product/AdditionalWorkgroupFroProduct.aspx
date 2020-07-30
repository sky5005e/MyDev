<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdditionalWorkgroupFroProduct.aspx.cs" Inherits="admin_CompanyStore_Product_AdditionalWorkgroupFroProduct" Title="Store >> Additional Workgroup" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
     <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="spacer10">
    </div>
    <div class="tabcontent" id="menu_access">

        <div class="alignnone">
            &nbsp;</div>
        <div class="form_pad">
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            
            <div class="form_table">
                <table>
                    <tr>
                        <td class="formtd">
                            <h4>
                                Additional Workgroup</h4>
                            <table class="checktable_supplier true">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dtlWorkgroup" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                                    <asp:CheckBox ID="chkdtlMenus" runat="server" />
                                                </span>
                                                <label>
                                                    <asp:Label ID="lblWorkgroupName" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnWorkgroupID" runat="server" Value='<%# Eval("iLookupID") %>' />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="formtd">
                            <h4>
                                Additional SubCategory</h4>
                            <table class="checktable_supplier true">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dtlSubCategory" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                                    <asp:CheckBox ID="chkSubCategory" runat="server" />
                                                </span>
                                                <label>
                                                    <asp:Label ID="lblSubCategoryName" Text='<%# Eval("SubCategoryName") %>' runat="server"></asp:Label></label>
                                                    <asp:HiddenField ID="hdnSubCategoryID" runat="server" Value='<%# Eval("SubCategoryID") %>' />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            
            <div class="aligncenter">
                <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save Information"
                    OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
