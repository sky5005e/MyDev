<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OpenServiceticketCE.ascx.cs"
    Inherits="usercontrol_OpenServiceticketCE" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>

<script type="text/javascript" language="javascript">
    function pageLoad(sender, args) {
        {
            assigndesign();
        }
    }
</script>

<script type="text/javascript">
    $(window).load(function() {
        $("#dvLoader").hide();
    });
</script>

<script type="text/javascript" language="javascript">
    $().ready(function() {
        $("#dvLoader").show();
        $.get('<%=ConfigurationSettings.AppSettings["siteurl"] %>JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
            objValMsg = $.xml2json(xml);
            $("#aspnetForm").validate({
                rules: {
                    ctl00$ContentPlaceHolder1$ostCEControl$txtLoginEmail: { required: true },
                    ctl00$ContentPlaceHolder1$ostCEControl$txtSubject: { required: true },                    
                    ctl00$ContentPlaceHolder1$ostCEControl$ddlServiceTicketReason: { NotequalTo: "0" },
                    ctl00$ContentPlaceHolder1$ostCEControl$txtQuestion: { required: true }
                },
                messages: {
                    ctl00$ContentPlaceHolder1$ostCEControl$txtLoginEmail: { required: replaceMessageString(objValMsg, "Required", "your login email") },                    
                    ctl00$ContentPlaceHolder1$ostCEControl$txtSubject: { required: replaceMessageString(objValMsg, "Required", "support ticket subject") },
                    ctl00$ContentPlaceHolder1$ostCEControl$ddlServiceTicketReason: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "support ticket reason") },
                    ctl00$ContentPlaceHolder1$ostCEControl$txtQuestion: { required: replaceMessageString(objValMsg, "Required", "your question") }
                },
                errorPlacement: function(error, element) {
                    if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostCEControl$txtLoginEmail")
                        error.insertAfter("#dvLoginEmail");                    
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostCEControl$txtSubject")
                        error.insertAfter("#dvSubject");
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostCEControl$ddlServiceTicketReason")
                        error.insertAfter("#dvServiceTicketReason");
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostCEControl$txtQuestion")
                        error.insertAfter("#dvQuestion");
                    else
                        error.insertAfter(element);
                }
            });
        });

        $("#ctl00_ContentPlaceHolder1_ostCEControl_btnSubmitTicketCE").click(function() {
            $('#dvLoader').show();
            if ($("#aspnetForm").valid()) {
                return true;
            }
            else {
                $('#dvLoader').hide();
                return false;
            }
        });
        
        
        $("#ctl00_ContentPlaceHolder1_ostCEControl_txtSubject").focus();
                
    });
</script>

<script type="text/javascript" src="colorbox/jquery.colorbox.js"></script>

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
<asp:LinkButton ID="lnkDummyServiceTicketCE" class="grey2_btn alignright" runat="server"
    Style="display: none"></asp:LinkButton>
<at:ModalPopupExtender BehaviorID="mpOpenServiceTicketCEBehaviour" ID="mpOpenServiceTicketCE"
    TargetControlID="lnkDummyServiceTicketCE" BackgroundCssClass="modalBackground"
    DropShadow="false" runat="server" CancelControlID="cboxClose" PopupControlID="pnlOpenServiceTicketCE">
</at:ModalPopupExtender>
<asp:PlaceHolder runat="server" ID="phScriptManager"></asp:PlaceHolder>
<asp:Panel ID="pnlOpenServiceTicketCE" runat="server" Style="display: none;">
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
            <div id="cboxMiddleLeft" style="float: left; height: 767px; min-height">
            </div>
            <div id="cboxContent" style="float: left; width: 420px; height: 767px;">
                <div id="cboxLoadedContent" style="display: block; width: 420px; overflow: hidden; height: 739px;">
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
                                                                Reason:</label>
                                                            <div class="inputvalue">
                                                                <span class="nftextLeft"></span><span class="custom-sel">
                                                                    <asp:DropDownList ID="ddlServiceTicketReason" TabIndex="2" onchange="pageLoad(this,value);"
                                                                        runat="server">
                                                                    </asp:DropDownList>
                                                                </span>
                                                                <div id="dvServiceTicketReason">
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li>
                                                            <label>
                                                                Your Question:</label>
                                                            <div class="inputvalue">
                                                                <span class="nftextareaLeft"></span>
                                                                <asp:TextBox ID="txtQuestion" MaxLength="500" TabIndex="3" TextMode="MultiLine" CssClass="nftextarearight"
                                                                    runat="server"></asp:TextBox>
                                                                <div id="dvQuestion">
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li style="padding-bottom: 15px;"><span class="infotxt">Or Call Us at: 772-453-2759</span>
                                                            <div class="alignright">
                                                                <label class="popup-button">
                                                                    <asp:Button ID="btnSubmitTicketCE" Text="Submit" TabIndex="4" runat="server" OnClick="btnSubmitTicketCE_Click" />
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
