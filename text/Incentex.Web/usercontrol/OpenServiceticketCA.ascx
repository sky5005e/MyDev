<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OpenServiceticketCA.ascx.cs"
    Inherits="usercontrol_OpenServiceticketCA" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>

<script type="text/javascript">
    $(window).load(function() {
        $("#dvLoader").hide();
    });
</script>

<script type="text/javascript" language="javascript">
    function pageLoad(sender, args) {
        {
            assigndesign();
        }
    }
</script>

<script type="text/javascript" language="javascript">
    $().ready(function() {
        $("#dvLoader").show();
        $.get('<%=ConfigurationSettings.AppSettings["siteurl"] %>JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
            objValMsg = $.xml2json(xml);
            $("#aspnetForm").validate({
                rules: {
                    ctl00$ContentPlaceHolder1$ostCAControl$txtLoginEmail: { required: true },
                    ctl00$ContentPlaceHolder1$ostCAControl$txtSubject: { required: true },
                    ctl00$ContentPlaceHolder1$ostCAControl$ddlServiceTicketOwner: { NotequalTo: "0" },
                    ctl00$ContentPlaceHolder1$ostCAControl$txtQuestion: { required: true }
                },
                messages: {
                    ctl00$ContentPlaceHolder1$ostCAControl$txtLoginEmail: { required: replaceMessageString(objValMsg, "Required", "your login email") },
                    ctl00$ContentPlaceHolder1$ostCAControl$txtSubject: { required: replaceMessageString(objValMsg, "Required", "support ticket subject") },
                    ctl00$ContentPlaceHolder1$ostCAControl$ddlServiceTicketOwner: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "support ticket owner") },
                    ctl00$ContentPlaceHolder1$ostCAControl$txtQuestion: { required: replaceMessageString(objValMsg, "Required", "your question") }
                },
                errorPlacement: function(error, element) {
                    if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostCAControl$txtLoginEmail")
                        error.insertAfter("#dvLoginEmail");
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostCAControl$txtSubject")
                        error.insertAfter("#dvSubject");
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostCAControl$ddlServiceTicketOwner")
                        error.insertAfter("#dvServiceTicketOwner");
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostCAControl$txtQuestion")
                        error.insertAfter("#dvQuestion");
                    else
                        error.insertAfter(element);
                }
            });
        });

        $("#ctl00_ContentPlaceHolder1_ostCAControl_btnSubmitTicketCA").click(function() {
            $('#dvLoader').show();
            if ($("#aspnetForm").valid()) {
                return true;
            }
            else {
                $('#dvLoader').hide();
                return false;
            }
        });
        
        $("#ctl00_ContentPlaceHolder1_ostCAControl_txtSubject").focus();
    });
</script>

<script src="JS/JQuery/jquery.MultiFile.pack.js" type="text/javascript" language="javascript"></script>

<link media="screen" rel="stylesheet" href='<%=ConfigurationSettings.AppSettings["siteurl"] %>CSS/colorboxpopup.css' />
<style type="text/css">
    #cboxClose
    {
        right: 50px;
    }
    #popup_header div#headercontent
    {
        width: 310px;
    }
    .nftextright
    {
        height: 35px;
        line-height: 20px;
        width: 333px;
    }
    .nftextarearight
    {
        line-height: 20px;
        width: 333px;
    }
</style>



<div id="dvLoader">
    <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
    </div>
    <div class="updateProgressDiv">
        <img alt="Loading" src="Images/ajax-loader-large.gif" />
    </div>
</div>
<asp:LinkButton ID="lnkDummyServiceTicketCA" class="grey2_btn alignright" runat="server"
    Style="display: none"></asp:LinkButton>
<at:ModalPopupExtender BehaviorID="mpOpenServiceTicketCABehaviour" ID="mpOpenServiceTicketCA"
    TargetControlID="lnkDummyServiceTicketCA" BackgroundCssClass="modalBackground"
    DropShadow="false" runat="server" CancelControlID="cboxClose" PopupControlID="pnlOpenServiceTicketCA">
</at:ModalPopupExtender>
<asp:PlaceHolder runat="server" ID="phScriptManager"></asp:PlaceHolder>
<asp:Panel ID="pnlOpenServiceTicketCA" runat="server" Style="display: none;">
    <div id="cboxWrapper" style="height: 640px; width: 420px; position: fixed; left: 36.4%;
        top: 8%;">
        <div style="">
            <div id="cboxTopLeft" style="float: left;">
            </div>
            <div id="cboxTopCenter" style="float: left; width: 420px;">
            </div>
            <div id="cboxTopRight" style="float: left;">
            </div>
        </div>
        <div style="clear: left;">
            <div id="cboxMiddleLeft" style="float: left; height: 767px;">
            </div>
            <div id="cboxContent" style="float: left; width: 420px; height: 767px;">
                <div id="cboxLoadedContent" style="display: block; width: 420px; overflow: hidden;
                    height: 739px;">
                    <div id="help">
                        <div id="popup_wrapper">
                            <div id="popup_header">
                                <div id="headercontent">
                                    <span class="help alignleft">help</span>
                                    <h1 id="logo-popup" class="alignright">
                                        <a href="" title="incentex-logo-popup">incentex-logo</a></h1>
                                    <div id="cboxClose" style="" class="">
                                        close</div>
                                </div>
                            </div>
                            <div id="container_popup">
                                <table cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td class="leftrounded">
                                            </td>
                                            <td class="mid">
                                                <form action="" class="MultiFile-intercepted">
                                                <fieldset>
                                                    <ul class="form-box">
                                                        <li>
                                                            <label>
                                                                Your Login Email:</label>
                                                            <div class="inputvalue">
                                                                <span class="nftextLeft"></span>
                                                                <asp:TextBox ID="txtLoginEmail" runat="server" TabIndex="1" CssClass="nftextright"></asp:TextBox>
                                                                <div id="dvLoginEmail">
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li>
                                                            <label>
                                                                Subject:</label>
                                                            <div class="inputvalue">
                                                                <span class="nftextLeft"></span>
                                                                <asp:TextBox ID="txtSubject" runat="server" TabIndex="1" CssClass="nftextright"></asp:TextBox>
                                                                <div id="dvSubject">
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li>
                                                            <label>
                                                                Assign To:</label>
                                                            <div class="inputvalue">
                                                                <span class="nftextLeft"></span><span class="custom-sel">
                                                                    <asp:DropDownList ID="ddlServiceTicketOwner" TabIndex="2" onchange="pageLoad(this,value);"
                                                                        runat="server">
                                                                    </asp:DropDownList>
                                                                </span>
                                                                <div id="dvServiceTicketOwner">
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li>
                                                            <label>
                                                                Your Question:</label>
                                                            <div class="inputvalue">
                                                                <span class="nftextareaLeft"></span>
                                                                <asp:TextBox ID="txtQuestion" MaxLength="500" TextMode="MultiLine" TabIndex="3" CssClass="nftextarearight"
                                                                    runat="server"></asp:TextBox>
                                                                <div id="dvQuestion">
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li>
                                                            <label>
                                                                Attachment(s):</label>
                                                            <div class="inputvalue">
                                                                <%--<fjx:FileUploader ID="FileUploader1" MaxFileSize="5MB" MaxNumberFiles="10" MaxFileQueueSize="20MB"
                                                                runat="server">
                                                            </fjx:FileUploader>--%>
                                                                <input type="file" id="flAttachment" runat="server" class="multi max-3 accept-gif|jpg|xlsx|xls|doc|docx|pdf|png"
                                                                    size="37" />
                                                            </div>
                                                        </li>
                                                        <li style="padding-bottom: 15px;"><span class="infotxt">Or Call Us at: 772-453-2759</span>
                                                            <div class="alignright">
                                                                <label class="popup-button">
                                                                    <asp:Button ID="btnSubmitTicketCA" Text="Submit" TabIndex="4" runat="server" OnClick="btnSubmitTicketCA_Click" />
                                                                </label>
                                                            </div>
                                                        </li>
                                                    </ul>
                                                </fieldset>
                                                </form>
                                            </td>
                                            <td class="rightrounded">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="cboxLoadingOverlay" style="height: 767px; display: none;" class="">
                </div>
                <div id="cboxLoadingGraphic" style="height: 767px; display: none;" class="">
                </div>
                <div id="cboxTitle" style="display: block;" class="">
                </div>
                <div id="cboxCurrent" style="display: none;" class="">
                </div>
                <div id="cboxNext" style="display: none;" class="">
                </div>
                <div id="cboxPrevious" style="display: none;" class="">
                </div>
                <div id="cboxSlideshow" style="display: none;" class="">
                </div>
            </div>
            <div id="cboxMiddleRight" style="float: left; height: 767px;">
            </div>
        </div>
        <div style="clear: left;">
            <div id="cboxBottomLeft" style="float: left;">
            </div>
            <div id="cboxBottomCenter" style="float: left; width: 420px;">
            </div>
            <div id="cboxBottomRight" style="float: left;">
            </div>
        </div>
    </div>
</asp:Panel>
