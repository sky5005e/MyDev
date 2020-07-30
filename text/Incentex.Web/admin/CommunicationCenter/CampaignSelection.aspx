<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="CampaignSelection.aspx.cs" Inherits="admin_CommunicationCenter_CampaignSelection" Title="Communications Center" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="form_pad" style="padding-left: 180px;">
        <div>
            <asp:HyperLink ID="lnkCreateCampaign"  runat="server" title="Create Campaign" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/create_campaign.png" alt="Create Campaign" />
                <span>
                   Create Campaign
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkViewCampaign" title="View Campaign" runat="server" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/view_campaign.png" alt="View Campaign" />
                <span>
                    View Campaign
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkPendingMOASOrders" title="Pending MOAS Orders" runat="server" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/pending_moas_orders.png" alt="Pending MOAS Orders" />
                <span>
                    Pending MOAS Orders
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkPendingShoppingCart" title="Pending Shopping cart Orders" runat="server" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/pending_cart_icn.png" alt="Pending Shopping cart Orders" />
                <span>
                    Pending Shopping Carts
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkPendingUsers" title="Pending Users" runat="server" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/pending_users.png" alt="Pending Users" />
                <span>
                    Pending Users
                </span>
            </asp:HyperLink>
            
             <asp:HyperLink ID="lnkTodayEmails" title="Todays Email" runat="server" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/today-sent-icon.png" alt="Todays Emails" />
                <span>
                   Todays Sent Emails
                </span>
            </asp:HyperLink>
             <asp:HyperLink ID="lnkSystemEmailTemplates" title="System Email Templates" runat="server" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/system-email-templates.png" alt="System Email Templates" />
                <span>
                   System Email Templates
                </span>
            </asp:HyperLink>
             <asp:HyperLink ID="lnkViewReports" title="View Reports" runat="server" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Incentex-executiveplanning.png" alt="View Reports" />
                <span>
                   View Reports
                </span>
            </asp:HyperLink>
             <asp:HyperLink ID="lnkOrdersPlacedReports" title="Order Placed Reports" runat="server" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/Incentex-executiveplanning.png" alt="Order Placed Reports" />
                <span>
                   Order Placed Reports
                </span>
            </asp:HyperLink>
             <asp:HyperLink ID="lnkBouncedDetails" title="Bounced Details" runat="server" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/undeliveredemail-icon.png" alt="Bounced Details" />
                <span>
                   Bounced Emails
                </span>
            </asp:HyperLink>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

