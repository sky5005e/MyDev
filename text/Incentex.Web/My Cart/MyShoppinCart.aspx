<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MyShoppinCart.aspx.cs" Inherits="My_Cart_MyShoppinCart" Title="My Shopping Cart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            var foundCount = 0;
            for (i = 0; i < document.getElementsByTagName("input").length; i++) {
                if (document.getElementsByTagName("input")[i].type == "checkbox") {
                    if (document.getElementsByTagName("input")[i].checked == true) {
                        foundCount++;
                    }
                }
            }

            if (foundCount > 0) {
                if (confirm("Selected products will be removed from cart. Confirm Delete?") == true)
                    return true;
                else
                    return false;
            }
        }

        function DeleteSingleConfirmation(obj) {
            var a;
            if (obj.checked) {
                if (confirm("Selected products will be removed from cart. Confirm Delete?")) {
                    a = true;
                    obj.checked = true;
                }
                else {
                    a = false;
                    obj.checked = false;
                }
            }
            else {
                a = false;
                obj.checked = false;
            }
            return a;
        }

        function CheckNum(id) {
            var txt = document.getElementById(id);
            if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.value = "";
                txt.focus();

            }
        }

        function IsNumeric(sText) {
            var ValidChars = "0123456789";
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdateProgress runat="server" ID="uprogressPGrid" DisplayAfter="1">
        <ProgressTemplate>
            <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
            </div>
            <div class="updateProgressDiv">
                <img alt="Loading" src="../Images/ajax-loader-large.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upPanel" runat="server">
        <ContentTemplate>
            <div class="form_pad">
                <div>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div class="centeralign">
                                    <asp:Label ID="lblrptMsg" runat="server" CssClass="errormessage" Visible="false"></asp:Label>
                                </div>
                                <asp:Repeater ID="rptMyShoppingCart" runat="server" OnItemDataBound="rptMyShoppingCart_ItemDataBound"
                                    OnItemCommand="rptMyShoppingCart_ItemCommand">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0" runat="server" id="tblCartItem">
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
                                                            <asp:HiddenField ID="hdnAnniversary" Value='<%# Eval("sLookupName") %>' runat="server" />
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="width: 80%;">
                                                    <table class="shoppingcart_box" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td colspan="7" style="padding: 0px;">
                                                                <div class="cart_header" runat="server" id="dvCartItemHeader">
                                                                    <div class="left_co">
                                                                    </div>
                                                                    <div class="right_co">
                                                                    </div>
                                                                    <table cellpadding="0" cellspacing="0" class="txt13">
                                                                        <tr>
                                                                            <td style="width: 3%; padding-top: 6px;">
                                                                                <asp:ImageButton ID="imgDelete" runat="server" CommandArgument='<%# Eval("MyShoppingCartID") %>'
                                                                                    OnClientClick="return confirm('Are you sure you want to delete this product from your cart?');"
                                                                                    CommandName="deleteProdcutItem" ImageUrl="../Images/close.png" Height="16" Width="16"
                                                                                    ToolTip="Delete Cart Item" />
                                                                                <asp:HiddenField ID="hdnIsBulkOrder" runat="server" Value='<%# Eval("IsBulkOrder") %>' />
                                                                                <asp:HiddenField runat="server" ID="hdnMasterItemNo" Value='<%# Eval("MasterItemNo") %>' />
                                                                                <asp:HiddenField ID="hdnSubCategory" runat="server" Value='<%# Eval("SubCategoryName") %>' />
                                                                                <asp:HiddenField ID="hdnStoreProductID" runat="server" Value='<%# Eval("StoreProductID") %>' />
                                                                                <asp:HiddenField ID="hdnTailoringOption" Value='<%# Eval("TailoringOption") %>' runat="server" />
                                                                                <asp:HiddenField runat="server" ID="hdnItemNumber" Value='<%# Eval("item") %>' />
                                                                                <asp:HiddenField ID="hdnRunCharge" Value='<%# Eval("RunCharge") %>' runat="server" />
                                                                                <asp:HiddenField ID="hdnTailoringLength" Value='<%# Eval("TailoringLength") %>' runat="server" />
                                                                                <asp:HiddenField ID="hdnInventory" Value='<%# Eval("Inventory") %>' runat="server" />  
                                                                                <asp:HiddenField ID="hdnSize" Value='<%# Eval("Size") %>' runat="server"/>  
                                                                            </td>
                                                                            <td style="width: 30%;" class="leftalign">
                                                                                Product Description
                                                                            </td>
                                                                            <td style="width: 15%;" class="centeralign">
                                                                                Size
                                                                            </td>
                                                                            <td style="width: 12%;" runat="server" id="inventory" class="centeralign">
                                                                                Inventory
                                                                            </td>
                                                                            <td style="width: 12%;" class="centeralign">
                                                                                Quantity
                                                                            </td>
                                                                            <td style="width: 13%;" class="leftalign">
                                                                                Unit Price
                                                                            </td>
                                                                            <td style="width: 15%;" class="leftalign">
                                                                                Total
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7" class="spacer10">
                                                            </td>
                                                        </tr>
                                                        <tr class="txt14">
                                                            <td style="width: 33%;" class="leftalign" colspan="2">
                                                                <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.MyShoppingCartID") %>'
                                                                    Style="display: none;"></asp:Label>
                                                                <asp:Label runat="server" ID="lblProductDescription" Text='<%# Eval("ProductDescrption") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 15%;" class="centeralign">
                                                                <%# DataBinder.Eval(Container,"DataItem.Size") %>
                                                                <asp:HiddenField ID="hdnCreditmessage" Value='<%# Eval("CreditMessage") %>' runat="server" />
                                                            </td>
                                                            <td id="Td1" style="width: 12%;" runat="server" class="centeralign">
                                                                <asp:Label runat="server" ID="InventoryData" Text='<%# DataBinder.Eval(Container,"DataItem.Inventory") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdnInventoryLevelShow" Value='<%# Eval("ShowInventoryLevel") %>'
                                                                    runat="server" />
                                                            </td>
                                                            <td style="width: 12%;" class="centeralign">
                                                                <div class="btn_space centeralign">
                                                                    <asp:TextBox runat="server" CssClass="qty_box" ID="txtQuantity" onchange="CheckNum(this.id)"
                                                                        Text='<%# DataBinder.Eval(Container, "DataItem.Quantity")%>' MaxLength="10"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                            <td style="width: 13%;" class="leftalign">
                                                                <asp:Label runat="server" ID="lblUnitPrice" Text='<%# DataBinder.Eval(Container,"DataItem.UnitPrice") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 15%;" class="leftalign">
                                                                <asp:Label runat="server" ID="lblTotal"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7" class="spacer10">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7" style="padding-left: 0px;">
                                                                <table>
                                                                    <tr>
                                                                        <td class="leftalign">
                                                                            <asp:Panel ID="pnlNameBars" runat="server" Visible="false">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td colspan="7" style="padding: 0px;">
                                                                                            <div class="alignleft">
                                                                                                Name to be Engraved:
                                                                                                <asp:Label ID="lblNameTobeEngraved" runat="server"></asp:Label>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="7" style="padding: 0px;">
                                                                                            <div id="divEmpTitle" class="alignleft" runat="server">
                                                                                                <asp:Label ID="lblEmplTitleView" runat="server" Text="Employee Title:"></asp:Label>
                                                                                                <asp:Label ID="lblEmplTitle" runat="server"></asp:Label>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="7" class="spacer10">
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:Panel ID="pnlAnniversaryCE" runat="server" Visible="false">
                                                                                <div class="cart_header alignright">
                                                                                    <asp:Label ID="lblAnniversary" Width="200px" ForeColor="Red" runat="server"></asp:Label>
                                                                                </div>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7" class="spacer10">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:Panel runat="server" ID="pnlTailoring">
                                                        <table>
                                                            <tr class="txt14">
                                                                <td>
                                                                </td>
                                                                <td colspan="6">
                                                                    <div class="cart_header alignleft">
                                                                        <div class="left_co">
                                                                        </div>
                                                                        <div class="right_co">
                                                                        </div>
                                                                        <div class="tailoring_detail_pad">
                                                                            <img src="../Images/tailoring-icon.gif" alt="" /><span>Tailoring Details</span></div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr class="txt14">
                                                                <td>
                                                                </td>
                                                                <td colspan="6" class="leftalign">
                                                                    The tailor will tell your garment to the following length.
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="7">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td colspan="6" class="leftalign">
                                                                    <div class="qty_selbox alignleft">
                                                                        <div class="form_box">
                                                                        </div>
                                                                        <asp:TextBox runat="server" Width="25px" ID="TextBox1" Text=' <%# DataBinder.Eval(Container, "DataItem.TailoringLength")%>'></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="spacer10">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td colspan="6">
                                                                    <asp:Label ID="lblRunChargeView" runat="server" Text="Run Charge:" Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblRunCharge" Style="color: Red; font-weight: bolder" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <SeparatorTemplate>
                                        <br />
                                    </SeparatorTemplate>
                                    <FooterTemplate>
                                        <table class="shoppingcart_box" cellpadding="0" cellspacing="0">
                                            <tr class="total_count txt14">
                                                <td colspan="5" style="width: 80%;">
                                                </td>
                                                <td class="rightalign">
                                                    Total :&nbsp;
                                                    <asp:Label runat="server" ID="lblTotalProductPrice"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7" class="spacer10">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7" class="spacer10">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr class="total_count txt14">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="rightalign">
                                            <asp:LinkButton ID="lnkBtnApplyChanges" class="grey2_btn" runat="server" ToolTip="Apply Changes"
                                                OnClick="lnkBtnApplyChanges_Click"><span>Apply Changes</span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkBtnContinueShopping" class="grey2_btn" runat="server" ToolTip="Continue Shopping"
                                                OnClick="lnkBtnContinueShopping_Click"><span>Continue Shopping</span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkBtnCheckout" class="grey2_btn" runat="server" ToolTip="Checkout"
                                                OnClick="lnkBtnCheckout_Click"><span>Checkout</span></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
