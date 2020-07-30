<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="addpopup.aspx.cs" Inherits="RND_addpopup" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        var formats = 'jpg|gif|png';

        $().ready(function() {
            $("#aspnetForm").validate({
                rules: {

                    ctl00$ContentPlaceHolder1$txtPriorityName: {
                        required: true

                    },

                    ctl00$ContentPlaceHolder1$flFile:
                     {
                         required: true,
                         accept: formats
                     }

                },
                messages: {
                    ctl00$ContentPlaceHolder1$txtPriorityName: {
                        required: "<br/>Please enter Name."

                    },
                    ctl00$ContentPlaceHolder1$flFile: { required: "<br />Please select file to upload.", accept: "<br />File type not supported." }


                }

            });
        });
 
    </script>

    
    <at:ToolkitScriptManager ID="src" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdatePanel ID="upPanle" runat="server">
        <ContentTemplate>
            <div class="form_pad">
                <div>
                    <asp:DataList ID="dtLst" runat="server" RepeatDirection="Vertical" OnItemDataBound="dtLst_ItemDataBound"
                        RepeatColumns="3" OnItemCommand="dtLst_ItemCommand">
                        <ItemTemplate>
                            <table class="form_table">
                                <tr>
                                    <td class="formtd">
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box clearfix dropdown_search">
                                                <span class="alignleft status_detail">
                                                    <asp:HiddenField ID="hf" Value='<%# DataBinder.Eval(Container.DataItem, "sPriorityIcon")%>'
                                                        runat="server" />
                                                    <img id="imgBtn" runat="server" alt='Loading' />
                                                    <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("sPriorityName")%>'></asp:TextBox>
                                                    <%--onblur="javascript:Reset(this.id,<%#Eval("sPriorityName")%>);"--%>
                                                </span><span class="alignright">
                                                    <asp:LinkButton ID="lnkEdit" CommandName="editvalue" CommandArgument='<%# Eval("iPriorityId")%>'
                                                        runat="server"><img src="../images/close-btn.png" alt="" /></asp:LinkButton>
                                                </span></span><span class="alignright">
                                                    <asp:LinkButton ID="LinkButton1" CommandName="deletevalue" CommandArgument='<%# Eval("iPriorityId")%>'
                                                        runat="server"><img src="../images/close-btn.png" alt="" /></asp:LinkButton>
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
                <div>
                    <a id="lnk" runat="server" class="grey2_btn alignright"><span>+ AddAjax</span></a>
                    <at:ModalPopupExtender ID="modal" TargetControlID="lnk" BackgroundCssClass="modalBackground"
                        DropShadow="true" runat="server" 
                        X="250"
                        Y="250"
                        PopupControlID="pnlPriority" CancelControlID="closepopup">
                    </at:ModalPopupExtender>
                    <asp:Panel ID="pnlPriority" runat="server">
                        <div class="pp_pic_holder facebook" style="display: block;
                             width: 411px;">
                            <div class="pp_top" style="">
                                <div class="pp_left">
                                </div>
                                <div class="pp_middle">
                                </div>
                                <div class="pp_right">
                                </div>
                            </div>
                            <div class="pp_content_container">
                                <div class="pp_left" >
                                    <div class="pp_right" >
                                        <div class="pp_content" style="height: 250px; display: block;">
                                            <div class="pp_loaderIcon" style="display: none;">
                                            </div>
                                            <div class="pp_fade" style="display: block;">
                                                <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                                    <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                        style="visibility: visible;">previous</a>
                                                </div>
                                                <div id="pp_full_res" style="">
                                                    <div class="pp_inline clearfix">
                                                        <div>
                                                            <div>
                                                                <div>
                                                                    <asp:Label ID="lblPrioryName" runat="server" Text="Priority"></asp:Label></div>
                                                                <div>
                                                                    <asp:TextBox ID="txtPriorityName" runat="server"></asp:TextBox></div>
                                                            </div>
                                                            <div id="icondiv" runat="server" visible="false">
                                                                <div>
                                                                    <asp:Label ID="lblPriorityIcon" runat="server" Text="Priority Icon"></asp:Label></div>
                                                                <div>
                                                                    <img id="imgEdit" runat="server" alt="load" />
                                                                </div>
                                                            </div>
                                                            <div>
                                                                <div>
                                                                    <asp:Label ID="lblNew" runat="server" Text="Upload New Icon"></asp:Label></div>
                                                                <div>
                                                                    <input type="file" id="flFile" runat="server" /></div>
                                                            </div>
                                                             <div id="Div1" runat="server" visible="false">
                                                                <div>
                                                                    <asp:Label ID="Label1" runat="server" Text="Priority Icon"></asp:Label></div>
                                                                <div>
                                                                    <img id="img1" runat="server" alt="load" />
                                                                </div>
                                                            </div>
                                                            <div>
                                                                <div>
                                                                    <asp:Label ID="Label2" runat="server" Text="Upload New Icon"></asp:Label></div>
                                                                <div>
                                                                    <input type="file" id="File1" runat="server" /></div>
                                                            </div>
                                                            
                                                             <div id="Div2" runat="server" visible="false">
                                                                <div>
                                                                    <asp:Label ID="Label3" runat="server" Text="Priority Icon"></asp:Label></div>
                                                                <div>
                                                                    <img id="img2" runat="server" alt="load" />
                                                                </div>
                                                            </div>
                                                            <div>
                                                                <div>
                                                                    <asp:Label ID="Label4" runat="server" Text="Upload New Icon"></asp:Label></div>
                                                                <div>
                                                                    <input type="file" id="File2" runat="server" /></div>
                                                            </div>
                                                             <div id="Div3" runat="server" visible="false">
                                                                <div>
                                                                    <asp:Label ID="Label5" runat="server" Text="Priority Icon"></asp:Label></div>
                                                                <div>
                                                                    <img id="img3" runat="server" alt="load" />
                                                                </div>
                                                            </div>
                                                            <div>
                                                                <div>
                                                                    <asp:Label ID="Label6" runat="server" Text="Upload New Icon"></asp:Label></div>
                                                                <div>
                                                                    <input type="file" id="File3" runat="server" /></div>
                                                            </div>
                                                            <div>
                                                                <div id="dvAdd">
                                                                    <asp:Button ID="btnSubmit" Text="Add" runat="server" OnClick="btnSubmit_Click" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="pp_details clearfix" style="width: 371px;">
                                                    <a href="#" id="closepopup" runat="server" class="pp_close">Close</a>
                                                    <p class="pp_description" style="display: none;">
                                                    </p>
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
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
