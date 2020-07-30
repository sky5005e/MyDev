<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewCompany.aspx.cs" Inherits="admin_Company_ViewCompany" Title="Company >> Company List" %>

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

    <asp:UpdatePanel runat="server" ID="upnlGrdCompany">
        <ContentTemplate>
            <div class="form_pad">
                <div>
                    <div style="text-align: center; color: Red; font-size: larger;">
                        <asp:Label ID="lblmsg" runat="server">
                        </asp:Label>
                    </div>
                    <asp:GridView ID="gvCompany" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" ShowFooter="true" GridLines="None" RowStyle-CssClass="ord_content"
                        OnRowDataBound="gvCompany_RowDataBound" OnRowCommand="gvCompany_RowCommand">
                        <Columns>
                            <asp:TemplateField Visible="False" HeaderText="Id">
                                <HeaderTemplate>
                                    <span>Company ID</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompanyID" Text='<%# Eval("CompanyID") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="2%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span class="custom-checkbox gridheader">
                                        <asp:CheckBox ID="cbSelectAll" runat="server" />&nbsp;</span>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </span>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                    <span class="first custom-checkbox gridcontent">
                                        <asp:CheckBox ID="CheckBox1" runat="server" />&nbsp; </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="5%" CssClass="b_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="CompanyName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCusName" runat="server" CommandArgument="CompanyName" CommandName="Sort"><span >Company Name</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderCompanyName" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="hypCompanyName" CommandName="Edit" runat="server"><span><%# Eval("CompanyName") %></span></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="29%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Country">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCountry" runat="server" CommandArgument="Country" CommandName="Sort"> <span >Country</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderCountry" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCountry" Text='<%# Eval("Country") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="14%" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField SortExpression="Telephone">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnTelephone" runat="server" CommandArgument="Telephone" CommandName="Sort"><span>Contact No.</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderTelephone" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStationManager" Text='<%# Eval("Telephone") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="14%" />
                            </asp:TemplateField>--%>
                            <asp:TemplateField SortExpression="NoOfEmp">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnNumberEmployees" runat="server" CommandArgument="Telephone"
                                        CommandName="Sort"><span># of Employees</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderNumberEmployees" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEmployeeno" Text='<%# Eval("NoOfEmp") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="12%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="StationID">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnNoofStation" runat="server" CommandArgument="StationID"
                                        CommandName="Sort"><span># of Stations</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderNoofStation" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStationID" Text='<%# Eval("StationID") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Add Emp</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%-- <span>
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkbtnAddemp" Text="+" CommandName="Emp" runat="server"><span><input type="button" value="+"  name="+"/></span></asp:LinkButton>
                                    </span>--%>
                                    <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnAddemp" CommandArgument='<%# Eval("CompanyID") %>' CommandName="Emp"
                                            runat="server" Text="+" />
                                    </span>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="g_box" Width="8%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Add Station</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%-- <asp:LinkButton ID="lnkbtnAddStation" CommandName="Station" Text="+" runat="server"><span><input type="button" value="+" name="+"/></span></asp:LinkButton>--%>
                                    <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnAddStation" CommandArgument='<%# Eval("CompanyID") %>' CommandName="Station"
                                            runat="server" Text="+" />
                                    </span>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="b_box" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Global Setting</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnAddGlobalSetting" CommandArgument='<%# Eval("CompanyID") %>' CommandName="GlobalSetting"
                                            runat="server" Text="+" />
                                    </span>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" CssClass="g_box" Width="12%" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="first" ForeColor="#72757C" Font-Bold="true" />
                    </asp:GridView>
                </div>
                <div>
                    <div class="companylist_botbtn alignleft">
                        <asp:LinkButton ID="btnAddCompany" runat="server" OnClick="btnAddCompany_Click" TabIndex="0"
                            CssClass="grey_btn"><span>Add Company</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" CssClass="grey_btn" runat="server" TabIndex="0" OnClick="btnDelete_Click"
                            OnClientClick="return DeleteConfirmation();"><span>Delete</span></asp:LinkButton>
                    </div>
                    <div id="pagingtable" runat="server" class="alignright pagging">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click">
                        </asp:LinkButton>
                        <span>
                            <asp:DataList ID="DataList2" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="DataList2_ItemCommand"
                                OnItemDataBound="DataList2_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
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
    <asp:HiddenField ID="hdnTotalValue" runat="server" />
    <asp:HiddenField ID="hndTotalStation" runat="server" />
</asp:Content>
