<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ListArtwork.aspx.cs" Inherits="admin_Artwork_ListArtwork" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsgGlobal" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="grdView_RowCommand">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblArtworkID" Text='<%# Eval("ArtworkID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblArtworkDesign" runat="server" CommandArgument="ArtworkDesign" CommandName="Sort">Design #</asp:Label>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span class="first btn_space">
                            <asp:HyperLink ID="lnkArtDesignNumber" runat="server" NavigateUrl='<%# "~/admin/Artwork/ArtworkPreview.aspx?Id=" + Eval("ArtworkID").ToString()%>'
                                Text='<%# Eval("ArtDesignNumber") %>'></asp:HyperLink>
                        </span>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="CompanyName">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkCompanyName" runat="server" CommandArgument="CompanyName"
                            CommandName="Sort"><span>Company Name</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCompanyName" Text='<%# Eval("CompanyName") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="ArtworkName">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkArtworkName" runat="server" CommandArgument="ArtworkName"
                            CommandName="Sort"><span>Artwork Name</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblArtworkName" Text='<%# Eval("ArtworkName") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Delete</span>
                        <div class="corner">
                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span class="btn_space">
                            <asp:LinkButton ID="lnkbtndelete" runat="server" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                CommandArgument='<%# Eval("ArtworkID") %>'>
                                <asp:Image ID="img" ImageUrl="~/Images/close.png" runat="server" />
                            </asp:LinkButton></span>
                        <div class="corner">
                            <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box centeralign" Width="10%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
