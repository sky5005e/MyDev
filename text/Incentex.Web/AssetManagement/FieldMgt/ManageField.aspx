<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ManageField.aspx.cs" Inherits="AssetManagement_FieldMgt_ManageField" Title="Manage Field" %>
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
                            <asp:TemplateField Visible="False">                               
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFieldMasterID" Text='<%# Eval("FieldMasterID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span>&nbsp;&nbsp;<asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);"
                                        runat="server" />
                                        &nbsp; </span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="first">&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </span>
                                </ItemTemplate>
                                 <ItemStyle CssClass="b_box"  VerticalAlign="Middle"  Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnFieldMasterName" runat="server" CommandArgument="FieldMasterName" 
                                        CommandName="Sort"><span style="text-align:center" >Field</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderFieldMasterName" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>                                
                                        <asp:HyperLink ID="lnkedit" CommandArgument='<%# Eval("FieldMasterName") %>' CommandName="FieldMasterName"
                                            runat="server" NavigateUrl='<%# "~/AssetManagement/FieldMgt/AddField.aspx?Id=" + Eval("FieldMasterID").ToString()%>'><span><%# Eval("FieldMasterName")%></span></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="95%" />
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

