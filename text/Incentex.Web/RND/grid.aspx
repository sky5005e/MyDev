<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="grid.aspx.cs" Inherits="RND_grid" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID"
        CellPadding="4" ForeColor="#333333" GridLines="None" ShowFooter="True">
        <Columns>
            <asp:TemplateField HeaderText="UserID" InsertVisible="False" SortExpression="UserID">
                <ItemTemplate>
                    <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="UserName" SortExpression="UserName">
                <EditItemTemplate>
                    <asp:TextBox ID="txtUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtfooterUserName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtfooterUserName"
                        ErrorMessage="*"></asp:RequiredFieldValidator>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="EditRecord">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                        Text="Update"></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel"></asp:LinkButton>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                        Text="Edit"></asp:LinkButton>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lnkInsert" runat="server" Text="Insert" ></asp:LinkButton>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="Silver" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
</asp:Content>
