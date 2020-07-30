<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewFAQ.aspx.cs" Inherits="admin_CompanyStore_ViewFAQ" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <asp:Repeater ID="rptFAQ" runat="server">
            <ItemTemplate>
                <div>
                    <div class="headersmall_bg">
                        <div class="headersmall_bgr title">
                            <asp:Panel ID="pnlQuestion" runat="server">
                                <strong>Q.</strong> <span>
                                    <%# Eval("FaqQuestion")%></span>
                            </asp:Panel>
                            <at:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlAnswer"
                                CollapseControlID="pnlQuestion" ExpandControlID="pnlQuestion" CollapsedSize="0">
                            </at:CollapsiblePanelExtender>
                        </div>
                    </div>
                    <div class="spacer10">
                    </div>
                    <asp:Panel ID="pnlAnswer" runat="server">
                        <div class="black_top_co">
                            <span>&nbsp;</span></div>
                        <div runat="server" class="black_middle order_detail_pad">
                            <span>
                                <%# Eval("FaqAnswer")%></span>
                        </div>
                        <div class="black_bot_co">
                            <span>&nbsp;</span></div>
                    </asp:Panel>
                </div>
            </ItemTemplate>
            <SeparatorTemplate>
                <div class="spacer20">
                </div>
            </SeparatorTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
