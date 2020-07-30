<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="GlobalMenuAccess.aspx.cs" Inherits="admin_Company_Employee_MenuAccess"
    Title="Store Workgroup >> Menu Access" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<style type="text/css">
        .basic_link .manage_link a, .header_bg
        {
        	text-align:left;
        }
        .basic_link img
        {
        	margin:0 4px;
        }    
</style>
    <script type="text/javascript" language="javascript">
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
            
            $("#ctl00_ContentPlaceHolder1_ddlWorkGroupManager").change(function() {
                $('#dvLoader').show();
            });
            
            $(window).scroll(function () {
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());            
        });
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
        <div class="employee_name">
            Work Group:
            <asp:Label ID="lblWorkGroup" runat="server" Text=""></asp:Label>
        </div>
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <h4>
            Main Menu Options</h4>
        <div class="form_table">
            <table>
                <tr>
                    <td class="formtd">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <asp:DataList ID="dtlMenus" runat="server">
                                        <ItemTemplate>
                                            <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                                <asp:CheckBox ID="chkdtlMenus" runat="server" />
                                            </span>
                                            <label>
                                                <asp:Label ID="lblMenus" Text='<%# Eval("sDescription") %>' runat="server"></asp:Label></label>
                                            <asp:HiddenField ID="hdnMenuAccess" runat="server" Value='<%# Eval("iMenuPrivilegeID") %>' />
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd">
                        <div>
                            <label style="color: #72757C; line-height: 22px;">
                                Work Group Manager</label>
                        </div>
                        <div class="spacer5">
                        </div>
                        <div class="select_box_pad alignleft">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="custom-sel">
                                    <asp:DropDownList ID="ddlWorkGroupManager" OnSelectedIndexChanged="ddlWorkGroupManager_SelectedIndexChanged"
                                        runat="server" onchange="pageLoad(this,value);" AutoPostBack="true" />
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        
                        <asp:DataList ID="dlPreferences" runat="server" OnItemDataBound="dlPreferences_ItemDataBound"
                                RepeatColumns="1" RepeatDirection="Vertical">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnKey" runat="server" Value='<%# Eval("PreferenceKey") %>' />
                                    <asp:HiddenField ID="hdnKeyID" runat="server" Value='<%# Eval("PreferenceID") %>' />
                                    <asp:HiddenField ID="hdnValue" runat="server" Value='<%# Eval("Value") %>' />
                                    <asp:HiddenField ID="hdnValueID" runat="server" Value='<%# Eval("PreferenceValueID") %>' />
                                    <div style="padding-top:10px;">
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
                                                    <asp:DropDownList ID="ddlPreference" runat="server" onchange="pageLoad(this,value);" 
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlPreference_SelectedIndexChanged"/>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                        <br />
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <div class="form_table clearfix">
            <div class="formtd alignleft">
                <h4>
                    Employee Uniforms</h4>
                <table class="checktable_supplier true">
                    <tr>
                        <td>
                            <asp:DataList ID="dtEmpUniform" runat="server">
                                <ItemTemplate>
                                    <span class="custom-checkbox alignleft" id="menuemployeeuni" runat="server">
                                        <asp:CheckBox ID="chkEmpUniform" runat="server" />
                                    </span>
                                    <label>
                                        <asp:Label ID="lblUniform" Text='<%# Eval("SubCategoryName") %>' runat="server"></asp:Label></label>
                                    <asp:HiddenField ID="hdnEmpUni" runat="server" Value='<%#Eval("SubCategoryID")%>' />
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="formtd alignleft">
                <h4>
                    Additional Information</h4>
                <table class="checktable_supplier true">
                    <tr>
                        <td>
                            <asp:DataList ID="dtAddiInfo" runat="server">
                                <ItemTemplate>
                                    <span class="custom-checkbox alignleft" id="addinfospan" runat="server">
                                        <asp:CheckBox ID="chkAddiInfo" runat="server" />
                                    </span>
                                    <label>
                                        <asp:Label ID="lblAddiInfo" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                    <asp:HiddenField ID="hdnAddiInfo" runat="server" Value='<%#Eval("iLookupID")%>' />
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                </table>
                <div class="spacer25">
                    &nbsp;</div>
                <h4>
                    Company Store</h4>
                <table class="checktable_supplier true">
                    <tr>
                        <td>
                            <asp:DataList ID="dtCompanyStore" runat="server" RepeatColumns="2">
                                <ItemTemplate>
                                    <span class="custom-checkbox alignleft" id="companystorespan" runat="server">
                                        <asp:CheckBox ID="chkCompanyStore" runat="server" /></span>
                                    <label>
                                        <asp:Label ID="lblCompanyStore" Text='<%# Eval("SubCategoryName") %>' runat="server"></asp:Label></label>
                                    <asp:HiddenField ID="hdnCompanyStore" runat="server" Value='<%#Eval("SubCategoryID")%>' />
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="divider">
        </div>
        <h4>
            Uniform Purchasing</h4>
        <div class="form_table">
            <table>
                <tr>
                    <td class="formtd">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <asp:DataList ID="dtUniPurchasing" runat="server">
                                        <ItemTemplate>
                                            <span class="custom-checkbox alignleft" id="uniformspan" runat="server">
                                                <asp:CheckBox ID="chkUniPurchasing" runat="server" /></span>
                                            <label>
                                                <asp:Label ID="lblUniPurchasing" Text='<%# Eval("SubCategoryName") %>' runat="server"></asp:Label></label>
                                            <asp:HiddenField ID="hdnUniformPurchasing" runat="server" Value='<%#Eval("SubCategoryID")%>' />
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <h4>
            Supplies/Equipment
        </h4>
        <div class="form_table">
            <table>
                <tr>
                    <td class="formtd">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <asp:DataList ID="dtSupplies" runat="server">
                                        <ItemTemplate>
                                            <span class="custom-checkbox alignleft" id="supplliespan" runat="server">
                                                <asp:CheckBox ID="chkSupplies" runat="server" /></span>
                                            <label>
                                                <asp:Label ID="lblSupplies" Text='<%# Eval("SubCategoryName")%>' runat="server"></asp:Label></label>
                                            <asp:HiddenField ID="hdnSupplies" runat="server" Value='<%#Eval("SubCategoryID")%>' />
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="botbtn centeralign">
            <asp:LinkButton ID="lnkBtnApplyAndOverride" class="grey2_btn" runat="server" ToolTip="Apply to New CE/CA and remove previous setting of existing CE/CA and apply this setting to existing CE/CA too."
                OnClick="lnkBtnApplyAndOverride_Click"><span>Apply & Override Existing</span></asp:LinkButton>
            <asp:LinkButton ID="lnkbtnApplyAndAddition" class="grey2_btn" runat="server" ToolTip="Apply to New CE/CA and add this setting to existing CE/CA previous settings."
                OnClick="lnkbtnApplyAndAddition_Click"><span>Apply & Addition to Existing</span></asp:LinkButton>
        </div>
    </div>
</asp:Content>
