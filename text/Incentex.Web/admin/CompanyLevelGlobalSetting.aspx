<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="CompanyLevelGlobalSetting.aspx.cs" Inherits="admin_CompanyLevelGlobalSetting" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .form_box .custom-sel span.error
        {
            padding: 23px 0px;
            margin-left: 31%;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();

            }
        }
        $().ready(function() {
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") },
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlDepartment")
                            error.insertAfter("#dvDepartment");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else
                            error.insertAfter(element);
                    }
                });

            });

            $("#<%=lnkNext.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });
        });
    </script>

    <div class="form_pad">
        <div style="text-align: center;">
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="errormessage" Visible="false"></asp:Label>
        </div>
        <br />
        <h4 style="text-align: center;">
            In this step please select the department and work group you are planning for.</h4>
        <br />
        <div class="uniform_pad" style="width: 418px;">
            <table class="form_table">
                <tr>
                    <td>
                        <!--department START -->
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <label class="dropimg_width400">
                                    <span class="custom-sel status_small">
                                        <asp:DropDownList ID="ddlDepartment" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvDepartment">
                                    </div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <!-- workgroup -->
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <label class="dropimg_width400">
                                    <span class="custom-sel status_small">
                                        <asp:DropDownList ID="ddlWorkgroup" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvWorkgroup">
                                    </div>
                                </label>
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
        <div class="bot_alert next">
            <asp:LinkButton ID="lnkNext" runat="server" title="Next Step 2" OnClick="lnkNext_Click">Next Step 2</asp:LinkButton>
        </div>
    </div>
</asp:Content>
