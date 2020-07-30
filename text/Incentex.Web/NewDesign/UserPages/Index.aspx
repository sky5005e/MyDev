<%@ Page Title="incentex | Home" Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master"
    AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="UserPages_Index" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/NewDesign/FrontMasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        //Current Date
        var todayDate = new Date();

        function ShowQuickPopup(StoreProductID, dvCartCaption, dvCartRemove) {
            CheckSessionExistance(siteurl);

            ShowLoader(true);
            $(".fade-layer").show();
            $("#ctl00_ContentPlaceHolder1_pnlProductItem").show();
            var Cartpath = siteurl + "UserFrameItems/ShopProductItem.aspx?spID=" + StoreProductID + "&dvCartAdded=" + dvCartRemove + "&dvCartRemove=" + dvCartCaption + "&IsfromProduct=true";
            $("#iframeCartItem").attr("onload", "ShowLoader(false);");
            $("#iframeCartItem").attr("src", Cartpath);

        }

        function ShowIssuancePopup(PolicyID) {
            CheckSessionExistance(siteurl);
            ShowLoader(true);
            $("#Issuance-block").css('top', '0');
            $(".fade-layer").show();
            $("#ctl00_ContentPlaceHolder1_pnlIssuanceBlock").show();
            var srcpath = siteurl + "UserFrameItems/IssuancePolicy.aspx?pID=" + PolicyID;
            $("#iframeIssuance").attr("onload", "ShowLoader(false);");
            $("#iframeIssuance").attr("src", srcpath);

        }
        
        
         function ShowPopupWhenLoginFirst() {
         
            $("#add-firsttimeloginvideo").css('top', '0');
            $(".fade-layer").show();
            //$("#iframeIssuance").attr("src", srcpath);
            }

        function SetPageIndexActive(currentIndex) {
            $(currentIndex).parent().removeClass('active');
            $(currentIndex).addClass("active");

        }
        // For Repeater size Drop down
        function BindProductItemDetails(ddlsize) {
            CheckSessionExistance(siteurl);
            var mainID = ddlsize.id.replace("_ddlSize", "");
            var soldBy = new String(); // to pass soldby field. here we will pass null value
            var StoreProductID = $("#" + mainID + "_hdnStoreProductID");
            var lblAmount = $("#" + mainID + "_lblAmount");
            var userType = $("#ctl00_ContentPlaceHolder1_hdnUserType").val();
            // get Div
            var dvCartRemove = $("#" + mainID + "_dvCartRemove");
            var dvCartCaption = $("#" + mainID + "_dvCartCaption");
            var btnAdd = $("#" + mainID + "_lnkAddtoCart");
            $.ajax({
                type: "POST",
                url: siteurl + "UserPages/WSUser.asmx/GetProductItemDetails",
                data: '{"StoreProductID": "' + StoreProductID.val() + '", "itemSizeID": "' + ddlsize.value + '","soldBy": "' + soldBy + '"}',
                contentType: "application/json; charset=utf-8",
                processData: false,
                dataType: "json",
                success: function(msg) {
                    var objlst = msg.d;
                    if (objlst != undefined) {
                        // close out price
                        if (objlst.IsCloseOut) {
                            lblAmount.html(objlst.CloseOutPrice.toFixed(2));
                        }
                        else if (objlst.Level3PricingStatus != null && objlst.Level3PricingStatus == '135' && todayDate.getDate() <= objlst.Level3PricingEndDate && todayDate.getDate() >= objlst.Level3PricingStartDate) {
                            lblAmount.html(objlst.Level3.toFixed(2));
                        }
                        else if (userType == 'Company Admin') {
                            lblAmount.html(objlst.Level1.toFixed(2));
                        }
                        else if (userType == 'Supplier Employee') {
                            lblAmount.html(objlst.Level4.toFixed(2));
                        }
                        else {
                            lblAmount.html(objlst.Level2.toFixed(2));
                        }
                        // If item exist in cart then display dvCartRemove
                        if (objlst.AlreadyInCart) {
                            // set it value;
                            dvCartRemove.find("span").attr("id", objlst.MyShoppingCartID);
                            dvCartRemove.show();
                            dvCartCaption.hide();
                            btnAdd.addClass("active");
                        }
                        else {
                            _myShoppingCartID = 0;
                            dvCartCaption.show();
                            dvCartRemove.hide();
                            btnAdd.removeClass('active');
                        }
                    }
                },

                error: function(x, e) {
                    if (x.status == 500) {
                        GeneralAlertMsg("An error has occurred during processing your request.");
                    }
                }
            });
        }




        // For Bind Cart Items
        function BindCartItems() {
            var dvCart = $("#dvCart");
            var lblCartAmount = $("#ctl00_lblCartTotalAmount");
            $.ajax({
                type: "POST",
                url: siteurl + "UserPages/WSUser.asmx/GetAllCartItems",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                processData: false,
                dataType: "json",
                success: function(msg) {
                    var objlst = msg.d;
                    if (objlst != undefined) {
                        dvCart.html(objlst.HItemsHtml);
                        lblCartAmount.html(objlst.TotalNumber);
                    }
                },

                error: function(x, e) {
                    if (x.status == 500) {
                        GeneralAlertMsg("An error has occurred during processing your request.");
                    }
                }
            });
        }
        function SetAddedToCartItems(_dvCartItemadded, _dvCartCaption) {
            $(".fade-layer").hide();
            $("#" + _dvCartItemadded).show();
            $("#" + _dvCartCaption).hide();
            $("#ctl00_ContentPlaceHolder1_pnlProductItem").hide();
        }
        // For add item to cart from repeater Add to cart button
        function AddToCart(lnk) {
            CheckSessionExistance(siteurl);
            if ($(lnk).parents("div.cart-description").siblings("div.image-block").find("div.cart-remove").css("display") == "block") {
                $(lnk).siblings("input.input-qty").val('');
                GeneralAlertMsg("Item already exist in your cart");
                return false;
            }
            else {
                var mainID = lnk.id.replace("_lnkAddtoCart", "");
                var StoreProductID = $("#" + mainID + "_hdnStoreProductID");
                var soldBy = new String();
                var qty = $("#" + mainID + "_txtQty");
                var itemSize = $("#" + mainID + "_ddlSize");
                var lblAmount = $("#" + mainID + "_lblAmount");
                //Item added to cart div and div Caption
                var dvCartItemadded = $("#" + mainID + "_dvCartRemove");
                var dvCartCaption = $("#" + mainID + "_dvCartCaption");
                if ($(qty).val() == 'QTY') {
                    $(qty).addClass("ErrorField");
                    $(qty).focus();
                    return;
                }
                $.ajax({
                    type: "POST",
                    url: siteurl + "UserPages/WSUser.asmx/AddProductToCart",
                    data: '{"StoreProductID": "' + $(StoreProductID).val() + '","itemSizeID": "' + $(itemSize).val() + '","soldBy": "' + soldBy + '","Quantity": "' + $(qty).val() + '","Price":"' + $(lblAmount).html() + '"}',
                    contentType: "application/json; charset=utf-8",
                    processData: false,
                    dataType: "json",
                    success: function(msg) {
                        BindCartItems();
                        //do further stuff
                        $(qty).removeClass("ErrorField");
                        $(qty).val('QTY');
                        dvCartItemadded.show();
                        dvCartCaption.hide();
                        $(lnk).attr("disabled", "disabled");
                        $(lnk).addClass("active");
                        var newCartID = msg.d;
                        dvCartItemadded.find("span").attr("id", newCartID);
                    },

                    error: function(x, e) {
                        if (x.status == 500) {
                            GeneralAlertMsg("An error has occurred during processing your request.");
                        }
                    }
                });
            }
        }


        //To set the Span ID as ShoppingCartID if that item exist in the Cart
        function SetSpanID(dvCartRemoveID, MyShoppingCartID) {
            var dvCartRemove = $("#" + dvCartRemoveID);
            dvCartRemove.find("span").attr("id", MyShoppingCartID);
        }

        // For Remove Items from  Cart
        function RemoveCartItems(lnk) {
            CheckSessionExistance(siteurl);
            var mainID = lnk.id.replace("_lnkRemoveItem", "");
            var dvCartRemove = $("#" + mainID + "_dvCartRemove");
            var dvCartCaption = $("#" + mainID + "_dvCartCaption");
            var _spanID = dvCartRemove.find("span").attr("id");
            $.ajax({
                type: "POST",
                url: siteurl + "UserPages/WSUser.asmx/RemoveItemFromCart",
                data: '{"MyShoppingCartID": "' + _spanID + '"}',
                contentType: "application/json; charset=utf-8",
                processData: false,
                dataType: "json",
                success: function(msg) {
                    GeneralAlertMsg("Cart Item successfully removed.");
                    BindCartItems();
                    dvCartCaption.show();
                    dvCartRemove.hide();
                    $("#" + mainID + "_lnkAddtoCart").removeClass("active");
                },
                error: function(x, e) {
                    if (x.status == 500) {
                        GeneralAlertMsg("An error has occurred during processing your request.");
                    }
                }
            });
        }
    </script>

    <script language="javascript" type="text/javascript">
        $().ready(function() {
            $("#ctl00_ContentPlaceHolder1_aclose").on('click', function(e) {
                $("#ctl00_ContentPlaceHolder1_pnlProductItem").hide();
            });

            $('#accordion li').children('div').hide();
            $('#accordion a').click(function() {
                $(this).parent().siblings('.active').removeClass('active').find('div').slideUp('fast');
                if ($(this).parent().hasClass('active')) {
                    $(this).next('div').slideUp('fast');
                    $(this).parent().removeClass('active');
                } else {
                    $(this).next('div').slideDown('fast');
                    $(this).parent().addClass('active');
                }
            });

            SetAccordionExpanded();

        });
        function SetAccordionExpanded() {
            var _id = $("#ctl00_ContentPlaceHolder1_hdnExpandedAccordion").val();
            $("#" + _id).parent().parent().parent().siblings('a').addClass("active");
            $("#" + _id).parent().parent().parent().show();
            $("#" + _id).parent().addClass("active");
        }
    </script>

    <style>
        .video-content h2
        {
            color: #323232;
            float: left;
            font-size: 25px;
            font-weight: normal;
            line-height: 33px;
            padding-bottom: 28px;
            padding: 15px;
        }
        #add-firsttimeloginvideo .video-pop-block
        {
            overflow-x: hidden;
            overflow-y: auto;
        }
        #ctl00_ContentPlaceHolder1_cbViewLeter
        {
            margin-top: -20px;
            margin-left: 20px;
        }
        .dmsvideo-content .videobox-shadow-login
        {
            background: none repeat scroll 0 0 #313131;
            height: 364px;
            margin: 0 auto;
            position: relative;
            width: 612px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <input type="hidden" value="shop-link" id="hdnActiveLink" />
    <%--<% if (!Request.IsLocal)
       { %>
    <section id="container" class="cf store-page">
    <% }
       else
       { %>--%>
    <div id="container" class="cf store-page">
        <%--<%} %> --%>
        <div class="narrowcolumn alignleft">
            <div class="store-left-block">
                <div class="subNav-block">
                    <h2 class="title-txt">
                        Products</h2>
                    <asp:Repeater ID="rptCategories" runat="server" OnItemDataBound="rptCategories_ItemDataBound">
                        <HeaderTemplate>
                            <ul id="accordion" class="cartNav cf">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnCategoryID" runat="server" Value='<%#Eval("CategoryID")%>' />
                            <li id="liCategory" class="selected"><a href="javascript:;" class="arrow" id="lnkCategory"
                                runat="server" title='<%# Eval("CategoryName") %>'><span></span>
                                <%# Eval("CategoryName") %></a>
                                <div id="dvSubCategory" runat="server" style="display: none;">
                                    <asp:Repeater ID="rptSubCategories" runat="server" OnItemDataBound="rptSubCategories_ItemDataBound"
                                        OnItemCommand="rptSubCategories_ItemCommand">
                                        <HeaderTemplate>
                                            <ul class="cf">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnSubCategoryID" runat="server" Value='<%#Eval("SubCategoryID")%>' />
                                            <asp:HiddenField ID="hdnCategoryName" runat="server" Value='<%#Eval("CategoryName")%>' />
                                            <li>
                                                <asp:LinkButton ID="lnkSubCategory" runat="server" CommandArgument='<%# Eval("SubCategoryID") %>'
                                                    CommandName="display" Text='<%# Eval("SubCategoryName") %>'></asp:LinkButton></li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
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
                                        <asp:PlaceHolder runat="server"><em>
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
                <div style="height: 52px;vertical-align:middle;background-color:#F5F5F5;border: 1px solid #E4E4E4;padding-left:17px;padding-top:18px;" id="DivGuideline" runat ="server" visible="false"  >
                    <div style="height: 40px; vertical-align: middle; padding-top: 7px;" class="MediaDocLink"
                          visible="false">
                        <a id="atagGuideline" runat="server" class="MediaDocLinkText" style="text-decoration: none;"
                            title="Guidelines Manual">Guidelines Manual</a>
                    </div>
                </div>
            </div>
        </div>
        <div class="widecolumn alignright">
            <div class="store-right-block">
                <div class="store-header cf">
                    <asp:Label ID="lblSelectionTitle" runat="server" class="store-title"></asp:Label>
                    <asp:LinkButton ID="lnkViewAllTop" runat="server" OnClick="lnkViewAll_Click" class="view-link">VIEW ALL</asp:LinkButton>
                    <%--START Top paging--%>
                    <div class="pagination alignright">
                        <asp:LinkButton ID="lnkbtnPrevious" runat="server" OnClick="lnkbtnPrevious_Click"
                            class="left-arrow"></asp:LinkButton>
                        <asp:DataList ID="dtTopPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>' OnClientClick="SetPageIndexActive(this);"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:LinkButton ID="lnkbtnNext" runat="server" OnClick="lnkbtnNext_Click" class="right-arrow"></asp:LinkButton>
                    </div>
                    <%--END Top paging--%>
                </div>
                <%-- START Binding store product items--%>
                <asp:Repeater ID="rptProductList" runat="server" OnItemDataBound="rptProductList_ItemDataBound">
                    <HeaderTemplate>
                        <div class="cart-slider-display">
                            <ul class="cf">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li id="liRepeater" runat="server">
                            <div class="image-block">
                                <a id="azoom" runat="server" class="pro-img" style="width: 236px !important; height: 315px !important;">
                                    <img id="imgProduct" runat="server" width="236" height="315" src="../StaticContents/img/progress-newloader.gif"
                                        title='<%# Eval("Summary")%>' alt='<%# Eval("Summary")%>' /></a>
                                <%--Display when Item  Added to cart--%>
                                <div id="dvCartRemove" runat="server" class="cart-remove">
                                    <a href="javascript: void(0);" class="remove-cart" id="lnkRemoveItem" runat="server"
                                        onclick="RemoveCartItems(this);">
                                        <img src="../StaticContents/img/cart-remove-btn.png" width="66" height="18" alt="remove"></a>
                                    <span>
                                        <img src="../StaticContents/img/check-cart.png" width="32" height="28" alt="cart"></span>
                                    <span>ITEM HAS BEEN ADDED TO YOUR CART</span></div>
                                <%--Display if Item is not Added to cart--%>
                                <div id="dvCartCaption" runat="server" class="cart-caption popup-openlink">
                                    <span>
                                        <img id="ImgQckView" runat="server" src="../StaticContents/img/quick-view-ico.png"
                                            width="32" height="32" alt="cart" /></span> <span>QUICKVIEW</span></div>
                            </div>
                            <asp:HiddenField ID="hdnStoreProductID" runat="server" Value='<%#Eval("StoreProductID")%>' />
                            <asp:HiddenField ID="hdnProductImage" runat="server" Value='<%#Eval("ProductImage")%>' />
                            <asp:HiddenField ID="hdnLargeProductImage" runat="server" Value='<%#Eval("LargerProductImage")%>' />
                            <div class="cart-description">
                                <div class="cart-price cf">
                                    <span class="alignleft">
                                        <asp:Label ID="lblsummary" runat="server" Text='<%# Convert.ToString(Eval("Summary")).Length>15 ? Convert.ToString(Eval("Summary")).Substring(0,15) : Eval("Summary")  %>'
                                            ToolTip='<%# Eval("Summary")%>'></asp:Label></span> <span class="alignright">$<asp:Label
                                                ID="lblAmount" runat="server"></asp:Label></span></div>
                                <div class="cart-btn-block cf">
                                    <div class="table-drop cart-dropin">
                                        <asp:DropDownList ID="ddlSize" runat="server" class="default" onchange="BindProductItemDetails(this);">
                                        </asp:DropDownList>
                                    </div>
                                    <asp:TextBox ID="txtQty" runat="server" class="input-qty" value="QTY" onfocus="if (this.value == 'QTY') {this.value=''}"
                                        onblur="if(this.value == '') { this.value='QTY'}" onchange="CheckNum(this.id)"></asp:TextBox>
                                    <a href="javascript:;" class="cart-btn" id="lnkAddtoCart" runat="server" onclick="AddToCart(this);">
                                        <span>&nbsp;</span>Add</a>
                                </div>
                            </div>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul> </div>
                    </FooterTemplate>
                </asp:Repeater>
                <%-- END Binding store product items--%>
                <%--START Bottom paging--%>
                <div class="store-footer cf">
                    <a href="#" class="store-title">BACK TO TOP</a>
                    <asp:LinkButton ID="lnkViewAllBottom" runat="server" OnClick="lnkViewAll_Click" class="view-link">VIEW ALL</asp:LinkButton>
                    <div class="pagination alignright">
                        <asp:LinkButton ID="lnkbtnBottomPrevious" runat="server" OnClick="lnkbtnPrevious_Click"
                            class="left-arrow"></asp:LinkButton>
                        <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>' OnClientClick="SetPageIndexActive(this);"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:LinkButton ID="lnkbtnBottomNext" runat="server" OnClick="lnkbtnNext_Click" class="right-arrow"></asp:LinkButton>
                    </div>
                </div>
                <%--END Bottom paging--%>
            </div>
        </div>
        <%--<% if (Request.IsLocal)
           { %>--%>
    </div>
    <%--<% }
           else
           { %>
    </section>
    <%} %>--%>
    <asp:Panel ID="pnlProductItem" runat="server" Style="display: none;">
        <div class="popup-outer" id="shop-popup">
            <div class="popupInner">
                <div class="shop-popup">
                    <a id="aclose" runat="server" href="javascript:;" class="close-btn">Close</a>
                    <iframe id="iframeCartItem" style="height: 517px; width: 100%;"></iframe>
                </div>
            </div>
        </div>
    </asp:Panel>
    <%--Start Popup Help Video  --%>
    <asp:Panel ID="pnlIssuanceBlock" runat="server" Style="display: none;">
        <div class="popup-outer" id="Issuance-block">
            <div class="popupInner">
                <div class="popup">
                    <a href="javascript: void(0);" class="close-btn">Close</a>
                    <iframe id="iframeIssuance" style="height: 517px; width: 100%;"></iframe>
                </div>
            </div>
        </div>
    </asp:Panel>
    <%--END Popup Help Video  --%>
    <div id="add-firsttimeloginvideo" class="popup-outer">
        <!--" -->
        <div class="popupInner">
            <a href="javascript:;" class="close-btn" id="a3">Close</a>
            <div class="video-pop-block" style="height: 519px;">
                <div class="dmsvideo-content video-content emp-height">
                    <h2>
                        Welcome to Incentex</h2>
                    <div class="videobox-shadow-login" id="divvidimage" runat="server" style="width: 612px;
                        height: 364px;">
                        <%--<a id="AVidTag" title="Play" class="videoplay-icon" href="#" runat="server">Play</a>--%>
                        <asp:LinkButton ID="lbPlayPubVideo" CssClass="videoplay-icon" runat="server" OnClick="lbPlayPubVideo_Click">Play</asp:LinkButton>
                        <asp:Literal ID="ltPubvideotag" runat="server"></asp:Literal>
                    </div>
                    <div class="videobox-shadow-login" runat="server" visible="false" id="divShowPubVideo">
                        <iframe id="iframepubvideo" runat="server" width="612px" height="364px" frameborder="0"
                            visible="false"></iframe>
                    </div>
                    <div class="dmsvideo-publishtext" style="display: block">
                        <div>
                            <div class="basic-form cf">
                                <div>
                                    <ul class="cf publishtext-list">
                                        <li class="alignleft" style="width: 350px;">
                                            <label>
                                                <span style="float: left; padding-left: 6px;">
                                                    <asp:CheckBox ID="cbDontShowAgain" runat="server" />
                                                    <div style="position: relative; width: 500px;">
                                                        <label class="ViewLater" style="margin-top: -20px; margin-left: 24px; position: absolute;">
                                                            Don't show this video again.
                                                        </label>
                                                    </div>
                                                </span>
                                            </label>
                                        </li>
                                    </ul>
                                </div>
                                <div class="emp-btn-block" style="padding-left: 15px;">
                                    <asp:LinkButton ID="btnOk" runat="server" TabIndex="21" CssClass="blue-btn submit"
                                        OnClick="btnOk_Click">
                                    Ok
                                    </asp:LinkButton>
                                    <asp:HiddenField ID="hfMediaVideoId" runat="server" Value="0" />
                                    <asp:HiddenField ID="HfUrlOfMyFirstVideo" runat="server" />
                                    <%-- OnClick="btnOk_Click"--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--Popup video when first time login  --%>
    <%--END Popup Help Video  --%>
    <asp:HiddenField ID="hdnUserType" runat="server" />
    <asp:HiddenField ID="hdnExpandedAccordion" runat="server" />
</asp:Content>
