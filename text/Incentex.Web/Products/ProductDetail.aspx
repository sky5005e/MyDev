<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProductDetail.aspx.cs" Inherits="Products_ProductDetail" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 36px;
        }
        .bulkorderquantity
        {
            margin: 0 auto;
            width: 35px;
            background: none repeat scroll 0 0 transparent;
            border: medium none;
            color: #F4F4F4;
            font-size: 11px;
            padding: 5px;
        }
        .orderreturn_box .ord_header th span
        {
            font-weight: bold;
        }
    </style>

    <script language="javascript" type="text/javascript">

        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }

        // Form Validate Start
         var formats = 'xls|xlsx|ods';
         $().ready(function() {

             $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                 objValMsg = $.xml2json(xml);

                 $("#aspnetForm").validate({
                     rules:
                    {
                        ctl00$ContentPlaceHolder1$ddlOrderby: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtOrderQty: { required: true },
                        ctl00$ContentPlaceHolder1$txtExample: { required: true },
                        ctl00$ContentPlaceHolder1$ExcelDoc: { accept: formats }
                    },
                     messages:
                    {
                        ctl00$ContentPlaceHolder1$ddlOrderby: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Order by")
                        },
                        ctl00$ContentPlaceHolder1$txtOrderQty: { required: replaceMessageString(objValMsg, "Required", "Quantity") },
                        ctl00$ContentPlaceHolder1$txtExample: { required: replaceMessageString(objValMsg, "Required", "Name To Engrave") },
                        ctl00$ContentPlaceHolder1$ExcelDoc: { accept: "<br/>" + replaceMessageString(objValMsg, "ImageType", "xls,xlsx,ods") }
                    },
                     errorPlacement: function(error, element) {
                         if (element.attr("name") == "ctl00$ContentPlaceHolder1$ExcelDoc")
                             error.insertAfter("#dvexcel");
                         else
                             error.insertAfter(element);
                     }
                 });
             });


             // Trigger Validation
             $("#<%=lnkAddToCart.ClientID %>").click(function() {
                 return $('#aspnetForm').valid();
             });

         });  // form validate end

        function CheckNum(id) {
            var txt = document.getElementById(id);
            if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.value = "";
                txt.focus();

            }
        }
        function IsNumeric(sText) {
            var ValidChars = "0123456789";
            //var ValidChars = "0123456789";
            var IsNumber = true;
            var Char;


            for (i = 0; i < sText.length && IsNumber == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    IsNumber = false;
                }
            }
            return IsNumber;

        }
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <link media="screen" rel="stylesheet" href="../CSS/colorbox.css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdateProgress runat="server" ID="uprogressPGrid" DisplayAfter="1" AssociatedUpdatePanelID="upExample">
        <ProgressTemplate>
            <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
            </div>
            <div class="updateProgressDiv">
                <img alt="Loading" src="../Images/ajax-loader-large.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upExample" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkAddToCart" />
<asp:PostBackTrigger ControlID="lnkbtnBulkOrderAddToCart"></asp:PostBackTrigger>
        </Triggers>
        <ContentTemplate>
            <div class="form_pad form_table" style="padding-top: 0px !important;">
                <div>
                    <table class="product_detail" cellpadding="0" cellspacing="0">
                        <tr style="height: 40px;">
                            <td style="width: 25%;">
                                &nbsp;
                            </td>
                            <td style="width: 50%;">
                                &nbsp;
                            </td>
                            <td style="width: 25%;" rowspan="3">
                                <div id="dvImages" runat="server" style="width: 224px;">
                                    <div style="height: 135px;">
                                        <asp:Image ID="imgSale" runat="server" Visible="false" ImageUrl="~/Images/Sale.jpg" />
                                    </div>
                                    <div class="spacer20">
                                        &nbsp;
                                    </div>
                                    <div class="spacer20">
                                        &nbsp;
                                    </div>
                                    <div>
                                        <asp:Image ID="imgNew" runat="server" ImageUrl="~/Images/New.png" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                            </td>
                            <td style="width: 75%;">
                                <asp:Label ID="lblMsg" runat="server" Font-Size="Small" Font-Italic="true" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width: 25%;">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%;">
                                <div>
                                    <h4>
                                        <span class="white_header" style="float: none">Item Number</span></h4>
                                    <asp:Label ID="lblItemNumber" runat="server"></asp:Label>
                                </div>
                                <div class="spacer10">
                                </div>
                                <div class="alignleft">
                                    <div class="agent_img">
                                        <%--<span class="tl_co"></span><span class="tr_co"></span><span class="bl_co"></span>
                                <span class="br_co"></span>--%>
                                        <div id="dvPriPhotoContainer" class="upload_photo aligncenter gallery" runat="server">
                                            <%-- <a id="lnkImage" href="../UploadedImages/employeePhoto/employee-photo.gif" runat="server">
                                        <asp:Image ID="imgProduct" runat="server" Width="145" Height="198" AlternateText="Product Image"
                                            ImageUrl="../UploadedImages/employeePhoto/employee-photo.gif" BorderWidth="8px" />
                                    </a>--%>
                                            <asp:DataList ID="dtProductImages" runat="server" RepeatDirection="Vertical" RepeatColumns="1"
                                                RepeatLayout="Table" OnItemDataBound="dtProductImages_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class="alignleft item">
                                                        <p class="upload_photo gallery">
                                                            <a id="prettyphotoDiv" rel='prettyPhoto[a]' href="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                                                runat="server">
                                                                <img id="imgSplashImage" runat="server" src="~/UploadedImages/ProductImages/ProductDefault.jpg" />
                                                            </a>
                                                            <asp:HiddenField ID="hdnimagestatus" runat="server" Value='<%# Eval("ProductImageActive") %>' />
                                                            <asp:HiddenField ID="hdndocumentname" runat="server" Value='<%# Eval("ProductImage") %>' />
                                                            <asp:HiddenField ID="hdnlargerimagename" runat="server" Value='<%# Eval("LargerProductImage") %>' />
                                                        </p>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:DataList>
                                            <asp:DataList ID="dtSubitemsImages" runat="server" RepeatDirection="Vertical" RepeatColumns="1"
                                                RepeatLayout="Table" OnItemDataBound="dtSubitemsImages_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class="alignleft item">
                                                        <p class="upload_photo gallery">
                                                            <a id="SubprettyphotoDiv" rel='prettyPhoto[a]' href="~/UploadedImages/ProductImages/ProductDefault.jpg"
                                                                runat="server">
                                                                <img id="imgSubItemsImage" runat="server" src="~/UploadedImages/ProductImages/ProductDefault.jpg" />
                                                            </a>
                                                            <asp:HiddenField ID="hdnSubItemImages" runat="server" Value='<%# Eval("ItemImage") %>' />
                                                        </p>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                    </div>
                                    <div style="text-align: center;">
                                        <asp:Label Style="line-height: 12px; width: 100%; height: auto;" ID="lblItemDescriptions"
                                            runat="server"></asp:Label>
                                    </div>
                                    <div class="leftalign">
                                        <%--<a href="#" class="btn_pdf2" title="Measurement Chart"><span>Measurement Chart</span></a>--%>
                                        <asp:HyperLink ID="lnkMeasurementChart" Target="_blank" CssClass="btn_pdf2" Visible="true"
                                            runat="server"><span>Measurement Chart</span></asp:HyperLink>
                                    </div>
                                </div>
                            </td>
                            <td style="width: 50%;" class="leftalign">
                                <div>
                                    <h4>
                                        <span class="white_header" style="float: none">Product Description</span></h4>
                                    <p style="padding: 0px 0px 5px 5px;">
                                        <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                    </p>
                                    <%--<p>In-Flight Attendant Jacket</p>
										<p>65% Polyester</p>
										<p>35% Cotton</p>--%>
                                    <div style="width: 90%">
                                        <table class="form_table">
                                            <tr id="trColor" runat="server">
                                                <td>
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span>
                                                        </div>
                                                        <div class="form_box">
                                                            <span class="input_label">Color</span> <span>
                                                                <img src="" id="imgColor" style="vertical-align: middle;" runat="server" alt="Color" /></span>
                                                            <span class="custom-sel label_sel" style="width: 43%;">
                                                                <asp:DropDownList ID="ddlColor" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);"
                                                                    OnSelectedIndexChanged="ddlColor_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </span>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trSizeOffered" runat="server">
                                                <td class="form_td">
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="input_label">Size Offered</span> <span class="custom-sel label_sel">
                                                                <asp:DropDownList ID="ddlSize" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);"
                                                                    OnSelectedIndexChanged="ddlSize_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </span>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trMaterialStyle" runat="server">
                                                <td class="form_td">
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="input_label">Material Style</span> <span class="custom-sel label_sel">
                                                                <asp:DropDownList ID="ddlMaterialStyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterialStyle_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </span>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trL3Price" runat="server">
                                                <td>
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box shipmax_in">
                                                            <span class="input_label">Price</span> <span id="dvPrice" runat="server" style="color: #F4F4F4;
                                                                font-size: 11px;"></span>
                                                            <asp:TextBox ID="txtPrice" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trL3" runat="server">
                                                <td>
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box shipmax_in">
                                                            <span class="input_label"><span class="errormessage">On Sale</span></span> <span
                                                                id="dvL3" runat="server" style="color: #F4F4F4; font-size: 11px;"></span>
                                                            <asp:TextBox ID="txtL3" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trOrderBy" runat="server">
                                                <td class="form_td">
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="input_label">Order By</span> <span class="custom-sel label_sel">
                                                                <asp:DropDownList ID="ddlOrderby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOrderby_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </span>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box shipmax_in">
                                                            <span class="input_label">Order Qty.</span>
                                                            <asp:TextBox ID="txtOrderQty" onchange="CheckNum(this.id)" runat="server" CssClass="w_label max_w"
                                                                MaxLength="10"></asp:TextBox>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trInventoryNo" runat="server">
                                                <td>
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box shipmax_in">
                                                            <span class="input_label">Inventory Balance</span>
                                                            <asp:Label ID="lblInventoryNo" runat="server" Style="color: White;" CssClass="w_label max_w"></asp:Label>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trInventory" runat="server">
                                                <td>
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box shipmax_in">
                                                            <span class="input_label">Inventory to Arrive On: </span>
                                                            <%--<input type="text" class="w_label max_w" value="L3"/>--%>
                                                            <asp:TextBox ID="txtInventoryDate" ReadOnly="true" runat="server" CssClass="w_label max_w"></asp:TextBox>
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <%--div for the Name Bars Started--%>
                                            <tr id="dvNameFormat" runat="server" visible="false">
                                                <td>
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="input_label">Name Format</span>
                                                            <asp:TextBox ID="txtNameFormat" Enabled="false" CssClass="w_label" runat="server" />
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="dvFontFormat" runat="server" visible="false">
                                                <td>
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="input_label">Font Format</span>
                                                            <asp:TextBox ID="txtFontFormat" Enabled="false" CssClass="w_label" runat="server" />
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="dvEnterName" runat="server" visible="false">
                                                <td>
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="input_label" style="padding-left: 4px;">Your Name</span>
                                                            <asp:TextBox ID="txtExample" Enabled="false" CssClass="w_label" runat="server" />
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="dvEmpTitle" runat="server" visible="false">
                                                <td>
                                                    <div>
                                                        <div class="form_top_co">
                                                            <span>&nbsp;</span></div>
                                                        <div class="form_box">
                                                            <span class="input_label" style="padding-left: 4px;">Employee Title</span>
                                                            <asp:TextBox ID="txtEmployeeTitle" Enabled="false" CssClass="w_label" runat="server" />
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <%--End--%>
                                        </table>
                                    </div>
                                </div>
                                <div class="spacer10">
                                </div>
                                <asp:Panel ID="pnlTailoring" runat="server" Visible="false">
                                    <div>
                                        <h4>
                                            <span class="white_header">Tailoring Options:</span></h4>
                                        <div class="clearfix">
                                            <div class="alignleft" style="width: 70%;">
                                                <table class="form_table">
                                                    <tr>
                                                        <td class="formtd">
                                                            <div>
                                                                <div class="form_top_co">
                                                                    <span>&nbsp;</span></div>
                                                                <div class="form_box shipmin_in">
                                                                    <span class="input_label max_w" style="width: 70%;">Please enter your desired length</span>
                                                                    <asp:TextBox ID="txtDesiredLength" runat="server" CssClass="w_label min_w"></asp:TextBox>
                                                                </div>
                                                                <div class="form_bot_co">
                                                                    <span>&nbsp;</span></div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div id="dvL3Msg" runat="server">
                                    <div style="width: 90%;" class="spacer20">
                                        <hr />
                                    </div>
                                    <div style="color: White; font-size: 15px;">
                                        Note: Discount (Special Price) will expire on Date:
                                        <asp:Label ID="lblDiscountExpDate" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div>
                                    <asp:HyperLink ID="lnkTailoringGuidelines" Target="_blank" CssClass="btn_pdf2" Visible="true"
                                        runat="server"><span>Tailoring Guidelines</span></asp:HyperLink>
                                </div>
                                <div>
                                    <asp:LinkButton ID="lnkAddToCart" runat="server" CssClass="grey2_btn" Style="margin-top: 15px"
                                        OnClick="lnkAddToCart_Click"><span>Add to Cart</span>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnBulkOrder" Visible="false" runat="server" CssClass="grey2_btn"
                                        Style="margin-top: 15px" OnClick="lnkbtnBulkOrder_Click"><span>Bulk Order</span>
                                    </asp:LinkButton>
                                </div>
                            </td>
                            <td style="width: 25%;" class="alignleft">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:HiddenField ID="hfMasterItemNumber" runat="server" />
            <asp:HiddenField ID="hfProductItemId" runat="server" />
            <%--Start bulk Order feature popup panel Add feature by mayur on 16-March-2012--%>
            <asp:LinkButton ID="lnkDummyBulkOrder" runat="server" Style="display: none"></asp:LinkButton>
            <at:ModalPopupExtender ID="mpeBulkOrder" TargetControlID="lnkDummyBulkOrder" BackgroundCssClass="modalBackground"
                DropShadow="true" runat="server" PopupControlID="pnlBulkOrder" CancelControlID="closeBulkOrder">
            </at:ModalPopupExtender>
            <asp:Panel ID="pnlBulkOrder" runat="server" Style="display: none;">
                <div class="cboxWrapper" style="display: block; width: 588px; position: fixed; left: 26%;
                    top: 12%;">
                    <div style="">
                        <div class="cboxTopLeft" style="float: left;">
                        </div>
                        <div class="cboxTopCenter" style="float: left; width: 538px;">
                        </div>
                        <div class="cboxTopRight" style="float: left;">
                        </div>
                    </div>
                    <div style="clear: left;">
                        <div class="cboxMiddleLeft" style="float: left; height: 403px;">
                        </div>
                        <div class="cboxContent" style="float: left; width: 538px; display: block; height: 403px;">
                            <div class="cboxLoadedContent" style="display: block; overflow: visible;">
                                <div style="padding: 35px 10px 10px 10px;">
                                    <div style="overflow: auto; height: 320px;">
                                        <asp:GridView ID="grdBulkOrder" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                            CssClass="orderreturn_box" OnRowDataBound="grdBulkOrder_RowDataBound" GridLines="None"
                                            RowStyle-CssClass="ord_content">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Size</span>
                                                        <div class="corner">
                                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="hdnCategoryID" Value='<%# Eval("CategoryID") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnCompanyID" Value='<%# Eval("CompanyID") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnProductDescrption" Value='<%# Eval("ProductDescrption") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnStoreID" Value='<%# Eval("StoreID") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnProductItemID" Value='<%# Eval("ProductItemID") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnStoreProductID" Value='<%# Eval("StoreProductID") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnSubCategoryID" Value='<%# Eval("SubCategoryID") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnWorkgroupID" Value='<%# Eval("WorkgroupID") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnMasterItemNo" Value='<%# Eval("MasterItemNo") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnAllowBackOrderID" Value='<%# Eval("AllowBackOrderID") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnMasterAllowBackOrderID" Value='<%# Eval("MasterItemAllowBackOrderID") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnLevel3PricingStatus" Value='<%# Eval("Level3PricingStatus") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnLevel3PricingEndDate" Value='<%# Eval("Level3PricingEndDate") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnLevel3PricingStartDate" Value='<%# Eval("Level3PricingStartDate") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnLevel3" Value='<%# Eval("Level3") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnLevel1" Value='<%# Eval("Level1") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnLevel2" Value='<%# Eval("Level2") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnLevel4" Value='<%# Eval("Level4") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnCloseOutPrice" Value='<%# Eval("CloseOutPrice") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnTailoringStatusName" Value='<%# Eval("TailoringStatusName") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnRunCharge" Value='<%# Eval("RunCharge") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnFontFormatForNameBars" Value='<%# Eval("FontFormatForNameBars") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnNameFormatForNameBars" Value='<%# Eval("NameFormatForNameBars") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnShowInventoryLevelInStoreID" Value='<%# Eval("ShowInventoryLevelInStoreID") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnItemNumber" Value='<%# Eval("ItemNumber") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnSoldByName" Value='<%# Eval("SoldbyName") %>' />
                                                        <asp:HiddenField runat="server" ID="hdnSupplierID" Value='<%# Eval("SupplierId") %>' />
                                                        <asp:Label runat="server" ID="lblSize" CssClass="first" Text='<%# Eval("ItemSize") %>' />
                                                        <div class="corner">
                                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="g_box centeralign" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Quantity</span>
                                                    </HeaderTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemTemplate>
                                                        <span class="btn_space">
                                                            <asp:TextBox runat="server" onchange="CheckNum(this.id)" CssClass="bulkorderquantity"
                                                                ID="txtQuantity" MaxLength="10"></asp:TextBox>
                                                        </span>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="b_box centeralign qty_selbox" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>In-Stock</span>
                                                    </HeaderTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="hdnInventory" Value='<%# Eval("Inventory") %>' />
                                                        <asp:Label runat="server" ID="lblInStock" Text='<%# Eval("Inventory") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="g_box centeralign" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>New Stock Arriving On</span>
                                                    </HeaderTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblNewStockArrivingOn" Text='<%# Eval("ToArriveOn") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="b_box centeralign" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <span>Stock to Arrive</span>
                                                        <div class="corner">
                                                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <HeaderStyle CssClass="centeralign" />
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblStockToArrive" Text='<%# Eval("OnOrderItemInventory") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="b_box centeralign" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div runat="server" id="dvUpload">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 38%">Upload Excel: </span>&nbsp;                                            
                                            <input type="file" id="ExcelDoc" runat="server" />
                                             <div id="dvexcel"></div>
                                        </div>                                       
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                    <div class="spacer10">
                                    </div>
                                    <div style="text-align: center;">
                                        <asp:LinkButton ID="lnkbtnBulkOrderAddToCart" runat="server" CssClass="grey2_btn"
                                            OnClick="lnkbtnBulkOrderAddToCart_Click"><span>Add to Cart</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="cboxLoadingOverlay" style="height: 503px; display: none;" id="bulkOrdercboxLoadingOverlay">
                                </div>
                                <div class="cboxLoadingGraphic" style="height: 503px; display: none;" id="bulkOrdercboxLoadingGraphic">
                                </div>
                                <div class="cboxTitle" style="display: block;" id="bulkOrderTitle">
                                </div>
                            </div>
                            <div class="cboxClose" style="" id="closeBulkOrder">
                                close</div>
                        </div>
                        <div class="cboxMiddleRight" style="float: left; height: 403px;">
                        </div>
                    </div>
                    <div style="clear: left;">
                        <div class="cboxBottomLeft" style="float: left;">
                        </div>
                        <div class="cboxBottomCenter" style="float: left; width: 538px;">
                        </div>
                        <div class="cboxBottomRight" style="float: left;">
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <%--End bulk upload feature popup panel--%>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkbtnBulkOrderAddToCart" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
