<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="HeadsetRepairDetails.aspx.cs" Inherits="HeadsetRepairCenter_HeadsetRepairDetails"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link media="screen" rel="stylesheet" href="../CSS/colorbox.css" />
    <link href="../CSS/red_colorbox.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .label-sel
        {
            width: 95%;
        }
        .dd2
        {
            width: 202px !important;
        }
        .dd2 .ddChild
        {
            width: 200px !important;
        }
        .order_detail td
        {
            font-size: 12px;
        }
        .order_detail label
        {
            padding-left: 0px;
        }
        .collapsibleContainerTitle div
        {
            color: #FFFFFF;
            font-weight: normal;
        }
        .collapsibleContainerSubInnerTitle
        {
            color: darkgray;
            font-weight: normal;
        }
        .collapsibleContainerContent
        {
            padding: 0px;
        }
        .form_box span.error
        {
            margin-left: 0px;
        }
        .form_table span.error
        {
            margin-top: -10px;
        }
        .collapsibleInnerContainer .collapsibleInnerContainer .collapsibleContainerInnerTitle div, .collapsibleSubInnerContainer, .collapsibleSubInnerContainer .collapsibleContainerSubInnerTitle div
        {
            font-size: 11px;
        }
        .collapsibleInnerContainer, .collapsibleSubInnerContainer, .collapsibleContainerSubInnerContent
        {
            margin: 10px 10px 0px 10px;
        }
        .textarea_box
        {
            width: 100%;
        }
        .popuplabel
        {
            color: #FFFFFF;
            font-size: 16px;
            margin: 5px;
            text-align: center;
        }
        .orderreturn_box img
        {
            height: 22px;
            margin-bottom: 4px;
        }
        .textarea_box textarea
        {
            font-size: 11px;
        }
        a.grey2_btn
        {
            font-size: 12px !important;
        }
        a.grey2_btn span
        {
            width: 83%;
            line-height: 38px;
        }
        a.grey2_btn img
        {
            border: medium none;
            float: left;
            height: 33px;
            margin: 0;
            padding: 0;
            vertical-align: middle;
            width: 47px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }

        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleInnerContainer").collapsibleInnerPanel();
            $(".collapsibleSubInnerContainer").collapsibleSubInnerPanel();
            $(".collapsibleContainerContent").hide();
            $(".collapsibleContainerInnerContent").hide();
            $(".collapsibleContainerSubInnerContent").hide();




            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                if ($("#ctl00_ContentPlaceHolder1_spnlogintype").html() == "CompanyEmployee") {
                    $("#aspnetForm").validate({
                        rules: {
                            ctl00$ContentPlaceHolder1$txtIEMessage: { required: true }
                        },
                        messages: {
                            ctl00$ContentPlaceHolder1$txtIEMessage: { required: replaceMessageString(objValMsg, "Required", "message") }
                        },
                        errorPlacement: function(error, element) {
                            error.insertAfter(element);
                        }
                    });
                }
                else {
                    $("#aspnetForm").validate({
                        rules: {
                            ctl00$ContentPlaceHolder1$txtMessage: { required: true },
                            ctl00$ContentPlaceHolder1$txtVendorMessage: { required: true },
                            ctl00$ContentPlaceHolder1$txtrepairquoteamount: { required: true, digits: true },
                            ctl00$ContentPlaceHolder1$txtestimatedleadtime: { required: true }
                        },
                        messages: {
                            ctl00$ContentPlaceHolder1$txtMessage: { required: replaceMessageString(objValMsg, "Required", "message") },
                            ctl00$ContentPlaceHolder1$txtVendorMessage: { required: replaceMessageString(objValMsg, "Required", "message") },
                            ctl00$ContentPlaceHolder1$txtrepairquoteamount: { required: replaceMessageString(objValMsg, "Required", "Repair Quote Amount"),
                                digits: replaceMessageString(objValMsg, "Digit", "")
                            },
                            ctl00$ContentPlaceHolder1$txtestimatedleadtime: { required: replaceMessageString(objValMsg, "Required", "Astimated Lead-Time") }
                        },
                        errorPlacement: function(error, element) {
                            error.insertAfter(element);
                        }
                    });
                }
            });

            $(".showloader").click(function() {
                $('#dvLoader').show();
            });

            $("#ctl00_ContentPlaceHolder1_lnkbtnSubmitNoteIE").click(function() {
                $('#dvLoader').show();
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });

            $("#ctl00_ContentPlaceHolder1_lnkbtnSubmitNote").click(function() {
                $('#dvLoader').show();

                addrules();
                $("#ctl00_ContentPlaceHolder1_txtVendorMessage").rules("remove");
                $("#ctl00_ContentPlaceHolder1_txtrepairquoteamount").rules("remove");
                $("#ctl00_ContentPlaceHolder1_txtestimatedleadtime").rules("remove");
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });

            $("#ctl00_ContentPlaceHolder1_lnkbtnVendorSubmitNote").click(function() {
                $('#dvLoader').show();

                addrules();

                $("#ctl00_ContentPlaceHolder1_txtMessage").rules("remove");
                $("#ctl00_ContentPlaceHolder1_txtrepairquoteamount").rules("remove");
                $("#ctl00_ContentPlaceHolder1_txtestimatedleadtime").rules("remove");
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });



            $("#ctl00_ContentPlaceHolder1_lnkbtnSubmitQuote").click(function() {
                $('#dvLoader').show();

                addrules();
                $("#ctl00_ContentPlaceHolder1_txtVendorMessage").rules("remove");
                $("#ctl00_ContentPlaceHolder1_txtMessage").rules("remove");
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });


            $("#ctl00_ContentPlaceHolder1_txtEnterOrderTrackingNumber").change(function() {
                //if (confirm('Are you sure you want to sent email')) {
                $('#dvLoader').show();
                $("#ctl00_ContentPlaceHolder1_btntrakingordernumber").click();
                //}
            });

            $("#ctl00_ContentPlaceHolder1_ddlStatus").change(function() {
                $('#dvLoader').show();
            });

            $("#ctl00_ContentPlaceHolder1_ddlVendor").change(function() {
                $('#dvLoader').show();
            });
        });
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
        $(function() {
            scrolltextarea(".scrollme", "#Scrolltop", "#ScrollBottom");
        });


        function addrules() {
            $("#ctl00_ContentPlaceHolder1_txtMessage").rules("add", {
                required: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "message") }
            });
            $("#ctl00_ContentPlaceHolder1_txtVendorMessage").rules("add", {
                required: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "message") }
            });
            $("#ctl00_ContentPlaceHolder1_txtrepairquoteamount").rules("add", {
                required: true,
                digits: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "Repair Quote Amount"),
                    digits: replaceMessageString(objValMsg, "Digit", "")
                }
            });
            $("#ctl00_ContentPlaceHolder1_txtestimatedleadtime").rules("add", {
                required: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "Astimated Lead-Time") }
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="black_top_co">
        <span>&nbsp;</span></div>
    <div class="black_middle" style="text-align: center;">
        <span id="spnlogintype" runat="server" style="display: none"></span>
        <table class="order_detail">
            <tr>
                <td width="50%">
                    <table>
                        <tr>
                            <td>
                                <asp:HiddenField ID="hdndeadsetRepairID" runat="server" />
                                <asp:HiddenField ID="hdnContactID" runat="server" />
                                <label>
                                    Customer Information :</label>
                                <asp:Label runat="server" ID="lblCustomerInformation"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Number of Sets :
                                </label>
                                <asp:Label runat="server" ID="lblNumberofSets"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Brand :
                                </label>
                                <asp:Label ID="lblBrand" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Repairing Supplier :
                                </label>
                                <asp:Label ID="lblrepairingsupplier" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="50%">
                    <table>
                        <tr>
                            <td>
                                <label>
                                    Repair Number :
                                </label>
                                <asp:Label ID="lblRepairNumber" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Date Submitted :
                                </label>
                                <asp:Label ID="lblDateSubmitted" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Request Repair Quote :
                                </label>
                                <asp:Label ID="lblRequestRepairQuote" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Status :</label>
                                <asp:DropDownList runat="server" ID="ddlStatus" Style="background-color: #303030;
                                    border: medium none; color: #ffffff; width: 150px; padding: 2px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%--   <asp:Label ID="lblstatus" runat="server"></asp:Label>
                               <span style="margin-left: 131px;">
                                    <asp:LinkButton ID="lnkBtnedit" class="gredient_btn" TabIndex="6" runat="server"
                                        ToolTip="Edit Repair" OnClick="btnedit_Click"><span>Edit</span></asp:LinkButton></span>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="spacer10">
        </div>
        <div class="collapsibleContainer" title="Customer Repair Communications" align="left">
            <div class="form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box taxt_area clearfix" style="height: 200px">
                    <div class="textarea_box alignright">
                        <div class="scrollbar" style="height: 200px">
                            <a href="#scroll" id="A5" class="scrolltop"></a><a href="#scroll" id="A6" class="scrollbottom">
                            </a>
                        </div>
                        <asp:TextBox ID="txtCustomerNotes" runat="server" TextMode="MultiLine" CssClass="scrollme"
                            Height="197px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="spacer10">
            </div>
            <%--Start Button for sending notes--%>
            <div class="form_table rightalign gallery" style="margin: 10px 10px 0; padding-right: 10px;
                height: 38px;">
                <asp:LinkButton ID="lnkbtnGeneralNotes" class="grey2_btn centeralign showloader"
                    runat="server" OnClick="lnkbtnGeneralNotes_Click" Width="143px">
                    
                 <img id="imgaddgenrealnote" src="../admin/Incentex_Used_Icons/Add_Notes.png"  />       
                 <span style="padding-right:20px;">Add Notes</span></asp:LinkButton>
            </div>
            <%--End Button for sending notes--%>
        </div>
        <div id="dvVendorspace" runat="server" class="spacer10">
        </div>
        <div id="dvVendorInformation" runat="server" class="collapsibleContainer" title="Supplier Communications"
            align="left">
            <div class="form_table">
                <table style="width: 100%; margin-top: 5px;">
                    <tr id="tr1" runat="server">
                        <td style="width: 50%">
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 40%;">Supplier </span><span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlVendor" onchange="pageLoad(this,value);" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlVendor_Selectedchanged" TabIndex="1">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvVendor">
                                    </div>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                        <td style="width: 50%; text-align: left; padding-left: 25px;">
                        </td>
                    </tr>
                </table>
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box taxt_area clearfix" style="height: 200px">
                    <div class="textarea_box alignright">
                        <div class="scrollbar" style="height: 200px">
                            <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                            </a>
                        </div>
                        <asp:TextBox ID="txtVendorNote" runat="server" TextMode="MultiLine" CssClass="scrollme"
                            Height="197px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="spacer10">
            </div>
            <%--Start Button for sending notes--%>
            <div class="form_table rightalign gallery" style="margin: 10px 10px 0; padding-right: 10px;
                height: 38px;">
                <asp:LinkButton ID="lnkbtnVendorNotes" class="grey2_btn centeralign showloader" runat="server"
                    OnClick="lnkbtnVendorNotes_Click" Width="143px">
                 <img id="imgaddnote" src="../admin/Incentex_Used_Icons/Add_Notes.png"  />    
                 <span style="padding-right:20px;">Add Notes</span></asp:LinkButton>
            </div>
            <%--End Button for sending notes--%>
        </div>
        <div class="spacer10">
        </div>
        <div class="collapsibleContainer" title="Headset Quote Information" align="left">
            <div class="form_table">
                <div class="spacer10">
                </div>
                <div id="dvHeadsetQuote" runat="server" style="height: 250px;">
                    <table style="width: 50%;">
                        <tr id="trQuote" runat="server">
                            <td>
                                <label style="vertical-align: middle;">
                                    Customer Approved Quote :
                                </label>
                                &nbsp;&nbsp;
                                <asp:Image ID="imgapprovedquote" runat="server" Width="22px" Height="22px" />
                                <%--<div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 35%;">Customer Approved Quote </span>
                                        <asp:Label ID="lblCustomerApprovedQuote" runat="server"></asp:Label>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>--%>
                            </td>
                        </tr>
                        <tr id="trRequestQuotebeforerepair" runat="server">
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 35%;">Request Quote Before Repair</span>
                                        <asp:Label ID="lblRequestquotebeforerepair" runat="server"></asp:Label>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                        <tr id="trRepairQuoteamount" runat="server">
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 35%;">Repair Quote Amount </span>
                                        <asp:TextBox ID="txtrepairquoteamount" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                        <tr id="trleadtime" runat="server">
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 35%;">Estimated Lead-Time </span>
                                        <asp:TextBox ID="txtestimatedleadtime" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                        <tr id="trsubmitquote" runat="server">
                            <td class="btn_w_block">
                                <asp:LinkButton ID="lnkbtnSubmitQuote" class="gredient_btn" TabIndex="6" runat="server"
                                    ToolTip="Submit Quote" OnClick="lnkbtnSubmitQuote_Click"><span>Submit Quote</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="spacer10">
        </div>
        <div class="collapsibleContainer" title="Repair Order Shipping Information" align="left">
            <div class="form_table">
                <%-- <div class="spacer10">
                </div>--%>
                <%-- <div>
                    <table style="width: 50%">
                        <tr id="trNumber" runat="server">
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 35%;">Enter Order Tracking Number </span>
                                        <asp:TextBox ID="txtEnterOrderTrackingNumber" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                                        <asp:Button ID="btntrakingordernumber" runat="server" OnClick="btntrakingordernumber_Click" />
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>--%>
                <div class="spacer15">
                </div>
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle order_detail_pad">
                    <div class="clearfix billing_head">
                        <div class="alignleft">
                            <span>Send Repaired Headset To: </span>
                        </div>
                        <div class="alignright">
                            <span style="padding-left: 22px!important;">Tracking Information: </span>
                        </div>
                    </div>
                    <div>
                        <div class="alignleft" style="width: 49%;">
                            <div class="tab_content_top_co">
                                <span>&nbsp;</span></div>
                            <div class="tab_content">
                                <table class="order_detail" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <label>
                                                Company :</label>
                                            <asp:Label ID="lblretCompany" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                Contact :</label>
                                            <asp:Label ID="lblretContact" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                Address1 :</label>
                                            <asp:Label ID="lblretAddress1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                Address2 :</label>
                                            <asp:Label ID="lblretAddress2" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                City :</label>
                                            <asp:Label ID="lblretCity" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                State :</label>
                                            <asp:Label ID="lblretState" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                Zip :</label>
                                            <asp:Label ID="lblretZip" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                Tel :</label>
                                            <asp:Label ID="lblretTel" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="tab_content_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        <div class="alignright" style="width: 49%;">
                            <div class="tab_content_top_co">
                                <span>&nbsp;</span></div>
                            <div class="tab_content">
                                <table class="order_detail" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-left: 2px;">
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label" style="width: 38%;">Inbound Tracking to Incentex </span>
                                                    <asp:TextBox ID="txtEnterOrderTrackingNumber" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                                                    <asp:Button ID="btntrakingordernumber" runat="server" OnClick="btntrakingordernumber_Click" />
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 2px;">
                                            <label style="width: 44%">
                                                Outbound Tracking to Supplier :</label>
                                            <asp:HyperLink ID="hypTrackingtosupplier" NavigateUrl="#" runat="server" Target="_blank" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 2px;">
                                            <label style="width: 44%">
                                                Inbound Tracking from Supplier :</label>
                                            <asp:HyperLink ID="hypTrackingfromsupplier" NavigateUrl="#" runat="server" Target="_blank" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 2px;">
                                            <label style="width: 44%">
                                                Outbound Tracking to Client :</label>
                                            <asp:HyperLink ID="hypTrackingtoclient" NavigateUrl="#" runat="server" Target="_blank" />
                                        </td>
                                    </tr>
                                    <tr class="centeralign">
                                        <td style="width: 100%; padding-left: 132; padding-top: 18px;">
                                            <asp:LinkButton ID="lnkBtnTrackingNumber" class="grey2_btn centeralign" runat="server"
                                                ToolTip="Save Tracking Number" OnClick="lnkBtnTrackingNumber_Click"><span>Add Tracking Number</span></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="tab_content_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        <div class="alignnone">
                        </div>
                    </div>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
                <div class="spacer15">
                </div>
            </div>
        </div>
    </div>
    <div class="black_bot_co">
        <span>&nbsp;</span></div>
    <div class="spacer15">
    </div>
    <asp:LinkButton ID="lnkDummyGeneralNote" class="grey2_btn alignright" runat="server"
        Style="display: none"></asp:LinkButton>
    <at:ModalPopupExtender ID="modal" TargetControlID="lnkDummyGeneralNote" BackgroundCssClass="modalBackground"
        DropShadow="true" runat="server" PopupControlID="pnlGeneralNote" CancelControlID="cboxClose">
    </at:ModalPopupExtender>
    <asp:Panel ID="pnlGeneralNote" runat="server" Style="display: none;">
        <div class="credboxWrapper" style="display: block; width: 450px; position: fixed;
            left: 35%; top: 25%;">
            <div style="">
                <div id="credboxTopLeft" style="float: left;">
                </div>
                <div id="credboxTopCenter" style="float: left; width: 400px;">
                </div>
                <div id="credboxTopRight" style="float: left;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="credboxMiddleLeft" style="float: left; height: 450px;">
                </div>
                <div id="credboxContent" style="float: left; display: block; height: 450px; width: 400px;">
                    <div id="credboxLoadedContent" style="display: block; margin: 0;">
                        <div id="credboxClose">
                            close</div>
                        <div style="margin-top: 30px;">
                            <table class="form_table" cellpadding="0" cellspacing="0">
                                <tr runat="server" id="tr2" class="checktable_supplier true">
                                    <td>
                                        <span style="font-weight: bold; color: #72757C;">Incentex Employees</span>
                                        <div class="spacer5">
                                        </div>
                                        <div style="overflow: auto; height: 84px;">
                                            <asp:DataList ID="dtlIE" runat="server" RepeatColumns="3" RepeatDirection="Vertical"
                                                OnItemDataBound="dtIE_ItemDataBound">
                                                <ItemTemplate>
                                                    <span class="custom-checkbox redbox alignleft" id="spnUser" runat="server">
                                                        <asp:CheckBox ID="chkUser" runat="server" Checked='<%# Eval("IsChecked") %>' />
                                                    </span>
                                                    <label>
                                                        <asp:Label ID="lblUserName" Text='<%# Eval("EmployeeName") %>' runat="server"></asp:Label></label>
                                                    <asp:HiddenField ID="hdnUserID" runat="server" Value='<%#Eval("UserInfoID")%>' />
                                                    <asp:HiddenField ID="hdnEmail" runat="server" Value='<%#Eval("Email")%>' />
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="red_form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="red_form_box taxt_area clearfix" style="height: 150px">
                                            <span class="input_label alignleft" style="height: 150px; line-height: 130px;">Message</span>
                                            <div class="textarea_box alignright" style="width: 78%;">
                                                <div class="red_scrollbar" style="height: 150px">
                                                    <a href="#scroll" id="scrolltop" class="red_scrolltop"></a><a href="#scroll" id="scrollbottom"
                                                        class="red_scrollbottom"></a>
                                                </div>
                                                <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" CssClass="scrollme"
                                                    Height="147px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="red_form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </td>
                                </tr>
                                <tr runat="server" id="trIsEmailSend" class="checktable_supplier true">
                                    <td>
                                        <span class="custom-checkbox redbox" id="spnIsEmailSend" runat="server" style="float: left;">
                                            <asp:CheckBox ID="chkIsEmailSend" runat="server" />
                                        </span>
                                        <label>
                                            <asp:Label ID="lblIsEmailSend" Text="Send Email" runat="server"></asp:Label></label>
                                    </td>
                                </tr>
                            </table>
                            <div class="centeralign">
                                <asp:LinkButton ID="lnkbtnSubmitNote" class="red2_btn" runat="server" OnClick="btnSubmitNote_Click"><span>Submit</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="credboxMiddleRight" style="float: left; height: 450px;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="credboxBottomLeft" style="float: left;">
                </div>
                <div id="credboxBottomCenter" style="float: left; width: 400px;">
                </div>
                <div id="credboxBottomRight" style="float: left;">
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlGeneralIENote" runat="server" Style="display: none;">
        <div class="cboxWrapper" style="display: block; width: 450px; position: fixed; left: 35%;
            top: 25%;">
            <div style="">
                <div id="cboxTopLeft" style="float: left;">
                </div>
                <div id="cboxTopCenter" style="float: left; width: 400px;">
                </div>
                <div id="cboxTopRight" style="float: left;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxMiddleLeft" style="float: left; height: 470px;">
                </div>
                <div id="cboxContent" style="float: left; display: block; height: 470px; width: 400px;">
                    <div id="cboxLoadedContent" style="display: block; margin: 0;">
                        <div id="cboxpasswordClose">
                            close</div>
                        <div style="margin-top: 30px;">
                            <table class="form_table" cellpadding="0" cellspacing="0">
                                <tr runat="server" id="trIE" class="checktable_supplier true">
                                    <td>
                                       <span style="font-weight: bold; color: #72757C;">Incentex Employees</span>
                                        <div class="spacer5">
                                        </div>
                                        <div style="overflow: auto; height: 84px;">
                                            <asp:DataList ID="dtIE" runat="server" RepeatColumns="3" RepeatDirection="Vertical"
                                                OnItemDataBound="dtIE_ItemDataBound">
                                                <ItemTemplate>
                                                    <span class="custom-checkbox alignleft" id="spnUser" runat="server">
                                                        <asp:CheckBox ID="chkUser" runat="server" Checked='<%# Eval("IsChecked") %>' />
                                                    </span>
                                                    <label>
                                                        <asp:Label ID="lblUserName" Text='<%# Eval("EmployeeName") %>' runat="server"></asp:Label></label>
                                                    <asp:HiddenField ID="hdnUserID" runat="server" Value='<%#Eval("UserInfoID")%>' />
                                                    <asp:HiddenField ID="hdnEmail" runat="server" Value='<%#Eval("Email")%>' />
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box taxt_area clearfix" style="height: 150px">
                                            <span class="input_label alignleft" style="height: 150px; line-height: 130px;">Message</span>
                                            <div class="textarea_box alignright" style="width: 78%;">
                                                <div class="scrollbar" style="height: 150px">
                                                    <a href="#scroll" id="scrolltop" class="scrolltop"></a><a href="#scroll" id="scrollbottom"
                                                        class="scrollbottom"></a>
                                                </div>
                                                <asp:TextBox ID="txtIEMessage" runat="server" TextMode="MultiLine" CssClass="scrollme"
                                                    Height="147px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </td>
                                </tr>
                                <tr runat="server" id="trIsEmailSendIE" class="checktable_supplier true">
                                    <td>
                                        <span class="custom-checkbox" id="spnIsEmailSendIE" runat="server" style="float: left;">
                                            <asp:CheckBox ID="chkIsEmailSendIE" runat="server" />
                                        </span>
                                        <label>
                                            <asp:Label ID="lblIsEmailSendIE" Text="Send Email" runat="server"></asp:Label></label>
                                    </td>
                                </tr>
                            </table>
                            <div class="centeralign">
                                <asp:LinkButton ID="lnkbtnSubmitNoteIE" class="grey2_btn" runat="server" OnClick="btnSubmitNote_Click"><span>Submit</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="cboxMiddleRight" style="float: left; height: 470px;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxBottomLeft" style="float: left;">
                </div>
                <div id="cboxBottomCenter" style="float: left; width: 400px;">
                </div>
                <div id="cboxBottomRight" style="float: left;">
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlVendorNote" runat="server" Style="display: none;">
        <div class="cboxWrapper" style="display: block; width: 450px; position: fixed; left: 35%;
            top: 25%;">
            <div style="">
                <div id="cboxTopLeft" style="float: left;">
                </div>
                <div id="cboxTopCenter" style="float: left; width: 400px;">
                </div>
                <div id="cboxTopRight" style="float: left;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxMiddleLeft" style="float: left; height: 350px;">
                </div>
                <div id="cboxContent" style="float: left; display: block; height: 350px; width: 400px;">
                    <div id="cboxLoadedContent" style="display: block; margin: 0;">
                        <div id="cboxVClose" class="cboxClose">
                            close</div>
                        <div style="margin-top: 30px;">
                            <table class="form_table" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box taxt_area clearfix" style="height: 150px">
                                            <span class="input_label alignleft" style="height: 150px; line-height: 130px;">Message</span>
                                            <div class="textarea_box alignright" style="width: 78%;">
                                                <div class="scrollbar" style="height: 150px">
                                                    <a href="#scroll" id="A3" class="scrolltop"></a><a href="#scroll" id="A4" class="scrollbottom">
                                                    </a>
                                                </div>
                                                <asp:TextBox ID="txtVendorMessage" runat="server" TextMode="MultiLine" CssClass="scrollme"
                                                    Height="147px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </td>
                                </tr>
                                <tr runat="server" id="trVendorIsEmailSend" class="checktable_supplier true">
                                    <td>
                                        <span class="custom-checkbox" id="spnVendorIsEmailSend" runat="server" style="float: left;">
                                            <asp:CheckBox ID="chkVendorIsEmailSend" runat="server" />
                                        </span>
                                        <label>
                                            <asp:Label ID="lblVendorIsEmailSend" Text="Send Email" runat="server"></asp:Label></label>
                                    </td>
                                </tr>
                            </table>
                            <div class="centeralign">
                                <asp:LinkButton ID="lnkbtnVendorSubmitNote" class="grey2_btn" runat="server" OnClick="lnkbtnVendorNotesSubmit_Click"><span>Submit</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="cboxMiddleRight" style="float: left; height: 350px;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxBottomLeft" style="float: left;">
                </div>
                <div id="cboxBottomCenter" style="float: left; width: 400px;">
                </div>
                <div id="cboxBottomRight" style="float: left;">
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlTrackingNumber" runat="server" Style="display: none; overflow-y: auto;">
        <div class="cboxWrapper" style="display: block; width: 650px; position: fixed; left: 35%;
            top: 25%;">
            <div style="">
                <div id="cboxTopLeft" style="float: left;">
                </div>
                <div id="cboxTopCenter" style="float: left; width: 575px;">
                </div>
                <div id="cboxTopRight" style="float: left;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxMiddleLeft" style="float: left; height: 300px;">
                </div>
                <div id="cboxContent" style="float: left; display: block; height: 300px; width: 575px;">
                    <div id="cboxLoadedContent" style="display: block; margin: 0;">
                        <div id="cboxClose">
                            close</div>
                        <div style="margin-top: 30px; height: 300px; overflow-y: auto; padding-right: 5px;">
                            <table class="form_table scrollme" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div style="float: left; width: 69%;">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">Outbound Tracking to Supplier </span>
                                                <asp:TextBox ID="txttrackingtosupplier" runat="server" CssClass="w_label"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                        <div style="float: right; width: 30%; margin-top: 7px;">
                                            <div class="checkout_checkbox alignlef" runat="server" id="spnUPStosupplier" style="width: 20px;
                                                height: 25px; margin-left: 0px;">
                                                <asp:CheckBox ID="chkUPStosupplier" runat="server" />
                                            </div>
                                            <span style="float: left; margin-top: -19px; margin-left: 41px; position: absolute;
                                                color: #72757C; line-height: 18px;">UPS</span>
                                            <div class="checkout_checkbox alignlef" runat="server" id="spnFedextosupplier" style="width: 20px;
                                                height: 25px; margin-top: -24px; margin-left: 76px;">
                                                <asp:CheckBox ID="chkFedextosupplier" runat="server" />
                                            </div>
                                            <span style="float: left; margin-top: -19px; margin-left: 117px; position: absolute;
                                                color: #72757C; line-height: 18px;">Fedex</span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="float: left; width: 69%;">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">Inbound Tracking from Supplier </span>
                                                <asp:TextBox ID="txttrackingfromsupplier" runat="server" CssClass="w_label"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                        <div style="float: right; width: 30%; margin-top: 7px;">
                                            <div class="checkout_checkbox alignlef" runat="server" id="spnUPSfromsupplier" style="width: 20px;
                                                height: 25px; margin-left: 0px;">
                                                <asp:CheckBox ID="chkUPSfromsupplier" runat="server" />
                                            </div>
                                            <span style="float: left; margin-top: -19px; margin-left: 41px; position: absolute;
                                                color: #72757C; line-height: 18px;">UPS</span>
                                            <div class="checkout_checkbox alignlef" runat="server" id="spnFedexfromsupplier"
                                                style="width: 20px; height: 25px; margin-top: -24px; margin-left: 76px;">
                                                <asp:CheckBox ID="chkFedexfromsupplier" runat="server" />
                                            </div>
                                            <span style="float: left; margin-top: -19px; margin-left: 117px; position: absolute;
                                                color: #72757C; line-height: 18px;">Fedex</span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="float: left; width: 69%;">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">Outbound Tracking to Client </span>
                                                <asp:TextBox ID="txttrackingtoclient" runat="server" CssClass="w_label"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                        <div style="float: right; width: 30%; margin-top: 7px;">
                                            <div class="checkout_checkbox alignlef" runat="server" id="spnUPStoclient" style="width: 20px;
                                                height: 25px; margin-left: 0px;">
                                                <asp:CheckBox ID="chkUPStoclient" runat="server" />
                                            </div>
                                            <span style="float: left; margin-top: -19px; margin-left: 41px; position: absolute;
                                                color: #72757C; line-height: 18px;">UPS</span>
                                            <div class="checkout_checkbox alignlef" runat="server" id="spnFedextoclient" style="width: 20px;
                                                height: 25px; margin-top: -24px; margin-left: 76px;">
                                                <asp:CheckBox ID="chkFedextoclient" runat="server" />
                                            </div>
                                            <span style="float: left; margin-top: -19px; margin-left: 117px; position: absolute;
                                                color: #72757C; line-height: 18px;">Fedex</span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="centeralign" style="padding-top: 15px">
                                <asp:LinkButton ID="lnkbtntrackinginformation" class="gredient_btn" TabIndex="6"
                                    runat="server" ToolTip="Submit Teacking Information" OnClick="lnkbtntrackinginformation_Click"><span>Submit</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="cboxMiddleRight" style="float: left; height: 300px;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxBottomLeft" style="float: left;">
                </div>
                <div id="cboxBottomCenter" style="float: left; width: 575px;">
                </div>
                <div id="cboxBottomRight" style="float: left;">
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
