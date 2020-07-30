<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ViewOpenRepairOrders.aspx.cs" Inherits="AssetManagement_RepairManagement_ViewOpenRepairOrders" %>

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
            Parent = document.getElementById('ctl00_ContentPlaceHolder1_gvRepairOrder');
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

   <%-- <asp:UpdatePanel runat="server" ID="upnlRepairOrder">
        <Triggers>
            <asp:PostBackTrigger ControlID="gvRepairOrder" />
        </Triggers>
        <ContentTemplate>--%>
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
                    <asp:GridView ID="gvRepairOrder" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvRepairOrder_RowCommand"
                        OnRowDataBound="gvRepairOrder_RowDataBound">
                        <Columns>
                            <asp:TemplateField Visible="false" HeaderText="EquipmentMasterID">
                                <HeaderTemplate>
                                    <span>Repair Number</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRepairOrderID" Text='<%# Eval("RepairOrderID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span><asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);"
                                        runat="server" />
                                        &nbsp;</span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                 <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                    <span class="first">
                                        <asp:CheckBox ID="chkDelete" runat="server" />&nbsp; </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                  <ItemStyle Width="5%" CssClass="b_box centeralign" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="EquipmentMasterID">
                                <HeaderTemplate>
                                    <span>Repair Number</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkRepairOrderID" CommandArgument='<%# Eval("RepairOrderID") %>' 
                                            runat="server" NavigateUrl='<%# "~/AssetManagement/RepairManagement/RepairProfile.aspx?RepairOrderId=" + Eval("RepairOrderID").ToString() + "&IsBillingComplete=" + Eval("IsBillingComplete").ToString()%>'><span><%#"RN" + Eval("AutoRepairNumber")%></span></asp:HyperLink>
                                </ItemTemplate>
                                 <ItemStyle CssClass="g_box" Width="10%" />
                            </asp:TemplateField>
                             <asp:TemplateField SortExpression="EquipmentID">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEquipmentID" runat="server" CommandArgument="AssetID" CommandName="Sort"><span >Asset ID</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderEquipmentID" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEquipmentID" Text='<%#Eval("EquipmentID")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="10%" />
                            </asp:TemplateField>
                           <asp:TemplateField SortExpression="EquiType">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEquiType" runat="server" CommandArgument="AssetType" CommandName="Sort"><span >Asset Type</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderEquiType" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfEquiType" runat="server" Value='<%# Eval("EquipmetType")%>' />
                                    <asp:Label runat="server" ID="lblEquiType" Text='<%# Eval("EquipmetType")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Base Station">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnBaseStation" runat="server" CommandArgument="BaseStation" CommandName="Sort"><span >Base Station</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderBaseStation" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfBaseStation" runat="server" Value='<%# Eval("baseStation")%>' />
                                    <asp:Label runat="server" ID="lblBaseStation" Text='<%# Eval("baseStation")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Status">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnStatus" runat="server" CommandArgument="Status" CommandName="Sort"><span >Status</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderStatus" runat="server"></asp:PlaceHolder>
                                     <div class="corner">
                                        <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("RepairStatus")%>' />
                                    <asp:Label runat="server" ID="lblStatus" Text='<%#"&nbsp;" + Eval("RepairStatus")%>'></asp:Label>
                                     <div class="corner">
                                        <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="15%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <div class="companylist_botbtn alignleft">
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
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

