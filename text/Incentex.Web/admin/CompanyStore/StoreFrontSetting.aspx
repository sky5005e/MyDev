<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="StoreFrontSetting.aspx.cs" Inherits="admin_Company_Employee_ShoppingSetting"
    Title="Store Workgroup >> Storefront Settings" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<style type="text/css">
        .basic_link .manage_link a, .header_bg
        {
        	text-align:left;
        }
        .basic_link img
        {
        	margin:0 4px;
        } 
</style>
    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }

        function ToglePanel(name) {
            if (name.indexOf("MOAS") >= 0) {
                var MOASCount = 0;
                $('#ctl00_ContentPlaceHolder1_dtPaymentOptions :checkbox:checked').each(function(i) {
                    if ($(this).parent().parent().find('label').find('span').text().indexOf("MOAS") >= 0)
                        MOASCount++;
                });
                $('#ctl00_ContentPlaceHolder1_dtPaymentOptionsBeforeHireDate :checkbox:checked').each(function(i) {
                    if ($(this).parent().parent().find('label').find('span').text().indexOf("MOAS") >= 0)
                        MOASCount++;
                });
                $('#ctl00_ContentPlaceHolder1_dtPaymentOptionsAfterHireDate :checkbox:checked').each(function(i) {
                    if ($(this).parent().parent().find('label').find('span').text().indexOf("MOAS") >= 0)
                        MOASCount++;
                });
                if (MOASCount > 0) {
                    $("#ctl00_ContentPlaceHolder1_dvMOASEmail").show();
                    $("#ctl00_ContentPlaceHolder1_dvReqChkoutInfo").show();
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_dvMOASEmail").hide();
                    $("#ctl00_ContentPlaceHolder1_dvReqChkoutInfo").hide();
                }
            }
            else if (name.indexOf("Employee Payroll Deduct") >= 0) {
                var EmployeePayrollCount = 0;
                $('#ctl00_ContentPlaceHolder1_dtPaymentOptions :checkbox:checked').each(function(i) {
                    if ($(this).parent().parent().find('label').find('span').text().indexOf("Employee Payroll Deduct") >= 0)
                        EmployeePayrollCount++;
                });
                $('#ctl00_ContentPlaceHolder1_dtPaymentOptionsBeforeHireDate :checkbox:checked').each(function(i) {
                    if ($(this).parent().parent().find('label').find('span').text().indexOf("Employee Payroll Deduct") >= 0)
                        EmployeePayrollCount++;
                });
                $('#ctl00_ContentPlaceHolder1_dtPaymentOptionsAfterHireDate :checkbox:checked').each(function(i) {
                    if ($(this).parent().parent().find('label').find('span').text().indexOf("Employee Payroll Deduct") >= 0)
                        EmployeePayrollCount++;
                });
                if (EmployeePayrollCount > 0) {
                    $('#ctl00_ContentPlaceHolder1_dvThirdParty').show();
                }
                else {
                    $('#ctl00_ContentPlaceHolder1_dvThirdParty').hide();
                }
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

        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
    </script>

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

            if ($('#ctl00_ContentPlaceHolder1_dvHireDatePaymentOption').css('display') != 'none') {
                if ($('#ctl00_ContentPlaceHolder1_txtHireDate').val() == '') {
                    alert("Please select hire date");
                    returnVal = false;
                }
                var val1 = [];
                $('#ctl00_ContentPlaceHolder1_dtPaymentOptionsBeforeHireDate :checkbox:checked').each(function(i) {
                    val1[i] = $(this).val();
                });

                if (val1.length <= 0) {
                    alert("Please select at least one payment option before hire date");
                    returnVal = false;
                }

                var val2 = [];
                $('#ctl00_ContentPlaceHolder1_dtPaymentOptionsAfterHireDate :checkbox:checked').each(function(i) {
                    val2[i] = $(this).val();
                });

                if (val2.length <= 0) {
                    alert("Please select at least one payment option after hire date");
                    returnVal = false;
                }
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
        function PaymentOptionVisible(Control) {
            if (Control.checked) {
                $("#ctl00_ContentPlaceHolder1_dvHireDatePaymentOption").show();
            }
            else {
                $("#ctl00_ContentPlaceHolder1_dvHireDatePaymentOption").hide();
            }
        }
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div class="employee_name">
            Work Group:
            <asp:Label ID="lblWorkGroup" runat="server" Text=""></asp:Label>
        </div>
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <h4>
            User Store Options</h4>
        <div class="form_table" id="userstoreoption">
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
                    <td class="formtd alignleft">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft" id="spanIsHiredRequire" runat="server">
                                        <asp:CheckBox ID="chkIsHiredDateRequire" runat="server" />
                                    </span>
                                    <label>
                                        <asp:Label ID="lblHiredDate" Text="Is Hired Date Require from the 'Not a Member yet page'?"
                                            runat="server"></asp:Label></label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <div class="form_table clearfix">
            <h4>
                World-Link System Payment Options</h4>
            <table class="checktable_supplier true">
                <tr>
                    <td>
                        <asp:DataList ID="dtPaymentOptions" runat="server">
                            <ItemTemplate>
                                <span class="custom-checkbox alignleft" id="paymentspan" runat="server">
                                    <asp:CheckBox ID="chkPaymentOptions" onclick='<%# string.Format("ToglePanel(\"{0}\")",Eval("sLookupName")) %>'
                                        runat="server" />
                                </span>
                                <label>
                                    <asp:Label ID="lblPaymentOptions" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                <asp:HiddenField ID="hdnPaymentOption" runat="server" Value='<%#Eval("iLookupID")%>' />
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
            <div class="checktable_supplier true">
                <span class="custom-checkbox alignleft" id="spnIsPaymentOptionByHireDate" runat="server">
                    <asp:CheckBox ID="chkIsPaymentOptionByHireDate" runat="server" onClick="PaymentOptionVisible(this)" />
                </span>
                <label>
                    Would you like to apply payment option based on Hire Date?
                </label>
            </div>
            <div class="spacer15">
            </div>
            <div id="dvHireDatePaymentOption" runat="server" style="display: none;">
                <table>
                    <tr>
                        <td class="formtd">
                            <div class="calender_l">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label">Hire Date</span>
                                    <asp:TextBox ID="txtHireDate" runat="server" CssClass="datepicker1 w_label"></asp:TextBox>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="formtd">
                            <h4>
                                Payment Options Before Hire Date</h4>
                            <table class="checktable_supplier true">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dtPaymentOptionsBeforeHireDate" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="spnPaymentOptionsBeforeHireDate" runat="server">
                                                    <asp:CheckBox ID="chkPaymentOptionsBeforeHireDate" onclick='<%# string.Format("ToglePanel(\"{0}\")",Eval("sLookupName")) %>'
                                                        runat="server" />
                                                </span>
                                                <label>
                                                    <asp:Label ID="lblPaymentOptionsBeforeHireDate" Text='<%# Eval("sLookupName") %>'
                                                        runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnPaymentOptionsBeforeHireDate" runat="server" Value='<%#Eval("iLookupID")%>' />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="formtd">
                            <h4>
                                Payment Options After Hire Date</h4>
                            <table class="checktable_supplier true">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dtPaymentOptionsAfterHireDate" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="spnPaymentOptionsAfterHireDate" runat="server">
                                                    <asp:CheckBox ID="chkPaymentOptionsAfterHireDate" onclick='<%# string.Format("ToglePanel(\"{0}\")",Eval("sLookupName")) %>'
                                                        runat="server" />
                                                </span>
                                                <label>
                                                    <asp:Label ID="lblPaymentOptionsAfterHireDate" Text='<%# Eval("sLookupName") %>'
                                                        runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnPaymentOptionsAfterHireDate" runat="server" Value='<%#Eval("iLookupID")%>' />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvMOASEmail" class="checktable_supplier true" runat="server" style="display: none;">
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
            </div>
            <div class="spacer15">
            </div>
            <div class="form_table clearfix" id="dvReqChkoutInfo" runat="server" style="display: none;">
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
        <div id="dvThirdParty" runat="server" style="display: none;">
            <h4>
                Employee Payroll Deduct Billing Address</h4>
            <table class="form_table">
                <tr>
                    <td class="formtd">
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
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">First Name</span>
                                <asp:TextBox ID="txtVendorFirstName" CssClass="w_label" runat="server"></asp:TextBox>
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
                                <span class="input_label">Last Name</span>
                                <asp:TextBox ID="txtLastName" CssClass="w_label" runat="server"></asp:TextBox>
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
                                <span class="input_label">Address</span>
                                <asp:TextBox ID="txtVendorAddress" CssClass="w_label" runat="server"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd">
                        <asp:UpdatePanel ID="upBillingCountry" runat="server">
                            <ContentTemplate>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="DrpBillingCountry" runat="server" AutoPostBack="true" CssClass="w_label"
                                                OnSelectedIndexChanged="DrpBillingCountry_SelectedIndexChanged" TabIndex="5">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td class="formtd_r">
                        <asp:UpdatePanel ID="upBillingState" runat="server">
                            <ContentTemplate>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="DrpBillingState" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DrpBillingState_SelectedIndexChanged"
                                                TabIndex="6">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <asp:UpdatePanel ID="upCity" runat="server">
                            <ContentTemplate>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="DrpBillingCity" AutoPostBack="true" runat="server" TabIndex="7">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td class="formtd">
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
                    <td class="formtd_r">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Telephone</span>
                                <asp:TextBox ID="txtVendorTelephone" CssClass="w_label" runat="server"></asp:TextBox>
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
                                <span class="input_label">Email</span>
                                <asp:TextBox ID="txtVendorEmail" CssClass="w_label" runat="server"></asp:TextBox>
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
                                <span class="input_label">Mobile</span>
                                <asp:TextBox ID="TxtMobile" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <div class="form_table clearfix">
            <h4>
                Third Party Supplier Payment Options</h4>
            <table class="checktable_supplier true">
                <tr>
                    <td>
                        <asp:DataList ID="dtThirdPartySupplierPaymentOptions" runat="server">
                            <ItemTemplate>
                                <span class="custom-checkbox alignleft" id="paymentspan" runat="server">
                                    <asp:CheckBox ID="chkPaymentOptions" runat="server" />
                                </span>
                                <label>
                                    <asp:Label ID="lblPaymentOptions" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                <asp:HiddenField ID="hdnPaymentOption" runat="server" Value='<%#Eval("iLookupID")%>' />
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer25">
        </div>
        <div class="botbtn centeralign">
            <asp:LinkButton ID="lnkBtnApplyAndOverride" class="grey2_btn" OnClientClick="return checkpaymentoption();"
                runat="server" ToolTip="Apply to New CE/CA and remove previous setting of existing CE/CA and apply this setting to existing CE/CA too."
                OnClick="lnkBtnApplyAndOverride_Click"><span>Apply & Override Existing</span></asp:LinkButton>
            <asp:LinkButton ID="lnkbtnApplyAndAddition" class="grey2_btn" OnClientClick="return checkpaymentoption();"
                runat="server" ToolTip="Apply to New CE/CA and add this setting to existing CE/CA previous settings."
                OnClick="lnkbtnApplyAndAddition_Click"><span>Apply & Addition to Existing</span></asp:LinkButton>
        </div>
    </div>
</asp:Content>
