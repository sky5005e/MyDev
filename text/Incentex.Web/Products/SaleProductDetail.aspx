<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SaleProductDetail.aspx.cs" Inherits="Products_SaleProductDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" src="../JS/jquery.prettyPhoto.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <div class="form_pad form_table" style="padding-top: 0px !important;">
        <div>
            <table class="product_detail" cellpadding="0" cellspacing="0" border="0">
                <tr style="height: 40px;">
                    <td style="width: 25%;">
                    </td>
                    <td style="width: 50%;">
                    </td>
                    <td style="width: 25%;" rowspan="3">
                        <div id="dvImages" runat="server">
                            <div style="height: 135px;">
                                <asp:Image ID="imgSale" runat="server" Visible="false" ImageUrl="~/Images/Sale.png" />
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
                    <td style="width: 50%;">
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
                                <div id="dvPriPhotoContainer" class="upload_photo aligncenter gallery" runat="server">
                                    <asp:DataList ID="dtProductImages" runat="server" RepeatDirection="Vertical" RepeatColumns="1"
                                        RepeatLayout="Table" OnItemDataBound="dtProductImages_ItemDataBound">
                                        <ItemTemplate>
                                            <div class="alignleft item">
                                                <p class="upload_photo gallery">
                                                    <a id="prettyphotoDiv" rel='prettyPhoto[a]' runat="server">
                                                        <img id="imgSplashImage" runat="server" />
                                                    </a>
                                                    <asp:HiddenField ID="hdnimagestatus" runat="server" Value='<%# Eval("ProductImageActive") %>' />
                                                    <asp:HiddenField ID="hdndocumentname" runat="server" Value='<%# Eval("ProductImage") %>' />
                                                    <asp:HiddenField ID="hdnlargerimagename" runat="server" Value='<%# Eval("LargerProductImage") %>' />
                                                </p>
                                            </div>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </div>
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
                            <div style="width: 90%">
                                <table class="form_table">
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span>
                                                </div>
                                                <div class="form_box shipmax_in">
                                                    <span class="input_label">Color</span>
                                                    <img src="" id="imgColor" style="vertical-align: middle;" runat="server" alt="Color" />
                                                    <asp:Label ID="lblColor" runat="server"></asp:Label>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form_td">
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Size Offered</span> <span class="custom-sel label_sel">
                                                        <asp:DropDownList ID="ddlSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSize_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trPrice" runat="server">
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box shipmax_in">
                                                    <span class="input_label">Price</span>
                                                    <asp:TextBox ID="txtPrice" ReadOnly="true" runat="server" CssClass="w_label max_w"></asp:TextBox>
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
                                                    <span class="input_label"><span class="errormessage">On Sale</span></span>
                                                    <asp:TextBox ID="txtL3" ReadOnly="true" runat="server" CssClass="w_label max_w"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form_td">
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Order By</span>
                                                    <asp:TextBox ID="txtOrderIn" runat="server" CssClass="w_label max_w" ReadOnly="true"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtInventoryDate" ReadOnly="true" runat="server" CssClass="w_label max_w"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="spacer10">
                        </div>
                        <div id="dvL3Msg" runat="server">
                            <div style="width: 90%;" class="spacer20">
                                <hr />
                            </div>
                            <div style="color: White; font-size: 15px;">
                                Note: Discount (Special Price) will expire on Date:
                                <asp:Label ID="lblDiscountExpDate" runat="server"></asp:Label>
                            </div>
                        </div>
                    </td>
                    <td style="width: 25%;" class="centeralign">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hfMasterItemNumber" runat="server" />
        <asp:HiddenField ID="hfProductItemId" runat="server" />
    </div>
</asp:Content>
