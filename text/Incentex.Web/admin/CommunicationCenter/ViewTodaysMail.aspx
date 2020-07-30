<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewTodaysMail.aspx.cs" Inherits="admin_CommunicationCenter_ViewTodaysMail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        function showNestedGridView(obj) {
            var nestedGridView = document.getElementById(obj);

            if (nestedGridView.style.display == "none") {
                nestedGridView.style.display = "inline";

            } else {
                nestedGridView.style.display = "none";

            }
        }
        // Call Popup to New Window
        function WindowPopup(id) {
            if (id != '0') {
                var url = "ViewTemplates.aspx?mailID=" + id;
                newwindow = window.open(url, 'name', 'width=650,height=650, scrollbars=yes');
                if (window.focus) { newwindow.focus() }
                return false;
            }
        }
    </script>

    <%--this is for select all checkbox within grid--%>

    <script type="text/javascript">
        function changeAll(chk) {
            var parent = getParentByTagName(chk, "table");
            var checkBoxes = parent.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++)
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].id.indexOf("chkSelectUser") >= 0)
                checkBoxes[i].checked = chk.checked;
        }
        function getParentByTagName(obj, tag) {
            var obj_parent = obj.parentNode;
            if (!obj_parent) return false;
            if (obj_parent.tagName.toLowerCase() == tag) return obj_parent;
            else return getParentByTagName(obj_parent, tag);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsgGlobal" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <div style="text-align: right; padding-right: 30px; color: #72757C" id="dvTotalDollarValue"
            runat="server">
            <asp:Label ID="lblCount" runat="server"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="grdView_RowDataBound">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblUserInfoID" Text='<%# Eval("UserInfoID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Company Name</span>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label CssClass="first" runat="server" ID="lblCompanyName" Text='<%# Eval("CompanyName") %> ' />
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Full Name</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblFirstName" Text='<%# Eval("FullName") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <%--<asp:TemplateField Visible="false">
                    <HeaderTemplate>
                        <span >Base Station</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblBaseStation" Text='<%# Eval("BaseStation")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="15%" />
                </asp:TemplateField>--%>
                <asp:TemplateField Visible="false">
                    <HeaderTemplate>
                        <asp:Label ID="lblEmailName" runat="server" CommandArgument="LoginEmail" CommandName="Sort">Email</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEmail" Text='<%# Eval("LoginEmail") %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>View Today's Emails Sent</span>
                        <div class="corner">
                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span><a href="javascript:showNestedGridView('userID-<%# Eval("UserInfoID") %>');">Details
                            Link </a></span>
                        <div class="corner">
                            <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="30%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <tr>
                            <td colspan="100%">
                                <div id="userID-<%# Eval("UserInfoID") %>" style="display: none; position: relative;">
                                    <asp:GridView ID="nestedGridView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" Style="margin-left: 250px;
                                        margin-right: 250px; width: 40%;" OnRowDataBound="nestedGridView_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblNestedUserInfoID" Text='<%# Eval("UserInfoID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span>Full Name</span>
                                                    <div class="corner">
                                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label CssClass="first" runat="server" ID="lblFirstName" Text='<%# Eval("FullName") %> ' />
                                                    <div class="corner">
                                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="g_box" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span>Details</span>
                                                    <div class="corner">
                                                        <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span>
                                                        <asp:HiddenField ID="hdnTempID" runat="server" Value='<%#Eval("mailID")%>' />
                                                        <asp:HyperLink ID="hypViewTemp" runat="server" ToolTip="view templates" CommandArgument='<%# Eval("mailID") %>'>View Templates</asp:HyperLink>
                                                    </span>
                                                    <div class="corner">
                                                        <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="b_box" Width="30%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
