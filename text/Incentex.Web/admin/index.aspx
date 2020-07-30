<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="index.aspx.cs" Inherits="admin_index" Title="World-Link System-Index" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/usercontrol/OpenServiceTicketIE.ascx" TagName="OpenServiceTicketIE"
    TagPrefix="ostIE" %>
<%@ Register Src="~/usercontrol/OpenServiceTicketSupplier.ascx" TagName="OpenServiceTicketSupplier"
    TagPrefix="ostSP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style type="text/css">
    .form_popup_box label
    {
        width:112px;
    }
</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function OnClickForSync() {
            return confirm("All the users who are accessing orders will get out of order accessing!\n\nAre you sure?");

        }
    </script>

    <ostIE:OpenServiceTicketIE ID="ostIEControl" runat="server" Visible="false" />
    <ostSP:OpenServiceTicketSupplier ID="ostSPControl" runat="server" Visible="false" />
    <div class="form_pad">
        <div class="btn_width worldlink_btn">
            <div>
                <asp:DataList ID="dtIndex" runat="server" RepeatDirection="Horizontal" RepeatColumns="3"
                    OnItemCommand="dtIndex_ItemCommand" OnItemDataBound="dtIndex_ItemDataBound">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkMenuAccess" CommandName="Redirect" CommandArgument='<%# Eval("sDescription")%>'
                            ToolTip='<%# Eval("sDescription")%>' CssClass="gredient_btn1" runat="server">
                            <img id="imgBtnPageURL" runat="server" alt="" src="" />
                            <span>
                                <%# Eval("sDescription")%></span>
                        </asp:LinkButton>
                        <asp:HiddenField ID="hdnMenuAccess" runat="server" Value='<%# Eval("iMenuPrivilegeID") %>' />
                        <asp:HiddenField ID="hdnMenuURL" runat="server" Value='<%# Eval("PageUrl") %>' />
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>
        <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
        <at:ModalPopupExtender ID="mpeViewUserStorefront" TargetControlID="lnkDummyAddNew"
            BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlViewUserStorefront"
            CancelControlID="closepopup">
        </at:ModalPopupExtender>
        <asp:Panel ID="pnlViewUserStorefront" runat="server" Style="display: none;">
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
                            <div class="pp_content" style="height: 250px; display: block;">
                                <div class="pp_loaderIcon" style="display: none;">
                                </div>
                                <div class="pp_fade" style="display: block;">
                                    <div id="pp_full_res">
                                        <div class="pp_inline clearfix">
                                            <div class="form_popup_box">
                                                <div class="label_bar centeralign">
                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                                </div>
                                                <div>
                                                    <div class="label_bar">
                                                        <label>
                                                            Select Company :</label>
                                                        <span>
                                                            <asp:DropDownList ID="ddlStorefrontCompany" Width="180px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlStorefrontCompany_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </span>
                                                    </div>
                                                    <div class="label_bar">
                                                        <label>
                                                            Select Access Type :</label>
                                                        <span>
                                                            <asp:DropDownList ID="ddlStorefrontAccessType" Width="180px" runat="server">
                                                                <asp:ListItem Text="Store Viewing Mode" Value="Normal"></asp:ListItem>
                                                                <asp:ListItem Text="Store Testing Mode" Value="Test"></asp:ListItem>
                                                                <asp:ListItem Text="Place Exchange Order" Value="PEO"></asp:ListItem>
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
                                    <div class="pp_details clearfix">
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
    </div>
</asp:Content>
<%--<Replaced <label> with <span> and <span> with <strong> on 2nd aug--%>