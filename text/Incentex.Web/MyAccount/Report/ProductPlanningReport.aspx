<%@ Page Title="Product Planning Dashboard" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="ProductPlanningReport.aspx.cs" Inherits="MyAccount_Report_ProductPlanningReport" %>

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
        <div id="dvProductSalesByFemaleSize" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Purchase History by Size</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtProductSalesByFemaleSize" runat="server" BackColor="Transparent"
                    Height="3000" Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Bar" XValueMember="Text" YValueMembers="Value"
                            IsValueShownAsLabel="true" Label="#PERCENT" LabelForeColor="#878282" ToolTip="#VALY"
                            CustomProperties="DrawingStyle=Cylinder,PointWidth=0.8,BarLabelStyle=Outside">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <AxisY Title="Unit" IsStartedFromZero="false" IsLabelAutoFit="false" TitleForeColor="#FFFFFF"
                                LineColor="#202020">
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <AxisX Title="Size" TitleForeColor="#FFFFFF" IsStartedFromZero="false" IsLabelAutoFit="false"
                                LineColor="#202020">
                                <LabelStyle ForeColor="#878282" Interval="1" />
                            </AxisX>
                            <Position Height="100" Width="100" Y="1" />
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvProductSalesByColor" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Purchase History by Color</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtProductSalesByColor" runat="server" BackColor="Transparent" Height="600"
                    Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="Text" YValueMembers="Value"
                            ToolTip="#VALX : #VALY" CustomProperties="CollectedToolTip=Other : #VALY, DrawingStyle=Pie, LabelsRadialLineSize=0.4, PieStartAngle=10, LabelsHorizontalLineSize=0.7, PieLabelStyle=Outside, CollectedColor=Green, PieDrawingStyle=Concave, CollectedThreshold=1, PieLineColor=White, CollectedSliceExploded=True, CollectedLabel=Other (#PERCENT)"
                            IsValueShownAsLabel="True" LabelForeColor="135, 130, 130" Label="#VALX (#PERCENT)">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvTopFiftyItemsByRevenue" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Top 50 Items Purchased</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtTopFiftyItemsByRevenue" runat="server" BackColor="Transparent"
                    Height="1500" Width="900">
                    <Series>
                        <asp:Series Name="SeriesRevenue" ChartType="Bar" IsValueShownAsLabel="true" Label="#VAL{C}"
                            Color="Purple" LabelForeColor="#878282" ToolTip="#VALX : #VAL{C}" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside">
                        </asp:Series>
                        <asp:Series Name="SeriesUnit" ChartType="Bar" IsValueShownAsLabel="true" Label="#VALY"
                            LabelForeColor="#878282" ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <AxisY Title="Spend" IsStartedFromZero="false" IsLabelAutoFit="false" TitleForeColor="#FFFFFF"
                                LineColor="#202020">
                                <LabelStyle ForeColor="#878282" Format="C0" />
                            </AxisY>
                            <Position Height="100" Width="100" />
                            <AxisX Title="Master Item" TitleForeColor="#FFFFFF" IsStartedFromZero="false" IsLabelAutoFit="false"
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
        <div id="dvBackOrderInventoryReport" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Back Orders Items</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtBackOrderInventoryReport" runat="server" BackColor="Transparent"
                    Height="6000" Width="900">
                    <Series>
                        <asp:Series Name="SeriesTotalOrder" ChartType="Bar" IsValueShownAsLabel="true" Label="#VALY"
                            Color="Purple" LabelForeColor="#878282" ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside">
                        </asp:Series>
                        <asp:Series Name="SeriesNotShipped" ChartType="Bar" Color="Red" IsValueShownAsLabel="true"
                            Label="#VALY" LabelForeColor="#878282" ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <AxisY Title="Spend" IsLabelAutoFit="false" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <Position Height="100" Width="100" Y="0.2" X="5" />
                            <AxisX Title="Master Item" TitleForeColor="#FFFFFF" IsLabelAutoFit="false" LineColor="#202020">
                                <LabelStyle ForeColor="#878282" Interval="1" Enabled="false" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvInventoryByItem" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Item History Snapshot</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtInventoryByItem" runat="server" BackColor="Transparent" Height="2000"
                    Width="900">
                    <Series>
                        <asp:Series Name="SeriesOnHand" ChartType="Bar" IsValueShownAsLabel="true" Label="#VALY"
                            LabelForeColor="#878282" ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            LegendText="On Hand">
                        </asp:Series>
                        <asp:Series Name="SeriesSold" ChartType="Bar" Color="Red" IsValueShownAsLabel="true"
                            Label="#VALY" LabelForeColor="#878282" ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            LegendText="Usage">
                        </asp:Series>
                        <asp:Series Name="SeriesReturn" ChartType="Bar" Color="Purple" IsValueShownAsLabel="true"
                            Label="#VALY" LabelForeColor="#878282" ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            LegendText="Back Order">
                        </asp:Series>
                        <asp:Series Name="SeriesReOrder" ChartType="Bar" Color="Yellow" IsValueShownAsLabel="true"
                            Label="#VALY" LabelForeColor="#878282" ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            LegendText="Re Order">
                        </asp:Series>
                        <asp:Series Name="SeriesOnOrder" ChartType="Bar" Color="Green" IsValueShownAsLabel="true"
                            Label="#VALY" LabelForeColor="#878282" ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            LegendText="On Order">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                            <AxisY IsMarginVisible="false" Title="Units" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <LabelStyle ForeColor="#878282" />
                            </AxisY>
                            <Position Height="100" Width="100" />
                            <AxisX LineColor="#202020" Title="Master Item" TitleForeColor="#FFFFFF">
                                <LabelStyle ForeColor="#878282" Interval="1" />
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
        <div id="dvCustomerOwnedInventory" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Customer Owned Inventory</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:GridView ID="gvCustomerOwnedInventory" runat="server" AutoGenerateColumns="false"
                    HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                    RowStyle-CssClass="ord_content" OnRowCommand="gvCustomerOwnedInventory_RowCommand"
                    OnRowDataBound="gvCustomerOwnedInventory_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtntemNumber" runat="server" CommandArgument="ItemNumber"
                                    CommandName="Sort"><span>Item #</span></asp:LinkButton>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblItemNumber" CssClass="first" ToolTip='<%# Eval("ItemNumber")%>'
                                    Text='<%# Convert.ToString(Eval("ItemNumber")).Length > 15 ? Convert.ToString(Eval("ItemNumber")).Substring(0, 15).Trim() + "..." : Convert.ToString(Eval("ItemNumber")) %>'></asp:Label>
                                <asp:HiddenField runat="server" ID="hdnRowNumber" Value='<%# Eval("RowNumber")%>' />
                                <asp:HiddenField runat="server" ID="hdnTotalNumber" Value='<%# Eval("TotalNumber")%>' />
                                <div class="corner">
                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnProductDescrption" runat="server" CommandArgument="ProductDescrption"
                                    CommandName="Sort"><span>Description</span></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblProductDescrption" Text='<%# Convert.ToString(Eval("ProductDescrption")).Length > 30 ? Convert.ToString(Eval("ProductDescrption")).Substring(0, 30).Trim() + "..." : Convert.ToString(Eval("ProductDescrption")) %>'
                                    ToolTip='<%# Eval("ProductDescrption") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="25%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnOnHand" runat="server" CommandArgument="OnHand" CommandName="Sort"><span>On-Hand</span></asp:LinkButton>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOnHand" Text='<%# Eval("OnHand") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box centeralign" Width="8%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnPrice" runat="server" CommandArgument="Price" CommandName="Sort"><span>Price</span></asp:LinkButton>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPrice" Text='<%# Eval("Price") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box centeralign" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnUsage" runat="server" CommandArgument="Usage" CommandName="Sort"><span>Usage</span></asp:LinkButton>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblUsage" Text='<%# Eval("Usage")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box centeralign" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnDrawDownValue" runat="server" CommandArgument="DrawDownValue"
                                    CommandName="Sort"><span>Draw Down Value</span></asp:LinkButton>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDrawDownValue" Text='<%# Eval("DrawDownValue")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box centeralign" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnOnHandInventoryValue" runat="server" CommandArgument="OnHandInventoryValue"
                                    CommandName="Sort"><span>On-Hand Value</span></asp:LinkButton>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOnHandInventoryValue" Text='<%# Eval("OnHandInventoryValue")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box centeralign" Width="15%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="spacer10">
                </div>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2" style="width: 40%;" class="rightalign">
                            <strong>Grand Total :</strong>
                        </td>
                        <td style="width: 8%;" class="centeralign">
                            <span>
                                <asp:Label runat="server" ID="lblTotalOnHand" Text="0"></asp:Label></span>
                        </td>
                        <td style="width: 7%;" class="centeralign">
                            <span>
                                <asp:Label runat="server" ID="lblTotalPrice" Text="0"></asp:Label></span>
                        </td>
                        <td style="width: 10%;" class="centeralign">
                            <span>
                                <asp:Label runat="server" ID="lblTotalUsage" Text="0"></asp:Label></span>
                        </td>
                        <td style="width: 15%;" class="centeralign">
                            <span>
                                <asp:Label runat="server" ID="lblTotalDrawDownValue" Text="0"></asp:Label></span>
                        </td>
                        <td style="width: 15%;" class="centeralign">
                            <span>
                                <asp:Label runat="server" ID="lblTotalOnHandValue" Text="0"></asp:Label></span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
    </div>
</asp:Content>
