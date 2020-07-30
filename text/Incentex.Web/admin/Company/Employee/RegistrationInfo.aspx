<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="RegistrationInfo.aspx.cs" Inherits="admin_Company_Employee_RegistrationInfo"
    Title="Registration Details" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 36px;
        }
        .form_box div.error
        {
            font-style: italic;
            margin-left: 43%;
            text-align: left;
            color: Red;
            font-size: 11px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtFirstName: { required: true },
                        ctl00$ContentPlaceHolder1$txtLastName: { required: true },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtCity: { required: true },
                        ctl00$ContentPlaceHolder1$txtZip: { required: true },
                        ctl00$ContentPlaceHolder1$txtEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: true },
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtEmployeeId: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$ddlUserType: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtSupplierCompanyName: { required: true },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlGender: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlBasestation: { NotequalTo: "0" }                        
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtFirstName: { required: replaceMessageString(objValMsg, "Required", "first name") },
                        ctl00$ContentPlaceHolder1$txtLastName: { required: replaceMessageString(objValMsg, "Required", "last name") },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") },
                        ctl00$ContentPlaceHolder1$txtCity: { required: replaceMessageString(objValMsg, "Required", "city name") },
                        ctl00$ContentPlaceHolder1$txtZip: { required: replaceMessageString(objValMsg, "Required", "zipcode") },
                        ctl00$ContentPlaceHolder1$txtEmail: {
                            required: replaceMessageString(objValMsg, "Required", "email address"),
                            email: replaceMessageString(objValMsg, "Valid", "email address")
                        },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: replaceMessageString(objValMsg, "Required", "telephone number") },
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "company") }, 
                        ctl00$ContentPlaceHolder1$txtEmployeeId:
                        {
                            required: replaceMessageString(objValMsg, "Required", "employee id"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "Employee Id")
                        },
                        ctl00$ContentPlaceHolder1$ddlUserType: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "status") }, 
                        ctl00$ContentPlaceHolder1$txtSupplierCompanyName: { required: replaceMessageString(objValMsg, "Required", "supplier companyname") },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "workgroup") },
                        ctl00$ContentPlaceHolder1$ddlGender: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "gender") },
                        ctl00$ContentPlaceHolder1$ddlBasestation: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "basestation") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtEmail")
                            error.insertAfter("#dvEmail");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtEmployeeId")
                            error.insertAfter("#dvEmployeeID");
                        else
                            error.insertAfter(element);
                    }
                });
            });
            
            $.checkEmailExists = function () {
                if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "1") {
                    $("#dvEmail").html("This email address already exists in the system.");
                    $("#dvEmail").show();
                    return false;
                }
                else if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "2") {
                    $("#dvEmail").html("A registration request with this email adress is already pending for approval.");
                    $("#dvEmail").show();
                    return false;
                }
                else if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "0") {
                    $("#dvEmail").hide();
                    return true;
                }                
            } 
            
            $.checkEmployeIDExists = function () {                
                if ($("#ctl00_ContentPlaceHolder1_hdnEmployeeIDExistsCode").val() == "1") {
                    $("#dvEmployeeID").html("This employee id already exists in the system.");
                    $("#dvEmployeeID").show();
                    return false;
                }
                else if ($("#ctl00_ContentPlaceHolder1_hdnEmployeeIDExistsCode").val() == "2") {
                    $("#dvEmployeeID").html("A registration request with this employee id is already pending for approval.");
                    $("#dvEmployeeID").show();
                    return false;
                }
                else if ($("#ctl00_ContentPlaceHolder1_hdnEmployeeIDExistsCode").val() == "0") {
                    $("#dvEmployeeID").hide();
                    return true;
                }                
            }
            
            $("#ctl00_ContentPlaceHolder1_txtEmail").blur(function() {
                $.ajax({
                    type: "POST",
                    url: "RegistrationInfo.aspx/CheckDuplicateEmail",
                    data: "{'email':'" + $(this).val() + "', 'registrationID':'" + $("#ctl00_ContentPlaceHolder1_hdnRegistrationID").val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(msg) {
                        $("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val(msg.d);                            
                        $.checkEmailExists();
                    }
                });
            });
            
            $("#ctl00_ContentPlaceHolder1_txtEmployeeId").blur(function() {
                $.ajax({
                    type: "POST",
                    url: "RegistrationInfo.aspx/CheckDuplicateEmployeeID",
                    data: "{'employeeID':'" + $(this).val() + "', 'companyID':'" + $("#ctl00_ContentPlaceHolder1_ddlCompany").val() + "', 'registrationID':'" + $("#ctl00_ContentPlaceHolder1_hdnRegistrationID").val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(msg) {
                        $("#ctl00_ContentPlaceHolder1_hdnEmployeeIDExistsCode").val(msg.d);                            
                        $.checkEmployeIDExists();
                    }
                });
            })
            
            $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {                
                return $('#aspnetForm').valid() && $.checkEmailExists() && $.checkEmployeIDExists();
            });
                        
            $(window).scroll(function () {              
                $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
            
            $('input.disablecutcopypaste').bind('cut copy paste contextmenu', function (e) {
                e.preventDefault();
            }).autocomplete({ disabled: true });
        });

        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
      
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="tabcontent" id="basic_information">
        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
        <asp:HiddenField ID="hdnRegistrationID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnEmailExistsCode" runat="server" Value="0" />
        <asp:HiddenField ID="hdnEmployeeIDExistsCode" runat="server" Value="0" />
        <div class="form_pad">
            <div class="form_table" style="margin: 0pt auto; width: 700px;">
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                </div>
                <div>
                    <table>
                        <tr>
                            <td class="formtd" style="padding-right: 12px;">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">First Name</span>
                                                    <asp:TextBox ID="txtFirstName" TabIndex="1" runat="server" CssClass="disablecutcopypaste w_label"></asp:TextBox>
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
                                                    <span class="input_label">Last Name</span>
                                                    <asp:TextBox ID="txtLastName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtAdress" runat="server" TabIndex="3" CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCountry" TabIndex="4" runat="server" onchange="pageLoad(this,value);"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </span>
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
                                                    <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlState" runat="server" TabIndex="5" onchange="pageLoad(this,value);"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                            <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
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
                                                    <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCity" runat="server" TabIndex="6" onchange="pageLoad(this,value);"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                                                            <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
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
                                                    <asp:TextBox ID="txtCity" runat="server" TabIndex="7" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtZip" runat="server" TabIndex="8" CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label">Telephone</span>
                                                    <asp:TextBox ID="txtTelephone" runat="server" TabIndex="9" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtEmail" runat="server" TabIndex="10" CssClass="w_label"></asp:TextBox>
                                                    <div id="dvEmail" class="error">
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="formtd" style="padding-right: 12px;">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box ">
                                                    <span class="input_label">Company</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCompany" onchange="pageLoad(this,value);" TabIndex="11"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trSupplierCompanyName" runat="server" visible="false">
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Supplier Company Name</span>
                                                    <asp:TextBox ID="txtSupplierCompanyName" TabIndex="15" runat="server" CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label">Employee #</span>
                                                    <asp:TextBox ID="txtEmployeeId" runat="server" TabIndex="12" CssClass="w_label"></asp:TextBox>
                                                    <div id="dvEmployeeID" class="error">
                                                    </div>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="calender_l">
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Company Hired Date</span>
                                                    <asp:TextBox ID="txtDateOfHired" TabIndex="13" runat="server" CssClass="datepicker1 cal_w"></asp:TextBox>
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
                                                <div class="form_box ">
                                                    <span class="input_label">Work Group</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlWorkgroup" onchange="pageLoad(this,value);" TabIndex="16"
                                                            runat="server">
                                                        </asp:DropDownList>
                                                    </span>
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
                                                    <span class="input_label">Base Station</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlBasestation" onchange="pageLoad(this,value);" TabIndex="17"
                                                            runat="server">
                                                        </asp:DropDownList>
                                                    </span>
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
                                                    <span class="input_label">Gender</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlGender" TabIndex="18" runat="server" onchange="pageLoad(this,value);">
                                                        </asp:DropDownList>
                                                    </span>
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
                                                    <span class="input_label">Employee Type</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlEmployeeType" TabIndex="19" runat="server" onchange="pageLoad(this,value);">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="botbtn centeralign">
                <asp:LinkButton ID="lnkBtnSaveInfo" TabIndex="20" class="grey2_btn" runat="server"
                    ToolTip="Save Information" OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
