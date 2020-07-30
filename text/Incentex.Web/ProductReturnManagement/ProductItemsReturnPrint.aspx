<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ProductItemsReturnPrint.aspx.cs" Inherits="ProductReturnManagement_ProductItemsReturnPrint"
    Title="Print Return Authorization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        table
        {
            width: 100% !important;
        }
        .centeralign
        {
            text-align: center !important;
        }
        .btn_space a
        {
            margin-left: 12px !important;
        }
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
        .shipmentclass
        {
            background: none repeat scroll 0 0 #101010;
            color: #72757C;
            display: block;
            border: solid 1px #1c1c1c;
            padding: 0px 8px;
            border-left: none;
            border-right: solid 1px #1f1f1f;
        }
        .width300
        {
            font-size: small;
            width: 300px !important;
        }
    </style>

    <script type="text/javascript" language="javascript">
        window.onload = function() {
            printDiv();
        };
        function printDiv() {
            var prtContent = document.getElementById('printableArea');
            var WinPrint = window.open('', '', 'left=0,top=0,width=1,height=1,toolbar=0,scrollbars=0,status=0');
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
        }
   
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="printableArea">
        <table cellpadding='2' cellspacing='0' width='100%' border='0'>
            <tr>
                <td>
                    <div class="black_round_box">
                        <div class="black2_round_top">
                            <span></span>
                        </div>
                        <div class="black2_round_middle">
                            <div class="form_pad">
                                <div class="pro_search_pad" style="width: 920px;">
                                    <div id="mainDIV">
                                        <div>
                                            <%--First Div--%>
                                            <div class="black_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="black_middle order_detail_pad">
                                                <table class="order_detail" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 50%">
                                                            <table>
                                                                <tr>
                                                                    <td style="font-size: small;" class="fontArial">
                                                                        <label>
                                                                            Order Number :
                                                                        </label>
                                                                        <asp:Label ID="lblOrderNo" CssClass="fontArial" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="font-size: small;" class="fontArial">
                                                                        <label>
                                                                            Reference Name :
                                                                        </label>
                                                                        <asp:Label CssClass="fontArial" ID="lblOrderBy" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 50%">
                                                            <table>
                                                                <tr>
                                                                    <td style="font-size: small;">
                                                                        <label style="padding-left: 28px!important;" class="fontArial">
                                                                            Ordered Date :
                                                                        </label>
                                                                        <asp:Label ID="lblOrderedDate" CssClass="fontArial" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="font-size: small;">
                                                                        <label style="padding-left: 28px!important;" class="fontArial">
                                                                            Order Status :
                                                                        </label>
                                                                        <asp:Label CssClass="fontArial" runat="server" ID="lblOrderStatus"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="font-size: small;" class="fontArial">
                                                                        <label style="padding-left: 28px!important;">
                                                                            Payment Method :
                                                                        </label>
                                                                        <asp:Label ID="lblPaymentMethod" CssClass="fontArial" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trCreditType" runat="server">
                                                                    <td style="font-size: small;" class="fontArial">
                                                                        <label style="padding-left: 28px!important;">
                                                                            Credit Type :
                                                                        </label>
                                                                        <asp:Label ID="lblCreditType" CssClass="fontArial" runat="server"></asp:Label>
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
                                            <%--Second Div--%>
                                            <div class="black_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="black_middle order_detail_pad">
                                                <div class="clearfix billing_head">
                                                    <div style="float: left; width: 49% !important;" class="fontArial">
                                                        <span>Bill To: </span>
                                                    </div>
                                                    <div style="float: right; width: 49% !important;" class="fontArial">
                                                        <span style="padding-left: 29px!important;">Ship To:</span>
                                                    </div>
                                                </div>
                                                <div>
                                                    <div style="float: left; width: 49% !important;">
                                                        <div class="tab_content_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="tab_content">
                                                            <table class="order_detail" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblBName" runat="server" /></label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblBCompany" runat="server" /></label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblBAddress" runat="server" /></label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblBCity" runat="server" />
                                                                        </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblBCountry" runat="server" /></label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblBEmailAddress" runat="server" /></label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblBPhoneNumber" runat="server" /></label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div class="tab_content_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                    <div style="float: right; width: 49% !important;">
                                                        <div class="tab_content_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="tab_content">
                                                            <table class="order_detail" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblSName" runat="server" /></label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblSCompany" runat="server" /></label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblSAddress" runat="server" /></label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblSAddress2" runat="server" /></label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblSStreet" runat="server" /></label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblSCity" runat="server" />
                                                                        </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblSCountry" runat="server" />
                                                                        </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblSEmailAddress" runat="server" /></label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="width300">
                                                                            <asp:Label ID="lblSPhoneNumber" runat="server" /></label>
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
                                            <%--Order Summary Div, Third Div--%>
                                            <table width="100%">
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
                                                        <asp:GridView ID="gvMainOrderDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                                            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvMainOrderDetail_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Reason Code </span>
                                                                        <div class="corner">
                                                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle CssClass="centeralign" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblReason" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Reason") %>'></asp:Label>
                                                                        <div class="corner">
                                                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="g_box centeralign" Width="25%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Return Qty </span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblReturnQty" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem,"ReturnQty") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="centeralign" />
                                                                    <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Item # </span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="centeralign" />
                                                                    <ItemStyle CssClass="g_box" Width="15%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Product Description</span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductionDescription" Text='<%#Eval("ProductDescrption")%>' ToolTip='<% #Eval("ProductDescrption")  %>'
                                                                            runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="centeralign" />
                                                                    <ItemStyle CssClass="b_box" Width="30%" Wrap="true" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Shipped Qty</span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQtyShipped" runat="server" Text='<%# Eval("ShippedQty") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="centeralign" />
                                                                    <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Ordered Qty</span>
                                                                        <div class="corner">
                                                                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtQtyOrder" CssClass="first" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderQty") %>'></asp:Label>
                                                                        <div class="corner">
                                                                            <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="centeralign" />
                                                                    <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="text-align: right; padding-right: 28px;">
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="spacer15">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
