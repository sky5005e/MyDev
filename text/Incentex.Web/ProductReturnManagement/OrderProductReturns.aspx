<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="OrderProductReturns.aspx.cs" Inherits="ProductReturnManagement_OrderProductReturns"
    Title="Order Product Return Details" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <style type="text/css">
        .textarea_box textarea
        {
            height: 96px;
        }
        .textarea_box
        {
            height: 96px;
        }
        .textarea_box .scrollbar
        {
            height: 96px;
        }
        .fontsizesmall
        {
            font-size: small;
        }
        .noteIncentex
        {
            background-color: #E1E0E0;
            color: black;
            font-family: "Trebuchet MS" ,tahoma,arial,verdana;
            font-size: 0.8em;
            padding-bottom: 5px;
            padding-top: 7px;
        }
        .rightalign
        {
            font-size: small;
            color: #72757C;
            text-align: right;
        }
        .width300
        {
            font-size: small;
            width: 300px !important;
        }
    </style>

    <script type="text/javascript">

        // check if value is numeric in textbox
        function CheckNum(id) {
            var txt = document.getElementById(id);
            if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.value = '';
                txt.focus();
            }

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

        //End
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <div class="black_round_box">
        <div class="black2_round_top">
            <span></span>
        </div>
        <div class="black2_round_middle">
            <div>
                <div style="width: 940px;">
                    <div>
                        <div>
                            <span>&nbsp;</span></div>
                        <asp:Panel runat="server" ID="pnlcolsedOrder">
                            <div style="font-size: medium; color: Gray; font-family: Trebuchet MS,tahoma,arial,verdana;
                                font-weight: bold;">
                                <asp:Label ID="litMessage" runat="server" Text="Please note that any product can be returned or exchanged within a 30 day period from the ship date. Once that 30 day period has past the products on that order may NOT be returned or exchanged.
                            Please select the order number in which you would like to return or exchange a product from the drop-down menu below. Please make sure when you fill out the return request form that you enter in the “Reason for Return” details. So, for example, if you are exchanging an item make sure you type in the item you wish to it exchange for."></asp:Label>
                            </div>
                            <div class="spacer15">
                            </div>
                            <div>
                                <table class="dropdown_pad form_table" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:UpdatePanel ID="upnlOrderClosed" runat="server">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="ddlOrderClosed" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="custom-sel">
                                                                <asp:DropDownList ID="ddlOrderClosed" AutoPostBack="True" runat="server" onchange="pageLoad(this,value);"
                                                                    OnSelectedIndexChanged="ddlOrderClosed_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </span>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <div class="spacer15">
                        </div>
                        <asp:Panel ID="pnlMain" runat="server">
                            <div class="black_top_co">
                                <span>&nbsp;</span></div>
                            <div class="black_middle order_detail_pad">
                                <table class="order_detail" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 50%">
                                            <table>
                                                <tr runat="server" id="trOrder">
                                                    <td style="font-size: small;">
                                                        <label>
                                                            Order Number :
                                                        </label>
                                                        <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="trReturnOrder">
                                                    <td style="font-size: small;">
                                                        <label>
                                                            Return Order # :
                                                        </label>
                                                        <asp:Label ID="lblReturnOrderNumber" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: small;">
                                                        <label>
                                                            Reference Name :
                                                        </label>
                                                        <asp:Label ID="lblOrderBy" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50%">
                                            <table>
                                                <tr>
                                                    <td style="font-size: small;">
                                                        <label style="padding-left: 28px!important;">
                                                            Ordered Date :
                                                        </label>
                                                        <asp:Label ID="lblOrderedDate" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: small;">
                                                        <label style="padding-left: 28px!important;">
                                                            Order Status :
                                                        </label>
                                                        <asp:Label runat="server" ID="lblOrderStatus"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: small;">
                                                        <label style="padding-left: 28px!important;">
                                                            Payment Method :
                                                        </label>
                                                        <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: small;">
                                                        <label style="padding-left: 28px!important;">
                                                            Type of Return :
                                                        </label>
                                                        <asp:DropDownList runat="server" ID="ddlRequesting" AutoPostBack="false" onchange="pageLoad(this,value);"
                                                            Style="background-color: #303030; border: medium none; color: #ffffff; width: 150px;
                                                            padding: 2px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="black_bot_co">
                                <span>&nbsp;</span></div>
                            <div class="spacer15">
                            </div>
                            <div class="black_top_co">
                                <span>&nbsp;</span></div>
                            <div class="black_middle order_detail_pad">
                                <div class="clearfix billing_head">
                                    <div class="alignleft">
                                        <span>Bill To: </span>
                                    </div>
                                    <div class="alignright">
                                        <span style="padding-left: 29px!important;">Ship To: </span>
                                    </div>
                                </div>
                                <div>
                                    <div class="alignleft" style="width: 49%;">
                                        <div class="tab_content_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="tab_content">
                                            <table class="order_detail" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <label style="font-size: small; width: 300px !important;">
                                                            <asp:Label ID="lblBName" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="font-size: small; width: 300px !important;">
                                                            <asp:Label ID="lblBCompany" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="font-size: small; width: 300px !important;">
                                                            <asp:Label ID="lblBAddress" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="font-size: small; width: 300px !important;">
                                                            <asp:Label ID="lblBCity" runat="server" />
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="font-size: small; width: 300px !important;">
                                                            <asp:Label ID="lblBCountry" runat="server" /></label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="tab_content_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                    <div class="alignright" style="width: 49%;">
                                        <div class="tab_content_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="tab_content">
                                            <table class="order_detail" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <label style="font-size: small; width: 300px !important;">
                                                            <asp:Label ID="lblSName" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="font-size: small; width: 300px !important;">
                                                            <asp:Label ID="lblSCompany" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="font-size: small; width: 300px !important;">
                                                            <asp:Label ID="lblSAddress" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="font-size: small; width: 300px !important;">
                                                            <asp:Label ID="lblSCity" runat="server" />
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="font-size: small; width: 300px !important;">
                                                            <asp:Label ID="lblSCountry" runat="server" />
                                                        </label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="tab_content_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                    <div class="alignnone">
                                    </div>
                                </div>
                            </div>
                            <div class="black_bot_co">
                                <span>&nbsp;</span></div>
                            <div class="spacer15">
                            </div>
                            <table width="100%">
                                <asp:Panel runat="server" ID="pnlOrderSummary">
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
                                            <div style="text-align: center; color: Red; font-size: larger;">
                                                <asp:Label ID="lblmsg" runat="server">
                                                </asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td>
                                        <asp:Panel runat="server" ID="PnlgvOrderRetun" Visible="false">
                                            <asp:GridView ID="gvOrderReturn" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvOrderReturn_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField SortExpression="ReceivedQty">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnReceivedQty" runat="server" CommandArgument="ReceivedQty"
                                                                CommandName="Sort"><span>Received Qty<br /></span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderReceivedQty" runat="server"></asp:PlaceHolder>
                                                            <div class="corner">
                                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hdnitemNo" runat="server" Value='<%# Eval("ItemNumber") %>' />
                                                            <asp:HiddenField ID="hdnShoppingCartId" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />
                                                            <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                                                            <asp:HiddenField ID="hdnshippid" runat="server" Value='<%# Eval("ShippID") %>' />
                                                            <asp:HiddenField ID="hdnTrackingNumber" runat="server" Value='<%# Eval("TrackingNo") %>' />
                                                            <asp:HiddenField ID="hdnPackageID" runat="server" Value='<%# Eval("PackageId") %>' />
                                                            <asp:HiddenField ID="hdnShipDate" runat="server" Value='<%# Eval("ShipingDate") %>' />
                                                            <asp:Label ID="lblReceivedQty" runat="server" Text='<% # (Convert.ToString(Eval("QuantityReceived")).ToString().Length > 10) ? Eval("QuantityReceived").ToString().Substring(0,10)+"..." :  Convert.ToString(Eval("QuantityReceived")+ "&nbsp;") %>'
                                                                ToolTip='<%# Eval("QuantityReceived") %>'></asp:Label>
                                                            <div class="corner">
                                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                            </div>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="QtyOrder">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnQtyOrder" runat="server" CommandArgument="QtyOrder" CommandName="Sort"><span>Qty Order<br /></span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderQtyOrder" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQtyOrder" runat="server" Text='<% # (Convert.ToString(Eval("QtyOrder")).ToString().Length > 10) ? Eval("QtyOrder").ToString().Substring(0,10)+"..." :  Convert.ToString(Eval("QtyOrder")+ "&nbsp;") %>'
                                                                ToolTip='<%# Eval("QtyOrder") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ReturnQty">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnReturnQty" runat="server" CommandArgument="ReturnQty" CommandName="Sort"><span >Return Qty<br /></span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderReturnQty" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <span class="btn_space">
                                                                <asp:HiddenField ID="hfReturnQty" runat="server" />
                                                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                                                    text-align: center; color: #ffffff; width: 50px; padding: 2px;" onchange="CheckNum(this.id)"
                                                                    MaxLength="10" BackColor="#303030" ID="txtReturnQty"></asp:TextBox>
                                                            </span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Color">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnColor" runat="server" CommandArgument="Color" CommandName="Sort"> <span >Color<br /></span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderColor" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <span class="btn_space">
                                                                <asp:Image ID="imgColor" runat="server" Height="20" Width="20" />
                                                            </span>
                                                            <asp:Label runat="server" ID="lblColor"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="8%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Size">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span >Size</span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderSize" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSize" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ProductDescrption">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnProductDescrption" runat="server" CommandArgument="Shipper"
                                                                CommandName="Sort"><span >Description</span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderProductDescrption" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductionDescription" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box" Width="40%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Requesting" Visible="false">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnRequesting" runat="server" CommandArgument="Requesting"
                                                                CommandName="Sort"><span>Requesting</span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderRequesting" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span>
                                                                <%-- <asp:DropDownList runat="server" ID="ddlRequesting" Style="background-color: #303030;
                                                                    border: medium none; color: #ffffff; width: 100px; padding: 2px" runat="server">
                                                                    <asp:ListItem></asp:ListItem>
                                                                </asp:DropDownList>--%>
                                                            </span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box" Width="10%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="PnlgvReturnProductUpdate" Visible="false">
                                            <asp:GridView ID="gvReturnProductUpdate" runat="server" AutoGenerateColumns="false"
                                                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                                RowStyle-CssClass="ord_content" OnRowDataBound="gvReturnProductUpdate_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField SortExpression="ReceivedQty">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnReceivedQty" runat="server" CommandArgument="ReceivedQty"
                                                                CommandName="Sort"><span>Received Qty<br /></span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderReceivedQty" runat="server"></asp:PlaceHolder>
                                                            <div class="corner">
                                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hdnProductReturnId" runat="server" Value='<%# Eval("ProductReturnId") %>' />
                                                            <asp:HiddenField ID="hdnRequesting" runat="server" Value='<%# Eval("Requesting") %>' />
                                                            <asp:HiddenField ID="hdnitemNo" runat="server" Value='<%# Eval("ItemNumber") %>' />
                                                            <asp:HiddenField ID="hdnShoppingCartId" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />
                                                            <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                                                            <asp:HiddenField ID="hdnshippid" runat="server" Value='<%# Eval("ShippID") %>' />
                                                            <asp:HiddenField ID="hdnTrackingNumber" runat="server" Value='<%# Eval("TrackingNumber") %>' />
                                                            <asp:HiddenField ID="hdnPackageID" runat="server" Value='<%# Eval("PackageId") %>' />
                                                            <asp:HiddenField ID="hdnShipDate" runat="server" />
                                                            <asp:Label runat="server" ID="lblReceivedQty" />
                                                            <div class="corner">
                                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                            </div>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" Width="11%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="QtyOrder">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnQtyOrder" runat="server" CommandArgument="QtyOrder" CommandName="Sort"><span>Qty Order<br /></span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderQtyOrder" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblQtyOrder" />
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ReturnQty">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnReturnQty" runat="server" CommandArgument="ReturnQty" CommandName="Sort"><span >Return Qty<br /></span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderReturnQty" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <span class="btn_space">
                                                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                                                    text-align: center; color: #ffffff; width: 50px; padding: 2px" onchange="CheckNum(this.id)"
                                                                    MaxLength="10" BackColor="#303030" ID="txtReturnQty" Text='<%# Eval("ReturnQty") %>'></asp:TextBox>
                                                            </span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box" Width="8%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Color">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnQtyReceived" runat="server" CommandArgument="Color" CommandName="Sort"> <span >Color<br /></span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderColor" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <span class="btn_space">
                                                                <asp:Image ID="imgColor" runat="server" Height="20" Width="20" />
                                                            </span>
                                                            <asp:Label runat="server" ID="lblColor"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="8%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Size">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span >Size</span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderSize" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSize" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" Width="13%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ProductDescrption">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnProductDescrption" runat="server" CommandArgument="Shipper"
                                                                CommandName="Sort"><span >Description</span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderProductDescrption" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductionDescription" runat="server"></asp:Label>
                                                            <asp:HiddenField ID="hdnReason" runat="server" Value='<%# Eval("Reason") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box" Width="30%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Reason" Visible="false">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnReason" runat="server" CommandArgument="Reason" CommandName="Sort"><span >Reason<br /></span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderReason" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span>
                                                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                                                    color: #ffffff; width: 150px; padding: 2px" onchange="CheckNum(this.id)" MaxLength="200"
                                                                    BackColor="#303030" ID="txtReason" Text='<%# Eval("Reason") %>'></asp:TextBox>
                                                            </span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box" Width="15%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Requesting" Visible="false">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnRequesting" runat="server" CommandArgument="Requesting"
                                                                CommandName="Sort"><span>Requesting</span></asp:LinkButton>
                                                            <asp:PlaceHolder ID="placeholderRequesting" runat="server"></asp:PlaceHolder>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span>
                                                                <%--<asp:DropDownList runat="server" ID="ddlRequesting" Style="background-color: #303030;
                                                                    border: medium none; color: #ffffff; width: 100px; padding: 2px" runat="server">
                                                                    <asp:ListItem></asp:ListItem>
                                                                </asp:DropDownList>--%>
                                                            </span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box" Width="10%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <div class="spacer20">
                            </div>
                            <asp:Panel runat="server" ID="pnlReturnStatus" Visible="false">
                                <table id="tblProductDescription" runat="server">
                                    <tr>
                                        <td>
                                            <div class="form_table">
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box taxt_area clearfix" style="height: 100px">
                                                    <span class="input_label alignleft" style="height: 100px">Reason for Return:</span>
                                                    <div class="textarea_box alignright">
                                                        <div class="scrollbar" style="height: 103px">
                                                            <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                                            </a>
                                                        </div>
                                                        <asp:TextBox ID="txtPrdDescription" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                                            Height="100px"></asp:TextBox>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="spacer20">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_table">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box taxt_area clearfix" style="height: 100px">
                                                        <span class="input_label alignleft" style="height: 100px">Notes/History :</span>
                                                        <div class="textarea_box alignright">
                                                            <div class="scrollbar" style="height: 103px">
                                                                <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                                    class="scrollbottom"></a>
                                                            </div>
                                                            <asp:TextBox ID="txtOrderNotesForCECA" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                                                Height="100px" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </div>
                                                <div class="alignnone spacer15">
                                                </div>
                                                <div class="rightalign gallery" id="divAddNots" runat="server">
                                                    <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkAddNew" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright"><span>+ Add Note</span></asp:LinkButton>
                                                    <at:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkAddNew" BackgroundCssClass="modalBackground"
                                                        DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup">
                                                    </at:ModalPopupExtender>
                                                </div>
                                                <div>
                                                    <asp:Panel ID="pnlNotes" runat="server" Style="display: none;">
                                                        <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                                            left: 35%; top: 30%;">
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
                                                                                    <div class="pp_inline clearfix">
                                                                                        <div class="form_popup_box">
                                                                                            <div class="label_bar">
                                                                                                <span>Notes/History :
                                                                                                    <br />
                                                                                                    <br />
                                                                                                    <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNote" runat="server"></asp:TextBox></span>
                                                                                            </div>
                                                                                            <div>
                                                                                                <asp:LinkButton ID="lnkButton" CommandName="SAVECACE" class="grey2_btn" runat="server"
                                                                                                    OnClick="lnkButton_Click"><span>Save Notes</span></asp:LinkButton>
                                                                                                <%-- <asp:Button ID="btnSubmit" class="grey2_btn" Text="Add Note" runat="server"  OnClick="btnSubmit_Click"  />--%>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="pp_details clearfix" style="width: 371px;">
                                                                                    <a href="#" id="closepopup" runat="server" class="pp_close">Close</a>
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
                                                </div>
                                                <br />
                                                <br />
                                                <br />
                                                <%--End Note History--%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table class="dropdown_pad form_table">
                                    <tr>
                                        <td>
                                            <div class="botbtn centeralign">
                                                <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save Information"
                                                    OnClick="lnkBtnSaveInfo_Click"><span>Process Now</span></asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
        <div class="black2_round_bottom">
            <span></span>
        </div>
        <div class="alignnone">
            &nbsp;</div>
    </div>
    <asp:HiddenField runat="server" ID="hdnReturnQuantity" />
    <%--Modal Popup Panels--%>
</asp:Content>
