<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UserList.aspx.cs" Inherits="admin_CommunicationCenter_UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
     <div style="text-align: right; padding-right: 30px; color: #72757C">
            <asp:Label ID="lblCount" runat="server"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <div>
            <asp:GridView ID="gvUser" runat="server" HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box"
                GridLines="None" AutoGenerateColumns="False" RowStyle-CssClass="ord_content"
                OnRowDataBound="gvUser_RowDataBound">
                <Columns>
                    <asp:TemplateField Visible="False" HeaderText="UserInfoID">
                        <HeaderTemplate>
                            <span>UserInfo ID</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblUserInfoID" Text='<%# Eval("UserInfoID") %>' />
                            <asp:Label runat="server" ID="lblMailID" Text='<%# Eval("MailID") %>' />
                        </ItemTemplate>
                        <ItemStyle />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblFirstName" Visible="true" runat="server" Text="Company Name"></asp:Label>
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCompanyName" runat="server" CssClass="first" Text='<%# Eval("CompanyName")%>'></asp:Label>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="25%" CssClass="b_box" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblFirstName" Visible="true" runat="server" Text="Name"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label CssClass="first" ID="lblFirstName" runat="server" Visible="true" Text='<%# Eval("FullName")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblDate" runat="server">Date</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblMailDate" Text='<%# Eval("DateTime") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span>Details</span>
                            <div class="corner">
                                <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span>
                                <asp:HiddenField ID="hdnTempID" runat="server" Value='<%#Eval("MailID")%>' />
                                <asp:HyperLink ID="hypViewTemp" runat="server" ToolTip="view templates" CommandArgument='<%# Eval("MailID") %>'>View Templates</asp:HyperLink>
                            </span>
                            <div class="corner">
                                <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="30%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
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
</asp:Content>
