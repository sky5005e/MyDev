<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CompanyLevleMenuSetting.aspx.cs" Inherits="admin_CompanyLevleMenuSetting" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <h4 style="text-align: center;">
            In this step please select Menu access for all the new uploaded employees for selected
            workgroup.</h4>
        <div class="tabcontent" id="menu_access">
            <div class="alignnone">
                &nbsp;</div>
            <div class="form_pad">
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                </div>
                <h4>
                    Main Menu Options</h4>
                <div class="form_table">
                    <table>
                        <tr>
                            <td class="formtd">
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
                            </td>
                        </tr>
                    </table>
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
                    Supplies/Equipment
                </h4>
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
            </div>
        </div>
        <div class="spacer25">
        </div>
        <div class="bot_alert next">
            <asp:LinkButton ID="lnkNext" runat="server" title="Next Step 3" OnClick="lnkNext_Click">Next Step 3</asp:LinkButton>
        </div>
        <div class="bot_alert prev">
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
    </div>
</asp:Content>
