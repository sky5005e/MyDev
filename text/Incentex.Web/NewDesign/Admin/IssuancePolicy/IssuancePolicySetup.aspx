<%@ Page Language="C#" MasterPageFile="~/NewDesign/Admin/AdminMaster.master" AutoEventWireup="true"
    CodeFile="IssuancePolicySetup.aspx.cs" Inherits="NewDesign_Admin_IssuancePolicy_IssuancePolicySetup"
    Title="Issuance Policy Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
     function validateCommon(source, e) {
     var sc = $(source).attr('class');
     var gruopName = 'ctl00$ContentPlaceHolder1$'+sc;
     alert(gruopName);
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
     var gruopName = sc;
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
     
        
     //for Policy Activation Radio Button DOH
     function ValidatePolicyActivationDOH(source, e) {
        if($("#<%=rdbPolicyActivationDOH.ClientID %>").is(':checked') == true){
        var DOH = $("#<%=rdbPolicyActivationDOH.ClientID %>").attr('name');
        alert(DOH);
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
            if($("#<%=txtDateFrom.ClientID %>").val().length > 0 )
            {
                e.IsValid = true;
            }
            else
            {
                e.IsValid = false;
                $("#<%=txtDateFrom.ClientID %>").addClass("ErrorField");
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
        $(document).ready(function() {

         // Allow to change month and year
           $(".setDatePicker").datepicker({
                changeMonth: true,
                changeYear: true
           });
     var objsteps = ['step-one', 'step-two', 'step-three', 'step-four', 'step-five','step-six', 'step-seven','step-eight'];
       var max = objsteps.length;
 
        // Get current div
        var curVl =  $("#<%=hdnCurrentStep.ClientID %>").val();
//        // Find ID in image src
//        var curID = curVl;

//        var curIdx = 0;
        $("div#IssuanceSetup > div").hide();
           // now set step one to display
           $("."+ curVl).show();
           $("#btn-div").show();
          // for steps changes
          $('.tabs li a').on('click', function(e)
          {
           var aid = $(this).attr("id");
           $("div#IssuanceSetup > div").hide();
           $("#btn-div").show();
           $("." + aid).show();
           $("#<%=hdnCurrentStep.ClientID %>").val(aid);
           $('.tabs li').removeClass('active');
           $('#'+ aid).parent().addClass('active');
           //alert(jQuery.inArray(aid, objsteps));
           //alert($("#<%=hdnCurrentStep.ClientID %>").val());
           curIdx = jQuery.inArray(aid, objsteps);
          
          });
          
        if($("div#IssuanceSetup > div").css('display') == 'block')
        {
            curVl = $("div#IssuanceSetup > div").attr('class');
            
            //alert(curVl);
        }
   

        // Search index of current ID
        while (curIdx < max) {
            if (objsteps[curIdx] == curID) {
                break;
            }
            curIdx++;
        }

   
    
        // Next image on button (and image) click
        $('#IssuanceSetup a.btn-danger').click( function() {
            curIdx = (curIdx+1) % max;
            $("div#IssuanceSetup > div").hide();
            $('.tabs li').removeClass('active');
            $("." + objsteps[curIdx]).show();
            $('#'+ objsteps[curIdx]).parent().addClass('active');
        });
//    
//        // Prev image on button click
//        $('#IssuanceSetup a.gray-btn').click( function() {
//            curIdx = (curIdx+max-1) % max;
//            $("div#IssuanceSetup > div").hide();
//             $('.tabs li').removeClass('active');
//            $("." + objsteps[curIdx]).show();
//           $('#'+ objsteps[curIdx]).parent().addClass('active');
//        });

        });
        
        function displayDivSteps(steps)
        {
             $("div#IssuanceSetup > div").hide();
             $('.tabs li').removeClass('active');
             // for steps div display
             $("." + steps).show();
             // for steps active tab
             $('#'+ steps).parent().addClass('active');
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderAdmin" runat="Server">
    <input type="hidden" value="admin-link" id="hdnActiveLink" />
    <asp:HiddenField ID="hdnCurrentStep" runat="server" Value="step-one" />
    <!-- BEGIN PAGE CONTAINER-->
    <div class="page-content">
        <!-- BEGIN SAMPLE PORTLET CONFIGURATION MODAL FORM-->
        <div id="portlet-config" class="modal hide">
            <div class="modal-header">
                <button data-dismiss="modal" class="close" type="button">
                </button>
                <h3>
                    Widget Settings</h3>
            </div>
            <div class="modal-body">
                Widget settings form goes here
            </div>
        </div>
        <div class="clearfix">
        </div>
        <div class="content">
            <div class="page-title">
                <h2 style="text-transform: uppercase">
                    Store Management</h2>
                <div class="search">
                    <select id="u60_input">
                        <option value="Spirit Airlines" selected="">Spirit Airlines</option>
                        <option value="Air Canada">Air Canada</option>
                        <option value="Southwest">Southwest</option>
                    </select>
                    <input type="submit" value="Change" class="btn">
                </div>
            </div>
            <div class="steps-header clearfix">
                <div class="title-txt alignleft">
                    Spirit Airlines</div>
                <div class="btn-group alignleft">
                    <a href="#" data-toggle="dropdown" class="btn-white dropdown-toggle btn-demo-space">
                        Issuance Policies<span class="caret"></span> </a>
                    <ul class="dropdown-menu">
                        <li><a href="#">Basic Information</a></li>
                        <li><a href="#">Shipping Policies</a></li>
                        <li><a href="#">Order Management</a></li>
                        <li><a href="#">Program document</a></li>
                        <li><a href="#">In-Store Marketing</a></li>
                        <li><a href="#">Store Credits</a></li>
                        <li><a href="#">Product Management</a></li>
                        <li><a href="#">Stations</a></li>
                        <li><a href="#">Workgroup Management</a></li>
                        <li><a href="#">Message Center</a></li>
                    </ul>
                </div>
            </div>
            <div class="container-body">
                <div id="IssuanceSetup">
                    <ul class="tabs">
                        <li class="active"><a href="javascript:;" id="step-one"><span>01</span>STEP</a></li>
                        <li><a href="javascript:;" id="step-two"><span>02</span>STEP</a></li>
                        <li><a href="javascript:;" id="step-three"><span>03</span>STEP</a></li>
                        <li><a href="javascript:;" id="step-four"><span>04</span>STEP</a></li>
                        <li><a href="javascript:;" id="step-five"><span>05</span>STEP</a></li>
                        <li><a href="javascript:;" id="step-six"><span>06</span>STEP</a></li>
                        <li><a href="javascript:;" id="step-seven"><span>07</span>STEP</a></li>
                        <li class="brd-none"><a href="javascript:;" id="step-eight"><span>08</span>STEP</a></li>
                    </ul>
                    <!--Issuance Policy STEP1-->
                    <div class="step-one">
                        <div class="steps-header">
                            <div class="title-txt">
                                Basic Info</div>
                        </div>
                        <div class="forms">
                            <div class="row">
                                <label>
                                    Company</label>
                                <div class="select-drop medium-drop">
                                    <asp:DropDownList ID="ddlCompany" runat="server" class="default" TabIndex="1" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rqCompany" runat="server" ControlToValidate="ddlCompany"
                                        Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" ValidationGroup="stepone"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <label>
                                    Gender</label>
                                <div class="right-cnt">
                                    <asp:RadioButtonList ID="rdbGender" runat="server" Width="35%" RepeatDirection="Horizontal">
                                    </asp:RadioButtonList>
                                    <asp:CustomValidator ID="cvGender" runat="server" ClientValidationFunction="validateCommon"
                                        CssClass="rdbGender" ValidationGroup="stepone"></asp:CustomValidator>
                                </div>
                            </div>
                            <div class="row">
                                <label>
                                    Workgroup</label>
                                <div class="select-drop medium-drop">
                                    <asp:DropDownList ID="ddlWorkGroup" runat="server" class="default" TabIndex="1">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rqWorkGroup" runat="server" ControlToValidate="ddlWorkGroup"
                                        Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" ValidationGroup="stepone"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <label>
                                    Kind of Item</label>
                                <div class="right-cnt">
                                    <asp:RadioButtonList ID="rdbCategoryList" runat="server" Width="68%" RepeatDirection="Horizontal">
                                        <asp:ListItem>Supply Items</asp:ListItem>
                                        <asp:ListItem Selected="True">Uniform Items</asp:ListItem>
                                        <asp:ListItem>Both Supply and Uniform Items</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:CustomValidator ID="rqCategoryList" runat="server" ClientValidationFunction="validateCommon"
                                        CssClass="rdbCategoryList" ValidationGroup="stepone"></asp:CustomValidator>
                                </div>
                            </div>
                            <div class="row">
                                <label>
                                    Policy Name</label>
                                <div class="right-cnt">
                                    <asp:TextBox ID="txtIssuancePolicyName" runat="server" class="input-field-all full-width"></asp:TextBox>
                                    <p class="msg">
                                        We recommend naming your policy something descriptive. For example: Male New Hite
                                        Ramp Agent.</p>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvIssuancepolicyName" runat="server" ControlToValidate="txtIssuancepolicyName"
                                    Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="stepone"
                                    ErrorMessage="Please enter program name."></asp:RequiredFieldValidator>
                            </div>
                            <div class="row">
                                <asp:LinkButton ID="lnkBtnStep1Next" runat="server" OnClick="lnkBtnStep1Next_Click"
                                    class="btn btn-danger btn-cons alignright" ValidationGroup="stepone"><span>Next</span><span class="next-arrow"></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <!--END STEP1-->
                    <!--Issuance Policy STEP2-->
                    <div class="step-two">
                        <div class="steps-header">
                            <div class="title-txt">
                                Pring Purchase</div>
                        </div>
                        <div class="forms">
                            <div class="row">
                                <label>
                                    Type of Pricing</label>
                                <div class="right-cnt">
                                    <asp:RadioButtonList ID="rdbPricingList" runat="server" Width="30%">
                                    </asp:RadioButtonList>
                                    <%-- <asp:CustomValidator ID="cvPricing" runat="server" ClientValidationFunction="validateCommon"
                                CssClass="rdbPricingList"></asp:CustomValidator>--%>
                                    <div class="sub-cnt">
                                        <label>
                                            Pricing End Date</label><input type="text" placeholder="" class="input-field-all first-field"
                                                title="First Name" /></div>
                                </div>
                            </div>
                            <div class="row">
                                <label>
                                    Show Pricing</label>
                                <div class="right-cnt">
                                    <asp:RadioButtonList ID="rdbShowPricingList" runat="server" Width="25%" RepeatDirection="Horizontal">
                                    </asp:RadioButtonList>
                                    <asp:CustomValidator ID="cvShowPricing" runat="server" ClientValidationFunction="validateCommon"
                                        CssClass="rdbShowPricingList" ValidationGroup="steptwo"></asp:CustomValidator>
                                </div>
                            </div>
                            <div class="row">
                                <label>
                                    Type of Purchase</label>
                                <div class="right-cnt">
                                    <label>
                                        <asp:RadioButton ID="rdbComplete" runat="server" GroupName="CompletePurchase" class="iradio_flat" />Complete
                                        Purchase:</label>
                                    <p>
                                        the employee must order everything in their issuance policy at one time to use the
                                        Issuance. Recommended.</p>
                                    <label>
                                        <asp:RadioButton ID="rdbPartial" runat="server" GroupName="CompletePurchase" class="iradio_flat" />Partial
                                        Purchase:</label>
                                    <p>
                                        the employee is allowed to place several orders until they have purchased everything
                                        available in their issuance policy. If you choose Partial Purchase the issuance
                                        will remain open until they have completed all their purchases.</p>
                                </div>
                                <asp:CustomValidator ID="rfvCompletePurchase" runat="server" ClientValidationFunction="validateCommon"
                                    CssClass="CompletePurchase" ValidationGroup="steptwo"></asp:CustomValidator>
                                <div class="row">
                                    <asp:LinkButton ID="lnkBtnSetp2Back" runat="server" OnClick="lnkBtnSetp2Back_Click"
                                        class="gray-btn alignleft" Text="Back" CausesValidation="false"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnStep2Next" runat="server" OnClick="lnkBtnStep2Next_Click"
                                        class="btn btn-danger btn-cons alignright" ValidationGroup="steptwo"><span>Next</span><span class="next-arrow"></span></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--END STEP2-->
                    <!--Issuance Policy STEP3-->
                    <div class="step-three">
                        <div class="steps-header">
                            <div class="title-txt">
                                Activation Period</div>
                        </div>
                        <div class="forms">
                            <div class="row">
                                <label>
                                    Activation Period</label>
                                <div class="right-cnt">
                                    <label>
                                        <asp:RadioButton ID="rdbPolicyActivationDOH" runat="server" GroupName="PolicyActivation"
                                            class="iradio_flat" />Date of Hire</label>
                                    <div class="sub-cnt">
                                        <label class="pad-none">
                                            <asp:RadioButton ID="rdbPolicyActivationUOH" runat="server" GroupName="DOH" class="iradio_flat" />Activate
                                            Upon Hire</label>
                                        <div class="sub-ctn2">
                                            <label>
                                                <asp:RadioButton ID="rdbPolicyActivationUOM" runat="server" GroupName="DOH" class="iradio_flat" />Activate</label>
                                            <div class="cart-dropin">
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
                                            </div>
                                            <label>
                                                Months from Date of Hire</label>
                                            <asp:CustomValidator ID="cvPolicyActivationDOH" runat="server" ClientValidationFunction="ValidatePolicyActivationDOH"
                                                CssClass="DOH" ValidationGroup="stepthree"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rqddlmonth" runat="server" ControlToValidate="ddlmonth"
                                                Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <label>
                                        <asp:RadioButton ID="rdbPolicyActivationDate" runat="server" GroupName="PolicyActivation"
                                            class="iradio_flat" />Chosen Date</label>
                                    <div class="sub-cnt">
                                        <label>
                                            From</label>
                                        <asp:TextBox ID="txtDateFrom" runat="server" class="input-field-all setDatePicker"
                                            TabIndex="11"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqtxtDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                            Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvPolicyActivationDate" runat="server" ClientValidationFunction="ValidatePolicyActivationDate"
                                            ValidationGroup="stepthree"></asp:CustomValidator>
                                    </div>
                                    <label>
                                        <asp:RadioButton ID="rdbPolicyActivationPreviusIssuance" runat="server" GroupName="PolicyActivation"
                                            class="iradio_flat" />Previous Issuance Policy</label>
                                    <div class="sub-cnt">
                                        <label class="pad-none black-text">
                                            <asp:RadioButton ID="rdbPolicyActivationMonth1" runat="server" GroupName="PolicyActivationPreviusIssuance"
                                                class="iradio_flat" />Based on previous Activation Date</label>
                                        <div class="sub-ctn2">
                                            <label>
                                                Activate</label>
                                            <div class="cart-dropin">
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
                                            </div>
                                            <label>
                                                months after previous Activation Date</label>
                                        </div>
                                        <label class="black-text">
                                            <asp:RadioButton ID="rdbPolicyActivationMonth2" runat="server" GroupName="PolicyActivationPreviusIssuance"
                                                class="iradio_flat" />Based on previous Expiration Date</label>
                                        <div class="sub-ctn2">
                                            <label>
                                                Activate</label>
                                            <div class="cart-dropin">
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
                                            </div>
                                            <label>
                                                months after previous Expiration Date</label>
                                        </div>
                                        <label class="black-text">
                                            <asp:RadioButton ID="rdbPolicyActivationMonth3" runat="server" GroupName="PolicyActivationPreviusIssuance"
                                                class="iradio_flat" />Based on previous Complete Order Ship Date</label>
                                        <div class="sub-ctn2">
                                            <label>
                                                Activate</label>
                                            <div class="cart-dropin">
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
                                            </div>
                                            <label>
                                                months after previous Complete Order Ship Date</label>
                                        </div>
                                    </div>
                                </div>
                                <asp:CustomValidator ID="cvPolicyActivation" runat="server" ClientValidationFunction="validateCommon"
                                    CssClass="PolicyActivation" ValidationGroup="stepthree"></asp:CustomValidator>
                            </div>
                            <div class="row">
                                <asp:LinkButton ID="lnkBtnSetp3Back" runat="server" OnClick="lnkBtnSetp3Back_Click"
                                    class="gray-btn alignleft" Text="Back" CausesValidation="false"></asp:LinkButton>
                                <asp:LinkButton ID="lnkBtnStep3Next" runat="server" OnClick="lnkBtnStep3Next_Click"
                                    class="btn btn-danger btn-cons alignright" ValidationGroup="stepthree"><span>Next</span><span class="next-arrow"></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <!--END STEP3-->
                    <!--Issuance Policy STEP4-->
                    <div class="step-four">
                        <div class="steps-header">
                            <div class="title-txt">
                                Expiration Access</div>
                        </div>
                        <div class="forms">
                            <div class="row">
                                <label>
                                    Expiration</label>
                                <div class="right-cnt">
                                    <label>
                                        <asp:RadioButton ID="rdbExpireAfterPurchaseComplete" runat="server" GroupName="ExpireAfterPurchaseComplete"
                                            class="iradio_flat" />Expires after purchase is complete</label>
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbExpireNoEmpHireDate" runat="server" GroupName="ExpireAfterPurchaseComplete"
                                                class="iradio_flat" />Expires</label>
                                        <div class="cart-dropin">
                                            <asp:DropDownList ID="ddlExpireNoEmpHireDate" runat="server" class="default" TabIndex="12">
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
                                        </div>
                                        <label>
                                            months after employee hire date.</label>
                                    </div>
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbSelectExpireDate" runat="server" GroupName="ExpireAfterPurchaseComplete"
                                                class="iradio_flat" />Select an expiration date</label>
                                        <asp:TextBox ID="txtExpireDate" runat="server" class="input-field-all setDatePicker"
                                            TabIndex="11"></asp:TextBox>
                                    </div>
                                </div>
                                <label>
                                    Employee Access</label>
                                <div class="right-cnt">
                                    <label>
                                        <asp:CheckBox ID="chkEmployeeAccesssExpiration" runat="server" GroupName="EmployeeAccesssExpiration" />Employee
                                        access stop upon expiration</label>
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbReactivateNomonthsofPrevIssuanceOrderShipDate" runat="server"
                                                GroupName="EmployeeAccesssExpiration" class="iradio_flat" />After</label>
                                        <div class="cart-dropin">
                                            <asp:DropDownList ID="ddlReactivateNomonthsofPrevIssuanceOrderShipDate" runat="server"
                                                class="default" TabIndex="12">
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
                                        </div>
                                        <label>
                                            months of previous issuance order ship date</label>
                                    </div>
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbReactivateNomonthsofPrevIssuanceExpireDate" runat="server"
                                                GroupName="EmployeeAccesssExpiration" class="iradio_flat" />After</label>
                                        <div class="cart-dropin">
                                            <asp:DropDownList ID="ddlReactivateNomonthsofPrevIssuanceExpireDate" runat="server"
                                                class="default" TabIndex="12">
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
                                        </div>
                                        <label>
                                            months of previous issuance expire date</label>
                                    </div>
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbReactivateSameTimeEveryYear" runat="server" GroupName="EmployeeAccesssExpiration"
                                                class="iradio_flat" />Same period of time every year</label></div>
                                </div>
                                <label>
                                    Product Grouping</label>
                                <div class="right-cnt">
                                    <label>
                                        <asp:RadioButton ID="rdbProductGroupingorderPiece1" runat="server" GroupName="ProductGrouping"
                                            class="iradio_flat" />Order 1 piece of every item in this group</label>
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbProductGroupingorderPieceNo" runat="server" GroupName="ProductGrouping"
                                                class="iradio_flat" />Order</label>
                                        <div class="cart-dropin">
                                            <asp:DropDownList ID="ddlProductGroupingorderPieceNo" runat="server" class="default"
                                                TabIndex="12">
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
                                        </div>
                                        <label>
                                            pieces of any combination you choose for the items in this group</label>
                                    </div>
                                </div>
                                <label>
                                    Marketing</label>
                                <div class="right-cnt">
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:CheckBox ID="chkMarketingEmailRemainderNo" runat="server"></asp:CheckBox>Email
                                            reminder</label>
                                        <div class="cart-dropin">
                                            <asp:DropDownList ID="ddlMarketingEmailRemainderNo" runat="server" class="default"
                                                TabIndex="12">
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
                                        </div>
                                        <label>
                                            month before expiration</label>
                                    </div>
                                    <label class="pad-b20">
                                        <asp:CheckBox ID="chkMarketingEmailRemainderPolicyActivate" runat="server"></asp:CheckBox>Email
                                        when policy is activated</label>
                                </div>
                                <div class="row">
                                    <asp:LinkButton ID="lnkBtnSetp4Back" runat="server" OnClick="lnkBtnSetp4Back_Click"
                                        class="gray-btn alignleft" Text="Back" CausesValidation="false"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnStep4Next" runat="server" OnClick="lnkBtnStep4Next_Click"
                                        class="btn btn-danger btn-cons alignright" ValidationGroup="stepfour"><span>Next</span><span class="next-arrow"></span></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <!--END STEP4-->
                    </div>
                    <!--END STEP4-->
                    <!--Issuance Policy STEP5-->
                    <div class="step-five">
                        <div class="steps-header">
                            <div class="title-txt">
                                Shipping</div>
                        </div>
                        <div class="forms">
                            <div class="row">
                                <label>
                                    Shipping Method</label>
                                <div class="right-cnt">
                                    <label>
                                        <asp:RadioButton ID="rdbShippingMethod" runat="server" GroupName="ShippingMethod"
                                            class="iradio_flat" />Select Shiping method on checkout</label>
                                    <div class="sub-cnt pad-none">
                                        <label class="black-text">
                                            <asp:RadioButton ID="rdbSpecificShipper" runat="server" GroupName="ShippingMethod"
                                                class="iradio_flat" />Select specific shipper</label></div>
                                    <div class="sub-cnt2 mrg-l25">
                                        <label>
                                            <asp:RadioButton ID="rdbUPS" runat="server" GroupName="ShippingMethodSpecific" class="iradio_flat" />UPS</label>
                                        <label>
                                            <asp:RadioButton ID="rdbFedex" runat="server" GroupName="ShippingMethodSpecific"
                                                class="iradio_flat" />Fedex</label>
                                        <label>
                                            <asp:RadioButton ID="rdbOther" runat="server" GroupName="ShippingMethodSpecific"
                                                class="iradio_flat" />Other</label>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <label>
                                    Account #</label>
                                <div class="right-cnt">
                                    <label>
                                        <asp:RadioButton ID="rdbShipperAccountNo" runat="server" GroupName="ShipperAccountNo"
                                            class="iradio_flat" />Use Customer Shipper Account Number</label>
                                    <asp:TextBox ID="txtShipperAccountNo" runat="server" class="input-field-all first-field"
                                        title="First Name" placeholder=""></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <label>
                                    Allow Issuance Order to Ship</label>
                                <div class="right-cnt">
                                    <label>
                                        <asp:RadioButton ID="rdbGroundService" runat="server" GroupName="AllowIssuanceOrderToShip"
                                            class="iradio_flat" />Ground Service</label>
                                    <label>
                                        <asp:RadioButton ID="rdb2dayAirService" runat="server" GroupName="AllowIssuanceOrderToShip"
                                            class="iradio_flat" />2 Day Air Service</label>
                                    <label>
                                        <asp:RadioButton ID="rdbOverNight" runat="server" GroupName="AllowIssuanceOrderToShip"
                                            class="iradio_flat" />Overnight</label>
                                </div>
                            </div>
                            <div class="row">
                                <asp:LinkButton ID="lnkBtnSetp5Back" runat="server" OnClick="lnkBtnSetp5Back_Click"
                                    class="gray-btn alignleft" Text="Back" CausesValidation="false"></asp:LinkButton>
                                <asp:LinkButton ID="lnkBtnStep5Next" runat="server" OnClick="lnkBtnStep5Next_Click"
                                    class="btn btn-danger btn-cons alignright" ValidationGroup="stepfive"><span>Next</span><span class="next-arrow"></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <!--END STEP5-->
                    <!--Issuance Policy STEP6-->
                    <div class="step-six">
                        <div class="steps-header">
                            <div class="title-txt">
                                Address</div>
                        </div>
                        <div class="forms">
                            <div class="row">
                                <label>
                                    Address</label>
                                <div class="right-cnt">
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbFixedShippingAddress" runat="server" GroupName="ShippingAddress"
                                                class="iradio_flat" />Fix Shipping Address for
                                        </label>
                                        <div class="cart-dropin">
                                            <asp:DropDownList ID="DropDownList1" runat="server" class="default" TabIndex="12">
                                                <asp:ListItem Value="0" Text="select"></asp:ListItem>
                                                <asp:ListItem Value="All" Text="All"></asp:ListItem>
                                                <asp:ListItem Value="Onlythis" Text="Only this"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <label>
                                            Issuances</label>
                                    </div>
                                    <ul class="listing">
                                        <li class="alignleft">
                                            <label>
                                                Company</label>
                                            <div class="select-drop medium-drop">
                                                <asp:DropDownList ID="ddlShippingAddressCompany" runat="server" class="default" TabIndex="12">
                                                </asp:DropDownList>
                                            </div>
                                        </li>
                                        <li class="alignright">
                                            <label>
                                                Department</label>
                                            <div class="select-drop medium-drop">
                                                <asp:DropDownList ID="ddlShippingAddressDepartment" runat="server" class="default"
                                                    TabIndex="12">
                                                </asp:DropDownList>
                                            </div>
                                        </li>
                                        <li class="alignleft">
                                            <label>
                                                Address</label>
                                            <asp:TextBox ID="txtAdress" runat="server" class="input-field-all"></asp:TextBox>
                                        </li>
                                        <li class="alignright">
                                            <label>
                                                Country</label>
                                            <div class="select-drop medium-drop">
                                                <asp:DropDownList ID="ddlCountry" runat="server" class="default" TabIndex="12">
                                                </asp:DropDownList>
                                            </div>
                                        </li>
                                        <li class="alignleft">
                                            <label>
                                                City</label>
                                            <asp:TextBox ID="txtCity" runat="server" class="input-field-all"></asp:TextBox>
                                        </li>
                                        <li class="alignright">
                                            <label>
                                                State</label>
                                            <div class="select-drop medium-drop">
                                                <asp:DropDownList ID="ddlState" runat="server" class="default" TabIndex="12">
                                                </asp:DropDownList>
                                            </div>
                                        </li>
                                        <li class="alignleft">
                                            <label>
                                                Zip Code</label>
                                            <asp:TextBox ID="txtZipCode" placeholder="98765" runat="server" class="input-field-all"></asp:TextBox>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="row">
                                <asp:LinkButton ID="lnkBtnSetp6Back" runat="server" OnClick="lnkBtnSetp6Back_Click"
                                    class="gray-btn alignleft" Text="Back" CausesValidation="false"></asp:LinkButton>
                                <asp:LinkButton ID="lnkBtnStep6Next" runat="server" OnClick="lnkBtnStep6Next_Click"
                                    class="btn btn-danger btn-cons alignright" ValidationGroup="stepsix"><span>Next</span><span class="next-arrow"></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <!--END STEP6-->
                    <!--Issuance Policy STEP7-->
                    <div class="step-seven">
                        <div class="steps-header">
                            <div class="title-txt">
                                Accounting payment</div>
                        </div>
                        <div class="forms">
                            <div class="row">
                                <label>
                                    Payment</label>
                                <div class="right-cnt">
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbPaymentMethodOnCheckOut" runat="server" GroupName="PaymentMethod"
                                                class="iradio_flat" />Select Method on Checkout</label>
                                    </div>
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbBillCorporate" runat="server" GroupName="PaymentMethod" class="iradio_flat" />Bill
                                            Corporate Office</label>
                                    </div>
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbMOAS" runat="server" GroupName="PaymentMethod" class="iradio_flat" />MOAS</label>
                                    </div>
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbEmployeepayroll" runat="server" GroupName="PaymentMethod"
                                                class="iradio_flat" />Employee Payroll Deduct</label>
                                    </div>
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbBillCorporatePayrollCreditCard" runat="server" GroupName="PaymentMethod"
                                                class="iradio_flat" />Bill Corporate Office & Difference Paid by Employee Payroll
                                            Deduct or Credit Card</label>
                                    </div>
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbCreditCard" runat="server" GroupName="PaymentMethod" class="iradio_flat" />Creadit
                                            Card</label>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <label>
                                    Accounting</label>
                                <div class="right-cnt">
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbGLCode" runat="server" GroupName="Accounting" class="iradio_flat" />GL
                                            Code</label>
                                        <asp:TextBox ID="txtGLCode" runat="server" class="input-field-all"></asp:TextBox>
                                    </div>
                                    <div class="sub-cnt3">
                                        <label>
                                            <asp:RadioButton ID="rdbPurchaseOrder" runat="server" GroupName="Accounting" class="iradio_flat" />Purchase
                                            Order Number</label>
                                        <asp:TextBox ID="txtPurchaseorder" runat="server" class="input-field-all"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <asp:LinkButton ID="lnkBtnSetp7Back" runat="server" OnClick="lnkBtnSetp7Back_Click"
                                    class="gray-btn alignleft" Text="Back" CausesValidation="false"></asp:LinkButton>
                                <asp:LinkButton ID="lnkBtnStep7Next" runat="server" OnClick="lnkBtnStep7Next_Click"
                                    class="btn btn-danger btn-cons alignright" ValidationGroup="stepseven"><span>Next</span><span class="next-arrow"></span></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <!--END STEP7-->
                    <!--Issuance Policy STEP8-->
                    <div class="step-eight">
                        <div class="steps-header">
                            <div class="title-txt">
                                Store Management</div>
                        </div>
                        <div class="forms">
                            <p class="msg">
                                Please select the item(s) that you will be setting a reminder for and also the number
                                of pieces that will be issued:</p>
                            <div class="row">
                                <ul class="stor-listing">
                                    <li class="multi-select">
                                        <div class="select-drop medium-drop">
                                            <select class="default" name="station-code">
                                                <option value="">Create Single Item Association</option>
                                                <option value="">Company 1</option>
                                                <option value="">Company 2</option>
                                                <option value="">Company 3</option>
                                            </select>
                                        </div>
                                        <div class="select-drop medium-drop">
                                            <select class="default" name="station-code">
                                                <option value="">Create Single Item Association</option>
                                                <option value="">Company 1</option>
                                                <option value="">Company 2</option>
                                                <option value="">Company 3</option>
                                            </select>
                                        </div>
                                        <div class="select-drop medium-drop">
                                            <select class="default" name="station-code">
                                                <option value="">Create Single Item Association</option>
                                                <option value="">Company 1</option>
                                                <option value="">Company 2</option>
                                                <option value="">Company 3</option>
                                            </select>
                                        </div>
                                    </li>
                                    <li class="select-half">
                                        <label>
                                            Employee Type</label>
                                        <div class="select-drop medium-drop">
                                            <select class="default" name="station-code">
                                                <option value="">Select...</option>
                                                <option value="">Department 1</option>
                                                <option value="">Department 2</option>
                                                <option value="">Department 3</option>
                                            </select>
                                        </div>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            Climate</label>
                                        <input type="text" value="" class="input-field-all" />
                                    </li>
                                </ul>
                                <div class="stor-img">
                                    <div class="product">
                                        <img src="assets/img/product.png" width="201" height="225" alt="product"></div>
                                    <a class="btn btn-danger btn-cons alignright">Add Master Item</a>
                                </div>
                            </div>
                            <div class="row">
                                <div class="tbl-title">
                                    List of all items</div>
                                <asp:GridView ID="gvPolicyItem" runat="server" AutoGenerateColumns="False"
                 class="table table-bordered" GridLines="None" 
                ShowFooter="True" OnRowCancelingEdit="gvPolicyItem_RowCancelingEdit"  OnRowUpdating="gvPolicyItem_RowUpdating" DataKeyNames="UniformIssuancePolicyItemID">
                <RowStyle CssClass="ord_content"></RowStyle>
                <Columns>
                    <asp:TemplateField SortExpression="MasterStyleName">
                        <HeaderTemplate>
                            <span>
                                <asp:LinkButton ID="lnkbtnMasterStyleName" runat="server" CommandArgument="MasterStyleName"
                                    CommandName="Sort">Master #</asp:LinkButton>
                            </span>
                            <div class="corner">
                                <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="first">
                               
                                
                            </span>
                            <div class="corner">
                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                            </div>
                            <asp:HiddenField runat="server" ID="lblRankId" Value='<%# Eval("RankId") %>' />
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkAssociationIssuanceType" runat="server" CommandName="Sort"><span>Association Type</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblsLookupName" runat="server" Text='<% # (Convert.ToString(Eval("sLookupName")).ToString().Length > 30) ? Eval("sLookupName").ToString().Substring(0,30)+"..." :  Convert.ToString(Eval("sLookupName")+ "&nbsp;") %>'
                                ToolTip='<%# Eval("sLookupName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkPolicyNote" runat="server" CommandName="Sort"><span>Policy Note</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPolicyNote" runat="server" Text='<% # (Convert.ToString(Eval("AssociationIssuancePolicyNote")).ToString().Length > 10) ? Eval("AssociationIssuancePolicyNote").ToString().Substring(0,10)+"..." :  Convert.ToString(Eval("AssociationIssuancePolicyNote")+ "&nbsp;") %>'
                                ToolTip='<%# Eval("AssociationIssuancePolicyNote") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkIssuance" runat="server" CommandName="Sort"><span>Issuance</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblIssuance" runat="server" Text='<% # Eval("Issuance") + "&nbsp;" %>'
                                ToolTip='<%# Eval("Issuance") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="7%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkBudgetAmount" runat="server" CommandName="Sort"><span>Budget Amt</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblBudgetAmt" runat="server" Text='<% # Eval("AssociationbudgetAmt") + "&nbsp;" %>'
                                ToolTip='<%# Eval("AssociationbudgetAmt") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="b_box" Width="9%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkEmployeeType" runat="server" CommandName="Sort"><span>Employee Type</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblEmployeeType" runat="server" Text='<% # (Convert.ToString(Eval("EmployeeType")).ToString().Length > 8) ? Eval("EmployeeType").ToString().Substring(0,8)+"..." :  Convert.ToString(Eval("EmployeeType")+ "&nbsp;") %>'
                                ToolTip='<%# Eval("EmployeeType") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEmployeeType" Style="background-color: #303030; border: medium none;
                                color: #ffffff; width: 120px; padding: 2px" BackColor="#303030" runat="server">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdnEmployeeType" runat="server" Value='<%# Eval("EmployeeType") %>' />
                        </EditItemTemplate>
                        <ItemStyle CssClass="g_box" Width="9%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkClimateType" runat="server" CommandName="Sort"><span>Climate Type</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblClimateType" runat="server" Text='<% # (Convert.ToString(Eval("WeatherType")).ToString().Length > 8) ? Eval("WeatherType").ToString().Substring(0,8)+"..." :  Convert.ToString(Eval("WeatherType")+ "&nbsp;") %>'
                                ToolTip='<%# Eval("WeatherType") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlWeatherType" Style="background-color: #303030; border: medium none;
                                color: #ffffff; width: 130px; padding: 2px" BackColor="#303030" runat="server">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdnWeatherType" runat="server" Value='<%# Eval("WeatherType") %>' />
                        </EditItemTemplate>
                        <ItemStyle CssClass="b_box" Width="9%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkGroupName" runat="server" CommandName="Sort"><span>Group Name</span></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblNEWGROUP" runat="server" Text='<% # (Convert.ToString(Eval("NEWGROUP")).ToString().Length > 8) ? Eval("NEWGROUP").ToString().Substring(0,8)+"..." :  Convert.ToString(Eval("NEWGROUP")+ "&nbsp;") %>'
                                ToolTip='<%# Eval("NEWGROUP") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" Width="9%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span class="white_co">Edit</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                    Text="Edit"></asp:LinkButton>
                            </span>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <span>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                                    Text="Update"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text="Cancel"></asp:LinkButton>
                            </span>
                        </EditItemTemplate>
                        <ItemStyle CssClass="b_box" HorizontalAlign="Center" Width="14%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span class="white_co">Delete</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span style="height: 26px">&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="imgDelete"
                                runat="server" ImageUrl="~/Images/close.png" CommandName="DeleteRec" CommandArgument='<%# Eval("UniformIssuancePolicyItemID") %>'
                                OnClientClick="javascript:return confirm('Are you sure, you want to delete this record ?')" /></span>
                        </ItemTemplate>
                        <ItemStyle CssClass="g_box" HorizontalAlign="Center" Width="3%" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="ord_header"></HeaderStyle>
            </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <!--END STEP8-->
                </div>
            </div>
        </div>
    </div>
</asp:Content>
