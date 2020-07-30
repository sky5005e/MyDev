<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CampDetail.aspx.cs" Inherits="admin_CommunicationCenter_CampDetail"
    Title="Communication Center" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div class="form_table">
            <table class="dropdown_pad ">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Campaign Name</span>
                                <asp:TextBox ID="txtCampaignName" runat="server" MaxLength="20" CssClass="w_label"
                                    ReadOnly="True"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Numbers Of User</span>
                                <asp:TextBox ID="TxtNumOfUser" ReadOnly="True" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Viewed</span>
                                <asp:TextBox ID="TxtViewd" runat="server" ReadOnly="True" MaxLength="20" CssClass="w_label"
                                    Style="width: 24%;"></asp:TextBox>
                                <asp:LinkButton ID="btnViewed" runat="server" ToolTip="Emails Viewed">
                           
                            <img id="imgView" class="grey_btn alignright" runat="server" src="../Incentex_Used_Icons/textview_email_button.png"
                                     alt="View" /></asp:LinkButton>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Not Open</span>
                                <asp:TextBox ID="TxtNotOpen" runat="server" MaxLength="20" ReadOnly="True" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Bounced</span>
                                <asp:TextBox ID="TxtBounced" runat="server" MaxLength="20" ReadOnly="True" CssClass="w_label"
                                    Style="width: 15%;"></asp:TextBox>
                                <asp:LinkButton ID="btnBounce" runat="server" ToolTip="Un-deliverable emails">
                             <img id="imgBounced" class="grey_btn alignright" runat="server" src="../Incentex_Used_Icons/undeliveredemail_button.png"
                                     alt="Bounced" /></asp:LinkButton>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
