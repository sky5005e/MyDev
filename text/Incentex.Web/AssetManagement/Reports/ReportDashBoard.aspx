<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ReportDashBoard.aspx.cs" Inherits="AssetManagement_Reports_ReportDashBoard" Title="Report Dash Board"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .form_box .custom-sel span.error
        {
            padding: 24px 0;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlReportType: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlReportType: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Report Type") }
                    },
                    errorPlacement: function(error, element) {
                        error.insertAfter(element);
                    }
                });

            });

            $("#ctl00_ContentPlaceHolder1_lnkSubmitRequest").click(function() {
                return $('#aspnetForm').valid();
            });

        });
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
            
               //Maintainscroll 
             $(window).scroll(function () {
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
        });
        
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
     <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
        <div class="form_table">
        <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
       <%--  <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>--%>
            <table class="dropdown_pad">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%">Company</span>
                                <label class="dropimg_width230">
                                    <span class="custom-sel label-sel-small-Product">
                                        <asp:DropDownList ID="ddlCompany" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
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
                            <div class="form_box">
                                <span class="input_label" style="width: 30%">Base Station</span>
                                <label class="dropimg_width230">
                                    <span class="custom-sel label-sel-small-Product">
                                        <asp:DropDownList ID="ddlBaseStation" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
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
                            <div class="form_box">
                                <span class="input_label" style="width: 30%">Equipment Type</span>
                                <label class="dropimg_width230">
                                    <span class="custom-sel label-sel-small-Product">
                                        <asp:DropDownList ID="ddlEquipmentType" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
               
                    <td id="dvFDate" runat="server">
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%;">From Date</span>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="cal_w datepicker1"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                   
                </tr>
                <tr>
               
                    <td id="dvTDate" runat="server">
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%;">To Date</span>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="cal_w datepicker1"></asp:TextBox>
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
                                <span class="input_label" style="width: 30%">Report Type</span>
                                <label class="dropimg_width230">
                                    <span class="custom-sel label-sel-small-Product">
                                        <asp:DropDownList ID="ddlReportType" onchange="pageLoad(this,value);" runat="server"
                                            AutoPostBack="true" 
                                    onselectedindexchanged="ddlReportType_SelectedIndexChanged"  >
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
               
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkSubmitRequest" class="gredient_btn" runat="server" OnClick="lnkSubmitRequest_Click"><span><strong>Search Now</strong></span></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <%--</ContentTemplate>
         </asp:UpdatePanel>--%>
        </div>
    </div>
</asp:Content>

