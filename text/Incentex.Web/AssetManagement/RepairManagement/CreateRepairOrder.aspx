<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CreateRepairOrder.aspx.cs" Inherits="AssetManagement_RepairManagement_CreateRepairOrder" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_table td
        {
            padding-bottom: 5px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                     ctl00$ContentPlaceHolder1$txtReturntoServiceDate: { required: true },
                        ctl00$ContentPlaceHolder1$ddlEquipmentType: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlEquipmentId: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlReasonforRepair: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtReturntoServiceDate: { 
                        required: replaceMessageString(objValMsg, "Required", "Return to ServiceDate") 
                        },
                        ctl00$ContentPlaceHolder1$ddlEquipmentType: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Equipment Type")
                        },
                        ctl00$ContentPlaceHolder1$ddlEquipmentId: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "EquipmentId")
                        },
                        ctl00$ContentPlaceHolder1$ddlCompany: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Company")
                        },
                        ctl00$ContentPlaceHolder1$ddlReasonforRepair: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Reason for Repair")
                        }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlEquipmentId")
                            error.insertAfter("#dvEquipmentId");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtReturntoServiceDate")
                            error.insertAfter("#dvReturntoServiceDate");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlEquipmentType")
                            error.insertAfter("#dvEquipmentType");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCompany")
                            error.insertAfter("#dvCompany");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlReasonforRepair")
                            error.insertAfter("#dvReasonforRepair");
                        else
                            error.insertAfter(element);
                    }


                });

                $("#<%=lnkBtnSubmitRepairOrder.ClientID %>").click(function() {

                    return $('#aspnetForm').valid();
                });
            });
        });

        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <div class="form_table">
            <table class="dropdown_pad ">
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">Company</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlCompany" runat="server" onchange="pageLoad(this,value);" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCompany">
                                </div>
                            </label>
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
                            <span class="input_label" style="width: 38%">Equipment Type</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlEquipmentType" runat="server" onchange="pageLoad(this,value);"
                                        OnSelectedIndexChanged="ddlEquipmentType_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvEquipmentType">
                                </div>
                            </label>
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
                            <span class="input_label" style="width: 38%">Equipment Id</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlEquipmentId" runat="server" onchange="pageLoad(this,value);"
                                    OnSelectedIndexChanged="ddlEquipmentId_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                                 <div id="dvEquipmentId">
                                </div>
                            </label>
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
                            <span class="input_label" style="width: 38%">Vendor</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlVendor" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                            </label>
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
                            <span class="input_label" style="width: 38%">Reason for Repair</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlReasonforRepair" runat="server" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                 <div id="dvReasonforRepair">
                                </div>
                            </label>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label cal_widper">Return to Service By</span>
                                <asp:TextBox ID="txtReturntoServiceDate" runat="server" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                             <div id="dvReturntoServiceDate">
                                </div>
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
                            <span class="input_label" style="width: 38%">Status</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlEquipmentStatus" runat="server" onchange="pageLoad(this,value);"                                     >
                                    </asp:DropDownList>
                                </span>
                                <div id="dvStatus">
                                </div>
                            </label>
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
                            <span class="input_label" style="width: 38%">Notify Vendor</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlNotifyVendor" runat="server" onchange="pageLoad(this,value);"
                                        OnSelectedIndexChanged="ddlNotifyVendor_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0" Text="-Select-" />
                                        <asp:ListItem Value="True" Text="Yes" />
                                        <asp:ListItem Value="False" Text="No" />
                                    </asp:DropDownList>
                                </span>
                            </label>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr id="trVendorStation" runat="server" visible="false">
                    <td>
                        <h4>
                            Vendor Station :</h4>
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <div style="height: 100px; overflow: auto; margin-left:110px;margin-top:-10px;">
                                        <asp:DataList ID="dtVBaseStation" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="VBaseStationspan" runat="server">
                                                    <asp:CheckBox ID="chkVBaseStation" runat="server" />
                                                </span>
                                                <label>
                                                    <asp:Label ID="lblVBaseStation" Text='<%# Eval("sBaseStation") %>' runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnVBaseStation" runat="server" Value='<%#Eval("iBaseStationId")%>' />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">Notify Our Company Personal</span>
                            <label class="dropimg_width">
                                <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlNotifyOurInternalCompanyPersonal" runat="server" onchange="pageLoad(this,value);"
                                     OnSelectedIndexChanged="ddlNotifyOurInternalCompanyPersonal_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0" Text="-Select-" />
                                        <asp:ListItem Value="True" Text="Yes" />
                                        <asp:ListItem Value="False" Text="No" />
                                    </asp:DropDownList>
                                </span>
                            </label>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span>
                        </div>
                    </td>
                </tr>
                <tr id="trCustomerStation" runat="server" visible="false">
                    <td>
                        <h4>
                            Customer Station :</h4>
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <div style="height: 100px; overflow: auto; margin-left:110px;margin-top:-10px;">
                                        <asp:DataList ID="dtCBaseStation" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="CBaseStationspan" runat="server">
                                                    <asp:CheckBox ID="chkCBaseStation" runat="server" />
                                                </span>
                                                <label>
                                                    <asp:Label ID="lblCBaseStation" Text='<%# Eval("sBaseStation") %>' runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnCBaseStation" runat="server" Value='<%#Eval("iBaseStationId")%>' />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box taxt_area clearfix">
                                <span class="input_label alignleft" style="width: 38%!important;">Description of Problem</span>
                                <div class="textarea_box alignright" style="width: 57%;">
                                    <div class="scrollbar">
                                        <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                        </a>
                                    </div>
                                    <asp:TextBox ID="txtDescriptionofProblem" runat="server" TabIndex="25" TextMode="MultiLine"
                                        CssClass="scrollme1" Height="70px"></asp:TextBox>
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
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSubmitRepairOrder" class="grey2_btn" runat="server" ToolTip="Submit Repair Order"
                            OnClick="lnkBtnSubmitRepairOrder_Click"><span>Submit Repair Order</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
