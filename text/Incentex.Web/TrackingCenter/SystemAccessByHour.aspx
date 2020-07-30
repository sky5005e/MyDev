<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="SystemAccessByHour.aspx.cs" Inherits="TrackingCenter_SystemAccessByHour" Title="System Access By Hour" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
    function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
        $().ready(function() {
            $("#ctl00_ContentPlaceHolder1_lnkBtnReportUserAccess").focus();
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtStartDate: { required: true, date: true },
                        ctl00$ContentPlaceHolder1$txtEndDate: { required: true, date: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtStartDate: { required: replaceMessageString(objValMsg, "Required", "start date"), date: replaceMessageString(objValMsg, "ValidDate", "Date") },
                        ctl00$ContentPlaceHolder1$txtEndDate: { required: replaceMessageString(objValMsg, "Required", "end date"), date: replaceMessageString(objValMsg, "ValidDate", "Date") }
                    },
                    errorPlacement: function(error, element) {
                        error.insertAfter(element);
                    }
                });
            });

            ////in case remove comment end
            //set link
            $("#<%=lnkBtnReportUserAccess.ClientID %>").click(function() {
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
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
        <div style="text-align: center; color: Red; font-size: larger;">
            <asp:Label ID="LblError" runat="server">
            </asp:Label>
        </div>
        <div class="form_table">
            <asp:Panel ID="pnl1" runat="server" DefaultButton="lnkBtnReportUserAccess">
                <table class="dropdown_pad">
                <tr>
                        <td class="form_table" style="padding-top: 2px">
                           <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Company</span> <span class="custom-sel label-sel-small">
                                                     <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" onchange="pageLoad(this,value);"
                                            OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_table" style="padding-top: 2px">
                           <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Employee</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlEmployee" TabIndex="1" onchange="pageLoad(this,value);"
                                                runat="server">
                                            </asp:DropDownList>
                                        </span>
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
                                <div class="form_box">
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
                                <div class="form_box">
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
                            <asp:LinkButton ID="lnkBtnReportUserAccess" class="grey2_btn" runat="server" ToolTip="Report"
                                TabIndex="0" OnClick="lnkBtnReportUserAccess_Click"><span>Report</span></asp:LinkButton>
                            <%--<asp:Button ID = "lnkBtnReportUserAccess" OnClick="lnkBtnReportUserAccess_Click" runat ="server"  Text="Report"/>--%>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <%--this is for the grid start--%>
         <asp:UpdateProgress runat="server" ID="uprogressPGrid" DisplayAfter="1" AssociatedUpdatePanelID="up1">
        <ProgressTemplate>
            <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
            </div>
            <div class="updateProgressDiv">
                <img alt="Loading" src="../Images/ajax-loader-large.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div class="form_pad">
                    <div>
                        <div style="text-align: center; color: Red; font-size: larger;">
                            <asp:Label ID="lblmsg" runat="server">
                            </asp:Label>
                        </div>                       
                       <div id="dvReport" runat="server" visible="false">
            <div class="black_top_co">
                <span>&nbsp;</span></div>
            <div class="header_bg" style="margin-top: -3px;">
                <div class="header_bgr clearfix">
                    <span class="title alignleft">System Access Report</span>
                </div>
            </div>
            <div class="black_middle" style="text-align: center;" >
                <asp:Chart ID="chrtStatusReport" runat="server" BackColor="Transparent" Height="500" OnClick="chrtStatusReport_Click"
                    Width="900">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Column" XValueMember="HR"  YValueMembers="AC" PostBackValue="#VALX"
                            IsValueShownAsLabel="true" Label="#VALY" LabelForeColor="#878282" CustomProperties="DrawingStyle=Cylinder,PointWidth=0.7,BarLabelStyle=Outside"
                            ToolTip="#VALY">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="Transparent" BackImageTransparentColor="Transparent"
                            BackImageWrapMode="TileFlipX">
                            <AxisY Title="System Access" TitleForeColor="#FFFFFF" LineColor="#202020" IsLabelAutoFit="True">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" Interval="1" />
                            </AxisY>
                            <AxisX Title="Hours" IsStartedFromZero="false" TitleForeColor="#FFFFFF" LineColor="#202020">
                                <MajorGrid Enabled="false" />
                                <LabelStyle ForeColor="#878282" Interval="2"/>
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
            <div class="black_bot_co">
                <span>&nbsp;</span></div>
        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--this is for the grid end--%>
    </div>
</asp:Content>

