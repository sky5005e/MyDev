<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="PreIssuancePopup.aspx.cs" Inherits="admin_CompanyStore_CompanyPrograms_PreIssuancePopup" Title="Untitled Page" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
   
    <div class="form_pad">
        <div>
            <div style="text-align: center; color: Red; font-size: larger;">
                <asp:Label ID="lblmsg" runat="server">
                </asp:Label>
            </div>
            <div class="dropdown_pad form_table">
            <div>
                <asp:TextBox ID="txtNO" runat="server"></asp:TextBox>
            </div>
            
              <asp:gridview ID="Gridview1" runat="server" ShowFooter="true" AutoGenerateColumns="false">
        <Columns>
       
        <asp:TemplateField HeaderText="Header 1">
            <ItemTemplate>
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
         <FooterStyle HorizontalAlign="Right" />
            <FooterTemplate>
             <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" 
                    onclick="ButtonAdd_Click1" />
            </FooterTemplate>
        </asp:TemplateField>
        </Columns>
</asp:gridview>
            </div>
        </div>
    </div>
   
</asp:Content>
