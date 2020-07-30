<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="SystemAccessByHourDetail.aspx.cs" Inherits="TrackingCenter_SystemAccessByHourDetail" Title="System Access By Hour Detail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>   

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
                    <asp:GridView ID="gvSystem" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvSystem_RowCommand"
                        OnRowDataBound="gvSystem_RowDataBound">
                        <Columns>
                            <asp:TemplateField Visible="False" HeaderText="UserInfoID">                               
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEquipmentMasterID" Text='<%# Eval("UserInfoID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField SortExpression="CompanyName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCompanyName" runat="server" CommandArgument="CompanyName" CommandName="Sort"><span >Company</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderCompanyName" runat="server"></asp:PlaceHolder>
                                <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfCompanyName" runat="server" Value='<%# Eval("CompanyName")%>' />
                                    <asp:Label runat="server" ID="lblCompanyName" Text='<%# "&nbsp;" + Eval("CompanyName")%>'></asp:Label>
                                                              
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="FullName">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnFullName" runat="server" CommandArgument="FullName" CommandName="Sort"><span >Name</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderFullName" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfFullName" runat="server" Value='<%# Eval("FullName")%>' />
                                    <asp:Label runat="server" ID="lblFullName" Text='<%# "&nbsp;" + Eval("FullName")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="50%" />
                            </asp:TemplateField>                           
                            <asp:TemplateField SortExpression="LoginCount">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkLoginCount" runat="server" CommandArgument="LoginCount" CommandName="Sort"><span >Number of Accesses</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderLoginCount" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfLoginCount" runat="server" Value='<%# Eval("LoginCount")%>' />
                                    <asp:Label runat="server" ID="lblLoginCount" Text='<%# "&nbsp;" + Eval("LoginCount")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="20%" />
                            </asp:TemplateField>  
                        </Columns>
                    </asp:GridView>
                </div>
                <div>                   
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

