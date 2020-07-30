<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ListWorldwideProspects.aspx.cs" Inherits="WorldwideProspect_ListWorldwideProspects" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function Loadloder() {
            if (confirm('Are you sure, you want to delete selected item?')) {
                $('#dvLoader').show();
                return true;
            }
            else {
                return false;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad" id="dvList" runat="server">
        <div style="text-align: center; color: Red; font-size: larger;">
            <asp:Label ID="lblmsg" runat="server">
            </asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="grdView_RowCommand">
            <EmptyDataTemplate>
                <div style="text-align: center; color: Red; font-size: larger;">
                    No records found for given criteria.
                </div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField SortExpression="CompanyName">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkCompanyName" runat="server" CommandArgument="CompanyName"
                            CommandName="Sort">
                        <span>Company</span></asp:LinkButton>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="first">
                            <asp:LinkButton ID="lnkcompanyname" runat="server" CommandName="CompanyName" CommandArgument='<%# Eval("ProspectID") %>' >
                              <span><%# Eval("CompanyName") %></span>
                            </asp:LinkButton>
                        </div>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="ContactName">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkCompany" runat="server" CommandArgument="ContactName" CommandName="Sort"><span>Contact</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblContact" Text='<%# Eval("ContactName") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Email">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkEmail" runat="server" CommandArgument="Email" CommandName="Sort"><span>Email</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEmail" Text='<%# Eval("Email") + "&nbsp;" %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="BusinessType">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkBusinessType" runat="server" CommandArgument="BusinessType"
                            CommandName="Sort"><span>Business Type</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblBusinessType" Text='<%# Eval("BusinessType") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Country</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCountry" Text='<%# Eval("Country") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Delete</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:LinkButton ID="lnkbtndelete" CommandName="DeleteProspect" OnClientClick="return Loadloder();"
                                CommandArgument='<%# Eval("ProspectID") %>' runat="server" CssClass="btn_space">
                                <img alt="X" id="delete" src="~/Images/close.png" runat="server" /></asp:LinkButton>
                        </span>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box centeralign" />
                    <HeaderStyle CssClass="centeralign" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
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
</asp:Content>
