<%@ Page Title="Employee Training Center" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="AddEmployeeTrainingCenter.aspx.cs" Inherits="admin_EmployeeTrainingCenter_AddEmployeeTrainingCenter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../../JS/JQuery/jquery.MultiFile.pack.js" type="text/javascript" language="javascript"></script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        var formats = 'mp4|mov';
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $.validator.addMethod("filesize", function(value, element, param) {
                    var size = element.files[0].size;
                    if ((size / 1048576) > param)
                        return false;
                    else
                        return true;
                }, "The file you are uploading is more than *MB.");

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlEmployeeTrainingType: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtFileName: { required: true },
                        ctl00$ContentPlaceHolder1$txtSearchKeyword: { required: true },
                        ctl00$ContentPlaceHolder1$txtYouTubeVideoID: { required: true },
                        ctl00$ContentPlaceHolder1$fileupload: { required: true, accept: formats, filesize: "150" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlEmployeeTrainingType: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Traing Type") },
                        ctl00$ContentPlaceHolder1$txtFileName: { required: replaceMessageString(objValMsg, "Required", "File Name") },
                        ctl00$ContentPlaceHolder1$txtSearchKeyword: { required: replaceMessageString(objValMsg, "Required", "Search Keyword") },
                        ctl00$ContentPlaceHolder1$txtYouTubeVideoID: { required: replaceMessageString(objValMsg, "Required", "youtube video id") },
                        ctl00$ContentPlaceHolder1$fileupload: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported.", filesize: "Please select file less than 150MB." }

                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlEmployeeTrainingType")
                            error.insertAfter("#dvEmployeeTrainingType");
                        else
                            error.insertAfter(element);
                    },
                    onsubmit: true
                });
            });

            $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                $('#dvLoader').show();
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad select_box_pad" style="width: 400px;">
        <div class="form_table">
            <div class="spacer10">
            </div>
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <div class="spacer10">
            </div>
            <table>
                <tr id="trDocumentType" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Training Video Type</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlEmployeeTrainingType" onchange="pageLoad(this,value);" runat="server"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvEmployeeTrainingType">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trFileName" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">File Name </span>
                                <asp:TextBox ID="txtFileName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trSearchKeyword" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Search Words </span>
                                <asp:TextBox ID="txtSearchKeyword" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="wheather_check" runat="server" id="spnUploadToWL" style="width: 40px;
                                height: 43px; margin-left:0px;">
                                <asp:CheckBox ID="chkUploadToWL" AutoPostBack="true" runat="server" OnCheckedChanged="chkUploadToWL_CheckedChanged" Text="Upload to WL"  />
                            </div>
                            <span style="float: left; margin-top:-31px; margin-left:60px;  position:absolute; color: #72757C; line-height: 18px;">Upload to WL</span>
                            <div class="wheather_check" runat="server" id="spnUploadToYouTube" style="width: 40px;
                                height: 43px; margin-top:-43px;  margin-left:165px;">
                                <asp:CheckBox ID="chkUploadToYouTube" AutoPostBack="true" runat="server" OnCheckedChanged="chkUploadToYouTube_CheckedChanged" />
                            </div>
                            <span style="float: left; margin-top:-31px; margin-left:225px;  position:absolute; color: #72757C; line-height: 18px;">Upload to YouTube</span>
                        </div>
                    </td>
                </tr>
                <tr id="trUploadToWL" runat="server" visible="false">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Upload Files </span>
                                <input type="file" id="fileupload" style="margin: -24px 0px 8px 150px" runat="server"
                                    tabindex="6" />
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trUploadToYoutube" visible="false">
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%">YouTube Video ID</span>
                                <asp:TextBox ID="txtYouTubeVideoID" CssClass="w_label" runat="server" />
                                <div style="position: absolute; left: 349px; top: 0px;">
                                    <a target="_blank" href="http://www.youtube.com/my_videos_upload" title="Login to youtube account">
                                        <img style="width: 38px;" src="../../Images/YouTubeLogo.png" alt="youtub" /></a>
                                </div>
                                <div style="position: absolute; left: 388px; top: 0px; width: 100%; color: #72757C;">
                                    User Name : incentexworldlink@gmail.com<br />
                                    Password : incentex
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="gredient_btn" runat="server" ToolTip="Save Document"
                            OnClick="lnkBtnSaveInfo_Click"><span>Save</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
