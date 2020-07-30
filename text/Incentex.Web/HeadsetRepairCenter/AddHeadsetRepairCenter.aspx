<%@ Page Title="Headset Repair Center" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="AddHeadsetRepairCenter.aspx.cs" Inherits="HeadsetRepairCenter_AddHeadsetRepairCenter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link media="screen" rel="stylesheet" href="../CSS/colorbox.css" />
    <style type="text/css">
        .input_txtarea
        {
            width: 40% !important;
            height: 75px !important;
            line-height: 70px !important;
            border-right: 1px solid #1B1B1B;
            color: #F4F4F4 !important;
            display: inline-block;
            font-size: 11px;
            line-height: 34px;
            margin-right: 3px !important;
            padding-right: 10px !important;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        var formats = 'pdf|jpg|jpeg|xls|xlsx|doc';
        $().ready(function() {
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlContact: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlHeadsetBrand: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlTotalHeadsetstoRepair: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlRequestQuote: { NotequalTo: "0" },

                        //After Repair Return Headset Address Popup
                        ctl00$ContentPlaceHolder1$txtretAddressName: { required: true },
                        ctl00$ContentPlaceHolder1$txtretCompany: { required: true },
                        ctl00$ContentPlaceHolder1$txtretContact: { required: true },
                        ctl00$ContentPlaceHolder1$txtretaddress1: { required: true },
                        ctl00$ContentPlaceHolder1$txtretaddress2: { required: true },
                        ctl00$ContentPlaceHolder1$ddlretCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlretState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlretCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtretzip: { required: true },
                        ctl00$ContentPlaceHolder1$txtrettelephone: { required: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "company") },
                        ctl00$ContentPlaceHolder1$ddlContact: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "contact") },
                        ctl00$ContentPlaceHolder1$ddlHeadsetBrand: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "headsetBrand") },
                        ctl00$ContentPlaceHolder1$ddlTotalHeadsetstoRepair: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "totalheadsetstorepair") },
                        ctl00$ContentPlaceHolder1$ddlRequestQuote: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "requestquote") },

                        //After Repair Return Headset Address Popup
                        ctl00$ContentPlaceHolder1$txtretAddressName: { required: replaceMessageString(objValMsg, "Required", "address name") },
                        ctl00$ContentPlaceHolder1$txtretCompany: { required: replaceMessageString(objValMsg, "Required", "company") },
                        ctl00$ContentPlaceHolder1$txtretContact: { required: replaceMessageString(objValMsg, "Required", "contact") },
                        ctl00$ContentPlaceHolder1$txtretaddress1: { required: replaceMessageString(objValMsg, "Required", "address1") },
                        ctl00$ContentPlaceHolder1$txtretaddress2: { required: replaceMessageString(objValMsg, "Required", "address2") },
                        ctl00$ContentPlaceHolder1$ddlretCountry: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "country") },
                        ctl00$ContentPlaceHolder1$ddlretState: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "state") },
                        ctl00$ContentPlaceHolder1$ddlretCity: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "city") },
                        ctl00$ContentPlaceHolder1$txtretzip: { required: replaceMessageString(objValMsg, "Required", "zip") },
                        ctl00$ContentPlaceHolder1$txtrettelephone: { required: replaceMessageString(objValMsg, "Required", "telephone") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCompany")
                            error.insertAfter("#dvCompany");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlContact")
                            error.insertAfter("#dvContact");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlHeadsetBrand")
                            error.insertAfter("#dvHeadsetBrand");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlTotalHeadsetstoRepair")
                            error.insertAfter("#dvTotalHeadsetstoRepair");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlRequestQuote")
                            error.insertAfter("#dvRequestQuote");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlretCountry")
                            error.insertAfter("#divretCountry");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlretState")
                            error.insertAfter("#divretState");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlretCity")
                            error.insertAfter("#divretCity");

                        //                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlStatus")
                        //                            error.insertAfter("#dvStatus");
                        else
                            error.insertAfter(element);
                    }
                });
            });

            $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                addcommanrules();
                if ($('#aspnetForm').valid()) {
                    $('#dvLoader').show();
                    return true;
                }
                else {
                    return false;
                }
            });

            $("#ctl00_ContentPlaceHolder1_ddlReturnHeadsets").change(function() {
                if ($("#ctl00_ContentPlaceHolder1_ddlReturnHeadsets option:selected").val() == "-1") {
                    $('#dvLoader').show();
                    $("#ctl00_ContentPlaceHolder1_btnreturnheadset").click();
                }
            });

            $("#<%=lnkbtnheadsetreturn.ClientID %>").click(function() {
                addpopuprules();
                if ($('#aspnetForm').valid()) {
                    $('#dvLoader').show();
                    return true;
                }
                else {
                    return false;
                }
            });
        });

        function addcommanrules() {
            $("#ctl00_ContentPlaceHolder1_ddlCompany").rules("add", {
                NotequalTo: "0",
                messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "company") }
            });

            $("#ctl00_ContentPlaceHolder1_ddlContact").rules("add", {
                NotequalTo: "0",
                messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "contact") }
            });
            $("#ctl00_ContentPlaceHolder1_ddlHeadsetBrand").rules("add", {
                NotequalTo: "0",
                messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "headsetBrand") }
            });
            $("#ctl00_ContentPlaceHolder1_ddlTotalHeadsetstoRepair").rules("add", {
                NotequalTo: "0",
                messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "totalheadsetstorepair") }
            });
            $("#ctl00_ContentPlaceHolder1_ddlRequestQuote").rules("add", {
                NotequalTo: "0",
                messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "RequestQuote") }
            });


            $("#ctl00_ContentPlaceHolder1_txtretAddressName").rules("remove");
            $("#ctl00_ContentPlaceHolder1_txtretCompany").rules("remove");
            $("#ctl00_ContentPlaceHolder1_txtretContact").rules("remove");
            $("#ctl00_ContentPlaceHolder1_txtretaddress1").rules("remove");
            $("#ctl00_ContentPlaceHolder1_txtretaddress2").rules("remove");
            $("#ctl00_ContentPlaceHolder1_ddlretCountry").rules("remove");
            $("#ctl00_ContentPlaceHolder1_ddlretState").rules("remove");
            $("#ctl00_ContentPlaceHolder1_ddlretCity").rules("remove");
            $("#ctl00_ContentPlaceHolder1_txtretzip").rules("remove");
            $("#ctl00_ContentPlaceHolder1_txtrettelephone").rules("remove");
        }

        function addpopuprules() {
            $("#ctl00_ContentPlaceHolder1_txtretAddressName").rules("add", {
                required: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "Address Name") }
            });
            $("#ctl00_ContentPlaceHolder1_txtretCompany").rules("add", {
                required: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "Company") }
            });
            $("#ctl00_ContentPlaceHolder1_txtretContact").rules("add", {
                required: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "Contact") }
            });
            $("#ctl00_ContentPlaceHolder1_txtretaddress1").rules("add", {
                required: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "Address1") }
            });
            $("#ctl00_ContentPlaceHolder1_txtretaddress2").rules("add", {
                required: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "Address2") }
            });
            $("#ctl00_ContentPlaceHolder1_ddlretCountry").rules("add", {
                NotequalTo: "0",
                messages: { required: replaceMessageString(objValMsg, "NotEqualTo", "Country") }
            });
            $("#ctl00_ContentPlaceHolder1_ddlretState").rules("add", {
                NotequalTo: "0",
                messages: { required: replaceMessageString(objValMsg, "NotEqualTo", "State") }
            });
            $("#ctl00_ContentPlaceHolder1_ddlretCity").rules("add", {
                NotequalTo: "0",
                messages: { required: replaceMessageString(objValMsg, "NotEqualTo", "City") }
            });
            $("#ctl00_ContentPlaceHolder1_txtretzip").rules("add", {
                required: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "Zip") }
            });
            $("#ctl00_ContentPlaceHolder1_txtrettelephone").rules("add", {
                required: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "Telephone") }
            });

            $("#ctl00_ContentPlaceHolder1_ddlCompany").rules("remove");
            $("#ctl00_ContentPlaceHolder1_ddlContact").rules("remove");
            $("#ctl00_ContentPlaceHolder1_ddlHeadsetBrand").rules("remove");
            $("#ctl00_ContentPlaceHolder1_ddlTotalHeadsetstoRepair").rules("remove");
            $("#ctl00_ContentPlaceHolder1_ddlRequestQuote").rules("remove");
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad select_box_pad" style="width: 400px;">
        <div class="form_table">
            <div class="spacer10">
            </div>
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <div class="spacer10">
            </div>
            <table>
                <tr id="trCompany" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 40%;">Company </span><span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlCompany" onchange="pageLoad(this,value);" runat="server"
                                        TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCompany">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trContact" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 40%;">Contact </span><span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlContact" onchange="pageLoad(this,value);" runat="server"
                                        OnSelectedIndexChanged="ddlContect_SelectedIndexChanged" AutoPostBack="true"
                                        TabIndex="2">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvContact">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trHeadsetBrand" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 40%;">Headset Brand </span><span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlHeadsetBrand" onchange="pageLoad(this,value);" runat="server"
                                        TabIndex="3">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvHeadsetBrand">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trTotalHeadsetstoRepair" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 40%;">Total Headsets to Repair </span><span
                                    class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlTotalHeadsetstoRepair" onchange="pageLoad(this,value);"
                                        runat="server" TabIndex="4">
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                                <div id="dvTotalHeadsetstoRepair">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trRequestQuote" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 40%;">Request Quote Before Repair </span>
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlRequestQuote" onchange="pageLoad(this,value);" runat="server"
                                        TabIndex="5">
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                                <div id="dvRequestQuote">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trReturnHeadsets" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 40%;">Return Headsets To </span><span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlReturnHeadsets" onchange="pageLoad(this,value);" runat="server"
                                        TabIndex="6">
                                    </asp:DropDownList>
                                </span>
                                <asp:Button ID="btnreturnheadset" runat="server" OnClick="btnbtnreturnheadset_Click" />
                                <div id="dvReturnHeadsets">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trStatus" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 40%;">Status </span><span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlStatus" onchange="pageLoad(this,value);" runat="server"
                                        TabIndex="6">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvStatus">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="gredient_btn" TabIndex="6" runat="server"
                            ToolTip="Submit Repair" OnClick="lnkBtnSaveInfo_Click"><span>Submit Repair & Print Shipping Label</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:LinkButton ID="lnkDummyPro" class="grey2_btn" runat="server" Style="display: none"></asp:LinkButton>
    <at:ModalPopupExtender ID="modalPrint" TargetControlID="lnkDummyPro" BackgroundCssClass="modalBackground"
        DropShadow="true" runat="server" PopupControlID="pnlPrintOrderReturn" CancelControlID="closepro">
    </at:ModalPopupExtender>
    <asp:Panel ID="pnlPrintOrderReturn" runat="server" Style="display: none;">
        <div class="pp_pic_holder facebook" style="display: block; width: 490px; position: fixed;
            left: 30%; top: 30%;">
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
                                    <div class="pp_inline clearfix" style="padding-left: 75px; padding-top: 30px">
                                        <div class="form_popup_box">
                                            <a id="lnkPrintRetunAuthorization" runat="server" title="Print Retun Authorization"
                                                class="gredient_btnMainPage" onclick="javascript:print();"><span>Print Retun Authorization
                                                </span></a>
                                            <asp:LinkButton ID="lnkPrintPrepaidLabel" runat="server" title="Print Pre-paid Return Shipping"
                                                class="gredient_btnMainPage">
                                                <span>Print Pre-paid Return Shipping</span></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <div class="pp_details clearfix" style="width: 371px;">
                                    <a href="#" id="closepro" runat="server" class="pp_close">Close</a>
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
    <asp:Panel ID="pnlReturnHeadsetrepaircenter" runat="server" Style="display: none;
        overflow-y: auto;">
        <div class="cboxWrapper" style="display: block; width: 500px; position: fixed; left: 35%;
            top: 15%;">
            <div style="">
                <div id="cboxTopLeft" style="float: left;">
                </div>
                <div id="cboxTopCenter" style="float: left; width: 425px;">
                </div>
                <div id="cboxTopRight" style="float: left;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxMiddleLeft" style="float: left; height: 675px;">
                </div>
                <div id="cboxContent" style="float: left; display: block; height: 675px; width: 425px;">
                    <div id="cboxLoadedContent" style="display: block; margin: 0;">
                        <div id="cboxClose">
                            close</div>
                        <div style="margin-top: 30px; height: 650px; overflow-y: auto; padding-right: 5px;">
                            <table class="form_table scrollme" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">Address Name </span>
                                                <asp:TextBox ID="txtretAddressName" runat="server" CssClass="w_label"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">Company </span>
                                                <asp:TextBox ID="txtretCompany" runat="server" CssClass="w_label"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">Contact </span>
                                                <asp:TextBox ID="txtretContact" runat="server" CssClass="w_label"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">Address1 </span>
                                                <asp:TextBox ID="txtretaddress1" runat="server" CssClass="w_label"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">Address2 </span>
                                                <asp:TextBox ID="txtretaddress2" runat="server" CssClass="w_label"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">Country </span><span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlretCountry" onchange="pageLoad(this,value);" runat="server"
                                                        TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlretCountry_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </span>
                                                <div id="divretCountry">
                                                </div>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">State </span><span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlretState" onchange="pageLoad(this,value);" runat="server"
                                                        TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlretState_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </span>
                                                <div id="divretState">
                                                </div>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">City </span><span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlretCity" onchange="pageLoad(this,value);" runat="server"
                                                        TabIndex="1">
                                                    </asp:DropDownList>
                                                </span>
                                                <div id="divretCity">
                                                </div>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">Zip </span>
                                                <asp:TextBox ID="txtretzip" runat="server" CssClass="w_label"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label" style="width: 40%;">Telephone </span>
                                                <asp:TextBox ID="txtrettelephone" runat="server" CssClass="w_label"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="btn_w_block" style="padding-top: 15px">
                                <asp:LinkButton ID="lnkbtnheadsetreturn" class="gredient_btn" TabIndex="6" runat="server"
                                    ToolTip="Submit headset returen address" OnClick="lnkbtnheadsetreturn_Click"><span>Submit</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="cboxMiddleRight" style="float: left; height: 675px;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxBottomLeft" style="float: left;">
                </div>
                <div id="cboxBottomCenter" style="float: left; width: 425px;">
                </div>
                <div id="cboxBottomRight" style="float: left;">
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
