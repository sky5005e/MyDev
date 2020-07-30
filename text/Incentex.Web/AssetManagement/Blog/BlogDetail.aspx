<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="BlogDetail.aspx.cs" Inherits="AssetManagement_Blog_BlogDetail" Title="Blog Center" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_popup_box span.error
        {
            padding: 0px;
        }
        .fcolor
        {
            color: #72757C;
        }
    </style>

    <script type="text/javascript" language="javascript">
         function pageLoad(sender, args) {
             assigndesign();
         }  
         
    </script>
     <script type="text/javascript" language="javascript">
         $().ready(function() {
             $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                 objValMsg = $.xml2json(xml); $("#aspnetForm").validate(
                {
                    rules:
                    {            
                      
                        ctl00$ContentPlaceHolder1$txtTitle: { required: true },
                        ctl00$ContentPlaceHolder1$txtDescription: { required: true }
                    },
                    messages:
                    {
                   
                        ctl00$ContentPlaceHolder1$txtTitle: { required: replaceMessageString(objValMsg, "Required", "Title") },
                        ctl00$ContentPlaceHolder1$txtDescription: { required: replaceMessageString(objValMsg, "Required", "Description") }
                    }
                });
             });
             $("#<%=lnkPost.ClientID %>").click(function() {
                 return $('#aspnetForm').valid();
             });
         });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div>
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <asp:Repeater ID="rptTitle" runat="server" OnItemDataBound="rptTitle_OnItemDataBound"
                OnItemCommand="rptTitle_OnItemCommand">
                <ItemTemplate>
                    <div class="black_top_co">
                        <span>&nbsp;</span></div>
                    <div class="black_middle">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <%-- <td width="15%" rowspan="2">                                   
                                      <asp:Image ID="imgBlog" runat="server" Height="130" Width="100" />
                                </td>--%>
                                <td width="98%">
                                    <h2>
                                        <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("BlogTitleName") %>' CssClass="fcolor"></asp:Label>
                                    </h2>
                                </td>
                                <td width="2%">
                                    <asp:LinkButton ID="lnkBtnDeleteDoc" CommandArgument='<%# Eval("BlogTitleID") %>'
                                        CommandName="DeleteBlog" runat="server" ToolTip="Delete Blog" OnClientClick="return DeleteConfirmation();">
                                 <img src="../../Images/delete_products_icon.png" alt="Delete Image" /></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <h4>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Convert.ToString(Eval("TitleDescription")).Length > 221 ? Convert.ToString(Eval("TitleDescription")).Substring(0, 221).Trim() + "..." : "&nbsp;" + Convert.ToString(Eval("TitleDescription")) %>'></asp:Label>
                                        </h4>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <td>
                                            <asp:LinkButton ID="lnkContinueReading" runat="server" Text="Continue Reading.."
                                                CssClass="fcolor" CommandArgument='<%# Eval("BlogTitleID") %>' CommandName="Continue"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <div class="rightalign">
                                                <asp:Label ID="lblPostedBy" runat="server" CssClass="fcolor"></asp:Label>
                                            </div>
                                        </td>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="label_bar rightalign">
                                        <asp:LinkButton ID="lnkViewImage" runat="server" Text="View Image" CssClass="fcolor"
                                            CommandArgument='<%# Eval("FilePath") %>' CommandName="Image"></asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                            <br />
                        </table>
                    </div>
                    <div class="black_bot_co">
                        <span>&nbsp;</span></div>
                </ItemTemplate>
            </asp:Repeater>
            <br />
            <div class="centeralign">
                <asp:LinkButton ID="lnkAddBlog" runat="server" CssClass="grey2_btn"><span>Add Comment</span></asp:LinkButton>
                <at:ModalPopupExtender ID="ModallnkAddBlog" TargetControlID="lnkAddBlog" BackgroundCssClass="modalBackground"
                    DropShadow="true" runat="server" PopupControlID="pnllnkAddBlog" CancelControlID="PopupCloselnkAddBlog">
                </at:ModalPopupExtender>
                <asp:Panel ID="pnllnkAddBlog" runat="server" Style="display: none;">
                    <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                        left: 35%; top: 30%;">
                        <div class="pp_top" style="">
                            <div class="pp_left">
                            </div>
                            <div class="pp_middle">
                            </div>
                            <div class="pp_right">
                            </div>
                        </div>
                        <div class="pp_content_container" style="">
                            <div class="pp_left" style="">
                                <div class="pp_right" style="">
                                    <div class="pp_content" style="height: 350px; display: block;">
                                        <div class="pp_loaderIcon" style="display: none;">
                                        </div>
                                        <div class="pp_fade" style="display: block;">
                                            <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                            <div class="pp_hoverContainer" style="height: 350px; width: 150px; display: none;">
                                                <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                    style="visibility: visible;">previous</a>
                                            </div>
                                            <div id="Div9">
                                                <div style="text-align: center">
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="errormessage"></asp:Label>
                                                </div>
                                                <div class="pp_inline clearfix">
                                                    <div class="form_popup_box">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span class="input_label">Company</span>
                                                                </td>
                                                                <td>
                                                                    <div class="label_bar">
                                                                        <asp:DropDownList ID="ddlCompany" Width="240px" runat="server" CssClass="w_label">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="input_label">Post</span>
                                                                </td>
                                                                <td>
                                                                    <div class="label_bar">
                                                                        <asp:DropDownList ID="ddlIsInternal" Width="240px" runat="server" CssClass="w_label">
                                                                            <asp:ListItem Selected="True" Text="External" Value="false"></asp:ListItem>
                                                                            <asp:ListItem Text="Internal" Value="true"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="input_label">Show Info</span>
                                                                </td>
                                                                <td>
                                                                    <div class="label_bar">
                                                                        <asp:DropDownList ID="ddlShowUser" Width="240px" runat="server" CssClass="w_label">
                                                                            <asp:ListItem Selected="True" Text="Show" Value="false"></asp:ListItem>
                                                                            <asp:ListItem Text="Don't Show" Value="true"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="input_label">Subject/Blog Post Title</span>
                                                                </td>
                                                                <td>
                                                                    <div class="label_bar">
                                                                        <asp:TextBox ID="txtTitle" Width="240px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="input_label">Post Question</span>
                                                                </td>
                                                                <td>
                                                                    <div class="label_bar">
                                                                        <asp:TextBox ID="txtDescription" Width="240px" runat="server" MaxLength="100" CssClass="w_label"
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="input_label">Upload Subject Images</span>
                                                                </td>
                                                                <td>
                                                                    <div class="label_bar">
                                                                        <input type="file" id="BlogImage" runat="server" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="centeralign" colspan="2" style="padding-top: 20px">
                                                                    <asp:LinkButton ID="lnkPost" runat="server" class="grey2_btn" OnClick="lnkPost_Click"
                                                                        ToolTip="Post"><span>Post Question</span></asp:LinkButton>
                                                                    <%--<a href="#" class="grey_btn" title="Save Information"><span>Save Information</span></a>--%>
                                                                    <div class="pp_details clearfix">
                                                                        <a id="PopupCloselnkAddBlog" runat="server" class="pp_close" href="#">Close</a>
                                                                        <p class="pp_description" style="display: none;">
                                                                        </p>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="pp_bottom" style="">
                            <div class="pp_left" style="">
                            </div>
                            <div class="pp_middle" style="">
                            </div>
                            <div class="pp_right" style="">
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="centeralign">
                <asp:LinkButton ID="lnkImage" runat="server" Style="display: none;"></asp:LinkButton>
                <at:ModalPopupExtender ID="ModalImage" TargetControlID="lnkImage" BackgroundCssClass="modalBackground"
                    DropShadow="true" runat="server" PopupControlID="pnlImage" CancelControlID="PopupCloseImage">
                </at:ModalPopupExtender>
                <asp:Panel ID="pnlImage" runat="server" Style="display: none;">
                    <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                        left: 35%; top: 30%;">
                        <div class="pp_top" style="">
                            <div class="pp_left">
                            </div>
                            <div class="pp_middle">
                            </div>
                            <div class="pp_right">
                            </div>
                        </div>
                        <div class="pp_content_container" style="">
                            <div class="pp_left" style="">
                                <div class="pp_right" style="">
                                    <div class="pp_content" style="height: 350px; display: block;">
                                        <div class="pp_loaderIcon" style="display: none;">
                                        </div>
                                        <div class="pp_fade" style="display: block;">
                                            <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                            <div class="pp_hoverContainer" style="height: 350px; width: 150px; display: none;">
                                                <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                    style="visibility: visible;">previous</a>
                                            </div>
                                            <div id="Div1">
                                                <div style="text-align: center">
                                                    <asp:Label ID="Label1" runat="server" CssClass="errormessage"></asp:Label>
                                                </div>
                                                <div>
                                                    <div class="form_popup_box centeralign">
                                                        <asp:Image ID="imgBlog" runat="server" Height="300" Width="300" />
                                                    </div>
                                                </div>
                                                <div class="pp_details clearfix">
                                                    <a id="PopupCloseImage" runat="server" class="pp_close" href="#">Close</a>
                                                    <p class="pp_description" style="display: none;">
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="pp_bottom" style="">
                            <div class="pp_left" style="">
                            </div>
                            <div class="pp_middle" style="">
                            </div>
                            <div class="pp_right" style="">
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
