<%@ Page Title="Search User Result" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="SearchUserResult.aspx.cs" Inherits="admin_SearchUserResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td width="50%">
                    <div style="text-align: left; color: #5C5B60; font-size: larger; font-weight: bold;
                        padding-left: 15px;">
                        <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                    </div>
                </td>
                <td width="50%">
                    <div style="text-align: right; color: #5C5B60; font-size: larger; font-weight: bold;
                        padding-left: 15px;">
                        Total Records :
                        <asp:Label ID="lblRecords" runat="server"></asp:Label>
                    </div>
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
                        <asp:HiddenField runat="server" ID="hdnUserInfoID" Value='<%# Eval("UserInformationId") %>' />
                        <asp:HiddenField runat="server" ID="hdnUserType" Value='<%# Eval("UserType") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="CompanyName">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtComapanyName" runat="server" CommandArgument="CompanyName"
                            CommandName="Sort"><span class="white_co">Company Name</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderComapany" runat="server"></asp:PlaceHolder>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="first">
                            <asp:Label runat="server" ID="lblCompanyName" Text='<% # (Convert.ToString(Eval("CompanyName")).Length > 30) ? Eval("CompanyName").ToString().Substring(0,30)+"..." : Convert.ToString(Eval("CompanyName"))+ "&nbsp;"  %>'
                                ToolTip='<% #Eval("CompanyName")  %>'></asp:Label>
                        </div>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="FullName">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnFullName" runat="server" CommandArgument="FullName" CommandName="Sort"><span>Full Name</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderFullName" runat="server"></asp:PlaceHolder>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:HyperLink ID="hplFullName" Text='<% # (Convert.ToString(Eval("FullName")).Length > 30) ? Eval("FullName").ToString().Substring(0,30)+"..." : Convert.ToString(Eval("FullName"))+ "&nbsp;"  %>'
                                ToolTip='<% #Eval("FullName")  %>' runat="server"></asp:HyperLink>
                        </span>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Contact">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnContactAddress" runat="server" CommandArgument="Contact"
                            CommandName="Sort"><span class="white_co">Contact Address</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderContract" runat="server"></asp:PlaceHolder>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblContact" Text='<% # (Convert.ToString(Eval("Contact")).Length > 20) ? Eval("Contact").ToString().Substring(0,20)+"..." : Convert.ToString(Eval("Contact"))+ "&nbsp;"  %>'
                            ToolTip='<% #Eval("Contact")  %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Email">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnEmaiAddress" runat="server" CommandArgument="Email" CommandName="Sort"><span class="white_co">Email Address</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderEmail" runat="server"></asp:PlaceHolder>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEmail" Text='<% # (Convert.ToString(Eval("Email")).Length > 30) ? Eval("Email").ToString().Substring(0,30)+"..." : Convert.ToString(Eval("Email"))+ "&nbsp;"  %>'
                            ToolTip='<% #Eval("Email")  %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Telephone">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnTelephone" runat="server" CommandArgument="Telephone" CommandName="Sort"><span class="white_co">Telephone</span></asp:LinkButton>
                        <asp:PlaceHolder ID="placeholderTelephone" runat="server"></asp:PlaceHolder>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTelephone" Text='<% # (Convert.ToString(Eval("Telephone")).Length > 20) ? Eval("Telephone").ToString().Substring(0,20)+"..." : Convert.ToString(Eval("Telephone"))+ "&nbsp;"  %>'
                            ToolTip='<% #Eval("Telephone")  %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
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
