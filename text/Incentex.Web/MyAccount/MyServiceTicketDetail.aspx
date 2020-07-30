<%@ Page Title="My Support Ticket Detail" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="MyServiceTicketDetail.aspx.cs" Inherits="MyAccount_MyServiceTicketDetail"
    MaintainScrollPositionOnPostback="true" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .textarea_box textarea
        {
            height: 96px;
        }
        .textarea_box
        {
            height: 96px;
        }
        .textarea_box .scrollbar
        {
            height: 96px;
        }
        .fontsizesmall
        {
            font-size: small;
        }
        .noteIncentex
        {
            background-color: #E1E0E0;
            color: black;
            font-family: "Trebuchet MS" ,tahoma,arial,verdana;
            font-size: 0.8em;
            padding-bottom: 5px;
            padding-top: 7px;
        }
        .shipmentclass
        {
            background: none repeat scroll 0 0 #101010;
            color: #72757C;
            display: block;
            border: solid 1px #1c1c1c;
            padding: 0px 8px;
            border-left: none;
            border-right: solid 1px #1f1f1f;
        }
        .rightalign
        {
            font-size: small;
            color: #72757C;
            text-align: right;
        }
        .width300
        {
            font-size: small;
            width: 300px !important;
        }
        .form_table .calender_l .ui-datepicker-trigger
        {
            top: -2px;
        }
        .form_table input
        {
            font-size: 15px;
            height: 12px;
            color: #72757C;
        }
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
        .form_pad
        {
            min-height: 10px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript">
        $(window).load(function() {
            $("#dvLoader").hide();
        });
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $("#dvLoader").show();
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$fpAttachment: { required: false, accept: formats }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$fpAttachment: { accept: "File format not supported." }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$fpAttachment")
                            error.insertAfter("#dvAttachment");
                        else
                            error.insertAfter(element);
                    }
                });
            });

            $("#ctl00_ContentPlaceHolder1_btnAddItem").click(function() {
                var fileinput = document.getElementById('ctl00_ContentPlaceHolder1_fpAttachment');
                if (!fileinput.files[0]) {
                    alert("Please select file.");
                    return false;
                }
                $('#dvLoader').show();
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });

            $('#ctl00_ContentPlaceHolder1_txtNote').live('keydown', function(e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode == 9) {
                    e.preventDefault();
                    $('#ctl00_ContentPlaceHolder1_lnkButton').focus();
                }
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkButton").click(function() {
                $('#dvLoader').show();
            });
            
            $(window).scroll(function () {
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());            
        });        
    </script>

    <script type="text/javascript" language="javascript">
        $(function() {
            scrolltextarea(".scrollme", "#ScrollTop", "#ScrollBottom");
        });
    </script>

    <link media="screen" rel="stylesheet" href="../CSS/colorbox.css" />
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
        <div class="pro_search_pad" style="width: 800px;">
            <div id="mainDIV">
                <div>
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
                                                <asp:Label ID="lblServiceTicketNumber" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Ticket Name :
                                                </label>
                                                <asp:Label ID="lblServiceTicketName" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Ticket Status :
                                                </label>
                                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 50%">
                                    <table>                                        
                                        <tr>
                                            <td>
                                                <label>
                                                    Start Date :
                                                </label>
                                                <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Due Date :
                                                </label>
                                                <asp:Label ID="lblEndDate" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trContact" runat="server">
                                            <td>
                                                <label id="lblReason" runat="server">
                                                    Contact :
                                                </label>
                                                <asp:Label ID="lblContact" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="black_bot_co">
                        <span>&nbsp;</span></div>
                </div>
            </div>
        </div>
    </div>
    <div class="pro_search_pad" style="width: 800px;">
        <div class="form_table">
            <div class="form_top_co">
                <span>&nbsp;</span></div>
            <div class="form_box taxt_area clearfix" style="height: 280px;">
                <span class="input_label" style="height: 180px; font-size: 13px; width: 20%!important;
                    padding-top: 100px;">Notes/History :</span>
                <div class="textarea_box alignright" style="width: 78%;">
                    <div class="scrollbar" style="height: 283px">
                        <a href="#scroll" id="ScrollTop" class="scrolltop"></a><a href="#scroll" id="ScrollBottom"
                            class="scrollbottom"></a>
                    </div>
                    <asp:TextBox ID="txtOrderNotesForCECA" runat="server" TextMode="MultiLine" CssClass="scrollme"
                        Height="280px" Style="font-size: 13px; color: #B0B0B0;" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="form_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div class="alignnone spacer15">
        </div>
        <div class="rightalign gallery" id="divAddNots" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkDummyAttachments" class="grey2_btn alignleft" runat="server"
                            Style="display: none"></asp:LinkButton>
                        <asp:LinkButton ID="lnkAttachments" CommandName="Attachment" runat="server" CssClass="grey2_btn alignleft">
                            <span id="atSpan" runat="server">Attachments</span></asp:LinkButton>
                        <at:ModalPopupExtender ID="modalAttachments" TargetControlID="lnkAttachments" BackgroundCssClass="modalBackground"
                            DropShadow="true" runat="server" PopupControlID="pnlAttachments" CancelControlID="closeattchement">
                        </at:ModalPopupExtender>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                        <asp:LinkButton ID="lnkAddNew" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright"><span>+ Add Note</span></asp:LinkButton>
                        <at:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkAddNew" BackgroundCssClass="modalBackground"
                            DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup">
                        </at:ModalPopupExtender>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:Panel ID="pnlNotes" runat="server" Style="display: none;">
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
                                                        <span>Notes/History :
                                                            <br />
                                                            <br />
                                                            <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNote" runat="server"></asp:TextBox></span>
                                                    </div>
                                                    <div>
                                                        <asp:LinkButton ID="lnkButton" class="grey2_btn alignright" runat="server" OnClick="lnkButton_Click"><span>Save Notes</span></asp:LinkButton>
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
        <div>
            <asp:Panel ID="pnlAttachments" runat="server" Style="display: none;">
                <div class="cboxWrapper" style="display: block; width: 558px; height: 549px; position: fixed;
                    left: 30%; top: 8%;">
                    <div style="">
                        <div class="cboxTopLeft" style="float: left;">
                        </div>
                        <div class="cboxTopCenter" style="float: left; width: 508px;">
                        </div>
                        <div class="cboxTopRight" style="float: left;">
                        </div>
                    </div>
                    <div style="clear: left;">
                        <div class="cboxMiddleLeft" style="float: left; height: 483px;">
                        </div>
                        <div class="cboxContent" style="float: left; width: 508px; display: block; height: 483px;">
                            <div class="cboxLoadedContent" style="display: block; overflow: visible;">
                                <div style="padding: 25px 10px 10px 10px;">
                                    <div style="height: 383px; overflow: auto;">
                                        <div style="text-align: center; color: Red; font-size: 14px;">
                                            <asp:Label ID="lblAttachmentMsg" runat="server">
                                            </asp:Label>
                                        </div>
                                        <asp:GridView ID="grvAttachment" runat="server" Width="100%" HeaderStyle-CssClass="ord_header"
                                            CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                                            OnRowCommand="grvAttachment_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>File Name </span>
                                                        <div class="corner">
                                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <span class="first">
                                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="" CommandName="download"><%#Eval("OnlyFileName")%></asp:LinkButton>
                                                        </span>
                                                        <div class="corner">
                                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="g_box" Width="85%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Delete</span>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkbtndelete" CommandName="DeleteAttachment" OnClientClick="return confirm('Are you sure, you want to delete attachment?');"
                                                            CommandArgument='<%#Eval("OnlyFileName")%>' runat="server">
                                                            <span class="btn_space">
                                                                <img alt="X" id="delete" src="~/Images/close.png" runat="server" /></span></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="spacer10">
                                    </div>
                                    <div class="form_top_co" style="clear: both;">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <input id="fpAttachment" name="fpAttachment" type="file" runat="server" style="float: left;" />
                                        <span style="float: left;">
                                            <asp:LinkButton ID="btnAddItem" Text="Add File" CssClass="greysm_btn" runat="server"
                                                OnClick="btnAddItem_Click"><span>Add File</span></asp:LinkButton>
                                        </span>
                                        <br />
                                        <div id="dvAttachment">
                                        </div>
                                        <br />
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                <div class="cboxLoadingOverlay" style="height: 483px; display: none;" id="attachementcboxLoadingOverlay">
                                </div>
                                <div class="cboxLoadingGraphic" style="height: 483px; display: none;" id="attachementcboxLoadingGraphic">
                                </div>
                                <div class="cboxTitle" style="display: block;" id="attachementtitle">
                                </div>
                            </div>
                            <div class="cboxClose" style="" id="closeattchement">
                                close</div>
                        </div>
                        <div class="cboxMiddleRight" style="float: left; height: 483px;">
                        </div>
                    </div>
                    <div style="clear: left;">
                        <div class="cboxBottomLeft" style="float: left;">
                        </div>
                        <div class="cboxBottomCenter" style="float: left; width: 508px;">
                        </div>
                        <div class="cboxBottomRight" style="float: left;">
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <br />
        <br />
        <br />
    </div>
</asp:Content>
