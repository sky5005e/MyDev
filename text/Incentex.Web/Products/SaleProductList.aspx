<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SaleProductList.aspx.cs" Inherits="Products_SaleProductList" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="front_page_pad">
        <div class="splash_img_pad">
            <asp:Label ID="lblMsg" runat="server" Visible="false" Text="No Records Found" CssClass="errormessage"></asp:Label>
            <!-- Product List -->
            <asp:DataList ID="lstProductList" runat="server" OnDataBinding="lstProductList_DataBinding"
                RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="5" CellPadding="0"
                CellSpacing="0" OnItemDataBound="lstProductList_ItemDataBound" OnItemCommand="lstProductList_ItemCommand"
                Style="width: auto;">
                <ItemTemplate>
                    <div class="clearfix">
                        <div class="agent_img alignleft">
                            <span class="tl_co"></span><span class="tr_co"></span>
                            <div id="dvPriPhotoContainer " class="upload_photo alignright gallery">
                                <a rel='prettyPhoto[p]' href="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                    runat="server" id="lnkImage">
                                    <img id="imgProduct" runat="server" src="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                        style="border-width: 8px;" />
                                </a>
                            </div>
                        </div>
                    </div>
                    <div class="uniform_price clearfix">
                        <span class="bl_co"></span><span class="br_co"></span>
                        <table>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <asp:LinkButton ID="lnkDetail" CommandName="Detail" runat="server" CssClass="grey2_btn"
                                            CommandArgument='<%#Eval("MasterItemNo")%>'><span>Details</span></asp:LinkButton>
                                        <asp:HiddenField ID="hndStoreProductImageId" runat="server" Value='<%#Eval("StoreProductID")%>' />
                                    </div>
                                </td>
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
    </div>
</asp:Content>
