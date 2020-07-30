<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HeadsetRepairCenter.aspx.cs" Inherits="HeadsetRepairCenter_HeadsetRepairCenter" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="form_pad" style="padding-left: 180px;">
        <div class="spacer10">
        </div>
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" Style="padding-left: 35px;"></asp:Label>
        <div class="spacer10">
        </div>
        <div id="dvDocument" runat="server">
            <asp:HyperLink ID="lnkCreateHeadsetRepairCenter" runat="server" title="Create Headset Repair" class="gredient_btnMainPage">
                <img src="../admin/Incentex_Used_Icons/Incentex-Addheadsetrepair.png" /> 
                <span>
                   Create Headset Repair
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkSearchHeadsetRepairCenter" runat="server" title="Search Headset Repair" class="gredient_btnMainPage">
                <img src="../admin/Incentex_Used_Icons/Incentex-Searchheadsetrepair.png" /> 
                <span>
                   Search Headset Repair  
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkAddHeadsetRepairVendor" runat="server" Visible="false" title="Add Headset Repair Vendor" class="gredient_btnMainPage">
                <img src="../admin/Incentex_Used_Icons/Incentex-Addheadsetrepair.png" /> 
                <span>
                   Add Headset Repair Vendor  
                </span>
            </asp:HyperLink>
        </div>
    </div>
</asp:Content>

