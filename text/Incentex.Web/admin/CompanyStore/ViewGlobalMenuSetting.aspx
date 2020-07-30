<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewGlobalMenuSetting.aspx.cs" Inherits="admin_CompanyStore_ViewGlobalMenuSetting"
    Title="Global Menu Settings" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <asp:DataList ID="dtLstLookup" runat="server" RepeatDirection="Vertical" RepeatColumns="1"
            OnItemDataBound="dtLstLookup_ItemDataBound">
            <ItemTemplate>
                <table class="form_table">
                    <tr>
                        <td class="formtd">
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box clearfix dropdown_search">
                                    <span class="alignleft status_detail">
                                        <asp:HiddenField ID="hdnLookupId" Value='<%# DataBinder.Eval(Container.DataItem, "iLookupID")%>'
                                            runat="server" />
                                        <asp:Literal ID="txtLookupName" runat="server" Text='<%# Eval("sLookupName")%>'></asp:Literal>
                                    </span><span class="alignright">
                                        <asp:HyperLink ID="lnkEdit" CommandName="editvalue" CommandArgument='<%# Eval("iLookupID")%>'
                                            runat="server"><img src="~/Images/edit-icon.png" runat="server" alt="Loading" /></asp:HyperLink>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>
    </div>
</asp:Content>
