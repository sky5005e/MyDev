<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SpecialProgram.aspx.cs" Inherits="admin_Company_Employee_SpecialProgram2"
    Title="Company Employee >> Special Program" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function FeatureNotActive() {
            jAlert("Currently this feature is not active..", "Feature Not Active", function(RetVal) {
                if (RetVal) {

                }
            });
        }
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <mb:MenuUserControl ID="menucontrol" runat="server" />

    <script type="text/javascript" language="javascript">
        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                buttonImage: '../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules:
                {
                //ctl00$ContentPlaceHolder1$txtCurrentCreditAmount: { required: true },
                //ctl00$ContentPlaceHolder1$txtCreditAppliedOn: { required: true }
            },
                    messages: {
                    /*
                    ctl00$ContentPlaceHolder1$txtCurrentCreditAmount: {
                    required: replaceMessageString(objValMsg, "Required", "credit amount")
                    },
                    ctl00$ContentPlaceHolder1$txtCreditAppliedOn: {
                    required: replaceMessageString(objValMsg, "Required", "credit applied on")
                    }*/
                },
                errorPlacement: function(error, element) {
                    if (element.attr("name") == "ctl00$ContentPlaceHolder1$hdnProductStatus")
                        error.insertAfter("#divProductStatus");
                    else if (element.attr("name") == "ctl00$ContentPlaceHolder1$hdnRank")
                        error.insertAfter("#divRank");
                    else
                        error.insertAfter(element);
                }
            });
                       //Add nagmani
           $('#ProductStatus option').each(function(i) {
               if ($("#ctl00_ContentPlaceHolder1_hdnProductStatus").val() == $(this).val()) {
                    $(this).attr("selected", "selected");
                     $("#ProductStatus").msDropDown({ mainCSS: 'dd2' });
              }

          });
        $('#ProductStatus').change(function() {

            $("#ctl00_ContentPlaceHolder1_hdnProductStatus").val($(this).val());
            return $('#ctl00_ContentPlaceHolder1_hdnProductStatus').valid();
        });

         $('#Rank option').each(function(i) {
            if ($("#ctl00_ContentPlaceHolder1_hdnRank").val() == $(this).val()) {
                     $(this).attr("selected", "selected");
                     $("#Rank").msDropDown({ mainCSS: 'dd2' });
             }

         });
         $('#Rank').change(function() {
              $("#ctl00_ContentPlaceHolder1_hdnRank").val($(this).val());
              return $('#ctl00_ContentPlaceHolder1_hdnRank').valid();
         });
         

        });

        if ($("#ctl00_ContentPlaceHolder1_txtCurrentCreditAmount").val()) {
            
//            $('#ctl00_ContentPlaceHolder1_chkCreditProgram').attr("checked", "true");
//            $('#menuspan').attr("class", "custom-checkbox_checked");
            $('#anneversarycredit').show();
            
        }
        else 
        {
            

                //$('#ctl00_ContentPlaceHolder1_chkCreditProgram').attr("checked", "false");
                $('#anneversarycredit').hide();
 
        }

        //Add nagmani
        if ($("#ctl00_ContentPlaceHolder1_hdnProductStatus").val() != "0") {

            $('#issuance').show();
        }
        else {

            $('#issuance').hide();

        }

        //End nagmani



        $('#ctl00_ContentPlaceHolder1_chkCreditProgram').click(function() {

            if ($(this).attr("checked") == true) {
                $("#ctl00_ContentPlaceHolder1_txtCurrentCreditAmount").rules("add", {
                    required: true,
                    number: true,
                    messages: {
                        required: replaceMessageString(objValMsg, "Required", "credit amount"),
                        number: replaceMessageString(objValMsg, "Number", "credit amount")
                    }

                });

              

                $('#menuspan').attr("class", "custom-checkbox_checked");
                $('#anneversarycredit').show();
                

            }
            else {
                $('#anneversarycredit').hide();
                $("#ctl00_ContentPlaceHolder1_txtCurrentCreditAmount").rules("remove");
              
            }
        });

        $('#ctl00_ContentPlaceHolder1_chkUniformIssuance').click(function() {
            if ($(this).attr("checked") == true) {
                $(this).attr("class", "custom-checkbox_checked");
                
                $('#menuspan1').attr("class", "custom-checkbox_checked");
                $('#issuance').show();



            }
            else {
              
                $('#issuance').hide();

            }
            $('#ProductStatus option').each(function(i) {
                if ($("#ctl00_ContentPlaceHolder1_hdnProductStatus").val() == $(this).val()) {
                    $(this).attr("selected", "selected");
                    $("#ProductStatus").msDropDown({ mainCSS: 'dd2' });
                }

            });
            $('#ProductStatus').change(function() {

                $("#ctl00_ContentPlaceHolder1_hdnProductStatus").val($(this).val());
                return $('#ctl00_ContentPlaceHolder1_hdnProductStatus').valid();
            });


//Add nagmani


            $('#Rank option').each(function(i) {
            if ($("#ctl00_ContentPlaceHolder1_hdnRank").val() == $(this).val()) {
                    $(this).attr("selected", "selected");
                    $("#Rank").msDropDown({ mainCSS: 'dd2' });
                }

            });
            
            $('#Rank').change(function() {

            $("#ctl00_ContentPlaceHolder1_hdnRank").val($(this).val());
            return $('#ctl00_ContentPlaceHolder1_hdnRank').valid();
            });
        });
        
        

        $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
            return $('#aspnetForm').valid();
        });
    });

    </script>

    <script type="text/javascript" language="javascript">
            function calculateSum()
	        {
	           
	             var creditamounttoexpire = 0.00;
	             var startingcreditamount = parseFloat($('#ctl00_ContentPlaceHolder1_txtCurrentCreditAmount').val());
	             if(isNaN(startingcreditamount))
	             {
	                startingcreditamount = 0.00;
	             }
	             
				 var credtiamounttoapplied = parseFloat($("#ctl00_ContentPlaceHolder1_hdnCreditAmtToBeApplied").val());
				 
				 if(isNaN(credtiamounttoapplied))
				 {
				    credtiamounttoapplied = 0.00;
				 }
	            
				 creditamounttoexpire = parseFloat(startingcreditamount + credtiamounttoapplied);
				 
				 //alert(creditamounttoexpire);
				 $('#ctl00_ContentPlaceHolder1_lblCreditAmtExpiresOn').val("0.00");
				 $('#ctl00_ContentPlaceHolder1_lblCreditAmtExpiresOn').val(creditamounttoexpire);
			}
			
    </script>

    <div class="spacer10">
    </div>
    <div class="tabcontent" id="special_programs">
        <div class="alignnone">
            &nbsp;</div>
        <div class="form_pad">
            <div class="employee_name">
                User Name:
                <asp:Label ID="lblUserFullName" runat="server" Text=""></asp:Label>
            </div>
            <h4>
                Anniversary Credit Program</h4>
            <div class="form_table">
                <div class="spacer20">
                </div>
                <div class="checktable_supplier true clearfix">
                    <span class="custom-checkbox alignleft" id="menuspan" runat="server">
                        <asp:CheckBox ID="chkCreditProgram" runat="server" />
                    </span>
                    <label>
                        Anniversary Credit Program</label>
                </div>
                <div class="spacer25">
                </div>
                <div id="anneversarycredit" style="display: none;">
                    <div class="checktable_supplier true clearfix">
                        <asp:LinkButton ID="lnkTransactionLog" CssClass="greysm_btn" runat="server" OnClick="lnkTransactionLog_Click"><span style="color:White;"> 
                                View Transaction Log</span></asp:LinkButton>
                    </div>
                    <table>
                        <tr>
                            <td class="formtd">
                                <table class="max_label_width">
                                    <tr>
                                        <td>
                                            <div>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Starting Credit Amount ($)</span>
                                                    <asp:TextBox ID="txtCurrentCreditAmount" TabIndex="1" CssClass="w_label" runat="server"
                                                        onchange="javascript:calculateSum();"></asp:TextBox>
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
                                                            <span class="input_label">Starting Credit Amount to Expire (Date)</span>
                                                            <asp:Label ID="lblCreditExpiresOn" runat="server"></asp:Label>
                                                            <asp:HiddenField ID="hdnCreditExpiresOn" runat="server" />
                                                        </div>
                                                        <div class="form_bot_co">
                                                            <span>&nbsp;</span></div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="formtd">
                                <table class="max_label_width">
                                    <tr>
                                        <td>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <span class="input_label">Next Credit Appliend on (Date)</span>
                                                <asp:Label ID="lblNextCreditAppliedOn" CssClass="w_label" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdnNextCreditAppliedOn" runat="server" />
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
                                                    <span class="input_label">Credit Amount to be applied ($ value)</span>
                                                    <asp:Label ID="lblCreditAmountAppliedTo" CssClass="w_label" runat="server"></asp:Label>
                                                    <asp:HiddenField ID="hdnCreditAmtToBeApplied" runat="server" />
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="formtd"">
                                <table class="max_label_width">
                                    <tr>
                                        <td>
                                            <%--  <div class="calender_l">--%>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <%--<span class="input_label">Credit Amount to expire ($ credit value)</span>--%>
                                                <span class="input_label">Total Credit Balance </span>
                                                <asp:TextBox ID="lblCreditAmtExpiresOn" runat="server" CssClass="w_label"></asp:TextBox>
                                                <asp:HiddenField ID="hdnCreditAmtExpiresOn" runat="server" />
                                                <%--<asp:TextBox ID="txtCreditExpiresOn" CssClass="datepicker w_label_cal" runat="server"></asp:TextBox>--%>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                            <%-- </div>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="divider">
            </div>
            <h4>
                Uniform Issuance Program</h4>
            <div class="spacer20">
            </div>
            <div class="checktable_supplier true clearfix">
                <span class="custom-checkbox alignleft" id="menuspan1" runat="server">
                    <asp:CheckBox ID="chkUniformIssuance" runat="server" />
                </span>
                <label>
                    Uniform Issuance Program</label>
            </div>
            <div class="spacer25">
            </div>
            <div id="issuance" style="display: none;">
                <table class="form_table">
                    <tr>
                        <td class="formtd">
                            <table class="max_label_width">
                                <tr>
                                    <td>
                                        <div runat="server" id="divRank">
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box">
                                                <label class="dropimg_width386">
                                                    <span class="form_box status_select label-sel-small">
                                                        <select id="Rank" name="Rank">
                                                            <option value="0">..Select Rank..</option>
                                                            <%
                                                                LookupDA sRank = new LookupDA();
                                                                LookupBE sRankBE = new LookupBE();
                                                                sRankBE.SOperation = "selectall";
                                                                sRankBE.iLookupCode = "Rank";

                                                                DataSet dsRank = sRank.LookUp(sRankBE);
                                                                for (int i = 0; i < dsRank.Tables[0].Rows.Count; i++)
                                                                {
                                                                    string path = "../../admin/Incentex_Used_Icons/" + dsRank.Tables[0].Rows[i]["sLookupIcon"].ToString();
                                                              
                                                                        
                                                            %>
                                                            <option value="<%=dsRank.Tables[0].Rows[i]["iLookupID"]%>" title="<%=path%>">
                                                                <%=dsRank.Tables[0].Rows[i]["sLookupName"].ToString()%>
                                                            </option>
                                                            <%
                                                                }

                                                            %>
                                                        </select>
                                                    </span>
                                                    <div id="divRank">
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
                                                <label class="dropimg_width386">
                                                    <span class="form_box status_select label-sel-small">
                                                        <select id="ProductStatus" name="ProductStatus">
                                                            <option value="0">..Employee Issuance Policy Status..</option>
                                                            <%
                                                                LookupDA sEmployeeStatus = new LookupDA();
                                                                LookupBE sEmployeeStatusBE = new LookupBE();
                                                                sEmployeeStatusBE.SOperation = "selectall";
                                                                sEmployeeStatusBE.iLookupCode = "Status";

                                                                DataSet dsEmplStatus = sEmployeeStatus.LookUp(sEmployeeStatusBE);
                                                                for (int i = 0; i < dsEmplStatus.Tables[0].Rows.Count; i++)
                                                                {
                                                                    string path = "../../admin/Incentex_Used_Icons/" + dsEmplStatus.Tables[0].Rows[i]["sLookupIcon"].ToString();
                                                              
                                                                        
                                                            %>
                                                            <option value="<%=dsEmplStatus.Tables[0].Rows[i]["iLookupID"]%>" title="<%=path%>">
                                                                <%=dsEmplStatus.Tables[0].Rows[i]["sLookupName"].ToString()%>
                                                            </option>
                                                            <%
                                                                }

                                                            %>
                                                        </select>
                                                    </span>
                                                    <div id="divProductStatus">
                                                    </div>
                                                </label>
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
            <div class="spacer25">
            </div>
            <div class="botbtn centeralign">
                <asp:LinkButton ID="lnkBtnSaveInfo" OnClick="lnkBtnSaveInfo_Click" class="grey2_btn"
                    runat="server" ToolTip="Save Information"><span>Save Information</span></asp:LinkButton>
            </div>
        </div>
    </div>
    <input id="hdnProductStatus" type="hidden" value="0" runat="server" />
    <input id="hdnRank" type="hidden" value="0" runat="server" />
    <input id="hdnStartingCredit" type="hidden" value="" runat="server" />
</asp:Content>
