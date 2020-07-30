<%@ Page Title="Vendor Access Inc_Order_Management" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="VendorAccessIncOrderManegement.aspx.cs" Inherits="admin_PurchaseOrderManagement_VendorAccessIncOrderManegement" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div class="form_pad" style="padding-left: 180px;">
        <div class="spacer10">
        </div>
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" Style="padding-left: 35px;"></asp:Label>
        <div class="spacer10">
        </div>
        <div id="dvDocument" runat="server">
            <asp:HyperLink ID="lnkTodaysProduction" runat="server" title="Today’s Production" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Todays_FollowUp.png" /> 
                <span>
                   Today’s Production
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkCurrentWeeksProduction" runat="server" title="Current Weeks Production" class="gredient_btnMainPage">
               <img src="../Incentex_Used_Icons/Todays_FollowUp.png" /> 
                <span>
                   Current Weeks Production
                </span>
            </asp:HyperLink>
        </div>
        <div>
           <asp:HyperLink ID="lnkSearchOpenOrdered" runat="server" title="Search Open Ordered" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Search_Purchase_Orders.png" />
                
                <span>
                   Search Open Ordered
                </span>
            </asp:HyperLink>
        </div>
    </div>
</asp:Content>


