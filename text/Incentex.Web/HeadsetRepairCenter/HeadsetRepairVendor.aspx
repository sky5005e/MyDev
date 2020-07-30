<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="HeadsetRepairVendor.aspx.cs" Inherits="HeadsetRepairCenter_HeadsetRepairVendor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../JS/JQuery/jquery.MultiFile.pack.js" type="text/javascript" language="javascript"></script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        var formats = 'pdf|jpg|jpeg|xls|xlsx|doc';
        $().ready(function() {
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtVendorName: { required: true },
                        ctl00$ContentPlaceHolder1$txtContact: { required: true },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: true },
                        ctl00$ContentPlaceHolder1$txtEmail: { required: true, email: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtVendorName: { required: replaceMessageString(objValMsg, "Required", "vendor name") },
                        ctl00$ContentPlaceHolder1$txtContact: { required: replaceMessageString(objValMsg, "Required", "contact") },
                        ctl00$ContentPlaceHolder1$txtTelephone: { required: replaceMessageString(objValMsg, "Required", "telephone") },
                        ctl00$ContentPlaceHolder1$txtEmail: { required: replaceMessageString(objValMsg, "Required", "email"), email: replaceMessageString(objValMsg, "Valid", "email address") }

                    },
                    errorPlacement: function(error, element) {
                        error.insertAfter(element);
                    }
                });
            });

            $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                if ($('#aspnetForm').valid()) {
                    $('#dvLoader').show();
                    return true;
                }
                else {
                    return false;
                }
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad select_box_pad" style="width: 400px;">
        <div class="form_table">
            <div class="spacer10">
            </div>
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <div class="spacer10">
            </div>
            <table>
                <tr id="trVendorName" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Vendor Name </span>
                                <asp:TextBox ID="txtVendorName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trContact" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Contact </span>
                                <asp:TextBox ID="txtContact" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trTelephone" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Telephone </span>
                                <asp:TextBox ID="txtTelephone" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trEmail" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Email </span>
                                <asp:TextBox ID="txtEmail" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save"
                            OnClick="lnkBtnSaveInfo_Click"><span>Save</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
