<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ListArts.aspx.cs" Inherits="admin_Artwork_ListArts"  %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad" style="padding-left: 180px;">
     <div class="spacer10">
            </div>
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" style="padding-left: 35px;"></asp:Label>
            <div class="spacer10">
            </div>
        <div id="dvArtWork" runat="server">
            <asp:HyperLink ID="lnkAddArtwork"  runat="server" title="Add Artwork" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/add_artwork.png" alt="Add Artwork" />
                <span>
                   Add Artwork
                </span>
            </asp:HyperLink>
             <asp:HyperLink ID="lnkSearchArtwork"  runat="server" title="Search Artwork" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/search_artwork.png" alt="Search Artwork" />
                <span>
                   Search Artwork
                </span>
            </asp:HyperLink>
            </div>
            <div id="dvImage" runat="server">
            <asp:HyperLink ID="lnkAddImage"  runat="server" title="Add Image" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/add_artwork.png" alt="Add Image" />
                <span>
                   Add Image
                </span>
            </asp:HyperLink>
             <asp:HyperLink ID="lnkSearchImage"  runat="server" title="Search Image" class="gredient_btnMainPage">
                <img src="../Incentex_Used_Icons/search_artwork.png" alt="Search Image" />
                <span>
                   Search Image
                </span>
            </asp:HyperLink>
            </div>
    </div>
</asp:Content>
