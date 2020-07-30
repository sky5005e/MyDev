<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="OrderDetail.aspx.cs" Inherits="MyAccount_OrderDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="content" class="black_round_middle">
        <div class="black_round_box">
            <div class="black2_round_top">
                <span></span>
            </div>
            <div class="black2_round_middle">
                <div class="form_pad">
                    <div class="pro_search_pad">
                        <div>
                            <div class="black_top_co">
                                <span>&nbsp;</span></div>
                            <div class="black_middle order_detail_pad">
                                <table class="order_detail" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                Order # :
                                            </label>
                                            <asp:Label ID="lblOrderId" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                Order Date :
                                            </label>
                                            <asp:Label ID="lblOrderDate" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                Reference Name :
                                            </label>
                                            <asp:Label ID="lblPurchasedBy" runat="server" />
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
                                        <span>Billing Information</span></div>
                                    <div class="alignright">
                                        <span>Shipping Information</span></div>
                                </div>
                                <div>
                                    <div class="alignleft" style="width: 49%;">
                                        <div class="tab_content_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="tab_content">
                                            <table class="order_detail" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Name :</label><asp:Label ID="lblBName" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Company :</label><asp:Label ID="lblBCompany" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Address :</label><asp:Label ID="lblBAddress" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            City :</label><asp:Label ID="lblBCity" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            State :</label><asp:Label ID="lblBState" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Zip :</label><asp:Label ID="lblBZip" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Country :</label><asp:Label ID="lblBCountry" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Phone :</label><asp:Label ID="lblBPhone" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Email :</label><asp:Label ID="lblBEmail" runat="server" />
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
                                                        <label>
                                                            Name :</label><asp:Label ID="lblSName" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Company :</label><asp:Label ID="lblSCompany" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Address 1 :</label><asp:Label ID="lblSAddress" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Address 2 :</label>
                                                        <asp:Label ID="lblSAddress2" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Suite/Apt. :</label><asp:Label ID="lblSStreet" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            City :</label><asp:Label ID="lblSCity" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            State :</label><asp:Label ID="lblSState" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Zip :</label><asp:Label ID="lblSZip" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Country :</label><asp:Label ID="lblSCountry" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Phone :</label><asp:Label ID="lblSPhone" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Email :</label><asp:Label ID="lblSEmail" runat="server" />
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
                            <div>
                                <table class="payment_option_box" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="header_box" style="width: 25%;">
                                            <div>
                                                <label>
                                                    Payment Method :
                                                </label>
                                                <span class="left_tco"></span><span class="left_bco"></span>
                                            </div>
                                        </td>
                                        <td class="condetail_box" style="width: 75%;">
                                            <div>
                                                <%--<label>
                                                    <img src="images/payment-icon-1.gif" alt="" />Payment Order :
                                                </label>--%>
                                                <asp:Label ID="lblPaymentMethod" runat="server" />
                                                <span class="right_tco"></span><span class="right_bco"></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="credittype" runat="server">
                                        <td class="header_box" style="width: 25%;">
                                            <div>
                                                <label>
                                                    Credit Type :
                                                </label>
                                                <span class="left_tco"></span><span class="left_bco"></span>
                                            </div>
                                        </td>
                                        <td class="condetail_box" style="width: 75%;">
                                            <div>
                                                <asp:Label ID="lblCreditType" runat="server" />
                                                <span class="right_tco"></span><span class="right_bco"></span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="spacer10">
                            </div>
                            <%--<div>
                                <table class="payment_option_box" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="header_box" style="width: 25%;">
                                            <div>
                                                <label>
                                                    Shipping Method :
                                                </label>
                                                <span class="left_tco"></span><span class="left_bco"></span>
                                            </div>
                                        </td>
                                        <td class="condetail_box" style="width: 75%;">
                                            <div>
                                                <label>
                                                    <img src="images/fedex-ground-icon.gif" alt="" /></label><asp:Label ID="lblShippingMethod"
                                                        runat="server" />
                                                <span class="right_tco"></span><span class="right_bco"></span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>--%>
                            <%--<div class="shopping_cart_line">
                                Order Notes:
                                <asp:Label ID="lblOrderNotes" runat="server" />
                            </div>--%>
                            <div class="spacer20">
                            </div>
                            <div>
                                <asp:Repeater ID="rptMyShoppingCart" runat="server" OnItemDataBound="rptMyShoppingCart_ItemDataBound">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0" runat="server" id="tblBulkOrderItem">
                                            <tr>
                                                <td style="width: 20%; vertical-align: top;">
                                                    <div class="alignleft">
                                                        <div class="agent_img">
                                                            <p class="upload_photo gallery">
                                                                <asp:HiddenField ID="hdnLookupIcon" Value='<%# DataBinder.Eval(Container.DataItem, "ProductImageID")%>'
                                                                    runat="server" />
                                                                <asp:HiddenField ID="hdndocumentname" runat="server" Value='<%# Eval("ProductImage") %>' />
                                                                <asp:HiddenField ID="hdnlargerimagename" runat="server" Value='<%# Eval("LargerProductImage") %>' />
                                                                <a id="prettyphotoDiv" href="~/UploadedImages/employeePhoto/employee-photo.gif" rel='prettyPhoto'
                                                                    runat="server">
                                                                    <img id="imgSplashImage" src="~/UploadedImages/employeePhoto/employee-photo.gif"
                                                                        height="198" width="145" runat="server" alt='Loading' />
                                                                </a>
                                                            </p>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="width: 2%;">
                                                </td>
                                                <td style="width: 78%;">
                                                    <table id="tblProductDescription" class="shoppingcart_box" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td colspan="6" style="padding: 0px;">
                                                                <div class="cart_header" id="dvBulkOrderHeader" runat="server">
                                                                    <div class="left_co">
                                                                    </div>
                                                                    <div class="right_co">
                                                                    </div>
                                                                    <table cellpadding="0" cellspacing="0" class="txt13">
                                                                        <tr>
                                                                            <td style="width: 30%;" class="leftalign">
                                                                                Product Description
                                                                            </td>
                                                                            <td style="width: 20%;">
                                                                                Size
                                                                            </td>
                                                                            <td style="width: 12%;">
                                                                                Quantity
                                                                            </td>
                                                                            <td style="width: 12%;">
                                                                                Unit Price
                                                                            </td>
                                                                            <td style="width: 14%;">
                                                                                Total
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6" class="spacer10">
                                                            </td>
                                                        </tr>
                                                        <tr class="txt14">
                                                            <td style="width: 30%;" class="leftalign" colspan="2">
                                                                <asp:Label runat="server" ID="lblProductDescription"></asp:Label>
                                                                <%--<%# DataBinder.Eval(Container,"DataItem.ProductDescrption") %>--%>
                                                                <asp:HiddenField ID="hdnIsBulkOrder" runat="server" Value='<%# DataBinder.Eval(Container,"DataItem.IsBulkOrder") %>' />
                                                                <asp:HiddenField runat="server" ID="hdnProductDescrption" Value='<%# DataBinder.Eval(Container,"DataItem.ProductDescrption") %>' />
                                                                <asp:HiddenField runat="server" ID="hdnItemNumber" Value='<%# DataBinder.Eval(Container,"DataItem.item") %>' />
                                                                <asp:HiddenField runat="server" ID="hdnStoreProductid" Value='<%# DataBinder.Eval(Container,"DataItem.StoreProductID") %>' />
                                                                <asp:HiddenField runat="server" ID="hdnMasterItemNo" Value='<%# DataBinder.Eval(Container,"DataItem.MasterItemNo") %>' />
                                                            </td>
                                                            <td style="width: 20%;">
                                                                <%# DataBinder.Eval(Container,"DataItem.Size") %>
                                                            </td>
                                                            <td style="width: 12%;">
                                                                <div class="qty_selbox alignleft">
                                                                    <asp:Label runat="server" Width="25px" ID="lblQuantity" Text='<%# DataBinder.Eval(Container, "DataItem.Quantity")%>'></asp:Label>
                                                                </div>
                                                            </td>
                                                            <td style="width: 12%;">
                                                                <div class="qty_selbox alignleft">
                                                                    <asp:Label runat="server" ID="lblUnitPrice" Text='<%#Eval("RunCharge") != "" ? (Convert.ToDecimal(Eval("RunCharge")) + Convert.ToDecimal(Eval("UnitPrice"))) : Eval("UnitPrice")%>'></asp:Label>
                                                                </div>
                                                            </td>
                                                            <td style="width: 14%;">
                                                                <div class="qty_selbox alignleft">
                                                                    <asp:Label runat="server" ID="lblTotal"></asp:Label>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6" class="spacer10">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6" style="padding-left: 15px;">
                                                                <asp:Panel ID="pnlNameBars" runat="server" Visible="false">
                                                                    <table>
                                                                        <tr>
                                                                            <td style="padding: 0px;">
                                                                                <div id="Div1" class="alignleft" runat="server">
                                                                                    Name to be Engraved:
                                                                                    <asp:Label ID="lblNameTobeEngraved" runat="server"></asp:Label>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="padding: 0px;">
                                                                                <div id="divEmpTitle" class="alignleft" runat="server">
                                                                                    <asp:Label ID="lblEmplTitleView" runat="server" Text="Employee Title:"></asp:Label>
                                                                                    <asp:Label ID="lblEmplTitle" runat="server"></asp:Label>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr class="txt14">
                                                            <td colspan="6">
                                                                <asp:Panel ID="pnlTailoring" runat="server">
                                                                    <div class="cart_header alignleft">
                                                                        <div class="left_co">
                                                                        </div>
                                                                        <div class="right_co">
                                                                        </div>
                                                                        <div class="tailoring_detail_pad">
                                                                            <img src="~/Images/tailoring-icon.gif" alt="" /><span>Tailoring Details</span>
                                                                            <br />
                                                                            <span style="color: #72757C;">Length:
                                                                                <asp:Label runat="server" Width="25px" ID="lblTailoringLength" Text=' <%# DataBinder.Eval(Container, "DataItem.TailoringLength")%>'></asp:Label>
                                                                            </span>
                                                                            <br />
                                                                            <span style="color: #72757C;">Run Charge:
                                                                                <asp:Label runat="server" Width="25px" ID="lblRunCharge" Text=' <%# DataBinder.Eval(Container, "DataItem.RunCharge")%>'></asp:Label>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <SeparatorTemplate>
                                        <div class="spacer10">
                                        </div>
                                    </SeparatorTemplate>
                                    <FooterTemplate>
                                        <table style="display: none;">
                                            <tr class="total_count txt14">
                                                <td colspan="5" class="rightalign" style="width: 89%;">
                                                    Total :
                                                </td>
                                                <td class="rightalign">
                                                    <asp:Label runat="server" ID="lblTotalProductPrice"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="spacer10">
                            </div>
                            <!--Bulkorder Order-->
                            <div>
                            </div>
                            <!--End-->
                            <div class="spacer10">
                            </div>
                            <div>
                                <asp:Repeater ID="rptMyIssuanceCart" runat="server" OnItemDataBound="rptMyIssuanceCart_ItemDataBound">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 20%; vertical-align: top;">
                                                    <div class="aligncenter">
                                                        <asp:HiddenField ID="hdnIsbackorderStatus" runat="server" Value='<%# Eval("BackOrderStatus") %>' />
                                                        <asp:Label runat="server" CssClass="errormessage" ID="lblIssuanceBackorder"></asp:Label>
                                                    </div>
                                                    <div class="alignleft">
                                                        <div class="agent_img">
                                                            <p class="upload_photo gallery">
                                                                <asp:HiddenField ID="hdnLookupIcon" Value='<%# DataBinder.Eval(Container.DataItem, "StoreProductImageId")%>'
                                                                    runat="server" />
                                                                <asp:HiddenField ID="hdndocumentname" runat="server" Value='<%# Eval("ProductImage") %>' />
                                                                <asp:HiddenField ID="hdnlargerimagename" runat="server" Value='<%# Eval("LargerProductImage") %>' />
                                                                <a id="prettyphotoDiv" href="~/UploadedImages/employeePhoto/employee-photo.gif" rel='prettyPhoto'
                                                                    runat="server">
                                                                    <img id="imgSplashImage" src="~/UploadedImages/employeePhoto/employee-photo.gif"
                                                                        height="198" width="145" runat="server" alt='Loading' />
                                                                </a>
                                                            </p>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="width: 2%;">
                                                </td>
                                                <td style="width: 78%;">
                                                    <table id="tblProductDescription" class="shoppingcart_box" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td colspan="6" style="padding: 0px;">
                                                                <div class="cart_header">
                                                                    <div class="left_co">
                                                                    </div>
                                                                    <div class="right_co">
                                                                    </div>
                                                                    <table cellpadding="0" cellspacing="0" class="txt13">
                                                                        <tr>
                                                                            <td style="width: 30%;" class="leftalign">
                                                                                Product Description
                                                                            </td>
                                                                            <td style="width: 20%;">
                                                                                Size
                                                                            </td>
                                                                            <td style="width: 16%;">
                                                                                Quantity
                                                                            </td>
                                                                            <td style="width: 16%;">
                                                                                Unit Price
                                                                            </td>
                                                                            <td style="width: 18%;">
                                                                                Total
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6" class="spacer10">
                                                            </td>
                                                        </tr>
                                                        <tr class="txt14">
                                                            <td style="width: 30%;" class="leftalign">
                                                                <span style="font-size: small;">
                                                                    <%# DataBinder.Eval(Container,"DataItem.ProductDescrption") %>
                                                                </span>
                                                            </td>
                                                            <td style="width: 20%;">
                                                                <%# DataBinder.Eval(Container, "DataItem.sLookupName")%>
                                                            </td>
                                                            <td style="width: 16%;">
                                                                <div class="qty_selbox alignleft">
                                                                    <asp:Label runat="server" Width="25px" ID="lblQuantityIssuance" Text='<%# DataBinder.Eval(Container, "DataItem.Qty")%>'></asp:Label>
                                                                </div>
                                                            </td>
                                                            <td style="width: 16%;">
                                                                <div class="qty_selbox alignleft">
                                                                    <asp:Label runat="server" ID="lblUnitPriceIssuance" Text='<%#Eval("RunCharge") != "" ? (Convert.ToDecimal(Eval("RunCharge")) + Convert.ToDecimal(Eval("Rate"))) : Eval("Rate")%>'></asp:Label>
                                                                </div>
                                                            </td>
                                                            <td style="width: 18%;">
                                                                <div class="qty_selbox alignleft">
                                                                    <asp:Label runat="server" ID="lblTotalIssuance"></asp:Label>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6" class="spacer10">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" id="pnlNameBarsForIssuance" runat="server" style="text-align: left;"
                                                                class="alignleft">
                                                                Name to be Engraved:
                                                                <asp:Label ID="lblNameTobeEngravedForIssuance" runat="server"></asp:Label>
                                                            </td>
                                                            <td colspan="4">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" id="pnlEmpTitleForIssuance" runat="server" style="text-align: left;"
                                                                class="alignleft">
                                                                Employee Title:
                                                                <asp:Label ID="lblEmplTitleForIssuance" runat="server"></asp:Label>
                                                            </td>
                                                            <td colspan="4">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6" class="spacer10">
                                                            </td>
                                                        </tr>
                                                        <tr class="txt14">
                                                            <td colspan="6">
                                                                <asp:Panel ID="pnlIssuanceTailoring" runat="server">
                                                                    <div class="cart_header alignleft">
                                                                        <div class="left_co">
                                                                        </div>
                                                                        <div class="right_co">
                                                                        </div>
                                                                        <div class="tailoring_detail_pad">
                                                                            <img src="../Images/tailoring-icon.gif" alt="" /><span>Tailoring Details</span>
                                                                            <br />
                                                                            <span style="color: #72757C;">Length:
                                                                                <asp:Label runat="server" Width="25px" ID="lblTailoringLength" Text=' <%# DataBinder.Eval(Container, "DataItem.TailoringLength")%>'></asp:Label>
                                                                            </span>
                                                                            <br />
                                                                            <span style="color: #72757C;">Run Charge:
                                                                                <asp:Label runat="server" Width="25px" ID="lblRunChargeForIssuance" Text=' <%# DataBinder.Eval(Container, "DataItem.RunCharge")%>'></asp:Label>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <SeparatorTemplate>
                                        <div class="spacer10">
                                        </div>
                                    </SeparatorTemplate>
                                    <FooterTemplate>
                                        <table style="display: none;">
                                            <tr class="total_count txt14">
                                                <td colspan="5" class="rightalign" style="width: 89%;">
                                                    Total :
                                                </td>
                                                <td class="rightalign">
                                                    <asp:Label runat="server" ID="lblTotalProductPrice"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="spacer10">
                            </div>
                            <div>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 80%;">
                                            <table class="shoppingcart_box">
                                                <tr class="total_count" id="trstartingCredit" runat="server">
                                                    <td colspan="5" class="rightalign">
                                                        <asp:Label ID="lblStrCrAmntAppliedLabel" runat="server" />
                                                    </td>
                                                    <td class="rightalign">
                                                        $<asp:Label ID="lblStCrAmntApplied" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr class="total_count" id="trpaymentmethod" runat="server">
                                                    <td colspan="5" class="rightalign">
                                                        <asp:Label ID="lblPaymentMethodUsed" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="rightalign">
                                                        $<asp:Label ID="lblPaymentMethodAmount" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <%--<tr class="total_count">
                                                    <td colspan="5" class="rightalign">
                                                        Sales Tax :
                                                    </td>
                                                    <td class="rightalign">
                                                        <asp:Label ID="lblSalesTax" runat="server" />
                                                    </td>
                                                </tr>--%>
                                                <tr class="total_count">
                                                    <td colspan="5" class="rightalign" style="width: 90%;">
                                                        Total Order Amount :
                                                    </td>
                                                    <td class="rightalign">
                                                        $<asp:Label ID="lblOrderTotal" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="spacer20">
                            </div>
                            <div class="black_top_co">
                                <span>&nbsp;</span>
                            </div>
                            <div class="black_middle order_detail_pad" style="font-size: x-small;">
                                <table class="order_detail" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="font-size: small;">
                                            <label>
                                                Order Notes:
                                            </label>
                                            <asp:Label ID="lblOrderNotes" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="black_bot_co">
                                <span>&nbsp;</span>
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
    </div>
    <asp:HiddenField ID="hdnStroreproductnewChange" runat="server" Value="0" />
    <asp:HiddenField ID="hdnBulkOrdernew" runat="server" Value="0" />
</asp:Content>
