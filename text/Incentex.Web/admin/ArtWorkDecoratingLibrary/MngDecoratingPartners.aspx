<%@ Page Title="Decorating Partners" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="MngDecoratingPartners.aspx.cs" Inherits="admin_ArtWorkDecoratingLibrary_MngDecoratingPartners" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
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

   
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleContainerContent").hide();

        });   
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>

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
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" AllowSorting="true">
                        <Columns>
                            <asp:TemplateField Visible="false" HeaderText="Id">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblID" Text='<%# Eval("SupplierID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span>&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this);"
                                        runat="server" /></span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="first">&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Middle" CssClass="b_box" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCompanyWebsite" runat="server" CommandArgument="CompanyWebsite"
                                        CommandName="Sorting"><span class="white_co">Company Name</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompanyName" Text='<% # Eval("CompanyName")%>' ToolTip='<% #Eval("CompanyName")  %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkFirstName" runat="server" CommandArgument="FirstName" CommandName="Sorting"><span >Owner Information</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>
                                        <asp:HyperLink ID="lnkEditSupp" runat="server" NavigateUrl='<%# "~/admin/ArtWorkDecoratingLibrary/MainCompanyPartners.aspx?Id=" + Eval("SupplierID").ToString()%>'
                                            Text='<%#Eval("Fullname")%>'>  </asp:HyperLink>
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCountryNamee" runat="server" CommandArgument="sCountryName"
                                        CommandName="Sorting"><span>Country</span></asp:LinkButton>
                                    <div class="corner">
                                        <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCountry" Text='<%# Eval("CountryName")%>'></asp:Label>
                                    <div class="corner">
                                        <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" />
                            </asp:TemplateField>
                            <%-- <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkStatus" runat="server" CommandArgument="Status" CommandName="Sorting"><span class="white_co">Status</span></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("Status")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" />
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <div class="companylist_botbtn alignleft">
                        <asp:HyperLink ID="lnkAddStation" runat="server" class="grey_btn" NavigateUrl="~/admin/ArtWorkDecoratingLibrary/MainCompanyPartners.aspx?Id=0">
						                <span>Add Company</span>
                        </asp:HyperLink>
                    </div>
                    <div id="pagingtable" runat="server" class="alignright pagging">
                        <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                        </asp:LinkButton>
                        <span>
                            <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
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
