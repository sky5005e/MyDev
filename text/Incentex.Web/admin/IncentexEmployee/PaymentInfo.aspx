<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="PaymentInfo.aspx.cs" Inherits="admin_IncentexEmployee_PaymentInfo" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="mb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script language="javascript" type="text/javascript">
$(function()
	{
		//scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");
		//scrolltextarea(".scrollme2", "#Scrolltop2", "#ScrollBottom2");
		scrolltextarea(".scrollme3", "#Scrolltop3", "#ScrollBottom3");
		//scrolltextarea(".scrollme4", "#Scrolltop4", "#ScrollBottom4");
});

$(function() {
    $(".datepicker").datepicker({
        buttonText: 'Date',
        showOn: 'button',
        buttonImage: '../../images/calender-icon.jpg',
        buttonImageOnly: true
    });
});


$().ready(function() {
    $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {

        objValMsg = $.xml2json(xml);
        //alert(objValMsg);

        $("#aspnetForm").validate({
            rules: {
                ctl00$ContentPlaceHolder1$txtHourlyRate: { number: true }
            , ctl00$ContentPlaceHolder1$txtWeeklySalary: { number: true }
            , ctl00$ContentPlaceHolder1$txtMonthlySalary: { number: true }
            , ctl00$ContentPlaceHolder1$txtCommissionRate: { number: true }
            , ctl00$ContentPlaceHolder1$txtDateHired: { date: true }
            }
            , messages:
            {
                ctl00$ContentPlaceHolder1$txtHourlyRate: {
                    number: replaceMessageString(objValMsg, "Number", "")

                }
                , ctl00$ContentPlaceHolder1$txtWeeklySalary: {
                    number: replaceMessageString(objValMsg, "Number", "")
                }
                , ctl00$ContentPlaceHolder1$txtMonthlySalary: {
                    number: replaceMessageString(objValMsg, "Number", "")
                }
                , ctl00$ContentPlaceHolder1$txtCommissionRate: {
                    number: replaceMessageString(objValMsg, "Number", "")
                }
                , ctl00$ContentPlaceHolder1$txtDateHired: {
                date: replaceMessageString(objValMsg, "ValidDate", "") 
                    }
            }
        });
    });

    $("#<%=lnkSave.ClientID %>").click(function() {
        return $('#aspnetForm').valid();
    }); //click

});        //ready

</script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<asp:ScriptManager ID="sc1" runat="server">
</asp:ScriptManager>

                <mb:MenuUserControl ID="manuControl" runat="server" />


    <div class="form_pad">
     <div style="text-align:center" >
           <asp:Label ID="lblMsg" runat="server" CssClass="errormessage" ></asp:Label>
                </div>
        <h4>
            Benefits</h4>
        <div>
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Hourly Rate</span>
                                            <%--<input type="text" class="w_label" />--%>
                                            <asp:TextBox ID="txtHourlyRate" runat="server" CssClass="w_label" TabIndex="1"></asp:TextBox>
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
                                            <span class="input_label">Commission Rate</span>
                                            <asp:TextBox ID="txtCommissionRate" runat="server" CssClass="w_label" 
                                                TabIndex="4"></asp:TextBox>
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
                                            <span class="input_label">Weekly Salary</span>
                                            <asp:TextBox ID="txtWeeklySalary" runat="server" CssClass="w_label" 
                                                TabIndex="2"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="shipmax_in">
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box commission_text clearfix">
                                            <span class="input_label alignleft">Commission Details</span>
                                            <div class="textarea_box alignright">
                                                <div class="scrollbar">
                                                    <a href="#scroll" id="Scrolltop3" class="scrolltop"></a><a href="#scroll" id="ScrollBottom3"
                                                        class="scrollbottom"></a>
                                                </div>
                                                <%--<textarea name="" cols="" rows="" class="scrollme3"></textarea>--%>
                                                <asp:TextBox ID="txtCommissionDetails" runat="server" CssClass="scrollme3"
                                                    TextMode="MultiLine" Rows="3" TabIndex="5"
                                                ></asp:TextBox>
                                            </div>
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
                                            <span class="input_label">Monthly Salary</span>
                                            <asp:TextBox ID="txtMonthlySalary" runat="server" CssClass="w_label" 
                                                TabIndex="3"></asp:TextBox>
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
                                            <span class="input_label">Date Hired</span>
                                            <asp:TextBox ID="txtDateHired" runat="server" CssClass="w_cal datepicker" 
                                                TabIndex="6"></asp:TextBox>
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
            Employee Benefits</h4>
        <div>
            <table>
                <tr>
                    <td>
                        <asp:DataList ID="lstEmployeeBenefits" runat="server" 
                        RepeatLayout="Table" 
                        RepeatColumns="3" 
                        RepeatDirection="Horizontal" 
                            ondatabinding="lstEmployeeBenefits_DataBinding"   >
                      
                        <ItemTemplate>
                            <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft" id="spChk" runat="server">
                                     
                                        <asp:CheckBox id="chk" runat="server" />
                                        </span>
                                        <label>
                                        <%#Eval("sLookupName") %>
                                        </label>
                                        <asp:HiddenField ID="hdnId" runat="server" Value=<%#Eval("iLookupID") %> />
                                </td>
                            </tr>
                            </table>
                        </ItemTemplate>
                        <ItemStyle Width="30%" />
                    </asp:DataList>
                    </td>
                </tr>
              <%--  <tr>
                    <td class="formtd">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft">
                                        <input type="checkbox" /></span><label>Health Insurance</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft">
                                        <input type="checkbox" /></span><label>Flexible Work Hours</label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft">
                                        <input type="checkbox" /></span><label>Company Vehicle</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft">
                                        <input type="checkbox" /></span><label>Home Internet Service</label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd_r">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft">
                                        <input type="checkbox" /></span><label>Corporate Gas Card</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft">
                                        <input type="checkbox" /></span><label>Home Phone Services</label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>--%>
            </table>
        </div>
        <div class="divider">
        </div>
        <h4>
            Pay Periods</h4>
        <div>
            <table>
            <tr>
                    <td>
                        <asp:DataList ID="lstPayPeriods" runat="server" 
                        RepeatLayout="Table" 
                        RepeatColumns="3" 
                        RepeatDirection="Horizontal" ondatabinding="lstPayPeriods_DataBinding" 
                     
                      >
                        <ItemTemplate>
                            <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft" id="spChk" runat="server">
                                     
                                        <asp:CheckBox id="chk" runat="server" />
                                        </span>
                                        <label>
                                        <%#Eval("sLookupName") %>
                                        </label>
                                        <asp:HiddenField ID="hdnId"
                                            Value=<%#Eval("iLookupID") %>
                                         runat="server"  />
                                </td>
                            </tr>
                            </table>
                        </ItemTemplate>
                        <ItemStyle Width="30%" />
                    </asp:DataList>
                    </td>
                </tr>
           <%--     <tr>
                    <td class="formtd">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft">
                                        <input type="checkbox" /></span><label>Paid Weekly</label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft">
                                        <input type="checkbox" /></span><label>Paid Bi-Monthly</label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="formtd_r">
                        <table class="checktable_supplier true">
                            <tr>
                                <td>
                                    <span class="custom-checkbox alignleft">
                                        <input type="checkbox" /></span><label>Paid Monthly</label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="checktable_supplier true">
                    <td colspan="3">
                        <span class="custom-checkbox alignleft">
                            <input type="checkbox" /></span><label>Paid Montly After We have Received Payment from
                                Customer</label>
                    </td>
                </tr>--%>
            </table>
        </div>
        <div class="alignnone spacer25">
        </div>
        <div class="additional_btn">
            <ul class="clearfix">
                <li>
                    <%--<a href="#" title="Save Information" class="grey2_btn"><span>Save Information</span></a>--%>
                    <asp:LinkButton ID="lnkSave" runat="server" CssClass="grey2_btn" 
                        onclick="lnkSave_Click">
								        <span>Save Information</span>
                    </asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
