<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewTemplates.aspx.cs" Inherits="admin_CommunicationCenter_ViewTemplates" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        iframe
        {
            height: 95%;
            overflow: hidden;
            width: 100%;
            background-color: #171717;
            overflow: auto;
            color: #878282;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="spacer10">
    </div>
    <div id="dvTemp" runat="server">
        <table style="width: 100%; height: 650px;">
            <tr id="trIframe" runat="server">
                <td style="width: 100%; height: 100%;">
                    <iframe id="iframe" runat="server" style="border-width: 1px;" scrolling="yes"></iframe>
                </td>
            </tr>
            <tr id="trscroll" runat="server">
                <td>
                    <div id="dvScroll" runat="server" class="iframe" style="color: #878282; width: 580px;
                        background-color: White;">
                    </div>
                </td>
            </tr>
            <tr id="truserDetails" runat="server">
                <td>
                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                    <asp:UpdatePanel runat="server" ID="upnlCompanyStore">
                        <ContentTemplate>
                            <div class="form_pad">
                                <div>
                                    <asp:GridView ID="gvCampaignUser" runat="server" HeaderStyle-CssClass="ord_header"
                                        CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="False" RowStyle-CssClass="ord_content"
                                        OnRowCommand="gvCampaignUser_RowCommand" OnRowDataBound="gvCampaignUser_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField Visible="False" HeaderText="UserInfoID">
                                                <HeaderTemplate>
                                                    <span>UserInfo ID</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <asp:HiddenField ID="hdnCompanyId" runat="server" Value='<%# Eval("CompanyID")%>' />
                                                    <asp:Label runat="server" ID="lblUserInfoID" Text='<%# Eval("UserInfoID") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="FirstName">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblFirstName" Visible="true" runat="server" Text="FirstName"></asp:Label>
                                                    <div class="corner">
                                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label CssClass="first" ID="lblFirstName" runat="server" Visible="true" Text='<%# Eval("FirstName")%>'></asp:Label>
                                                    <div class="corner">
                                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" CssClass="b_box" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="LastName">
                                                <HeaderTemplate>
                                                    <span class="centeralign">
                                                        <asp:LinkButton ID="lnkbtnLastName" runat="server" CommandName="Sort">Last Name</asp:LinkButton>
                                                    </span>
                                                    <asp:PlaceHolder ID="placeholderLastName" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("LastName")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box" Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="LoginEmail1">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnLoginEmail" runat="server" CommandName="Sort">
                                                        <span class="centeralign">Email Address</span>
                                                        <div class="corner" >
                                                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                        </div>
                                                    </asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderLoginEmail" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmail1" runat="server" Text='<%# Eval("LoginEmail")%>'></asp:Label>
                                                    <div class="corner" >
                                                        <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box" Width="30%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="LoginEmail2">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnLoginEmail" runat="server" CommandName="Sort">
                                            <span class="centeralign">Email Address</span>
                                                                
                                                    </asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderLoginEmail" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("LoginEmail")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box" Width="30%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span class="centeralign">Remove </span>
                                                    <div class="corner">
                                                        <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span class="btn_space centeralign">
                                                        <asp:HiddenField ID="hdnMailFlag" runat="server" Value='<%# Eval("MailFlag")%>' />
                                                        <asp:ImageButton ID="imgApprove" runat="server" Text="" CommandName="Approve" CommandArgument='<%# Eval("UserInfoID") %>'
                                                            ImageUrl="~/Images/grd_active.png" ToolTip="Approve" Height="20" Width="20">
                                                        </asp:ImageButton>
                                                        <asp:ImageButton ID="imgReject" runat="server" Text="" CommandName="Reject" CommandArgument='<%# Eval("UserInfoID") %>'
                                                            ImageUrl="~/Images/grd_inactive.png" ToolTip="Reject" Height="18" Width="18">
                                                        </asp:ImageButton>
                                                    </span>
                                                    <div class="corner">
                                                        <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box centeralign" Width="20%" />
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
