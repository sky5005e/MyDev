<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ShoppingSetting.aspx.cs" Inherits="admin_Company_Employee_ShoppingSetting"
    Title="Company Employee >> Shopping Setting" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .MOASbillinginfo span
        {
            margin-right: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function checkpaymentoption() {
            var val = [];
            var returnVal = true;
            $('#ctl00_ContentPlaceHolder1_dtPaymentOptions :checkbox:checked').each(function(i) {

                val[i] = $(this).val();
            });

            if (val.length <= 0) {

                alert("Please select at least one payment option");
                returnVal = false;

            }

            if ($('#ctl00_ContentPlaceHolder1_dvMOASEmail').css('display') != 'none') {
                var MOASval = [];
                $('#ctl00_ContentPlaceHolder1_dtCompanyAdmin :checkbox:checked').each(function(i) {
                    MOASval[i] = $(this).val();
                });

                if (MOASval.length <= 0) {
                    alert("Please select at least one MOAS user.");
                    returnVal = false;
                }
                $(".allowValue").each(function(i) {
                    if ($(this).css('display') != 'none' && $(this).val() == '') {
                        alert("Please enter priority.");
                        $(this).focus();
                        returnVal = false;
                    }
                });

            }
            if (returnVal == false)
                return false;
            else
                return true;
        }
       
    </script>

    <script language="javascript" type="text/javascript">
        function TogleMOASPanel(name) {
            if (name.indexOf("MOAS") >= 0) {
                $("#ctl00_ContentPlaceHolder1_dvMOASEmail").toggle();
                $("#ctl00_ContentPlaceHolder1_dvReqChkoutInfo").toggle();
            }
        }

        function VisiblePriority(name) {
            var parent = getParentByTagName(name, "td");
            var textBoxes = parent.getElementsByTagName("input");
            for (var i = 0; i < textBoxes.length; i++) {
                if (textBoxes[i].type == "text") {
                    if (name.checked) {
                        var Mainparent = getParentByTagName(name, "table");
                        var checkBoxes = Mainparent.getElementsByTagName("input");
                        var countCheckedCheckBoxes = 0;
                        for (var j = 0; j < checkBoxes.length; j++) {
                            if (checkBoxes[j].type == "checkbox" && checkBoxes[j].checked == true) {
                                countCheckedCheckBoxes = countCheckedCheckBoxes + 1;
                            }
                        }
                        $(textBoxes[i]).show();
                        $(textBoxes[i]).val(countCheckedCheckBoxes);
                    }
                    else {
                        $(textBoxes[i]).hide();
                        $(textBoxes[i]).val('');
                    }
                }
            }
        }
        function getParentByTagName(obj, tag) {
            var obj_parent = obj.parentNode;
            if (!obj_parent) return false;
            if (obj_parent.tagName.toLowerCase() == tag) return obj_parent;
            else return getParentByTagName(obj_parent, tag);
        }

        function isNumberkey(evt) {
            var charcode = (evt.which) ? evt.which : event.keycode
            if (charcode > 31 && (charcode < 48 || charcode > 57))
                return false;

            return true;
        }
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div class="spacer10">
    </div>
    <div class="tabcontent" id="shopping_settings">
        <div class="alignnone">
            &nbsp;</div>
        <div class="form_pad">
            <div class="employee_name">
                User Name:
                <asp:Label ID="lblUserFullName" runat="server" Text=""></asp:Label>
            </div>
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div>
                <table>
                    <tr>
                        <td style="width: 35%">
                            <h4>
                                User Store Options</h4>
                            <div class="form_table">
                                <table>
                                    <tr>
                                        <td class="formtd">
                                            <table class="checktable_supplier true">
                                                <tr>
                                                    <td>
                                                        <asp:DataList ID="dtUserStoreFront" runat="server">
                                                            <ItemTemplate>
                                                                <span class="custom-checkbox alignleft" id="storespan" runat="server">
                                                                    <asp:CheckBox ID="chkUserStoreFront" runat="server" />
                                                                </span>
                                                                <label>
                                                                    <asp:Label ID="lblUserStoreFront" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                                                <asp:HiddenField ID="hdnStoreFront" runat="server" Value='<%#Eval("iLookupID")%>' />
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td style="width: 65%" id="tdSupportTicketOwners" runat="server">
                            <h4>
                                Support Ticket Owners</h4>
                            <div class="form_table">
                                <table>
                                    <tr>
                                        <td class="formtd">
                                            <table class="checktable_supplier true">
                                                <tr>
                                                    <td>
                                                        <asp:DataList ID="dtSupportTicketOwners" OnItemDataBound="dtSupportTicketOwners_ItemDataBound"
                                                            runat="server" RepeatColumns="3" RepeatDirection ="Vertical">
                                                            <ItemTemplate>
                                                                <span class="custom-checkbox alignleft" id="ownerspan" runat="server">
                                                                    <asp:CheckBox ID="chkSupportTicketOwner" runat="server" />
                                                                </span>
                                                                <label>
                                                                    <asp:Label ID="lblSupportTicketOwner" Text='<%# Eval("FirstName") + " " + Eval("LastName")  %>'
                                                                        runat="server"></asp:Label></label>
                                                                <asp:HiddenField ID="hdnSupportTicketOwner" runat="server" Value='<%#Eval("UserInfoID")%>' />
                                                                <asp:HiddenField ID="hdnExisting" runat="server" Value='<%#Eval("Existing")%>' />
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="divider">
            </div>
            <h4>
                World-Link System Payment Options</h4>
            <div class="form_table clearfix" style="width: 100%;">
                <div class="alignleft" style="width: 100%;">
                    <table class="checktable_supplier true">
                        <tr>
                            <td>
                                <asp:DataList ID="dtPaymentOptions" runat="server">
                                    <ItemTemplate>
                                        <span class="custom-checkbox alignleft" id="paymentspan" runat="server">
                                            <asp:CheckBox ID="chkPaymentOptions" onclick='<%# string.Format("TogleMOASPanel(\"{0}\")",Eval("sLookupName")) %>'
                                                runat="server" />
                                        </span>
                                        <label>
                                            <asp:Label ID="lblPaymentOptions" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                        <asp:HiddenField ID="hdnPaymentOption" runat="server" Value='<%#Eval("iLookupID")%>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="dvMOASEmail" runat="server" style="display: none; width: 100%;">
                                    <h4>
                                        MOAS Users:</h4>
                                    <asp:DataList RepeatColumns="5" CellSpacing="5" ID="dtCompanyAdmin" runat="server">
                                        <ItemTemplate>
                                            <span class="custom-checkbox alignleft" id="adminspan" runat="server">
                                                <asp:CheckBox ID="chkCompanyAdmins" onclick='VisiblePriority(this);' runat="server" />
                                            </span>
                                            <label>
                                                <asp:Label ID="lblCompanyAdmins" Text='<%# Eval("FirstName") + " " + Eval("LastName") %>'
                                                    runat="server"></asp:Label></label>
                                            &nbsp;
                                            <asp:TextBox runat="server" ID="txtApproverPriority" onkeypress="return isNumberkey(event);"
                                                CssClass="allowValue" Width="12" MaxLength="1" Style="background-color: #303030;
                                                float: right; display: none; padding: 0px 5px;"></asp:TextBox>
                                            <asp:HiddenField ID="hdnCompanyAdmins" runat="server" Value='<%#Eval("UserInfoID")%>' />
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <div class="errormessage">
                                        Please provide priority order properly to make this functionality work correctly.
                                        (Avoid duplicate values)
                                    </div>
                                    <div class="spacer25">
                                    </div>
                                    <div id="dvMOASBillingInfoError" runat="server" visible="false" class="errormessage">
                                        There is no billing information available for this user.
                                    </div>
                                    <div runat="server" class="MOASbillinginfo" id="dvMOASBillingInfo">
                                        <h4>
                                            MOAS Billing Information:</h4>
                                        <table class="form_table" style="width: 700px;">
                                            <tr>
                                                <td class="formtd">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Company</span>
                                                        <asp:Label ID="lblBillingCompanyName" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </td>
                                                <td class="formtd">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">First Name</span>
                                                        <asp:Label ID="lblBillingFirstName" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formtd">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Last Name</span>
                                                        <asp:Label ID="lblBillingLastName" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </td>
                                                <td class="formtd">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Address1</span>
                                                        <asp:Label ID="lblBillingAddress1" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formtd">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Address2</span>
                                                        <asp:Label ID="lblBillingAddress2" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </td>
                                                <td class="formtd">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Country</span>
                                                        <asp:Label ID="lblBillingCountry" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="form_bot_co">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formtd">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">State</span>
                                                        <asp:Label ID="lblBillingState" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </td>
                                                <td class="formtd">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">City</span>
                                                        <asp:Label ID="lblBillingCity" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formtd">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Zip</span>
                                                        <asp:Label ID="lblBillingZip" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </td>
                                                <td class="formtd">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Telephone</span>
                                                        <asp:Label ID="lblBillingPhone" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formtd">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Mobile</span>
                                                        <asp:Label ID="lblBillingMobile" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </td>
                                                <td class="formtd">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Email</span>
                                                        <asp:Label ID="lblBillingEmail" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="spacer15">
                    </div>
                    <div id="dvReqChkoutInfo" runat="server" style="display: none;">
                        <h4>
                            Required Checkout Information</h4>
                        <div class="spacer15">
                        </div>
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <asp:DataList ID="dtCheckOutInfo" runat="server">
                                        <ItemTemplate>
                                            <span class="custom-checkbox alignleft" id="checkoutspan" runat="server">
                                                <asp:CheckBox ID="chkCheckOutInfo" runat="server" /></span>
                                            <label>
                                                <asp:Label ID="lblCheckOutInfo" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                            <asp:HiddenField ID="hdnChecoutInfo" runat="server" Value='<%#Eval("iLookupID")%>' />
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="formtd alignleft" style="visibility: hidden;">
                    <table class="checktable_supplier true">
                        <tr>
                            <td>
                                <span class="custom-checkbox alignleft" id="spanthirdpartybilling" runat="server">
                                    <asp:CheckBox ID="chkThirdPartyBilling" runat="server" /></span><label>Third Party Vendor
                                        Billing</label>
                            </td>
                        </tr>
                    </table>
                    <div class="spacer10">
                        &nbsp;</div>
                    <div id="dvThirdParty" runat="server" style="display: none;">
                        <table class="form_table">
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Company</span>
                                            <asp:TextBox ID="txtVendorCompany" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Contact</span>
                                            <asp:TextBox ID="txtVendorContact" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Address</span>
                                            <asp:TextBox ID="txtVendorAddress" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">City</span>
                                            <asp:TextBox ID="txtVendorCity" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">State/Province</span>
                                            <asp:TextBox ID="txtVendorState" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Zip</span>
                                            <asp:TextBox ID="txtVendorZip" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Tel</span>
                                            <asp:TextBox ID="txtVendorTelephone" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Email</span>
                                            <asp:TextBox ID="txtVendorEmail" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="divider">
            </div>
            <h4>
                Manage Email</h4>
            <div class="form_table" id="dvManageEmail">
                <table>
                    <tr>
                        <td class="formtd">
                            <table class="checktable_supplier true">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dtManageEmail" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="ManageEmailspan" runat="server">
                                                    <asp:CheckBox ID="chkManageEmail" runat="server" />
                                                </span>
                                                <label>
                                                    <asp:Label ID="lblManageEmail" Text='<%# Eval("ManageEmailName") %>' runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnManageEmail" runat="server" Value='<%#Eval("ManageEmailID")%>' />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="spacer25">
            </div>
            <div class="botbtn centeralign">
                <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" OnClientClick="return checkpaymentoption();"
                    runat="server" ToolTip="Save Information" OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
