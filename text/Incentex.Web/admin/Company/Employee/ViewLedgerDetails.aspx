<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewLedgerDetails.aspx.cs" Inherits="MyAccount_CompanyProgram_ViewLedgerDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="src" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div>
                <div class="form_pad" style="padding-top: 20px !important;">
                    <div style="text-align: center">
                        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                    </div>
                    <div style="text-align: right;">
                        <asp:LinkButton ID="lnkAddCredits" CssClass="greysm_btn" runat="server" OnClick="lnkAddCredits_Click"><span style="color:White;"> 
                                Post Credit Adjustment</span></asp:LinkButton>
                        <at:ModalPopupExtender ID="modal" TargetControlID="lnkAddCredits" BackgroundCssClass="modalBackground"
                            DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="closepopup">
                        </at:ModalPopupExtender>
                    </div>
                    <br />
                    <asp:GridView ID="gvEmployeeTransactionHistory" runat="server" AutoGenerateColumns="false"
                        HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                        RowStyle-CssClass="ord_content" OnRowCommand="gvEmployeeTransactionHistory_RowCommand"
                        OnRowDataBound="gvEmployeeTransactionHistory_RowDataBound">
                        <Columns>
                            <asp:TemplateField Visible="false" HeaderText="Id">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLedgerId" Text='<%# Eval("LedgerId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnTrasHistory" runat="server" CommandArgument="TransactionType"
                                        CommandName="Sorting"><span class="white_co">Transaction Type</span></asp:LinkButton>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTransactionType" CssClass="first" Text='<% # (Convert.ToString(Eval("TransactionType")).Length > 20) ? Eval("TransactionType").ToString().Substring(0,20)+"..." : Convert.ToString(Eval("TransactionType"))  %>' />
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="PreviousBalance">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnPreviousBalance" runat="server" CommandArgument="PreviousBalance"
                                        CommandName="Sorting">
                                <span>Previous Balance</span>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPreviousBalance" Text='<%# Convert.ToString(Eval("PreviousBalance")) != "" ? Eval("PreviousBalance") : "---" %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Credit/Debit">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnAmountCreditDebit" runat="server" CommandArgument="AmountCreditDebit"
                                        CommandName="Sorting"><span class="white_co">Credit/Debit</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblAmountCreditDebit" Text='<%# Eval("AmountCreditDebit").ToString() != "" ? Eval("AmountCreditDebit") : "---" %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="CreditedAmount">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCreditedAmount" runat="server" CommandArgument="TransactionAmount"
                                        CommandName="Sorting"><span class="white_co">Credited Amount</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCreditedAmount" Text='<%# Eval("TransactionAmount").ToString() != "" ? Eval("TransactionAmount") : "---" %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="CurrentBalance">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCurrentBalance" runat="server" CommandArgument="CurrentBalance"
                                        CommandName="Sorting">
                                <span>Current Balance</span>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCurrentBalance" Text='<%# Convert.ToString(Eval("CurrentBalance")) != "" ? Eval("CurrentBalance") : "---" %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="TransactionDate">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnTransactionDate" runat="server" CommandArgument="TransactionDate"
                                        CommandName="Sorting">
                                <span>Transaction Date</span>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTransactionDate" Text='<%#Eval("TransactionDate")%>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="OrderNumber">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnOrderNumber" runat="server" CommandArgument="OrderNumber"
                                        CommandName="Sorting">
                                <span>Order Number</span>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>
                                        <asp:LinkButton ID="lblOrderNumber" CommandArgument='<%#Eval("OrderId")%>' CommandName="View"
                                            runat="server" Text='<%# Convert.ToString(Eval("OrderNumber")) != "" ? Eval("OrderNumber") : "---"%>'>
                                        </asp:LinkButton>
                                    </span>
                                    <asp:Label runat="server" ID="lblORderId" Visible="false" Text='<%#Eval("OrderId")%>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="UpdatedBy">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnUpdatedBy" runat="server" CommandArgument="UpdatedBy" CommandName="Sorting">
                                <span>Updated By</span>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblUpdatedBy" Text='<% # (Convert.ToString(Eval("UpdatedBy")).Length > 8) ? Eval("UpdatedBy").ToString().Substring(0,8)+"..." : Convert.ToString(Eval("UpdatedBy"))  %>'
                                        ToolTip='<% #Eval("UpdatedBy")  %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div id="dvPaging" runat="server" class="alignright pagging">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                        </asp:LinkButton>
                        <span>
                            <asp:DataList ID="lstPaging" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="lstPaging_ItemCommand"
                                OnItemDataBound="lstPaging_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList></span>
                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
        <div class="pp_pic_holder facebook" style="display: block; width: 411px; position: fixed;
            left: 35%; top: 20%;">
            <div class="pp_top" style="">
                <div class="pp_left">
                </div>
                <div class="pp_middle">
                </div>
                <div class="pp_right">
                </div>
            </div>
            <div class="pp_content_container" style="">
                <div class="pp_left" style="">
                    <div class="pp_right" style="">
                        <div class="pp_content" style="height: 400px; display: block;">
                            <div class="pp_loaderIcon" style="display: none;">
                            </div>
                            <div class="pp_fade" style="display: block;">
                                <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                <div class="pp_hoverContainer" style="height: 92px; width: 371px; display: none;">
                                    <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                        style="visibility: visible;">previous</a>
                                </div>
                                <div id="pp_full_res">
                                    <div class="pp_inline clearfix">
                                        <div class="form_popup_box">
                                            <div class="label_bar">
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                            </div>
                                            <div class="label_bar">
                                                <label style="width: 100px !important;">
                                                    Credit Type :</label>
                                                <span>
                                                    <asp:DropDownList class="popup_input" ID="ddlCreditType" runat="server">
                                                        <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Starting" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Anniversary" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="label_bar">
                                                <label style="width: 100px !important;">
                                                    Credit Amount :</label>
                                                <span>
                                                    <asp:TextBox class="popup_input" ID="txtCrAmount" runat="server"></asp:TextBox></span>
                                            </div>
                                            <div class="label_bar">
                                                <label style="width: 100px !important; vertical-align: top;">
                                                    Note :</label>
                                                <span>
                                                    <asp:TextBox Rows="5" MaxLength="1000" TextMode="MultiLine" ID="txtMultiLine" runat="server"></asp:TextBox></span>
                                            </div>
                                            <div class="label_bar btn_padinn">
                                                <asp:Button ID="btnSubmit" Text="Save" runat="server" OnClick="btnSubmit_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="pp_details clearfix" style="width: 371px;">
                                    <a href="#" id="closepopup" runat="server" class="pp_close">Close</a>
                                    <p class="pp_description" style="display: none;">
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="pp_bottom" style="">
                <div class="pp_left" style="">
                </div>
                <div class="pp_middle" style="">
                </div>
                <div class="pp_right" style="">
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
