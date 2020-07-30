<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EmployeeProfile.aspx.cs" Inherits="MyAccount_CompanyProgram_EmployeeProfile"
    Title="Employee Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div>
            <div class="black_top_co">
                <span></span>
            </div>
            <div class="black_middle">
                <div>
                    <div class="alignleft" style="width: 49%;">
                        <div class="tab_content_top_co">
                            <span>&nbsp;</span></div>
                        <div class="tab_content profile_table">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <label>
                                            Employee Name :</label>
                                        <asp:Label ID="lblEmployeeName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Address :</label>
                                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Country :</label>
                                        <asp:Label ID="lblCountry" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            State :</label>
                                        <asp:Label ID="lblState" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            City :</label>
                                        <asp:Label ID="lblCity" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Zip :</label>
                                        <asp:Label ID="lblZip" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Telephone :</label>
                                        <asp:Label ID="lblTelephone" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Mobile :</label>
                                        <asp:Label ID="lblMobile" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="tab_content_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                    <div class="alignright" style="width: 49%;">
                        <div class="tab_content_top_co">
                            <span>&nbsp;</span></div>
                        <div class="tab_content profile_table">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <label>
                                            Date of Hire :</label>
                                        <asp:Label ID="lblDOH" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Employee Number :</label>
                                        <asp:Label ID="lblEmployeeID" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="profile_inn_content">
                                        <table>
                                            <tr>
                                                <td>
                                                    <label>
                                                        Status:</label>
                                                </td>
                                                <td style="width: 60%;">
                                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                  
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="alignleft">
                                            Anniversary Credit Balance :</label>
                                        <div class="alignleft" style="display: inline-block; width: 40%;">
                                            $
                                            <asp:Label ID="lblCurrentCreditBalance" runat="server"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="tab_content_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                    <div class="alignnone">
                    </div>
                </div>
            </div>
            <div class="black_bot_co">
                <span></span>
            </div>
        </div>
        <div class="profile_devider">
            &nbsp;</div>
        <div class="headersmall_bg">
            <div class="headersmall_bgr title">
                Employee Order History</div>
        </div>
        <div class="spacer10">
        </div>
        <asp:ScriptManager ID="src" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upGrid" runat="server">
            <ContentTemplate>
                <div>
                    <div class="tab_content_top_co">
                        <span>&nbsp;</span></div>
                    <div class="tab_content viewpro_tab">
                        <table cellpadding="0" cellspacing="0">
                            <div style="text-align: center; color: Red; font-size: larger;">
                                <asp:Label ID="lblMessage" runat="server">
                            No Record Found
                                </asp:Label>
                            </div>
                            <asp:GridView ID="gvEmployeeOrderAnniversary" runat="server" AutoGenerateColumns="false"
                                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                                RowStyle-CssClass="ord_content" 
                                OnRowCommand="gvEmployeeOrderAnniversary_RowCommand" onrowdatabound="gvEmployeeOrderAnniversary_RowDataBound" 
                                >
                                <Columns>
                                    <asp:TemplateField Visible="false" HeaderText="Id">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblOrderID" Text='<%# Eval("OrderID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Check">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnTrasHistory" runat="server" CommandArgument="TransHistory"
                                                CommandName="Sort"><span class="white_co">Order Number</span></asp:LinkButton>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblOrderNumber" Text='<%# Eval("OrderNumber") %>' />
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" CssClass="g_box centeralign" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Country">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnDate" runat="server" CommandArgument="OrderDate" CommandName="Sort"><span class="white_co">Date</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblOrderDate" Text='<%# Convert.ToDateTime(Eval("OrderDate")).ToShortDateString() %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ReferenceName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkReferenceName" runat="server" CommandArgument="OrderDate"
                                                CommandName="Sort"><span class="white_co">Reference Name</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblReferenceNameView" Text='<%#Eval("ReferenceName").ToString() != "" ? Eval("ReferenceName"): "---" %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="OrderAmount">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnOrderAmount" runat="server" CommandArgument="OrderAmount"
                                                CommandName="Sort">
                                <span>Total Order Amount</span>
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblOrderAmount" Text='<%# Eval("TotalAmount") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="15%" />
                                    </asp:TemplateField>
                                    <%--   <asp:TemplateField SortExpression="Payment Method">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnPaymentMethod" runat="server" CommandArgument="OrderAmount"
                                                CommandName="Sort">
                                <span>Payment Method</span>
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPayMethod" Text='<%#Eval("PaymentMethod").ToString() != "" ? Eval("PaymentMethod").ToString():"No Payment Method"%>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="25%" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField SortExpression="CreditAmount">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnCreditAmount" runat="server" CommandArgument="OrderAmount"
                                                CommandName="Sort">
                                <span>Credit Type</span>
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCreditUsed" Text='<%#Eval("CreditUsed")%>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="OrderAmount">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnOrderAmount" runat="server" CommandArgument="OrderAmount"
                                                CommandName="Sort">
                                                <span>Details</span>
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_box" >
                                                <asp:LinkButton ID="lnkRedirect" CssClass="greysm_btn" style="color:White;" runat="server" CommandArgument='<%# Eval("OrderID") %>'
                                                    CommandName="OrderDetail"><span style="color:White;">
                                View Order Details</span></asp:LinkButton>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" ForeColor="White" Width="20%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div id="pagingtable" runat="server" class="alignright pagging">
                                <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server"> 
                                </asp:LinkButton>
                                <span>
                                    <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:DataList></span>
                                <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server"></asp:LinkButton>
                            </div>
                        </table>
                    </div>
                    <div class="tab_content_bot_co">
                        <span>&nbsp;</span></div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
