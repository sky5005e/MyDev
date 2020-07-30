<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="AddSuplier.aspx.cs" Inherits="admin_ManageSupplierPartner_AddSuplier"
    Title="Manage Supplier Partner" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {

                        ctl00$ContentPlaceHolder1$txtSupllierName: { required: true },
                        ctl00$ContentPlaceHolder1$TxtUrl: { required: true },
                        ctl00$ContentPlaceHolder1$TxtLoginName: { required: true },
                        ctl00$ContentPlaceHolder1$TxtPassword: { required: true },
                        ctl00$ContentPlaceHolder1$DdlStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$TxtCreatedDate: { required: true, date: true }

                    },
                    messages: {

                        ctl00$ContentPlaceHolder1$txtSupllierName: { required: replaceMessageString(objValMsg, "Required", "Supplier Name") },
                        ctl00$ContentPlaceHolder1$TxtUrl: { required: replaceMessageString(objValMsg, "Required", "URL") },
                        ctl00$ContentPlaceHolder1$TxtLoginName: { required: replaceMessageString(objValMsg, "Required", "Login Name") },
                        ctl00$ContentPlaceHolder1$TxtPassword: { required: replaceMessageString(objValMsg, "Required", "Password") },
                        ctl00$ContentPlaceHolder1$DdlStatus: { required: replaceMessageString(objValMsg, "Required", "Status") },
                        ctl00$ContentPlaceHolder1$TxtCreatedDate: { date: replaceMessageString(objValMsg, "ValidDate", "Date") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$DdlStatus")
                            error.insertAfter("#dvStatus");


                        else
                            error.insertAfter(element);
                    }
                });
            });

            ////in case remove comment end
            //set link
            $("#<%=lnkSave.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });

        });
    
    $(function() {
            $(".datepicker").datepicker({
                buttonText: 'Date',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
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

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div style="text-align: center">
                    <asp:Label ID="lblMsg1" runat="server" CssClass="errormessage"></asp:Label>
                    <br />
                </div>
                <div>
                    <div class="form_pad">
                        <div class="form_table">
                            <table class="dropdown_pad">
                                <tr>
                                    <td>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Supllier Name</span>
                                                <asp:TextBox ID="txtSupllierName" runat="server" CssClass="w_label" TabIndex="1"></asp:TextBox>
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
                                                <span class="input_label">URL</span>
                                                <asp:TextBox ID="TxtUrl" runat="server" CssClass="w_label" TabIndex="2"></asp:TextBox>
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
                                                <span class="input_label">Login Name</span>
                                                <asp:TextBox ID="TxtLoginName" runat="server" CssClass="w_label" TabIndex="3"></asp:TextBox>
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
                                                <span class="input_label">Password</span>
                                                <asp:TextBox ID="TxtPassword" runat="server" CssClass="w_label" TabIndex="4"></asp:TextBox>
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
                                                <span class="input_label" style="width: 32%">Status</span>
                                                <label class="dropimg_width">
                                                    <span class="custom-sel label-sel-small" style="width: 195px">
                                                        <asp:DropDownList ID="DdlStatus" runat="server" onchange="pageLoad(this,value);">
                                                            <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Deactive"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div id="dvStatus">
                                                    </div>
                                                </label>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form_table" style="padding-top: 2px">
                                        <div class="calender_l" style="width: 450px">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box supplier_annual_date">
                                                <span class="input_label">Supplier Created Date</span>
                                                <asp:TextBox ID="TxtCreatedDate" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="alignnone spacer25">
                                        </div>
                                        <div class="additional_btn">
                                            <ul class="clearfix">
                                                <li>
                                                    <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" OnClick="lnkSave_Click">
								        <span>Save Information</span>
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <input id="hdnStatus" runat="server" value="0" type="hidden" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
