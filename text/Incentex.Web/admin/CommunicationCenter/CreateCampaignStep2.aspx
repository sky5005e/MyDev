<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CreateCampaignStep2.aspx.cs" Inherits="admin_CommunicationCenter_CreateCampaignStep2"
    Title="Communication Center" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdateProgress runat="server" ID="uprogressPGrid" DisplayAfter="1" AssociatedUpdatePanelID="upnlCompanyStore">
        <ProgressTemplate>
            <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
            </div>
            <div class="updateProgressDiv">
                <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <table>
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="upnlCompanyStore">
                    <ContentTemplate>
                        <div class="form_pad">
                            <div>
                                <div style="text-align: center; color: Red; font-size: larger;">
                                    <asp:Label ID="lblmsg" runat="server">
                                    </asp:Label>
                                </div>
                                <div style="text-align: right; color: Red; font-size: larger;">
                                    <span>Email Sending: </span>
                                    <asp:Label ID="LblEmailSending" runat="server">
                                    </asp:Label>
                                    <span>Emails Flagged: </span>
                                    <asp:Label ID="LblEmailFlaged" runat="server">
                                    </asp:Label>
                                </div>
                                <%--Before change (original) start--%>
                                <asp:GridView ID="gvCampaignUser" runat="server" HeaderStyle-CssClass="ord_header"
                                    CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                                    OnRowCommand="gvCampaignUser_RowCommand" OnRowDataBound="gvCampaignUser_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField Visible="False" HeaderText="UserInfoID">
                                            <HeaderTemplate>
                                                <span>UserInfo ID</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
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
                                                    <asp:LinkButton ID="lnkbtnLastName" runat="server" CommandName="Sort" Text="Last Name"></asp:LinkButton>
                                                    <%--<asp:Label CssClass="first" ID="lblLastName" runat="server" Text='<%# Eval("LastName")%>'></asp:Label>--%>
                                                    <%--<asp:Label ID="lblLastName" runat="server" Text="LastName"></asp:Label>--%>
                                                </span>
                                                <asp:PlaceHolder ID="placeholderLastName" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblLastName" runat="server" CssClass="first" Text='<%# Eval("LastName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="LoginEmail">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnLoginEmail" runat="server" CommandName="Sort">
                                            <span class="centeralign">Email Address</span>
                                                </asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderLoginEmail" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label CssClass="first" ID="lblEmail" runat="server" Text='<%# Eval("LoginEmail")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box centeralign" Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <span class="centeralign">Remove </span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class="btn_space centeralign">
                                                    <asp:HiddenField ID="hdnMaliFlag" runat="server" Value='<%# Eval("MailFlag")%>' />
                                                    <asp:ImageButton ID="imgApprove" runat="server" Text="" CommandName="Approve" CommandArgument='<%# Eval("UserInfoID") %>'
                                                        ImageUrl="~/Images/grd_active.png" ToolTip="Approve" Height="20" Width="20">
                                                    </asp:ImageButton>
                                                    <asp:ImageButton ID="imgReject" runat="server" Text="" CommandName="Reject" CommandArgument='<%# Eval("UserInfoID") %>'
                                                        ImageUrl="~/Images/grd_inactive.png" ToolTip="Reject" Height="18" Width="18">
                                                    </asp:ImageButton>
                                                </span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="20%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Total">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("TotalMail").ToString()%>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <%--Before change (original) end--%>
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
                            <div class="spacer25">
                            </div>
                            <div class="spacer25">
                            </div>
                            <div class="bot_alert next">
                                <asp:LinkButton ID="lnkNext" runat="server" title="Next Step 3" OnClick="lnkNext_Click">Next Step 3</asp:LinkButton>
                            </div>
                            <div class="bot_alert prev">
                                <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
                                <%--<asp:HyperLink ID="lnkPrev" NavigateUrl="~/admin/CompanyStore/CompanyPrograms/ViewAnniversaryPrograms.aspx" runat="server">Back</asp:HyperLink>--%>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
