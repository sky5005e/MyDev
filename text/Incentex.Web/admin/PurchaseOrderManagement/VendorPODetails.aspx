<%@ Page Title="Vendor PO Details" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="VendorPODetails.aspx.cs" Inherits="admin_PurchaseOrderManagement_VendorPODetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleContainerContent").hide();

        });   
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }

        // Let's use a lowercase function name to keep with JavaScript conventions
        function selectAll(invoker) {
            // Since ASP.NET checkboxes are really HTML input elements
            //  let's get all the inputs
            var inputElements = document.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                // Filter through the input types looking for checkboxes
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        } 
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="sc1" runat="server">
    </at:ToolkitScriptManager>
    <div class="spacer10">
    </div>
    <table class="order_detail">
        <tr>
            <td style="width: 5%;">
            </td>
            <td style="width: 50%; font-size: small;">
                Customer Information :
                <asp:Label runat="server" ID="lblVendorName"></asp:Label>
            </td>
            <td style="font-size: small; float: right">
                Expected Delivery :
            </td>
            <td style="font-size: small; padding-left: 10px">
                <asp:Label runat="server" ID="lblExpDelivery"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 50%;" colspan="2">
            </td>
            <td style="font-size: small; float: right">
                Start Production :
            </td>
            <td style="font-size: small; padding-left: 10px">
                <asp:Label runat="server" ID="lblStartPro"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 50%;" colspan="2">
            </td>
            <td style="font-size: small; float: right">
                Confirmed Delivery :
            </td>
            <td style="font-size: small; padding-left: 10px">
                <asp:Label runat="server" ID="lblConDelivery"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 50%;" colspan="2">
            </td>
            <td style="font-size: small; float: right">
                View Purchase Order :
            </td>
            <td style="font-size: small; padding-left: 10px">
                <asp:Label runat="server" ID="lblViewPO"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 50%;" colspan="2">
            </td>
            <td style="font-size: small; float: right">
                Current Status :
            </td>
            <td style="font-size: small; padding-left: 10px">
                <asp:DropDownList ID="ddlStatus" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <div class="spacer10">
    </div>
    <div style="text-align: center">
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
    </div>
    <div class="spacer20">
    </div>
    <div class="collapsibleContainer" title="Blank/Final Goods Details :" align="left">
        <div class="form_table">
            <asp:GridView ID="gvItemDetails" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
                <Columns>
                    <asp:TemplateField Visible="False" HeaderText="Id">
                        <HeaderTemplate>
                            <span>ProductItemID</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblProductItemID" Text='<%# Eval("ProductItemID") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="2%" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="False" HeaderText="Id">
                        <HeaderTemplate>
                            <span>Product ID</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblProductId" Text='<%# Eval("PurchaseOrderID") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="3%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Check">
                        <HeaderTemplate>
                            <span>
                                <asp:CheckBox ID="cbSelectAll" runat="server" OnClick="selectAll(this)" />&nbsp;</span>
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="centeralign" />
                        <ItemTemplate>
                            <span class="first">
                                <asp:CheckBox ID="chkItem" runat="server" />&nbsp; </span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="5%" CssClass="b_box centeralign" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="MasterStyleName">
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnMasterStyleName" runat="server" CommandArgument="MasterStyleName"
                                    CommandName="Sort">Master #</asp:LinkButton>
                            </span>
                            <asp:PlaceHolder ID="placeholderMasterStyleName" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span>
                                <asp:LinkButton ID="hypStyle" CommandName="Edit" runat="server" ToolTip='<%# Eval("MasterStyle")%>'><%# (Eval("MasterStyle").ToString().Length > 20) ? Eval("MasterStyle").ToString().Substring(0, 20) + "..." : Eval("MasterStyle") + "&nbsp;"%></asp:LinkButton>
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ProductStyle">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnStyle" runat="server" CommandArgument="ProductStyle" CommandName="Sort"><span>Style</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderStyle" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblStyle" Text='<%# Eval("ProductStyle") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="18%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ItemNumber">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                CommandName="Sort"> <span >Item #</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                            <asp:HiddenField ID="hdnItemNumber" runat="server" Value='<%# Eval("ItemNumber") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="17%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ItemColor">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnItemColor" runat="server" CommandArgument="ItemColor" CommandName="Sort"><span>Color</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderItemColor" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblItemColor" ToolTip='<%# Eval("ItemColor")%>' Text='<%# (Eval("ItemColor").ToString().Length > 8) ? Eval("ItemColor").ToString().Substring(0, 8) + "..." : Eval("ItemColor") + "&nbsp;"%>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="9%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ItemSize">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnItemSize" runat="server" CommandArgument="ItemSize" CommandName="Sort"><span>Size</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderItemSize" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblItemSize" Text='<%# Eval("ItemSize") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="6%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ItemQuantity">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnQuantity" runat="server" CommandArgument="ItemQuantity"
                                CommandName="Sort"><span>Quantity</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderItemQuantity" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblItemQuantity" Text='<%# Eval("Quantity") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box centeralign" Width="7%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ItemPrice">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnPrice" runat="server" CommandArgument="ItemPrice" CommandName="Sort"><span>Price</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderPrice" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblItemPrice" Text='<%# String.Format("{0:0,0.00}",Eval("Price")) %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box centeralign" Width="7%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ItemCount">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnCount" runat="server" CommandArgument="ItemCount" CommandName="Sort"><span>Count</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderItemCount" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="btn_space">
                                <asp:TextBox runat="server" ID="txtfinalamount" Text='<%#Eval("FinalQty")%>' Style="background-color: #303030;
                                    border: medium none; color: #FFFFFF; padding: 2px; width: 100%; text-align: center;" /></span>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box centeralign" Width="7%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="spacer10">
            </div>
            <div class="alignright" id="dvBtn" runat="server">
                <asp:LinkButton ID="lnkSubmit" runat="server" class="grey_btn" OnClick="lnkSubmit_Click"><span>Submit</span></asp:LinkButton>
            </div>
            <div style="clear: both;">
            </div>
        </div>
    </div>
    <div class="spacer20">
    </div>
    <div class="collapsibleContainer" title="Artwork Details :" align="left">
        <div class="form_table">
        </div>
    </div>
    <div class="spacer20">
    </div>
    <div class="collapsibleContainer" title="Post Note to Incentex Employee :" align="left">
        <div class="form_table">
            <div>
                <div class="form_table">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box taxt_area clearfix" style="height: 100px">
                        <span class="input_label alignleft" style="height: 100px">Incentex Internal Notes Only</span>
                        <div class="textarea_box alignright">
                            <div class="scrollbar" style="height: 103px">
                                <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                </a>
                            </div>
                            <asp:TextBox ID="txtOrderNotesForIE" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                Height="100px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="alignnone spacer15">
                </div>
                <div class="rightalign gallery" id="div1" runat="server">
                    <asp:LinkButton ID="lnkDummyAddNewIE" class="grey2_btn alignright" runat="server"
                        Style="display: none"></asp:LinkButton>
                    <asp:LinkButton ID="lnkAddNewIE" CommandName="AddNotes" runat="server" CssClass="grey2_btn alignright"><span>Post Notes to Incentex Employee</span></asp:LinkButton>
                    <at:ModalPopupExtender ID="modalAddnotesIE" TargetControlID="lnkAddNewIE" BackgroundCssClass="modalBackground"
                        DropShadow="true" runat="server" PopupControlID="pnlNotesIE" CancelControlID="A5">
                    </at:ModalPopupExtender>
                </div>
                <div>
                    <asp:Panel ID="pnlNotesIE" runat="server">
                        <div class="pp_pic_holder facebook" style="display: block; width: 411px;">
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
                                        <div class="pp_content" style="height: 45px; display: block;">
                                            <div class="pp_fade" style="display: block;">
                                                <span class="noteIncentex" style="width: 80%; font-size: 12px; background-color: inherit;
                                                    color: Black; font-weight: bold;">
                                                    <img src="../Images/errorpage.png" height="25px" width="25px" alt="note:" />&nbsp;&nbsp;
                                                    You are about to post an Incentex Internal Note. </span>
                                                <div class="pp_details clearfix" style="width: 371px;">
                                                    <a href="#" id="A5" runat="server" class="pp_close">Close</a>
                                                    <p class="pp_description" style="display: none;">
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="pp_content_container" style="">
                                <div class="pp_left" style="">
                                    <div class="pp_right" style="">
                                        <div class="pp_content" style="height: 228px; display: block;">
                                            <div class="pp_loaderIcon" style="display: none;">
                                            </div>
                                            <div class="pp_fade" style="display: block;">
                                                <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                                <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                                    <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                        style="visibility: visible;">previous</a>
                                                </div>
                                                <div id="Div2">
                                                    <div class="pp_inline clearfix">
                                                        <div class="form_popup_box" style="padding-top: 15px;">
                                                            <div class="label_bar">
                                                                <span>Incentex Employee Notes/History :
                                                                    <br />
                                                                    <br />
                                                                    <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtNoteIE" runat="server"></asp:TextBox></span>
                                                            </div>
                                                            <div>
                                                                <asp:LinkButton ID="lnkNoteHisForIE" runat="server" CommandName="SAVECACE" class="grey2_btn"
                                                                    OnClientClick="return CheckNoteHistory()" OnClick="lnkNoteHisForIE_Click"><span>Save Notes</span></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
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
                <br />
                <br />
                <br />
                <%--End Note History--%>
            </div>
        </div>
    </div>
</asp:Content>
