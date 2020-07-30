<%@ Page Title="View VideoSession" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewVideoSession.aspx.cs" Inherits="TrackingCenter_ViewVideoSession"
    %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
            <table style="padding-left: 180px;">
                <tr>
                    <td>
                        <asp:HyperLink ID="lnkViewTodaySessions" CssClass="gredient_btn1 action" runat="server"
                            ToolTip="View Today's Session">
                            <img src="../admin/Incentex_Used_Icons/view-today-sessions.png" alt="Create Campaign" />
                            <span>View Today's Sessions</span>
                        </asp:HyperLink>
                    </td>
                    <td>
                        <asp:HyperLink ID="lnkViewCartAbandonments" class="gredient_btn1 action" title="View Cart Abandonment's"
                            runat="server" >
                             <img src="../admin/Incentex_Used_Icons/view-cart-abandoment.png" alt="Create Campaign" />
                            <span>View Cart Abandonment's</span>
                        </asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HyperLink ID="lnkErrorMessageSessions" CssClass="gredient_btn1 action" runat="server" 
                        ToolTip="Error Message Sessions" >
                        <img src="../admin/Incentex_Used_Icons/error-message-sessions.png" alt="Create Campaign" />
                        <span>Error Message Sessions</span>
                        </asp:HyperLink>
                    </td>
                    <td>
                        <asp:HyperLink ID="lnkSessionReport" CssClass="gredient_btn1 action" runat="server"
                            ToolTip="Session Report" >
                            <img src="../admin/Incentex_Used_Icons/session-report.png" alt="Create Campaign" />
                            <span>Session Report</span>
                        </asp:HyperLink>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
