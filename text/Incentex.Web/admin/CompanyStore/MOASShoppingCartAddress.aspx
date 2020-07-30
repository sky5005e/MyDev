<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="MOASShoppingCartAddress.aspx.cs" Inherits="admin_CompanyStore_MOASShoppingCartAddress"
    Title="MOAS Shopping Cart Address" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();

            }
        }
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected image?") == true)
                return true;
            else
                return false;
        }



        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {

                        // ctl00$ContentPlaceHolder1$hdnDepartment: { NotequalTo: "0" },
                        //ctl00$ContentPlaceHolder1$txtMOASAddressPriority: { required: true },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" }
                    },
                    messages: {
                        //ctl00$ContentPlaceHolder1$hdnDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") },
                        // ctl00$ContentPlaceHolder1$txtMOASAddressPriority: { required: replaceMessageString(objValMsg, "Required", "MOAS Address Name") },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        //                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$hdnDepartment")
                        //                           error.insertAfter("#dvDepartment");
                        else
                            error.insertAfter(element);
                    }


                });
               

                /*End Change Event of all the dropdowns*/

                $("#<%=lnkAddItem.ClientID %>").click(function() {
                    return $('#aspnetForm').valid();

                });


            });
        });
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblFor" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <table class="form_table">
            <tr>
                <td style="width:49%;">
                 <div>
                             <asp:UpdatePanel ID="upDepartment" runat="server">
                                    <ContentTemplate>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label">Department :</span>
                                    <label class="dropimg_width230">
                                        <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlDepartment" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                        <div id="dvDepartment">
                                        </div>
                                    </label>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                            </div>
                </td>
                <td style="width:1%;">
                </td>
                <td style="width:50%;">
                 <div>
                             <asp:UpdatePanel ID="upWorkgroup" runat="server">
                                    <ContentTemplate>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label">Workgroup :</span>
                                    <label class="dropimg_width230">
                                        <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlWorkgroup" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                        <div id="dvWorkgroup">
                                        </div>
                                    </label>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                                    </ContentTemplate></asp:UpdatePanel>
                            </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label">Store Name:</span>
                            <asp:TextBox ID="lblCompanyName" runat="server" ReadOnly="true" CssClass="w_label"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
                <td width="5px">
                </td>
                <td runat="server" visible="false">
                    <div>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label">MOAS Address Name:</span>
                            <asp:TextBox ID="txtMOASAddressPriority" runat="server" MaxLength="50" CssClass="w_label"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
            </tr>
        </table>
        <div class="header_bg" style="text-align: left;">
            <div class="header_bgr title_small">
                Billing Information</div>
        </div>
        <div class="spacer10">
        </div>
        <div>
            <table class="form_table">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Company</span>
                                <asp:TextBox ID="TxtBillingCompanyName" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td width="5px">
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">First Name</span>
                                <asp:TextBox ID="TxtFirstName" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Last Name</span>
                                <asp:TextBox ID="TxtLastName" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Address1</span>
                                <asp:TextBox ID="TxtBillingAddress1" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Address2</span>
                                <asp:TextBox ID="TxtBillingAddress2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upBillingCountry" runat="server">
                            <ContentTemplate>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="DrpBillingCountry" runat="server" AutoPostBack="true" CssClass="w_label"
                                                OnSelectedIndexChanged="DrpBillingCountry_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="upBillingState" runat="server">
                            <ContentTemplate>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="DrpBillingState" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DrpBillingState_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upCity" runat="server">
                            <ContentTemplate>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="DrpBillingCity" runat="server">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Zip</span>
                                <asp:TextBox ID="TxtBillingZip" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Telephone</span>
                                        <asp:TextBox ID="TxtPhone" CssClass="w_label" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Mobile</span>
                                        <asp:TextBox ID="TxtMobile" CssClass="w_label" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Email</span>
                                        <asp:TextBox ID="TxtEmail" CssClass="w_label" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer10">
        </div>
        <!-- button -->
        <div class="botbtn centeralign">
         <asp:UpdatePanel runat="server" ID="pnladdress">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkAddItem" />
                           
                        </Triggers>
                        <ContentTemplate>
            <asp:LinkButton ID="lnkAddItem" runat="server" CssClass="grey2_btn" OnClick="lnkAddItem_Click"><span>Add Billing Address</span></asp:LinkButton>
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="spacer25">
        </div>
        <table>
            <tr>
                <td colspan="3">
                  <%--  <asp:UpdatePanel runat="server" ID="pnladdress">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkAddItem" />
                           
                        </Triggers>
                        <ContentTemplate>--%>
                            <asp:GridView ID="gvShippingBilling" runat="server" HeaderStyle-CssClass="ord_header"
                                CssClass="orderreturn_box" GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                                OnRowCommand="gvShippingBilling_RowCommand" OnRowDataBound="gvShippingBilling_RowDataBound">
                                <Columns>
                                    <asp:TemplateField Visible="False" HeaderText="Id">
                                        <HeaderTemplate>
                                            <span>MOASShoppingCartAddID</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblMOASShoppingCartAddID" Text='<%# Eval("MOASShoppingCartAddID") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Company</span>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="first">
                                                <asp:LinkButton ID="hypCompany" CommandName="EditRec" CommandArgument='<%# Eval("MOASShoppingCartAddID") %>'
                                                    runat="server" ToolTip="Click here to Edit Shipping/Billing Address"></asp:LinkButton>
                                            </span>
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Workgroup Name</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblWorkgroupName" />
                                            <asp:HiddenField ID="hdnWorkgroupiD" runat="server" Value='<%# Eval("WorkgroupId") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Email</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTitle" Text='<% # (Convert.ToString(Eval("BEmail")).Length > 35) ? Eval("BEmail").ToString().Substring(0,35)+"..." : Convert.ToString(Eval("BEmail"))+ "&nbsp;"  %>'
                                                ToolTip='<%# Eval("BEmail") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Delete</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:ImageButton ID="lnkbtndelete" CommandName="DeleteRec" OnClientClick="return DeleteConfirmation();"
                                                    CommandArgument='<%# Eval("MOASShoppingCartAddID") %>' runat="server" ImageUrl="~/Images/close.png" /></span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="10%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                      <%--  </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
