<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="SummaryOrderCompanyViewReport.aspx.cs" Inherits="admin_Report_SummaryOrderCompanyViewReport"
    Title="Summary Order Company View Report" %>

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
    </style>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
        function SetDropBackGround()
        {
            setTimeout(function() {
                $(".slc").css("background","url('../../images/redarrow.gif') no-repeat scroll 98% 5px transparent");
            },500);
        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleInnerContainer").collapsibleInnerPanel();
            $(".collapsibleSubInnerContainer").collapsibleSubInnerPanel();
            
            $(".collapsibleContainerContent").hide();
            $(".collapsibleContainerInnerContent").hide();
            $(".collapsibleContainerSubInnerContent").hide();
            $('.companyname').hide();
            
            CheckForOpenPanel();
            
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
                        error.insertAfter(element);
                    }
                });
            });
            
            $(".showloader").click(function(){
                $('#dvLoader').show();
            });

            $("#ctl00_ContentPlaceHolder1_lnkbtnSubmitNote").click(function() {
                $('#dvLoader').show();
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });
        });
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
        $(function() {
            scrolltextarea(".scrollme", "#Scrolltop", "#ScrollBottom");
        });
        
        function CallFunctionWhenClick(ItemID,CtrlID)
        {
                if(ItemID.toString().length == 1)
                    ItemID = "0" + ItemID;
                if(!$("#" + CtrlID).hasClass("setopen"))
                {
                    $("#<%= hfDivID.ClientID %>").val(CtrlID);
                    $("#<%= hfToggleStatus.ClientID %>").val("true");
                    $('#dvLoader').show();
                    __doPostBack("ctl00$ContentPlaceHolder1$rpCompany$ctl" + ItemID + "$lnkSelect",'');
                }
                else
                {
                    $("#" + CtrlID).removeClass("setopen");
                    $("#<%= hfDivID.ClientID %>").val("");
                    $("#<%= hfToggleStatus.ClientID %>").val("false");
                    $("#dvLoader").hide();
                }
        }
        function CheckForOpenPanel()
        {
            var CurrDivID = $("#<%= hfDivID.ClientID %>").val();
            var ToggleStatus = $("#<%= hfToggleStatus.ClientID %>").val();
            if(CurrDivID != undefined && CurrDivID != "")
            {
                $("#" + CurrDivID).addClass("setopen");
                $(".setopen").find(".collapsibleContainerContent").slideToggle(ToggleStatus);
            }
        }
        function ChildDiv(e)
        {
            //alert("ChildDiv" + e);
            e.stopPropagation();
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:HiddenField ID="hfDivID" runat="server" />
    <asp:HiddenField ID="hfToggleStatus" runat="server" />
    <asp:HiddenField ID="hfTempCompanyID" runat="server" />
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <asp:Repeater ID="rpCompany" runat="server" OnItemDataBound="rpCompany_ItemDataBound">
            <ItemTemplate>
                <div class="collapsibleContainer MainContainer" title='<%# Eval("CompanyName") %>'
                    id="divHead" runat="server">
                    <div class="black_top_co">
                        <span>&nbsp;</span></div>
                    <div class="black_middle">
                        <asp:LinkButton ID="lnkSelect" runat="server" CommandArgument='<%# Eval("CompanyID") %>'
                            CommandName="CompanyName" CssClass="companyname" OnClick="lnkSelect_Click"></asp:LinkButton>
                        <!-- Main Body Content from the Summary Order View Page -->
                        <div style="text-align: right; margin: 10px 0px;">
                            Record Count:
                            <asp:Label runat="server" ID="lblCompanyOrderCounter" Text="0"></asp:Label>
                            <asp:HiddenField runat="server" ID="hfCompanyID" Value='<%# Eval("CompanyID") %>' />
                        </div>
                        <asp:Repeater ID="rptSummaryOrderView" runat="server" OnItemCommand="rptSummaryOrderView_ItemCommand">
                            <ItemTemplate>
                                <div class="black_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="black_middle childDiv" style="text-align: center;" onclick="ChildDiv(event)">
                                    <table class="order_detail">
                                        <tr>
                                            <td width="50%">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                Order Number:
                                                            </label>
                                                            <asp:HyperLink runat="server" ID="hypOrderNumber" Text='<%# Eval("OrderNumber")%>'
                                                                Target="_blank" NavigateUrl='<%# "~/OrderManagement/OrderDetail.aspx?id=" + Eval("OrderID")%>'
                                                                ForeColor="#72757C"></asp:HyperLink>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                Company:
                                                            </label>
                                                            <asp:Label runat="server" ID="lblCompanyName" Text='<%# Eval("CompanyName")%>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                Ordered By:
                                                            </label>
                                                            <asp:Label ID="lblOrderedBy" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="50%">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                Order Date:
                                                            </label>
                                                            <asp:Label ID="lblOrderDate" runat="server" Text='<%# Eval("OrderDate")%>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                Order Status:
                                                            </label>
                                                            <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Eval("OrderStatus")%>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                Number of Days Opened:
                                                            </label>
                                                            <asp:Label ID="lblDaysOpened" runat="server" Text='<%# Eval("DaysOpened")%>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="spacer10">
                                    </div>
                                    <table>
                                        <tr>
                                            <td class="alignleft">
                                                <span>Order Items</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField runat="server" ID="hdnOrderID" Value='<%# Eval("OrderID") %>' />
                                                <asp:GridView ID="grdItemDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <span>Item #</span>
                                                                <div class="corner">
                                                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                                </div>
                                                            </HeaderTemplate>
                                                            <HeaderStyle />
                                                            <ItemTemplate>
                                                                <asp:Label CssClass="first" runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                                                <div class="corner">
                                                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="b_box" Width="20%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <span>Description</span>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="centeralign" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDescription" runat="server" Text='<%#(Eval("Description").ToString().Length > 45) ? Eval("Description").ToString().Substring(0, 45) + "..." : Eval("Description")%>'
                                                                    ToolTip='<%# Eval("Description") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="g_box" Width="35%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <span>Retail</span>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="centeralign" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRetail" Text='<%# Eval("UnitPrice")%>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <span>Qty Ordered</span>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="centeralign" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQtyOrdered" runat="server" Text='<%# Eval("QuantityOrdered") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <span>Qty Stock</span>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="centeralign" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQtyOnHand" runat="server" Text='<%# Eval("Inventory") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <span>Qty Shipped</span>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="centeralign" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQtyShipped" runat="server" Text='<%# Eval("QuantityShipped") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <span>Status</span>
                                                                <div class="corner">
                                                                    <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                                </div>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="centeralign" />
                                                            <ItemTemplate>
                                                                <span class="last">
                                                                    <asp:Image Style="width: 23px;" runat="server" ID="imgStatus" />&nbsp; </span>
                                                                <asp:HiddenField ID="hdnReference" runat="server" Value='<%# Eval("Status") %>' />
                                                                <asp:HiddenField ID="hdnIsDropShipItem" runat="server" Value='<%# Eval("IsDropShipItem") %>' />
                                                                <div class="corner">
                                                                    <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="spacer10">
                                    </div>
                                    <div class="collapsibleContainer" title="Order Communications Center" align="left">
                                        <%--Start display all emails sent for this order--%>
                                        <div class="collapsibleInnerContainer" title="System Auto Generated Email - History"
                                            align="left">
                                            <asp:GridView ID="grdEmailList" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Name</span>
                                                            <div class="corner">
                                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <HeaderStyle />
                                                        <ItemTemplate>
                                                            <asp:Label CssClass="first" runat="server" ID="lblName" Text='<%# Eval("FullName") %>' />
                                                            <asp:HiddenField runat="server" ID="hdnMailID" Value='<%# Eval("MailID") %>' />
                                                            <div class="corner">
                                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                            </div>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box" Width="60%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Date & Time</span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("DateTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box centeralign" Width="25%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>View</span>
                                                            <div class="corner">
                                                                <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnViewEmailDetail" runat="server" CssClass="last" PostBackUrl='<%# "~/admin/CommunicationCenter/ViewTemplates.aspx?mailID=" + Eval("MailID") %>'><span>View Template</span></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <%--End display all emails sent for this order--%>
                                        </div>
                                        <div class="spacer10">
                                        </div>
                                        <%--Start General Notes Area--%>
                                        <div class="collapsibleInnerContainer" title="General Notes" align="left">
                                            <%--Start display note sent by IE to Vendor--%>
                                            <div class="collapsibleSubInnerContainer" title="Notes Sent by IE to Vendor:" align="left">
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
                                            <div class="collapsibleSubInnerContainer" title="Internal Notes for IE:" align="left">
                                                <div class="form_table">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box taxt_area clearfix" style="height: 200px">
                                                        <div class="textarea_box alignright">
                                                            <div class="scrollbar" style="height: 200px">
                                                                <a href="#scroll" id="scrolltop" class="scrolltop"></a><a href="#scroll" id="scrollbottom"
                                                                    class="scrollbottom"></a>
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
                                            <%--Start display Customer Notes--%>
                                            <div class="collapsibleSubInnerContainer" title="Customer Notes:" align="left">
                                                <div class="form_table">
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box taxt_area clearfix" style="height: 200px">
                                                        <div class="textarea_box alignright">
                                                            <div class="scrollbar" style="height: 200px">
                                                                <a href="#scroll" id="A5" class="scrolltop"></a><a href="#scroll" id="A6" class="scrollbottom">
                                                                </a>
                                                            </div>
                                                            <asp:TextBox ID="txtCustomerNotes" runat="server" TextMode="MultiLine" CssClass="scrollme"
                                                                Height="197px" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </div>
                                            </div>
                                            <%--End display Customer Notes--%>
                                            <div class="spacer10">
                                            </div>
                                            <%--Start Button for sending notes--%>
                                            <div class="form_table centeralign" style="margin: 10px 10px 0; padding-right: 10px">
                                                <asp:LinkButton ID="lnkbtnVendorMessage" class="grey2_btn alignleft showloader" runat="server"
                                                    CommandName="Vendor Message" CommandArgument='<%# Eval("OrderID") %>' Width="143px">
                                             <img src="../Incentex_Used_Icons/system-email-templates.png"  /><span>Vendor Message</span></asp:LinkButton>
                                                <asp:LinkButton ID="lnkbtnIncentexEmployeeMessage" class="grey2_btn showloader" runat="server"
                                                    CommandName="Incentex Employee Message" CommandArgument='<%# Eval("OrderID") %>'
                                                    Width="222px" Style="margin-left: 40px;">
                                             <img src="../Incentex_Used_Icons/system-email-templates.png" /><span>Incentex Employee Message</span></asp:LinkButton>
                                                <asp:LinkButton ID="lnkbtnSendCustomerMessage" class="grey2_btn alignright showloader"
                                                    runat="server" CommandName="Send Customer Message" CommandArgument='<%# Eval("OrderID") %>'
                                                    Width="208px" Style="padding-right: 6px;">
                                            <img src="../Incentex_Used_Icons/system-email-templates.png" /> <span>Send Customer Message</span></asp:LinkButton>
                                                <asp:HiddenField runat="server" ID="hfWorkGroupID" Value='<%# Eval("WorkgroupID") %>' />
                                                <asp:HiddenField runat="server" ID="hfCompanyID" Value='<%# Eval("CompanyID") %>' />
                                                <asp:HiddenField runat="server" ID="hfUserID" Value='<%# Eval("UserID") %>' />
                                            </div>
                                            <%--End Button for sending notes--%>
                                        </div>
                                        <%--End General Notes Area--%>
                                        <div class="spacer10">
                                        </div>
                                        <%--Start display Back Order Management--%>
                                        <div class="collapsibleInnerContainer" title="Back Order Management" align="left">
                                            <asp:GridView ID="gvBackOrderManagement" runat="server" AutoGenerateColumns="false"
                                                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                                RowStyle-CssClass="ord_content" Width="100%">
                                                <EmptyDataTemplate>
                                                    <span style="color: Red; text-align: center;">No Records Found</span>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Item Number</span>
                                                            <div class="corner">
                                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <HeaderStyle />
                                                        <ItemTemplate>
                                                            <asp:Label CssClass="first" runat="server" ID="lblBackOrderItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                                            <asp:HiddenField ID="hdnMyShoppingCartiD" runat="server" Value='<%# Eval("MyShoppingCartiD") %>' />
                                                            <div class="corner">
                                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                            </div>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box" Width="25%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Product Description</span>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box leftalign" Width="60%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span>Back Ordered Until</span>
                                                            <div class="corner">
                                                                <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <HeaderStyle CssClass="centeralign" />
                                                        <ItemTemplate>
                                                            <span class="calender_l"">
                                                                <asp:TextBox ID="txtBackOrderDate" Style="background-color: #303030; border: medium none;
                                                                    color: #ffffff; width: 74px; padding: 2px" runat="server" CssClass="cal_w datepicker1 min_w"
                                                                    Text='<%# Convert.ToString(Eval("BackOrderedUntil")) != "" ? Convert.ToDateTime(Eval("BackOrderedUntil")).ToString("MM/dd/yyyy") : "" %>'></asp:TextBox>
                                                                <asp:HiddenField runat="server" ID="hdnBackOrderedUntil" Value='<%# Convert.ToString(Eval("BackOrderedUntil")) != "" ? Convert.ToDateTime(Eval("BackOrderedUntil")).ToString("MM/dd/yyyy") : "" %>' />
                                                            </span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box centeralign" Width="15%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div class="spacer20">
                                            </div>
                                            <table id="back_save_table" runat="server">
                                                <tr>
                                                    <td colspan="3">
                                                        <div class="botbtn centeralign">
                                                            <asp:LinkButton ID="lnkSaveOrderDetails" CommandName="OrderDetails" class="grey2_btn saveShipDetails showloader"
                                                                runat="server" CommandArgument='<%# Eval("OrderID") %>'><span>Save</span></asp:LinkButton>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <%--End display Back Order Management--%>
                                    </div>
                                </div>
                                <div class="black_bot_co">
                                    <span>&nbsp;</span></div>
                                <div class="spacer15">
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <!-- Main Body Content from the Summary Order View Page -->
                    </div>
                    <div class="black_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="spacer25">
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <!-- Black and Red Popups extender, Panel and Body-->
    <asp:LinkButton ID="lnkDummyForOrderNote" class="grey2_btn alignright" runat="server"
        Style="display: none"></asp:LinkButton>
    <at:ModalPopupExtender ID="mpeOrderNote" TargetControlID="lnkDummyForOrderNote" BackgroundCssClass="modalBackground"
        CancelControlID="cboxClose" DropShadow="true" runat="server" PopupControlID="pnlOrderNote">
    </at:ModalPopupExtender>
    <asp:HiddenField runat="server" ID="hdnNoteType" />
    <asp:HiddenField runat="server" ID="hdnOrderID" />
    <asp:HiddenField runat="server" ID="hdnWorkGroupID" />
    <asp:HiddenField runat="server" ID="hdnMainRepeaterItemIndex" />
    <asp:Panel ID="pnlOrderNote" runat="server" Style="display: none;">
        <div class="cboxWrapper" style="display: block; width: 450px; height: 550px; position: fixed;
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
                <div id="cboxMiddleLeft" style="float: left; height: 500px;">
                </div>
                <div id="cboxContent" style="float: left; display: block; height: 500px; width: 400px;">
                    <div id="cboxLoadedContent" style="display: block; margin: 0;">
                        <div id="cboxClose">
                            close</div>
                        <div style="margin-top: 30px;">
                            <table class="form_table" cellpadding="0" cellspacing="0">
                                <tr runat="server" id="trVendor">
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlVendor">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label" style="width: 19%;">Vendor</span> <span class="custom-sel label-sel"
                                                        style="width: 74%;">
                                                        <asp:DropDownList ID="ddlVendor" AutoPostBack="true" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged"
                                                            onchange="pageLoad(this,value);" runat="server">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr runat="server" id="trEmailAddress">
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlEmailAddress">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label" style="width: 19%;">Email Address</span> <span style="width: 74%;">
                                                        <asp:Label ID="lblEmailAddress" Text="" runat="server"></asp:Label></span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr runat="server" id="trIE" class="checktable_supplier true">
                                    <td>
                                        <span style="font-weight: bold; color: #72757C;">Incentex Employees</span>
                                        <div class="spacer5">
                                        </div>
                                        <div style="overflow: auto; height: 84px;">
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
                <div id="cboxMiddleRight" style="float: left; height: 500px;">
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
    <asp:Panel ID="pnlCustomerMessage" runat="server" Style="display: none;">
        <div class="credboxWrapper" style="display: block; width: 550px; height: 600px; position: fixed;
            left: 35%; top: 5%;">
            <div style="">
                <div id="credboxTopLeft" style="float: left;">
                </div>
                <div id="credboxTopCenter" style="float: left; width: 400px;">
                </div>
                <div id="credboxTopRight" style="float: left;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="credboxMiddleLeft" style="float: left; height: 550px;">
                </div>
                <div id="credboxContent" style="float: left; display: block; height: 550px; width: 400px;">
                    <div id="credboxLoadedContent" style="display: block; margin: 0; overflow: hidden;">
                        <div id="credboxClose">
                            close</div>
                        <div style="margin-top: 30px;">
                            <table class="form_table" cellpadding="0" cellspacing="0">
                                <tr runat="server" id="trAdminList">
                                    <td>
                                        <div class="popuplabel">
                                            You are about to send a note to a customer
                                        </div>
                                        <asp:UpdatePanel runat="server" ID="upAdminList">
                                            <ContentTemplate>
                                                <div class="red_form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="red_form_box">
                                                    <span class="input_label" style="width: 19%;">Admin</span> <span class="custom-sel label-sel"
                                                        style="width: 74%;">
                                                        <asp:DropDownList ID="ddlAdminForWorkgroup" onchange="pageLoad(this,value);" runat="server">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="red_form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr runat="server" id="tr4">
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                            <ContentTemplate>
                                                <div class="red_form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="red_form_box">
                                                    <span class="input_label" style="width: 19%;">Email Address</span> <span style="width: 74%;">
                                                        <asp:Label ID="lblCustomerEmailAddress" Text="" runat="server"></asp:Label></span>
                                                </div>
                                                <div class="red_form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr runat="server" id="tr1" class="checktable_supplier true">
                                    <td>
                                        <span style="font-weight: bold; color: #72757C;">Incentex Employees</span>
                                        <div class="spacer5">
                                        </div>
                                        <div style="overflow: auto; height: 84px;">
                                            <asp:DataList ID="dtIncentexEmployee" runat="server" RepeatColumns="3" RepeatDirection="Vertical">
                                                <ItemTemplate>
                                                    <span class="custom-checkbox alignleft redbox" id="spnUser" runat="server">
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
                                <tr>
                                    <td>
                                        <div class="red_form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="red_form_box taxt_area clearfix" style="height: 150px">
                                            <span class="input_label alignleft" style="height: 150px; line-height: 130px;">Message</span>
                                            <div class="textarea_box alignright" style="width: 78%;">
                                                <div class="red_scrollbar" style="height: 150px">
                                                    <a href="#scroll" id="A3" class="red_scrolltop"></a><a href="#scroll" id="A4" class="red_scrollbottom">
                                                    </a>
                                                </div>
                                                <asp:TextBox ID="txtCustomerNote" runat="server" TextMode="MultiLine" CssClass="scrollme"
                                                    Height="147px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="red_form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </td>
                                </tr>
                                <tr runat="server" id="tr5" class="checktable_supplier true">
                                    <td>
                                        <span class="custom-checkbox redbox" id="Span1" runat="server" style="float: left;">
                                            <asp:CheckBox ID="chkIsCustomerMailSend" runat="server" />
                                        </span>
                                        <label>
                                            <asp:Label ID="lblCustomerIsEmailSend" Text="Send Email" runat="server"></asp:Label></label>
                                    </td>
                                </tr>
                            </table>
                            <div class="centeralign">
                                <asp:LinkButton ID="lnkCustomerMessage" class="red2_btn" runat="server" OnClick="lnkbtnSubmitNote_Click"
                                    OnClientClick="return confirm('Are you sure you want to send this message to the customer?');"><span>Send</span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="credboxMiddleRight" style="float: left; height: 550px;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="credboxBottomLeft" style="float: left;">
                </div>
                <div id="credboxBottomCenter" style="float: left; width: 400px;">
                </div>
                <div id="credboxBottomRight" style="float: left;">
                </div>
            </div>
        </div>
    </asp:Panel>
    <!-- Black and Red Popups extender, Panel and Body-->
</asp:Content>
