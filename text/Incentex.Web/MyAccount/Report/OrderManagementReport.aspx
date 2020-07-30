<%@ Page Title="Order Management Dashboard" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="OrderManagementReport.aspx.cs" Inherits="MyAccount_Report_OrderManagementReport" %>

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
        .custom-checkbox input
        {
            height: 20px;
            width: 20px;
        }
        .custom-checkbox_checked input
        {
            height: 20px;
            width: 20px;
        }
        .g_box.checktable_supplier span.custom-checkbox, .g_box.checktable_supplier span.custom-checkbox_checked
        {
            height: 26px;
        }
        .checktable_supplier span
        {
            margin: none;
        }
        .g_box.checktable_supplier span.custom-checkbox span.checkbox-on
        {
            background: url("../../images/check-box-img.jpg") no-repeat scroll 7px 0px transparent !important;
            border-bottom: none;
        }
        .g_box.checktable_supplier span.custom-checkbox span.checkbox-off
        {
            background: url("../../images/check-box-img.jpg") no-repeat scroll 7px -22px transparent !important;
            border-bottom: none;
        }
        .g_box.checktable_supplier span.custom-checkbox_checked span.checkbox-on
        {
            background: url("../../images/check-box-img.jpg") no-repeat scroll 7px -22px transparent !important;
            border-bottom: none;
        }
        .g_box.checktable_supplier span.custom-checkbox_checked span.checkbox-off
        {
            background: url("../../images/check-box-img.jpg") no-repeat scroll 7px 0px transparent !important;
            border-bottom: none;
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
        <div id="dvOrderAtAGlance" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Order Status Snapshot</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <asp:Chart ID="chrtOrderAtAGlance" runat="server" BackColor="Transparent" Height="600"
                    Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="Status" YValueMembers="Count"
                            ToolTip="#VALX : #VALY" CustomProperties="DrawingStyle=Pie,
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
        <div id="dvEmployeePayrollDeduct" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Employee Payroll Deduct</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <div style="text-align: right; margin-bottom: 10px;">
                    Total Records:
                    <asp:Label runat="server" ID="lblRecordCounter" Text="0"></asp:Label>
                </div>
                <div>
                    <asp:GridView ID="gvEmployeePayrollDeduct" runat="server" AutoGenerateColumns="false"
                        HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                        RowStyle-CssClass="ord_content" OnRowDataBound="gvEmployeePayrollDeduct_RowDataBound"
                        OnRowCommand="gvEmployeePayrollDeduct_RowCommand">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnOrderNumber" runat="server" CommandArgument="OrderNumber"
                                        CommandName="Sort"><span>Order #</span></asp:LinkButton>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="first">
                                        <asp:LinkButton runat="server" ID="lnkbtnOrderNumber" Text='<%# Eval("OrderNumber")%>'
                                            CommandName="OrderDetail" CommandArgument='<%# Eval("OrderID")%>'></asp:LinkButton>
                                        <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                                    </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEmployeeNo" runat="server" CommandArgument="EmployeeNo"
                                        CommandName="Sort"><span>Employee #</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEmployeeNo" Text='<%# Eval("EmployeeNo")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEmployeeName" runat="server" CommandArgument="EmployeeName"
                                        CommandName="Sort"><span>Employee Name</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEmployeeName" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="40%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEDPAmount" runat="server" CommandArgument="EDPAmount" CommandName="Sort"><span>EDP Amount</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEDPAmount" Text='<%# Eval("EDPAmount")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>&nbsp;</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="custom-checkbox alignleft btn_space" id="spnEDPReceived" runat="server">
                                        <asp:CheckBox ID="chkEDPReceived" runat="server" AutoPostBack="true" OnCheckedChanged="chkEDPReceived_CheckedChanged" />
                                    </span>
                                    <asp:HiddenField ID="hdnEDPReceived" runat="server" Value='<%# Eval("IsEDPReceived") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box checktable_supplier true" Width="5%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <div id="pagingtable" runat="server" class="alignright pagging">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click">
                        </asp:LinkButton>
                        <span>
                            <asp:DataList ID="DataList2" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="DataList2_ItemCommand"
                                OnItemDataBound="DataList2_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList></span>
                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
        <div id="dvBackOrder" runat="server" visible="false">
            <%--<div>
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="grey2_btn alignright" OnClick="lnkPrint_Click"><span>
                    Print</span></asp:LinkButton>
            </div>--%>
            <div class="header_bg">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Back Order Report</span>
                </div>
            </div>
            <div style="text-align: right; margin-bottom: 10px;">
                Record Count:
                <asp:Label runat="server" ID="lblBackOrderCount" Text="0"></asp:Label>
            </div>
            <asp:Repeater ID="rpBackOrderReport" runat="server">
                <ItemTemplate>
                    <div class="black_top_co">
                        <span>&nbsp;</span></div>
                    <div class="black_middle" style="text-align: center;">
                        <table class="order_detail">
                            <tr>
                                <td width="50%">
                                    <table>
                                        <tr>
                                            <td>
                                                <label>
                                                    Company:
                                                </label>
                                                <asp:Label runat="server" ID="lblCompanyName" Text='<%# Eval("CompanyName")%>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Order Number:
                                                </label>
                                                <asp:HyperLink runat="server" ID="hypOrderNumber" Text='<%# Eval("OrderNumber")%>'
                                                    ForeColor="#72757C" Target="_blank" NavigateUrl='<%# "~/OrderManagement/OrderDetail.aspx?id=" + Eval("OrderID")%>'></asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="50%">
                                    <table>
                                        <tr>
                                            <td>
                                                <label>
                                                    Ordered By:
                                                </label>
                                                <asp:Label ID="lblOrderedBy" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Order Date:
                                                </label>
                                                <asp:Label ID="lblOrderDate" runat="server" Text='<%# Eval("OrderDate")%>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div class="spacer10">
                        </div>
                        <table>
                            <tr>
                                <td class="alignleft">
                                    <span>Back Order Items</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("OrderID") %>' />
                                    <asp:GridView ID="grdItemDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
                                     <EmptyDataTemplate>
                                        <span style="color: Red; text-align: center;">No Records Found</span>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <span>Item Number</span>
                                                <div class="corner">
                                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                </div>
                                            </HeaderTemplate>
                                            <HeaderStyle />
                                            <ItemTemplate>
                                                <asp:Label CssClass="first" runat="server" ID="lblBackOrderItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                                <asp:HiddenField ID="hdnMyShoppingCartiD" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />
                                                <div class="corner">
                                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="25%" />
                                        </asp:TemplateField>
                                       <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span>Quantity Ordered</span>
                                                </HeaderTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="centeralign"/>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblQuantityOrdered" Text='<%# Convert.ToString(Eval("QuantityOrdered")) != "" ? Eval("QuantityOrdered"): 0 %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box centeralign" Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span>Quantity Shipped</span>
                                                </HeaderTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblQuantityShipped" Text='<%# Convert.ToString(Eval("QuantityShipped")) != "" ? Eval("QuantityShipped") : 0  %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box centeralign" Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span>Back-Order Quantity</span>
                                                </HeaderTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblBackOrderQuantity" Text='<%# Convert.ToString(Eval("BackOrderQuantity")) != "" ? Eval("BackOrderQuantity"):0 %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box centeralign" Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span>Expected Arrival Date</span>
                                                    <div class="corner">
                                                        <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                    </div>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblArrivalDate" Text='<%# Convert.ToString(Eval("BackOrderedUntil")) != "" ? Convert.ToDateTime(Eval("BackOrderedUntil")).ToString("MM/dd/yyyy") : "" %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box centeralign emptyspan" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                            <HeaderTemplate>
                                                <span>Back Ordered Until</span>
                                                <div class="corner">
                                                    <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                </div>
                                            </HeaderTemplate>
                                            <HeaderStyle CssClass="centeralign" />
                                            <ItemTemplate>
                                                <span class="calender_l"">
                                                    <asp:TextBox ID="txtBackOrderDate" Style="background-color: #303030; border: medium none;
                                                        color: #ffffff; width: 78px; padding: 2px" runat="server" CssClass="cal_w datepicker1 min_w"
                                                        Text='<%# Convert.ToString(Eval("BackOrderedUntil")) != "" ? Convert.ToDateTime(Eval("BackOrderedUntil")).ToString("MM/dd/yyyy") : "" %>'></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hdnBackOrderedUntil" Value='<%# Convert.ToString(Eval("BackOrderedUntil")) != "" ? Convert.ToDateTime(Eval("BackOrderedUntil")).ToString("MM/dd/yyyy") : "" %>' />
                                                </span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <div class="spacer10">
                        </div>
                    </div>
                    <div class="black_bot_co">
                        <span>&nbsp;</span></div>
                    <div class="spacer15">
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
