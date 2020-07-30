<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="VendorMgtIndex.aspx.cs" Inherits="AssetManagement_VendorMgtIndex" Title="Vendor Mgt Index" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div class="form_pad">
        <div class="btn_width worldlink_btn">
                 <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td style="width:10%"></td>
                <td style="width:50%">
                 <asp:LinkButton ID="lnkAddVendor" runat="server" class="gredient_btn" PostBackUrl="~/AssetManagement/VendorManagement.aspx"
                    Text="Job Code">
                     <span><strong>Add Vendor</strong></span></asp:LinkButton>
                </td>
               
                <td style="width:40%">
                  <asp:LinkButton ID="lnkListVendor" runat="server" class="gredient_btn" Text="JobSubCode"
                    PostBackUrl="~/AssetManagement/VendorList.aspx">
                     <span><strong>List Vendor</strong></span></asp:LinkButton>    
                </td>
                </tr>
                </table>        
               
                        
           
        </div>
    </div>
     
</asp:Content>

