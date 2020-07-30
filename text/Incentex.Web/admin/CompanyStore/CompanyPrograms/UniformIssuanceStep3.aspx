<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UniformIssuanceStep3.aspx.cs" Inherits="admin_CompanyStore_CompanyPrograms_UniformIssuanceStep1"
    %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script language="javascript" type="text/javascript">
     // change value in custom dropdown
     function pageLoad(sender, args) {
         {
             assigndesign();
         }
     }

     // set date image and select date
 $(function() {
            $(".datepicker").datepicker({
                buttonText: 'Date',
                showOn: 'button',
                buttonImage: '../../../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });

        //validations

        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {

                objValMsg = $.xml2json(xml);
                //alert(objValMsg);

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$txtEligibleDate: { date: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$txtEligibleDate: { date: replaceMessageString(objValMsg, "ValidDate", "") }
                    }


                }); //validate
            }); //get


            $("#<%=lnkNext.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            }); //click

            $("#ctl00_ContentPlaceHolder1_chkDateOfHire").click(ShowHideDate);

            ShowHideDate();
        });       //ready

        function ShowHideDate() 
        {
        if ($("#ctl00_ContentPlaceHolder1_chkDateOfHire").is(':checked')) 
            {
                //alert("checked")
                $("#h4Or").hide();
                //show months
                $("#divEligibleMonth").show();
                
                    //hide date
                    $("#divEligibleDate").hide();
                    $("#ctl00_ContentPlaceHolder1_txtEligibleDate").val("");
                    
                    
                }
                else {
                    //alert("not checked")
                    
                    //hide months
                    $("#divEligibleMonth").hide();
                    
                    //show date
                    //$("#h4Or").show();
                    $("#divEligibleDate").show();
                }
        }
        

      
        
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    
    <div class="form_pad">
      <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            <br />
        </div>
        <div class="uniform_pad uniform_pad_w">
            <h4>
                Please select the starting reference point from which you will measure the time
                for when this/these items will be next issued.</h4>
            <div>
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle uniform_check_point">
                    <ul class="true">
                        <li><span class="alignright custom-checkbox" id="spChkDateOfHire" runat="server">
                            <%--<input type="checkbox" />--%>
                            <asp:CheckBox ID="chkDateOfHire" runat="server" />
                            </span>Date of Hire
                            <br class="alignnone" />
                        </li>
                    </ul>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="spacer20">
            </div>
            <h4>
                Now please select the number of months from your reference point that the employee
                will be eligible for a new issuance of this item:</h4>
            <div id="divEligibleMonth" >
                <div class="tab_content_top_co">
                    <span>&nbsp;</span></div>
                <div class="tab_content form_table">
                    <div class="form_top_co">
                        <span>&nbsp;</span></div>
                    <div class="form_box">
                        <span class="custom-sel">
                           <%-- <select>
                                <option>Number of Months</option>
                                <option>1</option>
                                <option>2</option>
                            </select>--%>
                            <asp:DropDownList ID="ddlMonths" runat="server">
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="form_bot_co">
                        <span>&nbsp;</span></div>
                </div>
                <div class="tab_content_bot_co">
                    <span>&nbsp;</span></div>
            </div>
                 <h4 id="h4Or">   
                    Or 
                </h4>
          
            <div id="divEligibleDate">
                <div class="tab_content_top_co">
                    <span>&nbsp;</span></div>
                <div class="tab_content form_table">
                    
                     <div class="calender_l" >
						    <div class="form_top_co"><span>&nbsp;</span></div>
						    <div class="form_box supplier_annual_date">
							    <span class="input_label">Eligible Date</span>
    							
					    <asp:TextBox ID="txtEligibleDate" runat="server" 
                            CssClass="w_label datepicker min_w" ></asp:TextBox>		
						    </div>
						    <div class="form_bot_co"><span>&nbsp;</span></div>
					    </div>
                   
                </div>
                <div class="tab_content_bot_co">
                    <span>&nbsp;</span></div>

                 
									
            </div>
        </div>
        <div class="spacer25">
        </div>
        <div class="bot_alert next">
        
            <%--<asp:HyperLink runat="server" ID="lnkNext"  NavigateUrl="UniformIssuanceStep4.aspx" title="Next Step 4">Next Step 4</asp:HyperLink>--%>
            <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click">Next Step 5</asp:LinkButton>
                </div>
        <div class="bot_alert prev">
        <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
        <%--    <asp:HyperLink  NavigateUrl="UniformIssuanceStep2.aspx" 
                ID="lnkPrev" runat="server">Prev Step 2</asp:HyperLink>--%>
                </div>
    </div>
</asp:Content>
