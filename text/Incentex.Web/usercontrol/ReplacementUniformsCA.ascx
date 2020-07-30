<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReplacementUniformsCA.ascx.cs"
    Inherits="usercontrol_ReplacementUniformsCA" %>
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
                    ctl00$ContentPlaceHolder1$ruCAControl$ddlEmployees: { NotequalTo: "0" }
                },
                messages: {
                    ctl00$ContentPlaceHolder1$ruCAControl$ddlEmployees: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "employee") }
                },
                errorPlacement: function(error, element) {
                    if (element.attr("name") == "ctl00$ContentPlaceHolder1$ruCAControl$ddlEmployees")
                        error.insertAfter("#dvEmployees");
                    else
                        error.insertAfter(element);
                }
            });
        });

        $("#ctl00_ContentPlaceHolder1_ruCAControl_btnPublishRU2CE").click(function() {
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
<asp:LinkButton ID="lnkDummyReplacementUniformCA" class="grey2_btn alignright" runat="server"
    Style="display: none"></asp:LinkButton>
<at:ModalPopupExtender BehaviorID="mpReplacementUniformCABehaviour" ID="mpReplacementUniformCA"
    TargetControlID="lnkDummyReplacementUniformCA" BackgroundCssClass="modalBackground"
    DropShadow="false" runat="server" CancelControlID="cboxClose" PopupControlID="pnlReplacementUniformCA">
</at:ModalPopupExtender>
<asp:PlaceHolder runat="server" ID="phScriptManager"></asp:PlaceHolder>
<asp:Panel ID="pnlReplacementUniformCA" runat="server" Style="display: none;">
    <div id="cboxWrapper" style="height: 220px; width: 420px; position: fixed; left: 36.4%;
        top: 28%;">
        <div style="">
            <div id="cboxTopLeft" style="float: left;">
            </div>
            <div id="cboxTopCenter" style="float: left; width: 420px;">
            </div>
            <div id="cboxTopRight" style="float: left;">
            </div>
        </div>
        <div style="clear: left;">
            <div id="cboxMiddleLeft" style="float: left; height: 367px;">
            </div>
            <div id="cboxContent" style="float: left; width: 420px; height: 367px;">
                <div id="cboxLoadedContent" style="display: block; width: 420px; overflow: hidden;
                    height: 339px;">
                    <div id="help">
                        <div id="popup_wrapper">
                            <div id="popup_header">
                                <div id="headercontent">
                                    <div id="cboxClose">
                                        close</div>
                                    <h6 style="line-height:26px; font-weight: bold; font-size: 14px;">
                                        Replacement Uniforms Payment Option</h6>
                                </div>
                            </div>
                            <div id="container_popup">
                                <table cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td class="leftrounded">
                                            </td>
                                            <td class="mid">
                                                <fieldset>
                                                    <ul class="form-box">
                                                        <li>
                                                            <label>
                                                                Publish To:</label>
                                                            <div class="inputvalue">
                                                                <span class="nftextLeft"></span><span class="custom-sel">
                                                                    <asp:DropDownList ID="ddlEmployees" TabIndex="1" onchange="pageLoad(this,value);"
                                                                        runat="server">
                                                                    </asp:DropDownList>
                                                                </span>
                                                                <div id="dvEmployees">
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li style="padding-bottom: 15px;"><span class="infotxt">Click publish to be used only
                                                            once</span>
                                                            <div class="alignright">
                                                                <label class="popup-button">
                                                                    <asp:Button ID="btnPublishRU2CE" Text="Publish" TabIndex="2" runat="server" OnClick="btnPublishRU2CE_Click" />
                                                                </label>
                                                            </div>
                                                        </li>
                                                    </ul>
                                                </fieldset>
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
                <div id="cboxLoadingOverlay" style="height: 367px; display: none;" class="">
                </div>
                <div id="cboxLoadingGraphic" style="height: 367px; display: none;" class="">
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
            <div id="cboxMiddleRight" style="float: left; height: 367px;">
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
