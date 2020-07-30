<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="PurchaseOrderList.aspx.cs" Inherits="admin_PurchaseOrderManagement_PurchaseOrderList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
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
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="grdView_RowDataBound"
            OnRowCommand="grdView_RowCommand">
            <EmptyDataTemplate>
                <div style="text-align: center; color: Red; font-size: larger;">
                    No records found for given criteria.
                </div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPurchaseOrderID" Text='<%# Eval("PurchaseOrderID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="purchaseordernumber">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkPONumber" runat="server" CommandArgument="purchaseordernumber"
                            CommandName="Sort"><span>PO Number</span></asp:LinkButton>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="first">
                            <span>
                                <asp:LinkButton runat="server" ID="lblPONumber" CommandArgument='<%# Eval("PurchaseOrderID") %>'
                                    CommandName="ViewOrderDetails" Text='<%# Eval("purchaseordernumber") %>' /></span>
                        </div>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="OrderedBy">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkOrderedBy" runat="server" CommandArgument="OrderedBy" CommandName="Sort"><span>Ordered By</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblUplodedDate" Text='<%# Eval("OrderedBy") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="OrderedOn">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkOrderedOn" runat="server" CommandArgument="OrderedOn" CommandName="Sort"><span>Ordered On</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblOrderedOn" Text='<%# Eval("OrderedOn") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="DeliveryDate">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkDeliveryDate" runat="server" CommandArgument="DeliveryDate"
                            CommandName="Sort"><span>Confirmed Delivery</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDeliveryDate" Text='<%# !string.IsNullOrEmpty(Eval("DeliveryDate").ToString()) ? DateTime.Parse(Eval("DeliveryDate").ToString()).ToString("dd/MM/yyyy") : "&nbsp;" %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span class="white_co">File</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:HiddenField ID="hdnfilename" runat="server" Value='<%# Eval("FileName") %>' />
                            <asp:HiddenField ID="hdnOriginalFileName" runat="server" Value='<%# Eval("OriginalFileName") %>' />
                            <asp:HiddenField ID="hdnextension" runat="server" Value='<%# Eval("extension") %>' />
                            <asp:LinkButton ID="lnkviewdocument" runat="server" CommandArgument='<%# Eval("PurchaseOrderID") %>'
                                CssClass="btn_space" ToolTip="view file" CommandName="ViewOrder">
                                <img alt="" id="viewimg" runat="server" />
                            </asp:LinkButton>&nbsp; </span>
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
                            <asp:LinkButton ID="lnkbtndelete" CommandName="Deleteorder" OnClientClick="return Loadloder();"
                                CommandArgument='<%# Eval("PurchaseOrderID") %>' runat="server" CssClass="btn_space">
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
