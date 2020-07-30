<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="SystemAccessCount.aspx.cs" Inherits="TrackingCenter_SystemAccessCount" Title="System Access Count" %>
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
                        <div style="text-align: right; color: Red; font-size: larger;" id="dvtotalnumber"
                            runat="server">
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
                                            <asp:Label runat="server" ID="lblUID" Text='<%# Eval("UserInfoID") %>'></asp:Label>
                                            <%--<asp:Label runat="server" ID="lblUserStatus" Text='<%# Eval("UserStatus") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span class="centeralign">
                                                <asp:LinkButton ID="lnkCompanyName" runat="server" CommandArgument="CompanyName"
                                                    CommandName="Sorting">Company Name</asp:LinkButton>
                                            </span>
                                            <div class="corner">
                                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" CssClass="first" ID="lblCompanyName" Text='<%# Eval("CompanyName")%>'
                                                ToolTip='<% #Eval("CompanyName")  %>'></asp:Label>
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span class=" centeralign">
                                                <asp:LinkButton ID="lnkFirstName" runat="server" CommandArgument="Name" CommandName="Sorting">Name</asp:LinkButton>
                                            </span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdnUserStatus" Value='<%# Eval("UserStatus")%>' />
                                            <asp:Label runat="server" ID="lblFirstName" Text='<%# Eval("Name")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkDateTime" runat="server" CommandArgument="LoginTime" CommandName="Sorting"><span class="centeralign">Date and Time</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblLastLoginTime" Text='<% #Eval("LoginTime") %>' CssClass="centeralign"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkTotalAccess" runat="server" CommandArgument="LoginCount" CommandName="Sorting"><span class="centeralign">Total Access</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTotalAccess" Text='<%# Eval("LoginCount")%>' CssClass="centeralign"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkDummyAddNew" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
                                            <span class="centeralign">Information</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="centeralign">
                                                <asp:HiddenField ID="hdnStatus" runat="server" Value='<%# Eval("UserInfoID")%>' />
                                                <asp:HyperLink ID="imgYes" CommandArgument='<%# Eval("UserInfoID") %>' CommandName="Inform"
                                                    NavigateUrl='<%# "~/TrackingCenter/ViewAccessInfo.aspx?uid=" + Eval("UserInfoID") + "&sdate=" + this.txtStartDate.Text + "&edate=" + this.txtEndDate.Text%>'
                                                    runat="server">Detail</asp:HyperLink></span>
                                            <%-- <asp:ImageButton ID="imgYes" runat="server" Text="Detail" CommandName="Inform" CommandArgument='<%# Eval("UserInfoID") %>'
                                                    ImageUrl="~/Images/button-big.png" ToolTip="Information" Height="30" Width="30"
                                                    class="btn_space"></asp:ImageButton>--%>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span class="centeralign">Status</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblStatus" CssClass="centeralign"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" />
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
        <%--this is for the grid end--%>
    </div>
</asp:Content>

