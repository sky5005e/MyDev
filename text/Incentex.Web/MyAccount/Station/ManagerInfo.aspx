<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManagerInfo.aspx.cs" Inherits="admin_Company_Station_AddManagerInfo"
 %>
<%--<%@ Register Src="~/admin/Company/Station/StationSubMenu.ascx" TagName="StationSubManu" TagPrefix="uc" %>--%>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="uc" %>

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
    </style>
 <script language="javascript" type="text/javascript">


        $(function() {
           // scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");
            scrolltextarea(".scrollme2", "#Scrolltop2", "#ScrollBottom2");
            scrolltextarea(".scrollme3", "#Scrolltop3", "#ScrollBottom3");
            //scrolltextarea(".scrollme4", "#Scrolltop4", "#ScrollBottom4");
            //scrolltextarea(".scrollme5", "#Scrolltop5", "#ScrollBottom5");
            //scrolltextarea(".scrollme6", "#Scrolltop6", "#ScrollBottom6");

        });


        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'Date',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });

        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {

                objValMsg = $.xml2json(xml);
                //alert(objValMsg);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtFirstName: { required: true, alphanumeric: true }
                    , ctl00$ContentPlaceHolder1$txtLastName: { required: true, alphanumeric: true }
                    , ctl00$ContentPlaceHolder1$txtTitle: { required: true, alphanumeric: true }
                    , ctl00$ContentPlaceHolder1$txtTel: { required: true, alphanumeric: true }
                    , ctl00$ContentPlaceHolder1$txtMobile: { required: true, alphanumeric: true }
                     , ctl00$ContentPlaceHolder1$txtEmail: { required: true, email: true }
                    , ctl00$ContentPlaceHolder1$txtEstimatedDateHired: { date: true }
                    , ctl00$ContentPlaceHolder1$txtBirthdate: { date: true }
                    }
                    , messages:
                    {
                        ctl00$ContentPlaceHolder1$txtFirstName: {
                            required: replaceMessageString(objValMsg, "Required", "first name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        }
                        , ctl00$ContentPlaceHolder1$txtLastName: {
                            required: replaceMessageString(objValMsg, "Required", "last name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        }
                          , ctl00$ContentPlaceHolder1$txtTitle: { required: replaceMessageString(objValMsg, "Required", "title"),
                              alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                          }
                     , ctl00$ContentPlaceHolder1$txtTel: {
                       required: replaceMessageString(objValMsg, "Required", "tel")
                            , alphanumeric: replaceMessageString(objValMsg, "Number", "")
                   }
                    , ctl00$ContentPlaceHolder1$txtMobile: {
                        required: replaceMessageString(objValMsg, "Required", "mobile"),
                        alphanumeric: replaceMessageString(objValMsg, "Number", "")
                    }
                        , ctl00$ContentPlaceHolder1$txtEmail: {
                            required: replaceMessageString(objValMsg, "Required", "email"),
                            email: replaceMessageString(objValMsg, "Email", "")
                        }
                        , ctl00$ContentPlaceHolder1$txtEstimatedDateHired: { date: replaceMessageString(objValMsg, "ValidDate", "") }
                        , ctl00$ContentPlaceHolder1$txtBirthdate: { date: replaceMessageString(objValMsg, "ValidDate", "") }
                    }


                }); //validate
            }); //get

            $("#<%=lnkSave.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            }); //click

        });   //ready

        $('#dvPriPhotoMain').click(function() {
            $('#dvPriPhoto').toggle(400);
        });


        function openPriModal() {
            $('#dvUpload').modal({ position: [110, ] });
        }

        $(function() {
            //alert($('#<%=hdPriPhoto.ClientID%>').val());
            if ($('#<%=hdPriPhoto.ClientID%>').val() != "") {
                $('#aDeletePri').show();
                $('#imgPhotoupload').attr('src', '../../UploadedImages/stationuserPhoto/' + $('#<%=hdPriPhoto.ClientID%>').val());
                $('#imgPhotoupload').parent().attr('href', '../../controller/createthumb.aspx?_ty=station&_path=' + $('#<%=hdPriPhoto.ClientID%>').val() + '&_twidth=800&_theight=800');

            }
            else
                $('#aDeletePri').hide();
        });

         function aDeletePri_click() {
             //alert("del");
             jConfirm(replaceMessageString(objValMsg, "DeleteConfirm", "photo"), "Delete photo", function(RetVal) {
                 if (RetVal) {
                     $('#aDeletePri').hide();
                     $('#dvProgPP').show();

                     $.post("../../controller/ajaxopes.aspx", {
                     mode: 'DELSTATIONPHOTO',
                         imgname: $('#imgPhotoupload').attr('title')
                     },
                        function(sRet) {
                            $('#dvProgPP').hide();
                            if (sRet == 'true') {

                                $('#imgPhotoupload').attr('src', '../../controller/createthumb.aspx?_ty=station&_path=employee-photo.gif&_twidth=140&_theight=161');
                                $('#imgPhotoupload').parent().attr('href', '../../controller/createthumb.aspx?_ty=station&_path=employee-photo.gif&_twidth=800&_theight=800');

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
        }
        
 </script>

<style type="text/css">
    .form_table .child_text .textarea_box {
        height:30px;
        width:60%;
    }
    div.ppt 
    {
    	position:relative;
    }
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<%--<uc:StationSubManu runat="server" ID="UcSubMenu" StationInfo="ManagerInfo_2" />--%>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<uc:MenuUserControl ID="manuControl" runat="server" />

<asp:HiddenField ID="hdPriPhoto" runat="server" />
    <div class="form_pad">
        <div class="form_table addedit_pad">
        <div style="text-align:center" >
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" ></asp:Label>
                </div>
            <h4>
                Station Manager Information</h4>
            <div>
                <table>
                    <tr>
                        <td class="formtd_left">
                            <table>
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">First Name</span>
                                                <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
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
                                                <asp:TextBox id="txtLastName" runat="server"></asp:TextBox>                                                
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
                                                <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
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
                                                <asp:TextBox ID="txtTel" runat="server"></asp:TextBox>
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
                                                <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
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
                                                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="formtd_right">
                            <table>
                                <tr>
                                    <td class="upload_img_basic">
                                        <div class="alignleft item">
                                            <div class="upload_photo">
                                                <span class="lt_co"></span><span class="rt_co"></span><span class="lb_co"></span>
                                                <span class="rb_co"></span>
                                                <div id="dvPriPhotoContainer" class="upload_photo  gallery" runat="server">
                                                        <a href="../../images/upload-img-splash.jpg" rel='prettyPhoto'>
                                                            <img src="../../images/upload-img-splash.jpg" width='140' height='161' id="imgPhotoupload" alt="" /></a>
                                                 </div>
                                            </div>
                                            <div class="upload_btn">
                                                <a class="grey2_btn" title="Delete photo"><span id="aDeletePri" onclick="javascript:aDeletePri_click()">
                                                    Delete photo</span> </a>
                                            </div>
                                              <div class="alignleft upload_btn">
                                                <a class="grey2_btn" title="Upload Photo"><span id="aUpload" onclick="openPriModal()">
                                                    Upload Photo</span></a>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <h4>
                Personal Information</h4>
            <div>
                <table>
                    <tr>
                        <td class="formtd_left">
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box shipmax_in">
                                    <span class="input_label max_w">Estimated Date Hired</span> 
                                    <span class="outer_dateimg">
                                        <%--<input type="text" class="datepicker w_label min_w" />--%>
                                        <asp:TextBox ID="txtEstimatedDateHired" runat="server"
                                        CssClass="datepicker w_label min_w" ></asp:TextBox>
                                        
                                        </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                        <td class="formtd_right">
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box clearfix employer_text">
                                    <span class="input_label alignleft">Previous Employer</span>
                                    <div class="textarea_box alignright">
                                        <div class="scrollbar">
                                            <a href="#scroll" id="Scrolltop2" class="scrolltop"></a>
                                            <a href="#scroll" id="ScrollBottom2" class="scrollbottom"></a>
                                        </div>
                                        <asp:TextBox ID="txtPreviousEmployer" runat="server" CssClass="scrollme2" 
                                            TextMode="MultiLine" Rows="2"></asp:TextBox>
                                        <%--<textarea name="" cols="" rows="" class="scrollme2"></textarea>--%>
                                    </div>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td class="formtd_left">
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Spouse Name</span>
                                                <asp:TextBox ID="txtSpouseName"  CssClass="w_label"  runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                    <td class="formtd_right">
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Birthdate</span>
                                                <span class="outer_dateimg">
                                                    <asp:TextBox ID="txtBirthdate" CssClass="datepicker w_label min_w" runat="server"></asp:TextBox>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                    </tr>
                                    <tr>
                                    <td class="formtd_left">
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box clearfix child_text">
                                                <span class="input_label alignleft">Childrens Name</span>
                                                <div class="textarea_box alignright">
                                                    <div class="scrollbar">
                                                        <a href="#scroll" id="Scrolltop3" class="scrolltop"></a><a href="#scroll" id="ScrollBottom3"
                                                            class="scrollbottom"></a>
                                                    </div>
                                                    <%--<textarea name="" cols="" rows="" class="scrollme3"></textarea>--%>
                                                    <asp:TextBox ID="txtChildrensName" runat="server" CssClass="scrollme3" 
                                                        TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                    
                                                </div>
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
            	<div class="alignnone spacer25"></div>
						<div class="additional_btn">
							<ul class="clearfix">
								<li>
								    <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" 
                                        onclick="lnkSave_Click">
								        <span>Save Information</span>
								    </asp:LinkButton>
								</li>
							</ul>
						</div>
        </div>
    </div>
    
    
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder3" runat="server" ID="PopUp">
 <!-- Upload Dialog -->
    <div id="dvUpload" style="display: none;">
        <form id="frmUploadPri" method="post" action="../../admin/controller/UploadPhoto.aspx"
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
                <img src="../../Images/lightbulb.gif" alt="note:" />&nbsp;Please upload your photo. You
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
                <img src="../../Images/ajaxbtn.gif" alt="Please wait..." />&nbsp;Uploading...
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

                                    $.post("../../controller/ajaxopes.aspx", {
                                        mode: 'DELSTATIONPHOTO',
                                        imgname: $('#imgPhotoupload').attr('title')
                                    },
                                        function(sRet) {
                                        }
                                    );

                                }

                                $('#imgPhotoupload').attr('src', '../../controller/createthumb.aspx?_ty=station&_path=' + retval + '&_twidth=140&_theight=161');
                                $('#imgPhotoupload').attr('title', retval);
                                $('#imgPhotoupload').parent().attr('href', '../../controller/createthumb.aspx?_ty=station&_path=' + retval + '&_twidth=800&_theight=800');

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
      
    <!-- Dialog End -->
</asp:Content>
