<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="RepairProfile.aspx.cs" Inherits="AssetManagement_RepairManagement_RepairProfile"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
        function DeleteConfirmation() {
            var ans = confirm("Are you sure, you want to delete selected Campaign?");
            if (ans) {

                return true;
            }
            else {

                return false;

            }

        }
        
    </script>

    <script type="text/javascript">
        $(document).ready(function() {
            //hide the all of the element with class msg_body
            $(".msg_body").hide();
            //toggle the componenet with class msg_body
            $(".msg_head").click(function() {
                $(this).next(".msg_body").slideToggle(600);
            });
        });
    </script>

    <style type="text/css">
        .subownername
        {
            color: #72757C;
        }
        .form_popup_box span.error
        {
            color: #FF0000;
            display: block;
            font-style: italic;
            padding: 0px 0 0 0px;
            text-align: left;
        }
        .textarea_box textarea
        {
            height: 198px;
        }
        .checklist input
        {
            float: left;
        }
        .checklist label
        {
            line-height: 10px;
            width: 100%;
            text-align: left;
            display: block;
        }
        p
        {
            padding: 0 0 1em;
        }
        .msg_list
        {
            margin: 0px;
            padding: 0px;
            width: 383px;
        }
        a.gredient_btnMainPage img 
        {
            border: medium none;
            float: left;
            height: 35px;
            margin: 0 !important;
            padding: 0;
            width: 35px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        var formats = 'jpg|gif|png|swf|pdf|doc';
        $().ready(function() {

            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {

                        ctl00$ContentPlaceHolder1$txtEquipmentId: { required: true },
                        ctl00$ContentPlaceHolder1$flFile:
                     {
                         accept: formats
                     },
                        ctl00$ContentPlaceHolder1$txtPICCEmail: { email: true },
                        ctl00$ContentPlaceHolder1$txtInsAgentEmail: { email: true },
                        ctl00$ContentPlaceHolder1$txtRegCmpCntEmail: { email: true },
                        ctl00$ContentPlaceHolder1$txtInsDate: { required: true }
                    },
                    messages: {

                        ctl00$ContentPlaceHolder1$txtEquipmentId: { required: replaceMessageString(objValMsg, "Required", "Equipment ID") },
                        ctl00$ContentPlaceHolder1$flFile: {
                            accept: "<br/>" + replaceMessageString(objValMsg, "ImageType", "jpg,gif,png,pdf,txt,doc")
                        },
                        ctl00$ContentPlaceHolder1$txtPICCEmail: {
                            email: replaceMessageString(objValMsg, "Email", "")
                        },
                        ctl00$ContentPlaceHolder1$txtInsAgentEmail: {
                            email: replaceMessageString(objValMsg, "Email", "")
                        },
                        ctl00$ContentPlaceHolder1$txtRegCmpCntEmail: {
                            email: replaceMessageString(objValMsg, "Email", "")
                        },
                        ctl00$ContentPlaceHolder1$txtInsDate: { required: replaceMessageString(objValMsg, "Required", "date") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtInsDate")
                            error.insertAfter("#dvDate");
                        else
                            error.insertAfter(element);
                    }
                });
            });

            $("#ctl00_ContentPlaceHolder1_lnkBtnSaveInfo").click(function() {

                return $('#aspnetForm').valid();
            });

            $("#ctl00_ContentPlaceHolder1_btnSubmit").click(function() {

                return $('#aspnetForm').valid();
            });            
            
            if ($("#ctl00_ContentPlaceHolder1_hdnExpand").attr("value") == "OP") {
                $("#dvOrderParts").show();
            }
           else if ($("#ctl00_ContentPlaceHolder1_hdnExpand").attr("value") == "RH") {
                $("#dvShowRepairHours").show();
            }
            else if ($("#ctl00_ContentPlaceHolder1_hdnExpand").attr("value") == "CB") {
                $("#dvCloseBilling").show();
            }
        });

        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'Date',
                showOn: 'button',
                buttonImage: '../../CSS/images/calendar_small.png',
                buttonImageOnly: true
            });
        });

        function DeleteConfirmation() {
            var ans = confirm("Are you sure, you want to delete selected Record?");
            if (ans) {
                return true;
            }
            else {
                return false;
            }
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:HiddenField ID="hdnExpand" runat="server" />
    <div class="spacer15">
    </div>
    <div class="black_middle order_detail_pad" style="margin-left: 50px; margin-right: 50px;">
        <table class="order_detail" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="font-size: 15px; width: 70%">
                    <label>
                        Repair #:
                    </label>
                    <asp:Label ID="lblRepairNumber" runat="server"></asp:Label>
                </td>
                <td style="font-size: 15px; width: 30%">
                    <label>
                        Requested Date :
                    </label>
                    <asp:Label ID="lblRequestedDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 15px">
                    <label>
                        Vendor Repair #:
                    </label>
                    <asp:Label ID="lblVendorRepairID" runat="server"></asp:Label>
                </td>
                <td style="font-size: 15px">
                    <label>
                        Returned Date :
                    </label>
                    <asp:Label ID="lblReturnedDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 15px">
                    <label>
                        Repair Status :
                    </label>
                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                </td>
                <td style="font-size: 15px">
                    <label>
                        Asset Type :
                    </label>
                    <asp:Label ID="lblAssetType" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 15px">
                    <label>
                        Asset ID :
                    </label>
                    <asp:Label ID="lblAssetID" runat="server" />
                </td>
                <td style="font-size: 15px">
                    <label>
                        Serial # :
                    </label>
                    <asp:Label ID="lblSerialNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 15px">
                    <label>
                        Brand :
                    </label>
                    <asp:Label ID="lblBrand" runat="server"></asp:Label>
                </td>
                <td style="font-size: 15px">
                    <label>
                        Fuel :
                    </label>
                    <asp:Label ID="lblFuel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 15px">
                    <label>
                        Year :
                    </label>
                    <asp:Label ID="lblYear" runat="server"></asp:Label>
                </td>
                <td style="font-size: 15px">
                    <label>
                        Repair Reason :
                    </label>
                    <asp:Label ID="lblReasonRepair" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td style="width: 97%">
                    <at:ModalPopupExtender ID="ModalEdit" TargetControlID="lnkEditRepairOrder" BackgroundCssClass="modalBackground"
                        DropShadow="true" runat="server" PopupControlID="pnlEditAsset" CancelControlID="PopupClose">
                    </at:ModalPopupExtender>
                    <asp:Panel ID="pnlEditAsset" runat="server" Style="display: none;">
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
                                        <div class="pp_content" style="height: 150px; display: block;">
                                            <div class="pp_loaderIcon" style="display: none;">
                                            </div>
                                            <div class="pp_fade" style="display: block;">
                                                <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                <div class="pp_hoverContainer" style="height: 100px; width: 500px; display: none;">
                                                    <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                        style="visibility: visible;">previous</a>
                                                </div>
                                                <div id="Div1">
                                                    <div class="pp_inline clearfix">
                                                        <div class="form_popup_box">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Vendor Repair#</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:TextBox ID="txtVendorRepairID" Width="170px" runat="server" MaxLength="10" CssClass="w_label"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span class="input_label">Repair Order Status</span>
                                                                    </td>
                                                                    <td>
                                                                        <div class="label_bar">
                                                                            <asp:DropDownList ID="ddlRepairOrderStatus" runat="server" Width="170px" onchange="pageLoad(this,value);">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="centeralign" colspan="2" style="padding-top: 10px">
                                                                        <asp:LinkButton ID="lnkBtnSaveInfo" runat="server" class="grey2_btn" ToolTip="Save Basic Information"
                                                                            OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
                                                                        <div class="pp_details clearfix">
                                                                            <a id="PopupClose" runat="server" class="pp_close" href="#">Close</a>
                                                                            <p class="pp_description" style="display: none;">
                                                                            </p>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
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
                </td>
                <td align="right" style="width: 3%">
                    <asp:LinkButton ID="lnkEditRepairOrder" runat="server" CssClass="subownername"><span>Edit</span></asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <div class="spacer10">
    </div>
    <div class="order_detail_pad" style="margin-left: 40px; margin-right: 8px;">
        <table width="100%">
            <tr>
                <td>
                    <div class="form_table">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="custom-sel">
                                <asp:DropDownList ID="ddlShowMe" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlShowMe_SelectedIndexChanged"
                                    onchange="pageLoad(this,value);">
                                </asp:DropDownList>
                            </span>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
                <td width="30%">
                </td>
                <td width="40%">
                </td>
            </tr>
        </table>
    </div>
    <%--Repair Reason Start --%>
    <div runat="server" id="dvRepairReason">
        <div style="width: 861px; margin: 0 auto;">
            <div class="header_bg" runat="server" id="dvInsuranceRecords">
                <div class="header_bgr" id="divheder" runat="server">
                    <%-- <span class="title alignleft" runat="server" id="spantitle" style="width: 36%;">
                        <asp:Label ID="lblHeading" runat="server" Text="Repair Reasons"></asp:Label>
                    </span>--%><span class="title alignleft">
                        <asp:Label ID="lblProblemDesc" runat="server"></asp:Label>
                    </span>
                    <div class="alignnone">
                        &nbsp;</div>
                </div>
            </div>
            <div class="alignnone">
                &nbsp;</div>
        </div>
        <div class="spacer25">
        </div>
    </div>
    <%--Repair Reason End--%>
    <%--Parts & Asset Specification Start--%>
    <div runat="server" id="dvPartsAsset" style="margin-left: 50px; margin-right: 50px;">
        <div style="text-align: center">
            <asp:Label ID="lblFieldError" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="form_table">
            <asp:DataList ID="dtlField" runat="server" RepeatDirection="Vertical" RepeatColumns="1"
                RepeatLayout="Flow" OnItemDataBound="dtlField_ItemDataBound">
                <ItemTemplate>
                    <div style="margin-bottom: -10px;">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">
                                <%# Eval("FieldMasterName") %></span>
                            <asp:HiddenField ID="hdnFieldMasterID" runat="server" Value='<%# Eval("FieldMasterID") %>' />
                            <asp:TextBox ID="txtFieldDesc" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <div class="spacer25">
            </div>
            <%-- <div class="aligncenter ">
                <asp:LinkButton ID="lnkbtnSaveField" class="grey2_btn" runat="server" OnClick="lnkbtnSaveField_Click"><span>Save</span></asp:LinkButton>
            </div>--%>
        </div>
        <div class="spacer25">
        </div>
    </div>
    <%--Parts & Asset Specification End--%>
    <%--My Repair Hours Start--%>
    <div runat="server" id="dvShowRepairHours" style="margin-left: 50px; margin-right: 50px;">
        <div style="clear: both;">
        </div>
        <div class="rightalign">
            <asp:LinkButton ID="lnkPostBillingDetails" runat="server" CssClass="grey2_btn rightalign"><span>Post Hours</span></asp:LinkButton>
            <asp:LinkButton ID="lnkPostBillingDetailDummy" runat="server" CssClass="grey2_btn"
                Style="display: none;"></asp:LinkButton>
            <div class="spacer5">
            </div>
        </div>
        <div>
            <asp:GridView ID="gvPostJobBilling" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblUserInfoId" Text='<%# Eval("UserInfoID") %>' />
                            <asp:Label runat="server" ID="lblBillingPartsOrderedID" Text='<%# Eval("BillingRepairHoursID") %>' />
                            <asp:Label runat="server" ID="lblRepairOrderID" Text='<%# Eval("RepairOrderID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Date">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnDate" runat="server" CommandArgument="Date" CommandName="Sort"><span>Date</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDate" Text='<%# Eval("CreatedDate","{0:MM/dd/yyyy}") %> ' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="FirstName">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnFullName" runat="server" CommandArgument="FirstName" CommandName="Sort"><span>Mechanic</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblFirstName" Text='<%# Eval("MechanicName") %> ' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Hours">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnHours" runat="server" CommandArgument="Hours" CommandName="Sort"><span>Hours</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblHoursWorked" Text='<%# Eval("HoursWorked") %> ' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle CssClass="centeralign" />
                        <HeaderTemplate>
                            <span>Summary Notes</span>
                            <div class="corner">
                                <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                            </div>
                        </HeaderTemplate>
                        <HeaderStyle />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblSummary" Text='<%# Eval("SummaryWorkPerformed") %>' />
                            <div class="corner">
                                <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box leftalign" Width="65%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="spacer5">
            </div>
            <div style="text-align: right; font-size: larger; color: #72757C" id="dvTotalRecords"
                visible="false" runat="server">
                <span>Total Billable Hours: </span>
                <asp:Label ID="LblTotalHours" runat="server" />
            </div>
        </div>
        <div>
            <table width="100%" id="tblPostBilling" runat="server">
                <tr>
                    <td style="width: 97%">
                        <at:ModalPopupExtender ID="ModalPostBillingDetails" TargetControlID="lnkPostBillingDetails"
                            BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlpostHour"
                            CancelControlID="PopupClose1">
                        </at:ModalPopupExtender>
                        <asp:Panel ID="pnlpostHour" runat="server" Style="display: none;">
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
                                            <div class="pp_content" style="height: 250px; display: block;">
                                                <div class="pp_loaderIcon" style="display: none;">
                                                </div>
                                                <div class="pp_fade" style="display: block;">
                                                    <div id="Div7">
                                                        <div class="pp_inline clearfix">
                                                            <div class="form_popup_box">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label" style="vertical-align: bottom">Date</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <span>
                                                                                    <asp:TextBox ID="txtDatejobbilling" Width="140px" runat="server" CssClass="w_label datepicker min_w"
                                                                                        TabIndex="1">
                                                                                    </asp:TextBox></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label" style="vertical-align: bottom">Hours Worked</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <span>
                                                                                    <asp:TextBox ID="txtHoursWorked" Width="140px" runat="server" TabIndex="2">
                                                                                    </asp:TextBox>
                                                                                    <at:FilteredTextBoxExtender ID="fttxtHoursWorked" runat="server" TargetControlID="txtHoursWorked"
                                                                                        FilterType="Custom,Numbers" ValidChars=".">
                                                                                    </at:FilteredTextBoxExtender>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label" style="vertical-align: bottom">Summary Work Performed</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <span>
                                                                                    <asp:TextBox ID="txtSummaryWorkPerformed" runat="server" Height="80px" Width="220"
                                                                                        TextMode="MultiLine">
                                                                                    </asp:TextBox></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="centeralign" colspan="2" style="padding-top: 20px">
                                                                            <asp:LinkButton ID="lnkBtnSavePostHours" runat="server" class="grey2_btn" ToolTip="Save Information"
                                                                                OnClick="lnkBtnSavePostHours_Click"><span>Save Information</span></asp:LinkButton>
                                                                            <div class="pp_details clearfix">
                                                                                <a id="PopupClose1" runat="server" class="pp_close" href="#">Close</a>
                                                                                <p class="pp_description" style="display: none;">
                                                                                </p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
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
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%--My Repair Hours End--%>
    <%--My Parts Ordered Start--%>
    <div runat="server" id="dvOrderParts" style="margin-left: 50px; margin-right: 50px;">
        <div style="text-align: center; color: Red; font-size: larger;">
            <asp:Label ID="lblPartsOrderedMsg" runat="server">
            </asp:Label>
        </div>
        <asp:GridView ID="gvPartsOrdered" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvPartsOrdered_RowCommand">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblUserInfoID" Text='<%# Eval("UserInfoID") %>' />
                        <asp:Label runat="server" ID="lblBillingPartsOrderedID" Text='<%# Eval("BillingPartsOrderedID") %>' />
                        <asp:Label runat="server" ID="lblRepairOrderID" Text='<%# Eval("RepairOrderID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Vendor">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnVendor" runat="server" CommandArgument="Vendor" CommandName="Sort"><span>Vendor</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblVendor" Text='<%# "&nbsp;" + Eval("VendorName") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="15%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Quantity">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnQuantity" runat="server" CommandArgument="Quantity" CommandName="Sort"><span>QTY</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblQuantity" Text='<%# Eval("Quantity") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="5%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="PartNumber">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnPartNumber" runat="server" CommandArgument="PartNumber"
                            CommandName="Sort"><span>PartNumber</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPartNumber" Text='<%# Eval("PartNumber") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="15%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle CssClass="centeralign" />
                    <HeaderTemplate>
                        <span>Description</span>
                    </HeaderTemplate>
                    <HeaderStyle />
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDescriptions" Text='<%# Eval("SummaryDescriptions") %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="60%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span class="centeralign">Delete</span>
                        <div class="corner">
                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span class="btn_space">
                            <asp:ImageButton ID="lnkbtndelete" runat="server" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                CommandArgument='<%# Eval("BillingPartsOrderedID") %>' ImageUrl="~/Images/close.png" /></span>
                        <div class="corner">
                            <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box centeralign" Width="5%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="spacer10">
        </div>
        <div style="clear: both;">
        </div>
        <div class="rightalign">
            <asp:LinkButton ID="lnkCheckOnSiteStock" runat="server" CssClass="grey2_btn" OnClick="lnkCheckOnSiteStock_Click"><span>Check On-Site Stock</span></asp:LinkButton>
            <asp:LinkButton ID="lnkAddParts" runat="server" CssClass="grey2_btn"><span>Add Parts</span></asp:LinkButton>
            <%--<asp:LinkButton ID="lnkAddPartsDummy" runat="server" CssClass="grey2_btn" Style="display: none;"></asp:LinkButton>--%>
        </div>
        <div>
            <table width="100%" id="tblPartsOrdered" runat="server">
                <tr>
                    <td style="width: 97%">
                        <at:ModalPopupExtender ID="ModalPartsOrdered" TargetControlID="lnkAddParts" BackgroundCssClass="modalBackground"
                            DropShadow="true" runat="server" PopupControlID="pnlPartsOrdered" CancelControlID="ClosePart">
                        </at:ModalPopupExtender>
                        <asp:Panel ID="pnlPartsOrdered" runat="server" Style="display: none;">
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
                                            <div class="pp_content" style="height: 300px; display: block;">
                                                <div class="pp_loaderIcon" style="display: none;">
                                                </div>
                                                <div class="pp_fade" style="display: block;">
                                                    <div id="Div2">
                                                        <div class="pp_inline clearfix">
                                                            <div class="form_popup_box">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label" style="vertical-align: bottom">Order Quantity</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <span>
                                                                                    <asp:TextBox ID="txtOrderQuantity" Width="140px" runat="server" MaxLength="4">
                                                                                    </asp:TextBox>
                                                                                    <at:FilteredTextBoxExtender ID="fltOrderQuantity" runat="server" TargetControlID="txtOrderQuantity"
                                                                                        FilterType="Numbers">
                                                                                    </at:FilteredTextBoxExtender>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label" style="vertical-align: bottom">Part Number</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <span>
                                                                                    <asp:TextBox ID="txtPartNumber" Width="140px" runat="server" MaxLength="20">
                                                                                    </asp:TextBox></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label" style="vertical-align: bottom">Part Description</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <span>
                                                                                    <asp:TextBox ID="txtPartDescription" runat="server" Height="80px" Width="220" TextMode="MultiLine">
                                                                                    </asp:TextBox></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label" style="vertical-align: bottom">Vendor</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <span>
                                                                                    <asp:DropDownList ID="ddlVendor" runat="server" onchange="pageLoad(this,value);">
                                                                                    </asp:DropDownList>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="centeralign" colspan="2" style="padding-top: 20px">
                                                                            <asp:LinkButton ID="lnkAddPartOrderInfo" runat="server" class="grey2_btn" ToolTip="Save Information"
                                                                                OnClick="lnkAddPartOrderInfo_Click"><span>Save Information</span></asp:LinkButton>
                                                                            <div class="pp_details clearfix">
                                                                                <a id="ClosePart" runat="server" class="pp_close" href="#">Close</a>
                                                                                <p class="pp_description" style="display: none;">
                                                                                </p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
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
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%--My Parts Ordered End--%>
    <%--My Close Billing Start--%>
    <div runat="server" id="dvCloseBilling" style="margin-left: 50px; margin-right: 50px;">
        <div>
         <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
            <div class="spacer10">
            </div>
            <div id="dvPartsUsed" runat="server" visible="false">
                Parts Used:
            </div>
            <div class="form_table">
                <table>
                    <tr>
                        <td>
                            <div>
                                <asp:GridView ID="gvFBPartsOrdered" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblUserInfoID" Text='<%# Eval("UserInfoID") %>' />
                                                <asp:Label runat="server" ID="lblBillingPartsOrderedID" Text='<%# Eval("BillingPartsOrderedID") %>' />
                                                <asp:Label runat="server" ID="lblRepairOrderID" Text='<%# Eval("RepairOrderID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Vendor">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnVendor" runat="server" CommandArgument="Vendor" CommandName="Sort"><span>Vendor</span></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblVendor" Text='<%#  "&nbsp;" + Eval("VendorName") %> ' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Quantity">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnQuantity" runat="server" CommandArgument="Quantity" CommandName="Sort"><span>QTY</span></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblQuantity" Text='<%# Eval("Quantity") %> ' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="PartNumber">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnPartNumber" runat="server" CommandArgument="PartNumber"
                                                    CommandName="Sort"><span>PartNumber</span></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblPartNumber" Text='<%# Eval("PartNumber") %> ' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle CssClass="centeralign" />
                                            <HeaderTemplate>
                                                <span>Description</span>
                                            </HeaderTemplate>
                                            <HeaderStyle />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDescriptions" Text='<%# Eval("SummaryDescriptions") %>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="65%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvRepairHours" runat="server" visible="false">
                Repair Hours:
            </div>
            <div class="form_table">
                <table>
                    <tr>
                        <td>
                            <div>
                                <asp:GridView ID="gvFBPostJobBilling" runat="server" AutoGenerateColumns="false"
                                    HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                    RowStyle-CssClass="ord_content">
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblUserInfoId" Text='<%# Eval("UserInfoID") %>' />
                                                <asp:Label runat="server" ID="lblBillingPartsOrderedID" Text='<%# Eval("BillingRepairHoursID") %>' />
                                                <asp:Label runat="server" ID="lblRepairOrderID" Text='<%# Eval("RepairOrderID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Date">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnDate" runat="server" CommandArgument="Date" CommandName="Sort"><span>Date</span></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDate" Text='<%# Eval("CreatedDate","{0:MM/dd/yyyy}") %> ' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="FirstName">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnFullName" runat="server" CommandArgument="FirstName" CommandName="Sort"><span>Mechanic</span></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblFirstName" Text='<%# Eval("MechanicName") %> ' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Hours">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnHours" runat="server" CommandArgument="Hours" CommandName="Sort"><span>Hours</span></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblHoursWorked" Text='<%# Eval("HoursWorked") %> ' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle CssClass="centeralign" />
                                            <HeaderTemplate>
                                                <span>Summary Notes</span>
                                                <div class="corner">
                                                    <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                </div>
                                            </HeaderTemplate>
                                            <HeaderStyle />
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSummary" Text='<%# Eval("SummaryWorkPerformed") %>' />
                                                <div class="corner">
                                                    <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="65%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form_table">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box taxt_area clearfix" style="height: 220px">
                    <span class="input_label alignleft" style="height: 218px">
                        <br />
                        Mechanic Notes : </span>
                    <div class="textarea_box alignright">
                        <div class="scrollbar" style="height: 218px">
                            <a href="#scroll" id="A3" class="scrolltop"></a><a href="#scroll" id="A4" class="scrollbottom">
                            </a>
                        </div>
                        <asp:TextBox ID="txtMecanicNotes" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                            ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <%-- Post Invoice Start  --%>
             <div class="spacer20">
            </div>
             <div style="width: 861px; margin: 0 auto;">
                <div class="header_bg" runat="server" id="Div5">
                    <div class="header_bgr" id="div8" runat="server">
                        <span class="title alignleft" runat="server" id="span1" style="width: 36%;">
                            <asp:Label ID="Label1" runat="server" Text="Post Repair Invoice"></asp:Label>
                        </span><div class="alignnone">
                            &nbsp;</div>
                    </div>
                </div>
                <div class="alignnone">
                    &nbsp;</div>
            </div>
             <div class="spacer20">
            </div>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                    <div class="rightalign">
                        <asp:LinkButton ID="lnkPostInvoice" runat="server" CssClass="grey2_btn" OnClick="lnkPostInvoice_Click"><span>Post Invoice</span></asp:LinkButton>
                        <asp:LinkButton ID="lnkPostInvoiceDummy" runat="server" CssClass="grey2_btn" Style="display: none;"></asp:LinkButton>
                    </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="spacer20">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div>
                                <div style="text-align: center; color: Red; font-size: larger;">
                                    <asp:Label ID="lblMaintenance" runat="server">
                                    </asp:Label>
                                </div>
                                <div>
                                    <asp:GridView ID="gvEquipment" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvEquipment_RowDataBound"
                                        OnRowCommand="gvEquipment_RowCommand" ShowFooter="True">
                                        <Columns>
                                            <asp:TemplateField Visible="False" HeaderText="EquipmentMaintenanceCostID">
                                                <HeaderTemplate>
                                                    <span>EquipmentMaintenanceCostID</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblEquipmentID" Text='<%# Eval("EquipmentMaintenanceCostID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False" HeaderText="EquipmentMasterID">
                                                <HeaderTemplate>
                                                    <span>EquipmentMasterID</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblEquipmentMasterID" Text='<%# Eval("EquipmentMasterID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtVendor" runat="server" CommandArgument="Vendor" CommandName="Sort"><span>Vendor</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderVendor" runat="server"></asp:PlaceHolder>
                                                    <div class="corner">
                                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl centeralign"></span>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hfVendor" runat="server" Value='<%# Eval("Vendor")%>' />
                                                    <%--<asp:Label runat="server" ID="lblVendor" Text='<%# "&nbsp;" + Eval("Vendor") %>'></asp:Label>--%>
                                                    <asp:LinkButton ID="lnkVendor" CommandArgument='<%# Eval("EquipmentMaintenanceCostID") %>'
                                                        CommandName="Vendor" runat="server">
                                         <span class="first"><%# "&nbsp;" + Eval("EquipmentVendorName")%></span>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box" Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="DateofService">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnDateofService" runat="server" CommandArgument="DateofService"
                                                        CommandName="Sort"><span >Date of Service</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderDateofService" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hfDateofService" runat="server" Value='<%# Eval("DateofService")%>' />
                                                    <asp:Label runat="server" ID="lblDateofService" Text='<%# "&nbsp;" + Convert.ToDateTime(Eval("DateofService")).ToString("MM/dd/yyyy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Invoice">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnInvoice" runat="server" CommandArgument="Invoice" CommandName="Sort"><span >Invoice #</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderInvoice" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hfInvoice" runat="server" Value='<%# Eval("Invoice")%>' />
                                                    <asp:Label runat="server" ID="lblInvoice" Text='<%# "&nbsp;" + Eval("Invoice")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="JobCodeName">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnJobCodeName" runat="server" CommandArgument="JobCodeName"
                                                        CommandName="Sort"><span >Job Code</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderJobCodeName" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hfJobCodeName" runat="server" Value='<%# Eval("JobCodeName")%>' />
                                                    <asp:Label runat="server" ID="lblJobCodeName" Text='<%# Eval("JobCodeName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Amount">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnAmount" runat="server" CommandArgument="Amount" CommandName="Sort"><span >Amount</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderAmount" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hfAmount" runat="server" Value='<%# Eval("Amount")%>' />
                                                    <asp:Label runat="server" ID="lblAmount" Text='<%# Eval("Amount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box rightalign" Width="15%" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblShow" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Total: $"></asp:Label>
                                                    <asp:Label ID="lblTotalAmount" runat="server" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span class="centeralign">File</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span class="btn_space">
                                                        <asp:ImageButton ID="lnkbtnPDF" runat="server" CommandName="PDF" CommandArgument='<%# Eval("EquipmentMaintenanceCostID") %>' /></span>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span class="centeralign">Delete</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span class="btn_space">
                                                        <asp:ImageButton ID="lnkbtndelete" runat="server" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                                            CommandArgument='<%# Eval("EquipmentMaintenanceCostID") %>' ImageUrl="~/Images/close.png" /></span>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div>
                                <div class="companylist_botbtn alignleft">
                                </div>
                                <div id="pagingtable" runat="server" class="alignright pagging">
                                    <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                                    </asp:LinkButton>
                                    <span>
                                        <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:DataList></span>
                                    <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="width: 97%">
                        <at:ModalPopupExtender ID="ModalPostInvoice" TargetControlID="lnkPostInvoiceDummy"
                            BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlPostInvoice"
                            CancelControlID="PopupClosePostInvoice">
                        </at:ModalPopupExtender>
                        <asp:Panel ID="pnlPostInvoice" runat="server" Style="display: none;">
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
                                            <div class="pp_content" style="height: 450px; display: block;">
                                                <div class="pp_loaderIcon" style="display: none;">
                                                </div>
                                                <div class="pp_fade" style="display: block;">
                                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                    <div class="pp_hoverContainer" style="height: 250px; width: 350px; display: none;">
                                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                            style="visibility: visible;">previous</a>
                                                    </div>
                                                    <div id="Div6">
                                                        <div class="pp_inline clearfix">
                                                            <div class="form_popup_box">
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:Label ID="Label3" runat="server" CssClass="errormessage"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Vendor</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:DropDownList ID="ddlInvoiceVendor" Width="170px" runat="server" CssClass="w_label">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Date of Service</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtDateofService" Width="140px" runat="server" CssClass="w_label datepicker min_w"
                                                                                    TabIndex="1">
                                                                                </asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Invoice #</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtInvoice" Width="170px" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Description</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtDescription" Width="170px" runat="server" MaxLength="100" CssClass="w_label"
                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Amount</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:TextBox ID="txtAmount" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                                                                <at:FilteredTextBoxExtender ID="ftbAmount" TargetControlID="txtAmount" runat="server"
                                                                                    FilterType="Custom,Numbers" ValidChars=".">
                                                                                </at:FilteredTextBoxExtender>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Job Code</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <asp:DropDownList ID="ddlJobCode" runat="server" Width="170px" OnSelectedIndexChanged="ddlJobCode_SelectedIndexChanged"
                                                                                    AutoPostBack="true" onchange="pageLoad(this,value);">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Job Sub Code</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="checklist" style="height: 80px; border: 1px solid Grey; overflow: auto">
                                                                                <asp:CheckBoxList ID="cblJobSubCode" runat="server" DataTextField="JobCodeName" DataValueField="JobCodeID">
                                                                                </asp:CheckBoxList>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span class="input_label">Document</span>
                                                                        </td>
                                                                        <td>
                                                                            <div class="label_bar">
                                                                                <input type="file" id="InvoiceDoc" runat="server" />
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="centeralign" colspan="2" style="padding-top: 20px">
                                                                            <asp:LinkButton ID="lnkSaveInvoice" runat="server" class="grey2_btn" OnClick="lnkSaveInvoice_Click"
                                                                                ToolTip="Save Basic Information"><span>Save Information</span></asp:LinkButton>
                                                                            <%--<a href="#" class="grey_btn" title="Save Information"><span>Save Information</span></a>--%>
                                                                            <div class="pp_details clearfix">
                                                                                <a id="PopupClosePostInvoice" runat="server" class="pp_close" href="#">Close</a>
                                                                                <p class="pp_description" style="display: none;">
                                                                                </p>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
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
                    </td>
                </tr>
            </table>
            <%-- Post Invoice End  --%>
            
            <div style="width: 861px; margin: 0 auto;">
                <div class="header_bg" runat="server" id="Div3">
                    <div class="header_bgr" id="div4" runat="server">
                        <span class="title alignleft" runat="server" id="spantitle" style="width: 36%;">
                            <asp:Label ID="lblHeading" runat="server" Text="Complete Billing & Close Repair Order"></asp:Label>
                        </span>
                        <%--<a href="~/login.aspx"
                                        class="grey_btn alignright" title="Return to Login Page"><span>Return to Login Page</span></a>--%>
                        <div class="alignnone">
                            &nbsp;</div>
                    </div>
                </div>
                <div class="alignnone">
                    &nbsp;</div>
            </div>
            <div class="spacer20">
            </div>
            <%--<div>
            <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
            <td style="width:60%">
            <div class="form_table" style="width: 280px">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <span class="input_label" style="width: 48%">Service Complete</span> <span class="custom-sel label-sel"
                        style="width: 40%;">
                        <asp:DropDownList ID="ddlServiceComplete" runat="server" onchange="pageLoad(this,value);">
                            <asp:ListItem Value="0" Text="-Select-" />
                            <asp:ListItem Value="True" Text="Yes" />
                            <asp:ListItem Value="False" Text="No" />
                        </asp:DropDownList>
                    </span>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span>
                </div>
            </div>
            </td>
            <td style="width:40%">
            <div class="form_table" style="width: 450px">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <span class="input_label" style="width: 44%">Notify Customer Asset Back in Service</span>
                    <span class="custom-sel label-sel-small" style="width: 51%;">
                        <asp:DropDownList ID="ddlNotifyVendor" runat="server" onchange="pageLoad(this,value);">
                            <asp:ListItem Value="0" Text="-Select-" />
                            <asp:ListItem Value="True" Text="Yes" />
                            <asp:ListItem Value="False" Text="No" />
                        </asp:DropDownList>
                    </span>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span>
                </div>
            </div>
            </td>
            </tr>
             </table>
            </div>--%>
            <%--<div class="spacer20">
            </div>--%>
          <%--  <div class="spacer10">
            </div>
            <div class="form_table" style="width: 630px">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box">
                    <span class="input_label" style="width: 46%">Notify Our Accounting Department Repair
                        Order Complete</span> <span class="custom-sel label-sel-small" style="width: 50%;">
                            <asp:DropDownList ID="ddlNotifyAccounting" runat="server" onchange="pageLoad(this,value);">
                                <asp:ListItem Value="0" Text="-Select-" />
                                <asp:ListItem Value="True" Text="Yes" />
                                <asp:ListItem Value="False" Text="No" />
                            </asp:DropDownList>
                        </span>
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span>
                </div>
            </div>--%>
            <div class="spacer10">
            </div>
             <div class="centeralign">
               
           
            &nbsp;&nbsp;
            
              
            </div>
           
            <div >
            <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
            <td style="width:30%">
            <div class="aligncenter">
               <asp:LinkButton ID="lnkAssetBack" runat="server" CssClass="gredient_btnMainPage" OnClick="lnkAssetBack_Click" Width="272px">
                <img src="../../admin/Incentex_Used_Icons/nabis.png" /><span style="width:auto;">Notify Asset Back in Service</span></asp:LinkButton>
                                
                                
                               <%-- <asp:HyperLink ID="lnkCreateRepairOrder"  runat="server" title="Create Repair Order" class="gredient_btnMainPage">
                <img src="../../admin/Incentex_Used_Icons/addinventory.png" alt="Create Repair Order" />
                <span>
                  Create Repair Order
                </span>
            </asp:HyperLink>--%>
                                </div>
            </td>
            <td style="width:30%" align="center">
            <div class="aligncenter">
             <asp:LinkButton ID="lnkReadyForBilling" runat="server" CssClass="gredient_btnMainPage" OnClick="lnkReadyForBilling_Click" Width="272px">
                <img src="../../admin/Incentex_Used_Icons/ready-for-billing.png" /> <span style="width:auto;">Ready for Billing</span></asp:LinkButton>
                                </div>
            </td>
            <td style="width:40%" align="right">
            <div class="aligncenter">
             <asp:LinkButton ID="lnkBtnCompleteBilling" runat="server" CssClass="gredient_btnMainPage" OnClick="lnkBtnCompleteBilling_Click" Width="252px">
              <img src="../../admin/Incentex_Used_Icons/billing-finished.png" /> <span style="width:auto;">Billing Finished</span></asp:LinkButton>
                                </div>
            </td>
            </tr>  
            <tr>
            <td>
           <br />
           <div class="form_table" style="margin-left: 0px; margin-right: 32px;">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box taxt_area clearfix" >   
                        <asp:TextBox ID="txtAssetBack" runat="server" Width="170px"></asp:TextBox>                   
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
           
            </td>
            <td>
            <br />
              <div class="form_table" style="margin-left: 0px; margin-right: 28px;">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box taxt_area clearfix" >   
                        <asp:TextBox ID="txtReadyForBilling" runat="server" Width="170px"></asp:TextBox>                   
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            </td>
            <td>
            <br />
              <div class="form_table" style="margin-left: 0px; margin-right: 28px;">
                <div class="form_top_co">
                    <span>&nbsp;</span></div>
                <div class="form_box taxt_area clearfix" >   
                        <asp:TextBox ID="txtBillingFinished" runat="server" Width="170px"></asp:TextBox>                   
                </div>
                <div class="form_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            </td>
            </tr>
                      
            </table>
             
             
             </div>    
     
        </div>
    </div>
    <%--My Close Billing End--%>
    <%--Equipment Image Start--%>
    <div runat="server" id="dvEquipmentImageHide" style="margin-left: 50px; margin-right: 50px;">
        <div>
            <table width="100%">
                <tr>
                    <td align="center" style="width: 100%">
                        <div style="text-align: center">
                            <asp:RadioButton ID="rdbAssetImage" runat="server" Text="Asset Images" GroupName="ImageType"
                                Checked="true" AutoPostBack="true" Font-Size="15px" ForeColor="Gray"></asp:RadioButton>
                            &nbsp;
                            <asp:RadioButton ID="rdbIncidentRepair" runat="server" Text="Incident/Repair Images"
                                GroupName="ImageType" AutoPostBack="true" Font-Size="15px" ForeColor="Gray">
                            </asp:RadioButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="spacer25">
                        </div>
                        <asp:LinkButton ID="lnkAddEquipmentImage" runat="server" CssClass="grey2_btn" OnClick="lnkAddEquipmentImage_Click"><span>Add Image</span></asp:LinkButton>
                        <asp:LinkButton ID="lnkAddEquipmentImageDummy" runat="server" CssClass="grey2_btn"
                            Style="display: none;"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <div style="text-align: center">
                <asp:Label ID="lblErrorImage" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <table width="100%">
                <tr>
                    <td style="width: 97%">
                        <at:ModalPopupExtender ID="ModalEquipmentImage" TargetControlID="lnkAddEquipmentImageDummy"
                            BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlEquipmentImage"
                            CancelControlID="PopupCloseEquipmentImage">
                        </at:ModalPopupExtender>
                        <asp:Panel ID="pnlEquipmentImage" runat="server" Style="display: none;">
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
                                            <div class="pp_content" style="height: 200px; display: block;">
                                                <div class="pp_loaderIcon" style="display: none;">
                                                </div>
                                                <div class="pp_fade" style="display: block;">
                                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                    <div class="pp_hoverContainer" style="height: 100px; width: 350px; display: none;">
                                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                            style="visibility: visible;">previous</a>
                                                    </div>
                                                    <div id="Div10">
                                                        <div class="pp_inline clearfix">
                                                            <div class="form_popup_box">
                                                                <table cellpadding="5" cellspacing="5">
                                                                    <tr>
                                                                        <td>
                                                                            <label>
                                                                                Date :</label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtImageDate" runat="server" CssClass="w_label datepicker min_w"
                                                                                TabIndex="1"></asp:TextBox>
                                                                            <div id="Div11">
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label>
                                                                                Name :</label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtImageName" Width="170px" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label>
                                                                                Image :</label>
                                                                        </td>
                                                                        <td>
                                                                            <input type="file" id="flImage" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <div class="label_bar btn_padinn" style="margin-left: 10px;">
                                                                                <asp:LinkButton ID="btnSaveImage" CssClass="grey2_btn" runat="server" OnClick="btnSaveImage_Click"><span>ADD</span></asp:LinkButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="pp_details clearfix" style="width: 371px;">
                                                        <a href="#" id="PopupCloseEquipmentImage" runat="server" class="pp_close">Close</a>
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
                    </td>
                </tr>
            </table>
            <div class="splash_img_pad">
                <asp:DataList ID="dtSplash" runat="server" RepeatDirection="Vertical" RepeatColumns="5"
                    RepeatLayout="Flow" OnItemDataBound="dtSplash_ItemDataBound" OnItemCommand="dtSplash_ItemCommand">
                    <ItemTemplate>
                        <div class="alignleft item">
                            <div>
                                <p class="upload_photo gallery">
                                    <%--  <span class="lt_co"></span><span class="rt_co"></span><span class="lb_co"></span>
                            <span class="rb_co"></span>--%>
                                    <a id="prettyphotoDiv" rel="prettyPhoto[p]" runat="server">
                                        <img id="imgSplashImage" height="182" width="172" runat="server" alt='Loading' />
                                    </a>
                                </p>
                            </div>
                            <div>
                                <p class="upload_photo gallery">
                                    <table cellpadding="0" cellspacing="0" width="100%" frame="box" style="border-color: Black">
                                        <tr>
                                            <td style="width: 33%">
                                                <asp:Label runat="server" ID="lblImageName" Text='<%# "&nbsp;" + Eval("ImageName") %>'></asp:Label>
                                            </td>
                                            <td style="width: 33%">
                                                <asp:Label runat="server" ID="lblImageDate" Text='<%# "&nbsp;" + Eval("ImageDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                            </td>
                                            <td style="width: 33%" align="right">
                                                <div class="alignright item">
                                                    <asp:LinkButton ID="lnkBtnDeleteDoc" CommandArgument='<%# Eval("EquipmentImageID") %>'
                                                        CommandName="DeleteSplashImage" runat="server" ToolTip="Upload Image" OnClientClick="return DeleteConfirmation();"><img src="../../Images/delete_products_icon.png" alt="Delete Image" /></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnImageFileName" runat="server" Value='<%# Eval("ImageFileName") %>' />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </p>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <div class="alignnone">
                </div>
            </div>
        </div>
        <div class="spacer25">
        </div>
    </div>
    <%--Equipment Image Stop--%>
    <%--Service Repair Notes Start--%>
    <div runat="server" id="dvNotesHistory" style="margin-left: 50px; margin-right: 50px;">
        <table class="order_detail" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td colspan="3">
                    <div class="form_table">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box taxt_area clearfix" style="height: 220px">
                            <span class="input_label alignleft" style="height: 218px">
                                <br />
                                Notes/History : </span>
                            <div class="textarea_box alignright">
                                <div class="scrollbar" style="height: 218px">
                                    <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                    </a>
                                </div>
                                <asp:TextBox ID="txtNotesHistory" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                    ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                    <div class="alignnone
    spacer15">
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 30%">
                    <div class="rightalign
    gallery">
                        <asp:LinkButton ID="lnkDummyAddHisotry" class="grey2_btn alignright" runat="server"
                            Style="display: none"></asp:LinkButton>
                        <asp:LinkButton ID="lnkAddHisotry" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright"><span>+ Add Note</span></asp:LinkButton>
                        <at:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="lnkAddHisotry" BackgroundCssClass="modalBackground"
                            DropShadow="true" runat="server" PopupControlID="pnlOrderHistory" CancelControlID="closepopuphistory">
                        </at:ModalPopupExtender>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Panel ID="pnlOrderHistory" runat="server" Style="display: none;">
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
                                                <div id="dv">
                                                    <div class="pp_inline clearfix">
                                                        <div class="form_popup_box">
                                                            <div class="label_bar">
                                                                <span>Notes/History :
                                                                    <br />
                                                                    <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtOrderNotesHistory"
                                                                        runat="server"></asp:TextBox></span>
                                                            </div>
                                                            <div>
                                                                <asp:LinkButton ID="lnkButtonSaveHistory" CommandName="SAVECACE" class="grey2_btn"
                                                                    runat="server" OnClick="lnkButtonSaveHistory_Click"><span>Save Notes</span></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="pp_details clearfix" style="width: 371px;">
                                                    <a href="#" id="closepopuphistory" runat="server" class="pp_close">Close</a>
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
                </td>
            </tr>
        </table>
    </div>
    <%--Service Repair Notes Stop--%>
    <%--Customer Information Start--%>
    <div class="spacer15">
    </div>
    <div runat="server" id="dvCustInfo" class="black_middle order_detail_pad" style="margin-left: 50px;
        margin-right: 50px;" visible="false">
        <table class="order_detail" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="font-size: 15px; width: 70%">
                    <label>
                        Name:
                    </label>
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
                <td style="font-size: 15px; width: 30%">
                    <label>
                        Telephone :
                    </label>
                    <asp:Label ID="lblTelephone" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 15px">
                    <label>
                        Address:
                    </label>
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                </td>
                <td style="font-size: 15px">
                    <label>
                        Mobile :
                    </label>
                    <asp:Label ID="lblMobile" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 15px">
                    <label>
                        Email :
                    </label>
                    <asp:Label ID="lblEmail" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <%--Customer Information Stop--%>
    <%--Parts History Start--%>
    <div runat="server" id="dvPartsHistory" visible="false" style="width: 861px; margin: 0 auto;">
        <asp:GridView ID="gvPartsHistory" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblBillingPartsOrderedID" Text='<%# Eval("BillingPartsOrderedID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="PartNumber">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnPartNumber" runat="server" CommandArgument="PartNumber"
                            CommandName="Sort"><span>Part Number</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPartNumber" Text='<%# Eval("PartNumber") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="15%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="CreatedDate">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnDateUsed" runat="server" CommandArgument="DateUsed" CommandName="Sort"><span>Date Used</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDateUsed" Text='<%# Eval("CreatedDate","{0:MM/dd/yyyy}") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Quantity">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnQuantity" runat="server" CommandArgument="Quantity" CommandName="Sort"><span>Qty</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblQuantity" Text='<%# Eval("Quantity") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="5%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle CssClass="centeralign" />
                    <HeaderTemplate>
                        <span>Description</span>
                    </HeaderTemplate>
                    <HeaderStyle />
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDescriptions" Text='<%# Eval("SummaryDescriptions") %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box leftalign" Width="70%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="spacer25">
        </div>
    </div>
    <%--Parts History End--%>
 
</asp:Content>
