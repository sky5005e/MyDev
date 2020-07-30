<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="signup.aspx.cs" Inherits="signup" Title="World-Link System - Signup" %>

<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) { 
            assigndesign();
        }
    </script>

    <script type="text/javascript" language="javascript">    
        $().ready(function() {
            $("#dvLoader").hide();
            
            $.get('JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtFirstName: { required: true, alpha1: true },
                        ctl00$ContentPlaceHolder1$txtLastName: { required: true, alpha1: true },
                        ctl00$ContentPlaceHolder1$txtAddress: { required: true },
                        ctl00$ContentPlaceHolder1$txtEmployeeId: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtZip: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtCaptcha: { required: true, equalTo: "#<%=hdnRandomCaptcha.ClientID%>" },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtCity: { required: true },
                        ctl00$ContentPlaceHolder1$txtHireDate: { required: true },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlGender: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlBaseStation: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlEmployeeType: { NotequalTo: "0" }
                    },

                    messages: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "company") },
                        ctl00$ContentPlaceHolder1$txtFirstName: {
                            required: replaceMessageString(objValMsg, "Required", "first name"),
                            alpha1: replaceMessageString(objValMsg, "Alpha", "")
                        },
                        ctl00$ContentPlaceHolder1$txtLastName: {
                            required: replaceMessageString(objValMsg, "Required", "last name"),
                            alpha1: replaceMessageString(objValMsg, "Alpha", "")
                        },
                        ctl00$ContentPlaceHolder1$txtHireDate: {
                        required: replaceMessageString(objValMsg, "Required", "Hired Date")
                        },
                        ctl00$ContentPlaceHolder1$txtAddress: {
                            required: replaceMessageString(objValMsg, "Required", "address")
                        },
                        ctl00$ContentPlaceHolder1$txtEmployeeId:
                        {
                            required: replaceMessageString(objValMsg, "Required", "employee id"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "employee id")
                        },
                        ctl00$ContentPlaceHolder1$txtZip: {
                            required: replaceMessageString(objValMsg, "Required", "zip"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        },
                        ctl00$ContentPlaceHolder1$txtEmail: {
                            required: replaceMessageString(objValMsg, "Required", "email address"),
                            email: replaceMessageString(objValMsg, "Valid", "email address")
                        },
                        ctl00$ContentPlaceHolder1$txtTelephone: {
                            required: replaceMessageString(objValMsg, "Required", "telephone number"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        },
                        ctl00$ContentPlaceHolder1$txtCaptcha: { required: replaceMessageString(objValMsg, "Required", "verification code"), equalTo: replaceMessageString(objValMsg, "Valid", "verification code") },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") },
                        ctl00$ContentPlaceHolder1$txtCity: { required: replaceMessageString(objValMsg, "Required", "city name") },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup") },
                        ctl00$ContentPlaceHolder1$ddlGender: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "gender") },
                        ctl00$ContentPlaceHolder1$ddlBaseStation: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "base station") },
                        ctl00$ContentPlaceHolder1$ddlEmployeeType: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "employee type") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlGender")
                            error.insertAfter("#dvGender");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlBaseStation")
                            error.insertAfter("#dvBaseStation");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtEmail")
                            error.insertAfter("#dvEmail");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtEmployeeId")
                            error.insertAfter("#dvEmployeeID");                            
                        else
                            error.insertAfter(element);
                    }
                });

                $(function() {
                    $(".datepicker1").datepicker({
                        buttonText: 'DatePicker',
                        showOn: 'button',
                        buttonImage: 'images/calender-icon.jpg',
                        buttonImageOnly: true,
                        changeYear: true,
                        changeMonth: true,
                        yearRange: '-50:+0',
                        maxDate: new Date()
                    });
                });

                $("#<%=lnkSubmitRequest.ClientID %>").click(function() {
                    return $('#aspnetForm').valid() && $.checkEmailExists() && $.checkEmployeIDExists();
                });
                
                $("#ctl00_ContentPlaceHolder1_txtEmail").blur(function() {
                    $.ajax({
                        type: "POST",
                        url: "signup.aspx/CheckDuplicateEmail",
                        data: "{'email':'" + $(this).val() + "'}",
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
                        url: "signup.aspx/CheckDuplicateEmployeeID",
                        data: "{'employeeID':'" + $(this).val() + "', 'companyID':'" + $("#ctl00_ContentPlaceHolder1_ddlCompany").val() + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function(msg) {
                            $("#ctl00_ContentPlaceHolder1_hdnEmployeeIDExistsCode").val(msg.d);                            
                            $.checkEmployeIDExists();
                        }
                    });
                })
            });            
            
            $.checkEmailExists = function () {                
                if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "1") {
                    $("#dvEmail").html("This email address already exists in the system. If you do not have your login information please click on the forgot password link on the front page and enter in your company email address or personal email address, the email associated with the account will be sent with your login information.");
                    $("#dvEmail").show();
                    return false;
                }
                else if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "2") {
                    $("#dvEmail").html("A registration request with this email address is already pending for approval. Please wait for 24 hours before registering again.");
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
                    $("#dvEmployeeID").html("This employee id already exists in the system for the selected company.");
                    $("#dvEmployeeID").show();
                    return false;
                }
                else if ($("#ctl00_ContentPlaceHolder1_hdnEmployeeIDExistsCode").val() == "2") {
                    $("#dvEmployeeID").html("A registration request with this employee id is already pending for approval for the selected company. Please wait for 24 hours before registering again.");
                    $("#dvEmployeeID").show();
                    return false;
                }
                else if ($("#ctl00_ContentPlaceHolder1_hdnEmployeeIDExistsCode").val() == "0") {
                    $("#dvEmployeeID").hide();
                    return true;
                }                
            }
            
            $(window).scroll(function() {
                $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop());
            });

            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
        });
    </script>

    <style type="text/css">
        #recaptcha_switch_audio
        {
            display: none;
        }
        .dropimg_width318 select
        {
            width: 318px;
        }
        .form_box div.error
        {
            font-style: italic;
            margin-left: 31%;
            text-align: left;
            color: Red;
            font-size: 11px;
        }
    </style>
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
    <asp:HiddenField ID="hdnEmailExistsCode" runat="server" Value="0" />
    <asp:HiddenField ID="hdnEmployeeIDExistsCode" runat="server" Value="0" />
    <div class="form_pad">
        <div style="text-align: center; color: Red; font-size: larger;">
            <label id="lblmsg" runat="server">
            </label>
        </div>
        <h4 style="text-align: center;">
            Please fill out the information requested on this form. Once you have submitted
            your request it will be reviewed.<br />
            When approved we will email you your login and password to access the World-Link
            System.</h4>
        <div class="spacer20">
        </div>
        <div class="not_yet_pad">
            <table class="form_table">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">First Name</span>
                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
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
                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
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
                                <asp:TextBox ID="txtAddress" runat="server" MaxLength="300" CssClass="w_label"></asp:TextBox>
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
                                <span class="input_label">Country</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlCountry" runat="server" onchange="pageLoad(this,value);"
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
                                <span class="input_label">State</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlState" runat="server" onchange="pageLoad(this,value);" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
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
                                <span class="input_label">City</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);"
                                        OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
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
                                <asp:TextBox ID="txtCity" runat="server" MaxLength="200" CssClass="w_label"></asp:TextBox>
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
                                <asp:TextBox ID="txtZip" runat="server" MaxLength="15" CssClass="w_label"></asp:TextBox>
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
                                <asp:TextBox ID="txtTelephone" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
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
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" CssClass="w_label"></asp:TextBox>
                                <div id="dvEmail" class="error">
                                </div>
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
                                <span class="input_label">Company</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlCompany" onchange="pageLoad(this,value);" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Employee ID</span>
                                <asp:TextBox ID="txtEmployeeId" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
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
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Workgroup</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlWorkgroup" runat="server" onchange="pageLoad(this,value);"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlWorkgroup__SelectedIndexChanged">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvWorkgroup">
                                </div>
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
                                <span class="input_label">Base Station</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlBaseStation" runat="server" onchange="pageLoad(this,value);">
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
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Gender</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlGender" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvGender">
                                </div>
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
                                <span class="input_label">Employee Type</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlEmployeeType" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trHireDate" runat="server">
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 28%;">Hire Date</span>
                                <asp:TextBox ID="txtHireDate" runat="server" MaxLength="20" CssClass="datepicker1 cal_w"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box" align="center">
                                <table>
                                    <tr>
                                        <td style="padding-bottom: 0px;">
                                            <span class="input_label" style="height: 65px; width: 92%; text-align: left;">Verification
                                                Image</span>
                                        </td>
                                        <td style="padding-bottom: 0px;">
                                            <table>
                                                <tr>
                                                    <td style="padding-bottom: 0px; width: 152px;">
                                                        <img id="imgCaptcha" runat="server" alt="captcha" src="webchaptcha.aspx" />
                                                    </td>
                                                    <td style="float: left; margin-top: 20px;">
                                                        <asp:LinkButton ID="btnRefresh" CssClass="btn_gray" runat="server"><span>Refresh</span></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
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
                                <span class="input_label">Verification Code</span>
                                <asp:TextBox ID="txtCaptcha" runat="Server" MaxLength="10"></asp:TextBox>
                                <%--<recaptcha:RecaptchaControl ID="RecaptchaControl1" runat="server" CssClass="w_label" PublicKey="6Ld8B8wSAAAAABOGLgvssj9Svr3QBvuKJiSnJDsg" PrivateKey="6Ld8B8wSAAAAACqW3xjDsdQ7O8hA_55M-F5BAvkF"  Theme="blackglass" Width="100%" />--%>
                                <div id="dvCaptcha">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkSubmitRequest" class="gredient_btn" runat="server" OnClick="lnkSubmitRequest_Click"><span><strong>Submit Request</strong></span></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <div id="divSqlDataSource">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <input id="hdnDepartment" type="hidden" value="0" runat="server" />
    <input id="hdnRandomCaptcha" type="hidden" style="display: none;" runat="server" />
    <%-- </form>--%>

    <script type="text/javascript">
   (function() {
       var mf = document.createElement("script"); mf.type = "text/javascript"; mf.async = true;
       mf.src = "//cdn.mouseflow.com/projects/6400d321-64cf-4fcb-a60d-a9e5ecdd0ee2.js";
       document.getElementsByTagName("head")[0].appendChild(mf);
   })();
    </script>

</asp:Content>
