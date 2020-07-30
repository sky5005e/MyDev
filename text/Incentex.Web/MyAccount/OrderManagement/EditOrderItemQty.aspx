<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EditOrderItemQty.aspx.cs" Inherits="MyAccount_OrderManagement_EditOrderItemQty"
    Title="Edit Order Item Quantity" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>

    <script type="text/javascript" language="javascript">
    //change textbox value on grid
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
        }
    //check for enter value is number
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
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="lblmsg" runat="server" CssClass="errormessage" Style="font-size: small;">
                                </asp:Label>
                            </td>
                        </tr>
                        <td>
                            <asp:GridView ID="gvMainOrderDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvMainOrderDetail_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Item #<br /></span>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                <asp:Label CssClass="first" runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="g_box" Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Ordered<br /></span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnSupplierId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"SupplierId") %>' />
                                            <asp:HiddenField ID="hdnMyShoppingCartiD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"MyShoppingCartiD") %>' />
                                            <asp:Label ID="txtQtyOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>New Qty<br /></span>
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
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span >Remaining<br /></span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label ID="txtQtyOrderRemaining" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField >
                                        <HeaderTemplate>
                                            <span >Qty Adjust<br /></span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="txtQtyAdjust" runat="server" Text="0"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span >Shipped<br /></span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="txtQtyShipped" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span >Color<br /></span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:Image ID="imgColor" runat="server" Height="20" Width="20" />
                                            </span>
                                            <asp:Label runat="server" ID="lblColor" Text='<%# DataBinder.Eval(Container.DataItem,"Color") %>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="b_box centeralign" Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span >Size</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSize" ToolTip='<%# DataBinder.Eval(Container.DataItem,"Size") %>'
                                                Text='<%#(Eval("Size").ToString().Length > 8) ? Eval("Size").ToString().Substring(0, 8) + "..." : Eval("Size")%>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Price</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrice" Text=' <%# Convert.ToDecimal(Eval("Price")).ToString("#,##0.00")%>' runat="server"></asp:Label>
                                            <asp:Label ID="lblMOASPrice" Text=' <%# Convert.ToDecimal(Eval("MOASItemPrice")).ToString("#,##0.00")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemStyle CssClass="g_box centeralign" Width="4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField >
                                        <HeaderTemplate>
                                            <span>Extended</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblExtendedPrice" Text='<%#  Convert.ToDecimal((Convert.ToDecimal(Eval("Price")) * Convert.ToDecimal(Eval("Quantity")))).ToString("#,##0.00")%>' runat="server"></asp:Label>
                                            <asp:Label ID="lblMOASExtendedPrice" Text='<%#  Convert.ToDecimal((Convert.ToDecimal(Eval("MOASItemPrice")) * Convert.ToDecimal(Eval("Quantity")))).ToString("#,##0.00")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="centeralign" />
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
