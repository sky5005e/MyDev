<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderItemEditQty.aspx.cs" Inherits="OrderManagement_OrderItemEditQty" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>

    <script type="text/javascript" language="javascript">
    function change(id)
    {
    
        
        var txt = document.getElementById(id);
        var txt1 = txt.id.split('_');
        var fin = txt1[0]+"_"+txt1[1]+"_"+txt1[2]+"_"+txt1[3]+"_"+txt1[4]+"_"+"txtQtyCurrent";
        var rem = document.getElementById(fin);
        
        var qtyOrder =  $("#ctl00_ContentPlaceHolder1_gvMainOrderDetail_ctl02_txtQtyOrder").text();
        var qtyCurrent = txt.value;
        var qtyShipped = $("#ctl00_ContentPlaceHolder1_gvMainOrderDetail_ctl02_txtQtyShipped").text();
        
         if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.focus();
                $("#ctl00_ContentPlaceHolder1_lnkSaveOrderDetails").attr("style", "visibility:hidden");
                return;
                
         }
        
        if(Number(qtyCurrent) < Number("-"+qtyOrder))
        {
            if(Number(qtyCurrent) < 0)
            {
                alert("You can not return quantity less then ordered!!");
                txt.focus();
                $("#ctl00_ContentPlaceHolder1_lnkSaveOrderDetails").attr("style", "visibility:hidden");
                return;
            }
            else
            {
                $("#ctl00_ContentPlaceHolder1_lnkSaveOrderDetails").attr("style", "visibility:visible");
            }
        }
        //else
        //{
            $("#ctl00_ContentPlaceHolder1_gvMainOrderDetail_ctl02_txtQtyOrderRemaining").text(txt.value);
            $("#ctl00_ContentPlaceHolder1_gvMainOrderDetail_ctl02_txtQtyAdjust").text(txt.value);
            //alert($("#ctl00_ContentPlaceHolder1_gvMainOrderDetail_ctl02_txtQtyAdjust").text());
            //Set Remaining
           
            //Adding value qtyOrdered
            var qtyRemaining = (Number(qtyOrder) + Number(qtyCurrent)) - Number(qtyShipped);
            $("#ctl00_ContentPlaceHolder1_gvMainOrderDetail_ctl02_txtQtyOrderRemaining").text(qtyRemaining);
            
            //
            var price = $("#ctl00_ContentPlaceHolder1_gvMainOrderDetail_ctl02_lblPrice").text();
            var extendedprice = ((Number(qtyOrder) + Number(qtyCurrent))) * Number(price);
            $("#ctl00_ContentPlaceHolder1_gvMainOrderDetail_ctl02_lblExtendedPrice").text(extendedprice.toFixed(2));
            $("#ctl00_ContentPlaceHolder1_lnkSaveOrderDetails").attr("style", "visibility:visible");
        
        //}
        
//        if(txt.value == "0" || txt.value == "")
//        {
//            alert("You can not enter 0 as a ship order");
//            txt.focus();
//            $("#ctl00_ContentPlaceHolder1_lnkSaveOrderDetails").attr("Enabled", "false");
//            return;
//        }
        
         
        
      

    }
    function IsNumeric(sText) {
            var ValidChars = "0123456789-1-2-3-4-5-6-7-8-9-10";
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
    </script>

    <div class="pro_search_pad" style="width: 920px;">
        <div class="black_middle order_detail_pad">
            <asp:UpdatePanel ID="upMain" runat="server">
                <ContentTemplate>
                    <table class="order_detail" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="lblmsg" runat="server" CssClass="errormessage" Style="font-size: small;">
                                </asp:Label>
                            </td>
                        </tr>
                        <td>
                            <asp:GridView ID="gvMainOrderDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvMainOrderDetail_RowDataBound"
                                >
                                <Columns>
                                    <asp:TemplateField SortExpression="ItemNumber" Visible="false">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnItemNumberLabel" runat="server" CommandArgument="ItemNumber"
                                                CommandName="Sort"><span>Item #<br /></span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderItemNumberLabel" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>'  Visible="false"/>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="g_box" Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ItemNumber">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                                CommandName="Sort"><span>Item #<br /></span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlItemNumbers" ToolTip="Changing item number will result in to change in price,size and color of item in database directly.." runat="server" OnSelectedIndexChanged="ddlItemNumbers_selectedindexchanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="g_box" Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="QuantityOrder">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnQuantityOrder" runat="server" CommandArgument="OrderNumber"
                                                CommandName="Sort"><span>Ordered<br /></span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnSupplierId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"SupplierId") %>' />
                                            <asp:HiddenField ID="hdnMyShoppingCartiD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"MyShoppingCartiD") %>' />
                                            <asp:Label ID="txtQtyOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="QtyCurrent">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnQtyCurrent" runat="server" CommandArgument="QtyShipped"
                                                CommandName="Sort"><span>QtyCurrent<br /></span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderQtyCurrent" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:TextBox ID="txtQtyCurrent" o Style="background-color: #303030; border: medium none;
                                                    color: #ffffff; width: 50px; padding: 2px" BackColor="#303030" runat="server"
                                                    onchange="javascript:return change(this.id);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="RemainingOrderQuantity">
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
                                    <asp:TemplateField SortExpression="QtyAdjust">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnQtyAdjust" runat="server" CommandArgument="QtyShipped"
                                                CommandName="Sort"><span >Qty Adjust<br /></span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderQtyAdjust" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="txtQtyAdjust" runat="server" Text="0"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
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
                                                <asp:HiddenField ID="hdnColor" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Color") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="b_box centeralign" Width="6%" />
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
                                                <asp:HiddenField ID="hdnSize" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Size") %>' />
                                                <asp:HiddenField ID="hdnProductItemID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"ProductItemID") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ProductDescrption" Visible="false">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnProductDescrption" runat="server" CommandArgument="Shipper"
                                                CommandName="Sort"><span>Description</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderProductDescrption" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblProductionDescription" Text='<%#(Eval("ProductDescrption").ToString().Length > 40) ? Eval("ProductDescrption").ToString().Substring(0,40)+"..." : Eval("ProductDescrption")  %>'
                                                            ToolTip='<% #Eval("ProductDescrption")  %>' runat="server"></asp:Label>--%>
                                            <asp:Label ID="lblProductionDescription" Text='<% #Eval("ProductDescrption")  %>'
                                                ToolTip='<% #Eval("ProductDescrption")  %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="b_box" Width="10%" Wrap="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Price">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnPrice" runat="server" CommandArgument="Price" CommandName="Sort"><span>Price</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderPrice" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnPaymentOption" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"PaymentOption") %>' />
                                            <asp:Label ID="lblPrice" Text=' <%# Convert.ToDecimal(Eval("Price")).ToString("#,##0.00")%>'
                                                runat="server"></asp:Label>
                                            <asp:Label ID="lblMOASPrice" Text=' <%# Convert.ToDecimal(Eval("MOASItemPrice")).ToString("#,##0.00")%>' runat="server"></asp:Label>
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
                                            <asp:Label ID="lblExtendedPrice" Text='<%#  Convert.ToDecimal((Convert.ToDecimal(Eval("Price")) * Convert.ToDecimal(Eval("Quantity")))).ToString("#,##0.00")%>'
                                                runat="server"></asp:Label>
                                            <asp:Label ID="lblMOASExtendedPrice" Text='<%#  Convert.ToDecimal((Convert.ToDecimal(Eval("MOASItemPrice")) * Convert.ToDecimal(Eval("Quantity")))).ToString("#,##0.00")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Delete" Visible="false">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnDeleteShip" runat="server" CommandArgument="Delete" CommandName="Sort"><span>Delete</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderDeleteShip" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="hypDeleteShippedItem" CommandName="DeleteOrderItem" CommandArgument='<%#Eval("MyShoppingCartiD")%>'
                                                OnClientClick="return confirm('Are you sure, you want to delete order item ?')"
                                                runat="server"><span>Delete</span></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="botbtn centeralign">
                                    <asp:LinkButton ID="lnkSaveOrderDetails" class="grey2_btn" runat="server" OnClick="lnkSaveOrderDetails_Click"><span>Save</span></asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
