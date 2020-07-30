<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UserManagement.aspx.cs" Inherits="MyAccount_UserManagement" Title="Company Admin User Management" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_popup_box label
        {
            width: 112px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="SrcMgr" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div class="user_manage_btn btn_width_small" style="text-align: left;">
            <asp:HyperLink ID="lnkViewEmpl" class="gredient_btn" title="View Employee" runat="server">
                <span>
                <strong>
                    View Employees
                </strong>
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkViewStations" class="gredient_btn" title="View Stations" runat="server">
                <span>
                <strong>
                    View Stations
                </strong>
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkSearchUsers" class="gredient_btn" title="Search Users" runat="server">
                <span>
                <strong>
                    Search Users
                </strong>
                </span>
            </asp:HyperLink>
            <asp:HyperLink ID="lnkPendinUsers" class="gredient_btn" title="View Pending Users"
                runat="server">
                <span>
                <strong>
                View Pending Users
                </strong>
                </span>
            </asp:HyperLink>
        </div>
        <div style="height: 10px;">
        </div>
        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>
                <div class="user_manage_btn btn_width_small" style="text-align: left;">
                    <%if (!IncentexGlobal.IsIEFromStore)
                      { %>
                    <asp:HyperLink ID="lnkViewUserStoreFront" class="gredient_btn" title="View Users StoreFront"
                        runat="server" Style="text-align: left;" align="left"><span><strong>View Users StoreFront</strong></span>
                    </asp:HyperLink>
                    <% }%>
                    <asp:HyperLink ID="lnkStoreManagement" class="gredient_btn" title="Store Management"
                        runat="server"><span><strong>Store Management</strong></span>
                    </asp:HyperLink>
                </div>
                <at:ModalPopupExtender ID="modal" TargetControlID="lnkViewUserStoreFront" BackgroundCssClass="modalBackground"
                    DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="closepopup">
                </at:ModalPopupExtender>
                <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
                    <div class="pp_pic_holder facebook" style="display: block; width: 400px; position: fixed;
                        left: 35%; top: 30%;">
                        <div class="pp_top" style="">
                            <div class="pp_left">
                            </div>
                            <div class="pp_middle">
                            </div>
                            <div class="pp_right">
                            </div>
                        </div>
                        <div class="pp_content_container" style="">
                            <div class="pp_left" style="">
                                <div class="pp_right" style="">
                                    <div class="pp_content" style="height: 230px; display: block;">
                                        <div class="pp_fade" style="display: block;">
                                            <div id="pp_full_res">
                                                <div class="pp_inline clearfix">
                                                    <div class="form_popup_box">
                                                        <div class="label_bar centeralign">
                                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                        </div>
                                                        <div>
                                                            <div class="label_bar">
                                                                <label style="width: 112px;">
                                                                    Select Access Type :</label>
                                                                <span>
                                                                    <asp:DropDownList ID="ddlStorefrontAccessType" Width="180px" runat="server">
                                                                        <asp:ListItem Text="Store Viewing Mode" Value="Normal"></asp:ListItem>
                                                                        <asp:ListItem Text="Store Testing Mode" Value="Test"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </span>
                                                            </div>
                                                            <div class="label_bar">
                                                                <label>
                                                                    Select User :</label>
                                                                <span>
                                                                    <asp:DropDownList ID="ddlStorefrontUser" AutoPostBack="true" Width="180px" runat="server"
                                                                        OnSelectedIndexChanged="ddlStorefrontUser_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </span>
                                                            </div>
                                                            <div class="label_bar">
                                                                <label>
                                                                    Login :</label>
                                                                <span>
                                                                    <asp:Label runat="server" ID="lblLogin"></asp:Label>
                                                                </span>
                                                            </div>
                                                            <div class="label_bar">
                                                                <label>
                                                                    Password :</label>
                                                                <span>
                                                                    <asp:Label runat="server" ID="lblPassword"></asp:Label>
                                                                </span>
                                                            </div>
                                                            <div class="label_bar btn_padinn">
                                                                <asp:LinkButton ID="lnkbtnViewUserStoreFront" runat="server" CssClass="grey2_btn"
                                                                    OnClick="lnkbtnViewUserStoreFront_Click">
								                                      <span>Access Storefront</span>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="pp_details clearfix" style="width: 371px;">
                                                <a href="#" id="closepopup" runat="server" class="pp_close">Close</a>
                                                <p class="pp_description" style="display: none;">
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="pp_bottom" style="">
                            <div class="pp_left" style="">
                            </div>
                            <div class="pp_middle" style="">
                            </div>
                            <div class="pp_right" style="">
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
