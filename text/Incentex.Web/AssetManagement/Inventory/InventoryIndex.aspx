<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="InventoryIndex.aspx.cs" Inherits="AssetManagement_Inventory_InventoryIndex" Title="Inventory Index" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div class="form_pad">
        <div class="btn_width worldlink_btn">
                 <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td style="width:10%"></td>
                <td style="width:50%">
                                
                 <asp:LinkButton ID="lnkAddEmployee" class="gredient_btnMainPage" title="Add Inventory"
                            runat="server" PostBackUrl="~/AssetManagement/Inventory/BasicInfo.aspx?Id=0" >
                <img src="../../admin/Incentex_Used_Icons/addinventory.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Add Inventory            
                </span>
                        </asp:LinkButton> 
                
                </td>
               
                <td style="width:40%">
                      <asp:LinkButton ID="lnkListEmployee" class="gredient_btnMainPage" title="Add Inventory"
                            runat="server" PostBackUrl="~/AssetManagement/Inventory/SearchInventory.aspx" >
                <img src="../../admin/Incentex_Used_Icons/searchinventory.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Search Inventory            
                </span>
                        </asp:LinkButton> 
                </td>
                </tr>
                </table>        
               
                        
           
        </div>
    </div>
     
</asp:Content>


