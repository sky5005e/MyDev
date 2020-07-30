<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="proofstatus.aspx.cs" Inherits="admin_proofstatus" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
        function showModal() { $find('ShowModal').show(); }
    </script>
    <at:ToolkitScriptManager ID="src" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdatePanel ID="upPanle" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="flFile" />
            <asp:PostBackTrigger ControlID="btnSubmit" />
        </Triggers>
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
                                                    <asp:LinkButton CommandName="deletevalue" CommandArgument='<%# Eval("iPriorityId")%>'
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
                    <%--<table class="form_table">
                <tr>
                    <td class="formtd">
                        <div id="dvPhotoList">
                        </div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box clearfix dropdown_search">
                                <span class="alignleft status_detail">
                                    <img src="../images/proof-supplier-img.jpg" alt="" />
                                    <input type="text" onfocus="if (this.value == 'Proof Sent from Supplier') {this.value = '';}"
                                        onblur="if (this.value == ''){this.value = 'Proof Sent from Supplier';}" value="Proof Sent from Supplier"
                                        class="proof_input" /></span><span class="alignright"><a href="#"><img src="../images/close-btn.png"
                                            alt="" /></a></span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box clearfix dropdown_search">
                                <span class="alignleft status_detail">
                                    <img src="../images/proof-arrived-img.jpg" alt="" /><input type="text" onfocus="if (this.value == 'Proof Arrived at Office') {this.value = '';}"
                                        onblur="if (this.value == ''){this.value = 'Proof Arrived at Office';}" value="Proof Arrived at Office"
                                        class="proof_input" /></span><span class="alignright"><a href="#"><img src="../images/close-btn.png"
                                            alt="" /></a></span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd_r">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box clearfix dropdown_search">
                                <span class="alignleft status_detail">
                                    <img src="../images/proof-sent-img.jpg" alt="" /><input type="text" onfocus="if (this.value == 'Proof Sent to Customer') {this.value = '';}"
                                        onblur="if (this.value == ''){this.value = 'Proof Sent to Customer';}" value="Proof Sent to Customer"
                                        class="proof_input" /></span><span class="alignright"><a href="#"><img src="../images/close-btn.png"
                                            alt="" /></a></span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="spacer10" colspan="3">
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box clearfix dropdown_search">
                                <span class="alignleft status_detail">
                                    <img src="../images/new-proofrequired-img.jpg" alt="" /><input type="text" onfocus="if (this.value == 'New Proof Required') {this.value = '';}"
                                        onblur="if (this.value == ''){this.value = 'New Proof Required';}" value="New Proof Required"
                                        class="proof_input" /></span><span class="alignright"><a href="#"><img src="../images/close-btn.png"
                                            alt="" /></a></span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box clearfix dropdown_search">
                                <span class="alignleft status_detail">
                                    <img src="../images/proof-approved-img.jpg" alt="" /><input type="text" onfocus="if (this.value == 'Proof Approved') {this.value = '';}"
                                        onblur="if (this.value == ''){this.value = 'Proof Approved';}" value="Proof Approved"
                                        class="proof_input" /></span><span class="alignright"><a href="#"><img src="../images/close-btn.png"
                                            alt="" /></a></span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="spacer10" colspan="3">
                        &nbsp;
                    </td>
                </tr>
            </table>--%>
                </div>
                <div>
                    <%--<a id="lnk" runat="server" class="grey2_btn alignright"><span>+ Add</span></a>--%>
                   
                    <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" style="display:none"></asp:LinkButton>   
                    <asp:LinkButton ID="lnkAddNew" runat="server" 
                        CssClass="grey2_btn alignright" onclick="lnkAddNew_Click"><label><span>+ Add</span></label></asp:LinkButton>
                  
                    <%--<a href="#inline_popup1" id="lnk1" runat="server" class="grey2_btn alignright" rel="prettyPhoto[inline]">
                            <span>+ Add</span></a>--%>
                    <%--<div id="inline_popup1" style="display: none;">
                    <div class="form_popup_box">
                        <div class="label_bar">
                            <label>
                                Username :</label>
                            <span class="popup_input">
                                <asp:TextBox ID="txtPriorityName" runat="server"></asp:TextBox></span>
                        </div>
                        <div class="label_bar">
                            <label>
                                Password :</label>
                            <span class="popup_input">
                                <input type="text" /></span>
                        </div>
                        <div class="label_bar btn_padinn">
                            <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" />
                            
                        </div>
                    </div>
                </div>--%>
                    <at:ModalPopupExtender ID="modal" TargetControlID="lnkAddNew" BackgroundCssClass="modalBackground"
                        DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="closepopup"
                        >
                    </at:ModalPopupExtender>
                    <%--  <asp:Panel ID="pnlPriority" runat="server">
                            <div id="inline_popup1">
                                <div >
                                  
                                </div>
                            </div>
                        </asp:Panel>--%>
                    <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
                        <div class="pp_pic_holder facebook" style="display: block; width: 411px; position:fixed;left:35%;top:30%;">
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
                                        <div class="pp_content" style="height: 228px; display: block;">
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
                                                            <div class="label_bar">
                                                                <div>
                                                                    <asp:Label ID="lblPrioryName" runat="server" Text="Priority"></asp:Label></div>
                                                                <div>
                                                                    <asp:TextBox class="popup_input" ID="txtPriorityName" runat="server"></asp:TextBox></div>
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
                                                                    <input type="file" class="popup_input" id="flFile" runat="server" /></div>
                                                            </div>
                                                            <br />
                                                            <div class="label_bar btn_padinn">
                                                                <asp:Button ID="btnSubmit" class="popup_btngrey" Text="Add" runat="server" OnClick="btnSubmit_Click" />
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
