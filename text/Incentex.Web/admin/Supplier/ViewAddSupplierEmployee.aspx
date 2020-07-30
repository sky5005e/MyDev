<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ViewAddSupplierEmployee.aspx.cs" Inherits="admin_Supplier_ViewAddSupplierEmployee" Title="Supplier >> SupplierEmployee List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="mb" %>
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
  <script type="text/javascript">
 // Let's use a lowercase function name to keep with JavaScript conventions
      function selectAll(invoker) {
  // Since ASP.NET checkboxes are really HTML input elements
          //  let's get all the inputs
          var inputElements = document.getElementsByTagName('input');
        for (var i = 0; i < inputElements.length; i++) 
          {
              var myElement = inputElements[i];
              // Filter through the input types looking for checkboxes
              if (myElement.type === "checkbox") 
              {
                  myElement.checked = invoker.checked;
              }
          }
      } 
</script>	
<script type="text/javascript" language="javascript">
function DeleteConfirmation()
{
if (confirm("Are you sure, you want to delete selected records ?")==true)
   return true;
else
   return false;
}
</script>
<mb:MenuUserControl ID="manuControl" runat="server" />
<asp:UpdatePanel runat="server" ID="upnlGrdCompany">
<ContentTemplate>
<div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                </div>
<div class="form_pad">
					<div>
					<asp:GridView ID="gvSupplierEmployee" runat="server"
                        AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None"  
                        RowStyle-CssClass="ord_content" onrowdatabound="gvSupplierEmployee_RowDataBound" 
                            onrowcommand="gvSupplierEmployee_RowCommand">
                           
					  <Columns>
					  <asp:TemplateField Visible="False" HeaderText="Id" >
                                    <HeaderTemplate>
                                    <span >Docmentid</span>
                                </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSupplierEmpiD" Text='<%# Eval("SupplierEmployeeID") %>' />
                                    </ItemTemplate>
                                     <ItemStyle Width="2%" />
                                 </asp:TemplateField>
					  <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span >
                                    &nbsp;<asp:CheckBox ID="cbSelectAll" runat="server" OnClick="selectAll(this)" />
                                        &nbsp; </span>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="centeralign" Width="5%"  />
                                <ItemTemplate>
                                    <span class="first">
                                      &nbsp;&nbsp; <asp:CheckBox ID="CheckBox1" runat="server" />
                                    </span>
                                </ItemTemplate>
                               <ItemStyle VerticalAlign="Middle" Width="3%" HorizontalAlign="Left" />
                            </asp:TemplateField>
                        <asp:TemplateField SortExpression="EmployeeName">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnEmployeeName" runat="server" CommandArgument="EmployeeName"
                                    CommandName="Sort"><span>Employee Name</span></asp:LinkButton>
                                
                                <asp:PlaceHolder ID="placeholderEmployeeName" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                              <asp:LinkButton ID="lnkbtnfullname" CommandName="Edit" CommandArgument='<%# Eval("SupplierEmployeeID") %>'
                                    runat="server"><span><%# Eval("EmployeeName") %></span></asp:LinkButton>
                               
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Country">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnCountry" runat="server" CommandArgument="Country" CommandName="Sort"><span class="white_co">Country</span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderCountry" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCountry" Text='<% # (Convert.ToString(Eval("Country")).Length > 30) ? Eval("Country").ToString().Substring(0,30)+"..." : Convert.ToString(Eval("Country"))+ "&nbsp;"  %>'
                                    ToolTip='<% #Eval("Country")  %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="State">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnState" runat="server" CommandArgument="State" CommandName="Sort">
                                <span>State</span>
                                </asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderState" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStationManager" Text='<% # (Convert.ToString(Eval("State")).Length > 30) ? Eval("State").ToString().Substring(0,30)+"..." : Convert.ToString(Eval("State"))+ "&nbsp;"  %>'
                                    ToolTip='<% #Eval("State")  %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Mobile">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtncntctnum" runat="server" CommandArgument="Mobile" CommandName="Sort">
                        <span class="white_co">Contact no.</span>
                                </asp:LinkButton>
                                <asp:PlaceHolder ID="placeholdercontactnumber" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblMobile" Text='<% # (Convert.ToString(Eval("Mobile")).Length > 30) ? Eval("Mobile").ToString().Substring(0,30)+"..." : Convert.ToString(Eval("Mobile"))+ "&nbsp;"  %>'
                                    ToolTip='<% #Eval("Mobile")  %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="IsDirectEmployee">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnEmployeeType" runat="server" CommandArgument="IsDirectEmployee"
                                    CommandName="Sort">
                                <span class="white_co">Employee Type</span>
                                <div class="corner"><span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span></div>
                                </asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderEmployeeType" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblEmployeeType" Text='<% # (Convert.ToString(Eval("IsDirectEmployee")).Length > 40) ? Eval("IsDirectEmployee").ToString().Substring(0,40)+"..." : Convert.ToString(Eval("IsDirectEmployee"))+ "&nbsp;"  %>'
                                    ToolTip='<% #Eval("IsDirectEmployee")  %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                                 
                                 
                                 
                        </Columns>
					   </asp:GridView>
					
					</div>
					
<div>
						
			<div>
						    <div class="companylist_botbtn alignleft">
					            <asp:LinkButton ID="btnAddSuppEmp" runat="server" onclick="btnAddSuppEmp_Click"  TabIndex="0" CssClass="grey_btn"><span>Add Supplier Employee</span>
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
					        
						</div>			
						
						
						
					</div>
	</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>


