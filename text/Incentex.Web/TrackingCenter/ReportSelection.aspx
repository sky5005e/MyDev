<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ReportSelection.aspx.cs" Inherits="TrackingCenter_ReportSelection"
    Title="Tracking Center" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">        
        $().ready(function() {
            $(".action").click(function() {
                $('#dvLoader').show();
            });
        });
    </script>

    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div class="user_manage_btn btn_width_small">
            <table>
                <tr>
                    <td>
                        <asp:HyperLink ID="lnkUserAccess" CssClass="gredient_btn1 action" runat="server"
                            ToolTip="User System Access" NavigateUrl="~/TrackingCenter/UserSystemAccessRpt.aspx">
                                        <img alt='X' src="../Images/user-system-access.png" />
                                            <span>User System Access</span>
                        </asp:HyperLink>
                    </td>
                    <td>
                        <asp:HyperLink ID="lnkUserPurchase" class="gredient_btn1 action" title="User Purchases Report"
                            runat="server" NavigateUrl="~/TrackingCenter/UserPurchaseRpt.aspx">
                            <img alt='X' src="../Images/user-purchase-report.png" /><span>User Purchases Report</span>
                        </asp:HyperLink>
                    </td>
                    <td>
                        <asp:HyperLink ID="lnkWorldView" CssClass="gredient_btn1 action" runat="server" ToolTip="World View"
                            NavigateUrl="~/TrackingCenter/worldview.aspx">
                                        <img alt='X' src="../Images/world-view.png" />
                                            <span>World View</span>
                        </asp:HyperLink>
                    </td>
                </tr>
           <%-- </table>
            <table>--%>
                <tr>
                    <td>
                        <asp:HyperLink ID="lnkPageViewed" CssClass="gredient_btn1 action" runat="server"
                            ToolTip="Page Viewed" NavigateUrl="~/TrackingCenter/PageViewedHistory.aspx">
                                        <img alt='X' src="../Images/page-view.png" />
                                            <span>Page Viewed</span>
                        </asp:HyperLink>
                    </td>
                    <td>
                        <asp:HyperLink ID="lnkActivityReport" CssClass="gredient_btn1 action" runat="server"
                            ToolTip="Activity Report" NavigateUrl="~/TrackingCenter/ActivityReport.aspx">
                                        <img alt='X' src="../Images/activity-report.png" />
                                            <span>Activity Report</span>
                        </asp:HyperLink>
                    </td>
                    <td>
                        <asp:HyperLink ID="lnkBrowserType" class="gredient_btn1 action" title="Browser Type"
                            runat="server" NavigateUrl="~/TrackingCenter/BrowserType.aspx">
                            <img alt='X' src="../Images/browser-type.png" /><span>Browser Type</span>
                        </asp:HyperLink>
                    </td>
                </tr>
           <%-- </table>
            <table>--%>
                <tr>
                    <td>
                        <asp:HyperLink ID="lnkAccessHours" class="gredient_btn1 action" title="Access Hours"
                            runat="server" NavigateUrl="~/TrackingCenter/SystemAccessByHour.aspx">
                            <img alt='X' src="../Images/access-by-hour.png" /><span>Access By Hours</span>
                        </asp:HyperLink>
                    </td>
                    <td>
                        <asp:HyperLink ID="lnkSessionRecordings" class="gredient_btn1 action" title="Session Recordings"
                            runat="server" NavigateUrl="~/TrackingCenter/ViewVideoSession.aspx">
                            <img alt='X' src="../admin/Incentex_Used_Icons/session_recording.png" /><span>Session Recordings</span>
                        </asp:HyperLink>
                    </td>
                    <td >
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
