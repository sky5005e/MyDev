<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ProductReturnLookupSearch.aspx.cs" Inherits="ProductReturnManagement_ProductReturnLookupSearch"
    Title="Product Return >> Lookup Search" %>

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

    <script type="text/javascript" language="javascript">
    $().ready(function() {
        $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
        objValMsg = $.xml2json(xml);
        $("#aspnetForm").validate({
            rules: {
            ctl00$ContentPlaceHolder1$txtEmail: {email: true },
                 ctl00$ContentPlaceHolder1$txtFromDate: {
                    //required: true,
                   // validdate: true,
                   // CompareTo: '{"controltovalidate":"#ctl00_ContentPlaceHolder1_txtToDate","operator":"<="}'
                },
                ctl00$ContentPlaceHolder1$txtToDate: {
                    //required: true,
                    //validdate: true,
                    //CompareTo: '{"controltovalidate":"#ctl00_ContentPlaceHolder1_txtFromDate","operator":">="}'
                }

            },
            messages: {
            ctl00$ContentPlaceHolder1$txtEmail: {
               // required: replaceMessageString(objValMsg, "Required", "email address"),
                email: replaceMessageString(objValMsg, "Valid", "email address")
                
            },
                ctl00$ContentPlaceHolder1$txtFromDate: {
                    //required: "<br/>Please select Ads start Date",
                    //validdate: "<br />Invalid date format.",
                  //  CompareTo: "<br />From date should be less than To date"
                },
                ctl00$ContentPlaceHolder1$txtToDate: {
                    //required: "<br/>Please select Ads End Date",
                    //validdate: "<br />Invalid date format.",
                    //CompareTo: "<br />To date should be greater than From date"
                }
            }

        });
    });

    $("#<%=lnkSubmitRequest.ClientID %>").click(function() {

        return $('#aspnetForm').valid();
    });


});
$(function() {

    $(".datepicker1").datepicker({
        buttonText: 'DatePicker',
        showOn: 'button',
        buttonImage: '../images/calender-icon.jpg',
        buttonImageOnly: true
    });
    //$(".datepicker1").datepicker('setDate', new Date());
});
    </script>

    <script language="javascript" type="text/javascript">
        function searchKeyPress(e)
        {
                // look for window.event in case event isn't passed in
                if (window.event)
                 { e = window.event; }
                if (e.keyCode == 13)
                {
                   
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

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="form_pad">
        <div class="form_table">
            <table class="dropdown_pad ">
                <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label" style="width: 32%">Company Store</span>
                                        <label class="dropimg_width">
                                            <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlCompanyStore" TabIndex="0" onchange="pageLoad(this,value);"
                                                    runat="server" AutoPostBack="true">
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
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 37%;">From Date</span>
                                <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 37%;">To Date</span>
                                <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="cal_w datepicker1 min_w"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
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
                                        <span class="input_label" style="width: 32%">Status</span>
                                        <label class="dropimg_width">
                                            <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlOrderStatus" TabIndex="3" onchange="pageLoad(this,value);"
                                                    runat="server" AutoPostBack="true">
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
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 32%">Order Number</span>
                                <asp:TextBox ID="txtOrderNumber" runat="server" TabIndex="4" MaxLength="20" CssClass="w_label"></asp:TextBox>
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
                                <span class="input_label" style="width: 32%">Employee Email</span>
                                <asp:TextBox ID="txtEmail" runat="server" TabIndex="5" MaxLength="100" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <asp:LinkButton ID="lnkSubmitRequest" class="gredient_btn" TabIndex="6" runat="server"
                            OnClick="lnkSubmitRequest_Click"><span><strong>Search Now</strong></span></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="btn_w_block">
                        <div id="divSqlDataSource">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
