<%@ Page Title="Employee Information Report" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="EmployeeDataReport.aspx.cs" Inherits="MyAccount_Report_EmployeeDataReport" %>

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
                        <td class="formtd" runat="server" id="trPeriod">
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
                        <td runat="server" id="trFromDate" visible="false" class="formtd">
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
        <div id="dvEmployeeByStation" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Employees by Station</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtEmployeeByStation" runat="server" BackColor="Transparent" Height="650"
                    Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="Text" YValueMembers="Value"
                            ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Pie,
                            PieDrawingStyle=Concave,PieLabelStyle=Outside,PieLineColor=Gray,LabelsRadialLineSize=1, LabelsHorizontalLineSize=1,
                            CollectedThreshold=1.8,CollectedLabel=Other (#PERCENT),CollectedSliceExploded=True,CollectedColor=Green,CollectedToolTip=Other : #VALY"
                            IsValueShownAsLabel="True" LabelForeColor="135, 130, 130" Label="#VALX (#PERCENT)">
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
        <div id="dvEmployeeByWorkgroup" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Employee Totals by Workgroup</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtEmployeeByWorkgroup" runat="server" BackColor="Transparent" Height="600"
                    Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Bar" XValueMember="Text" YValueMembers="Value"
                            IsValueShownAsLabel="true" Label="#VALY" LabelForeColor="#878282" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            ToolTip="#VALY">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent" BackImageTransparentColor="Transparent"
                            BackImageWrapMode="TileFlipX">
                            <AxisY Title="Employees" IsStartedFromZero="false" IsLabelAutoFit="false" TitleForeColor="#FFFFFF"
                                LineColor="#202020">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <AxisX Title="Workgroup" TitleForeColor="#FFFFFF" IsStartedFromZero="false" IsLabelAutoFit="false"
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
        <div id="dvEmployeeByGender" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Employee Totals by Gender</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtEmployeeByGender" runat="server" BackColor="Transparent" Height="400"
                    Width="600">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="Text" YValueMembers="Value"
                            ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Pie,PieDrawingStyle=Concave,PieLabelStyle=Outside,
                            PieLineColor=Gray,LabelsRadialLineSize=1, LabelsHorizontalLineSize=1" IsValueShownAsLabel="True"
                            LabelForeColor="135, 130, 130" Label="#VALX (#PERCENT)">
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
        <div id="dvEmployeeGenderByWorkgroup" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Gender by Workgroup</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtEmployeeGenderByWorkgroup" runat="server" BackColor="Transparent"
                    Height="800" Width="900">
                    <Series>
                        <asp:Series Name="SeriesForMale" ChartType="Bar" ToolTip="#VALY" Color="#4CC7F0"
                            Legend="Legend1" LegendText="Male" IsValueShownAsLabel="true" LabelForeColor="#878282"
                            CustomProperties="DrawingStyle=Cylinder,PointWidth=0.8,BarLabelStyle=Outside">
                        </asp:Series>
                        <asp:Series Name="SeriesForFemale" Color="#EB26E5" ChartType="Bar" ToolTip="#VALY"
                            Legend="Legend1" IsValueShownAsLabel="true" LabelForeColor="#878282" LegendText="Female"
                            CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <AxisY Title="Employees" TitleForeColor="#FFFFFF" LineColor="#202020" IsStartedFromZero="false">
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <AxisX Title="Workgroup" TitleForeColor="#FFFFFF" LineColor="#202020" IsLabelAutoFit="false"
                                Interval="1">
                                <LabelStyle ForeColor="#878282" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                    <Legends>
                        <asp:Legend Name="Legend1" LegendStyle="Table" BackGradientStyle="TopBottom" BackSecondaryColor="#B6D6EC"
                            BorderDashStyle="Solid" BorderWidth="2">
                        </asp:Legend>
                    </Legends>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvEmployeeByStatus" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Employment Status</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtEmployeeByStatus" runat="server" BackColor="Transparent" Height="400"
                    Width="600">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="Text" YValueMembers="Value"
                            ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Pie,PieDrawingStyle=Concave,PieLabelStyle=Outside,
                            PieLineColor=Gray,LabelsRadialLineSize=1, LabelsHorizontalLineSize=1" IsValueShownAsLabel="True"
                            LabelForeColor="135, 130, 130" Label="#VALX (#PERCENT)">
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
    </div>
</asp:Content>
