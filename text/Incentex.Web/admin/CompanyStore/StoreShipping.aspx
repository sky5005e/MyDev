<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="StoreShipping.aspx.cs" Inherits="admin_CompanyStore_StoreShipping"
    Title="Shipping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
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
            padding-left: 40px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />

    <script type="text/javascript" language="javascript">

        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlShipper: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtProgStartDate: { required: true, validdate: true },
                        ctl00$ContentPlaceHolder1$txtProgEndDate: { required: true, validdate: true }
                        
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlShipper: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "Shipping type") },
                        ctl00$ContentPlaceHolder1$txtProgStartDate:{required: replaceMessageString(objValMsg, "Required", "Start Date"),validdate: "Enter date in mm/dd/yyyy format"},
                        ctl00$ContentPlaceHolder1$txtProgEndDate:{required: replaceMessageString(objValMsg, "Required", "End Date"),validdate: "Enter date in mm/dd/yyyy format"}

                    }

                });
            });

            $("#<%=lnkBtnSaveInformation.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });

            $('#ctl00_ContentPlaceHolder1_chkShippingPercenteageOfSale').click(function() {
                if ($(this).attr("checked") == true) {
                    $("#ctl00_ContentPlaceHolder1_txtMinShipping").rules("add", {
                        number: true,
                        messages: {
                            number: replaceMessageString(objValMsg, "Number", "Minimum Shipping Amount")
                        }
                    });

                    $("#ctl00_ContentPlaceHolder1_txtShippingOfSale").rules("add", {
                        number: true,
                        messages: {
                            number: replaceMessageString(objValMsg, "Number", "% shipping of sale")
                        }
                    });

                    $('#ctl00_ContentPlaceHolder1_dvMinShippingAmount').show();
                    $('#ctl00_ContentPlaceHolder1_dvShippingOfSale').show();
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtMinShipping").rules("remove");
                    $("#ctl00_ContentPlaceHolder1_txtShippingOfSale").rules("remove");
                    $('#ctl00_ContentPlaceHolder1_dvMinShippingAmount').hide();
                    $('#ctl00_ContentPlaceHolder1_dvShippingOfSale').hide();
                }
            });
        });
    </script>

    <div class="alignnone">
        &nbsp;</div>
    <div class="form_pad select_box_pad" style="width: 400px">
        <div class="form_table">
            <table>
                <%--<tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <asp:UpdatePanel ID="upShipping" runat="server">
                                <ContentTemplate>
                                    <div class="form_box">
                                        <label class="dropimg_width400">
                                            <span class="custom-sel">
                                                <asp:DropDownList ID="ddlShipper" onchange="pageLoad(this,value);" runat="server"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </span>
                                        </label>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <table>
                            <%--<tr>
                                <td style="width: 55%;" >
                                    <h4>
                                        Services:
                                    </h4>
                                </td>
                                <td>
                                    <table class="checktable_supplier true">
                                        <tr>
                                            <td>
                                                <asp:DataList ID="dtlShippingType" runat="server">
                                                    <ItemTemplate>
                                                        <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                                                            <asp:CheckBox ID="chkdtlMenus" runat="server" />
                                                        </span>
                                                        <label>
                                                            <asp:Label ID="lblMenus" Text='<%# Eval("sLookupName") %>' runat="server"></asp:Label></label>
                                                        <asp:HiddenField ID="hdnServices" runat="server" Value='<%# Eval("iLookupID") %>' />
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </td>
                                        </tr>
                                        <%--   <tr>
                                            <td>
                                                <span class="custom-checkbox alignleft">
                                                    <input type="checkbox" /></span><label>Ground Service</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="custom-checkbox alignleft">
                                                    <input type="checkbox" /></span><label>Two Day Air</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="custom-checkbox alignleft">
                                                    <input type="checkbox" /></span><label>Overnight</label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                            <tr>
                                <td style="width: 55%;">
                                    <h4>
                                        Shipping as Percentage of Sale:
                                    </h4>
                                </td>
                                <td>
                                    <table class="checktable_supplier true">
                                        <tr>
                                            <td>
                                                <span class="custom-checkbox alignleft" id="spnShip" runat="server">
                                                    <asp:CheckBox ID="chkShippingPercenteageOfSale" runat="server" />
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%--<div >--%>
                            <tr id="dvMinShippingAmount" style="display: none;" runat="server">
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Min Shipping Amount</span>
                                            <asp:TextBox ID="txtMinShipping" CssClass="w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="dvShippingOfSale" style="display: none;" runat="server">
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Shipping % of Sale</span>
                                            <asp:TextBox ID="txtShippingOfSale" runat="server" CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Total Sale Above</span>
                                            <asp:TextBox ID="txtTotalSaleAbove" runat="server" CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 50%;">
                                    <h4>
                                        Exclude From Free Shipping Program
                                    </h4>
                                </td>
                                <td style="width: 50%;">
                                    <table class="checktable_supplier true">
                                        <tr>
                                            <td style="width: 25%;">
                                                <span class="custom-checkbox alignleft" id="Span1" runat="server">
                                                    <asp:CheckBox ID="chkEmployee" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Employee
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="checktable_supplier true">
                                        <tr>
                                            <td>
                                                <span class="custom-checkbox alignleft" id="Span2" runat="server">
                                                    <asp:CheckBox ID="chkAdmin" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Admin
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <asp:UpdatePanel ID="up" runat="server">
                                    <ContentTemplate>
                                        <td colspan="2">
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Free Shipping</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlFreeStatus" runat="server" >
                                                            <asp:ListItem Text="-select status.." Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="calender_l">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Program Start Date</span>
                                            <asp:TextBox ID="txtProgStartDate" CssClass="datepicker w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="calender_l">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Program End Date</span>
                                            <asp:TextBox ID="txtProgEndDate" CssClass="datepicker w_label" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <%--</div>--%>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSaveInformation" class="grey2_btn" runat="server" ToolTip="Save Information"
                            OnClick="lnkBtnSaveInformation_Click"><span>Save Information</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
