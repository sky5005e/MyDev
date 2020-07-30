<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="VendorList.aspx.cs" Inherits="AssetManagement_VendorList" Title="Vendor List" %>

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

    </script>

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
                        OnRowDataBound="gvEquipment_RowDataBound" >
                        <Columns>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span class="custom-checkbox gridheader">
                                        <asp:CheckBox ID="chkAll" runat="server" />
                                    </span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="custom-checkbox gridcontent first">
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" VerticalAlign="Middle" Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEquipmentVendorID" Text='<%# Eval("EquipmentVendorID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="EquipmentVendorName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEquipmentVendorName" runat="server" CommandArgument="EquipmentVendorName"
                                        CommandName="Sort"><span>Company</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderEquipmentVendorName" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkedit" CommandArgument='<%# Eval("EquipmentVendorName") %>'
                                        runat="server" NavigateUrl='<%# "~/AssetManagement/BasicVendorInformation.aspx?Id=" + Eval("EquipmentVendorID").ToString()%>'><span><%# Eval("EquipmentVendorName")%></span></asp:HyperLink>
                                    <asp:Label runat="server" ID="lnkVendorName" Text='<%# Eval("EquipmentVendorName") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Owner">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnOwner" runat="server" CommandArgument="Owner" CommandName="Sort"><span >Owner</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderOwner" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfOwner" runat="server" Value='<%# Eval("Contact")%>' />
                                    <asp:Label runat="server" ID="lblOwner" Text='<%# "&nbsp;" + Eval("Contact") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="25%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="BaseStation">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnBaseStation" runat="server" CommandArgument="BaseStation"
                                        CommandName="Sort"><span >BaseStation</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderBaseStation" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfBaseStationID" runat="server" Value='<%# Eval("BaseStationID")%>' />
                                    <asp:HiddenField ID="hfCompanyID" runat="server" Value='<%# Eval("CompanyID")%>' />
                                    <asp:Label runat="server" ID="lblBaseStation" Text='<%# "&nbsp;" + Eval("sBaseStation") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Add Employee</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="btn_space">
                                        <asp:Button ID="lnkbtnAddEmployee" runat="server" Text="+" CommandName="AddEmployee"
                                            CommandArgument='<%# Eval("EquipmentVendorID") %>' />
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box centeralign" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>View Employee</span>
                                      <div class="corner">
                                        <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="btn_box">
                                        <asp:Button ID="lnkbtnViewEmployee" CommandName="ViewEmployee" Text=">>" CommandArgument='<%# Eval("EquipmentVendorID")%>'
                                            runat="server"></asp:Button></span>
                                    <div class="corner">
                                        <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" Width="15%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <div class="companylist_botbtn alignleft">
                        <asp:LinkButton ID="btnDelete" CssClass="grey_btn" runat="server" TabIndex="0" OnClientClick="return DeleteConfirmation();"
                            OnClick="btnDelete_Click"><span>Delete</span></asp:LinkButton>
                    </div>
                    <div class="companylist_botbtn">
                        <asp:LinkButton ID="lnkbtnAddCompany" CssClass="grey_btn" runat="server" TabIndex="0"
                            OnClick="lnkbtnAddCompany_Click"><span>Add Company</span></asp:LinkButton>
                    </div>
                    <%-- <div class="companylist_botbtn">
                        <asp:LinkButton ID="lnkbtnAddCustomerCompany" CssClass="grey_btn" runat="server" TabIndex="0" 
                            OnClick="lnkbtnAddCustomerCompany_Click"><span>Add Customer Company</span></asp:LinkButton>
                    </div>--%>
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
