<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadPhotoTest.aspx.cs" Inherits="admin_Company_Station_UploadPhotoTest" %>
<%@ Register Src="~/UserControl/CommonHead.ascx" TagPrefix="uc" TagName="CommonHead" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
            <uc:CommonHead ID="ucCommonHead" runat="server" />
            
 <script type="text/javascript" language="javascript">
     function openPriModal() {
         $('#dvUpload').modal({ position: [110, ] });
     }
 </script>  
 
</head>
<body>

    <form id="form1" runat="server">
    <div>
        <div class="top_songs_midbg" id="dvPriPhoto">
            <div>
                <span class="lt_co"></span><span class="rt_co"></span><span class="lb_co"></span>
                <span class="rb_co"></span>
                <div id="dvActionMessagePP">
                </div>
                <div id="dvPriPhotoContainer" class="upload_photo alignright gallery" runat="server">
                    <a href="../../../UploadedImages/employeePhoto/employee-photo.gif" rel='prettyPhoto'>
                        <img id='imgPhotoupload' src="../../../UploadedImages/employeePhoto/employee-photo.gif" width='140'
                            height='161' /></a>
                </div>
                <div class="alignright upload_btn">
                    <a class="grey2_btn" title="Delete photo"><span id="aDeletePri">Delete photo</span>
                    </a>
                </div>
                <div class="alignright upload_btn">
                    <a class="grey2_btn" title="Upload Photo"><span id="aUpload" onclick="openPriModal()">
                        Upload Photo</span></a>
                </div>
            </div>
        </div>
    </div>
    </form>
    
    
    <div id="dvUpload" style="display: none;">
        <form id="frmUploadPri" method="post" action="UploadPhoto.aspx"
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
                <img src="../../../Images/ajaxbtn.gif" alt="Please wait..." />&nbsp;Uploading...
            </div>
        </div>

        <script language="javascript" type="text/javascript">
            //setting allowed image file types
            var hash = { 'jpg': 1, 'gif': 1, 'jpeg': 1, 'png': 1, 'bmp': 1 };

            function checkValid() {
                //alert("validate");
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
                //alert("OK");
                var btnUpload = $('#btnUploadPri');
                var progstatus = $('#imgProcess');

                $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                    objValMsg = $.xml2json(xml);

                    $('#frmUploadPri').ajaxSubmit({
                        data: {
                            mode: 'priphoto',Pg:"SignUp"
                        },
                        beforeSubmit: function(a, f, o) {
                        //alert("OK2");
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

                                $('#imgPhotoupload').attr('src', 'controller/createthumb.aspx?_ty=user&_path=' + retval + '&_twidth=140&_theight=161');
                                $('#imgPhotoupload').attr('title', retval);
                                $('#imgPhotoupload').parent().attr('href', 'controller/createthumb.aspx?_ty=user&_path=' + retval + '&_twidth=800&_theight=800');

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
    
</body>
</html>
