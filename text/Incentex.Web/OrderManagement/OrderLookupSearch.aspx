<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="OrderLookupSearch.aspx.cs" Inherits="OrderManagement_OrderLookupSearch"
    Title="Order Lookup Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $("#trFromDate").hide();
            $("#trToDate").hide();

            $("#ctl00_ContentPlaceHolder1_ddlOrderPlaced").change(function() {
                if ($("#ctl00_ContentPlaceHolder1_ddlOrderPlaced option:selected").text() == "Custom Date") {
                    $("#trFromDate").show();
                    $("#trToDate").show();
                }
                else {
                    $("#trFromDate").hide();
                    $("#trToDate").hide();
                    $("#ctl00_ContentPlaceHolder1_txtFromDate").val("");
                    $("#ctl00_ContentPlaceHolder1_txtToDate").val("");
                }
            });
        });

        $(function() {
            $(".datepicker1").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
    </script>

    <script language="javascript" type="text/javascript">
        function searchKeyPress(e) {
            // look for window.event in case event isn't passed in
            if (window.event) {
                e = window.event;
            }

            if (e.keyCode == 13) {
                document.getElementById('ctl00_ContentPlaceHolder1_lnkSubmitRequest').click();
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
     <div style="text-align: center; color: Red; font-size: small;">
                <asp:Label ID="lblmsg" runat="server">
                </asp:Label>
            </div>
            <div class="spacer15">
            </div>
        <div class="form_table">
            <table class="dropdown_pad">
                <tr id="trCompanyStore" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Company Store</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlCompanyStore" TabIndex="1" onchange="pageLoad(this,value);"
                                            runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trStationCodeCA" runat="server" visible="false">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Station Code</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlStationCodeCA" TabIndex="1" onchange="pageLoad(this,value);"
                                            runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trOrderPlaced">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Order Placed</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlOrderPlaced" TabIndex="2" onchange="pageLoad(this,value);"
                                            runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trFromDate">
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 37%;">From Date</span>
                                <asp:TextBox ID="txtFromDate" runat="server" TabIndex="3" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trToDate">
                    <td>
                        <div class="calender_l" >
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 37%;">To Date</span>
                                <asp:TextBox ID="txtToDate" runat="server" TabIndex="4" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trOrderStatus">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Order Status</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlOrderStatus" TabIndex="5" onchange="pageLoad(this,value);"
                                            runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trOrderNumber">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Order Number</span>
                                <asp:TextBox ID="txtOrderNumber" runat="server" TabIndex="6" MaxLength="20" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trFirstName">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">First Name</span>
                                <asp:TextBox ID="txtFName" runat="server" MaxLength="20" TabIndex="7" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trLastName">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Last Name</span>
                                <asp:TextBox ID="txtLName" runat="server" MaxLength="20" TabIndex="8" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trEmpNumber">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Employee Number</span>
                                <asp:TextBox ID="txtEmpNumber" runat="server" MaxLength="20" TabIndex="9" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trEmpEmail">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Employee Email</span>
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" TabIndex="10" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trSAPImportStatus" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">SAP Import Status</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlSAPImportStatus" TabIndex="11" onchange="pageLoad(this,value);"
                                            runat="server">
                                            <asp:ListItem Text="-Select-" Value="-1" Selected="True" />
                                            <asp:ListItem Text="Failed" Value="0" />
                                            <asp:ListItem Text="Succeeded" Value="1" />
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trStationCodeIE" runat="server" visible="false">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Station Code</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlStationCodeIE" TabIndex="13" onchange="pageLoad(this,value);"
                                            runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trWorkgruop">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Workgroup</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlWorkgroup" TabIndex="14" onchange="pageLoad(this,value);"
                                            runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trPaymentby">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Payment By</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small">
                                        <asp:DropDownList ID="ddlPaymentOption" TabIndex="15" onchange="pageLoad(this,value);"
                                            runat="server">
                                        </asp:DropDownList>
                                    </span>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trHireDateBefore" runat="server" visible="false">
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 37%;">Hire Date Before</span>
                                <asp:TextBox ID="txtHireDateBefore" runat="server" TabIndex="16" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trSearch">
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkSubmitRequest" class="gredient_btn" runat="server" TabIndex="17"
                            OnClick="lnkSubmitRequest_Click"><span><strong>Run Report</strong></span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%-- </form>--%>
</asp:Content>
