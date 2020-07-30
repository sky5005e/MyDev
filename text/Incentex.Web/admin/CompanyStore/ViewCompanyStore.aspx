<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewCompanyStore.aspx.cs" Inherits="admin_UserManagement_CompanyStoreView"
    Title="Store >> Company Store List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>

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
//        };

       
         
    </script>

    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }
    </script>

    <div class="form_pad">
        <div>
            <div style="text-align: center; color: Red; font-size: larger;">
                <asp:Label ID="lblmsg" runat="server">
                </asp:Label>
            </div>
            <asp:GridView ID="gvCompanyStore" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvCompanyStore_RowCommand"
                OnRowDataBound="gvCompanyStore_RowDataBound">
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
                                <asp:CheckBox ID="cbSelectAll" runat="server" OnClick="selectAll(this)" />&nbsp;</span>
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <HeaderStyle CssClass="centeralign" />
                        <ItemTemplate>
                            <span class="first custom-checkbox gridcontent">
                                <asp:CheckBox ID="chkStore" runat="server" />&nbsp; </span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="5%" CssClass="b_box centeralign" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Company">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnCusName" runat="server" CommandArgument="Company" CommandName="Sort"><span >Company Name</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderCompanyName" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:HiddenField ID="hfComapnyName" runat="server" Value='<%# Eval("Company")%>' />
                            <asp:LinkButton ID="hypCompanyName" CommandName="EditStore" CommandArgument='<%# Eval("StoreID") %>'
                                runat="server"><span><%# Eval("Company")%></span></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="CountryName">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnCountry" runat="server" CommandArgument="CountryName" CommandName="Sort"> <span >Country</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderCountry" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblCountry" Text='<%# Eval("CountryName") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="16%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="StoreStatus">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnStatus" runat="server" CommandArgument="StoreStatus" CommandName="Sort"><span>Store Status</span></asp:LinkButton>
                            <asp:PlaceHolder ID="placeholderStatus" runat="server"></asp:PlaceHolder>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("StoreStatus") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span>Add Products</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="btn_space">
                                <asp:Button ID="lnkbtnAddProduct1" runat="server" Text="+" CommandName="ViewProducts"
                                    CommandArgument='<%# Eval("StoreID") %>' />
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box centeralign" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span>Employee Credits</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="btn_space">
                                <asp:Button ID="lnkbtnEmpCradits" CommandName="EmpCradits" Text=">>" runat="server"
                                    CommandArgument='<%# Eval("CompanyID") %>'></asp:Button>
                            </span>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box centeralign" Width="14%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="SAPCompanyCode">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkBtnSAPCompanyCode" runat="server" CommandArgument="SAPCompanyCode"
                                CommandName="Sort"><span>SAP Customer Code</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblSAPCompanyCode" Text='<%# "&nbsp;" + Convert.ToString(Eval("SAPCompanyCode")) %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box centeralign" Width="15%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <div class="companylist_botbtn alignleft">
                <asp:LinkButton ID="btnAddCompanyStroe" runat="server" TabIndex="0" CssClass="grey_btn"
                    OnClick="btnAddCompanyStroe_Click"><span>Add Company Store</span>
                </asp:LinkButton>
                <asp:LinkButton ID="btnDelete" CssClass="grey_btn" runat="server" TabIndex="0" OnClientClick="return DeleteConfirmation();"
                    OnClick="btnDelete_Click"><span>Delete</span></asp:LinkButton>
                <asp:LinkButton ID="lnkPrdSearch" CssClass="grey_btn" runat="server" TabIndex="0"
                    OnClick="lnkPrdSearch_Click"><span>Product Search</span></asp:LinkButton>
            </div>
            <div id="pagingtable" runat="server" class="alignright pagging">
                <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                </asp:LinkButton>
                <span>
                    <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                        RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:DataList></span>
                <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
