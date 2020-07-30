<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ServiceTicketList.aspx.cs" 
Inherits="MyAccount_Report_ServiceTicketList" Title="Service Ticket List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .chkClass123xyz input
        {
            width: 15px;
            height: 15px;
        }
        .fancybox-overlay
        {
            left: 0;
            position: fixed;
            top: 0;
            width: 100%;
            height: 100%;
            z-index: 1100;
            opacity: 0.7;
            display: block;
            background-color: #777777;
        }
        .txtAreaFont
        {
            font-size: 13px;
            line-height: 17px;
            padding: 10px 10px 10px 10px;
            color: Black;
        }
        .order_detail td
        {
            font-size: 15px;
            color: #72757c;
            line-height: 20px;
            padding-bottom: 0px !important;
            text-align: left !important;
        }
        .order_detail label
        {
            color: #B0B0B0;
            padding-right: 6px;
            padding-left: 20px;
        }
        .chkPostedNote
        {
            color: #72757C;
            font-size: 15px;
            line-height: 20px;
            text-align: left !important;
            vertical-align: middle;
        }
    </style>
    <link media="screen" rel="stylesheet" href="../../CSS/colorbox.css" />

    <script type="text/javascript">
        function HeaderClick(CheckBox) {
            //Get target base & child control.            
            var TargetBaseControl = document.getElementById(CheckBox.id.substring(0, CheckBox.id.length - 18));
            var TargetChildControl = "chkBxSelect";

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                Inputs[n].checked = CheckBox.checked;                
            }
        }

        function ChildClick(CheckBox, HCheckBox) {
            //get target control.            
            var flag = true;
            
            if (CheckBox.checked == true) {
                var TargetBaseControl = document.getElementById(CheckBox.id.substring(0, CheckBox.id.length - 18));
                var TargetChildControl = "chkBxSelect";

                //Get all the control of the type INPUT in the base control.
                var Inputs = TargetBaseControl.getElementsByTagName("input");
                
                //Checked/Unchecked all the checkBoxes in side the GridView.            
                for (var n = 0; n < Inputs.length; ++n) {            
                    if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                        if (Inputs[n].checked == false) {
                            flag = false;
                            break;
                        }
                    } 
                }
            }
            else {
                flag = false;
            }
            //Change state of the header CheckBox.
            var HeaderCheckBox = document.getElementById(HCheckBox);
            HeaderCheckBox.checked = flag;
        }

        function CollapsibleContainerTitleOnClick() {            
            var hdnIamExpanded = document.getElementById($(this).parent().attr("id").substring(0, $(this).parent().attr("id").length - 13) + "hdnIamExpanded");            
            
            // The item clicked is the title div... get this parent (the overall container) and toggle the content within it.
            $(".collapsibleContainerContent", $(this).parent()).slideToggle();
            
            if (hdnIamExpanded.value == "true") {
                hdnIamExpanded.value = "false";
            }
            else if (hdnIamExpanded.value == "false") {
                hdnIamExpanded.value = "true";
            }
        }
    </script>

    <script type="text/javascript">
        $(window).load(function() {
            $("#dvLoader").hide();
        });
    </script>

    <script type="text/javascript" language="javascript">
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true,
                minDate: new Date()
            });
        });
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $("#dvLoader").hide();
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleContainerContent").each(function() {
                var hdnIamExpanded = document.getElementById($(this).parent().attr("id").substring(0, $(this).parent().attr("id").length - 13) + "hdnIamExpanded");
                if (hdnIamExpanded.value == "true") {
                    $(this).show();
                }
                else if (hdnIamExpanded.value == "false") {
                    $(this).hide();
                }
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkNoteHisForIE").click(function() {
                $('#dvLoader').show();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkClose").click(function() {
                $('#dvLoader').show();
            });
            
            $("#ctl00_ContentPlaceHolder1_btnChangeDueDate").click(function() {
                $('#dvLoader').show();
            });
            
            $(".delbtn").click(function() {
                $('#dvLoader').show();
            });
            
            $(".pagin").click(function() {
                $('#dvLoader').show();
            });
            
            $("#closeDueDatePopup").click(function() {                
                $("#ctl00_ContentPlaceHolder1_pnlDueDate").hide();
            });
            
            $("#ctl00_ContentPlaceHolder1_txtDueDate").click(function() {
                $(this).select();
            });
            
            $("#ctl00_ContentPlaceHolder1_txtDueDate").keypress(function(e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode == 46) {                    
                    return true;
                }
                else {
                    return false;
                }
            });
            
            if ($("#ctl00_ContentPlaceHolder1_hdnIsSuperAdmin").val() == "1") {            
                $(".IsAdmin1").each(function () {
                    $(this).hide();
                });
                
                $(".IsAdmin2").each(function () {
                    $(this).removeClass("first centeralign IsAdmin2").addClass("centeralign IsAdmin2");
                });
            }
            
            $(window).scroll(function () {
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());            
        });
    </script>

    <script language="javascript" type="text/javascript">
        function NotePopup(id, notetype, statusid, ticketname, issupplierticket) {            
            $.ajax({
                type: "POST",
                url: "SearchResult.aspx/GetExistingNotes",
                data: "{'TicketID':'" + id + "', 'NoteType':'" + notetype + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg) {
                    $("#ctl00_ContentPlaceHolder1_txtExistingNotes").val(msg.d);
                }
            });
            $("#bNotePopup").html('Ticket # : ' + id + '.');
            $("#bTicketName").html('Ticket Name : ' + ticketname);
            $("#ctl00_ContentPlaceHolder1_hdnPopupStatusID").val(statusid);
            $("#ctl00_ContentPlaceHolder1_hdnPopupTicketID").val(id);
            $("#ctl00_ContentPlaceHolder1_hdnPopupIsSupplierTicket").val(issupplierticket);
            
            if (issupplierticket == 0) {
                $("#spanCustSuppNote").html('Customer Note');
            }
            else {
                $("#spanCustSuppNote").html('Supplier Note');
            }
            
            $("#ctl00_ContentPlaceHolder1_pnlNotesIE").show();            
        }
        
        function DueDatePopup(id, duedate) {            
            $("#DueDatePopupTitle").html('Ticket no.: ' + id + '.');
            $("#ctl00_ContentPlaceHolder1_hdnPopupTicketID").val(id);
            $("#ctl00_ContentPlaceHolder1_txtDueDate").val(duedate);
            $("#ctl00_ContentPlaceHolder1_pnlDueDate").show();
        }
        
        function CloseTicket(id) {
            if (confirm("Are you sure, you want to close ticket no. " + id + "?")) {
                $("#ctl00_ContentPlaceHolder1_hdnPopupTicketID").val(id);
                $('#dvLoader').show();
                return true;
            }
            else {
                return false;
            }
        }
        
        function FlagTicket(ticketid) {
            $("#ctl00_ContentPlaceHolder1_hdnPopupTicketID").val(ticketid);
            $('#dvLoader').show();
        }
    </script>

    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="Div1" style="display: none;">
        <div id="Div2" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad shipping_method_pad" style="width: 900px;">
        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
        <asp:HiddenField ID="hdnIsSuperAdmin" runat="server" Value="0" />
        <%--<div>
            <table class="true">
                <tr>
                    <td>
                        <span id="spanExpandCollapseAll" runat="server" class="custom-checkbox alignleft">
                            <asp:CheckBox ID="chkExpandCollapseAll" runat="server" />
                        </span>&nbsp;&nbsp;<span class="chkPostedNote" id="lblExpandCollapseAll">Expand All</span>
                    </td>
                    <td>
                        <span id="spanExpandOneAtATime" runat="server" class="custom-checkbox alignleft">
                            <asp:CheckBox ID="chkExpandOneAtATime" runat="server" />
                        </span>&nbsp;&nbsp;<span class="chkPostedNote">Expand One At a Time</span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer10">
        </div>--%>
        <div style="text-align: center">
            <asp:Label ID="lblMsgList" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <asp:Repeater ID="repServiceTickets" runat="server" OnItemDataBound="repServiceTickets_ItemDataBound">
            <ItemTemplate>
                <div style="clear: both;" runat="server" id="dvCollapsible" class="collapsibleContainer"
                    title='<%# Eval("sLookupName") %>'>
                    <asp:HiddenField ID="hdnRepStatusID" runat="server" Value='<%# Eval("iLookupID") %>' />
                    <asp:HiddenField ID="hdnIamExpanded" runat="server" Value="false" />
                    <div>
                        <asp:GridView ID="gvServiceTickets" runat="server" HeaderStyle-CssClass="ord_header"
                            CssClass="orderreturn_box true records" GridLines="None" AutoGenerateColumns="false"
                            RowStyle-CssClass="ord_content" OnRowDataBound="gvServiceTickets_RowDataBound"
                            OnRowCommand="gvServiceTickets_RowCommand" OnRowCreated="gvServiceTickets_RowCreated">
                            <Columns>
                                <asp:TemplateField HeaderText="Check">
                                    <HeaderTemplate>
                                        <span class="chkClass123xyz">
                                            <asp:CheckBox ID="chkBxHeader" onclick="javascript:HeaderClick(this);" runat="server" />&nbsp;</span>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="centeralign" />
                                    <ItemTemplate>
                                        <span class="first centeralign chkClass123xyz">
                                            <asp:CheckBox ID="chkBxSelect" runat="server" />&nbsp;</span>
                                        <div class="corner">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>
                                        <asp:HiddenField runat="server" ID="hdnIsSupplierTicket" Value='<%# Eval("IsSupplierTicket") %>' />
                                        <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ServiceTicketID") %>' />
                                        <asp:HiddenField runat="server" ID="hdnUnReadNotesExists" Value='<%# Eval("UnReadNotesExists") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" CssClass="b_box centeralign" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ServiceTicketNumber">
                                    <HeaderTemplate>
                                        <span class="centeralign">
                                            <asp:LinkButton ID="lnkbtnHeaderTickteNumber" runat="server" CommandArgument="ServiceTicketNumber"
                                                CommandName="Sort">Ticket #</asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderTickteNumber" runat="server"></asp:PlaceHolder>
                                        </span>
                                        <div class="corner IsAdmin1">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="first centeralign IsAdmin2">
                                            <asp:LinkButton ID="lnkbtnItemTickteNumber" CommandName="TicketDetail" CommandArgument='<%# Eval("ServiceTicketID") %>'
                                                runat="server" ToolTip='<%# Convert.ToString(Eval("ServiceTicketNumber")) %>'
                                                Text='<%# Convert.ToString(Eval("ServiceTicketNumber")).Length > 10 ? Convert.ToString(Eval("ServiceTicketNumber")).Substring(0, 10).Trim() + "..." : Convert.ToString(Eval("ServiceTicketNumber")) %>'></asp:LinkButton>
                                        </span>
                                        <div class="corner IsAdmin1">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="9%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="OpenedByName">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnHeaderOpenedByName" runat="server" CommandArgument="OpenedByName"
                                            CommandName="Sort"><span class="centeralign">Opened By</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderOpenedByName" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblOpenedByName" Text='<%# Convert.ToString(Eval("OpenedByName")).Length > 18 ? Convert.ToString(Eval("OpenedByName")).Substring(0, 18).Trim() + "..." : Convert.ToString(Eval("OpenedByName")).Trim() != "" ? Eval("OpenedByName") : "Anonymous User" %>'
                                            ToolTip='<%# Eval("OpenedByName")  %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="18%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ServiceTicketName">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnHeaderTicketName" runat="server" CommandArgument="ServiceTicketName"
                                            CommandName="Sort"><span class="centeralign">Ticket Name</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderTicketName" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span>
                                            <asp:LinkButton runat="server" ID="lblTicketName" CommandName="TicketDetail" CommandArgument='<%# Eval("ServiceTicketID") %>'
                                                Text='<%# Convert.ToString(Eval("ServiceTicketName")).Length > 21 ? Convert.ToString(Eval("ServiceTicketName")).Substring(0, 21).Trim() + "..." : "&nbsp;" + Convert.ToString(Eval("ServiceTicketName")) %>'
                                                ToolTip='<%# Eval("ServiceTicketName") %>'></asp:LinkButton></span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="24%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="StartDateNTime">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnHeaderStartDateNTime" runat="server" CommandArgument="StartDateNTime"
                                            CommandName="Sort"><span class="centeralign">Start Date & Time</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderStartDateNTime" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" CssClass="centeralign" ID="lblStartDateNTime" Text='<%# Eval("StartDateNTime") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="DatePromisedFormatted">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnHeaderDatePromisedFormatted" runat="server" CommandArgument="DatePromisedFormatted"
                                            CommandName="Sort">
                                        <span>Due Date</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderDatePromisedFormatted" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span runat="server" id="lblDatePromisedFormatted">
                                            <%# "&nbsp;" + Eval("DatePromisedFormatted") %></span>
                                        <asp:HiddenField ID="hdnDueDate" runat="server" Value='<%# Eval("DatePromisedFormatted") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="TicketStatus">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnHeaderTicketStatus" runat="server" CommandArgument="TicketStatus"
                                            CommandName="Sort">
                                        <span class="centeralign">Status</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderTicketStatus" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="centeralign">
                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ServiceTicketID") %>' CommandName="CloseTicket"
                                                ID="lnkTicketStatus" Text='<%# Eval("TicketStatus") %>' />
                                        </span>
                                        <asp:HiddenField ID="hdnGvStatusID" runat="server" Value='<%# Eval("TicketStatusID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="12%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span>Notes</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="btn_space">
                                            <asp:Image AlternateText="X" ID="imgNote" ImageUrl="~/Images/edit_gray.png" ToolTip='<%# Eval("UnReadCount") %>'
                                                runat="server" /></span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box centeralign" Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span>Flag</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="btn_space">
                                            <asp:LinkButton ID="lnkFlag" runat="server" CommandArgument='<%# Eval("ServiceTicketID") %>'
                                                CommandName="FlagTicket" ToolTip="Add Flag" Style="margin-left: 1px;">
                                                <img alt="F" src="~/Images/flag_gray.png" style="margin-right: 2px;" runat="server"
                                                    id="imgFlag" /></asp:LinkButton></span>
                                        <asp:HiddenField ID="hdnIsFlagged" runat="server" Value='<%# Eval("IsFlagged") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="1%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="orderreturn_box">
                            <div class="companylist_botbtn alignleft">
                                <asp:LinkButton ID="lnkDelete" runat="server" class="grey_btn delbtn" OnClick="lnkDelete_Click"
                                    OnClientClick="return confirm('Are you sure, you want to delete selected record(s)?')">
						                <span>Delete</span>
                                </asp:LinkButton>
                            </div>
                            <div id="pagingtable" runat="server" class="alignright pagging">
                                <asp:LinkButton ID="lnkbtnPrevious" CssClass="prevb pagin" runat="server" OnClick="lnkbtnPrevious_Click"> 
                                </asp:LinkButton>
                                <span>
                                    <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>' CssClass="pagin"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:DataList></span>
                                <asp:LinkButton ID="lnkbtnNext" CssClass="nextb pagin" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="spacer10">
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div>
        <asp:Panel ID="pnlNotesIE" runat="server" Style="display: none;">
            <div class="fancybox-overlay">
            </div>
            <div class="pp_pic_holder facebook" style="display: block; width: 640px; height: 505px;
                position: fixed; left: 25%; top: 5%;">
                <div class="pp_top" style="">
                    <div class="pp_left">
                    </div>
                    <div class="pp_middle">
                    </div>
                    <div class="pp_right">
                    </div>
                </div>
                <div class="pp_content_container" style="">
                    <div class="pp_left" style="">
                        <div class="pp_right" style="">
                            <div class="pp_fade" style="display: block;">
                                <asp:LinkButton runat="server" ID="lnkClose" CssClass="pp_close" OnClick="lnkClose_Click">Close</asp:LinkButton>
                                <p class="pp_description" style="display: none;">
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="pp_content_container" style="">
                    <div class="pp_left" style="">
                        <div class="pp_right" style="">
                            <div class="pp_content" style="height: 505px; display: block;">
                                <div class="pp_loaderIcon" style="display: none;">
                                </div>
                                <div class="pp_fade" style="display: block;">
                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                    <div class="pp_hoverContainer" style="height: 92px; width: 600px; display: none;">
                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                            style="visibility: visible;">previous</a>
                                    </div>
                                    <div class="pp_full_res">
                                        <div class="pp_inline clearfix">
                                            <div class="form_popup_box" style="padding-top: 15px;">
                                                <div class="label_bar">
                                                    <asp:HiddenField ID="hdnPopupTicketID" runat="server" />
                                                    <asp:HiddenField ID="hdnPopupStatusID" runat="server" />
                                                    <asp:HiddenField ID="hdnPopupIsSupplierTicket" runat="server" />
                                                    <span style="color: Black;"><b id="bTicketName">Ticket Name : </b></span>
                                                    <br />
                                                    <span style="color: Black;"><b id="bNotePopup">Ticket # : </b>
                                                        <br />
                                                        <asp:TextBox ID="txtExistingNotes" runat="server" CssClass="txtAreaFont" TextMode="MultiLine"
                                                            Height="200px" Width="95%"></asp:TextBox>
                                                        <br />
                                                        <br />
                                                        <b>Enter new note :</b>
                                                        <br />
                                                        <asp:TextBox Height="120px" Width="95%" TextMode="MultiLine" CssClass="txtAreaFont"
                                                            ID="txtNoteIE" runat="server"></asp:TextBox></span>
                                                </div>
                                                <br />
                                                <div>
                                                    <table class="true">
                                                        <tr>
                                                            <td style="float: left; margin-left: 10px; margin-right: 10px;">
                                                                <span class="custom-checkbox alignleft" id="spanCloseTicket" runat="server">
                                                                    <asp:CheckBox ID="chkCloseTicket" runat="server" />
                                                                </span><span style="float: left; margin-top: 3px; margin-left: 10px; font-size: 12px;
                                                                    background-color: inherit; color: Black; text-align: left; font-weight: bold;">Close
                                                                    Ticket</span>
                                                            </td>
                                                            <td style="float: left; margin-left: 10px; margin-right: 10px;">
                                                                <span class="custom-checkbox alignleft" id="spanFlagTicket" runat="server">
                                                                    <asp:CheckBox ID="chkFlagTicket" runat="server" />
                                                                </span><span style="float: left; margin-top: 3px; margin-left: 10px; font-size: 12px;
                                                                    background-color: inherit; color: Black; text-align: left; font-weight: bold;">Flag
                                                                    Ticket</span>
                                                            </td>
                                                            <td style="float: left; margin-left: 10px; margin-right: 10px;">
                                                                <span class="custom-checkbox alignleft" id="span1" runat="server">
                                                                    <asp:CheckBox ID="chkCustomerNote" runat="server" />
                                                                </span><span style="float: left; margin-top: 3px; margin-left: 10px; font-size: 12px;
                                                                    background-color: inherit; color: Black; text-align: left; font-weight: bold;"
                                                                    id="spanCustSuppNote">Customer Note</span>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkNoteHisForIE" runat="server" class="grey2_btn alignright"
                                                                    OnClick="lnkNoteHisForIE_Click"><span>Save Notes</span></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="pp_bottom" style="">
                    <div class="pp_left" style="">
                    </div>
                    <div class="pp_middle" style="">
                    </div>
                    <div class="pp_right" style="">
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div>
        <asp:Panel ID="pnlDueDate" runat="server" Style="display: none;">
            <div class="fancybox-overlay">
            </div>
            <div class="cboxWrapper" style="display: block; width: 268px; height: 199px; position: fixed;
                left: 40%; top: 35%;">
                <div style="">
                    <div class="cboxTopLeft" style="float: left;">
                    </div>
                    <div class="cboxTopCenter" style="float: left; width: 218px;">
                    </div>
                    <div class="cboxTopRight" style="float: left;">
                    </div>
                </div>
                <div style="clear: left;">
                    <div class="cboxMiddleLeft" style="float: left; height: 133px;">
                    </div>
                    <div class="cboxContent" style="float: left; width: 218px; display: block; height: 133px;">
                        <div class="cboxLoadedContent" style="display: block; overflow: visible;">
                            <div style="padding: 25px 10px 10px 10px;">
                                <span style="color: #72757C;"><b id="DueDatePopupTitle">Ticket no.:</b></span>
                                <div class="spacer10">
                                </div>
                                <div class="weather_form select_box_pad form_table" style="width: 100%; padding: 0px 0px 0px 0px;
                                    margin: 0px auto 10px;">
                                    <div class="calender_l">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 35%;">Due Date</span>
                                            <asp:TextBox ID="txtDueDate" TabIndex="1" runat="server" CssClass="datepicker1 w_label"
                                                Style="width: 45%"></asp:TextBox>
                                            <div id="dvDueDate">
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </div>
                                <div style="width: 100%;">
                                    <span class="btn_space">
                                        <asp:LinkButton ID="btnChangeDueDate" Text="Apply" TabIndex="2" CssClass="greysm_btn"
                                            runat="server" OnClick="btnChangeDueDate_Click"><span>Apply</span></asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                            <div class="cboxLoadingOverlay" style="height: 133px; display: none;">
                            </div>
                            <div class="cboxLoadingGraphic" style="height: 133px; display: none;">
                            </div>
                            <div class="cboxTitle" style="display: block;">
                            </div>
                        </div>
                        <div class="cboxClose" style="" id="closeDueDatePopup">
                            close</div>
                    </div>
                    <div class="cboxMiddleRight" style="float: left; height: 133px;">
                    </div>
                </div>
                <div style="clear: left;">
                    <div class="cboxBottomLeft" style="float: left;">
                    </div>
                    <div class="cboxBottomCenter" style="float: left; width: 218px;">
                    </div>
                    <div class="cboxBottomRight" style="float: left;">
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

