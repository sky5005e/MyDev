<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EquipmentSearchResult.aspx.cs" Inherits="AssetManagement_EquipmentSearchResult" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="upnlCompanyStore">
        <Triggers>
            <asp:PostBackTrigger ControlID="gvEquipment" />
        </Triggers>
        <ContentTemplate>
            <div class="form_pad">
                <div>
                    <div style="text-align: center; color: Red; font-size: larger;">
                        <asp:Label ID="lblmsg" runat="server">
                        </asp:Label>
                    </div>
                    <%--<table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="50%" runat="server" id="tdCompany">
                        <div style="text-align: left; color: #5C5B60; font-size: larger; font-weight: bold;
                            padding-left: 15px;">
                            Company Name :
                            <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                        </div>
                    </td>
                    <td width="50%" runat="server" id="tdReords">
                        <div style="text-align: right; color: #5C5B60; font-size: larger; font-weight: bold;
                            padding-left: 15px;">
                            Total Records :
                            <asp:Label ID="lblRecords" runat="server"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>--%>
                    <asp:GridView ID="gvEquipment" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
                        <Columns>
                            <asp:TemplateField Visible="False" HeaderText="EquipmentMasterID">
                                <HeaderTemplate>
                                    <span>EquipmentMasterID</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEquipmentMasterID" Text='<%# Eval("EquipmentMasterID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="EquipmentID">
                                <HeaderTemplate>
                                    <span>EquipmentID</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEquipmentID" Text='<%# Eval("EquipmentID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEquipmentID" runat="server" CommandArgument="EquipmentID"
                                        CommandName="Sort"><span>Equipment ID</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderEquipmentID" runat="server"></asp:PlaceHolder>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkedit" CommandArgument='<%# Eval("EquipmentMasterID") %>' CommandName="detailCamp"
                                        runat="server" NavigateUrl='<%# "~/AssetManagement/AssetProfile.aspx?Id=" + Eval("EquipmentMasterID").ToString() + "&Page=EquipmentSearchResult"%>'><span class="first"><%# "&nbsp;" + Eval("EquipmentID")%></span></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="EquiType">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEquiType" runat="server" CommandArgument="EquiType" CommandName="Sort"><span >Equipment Type</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderEquiType" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfEquiType" runat="server" Value='<%# Eval("EquiType")%>' />
                                    <asp:Label runat="server" ID="lblEquiType" Text='<%# "&nbsp;" + Eval("EquiType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Location">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnLocation" runat="server" CommandArgument="Location" CommandName="Sort"><span >Location</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderLocation" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfLocation" runat="server" Value='<%# Eval("Location")%>' />
                                    <asp:Label runat="server" ID="lblLocation" Text='<%# "&nbsp;" + Eval("Location")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Status">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnStatus" runat="server" CommandArgument="Status" CommandName="Sort"><span >Status</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderStatus" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("Status")%>' />
                                    <asp:Label runat="server" ID="lblStatus" Text='<%# "&nbsp;" + Eval("Status") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="15%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <div class="companylist_botbtn alignleft">
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
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
