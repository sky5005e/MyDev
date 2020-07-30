<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AdditionalWorkgroup.aspx.cs" Inherits="admin_Company_Employee_AdditionalWorkgroup" Title="Company Employee >> Additional Workgroup" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                User Name: <asp:Label ID="lblUserFullName" runat="server" Text=""></asp:Label>
            </div>
              <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <h4>
                Additional Workgroup</h4>
            <div class="form_table">
                <table>
                    <tr>
                        <td class="formtd">
                            <table class="checktable_supplier true">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dtlWorkgroup" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                                    <asp:CheckBox ID="chkdtlMenus" runat="server" />
                                                </span>
                                                <label>
                                                    <asp:Label ID="lblWorkgroupName" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnWorkgroupID" runat="server" Value='<%# Eval("iLookupID") %>' />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            
            <div class="botbtn">
             <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" 
             runat="server" ToolTip="Save Information" onclick="lnkBtnSaveInfo_Click1"><span>Save Information</span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>

