<%@ Page Title="Sales Summary Report" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="SalesSummaryReport.aspx.cs" Inherits="MyAccount_Report_SalesSummaryReport" %>

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

            $("#ctl00_ContentPlaceHolder1_lnkSubmitRequest").click(function() {
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
                        <td class="formtd" runat="server" id="trFromDate" visible="false">
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
                        <td class="formtd_r" runat="server" id="trToDate" visible="false">
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
                    </tr>
                    <tr>
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
                        <td class="formtd">
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
                        <td class="formtd_r">
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
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="formtd_r btn_w_block">
                            <asp:LinkButton ID="lnkSubmitRequest" class="gredient_btn" runat="server" OnClick="lnkSubmitRequest_Click"><span>Run Report</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div class="spacer25">
        </div>
        <div id="dvSalesByStation" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Spend By Station Location</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtSalesByStation" runat="server" BackColor="Transparent" Height="600" OnClick="chrtSalesByStation_Click"
                    Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="BaseStation" PostBackValue="#VALX"
                            YValueMembers="OrderAmount" ToolTip="#VALX : #VAL{C}" CustomProperties="DrawingStyle=Pie,
                            PieDrawingStyle=Concave,PieLabelStyle=Outside,PieLineColor=Gray,LabelsRadialLineSize=1, LabelsHorizontalLineSize=0,
                            CollectedThreshold=2,CollectedLabel=Other (#PERCENT),CollectedSliceExploded=True,CollectedColor=Green,CollectedToolTip=Other : #VAL{C}"
                            IsValueShownAsLabel="True" LabelForeColor="135, 130, 130" Label="#VALX (#PERCENT)"
                            Font="Microsoft Sans Serif, 8pt">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent" Area3DStyle-Enable3D="false"
                            BackImageTransparentColor="Transparent" BackImageWrapMode="TileFlipX">
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvSalesByWorkgroup" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Spend By Workgroup</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtSalesByWorkgroup" runat="server" BackColor="Transparent" Height="400"
                    Width="750">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Bar" XValueMember="Workgroup" YValueMembers="OrderAmount"
                            IsValueShownAsLabel="true" Label="#VAL{C}" LabelForeColor="#878282" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            ToolTip="#VAL{C}">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent" BackImageTransparentColor="Transparent"
                            BackImageWrapMode="TileFlipX">
                            <AxisY Title="Spend" TitleForeColor="#FFFFFF" LineColor="#202020" IsLabelAutoFit="True">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" Format="C0" />
                            </AxisY>
                            <AxisX Title="Workgroup" IsStartedFromZero="false" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvSalesByWorkgroupPer" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Spend By Workgroup %</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtSalesByWorkgroupPer" runat="server" BackColor="Transparent" Height="550"
                    Width="800">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="Workgroup" YValueMembers="OrderAmount"
                            ToolTip="#VALX : #VAL{C}" CustomProperties="DrawingStyle=Pie, LabelsRadialLineSize=1, LabelsHorizontalLineSize=0, PieLabelStyle=Outside, PieDrawingStyle=Concave, PieLineColor=Gray,
                            CollectedThreshold=1,CollectedLabel=Other (#PERCENT),CollectedSliceExploded=True,CollectedColor=Green,CollectedToolTip=Other : #VAL{C}"
                            IsValueShownAsLabel="True" Label="#VALX (#PERCENT)" Font="Microsoft Sans Serif, 8pt"
                            LabelForeColor="135, 130, 130">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent" BackImageTransparentColor="Transparent"
                            BackImageWrapMode="TileFlipX">
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvSalesByYear" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Total Program Spend Since Start Date</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtSalesByYear" runat="server" BackColor="Transparent" Height="400"
                    Width="500">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Column" XValueMember="Year" YValueMembers="OrderAmount"
                            IsValueShownAsLabel="true" LabelForeColor="#878282" Label="#VAL{C}" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.5,BarLabelStyle=Outside"
                            ToolTip="#VAL{C}">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent" BackImageTransparentColor="Transparent"
                            BackImageWrapMode="TileFlipX">
                            <AxisY Title="Spend" IsStartedFromZero="false" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <AxisX Title="Year" IsStartedFromZero="false" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvSalesByCompanyVSEmployee" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Total Company Spend VS. Employee Spend</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtSalesByCompanyVSEmployee" runat="server" BackColor="Transparent"
                    Height="600" Width="900">
                    <Series>
                        <asp:Series Name="SeriesForEmployee" ChartType="Column" ToolTip="#VAL{C}" Legend="Legend1"
                            LegendText="Employee Purchases" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside">
                        </asp:Series>
                        <asp:Series Name="SeriesForCompany" ChartType="Column" ToolTip="#VAL{C}" Legend="Legend1"
                            LegendText="Company Purchases" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <AxisY Title="Spend" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <AxisX Title="Month" IsStartedFromZero="false" TitleForeColor="#FFFFFF" LineColor="#202020"
                                IsLabelAutoFit="true">
                                <LabelStyle ForeColor="#878282" Interval="1" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                    <Legends>
                        <asp:Legend Name="Legend1" LegendStyle="Column" BackGradientStyle="TopBottom" BackSecondaryColor="#B6D6EC"
                            BorderDashStyle="Solid" BorderWidth="2">
                        </asp:Legend>
                    </Legends>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvSalesByCompany" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Total Company Spend By Month</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtSalesByCompany" runat="server" BackColor="Transparent" Height="500"
                    Width="900">
                    <Series>
                        <asp:Series Name="SeriesForCompany" ChartType="Column" XValueMember="Month" YValueMembers="OrderAmount"
                            ToolTip="#VAL{C}" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <AxisY Title="Spend" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <AxisX Title="Month" IsStartedFromZero="false" TitleForeColor="#FFFFFF" LineColor="#202020"
                                IsLabelAutoFit="true">
                                <LabelStyle ForeColor="#878282" Interval="1" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvSalesByPersonalProtectiveEquiptment" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Personal Protective Equiptment</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtSalesByPersonalProtectiveEquiptment" runat="server" BackColor="Transparent" OnClick="chrtSalesByPersonalProtectiveEquiptment_Click"
                    Height="600" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="BaseStation" YValueMembers="OrderAmount" PostBackValue="#VALX"
                            ToolTip="#VALX : #VAL{C}" CustomProperties="DrawingStyle=Pie,
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
        <div id="dvSalesBySpanx" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Spanx Sales Summary</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtSalesBySpanx" runat="server" BackColor="Transparent"
                    Height="600" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="BaseStation" YValueMembers="OrderAmount"
                            ToolTip="#VALX : #VAL{C}" CustomProperties="DrawingStyle=Pie,
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
          <div style="float: right; position: relative; top: -30px; right: 10px;" id="dvTotalSpend" runat="server" visible="false">
            Total Spend :
            <asp:Label runat="server" ID="lblTotalSpend"></asp:Label>
        </div>
    </div>
</asp:Content>
