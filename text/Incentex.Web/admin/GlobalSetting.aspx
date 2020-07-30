<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="GlobalSetting.aspx.cs" Inherits="admin_GlobalSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<link rel="stylesheet" href="../CSS/jquery.timepicker-1.1.0.css" />--%>

    <%--<script src="../JS/JQuery/jquery.timepicker-1.1.0.js" language="javascript" type="text/javascript"></script>--%>

    <style type="text/css">
        .select_box_pad
        {
            width: 325px;
            margin: 0px auto;
        }
        .employeeedit_text .input_label
        {
            width: 45%;
        }
        .globalDivider
        {
            background: url(       "../images/divider-bg.gif" ) repeat-x scroll 0 0 transparent;
            height: 14px;
            margin-top: 0px;
        }
        .form_pad
        {
            min-height: 0px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                    ctl00$ContentPlaceHolder1$txtPageSize: { required: true, number: true },
                    ctl00$ContentPlaceHolder1$txtNoOfDays: { required: true, number: true }

                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtPageSize: {
                            required: replaceMessageString(objValMsg, "Required", "page size"),
                            number: replaceMessageString(objValMsg, "Number", "page size")
                        },
                        ctl00$ContentPlaceHolder1$txtNoOfDays: {
                            required: replaceMessageString(objValMsg, "Required", "Reminder Day"),
                            number: replaceMessageString(objValMsg, "Number", "Reminder Day")
                        }
                    }
                });
            });
            
            $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });
            
            $(".datetimepicker1").datetimepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calender-icon.jpg',
                buttonImageOnly: true,
                timeFormat: "hh:mm tt",
                minDate: new Date(),
                changeMonth: true,
                changeYear: true,
                ampm: true
            }).attr("readonly", "readonly");
        });
    </script>

    <div class="spacer20">
    </div>
    <div style="text-align: center">
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
    </div>
    <div class="shipping_method_pad">
        <table class="select_box_pad form_table">
            <tr>
                <td>
                    <div>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box employeeedit_text clearfix">
                            <span class="input_label">Number of Views Per Page:</span>
                            <asp:TextBox ID="txtPageSize" runat="server"></asp:TextBox>
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
                            <span class="input_label">MOAS Reminder Day:</span>
                            <asp:TextBox ID="txtNoOfDays" runat="server"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="botbtn centeralign">
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" ToolTip="Save Basic Information"
                            OnClick="lnkBtnSaveInfo_Click"><span>Save Information</span></asp:LinkButton>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="globalDivider">
    </div>
    <div class="shipping_method_pad">
        <table class="select_box_pad form_table">
            <tr>
                <td align="right">
                    <div class="botbtn centeralign">
                        <asp:LinkButton ID="lnkBtnGetDataBaseValues" class="grey2_btn" runat="server" ToolTip="Get DB Values"
                            OnClick="lnkBtnGetDataBaseValues_Click"><span>Get DB Values</span></asp:LinkButton>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <div class="botbtn centeralign">
                        <asp:LinkButton ID="lnkUpdateAllStore" class="grey2_btn" runat="server" ToolTip="Update All Store"
                            OnClick="lnkUpdateAllStore_Click"><span>Update All Store Status</span></asp:LinkButton>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="globalDivider">
    </div>
    <div class="form_pad">
        <h4>
            SAP Settings</h4>
        <asp:DataList ID="dlSAPSettings" runat="server">
            <ItemTemplate>
                <div>
                    <table class="form_table">
                        <tr>
                            <td>
                                <div class='<%# Convert.ToBoolean(Eval("IsValueDateTime")) ? "calender_l" : "" %>'>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box" title='<%# Eval("ToolTip") %>'>
                                        <span id="lblSettingLabel" style='<%# Convert.ToBoolean(Eval("IsValueDateTime")) ? "width: 38.5%;": "width: 33%;" %>'
                                            class="input_label">
                                            <%# Eval("SettingLabel") %></span>
                                        <asp:TextBox ID="txtSettingValue" runat="server" Width="58%" MaxLength="500" Text='<%# Convert.ToBoolean(Eval("IsValueDateTime")) ? Convert.ToDateTime(Eval("SettingValue")).ToString("MM/dd/yyyy hh:mm tt") : Eval("SettingValue") %>'
                                            CssClass='<%# Convert.ToBoolean(Eval("IsValueDateTime")) ? "datetimepicker1 w_label" : "" %>'></asp:TextBox>
                                        <asp:HiddenField ID="hdnIsValueDateTime" runat="server" Value='<%# Eval("IsValueDateTime") %>' />
                                        <asp:HiddenField ID="hdnSettingKey" runat="server" Value='<%# Eval("SettingKey") %>' />
                                        <asp:HiddenField ID="hdnSettingID" runat="server" Value='<%# Eval("SettingID") %>' />
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </ItemTemplate>
        </asp:DataList>
        <div class="botbtn centeralign">
            <asp:LinkButton ID="lnkSaveSAPSettings" class="grey2_btn" runat="server" ToolTip="Save SAP Settings"
                OnClick="lnkSaveSAPSettings_Click"><span>Save SAP Settings</span></asp:LinkButton>
        </div>
    </div>
</asp:Content>
