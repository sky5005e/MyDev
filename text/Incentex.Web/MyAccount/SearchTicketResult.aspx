<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SearchTicketResult.aspx.cs" Inherits="MyAccount_SearchTicketResult"
    Title="Support Tickets" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
        function CollapsibleContainerTitleOnClick() {
            // The item clicked is the title div... get this parent (the overall container) and toggle the content within it.
            var hdnIamExpanded = document.getElementById($(this).parent().attr("id").substring(0, $(this).parent().attr("id").length - 13) + "hdnIamExpanded");
            
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
                buttonImage: '../images/calender-icon.jpg',
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
            
            $("#ctl00_ContentPlaceHolder1_lnkButton").click(function() {
                $('#dvLoader').show();
            });
            
            $("#ctl00_ContentPlaceHolder1_lnkClose").click(function() {
                $('#dvLoader').show();
            });
            
            $(".pagin").click(function() {
                $('#dvLoader').show();
            });
            
            $(window).scroll(function () {
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());            
        });
    </script>

    <script language="javascript" type="text/javascript">
        function NotePopup(id, notetype, statusid, ticketname) {            
            $.ajax({
                type: "POST",
                url: "SearchTicketResult.aspx/GetExistingNotes",
                data: "{'TicketID':'" + id + "', 'NoteType':'" + notetype + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg) {
                    $("#ctl00_ContentPlaceHolder1_txtExistingNotes").val(msg.d);
                }
            });
            $("#bNotePopup").html('Notes for ticket no.: ' + id + '.');
            $("#bTicketName").html('Ticket Name : ' + ticketname);
            $("#ctl00_ContentPlaceHolder1_hdnPopupStatusID").val(statusid);
            $("#ctl00_ContentPlaceHolder1_hdnPopupTicketID").val(id);
            $("#ctl00_ContentPlaceHolder1_pnlNotes").show();            
        }
        
        function FlagTicket(ticketid) {
            $("#ctl00_ContentPlaceHolder1_hdnPopupTicketID").val(ticketid);
            $('#dvLoader').show();
        }
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad shipping_method_pad" style="width: 900px;">
        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
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
                            OnRowCommand="gvServiceTickets_RowCommand">
                            <Columns>
                                <asp:TemplateField SortExpression="ServiceTicketNumber">
                                    <HeaderTemplate>
                                        <span class="centeralign">
                                            <asp:LinkButton ID="lnkbtnHeaderTickteNumber" runat="server" CommandArgument="ServiceTicketNumber"
                                                CommandName="Sort">Ticket #</asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderTickteNumber" runat="server"></asp:PlaceHolder>
                                        </span>
                                        <div id="dvHeaderTN" runat="server" class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span id="spanItemTN" runat="server" class="first centeralign">
                                            <asp:LinkButton ID="lnkbtnItemTickteNumber" CommandName="TicketDetail" CommandArgument='<%# Eval("ServiceTicketID") %>'
                                                runat="server" ToolTip='<%# Convert.ToString(Eval("ServiceTicketNumber")) %>'
                                                Text='<%# Convert.ToString(Eval("ServiceTicketNumber")).Length > 10 ? Convert.ToString(Eval("ServiceTicketNumber")).Substring(0, 10).Trim() + "..." : Convert.ToString(Eval("ServiceTicketNumber")) %>'></asp:LinkButton>
                                        </span>
                                        <div id="dvItemTN" runat="server" class="corner">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>
                                        <asp:HiddenField runat="server" ID="hdnID" Value='<%#Eval("ServiceTicketID")%>' />
                                        <asp:HiddenField runat="server" ID="hdnUnReadNotesExists" Value='<%#Eval("UnReadNotesExists") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ContactName">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnHeaderContactName" runat="server" CommandArgument="ContactName"
                                            CommandName="Sort"><span class="centeralign">Contact</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderContactName" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblContactName" Text='<%# Convert.ToString(Eval("ContactName")).Length > 20 ? Convert.ToString(Eval("ContactName")).Substring(0, 20).Trim() + "..." : "&nbsp;" + Convert.ToString(Eval("ContactName")) %>'
                                            ToolTip='<%# Eval("ContactName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="20%" />
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
                                                Text='<%# Convert.ToString(Eval("ServiceTicketName")).Length > 30 ? Convert.ToString(Eval("ServiceTicketName")).Substring(0, 30).Trim() + "..." : "&nbsp;" + Convert.ToString(Eval("ServiceTicketName")) %>'
                                                ToolTip='<%# Eval("ServiceTicketName") %>'></asp:LinkButton></span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="30%" />
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
                                    <ItemStyle CssClass="b_box" Width="18%" />
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
                                    <ItemStyle CssClass="g_box" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="TicketStatus">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnHeaderTicketStatus" runat="server" CommandArgument="TicketStatus"
                                            CommandName="Sort">
                                        <span class="centeralign">Status</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderTicketStatus" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lnkTicketStatus" CssClass="centeralign" Text='<%# Eval("TicketStatus") %>' />
                                        <asp:HiddenField ID="hdnGvStatusID" runat="server" Value='<%# Eval("TicketStatusID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="10%" />
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
        <asp:Panel ID="pnlNotes" runat="server" Style="display: none;">
            <div class="fancybox-overlay">
            </div>
            <div class="pp_pic_holder facebook" style="display: block; width: 640px; height: 505px;
                position: fixed; left: 25%; top: 15%; "">
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
                            <div class="pp_content" style="height: 505px; display: block;">
                                <div class="pp_loaderIcon" style="display: none;">
                                </div>
                                <div class="pp_fade" style="display: block;">
                                    <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                    <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                        <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                            style="visibility: visible;">previous</a>
                                    </div>
                                    <div id="pp_full_res">
                                        <div class="pp_inline clearfix">
                                            <div class="form_popup_box">
                                                <div class="label_bar">
                                                    <asp:HiddenField ID="hdnPopupTicketID" runat="server" />
                                                    <asp:HiddenField ID="hdnPopupStatusID" runat="server" />
                                                    <span style="color: Black;"><b id="bTicketName">Ticket Name : </b></span>
                                                    <br />
                                                    <span style="color: Black;"><b id="bNotePopup">Notes for ticket no.:</b>
                                                        <br />
                                                        <asp:TextBox ID="txtExistingNotes" runat="server" CssClass="txtAreaFont" TextMode="MultiLine"
                                                            Height="200px" Width="95%"></asp:TextBox>
                                                        <br />
                                                        <br />
                                                        <b>Enter new note :</b>
                                                        <br />
                                                        <asp:TextBox Height="120px" Width="95%" TextMode="MultiLine" CssClass="txtAreaFont"
                                                            ID="txtNote" runat="server"></asp:TextBox></span>
                                                </div>
                                                <div>
                                                    <asp:LinkButton ID="lnkButton" class="grey2_btn alignright" runat="server" OnClick="lnkButton_Click"><span>Save Notes</span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="pp_details clearfix" style="width: 371px;">
                                        <asp:LinkButton runat="server" ID="lnkClose" CssClass="pp_close" OnClick="lnkClose_Click">Close</asp:LinkButton>
                                        <p class="pp_description" style="display: none;">
                                        </p>
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
</asp:Content>
