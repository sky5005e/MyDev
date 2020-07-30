<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RecentlyUpdated.aspx.cs" Inherits="MyAccount_RecentlyUpdated" Title="Recently Updated Tickets" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
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
    <link href="../CSS/colorbox.css" rel="stylesheet" type="text/css" />

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
                buttonImage: '..images/calender-icon.jpg',
                buttonImageOnly: true,
                minDate: new Date()
            });
        });
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $("#dvLoader").hide();            
            
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
        function NotePopup(id, notetype, ticketname) {
            $.ajax({
                type: "POST",
                url: "RecentlyUpdated.aspx/GetExistingNotes",
                data: "{'TicketID':'" + id + "', 'NoteType':'" + notetype + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg) {
                    $("#ctl00_ContentPlaceHolder1_txtExistingNotes").val(msg.d);
                }
            });
            $("#bNotePopup").html('Ticket # : ' + id + '.');
            $("#bTicketName").html('Ticket Name : ' + ticketname);
            $("#ctl00_ContentPlaceHolder1_hdnPopupTicketID").val(id);
            
            $("#ctl00_ContentPlaceHolder1_pnlNotes").show();            
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
    <div class="form_pad shipping_method_pad" style="width: 900px;">
        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
        <asp:HiddenField ID="hdnIsSuperAdmin" runat="server" Value="0" />
        <div style="text-align: center">
            <asp:Label ID="lblMsgList" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div style="text-align: right; color: #5C5B60; font-size: larger; font-weight: bold;
            padding-left: 15px;">
            Total Records :
            <asp:Label ID="lblRecords" Text="0" runat="server"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <div>
            <asp:GridView ID="gvServiceTickets" runat="server" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                OnRowCommand="gvServiceTickets_RowCommand" OnRowDataBound="gvServiceTickets_RowDataBound">
                <Columns>
                    <asp:TemplateField SortExpression="ServiceTicketNumber">
                        <HeaderTemplate>
                            <span class="centeralign">
                                <asp:LinkButton ID="lnkbtnHeaderTickteNumber" runat="server" CommandArgument="ServiceTicketNumber"
                                    CommandName="Sort">Ticket #</asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderTickteNumber" runat="server"></asp:PlaceHolder>
                            </span>
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="first centeralign">
                                <asp:LinkButton ID="lnkbtnItemTickteNumber" CommandName="TicketDetail" CommandArgument='<%# Eval("ServiceTicketID") %>'
                                    runat="server" ToolTip='<%# Convert.ToString(Eval("ServiceTicketNumber")) %>'
                                    Text='<%# Convert.ToString(Eval("ServiceTicketNumber")).Length > 10 ? Convert.ToString(Eval("ServiceTicketNumber")).Substring(0, 10).Trim() + "..." : Convert.ToString(Eval("ServiceTicketNumber")) %>'></asp:LinkButton>
                            </span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                            <asp:HiddenField runat="server" ID="hdnID" Value='<%#Eval("ServiceTicketID")%>' />
                            <asp:HiddenField runat="server" ID="hdnUnReadNotesExists" Value='<%#Eval("UnReadNotesExists") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%" />
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
                                    Text='<%# Convert.ToString(Eval("ServiceTicketName")).Length > 35 ? Convert.ToString(Eval("ServiceTicketName")).Substring(0, 35).Trim() + "..." : "&nbsp;" + Convert.ToString(Eval("ServiceTicketName")) %>'
                                    ToolTip='<%# Eval("ServiceTicketName") %>'></asp:LinkButton></span>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="40%" />
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
                        <ItemStyle CssClass="g_box" Width="19%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="DatePromisedFormatted">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnHeaderDatePromisedFormatted" runat="server" CommandArgument="DatePromisedFormatted"
                                CommandName="Sort">
                                        <span class="centeralign">Due Date</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderDatePromisedFormatted" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDatePromisedFormatted" Text='<%# Eval("DatePromisedFormatted") %>'
                                CssClass="centeralign" />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="TicketStatus">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnHeaderTicketStatus" runat="server" CommandArgument="TicketStatus"
                                CommandName="Sort">
                                        <span class="centeralign">Status</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderTicketStatus" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" CssClass="centeralign" ID="lblTicketStatus" Text='<%# Eval("TicketStatus") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="12%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span>Notes</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="btn_space">
                                <asp:Image AlternateText="X" ID="imgNote" ImageUrl="~/Images/edit_gray.png" runat="server" /></span>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box centeralign" Width="6%" />
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
                        <ItemStyle CssClass="g_box" Width="3%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div id="pagingtable" runat="server" class="alignright pagging">
                <asp:LinkButton ID="lnkbtnPrevious" class="prevb pagin" runat="server" OnClick="lnkbtnPrevious_Click"> 
                </asp:LinkButton>
                <span>
                    <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                        RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>' CssClass="pagin"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:DataList></span>
                <asp:LinkButton ID="lnkbtnNext" class="nextb pagin" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
            </div>
            <div class="spacer10">
            </div>
        </div>
    </div>
    <div>
        <asp:Panel ID="pnlNotes" runat="server" Style="display: none;">
            <div class="fancybox-overlay">
            </div>
            <div class="pp_pic_holder facebook" style="display: block; width: 640px; height: 505px;
                position: fixed; left: 25%; top: 15%;">
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
