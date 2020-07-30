<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="RepairManagementIndex.aspx.cs" Inherits="AssetManagement_RepairManagement_RepairManagementIndex" %>

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
              <asp:HyperLink ID="lnkCreateRepairOrder"  runat="server" title="Create Repair Order" class="gredient_btnMainPage">
                <img src="../../admin/Incentex_Used_Icons/addinventory.png" alt="Create Repair Order" />
                <span>
                  Create Repair Order
                </span>
            </asp:HyperLink>
            </td>
            <td>
             <asp:HyperLink ID="lnkViewOpenRepairOrders" title="View Open Repair Orders" runat="server" class="gredient_btnMainPage">
                <img src="../../admin/Incentex_Used_Icons/Managefield_icon.png" alt="View Open Repair Orders" />
                <span>
                    View Open Repair Orders
                </span>
            </asp:HyperLink>
            </td>
            <td>
            <asp:HyperLink ID="lnkSearchPastRepairOrders" title="Search Past Repair Orders" runat="server" class="gredient_btnMainPage">
                <img src="../../admin/Incentex_Used_Icons/SearchEquipment.png" alt="Search Past Repair Orders" />
                <span>
                    Search Past Repair Orders
                </span>
            </asp:HyperLink>
            </td>
            </tr>  
            <tr>
           <td>
              <asp:HyperLink ID="lnkViewRepairOrderBaseStation"  runat="server" title="View Repair Orders by BaseStation" class="gredient_btnMainPage">
                <img src="../../admin/Incentex_Used_Icons/jobsubcode_icon.png" alt="Create Repair Order" />
                <span>
                  Repair Orders by BaseStation
                </span>
            </asp:HyperLink>
            </td>
            <td></td><td></td>
            </tr> 
                      
            </table>
             
             
             </div>    
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

