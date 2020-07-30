<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="vLookupReasonForRepair.aspx.cs" Inherits="AssetManagement_vLookupReasonForRepair" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        $().ready(function() {

            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {
                    ctl00$ContentPlaceHolder1$txtRepairReasonName: {
                            required: true
                        }
                    },
                    messages: {
                    ctl00$ContentPlaceHolder1$txtRepairReasonName: {
                    required: replaceMessageString(objValMsg, "Required", "Repair Reason name")
                        }
                    }

                });

            });
        });
        
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div>
            <div style="text-align: center">
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div class="spacer10">
    </div>
            <asp:DataList ID="dtlRepairReason" runat="server" RepeatDirection="Vertical" RepeatColumns="2"
                OnItemCommand="dtlRepairReason_ItemCommand">
                <ItemTemplate>
                    <table class="form_table">
                        <tr>
                            <td class="formtd">
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box clearfix">
                                        <span class="alignleft" style="padding:8px 0px;"><%# Eval("sLookupName")%></span>
                                        <span class="alignright">
                                            <asp:LinkButton ID="lnkDelete" CommandName="deletevalue" CommandArgument='<%# Eval("iLookupID")%>'
                                                runat="server" OnClientClick="return confirm('Are you sure you want to delete record?');"><img src="../images/close-btn.png" alt=""  /></asp:LinkButton>
                                        </span>
                                        <span class="alignright">
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
            <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="grey2_btn alignright" OnClick="lnkAddNew_Click"><span>+ Add</span></asp:LinkButton>
            <at:ModalPopupExtender ID="modal" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
                DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="closepopup">
            </at:ModalPopupExtender>
            <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
                <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                    left: 35%; top: 30%;">
                    <div class="pp_top">
                        <div class="pp_left">
                        </div>
                        <div class="pp_middle">
                        </div>
                        <div class="pp_right">
                        </div>
                    </div>
                    <div class="pp_content_container">
                        <div class="pp_left">
                            <div class="pp_right">
                                <div class="pp_content" style="height: 228px; display: block;">
                                    <div class="pp_fade" style="display: block;">
                                        <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                        <div id="pp_full_res">
                                            <div class="pp_inline clearfix">
                                                <div class="form_popup_box">
                                                    <div class="label_bar" style="text-align:center;">
                                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                    </div>
                                                    <div class="label_bar">
                                                        <label style="display:inline;">
                                                           Repair Reason name :</label>
                                                        <asp:TextBox class="popup_input" ID="txtRepairReasonName" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="label_bar btn_padinn">
                                                        <asp:Button ID="btnSubmit" Text="Add" runat="server" OnClick="btnSubmit_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="pp_details clearfix" style="width: 371px;">
                                            <a href="#" id="closepopup" runat="server" class="pp_close">Close</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="pp_bottom">
                        <div class="pp_left">
                        </div>
                        <div class="pp_middle">
                        </div>
                        <div class="pp_right">
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>


