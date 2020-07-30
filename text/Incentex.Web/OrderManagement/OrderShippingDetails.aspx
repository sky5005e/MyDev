<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderShippingDetails.aspx.cs" Inherits="OrderManagement_OrderShippingDetails" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .width300
        {
            font-size: small;
            width: 300px !important;
        }
        .bigwidthlabel
        {
            width: 135px !important;
        }
    </style>

    <script type="text/javascript">
        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();            
            $(".collapsibleContainerContent").each(function() {
                var hdnIamExpanded = document.getElementById($(this).parent().attr("id").substring(0, $(this).parent().attr("id").length - 13) + "hdnIamExpanded");
                if (hdnIamExpanded.value == "true") {
                    $(this).show();
                }
                else if (hdnIamExpanded.value == "false") {
                    $(this).hide();
                }
            });

            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calendar-small.png',
                buttonImageOnly: true
            });
        });
        
        
        //Added on 11 Apr 2011
        function CheckNoOfBox(id) {
            var txt = document.getElementById(id);
            if (!IsOnlyNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.focus();
              }

        }
        
          function CheckTrackingNumber(id) {
            var txt = document.getElementById(id);
            if (!IsValidTrackingNumber(txt.value)) {
                alert("Please enter valid tracking number");
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
        
         function IsValidTrackingNumber(sText) {
            var ValidChars = "0123456789-ABCDEFGHITJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
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
        
        
        function checkvalidations(id) 
        {
        
             var txt = document.getElementById(id);
             var txt1 = txt.id.split('_');
             var fin = txt1[0]+"_"+txt1[1]+"_"+txt1[2]+"_"+txt1[3]+"_"+"txtShipDate";
             var shipper = txt1[0]+"_"+txt1[1]+"_"+txt1[2]+"_"+txt1[3]+"_"+"ddlShipper";
             var NoOfBox = txt1[0]+"_"+txt1[1]+"_"+txt1[2]+"_"+txt1[3]+"_"+"txtNoOfBoxes";
             
             var a = document.getElementById(fin);
             var b = document.getElementById(shipper);
             var c = document.getElementById(NoOfBox);
             
             if(a.value == "")
             {
                alert('Please enter shipping date');
                return false;
             }
             
             if(b.value == "0")
             {
                alert('Please select a shipper');
                return false;
             }
             
             if(c.value == "")
             {
                 alert('Please enter Number Of Boxes');
                 return false;     
             }
        }
        
        function CollapsibleContainerTitleOnClick() {            
            var hdnIamExpanded = document.getElementById($(this).parent().attr("id").substring(0, $(this).parent().attr("id").length - 13) + "hdnIamExpanded");            
            
            // The item clicked is the title div... get this parent (the overall container) and toggle the content within it.
            $(".collapsibleContainerContent", $(this).parent()).slideToggle();
            
            if (hdnIamExpanded.value == "true") {
                hdnIamExpanded.value = "false";
            }
            else if (hdnIamExpanded.value == "false") {
                hdnIamExpanded.value = "true";
            }
        }
    </script>

    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="black_round_box">
        <div class="black2_round_top">
            <span></span>
        </div>
        <div class="alignright">
            <asp:HyperLink ID="lnkRetunrExchangeDetails" CommandName="OrderDetails" class="grey2_btn"
                NavigateUrl="~/ProductReturnManagement/ProductReturn.aspx" runat="server" Visible="false"><span>Return/Exchange</span></asp:HyperLink>
        </div>
        <div class="black2_round_middle">
            <div class="form_pad">
                <div class="pro_search_pad" style="width: 900px;">
                    <mb:MenuUserControl ID="menucontrol" runat="server" />
                    <%--    <asp:UpdatePanel ID="up" runat="server">
                        <ContentTemplate>--%>
                    <div>
                        <%-- <div class="black_top_co">
                            <span>&nbsp;</span></div>--%>
                        <div class="black_middle order_detail_pad">
                            <table class="order_detail" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label>
                                                        Order Number :
                                                    </label>
                                                    <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label>
                                                        Reference Name :
                                                    </label>
                                                    <asp:Label ID="lblOrderBy" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Ordered Date :
                                                    </label>
                                                    <asp:Label ID="lblOrderedDate" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Order Status :
                                                    </label>
                                                    <asp:Label runat="server" ID="lblOrderStatus"></asp:Label>
                                                </td>
                                            </tr>
                                            <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                              {%>
                                            <tr>
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Payment Method :
                                                    </label>
                                                    <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <%} %>
                                            <tr id="trCreditType" runat="server">
                                                <td style="font-size: small;">
                                                    <label style="padding-left: 28px!important;">
                                                        Credit Type :
                                                    </label>
                                                    <asp:Label ID="lblCreditType" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="black_bot_co">
                            <span>&nbsp;</span></div>
                        <div class="spacer15">
                        </div>
                        <div class="black_top_co">
                            <span>&nbsp;</span></div>
                        <div class="black_middle order_detail_pad">
                            <div class="clearfix billing_head">
                                <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                  {%>
                                <div class="alignleft">
                                    <span>Bill To: </span>
                                </div>
                                <%}
                                  else
                                  {%>
                                <div class="alignleft">
                                    <span>&nbsp;</span>
                                </div>
                                <%} %>
                                <div class="alignright">
                                    <span style="padding-left: 29px!important;">Ship To:</span>
                                </div>
                            </div>
                            <div>
                                <%if (IncentexGlobal.CurrentMember.Usertype != Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                                  {%>
                                <div class="alignleft" style="width: 49%;">
                                    <div class="tab_content_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="tab_content">
                                        <table class="order_detail" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBName" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBCompany" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBAddress1" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBAddress2" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBCity" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblBCountry" runat="server" /></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="tab_content_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                                <%}
                                  else
                                  {%>
                                <div class="alignleft" style="width: 49%;">
                                    &nbsp;
                                </div>
                                <%} %>
                                <div class="alignright" style="width: 49%;">
                                    <div class="tab_content_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="tab_content">
                                        <table class="order_detail" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSName" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSCompany" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSAddress" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSAddress2" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSStreet" runat="server" /></label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSCity" runat="server" />
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="width300">
                                                        <asp:Label ID="lblSCountry" runat="server" />
                                                    </label>
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
                        <div class="spacer15">
                        </div>
                        <div class="black_middle order_detail_pad">
                            <asp:Repeater ID="parentRepeater" runat="server" OnItemCommand="parentRepeater_ItemCommand"
                                OnItemDataBound="parentRepeater_ItemDataBound">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="collapsibleContainer" runat="server" id="dvCollapsible" title='<%# Eval("CompanyName")%>' align="center">
                                        <asp:HiddenField ID="hdnSupplierId" runat="server" Value='<%# Eval("Supplierid") %>' />
                                        <asp:HiddenField ID="hdnIamExpanded" runat="server" Value="false" />
                                        <div class="centeralign">
                                            <asp:GridView ID="gvShippedOrderDetail" runat="server" AutoGenerateColumns="false"
                                                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                                RowStyle-CssClass="ord_content">
                                                <Columns>
                                                    <asp:TemplateField SortExpression="ShipQuantity" Visible="false">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkbtnShipQuantity" runat="server" CommandArgument="ShipQuantity"
                                                                CommandName="Sort"><span ></span></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="hdnQuantityShipped" Value='<%# Eval("ShipQuantity") %>' />
                                                            <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                                                            <asp:HiddenField ID="hdnMyShoppingCartId" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />
                                                            <asp:HiddenField ID="hdnSupplierid" runat="server" Value='<%# Eval("SupplierId") %>' />
                                                            <asp:HiddenField ID="hdhQtyOrder" runat="server" Value='<%# Eval("QtyOrder") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box" Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="hdnShipID" runat="server" Text='<%#Eval("ShippID")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ItemNumber">
                                                        <HeaderTemplate>
                                                            <%--<asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                                                CommandName="Sort"><span>Item #<br /></span></asp:LinkButton>--%>
                                                            <span>Item #<br />
                                                            </span>
                                                            <div class="corner">
                                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' CssClass="first" />
                                                            <div class="corner">
                                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                            </div>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="QuantityOrder">
                                                        <HeaderTemplate>
                                                            <%--<asp:LinkButton ID="lnkbtnQuantityOrder" runat="server" CommandArgument="OrderNumber"
                                                                CommandName="Sort"><span>Ordered<br /></span></asp:LinkButton>--%>
                                                            <span>Ordered<br />
                                                            </span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtQtyOrder" runat="server" Text='<%# Eval("QtyOrder") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="RemainingOrderQuantity">
                                                        <HeaderTemplate>
                                                            <%--<asp:LinkButton ID="lnkbtnQuantityOrder" runat="server" CommandArgument="OrderNumber"
                                                                CommandName="Sort"><span >Remaining<br /></span></asp:LinkButton>--%>
                                                            <span>Remaining<br />
                                                            </span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtQtyOrderRemaining" runat="server" Text='<%# Eval("RemaingQutOrder") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="QtyShipped">
                                                        <HeaderTemplate>
                                                            <span>Shipped<br />
                                                            </span>
                                                            <%--<asp:LinkButton ID="lnkbtnQtyShipped" runat="server" CommandArgument="QtyShipped"
                                                                CommandName="Sort"><span >Shipped<br /></span></asp:LinkButton>--%>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQtyShip" runat="server" Text='<%# Eval("ShipQuantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="4%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Color">
                                                        <HeaderTemplate>
                                                            <span>Color</span>
                                                            <%--<asp:LinkButton ID="lnkbtnColor" runat="server" CommandArgument="Color" CommandName="Sort"><span>Color</span></asp:LinkButton>--%>
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
                                                    <asp:TemplateField SortExpression="Size">
                                                        <HeaderTemplate>
                                                            <span>Size</span>
                                                            <%--<asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span>Size</span></asp:LinkButton>--%>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblSize" ToolTip='<%# Eval("Size") %>' Text='<%# Convert.ToString(Eval("Size")).Length > 10 ? Convert.ToString(Eval("Size")).Substring(0, 10) + "..." : Convert.ToString(Eval("Size")) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="9%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Description">
                                                        <HeaderTemplate>
                                                            <span>Description</span>
                                                            <%--<asp:LinkButton ID="lnkbtnSize" runat="server" CommandArgument="Size" CommandName="Sort"><span>Description</span></asp:LinkButton>--%>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblDescription" ToolTip='<%# Convert.ToString(Eval("ProductDescrption")) %>'
                                                                Text='<%# Convert.ToString(Eval("ProductDescrption")).Length > 20 ? Convert.ToString(Eval("ProductDescrption")).Substring(0, 20) + "..." : Convert.ToString(Eval("ProductDescrption")) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box" Width="15%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="BackorderedUntil ">
                                                        <HeaderTemplate>
                                                            <%--<asp:LinkButton ID="lnkbtnBackorderedUntil" runat="server" CommandArgument="BackorderedUntil "
                                                                CommandName="Sort"><span >Backordered Until </span></asp:LinkButton>--%>
                                                            <span>Backordered Until </span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblBackOrderUntil" Text='<%# "&nbsp;" + Eval("BackOrderUntil","{0:d}") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="11%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div class="spacer15">
                                            </div>
                                            <div align="center">
                                                <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn" runat="server" Style="display: none">
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lnkAddNew" CommandName="AddNotes" runat="server" CssClass="grey2_btn">
                                                        <span>+ Add Tracking Information</span>
                                                </asp:LinkButton>
                                                <at:ModalPopupExtender ID="modalAddnotes" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
                                                    DropShadow="true" runat="server" PopupControlID="pnlNotes" CancelControlID="closepopup">
                                                </at:ModalPopupExtender>
                                                <div class="spacer15">
                                                </div>
                                                <asp:Panel ID="pnlNotes" runat="server" Style="display: none;">
                                                    <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
                                                        left: 35%; top: 30%;">
                                                        <div class="pp_top" style="">
                                                            <div class="pp_left">
                                                            </div>
                                                            <div class="pp_middle">
                                                            </div>
                                                            <div class="pp_right">
                                                            </div>
                                                        </div>
                                                        <div class="pp_content_container" style="">
                                                            <div class="pp_left" style="">
                                                                <div class="pp_right" style="">
                                                                    <div class="pp_content" style="height: 400px; display: block;">
                                                                        <div class="pp_loaderIcon" style="display: none;">
                                                                        </div>
                                                                        <div class="pp_fade" style="display: block;">
                                                                            <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                                            <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                                                                <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                                                    style="visibility: visible;">previous</a>
                                                                            </div>
                                                                            <div id="pp_full_res">
                                                                                <div class="pp_inline clearfix">
                                                                                    <div class="form_popup_box">
                                                                                        <div class="label_bar">
                                                                                            <label class="bigwidthlabel">
                                                                                                Shipdate :</label>
                                                                                            <span class="calender_l">
                                                                                                <asp:TextBox ID="txtShipDate" runat="server" CssClass="cal_w datepicker1 min_w popup_input"></asp:TextBox>
                                                                                            </span>
                                                                                        </div>
                                                                                        <div class="label_bar">
                                                                                            <label class="bigwidthlabel">
                                                                                                Shipper :</label>
                                                                                            <span>
                                                                                                <asp:DropDownList CssClass="popup_input" Width="150px" ID="ddlShipper" runat="server"
                                                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlShipper_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                            </span>
                                                                                        </div>
                                                                                        <div class="label_bar" id="otherShipper" runat="server" visible="false">
                                                                                            <label class="bigwidthlabel">
                                                                                                Other Shipper:</label>
                                                                                            <span>
                                                                                                <asp:TextBox ID="txtOtherShipper" runat="server" CssClass="cal_w min_w popup_input"></asp:TextBox>
                                                                                            </span>
                                                                                        </div>
                                                                                        <div class="label_bar">
                                                                                            <label class="bigwidthlabel">
                                                                                                Number Of Boxes :</label>
                                                                                            <span>
                                                                                                <asp:TextBox class="popup_input" ID="txtNoOfBoxes" onchange="CheckNoOfBox(this.id)"
                                                                                                    runat="server"></asp:TextBox></span>
                                                                                        </div>
                                                                                        <div class="label_bar">
                                                                                            <label class="bigwidthlabel">
                                                                                                Tracking Number 1 :</label>
                                                                                            <span>
                                                                                                <asp:TextBox class="popup_input" ID="txtTrackingNumber_1" onchange="CheckTrackingNumber(this.id)"
                                                                                                    runat="server"></asp:TextBox></span>
                                                                                        </div>
                                                                                        <div class="label_bar">
                                                                                            <label class="bigwidthlabel">
                                                                                                Tracking Number 2 :</label>
                                                                                            <span>
                                                                                                <asp:TextBox class="popup_input" ID="txtTrackingNumber_2" onchange="CheckTrackingNumber(this.id)"
                                                                                                    runat="server"></asp:TextBox></span>
                                                                                        </div>
                                                                                        <div class="label_bar">
                                                                                            <label class="bigwidthlabel">
                                                                                                Tracking Number 3 :</label>
                                                                                            <span>
                                                                                                <asp:TextBox class="popup_input" ID="txtTrackingNumber_3" onchange="CheckTrackingNumber(this.id)"
                                                                                                    runat="server"></asp:TextBox></span>
                                                                                        </div>
                                                                                        <div class="label_bar">
                                                                                            <label class="bigwidthlabel">
                                                                                                Tracking Number 4 :</label>
                                                                                            <span>
                                                                                                <asp:TextBox class="popup_input" ID="txtTrackingNumber_4" onchange="CheckTrackingNumber(this.id)"
                                                                                                    runat="server"></asp:TextBox></span>
                                                                                        </div>
                                                                                        <div class="label_bar">
                                                                                            <label class="bigwidthlabel">
                                                                                                Tracking Number 5 :</label>
                                                                                            <span>
                                                                                                <asp:TextBox class="popup_input" ID="txtTrackingNumber_5" onchange="CheckTrackingNumber(this.id)"
                                                                                                    runat="server"></asp:TextBox></span>
                                                                                        </div>
                                                                                        <div class="label_bar">
                                                                                            <label class="bigwidthlabel">
                                                                                                Tracking Number 6 :</label>
                                                                                            <span>
                                                                                                <asp:TextBox class="popup_input" ID="txtTrackingNumber_6" onchange="CheckTrackingNumber(this.id)"
                                                                                                    runat="server"></asp:TextBox></span>
                                                                                        </div>
                                                                                        <div class="label_bar">
                                                                                            <label class="bigwidthlabel">
                                                                                                Tracking Number 7 :</label>
                                                                                            <span>
                                                                                                <asp:TextBox class="popup_input" ID="txtTrackingNumber_7" onchange="CheckTrackingNumber(this.id)"
                                                                                                    runat="server"></asp:TextBox></span>
                                                                                        </div>
                                                                                        <div class="centeralign">
                                                                                            <asp:LinkButton ID="lnkButton" CommandName="SAVECACE" OnClientClick="javascript:return checkvalidations(this.id);"
                                                                                                class="grey2_btn" CommandArgument='<%#Eval("Supplierid")%>' runat="server"><span>Save Information</span></asp:LinkButton>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="pp_details clearfix" style="width: 371px;">
                                                                                <a href="#" id="closepopup" runat="server" class="pp_close">Close</a>
                                                                                <p class="pp_description" style="display: none;">
                                                                                </p>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="pp_bottom" style="">
                                                            <div class="pp_left" style="">
                                                            </div>
                                                            <div class="pp_middle" style="">
                                                            </div>
                                                            <div class="pp_right" style="">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <div style="text-align: center">
                                            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="spacer15">
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
            </div>
        </div>
</asp:Content>