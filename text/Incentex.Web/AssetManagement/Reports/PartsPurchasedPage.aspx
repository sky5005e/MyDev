<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="PartsPurchasedPage.aspx.cs" Inherits="AssetManagement_Reports_PartsPurchasedPage" Title="Parts Purchased Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type ="text/css">
        .form_popup_box span.error
        {
        	padding:0px;
        }
    </style>
    <script type="text/javascript">
        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleContainerContent").hide();
        });
    </script>   

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div>
            <div style="text-align: center">
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <asp:Repeater ID="rptProductCategory" runat="server" OnItemDataBound="rptProductCategory_ItemDataBound">
                <ItemTemplate>
                    <div  class="collapsibleContainer" 
                    title="<%# DataBinder.Eval(Container.DataItem, "JobCodeName")%>" total='<%# DataBinder.Eval(Container.DataItem, "Amount")%>' >
                        <div style="text-align: center">
                            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                        </div>
                        <asp:HiddenField ID="hdnJobCodeID" Value='<%# DataBinder.Eval(Container.DataItem, "JobCodeID")%>'
                            runat="server"></asp:HiddenField>
                        <div>
                            <div style="text-align: center; color: Red; font-size: larger;">
                                <asp:Label ID="lblMsgGrid" runat="server">
                                </asp:Label>
                            </div>
                            <div>
                                <asp:GridView ID="gvEquipment" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvEquipment_RowDataBound"
                                    OnRowCommand="gvEquipment_RowCommand" ShowFooter="True">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtVendor" runat="server" CommandArgument="Vendor" CommandName="Sort"><span class="centeralign">Vendor</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderVendor" runat="server"></asp:PlaceHolder>
                                                <div class="corner">
                                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl centeralign"></span>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfVendor" runat="server" Value='<%# Eval("Vendor")%>' />
                                                <asp:Label runat="server" ID="lblVendor" Text='<%# "&nbsp;" + Eval("Vendor") %>'></asp:Label>                                                                                                                                      
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="DateofService">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnDateofService" runat="server" CommandArgument="DateofService"
                                                    CommandName="Sort"><span class="centeralign">Date of Service</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderDateofService" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfDateofService" runat="server" Value='<%# Eval("DateofService")%>' />
                                                <asp:Label runat="server" ID="lblDateofService" Text='<%# "&nbsp;" + Eval("DateofService", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Invoice">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnInvoice" runat="server" CommandArgument="Invoice" CommandName="Sort"><span class="centeralign">Invoice #</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderInvoice" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfInvoice" runat="server" Value='<%# Eval("InvoiceNumber")%>' />
                                                <asp:Label runat="server" ID="lblInvoice" Text='<%# "&nbsp;" + Eval("InvoiceNumber")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="15%" />
                                        </asp:TemplateField>                                      
                                        <asp:TemplateField SortExpression="Amount">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnAmount" runat="server" CommandArgument="Amount" CommandName="Sort"><span class="centeralign">Amount</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderAmount" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfAmount" runat="server" Value='<%# Eval("Amount")%>' />
                                                <asp:Label runat="server" ID="lblAmount" Text='<%# Eval("Amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box rightalign" Width="15%" />
                                             </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <span class="centeralign">File</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class="btn_space">
                                                    <asp:ImageButton ID="lnkbtnPDF" runat="server" CommandName="PDF" CommandArgument='<%# Eval("File") %>' /></span>
                                                     <asp:HiddenField ID="hfPDF" runat="server" Value='<%# Eval("File")%>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                </asp:GridView>
                            </div>                                           
                    <%--<div id="pagingtable" runat="server" class="alignright pagging">
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
                    </div>--%>
                        </div>
                    </div>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
        </div>      
    </div>
</asp:Content>


