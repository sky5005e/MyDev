<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="DisplayMailingList.aspx.cs" Inherits="admin_CommunicationCenter_DisplayMailingList"
    Title="Communication Center" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div class="user_manage_btn btn_width_small">
            <%--<asp:HyperLink ID="lnkSendMail" class="gredient_btn" title="Send Mailing Now" runat="server">
                <span style="width:200px;">
                <strong>
                  Send Mail Now
                </span>
            </asp:HyperLink>--%>
            <table>
                <tr>
                    <td class="centeralign">
                        <span>
                            <div style="text-align: center; color: Red; font-size: 14px;">
                                <asp:Label ID="Label1" runat="server">
                                </asp:Label>
                            </div>
                        </span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer25">
        </div>
        <div class="bot_alert prev">
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
