<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="Checkout.aspx.cs" Inherits="UserPages_Checkout" Title="incentex | Checkout" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aCTK" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
            $().ready(function() {
                $(window).ValidationUI();
                
                $("#<%= chkAddShipping.ClientID %>").on('ifChanged', function(event){
                    toggleShippingAddressNameValidation();
                });
                
                $("#<%= chkAddBilling.ClientID %>").on('ifChanged', function(event){
                    toggleBillingAddressNameValidation();
                });

                $(".drppostback").change(function() {
                    $(".progress-layer").show();
                });
                
                $(".postback").click(function() {
                    $(".progress-layer").show();
                });
                
                $(".saveshipping").click(function() {
                    if (Page_ClientValidate("SaveShipping"))
                        $(".progress-layer").show();
                });
                
                $(".savebilling").click(function() {
                    if (Page_ClientValidate("SaveBilling"))
                        $(".progress-layer").show();
                });
                
                $(".savepayment").click(function() {
                    if (Page_ClientValidate("SavePayment"))
                        $(".progress-layer").show();
                });
                
                $("#<%= lbProcessOrder.ClientID %>").click(function() {                    
                    if ($('#<%= hdnItemCount.ClientID %>').val() == "0") {
                        GeneralAlertMsg('There are no items in your cart.');
                        return false;
                    }
                    
                    toggleAddressNameValidations();
                    var isValid = false;
                    isValid = Page_ClientValidate("SaveShipping");
                    
                    if (isValid) {
                        isValid = Page_ClientValidate("SaveBilling");
                    }
                    
                    if (isValid) {
                        isValid = Page_ClientValidate("SavePayment");
                    }
                    
                    if (isValid) {
                        $(".progress-layer").show();
                    }
                    
                    return isValid;
                });                
                
                $("#hlPrintOrderConfirmation").click(function(){
				    // Print the DIV.
				    $("#<%= dvPrintOrderConfirmationContent.ClientID %>").show();
				    $("#<%= dvPrintOrderConfirmationContent.ClientID %>").printElement();
				    $("#<%= dvPrintOrderConfirmationContent.ClientID %>").hide();
				    // Cancel click event.
				    return( false );
			    });
			    
			    function toggleShippingAddressNameValidation() {
			        if ($("#<%= chkAddShipping.ClientID %>").is(":checked")) {
                        ValidatorEnable(document.getElementById('<%= rfvSAddressName.ClientID %>'), true);                        
                    }
                    else {
                        ValidatorEnable(document.getElementById('<%= rfvSAddressName.ClientID %>'), false);
                        $("#<%= txtSAddressName.ClientID %>").removeClass("ErrorField");
                    }
                }
                
                function toggleBillingAddressNameValidation() {    
                    if ($("#<%= chkAddBilling.ClientID %>").is(":checked")) {
                        ValidatorEnable(document.getElementById('<%= rfvBAddressName.ClientID %>'), true);                        
                    }
                    else {
                        ValidatorEnable(document.getElementById('<%= rfvBAddressName.ClientID %>'), false);
                        $("#<%= txtBAddressName.ClientID %>").removeClass("ErrorField");
                    }
			    }
			    
			    function toggleAddressNameValidations() {
			        toggleShippingAddressNameValidation();
                    toggleBillingAddressNameValidation();
			    }
            });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <aCTK:ToolkitScriptManager ID="tksmCheckout" runat="server">
    </aCTK:ToolkitScriptManager>
    <input type="hidden" value="checkout-link" id="hdnActiveLink" />
    <asp:HiddenField ID="hfMOASApproverLevelID" runat="server" />
    <asp:HiddenField ID="hdnItemCount" runat="server" />
    <% if (!Request.IsLocal)
       { %>
    <section id="container" class="cf">
    <% }
       else
       { %>
    <div id="container" class="cf">
        <%} %>
        <div class="narrowcolumn alignleft">
            <div class="lefthelp-video cf">
                <a class="lefthelp-videobtn" title="Help video" href="javascript: void(0);" onclick="GetHelpVideo('Checkout','Checkout')">Help video</a>
            </div>
            <div class="leftcart-block cf">
                <h2 class="title-txt">
                    <span class="cur-lbl">USD</span>Shopping Cart</h2>
                <ul class="cart-listing cf">
                    <asp:Repeater ID="repCartItems" runat="server" OnItemDataBound="repCartItems_ItemDataBound"
                        OnItemCommand="repCartItems_ItemCommand">
                        <ItemTemplate>
                            <li>
                                <div class="image-block">
                                    <asp:Image ID="imgProductImage" runat="server" Width="90" Height="120" AlternateText="cart" /></div>
                                <div class="cart-desc">
                                    <div class="product-name">
                                        <asp:Label ID="lblProductName" runat="server"></asp:Label>
                                        <asp:Label ID="lblProductPrice" runat="server" CssClass="pro-price"></asp:Label>
                                    </div>
                                    <asp:Label ID="lblProductSize" runat="server" CssClass="pro-size"></asp:Label>
                                    <asp:Label ID="lblProductQty" runat="server" CssClass="pro-qty"></asp:Label>
                                    <asp:LinkButton ID="lbRemoveItem" runat="server" CssClass="remove-btn postback" CommandName="remove"
                                        CommandArgument='<%# Eval("MyShoppingCartID") %>'>remove</asp:LinkButton>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div id="pagingtable" runat="server" class="pagination cf" visible="false">
                    <asp:LinkButton ID="lnkPrevious" CssClass="left-arrow alignleft" runat="server" OnClick="lnkPrevious_Click"
                        ToolTip="Invoice">                        
                    </asp:LinkButton>
                    <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                        RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPaging" runat="server" CommandArgument='<%# Eval("Index") %>'
                                CommandName="ChangePage" Text='<%# Eval("Index") %>'>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:DataList>
                    <asp:LinkButton ID="lnkNext" CssClass="right-arrow alignright" runat="server" OnClick="lnkNext_Click"
                        ToolTip="Invoice">                        
                    </asp:LinkButton>
                </div>
                <ul class="cart-total-block cf">
                    <li><span class="cart-detail">Subtotal</span>
                        <asp:Label ID="lblSubTotal" runat="server" CssClass="cart-price"></asp:Label>
                    </li>
                    <%--<li><span class="cart-detail">Promo Code Discount</span>
                        <asp:Label ID="lblPromoDiscount" runat="server" CssClass="cart-price"></asp:Label>
                    </li>--%>
                    <li><span class="cart-detail">Tax</span>
                        <asp:Label ID="lblTax" runat="server" CssClass="cart-price"></asp:Label>
                    </li>
                    <li class="odd"><span class="cart-detail">Shipping Charges</span>
                        <asp:Label ID="lblShippingAmount" runat="server" CssClass="cart-price"></asp:Label>
                    </li>
                    <li class="grand-total"><span class="cart-detail">Grand Total</span>
                        <asp:Label ID="lblGrandTotal" runat="server" CssClass="cart-price"></asp:Label>
                    </li>
                    <%--<li class="promo-data">
                        <asp:LinkButton ID="lbPromoApply" runat="server" OnClick="lbPromoApply_Click" CssClass="small-btn apply-btn postback">APPLY</asp:LinkButton>
                        <span class="promo-code">Promo Code</span>
                        <asp:TextBox ID="txtPromoCode" runat="server" CssClass="input-field-small" placeholder="12345">
                        </asp:TextBox>
                    </li>--%>
                </ul>
            </div>
        </div>
        <div class="widecolumn alignright">
            <div class="account-form-block">
                <div class="account-formInner">
                    <h2 class="title-txt">
                        1. Shipping Address</h2>
                    <div class="basic-form">
                        <div class="cf">
                            <ul class="left-form cf">
                                <li class="alignleft border-data">
                                    <label>
                                        <span class="lbl-txt1">Address Name</span>
                                    </label>
                                    <span class="select-drop country-drop">
                                        <asp:DropDownList ID="ddlShippingAddressName" runat="server" CssClass="default drppostback"
                                            OnSelectedIndexChanged="ddlShippingAddressName_SelectedIndexChanged" AutoPostBack="true"
                                            TabIndex="1">
                                        </asp:DropDownList>
                                    </span></li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">First Name*</span>
                                        <asp:TextBox ID="txtSFirstName" runat="server" TabIndex="2" CssClass="input-field-all first-field checkvalidation"
                                            MaxLength="100">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvSFirstName" runat="server" ControlToValidate="txtSFirstName"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveShipping"
                                            ErrorMessage="Please enter shipping first name.">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt">Last Name*</span>
                                        <asp:TextBox ID="txtSLastName" runat="server" TabIndex="3" CssClass="input-field-all first-field checkvalidation"
                                            MaxLength="100">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvSLastName" runat="server" ControlToValidate="txtSLastName"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveShipping"
                                            ErrorMessage="Please enter shipping last name."></asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Company</span>
                                        <asp:TextBox ID="txtSCompany" runat="server" TabIndex="4" CssClass="input-field-all full-width"
                                            MaxLength="100">
                                        </asp:TextBox>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Address*</span>
                                        <asp:TextBox ID="txtSAddress1" runat="server" TabIndex="5" CssClass="default_title_text input-field-all full-width checkvalidation"
                                            MaxLength="35">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvSAddress1" runat="server" ControlToValidate="txtSAddress1"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveShipping"
                                            ErrorMessage="Please enter shipping address.">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Suite/Apt #</span>
                                        <asp:TextBox ID="txtSAddress2" runat="server" TabIndex="6" CssClass="default_title_text input-field-all"
                                            MaxLength="35">
                                        </asp:TextBox>
                                    </label>
                                </li>
                                <li class="alignright">
                                    <div class="cf">
                                        <label>
                                            <span class="lbl-txt">City*</span>
                                            <asp:TextBox ID="txtSCity" runat="server" TabIndex="7" CssClass="default_title_text input-field-all checkvalidation"
                                                MaxLength="100">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvSCity" runat="server" ControlToValidate="txtSCity"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveShipping"
                                                ErrorMessage="Please enter shipping city." InitialValue="Please first select shipping state.">
                                            </asp:RequiredFieldValidator>
                                            <aCTK:AutoCompleteExtender runat="server" ID="aceSCity" TargetControlID="txtSCity"
                                                ServiceMethod="FillSCity" MinimumPrefixLength="1" CompletionInterval="0" EnableCaching="true"
                                                CompletionSetCount="12" ContextKey="0" />
                                        </label>
                                    </div>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">State*</span><span class="select-drop state-drop cmn-drop drppostback cf">
                                            <asp:DropDownList ID="ddlSState" runat="server" CssClass="default checkvalidation"
                                                OnSelectedIndexChanged="ddlSState_SelectedIndexChanged" AutoPostBack="true" TabIndex="8">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvSState" runat="server" ControlToValidate="ddlSState"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveShipping"
                                                ErrorMessage="Please select shipping state." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span>
                                    </label>
                                </li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt">Zip Code*</span>
                                        <asp:TextBox ID="txtSZipCode" runat="server" TabIndex="9" CssClass="default_title_text input-field-all checkvalidation"
                                            MaxLength="50">
                                        </asp:TextBox>
                                        <%--<asp:Button ID="btnCallStrikeIron" runat="server" Text="Call Strike Iron" OnClick="btnCallStrikeIron_Click"
                                                Style="display: none;" />--%>
                                        <asp:RequiredFieldValidator ID="rfvSZipCode" runat="server" ControlToValidate="txtSZipCode"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveShipping"
                                            ErrorMessage="Please enter shipping zip code."></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revSZipCode" runat="server" ControlToValidate="txtSZipCode"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveShipping"
                                            ErrorMessage="Please enter valid shipping zip code.">
                                        </asp:RegularExpressionValidator>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Country*</span></label>
                                    <span class="select-drop half-drop cmn-drop">
                                        <asp:DropDownList ID="ddlSCountry" runat="server" CssClass="default checkvalidation drppostback"
                                            TabIndex="10">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvSCountry" runat="server" ControlToValidate="ddlSCountry"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveShipping"
                                            ErrorMessage="Please select shipping country." InitialValue="0"></asp:RequiredFieldValidator>
                                    </span></li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt1">Shipping Method</span></label><span class="select-drop half-drop cmn-drop">
                                            <asp:DropDownList ID="ddlShippingMethod" runat="server" CssClass="default"
                                                TabIndex="11">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="rfvShippingMethod" runat="server" ControlToValidate="ddlShippingMethod"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveShipping"
                                                ErrorMessage="Please select shipping method." InitialValue="0"></asp:RequiredFieldValidator>--%>
                                        </span></li>
                            </ul>
                            <div class="check-block">
                                <label class="label_checkbox" for="add_my_address">
                                    <asp:CheckBox class="icheckbox_flat" ID="chkAddShipping" runat="server" Checked="true" TabIndex="12" /> Add this to My Address Book.
                                </label>
                                <asp:TextBox ID="txtSAddressName" runat="server" TabIndex="13" CssClass="default_title_text input-field-small checkvalidation"
                                    MaxLength="50">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvSAddressName" runat="server" ControlToValidate="txtSAddressName"
                                    Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveShipping"
                                    ErrorMessage="Please enter shipping address name.">
                                </asp:RequiredFieldValidator>
                                <asp:LinkButton ID="lbSaveShippingAddress" runat="server" OnClick="lbSaveShippingAddress_Click"
                                    CssClass="lbl-btn submit saveshipping" ValidationGroup="SaveShipping" call="SaveShipping"
                                    TabIndex="14">SAVE</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="account-formInner">
                    <h2 class="title-txt">
                        2. Billing Address</h2>
                    <div class="basic-form">
                        <div class="cf">
                            <ul class="left-form cf">
                                <li class="alignleft border-data">
                                    <label>
                                        <span class="lbl-txt1">Address Name</span></label>
                                    <span class="select-drop country-drop">
                                        <asp:DropDownList ID="ddlBillingAddressName" runat="server" CssClass="default drppostback"
                                            OnSelectedIndexChanged="ddlBillingAddressName_SelectedIndexChanged" AutoPostBack="true"
                                            TabIndex="15">
                                        </asp:DropDownList>
                                    </span></li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">First Name*</span>
                                        <asp:TextBox ID="txtBFirstName" runat="server" TabIndex="16" CssClass="input-field-all first-field checkvalidation"
                                            MaxLength="100">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvBFirstName" runat="server" ControlToValidate="txtBFirstName"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBilling"
                                            ErrorMessage="Please enter billing first name.">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt">Last Name*</span>
                                        <asp:TextBox ID="txtBLastName" runat="server" TabIndex="17" CssClass="input-field-all first-field checkvalidation"
                                            MaxLength="100">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvBLastName" runat="server" ControlToValidate="txtBLastName"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBilling"
                                            ErrorMessage="Please enter billing last name."></asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Company</span>
                                        <asp:TextBox ID="txtBCompany" runat="server" TabIndex="18" CssClass="input-field-all full-width"
                                            MaxLength="100">
                                        </asp:TextBox>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Address*</span>
                                        <asp:TextBox ID="txtBAddress1" runat="server" TabIndex="19" CssClass="default_title_text input-field-all full-width checkvalidation"
                                            MaxLength="250">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvBAddress1" runat="server" ControlToValidate="txtBAddress1"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBilling"
                                            ErrorMessage="Please enter billing address.">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Suite/Apt #</span>
                                        <asp:TextBox ID="txtBAddress2" runat="server" TabIndex="20" CssClass="default_title_text input-field-all full-width"
                                            MaxLength="250">
                                        </asp:TextBox>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <div class="cf">
                                        <label>
                                            <span class="lbl-txt">City*</span>
                                            <asp:TextBox ID="txtBCity" runat="server" TabIndex="21" CssClass="default_title_text input-field-all checkvalidation"
                                                MaxLength="100">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBCity" runat="server" ControlToValidate="txtBCity"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBilling"
                                                ErrorMessage="Please enter billing city." InitialValue="Please first select billing state.">
                                            </asp:RequiredFieldValidator>
                                            <aCTK:AutoCompleteExtender runat="server" ID="aceBCity" TargetControlID="txtBCity"
                                                ServiceMethod="FillBCity" MinimumPrefixLength="1" CompletionInterval="0" EnableCaching="true"
                                                CompletionSetCount="12" ContextKey="0" />
                                        </label>
                                    </div>
                                </li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt">State*</span> <span class="select-drop state-drop cmn-drop drppostback">
                                            <asp:DropDownList ID="ddlBState" runat="server" CssClass="default checkvalidation"
                                                OnSelectedIndexChanged="ddlBState_SelectedIndexChanged" AutoPostBack="true" TabIndex="22">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBState" runat="server" ControlToValidate="ddlBState"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBilling"
                                                ErrorMessage="Please select billing state." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span>
                                    </label>
                                </li>
                                <li class="alignleft">
                                    <label>
                                        <span class="lbl-txt">Zip Code*</span>
                                        <asp:TextBox ID="txtBZipCode" runat="server" TabIndex="23" CssClass="default_title_text input-field-all checkvalidation"
                                            MaxLength="50">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvBZipCode" runat="server" ControlToValidate="txtBZipCode"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBilling"
                                            ErrorMessage="Please enter billing zipcode.">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revBZipCode" runat="server" ControlToValidate="txtBZipCode"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBilling"
                                            ErrorMessage="Please enter valid billing zipcode.">
                                        </asp:RegularExpressionValidator>
                                    </label>
                                </li>
                                <li class="alignright">
                                    <label>
                                        <span class="lbl-txt">Country*</span>
                                    </label>
                                    <span class="select-drop half-drop cmn-drop">
                                        <asp:DropDownList ID="ddlBCountry" runat="server" CssClass="default checkvalidation drppostback"
                                            OnSelectedIndexChanged="ddlBCountry_SelectedIndexChanged" AutoPostBack="true"
                                            TabIndex="24">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvBCountry" runat="server" ControlToValidate="ddlBCountry"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBilling"
                                            ErrorMessage="Please select billing country." InitialValue="0">
                                        </asp:RequiredFieldValidator>
                                    </span></li>
                            </ul>
                            <div class="check-block">
                                <label class="label_checkbox" for="add_my_address">
                                    <asp:CheckBox class="icheckbox_flat" ID="chkAddBilling" runat="server" Checked="true" TabIndex="25" />
                                    Add this to My Address Book.
                                </label>
                                <asp:TextBox ID="txtBAddressName" runat="server" TabIndex="26" CssClass="default_title_text input-field-small checkvalidation"
                                    MaxLength="50">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvBAddressName" runat="server" ControlToValidate="txtBAddressName"
                                    Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBilling"
                                    ErrorMessage="Please enter billing address name.">
                                </asp:RequiredFieldValidator>
                                <asp:LinkButton ID="lbSaveBillingAddress" runat="server" OnClick="lbSaveBillingAddress_Click"
                                    CssClass="lbl-btn submit savebilling" ValidationGroup="SaveBilling" call="SaveBilling"
                                    TabIndex="27">SAVE</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="account-formInner">
                    <h2 class="title-txt">
                        3. Payment Details</h2>
                    <div class="basic-form">
                        <div class="cf">
                            <ul class="left-form cf">
                                <li class="alignleft simple-data" id="liAnniversaryCredits" runat="server" visible="false">
                                    <label>
                                        <span class="lbl-txt1" id="spanAnniversaryCredits" runat="server">Payment Method</span>
                                    </label>
                                    <span class="select-drop country-drop">
                                        <asp:DropDownList ID="ddlAnniversaryCredits" runat="server" CssClass="default checkvalidation drppostback"
                                            OnSelectedIndexChanged="ddlAnniversaryCredits_SelectedIndexChanged" AutoPostBack="true"
                                            TabIndex="28">
                                        </asp:DropDownList>
                                    </span>
                                    <div class="dis-txt" id="dvRemainingBalance" runat="server" visible="false">
                                        Remaining balance is <em>$209.00</em> for this purchase. Select a secondary payment
                                        option.
                                    </div>
                                </li>
                                <li class="alignleft" id="liPaymentOption" runat="server" visible="true">
                                    <label>
                                        <span class="lbl-txt1" id="spanPaymentMethod" runat="server">Payment Method*</span>
                                    </label>
                                    <span class="select-drop country-drop">
                                        <asp:DropDownList ID="ddlPaymentOption" runat="server" CssClass="default checkvalidation drppostback"
                                            OnSelectedIndexChanged="ddlPaymentOption_SelectedIndexChanged" AutoPostBack="true"
                                            TabIndex="29">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvPaymentOption" runat="server" ControlToValidate="ddlPaymentOption"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavePayment"
                                            ErrorMessage="Please select payment method." InitialValue="0">
                                        </asp:RequiredFieldValidator>
                                    </span></li>
                                <li class="alignleft" id="liPaymentOptionCode" runat="server" visible="false">
                                    <label>
                                        <span class="lbl-txt">Code*</span>
                                        <asp:TextBox ID="txtPaymentOptionCode" runat="server" TabIndex="30" CssClass="input-field-all first-field full-width checkvalidation">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPaymentOptionCode" runat="server" ControlToValidate="txtPaymentOptionCode"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavePayment"
                                            ErrorMessage="Please enter value.">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignleft" id="liCreditCardType" runat="server" visible="false">
                                    <label>
                                        <span class="lbl-txt1">Credit Card Type*</span>
                                    </label>
                                    <span class="select-drop country-drop">
                                        <asp:DropDownList ID="ddlCreditCardType" runat="server" CssClass="default checkvalidation drppostback"
                                            OnSelectedIndexChanged="ddlCreditCardType_SelectedIndexChanged" AutoPostBack="true"
                                            TabIndex="31">
                                            <asp:ListItem Value="1" Text="Master Card"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Visa"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Discover"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="American Express"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCreditCardType" runat="server" ControlToValidate="ddlCreditCardType"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavePayment"
                                            ErrorMessage="Please select credit card type." InitialValue="0">
                                        </asp:RequiredFieldValidator>
                                    </span></li>
                                <li class="alignleft" id="liCardHolderName" runat="server" visible="false">
                                    <label>
                                        <span class="lbl-txt1">Card Holder Name*</span>
                                        <asp:TextBox ID="txtCardHolderName" runat="server" TabIndex="32" MaxLength="50" CssClass="input-field-all first-field full-width checkvalidation">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCardHolderName" runat="server" ControlToValidate="txtCardHolderName"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavePayment"
                                            ErrorMessage="Please enter card holder name.">
                                        </asp:RequiredFieldValidator>
                                    </label>
                                </li>
                                <li class="alignleft" id="liCardNumber" runat="server" visible="false">
                                    <label>
                                        <span class="lbl-txt1">Credit Card Number*</span>
                                        <asp:TextBox ID="txtCardNumber" runat="server" TabIndex="33" MaxLength="30" CssClass="input-field-all first-field full-width checkvalidation">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCardNumber" runat="server" ControlToValidate="txtCardNumber"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavePayment"
                                            ErrorMessage="Please enter card number.">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revCardNumber" runat="server" ControlToValidate="txtCardNumber"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavePayment"
                                            ErrorMessage="Please enter valid master card number." ValidationExpression="^5[1-5][0-9]{14}$">
                                        </asp:RegularExpressionValidator>
                                    </label>
                                </li>
                                <li class="alignleft" id="liCardDetails1" runat="server" visible="false">
                                    <div class="cf">
                                        <label>
                                            <span class="lbl-txt">Expires*</span>
                                        </label>
                                        <span class="select-drop state-drop exp-drop">
                                            <asp:DropDownList ID="ddlCreditCardExpirationMonth" runat="server" CssClass="default checkvalidation"
                                                TabIndex="34">
                                                <asp:ListItem Value="1" Text="01 - January"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="02 - February"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="03 - March"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="04 - April"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="05 - May"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="06 - June"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="07 - July"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="08 - August"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="09 - September"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="10 - October"></asp:ListItem>
                                                <asp:ListItem Value="11" Text="11 - November"></asp:ListItem>
                                                <asp:ListItem Value="12" Text="12 - December"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCreditCardExpires" runat="server" ControlToValidate="ddlCreditCardExpirationMonth"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavePayment"
                                                ErrorMessage="Please select credit card expiration month." InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span>
                                    </div>
                                </li>
                                <li class="alignleft" id="liCardDetails2" runat="server" visible="false">
                                    <asp:TextBox ID="txtCardExpirationYear" runat="server" TabIndex="35" MaxLength="4"
                                        CssClass="input-field-all zipcode-width checkvalidation">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCardExpirationYear" runat="server" ControlToValidate="txtCardExpirationYear"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavePayment"
                                        ErrorMessage="Please enter credit card expiration year.">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revCardExpirationYear" runat="server" ControlToValidate="txtCardExpirationYear"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavePayment"
                                        ErrorMessage="Please enter valid credit card expiration year." ValidationExpression="^20[0-9]{2}$">
                                    </asp:RegularExpressionValidator>
                                    <aCTK:AutoCompleteExtender runat="server" ID="aceCardExpirationYear" TargetControlID="txtCardExpirationYear"
                                        ServiceMethod="FillCreditCardExpirationYear" MinimumPrefixLength="1" CompletionInterval="0"
                                        EnableCaching="true" CompletionSetCount="12" />
                                </li>
                                <li class="alignleft" id="liCardDetails3" runat="server" visible="false">
                                    <label>
                                        <span class="lbl-txt lbl-txt-code">Security Code*</span>
                                        <asp:TextBox ID="txtSecurityCode" runat="server" TabIndex="36" MaxLength="3" CssClass="input-field-all zipcode-width checkvalidation">
                                        </asp:TextBox>
                                        <a href="http://www.cvvnumber.com/cvv.html" target="_blank" class="lbl-btn lbl-midbtn" title="What is my security code?">
                                            ?</a>
                                        <asp:RequiredFieldValidator ID="rfvSecurityCode" runat="server" ControlToValidate="txtSecurityCode"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavePayment"
                                            ErrorMessage="Please enter credit card cvv code.">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revSecurityCode" runat="server" ControlToValidate="txtSecurityCode"
                                            Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavePayment"
                                            ErrorMessage="Please enter valid security code." ValidationExpression="^[0-9]{3,4}$">
                                        </asp:RegularExpressionValidator>
                                    </label>
                                </li>
                            </ul>
                            <div class="bottom-block">
                                <span class="txt-display">Would you like to name this order?*</span>
                                <asp:TextBox ID="txtReferenceName" runat="server" CssClass="default_title_text input-field-small checkvalidation"
                                    TabIndex="37">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvReferenceName" runat="server" ControlToValidate="txtReferenceName"
                                    Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SavePayment"
                                    ErrorMessage="Please enter order name.">
                                </asp:RequiredFieldValidator>
                                <asp:LinkButton ID="lbSavePayment" runat="server" OnClick="lbSavePayment_Click" CssClass="lbl-btn submit savepayment"
                                    ValidationGroup="SavePayment" call="SavePayment" TabIndex="38">SAVE</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="cart-bottom-block">
                    <div class="cart-txt-block">
                        By clicking the red button below, you are accepting this site's purchasing conditions.
                        <br>
                        Please verify your orders and correct and complete by reviewing your Shopping Cart
                        Summary on the left of this Checkout page.</div>
                    <asp:LinkButton ID="lbProcessOrder" runat="server" CssClass="blue-btn submit" TabIndex="39"
                        OnClick="lbProcessOrder_Click" ValidationGroup="ProcessOrder" call="ProcessOrder">
                        Process Order
                    </asp:LinkButton>
                </div>
            </div>
        </div>
        <% if (Request.IsLocal)
           { %>
    </div>
    <% }
           else
           { %>
    </section>
    <%} %>
    <asp:LinkButton ID="lnkDummyServiceTicketAnn" class="grey2_btn alignright" runat="server"
        Style="display: none"></asp:LinkButton>
    <aCTK:ModalPopupExtender BehaviorID="mpeProcessOrderPopupBehaviour" ID="mpeProcessOrderPopup"
        TargetControlID="lnkDummyServiceTicketAnn" BackgroundCssClass="modalBackground"
        DropShadow="false" runat="server" CancelControlID="cboxClose" PopupControlID="pnlProcessOrderPopup">
    </aCTK:ModalPopupExtender>
    <asp:Panel ID="pnlProcessOrderPopup" runat="server" CssClass="cart-order-block popup-outer"
        Style="display: none; position: fixed; left: 36.4%; top: 5%;">
        <div class="popupInner">
            <div class="cart-block">
                <a href="#" class="close-btn" id="cboxClose">Close</a>
                <div class="cart-order">
                    <h2 id="h2OrderConfirmed" runat="server">
                    </h2>
                    <p id="pOrderSentForApproval" runat="server">
                    </p>
                    <div class="pop-btn-block" id="dvPrintOrder" runat="server">
                        <a id="hlPrintOrderConfirmation" href="javascript:;" class="blue-btn">Print Order Confirmation</a></div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div id="dvPrintOrderConfirmationContent" runat="server" style="display: none;">
    </div>
</asp:Content>
