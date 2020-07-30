<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="BlogSearch.aspx.cs" Inherits="AssetManagement_Blog_BlogSearch" Title="Search Blog" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div class="btn_width worldlink_btn form_table">
            <table width="100%" cellpadding="0" cellspacing="0">                
                <tr>
                    <td>
                        <div style="margin: 0 85px 0 60px;">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label leftalign" style="width: 22%">Search String</span>
                                <asp:TextBox ID="txtSearchString" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="space20">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="margin: 0 0 0 300px;">
                            <asp:LinkButton ID="lnkSearchBlog" class="gredient_btnMainPage" title="Search Blogs"
                                runat="server" OnClick="lnkSearchBlog_Click">
                <img src="../../admin/Incentex_Used_Icons/searchinventory.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Search Blogs           
                </span>
                            </asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

