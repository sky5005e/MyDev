<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ProductItemsReturnSearch.aspx.cs" Inherits="ProductReturnManagement_ProductItemsReturnSearch"
    Title="Product Return >> Lookup Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<style type="text/css">
        .form_box .custom-sel span.error
        {
            padding: 24px 0;
        }
        .form_table td
        {
            padding-bottom: 5px;
        }
        .label-sel
        {
            width: 64%;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();
        }
        function searchKeyPress(e) {
            if (event.which || event.keyCode) {
                if ((event.which == 13) || (event.keyCode == 13)) {
                    document.getElementById('ctl00_ContentPlaceHolder1_lnkSubmitRequest').click();
                }
            }
        }
//            alert("hi");
//            // look for window.event in case event isn't passed in
//            if (typeof e == 'undefined' && window.event) { e = window.event; }
//            if (e.keyCode == 13) {
//                document.getElementById('lnkSubmitRequest').click();
//            }
//        }
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
   
  
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Panel id="pnl" runat="server" defaultbutton="lnkSubmitRequest">
    <div class="form_pad">
        <div class="form_table">
            <table class="dropdown_pad ">
                <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 30%">Enter RA Number</span>
                            <asp:TextBox ID="txtRANumber" runat="server" TabIndex="1" CssClass="w_label"></asp:TextBox>
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
                            <span class="input_label" style="width: 30%;">First Name</span>
                            <asp:TextBox ID="txtFirstName" runat="server" TabIndex="2" CssClass="w_label"></asp:TextBox>
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
                            <span class="input_label" style="width: 30%;">Last Name</span>
                            <asp:TextBox ID="txtLastName" runat="server" TabIndex="3" CssClass="w_label"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                 <tr runat="server" id="trDateRange">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%">Date Range :</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlDateRange" onchange="pageLoad(this,value);" runat="server" TabIndex="4"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlDateRange_SelectedIndexChanged">
                                        <asp:ListItem Text="-select-" Value="select"></asp:ListItem>
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
                    <td class="btn_w_block">
                    <asp:TextBox ID="txt" runat="server" style="visibility:hidden;"></asp:TextBox>
                        <asp:LinkButton ID="lnkSubmitRequest" class="gredient_btn" TabIndex="7" runat="server"
                            OnClick="lnkSubmitRequest_Click" onkeypress="searchKeyPress(event);"><span><strong>Search Now</strong></span></asp:LinkButton>
                    </td>
                </tr>
               
            </table>
        </div>
    </div>
    </asp:Panel>
</asp:Content>
