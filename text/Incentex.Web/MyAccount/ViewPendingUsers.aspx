<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewPendingUsers.aspx.cs" Inherits="MyAccount_ViewPendingUsers" Title="View Pending Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $().ready(function() {
        });
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to reject selected user?") == true)
                return true;
            else
                return false;
        }

        function ApproveConfirmation() {
            if (confirm("Are you sure, you want to approve selected user?") == true)
                return true;
            else
                return false;
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
    
    <style type="text/css">
    .orderreturn_box .ord_content td span
    {
        height:25px;	
    }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsgGlobal" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <asp:GridView ID="gvPendingUserList" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
            CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvPendingUserList_RowCommand">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblRegistrationId" Text='<%# Eval("iRegistraionID") %>' />
                    </ItemTemplate>
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
                    <ItemStyle Width="5%" CssClass="g_box centeralign" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnEmployeeId" runat="server" CommandArgument="sEmployeeId"
                            CommandName="Sort"><span>Employee Id</span></asp:LinkButton>
                    </HeaderTemplate>
                     <ItemTemplate>
                        <asp:Label runat="server" ID="lblContact" Text='<%# Convert.ToString(Eval("sEmployeeId")).Length > 20 ?  Convert.ToString(Eval("sEmployeeId")).Substring(0,19) + "..." : Convert.ToString(Eval("sEmployeeId")) %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnFullName" runat="server" CommandArgument="sFirstName" CommandName="Sort"><span>Full Name</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span>
                            <asp:LinkButton runat="server" ID="lblFirstName" Text='<%# Eval("EmployeeName") %>'
                                CommandArgument='<%# Eval("iRegistraionID") %>' CommandName="Detail" />
                        </span>
                        <asp:HiddenField runat="server" ID="hdnEmail" Value='<%# Eval("sEmailAddress") %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="18%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnEmploeeType" runat="server" CommandArgument="EmployeeType"
                            CommandName="Sort"><span>Employee Type</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEmployeeType" Text='<%# Eval("EmployeeType") %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="18%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnBaseStationName" runat="server" CommandArgument="BaseStationName"
                            CommandName="Sort"><span>Base Station</span></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblBaseStation" Text='<%#  "&nbsp;" + Eval("BaseStation") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="20%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>WorkGroup</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblWorkgroup" Text='<%#  "&nbsp;" + Eval("WorkGroup") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="b_box" Width="20%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <span>Approver Level</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblApproverLevel" Text='<%#  "&nbsp;" + Eval("ApproverLevel") %> ' />
                    </ItemTemplate>
                    <ItemStyle CssClass="g_box" Width="9%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="spacer15">
        </div>
        <div class="alignleft" id="dvButtonControler" runat="server">
            <asp:LinkButton ID="lnkBtnApproveUser" OnClick="lnkbtnApproveUser_Click"
                runat="server" OnClientClick="return ApproveConfirmation();" class="grey_btn"><span>Approve</span></asp:LinkButton>
            <asp:LinkButton ID="lnkBtnRejectUser" OnClick="lnkBtnRejectUser_Click"
                OnClientClick="return DeleteConfirmation();" runat="server" class="grey_btn"><span>Reject</span></asp:LinkButton>
            <asp:LinkButton ID="lnkbtnApproveAndApplyPassword" OnClick="lnkbtnApproveAndApplyPassword_Click" OnClientClick="return ApproveConfirmation();"
                runat="server" class="grey_btn"><span>Approve & Apply Password</span></asp:LinkButton>
        </div>
        <div style="clear: both;">
        </div>
    </div>
</asp:Content>
