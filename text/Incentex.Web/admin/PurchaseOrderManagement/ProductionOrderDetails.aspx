<%@ Page Title="Production Order Details" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="ProductionOrderDetails.aspx.cs" Inherits="admin_PurchaseOrderManagement_ProductionOrderDetails" %>

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
        <asp:GridView ID="grdViewTP" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="grdViewTP_RowDataBound">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPurchaseOrderID" Text='<%# Eval("PurchaseOrderID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="PONumber">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkPONumber" runat="server" CommandName="Sort"><span>Our PO Number</span></asp:LinkButton>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span class="first btn_space">
                            <asp:HyperLink ID="lnkPONumber" runat="server" NavigateUrl='<%# "~/admin/PurchaseOrderManagement/VendorPODetails.aspx?POID=" + Eval("PurchaseOrderID").ToString()%>'
                                Text='<%# Eval("purchaseordernumber") %>'></asp:HyperLink>
                        </span>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                     <ItemStyle CssClass="b_box centeralign" Width="12%" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Decorating">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkDecorating" runat="server" CommandName="Sort"><span>Decorating</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDecorating" Text="test" />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box"  Width="25%"/>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="PCS">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkPCS" runat="server" CommandName="Sort"><span>PCS</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPCS" Text='<%#Eval("RcvQuantity") %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box"  Width="20%"/>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Status">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkStatus" runat="server" CommandName="Sort"><span>Status</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("Status").ToString().Length == 0 ? "..." : Eval("Status")  %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box"  Width="10%"/>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span class="white_co">File</span>
                         <div class="corner">
                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span class="btn_space">
                            <asp:HiddenField ID="hdnfilename" runat="server" Value='<%# Eval("FileName") %>' />
                            <asp:HiddenField ID="hdnOriginalFileName" runat="server" Value='<%# Eval("OriginalFileName") %>' />
                            <asp:HiddenField ID="hdnextension" runat="server" Value='<%# Eval("extension") %>' />
                            <asp:LinkButton ID="lnkviewdocument" runat="server" CommandArgument='<%# Eval("PurchaseOrderID") %>'
                                CssClass="btn_space" ToolTip="view file" CommandName="View">
                                <img alt="" id="viewimg" runat="server" />
                            </asp:LinkButton>&nbsp; </span>
                              <div class="corner">
                            <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                        </div>
                    </ItemTemplate>
                     
                    <ItemStyle CssClass="b_box centeralign" Width="15%"/>
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
