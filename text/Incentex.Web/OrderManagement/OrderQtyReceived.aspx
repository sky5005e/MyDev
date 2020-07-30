<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="OrderQtyReceived.aspx.cs" Inherits="OrderManagement_OrderQtyReceived" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calendar-small.png',
                buttonImageOnly: true
            });
        });

        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }
        
    </script>

    <style type="text/css">
        .fontsizemedium
        {
            font-size: small;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="form_pad">
        <div>
            <div style="text-align: center; color: Red; font-size: larger;">
                <asp:Label ID="lblmsg" runat="server">
                </asp:Label>
            </div>
            <div class="centeralign">
                <table class="order_detail" border="0">
                    <tr valign="bottom">
                        <td style="width: 7%">
                            <span class="fontsizemedium">Ship Date: </span>
                        </td>
                        <td style="width: 93%">
                            <span class="fontsizemedium">
                                <asp:Label ID="txtShipDate" runat="server"></asp:Label>
                            </span>
                        </td>
                    </tr>
                    <%--<tr>
                        <td>
                            <span class="fontsizemedium">Shipper: </span>
                        </td>
                        <td>
                            <span class="fontsizemedium">
                                <asp:Label ID="lblShipper" runat="server"></asp:Label>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="fontsizemedium">Tracking Numbers: </span>
                        </td>
                        <td colspan="2">
                            <asp:GridView ID="grvTrackingNumber" runat="server" OnRowCommand="grvTrackingNumber_RowCommand"
                                AutoGenerateColumns="false" OnRowDeleting="grvTrackingNumber_RowDeleting" ShowHeader="false"
                                BorderWidth="0px">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTrackingNumber" Text='<%#Eval("trackingnuber")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle BorderWidth="0px" Font-Size="Small" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>--%>
                </table>
            </div>
            <div class="alignnone spacer15">
            </div>
            <div>
                <asp:GridView ID="gvShippedOrderDetail" runat="server" AutoGenerateColumns="false"
                    HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                    RowStyle-CssClass="ord_content" OnRowCommand="gvShippedOrderDetail_RowCommand"
                    OnRowDataBound="gvShippedOrderDetail_RowDataBound" OnRowEditing="gvShippedOrderDetail_RowEditing">
                    <Columns>
                        <asp:TemplateField SortExpression="ShipQuantity" Visible="false">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnShipQuantity" runat="server" CommandArgument="ShipQuantity"
                                    CommandName="Sort"><span ></span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderShipQuantity" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblShipId" runat="server" Text='<%# Eval("ShippID") %>'></asp:Label>
                                <asp:HiddenField runat="server" ID="hdnQuantityShipped" Value='<%# Eval("ShipQuantity") %>' />
                                <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                                <asp:HiddenField ID="hdnMyShoppingCartId" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />
                                <asp:HiddenField ID="hdnSupplierid" runat="server" Value='<%# Eval("SupplierId") %>' />
                                <asp:HiddenField ID="hdhQtyOrder" runat="server" Value='<%# Eval("QtyOrder") %>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="g_box centeralign" Width="2%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="ItemNumber">
                            <HeaderTemplate>
                                <span>
                                    <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                        CommandName="Sort" Text="Item #"></asp:LinkButton>
                                </span>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                                <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <span class="first">
                                    <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                </span>
                                <div class="corner">
                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="g_box " Width="12%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="QuantityOrder">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnQuantityOrder" runat="server" CommandArgument="OrderNumber"
                                    CommandName="Sort"><span >Ordered<br /></span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderQuantityOrder" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="txtQtyOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"QtyOrder") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="b_box centeralign" Width="3%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="RemainingOrderQuantity" Visible="false">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnQuantityOrder" runat="server" CommandArgument="OrderNumber"
                                    CommandName="Sort"><span >Remaining<br /></span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderQuantityOrder" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="txtQtyOrderRemaining" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RemaingQutOrder") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="g_box centeralign" Width="4%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="QtyShipped">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnQtyShipped" runat="server" CommandArgument="QtyShipped"
                                    CommandName="Sort"><span >Shipped<br /></span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderQtyShipped" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblQtyShip" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShipQuantity") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="g_box centeralign" Width="3%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Size">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span>Size</span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderSize" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSize" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="b_box centeralign" Width="8%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Color">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnColor" runat="server" CommandArgument="Color" CommandName="Sort"><span>Color</span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderColor" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="btn_space">
                                    <asp:Image ID="imgColor" runat="server" Height="20" Width="20" />
                                </span>
                                <asp:Label runat="server" ID="lblColor" Visible="false" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="g_box centeralign" Width="8%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Description">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span>Description</span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderDescription" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDescription" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="b_box" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="BackorderedUntil ">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnBackorderedUntil" runat="server" CommandArgument="BackorderedUntil "
                                    CommandName="Sort"><span >Backordered Until </span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderBackorderedUntil" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblBackOrderUntil" Text='<%# Eval("BackOrderUntil","{0:d}") %>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="g_box centeralign" Width="11%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="QtyReceived">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnQtyReceived" runat="server" CommandArgument="Received"
                                    CommandName="Sort"><span >Received<br /></span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderQtyReceived" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span>
                                    <asp:TextBox runat="server" Style="text-align:center;background-color: #303030; border: medium none;
                                        color: #ffffff; width: 50px; padding: 2px" onchange="CheckNum(this.id)" MaxLength="10"
                                        BackColor="#303030" ID="txtQtyReceived" Text='<%# DataBinder.Eval(Container.DataItem,"QuantityReceived") %>'></asp:TextBox>
                                </span>
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="b_box centeralign" Width="4%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Save</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="btn_space">
                                    <asp:ImageButton ID="lnkbtndelete" CommandName="SaveReceivedQty" CommandArgument='<%# Eval("ShippID") %>'
                                        runat="server" ImageUrl="~/Images/save-information-icon.png" Height="24" Width="24" /></span>
                            </ItemTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemStyle CssClass="g_box centeralign" Width="5%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <%--<br />
            <div class="botbtn centeralign">
                <asp:LinkButton ID="lnkSaveOrderDetails" class="grey2_btn" runat="server" OnClick="lnkSaveOrderDetails_Click"><span>Save</span></asp:LinkButton>
            </div>--%>
        </div>
    </div>
</asp:Content>
