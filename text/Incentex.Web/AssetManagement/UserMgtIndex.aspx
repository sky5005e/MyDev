<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="UserMgtIndex.aspx.cs" Inherits="AssetManagement_UserMgtIndex" Title="User Management Index" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div class="form_pad">
        <div class="btn_width worldlink_btn">
                 <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td style="width:35%"></td>
                <td style="width:65%">
                 <asp:LinkButton ID="lnkVendorManagement" runat="server" class="gredient_btn" PostBackUrl="~/AssetManagement/VendorMgtIndex.aspx">
                     <span><strong>Vendor Management</strong></span></asp:LinkButton>
                </td>
               <%--
                <td style="width:40%">
                  <asp:LinkButton ID="lnkEmployeeManagement" runat="server" class="gredient_btn" PostBackUrl="~/AssetManagement/EmployeeMgtIndex.aspx">
                     <span><strong>Employee Management</strong></span></asp:LinkButton>    
                </td>--%>
                </tr>
                </table>  
           
        </div>
    </div>
     
</asp:Content>

