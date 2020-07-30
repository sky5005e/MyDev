<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderDetailForPrint.aspx.cs" Inherits="OrderManagement_OrderDetailForPrint"
    Title="Print Order" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%--<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="mb" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <style type="text/css">
        *
        {
            font-family: Arial !important;
        }
        .textarea_box
        {
            position: relative;
            background: none;
            width: 79%;
            margin-right: -5px;
            height: 78px;
        }
        .textarea_box textarea
        {
            height: 78px;
            background: none;
            border: none;
            font-size: 11px;
            color: #f4f4f4;
            vertical-align: top;
            width: 100%;
            overflow-x: hidden;
        }
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

    <script type="text/javascript">
        
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });


        

         
    </script>

    <script type="text/javascript" language="javascript">
        
        $().ready(function() {
            //$(".collapsibleContainer").wrapAll();
            $(".collapsibleContainer").collapsiblePanel();
            //$('#header').remove();
            window.print();
    });
   
    </script>

    <%--<mb:MenuUserControl ID="manuControl" runat="server" />--%>
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
                                <%--<mb:MenuUserControl ID="menucontrol" runat="server" />--%>
                                <div id="mainDIV">
                                    <div>
                                        <%--First Div--%>
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
                                                            <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                                              {%>
                                                            <tr>
                                                                <td style="font-size: small;" class="fontArial">
                                                                    <label style="padding-left: 28px!important;">
                                                                        Payment Method :
                                                                    </label>
                                                                    <asp:Label ID="lblPaymentMethod" CssClass="fontArial" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <%} %>
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
                                                <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                                  {%>
                                                <div style="float: left; width: 49% !important;" class="fontArial">
                                                    <span>Bill To: </span>
                                                </div>
                                                <%}
                                                  else
                                                  {%>
                                                <div style="float: left; width: 49% !important;" class="fontArial">
                                                    <span>&nbsp;</span>
                                                </div>
                                                <%} %>
                                                <div style="float: right; width: 49% !important;" class="fontArial">
                                                    <span style="padding-left: 29px!important;">Ship To:</span>
                                                </div>
                                            </div>
                                            <div>
                                                <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                                  {%>
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
                                                                        <asp:Label ID="lblBAddress1" runat="server" /></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <label class="width300">
                                                                        <asp:Label ID="lblBAddress2" runat="server" /></label>
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
                                                <%}
                                                  else
                                                  {%>
                                                <div style="float: left; width: 49% !important;">
                                                    &nbsp;
                                                </div>
                                                <%} %>
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
                                                            <asp:TemplateField SortExpression="QuantityOrder">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="lnkbtnQuantityOrder" runat="server" CommandArgument="OrderNumber"
                                                                        CommandName="Sort"><span>Ordered<br /></span></asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <HeaderStyle CssClass="centeralign" />
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdnMyShoppingCartiD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"MyShoppingCartiD") %>' />
                                                                    <asp:Label ID="txtQtyOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="RemainingOrderQuantity" Visible="false">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="lnkbtnQuantityOrderRem" runat="server" CommandArgument="OrderNumber"
                                                                        CommandName="Sort"><span >Remaining<br /></span></asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <HeaderStyle CssClass="centeralign" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="txtQtyOrderRemaining" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="QtyShipped">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="lnkbtnQtyShipped" runat="server" CommandArgument="QtyShipped"
                                                                        CommandName="Sort"><span >Shipped<br /></span></asp:LinkButton>
                                                                    <asp:PlaceHolder ID="placeholderQtyShipped" runat="server"></asp:PlaceHolder>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="txtQtyShipped" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="centeralign" />
                                                                <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ItemNumber">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                                                        CommandName="Sort"><span>Item #<br /></span></asp:LinkButton>
                                                                    <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                                                    <asp:Label runat="server" ID="lblProductItemID" Text='<%# Eval("ProductItemID") %>'
                                                                        Visible="false" />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="centeralign" />
                                                                <ItemStyle CssClass="g_box" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Color">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="lnkbtnQtyReceived" runat="server" CommandArgument="Color" CommandName="Sort"> <span >Color<br /></span></asp:LinkButton>
                                                                    <asp:PlaceHolder ID="placeholderColor" runat="server"></asp:PlaceHolder>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <span class="btn_space">
                                                                        <asp:Image ID="imgColor" runat="server" Height="20" Width="20" />
                                                                    </span>
                                                                    <asp:Label runat="server" ID="lblColor" Text='<%# DataBinder.Eval(Container.DataItem,"Color") %>'
                                                                        Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="centeralign" />
                                                                <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Size">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span >Size</span></asp:LinkButton>
                                                                    <asp:PlaceHolder ID="placeholderSize" runat="server"></asp:PlaceHolder>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSize" ToolTip='<%# DataBinder.Eval(Container.DataItem,"Size") %>'
                                                                        Text='<%#(Eval("Size").ToString().Length > 8) ? Eval("Size").ToString().Substring(0, 8) + "..." : Eval("Size")%>'
                                                                        runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="centeralign" />
                                                                <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ProductDescrption">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="lnkbtnProductDescrption" runat="server" CommandArgument="Shipper"
                                                                        CommandName="Sort"><span>Description</span></asp:LinkButton>
                                                                    <asp:PlaceHolder ID="placeholderProductDescrption" runat="server"></asp:PlaceHolder>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%--<asp:Label ID="lblProductionDescription" Text='<%#(Eval("ProductDescrption").ToString().Length > 50) ? Eval("ProductDescrption").ToString().Substring(0,50)+"..." : Eval("ProductDescrption")  %>'
                                                                        ToolTip='<% #Eval("ProductDescrption")  %>' runat="server"></asp:Label>--%>
                                                                    <asp:Label ID="lblProductionDescription" Text='<%#Eval("ProductDescrption")%>' ToolTip='<% #Eval("ProductDescrption")  %>'
                                                                        runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="centeralign" />
                                                                <ItemStyle CssClass="b_box" Width="21%" Wrap="true" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Price">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="lnkbtnPrice" runat="server" CommandArgument="Price" CommandName="Sort"><span>Price</span></asp:LinkButton>
                                                                    <asp:PlaceHolder ID="placeholderPrice" runat="server"></asp:PlaceHolder>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPrice" Text=' <%# "$" + Convert.ToDecimal(Eval("Price")).ToString("#,##0.00")%>'
                                                                        runat="server"></asp:Label>
                                                                    <asp:Label ID="lblMOASPrice" Text=' <%# "$" + Convert.ToDecimal(Eval("MOASItemPrice")).ToString("#,##0.00")%>'
                                                                        runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="centeralign" />
                                                                <ItemStyle CssClass="g_box centeralign" Width="4%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="ExtendedPrice">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="lnkbtnExtendedPrice" runat="server" CommandArgument="ExtendedPrice"
                                                                        CommandName="Sort"><span>Extended</span></asp:LinkButton>
                                                                    <asp:PlaceHolder ID="placeholderExtendedPrice" runat="server"></asp:PlaceHolder>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExtendedPrice" Text='<%# "$" + Convert.ToDecimal((Convert.ToDecimal(Eval("Price")) * Convert.ToDecimal(Eval("Quantity")))).ToString("#,##0.00")%>'
                                                                        runat="server"></asp:Label>
                                                                    <asp:Label ID="lblMOASExtendedPrice" Text='<%# "$" + Convert.ToDecimal((Convert.ToDecimal(Eval("MOASItemPrice")) * Convert.ToDecimal(Eval("Quantity")))).ToString("#,##0.00")%>'
                                                                        runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="centeralign" />
                                                                <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="BackorderedUntil" Visible="false">
                                                                <HeaderTemplate>
                                                                    <asp:LinkButton ID="lnkbtnBackorderedUntil" runat="server" CommandArgument="BackorderedUntil "
                                                                        CommandName="Sort"><span >Backordered Until </span></asp:LinkButton>
                                                                    <asp:PlaceHolder ID="placeholderBackorderedUntil" runat="server"></asp:PlaceHolder>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <span class="calender_l">
                                                                        <asp:TextBox ID="txtBackOrderDate" Style="background-color: #303030; border: medium none;
                                                                            color: #ffffff; width: 80px; padding: 2px" runat="server" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                                                                    </span>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="centeralign" />
                                                                <ItemStyle CssClass="g_box" Width="10%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                              {%>
                                            <tr>
                                                <td align="right" style="text-align: right; padding-right: 28px;">
                                                    <table>
                                                        <tr>
                                                            <td class="rightalign">
                                                                <label>
                                                                    Shipping: $
                                                                </label>
                                                                <asp:Label ID="lblShipping" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="rightalign">
                                                                <label>
                                                                    Sales Tax: $
                                                                </label>
                                                                <asp:Label ID="lblSalesTax" runat="server"></asp:Label>
                                                                <asp:Label ID="lblStrikeIronSalesTax" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="rightalign">
                                                                <label>
                                                                    Order Total: $
                                                                </label>
                                                                <asp:Label ID="lblOrderTotal" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <%} %>
                                        </table>
                                    </div>
                                    <%--repeater Div--%>
                                    <div class="black_middle order_detail_pad">
                                        <asp:Repeater ID="parentRepeater" runat="server" OnItemDataBound="parentRepeater_ItemDataBound">
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div class="collapsibleContainer" title="<%# DataBinder.Eval(Container.DataItem, "CompanyName")%>"
                                                    align="center">
                                                    <asp:HiddenField ID="hdnSupplierId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Supplierid") %>' />
                                                    <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                                      {%>
                                                    <table>
                                                        <tr>
                                                            <td width="50%">
                                                                <table class="order_detail" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <label class="fontsizesmall">
                                                                                Vendor:
                                                                            </label>
                                                                            <asp:Label CssClass="fontsizesmall" runat="server" ID="lblSupplierCompanyName" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName")%>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label>
                                                                            </label>
                                                                            <asp:Label runat="server" CssClass="fontsizesmall" ID="lblVendorAdress1" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyAdddress")%>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <%--style="padding-left: 68px;"--%>
                                                                            <label>
                                                                            </label>
                                                                            <asp:Label runat="server" CssClass="fontsizesmall" ID="lblVendorCity" Text='<%# DataBinder.Eval(Container.DataItem, "CityName")%>'> </asp:Label>
                                                                            ,
                                                                            <asp:Label runat="server" ID="lblStateName" Text='<%# DataBinder.Eval(Container.DataItem, "sStatename")%>'
                                                                                CssClass="fontsizesmall"></asp:Label>
                                                                            <asp:Label runat="server" ID="lblCompanyZip" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyZip")%>'
                                                                                CssClass="fontsizesmall"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="50%">
                                                                <table class="order_detail">
                                                                    <tr>
                                                                        <td style="font-size: small;">
                                                                            <label>
                                                                                Ship To Address :
                                                                            </label>
                                                                            <asp:Label ID="lblShipContaName" runat="server"></asp:Label>
                                                                            <%--Text='<%#  DataBinder.Eval(Container.DataItem, "ShippingName") %>'--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label>
                                                                            </label>
                                                                            <asp:Label ID="lblShipToAddress1" runat="server" CssClass="fontsizesmall"></asp:Label>
                                                                            <%--Text='<%#  DataBinder.Eval(Container.DataItem, "ShippAddress") %>'--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label>
                                                                            </label>
                                                                            <asp:Label ID="lblShipToAddress2" CssClass="fontsizesmall" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label>
                                                                            </label>
                                                                            <asp:Label ID="lblShipZipCode" CssClass="fontsizesmall" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label>
                                                                            </label>
                                                                            <asp:Label ID="lblCountryName" CssClass="fontsizesmall" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table class="order_detail">
                                                                    <tr>
                                                                        <td>
                                                                            <label class="fontsizesmall">
                                                                                Contact:
                                                                            </label>
                                                                            <asp:Label runat="server" ID="lblContactVendor" CssClass="fontsizesmall" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>'></asp:Label>
                                                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Supplierid") %>' />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label class="fontsizesmall">
                                                                                Account # :
                                                                            </label>
                                                                            <asp:Label runat="server" ID="lblAccountNo" CssClass="fontsizesmall" Text='<%# DataBinder.Eval(Container.DataItem, "BankAccountNumber")%>'></asp:Label>
                                                                            <asp:HiddenField ID="hdnMyShoppingCartId" runat="server" />
                                                                            <asp:HiddenField ID="hdnOrderFor" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label class="fontsizesmall">
                                                                                Tel :
                                                                            </label>
                                                                            <asp:Label runat="server" ID="lblSupplierTelephone" CssClass="fontsizesmall" Text='<%# DataBinder.Eval(Container.DataItem, "Telephone")%>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label class="fontsizesmall">
                                                                                Email :
                                                                            </label>
                                                                            <asp:Label runat="server" ID="lblContactEmail" CssClass="fontsizesmall" Text='<%# DataBinder.Eval(Container.DataItem, "ContactEmail")%>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <%} %>
                                                    <table>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:GridView ID="gvOrderDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                                                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvOrderDetail_RowCommand"
                                                                    OnRowDataBound="gvOrderDetail_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField SortExpression="ItemNumber">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                                                                    CommandName="Sort"><span>Item #<br /></span></asp:LinkButton>
                                                                                <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle CssClass="centeralign" />
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="b_box" Width="12%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="QuantityOrder">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="lnkbtnQuantityOrder" runat="server" CommandArgument="OrderNumber"
                                                                                    CommandName="Sort"><span >Ordered<br /></span></asp:LinkButton>
                                                                                <asp:PlaceHolder ID="placeholderQuantityOrder" runat="server"></asp:PlaceHolder>
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle CssClass="centeralign" />
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnMyShoppingCartiD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"MyShoppingCartiD") %>' />
                                                                                <asp:Label ID="txtQtyOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="g_box centeralign" Width="2%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="RemainingOrderQuantity">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="lnkbtnQuantityOrderRem" runat="server" CommandArgument="OrderNumber"
                                                                                    CommandName="Sort"><span >Remaining<br /></span></asp:LinkButton>
                                                                                <asp:PlaceHolder ID="placeholderQuantityOrder" runat="server"></asp:PlaceHolder>
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle CssClass="centeralign" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtQtyOrderRemaining" runat="server"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="b_box centeralign" Width="2%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="QtyShipped" Visible="false">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="lnkbtnQtyShipped" runat="server" CommandArgument="QtyShipped"
                                                                                    CommandName="Sort"><span >Shipped<br /></span></asp:LinkButton>
                                                                                <asp:PlaceHolder ID="placeholderQtyShipped" runat="server"></asp:PlaceHolder>
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle CssClass="centeralign" />
                                                                            <ItemTemplate>
                                                                                <span class="btn_space">
                                                                                    <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                                                                        color: #ffffff; width: 50px; padding: 2px" MaxLength="10" BackColor="#303030"
                                                                                        ID="txtQtyShipped"></asp:TextBox>
                                                                                </span>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="g_box centeralign" Width="3%" />
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
                                                                                <asp:Label runat="server" ID="lblColor" Text='<%# DataBinder.Eval(Container.DataItem,"Color") %>'
                                                                                    Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="b_box centeralign" Width="6%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="Size">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span >Size</span></asp:LinkButton>
                                                                                <asp:PlaceHolder ID="placeholderSize" runat="server"></asp:PlaceHolder>
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle CssClass="centeralign" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSize" ToolTip='<%# DataBinder.Eval(Container.DataItem,"Size") %>'
                                                                                    Text='<%#(Eval("Size").ToString().Length > 5) ? Eval("Size").ToString().Substring(0, 5) + "..." : Eval("Size")%>'
                                                                                    runat="server"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="ProductDescrption">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="lnkbtnProductDescrption" runat="server" CommandArgument="Shipper"
                                                                                    CommandName="Sort"><span>Description</span></asp:LinkButton>
                                                                                <asp:PlaceHolder ID="placeholderProductDescrption" runat="server"></asp:PlaceHolder>
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle CssClass="centeralign" />
                                                                            <ItemTemplate>
                                                                                <span>
                                                                                    <%--<asp:Label ID="lblProductionDescription" Text='<%#(Eval("ProductDescrption").ToString().Length >50) ? Eval("ProductDescrption").ToString().Substring(0,50)+"..." : Eval("ProductDescrption")  %>'
                                                                                        ToolTip='<% #Eval("ProductDescrption")  %>' runat="server"></asp:Label>--%>
                                                                                    <asp:Label ID="lblProductionDescription" Text='<%#Eval("ProductDescrption")%>' ToolTip='<% #Eval("ProductDescrption")  %>'
                                                                                        runat="server"></asp:Label>
                                                                                </span>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="b_box" Width="19%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="BackorderedUntil" Visible="false">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="lnkbtnBackorderedUntil" runat="server" CommandArgument="BackorderedUntil "
                                                                                    CommandName="Sort"><span >Backordered Until </span></asp:LinkButton>
                                                                                <asp:PlaceHolder ID="placeholderBackorderedUntil" runat="server"></asp:PlaceHolder>
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle CssClass="centeralign" />
                                                                            <ItemTemplate>
                                                                                <span class="calender_l">
                                                                                    <asp:TextBox ID="txtBackOrderDate" Style="background-color: #303030; border: medium none;
                                                                                        color: #ffffff; width: 80px; padding: 2px" runat="server" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                                                                                </span>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="g_box" Width="10%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="Edit" Visible="false">
                                                                            <HeaderTemplate>
                                                                                <asp:LinkButton ID="lnkbtnShippment" runat="server" CommandArgument="Edit" CommandName="Sort"><span>Edit</span></asp:LinkButton>
                                                                                <asp:PlaceHolder ID="placeholderShippment" runat="server"></asp:PlaceHolder>
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle CssClass="centeralign" />
                                                                            <ItemTemplate>
                                                                                <asp:HyperLink ID="hypEditShippmentOrder" CommandName="EditShippingOrder" runat="server"><span>Edit</span></asp:HyperLink>
                                                                                <%--<asp:HiddenField ID="hdnShippment" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />--%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="spacer20">
                                                    </div>
                                                </div>
                                                <div class="spacer15">
                                                </div>
                                                <%--<hr />--%>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div class="spacer15">
                                    </div>
                                    <%--Special Order Instruction Div--%>
                                    <div class="black_middle order_detail_pad" id="CompanyNoteHistory" runat="server">
                                        <%--Popup Notes--%>
                                        <div class="form_table">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box taxt_area clearfix" style="height: 100px">
                                                <span class="input_label alignleft" style="height: 100px">Order Special Instruction</span>
                                                <div class="textarea_box alignright">
                                                    <asp:TextBox ID="txtOrderNotesForCECA" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                                        Height="100px" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                        <%--End Note History--%>
                                    </div>
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
            </td>
        </tr>
    </table>
    <input type="hidden" id="hfX" value="" runat="server" />
    <input type="hidden" id="hfY" value="" runat="server" />
    <%--Modal Popup Panels--%>
</asp:Content>
