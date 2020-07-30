<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AddEquipment.aspx.cs" Inherits="AssetManagement_AddEquipment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
         $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <style type="text/css">
        .textarea_box textarea
        {
            height: 178px;
        }
        .textarea_box
        {
            height: 178px;
        }
        .textarea_box .scrollbar
        {
            height: 185px;
        }
    </style>

    <script type="text/javascript" language="javascript">
         $().ready(function() {
             $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                 objValMsg = $.xml2json(xml); $("#aspnetForm").validate(
                {
                    rules:
                    {
                    ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: "0" },
                     ctl00$ContentPlaceHolder1$ddlBaseStation: { NotequalTo: "0" },
                    ctl00$ContentPlaceHolder1$ddlEquipmentType: { NotequalTo: "0" },
                         ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtEquipmentId: { required: true }
                        
                    },
                    messages:
                    {
                    ctl00$ContentPlaceHolder1$ddlStatus: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Status")
                        },
                     ctl00$ContentPlaceHolder1$ddlBaseStation: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Base Station")
                        },
                     ctl00$ContentPlaceHolder1$ddlEquipmentType: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Equipment Type")
                        },
                            ctl00$ContentPlaceHolder1$ddlCompany: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Company")
                        },
                        ctl00$ContentPlaceHolder1$txtEquipmentId: { required: replaceMessageString(objValMsg, "Required", "Equipment ID") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlEquipmentId")
                            error.insertAfter("#dvEquipmentId");                        
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlEquipmentType")
                            error.insertAfter("#dvEquipmentType");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCompany")
                            error.insertAfter("#dvCompany");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlBaseStation")
                            error.insertAfter("#dvBaseStation");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlStatus")
                            error.insertAfter("#dvStatus");
                        else
                            error.insertAfter(element);
                    }
                });
             });
             $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                 return $('#aspnetForm').valid();
             });
         });
    </script>

    <div style="text-align: center">
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
    </div>
    <div class="form_table">
        <div class="black_top_co">
            <span>&nbsp;</span></div>
        <div class="black_middle order_detail_pad">
            <div>
                <div class="alignleft" style="width: 49%;">
                    <div class="tab_content_top_co">
                        <span>&nbsp;</span></div>
                    <div class="tab_content">
                        <table class="order_detail" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <span style="font-size: 16px">Basic Equipment Info:</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Company</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlCompany" runat="server" onchange="pageLoad(this,value);" TabIndex="1">
                                            </asp:DropDownList>
                                        </span>
                                        <div id="dvCompany">
                                        </div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Equipment Type</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlEquipmentType" runat="server" onchange="pageLoad(this,value);" TabIndex="2">
                                            </asp:DropDownList>
                                        </span>
                                        <div id="dvEquipmentType">
                                        </div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Equipment Model</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlEquipmentModel" runat="server" onchange="pageLoad(this,value);" TabIndex="3">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">GSE Department</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlGSEDepartment" runat="server" onchange="pageLoad(this,value);" TabIndex="3">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                           
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Brand</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlBrand" runat="server" onchange="pageLoad(this,value);" TabIndex="4">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Purchase Price</span>
                                        <asp:TextBox ID="TextBox1" runat="server" MaxLength="7" CssClass="w_label" TabIndex="5"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Owned By</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlOwnedBy" runat="server" onchange="pageLoad(this,value);" TabIndex="6">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="tab_content_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="alignright" style="width: 49%;">
                    <div class="tab_content_top_co">
                        <span>&nbsp;</span></div>
                    <div class="tab_content">
                        <table class="order_detail" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <span style="font-size: 16px">Equipment ID Info:</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Equipment ID</span>
                                        <asp:TextBox ID="txtEquipmentId" runat="server" MaxLength="7" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div id="dvEquipmentID">
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Serial Number</span>
                                        <asp:TextBox ID="txtSerialNumber" runat="server" MaxLength="7" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Vin Number</span>
                                        <asp:TextBox ID="txtVinNumber" runat="server" MaxLength="7" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Plate Number</span>
                                        <asp:TextBox ID="txtPlateNumber" runat="server" MaxLength="7" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="tab_content_bot_co">
                        <span>&nbsp;</span></div>
                </div>
            </div>
            <div class="alignnone">
            </div>
        </div>
        <div class="black_bot_co">
        </div>
        <div class="spacer10">
        </div>
        <div class="black_top_co">
            <span>&nbsp;</span></div>
        <div class="black_middle order_detail_pad">
            <div>
                <div class="alignleft" style="width: 49%;">
                    <div class="tab_content_top_co">
                        <span>&nbsp;</span></div>
                    <div class="tab_content">
                        <table class="order_detail" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <span style="font-size: 16px">Purchase Info:</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="calender_l">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box shipmax_in">
                                            <span class="input_label cal_widper">Purchase Date</span>
                                            <asp:TextBox ID="txtPurchaseDate" runat="server" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Purchase From</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlPurchasedFrom" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">New or Refurbished</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlNewRefurbished" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Purchase Method</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlPurchaseMethod" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Purchase Price</span>
                                        <asp:TextBox ID="txtPurchasePrice" runat="server" MaxLength="7" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Charge To</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlChargeTo" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Equipment Life</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlEquipmentLife" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Warranty Parts</span>
                                        <asp:TextBox ID="txtWarrantyParts" runat="server" MaxLength="7" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Warranty Labor</span>
                                        <asp:TextBox ID="txtWarrantyLabor" runat="server" MaxLength="7" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="tab_content_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="alignright" style="width: 49%;">
                    <div class="tab_content_top_co">
                        <span>&nbsp;</span></div>
                    <div class="tab_content">
                        <table class="order_detail" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <span style="font-size: 16px">Additional Info:</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Base Station</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlBaseStation" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                        <div id="dvBaseStation">
                                        </div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Power Source</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlPowerSource" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Fuel Type</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlFuelType" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Max Weight</span>
                                        <asp:TextBox ID="txtMaxWeight" runat="server" MaxLength="7" CssClass="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Status</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlStatus" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                        <div id="dvStatus">
                                        </div>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                            <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">Model Year</span>
                            <asp:TextBox ID="txtModelYear" runat="server" MaxLength="4" CssClass="w_label"></asp:TextBox>
                            <at:FilteredTextBoxExtender ID="flttxtModelYear" runat="server" TargetControlID="txtModelYear" FilterType="Numbers"></at:FilteredTextBoxExtender>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                        </table>
                    </div>
                    <div class="tab_content_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="alignright" style="width: 49%;">
                    <div class="spacer10">
                    </div>
                    <div class="tab_content_top_co">
                        <span>&nbsp;</span></div>
                    <div class="tab_content">
                        <table class="order_detail" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <span style="font-size: 16px">Equipment Uses Info:</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Aircraft Type</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlAircraftType" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 40%">Max Aircraft Weight</span> <span class="custom-sel label-sel-small"
                                            style="width: 50%;">
                                            <asp:DropDownList ID="ddlMaxAircraftWeight" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="tab_content_bot_co">
                        <span>&nbsp;</span></div>
                </div>
            </div>
            <div class="alignnone">
            </div>
        </div>
        <div class="black_bot_co">
        </div>
    </div>
    <div class="aligncenter">
        <div class="spacer10">
        </div>
        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save Basic Information" OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
