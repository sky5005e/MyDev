<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OpenServiceticketAnonymous.ascx.cs"
    Inherits="usercontrol_OpenServiceticketAnonymous" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
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
</style>

<script type="text/javascript">
    $(window).load(function() {
        $("#dvLoader").hide();
    });
</script>

<script type="text/javascript" language="javascript">
    $().ready(function() {
        $("#dvLoader").show();
        $("#ctl00_ContentPlaceHolder2_ostAnnControl_txtLoginEmail").focus();
    });
</script>

<div id="dvLoader">
    <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
    </div>
    <div class="updateProgressDiv">
        <img alt="Loading" src="Images/ajax-loader-large.gif" />
    </div>
</div>
<asp:LinkButton ID="lnkDummyServiceTicketAnn" class="grey2_btn alignright" runat="server"
    Style="display: none"></asp:LinkButton>
<at:ModalPopupExtender BehaviorID="mpOpenServiceTicketAnnBehaviour" ID="mpOpenServiceTicketAnn"
    TargetControlID="lnkDummyServiceTicketAnn" BackgroundCssClass="modalBackground"
    DropShadow="false" runat="server" CancelControlID="cboxClose" PopupControlID="pnlOpenServiceTicketAnn">
</at:ModalPopupExtender>
<asp:PlaceHolder runat="server" ID="phScriptManager"></asp:PlaceHolder>
<asp:Panel ID="pnlOpenServiceTicketAnn" runat="server" Style="display: none;">
    <div id="cboxWrapper" style="height: 480px; width: 420px; position: fixed; left: 36.4%; top: 20%;">
        <div style="">
            <div id="cboxTopLeft" style="float: left;">
            </div>
            <div id="cboxTopCenter" style="float: left; width: 420px;">
            </div>
            <div id="cboxTopRight" style="float: left;">
            </div>
        </div>
        <div style="clear: left;">
            <div id="cboxMiddleLeft" style="float: left; height: 647px;">
            </div>
            <div id="cboxContent" style="float: left; width: 420px; height: 647px;">
                <div id="cboxLoadedContent" style="display: block; width: 420px; overflow: hidden;
                    height: 619px;">
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
                                                                Your Email:</label>
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
                                                                <asp:TextBox ID="txtSubject" runat="server" TabIndex="2" CssClass="nftextright"></asp:TextBox>
                                                                <div id="dvSubject">
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
                                                                    <asp:Button ID="btnSubmitTicketAnn" Text="Submit" TabIndex="4" runat="server" OnClick="btnSubmitTicketAnn_Click" />
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
                <div id="cboxLoadingOverlay" style="height: 647px; display: none;" class="">
                </div>
                <div id="cboxLoadingGraphic" style="height: 647px; display: none;" class="">
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
            <div id="cboxMiddleRight" style="float: left; height: 647px;">
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
