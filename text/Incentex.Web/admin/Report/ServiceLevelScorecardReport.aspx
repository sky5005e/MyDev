<%@ Page Title="Service Level Scorecard Dashboard" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="ServiceLevelScorecardReport.aspx.cs" Inherits="admin_Report_ServiceLevelScorecardReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
           
            $(".showloader").click(function(){
                $('#dvLoader').show();
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
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
                                    <span class="custom-sel label-sel">
                                        <asp:DropDownList ID="ddlCompanyStore" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
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
                                    <span class="input_label" style="width: 30%">Select Period</span> <span class="custom-sel label-sel"
                                        style="width: 60%;">
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
                        <td runat="server" id="trFromDate" visible="false" class="formtd_r">
                            <div class="calender_l">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 25%;">From Date</span>
                                    <asp:TextBox ID="txtFromDate" runat="server" Style="width: 66%;" CssClass="cal_w datepicker1"></asp:TextBox>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td runat="server" id="trToDate" visible="false" class="formtd">
                            <div class="calender_l">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 25%;">To Date</span>
                                    <asp:TextBox ID="txtToDate" runat="server" Style="width: 66%;" CssClass="cal_w datepicker1"></asp:TextBox>
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
                                    <span class="custom-sel label-sel">
                                        <asp:DropDownList ID="ddlWorkgroup" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                        <td class="formtd_r">
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="custom-sel label-sel">
                                        <asp:DropDownList ID="ddlBasestation" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="formtd">
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="custom-sel label-sel">
                                        <asp:DropDownList ID="ddlGender" runat="server" onchange="pageLoad(this,value);">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                        <td class="formtd">
                        </td>
                        <td class="formtd_r btn_w_block">
                            <asp:LinkButton ID="lnkSubmitRequest" Style="margin: 0px;" class="gredient_btn1"
                                runat="server" OnClick="lnkSubmitRequest_Click">
                            <img src="../Incentex_Used_Icons/run_report_icn.png" alt="Run Report"/><span>Run Report</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div class="spacer25">
        </div>
        <div id="dvAverageShipTime" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Ship Time for In-Stock Merchandise</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtAverageShipTime" runat="server" BackColor="Transparent" Height="600"
                    Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Bar" XValueMember="Days" YValueMembers="Count"
                            ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            IsValueShownAsLabel="True" LabelForeColor="135, 130, 130" Label="#PERCENT">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent" BackImageTransparentColor="Transparent"
                            BackImageWrapMode="TileFlipX">
                            <AxisY Title="Order Count" IsStartedFromZero="true" IsLabelAutoFit="false" TitleForeColor="#FFFFFF"
                                LineColor="#202020">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <AxisX Title="Days" TitleForeColor="#FFFFFF" IsStartedFromZero="true" IsLabelAutoFit="false"
                                LineColor="#202020">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" Interval="1" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvPurchasesVSReturns" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Returns vs. Orders Placed</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtPurchasesVSReturns" runat="server" BackColor="Transparent" Height="5000"
                    Width="900">
                    <Series>
                        <asp:Series Name="SeriesForPurchase" ChartType="Bar" IsValueShownAsLabel="true" Label="#VALY"
                            LabelForeColor="#878282" ToolTip="#PERCENT" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.8,BarLabelStyle=Outside">
                        </asp:Series>
                        <asp:Series Name="SeriesForReturn" Color="Red" ChartType="Bar" IsValueShownAsLabel="true"
                            Label="#VALY" LabelForeColor="#878282" ToolTip="#PERCENT" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.8,BarLabelStyle=Outside">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <AxisY Title="Unit" IsStartedFromZero="false" IsLabelAutoFit="false" TitleForeColor="#FFFFFF"
                                LineColor="#202020">
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <Position Height="100" Width="100" Y="0.2" />
                            <AxisX Title="Size" TitleForeColor="#FFFFFF" IsStartedFromZero="false" IsLabelAutoFit="false"
                                LineColor="#202020">
                                <LabelStyle ForeColor="#878282" Interval="1" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvUnitsVSReturnsSummary" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Returns vs. Units Purchased</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtUnitsVSReturnsSummary" runat="server" BackColor="Transparent"
                    Height="400" Width="900">
                    <Series>
                        <asp:Series Name="SeriesTotalOrder" ChartType="Bar" IsValueShownAsLabel="true" Label="#VALY"
                            Color="Purple" LabelForeColor="#878282" ToolTip="Total Solds : #VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.8,BarLabelStyle=Outside">
                        </asp:Series>
                        <asp:Series Name="SeriesTotalReturn" ChartType="Bar" Color="Red" IsValueShownAsLabel="true"
                            Label="#VALY" LabelForeColor="#878282" ToolTip="Total Returns : #VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.8,BarLabelStyle=Outside">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <AxisY Title="Units" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <AxisX LineColor="#202020">
                                <LabelStyle Enabled="false" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvHelpTicketsbyLocation" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Help Tickets by Location</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtHelpTicketsbyLocation" runat="server" BackColor="Transparent" OnClick="chrtHelptickets_Click"
                    Height="600" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="BaseStation" PostBackValue="#VALX" YValueMembers="TicketCount"
                            ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Pie,
                            PieDrawingStyle=Concave,PieLabelStyle=Outside,PieLineColor=Gray,LabelsRadialLineSize=1, LabelsHorizontalLineSize=0,
                            CollectedThreshold=1,CollectedLabel=Other (#PERCENT),CollectedSliceExploded=True,CollectedColor=Green,CollectedToolTip=Other : #VAL{C}"
                            IsValueShownAsLabel="True" LabelForeColor="135, 130, 130" Label="#VALX (#PERCENT)"
                            Font="Microsoft Sans Serif, 8pt">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <Position Width="100" Height="100" />
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
           <div id="dvHelpTicketsbyType" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Help Tickets by Type</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtHelpticketsByType" runat="server" BackColor="Transparent" OnClick="chrtHelptickets_Click"
                    Height="600" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="Type" PostBackValue="#VALX" YValueMembers="TicketCount"
                            ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Pie,
                            PieDrawingStyle=Concave,PieLabelStyle=Outside,PieLineColor=Gray,LabelsRadialLineSize=1, LabelsHorizontalLineSize=0,
                            CollectedThreshold=1,CollectedLabel=Other (#PERCENT),CollectedSliceExploded=True,CollectedColor=Green,CollectedToolTip=Other : #VAL{C}"
                            IsValueShownAsLabel="True" LabelForeColor="135, 130, 130" Label="#VALX (#PERCENT)"
                            Font="Microsoft Sans Serif, 8pt">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <Position Width="100" Height="100" />
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvHelpTicketsbyWorkgroup" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Help Tickets by Workgroup</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtHelpTicketsbyWorkgroup" runat="server" BackColor="Transparent" OnClick="chrtHelptickets_Click"
                    Height="600" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Bar" IsValueShownAsLabel="true" XValueMember="Workgroup"
                            YValueMembers="TicketCount" LabelForeColor="#878282" ToolTip="#VALX : #VALY" PostBackValue="#VALX"
                            Label="#PERCENT" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.8,BarLabelStyle=Outside">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <AxisY Title="Ticket Count" IsStartedFromZero="false" IsLabelAutoFit="false" TitleForeColor="#FFFFFF"
                                LineColor="#202020">
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <AxisX Title="Workgroup" TitleForeColor="#FFFFFF" IsStartedFromZero="false" IsLabelAutoFit="false"
                                LineColor="#202020">
                                <LabelStyle ForeColor="#878282" Interval="1" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvHelpTicketsUsersvsTotalEmployees" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Help Tickets Users vs. Total Employees</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtHelpTicketsUsersvsTotalEmployees" runat="server" BackColor="Transparent"
                    Height="400" Width="900">
                    <Series>
                        <asp:Series Name="SeriesTotalEmployee" ChartType="Bar" IsValueShownAsLabel="true"
                            Label="#VALY" LabelForeColor="#878282" ToolTip="Total Employee : #VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.8,BarLabelStyle=Outside">
                        </asp:Series>
                        <asp:Series Name="SeriesTotalTicketEmployee" ChartType="Bar" Color="Purple" IsValueShownAsLabel="true"
                            Label="#VALY" LabelForeColor="#878282" ToolTip="Total Ticket Employee : #VALY"
                            CustomProperties="DrawingStyle=Cylinder,PointWidth=0.8,BarLabelStyle=Outside">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <AxisY TitleForeColor="#FFFFFF" LineColor="#202020">
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <AxisX LineColor="#202020">
                                <LabelStyle Enabled="false" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvTop25EmployeesSubmittingTickets" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Top 25 Employees Submitting Tickets</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:GridView ID="gvTop25EmployeesSubmittingTickets" runat="server" AutoGenerateColumns="false"
                    HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                    RowStyle-CssClass="ord_content">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Company Name</span>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="first">
                                    <%#  Eval("CompanyName") %></span>
                                <div class="corner">
                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="25%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Full Name</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span>
                                    <%#  Eval("User") %></span>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" Width="25%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Email</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span>
                                    <%# Eval("Email") %></span>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="30%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Total Tickets</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("TicketCount")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" Width="20%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvShipCompleteReport" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Ship Complete Report</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtShipCompleteReportChart" runat="server" OnClick="chrtShipCompleteReportChart_Click"
                    BackColor="Transparent" Height="600" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="Days" PostBackValue="#VALX"
                            YValueMembers="Count" ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Pie,
                            PieDrawingStyle=Concave,PieLabelStyle=Outside,PieLineColor=Gray,LabelsRadialLineSize=1, LabelsHorizontalLineSize=0,
                            CollectedThreshold=1,CollectedLabel=Other (#PERCENT),CollectedSliceExploded=True,CollectedColor=Green,CollectedToolTip=Other : #VAL{C}"
                            IsValueShownAsLabel="True" LabelForeColor="135, 130, 130" Label="#VALX (#PERCENT)"
                            Font="Microsoft Sans Serif, 8pt">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="caShipCompleteChartArea" BackColor="Transparent">
                            <Position Width="100" Height="100" />
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div style="float: right; position: relative; top: -30px; right: 10px;">
            <asp:Label runat="server" ID="lblDisplayText"></asp:Label>
        </div>
    </div>
</asp:Content>
