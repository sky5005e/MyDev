<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MenuAccess.aspx.cs" Inherits="admin_Company_Employee_MenuAccess" Title="Company Employee >> Menu Access" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div class="spacer10">
    </div>
    <div class="tabcontent" id="menu_access">
   <%--     <div class="header_bg">
            <div class="header_bgr">
                <span class="title alignleft">Menu Access</span> <span class="date alignright"></span>
                <div class="alignnone">
                    &nbsp;</div>
            </div>
        </div>--%>
        <div class="alignnone">
            &nbsp;</div>
        <div class="form_pad">
            <div class="employee_name">
                User Name: <asp:Label ID="lblUserFullName" runat="server" Text=""></asp:Label>
            </div>
              <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div class="form_table clearfix">
                <div class="formtd alignleft">
                    <h4>
                        Main Menu Options</h4>
                    <table class="checktable_supplier true">
                        <tr>
                            <td>
                                <asp:DataList ID="dtlMenus" runat="server">
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
                </div>
                <div class="formtd alignleft">
                    <h4>
                        Preferences</h4>
                    <table class="checktable_supplier true">
                        <tr>
                            <td>
                                <asp:DataList ID="dtlPreferences" runat="server">
                                    <ItemTemplate>
                                        <span class="custom-checkbox alignleft" id="spanPreferences" runat="server">
                                            <asp:CheckBox ID="chkPreferences" runat="server" />
                                        </span>
                                        <label>
                                            <asp:Label ID="lblPreferences" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                        <asp:HiddenField ID="hdnPreferences" runat="server" Value='<%# Eval("iLookupID") %>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="divider">
            </div>
            <div class="form_table clearfix">
                <div class="formtd alignleft">
                    <h4>
                        Employee Uniforms</h4>
                    <table class="checktable_supplier true">
                        <tr>
                            <td>
                                <asp:DataList ID="dtEmpUniform" runat="server">
                                    <ItemTemplate>
                                        <span class="custom-checkbox alignleft" id="menuemployeeuni" runat="server">
                                            <asp:CheckBox ID="chkEmpUniform" runat="server" />
                                        </span>
                                        <label>
                                            <asp:Label ID="lblUniform" Text='<%# Eval("SubCategoryName") %>' runat="server"></asp:Label></label>
                                        <asp:HiddenField ID="hdnEmpUni" runat="server" Value='<%#Eval("SubCategoryID")%>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="formtd alignleft">
                    <h4>
                        Additional Information</h4>
                    <table class="checktable_supplier true">
                        <tr>
                            <td>
                                <asp:DataList ID="dtAddiInfo" runat="server">
                                    <ItemTemplate>
                                        <span class="custom-checkbox alignleft" id="addinfospan" runat="server">
                                            <asp:CheckBox ID="chkAddiInfo" runat="server" />
                                        </span>
                                        <label>
                                            <asp:Label ID="lblAddiInfo" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                        <asp:HiddenField ID="hdnAddiInfo" runat="server" Value='<%#Eval("iLookupID")%>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                    </table>
                    <div class="spacer25">
                        &nbsp;</div>
                    <h4>
                        Company Store</h4>
                    <table class="checktable_supplier true">
                        <tr>
                            <td>
                                <asp:DataList ID="dtCompanyStore" runat="server" RepeatColumns="2">
                                    <ItemTemplate>
                                        <span class="custom-checkbox alignleft" id="companystorespan" runat="server">
                                        <asp:CheckBox ID="chkCompanyStore" runat="server" /></span>
                                        <label>
                                            <asp:Label ID="lblCompanyStore" Text='<%# Eval("SubCategoryName") %>' runat="server"></asp:Label></label>
                                        <asp:HiddenField ID="hdnCompanyStore" runat="server" Value='<%#Eval("SubCategoryID")%>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="divider">
            </div>
            <h4>
                Uniform Purchasing</h4>
            <div class="form_table">
                <table>
                    <tr>
                        <td class="formtd">
                            <table class="checktable_supplier true">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dtUniPurchasing" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="uniformspan" runat="server">
                                                <asp:CheckBox ID="chkUniPurchasing" runat="server" /></span>
                                                <label>
                                                    <asp:Label ID="lblUniPurchasing" Text='<%# Eval("SubCategoryName") %>' runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnUniformPurchasing" runat="server" Value='<%#Eval("SubCategoryID")%>' />
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
                Supplies/Equipment </h4>
            <div class="form_table">
                <table>
                    <tr>
                        <td class="formtd">
                            <table class="checktable_supplier true">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dtSupplies" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="supplliespan" runat="server">
                                                <asp:CheckBox ID="chkSupplies" runat="server" /></span>
                                                <label>
                                                    <asp:Label ID="lblSupplies" Text='<%# Eval("SubCategoryName")%>' runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnSupplies" runat="server" Value='<%#Eval("SubCategoryID")%>' />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="botbtn centeralign">
                <%--<a href="#" class="grey2_btn" title="Edit Information"><span>Edit Information</span></a>--%>
                <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" OnClientClick="checkpaymentoption();"
                    runat="server" ToolTip="Save Information" OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
