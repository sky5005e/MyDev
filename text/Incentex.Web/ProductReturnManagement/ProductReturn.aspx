<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProductReturn.aspx.cs" Inherits="ProductReturnManagement_ProductReturn"
    Title="Product Return" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <script type="text/javascript">
 function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }

        // check if value is numeric in textbox
        function CheckNum(id) {
            var txt = document.getElementById(id);
            if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
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
                            <asp:UpdatePanel ID="upnlOrderClosed" runat="server">
                            <Triggers>
                            <asp:PostBackTrigger ControlID="ddlOrderClosed" />
                            </Triggers>
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="custom-sel">
                                            <asp:DropDownList ID="ddlOrderClosed" AutoPostBack="True" runat="server" 
                                            onchange="pageLoad(this,value);" 
                                            onselectedindexchanged="ddlOrderClosed_SelectedIndexChanged">
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
        <table class="form_table">
         <tr>
                    <td>
            <div style="text-align: center; color: Red; font-size: larger;">
            </div>
             <div style="text-align: center; color: Red; font-size: larger;">
                <asp:Label ID="lblmsg" runat="server">
                </asp:Label>
            </div>
             <asp:Panel runat="server" ID="PnlgvOrderRetun" Visible="false">
            <asp:GridView ID="gvOrderReturn" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" 
                RowStyle-CssClass="ord_content" onrowdatabound="gvOrderReturn_RowDataBound" >
                <Columns>
                    <asp:TemplateField SortExpression="ReceivedQty">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnReceivedQty" runat="server" CommandArgument="ReceivedQty"
                                CommandName="Sort"><span>Received Qty<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderReceivedQty" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblReceivedQty" Text='<%# Eval("QuantityReceived") %>' />
                            <asp:HiddenField ID="hdnitemNo" runat="server" Value='<%# Eval("ItemNumber") %>' />
                            <asp:HiddenField ID="hdnShoppingCartId" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />
                            <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                            <asp:HiddenField ID="hdnshippid" runat="server" Value='<%# Eval("ShippID") %>' />
                            <asp:HiddenField ID="hdnTrackingNumber" runat="server" Value='<%# Eval("TrackingNo") %>' />
                            <asp:HiddenField ID="hdnPackageID" runat="server" Value='<%# Eval("PackageId") %>' />
                            <asp:HiddenField ID="hdnShipDate" runat="server" Value='<%# Eval("ShipingDate") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ReturnQty">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnReturnQty" runat="server" CommandArgument="ReturnQty" CommandName="Sort"><span >Return Qty<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderReturnQty" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span>
                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                    color: #ffffff; width: 50px; padding: 2px" onchange="CheckNum(this.id)" MaxLength="10"
                                    BackColor="#303030" ID="txtReturnQty"></asp:TextBox>
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Color">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnQtyReceived" runat="server" CommandArgument="Color" CommandName="Sort"> <span >Color<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderColor" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblColor" ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="13%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Size">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span >Size</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderSize" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                          <asp:Label ID="lblSize" runat="server"></asp:Label>
                          </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="15%" />
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
                        <ItemStyle CssClass="b_box" Width="40%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Requesting">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnRequesting" runat="server" CommandArgument="Requesting"
                                CommandName="Sort"><span>Requesting</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderRequesting" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                         <span>
                            <asp:DropDownList runat="server" ID="ddlRequesting" Style="background-color: #303030;
                                border: medium none; color: #ffffff; width: 100px; padding: 2px" runat="server">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="10%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </asp:Panel>
            <asp:Panel runat="server" ID="PnlgvReturnProductUpdate" Visible="false">
            <asp:GridView ID="gvReturnProductUpdate" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" 
                RowStyle-CssClass="ord_content" onrowdatabound="gvReturnProductUpdate_RowDataBound" >
                <Columns>
                    <asp:TemplateField SortExpression="ReceivedQty">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnReceivedQty" runat="server" CommandArgument="ReceivedQty"
                                CommandName="Sort"><span>Received Qty<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderReceivedQty" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblReceivedQty"/>
                            <asp:HiddenField ID="hdnProductReturnId" runat="server" Value='<%# Eval("ProductReturnId") %>' />
                              <asp:HiddenField ID="hdnRequesting" runat="server" Value='<%# Eval("Requesting") %>' />
                            <asp:HiddenField ID="hdnitemNo" runat="server" Value='<%# Eval("ItemNumber") %>' />
                            <asp:HiddenField ID="hdnShoppingCartId" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />
                            <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                            <asp:HiddenField ID="hdnshippid" runat="server" Value='<%# Eval("ShippID") %>' />
                            <asp:HiddenField ID="hdnTrackingNumber" runat="server" Value='<%# Eval("TrackingNumber") %>' />
                            <asp:HiddenField ID="hdnPackageID" runat="server" Value='<%# Eval("PackageId") %>' />
                            <asp:HiddenField ID="hdnShipDate" runat="server" />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="11%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ReturnQty">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnReturnQty" runat="server" CommandArgument="ReturnQty" CommandName="Sort"><span >Return Qty<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderReturnQty" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span>
                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                    color: #ffffff; width: 50px; padding: 2px" onchange="CheckNum(this.id)" MaxLength="10"
                                    BackColor="#303030" ID="txtReturnQty" Text='<%# Eval("ReturnQty") %>'></asp:TextBox>
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="8%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Color">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnQtyReceived" runat="server" CommandArgument="Color" CommandName="Sort"> <span >Color<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderColor" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblColor" ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="8%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Size">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span >Size</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderSize" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                          <asp:Label ID="lblSize" runat="server"></asp:Label>
                          </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="13%" />
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
                        <ItemStyle CssClass="b_box" Width="35%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Reason">
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
                    <asp:TemplateField SortExpression="Requesting">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnRequesting" runat="server" CommandArgument="Requesting"
                                CommandName="Sort"><span>Requesting</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderRequesting" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                         <span>
                            <asp:DropDownList runat="server" ID="ddlRequesting" Style="background-color: #303030;
                                border: medium none; color: #ffffff; width: 100px; padding: 2px" runat="server">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="10%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </asp:Panel>
      </td>
                </tr>
                </table>
        <asp:Panel runat="server" ID="pnlReturnStatus" Visible="false">
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
                                    <asp:TextBox ID="txtPrdDescription" runat="server" TextMode="MultiLine" CssClass="scrollme1" Height="70px"></asp:TextBox>
                                    <div class="form_bot_co"><span>&nbsp;</span></div>
                                </div>
                                </div>
                             </div>
                    </td>
                </tr>
            </table>
        <table class="dropdown_pad form_table">
         <tr>
                    <td>
                        <div class="botbtn centeralign">
                            <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save Information"
                                OnClick="lnkBtnSaveInfo_Click" ><span>Submit Information</span></asp:LinkButton>
                        </div>
                    </td>
                </tr>
                </table>
         </asp:Panel>
</asp:Content>
