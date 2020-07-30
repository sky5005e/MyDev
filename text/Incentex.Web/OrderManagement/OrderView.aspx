<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderView.aspx.cs" Inherits="OrderManagement_OrderView" Title="Order Management>> Order Detail View" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link media="screen" rel="stylesheet" href="../CSS/colorbox.css" />
    <style type="text/css">
        .custom-checkbox input, .custom-checkbox_checked input
        {
            width: 20px;
            height: 20px;
            margin-left: -8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>

    <script type="text/javascript">
        function SendConfirmation() {
            if (confirm("Are you sure, you want to send an email to all the supplier(s) for this order?") == true)
                return true;
            else
                return false;
        }   
    </script>

    <script type="text/javascript">
        $(window).load(function() {
            $("#dvLoader").hide();
        });
        
        $().ready(function() {
            $(".editorder").click(function() {
                $('#dvLoader').show();
            });
            
            $(window).scroll(function () {
                $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop());
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
        });
        
        function confirmTransmission(orderNumber) {
            if (confirm("Are you sure, you want to transmit order # " + orderNumber + " to SAP?")) {
                $('#dvLoader').show();
                return true;
            }
            else
                return false;
        }
    </script>

    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div>
            <div style="text-align: center; color: Red; font-size: larger;">
                <asp:Label ID="lblmsg" runat="server">
                </asp:Label>
            </div>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="50%" runat="server" id="tdCompany" visible="false">
                        <div style="text-align: left; color: #5C5B60; font-size: larger; font-weight: bold;
                            padding-left: 15px;">
                            Company Name :
                            <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                        </div>
                    </td>
                    <td width="50%" runat="server" id="tdReords">
                        <div style="text-align: right; color: #5C5B60; font-size: larger; font-weight: bold;
                            padding-left: 15px;">
                            <asp:Label ID="lblRecords" runat="server"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="alignnone spacer15">
            </div>
            <div class="form_pad" style="padding-top: 0px !important;">
                <div>
                    <div style="text-align: center; color: Red; font-size: larger;">
                    </div>
                    <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
                    <asp:GridView ID="gvOrderDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvOrderDetail_RowDataBound"
                        OnRowCommand="gvOrderDetail_RowCommand">
                        <Columns>
                            <asp:TemplateField SortExpression="OrderNumber">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnOrderNumber" runat="server" CommandArgument="OrderID" CommandName="Sort"><span>Order #</span></asp:LinkButton>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="first">
                                        <asp:LinkButton ID="lnkOrdrNmbr" CommandArgument='<%# Eval("OrderID") %>' CommandName="orderdetail"
                                            runat="server" CssClass="editorder" Text='<%# Eval("OrderNumber")%>'></asp:LinkButton>
                                        <asp:HiddenField ID="hdnOrderNumber" runat="server" Value='<%# Eval("OrderID") %>' />
                                    </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="SAPStatus">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnSAPStatus" runat="server" CommandArgument="SentToSAP" CommandName="Sort"
                                        ToolTip="SAP Status"><span>SAP</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="btn_space">
                                        <asp:ImageButton AlternateText="X" ID="imgSAPStatus" CommandArgument='<%# Eval("OrderID") + "," + Eval("OrderNumber") %>'
                                            CommandName="Retransmit" runat="server" OnClientClick='<%# "return confirmTransmission(" + Convert.ToString(Eval("OrderNumber")) + ");" %>' /></span>
                                    <asp:HiddenField runat="server" ID="hdnSentToSAP" Value='<%# Eval("SentToSAP") %>' />
                                    <asp:HiddenField runat="server" ID="hdnUpdatedBySAPDate" Value='<%# Eval("UpdatedBySAPDate") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCanReTransmitToSAP" Value='<%# Eval("CanReTransmitToSAP") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box centeralign" Width="3%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Name">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCompanyName" runat="server" CommandName="Sort" CommandArgument="Name"><span>Company Name</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompanyName" ToolTip='<%# Eval("Name") %>' Text='<%# Convert.ToString(Eval("Name")).Length > 15 ? Convert.ToString(Eval("Name")).Substring(0, 15).Trim() + "..." : Convert.ToString(Eval("Name")) %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="14%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="OrderDate">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnOrderDate" runat="server" CommandName="Sort" CommandArgument="OrderDate"><span class="centeralign">Submit Date</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrderDate" Text='<%# Eval("OrderDate") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box centeralign" Width="17%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Contact">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnContact" runat="server" CommandArgument="FirstName" CommandName="Sort"><span>Contact</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblContact" ToolTip='<%# Eval("FirstName") + " " + Eval("LastName") %>'
                                        Text='<%# Convert.ToString(Eval("FirstName") + " " + Eval("LastName")).Length > 18 ? Convert.ToString(Eval("FirstName") + " " + Eval("LastName")).Substring(0, 18).Trim() + "..." : Convert.ToString(Eval("FirstName") + " " + Eval("LastName")) %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="18%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Workgroup">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnWorkgroup" runat="server" CommandArgument="Workgroup" CommandName="Sort"><span>Work Group</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblWorkgroup" ToolTip='<%# Eval("WorkGroup") %>' Text='<%# Convert.ToString(Eval("WorkGroup")).Length > 15 ? Convert.ToString(Eval("WorkGroup")).Substring(0, 15).Trim() + "..." : Convert.ToString(Eval("WorkGroup")) %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="16%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblEmail" runat="server">Email</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>
                                        <asp:LinkButton ID="lnkSendEmail" runat="server" CommandArgument='<%# Eval("OrderID") %>'
                                            CssClass="btn_space" ToolTip="Send an E-mail to the supplier(s) of this order"
                                            OnClientClick="return SendConfirmation();" CommandName="SendEmail"><img height="24" width="24" src="../Images/shipment06.png" alt="X" /></asp:LinkButton>
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" Width="3%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="OrderStatus">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnOrderStatus" runat="server" CommandArgument="OrderStatus"
                                        CommandName="Sort"><span>Status</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnOrderStatus" Value='<%# Eval("OrderStatus") %>' />
                                    <asp:Label runat="server" ID="lblStatus" ToolTip='<%# Eval("OrderStatus") %>' Text='<%# Convert.ToString(Eval("OrderStatus")).Length > 16 ? Convert.ToString(Eval("OrderStatus")).Substring(0, 16).Trim() + "..." : Convert.ToString(Eval("OrderStatus")) %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="16%" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:Label ID="lblDelete" runat="server">Delete</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkOrderID" CommandName="deleteorder" CommandArgument='<%# Eval("OrderID")%>'
                                        CssClass="btn_space" runat="server" OnClientClick="return confirm('Are you sure you want to delete Order?');"><span><img src="../Images/close.png" alt="X" /></span></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" Width="3%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <div id="pagingtable" runat="server" class="alignright pagging">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click">
                        </asp:LinkButton>
                        <span>
                            <asp:DataList ID="DataList2" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="DataList2_ItemCommand"
                                OnItemDataBound="DataList2_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList></span>
                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
    <at:ModalPopupExtender ID="modal" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
        DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="cboxClose">
    </at:ModalPopupExtender>
    <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
        <div id="cboxWrapper" style="display: block; width: 458px; height: 300px; left: 35%;
            top: 30%; position: fixed;">
            <div style="">
                <div id="cboxTopLeft" style="float: left;">
                </div>
                <div id="cboxTopCenter" style="float: left; width: 408px;">
                </div>
                <div id="cboxTopRight" style="float: left;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxMiddleLeft" style="float: left; height: 250px;">
                </div>
                <div id="cboxContent" style="float: left; width: 408px; display: block; height: 250px;">
                    <div id="cboxLoadedContent" style="display: block;">
                        <div style="padding: 10px;">
                            <div class="weatherDetails true" style="height: auto;">
                                <asp:HiddenField ID="hdnOrderID" runat="server" />
                                <asp:GridView ID="gvSupplier" ShowHeader="false" runat="server" AutoGenerateColumns="false"
                                    GridLines="None" CssClass="weather_box">
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("SupplierID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="alignleft d-weather" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <span runat="server" id="dvChk" class="custom-checkbox centeralign">
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </span></div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="alignright d-link" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="spacer10" style="clear: both;">
                            </div>
                            <div class="centeralign">
                                <asp:LinkButton ID="btnSubmit" CssClass="grey2_btn" runat="server" OnClick="btnSubmit_Click">
                                <span>Send Mail</span>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div id="cboxLoadingOverlay" style="height: 250px; display: none;" class="">
                        </div>
                        <div id="cboxLoadingGraphic" style="height: 250px; display: none;" class="">
                        </div>
                        <div id="cboxTitle" style="display: block;" class="">
                        </div>
                        <div id="cboxClose" style="" class="">
                            close</div>
                    </div>
                </div>
                <div id="cboxMiddleRight" style="float: left; height: 250px;">
                </div>
            </div>
            <div style="clear: left;">
                <div id="cboxBottomLeft" style="float: left;">
                </div>
                <div id="cboxBottomCenter" style="float: left; width: 408px;">
                </div>
                <div id="cboxBottomRight" style="float: left;">
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
