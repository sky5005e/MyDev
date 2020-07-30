<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderPlacedReports.aspx.cs" Inherits="admin_CommunicationCenter_OrderPlacedReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
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
    </div>
</asp:Content>
