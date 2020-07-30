<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="PendingOrderList.aspx.cs" Inherits="admin_Report_PendingOrderList"
    Title="Untitled Page" %>

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
        .order_detail td
        {
            font-size: 12px;
        }
        .order_detail label
        {
            padding-left: 0px;
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
            background: url( "../../images/check-box-img.jpg" ) no-repeat scroll 7px 0px transparent !important;
            border-bottom: none;
        }
        .g_box.checktable_supplier span.custom-checkbox span.checkbox-off
        {
            background: url( "../../images/check-box-img.jpg" ) no-repeat scroll 7px -22px transparent !important;
            border-bottom: none;
        }
        .g_box.checktable_supplier span.custom-checkbox_checked span.checkbox-on
        {
            background: url( "../../images/check-box-img.jpg" ) no-repeat scroll 7px -22px transparent !important;
            border-bottom: none;
        }
        .g_box.checktable_supplier span.custom-checkbox_checked span.checkbox-off
        {
            background: url( "../../images/check-box-img.jpg" ) no-repeat scroll 7px 0px transparent !important;
            border-bottom: none;
        }
        .b_box.checktable_supplier span.custom-checkbox span.checkbox-on
        {
            background: url( "../../images/check-box-img.jpg" ) no-repeat scroll 7px 0px transparent !important;
            border-bottom: none;
        }
        .b_box.checktable_supplier span.custom-checkbox span.checkbox-off
        {
            background: url( "../../images/check-box-img.jpg" ) no-repeat scroll 7px -22px transparent !important;
            border-bottom: none;
        }
        .b_box.checktable_supplier span.custom-checkbox_checked span.checkbox-on
        {
            background: url( "../../images/check-box-img.jpg" ) no-repeat scroll 7px -22px transparent !important;
            border-bottom: none;
        }
        .b_box.checktable_supplier span.custom-checkbox_checked span.checkbox-off
        {
            background: url( "../../images/check-box-img.jpg" ) no-repeat scroll 7px 0px transparent !important;
            border-bottom: none;
        }
        .b_box.checktable_supplier span.custom-checkbox, .b_box.checktable_supplier span.custom-checkbox_checked
        {
            float: left;
            padding-bottom: 5px;
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
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
                        <td runat="server" id="trFromDate" class="formtd_r">
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
                        <td runat="server" id="trToDate" class="formtd">
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
                                        <asp:DropDownList ID="ddlApprovers" runat="server" onchange="pageLoad(this,value);">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
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
        <div id="dvPendingOrders" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">Pending Order List</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;">
                <div style="text-align: right; margin-bottom: 10px;">
                    Total Records:
                    <asp:Label runat="server" ID="lblRecordCounter" Text="0"></asp:Label>
                </div>
                <div>
                    <asp:GridView ID="gvPendingOrders" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvPendingOrders_RowDataBound"
                        OnRowCommand="gvPendingOrders_RowCommand">
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
                                    
                                        <%--<asp:LinkButton runat="server" ID="lnkbtnOrderNumber" Text='<%# Eval("OrderNumber")%>'
                                            CommandName="OrderDetail" CommandArgument='<%# Eval("OrderID")%>'></asp:LinkButton>--%>
                                        <asp:Label runat="server" ID="lblOrderNumber" Text='<%# Eval("OrderNumber")%>' CssClass="first"></asp:Label>
                                        <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnOrderDate" runat="server" CommandArgument="OrderDate" CommandName="Sort"><span>Date</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%# Convert.ToDateTime(Eval("OrderDate")).ToString("MM/dd/yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnUserName" runat="server" CommandArgument="UserName" CommandName="Sort"><span>User Name</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblUserName" Text='<%# Eval("UserName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnBaseStation" runat="server" CommandArgument="BaseStation"
                                        CommandName="Sort"><span>Station</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBaseStation" Text='<%# Convert.ToString(Eval("BaseStation")).Substring(0,3) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnWorkgroup" runat="server" CommandArgument="Workgroup" CommandName="Sort"><span>WorkGroup</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblWorkgroup" Text='<%# Eval("Workgroup") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnApproverLevel" runat="server" CommandArgument="ApproverLevel"
                                        CommandName="Sort"><span>ApproverLevel</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblApproverLevel" Text='<%# Eval("ApproverLevel") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="13%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                     <asp:LinkButton ID="lnkbtnApproverLevel" runat="server" CommandArgument="ApproverLevel"
                                        CommandName="Sort"><span>Approvers</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Repeater ID="rpApprovers" runat="server">
                                        <ItemTemplate>
                                            <table class="orderreturn_box" cellpadding="0" cellspacing="0" width="100%">
                                                <tr class="ord_content">
                                                    <td class="b_box" style="width:60%;border-left:1px;" >
                                                        <asp:Label runat="server" ID="lblOrderDate" Text='<%# Eval("ApproverName")%>'></asp:Label>
                                                    </td>
                                                    <td class="g_box" style="width:40%">
                                                        <asp:Label runat="server" ID="Label1" Text='<%# Eval("Status")%>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="32%" />
                            </asp:TemplateField>
                            
                        </Columns>
                        <EmptyDataTemplate>
                             <table class="orderreturn_box" cellpadding="0" cellspacing="0" width="100%">
                                <tr class="ord_header">
                                    <th>
                                        <span>Order Number</span>
                                    </th>
                                    <th>
                                        <span>Date</span>
                                   </th>
                                    <th>
                                        <span>User Name</span>
                                   </th>
                                    <th>
                                        <span>Station</span>
                                   </th>
                                    <th>
                                        <span>WorkGroup</span>
                                   </th>
                                    <th>
                                        <span>Approver Level</span>
                                   </th>
                                    <th>
                                        <span>Approvers</span>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="7" style="color:Red;text-align:center;">
                                        No Records Found
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
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
        <div style="float: right; position: relative; top: -30px; right: 10px;">
            <asp:Label runat="server" ID="lblDisplayText"></asp:Label>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
