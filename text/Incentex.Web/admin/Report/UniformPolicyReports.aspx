<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UniformPolicyReports.aspx.cs" Inherits="admin_Report_UniformPolicyReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsgGlobal" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <%--First Div--%>
        <div class="black_top_co">
            <span>&nbsp;</span></div>
        <div class="black_middle order_detail_pad">
            <table class="order_detail" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2" style="font-size: small;">
                        <label>
                            Issuance Program Name :
                        </label>
                        <asp:Label ID="lblIssuanceProgramName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%">
                        <table>
                            <tr>
                                <td style="font-size: small;">
                                    <label>
                                        Policy Status :
                                    </label>
                                    <asp:Label ID="lblPolicyStatus" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: small;">
                                    <label>
                                        Policy Date :
                                    </label>
                                    <asp:Label ID="lblPolicyDate" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%">
                        <table>
                            <tr>
                                <td style="font-size: small;">
                                    <label>
                                        Policy Eligible Date:
                                    </label>
                                    <asp:Label ID="lblPolicyEligibleDate" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: small;">
                                    <label>
                                        Expire Date:
                                    </label>
                                    <asp:Label ID="lblExpireDate" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="black_bot_co">
            <span>&nbsp;</span></div>
        <div class="spacer15">
        </div>
        <asp:GridView ID="gvUniformPolicy" runat="server" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblUserID" Text='<%# Eval("UserInfoID")%> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Check">
                    <HeaderTemplate>
                        <span>
                            <asp:CheckBox ID="cbSelectAll" runat="server" OnClick="changeAll(this)" />&nbsp;</span>
                        <div class="corner">
                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                        </div>
                    </HeaderTemplate>
                    <HeaderStyle CssClass="centeralign" />
                    <ItemTemplate>
                        <span class="first">
                            <asp:CheckBox ID="chkSelectUser" runat="server" />&nbsp; </span>
                        <div class="corner">
                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="5%" CssClass="b_box centeralign" />
                </asp:TemplateField>
                <asp:TemplateField SortExpression="FirstName">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnFullName" runat="server" CommandArgument="FirstName" CommandName="Sort"><span>Full Name</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblFirstName" Text='<%# Eval("UserFirstName") +" "+ Eval("UserLastName") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField >
                    <HeaderTemplate>
                        <asp:Label ID="lblEmailName" runat="server" CommandArgument="LoginEmail" CommandName="Sort">Email</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEmail" Text='<%# Eval("LoginEmail") %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle CssClass="centeralign" />
                    <HeaderTemplate>
                        <span>Hire Date</span>
                    </HeaderTemplate>
                    <HeaderStyle />
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTotal" Text='<%# Convert.ToDateTime(Eval("HiredDate")).ToShortDateString() %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
                <asp:TemplateField >
                    <HeaderTemplate>
                        <span>Eligible</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEligible" Text='<%#  Convert.ToString(Eval("IsEligible")) == "1" ? "Yes" : "No"%>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" />
                </asp:TemplateField>
                <asp:TemplateField >
                    <HeaderTemplate>
                        <span>Order Placed</span>
                        <div class="corner">
                            <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblOrderPlaced" Text='<%# Convert.ToString(Eval("IsPlacedOrder")) == "1" ? "Yes" : "No" %>' />
                        <div class="corner">
                            <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                        </div>
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="spacer10">
        </div>
        <div class="alignleft" id="dvButtonControler" runat="server">
            <asp:LinkButton ID="lnkBtnEmails" runat="server" class="grey_btn" OnClick="lnkBtnEmails_Click"><span>Email Reminder</span></asp:LinkButton>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
