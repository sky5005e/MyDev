<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ListDecoratingItems.aspx.cs" Inherits="admin_ArtWorkDecoratingLibrary_ListDecoratingItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad" style="margin-left: 100px; margin-right : 80px;">
        <div style="text-align: center">
            <asp:Label ID="lblMsgGlobal" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <asp:GridView ID="grdViewDecoSpec" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblArtworkID" Text='<%# Eval("DecoratingSpecificationsID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="CompanyName">
                    <HeaderTemplate>
                        <span>Company Name</span>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCompanyName" Text='<%# Eval("CompanyName") %> ' />
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box"  Width="30%"/>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="ArtworkName">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkArtworkName" runat="server" CommandArgument="ArtworkName"
                            CommandName="Sort"><span>Job Name</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                    <span>
                     <asp:HyperLink ID="lblJobName" runat="server" NavigateUrl='<%# "~/admin/ArtWorkDecoratingLibrary/AddDecoratingSpecifications.aspx?DecoId=" + Eval("DecoratingSpecificationsID").ToString()%>'
                                Text='<%# Eval("JobName") %>'></asp:HyperLink>
                                </span>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="40%"/>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Updated On</span>
                        <div class="corner">
                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        
                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Date") %>' />
                            <div class="corner">
                                <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                            </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box centeralign" Width="30%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
