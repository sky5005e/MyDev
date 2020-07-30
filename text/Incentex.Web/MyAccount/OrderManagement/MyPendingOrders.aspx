<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MyPendingOrders.aspx.cs" Inherits="MyAccount_OrderManagement_MyPendingOrders"
    Title="My Pending Orders" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script type="text/javascript">
        // Let's use a lowercase function name to keep with JavaScript conventions
//        function selectAll(invoker) {
//            // Since ASP.NET checkboxes are really HTML input elements
//            //  let's get all the inputs
//            var inputElements = document.getElementsByTagName('input');
//            for (var i = 0; i < inputElements.length; i++) {
//                var myElement = inputElements[i];
//                // Filter through the input types looking for checkboxes
//                if (myElement.type === "checkbox") {
//                    myElement.checked = invoker.checked;
//                }
//            }
//        } 
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager runat="server" ID="ToolkitScriptManager1">
    </at:ToolkitScriptManager>
    <asp:UpdateProgress runat="server" ID="uprogressPGrid" DisplayAfter="1" AssociatedUpdatePanelID="upPanel">
        <ProgressTemplate>
            <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
            </div>
            <div class="updateProgressDiv">
                <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="gvEmployeePendingOrders" />
        </Triggers>
        <ContentTemplate>
            <div class="form_pad" style="padding-top: 20px !important;">
                <div style="text-align: center">
                    <asp:Label ID="lblmsg" runat="server" CssClass="errormessage"></asp:Label>
                </div>
                <%--Start Gridview part--%>
                <asp:GridView ID="gvEmployeePendingOrders" runat="server" AutoGenerateColumns="false"
                    HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                    RowStyle-CssClass="ord_content" OnRowCommand="gvEmployeePendingOrders_RowCommand"
                    OnRowDataBound="gvEmployeePendingOrders_RowDataBound">
                    <Columns>
                        <asp:TemplateField Visible="false" HeaderText="Id">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOrderID" Text='<%# Eval("OrderID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Check">
                            <HeaderTemplate>
                                 <span class="custom-checkbox gridheader">
                                    <asp:CheckBox ID="cbSelectAll" runat="server" />&nbsp;</span>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <span class="first custom-checkbox gridcontent">
                                    <asp:CheckBox ID="chkSelectOrder" runat="server" />&nbsp; </span>
                                <div class="corner">
                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="5%" CssClass="b_box centeralign" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnTrasHistory" CommandArgument="OrderID" CommandName="Sorting"
                                    runat="server"><span>Order Number</span></asp:LinkButton>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <span>
                                    <asp:LinkButton ID="lblOrderNumber" runat="server" CommandArgument='<%# Eval("OrderID") %>'
                                        CommandName="OrderDetail" Text='<%# Eval("OrderNumber") %>'></asp:LinkButton>
                                </span>
                            </ItemTemplate>
                            <ItemStyle Width="15%" CssClass="g_box centeralign" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="ReferenceName">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnReferenceName" CommandArgument="ReferenceName" CommandName="Sorting"
                                    runat="server"><span class="white_co">Reference Name</span></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblReferenceName" Text='<%# Eval("ReferenceName").ToString() != "" ? Eval("ReferenceName") : "---" %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="TotalOrderAmount">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnOrderAmount" runat="server" CommandArgument="TotalAmount"
                                    CommandName="Sorting">
                                <span>Order Amount</span>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblMOASOrderAmountView" Text='<%# Eval("MOASPriceOrderAmount") %>' />
                                <asp:Label runat="server" ID="lblOrderAmountView" Text='<%# Eval("TotalAmount") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" Width="12%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="OrderDate">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnOrderDate" runat="server" CommandArgument="OrderDate" CommandName="Sorting">
                                <span>Submit Date</span>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOrderSubmitedDate" />
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Status">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblStatus" runat="server" CommandArgument="Status" CommandName="Sort">
                                <span>Status</span>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("OrderStatus") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Approve</span>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" Width="4%" />
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="imgbtnApproveOrder" ImageUrl="~/Images/approve_order_grid_btn.png"
                                    CommandArgument='<%# Eval("OrderID") %>' CommandName="ApproveOrder" />
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box centeralign" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Edit</span>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" Width="4%" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEditOrder" ImageUrl="~/Images/edit_order_grid_btn.png"
                                    runat="server" CommandArgument='<%# Eval("OrderID") %>' CommandName="OrderDetail" />
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box centeralign" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Cancel</span>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" Width="4%" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnCancelOrder" runat="server" ImageUrl="~/Images/cancel_order_grid_btn.png"
                                    CommandArgument='<%# Eval("OrderID") %>' CommandName="CancelOrder" />
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box centeralign" Width="7%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <%--End Gridview part--%>
                <div class="spacer15">
                    </div>
                    <div class="alignleft">
                        <asp:LinkButton ID="lnkBtnApproveOrder" runat="server" class="grey_btn" OnClick="lnkBtnApproveOrder_Click"><span>Approve</span></asp:LinkButton>
                    </div>
                <%--Start Gridview pagging part--%>
                <div id="pagingtable" runat="server" class="alignright pagging">
                    <asp:LinkButton ID="lnkbtnPrevious" CssClass="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                    </asp:LinkButton>
                    <span>
                        <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList></span>
                    <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                </div>
                <%--End Gridview pagging part--%>
            </div>
            <asp:HiddenField runat="server" ID="hdnCancelOrderID" />
            <asp:LinkButton ID="lnkCancelOrder1" runat="server" Style="display: none"></asp:LinkButton>
            <at:ModalPopupExtender ID="mpeCancelOrder" TargetControlID="lnkCancelOrder1" BackgroundCssClass="modalBackground"
                DropShadow="true" runat="server" PopupControlID="pnlNotesOrder" CancelControlID="closepopup">
            </at:ModalPopupExtender>
            <%--Start Cancel Order reason popup panel part--%>
            <asp:Panel ID="pnlNotesOrder" runat="server" Style="display: none; left: -200px;
                top: -100px;">
                <div class="pp_pic_holder facebook" style="display: block; width: 411px; ">
                    <div class="pp_top" style="">
                        <div class="pp_left">
                        </div>
                        <div class="pp_middle">
                        </div>
                        <div class="pp_right">
                        </div>
                    </div>
                    <div class="pp_content_container">
                        <div class="pp_left" style="">
                            <div class="pp_right" style="">
                                <div class="pp_content" style="height: 228px; display: block;">
                                    <div class="pp_fade" style="display: block;">
                                        <div id="pp_full_res">
                                            <div class="pp_inline clearfix">
                                                <div class="form_popup_box">
                                                    <div class="label_bar">
                                                        <span>Reason for Cancelling:
                                                            <br />
                                                            <br />
                                                            <asp:TextBox Height="120px" Width="350" TextMode="MultiLine" ID="txtCancelReason"
                                                                runat="server"></asp:TextBox></span>
                                                    </div>
                                                    <div>
                                                        <asp:LinkButton ID="lnkbtnCancelNow" CssClass="grey2_btn" runat="server" OnClick="lnkbtnCancelNow_Click"><span>Cancel Now</span></asp:LinkButton>
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
            <%--End Cancel Order reason popup panel part--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
