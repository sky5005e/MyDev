<%@ Page Title="Sync Information" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="SyncInformation.aspx.cs" Inherits="admin_Supplier_SyncInformation" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });

        $(function() {
            $("#<%=lnkSyncOrderData.ClientID %>").click(function() {
                if (!CompareDates($("#ctl00_ContentPlaceHolder1_txtFromDate").val(), $("#ctl00_ContentPlaceHolder1_txtToDate").val(), "/")) {
                    ShowMessage('1');
                    return false;
                }
                else {
                    $('#dvLoader').show();
                    return true;
                }
            });
            
            $("#<%=lnkSyncInventoryData.ClientID %>").click(function() {               
                $('#dvLoader').show();
            });

            $("#ctl00_ContentPlaceHolder1_txtFromDate").change(function() {
                if (!CompareDates($("#ctl00_ContentPlaceHolder1_txtFromDate").val(), $("#ctl00_ContentPlaceHolder1_txtToDate").val(), "/")) {
                    ShowMessage('1');
                }
                else {
                    ShowMessage('0');
                }
            });

            $("#ctl00_ContentPlaceHolder1_txtToDate").change(function() {
                if (!CompareDates($("#ctl00_ContentPlaceHolder1_txtFromDate").val(), $("#ctl00_ContentPlaceHolder1_txtToDate").val(), "/")) {
                    ShowMessage('1');
                }
                else {
                    ShowMessage('0');
                }
            });
            
            $(window).scroll(function () {
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
        });

        function ShowMessage(show) {
            if (show == '1') {
                document.getElementById("spanFromDate").style.display = '';
                document.getElementById("spanToDate").style.display = '';
            }
            else {
                document.getElementById("spanFromDate").style.display = 'none';
                document.getElementById("spanToDate").style.display = 'none';
            }
        }
        
        function CompareDates(fromDate, toDate, separator) {
            var fromDateArr = Array();
            var toDateArr = Array();

            fromDateArr = fromDate.split(separator);
            toDateArr = toDate.split(separator);

            var fromMt = fromDateArr[0];
            var fromDt = fromDateArr[1];
            var fromYr = fromDateArr[2];

            var toMt = toDateArr[0];
            var toDt = toDateArr[1];
            var toYr = toDateArr[2];

            if (fromYr > toYr) {
                return 0;
            }
            else if (fromYr == toYr && fromMt > toMt) {
                return 0;
            }
            else if (fromYr == toYr && fromMt == toMt && fromDt > toDt) {
                return 0;
            }
            else
                return 1;
        }
    </script>

    <style type="text/css">
        .form_box span.error
        {
            margin-left: 0%;
        }
        .order_detail td
        {
            font-size: 15px;
            color: #72757c;
            line-height: 20px;
            padding-bottom: 0px !important;
            text-align: left !important;
        }
        .order_detail label
        {
            color: #B0B0B0;
            padding-right: 6px;
            padding-left: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <table class="form_table select_box_pad">
            <tr>
                <td>
                    <div class="calender_l">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label">From Date</span>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="datepicker1 w_label"></asp:TextBox>
                            <span class="error" id="spanFromDate" style="display: none;">From date should be less
                                than to date.</span>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="calender_l">
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label">To Date</span>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker1 w_label"></asp:TextBox>
                            <span class="error" id="spanToDate" style="display: none;">To date should be greater
                                than from date.</span>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:LinkButton ID="lnkSyncOrderData" runat="server" class="gredient_btn1" ToolTip="Sync Order Data"
                            OnClick="lnkSyncOrderData_Click">
                    <span>
                        <img src="../Incentex_Used_Icons/syncorderdat.png" alt="SOD" />Sync Order Data</span></asp:LinkButton>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="divider">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:LinkButton ID="lnkSyncInventoryData" runat="server" class="gredient_btn1" ToolTip="Sync Inventory Data"
                            OnClick="lnkSyncInventoryData_Click">
                                                        <span><img src="../Incentex_Used_Icons/syncinvetorydata.png" alt="SID" />Sync Inventory Data</span></asp:LinkButton>
                    </div>
                </td>
            </tr>
        </table>
        <div runat="server" id="dvDetail" class="form_pad" style="width: 900px;" visible="false">
            <div style="text-align: right; color: #5C5B60; font-size: larger; font-weight: bold;
                padding-left: 15px; float: left;">
                <asp:Label runat="server" ID="lblUpdateTitle"></asp:Label>
            </div>
            <div style="text-align: right; color: #5C5B60; font-size: larger; font-weight: bold;
                padding-left: 15px; float: right">
                Total Records :
                <asp:Label ID="lblRecords" runat="server"></asp:Label>
            </div>
            <div class="spacer10" style="clear: both;">
            </div>
            <asp:GridView ID="gvUpdatedItems" runat="server" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblHeaderProdDesc" runat="server">Product Description</asp:Label>
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblProdDesc" runat="server" Text='<%# Eval("ProdDesc") %>' CssClass="first"></asp:Label>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="88%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblHeaderUpdatedQty" runat="server" CssClass="centeralign">Updated Qty.</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblUpdatedQty" Text='<%# Eval("UpdatedQty") %>' style="text-align:right;" />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="12%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Repeater ID="repUpdatedOrders" runat="server" OnItemDataBound="repUpdatedOrders_ItemDataBound"
                OnItemCommand="repUpdatedOrders_ItemCommand">
                <HeaderTemplate>
                    <div>
                        <table class="orderreturn_box true records" border="0" cellspacing="0" style="border-collapse: collapse;">
                            <tr class="ord_header">
                                <th class="centeralign" style="width: 15%;">
                                    <span>Order #</span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </th>
                                <th style="width: 40%;">
                                    <span>Company</span>
                                </th>
                                <th style="width: 45%;">
                                    <span>Contact</span>
                                </th>
                            </tr>
                        </table>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HiddenField ID="hdnRepOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                    <div>
                        <table class="orderreturn_box true records" border="0" cellspacing="0" style="border-collapse: collapse;">
                            <tr class="ord_content">
                                <td class="g_box" style="width: 15%;">
                                    <span class="first centeralign">
                                        <asp:LinkButton ID="lblOrderNo" runat="server" Text='<%# Eval("OrderNumber") %>'
                                            CommandArgument='<%# Eval("OrderID") %>' CommandName="OrderDetail"></asp:LinkButton></span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </td>
                                <td class="b_box" style="width: 40%;">
                                    <asp:Label ID="lblCompany" runat="server" Text='<%# Eval("Company") %>'></asp:Label>
                                </td>
                                <td class="g_box" style="width: 45%;">
                                    <asp:Label ID="lblContact" runat="server" Text='<%# Eval("FirstName") + " " + Eval("LastName") %>'></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="spacer10">
                    </div>
                    <div>
                        <asp:GridView ID="gvUpdatedOrderItems" runat="server" HeaderStyle-CssClass="ord_header"
                            CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblHeaderProdDesc" runat="server">Product Description</asp:Label>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                                        <asp:Label ID="lblProdDesc" runat="server" Text='<%# Eval("ProdDesc") %>' CssClass="first"></asp:Label>
                                        <div class="corner">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="52%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblHeaderOrderedQty" runat="server" CssClass="centeralign">Ordered</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblOrderedQty" Text='<%# Eval("OrderQty") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box centeralign" Width="12%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblHeaderAlreadyShippedQty" runat="server" CssClass="centeralign">Already Shipped</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblAlreadyShippedQty" Text='<%# Eval("AlreadyShippedQty") %>'
                                            CssClass="centeralign" />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="12%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblHeaderShippedQty" runat="server" ToolTip="Shipped in this sync"
                                            CssClass="centeralign">Shipped</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblShippedQty" Text='<%# Eval("ShippedQty") %>' ToolTip="Shipped in this sync" />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box centeralign" Width="12%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblHeaderRemainingQty" runat="server" CssClass="centeralign">Remaining</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblRemQty" Text='<%# Convert.ToInt64(Eval("OrderQty")) - Convert.ToInt64(!String.IsNullOrEmpty(Convert.ToString(Eval("AlreadyShippedQty"))) ? Eval("AlreadyShippedQty") : "0") - Convert.ToInt64(Eval("ShippedQty")) %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box centeralign" Width="12%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="divider">
                    </div>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
