<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="SAPBillingAddressSetting.aspx.cs" Inherits="SAPBillingAddressSetting"
    Title="SAP Billing Address Setting" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        $(window).load(function() {
            $("#dvLoader").hide();
        });

        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
        
        function DeleteConfirmation() {
            Removerules();
            if (confirm("Are you sure, you want to delete selected records ?") == true) {
                $('#dvLoader').show();
                return true;
            }
            else
                return false;
        }

        function Removerules() {
            $("#ctl00_ContentPlaceHolder1_txtSAPBillToCode").rules("remove");
            $("#ctl00_ContentPlaceHolder1_txtFirstName").rules("remove");
            $("#ctl00_ContentPlaceHolder1_DrpBillingCountry").rules("remove");
            $("#ctl00_ContentPlaceHolder1_DrpBillingState").rules("remove");
            $("#ctl00_ContentPlaceHolder1_DrpBillingCity").rules("remove");
            $("#ctl00_ContentPlaceHolder1_ddlBasedStation").rules("remove");
            $("#ctl00_ContentPlaceHolder1_txtZip").rules("remove");
            $("#ctl00_ContentPlaceHolder1_txtAddressLine1").rules("remove");
            $("#ctl00_ContentPlaceHolder1_txtEmail").rules("remove");
        }

        function Addrules() {
            $("#ctl00_ContentPlaceHolder1_txtSAPBillToCode").rules("add",
                    { required: true, messages: { required: replaceMessageString(objValMsg, "Required", "SAP bill to code")} });
            $("#ctl00_ContentPlaceHolder1_txtFirstName").rules("add",
                    { required: true, messages: { required: replaceMessageString(objValMsg, "Required", "first name")} });
            $("#ctl00_ContentPlaceHolder1_DrpBillingCountry").rules("add",
                    { NotequalTo: "0", messages: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country")} });
            $("#ctl00_ContentPlaceHolder1_DrpBillingState").rules("add",
                    { NotequalTo: "0", messages: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state")} });
            $("#ctl00_ContentPlaceHolder1_DrpBillingCity").rules("add",
                    { NotequalTo: "0", messages: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city")} });
            $("#ctl00_ContentPlaceHolder1_ddlBasedStation").rules("add",
                    { NotequalTo: "0", messages: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "basestation")} });
            $("#ctl00_ContentPlaceHolder1_txtZip").rules("add",
                    { required: true, alphanumeric: true, validzipcode: GetCountryCode($("#ctl00_ContentPlaceHolder1_DrpBillingCountry :selected").text()), messages: { required: replaceMessageString(objValMsg, "Required", "zipcode")} });
            $("#ctl00_ContentPlaceHolder1_txtAddressLine1").rules("add",
                    { required: true, messages: { required: replaceMessageString(objValMsg, "Required", "address")} });            
            $("#ctl00_ContentPlaceHolder1_txtEmail").rules("add",
                    { email: true, messages: { email: replaceMessageString(objValMsg, "Valid", "email address")} });
        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $("#dvLoader").show();

            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                    },
                    messages: {
                    },
                    errorPlacement: function(error, element) {
                        error.insertAfter(element);
                    }
                });
            });

            $("#ctl00_ContentPlaceHolder1_lnkSave").click(function() {
                Addrules();
                if ($('#aspnetForm').valid()) {
                    return true;
                }
                else {
                    return false;
                }
            });

            $("#ctl00_ContentPlaceHolder1_DrpBillingCountry").change(function() {
                $('#dvLoader').show();
            });

            $("#ctl00_ContentPlaceHolder1_DrpBillingState").change(function() {
                $('#dvLoader').show();
            });

            $(window).scroll(function() {
                $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop());
            });

            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
        });
    </script>

    <style type="text/css">
        .basic_link .manage_link a, .header_bg
        {
            text-align: left;
        }
        .basic_link img
        {
            margin: 0 4px;
        }
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 15%;
        }
        .formtd_r
        {
            padding-right: 30px !important;
            width: 290px !important;
        }
    </style>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
    <div id="dvLoader">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div>
            <div>
                <h3 style="float: left; margin-right: 7px; color: #B0B0B0;">
                    Store :</h3>
                <asp:Label ID="lblStore" runat="server" Style="float: left; color: #72757C; line-height: 23px;
                    font-size: 15px;" />
            </div>
            <div>
                <asp:Label ID="lblWorkGroup" runat="server" Style="float: right; margin-left: 7px;
                    color: #72757C; line-height: 23px; font-size: 15px;" />
                <h3 style="float: right; color: #B0B0B0;">
                    Work Group :</h3>
            </div>
        </div>
        <div class="divider" style="clear: both;">
        </div>
        <div class="form_table">
            <h4>
                SAP Billing Address</h4>
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">SAP Bill To Code</span>
                                <asp:TextBox ID="txtSAPBillToCode" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="dvSAPBillToCode">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">First Name</span>
                                <asp:TextBox ID="txtFirstName" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="dvFirstName">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd_r">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Last Name</span>
                                <asp:TextBox ID="txtLastName" CssClass="w_label" runat="server"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Company</span>
                                <asp:TextBox ID="txtCompany" CssClass="w_label" runat="server"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Address Line 1</span>
                                <asp:TextBox ID="txtAddressLine1" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="dvAddress">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd_r">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Address Line 2</span>
                                <asp:TextBox ID="txtAddressLine2" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="Div1">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="DrpBillingCountry" runat="server" AutoPostBack="true" CssClass="w_label"
                                        OnSelectedIndexChanged="DrpBillingCountry_SelectedIndexChanged" TabIndex="5">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCountry">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">BaseStation</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlBasedStation" onchange="pageLoad(this,value);" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlBasedStation_SelectedIndexChanged"
                                        CssClass="w_label" TabIndex="1">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvbasedstation">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd_r">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="DrpBillingState" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DrpBillingState_SelectedIndexChanged"
                                        TabIndex="6">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvState">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="DrpBillingCity" AutoPostBack="true" runat="server" TabIndex="7">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCity">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Zip</span>
                                <asp:TextBox ID="txtZip" CssClass="w_label" runat="server"></asp:TextBox>
                                <div id="dvZip">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd_r">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Telephone</span>
                                <asp:TextBox ID="txtTelephone" CssClass="w_label" runat="server"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Email</span>
                                <asp:TextBox ID="txtEmail" CssClass="w_label" runat="server"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td class="formtd_r">
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Mobile</span>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer25">
        </div>
        <div class="centeralign">
            <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" OnClick="lnkSave_Click"><span>Save Information</span></asp:LinkButton>
        </div>
        <div class="spacer25">
        </div>
        <div>
            <table class="form_table">
                <tr>
                    <td colspan="3">
                        <div class="collapsibleContainer" title="SAP Billing Addresses">
                            <asp:GridView ID="gvbillingaddress" runat="server" HeaderStyle-CssClass="ord_header"
                                CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                                OnRowCommand="gvbillingaddress_RowCommand">
                                <Columns>
                                    <asp:TemplateField SortExpression="SAPBillToCode">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkSAPBillToCode" runat="server" CommandArgument="SAPBillToCode"
                                                CommandName="Sort"><span>SAP Bill To Code</span></asp:LinkButton>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="first">
                                                <asp:HyperLink ID="hypSAPAddress" CommandName="EditRec" CommandArgument='<%# Eval("BillingAddressID") %>'
                                                    runat="server" ToolTip='<%# Convert.ToString(Eval("SAPBillToCode")) %>'><%# "&nbsp;" + (Convert.ToString(Eval("SAPBillToCode")) != null ? (Convert.ToString(Eval("SAPBillToCode")).Length > 22 ? Convert.ToString(Eval("SAPBillToCode")).Substring(0, 22) + "..." : Eval("SAPBillToCode")) : "") %></asp:HyperLink>
                                            </span>
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greytop_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="19%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SAPBillToCode">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkFirstName" runat="server" CommandArgument="FirstName" CommandName="Sort"><span>First Name</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSAPBillToCode" ToolTip='<%# Convert.ToString(Eval("FirstName")) %>'
                                                Text='<%# "&nbsp;" + (Convert.ToString(Eval("FirstName")) != null ? (Convert.ToString(Eval("FirstName")).Length > 15 ? Convert.ToString(Eval("FirstName")).Substring(0, 15) + "..." : Eval("FirstName")) : "") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="LastName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkTitle" runat="server" CommandArgument="LastName" CommandName="Sort"><span>Last Name</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblLastName" ToolTip='<%# Convert.ToString(Eval("LastName")) %>'
                                                Text='<%# "&nbsp;" + (Convert.ToString(Eval("LastName")) != null ? (Convert.ToString(Eval("LastName")).Length > 15 ? Convert.ToString(Eval("LastName")).Substring(0, 15) + "..." : Eval("LastName")) : "") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Country">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkCountry" runat="server" CommandArgument="Country" CommandName="Sort"><span>Country</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCountry" ToolTip='<%# Convert.ToString(Eval("Country")) %>'
                                                Text='<%# "&nbsp;" + (Convert.ToString(Eval("Country")) != null ? (Convert.ToString(Eval("Country")).Length > 22 ? Convert.ToString(Eval("Country")).Substring(0, 22) + "..." : Eval("Country")) : "") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="18%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CompanyName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkCompanyName" runat="server" CommandArgument="CompanyName"
                                                CommandName="Sort"><span>Company Name</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCompanyName" ToolTip='<%# Convert.ToString(Eval("CompanyName")) %>'
                                                Text='<%# "&nbsp;" + (Convert.ToString(Eval("CompanyName")) != null ? (Convert.ToString(Eval("CompanyName")).Length > 22 ? Convert.ToString(Eval("CompanyName")).Substring(0, 22) + "..." : Eval("CompanyName")) : "") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="18%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="BaseStation">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkBaseStation" runat="server" CommandArgument="BaseStation"
                                                CommandName="Sort"><span>Base Station</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblBaseStation" ToolTip='<%# Convert.ToString(Eval("BaseStation")) %>'
                                                Text='<%# "&nbsp;" + (Convert.ToString(Eval("BaseStation")) != null ? (Convert.ToString(Eval("BaseStation")).Length > 20 ? Convert.ToString(Eval("BaseStation")).Substring(0, 20) + "..." : Eval("BaseStation")) : "") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="18%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Delete</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:ImageButton ID="lnkbtndelete" CommandName="DeleteRec" OnClientClick="return DeleteConfirmation();"
                                                    CommandArgument='<%# Eval("BillingAddressID") %>' runat="server" ImageUrl="~/Images/close.png" /></span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="1%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
