<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ShoppingSetting.aspx.cs" Inherits="admin_Supplier_ShoppingSetting" Title="Storefront Setting" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                Manage Email</h4>
                <div class="form_table" id="dvManageEmail">
                <table>
                    <tr>
                        <td class="formtd">
                            <table class="checktable_supplier true">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dtManageEmail" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="ManageEmailspan" runat="server">
                                                    <asp:CheckBox ID="chkManageEmail" runat="server" />
                                                </span>
                                                <label>
                                                    <asp:Label ID="lblManageEmail" Text='<%# Eval("ManageEmailName") %>' runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnManageEmail" runat="server" Value='<%#Eval("ManageEmailID")%>' />
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
            <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" TabIndex="19" OnClick="lnkSave_Click">
								        <span>Save Information</span>
            </asp:LinkButton>
        </div>
    </div>
    </div>
</asp:Content>

