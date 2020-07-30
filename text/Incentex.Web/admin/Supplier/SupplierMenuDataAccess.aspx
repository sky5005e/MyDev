<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="SupplierMenuDataAccess.aspx.cs" Inherits="admin_Supplier_SupplierMenuDataAccess" Title="Supplier Employee Menu Access System" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div class="tabcontent" id="menudata">
    <div class="alignnone">
            &nbsp;</div>
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <h4>
            World-Link System Menu Access</h4>
        <div>
            <table>
                <tr>
                    <td >
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <asp:DataList ID="dtlMenus" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                        >
                                        <ItemTemplate>
                                            <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                                <asp:CheckBox ID="chkdtlMenus" runat="server" />
                                            </span>
                                            <label>
                                                <asp:Label ID="lblMenus" Text='<%# Eval("sDescription") %>' runat="server"></asp:Label></label>
                                            <asp:HiddenField ID="hdnMenuAccess" runat="server" Value='<%# Eval("iMenuPrivilegeID") %>' />
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <h4>
            World-Link System Database Restrictions</h4>
        <div>
            <table>
                <tr>
                    <td style="width: 42%;">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <asp:DataList ID="dtlDataAccess" runat="server" RepeatColumns="2">
                                        <ItemTemplate>
                                            <span class="custom-checkbox alignleft" id="menuspanData" runat="server">
                                                <asp:CheckBox ID="chkdtlDataAccess" runat="server" />
                                            </span>
                                            <label>
                                                <asp:Label ID="lbldtlDataAccess" Text='<%# Eval("sDescription") %>' runat="server"></asp:Label></label>
                                            <asp:HiddenField ID="hdnDataBaseAccess" runat="server" Value='<%# Eval("iDataPrivilegeId") %>' />
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div class="alignnone spacer25">
            </div>
        </div>
        <div class="botbtn centeralign">
            <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" TabIndex="19" OnClick="lnkSave_Click">
								        <span>Save Information</span>
            </asp:LinkButton>
        </div>
    </div>
    </div>
</asp:Content>

