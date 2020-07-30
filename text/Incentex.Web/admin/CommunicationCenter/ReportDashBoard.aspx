<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ReportDashBoard.aspx.cs" Inherits="admin_CommunicationCenter_ReportDashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_box .custom-sel span.error
        {
            padding: 24px 0;
        }
        .form_table td
        {
            padding-bottom: 5px;
        }
        .label-sel
        {
            width: 64%;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
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
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div class="form_table">
            <table class="dropdown_pad">
                <tr runat="server" id="trStore">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%">Select Company :</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlCompanyStore" onchange="pageLoad(this,value);" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlCompanyStore_SelectedIndexChanged">
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
                                <span class="input_label" style="width: 30%">Select User :</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlEmployee" onchange="pageLoad(this,value);" runat="server">
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
                                <span class="input_label" style="width: 30%">Select Reports :</span>
                               
                                    <span class="custom-sel label-sel">
                                        <asp:DropDownList ID="ddlReportOptions" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
                                
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trDateRange">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%">Date Range :</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlDateRange" onchange="pageLoad(this,value);" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlDateRange_SelectedIndexChanged">
                                        <asp:ListItem Text="Today" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Yesterday" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Last 3 Days" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Last 7 Days" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="Last 30 Days" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="Date Range" Value="Range"></asp:ListItem>
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
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkRunReport" class="gredient_btn" runat="server" OnClick="lnkRunReport_Click"><span><strong>Run Report</strong></span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
