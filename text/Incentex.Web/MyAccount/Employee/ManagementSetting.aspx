<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManagementSetting.aspx.cs" Inherits="admin_Company_Employee_ManagementSetting"
    Title="Company Employee >> Management Setting" %>

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
            padding-left: 36px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: "0" }

                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlWorkgroup: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "workgroup") },
                        ctl00$ContentPlaceHolder1$ddlDepartment: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "department") }

                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlWorkgroup")
                            error.insertAfter("#dvWorkgroup");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlDepartment")
                            error.insertAfter("#dvDepartment");
                        else
                            error.insertAfter(element);
                    }

                });


            });


            $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {

                return $('#aspnetForm').valid();
            });
        });

        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }

    </script>

   <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div class="spacer10">
    </div>
    <div class="tabcontent" id="management_settings">
        <%--<div class="header_bg">
            <div class="header_bgr">
                <span class="title alignleft">Management Settings</span> <span class="date alignright">
                </span>
                <div class="alignnone">
                    &nbsp;</div>
            </div>
        </div>--%>
        <div class="alignnone">
            &nbsp;</div>
        <div class="form_pad" id="isAdmin" runat="server">
            <div class="employee_name">
                User Name:
                <asp:Label ID="lblUserFullName" runat="server" Text=""></asp:Label>
            </div>
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <h4>
                Additional Management Level Store Menu Options</h4>
            <div class="form_table">
                <table>
                    <tr>
                        <td class="formtd">
                            <table class="checktable_supplier true">
                                <tr>
                                    <td>
                                        <asp:DataList ID="dtlMenus" runat="server">
                                            <ItemTemplate>
                                                <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                                    <asp:CheckBox ID="chkdtlMenus" runat="server" /></span>
                                                <label>
                                                    <asp:Label ID="lblMenus" Text='<%# Eval("sDescription") %>' runat="server"></asp:Label></label>
                                                <asp:HiddenField ID="hdnMenuAccess" runat="server" Value='<%# Eval("iMenuPrivilegeID") %>' />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="divider">
            </div>
            <h4>
                Management Control for the Following</h4>
            <div class="form_table clearfix">
                <div class="select_formtd alignleft">
                    <table class="maxform_td">
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box ">
                                        <label class="dropimg_width">
                                            <span class="input_label">Department</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlDepartment" runat="server" onchange="pageLoad(this,value);">
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
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box ">
                                        <label class="dropimg_width">
                                            <span class="input_label">Workgroup</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlWorkgroup" runat="server" onchange="pageLoad(this,value);">
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
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box ">
                                        <label class="dropimg_width">
                                            <span class="input_label">Base Station</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlBaseStation" runat="server" onchange="pageLoad(this,value);">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvstation">
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
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box ">
                                        <label class="dropimg_width">
                                            <span class="input_label">Region</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlRegion" runat="server" onchange="pageLoad(this,value);">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvRegion">
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
            </div>
            <div class="spacer25">
            </div>
            <div class="botbtn centeralign">
                <%--<a href="#" class="grey2_btn" title="Edit Information"><span>Edit Information</span></a>--%>
                <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save Information"
                    OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
            </div>
        </div>
        <div class="form_pad" id="isEmployee" runat="server">
            <div style="text-align: center">
                <asp:Label ID="lblIsEmployeeMsg" runat="server" Text="This tab is disabled for company employee!!"
                    CssClass="errormessage"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
