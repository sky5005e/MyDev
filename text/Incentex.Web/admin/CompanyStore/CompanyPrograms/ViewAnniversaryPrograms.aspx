<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewAnniversaryPrograms.aspx.cs" Inherits="admin_CompanyStore_CompanyPrograms_ViewAnniversaryPrograms"
    Title="Company Anniversary Programs" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

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

    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }
    </script>

    <mb:MenuUserControl ID="menuControl" runat="server" />
    <asp:UpdatePanel runat="server" ID="upnlCompanyStore">
        <ContentTemplate>
            <div class="form_pad">
                <div>
                    <div style="text-align: center; color: Red; font-size: larger;">
                        <asp:Label ID="lblmsg" runat="server">
                        </asp:Label>
                    </div>
                    <asp:GridView ID="gvCompanyProgram" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None"  RowStyle-CssClass="ord_content"
                        OnRowCommand="gvCompanyProgram_RowCommand" OnRowDataBound="gvCompanyProgram_RowDataBound">
                        <Columns>
                            <asp:TemplateField Visible="False" HeaderText="Id">
                                <HeaderTemplate>
                                    <span>StoreID</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStoreID" Text='<%# Eval("StoreID") %>' />
                                    <asp:Label runat="server" ID="lblAnniversaryCreditProgramID" Text='<%# Eval("AnniversaryCreditProgramID") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="3%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span>
                                        <asp:CheckBox ID="cbSelectAll" runat="server" OnClick="selectAll(this)" />&nbsp;
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </span>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                    <span class="first">
                                        <asp:CheckBox ID="chkStore" runat="server" />&nbsp; </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="3%" CssClass="b_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Workgroup">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnWorkgroup" runat="server" CommandArgument="Workgroup" CommandName="Sort"><span >Workgroup</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderWorkgroup" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="hypWorkgroup" CommandName="EditProgram" CommandArgument='<%# Eval("AnniversaryCreditProgramID") %>'
                                        runat="server"><span><%# Eval("Workgroup")%></span></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Department">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnDepartment" runat="server" CommandArgument="Department"
                                        CommandName="Sort"> <span >Department</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderDepartment" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDepartment" Text='<%# Eval("Department") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="StandardCreditAmount">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnStandardCreditAmount" runat="server" CommandArgument="StandardCreditAmount"
                                        CommandName="Sort"><span>Annual Credit Amount</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderStandardCreditAmount" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStandardCreditAmount" Text='<%# Eval("StandardCreditAmount") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="IssueCreditIn">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnIssueCreditIn" runat="server" CommandArgument="IssueCreditIn"
                                        CommandName="Sort"><span>Credit Issued (Months)</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderIssueCreditIn" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblIssueCreditIn" Text='<%# Eval("IssueCreditIn") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="CreditExpiresIn">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCreditExpiresIn" runat="server" CommandArgument="CreditExpiresIn"
                                        CommandName="Sort"><span>Expires After (Months)</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderCreditExpiresIn" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblIssueCreditExpiresIn" Text='<%# Eval("CreditExpiresIn") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="EmployeeStatus">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnProgramStatus" runat="server" CommandArgument="EmployeeStatus"
                                        CommandName="Sort"><span>Status</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderProgramStatus" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnLookupIcon" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "IconPath")%>' />
                                    <asp:HiddenField ID="hdnStatusID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "EmployeeStatus")%>' />
                                    <asp:LinkButton ID="lblStatus" runat="server" Text="" CommandName="StatusVhange"
                                        class="btn_space">
                                        <span class="btn_space">
                                            <img id="imgLookupIcon" style="height: 20px; width: 20px" runat="server" alt='Loading' /></span></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box centeralign" Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Updatestatus" >
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnProgramStatus1" runat="server" CommandArgument="EmployeeStatus"
                                        CommandName="Sort"><span>Update</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderProgramStatus1" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblUpdateStatus1" runat="server" Text="" CommandName="UpdateStatus"
                                        CommandArgument='<%# Eval("AnniversaryCreditProgramID") %>' class="btn_space">Update
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box centeralign" Width="6%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <div class="companylist_botbtn alignleft">
                        <asp:LinkButton ID="btnAddAnniversaryProg" runat="server" TabIndex="0" CssClass="grey_btn"
                            OnClick="btnAddAnniversaryProg_Click"><span>Add Anniversary Program</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" CssClass="grey_btn" runat="server" TabIndex="0" OnClientClick="return DeleteConfirmation();"
                            OnClick="btnDelete_Click"><span>Delete</span></asp:LinkButton>
                    </div>
                    <div id="pagingtable" runat="server" class="alignright pagging">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server"> 
                        </asp:LinkButton>
                        <span>
                            <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList></span>
                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
