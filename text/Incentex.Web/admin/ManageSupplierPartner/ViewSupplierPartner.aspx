<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewSupplierPartner.aspx.cs" Inherits="admin_ManageSupplierPartner_ViewSupplierPartner"
    Title="Supplier Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
    
    function SelectAllCheckboxes(spanChk) {
        var IsChecked = spanChk.checked;
        var Chk = spanChk;
        Parent = Chk.form.elements;
        for (i = 0; i < Parent.length; i++) {
            if (Parent[i].type == "checkbox" && Parent[i].id != Chk.id) {
                if (Parent[i].checked != IsChecked)
                    Parent[i].click();
            }
        }
    }
    function SelectAllCheckboxesSpecific(spanChk) {
        var IsChecked = spanChk.checked;
        var Chk = spanChk;
        Parent = document.getElementById('ctl00_ContentPlaceHolder1_gv');
        var items = Parent.getElementsByTagName('input');
        for (i = 0; i < items.length; i++) {
            if (items[i].id != Chk && items[i].type == "checkbox") {
                if (items[i].checked != IsChecked) {
                    items[i].click();
                }
            }
        }
    }

    function SelectAllCheckboxesMoreSpecific(spanChk) {
        var IsChecked = spanChk.checked;
        var Chk = spanChk;
        Parent = document.getElementById('ctl00_ContentPlaceHolder1_gvIncentexEmployee');
        for (i = 0; i < Parent.rows.length; i++) {
            var tr = Parent.rows[i];
            //var td = tr.childNodes[0];			   		   
            var td = tr.firstChild;
            var item = td.firstChild;
            if (item.id != Chk && item.type == "checkbox") {
                if (item.checked != IsChecked) {
                    item.click();
                }
            }
        }
    }


    function HighlightRow(chkB) {
        var IsChecked = chkB.checked;
        if (IsChecked) {
            chkB.parentElement.parentElement.style.backgroundColor = '#228b22';  // grdEmployees.SelectedItemStyle.BackColor
            chkB.parentElement.parentElement.style.color = 'white'; // grdEmployees.SelectedItemStyle.ForeColor
        }
        else {
            chkB.parentElement.parentElement.style.backgroundColor = 'white'; //grdEmployees.ItemStyle.BackColor
            chkB.parentElement.parentElement.style.color = 'black'; //grdEmployees.ItemStyle.ForeColor
        }
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div class="form_pad">
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                    <br />
                </div>
                <div>
                    <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gv_RowDataBound"
                        AllowSorting="true" OnRowCommand="gv_RowCommand">
                        <Columns>
                            <asp:TemplateField Visible="false" HeaderText="Id">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblID" Text='<%# Eval("SupplierPartnerID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span>&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);"
                                        runat="server" />
                                        &nbsp; </span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="first">&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </span>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Middle" CssClass="b_box" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkFirstName" runat="server" CommandArgument="Name" CommandName="Sorting"><span >Supplier Name</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--<asp:Label runat="server" ID="lblSupplierName" Text=<%#Eval("FirstName") + " " + Eval("LastName") %> ></asp:Label>--%>
                                    <span>
                                        <asp:HyperLink ID="lnkEditSupp" runat="server" NavigateUrl='<%# "~/admin/ManageSupplierPartner/AddSuplier.aspx?Id=" + Eval("SupplierPartnerID").ToString()%>'
                                            Text='<%#Eval("Name") %>'>  </asp:HyperLink>
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCompanyWebsite" runat="server" CommandArgument="URL" CommandName="Sorting"><span class="white_co">Company Website</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompanyWebsite" Text='<% # (Convert.ToString(Eval("URL")).Length > 15) ? Eval("URL").ToString().Substring(0,15)+"..." : Convert.ToString(Eval("URL"))+ "&nbsp;"  %>'
                                        ToolTip='<% #Eval("URL")  %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCountryNamee" runat="server" CommandArgument="LoginName" CommandName="Sorting"><span>LoginName</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLoginName" Text='<%# Eval("LoginName")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkPassword" runat="server" CommandArgument="Password" CommandName="Sorting"><span class="white_co">Password</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPassword" Text='<%# Eval("Password")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkStatus" runat="server" CommandArgument="Status" CommandName="Sorting"><span class="white_co">Status</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="centeralign">
                                        <asp:HiddenField ID="hdnStatus" runat="server" Value='<%# Eval("Status")%>' />
                                        <asp:ImageButton ID="imgApprove" runat="server" Text="" CommandName="Approve" CommandArgument='<%# Eval("SupplierPartnerID") %>'
                                            ImageUrl="~/admin/Incentex_Used_Icons/EmploymentStatus _Active-Status.png" ToolTip="Active"
                                            Height="20" Width="20" class="btn_space"></asp:ImageButton>
                                        <asp:ImageButton ID="imgReject" runat="server" Text="" CommandName="Reject" CommandArgument='<%# Eval("SupplierPartnerID") %>'
                                            ImageUrl="~/admin/Incentex_Used_Icons/EmploymentStatus _Deactivated.png" ToolTip="Reject"
                                            Height="20" Width="20" class="btn_space"></asp:ImageButton>
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCratedDate" runat="server" CommandArgument="CratedDate" CommandName="Sorting"><span class="white_co">CratedDate</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCratedDate" Text='<%# Convert.ToDateTime(Eval("CratedDate")).ToShortDateString() %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <div class="companylist_botbtn alignleft">
                        <asp:HyperLink ID="lnkAddStation" runat="server" class="grey_btn" NavigateUrl="~/admin/ManageSupplierPartner/AddSuplier.aspx?Id=0">
						                <span>Add Supplier</span>
                        </asp:HyperLink>
                        <asp:LinkButton ID="lnkDelete" runat="server" class="grey_btn" OnClick="lnkDelete_Click"
                            OnClientClick="return confirm('Are you sure, you want to delete selected records ?')">
						                <span>Delete</span>
                        </asp:LinkButton>
                    </div>
                    <div class="alignright pagging">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                        </asp:LinkButton>
                        <span>
                            <asp:DataList ID="lstPaging" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="lstPaging_ItemCommand"
                                OnItemDataBound="lstPaging_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList></span>
                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
