<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MyOrderDetails.aspx.cs" Inherits="MyAccount_MyOrderDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/usercontrol/UPSPackageTracking.ascx" TagName="UPSPackageTracking"
    TagPrefix="ups" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:Panel runat="server" ID="pnlUPS">
    </asp:Panel>
    <style type="text/css">
        .textarea_box textarea
        {
            height: 96px;
        }
        .textarea_box
        {
            height: 96px;
        }
        .textarea_box .scrollbar
        {
            height: 96px;
        }
        .fontsizesmall
        {
            font-size: small;
        }
        .noteIncentex
        {
            background-color: #E1E0E0;
            color: black;
            font-family: "Trebuchet MS" ,tahoma,arial,verdana;
            font-size: 0.8em;
            padding-bottom: 5px;
            padding-top: 7px;
        }
        .shipmentclass
        {
            background: none repeat scroll 0 0 #101010;
            color: #72757C;
            display: block;
            border: solid 1px #1c1c1c;
            padding: 0px 8px;
            border-left: none;
            border-right: solid 1px #1f1f1f;
        }
        .rightalign
        {
            font-size: small;
            color: #72757C;
            text-align: right;
        }
        .width300
        {
            font-size: small;
            width: 300px !important;
        }
    </style>

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

        // check if value is proper in tracking number
        function CheckTrackingNumber(id) {
            var txt = document.getElementById(id);
            if (!IsValidTrackingNumber(txt.value)) {
                alert("Please enter valid tracking number");
                txt.focus();
            }

        }
        //Function that will check if tracking number is not inserted into the textbox ....
        //Created on 17 Mar by Ankit
        function textboxFinder(id) {
            var txt = document.getElementById(id);
            var txt1 = txt.id.split('_');
            var fin = txt1[0] + "_" + txt1[1] + "_" + txt1[2] + "_" + txt1[3] + "_" + "txtTrackingNo";
            var a = document.getElementById(fin);
            if (a.value == "") {
                alert('Please enter tracking number');
                return false;
            }
        }
        //End

        function IsValidTrackingNumber(sText) {
            var ValidChars = "0123456789-ABCDEFGHITJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
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

    <script type="text/javascript">
        function CheckFile() {
            var file = document.getElementById('ctl00_ContentPlaceHolder1_parentRepeater_ctl01_fpSupplierPurchadOrderUpload');
            var len = file.value.length;
            var ext = file.value;
            if (file.value.length <= 0) {
                alert('Please Upload Supplier Purchase Order(s).');
                document.getElementById('ctl00_ContentPlaceHolder1_parentRepeater_ctl01_fpSupplierPurchadOrderUpload').focus();
                return false;
            }
            else if (ext.substr(len - 3, len) != "pdf") {
                alert("Please Select a PDF File ");
                return false;
            }
        }
        function CheckFile1() {
            var file = document.getElementById('ctl00_ContentPlaceHolder1_parentRepeater_ctl01_fpSupplierInvoice');
            var len = file.value.length;
            var ext = file.value;
            if (file.value.length <= 0) {
                alert('Please Upload Supplier Invoice(s).');
                document.getElementById('ctl00_ContentPlaceHolder1_parentRepeater_ctl01_fpSupplierInvoice').focus();
                return false;
            }
            else if (ext.substr(len - 3, len) != "pdf") {
                alert("Please Select a PDF File ");
                return false;
            }
        }
        function CheckFile2() {
            var file = document.getElementById('ctl00_ContentPlaceHolder1_parentRepeater_ctl01_fpCustomerInvoice');
            var len = file.value.length;
            var ext = file.value;
            if (file.value.length <= 0) {
                alert('Please Upload Customer Invoice(s).');
                document.getElementById('ctl00_ContentPlaceHolder1_parentRepeater_ctl01_fpCustomerInvoice').focus();
                return false;
            }
            else if (ext.substr(len - 3, len) != "pdf") {
                alert("Please Select a PDF File ");
                return false;
            }


        }

        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calendar-small.png',
                buttonImageOnly: true
            });
        });


        $(function() {
            scrolltextarea(".scrollme1", "#Scrolltop1", "#ScrollBottom1");

        });


         
    </script>

    <script type="text/javascript" language="javascript">

        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();

            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({

            });
            /*Function to get X and Y co-ordinates of a browser*/
            posY = getScreenCenterY();
            posX = getScreenCenterX();
            $("#ctl00_ContentPlaceHolder1_hfY").val(posY);
            $("#ctl00_ContentPlaceHolder1_hfX").val(posX);

            function getScreenCenterY() {
                var y = 0;
                y = getScrollOffset() + (getInnerHeight() / 2);
                return (y);
            }

            function getScreenCenterX() {
                return (document.body.clientWidth / 2);
            }

            function getInnerHeight() {
                var y;
                if (self.innerHeight) // all except Explorer
                {
                    y = self.innerHeight;
                }
                else if (document.documentElement &&
            document.documentElement.clientHeight)
                // Explorer 6 Strict Mode
                {
                    y = document.documentElement.clientHeight;
                }
                else if (document.body) // other Explorers
                {
                    y = document.body.clientHeight;
                }
                return (y);
            }

            function getScrollOffset() {
                var y;
                if (self.pageYOffset) // all except Explorer
                {
                    y = self.pageYOffset;
                }
                else if (document.documentElement &&
            document.documentElement.scrollTop)
                // Explorer 6 Strict
                {
                    y = document.documentElement.scrollTop;
                }
                else if (document.body) // all other Explorers
                {
                    y = document.body.scrollTop;
                }
                return (y);
            }

            /*End*/

        });
    });
   
    </script>

    <script type="text/javascript">

        function compare() {
            var first = document.getElementById('ctl00_ContentPlaceHolder1_parentRepeater_ctl01_gvOrderDetail_ctl02_txtQtyOrderRemaining').value;
            var second = document.getElementById('ctl00_ContentPlaceHolder1_parentRepeater_ctl01_gvOrderDetail_ctl02_txtQtyShipped').value;
            if (second == first) {
                return false;
            }
        }
 
    </script>

    <div class="black_round_box">
        <div class="black2_round_top">
            <span></span>
        </div>
        <div class="worldlink_btn">
            <div align="right">
           
                <a id="lnkRetunrExchangeDetails" runat="server" style="margin-bottom: 0px; margin-top: 0px;"
                    title="Items to Return" class="gredient_btn"><span>Order Returns</span></a>
            </div>
        </div>
        <div class="black2_round_middle">
            <div class="form_pad" style="padding-top: 20px!important;">
                <div class="pro_search_pad" style="width: 940px;">
                    <div>
                        <div class="black_top_co">
                            <span>&nbsp;</span></div>
                        <div class="black_middle order_detail_pad">
                            <table class="order_detail" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label>
                                                        Order Number :
                                                    </label>
                                                    <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label>
                                                        Reference Name :
                                                    </label>
                                                    <asp:Label ID="lblOrderBy" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label>
                                                        Ordered Date :
                                                    </label>
                                                    <asp:Label ID="lblOrderedDate" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label>
                                                        Order Status :
                                                    </label>
                                                    <asp:Label runat="server" ID="lblOrderStatus"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label>
                                                        Payment Method :
                                                    </label>
                                                    <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trCreditType" runat="server">
                                                <td style="font-size: small;">
                                                    <label>
                                                        Credit Type :
                                                    </label>
                                                    <asp:Label ID="lblCreditType" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
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
                                    <span>Bill To: </span>
                                </div>
                                <div class="alignright">
                                    <span style="padding-left: 22px!important;">Ship To: </span>
                                </div>
                            </div>
                            <div>
                                <div class="alignleft" style="width: 49%;">
                                    <div class="tab_content_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="tab_content">
                                        <table class="order_detail" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBName" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBCompany" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBAddressLine1" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBAddressLine2" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBCity" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBCountry" runat="server" /></label>
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
                                                <td style="padding-left: 2px;">
                                                    <label class="width300">
                                                        <asp:Label ID="lblSName" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 2px;">
                                                    <label class="width300">
                                                        <asp:Label ID="lblSCompany" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 2px;">
                                                    <label class="width300">
                                                        <asp:Label ID="lblSAddress" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 2px;">
                                                    <label class="width300">
                                                        <asp:Label ID="lblSAddress2" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 2px;">
                                                    <label class="width300">
                                                        <asp:Label ID="lblSStreet" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 2px;">
                                                    <label class="width300">
                                                        <asp:Label ID="lblSCity" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 2px;">
                                                    <label class="width300">
                                                        <asp:Label ID="lblSCountry" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 2px;">
                                                    <label class="width300">
                                                        <asp:Label ID="lblSOrderFor" runat="server" />
                                                    </label>
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
                        <table width="100%">
                            <tr>
                                <td style="width: 80%; vertical-align: middle;">
                                    <div>
                                        <div class="alignleft" style="width: 100%;">
                                            <span>Order Summary:</span></div>
                                    </div>
                                    <div class="spacer20">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvOrderDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvOrderDetail_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField SortExpression="QuantityOrder">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnQuantityOrder" runat="server" CommandArgument="OrderNumber"
                                                        CommandName="Sort"><span>Ordered<br /></span></asp:LinkButton>
                                                    <div class="corner">
                                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                    </div>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnMyShoppingCartiD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"MyShoppingCartiD") %>' />
                                                    <asp:Label ID="txtQtyOrder" CssClass="first" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                    <div class="corner">
                                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="RemainingOrderQuantity" Visible="false">
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
                                            <asp:TemplateField SortExpression="ItemNumber">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                                        CommandName="Sort"><span>Item #<br /></span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemStyle CssClass="g_box" Width="10%" />
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
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ProductDescrption">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnProductDescrption" runat="server" CommandArgument="Shipper"
                                                        CommandName="Sort"><span>Description</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderProductDescrption" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductionDescription" Text='<%# Convert.ToString(Eval("ProductDescrption")).Length > 40 ? Convert.ToString(Eval("ProductDescrption")).Substring(0, 40).Trim() + "..." : (Convert.ToString(Eval("ProductDescrption")) + "&nbsp;") %>'
                                                        ToolTip='<% #Eval("ProductDescrption")  %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemStyle CssClass="g_box" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Price">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnPrice" runat="server" CommandArgument="Price" CommandName="Sort"><span>Price</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderPrice" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPrice" Text=' <%# "$" + Convert.ToDecimal(Eval("Price")).ToString("#,##0.00")%>'
                                                        runat="server"></asp:Label>
                                                        <asp:Label ID="lblMOASPrice" Text=' <%# "$" + Convert.ToDecimal(Eval("MOASItemPrice")).ToString("#,##0.00")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemStyle CssClass="b_box centeralign" Width="6%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ExtendedPrice">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnExtendedPrice" runat="server" CommandArgument="ExtendedPrice"
                                                        CommandName="Sort"><span>Extended</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderExtendedPrice" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExtendedPrice" Text='<%# "$" + Convert.ToDecimal((Convert.ToDecimal(Eval("Price")) * Convert.ToDecimal(Eval("Quantity")))).ToString("#,##0.00")%>'
                                                        runat="server"></asp:Label>
                                                    <asp:Label ID="lblMOASExtendedPrice" Text='<%# "$" + Convert.ToDecimal((Convert.ToDecimal(Eval("MOASItemPrice")) * Convert.ToDecimal(Eval("Quantity")))).ToString("#,##0.00")%>'
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemStyle CssClass="g_box centeralign" Width="6%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="BackorderedUntil">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnBackorderedUntil" runat="server" CommandArgument="BackorderedUntil "
                                                        CommandName="Sort"><span >Back Ordered Until </span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderBackorderedUntil" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBackOrderDate" runat="server" Text='<%# "&nbsp;" + (Convert.ToString(Eval("BackOrderedUntil")) != "" ? Convert.ToDateTime(Eval("BackOrderedUntil")).ToString("MM/dd/yyyy") : "") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr id="trPrice" runat="server">
                                <td align="right" style="text-align: right; padding-right: 52px;">
                                    <table>
                                        <tr>
                                            <td class="rightalign">
                                                <label>
                                                    Shipping: $
                                                </label>
                                                <asp:Label ID="lblShipping" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="rightalign">
                                                <label>
                                                    Sales Tax: $
                                                </label>
                                                <asp:Label ID="lblSalesTax" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="rightalign">
                                                <label>
                                                    Order Total: $
                                                </label>
                                                <asp:Label ID="lblOrderTotal" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trCorporateDiscount" visible="false">
                                            <td class="rightalign">
                                                <label>
                                                    Corporate Discount: $
                                                </label>
                                                <asp:Label ID="lblCorporateDiscount" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div class="alignnone spacer15">
                        </div>
                        <table width="100%">
                            <tr>
                                <td style="width: 80%; vertical-align: middle;">
                                    <div>
                                        <div class="alignleft" style="width: 100%;">
                                            <span>Invoice(s) Details:</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table width="100%">
                                        <tr id="paperw" runat="server" visible="false">
                                            <td style="width: 80%; vertical-align: middle;">
                                                <div class="clearfix billing_head">
                                                    <div class="alignleft" style="width: 100%;">
                                                        <span>Order Paperwork :</span></div>
                                                </div>
                                            </td>
                                            <td style="width: 20; vertical-align: top;">
                                                <div class="alignleft gallery">
                                                    <asp:LinkButton ID="lnkbtnOrderPaper1" class="grey2_btn alignright" runat="server"
                                                        Style="display: none"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnOrderPaper" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright"><span>+ Add Document</span></asp:LinkButton>
                                                    <at:ModalPopupExtender ID="ModalPopupExtenderOrder" TargetControlID="lnkbtnOrderPaper"
                                                        BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlNotesOrder"
                                                        CancelControlID="A3">
                                                    </at:ModalPopupExtender>
                                                </div>
                                                <div>
                                                    <asp:Panel ID="pnlNotesOrder" runat="server" Style="display: none;">
                                                        <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                                            left: 35%; top: 30%;">
                                                            <div class="pp_top" style="">
                                                                <div class="pp_left">
                                                                </div>
                                                                <div class="pp_middle">
                                                                </div>
                                                                <div class="pp_right">
                                                                </div>
                                                            </div>
                                                            <div class="pp_content_container" style="">
                                                                <div class="pp_left" style="">
                                                                    <div class="pp_right" style="">
                                                                        <div class="pp_content" style="height: 228px; display: block;">
                                                                            <div class="pp_loaderIcon" style="display: none;">
                                                                            </div>
                                                                            <div class="pp_fade" style="display: block;">
                                                                                <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                                                <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                                                                    <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                                                        style="visibility: visible;">previous</a>
                                                                                </div>
                                                                                <div id="Div1">
                                                                                    <div class="pp_inline clearfix">
                                                                                        <div class="form_popup_box">
                                                                                            <div class="label_bar" style="padding-left: 9px;">
                                                                                                <span>Select File Type:</span>
                                                                                                <asp:PlaceHolder ID="PHolderddlFileType" runat="server">
                                                                                                    <asp:DropDownList runat="server" ID="ddlFileType">
                                                                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                                                        <asp:ListItem Value="1">Customer Invoice</asp:ListItem>
                                                                                                        <asp:ListItem Value="2">Supplier Invoice</asp:ListItem>
                                                                                                        <asp:ListItem Value="3">Supplier Purchase Order</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </asp:PlaceHolder>
                                                                                                <asp:PlaceHolder ID="PlaceHolderddlFileTypeSupplier" runat="server">
                                                                                                    <asp:DropDownList runat="server" ID="ddlFileTypeSupplier">
                                                                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                                                        <asp:ListItem Value="1">Supplier Invoice</asp:ListItem>
                                                                                                        <asp:ListItem Value="2">Supplier Purchase Order</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </asp:PlaceHolder>
                                                                                            </div>
                                                                                            <div class="form_box" style="background-color: #E1E0E0;">
                                                                                                <span class="input_label">Upload Document : </span>
                                                                                                <input id="fpSupplierPurchadOrderUpload" type="file" runat="server" style="border: medium none;
                                                                                                    color: #ffffff; padding: 2px" />
                                                                                                <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                                                                                    <img src="../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file formats are
                                                                                                    <b>.pdf</b></div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="pp_details clearfix" style="width: 371px;">
                                                                                    <a href="#" id="A3" runat="server" class="pp_close">Close</a>
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
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="alignnone spacer15">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div>
                                                    <asp:GridView ID="gvSupplierPurchaseOrder" runat="server" AutoGenerateColumns="false"
                                                        HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                                        OnRowCommand="gvSupplierPurchaseOrder_RowCommand" RowStyle-CssClass="ord_content"
                                                        OnRowDataBound="gvSupplierPurchaseOrder_RowDataBound" ShowHeader="false">
                                                        <Columns>
                                                            <asp:TemplateField Visible="False" HeaderText="Id">
                                                                <HeaderTemplate>
                                                                    <span>OrderDocumentID</span>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblSupplierPurchaseOrderId" Text='<%# Eval("OrderDocumentID") %>' />
                                                                    <asp:HiddenField ID="hdnPSupplierId" runat="server" Value='<%# Eval("SupplierId") %>' />
                                                                    <asp:HiddenField runat="server" ID="hdnPOrderId" Value='<%# Eval("OrderId") %>' />
                                                                    <asp:HiddenField ID="hdnMyShoppingCartId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "MyShoppingCartID")%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="1%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoice" runat="server" Text="Invoice #"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="b_box" Width="5%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="FileName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lnkFileName" runat="server" Text='<% # (Convert.ToString(Eval("FileName")).Length > 30) ? Eval("FileName").ToString().Substring(0,30)+"..."  : Convert.ToString(Eval("FileName"))+ "&nbsp;"  %>'
                                                                        ToolTip='<% #Eval("FileName")  %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnFileNamePurchase" Value='<%# Eval("FileName") %>' runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="b_box" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblUserName" Text='<%# Eval("UploadedBy") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="b_box" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <span class="btn_space">
                                                                        <asp:LinkButton runat="server" ID="lnkuIcon" CommandName="viewPurchaseOrder" CommandArgument='<%# Eval("FileName") %>'
                                                                            CssClass="pdf2"></asp:LinkButton>
                                                                    </span>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" CssClass="b_box" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <span class="btn_space">&nbsp; </span>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20%" CssClass="b_box" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                        </table>
                        <div class="alignnone spacer15">
                        </div>
                        <table width="100%">
                            <tr>
                                <td style="width: 80%; vertical-align: middle;">
                                    <div>
                                        <div class="alignleft" style="width: 100%;">
                                            <span>Shipment & Tracking Details</span></div>
                                    </div>
                                    <div class="spacer20">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvShippedOrderDetail" runat="server" AutoGenerateColumns="false"
                                        HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                        RowStyle-CssClass="ord_content" OnRowDataBound="gvShippedOrderDetail_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnShipment" runat="server" CommandArgument="ShipingDate"
                                                        CommandName="Sort"> <span>Shipment</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderShipment" runat="server"></asp:PlaceHolder>
                                                    <div class="corner">
                                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                    </div>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:Label CssClass="first" runat="server" ID="lblShipment" Text='<%#Convert.ToInt32(Container.DataItemIndex + 1)%>'></asp:Label>
                                                    <div class="corner">
                                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ShipingDate">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnShipingDate" runat="server" CommandArgument="ShipingDate"
                                                        CommandName="Sort"> <span >Shipping Date</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderShipingDate" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblShipingDate" Text='<%# Eval("ShipingDate","{0:d}") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtShipDate" runat="server" Text='<%# Eval("ShipingDate","{0:d}") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemStyle CssClass="b_box centeralign" Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="boxes">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnBoxes" runat="server" CommandName="Sort"> <span>Boxes</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderBoxes" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNoOfBoxes" runat="server" Text='<%#Eval("NoOfBoxes")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="sLookupName">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnsLookupName" runat="server" CommandArgument="sLookupName"
                                                        CommandName="Sort"><span>Carrier</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholdersLookupName" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSelectedService" runat="server"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdnShipperService" Value='<%# Eval("ShipperService") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdnShipperService1" Value='<%# Eval("ShipperService") %>' />
                                                    <asp:DropDownList ID="ddlShipperService" runat="server">
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                                <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="TrackingNo">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnTrackingNo" runat="server" CommandArgument="TrackingNo"
                                                        CommandName="Sort"><span >Tracking Number</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderTrackingNo" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdnTrackingNo" Value='<%# Eval("TrackingNo") %>' />
                                                    <asp:GridView ID="gvTrackingNo" runat="server" AutoGenerateColumns="false" CssClass="orderreturn_box"
                                                        GridLines="None" RowStyle-CssClass="ord_content" Width="100%" ShowHeader="false"
                                                        OnRowCommand="gvTrackingNo_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <span>
                                                                        <asp:LinkButton runat="server" ID="lbltrackingNo" Text='<%#Eval("trackingnuber")%>'
                                                                            CommandName="TrackingNumber" CommandArgument='<%#Eval("trackingnuber")%>'></asp:LinkButton></span>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="centeralign" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtTrackingNumber" runat="server" Text='<%# Eval("TrackingNo") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemStyle CssClass="g_box centeralign" Width="30%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Shippment">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnShippment" runat="server" CommandArgument="Shippment" CommandName="Sort"><span>Packing Slip</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderShippment" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <span class="btn_space">
                                                        <asp:HyperLink ID="hypShippment" CommandArgument='<%# Eval("PackageId") %>' CommandName="Go"
                                                            ToolTip="View Shipment Details" runat="server">
                                                            <asp:Image ID="img" runat="server" ImageUrl="~/Images/shipment06.png" Height="24px"
                                                                Width="24px" />
                                                        </asp:HyperLink>
                                                    </span>
                                                    <asp:HiddenField ID="hdnShippment" runat="server" Value='<%# Eval("PackageId") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <div class="spacer20">
                        </div>
                        <div>
                            <div class="form_table">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box taxt_area clearfix" style="height: 100px">
                                    <span class="input_label alignleft" style="height: 100px">Notes/History :</span>
                                    <div class="textarea_box alignright">
                                        <div class="scrollbar" style="height: 103px">
                                            <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                class="scrollbottom"></a>
                                        </div>
                                        <asp:TextBox ID="txtOrderNotesForCECA" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                            Height="100px" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                            <div class="alignnone spacer15">
                            </div>
                            <div class="rightalign gallery">
                                <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                                <asp:LinkButton ID="lnkAddNew" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright" ><span>+ Add Note</span></asp:LinkButton>
                                <at:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkAddNew" BackgroundCssClass="modalBackground"
                                    DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup">
                                </at:ModalPopupExtender>
                            </div>
                            <div>
                                <asp:Panel ID="pnlNotes" runat="server" Style="display: none">
                                    <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                        left: 35%; top: 25%">
                                        <div class="pp_top" style="">
                                            <div class="pp_left">
                                            </div>
                                            <div class="pp_middle">
                                            </div>
                                            <div class="pp_right">
                                            </div>
                                        </div>
                                        <div class="pp_content_container" style="">
                                            <div class="pp_left" style="">
                                                <div class="pp_right" style="">
                                                    <div class="pp_content" style="height: 228px; display: block;">
                                                        <div class="pp_loaderIcon" style="display: none;">
                                                        </div>
                                                        <div class="pp_fade" style="display: block;">
                                                            <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                            <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                                                <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                                    style="visibility: visible;">previous</a>
                                                            </div>
                                                            <div id="pp_full_res">
                                                                <div class="pp_inline clearfix">
                                                                    <div class="form_popup_box">
                                                                        <div class="label_bar">
                                                                            <span>Notes/History :
                                                                                <br />
                                                                                <br />
                                                                                <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNote" runat="server"></asp:TextBox></span>
                                                                        </div>
                                                                        <div>
                                                                            <asp:LinkButton ID="lnkButton" CommandName="SAVECACE" class="grey2_btn" runat="server"
                                                                                OnClick="lnkButton_Click"><span>Save Notes</span></asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="pp_details clearfix" style="width: 371px;">
                                                                <a href="#" id="closepopup" runat="server" class="pp_close">Close</a>
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
                            </div>
                            <br />
                            <br />
                            <br />
                            <%--End Note History--%>
                        </div>
                        <div class="spacer15">
                        </div>
                        <%--Special ORder Instruction Div--%>
                        <div class="black_middle order_detail_pad" id="CompanyNoteHistory" runat="server">
                            <%--Popup Notes--%>
                            <div class="form_table">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box taxt_area clearfix" style="height: 100px">
                                    <span class="input_label alignleft" style="height: 100px">Order Special Instruction</span>
                                    <div class="textarea_box alignright">
                                        <div class="scrollbar" style="height: 103px">
                                            <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                            </a>
                                        </div>
                                        <asp:TextBox ID="txtSpecialOrderInstruction" runat="server" TextMode="MultiLine"
                                            CssClass="scrollme1" Height="100px" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                            <%--End Note History--%>
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
    <input type="hidden" id="hfX" value="" runat="server" />
    <input type="hidden" id="hfY" value="" runat="server" />
    <%--Modal Popup Panels--%>
</asp:Content>
