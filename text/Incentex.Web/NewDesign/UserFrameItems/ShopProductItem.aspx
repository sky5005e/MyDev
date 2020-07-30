<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopProductItem.aspx.cs" Inherits="UserFrameItems_ShopProductItem" %>


<%@ Register TagPrefix="uc" TagName="CommonHeader" Src="~/NewDesign/UserControl/NewCommonHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <uc:CommonHeader ID="ucCommonHead" runat="server" />
    <title>Incentex</title>
    
</head>
<body class="NoClass">
    <form id="form2" runat="server">
      <div id="dvUpdateContent" runat="server" class="shop-content cf">
                        <div class="shop-imageblock alignleft">
                            <a href="javascript: void(0);">
                                <asp:Image id="imgMasterPopImage" runat="server" width="427" height="570" alt="product"></asp:Image></a>
                        </div>
                        <div class="shop-rightblock">
                            <div class="shop-title-block cf">
                                <asp:Label ID="lblMasterPopupsummary" CssClass="shop-title-top" runat="server"></asp:Label><em
                                    class="shop-new">NEW</em><strong class="shop-price">$<asp:Label ID="lblMasterPopupAmount"
                                        runat="server"></asp:Label></strong></div>
                            <p class="shop-dis">
                                <asp:Label ID="lblMasterProductDescription" runat="server"></asp:Label></p>
                            <ul class="shop-block cf">
                                <li><span class="shop-lbl">Color</span><em class="shop-color"><asp:Label ID="lblMasterPopupColor"
                                    runat="server"></asp:Label>
                                </em></li>
                                <li><span class="shop-lbl">Size</span><div class="table-drop cart-pop smalldrop">
                                    <asp:DropDownList ID="ddlMasterPopupSize" runat="server" class="default" OnSelectedIndexChanged="ddlMasterPopupSize_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                    <div class="shop-links">
                                        <asp:LinkButton ID="lnkMasterPopupSizeChart" runat="server" target="_blank" Style="display: none;">Size Chart</asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="lnkMasterPopupMeasurement" runat="server" target="_blank" Style="display: none;">How to Measure</asp:LinkButton></div>
                                </li>
                                <li><span class="shop-lbl">Order By</span><div class="table-drop cart-piece smalldrop">
                                    <asp:DropDownList ID="ddlMasterPopupSoldBy" runat="server" class="default" OnSelectedIndexChanged="ddlMasterPopupSoldBy_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                    <span class="shop-lbl1">Order Quantity</span><label class="order-input">
                                        <asp:TextBox ID="txtMasterPopupQty" runat="server" class="input-field-small" value="QTY"
                                            onfocus="if (this.value == 'QTY') {this.value=''}" onblur="if(this.value == '') { this.value='QTY'}"
                                            onchange="CheckNum(this.id)"></asp:TextBox>
                                    </label>
                                </li>
                            </ul>
                            <div class="bottom-btn-block">
                                <asp:LinkButton class="blue-btn"  ID="lnkSaveCartItem" runat="server" OnClick="lnkSaveCartItem_Click">
                                     <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                                  <span class="cart-ico"><img id="img" src="<%=System.Configuration.ConfigurationManager.AppSettings["NewDesignSiteurl"]%>StaticContents/img/cart-ico-w.png"  alt="cart" width="16" height="16" /></span>
                                  <strong><asp:Label ID="lblbtnText" runat="server"></asp:Label></strong>
                                    <span class="btn-label"><em>$<asp:Label ID="lblbtnMasterPopupAmount" runat="server"></asp:Label></em></span>
                                    </asp:PlaceHolder>
                                </asp:LinkButton>
                            </div>
                            <div class="shop-bot-links">
                                <div>
                                    <asp:LinkButton ID="lnkMastervideoDisplay" runat="server" title="Product Video"
                                        style="display: none;" OnClick="lnkMastervideoDisplay_Click" >Product Video</asp:LinkButton> &nbsp; <a id="lnkMastercertification" runat="server"
                                            href="javascript:;" target="_blank" style="display: none;">Specifications</a></div>
                                <p>
                                    ITEM #<asp:Label ID="lblMasterPopupItemNumber" runat="server"></asp:Label>
                                    | INVENTORY:
                                    <asp:Label ID="lblMasterPopupInventory" runat="server"></asp:Label>
                                    PCS</p>
                            </div>
                        </div>
                    </div>
                    <div ID="dvVideoContent" runat="server">
                  <div style="text-align : center;">
                    <asp:LinkButton ID="lnkBackToProduct" class="lnkBackToProduct" runat="server" title="Product Video"
                                         OnClick="lnkBackToProduct_Click">Back To Product</asp:LinkButton></div>
                     <iframe id="iframeMasterVideo" runat="server" width="930px" height="590px" frameborder="0">
                        </iframe>
                        </div>
    </form>
</body>
</html>
