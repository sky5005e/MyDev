<%@ Page Title="Sessions Search" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="SessionsSearch.aspx.cs" Inherits="TrackingCenter_SessionsSearch" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $().ready(function() {
            $(".action").click(function() {
                $('#dvLoader').show();
            });
        });
    </script>
    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
    </script>
    <script type="text/javascript" language="javascript">
        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
      
    
    </script>
   <script type="text/javascript">
       function PlaySession(siteid) {
           window.open(siteid, 'playvideo', 'width=420,height=500 ,scrollbars=yes');
       }
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
        <div class="user_manage_btn btn_width_small">
         <div class="form_table">
            <table class="dropdown_pad ">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Company</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvCompany">
                                    </div>
                                </label>
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
                                <span class="input_label" style="width: 32%">Employee</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlEmployee" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="DivEmpList">
                                    </div>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trDateRange">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Date Range :</span> <span class="custom-sel label-sel-small">
                                    <asp:DropDownList ID="ddlDateRange" onchange="pageLoad(this,value);" runat="server" TabIndex="4"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlDateRange_SelectedIndexChanged">
                                        <asp:ListItem Text="Today" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Yesterday" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Last 3 Days" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Last 7 Days" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="Last 30 Days" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="Date Range" Value="Range"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trFromDate" visible="false">
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%;">From Date</span>
                                <asp:TextBox ID="txtFromDate" runat="server" TabIndex="5" CssClass="cal_w datepicker1"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trToDate" visible="false">
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%;">To Date</span>
                                <asp:TextBox ID="txtToDate" runat="server" TabIndex="6" CssClass="cal_w datepicker1"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                  
                    <td class="aligncenter">
                        <asp:LinkButton ID="lnkViewSessions" CssClass="gredient_btn action" runat="server" 
                        ToolTip="Submit" OnClick="lnkViewSessions_Click"><span>View Sessions</span>
                        </asp:LinkButton>
                    </td>
                   
              
                </tr>
            </table>
            </div>
        </div>
    </div>
</asp:Content>


