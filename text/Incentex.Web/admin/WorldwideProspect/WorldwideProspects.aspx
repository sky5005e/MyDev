<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="WorldwideProspects.aspx.cs" Inherits="admin_WorldwideProspect_WorldwideProspects" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad" style="padding-left: 180px;">
        <div class="spacer10">
        </div>
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" Style="padding-left: 35px;"></asp:Label>
        <div class="spacer10">
        </div>
        <div id="dvWorldwideProspects" runat="server">
            <asp:HyperLink ID="lnkAddProspect" runat="server" title="Add Prospect"
                class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/AddProspects.png" /> 
                <span>
                   Add New Prospect 
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkSearchProspects" runat="server" title="Search Prospects"
                class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/SearchProspects.png" /> 
                <span>
                   Search Prospects  
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkCreateCampaign" runat="server" title="Create Campaign"
                class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/create_campaign.png" /> 
                <span>
                   Create Campaign
                </span>
            </asp:HyperLink>
        </div>
    </div>
</asp:Content>
