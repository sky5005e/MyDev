<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ReturnProductIE.aspx.cs" Inherits="ProductReturnManagement_ReturnProductIA"
    Title="Product Return View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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

    <div class="form_pad">
        <div>
            <div style="text-align: center; color: Red; font-size: larger;">
                <asp:Label ID="lblmsg" runat="server">
                </asp:Label>
            </div>
            <div class="form_pad" style="padding-top: 0px !important;">
                <div>
                    <div style="text-align: center; color: Red; font-size: larger;">
                    </div>
                    <asp:GridView ID="gvProductReturn" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvProductReturn_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span>
                                        <asp:CheckBox ID="cbSelectAll" runat="server" OnClick="selectAll(this)" />&nbsp;
                                    </span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                    <span class="first">
                                        <asp:CheckBox ID="CheckBox1" runat="server" />&nbsp; </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="5%" CssClass="b_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="OrderNumber">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnOrderNumber" runat="server" CommandArgument="OrderNumber"
                                        CommandName="Sort"><span >Return #</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderOrderNumber" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                    <asp:HyperLink ID="hypOrderNumber" CommandArgument='<%# Eval("OrderNumber") %>' CommandName="Edit"
                                        runat="server"><span><%# Eval("OrderNumber")%></span></asp:HyperLink>
                                    <asp:HiddenField ID="hdnOrderNumber" runat="server" Value='<%# Eval("OrderNumber") %>' />
                                    <asp:HiddenField ID="hdnOrderID" runat="server" Value='<%# Eval("OrderID") %>' />
                                    <asp:HiddenField ID="hdnProductReturnId" runat="server" Value='<%# Eval("ProductReturnId") %>' />
                                    <asp:HiddenField ID="hdnShippID" runat="server" Value='<%# Eval("ShippID") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="SubmitDate">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnSubmitDate" runat="server" CommandArgument="SubmitDate"><span>Submit Date</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderSubmitDate" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSubmitDate" Text='<%# Eval("SubmitDate","{0:d}") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Contact">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnContact" runat="server" CommandArgument="Contact" CommandName="Sort"><span >Contact</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderContact" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblContact" Text='<%# Eval("Contact") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="28%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Company">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnCompany" runat="server" CommandArgument="Company" CommandName="Sort"> <span >Company</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderCompany" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCompany" Text='<%# Eval("CompanyName") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Status">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtStatus" runat="server" CommandArgument="Status" CommandName="Sort"><span>Status</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderStatus" runat="server"></asp:PlaceHolder>
                                    <div class="corner">
                                        <span class="ord_headtop_cr"></span><span class="ord_headbot_cr"></span>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnStatus" Value='<%# Eval("Status") %>' />
                                    <span style="height: 26px;">
                                        <asp:DropDownList runat="server" ID="ddlStatus" Style="background-color: #303030;
                                            border: medium none; color: #ffffff; width: 135px; padding: 2px; margin-top: 3px;" runat="server">
                                        </asp:DropDownList>
                                    </span>
                                    <div class="corner">
                                        <span class="ord_blacktop_cr"></span><span class="ord_blackbot_cr"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="10%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="companylist_botbtn alignleft">
                    <asp:LinkButton ID="btnSaveStatus" runat="server" TabIndex="0" CssClass="grey_btn"
                        OnClick="btnSaveStatus_Click"><span>Save Status</span>
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
