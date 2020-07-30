<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="EmployeeMgtIndex.aspx.cs" Inherits="AssetManagement_EmployeeMgtIndex" Title="Employee Management Index" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div class="form_pad">
        <div class="btn_width worldlink_btn">
                 <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td style="width:10%"></td>
                <td style="width:50%">
                 <asp:LinkButton ID="lnkAddEmployee" runat="server" class="gredient_btn" PostBackUrl="~/AssetManagement/AddEmployee.aspx">
                     <span><strong>Add Employee</strong></span></asp:LinkButton>
                </td>
               
                <td style="width:40%">
                  <asp:LinkButton ID="lnkListEmployee" runat="server" class="gredient_btn" PostBackUrl="~/AssetManagement/EmployeeList.aspx">
                     <span><strong>List Employee</strong></span></asp:LinkButton>    
                </td>
                </tr>
                </table>        
               
                        
           
        </div>
    </div>
     
</asp:Content>

