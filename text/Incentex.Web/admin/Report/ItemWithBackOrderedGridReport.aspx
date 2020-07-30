<%@ Page Title="Items with Backorders Report" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="ItemWithBackOrderedGridReport.aspx.cs" Inherits="admin_Report_ItemWithBackOrderedGridReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div style="text-align: right; margin-bottom: 10px;">
            Total Items:
            <asp:Label runat="server" ID="lblTotalItems" Text="0"></asp:Label><br />
            Total Value:
            <asp:Label runat="server" ID="lblTotalValues" Text="0"></asp:Label>
        </div>
        <div>
            <asp:GridView ID="gvItemswithBackorders" runat="server" AutoGenerateColumns="false"
                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                RowStyle-CssClass="ord_content" OnRowCommand="gvItemswithBackorders_RowCommand">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                CommandName="Sort"><span>Item #</span></asp:LinkButton>
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblItemNumber" CssClass="first" Text='<%# Eval("ItemNumber")%>'></asp:Label>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnProductDescription" runat="server" CommandArgument="Description"
                                CommandName="Sort"><span>Description</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblProductDescription" Text='<%# Convert.ToString(Eval("Description")).Length > 40 ? Convert.ToString(Eval("Description")).Substring(0, 40).Trim() + "..." : Convert.ToString(Eval("Description")) %>'
                                ToolTip='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnCurrentStock" runat="server" CommandArgument="CurrentStock"
                                CommandName="Sort"><span>Current Stock</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblCurrentStock" Text='<%# Eval("CurrentStock") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnOnOrder" runat="server" CommandArgument="OnOrder" CommandName="Sort"><span> On-Order</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblOnOrder" Text='<%# Eval("OnOrder") + "&nbsp;" %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnBackOrdered" runat="server" CommandArgument="BackOrdered"
                                CommandName="Sort"><span>BackOrdered</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblBackOrdered" Text='<%# Eval("BackOrdered")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnExpectedDate" runat="server" CommandArgument="ExpectedDate"
                                CommandName="Sort"><span>Expected Date</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblExpectedDate" Text='<%# Convert.ToString(Eval("ExpectedDate")).Length > 0 ? Convert.ToDateTime(Eval("ExpectedDate")).ToShortDateString() : "&nbsp;"%>'></asp:Label>
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
    </div>
</asp:Content>
