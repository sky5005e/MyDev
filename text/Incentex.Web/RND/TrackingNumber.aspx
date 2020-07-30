<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TrackingNumber.aspx.cs" Inherits="RND_Default2" Title="Untitled Page" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:LinkButton ID="lnkAdd" runat="server" Text="+" onclick="lnkAdd_Click" >
</asp:LinkButton>
<asp:PlaceHolder ID="plc" runat="server">

</asp:PlaceHolder>--%>
    <asp:GridView ID="grv" runat="server" OnRowCancelingEdit="grv_RowCancelingEdit" OnRowDeleting="grv_RowDeleting"
        OnRowEditing="grv_RowEditing1" OnRowUpdating="grv_RowUpdating" Width="30%" AutoGenerateColumns="false"  >
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:TextBox ID="lblTrackingNumber" Text='<%#Eval("trackingnuber")%>' runat="server" ></asp:TextBox>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEdit" Text='<%#Eval("trackingnuber")%>' runat="server"></asp:TextBox>
                </EditItemTemplate>
                <%--<FooterTemplate>
                    <asp:TextBox ID="txtFooter" runat="server"></asp:TextBox>
                </FooterTemplate>--%>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
    <asp:TextBox ID="txtValue" runat="server"></asp:TextBox>
    <asp:Button ID="btnAddItem" runat="server" Text="Add Item" OnClick="btnAddItem_Click">
    </asp:Button>
</asp:Content>
