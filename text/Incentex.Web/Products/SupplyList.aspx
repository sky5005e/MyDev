<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SupplyList.aspx.cs" Inherits="Products_SupplyList" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    </style>
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="front_page_pad">
        <div class="splash_img_pad">
            <asp:Label ID="lblMsg" runat="server" Visible="false" Text="No Records Found" CssClass="errormessage"></asp:Label>
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
                                    <img id="imgProduct" runat="server" src="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                        style="border-width: 8px; width: 145px; height: 198px;" />
                                </a>
                                <asp:HiddenField ID="hndStoreProductImageId" runat="server" Value='<%#Eval("StoreProductID")%>' />
                                <asp:HiddenField ID="hdnMasteItem" runat="server" Value='<%#Eval("MasterItemNo")%>' />
                            </div>
                        </div>
                    </div>
                    <%if (!IncentexGlobal.IsIEFromStore)
                      {%>
                    <div class="uniform_price clearfix">
                        <span class="bl_co"></span><span class="br_co"></span>
                        <table class="product_detail">
                            <tr>
                                <td colspan="2" valign="top">
                                    <div id="dvSummary">
                                        <asp:Label Style="line-height: 12px; position: relative; width: 100%; height: auto;"
                                            ID="lblSummary" runat="server"></asp:Label>
                                    </div>
                                    <div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%} %>
                    <div class="spacer10">
                    </div>
                </ItemTemplate>
                <ItemStyle Width="195" />
            </asp:DataList>
            <!-- Product List End -->
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
