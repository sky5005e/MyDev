<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OpenServiceTicketIE.ascx.cs"
    Inherits="usercontrol_OpenServiceTicketIE" %>
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
    $().ready(function() {
        $("#dvLoader").show();
        $.get('<%=ConfigurationSettings.AppSettings["siteurl"] %>JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
            objValMsg = $.xml2json(xml);
            $("#aspnetForm").validate({
                rules: {
                    ctl00$ContentPlaceHolder1$ostIEControl$txtServiceTicketName: { required: true },
                    ctl00$ContentPlaceHolder1$ostIEControl$ddlCompanyName: { NotequalTo: "0" },
                    ctl00$ContentPlaceHolder1$ostIEControl$ddlSupplier: { NotequalTo: "0" },
                    ctl00$ContentPlaceHolder1$ostIEControl$ddlServiceTicketOwner: { NotequalTo: "0" },
                    ctl00$ContentPlaceHolder1$ostIEControl$txtQuestion: { required: true }
                },
                messages: {
                    ctl00$ContentPlaceHolder1$ostIEControl$txtServiceTicketName: { required: replaceMessageString(objValMsg, "Required", "support ticket subject") },
                    ctl00$ContentPlaceHolder1$ostIEControl$ddlCompanyName: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "company name") },
                    ctl00$ContentPlaceHolder1$ostIEControl$ddlSupplier: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "supplier name") },
                    ctl00$ContentPlaceHolder1$ostIEControl$ddlServiceTicketOwner: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "support ticket owner") },
                    ctl00$ContentPlaceHolder1$ostIEControl$txtQuestion: { required: replaceMessageString(objValMsg, "Required", "your request") }
                },
                errorPlacement: function(error, element) {
                    if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostIEControl$txtServiceTicketName")
                        error.insertAfter("#dvTicketName");
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostIEControl$ddlCompanyName")
                        error.insertAfter("#dvCompanyName");
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostIEControl$ddlSupplier")
                        error.insertAfter("#dvSupplier");
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostIEControl$ddlServiceTicketOwner")
                        error.insertAfter("#dvServiceTicketOwner");
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ostIEControl$txtQuestion")
                        error.insertAfter("#dvRequest");
                    else
                        error.insertAfter(element);
                }
            });
        });

        $("#ctl00_ContentPlaceHolder1_ostIEControl_btnSubmitTicketIE").click(function() {
            $('#dvLoader').show();            
            if ($("#aspnetForm").valid()) {
                return true;
            }
            else {
                $('#dvLoader').hide();
                return false;
            }
        });

        $("#ctl00_ContentPlaceHolder1_ostIEControl_txtServiceTicketName").focus();
    });    
</script>

<script src='../JS/JQuery/jquery.MultiFile.pack.js' type="text/javascript" language="javascript"></script>

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


<div id="dvLoader">
    <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
    </div>
    <div class="updateProgressDiv">
        <img alt="Loading" src="../Images/ajax-loader-large.gif" />
    </div>
</div>
<asp:LinkButton ID="lnkDummyServiceTicketIE" class="grey2_btn alignright" runat="server"
    Style="display: none"></asp:LinkButton>
<at:ModalPopupExtender ID="mpOpenServiceTicketIE" BehaviorID="mpOpenServiceTicketIEBehaviour"
    TargetControlID="lnkDummyServiceTicketIE" BackgroundCssClass="modalBackground"
    DropShadow="false" runat="server" CancelControlID="cboxClose" PopupControlID="pnlOpenServiceTicketIE">
</at:ModalPopupExtender>
<asp:PlaceHolder runat="server" ID="phScriptManager"></asp:PlaceHolder>
<asp:Panel ID="pnlOpenServiceTicketIE" runat="server" Style="display: none;">
    <div id="cboxWrapper" style="min-height: 767px; max-height: 867px; width: 420px;
        overflow: auto; position: fixed; left: 36.4%; top: 2%;">
        <div style="">
            <div id="cboxTopLeft" style="float: left;">
            </div>
            <div id="cboxTopCenter" style="float: left; width: 420px;">
            </div>
            <div id="cboxTopRight" style="float: left;">
            </div>
        </div>
        <div style="clear: left;">
            <div id="cboxMiddleLeft" style="float: left; min-height: 767px; max-height: 867px;
                overflow: auto;">
            </div>
            <div id="cboxContent" style="float: left; width: 420px; min-height: 767px; max-height: 867px;
                overflow: auto;">
                <div id="cboxLoadedContent" style="display: block; width: 420px; overflow: hidden;
                    min-height: 739px; max-height: 839px;">
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
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="leftrounded">
                                        </td>
                                        <td class="alignleft mid">
                                            <form action="">
                                            <fieldset>
                                                <ul class="form-box">
                                                    <li>
                                                        <label>
                                                            Subject:</label>
                                                        <div class="inputvalue">
                                                            <span class="nftextLeft"></span>
                                                            <asp:TextBox ID="txtServiceTicketName" TabIndex="1" runat="server" CssClass="nftextright"></asp:TextBox>
                                                            <div id="dvTicketName">
                                                            </div>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <label>
                                                            Company:</label>
                                                        <div class="inputvalue">
                                                            <span class="nftextLeft"></span><span class="custom-sel">
                                                                <asp:DropDownList ID="ddlCompanyName" TabIndex="2" onchange="pageLoad(this,value);"
                                                                    runat="server" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </span>
                                                            <div id="dvCompanyName">
                                                            </div>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <label>
                                                            Contact:</label>
                                                        <div class="inputvalue">
                                                            <span class="nftextLeft"></span><span class="custom-sel">
                                                                <asp:DropDownList ID="ddlContactName" TabIndex="3" onchange="pageLoad(this,value);"
                                                                    runat="server" Enabled="False">
                                                                </asp:DropDownList>
                                                            </span>
                                                            <div id="dvContactName">
                                                            </div>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <label>
                                                            Supplier:</label>
                                                        <div class="inputvalue">
                                                            <span class="nftextLeft"></span><span class="custom-sel">
                                                                <asp:DropDownList ID="ddlSupplier" TabIndex="4" onchange="pageLoad(this,value);"
                                                                    runat="server" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </span>
                                                            <div id="dvSupplier">
                                                            </div>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <label>
                                                            Assign To:</label>
                                                        <div class="inputvalue">
                                                            <span class="nftextLeft"></span><span class="custom-sel">
                                                                <asp:DropDownList ID="ddlServiceTicketOwner" TabIndex="5" onchange="pageLoad(this,value);"
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
                                                            <asp:TextBox ID="txtQuestion" MaxLength="500" TabIndex="6" TextMode="MultiLine" CssClass="nftextarearight"
                                                                runat="server"></asp:TextBox>
                                                            <div id="dvRequest">
                                                            </div>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <label>
                                                            Date Needed:</label>
                                                        <div class="inputvalue">
                                                            <span class="nftextLeft"></span>
                                                            <asp:TextBox ID="txtDatePromised" runat="server" TabIndex="7" Width="300px" CssClass="datepicker1 nftextright"></asp:TextBox>
                                                            <div id="dvDatePromised" style="clear: both;">
                                                            </div>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <label>
                                                            Attachment(s):</label>
                                                        <div class="inputvalue">
                                                            <input type="file" id="flAttachment" runat="server" tabindex="8" class="multi max-3 accept-gif|jpg|xlsx|xls|doc|docx|pdf|png"
                                                                size="37" />
                                                        </div>
                                                    </li>
                                                    <li style="padding-bottom: 15px;">
                                                        <div class="alignright">
                                                            <label class="popup-button">
                                                                <asp:Button ID="btnSubmitTicketIE" Text="Submit" TabIndex="9" runat="server" OnClick="btnSubmitTicketIE_Click" />
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
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="cboxLoadingOverlay" style="min-height: 767px; max-height: 867px; overflow: auto;
                    display: none;" class="">
                </div>
                <div id="cboxLoadingGraphic" style="min-height: 767px; max-height: 867px; overflow: auto;
                    display: none;" class="">
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
            <div id="cboxMiddleRight" style="float: left; min-height: 767px; max-height: 867px;
                overflow: auto;">
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
