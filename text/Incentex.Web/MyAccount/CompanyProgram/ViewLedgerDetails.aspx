<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewLedgerDetails.aspx.cs" Inherits="MyAccount_CompanyProgram_ViewLedgerDetails"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<%--    <style>
        span.show-tooltip-text
        {
            display: none;
            position: inline;
            font-size: 0.9em;
            background-image: url(../../images/bg.gif);
            background-repeat: repeat-x;
            padding: 6px;
            padding-left: 12px;
            padding-right: 12px;
            color: white;
        }
    </style>

    <script src="../../JS/JQuery/jqtooltip.js" type="text/javascript" language="javascript"></script>--%>

    <%--<script language="JavaScript" type="text/javascript">

ShowTooltip = function(e)
{
	var text = $(this).next('.show-tooltip-text');
	if (text.attr('class') != 'show-tooltip-text')
		return false;

	text.fadeIn()
		.css('top', e.pageY)
		.css('left', e.pageX+10);

	return false;
}
HideTooltip = function(e)
{
	var text = $(this).next('.show-tooltip-text');
	if (text.attr('class') != 'show-tooltip-text')
		return false;

	text.fadeOut();
}

SetupTooltips = function()
{
	$('.show-tooltip')
		.each(function(){
			$(this)
				.after($('<span/>')
					.attr('class', 'show-tooltip-text')
					.html($(this).attr('title')))
				.attr('title', '');
		})
		.hover(ShowTooltip, HideTooltip);
}



    </script>--%>

    <at:ToolkitScriptManager ID="src" runat="server">
    </at:ToolkitScriptManager>
    <%--<a href="http://google.com/" class="show-tooltip" 
        title="Start your search with Google!">Google.</a>--%>
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div>
                <div class="form_pad" style="padding-top: 20px !important;">
                    <div style="text-align: center">
                        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                    </div>
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
                                    <span class="first">
                                        <asp:Label runat="server" ID="lblTransactionType" Text='<% # (Convert.ToString(Eval("TransactionType")).Length > 20) ? Eval("TransactionType").ToString().Substring(0,20)+"..." : Convert.ToString(Eval("TransactionType"))  %>'
                                            />
                                    </span>
                                    <%--<asp:LinkButton class="show-tooltip"  runat="server" ID="lblTransactionType" Text='<% # (Convert.ToString(Eval("TransactionType")).Length > 20) ? Eval("TransactionType").ToString().Substring(0,20)+"..." : Convert.ToString(Eval("TransactionType"))  %>'
                                         title="hi hi hi hi" onmouseover="ShowTooltip();" />--%>
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
                                    <asp:LinkButton ID="lblOrderNumber" CommandArgument='<%#Eval("OrderId")%>' CommandName="View"
                                        runat="server" Text='<%# Convert.ToString(Eval("OrderNumber")) != "" ? Eval("OrderNumber") : "---"%>'>
                                    </asp:LinkButton>
                                    <asp:Label runat="server" ID="lblORderId" Visible="false" Text='<%#Eval("OrderId")%>' />
                                    <%--<asp:Label runat="server" ID="lblOrderNumber" Text='<%# Convert.ToString(Eval("OrderNumber")) != "" ? Eval("OrderNumber") : "---"%>' />--%>
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
</asp:Content>
