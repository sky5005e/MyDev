<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="AddProspects.aspx.cs" Inherits="admin_WorldwideProspect_AddProspects" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../../JS/JQuery/jquery.MultiFile.pack.js" type="text/javascript" language="javascript"></script>

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
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtCompanyName: { required: true },
                        ctl00$ContentPlaceHolder1$txtContactName: { required: true },
                        ctl00$ContentPlaceHolder1$txtEmail: { required: true, email: true },
                        ctl00$ContentPlaceHolder1$ddlBusinessType: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtCompanyName: { required: replaceMessageString(objValMsg, "Required", "company name") },
                        ctl00$ContentPlaceHolder1$txtContactName: { required: replaceMessageString(objValMsg, "Required", "cotact name") },
                        ctl00$ContentPlaceHolder1$txtEmail: { required: replaceMessageString(objValMsg, "Required", "email address"),
                            email: replaceMessageString(objValMsg, "Valid", "email address")
                        },
                        ctl00$ContentPlaceHolder1$ddlBusinessType: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "businesstype") },
                        ctl00$ContentPlaceHolder1$ddlCountry: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "country") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCountry")
                            error.insertAfter("#dvcountry");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlBusinessType")
                            error.insertAfter("#dvbusinesstype");
                        else
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
            <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
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
                <tr id="trCompanyName" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Company Name</span>
                                <asp:TextBox ID="txtCompanyName" TabIndex="1" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trContactName" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Contact Name</span>
                                <asp:TextBox ID="txtContactName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
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
                                <span class="input_label" style="width: 30%;">Email</span>
                                <asp:TextBox ID="txtEmail" TabIndex="3" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trBusinessType" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Business Type</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlBusinessType" onchange="pageLoad(this,value);" runat="server"
                                        TabIndex="4">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvbusinesstype">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trCountry" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Country</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlCountry" onchange="pageLoad(this,value);" runat="server"
                                        TabIndex="5">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvcountry">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save Prospects"
                            TabIndex="8" OnClick="lnkBtnSaveInfo_Click"><span>Save</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
