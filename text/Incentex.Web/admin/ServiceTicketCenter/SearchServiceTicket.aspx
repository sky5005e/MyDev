<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="SearchServiceTicket.aspx.cs" Inherits="admin_ServiceTicketCenter_SearchServiceTicket"
    Title="Search Support Ticket" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .form_box span.error
        {
            margin-left: 0%;
        }
        .select_box_pad
        {
            width: 380px;
        }
        .chkPostedNote
        {
            color: #72757C;
            font-size: 15px;
            line-height: 20px;
            text-align: left !important;
            vertical-align: middle;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }

        function CompareDates(fromDate, toDate, separator) {
            var fromDateArr = Array();
            var toDateArr = Array();

            fromDateArr = fromDate.split(separator);
            toDateArr = toDate.split(separator);
            
            var fromMt = fromDateArr[0];
            var fromDt = fromDateArr[1];
            var fromYr = fromDateArr[2];

            var toMt = toDateArr[0];
            var toDt = toDateArr[1];            
            var toYr = toDateArr[2];

            if (fromYr > toYr) {
                return 0;
            }
            else if (fromYr == toYr && fromMt > toMt) {
                return 0;
            }
            else if (fromYr == toYr && fromMt == toMt && fromDt > toDt) {
                return 0;
            }
            else
                return 1;
        }
    </script>

    <script type="text/javascript" language="javascript">
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });

        $(function() {
            $("#<%=lnkBtnSearchNow.ClientID %>").click(function() {
                return validateButtons();
            });
            
            $("#<%=lnkActivityReport.ClientID %>").click(function() {
                return validateButtons();
            });

            $("#ctl00_ContentPlaceHolder1_txtFromDate").change(function() {
                validateDates();
            });

            $("#ctl00_ContentPlaceHolder1_txtToDate").change(function() {
                validateDates();
            });
            
            $(window).scroll(function () {
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
        });

        function ShowMessage(show) {
            if (show == '1') {
                document.getElementById("spanFromDate").style.display = '';
                document.getElementById("spanToDate").style.display = '';
            }
            else {
                document.getElementById("spanFromDate").style.display = 'none';
                document.getElementById("spanToDate").style.display = 'none';
            }
        }
        
        function validateDates() {
            if (!CompareDates($("#ctl00_ContentPlaceHolder1_txtFromDate").val(), $("#ctl00_ContentPlaceHolder1_txtToDate").val(), "/")) {
                ShowMessage('1');
                return false;
            }
            else {
                ShowMessage('0');
                return true;
            }
        }
        
        function validateButtons() {
            if (!validateDates()) {
                    return false;
            }
            else {
                $('#dvLoader').show();
                return true;
            }
        }
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="form_table">
            <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
            <table class="select_box_pad">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="custom-sel">
                                    <asp:DropDownList ID="ddlCompanyName" TabIndex="0" onchange="pageLoad(this,value);"
                                        runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged">
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
                        <div>
                            <asp:UpdatePanel ID="upContactName" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="ddlCompanyName" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="custom-sel">
                                            <asp:DropDownList ID="ddlContactName" TabIndex="1" onchange="pageLoad(this,value);"
                                                runat="server" Enabled="False">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="custom-sel">
                                    <asp:DropDownList ID="ddlServiceTicketStatus" TabIndex="2" onchange="pageLoad(this,value);"
                                        runat="server">
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
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="custom-sel">
                                    <asp:DropDownList ID="ddlServiceTicketOwner" TabIndex="3" onchange="pageLoad(this,value);"
                                        runat="server">
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
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 26%">Ticket Name</span>
                                <asp:TextBox ID="txtServiceTicketName" TabIndex="4" runat="server" CssClass="w_label"></asp:TextBox>
                                <div id="dvTicketName">
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
                                <span class="input_label" style="width: 26%">Ticket #</span>
                                <asp:TextBox ID="txtServiceTicketNumber" TabIndex="5" runat="server" CssClass="w_label"></asp:TextBox>
                                <div id="dvTicketNumber">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Date Needed</span>
                                <asp:TextBox ID="txtDatePromised" runat="server" TabIndex="6" CssClass="datepicker1 w_label"></asp:TextBox>
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
                                <span class="custom-sel">
                                    <asp:DropDownList ID="ddlOpenedby" TabIndex="7" onchange="pageLoad(this,value);"
                                        runat="server">
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
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="custom-sel">
                                    <asp:DropDownList ID="ddlSupplier" TabIndex="8" onchange="pageLoad(this,value);"
                                        runat="server">
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
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="custom-sel">
                                    <asp:DropDownList ID="ddlTypeOfRequest" TabIndex="9" onchange="pageLoad(this,value);"
                                        runat="server">
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
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 26%">Key Word</span>
                                <asp:TextBox ID="txtKeyWord" TabIndex="10" runat="server" CssClass="w_label"></asp:TextBox>
                                <div id="divKeyWord">
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
                                <span class="custom-sel">
                                    <asp:DropDownList ID="ddlSubOwner" TabIndex="11" onchange="pageLoad(this,value);"
                                        runat="server">
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
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">From Date</span>
                                <asp:TextBox ID="txtFromDate" runat="server" TabIndex="12" CssClass="datepicker1 w_label"></asp:TextBox>
                                <span class="error" id="spanFromDate" style="display: none;">From date should be less
                                    than to date.</span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">To Date</span>
                                <asp:TextBox ID="txtToDate" runat="server" TabIndex="13" CssClass="datepicker1 w_label"></asp:TextBox>
                                <span class="error" id="spanToDate" style="display: none;">To date should be greater
                                    than from date.</span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="true">
                            <tr>
                                <td>
                                    <span id="spanChkPostedNote" runat="server" class="custom-checkbox alignleft">
                                        <asp:CheckBox ID="chkPostedNotes" TabIndex="14" runat="server" />
                                    </span>
                                </td>
                                <td class="chkPostedNote">
                                    Show only posted notes in activity report.
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="true">
                            <tr>
                                <td>
                                    <span id="spanNoActivity" runat="server" class="custom-checkbox alignleft">
                                        <asp:CheckBox ID="chkNoActivity" TabIndex="15" runat="server" />
                                    </span>
                                </td>
                                <td class="chkPostedNote">
                                    Search tickets with no owner activity today.
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td class="btn_w_block">
                                    <asp:LinkButton ID="lnkBtnSearchNow" TabIndex="16" class="gredient_btn" runat="server"
                                        ToolTip="Search Now" OnClick="lnkBtnSearchNow_Click"><span>&nbsp;&nbsp;Search Now&nbsp;&nbsp;</span></asp:LinkButton>
                                </td>
                                <td class="btn_w_block">
                                    <asp:LinkButton ID="lnkActivityReport" TabIndex="17" class="gredient_btn" runat="server"
                                        ToolTip="Search Now" OnClick="lnkActivityReport_Click"><span>Activity Report</span></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
