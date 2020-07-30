<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CreateCampaignStep5.aspx.cs" Inherits="admin_CommunicationCenter_CreateCampaignStep5"
    Title="Communications Center" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div class="form_table">
            <%--<asp:HyperLink ID="lnkSendMail" class="gredient_btn" title="Send Mailing Now" runat="server">
                <span style="width:200px;">
                <strong>
                  Send Mail Now
                </span>
            </asp:HyperLink>--%>
            <table class="dropdown_pad" style="width:250px;">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label1" Style="display: none"></asp:Label>
                        <asp:LinkButton ID="lnkSendMail" CssClass="gredient_btn1" runat="server" OnClick="lnkSendMail_Click"
                            align="center">
                             <img src="../Incentex_Used_Icons/send_mailing.png" alt="Send Mail Now" />
                            <span>Send Mailing Now</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div>
         <asp:TextBox ID =TxtEmailText runat ="server" Visible ="false" ></asp:TextBox>
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
