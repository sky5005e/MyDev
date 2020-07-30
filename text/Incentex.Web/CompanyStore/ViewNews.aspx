<%@ Page Title="Recent News" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewNews.aspx.cs" Inherits="CompanyStore_ViewNews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <asp:Repeater ID="rptNews" runat="server">
            <ItemTemplate>
                <div>
                    <div class="black_top_co">
                        <span>&nbsp;</span></div>
                    <div class="black_middle clearfix">
                        <div>
                            <table>
                                <tr>
                                    <td style="width: 135px;" align="left" valign="top">
                                        <a href='<%# SetNews(Eval("NewsTitle")) %>' target="_blank" class="newsheadline">
                                            <img class="alignleft newsimg" style="height: 128px; width: 128px;" alt="Recent News" src="../UploadedImages/CompanyStoreDocuments/PDF_File.png">
                                        </a>
                                    &nbsp;&nbsp;</td>
                                    <td valign="top">
                                        <a href='<%# SetNews(Eval("NewsTitle")) %>' target="_blank" class="newsheadline">
                                            <%#Eval("NewsTitleDes")%></a><br />
                                        <span class="font_small10">Posted On:
                                            <%# Convert.ToDateTime(Eval("NewsPostDate")).ToLongDateString() %></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="spacer10">
                        </div>
                    </div>
                    <div class="black_bot_co">
                        <span>&nbsp;</span></div>
                </div>
            </ItemTemplate>
            <SeparatorTemplate>
                <div class="spacer20">
                </div>
            </SeparatorTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
