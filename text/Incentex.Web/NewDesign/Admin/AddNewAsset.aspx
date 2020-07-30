<%@ Page Title="incentex | Add New Asset" Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master"
    AutoEventWireup="true" EnableEventValidation="false" CodeFile="AddNewAsset.aspx.cs"
    Inherits="Admin_AddNewAsset" EnableViewState="true" %>

<%@ Reference Control="~/NewDesign/UserControl/CustomDropDown.ascx" %>
<%@ Reference Control="~/NewDesign/UserControl/TextBoxControl.ascx" %>
<%@ Reference Control="~/NewDesign/UserControl/DropDownControl.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/NewDesign/UserControl/CustomDropDown.ascx" TagName="CustomDropDown"
    TagPrefix="cdd" %>
<%@ Register Src="~/NewDesign/UserControl/TextBoxControl.ascx" TagName="TextBoxControl"
    TagPrefix="cdd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../StaticContents/js/MultipeUpload.js" type="text/javascript"></script>

    <script src="../StaticContents/js/autoNumeric.js" type="text/javascript"></script>

    <script type="text/javascript">
        var form_original_data;
        $(document).ready(function () {            
            
            //assetSection
            $(".assetSection input, .assetSection select, .assetSection textarea").on('input, change', function() {
                TabInfoChanged();
            });
            
            GetTriggeredElement();
            $(window).ValidationUI();
            $(".pricetext").autoNumeric('init');
            
            $('.FieldTypes').on('ifClicked', function(event){
                $(this).parents("li.typeoffield-radio").find("div.iradio_flat").removeClass("checked");
                $(this).parents("li.typeoffield-radio").find("input[type=radio]").removeAttr("checked");
                $("." + $(this).parent("div").attr("class")).children("input[type=radio]").removeAttr("checked");
                $(this).attr("checked", true);
            });
            
            $('.accmethod').on('ifClicked', function(event){
                $(this).parents("li.acq-method").find("div.iradio_flat").removeClass("checked");
                $(this).parents("li.acq-method").find("input[type=radio]").removeAttr("checked");
                $("." + $(this).parent("div").attr("class")).children("input[type=radio]").removeAttr("checked");
                $(this).attr("checked", true);
            });
            
            $("ul.CustomFields input,ul.CustomFields select").on('input, change', function() {
                var CurrentValue = $(this).val();
                $(this).removeAttr("value").attr("value",CurrentValue);
            });
            
            form_original_data =  $("#" + $(".MainDiv").val()).find("input, textarea, select").serialize(); 
        });
        
        function ClearFileUploadControl()
        {
            $("#dvAccountingLeaseFiles").siblings("div.asset-img-block").find('a.MultiFile-remove').click();
        }
        
        function GetTriggeredElement() {
            SetCheckBoxRadioButtonEvents();
          //  SetCalendar();
            if($("a.active").length > 0)
            {
                if ($("a.active").attr("title").toUpperCase() == "BASIC")
                {
                    //BindBasicTab_PopUpEvents();
                }
                else if ($("a.active").attr("title").toUpperCase() == "ACCOUNTING")
                    BindAccountingTab_PopUpEvents(); 
                else if ($("a.active").attr("title").toUpperCase() == "WARRANTY") 
                    BindWarrantyTab_PopUpEvents();
                else if ($("a.active").attr("title").toUpperCase() == "SPECS") 
                    BindSpecsTab_PopUpEvents();
            }
            
//            $('.asset-bar .view-note').click(function () {
//                ShowPopUp('view-note-popup', 'view-note-popup .warranty-content','viewnote');
//                ResetAllFields('view-note-popup');
//            });
            
            $(".default").uniform();
            SetDatePicker();
            //ScrollToTag("container",false); 
            //            $("#container").delegate(".parentdiv","click",function(e)
            $("div.parentdiv").on("click", function (e) {
                //$(this).next(".childdiv").slideToggle('slow');
                 $(this).toggleClass("setOpen");
               
                SetOpenDivIDsToHiddenFields();
            });
            
            var SetOpenDivIds = $("#<%= hdnOpenDivs.ClientID %>").val();
            if(SetOpenDivIds != "#")
            {
                $(SetOpenDivIds).show();
                $(SetOpenDivIds).next(".childdiv").show();
                $(SetOpenDivIds).addClass("setOpen");
            }
            
            var SetCloseDivIds = $("#<%= hdnCloseDivs.ClientID %>").val();
            if(SetCloseDivIds != "#")
            {
                $(SetCloseDivIds).hide();
                $(SetCloseDivIds).addClass("closeDiv").removeClass("setOpen");
            }
            
            $(".applyScroll").niceScroll(
			{
			                touchbehavior: false,
			                cursorcolor: "#666",
			                cursoropacitymax: 0.7,
			                cursorwidth: 6,
			                cursorborder: "1px solid #666",
			                cursorborderradius: "8px",
			                autohidemode: "scroll"
			});
			
			$(".invoiceamount").blur(function(){
			    var CurrentCtrl = $(this);
    			var TotalAmount = $("#<%= txtTotalInvoiceAmt.ClientID %>").val().replace('$','').trim().replace(/,/g,'').trim();
			    var PartsAmount = $("#<%= txtPartsAmount.ClientID %>").val().replace('$','').trim().replace(/,/g,'').trim();
			    var LaborAmount = $("#<%= txtLaborAmount.ClientID %>").val().replace('$','').trim().replace(/,/g,'').trim();
			    
			    if(CurrentCtrl.attr("id") == "<%= txtTotalInvoiceAmt.ClientID %>")
			    {
			        if(IsDecimal(TotalAmount) && IsDecimal(LaborAmount))
                        PartsAmount = parseFloat(parseFloat(TotalAmount) - parseFloat(LaborAmount));                        
                    else if(IsDecimal(TotalAmount) && IsDecimal(PartsAmount))
                        LaborAmount = parseFloat(parseFloat(TotalAmount) - parseFloat(PartsAmount));                        
                }
			    else if(CurrentCtrl.attr("id") == "<%= txtPartsAmount.ClientID %>")
			    {
			        if(PartsAmount != "" && parseFloat(TotalAmount) >= parseFloat(PartsAmount))
			        {
			            if(IsDecimal(TotalAmount) && IsDecimal(PartsAmount))
                            LaborAmount = parseFloat(parseFloat(TotalAmount) - parseFloat(PartsAmount));                        
                        else if(IsDecimal(LaborAmount) && IsDecimal(PartsAmount))
                            TotalAmount = parseFloat(parseFloat(LaborAmount) + parseFloat(PartsAmount));
                    }
                    else if (parseFloat(TotalAmount) < parseFloat(PartsAmount))
                    {
                        if(LaborAmount != "")
                            TotalAmount = parseFloat(parseFloat(PartsAmount) + parseFloat(LaborAmount));
                        else
                            TotalAmount = parseFloat(PartsAmount); 
                    }
                    else if(PartsAmount != "")
                    {
                        TotalAmount = parseFloat(parseFloat(TotalAmount) + parseFloat(PartsAmount));
                        if(LaborAmount != "")
                            TotalAmount = parseFloat(TotalAmount + parseFloat(LaborAmount));
                        else
                            LaborAmount = parseFloat(parseFloat(TotalAmount) - parseFloat(PartsAmount)); 
                    }   
			    }
			    else if(CurrentCtrl.attr("id") == "<%= txtLaborAmount.ClientID %>")
			    {
			        if(LaborAmount != "" && parseFloat(TotalAmount) >= parseFloat(LaborAmount))
			        {
			            if(IsDecimal(TotalAmount) && IsDecimal(LaborAmount))
                            PartsAmount = parseFloat(parseFloat(TotalAmount) - parseFloat(LaborAmount));                        
                        else if(IsDecimal(PartsAmount) && IsDecimal(LaborAmount))
                            TotalAmount = parseFloat(parseFloat(PartsAmount) + parseFloat(LaborAmount));
                    }
                    else if (parseFloat(TotalAmount) < parseFloat(LaborAmount))
                    {
                        if(LaborAmount != "")
                            TotalAmount = parseFloat(parseFloat(PartsAmount) + parseFloat(LaborAmount));
                        else
                            TotalAmount = parseFloat(LaborAmount); 
                    }
                    else if(LaborAmount != "")
                    {
                        TotalAmount = parseFloat(parseFloat(TotalAmount) + parseFloat(LaborAmount));
                        if(PartsAmount != "")
                            TotalAmount = parseFloat(TotalAmount + parseFloat(PartsAmount));    
                         else
                            PartsAmount = parseFloat(parseFloat(TotalAmount) - parseFloat(LaborAmount)); 
                    }
			    }
			    
			    $("#<%= txtTotalInvoiceAmt.ClientID %>").val(TotalAmount).autoNumeric('update');
			    $("#<%= txtPartsAmount.ClientID %>").val(PartsAmount).autoNumeric('update');
			    $("#<%= txtLaborAmount.ClientID %>").val(LaborAmount).autoNumeric('update');
			    
			});
        }
        
        function SetOpenDivIDsToHiddenFields()
        {
            var CurrOpenDivIds = "#";
            $("div.setOpen").each(function() {
                    if(CurrOpenDivIds != "#")
                        CurrOpenDivIds += ", #";
                    CurrOpenDivIds += $(this).attr("id");     
            });
            $("#<%= hdnOpenDivs.ClientID %>").val(CurrOpenDivIds);
        }
        
        function SetCloseDivIDsToHiddenFields()
        {
            var CurrCloseDivIds = "#";
            $("div.closeDiv").each(function() {
                    if(CurrCloseDivIds != "#")
                        CurrCloseDivIds  += ", #";
                    CurrCloseDivIds  += $(this).attr("id");     
            });
            $("#<%= hdnCloseDivs.ClientID %>").val(CurrCloseDivIds);
        }
        
        function ResetAllFields(TagID)
        {
            $("#" +TagID).find("input").filter(function () { if (this.type != undefined && (this.type == "text" || this.type == "password" || this.type == "textarea" || this.type == "file")) return this; }).each(function(){
                $(this).val("").removeAttr("readonly").removeClass("ErrorField");
            });
            
            $("#" +TagID).find("textarea").each(function(){
                $(this).val("").removeAttr("readonly").removeClass("ErrorField");
            });
            
            $("#" +TagID).find("select").each(function(){
                $(this).val("0");
                $(this).prev("span").html($(this).find("option:selected").html());
                $(this).removeAttr("readonly").removeClass("ErrorField");
            });
            var HasRadioOrCheckBox = false;
            $("#" +TagID).find("input").filter(function () { if (this.type != undefined && (this.type == "checkbox" || this.type == "radio")) return this; }).each(function(){
                $(this).removeAttr("checked");
                $(this).parents("li.typeoffield-radio").find("div.iradio_flat").removeClass("checked");
                HasRadioOrCheckBox = true;
            });
            if(HasRadioOrCheckBox)
            {
                var GroupName = "";
                $("#" +TagID).find("li").each(function(){
                    $(this).find("input[type=radio]:first").filter(function () { if($(this).attr("name") != GroupName) return this; }).each(function(){
                        $(this).attr("checked","checked");
                        $(this).parent("div.iradio_flat").addClass("checked");
                    });
                    GroupName = $(this).find("input[type=radio]:first").attr("name");
                });
            }
            $(".input-textarea").html("");
        }
        function BindAccountingTab_PopUpEvents()
        {
            $('.invoicepostbtn').click(function () {
                $('#<%= hfInvoiceID.ClientID %>').val("0");
                ShowPopUp('assetspendinginvoice-popup', 'assetspendinginvoice-popup .assetspoupup-content','AddInvoice');
                ResetAllFields('assetspendinginvoice-popup');
            });
            
            //for ipad
              $('.invoicepostbtn').bind("touchend",function () {
                $('#<%= hfInvoiceID.ClientID %>').val("0");
                ShowPopUp('assetspendinginvoice-popup', 'assetspendinginvoice-popup .assetspoupup-content','ipadclick');
                ResetAllFields('assetspendinginvoice-popup');
                //temp change to test in iPad
                $(".fade-layer").hide();
            });
            
            $("#<%= ddlAccountingAcquisitionMethod.ClientID %>").bind("change",function(){
                $("#<%= ddlAccountingAcquisitionMethod.ClientID %>").find("option").filter(function () {
                    if(this.value == $("#<%= ddlAccountingAcquisitionMethod.ClientID %>").val())
                    {
                        if(this.text.toLowerCase() == "lease asset")
                        {
                            $(".purchaseinfodiv").attr("style","display:none;"); 
                            ClearUploadedFiles("purchaseinfodiv");
                            $(".leaseinfodiv").removeAttr("style");
                            $(".purchaseinfodiv").addClass("closeDiv").removeClass("setOpen");
                            $(".leaseinfodiv").removeClass("closeDiv").addClass("setOpen");
                            $("#<%= hdnFileFor.ClientID %>").val("Lease");
                        }
                        else
                        {
                            $(".leaseinfodiv").attr("style","display:none;");
                            $(".purchaseinfodiv").removeAttr("style");
                            ClearUploadedFiles("leaseinfodiv");
                            $(".leaseinfodiv").addClass("closeDiv").removeClass("setOpen");
                            $(".purchaseinfodiv").removeClass("closeDiv").addClass("setOpen");
                            $("#<%= hdnFileFor.ClientID %>").val("Purchase");
                        }
                    }
                 });
                 SetOpenDivIDsToHiddenFields();
                 SetCloseDivIDsToHiddenFields();
                 return false;
                 
            });
        }
        
        function ClearUploadedFiles(dvClassName)
        {
            $("." + dvClassName).find("div.multifilediv").find("div.clientside").each(function(){
                 $(this).find("a.MultiFile-remove").click();
            });
        }
        
        function BindWarrantyTab_PopUpEvents() {
            $('.warrantyheader .small-blue-btn').click(function () {
                ShowPopUp('new-warranty-popup', 'new-warranty-popup .warranty-content','newwarranty');
                ResetAllFields('new-warranty-popup');
            });

            $('.claimpost a.claimpostbtn').click(function () {
                ShowPopUp('post-warranty-popup', 'post-warranty-popup .warranty-content','postclaim');
                ResetAllFields('post-warranty-popup');
            });
            //claim-addnote-popup;claim-view-notes-popup
             $('.claimaddpopup').click(function () {
                ShowPopUp('claim-addnote-popup', 'claim-addnote-popup .warranty-content','addclaimnote');
                ResetAllFields('claim-addnote-popup');
                $('#<%=  hfWarrantyClaimID.ClientID %>').val($(this).attr("data-ID"));
            });

            BindPopUpCloseEvents();
        }
        
        function OpenViewNotes(Ctrl,_msg,CtrlID,event,IsCssClassNameForCtrlID)
        {
            if($(".ctrlinfochanged").val().trim().toLowerCase() == "true")
            { 
                GeneralConfirmationMsgForRecordSave(_msg,CtrlID,event,IsCssClassNameForCtrlID);
            }
            else
            {
               ShowPopUp('view-note-popup', 'view-note-popup .warranty-content','viewnote');
               ResetAllFields('view-note-popup');
               $(".MainDiv").val("view-note-popup");
            }
            BindPopUpCloseEvents(true,_msg,CtrlID,event,IsCssClassNameForCtrlID);
        }
        function ShowAddNotePopup(Ctrl,_msg,CtrlID,event,IsCssClassNameForCtrlID)
        {
            
            if($(".ctrlinfochanged").val().trim().toLowerCase() == "true")
            { 
                GeneralConfirmationMsgForRecordSave(_msg,CtrlID,event,IsCssClassNameForCtrlID);
            }
            else
            {
                if($(Ctrl).hasClass("asset-flag-off"))
                {
                    ShowPopUp('add-note-popup', 'add-note-popup .warranty-content','addnote');
                    ResetAllFields('add-note-popup');
                    $(".MainDiv").val("add-note-popup");
                }
                else
                    $("#<%= lnkbtnAssetsUnflagged.ClientID %>").click();
            }
            //BindPopUpCloseEvents(true,_msg,CtrlID,event,IsCssClassNameForCtrlID);
        }
        function ShowAddNewFieldPopUp(Ctrl,_msg,CtrlID,event,IsCssClassNameForCtrlID)
        {
            if($(".ctrlinfochanged").val().trim().toLowerCase() == "true")
            { 
                GeneralConfirmationMsgForRecordSave(_msg,CtrlID,event,IsCssClassNameForCtrlID);
            }
            else
            {
                $("#<%= hfSectionName.ClientID %>").val($(Ctrl).attr("data-section"));
                $(".MainDiv").val("add-newfield-popup");
                ShowPopUp("add-newfield-popup", "add-newfield-popup .warranty-content",'addnewfield');
                ResetAllFields("add-newfield-popup");
                $(".fade-layer").hide();
            }
            
        }
        function ShowInvoicePopUp(_msg,CtrlID,event,IsCssClassNameForCtrlID)
        {
            if($(".ctrlinfochanged").val().trim().toLowerCase() == "true")
            { 
                GeneralConfirmationMsgForRecordSave(_msg,CtrlID,event,IsCssClassNameForCtrlID);
            }
            else
            {
                $(".MainDiv").val("assetspendinginvoice-popup");
                $('#<%= hfInvoiceID.ClientID %>').val("0");
                ShowPopUp('assetspendinginvoice-popup', 'assetspendinginvoice-popup .assetspoupup-content','AddInvoice');
                ResetAllFields('assetspendinginvoice-popup');
            }
        }
        function ClosePopUp(Ctrl,event,CtrlIDorClassName,IsClientSideCtrl)
        {
            if($(".ctrlinfochanged").val().trim().toLowerCase() == "true")
            { 
                GeneralConfirmationMsgForRecordSave('Do you want to save your changes',CtrlIDorClassName,event,IsClientSideCtrl);
            }
            else
            {
                $('.popup-outer').css('top', '-9999px');
                $('.fade-layer').hide();
               // if(typeof(CheckChange) != "undefined" || $(".ctrlinfochanged").val().trim().toLowerCase() == "false")
                //{
                    var CurrentMainDiv = $(".ParentTab").filter(function() { if( $(this).css("display") && $(this).css("display") != "none") return this; });
                    $(".MainDiv").val(CurrentMainDiv.attr("id"));
                    $(".savebuttontotrigger").val($("#" + $(".MainDiv").val()).attr("data-defaultbutton"));
                //}
            }
            
        }
        function BindSpecsTab_PopUpEvents() {
            
//            $('.add-field').click(function () {
//                alet(345);
//                
//            });
            
            //BindPopUpCloseEvents();
        }

        function ShowPopUp(MainDivTargetID, PopUpDivTargetID,PopUpFor) {
            if(PopUpFor)
            {
                $("#" + MainDivTargetID).css('top', '0');
                $("#" + PopUpDivTargetID).show();
                $(".fade-layer").show();
                $(".popup-asset-title").html($(".mainAssetTitle").html() + " - ");
                SetPopUpAtTop();
            }
        }

        function SetWarrantyIDToClaim(WarrantyID, RowIndex) {
            $("#<%= hfWarrantyIDForClaim.ClientID %>").val(WarrantyID);
            $("#<%= hfRowIndex.ClientID %>").val(RowIndex);
        }
      
        function SetDatePicker() {
            $(".setDatePicker").datepicker({
                changeMonth: true,
                changeYear: true
            }).on("focus",SelectMe(this)).attr("readonly","readonly");
        }
        function CalculateDepreciation()
        {
            var PurchasedPrice = $("#<%= txtAccountingPurchasePrice.ClientID %>").val().replace('$','').trim().replace(/,/g,'').trim();
            var DepreciationInYears = $("#<%= txtAccountingDepreciation.ClientID %>").val();
            var YearlyDepreciation = 0;
            
            if(PurchasedPrice != "" && DepreciationInYears != "" && IsDecimal(PurchasedPrice) && IsNumeric(DepreciationInYears))
            {
                YearlyDepreciation = (parseFloat(PurchasedPrice) / parseInt(DepreciationInYears)).toFixed(2);
                $("#<%= txtAccountingYearlyDepreciation.ClientID %>").val("$ " + YearlyDepreciation).autoNumeric('update');
            }
        }
        
        function CalculateTotalCost()
        {
            var MonthlyBasePayment = $("#<%= txtAccountingMonthlyBasePayment.ClientID %>").val().replace('$','').trim().replace(/,/g,'').trim().trim();
            var MaintenancePlanCost = $("#<%= txtAccountingMaintenancePlanCost.ClientID %>").val().replace('$','').trim().replace(/,/g,'').trim().trim();
            var SalesTax = $("#<%= txtAccountingSalesTax.ClientID %>").val();
            var TotalCost = 0
            if(MonthlyBasePayment != "" && MaintenancePlanCost != "" && SalesTax != "" && IsDecimal(MonthlyBasePayment) && IsDecimal(MaintenancePlanCost) && IsDecimal(SalesTax))
            {
                TotalCost = parseFloat(parseFloat(MonthlyBasePayment) + parseFloat(MaintenancePlanCost) + (parseFloat(MonthlyBasePayment) + parseFloat(MaintenancePlanCost)) * SalesTax / 100).toFixed(2);
                $("#<%= txtAccountingTotalMonthlyCost.ClientID %>").val("$ " + TotalCost).autoNumeric('update');
            }
        }
        
        
        
        function ToggleDiv(Classname) {
            $("." + Classname).slideToggle('slow');
        }
        
        function OpenFileDialog(ClassName) {
            $("." + ClassName).click();
        }
        
        function SetActiveLinks(UlClassNameToRemove,ActiveLinkID)
        {
            $("." + UlClassNameToRemove+ " li a").filter(function() { if(this.id != ActiveLinkID) return this;}).removeClass("active");
        }
        
        function SendMailPopUp(FileName,MailSubject,SectionName,FileFor,FileID)
        {
            $("#<%= hfSubject.ClientID %>").val(MailSubject);
            $("#<%= hfAssetFiles.ClientID %>").val(FileName);
            $("#<%= hfFileFor.ClientID %>").val(FileFor);
            $("#<%= hfFileID.ClientID %>").val(FileID);
            $("#<%= hfSectionName.ClientID %>").val(SectionName);
            ShowPopUp('sendmail-popup', 'sendmail-popup .warranty-content','sendmail');
            ResetAllFields("sendmail-popup");
        }
        
        function CheckNameAlreadyExists(sender,args)
        {
            
            var TextBoxCtrl = $("#<%= txtFileName.ClientID %>");
            var strNames = $("." + $("#<%= hfFileNamesClass.ClientID %>").val());
            var CurrValue = $(TextBoxCtrl).val().trim();
            if(CurrValue != "")
            {
                $(strNames).each(function(){
                    if(CurrValue == $(this).html())
                         args.IsValid = false;
                    else
                         args.IsValid = true;
                });
            }
            else
                 args.IsValid = false;
        }
        
        function ShowFileUploadPopUp(FileFor,FileNameClass,FileType)
        {
            $("#<%= hfFileNamesClass.ClientID %>").val(FileNameClass);
            ShowPopUp('Upload-File-popup', 'Upload-File-popup .warranty-content','fileupload');
            ResetAllFields("Upload-File-popup");
            
            var ImageValidatorObject = $("#<%= revAssetImages.ClientID %>");
            ImageValidatorObject.isvalid = true;
            
            var DocumentValidatorObject = $("#<%= revAssetDocuments.ClientID %>");
            DocumentValidatorObject.isvalid = true;
             //<span class="fileformate" id="format_span"></span>
            if(FileType.toLowerCase() != 'document')
            {
                $("#format_span").html("Only Image and Video(mp4) files allowed.");
                 ImageValidatorObject.each(function () {
                    this.enabled = true;
                 });
                 
                 DocumentValidatorObject.each(function () {
                    this.enabled = false;
                 });
            }
            else
            {
                $("#format_span").html("Only PDF, Word Doc files are allowed. Not available on mobile devices.");
                 ImageValidatorObject.each(function () {
                    this.enabled = false;
                 });
                 
                 DocumentValidatorObject.each(function () {
                    this.enabled = true;
                 });
               
            }
        }
       
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <input type="hidden" value="admin-link" id="hdnActiveLink" />
    <!-- value='#' is kept intentionally, please do not remove that-->
    <input type="hidden" value="#" id="hdnOpenDivs" runat="server" />
    <!-- value='#' is kept intentionally, please do not remove that-->
    <input type="hidden" value="#" id="hdnCloseDivs" runat="server" />
    <section id="container" class="cf basic-page assetSection">
    
    <div class="filter-content">
    
       <div class="filter-headbar cf">
            <span class="headbar-title mainAssetTitle" id="title_span" runat="server">Asset Name / Add New Asset</span> 
            
             <div class="asset-bar alignleft" id="flag_li" runat="server" visible="false">
               <a id="lnkFlagAsset" class="asset-flag-off clientsidectrl" runat="server"><span>Flag this Asset</span></a>
               <a id="lnkViewNotes" runat="server" href="javascript:void(0);" class="generate-btn view-note clientsidectrl" title="VIEW NOTES">VIEW NOTES</a>
               <a href="javascript:void(0);" class="generate-btn add-note" title="ADD A NOTE" style="display:none;">ADD A NOTE</a>
            </div>
            
            <a href="javascript:void(0);" onclick="GetHelpVideo('Asset Management','Add New Asset');" title="Help Video" class="help-videolink" onclick="ClearFileUploadControl();">Help Video</a>
        </div>
        
       <ul class="order-links cf" id="ul_NewAsset" runat="server">
            <li class="first">
                <asp:LinkButton ID="lnkbtnBasic" runat="server" CommandArgument="0" OnClick="lnkbtnChangeTab_Click" CommandName="order-links"
                    ToolTip="Basic" data-divID="dvBasic"><em></em>Basic</asp:LinkButton></li>
            <li>
                <asp:LinkButton ID="lnkbtnAccounting" runat="server" CommandArgument="1" OnClick="lnkbtnChangeTab_Click"  CommandName="order-links"
                    ToolTip="Accounting" data-divID="dvAccounting"><em></em>Accounting</asp:LinkButton>
                     
                    </li>
            <li>
                <asp:LinkButton ID="lnkbtnWarranty" runat="server" CommandArgument="2" OnClick="lnkbtnChangeTab_Click" CommandName="order-links"
                    ToolTip="Warranty" data-divID="WarrantyTableBlock"><em></em>Warranty</asp:LinkButton></li>
            <li>
                <asp:LinkButton ID="lnkbtnSpecs" runat="server" CommandArgument="3" OnClick="lnkbtnChangeTab_Click" CommandName="order-links"
                    ToolTip="Specs" data-divID="dvSpecification"><em></em>Specs</asp:LinkButton></li>
            <li>
                <asp:LinkButton ID="lnkbtnRegistration" runat="server" CommandArgument="4" OnClick="lnkbtnChangeTab_Click" CommandName="order-links"
                    ToolTip="Registration"><em></em>Registration</asp:LinkButton></li>
            <li class="last">
                <asp:LinkButton ID="lnkbtnHistory" runat="server" CommandArgument="5" OnClick="lnkbtnChangeTab_Click" CommandName="order-links"
                    ToolTip="History"><em></em>History</asp:LinkButton></li>
        </ul>
        
       <%--<div id="msg_div" runat="server" visible="false" style="text-align:center;padding:8px;">
                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
        </div>--%>
        
       <asp:MultiView ID="mvAddNewAsset" runat="server" ActiveViewIndex="0" OnActiveViewChanged="mvAddNewAsset_ActiveViewChanged">
            <asp:View ID="vBasic" runat="server">
                <div id="dvBasic" class="account-form-block ParentTab" data-defaultbutton="<%= btnBasicSave.ClientID %>">
                    <div class="account-formInner">
                        <div class="basic-form" method="post">
                            <div class="cf">
                                <ul class="left-form cf">
                                 <li class="alignleft" tabindex="1"><span class="lbl-txt">Asset Type</span>
                                 
                                 <span id="basic_AssetType_parent_span"
                                        runat="server" class="select-drop basic-drop">
                                        
                                        <cdd:CustomDropDown  ID="ddlBasicAssetType" runat="server" DropDownCssClass="default checkvalidation"
                                            TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop basic-drop" Module="lookup"
                                            OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                            GroupName="EquipmentType" DefaultOptionText="Asset Type" />
                                        <asp:RequiredFieldValidator ID="rfvTemp" runat="server" ErrorMessage="Please select Type" Display="Dynamic" InitialValue="0" CssClass="error"
                                            ControlToValidate="ddlBasicAssetType$ddlDropDown" ValidationGroup="BasicTabValidate" SetFocusOnError="True"> </asp:RequiredFieldValidator>   
                                    </span></li>
                                    <li class="alignright" TabIndex="2">
                                        <label>
                                            <span class="lbl-txt">Plate</span>
                                            <asp:TextBox ID="txtBasicPlate" runat="server" CssClass="input-field-all checkvalidation"></asp:TextBox>
                                        </label>
                                    </li>
                                    <li class="alignleft" TabIndex="3"><span class="lbl-txt">Manufacturer</span><span id="basic_manufacturer_parent_span"
                                        runat="server" class="select-drop basic-drop">
                                        <cdd:CustomDropDown ID="ddlBasicManufacturer" runat="server" DropDownCssClass="default"
                                            TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop basic-drop" Module="lookup"
                                            OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                            GroupName="Brand" DefaultOptionText="Manufacturer" />
                                        
                                    </span></li>
                                    <li class="alignright" TabIndex="4"><span class="lbl-txt">Model</span><span id="model_parent_span"
                                        runat="server" class="select-drop basic-drop">
                                        <cdd:CustomDropDown ID="ddlBasicModel" runat="server" DropDownCssClass="default"
                                            TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop basic-drop" Module="Equipmentlookup"
                                            OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                            GroupName="EquipmentModel" DefaultOptionText="Model" />
                                        
                                    </span></li>
                                    <li class="alignleft" TabIndex="5">
                                        <label>
                                            <span class="lbl-txt">Serial #</span>
                                            <asp:TextBox ID="txtBasicSerialNumber" runat="server" CssClass="input-field-all"></asp:TextBox>
                                        </label>
                                    </li>
                                    <li class="alignright" TabIndex="6"><span class="lbl-txt">Location</span>
                                    <span class="select-drop basic-drop">
                                        <asp:DropDownList ID="ddlBasicLocation" runat="server" class="checkvalidation default"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rqLocation" runat="server" ControlToValidate="ddlBasicLocation"
                                         Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" ValidationGroup="BasicTabValidate"
                                         ErrorMessage="Please select Location"></asp:RequiredFieldValidator>
                                    </span>
                                    </li>
                                    <li class="alignleft asset-gen-btn" TabIndex="7">
                                        <label>
                                            <span class="lbl-txt">Asset ID</span>
                                            <asp:TextBox ID="txtBasicAssetID" runat="server" CssClass="input-field-all checkvalidation"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqAssetID" runat="server" ControlToValidate="txtBasicAssetID"
                                         Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="BasicTabValidate"
                                         ErrorMessage="Please enter assetid"></asp:RequiredFieldValidator>
                                        </label>
                                        <a href="javascript:void(0);" class="generate-btn" title="AUTO-GENERATE" TabIndex="8">AUTO-GENERATE</a></li>
                                    <li class="alignright" TabIndex="9">
                                        <label>
                                        <span class="lbl-txt1">Manufacturing
                                        <br>
                                        Date</span>
                                        <asp:TextBox ID="txtManufacturingDate" runat="server" CssClass="input-field-all setDatePicker" ></asp:TextBox>
                                        </label>
                                    </li>
                                    <li class="alignleft row" id="SubFields">
                                        <ul id="ul_basicfields" runat="server" class="add-new-field-block CustomFields" enableviewstate="false"></ul>
                                    </li>
                                    <li class="cf">&nbsp;</li>    
                                </ul>
                            </div>
                        </div>
                    </div>
					<div class="asset-btn-block assetbtn-blockbar cf">
                        <a id="btnBasicCancel" class="small-gray-btn cancelasset" runat="server" href="Assets.aspx?se=add"><span>Cancel</span></a> 
                        <asp:LinkButton ID="btnBasicSave" CssClass="small-blue-btn submit" runat="server" OnClick="btnBasicSave_Click" ValidationGroup="BasicTabValidate" call="BasicTabValidate"><span>SAVE</span></asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnBasicClose" runat="server" CssClass="small-gray-btn" ToolTip="Close" OnClick="lnkbtnClose_Click"><span>Close</span></asp:LinkButton>
                        
                    </div>
                </div>
            </asp:View>
            <asp:View ID="vAccounting" runat="server">
            
                <div id="dvAccounting" class="account-form-block ParentTab" data-defaultbutton="<%= lnkbtnAccountingSave.ClientID %>">
                    <div class="account-formInner">
                        <div class="basic-form">
                            <div class="cf">
                                <ul class="left-form cf">
                                    <li class="alignleft" TabIndex="1">
                                      <label>
                                        <span class="lbl-txt">Acquisition Date</span><asp:TextBox ID="txtAccountingAcquisitonDate" runat="server" CssClass="input-field-all setDatePicker" ></asp:TextBox>
                                        </label></li>
                                    <li class="alignright" TabIndex="2"><span class="lbl-txt">Purchased From</span><span id="purchase_from_parent_span"
                                        runat="server" class="select-drop basic-drop">
                                        <cdd:CustomDropDown ID="ddlAccountingPurchasedFrom" runat="server" DropDownCssClass="default"
                                            TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop basic-drop" Module="lookup"
                                            OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                            GroupName="PurchasedFrom" DefaultOptionText="Purchased From"/>
                                    </span></li>
                                    <li class="alignleft acq-method" TabIndex="3"><span class="lbl-txt1">Acquisition Method</span>
                                        <span class="select-drop basic-drop">
                                        <asp:DropDownList id="ddlAccountingAcquisitionMethod" runat="server" CssClass="default"></asp:DropDownList>
                                        <%--<cdd:CustomDropDown ID="ddlAccountingAcquisitionMethod" runat="server" DropDownCssClass="default"
                                            TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop basic-drop" Module="Equipmentlookup"
                                            OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                            GroupName="PurchaseMethod" />--%>
                                        </span>
                                    </li>
                                    <li class="alignright" TabIndex="4"><span class="lbl-txt">Condition</span><span id="condition_parent_span"
                                        runat="server" class="select-drop basic-drop">
                                        <cdd:CustomDropDown ID="ddlAccountingConditions" runat="server" DropDownCssClass="default"
                                            TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop basic-drop" Module="Equipmentlookup"
                                            OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                            GroupName="NewOrRefurbished" DefaultOptionText="Condition" />
                                    </span></li>
                                    <li class="alignleft" TabIndex="5"><span class="lbl-txt">Asset Life</span><span id="AssetLife_parent_span"
                                        runat="server" class="select-drop basic-drop">
                                        <cdd:CustomDropDown ID="ddlAccountingAssetLife" runat="server" DropDownCssClass="default"
                                            TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop basic-drop" Module="Equipmentlookup"
                                            OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                            GroupName="EquipmentLife" DefaultOptionText="Asset Life" />
                                    </span></li>
                                    <li class="alignright" TabIndex="6"><span class="lbl-txt">Department</span><span id="Departments_parent_span"
                                        runat="server" class="select-drop basic-drop">
                                        <cdd:CustomDropDown ID="ddlAccountingDepartments" runat="server" DropDownCssClass="default"
                                            TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop basic-drop" Module="lookup"
                                            OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                            GroupName="GSEDepartment" DefaultOptionText="Department" />
                                    </span></li>
                                    <li class="alignleft" TabIndex="7"><span class="lbl-txt">Cost Center</span><span id="CostCenter_parent_span"
                                        runat="server" class="select-drop basic-drop">
                                         <cdd:CustomDropDown ID="ddlAccountingCostCenter" runat="server" DropDownCssClass="default"
                                            TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop basic-drop" Module="Equipmentlookup"
                                            OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                            GroupName="CostCenterCode" DefaultOptionText="Cost Center" />
                                    </span></li>
                                    <li class="alignright row" id="Accounting_SubFields">
                                        <ul id="ul_AccountingFields" runat="server" enableviewstate="false">
                                        </ul>
                                    </li>
                                </ul>
                                <div id="purchase_info_div" runat="server" class="purchaseinfodiv">
                                    <div id="dvAccountingPurchaseInfo" class="asset-title parentdiv">
                                        PURCHASE ASSET INFORMATION <span class="down-arrow">&nbsp;</span></div>
                                    <ul class="left-form cf childdiv" style="display:none;">
                                        <li class="alignleft" TabIndex="8">
                                            <label>
                                                <span class="lbl-txt">Purchase Price</span>
                                                <asp:TextBox ID="txtAccountingPurchasePrice" runat="server" CssClass="input-field-all pricetext" data-a-sign="$ " onkeypress="return AllowNumbersONLY(event);" onkeyup="CalculateDepreciation();"></asp:TextBox></label></li>
                                        <li class="alignright"><span class="lbl-txt">Depreciation</span>
                                        <asp:TextBox ID="txtAccountingDepreciation" runat="server" CssClass="input-field-year checkvalidation"
                                            MaxLength="4" onkeypress="return AllowNumbersONLY(event);"  onkeyup="CalculateDepreciation();"></asp:TextBox><span class="lbl-txt">Years</span>
                                        <%--<asp:RangeValidator ID="rvDepreciation" runat="server" ControlToValidate="txtAccountingDepreciation" Display="Dynamic"
                                        ErrorMessage="Please enter numeric value only" CssClass="error" Type="Double" ValidationGroup="AccountingTabGroup"></asp:RangeValidator>--%>
                                        </li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt1">Yearly Depreciation</span>
                                                <asp:TextBox ID="txtAccountingYearlyDepreciation" runat="server" TabIndex="2" CssClass="input-field-all pricetext" data-a-sign="$ "></asp:TextBox></label></li>
                                    </ul>
                                    <div id="dvAccountingPurchasefile" class="asset-title parentdiv">
                                        FILES<span class="down-arrow">&nbsp;</span></div>
                                       <div class="childdiv" style="display:none;">
                                        <ul class="left-form cf" >
                                            <li class="alignleft"><span class="lbl-txt1">Purchase<br>
                                                Agreement</span>
                                                     <%--<div class="upload-file">
                                                                
                                                                <asp:FileUpload ID="fuPurchaseAgreement" runat="server" class="fileU multi filupCtrl checkvalidation" type="file" accept="pdf|PDF|doc|DOC|docx|DOCX" allowedextensionmsg="pdf,doc and docx"/>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only PDF/pdf file allowed."
                                                                 CssClass="error" ControlToValidate="fuPurchaseAgreement" Display="None" SetFocusOnError="False"
                                                                 ValidationGroup="AccountingTabGroup" ValidationExpression="^.*\.(pdf|PDF|doc|DOC|docx|DOCX)$"></asp:RegularExpressionValidator>
                                                      </div>--%>
                                                      <a href="javascript:void(0);" onclick="ShowFileUploadPopUp('Purchase','PurchaseFileName','Document');" ontouchstart="ShowFileUploadPopUp('Purchase','PurchaseFileName','Document');" class="btn-uploadfile">Upload File</a>
                                                     
                                            </li>
                                        </ul>
                                        <div class="multifilediv">
                                            <div id="dvPurchaseAgreement" runat="server"  class="asset-img-block imagesblock">
                                                <asp:Repeater ID="rpPurchaseAgreement" runat="server" OnItemDataBound="rpAccountingFiles_OnItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="asset-image">
                                                            <div class="assetdoc-innerimage">
                                                                 <span id="lblPurchaseFileType" class="PurchaseFileType" style="display:none;" runat="server"><%# Convert.ToString(Eval("FileType")) %></span> 
                                                                 <span id="lblPurchaseFileName" class="PurchaseFileName" style="display:none;" runat="server"><%# Convert.ToString(Eval("FileTitle"))%></span> 
                                                                <a href="javascript:void(0);">
                                                                     <img src="../StaticContents/img/file-ico.png" width="79" height="79" alt='<%# Eval("FileTitle") %>' title='<%# Eval("FileTitle") %>'>
                                                                        <ul class="asset-imagehover cf">
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkbtnDownload" runat="server" OnClick="lnkbtnDownload_Click" Text="Download" CommandArgument='<%# "UploadedImages/AssetManagement/" +Eval("EquipmentMasterID")+ "/Accounting/Purchase/" +  Eval("Name") %>' CommandName='<%# Eval("FileTitle") %>'>
                                                                                <img src="../StaticContents/img/asset-ico2.png" width="20" height="20"></asp:LinkButton>
                                                                            </li>
                                                                            <li><asp:LinkButton ID="lnkbtnDelete" runat="server" OnClick="lnkbtnDeleteFile_Click"  CommandName='<%# "UploadedImages/AssetManagement/" +Eval("EquipmentMasterID")+ "/Accounting/Purchase/" +  Eval("Name") %>' CommandArgument='<%# Eval("EquipmentFileID") %>'>
                                                                                <img src="../StaticContents/img/asset-ico3.png" width="20" height="20"></asp:LinkButton>
                                                                                </li>                                     
                                                                            <li><a href="javascript: void(0);" onclick="SendMailPopUp('<%# "UploadedImages/AssetManagement/" +Eval("EquipmentMasterID")+ "/Accounting/Purchase/" +  Eval("Name") %>','Asset Purchase Information File','Purchase','PurchaseAsset',<%# Eval("EquipmentFileID") %>);" ontouchstart="SendMailPopUp('<%# "UploadedImages/AssetManagement/" +Eval("EquipmentMasterID")+ "/Accounting/Purchase/" +  Eval("Name") %>','Asset Purchase Information File','Purchase','PurchaseAsset',<%# Eval("EquipmentFileID") %>);">
                                                                                <img src="../StaticContents/img/asset-ico1.png" width="20" height="20"></a></li>
                                                                        </ul>
                                                                </a>
                                                            </div>
                                                            
                                                            <strong><%# Eval("FileTitle") %></strong>
                                                        </div>
                                                    </ItemTemplate>  
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                        </div>
                                </div>
                                <div id="lease_info_div" runat="server" class="leaseinfodiv cf" >
                                    <div id="dvAccountingLeaseInfo" class="asset-title parentdiv">
                                        LEASE ASSET INFORMATION<span class="down-arrow">&nbsp;</span></div>
                                    <ul class="left-form cf childdiv" style="display:none;">
                                      <li class="alignleft"><span class="lbl-txt">Operating Lease</span><span class="select-drop basic-drop" id="operating_lease_parent_span"
                                                runat="server">
                                                <cdd:CustomDropDown ID="ddlAccountingOperatingLease" runat="server" DropDownCssClass="default"
                                                    TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop basic-drop" Module="lookup"
                                                    OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                                    GroupName="OperatingLease" DefaultOptionText="Operating Lease" />
                                            </span></li>
                                        <li class="alignright"><span class="lbl-txt">Lease Terms</span><span id="Lease_terms_parent_span"
                                            runat="server" class="select-drop basic-drop">
                                            <cdd:CustomDropDown ID="ddlAccountingLeaseTerms" runat="server" DropDownCssClass="default"
                                                TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop basic-drop" Module="lookup"
                                                OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                                GroupName="LeaseTerms" DefaultOptionText="Lease Terms" />
                                        </span></li>
                                    <li class="alignleft" TabIndex="13">
                                        <label>
                                            <span class="lbl-txt1">Monthly Base Payment</span>
                                            <asp:TextBox ID="txtAccountingMonthlyBasePayment" runat="server" CssClass="input-field-all pricetext" data-a-sign="$ " onkeypress="return AllowNumbersONLY(event);"  onkeyup="CalculateTotalCost();"></asp:TextBox>
                                        </label>
                                    </li>
                                    <li class="alignright">
                                        <label>
                                            <span class="lbl-txt1">Maintenance Plan Cost</span>
                                            <asp:TextBox ID="txtAccountingMaintenancePlanCost" runat="server" CssClass="input-field-all pricetext" data-a-sign="$ " onkeypress="return AllowNumbersONLY(event);"  onkeyup="CalculateTotalCost();"></asp:TextBox>
                                        </label>
                                    </li>
                                    <li class="alignleft" TabIndex="14">
                                        <label>
                                            <span class="lbl-txt">Sales Tax(%)</span>
                                            <asp:TextBox ID="txtAccountingSalesTax" runat="server" MaxLength="3" CssClass="input-field-all" onkeypress="return AllowNumbersONLY(event);"  onkeyup="CalculateTotalCost();"></asp:TextBox>
                                        </label>
                                    </li>
                                    <li class="alignright TabIndex="15"">
                                        <label>
                                            <span class="lbl-txt1">Total Monthly Cost</span>
                                            <asp:TextBox ID="txtAccountingTotalMonthlyCost" runat="server" CssClass="input-field-all pricetext" data-a-sign="$ "></asp:TextBox>
                                        </label>
                                    </li>
                                </ul>
                                    <div id="dvAccountingLeaseFiles" class="asset-title parentdiv">
                                        FILES<span class="down-arrow">&nbsp;</span></div>
                                          <div class="childdiv" style="display:none;">
                                        <ul class="left-form cf " >
                                            <li class="alignleft"><span class="lbl-txt1">Lease<br>
                                                Agreement</span>
                                                    <%--<div class="upload-file">
                                                     <asp:FileUpload ID="fuLeaseAgreement" runat="server" class="fileU multi filupCtrl checkvalidation" type="file" accept="pdf|PDF|doc|DOC|docx|DOCX" allowedextensionmsg="pdf,doc and docx"/>
                                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only PDF/pdf file allowed."
                                                     CssClass="error" ControlToValidate="fuLeaseAgreement" Display="None" SetFocusOnError="False"
                                                     ValidationGroup="AccountingValidation" ValidationExpression="^.*\.(pdf|PDF|doc|DOC|docx|DOCX)$"></asp:RegularExpressionValidator>
                                                     </div>--%>
                                                     <a class="btn-uploadfile" href="javascript:void(0);" onclick="ShowFileUploadPopUp('Lease','LeaseFileName','Document');" ontouchstart="ShowFileUploadPopUp('Lease','LeaseFileName','Document');">Upload File</a>
                                                
                                            </li>
                                    </ul>
                                       <div class="multifilediv">
                                                <div id="dvLeaseAgreement" runat="server"  class="asset-img-block imagesblock">
                                                    <asp:Repeater ID="rpLeaseFiles" runat="server">
                                                        <ItemTemplate>
                                                            <div class="asset-image">
                                                                <div class="assetdoc-innerimage">
                                                                     <span id="lblLeaseFileType" class="LeaseFileType" style="display:none;" runat="server"><%# Convert.ToString(Eval("FileType")) %></span> 
                                                                     <span id="lblLeaseFileName" class="LeaseFileName" style="display:none;" runat="server"><%# Convert.ToString(Eval("FileTitle"))%></span> 
                                                                    <a href="javascript:void(0);">
                                                                         <img src="../StaticContents/img/file-ico.png" width="79" height="79" alt='<%# Eval("FileTitle") %>' title='<%# Eval("FileTitle") %>'>
                                                                            <ul class="asset-imagehover cf">
                                                                                <li>
                                                                                    <asp:LinkButton ID="lnkbtnDownload" runat="server" OnClick="lnkbtnDownload_Click" Text="Download" CommandArgument='<%# "UploadedImages/AssetManagement/" + Eval("EquipmentMasterID") +"/Accounting/Lease/" +  Eval("Name") %>' CommandName='<%# Eval("FileTitle") %>'>
                                                                                    <img src="../StaticContents/img/asset-ico2.png" width="20" height="20"></asp:LinkButton>
                                                                                </li>
                                                                                <li><asp:LinkButton ID="lnkbtnDelete" runat="server" OnClientClick="return GeneralConfirmationMsgForDelete('Are you sure you want to delete this file?','<%# lnkbtnDeleteInvoice.ClientID %>',event);" OnClick="lnkbtnDeleteFile_Click"  CommandName='<%# "UploadedImages/AssetManagement/" + Eval("EquipmentMasterID") +"/Accounting/Lease/" +  Eval("Name") %>' CommandArgument='<%# Eval("EquipmentFileID") %>'>
                                                                                    <img src="../StaticContents/img/asset-ico3.png" width="20" height="20"></asp:LinkButton>
                                                                                    </li>
                                                                                <li><a href="javascript: void(0);" onclick="SendMailPopUp('<%# "UploadedImages/AssetManagement/" + Eval("EquipmentMasterID") +"/Accounting/Lease/" +  Eval("Name") %>','Asset Lease Information File','Lease','LeaseAsset',<%# Eval("EquipmentFileID") %>);" ontouchstart="SendMailPopUp('<%# "UploadedImages/AssetManagement/" + Eval("EquipmentMasterID") +"/Accounting/Lease/" +  Eval("Name") %>','Asset Lease Information File','Lease','LeaseAsset',<%# Eval("EquipmentFileID") %>);">
                                                                                    <img src="../StaticContents/img/asset-ico1.png" width="20" height="20"></a></li>
                                                                            </ul>
                                                                    </a>
                                                                </div>
                                                                
                                                                <strong><%# Eval("FileTitle") %></strong>
                                                            </div>
                                                        </ItemTemplate>  
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                    </div>
                                </div>
                                <div id="dvAccountingInvoice" class="asset-title parentdiv">
                                    INVOICES<span class="down-arrow">&nbsp;</span></div>
                                <div class="childdiv" id="AccountingInvoice" style="display:none;">
                                <div class="basic-invoice-table-block">
                                    <asp:GridView ID="gvEquipment" runat="server" AutoGenerateColumns="false" GridLines="None"
                                        OnRowDataBound="gvEquipment_RowDataBound" CssClass="table-grid" ShowFooter="True" OnRowCommand="gvEquipment_RowCommand">
                                        <Columns>
                                            <asp:TemplateField SortExpression="DateofService">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnDateofService" runat="server" CommandArgument="DateofService"
                                                        CommandName="Sort"><span >Date of Service</span></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hfDateofService" runat="server" Value='<%# Eval("DateofService")%>' />
                                                    <asp:Label runat="server" ID="lblDateofService" Text='<%# Eval("DateofService", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col1" />
                                                <ItemStyle CssClass="col1" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtVendor" runat="server" CommandArgument="Vendor" CommandName="Sort"><span>Vendor</span></asp:LinkButton>
                                                    <div class="corner">
                                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl centeralign"></span>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hfVendor" runat="server" Value='<%# Eval("Vendor")%>' />
                                                    <asp:LinkButton ID="lnkVendor" CommandArgument='<%# Eval("EquipmentMaintenanceCostID") %>'
                                                        CommandName="Vendor" runat="server">
                                    <span><%# Eval("EquipmentVendorName")%></span>
                                                    </asp:LinkButton>
                                                    <asp:Label runat="server" ID="lblEquipmentMasterID" Text='<%# Eval("EquipmentMasterID") %>'
                                                        Visible="false" />
                                                    <asp:Label runat="server" ID="lblEquipmentID" Text='<%# Eval("EquipmentMaintenanceCostID") %>'
                                                        Visible="false" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col2" />
                                                <ItemStyle CssClass="col2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="LaborAmount">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnLaborAmount" runat="server" CommandArgument="LaborAmount"
                                                        CommandName="Sort"><span >Labor Amt</span></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hfLaborAmount" runat="server" Value='<%# Eval("LaborAmount")%>' />
                                                    <asp:Label runat="server" ID="lblLaborAmount" Text='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("LaborAmount"))) ? Convert.ToDecimal(Eval("LaborAmount")).ToString("C2") : "" %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col3 textright" />
                                                <ItemStyle CssClass="col3 textright" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="PartsAmount">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnPartsAmount" runat="server" CommandArgument="PartsAmount"
                                                        CommandName="Sort"><span >Parts Amt</span></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hfPartsAmount" runat="server" Value='<%# Eval("PartsAmount")%>' />
                                                    <asp:Label runat="server" ID="lblPartsAmount" Text='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("PartsAmount"))) ? Convert.ToDecimal(Eval("PartsAmount")).ToString("C2") : "" %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col4 textright" />
                                                <ItemStyle CssClass="col4 textright" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Amount">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnAmount" runat="server" CommandArgument="Amount" CommandName="Sort"><span >Total Invoice Amt</span></asp:LinkButton>
                                                    <asp:PlaceHolder ID="placeholderAmount" runat="server"></asp:PlaceHolder>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblAmount" Text='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("Amount"))) ? Eval("Amount") : "" %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col5 textright" />
                                                <ItemStyle CssClass="col5 textright" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblShow" runat="server" Text="Total: "></asp:Label>
                                                    <asp:Label ID="lblTotalAmount" runat="server"  />
                                                </FooterTemplate>
                                                <FooterStyle CssClass="textright" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span>View</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnInvoiceFileName" runat="server" Value='<%# Eval("DocumentPath") %>' />
                                                    <asp:LinkButton ID="lnkbtnDownloadInvoiceFromGrid" runat="server" CssClass="small-blue-btn" CommandArgument='<%# "UploadedImages/AssetManagement/" + Eval("EquipmentMasterID") + "/Accounting/Invoice/" + Eval("EquipmentMaintenanceCostID") + "/" + Eval("DocumentPath") %>' CommandName="DownloadInvoice"><span>Invoice</span></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnViewInvoice" runat="server" CssClass="small-blue-btn" CommandArgument='<%# Eval("EquipmentMaintenanceCostID") %>' CommandName="View" ><span>Edit</span></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnDeleteInvoice" runat="server" CssClass="small-blue-btn" CommandArgument='<%# Eval("EquipmentMaintenanceCostID") %>' CommandName="DeleteInvoice"><span>DELETE</span></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="col1 text-center" />
                                                <ItemStyle CssClass="col1 text-center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                           <tr class="no-border">
                                                <td colspan="100%" class="no-padding">
                                                    <table class="table-grid">
                                                        <tr>
                                                            <th class="col1">
                                                                Invoice Date
                                                            </th>
                                                            <th class="col2">
                                                                Supplier
                                                            </th>
                                                            <th class="col3">
                                                                Labor Amount
                                                            </th>
                                                            <th class="col4">
                                                                Parts Amount
                                                            </th>
                                                            <th class="col5">
                                                                Invoice Amount
                                                            </th>
                                                            <th class="col6">
                                                                View
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6" style="text-align: center; color: #8BC1F6;">
                                                                Records not found
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                                <div id="pagingtable" runat="server" class="store-footer cf" visible="false">
                                    <a href="javascript: void(0);" class="store-title">BACK TO TOP</a>
                                    <asp:LinkButton ID="lnkViewAllInvoice" runat="server" OnClick="lnkViewAllInvoice_Click" CssClass="pagination alignright view-link postback cf" CommandName="Invoice">
                                VIEW ALL
                            </asp:LinkButton>
                             <div class="pagination alignright cf" >
                                    <asp:LinkButton ID="lnkbtnPrevious" class="left-arrow alignleft" runat="server" OnClick="lnkbtnPrevious_Click"
                                        ToolTip="Invoice"> 
                                    </asp:LinkButton>
                                    <%--<a title="1" href="javascript: void(0);">1</a><a title="2" class="active" href="javascript: void(0);">2</a><a title="3" href="javascript: void(0);">3</a>--%>
                                    <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") + ";1" %>'
                                                CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <asp:LinkButton ID="lnkbtnNext" class="right-arrow alignright" runat="server" OnClick="lnkbtnNext_Click"
                                        ToolTip="Invoice"> 
                                    </asp:LinkButton>
                                </div>
                                </div>
                                <div class="store-footer cf" style="display:none;"> <a href="#" class="store-title">BACK TO TOP</a> <a class="pagination alignright view-link postback cf"> VIEW ALL </a>
  <div class="pagination alignright cf"> <span> <a class="left-arrow alignleft postback"></a> </span> <span><span> <a class="postback" disabled="disabled">1</a> </span><span> <a href="#" class="postback">2</a> </span><span> <a href="#" class="postback">3</a> </span><span> <a href="#" class="postback">4</a> </span><span> <a href="#" class="postback">5</a> </span></span> <a href="#" class="right-arrow alignright postback"></a> </div>
</div>
								
								
								
								
								<table class="table-grid">
                                    <tr>
                                        <td class="footbtn-row claimpost" colspan="6">
                                            <a href="javascript:void(0);" title="ADD INVOICE" class="footbtn popup-openlink invoicepostbtn"
                                                id="a_invoicepost" runat="server"><span>Add Invoice</span></a>
                                            <%--<asp:LinkButton ID="lnkbtnClaim" runat="server" CssClass="small-blue-btn popup-openlink" OnClick="lnkbtnClaim_Click"><span>POST CLAIM</span></asp:LinkButton> --%>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                            </div>
                        </div>
                    </div>
					<div class="asset-btn-block last assetbtn-blockbar">
                        <asp:LinkButton ID="lnkbtnAccountingBack" runat="server" ToolTip="Back" CssClass="small-gray-btn"
                        OnClick="lnkbtnBack_Click"><span>Back</span></asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnAccountingSave" runat="server" ToolTip="SAVE" CssClass="small-blue-btn submit"
                        OnClick="lnkbtnAccountingSave_Click" ValidationGroup="AccountingTabGroup" call="AccountingTabGroup"><span>SAVE</span></asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnAccountingCLose" runat="server" CssClass="small-gray-btn" ToolTip="Close" OnClick="lnkbtnClose_Click"><span>Close</span></asp:LinkButton>
                    </div>
                </div>
                
                <div id="assetspendinginvoice-popup" class="popup-outer ParentTab" data-defaultbutton="<%= lnkbtnSaveInformation.ClientID %>">
	                
	                <div class="popupInner">
	                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Asset Management','Add/Edit Invoice');">Help Video</a>
	                <a id="pendinginvoice_close_a" class="close_a clientsidectrl" onclick="ClosePopUp(this,event,'close_a',true);" href="javascript: void(0);" >Close</a>
		                <div class="assetspoupup-content cf">
			                <h2 id="invoiceHeader"><span class="popup-asset-title"></span> Add/Edit Invoice </h2>
			                
			                <div class="request-form">
				                    <ul class="cf">
						                <li class="alignleft" tabindex="1">
							                <span class="lbl-txt">Vendor</span>
							                <span class="full-drop">
								                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="default">
                                                </asp:DropDownList>
							                </span>
						                </li>


                                         <li class="alignleft" tabindex="2">
							                <span class="lbl-txt">Invoice #</span>
							                <label>
								                <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="input-field-all"></asp:TextBox>
							                </label>
						                </li>

						                <li class="alignright" tabindex="3">
							                <span class="lbl-txt">Date of Service</span>
							                <label>
								                <asp:TextBox ID="txtDateOfService" runat="server" CssClass="input-field-all setDatePicker" ></asp:TextBox>
							                </label>
						                </li>
						               
						                <li class="alignleft" tabindex="4">
							                <span class="lbl-txt">Description</span>
							                <textarea id="txtDescription" runat="server" class="input-textarea"></textarea>
						                </li>
						                <li class="alignleft" tabindex="5">
							                <span class="lbl-txt">Total Invoice Amt</span>
							                <label>
								                <asp:TextBox ID="txtTotalInvoiceAmt" runat="server" CssClass="input-field-all pricetext invoiceamount" data-a-sign="$ " ></asp:TextBox>
							                </label>
						                </li>
						                <li class="alignright" tabindex="6">
							                <span class="lbl-txt">Labor Amount</span>
							                <label>
								                <asp:TextBox ID="txtLaborAmount" runat="server" CssClass="input-field-all pricetext invoiceamount" data-a-sign="$ "></asp:TextBox>
							                </label>
						                </li>
						                <li class="alignleft" tabindex="7">
							                <span class="lbl-txt">Parts Amount</span>
							                <label>
								                <asp:TextBox ID="txtPartsAmount" runat="server" CssClass="input-field-all pricetext invoiceamount" data-a-sign="$ "></asp:TextBox>
							                </label>
						                </li>
						                <li class="alignright" tabindex="8">
							                <span class="lbl-txt">Job Code</span>
							                <span class="select-drop medium-drop popup" id="accounting_jobcode_span" runat="server" data-content="assetspendinginvoice-popup;assetspendinginvoice-popup .assetspoupup-content">
							                 <cdd:CustomDropDown ID="ddlJobCode" runat="server" DropDownCssClass="default"
                                            TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop medium-drop popup" Module="jobcode"
                                            OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                            GroupName="JobCode" DefaultOptionText="Job Code" />
                                            </span>
							            </li>
						                <li class="alignleft" tabindex="9">
							                <span class="lbl-txt">Job Sub Code</span>
							                <div class="input-textarea" style="height: 140px; border: 1px solid Grey; overflow: auto">
                                                 <asp:CheckBoxList ID="cblJobSubCode" runat="server" DataTextField="JobCodeName" DataValueField="JobCodeID" CssClass="checksubcode" RepeatColumns="2" RepeatDirection="Vertical" >
                                                 </asp:CheckBoxList>
                                            </div>
						                </li>
						                <li class="alignleft" tabindex="10">
							                <span class="lbl-txt">Document</span>
							                <label class="filedoc input-file-upload">
								                <asp:FileUpload id="fuDocument" runat="server" />
								                <asp:LinkButton ID="lnkbtnDownloadInvoice" runat="server" Text="View Document" OnClick="lnkbtnDownload_Click" Visible="false"></asp:LinkButton>
								                 <span class="fileformate" >Only PDF, Word Doc files are allowed. Not available on mobile devices.</span>
							                </label>
						                </li>
					                </ul>
					                <div class="warranty-form-block" tabindex="11">
					                    <asp:HiddenField ID="hfInvoiceID" runat="server" />
						                <a id="pendinginvoice_cancel_a" href="javascript: void(0);"  title="Cancel" onclick="ClosePopUp(this,event,'cancel_a',true);" class="gray-home-btn cancel_a clientsidectrl"><span>Cancel</span></a>
						                <asp:LinkButton ID="lnkbtnSaveInformation" runat="server" CssClass="blue-home-btn" ToolTip="Save Information" OnClick="lnkbtnSaveInformation_Click"><span>Save Information</span></asp:LinkButton>
					                </div>
				               
			                </div>
		                </div>
	                </div>
                </div>
                
                <div id="assetsviewpendinginvoice-popup" class="popup-outer">
	                
	                <div class="popupInner">
	                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Asset Management','View Invoice');">Help Video</a>
	                <a id="assetsviewpending_close_a" class="close_a clientsidectrl" onclick="ClosePopUp(this,event,'close_a',true);" href="javascript: void(0);" >Close</a>
		                <div class="assetspoupup-content cf">
			                
			                <h2><span class="popup-asset-title"></span> View Invoice </h2>
			                
			                <div class="request-form">
				                    <ul class="cf">
						                <li class="alignleft">
							                <span class="lbl-txt">Vendor</span>
							                <span class="full-drop">
								                <asp:Label ID="lblVendor" CssClass="select-label" runat="server"></asp:Label>
							                </span>
						                </li>
						                <li class="alignright">
							                <span class="lbl-txt">Date of Service</span>
							              
								                <asp:Label ID="lblDateOfService" CssClass="select-label" runat="server"></asp:Label>
							              
						                </li>
						                <li class="alignleft">
							                <span class="lbl-txt">Invoice #</span>
							             
								                <asp:Label ID="lblInvoiceNumber" CssClass="select-label" runat="server"></asp:Label>
							             
						                </li>
						                
						                <li class="alignright">
							                <span class="lbl-txt line-ht20">Total Invoice Amt</span>
							               
								                <asp:Label ID="lblTotalInvoiceAmt" CssClass="select-label pricetext" data-a-sign="$ " runat="server"></asp:Label>
							                
						                </li>
						                <li class="alignleft">
							                <span class="lbl-txt">Description</span>
							                <asp:Label ID="lblInvoiceDescription" CssClass="select-label" runat="server"></asp:Label>
						                </li>
						                <li class="alignright">
							                <span class="lbl-txt">Labor Amount</span>
							              
								                <asp:Label ID="lblLaborAmount" CssClass="select-label pricetext" data-a-sign="$ " runat="server"></asp:Label>
							               
						                </li>
						                <li class="alignleft">
							                <span class="lbl-txt">Parts Amount</span>
							                
								                <asp:Label ID="lblPartsAmount" CssClass="select-label pricetext" data-a-sign="$ " runat="server"></asp:Label>
							            </li>
						                <li class="alignright">
							                <span class="lbl-txt">Job Code</span>
							                <asp:Label ID="lblJobCode" runat="server" CssClass="select-label" ></asp:Label>
						                </li>
						                <li class="alignleft">
							                <span class="lbl-txt">Job Sub Code</span>
							                <div class="input-textarea" style="height: 135px; border: 1px solid Grey; overflow: auto">
                                                 <asp:Label ID="lblJobSubCode" runat="server"></asp:Label>
                                            </div>
						                </li>
						                <li class="alignright" style="display:none;">
							                <span class="lbl-txt">Document</span>
							                <label class="filedoc">
								                <asp:FileUpload id="FileUpload1" runat="server" />
							                </label>
						                </li>
					                </ul>
					                <div class="warranty-form-block">
						                <a id="viewpendinginvoice_cancel_a" href="javascript: void(0);"  title="Cancel" class="gray-home-btn cancel_a clientsidectrl" onclick="ClosePopUp(this,event,'cancel_a',true);"><span>Cancel</span></a>
					                </div>
			                </div>
		                </div>
	                </div>
                </div>
                
            </asp:View>
            <asp:View ID="vWarranty" runat="server">
                
                <div class="warranty-table-block" id="WarrantyTableBlock ParentTab">

					<div class="table-heading cf warrantyheader"><strong>WARRANTIES</strong><a title="New Warranty" class="small-blue-btn popup-openlink" href="javascript: void(0);"><span>New Warranty</span></a></div>
                    <asp:GridView ID="gvWarranty" runat="server" AutoGenerateColumns="false" GridLines="None"
                            CssClass="table-grid" ShowFooter="True" OnRowCommand="gvWarranty_RowCommand">
                            <Columns>
                                <asp:TemplateField SortExpression="WarrantyBy">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnWarrantyBy" runat="server" CommandArgument="WarrantyBy"
                                            CommandName="Sort"><span>Warranty By</span></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CssClass="link" ID="lnkbtnWarrantyBy" CommandName="ViewWarranty" CommandArgument='<%# Eval("WarrantyID") %>'
                                            Text='<%# Eval("WarrantyBy" ) %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col1" />
                                    <ItemStyle CssClass="col1" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Supplier">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnSupplier" runat="server" CommandArgument="Supplier" CommandName="Sort"><span>Supplier</span></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--<asp:Label runat="server" ID="lblSupplier" Text='<%# Eval("Supplier" ) %>'></asp:Label>--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col2" />
                                    <ItemStyle CssClass="col2" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Period">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnPeriod" runat="server" CommandArgument="Period" CommandName="Sort"><span>Period</span></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblPeriod" Text='<%# Eval("WarrantyPeriod")+ " " + Eval("WarrantyPeriodType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col3" />
                                    <ItemStyle CssClass="col3" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="DayToExpire">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnDayToExpire" runat="server" CommandArgument="DayToExpire"
                                            CommandName="Sort"><span>Day To Expire</span></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDayToExpire" Text='<%# Eval("DaysToExpire") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col4" />
                                    <ItemStyle CssClass="col4" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span>View Warranty</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnViewWarranty" runat="server" CommandName="View" CommandArgument='<%# Eval("WarrantyID") %>'
                                            CssClass="gray-button" OnClientClick="ShowDefaultLoader();" ToolTip="View"><span>VIEW</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkbtnDeleteWarranty" runat="server" CommandName="WarrantyDelete" CommandArgument='<%# Eval("WarrantyID") %>'
                                            CssClass="gray-button del-button" OnClientClick="ShowDefaultLoader();" ToolTip="Delete"><span>Delete</span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col5 viewwarranty-td" />
                                    <ItemStyle CssClass="col5 viewwarranty-td" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <tr class="no-border">
                                            <td colspan="100%" class="no-padding">
                                                <div id="dvClaim" runat="server" visible="false">
                                                    <div class="table-griddesc">
                                                        <span class="top-arrow"></span>
                                                        <asp:GridView ID="gvWarrantyClaim" runat="server" AutoGenerateColumns="false" GridLines="None"
                                                             CssClass="table-grid" ShowFooter="True" OnRowCommand="gvWarrantyClaim_RowCommand">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Claim Date</span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%--<asp:Label runat="server" ID="lblClaimDate" Text='<%# Eval("ClaimDate") != null && Eval("ClaimDate" ) != ""?Convert.ToDateTime(Eval("ClaimDate")).ToString("MM/dd/yyyy"):"" %>'></asp:Label>--%>
                                                                        <asp:LinkButton runat="server" CssClass="link" ID="lnkbtnClaim" CommandName="ViewClaim" CommandArgument='<%# Eval("ClaimID") %>'
                                                                        Text='<%# Eval("ClaimDate") != null && Eval("ClaimDate" ) != ""?Convert.ToDateTime(Eval("ClaimDate")).ToString("MM/dd/yyyy"):"" %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="col1" />
                                                                    <ItemStyle CssClass="col1" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>ClaimAmount</span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblClaimAmount" Text='<%# Eval("ClaimAmount" ) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="col2" />
                                                                    <ItemStyle CssClass="col2" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Eligible Amount</span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblPeriod" Text='<%# Eval("ClaimEligibleAmount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="col3" />
                                                                    <ItemStyle CssClass="col3" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Manufacturer</span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblManufacturer" Text=''></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="col4" />
                                                                    <ItemStyle CssClass="col4" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Warranty Type</span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblWarrantyType" Text=''></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="col4" />
                                                                    <ItemStyle CssClass="col4" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <span>Notes</span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hfGridWarrantyClaimID" runat="server" Value='<%# Eval("ClaimID") %>' />
                                                                        <asp:LinkButton id="lnkbtnViewClaimNotes" runat="server" CssClass="gray-button" ToolTip="View" OnClick="lnkbtnViewClaimNotes_Click"><span>View</span></asp:LinkButton>&nbsp;
                                                                        <a href="javascript:void(0);" class="gray-button claimaddpopup" title="Add" data-ID='<%# Eval("ClaimID") %>'><span>Add</span></a>
                                                                        <asp:LinkButton ID="lnkbtnDeleteClaim" runat="server" CommandName="ClaimDelete" CommandArgument='<%# Eval("ClaimID") %>'
                                                                            CssClass="gray-button del-button" OnClientClick="ShowDefaultLoader();" ToolTip="Delete"><span>Delete</span>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="col5 viewwarranty-td" />
                                                                    <ItemStyle CssClass="col5 viewwarranty-td" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField></asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                             <tr class="no-border">
                                                                 <td colspan="100%" class="no-padding">
                                                                    <table class="table-grid">
                                                                        <tr>
                                                                            <th class="col1">
                                                                                Claim Date
                                                                            </th>
                                                                            <th class="col2">
                                                                                Claim Amount
                                                                            </th>
                                                                            <th class="col3">
                                                                                Eligible Amount
                                                                            </th>
                                                                            <th class="col4">
                                                                                Manufacturer
                                                                            </th>
                                                                            <th class="col5">
                                                                                Warranty Type
                                                                            </th>
                                                                            <th class="col6">
                                                                                Notes
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="6" style="text-align: center; color: #8BC1F6;">
                                                                                Records not found
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                    <div id="ClaimPagingTable" runat="server" class="store-footer cf">
                                                        <a href="javascript: void(0);" class="store-title">BACK TO TOP</a>
                                                           <asp:LinkButton ID="lnkViewAllClaim" runat="server" OnClick="lnkViewAllClaim_Click"  CommandArgument='<%# Eval("WarrantyID") %>'  CssClass="pagination alignright view-link cf">
                                                                VIEW ALL
                                                            </asp:LinkButton>
                                                            <div class="pagination alignright cf" >
                                                        <asp:LinkButton ID="lnkbtnClaimPrevious" class="left-arrow alignleft" runat="server"
                                                            CommandArgument='<%# Eval("WarrantyID") %>' CommandName="ClaimPrevious" OnClick="lnkbtnPrevious_Click"
                                                            ToolTip="Invoice"> 
                                                        </asp:LinkButton>
                                                        <%--<a title="1" href="javascript: void(0);">1</a><a title="2" class="active" href="javascript: void(0);">2</a><a title="3" href="javascript: void(0);">3</a>--%>
                                                        <asp:DataList ID="dtlClaimPaging" runat="server" CellPadding="1" CellSpacing="1"
                                                            RepeatDirection="Horizontal" RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand"
                                                            OnItemDataBound="dtlPaging_ItemDataBound">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") + ";3" %>'
                                                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                        <asp:LinkButton ID="lnkbtnClaimNext" class="right-arrow alignright" runat="server"
                                                            CommandArgument='<%# Eval("WarrantyID") %>' CommandName="ClaimNext" OnClick="lnkbtnNext_Click"
                                                            ToolTip="Invoice"> 
                                                        </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <table class="table-grid">
                                                        <tr>
                                                            <td class="footbtn-row claimpost" colspan="6">
                                                                <a href="javascript:void(0);" title="POST CLAIM" class="footbtn popup-openlink claimpostbtn"
                                                                    id="a_PostClaim" runat="server"><span>POST CLAIM</span></a>
                                                                <%--<asp:LinkButton ID="lnkbtnClaim" runat="server" CssClass="small-blue-btn popup-openlink" OnClick="lnkbtnClaim_Click"><span>POST CLAIM</span></asp:LinkButton> --%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataRowStyle CssClass="no-border" />
                            <EmptyDataTemplate>
                                <table class="table-grid">
                                    <tr>
                                        <th class="col1">
                                            Warranty By
                                        </th>
                                        <th class="col2">
                                            Supplier
                                        </th>
                                        <th class="col3">
                                            Period
                                        </th>
                                        <th class="col4">
                                            Day to Expire
                                        </th>
                                        <th class="col5">
                                            View Warranty
                                        </th>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="text-align: center; color: #8BC1F6;">
                                            Records not found
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    <div id="WarrantyPagingTable" runat="server" class="store-footer cf" >
                        <a href="javascript: void(0);" class="store-title">BACK TO TOP</a>
                        <asp:LinkButton ID="lnkViewAllWarranty" runat="server" OnClick="lnkViewAllWarranty_Click" CssClass="pagination alignright view-link postback cf">
                                VIEW ALL
                            </asp:LinkButton>
                        <div class="pagination alignright cf" >
                        <asp:LinkButton ID="lnkbtnWarrantyPrevious" class="left-arrow alignleft" runat="server"
                            OnClick="lnkbtnPrevious_Click" ToolTip="Invoice"> 
                        </asp:LinkButton>
                        
                        <asp:DataList ID="dtlWarrantyPaging" runat="server" CellPadding="1" CellSpacing="1"
                            RepeatDirection="Horizontal" RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand"
                            OnItemDataBound="dtlPaging_ItemDataBound">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") + ";2" %>'
                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:LinkButton ID="lnkbtnWarrantyNext" class="right-arrow alignright" runat="server"
                            OnClick="lnkbtnNext_Click" ToolTip="Invoice"> 
                        </asp:LinkButton>
                        </div>
                    </div>
                   <div class="asset-btn-block last assetbtn-blockbar">
                        <asp:LinkButton ID="lnkbtnWarrantyBack" runat="server" ToolTip="Back" CssClass="small-gray-btn"
                        OnClick="lnkbtnBack_Click" OnClientClick="ShowDefaultLoader();"><span>Back</span></asp:LinkButton>
                        <a class="small-gray-btn" title="Close" href="javascript: void(0);" ><span>Close</span></a>
                   </div>
                </div>
             
                <div id="new-warranty-popup" class="popup-outer ParentTab" data-defaultbutton="<%= lnkbtnAddNewWarranty.ClientID %>">
                    <div class="popupInner">
                        <div class="warranty-popup">
                        <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Asset Management','Add New Warranty');">Help Video</a>
                            <a id="newwarranty_close_a" class="close_a clientsidectrl" onclick="ClosePopUp(this,event,'close_a',true);" href="javascript:void(0);">Close</a>
                            <div class="warranty-content">
                                <h2>Add New Warranty</h2>
                                
                                <div class="warranty-form">
                                    <ul class="cf">
                                        <li class="alignleft"><span class="lbl-txt">Warranty By</span><span class="select-drop warranty-mid-drop popup"
                                            id="warrantyby_parent_span" runat="server" data-content="new-warranty-popup;new-warranty-popup .warranty-content">
                                            <%--<asp:Label ID="lblPopUpDivIDs" runat="server" Text="new-warranty-popup;new-warranty-popup .warranty-content"
                                                Visible="false" />--%>
                                            <cdd:CustomDropDown ID="ddlWarrantyBy" runat="server" DropDownCssClass="default"
                                                TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop warranty-mid-drop popup" Module="Equipmentlookup"
                                                OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                                GroupName="WarrantyBy" DefaultOptionText="Warranty By"/>
                                        </span></li>
                                        <li class="alignright warrantyperiod-radio">
                                            <span class="lbl-txt1">Warranty<br>Period</span>
                                            <label class="warrantyperiod-input"><asp:TextBox ID="txtWarrantyPeriod" runat="server" CssClass="input-field-all warranty-small" onkeypress="return AllowNumbersONLY(event);"></asp:TextBox></label>
                                            <label class="label_radiobox">
                                                <asp:RadioButton CssClass="iradio_flat" ID="rdbtnYears" Checked="true" runat="server" GroupName="warranty-period" ToolTip="Years" /> Years
                                            </label>                                                
                                            <label class="label_radiobox">
                                                <asp:RadioButton CssClass="iradio_flat" ID="rdbtnDays" runat="server" GroupName="warranty-period" ToolTip="Days" /> Days
                                            </label>
                                        </li>
                                        <li class="alignleft"><span class="lbl-txt">Start Date</span>
                                        <asp:TextBox ID="txtWarrantyStartDate" runat="server" CssClass="input-field-all setDatePicker" TabIndex="9" ></asp:TextBox>
                                       </li>
                                        <li class="alignright"><span class="lbl-txt1">Purchase
                                            <br>
                                            Condition</span><span class="select-drop warranty-mid-drop popup" id="purchase_condition_parent_span"
                                                runat="server" data-content="new-warranty-popup;new-warranty-popup .warranty-content">
                                                <cdd:CustomDropDown ID="ddlPurchaseCondition" runat="server" DropDownCssClass="default" Module="Equipmentlookup"
                                                    TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop warranty-mid-drop popup"
                                                    OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                                    GroupName="PurchaseConditions" DefaultOptionText="Purchase Condition" />
                                            </span></li>
                                        <li class="alignleft"><span class="lbl-txt1">Warranty
                                            <br>
                                            Terms</span><span class="select-drop warranty-mid-drop popup" id="warrantyterms_parent_span"
                                                runat="server" data-content="new-warranty-popup;new-warranty-popup .warranty-content">
                                                <cdd:CustomDropDown ID="ddlWarantyTerms" runat="server" DropDownCssClass="default"
                                                    TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop warranty-mid-drop popup" Module="Equipmentlookup"
                                                    OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                                    GroupName="WarrantyTerms" DefaultOptionText="Warranty Terms" />
                                            </span></li>
                                        <li class="alignright"><span class="lbl-txt1">Attach
                                            <br>
                                            Warranty</span>
                                            <label class="input-file-upload">
                                                <input id="fuAttachWarranty" runat="server" class="fileU" type="file">
                                                 <span class="fileformate">Only PDF, Word Doc files are allowed. Not available on mobile devices.</span>
                                                </label>
                                            </li>
                                        <li class="alignleft mrt-15"><span class="lbl-txt">Notes</span><label><textarea id="txtWarrantyNotes"
                                            runat="server" class="input-textarea"></textarea></label></li>
                                    </ul>
                                    <div class="warranty-form-block">
                                        <a id="newwarranty_cancel_a" href="javascript:void(0);" title="Cancel" class="gray-home-btn cancel_a clientsidectrl" onclick="ClosePopUp(this,event,'cancel_a',true);"><span>
                                            Cancel</span></a>
                                        <asp:LinkButton ID="lnkbtnAddNewWarranty" runat="server" ToolTip="Add New Warranty"
                                            CssClass="blue-home-btn" OnClick="lnkbtnAddNewWarranty_Click"><span>Add New Warranty</span></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
             
                <div id="post-warranty-popup" class="popup-outer ParentTab" data-defaultbutton="<%= lnkbtnAddNewWarrantyClaim.ClientID %>">
                    <div class="popupInner">
                        <div class="warranty-popup">
                        <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Asset Management','Post Claim');">Help Video</a>
                            <a id="postwarranty_close_a" class="close_a clientsidectrl" onclick="ClosePopUp(this,event,'close_a',true);" href="javascript:void(0);">Close</a>
                            <div class="warranty-content">
                                <h2>Post Claim</h2>
                                
                                <div class="warranty-form">
                                    <ul class="cf">
                                        <li class="alignleft"><span class="lbl-txt">Warranty Type</span><span id="warranty_type_parent_span"
                                            runat="server" class="select-drop warranty-mid-drop claimdrop popup" data-content="post-warranty-popup;post-warranty-popup .warranty-content">
                                             <%--<asp:Label ID="lblClaimPopup" runat="server" Text="post-warranty-popup;post-warranty-popup .warranty-content"
                                                Visible="false" />--%>
                                            <cdd:CustomDropDown ID="ddlWarrantyType" runat="server" DropDownCssClass="default"
                                                TextBoxCssClass="input-field-all txtCustom" ParentSpanClassToRemove="select-drop warranty-mid-drop claimdrop popup" Module="Equipmentlookup"
                                                OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged" OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted"
                                                GroupName="WarrantyType" DefaultOptionText="Warranty Type" />
                                        </span></li>
                                        <li class="alignright"><span class="lbl-txt1">Claim Date</span>
                                            <asp:TextBox ID="txtClaimDate" runat="server" CssClass="input-field-all setDatePicker" TabIndex="9" ></asp:TextBox>
                                            </li>
                                        <li class="alignleft"><span class="lbl-txt1">Claim
                                            <br>
                                            Amount</span><label>
                                                <asp:TextBox ID="txtClaimAmount" runat="server" CssClass="input-field-all pricetext" data-a-sign="$ " ></asp:TextBox>
                                            </label>
                                        </li>
                                        <li class="alignright"><span class="lbl-txt1">Eligible
                                            <br>
                                            Amount</span><label>
                                                <asp:TextBox ID="txtClaimEligibleAmount" runat="server" CssClass="input-field-all pricetext" data-a-sign="$ "></asp:TextBox>
                                            </label>
                                        </li>
                                        <li class="alignleft"><span class="lbl-txt">Notes</span>
                                            <textarea id="txtClaimNotes" runat="server" class="input-textarea"></textarea></li>
                                        <li class="alignleft file-input">
                                            <label class="input-file-upload">
                                                <input id="fuClaim" runat="server" class="fileU" type="file">
                                                 <span class="fileformate">Only PDF, Word Doc files are allowed. Not available on mobile devices.</span>
                                            </label>
                                        </li>
                                    </ul>
                                    <div class="warranty-form-block">
                                        <a id="postclaim_cancel_a" href="javascript:void(0);" title="Cancel" onclick="ClosePopUp(this,event,'cancel_a',true);" class="gray-home-btn cancel_a clientsidectrl"><span>
                                            Cancel</span></a>
                                        <asp:HiddenField ID="hfWarrantyIDForClaim" runat="server" />
                                        <asp:HiddenField ID="hfRowIndex" runat="server" />
                                        <asp:LinkButton ID="lnkbtnAddNewWarrantyClaim" runat="server" ToolTip="Post Claim"
                                            CssClass="blue-home-btn" OnClick="lnkbtnAddNewWarrantyClaim_Click"><span>Post Claim</span></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            
                <div id="claim-addnote-popup" class="popup-outer ParentTab" data-defaultbutton="<%= lnkbtnClaimAddNote.ClientID %>" >
	                <div class="popupInner">
		                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Asset Management','Add Claim Note');">Help Video</a>
			                <a id="claimaddnote_close_a" class="close_a clientsidectrl" onclick="ClosePopUp(this,event,'close_a',true);" href="javascript: void(0);">Close</a>
		                <div class="assetspoupup-content cf">
			                
			                <h2>Add Note</h2>
			                
			                <div class="request-form">
				                    <ul class="cf">
						                <li class="alignleft">
							                <span class="lbl-txt">Note</span>
							                <textarea id="txtClaimNoteDetails" runat="server" class="input-textarea"></textarea>
						                </li>						
					                </ul>
					                <div class="warranty-form-block">
						                <a id="claimaddnote_cancel_a" href="javascript: void(0);" title="Cancel" class="gray-home-btn cancel_a clientsidectrl" onclick="ClosePopUp(this,event,'cancel_a',true);"><span>Cancel</span></a>
						                <asp:LinkButton ID="lnkbtnClaimAddNote" runat="server" CssClass="blue-home-btn" ToolTip="Save Note" OnClick="lnkbtnClaimAddNote_Click"><span>Save Note</span></asp:LinkButton>
					                </div>
					                <asp:HiddenField ID="hfWarrantyClaimID" runat="server" />
			                </div>
		                </div>
	                </div>
                </div>
            
                <div id="claim-view-notes-popup" class="popup-outer ParentTab" data-defaultbutton="<%= lnkbtnClaimAddNote.ClientID %>">
	                <div class="popupInner">
		                <div class="assetspoupup-content cf">
			                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Asset Management','View Claim Notes');">Help Video</a>
			                <a id="claimviewnote_close_a" class="close_a clientsidectrl" onclick="ClosePopUp(this,event,'close_a',true);" href="javascript: void(0);">Close</a>
			                <h2>View Notes</h2>
			                
			                <div class="message-container">
					              <div id="boxscroll">
						                <div class="viewnote-listbox">
							                <ul class="viewnote-list cf">
								               <asp:Repeater ID="rpWarrantyClaimNotes" runat="server">
							                        <ItemTemplate>
							                            <li>
								                            <h5 class="cf"><span><%# Eval("FirstName") + " " + Eval("LastName")%></span><em><%# Eval("DateNTime") %></em></h5>
								                            <p><%# Eval("Notecontents") %></p>
								                        </li>    
							                        </ItemTemplate>
							                    </asp:Repeater>
							                     <li id="li_EmptyClaimNotes" runat="server" style="text-align:center;">
							                        <p>No data found.</p>
							                    </li>
							                </ul>
						                </div>
				                  </div>					
			                </div>
		                </div>
	                </div>
                </div>
                
            </asp:View>
            <asp:View ID="vSpecs" runat="server">
                 <div class="account-form-block ParentTab" id="dvSpecification" data-defaultbutton="<%= lnkbtnSaveSpecifications.ClientID %>">
                    <div class="account-formInner">
                        <div class="basic-form specs-form" >
                            <div class="cf">
                                <ul class="left-form cf dynamic-form" id="ul_SpecificationsFields" runat="server" enableviewstate="false">
                                </ul>
                                <div id="dvSpecImages" class="asset-title parentdiv">
                                    ASSET IMAGES AND VIDEOS<a href="javascript:void(0);" class="gray-button alignright"><span>Upload
                                        New</span></a><em class="down-arrow">&nbsp;</em></div>
                                    <div class="childdiv" style="display:none;">
                                            <div class="upload-form">
                                                <div class="upload-content">
                                                    <div class="upload-txt">
                                                        Drag or paste your Image(jpeg,gif or mp4) file here, or <a title="browse" href="javascript: void(0);" onclick="ShowFileUploadPopUp('Specification','ImagesFileName','ImageVideo');" ontouchstart="ShowFileUploadPopUp('Specification','ImagesFileName','ImageVideo');">browse</a> for
                                                        a file to upload.</div>
                                                    <div class="upload-file">
                                                        <%--<input id="fuAssetImages" runat="server" class="fileU" type="file">--%>
                                                        <a href="javascript: void(0);" class="gray-button" onclick="ShowFileUploadPopUp('Specification','ImagesFileName','ImageVideo');" ontouchstart="ShowFileUploadPopUp('Specification','ImagesFileName','ImageVideo');"><span>Browse</span></a>
                                                        <%--<asp:FileUpload ID="fuSpecAssetImages" runat="server" class="fileU multi filupCtrl assetimagectrl checkvalidation" type="file" Style="opacity:0;position:absolute;left:-99999px;" accept="jpg|JPG|jpeg|JPEG|gif|GIF|mp4|MP4" allowedextensionmsg="jpeg, gif and mp4"/>
                                                         <asp:RegularExpressionValidator ID="revAssetImages" runat="server" ErrorMessage="Only jpeg/png/mp4 files allowed."
                                                            CssClass="error" ControlToValidate="fuSpecAssetImages" Display="None" SetFocusOnError="False"
                                                            ValidationGroup="SpecValidation" ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG|gif|GIF|mp4|MP4)$"></asp:RegularExpressionValidator>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div  class="multifilediv">
                                                <div id="AssetImage_div" runat="server" visible="false" class="asset-img-block imagesblock" >
                                                    <asp:Repeater ID="rpAssetImage" runat="server">
                                                        <ItemTemplate>
                                                            <div class="asset-image">
                                                                <div class="asset-innerimage">
                                                                     <span id="lblFileType" class="filetype" style="display:none;"><%# Convert.ToString(Eval("FileType")) %></span> 
                                                                     <span id="lblImagesFileName" class="ImagesFileName" style="display:none;"><%# Convert.ToString(Eval("FileTitle"))%></span>
                                                                    <a onclick='<%# "OpenGallery(this,'specs-gallerypopup','" + System.Configuration.ConfigurationManager.AppSettings["siteurl"] + "UploadedImages/AssetManagement/" + Eval("EquipmentMasterID") + "/Specification/" +  Eval("Name") + "');" %>' class="opendialog">
                                                                         <img src='<%# Convert.ToString(Eval("FileType")) == "Image" ?  System.Configuration.ConfigurationManager.AppSettings["siteurl"] + "UploadedImages/AssetManagement/" + Eval("EquipmentMasterID") + "/Specification/" +  Eval("Name") : "../StaticContents/img/asset-img.jpg" %>' width="69" height="69" alt='<%# Eval("FileTitle") %>' title='<%# Eval("FileTitle") %>'>
                                                                            <ul class="asset-imagehover cf">
                                                                                <li>
                                                                                    <asp:LinkButton ID="lnkbtnDownload" runat="server" OnClick="lnkbtnDownload_Click" Text="Download" CommandArgument='<%# "UploadedImages/AssetManagement/" + Eval("EquipmentMasterID") +"/Specification/" +  Eval("Name") %>' CommandName='<%# Eval("FileTitle") %>'>
                                                                                    <img src="../StaticContents/img/asset-ico2.png" width="20" height="20"></asp:LinkButton>
                                                                                </li>
                                                                                <li><asp:LinkButton ID="lnkbtnDelete" runat="server" OnClientClick="return confirm('Are you sure you want to delete the file?');" OnClick="lnkbtnDeleteFile_Click"  CommandName='<%# "UploadedImages/AssetManagement/" + Eval("EquipmentMasterID") +"/Specification/" +  Eval("Name") %>' CommandArgument='<%# Eval("EquipmentFileID") %>'>
                                                                                    <img src="../StaticContents/img/asset-ico3.png" width="20" height="20"></asp:LinkButton>
                                                                                    </li>
                                                                                <li><a href="javascript: void(0);" onclick="SendMailPopUp('<%# "UploadedImages/AssetManagement/" + Eval("EquipmentMasterID") +"/Specification/" +  Eval("Name") %>','Asset Specification File','Specification','AssetImagesVideos',<%# Eval("EquipmentFileID") %>);" ontouchstart="SendMailPopUp('<%# "UploadedImages/AssetManagement/" + Eval("EquipmentMasterID") +"/Specification/" +  Eval("Name") %>','Asset Specification File','Specification','AssetImagesVideos',<%# Eval("EquipmentFileID") %>);">
                                                                                    <img src="../StaticContents/img/asset-ico1.png" width="20" height="20"></a></li>
                                                                            </ul>
                                                                    </a>
                                                                </div>
                                                                
                                                                <strong><%# Eval("FileTitle") %></strong>
                                                            </div>
                                                        </ItemTemplate>  
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                            
                                    </div>
                                    
                                <div id="dvSpecManuals" class="asset-title parentdiv">
                                    ASSET MANUALS<a href="javascript: void(0);" class="gray-button alignright"><span>Upload New</span></a><em
                                        class="down-arrow">&nbsp;</em></div>
                                <div class="childdiv" style="display:none;">
                                    <div class="upload-form ">
                                        <div class="upload-content">
                                            <div class="upload-txt">
                                                Drag or paste your Document(pdf,doc or docx) file here, or <a title="browse" href="javascript: void(0);" onclick="ShowFileUploadPopUp('Specification','MannualsFileName','Document');" ontouchstart="ShowFileUploadPopUp('Specification','MannualsFileName','Document');">browse</a> for
                                                a file to upload.</div>
                                            <div class="upload-file">
                                                <%--<input type="file" id="fuSpecManual" runat="server"></div>--%>
                                                <a href="javascript: void(0);" class="gray-button" onclick="ShowFileUploadPopUp('Specification','MannualsFileName','Document');" ontouchstart="ShowFileUploadPopUp('Specification','MannualsFileName','Document');"><span>Browse</span></a>
                                                <%--<asp:FileUpload ID="fuSpecAssetManuals" runat="server" class="fileU multi filupCtrl assetmanualctrl checkvalidation" type="file" Style="opacity:0;position:absolute;left:-99999px;" accept="pdf|PDF|doc|DOC|docx|DOCX" allowedextensionmsg="pdf,doc and docx"/>
                                                <asp:RegularExpressionValidator ID="revAssetManuals" runat="server" ErrorMessage="Only PDF/pdf file allowed."
                                                 CssClass="error" ControlToValidate="fuSpecAssetManuals" Display="None" SetFocusOnError="False"
                                                 ValidationGroup="SpecValidation" ValidationExpression="^.*\.(pdf|PDF|doc|DOC|docx|DOCX)$"></asp:RegularExpressionValidator>--%>
                                        </div>
                                        </div>
                                    </div>
                                        <div  class="multifilediv">
                                            <div id="AssetManuals_div" runat="server" visible="false" class="asset-img-block file-block">
                                                <asp:Repeater ID="rpAssetManuals" runat="server">
                                                    <ItemTemplate>                                                        
                                                         <div class="asset-image">
                                                                <div class="assetdoc-innerimage">
                                                                    <span id="lblMannualsFileName" class="MannualsFileName" style="display:none;"><%# Convert.ToString(Eval("FileTitle"))%></span> 
                                                                    <img src="../StaticContents/img/file-ico.png" width="79" height="79" alt='<%# Eval("FileTitle") %>' title='<%# Eval("FileTitle") %>'>
                                                                    <ul class="asset-imagehover cf">
                                                                        <li>
                                                                            <asp:LinkButton ID="lnkbtnDownload" runat="server" OnClick="lnkbtnDownload_Click" Text="Download" CommandArgument='<%# "UploadedImages/AssetManagement/Specification/" +  Eval("Name") %>' CommandName='<%# Eval("FileTitle") %>'>
                                                                            <img src="../StaticContents/img/asset-ico2.png" width="20" height="20"></asp:LinkButton>
                                                                        </li>
                                                                        <li>
                                                                            <asp:LinkButton ID="lnkbtnDelete" runat="server" OnClientClick="return confirm('Are you sure you want to delete the file?');" OnClick="lnkbtnDeleteFile_Click" CommandName='<%# "UploadedImages/AssetManagement/Specification/" +  Eval("Name") %>' CommandArgument='<%# Eval("EquipmentFileID") %>'>
                                                                            <img src="../StaticContents/img/asset-ico3.png" width="20" height="20"></asp:LinkButton>
                                                                        </li>
                                                                        <li><a href="javascript: void(0);" onclick="SendMailPopUp('<%# "UploadedImages/AssetManagement/Specification/" +  Eval("Name") %>','Asset Specification Mauals','Specification','AssetManuals',<%# Eval("EquipmentFileID") %>);" ontouchstart="SendMailPopUp('<%# "UploadedImages/AssetManagement/Specification/" +  Eval("Name") %>','Asset Specification Mauals','Specification','AssetManuals',<%# Eval("EquipmentFileID") %>);">
                                                                            <img src="../StaticContents/img/asset-ico1.png" width="20" height="20"></a></li>
                                                                    </ul>
                                                                </div>
                                                                
                                                                <strong><%# Eval("FileTitle") %></strong>
                                                                <input type="text" value="" class="input-field-small input-55" />
                                                            </div>
                                                     </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                </div>
                            </div>
                        </div>                        
                    </div>
                    <div class="warranty-page-btn last">
                        <asp:LinkButton ID="lnkbtnSpecBack" runat="server" ToolTip="Back" CssClass="small-gray-btn"
                        OnClick="lnkbtnBack_Click" OnClientClick="ShowDefaultLoader();"><span>Back</span></asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnSaveSpecifications" runat="server" ToolTip="Save" OnClick="lnkbtnSaveSpecifications_Click" CssClass="small-blue-btn submit" OnClientClick="ShowDefaultLoader();"
                        ValidationGroup="SpecValidation" call="SpecValidation">
                        <span>SAVE</span></asp:LinkButton>
                        <a class="small-gray-btn" title="Close" href="javascript: void(0);" ><span>Close</span></a>
                   </div>
                </div>
                  <div id="specs-gallerypopup" class="popup-outer">
                    <div class="popupInner">
                        <div class="specs-gallery">
                            <a id="specgallery_close_a" class="close-btn clientsidectrl" href="javascript:void(0);">Close</a>
                            <img src='' alt='Image Namge' />
                         </div>
                    </div>
                </div>
                <asp:HiddenField ID="hfFieldIds" runat="server" />
            </asp:View>
            <asp:View ID="vRegistration" runat="server">
                 <div class="account-form-block">
      	            <div class="account-formInner">
      	            <div class="basic-form registration-form" method="post">
      	            <div class="cf">
        	            <div class="asset-title">ASSET REGISTRATION</div>
        	            <ul class="left-form cf">
          	            <li class="alignleft"><span class="lbl-txt1">Registration <br>State</span><span class="select-drop mediumLarge-drop"><select class="default" name="station-code">
					              <option value="">Select...</option>
					              <option value="">Registration State 1</option>
                        <option value="">Registration State 2</option>
                        <option value="">Registration State 3</option>
						            </select></span></li>
          	            <li class="alignright"><span class="lbl-txt">Expires On</span>
          	            <asp:TextBox ID="txtExpiresOn" runat="server" CssClass="input-field-all setDatePicker" ></asp:TextBox>
          	            </li>
                        <li class="alignleft"><span class="lbl-txt1">Government Contact Name</span><label><asp:TextBox ID="txtGovContactName" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright"><span class="lbl-txt">St. Address</span><label><asp:TextBox ID="txtStAddress" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignleft"><span class="lbl-txt">City</span><label><asp:TextBox ID="txtRegCity" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright"><span class="lbl-txt">State</span><span class="select-drop mediumLarge-drop"><select class="default" name="station-code">
					              <option value="">Select...</option>
					              <option value="">State 1</option>
                        <option value="">State 2</option>
                        <option value="">State 3</option>
						            </select></span></li>
                        <li class="alignleft"><span class="lbl-txt">Zip Code</span><label><asp:TextBox ID="txtRegZipCode" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright"><span class="lbl-txt">Country</span><span class="select-drop mediumLarge-drop"><select class="default" name="station-code">
					              <option value="">Select...</option>
					              <option value="">Country 1</option>
                        <option value="">Country 2</option>
                        <option value="">Country 3</option>
						            </select></span></li>
                        <li class="alignleft"><span class="lbl-txt">Telephone #</span><label><asp:TextBox ID="txtRegTelephone" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright"><span class="lbl-txt">Email</span><label><asp:TextBox ID="txtRegEmail" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
          	            <li class="alignleft"><span class="lbl-txt">Payment Method</span><label><asp:TextBox ID="txtRegPaymentMethod" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright"><span class="lbl-txt1">Website <br>Payment</span><label><asp:TextBox ID="txtWebsitePayment" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                      </ul>
                      <div class="asset-title">REGISTRATION DOCUMENTS<em class="down-arrow">&nbsp;</em></div>
                      
					            <div class="asset-img-block file-block cf">
          	            <div class="asset-file"><input type="file"></div>
          	            <div style="display:none;">
          	            <div class="asset-hover" ><ul class="cf">
            	            <li><a href="javascript: void(0);" ><img src="../StaticContents/img/asset-ico2.png" width="20" height="20"></a></li>
                          <li><a href="javascript: void(0);" ><img src="../StaticContents/img/asset-ico3.png" width="20" height="20"></a></li>
                          <li><a href="javascript: void(0);" ><img src="../StaticContents/img/asset-ico1.png" width="20" height="20"></a></li>
                        </ul><img src="../StaticContents/img/file-ico.png" width="79" height="79" alt="Image Name" title="Image Name"><strong>File Name</strong></div>
          	            <div class="asset-hover"><ul class="cf">
            	            <li><a href="javascript: void(0);" ><img src="../StaticContents/img/asset-ico2.png" width="20" height="20"></a></li>
                          <li><a href="javascript: void(0);" ><img src="../StaticContents/img/asset-ico3.png" width="20" height="20"></a></li>
                          <li><a href="javascript: void(0);" ><img src="../StaticContents/img/asset-ico1.png" width="20" height="20"></a></li>
                        </ul><img src="../StaticContents/img/file-ico.png" width="79" height="79" alt="Image Name" title="Image Name"><strong>File Name</strong></div>
                        <div class="asset-hover"><ul class="cf">
            	            <li><a href="javascript: void(0);" ><img src="../StaticContents/img/asset-ico2.png" width="20" height="20"></a></li>
                          <li><a href="javascript: void(0);" ><img src="../StaticContents/img/asset-ico3.png" width="20" height="20"></a></li>
                          <li><a href="javascript: void(0);" ><img src="../StaticContents/img/asset-ico1.png" width="20" height="20"></a></li>
                        </ul><img src="../StaticContents/img/file-ico.png" width="79" height="79" alt="Image Name" title="Image Name"><strong>Hovered</strong></div>
                        <div class="asset-hover"><ul class="cf">
            	            <li><a href="javascript: void(0);" ><img src="../StaticContents/img/asset-ico2.png" width="20" height="20"></a></li>
                          <li><a href="javascript: void(0);" ><img src="../StaticContents/img/asset-ico3.png" width="20" height="20"></a></li>
                          <li><a href="javascript: void(0);" ><img src="../StaticContents/img/asset-ico1.png" width="20" height="20"></a></li>
                        </ul><img src="../StaticContents/img/file-ico.png" width="79" height="79" alt="Image Name" title="Image Name"><strong>File Name</strong></div>
                        </div>
                      </div>
                      <div class="asset-title">MANAGED BY<em class="down-arrow">&nbsp;</em></div>
                      <ul class="left-form cf">
          	            <li class="alignleft"><span class="lbl-txt">First Name</span><label><asp:TextBox ID="txt1231" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright"><span class="lbl-txt">Last Name</span><label><asp:TextBox ID="txt2341" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignleft"><span class="lbl-txt">Telephone #</span><label><asp:TextBox ID="txt3451" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright asset-gen-btn"><span class="lbl-txt">Email</span><label><asp:TextBox ID="txt451" runat="server" CssClass="input-field-all"></asp:TextBox></label><a title="AUTO-GENERATE" class="generate-btn" href="javascript: void(0);" >EMAIL CONTACT</a></li>
                      </ul>
                      <div class="asset-title">INSURANCE</div>
                      <ul class="left-form cf">
          	            <li class="alignleft"><span class="lbl-txt">Carrier</span><span class="select-drop mediumLarge-drop"><select class="default" name="station-code">
					              <option value="">Select...</option>
					              <option value="">Carrier 1</option>
                        <option value="">Carrier 2</option>
                        <option value="">Carrier 3</option>
						            </select></span></li>
                                    <li class="alignright"><span class="lbl-txt">Expires On</span>
						            <asp:TextBox ID="txtInsuranceExpiresOn" runat="server" CssClass="input-field-all setDatePicker" ></asp:TextBox>
						            </li>
          	            <li class="alignleft"><span class="lbl-txt">Agent Name</span><label><asp:TextBox ID="txt1safd" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright"><span class="lbl-txt">Agency</span><label><asp:TextBox ID="txt12" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignleft"><span class="lbl-txt">Title</span><label><asp:TextBox ID="txt124" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright"><span class="lbl-txt">St. Address</span><label><asp:TextBox ID="txt61" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignleft"><span class="lbl-txt">City</span><label><asp:TextBox ID="txt341" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright"><span class="lbl-txt">State</span><span class="select-drop mediumLarge-drop"><select class="default" name="station-code">
					              <option value="">Select...</option>
					              <option value="">State 1</option>
                        <option value="">State 2</option>
                        <option value="">State 3</option>
						            </select></span></li>
                        <li class="alignleft"><span class="lbl-txt">Zip Code</span><label><asp:TextBox ID="txt21" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright"><span class="lbl-txt">Country</span><span class="select-drop mediumLarge-drop"><select class="default" name="station-code">
					              <option value="">Select...</option>
					              <option value="">State 1</option>
                        <option value="">State 2</option>
                        <option value="">State 3</option>
						            </select></span></li>
                        <li class="alignleft"><span class="lbl-txt">Telephone #</span><label><asp:TextBox ID="txt11" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright"><span class="lbl-txt">Email</span><label><asp:TextBox ID="txt1" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                      </ul>
                      <div class="asset-title">MANAGED BY<em class="down-arrow">&nbsp;</em></div>
                      <ul class="left-form cf">
          	            <li class="alignleft"><span class="lbl-txt">First Name</span><label><asp:TextBox ID="txt671" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright"><span class="lbl-txt">Last Name</span><label><asp:TextBox ID="txt156" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignleft"><span class="lbl-txt">Telephone #</span><label><asp:TextBox ID="txt175" runat="server" CssClass="input-field-all"></asp:TextBox></label></li>
                        <li class="alignright asset-gen-btn"><span class="lbl-txt">Email</span><label><asp:TextBox ID="txt51" runat="server" CssClass="input-field-all"></asp:TextBox></label><a title="AUTO-GENERATE" class="generate-btn" href="javascript: void(0);" >EMAIL CONTACT</a></li>
                      </ul>
                    </div>
                  </div>
      	            <div class="asset-btn-block last"><a href="javascript: void(0);"  class="small-gray-btn"><span>Cancel</span></a><a href="javascript: void(0);"  title="SAVE" class="small-blue-btn"><span>SAVE</span></a></div>
                  </div>
                  </div>
            </asp:View>
            <asp:View ID="vHistory" runat="server">
                  <ul class="cf secondaryNav" id="ul_history" runat="server">
      	            <li class="first">
      	            <asp:LinkButton ID="lnkbtnNotes" runat="server" CommandArgument="0" OnClick="lnkbtnHistoryChangeTab_Click" OnClientClick="ShowDefaultLoader();" CommandName="secondaryNav" 
      	                ToolTip="Notes">Notes</asp:LinkButton>
      	             </li>
                    <li>
                        <asp:LinkButton ID="lnkbtnPMInspections" runat="server" CommandArgument="1" OnClick="lnkbtnHistoryChangeTab_Click" OnClientClick="ShowDefaultLoader();" CommandName="secondaryNav" 
      	                ToolTip="PM Inspections">PM Inspections</asp:LinkButton>   
                     </li>
                    <li>
                        <asp:LinkButton ID="lnkbtnWeeklyInspections" runat="server" CommandArgument="2" OnClick="lnkbtnHistoryChangeTab_Click" OnClientClick="ShowDefaultLoader();" CommandName="secondaryNav" 
      	                ToolTip="Weekly Safety Inspections">Weekly Safety <br>Inspections</asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbtnManufacturerInspections" runat="server" CommandArgument="3" OnClick="lnkbtnHistoryChangeTab_Click" OnClientClick="ShowDefaultLoader();" CommandName="secondaryNav" 
      	                ToolTip="Manufacturer Recalls &amp; Inspections">Manufacturer <br>Recalls &amp; Inspections</asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbtnPartsHistory" runat="server" CommandArgument="4" OnClick="lnkbtnHistoryChangeTab_Click" OnClientClick="ShowDefaultLoader();" CommandName="secondaryNav" 
      	                ToolTip="Parts History">Parts History</asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbtnAssetTransfers" runat="server" CommandArgument="5" OnClick="lnkbtnHistoryChangeTab_Click" OnClientClick="ShowDefaultLoader();"  CommandName="secondaryNav" 
      	                ToolTip="Asset Transfers">Asset Transfers</asp:LinkButton>
                    </li>
                    <li class="last">
                        <asp:LinkButton ID="lnkbtnRepairOrders" runat="server" CommandArgument="6" OnClick="lnkbtnHistoryChangeTab_Click" OnClientClick="ShowDefaultLoader();" CommandName="secondaryNav"
      	                ToolTip="Repair Orders">Repair Orders</asp:LinkButton>
                    </li>
                  </ul>
                   <asp:MultiView ID="mvHistory" runat="server" ActiveViewIndex="0">
                        <asp:View ID="vNotes" runat="server">
                            <div class="records-block">
      	                        <div class="order-history-list">
      		                        <ul class="cf message-listing">
      		                            <asp:Repeater ID="rpNotes" runat="server">
      		                                <ItemTemplate>
      		                                    <li>
      		                                        <h4 class="cf msg-header">
      		                                            <strong><%# Eval("Name") %></strong>
      		                                            <span class="msg-date"><%# Convert.ToDateTime(Eval("CreatedDate")).ToString("MMM") %> </span>
      		                                        </h4>
        			                                <div class="msg-txt">
        			                                    <%# Eval("Description") %>
        			                                </div>
      			                                </li>
      		                                </ItemTemplate>
      		                            </asp:Repeater>
    				                        
      				                        
      			                        <li>
      				                        <h4 class="cf msg-header"><strong>John Doe</strong><span class="msg-date">January 17, 2013 8:00am</span></h4>
        			                        <div class="msg-txt">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam quis risus eget urna mollis ornare vel eu leo. Donec ullamcorper nulla non metus auctor fringilla. Vivamus sagittis lacus vel augue laoreet rutrum faucibus dolor auctor. Cras mattis consectetur purus sit amet fermentum.							</div>
      			                        </li>
                                    <li class="post-note">
            	                        <h5>Post Note:</h5>
                                      <label><textarea id="txtPostNote" runat="server" class="input-textarea"></textarea></label>
                                    </li>
                                    <li class="save-block">
            	                        <button class="small-blue-btn"><span>SUBMIT</span></button>
                                    </li>
                                    <li class="records-btn-block">
            	                        <a href="javascript: void(0);"  title="Cancel" class="small-gray-btn"><span>Cancel</span></a><a href="javascript: void(0);"  class="small-blue-btn" title="SAVE"><span>SAVE</span></a>
                                    </li>
      		                        </ul>
    	                        </div>	
                              </div>
                        </asp:View>
                        <asp:View ID="vPMInspections" runat="server">
                            <div class="records-block">
      	                        <div class="records-table">
        	                        <table>
          	                        <tr>
            	                        <th class="col1">Inspection Date</th>
                                      <th class="col2">Reference #</th>
                                      <th class="col3">Vendor</th>
                                      <th class="col4">Mechanic</th>
                                      <th class="col5">Report</th>
                                      <th class="col6">Invoice</th>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Lorem Ipsum</td>
                                      <td class="col2">Lorem Ipsum</td>
                                      <td class="col3">Bibendum Sollicitudin</td>
                                      <td class="col4">Lorem Ipsum</td>
                                      <td class="col5"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                      <td class="col6"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Dolor Sit</td>
                                      <td class="col2">Dolor Sit</td>
                                      <td class="col3">Sollicitudin Euismod</td>
                                      <td class="col4">Dolor Sit</td>
                                      <td class="col5"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                      <td class="col6"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Lorem Ipsum</td>
                                      <td class="col2">Lorem Ipsum</td>
                                      <td class="col3">Bibendum Sollicitudin</td>
                                      <td class="col4">Lorem Ipsum</td>
                                      <td class="col5"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                      <td class="col6"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                  </table>
                                  <div class="records-btn-block"><a href="javascript: void(0);"  title="Cancel" class="small-gray-btn"><span>Cancel</span></a><a href="javascript: void(0);"  class="small-blue-btn" title="SAVE"><span>SAVE</span></a></div>
                                </div>
                              </div>
                        </asp:View>
                        <asp:View ID="vWeeklyInspections" runat="server">
                            <div class="records-block">
      	                        <div class="records-table records3-table">
        	                        <table>
          	                        <tr>
            	                        <th class="col1">Inspection Date</th>
                                      <th class="col2">Reference #</th>
                                      <th class="col3">Inspector</th>
                                      <th class="col4">Days from Last Inspection</th>
                                      <th class="col5">Details</th>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Lorem Ipsum</td>
                                      <td class="col2">Lorem Ipsum</td>
                                      <td class="col3">Bibendum Sollicitudin</td>
                                      <td class="col4">Lorem Ipsum</td>
                                      <td class="col5"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Dolor Sit</td>
                                      <td class="col2">Dolor Sit</td>
                                      <td class="col3">Sollicitudin Euismod</td>
                                      <td class="col4">Dolor Sit</td>
                                      <td class="col5"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Lorem Ipsum</td>
                                      <td class="col2">Lorem Ipsum</td>
                                      <td class="col3">Bibendum Sollicitudin</td>
                                      <td class="col4">Lorem Ipsum</td>
                                      <td class="col5"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                  </table>
                                  <div class="records-btn-block"><a href="javascript: void(0);"  title="Cancel" class="small-gray-btn"><span>Cancel</span></a><a href="javascript: void(0);"  class="small-blue-btn" title="SAVE"><span>SAVE</span></a></div>
                                </div>
                            </div>
                        </asp:View>
                        <asp:View ID="vManufacturerInspections" runat="server">
                            <div class="records-block">
      	                        <div class="cf post-btn"><a href="javascript: void(0);"  title="POST INSPECTION" class="small-blue-btn"><span>POST INSPECTION</span></a></div>
      	                        <div class="records-table records4-table">
        	                        <table>
          	                        <tr>
            	                        <th class="col1">Notification Date</th>
                                      <th class="col2">Inspection Date</th>
                                      <th class="col3">Reference #</th>
                                      <th class="col4">Inspector</th>
                                      <th class="col5">Status</th>
                                      <th class="col6">Details</th>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Lorem Ipsum</td>
                                      <td class="col2">Lorem Ipsum</td>
                                      <td class="col3">Bibendum Sollicitudin</td>
                                      <td class="col4">Lorem Ipsum</td>
                                      <td class="col5">Bibendum Sollicitudin</td>
                                      <td class="col6"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Dolor Sit</td>
                                      <td class="col2">Dolor Sit</td>
                                      <td class="col3">Sollicitudin Euismod</td>
                                      <td class="col4">Dolor Sit</td>
                                      <td class="col5">Sollicitudin Euismod</td>
                                      <td class="col6"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Lorem Ipsum</td>
                                      <td class="col2">Lorem Ipsum</td>
                                      <td class="col3">Bibendum Sollicitudin</td>
                                      <td class="col4">Lorem Ipsum</td>
                                      <td class="col5">Bibendum Sollicitudin</td>
                                      <td class="col6"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                  </table>
                                  <div class="records-btn-block"><a href="javascript: void(0);"  title="Cancel" class="small-gray-btn"><span>Cancel</span></a><a href="javascript: void(0);"  class="small-blue-btn" title="SAVE"><span>SAVE</span></a></div>
                                </div>
                              </div>
                        </asp:View>
                        <asp:View ID="vPartsHistory" runat="server">
                            <div class="records-block">
      	                        <div class="cf post-btn"><a href="javascript: void(0);"  title="POST PART" class="small-blue-btn"><span>POST PART</span></a></div>
      	                        <div class="records-table records5-table">
        	                        <table>
          	                        <tr>
            	                        <th class="col1">Purchase Date</th>
                                      <th class="col2">Quantity</th>
                                      <th class="col3">Description</th>
                                      <th class="col4">Install Vendor</th>
                                      <th class="col5">Mechanic</th>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Lorem Ipsum</td>
                                      <td class="col2">Lorem Ipsum</td>
                                      <td class="col3">Bibendum Sollicitudin</td>
                                      <td class="col4">Lorem Ipsum</td>
                                      <td class="col5">Bibendum Sollicitudin</td>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Dolor Sit</td>
                                      <td class="col2">Dolor Sit</td>
                                      <td class="col3">Sollicitudin Euismod</td>
                                      <td class="col4">Dolor Sit</td>
                                      <td class="col5">Sollicitudin Euismod</td>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Lorem Ipsum</td>
                                      <td class="col2">Lorem Ipsum</td>
                                      <td class="col3">Bibendum Sollicitudin</td>
                                      <td class="col4">Lorem Ipsum</td>
                                      <td class="col5">Bibendum Sollicitudin</td>
                                    </tr>
                                  </table>
                                  <div class="records-btn-block"><a href="javascript: void(0);"  title="Cancel" class="small-gray-btn"><span>Cancel</span></a><a href="javascript: void(0);"  class="small-blue-btn" title="SAVE"><span>SAVE</span></a></div>
                                </div>
                              </div>
                        </asp:View>
                        <asp:View ID="vAssetTransfers" runat="server">
                             <div class="records-block">
      	                        <div class="cf post-btn"><a href="javascript: void(0);"  class="small-blue-btn"><span>POST TRANSFER</span></a><a href="javascript: void(0);"  title="POST PART" class="small-blue-btn"><span>REQUEST QUOTE</span></a></div>
      	                        <div class="records-table records6-table">
        	                        <table>
          	                        <tr>
            	                        <th class="col1">Transfer Date</th>
                                      <th class="col2">Last Location</th>
                                      <th class="col3">New Location</th>
                                      <th class="col4">Transfer</th>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Lorem Ipsum</td>
                                      <td class="col2">Lorem Ipsum</td>
                                      <td class="col3">Bibendum Sollicitudin</td>
                                      <td class="col4"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Dolor Sit</td>
                                      <td class="col2">Dolor Sit</td>
                                      <td class="col3">Sollicitudin Euismod</td>
                                      <td class="col4"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Lorem Ipsum</td>
                                      <td class="col2">Lorem Ipsum</td>
                                      <td class="col3">Bibendum Sollicitudin</td>
                                      <td class="col4"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                  </table>
                                  <div class="records-btn-block"><a href="javascript: void(0);"  title="Cancel" class="small-gray-btn"><span>Cancel</span></a><a href="javascript: void(0);"  class="small-blue-btn" title="SAVE"><span>SAVE</span></a></div>
                                </div>
                              </div>
                        </asp:View>
                        <asp:View ID="vRepairOrders" runat="server">
                            <div class="records-block">
      	                        <div class="cf post-btn"><a href="javascript: void(0);"  title="POST PART" class="small-blue-btn"><span>CREATE REPAIR</span></a></div>
      	                        <div class="records-table records7-table">
        	                        <table>
          	                        <tr>
            	                        <th class="col1">Repair Date</th>
                                      <th class="col2">Repair #</th>
                                      <th class="col3">Description</th>
                                      <th class="col4">Status</th>
                                      <th class="col5">Details</th>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Lorem Ipsum</td>
                                      <td class="col2">Lorem Ipsum</td>
                                      <td class="col3">Bibendum Sollicitudin</td>
                                      <td class="col4">Bibendum Sollicitudin</td>
                                      <td class="col5"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Dolor Sit</td>
                                      <td class="col2">Dolor Sit</td>
                                      <td class="col3">Sollicitudin Euismod</td>
                                      <td class="col4">Sollicitudin Euismod</td>
                                      <td class="col5"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                    <tr>
            	                        <td class="col1">Lorem Ipsum</td>
                                      <td class="col2">Lorem Ipsum</td>
                                      <td class="col3">Bibendum Sollicitudin</td>
                                      <td class="col4">Bibendum Sollicitudin</td>
                                      <td class="col5"><a href="javascript: void(0);"  class="small-blue-btn"><span>View</span></a></td>
                                    </tr>
                                  </table>
                                  <div class="records-btn-block"><a href="javascript: void(0);"  title="Cancel" class="small-gray-btn"><span>Cancel</span></a><a href="javascript: void(0);"  class="small-blue-btn" title="SAVE"><span>SAVE</span></a></div>
                                </div>
                              </div>
                        </asp:View>
                   </asp:MultiView>
            </asp:View>
        </asp:MultiView>
        
    </div>
    
    <asp:HiddenField ID="hfSectionName" runat="server" />
    
    <div id="add-note-popup" class="popup-outer ParentTab" data-defaultbutton="<%= lnkbtnSaveNote.ClientID %>">
	                <div class="popupInner">
	                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Asset Management','Add Basic Note');">Help Video</a>
			                <a id="addnote_close_a" class="close_a clientsidectrl" onclick="ClosePopUp(this,event,'close_a',true);" href="javascript: void(0);">Close</a>
		                <div class="assetspoupup-content cf">
		                
			                
			                <h2><span class="popup-asset-title"></span> Add Note </h2>
			                
			                <div class="request-form">
				                    <ul class="cf">
						                <li class="alignleft">
							                <span class="lbl-txt">Note</span>
							                <textarea id="txtNoteDetails" runat="server" class="input-textarea"></textarea>
						                </li>						
					                </ul>
					                <div class="warranty-form-block">
						                <a id="addnote_cancel_a" href="javascript: void(0);" title="Cancel" class="gray-home-btn cancel_a clientsidectrl" onclick="ClosePopUp(this,event,'cancel_a',true);"><span>Cancel</span></a>
						                <asp:LinkButton ID="lnkbtnSaveNote" runat="server" CssClass="blue-home-btn" ToolTip="Save Note" OnClick="lnkbtnSaveNote_Click"><span>Save Note</span></asp:LinkButton>
					                </div>
			                </div>
		                </div>
	                </div>
    </div>
    
    <div id="view-note-popup" class="popup-outer ParentTab" data-defaultbutton="<%= lnkbtnAssetsNote.ClientID %>">
	                <div class="popupInner">
		                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Asset Management','View flagged notes');">Help Video</a>
			            <a id="viewnote_close_a" class="close_a clientsidectrl" onclick="ClosePopUp(this,event,'close_a',true);" href="javascript: void(0);">Close</a>
		                <div class="assetspoupup-content cf">
		                
			                <h2><span class="popup-asset-title"></span> View Note</h2>
			                
			                <div class="message-container">
					                <div id="Div1">
						                <div class="viewnote-listbox">
							                <ul class="viewnote-list applyScroll cf">
							                    <asp:Repeater ID="rpBasicViewNote" runat="server">
							                        <ItemTemplate>
							                            <li>
								                            <h5 class="cf"><asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("FirstName")+" "+Eval("LastName") %>'></asp:Label><em><%# Eval("DateNTime") %></em></h5>
								                            <p><%# Eval("Notecontents") %></p>
								                        </li>    
							                        </ItemTemplate>
							                    </asp:Repeater>
							                    <li id="nobasicnotes_li" runat="server" style="text-align:center;" visible="false">
							                        <p>No data found.</p>
							                    </li>
                                                <%--<li class="text-area">
                                                  <textarea id="txtFlaggedNoteDetails" runat="server" class="input-textarea checkvalidation"></textarea>
                                                  <asp:RequiredFieldValidator ID="rfvFlaggedNoteDetails" runat="server" ErrorMessage="Please enter note" Display="Dynamic" CssClass="error"
                                            ControlToValidate="txtFlaggedNoteDetails" ValidationGroup="ViewFlaggedNotes" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                </li>--%>
							                </ul>
							                <div class="text-area">
							                     <textarea id="txtFlaggedNoteDetails" runat="server" class="input-textarea checkvalidation"></textarea>
                                                  <asp:RequiredFieldValidator ID="rfvFlaggedNoteDetails" runat="server" ErrorMessage="Please enter note" Display="Dynamic" CssClass="error"
                                                ControlToValidate="txtFlaggedNoteDetails" ValidationGroup="ViewFlaggedNotes" SetFocusOnError="True"></asp:RequiredFieldValidator>
							                </div>
                                            <div class="notes-btn-block cf">
                                                <a id="viewnote_cancel_a" class="small-gray-btn cancel_a clientsidectrl" onclick="ClosePopUp(this,event,'cancel_a',true);" href="javascript: void(0);"><span>Cancel</span></a> 
                                                <asp:LinkButton ID="lnkbtnAssetsNote" runat="server" CssClass="small-blue-btn submit" OnClick="lnkbtnAssetsNote_Click" ToolTip="Save" ValidationGroup="ViewFlaggedNotes" call="ViewFlaggedNotes"><span>SAVE</span></asp:LinkButton>
                                                <asp:LinkButton ID="lnkbtnAssetsUnflagged" runat="server" CssClass="small-gray-btn" OnClick="lnkbtnAssetsUnflagged_Click" ToolTip="Unflagged"><span>Unflagged</span></asp:LinkButton>
                                                
                                            </div>

						                </div>
				                  </div>					
			                </div>
		                </div>
	                </div>
                </div>
     
    <div id="sendmail-popup" class="popup-outer popupouter-center ParentTab" data-defaultbutton="<%= lnkbtnSendMail.ClientID %>">
                 <div class="popupInner">
                        <div class="specs-popup">
                            <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Asset Management','Send Mail');">Help Video</a>
                            <a id="sendmail_close_a" class="close_a clientsidectrl" onclick="ClosePopUp(this,event,'close_a',true);" href="javascript:void(0);">Close</a>
                            <div class="warranty-content">
                                <h2><span class="popup-asset-title"></span> Send Mail </h2>
                                
                                <div class="warranty-form">
                                    <ul class="cf">
                                        <li class="alignleft"><span class="lbl-txt1">
                                            Email To </span>
                                            <label>
                                            <asp:TextBox ID="txtToEmail" runat="server" CssClass="input-field-all input-large checkvalidation"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvToEmail" runat="server" ErrorMessage="Please enter email" Display="Dynamic" CssClass="error"
                                            ControlToValidate="txtToEmail" ValidationGroup="SendMail" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revToEmail" runat="server" ControlToValidate="txtToEmail"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SendMail"
                                                ErrorMessage="Please enter valid email." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                            </asp:RegularExpressionValidator>
                                            <asp:HiddenField ID="hfAssetFiles" runat="server" />
                                            <asp:HiddenField ID="hfFileFor" runat="server" />
                                            <asp:HiddenField ID="hfSubject" runat="server" />
                                            <asp:HiddenField ID="hfFileID" runat="server" />
                                            </label></li>
                                                  <li class="alignleft"><span class="lbl-txt1">
                                            Message</span>
                                            <label>
                                                    <textarea id="txtMailMessage" runat="server" class="input-textarea checkvalidation"></textarea>
                                                    <asp:RequiredFieldValidator ID="rfvEmailMessage" runat="server" ErrorMessage="Please enter message" Display="Dynamic" CssClass="error"
                                            ControlToValidate="txtMailMessage" ValidationGroup="SendMail" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </label></li>
                                            
                                    </ul>
                                    <div class="warranty-form-block">
                                        <a id="sendmail_cancel_a" href="javascript:void(0);" title="Cancel" onclick="ClosePopUp(this,event,'cancel_a',true);" class="gray-home-btn cancel_a clientsidectrl"><span>Cancel</span></a><asp:LinkButton
                                            ID="lnkbtnSendMail" runat="server" CssClass="blue-home-btn submit" ToolTip="Send Mail" ValidationGroup="SendMail" call="SendMail"
                                            OnClick="lnkbtnSendMail_Click"><span>Send Mail</span></asp:LinkButton></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
    
    <div id="Upload-File-popup" class="popup-outer ParentTab" data-defaultbutton="<%= lnkbtnFileUpload.ClientID %>">
                 <div class="popupInner">
                        <div class="specs-popup">
                            <a class="help-video-btn" title="Help Video" href="javascript: void(0);"  onclick="GetHelpVideo('Asset Management','Upload File');">Help Video</a>
                            <a id="uploadfile_close_a" class="close_a clientsidectrl" onclick="ClosePopUp(this,event,'close_a',true);" href="javascript:void(0);">Close</a>
                            <div class="warranty-content">
                                <h2><span class="popup-asset-title"></span> Upload File</h2>
                                
                                <div class="warranty-form">
                                    <ul class="cf">
                                        <li class="alignleft"><span class="lbl-txt1">
                                            File Name </span>
                                            <label>
                                            <asp:TextBox ID="txtFileName" runat="server" CssClass="input-field-all input-large checkvalidation" MaxLength="25"></asp:TextBox>
                                            <asp:HiddenField ID="hdnFileFor" runat="server" />
                                            <asp:HiddenField ID="hfFileNamesClass" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvFileName" runat="server" ErrorMessage="Please enter file name" Display="Dynamic" CssClass="error"
                                            ControlToValidate="txtFileName" ValidationGroup="UploadFiles" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvFileNameExist" runat="server" ErrorMessage="File name already exists" ValidationGroup="UploadFiles"
                                            ControlToValidate="txtFileName" CssClass="error" Display="Dynamic" SetFocusOnError="True" ClientValidationFunction="CheckNameAlreadyExists"></asp:CustomValidator>
                                            </label></li>
                                                  <li class="alignleft"><span class="lbl-txt1">
                                            Browse</span>                                            
                                            <label class="input-file-upload">
                                                    <asp:FileUpload ID="fuCommonFileUpload" runat="server" CssClass="checkvalidation" />
                                                    <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ErrorMessage="Please select file" Display="Dynamic" CssClass="error"
                                            ControlToValidate="fuCommonFileUpload" ValidationGroup="UploadFiles" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revAssetImages" runat="server" ErrorMessage="Only jpeg/png/mp4 files allowed."
                                            CssClass="error" ControlToValidate="fuCommonFileUpload" Display="Dynamic" SetFocusOnError="False" Enabled="false"
                                            ValidationGroup="UploadFiles" ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG|gif|GIF|mp4|MP4)$"></asp:RegularExpressionValidator>
                                            <asp:RegularExpressionValidator ID="revAssetDocuments" runat="server" ErrorMessage="Only pdf/doc/docx files allowed."
                                            CssClass="error" ControlToValidate="fuCommonFileUpload" Display="Dynamic" SetFocusOnError="False"
                                            ValidationGroup="UploadFiles" ValidationExpression="^.*\.(pdf|PDF|doc|DOC|docx|DOCX)$"></asp:RegularExpressionValidator>
                                            <span class="fileformate" id="format_span">Only PDF, Word Doc files are allowed. Not available on mobile devices.</span>
                                            </label>
                                             
                                            </li>
                                            
                                    </ul>
                                    <div class="warranty-form-block">
                                        <a id="uploadfile_cancel_a" href="javascript:void(0);" title="Cancel" onclick="ClosePopUp(this,event,'cancel_a',true);" class="gray-home-btn cancel_a clientsidectrl"><span>Cancel</span></a><asp:LinkButton
                                            ID="lnkbtnFileUpload" runat="server" CssClass="blue-home-btn submit" ToolTip="Upload File" ValidationGroup="UploadFiles" call="UploadFiles"
                                            OnClick="lnkbtnFileUpload_Click"><span>Upload File</span></asp:LinkButton></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
  
    <div id="add-newfield-popup" class="popup-outer popupouter-center ParentTab" data-defaultbutton="<%= lnkbtnAddField.ClientID %>">
                    <div class="popupInner">
                        <div class="specs-popup">
                        <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Asset Management','Add New Field');">Help Video</a>
                            <a id="addnewfield_close_a" class="close_a clientsidectrl" onclick="ClosePopUp(this,event,'close_a',true);" href="javascript:void(0);">Close</a>
                            <div class="warranty-content">
                                <h2><span class="popup-asset-title"></span> Add New Field</h2>
                                <div class="warranty-form">
                                    <ul class="cf">
                                        <li class="typeoffield-radio alignleft"><span class="lbl-txt">Type of Field</span>
                                            <asp:Repeater ID="rpFieldType" runat="server" >
                                                <ItemTemplate>
                                                    
                                                    <asp:HiddenField ID="hfValue" runat="server" Value='<%# Eval("Value") %>' />
                                                    <label class="label_radiobox" style="margin-right:100px;">
                                                        <asp:RadioButton CssClass="FieldTypes iradio_flat" ID="rdbtnControlType" runat="server" GroupName="FieldType" />
                                                        <%# Eval("Display") %>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </li>
                                        <li class="alignleft"><span class="lbl-txt">Name of Field</span><label>
                                        <asp:TextBox ID="txtFieldName" runat="server" CssClass="input-field-all input-large " Text="Custom Text For Testing"></asp:TextBox>
                                         <%--<asp:RequiredFieldValidator ID="rfvNewFieldText" runat="server" ErrorMessage="Please enter field name" Display="Dynamic" CssClass="error"
                                            ControlToValidate="txtFieldName" ValidationGroup="BasicAddNewField" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                        </label></li>
                                        <li class="typeoffield-radio radio-right">
                                            
                                            <label class="label_radiobox">
                                                <asp:RadioButton CssClass="iradio_flat" ID="rdbtnThisAssetOnly" runat="server" GroupName="CreateFor" Checked="true" /> Create field for this asset only
                                            </label>
                                        </li>
                                        <li class="typeoffield-radio radio-right">
                                            
                                            <label class="label_radiobox">
                                                <asp:RadioButton CssClass="iradio_flat" ID="rdbtnAllAsset" runat="server" GroupName="CreateFor" /> Create field for ALL of this type of asset
                                            </label>
                                        </li>
                                    </ul>
                                    <div class="warranty-form-block">
                                        <a id="addnewfield_cancel_a" href="javascript:void(0);" title="Cancel" onclick="ClosePopUp(this,event,'cancel_a',true);" class="gray-home-btn cancel_a clientsidectrl"><span>Cancel</span></a>
                                        <asp:LinkButton ID="lnkbtnAddField" runat="server" CssClass="blue-home-btn submit" ToolTip="Add Field"
                                            OnClick="lnkbtnAddField_Click" ValidationGroup="BasicAddNewField" call="BasicAddNewField"><span>Add Field</span></asp:LinkButton></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
  
    </section>
</asp:Content>
