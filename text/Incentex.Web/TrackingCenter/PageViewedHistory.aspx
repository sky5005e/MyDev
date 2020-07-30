<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="PageViewedHistory.aspx.cs" Inherits="TrackingCenter_PageViewedHistory"
    Title="Tracking Center" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
     $().ready(function() {
         $("#ctl00_ContentPlaceHolder1_lnkBtnPageview").focus();
         $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
             objValMsg = $.xml2json(xml);

             $("#aspnetForm").validate({
                 rules: {
                     ctl00$ContentPlaceHolder1$txtStartPageview: { required: true, date: true },
                     ctl00$ContentPlaceHolder1$txtEndPageview: { required: true, date: true },
                     ctl00$ContentPlaceHolder1$TxtNosOfRecord: { required: true}
                 },
                 messages: {
                     ctl00$ContentPlaceHolder1$txtStartPageview: { required: replaceMessageString(objValMsg, "Required", "start date"), date: replaceMessageString(objValMsg, "ValidDate", "Date") },
                     ctl00$ContentPlaceHolder1$txtEndPageview: { required: replaceMessageString(objValMsg, "Required", "end date"), date: replaceMessageString(objValMsg, "ValidDate", "Date") },
                     ctl00$ContentPlaceHolder1$TxtNosOfRecord: { required: replaceMessageString(objValMsg, "Required", "numbers of records"), date: replaceMessageString(objValMsg, "ValidDate", "numbers of records") }
                 },
                 errorPlacement: function(error, element) {
                     error.insertAfter(element);
                 }
             });

         });
         ////in case remove comment end
         //set link
         $("#<%=lnkBtnPageview.ClientID %>").click(function() {
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
                                <asp:TextBox ID="txtStartPageview" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
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
                                <asp:TextBox ID="txtEndPageview" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="form_table" style="padding-top: 2px">
                        <div>
                            <div class="form_top_co" style="width: 450px">
                                <span>&nbsp;</span></div>
                            <div class="form_box supplier_annual_date">
                                <span class="input_label">Numbers of Record(s)</span>
                                <asp:TextBox ID="TxtNosOfRecord" runat="server" CssClass="w_label">10</asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnPageview" class="grey2_btn" runat="server" ToolTip="Report"
                            OnClick="lnkBtnPageview_Click"><span>Report</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div class="form_pad">
                    <div>
                        <div style="text-align: center; color: Red; font-size: larger;">
                            <asp:Label ID="lblmsg" runat="server">
                            </asp:Label>
                        </div>
                        <asp:GridView ID="gvPageViewedHistory" runat="server" AutoGenerateColumns="false"
                            HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                            RowStyle-CssClass="ord_content" 
                            onrowcommand="gvPageViewedHistory_RowCommand1">
                            <Columns>
                                <%--<asp:TemplateField Visible="false" HeaderText="Id">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblUTID" Text='<%# Eval("UserInfoID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
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
                                        <asp:LinkButton ID="lnkPageName" runat="server" CommandArgument="Name" ><span >Page Name</span></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span>
                                        
                                            <asp:LinkButton ID="lnkUserList" runat="server" 
                                                Text='<%#Eval("PageName") %>' ToolTip='<% #Eval("CompleteURL")  %>' CommandName="Sorting">  </asp:LinkButton>
                                        </span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" />
                                </asp:TemplateField>
                                <%--this page name is working start --%>
                                <%--<asp:TemplateField>
                                    <HeaderTemplate>
                                        <span class="centeralign">Page Name</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblPageName" Text='<% #Eval("PageName")%>' ToolTip='<% #Eval("CompleteURL")  %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="65%" />
                                </asp:TemplateField>--%>
                                <%--this page name is working end--%>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span class="centeralign">Pages Viewd</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="centeralign">
                                            <%# Eval("PagesViewed") %></span>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
