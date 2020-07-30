<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="CompanyStoreView.aspx.cs" Inherits="admin_UserManagement_CompanyStoreView" Title="Store >> Company Store List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        // Let's use a lowercase function name to keep with JavaScript conventions
//        function selectAll(invoker) {
//            // Since ASP.NET checkboxes are really HTML input elements
//            //  let's get all the inputs
//            var inputElements = document.getElementsByTagName('input');
//            for (var i = 0; i < inputElements.length; i++) {
//                var myElement = inputElements[i];
//                // Filter through the input types looking for checkboxes
//                if (myElement.type === "checkbox") {
//                    myElement.checked = invoker.checked;
//                }
//            }
//        }
         
    </script>
    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }
    </script>
    <asp:UpdatePanel runat="server" ID="upnlCompanyStore">
        <ContentTemplate>
            <div class="form_pad">
                <div>
                    <div style="text-align: center; color: Red; font-size: larger;">
                        <asp:Label ID="lblmsg" runat="server">
                        </asp:Label>
                    </div>
                    <asp:GridView ID="gvCompanyStore" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content"
                        >
                        <Columns>
                            <asp:TemplateField Visible="False" HeaderText="Id">
                                <HeaderTemplate>
                                    <span>StoreID</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStoreID" Text='<%# Eval("StoreID") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="3%" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="Id">
                                <HeaderTemplate>
                                    <span>ComapyID</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompanyID" Text='<%# Eval("CompanyID") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="2%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                     <span class="custom-checkbox gridheader">
                                        <asp:CheckBox ID="cbSelectAll" runat="server"  />&nbsp;
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </span>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                     <span class="first custom-checkbox gridcontent">
                                        <asp:CheckBox ID="CheckBox1" runat="server" />&nbsp;
                                    </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="5%" CssClass="g_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="CompanyName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCusName" runat="server" CommandArgument="CompanyName" CommandName="Sort"><span >Company Name</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="hypCompanyName" CommandName="Edit" runat="server"><span><%# Eval("CompanyName") %></span></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Country">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCountry" runat="server" CommandArgument="Country" CommandName="Sort"> <span >Country</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCountry" Text='<%# Eval("Country") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Status">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnStatus" runat="server" CommandArgument="Status" CommandName="Sort"><span>Store Status</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("Status") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="sLookupName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnShipping" runat="server" CommandArgument="sLookupName" CommandName="Sort"><span>Shipping</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblShipping" Text='<%# Eval("sLookupName") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Add Products</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnAddProduct" Text="+" CommandName="Emp" runat="server"><span><input type="button" value="+" name="+"/></span></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box centeralign" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Employee Cradits</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnEmpCradits" CommandName="Station" Text=">>" runat="server"><span><input type="button" value=">>" name=">>"/></span></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>View Existing Store</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnViewStore" CommandName="Station" Text=">>" runat="server"><span><input type="button" value=">>" name=">>"/></span></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" Width="10%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
               <div >
						    <div class="companylist_botbtn alignleft">
					            <asp:LinkButton ID="btnAddCompanyStroe"  runat="server" TabIndex="0" 
                                    CssClass="grey_btn" onclick="btnAddCompanyStroe_Click"><span>Add Company Store</span>
						           </asp:LinkButton>	
						           <asp:LinkButton ID="btnDelete" CssClass="grey_btn" runat="server"  TabIndex="0"   OnClientClick="return DeleteConfirmation();"><span>Delete</span></asp:LinkButton>
						           <asp:LinkButton ID="lnkPrdSearch" CssClass="grey_btn" runat="server"  TabIndex="0" ><span>Product Search</span></asp:LinkButton>
						    </div>
					        <div id="pagingtable" runat="server" class="alignright pagging">
					             <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" 
                                   > 
                                 </asp:LinkButton>
                                    <span><asp:DataList ID="DataList2" runat="server" CellPadding="1" CellSpacing="1" 
                                             RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:DataList></span>
                                <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" 
                                  ></asp:LinkButton>
						    </div>
					        
						</div>		
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
