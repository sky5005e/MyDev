<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ServiceTicketCenter.aspx.cs" Inherits="admin_ServiceTicketCenter" Title="World-Link System - Support Ticket Center" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdateProgress ID="upPro" runat="server" DisplayAfter="1">
        <ProgressTemplate>
            <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
            </div>
            <div class="updateProgressDiv">
                <img alt="Loading" src="../Images/ajax-loader-large.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upPanel" runat="server">
        <ContentTemplate>
            <div class="view_store_pad form_pad">
                <div>
                    <asp:DataList ID="dtServiceTicketCenter" runat="server" RepeatDirection="Vertical"
                        RepeatColumns="2" OnItemCommand="dtServiceTicketCenter_ItemCommand">
                        <ItemTemplate>
                            <div>
                                <div class="user_manage_btn btn_width_small">
                                    <asp:LinkButton ID="lnkManageName" CommandName='<%# Eval("sManageName")%>' CssClass="gredient_btn"
                                        runat="server"><span><strong><%# Eval("sManageName")%></strong></span></asp:LinkButton>
                                </div>
                            </div>
                            <br />
                            <br />
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
