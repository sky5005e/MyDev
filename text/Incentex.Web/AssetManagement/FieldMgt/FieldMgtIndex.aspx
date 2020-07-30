<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="FieldMgtIndex.aspx.cs" Inherits="AssetManagement_FieldMgt_FieldMgtIndex" Title="Field Management Index" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div class="form_pad">
        <div class="btn_width worldlink_btn">
                 <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td style="width:10%"></td>
                <td style="width:50%">
                      <asp:LinkButton ID="lnkAddField" class="gredient_btnMainPage" title="Add Field"
                            runat="server" PostBackUrl="~/AssetManagement/FieldMgt/AddField.aspx" >
                <img src="../../admin/Incentex_Used_Icons/addfiled_icon.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Add Field            
                </span>
                        </asp:LinkButton> 
                </td>
               
                <td style="width:40%">    
                      <asp:LinkButton ID="lnkManageField" class="gredient_btnMainPage" title="Manage Field"
                            runat="server" PostBackUrl="~/AssetManagement/FieldMgt/SearchField.aspx" >
                <img src="../../admin/Incentex_Used_Icons/Managefield_icon.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Manage Field            
                </span>
                        </asp:LinkButton> 
                </td>
                </tr>
                </table>        
               
                        
           
        </div>
    </div>
     
</asp:Content>

