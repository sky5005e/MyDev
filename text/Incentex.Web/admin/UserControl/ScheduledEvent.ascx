<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ScheduledEvent.ascx.cs"
    Inherits="admin_UserControl_ScheduledEvent" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<script type="text/javascript" src="../JS/JQuery/jquery-ui-timepicker-addon.js"></script>
<style type="text/css">
.scheduled_event
{
	padding:5px 0px;
}
.datetimedetail
{
	margin-left:20px;
	margin-top:10px;
}
.datetimedetail input
{
	margin-bottom:5px;
}
</style>
<script type="text/javascript" language="javascript">
        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                 //dateFormat: 'dd-mm-yy',
                buttonImage: '../icons/icon_calendar.gif',
                buttonImageOnly: true
            });
        });

        $(function() {
            $('#ctl00_ContentPlaceHolder1_ucScheduledEvent_dtScheduledEvent_ctl00_txtEventTime').timepicker({});
        });
</script>
<asp:LinkButton ID="lnkDummyEvent" runat="server" Style="display: none"></asp:LinkButton>
<at:ModalPopupExtender ID="mpeScheduledEvent" TargetControlID="lnkDummyEvent" BackgroundCssClass="modalBackground"
    DropShadow="true" runat="server" CancelControlID="closepopupevent" PopupControlID="pnlScheduledEvent">
</at:ModalPopupExtender>
<at:ToolkitScriptManager ID="ScriptManager1" runat="server">
</at:ToolkitScriptManager>
<asp:Panel ID="pnlScheduledEvent" runat="server" Style="display: none;">
    <div class="pp_pic_holder facebook" style="display: block; width: 350px; position:fixed;left:35%;top:30%;">
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
                    <div class="pp_content" style="height: auto; display: block;">
                        <div class="pp_fade" style="display: block;">
                            <div class="clearfix">
                                <a href="#" id="closepopupevent" style="position: inherit;" runat="server" class="pp_close">
                                    Close</a>
                            </div>
                            <div id="pp_full_res">
                                <div class="pp_inline clearfix">
                                    <h4>Scheduled Events</h4>
                                    <asp:UpdatePanel runat="server" ID="upnlScheduledEvent">
                                        <ContentTemplate>
                                            <div style="text-align: center">
                                                <asp:Label ID="lblErrorMsg" runat="server" CssClass="errormessage"></asp:Label>
                                            </div>
                                            <asp:DataList ID="dtScheduledEvent" runat="server" OnPreRender="ShowFooter_OnPreRender" ShowFooter="false" RepeatDirection="Horizontal" RepeatColumns="1"
                                                OnItemCommand="dtScheduledEvent_ItemCommand" OnItemDataBound="dtScheduledEvent_ItemDataBound">
                                                <ItemStyle CssClass="scheduled_event"/>
                                                <ItemTemplate>
                                                    <strong><%#Container.ItemIndex + 1 %>) </strong>
                                                    <asp:Label runat="server" ID="lblEventName" Text='<%# Eval("EventName")%>'></asp:Label>
                                                    <div class="alignright">
                                                        <asp:LinkButton ID="lnkbtnCompleted" ToolTip="Completed" CommandName="Completed" CommandArgument='<%# Eval("EventID")%>' runat="server">
                                                            <span>
                                                            <img height="24" width="24" alt="completed" src="../Images/completed-icon.png"/>
                                                            </span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnkbtnRemindMeLatter" ToolTip="Remind Me Latter" CommandName="RemindMeLatter" CommandArgument='<%# Eval("EventID")%>' runat="server">
                                                            <span>
                                                            <img alt="RemindMeLatter" src="../Images/remindlatter.png"/>
                                                            </span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnkbtnRescheduled" ToolTip="Re-Scheduled" CommandName="Rescheduled" CommandArgument='<%# Eval("EventID")%>' runat="server">
                                                            <span>
                                                            <img alt="Rescheduled" src="../Images/reschedule.png"/>
                                                            </span>
                                                        </asp:LinkButton>
                                                    </div>
                                                    <div runat="server" class="datetimedetail" id="dvRescheduled" visible="false">
                                                        <asp:TextBox runat="server" ID="txtEventDate" CssClass="datepicker" Text='<%# Eval("EventDate")%>'></asp:TextBox><br />
                                                        <asp:TextBox runat="server" ID="txtEventTime" Text='<%# Eval("EventTime")%>'></asp:TextBox><br />
                                                        <asp:Button runat="server" ID="btnEventRescheduled" Text="Apply" CommandName="SubmitChange" CommandArgument='<%# Eval("EventID")%>'/>
                                                    </div>
                                                    <div runat="server" class="datetimedetail" id="dvCompletedWithNote" visible="false">
                                                        <asp:TextBox runat="server" ID="txtNote" TextMode="MultiLine"></asp:TextBox><br />
                                                        <asp:Button runat="server" ID="btnEventCompleted" Text="Submit" CommandName="CompletedWithNote" CommandArgument='<%# Eval("EventID")%>' />
                                                    </div>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: center;">
                                                        No event available.</div>
                                                </FooterTemplate>
                                            </asp:DataList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
