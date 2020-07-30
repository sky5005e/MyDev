<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OpenServiceTicket.aspx.cs" Inherits="admin_ServiceTicketCenter_OpenServiceTicket"
    Title="Open Support Ticket" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script src="../../JS/JQuery/jquery.maskedinput-1.2.2.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script language="javascript" type="text/javascript">
        function clickButton(e, buttonid) {
            var bt = document.getElementById(buttonid);
            if (typeof (bt) == 'object') {
                if (navigator.appName.indexOf("Netscape") > -1) {
                    if (e.keyCode == 13) {
                        if (bt && typeof (bt.click) == 'undefined') {
                            bt.click = addClickFunction1(bt);
                        }
                        else
                            bt.click();
                    }
                }
                if (navigator.appName.indexOf("Microsoft Internet Explorer") > -1) {
                    if (event.keyCode == 13) {
                        bt.click();
                        return false;
                    }
                }
            }
        }

        function addClickFunction1(bt) {
            debugger;
            var result = true;
            if (bt.onclick) result = bt.onclick();
            if (typeof (result) == 'undefined' || result) {
                eval(bt.href);
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {                        
                        ctl00$ContentPlaceHolder1$txtServiceTicketName: { required: true },                        
                        ctl00$ContentPlaceHolder1$ddlServiceTicketOwner: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCompanyName: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlSupplier: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtServiceTicketName: { required: replaceMessageString(objValMsg, "Required", "support ticket subject") },
                        ctl00$ContentPlaceHolder1$ddlServiceTicketOwner: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "support ticket owner") },
                        ctl00$ContentPlaceHolder1$ddlCompanyName: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "company name") },
                        ctl00$ContentPlaceHolder1$ddlSupplier: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "supplier name") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtServiceTicketName")
                            error.insertAfter("#dvTicketName");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlServiceTicketOwner")
                            error.insertAfter("#dvServiceTicketOwner");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCompanyName")
                            error.insertAfter("#dvCompanyName");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlSupplier")
                            error.insertAfter("#dvSupplier");
                        else
                            error.insertAfter(element);
                    }
                });
            });

            $("#<%=lnkBtnStartServicingNow.ClientID %>").click(function() {
                document.getElementById('ctl00_ContentPlaceHolder1_lblMsg').style.display = 'none';
                $('#dvLoader').show();
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });

            $("#ctl00_ContentPlaceHolder1_btnAddItem").click(function() {
                var fileinput = document.getElementById('ctl00_ContentPlaceHolder1_fpAttachment');
                if (!fileinput.files[0]) {
                    alert("Please select file.");
                    return false;
                }
                $('#dvLoader').show();                
            });
            
            $(window).scroll(function () {
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());            
        });

        $(function() {
            $("#ctl00_ContentPlaceHolder1_txtDatePromised").mask("99/99/9999");
            
            $("#ctl00_ContentPlaceHolder1_txtDatePromised").change(function() {
                isDate($("#ctl00_ContentPlaceHolder1_txtDatePromised").val());
            });
        });

        $(function() {
            scrolltextarea(".scrollme", "#Scrolltop", "#ScrollBottom");
        });
    </script>

    <script type="text/javascript" language="javascript">
        var dtCh = "/";
        var minYear = 1900;
        var maxYear = 2100;

        function isInteger(s) {
            var i;
            for (i = 0; i < s.length; i++) {
                // Check that current character is number.
                var c = s.charAt(i);
                if (((c < "0") || (c > "9"))) return false;
            }
            // All characters are numbers.
            return true;
        }

        function stripCharsInBag(s, bag) {
            var i;
            var returnString = "";
            // Search through string's characters one by one.
            // If character is not in bag, append to returnString.
            for (i = 0; i < s.length; i++) {
                var c = s.charAt(i);
                if (bag.indexOf(c) == -1) returnString += c;
            }
            return returnString;
        }

        function daysInFebruary(year) {
            // February has 29 days in any year evenly divisible by four,
            // EXCEPT for centurial years which are not also divisible by 400.
            return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
        }
        function DaysArray(n) {
            for (var i = 1; i <= n; i++) {
                this[i] = 31
                if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
                if (i == 2) { this[i] = 29 }
            }
            return this
        }

        function isDate(dtStr) {
            var daysInMonth = DaysArray(12)
            var pos1 = dtStr.indexOf(dtCh)
            var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
            var strMonth = dtStr.substring(0, pos1)
            var strDay = dtStr.substring(pos1 + 1, pos2)
            var strYear = dtStr.substring(pos2 + 1)
            strYr = strYear
            if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
            if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
            for (var i = 1; i <= 3; i++) {
                if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
            }
            month = parseInt(strMonth)
            day = parseInt(strDay)
            year = parseInt(strYr)
            if (pos1 == -1 || pos2 == -1) {
                alert("The date format should be : mm/dd/yyyy")
                return false
            }
            if (strMonth.length < 1 || month < 1 || month > 12) {
                alert("Please enter a valid month")
                return false
            }
            if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
                alert("Please enter a valid day")
                return false
            }
            if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
                alert("Please enter a valid 4 digit year between " + minYear + " and " + maxYear)
                return false
            }
            if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
                alert("Please enter a valid date")
                return false
            }
            return true
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
        });
    </script>

    <link media="screen" rel="stylesheet" href="../../CSS/colorbox.css" />
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad select_box_pad" style="width: 306px;">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="form_table">
            <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
            <table>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 24%">Subject</span>
                                <asp:TextBox ID="txtServiceTicketName" TabIndex="0" onkeypress="javascript:return clickButton(event,'ctl00_ContentPlaceHolder1_lnkBtnStartServicingNow');"
                                    runat="server" CssClass="w_label"></asp:TextBox>
                                <div id="dvTicketName">
                                </div>
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
                                <span class="input_label" style="width: 24%;">Date Needed</span>
                                <asp:TextBox ID="txtDatePromised" runat="server" onkeypress="javascript:return clickButton(event,'ctl00_ContentPlaceHolder1_lnkBtnStartServicingNow');"
                                    TabIndex="1" CssClass="w_label"></asp:TextBox>
                                <div id="dvDatePromised">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel ID="upCompanyName" runat="server">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box" style="padding-left: 0px">
                                        <span class="custom-sel">
                                            <asp:DropDownList ID="ddlCompanyName" TabIndex="2" onchange="pageLoad(this,value);"
                                                runat="server" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged" AutoPostBack="True"
                                                onkeypress="javascript:return clickButton(event,'ctl00_ContentPlaceHolder1_lnkBtnStartServicingNow');">
                                            </asp:DropDownList>
                                        </span>
                                        <div id="dvCompanyName">
                                        </div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel ID="upContact" runat="server">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box" style="padding-left: 0px">
                                        <span class="custom-sel">
                                            <asp:DropDownList ID="ddlContactName" TabIndex="3" onchange="pageLoad(this,value);"
                                                runat="server" Enabled="false" onkeypress="javascript:return clickButton(event,'ctl00_ContentPlaceHolder1_lnkBtnStartServicingNow');">
                                            </asp:DropDownList>
                                        </span>
                                        <div id="dvContactName">
                                        </div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel ID="upServiceTicketOwner" runat="server">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="custom-sel">
                                            <asp:DropDownList ID="ddlServiceTicketOwner" TabIndex="4" onchange="pageLoad(this,value);"
                                                runat="server" onkeypress="javascript:return clickButton(event,'ctl00_ContentPlaceHolder1_lnkBtnStartServicingNow');">
                                            </asp:DropDownList>
                                        </span>
                                        <div id="dvServiceTicketOwner">
                                        </div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel ID="upSupplier" runat="server">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="custom-sel">
                                            <asp:DropDownList ID="ddlSupplier" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged"
                                                AutoPostBack="True" TabIndex="5" onchange="pageLoad(this,value);" runat="server"
                                                onkeypress="javascript:return clickButton(event,'ctl00_ContentPlaceHolder1_lnkBtnStartServicingNow');">
                                            </asp:DropDownList>
                                        </span>
                                        <div id="dvSupplier">
                                        </div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Support Ticket Details:
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box employeeedit_text clearfix">
                                <div class="textarea_box alignright" style="width: 100%; height: 72px;">
                                    <div class="scrollbar" style="height: 79px;">
                                        <a href="#scroll" id="Scrolltop" class="scrolltop"></a><a href="#scroll" id="ScrollBottom"
                                            class="scrollbottom"></a>
                                    </div>
                                    <textarea id="txtServiceTicketDetails" tabindex="6" style="height: 72px;" rows="10"
                                        runat="server" class="scrollme"></textarea>
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <div class="alignleft item">
                                        <div>
                                            <asp:LinkButton TabIndex="7" ID="lnkOtherDocument" class="greyicon_btn btn" runat="server"
                                                ToolTip="Upload Attachments">
                                                <span runat="server" class="btn_width250" style="width: 213px;" id="atSpan">&nbsp;&nbsp;&nbsp;Attachment(s)
                                                    (0)<img src="../../images/upload-other-icon.png" alt="" /></span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDummyAttachments" class="grey2_btn alignleft" runat="server"
                                                Style="display: none"></asp:LinkButton>
                                            <at:ModalPopupExtender ID="modalAttachments" TargetControlID="lnkOtherDocument" BackgroundCssClass="modalBackground"
                                                DropShadow="true" runat="server" PopupControlID="pnlAttachments" CancelControlID="cboxClose">
                                            </at:ModalPopupExtender>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton TabIndex="8" ID="lnkBtnStartServicingNow" class="gredient_btn" runat="server"
                            ToolTip="Submit Support Ticket" OnClick="lnkBtnStartServicingNow_Click">
                            <span id="spanSubmitServiceTicket" runat="server">Submit Support Ticket</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div>
        <asp:Panel ID="pnlAttachments" runat="server" Style="display: none;">
            <div id="cboxWrapper" style="display: block; width: 558px; height: 549px; position: fixed;
                left: 30%; top: 8%;">
                <div style="">
                    <div id="cboxTopLeft" style="float: left;">
                    </div>
                    <div id="cboxTopCenter" style="float: left; width: 508px;">
                    </div>
                    <div id="cboxTopRight" style="float: left;">
                    </div>
                </div>
                <div style="clear: left;">
                    <div id="cboxMiddleLeft" style="float: left; height: 483px;">
                    </div>
                    <div id="cboxContent" style="float: left; width: 508px; display: block; height: 483px;">
                        <div id="cboxLoadedContent" style="display: block; overflow: visible;">
                            <div style="padding: 25px 10px 10px 10px;">
                                <div style="height: 383px; overflow: auto;">
                                    <div style="text-align: center; color: Red; font-size: 14px;" visible="false">
                                        <asp:Label ID="lblMessage" runat="server">
                                        </asp:Label>
                                    </div>
                                    <asp:GridView ID="grvAttachment" runat="server" Width="100%" HeaderStyle-CssClass="ord_header"
                                        CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                                        OnRowCommand="grvAttachment_RowCommand">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span>File Name </span>
                                                    <div class="corner">
                                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span class="first">
                                                        <%#Eval("OnlyFileName")%>
                                                    </span>
                                                    <div class="corner">
                                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box" Width="85%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span>Delete</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtndelete" CommandName="DeleteAttachment" OnClientClick="return confirm('Are you sure, you want to delete attachment?');"
                                                        CommandArgument='<%#Eval("OnlyFileName")%>' runat="server">
                                                        <span class="btn_space">
                                                            <img alt="X" id="delete" src="~/Images/close.png" runat="server" /></span></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="spacer10">
                                </div>
                                <div class="form_top_co" style="clear: both;">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <input id="fpAttachment" name="fpAttachment" type="file" runat="server" style="float: left;" />
                                    <span style="float: left;">
                                        <asp:LinkButton ID="btnAddItem" Text="Add File" CssClass="greysm_btn" runat="server"
                                            OnClick="btnAddItem_Click"><span>Add File</span></asp:LinkButton>
                                    </span>
                                    <br />
                                    <div id="dvAttachment">
                                    </div>
                                    <br />
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                            <div id="cboxLoadingOverlay" style="height: 483px; display: none;" class="">
                            </div>
                            <div id="cboxLoadingGraphic" style="height: 483px; display: none;" class="">
                            </div>
                            <div id="cboxTitle" style="display: block;" class="">
                            </div>
                        </div>
                        <div id="cboxClose" style="" class="">
                            close</div>
                    </div>
                    <div id="cboxMiddleRight" style="float: left; height: 483px;">
                    </div>
                </div>
                <div style="clear: left;">
                    <div id="cboxBottomLeft" style="float: left;">
                    </div>
                    <div id="cboxBottomCenter" style="float: left; width: 508px;">
                    </div>
                    <div id="cboxBottomRight" style="float: left;">
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
