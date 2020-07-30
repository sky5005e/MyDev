<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="General.aspx.cs" Inherits="admin_CompanyStore_Product_General" Title="Store>> Product general" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 50px;
        }
        .form_table td
        {
            padding-bottom: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {

                        ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlInventoryStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlSupplierName: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlPrdCategory: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlSubCategory: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtPrdDescription: { required: true },
                        ctl00$ContentPlaceHolder1$txtSummary: { required: true },
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCreditEligible: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlGender: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlTailoringGuidelineStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlTailoringMeasurementChart: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlInventoryNotificationSystem: { NotequalTo: "0" }

                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlPrdCategory: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "category")
                        },
                        ctl00$ContentPlaceHolder1$ddlInventoryStatus: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "inventory status")
                        },
                        ctl00$ContentPlaceHolder1$ddlSupplierName: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Supplier name")
                        },

                        ctl00$ContentPlaceHolder1$ddlSubCategory: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "subcategory")
                        },
                        ctl00$ContentPlaceHolder1$txtPrdDescription: {
                            required: replaceMessageString(objValMsg, "Required", "product description")

                        },
                        ctl00$ContentPlaceHolder1$txtSummary: {
                            required: "<br/>" + replaceMessageString(objValMsg, "Required", "summary")

                        },

                        ctl00$ContentPlaceHolder1$ddlDepartment: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department")
                        },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup")
                        },
                        ctl00$ContentPlaceHolder1$ddlGender: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "garment Type")
                        },
                        ctl00$ContentPlaceHolder1$ddlStatus: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "product status")
                        },
                        ctl00$ContentPlaceHolder1$ddlTailoringMeasurementChart: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Tailoring measurement chart")
                        },
                        ctl00$ContentPlaceHolder1$ddlCreditEligible: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "anniversary Credit Eligible")
                        },
                        ctl00$ContentPlaceHolder1$ddlInventoryNotificationSystem: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Inventory Notification")
                        },
                        ctl00$ContentPlaceHolder1$ddlTailoringGuidelineStatus: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Tailoring Guideline Status")
                        }

                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtPrdDescription")
                            error.insertAfter("#divtxtPrdDescription");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtSummary")
                            error.insertAfter("#divtxtSummary");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlSupplierName")
                            error.insertAfter("#dvSupplierName");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlStatus")
                            error.insertAfter("#dvStatus");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlInventoryStatus")
                            error.insertAfter("#dvInventoryStatus");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlPrdCategory")
                            error.insertAfter("#dvPrdCategory");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlSubCategory")
                            error.insertAfter("#dvSubCategory");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlDepartment")
                            error.insertAfter("#dvDepartment");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCreditEligible")
                            error.insertAfter("#dvCreditEligible");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlGender")
                            error.insertAfter("#dvGender");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlTailoringGuidelineStatus")
                            error.insertAfter("#dvTailoringGuidelineStatus");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlTailoringMeasurementChart")
                            error.insertAfter("#dvTailoringMeasurementChart");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlInventoryNotificationSystem")
                            error.insertAfter("#dvInventoryNotificationSystem");
                        else
                            error.insertAfter(element);
                    }


                });

                $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {

                    return $('#aspnetForm').valid();
                });
            });
        });

        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>

    <div class="form_pad">
        <div>
            <table class="form_table">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Company</span>
                                <asp:Label ID="lblEmployeeName" runat="server" TabIndex="1" CssClass="w_label"></asp:Label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Supplier Name</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlSupplierName" TabIndex="2" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvSupplierName">
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
                            <div class="form_box shipmax_in">
                                <span class="input_label">Status</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="3" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvStatus">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Inventory Status</span><span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlInventoryStatus" TabIndex="4" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvInventoryStatus">
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
                                <span class="input_label">Product Category</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlPrdCategory" AutoPostBack="True" runat="server" TabIndex="5"
                                        onchange="pageLoad(this,value);" OnSelectedIndexChanged="ddlPrdCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvPrdCategory">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Workgroup</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlWorkgroup" runat="server" TabIndex="6" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvWorkgroup">
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
                                <span class="input_label">Sub-Category</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlSubCategory" runat="server" TabIndex="7" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvSubCategory">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Department</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlDepartment" runat="server" TabIndex="8" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvDepartment">
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
                            <div class="form_box shipmax_in">
                                <span class="input_label">Gender</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlGender" runat="server" TabIndex="9" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvGender">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Credit Eligible</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlCreditEligible" runat="server" TabIndex="10" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCreditEligible">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label cal_widper">New Product Until</span>
                                <asp:TextBox ID="txtNewProdUntil" runat="server" TabIndex="11" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Show Inventory Levels in Store</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlInvtLevelStore" runat="server" TabIndex="12" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvInvtLevelStore">
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
                            <div class="form_box shipmax_in">
                                <span class="input_label">Inventory Notification System</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlInventoryNotificationSystem" runat="server" TabIndex="13"
                                        onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvInventoryNotificationSystem">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Allow for Back Orders</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlAllowbackOrders" runat="server" TabIndex="14" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvAllowbackOrders">
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
                            <div class="form_box shipmax_in">
                                <span class="input_label">Tailoring Guideline Status</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlTailoringGuidelineStatus" runat="server" TabIndex="15" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvTailoringGuidelineStatus">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label cal_widper">New Inventory to Arrive On</span>
                                <asp:TextBox ID="txtDate" runat="server" TabIndex="16" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
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
                            <div class="form_box shipmax_in">
                                <span class="input_label">Tailoring Measurement Chart</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlTailoringMeasurementChart" runat="server" TabIndex="16"
                                        onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvTailoringMeasurementChart">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label">Run Charge ($)</span>
                                <asp:TextBox ID="txtRailoringRunCharge" MaxLength="100" TabIndex="17" runat="server"
                                    CssClass="w_label"></asp:TextBox>
                                <at:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                    FilterType="Custom, Numbers" ValidChars="." FilterInterval="0" TargetControlID="txtRailoringRunCharge">
                                </at:FilteredTextBoxExtender>
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
                                <span class="input_label">Tailoring Services Lead-Time</span>
                                <asp:TextBox ID="txtTailoringServicesLeadTime" MaxLength="100" TabIndex="18" runat="server"
                                    CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Credit Message</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlCreditMessage" runat="server" TabIndex="19" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCreditMessage">
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
                            <div class="form_box shipmax_in">
                                <span class="input_label">Employee Type</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlEmployeeType" runat="server" TabIndex="20" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvEmployeeType">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Color</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlColor" runat="server" TabIndex="21" onchange="pageLoad(this,value);">
                                        <asp:ListItem Value="0" Text="-Select-" />
                                        <asp:ListItem Value="True" Text="Active" />
                                        <asp:ListItem Value="False" Text="Inactive" />
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
                            <div class="form_box shipmax_in">
                                <span class="input_label">Size</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlSize" runat="server" TabIndex="22" onchange="pageLoad(this,value);">
                                        <asp:ListItem Value="0" Text="-Select-" />
                                        <asp:ListItem Value="True" Text="Active" />
                                        <asp:ListItem Value="False" Text="Inactive" />
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Material</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlMaterial" runat="server" TabIndex="23" onchange="pageLoad(this,value);">
                                        <asp:ListItem Value="0" Text="-Select-" />
                                        <asp:ListItem Value="True" Text="Active" />
                                        <asp:ListItem Value="False" Text="Inactive" />
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
                            <div class="form_box shipmax_in">
                                <span class="input_label">Close Out</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlCloseOut" runat="server" TabIndex="24" onchange="pageLoad(this,value);">
                                        <asp:ListItem Value="0" Text="-Select-" />
                                        <asp:ListItem Value="True" Text="Active" />
                                        <asp:ListItem Value="False" Text="Inactive" />
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Report Tag</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlReportTag" runat="server" TabIndex="25" onchange="pageLoad(this,value);">
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
                            <div class="form_box shipmax_in">
                                <span class="input_label">Item Type</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlItemType" runat="server" TabIndex="26" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">GL-Code</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlProductGLCode" runat="server" TabIndex="25" onchange="pageLoad(this,value);">
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
                            <div class="form_box shipmax_in">
                                <span class="input_label">Show Product Video</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlShowProductVideo" runat="server" TabIndex="30" onchange="pageLoad(this,value);">
                                        <asp:ListItem Value="0" Text="-Select-" />
                                        <asp:ListItem Value="True" Text="Active" />
                                        <asp:ListItem Value="False" Text="Inactive" />
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Show Certification</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlCertification" runat="server" TabIndex="31" onchange="pageLoad(this,value);">
                                        <asp:ListItem Value="0" Text="-Select-" />
                                        <asp:ListItem Value="True" Text="Active" />
                                        <asp:ListItem Value="False" Text="Inactive" />
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
                            <div class="form_box shipmax_in">
                                <span class="input_label">Product Video Url</span>
                                   <asp:TextBox ID="txtProductVideoUrl" MaxLength="100" TabIndex="32" runat="server"
                                    CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Certification file</span>
                                <asp:FileUpload ID="fpCertFile" runat="server" />
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
                            <div class="form_box shipmax_in">
                                <span class="input_label">Climate Setting</span> <span class="custom-sel label-sel-small">
                                     <asp:DropDownList ID="ddlClimateSetting" runat="server" onchange="pageLoad(this,value);" TabIndex="33">
                                      </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                     <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label">Show Sold by</span> <span class="custom-sel label-sel-small">
                                     <asp:DropDownList ID="ddlShowSoldby" runat="server" onchange="pageLoad(this,value);" TabIndex="34">
                                       <asp:ListItem Value="0" Text="-Select-" />
                                        <asp:ListItem Value="True" Text="Active" />
                                        <asp:ListItem Value="False" Text="Inactive" />
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
                            <div class="form_box taxt_area clearfix">
                                <span class="input_label alignleft" style="width: 39%!important;">Product Description</span>
                                <div class="textarea_box alignright" style="width: 57%;">
                                    <div class="scrollbar">
                                        <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                            class="scrollbottom"></a>
                                    </div>
                                    <asp:TextBox ID="txtPrdDescription" runat="server" TabIndex="27" TextMode="MultiLine"
                                        CssClass="scrollme1" Height="70px"></asp:TextBox>
                                </div>
                                <div id="divtxtPrdDescription">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box taxt_area clearfix">
                                <span class="input_label alignleft" style="width: 39%!important;">Summary</span>
                                <div class="textarea_box alignright" style="width: 57%;">
                                    <div class="scrollbar">
                                        <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                        </a>
                                    </div>
                                    <asp:TextBox ID="txtSummary" runat="server" TabIndex="28" TextMode="MultiLine" CssClass="scrollme1"
                                        Height="70px"></asp:TextBox>
                                </div>
                                <div id="divtxtSummary">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                
                <tr>
                    <td>
                        <div class="botbtn alignright">
                            <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" TabIndex="29"
                                ToolTip="Save Information" OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    
     <asp:HiddenField ID="hdnCertPath" value="0" runat="server" />
</asp:Content>
