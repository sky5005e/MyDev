<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="BillingAddressSetting.aspx.cs" Inherits="admin_CompanyStore_BillingAddressSetting"
    Title="Billing Address Setting" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        $(window).load(function() {
            $("#dvLoader").hide();
        });
    
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $("#dvLoader").show();
            
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtFirstName: { required: true },
                        ctl00$ContentPlaceHolder1$DrpBillingCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpBillingState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpBillingCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtAddressLine1: { required: true },
                        ctl00$ContentPlaceHolder1$txtZip: { required: true, alphanumeric: true, validzipcode: GetCountryCode($("#ctl00_ContentPlaceHolder1_DrpBillingCountry :selected").text()) },
                        ctl00$ContentPlaceHolder1$txtEmail: { email: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtFirstName: { required: replaceMessageString(objValMsg, "Required", "first name") },
                        ctl00$ContentPlaceHolder1$DrpBillingCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") },
                        ctl00$ContentPlaceHolder1$DrpBillingState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") },
                        ctl00$ContentPlaceHolder1$DrpBillingCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") },
                        ctl00$ContentPlaceHolder1$txtZip: { required: replaceMessageString(objValMsg, "Required", "zipcode") },
                        ctl00$ContentPlaceHolder1$txtAddressLine1: { required: replaceMessageString(objValMsg, "Required", "address") },
                        ctl00$ContentPlaceHolder1$txtEmail: { email: replaceMessageString(objValMsg, "Valid", "email address") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtFirstName")
                            error.insertAfter("#dvFirstName");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtAddressLine1")
                            error.insertAfter("#dvAddressLine1");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$DrpBillingCountry")
                            error.insertAfter("#dvCountry");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$DrpBillingState")
                            error.insertAfter("#dvState");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$DrpBillingCity")
                            error.insertAfter("#dvCity");
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
            
            $("#ctl00_ContentPlaceHolder1_DrpBillingCountry").change(function() {
                $('#dvLoader').show();
            });
            
            $("#ctl00_ContentPlaceHolder1_DrpBillingState").change(function() {
                $('#dvLoader').show();
            });
            
            $(window).scroll(function () {
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());            
        });
    </script>

    <style type="text/css">
        .basic_link .manage_link a, .header_bg
        {
            text-align: left;
        }
        .basic_link img
        {
            margin: 0 4px;
        }
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 15%;
        }
    </style>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
    <div id="dvLoader">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
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
        <div class="divider" style="clear: both;">
        </div>
        <div class="form_table">
            <h4>
                Fixed Corporate Billing Address</h4>
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">First Name</span>
                                <asp:TextBox ID="txtFirstName" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="dvFirstName">
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
                                <span class="input_label">Last Name</span>
                                <asp:TextBox ID="txtLastName" CssClass="w_label" runat="server"></asp:TextBox>
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
                                <span class="input_label">Company</span>
                                <asp:TextBox ID="txtCompany" CssClass="w_label" runat="server"></asp:TextBox>
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
                                <span class="input_label">Address Line 1</span>
                                <asp:TextBox ID="txtAddressLine1" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="dvAddressLine1">
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
                                <span class="input_label">Address Line 2</span>
                                <asp:TextBox ID="txtAddressLine2" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="Div1">
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
                                <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="DrpBillingCountry" runat="server" AutoPostBack="true" CssClass="w_label"
                                        OnSelectedIndexChanged="DrpBillingCountry_SelectedIndexChanged" TabIndex="5">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCountry">
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
                                <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="DrpBillingState" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DrpBillingState_SelectedIndexChanged"
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
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="DrpBillingCity" AutoPostBack="true" runat="server" TabIndex="7">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCity">
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
                                <span class="input_label">Zip</span>
                                <asp:TextBox ID="txtZip" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="dvZip">
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
                                <span class="input_label">Telephone</span>
                                <asp:TextBox ID="txtTelephone" CssClass="w_label" runat="server"></asp:TextBox>
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
                                <span class="input_label">Email</span>
                                <asp:TextBox ID="txtEmail" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="dvEmail">
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
                                <span class="input_label">Mobile</span>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table>
                <tr>
                    <td style="width: 5%; font-size: 13px; color: #72757C;">
                        <b>Note :</b>
                    </td>
                    <td style="width: 95%; font-size: 13px; color: #72757C;">
                        To apply this billing adress on checkout steps, please select "Fixed Corporate Billing
                        By Work Group" from the Store Preference tab.
                    </td>
                </tr>
                <tr>
                    <td style="width: 5%; font-size: 13px; color: #72757C;">
                    </td>
                    <td style="width: 95%; font-size: 13px; color: #72757C;">
                        If the setting selected there is "Capture Billing On Checkout", billing address
                        provided by the user would be captured on the checkout steps.
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer25">
        </div>
        <div class="centeralign">
            <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" OnClick="lnkSave_Click"><span>Save Information</span></asp:LinkButton>
        </div>
    </div>
</asp:Content>
