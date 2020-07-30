<%@ Page Title="Employee Credit Report" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="AnniversaryCreditEmployeeWiseReport.aspx.cs"
    Inherits="admin_Report_AnniversaryCreditEmployeeWiseReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div>
            <asp:GridView ID="dtlEmployeeCredit" runat="server" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                OnRowCommand="dtlEmployeeCredit_RowCommand">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnHeaderEmployeeID" runat="server" CommandArgument="EmployeeID"
                                    CommandName="Sort">Employee #</asp:LinkButton>
                            </span>
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" CssClass="first" ID="lblEmployeeID" Text='<%# Eval("EmployeeID")%>'></asp:Label>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnHeaderName" runat="server" CommandArgument="Name" CommandName="Sort"><span>Employee Name</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblName" Text='<%# Eval("Name") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnHeaderHirerdDate" runat="server" CommandArgument="HirerdDate"
                                CommandName="Sort">
                                <span>Hire Date</span>
                            </asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblHirerdDater" Text='<%#Eval("HirerdDate")!= DBNull.Value ? Convert.ToDateTime(Eval("HirerdDate")).ToShortDateString() : "--" %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnHeaderLastIssueDate" runat="server" CommandArgument="LastIssueDate"
                                CommandName="Sort"><span>Date Issued</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblLastIssueDate" Text='<%#Eval("LastIssueDate")!= DBNull.Value ? Convert.ToDateTime(Eval("LastIssueDate")).ToShortDateString() : "--" %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnHeaderLastIssueAmount" runat="server" CommandArgument="LastIssueAmount" CommandName="Sort"><span>Issued Amount</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblLastIssueAmount" Text='<%# Eval("LastIssueAmount") != DBNull.Value ? Eval("LastIssueAmount") : "--" %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%"/>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnHeaderTotalIssueAmount" runat="server" CommandArgument="TotalIssueAmount" CommandName="Sort"><span>Total Credit Issued</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblTotalIssueAmount" Text='<%# Eval("TotalIssueAmount") != DBNull.Value ? Eval("TotalIssueAmount") : "--" %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="10%"/>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnHeaderTotalUsedAmount" runat="server" CommandArgument="TotalUsedAmount" CommandName="Sort"><span>Total Credit Used</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblTotalUsedAmount" Text='<%# Eval("TotalUsedAmount") != DBNull.Value ? Eval("TotalUsedAmount") : "--" %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%"/>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnHeaderCreditAmtToApplied" runat="server" CommandArgument="CreditAmtToApplied"
                                CommandName="Sort"><span>Current Balance</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblAmount" Text='<%# Eval("CreditAmtToApplied") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="15%"/>
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
