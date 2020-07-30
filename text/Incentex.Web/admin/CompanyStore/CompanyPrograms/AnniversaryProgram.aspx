<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="AnniversaryProgram.aspx.cs" Inherits="admin_CompanyStore_CompanyPrograms_AnniversaryProgram"
    Title="Anniversary Program" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .form_table span.error
        {
            padding-left: 55px;
        }
    </style>
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>

    <script type="text/javascript" language="javascript">
        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                // dateFormat: 'dd-mm-yy',
                buttonImage: '../../../images/calender-icon.jpg',
                buttonImageOnly: true


            });
        });

        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtCreditAmount: { required: true, number: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup") },
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") },
                        ctl00$ContentPlaceHolder1$ddlStatus: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "program status") },
                        ctl00$ContentPlaceHolder1$txtCreditAmount: { required: replaceMessageString(objValMsg, "Required", "credit amount"), number: replaceMessageString(objValMsg, "Number", "Credit Amount") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlDepartment")
                            error.insertAfter("#dvDepartment");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlStatus")
                            error.insertAfter("#dvStatus");
                        else
                            error.insertAfter(element);
                    }

                });

            });

            if ($("#ctl00_ContentPlaceHolder1_txtPreviousCreditExpire").val()) {
                $('#ctl00_ContentPlaceHolder1_chkPreviousCreditExpire').attr("checked", "true");
                $('#menuspan').attr("class", "custom-checkbox_checked");
                $('#anneversarycredit').show();
            }
            else {
                $('#ctl00_ContentPlaceHolder1_chkPreviousCreditExpire').attr("checked", "false");
                $('#anneversarycredit').hide();

            }

            $('#ctl00_ContentPlaceHolder1_chkPreviousCreditExpire').click(function() {

                if ($(this).attr("checked") == true) {
                    $("#ctl00_ContentPlaceHolder1_txtPreviousCreditExpire").rules("add", {
                        required: true,
                        //number: true,
                        messages: {
                            required: replaceMessageString(objValMsg, "Required", "Set expire of previous credit.")
                        }
                    });

                    $('#menuspan').attr("class", "custom-checkbox_checked");
                    $('#anneversarycredit').show();

                }
                else {
                    $('#anneversarycredit').hide();
                    $("#ctl00_ContentPlaceHolder1_txtPreviousCreditExpire").rules("remove");

                }
            });
            $(function() {
                $(".datepicker1").datepicker({
                    buttonText: 'DatePicker',
                    showOn: 'button',
                    buttonImage: '../../../images/calender-icon.jpg',
                    buttonImageOnly: true
                });
            });

            $("#<%=lnkBtnSaveInformation.ClientID %>").click(function() {
                return $('#aspnetForm').valid();

            });
        });

        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }

    </script>

    <mb:MenuUserControl ID="menuControl" runat="server" />
    <div class="form_pad">
        <div>
            <div style="text-align: center; color: Red; font-size: larger;">
                <asp:Label ID="lblmsg" runat="server">
                </asp:Label>
            </div>
            <div class="dropdown_pad form_table">
                <table>
                    <tr>
                        <td>
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box shipmax_in">
                                    <span class="input_label">Department</span> <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlDepartment" runat="server" TabIndex="6" onchange="pageLoad(this,value);">
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
                                <div class="checktable_supplier true clearfix">
                                    <span class="custom-checkbox alignleft" id="menuspan">
                                        <asp:CheckBox ID="chkPreviousCreditExpire" runat="server" />
                                    </span>
                                    <label>
                                        Set Expire of Starting Credit</label>
                                </div>
                            </div>
                            <div class="spacer10">
                            </div>
                            <div id="anneversarycredit" style="display: none;">
                                <table>
                                    <tr>
                                        <td>
                                            <div class="calender_l">
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label" style="width: 45%;">Set Expire of Starting Credit:</span>
                                                    <asp:TextBox ID="txtPreviousCreditExpire" runat="server" CssClass="datepicker1 cal_w"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label">Standard Credit Amount ($):</span>
                                    <asp:TextBox ID="txtCreditAmount" runat="server" CssClass="w_label"></asp:TextBox>
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
                                    <span class="input_label">Issue Credit In (Months) :</span> <span class="custom-sel  label-sel-small">
                                        <asp:DropDownList ID="ddlMonths" runat="server">
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
                                    <span class="input_label">Credit Expires After (Months):</span> <span class="custom-sel  label-sel-small">
                                        <asp:DropDownList ID="ddlCreditExpiresAfter" runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td>
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label">Credits Status :</span> <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlCreditStatus" runat="server" onchange="pageLoad(this,value);">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td>
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label">Rule Status :</span> <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlRuleStatus" runat="server" onchange="pageLoad(this,value);">
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
                                    <span class="input_label">Status</span> <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="6" onchange="pageLoad(this,value);">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvStatus">
                                    </div>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="spacer10">
                        </td>
                    </tr>
                    <tr>
                        <td class="centeralign">
                            <asp:LinkButton ID="lnkBtnSaveInformation" CssClass="grey_btn" runat="server" ToolTip="Save Information"
                                OnClick="lnkBtnSaveInformation_Click"><span>Save Information</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
