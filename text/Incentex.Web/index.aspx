<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeFile="index.aspx.cs" Inherits="index" Title="Home Page" %>

<%@ Register Src="~/usercontrol/TrainingVideo.ascx" TagName="VideoUserControl" TagPrefix="vti" %>
<%@ Register Src="~/usercontrol/OpenServiceTicketCE.ascx" TagName="OpenServiceTicketCE"
    TagPrefix="ostCE" %>
<%@ Register Src="~/usercontrol/OpenServiceTicketCA.ascx" TagName="OpenServiceTicketCA"
    TagPrefix="ostCA" %>
<%@ Register Src="~/usercontrol/OpenServiceticketAnonymous.ascx" TagName="OpenServiceticketAnonymous"
    TagPrefix="ostAnn" %>
<%@ Register Src="~/usercontrol/ReplacementUniformsCA.ascx" TagName="ReplacementUniformsCA"
    TagPrefix="ruCA" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $().ready(function() {
            //Load the slideshow
            theRotator();
        });
    </script>

    <vti:VideoUserControl runat="server" ID="VideoControlIndexPage" IsForGeneralVideo="true"
        Visible="false" />
    <ostCE:OpenServiceTicketCE ID="ostCEControl" runat="server" Visible="false" />
    <ostCA:OpenServiceTicketCA ID="ostCAControl" runat="server" Visible="false" />
    <ostAnn:OpenServiceticketAnonymous ID="ostAnnControl" runat="server" Visible="false" />
    <ruCA:ReplacementUniformsCA ID="ruCAControl" runat="server" Visible="false" />
    <div id="header" class=" clearfix">
        <div class="black2_round_top">
            <span></span>
        </div>
        <%--Second Top Navigation Menu for the Company Admins--%>
        <div class="top_navigation" id="dvSecondTopNavigation" runat="server" visible="false">
            <ul>
                <asp:Repeater ID="topSecondNavigation" runat="server" OnItemDataBound="topSecondNavigation_ItemDataBound"
                    OnItemCommand="topSecondNavigation_ItemCommand">
                    <ItemTemplate>
                        <li id="lis" runat="server">
                            <asp:HyperLink ID="lnkMenu" runat="server" Text='<%# Eval("sDescription") %>' NavigateUrl='<%# Eval("PageUrl") %>'
                                Style="margin: 0 10px;" CommandArgument='<%# Eval("iMenuPrivilegeID") %>'></asp:HyperLink>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div class="spacer10">
        </div>
        <%--end--%>
        <div class="top_navigation" id="dvTopNavigation" runat="server">
            <ul>
                <asp:Repeater ID="topNavigation" runat="server" OnItemDataBound="topNavigation_ItemDataBound"
                    OnItemCommand="topNavigation_ItemCommand">
                    <ItemTemplate>
                        <li id="lis" runat="server">
                            <asp:HyperLink ID="lnkMenu" runat="server" Text='<%# Eval("sDescription") %>' NavigateUrl='<%# Eval("PageUrl") %>'
                                CommandArgument='<%# Eval("iMenuPrivilegeID") %>'></asp:HyperLink>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>
    <%--</div>
        </div>--%>
    <div class="clearfix content">
        <div class="alignleft narrowcolumn">
            <div class="category_menu" id="dvLocationA" runat="server">
                <h2>
                    <span id="spnLocationA" runat="server"></span>
                </h2>
                <ul>
                    <asp:Repeater ID="rptLocationA" runat="server" OnItemDataBound="rptLocation_ItemDataBound"
                        OnItemCommand="rptLocation_ItemCommand">
                        <ItemTemplate>
                            <li id="liLocation" runat="server">
                                <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Eval("SubCategoryName") %>'
                                    CommandName="ListProduct" CommandArgument='<%# Eval("SubCategoryID") %>'></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <%--Special Div Created By Ankit on 9th April..If user have no Data selected from the admin bar--%>
            <div style="margin-bottom: 5px" id="dvLocationAEmpty" runat="server" visible="false">
            </div>
            <%--End--%>
            <div class="category_menu" id="dvLocationB" runat="server">
                <h2>
                    <span id="spnLocationB" runat="server"></span>
                </h2>
                <ul>
                    <asp:Repeater ID="rptLocationB" runat="server" OnItemDataBound="rptLocation_ItemDataBound"
                        OnItemCommand="rptLocation_ItemCommand">
                        <ItemTemplate>
                            <li id="liLocation" runat="server">
                                <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Eval("SubCategoryName") %>'
                                    CommandName="ListProduct" CommandArgument='<%# Eval("SubCategoryID") %>'></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
        <div class="alignleft widecolumn">
            <div class="gray_round_box">
                <div class="gray_round_top">
                    <span></span>
                </div>
                <div class="gray_round_middle">
                    <%-- <img src="images/middle_bannersmall.jpg" />--%>
                    <div id="rotator" runat="server">
                    </div>
                </div>
                <div class="gray_round_bottom">
                    <span></span>
                </div>
            </div>
        </div>
        <div class="alignright narrowcolumn">
            <div class="category_menu" id="dvAdditionalInfo" runat="server">
                <h2>
                    <span>Additional Information</span></h2>
                <ul>
                    <asp:Repeater ID="rptAddiInfo" runat="server" OnItemDataBound="rptAddiInfo_ItemDataBound"
                        OnItemCommand="rptAddiInfo_ItemCommand">
                        <ItemTemplate>
                            <li id="AddiInfolis" runat="server">
                                <asp:LinkButton ID="lnkAddiInfo" runat="server" Text='<%# Eval("sLookupName") %>'
                                    CommandArgument='<%# Eval("sLookupName") %>'></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <div class="category_menu" id="dvLocationC" runat="server">
                <h2>
                    <span id="spnLocationC" runat="server"></span>
                </h2>
                <ul>
                    <asp:Repeater ID="rptLocationC" runat="server" OnItemDataBound="rptLocation_ItemDataBound"
                        OnItemCommand="rptLocation_ItemCommand">
                        <ItemTemplate>
                            <li id="liLocation" runat="server">
                                <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Eval("SubCategoryName") %>'
                                    CommandName="ListProduct" CommandArgument='<%# Eval("SubCategoryID") %>'></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <div class="category_menu" id="dvLocationD" runat="server">
                <h2>
                    <span id="spnLocationD" runat="server"></span>
                </h2>
                <ul>
                    <asp:Repeater ID="rptLocationD" runat="server" OnItemDataBound="rptLocation_ItemDataBound"
                        OnItemCommand="rptLocation_ItemCommand">
                        <ItemTemplate>
                            <li id="liLocation" runat="server">
                                <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Eval("SubCategoryName") %>'
                                    CommandName="ListProduct" CommandArgument='<%# Eval("SubCategoryID") %>'></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <div id="dvGuideLineManuals" runat="server">
                <asp:Repeater ID="rpuGuideLineManuals" runat="server" OnItemDataBound="rpuGuideLineManuals_ItemDataBound"
                    OnItemCommand="rpuGuideLineManuals_ItemCommand">
                    <ItemTemplate>
                        <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("DocumentName") %>' Visible="false"></asp:Label>
                        <asp:LinkButton ID="lnkrpuGuideLineManuals" CssClass="btn_pdf" CommandName="viewguidelinemanual"
                            runat="server" Text='<%# Eval("DocumentName") %>' CommandArgument='<%# Eval("StoreDocumentID") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <%--<a href="#" class="btn_pdf" title="Uniform Guidelines Manuel">Uniform Guidelines Manuel</a>--%>
        </div>
    </div>
</asp:Content>
