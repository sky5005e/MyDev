<%@ Page Title="Support Ticket - Activity Report" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="ActivityReport.aspx.cs" Inherits="admin_ServiceTicketCenter_ActivityReport"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(window).load(function() {
            $("#dvLoader").hide();            
        });
    </script>

    <script type="text/javascript" language="javascript">        
        $().ready(function() {
            $("#dvLoader").show();
            
            $("#ctl00_ContentPlaceHolder1_closeIEPopup").click(function() {            
                $("#ctl00_ContentPlaceHolder1_pnlNotesIE").hide();
            });
            
             $("#ctl00_ContentPlaceHolder1_closeOrderAssociatePopup").click(function() {            
                $("#ctl00_ContentPlaceHolder1_pnlOrderAssociate").hide();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkNoteIE").click(function() {
                $("#dvLoader").show();
            });
            
            $(window).scroll(function () {              
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
            
//            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
//                objValMsg = $.xml2json(xml);

//                $("#aspnetForm").validate({
//                    rules: {
//                        ctl00$ContentPlaceHolder1$txtMessage: { required: true }
//                    },
//                    messages: {
//                        ctl00$ContentPlaceHolder1$txtMessage: { required: replaceMessageString(objValMsg, "Required", "message") }
//                    },
//                    errorPlacement: function(error, element) {
//                        error.insertAfter(element);
//                    }
//                });
//            });
            
        });
        
        function NotePopup(id) {
            $("#NotePopupTitle").html('Post an internal note for ticket no.: ' + id + '.');
            $("#ctl00_ContentPlaceHolder1_hdnPopupTicketID").val(id);
            $("#ctl00_ContentPlaceHolder1_pnlNotesIE").show();
            return false;
        }
        function AssociatePopup(id) {
            $("#b_title").html(id);
            $("#ctl00_ContentPlaceHolder1_hdnAssociateTicketID").val(id);
            $("#ctl00_ContentPlaceHolder1_pnlOrderAssociate").show();
            return false;
        }
    </script>

    <style type="text/css">
        .order_detail td
        {
            font-size: 15px;
            color: #72757c;
            line-height: 20px;
            padding-bottom: 0px !important;
            text-align: left !important;
        }
        .order_detail label
        {
            color: #B0B0B0;
            padding-right: 6px;
            padding-left: 20px;
        }
        .st_detail td
        {
            font-size: 15px;
            color: #72757c;
        }
        .st_detail label
        {
            color: #B0B0B0;
        }
        .clearfix
        {
            line-height: 8px;
        }
        .fancybox-overlay
        {
            left: 0;
            position: fixed;
            top: 0;
            width: 100%;
            height: 100%;
            z-index: 1100;
            opacity: 0.7;
            display: block;
            background-color: #777777;
        }
    </style>
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad shipping_method_pad" style="width: 900px;">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 50%;">
                    <div style="text-align: left; color: #5C5B60; font-size: larger; font-weight: bold;
                        padding-left: 15px;">
                        List of Support Tickets With Recent Activities
                    </div>
                </td>
                <td style="width: 50%;">
                    <div style="text-align: right; color: #5C5B60; font-size: larger; font-weight: bold;
                        padding-left: 15px;">
                        Total Records :
                        <asp:Label ID="lblRecords" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:Label ID="lblMsgList" runat="server" CssClass="errormessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="spacer10">
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
                        <asp:DataList ID="dlTicketActivities" runat="server" OnItemDataBound="dlTicketActivities_ItemDataBound"
                            OnItemCommand="dlTicketActivities_ItemCommand">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnTicketID" runat="server" Value='<%# Eval("ServiceTicketID") %>' />
                                <div class="spacer20">
                                </div>
                                <div class="pro_search_pad" style="width: 900px; clear: both;">
                                    <div class="black_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="black_middle order_detail_pad">
                                        <table class="order_detail" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 50%">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Ticket Number :
                                                                </label>
                                                                <asp:LinkButton ID="lblServiceTicketNumber" Text='<%# Eval("ServiceTicketNumber") %>'
                                                                    runat="server" Style="color: #72757C;" CommandArgument='<%# Eval("ServiceTicketID") %>'
                                                                    CommandName="TicketDetail"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Ticket Name :
                                                                </label>
                                                                <asp:Label ID="lblServiceTicketName" Text='<%# Eval("ServiceTicketName") %>' runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Ticket Status :
                                                                </label>
                                                                <asp:Label ID="lblServiceTicketStatus" Text='<%# Eval("TicketStatus") %>' runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 50%">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <label style="padding-left: 28px!important;">
                                                                    Ticket Owner :
                                                                </label>
                                                                <asp:Label ID="lblServiceTicketOwner" Text='<%# Eval("FirstName") + " " + Eval("LastName") %>'
                                                                    runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label style="padding-left: 28px!important;">
                                                                    Due Date :
                                                                </label>
                                                                <asp:Label runat="server" ID="lblDatePromised" Text='<%# Eval("DatePromisedFormatted") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="divAssociateOrder" runat="server" visible="false">
                                                                    <label style="padding-left: 28px!important;">
                                                                        Associate OrderID :
                                                                    </label>
                                                                    <asp:Label runat="server" ID="lblAssociateOrder"></asp:Label>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="black_bot_co">
                                        <span>&nbsp;</span></div>
                                    <%--<div class="spacer5">
                                    </div>--%>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <asp:DataList ID="dlNotes" runat="server" CssClass="form_box">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnNoteID" runat="server" Value='<%# Eval("NoteID") %>' />
                                            <table class="order_detail" style="padding: 0px 32px 0px 32px;" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNote" runat="server" Text='<%# Eval("Notecontents") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="border-bottom: solid 1px Gray;">
                                                        <asp:Label ID="lblNoteAuthor" runat="server" Text='<%# Convert.ToString(Convert.ToString(Eval("FirstName")) + Convert.ToString(Eval("LastName"))) != "" ? Eval("FirstName") + " " + Eval("LastName") + " " + Eval("DateNTime") : Eval("TicketEmail") + " " + Eval("DateNTime") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="spacer10">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                    <div class="spacer10">
                                    </div>
                                    <div class="form_table centeralign" style="margin: 10px 10px 0; padding-right: 10px">
                                        <asp:LinkButton ID="lnkAssignSubOwners" runat="server" CssClass="grey2_btn alignleft"
                                            CommandName="SubOwners" CommandArgument='<%# Eval("ServiceTicketID") %>'>
                                        <span>Assign Sub Owners</span></asp:LinkButton>
                                        <asp:LinkButton ID="lnkbtnAssociateWithOrder" runat="server" CssClass="grey2_btn "
                                            CommandName="AssociateOrder" CommandArgument='<%# Eval("ServiceTicketID") %>'>
                                            <span id="spanAssociateWithOrder" runat="server">Associate with Order</span></asp:LinkButton>
                                        <asp:LinkButton ID="lnkAddNewID" runat="server" CssClass="grey2_btn alignright" OnClientClick="javascript:return false;">
                                            <span id="spanAddNote" runat="server">+ Add Internal Company Note</span></asp:LinkButton>
                                    </div>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:Panel ID="pnlNotesIE" runat="server" Style="display: none;">
            <div class="fancybox-overlay">
            </div>
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
                                    <div id="pp_full_res">
                                        <div class="pp_inline clearfix">
                                            <div class="form_popup_box">
                                                <div class="label_bar">
                                                    <asp:HiddenField ID="hdnPopupTicketID" runat="server" />
                                                    <span style="color: Black;"><b id="NotePopupTitle">Post an internal note for ticket
                                                        no.:</b>
                                                        <br />
                                                        <br />
                                                        <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNoteIE" runat="server"></asp:TextBox></span>
                                                </div>
                                                <div>
                                                    <asp:LinkButton ID="lnkNoteIE" class="grey2_btn alignright" runat="server" OnClick="lnkNoteIE_Click"><span>Save Notes</span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="pp_details clearfix" style="width: 371px;">
                                        <span id="closeIEPopup" runat="server" class="pp_close"></span>
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
    <div>
        <asp:Panel ID="pnlOrderAssociate" runat="server" Style="display: none;">
            <div class="fancybox-overlay">
            </div>
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
                                    <%--<a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                    <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                            style="visibility: visible;">previous</a>
                                    </div>--%>
                                    <div id="pp_full_res">
                                        <div class="pp_inline clearfix">
                                            <div class="form_popup_box">
                                                <div class="label_bar centeralign">
                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                </div>
                                                <div>
                                                    <div class="label_bar centeralign">
                                                        <label style="width:158px;">
                                                           <b>Ticket :</b>
                                                           </label>   
                                                        <label style="text-align:left;margin-left:0px;width:180px;">
                                                           <b id="b_title"> </b>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div>
                                                    <div class="label_bar">
                                                        <label style="width:158px;">
                                                            <b>Select Order to Associate : </b>
                                                        </label>
                                                        <span>
                                                            <asp:HiddenField ID="hdnAssociateTicketID" runat="server" />
                                                            <asp:DropDownList ID="ddlUnAssociateOrder" Width="180px" runat="server">
                                                            </asp:DropDownList>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div style="width:264px;">
                                                    <asp:LinkButton ID="lnkbtnAssociateOrder" class="grey2_btn alignright" runat="server"
                                                        OnClick="lnkbtnAssociateOrder_Click"><span>Associate</span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="pp_details clearfix" style="width: 371px;">
                                        <span id="closeOrderAssociatePopup" runat="server" class="pp_close"></span>
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
