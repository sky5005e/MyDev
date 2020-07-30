<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="BasicVendorEmpInformation.aspx.cs" Inherits="AssetManagement_BasicVendorEmpInformation"
    Title="Vendor Employee Information" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 36px;
        }
        .checklist input
        {
            float: left;
        }
        .checklist label
        {
            line-height: 10px;
            width: 100%;
            text-align: left;
            display: block;
        }
        div.rolediv .label-sel-small
        {
            width: 71%;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $(function() {
            scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");

        });

        $().ready(function() {
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtFirstName: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtLastName: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtAddress: { required: true },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: true, alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtLogInEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$txtPassword: { required: true },
                        ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: true },
                        ctl00$ContentPlaceHolder1$txtMobile: { alphanumeric: true },
                        ctl00$ContentPlaceHolder1$ddlUserRole: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "WLS Status") },
                        ctl00$ContentPlaceHolder1$ddlUserRole: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Role") },
                        ctl00$ContentPlaceHolder1$txtFirstName: {
                            required: replaceMessageString(objValMsg, "Required", "first name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        },
                        ctl00$ContentPlaceHolder1$txtLastName: {
                            required: replaceMessageString(objValMsg, "Required", "last name"),
                            alphanumeric: replaceMessageString(objValMsg, "Alphanumeric", "")
                        },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") },
                        ctl00$ContentPlaceHolder1$ddlState: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") },
                        ctl00$ContentPlaceHolder1$ddlCity: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") },
                        ctl00$ContentPlaceHolder1$txtAddress: { required: replaceMessageString(objValMsg, "Required", "address") },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: replaceMessageString(objValMsg, "Required", "telephone"), alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtLogInEmail: { required: replaceMessageString(objValMsg, "Required", "login email"), email: replaceMessageString(objValMsg, "Email", "") },
                        ctl00$ContentPlaceHolder1$txtPassword: { required: replaceMessageString(objValMsg, "Required", "password") },
                        ctl00$ContentPlaceHolder1$txtZip: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtFax: { alphanumeric: replaceMessageString(objValMsg, "Number", "") },
                        ctl00$ContentPlaceHolder1$txtMobile: { alphanumeric: replaceMessageString(objValMsg, "Number", "") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtAddress")
                            error.insertAfter("#divtxtAddress");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlStatus")
                            error.insertAfter("#divStatus");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlUserRole")
                            error.insertAfter("#divRole");
                        else
                            error.insertAfter(element);
                    }
                });
            });


            $("#<%=lnkSave.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });
        });
     
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div id="dvContact" runat="server">
            <h4>
                Contact Information</h4>
            <div>
                <asp:UpdatePanel ID="up1" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="ddlCity" />
                    </Triggers>
                    <ContentTemplate>
                        <table class="form_table">
                            <tr>
                                <td class="formtd">
                                    <table>
                                        <tr>
                                            <td>
                                                <div>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Company Name</span>
                                                        <asp:TextBox ID="txtCompany" ReadOnly="true" runat="server" CssClass="w_label" TabIndex="1"></asp:TextBox>
                                                        </span>
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
                                                    <div class="form_box employeeedit_text clearfix">
                                                        <span class="input_label alignleft">Address</span>
                                                        <div class="textarea_box alignright">
                                                            <div class="scrollbar">
                                                                <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                                    class="scrollbottom"></a>
                                                            </div>
                                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="scrollme" TextMode="MultiLine"
                                                                TabIndex="6"></asp:TextBox>
                                                        </div>
                                                        <div id="divtxtAddress">
                                                        </div>
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
                                                        <span class="input_label">City</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlCity" runat="server" TabIndex="9" onchange="pageLoad(this,value);"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                                                                <asp:ListItem Text="-select city-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="PnlCityOther" runat="server" visible="false">
                                            <td>
                                                <div>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">City Name</span>
                                                        <asp:TextBox ID="txtCity" runat="server" TabIndex="10" CssClass="w_label"></asp:TextBox>
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
                                                        <span class="input_label">Fax</span>
                                                        <asp:TextBox ID="txtFax" runat="server" CssClass="w_label" TabIndex="14"></asp:TextBox>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="formtd">
                                    <table>
                                        <tr>
                                            <td>
                                                <div>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">First Name</span>
                                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="w_label" TabIndex="2"></asp:TextBox>
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
                                                        <span class="input_label">Country</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" TabIndex="7"
                                                                OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                                <asp:ListItem Text="-select country-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
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
                                                        <span class="input_label">Zip Code</span>
                                                        <asp:TextBox ID="txtZip" runat="server" CssClass="w_label" TabIndex="11"></asp:TextBox>
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
                                                        <span class="input_label">Mobile</span>
                                                        <asp:TextBox ID="txtMobile" runat="server" CssClass="w_label" TabIndex="15"></asp:TextBox>
                                                        <at:FilteredTextBoxExtender ID="flttxtMobile" runat="server" TargetControlID="txtMobile"
                                                            FilterType="Numbers">
                                                        </at:FilteredTextBoxExtender>
                                                    </div>
                                                    <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                                        <img src="../Images/lightbulb.gif" alt="Note:" />&nbsp;Preceded by Country Code
                                                        like 13055195004</div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="formtd_r">
                                    <table>
                                        <tr>
                                            <td>
                                                <div>
                                                    <div class="form_top_co">
                                                        <span>&nbsp;</span></div>
                                                    <div class="form_box">
                                                        <span class="input_label">Last Name</span>
                                                        <asp:TextBox ID="txtLastName" runat="server" CssClass="w_label" TabIndex="3"></asp:TextBox>
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
                                                        <span class="input_label">State/Province</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" TabIndex="8" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                                <asp:ListItem Text="-select state-" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
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
                                                        <span class="input_label">Telephone</span>
                                                        <asp:TextBox ID="txtTelephone" runat="server" CssClass="w_label" TabIndex="12"></asp:TextBox>
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
                                                    <div class="form_box rolediv">
                                                        <span class="input_label" style="width: 17%;">Role</span> <span class="custom-sel label-sel-small">
                                                            <asp:DropDownList ID="ddlUserRole" runat="server" TabIndex="9" onchange="pageLoad(this,value);">
                                                                <asp:ListItem Selected="True" Text="Equipment Vendor Employee" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="FAA Users" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </span>
                                                        <div id="divRole">
                                                        </div>
                                                    </div>
                                                    <div class="form_bot_co">
                                                        <span>&nbsp;</span></div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="divider">
            </div>
            <h4>
                Base Station</h4>
            <div class="form_table">
                <table class="checktable_supplier true">
                    <tr>
                        <td style="width: 36%">
                            <table>
                                <tr>
                                    <td class="formtd">
                                        <table class="checktable_supplier true">
                                            <tr>
                                                <td>
                                                    <div style="height: 150px; overflow: auto">
                                                        <asp:DataList ID="dtBaseStation" runat="server">
                                                            <ItemTemplate>
                                                                <span class="custom-checkbox alignleft" id="BaseStationspan" runat="server">
                                                                    <asp:CheckBox ID="chkBaseStation" runat="server" />
                                                                </span>
                                                                <label>
                                                                    <asp:Label ID="lblBaseStation" Text='<%# Eval("sBaseStation") %>' runat="server"></asp:Label></label>
                                                                <asp:HiddenField ID="hdnBaseStation" runat="server" Value='<%#Eval("iBaseStationId")%>' />
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                        <asp:Label ID="lblNoBaseSation" runat="server" ForeColor="Red" Visible="false" Text="No data found"></asp:Label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 64%">
                        </td>
                    </tr>
                </table>
            </div>
            <div class="divider">
            </div>
        </div>
        <h4>
            System Login and Password</h4>
        <div>
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div class="shipmax_in">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Login Email</span>
                                            <asp:TextBox ID="txtLogInEmail" TabIndex="19" runat="server" CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Password</span>
                                            <asp:TextBox ID="txtPassword" TabIndex="20" runat="server" CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd_r ">
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label ">WLS Status</span>
                            <label class="dropimg_width150">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlStatus" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="divStatus">
                                </div>
                            </label>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="alignnone spacer25">
        </div>
        <div class="additional_btn">
            <ul class="clearfix">
                <li>
                    <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" TabIndex="22" OnClick="lnkSave_Click">

								        <span>Save Information</span>
                    </asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
