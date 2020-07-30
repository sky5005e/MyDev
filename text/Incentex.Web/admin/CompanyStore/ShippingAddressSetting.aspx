<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ShippingAddressSetting.aspx.cs" Inherits="admin_CompanyStore_ShippingAddressSetting"
    Title="Shipping Address Setting | World-Link" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .basic_link .manage_link a, .header_bg
        {
            text-align: left;
        }
        .basic_link img
        {
            margin: 0 4px;
        }
    </style>
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>

    <script type="text/javascript" language="javascript">
        $(window).load(function() {
            $("#dvLoader").hide();
        });

        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $("#dvLoader").show();

            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlBaseStation: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlPaymentOption: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpShippingCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpShippingState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpShippingCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtAddress: { required: true },
                        ctl00$ContentPlaceHolder1$txtZip: { required: true, alphanumeric: true, validzipcode: GetCountryCode($("#ctl00_ContentPlaceHolder1_DrpShippingCountry :selected").text()) },
                        ctl00$ContentPlaceHolder1$txtEmail: { email: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlBaseStation: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "base station") },
                        ctl00$ContentPlaceHolder1$ddlPaymentOption: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "payment option") },
                        ctl00$ContentPlaceHolder1$DrpShippingCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") },
                        ctl00$ContentPlaceHolder1$DrpShippingState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") },
                        ctl00$ContentPlaceHolder1$DrpShippingCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") },
                        ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "status") },
                        ctl00$ContentPlaceHolder1$txtZip: {
                            required: replaceMessageString(objValMsg, "Required", "ZipCode"),
                            alphanumeric: replaceMessageString(objValMsg, "Number", ""),
                            validzipcode: replaceMessageString(objValMsg, "ValidZipCode", "")
                        },
                        ctl00$ContentPlaceHolder1$txtAddress: { required: replaceMessageString(objValMsg, "Required", "address") },
                        ctl00$ContentPlaceHolder1$txtEmail: { email: replaceMessageString(objValMsg, "Valid", "email address")}
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtAddress")
                            error.insertAfter("#dvAddress");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlBaseStation")
                            error.insertAfter("#dvBaseStation");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlPaymentOption")
                            error.insertAfter("#dvPaymentOption");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$DrpShippingCountry")
                            error.insertAfter("#dvCountry");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$DrpShippingState")
                            error.insertAfter("#dvState");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$DrpShippingCity")
                            error.insertAfter("#dvCity");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlStatus")
                            error.insertAfter("#dvStatus");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtZip")
                            error.insertAfter("#dvZip");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtEmail")
                            error.insertAfter("#dvEmail");
                        else
                            error.insertAfter(element);
                    }
                });
            });

            $("#ctl00_ContentPlaceHolder1_lnkSave").click(function() {
                if ($('#aspnetForm').valid()) {
                    $('#dvLoader').show();
                    return true;
                }
                else {
                    return false;
                }
            });

            $("#ctl00_ContentPlaceHolder1_DrpShippingCountry").change(function() {
                $('#dvLoader').show();
            });

            $("#ctl00_ContentPlaceHolder1_DrpShippingState").change(function() {
                $('#dvLoader').show();
            });

          
        });
        
        function ScrollToTag(TargetName,IsTargetByClass)
        {
            if(!IsTargetByClass)
            {
                $('html, body').animate({
                scrollTop: $("#" + TargetName).offset().top
                }, 200);
            }
            else
            {
                $('html, body').animate({
                scrollTop: $("." + TargetName).offset().top
                }, 200);
            }
        }
    </script>

    <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 15%;
        }
        .paymentCheckbox td label
        {
            padding: 5px;
            font-size: 11px;
        }
    </style>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div id="dvLoader">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div>
            <div>
                <h3 style="float: left; margin-right: 7px; color: #B0B0B0;">
                    Store :</h3>
                <asp:Label ID="lblStore" runat="server" Style="float: left; color: #72757C; line-height: 23px;
                    font-size: 15px;" />
            </div>
            <div>
                <asp:Label ID="lblWorkGroup" runat="server" Style="float: right; margin-left: 7px;
                    color: #72757C; line-height: 23px; font-size: 15px;" />
                <h3 style="float: right; color: #B0B0B0;">
                    Work Group :</h3>
            </div>
        </div>
        <div class="divider" style="clear: both; height: 10px;">
        </div>
        <div class="divider" style="clear: both; text-align: center; background-position: bottom;
            margin-top: 0px; margin-bottom: 10px; padding-bottom: 10px" id="dvMsg" runat="server"
            visible="false">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="form_table" id="dvfixShipAdressfields">
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Base Station</span><span class="custom-sel label-sel-small"
                                    style="width: 58%;">
                                    <asp:DropDownList ID="ddlBaseStation" runat="server" AutoPostBack="false" CssClass="w_label">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvBaseStation">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="border-right: none;">Payment Option</span>
                                <asp:CheckBoxList ID="chklstPaymentOptions" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                    CssClass="paymentCheckbox" DataMember="iLookupID">
                                </asp:CheckBoxList>
                                <div id="dvPaymentOption">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider" style="clear: both;">
        </div>
        <div class="form_table">
            <h4>
                Fixed Corporate Shipping Address</h4>
            <table class="form_table" >
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Company</span>
                                <asp:TextBox ID="txtCompany" CssClass="w_label" runat="server"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Address</span>
                                <asp:TextBox ID="txtAddress" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="dvAddress">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd_r">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Email</span>
                                <asp:TextBox ID="txtEmail" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="dvEmail">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="DrpShippingCountry" runat="server" AutoPostBack="true" CssClass="w_label"
                                        OnSelectedIndexChanged="DrpShippingCountry_SelectedIndexChanged" TabIndex="5">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCountry">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="DrpShippingState" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DrpShippingState_SelectedIndexChanged"
                                        TabIndex="6">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvState">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd_r">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="DrpShippingCity" runat="server" TabIndex="7" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCity">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Zip</span>
                                <asp:TextBox ID="txtZip" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="dvZip">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Telephone</span>
                                <asp:TextBox ID="txtTelephone" CssClass="w_label" runat="server"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd_r">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Mobile</span>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Status</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="10" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvStatus">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Apply To</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlApplyToIssuancePolicy" runat="server" TabIndex="10" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="Div1">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd_r">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table>
                <tr style="display: none;">
                    <td style="width: 5%; font-size: 13px; color: #72757C;">
                        <b>Note :</b>
                    </td>
                    <td style="width: 95%; font-size: 13px; color: #72757C;">
                        In order to apply this Shipping adress on checkout steps, please select "Fixed Corporate
                        Shipping By Work Group" from the Store Preference tab.
                    </td>
                </tr>
                <tr style="display: none;">
                    <td style="width: 5%; font-size: 13px; color: #72757C;">
                    </td>
                    <td style="width: 95%; font-size: 13px; color: #72757C;">
                        Other wise Shipping address provided by the user would be captured on the checkout
                        steps.
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer25">
        </div>
        <div class="centeralign">
            <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" OnClick="lnkSave_Click"><span>Save Information</span></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lnkReset" runat="server" CssClass="grey2_btn" OnClick="lnkReset_Click"><span>Reset</span></asp:LinkButton>
        </div>
        <div class="spacer25">
        </div>
        <asp:GridView ID="gvShippingAddress" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvShippingAddress_RowCommand">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnBaseStation" runat="server" CommandArgument="BaseStation"
                            CommandName="Sort"><span>Base Station</span></asp:LinkButton>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                    <asp:Label runat="server" ID="lblShippingAddressID" Text='<%# Eval("ShippingAddressID") %>' Visible="false" />
                        <asp:Label runat="server" ID="lblBaseStation" Text='<%# Eval("BaseStation") %> ' />
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="20%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Payment Option</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPaymentOptionType" Text='<%# Eval("PaymentOption") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="38%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                       <span>WorkGroup</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblWorkgroup" Text='<%# lblWorkGroup.Text %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="20%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnStatus" runat="server" CommandArgument="StatusID"
                        CommandName="Sort"><span>Status</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStatusID" Text='<%# Convert.ToInt32(Eval("StatusID")) == 135? "Active" : "InActive" %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>....</span>
                        <div class="corner">
                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span class="btn_space">
                            <asp:LinkButton runat="server" ID="lnkEdit" Text="Edit" CommandName="Modify" CommandArgument='<%# Eval("ShippingAddressID") %>'  />
                            <asp:LinkButton runat="server" ID="lnkDelete" Text="Delete" CommandName="ShipDelete" CommandArgument='<%# Eval("ShippingAddressID") %>'  />
                        </span>
                        <div class="corner">
                            <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="12%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div>
            <div>
                <div class="companylist_botbtn">
                    <div class="alignright pagging" id="dvPager" runat="server">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"></asp:LinkButton>
                        <span>
                            <asp:DataList ID="dtlAddresses" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="dtlAddresses_ItemCommand"
                                OnItemDataBound="dtlAddresses_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList>
                        </span>
                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
