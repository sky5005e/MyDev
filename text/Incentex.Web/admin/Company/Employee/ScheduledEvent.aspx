<%@ Page Title="Company Employee >> Schedule Event" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="ScheduledEvent.aspx.cs" Inherits="admin_Company_Employee_ScheduledEvent" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<link media="screen" rel="stylesheet" href="../../../CSS/colorbox.css" />
    <style type="text/css">
        .custom-checkbox input, .custom-checkbox_checked input
        {
            width: 20px;
            height: 20px;
            margin-left: -8px;
        }
    </style>
    <script type="text/javascript" src="../../../JS/JQuery/jquery-ui-timepicker-addon.js"></script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>

    <script type="text/javascript" language="javascript">

        $().ready(function() {

            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtEventName: { required: true },
                        ctl00$ContentPlaceHolder1$txtEventDate: { required: true },
                        ctl00$ContentPlaceHolder1$txtEventTime: { required: true },
                        ctl00$ContentPlaceHolder1$ddlEventReminder: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtEventName: { required: replaceMessageString(objValMsg, "Required", "Event name") },
                        ctl00$ContentPlaceHolder1$txtEventDate: { required: replaceMessageString(objValMsg, "Required", "Event date") },
                        ctl00$ContentPlaceHolder1$txtEventTime: { required: replaceMessageString(objValMsg, "Required", "Event time") },
                        ctl00$ContentPlaceHolder1$ddlEventReminder: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Event reminder") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlEventReminder")
                            error.insertAfter("#dvReminder");
                        else
                            error.insertAfter(element);
                    },
                    onsubmit: true
                });

            });


            $("#ctl00_ContentPlaceHolder1_lnkBtnSave").click(function() {
                return $("#aspnetForm").valid();
            });

        });

        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                // dateFormat: 'dd-mm-yy',
                buttonImage: '../../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });

        $(function() {
            $('#ctl00_ContentPlaceHolder1_txtEventTime').timepicker({
        });
    });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<at:ToolkitScriptManager ID="ScriptManager1" runat="server">
</at:ToolkitScriptManager>
    <div class="form_pad">
        <div class="shipping_method_pad" style="width: 900px;">
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div class="spacer10">
            </div>
            <table class="select_box_pad form_table">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 40%">Event Name</span>
                                <asp:TextBox ID="txtEventName" CssClass="w_label" runat="server" />
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
                            <div class="form_box">
                                <span class="input_label">Select Date</span>
                                <asp:TextBox ID="txtEventDate" CssClass="datepicker w_label" runat="server"></asp:TextBox>
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
                                <span class="input_label">Select Time</span>
                                <asp:TextBox ID="txtEventTime" CssClass="w_label" runat="server"></asp:TextBox>
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
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small" style="width: 61%;">
                                        <asp:DropDownList ID="ddlEventReminder" runat="server" onchange="pageLoad(this,value);">
                                            <asp:ListItem Text="-select Reminder-" Selected="True" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="5 Minutes" Value="5 Minutes"></asp:ListItem>
                                            <asp:ListItem Text="10 Minutes" Value="10 Minutes"></asp:ListItem>
                                            <asp:ListItem Text="15 Minutes" Value="15 Minutes"></asp:ListItem>
                                            <asp:ListItem Text="20 Minutes" Value="20 Minutes"></asp:ListItem>
                                            <asp:ListItem Text="25 Minutes" Value="25 Minutes"></asp:ListItem>
                                            <asp:ListItem Text="30 Minutes" Value="30 Minutes"></asp:ListItem>
                                            <asp:ListItem Text="35 Minutes" Value="35 Minutes"></asp:ListItem>
                                            <asp:ListItem Text="40 Minutes" Value="40 Minutes"></asp:ListItem>
                                            <asp:ListItem Text="45 Minutes" Value="45 Minutes"></asp:ListItem>
                                            <asp:ListItem Text="50 Minutes" Value="50 Minutes"></asp:ListItem>
                                            <asp:ListItem Text="55 Minutes" Value="55 Minutes"></asp:ListItem>
                                            <asp:ListItem Text="1 Hours" Value="1 Hours"></asp:ListItem>
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvReminder">
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
                        <asp:LinkButton runat="server" ID="lnkbtnAdditionalEmployee" class="grey2_btn" OnClick="lnkBtnlnkbtnAdditionalEmployee_Click"><span>Add Employee</span></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton runat="server" ID="lnkBtnSave" class="gredient_btn" ToolTip="Save"
                            OnClick="lnkBtnSave_Click"><span><strong>Save</strong></span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:LinkButton runat="server" ID="lnkbtnAdditionalEmployeeDummy" class="grey2_btn"></asp:LinkButton>
            <at:ModalPopupExtender ID="modal" TargetControlID="lnkbtnAdditionalEmployeeDummy"
                BackgroundCssClass="modalBackground" DropShadow="true" runat="server" PopupControlID="pnlAdditionalEmployee"
                CancelControlID="cboxClose">
            </at:ModalPopupExtender>
            <asp:Panel ID="pnlAdditionalEmployee" runat="server" Style="display: none;">
                <div id="cboxWrapper" style="display: block; width: 458px; height: 659px; position:fixed;left:35%;top:5%;">
                    <div style="">
                        <div id="cboxTopLeft" style="float: left;">
                        </div>
                        <div id="cboxTopCenter" style="float: left; width: 408px;">
                        </div>
                        <div id="cboxTopRight" style="float: left;">
                        </div>
                    </div>
                    <div style="clear: left;">
                        <div id="cboxMiddleLeft" style="float: left; height: 450px;">
                        </div>
                        <div id="cboxContent" style="float: left; width: 408px; display: block; height: 450px;">
                            <div id="cboxLoadedContent" style="display: block;">
                                <div style="padding:22px 10px 10px 10px;">
                                    <div class="weatherDetails true" style="height:auto; overflow:auto;">
                                        <table>
                                            <tr class="header">
                                                <td>
                                                    <div class="alignleft h-weather">
                                                        Employee Name</div>
                                                    <div class="alignleft h-link">
                                                        Links</div>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvIncentexEmployee" ShowHeader="false" runat="server" AutoGenerateColumns="false"
                                            GridLines="None" CssClass="weather_box">
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblId" runat="server" Text='<%#Eval("UserInfoID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <span>
                                                            <%#Eval("EmployeeName")%>
                                                        </span>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="alignleft d-weather" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <span runat="server" id="dvChk" class="custom-checkbox centeralign">
                                                            <asp:CheckBox ID="chk" runat="server" />
                                                        </span>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="alignright d-link" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="spacer10" style="clear: both;">
                                    </div>
                                    <div class="centeralign">
                                        <asp:LinkButton runat="server" ID="lnkbtnAddEmployee" OnClick="lnkbtnAddEmployee_Click"
                                            class="grey2_btn"><span>Add Employee</span></asp:LinkButton>
                                    </div>
                                </div>
                                <div id="cboxTitle" style="display: block;" class="">
                                </div>
                                <div id="cboxClose" style="" class="">
                                    close</div>
                            </div>
                        </div>
                        <div id="cboxMiddleRight" style="float: left; height: 450px;">
                        </div>
                    </div>
                    <div style="clear: left;">
                        <div id="cboxBottomLeft" style="float: left;">
                        </div>
                        <div id="cboxBottomCenter" style="float: left; width: 408px;">
                        </div>
                        <div id="cboxBottomRight" style="float: left;">
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
