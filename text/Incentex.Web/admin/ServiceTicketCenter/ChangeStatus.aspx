<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ChangeStatus.aspx.cs" Inherits="admin_ServiceTicketCenter_ChangeStatus"
    Title="Close Support Ticket" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $('#dvLoader').hide();
            
            $("#ctl00_ContentPlaceHolder1_lnkCloseTicket").click(function() {
                $('#dvLoader').show();
            });
            
            $(window).scroll(function () {
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());            
        });
    </script>

    <div id="dvLoader">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="tabcontent" id="menu_access">
        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
        <div class="alignnone">
            &nbsp;</div>
        <div class="form_pad">
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div>
                <table>
                    <tr>
                        <td width="45%;">
                            <asp:Label ID="lblOwnerName" runat="server" Text="" Style="color: #72757C; font-size: 15px;
                                line-height: 25px;" />
                            <asp:LinkButton ID="lnkCloseTicket" CssClass="btn_red" TabIndex="5" runat="server"
                                ToolTip="Close Ticket" OnClick="lnkCloseTicket_Click"><span>Close Ticket</span></asp:LinkButton>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
