<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="PurchaseOrderManagement.aspx.cs" Inherits="admin_PurchaseOrderManagement_PurchaseOrderManagement" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div class="form_pad" style="padding-left: 180px;">
        <div class="spacer10">
        </div>
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" Style="padding-left: 35px;"></asp:Label>
        <div class="spacer10">
        </div>
        <div id="dvDocument" runat="server">
            <asp:HyperLink ID="lnkAddPostPurchaseOrder" runat="server" title="Post Purchase Order" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Post_Purchase_Order.png" /> 
                <span>
                   Post Purchase Order
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkSearchPurchaseOrders" runat="server" title="Search Purchase Orders" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Search_Purchase_Orders.png" /> 
                <span>
                   Search Purchase Orders  
                </span>
            </asp:HyperLink>
        </div>
        <div>
           <asp:HyperLink ID="lnkTodaysFollowUp" runat="server" title="Today’s Follow Up" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Todays_FollowUp.png" /> 
                <span>
                   Today’s Follow Up  
                </span>
            </asp:HyperLink>
        </div>
    </div>
</asp:Content>

