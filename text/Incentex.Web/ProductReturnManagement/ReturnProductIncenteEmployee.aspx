<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ReturnProductIncenteEmployee.aspx.cs" Inherits="ProductReturnManagement_ReturnProductIncenteEmployee"
    Title="Return Product >> For Incentex Employee" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
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



    //End
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
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

    <div class="black_round_box">
        <div class="black2_round_top">
            <span></span>
        </div>
        <div class="black2_round_middle">
            <div class="form_pad">
                <div class="pro_search_pad" style="width: 940px;">
                    <div>
                        <div style="text-align: center; color: Red; font-size: larger;">
                            <asp:Label ID="lblmsg" runat="server">
                            </asp:Label>
                        </div>
                        <div class="black_top_co">
                            <span>&nbsp;</span></div>
                        <div class="black_middle order_detail_pad">
                        </div>
                        <div class="black_middle order_detail_pad">
                            <table class="order_detail" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label>
                                                        Return Order # :
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
                                                    <label style="padding-left: 28px!important;">
                                                        Ordered Date :
                                                    </label>
                                                    <asp:Label ID="lblOrderedDate" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Order Status :
                                                    </label>
                                                    <asp:Label runat="server" ID="lblOrderStatus"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Payment Method :
                                                    </label>
                                                    <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Type of Return :
                                                    </label>
                                                    <asp:DropDownList runat="server" ID="ddlRequesting" Style="background-color: #303030;
                                                        border: medium none; color: #ffffff; width: 150px; padding: 2px" runat="server">
                                                    </asp:DropDownList>
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
                       <!--Start 15-March-2012 -->
                        <div class="black_middle order_detail_pad">
                            <div class="clearfix billing_head">
                             <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                      {%>
                                <div class="alignleft">
                                    <span>Bill To: </span>
                                </div>
                                 <%}
                                      else
                                      {%>
                                      <div class="alignleft">
                                        <span>&nbsp;</span>
                                    </div>
                                    <%} %>
                                <div class="alignright">
                                    <span style="padding-left: 29px!important;">Ship To: </span>
                                </div>
                            </div>
                            <div>
                             <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                      {%>
                                <div class="alignleft" style="width: 49%;">
                                    <div class="tab_content_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="tab_content">
                                        <table class="order_detail" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <label style="font-size: small; width: 300px !important;">
                                                        <asp:Label ID="lblBName" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label style="font-size: small; width: 300px !important;">
                                                        <asp:Label ID="lblBCompany" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label style="font-size: small; width: 300px !important;">
                                                        <asp:Label ID="lblBAddress" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label style="font-size: small; width: 300px !important;">
                                                        <asp:Label ID="lblBCity" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label style="font-size: small; width: 300px !important;">
                                                        <asp:Label ID="lblBCountry" runat="server" /></label>
                                                          <asp:HiddenField ID="hfBillingInfoID" runat="server" />
                                                </td>
                                            </tr>
                                             <tr>
                                                    <td align="right">
                                                        <asp:LinkButton ID="btnEditBillingDummy" class="grey2_btn alignright" runat="server"
                                                            Style="display: none"></asp:LinkButton>
                                                        <asp:LinkButton ID="btnEditBilling" runat="server" CssClass="grey2_btn alignright"
                                                            OnClick="btnEditBilling_Click"><span>+ Edit Billing</span></asp:LinkButton>
                                                        <at:ModalPopupExtender ID="modalEditBilling" TargetControlID="btnEditBillingDummy"
                                                            BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlBilling"
                                                            CancelControlID="A8">
                                                        </at:ModalPopupExtender>
                                                        <asp:Panel ID="pnlBilling" runat="server">
                                                            <div class="pp_pic_holder facebook" style="display: block; width: 411px;">
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
                                                                            <div class="pp_content" style="height: 375px; display: block;">
                                                                                <div class="pp_loaderIcon" style="display: none;">
                                                                                </div>
                                                                                <div class="pp_fade" style="display: block;">
                                                                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                                                    <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                                                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                                                            style="visibility: visible;">previous</a>
                                                                                    </div>
                                                                                    <div id="Div5">
                                                                                        <div class="pp_inline clearfix">
                                                                                            <div class="form_popup_box">
                                                                                                <div class="label_bar">
                                                                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        First Name:</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox class="popup_input" ID="txtBillingFnameEdit" runat="server"></asp:TextBox></span>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        Last Name :</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox class="popup_input" ID="txtBillingLnameEdit" runat="server"></asp:TextBox></span>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        Company Name :</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox class="popup_input" ID="txtBillingCompanyNameEdit" runat="server"></asp:TextBox></span>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        Address :</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox class="popup_input" ID="txtBillingAddressEdit" runat="server"></asp:TextBox></span>
                                                                                                    <span>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        Country :</label>
                                                                                                    <span>
                                                                                                        <asp:DropDownList ID="drpCountry" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged"
                                                                                                            AutoPostBack="true" runat="server">
                                                                                                        </asp:DropDownList>
                                                                                                    </span><span>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        State :</label>
                                                                                                    <span>
                                                                                                        <asp:DropDownList ID="drpState" OnSelectedIndexChanged="drpState_SelectedIndexChanged"
                                                                                                            AutoPostBack="true" runat="server">
                                                                                                        </asp:DropDownList>
                                                                                                    </span><span>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        City :</label>
                                                                                                    <span>
                                                                                                        <asp:DropDownList ID="drpCity" runat="server">
                                                                                                        </asp:DropDownList>
                                                                                                    </span><span>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        Zip Code :</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox class="popup_input" ID="txtZipcodeEdit" runat="server"></asp:TextBox></span>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        Email Address :</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox class="popup_input" ID="txtEmailAdrressEdit" runat="server"></asp:TextBox></span>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        Phone Number :</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox class="popup_input" ID="txtPhoneNumberEdit" runat="server"></asp:TextBox></span>
                                                                                                </div>
                                                                                                <div class="label_bar btn_padinn">
                                                                                                    <asp:Button ID="btnSaveBilling" runat="server" OnClick="btnSaveBilling_Click" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="pp_details clearfix" style="width: 371px;">
                                                                                        <a href="#" id="A8" runat="server" class="pp_close">Close</a>
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
                                                    </td>
                                                </tr>
                                        </table>
                                    </div>
                                    <div class="tab_content_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                 <%}
                                      else
                                      {%>
                                    <div class="alignleft" style="width: 49%;">
                                        &nbsp;
                                    </div>
                                    <%} %>
                                <div class="alignright" style="width: 49%;">
                                    <div class="tab_content_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="tab_content">
                                        <table class="order_detail" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <label style="font-size: small; width: 300px !important;">
                                                        <asp:Label ID="lblSName" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label style="font-size: small; width: 300px !important;">
                                                        <asp:Label ID="lblSCompany" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label style="font-size: small; width: 300px !important;">
                                                        <asp:Label ID="lblSAddress" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label style="font-size: small; width: 300px !important;">
                                                        <asp:Label ID="lblSCity" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label style="font-size: small; width: 300px !important;">
                                                        <asp:Label ID="lblSCountry" runat="server" />
                                                         <asp:HiddenField ID="hfShippingInfoID" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                             <tr>
                                                    <td align="right">
                                                        <asp:LinkButton ID="btnEditShippingDummy" class="grey2_btn alignright" runat="server"
                                                            Style="display: none"></asp:LinkButton>
                                                        <asp:LinkButton ID="btnEditShipping" runat="server" CssClass="grey2_btn alignright"
                                                            OnClick="btnEditShipping_Click"><span>+ Edit Shipping</span></asp:LinkButton>
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
                         <!--End 15-March-2012 -->
                        <div class="black_bot_co">
                            <span>&nbsp;</span></div>
                        <div class="spacer15">
                        </div>
                        <table width="100%">
                            <tr>
                                <td style="width: 80%; vertical-align: middle;">
                                    <div>
                                        <div class="alignleft" style="width: 100%;">
                                            <span>Product Return Summary:</span></div>
                                    </div>
                                    <div class="spacer20">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvProductReturn" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvProductReturn_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField SortExpression="ItemNumber">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                                        CommandName="Sort"> <span >Item #<br /></span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ReceivedQty">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnReceivedQty" runat="server" CommandArgument="ReceivedQty"
                                                        CommandName="Sort"><span>Received Qty<br /></span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderReceivedQty" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceivedQty" runat="server" Text='<% # (Convert.ToString(Eval("QuantityReceived")).ToString().Length > 10) ? Eval("QuantityReceived").ToString().Substring(0,10)+"..." :  Convert.ToString(Eval("QuantityReceived")+ "&nbsp;") %>'
                                                        ToolTip='<%# Eval("QuantityReceived") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdnshippid" runat="server" Value='<%# Eval("ShippID") %>' />
                                                    <asp:HiddenField ID="hdnitemNo" runat="server" Value='<%# Eval("ItemNumber") %>' />
                                                    <asp:HiddenField ID="hdnShoppingCartId" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box centeralign" Width="14%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ReturnQty">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnReturnQty" runat="server" CommandArgument="ReturnQty" CommandName="Sort"><span >Return Qty<br /></span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderReturnQty" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblReturnQty" Text='<%# Eval("ReturnQty") %>' />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box centeralign" Width="9%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Color">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnQtyReceived" runat="server" CommandArgument="Color" CommandName="Sort"> <span >Color<br /></span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderColor" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <span class="btn_space">
                                                        <asp:Image ID="imgColor" runat="server" Height="20" Width="20" />
                                                    </span>
                                                    <asp:Label runat="server" ID="lblColor"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box centeralign" Width="8%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Size">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span >Size</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderSize" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="centeralign" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSize" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ProductDescrption">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnProductDescrption" runat="server" CommandArgument="Shipper"
                                                        CommandName="Sort"><span >Description</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderProductDescrption" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductionDescription" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box" Width="30%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Requesting" Visible="false">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnRequesting" runat="server" CommandArgument="Requesting"
                                                        CommandName="Sort"><span>Requesting</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderRequesting" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnRequest" runat="server" Value='<%# Eval("Requesting") %>' />
                                                    <span>
                                                        <%--  <asp:DropDownList runat="server" ID="ddlRequesting" Style="background-color: #303030;
                                border: medium none; color: #ffffff; width: 100px; padding: 2px" runat="server">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>--%>
                                                    </span>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box" Width="10%" />
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
                                    <span class="input_label alignleft" style="height: 100px">Reason for Return:</span>
                                    <div class="textarea_box alignright">
                                        <div class="scrollbar" style="height: 103px">
                                            <a href="#scroll" id="A3" class="scrolltop"></a><a href="#scroll" id="A4" class="scrollbottom">
                                            </a>
                                        </div>
                                        <asp:TextBox ID="txtPrdDescription" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                            Height="100px" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </div>
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
                            <div class="rightalign gallery" id="divAddNots" runat="server">
                                <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                                <asp:LinkButton ID="lnkAddNew" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright"><span>+ Add Note</span></asp:LinkButton>
                                <at:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkAddNew" BackgroundCssClass="modalBackground"
                                    DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup">
                                </at:ModalPopupExtender>
                            </div>
                            <div>
                                <asp:Panel ID="pnlNotes" runat="server">
                                <div class="pp_pic_holder facebook" style="display: block; width: 411px;">
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
                                                <div class="pp_content" style="height: 30px; display: block;">
                                                    <div class="pp_fade" style="display: block;">
                                                        <span class="noteIncentex" style="width: 80%; font-size: 12px; background-color: inherit;
                                                            color: Black; font-weight: bold;">
                                                            <img src="../Images/errorpage.png" height="25px" width="25px" alt="note:" />&nbsp;&nbsp;YOU
                                                            ARE ABOUT TO SEND A NOTE TO A CUSTOMER </span>
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
                                                                <div class="form_popup_box" style="padding-top: 15px;">
                                                                    <div class="label_bar">
                                                                        <span>Notes/History :
                                                                            <br />
                                                                            <br />
                                                                            <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNote" runat="server"></asp:TextBox></span>
                                                                    </div>
                                                                    <div>
                                                                        <asp:LinkButton ID="lnkButton" runat="server" CommandName="SAVECACE" class="grey2_btn"
                                                                            OnClientClick="return CheckNoteHistory()" OnClick="lnkButton_Click"><span>Save Notes</span></asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>
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
                        
                        <%--NewLy Added--%>
                           <div class="spacer20">
                        </div>
                        <div>
                            <div class="form_table">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box taxt_area clearfix" style="height: 100px">
                                    <span class="input_label alignleft" style="height: 100px">Incentex Internal Notes Only</span>
                                    <div class="textarea_box alignright">
                                        <div class="scrollbar" style="height: 103px">
                                            <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2"
                                                class="scrollbottom"></a>
                                        </div>
                                        <asp:TextBox ID="txtOrderNotesForIE" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                            Height="100px" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                            <div class="alignnone spacer15">
                            </div>
                            <div class="rightalign gallery" id="div1" runat="server">
                                <asp:LinkButton ID="lnkDummyAddNewIE" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                                <asp:LinkButton ID="lnkAddNewIE" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright"><span>+ Add Note</span></asp:LinkButton>
                                <at:ModalPopupExtender ID="modalAddnotesIE" TargetControlID="lnkAddNewIE" BackgroundCssClass="modalBackground"
                                    DropShadow="true" runat="server" PopupControlID="pnlNotesIE" CancelControlID="A5">
                                </at:ModalPopupExtender>
                            </div>
                            <div>
                                <asp:Panel ID="pnlNotesIE" runat="server">
                                <div class="pp_pic_holder facebook" style="display: block; width: 411px;">
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
                                                <div class="pp_content" style="height: 45px; display: block;">
                                                    <div class="pp_fade" style="display: block;">
                                                        <span class="noteIncentex" style="width: 80%; font-size: 12px; background-color: inherit;
                                                            color: Black; font-weight: bold;">
                                                            <img src="../Images/errorpage.png" height="25px" width="25px" alt="note:" />&nbsp;&nbsp;
                                                            You are about to post an Incentex Internal Note. 
                                                            <br />
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Customer will not be able to view this note. 
                                                            </span>
                                                        <div class="pp_details clearfix" style="width: 371px;">
                                                            <a href="#" id="A5" runat="server" class="pp_close">Close</a>
                                                            <p class="pp_description" style="display: none;">
                                                            </p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
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
                                                        <div id="Div2">
                                                            <div class="pp_inline clearfix">
                                                                <div class="form_popup_box" style="padding-top: 15px;">
                                                                    <div class="label_bar">
                                                                        <span>Incentex Employee Notes/History :
                                                                            <br />
                                                                            <br />
                                                                            <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNoteIE" runat="server"></asp:TextBox></span>
                                                                    </div>
                                                                    <div>
                                                                        <asp:LinkButton ID="lnkNoteHisForIE" runat="server" CommandName="SAVECACE" class="grey2_btn"
                                                                            OnClientClick="return CheckNoteHistory()" OnClick="lnkNoteHisForIE_Click"><span>Save Notes</span></asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>
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
                        <%--END--%>
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
