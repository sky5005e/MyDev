<%@ Page Title="Store Workgroup >> International Shipping" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="InternationalShipping.aspx.cs" Inherits="admin_CompanyStore_InternationalShipping" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();

            }
        }
    </script>

    <style type="text/css">
       .basic_link .manage_link a, .header_bg
        {
        	text-align:left;
        }
        .basic_link img
        {
        	margin:0 4px;
        }  
        .form_table .input_label
        {
            width: 28%;
        }
        .employeeedit_text .input_label
        {
            width: 20%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div class="employee_name">
            Work Group:
            <asp:Label ID="lblWorkGroup" runat="server"></asp:Label>
        </div>
        <h4>
            International Shipping Address for the Users<br />
            <span style="font-size: 12px;">Note: This rule applyes to all users out side of the
                United State.</span></h4>
        <table class="checktable_supplier true">
            <tr>
                <td>
                    <span class="custom-checkbox alignleft" id="spnShoppingCart" runat="server">
                        <asp:CheckBox ID="chkShoppingCart" runat="server" />
                    </span>
                    <label>
                        Shopping Cart</label>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="custom-checkbox alignleft" id="spnIssuancePolicy" runat="server">
                        <asp:CheckBox ID="chkIssuancePolicy" runat="server" />
                    </span>
                    <label>
                        Issuance Policy</label>
                </td>
            </tr>
        </table>
        <div style="text-align: center; margin-bottom: 5px;">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <table class="form_table">
            <tr>
                <td class="formtd">
                    <div>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label">First Name</span>
                            <asp:TextBox ID="TxtFirstName" runat="server" CssClass="w_label" TabIndex="1"></asp:TextBox>
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
                            <span class="input_label">Last Name</span>
                            <asp:TextBox ID="TxtLastName" runat="server" CssClass="w_label" TabIndex="2"></asp:TextBox>
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
                            <span class="input_label">Company</span>
                            <asp:TextBox ID="TxtCompany" runat="server" CssClass="w_label" TabIndex="3"></asp:TextBox>
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
                            <span class="input_label">Department</span> <span class="custom-sel label-sel">
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="w_label" TabIndex="4">
                                </asp:DropDownList>
                            </span>
                            <div id="dvDepartment">
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
                            <span class="input_label">Airport</span>
                            <asp:TextBox ID="txtAirport" runat="server" CssClass="w_label" TabIndex="5"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
                <td class="formtd_r">
                    <div>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box employeeedit_text clearfix">
                            <span class="input_label">Address</span>
                            <div class="textarea_box alignright">
                                <div class="scrollbar">
                                    <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                    </a>
                                </div>
                                <textarea id="TxtAddress" rows="3" runat="server" class="scrollme2" tabindex="6"></textarea>
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
                            <span class="input_label">Country</span> <span class="custom-sel label-sel">
                                <asp:DropDownList ID="ddlCoutry" runat="server" CssClass="w_label" AutoPostBack="true"
                                    TabIndex="7">
                                </asp:DropDownList>
                            </span>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
                <td class="formtd">
                    <asp:UpdatePanel ID="upShippingState" runat="server">
                        <ContentTemplate>
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label">State</span> <span class="custom-sel label-sel">
                                        <asp:DropDownList ID="ddlState" runat="server" OnSelectedIndexChanged="DrpShipingState_SelectedIndexChanged"
                                            AutoPostBack="true" TabIndex="8">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="formtd_r">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label">City</span> <span class="custom-sel label-sel">
                                        <asp:DropDownList ID="ddlCity" AutoPostBack="true" runat="server" TabIndex="9">
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
                <td class="formtd">
                    <div>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label">County</span>
                            <asp:TextBox ID="txtCounty" runat="server" CssClass="w_label" TabIndex="10"></asp:TextBox>
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
                            <asp:TextBox ID="TxtZip" runat="server" CssClass="w_label" TabIndex="11"></asp:TextBox>
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
                            <asp:TextBox ID="TxtTelephone" runat="server" CssClass="w_label" TabIndex="12"></asp:TextBox>
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
                            <span class="input_label">Mobile</span>
                            <asp:TextBox ID="TxtMobile" runat="server" CssClass="w_label" TabIndex="13"></asp:TextBox>
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
                            <span class="input_label">Email</span>
                            <asp:TextBox ID="TxtEmail" runat="server" CssClass="w_label" TabIndex="14"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="centeralign">
                    <asp:LinkButton ID="lnkBtnSaves" class="grey2_btn" OnClick="lnkBtnSave_Click" runat="server"
                        TabIndex="15"><span>Save</span></asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
