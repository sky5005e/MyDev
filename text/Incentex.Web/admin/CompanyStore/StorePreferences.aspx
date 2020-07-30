<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="StorePreferences.aspx.cs" Inherits="admin_CompanyStore_StorePreferences"
    Title="Store Preferences" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(window).load(function() {
            $("#dvLoader").hide();
        });
        
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
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

    <style type="text/css">
        .form_pad .divider
        {
            height: 15px;
        }
    </style>
    <div id="dvLoader">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="tabcontent" id="menu_access">
        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
        <div class="alignnone">
            &nbsp;</div>
        <div class="form_pad">
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div>
                <h3 style="float: left; margin-right: 7px; color: #B0B0B0;">
                    Store :</h3>
                <asp:Label ID="lblStore" runat="server" Style="float: left; color: #72757C; line-height: 23px;
                    font-size: 15px;" />
            </div>
            <div class="spacer10">
            </div>
            <div class="divider">
            </div>
            <div class="form_table clearfix">
                <table>
                    <tr>
                        <td>
                        </td>
                        <asp:DataList ID="dlPreferences" runat="server" OnItemDataBound="dlPreferences_ItemDataBound"
                            RepeatColumns="1" RepeatDirection="Vertical">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnKey" runat="server" Value='<%# Eval("PreferenceKey") %>' />
                                <asp:HiddenField ID="hdnKeyID" runat="server" Value='<%# Eval("PreferenceID") %>' />
                                <asp:HiddenField ID="hdnValue" runat="server" Value='<%# Eval("Value") %>' />
                                <asp:HiddenField ID="hdnValueID" runat="server" Value='<%# Eval("PreferenceValueID") %>' />
                                <div>
                                    <div>
                                        <asp:Label ID="lblPreference" runat="server" Style="color: #72757C; line-height: 22px;"
                                            Text='<%# Eval("LableText") %>'></asp:Label>
                                    </div>
                                    <div class="spacer5">
                                    </div>
                                    <div class="select_box_pad alignleft">
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
                    </tr>
                </table>
            </div>
            <div class="divider">
            </div>
            <div style="clear: both;">
                <h4 style="color: #B0B0B0;">
                    Work Groups</h4>
                <div class="form_table">
                    <table class="checktable_supplier true">
                        <tr>
                            <td>
                                <asp:DataList ID="dtlWorkGroup" runat="server" OnItemDataBound="dtlWorkGroup_ItemDataBound"
                                    RepeatColumns="4" RepeatDirection="Vertical" RepeatLayout="Table">
                                    <ItemTemplate>
                                        <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                            <asp:CheckBox ID="chkWorkGroup" Checked='<%# Convert.ToBoolean(Eval("Existing")) %>'
                                                runat="server" />
                                        </span>
                                        <label>
                                            <asp:Label ID="lblWorkGroup" Text='<%# Eval("WorkGroup") %>' runat="server"></asp:Label></label>
                                        <asp:HiddenField ID="hdnWorkGroupID" runat="server" Value='<%# Eval("WorkGroupID") %>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="divider">
            </div>
            <div style="clear: both;">
                <h4 style="color: #B0B0B0;">
                    Departments</h4>
                <div class="form_table">
                    <table class="checktable_supplier true">
                        <tr>
                            <td>
                                <asp:DataList ID="dtlDepartment" runat="server" OnItemDataBound="dtlDepartment_ItemDataBound"
                                    RepeatColumns="4" RepeatDirection="Vertical" RepeatLayout="Table">
                                    <ItemTemplate>
                                        <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                            <asp:CheckBox ID="chkDepartment" Checked='<%# Convert.ToBoolean(Eval("Existing")) %>'
                                                runat="server" />
                                        </span>
                                        <label>
                                            <asp:Label ID="lblDepartment" Text='<%# Eval("Department") %>' runat="server"></asp:Label></label>
                                        <asp:HiddenField ID="hdnDepartmentID" runat="server" Value='<%# Eval("DepartmentID") %>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="centeralign" style="padding-top: 140px;">
                <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" OnClick="lnkSave_Click"><span>Save Information</span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>