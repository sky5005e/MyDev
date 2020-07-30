<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="EmployeeList.aspx.cs" Inherits="AssetManagement_EmployeeList" Title="Employee List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>

    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }

        function SelectAllCheckboxesSpecific(spanChk) {
            var IsChecked = spanChk.checked;
            var Chk = spanChk;
            Parent = document.getElementById('ctl00_ContentPlaceHolder1_gvEquipment');
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != Chk && items[i].type == "checkbox") {
                    if (items[i].checked != IsChecked) {
                        items[i].click();
                    }
                }
            }
        }
    </script>
    <style type="text/css">
        .headspan input
        {
        	margin:7px 6px 6px 6px;
        }
        
    </style>
    <asp:UpdatePanel runat="server" ID="upnlCompanyStore">
        <Triggers>
            <asp:PostBackTrigger ControlID="gvEquipment" />
        </Triggers>
        <ContentTemplate>
            <div class="form_pad">
                <div>
                    <div style="text-align: right; color: Red; font-size: larger;" id="dvTotalRecords"
                        runat="server">
                        <span>Record Count: </span>
                        <asp:Label ID="LblTotalRecords" runat="server">
                        </asp:Label>
                    </div>
                    <div style="text-align: center; color: Red; font-size: larger;">
                        <asp:Label ID="lblmsg" runat="server">
                        </asp:Label>
                    </div>
                    <asp:GridView ID="gvEquipment" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvEquipment_RowCommand"
                        OnRowDataBound="gvEquipment_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span class="headspan">
                                        <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);"
                                            runat="server" />
                                    </span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="first">&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" VerticalAlign="Middle" Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="VendorID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblVendorID" Text='<%# Eval("VendorID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="VendorEmployeeID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblVendorEmployeeID" Text='<%# Eval("VendorEmployeeID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="VendorEmployeeName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnVendorEmployeeName" runat="server" CommandArgument="VendorEmployeeName"
                                        CommandName="Sort"><span>Employee</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderVendorEmployeeName" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--  <asp:Label runat="server" CssClass="first" ID="lblVendorEmployee" 
                                            Text='<%# "&nbsp;" + Eval("VendorEmployeeFirstName") + "&nbsp;" + Eval("VendorEmployeeLastName")%>' ></asp:Label>--%>
                                    <asp:HyperLink ID="lnkedit" CommandArgument='<%# Eval("VendorEmployee") %>' runat="server"
                                        NavigateUrl='<%# "~/AssetManagement/BasicVendorEmpInformation.aspx?SubId=" + Eval("VendorEmployeeID").ToString()+"&Id="+ Eval("VendorID").ToString()%>'><span><%# "&nbsp;" + Eval("VendorEmployee")%></span></asp:HyperLink>
                                    <asp:Label runat="server" ID="lblVendorEmployee" Text='<%# Eval("VendorEmployee") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="35%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="VendorCompany">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnVendorCompany" runat="server" CommandArgument="Contact"
                                        CommandName="Sort"><span >Company</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderVendorCompany" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfVendorCompany" runat="server" Value='<%# Eval("EquipmentVendorName")%>' />
                                    <asp:Label runat="server" ID="lblVendorCompany" Text='<%# Eval("EquipmentVendorName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="UserType">
                                <HeaderTemplate>
                                    <span>User Type</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblUserType" Text='<%# "&nbsp;" + Eval("UserRole") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>BaseStations</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBaseStaions" Text="View Stations" ToolTip='<%# Eval("BaseStaions") %>'
                                        Style="cursor: pointer;"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Telephone">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnTelephone" runat="server" CommandArgument="Telephone" CommandName="Sort"><span >Telephone</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderTelephone" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfTelephone" runat="server" Value='<%# Eval("Telephone")%>' />
                                    <asp:Label runat="server" ID="lblTelephone" Text='<%# "&nbsp;" + Eval("Telephone") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="30%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <div class="companylist_botbtn alignleft">
                        <%-- <asp:LinkButton ID="btnAddEquipment" runat="server" TabIndex="0" CssClass="grey_btn"
                            OnClick="btnAddEquipment_Click"><span>Add Equipment</span>
                        </asp:LinkButton>--%>
                        <asp:LinkButton ID="btnDelete" CssClass="grey_btn" runat="server" TabIndex="0" OnClientClick="return DeleteConfirmation();"
                            OnClick="btnDelete_Click"><span>Delete</span></asp:LinkButton>
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
            <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
