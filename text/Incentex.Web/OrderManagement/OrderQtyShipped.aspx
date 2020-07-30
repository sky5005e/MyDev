<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderQtyShipped.aspx.cs" Inherits="OrderManagement_OrderQtyShipped"
    Title="Order Management>>Order Quantity Shipped" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calendar-small.png',
                buttonImageOnly: true
            });
        });

        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }
        function textboxFinder(id) {
            var txt = document.getElementById('<%=txtTrackingNo.ClientID%>');
            if (txt.value == "") {
                alert('Please enter tracking number');
                return false;
            }
        }

        function CheckNoOfBox(id) {
            var txt = document.getElementById(id);
            if (!IsOnlyNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.focus();
            }

        }

        function IsOnlyNumeric(sText) {
            var ValidChars = "0123456789";
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

    <style type="text/css">
        .fontsizesmall
        {
            font-size: small;
            width: 65px !important;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="black_round_box">
        <div class="black2_round_top">
            <span></span>
        </div>
        <div class="black2_round_middle">
            <div class="form_pad">
                <div class="pro_search_pad" style="width: 920px;">
                    <mb:MenuUserControl ID="menucontrol" runat="server" />
                    <div class="black_top_co">
                        <span>&nbsp;</span></div>
                    <div class="black_middle order_detail_pad">
                        <div style="text-align: center; color: Red; font-size: larger;">
                            <asp:Label ID="lblmsg" runat="server">
                            </asp:Label>
                        </div>
                        <div>
                            <asp:GridView ID="gvShippedOrderDetail" runat="server" AutoGenerateColumns="false"
                                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                RowStyle-CssClass="ord_content">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                            <span></span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdnQuantityShipped" Value='<%# Eval("ShipQuantity") %>' />
                                            <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                                            <asp:HiddenField ID="hdnShipID" runat="server" Value='<%# Eval("ShippID") %>' />
                                            <asp:HiddenField ID="hdnMyShoppingCartId" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />
                                            <asp:HiddenField ID="hdnSupplierid" runat="server" Value='<%# Eval("SupplierId") %>' />
                                            <asp:HiddenField ID="hdhQtyOrder" runat="server" Value='<%# Eval("QtyOrder") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Item #</span>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label CssClass="first" runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Ordered</span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label ID="txtQtyOrder" runat="server" Text='<%# Eval("QtyOrder") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Remaining</span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label ID="txtQtyOrderRemaining" runat="server" Text='<%# Eval("RemaingQutOrder") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Shipped<br />
                                            </span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblQtyShip" runat="server" Text='<%# Eval("ShipQuantity") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Color</span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:Image ID="imgColor" runat="server" Height="20" Width="20" ImageUrl='<%# "~/admin/Incentex_Used_Icons/" + Eval("ColorIcon") %>' />
                                            </span>
                                            <asp:HiddenField runat="server" ID="hdnColor" Value='<%# Eval("Color") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="9%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Size</span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSize" ToolTip='<%# Eval("Size") %>' Text='<%# Convert.ToString(Eval("Size")).Length > 10 ? Convert.ToString(Eval("Size")).Substring(0, 10) + "..." : Convert.ToString(Eval("Size")) %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="9%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Description</span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblDescription" ToolTip='<%# Convert.ToString(Eval("ProductDescrption")) %>'
                                                Text='<%# Convert.ToString(Eval("ProductDescrption")).Length > 20 ? Convert.ToString(Eval("ProductDescrption")).Substring(0, 20) + "..." : Convert.ToString(Eval("ProductDescrption")) %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Backordered Until</span>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblBackOrderUntil" Text='<%# Eval("BackOrderUntil") == null ? "---" : Eval("BackOrderUntil","{0:d}") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="11%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <div class="black_top_co">
                            <span>&nbsp;</span></div>
                        <div class="black_middle order_detail_pad">
                            <div>
                                <div class="alignleft" style="width: 49%;">
                                    <div class="tab_content_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="tab_content" style="height: 200px;">
                                        <table class="order_detail">
                                            <tr>
                                                <td>
                                                    <label class="fontsizesmall">
                                                        Ship Date:
                                                    </label>
                                                    <span class="calender_l">
                                                        <asp:TextBox ID="txtShipDate" Style="background-color: #303030; border: medium none;
                                                            color: #ffffff; width: 120px; padding: 2px" runat="server" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="fontsizesmall">
                                                        Shipper:
                                                    </label>
                                                    <asp:DropDownList ID="ddlShipper" Style="background-color: #303030; border: medium none;
                                                        color: #ffffff; width: 100px; padding: 2px" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlShipper_SelectedIndexChange">
                                                        <%--onchange="pageLoad(this,value);"--%>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr id="otherShipper" runat="server" visible="false">
                                                <td>
                                                    <label class="fontsizesmall">
                                                        Other Shipper:
                                                    </label>
                                                    <asp:TextBox ID="txtOtherShipper" Style="background-color: #303030; border: medium none;
                                                        color: #ffffff; width: 120px; padding: 2px" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 5px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="fontsizesmall">
                                                        Boxes:
                                                    </label>
                                                    <asp:TextBox ID="txtBoxes" Style="background-color: #303030; border: medium none;
                                                        color: #ffffff; width: 120px; padding: 2px" runat="server" onchange="CheckNoOfBox(this.id)"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                                      { %>
                                                    <label class="fontsizesmall">
                                                        Status 2:
                                                    </label>
                                                    <%}
                                                      else
                                                      { %>
                                                    &nbsp;
                                                    <%} %>
                                                    <%if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                                      { %>
                                                    <asp:DropDownList ID="drpStatusTwo" Style="background-color: #303030; border: medium none;
                                                        color: #ffffff; width: 150px; padding: 2px" onchange="pageLoad(this,value);"
                                                        runat="server" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <%}
                                                      else
                                                      { %>
                                                    &nbsp;
                                                    <%} %>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="tab_content_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                <div class="alignright" style="width: 49%;">
                                    <div class="tab_content_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="tab_content" style="height: 200px;">
                                        <table class="order_detail" border="0">
                                            <tr>
                                                <td style="width: 50%">
                                                </td>
                                                <td style="width: 50%">
                                                    <table class="order_detail">
                                                        <tr valign="bottom" id="trNumber" runat="server">
                                                            <td>
                                                                <label class="fontsizesmall" style="padding-left: 0px; width: 110px!important;">
                                                                    Tracking Numbers:
                                                                </label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTrackingNo" runat="server" Style="background-color: #303030;
                                                                    border: medium none; color: #ffffff; width: 105px; padding: 2px" onchange="CheckTrackingNumber(this.id)"
                                                                    CssClass="w_label"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnTrackingNumber" runat="server" Text="Add Tracking #" CommandName="AddTrackingNumber"
                                                                    OnClientClick="javascript:return textboxFinder(this.id);" OnClick="btnTrackingNumber_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="spacer15">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td colspan="2" align="right">
                                                                <asp:GridView ID="grvTrackingNumber" runat="server" OnRowCommand="grvTrackingNumber_RowCommand"
                                                                    AutoGenerateColumns="false">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                Tracking Number
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle CssClass="fontsizesmall" Width="80%" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTrackingNumber" Text='<%#Eval("trackingnuber")%>' CssClass="fontsizesmall"
                                                                                    runat="server"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="70%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                Action
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle CssClass="fontsizesmall centeralign" Width="20%" />
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkBtnDelete" Text="Delete" CommandArgument='<%#Eval("trackingnuber")%>'
                                                                                    CommandName="deletetrackingnumber" CssClass="fontsizesmall" OnClientClick="return confirm('Are you sure you want to delete tracking number?');"
                                                                                    runat="server"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="30%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="tab_content_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                <div class="alignnone">
                                </div>
                            </div>
                        </div>
                        <div class="black_bot_co">
                            <span>&nbsp;</span></div>
                        <div class="centeralign">
                        </div>
                        <br />
                        <div class="botbtn centeralign">
                            <asp:LinkButton ID="lnkSaveOrderDetails" class="grey2_btn" runat="server" OnClick="lnkSaveOrderDetails_Click"><span>Save</span></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>