<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="PageAccessList.aspx.cs" Inherits="TrackingCenter_PageAccessList" Title="Page Access List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div class="form_pad">
                <div>
                    <div style="text-align: center; color: Red; font-size: larger;">
                        <asp:Label ID="lblmsg" runat="server">
                        </asp:Label>
                    </div>
                    <div id="dvgrid" align="center">
                        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span class="first centeralign">Sr.</span>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="first centeralign">
                                            <%#Container.DataItemIndex+1 %></span>
                                        <div class="corner">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span class="centeralign">Company Name</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompanyName" Text='<% #(Convert.ToString(Eval("CompanyName")).Length > 25) ? Eval("CompanyName").ToString().Substring(0,25)+"..." : Convert.ToString(Eval("CompanyName"))+ "&nbsp;"  %>' ToolTip='<%# Eval("CompanyName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span class="centeralign">Base Station</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompanyName" Text='<% #(Convert.ToString(Eval("sBaseStation")).Length > 25) ? Eval("sBaseStation").ToString().Substring(0,25)+"..." :  Convert.ToString(Eval("sBaseStation"))+ "&nbsp;"  %>'
                                            ToolTip='<%# Eval("sBaseStation") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span class="centeralign">Name</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--<span class="first">
                                            <%# Eval("FirstName") %></span>--%>
                                            <asp:Label runat="server" ID="lblFirstName" Text='<% #(Convert.ToString(Eval("FirstName")).Length > 25) ? Eval("FirstName").ToString().Substring(0,25)+"..." :  Convert.ToString(Eval("FirstName"))+ "&nbsp;"  %>'
                                            ToolTip='<%# Eval("FirstName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span class="centeralign">Access DateTime</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="first">
                                            <%# Eval("DateTimePage") %></span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span class="centeralign">Browser Type</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="first">
                                            <%# Eval("BrowserName") %></span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="20%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="alignright pagging" runat="server" visible="false" id="pager">
                            <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                            </asp:LinkButton>
                            <span>
                                <asp:DataList ID="lstPaging" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="lstPaging_ItemCommand"
                                    OnItemDataBound="lstPaging_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                            CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList></span>
                            <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
