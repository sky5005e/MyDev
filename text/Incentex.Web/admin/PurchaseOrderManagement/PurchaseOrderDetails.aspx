<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="PurchaseOrderDetails.aspx.cs" Inherits="PurchaseOrderDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link media="screen" rel="stylesheet" href="../../CSS/colorbox.css" />
    <link href="../../CSS/red_colorbox.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .label-sel
        {
            width: 95%;
        }
        .dd2
        {
            width: 202px !important;
        }
        .dd2 .ddChild
        {
            width: 200px !important;
        }
        .order_detail td
        {
            font-size: 12px;
        }
        .order_detail label
        {
            padding-left: 0px;
        }
        .collapsibleContainerTitle div
        {
            color: #FFFFFF;
            font-weight: normal;
        }
        .collapsibleContainerSubInnerTitle
        {
            color: darkgray;
            font-weight: normal;
        }
        .collapsibleContainerContent
        {
            padding: 0px;
        }
        .form_box span.error
        {
            margin-left: 0px;
        }
        .form_table span.error
        {
            margin-top: -10px;
        }
        .collapsibleInnerContainer .collapsibleInnerContainer .collapsibleContainerInnerTitle div, .collapsibleSubInnerContainer, .collapsibleSubInnerContainer .collapsibleContainerSubInnerTitle div
        {
            font-size: 11px;
        }
        .collapsibleInnerContainer, .collapsibleSubInnerContainer, .collapsibleContainerSubInnerContent
        {
            margin: 10px 10px 0px 10px;
        }
        .textarea_box
        {
            width: 100%;
        }
        .popuplabel
        {
            color: #FFFFFF;
            font-size: 16px;
            margin: 5px;
            text-align: center;
        }
        .orderreturn_box img
        {
            height: 22px;
            margin-bottom: 4px;
        }
        .textarea_box textarea
        {
            font-size: 11px;
        }
        a.grey2_btn
        {
            font-size: 12px !important;
        }
        a.grey2_btn span
        {
            width: 83%;
            line-height: 38px;
        }
        a.grey2_btn img
        {
            border: medium none;
            float: left;
            height: 33px;
            margin: 0;
            padding: 0;
            vertical-align: middle;
            width: 47px;
        }
        .hyperlink
        {
            text-decoration: none;
            color: #ffffff;
        }
        .rightalian
        {
            margin-right: -400px;
        }
        .select_box_pad
        {
            margin: -25px auto 0 !important;
        }
        .ui-datepicker-trigger
        {
            position: absolute !important;
            right: -38px !important;
            top: 0 !important;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
        function SetDropBackGround() {
            setTimeout(function() {
                $(".slc").css("background", "url('../../images/redarrow.gif') no-repeat scroll 98% 5px transparent");
            }, 500);
        }

        //        window.onbeforeunload = function(e) {
        //            var res = $("#ctl00_ContentPlaceHolder1_hdnIsFillUpfollowup").val();
        //            if (res == 'True')
        //                alert(res);
        //            else
        //                window.onbeforeunload = null;
        //        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleInnerContainer").collapsibleInnerPanel();
            $(".collapsibleSubInnerContainer").collapsibleSubInnerPanel();
            $(".collapsibleContainerContent").hide();
            $(".collapsibleContainerInnerContent").hide();
            $(".collapsibleContainerSubInnerContent").hide();

            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtMessage: { required: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtMessage: { required: replaceMessageString(objValMsg, "Required", "message") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlorderstatus")
                            error.insertAfter("#dvOrderStatus");
                        else
                            error.insertAfter(element);
                    }
                });
            });

            $(".showloader").click(function() {
                $('#dvLoader').show();
            });

            $("#ctl00_ContentPlaceHolder1_lnkbtnSubmitNote").click(function() {
                $('#dvLoader').show();
                $("#ctl00_ContentPlaceHolder1_txtMessage").rules("add", {
                    required: true,
                    messages: { required: replaceMessageString(objValMsg, "Required", "message") }
                });

                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });

            $("#ctl00_ContentPlaceHolder1_lnkbtnSubmitfollowup").click(function() {
                $('#dvLoader').show();
                addrules();
                $("#ctl00_ContentPlaceHolder1_txtMessage").rules("remove");
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });

        });

        function addrules() {
            $("#ctl00_ContentPlaceHolder1_txtconfirmdedate").rules("add", {
                required: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "delivery date") }
            });
            $("#ctl00_ContentPlaceHolder1_ddlorderstatus").rules("add", {
                NotequalTo: 0,
                messages: { required: replaceMessageString(objValMsg, "NotEqualTo", "order status") }
            });
            $("#ctl00_ContentPlaceHolder1_txtnextfollowupdate").rules("add", {
                required: true,
                messages: { required: replaceMessageString(objValMsg, "Required", "next follow up date") }
            });
        }

        //        $(function() {
        //            $(".datepicker1").datepicker({
        //                buttonText: 'DatePicker',
        //                showOn: 'button',
        //                buttonImage: '../../images/calender-icon.jpg',
        //                buttonImageOnly: true
        //            });
        //        });

        $(function() {
            scrolltextarea(".scrollme", "#Scrolltop", "#ScrollBottom");
        });

        $("#ctl00_ContentPlaceHolder1_ddlStatus").change(function() {
            $('#dvLoader').show();
        });

        $("#ctl00_ContentPlaceHolder1_txtDatePromised").change(function() {
            $('#dvLoader').show();
        });

        $("#ctl00_ContentPlaceHolder1_txtDatePromised").click(function() {
            $(this).select();
        });

        $("#ctl00_ContentPlaceHolder1_txtDatePromised").keypress(function(e) {
            var keyCode = e.keyCode || e.which;
            if (keyCode == 46) {
                return true;
            }
            else {
                return false;
            }
        });

        $(function() {
            $(".datepicker1").datepicker({
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true,
                minDate: new Date()
            });
        });

        
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function ImageButton() {
            var purchaseOrderID = $("#ctl00_ContentPlaceHolder1_hdnPurchaseOrderID").val();
            var IsTodayFollowup = $("#ctl00_ContentPlaceHolder1_hdnIsTodayFollowup").val();
            var IsFillUpfollowup = $("#ctl00_ContentPlaceHolder1_hdnIsFillUpfollowup").val();
            var SearchVendorID = $("#ctl00_ContentPlaceHolder1_hdnSerVendorID").val();
            var SearchOrderNumber = $("#ctl00_ContentPlaceHolder1_hdnSerOrderNumber").val();
            var SearchMasterItemID = $("#ctl00_ContentPlaceHolder1_hdnSerMasterItemID").val();

            var siteurl = '<%=ConfigurationSettings.AppSettings["siteurl"].ToString()%>';

            if (IsFillUpfollowup == 'True') {
                window.location = siteurl + "admin/PurchaseOrderManagement/PurchaseOrderDetails.aspx?PurchaseOrderID=" + purchaseOrderID + "&IsTodayFollowup=" + IsTodayFollowup + "&IsFillUpfollowup=" + IsFillUpfollowup + "&vendorID=" + SearchVendorID + "&MasterItemID=" + SearchMasterItemID + "&OrderNumber=" + SearchOrderNumber;
            }
            else {
                window.location = siteurl + "admin/index.aspx";
            }
        }

        function OnLogoutClick() {
            var purchaseOrderID = $("#ctl00_ContentPlaceHolder1_hdnPurchaseOrderID").val();
            var IsTodayFollowup = $("#ctl00_ContentPlaceHolder1_hdnIsTodayFollowup").val();
            var IsFillUpfollowup = $("#ctl00_ContentPlaceHolder1_hdnIsFillUpfollowup").val();
            var SearchVendorID = $("#ctl00_ContentPlaceHolder1_hdnSerVendorID").val();
            var SearchOrderNumber = $("#ctl00_ContentPlaceHolder1_hdnSerOrderNumber").val();
            var SearchMasterItemID = $("#ctl00_ContentPlaceHolder1_hdnSerMasterItemID").val();

            var siteurl = '<%=ConfigurationSettings.AppSettings["siteurl"].ToString()%>';

            if (IsFillUpfollowup == 'True') {
                window.location = siteurl + "admin/PurchaseOrderManagement/PurchaseOrderDetails.aspx?PurchaseOrderID=" + purchaseOrderID + "&IsTodayFollowup=" + IsTodayFollowup + "&IsFillUpfollowup=" + IsFillUpfollowup + "&vendorID=" + SearchVendorID + "&MasterItemID=" + SearchMasterItemID + "&OrderNumber=" + SearchOrderNumber;
                return false;
            }
            else {
                return true;
            }
        } 
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="black_top_co">
        <span>&nbsp;</span></div>
    <div class="black_middle" style="text-align: center;">
        <span id="spnlogintype" runat="server" style="display: none"></span>
        <div style="text-align: center;">
            <asp:UpdatePanel ID="upMsg" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="spacer10">
        </div>
        <asp:HiddenField ID="hdnIsFillUpfollowup" runat="server" />
        <asp:HiddenField ID="hdnIsTodayFollowup" runat="server" />
        <asp:HiddenField ID="hdnSerVendorID" runat="server" />
        <asp:HiddenField ID="hdnSerMasterItemID" runat="server" />
        <asp:HiddenField ID="hdnSerOrderNumber" runat="server" />
        <table class="order_detail">
            <tr>
                <td width="50%">
                    <table>
                        <tr>
                            <td>
                                <asp:HiddenField ID="hdnPurchaseOrderID" runat="server" />
                                <label>
                                    Vendor Name :</label>
                                <asp:Label runat="server" ID="lblvendorname"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Vendor Company Name :
                                </label>
                                <asp:Label runat="server" ID="lblvendorcompanyname"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="50%">
                    <table>
                        <tr>
                            <td>
                                <label>
                                    Confirmed Delivery Date :
                                </label>
                                <%-- <asp:Label ID="lblDeliveryDate" runat="server"></asp:Label>--%>
                                <div class="form_table select_box_pad" style="width: 145px;">
                                    <div class="calender_l">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <asp:TextBox ID="txtDatePromised" runat="server" Style="width: 80px;" CssClass="datepicker1"
                                                OnTextChanged="txtDatePromised_TextChanged" AutoPostBack="true" AutoCompleteType="None"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="btnViewPurchaseOrder" runat="server" Text="View Purchase Order #"
                                    OnClick="btnViewPurchaseOrder_Click"></asp:LinkButton>
                                <asp:HiddenField ID="hdnFileName" runat="server" />
                                <asp:HiddenField ID="hdnOriginalFileName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Purchase Order # :
                                </label>
                                <asp:Label ID="lblOrderNumber" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Current Status :</label>
                                <asp:DropDownList runat="server" ID="ddlStatus" Style="background-color: #303030;
                                    border: medium none; color: #ffffff; width: 150px; padding: 2px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                </asp:DropDownList>
                                <div class="form_table centeralign" style="margin: 10px 10px 0; padding-right: 10px;
                                    visibility: hidden">
                                    <asp:LinkButton ID="LinkButton1" class="grey2_btn showloader" runat="server" OnClick="lnkbtnfollowup_Click"
                                        Width="143px">
                                      <span>FoolowUp</span></asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="spacer10">
        </div>
        <div class="collapsibleInnerContainer" title="Item Listing" align="left">
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
                            <asp:Label runat="server" ID="lblProductId" Text='<%# Eval("ProductId") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="3%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="MasterStyleName">
                        <HeaderTemplate>
                            <span>Master #</span>
                            <asp:PlaceHolder ID="placeholderMasterStyleName" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="first">
                                <asp:Label ID="hypStyle" CommandName="Edit" runat="server" ToolTip='<%# Eval("MasterStyleName")%>'><%# (Eval("MasterStyleName").ToString().Length > 20) ? Eval("MasterStyleName").ToString().Substring(0, 20) + "..." : Eval("MasterStyleName") + "&nbsp;"%></asp:Label>
                            </div>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ProductStyle">
                        <HeaderTemplate>
                            <span>Style</span>
                            <%--<asp:LinkButton ID="lnkbtnStyle" runat="server" CommandArgument="ProductStyle" CommandName="Sort"><span>Style</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderStyle" runat="server"></asp:PlaceHolder>--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblStyle" Text='<%# Eval("ProductStyle") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="18%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ItemNumber">
                        <HeaderTemplate>
                            <span>Item #</span>
                            <%--<asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                CommandName="Sort"> <span >Item #</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                            <asp:HiddenField ID="hdnItemNumber" runat="server" Value='<%# Eval("ItemNumber") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="17%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ItemColor">
                        <HeaderTemplate>
                            <span>Color</span>
                            <%--<asp:LinkButton ID="lnkbtnItemColor" runat="server" CommandArgument="ItemColor" CommandName="Sort"><span>Color</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderItemColor" runat="server"></asp:PlaceHolder>--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblItemColor" ToolTip='<%# Eval("ItemColor")%>' Text='<%# (Eval("ItemColor").ToString().Length > 8) ? Eval("ItemColor").ToString().Substring(0, 8) + "..." : Eval("ItemColor") + "&nbsp;"%>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="9%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ItemSize">
                        <HeaderTemplate>
                            <span>Size</span>
                            <%--<asp:LinkButton ID="lnkbtnItemSize" runat="server" CommandArgument="ItemSize" CommandName="Sort"><span>Size</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderItemSize" runat="server"></asp:PlaceHolder>--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblItemSize" Text='<%# Eval("ItemSize") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="6%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ItemQuantity">
                        <HeaderTemplate>
                            <span>Quantity</span>
                            <%-- <asp:LinkButton ID="lnkbtnQuantity" runat="server" CommandArgument="ItemQuantity"
                                CommandName="Sort"><span>Quantity</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderItemQuantity" runat="server"></asp:PlaceHolder>--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblQuantity" Text='<%# Eval("ItemQuantity") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box centeralign" Width="7%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ItemPrice">
                        <HeaderTemplate>
                            <span>Price</span>
                            <%--<asp:LinkButton ID="lnkbtnPrice" runat="server" CommandArgument="ItemPrice" CommandName="Sort"><span>Price</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderPrice" runat="server"></asp:PlaceHolder>--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblPrice" Text='<%# Eval("ItemPrice") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box centeralign" Width="7%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="spacer10">
        </div>
        <div class="collapsibleInnerContainer" title="Follow Up Timeline" align="left">
            <asp:GridView ID="gvFollowUpTimeline" runat="server" AutoGenerateColumns="false"
                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                RowStyle-CssClass="ord_content">
                <Columns>
                    <asp:TemplateField SortExpression="LastFollowUpDate&Time">
                        <HeaderTemplate>
                            <span>Last Follow Up Date</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="first">
                                <asp:Label ID="lbllastfolloeupdate" runat="server"><%# Eval("LastFollowUpDate")%></asp:Label>
                            </div>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="FollowUpBy">
                        <HeaderTemplate>
                            <span>Follow Up By</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblStyle" Text='<%# Eval("FollowUpBy") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="PriorStatus">
                        <HeaderTemplate>
                            <span>Prior Status</span></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblPriorStatus" Text='<%# Eval("PriorStatus") + "&nbsp;"%>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="CurrentStatus">
                        <HeaderTemplate>
                            <span>Current Status</span></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblItemColor" Text='<%# Eval("CurrentStatus") + "&nbsp;"%>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="25%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="spacer10">
        </div>
        <%--Start General Notes Area--%>
        <div class="collapsibleInnerContainer" title="General Purchase Order Notes" align="left">
            <%--Start display note sent by IE to Vendor--%>
            <div class="collapsibleSubInnerContainer" title="Notes from IE to Vendor:" align="left">
                <div class="form_table">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box taxt_area clearfix" style="height: 200px">
                        <div class="textarea_box alignright">
                            <div class="scrollbar" style="height: 200px">
                                <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                </a>
                            </div>
                            <asp:TextBox ID="txtNotesForVendor" runat="server" TextMode="MultiLine" CssClass="scrollme"
                                Height="197px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
            </div>
            <%--End display note sent by IE to Vendor--%>
            <div class="spacer10">
            </div>
            <%--Start display IE internal notes--%>
            <div class="collapsibleSubInnerContainer" title="Internal IE Notes:" align="left">
                <div class="form_table">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box taxt_area clearfix" style="height: 200px">
                        <div class="textarea_box alignright">
                            <div class="scrollbar" style="height: 200px">
                                <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                </a>
                            </div>
                            <asp:TextBox ID="txtIEInternalNotes" runat="server" TextMode="MultiLine" CssClass="scrollme"
                                Height="197px" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
            </div>
            <%--End display IE internal notes--%>
            <div class="spacer10">
            </div>
            <%--Start Button for sending notes--%>
            <div class="form_table centeralign" style="margin: 10px 10px 0; padding-right: 10px">
                <asp:LinkButton ID="lnkbtnVendorMessage" class="grey2_btn rightalian showloader"
                    runat="server" OnClick="lnkbtnVendorMessage_Click" Width="143px">
                    <img src="../Incentex_Used_Icons/system-email-templates.png"  /><span>Vendor Message</span></asp:LinkButton>
                <asp:LinkButton ID="lnkbtnIncentexEmployeeMessage" class="grey2_btn alignright" runat="server"
                    OnClick="lnkbtnIncentexEmployeeMessage_Click" Width="222px" Style="margin-left: 40px;">
                    <img src="../Incentex_Used_Icons/system-email-templates.png" /><span>Incentex Employee Message</span></asp:LinkButton>
                <asp:HiddenField runat="server" ID="hdnVendorID" />
            </div>
            <%--End Button for sending notes--%>
        </div>
        <%--End General Notes Area--%>
    </div>
    <div class="black_bot_co">
        <span>&nbsp;</span></div>
    <div class="spacer15">
    </div>
    <asp:LinkButton ID="lnkDummyForOrderNote" class="grey2_btn alignright" runat="server"
        Style="display: none"></asp:LinkButton>
    <at:ModalPopupExtender ID="mpeOrderNote" TargetControlID="lnkDummyForOrderNote" BackgroundCssClass="modalBackground"
        CancelControlID="cboxClose" DropShadow="true" runat="server" PopupControlID="pnlOrderNote">
    </at:ModalPopupExtender>
    <asp:HiddenField runat="server" ID="hdnNoteType" />
    <asp:HiddenField runat="server" ID="hdnOrderID" />
    <asp:HiddenField runat="server" ID="hdnWorkGroupID" />
    <asp:Panel ID="pnlOrderNote" runat="server" Style="display: none;">
        <div class="cboxWrapper" style="display: block; width: 450px; position: fixed; height: 600px;
            left: 35%; top: 8%;">
            <div style="">
                <div id="cboxTopLeft" style="float: left;">
                </div>
                <div id="cboxTopCenter" style="float: left; width: 400px;">
                </div>
                <div id="cboxTopRight" style="float: left;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxMiddleLeft" style="float: left; height: 550px;">
                </div>
                <div id="cboxContent" style="float: left; display: block; height: 550px; width: 400px;">
                    <div id="cboxLoadedContent" style="display: block; margin: 0;">
                        <div id="cboxClose">
                            close</div>
                        <div style="margin-top: 30px;">
                            <table class="form_table" cellpadding="0" cellspacing="0">
                                <tr runat="server" id="trIE" class="checktable_supplier true">
                                    <td>
                                        <span>Incentex Employee</span>
                                        <div class="spacer5">
                                        </div>
                                        <div style="overflow: auto; height: 185px;">
                                            <asp:DataList ID="dtIE" runat="server" RepeatColumns="3" RepeatDirection="Vertical">
                                                <ItemTemplate>
                                                    <span class="custom-checkbox alignleft" id="spnUser" runat="server">
                                                        <asp:CheckBox ID="chkUser" runat="server" />
                                                    </span>
                                                    <label>
                                                        <asp:Label ID="lblUserName" Text='<%# Eval("EmployeeName") %>' runat="server"></asp:Label></label>
                                                    <asp:HiddenField ID="hdnUserID" runat="server" Value='<%#Eval("UserInfoID")%>' />
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                    </td>
                                </tr>
                                <tr runat="server" id="trEmailAddress">
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlEmailAddress">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label" style="width: 20%;">Email Address</span> <span style="width: 74%;">
                                                        <asp:Label ID="lblEmailAddress" Text="" runat="server"></asp:Label></span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box taxt_area clearfix" style="height: 150px">
                                            <span class="input_label alignleft" style="height: 150px; line-height: 130px;">Message</span>
                                            <div class="textarea_box alignright" style="width: 78%;">
                                                <div class="scrollbar" style="height: 150px">
                                                    <a href="#scroll" id="scrolltop" class="scrolltop"></a><a href="#scroll" id="scrollbottom"
                                                        class="scrollbottom"></a>
                                                </div>
                                                <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" CssClass="scrollme"
                                                    Height="147px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </td>
                                </tr>
                                <tr runat="server" id="trIsEmailSend" class="checktable_supplier true">
                                    <td>
                                        <span class="custom-checkbox" id="spnIsEmailSend" runat="server" style="float: left;">
                                            <asp:CheckBox ID="chkIsEmailSend" runat="server" />
                                        </span>
                                        <label>
                                            <asp:Label ID="lblIsEmailSend" Text="Send Email" runat="server"></asp:Label></label>
                                    </td>
                                </tr>
                            </table>
                            <div class="centeralign">
                                <asp:LinkButton ID="lnkbtnSubmitNote" class="grey2_btn" runat="server" OnClick="lnkbtnSubmitNote_Click"><span>Send</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="cboxMiddleRight" style="float: left; height: 550px;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxBottomLeft" style="float: left;">
                </div>
                <div id="cboxBottomCenter" style="float: left; width: 400px;">
                </div>
                <div id="cboxBottomRight" style="float: left;">
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlfollowup" runat="server" Style="display: none;">
        <div class="cboxWrapper" style="display: block; width: 450px; position: fixed; height: 400px;
            left: 35%; top: 8%;">
            <div style="">
                <div id="cboxTopLeft" style="float: left;">
                </div>
                <div id="cboxTopCenter" style="float: left; width: 400px;">
                </div>
                <div id="cboxTopRight" style="float: left;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxMiddleLeft" style="float: left; height: 350px;">
                </div>
                <div id="cboxContent" style="float: left; display: block; height: 350px; width: 400px;">
                    <div id="cboxLoadedContent" style="display: block; margin: 0; padding-right: 40px;">
                        <div id="cboxpasswordClose">
                            close</div>
                        <div class="spacer15">
                        </div>
                        <div style="margin-top: 30px;">
                            <table class="form_table" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="text-align: center; font-size: 17px;">
                                        <span>Today’s follow up details</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 42%;">Confirmed Delivery Date </span>
                                            <asp:TextBox ID="txtconfirmdedate" runat="server" Style="width: 80px;" CssClass="datepicker1"
                                                AutoCompleteType="None"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 42%;">Purchase Order Status</span><span class="custom-sel label_sel">
                                                <asp:DropDownList ID="ddlorderstatus" runat="server" onchange="pageLoad(this,value);">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvOrderStatus">
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="width: 42%;">Next Follow Up Date </span>
                                            <asp:TextBox ID="txtnextfollowupdate" runat="server" Style="width: 80px;" CssClass="datepicker1"
                                                AutoCompleteType="None"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="input_label" style="width: 42%;">Show on Tomorrows Report :</span>
                                        <div class="checkout_checkbox alignlef" runat="server" id="spnUPStoclient" style="width: 20px;
                                            height: 25px; margin-left: 150px; margin-top: -31px; padding-bottom: 15px;">
                                            <asp:CheckBox ID="chkshoonTomorrow" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="centeralign">
                                <asp:LinkButton ID="lnkbtnSubmitfollowup" class="grey2_btn" runat="server" OnClick="lnkbtnSubmitfollowup_Click"><span>Save</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="cboxMiddleRight" style="float: left; height: 350px;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxBottomLeft" style="float: left;">
                </div>
                <div id="cboxBottomCenter" style="float: left; width: 400px;">
                </div>
                <div id="cboxBottomRight" style="float: left;">
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
