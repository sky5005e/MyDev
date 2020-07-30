<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MemeberControl.ascx.cs"
    Inherits="usercontrol_MemeberControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<link media="screen" rel="stylesheet" href='<%=ConfigurationSettings.AppSettings["siteurl"] %>CSS/colorboxpopup.css' />
<style type="text/css">
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
    span.help
    {
        background: url(     "" ) no-repeat scroll left top transparent !important;
        font-family: Arial,Helvetica,sans-serif;
        font-size: 18px !important;
        height: 25px;
        line-height: 25px;
        padding-left: 20px !important;
        text-transform: capitalize;
        width: 100% !important;
    }
    .custom-sel select
    {
        float: right !important;
        height: 0px;
        left: 25px;
        position: absolute;
        top: 0;
        width: 300px !important;
        z-index: 2;
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
    });
</script>

<script language="javascript" type="text/javascript">
    function ChangeCheckbox() {
        if (document.getElementById('chkEmp').checked == true) {
            document.getElementById('chkVendor').checked = false;
            document.getElementById('<%=hdCheck.ClientID %>').value = "true";
            var eletxt = document.getElementById('<%=dvtxtCompany.ClientID%>');
            if (eletxt.style.display == "block") {
                eletxt.style.display = "none";
            }
            else if (eletxt.style.display == "none") {
                eletxt.style.display = "none";
            }
            var eleddl = document.getElementById('<%=dvddlCompany.ClientID%>');
            if (eleddl.style.display == "block") {
                eleddl.style.display = "none";
            }
            else if (eleddl.style.display == "none") {
                eleddl.style.display = "block";
            }
        }
        else {
            document.getElementById('chkVendor').checked = true;
            document.getElementById('<%=hdCheck.ClientID %>').value = "false";
            var eletxt = document.getElementById('<%=dvtxtCompany.ClientID%>');
            if (eletxt.style.display == "block") {
                eletxt.style.display = "none";
            }
            else if (eletxt.style.display == "none") {
                eletxt.style.display = "block";
            }
            var eleddl = document.getElementById('<%=dvddlCompany.ClientID%>');
            if (eleddl.style.display == "block") {
                eleddl.style.display = "none";
            }
            else if (eleddl.style.display == "none") {
                eleddl.style.display = "none";
            }
        }
    }
    function SecChangeCheckbox() {
        if (document.getElementById('chkVendor').checked == true) {
            document.getElementById('<%=hdCheck.ClientID %>').value = "false";
            document.getElementById('chkEmp').checked = false;
            var eletxt = document.getElementById('<%=dvtxtCompany.ClientID%>');
            if (eletxt.style.display == "block") {
                eletxt.style.display = "none";
            }
            else if (eletxt.style.display == "none") {
                eletxt.style.display = "block";
            }
            var eleddl = document.getElementById('<%=dvddlCompany.ClientID%>');
            if (eleddl.style.display == "block") {
                eleddl.style.display = "none";
            }
            else if (eleddl.style.display == "none") {
                eleddl.style.display = "none";
            }
        }
        else {
            document.getElementById('<%=hdCheck.ClientID %>').value = "true";
            document.getElementById('chkEmp').checked = true;
            var eletxt = document.getElementById('<%=dvtxtCompany.ClientID%>');
            if (eletxt.style.display == "block") {
                eletxt.style.display = "none";
            }
            else if (eletxt.style.display == "none") {
                eletxt.style.display = "none";
            }
            var eleddl = document.getElementById('<%=dvddlCompany.ClientID%>');
            if (eleddl.style.display == "block") {
                eleddl.style.display = "none";
            }
            else if (eleddl.style.display == "none") {
                eleddl.style.display = "block";
            }
        }
    }
</script>
 <script type="text/javascript" language="javascript">
     function pageLoad(sender, args) {
         assigndesign();
     }
     
    </script>
<script language="javascript" type="text/javascript">
    $(document).ready(function() {
        document.getElementById('chkEmp').checked = true;
        document.getElementById('chkVendor').checked = false;
        document.getElementById('<%=hdCheck.ClientID %>').value = "true";
        var eletxt = document.getElementById('<%=dvtxtCompany.ClientID%>');
        if (eletxt.style.display == "block") {
            eletxt.style.display = "none";
        }
        else if (eletxt.style.display == "none") {
            eletxt.style.display = "none";
        }
        var eleddl = document.getElementById('<%=dvddlCompany.ClientID%>');
        if (eleddl.style.display == "block") {
            eleddl.style.display = "none";
        }
        else if (eleddl.style.display == "none") {
            eleddl.style.display = "block";
        }
    });
</script>

<div id="dvLoader">
    <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
    </div>
    <div class="updateProgressDiv">
        <img alt="Loading" src="Images/ajax-loader-large.gif" />
    </div>
</div>
<asp:LinkButton ID="lnkEmpStatus" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
<at:ModalPopupExtender BehaviorID="mpMemberStatusBehaviour" ID="mpMemberStatus" TargetControlID="lnkEmpStatus"
    BackgroundCssClass="modalBackground" DropShadow="false" runat="server" CancelControlID="cboxClose"
    PopupControlID="pnlEmpStatus">
</at:ModalPopupExtender>
<input type="hidden" id="hdCheck" runat="server" />
<asp:PlaceHolder runat="server" ID="phScriptManager"></asp:PlaceHolder>
<asp:Panel ID="pnlEmpStatus" runat="server" Style="display: none;">
    <div id="cboxWrapper" style="min-height: 195px; max-height: 215px; width: 370px;
        position: fixed; left: 36.35%; top: 30%;">
        <div style="">
            <div id="cboxTopLeft" style="float: left;">
            </div>
            <div id="cboxTopCenter" style="float: left; width: 370px;">
            </div>
            <div id="cboxTopRight" style="float: left;">
            </div>
        </div>
        <div style="clear: left;">
            <div id="cboxMiddleLeft" style="float: left; min-height: 195px; max-height: 215px;">
            </div>
            <div id="cboxContent" style="float: left; width: 370px; min-height: 195px; max-height: 215px;">
                <div id="cboxLoadedContent" style="display: block; width: 370px; overflow: hidden;
                    min-height: 195px; max-height: 215px;">
                    <div id="help">
                        <div id="popup_wrapper">
                            <div id="popup_header">
                                <div id="headercontent">
                                    <span class="help alignleft">My Employment Status (Check One)</span>
                                </div>
                            </div>
                            <div id="container_popup">
                                <table cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td class="leftrounded">
                                            </td>
                                            <td class="mid" style="width: auto;">
                                                <form action="">
                                                <fieldset>
                                                    <div class="form-box">
                                                        <div class="spacer10">
                                                        </div>
                                                        <div>
                                                            <table style="padding-bottom: 10px;" class="true">
                                                                <tr>
                                                                    <td class="alignleft">
                                                                        <label>
                                                                            I am a Direct Employee</label>
                                                                    </td>
                                                                    <td class="alignright">
                                                                        <input type="checkbox" id="chkEmp" onclick="ChangeCheckbox();" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div>
                                                            <table style="padding-bottom: 12px;" class="true">
                                                                <tr>
                                                                    <td class="alignleft">
                                                                        <label>
                                                                            I am a 3rd Party Vendor Employee</label>
                                                                    </td>
                                                                    <td class="alignright">
                                                                        <input type="checkbox" id="chkVendor" onclick="SecChangeCheckbox();" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div>
                                                            <table style="padding-bottom: 12px;" class="true">
                                                                <tr>
                                                                    <td class="alignleft">
                                                                        <label>
                                                                            Company Name</label>
                                                                    </td>
                                                                    <td class="alignright">
                                                                        <div id="dvddlCompany" runat="server" style="display: none">
                                                                            <asp:DropDownList runat="server" ID="ddlCompany">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        <div id="dvtxtCompany" runat="server" style="display: none">
                                                                            <asp:TextBox ID="txtCompanyName" runat="server" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div class="alignright" style="clear: both;">
                                                            <label class="popup-button">
                                                                <asp:Button ID="btnSubmit" Text="Submit" TabIndex="3" runat="server" OnClick="btnSubmit_Click" />
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div id="cboxClose" style="" class="">
                                                    close</div>
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
                <div id="cboxLoadingOverlay" style="display: none;" class="">
                </div>
                <div id="cboxLoadingGraphic" style="display: none;" class="">
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
            <div id="cboxMiddleRight" style="float: left; min-height: 195px; max-height: 215px;">
            </div>
        </div>
        <div style="clear: left;">
            <div id="cboxBottomLeft" style="float: left;">
            </div>
            <div id="cboxBottomCenter" style="float: left; width: 370px;">
            </div>
            <div id="cboxBottomRight" style="float: left;">
            </div>
        </div>
    </div>
</asp:Panel>
