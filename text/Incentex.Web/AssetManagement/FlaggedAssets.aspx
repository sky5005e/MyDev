<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FlaggedAssets.aspx.cs" Inherits="AssetManagement_FlaggedAssets" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
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
                           <%-- <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEquipmentID" runat="server" CommandArgument="EquipmentID"
                                        CommandName="Sort"><span>Equipment ID</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderEquipmentID" runat="server"></asp:PlaceHolder>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                 <ItemTemplate>
                                            <asp:Label runat="server" CssClass="first" ID="lblEquipID" Text='<%# Eval("EquipmentID")%>'
                                                ToolTip='<% #Eval("EquipmentID")  %>'></asp:Label>
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>                              
                                <ItemStyle CssClass="g_box" Width="10%" />
                            </asp:TemplateField>--%>
                            <asp:TemplateField >
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnEquipmentID" runat="server" CommandArgument="EquipmentID" 
                                        CommandName="Sort"><span style="text-align:center" >id#</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderEquipmentID" runat="server"></asp:PlaceHolder>
                                     <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>                               
                                        <asp:HyperLink ID="lnkedit" CommandArgument='<%# Eval("EquipmentMasterID") %>' CommandName="detailCamp" 
                                            runat="server" NavigateUrl='<%# "~/AssetManagement/AssetProfile.aspx?Id=" + Eval("EquipmentMasterID").ToString()+ "&Page=FlaggedAssets"%>'><span><%# Eval("EquipmentID")%></span></asp:HyperLink>
                                 <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="10%" />
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
                                <ItemStyle CssClass="g_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="BaseStation">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnBaseStation" runat="server" CommandArgument="BaseStation" CommandName="Sort"><span >BaseStation</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderBaseStation" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfBaseStation" runat="server" Value='<%# Eval("BaseStation")%>' />
                                    <asp:Label runat="server" ID="lblBaseStation" Text='<%# "&nbsp;" + Eval("BaseStation")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="15%" />
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
                                <ItemStyle CssClass="g_box" Width="15%" />
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
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

