<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ArtworkIndex.aspx.cs" Inherits="admin_ArtWorkDecoratingLibrary_ArtworkIndex" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad" style="padding-left: 180px;">
        <div class="spacer10">
        </div>
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" Style="padding-left: 35px;"></asp:Label>
        <div class="spacer10">
        </div>
        <div id="dvArtWork" runat="server">
            <asp:HyperLink ID="lnkManageDecoratingPartners" runat="server" title="Manage Decorating Partners"
                class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/World-Link System Controls.png" alt="Manage Decorating Partners" />
                <span>
                   Manage Decorating Partners
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkAddArtwork" runat="server" title="Add Artwork" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/add_artwork.png" alt="Add Artwork" />
                <span>
                   Add Artwork
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkSearchArtwork" runat="server" title="Search Artwork" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/search_artwork.png" alt="Search Artwork" />
                <span>
                   Search Artwork
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkAddDecoratingSpecifications" runat="server" title="Add Decorating Specifications"
                class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/World-Link System Controls.png" alt="Add Decorating Specifications" />
                <span>
                   Add Decorating Specifications
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkSearchDecoratedItems" runat="server" title="Search Decorated Items"
                class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/World-Link System Controls.png" alt="Search Decorated Items" />
                <span>
                  Search Decorated Items
                </span>
            </asp:HyperLink>
        </div>
    </div>
</asp:Content>
