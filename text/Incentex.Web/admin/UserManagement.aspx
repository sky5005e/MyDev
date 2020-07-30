<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UserManagement.aspx.cs" Inherits="UserManagement_UserManagement" Title="World-Link System - UserManagement" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">        
        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleInnerContainer").collapsibleInnerPanel();
            $(".collapsibleContainerContent").hide();
            $(".collapsibleContainerInnerContent").hide();
            
            $(".action").click(function() {
                $('#dvLoader').show();
            });
        });
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div class="btn_width worldlink_btn">
            <asp:DataList ID="dtUserManage" runat="server" RepeatDirection="Vertical" RepeatColumns="3"
                OnItemCommand="dtUserManage_ItemCommand">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkManageName" CommandName="GO" CssClass="gredient_btn1 action" runat="server">
                                        <img alt='<%# Eval("sManageName")%>' src="<%# Eval("PageUrl") %>" />
                                            <span><%# Eval("sManageName")%></span>
                    </asp:LinkButton>
                    <asp:HiddenField ID="hdnManageID" Value='<%# DataBinder.Eval(Container.DataItem, "iManageID")%>'
                        runat="server" />
                    <asp:HiddenField ID="hdnsManagename" Value='<%# Eval("sManageName")%>' runat="server" />
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>
</asp:Content>
