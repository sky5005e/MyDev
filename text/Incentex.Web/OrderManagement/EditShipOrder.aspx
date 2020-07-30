<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="EditShipOrder.aspx.cs" Inherits="OrderManagement_EditShipOrder" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" language="javascript">
        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calendar-small.png',
                buttonImageOnly: true
            });
        });
         // check if value is numeric in textbox
        function CheckNum(id) {
            var txt = document.getElementById(id);
            var txt1 = txt.id.split('_');
            //ctl00_ContentPlaceHolder1_gvShippedOrderDetail_ctl02_txtQtyOrderRemaining
            //ctl00_ContentPlaceHolder1_gvShippedOrderDetail_ctl02_txtQtyShipped
            
            var fin = txt1[0]+"_"+txt1[1]+"_"+txt1[2]+"_"+txt1[3]+"_"+"txtQtyOrderRemaining";
            
            var rem = document.getElementById(fin);
            
//            if(parseInt(txt.value) > parseInt(rem.innerHTML))
//            {
//                alert("Please enter shipped quantity less then remaining quantity");
//                txt.focus();
//                return;
//            }
            if(txt.value == "0" || txt.value == "")
            {
                alert("You can not enter 0 as a ship order");
                
                txt.focus();
                return;
            }
            
            if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.focus();

            }

        }


        function IsNumeric(sText) {
            var ValidChars = "0123456789";
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
        
        function DeleteConfirmation() {
            if (confirm("Deleting ship order will also delete shipments too,if any..Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }
        
    </script>

    <div class="black_round_box">
        <div class="black2_round_top">
            <span></span>
        </div>
        <div class="black2_round_middle">
            <div class="form_pad">
                <div class="pro_search_pad" style="width: 920px;">
                    <mb:MenuUserControl ID="menucontrol" runat="server" />
                    <div class="black_middle order_detail_pad">
                        <div style="text-align: center; color: Red; font-size: larger;">
                            <asp:Label ID="lblmsg" runat="server">
                            </asp:Label>
                        </div>
                        <div>
                            <asp:GridView ID="gvShippedOrderDetail" runat="server" AutoGenerateColumns="false"
                                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                RowStyle-CssClass="ord_content" OnRowCommand="gvShippedOrderDetail_RowCommand"
                                OnRowDataBound="gvShippedOrderDetail_RowDataBound" OnRowEditing="gvShippedOrderDetail_RowEditing">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShipId" runat="server" Text='<%# Eval("ShippID") %>'></asp:Label>
                                            <asp:Label ID="lblOldShippedValue" runat="server" Text='<%# Eval("ShipQuantity") %>'></asp:Label>
                                            <asp:Label ID="lblOldBackOrderedDate" runat="server" Text='<%# Eval("BackOrderUntil","{0:d}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ShipQuantity" Visible="false">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnShipQuantity" runat="server" CommandArgument="ShipQuantity"
                                                CommandName="Sort"><span ></span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderShipQuantity" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdnQuantityShipped" Value='<%# Eval("ShipQuantity") %>' />
                                            <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                                            <asp:HiddenField ID="hdnShipID" runat="server" Value='<%# Eval("ShippID") %>' />
                                            <asp:HiddenField ID="hdnMyShoppingCartId" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />
                                            <asp:HiddenField ID="hdnSupplierid" runat="server" Value='<%# Eval("SupplierId") %>' />
                                            <asp:HiddenField ID="hdhQtyOrder" runat="server" Value='<%# Eval("QtyOrder") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box " Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ItemNumber">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                                CommandName="Sort"><span>Item #<br /></span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="QuantityOrder">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnQuantityOrder" runat="server" CommandArgument="OrderNumber"
                                                CommandName="Sort"><span >Ordered<br /></span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label ID="txtQtyOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"QtyOrder") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="RemainingOrderQuantity">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnRemainingOrder" runat="server" CommandArgument="OrderNumber"
                                                CommandName="Sort"><span >Remaining<br /></span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label ID="txtQtyOrderRemaining" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RemaingQutOrder") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="QtyShipped">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnQtyShipped" runat="server" CommandArgument="QtyShipped"
                                                CommandName="Sort"><span >Shipped<br /></span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                                    color: #ffffff; text-align: center; width: 50px; padding: 2px" onchange="CheckNum(this.id)"
                                                    MaxLength="10" BackColor="#303030" ID="txtQtyShipped" Text='<%# DataBinder.Eval(Container.DataItem,"ShipQuantity") %>'></asp:TextBox>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Color">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnColor" runat="server" CommandArgument="Color" CommandName="Sort"><span>Color</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:Image ID="imgColor" runat="server" Height="20" Width="20" />
                                            </span>
                                            <asp:Label runat="server" ID="lblColor" Visible="false" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Size">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span>Size</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSize" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Description">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnDescription" runat="server" CommandArgument="Size" CommandName="Sort"><span>Description</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblDescription" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="10%" />
                                    </asp:TemplateField>
                                     <asp:TemplateField SortExpression="Status">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkbtnStatus" runat="server" CommandArgument="Status"
                                                            CommandName="Sort"><span >Shipped Status</span></asp:LinkButton>
                                                        <asp:PlaceHolder ID="placeholderBackorderedUntil" runat="server"></asp:PlaceHolder>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                         <asp:Label ID="lblShippedStatus"  Text='<%# Eval("ShippingOrderStatus") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemStyle CssClass="g_box" Width="12%" />
                                                </asp:TemplateField>
                                                
                                    <asp:TemplateField SortExpression="BackorderedUntil ">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnBackorderedUntil" runat="server" CommandArgument="BackorderedUntil "
                                                CommandName="Sort"><span >Backordered Until </span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderBackorderedUntil" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <span class="calender_l">
                                                <asp:TextBox ID="txtBackOrderDate" Style="background-color: #303030; border: medium none;
                                                    color: #ffffff; width: 90px; padding: 2px" runat="server" Text='<%# Eval("BackOrderUntil","{0:d}") %>'
                                                    CssClass="cal_w datepicker min_w"></asp:TextBox>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="14%" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField SortExpression="QuantityReceived" Visible="false">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnQuantityReceived" runat="server" CommandArgument="QuantityReceived"><span>Received</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblQuantityReceived" Text='<%# (Convert.ToString(Eval("QuantityReceived")))%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                            <span>Save</span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:ImageButton ID="lnkbtnsave" CommandName="SaveShippingDetails" CommandArgument='<%# Eval("ShippID") %>'
                                                    Height="25" Width="25" runat="server" ImageUrl="~/Images/save-information-icon.png" /></span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Delete</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:ImageButton ID="lnkbtndelete" CommandName="deleteshiporder" CommandArgument='<%# Eval("ShippID") %>' OnClientClick="return DeleteConfirmation();"
                                                    runat="server" ImageUrl="~/Images/close.png" Height="24" Width="24" /></span>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <br />
                        <div>
                            <div class="botbtn centeralign">
                                <asp:LinkButton ID="lnkSaveOrderDetails" class="grey2_btn" runat="server" OnClick="lnkSaveOrderDetails_Click"><span>Save</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
