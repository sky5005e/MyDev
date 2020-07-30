<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SearchAnniversaryProgram.aspx.cs" Inherits="MyAccount_CompanyProgram_SearchAnniversaryProgram"
     %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="src" runat="server">
    </at:ToolkitScriptManager>
 <script type="text/javascript" language="javascript">
     function pageLoad(sender, args) {
         {
             assigndesign();
         }
     }
    </script>
    <div class="form_pad">
        <div class="form_table">
            <table class="dropdown_pad">
                <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 32%">Station</span>
                                        <label class="dropimg_width212">
                                            <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlStation" onchange="pageLoad(this,value);" runat="server"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </span>
                                        </label>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 32%">First Name</span>
                                        <asp:TextBox ID="txtFirstName" TabIndex="1" runat="server" class="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 32%">Last Name</span>
                                        <asp:TextBox ID="txtLastName" TabIndex="1" runat="server" class="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 32%">Employee Number</span>
                                        <asp:TextBox ID="txtEmployeeNumber" TabIndex="1" runat="server" class="w_label"></asp:TextBox>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Employee Status</span>
                                <label class="dropimg_width272">
                                   <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="3" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvStatus"></div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                 <tr id="trCreditType" runat="server" >
                    <td>
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 32%">Credit Type</span>
                                        <label class="dropimg_width212">
                                            <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlCreditType" runat="server"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Text="-select-" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Starting Credit" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Anniversary Credit" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </span>
                                        </label>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="botbtn centeralign">
                            <asp:LinkButton CssClass="grey2_btn" ID="btnSearch" OnClick="btnSearch_Click" runat="server"
                                TabIndex="7"><span>Search</span></asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
