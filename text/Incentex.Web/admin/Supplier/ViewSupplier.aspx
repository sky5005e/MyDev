<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ViewSupplier.aspx.cs" Inherits="admin_Supplier_ViewSupplier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script language="javascript" type="text/javascript">
    function SelectAllCheckboxes(spanChk) {
        var IsChecked = spanChk.checked;
        var Chk = spanChk;
        Parent = Chk.form.elements;
        for (i = 0; i < Parent.length; i++) {
            if (Parent[i].type == "checkbox" && Parent[i].id != Chk.id) {
                if (Parent[i].checked != IsChecked)
                    Parent[i].click();
            }
        }
    }
    function SelectAllCheckboxesSpecific(spanChk) {
        var IsChecked = spanChk.checked;
        var Chk = spanChk;
        Parent = document.getElementById('ctl00_ContentPlaceHolder1_gv');
        var items = Parent.getElementsByTagName('input');
        for (i = 0; i < items.length; i++) {
            if (items[i].id != Chk && items[i].type == "checkbox") {
                if (items[i].checked != IsChecked) {
                    items[i].click();
                }
            }
        }
    }

    function SelectAllCheckboxesMoreSpecific(spanChk) {
        var IsChecked = spanChk.checked;
        var Chk = spanChk;
        Parent = document.getElementById('ctl00_ContentPlaceHolder1_gvIncentexEmployee');
        for (i = 0; i < Parent.rows.length; i++) {
            var tr = Parent.rows[i];
            //var td = tr.childNodes[0];			   		   
            var td = tr.firstChild;
            var item = td.firstChild;
            if (item.id != Chk && item.type == "checkbox") {
                if (item.checked != IsChecked) {
                    item.click();
                }
            }
        }
    }


    function HighlightRow(chkB) {
        var IsChecked = chkB.checked;
        if (IsChecked) {
            chkB.parentElement.parentElement.style.backgroundColor = '#228b22';  // grdEmployees.SelectedItemStyle.BackColor
            chkB.parentElement.parentElement.style.color = 'white'; // grdEmployees.SelectedItemStyle.ForeColor
        }
        else {
            chkB.parentElement.parentElement.style.backgroundColor = 'white'; //grdEmployees.ItemStyle.BackColor
            chkB.parentElement.parentElement.style.color = 'black'; //grdEmployees.ItemStyle.ForeColor
        }
    }
	</script>
	
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="sc1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="up1" runat="server">
    <ContentTemplate>
                <div class="form_pad">
                
					 <div style="text-align:center" >
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" ></asp:Label>
                    <br />
                </div>
						<div>
                        <asp:GridView ID="gv" runat="server" ondatabinding="gv_DataBinding"
                        AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None"
                        RowStyle-CssClass="ord_content" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"
                        AllowSorting="true" onrowcommand="gv_RowCommand"
                        >
                            <Columns>
                                <asp:TemplateField Visible="false" HeaderText="Id" >
                                    
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblID" Text=<%# Eval("SupplierID") %> />
                                    </ItemTemplate>
                                 </asp:TemplateField>
                                <asp:TemplateField HeaderText="Check">
                                
                                    <HeaderTemplate>
                                        <span >
                                        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);"
                                            runat="server" />
                                            &nbsp;
                                            </span>
                                            <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <span class="first">
                                      &nbsp;&nbsp; <asp:CheckBox  ID="chkDelete" runat="server" />
                                      </span>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Middle"  CssClass="b_box"  HorizontalAlign="Left" />
                                    
                                </asp:TemplateField>
                                <asp:TemplateField  >
                                <HeaderTemplate>
                                     <asp:LinkButton ID="lnkFirstName" runat="server" CommandArgument="FirstName" CommandName="Sorting"><span >Supplier Name</span></asp:LinkButton>
                                    
                                </HeaderTemplate>
                                
                                    <ItemTemplate>
                                        <%--<asp:Label runat="server" ID="lblSupplierName" Text=<%#Eval("FirstName") + " " + Eval("LastName") %> ></asp:Label>--%>
                                    <span>     <asp:HyperLink ID="lnkEditSupp" runat="server" NavigateUrl=<%# "~/admin/Supplier/MainCompanyContact.aspx?Id=" + Eval("SupplierID").ToString()%> 
                                        Text=<%#Eval("FirstName") + " " + Eval("LastName") %> >  </asp:HyperLink>
                                        </span>
                                        
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" />
                                 </asp:TemplateField>
                                <asp:TemplateField  >
                                    <HeaderTemplate>
                                             <asp:LinkButton ID="lnkCompanyWebsite" runat="server" CommandArgument="CompanyWebsite" CommandName="Sorting"><span class="white_co">Company Website</span></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        
                                        <asp:Label runat="server" ID="lblCompanyWebsite"  Text='<% # (Convert.ToString(Eval("CompanyWebsite")).Length > 15) ? Eval("CompanyWebsite").ToString().Substring(0,15)+"..." : Convert.ToString(Eval("CompanyWebsite"))+ "&nbsp;"  %>' ToolTip='<% #Eval("CompanyWebsite")  %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" />
                                 </asp:TemplateField>
                                 <asp:TemplateField  >
                                    <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCountryNamee" runat="server" CommandArgument="sCountryName" CommandName="Sorting"><span>Country</span></asp:LinkButton>
                                        
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCountry" Text=<%# Eval("sCountryName")%>  ></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" />
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField >
                                    <HeaderTemplate>
                                       <asp:LinkButton ID="lnkTelephone" runat="server" CommandArgument="Telephone" CommandName="Sorting"><span class="white_co">Contact No</span></asp:LinkButton>                                        
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblContactNo" Text=<%# Eval("Telephone")%> ></asp:Label>
                                        
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" />
                                 </asp:TemplateField>
                                 <asp:TemplateField >
                                    <HeaderTemplate>
                                        <span>No of Emp.</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblNoofEmp"  ></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" />
                                 </asp:TemplateField>
                                 <asp:TemplateField >
                                    <HeaderTemplate>
                                        <span class="white_co">Add Employee</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--<asp:HyperLink ID="lnkAdd" runat="server" Text="Add" ></asp:HyperLink>--%>
                                        <%--<asp:LinkButton ID="lnkAddEmloyee"  Text="+" runat="server"><span><input type="button" value="+" name="+"/></span> </asp:LinkButton>--%>
                                        <span>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnAddEmployee" CommandArgument=<%# Eval("SupplierID") %> CommandName="AddEmp" runat="server" Text="+" />
                                            </span>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                 </asp:TemplateField>
                            </Columns>  
                            
                        </asp:GridView>
						</div>
						<div>
						    <div class="companylist_botbtn alignleft">
						    
						            <asp:HyperLink ID="lnkAddStation" runat="server" class="grey_btn" NavigateUrl="~/admin/Supplier/MainCompanyContact.aspx?Id=0">
						                <span>Add Supplier</span>
						            </asp:HyperLink>
        						    
						            <asp:LinkButton ID="lnkDelete" runat="server" class="grey_btn" 
                                        onclick="lnkDelete_Click"
                                        OnClientClick="return confirm('Are you sure, you want to delete selected records ?')"
                                        >
						                <span>Delete</span>
						            </asp:LinkButton>
        						    
						            <%--    <a href="#" title="Add Category" class="grey_btn"><span>Add Category</span></a>	<a href="#" title="Delete" class="grey_btn"><span>Delete</span></a>--%>
					        </div>
					        
					       <%-- <div class="alignright pagging">
					        
					       <a href="#" class="prevb">Prev</a><a href="#">1</a>
					       <a href="#">2</a><a href="#">3</a><a href="#">4</a><a href="#">5</a>



					       <a href="#" class="nextb">Next</a>
					        </div>--%>
					        <div class="alignright pagging">
					             <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" 
                                    onclick="lnkbtnPrevious_Click" > 
                                 </asp:LinkButton>
                                    <span><asp:DataList ID="lstPaging" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="lstPaging_ItemCommand"
                                    OnItemDataBound="lstPaging_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
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
				</ContentTemplate>
				</asp:UpdatePanel>
				
</asp:Content>

