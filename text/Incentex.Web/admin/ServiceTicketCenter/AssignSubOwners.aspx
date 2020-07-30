<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="AssignSubOwners.aspx.cs" Inherits="admin_ServiceTicketCenter_AssignSubOwners"
    Title="Assign Sub Owners" MaintainScrollPositionOnPostback="true" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function HideTodo(id, subowner, ownid) {
            document.getElementById('ctl00_ContentPlaceHolder1_hdnTempID').value = ownid;
            document.getElementById('ctl00_ContentPlaceHolder1_hdnSubOwner').value = subowner;
            $('#dvLoader').show();
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
                        <td style="width: 33%;">
                            <h3 style="float: left; margin-right: 7px; color: #B0B0B0;">
                                Ticket No. :</h3>
                            <asp:Label ID="lblTicketNumber" runat="server" Style="float: left; color: #72757C;
                                line-height: 23px; font-size: 15px;" />
                        </td>
                        <td style="width: 33%;">
                            <h3 style="float: left; margin-right: 7px; color: #B0B0B0;">
                                Ticket Name :</h3>
                            <asp:Label ID="lblTicketName" runat="server" Style="float: left; color: #72757C;
                                line-height: 23px; font-size: 15px;" />
                        </td>
                        <td style="width: 33%;">
                            <h3 style="float: left; margin-right: 7px; color: #B0B0B0;">
                                Ticket Status :</h3>
                            <asp:Label ID="lblTicketStatus" runat="server" Style="float: left; color: #72757C;
                                line-height: 23px; font-size: 15px;" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="clear: both;">
                <h4 style="float: left; margin-right: 7px; color: #B0B0B0;">
                    Owner :</h4>
                <asp:Label ID="lblOwnerName" runat="server" Text="" Style="float: left; color: #72757C;
                    line-height: 23px;" />
            </div>
            <div style="clear: both;">
                <h4 style="color: #B0B0B0;">
                    Sub Owners</h4>
                <div class="form_table">
                    <table>
                        <tr>
                            <td class="formtd" style="width: 50%;">
                                <h6 style="color: #B0B0B0;">
                                    Incentex Employees</h6>
                                <table class="checktable_supplier true">
                                    <tr>
                                        <td>
                                            <asp:DataList ID="dtlIESubOwners" runat="server" OnItemDataBound="dtlSubOwners_ItemDataBound"
                                                RepeatColumns="2" RepeatDirection="Horizontal">
                                                <ItemTemplate>
                                                    <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                                        <asp:CheckBox ID="chkSubOwners" Checked='<%# Convert.ToBoolean(Eval("Existing")) %>'
                                                            runat="server" OnCheckedChanged="chkSubOwners_CheckedChanged" AutoPostBack="true" />
                                                    </span>
                                                    <label>
                                                        <asp:Label ID="lblSubOwners" Text='<%# Convert.ToString(Eval("FirstName")) + " " + Convert.ToString(Eval("LastName")) %>'
                                                            runat="server"></asp:Label></label>
                                                    <asp:HiddenField ID="hdnSubOwnerID" runat="server" Value='<%# Eval("SubOwnerID") %>' />
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="formtd" style="width: 50%;">
                                <h6 style="color: #B0B0B0;" id="h6CASupps" runat="server">
                                    Company Admins</h6>
                                <table class="checktable_supplier true">
                                    <tr>
                                        <td>
                                            <asp:DataList ID="dtlCASubOwners" runat="server" OnItemDataBound="dtlSubOwners_ItemDataBound"
                                                RepeatColumns="2" RepeatDirection="Horizontal">
                                                <ItemTemplate>
                                                    <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                                        <asp:CheckBox ID="chkSubOwners" Checked='<%# Convert.ToBoolean(Eval("Existing")) %>'
                                                            runat="server" OnCheckedChanged="chkSubOwners_CheckedChanged" AutoPostBack="true" />
                                                    </span>
                                                    <label>
                                                        <asp:Label ID="lblSubOwners" Text='<%# Convert.ToString(Eval("FirstName")) + " " + Convert.ToString(Eval("LastName")) %>'
                                                            runat="server"></asp:Label></label>
                                                    <asp:HiddenField ID="hdnSubOwnerID" runat="server" Value='<%# Eval("SubOwnerID") %>' />
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnTempID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnSubOwner" runat="server" Value="" />
</asp:Content>
