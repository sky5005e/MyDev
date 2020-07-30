<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ListofHeadsetRepairCenter.aspx.cs" Inherits="HeadsetRepairCenter_ListofHeadsetRepairCenter" %>

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
    <asp:PlaceHolder runat="server" ID="plhPlayVideo"></asp:PlaceHolder>
    <div class="form_pad" id="dvList" runat="server">
        <div style="text-align: center; color: Red; font-size: larger;">
            <asp:Label ID="lblmsg" runat="server">
            </asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="grdView_RowCommand"
            OnRowDataBound="grdView_RowDataBound">
            <EmptyDataTemplate>
                <div style="text-align: center; color: Red; font-size: larger;">
                    No records found for given criteria.
                </div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField SortExpression="RepairNumber">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkRepairNumber" runat="server" CommandArgument="RepairNumber"
                            CommandName="Sort">
                        <span>Repair Number</span></asp:LinkButton>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="first">
                            <asp:LinkButton ID="lblRepairNumber" runat="server" CommandArgument='<%# Eval("RepairNumber") %>'
                                CommandName="RepairNumber">
                               <span><%# "HR" + Eval("RepairNumber") %></span>
                            </asp:LinkButton>
                        </div>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="CompanyName">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkCompany" runat="server" CommandArgument="CompanyName" CommandName="Sort"><span>Company </span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCompany" Text='<%# Eval("CompanyName") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Date">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkDate" runat="server" CommandArgument="Date" CommandName="Sort"><span>Submit Date</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDate" Text='<%# Eval("Date") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="OrderTrackingNumber">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkOrderTrackingNumber" runat="server" CommandArgument="OrderTrackingNumber"
                            CommandName="Sort"><span>Order Tracking Number</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:HyperLink runat="server" ID="lblOrderTrackingNumber" NavigateUrl="http://www.ups.com/tracking/tracking.html"
                                Target="_blank" Text='<%# Eval("OrderTrackingNumber") + "&nbsp;" %> ' />
                        </span>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box centeralign" />
                    <HeaderStyle CssClass="centeralign" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Is Approved Quote</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:Image alt="&nbsp;" ID="imgQuote" runat="server" Width="26px" Height="26px" />
                            <asp:Label runat="server" Visible="false" ID="lblIsCustomerApprovedQuote" Text='<%# Eval("IsCustomerApprovedQuote") + "&nbsp;" %> ' />
                            <asp:HiddenField ID="hdnRequestquotebeforerepair" runat="server" Value='<%# Eval("Requestquotebeforerepair") %>' />
                        </span>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box centeralign" />
                    <HeaderStyle CssClass="centeralign" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Delete</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:LinkButton ID="lnkbtndelete" CommandName="DeleteHeadsetrepair" OnClientClick="return Loadloder();"
                                CommandArgument='<%# Eval("HeadsetRepairID") %>' runat="server" CssClass="btn_space">
                                <img alt="X" id="delete" src="~/Images/close.png" runat="server" /></asp:LinkButton>
                        </span>
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box centeralign" />
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
