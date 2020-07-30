<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UserPurchaseRpt.aspx.cs" Inherits="TrackingCenter_UserPurchaseRpt"
    Title="Tracking Center" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript" language="javascript">
    $().ready(function() {
        $("#ctl00_ContentPlaceHolder1_lnkBtnReportUserPurchase").focus();
        $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
            objValMsg = $.xml2json(xml);

            $("#aspnetForm").validate({
                rules: {

                    ctl00$ContentPlaceHolder1$txtStartDatePurchase: { required: true, date: true },
                    ctl00$ContentPlaceHolder1$txtEndDatePurchase: { required: true, date: true }

                },
                messages: {


                ctl00$ContentPlaceHolder1$txtStartDatePurchase: { required: replaceMessageString(objValMsg, "Required", "start date"), date: replaceMessageString(objValMsg, "ValidDate", "Date") },
                ctl00$ContentPlaceHolder1$txtEndDatePurchase: { required: replaceMessageString(objValMsg, "Required", "end date"), date: replaceMessageString(objValMsg, "ValidDate", "Date") }
                },
                errorPlacement: function(error, element) {
                   error.insertAfter(element);
                }
            });

        });

        ////in case remove comment end
        //set link
        $("#<%=lnkBtnReportUserPurchase.ClientID %>").click(function() {
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
                                <asp:TextBox ID="txtStartDatePurchase" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
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
                                <asp:TextBox ID="txtEndDatePurchase" runat="server" CssClass="w_label datepicker min_w"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnReportUserPurchase" class="grey2_btn" runat="server" ToolTip="Report"
                            OnClick="lnkBtnReportUserPurchase_Click"><span>Report</span></asp:LinkButton>
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
                        <div style="text-align: right; color: Red; font-size: larger;" id="totalnumber" runat="server">
                            <span>Total Number of Users: </span>
                            <asp:Label ID="LblTotalUser" runat="server">
                            </asp:Label>
                        </div>
                        <div>
                            <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" AllowSorting="true"
                                OnRowCommand="gv_RowCommand" OnRowDataBound="gv_RowDataBound">
                                <Columns>
                                    <asp:TemplateField Visible="false" HeaderText="Id">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblID" Text='<%# Eval("CompanyName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span class=" first centeralign">
                                                <asp:LinkButton ID="lnkCompanyName" runat="server" CommandArgument="CompanyName"
                                                    CommandName="Sorting">Company Name</asp:LinkButton>
                                            </span>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" CssClass="first centeralign" ID="lblCompanyName" Text='<%# Eval("CompanyName")%>'
                                                ToolTip='<% #Eval("CompanyName")  %>'></asp:Label>
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span class="centeralign">
                                                <asp:LinkButton ID="lnkFirstName" runat="server" CommandArgument="Name" CommandName="Sorting">Name</asp:LinkButton>
                                            </span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFirstName" Text='<%# Eval("Name")%>' CssClass="centeralign"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkDateTime" runat="server" CommandArgument="LoginTime" CommandName="Sorting"><span class="centeralign">Date and Time Accessed</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblLastName" Text='<%# Eval("LoginTime")%>' CssClass="centeralign"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkTotalAccess" runat="server" CommandArgument="LoginCount" CommandName="Sorting"><span class="white_co">TotalAccess</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTotalAccess" Text='<%# Eval("LoginCount")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkStatus" runat="server" CommandArgument="Isupdate" CommandName="Sorting"><span class= "centeralign">Purchase</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space centeralign">
                                                <asp:HiddenField ID="hdnStatus" runat="server" Value='<%# Eval("Isupdate")%>' />
                                                <asp:Image ID="imgYes" runat="server" ImageUrl="~/admin/Incentex_Used_Icons/Yes.png"
                                                    ToolTip="Yes" Height="20" Width="20" />
                                                <asp:Image ID="imgNo" runat="server" ImageUrl="~/admin/Incentex_Used_Icons/No.png"
                                                    ToolTip="No" Height="20" Width="20" />
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span class="centeralign">
                                                <asp:LinkButton ID="lnkMenuBrowserName" runat="server" CommandArgument="Page" CommandName="Sorting">Page</asp:LinkButton>
                                            </span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="centeralign">
                                                <%--<asp:Button ID="btnAddemp" CommandArgument='<%# Eval("UserTrackID") %>' CommandName="BindHistory"
                                                    runat="server" Text="+" Height="26" Width="26"  />--%>
                                                <asp:ImageButton ID="btnAddemp" CommandArgument='<%# Eval("UserTrackID") %>' CommandName="BindHistory"
                                                    runat="server" ImageUrl="~/admin/Incentex_Used_Icons/Plussign.png" Visible="true"  />
                                                    <asp:ImageButton ID="btnminusemp" CommandArgument='<%# Eval("UserTrackID") %>' CommandName="BindHistory"
                                                    runat="server" ImageUrl="~/admin/Incentex_Used_Icons/minussign.png" Visible="false"  />
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="10%" />
                                    </asp:TemplateField>
                                    <%--this is second grid for page history--%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            </td></tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td colspan="4">
                                                    <asp:GridView ID="gvChPageHistory" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content">
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
                                                                    <span class="centeralign">Page Name</span>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblPageName" Text='<% # (Convert.ToString(Eval("PagesName")).Length > 65) ? Eval("PagesName").ToString().Substring(0,65)+"..." : Convert.ToString(Eval("PagesName"))  %>'
                                                                        ToolTip='<% #Eval("PagesName")  %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="b_box" Width="65%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <span class="centeralign">Time Accessed</span>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <span>
                                                                        <%# Eval("DateTimePage") %></span>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="g_box" Width="25%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
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
