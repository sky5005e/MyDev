<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ReturnProductSubView.aspx.cs" Inherits="ProductReturnManagement_ReturnProductSubView" Title="Product Return" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
   <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>
<table>
<tr>
                    <td class="spacer10">
                    </td>
                </tr>
</table>
        <table class="dropdown_pad form_table">
                 <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel runat="server" ID="upnlCompany">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Return Order#: </span>
                                      <asp:Label ID="lblOrderNo" runat="server" CssClass="w_label"></asp:Label>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Type of Return:  </span>
                                      <asp:Label ID="lblRequest" runat="server" CssClass="w_label"></asp:Label>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
        <table class="form_table">
         <tr>
           <td>
           
            <asp:GridView ID="gvProductReturn" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" 
                RowStyle-CssClass="ord_content" onrowdatabound="gvProductReturn_RowDataBound" >
                <Columns>
                 <asp:TemplateField SortExpression="ItemNumber">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber" CommandName="Sort"> <span >Item #<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="12%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ReceivedQty">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnReceivedQty" runat="server" CommandArgument="ReceivedQty"
                                CommandName="Sort"><span>Received Qty<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderReceivedQty" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblReceivedQty" Text='<%# Eval("QuantityReceived") %>' />
                           <asp:HiddenField ID="hdnshippid" runat="server" Value='<%# Eval("ShippID") %>' />
                             <asp:HiddenField ID="hdnitemNo" runat="server" Value='<%# Eval("ItemNumber") %>' />
                            <asp:HiddenField ID="hdnShoppingCartId" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ReturnQty">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnReturnQty" runat="server" CommandArgument="ReturnQty" CommandName="Sort"><span >Return Qty<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderReturnQty" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%--<span>
                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                    color: #ffffff; width: 50px; padding: 2px" onchange="CheckNum(this.id)" MaxLength="10"
                                    BackColor="#303030" ID="txtReturnQty"></asp:TextBox>
                            </span>--%>
                              <asp:Label runat="server" ID="lblReturnQty" Text='<%# Eval("ReturnQty") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="9%" />
                    </asp:TemplateField>
                   
                    <asp:TemplateField SortExpression="Color">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnQtyReceived" runat="server" CommandArgument="Color" CommandName="Sort"> <span >Color<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderColor" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblColor" ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Size">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span >Size</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderSize" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                          <asp:Label ID="lblSize" runat="server"></asp:Label>
                          </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ProductDescrption">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnProductDescrption" runat="server" CommandArgument="Shipper"
                                CommandName="Sort"><span >Description</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderProductDescrption" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                          <asp:Label ID="lblProductionDescription"  runat="server"></asp:Label>
                          </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="PaymentOption">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnPaymentOption" runat="server" CommandArgument="PaymentOption"
                                CommandName="Sort"><span>Payment method</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderPaymentOption" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                         <ItemTemplate>
                          <asp:Label ID="lblPaymentOption"  runat="server" Text='<%# Eval("PaymentOption") %>' ></asp:Label>
                          </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="19%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
     
      </td>
                </tr>
                </table>
      
        <table class="dropdown_pad form_table" id="tblProductDescription" runat="server">
                  <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box taxt_area clearfix">
                                <span class="input_label alignleft" style="width: 39%!important;">Reason for Return:</span>
                                <div class="textarea_box alignright" style="width: 57%;">
                                    <div class="scrollbar">
                                        <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                            class="scrollbottom"></a>
                                    </div>
                                    <asp:TextBox ID="txtPrdDescription" ReadOnly="true" runat="server" TextMode="MultiLine" CssClass="scrollme1" Height="70px"></asp:TextBox>
                                    <div class="form_bot_co"><span>&nbsp;</span></div>
                                </div>
                                </div>
                             </div>
                    </td>
                </tr>
                <tr>                   
                 <td>
                        <div>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Shipper: </span>
                                      <asp:Label ID="lblShipper" runat="server" CssClass="w_label"></asp:Label>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Tracking Number: </span>
                                      <asp:Label ID="lblTrackingNumber" runat="server" CssClass="w_label"></asp:Label>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
</asp:Content>
