<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="Settings.aspx.cs" Inherits="admin_IncentexEmployee_Settings" Title="Incentex Employee Settings" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(window).load(function() {
            $("#dvLoader").hide();
        });
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $("#dvLoader").show();
        });        
        
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
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
        <mb:MenuUserControl ID="manuControl" runat="server" />
        <div class="form_pad">
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div>
                <h4>
                    Incentex Employee :&ensp;<b><asp:Label ID="lblUserFullName" runat="server"></asp:Label></b></h4>
            </div>
            <div class="divider">
            </div>
            <div style="float: right; padding-right: 20px;">
                <h4>
                    Preferences</h4>
                <asp:DataList ID="dlPreferences" runat="server" OnItemDataBound="dlPreferences_ItemDataBound"
                    RepeatColumns="1" RepeatDirection="Vertical">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnKey" runat="server" Value='<%# Eval("PreferenceKey") %>' />
                        <asp:HiddenField ID="hdnKeyID" runat="server" Value='<%# Eval("PreferenceID") %>' />
                        <asp:HiddenField ID="hdnValue" runat="server" Value='<%# Eval("Value") %>' />
                        <asp:HiddenField ID="hdnValueID" runat="server" Value='<%# Eval("PreferenceValueID") %>' />
                        <div class="form_table">
                            <div class="select_box_pad">
                                <asp:Label ID="lblPreference" runat="server" Style="color: #72757C; font-size: 13px;
                                    line-height: 22px;" Text='<%# Eval("LableText") %>'></asp:Label>
                            </div>
                            <div class="spacer10">
                            </div>
                            <div class="select_box_pad">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="custom-sel">
                                        <asp:DropDownList ID="ddlPreference" runat="server" onchange="pageLoad(this,value);" />
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                            <br />
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <div style="float: left; padding-left: 20px;">
                <h4>
                    Manage Email</h4>
                <div class="form_table" id="dvManageEmail">
                    <table>
                        <tr>
                            <td class="formtd">
                                <table class="checktable_supplier true">
                                    <tr>
                                        <td>
                                            <asp:DataList ID="dtManageEmail" runat="server" OnItemDataBound="dtManageEmail_ItemDataBound">
                                                <ItemTemplate>
                                                    <span class="custom-checkbox alignleft" id="ManageEmailspan" runat="server">
                                                        <asp:CheckBox ID="chkManageEmail" runat="server" />
                                                    </span>
                                                    <asp:Label ID="lblManageEmail" Text='<%# Eval("ManageEmailName") %>' runat="server"
                                                        Style="color: #72757C; line-height: 22px; font-size: 13px;"></asp:Label>
                                                    <asp:HiddenField ID="hdnManageEmailID" runat="server" Value='<%#Eval("ManageEmailID")%>' />
                                                    <asp:HiddenField ID="hdnUserInfoID" runat="server" Value='<%#Eval("UserInfoID")%>' />
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
            <div class="spacer25" style="clear: both;">
            </div>
            <div class="spacer25">
            </div>
            <div class="centeralign">
                <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" OnClick="lnkSave_Click"><span>Save Information</span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
