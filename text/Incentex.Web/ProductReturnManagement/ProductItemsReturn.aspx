<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ProductItemsReturn.aspx.cs" Inherits="ProductReturnManagement_ProductItemsReturn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        a.gredient_btnMainPage
        {
            background: url("../images/button.png") no-repeat scroll 0 0 transparent;
            color: #FFFFFF;
            cursor: pointer;
            display: block;
            float: left;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 14px;
            font-weight: normal;
            height: 40px;
            line-height: 40px;
            margin: 0 32px 18px 0;
            padding: 0 0 8px 19px;
            text-decoration: none;
            width: 286px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="sc1" runat="server">
    </at:ToolkitScriptManager>

    <script type="text/javascript">

        // check if value is numeric in textbox
        function CheckNum(id) {
            var txt = document.getElementById(id);
            if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.value = '';
                txt.focus();
                return
            }

            var OrderQtyField = id.toString().substring(0, id.toString().indexOf("txtReturnQty", 0) - 1);
            OrderQtyField = OrderQtyField + '_lblQtyShipped';

            if (parseInt(txt.value) > document.getElementById(OrderQtyField).innerHTML) {
                alert("You can not enter return qty more than actual qty " + document.getElementById(OrderQtyField).innerHTML + ".");
                txt.value = "";
                txt.focus();
                return;
            }
            ValidateQty(id, 'txtReturnQty');
        }

        function ValidateItemGrid(id) {
            var count = 2;
            for (var i = 0; i < $('#ctl00_ContentPlaceHolder1_gvOrderDetail tr').length - 1; i++) {

                var Field = "ctl00_ContentPlaceHolder1_gvOrderDetail_ctl";

                var ReturnQtyField;
                if (count.toString().length == 1)
                    ReturnQtyField = Field + '0' + count + '_txtReturnQty';
                else
                    ReturnQtyField = Field + count + '_txtReturnQty';

                var ReasonField;
                if (count.toString().length == 1)
                    ReasonField = Field + '0' + count + '_ddlReasonCode';
                else
                    ReasonField = Field + count + '_ddlReasonCode';

                if (document.getElementById(ReasonField).value == 0 && document.getElementById(ReturnQtyField).value != "") {
                    document.getElementById(ReasonField).focus();
                    alert("Please Select the Reason code for the item.");
                    return false;
                }
                if (document.getElementById(ReasonField).value > 0 && document.getElementById(ReturnQtyField).value == "") {
                    document.getElementById(ReturnQtyField).focus();
                    alert("Please enter Return Qty for the item.");
                    return false;
                }
                count = count + 1;
            }
            return true;
        }


        function IsNumeric(sText) {
            var ValidChars = "0123456789.";
            //var ValidChars = "0123456789";
            var IsNumber = true;
            var Char;


            for (i = 0; i < sText.length && IsNumber == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    IsNumber = false;
                }
            }
            return IsNumber;

        }

        function ValidatedReturnQty(id) {

            var txt = document.getElementById(id);
            if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.value = '';
                txt.focus();
                return
            }
            var totalReturnQtyLeft = document.getElementById("<%= hdnMinimumRtnQty.ClientID %>").value;
            var txtReturnQtyNo = document.getElementById("<%= txtReturnQtyNo.ClientID %>").value;
            if (parseInt(txtReturnQtyNo) > totalReturnQtyLeft) {
                alert("Please enter value less then :" + totalReturnQtyLeft);
                txt.value = '';
                txt.focus();
            }
        }

        function ValidateQty(id, type) {
            var OrderQtyField;
            if (type == 'txtReturnQty')
                OrderQtyField = id.toString().substring(0, id.toString().indexOf("txtReturnQty", 0) - 1);
            else
                OrderQtyField = id.toString().substring(0, id.toString().indexOf("lnkbtnadd", 0) - 1);

            var TotalItemQtyField = OrderQtyField + '_lblQtyShipped';

            var ItemNameNewField = OrderQtyField + '_hdnitemNo';

            var TotalItemQty = document.getElementById(TotalItemQtyField).innerHTML;
            var ItemName = document.getElementById(ItemNameNewField).value;
            var TotalReturnQty = 0;
            var count = 2;
            for (var i = 0; i < $('#ctl00_ContentPlaceHolder1_gvOrderDetail tr').length - 1; i++) {

                var Field;

                if (type == 'txtReturnQty')
                    Field = id.toString().substring(0, id.toString().indexOf("txtReturnQty", 0) - 3);
                else
                    Field = id.toString().substring(0, id.toString().indexOf("lnkbtnadd", 0) - 3);

                var ReturnQtyField;
                if (count.toString().length == 1)
                    ReturnQtyField = Field + '0' + count + '_txtReturnQty';
                else
                    ReturnQtyField = Field + count + '_txtReturnQty';

                var ItemNameField;
                if (count.toString().length == 1)
                    ItemNameField = Field + '0' + count + '_hdnitemNo';
                else
                    ItemNameField = Field + count + '_hdnitemNo';




                if (ItemName.toString() == document.getElementById(ItemNameField).value.toString()) {
                    if (document.getElementById(ReturnQtyField).value != "")
                        TotalReturnQty = parseInt(TotalReturnQty) + parseInt(document.getElementById(ReturnQtyField).value);
                }
                count = count + 1;
            }
            if (type == 'txtReturnQty') {
                if (parseInt(TotalReturnQty) > TotalItemQty) {
                    alert("You can not enter return qty more than actual qty " + TotalItemQty + ".");
                    document.getElementById(id).value = "";
                    document.getElementById(id).focus();
                }
            }
            else {
                if (parseInt(TotalReturnQty) >= TotalItemQty) {
                    alert("You can not enter return qty more than actual qty " + TotalItemQty + ".");
                    return false;
                }

            }
            return true;
        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleContainerContent").hide();

        });   
    </script>

    <script type="text/javascript">
        function print() {
            var oid = document.getElementById('ctl00_ContentPlaceHolder1_hdORderID').value;
            window.open('ProductItemsReturnPrint.aspx?Id=' + oid, 'printwin', 'width=420,height=500 ,scrollbars=yes');
        }
        function ItemAddConfirmation() {
            if (confirm("Are all items being returned for the same reason?") == true)
                return true;
            else
                return false;
        }

        function ShowReturnQuantityQues() {
            var styleString = 'display: block; visibility: visible';
            var id = document.getElementById('dvQuesRqty');
            id.setAttribute('style', styleString);
        }
        function HideReturnQuantityQues() {
            var styleString = 'display: none; visibility: hidden';
            var id = document.getElementById('dvQuesRqty');
            id.setAttribute('style', styleString);
        }
    </script>

    <div class="spacer10">
    </div>
    <table class="order_detail">
        <tr>
            <td style="width: 60%;">
            </td>
            <td style="font-size: small; float: right">
                <asp:Label runat="server" ID="lblProcessedOn" Text="Processed On :"></asp:Label>
            </td>
            <td style="font-size: small; padding-left: 10px">
                <asp:Label ID="lblProcessDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 60%;">
            </td>
            <td style="font-size: small; float: right">
                <asp:Label runat="server" ID="lblStatuslabel" Text="Status :"></asp:Label>
            </td>
            <td style="font-size: small; padding-left: 10px">
                <asp:Label runat="server" ID="lblStatus"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="spacer10">
    </div>
    <div style="text-align: center">
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
    </div>
    <table width="100%">
        <tr>
            <td>
                <div class="botbtn rightalign">
                    <asp:LinkButton ID="lnkPrint" class="grey2_btn" runat="server" ToolTip="Print Previous Request"
                        OnClick="LinPrint_Click"><span>Print Previous Request</span></asp:LinkButton>
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 80%; vertical-align: middle;">
                <div>
                    <div class="alignleft" style="width: 100%;">
                        <span>Order Summary:</span></div>
                </div>
                <div class="spacer20">
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvOrderSummary" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvOrderSummary_RowDataBound"
                    OnRowCommand="gvOrderSummary_RowCommand">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Item Number</span>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblItemNumber" runat="server" Text='<%#Eval("ItemNumber")%>' ToolTip='<%#Eval("ItemNumber")%>'></asp:Label>
                                <div class="corner">
                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="b_box centeralign" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Product Description</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProductionDescription" runat="server"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="g_box" Width="50%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Ordered</span>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnReturnStatusId" runat="server" Value='<%# Eval("ReturnStatusId") %>' />
                                <asp:HiddenField ID="hdnReturnQtyValue" runat="server" Value='<%# Eval("ReturnQty") %>' />
                                <asp:HiddenField ID="hdnProductReturnId" runat="server" Value='<%# Eval("ProductReturnId") %>' />
                                <asp:HiddenField ID="hdnShippid" runat="server" Value='<%# Eval("ShippID") %>' />
                                <asp:HiddenField ID="hdnitemNo" runat="server" Value='<%# Eval("ItemNumber") %>' />
                                <asp:HiddenField ID="hdnMyShoppingCartiD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"MyShoppingCartiD") %>' />
                                <asp:Label ID="lblQtyOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box centeralign" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Shipped</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblQtyShipped" runat="server" Text='<%# Eval("ShippedQty") %>' ToolTip='<%# Eval("ShippedQty") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="g_box centeralign" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>**</span>
                                <div class="corner">
                                    <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                </div>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <span class="btn_space">
                                    <asp:LinkButton ID="lnkbtnadd" runat="server" CommandArgument='<%# Eval("ItemNumber")+";"+ Eval("ShippID")%>'
                                        CommandName="ItemAdd">
                                        <asp:Image ID="imgadd" ImageUrl="~/Images/TreeLineImages/plus.gif" runat="server" />
                                    </asp:LinkButton>
                                </span>
                                <div class="corner">
                                    <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box centeralign" Width="15%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <table width="100%" id="tblReturnRequest" runat="server">
        <tr>
            <td style="width: 80%; vertical-align: middle;">
                <div>
                    <div class="alignleft" style="width: 100%;">
                        <span>Please enter the return quanty for below items and select reason code</span></div>
                </div>
                <div class="spacer20">
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvOrderDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvOrderDetail_RowDataBound"
                    OnRowCommand="gvOrderDetail_RowCommand">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Item Number</span>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblItemNumber" runat="server" Text='<%# Eval("ItemNumber") %>' ToolTip='<%#Eval("ItemNumber") %>'></asp:Label>
                                <div class="corner">
                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="b_box centeralign" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Product Description</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProductionDescription" runat="server"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="g_box" Width="32%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Ordered</span>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnReturnQtyValue" runat="server" Value='<%# Eval("ReturnQty") %>' />
                                <asp:HiddenField ID="hdnQuantity" runat="server" Value='<%#Eval("Quantity") %>' />
                                <asp:HiddenField ID="hdnShippid" runat="server" Value='<%# Eval("ShippID") %>' />
                                <asp:HiddenField ID="hdnitemNo" runat="server" Value='<%# Eval("ItemNumber") %>' />
                                <asp:HiddenField ID="hdnReasonCode" runat="server" Value='<%# Eval("ReasonCode") %>' />
                                <asp:HiddenField ID="hdnMyShoppingCartiD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"MyShoppingCartiD") %>' />
                                <asp:Label ID="lblQtyOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box centeralign" Width="7" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Shipped</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblQtyShipped" runat="server" Text='<%# Eval("ShippedQty") %>' ToolTip='<%# Eval("ShippedQty") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="g_box centeralign" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="ReturnQty">
                            <HeaderTemplate>
                                <span>Return Qty</span>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <span class="btn_space">
                                    <asp:TextBox ID="txtReturnQty" runat="server" onchange="CheckNum(this.id)" Style="background-color: #303030;
                                        border: medium none; color: #FFFFFF; padding: 2px; width: 100%; text-align: center;"
                                        Text='<%#Eval("ReturnQty") %>'></asp:TextBox>
                                </span>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Reason Code</span>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <span style="height: 26px;">
                                    <asp:DropDownList ID="ddlReasonCode" Style="margin-top: 3px; background-color: #303030;
                                        border: #303030; width: 100%; color: #72757C;" runat="server">
                                    </asp:DropDownList>
                                </span>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>**</span>
                                <div class="corner">
                                    <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                </div>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <span class="btn_space">
                                    <asp:LinkButton ID="lnkbtndelete" runat="server" CommandArgument='<%# Eval("OrderStatus") %>'
                                        CommandName="ItemDelete" ToolTip='<%# Eval("OrderStatus") %>'>
                                        <asp:Image ID="img" ImageUrl="~/Images/close.png" runat="server" />
                                    </asp:LinkButton></span>
                                <div class="corner">
                                    <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box centeralign" Width="15%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="botbtn rightalign">
                    <asp:LinkButton ID="lnkBtnSubmitReturn" class="grey2_btn" runat="server" ToolTip="Submit Return Information"
                        OnClientClick="return ValidateItemGrid(this.id);" OnClick="lnkBtnSubmitReturn_Click"><span>Submit Return</span></asp:LinkButton>
                </div>
            </td>
        </tr>
    </table>
    <div class="spacer20">
    </div>
    <div class="spacer20">
    </div>
    <div id="divShowOrderDtls" runat="server">
        <div>
            <div class="alignleft" style="width: 100%;">
                <span>Return Request Summary:</span></div>
        </div>
        <div class="spacer20">
        </div>
        <div class="black_top_co">
            <span>&nbsp;</span></div>
        <div class="black_middle order_detail_pad">
            <asp:Repeater ID="dtlReturnRequest" runat="server" OnItemDataBound="dtlReturnRequest_ItemDataBound">
                <ItemTemplate>
                    <div class="collapsibleContainer" title='<%# Eval("ItemNumber")%>' total='<%# Eval("ProductDescription") %>'>
                        <asp:HiddenField ID="hdnIamExpanded" runat="server" Value="false" />
                        <asp:HiddenField ID="hdnOrderNumber" runat="server" Value='<%# Eval("ItemNumber")%>' />
                        <asp:HiddenField ID="hdnSubmiDate" runat="server" Value='<%# Eval("ProductDescription")%>' />
                        <asp:GridView ID="grdViewReturnItems" runat="server" AutoGenerateColumns="false"
                            HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                            RowStyle-CssClass="ord_content" OnRowDataBound="grdViewReturnItems_RowDataBound">
                            <Columns>
                                <asp:TemplateField SortExpression="FirstName">
                                    <HeaderTemplate>
                                        <span>Return Qty</span>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblReturnQty" Text='<%# Eval("ReturnQty") %> ' />
                                        <div class="corner">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="true">
                                    <HeaderTemplate>
                                        <span>Item Number</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemNo" runat="server" Text='<%# Eval("ItemNumber") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="15%" />
                                    <ItemStyle CssClass="b_box" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="centeralign" />
                                    <HeaderTemplate>
                                        <span>Product Description</span>
                                    </HeaderTemplate>
                                    <HeaderStyle />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" Text='<%# Convert.ToString(Eval("ProductDescription")).Length > 40 ? Convert.ToString(Eval("ProductDescription")).Substring(0, 40).Trim() + "..." : (Convert.ToString(Eval("ProductDescription")) + "&nbsp;") %>'
                                            ToolTip='<% #Eval("ProductDescription")  %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="centeralign" />
                                    <HeaderTemplate>
                                        <span>Received Qty</span>
                                    </HeaderTemplate>
                                    <HeaderStyle />
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnMyShoppingCartIdView" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"MyShoppingCartiD") %>' />
                                        <asp:Label runat="server" ID="lblReceivedQty" Text='<%# Eval("Quantity") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="centeralign" />
                                    <HeaderTemplate>
                                        <span>Reason</span>
                                    </HeaderTemplate>
                                    <HeaderStyle />
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnReasonCode" runat="server" Value='<%# Eval("ReasonCode")%>' />
                                        <asp:Label runat="server" ID="lblReason" Text='<%# Eval("ReasonCode") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box centeralign" Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="centeralign" />
                                    <HeaderTemplate>
                                        <span>Status</span>
                                        <div class="corner">
                                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <HeaderStyle />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("OrderStatus") %>' />
                                        <div class="corner">
                                            <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="black_bot_co">
            <span>&nbsp;</span></div>
    </div>
    <asp:LinkButton ID="lnkDummyPro" class="grey2_btn" runat="server" Style="display: none"></asp:LinkButton>
    <at:ModalPopupExtender ID="modalPrint" TargetControlID="lnkDummyPro" BackgroundCssClass="modalBackground"
        DropShadow="true" runat="server" PopupControlID="pnlPrintOrderReturn" CancelControlID="closepro">
    </at:ModalPopupExtender>
    <asp:Panel ID="pnlPrintOrderReturn" runat="server" Style="display: none;">
        <div class="pp_pic_holder facebook" style="display: block; width: 490px; position: fixed;
            left: 30%; top: 30%;">
            <div class="pp_top" style="">
                <div class="pp_left">
                </div>
                <div class="pp_middle">
                </div>
                <div class="pp_right">
                </div>
            </div>
            <div class="pp_content_container" style="">
                <div class="pp_left" style="">
                    <div class="pp_right" style="">
                        <div class="pp_content" style="height: 228px; display: block;">
                            <div class="pp_loaderIcon" style="display: none;">
                            </div>
                            <div class="pp_fade" style="display: block;">
                                <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                    <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                        style="visibility: visible;">previous</a>
                                </div>
                                <div id="pp_full_res">
                                    <div class="pp_inline clearfix" style="padding-left: 75px; padding-top: 30px">
                                        <div class="form_popup_box">
                                            <a id="lnkPrintRetunAuthorization" runat="server" title="Print Retun Authorization"
                                                class="gredient_btnMainPage" onclick="javascript:print();"><span>Print Retun Authorization
                                                </span></a>
                                            <asp:LinkButton ID="lnkPrintPrepaidLabel" runat="server" title="Print Pre-paid Return Shipping"
                                                class="gredient_btnMainPage" OnClick="lnkPrintPrepaidLabel_Click">
                                                <span>Process Return</span></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <div class="pp_details clearfix" style="width: 371px;">
                                    <a href="#" id="closepro" runat="server" class="pp_close">Close</a>
                                    <p class="pp_description" style="display: none;">
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="pp_bottom" style="">
                <div class="pp_left" style="">
                </div>
                <div class="pp_middle" style="">
                </div>
                <div class="pp_right" style="">
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lnkDummyConfirm" class="grey2_btn" runat="server" Style="display: none"></asp:LinkButton>
    <at:ModalPopupExtender ID="modalPopupConfirm" TargetControlID="lnkDummyConfirm" BackgroundCssClass="modalBackground"
        DropShadow="true" runat="server" PopupControlID="pnlConfirmation" CancelControlID="ClsConfirm">
    </at:ModalPopupExtender>
    <asp:Panel ID="pnlConfirmation" runat="server" Style="display: none;">
        <div class="pp_pic_holder facebook" style="display: block; width: 490px; position: fixed;
            left: 30%; top: 30%;">
            <div class="pp_top" style="">
                <div class="pp_left">
                </div>
                <div class="pp_middle">
                </div>
                <div class="pp_right">
                </div>
            </div>
            <div class="pp_content_container" style="">
                <div class="pp_left" style="">
                    <div class="pp_right" style="">
                        <div class="pp_content" style="height: 248px; display: block;">
                            <div class="pp_loaderIcon" style="display: none;">
                            </div>
                            <div class="pp_fade" style="display: block;">
                                <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                    <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                        style="visibility: visible;">previous</a>
                                </div>
                                <div id="DivPop">
                                    <div class="pp_inline clearfix" style="padding-left: 75px; padding-top: 30px">
                                        <div class="form_popup_box">
                                            <div class="tbl_row">
                                                <strong>Are all items being returned for the same reason? </strong>
                                            </div>
                                            <div class="cl spacer">
                                            </div>
                                            <div class="tbl_row">
                                                <div class="alignleft" style="padding-left: 20px;">
                                                    <span>
                                                        <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="btnYes_Click" />
                                                    </span><span>
                                                        <input id="btnNo" type="button" value="No" onclick="return ShowReturnQuantityQues();" /></span></div>
                                            </div>
                                            <div class="cl spacer">
                                            </div>
                                            <div id="dvQuesRqty" style="display: none; visibility: hidden;">
                                                <div class="tbl_row">
                                                    <strong>How many of this items are you returning?</strong>
                                                </div>
                                                <div class="tbl_row">
                                                    <asp:TextBox ID="txtReturnQtyNo" runat="server" onchange="ValidatedReturnQty(this.id)"></asp:TextBox>
                                                </div>
                                                <div class="cl spacer">
                                                </div>
                                                <div class="tbl_row">
                                                    <div class="alignleft" style="padding-left: 20px;">
                                                        <span>
                                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                                        </span><span>
                                                            <input id="btnCancel" type="button" value="Cancel" onclick="return HideReturnQuantityQues();" />
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="pp_details clearfix" style="width: 371px;">
                                    <a href="#" id="ClsConfirm" runat="server" class="pp_close">Close</a>
                                    <p class="pp_description" style="display: none;">
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="pp_bottom" style="">
                <div class="pp_left" style="">
                </div>
                <div class="pp_middle" style="">
                </div>
                <div class="pp_right" style="">
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hdnMinimumRtnQty" runat="server" Value="0" />
    <asp:HiddenField ID="hdORderID" runat="server" />
</asp:Content>
