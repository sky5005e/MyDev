<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ProductNote.aspx.cs" Inherits="admin_ProductManagement_ProductNote" Title="Product >> Note" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 36px;
        }
    </style>
    <style type="text/css">
        .textarea_box textarea
        {
            height: 178px;
        }
        .textarea_box
        {
            height: 178px;
        }
        .textarea_box .scrollbar
        {
            height: 185px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" language="javascript">


    $().ready(function() {
        $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
            objValMsg = $.xml2json(xml);
            $("#aspnetForm").validate({
               
            });

        });
    });
    </script>
<at:ToolkitScriptManager ID="sc1" runat="server"></at:ToolkitScriptManager>
 <mb:MenuUserControl ID="menuControl" runat="server" />

				<div class="form_pad">
				<div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
					<div>
						<table class="form_table">
							<tr>
								<td>
									<div>
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box taxt_area clearfix" style="height: 180px">
                        <span class="input_label alignleft" style="height: 178px">Notes/History</span>
                        <div class="textarea_box alignright">
                            <div class="scrollbar" style="height: 182px">
                                <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                    class="scrollbottom"></a>
                            </div>
                            <asp:TextBox ID="txtNoteHistory" runat="server" TabIndex="30" TextMode="MultiLine"
                                CssClass="scrollme1" Height="178px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
								</td>
							</tr>
							<tr>
								<td class="gallery" colspan="3">
								 <div class="rightalign gallery">
                    <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                    <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="grey2_btn alignright" OnClick="lnkAddNew_Click"><span>+ Add Note</span></asp:LinkButton>
                    <at:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
                        DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup">
                    </at:ModalPopupExtender>
                </div>
                                     </td>
							</tr>
						</table>
					</div>
					<asp:Panel ID="pnlNotes" runat="server" Style="display: none;">
                    <div class="pp_pic_holder facebook" style="display: block; width: 411px;position:fixed;left:35%;top:30%;">
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
                                            <div id="pp_full_res">
                                                <div class="pp_inline clearfix">
                                                    <div class="form_popup_box">
                                                        <div class="label_bar">
                                                            <span>Add Notes / History
                                                                <br />
                                                                <br />
                                                                <asp:TextBox Height="120" Width="350" TextMode="MultiLine" ID="txtNote" runat="server"></asp:TextBox></span>
                                                        </div>
                                                        <div >
                                                            <asp:LinkButton ID="btnSubmit" runat="server" CssClass="grey2_btn" OnClick="btnSubmit_Click">
								                                      <span>Save Notes</span>
                                                            </asp:LinkButton>
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
		
</asp:Content>

