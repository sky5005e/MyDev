<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IssuancePolicy.aspx.cs" Inherits="NewDesign_UserFrameItems_IssuancePolicy" %>

<%@ Register TagPrefix="uc" TagName="CommonHeader" Src="~/NewDesign/UserControl/NewCommonHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <uc:CommonHeader ID="ucCommonHead" runat="server" />
    <title></title>

    <script type="text/javascript">
        jQuery(document).ready(function() {
            jQuery('#mycarousel').jcarousel({ scroll: 1 });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function() {
            $(".cart-slider-display .image-block").hoverIntent(function() {
                $(".cart-caption", this).animate({ bottom: 0 }, { duration: 400 });
            }, function() {
                $(".cart-caption", this).animate({ bottom: -68 }, { duration: 400 });
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="cart-content">
        <ul class="cart-category cf">
            <li><a class="active" href="#" title="Shirts"><em></em>Shirts</a></li>
            <li><a href="#" title="Accessories"><em></em>Accessories</a></li>
        </ul>
        <div class="cart-text cf">
            <span class="alignleft">Select <em>2 items</em> from the 8 products below.</span><span
                class="alignright">2 of 2 selected</span></div>
        <div class="cart-slider-display">
            <ul id="mycarousel" class="jcarousel-skin-tango">
                <li>
                    <div class="image-block">
                        <a href="#" class="pro-img">
                            <img src="../StaticContents/img/product-img1.jpg" width="236" height="315" title="Dress Shirt" alt="Dress Shirt"></a><div
                                class="cart-caption">
                                <span>
                                    <img src="../StaticContents/img/quick-view-ico.png" width="32" height="32" alt="cart"></span><span>QUICKVIEW</span></div>
                    </div>
                    <div class="cart-description">
                        <div class="cart-price cf">
                            <span class="alignleft">Dress Shirt</span><span class="alignright">$50</span></div>
                        <div class="cart-btn-block cf">
                            <div class="table-drop cart-pop smalldrop">
                                <select name="size" class="default">
                                    <option value="">Size</option>
                                    <option value="">Small</option>
                                    <option value="">Medium</option>
                                    <option value="">Large</option>
                                </select></div>
                            <input type="text" class="default_title_text input-qty" placeholder="QTY" title="QTY">
                            <button class="cart-btn">
                                <span>&nbsp;</span>Add</button>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="image-block">
                        <a href="#" class="pro-img">
                            <img src="../StaticContents/img/product-img2.jpg" width="236" height="315" title="Dress Shirt" alt="Dress Shirt"></a><div
                                class="cart-caption">
                                <span>
                                    <img src="../StaticContents/img/quick-view-ico.png" width="32" height="32" alt="cart"></span><span>QUICKVIEW</span></div>
                    </div>
                    <div class="cart-description">
                        <div class="cart-price cf">
                            <span class="alignleft">Dress Shirt</span><span class="alignright">$50</span></div>
                        <div class="cart-btn-block cf">
                            <div class="select-drop cart-dropin">
                                <select name="size1" class="default">
                                    <option value="">Size</option>
                                    <option value="">Small</option>
                                    <option value="">Medium</option>
                                    <option value="">Large</option>
                                </select></div>
                            <input type="text" class="default_title_text input-qty" placeholder="QTY" title="QTY">
                            <button class="cart-btn">
                                <span>&nbsp;</span>Add</button>
                        </div>
                    </div>
                </li>
                <li class="last">
                    <div class="image-block">
                        <a href="#" class="pro-img">
                            <img src="../StaticContents/img/product-img3.jpg" width="236" height="315" title="Dress Shirt" alt="Dress Shirt"></a><div
                                class="cart-caption">
                                <span>
                                    <img src="../StaticContents/img/quick-view-ico.png" width="32" height="32" alt="cart"></span><span>QUICKVIEW</span></div>
                    </div>
                    <div class="cart-description">
                        <div class="cart-price cf">
                            <span class="alignleft">Dress Shirt</span><span class="alignright">$50</span></div>
                        <div class="cart-btn-block cf">
                            <div class="select-drop cart-dropin">
                                <select name="size2" class="default">
                                    <option value="">Size</option>
                                    <option value="">Small</option>
                                    <option value="">Medium</option>
                                    <option value="">Large</option>
                                </select></div>
                            <input type="text" class="default_title_text input-qty" placeholder="QTY" title="QTY">
                            <button class="cart-btn">
                                <span>&nbsp;</span>Add</button>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="image-block">
                        <a href="#" class="pro-img">
                            <img src="../StaticContents/img/product-img1.jpg" width="236" height="315" title="Dress Shirt" alt="Dress Shirt"></a><div
                                class="cart-caption">
                                <span>
                                    <img src="../StaticContents/img/quick-view-ico.png" width="32" height="32" alt="cart"></span><span>QUICKVIEW</span></div>
                    </div>
                    <div class="cart-description">
                        <div class="cart-price cf">
                            <span class="alignleft">Dress Shirt</span><span class="alignright">$50</span></div>
                        <div class="cart-btn-block cf">
                            <div class="select-drop cart-dropin">
                                <select name="size3" class="default" tabindex="4">
                                    <option value="">Size</option>
                                    <option value="">Small</option>
                                    <option value="">Medium</option>
                                    <option value="">Large</option>
                                </select></div>
                            <input type="text" class="default_title_text input-qty" placeholder="QTY" title="QTY">
                            <button class="cart-btn">
                                <span>&nbsp;</span>Add</button>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="image-block">
                        <a href="#" class="pro-img">
                            <img src="../StaticContents/img/product-img2.jpg" width="236" height="315" title="Dress Shirt" alt="Dress Shirt"></a><div
                                class="cart-caption">
                                <span>
                                    <img src="../StaticContents/img/quick-view-ico.png" width="32" height="32" alt="cart"></span><span>QUICKVIEW</span></div>
                    </div>
                    <div class="cart-description">
                        <div class="cart-price cf">
                            <span class="alignleft">Dress Shirt</span><span class="alignright">$50</span></div>
                        <div class="cart-btn-block cf">
                            <div class="select-drop cart-dropin">
                                <select name="size4" class="default">
                                    <option value="">Size</option>
                                    <option value="">Small</option>
                                    <option value="">Medium</option>
                                    <option value="">Large</option>
                                </select></div>
                            <input type="text" class="default_title_text input-qty" placeholder="QTY" title="QTY">
                            <button class="cart-btn">
                                <span>&nbsp;</span>Add</button>
                        </div>
                    </div>
                </li>
                <li class="last">
                    <div class="image-block">
                        <a href="#" class="pro-img">
                            <img src="../StaticContents/img/product-img3.jpg" width="236" height="315" title="Dress Shirt" alt="Dress Shirt"></a><div
                                class="cart-caption">
                                <span>
                                    <img src="../StaticContents/img/quick-view-ico.png" width="32" height="32" alt="cart"></span><span>QUICKVIEW</span></div>
                    </div>
                    <div class="cart-description">
                        <div class="cart-price cf">
                            <span class="alignleft">Dress Shirt</span><span class="alignright">$50</span></div>
                        <div class="cart-btn-block cf">
                            <div class="select-drop cart-dropin">
                                <select name="size5" class="default">
                                    <option value="">Size</option>
                                    <option value="">Small</option>
                                    <option value="">Medium</option>
                                    <option value="">Large</option>
                                </select></div>
                            <input type="text" class="default_title_text input-qty" placeholder="QTY" title="QTY">
                            <button class="cart-btn">
                                <span>&nbsp;</span>Add</button>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
        <div class="pop-btn-block">
            <a href="#" class="blue-btn">Save Changes</a></div>
    </div>
    </form>
</body>
</html>
