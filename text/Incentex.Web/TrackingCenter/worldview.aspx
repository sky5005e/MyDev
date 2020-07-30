<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="worldview.aspx.cs" Inherits="TrackingCenter_worldview" Title="World view" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

 <script type="text/javascript" language="javascript">
        $().ready(function() {
            $("#ctl00_ContentPlaceHolder1_lnkBtnReportWorldWide").focus();
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $("#aspnetForm").validate({
                    rules: {

                        ctl00$ContentPlaceHolder1$txtStartDateip: { required: true, date: true },
                        ctl00$ContentPlaceHolder1$txtEndDateip: { required: true, date: true }

                    },
                    messages: {


                    ctl00$ContentPlaceHolder1$txtStartDateip: { required: replaceMessageString(objValMsg, "Required", "start date"), date: replaceMessageString(objValMsg, "ValidDate", "Date") },
                    ctl00$ContentPlaceHolder1$txtEndDateip: { required: replaceMessageString(objValMsg, "Required", "end date"), date: replaceMessageString(objValMsg, "ValidDate", "Date") }
                    },
                    errorPlacement: function(error, element) {
                       error.insertAfter(element);
                    }
                });



                //set link
                $("#<%=lnkBtnReportWorldWide.ClientID %>").click(function() {
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
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div class="form_pad">
    
        <div class="form_table">
            <table class="dropdown_pad">
                <tr>
                    <td class="form_table" style="padding-top: 2px">
                        <div class="calender_l" style="width: 450px">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box supplier_annual_date">
                                <span class="input_label">Start Date</span>
                                <asp:TextBox ID="txtStartDateip" runat="server" CssClass="w_label datepicker min_w"
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
                                <asp:TextBox ID="txtEndDateip" runat="server" CssClass="w_label datepicker min_w"
                                    TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnReportWorldWide" class="grey2_btn" runat="server" ToolTip="Report"
                            OnClick="lnkBtnReportWorldWide_Click"><span>Report</span></asp:LinkButton>
                        <%--<asp:Button ID = "lnkBtnReportUserAccess" OnClick="lnkBtnReportUserAccess_Click" runat ="server"  Text="Report"/>--%>
                    </td>
                </tr>
            </table>
            <%--<artem:GoogleMap ID="GoogleMap1" Width="920px" Height="600px" runat="server" IsSensor="false"
                EnableInfoWindow="true" EnableMarkerManager="false" Latitude="50.72870" Longitude="-1.85046"
                Zoom="3" EnableScrollWheelZoom="true" BorderWidth="2" Visible="false">
            </artem:GoogleMap>--%>
            <%--if not work delete below code and uncomment above code of googlemap--%>
            <%--popup for the map--%>
            <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
            <at:ModalPopupExtender ID="modal" TargetControlID="lnkDummyAddNew" BackgroundCssClass="modalBackground"
                DropShadow="true" runat="server" PopupControlID="pnlPriority" CancelControlID="closepopup">
            </at:ModalPopupExtender>
            <asp:Panel ID="pnlPriority" runat="server" Style="display: none;">
                <div class="pp_pic_holder facebook" style="display: block; width: 1022px; position:fixed;left:5%;top:4%;">
                    <div class="pp_top" style="">
                        <div class="pp_left">
                        </div>
                        <div class="pp_middle">
                        </div>
                        <div class="pp_right">
                        </div>
                    </div>
                    <div class="pp_content_container" style="">
                        <div class="pp_left" style="">
                            <div class="pp_right" style="">
                                <div class="pp_content" style="height: 656px; display: block;">
                                    <div class="pp_loaderIcon" style="display: none;">
                                    </div>
                                    <div class="pp_fade" style="display: block;">
                                        <a title="Expand the image" class="pp_expand" href="#">Expand</a>
                                        <div class="pp_hoverContainer" style="height: 92px; width: 742px; display: none;">
                                            <a href="#" class="pp_next" style="visibility: hidden;">next</a> <a href="#" class="pp_previous"
                                                style="visibility: visible;">previous</a>
                                        </div>
                                        <div id="pp_full_res">
                                            <div class="pp_inline clearfix">
                                                <div class="form_popup_box">
                                                    <div>
                                                        <artem:GoogleMap ID="GoogleMap1" Width="920px" Height="600px" runat="server" IsSensor="false"
                                                            EnableInfoWindow="true" EnableMarkerManager="false" Latitude="50.72870" Longitude="-1.85046"
                                                            Zoom="10" EnableScrollWheelZoom="true" BorderWidth="2">
                                                        </artem:GoogleMap>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="pp_details clearfix" style="width: 742px;">
                                            <a href="#" id="closepopup" runat="server" class="pp_close">Close</a>
                                            <p class="pp_description" style="display: none;">
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="pp_bottom" style="">
                        <div class="pp_left" style="">
                        </div>
                        <div class="pp_middle" style="">
                        </div>
                        <div class="pp_right" style="">
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <input type="hidden" id="hfX" value="" runat="server" />
            <input type="hidden" id="hfY" value="" runat="server" />
        </div>
    
       
    </div>
</asp:Content>
