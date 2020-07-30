<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="MenuAccess.aspx.cs" Inherits="admin_Company_Employee_MenuAccess" Title="Company Employee >> Menu Access" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
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

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div class="spacer10">
    </div>
    <div class="tabcontent" id="menu_access">
        <div class="alignnone">
            &nbsp;</div>
        <div class="form_pad">
            <div class="employee_name">
                User Name:
                <asp:Label ID="lblUserFullName" runat="server" Text=""></asp:Label>
            </div>
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div class="form_table clearfix">
                <div class="alignleft" style="width: 34.5%;">
                    <h4>
                        Main Menu Options</h4>
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
                </div>
                <div class="alignleft" style="width: 65.5%;">
                    <h4>
                        Preferences</h4>
                    <table class="checktable_supplier true">
                        <tr>
                            <td>
                                <asp:DataList ID="dtlPreferences" runat="server">
                                    <ItemTemplate>
                                        <span class="custom-checkbox alignleft" id="spanPreferences" runat="server">
                                            <asp:CheckBox ID="chkPreferences" runat="server" />
                                        </span>
                                        <label>
                                            <asp:Label ID="lblPreferences" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                        <asp:HiddenField ID="hdnPreferences" runat="server" Value='<%# Eval("iLookupID") %>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
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
            <div class="divider">
            </div>
            <h4>
                Other Features</h4>
            <div class="form_table">
                <div style="height: 150px; overflow: auto; margin-left: 2px; margin-top: -10px;">
                    <table>
                        <tr>
                            <td class="formtd">
                                <table class="checktable_supplier true">
                                    <tr>
                                        <td>
                                            <span class="custom-checkbox alignleft" id="displaytotalSpan" runat="server">
                                                <asp:CheckBox ID="chkDispTotalOrderAmount" runat="server" /></span>
                                            <label>
                                                <asp:Label ID="lblDispTotalOrderAmount" Text='Display Total Order Amount' runat="server"></asp:Label></label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="divider">
            </div>
            <h4>
                Base Stations Access</h4>
            <div class="form_table">
                <div style="height: 150px; overflow: auto; margin-left: 2px; margin-top: -10px;">
                    <table>
                        <tr>
                            <td class="formtd">
                                <table class="checktable_supplier true">
                                    <tr>
                                        <td>
                                            <asp:DataList ID="dtBaseStations" runat="server">
                                                <ItemTemplate>
                                                    <span class="custom-checkbox alignleft" id="BaseStationsNamespan" runat="server">
                                                        <asp:CheckBox ID="chkBaseStationsName" runat="server" /></span>
                                                    <label>
                                                        <asp:Label ID="lblBaseStationsName" Text='<%# Eval("sBaseStation") %>' runat="server"></asp:Label></label>
                                                    <asp:HiddenField ID="hdnBaseStationID" runat="server" Value='<%#Eval("iBaseStationId")%>' />
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
            <div class="botbtn centeralign">
                <%--<a href="#" class="grey2_btn" title="Edit Information"><span>Edit Information</span></a>--%>
                <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" OnClientClick="checkpaymentoption();"
                    runat="server" ToolTip="Save Information" OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
