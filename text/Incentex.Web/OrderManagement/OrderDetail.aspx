<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderDetail.aspx.cs" Inherits="OrderManagement_OrderDetail" Title="Order Detail"
    ValidateRequest="false" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
        .form_popup_box .label_bar label
        {
            color: #72757C;
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

        function StatusConfirmation(status) {
            if (confirm("Are you sure, you want to change the status of this order to " + status + " ?") == true) {
                __doPostBack("<%=ddlStatus.ClientID %>", '');

                return true;
            }
            else
                return false;
        }


        // check if value is numeric in textbox
        function CheckNum(id) {
            var txt = document.getElementById(id);
            var txt1 = txt.id.split('_');

            var fin = txt1[0] + "_" + txt1[1] + "_" + txt1[2] + "_" + txt1[3] + "_" + txt1[4] + "_" + txt1[5] + "_" + "txtQtyOrderRemaining";

            var rem = document.getElementById(fin);

            if (parseInt(txt.value) > parseInt(rem.innerHTML)) {
                alert("Please enter shipped quantity less then remaining quantity");
                txt.value = parseInt(rem.innerHTML);
                txt.focus();
                return;
            }
            if (txt.value == "0" || txt.value == "") {
                alert("You can not enter 0 as a shipped");
                txt.value = "";
                txt.focus();
                return;
            }

            if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.value = "";
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
    </script>

    <script type="text/javascript">

        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calendar-small.png',
                buttonImageOnly: true
            });
        });

        $(function() {
            var prev_val1;

            $('#ctl00_ContentPlaceHolder1_ddlStatus').focus(function() {
                prev_val1 = $(this).val();
            }).change(function() {
                $(this).blur() // Firefox fix as suggested by AgDude
                var success = confirm('Are you sure, you want to change order status to ' + $('#ctl00_ContentPlaceHolder1_ddlStatus :selected').text() + '?');
                if (success) {
                    //alert('changed');
                    // Other changed code would be here...
                }
                else {
                    $(this).val(prev_val1);
                    //alert('unchanged');
                    return false;
                }
            });
        });
         
    </script>

    <script type="text/javascript" language="javascript">

        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleContainerContent").hide();
            
            $(".saveShipDetails").click(function() {
                $('#dvLoader').show();
            });
        });
   
    </script>

    <script type="text/javascript">
        function print() {
            var oid = document.getElementById('ctl00_ContentPlaceHolder1_hdORderID').value;
            window.open('OrderDetailForPrint.aspx?Id=' + oid, 'printwin', 'left=100,top=100,width=420,height=500');
        }
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript">
        $(window).load(function() {
            $("#dvLoader").hide();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div style="padding-right: 15px;">
            <a id="lnkPrintOrder" onclick="javascript:print();" class="grey2_btn alignright"><span>
                Print Order</span></a>
        </div>
        <div class="pro_search_pad" style="width: 920px;">
            <mb:MenuUserControl ID="menucontrol" runat="server" />
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
                                                <asp:DropDownList runat="server" ID="ddlStatus" Style="background-color: #303030;
                                                    border: medium none; color: #ffffff; width: 150px; padding: 2px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                    <%--onchange="return StatusConfirmation(this.options[this.selectedIndex].text);"--%>
                                                </asp:DropDownList>
                                                <asp:Label runat="server" ID="lblOrderStatus" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                          {%>
                                        <tr>
                                            <td style="font-size: small;">
                                                <label style="padding-left: 28px!important;">
                                                    Payment Method :
                                                </label>
                                                <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <%} %>
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
                                <span style="padding-left: 29px!important;">Ship To:</span>
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
                                                <asp:LinkButton ID="btnEditBillingDummy" class="grey2_btn alignright" runat="server"
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
                                                                                            </span>
                                                                                        </div>
                                                                                        <div class="label_bar">
                                                                                            <label>
                                                                                                State :</label>
                                                                                            <span>
                                                                                                <asp:DropDownList ID="drpState" OnSelectedIndexChanged="drpState_SelectedIndexChanged"
                                                                                                    AutoPostBack="true" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </span>
                                                                                        </div>
                                                                                        <div class="label_bar">
                                                                                            <label>
                                                                                                City :</label>
                                                                                            <span>
                                                                                                <asp:DropDownList ID="drpCity" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </span>
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
                                            <td>
                                                <label class="width300">
                                                    <asp:Label ID="lblSOrderFor" runat="server" />
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
                    <div class="black_bot_co">
                        <span>&nbsp;</span></div>
                    <div class="spacer15">
                    </div>
                    <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                      {%>
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
                                <asp:GridView ID="gvMainOrderDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvMainOrderDetail_RowDataBound"
                                    OnRowCommand="gvMainOrderDetail_RowCommand">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <span>Ordered</span>
                                                <div class="corner">
                                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                </div>
                                            </HeaderTemplate>
                                            <HeaderStyle CssClass="centeralign" />
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnSupplierId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"SupplierId") %>' />
                                                <asp:HiddenField ID="hdnMyShoppingCartiD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"MyShoppingCartiD") %>' />
                                                <asp:Label CssClass="first" ID="txtQtyOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                <div class="corner">
                                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box centeralign" Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <HeaderTemplate>
                                                <span>Remaining</span>
                                            </HeaderTemplate>
                                            <HeaderStyle CssClass="centeralign" />
                                            <ItemTemplate>
                                                <asp:Label ID="txtQtyOrderRemaining" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <span>Shipped</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="txtQtyShipped" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="centeralign" />
                                            <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <span>Item #</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                                <asp:Label runat="server" ID="lblProductItemID" Text='<%# Eval("ProductItemID") %>' Visible="false" />
                                            </ItemTemplate>
                                            <HeaderStyle />
                                            <ItemStyle CssClass="g_box" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <span>Color</span>
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
                                                <asp:Label ID="lblProductionDescription" ToolTip='<% #Eval("ProductDescrption") %>'
                                                    runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle />
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
                                            <ItemStyle CssClass="g_box centeralign" Width="10%" />
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
                                            <ItemStyle CssClass="g_box centeralign" Width="4%" />
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
                    <%} %>
                </div>
                <%--repeater Div--%>
                <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyAdmin))
                  {%>
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle order_detail_pad">
                    <asp:Repeater ID="parentRepeater" runat="server" OnItemDataBound="parentRepeater_ItemDataBound"
                        OnItemCommand="parentRepeater_ItemCommand">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="collapsibleContainer" title="<%# DataBinder.Eval(Container.DataItem, "CompanyName")%>"
                                total="<%# DisplayTotal(Convert.ToString(DataBinder.Eval(Container.DataItem, "Supplierid"))) %>"
                                align="left">
                                <asp:HiddenField ID="hdnSupplierId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Supplierid") %>' />
                                <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                  {%>
                                <table>
                                    <tr>
                                        <td width="50%">
                                            <table class="order_detail" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <label class="fontsizesmall">
                                                            Vendor:
                                                        </label>
                                                        <asp:Label CssClass="fontsizesmall" runat="server" ID="lblSupplierCompanyName" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName")%>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </label>
                                                        <asp:Label runat="server" CssClass="fontsizesmall" ID="lblVendorAdress1" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyAdddress")%>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%--style="padding-left: 68px;"--%>
                                                        <label>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </label>
                                                        <asp:Label runat="server" CssClass="fontsizesmall" ID="lblVendorCity" Text='<%# DataBinder.Eval(Container.DataItem, "CityName")%>'> </asp:Label>
                                                        ,
                                                        <asp:Label runat="server" ID="lblStateName" Text='<%# DataBinder.Eval(Container.DataItem, "sStatename")%>'
                                                            CssClass="fontsizesmall"></asp:Label>
                                                        <asp:Label runat="server" ID="lblCompanyZip" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyZip")%>'
                                                            CssClass="fontsizesmall"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="50%">
                                            <table class="order_detail">
                                                <tr>
                                                    <td style="font-size: small;">
                                                        <label>
                                                            Ship To Address :
                                                        </label>
                                                        <asp:Label ID="lblShipContaName" runat="server"></asp:Label>
                                                        <%--Text='<%#  DataBinder.Eval(Container.DataItem, "ShippingName") %>'--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="padding-right: 25px;">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </label>
                                                        <asp:Label ID="lblShipToAddress1" runat="server" CssClass="fontsizesmall"></asp:Label>
                                                        <%--Text='<%#  DataBinder.Eval(Container.DataItem, "ShippAddress") %>'--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="padding-right: 25px;">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </label>
                                                        <asp:Label ID="lblShipToAddress2" CssClass="fontsizesmall" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="padding-right: 25px;">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </label>
                                                        <asp:Label ID="lblShipToAddress3" CssClass="fontsizesmall" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="padding-right: 25px;">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </label>
                                                        <asp:Label ID="lblShipToStreet" CssClass="fontsizesmall" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="padding-right: 25px;">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </label>
                                                        <asp:Label ID="lblShipZipCode" CssClass="fontsizesmall" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label style="padding-right: 25px;">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </label>
                                                        <asp:Label ID="lblCountryName" CssClass="fontsizesmall" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <table class="order_detail">
                                                <tr>
                                                    <td>
                                                        <label class="fontsizesmall">
                                                            Contact:
                                                        </label>
                                                        <asp:Label runat="server" ID="lblContactVendor" CssClass="fontsizesmall" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>'></asp:Label>
                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Supplierid") %>' />
                                                    </td>
                                                </tr>
                                                <tr id="trPAASOrderNumber" runat="server">
                                                    <td>
                                                        <label class="fontsizesmall">
                                                            PAAS Order # :
                                                        </label>
                                                        <asp:Label runat="server" ID="lblPAASOrderNumber" CssClass="fontsizesmall"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="fontsizesmall">
                                                            Account # :
                                                        </label>
                                                        <asp:Label runat="server" ID="lblAccountNo" CssClass="fontsizesmall" Text='<%# DataBinder.Eval(Container.DataItem, "BankAccountNumber")%>'></asp:Label>
                                                        <asp:HiddenField ID="hdnMyShoppingCartId" runat="server" />
                                                        <asp:HiddenField ID="hdnOrderFor" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="fontsizesmall">
                                                            Tel :
                                                        </label>
                                                        <asp:Label runat="server" ID="lblSupplierTelephone" CssClass="fontsizesmall" Text='<%# DataBinder.Eval(Container.DataItem, "Telephone")%>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label class="fontsizesmall">
                                                            Email :
                                                        </label>
                                                        <asp:Label runat="server" ID="lblContactEmail" CssClass="fontsizesmall" Text='<%# DataBinder.Eval(Container.DataItem, "ContactEmail")%>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <%} %>
                                <table>
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView ID="gvOrderDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvOrderDetail_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Item #</span>
                                                            <div class="corner">
                                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <HeaderStyle />
                                                        <ItemTemplate>
                                                            <asp:Label CssClass="first" runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                                            <div class="corner">
                                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                            </div>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box" Width="12%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Ordered</span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hdnMyShoppingCartiD" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"MyShoppingCartiD") %>' />
                                                            <asp:Label ID="txtQtyOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Remaining</span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtQtyOrderRemaining" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Shipped</span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAlreadyShipped" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Shipping</span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <span class="btn_space">
                                                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                                                    color: #ffffff; width: 50px; padding: 2px; text-align: center;" onchange="CheckNum(this.id)"
                                                                    MaxLength="10" BackColor="#303030" ID="txtQtyShipped"></asp:TextBox>
                                                            </span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Color</span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <span class="btn_space">
                                                                <asp:Image ID="imgColor" runat="server" Height="20" Width="20" />
                                                            </span>
                                                            <asp:Label runat="server" ID="lblColor" Text='<%# DataBinder.Eval(Container.DataItem,"Color") %>'
                                                                Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" Width="6%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Size</span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSize" ToolTip='<%# DataBinder.Eval(Container.DataItem,"Size") %>'
                                                                Text='<%#(Eval("Size").ToString().Length > 5) ? Eval("Size").ToString().Substring(0, 5) + "..." : Eval("Size")%>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Description</span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductionDescription" Text='<%#(Eval("ProductDescrption").ToString().Length > 10) ? Eval("ProductDescrption").ToString().Substring(0,10)+"..." : Eval("ProductDescrption")  %>'
                                                                ToolTip='<% #Eval("ProductDescrption")  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box" Width="9%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Backordered Until </span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle />
                                                        <ItemTemplate>
                                                            <span class="calender_l"">
                                                                <asp:TextBox ID="txtBackOrderDate" Style="background-color: #303030; border: medium none;
                                                                    color: #ffffff; width: 80px; padding: 2px" runat="server" CssClass="cal_w datepicker1 min_w"
                                                                    Text='<%# Convert.ToString(Eval("BackOrderedUntil")) != "" ? Convert.ToDateTime(Eval("BackOrderedUntil")).ToString("MM/dd/yyyy") : "" %>'></asp:TextBox>
                                                                <asp:HiddenField runat="server" ID="hdnBackOrderedUntil" Value='<%# Convert.ToString(Eval("BackOrderedUntil")) != "" ? Convert.ToDateTime(Eval("BackOrderedUntil")).ToString("MM/dd/yyyy") : "" %>' />
                                                            </span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Edit</span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="hypEditShippmentOrder" CommandName="EditShippingOrder" runat="server"><span>Edit</span></asp:HyperLink>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" Width="4%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div class="spacer20">
                                </div>
                                <table>
                                    <tr>
                                        <td colspan="3">
                                            <div class="botbtn centeralign">
                                                <asp:LinkButton ID="lnkSaveOrderDetails" CommandName="OrderDetails" class="grey2_btn saveShipDetails"
                                                    runat="server" ><span>Save</span></asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="spacer20">
                                </div>
                                <table>
                                    <tr>
                                        <td colspan="3">
                                            <div class="form_table">
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box taxt_area clearfix" style="height: 100px">
                                                    <span class="input_label alignleft" style="height: 98px">
                                                        <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                                          {%>
                                                        Supplier Notes/History :
                                                        <%}
                                                          else
                                                          { %>
                                                        Incentex Notes/History :
                                                        <%} %>
                                                    </span>
                                                    <div class="textarea_box alignright">
                                                        <div class="scrollbar" style="height: 99px">
                                                            <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                                            </a>
                                                        </div>
                                                        <asp:TextBox ID="txtNotesHistory" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                                            ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                            <div class="alignnone spacer15">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <div class="rightalign gallery">
                                                <asp:LinkButton ID="lnkDummyAddHisotry" class="grey2_btn alignright" runat="server"
                                                    Style="display: none"></asp:LinkButton>
                                                <asp:LinkButton ID="lnkAddHisotry" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright"><span>+ Add Note</span></asp:LinkButton>
                                                <at:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="lnkAddHisotry" BackgroundCssClass="modalBackground"
                                                    DropShadow="true" runat="server" PopupControlID="pnlOrderHistory" CancelControlID="closepopuphistory">
                                                </at:ModalPopupExtender>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlOrderHistory" runat="server" Style="display: none;">
                                                <div class="pp_pic_holder facebook" style="display: block; width: 411px; left: 35%;
                                                    top: 30%; position: fixed;">
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
                                                                        <div id="Div2">
                                                                            <div class="pp_inline clearfix">
                                                                                <div class="form_popup_box">
                                                                                    <div class="label_bar">
                                                                                        <span>
                                                                                            <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                                                                              {%>
                                                                                            Supplier Notes/History :
                                                                                            <%}
                                                                                              else
                                                                                              { %>
                                                                                            Incentex Notes/History :
                                                                                            <%} %>
                                                                                            <br />
                                                                                            <br />
                                                                                            <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtOrderNotesHistory"
                                                                                                runat="server"></asp:TextBox></span>
                                                                                    </div>
                                                                                    <div>
                                                                                        <asp:LinkButton ID="lnkButtonSaveHistory" CommandName="SAVECACE" class="grey2_btn"
                                                                                            runat="server" OnClick="lnkButtonSaveHistory_Click"><span>Save Notes</span></asp:LinkButton>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="pp_details clearfix" style="width: 371px;">
                                                                            <a href="#" id="closepopuphistory" runat="server" class="pp_close">Close</a>
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
                            <div class="spacer15">
                            </div>
                            <%--<hr />--%>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
                <%} %>
                <div class="spacer15">
                </div>
                <%--Special Order Instruction Div--%>
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
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
                
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
                    <div runat="server" id="dvNameBarFile">
                    <br />&nbsp; <span>Namebar Bulk Upload File: </span>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">                   
                     </asp:PlaceHolder>
                    </div>
            </div>
        </div>
    </div>
    <input type="hidden" id="hdnOrderType" value="" runat="server" />
</asp:Content>
