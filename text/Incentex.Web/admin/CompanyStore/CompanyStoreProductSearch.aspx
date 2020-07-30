<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="CompanyStoreProductSearch.aspx.cs" Inherits="admin_CompanyStore_CompanyStoreProductSearch" Title="Company Storeprodcut Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <asp:ScriptManager ID="srcmgr" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="form_pad">
                <h4>
                   Company Store Product Search</h4>
                <div>
                    <table class="form_table">
                        <tr>
                            <td class="formtd">
                                <asp:UpdatePanel ID="upUserType" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Company Store</span>
                                                 <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlComapnyStore" TabIndex="0" runat="server" onchange="pageLoad(this,value);"
                                                        AutoPostBack="True" >
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </Conte
                                    
                                  </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td class="formtd">
                                <table>
                                    <tr>
                                        <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Master Item Number</span>
                                                 <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlMasterItemNo" TabIndex="1" runat="server" onchange="pageLoad(this,value);"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlMasterItemNo_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">KeyWord</span>
                                                    <asp:TextBox ID="txtProductName" TabIndex="2" runat="server" class="w_label"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                    
                                </table>
                            </td>
                            <td class="formtd">
                                <table>
                                   
                                    <tr>
                                         <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Item Number</span> <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlItemNo" TabIndex="2" runat="server" onchange="pageLoad(this,value);"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                                    </tr>
                                     <tr>
                                        <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Style Number</span> <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlStyleNo" TabIndex="4" runat="server" onchange="pageLoad(this,value);"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                                    </tr>
                                </table>
                            </td>
                           
                        </tr>
                    </table>
                </div>
                <div class="botbtn centeralign">
                <asp:UpdatePanel runat="server" ID="upnlSearchbtn">
                <ContentTemplate>
               
                    <asp:LinkButton CssClass="grey2_btn" ID="btnSearch" runat="server" TabIndex="7" OnClick="btnSearch_Click"><span>Search</span></asp:LinkButton>
                    <asp:LinkButton CssClass="grey2_btn" ID="btnClear" runat="server" OnClick="btnClear_Click"><span>Clear</span></asp:LinkButton>
                     </ContentTemplate>
                </asp:UpdatePanel>
                </div>
                <div class="form_pad">
                    <div style="text-align: center">
                        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                    </div>
                    <asp:GridView ID="grdStoreProductSearch" runat="server" 
                        AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" 
                        RowStyle-CssClass="ord_content" onrowcommand="grdStoreProductSearch_RowCommand" onrowdatabound="grdStoreProductSearch_RowDataBound" 
                       >
                        <Columns>
                       
                             <asp:TemplateField SortExpression="MasterNo">
                                        <HeaderTemplate>
                                            <span>
                                                <asp:LinkButton ID="lnkbtnMasterNo" runat="server" CommandArgument="MasterNo"
                                                    CommandName="Sort">Master #</asp:LinkButton>
                                                     </span>
                                                <asp:PlaceHolder ID="placeholderMasterNo" runat="server"></asp:PlaceHolder>
                                               <div class="corner">
                                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                </div>
                                            </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="first">
                                                <asp:LinkButton ID="hypStyle" CommandName="Edit" runat="server" ToolTip="Edit the record"><%# Eval("MasterNo")%></asp:LinkButton>
                                            </span>
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="20%" />
                                    </asp:TemplateField>
                             <asp:TemplateField SortExpression="CompanyName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtComapanyName" runat="server" CommandArgument="CompanyName"
                                        CommandName="Sort"><span class="white_co">Store Name</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderComapany" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompanyName" Text='<%# Eval("CompanyName") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                            </asp:TemplateField>
                             <asp:TemplateField SortExpression="StoreStatus">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnStoreStatus" runat="server" CommandArgument="StoreStatus" CommandName="Sort"><span>Store Status</span></asp:LinkButton>
                                   <asp:PlaceHolder ID="placeholderStoreStatus" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:Label runat="server" ID="lblStoreStatus" Text='<%# Eval("StoreStatus") %>' />
                                   </ItemTemplate>
                                <ItemStyle CssClass="b_box" />
                            </asp:TemplateField>
                             <asp:TemplateField SortExpression="ProductDescrption">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtProductDescrption" runat="server" CommandArgument="ProductDescrption"
                                        CommandName="Sort"><span class="white_co">Product Description</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderProductDescrption" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblProductDescrption" Text='<%# Eval("ProductDescrption") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" />
                            </asp:TemplateField>
                             <asp:TemplateField SortExpression="ProductStatus">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnProductStatus" runat="server" CommandArgument="ProductStatus" CommandName="Sort"><span class="white_co">Product Status</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderProductStatus" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    
                                    <asp:HiddenField ID="hdnStatusID" runat="server"  Value='<%# DataBinder.Eval(Container.DataItem, "ProductStatus")%>' />
                                    <span class="btn_space" style="text-align:center;"><img id="imgLookupIcon" style="height:20px;width:20px"  runat="server" alt='Loading' /></span>
                                    <%--<asp:Label runat="server" ID="lblProductStatus" Text='<%# Eval("ProductStatus") %>' />--%>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                            </asp:TemplateField>
                          </Columns>
                    </asp:GridView>
                    <div id="divPaging" runat="server">
                        <div>
                            <div class="companylist_botbtn">    
                                <div class="alignright pagging" id="dvPager" runat="server">
                                    <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"></asp:LinkButton>
                                    <span>
                                        <asp:DataList ID="dtlViewUsers" runat="server" CellPadding="1" CellSpacing="1" 
                                             RepeatDirection="Horizontal" RepeatLayout="Flow" 
                                        onitemcommand="dtlViewUsers_ItemCommand" 
                                        onitemdatabound="dtlViewUsers_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </span>
                                    <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>