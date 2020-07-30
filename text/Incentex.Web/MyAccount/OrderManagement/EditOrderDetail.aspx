<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EditOrderDetail.aspx.cs" Inherits="MyAccount_OrderManagement_EditOrderDetail"
    Title="Edit Order Detail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
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
        .order_detail label
        {
            color: #72757C;
        }
        .fontsizesmall
        {
            font-size: small;
        }
        .width300
        {
            font-size: small;
            width: 300px !important;
        }
    </style>

    <script type="text/javascript">
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
         
    </script>

    <script type="text/javascript" language="javascript">

        $().ready(function() {
            
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

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <div class="black_round_box">
        <div class="black2_round_top">
            <span></span>
        </div>
        <div class="black2_round_middle">
            <div class="form_pad">
                <div class="pro_search_pad" style="width: 920px;">
                    <div id="mainDIV">
                        <div>
                            <%--First Div--%>
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
                                                        <asp:HiddenField ID="hdORderID" runat="server" />
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
                                                <tr id="trCreditType" runat="server">
                                                    <td style="font-size: small;">
                                                        <label style="padding-left: 28px!important;">
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
                            <%--Second Div--%>
                            <div class="black_top_co">
                                <span>&nbsp;</span></div>
                            <div class="black_middle order_detail_pad">
                                <div class="clearfix billing_head">
                                    <div class="alignleft">
                                        <span>Bill To: </span>
                                    </div>
                                    <div class="alignright">
                                        <span style="padding-left: 29px!important;">Ship To:</span>
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
                                                            <asp:Label ID="lblBAddress1" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="width300">
                                                            <asp:Label ID="lblBAddress2" runat="server" /></label>
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
                                                <tr>
                                                    <td>
                                                        <label class="width300">
                                                            <asp:Label ID="lblBEmailAddress" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="width300">
                                                            <asp:Label ID="lblBPhoneNumber" runat="server" />
                                                            <asp:HiddenField ID="hfBillingInfoID" runat="server" />
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:LinkButton ID="btnEditBillingDummy" CssClass="grey2_btn alignright" runat="server"
                                                            Style="display: none"></asp:LinkButton>
                                                        <asp:LinkButton ID="btnEditBilling" runat="server" CssClass="grey2_btn alignright"
                                                            OnClick="btnEditBilling_Click"><span>+ Edit Billing</span></asp:LinkButton>
                                                        <at:ModalPopupExtender ID="modalEditBilling" TargetControlID="btnEditBillingDummy"
                                                            BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlBilling"
                                                            CancelControlID="closepopup">
                                                        </at:ModalPopupExtender>
                                                        <asp:Panel ID="pnlBilling" runat="server" Style="display: none;">
                                                            <div class="pp_pic_holder facebook" style="display: block; width: 411px; left: 35%;
                                                                top: 25%; position: fixed;" runat="server" id="dvHolderFacebook">
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
                                                                            <div class="pp_content" style="height: 385px; display: block;" runat="server" id="dvContent">
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
                                                                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        First Name:</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox CssClass="popup_input" ID="txtBillingFnameEdit" runat="server"></asp:TextBox></span>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        Last Name :</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox CssClass="popup_input" ID="txtBillingLnameEdit" runat="server"></asp:TextBox></span>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label>
                                                                                                        Company Name :</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox CssClass="popup_input" ID="txtBillingCompanyNameEdit" runat="server"></asp:TextBox></span>
                                                                                                </div>
                                                                                                <div class="label_bar">
                                                                                                    <label runat="server" id="lblAddressEdit">
                                                                                                        Address 1:</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox class="popup_input" ID="txtBillingAddressEdit" runat="server"></asp:TextBox></span>
                                                                                                </div>
                                                                                                <div class="label_bar" runat="server" id="dvAddress2Edit">
                                                                                                    <label>
                                                                                                        Address 2 :</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox class="popup_input" ID="txtBillingAddress2Edit" MaxLength="35" runat="server"></asp:TextBox></span>
                                                                                                </div>
                                                                                                <div class="label_bar" runat="server" id="dvStreetEdit">
                                                                                                    <label>
                                                                                                        Suite/Apt. :</label>
                                                                                                    <span>
                                                                                                        <asp:TextBox class="popup_input" ID="txtBillingStreetEdit" MaxLength="35" runat="server"></asp:TextBox></span>
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
                                                    <td>
                                                        <label class="width300">
                                                            <asp:Label ID="lblSName" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="width300">
                                                            <asp:Label ID="lblSCompany" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="width300">
                                                            <asp:Label ID="lblSAddress" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="width300">
                                                            <asp:Label ID="lblSAddress2" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="width300">
                                                            <asp:Label ID="lblSStreet" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="width300">
                                                            <asp:Label ID="lblSCity" runat="server" />
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="width300">
                                                            <asp:Label ID="lblSCountry" runat="server" />
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="width300">
                                                            <asp:Label ID="lblSEmailAddress" runat="server" /></label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="width300">
                                                            <asp:Label ID="lblSPhoneNumber" runat="server" />
                                                            <asp:HiddenField ID="hfShippingInfoID" runat="server" />
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:LinkButton ID="btnEditShippingDummy" CssClass="grey2_btn alignright" runat="server"
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
                            <div class="black_bot_co">
                                <span>&nbsp;</span></div>
                            <div class="spacer15">
                            </div>
                            <%--Order Summary Div, Third Div--%>
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
                                        <%--Start gridview control part--%>
                                        <asp:GridView ID="gvMainOrderDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvMainOrderDetail_RowDataBound"
                                            OnRowCommand="gvMainOrderDetail_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Ordered<br />
                                                        </span>
                                                        <div class="corner">
                                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnSupplierId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"SupplierId") %>' />
                                                        <asp:HiddenField ID="hdnMyShoppingCartiD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"MyShoppingCartiD") %>' />
                                                        <asp:Label ID="txtQtyOrder" CssClass="first" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                        <div class="corner">
                                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>
                                                        <span>Remaining<br />
                                                        </span>
                                                    </HeaderTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtQtyOrderRemaining" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Shipped<br />
                                                        </span>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtQtyShipped" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Item #<br />
                                                        </span>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                                        <asp:Label runat="server" ID="lblProductItemID" Text='<%# Eval("ProductItemID") %>' Visible="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemStyle CssClass="g_box" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Color<br />
                                                        </span>
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
                                                        <span>Size</span>
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
                                                        <span>Description</span>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProductionDescription" Text='<% #Eval("ProductDescrption")  %>'
                                                            ToolTip='<% #Eval("ProductDescrption")  %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemStyle CssClass="b_box" Width="15%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Price</span>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrice" Text=' <%# "$" + Convert.ToDecimal(Eval("Price")).ToString("#,##0.00")%>'
                                                            runat="server"></asp:Label>
                                                            <asp:Label ID="lblMOASPrice" Text=' <%# "$" + Convert.ToDecimal(Eval("MOASItemPrice")).ToString("#,##0.00")%>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemStyle CssClass="g_box centeralign" Width="4%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Extended</span>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExtendedPrice" Text='<%# "$" + Convert.ToDecimal((Convert.ToDecimal(Eval("Price")) * Convert.ToDecimal(Eval("Quantity")))).ToString("#,##0.00")%>'
                                                            runat="server"></asp:Label>
                                                            <asp:Label ID="lblMOASExtendedPrice" Text='<%# "$" + Convert.ToDecimal((Convert.ToDecimal(Eval("MOASItemPrice")) * Convert.ToDecimal(Eval("Quantity")))).ToString("#,##0.00")%>'
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Edit</span>
                                                    </HeaderTemplate>
                                                    <HeaderStyle CssClass="centeralign" Width="4%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="hypEditShippedItem" CommandName="EditOrderItem" CommandArgument='<%#Eval("MyShoppingCartiD")%>'
                                                            runat="server"><span>Edit</span></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Delete</span>
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
                                        <%--End gridview control part--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="text-align: right; padding-right: 28px;">
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
                                                    <asp:Label ID="lblStrikeIronSalesTax" runat="server" Visible="false"></asp:Label>
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
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="spacer15">
                        </div>
                        <%--Special Order Instruction Div--%>
                        <div class="black_middle order_detail_pad" id="CompanyNoteHistory" runat="server">
                            <%--Popup Notes--%>
                            <div class="form_table">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box taxt_area clearfix" style="height: 100px">
                                    <span class="input_label alignleft" style="height: 100px">Order Special Instruction</span>
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
                            <%--End Note History--%>
                        </div>
                        <div class="spacer15">
                        </div>
                        <div class="botbtn centeralign">
                            <asp:UpdateProgress runat="server" ID="uprogressPGrid" DisplayAfter="1" AssociatedUpdatePanelID="upPanel">
                                <ProgressTemplate>
                                    <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
                                    </div>
                                    <div class="updateProgressDiv">
                                        <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ImageButton runat="server" ID="imgbtnApproveOrder" OnClick="imgbtnApproveOrder_Click"
                                        ImageUrl="~/Images/approve_order_btn.png" />&nbsp;&nbsp;
                                    <asp:ImageButton runat="server" ID="imgbtnCancelOrder" ImageUrl="~/Images/cancel_order_btn.png" />
                                    <at:ModalPopupExtender ID="mpeCancelOrder" TargetControlID="imgbtnCancelOrder" BackgroundCssClass="modalBackground"
                                        DropShadow="true" runat="server" PopupControlID="pnlNotesOrder" CancelControlID="closecancelpopup">
                                    </at:ModalPopupExtender>
                                    <asp:Panel ID="pnlNotesOrder" runat="server" Style="display: none; left: -200px;
                                        top: -100px;">
                                        <div class="pp_pic_holder facebook" style="display: block; width: 411px;">
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
                                                        <div class="pp_content" style="height: 228px; display: block;">
                                                            <div class="pp_fade" style="display: block;">
                                                                <div id="Div1">
                                                                    <div class="pp_inline clearfix">
                                                                        <div class="form_popup_box">
                                                                            <div class="label_bar">
                                                                                <span>Reason for Cancelling:
                                                                                    <br />
                                                                                    <br />
                                                                                    <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtCancelReason"
                                                                                        runat="server"></asp:TextBox></span>
                                                                            </div>
                                                                            <div>
                                                                                <asp:LinkButton ID="lnkbtnCancelNow" CssClass="grey2_btn" runat="server" OnClick="lnkbtnCancelNow_Click"><span>Cancel Now</span></asp:LinkButton>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="pp_details clearfix" style="width: 371px;">
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
    <input type="hidden" id="hdnOrderType" value="" runat="server" />
</asp:Content>
