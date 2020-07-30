<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProductReturnView.aspx.cs" Inherits="ProductReturnManagement_ProductReturnView" Title="Product Return" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <script type="text/javascript">
 function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }
    </script>
   <asp:UpdatePanel runat="server" ID="upnlGrdCompany">
        <ContentTemplate>
   
<table>
<tr>
                    <td class="spacer10">
                    </td>
                </tr>
</table>
<table class="form_table">
         <tr>
                    <td>
            <div style="text-align: center; color: Red; font-size: larger;">
            </div>
             <div style="text-align: center; color: Red; font-size: larger;">
                <asp:Label ID="lblmsg" runat="server">
                </asp:Label>
            </div>
            <div>
            <asp:GridView ID="gvProductReturn" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" 
                RowStyle-CssClass="ord_content" onrowdatabound="gvProductReturn_RowDataBound" OnRowCommand="gvProductReturn_RowCommand"  >
                <Columns>
                    <asp:TemplateField SortExpression="ReturnOrder">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnReturnOrder" runat="server" CommandArgument="ReturnOrder"
                                CommandName="Sort"><span>Return Order #<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderReturnOrder" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                         <HeaderStyle CssClass="centeralign" />
                        <ItemTemplate>
                        <span>
                            <asp:HyperLink runat="server"  ID="lblReturnOrder"></asp:HyperLink>
                            </span>
                            <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                            <asp:HiddenField ID="hdnProductReturnId" runat="server" Value='<%# Eval("ProductReturnId") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box centeralign" Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ReturnQty" Visible="false">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnReturnQty" runat="server" CommandArgument="ReturnQty" CommandName="Sort"><span >Return Qty<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderReturnQty" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblReturnQty" Text='<%# Eval("ReturnQty") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ReferenceName">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnReferenceName" runat="server" CommandArgument="ReferenceName" CommandName="Sort"><span >Reference Name<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderReferenceName" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                           <asp:Label runat="server" ID="lblReferenceName"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="SubmitDate">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnSubmitDate" runat="server" CommandArgument="SubmitDate" CommandName="Sort"> <span >Submit Date<br /></span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderSubmitDate" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="centeralign" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblSubmitDate" Text='<%# Eval("SubmitDate","{0:d}") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box centeralign" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Status">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnStatus" runat="server" CommandArgument="Status" CommandName="Sort"><span >Status</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderStatus" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                          <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                          </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="15%" />
                    </asp:TemplateField>
                   
                </Columns>
            </asp:GridView>
            </div>
            <div >
						  
					        <div id="pagingtable" runat="server" class="alignright pagging">
					             <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" 
                                    onclick="lnkbtnPrevious_Click" >
                                 </asp:LinkButton>
                                    <span><asp:DataList ID="DataList2" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="DataList2_ItemCommand"
                                            OnItemDataBound="DataList2_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:DataList></span>
                                <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" 
                                    onclick="lnkbtnNext_Click" ></asp:LinkButton>
						    </div>
					        
						</div>	
      </td>
                </tr>
        </table>
<table class="dropdown_pad form_table" runat="server" visible="false">
         <tr>
                    <td>
                        <div class="botbtn centeralign">
                            <asp:LinkButton ID="lnkReturnExchange"  class="grey2_btn" runat="server" ToolTip=""
                                OnClick="lnkReturnExchange_Click"><span>Return/Exchange</span></asp:LinkButton>
                        </div>
                    </td>
                </tr>
                </table>
 </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

