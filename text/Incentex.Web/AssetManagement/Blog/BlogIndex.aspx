<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="BlogIndex.aspx.cs" Inherits="AssetManagement_Blog_BlogIndex" Title="Blog Center Index" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div class="btn_width worldlink_btn form_table">
            <table width="100%" cellpadding="0" cellspacing="0">
                
                 
                <tr>
                <td style="width:33%">
                 <asp:LinkButton ID="lnkPostBlog" class="gredient_btnMainPage" title="Post Blog"
                            runat="server" OnClick="lnkPostBlog_Click">
                <img src="../../admin/Incentex_Used_Icons/Post-Blog.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Post Blog           
                </span>
                        </asp:LinkButton> 
                </td>
                <td style="width:33%">
                      <asp:LinkButton ID="lnkSearchBlog" class="gredient_btnMainPage" title="Search Blogs"
                            runat="server" OnClick="lnkSearchBlog_Click">
                <img src="../../admin/Incentex_Used_Icons/Search-Blogs.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Search Blogs           
                </span>
                        </asp:LinkButton> 
                </td>
               
                <td style="width:33%">    
                      <asp:LinkButton ID="lnkTodayBlog" class="gredient_btnMainPage" title="Today's Blogs"
                            runat="server"  OnClick="lnkTodayBlog_Click" >
                <img src="../../admin/Incentex_Used_Icons/Today's-Blogs.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Today's Blogs           
                </span>
                        </asp:LinkButton> 
                </td>
                </tr>
                
            </table>
        </div>
    </div>
</asp:Content>
