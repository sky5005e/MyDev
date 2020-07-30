<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="IssuancePolicyReports.aspx.cs" Inherits="admin_Report_IssuancePolicyReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="dvchrtIssuancePolicyName" runat="server" visible="false">
        <asp:Chart ID="chrtIssuancePolicyName" runat="server" BackColor="Transparent" Height="800"
            Width="900" OnClick="chrtIssuancePolicyName_Click">
            <Series>
                <asp:Series Name="Series1" ChartType="Pie" XValueMember="IssuanceProgramName" PostBackValue="#VALX"
                    YValueMembers="TotalCount" ToolTip="#VALX : #VALY"
                    IsValueShownAsLabel="True" LabelForeColor="135, 130, 130" Label="#VALX" Font="Microsoft Sans Serif, 8pt">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                    <Position Width="100" Height="100" />
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>
    <div id="dvStorePolicyName" runat="server" visible="false">
        <asp:Chart ID="ChrtStorePolicyName" runat="server" BackColor="Transparent" Height="800"
            Width="900" OnClick="ChrtStorePolicyName_Click">
            <Series>
                <asp:Series Name="Series1" ChartType="Pie" XValueMember="CompanyName" PostBackValue="#VALX"
                    YValueMembers="TotalCount" ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Pie,
                            PieDrawingStyle=Concave,PieLabelStyle=Outside,PieLineColor=Gray,LabelsRadialLineSize=1, LabelsHorizontalLineSize=0,
                            CollectedThreshold=1,CollectedLabel=Other,CollectedSliceExploded=True,CollectedColor=Green,CollectedToolTip=Other : #VALY"
                    IsValueShownAsLabel="True" LabelForeColor="135, 130, 130" Label="#VALX" Font="Microsoft Sans Serif, 8pt">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                    <Position Width="100" Height="100" />
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
