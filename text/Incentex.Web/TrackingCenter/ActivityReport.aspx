<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ActivityReport.aspx.cs" Inherits="TrackingCenter_ActivityReport" Title="Tracking Center" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript" language="javascript">
        $().ready(function() {
            $("#ctl00_ContentPlaceHolder1_lnkBtnActivityReport").focus();
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtStartDate: { required: true, date: true },
                        ctl00$ContentPlaceHolder1$txtEndDate: { required: true, date: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtStartDate: { required: replaceMessageString(objValMsg, "Required", "start date"),date: replaceMessageString(objValMsg, "ValidDate", "Date") },
                        ctl00$ContentPlaceHolder1$txtEndDate: { required: replaceMessageString(objValMsg, "Required", "end date"),date: replaceMessageString(objValMsg, "ValidDate", "Date") }
                    },
                    errorPlacement: function(error, element) {
                    error.insertAfter(element);
                    }
                });
            });
            
            //set link
            $("#<%=lnkBtnActivityReport.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });

        });

        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'Date',
                showOn: 'button',
                buttonImage: '../images/calender-icon.jpg',
                buttonImageOnly: true
            });
            $(".datepicker").datepicker('setDate', new Date());
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div style="text-align: center; color: Red; font-size: larger;">
            <asp:Label ID="LblError" runat="server">
            </asp:Label>
        </div>
        <div class="form_table">
            <asp:Panel ID="pnl1" runat="server" DefaultButton="lnkBtnActivityReport">
                <table class="dropdown_pad">
                    <tr>
                        <td class="form_table" style="padding-top: 2px">
                            <div class="calender_l" style="width: 450px">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box supplier_annual_date">
                                    <span class="input_label">Start Date</span>
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="w_label datepicker min_w"
                                        TabIndex="1"></asp:TextBox>
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
                                    <span class="input_label">End Date</span>
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="w_label datepicker min_w" TabIndex="1"></asp:TextBox>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="centeralign">
                            <asp:LinkButton ID="lnkBtnActivityReport" class="grey2_btn" runat="server" ToolTip="Report"
                                OnClick="lnkBtnActivityReport_Click"><span>Report</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PnlFetchData" runat="server">
                <table class="dropdown_pad ">
                    <tr>
                        <td class="form_table" style="padding-top: 2px">
                            <div class="calender_l" style="width: 450px">
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box supplier_annual_date">
                                    <span class="input_label">Number of User Accessed </span>
                                    <asp:TextBox ID="TxtNumOfUserAccessed" Enabled="false" runat="server"></asp:TextBox>
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
                                    <span class="input_label">Number of Users Placed Orders </span>
                                    <asp:TextBox ID="TxtNumOfUserPlacedOrder" Enabled="false" runat="server"></asp:TextBox>
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
                                    <span class="input_label">Number of Orders Placed </span>
                                    <asp:TextBox ID="TxtNumOfOrderPlaced" Enabled="false" runat="server"></asp:TextBox>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
