<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="IssuancePackage.aspx.cs" Inherits="UserPages_IssuancePackage" Title="incentex | Issuance Package" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aCTK" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function DisplaySubCategoryRpt(item) {
            $(item).closest('#accordion').find('div').hide();
            $(item).closest('#accordion').find('li').removeClass('active');
            $(item).next().toggle();
            $(item).parent().addClass("active");
        }
    </script>

    <%--<script type="text/javascript">
        jQuery(document).ready(function() {
            jQuery('.mycarousel').jcarousel({ 
                scroll: 1
            });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".cart-slider-display .image-block").hoverIntent(function(){
                $(".cart-caption", this).animate({ bottom: 0}, { duration: 400});
            },function(){
                $(".cart-caption", this).animate({ bottom: -68}, { duration: 400});
            });
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <aCTK:ToolkitScriptManager ID="tksmCheckout" runat="server">
    </aCTK:ToolkitScriptManager>
    <input type="hidden" value="shop-link" id="hdnActiveLink" />
    <% if (!Request.IsLocal)
       { %>
    <section id="container" class="cf">
    <% }
       else
       { %>
    <div id="container" class="cf store-page">
        <%} %>
        <div class="narrowcolumn alignleft">
            <div class="store-left-block">
                <div class="subNav-block">
                    <h2 class="title-txt">
                        Products</h2>
                    <asp:Repeater ID="rptUniformAccess" runat="server" OnItemDataBound="rptUniformAccess_ItemDataBound">
                        <HeaderTemplate>
                            <ul id="accordion" class="cartNav cf">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li id="liCategory" class="selected"><a href="javascript:;" class="arrow" id="lnkCategory"
                                runat="server" title='<%# Eval("CategoryName") %>' onclick="DisplaySubCategoryRpt(this);">
                                <span></span>
                                <%# Eval("CategoryName") %></a>
                                <div id="dvSubCategory" runat="server" style="display: none;">
                                    <ul class="cf">
                                        <asp:Repeater ID="rptSubItemsUniformAccess" runat="server">
                                            <ItemTemplate>
                                                <li>
                                                    <asp:HyperLink ID="lnkSubCategory" runat="server" NavigateUrl='<%# System.Configuration.ConfigurationManager.AppSettings["siteurl"] + "UserPages/Index.aspx?SubCatID=" + Convert.ToString(Eval("SubCategoryID")) %>'
                                                        Text='<%# Eval("SubCategoryName") %>'>
                                                    </asp:HyperLink>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div class="subNav-block">
                    <h2 class="title-txt">
                        Uniform Issuances</h2>
                    <ul class="cf uniform-links">
                        <asp:Repeater ID="repIssuancePolicies" runat="server" OnItemDataBound="repIssuancePolicies_ItemDataBound">
                            <ItemTemplate>
                                <li>
                                    <asp:HyperLink ID="hlIssuancePolicy" runat="server">
                                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"><em>
                                            <asp:Image ID="imgIsOrdered" runat="server" Width="7" Height="12" AlternateText="OS"
                                                ImageUrl="" />
                                        </em>
                                            <asp:Label ID="lblPolicyName" runat="server"></asp:Label>
                                        </asp:PlaceHolder>
                                    </asp:HyperLink>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </div>
        <div class="widecolumn alignright">
            <div class="store-right-block">
                <div class="store-header cf">
                    <span class="store-title">Ordering For: <strong>Jane Doe</strong></span><a href="#"
                        title="change" class="small-btn">change</a><a href="#" class="small-btn" title="History">History</a></div>
                <div class="store-select-bar cf">
                    <div class="select-left alignleft">
                        <span>SHIRTS</span>: Select <span>2 items</span> from the products below.</div>
                    <div class="select-right alignright">
                        <span>1</span> of 2 selected</div>
                </div>
                <div class="cart-slider-display">
                    <ul class="jcarousel-skin-tango cf">
                        <li>
                            <div class="image-block">
                                <a href="#" class="pro-img">
                                    <img src="../../UploadedImages/ProductImages/Large_634584494329223446_incentex-spirit-commercial-photography-1585cc.jpg" width="236" height="315"
                                        title="Dress Shirt" alt="Dress Shirt"></a><div class="cart-caption">
                                            <span>
                                                <img src="../StaticContents/img/quick-view-ico.png" width="32" height="32" alt="cart"></span><span>QUICKVIEW</span></div>
                            </div>
                            <div class="cart-description">
                                <div class="cart-price cf">
                                    <span class="alignleft">Dress Shirt</span><span class="alignright">$50</span></div>
                                <div class="cart-btn-block cf">
                                    <div class="table-drop cart-dropin">
                                        <select name="size" class="default">
                                            <option value="">Size</option>
                                            <option value="">Small</option>
                                            <option value="">Medium</option>
                                            <option value="">Large</option>
                                        </select></div>
                                    <input type="text" class="default_title_text input-qty" placeholder="QTY" title="QTY">
                                    <a href="javascript:;" class="cart-btn"><span>&nbsp;</span>Add</a>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="image-block">
                                <a href="#" class="pro-img">
                                    <img src="../../UploadedImages/ProductImages/Large_634584496535691321_incentex-spirit-commercial-photography-1370cc.jpg" width="236" height="315"
                                        title="Dress Shirt" alt="Dress Shirt"></a><div class="cart-caption">
                                            <span>
                                                <img src="../StaticContents/img/quick-view-ico.png" width="32" height="32" alt="cart"></span><span>QUICKVIEW</span></div>
                            </div>
                            <div class="cart-description">
                                <div class="cart-price cf">
                                    <span class="alignleft">Dress Shirt</span><span class="alignright">$50</span></div>
                                <div class="cart-btn-block cf">
                                    <div class="table-drop cart-dropin">
                                        <select name="size1" class="default">
                                            <option value="">XS</option>
                                            <option value="">Small</option>
                                            <option value="">Medium</option>
                                            <option value="">Large</option>
                                        </select></div>
                                    <input type="text" class="default_title_text input-qty" placeholder="1" title="1">
                                    <a href="javascript:;" class="cart-btn"><span>&nbsp;</span>Add</a>
                                </div>
                            </div>
                        </li>
                        <li class="last">
                            <div class="image-block">
                                <a href="#" class="pro-img">
                                    <img src="../../UploadedImages/ProductImages/Large_634584501210395532_incentex-spirit-commercial-photography-0799cc.jpg" width="236" height="315"
                                        title="Dress Shirt" alt="Dress Shirt"></a><div class="cart-caption">
                                            <span>
                                                <img src="../StaticContents/img/quick-view-ico.png" width="32" height="32" alt="cart"></span><span>QUICKVIEW</span></div>
                            </div>
                            <div class="cart-description">
                                <div class="cart-price cf">
                                    <span class="alignleft">Dress Shirt</span><div class="alignright cart-price-ico">
                                        <div class="cart-paid-block">
                                            This item is being paid by corporate.</div>
                                        <a href="#">
                                            <img src="../StaticContents/img/price-ico.png" width="16" height="18" alt="price"></a></div>
                                </div>
                                <div class="cart-btn-block cf">
                                    <div class="table-drop cart-dropin">
                                        <select name="size2" class="default">
                                            <option value="">Size</option>
                                            <option value="">Small</option>
                                            <option value="">Medium</option>
                                            <option value="">Large</option>
                                        </select></div>
                                    <input type="text" class="default_title_text input-qty" placeholder="QTY" title="QTY">
                                    <a href="javascript:;" class="cart-btn"><span>&nbsp;</span>Add</a>
                                </div>
                            </div>
                        </li>                        
                    </ul>
                </div>
                <div class="store-select-bar cf">
                    <div class="select-left alignleft">
                        <span>ACCESSORIES</span>: Select <span>2 items</span> from the products below.</div>
                    <div class="select-right alignright">
                        <span>0</span> of 2 selected</div>
                </div>
                <div class="cart-slider-display">
                    <ul class="jcarousel-skin-tango mycarousel cf">
                        <li>
                            <div class="image-block">
                                <a href="#" class="pro-img">
                                    <img src="../../UploadedImages/ProductImages/Large_634315005027818750_Spirit Airlines-Belt-Large.jpg" width="236" height="315"
                                        title="Dress Shirt" alt="Dress Shirt"></a><div class="cart-caption">
                                            <span>
                                                <img src="../StaticContents/img/quick-view-ico.png" width="32" height="32" alt="cart"></span><span>QUICKVIEW</span></div>
                            </div>
                            <div class="cart-description">
                                <div class="cart-price cf">
                                    <span class="alignleft">Dress Shirt</span><span class="alignright">$50</span></div>
                                <div class="cart-btn-block cf">
                                    <div class="table-drop cart-dropin">
                                        <select name="size-a" class="default">
                                            <option value="">Size</option>
                                            <option value="">Small</option>
                                            <option value="">Medium</option>
                                            <option value="">Large</option>
                                        </select></div>
                                    <input type="text" class="default_title_text input-qty" placeholder="QTY" title="QTY">
                                    <a href="javascript:;" class="cart-btn"><span>&nbsp;</span>Add</a>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="image-block">
                                <a href="#" class="pro-img">
                                    <img src="../../UploadedImages/ProductImages/Large_634315004428131250_Spirit Airlines - Lanyard - Large.jpg" width="236" height="315"
                                        title="Dress Shirt" alt="Dress Shirt"></a><div class="cart-caption">
                                            <span>
                                                <img src="../StaticContents/img/quick-view-ico.png" width="32" height="32" alt="cart"></span><span>QUICKVIEW</span></div>
                            </div>
                            <div class="cart-description">
                                <div class="cart-price cf">
                                    <span class="alignleft">Dress Shirt</span><span class="alignright">$50</span></div>
                                <div class="cart-btn-block cf">
                                    <div class="table-drop cart-dropin">
                                        <select name="size1-a" class="default">
                                            <option value="">XS</option>
                                            <option value="">Small</option>
                                            <option value="">Medium</option>
                                            <option value="">Large</option>
                                        </select></div>
                                    <input type="text" class="default_title_text input-qty" placeholder="1" title="1">
                                    <a href="javascript:;" class="cart-btn"><span>&nbsp;</span>Add</a>
                                </div>
                            </div>
                        </li>
                        <li class="last">
                            <div class="image-block">
                                <a href="#" class="pro-img">
                                    <img src="../../UploadedImages/ProductImages/Large_634322402479565876_spirit-badge-front.jpg" width="236" height="315"
                                        title="Dress Shirt" alt="Dress Shirt"></a><div class="cart-caption">
                                            <span>
                                                <img src="../StaticContents/img/quick-view-ico.png" width="32" height="32" alt="cart"></span><span>QUICKVIEW</span></div>
                            </div>
                            <div class="cart-description">
                                <div class="cart-price cf">
                                    <span class="alignleft">Dress Shirt</span><div class="alignright cart-price-ico">
                                        <div class="cart-paid-block">
                                            This item is being paid by corporate.</div>
                                        <a href="#">
                                            <img src="../NewDesign/img/price-ico.png" width="16" height="18" alt="price"></a></div>
                                </div>
                                <div class="cart-btn-block cf">
                                    <div class="table-drop cart-dropin">
                                        <select name="size2-a" class="default">
                                            <option value="">Size</option>
                                            <option value="">Small</option>
                                            <option value="">Medium</option>
                                            <option value="">Large</option>
                                        </select></div>
                                    <input type="text" class="default_title_text input-qty" placeholder="QTY" title="QTY">
                                    <a href="javascript:;" class="cart-btn"><span>&nbsp;</span>Add</a>
                                </div>
                            </div>
                        </li>                        
                    </ul>
                </div>
                <div class="order-checkout-block">
                    <a href="#" class="blue-btn" title="Add to Cart and Order for a Another Employee">Add to Cart and Order for a Another Employee</a>
                    <button class="royal-largebtn">
                        <strong>Checkout</strong><span class="btn-label"> <em>$150.00</em></span></button>
                </div>
            </div>
        </div>
        <% if (Request.IsLocal)
           { %>
    </div>
    <% }
           else
           { %>
    </section>
    <%} %>
</asp:Content>
