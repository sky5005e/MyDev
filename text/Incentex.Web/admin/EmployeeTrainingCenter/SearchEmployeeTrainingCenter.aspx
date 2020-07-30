<%@ Page Title="Employee Training Center" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="SearchEmployeeTrainingCenter.aspx.cs" Inherits="admin_EmployeeTrainingCenter_SearchEmployeeTrainingCenter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad select_box_pad" style="width: 400px;">
        <div class="form_table">
            <div class="spacer10">
            </div>
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <div class="spacer10">
            </div>
            <table>
                <tr id="trVideoType" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                               <span class="input_label" style="width: 30%;">Training Video Type</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlEmployeeTrainingType" onchange="pageLoad(this,value);" runat="server"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trFileName" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">File Name </span>
                                <asp:TextBox ID="txtFileName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trSearchKeyword" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Search Words </span>
                                <asp:TextBox ID="txtSearchKeyword" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                                <div id="dvSearchKeyword">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="tr1" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Uploaded User Name </span>
                                <%--<asp:TextBox ID="txtuploadby" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>--%>
                                <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlIncentexEmp" onchange="pageLoad(this,value);" runat="server"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                </span>
                                <div id="Div1">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkBtnSearch" class="gredient_btn" runat="server" ToolTip="Search Document"
                            OnClick="lnkBtnSearch_Click"><span>Search Now</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
