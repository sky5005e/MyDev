<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AssetMgtIndex.aspx.cs" Inherits="AssetManagement_AssetMgtIndex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div class="form_pad">
        <div >
            <div>
            <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
            <td>
                    
             <asp:LinkButton ID="lnkManageEquipment" class="gredient_btnMainPage" title="Manage Equipment"
                            runat="server" onclick="lnkManageEquipment_Click" >
                <img src="../admin/Incentex_Used_Icons/AddNewEquipment.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                   Add GSE Equipment             
                </span>
                        </asp:LinkButton>
            </td>
            <td>
             <asp:LinkButton ID="lnkSearchEquipment" class="gredient_btnMainPage" title="Search Equipment"
                            runat="server" onclick="lnkSearchEquipment_Click" >
                <img src="../admin/Incentex_Used_Icons/SearchEquipment.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                   Search Equipment             
                </span>
                        </asp:LinkButton>
            </td>
            <td>
           
             <asp:LinkButton ID="lnkFlaggedAssets" class="gredient_btnMainPage" title="Flagged Assets"
                            runat="server" onclick="lnkFlaggedAssets_Click" >
                <img src="../admin/Incentex_Used_Icons/FlagAsset.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                   Flagged Assets             
                </span>
                        </asp:LinkButton>
            </td>
            </tr>
             <tr>
            <td>
              <asp:LinkButton ID="lnkUserManagement" class="gredient_btnMainPage" title="User Management"
                            runat="server" onclick="lnkUserManagement_Click" >
                <img src="../admin/Incentex_Used_Icons/VendorManagement.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  User Management            
                </span>
                        </asp:LinkButton>
           
            </td>
            <td>
              <asp:LinkButton ID="lnkPlanningReports" class="gredient_btnMainPage" title="Planning Reports"
                            runat="server" onclick="lnkPlanningReports_Click" >
                <img src="../admin/Incentex_Used_Icons/PlanningReports.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Planning Reports            
                </span>
                        </asp:LinkButton>
         
            </td>
            <td>
              <asp:LinkButton ID="LnkDropDownMenu" class="gredient_btnMainPage" title="Drop Down Menu"
                            runat="server" onclick="LnkDropDownMenu_Click" >
                <img src="../admin/Incentex_Used_Icons/Incentex-dropdownmenu.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Drop Down Menu            
                </span>
                        </asp:LinkButton>
         
            </td>
            </tr>
            
            <tr>
            <td>
              <asp:LinkButton ID="lnkPendingInvoices" class="gredient_btnMainPage" title="Pending Invoices"
                            runat="server" onclick="lnkPendingInvoices_Click" >
                <img src="../admin/Incentex_Used_Icons/Incentex-artwork.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Pending Invoices            
                </span>
                        </asp:LinkButton>
           
            </td>
            <td>
              <asp:LinkButton ID="lnkInventory" class="gredient_btnMainPage" title="Inventory"
                            runat="server" onclick="lnkInventory_Click" >
                <img src="../admin/Incentex_Used_Icons/Incentex-purcasingservices.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Station Inventory            
                </span>
                        </asp:LinkButton>
           
            </td>
            <td>
              <asp:LinkButton ID="lnkFieldManagement" class="gredient_btnMainPage" title="Field Management"
                            runat="server" onclick="lnkFieldManagement_Click" >
                <img src="../admin/Incentex_Used_Icons/Incentex-resetorderaccess.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Field Management            
                </span>
                        </asp:LinkButton>
           
            </td>
            </tr>
              <tr>
            <td>
              <asp:LinkButton ID="lnkRepairOrderManagement" class="gredient_btnMainPage" title="Repair Order Management"
                            runat="server" onclick="lnkRepairOrderManagement_Click" >
                <img src="../admin/Incentex_Used_Icons/World-Link System Controls.png" alt="Repair Order Management" />
                <span style="width:200px;">               
                Repair Order Management  
                </span>
                        </asp:LinkButton>
           
            </td>
             <td>
              <asp:LinkButton ID="lnkBlogCenter" class="gredient_btnMainPage" title="Blog Center"
                            runat="server"  onclick="lnkBlogCenter_Click">                            
                <img src="../admin/Incentex_Used_Icons/blog-center.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Blog Center           
                </span>
                        </asp:LinkButton>
           
            </td>
            <td>
             
           
            </td>
            </tr>
            </table>
             
             
             </div>    
        </div>
    </div>
    
     
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

