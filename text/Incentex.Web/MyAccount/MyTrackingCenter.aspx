<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MyTrackingCenter.aspx.cs" Inherits="MyAccount_MyTrackingCenter" Title="My Tracking Center" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="srcMgr" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upPanel" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="gvEmployeeTrackingCenter" />
        </Triggers>
        <ContentTemplate>
            <div class="form_pad" style="padding-top: 20px !important;">
                <asp:GridView ID="gvEmployeeTrackingCenter" runat="server" AutoGenerateColumns="false"
                    HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                    RowStyle-CssClass="ord_content" OnRowCommand="gvEmployeeTrackingCenter_RowCommand"
                    OnRowDataBound="gvEmployeeTrackingCenter_RowDataBound">
                    <Columns>
                        <asp:TemplateField Visible="false" HeaderText="Id">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOrderID" Text='<%# Eval("OrderID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="OrderID">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnTrasHistory" CommandArgument="OrderID" CommandName="Sorting"
                                    runat="server"><span class="white_co">Order Number</span></asp:LinkButton>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <span class="first">
                                    <asp:LinkButton ID="lblOrderNumber" runat="server" CommandArgument='<%# Eval("OrderID") %>'
                                        CommandName="OrderDetail" Text='<%# Eval("OrderNumber") %>'></asp:LinkButton>
                                    <%--<asp:Label runat="server" ID="lblOrderNumber" Text='<%# Eval("OrderNumber") %>' />--%>
                                </span>
                                <div class="corner">
                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="20%" CssClass="g_box centeralign" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="ReferenceName">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnReferenceName" CommandArgument="ReferenceName" CommandName="Sorting"
                                    runat="server"><span class="white_co">Reference Name</span></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblReferenceName" Text='<%# Eval("ReferenceName").ToString() != "" ? Eval("ReferenceName") : "---" %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="25%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="TotalOrderAmount">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnOrderAmount" runat="server" CommandArgument="TotalAmount"
                                    CommandName="Sorting">
                                <span>Order Amount</span>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOrderAmountView" Text='<%# Eval("TotalAmount") %>' />
                                <asp:Label runat="server" ID="lblMOASOrderAmountView" Text='<%# Eval("TotalMOASOrderAmount") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="CreditAmount">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnCreditAmount" runat="server" CommandArgument="PaymentMethod"
                                    CommandName="Sorting">
                                <span>Payment Method</span>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPaymentMethod" Text='<%# Eval("PaymentMethod") != null? Eval("PaymentMethod") : "Paid By Corporate" %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="20%" />
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
                                <div class="corner">
                                    <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("OrderStatus") %>' />
                                <div class="corner">
                                    <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="30%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div id="pagingtable" runat="server" class="alignright pagging">
                    <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
