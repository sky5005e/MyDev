<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/admin/MasterPageAdmin.master"
    CodeFile="ViewPendingShoppingCart.aspx.cs" Inherits="admin_CommunicationCenter_ViewPendingShoppingCart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleContainerContent").each(function() {
                var hdnIamExpanded = document.getElementById($(this).parent().attr("id").substring(0, $(this).parent().attr("id").length - 13) + "hdnIamExpanded");
                if (hdnIamExpanded.value == "true") {
                    $(this).show();
                }
                else if (hdnIamExpanded.value == "false") {
                    $(this).hide();
                }
            });
        });            
            
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
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsgGlobal" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div style="text-align: right; padding-right: 30px; color: #72757C">
            <asp:Label ID="lblCount" runat="server"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <asp:Repeater ID="dtlCompany" runat="server" OnItemDataBound="dtlCompany_ItemDataBound">
            <ItemTemplate>
                <div style="clear: both;" runat="server" id="dvCollapsible" class="collapsibleContainer"
                    title='<%# Eval("CompanyName")%>' total='<%# Eval("TotalPending") %>'>
                    <div style="text-align: center">
                        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                    </div>
                    <asp:HiddenField ID="hdnIamExpanded" runat="server" Value="false" />
                    <asp:HiddenField ID="hdnCompanyId" Value='<%# DataBinder.Eval(Container.DataItem, "CompanyID")%>'
                        runat="server"></asp:HiddenField>
                    <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblUserInfoID" Text='<%# Eval("UserInfoID") %>' />
                                    <asp:Label runat="server" ID="lblMyShoppingCartID" Text='<%# Eval("MyShoppingID") %>' />
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
                                <ItemStyle Width="5%" CssClass="b_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblFullName" runat="server">Full Name</span></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFirstName" Text='<%# Eval("FullName") %> ' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="30%"/>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEmailName" runat="server" >Email</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEmail" Text='<%# Eval("LoginEmail") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" />
                            </asp:TemplateField>
                             <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblWorkgroupName" runat="server">Workgroup</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblWorkgroupName" Text='<%# Eval("WorkGroupName") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="30%"/>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCartStartedDate" runat="server">Cart Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCartStartedDate" Text='<%# String.Format("{0:MM/dd/yyyy}",Eval("CreatedDate")== null ? "Null" : Eval("CreatedDate")) %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle CssClass="centeralign" />
                                <HeaderTemplate>
                                    <span>Total Dollar Value</span>
                                    <div class="corner">
                                        <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                    </div>
                                </HeaderTemplate>
                                <HeaderStyle />
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTotalDollar" Text='<%# String.Format("{0:0,0.00}",Eval("ValueofCart"))%>' />
                                    <div class="corner">
                                        <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box centeralign" Width="20%" />
                            </asp:TemplateField>
                            
                        </Columns>
                    </asp:GridView>
                    <div class="spacer10">
                    </div>
                    <div class="alignleft" id="dvButtonControler" runat="server">
                        <asp:LinkButton ID="lnkBtnEmails" CommandName="sendMail" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CompanyID")%>'
                            runat="server" class="grey_btn" OnClick="lnkBtnEmails_Click"><span>Email Reminder</span></asp:LinkButton>
                    </div>
                    <div style="text-align: right; padding-right: 30px; color: #72757C" id="dvTotalDollarValue"
                        runat="server" visible="false">
                        <span>Grand Total : $</span>
                        <asp:Label ID="LblGrandTotal" runat="server" />
                    </div>
                    <div style="clear: both;">
                    </div>
                </div>
                <br />
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
