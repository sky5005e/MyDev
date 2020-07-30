<%@ Page Title="World-Link System - Manage State" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="State.aspx.cs" Inherits="admin_State" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        var formats = 'jpg|gif|png';
        $().ready(function() {

            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtState: {
                            required: true
                        }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtState: {
                            required: "<br/>" + replaceMessageString(objValMsg, "Required", "state")
                        }
                    }
                });

            });
        });

        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
     <asp:UpdateProgress runat="server" ID="uprogressPGrid" DisplayAfter="1" AssociatedUpdatePanelID="upPanel">
        <ProgressTemplate>
            <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
            </div>
            <div class="updateProgressDiv">
                    <img alt="Loading" src="../Images/ajax-loader-large.gif"/>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upPanel" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmit" />
            <asp:PostBackTrigger ControlID="lnkAddNew" />
        </Triggers>
        <ContentTemplate>
            <div class="form_pad">
                <div class="centeralign">
                    <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
                <div class="not_yet_pad">
                    <table class="form_table">
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Country</span> <span class="custom-sel label-sel">
                                                        <asp:DropDownList ID="ddlCountry" runat="server" onchange="pageLoad(this,value);"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvState" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvState_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>State Name</span>
                                                            <div class="corner">
                                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span class="first"><%# Eval("sStateName")%></span>
                                                            <div class="corner">
                                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                            </div>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box"/>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Edit</span>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span class-"btn_space">
                                                                <asp:LinkButton ID="lnkbtnedit" CommandName="EditState" CommandArgument='<%# Eval("iStateID") %>'
                                                                    runat="server">
                                                                    <img id="edit" src="~/Images/edit-icon.png" height="25" width="25" runat="server" /></asp:LinkButton></span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Delete</span>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span class-"btn_space">
                                                                <asp:LinkButton ID="lnkbtndelete" CommandName="DeleteState" OnClientClick="javascript:return confirm('Are you sure, you want to delete selected records?');"
                                                                    CommandArgument='<%# Eval("iStateID") %>' runat="server">
                                                                    <img id="delete" src="~/Images/close.png" runat="server" /></asp:LinkButton></span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div>
                                                <div>
                                                    <div class="alignright pagging" id="dvPaging" runat="server">
                                                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                                                        </asp:LinkButton>
                                                        <span>
                                                            <asp:DataList ID="dtlViewEmployee" runat="server" CellPadding="1" CellSpacing="1"
                                                                OnItemCommand="dtlViewEmployee_ItemCommand" OnItemDataBound="dtlViewEmployee_ItemDataBound"
                                                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                        </span>
                                                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                    <div>
                        <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                        <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="grey2_btn alignright" OnClick="lnkAddNew_Click"><span>+ Add</span></asp:LinkButton>
                        <at:ModalPopupExtender ID="modal" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
                            DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="closepopup">
                        </at:ModalPopupExtender>
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
                                                <div class="pp_fade" style="display: block;">
                                                    <div id="pp_full_res">
                                                        <div class="pp_inline clearfix">
                                                            <div class="form_popup_box">
                                                                <div class="label_bar">
                                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                                </div>
                                                                <div class="label_bar">
                                                                    <label>
                                                                        State :</label>
                                                                    <span>
                                                                        <asp:TextBox class="popup_input" ID="txtState" runat="server"></asp:TextBox></span>
                                                                </div>
                                                                <div class="label_bar btn_padinn">
                                                                    <asp:Button ID="btnSubmit" Text="Add City" runat="server" OnClick="btnSubmit_Click" />
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
                    <input type="hidden" id="hfStateID" value="" runat="server" />
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
