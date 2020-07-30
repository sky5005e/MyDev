<%@ Page Title="Company Employee >> Basic Information" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="BasicInformation.aspx.cs" Inherits="admin_Company_Employee_BasicInformation" %>

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
        .checktable_supplier input
        {
            width: 22px;
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

        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlEmployeeStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtFirstName: { required: true },
                        ctl00$ContentPlaceHolder1$txtLastName: { required: true },
                        ctl00$ContentPlaceHolder1$txtLoginEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$txtPassword: { required: true },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtCity: { required: true },
                        ctl00$ContentPlaceHolder1$txtZip: { required: true },
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
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlGender: { NotequalTo: "0" },
                        //ctl00$ContentPlaceHolder1$ddlRegion: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlBasestation: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtDateActivated: { required: true, validdate: true },
                        ctl00$ContentPlaceHolder1$txtDateRequestSubmitted: { required: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlEmployeeStatus: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Employee status") },
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
                        ctl00$ContentPlaceHolder1$txtZip: { required: replaceMessageString(objValMsg, "Required", "zipcode") },
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
                        ctl00$ContentPlaceHolder1$ddlEmployeeType: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "status") },
                        ctl00$ContentPlaceHolder1$txtSupplierCompanyName: { required: replaceMessageString(objValMsg, "Required", "supplier companyname") },
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup") },
                        ctl00$ContentPlaceHolder1$ddlGender: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "gender") },
                        //ctl00$ContentPlaceHolder1$ddlRegion: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "region") },
                        ctl00$ContentPlaceHolder1$ddlBasestation: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "basestation") },
                        ctl00$ContentPlaceHolder1$txtDateActivated:
                        {
                            required: replaceMessageString(objValMsg, "Required", "Date Activated"),
                            validdate: "Enter date in mm/dd/yyyy format"
                        },
                        ctl00$ContentPlaceHolder1$txtDateRequestSubmitted:
                        {
                            required: replaceMessageString(objValMsg, "Required", "Date Request Submitted")
                        }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlGender")
                            error.insertAfter("#dvGender");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlEmployeeType")
                            error.insertAfter("#dvEmployeeStatusError");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlDepartment")
                            error.insertAfter("#dvDepartment");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlEmployeeStatus")
                            error.insertAfter("#dvEmployeeStatus");
                        //else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlRegion")
                        //error.insertAfter("#dvRegion");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlBasestation")
                            error.insertAfter("#dvBasestation");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtLoginEmail")
                            error.insertAfter("#dvLoginEmail");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtEmployeeId")
                            error.insertAfter("#dvEmployeeID");
                        else
                            error.insertAfter(element);
                    }
                });//validate
            });//get
            
            $.checkEmailExists = function () {
                if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "1") {
                    $("#dvLoginEmail").html("This email address already exists in the system.");
                    $("#dvLoginEmail").show();
                    return false;
                }
                else if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "2") {
                    $("#dvLoginEmail").html("A registration request with this email adress is already pending for approval.");
                    $("#dvLoginEmail").show();
                    return false;
                }
                else if ($("#ctl00_ContentPlaceHolder1_hdnEmailExistsCode").val() == "0") {
                    $("#dvLoginEmail").hide();
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
            
            $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                return $('#aspnetForm').valid() && $.checkEmailExists() && $.checkEmployeIDExists();
            });    

            $('#ctl00_ContentPlaceHolder1_txtNote').live('keydown', function(e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode == 9) {
                    e.preventDefault();
                    $('#ctl00_ContentPlaceHolder1_btnSubmit').focus();
                }
            });
            
            $('#aDeletePri').click(function() {
                jConfirm(replaceMessageString(objValMsg, "DeleteConfirm", "photo"), "Delete photo", function(RetVal) {
                    if (RetVal) {
                        $('#aDeletePri').hide();
                        $('#dvProgPP').show();

                        $.post("../../../controller/ajaxopes.aspx", {
                                mode: 'DELPRIPHOTO',
                                imgname: $('#imgPhotoupload').attr('title')
                            },
                            function(sRet) {
                                $('#dvProgPP').hide();
                                if (sRet == 'true') {
                                    $('#imgPhotoupload').attr('src', '../../../controller/createthumb.aspx?_ty=user&_path=employee-photo.gif&_twidth=140&_theight=161');
                                    $('#imgPhotoupload').parent().attr('href', '../../../controller/createthumb.aspx?_ty=userlarge&_path=employee-photo.gif&_twidth=600&_theight=600');
                                    $('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value', 'employee-photo.gif');
                                    $('#aDeletePri').hide();
                                    $('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value', '');
                                }
                                else {
                                    $('#aDeletePri').show();
                                    $.showmsg('dvActionMessagePP', 'error', replaceMessageString(objValMsg, "ProcessError", ""), true);
                                }
                            }
                        );//post
                    }//if
                });//jConfirm
            });//aDeletePri click

            $("#<%=lnkApplychanges.ClientID %>").click(function() {
                return $('#aspnetForm').valid() && $.checkEmailExists() && $.checkEmployeIDExists();
            });
            
            $(window).scroll(function () {              
                $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
            
        });//ready

        function openPriModal() {
            $('#dvUpload').modal({ position: [110, ] });
        }

        $(function() {

            if ($('#<%=hdPriPhoto.ClientID%>').val() != "") {
                $('#aDeletePri').show();
                var retval = $('#<%=hdPriPhoto.ClientID%>').val();

                $('#imgPhotoupload').attr('src', '../../../controller/createthumb.aspx?_ty=user&_path=' + retval + '&_twidth=140&_theight=161');
                $('#imgPhotoupload').attr('title', retval);
                $('#imgPhotoupload').parent().attr('href', '../../../controller/createthumb.aspx?_ty=userlarge&_path=' + retval + '&_twidth=600&_theight=600');

                //$('#ankit').attr('href', '../../../controller/createthumb.aspx?_ty=user&_path=' + val + '&_twidth=800&_theight=800');
            }
            else
                $('#aDeletePri').hide();
        });



        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../../images/calender-icon.jpg',
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
        
        function ShowHideWorkGroupPanel(Ctrl,DivID) {
            $("#" + DivID).toggle($(Ctrl).is(":checked"));
        }
        
        $(document).ready(function() {            
            ShowHideWorkGroupPanel($("#<%= chkIsStaionLevelMOASApprover.ClientID%>"),'MOASWorkGroupAcess');
            $(".workgroupapprover input").each(function() {
                if($(this).is(":checked")) {
                    $(this).next().toggleClass("checkbox-off");
                    $(this).next().toggleClass("checkbox-on");
                }
            });            
        });
        
        function CheckWorkgroupSelection() {
            if($('#<%= tr_MOASStationLevelApprover.ClientID %>').is(':visible') && $('#<%= chkIsStaionLevelMOASApprover.ClientID %>').is(":checked")) {
                var SelectionCount = 0;
                $(".workgrouplist").find(".workgroupapprover input").each(function() {
                    if($(this).is(":checked"))
                        SelectionCount++;
                });
                
                if(SelectionCount == 0) {
                    alert("Please select atleast one workgroup for station level approver")
                    return false;                
                }
                else 
                    return true;
             }
             else 
                return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
    <asp:HiddenField ID="hdnRegistrationID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnUserInfoID" runat="server" />
    <asp:HiddenField ID="hdnCompanyID" runat="server" />
    <asp:HiddenField ID="hdnEmailExistsCode" runat="server" Value="0" />
    <asp:HiddenField ID="hdnEmployeeIDExistsCode" runat="server" Value="0" />
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
                    User Information:</h4>
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
                                                            <span class="input_label">Title</span> <span class="custom-sel label-sel-small">
                                                                <asp:DropDownList ID="ddlTitle" TabIndex="4" onchange="pageLoad(this,value);" runat="server">
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
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box" style="height: 38px;">
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
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlState" EventName="SelectedIndexChanged" />
                                                    </Triggers>
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
                                                                        <img src="../../../Images/ajaxbtn.gif" style="right: -18px;" class="progress_img" /></ProgressTemplate>
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
                                                        <asp:AsyncPostBackTrigger ControlID="ddlCity" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box" style="height: 38px;">
                                                            <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                                <asp:DropDownList ID="ddlCity" runat="server" TabIndex="9" onchange="pageLoad(this,value);"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                                                                    <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </span>
                                                            <div>
                                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="upnlSate">
                                                                    <ProgressTemplate>
                                                                        <img src="../../../Images/ajaxbtn.gif" style="right: -18px;" class="progress_img" /></ProgressTemplate>
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
                                                    <span class="input_label">Zip</span>
                                                    <asp:TextBox ID="txtZip" runat="server" TabIndex="11" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtTelephone" runat="server" TabIndex="12" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtMobile" runat="server" TabIndex="13" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtFax" runat="server" TabIndex="14" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtCompanyEmail" runat="server" TabIndex="15" CssClass="w_label"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtEmail" runat="server" TabIndex="16" CssClass="w_label"></asp:TextBox>
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
                                                        <asp:DropDownList ID="ddlEmployeeTypeLast" TabIndex="17" runat="server" onchange="pageLoad(this,value);">
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
                                                <div class="form_box" style="height: 38px;">
                                                    <span class="input_label">Climate Setting</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlClimateSetting" runat="server" onchange="pageLoad(this,value);"
                                                            TabIndex="7">
                                                            <asp:ListItem Text="-select Climate-" Value="0"></asp:ListItem>
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
                                                    <a href="../../../UploadedImages/employeePhoto/employee-photo.gif" rel='prettyPhoto'>
                                                        <img id='imgPhotoupload' src="../../../UploadedImages/employeePhoto/employee-photo.gif" /></a>
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
                                                    <span class="input_label">Gender</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlGender" TabIndex="18" runat="server" onchange="pageLoad(this,value);">
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
                                                    <span class="input_label">Employee #</span>
                                                    <asp:TextBox ID="txtEmployeeId" runat="server" TabIndex="19" CssClass="w_label"></asp:TextBox>
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
                                                    <span class="input_label">Company Hired Date</span>
                                                    <asp:TextBox ID="txtDateOfHired" TabIndex="20" runat="server" CssClass="datepicker1 cal_w"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtAnniversaryDate" TabIndex="21" Enabled="false" runat="server"
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
                                                    <span class="input_label">Status</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlEmployeeType" OnSelectedIndexChanged="ddlEmployeeType_SelectedIndexChanged"
                                                            TabIndex="22" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div id="dvEmployeeStatusError">
                                                    </div>
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
                                                    <span class="custom-sel label-sel">
                                                        <asp:DropDownList ID="ddlDepartment" onchange="pageLoad(this,value);" TabIndex="23"
                                                            runat="server">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div id="dvDepartment">
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
                                                <div class="form_box ">
                                                    <span class="custom-sel label-sel">
                                                        <asp:DropDownList ID="ddlWorkgroup" onchange="pageLoad(this,value);" TabIndex="24"
                                                            runat="server">
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
                                                <asp:UpdatePanel ID="upBaseStation" runat="server">
                                                    <ContentTemplate>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="custom-sel label-sel">
                                                                <asp:DropDownList ID="ddlBasestation" onchange="pageLoad(this,value);" TabIndex="24"
                                                                    runat="server">
                                                                </asp:DropDownList>
                                                            </span>
                                                            <div id="dvBasestation">
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
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box ">
                                                    <span class="custom-sel label-sel">
                                                        <asp:DropDownList ID="ddlRegion" onchange="pageLoad(this,value);" TabIndex="25" runat="server">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div id="dvRegion">
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
                                                    <div id="dvLoginEmail" class="errorCustom">
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
                                                    <span class="input_label">Date Request Submitted</span>
                                                    <asp:TextBox ID="txtDateRequestSubmitted" TabIndex="28" runat="server" Enabled="false"
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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="custom-sel label-sel">
                                                        <asp:DropDownList ID="ddlEmployeeStatus" onchange="pageLoad(this,value);" TabIndex="31"
                                                            runat="server">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div id="dvEmployeeStatus">
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
                                                    <span class="input_label">World-Link Contact ID</span>
                                                    <asp:TextBox ID="txtWorldLinkContactID" ReadOnly="true" TabIndex="32" runat="server"
                                                        CssClass="w_label"></asp:TextBox>
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
                                                        <asp:DropDownList ID="ddlMOASPayment" TabIndex="33" runat="server" onchange="pageLoad(this,value);">
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
                                    <tr id="tr_MOASStationLevelApprover" runat="server" visible="false">
                                        <td>
                                            <div class="form_table">
                                                <div class="checktable_supplier true clearfix">
                                                    <span id="Span_MoasAprover" runat="server" class="custom-checkbox alignleft workgroupapprover">
                                                        <asp:CheckBox ID="chkIsStaionLevelMOASApprover" runat="server" onclick="ShowHideWorkGroupPanel(this,'MOASWorkGroupAcess')" />
                                                    </span>
                                                    <label>
                                                        Is Station Level MOAS Approver
                                                    </label>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="MOASWorkGroupAcess" style="display: none;" class="checktable_supplier true">
                            <td colspan="2">
                                <div>
                                    <asp:DataList RepeatColumns="3" CellSpacing="5" ID="dtWorkGroups" runat="server"
                                        CssClass="workgrouplist">
                                        <ItemTemplate>
                                            <%--<span class='<%# Convert.ToBoolean(Eval("IsAssigned"))== false?"custom-checkbox alignleft":"custom-checkbox_checked alignleft" %>' id="adminspan" runat="server">--%>
                                            <span class="custom-checkbox alignleft workgroupapprover" id="adminspan" runat="server">
                                                <asp:CheckBox ID="chkWorkGroups" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsAssigned")) %>' />
                                            </span>
                                            <label>
                                                <asp:Label ID="lblCompanyAdmins" Text='<%# Eval("sLookupName") %>' runat="server"
                                                    CssClass="workgroupname"></asp:Label></label>
                                            <asp:HiddenField ID="hdnWorkGroupID" runat="server" Value='<%#Eval("iLookupID")%>' />
                                            <%--<span id="BasicWorkGroupID"><%# Convert.ToBoolean(Eval("IsBasicWorkgroup")) == true? Eval("sLookupName") :  "" %></span>--%>
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <asp:HiddenField ID="hdnAssigedWorkGroup" runat="server" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="botbtn centeralign">
                <asp:LinkButton ID="lnkBtnSaveInfo" TabIndex="33" class="grey2_btn" runat="server"
                    ToolTip="Save Basic Information" OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
                <asp:LinkButton ID="lnkApplychanges" TabIndex="32" Visible="false" OnClick="lnkApplychanges_Click"
                    runat="server" CssClass="grey2_btn"><span>Send Email</span></asp:LinkButton>
            </div>
            <div class="divider">
            </div>
            <h4>
                Scheduled Events:</h4>
            <div>
                <asp:UpdatePanel runat="server" ID="upnlEventGrid">
                    <ContentTemplate>
                        <div style="text-align: center">
                            <asp:Label ID="lblEventMsg" runat="server" CssClass="errormessage"></asp:Label>
                        </div>
                        <asp:GridView ID="dtlEvents" runat="server" HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box"
                            GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                            OnRowDataBound="dtlEvents_RowDataBound" OnRowCommand="dtlEvents_RowCommand">
                            <Columns>
                                <asp:TemplateField SortExpression="EventName">
                                    <HeaderTemplate>
                                        <span>
                                            <asp:LinkButton ID="lnkbtnEventName" runat="server" CommandArgument="EventName" CommandName="Sort">Event</asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderEventName" runat="server"></asp:PlaceHolder>
                                        </span>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="centeralign" />
                                    <ItemTemplate>
                                        <span class="first">
                                            <%# Eval("EventName") %>
                                        </span>
                                        <div class="corner">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box centeralign" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="EventDate">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnEventDate" runat="server" CommandArgument="EventDate" CommandName="Sort"><span>Date</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderEventDate" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="centeralign" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblEventDate" Text='<%# Eval("EventDate","{0:d}") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box centeralign" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="EventTime">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnEventTime" runat="server" CommandArgument="EventTime" CommandName="Sort"><span>Time</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderEventTime" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="centeralign" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblEventTime" Text='<%# Eval("EventTime") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box centeralign" Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="EventReminder">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnEventReminder" runat="server" CommandArgument="EventReminder"
                                            CommandName="Sort"><span>Reminder</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderEventReminder" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="centeralign" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblEventReminder" Text='<%# Eval("EventReminder") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span>Edit</span>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="centeralign" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnEdit" ToolTip="Click to edit event" CommandName="EditEvent"
                                            CommandArgument='<%# Eval("EventID") %>' runat="server">
                                            <span class="btn_space">
                                                <img alt="Edit" id="edit" src="~/Images/edit-icon.png" width="24" height="24" runat="server" /></span></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span>Delete</span>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="centeralign" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnDelete" ToolTip="Click to delete event" OnClientClick="return confirm('Are you sure, you want to delete selected item?');"
                                            CommandName="DeleteEvent" CommandArgument='<%# Eval("EventID") %>' runat="server">
                                            <span class="btn_space">
                                                <img alt="Delete" id="delete" src="~/Images/close-btn.png" width="24" height="24"
                                                    runat="server" /></span></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="spacer10">
            </div>
            <div class="rightalign">
                <asp:LinkButton ID="lnkbtnAddEvent" runat="server" OnClick="lnkbtnAddEvent_Click"
                    Visible="false" CssClass="grey2_btn"><span>Schedule Event</span></asp:LinkButton>
            </div>
            <div class="divider">
            </div>
            <div id="dvCampaign" runat="server">
                <h4>
                    Campaign:</h4>
                <div>
                    <asp:GridView ID="gvTemplatesList" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvTemplatesList_RowDataBound">
                        <Columns>
                            <asp:TemplateField Visible="False" HeaderText="Id">
                                <HeaderTemplate>
                                    <span>TempID</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTempID" Text='<%# Eval("TempID") %>' />
                                    <asp:Label runat="server" ID="lblNoteID" Text='<%# Eval("CampaignID") %>' />
                                    <asp:Label runat="server" ID="lblCampid" Text='<%# Eval("CampNoteID") %>' />
                                    <asp:Label runat="server" ID="lblUserInfoId" Text='<%# Eval("UserInfoID") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="2%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="NoteContents">
                                <HeaderTemplate>
                                    <span>
                                        <asp:LinkButton ID="lnkbtnNoteContents" runat="server" CommandArgument="NoteContents"
                                            CommandName="Sort">Note Contents</asp:LinkButton>
                                    </span>
                                    <asp:PlaceHolder ID="placeholderNoteContents" runat="server"></asp:PlaceHolder>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblNoteContents" Text='<%# Eval("Notecontents")%>' />
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="70%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="ViewTemp">
                                <HeaderTemplate>
                                    <span>
                                        <asp:LinkButton ID="lnkbtnViewCamp" runat="server" CommandArgument="ViewCamp" CommandName="Sort">Details</asp:LinkButton>
                                    </span>
                                    <asp:PlaceHolder ID="placeholderViewCamp" runat="server"></asp:PlaceHolder>
                                    <div class="corner">
                                        <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>
                                        <asp:LinkButton ID="hypViewTemp" CommandName="open" runat="server" ToolTip="view templates"
                                            CommandArgument='<%# Eval("TempID") %>'>View Templates</asp:LinkButton>
                                    </span>
                                    <div class="corner">
                                        <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="28%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="divider">
                </div>
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
                                                            <span>Add Notes / History
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
            </div>
        </div>
    </div>
    <input type="hidden" id="hdPriPhoto" runat="server" value="" />
</asp:Content>
<asp:Content ID="cc" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div id="dvUpload" style="display: none;">
        <form id="frmUploadPri" method="post" action="../../../controller/uploadSignupPhoto.aspx"
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

                $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
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

                $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
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

                                $('#imgPhotoupload').attr('src', '../../../controller/createthumb.aspx?_ty=user&_path=' + retval + '&_twidth=140&_theight=161');
                                $('#imgPhotoupload').attr('title', retval);
                                $('#imgPhotoupload').parent().attr('href', '../../../controller/createthumb.aspx?_ty=userlarge&_path=' + retval + '&_twidth=600&_theight=600');

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
