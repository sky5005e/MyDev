<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewIncentexEmployee.aspx.cs" Inherits="admin_UserManagement_ViewIncentexEmployee"
    Title="World-Link System-View Incentex Employee" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }
    </script>

    <%-- <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            $("tr").filter(function() {
                return $('td', this).length && !$('table', this).length
            })
            .click(function() {
                __doPostBack('javaScriptEvent', $(this).find("span").text());
                __doPostBack('javaScriptEvent', $(this).find("span").text());
            })
            .mouseover(function() {
                $(this).css("cursor", "hand");
            })
            .css({ background: "" }).hover(
                function() { $(this).css({ background: "" }); },
                function() { $(this).css({ background: "" }); }
                );
        }); 
    </script>--%>
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>

    <script type="text/javascript">
        // Let's use a lowercase function name to keep with JavaScript conventions
        function selectAll(invoker) {
            // Since ASP.NET checkboxes are really HTML input elements
            //  let's get all the inputs
            var inputElements = document.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                // Filter through the input types looking for checkboxes
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        } 
    </script>

    <div class="form_pad">
        <asp:UpdatePanel ID="upPanel" runat="server">
            <ContentTemplate>
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                </div>
                <asp:GridView ID="gvIncentexEmployee" runat="server" AutoGenerateColumns="false"
                    HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                    RowStyle-CssClass="ord_content" OnPageIndexChanging="gvIncentexEmployee_PageIndexChanging"
                    OnRowCommand="gvIncentexEmployee_RowCommand" OnRowDataBound="gvIncentexEmployee_RowDataBound">
                    <Columns>
                        <asp:TemplateField Visible="false" HeaderText="Id">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompanyEmployeeID" Text='<%# Eval("IncentexEmployeeID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Check">
                            <HeaderTemplate>
                                <span class="custom-checkbox gridheader">
                                    <asp:CheckBox ID="cbSelectAll" runat="server" />&nbsp;
                                </span>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="b_box centeralign" />
                            <ItemTemplate>
                                <span class="first custom-checkbox gridcontent">
                                    <asp:CheckBox ID="chkemployee" runat="server" />&nbsp;
                                </span>
                                <div class="corner">
                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="5%" CssClass="b_box centeralign" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="EmployeeName">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnEmployeeName" runat="server" CommandArgument="EmployeeName"
                                    CommandName="Sort"><span>Employee Name</span></asp:LinkButton>
                                <%--<div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>--%>
                                <asp:PlaceHolder ID="placeholderEmployeeName" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%--<span class="first">--%>
                                <%--<asp:CheckBox ID="chkemployee" runat="server" />--%>
                                <asp:LinkButton ID="lnkbtnfullname" CommandName="viewemployeeinfo" CommandArgument='<%# Eval("IncentexEmployeeID") %>'
                                    runat="server"><span><%# Eval("EmployeeName") %></span></asp:LinkButton>
                                <%--</span>--%><%--<div class="corner">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>--%>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Country">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnCountry" runat="server" CommandArgument="Country" CommandName="Sort"><span class="white_co">Country</span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderCountry" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCountry" Text='<% # (Convert.ToString(Eval("Country")).Length > 30) ? Eval("Country").ToString().Substring(0,30)+"..." : Convert.ToString(Eval("Country"))+ "&nbsp;"  %>'
                                    ToolTip='<% #Eval("Country")  %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="State">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnState" runat="server" CommandArgument="State" CommandName="Sort">
                                <span>State</span>
                                </asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderState" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStationManager" Text='<% # (Convert.ToString(Eval("State")).Length > 30) ? Eval("State").ToString().Substring(0,30)+"..." : Convert.ToString(Eval("State"))+ "&nbsp;"  %>'
                                    ToolTip='<% #Eval("State")  %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Mobile">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtncntctnum" runat="server" CommandArgument="Mobile" CommandName="Sort">
                        <span class="white_co">Contact no.</span>
                                </asp:LinkButton>
                                <asp:PlaceHolder ID="placeholdercontactnumber" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblMobile" Text='<% # (Convert.ToString(Eval("Mobile")).Length > 30) ? Eval("Mobile").ToString().Substring(0,30)+"..." : Convert.ToString(Eval("Mobile"))+ "&nbsp;"  %>'
                                    ToolTip='<% #Eval("Mobile")  %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="IsDirectEmployee">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnEmployeeType" runat="server" CommandArgument="IsDirectEmployee"
                                    CommandName="Sort">
                                <span class="white_co">Employee Type</span>
                                <div class="corner"><span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span></div>
                                </asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderEmployeeType" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblEmployeeType" Text='<% # (Convert.ToString(Eval("IsDirectEmployee")).Length > 40) ? Eval("IsDirectEmployee").ToString().Substring(0,40)+"..." : Convert.ToString(Eval("IsDirectEmployee"))+ "&nbsp;"  %>'
                                    ToolTip='<% #Eval("IsDirectEmployee")  %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div>
                    <div class="companylist_botbtn alignleft">
                        <asp:LinkButton ID="lnkBtnAddEmployee" OnClick="lnkBtnAddEmployee_Click" runat="server"
                            class="grey_btn"><span>Add Incentex Employee</span></asp:LinkButton>
                        <asp:LinkButton ID="lnkBtnDelete" OnClick="lnkBtnDelete_Click" OnClientClick="return DeleteConfirmation();"
                            runat="server" class="grey_btn"><span>Delete</span></asp:LinkButton>
                    </div>
                    <div class="alignright pagging" id="dvPaging" runat="server">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                        </asp:LinkButton>
                        <span>
                            <asp:DataList ID="dtlViewEmployee" runat="server" CellPadding="1" CellSpacing="1"
                                OnItemCommand="dtlViewEmployee_ItemCommand" OnItemDataBound="dtlViewEmployee_ItemDataBound"
                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList>
                        </span>
                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
