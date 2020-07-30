<%@ Page Title="Search Users" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="SearchUserResult.aspx.cs" Inherits="MyAccount_SearchUserResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td width="50%" style="text-align: left;">
                </td>
                <td width="50%" style="text-align: right;" class="errormessage">
                    Total Records :
                    <asp:Label ID="lblRecords" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="grdView_RowDataBound"
            OnRowCommand="grdView_RowCommand">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCompanyEmployeeId" Text='<% #Eval("CompanyEmployeeId")  %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="FullName">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnFullName" runat="server" CommandArgument="FullName" CommandName="Sort"><span>Full Name</span></asp:LinkButton>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                        <asp:PlaceHolder ID="placeholderFullName" runat="server"></asp:PlaceHolder>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span class="first">
                            <asp:HyperLink runat="server" ID="lblFullName" Text='<%# Eval("FullName") %>' />
                        </span>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="25%" />
                </asp:TemplateField>
                <%--<asp:TemplateField SortExpression="Contact">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnContactAddress" runat="server" CommandArgument="Contact"
                            CommandName="Sort"><span class="white_co">Contact Address</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderContract" runat="server"></asp:PlaceHolder>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblContact" Text='<%# Eval("Contact") %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>--%>
                <asp:TemplateField SortExpression="Email">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnEmaiAddress" runat="server" CommandArgument="Email" CommandName="Sort"><span class="white_co">Email Address</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderEmail" runat="server"></asp:PlaceHolder>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEmail" Text='<%# Eval("Email") %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="25%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Telephone">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnTelephone" runat="server" CommandArgument="Telephone" CommandName="Sort"><span class="white_co">Telephone</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderTelephone" runat="server"></asp:PlaceHolder>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTelephone" Text='<%# Eval("Telephone") + "&nbsp;"%>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="20%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Mobile">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnMobile" runat="server" CommandArgument="Mobile" CommandName="Sort"><span class="white_co">Mobile</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderMobile" runat="server"></asp:PlaceHolder>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblMobile" Text='<%# Eval("Mobile") + "&nbsp;" %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="20%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span class="centeralign">Status</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span class="btn_space">
                            <asp:HiddenField ID="hdnLookupIcon" runat="server" Value='<%# Eval("CompanyName")%>' />
                            <img id="imgLookupIconView" style="height: 20px; width: 20px" runat="server" alt='Loading' /></span>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box centeralign" Width="10%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div>
            <div>
                <div class="companylist_botbtn">
                    <div class="alignright pagging" id="dvPager" runat="server">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"></asp:LinkButton>
                        <span>
                            <asp:DataList ID="dtlViewUsers" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="dtlViewUsers_ItemCommand"
                                OnItemDataBound="dtlViewUsers_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList>
                        </span>
                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
