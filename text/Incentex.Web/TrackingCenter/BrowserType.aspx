<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="BrowserType.aspx.cs" Inherits="TrackingCenter_BrowserType" Title="Browser Type" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
    $().ready(function() {
        $("#ctl00_ContentPlaceHolder1_lnkBtnReportBrowserType").focus();
        $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
            objValMsg = $.xml2json(xml);

            $("#aspnetForm").validate({
                rules: {

                    ctl00$ContentPlaceHolder1$txtStartDateBrowserType: { required: true, date: true },
                    ctl00$ContentPlaceHolder1$txtEndDateBrowserType: { required: true, date: true }

                },
                messages: {


                ctl00$ContentPlaceHolder1$txtStartDateBrowserType: { required: replaceMessageString(objValMsg, "Required", "start date"), date: replaceMessageString(objValMsg, "ValidDate", "Date") },
                ctl00$ContentPlaceHolder1$txtEndDateBrowserType: { required: replaceMessageString(objValMsg, "Required", "end date"), date: replaceMessageString(objValMsg, "ValidDate", "Date") }
                },
                errorPlacement: function(error, element) {
                   error.insertAfter(element);
                }
            });

        });

        $("#<%=lnkBtnReportBrowserType.ClientID %>").click(function() {
            return $('#aspnetForm').valid();
        });

    });

    $(function() {
        $(".datepicker").datepicker({ buttonText: 'Date',
            showOn: 'button',
            buttonImage: '../images/calender-icon.jpg',
            buttonImageOnly: true
        });
        $(".datepicker").datepicker('setDate', new Date());
    });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
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
            <table class="dropdown_pad">
                <tr>
                    <td class="form_table" style="padding-top: 2px">
                        <div class="calender_l" style="width: 450px">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box supplier_annual_date">
                                <span class="input_label">Start Date</span>
                                <asp:TextBox ID="txtStartDateBrowserType" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
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
                                <asp:TextBox ID="txtEndDateBrowserType" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnReportBrowserType" class="grey2_btn" runat="server" ToolTip="Report"
                            OnClick="lnkBtnReportBrowserType_Click"><span>Report</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <%--this is for the gird--%>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div class="form_pad">
                    <div>
                        <div style="text-align: center; color: Red; font-size: larger;">
                            <asp:Label ID="lblmsg" runat="server">
                            </asp:Label>
                        </div>
                        <div>
                            <asp:GridView ID="gvBrowserType" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                CssClass="orderreturn_box" GridLines="None" 
                                RowStyle-CssClass="ord_content" AllowSorting="true" 
                                onrowcommand="gvBrowserType_RowCommand">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span class="first centeralign">Sr.</span>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="first centeralign">
                                                <%#Container.DataItemIndex+1 %></span>
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span class="centeralign">BrowserName</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                         <span class="first">
                                            <asp:LinkButton ID="lnkBrowserName" runat="server" Text='<%#Eval("BrowserName") %>' ToolTip='<% #Eval("BrowserName")  %>'
                                                CommandName="Sorting">  </asp:LinkButton>
                                               </span>
                                                
                                           
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="65%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span class="centeralign">Count</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCountTotal" Text='<% #(Convert.ToString(Eval("CountTotal")))%>'
                                                ToolTip='<% #Eval("CountTotal")  %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="25%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="alignright pagging" runat="server" visible="false" id="pager">
                            <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                            </asp:LinkButton>
                            <span>
                                <asp:DataList ID="lstPaging" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="lstPaging_ItemCommand"
                                    OnItemDataBound="lstPaging_ItemDataBound" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                            CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList></span>
                            <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
