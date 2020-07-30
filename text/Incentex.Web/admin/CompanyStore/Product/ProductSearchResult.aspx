<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ProductSearchResult.aspx.cs" Inherits="admin_CompanyStore_Product_ProductSearchResult"
    Title="Product Search Result" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="cthead">

    <script type="text/javascript">
    $(document).ready(function(){
        $("tr.ord_content").each(function(){
            $(this).children("td:even").addClass("b_box").removeClass("g_box");
            $(this).children("td:odd").addClass("g_box").removeClass("b_box");
        });
        //$("tr.ord_content").children("td:odd").addClass("b_box");
    });
    </script>

    <style type="text/css">
        .centeralign
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="50%" runat="server" id="tdCompany">
                        <div style="text-align: left; color: #5C5B60; font-size: larger; font-weight: bold;
                            padding-left: 15px;">
                            Company Name :
                            <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                        </div>
                    </td>
                    <td width="50%" runat="server" id="tdReords">
                        <div style="text-align: right; color: #5C5B60; font-size: larger; font-weight: bold;
                            padding-left: 15px;">
                            Total Records :
                            <asp:Label ID="lblRecords" runat="server"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="alignnone spacer15">
            </div>
            <div class="form_pad" style="padding-top: 0px !important;">
                <div>
                    <div style="text-align: center; color: Red; font-size: larger;">
                        <asp:Label ID="lblmsg" runat="server">
                        </asp:Label>
                    </div>
                    <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" ShowFooter="true" GridLines="None" RowStyle-CssClass="ord_content"
                        OnRowCommand="gvProduct_RowCommand">
                        <EmptyDataTemplate>
                            <div style="text-align: center; color: Red; font-size: larger;">
                                No records found for given criteria.
                            </div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnHeaderStoreName" runat="server" CommandArgument="StoreName"
                                        CommandName="Sort"><span >Company Name</span></asp:LinkButton>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="first">
                                        <asp:LinkButton ID="lnkbtnStoreName" CommandArgument='<%# Eval("StoreProductID")%>'
                                            CommandName="Detail" runat="server" ToolTip='<%# Eval("StoreName")%>'><%# Convert.ToString(Eval("StoreName")).Length > 25 ? Convert.ToString(Eval("StoreName")).Substring(0, 25).Trim() + "..." : Convert.ToString(Eval("StoreName"))%></asp:LinkButton>
                                    </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                                <HeaderStyle Width="25%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                        CommandName="Sort"><span>Item Number</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>
                                        <%--<asp:Label runat="server" ID="lblItemNumber" Text='<%# Convert.ToString(Eval("ItemNumber")).Length > 25 ? Convert.ToString(Eval("ItemNumber")).Substring(0, 25).Trim() + "..." : Convert.ToString(Eval("ItemNumber")) %>'
                                        ToolTip='<%# Eval("ItemNumber") %>' />--%>
                                        <asp:LinkButton ID="lnkbtnItemNumber" CommandArgument='<%# Eval("StoreProductID")%>'
                                            CommandName="Detail" runat="server" ToolTip='<%# Eval("ItemNumber")%>'><%# Convert.ToString(Eval("ItemNumber")).Length > 25 ? Convert.ToString(Eval("ItemNumber")).Substring(0, 25).Trim() + "..." : Convert.ToString(Eval("ItemNumber"))%></asp:LinkButton>
                                            <asp:HiddenField runat="server" ID="hdnStoreID" Value='<%# Eval("StoreID")%>' />
                                        <asp:HiddenField runat="server" ID="hdnWorkgroupName" Value='<%# Eval("WorkgroupName")%>' />
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" />
                                <HeaderStyle Width="25%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnProductDescrption" runat="server" CommandArgument="ProductDescrption"
                                        CommandName="Sort"><span>Product Description</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblProductDescription" Text='<%# Convert.ToString(Eval("ProductDescription")).Length > (DescSubStringLength) ? Convert.ToString(Eval("ProductDescription")).Substring(0, DescSubStringLength).Trim() + "..." : Convert.ToString(Eval("ProductDescription")) %>'
                                        ToolTip='<%# Eval("ProductDescription") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                                <HeaderStyle Width="50%" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnVendor" runat="server" CommandArgument="Vendor" CommandName="Sort"><span>Vendor</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblVendor" Text='<%# Eval("Vendor") + "&nbsp;" %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="g_box" />
                                <HeaderStyle Width="25%" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnLevel1" runat="server" CommandArgument="Level1" CommandName="Sort"><span>L1</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLevel1" Text='<%# Eval("Level1") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="b_box centeralign" />
                                <HeaderStyle Width="5%" CssClass="centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnLevel2" runat="server" CommandArgument="Level2" CommandName="Sort"><span>L2</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLevel2" Text='<%# Eval("Level2") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="g_box centeralign" />
                                <HeaderStyle Width="5%" CssClass="centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnLevel3" runat="server" CommandArgument="Level3" CommandName="Sort"><span>L3</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLevel3" Text='<%# Eval("Level3") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="b_box centeralign" />
                                <HeaderStyle Width="5%" CssClass="centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnLevel4" runat="server" CommandArgument="Level4" CommandName="Sort"><span>L4</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLevel4" Text='<%# Eval("Level4") + "&nbsp;" %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="g_box centeralign" />
                                <HeaderStyle Width="5%" CssClass="centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCloseout" runat="server" CommandArgument="CloseOut" CommandName="Sort"><span>Close Out</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCloseout" Text='<%# Eval("CloseOut") + "&nbsp;" %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="g_box centeralign" />
                                <HeaderStyle Width="10%" CssClass="centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnAllowBackOrder" runat="server" CommandArgument="AllowBackOrder"
                                        CommandName="Sort"><span>Allow for Back-Orders</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblAllowBackOrder" Text='<%# Eval("AllowBackOrder") + "&nbsp;" %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="g_box centeralign" VerticalAlign="Middle" />
                                <HeaderStyle Width="16%" CssClass="centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkMonthUsage" runat="server" CommandArgument="MonthUsage" CommandName="Sort"><span>Usage</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMonthUsage" Text='<%# Eval("MonthUsage") + "&nbsp;" %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="g_box centeralign" />
                                <HeaderStyle Width="5%" CssClass="centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkOnHand" runat="server" CommandArgument="OnHand" CommandName="Sort"><span>On-Hand</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOnHand" Text='<%# Eval("OnHand") + "&nbsp;" %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="g_box centeralign" />
                                <HeaderStyle Width="5%" CssClass="centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkOnOrder" runat="server" CommandArgument="OnOrder" CommandName="Sort"><span>On-Order</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOnOrder" Text='<%# Eval("OnOrder") + "&nbsp;" %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="g_box centeralign" />
                                <HeaderStyle Width="5%" CssClass="centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkDelieveryDate" runat="server" CommandArgument="DelieveryDate"
                                        CommandName="Sort"><span>Delivery Date</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDelieveryDate" Text='<%# (Eval("DelieveryDate") != null && Convert.ToString(Eval("DelieveryDate")) != "")? Convert.ToDateTime(Eval("DelieveryDate")).ToString("MM/dd/yyyy") + "&nbsp;" : "&nbsp;"%>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="g_box centeralign" />
                                <HeaderStyle Width="13%" CssClass="centeralign" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <div id="pagingtable" runat="server" class="alignright pagging">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click">
                        </asp:LinkButton>
                        <span>
                            <asp:DataList ID="DataList2" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="DataList2_ItemCommand"
                                OnItemDataBound="DataList2_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
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
    </div>
</asp:Content>
