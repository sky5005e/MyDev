<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewSearchResults.aspx.cs" Inherits="admin_Artwork_ViewSearchResults" %>

<%@ Import Namespace="Incentex.DAL.SqlRepository" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function pass(id) {

            if (id != '0')
                window.location = "ViewLargerImage.aspx?id=" + id;
        }
        function passProductId(id,imgPath) {
            if (id != '0')
                window.location = "ViewLargerImage.aspx?ProductId=" + id;
        }
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete this art ?") == true)
                return true;
            else
                return false;
        }
    </script>

    <div class="form_pad">
        <asp:Label ID="lblMsg" runat="server" Visible="false" Text="No Records Found" CssClass="errormessage"></asp:Label>
        <!-- Product List -->
        <asp:DataList ID="lstImageList" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
            RepeatColumns="5" CellPadding="0" CellSpacing="0" OnItemDataBound="lstImageList_ItemDataBound"
            OnItemCommand="lstImageList_ItemCommand">
            <ItemTemplate>
                <div class="clearfix">
                    <div class="agent_img alignleft">
                        <span class="tl_co"></span><span class="tr_co"></span>
                        <div id="dvPriPhotoContainer " class="upload_photo alignright gallery">
                            <img id="imgProduct" onclick="pass(this.alt);" runat="server" src="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                style="border-width: 5px; height: 198px; width: 140px" />
                        </div>
                    </div>
                </div>
                <div class="uniform_price clearfix" style="width: 125px;">
                    <span class="bl_co"></span><span class="br_co"></span>
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkDownload" runat="server" Text="" CommandName="download" class="btn_space">
                                    <span class="btn_space">
                                        <img id="img2" src="~/Images/download_btn.png" style="height: 20px; width: 20px"
                                            runat="server" alt='Loading' /></span></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkemail" runat="server" Text="" CommandName="email" class="btn_space"
                                    CommandArgument='<%# Eval("ArtWorkId") %>'>
                                    <span class="btn_space">
                                        <img id="img1" src="~/Images/shipment06.png" style="height: 20px; width: 20px" runat="server"
                                            alt='Loading' /></span></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lblDelete" OnClientClick="return DeleteConfirmation();" runat="server"
                                    Text="" CommandName="del" CommandArgument='<%# Eval("ArtWorkId") %>' class="btn_space">
                                    <span class="btn_space">
                                        <img id="imgdelete" src="~/Images/close.png" style="height: 20px; width: 20px"
                                            runat="server" alt='Loading' /></span></asp:LinkButton>
                            </td>
                            <asp:HiddenField ID="hfThumb" runat="server" Value='<%#Eval("ThumbImageSName")%>' />
                            <asp:HiddenField ID="hfLarge" runat="server" Value='<%#Eval("LargerImageSName")%>' />
                            <asp:HiddenField ID="hfThumbO" runat="server" Value='<%#Eval("ThumbImageOName")%>' />
                            <asp:HiddenField ID="hfLargeO" runat="server" Value='<%#Eval("LargerImageOName")%>' />
                        </tr>
                    </table>
                </div>
                <div class="spacer10">
                </div>
            </ItemTemplate>
            <ItemStyle Width="195" />
        </asp:DataList>
        <!-- Product List End -->
        
        <!-- Image form StoreProduct List -->
        <asp:DataList ID="dtStoreProdcutImageList" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
            RepeatColumns="5" CellPadding="0" CellSpacing="0" OnItemDataBound="dtStoreProdcutImageList_ItemDataBound"
            OnItemCommand="dtStoreProdcutImageList_ItemCommand">
            <ItemTemplate>
                <div class="clearfix">
                    <div class="agent_img alignleft">
                        <span class="tl_co"></span><span class="tr_co"></span>
                        <div id="dvStorePhotoContainer " class="upload_photo alignright gallery">
                            <img id="imgStoreProduct" onclick="passProductId(this.alt);" runat="server" src="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                style="border-width: 5px; height: 198px; width: 140px" />
                        </div>
                    </div>
                </div>
                <div class="uniform_price clearfix" style="width: 125px;">
                    <span class="bl_co"></span><span class="br_co"></span>
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkStoreDownload" runat="server" Text="" CommandName="download" class="btn_space">
                                    <span class="btn_space">
                                        <img id="imgStore2" src="~/Images/download_btn.png" style="height: 20px; width: 20px"
                                            runat="server" alt='Loading' /></span></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkemail" runat="server" Text="" CommandName="email" class="btn_space"
                                    CommandArgument='<%# Eval("StoreProductImageId") %>'>
                                    <span class="btn_space">
                                        <img id="img1" src="~/Images/e-mail_btn.png" style="height: 20px; width: 20px" runat="server"
                                            alt='Loading' /></span></asp:LinkButton>
                            </td>
                             <asp:HiddenField ID="hdnProductImage" runat="server" Value='<%#Eval("ProductImage")%>' />
                             <asp:HiddenField ID="hdnProductImageLarge" runat="server" Value='<%#Eval("LargerProductImage")%>' />
                             <asp:HiddenField ID="hdnStoreProductImageId" runat="server" Value='<%#Eval("StoreProductImageId")%>' />
                        </tr>
                    </table>
                </div>
                <div class="spacer10">
                </div>
            </ItemTemplate>
            <ItemStyle Width="195" />
        </asp:DataList>
        <!-- Product List End -->
    </div>
</asp:Content>
