<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MySetting.aspx.cs" Inherits="MyAccount_MySetting" MaintainScrollPositionOnPostback="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        $().ready(function() {

            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        //user info
                        ctl00$ContentPlaceHolder1$TxtFirstName: { required: true },
                        ctl00$ContentPlaceHolder1$TxtLastName: { required: true },
                        ctl00$ContentPlaceHolder1$TxtAddress: { required: true },
                        ctl00$ContentPlaceHolder1$DrpCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$TxtZip: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$TxtEmail: { required: true, email: true },

                        //billing info
                        ctl00$ContentPlaceHolder1$DrpBillingCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpBillingState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$DrpBillingCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$TxtBillingAddress: { required: true },
                        ctl00$ContentPlaceHolder1$TxtBillingZip: { required: true, alphanumeric: true },

                        //shipping

                        //ctl00$ContentPlaceHolder1$TxtShippingEmail: { email: true },
                        //ctl00$ContentPlaceHolder1$TxtShippingZip: { required: true, alphanumeric: true },
                        //ctl00$ContentPlaceHolder1$TxtShippingMobile: { required: true, alphanumeric: true },
                        // ctl00$ContentPlaceHolder1$TxtShppingTelephone: { required: true, alphanumeric: true },


                        //password

                        ctl00$ContentPlaceHolder1$TxtConfirmNewPassword: { equalTo: "#<%=TxtNewPassword.ClientID%>" }
                    },
                    messages: {
                        //user info
                        ctl00$ContentPlaceHolder1$TxtFirstName: { required: replaceMessageString(objValMsg, "Required", "First Name") },
                        ctl00$ContentPlaceHolder1$TxtLastName: { required: replaceMessageString(objValMsg, "Required", "Last Name") },
                        ctl00$ContentPlaceHolder1$TxtAddress: { required: replaceMessageString(objValMsg, "Required", "Address") },
                        ctl00$ContentPlaceHolder1$DrpCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") },
                        ctl00$ContentPlaceHolder1$DrpState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") },
                        ctl00$ContentPlaceHolder1$DrpCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") },
                        ctl00$ContentPlaceHolder1$TxtZip: { required: replaceMessageString(objValMsg, "Required", "Zip") },
                        ctl00$ContentPlaceHolder1$TxtEmail: { required: replaceMessageString(objValMsg, "Required", "Email")
                                 , email: replaceMessageString(objValMsg, "Valid", "email address")
                        },

                        //billing info
                        ctl00$ContentPlaceHolder1$DrpBillingCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") },
                        ctl00$ContentPlaceHolder1$DrpBillingState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") },
                        ctl00$ContentPlaceHolder1$DrpBillingCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") },

                        ctl00$ContentPlaceHolder1$TxtBillingAddress: { required: replaceMessageString(objValMsg, "Required", "Address") },
                        ctl00$ContentPlaceHolder1$TxtBillingZip: {
                            required: replaceMessageString(objValMsg, "Required", "zip"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        },
                        //shipping

                        //ctl00$ContentPlaceHolder1$TxtShippingEmail: { email: replaceMessageString(objValMsg, "Valid", "email address") },
                        // ctl00$ContentPlaceHolder1$TxtShippingZip: {
                        //   required: replaceMessageString(objValMsg, "Required", "zip"),
                        //  alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        // },
                        //  ctl00$ContentPlaceHolder1$TxtShippingMobile: {
                        //   required: replaceMessageString(objValMsg, "Required", "mobile number"),
                        //  alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        // },
                        // ctl00$ContentPlaceHolder1$TxtShppingTelephone: {
                        //   required: replaceMessageString(objValMsg, "Required", "mobile number"),
                        //   alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        //},
                        ctl00$ContentPlaceHolder1$TxtConfirmNewPassword: { equalTo: replaceMessageString(objValMsg, "EqualTo", "Passeord") }

                    }
                }); // form validate

                $("#<%=lnkChangePassword.ClientID %>").click(function() {

                    $("#ctl00_ContentPlaceHolder1_TxtOldPassword").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "old password") }
                    });
                });

                // validate on click of shipping
                $("#<%=lnkBtnAddAnotherShiping.ClientID %>").click(function() {

                    $("#ctl00_ContentPlaceHolder1_TxtShippingName").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "name") }
                    });

                    $("#ctl00_ContentPlaceHolder1_TxtShipingTitle").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "title") }
                    });

                    $("#ctl00_ContentPlaceHolder1_TxtShippingEmail").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "email") }
                    });


                    $("#ctl00_ContentPlaceHolder1_TxtShippingZip").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "zip") }

                    });
                    $("#ctl00_ContentPlaceHolder1_TxtShippingMobile").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "mobile no") }

                    });
                    $("#ctl00_ContentPlaceHolder1_TxtShppingTelephone").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "telephone no") }

                    });


                    $("#ctl00_ContentPlaceHolder1_DrpShipingCoutry").rules("add",
                    {
                        NotequalTo: "0", messages: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") }
                    });

                    $("#ctl00_ContentPlaceHolder1_DrpShipingState").rules("add",
                    {
                        NotequalTo: "0", messages: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") }
                    });

                    $("#ctl00_ContentPlaceHolder1_DrpShippingCity").rules("add",
                    {
                        NotequalTo: "0", messages: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") }
                    });

                    $("#ctl00_ContentPlaceHolder1_TxtShippingName").valid();
                    $("#ctl00_ContentPlaceHolder1_TxtShipingTitle").valid();
                    $("#ctl00_ContentPlaceHolder1_TxtShippingEmail").valid();
                    $("#ctl00_ContentPlaceHolder1_TxtShippingZip").valid();
                    $("#ctl00_ContentPlaceHolder1_TxtShippingMobile").valid();
                    $("#ctl00_ContentPlaceHolder1_TxtShppingTelephone").valid();
                    $("#ctl00_ContentPlaceHolder1_DrpShipingCoutry").valid();
                    $("#ctl00_ContentPlaceHolder1_DrpShipingState").valid();
                    $("#ctl00_ContentPlaceHolder1_DrpShippingCity").valid();


                    if ($("#ctl00_ContentPlaceHolder1_TxtShippingName").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_TxtShipingTitle").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_TxtShippingEmail").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_TxtShippingZip").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_TxtShippingMobile").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_TxtShppingTelephone").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_DrpShipingCoutry").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_DrpShipingState").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_DrpShippingCity").valid() == '0'
                    ) {
                        RemoveShippingRules();
                        return false;
                    }
                    else {
                        RemoveShippingRules();
                        return true;
                    }

                });

            }); //get


            function RemoveShippingRules() {
                //remove rules for shipping
                $("#ctl00_ContentPlaceHolder1_TxtShippingName").rules("remove");
                $("#ctl00_ContentPlaceHolder1_TxtShipingTitle").rules("remove");
                $("#ctl00_ContentPlaceHolder1_TxtShippingEmail").rules("remove");
                $("#ctl00_ContentPlaceHolder1_TxtShippingZip").rules("remove");
                $("#ctl00_ContentPlaceHolder1_TxtShippingMobile").rules("remove");
                $("#ctl00_ContentPlaceHolder1_TxtShppingTelephone").rules("remove");
                $("#ctl00_ContentPlaceHolder1_DrpShipingCoutry").rules("remove");
                $("#ctl00_ContentPlaceHolder1_DrpShipingState").rules("remove");
                $("#ctl00_ContentPlaceHolder1_DrpShippingCity").rules("remove");

            }


            $("#<%=LnkAppyChanges.ClientID %>").click(function() {

                return $('#aspnetForm').valid();
                RemoveShippingRules();
                return true;
            });

            $(function() {

                if ($('#<%=hdPriPhoto.ClientID%>').val() != "") {

                    $('#aDeletePri').show();
                    var retval = $('#<%=hdPriPhoto.ClientID%>').val();
                    $('#imgPhotoupload').attr('src', '../controller/createthumb.aspx?_ty=user&_path=' + retval + '&_twidth=140&_theight=161');
                    $('#imgPhotoupload').attr('title', retval);
                    $('#imgPhotoupload').parent().attr('href', '../controller/createthumb.aspx?_ty=userlarge&_path=' + retval + '&_twidth=600&_theight=600');

                    //$('#ankit').attr('href', '../../../controller/createthumb.aspx?_ty=user&_path=' + val + '&_twidth=800&_theight=800');
                }
                else
                    $('#aDeletePri').hide();
            });

            //Delete Event
            $('#aDeletePri').click(function() {

                jConfirm(replaceMessageString(objValMsg, "DeleteConfirm", "photo"), "Delete photo", function(RetVal) {
                    if (RetVal) {
                        $('#aDeletePri').hide();
                        $('#dvProgPP').show();

                        $.post("../controller/ajaxopes.aspx", {
                            mode: 'DELPRIPHOTO',
                            imgname: $('#imgPhotoupload').attr('title')
                        },
                        function(sRet) {
                            $('#dvProgPP').hide();
                            if (sRet == 'true') {

                                $('#imgPhotoupload').attr('src', '../controller/createthumb.aspx?_ty=user&_path=employee-photo.gif&_twidth=140&_theight=161');
                                $('#imgPhotoupload').parent().attr('href', '../controller/createthumb.aspx?_ty=userlarge&_path=employee-photo.gif&_twidth=600&_theight=600');

                                $('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value', 'employee-photo.gif');


                                $('#aDeletePri').hide();
                                $('#ctl00_ContentPlaceHolder1_hdPriPhoto').attr('value', '');

                            }
                            else {
                                $('#aDeletePri').show();
                                $.showmsg('dvActionMessagePP', 'error', replaceMessageString(objValMsg, "ProcessError", ""), true);
                            } //if
                        } //function
                    ); //post
                    } //if
                }); //function
            }); //click

            // End Delete Event 

        });         //ready


        function pageLoad(sender, args) {
            {

                assigndesign();
            }
        }




        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }

        function openPriModal() {
            $('#dvUpload').modal({ position: [110, ] });
        }

        
        
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div>
            <table>
                <tr>
                    <td class="upload_img_basic">
                        <div class="upload_photo alignleft">
                            <span class="lt_co"></span><span class="rt_co"></span>
                            <div id="dvPriPhotoContainer" class="upload_photo alignright gallery" runat="server">
                                <a href="../UploadedImages/employeePhoto/employee-photo.gif">
                                    <img id='imgPhotoupload' src="../Images/employee-photo.gif" alt="" />
                                </a>
                            </div>
                        </div>
                        <div class="alignnone">
                        </div>
                        <div class="alignleft upload_btn">
                            <div class="noteIncentex" style="width: 100%; font-size: 12px; border: 1px;">
                                <img src="../Images/lightbulb.gif" style="z-index: -1" alt="note:" />&nbsp;Supported
                                file resolution is 140 X 160 (Width X Height)</div>
                        </div>
                        <div class="alignnone">
                        </div>
                        <div class="alignleft upload_btn">
                            <a class="grey2_btn" title="Delete photo"><span id="aDeletePri">Delete Photo</span>
                            </a>
                        </div>
                        <div class="alignnone">
                        </div>
                        <div class="alignleft upload_btn">
                            <a class="grey2_btn" title="Upload Photo"><span id="aUpload" onclick="openPriModal()">
                                Upload Photo</span></a></div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer10">
        </div>
        <div class="header_bg" style="text-align: left;">
            <div class="header_bgr title_small">
                Users Information</div>
        </div>
        <div class="spacer10">
        </div>
        <div>
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
                                            <span class="input_label">Company</span>
                                            <asp:TextBox ID="TxtCompanyName" runat="server" CssClass="w_label" ReadOnly="true"
                                                TabIndex="1"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upCoutry" runat="server">
                                        <ContentTemplate>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="DrpCountry" runat="server" CssClass="w_label" AutoPostBack="true"
                                                            OnSelectedIndexChanged="DrpCountry_SelectedIndexChanged" TabIndex="4">
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
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Address 1</span>
                                            <asp:TextBox ID="TxtAddress" runat="server" CssClass="w_label" TabIndex="7"></asp:TextBox>
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
                                            <asp:TextBox ID="TxtTelephone" runat="server" CssClass="w_label" TabIndex="10"></asp:TextBox>
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
                                            <asp:TextBox ID="TxtFirstName" runat="server" CssClass="w_label" TabIndex="2"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upState" runat="server">
                                        <ContentTemplate>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="DrpState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DrpState_SelectedIndexChanged"
                                                            TabIndex="5">
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
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Address 2</span>
                                            <asp:TextBox ID="TxtAddress2" runat="server" CssClass="w_label" TabIndex="8"></asp:TextBox>
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
                                            <asp:TextBox ID="TxtMobile" runat="server" CssClass="w_label" TabIndex="11"></asp:TextBox>
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
                                            <asp:TextBox ID="TxtLastName" runat="server" CssClass="w_label" TabIndex="3"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="DrpCity" runat="server" TabIndex="6">
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
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Zip</span>
                                            <asp:TextBox ID="TxtZip" runat="server" CssClass="w_label" TabIndex="9"></asp:TextBox>
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
                                        <div class="form_box shipmax_in">
                                            <span class="input_label min_w">Email</span>
                                            <asp:TextBox ID="TxtEmail" runat="server" CssClass="w_label" TabIndex="12"></asp:TextBox>
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
        <div class="header_bg" style="text-align: left;">
            <div class="header_bgr title_small">
                Billing Information</div>
        </div>
        <div class="spacer10">
        </div>
        <div>
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
                                            <span class="input_label">Company</span>
                                            <asp:TextBox ID="TxtBillingCompanyName" runat="server" CssClass="w_label" TabIndex="13"></asp:TextBox>
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
                                            <asp:TextBox ID="TxtBillingAddress" runat="server" CssClass="w_label" TabIndex="16"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upCity" runat="server">
                                        <ContentTemplate>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="DrpBillingCity" runat="server" TabIndex="19">
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
                            <%--<tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Station</span>
                                            <asp:TextBox ID="TxtStation" runat="server" TabIndex="18" CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>--%>
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
                                            <asp:TextBox ID="TxtCO" runat="server" CssClass="w_label" TabIndex="14"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upBillingCountry" runat="server">
                                        <ContentTemplate>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="DrpBillingCountry" runat="server" AutoPostBack="true" CssClass="w_label"
                                                            OnSelectedIndexChanged="DrpBillingCountry_SelectedIndexChanged" TabIndex="17">
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
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Zip</span>
                                            <asp:TextBox ID="TxtBillingZip" runat="server" CssClass="w_label" TabIndex="20"></asp:TextBox>
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
                                            <asp:TextBox ID="TxtBillingManager" TabIndex="15" runat="server" CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upBillingState" runat="server">
                                        <ContentTemplate>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="DrpBillingState" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DrpBillingState_SelectedIndexChanged"
                                                            TabIndex="18">
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
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="header_bg" style="text-align: left;">
            <div class="header_bgr title_small">
                Shipping Information</div>
        </div>
        <div class="spacer10">
        </div>
        <div>
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
                                            <span class="input_label">Company</span>
                                            <asp:TextBox ID="TxtShippingCompany" runat="server" CssClass="w_label" TabIndex="20"></asp:TextBox>
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
                                            <asp:TextBox ID="TxtShipingAddress" runat="server" CssClass="w_label" TabIndex="23"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="DrpShippingCity" runat="server" TabIndex="26">
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
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Mobile</span>
                                            <asp:TextBox ID="TxtShippingMobile" runat="server" CssClass="w_label" TabIndex="29"></asp:TextBox>
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
                                            <asp:TextBox ID="TxtShippingName" runat="server" CssClass="w_label" TabIndex="21"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upShippingCountry" runat="server">
                                        <ContentTemplate>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="DrpShipingCoutry" runat="server" CssClass="w_label" OnSelectedIndexChanged="DrpShipingCoutry_SelectedIndexChanged"
                                                            AutoPostBack="true" TabIndex="24">
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
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Zip</span>
                                            <asp:TextBox ID="TxtShippingZip" runat="server" CssClass="w_label" TabIndex="27"></asp:TextBox>
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
                                            <asp:TextBox ID="TxtShippingEmail" runat="server" CssClass="w_label" TabIndex="30"></asp:TextBox>
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
                                            <asp:TextBox ID="TxtShipingFAX" runat="server" CssClass="w_label" TabIndex="22"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upShippingState" runat="server">
                                        <ContentTemplate>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="DrpShipingState" runat="server" OnSelectedIndexChanged="DrpShipingState_SelectedIndexChanged"
                                                            AutoPostBack="true" TabIndex="25">
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
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Telephone</span>
                                            <asp:TextBox ID="TxtShppingTelephone" runat="server" CssClass="w_label" TabIndex="28"></asp:TextBox>
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
                                            <span class="input_label">Address Name</span>
                                            <asp:TextBox ID="TxtShipingTitle" runat="server" CssClass="w_label" TabIndex="31"></asp:TextBox>
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
                    <td class="formtd">
                        &nbsp;
                    </td>
                    <td class="formtd">
                        &nbsp;
                    </td>
                    <td style="text-align: right; white-space: nowrap;">
                        <asp:LinkButton ID="lnkBtnAddAnotherShiping" class="grey2_btn" runat="server" OnClick="lnkBtnAddAnotherShiping_Click"
                            TabIndex="32"><span>Save & Add New Shipping Address</span></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <%--<asp:UpdatePanel ID="upPanel" runat="server">
                            <ContentTemplate>--%>
                        <asp:GridView ID="dtlShipping" runat="server" HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box"
                            GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                            OnRowCommand="dtlShipping_RowCommand">
                            <Columns>
                                <asp:TemplateField Visible="False" HeaderText="Id">
                                    <HeaderTemplate>
                                        <span>Company Contact ID</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCompanyID" Text='<%# Eval("CompanyContactInfoID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Company">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkCompany" runat="server" CommandArgument="Company" CommandName="Sort"><span>Company</span></asp:LinkButton>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="first">
                                            <asp:LinkButton ID="hypCompany" CommandName="EditRec" CommandArgument='<%# Eval("CompanyContactInfoID") %>'
                                                runat="server" ToolTip="Click here to Edit Shipping"><%# Eval("CompanyName")%></asp:LinkButton>
                                        </span>
                                        <div class="corner">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Title">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkTitle" runat="server" CommandArgument="Title" CommandName="Sort"><span>Address Name</span></asp:LinkButton>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblTitle" Text='<%# Eval("Title") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Email">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkEmail" runat="server" CommandArgument="Email" CommandName="Sort"><span>Email</span></asp:LinkButton>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblEmail" Text='<%# Eval("Email") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span>Delete</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="btn_space">
                                            <asp:ImageButton ID="lnkbtndelete" CommandName="DeleteRec" OnClientClick="return DeleteConfirmation();"
                                                CommandArgument='<%# Eval("CompanyContactInfoID") %>' runat="server" ImageUrl="~/Images/close.png" /></span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </td>
                </tr>
            </table>
        </div>
        <div class="header_bg" style="text-align: left;">
            <div class="header_bgr title_small">
                Change Password</div>
        </div>
        <div class="spacer10">
        </div>
        <div>
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
                                            <span class="input_label">Old Password</span>
                                            <asp:TextBox ID="TxtOldPassword" runat="server" TextMode="Password" TabIndex="33"
                                                CssClass="w_label"></asp:TextBox>
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
                                            <span class="input_label">New Password</span>
                                            <asp:TextBox ID="TxtNewPassword" runat="server" TextMode="Password" TabIndex="34"
                                                CssClass="w_label"></asp:TextBox>
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
                                            <span class="input_label">Confirm Password</span>
                                            <asp:TextBox ID="TxtConfirmNewPassword" runat="server" TabIndex="35" TextMode="Password"
                                                CssClass="w_label"></asp:TextBox>
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
                    <td class="formtd">
                        &nbsp;
                    </td>
                    <td class="formtd">
                        &nbsp;
                    </td>
                    <td class="alignright">
                        <asp:LinkButton ID="lnkChangePassword" class="grey2_btn" runat="server" OnClick="lnkChangePassword_Click"
                            TabIndex="32"><span>Change Password</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer20">
        </div>
        <div class="centeralign">
            <asp:LinkButton ID="LnkAppyChanges" class="grey2_btn" runat="server" OnClick="LnkAppyChanges_Click"
                TabIndex="37"><span>Apply Changes</span></asp:LinkButton>
        </div>
    </div>
    <input type="hidden" id="hdPriPhoto" runat="server" value="" />
</asp:Content>
<asp:Content ID="cc" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div id="dvUpload" style="display: none;">
        <form id="frmUploadPri" method="post" action="../controller/uploadMySettingsPhoto.aspx"
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
                <img src="../Images/ajaxbtn.gif" alt="Please wait..." />&nbsp;Uploading...
            </div>
        </div>

        <script language="javascript" type="text/javascript">
            //setting allowed image file types
            var hash = { 'jpg': 1, 'gif': 1, 'jpeg': 1, 'png': 1, 'bmp': 1 };

            function checkValid() {

                $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
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

                $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
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

                                    $.post("../controller/ajaxopes.aspx", {
                                        mode: 'DELPRIPHOTO',
                                        imgname: $('#imgPhotoupload').attr('title')
                                    },
                                        function(sRet) {
                                        }
                                    );

                                }

                                $('#imgPhotoupload').attr('src', '../controller/createthumb.aspx?_ty=user&_path=' + retval + '&_twidth=140&_theight=161');
                                $('#imgPhotoupload').attr('title', retval);
                                $('#imgPhotoupload').parent().attr('href', '../controller/createthumb.aspx?_ty=userlarge&_path=' + retval + '&_twidth=600&_theight=600');

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
