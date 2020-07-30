<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewAccessInfo.aspx.cs" Inherits="TrackingCenter_ViewAccessInfo" Title="Tracking Center" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <asp:UpdatePanel ID="up1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="gvmenu" />
                <asp:PostBackTrigger ControlID="lnkDummyAddNew" />
            </Triggers>
            <ContentTemplate>
                <div class="form_pad">
                    <div style="text-align: center; color: Red; font-size: larger;">
                        <asp:Label ID="lblmsg" runat="server">
                        </asp:Label>
                    </div>
                    <asp:GridView ID="gvmenu" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" AllowSorting="true"
                        OnRowCommand="gvmenu_RowCommand" OnRowDataBound="gvmenu_RowDataBound" OnDataBound="gvmenu_DataBound">
                        <Columns>
                            <asp:TemplateField Visible="false" HeaderText="Id">
                                <ItemTemplate>
                                    <%--<asp:Label runat="server" ID="lblID" Text='<%# Eval("CompanyName") %>'></asp:Label>
                                                                                <asp:Label runat="server" ID="lblUID" Text='<%# Eval("UserInfoID") %>'></asp:Label>--%>
                                    <%--<asp:Label runat="server" ID="lblUserStatus" Text='<%# Eval("UserStatus") %>'></asp:Label>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblsno" runat="server" CssClass="centeralign">Access</asp:Label>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label CssClass="first centeralign" runat="server" ID="SrNo"></asp:Label>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span class="centeralign">
                                        <asp:LinkButton ID="lnkMenuBrowserName" runat="server" CommandArgument="BrowserName"
                                            CommandName="Sorting">Browser Icon</asp:LinkButton>
                                    </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="centeralign">
                                        <asp:Label ID="mapid" runat="server" Style="display: none"></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdnBrowserName" Value='<%# Eval("BrowserName")%>' />
                                        <asp:ImageButton ID="imgFirefox" runat="server" Text="" CommandName="FireFox" CommandArgument='<%# Eval("BrowserName") %>'
                                            ImageUrl="~/admin/Incentex_Used_Icons/mozilla.png" Width="25px" ToolTip='<%# GetTooltip((Eval("BrowserName")),(Eval("BrowserVersion")))%>'>
                                        </asp:ImageButton>
                                        <asp:ImageButton ID="imgIE" runat="server" Text="" CommandName="IE" CommandArgument='<%# Eval("BrowserName") %>'
                                            ImageUrl="~/admin/Incentex_Used_Icons/ie.png" Width="25px" ToolTip='<%# GetTooltip((Eval("BrowserName")),(Eval("BrowserVersion")))%>'>
                                        </asp:ImageButton>
                                        <asp:ImageButton ID="imgOpera" runat="server" Text="" CommandName="Opera" CommandArgument='<%# Eval("BrowserName") %>'
                                            ImageUrl="~/admin/Incentex_Used_Icons/Opera.png" Width="25px" ToolTip='<%# GetTooltip((Eval("BrowserName")),(Eval("BrowserVersion")))%>'>
                                        </asp:ImageButton>
                                        <asp:ImageButton ID="imgSafari" runat="server" Text="" CommandName="Safari" CommandArgument='<%# Eval("BrowserName") %>'
                                            ImageUrl="~/admin/Incentex_Used_Icons/safari.png" Width="25px" ToolTip='<%# GetTooltip((Eval("BrowserName")),(Eval("BrowserVersion")))%>'> </asp:ImageButton>
                                        <%--<asp:Label runat="server" ID="lblBrowserName" Text='<% #Eval("BrowserName") %>'></asp:Label>--%>
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span class="centeralign">
                                        <asp:LinkButton ID="lnkMenuBrowserName" runat="server" CommandArgument="Page" CommandName="Sorting">Page</asp:LinkButton>
                                    </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="centeralign">
                                        <%--<asp:Button ID="btnAddemp" CommandArgument='<%# Eval("UserTrackID") %>' CommandName="BindHistory"
                                            runat="server" Text="+" Height="25px" Width="25px" />--%>
                                        <asp:ImageButton ID="btnAddemp" CommandArgument='<%# Eval("UserTrackID") %>' CommandName="BindHistory"
                                            runat="server" ImageUrl="~/admin/Incentex_Used_Icons/Plussign.png" Visible="true" />
                                        <asp:ImageButton ID="btnminusemp" CommandArgument='<%# Eval("UserTrackID") %>' CommandName="BindHistory"
                                            runat="server" ImageUrl="~/admin/Incentex_Used_Icons/minussign.png" Visible="false" />
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span class="centeralign">
                                        <asp:LinkButton ID="lnkMenuLoginTime" runat="server" CommandArgument="LoginTime"
                                            CommandName="Sorting">Date & Time</asp:LinkButton>
                                    </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLoginTime" Text='<%# Eval("LoginTime")%>' ToolTip='<% #Eval("LoginTime")  %>'
                                        CssClass="centeralign"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span class="centeralign">
                                        <asp:LinkButton ID="lnkLocation" runat="server" CommandArgument="IPAddress" CommandName="Sorting">Location</asp:LinkButton>
                                    </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="imgLocation" Text="" CssClass="centeralign"></asp:Label>
                                    <asp:HiddenField ID="hdnIpadd" runat="server" Value='<%# Eval("IPAddress")%>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span class="centeralign">
                                        <asp:LinkButton ID="lnkMap" runat="server" CommandArgument="MapLocation" CommandName="Sorting">Map</asp:LinkButton>
                                    </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="centeralign">
                                        <asp:ImageButton ID="imgmap" runat="server" Text="" CommandName="MapLocation" CommandArgument='<%# Eval("IPAddress") %>'
                                            ImageUrl="~/admin/Incentex_Used_Icons/map_blue.png" ToolTip="Get Map" Height="25"
                                            Width="25" class="btn_space"></asp:ImageButton>
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="10%" />
                            </asp:TemplateField>
                            <%--this is second grid for page history--%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    </td></tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td colspan="4">
                                            <asp:GridView ID="gvChPageHistory" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblsr" runat="server" Text="Sr." CssClass="centeralign"></asp:Label>
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
                                                            <span class="centeralign">Page Name</span>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPageName" Text='<% # (Convert.ToString(Eval("PagesName")).Length > 74) ? Eval("PagesName").ToString().Substring(0,74)+"..." : Convert.ToString(Eval("PagesName"))  %>'
                                                                ToolTip='<% #Eval("PagesName")  %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="b_box" Width="70%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <span class="centeralign">Time Accessed</span>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span class="first centeralign">
                                                                <%# Eval("DateTimePage") %></span>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="g_box" Width="20%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="alignright pagging" runat="server" id="pager">
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
                <%--popup for the map--%>
                <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                <at:ModalPopupExtender ID="modal" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
                    DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="closepopup">
                </at:ModalPopupExtender>
                <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
                    <div class="pp_pic_holder facebook" style="display: block; width: 1022px;left:10%;top:10%;position:fixed;">
                        <div class="pp_top" style="">
                            <div class="pp_left">
                            </div>
                            <div class="pp_middle">
                            </div>
                            <div class="pp_right">
                            </div>
                        </div>
                        <div class="pp_content_container" style="">
                            <div class="pp_left" style="">
                                <div class="pp_right" style="">
                                    <div class="pp_content" style="height: 656px; display: block;">
                                        <div class="pp_loaderIcon" style="display: none;">
                                        </div>
                                        <div class="pp_fade" style="display: block;">
                                            <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                            <div class="pp_hoverContainer" style="height: 92px; width: 742px; display: none;">
                                                <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                    style="visibility: visible;">previous</a>
                                            </div>
                                            <div id="pp_full_res">
                                                <div class="pp_inline clearfix">
                                                    <div class="form_popup_box">
                                                        <div>
                                                            <artem:GoogleMap ID="GoogleMap1" Width="920px" Height="600px" runat="server" IsSensor="false"
                                                                EnableInfoWindow="true" EnableMarkerManager="false" Latitude="50.72870" Longitude="-1.85046"
                                                                Zoom="10" EnableScrollWheelZoom="true" BorderWidth="2">
                                                            </artem:GoogleMap>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="pp_details clearfix" style="width: 742px;">
                                                <a href="#" id="closepopup" runat="server" class="pp_close">Close</a>
                                                <p class="pp_description" style="display: none;">
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="pp_bottom" style="">
                            <div class="pp_left" style="">
                            </div>
                            <div class="pp_middle" style="">
                            </div>
                            <div class="pp_right" style="">
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
