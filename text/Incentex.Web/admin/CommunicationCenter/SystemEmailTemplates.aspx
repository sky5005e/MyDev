<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="SystemEmailTemplates.aspx.cs" Inherits="admin_CommunicationCenter_SystemEmailTemplates" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="sc1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvTemp" runat="server" style="margin: 0px 120px 0px 120px;">
        <div class="spacer25">
        </div>
        <div class="errormessage" style="font-size: 12px;">
            <h4 style="font-size: 10px;">
                <i>Warning while editing templates:</i></h4>
            Please do not change the text which are between "{ text }".<br />
            ie if you will make any change like {fullname} = {whatever}. Then user name will
            not replace on the emails
        </div>
        <div class="spacer25">
        </div>
        <asp:GridView ID="gvTemplatesList" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvTemplatesList_RowDataBound"
            OnRowCommand="gvTemplatesList_RowCommand">
            <Columns>
                <asp:TemplateField Visible="False" HeaderText="Id">
                    <HeaderTemplate>
                        <span>ID#</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblID" Text='<%# Eval("Key") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="2%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Template Name</span>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" CssClass="first" ID="lblTempName" Text='<%# Eval("Value")%>' />
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="30%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Details</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:LinkButton ID="hypViewTemp" CommandName="open" runat="server" ToolTip="view templates">View Templates</asp:LinkButton>
                        </span>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="30%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="ViewTemp">
                    <HeaderTemplate>
                        <span>&nbsp;</span>
                        <div class="corner">
                                <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                            </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:LinkButton ID="hypEditTemp" CommandName="Modify" CommandArgument='<%# Eval("Value")%>'
                                runat="server" ToolTip="view templates">Edit</asp:LinkButton>
                        </span>
                         <div class="corner">
                                <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                            </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="5%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div>
        <asp:Panel ID="pnlRTB" runat="server" Style="display: none;" d>
            <div class="fancybox-overlay">
            </div>
            <div class="pp_pic_holder facebook" style="display: block; width: auto; height: auto;
                position: fixed; left: 10%; top: 5%;">
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
                            <div class="pp_content" style="height: auto; display: block;">
                                <div class="pp_loaderIcon" style="display: none;">
                                </div>
                                <div class="pp_fade" style="display: block;">
                                    <div id="pp_full_res">
                                        <div class="pp_inline clearfix">
                                            <div class="form_popup_box">
                                                <div class="note" style="width: 100%;">
                                                    <img src="../../Images/lightbulb.gif" alt="note:" />&nbsp;Warning : Please do not
                                                    change the text which are between "{ text }" . ie if you will make any change like
                                                    {fullname} = {whatever}. Then user name will not replace on the emails.
                                                </div>
                                                <CKEditor:CKEditorControl ID="TxtEmailText" BasePath="../../JS/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                                            </div>
                                            <div class="centeralign">
                                                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="grey2_btn" OnClick="btnSubmit_Click">
								                                      <span>Save</span>
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
                <div class="pp_bottom" style="margin-top: -2px;">
                    <div class="pp_left" style="">
                    </div>
                    <div class="pp_middle" style="">
                    </div>
                    <div class="pp_right" style="">
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:LinkButton ID="lnkPopup" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
        <at:ModalPopupExtender ID="modalPopup" TargetControlID="lnkPopup" BackgroundCssClass="modalBackground"
            DropShadow="true" Drag="true" runat="server" PopupControlID="pnlRTB" CancelControlID="closepopup">
        </at:ModalPopupExtender>
    </div>
</asp:Content>
