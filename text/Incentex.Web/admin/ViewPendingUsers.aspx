<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewPendingUsers.aspx.cs" Inherits="MyAccount_ViewPendingUsers" Title="View Pending Users" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .collapsibleContainerContent
        {
            padding: 10px 15px 10px 15px;
        }
    </style>

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
        
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected user?") == true)
                return true;
            else
                return false;
        }

        function RejectConfirmation() {
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
//        function changeAll(chk) {
//            var parent = getParentByTagName(chk, "table");
//            var checkBoxes = parent.getElementsByTagName("input");
//            for (var i = 0; i < checkBoxes.length; i++)
//                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].id.indexOf("chkSelectUser") >= 0)
//                checkBoxes[i].checked = chk.checked;
//        }
//        function getParentByTagName(obj, tag) {
//            var obj_parent = obj.parentNode;
//            if (!obj_parent) return false;
//            if (obj_parent.tagName.toLowerCase() == tag) return obj_parent;
//            else return getParentByTagName(obj_parent, tag);
//        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsgGlobal" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div style="text-align: right; color: #5C5B60; font-size: larger; font-weight: bold;
            padding-left: 15px;">
            Total Records :
            <asp:Label ID="lblRecords" runat="server"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <asp:Repeater ID="dtlStore" runat="server" OnItemDataBound="dtlStore_ItemDataBound">
            <ItemTemplate>
                <div class="collapsibleContainer" title="<%# DataBinder.Eval(Container.DataItem, "Company")%>"
                    total="<%# DisplayTotal(Convert.ToString(DataBinder.Eval(Container.DataItem, "CompanyId"))) %>">
                    <div style="text-align: center">
                        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                    </div>
                    <asp:HiddenField ID="hdnCompanyId" Value='<%# DataBinder.Eval(Container.DataItem, "CompanyId")%>'
                        runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnStoreId" Value='<%# DataBinder.Eval(Container.DataItem, "StoreId")%>'
                        runat="server"></asp:HiddenField>
                    <asp:Repeater ID="dtlWorkGroup" runat="server" OnItemDataBound="dtlWorkGroup_ItemDataBound"
                        OnItemCommand="dtlWorkGroup_ItemCommand">
                        <ItemTemplate>
                            <br />
                            <div id="dvWorkGroup" runat="server" class="collapsibleInnerContainer" title='<%# Eval("WorkGroup") %>'>
                                <div style="text-align: center">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                                </div>
                                <asp:HiddenField ID="hdnWorkGroupID" Value='<%# DataBinder.Eval(Container.DataItem, "WorkGroupID")%>'
                                    runat="server"></asp:HiddenField>
                                <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="grdView_RowCommand"
                                    OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblRegistrationId" Text='<%# Eval("iRegistraionID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Check">
                                            <HeaderTemplate>
                                              <span class="custom-checkbox gridheader">
                                                    <asp:CheckBox ID="cbSelectAll" runat="server" />&nbsp;</span>
                                                <div class="corner">
                                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                                </div>
                                            </HeaderTemplate>
                                            <HeaderStyle CssClass="centeralign" />
                                            <ItemTemplate>
                                                <span class="first custom-checkbox gridcontent">
                                                    <asp:CheckBox ID="chkSelectUser" runat="server" />&nbsp; </span>
                                                <div class="corner">
                                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" CssClass="g_box centeralign" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="EmployeeId">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnEmployeeId" runat="server" CommandArgument="sEmployeeId"
                                                    CommandName="Sort"><span>Employee Id</span></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblContact" Text='<%# Eval("sEmployeeId") %>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="sFirstName">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnFullName" runat="server" CommandArgument="sFirstName" CommandName="Sort"><span>Full Name</span></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span>
                                                    <asp:LinkButton runat="server" ID="lblFirstName" Text='<%# Eval("sFirstName") + " " + Eval("sLastName") %>'
                                                        CommandArgument='<%# Eval("iRegistraionID") %>' CommandName="Detail" /></span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnEmploeeType" runat="server" CommandArgument="EmployeeType"
                                                    CommandName="Sort"><span>Employee Type</span></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmployeeType" Text='<%# "&nbsp;" + Eval("EmployeeType") %>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="BaseStationName">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnBaseStationName" runat="server" CommandArgument="BaseStationName"
                                                    CommandName="Sort"><span>Base Station</span></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblBaseStation" Text='<%# "&nbsp;" + Eval("BaseStationName") %> ' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="g_box" Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmail" Text='<%# Eval("sEmailAddress") %>' />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="spacer15">
                                </div>
                                <div class="alignleft" id="dvButtonControler" runat="server">
                                    <asp:LinkButton ID="lnkBtnApproveUser" CommandName="Approve" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "WorkGroupID")%>'
                                        runat="server" OnClientClick="return ApproveConfirmation();" class="grey_btn action"><span>Approve</span></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnApproveAndApplyPassword" CommandName="ApproveAndApplyPassword"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "WorkGroupID")%>' OnClientClick="return ApproveConfirmation();"
                                        runat="server" class="grey_btn action"><span>Approve & Apply Password</span></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnRejectUser" CommandName="Reject" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "WorkGroupID")%>'
                                        OnClientClick="return RejectConfirmation();" runat="server" class="grey_btn action"><span>Reject</span></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnDelete" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "WorkGroupID")%>'
                                        OnClientClick="return DeleteConfirmation();" runat="server" class="grey_btn action"><span>Delete</span></asp:LinkButton>
                                </div>
                                <div style="clear: both;">
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <br />
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
