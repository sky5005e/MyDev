<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderDetailView.aspx.cs" Inherits="OrderManagement_OrderDetailView"
    Title="Order Management>> Order Detail View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <script type="text/javascript">
        // Let's use a lowercase function name to keep with JavaScript conventions
        function selectAll(invoker) {
            // Since ASP.NET checkboxes are really HTML input elements
            //  let's get all the inputs
            var inputElements = document.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                // Filter through the input types looking for checkboxes
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        } 
    </script>
    <%--  <asp:UpdatePanel runat="server" ID="upnlGrdCompany">
        <ContentTemplate>--%>
    <div class="form_pad">
        <div>
            <div style="text-align: center; color: Red; font-size: larger;">
                <asp:Label ID="lblmsg" runat="server">
                </asp:Label>
            </div>
            <asp:Repeater ID="rptOrderHeader" runat="server" 
                onitemdatabound="rptOrderHeader_ItemDataBound" 
                onitemcommand="rptOrderHeader_ItemCommand">
                <ItemTemplate>
                    <h4>
                        Order Detail :
                        <%#DataBinder.Eval(Container.DataItem, "CompanyName") %></h4>
                    <asp:HiddenField ID="hiddenCompanyID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "CompanyID") %>' />
                    <div class="form_pad" style="padding-top:0px !important;">
                        <div>
                            <div style="text-align: center; color: Red; font-size: larger;">
                            </div>
                            <asp:GridView ID="gvOrderDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvOrderDetail_RowDataBound">
                                <Columns>
                                <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span >
                                        <asp:CheckBox ID="cbSelectAll" runat="server" OnClick="selectAll(this)" />&nbsp;
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </span>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                    <span class="first">
                                        <asp:CheckBox ID="CheckBox1" runat="server" />&nbsp;
                                    </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="5%" CssClass="b_box centeralign" />
                            </asp:TemplateField>
                                <asp:TemplateField SortExpression="OrderNumber">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnOrderNumber" runat="server" CommandArgument="OrderNumber"
                                            CommandName="Sort"><span >Order #</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderOrderNumber" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hypOrderNumber" CommandArgument='<%# Eval("OrderNumber") %>' CommandName="Edit"
                                            runat="server"><span><%# Eval("OrderNumber")%></span></asp:HyperLink>
                                        <asp:HiddenField ID="hdnOrderNumber" runat="server"  Value='<%# Eval("OrderID") %>'/>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Contact">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnContact" runat="server" CommandArgument="Contact" CommandName="Sort"><span >Contact</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderContact" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblContact" Text='<%# Eval("Contact") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Workgroup">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnWorkgroup" runat="server" CommandArgument="Workgroup" CommandName="Sort"> <span >Work Group</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderWorkgroup" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblWorkgroup" Text='<%# Eval("Workgroup") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="OrderDate">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnOrderDate" runat="server" CommandArgument="OrderDate"><span>Submit Date</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderOrderDate" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblOrderDate" Text='<%# Eval("OrderDate","{0:d}") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="OrderStatus">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnOrderStatus" runat="server" CommandArgument="OrderStatus"
                                                CommandName="Sort"><span>Status</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderOrderStatus" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdnOrderStatus" Value='<%# Eval("OrderStatus") %>' />
                                            <asp:DropDownList runat="server" ID="ddlStatus" Style="background-color: #303030; border: medium none;
                                                                        color: #ffffff; width: 100px; padding: 2px" runat="server" 
                                                                       ></asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="10%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <%--  <div >
						    <div class="companylist_botbtn alignleft">
					            <asp:LinkButton ID="btnAddCompany" runat="server" onclick="btnAddCompany_Click"  TabIndex="0" CssClass="grey_btn"><span>Add Company</span>
						           </asp:LinkButton>	
						           <asp:LinkButton ID="btnDelete" CssClass="grey_btn" runat="server"  TabIndex="0" onclick="btnDelete_Click"  OnClientClick="return DeleteConfirmation();"><span>Delete</span></asp:LinkButton>
						    </div>
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
					        
						</div>		--%>
					    <div class="companylist_botbtn alignleft">
					            <asp:LinkButton ID="btnSaveStatus" runat="server" CommandName="Status"  TabIndex="0" CssClass="grey_btn"><span>Save Status</span>
						           </asp:LinkButton>	
						          
						    </div>
					</div>
                </ItemTemplate>
                <SeparatorTemplate>
                    <hr />
                </SeparatorTemplate>
            </asp:Repeater>
        </div>
    </div>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
