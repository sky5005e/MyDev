<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MyBilling.aspx.cs" Inherits="MyAccount_MySettings_MyBilling" Title="Billing Information" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        $().ready(function() {

            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                 $("#aspnetForm").validate({
                    rules: {
                       
                        //billing info
                        ctl00$ContentPlaceHolder1$DrpBillingCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpBillingState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpBillingCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$TxtBillingAddressLine1: { required: true },
                        ctl00$ContentPlaceHolder1$TxtEmail: { email: true },
                        ctl00$ContentPlaceHolder1$TxtBillingZip: { required: true, alphanumeric: true, validzipcode: GetCountryCode($("#ctl00_ContentPlaceHolder1_DrpBillingCountry :selected").text()) }

                       

                    },
                    messages: {
                        

                        //billing info
                        ctl00$ContentPlaceHolder1$DrpBillingCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") },
                        ctl00$ContentPlaceHolder1$DrpBillingState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") },
                        ctl00$ContentPlaceHolder1$DrpBillingCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") },

                        ctl00$ContentPlaceHolder1$TxtBillingAddressLine1: { required: replaceMessageString(objValMsg, "Required", "Address") },
                         ctl00$ContentPlaceHolder1$TxtEmail: { required: replaceMessageString(objValMsg, "Required", "Email")
                                 , email: replaceMessageString(objValMsg, "Valid", "email address")
                        },

                      ctl00$ContentPlaceHolder1$TxtBillingZip: {
                            required: replaceMessageString(objValMsg, "Required", "zip"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        }
                    }
                }); // form validate

           
            $("#<%=LnkAppyChanges.ClientID %>").click(function() {
                if ($("#<%=DrpBillingCity.ClientID %> option:selected").attr('value') == "-Other-") {                    
                    $("#ctl00_ContentPlaceHolder1_txtCity").rules("add", {
                            required: true,
                            messages: {
                                required: replaceMessageString(objValMsg, "Required", "city name")
                            }
                    });
                }
                else {                    
                    $("#ctl00_ContentPlaceHolder1_txtCity").rules("remove");
                }
            
                return $('#aspnetForm').valid();            
            });            

          });
    
        $(window).scroll(function () {              
          $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
        });
        
        $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val()); 

        });         //ready
    </script>

    <script type="text/javascript" language="javascript">
function pageLoad(sender, args) {
            {

                assigndesign();
            }
        }
    </script>

    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad select_box_pad" style="width: 400px">
        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <div class="form_table">
            <table>
                <tr>
                    <td>
                        <table>
                            <tr id="dvMinShippingAmount" runat="server">
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Company</span>
                                            <asp:TextBox ID="TxtBillingCompanyName" runat="server" CssClass="w_label" TabIndex="1"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="dvShippingOfSale" runat="server">
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">First Name</span>
                                            <asp:TextBox ID="TxtCO" runat="server" CssClass="w_label" TabIndex="2"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Last Name</span>
                                            <asp:TextBox ID="TxtBillingManager" TabIndex="3" runat="server" CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Address</span>
                                            <asp:TextBox ID="TxtBillingAddressLine1" runat="server" CssClass="w_label" TabIndex="4"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Address</span>
                                            <asp:TextBox ID="TxtBillingAddressLine2" runat="server" CssClass="w_label" TabIndex="5"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="DrpBillingCountry" runat="server" AutoPostBack="true" CssClass="w_label"
                                                    OnSelectedIndexChanged="DrpBillingCountry_SelectedIndexChanged" TabIndex="6">
                                                </asp:DropDownList>
                                            </span>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="DrpBillingState" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DrpBillingState_SelectedIndexChanged"
                                                    TabIndex="7">
                                                </asp:DropDownList>
                                            </span>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="DrpBillingCity" OnSelectedIndexChanged="DrpBillingCity_SelectedIndexChanged"
                                                    AutoPostBack="true" runat="server" TabIndex="8">
                                                </asp:DropDownList>
                                            </span>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="PnlCityOther" runat="server" visible="false">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">City Name</span>
                                            <asp:TextBox ID="txtCity" runat="server" CssClass="w_label" TabIndex="9"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Zip</span>
                                            <asp:TextBox ID="TxtBillingZip" runat="server" CssClass="w_label" TabIndex="10"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Telephone</span>
                                            <asp:TextBox ID="TxtTelephone" runat="server" CssClass="w_label" TabIndex="11"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Mobile</span>
                                            <asp:TextBox ID="TxtMobile" runat="server" CssClass="w_label" TabIndex="12"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Email</span>
                                            <asp:TextBox ID="TxtEmail" runat="server" CssClass="w_label" TabIndex="13"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <div class="centeralign">
                            <asp:LinkButton ID="LnkAppyChanges" class="grey2_btn" runat="server" OnClick="LnkAppyChanges_Click"
                                TabIndex="14"><span>Save</span></asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="spacer10">
            </div>
        </div>
    </div>
</asp:Content>
