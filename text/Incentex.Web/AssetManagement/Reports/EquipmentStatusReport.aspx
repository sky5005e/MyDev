<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="EquipmentStatusReport.aspx.cs" Inherits="AssetManagement_Reports_EquipmentStatusReport" Title="Equipment Status Report" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
        <div id="dvReport" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>           
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtStatusReport" runat="server" OnClick="chrtStatusReport_Click"
                    BackColor="Transparent" Height="600" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="Text" PostBackValue="#VALX"
                            YValueMembers="Value" ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Pie,
                            PieDrawingStyle=Concave,PieLabelStyle=Outside,PieLineColor=Gray,LabelsRadialLineSize=1, LabelsHorizontalLineSize=0,
                            CollectedThreshold=1,CollectedLabel=Other (#PERCENT),CollectedSliceExploded=True,CollectedColor=Green,CollectedToolTip=Other : #VALY"
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
    
</asp:Content>

