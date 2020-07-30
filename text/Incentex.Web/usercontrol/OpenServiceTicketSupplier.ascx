<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OpenServiceTicketSupplier.ascx.cs"
    Inherits="usercontrol_OpenServiceTicketSupplier" Debug="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>

<script type="text/javascript" language="javascript">
    $().ready(function() {
        $("#dvLoader").show();
        $.get('<%=ConfigurationSettings.AppSettings["siteurl"] %>JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
            objValMsg = $.xml2json(xml);
            $("#aspnetForm").validate({
                rules: {
                    ctl00$ContentPlaceHolder1$ostSPControl$txtServiceTicketName: { required: true },
                    ctl00$ContentPlaceHolder1$ostSPControl$ddlServiceTicketOwner: { NotequalTo: "0" },
                    ctl00$ContentPlaceHolder1$ostSPControl$txtQuestion: { required: true }
                    //,ctl00$ContentPlaceHolder1$ostSPControl$txtDatePromised: { required: true }
                },
                messages: {
                    ctl00$ContentPlaceHolder1$ostSPControl$txtServiceTicketName: { required: replaceMessageString(objValMsg, "Required", "support ticket subject") },
                    ctl00$ContentPlaceHolder1$ostSPControl$ddlServiceTicketOwner: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "support ticket owner") },
                    ctl00$ContentPlaceHolder1$ostSPControl$txtQuestion: { required: replaceMessageString(objValMsg, "Required", "your request") }
                    //,ctl00$ContentPlaceHolder1$ostSPControl$txtDatePromised: { required: replaceMessageString(objValMsg, "Required", "date needed") }
                },
                errorPlacement: function(error, element) {
                    if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostSPControl$txtServiceTicketName") {                        
                        error.insertAfter("#dvTicketName");
                    }
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostSPControl$ddlServiceTicketOwner")
                        error.insertAfter("#dvServiceTicketOwner");
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostSPControl$txtQuestion")
                        error.insertAfter("#dvRequest");
                    //else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostSPControl$txtDatePromised")
                        //error.insertAfter("#dvDatePromised");
                    else
                        error.insertAfter(element);
                }
            });
        });

        $("#ctl00_ContentPlaceHolder1_ostSPControl_btnSubmitTicketSP").click(function() {
            $('#dvLoader').show();            
            if ($("#aspnetForm").valid()) {                
                return true;

            }
            else {
                $('#dvLoader').hide();                
                return false;
            }
        });
        
        var focusfirsttime = true;
        $("#ctl00_ContentPlaceHolder1_ostSPControl_pnlOpenServiceTicketSP").mouseover(function() {
            if (focusfirsttime) {                
                $("#ctl00_ContentPlaceHolder1_ostSPControl_txtServiceTicketName").focus();                
                focusfirsttime = false;
            }
        });
    });

    $(function() {
        $(".datepicker1").datepicker({
            buttonText: 'DatePicker',
            showOn: 'button',
            buttonImage: '<%=ConfigurationSettings.AppSettings["siteurl"] %>CSS/images/calendar.png',
            buttonImageOnly: true,
            minDate: new Date()
        });
    });
</script>

<script type="text/javascript" language="javascript">
    function pageLoad(sender, args) {
        {
            assigndesign();
        }
    }
</script>

<script src="../JS/JQuery/jquery.MultiFile.pack.js" type="text/javascript" language="javascript"></script>

<link media="screen" rel="stylesheet" href='<%=ConfigurationSettings.AppSettings["siteurl"] %>CSS/colorboxpopup.css' />
<input type="hidden" id="hfPopX" value="0" runat="server" />
<input type="hidden" id="hfPopY" value="0" runat="server" />
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
    #container_popup .mid
    {
        width: 345px;
    }
</style>

<script type="text/javascript">
    $(window).load(function() {
        $("#dvLoader").hide();
    });
</script>

<div id="dvLoader">
    <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
    </div>
    <div class="updateProgressDiv">
        <img alt="Loading" src="../Images/ajax-loader-large.gif" />
    </div>
</div>
<asp:LinkButton ID="lnkDummyServiceTicketSP" class="grey2_btn alignright" runat="server"
    Style="display: none"></asp:LinkButton>
<at:ModalPopupExtender BehaviorID="mpOpenServiceTicketSPBehaviour" ID="mpOpenServiceTicketSP"
    TargetControlID="lnkDummyServiceTicketSP" BackgroundCssClass="modalBackground"
    DropShadow="false" runat="server" CancelControlID="cboxClose" PopupControlID="pnlOpenServiceTicketSP">
</at:ModalPopupExtender>
<asp:PlaceHolder runat="server" ID="phScriptManager"></asp:PlaceHolder>
<asp:Panel ID="pnlOpenServiceTicketSP" runat="server" Style="display: none;">
    <div id="cboxWrapper" style="min-height: 597px; width: 420px; position: fixed; left: 36.4%;
        top: 10%;">
        <div style="">
            <div id="cboxTopLeft" style="float: left;">
            </div>
            <div id="cboxTopCenter" style="float: left; width: 420px;">
            </div>
            <div id="cboxTopRight" style="float: left;">
            </div>
        </div>
        <div style="clear: left;">
            <div id="cboxMiddleLeft" style="float: left; min-height: 597px;">
            </div>
            <div id="cboxContent" style="float: left; width: 420px; min-height: 597px;">
                <div id="cboxLoadedContent" style="display: block; width: 420px; overflow: hidden;
                    min-height: 569px;">
                    <div id="help2">
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
                                            <td class="alignleft mid">
                                                <form action="" class="MultiFile-intercepted">
                                                <fieldset>
                                                    <ul class="form-box">
                                                        <li>
                                                            <label>
                                                                Subject:</label>
                                                            <div class="inputvalue">
                                                                <span class="nftextLeft"></span>
                                                                <asp:TextBox ID="txtServiceTicketName" TabIndex="0" runat="server" CssClass="nftextright"></asp:TextBox>
                                                                <div id="dvTicketName">
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li>
                                                            <label>
                                                                Assign To:</label>
                                                            <div class="inputvalue">
                                                                <span class="nftextLeft"></span><span class="custom-sel">
                                                                    <asp:DropDownList ID="ddlServiceTicketOwner" TabIndex="1" onchange="pageLoad(this,value);"
                                                                        runat="server">
                                                                    </asp:DropDownList>
                                                                </span>
                                                                <div id="dvServiceTicketOwner">
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li>
                                                            <label>
                                                                Your Request:</label>
                                                            <div class="inputvalue">
                                                                <span class="nftextareaLeft"></span>
                                                                <%--<textarea cols="38" rows="10" size="50" name="ur-request" class="nftextarearight"></textarea>--%>
                                                                <asp:TextBox ID="txtQuestion" MaxLength="500" TabIndex="2" TextMode="MultiLine" CssClass="nftextarearight"
                                                                    runat="server"></asp:TextBox>
                                                                <div id="dvRequest">
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li>
                                                            <label for="date">
                                                                Date Needed:</label>
                                                            <div class="inputvalue">
                                                                <span class="nftextLeft"></span>
                                                                <asp:TextBox ID="txtDatePromised" runat="server" TabIndex="3" Width="300px" CssClass="datepicker1 nftextright"></asp:TextBox>
                                                                <div id="dvDatePromised" style="clear: both;">
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li>
                                                            <label>
                                                                Attachment(s):</label>
                                                            <div class="inputvalue">
                                                                <div id="MultiFile1_wrap" class="MultiFile-wrap">
                                                                    <%--<fjx:FileUploader ID="FileUploader1" MaxFileSize="5MB" MaxNumberFiles="10" MaxFileQueueSize="20MB"
                                                                    runat="server">
                                                                </fjx:FileUploader>--%>
                                                                    <input type="file" id="flAttachment" runat="server" tabindex="4" class="multi max-3 accept-gif|jpg|xlsx|xls|doc|docx|pdf|png"
                                                                        size="37" />
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li style="padding-bottom: 15px;">
                                                            <div class="alignright">
                                                                <label class="popup-button">
                                                                    <asp:Button ID="btnSubmitTicketSP" Text="Submit" TabIndex="5" runat="server" OnClick="btnSubmitTicketSP_Click" />
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
                <div id="cboxLoadingOverlay" style="min-height: 597px; display: none;" class="">
                </div>
                <div id="cboxLoadingGraphic" style="min-height: 597px; display: none;" class="">
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
            <div id="cboxMiddleRight" style="float: left; min-height: 597px;">
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
