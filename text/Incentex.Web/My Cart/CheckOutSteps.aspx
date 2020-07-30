<%@ Page Title="My Check Out" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="CheckOutSteps.aspx.cs" Inherits="My_Cart_CheckOutSteps" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml); $("#aspnetForm").validate(
                {
                    rules:
                    {
                        ctl00$ContentPlaceHolder1$txtReferenceName: { required: true },
                        ctl00$ContentPlaceHolder1$txtBFirstName: { required: true },
                        ctl00$ContentPlaceHolder1$txtBAddressLine1: { required: true },
                        ctl00$ContentPlaceHolder1$ddlBCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlBState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlBCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtBCity: { required: true },
                        ctl00$ContentPlaceHolder1$txtSName: { required: true },
                        ctl00$ContentPlaceHolder1$txtSAddress: { required: true },
                        ctl00$ContentPlaceHolder1$ddlSCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlSState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlSCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtSCity: { required: true },
                        ctl00$ContentPlaceHolder1$txtSTelephone: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtSZip: { required: true, alphanumeric: true, validzipcode: GetCountryCode($("#ctl00_ContentPlaceHolder1_ddlSCountry :selected").text()) },
                        ctl00$ContentPlaceHolder1$txtBZip: { required: true, alphanumeric: true, validzipcode: GetCountryCode($("#ctl00_ContentPlaceHolder1_ddlBCountry :selected").text()) },
                        //ctl00$ContentPlaceHolder1$txtBEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$txtSEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$ddlPaymentOption: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtPaymentOptionCode: { required: true }, //, digits: degitsRequired(), minlength: minLength7() },
                        ctl00$ContentPlaceHolder1$txtCreditCardNumber: { required: true },
                        ctl00$ContentPlaceHolder1$txtCVVNumber: { required: true },
                        ctl00$ContentPlaceHolder1$txtCardHolderName: { required: true },
                        ctl00$ContentPlaceHolder1$ddlCostCode: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlReplacementReason: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtSAirport: { required: CheckForFixedShippingAddress() }
                    },
                    messages:
                    {
                        ctl00$ContentPlaceHolder1$txtReferenceName: { required: replaceMessageString(objValMsg, "Required", "reference name for this order ie: New Uniform Shirts") },
                        ctl00$ContentPlaceHolder1$txtBFirstName: { required: replaceMessageString(objValMsg, "Required", "Name") },
                        ctl00$ContentPlaceHolder1$txtBAddressLine1: { required: replaceMessageString(objValMsg, "Required", "Address") },
                        ctl00$ContentPlaceHolder1$ddlBCountry: { NotequalTo: "<br />" + replaceMessageString(objValMsg, "NotEqualTo", "Country") },
                        ctl00$ContentPlaceHolder1$ddlBState: { NotequalTo: "<br />" + replaceMessageString(objValMsg, "NotEqualTo", "State") },
                        ctl00$ContentPlaceHolder1$ddlBCity: { NotequalTo: "<br />" + replaceMessageString(objValMsg, "NotEqualTo", "City") },
                        ctl00$ContentPlaceHolder1$txtBCity: { required: replaceMessageString(objValMsg, "Required", "city name") },
                        ctl00$ContentPlaceHolder1$txtSName: { required: replaceMessageString(objValMsg, "Required", "Name") },
                        ctl00$ContentPlaceHolder1$txtSAddress: { required: replaceMessageString(objValMsg, "Required", "Address") },
                        ctl00$ContentPlaceHolder1$ddlSCountry: { NotequalTo: "<br />" + replaceMessageString(objValMsg, "NotEqualTo", "Country") },
                        ctl00$ContentPlaceHolder1$ddlSState: { NotequalTo: "<br />" + replaceMessageString(objValMsg, "NotEqualTo", "State") },
                        ctl00$ContentPlaceHolder1$ddlSCity: { NotequalTo: "<br />" + replaceMessageString(objValMsg, "NotEqualTo", "City") },
                        ctl00$ContentPlaceHolder1$txtSCity: { required: replaceMessageString(objValMsg, "Required", "city name") },
                        ctl00$ContentPlaceHolder1$txtSTelephone: {
                            required: replaceMessageString(objValMsg, "Required", "telephone number"),
                            alphanumeric: replaceMessageString(objValMsg, "Number", "")
                        },
                        ctl00$ContentPlaceHolder1$txtSZip: {
                            required: replaceMessageString(objValMsg, "Required", "ZipCode"),
                            alphanumeric: replaceMessageString(objValMsg, "Number", ""),
                            validzipcode: replaceMessageString(objValMsg, "ValidZipCode", "")
                        },
                        ctl00$ContentPlaceHolder1$txtBZip: {
                            required: replaceMessageString(objValMsg, "Required", "ZipCode"),
                            alphanumeric: replaceMessageString(objValMsg, "Number", ""),
                            validzipcode: replaceMessageString(objValMsg, "ValidZipCode", "")
                        },
                        //                        ctl00$ContentPlaceHolder1$txtBEmail: {
                        //                            required: replaceMessageString(objValMsg, "Required", "email"),
                        //                            email: replaceMessageString(objValMsg, "Valid", "email address")
                        //                        },
                        ctl00$ContentPlaceHolder1$txtSEmail: {
                            required: replaceMessageString(objValMsg, "Required", "email"),
                            email: replaceMessageString(objValMsg, "Valid", "email address")
                        },
                        ctl00$ContentPlaceHolder1$ddlPaymentOption: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Payment Option") },
                        ctl00$ContentPlaceHolder1$ddlCostCode: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Cost Code") },
                        ctl00$ContentPlaceHolder1$txtPaymentOptionCode: { required: replaceMessageString(objValMsg, "Required", "Code") },
                        ctl00$ContentPlaceHolder1$txtCreditCardNumber: { required: replaceMessageString(objValMsg, "Required", "Credit Card Number") },
                        ctl00$ContentPlaceHolder1$txtCVVNumber: { required: replaceMessageString(objValMsg, "Required", "CVV Number") },
                        ctl00$ContentPlaceHolder1$txtCardHolderName: { required: replaceMessageString(objValMsg, "Required", "Card holder name") },
                        ctl00$ContentPlaceHolder1$ddlReplacementReason: { NotequalTo: "<br />" + replaceMessageString(objValMsg, "NotEqualTo", "Reason") },
                        ctl00$ContentPlaceHolder1$txtSAirport: { required: "<br />" + replaceMessageString(objValMsg, "Required", "Airport") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCostCode")
                            error.insertAfter("#dvCostCode");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlPaymentOption")
                            error.insertAfter("#dvPaymentOption");
                        else
                            error.insertAfter(element);
                    }

                });
            });

            $("#ctl00_ContentPlaceHolder1_lnkbtnBillingNextStep").click(function() {
                $('#dvLoader').show();
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });

            $("#ctl00_ContentPlaceHolder1_lnkbtnBillingNextStepBottom").click(function() {
                $('#dvLoader').show();
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });

            $("#ctl00_ContentPlaceHolder1_lnkbtnReviewOrderNextStep").click(function() {
                return $('#aspnetForm').valid();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkbtnReviewOrderNextStepSecond").click(function() {
                return $('#aspnetForm').valid();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkbtnShippingNextStep").click(function() {
                return $('#aspnetForm').valid();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkbtnShippingNextStepBottom").click(function() {
                return $('#aspnetForm').valid();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkbtnMainProcessOrderNow").click(function() {
                return $('#aspnetForm').valid();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkbtnMainProcessOrderNowForCompanyPay").click(function() {
                return $('#aspnetForm').valid();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkbtnProcessPaymentCreditcard").click(function() {
                return $('#aspnetForm').valid();
            });  
            
            function CheckForInternationalUser()
            {
                if($("#ctl00_ContentPlaceHolder1_hfIsInternationalUser").val().trim() == "true")
                {
                     $("#airport_label").html("Airport:");
                     return true;
                }
                else
                {
                    $("#airport_label").html("Airport:<br />(optional)");
                    return false;
                }
                
            }
            
            function CheckForFixedShippingAddress()
            {
                //hfIsAirportFieldRequired
                if($("#ctl00_ContentPlaceHolder1_hfIsAirportFieldRequired").val().trim() == "true")
                {
                     $("#airport_label").html("Airport:");
                     return true;
                }
                else
                {
                    $("#airport_label").html("Airport:<br />(optional)");
                    return false;
                }
            }
        });
        
    </script>

    <style type="text/css">
        .form_table td
        {
            padding: 2px 0px;
        }
        .form_table td.label_text
        {
            width: 80px;
            vertical-align: middle;
            color: #73767C;
            text-align: right;
            padding-right: 5px;
        }
        .form_table td.label_input
        {
            width: 200px;
        }
        .form_table label
        {
        }
        .form_table input
        {
            height: 25px;
            line-height: normal;
            padding: 5px 0px;
        }
        .card_details dd .card_select_box input
        {
            height: 28px;
            line-height: normal;
            padding: 7px;
        }
        .form_table input.w_label
        {
            width: 98%;
        }
        .form_table span.error
        {
            color: red;
            display: block;
            font-style: italic;
            margin-left: 1%;
            padding: 0px;
            text-align: left;
        }
        .packaging_pos .custom_sel_name span.error, .card_details dd span.error, .form_box span.error, .packaging_bg span.error
        {
            color: red;
            display: block;
            font-style: italic;
            text-align: left;
            font-size: 11px;
            line-height: 14px;
            margin-bottom: -5px;
        }
        .black_round_box .black2_round_middle
        {
            float: left;
        }
        .form_box span.input_label
        {
            color: #F4F4F4;
            float: left;
            font-size: 11px;
            margin-top: -2px;
            width: 98px;
        }
        .error
        {
            color: Red;
        }
    </style>

    <script type="text/javascript" language="javascript">
        //check for credit card number verification
        function CheckCardNumber() {
            var cardType;
            if (document.getElementById('<%=rdbMasterCard.ClientID %>') != null && document.getElementById('<%=rdbMasterCard.ClientID %>').checked == true)
                cardType = "Master Card";
            else if (document.getElementById('<%=rdbVisaCard.ClientID %>') != null && document.getElementById('<%=rdbVisaCard.ClientID %>').Checked == true)
                CardType = "Visa";
            else if (document.getElementById('<%=rdbDiscoder.ClientID %>') != null && document.getElementById('<%=rdbDiscoder.ClientID %>').Checked == true)
                CardType = "Discover";
            else if (document.getElementById('<%=rdbAmericanExpress.ClientID %>') != null && document.getElementById('<%=rdbAmericanExpress.ClientID %>').Checked == true)
                CardType = "American Express";

            if (document.getElementById('<%=txtCreditCardNumber.ClientID %>') != null) {
                var ccnum = document.getElementById('<%=txtCreditCardNumber.ClientID %>').value;
                var valueFilter;

                if (ccnum != "") {
                    if (cardType.toLowerCase().indexOf("visa") >= 0) {
                        valueFilter = /^4\d{3}-?\d{4}-?\d{4}-?\d{4}$/;
                    }
                    else if (cardType.toLowerCase().indexOf("master card") >= 0) {
                        valueFilter = /^5[1-5]\d{2}-?\d{4}-?\d{4}-?\d{4}$/;
                    }
                    else if (cardType.toLowerCase().indexOf("american express") >= 0) {
                        valueFilter = /^3[4,7]\d{13}$/;
                    }
                    else if (cardType.toLowerCase().indexOf("diners club") >= 0) {
                        valueFilter = /^3[0,6,8]\d{12}$/;
                    }
                    else if (cardType.toLowerCase().indexOf("discover") >= 0) {
                        valueFilter = /^6011-?\d{4}-?\d{4}-?\d{4}$/;
                    }
                    else {
                        valueFilter = /^4\d{3}-?\d{4}-?\d{4}-?\d{4}$/;
                    }

                    if (!valueFilter.test(ccnum)) {
                        alert("Invalid Credit Card Number");
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
            return true;
        }

        function CheckForValidAdderssBookTitle() {
            if ($("#ctl00_ContentPlaceHolder1_txtAddToMyAddressBookName").val() == "") {
                alert("Please enter value for address name.");
                return false;
            }
            return true;
        }

        //this is for checked credit card type radio button when click on image also.
        function CreditCardRadioButtonClick(control) {
            $("#ctl00_ContentPlaceHolder1_" + control).attr('checked', true);
        }

        //        function degitsRequired() {
        //            return $('#ctl00_ContentPlaceHolder1_ddlPaymentOption :selected').text() == "Cost-Center Code";
        //        }
        //        
        //        function minLength7() {            
        //            if ($('#ctl00_ContentPlaceHolder1_ddlPaymentOption :selected').text() == "Cost-Center Code")
        //                return 7;
        //            else 
        //                return 0;
        //        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <asp:HiddenField ID="hfIsInternationalUser" runat="server" />
    <asp:HiddenField ID="hfIsAirportFieldRequired" runat="server" />
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="secure_checkout">
        <div class="spacer20">
        </div>
        <h2 class="heading">
            Secure Checkout</h2>
        <div class="form_pad">
            <asp:MultiView runat="server" ID="mvCheckoutSteps" ActiveViewIndex="0" OnActiveViewChanged="mvCheckoutSteps_ActiveViewChanged">
                <asp:View ID="reviewOrderView" runat="server">
                    <ul class="checkout_steps">
                        <li class="review_active"><a href="#" title="Review Order"><span>Review Order</span></a></li>
                        <li class="shipping"><a href="#" title="Shipping"><span>Shipping</span></a></li>
                        <li class="selectpayment"><a href="#" title="Select Payment"><span>Select Payment</span></a></li>
                        <li class="billing"><a href="#" title="Billing"><span>Billing</span></a></li>
                        <li class="ordercomplete"><a href="#" title="Order Complete"><span>Order Complete</span></a></li>
                    </ul>
                    <div class="next_step">
                        <asp:LinkButton runat="server" ID="lnkbtnReviewOrderNextStep" OnClick="lnkbtnReviewOrderNextStep_Click"><img src="../Images/Checkout_NextStepSmall.png" alt="Next Step &gt;&gt;" /></asp:LinkButton></div>
                    <div id="dvPaymentMessage1" runat="server" visible="false">
                        <div class="spacer5">
                        </div>
                        <div style="text-align: left; color: Red">
                            <asp:Label ID="lblPaymentMessage1" runat="server"></asp:Label>
                        </div>
                        <div class="spacer5">
                        </div>
                    </div>
                    <div class="add_products">
                        <asp:Label ID="lblrptMsg" runat="server" Style="padding-left: 20%" ForeColor="Red"
                            Visible="false"></asp:Label>
                        <%--Start shopping cart item panel--%>
                        <asp:Repeater ID="rptMyShoppingCart" runat="server" OnItemDataBound="rptMyShoppingCart_ItemDataBound">
                            <ItemTemplate>
                                <table cellpadding="0" cellspacing="0" runat="server" id="tblOrderItem">
                                    <tr>
                                        <td style="width: 20%; vertical-align: top;">
                                            <div class="alignleft">
                                                <div class="agent_img">
                                                    <p class="upload_photo gallery">
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
                                                        <div class="cart_header" runat="server" id="dvOrderItemHeader">
                                                            <div class="left_co">
                                                            </div>
                                                            <div class="right_co">
                                                            </div>
                                                            <table cellpadding="0" cellspacing="0" class="txt13">
                                                                <tr>
                                                                    <td style="width: 33%;" class="leftalign">
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
                                                        <asp:HiddenField ID="hdnIsBulkOrder" runat="server" Value='<%# Eval("IsBulkOrder") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnMasterItemNo" Value='<%# Eval("MasterItemNo") %>' />
                                                        <asp:HiddenField ID="hdnSubCategory" runat="server" Value='<%# Eval("SubCategoryName") %>' />
                                                        <asp:HiddenField ID="hdnStoreProductID" runat="server" Value='<%# Eval("StoreProductID") %>' />
                                                        <asp:HiddenField ID="hdnTailoringOption" Value='<%# Eval("TailoringOption") %>' runat="server" />
                                                        <asp:HiddenField runat="server" ID="hdnItemNumber" Value='<%# Eval("item") %>' />
                                                        <asp:HiddenField ID="hdnRunCharge" Value='<%# Eval("RunCharge") %>' runat="server" />
                                                        <asp:HiddenField ID="hdnTailoringLength" Value='<%# Eval("TailoringLength") %>' runat="server" />
                                                        <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.MyShoppingCartID") %>'
                                                            Style="display: none;"></asp:Label>
                                                        <asp:Label runat="server" ID="lblProductDescription" Text='<%# Eval("ProductDescrption") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 15%;" class="centeralign">
                                                        <%# DataBinder.Eval(Container,"DataItem.Size") %>
                                                        <asp:HiddenField ID="hdnCreditmessage" Value='<%# Eval("CreditMessage") %>' runat="server" />
                                                    </td>
                                                    <td id="Td3" style="width: 12%;" runat="server" class="centeralign">
                                                        <asp:Label runat="server" ID="InventoryData" Text='<%# DataBinder.Eval(Container,"DataItem.Inventory") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnInventoryLevelShow" Value='<%# Eval("ShowInventoryLevel") %>'
                                                            runat="server" />
                                                    </td>
                                                    <td style="width: 12%;" class="centeralign">
                                                        <div class="qty_selbox centeralign">
                                                            <asp:Label runat="server" Width="25px" ID="lblQuantity" Style="text-align: center;"
                                                                Text='<%# DataBinder.Eval(Container, "DataItem.Quantity")%>'></asp:Label>
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
                                                                <asp:Label runat="server" ID="lblTailaringLength" Width="25px" Text=' <%# DataBinder.Eval(Container, "DataItem.TailoringLength")%>'></asp:Label>
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
                                                            <asp:Label ID="lblRunCharge" Style="color: Red" runat="server"></asp:Label>
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
                                        <td style="width: 70%;">
                                            &nbsp;
                                        </td>
                                        <td class="rightalign" style="padding-right: 60px; width: 30%;">
                                            Shipping : &nbsp;
                                            <asp:Label runat="server" ID="lblTotalShippingCharge"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="total_count txt14">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td class="rightalign" style="padding-right: 60px; padding-top: 5px;">
                                            Total :&nbsp;
                                            <asp:Label runat="server" ID="lblTotalProductPrice"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <%--End shopping cart item panel--%>
                        <%--Start issuance policy item panel--%>
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
                                            <table class="shoppingcart_box" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td colspan="6" style="padding: 0px;">
                                                        <div class="cart_header">
                                                            <div class="left_co">
                                                            </div>
                                                            <div class="right_co">
                                                            </div>
                                                            <table cellpadding="0" width="100%" cellspacing="0" class="txt13">
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
                                                                    <td style="width: 16%;" runat="server" id="tdUnitPrice">
                                                                        <asp:Label ID="lblnitPrice" runat="server" Text="Unit Price"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 18%;" runat="server" id="tdTotal">
                                                                        <asp:Label ID="lblTotalview" runat="server" Text="Total"></asp:Label>
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
                                                        <div class="qty_selbox">
                                                            <asp:Label runat="server" Width="25px" ID="lblQuantity" Text='<%# DataBinder.Eval(Container, "DataItem.Qty")%>'></asp:Label>
                                                        </div>
                                                    </td>
                                                    <td style="width: 16%;">
                                                        <div class="qty_selbox">
                                                            <asp:Label runat="server" ID="lblUnitPrice" Text='<%#Eval("RunCharge") != "" ? (Convert.ToDecimal(Eval("RunCharge")) + Convert.ToDecimal(Eval("Rate"))) : Eval("Rate")%>'></asp:Label>
                                                        </div>
                                                    </td>
                                                    <td style="width: 18%;">
                                                        <div class="qty_selbox">
                                                            <asp:Label runat="server" ID="lblTotal"></asp:Label>
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
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="4" id="pnlInventoryDateForIssuance" runat="server" style="text-align: left;"
                                                        class="alignleft">
                                                        Inventory To Arrive On :
                                                        <asp:Label ID="InventoryToArriveOnDateForIssuance" runat="server"></asp:Label>
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
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="txt14">
                                        <td colspan="3">
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
                            </ItemTemplate>
                            <SeparatorTemplate>
                                <div class="spacer10">
                                </div>
                            </SeparatorTemplate>
                            <FooterTemplate>
                                <table>
                                    <tr class="total_count txt14">
                                        <td colspan="5" class="rightalign" style="width: 89%;" runat="server" id="tdFooterShipping">
                                            Shipping :
                                        </td>
                                        <td class="rightalign">
                                            <asp:Label runat="server" ID="lblTotalShippingCharge"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="total_count txt14">
                                        <td colspan="5" class="rightalign" style="width: 89%;" runat="server" id="tdFooterTotal">
                                            Total :
                                        </td>
                                        <td class="rightalign">
                                            <asp:Label runat="server" ID="lblTotalProductPrice"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <%--End issuance policy item panel--%>
                        <div class="select_payment_step">
                            <!-- shipping_information start-->
                            <div id="dvPaymentMethodSelection" runat="server" class="select_box_pad" style="margin: 0 0 0 340px;
                                padding: 0;">
                                <table class="form_table" style="width: 87%;">
                                    <tr>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <label class="dropimg_width170">
                                                    <span class="custom-sel">
                                                        <asp:DropDownList ID="ddlPaymentOption" TabIndex="5" runat="server" onchange="pageLoad(this,value);"
                                                            Style="width: 227px;" OnSelectedIndexChanged="ddlPaymentOption_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div id="dvPaymentOption">
                                                    </div>
                                                </label>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label_input" style="text-align: center; color: Red; font-weight: bold;">
                                            <span>You can use Anniversary Credit in the Select Payment Step</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="spacer10">
                            </div>
                            <div class="special_order_box" style="display: none;">
                                <table class="form_table">
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box taxt_area clearfix">
                                                    <span class="input_label alignleft">Special Order Instructions:</span>
                                                    <div class="textarea_box alignright">
                                                        <div class="scrollbar">
                                                            <a href="#scroll" id="Scrolltop" class="scrolltop"></a><a href="#scroll" id="ScrollBottom"
                                                                class="scrollbottom"></a>
                                                        </div>
                                                        <asp:TextBox ID="txtSpecialOrderInstruction" runat="server" TextMode="MultiLine"
                                                            MaxLength="300" CssClass="scrollme"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="text-align: center;">
                                <asp:LinkButton ToolTip="My Shopping Cart" Visible="false" ID="lnkbtnReviewOrderMyShoppingCart"
                                    runat="server"><img src="../Images/Checkout_My Shopping Cart.png" alt="My Shopping Cart" /></asp:LinkButton>
                                <asp:LinkButton ToolTip="My Issuance Cart" Visible="false" ID="lnkbtnReviewOrderMyIssuanceCart"
                                    runat="server"><img src="../Images/Checkout_My Issuance Cart.png" alt="My Issuance Cart" /></asp:LinkButton>&nbsp;
                                <asp:LinkButton ToolTip="Cancel Order" ID="lnkbtnReviewOrderCancelOrder" runat="server"><img src="../Images/Checkout_Cancel Order.png" alt="Cancel Order" /></span></asp:LinkButton>&nbsp;
                                <asp:LinkButton ToolTip="Next Step" ID="lnkbtnReviewOrderNextStepSecond" OnClick="lnkbtnReviewOrderNextStep_Click"
                                    runat="server"><img src="../Images/Checkout_Next Stepbig.png" alt="Next Step" /></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View runat="server" ID="shippingView">
                    <ul class="checkout_steps">
                        <li class="review_complete"><a href="#" title="Review Order"><span>Review Order</span></a></li>
                        <li class="shipping_active"><a href="#" title="Shipping"><span>Shipping</span></a></li>
                        <li class="selectpayment"><a href="#" title="Select Payment"><span>Select Payment</span></a></li>
                        <li class="billing"><a href="#" title="Billing"><span>Billing</span></a></li>
                        <li class="ordercomplete"><a href="#" title="Order Complete"><span>Order Complete</span></a></li>
                    </ul>
                    <div class="shipping_billing_title" style="float: left;">
                        <h2>
                            Order Shipping Information</h2>
                    </div>
                    <div class="next_step">
                        <asp:LinkButton runat="server" ID="lnkbtnShippingNextStep" OnClick="lnkbtnShippingNextStep_Click"><img src="../Images/Checkout_NextStepSmall.png" alt="Next Step &gt;&gt;" /></span></asp:LinkButton></div>
                    <div class="select_payment_step">
                        <!-- shipping_information start-->
                        <div class="select_box_pad" style="margin: 0; padding: 0; width: 100%;">
                            <div id="dvShippingMessage" runat="server" visible="false">
                                <div class="spacer10">
                                </div>
                                <div style="text-align: left; color: Red">
                                    <asp:Label ID="lblShippingMessage" runat="server" class="error"></asp:Label>
                                </div>
                                <div class="spacer10">
                                </div>
                            </div>
                            <div class="black_top_co">
                                <span>&nbsp;</span></div>
                            <div class="black_middle">
                                <table class="form_table">
                                    <tr>
                                        <td class="label_text">
                                            <label>
                                                First Name:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtSName" TabIndex="2" runat="server" CssClass="w_label" MaxLength="100"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                Last Name:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtSLName" TabIndex="3" runat="server" class="w_label" MaxLength="100"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                Company:
                                                <br />
                                                (optional)</label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtSCompany" TabIndex="4" runat="server" CssClass="w_label" MaxLength="100"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label_text">
                                            <label>
                                                Department:
                                                <br />
                                                (optional)</label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <label class="dropimg_width170">
                                                    <span class="custom-sel">
                                                        <asp:DropDownList ID="ddlDepartment" TabIndex="5" runat="server" onchange="pageLoad(this,value);">
                                                        </asp:DropDownList>
                                                    </span>
                                                </label>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                        <td class="label_text">
                                            <label id="airport_label">
                                                Airport:
                                                <br />
                                                (optional)</label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtSAirport" TabIndex="6" runat="server" CssClass="w_label" MaxLength="100"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                Address:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <asp:TextBox ID="txtSAddress" runat="server" MaxLength="35" TabIndex="7"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label_text">
                                            <label>
                                                Address
                                                <br />
                                                Line 2:
                                                <br />
                                                (optional)
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <asp:TextBox ID="txtSAddress2" runat="server" MaxLength="35" TabIndex="7"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                Suite/Apt.:<br />
                                                (optional)
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <asp:TextBox ID="txtSStreet" runat="server" MaxLength="200" TabIndex="7"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                Country:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <asp:UpdatePanel ID="upnlCountry" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box select_pad">
                                                        <span class="custom-sel">
                                                            <asp:DropDownList ID="ddlSCountry" TabIndex="8" runat="server" onchange="pageLoad(this,value);"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSCountry_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </span>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label_text">
                                            <label>
                                                State:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <asp:UpdatePanel ID="upnlState" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box select_pad">
                                                        <span class="custom-sel">
                                                            <asp:DropDownList TabIndex="9" ID="ddlSState" runat="server" onchange="pageLoad(this,value);"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSState_SelectedIndexChanged">
                                                                <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                City:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <asp:UpdatePanel ID="upnlCity" runat="server">
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="ddlSCity" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box select_pad">
                                                        <span class="custom-sel">
                                                            <asp:DropDownList ID="ddlSCity" TabIndex="10" runat="server" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlSCity_SelectedIndexChanged">
                                                                <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <%--<td class="label_text">
                                            <label>
                                                County:
                                                <br />
                                                (optional)</label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtCounty" runat="server" TabIndex="11" CssClass="w_label" MaxLength="100"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                    </tr>
                                    <tr>--%>
                                        <td class="label_text">
                                            <label>
                                                Zip Code:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtSZip" runat="server" TabIndex="12" CssClass="w_label" MaxLength="50"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label_text">
                                            <label>
                                                Phone:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtSTelephone" TabIndex="13" runat="server" CssClass="w_label" MaxLength="50"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                Email:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtSEmail" TabIndex="14" runat="server" CssClass="w_label" MaxLength="100"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                        <div id="trSCityOther" runat="server" visible="false">
                                            <td class="label_text">
                                                <label>
                                                    City Name:
                                                </label>
                                            </td>
                                            <td class="label_input">
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <asp:TextBox ID="txtSCity" TabIndex="15" runat="server" class="w_label" MaxLength="100"></asp:TextBox></div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </td>
                                        </div>
                                    </tr>
                                    <tr>
                                        <div id="trOrderFor" runat="server" visible="false">
                                            <td class="label_text">
                                                <label>
                                                    Order For:
                                                </label>
                                            </td>
                                            <td class="label_input">
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <label class="dropimg_width170">
                                                        <span class="custom-sel">
                                                            <asp:DropDownList ID="ddlOrderFor" TabIndex="16" runat="server" onchange="pageLoad(this,value);">
                                                            </asp:DropDownList>
                                                        </span>
                                                    </label>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </td>
                                        </div>
                                    </tr>
                                </table>
                                <!-- shipping_location start -->
                                <div id="dvShippingLocation" runat="server" class="alignright" style="width: 315px;
                                    margin-bottom: 15px;">
                                    <table class="form_table">
                                        <tr>
                                            <td class="label_text" style="width: 120px;">
                                                <label>
                                                    Shipping Location (s):
                                                </label>
                                            </td>
                                            <td>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box select_pad">
                                                    <span class="custom-sel">
                                                        <asp:DropDownList ID="ddlShippingLocations" TabIndex="1" DataTextField="Title" DataValueField="CompanyContactInfoID"
                                                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlShippingLocations_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div>
                                                    <label class="checkout_checkbox_label">
                                                        Add To Address Book:
                                                    </label>
                                                    <span class="checkout_checkbox alignleft" id="spnAddToMyAddressBook" runat="server">
                                                        <asp:CheckBox runat="server" ID="chkAddToMyAddressBook" TabIndex="16" AutoPostBack="true"
                                                            OnCheckedChanged="chkAddToMyAddressBook_CheckedChanged" />
                                                        <asp:LinkButton runat="server" ID="lnkbtnDummyAddToMyAddressBook" Style="display: none;"></asp:LinkButton>
                                                    </span>
                                                </div>
                                                <%--popup panel start for apply anniversary credit--%>
                                                <at:ModalPopupExtender ID="mpeAddToMyAddressBook" TargetControlID="lnkbtnDummyAddToMyAddressBook"
                                                    BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlAddToMyAddressBook"
                                                    CancelControlID="A1">
                                                </at:ModalPopupExtender>
                                                <asp:Panel ID="pnlAddToMyAddressBook" runat="server" Style="display: none;">
                                                    <div class="pp_pic_holder facebook" style="display: block; width: 331px; position: fixed;
                                                        left: 35%; top: 30%;">
                                                        <div class="pp_top" style="">
                                                            <div class="pp_left">
                                                            </div>
                                                            <div class="pp_middle">
                                                            </div>
                                                            <div class="pp_right">
                                                            </div>
                                                        </div>
                                                        <div class="pp_content_container">
                                                            <div class="pp_left" style="">
                                                                <div class="pp_right" style="">
                                                                    <div class="pp_content" style="height: 128px; display: block;">
                                                                        <div class="pp_fade" style="display: block;">
                                                                            <div class="pp_inline clearfix">
                                                                                <div class="form_popup_box" style="padding: 0px;">
                                                                                    <div class="card_details" style="margin: 20px 0px 0px 0px;">
                                                                                        <dl>
                                                                                            <dd style="width: 100%;">
                                                                                                <label style="color: #73767C; width: 96px; font-weight: bold; font-size: 13px; margin-left: 0px;
                                                                                                    margin-top: 6px;">
                                                                                                    Address Name:</label>
                                                                                                <div class="card_select_box" style="float: right; margin: 0px;">
                                                                                                    <asp:TextBox runat="server" ID="txtAddToMyAddressBookName" MaxLength="50"></asp:TextBox>
                                                                                                </div>
                                                                                            </dd>
                                                                                        </dl>
                                                                                    </div>
                                                                                    <div style="text-align: center;">
                                                                                        <asp:LinkButton ID="lnkbtnAddToMyAddressBookSave" CssClass="grey2_btn" runat="server"
                                                                                            OnClientClick="javascript:return CheckForValidAdderssBookTitle();" OnClick="lnkbtnAddToMyAddressBookSave_Click"><span>Save</span></asp:LinkButton>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="pp_details clearfix" style="width: 291px;">
                                                                                <a href="#" id="A1" runat="server" class="pp_close">Close</a>
                                                                                <p class="pp_description" style="display: none;">
                                                                                </p>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="pp_bottom" style="">
                                                            <div class="pp_left" style="">
                                                            </div>
                                                            <div class="pp_middle" style="">
                                                            </div>
                                                            <div class="pp_right" style="">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <%--popup panel end for apply anniversary credit--%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <!-- shipping_location end -->
                            </div>
                            <div class="black_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        <!-- shipping_information end -->
                    </div>
                    <div class="next_step cl" style="padding-top: 20px; padding-bottom: 35px;">
                        <asp:LinkButton runat="server" ID="lnkbtnShippingNextStepBottom" OnClick="lnkbtnShippingNextStep_Click"><img src="../Images/Checkout_NextStepSmall.png" alt="Next Step &gt;&gt;" /></asp:LinkButton></div>
                </asp:View>
                <asp:View runat="server" ID="selectPaymentView">
                    <ul class="checkout_steps">
                        <li class="review_complete"><a href="#" title="Review Order"><span>Review Order</span></a></li>
                        <li class="shipping_complete"><a href="#" title="Shipping"><span>Shipping</span></a></li>
                        <li class="selectpayment_active"><a href="#" title="Select Payment"><span>Select Payment</span></a></li>
                        <li class="billing"><a href="#" title="Billing"><span>Billing</span></a></li>
                        <li class="ordercomplete"><a href="#" title="Order Complete"><span>Order Complete</span></a></li>
                    </ul>
                    <div class="select_payment_step">
                        <div id="dvPaymentMessage2" runat="server" visible="false">
                            <div class="spacer5">
                            </div>
                            <div style="text-align: left; color: Red">
                                <asp:Label ID="lblPaymentMessage2" runat="server"></asp:Label>
                            </div>
                            <div class="spacer5">
                            </div>
                        </div>
                        <ul>
                            <li id="divNotDisplayissuanceCart" runat="server">
                                <table>
                                    <tr>
                                        <td runat="server" id="tdTotalOrderAmt">
                                            Items Ordered Total :
                                            <asp:Label CssClass="price" ID="lblOrderTotal" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td runat="server" id="tdShippingCharge">
                                            Shipping Charges :
                                            <asp:Label ID="lblFreeshipping" Visible="false" Text="Free Shipping" CssClass="errormessage"
                                                runat="server" />
                                            <asp:Label ID="lblShippingAmount" CssClass="price" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td runat="server" id="tdSalestax">
                                            Sales Tax :
                                            <asp:Label ID="lblSalesTax" CssClass="price" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="trAnniversaryCredit" runat="server">
                                        <td>
                                            Anniversary Credits Applied :
                                            <asp:Label ID="lblAnniversaryCredit" runat="server" CssClass="price" />
                                        </td>
                                    </tr>
                                    <tr id="trRemAnniversaryCredit" runat="server">
                                        <td>
                                            Remaining Anniversary Credit :
                                            <asp:Label ID="lblRemAnniversaryCredit" runat="server" CssClass="price" />
                                        </td>
                                    </tr>
                                    <tr id="trPurchasesNotEligableForAnniversary" runat="server">
                                        <td>
                                            Amount Not Eligable for Credit Use:
                                            <asp:Label ID="lblNotEligibleforAnniversarycredit" runat="server" CssClass="price" />
                                        </td>
                                    </tr>
                                    <tr id="trCorporateDiscount" runat="server" visible="false">
                                        <td>
                                            Corporate Discount:
                                            <asp:Label ID="lblCorporateDiscount" Text="0" runat="server" CssClass="price" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td runat="server" id="tdBalanceDue" style="border-top: groove 1px gray;">
                                            Your Balance Due :
                                            <asp:Label ID="lblPayByCreditCard" runat="server" CssClass="price" /><br />
                                        </td>
                                    </tr>
                                </table>
                            </li>
                            <li id="liAnniversaryCredits" runat="server" class="popup_btn">
                                <asp:LinkButton runat="server" ID="lnkbtnAnniversaryCredits" class="credit_btn"><span>Check Anniversary Credits</span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lnkbtnAnniversaryCreditsCancel" OnClientClick="javascript:return confirm('would you like to remove applied credit from this order?');"
                                    Visible="false" OnClick="lnkbtnAnniversaryCreditsCancel_Click" class="apply_btn"><span>Anniversary Credits Applied</span></asp:LinkButton>
                                <%--popup panel start for apply anniversary credit--%>
                                <at:ModalPopupExtender ID="mpeCancelOrder" TargetControlID="lnkbtnAnniversaryCredits"
                                    BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlApplyAnniversaryCredit"
                                    CancelControlID="closecancelpopup">
                                </at:ModalPopupExtender>
                                <asp:Panel ID="pnlApplyAnniversaryCredit" runat="server" Style="display: none;">
                                    <div class="pp_pic_holder facebook" style="display: block; width: 311px; position: fixed;
                                        left: 35%; top: 30%;">
                                        <div class="pp_top" style="">
                                            <div class="pp_left">
                                            </div>
                                            <div class="pp_middle">
                                            </div>
                                            <div class="pp_right">
                                            </div>
                                        </div>
                                        <div class="pp_content_container">
                                            <div class="pp_left" style="">
                                                <div class="pp_right" style="">
                                                    <div class="pp_content" style="height: 128px; display: block;">
                                                        <div class="pp_fade" style="display: block;">
                                                            <div class="pp_inline clearfix">
                                                                <div class="form_popup_box">
                                                                    <div class="label_bar" style="font-size: 13px; font-weight: bold;">
                                                                        Anniversary Credit Balance:<asp:Label runat="server" ID="lblAnniversaryCreditBallance"
                                                                            Text="$0.00"></asp:Label>
                                                                    </div>
                                                                    <div style="margin-left: 40px;">
                                                                        <asp:LinkButton ID="lnkbtnApplyAnniversaryCredit" CssClass="grey2_btn" runat="server"
                                                                            OnClick="lnkbtnApplyAnniversaryCredit_Click"><span>Apply to Current Order</span></asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="pp_details clearfix" style="width: 271px;">
                                                                <a href="#" id="closecancelpopup" runat="server" class="pp_close">Close</a>
                                                                <p class="pp_description" style="display: none;">
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="pp_bottom" style="">
                                            <div class="pp_left" style="">
                                            </div>
                                            <div class="pp_middle" style="">
                                            </div>
                                            <div class="pp_right" style="">
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <%--popup panel end for apply anniversary credit--%>
                            </li>
                            <li id="divPaymentOptionCompany" runat="server">
                                <p style="color: Red; width: 300px;">
                                    Order and Shipping Paid by Corporate.
                                </p>
                            </li>
                            <%--<li id="liPaymentOptions" runat="server">
                                <div class="select_box">
                                    <div class="packaging_pos">
                                        <div class="packaging_bg">
                                            <span class="label_text">Payment Option:</span> <span class="custom_sel_name" style="width: 165px;
                                                position: inherit;">
                                                <asp:DropDownList ID="ddlPaymentOption" onchange="pageLoad(this,value);" runat="server"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentOption_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </li>--%>
                            <li id="liPaymentOptions" runat="server">
                                <asp:Label ID="lblBalanceDueTip" runat="server" Text="(Below Payment Option would be applied for Balance Due)"
                                    Visible="false"></asp:Label>
                                <div class="select_payment_step_input">
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Payment Option:</span>
                                        <asp:Label ID="lblPaymentOption" runat="server" Style="width: 165px; position: inherit;"></asp:Label>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </li>
                            <li id="liReasonForReplacement" runat="server" visible="false">
                                <div class="select_box">
                                    <div class="packaging_pos">
                                        <div class="packaging_bg">
                                            <span class="label_text">Reason:</span> <span class="custom_sel_name" style="width: 165px;
                                                position: inherit;">
                                                <asp:DropDownList ID="ddlReplacementReason" onchange="pageLoad(this,value);" runat="server"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li id="liPaymentOptionCode" runat="server" visible="false">
                                <div class="select_payment_step_input">
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" runat="server" id="lblPaymentOptionCode"></span>
                                        <asp:TextBox runat="server" ID="txtPaymentOptionCode" CssClass="w_label" Height="29px"
                                            MaxLength="60"></asp:TextBox>
                                        <asp:Label ID="lblCostCenterMessage" runat="server" CssClass="errormessage"></asp:Label>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </li>
                            <li id="liPaymentOptiondropdownCode" runat="server" visible="false">
                                <div style="margin-top: 5px;">
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 102px;">Cost-Center code :</span>
                                        <label class="dropimg_width">
                                            <span class="custom-sel label-sel-small" style="width: 161px;">
                                                <asp:DropDownList ID="ddlCostCode" AutoPostBack="false" onchange="pageLoad(this,value);"
                                                    runat="server">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvCostCode">
                                            </div>
                                            <asp:Label ID="lblCostCenterdrpMessage" runat="server" CssClass="errormessage"></asp:Label>
                                        </label>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </li>
                        </ul>
                        <ul>
                            <li class="button">
                                <asp:LinkButton runat="server" ID="lnkbtnMainProcessOrderNow" OnClientClick="return CheckCardNumber();"
                                    OnClick="lnkbtnMainProcessOrderNow_Click"><img src="../Images/Checkout_Apply Payment & Next Step.png" alt="Apply Payment &amp; Next Step &gt;&gt;"/></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lnkbtnMainProcessOrderNowForCompanyPay" Visible="false"
                                    OnClientClick="return CheckCardNumber();" OnClick="lnkbtnMainProcessOrderNow_Click"><img src="../Images/Proceed to Next step.png" alt="Proceed to Next Step &gt;&gt;"/></asp:LinkButton>
                            </li>
                        </ul>
                    </div>
                    <asp:Panel runat="server" ID="pnlCreditCardPayment" Visible="false" CssClass="full-width">
                        <div style="text-align: center; clear: both; margin-bottom: -10px;">
                            <asp:Label ID="lblExpDateMessage" runat="server" CssClass="errormessage"></asp:Label>
                        </div>
                        <div class="credit_card">
                            <div class="black_top_co">
                                <span>&nbsp;</span></div>
                            <div class="black_middle">
                                <div class="alignleft list_title">
                                    Select Credit Card Type</div>
                                <div class="alignleft list">
                                    <ul>
                                        <li>
                                            <asp:RadioButton ID="rdbMasterCard" Checked="true" runat="server" GroupName="credit-card-type" />
                                            <img src="../Images/mastercard.png" width="51" height="32" onclick="javascript:CreditCardRadioButtonClick('rdbMasterCard');"
                                                alt="Mastercard" title="Mastercard" />
                                        </li>
                                        <li>
                                            <asp:RadioButton ID="rdbVisaCard" runat="server" GroupName="credit-card-type" />
                                            <img src="../Images/visa.png" width="51" height="32" onclick="javascript:CreditCardRadioButtonClick('rdbVisaCard');"
                                                alt="Visa" title="Visa" />
                                        </li>
                                        <li>
                                            <asp:RadioButton ID="rdbDiscoder" runat="server" GroupName="credit-card-type" />
                                            <img src="../Images/discover.png" width="51" height="32" onclick="javascript:CreditCardRadioButtonClick('rdbDiscoder');"
                                                alt="Discover" title="Discover" />
                                        </li>
                                        <li>
                                            <asp:RadioButton ID="rdbAmericanExpress" runat="server" GroupName="credit-card-type" />
                                            <img src="../Images/americanexp.png" width="51" height="32" onclick="javascript:CreditCardRadioButtonClick('rdbAmericanExpress');"
                                                alt="American Express" title="American Express" />
                                        </li>
                                    </ul>
                                </div>
                                <%--  <div class="alignright">
                                    <ul>
                                        <li class="button"><a href="selectpayment3.html" class="new_green_btn"><span>Process
                                            Payment &gt;&gt;</span></a> </li>
                                    </ul>
                                </div>--%>
                            </div>
                            <div class="black_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        <div class="card_details">
                            <div class="black_top_co">
                                <span>&nbsp;</span></div>
                            <div class="black_middle">
                                <p>
                                    <font style="color: Red; margin-right: 3px;">*</font>Indicates Required Filed</p>
                                <form>
                                <fieldset>
                                    <dl>
                                        <dt>
                                            <label for="name">
                                                <font style="color: Red; margin-right: 3px;">*</font>Cardholder Name</label>
                                        </dt>
                                        <dd>
                                            <div class="card_select_box">
                                                <asp:TextBox runat="server" ID="txtCardHolderName"></asp:TextBox>
                                            </div>
                                        </dd>
                                    </dl>
                                    <dl>
                                        <dt>
                                            <label for="name">
                                                <font style="color: Red; margin-right: 3px;">*</font>Credit Card Number</label>
                                        </dt>
                                        <dd>
                                            <div class="card_select_box">
                                                <asp:TextBox runat="server" ID="txtCreditCardNumber"></asp:TextBox>
                                            </div>
                                        </dd>
                                    </dl>
                                    <dl>
                                        <dt>
                                            <label for="name">
                                                <font style="color: Red; margin-right: 3px;">*</font>Expiration Date</label>
                                        </dt>
                                        <dd>
                                            <div class="card_select_box">
                                                <div class="packaging_pos">
                                                    <div class="packaging_bg">
                                                        <span class="custom_sel_name">
                                                            <asp:DropDownList runat="server" ID="ddlCreditCardExpirationMonth">
                                                                <asp:ListItem Value="1" Text="1 - January"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="2 - February"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="3 - March"></asp:ListItem>
                                                                <asp:ListItem Value="4" Text="4 - April"></asp:ListItem>
                                                                <asp:ListItem Value="5" Text="5 - May"></asp:ListItem>
                                                                <asp:ListItem Value="6" Text="6 - June"></asp:ListItem>
                                                                <asp:ListItem Value="7" Text="7 - July"></asp:ListItem>
                                                                <asp:ListItem Value="8" Text="8 - August"></asp:ListItem>
                                                                <asp:ListItem Value="9" Text="9 - September"></asp:ListItem>
                                                                <asp:ListItem Value="10" Text="10 - October"></asp:ListItem>
                                                                <asp:ListItem Value="11" Text="11 - November"></asp:ListItem>
                                                                <asp:ListItem Value="12" Text="12 - December"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card_select_box">
                                                <div class="packaging_pos">
                                                    <div class="packaging_bg">
                                                        <span class="custom_sel_name">
                                                            <asp:DropDownList runat="server" ID="ddlCreditCardExpirationYear">
                                                            </asp:DropDownList>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </dd>
                                    </dl>
                                    <dl>
                                        <dt>
                                            <label for="name">
                                                <font style="color: Red; margin-right: 3px;">*</font>CVV Number</label>
                                        </dt>
                                        <dd>
                                            <div class="card_select_box">
                                                <asp:TextBox runat="server" ID="txtCVVNumber"></asp:TextBox>
                                            </div>
                                            <asp:LinkButton runat="server" ID="lnkbtnCVVHelp">
                                                <img src="../Images/question_blue.png" alt="What is this?" width="12" height="12" border="0"
                                                    align="middle" /></asp:LinkButton>
                                            <%--popup panel start for cvv number help--%>
                                            <at:ModalPopupExtender ID="mpeCVVHelp" TargetControlID="lnkbtnCVVHelp" BackgroundCssClass="modalBackground"
                                                DropShadow="true" runat="server" PopupControlID="pnlCVVHelp" CancelControlID="A2">
                                            </at:ModalPopupExtender>
                                            <asp:Panel ID="pnlCVVHelp" runat="server" Style="display: none;">
                                                <div class="pp_pic_holder facebook" style="display: block; width: 532px; position: fixed;
                                                    left: 35%; top: 30%;">
                                                    <div class="pp_top" style="">
                                                        <div class="pp_left">
                                                        </div>
                                                        <div class="pp_middle">
                                                        </div>
                                                        <div class="pp_right">
                                                        </div>
                                                    </div>
                                                    <div class="pp_content_container">
                                                        <div class="pp_left" style="">
                                                            <div class="pp_right" style="">
                                                                <div class="pp_content" style="height: 360px; display: block;">
                                                                    <div class="pp_fade" style="display: block;">
                                                                        <div class="pp_inline clearfix">
                                                                            <div class="form_popup_box" style="padding: 0px;">
                                                                                <%--<iframe src="CVVNumber.htm" style="height:360px;width:510px;margin-top:20px;" frameborder="0" runat="server" id="ifrmCVVHelp"></iframe> --%>
                                                                                <img src="../Images/CVVHelp.jpg" alt="What is cvv number" style="margin-top: 30px;" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="pp_details clearfix" style="width: 532px;">
                                                                            <a href="#" id="A2" runat="server" class="pp_close">Close</a>
                                                                            <p class="pp_description" style="display: none;">
                                                                            </p>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="pp_bottom" style="">
                                                        <div class="pp_left" style="">
                                                        </div>
                                                        <div class="pp_middle" style="">
                                                        </div>
                                                        <div class="pp_right" style="">
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <%--popup panel end for cvv number help--%>
                                        </dd>
                                    </dl>
                                </fieldset>
                                &nbsp;&nbsp;&nbsp;&nbsp;</form>
                                <div class="alignright">
                                    <asp:LinkButton runat="server" CssClass="new_green_btn" ID="lnkbtnProcessPaymentCreditcard"
                                        OnClientClick="return CheckCardNumber();" OnClick="lnkbtnMainProcessOrderNow_Click"><span>Process Payment &gt;&gt;</span></asp:LinkButton>
                                </div>
                            </div>
                            <div class="black_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlMOASPayment" Visible="false">
                        <div class="card_details approval_manage">
                            <div class="black_top_co">
                                <span>&nbsp;</span></div>
                            <div class="black_middle">
                                <p>
                                    Manager Order Approval System</p>
                                <div class="form">
                                    <fieldset>
                                        <div runat="server" id="dvMOASManagerApprover">
                                        </div>
                                        <asp:HiddenField ID="hfMOASApproverLevelID" runat="server" />
                                    </fieldset>
                                    &nbsp;&nbsp;&nbsp;&nbsp;</div>
                            </div>
                            <div class="black_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </asp:Panel>
                </asp:View>
                <asp:View runat="server" ID="billingView">
                    <ul class="checkout_steps">
                        <li class="review_complete"><a href="#" title="Review Order"><span>Review Order</span></a></li>
                        <li class="shipping_complete"><a href="#" title="Shipping"><span>Shipping</span></a></li>
                        <li class="selectpayment_complete"><a href="#" title="Select Payment"><span>Select Payment</span></a></li>
                        <li class="billing_active"><a href="#" title="Billing"><span>Billing</span></a></li>
                        <li class="ordercomplete"><a href="#" title="Order Complete"><span>Order Complete</span></a></li>
                    </ul>
                    <div class="shipping_billing_title" style="float: left;">
                        <h2>
                            Order Billing Information:
                        </h2>
                    </div>
                    <div class="next_step">
                        <asp:LinkButton runat="server" ID="lnkbtnBillingNextStep" OnClick="lnkbtnBillingNextStep_Click"><img src="../Images/Checkout_NextStepSmall.png" alt="Next Step &gt;&gt;" /></span></asp:LinkButton></div>
                    <div class="select_payment_step">
                        <!-- billing_information start -->
                        <div class="select_box_pad" style="margin: 0; padding: 0; width: 100%;">
                            <div id="dvBillingMessage" runat="server" visible="false">
                                <div class="spacer5">
                                </div>
                                <div style="text-align: left; color: Red">
                                    <asp:Label ID="lblBillingMessage" runat="server"></asp:Label>
                                </div>
                                <div class="spacer5">
                                </div>
                            </div>
                            <div class="black_top_co">
                                <span>&nbsp;</span></div>
                            <div class="black_middle">
                                <table class="form_table">
                                    <tr>
                                        <td class="label_text">
                                            <label>
                                                First Name:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtBFirstName" TabIndex="1" runat="server" class="w_label" MaxLength="100"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                Last Name:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtBLastName" TabIndex="2" runat="server" class="w_label" MaxLength="100"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                Company:
                                                <br />
                                                (optional)</label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtBCompany" TabIndex="3" runat="server" class="w_label" MaxLength="100"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label_text">
                                            <label>
                                                Address:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <div class="textarea_box" style="height: auto; width: auto;">
                                                    <div class="scrollbar" style="height: 39px;">
                                                        <a class="scrolltop" href="#scroll"></a><a class="scrollbottom" href="#scroll"></a>
                                                    </div>
                                                    <asp:TextBox ID="txtBAddressLine1" TextMode="MultiLine" CssClass="w_label" TabIndex="4"
                                                        Height="32" runat="server" MaxLength="200"></asp:TextBox></div>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                Address
                                                <br />
                                                Line 2:
                                                <br />
                                                (optional)
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <div class="textarea_box" style="height: auto; width: auto;">
                                                    <div class="scrollbar" style="height: 39px;">
                                                        <a class="scrolltop" href="#scroll"></a><a class="scrollbottom" href="#scroll"></a>
                                                    </div>
                                                    <asp:TextBox ID="txtBAddressLine2" TextMode="MultiLine" CssClass="w_label" TabIndex="4"
                                                        Height="32" runat="server" MaxLength="200"></asp:TextBox></div>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                Country:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <asp:UpdatePanel ID="upnlBCountry" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box select_pad">
                                                        <span class="custom-sel">
                                                            <asp:DropDownList TabIndex="5" ID="ddlBCountry" runat="server" AutoPostBack="true"
                                                                onchange="pageLoad(this,value);" OnSelectedIndexChanged="ddlBCountry_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </span>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label_text">
                                            <label>
                                                State:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <asp:UpdatePanel ID="upnlBSate" runat="server">
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box select_pad">
                                                        <span class="custom-sel">
                                                            <asp:DropDownList TabIndex="6" ID="ddlBState" runat="server" AutoPostBack="true"
                                                                onchange="pageLoad(this,value);" OnSelectedIndexChanged="ddlBState_SelectedIndexChanged">
                                                                <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                City:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <asp:UpdatePanel ID="upnlBCity" runat="server">
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="ddlBCity" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box select_pad">
                                                        <span class="custom-sel">
                                                            <asp:DropDownList ID="ddlBCity" TabIndex="7" onchange="pageLoad(this,value);" runat="server"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlBCity_SelectedIndexChanged">
                                                                <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="label_text">
                                            <label>
                                                Zip Code:
                                            </label>
                                        </td>
                                        <td class="label_input">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtBZip" runat="server" TabIndex="8" CssClass="w_label" MaxLength="50"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label_text">
                                            <label>
                                                Reference Name:
                                            </label>
                                        </td>
                                        <td>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtReferenceName" TabIndex="9" class="w_label" runat="server" MaxLength="100"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                        <td class="label_text" id="tdBCityOtherLabel" runat="server" visible="false">
                                            <label>
                                                City Name:
                                            </label>
                                        </td>
                                        <td class="label_input" id="tdBCityOtherInput" runat="server" visible="false">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtBCity" runat="server" TabIndex="10" class="w_label" MaxLength="100"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                        <td class="label_text" style="display: none;">
                                            <label>
                                                Email:
                                            </label>
                                        </td>
                                        <td style="display: none;">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <asp:TextBox ID="txtBEmail" runat="server" CssClass="w_label" MaxLength="100"></asp:TextBox></div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="spacer20">
                                </div>
                                <table class="form_table" style="margin-left: 250px; width: auto; margin-bottom: 10px;">
                                    <tr>
                                        <td>
                                            <div>
                                                <label class="checkout_checkbox_label" style="font-size: 16px;">
                                                    Billing Information Same as Shipping Information
                                                </label>
                                                <span class="checkout_checkbox alignleft" id="spnBillingSameAsShipping" runat="server">
                                                    <asp:CheckBox runat="server" TabIndex="11" ID="chkBillingSameAsShipping" AutoPostBack="true"
                                                        OnCheckedChanged="chkBillingSameAsShipping_CheckedChanged" />
                                                </span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="black_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        <!-- billing_information end -->
                    </div>
                    <div class="next_step cl" style="padding-top: 20px; padding-bottom: 35px;">
                        <asp:LinkButton runat="server" ID="lnkbtnBillingNextStepBottom" OnClick="lnkbtnBillingNextStep_Click"><img src="../Images/Checkout_NextStepSmall.png" alt="Next Step &gt;&gt;" /></asp:LinkButton></div>
                </asp:View>
                <asp:View runat="server" ID="orderCompleteView">
                    <ul class="checkout_steps">
                        <li class="review_complete"><a href="#" title="Review Order"><span>Review Order</span></a></li>
                        <li class="shipping_complete"><a href="#" title="Shipping"><span>Shipping</span></a></li>
                        <li class="selectpayment_complete"><a href="#" title="Select Payment"><span>Select Payment</span></a></li>
                        <li class="billing_complete"><a href="#" title="Billing"><span>Billing</span></a></li>
                        <li class="ordercomplete_complete"><a href="#" title="Order Complete"><span>Order Complete</span></a></li>
                    </ul>
                    <div class="order_complete">
                        <img src="../Images/thankyouimg.png" width="561" height="381" />
                    </div>
                </asp:View>
                <asp:View runat="server" ID="orderReview">
                    <ul class="checkout_steps">
                        <li class="review_complete"><a href="#" title="Review Order"><span>Review Order</span></a></li>
                        <li class="shipping_complete"><a href="#" title="Shipping"><span>Shipping</span></a></li>
                        <li class="selectpayment_complete"><a href="#" title="Select Payment"><span>Select Payment</span></a></li>
                        <li class="billing_complete"><a href="#" title="Billing"><span>Billing</span></a></li>
                        <li class="ordercomplete_complete"><a href="#" title="Order Complete"><span>Order Complete</span></a></li>
                    </ul>
                    <div class="order_complete">
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
</asp:Content>
