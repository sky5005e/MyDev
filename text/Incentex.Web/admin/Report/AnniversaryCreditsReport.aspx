<%@ Page Title="Anniversary Credit Dashboard" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="AnniversaryCreditsReport.aspx.cs" Inherits="admin_Report_AnniversaryCreditsReport" %>

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
                        <td class="formtd" runat="server" id="trPeriod">
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 30%">Select Period :</span> <span class="custom-sel label-sel"
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
        <div id="dvAnniversaryCreditByWorkgroup" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Credits by Workgroup</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtAnniversaryCreditByWorkgroup" runat="server" OnClick="chrtAnniversaryCreditByWorkgroup_Click"
                    BackColor="Transparent" Height="500" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Bar" PostBackValue="#VALX" XValueMember="Text"
                            YValueMembers="Value" IsValueShownAsLabel="true" Label="#VAL{C}" LabelForeColor="#878282"
                            CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            ToolTip="#VAL{C}">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent" BackImageTransparentColor="Transparent"
                            BackImageWrapMode="TileFlipX">
                            <AxisY Title="Anniversary Credit" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" Format="C0" />
                            </AxisY>
                            <AxisX Title="Workgroup" TitleForeColor="#FFFFFF" LineColor="#202020">
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
        <div id="dvAnniversaryCreditByBasestation" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Credits by Station</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtAnniversaryCreditByBasestation" runat="server" OnClick="chrtAnniversaryCreditByBasestation_Click"
                    BackColor="Transparent" Height="600" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Bar" PostBackValue="#VALX" XValueMember="Text"
                            YValueMembers="Value" IsValueShownAsLabel="true" Label="#VAL{C}" LabelForeColor="#878282"
                            CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            ToolTip="#VAL{C}">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent" BackImageTransparentColor="Transparent"
                            BackImageWrapMode="TileFlipX">
                            <AxisY Title="Anniversary Credit" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" Format="C0" />
                            </AxisY>
                            <AxisX Title="Workgroup" TitleForeColor="#FFFFFF" LineColor="#202020">
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
        <div id="dvTopFiftyEmployeeByAnniversaryCredit" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Top 50 Credit Balances</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtTopFiftyEmployeeByAnniversaryCredit" runat="server" BackColor="Transparent"
                    Height="1500" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Bar" XValueMember="Text" YValueMembers="Value"
                            IsValueShownAsLabel="true" Label="#VAL{C}" LabelForeColor="#878282" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            ToolTip="#VAL{C}">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent" BackImageTransparentColor="Transparent"
                            BackImageWrapMode="TileFlipX">
                            <AxisY Title="Anniversary Credit" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" Format="C0" />
                            </AxisY>
                            <Position Height="100" Width="100" />
                            <AxisX Title="Workgroup" TitleForeColor="#FFFFFF" LineColor="#202020">
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
        <div id="dvAnniversaryCreditsAtAGlance" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Summary View of Credits</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtAnniversaryCreditsAtAGlance" OnClick="chrtAnniversaryCreditsAtAGlance_Click"
                    runat="server" BackColor="Transparent" Height="600" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="Text" YValueMembers="Value"
                            PostBackValue="#VALX" ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Pie,
                            PieDrawingStyle=Concave,PieLabelStyle=Outside,PieLineColor=Gray,LabelsRadialLineSize=1, LabelsHorizontalLineSize=1"
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
        <div id="dvAnniversaryCreditsIssuedByMonth" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Credits Issued by Month</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtAnniversaryCreditsIssuedByMonth" runat="server" OnClick="chrtAnniversaryCreditsIssuedByMonth_Click"
                    BackColor="Transparent" Height="600" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Bar" PostBackValue="#VALX" XValueMember="Text"
                            YValueMembers="Value" IsValueShownAsLabel="true" Label="#VAL{C}" LabelForeColor="#878282"
                            CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            ToolTip="#VAL{C}">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent" BackImageTransparentColor="Transparent"
                            BackImageWrapMode="TileFlipX">
                            <AxisY Title="Anniversary Credit" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" Format="C0" />
                            </AxisY>
                            <AxisX Title="Month" TitleForeColor="#FFFFFF" LineColor="#202020">
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
        <div style="float: right; position: relative; top: -30px; right: 10px;">
            Total Anniversary Credit :
            <asp:Label runat="server" ID="lblTotalAnniversaryCredit"></asp:Label>
        </div>
    </div>
</asp:Content>
