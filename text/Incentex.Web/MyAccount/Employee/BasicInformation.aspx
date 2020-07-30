<%@ Page Title="Company Employee >> Basic Information" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="BasicInformation.aspx.cs" Inherits="MyAccount_Employee_BasicInformation" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
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
        .textarea_box textarea
        {
            height: 178px;
        }
        .textarea_box
        {
            height: 178px;
        }
        .textarea_box .scrollbar
        {
            height: 185px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">


        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtFirstName: { required: true },
                        ctl00$ContentPlaceHolder1$txtLastName: { required: true },
                        ctl00$ContentPlaceHolder1$txtLoginEmail: { required: true,
                            email: true
                            //, 
                            //remote: "../../checkexistence.aspx?action=emailexistence"
                        },
                        ctl00$ContentPlaceHolder1$txtPassword: { required: true },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtCity: { required: true },
                        ctl00$ContentPlaceHolder1$txtZip: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtCompanyEmail: { email: true },
                        ctl00$ContentPlaceHolder1$txtEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtExtension: { alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtMobile: { alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: true },
                        //ctl00$ContentPlaceHolder1$txtDateOfHired: { required: true, validdate: true },
                        ctl00$ContentPlaceHolder1$txtEmployeeId: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$ddlEmployeeType: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtSupplierCompanyName: { required: true },
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: "0" },
                        //ctl00$ContentPlaceHolder1$hdnWorkgroup: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlGender: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlRegion: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlBaseStation: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtDateActivated: { required: true, validdate: true },
                        ctl00$ContentPlaceHolder1$txtDateRequestSubmitted: { required: true }
                        //,
                        //ctl00$ContentPlaceHolder1$ddlTitle: { NotequalTo: "0" }

                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Employee status") },
                        ctl00$ContentPlaceHolder1$txtFirstName: {
                            required: replaceMessageString(objValMsg, "Required", "first name")

                        },
                        ctl00$ContentPlaceHolder1$txtLastName: {
                            required: replaceMessageString(objValMsg, "Required", "last name")

                        },
                        ctl00$ContentPlaceHolder1$txtLoginEmail: {
                            required: replaceMessageString(objValMsg, "Required", "login email"),
                            email: replaceMessageString(objValMsg, "Valid", "email address")
                        },
                        ctl00$ContentPlaceHolder1$txtPassword: {
                            required: replaceMessageString(objValMsg, "Required", "password")
                        },
                        ctl00$ContentPlaceHolder1$ddlCountry: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country")
                        },
                        ctl00$ContentPlaceHolder1$ddlState: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state")
                        },
                        ctl00$ContentPlaceHolder1$ddlCity: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city")
                        },
                        ctl00$ContentPlaceHolder1$txtCity: { required: replaceMessageString(objValMsg, "Required", "city name") },
                        ctl00$ContentPlaceHolder1$txtZip: {
                            required: replaceMessageString(objValMsg, "Required", "Zipcode"),
                            alphanumeric: replaceMessageString(objValMsg, "Number", "")
                        },
                        ctl00$ContentPlaceHolder1$txtCompanyEmail: { email: replaceMessageString(objValMsg, "Valid", "email address")
                        },
                        ctl00$ContentPlaceHolder1$txtEmail: {
                            required: replaceMessageString(objValMsg, "Required", "email"),
                            email: replaceMessageString(objValMsg, "Valid", "email address")
                        },
                        ctl00$ContentPlaceHolder1$txtTelephone: {
                            required: replaceMessageString(objValMsg, "Required", "telephone number"),
                            alphanumeric: replaceMessageString(objValMsg, "Number", "")
                        },
                        ctl00$ContentPlaceHolder1$txtExtension: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtMobile: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        //ctl00$ContentPlaceHolder1$txtDateOfHired: { required: replaceMessageString(objValMsg, "Required", "Date of hired"), validdate: "Enter date in mm/dd/yyyy format" },
                        ctl00$ContentPlaceHolder1$txtEmployeeId:
                        {
                            required: replaceMessageString(objValMsg, "Required", "Employee Id"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "Employee Id")
                        },
                        ctl00$ContentPlaceHolder1$ddlEmployeeType: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "status") },
                        ctl00$ContentPlaceHolder1$txtSupplierCompanyName: { required: replaceMessageString(objValMsg, "Required", "supplier companyname") },
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") },
                        //ctl00$ContentPlaceHolder1$hdnWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup") },
                        ctl00$ContentPlaceHolder1$ddlGender: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "gender") },
                        ctl00$ContentPlaceHolder1$ddlRegion: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "region") },
                        ctl00$ContentPlaceHolder1$ddlBaseStation: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "basestation") },
                        ctl00$ContentPlaceHolder1$txtDateActivated:
                        {
                            required: replaceMessageString(objValMsg, "Required", "Date Activated"),
                            validdate: "Enter date in mm/dd/yyyy format"
                        },
                        ctl00$ContentPlaceHolder1$txtDateRequestSubmitted:
                        {
                            required: replaceMessageString(objValMsg, "Required", "Date Request Submitted")
                        }
                        //,
                        //ctl00$ContentPlaceHolder1$ddlTitle: {NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "title") }


                    },
                    errorPlacement: function(error, element) {
                        //if (element.attr("name") == "ctl00$ContentPlaceHolder1$hdnWorkgroup")
                        // error.insertAfter("#dvWorkgroup");
                        //else 
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlGender")
                            error.insertAfter("#dvGender");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlDepartment")
                            error.insertAfter("#dvDepartment");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlStatus")
                            error.insertAfter("#divEmployeeStatus");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlRegion")
                            error.insertAfter("#dvRegion");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlBaseStation")
                            error.insertAfter("#dvstation");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtEmail")
                            error.insertAfter("#dvEmail");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtEmployeeId")
                            error.insertAfter("#dvEmployeeID");                            
                        else
                            error.insertAfter(element);
                    }
                });
                
                $("#ctl00_ContentPlaceHolder1_txtLoginEmail").blur(function() {
                    $.ajax({
                        type: "POST",
                        url: "BasicInformation.aspx/CheckDuplicateEmail",
                        data: "{'email':'" + $(this).val() + "', 'userInfoID':'" + $("#ctl00_ContentPlaceHolder1_hdnUserInfoID").val() + "', 'registrationID':'" + $("#ctl00_ContentPlaceHolder1_hdnRegistrationID").val() + "'}",
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
                        url: "BasicInformation.aspx/CheckDuplicateEmployeeID",
                        data: "{'employeeID':'" + $(this).val() + "', 'companyID':'" + $("#ctl00_ContentPlaceHolder1_hdnCompanyID").val() + "', 'userInfoID':'" + $("#ctl00_ContentPlaceHolder1_hdnUserInfoID").val() + "', 'registrationID':'" + $("#ctl00_ContentPlaceHolder1_hdnRegistrationID").val() + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function(msg) {
                            $("#ctl00_ContentPlaceHolder1_hdnEmployeeIDExistsCode").val(msg.d);                            
                            $.checkEmployeIDExists();
                        }
                    });
                })


                //Delete Event
                $('#aDeletePri').click(function() {

                    jConfirm(replaceMessageString(objValMsg, "DeleteConfirm", "photo"), "Delete photo", function(RetVal) {
                        if (RetVal) {
                            $('#aDeletePri').hide();
                            $('#dvProgPP').show();

                            $.post("../../controller/ajaxopes.aspx", {
                                mode: 'DELPRIPHOTO',
                                imgname: $('#imgPhotoupload').attr('title')
                            },
                        function(sRet) {
                            $('#dvProgPP').hide();
                            if (sRet == 'true') {

                                $('#imgPhotoupload').attr('src', '../../controller/createthumb.aspx?_ty=user&_path=employee-photo.gif&_twidth=140&_theight=161');
                                $('#imgPhotoupload').parent().attr('href', '../../controller/createthumb.aspx?_ty=userlarge&_path=employee-photo.gif&_twidth=600&_theight=600');

                                $('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value', 'employee-photo.gif');


                                $('#aDeletePri').hide();
                                $('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value', '');

                            }
                            else {
                                $('#aDeletePri').show();
                                $.showmsg('dvActionMessagePP', 'error', replaceMessageString(objValMsg, "ProcessError", ""), true);
                            }
                        }
                    );
                        }
                    });
                });

                /*End Change Event of all the dropdowns*/

                $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                    return $('#aspnetForm').valid() && $.checkEmailExists() && $.checkEmployeIDExists();
                });

                /* Validation for appy changes button*/

                $("#<%=lnkApplychanges.ClientID %>").click(function() {
                    $('#ctl00_ContentPlaceHolder1_txtLoginEmail').valid();
                    $('#ctl00_ContentPlaceHolder1_txtPassword').valid();
                    if ($('#ctl00_ContentPlaceHolder1_txtLoginEmail').valid() == '0' || $('#ctl00_ContentPlaceHolder1_txtPassword').valid() == '0')
                        return false;
                });

            });
            
            $.checkEmailExists = function () {                
                if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "1") {
                    $("#dvEmail").html("This email address already exists in the system.");
                    $("#dvEmail").show();
                    return false;
                }
                else if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "2") {
                    $("#dvEmail").html("A registration request with this email address is already pending for approval.");
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
            
            $(window).scroll(function () {              
                $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
        });

        function openPriModal() {
            $('#dvUpload').modal({ position: [110, ] });
        }

        $(function() {

            if ($('#<%=hdPriPhoto.ClientID%>').val() != "") {

                $('#aDeletePri').show();
                var retval = $('#<%=hdPriPhoto.ClientID%>').val();
                $('#imgPhotoupload').attr('src', '../../controller/createthumb.aspx?_ty=user&_path=' + retval + '&_twidth=140&_theight=161');
                $('#imgPhotoupload').attr('title', retval);
                $('#imgPhotoupload').parent().attr('href', '../../controller/createthumb.aspx?_ty=userlarge&_path=' + retval + '&_twidth=600&_theight=600');

                //$('#ankit').attr('href', '../../../controller/createthumb.aspx?_ty=user&_path=' + val + '&_twidth=800&_theight=800');
            }
            else
                $('#aDeletePri').hide();
        });



        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });


        $(function() {
            scrolltextarea(".scrollme1", "#Scrolltop1", "#ScrollBottom1");

        });
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
    <asp:HiddenField ID="hdnEmailExistsCode" runat="server" Value="0" />
    <asp:HiddenField ID="hdnEmployeeIDExistsCode" runat="server" Value="0" />
    <asp:HiddenField ID="hdnUserInfoID" runat="server" />
    <asp:HiddenField ID="hdnCompanyID" runat="server" />
    <asp:HiddenField ID="hdnRegistrationID" runat="server" />
    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div class="spacer10">
    </div>
    <div class="tabcontent" id="basic_information">
        <div class="form_pad">
            <div class="form_table" style="margin: 0pt auto; width: 700px;">
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                </div>
                <h4>
                    User Information</h4>
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
                                                <div class="form_box" style="height: 38px;">
                                                    <span class="input_label">First Name</span>
                                                    <asp:TextBox ID="txtFirstName" TabIndex="1" runat="server" CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label">Middle Name</span>
                                                    <asp:TextBox ID="txtMiddleName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtLastName" TabIndex="3" runat="server" CssClass="w_label"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server">
                                                    <ContentTemplate>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="input_label">Title</span>
                                                            <label class="dropimg_width">
                                                                <span class="custom-sel label-sel-small">
                                                                    <asp:DropDownList ID="ddlTitle" TabIndex="4" onchange="pageLoad(this,value);" runat="server"
                                                                        AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                </span>
                                                            </label>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Address1</span>
                                                    <asp:TextBox ID="txtAdress1" runat="server" TabIndex="5" CssClass="w_label"></asp:TextBox>
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
                                                <div class="form_box" style="height: 38px;">
                                                    <span class="input_label">Address2</span>
                                                    <asp:TextBox ID="txtAddress2" runat="server" TabIndex="6" CssClass="w_label"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:UpdatePanel ID="upnlCountry" runat="server">
                                                    <ContentTemplate>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                                <asp:DropDownList ID="ddlCountry" TabIndex="7" runat="server" onchange="pageLoad(this,value);"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </span>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:UpdatePanel ID="upnlSate" runat="server">
                                                    <ContentTemplate>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box" style="height: 38px;">
                                                            <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                                <asp:DropDownList ID="ddlState" runat="server" TabIndex="8" onchange="pageLoad(this,value);"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                                    <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </span>
                                                            <div>
                                                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upnlCountry">
                                                                    <ProgressTemplate>
                                                                        <img src="../../Images/ajaxbtn.gif" style="right: -18px;" class="progress_img" /></ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                            </div>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:UpdatePanel ID="upnlCity" runat="server">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="ddlCity" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box" style="height: 38px;">
                                                            <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                                <asp:DropDownList ID="ddlCity" runat="server" TabIndex="9" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged"
                                                                    onchange="pageLoad(this,value);" AutoPostBack="true">
                                                                    <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </span>
                                                            <div>
                                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upnlSate">
                                                                    <ProgressTemplate>
                                                                        <img src="../../Images/ajaxbtn.gif" style="right: -18px;" class="progress_img" /></ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                            </div>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
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
                                                    <asp:TextBox ID="txtCity" runat="server" TabIndex="9" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtZip" runat="server" TabIndex="10" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtTelephone" runat="server" TabIndex="11" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtMobile" runat="server" TabIndex="12" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtFax" runat="server" TabIndex="13" CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label">Company Email</span>
                                                    <asp:TextBox ID="txtCompanyEmail" runat="server" TabIndex="14" CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label">Personal Email</span>
                                                    <asp:TextBox ID="txtEmail" runat="server" TabIndex="15" CssClass="w_label"></asp:TextBox>
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
                                                <div class="form_box" style="height: 38px;">
                                                    <span class="input_label">Employee Type</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlEmployeeTypeLast" TabIndex="16" runat="server" onchange="pageLoad(this,value);">
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
                            <td class="formtd" style="padding-right: 12px;">
                                <table>
                                    <tr style="display: none;">
                                        <td class="upload_img_basic">
                                            <div class="upload_photo alignleft">
                                                <span class="lt_co"></span><span class="rt_co"></span><span class="lb_co"></span>
                                                <span class="rb_co"></span>
                                                <div id="dvPriPhotoContainer" class="upload_photo  gallery" runat="server">
                                                    <a href="../../UploadedImages/employeePhoto/employee-photo.gif" rel='prettyPhoto'>
                                                        <img id='imgPhotoupload' src="../../UploadedImages/employeePhoto/employee-photo.gif"
                                                            width='140' height='161' /></a>
                                                </div>
                                                <div class="alignright upload_btn">
                                                    <div class="noteIncentex" style="width: 100%; font-size: 12px; border: 1px;">
                                                        <img src="../../../Images/lightbulb.gif" style="z-index: -1" alt="note:" />&nbsp;Supported
                                                        file resolution is 140 X 160 (Width X Height)</div>
                                                </div>
                                            </div>
                                            <div class="alignnone">
                                            </div>
                                            <div class="alignleft upload_btn">
                                                <a class="grey2_btn" title="Delete photo"><span id="aDeletePri">Delete photo</span>
                                                </a>
                                            </div>
                                            <div class="alignleft upload_btn">
                                                <a class="grey2_btn" title="Upload Photo"><span id="aUpload" onclick="openPriModal()">
                                                    Upload Photo</span></a>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <label class="dropimg_width">
                                                        <span class="input_label">Gender</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlGender" runat="server" onchange="pageLoad(this,value);">
                                                            </asp:DropDownList>
                                                        </span>
                                                        <div id="dvGender">
                                                        </div>
                                                    </label>
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
                                                    <asp:TextBox ID="txtEmployeeId" runat="server" TabIndex="18" CssClass="w_label"></asp:TextBox>
                                                    <div id="dvEmployeeID" class="errorCustom">
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
                                                    <span class="input_label" style="width: 41%;">Company Hired Date</span>
                                                    <asp:TextBox ID="txtDateOfHired" TabIndex="19" runat="server" CssClass="datepicker1 cal_w"></asp:TextBox>
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
                                                    <span class="input_label">Last Anniversary Date</span>
                                                    <asp:TextBox ID="txtAnniversaryDate" TabIndex="20" Enabled="false" runat="server"
                                                        CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label">Status</span>
                                                    <label class="dropimg_width">
                                                        <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlEmployeeType" OnSelectedIndexChanged="ddlEmployeeType_SelectedIndexChanged"
                                                                TabIndex="22" runat="server" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </span>
                                                        <div id="dvEmployeeStatusError">
                                                        </div>
                                                    </label>
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
                                                    <asp:TextBox ID="txtSupplierCompanyName" TabIndex="23" runat="server" CssClass="w_label"></asp:TextBox>
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
                                                    <label class="dropimg_width">
                                                        <span class="input_label">Department</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlDepartment" runat="server" onchange="pageLoad(this,value);">
                                                            </asp:DropDownList>
                                                        </span>
                                                        <div id="dvDepartment">
                                                        </div>
                                                    </label>
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
                                                    <span class="input_label">Workgroup</span>
                                                    <asp:Label ID="txtWorkgroup" runat="server" TabIndex="23" CssClass="w_label"></asp:Label>
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
                                                    <label class="dropimg_width">
                                                        <span class="input_label">Base Station</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlBaseStation" runat="server" onchange="pageLoad(this,value);">
                                                            </asp:DropDownList>
                                                        </span>
                                                        <div id="dvstation">
                                                        </div>
                                                    </label>
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
                                                    <label class="dropimg_width">
                                                        <span class="input_label">Region</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlRegion" runat="server" onchange="pageLoad(this,value);">
                                                            </asp:DropDownList>
                                                        </span>
                                                        <div id="dvRegion">
                                                        </div>
                                                    </label>
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
                                                    <span class="input_label">Ext</span>
                                                    <asp:TextBox ID="txtExtension" TabIndex="25" runat="server" CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label">Login Email</span>
                                                    <asp:TextBox ID="txtLoginEmail" TabIndex="26" runat="server" CssClass="w_label"></asp:TextBox>
                                                    <div id="dvEmail" class="errorCustom">
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
                                                    <span class="input_label">Password</span>
                                                    <asp:TextBox ID="txtPassword" runat="server" TabIndex="27" CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label" style="width: 42%;">Date Request Submitted</span>
                                                    <asp:TextBox ID="txtDateRequestSubmitted" Enabled="false" TabIndex="28" runat="server"
                                                        CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label">Date Activated</span>
                                                    <asp:TextBox ID="txtDateActivated" TabIndex="29" runat="server" CssClass="cal_w datepicker1"></asp:TextBox>
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
                                                    <span class="input_label">Activated By</span>
                                                    <asp:TextBox ID="txtActivatedBy" Enabled="false" TabIndex="30" runat="server" CssClass="w_label"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                            <%----%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <label class="dropimg_width">
                                                        <span class="input_label">Status</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" onchange="pageLoad(this,value);">
                                                            </asp:DropDownList>
                                                        </span>
                                                        <div id="divEmployeeStatus">
                                                        </div>
                                                    </label>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trMOASPayment" visible="false">
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">MOAS Payment</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlMOASPayment" TabIndex="32" runat="server" onchange="pageLoad(this,value);">
                                                            <asp:ListItem Selected="True" Text="Active" Value="True"></asp:ListItem>
                                                            <asp:ListItem Text="InActive" Value="False"></asp:ListItem>
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
                <asp:LinkButton ID="lnkBtnSaveInfo" TabIndex="32" class="grey2_btn" runat="server"
                    ToolTip="Save Basic Information" OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
                <asp:LinkButton ID="lnkApplychanges" TabIndex="33" Visible="false" OnClick="lnkApplychanges_Click"
                    runat="server" CssClass="grey2_btn"><span>Send Email</span></asp:LinkButton>
            </div>
            <div class="divider">
            </div>
            <div class="form_table">
                <div>
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box taxt_area clearfix" style="height: 180px">
                        <span class="input_label alignleft" style="height: 178px">Notes/History</span>
                        <div class="textarea_box alignright">
                            <div class="scrollbar" style="height: 182px">
                                <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                    class="scrollbottom"></a>
                            </div>
                            <asp:TextBox ID="txtNoteHistory" runat="server" TabIndex="34" TextMode="MultiLine"
                                CssClass="scrollme1" Height="178px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="alignnone spacer15">
                </div>
                <div class="rightalign gallery">
                    <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                    <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="grey2_btn alignright" OnClick="lnkAddNew_Click"><span>+ Add Note</span></asp:LinkButton>
                    <at:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
                        DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup">
                    </at:ModalPopupExtender>
                </div>
                <asp:Panel ID="pnlNotes" runat="server" Style="display: none;">
                    <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                        left: 35%; top: 30%;">
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
                                            <div id="pp_full_res">
                                                <div class="pp_inline clearfix">
                                                    <div class="form_popup_box">
                                                        <div class="label_bar">
                                                            <span>Add Notes / Hisory
                                                                <br />
                                                                <br />
                                                                <asp:TextBox Height="120" Width="350" TextMode="MultiLine" ID="txtNote" runat="server"></asp:TextBox></span>
                                                        </div>
                                                        <div>
                                                            <asp:LinkButton ID="btnSubmit" runat="server" CssClass="grey2_btn" OnClick="btnSubmit_Click">
								                                      <span>Save Notes</span>
                                                            </asp:LinkButton>
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
                <div class="alignnone spacer25">
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" id="hdPriPhoto" runat="server" value="" />
</asp:Content>
<asp:Content ID="cc" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div id="dvUpload" style="display: none;">
        <form id="frmUploadPri" method="post" action="../../controller/uploadSignupPhoto.aspx"
        enctype="multipart/form-data">
        <div class="tbl_row">
            <strong>Upload photo </strong>
        </div>
        <div class="cl spacer">
        </div>
        <div class="cl spacer">
        </div>
        <div class="tbl_row">
            <div class="note" style="width: 100%;">
                <img src="Images/lightbulb.gif" alt="note:" />&nbsp;Please upload your photo. You
                can upload jpg, gif, bmp and png image files.</div>
        </div>
        <div class="cl spacer">
        </div>
        <div class="cl spacer">
        </div>
        <div class="tbl_row">
            <input type="file" size="45" name="flPriPhoto" id="flPriPhoto" onchange="checkValid();" />
        </div>
        <div class="spacer">
        </div>
        <div style="font-size: small; color: Black;">
            Maximum file size is <strong>1 MB.</strong>
        </div>
        <div class="spacer">
        </div>
        <div class="tbl_row">
            <label id="dvEditStatus" class="size-note">
            </label>
        </div>
        <div class="cl spacer">
        </div>
        <div class="tbl_row">
            <input type="button" onclick="submitpriphoto()" id="btnUploadPri" value="Upload" />
            <div id="imgProcess" style="display: none;">
                <img src="~/Images/ajaxbtn.gif" alt="Please wait..." />&nbsp;Uploading...
            </div>
        </div>

        <script language="javascript" type="text/javascript">
            //setting allowed image file types
            var hash = { 'jpg': 1, 'gif': 1, 'jpeg': 1, 'png': 1, 'bmp': 1 };

            function checkValid() {

                $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                    objValMsg = $.xml2json(xml);

                    if ($.trim($("#flPriPhoto").val()) == "") {

                        $('#dvEditStatus').html(replaceMessageString(objValMsg, "FileRequired", ""));
                        //$("#err").show();
                        return false;
                    }
                    if (!hash[get_extension($("#flPriPhoto").val())]) {
                        $('#dvEditStatus').html(replaceMessageString(objValMsg, "ImageType", "jpg,gif,bmp,png"));
                        //$("#err").show();
                        return false;
                    }
                    $('#dvEditStatus').html('');
                    return true;

                });
            }

            function submitpriphoto() {

                var btnUpload = $('#btnUploadPri');
                var progstatus = $('#imgProcess');

                $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                    objValMsg = $.xml2json(xml);

                    $('#frmUploadPri').ajaxSubmit({
                        data: {
                            mode: 'priphoto', Pg: "SignUp"
                        },
                        beforeSubmit: function(a, f, o) {

                            if ($.trim($("#flPriPhoto").val()) == "") {
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "FileRequired", ""));
                                return false;
                            }
                            if (!hash[get_extension($("#flPriPhoto").val())]) {
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "ImageType", "jpg,gif,bmp,png"));
                                //$("#err").show();
                                return false;
                            }

                            btnUpload.hide();
                            progstatus.show();

                        },
                        success: function(retval) {

                            if (retval == "SIZELIMIT") {
                                btnUpload.show();
                                progstatus.hide();
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "FileSize", "1MB"));
                            }
                            else if (retval == "FILETYPE") {
                                btnUpload.show();
                                progstatus.hide();
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "ImageType", "ImageType", "jpg,gif,bmp,png"));
                            }
                            else if (retval == "false") {
                                btnUpload.show();
                                progstatus.hide();
                                $('#dvEditStatus').html(replaceMessageString(objValMsg, "ProcessError", ""));
                            }
                            else {
                                //window.location.reload();
                                $('#dvEditStatus').html('');
                                $('#aDeletePri').show();

                                if ($('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value') != "") {

                                    $.post("controller/ajaxopes.aspx", {
                                        mode: 'DELPRIPHOTO',
                                        imgname: $('#imgPhotoupload').attr('title')
                                    },
                                        function(sRet) {
                                        }
                                    );

                                }

                                $('#imgPhotoupload').attr('src', '../../controller/createthumb.aspx?_ty=user&_path=' + retval + '&_twidth=140&_theight=161');
                                $('#imgPhotoupload').attr('title', retval);
                                $('#imgPhotoupload').parent().attr('href', '../../controller/createthumb.aspx?_ty=userlarge&_path=' + retval + '&_twidth=600&_theight=600');

                                $('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value', retval);
                                $.modal.close();
                            }
                        }
                    });
                });
            }
            function get_extension(n) {
                n = n.substr(n.lastIndexOf('.') + 1);
                return n.toLowerCase();
            }
        </script>

        </form>
    </div>
</asp:Content>
