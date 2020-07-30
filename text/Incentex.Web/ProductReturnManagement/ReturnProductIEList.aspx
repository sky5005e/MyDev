<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ReturnProductIEList.aspx.cs" Inherits="ProductReturnManagement_ReturnProductIEList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div>
            <div style="text-align: right; padding-right: 30px; color: #72757C">
                <asp:Label ID="lblCount" runat="server"></asp:Label>
            </div>
            <div class="spacer10">
            </div>
            <div style="text-align: center; color: Red; font-size: larger;">
                <asp:Label ID="lblmsg" runat="server">
                </asp:Label>
            </div>
            <div class="form_pad" style="padding-top: 0px !important;">
                <div>
                    <div style="text-align: center; color: Red; font-size: larger;">
                    </div>
                    <asp:GridView ID="gvProductReturn" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvProductReturn_RowDataBound">
                        <Columns>
                            <asp:TemplateField SortExpression="OrderNumber">
                                <HeaderTemplate>
                                    <span>Return #</span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                    <asp:HyperLink ID="hypOrderNumber" CommandArgument='<%# Eval("OrderNumber") %>' CommandName="Edit"
                                        runat="server"><span><%# Eval("OrderNumber")%></span></asp:HyperLink>
                                    <asp:HiddenField ID="hdnOrderNumber" runat="server" Value='<%# Eval("OrderNumber") %>' />
                                    <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                                    <asp:HiddenField ID="hdnProductReturnId" runat="server" Value='<%# Eval("ProductReturnId") %>' />
                                    <asp:HiddenField ID="hdnShippID" runat="server" Value='<%# Eval("ShippID") %>' />
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Company">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCompany" runat="server" CommandArgument="Company" CommandName="Sort"> <span >Company</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderCompany" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompany" Text='<%# Eval("CompanyName") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Contact">
                                <HeaderTemplate>
                                    <span>Customer Name</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblContact" Text='<%# Eval("ContactName") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Status">
                                <HeaderTemplate>
                                    <span>Status</span>
                                    <div class="corner">
                                        <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnStatus" Value='<%# Eval("ReturnStatus") %>' />
                                    <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("ReturnStatus") %>' />
                                    <div class="corner">
                                        <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="15%" />
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
</asp:Content>
