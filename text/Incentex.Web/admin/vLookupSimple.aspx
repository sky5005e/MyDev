<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="vLookupSimple.aspx.cs" Inherits="admin_vLookupSimple" Title="World-Link System - Dropdown Listing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        var formats = 'jpg|gif|png';
        $().ready(function() {

            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtPriorityName: {
                            required: true
                            // remote: "checkexistence.aspx?action=lookupnameexistence&button=<%=btnSubmit.Text%>"
                        }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtPriorityName: {
                            required: "<br/>" + replaceMessageString(objValMsg, "Required", "icon name")
                            //remote : "Record already exist."
                        }
                    }

                });

            });
        });
        
    </script>

    <style type="text/css">
        .grey2_btn
        {
            background: url("../images/bot-grey-btn.png") no-repeat scroll 0 -35px transparent;
            color: #FFFFFF !important;
            display: inline-block;
            font-size: 15px !important;
            margin-right: 33px;
            padding-left: 7px;
            text-decoration: none;
        }
    </style>
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdatePanel ID="upPanel" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmit" />
            <asp:PostBackTrigger ControlID="lnkAddNew" />
        </Triggers>
        <ContentTemplate>
            <div class="form_pad">
                <div>
                    <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="grey2_btn alignright" OnClick="lnkAddNew_Click"><span>+ Add</span></asp:LinkButton>
                    <br />
                    <br />
                    <br />
                    <asp:DataList ID="dtLstLookup" runat="server" RepeatDirection="Vertical" RepeatColumns="2"
                        OnItemCommand="dtLstLookup_ItemCommand" OnItemDataBound="dtLstLookup_ItemDataBound">
                        <ItemTemplate>
                            <table class="form_table">
                                <tr>
                                    <td class="formtd">
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box clearfix dropdown_search">
                                                <span class="alignleft status_detail">
                                                    <asp:Literal ID="txtLookupName" runat="server" Text='<%# Eval("sLookupName")%>'></asp:Literal>
                                                    <%--<asp:TextBox ID="txtLookupName" runat="server" Text='<%# Eval("sLookupName")%>' MaxLength="100" ></asp:TextBox>--%>
                                                </span><span class="alignright">
                                                    <asp:LinkButton ID="lnkLookupID" CommandName="deletevalue" CommandArgument='<%# Eval("iLookupID")%>'
                                                        runat="server" OnClientClick="return confirm('Are you sure you want to delete record?');"><img src="../images/close-btn.png" alt=""  /></asp:LinkButton>
                                                </span><span class="alignright">
                                                    <asp:LinkButton ID="lnkEdit" CommandName="editvalue" CommandArgument='<%# Eval("iLookupID")%>'
                                                        runat="server"><img src="../images/edit-icon.png" alt="" /></asp:LinkButton>
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
                    <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                    
                    <at:ModalPopupExtender ID="modal" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
                        DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="closepopup">
                    </at:ModalPopupExtender>
                    <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
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
                                                        <div class="form_popup_box">
                                                            <div class="label_bar">
                                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                            </div>
                                                            <div class="label_bar">
                                                                <label>
                                                                    Icon name :</label>
                                                                <asp:TextBox class="popup_input" ID="txtPriorityName" runat="server"></asp:TextBox><span
                                                                    id="duplicate"></span>
                                                            </div>
                                                            <div class="label_bar btn_padinn">
                                                                <asp:Button ID="btnSubmit" Text="Add" runat="server" OnClick="btnSubmit_Click" />
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
