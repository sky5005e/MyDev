<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="BasicInformation.aspx.cs" Inherits="admin_IncentexEmployee_BasicInformation" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
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
        .errorCustom
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            margin-left: 45%;
            font-style: italic;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $(function() {
            scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");
        });

        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtFirstName: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtLastName: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$txtCompanyWebsite: { url: true },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtCity: { required: true },
                        ctl00$ContentPlaceHolder1$txtAddress: { required: true },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtLogInEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$txtPassword: { required: true },
                        ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtMobile: { alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtExtension: { alphanumeric: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "WLS Status") },
                        ctl00$ContentPlaceHolder1$txtFirstName: {
                            required: replaceMessageString(objValMsg, "Required", "first name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        },
                        ctl00$ContentPlaceHolder1$txtLastName: {
                            required: replaceMessageString(objValMsg, "Required", "last name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        },
                        ctl00$ContentPlaceHolder1$txtEmail: {
                            required: replaceMessageString(objValMsg, "Required", "email"),
                            email: replaceMessageString(objValMsg, "Email", "")
                        },
                        ctl00$ContentPlaceHolder1$txtCompanyWebsite: {
                            url: replaceMessageString(objValMsg, "Website", "")
                        },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") },
                        ctl00$ContentPlaceHolder1$txtCity: { required: replaceMessageString(objValMsg, "Required", "city name") },
                        ctl00$ContentPlaceHolder1$txtAddress: { required: replaceMessageString(objValMsg, "Required", "address") },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: replaceMessageString(objValMsg, "Required", "telephone"), alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtLogInEmail: { required: replaceMessageString(objValMsg, "Required", "login email"), email: replaceMessageString(objValMsg, "Email", "") },
                        ctl00$ContentPlaceHolder1$txtPassword: { required: replaceMessageString(objValMsg, "Required", "password") },
                        ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtMobile: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtExtension: { alphanumeric: replaceMessageString(objValMsg, "Number", "") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtAddress")
                            error.insertAfter("#divtxtAddress");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlStatus")
                            error.insertAfter("#divStatus");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtLogInEmail")
                            error.insertAfter("#dvEmail");
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
            
            $("#ctl00_ContentPlaceHolder1_txtLogInEmail").blur(function() {                
                $.ajax({
                    type: "POST",
                    url: "BasicInformation.aspx/CheckDuplicateEmail",
                    data: "{'email':'" + $(this).val() + "', 'userInfoID':'" + $("#ctl00_ContentPlaceHolder1_hdnUserInfoID").val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(msg) {
                        $("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val(msg.d);
                        $.checkEmailExists();
                    }
                });
            });
            
            $("#<%=lnkSave.ClientID %>").click(function() {
                return $('#aspnetForm').valid() && $.checkEmailExists();
            });
            
            $(window).scroll(function () {              
                $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
        });
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
    <asp:HiddenField ID="hdnUserInfoID" runat="server" />
    <asp:HiddenField ID="hdnEmailExistsCode" runat="server" Value="0" />
    <mb:MenuUserControl ID="manuControl" runat="server" />
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <h4>
            Contact Information</h4>
        <div>
            <asp:UpdatePanel ID="up1" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddlCity" />
                </Triggers>
                <ContentTemplate>
                    <table class="form_table">
                        <tr>
                            <td class="formtd">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Company Name</span>
                                                    <%--<input type="text" class="w_label" />--%>
                                                    <asp:TextBox ID="txtCompany" ReadOnly="true" Text="Incentex" runat="server" CssClass="w_label"
                                                        TabIndex="1"></asp:TextBox>
                                                    <%-- <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" >
                                                        <asp:ListItem Text="..Choose a Coumpany.." Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                              --%>
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
                                                    <span class="input_label">Title</span>
                                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="w_label" TabIndex="4"></asp:TextBox>
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
                                                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" TabIndex="7"
                                                            OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                            <asp:ListItem Text="-select country-" Value="0"></asp:ListItem>
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
                                                    <span class="input_label">Zip Code</span>
                                                    <asp:TextBox ID="txtZip" runat="server" CssClass="w_label" TabIndex="11"></asp:TextBox>
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
                                                    <span class="input_label">Fax</span>
                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="w_label" TabIndex="14"></asp:TextBox>
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
                                                    <span class="input_label">Skype Name</span>
                                                    <asp:TextBox ID="txtSkypeName" runat="server" CssClass="w_label" TabIndex="17"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="formtd">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">First Name</span>
                                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="w_label" TabIndex="2"></asp:TextBox>
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
                                                    <span class="input_label">Department</span>
                                                    <asp:TextBox ID="txtDepartment" runat="server" CssClass="w_label" TabIndex="5"></asp:TextBox>
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
                                                    <span class="input_label">State/Province</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" TabIndex="8" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
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
                                                    <span class="input_label">Telephone</span>
                                                    <asp:TextBox ID="txtTelephone" runat="server" CssClass="w_label" TabIndex="12"></asp:TextBox>
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
                                                    <span class="input_label">Mobile</span>
                                                    <asp:TextBox ID="txtMobile" runat="server" CssClass="w_label" TabIndex="15"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="formtd_r">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Last Name</span>
                                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="w_label" TabIndex="3"></asp:TextBox>
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
                                                <div class="form_box employeeedit_text clearfix">
                                                    <span class="input_label alignleft">Address</span>
                                                    <div class="textarea_box alignright">
                                                        <div class="scrollbar">
                                                            <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                                class="scrollbottom"></a>
                                                        </div>
                                                        <%--<textarea name="" cols="" rows="" class="scrollme"></textarea>--%>
                                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="scrollme" TextMode="MultiLine"
                                                            TabIndex="6"></asp:TextBox>
                                                    </div>
                                                    <div id="divtxtAddress">
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
                                                    <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlCity" runat="server" TabIndex="9" onchange="pageLoad(this,value);"
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
                                                    <asp:TextBox ID="txtCity" runat="server" TabIndex="10" CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label">Extension</span>
                                                    <asp:TextBox ID="txtExtension" runat="server" CssClass="w_label" TabIndex="13"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="w_label" TabIndex="16"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Company Website</span>
                                            <asp:TextBox ID="txtCompanyWebsite" runat="server" CssClass="w_label" 
                                                TabIndex="17"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>--%>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="divider">
        </div>
        <h4>
            Employee Type</h4>
        <div class="form_table">
            <table class="checktable_supplier true">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box" style="width: 607px">
                                <span class="input_label">Employee Type</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlEmpType" runat="server" TabIndex="18" onchange="pageLoad(this,value)">
                                        <asp:ListItem Text="Direct Company Employee" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Independent Contractor" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd_r">
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <h4>
            System Login and Password</h4>
        <div>
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div class="shipmax_in">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Login Email</span>
                                            <%--<input type="text" class="w_label "/>--%>
                                            <asp:TextBox ID="txtLogInEmail" TabIndex="19" runat="server" CssClass="w_label"></asp:TextBox>
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
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Password</span>
                                            <asp:TextBox ID="txtPassword" TabIndex="20" runat="server" CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd_r ">
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label ">WLS Status</span>
                            <label class="dropimg_width150">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlStatus" onchange="pageLoad(this,value);" runat="server">
                                    </asp:DropDownList>
                                </span>
                                <div id="divStatus">
                                </div>
                            </label>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="alignnone spacer25">
        </div>
        <div class="additional_btn">
            <ul class="clearfix">
                <li>
                    <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" TabIndex="22" OnClick="lnkSave_Click">

								        <span>Save Information</span>
                    </asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
