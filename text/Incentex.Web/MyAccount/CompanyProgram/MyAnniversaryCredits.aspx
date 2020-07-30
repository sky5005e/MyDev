<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MyAnniversaryCredits.aspx.cs" Inherits="MyAccount_CompanyProgram_MyAnniversaryCredits"
    Title="My Anniversary Credits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad" style="padding-top: 20px !important;">
        <div id="startingcredittable" runat="server">
            <table class="orderreturn_box" cellpadding="0" cellspacing="0" style="display: none;">
                <tr>
                    <td colspan="3" align="right">
                        <span>
                            <asp:LinkButton ID="lnkTransactionLog" CssClass="greysm_btn" runat="server" OnClick="lnkTransactionLog_Click"><span style="color:White;"> 
                                View Transaction Log</span></asp:LinkButton>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr class="ord_header">
                    <td style="width: 35%;">
                        <span>My Starting Credit Details</span><div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </td>
                    <td style="width: 25%;">
                        <span class="white_co">Amount</span>
                    </td>
                    <td style="width: 40%;" colspan="2">
                        <span>Date</span>
                    </td>
                </tr>
                <tr class="ord_content">
                    <td class="g_box" style="width: 35%">
                        <span class="first">Starting Credit Amount: </span>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </td>
                    <td class="b_box" style="width: 25%">
                        <span>
                            <%if (hfStartingCreditCurrentBalance.Value != null && hfStartingCreditCurrentBalance.Value != "0" && hfStartingCreditCurrentBalance.Value != "")
                              { %>
                            $<%=hfStartingCreditCurrentBalance.Value%>
                            <%}
                              else
                              { %>
                            ---
                            <%} %>
                        </span>
                    </td>
                    <td class="g_box" colspan="2" style="width: 40%">
                        <span>
                            <%if (hfStartingCreditUpdatedDate.Value != null && hfStartingCreditUpdatedDate.Value != "1/1/0001")
                              { %>
                            <%=hfStartingCreditUpdatedDate.Value%>
                            <%}
                              else
                              { %>
                            ---
                            <%} %></span>
                    </td>
                </tr>
                <tr class="ord_content">
                    <td class="g_box" style="width: 35%">
                        <span class="first">Starting Credit Balance: </span>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </td>
                    <td class="b_box" style="width: 25%">
                        <span>
                            <%if (hfStartingCreditAmount.Value != null && hfStartingCreditAmount.Value != "0" && hfStartingCreditAmount.Value != "")
                              { %>
                            $<%=hfStartingCreditAmount.Value%>
                            <%}
                              else
                              { %>
                            ---
                            <%} %>
                        </span>
                    </td>
                    <td class="g_box" colspan="2" style="width: 40%">
                        <span>
                            <%=System.DateTime.Now.ToShortDateString() %></span>
                    </td>
                </tr>
                <tr class="ord_content">
                    <td class="g_box" style="width: 35%">
                        <span class="first">Starting Credit Balance Expiration Amount / Date: </span>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </td>
                    <td class="b_box" style="width: 25%">
                        <span>
                            <%if (hfStartingCreditAmount.Value != null && hfStartingCreditAmount.Value != "0" && hfStartingCreditAmount.Value != "")
                              { %>
                            $<%=hfStartingCreditAmount.Value%>
                            <%}
                              else
                              { %>
                            ---
                            <%} %>
                        </span>
                    </td>
                    <td class="g_box" colspan="3" style="width: 40%">
                        <span>
                            <%if (hfStatrtingCreditExpirationDate.Value != null)
                              { %>
                            <%=hfStatrtingCreditExpirationDate.Value%>
                            <%}
                              else
                              { %>
                            ---
                            <%} %>
                        </span>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <div>
                <table class="orderreturn_box" cellpadding="0" cellspacing="0">
                    <tr class="ord_header">
                        <td style="width: 35%;">
                            <span>My Annual Credit Details</span><div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </td>
                        <td style="width: 25%;">
                            <span>Amount</span>
                        </td>
                        <td style="width: 40%;" colspan="2">
                            <span class="white_co">Date</span>
                        </td>
                    </tr>
                    <%--<tr class="ord_content">
                        <td class="g_box">
                            <span class="first">My Anniversary Credit Balance: </span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </td>
                        <td class="b_box">
                            <span>
                                <%if (hfAnniversaryStartingBalance.Value != null)
                                  { %>
                                $<%=hfAnniversaryStartingBalance.Value%>
                                <%}
                                  else
                                  { %>
                                ---
                                <%} %></span>
                        </td>
                        <td class="g_box">
                            <span>
                                <%=System.DateTime.Now.ToShortDateString()%></span>
                        </td>
                    </tr>--%>
                    <tr class="ord_content">
                        <td class="g_box">
                            <span class="first">My Company Hired Date: </span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </td>
                        <td class="b_box">
                            <span>--- </span>
                        </td>
                        <td class="g_box">
                            <span>
                                <%if (hfDateHiredDate.Value != null)
                                  { %>
                                <%=hfDateHiredDate.Value%>
                                <%}
                                  else
                                  { %>
                                ---
                                <%} %></span>
                        </td>
                    </tr>
                    <tr id="trLastAnnCreditAmount" class="ord_content" runat="server">
                        <td class="g_box">
                            <span class="first">My Last Anniversary Credit Amount / Date: </span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </td>
                        <td class="b_box">
                            <span>---
                                <%--    <%if (hfDollarCreditToApply.Value != null && hfDollarCreditToApply.Value != "0")
                                  { %>
                                $<%=hfDollarCreditToApply.Value%>
                                <%}
                                  else
                                  { %>
                                ---
                                <%} %>--%></span>
                        </td>
                        <td class="g_box">
                            <span>
                                <%if (hfNewHiredDate.Value != null)
                                  { %>
                                <%=hfNewHiredDate.Value%>
                                <%}
                                  else
                                  { %>
                                ---
                                <%} %></span>
                        </td>
                    </tr>
                    <tr class="ord_content">
                        <td class="g_box">
                            <span class="first">Next Anniversary Credit Amount / Date: </span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </td>
                        <td class="b_box">
                            <span>
                                <%if (hfDollarCreditToApply.Value != null && hfDollarCreditToApply.Value != "0" && hfDollarCreditToApply.Value != "")
                                  { %>
                                $<%=hfDollarCreditToApply.Value%>
                                <%}
                                  else
                                  { %>
                                ---
                                <%} %></span>
                        </td>
                        <td class="g_box">
                            <span>
                                <%if (hfNextCreditToBeApplyDate.Value != null && hfNextCreditToBeApplyDate.Value != "")
                                  { %>
                                <%=hfNextCreditToBeApplyDate.Value%>
                                <%}
                                  else
                                  { %>
                                ---
                                <%} %></span>
                        </td>
                    </tr>
                    <tr class="ord_content">
                        <td class="g_box">
                            <span class="first">Next Anniversary Credit Expires Amount / Date: </span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </td>
                        <%--Updated On 22-2-11--%>
                        <td class="b_box">
                            <span>
                                <%if (hfAnniversaryStartingBalance.Value != null)
                                  { %>
                                $<%=hfAnniversaryStartingBalance.Value%>
                                <%}
                                  else
                                  { %>
                                ---
                                <%} %>
                            </span>
                        </td>
                        <td class="g_box">
                            <span>
                                <%if (hfeNextCreditExpirationDate.Value != null)
                                  { %>
                                <%=hfeNextCreditExpirationDate.Value%>
                                <%}
                                  else
                                  { %>
                                ---
                                <%} %></span>
                            <%--<%if (hfCreditAfterExpiry.Value != null && hfCreditAfterExpiry.Value != "0")
                                  { %>
                                $<%=hfCreditAfterExpiry.Value%>
                                <%}
                                  else
                                  { %>
                                ---
                                <%} %>--%>
                        </td>
                    </tr>
                    <tr class="ord_content">
                        <td class="g_box" style="width: 25%">
                            <span class="first">My Current Credit Total: </span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </td>
                        <td class="b_box" style="width: 25%">
                            <span>
                                <%if (hfAnniversaryStartingBalance.Value != null && hfAnniversaryStartingBalance.Value != "0") //hfCreditBalance.Value
                                  { %>
                                $<%=hfAnniversaryStartingBalance.Value%>
                                <%}
                                  else
                                  { %>
                                ---
                                <%} %>
                            </span>
                        </td>
                        <td class="g_box" colspan="4" style="width: 50%">
                            <span>
                                <%=System.DateTime.Now.ToShortDateString() %>
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div>
            <table class="orderreturn_box" cellpadding="0" cellspacing="0">
                <tr class="ord_header">
                    <td style="width: 100%;">
                        <span>Store Credit Orders</span><div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <div style="text-align: center; color: Red; font-size: larger;">
                            <asp:Label ID="lblmsg" runat="server">
                            </asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvEmployeeOrderAnniversary" runat="server" AutoGenerateColumns="false"
                HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                RowStyle-CssClass="ord_content" OnRowCommand="gvEmployeeOrderAnniversary_RowCommand"
                OnRowDataBound="gvEmployeeOrderAnniversary_RowDataBound">
                <Columns>
                    <asp:TemplateField Visible="false" HeaderText="Id">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblOrderID" Text='<%# Eval("OrderID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnTrasHistory" runat="server" CommandArgument="TransHistory"
                                CommandName="Sort"><span class="white_co">Order Number</span></asp:LinkButton>
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="centeralign" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblOrderNumber" Text='<%# Eval("OrderNumber") %>' CssClass="first" />
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="20%" CssClass="g_box centeralign" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ReferenceName">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnReferenceName" runat="server" CommandArgument="ReferenceName"
                                CommandName="Sort"><span class="white_co">Reference Name</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblReferenceName" Text='<%# (Eval("ReferenceName") != null && Eval("ReferenceName") != "") ? Eval("ReferenceName").ToString() : "---" %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="20%" />
                    </asp:TemplateField>
                    <%-- <asp:TemplateField SortExpression="Country">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnDate" runat="server" CommandArgument="OrderDate" CommandName="Sort"><span class="white_co">Date</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblOrderDate" Text='<%# Convert.ToDateTime(Eval("OrderDate")).ToShortDateString() %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="20%" />
                    </asp:TemplateField>--%>
                    <asp:TemplateField SortExpression="TotalOrderAmount">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnOrderAmount" runat="server" CommandArgument="OrderAmount"
                                CommandName="Sort">
                                <span>Total Order Amount</span>
                            </asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblOrderAmountView" Text='<%# Eval("TotalAmount") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="20%" />
                    </asp:TemplateField>
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
                        <ItemStyle CssClass="b_box" Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Details">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnOrderDetails" runat="server" CommandArgument="OrderAmount"
                                CommandName="Sort">
                                <span>Details</span>
                            </asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="btn_box centeralign">
                                <asp:LinkButton ID="lnkRedirect" CssClass="greysm_btn" runat="server" CommandArgument='<%# Eval("OrderID") %>'
                                    CommandName="OrderDetail"><span style="color:White;"> 
                                View Order Details</span></asp:LinkButton>
                                <%--<div class="corner">
                                    <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                </div>--%>
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box centeralign" Width="20%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:HiddenField ID="hfCreditBalance" runat="server" />
        <asp:HiddenField ID="hfDateHiredDate" runat="server" />
        <asp:HiddenField ID="hfNextCreditToBeApplyDate" runat="server" />
        <asp:HiddenField ID="hfeNextCreditExpirationDate" runat="server" />
        <asp:HiddenField ID="hfDollarCreditToApply" runat="server" />
        <asp:HiddenField ID="hfStatrtingCreditExpirationDate" runat="server" />
        <asp:HiddenField ID="hfStartingCreditAmount" runat="server" />
        <asp:HiddenField ID="hfCreditAfterExpiry" runat="server" />
        <asp:HiddenField ID="hfNewHiredDate" runat="server" />
        <%--new--%>
        <asp:HiddenField ID="hfStartingCreditCurrentBalance" runat="server" />
        <asp:HiddenField ID="hfAnniversaryStartingBalance" runat="server" />
        <asp:HiddenField ID="hfStartingCreditUpdatedDate" runat="server" />
    </div>
</asp:Content>
