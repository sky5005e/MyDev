<%@ Page Title="Report DashBoard" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="ReportDashBoard.aspx.cs" Inherits="MyAccount_Report_ReportDashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_box .custom-sel span.error
        {
            padding: 24px 0;
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
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlParentReport: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlChildReport: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlMasterItem: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlParentReport: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "category") },
                        ctl00$ContentPlaceHolder1$ddlChildReport: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "sub-category") },
                        ctl00$ContentPlaceHolder1$ddlMasterItem: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "master item") }
                    },
                    errorPlacement: function(error, element) {
                        error.insertAfter(element);
                    }
                });

            });

            $("#ctl00_ContentPlaceHolder1_lnkSubmitRequest").click(function() {
                $('#dvLoader').show();
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });

        });
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div class="form_table">
            <table class="dropdown_pad">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%">Select Report :</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlParentReport" onchange="pageLoad(this,value);" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlParentReport_SelectedIndexChanged">
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
                                <span class="input_label" style="width: 30%">Select Sub-Report :</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlChildReport" OnSelectedIndexChanged="ddlChildReport_SelectedIndexChanged"
                                        onchange="pageLoad(this,value);" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trPeriod">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%">Select Period :</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlPeriod" onchange="pageLoad(this,value);" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged">
                                        <asp:ListItem Text="Today" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Yesterday" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Last 3 Days" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Last 7 Days" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="Last 30 Days" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="Date Range" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trFromDate" visible="false">
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%;">From Date</span>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="cal_w datepicker1"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trToDate" visible="false">
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%;">To Date</span>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="cal_w datepicker1"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trMasterItem" visible="false">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%">Select Item:</span>
                                <label class="dropimg_width230">
                                    <span class="custom-sel label-sel-small-Product">
                                        <asp:DropDownList ID="ddlMasterItem" onchange="pageLoad(this,value);" runat="server"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkSubmitRequest" class="gredient_btn" runat="server" OnClick="lnkSubmitRequest_Click"><span><strong>Search Now</strong></span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
