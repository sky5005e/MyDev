<%@ Page Title="All Session Records" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="AllSessionRecords.aspx.cs" Inherits="TrackingCenter_AllSessionRecords" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $().ready(function() {
            $(".action").click(function() {
                $('#dvLoader').show();
            });
        });
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>

    <script type="text/javascript">
        function PlaySession(siteid) {
            window.open(siteid, 'playvideo', 'width=420,height=500 ,scrollbars=yes');
        }
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div id="dvTemp" runat="server">
            <div style="text-align: center">
                <asp:Label ID="lblMsgGrid" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div style="text-align: right; padding-right: 30px; color: #72757C">
                <asp:Label ID="lblCount" runat="server"></asp:Label>
            </div>
            <div class="spacer25">
            </div>
            <asp:GridView ID="gvRecordingList" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content"
                OnRowDataBound="gvRecordingList_RowDataBound" OnRowCommand="gvRecordingList_RowCommand">
                <Columns>
                    <asp:TemplateField Visible="False" HeaderText="Id">
                        <HeaderTemplate>
                            <span>ID</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblRcdID" Text='<%# Eval("Id") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="1%" />
                    </asp:TemplateField>
                   
                    <asp:TemplateField SortExpression="CountryCode">
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnCounrty" runat="server" CommandArgument="CountryCode" CommandName="Sort">Country</asp:LinkButton>
                            </span>
                            <asp:PlaceHolder ID="placeholderCountryCode" runat="server"></asp:PlaceHolder>
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="btn_space first aligncenter">
                                <asp:Image ID="img" ImageUrl='<%# Eval("ImageSrc") %>' border="0" title='<%# Eval("CountryCode")%>'
                                    runat="server" />
                            </span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="3%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="CompanyName">
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnCompanyName" runat="server" CommandArgument="CompanyName" CommandName="Sort">CompanyName</asp:LinkButton>
                            </span>
                            <asp:PlaceHolder ID="placeholderCompanyName" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblPageCompanyName" Text='<%# Eval("CompanyName").ToString().Length>18 ? (Eval("CompanyName") as string).Substring(0,18)+".." : Eval("CompanyName")  %>'
                                ToolTip='<%# Eval("CompanyName")%>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="UserName">
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnUserName" runat="server" CommandArgument="UserName" CommandName="Sort">UserName</asp:LinkButton>
                            </span>
                            <asp:PlaceHolder ID="placeholderUserName" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblPageURL" Text='<%# Eval("UserName")  %>'
                                ToolTip='<%# Eval("UserName")%>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="VisitLength">
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnDuration" runat="server" CommandArgument="VisitLength" CommandName="Sort">Duration</asp:LinkButton>
                            </span>
                            <asp:PlaceHolder ID="placeholderVisitLength" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDuration" Text='<%# Eval("VisitLength")%>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Browser">
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnBrowserName" runat="server" CommandArgument="Browser"
                                    CommandName="Sort">Browser Name</asp:LinkButton>
                            </span>
                            <asp:PlaceHolder ID="placeholderBrowser" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblBrowser" Text='<%# Eval("Browser")%>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="OS">
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnOS" runat="server" CommandArgument="OS" CommandName="Sort">OS</asp:LinkButton>
                            </span>
                            <asp:PlaceHolder ID="placeholderOS" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblOS" Text='<%# Eval("OS")%>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span>Play</span>
                            <div class="corner">
                                <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="btn_space"><a id="lnkPlayUrl" runat="server" onclick="javascript:PlaySession(this.title);"
                                title='<%# Eval("PopUrl")%>'>
                                <asp:Image ID="img" ImageUrl="~/admin/Incentex_Used_Icons/play_16x16.png" Width="16"
                                    Height="16" border="0" title="Play" runat="server" />
                            </a></span>
                            <div class="corner">
                                <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box centeralign" Width="4%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
