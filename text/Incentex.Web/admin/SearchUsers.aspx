<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="SearchUsers.aspx.cs" Inherits="admin_searchusers" Title="Search Users" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script language="javascript" type="text/javascript">
        function clickButton(e, buttonid) {
            var bt = document.getElementById(buttonid);
            if (typeof (bt) == 'object') {
                if (navigator.appName.indexOf("Netscape") > -1) {
                    if (e.keyCode == 13) {
                        if (bt && typeof (bt.click) == 'undefined') {
                            bt.click = addClickFunction1(bt);
                        }
                        else
                            bt.click();
                    }
                }
                if (navigator.appName.indexOf("Microsoft Internet Explorer") > -1) {
                    if (event.keyCode == 13) {
                        bt.click();
                        return false;
                    }
                }
            }
        }

        function addClickFunction1(bt) {
            debugger;
            var result = true;
            if (bt.onclick) result = bt.onclick();
            if (typeof (result) == 'undefined' || result) {
                eval(bt.href);
            }
        }
    </script>

    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:UpdateProgress ID="upPro" AssociatedUpdatePanelID="upMain" runat="server" DisplayAfter="5">
                <ProgressTemplate>
                    <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
                    </div>
                    <div class="updateProgressDiv">
                        <img alt="Loading" runat="server" src="~/Images/ajax-loader-large.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div class="form_pad">
                <div class="search_user_pad">
                    <h4>
                        Search Contacts</h4>
                    <table class="form_table">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upUserType" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">User Type</span> <span class="custom-sel label-sel-small">
                                                    <asp:DropDownList ID="ddlUserType" runat="server" onchange="pageLoad(this,value);"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlUsertType_SelectedIndexChanged"
                                                        TabIndex="0">
                                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Company Name</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlCompanyStore" TabIndex="1" onchange="pageLoad(this,value);"
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
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">First Name</span>
                                        <asp:TextBox ID="txtFirstName" TabIndex="2" runat="server" class="w_label"></asp:TextBox>
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
                                        <span class="input_label">Last Name</span>
                                        <asp:TextBox ID="txtLastname" TabIndex="3" runat="server" class="w_label"></asp:TextBox>
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
                                        <span class="input_label">Employee #</span>
                                        <asp:TextBox ID="txtEmployeeId" TabIndex="4" runat="server" class="w_label"></asp:TextBox>
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
                                        <span class="input_label">Email Address</span>
                                        <asp:TextBox ID="txtEmailAddress" TabIndex="5" runat="server" class="w_label"></asp:TextBox>
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
                                        <span class="input_label">Employee Type</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlEmployeeType" TabIndex="6" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
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
                                        <span class="input_label" style="width: 28%;">Base Station</span> <span class="custom-sel label-sel">
                                            <asp:DropDownList ID="ddlBasestation" TabIndex="7" onchange="pageLoad(this,value);"
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
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 28%;">Workgroup</span> <span class="custom-sel label-sel">
                                            <asp:DropDownList ID="ddlWorkgroup" TabIndex="8" onchange="pageLoad(this,value);"
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
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 28%;">Gender</span> <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlGender" TabIndex="9" runat="server" onchange="pageLoad(this,value);">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="botbtn centeralign">
                    <asp:LinkButton CssClass="grey2_btn" ID="btnSearch" runat="server" TabIndex="10"
                        OnClick="btnSearch_Click"><span>Search</span></asp:LinkButton>
                    <asp:LinkButton CssClass="grey2_btn" ID="btnClear" runat="server" TabIndex="11" OnClick="btnClear_Click"><span>Clear</span></asp:LinkButton>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
