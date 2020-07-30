<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SearchTicketCA.aspx.cs" Inherits="MyAccount_SearchTicketCA" Title="Search Tickets" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .form_box span.error
        {
            margin-left: 0%;
        }
        .select_box_pad
        {
            width: 380px;
        }
        .chkPostedNote
        {
            color: #72757C;
            font-size: 15px;
            line-height: 20px;
            text-align: left !important;
            vertical-align: middle;
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
        $(function() {        
            $("#<%=lnkBtnSearchNow.ClientID %>").click(function() {
                $("#dvLoader").show();
            });
        
            $(window).scroll(function () {
              $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop()); 
            });
            
            $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());
        });
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="form_table">
            <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
            <table class="select_box_pad">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 26%">Ticket Name</span>
                                <asp:TextBox ID="txtServiceTicketName" TabIndex="1" runat="server" CssClass="w_label"></asp:TextBox>
                                <div id="dvTicketName">
                                </div>
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
                                <span class="input_label" style="width: 26%">Ticket #</span>
                                <asp:TextBox ID="txtServiceTicketNumber" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                                <div id="dvTicketNumber">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="custom-sel">
                                <asp:DropDownList ID="ddlWorkGroup" OnSelectedIndexChanged="ddlWorkGroup_SelectedIndexChanged"
                                    TabIndex="3" onchange="pageLoad(this,value);" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </span>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="custom-sel">
                                <asp:DropDownList ID="ddlContactName" TabIndex="3" onchange="pageLoad(this,value);"
                                    runat="server">
                                </asp:DropDownList>
                            </span>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="custom-sel">
                                <asp:DropDownList ID="ddlServiceTicketStatus" TabIndex="4" onchange="pageLoad(this,value);"
                                    runat="server">
                                </asp:DropDownList>
                            </span>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="custom-sel">
                                    <asp:DropDownList ID="ddlServiceTicketOwner" TabIndex="3" onchange="pageLoad(this,value);"
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
                                <span class="input_label" style="width: 26%">Key Word</span>
                                <asp:TextBox ID="txtKeyWord" TabIndex="8" runat="server" CssClass="w_label"></asp:TextBox>
                                <div id="divKeyWord">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkBtnSearchNow" TabIndex="11" class="gredient_btn" runat="server"
                            ToolTip="Search Now" OnClick="lnkBtnSearchNow_Click"><span>Search Now</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
