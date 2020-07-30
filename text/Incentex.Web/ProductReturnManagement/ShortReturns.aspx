<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ShortReturns.aspx.cs" Inherits="ProductReturnManagement_ShortReturns"
    Title="Short Return" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
        .valCSS
        {
            display: block;
            line-height: 14px;
            font-size: 11px;
            font-style: italic;
        }
        .order_detail label
        {
            color: gray !important;
            padding-left: 20px;
            width: 130px;
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
        .packaging_pos .custom_sel_name span.error, .card_details dd span.error, .form_box span.error
        {
            color: red;
            display: block;
            font-style: italic;
            text-align: left;
            font-size: 11px;
            line-height: 14px;
            margin-bottom: -5px;
        }
        .form_box .custom-sel span.error
        {
            padding: 24px 0px;
        }
        span.error
        {
            display: block;
            color: Red;
            font-style: italic;
            text-align: left;
            font-size: 11px;
            line-height: 14px;
        }
        .select_box_pad .black_middle
        {
            float: left;
            padding: 4px 8px;
            width: 98%;
        }
        .select_box_pad .black_top_co
        {
            float: left;
            width: 98.8%;
        }
        .select_box_pad .black_bot_co
        {
            float: left;
            width: 98.8%;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
        // Form Validate Start
        $().ready(function() {

            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules:
                    {
                        ctl00$ContentPlaceHolder1$ddlSizeINC3034: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlSizeINC3032: { NotequalTo: "0" },

                        ctl00$ContentPlaceHolder1$txtQnty: { required: true },

                        ctl00$ContentPlaceHolder1$txtShippingFnameEdit: { required: true },
                        ctl00$ContentPlaceHolder1$txtShippingLnameEdit: { required: true },
                        //ctl00$ContentPlaceHolder1$txtShippingCompanyNameEdit: { required: true },
                        ctl00$ContentPlaceHolder1$txtShippingAddressEdit: { required: true },
                        ctl00$ContentPlaceHolder1$txtEmailAdrressEdit: { required: true },
                        //ctl00$ContentPlaceHolder1$txtPhoneNumberEdit: { required: true },
                        ctl00$ContentPlaceHolder1$txtZipcodeEdit: { required: true },

                        ctl00$ContentPlaceHolder1$drpCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$drpState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$drpCity: { NotequalTo: "0" }


                    },
                    messages:
                    {
                        ctl00$ContentPlaceHolder1$ddlSizeINC3034: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Size")
                        },
                        ctl00$ContentPlaceHolder1$ddlSizeINC3032: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Size")
                        },

                        ctl00$ContentPlaceHolder1$txtQnty: { required: replaceMessageString(objValMsg, "Required", "Quantity") },
                        ctl00$ContentPlaceHolder1$txtShippingFnameEdit: { required: replaceMessageString(objValMsg, "Required", "First name") },
                        ctl00$ContentPlaceHolder1$txtShippingLnameEdit: { required: replaceMessageString(objValMsg, "Required", "Last name") },
                        ctl00$ContentPlaceHolder1$txtShippingAddressEdit: { required: replaceMessageString(objValMsg, "Required", "Shipping Address") },
                        //ctl00$ContentPlaceHolder1$txtShippingCompanyNameEdit: { required: replaceMessageString(objValMsg, "Required", "Company Name") },
                        ctl00$ContentPlaceHolder1$txtEmailAdrressEdit: { required: replaceMessageString(objValMsg, "Required", "Email Adrress") },
                        //ctl00$ContentPlaceHolder1$txtPhoneNumberEdit: { required: replaceMessageString(objValMsg, "Required", "Phone Number") },
                        ctl00$ContentPlaceHolder1$txtZipcodeEdit: { required: replaceMessageString(objValMsg, "Required", "Zipcode") },

                        ctl00$ContentPlaceHolder1$drpCountry: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "-select country-")
                        },
                        ctl00$ContentPlaceHolder1$drpState: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "-select state-")
                        },

                        ctl00$ContentPlaceHolder1$drpCity: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "-select city-")
                        }
                    }
                });

            });
            // Trigger Validation
            $("#<%=btnProcessNow.ClientID %>").click(function() {
                var txtQuantityINC3034 = $("#<%=txtQuantityINC3034.ClientID%>").val();
                var txtQuantityINC3032 = $("#<%=txtQuantityINC3032.ClientID%>").val();

                if (txtQuantityINC3034 != "") {
                    $("#ctl00_ContentPlaceHolder1_ddlSizeINC3034").rules("add", {
                        NotequalTo: "0",
                        messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "size") }
                    });
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_ddlSizeINC3034").rules("remove");
                }
                if (txtQuantityINC3032 != "") {
                    $("#ctl00_ContentPlaceHolder1_ddlSizeINC3032").rules("add", {
                        NotequalTo: "0",
                        messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "size") }
                    });
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_ddlSizeINC3032").rules("remove");
                }

                return $('#aspnetForm').valid();
            });
        });                  // form validate end

    </script>

    <link media="screen" rel="stylesheet" href="../CSS/colorbox.css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div>
            <asp:Panel runat="server" ID="pnlShortReturn">
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle">
                    <div class="clearfix header_bg">
                        <div class="header_bgr">
                            <span class="title alignleft">Shorts Return </span>
                        </div>
                    </div>
                    <div class="spacer15">
                    </div>
                    <div class="shipping_billing_title" style="float: left; color: #72757C; margin-left: 8px;">
                        Please enter in the box below the number of shorts being returned up to three pair
                        of shorts.
                    </div>
                    <div class="spacer20">
                    </div>
                    <div style="width: 49%;">
                        <table class="shoppingcart_box" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <div class="cart_header" runat="server" id="Div1">
                                        <div class="left_co">
                                        </div>
                                        <div class="right_co">
                                        </div>
                                        <table cellpadding="0" cellspacing="0" class="txt13">
                                            <tr>
                                                <td style="width: 32%;" class="leftalign">
                                                    Quantity
                                                </td>
                                                <td style="width: 68%;" class="leftalign">
                                                    Description
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 32%;" class="leftalign">
                                                    <asp:TextBox runat="server" Width="30px" ID="txtQnty" Style="background-color: #303030;
                                                        border: medium none; color: #FFFFFF; padding: 2px; text-align: center; font-size: 12px;"></asp:TextBox>
                                                    <asp:RangeValidator ID="valRngtxtQnty" runat="server" ErrorMessage="Please enter number between 1-3"
                                                        ControlToValidate="txtQnty" MaximumValue="3" MinimumValue="1" CssClass="valCSS"></asp:RangeValidator>
                                                </td>
                                                <td style="width: 68%;" class="leftalign">
                                                    <asp:Label ID="lblShortReturnDescriptions" runat="server" Style="color: #72757C;
                                                        padding: 2px; font-size: 13px; text-align: center;" Text="Female Shorts Returns"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
            </asp:Panel>
        </div>
        <div class="error" id="dvError" runat="server">
            <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="#FF3300" />
            <div class="spacer20">
            </div>
        </div>
        <div>
            <asp:Panel runat="server" ID="pnlExchange">
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle">
                    <div class="clearfix header_bg">
                        <div class="header_bgr">
                            <span class="title alignleft">Exchange For </span>
                        </div>
                    </div>
                    <div class="spacer20">
                    </div>
                    <div style="width: 90%;">
                        <table class="shoppingcart_box" cellpadding="0" cellspacing="0">
                            <tr>
                                <td colspan="7">
                                    <div class="cart_header" runat="server" id="dvCartItemHeader">
                                        <div class="left_co">
                                        </div>
                                        <div class="right_co">
                                        </div>
                                        <table cellpadding="0" cellspacing="0" class="txt13">
                                            <tr>
                                                <td style="width: 30%;" class="leftalign">
                                                </td>
                                                <td style="width: 30%;" class="leftalign">
                                                    Product Description
                                                </td>
                                                <td style="width: 20%;" class="leftalign">
                                                    Quantity
                                                </td>
                                                <td style="width: 20%;" class="leftalign">
                                                    Size
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" class="spacer10">
                                </td>
                            </tr>
                            <tr class="txt14">
                                <td style="width: 30%; vertical-align: top;">
                                    <div class="alignleft">
                                        <div class="agent_img">
                                            <p class="upload_photo gallery">
                                                <img id="imgINC3034" src="~/UploadedImages/employeePhoto/employee-photo.gif" height="120"
                                                    width="100" runat="server" alt='Loading' />
                                            </p>
                                        </div>
                                    </div>
                                </td>
                                <td style="width: 30%;" class="leftalign" colspan="2">
                                    <asp:Label ID="lblProductDescriptionINC3034" runat="server"></asp:Label>
                                </td>
                                <td style="width: 20%;" class="leftalign">
                                    <asp:TextBox runat="server" Width="30px" ID="txtQuantityINC3034" Style="background-color: #303030;
                                        border: medium none; color: #FFFFFF; padding: 2px; text-align: center; font-size: 12px;"></asp:TextBox>
                                    <asp:RangeValidator ID="valRngtxtQuantityINC3034" runat="server" ErrorMessage="Please enter number between 1-3"
                                        ControlToValidate="txtQuantityINC3034" MaximumValue="3" MinimumValue="1" CssClass="valCSS"></asp:RangeValidator>
                                </td>
                                <td style="width: 20%;" class="leftalign">
                                    <asp:DropDownList ID="ddlSizeINC3034" Style="background-color: silver;" runat="server"
                                        AutoPostBack="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" class="spacer10">
                                </td>
                            </tr>
                            <tr class="txt14">
                                <td style="width: 30%; vertical-align: top;">
                                    <div class="alignleft">
                                        <asp:HyperLink ID="lnkINC3034" Target="_blank" CssClass="btn_pdf2" Visible="true"
                                            runat="server"><span>Women Skirt</span></asp:HyperLink>
                                    </div>
                                </td>
                                <td colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" class="spacer10">
                                </td>
                            </tr>
                            <tr class="txt14">
                                <td style="width: 30%; vertical-align: top;">
                                    <div class="alignleft">
                                        <div class="agent_img">
                                            <p class="upload_photo gallery">
                                                <img id="imgINC3032" src="~/UploadedImages/employeePhoto/employee-photo.gif" height="120"
                                                    width="100" runat="server" alt='Loading' />
                                            </p>
                                        </div>
                                    </div>
                                </td>
                                <td style="width: 30%;" class="leftalign" colspan="2">
                                    <asp:Label ID="lblDescriptionINC3032" runat="server"></asp:Label>
                                </td>
                                <td style="width: 20%;" class="leftalign">
                                    <asp:TextBox runat="server" Width="30px" ID="txtQuantityINC3032" Style="background-color: #303030;
                                        border: medium none; color: #FFFFFF; padding: 2px; text-align: center; font-size: 12px;"></asp:TextBox>
                                    <asp:RangeValidator ID="ValRngtxtQuantityINC3032" runat="server" ErrorMessage="Please enter number between 1-3"
                                        ControlToValidate="txtQuantityINC3032" MaximumValue="3" MinimumValue="1" CssClass="valCSS"></asp:RangeValidator>
                                </td>
                                <td style="width: 20%;" class="leftalign">
                                    <asp:DropDownList Style="background-color: silver;" ID="ddlSizeINC3032" runat="server"
                                        AutoPostBack="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" class="spacer10">
                                </td>
                            </tr>
                            <tr class="txt14">
                                <td style="width: 30%; vertical-align: top;">
                                    <div class="alignleft">
                                        <asp:HyperLink ID="lnkINC3032" Target="_blank" CssClass="btn_pdf2" Visible="true"
                                            runat="server"><span>Women Pants</span></asp:HyperLink>
                                    </div>
                                </td>
                                <td colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" class="spacer10">
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
            </asp:Panel>
        </div>
        <div class="spacer15">
        </div>
        <div class="shipping_billing_title" style="float: left;">
            <h2>
                Order Shipping Information</h2>
        </div>
        <div class="select_payment_step">
            <!-- shipping_information start-->
            <div class="select_box_pad" style="margin: 0; padding: 0; width: 100%;">
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle">
                    <!-- shipping_location start -->
                    <div class="alignleft" style="width: 315px; margin-bottom: 15px;">
                        <div class="shipping_billing_title" style="float: left; color: #72757C; margin-left: 8px;
                            width: 750px;">
                            Please choose the shipping location you would like your return shipped to from the
                            list below or you may add a new shipping address.
                        </div>
                        <div class="spacer20">
                        </div>
                        <table class="form_table">
                            <tr>
                                <td class="label_text" style="width: 86px;">
                                    <label>
                                        Shipping
                                        <br />
                                        Location (s):
                                    </label>
                                </td>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box select_pad">
                                        <span class="custom-sel">
                                            <asp:DropDownList ID="ddlShippingLocations" DataTextField="Title" DataValueField="CompanyContactInfoID"
                                                OnSelectedIndexChanged="ddlShippingLocations_SelectedIndexChanged" AutoPostBack="true"
                                                runat="server">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!-- shipping_location end -->
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
                                    <asp:TextBox ID="txtShippingFnameEdit" CssClass="w_label" runat="server"></asp:TextBox></div>
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
                                    <asp:TextBox ID="txtShippingLnameEdit" class="w_label" runat="server"></asp:TextBox></div>
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
                                    <asp:TextBox ID="txtShippingCompanyNameEdit" CssClass="w_label" runat="server" Text="Spirit Airlines"></asp:TextBox>
                                </div>
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
                                        <asp:TextBox Height="32" ID="txtShippingAddressEdit" TextMode="MultiLine" CssClass="w_label"
                                            runat="server"></asp:TextBox>
                                    </div>
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
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box select_pad">
                                    <span class="custom-sel">
                                        <asp:DropDownList ID="drpCountry" onchange="pageLoad(this,value);" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged"
                                            AutoPostBack="true" runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </td>
                            <td class="label_text">
                                <label>
                                    State:
                                </label>
                            </td>
                            <td class="label_input">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box select_pad">
                                    <span class="custom-sel">
                                        <asp:DropDownList ID="drpState" onchange="pageLoad(this,value);" OnSelectedIndexChanged="drpState_SelectedIndexChanged"
                                            AutoPostBack="true" runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="label_text">
                                <label>
                                    City:
                                </label>
                            </td>
                            <td class="label_input">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box select_pad">
                                    <span class="custom-sel">
                                        <asp:DropDownList ID="drpCity" runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
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
                                    <asp:TextBox ID="txtZipcodeEdit" CssClass="w_label" runat="server"></asp:TextBox></div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </td>
                            <td class="label_text">
                                <label>
                                    Phone:
                                </label>
                            </td>
                            <td class="label_input">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <asp:TextBox CssClass="w_label" ID="txtPhoneNumberEdit" runat="server"></asp:TextBox></span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="label_text">
                                <label>
                                    Email:
                                </label>
                            </td>
                            <td class="label_input">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <asp:TextBox CssClass="w_label" ID="txtEmailAdrressEdit" runat="server"></asp:TextBox></span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </td>
                            <td class="label_text">
                                <label>
                                    Address Name:
                                </label>
                            </td>
                            <td class="label_input">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <asp:TextBox CssClass="w_label" ID="txtAddressTitle" runat="server"></asp:TextBox>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <!-- shipping_information end -->
        </div>
        <div class="spacer20" style="clear: both;">
        </div>
        <table class="order_detail" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:LinkButton ID="btnProcessNow" runat="server" CssClass="grey2_btn alignright"
                        OnClick="btnProcessNow_Click"><span>Process Now</span></asp:LinkButton>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnShippingInfoID" runat="server" />
    </div>
</asp:Content>
