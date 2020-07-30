<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IPWizardSetup.aspx.cs" Inherits="NewDesign_UserFrameItems_IPWizardSetup"
    Title="Untitled Page" %>

<%@ Register TagPrefix="uc" TagName="CommonHeader" Src="~/NewDesign/UserControl/NewCommonHeader.ascx" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <uc:CommonHeader ID="ucCommonHead" runat="server" />
    <title>Wizard Steps</title>

    <script type="text/javascript">
        $(document).ready(function() {
            $('input').iCheck({
                checkboxClass: 'icheckbox_flat',
                radioClass: 'iradio_flat'
            });
           
            // Allow to change month and year
           $(".setDatePicker").datepicker({
                changeMonth: true,
                changeYear: true
           });
          
              $('input:checkbox').on('ifChecked', function (event) {
                var idcls = $(this).attr("id");
                //show
                $("." + idcls).show();
            });
            
            $('input:checkbox').on('ifUnchecked', function (event) {
                var idcls = $(this).attr("id");
                //show
                $("." + idcls).hide();
            });
             $('input:radio').on('ifChecked', function (event) {
                var idcls = $(this).attr("id").replace("WizardIP_rdb", "");
                 var thisName = $(this).attr('name');
               $('input:radio[name="' + thisName + '"]').parent('div').removeClass("error");
                //show
                $("." + idcls).show();
            });
            
            $('input:radio').on('ifUnchecked', function (event) {
                var idcls = $(this).attr("id").replace("WizardIP_rdb", "");
                //hide
                $("." + idcls).hide();
            });
            // for 8th steps
           if($('input:radio').is(':checked') == true)
           {
             var gpname = 'WizardIP$PolicyActivation';
             var divshow = $('input:radio[name="' + gpname + '"]:checked', '#WizardIP').attr("id").replace("WizardIP_rdb", "");
             $("." + divshow).show();
            }
             $(".dvScroll").niceScroll(
	            {
	                touchbehavior: false,
	                cursorcolor: "#666",
	                cursoropacitymax: 0.7,
	                cursorwidth: 6,
	                cursorborder: "1px solid #666",
	                cursorborderradius: "8px",
	                autohidemode: "scroll"
	            });
        });
       
    </script>

    <script type="text/javascript">
     function validateCommon(source, e) {
     var sc = $(source).attr('class');
     var gruopName ='WizardIP$'+ sc;
      if ($('input:radio[name="' + gruopName + '"]').is(':checked') == true) {
            e.IsValid = true;
            $('input:radio[name="' + gruopName + '"]').parent('div').removeClass("error");
        }
        else {
            e.IsValid = false;
              $('input:radio[name="' + gruopName + '"]').parent('div').addClass('error');
            }     
       }
           
   function validateCommonPnl2(source, e) {
     var sc = $(source).attr('class');
     var gruopName ='WizardIP$'+ sc;
     var IsgridChkbox = $('.dvScroll').find('input:checkbox').is(':checked');
     if(IsgridChkbox)
     {
      if ($('input:radio[name="' + gruopName + '"]').is(':checked') == true) {
            e.IsValid = true;
            $('input:radio[name="' + gruopName + '"]').parent('div').removeClass("error");
        }
        else {
            e.IsValid = false;
              $('input:radio[name="' + gruopName + '"]').parent('div').addClass('error');
            }     
        }
       else{
        e.IsValid = true;
       }
    }
     //for Validate Months ReActivate Steps 9
     function ValidateMonthsReActivate(source, e) {
     if($("#<%=rdbPolicyExpireReactivateBeforeMonth.ClientID %>").is(':checked') == true){
        ValidatorEnabler(document.getElementById('<%= rqddlPolicyReactivateMonthNo.ClientID %>'), true);
          if($("#<%=ddlPolicyReactivateMonthNo.ClientID %>").val() !='0'){
            e.IsValid = true;
           }
           else {
            e.IsValid = false;
           }
      }
    }
        
     //for Policy Activation Radio Button DOH
     function ValidatePolicyActivationDOH(source, e) {
        if($("#<%=rdbPolicyActivationDOH.ClientID %>").is(':checked') == true){
        var DOH = 'WizardIP$DOH';//$('input:radio').attr('name');
        if ($('input:radio[name="' + DOH + '"]').is(':checked') == true) {
            e.IsValid = true;
             $('input:radio[name="' + DOH + '"]').parent('div').removeClass("error");
            // For 1
             if($("#<%=rdbPolicyActivationUOM.ClientID %>").is(':checked') == true){
                ValidatorEnabler(document.getElementById('<%= rqddlmonth.ClientID %>'), true);
                }
        }
        else {
              e.IsValid = false;
              $('input:radio[name="' + DOH + '"]').parent('div').addClass("error");
              ValidatorEnabler(document.getElementById('<%= rqddlmonth.ClientID %>'), false);
             
            }
        }
     }
     // for Text Date 
     function ValidatePolicyActivationDate(source, e) {
      if($("#<%=rdbPolicyActivationDate.ClientID %>").is(':checked') == true){
            ValidatorEnabler(document.getElementById('<%= rqtxtDateFrom.ClientID %>'), true);
            ValidatorEnabler(document.getElementById('<%= rqtxtDateTo.ClientID %>'), true);
            if($("#<%=txtDateFrom.ClientID %>").val().length > 0 && $("#<%=txtDateTo.ClientID %>").val().length > 0 )
            {
                e.IsValid = true;
            }
            else
            {
                e.IsValid = false;
                $("#<%=txtDateFrom.ClientID %>").addClass("ErrorField");
                $("#<%=txtDateTo.ClientID %>").addClass("ErrorField");
            }
        }
     }
     
     function ValidatePolicyExpire(source, e) {
        var PolicyExpire = $('input:radio').attr('name');
        if ($('input:radio[name="' + PolicyExpire + '"]').is(':checked') == true) {
            e.IsValid = true;
            $('input:radio[name="' + PolicyExpire + '"]').parent('div').removeClass("error");
        }
        else {
            e.IsValid = false;
              $('input:radio[name="' + PolicyExpire + '"]').parent('div').addClass('error');
            }
         }
        function ValidatePolicyActivationPreviusIssuance(source, e) {
          if($("#<%=rdbPolicyActivationPreviusIssuance.ClientID %>").is(':checked') == true){
             var PreviusIssuance = 'WizardIP$PolicyActivationPreviusIssuance';
            if ($('input:radio[name="' + PreviusIssuance + '"]').is(':checked') == true) {
                
               
                $('input:radio[name="' + PreviusIssuance + '"]').parent('div').removeClass("error");
                 if($("#<%=rdbPolicyActivationMonth1.ClientID %>").is(':checked') == true){
                 ValidatorEnabler(document.getElementById('<%= rqddlPolicyActivationMonth1.ClientID %>'), true);
                 }
                 else if ($("#<%=rdbPolicyActivationMonth2.ClientID %>").is(':checked') == true){
                  ValidatorEnabler(document.getElementById('<%= rqddlPolicyActivationMonth2.ClientID %>'), true);
                  }
                  else  if($("#<%=rdbPolicyActivationMonth3.ClientID %>").is(':checked') == true){
                   ValidatorEnabler(document.getElementById('<%= rqddlPolicyActivationMonth3.ClientID %>'), true);
                   }
                   if($("#<%=ddlPolicyActivationMonth1.ClientID %>").val() !='0' || $("#<%=ddlPolicyActivationMonth2.ClientID %>").val() !='0' || $("#<%=ddlPolicyActivationMonth3.ClientID %>").val()!='0'){
                    e.IsValid = true;
                   }
                   else {
                    e.IsValid = false;
                   }
                
            }
            else {
                e.IsValid = false;
                  $('input:radio[name="' + PreviusIssuance + '"]').parent('div').addClass('error');
                }
            } 
        }
         
        function ValidatorEnabler(validator, enable) {
            validator.enabled = enable;
            ValidatorUpdateDisplay(validator);
        }
      
      function closePop(){
            $('#dvItemsDetails').hide().removeClass("TopActive");
            $('.fade-layer').hide();
       }
      function displaypopup2(){
            $('#dvItemsDetails').show().addClass("TopActive");
            $('.fade-layer').show();
        }
    </script>

    <script type="text/javascript">
        jQuery(document).ready(function() {
            jQuery('#mycarousel').jcarousel({ scroll: 1 });
        });
        
    </script>

    <script type="text/javascript">
        $(document).ready(function() {
            $(".cart-slider-display .image-block").hoverIntent(function() {
                $(".cart-caption", this).animate({ bottom: 0 }, { duration: 400 });
            }, function() {
                $(".cart-caption", this).animate({ bottom: -68 }, { duration: 400 });
            });
        });
    </script>

</head>
<body class="NoClass">
    <span id="fade-layer" style="display: none;" class="fade-layer small-popup-fade">
    </span>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="pnlPart1wizard" runat="server" Visible="false">
            <asp:Wizard ID="WizardIP" runat="server" Width="843px" DisplaySideBar="False" StepPreviousButtonStyle-CssClass="btn-blue-n btn-Pre"
                StepNextButtonStyle-CssClass="btn-blue-n btn-Next" StartNextButtonStyle-CssClass="btn-blue-n btn-Next"
                FinishCompleteButtonStyle-CssClass="btn-blue-n btn-Next" FinishPreviousButtonStyle-CssClass="btn-blue-n btn-Pre"
                OnFinishButtonClick="WizardIP_FinishButtonClick" OnNextButtonClick="WizardIP_NextButtonClick">
                <WizardSteps>
                    <asp:WizardStep ID="WizardStep1" runat="server" Title="Company">
                        <div>
                            <h3>
                                <em>1.</em><span>Please choose the company this Issuance Policy is for from the list
                                    below – select one.</span></h3>
                        </div>
                        <div class="select-policy">
                            <asp:DropDownList ID="ddlCompany" runat="server" class="default" TabIndex="1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqCompany" runat="server" ControlToValidate="ddlCompany"
                                Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep2" runat="server" Title="Gender">
                        <div>
                            <h3>
                                <em>2.</em><span> Please choose the gender for your Issuance Policy – if you choose
                                    Unisex the policy will show up for both males and females.</span></h3>
                        </div>
                        <div class="select-policy">
                            <asp:DropDownList ID="ddlGender" runat="server" class="default" TabIndex="1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqGender" runat="server" ControlToValidate="ddlGender"
                                Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep3" runat="server" Title="Workgroup">
                        <div>
                            <h3>
                                <em>3.</em><span>Please choose the workgroup this policy will be for from the selection
                                    below – select one.</span></h3>
                        </div>
                        <div class="select-policy">
                            <asp:DropDownList ID="ddlWorkGroup" runat="server" class="default" TabIndex="1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqWorkGroup" runat="server" ControlToValidate="ddlWorkGroup"
                                Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep4" runat="server" Title="Issuance Types">
                        <div>
                            <h3>
                                <em>4.</em><span>Please select what kind of items will be in your Issuance – you will
                                    have 3 choices listed below.</span></h3>
                        </div>
                        <div class="select-policy">
                            <asp:RadioButtonList ID="rdbCategoryList" runat="server" Width="100%">
                                <asp:ListItem>Supply Items</asp:ListItem>
                                <asp:ListItem Selected="True">Uniform Items</asp:ListItem>
                                <asp:ListItem>Both Supply and Uniform Items</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="rqCategoryList" runat="server" ControlToValidate="rdbCategoryList"
                                Display="Dynamic" CssClass="error" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep5" runat="server" Title="Climate">
                        <div>
                            <h3>
                                <em>5.</em><span>Please choose the climate for this Issuance – you may choose more than
                                    one climate – the climates chosen will make the policy show up for employees at
                                    the base stations with that climate selected.</span></h3>
                        </div>
                        <div class="select-policy">
                            <asp:RadioButtonList ID="rdbClimateList" runat="server" Width="100%">
                            </asp:RadioButtonList>
                            <asp:CustomValidator ID="cvClimate" runat="server" ClientValidationFunction="validateCommon"
                                CssClass="rdbClimateList"></asp:CustomValidator>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep6" runat="server" Title="Pricing">
                        <div>
                            <h3>
                                <em>6.</em><span>Now you will select the pricing for your Issuance Policy. Please choose
                                    from the list below</span></h3>
                        </div>
                        <div class="select-policy">
                            <asp:RadioButtonList ID="rdbPricingList" runat="server" Width="100%">
                            </asp:RadioButtonList>
                            <%-- <asp:CustomValidator ID="cvPricing" runat="server" ClientValidationFunction="validateCommon"
                                CssClass="rdbPricingList"></asp:CustomValidator>--%>
                        </div>
                        <div class="select-policy">
                            <h3>
                                Would you like to show pricing to the employee</h3>
                            <asp:RadioButtonList ID="rdbShowPricingList" runat="server" Width="100%">
                            </asp:RadioButtonList>
                            <asp:CustomValidator ID="cvShowPricing" runat="server" ClientValidationFunction="validateCommon"
                                CssClass="rdbShowPricingList"></asp:CustomValidator>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep7" runat="server" Title="Complete Purchase ">
                        <div>
                            <h3>
                                <em>7.</em><span> Please select the purchase requirements from below. You will have
                                    2 choices – please select one.</span></h3>
                        </div>
                        <div class="select-policy">
                            <label class="label_radio">
                                <asp:RadioButton ID="rdbComplete" runat="server" GroupName="CompletePurchase" class="iradio_flat" />Complete
                                Purchase - this means the employee must order everything in their issuance policy
                                at one time to use the Issuance
                            </label>
                            <label class="label_radio">
                                <asp:RadioButton ID="rdbPartial" runat="server" GroupName="CompletePurchase" class="iradio_flat" />Partial
                                Purchase – this means the employee is allowed to place several orders until they
                                have purchased everything available in their issuance policy. If you choose Partial
                                Purchase the issuance will remain open until they have completed all their purchases
                                – we recommend choosing Complete Purchase.</label>
                            <asp:CustomValidator ID="cvCompletePurchase" runat="server" ClientValidationFunction="validateCommon"
                                CssClass="CompletePurchase"></asp:CustomValidator>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep8" runat="server" Title="Policy Activation">
                        <div>
                            <h3>
                                <em>8.</em><span> Now you will choose the activation period for this Issuance Policy
                                    – please choose one of the 3 options listed below.</span></h3>
                        </div>
                        <div class="wizard8 select-policy">
                            <label class="label_radio">
                                <asp:RadioButton ID="rdbPolicyActivationDOH" runat="server" GroupName="PolicyActivation"
                                    class="iradio_flat" />Date of Hire – the policy will activate based on the employee’s
                                Date of Hire.</label>
                            <br />
                            <div class="PolicyActivationDOH row" style="display: none">
                                <div class="row mrg-l15">
                                    <label class="label_radio">
                                        <asp:RadioButton ID="rdbPolicyActivationUOH" runat="server" GroupName="DOH" class="iradio_flat" />Activate
                                        upon Hire</label></div>
                                <div class="row mrg-l15">
                                    <label class="label_radio">
                                        <asp:RadioButton ID="rdbPolicyActivationUOM" runat="server" GroupName="DOH" class="iradio_flat" />Activate
                                        <span class="table-drop">
                                            <asp:DropDownList ID="ddlmonth" runat="server" class="default" TabIndex="12">
                                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                                <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                                <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                                <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                                <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                                <asp:ListItem Value="24" Text="24"></asp:ListItem>
                                                <asp:ListItem Value="27" Text="27"></asp:ListItem>
                                                <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                                <asp:ListItem Value="33" Text="33"></asp:ListItem>
                                                <asp:ListItem Value="36" Text="36"></asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                    </label>
                                    <asp:CustomValidator ID="cvPolicyActivationDOH" runat="server" ClientValidationFunction="ValidatePolicyActivationDOH"
                                        CssClass="DOH"></asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="rqddlmonth" runat="server" ControlToValidate="ddlmonth"
                                        Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row-2">
                                <label class="label_radio">
                                    <asp:RadioButton ID="rdbPolicyActivationDate" runat="server" GroupName="PolicyActivation"
                                        class="iradio_flat" />Chosen Date – the policy will activate based on a specific
                                    date chosen in the calendar drop down and will expire on another specific date chosen
                                    in the drop down calendar</label>
                                <div class="PolicyActivationDate mrg-l15" style="display: none">
                                    <span class="lable-txt">From:</span>
                                    <asp:TextBox ID="txtDateFrom" runat="server" class="input-field-all setDatePicker"
                                        TabIndex="11"></asp:TextBox>
                                    <span class="lable-txt">To:</span>
                                    <asp:TextBox ID="txtDateTo" runat="server" class="input-field-all setDatePicker"
                                        TabIndex="16"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqtxtDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                        Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rqtxtDateTo" runat="server" ControlToValidate="txtDateTo"
                                        Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvPolicyActivationDate" runat="server" ClientValidationFunction="ValidatePolicyActivationDate"></asp:CustomValidator>
                                </div>
                            </div>
                            <label class="label_radio">
                                <asp:RadioButton ID="rdbPolicyActivationPreviusIssuance" runat="server" GroupName="PolicyActivation"
                                    class="iradio_flat" />Previous Issuance Policy – the policy will activate based
                                on the last Issuance policy the employee received.</label>
                            <div class="PolicyActivationPreviusIssuance row mrg-l25" style="display: none">
                                <label class="label_radio">
                                    <asp:RadioButton ID="rdbPolicyActivationMonth1" runat="server" GroupName="PolicyActivationPreviusIssuance"
                                        class="iradio_flat" />Policy will activate of months after the previous policy’s
                                    activation date
                                </label>
                                <span class="lbl-txt">Policy will activate of months after the previous policy’s expiration
                                    datePolicy will activate of months after the previous policy’s Issuance order ship
                                    datePolicy will activate based on the previous policy’s activation date <span class="table-drop">
                                        <asp:DropDownList ID="ddlPolicyActivationMonth1" runat="server" class="default" TabIndex="12">
                                            <asp:ListItem Value="0" Text="select"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                            <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                            <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                            <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                            <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                            <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                            <asp:ListItem Value="24" Text="24"></asp:ListItem>
                                            <asp:ListItem Value="27" Text="27"></asp:ListItem>
                                            <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                            <asp:ListItem Value="33" Text="33"></asp:ListItem>
                                            <asp:ListItem Value="36" Text="36"></asp:ListItem>
                                        </asp:DropDownList>
                                    </span>-select one</span>
                                <label class="label_radio">
                                    <asp:RadioButton ID="rdbPolicyActivationMonth2" runat="server" GroupName="PolicyActivationPreviusIssuance"
                                        class="iradio_flat" />
                                    Policy will activate based on the previous policy’s expiration date
                                </label>
                                <span class="lbl-txt">Policy will activateof months after the previous policy’s expiration
                                    date <span class="table-drop">
                                        <asp:DropDownList ID="ddlPolicyActivationMonth2" runat="server" class="default" TabIndex="12">
                                            <asp:ListItem Value="0" Text="select"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                            <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                            <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                            <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                            <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                            <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                            <asp:ListItem Value="24" Text="24"></asp:ListItem>
                                            <asp:ListItem Value="27" Text="27"></asp:ListItem>
                                            <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                            <asp:ListItem Value="33" Text="33"></asp:ListItem>
                                            <asp:ListItem Value="36" Text="36"></asp:ListItem>
                                        </asp:DropDownList>
                                    </span>-select one.</span>
                                <label class="label_radio">
                                    <asp:RadioButton ID="rdbPolicyActivationMonth3" runat="server" GroupName="PolicyActivationPreviusIssuance"
                                        class="iradio_flat" />
                                    Policy will activate based on the previous policy’s Issuance order ship date (this
                                    will be recorded as the date the order was completely shipped).
                                </label>
                                <span class="lbl-txt">Policy will activate of months after the previous policy’s Issuance
                                    order ship date <span class="table-drop">
                                        <asp:DropDownList ID="ddlPolicyActivationMonth3" runat="server" class="default" TabIndex="12">
                                            <asp:ListItem Value="0" Text="select"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                            <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                            <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                            <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                            <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                            <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                            <asp:ListItem Value="24" Text="24"></asp:ListItem>
                                            <asp:ListItem Value="27" Text="27"></asp:ListItem>
                                            <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                            <asp:ListItem Value="33" Text="33"></asp:ListItem>
                                            <asp:ListItem Value="36" Text="36"></asp:ListItem>
                                        </asp:DropDownList>
                                    </span>-select one.</span>
                                <asp:RequiredFieldValidator ID="rqddlPolicyActivationMonth1" runat="server" ControlToValidate="ddlPolicyActivationMonth1"
                                    Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rqddlPolicyActivationMonth2" runat="server" ControlToValidate="ddlPolicyActivationMonth2"
                                    Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rqddlPolicyActivationMonth3" runat="server" ControlToValidate="ddlPolicyActivationMonth3"
                                    Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvPolicyActivationPreviusIssuance" runat="server" ClientValidationFunction="ValidatePolicyActivationPreviusIssuance"
                                    CssClass="PolicyActivationPreviusIssuance"></asp:CustomValidator>
                            </div>
                            <asp:CustomValidator ID="cvPolicyActivation" runat="server" ClientValidationFunction="validateCommon"
                                CssClass="PolicyActivation"></asp:CustomValidator>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep9" runat="server" Title="Policy Expire">
                        <div>
                            <h3>
                                <em>9.</em><span> To set when the policy will expire and the employee will no longer
                                    be able to order from it – please select one from the choices below.</span>
                            </h3>
                        </div>
                        <div class="WizardStep9 select-policy">
                            <span class="lbl-txt">The policy will expire of months from the date the policy was
                                activated.</span> <span class="table-drop">
                                    <asp:DropDownList ID="ddlExpiremonthNumber" runat="server" class="default" TabIndex="12">
                                        <asp:ListItem Value="0" Text="select"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                        <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                        <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                        <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                        <asp:ListItem Value="24" Text="24"></asp:ListItem>
                                        <asp:ListItem Value="27" Text="27"></asp:ListItem>
                                        <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                        <asp:ListItem Value="33" Text="33"></asp:ListItem>
                                        <asp:ListItem Value="36" Text="36"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            <label class="label_radio">
                                <asp:RadioButton ID="rdbPolicyExpireFinal" runat="server" GroupName="PolicyExpire"
                                    class="iradio_flat" />Do you want this policy to expire and not be available
                                to that employee again?
                            </label>
                            <label class="label_radio">
                                <asp:RadioButton ID="rdbPolicyExpireReactivateBeforeTime" runat="server" GroupName="PolicyExpire"
                                    class="iradio_flat" />
                                Do you want this policy to re-activate and be active for the same time period as
                                before?
                            </label>
                            <label class="label_radio">
                                <asp:RadioButton ID="rdbPolicyExpireReactivateBeforeMonth" runat="server" GroupName="PolicyExpire"
                                    class="iradio_flat" />
                                Do you want this policy to re-activate and be active for of months before expiring
                                and re-activating. <span class="table-drop">
                                    <asp:DropDownList ID="ddlPolicyReactivateMonthNo" runat="server" class="default"
                                        TabIndex="12">
                                        <asp:ListItem Value="0" Text="select"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="1"> </asp:ListItem>
                                        <asp:ListItem Value="2" Text="2"> </asp:ListItem>
                                        <asp:ListItem Value="3" Text="3"> </asp:ListItem>
                                        <asp:ListItem Value="4" Text="4"> </asp:ListItem>
                                        <asp:ListItem Value="5" Text="5"> </asp:ListItem>
                                        <asp:ListItem Value="6" Text="6"> </asp:ListItem>
                                        <asp:ListItem Value="7" Text="7"> </asp:ListItem>
                                        <asp:ListItem Value="8" Text="8"> </asp:ListItem>
                                        <asp:ListItem Value="9" Text="9"> </asp:ListItem>
                                        <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                        <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </label>
                            <div class="row">
                                <asp:RequiredFieldValidator ID="rqddlExpiremonthNumber" runat="server" ControlToValidate="ddlExpiremonthNumber"
                                    Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" Enabled="true"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rqddlPolicyReactivateMonthNo" runat="server" ControlToValidate="ddlPolicyReactivateMonthNo"
                                    Display="Dynamic" CssClass="error" SetFocusOnError="True" InitialValue="0" Enabled="false"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvPolicyExpire" runat="server" ClientValidationFunction="validateCommon"
                                    CssClass="PolicyExpire"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvMonthsReActivate" runat="server" ClientValidationFunction="ValidateMonthsReActivate"
                                    CssClass="PolicyExpire"></asp:CustomValidator>
                            </div>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep10" runat="server" Title="Policy Name">
                        <div>
                            <h3>
                                <em>10.</em><span> Congratulations! You have now created the perimeters of this Issuance
                                    Policy – please name the policy and select submit. Then we will add the uniform
                                    and/or supply items to your Issuance Policy. Please make the name descriptive of
                                    the policy – for example: Male New Hire Ramp Agent is a good descriptive name.</span>
                            </h3>
                        </div>
                        <div class="select-policy">
                            <asp:TextBox ID="txtIssuancePolicyName" runat="server" class="input-field-all"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rqtxtIssuancePolicyName" runat="server" ControlToValidate="txtIssuancePolicyName"
                            Display="Dynamic" CssClass="error" SetFocusOnError="True" Enabled="true"></asp:RequiredFieldValidator>
                    </asp:WizardStep>
                </WizardSteps>
            </asp:Wizard>
        </asp:Panel>
        <asp:Panel ID="pnlPart2Item" CssClass="WizardIP" runat="server" Visible="false">
            <div>
                <h3>
                    In this section you will select how many items of one type an employee can purchase
                    and then you will select what items they can choose from. For example: they may
                    be able to purchase 5 shirts – but they can select from 3 different colors and long
                    sleeve or short sleeve.
                </h3>
                <div class="row">
                    <label class="lbl-txt">
                        First you need to choose the style - uniform dress shirts, or uniform pants</label>
                    <div class="select-policy alignleft">
                        <span class="table-drop">
                            <asp:DropDownList ID="ddlProductStyle" runat="server" class="default" TabIndex="1"
                                OnSelectedIndexChanged="ddlProductStyle_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </span>
                    </div>
                </div>
                <p class="lbl-txt">
                    Choose a number of items and whether there is a choice of any combination or each.</p>
                <div class="row">
                    <label class="lbl-txt">
                        ADD</label>
                    <div class="select-policy alignleft">
                        <span class="table-drop">
                            <asp:DropDownList ID="ddlItemQty" runat="server" class="default" TabIndex="1">
                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                <asp:ListItem Value="10" Text="10"></asp:ListItem>
                            </asp:DropDownList>
                        </span>
                    </div>
                    <label class="lbl-txt">
                        of</label>
                    <span class="table-drop">
                        <asp:DropDownList ID="ddlItemOfQty" runat="server" class="default alignleft" TabIndex="1">
                            <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                            <asp:ListItem Value="5" Text="5"></asp:ListItem>
                            <asp:ListItem Value="6" Text="6"></asp:ListItem>
                            <asp:ListItem Value="7" Text="7"></asp:ListItem>
                            <asp:ListItem Value="8" Text="8"></asp:ListItem>
                            <asp:ListItem Value="9" Text="9"></asp:ListItem>
                            <asp:ListItem Value="10" Text="10"></asp:ListItem>
                        </asp:DropDownList>
                    </span>
                </div>
                <asp:RequiredFieldValidator ID="rqItemQty" runat="server" ControlToValidate="ddlItemQty"
                    Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rqItemOfQty" runat="server" ControlToValidate="ddlItemOfQty"
                    Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>
            </div>
            <div class="employee-payment preview cf" style="display: block">
                <div class="dvScroll" style="height: 210px;">
                    <div class="warranty-desc-table">
                        <asp:GridView ID="gvItemsInfo" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Item#</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemNumber" runat="server" Text='<%#Eval("MasterItem") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Summary Description</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSummaryDesc" runat="server" Text='<%#Eval("ProductDescription") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Add</HeaderTemplate>
                                    <ItemTemplate>
                                        <a href="javascript: void(0);" class="small-blue-btn" title="ADD" onclick="displaypopup2();"
                                            id='<%# Eval("StoreProductID") %>'><span>Add</span></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Select</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkItem" runat="server"></asp:CheckBox></ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div>
                <asp:LinkButton ID="btnAdd" runat="server" class="blue-btn small" OnClick="btnAdd_Click">Add</asp:LinkButton>
            </div>
            <div id="dvItemsDetails" class="small-popup" style="display: none">
                <a href="#" class="close-btn" onclick="closePop();">Close</a>
                <div class="small-popup-inner">
                    <h3>
                        1. Would you like to select any base stations to include? This will override the
                        climate setting for this item.</h3>
                    <div class="table-drop cart-piece">
                        <asp:DropDownList ID="ddlItemBaseStation" runat="server" class="default" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <span class="row">a. Do you want to exclude this station from all other policies with
                        this item that are associated with this workgroup?</span>
                    <div class="row">
                        <asp:RadioButtonList ID="rdbExcludedBaseStation" runat="server" Width="100%">
                        </asp:RadioButtonList>
                        <asp:CustomValidator ID="cvrdbExcludedBaseStation" runat="server" ClientValidationFunction="validateCommonPnl2"
                            CssClass="rdbExcludedBaseStation"></asp:CustomValidator>
                    </div>
                    <h3 class="row">
                        2. Would you like to add a corporate credit for this item?</h3>
                    <div class="row">
                        <span class="alignleft lbl-txt">a. Corporate credit is</span>
                        <asp:TextBox ID="txtCorporateCreditPercent" runat="server" class="input-field-small input-55 alignleft"></asp:TextBox>
                        <span class="alignleft lbl-txt">% of the price of the item.</span>
                    </div>
                    <div class="row">
                        <span class="alignleft lbl-txt">b. Corporate credit is $</span>
                        <asp:TextBox ID="txtCorporateCreditAmount" runat="server" class="input-field-small input-55"></asp:TextBox>
                        <span class="alignleft lbl-txt">of the price of the item.</span>
                    </div>
                    <div class="row">
                        3. Would you like this item to be made available to only one employee type? By selecting
                        the employee type – this item will only show up for employees in that employee type.
                        Please select the correct type below.
                    </div>
                    <div class="row">
                        <asp:RadioButtonList ID="rdbEmployeeType" runat="server" Width="100%">
                        </asp:RadioButtonList>
                        <asp:CustomValidator ID="cvrdbEmployeeType" runat="server" ClientValidationFunction="validateCommonPnl2"
                            CssClass="rdbEmployeeType"></asp:CustomValidator>
                    </div>
                </div>
                <div>
                    <a href="#" class="small-blue-btn alignright" tabindex="23" id="btnadditem" onclick="closePop();">
                        <span>Add</span></a>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlPartItemSelect" CssClass="WizardIP" runat="server" Visible="false">
            <div class="cart-content">
                <%-- START Binding IP product items--%>
                <asp:Repeater ID="rptIPItemList" runat="server" OnItemDataBound="rptIPItemList_ItemDataBound">
                    <HeaderTemplate>
                        <div class="cart-slider-display">
                            <ul id="mycarousel" class="jcarousel-skin-tango">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li id="liRepeater" runat="server">
                            <div class="image-block">
                                <a href="#" class="pro-img">
                                    <img id="imgProduct" runat="server" src="../StaticContents/img/product-img1.jpg"
                                        width="236" height="315" title='<%# Eval("Summary")%>' alt='<%# Eval("Summary")%>' /></a>
                                <asp:HiddenField ID="hdnStoreProductID" runat="server" Value='<%#Eval("StoreProductID")%>' />
                                <asp:HiddenField ID="hdnMasterItemID" runat="server" Value='<%#Eval("MasterItemID")%>' />
                                <asp:HiddenField ID="hdnProductImage" runat="server" Value='<%#Eval("ProductImage")%>' />
                            </div>
                            <div class="cart-description">
                                <div class="cart-price cf">
                                    <label class="alignleft">
                                        <asp:CheckBox ID="chkAddItem" runat="server" />
                                    </label>
                                    <asp:Label ID="lblsummary" runat="server" Text='<%# Convert.ToString(Eval("ProductDescription")).Length>50 ? Convert.ToString(Eval("ProductDescription")).Substring(0,50) : Eval("ProductDescription")  %>'
                                        ToolTip='<%# Eval("ProductDescription")%>'></asp:Label>
                                </div>
                            </div>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul> </div>
                    </FooterTemplate>
                </asp:Repeater>
                <%-- END Binding IP product items--%>
                <div class="pop-btn-block">
                    <asp:LinkButton ID="btnAddItemtoIP" runat="server" class="blue-btn" OnClick="btnAddItemtoIP_Click">Add Items to Policy</asp:LinkButton></div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlPart3Preview" CssClass="WizardIP" runat="server" Visible="false">
            <div>
                <h3>
                    Each part of the preview has an edit button on the right that you can select and
                    make changes to any part of the issuance.</h3>
                <ul class="ul-listing">
                    <li><em>
                        <asp:Label ID="lblIssuancePolicyName" runat="server"></asp:Label>
                    </em><a href="#" class="small-blue-btn" tabindex="23"><span>Edit</span></a></li>
                    <li><em>Gender</em> <a href="#" class="small-blue-btn" tabindex="23"><span>Edit</span></a></li>
                    <li><em>Workgroup</em> <a href="#" class="small-blue-btn" tabindex="23"><span>Edit</span></a></li>
                    <li><em>Climate</em> <a href="#" class="small-blue-btn" tabindex="23"><span>Edit</span></a></li>
                    <li><em>Pricing</em> <a href="#" class="small-blue-btn" tabindex="23"><span>Edit</span></a></li>
                    <li><em>Purchase Requirements</em> <a href="#" class="small-blue-btn" tabindex="23">
                        <span>Edit</span></a></li>
                    <li><em>Activation</em> <a href="#" class="small-blue-btn" tabindex="23"><span>Edit</span></a></li>
                    <li><em>Expiration</em> <a href="#" class="small-blue-btn" tabindex="23"><span>Edit</span></a></li>
                </ul>
            </div>
            <div>
                <asp:LinkButton ID="btnPeviewNext" runat="server" class="blue-btn small" OnClick="btnPeviewNext_Click">Next</asp:LinkButton>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlPart4Preview" CssClass="WizardIP" runat="server" Visible="false">
            <div>
            <div class="header">
                <h3>This page will show you all of the selections that have been made for the Issuance Policy</h3>
            </div>
            <div class="dvScroll" style="height: 350px;">
                <asp:Repeater ID="rptPolicyPreviewItem" runat="server" OnItemDataBound="rptPolicyPreviewItem_ItemDataBound" OnItemCommand="rptPolicyPreviewItem_ItemCommand">
                    <HeaderTemplate>
                        <ul class="cart-listing cf">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li><a href="#" class="image-block">
                            <img id="imgIPItem" runat="server" src="../StaticContents/img/product-img1.jpg"
                                         width="90" height="120" title='<%# Eval("Summary")%>' alt='<%# Eval("Summary")%>' /></a>
                                <asp:HiddenField ID="hdnItemStoreProductID" runat="server" Value='<%#Eval("StoreProductID")%>' />
                                <asp:HiddenField ID="hdnItemProductImage" runat="server" Value='<%#Eval("ProductImage")%>' />
                            <div class="cart-desc">
                                <div class="product-name">
                                    <%# Eval("ProductDescription")%>
                                    <span class="pro-qty">QTY <%# Eval("IssuanceQty")%></span>
                                </div>
                                
                                <asp:LinkButton ID="lnkbtnRemoveItem" runat="server" class="remove-btn" CommandArgument='<%# Eval("UniformIssuancePolicyItemID") %>' CommandName="Remove">remove</asp:LinkButton>
                            </div>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul> </div>
                    </FooterTemplate>
                </asp:Repeater>
                </div>
              <div style="height: 60px;">
            <div>
                    <a href="#" class="small-blue-btn alignright" tabindex="23" id="A1">
                        <span>Publish</span></a>
                        <a href="#" class="small-blue-btn alignright" tabindex="23" id="A2">
                        <span>Save</span></a>
                        <a href="#" class="small-blue-btn alignright" tabindex="23" id="A3">
                        <span>DeActivate</span></a>
                </div>
            </div>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
