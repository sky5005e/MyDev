<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewEmployee.aspx.cs" Inherits="admin_Company_Employee_ViewEmployee"
    Title="View Company Employee" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <%-- <script type="text/javascript">
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
    </script>--%>
    <div class="form_pad">
        <asp:UpdatePanel ID="upPanel" runat="server">
            <ContentTemplate>
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                </div>
                <table>
                    <tr>
                        <td style="text-align: left; width: 50%;" class="errormessage">
                            <asp:Label ID="lblWorkgroupName" runat="server" CssClass="errormessage"></asp:Label>
                            <asp:HiddenField ID="hdnWorkGroupName" runat="server" />
                        </td>
                        <td style="text-align: right; width: 50%;" class="errormessage">
                            Total Records :
                            <asp:Label ID="lblRecords" Text="0" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvEmployee" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnPageIndexChanging="gvEmployee_PageIndexChanging"
                    OnRowCommand="gvEmployee_RowCommand" OnRowDataBound="gvEmployee_RowDataBound">
                    <Columns>
                        <asp:TemplateField Visible="false" HeaderText="Id">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompanyEmployeeID" Text='<%# Eval("CompanyEmployeeID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="FullName">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnCusName" runat="server" CommandArgument="FullName" CommandName="Sort"><span>Employee Name</span></asp:LinkButton>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                                <asp:PlaceHolder ID="placeholderEmployeeName" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnfullname" CommandName="vieweditces" CommandArgument='<%# Eval("CompanyEmployeeID") %>'
                                    runat="server">
                                    <span class="first">
                                        <%# Eval("FullName") %>
                                        &nbsp; &nbsp;
                                        <img id="imgNew" src="../../Images/New_Old.png" title="Menu setup for this employee is required"
                                            height="20" width="20" runat="server" visible="false" alt="load" />
                                    </span>
                                </asp:LinkButton>
                                <div class="corner">
                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Country">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnCountry" runat="server" CommandArgument="Country" CommandName="Sort"><span class="white_co">Country</span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderCountry" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCountry" Text='<%# Eval("Country") + "&nbsp;" %>' />
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
                                <asp:Label runat="server" ID="lblStationManager" Text='<%# Eval("State") + "&nbsp;" %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Telephone">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtncntctnum" runat="server" CommandArgument="Telephone" CommandName="Sort">
                        <span class="white_co">Contact no.</span>
                         <div class="corner"><span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span></div>
                                </asp:LinkButton>
                                <asp:PlaceHolder ID="placeholdercontactnumber" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="btn_box">
                                    <asp:Label runat="server" ID="lblStationAdmin" Text='<%# Eval("Telephone") %>' />
                                </span>
                                <div class="corner">
                                    <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="WLSStatusId">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnProductStatus" runat="server" CommandArgument="WLSStatusId"
                                    CommandName="Sort"><span>Employee Status</span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderProductStatus" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnLookupIcon" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "IconPath")%>' />
                                <asp:HiddenField ID="hdnStatusID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "WLSStatusId")%>' />
                                <asp:LinkButton ID="lblStatus" runat="server" Text="" CommandName="ChangeStatus"
                                    class="btn_space">
                                    <span class="btn_space">
                                        <img id="imgLookupIcon" style="height: 20px; width: 20px" runat="server" alt='Loading' /></span></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box centeralign" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div>
                    <div>
                        <div class="companylist_botbtn alignleft">
                            <asp:LinkButton ID="lnkBtnAddEmployee" OnClick="lnkBtnAddEmployee_Click" runat="server"
                                class="grey_btn"><span>Add Employee</span></asp:LinkButton>
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
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
