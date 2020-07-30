<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="BlogComments.aspx.cs" Inherits="AssetManagement_Blog_BlogComments"
    Title="Blog Center Comments" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
         .fcolor
        {
            color: #72757C;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div class="form_table">
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="25%" rowspan="2">
                        <asp:Image ID="imgBlog" runat="server" Height="240" Width="200" />
                    </td>
                    <td width="75%">
                        <h2>
                            <asp:Label ID="lblTitle" runat="server" CssClass="fcolor"></asp:Label>
                        </h2>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div>
                        <h4>
                            <asp:Label ID="lblDescription" runat="server" CssClass="fcolor"></asp:Label>
                            </h4>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Repeater ID="rptComments" runat="server"  OnItemCommand="rptComments_OnItemCommand" OnItemDataBound="rptComments_OnItemDataBound">
                            <ItemTemplate>
                                <div class="black_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="black_middle form_table">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="98%">
                                                <div>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("BlogCommentDesc") %>' CssClass="fcolor"></asp:Label>
                                                </div>
                                            </td>
                                            <td width="2%">
                                                <asp:LinkButton ID="lnkBtnDeleteDoc" CommandArgument='<%# Eval("BlogCommentID") %>'
                                                    CommandName="DeleteBlog" runat="server" ToolTip="Delete Blog" OnClientClick="return DeleteConfirmation();">
                                 <img src="../../Images/delete_products_icon.png" alt="Delete Image" /></asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                        <td  class="rightalign">
                                        <asp:Label ID="lblPostedBy" runat="server" CssClass="fcolor"></asp:Label>
                                        </td>
                                        </tr>
                                    </table>
                                </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td  colspan="2">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box taxt_area clearfix">
                                <span class="input_label alignleft">Comment</span>
                                <div class="textarea_box alignright" style="width: 79%;">
                                    <div class="scrollbar">
                                        <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                            class="scrollbottom"></a></label>
                                    </div>
                                    <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                        Height="70px"></asp:TextBox>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="centeralign">
                            <asp:LinkButton ID="lnkAddBlog" runat="server" CssClass="grey2_btn" 
                                onclick="lnkAddBlog_Click"><span>Add Comment</span></asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
        </div>
    </div>
</asp:Content>
