<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProductList.aspx.cs" Inherits="Products_ProductList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="front_page_pad">
        <div class="splash_img_pad">
            <div class="centeralign">
                <asp:Label ID="lblMsg" runat="server" Visible="false" Text="No Records Found" CssClass="errormessage"></asp:Label>
            </div>
            <!-- Product List -->
            <asp:DataList ID="lstProductList" runat="server" OnDataBinding="lstProductList_DataBinding"
                RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="5" CellPadding="0"
                CellSpacing="0" OnItemDataBound="lstProductList_ItemDataBound" Style="width: auto;">
                <ItemTemplate>
                    <div class="clearfix">
                        <div class="agent_img alignleft">
                            <span class="tl_co"></span><span class="tr_co"></span>
                            <div id="dvPriPhotoContainer " class="upload_photo alignright gallery">
                                <a href="~/UploadedImages/ProductImages/ProductDefault.jpg" runat="server" id="lnkImage">
                                    <img alt="Product Image" id="imgProduct" runat="server" src="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                        style="border-width: 8px; width: 145px; height: 198px;" />
                                </a>
                                <asp:HiddenField ID="hndStoreProductImageId" runat="server" Value='<%# Eval("StoreProductID")%>' />
                                <asp:HiddenField ID="hdnMasteItem" runat="server" Value='<%# Eval("MasterItemNo")%>' />
                                <asp:HiddenField ID="hdnPSubCateID" runat="server" Value='<%# Eval("PSubCateID") %>' />
                            </div>
                        </div>
                    </div>
                    <div class="uniform_price clearfix">
                        <span class="bl_co"></span><span class="br_co"></span>
                        <table class="product_detail">
                            <tr>
                                <td colspan="2" valign="top">
                                    <div id="dvSummary">
                                        <asp:Label Style="line-height: 12px; position: relative; width: 100%; height: auto;"
                                            ID="lblSummary" runat="server" Text='<%# Eval("Summary") %>'></asp:Label>
                                    </div>
                                    <div>
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
