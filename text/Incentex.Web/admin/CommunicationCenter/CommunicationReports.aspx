<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CommunicationReports.aspx.cs" Inherits="admin_CommunicationCenter_CommunicationReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .dd2
        {
            width: 202px !important;
        }
        .dd2 .ddChild
        {
            width: 200px !important;
        }
        .collapsibleContainerTitle div
        {
            color: #FFFFFF;
            font-weight: normal;
        }
        .collapsibleContainerContent
        {
            padding: 0px;
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
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleContainerContent").hide();
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
    <div class="form_pad">
        <div class="collapsibleContainer" title="Filter Criteria">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="black_middle">
                <table class="form_table">
                    <tr>
                        <td class="formtd">
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 30%">Store :</span>
                                    <label class="dropimg_width230">
                                        <span class="custom-sel label-sel-small-Product">
                                            <asp:DropDownList ID="ddlCompanyStore" onchange="pageLoad(this,value);" runat="server">
                                            </asp:DropDownList>
                                        </span>
                                    </label>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                        <td class="formtd">
                            <div class="calender_l">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 25%;">Date</span>
                                    <asp:TextBox ID="txtFromDate" runat="server" Style="width: 66%;" CssClass="cal_w datepicker1"></asp:TextBox>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                        <td class="formtd">
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 30%">Select Reports</span>
                                    <label class="dropimg_width230">
                                        <span class="custom-sel label-sel-small-Product">
                                            <asp:DropDownList ID="ddlReportOptions" onchange="pageLoad(this,value);" runat="server">
                                                <asp:ListItem Text="Emails Sent by Hour" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Emails Sent by Day" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Emails Opened by Hour" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Emails Opened by Day" Value="3"></asp:ListItem>
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
                        <td colspan="2">
                        </td>
                        <td class="formtd_r btn_w_block">
                            <asp:LinkButton ID="lnkGeneratesReports" class="gredient_btn" runat="server" ToolTip="Run Report"
                                OnClick="lnkGeneratesReports_Click"><span>Run Report</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div class="spacer25">
        </div>
        <div style="text-align: center; padding-right: 30px; color: #72757C" id="dvTotalDollarValue"
            runat="server">
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </div>
        <div class="spacer25">
        </div>
        <div id="dvChartsTodaysMails" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="ChartsTodaysComm" runat="server" BackColor="Transparent" Height="800px"
                    Width="900px">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Column" XValueMember="Hours" YValueMembers="Count"
                            ToolTip="#VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.6,BarLabelStyle=Outside">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <Position Width="100" Height="100" />
                            <AxisX Title="Hours of the day" TitleForeColor="#FFFFFF" IsStartedFromZero="true"
                                IsLabelAutoFit="false" LineColor="#202020">
                                <LabelStyle ForeColor="#878282" Interval="1" />
                            </AxisX>
                            <AxisY Title="Total number of Email Count" IsStartedFromZero="true" IsLabelAutoFit="false"
                                TitleForeColor="#FFFFFF" LineColor="#202020">
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvChartsByDays" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="ChartbyDays" runat="server" BackColor="Transparent" Height="800px"
                    Width="900px">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Column" XValueMember="Hours" YValueMembers="Count"
                            ToolTip="#VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.6,BarLabelStyle=Outside">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <Position Width="100" Height="100" />
                            <AxisX Title="Days of the week" TitleForeColor="#FFFFFF" IsStartedFromZero="true"
                                IsLabelAutoFit="true" LineColor="#202020">
                                <LabelStyle ForeColor="#878282" Interval="1" />
                            </AxisX>
                            <AxisY Title="Total number of Email Count" IsStartedFromZero="true" IsLabelAutoFit="true"
                                TitleForeColor="#FFFFFF" LineColor="#202020">
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
    </div>
</asp:Content>
