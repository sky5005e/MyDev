<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="PersonalInformation.aspx.cs" Inherits="admin_IncentexEmployee_PersonalInformation"
    Title="Incentex Employee >> Personal Information" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <style type="text/css">
        .form_table span.error
        {
            color: red;
            display: block;
            font-size: 11px;
            margin-bottom: -5px;
            padding-left: 36px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $().ready(function() {

            $("#aspnetForm").validate({
                rules: {
                    ctl00$ContentPlaceHolder1$txtDateIssuedComputer: { validdate: true },
                    ctl00$ContentPlaceHolder1$txtDateIssuedMobile: { validdate: true },
                    ctl00$ContentPlaceHolder1$txtBirthDay: { validdate: true },
                    ctl00$ContentPlaceHolder1$txtMobilePhone: { alphanumeric: true },
                    ctl00$ContentPlaceHolder1$txtMobileNumber: { alphanumeric: true },
                    ctl00$ContentPlaceHolder1$txtSerialNumberComputer:{alphanumeric: true }
                },
                messages:
                    {
                        ctl00$ContentPlaceHolder1$txtDateIssuedComputer:
                        {
                            validdate: "Enter date in mm/dd/yyyy format"
                        },
                        ctl00$ContentPlaceHolder1$txtDateIssuedMobile:
                        {
                            validdate: "Enter date in mm/dd/yyyy format"
                        },
                        ctl00$ContentPlaceHolder1$txtBirthDay:
                        {
                            validdate: "Enter date in mm/dd/yyyy format"
                        },
                        ctl00$ContentPlaceHolder1$txtMobilePhone: 
                        { 
                            alphanumeric: "Enter valid Phone number"
                        },
                         ctl00$ContentPlaceHolder1$txtMobileNumber: 
                        { 
                            alphanumeric: "Enter valid mobile number"
                        },
                        ctl00$ContentPlaceHolder1$txtSerialNumberComputer: 
                        { 
                            alphanumeric: "Enter valid serial number"
                        }
                        
                    }
            });



            $(function() {
                $(".datepicker").datepicker({
                    buttonText: 'DatePicker',
                    showOn: 'button',
                    buttonImage: '../../images/calender-icon.jpg',
                    buttonImageOnly: true
                });
            });
        });
       

    </script>

    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <mb:MenuUserControl ID="menucontrol" runat="server" />
    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <h4>
            Company Equipment Issued</h4>
        <div>
            <table class="form_table">
                <tr>
                    <td colspan="3" style="padding: 0px;">
                        <label class="label">
                            Company Computer</label>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div class="shipmax_in">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label max_w">Company Computer</span>
                                            <asp:TextBox ID="txtCompanyComputer" CssClass="w_label min_w" runat="server"></asp:TextBox>
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
                                            <span class="input_label">Date Issued</span>
                                            <asp:TextBox ID="txtDateIssuedComputer" CssClass="w_cal datepicker" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Computer Brand</span>
                                            <asp:TextBox ID="txtComputerBrand" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd_r">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Serial Number</span>
                                            <asp:TextBox ID="txtSerialNumberComputer" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div class="spacer20">
                &nbsp;</div>
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div class="shipmax_in">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label max_w">Phone</span>
                                            <asp:TextBox ID="txtMobilePhone" CssClass="w_label min_w" runat="server"></asp:TextBox>
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
                                            <span class="input_label">Date Issued</span>
                                            <asp:TextBox ID="txtDateIssuedMobile" CssClass="w_cal datepicker" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Mobile</span>
                                            <asp:TextBox ID="txtMobileNumber" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd_r">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Serial Number</span>
                                            <asp:TextBox ID="txtSerialNumberMobile" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider">
        </div>
        <h4>
            Personal Information</h4>
        <div>
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div class="calender_l">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label max_w">Birthday</span>
                                            <asp:TextBox ID="txtBirthDay" CssClass="w_cal datepicker" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Spouses Name</span>
                                            <asp:TextBox ID="txtSpouseName" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd_r">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Childrens Name</span>
                                            <asp:TextBox ID="txtChildrenName" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="alignnone spacer25">
        </div>
        <div class="botbtn centeralign">
            <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" TabIndex="19" OnClick="lnkSave_Click">
								        <span>Save Information</span>
            </asp:LinkButton>
        </div>
    </div>
</asp:Content>
