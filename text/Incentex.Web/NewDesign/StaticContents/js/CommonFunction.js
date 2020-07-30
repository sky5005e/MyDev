
// check if value is numeric in textbox
function CheckNum(id) {
    var txt = $("#" + id);
    if (!IsNumeric(txt.val())) {
        $(txt).addClass("ErrorField");
        $(txt).val('');
        $(txt).focus();
        return
    }
}
function IsNumeric(sText) {
    var ValidChars = "0123456789";
    var IsNumber = true;
    var Char;
    for (i = 0; i < sText.length && IsNumber == true; i++) {
        Char = sText.charAt(i);
        if (ValidChars.indexOf(Char) == -1) {
            IsNumber = false;
        }
    }
    return IsNumber;
}
function AllowNumbersONLY(evt)
{
    var k = evt.which || evt.keyCode;
    if( (k > 47 && k <58) || k == 46 || (k >= 8 && k <= 13) || (k >= 35 && k <= 40))
        return true;
    else
        return false;
}
function AllowForRenamingUploadedFile(evt)
{
     var k = evt.which || evt.keyCode;
    if( (k > 47 && k <58) || k == 45 || (k >= 8 && k <= 13) || (k >= 35 && k <= 40) || (k > 64 && k <91)|| k == 95 || (k > 96 && k <123))
        return true;
    else
        return false;
}

function GeneralAlertMsg(_msg, _hideFadeLayer,RedirectURL) {
    $("#GeneralMsg-master-popup").css('top', '0');
    $(".fade-layer").show();
    if(_hideFadeLayer)
        $("#btncloseMsg").removeAttr("onclick").attr("onclick","HideMsg(false)");
    $("#dvGeneralMsg").show();
    $("#pMsg").html(_msg);
    if(RedirectURL && RedirectURL.trim() != '')
        $("#RedirectURL_Span").html(RedirectURL);
    else
        $("#RedirectURL_Span").html("");
}
function GeneralConfirmationMsg(_msg) {
    $("#GeneralConfirmation-master-popup").css('top', '0');
    $(".fade-layer").show();
    $("#dvGeneralConfirmationMsg").show();
    $("#pdMsg").html(_msg);
}
function CloseGeneralConfirmation(moveToNextTab) {
    $("#GeneralConfirmation-master-popup").css('top', '-9999px');
    $(".fade-layer").hide();
    $("#dvGeneralConfirmationMsg").hide();
    $("#pdMsg").html('');    
    if (moveToNextTab) {        
        var liCtrl = $("li[tab-id='" + $("#hdnClickedTab").val() + "']");
        $.changeTab(liCtrl, true);
    }
    
}
function CheckForFormChange()
{
    var Current_Form_data = $("#" + $(".MainDiv").val()).find("input, textarea, select").serialize();
    if(form_original_data == Current_Form_data)
    {
        alert("No change");
    }
    else
    {
        alert("Change occured");
        $("#aspnetForm").reset();
    }
}
//function GeneralConfirmationMsgForRecordSave(_msg,ServerCtrlID,SaveButtonCtrolID,event) {
function GeneralConfirmationMsgForRecordSave(_msg,CtrlID,event,IsCssClassNameForCtrlID) {
    if($(".ctrlinfochanged").val().trim().toLowerCase() == "true")
    { 
        $("#GeneralConfirmation-master-popup-recordsave").css('top', '0');
        $(".fade-layer").show();
        $("#dvGeneralConfirmationForRecordsSave").show();
        $("#pdMsgForRecordSave").html(_msg);
        //$("#hdnSaveButtonToTrigger").val(SaveButtonCtrolID);
        $(".ctrlthatfiresevent").val(CtrlID);
        //alert("cl=" + IsCssClassNameForCtrlID);
        if(IsCssClassNameForCtrlID)
        {
            $(".ctrlthatfiresevent").removeClass("classname").removeClass("noclass").addClass("classname");
            //alert($(".ctrlthatfiresevent").attr("class"));
        }
        else
            $(".ctrlthatfiresevent").removeClass("classname").removeClass("noclass").addClass("noclass");
        //alert("op=" + $(".ctrlthatfiresevent").val());
        event.preventDefault();
        //event.stopPropagation();
        //event.stopImmediatePropagation();
        //return false;
    }
}
function CloseGeneralConfirmationForRecordsSave(moveToNextTab, isMultiTabPopUp) {
    $("#GeneralConfirmation-master-popup-recordsave").css('top', '-9999px');
    $(".fade-layer").hide();
    $("#dvGeneralConfirmationForRecordsSave").hide();
    $("#pdMsgForRecordSave").html('');    
    if (moveToNextTab && isMultiTabPopUp) {        
        var liCtrl = $("li[tab-id='" + $("#hdnClickedTab").val() + "']");
        $.changeTab(liCtrl, true);
    }
    else
    {
        if(moveToNextTab)
        {
            //set the Text Changes property to false;
            $(".ctrlinfochanged").val('false');
            FireServerClickEvent($(".ctrlthatfiresevent").val(),$(".ctrlthatfiresevent").hasClass("classname"));
            var SaveButton = $("#" + $(".MainDiv").val()).attr("data-defaultbutton");
            //alert("SaveButton=" + SaveButton);
            $(".savebuttontotrigger").val(SaveButton);
            $("#aspnetForm").reset();
        }
        else
        {
            FireServerClickEvent($("#" + $(".MainDiv").val()).attr("data-defaultbutton"),$(".ctrlthatfiresevent").hasClass("classname"));
        }
    }
}
function GeneralConfirmationMsgForDelete(_msg,CtrlID,event) {
    $("#GeneralConfirmation-master-popup-delete").css('top', '0');
    $(".fade-layer").show();
    $("#dvGeneralConfirmationForDelete").show();
    $("#confirmMesage").html(_msg);
    $("#ButtonIDToTrigger").html(CtrlID);  
    event.preventDefault();
}
function CloseGeneralConfirmationForDelete(returnValue) {
    $("#GeneralConfirmation-master-popup-delete").css('top', '-9999px');
    $(".fade-layer").hide();
    $("#dvGeneralConfirmationForDelete").hide();
    $("#confirmMesage").html('');    
    if(returnValue)
    {
        FireServerClickEvent($("#ButtonIDToTrigger").html());
    }
    return returnValue;
}
function FireServerClickEvent(TargetCtrlIDorClassName,IsClientSideCtrl)
{
    //alert(TargetCtrlIDorClassName +" === " +IsClientSideCtrl);
    //debugger;
    ShowDefaultLoader();
    var ButtonToTrigger = $("#" + TargetCtrlIDorClassName);
    if(IsClientSideCtrl && ButtonToTrigger.attr("id") == undefined)
    {
        ButtonToTrigger = $("." + TargetCtrlIDorClassName);
    }
    if(ButtonToTrigger)
    {
        //document.getElementById(ButtonToTrigger.attr("id")).click();
        if(IsClientSideCtrl && ButtonToTrigger.hasClass("clientsidectrl"))
        {
            ButtonToTrigger.click();
            ShowDefaultLoader(false);
        }
        else
        {
            ButtonToTrigger.removeAttr("onclick");
            ShowDefaultLoader();
            if(ButtonToTrigger.attr('href'))
            {
                window.location = ButtonToTrigger.attr('href') + ";";
            }
            else 
            {
                window.location = "javascript:__doPostBack('" + ButtonToTrigger.attr('name') + "','')";
            }
        }
    }
//    else
//        alert("No Ctrl Found");
}

function HideMsg(flag) {
    $("#GeneralMsg-master-popup").css('top', '-9999px');
    if(flag)
        $(".fade-layer").hide();
    $("#dvGeneralMsg").hide();
    var RedirectURL = $("#RedirectURL_Span").html();
    if(RedirectURL != "")
        window.location = RedirectURL;
}

//Function to scroll to a specific tag when the page is postback/redirect-----------------START
function ScrollToTag(TargetName,IsTargetByClass)
{
    if(!IsTargetByClass)
    {
        $('html, body').animate({
        scrollTop: $("#" + TargetName).offset().top
        }, 200);
    }
    else
    {
        $('html, body').animate({
        scrollTop: $("." + TargetName).offset().top
        }, 200);
    }
}
//Function to scroll to a specific tag when the page is postback/redirect-----------------ENDS
// Function to Check Session Existance from Ajax Call
function CheckSessionExistance(siteurl) {
    $.ajax({
        type: "POST",
        url: siteurl + "UserPages/WSUser.asmx/IsUserSessionExist",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        processData: false,
        dataType: "json",
        success: function(msg) {
            if (msg.d == false) {
                window.location = siteurl + "login.aspx";
            }
        },
        error: function(x, e) {
            //window.location = siteurl + "login.aspx";
        }
    });
}

// Function for "Enter" keyword hit ------ START
function clickButton(e, buttonid) {
    var bt = document.getElementById(buttonid);
    if (typeof (bt) == 'object') {
        if (navigator.appName.indexOf("Netscape") > -1) {
            if (e.keyCode == 13) {
                if (bt && typeof (bt.click) == 'undefined') {
                    bt.click = addClickFunction1(bt);
                }
                else
                    bt.click();
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
    var result = true;
    if (bt.onclick) result = bt.onclick();
    if (typeof (result) == 'undefined' || result) {
        eval(bt.href);
    }
}
// Function for "Enter" keyword hit ------ END


//function when custom dropdown control is changed ------ Starts
function SetDropValue(DropDownCtrl)
{
        //select-drop
       var MainLabel = $(DropDownCtrl).parents(".select-drop");
       var CurrDropDownID = $(MainLabel).children("select").attr("id");
      
       if($("#" + CurrDropDownID).hasClass("checkvalidation"))
       {
         //Trigger the javascript 'change' event of the dropdown 
         $.fn.ValidationUI.ValidateMe({}, $("#" + CurrDropDownID));
       }
       if($("#" + CurrDropDownID).attr("onchange") != undefined)
       {
            $("#" + CurrDropDownID).trigger("change");
       }
}
//function when custom dropdown control is changed ------ Ends

//function to populate the Dropdown ----------------------Starts
function populateDropdown(select, data,AppendDefaultSelectOption) {
    select.html('');
    if(AppendDefaultSelectOption)
        select.append($('<option></option>').val(0).html("-- Select --"));
    $.each(data, function (id, option) {
        select.append($('<option></option>').val(option.Value).html(option.Text));
    });
}
//function to populate the Dropdown ----------------------End

//function to BindDropdown for Array Object
function BindDropdown(Dropdown, data, select) {
    if(Dropdown.attr("id")!= undefined)
    {
        Dropdown.get(0).options.length = 0;
        Dropdown.empty().append($("<option></option>").val(0).html(select));
        Dropdown.get(0).selectedIndex = 0;
        for (i = 0; i < data.length; i++) {
            Dropdown.append($("<option></option>").val(data[i]).html(data[i]));
        }
    }
}

//Function to find and set the Selected Value for the Dropdown Control
function SetOptionSelectedByText(Ctrl,SelectedOption,ByValueorText)
{
    if(ByValueorText == 'Text')
        $(Ctrl).find("option").filter(function () { return (this.text == SelectedOption); }).attr("selected", "selected");
    else
        $(Ctrl).find("option").filter(function () { return (this.value == SelectedOption); }).attr("selected", "selected");
}

//Function TO Bind the Pop Up Events when using Update Panel(Asynchronous Postback)-------------------------------STARTS
var CheckChange;
function BindPopUpCloseEvents(CheckForChange,_msg,CtrlID,Oldevent,IsCssClassNameForCtrlID)
{
   //alert("CheckForChange = " + CheckForChange);
    $('.close-btn').on("click",function (event) {
        if(CheckForChange)
            CheckChange = CheckForChange;
            
        if(CheckForChange)
        {
            if($(".ctrlinfochanged").val().trim().toLowerCase() == "true")
            { 
            //$(this).attr("id")
                GeneralConfirmationMsgForRecordSave(_msg,'close-btn',event,true);
            }
        }
        else
        {
            var CurrentMainDiv = $(".ParentTab").filter(function() { if( $(this).css("display") && $(this).css("display") != "none") return this; });
            $(".MainDiv").val(CurrentMainDiv.attr("id"));
            $(".savebuttontotrigger").val($("dvBasic").attr("data-defaultbutton"));
            $('.popup-outer').css('top', '-9999px');
            $('.fade-layer').hide();
        }
    });
    
    $('.cancel-popup').on("click",function (event) {
        //alert("call");
        if(CheckForChange)
            CheckChange = CheckForChange;
        if(CheckForChange || typeof(CheckChange) != "undefined" )
        {
           if($(".ctrlinfochanged").val().trim().toLowerCase() == "true")
           { 
               GeneralConfirmationMsgForRecordSave(_msg,'cancel-popup',event,true);
           }
          
        }
        else
        {
            //alert("else");
            $('.popup-outer').css('top', '-9999px');
            $('.fade-layer').hide();
            //alert(typeof(CheckChange) + "===" + $(".ctrlinfochanged").val().trim().toLowerCase());
            if(typeof(CheckChange) != "undefined" || $(".ctrlinfochanged").val().trim().toLowerCase() == "false")
            {
                var CurrentMainDiv = $(".ParentTab").filter(function() { if( $(this).css("display") && $(this).css("display") != "none") return this; });
                //alert(CurrentMainDiv.length);
                $(".MainDiv").val(CurrentMainDiv.attr("id"));
                $(".savebuttontotrigger").val($("#" + $(".MainDiv").val()).attr("data-defaultbutton"));
            }
        }
    });
    
    $('.iframeCancel').click(function () {
        window.parent.TriggerClickEvent('close-btn',true);
    });
}
//Function TO Rebind the Pop Up Events when using Update Panel(Asynchronous Postback)-------------------------------ENDS

//Function to show the Default Loader for the system-------------------------------STARTS
 function ShowDefaultLoader(HideIT){
    if(typeof(HideIT) == undefined || HideIT == true)
        $(".progress-layer").show();
    else
        $(".progress-layer").hide();
 }
 function ShowLoader(Status){
    $(".progress-layer").toggle(Status);
}
//Function to show the Default Loader for the system-------------------------------ENDS

//Function TO Reset the ASP Validator inside the popups -------------------------------ENDS
function Page_ClientValidateReset(ValidatorArray) {
        if (typeof (ValidatorArray) != "undefined") {
            for (var i = 0; i < ValidatorArray.length; i++) {
                var validator = Page_Validators[i]; 
                validator.isvalid = true;
                $.fn.ValidationUI.Reset({}, validator);
            }
        }
    }
//Function TO Reset the ASP Validator inside the popups-------------------------------ENDS

function IsDecimal(amount)
{
    /*
    /^(\d+\.?\d{0,9}|\.\d{1,9})$/
    */
    var pattern = /^(\d+\.?\d{0,9}|\.\d{1,9})$/;
    if(pattern.test(amount))
        return true;
    else
        return false;

}
function TriggerClickEvent(Ctrl,IsClassName)
{
    if(IsClassName)
        $('.' + Ctrl).click();
    else
        $('#' + Ctrl).click();
}
function SelectMe(Ctrl)
{
    $(Ctrl).select();
}

function CheckCustomTextBox(Ctrl)
{
     var ParentSpan = $(Ctrl).parent("span.popup");
     if($(ParentSpan).attr("data-content") != undefined)
     {
        var DivIdsClassName = $(ParentSpan).attr("data-content");
        var DivsArry = DivIdsClassName.split(";");
        if(DivsArry.length >0)
        {
            ShowPopUp(DivsArry[0],DivsArry[1]);
        }
     }
}



// left: 37, up: 38, right: 39, down: 40,
// spacebar: 32, pageup: 33, pagedown: 34, end: 35, home: 36
var keys = [37, 38, 39, 40];

function preventDefault(e) {
  e = e || window.event;
  if (e.preventDefault)
      e.preventDefault();
  e.returnValue = false;  
}

function keydown(e) {
    for (var i = keys.length; i--;) {
        if (e.keyCode === keys[i]) {
            preventDefault(e);
            return;
        }
    }
}

function wheel(e) {
  preventDefault(e);
}

function disable_scroll() {
  if (window.addEventListener) {
      window.addEventListener('DOMMouseScroll', wheel, false);
  }
  window.onmousewheel = document.onmousewheel = wheel;
  document.onkeydown = keydown;
}

function enable_scroll() {
    if (window.removeEventListener) {
        window.removeEventListener('DOMMouseScroll', wheel, false);
    }
    window.onmousewheel = document.onmousewheel = document.onkeydown = null;  
}

function RedirectURL(URL,_msg,CtrlIDorClassName,IsClientSideCtrl,event)
{
    if($(".ctrlinfochanged").val().trim().toLowerCase() == "false")
    { 
        window.location = URL;
    }   
    else
    {
        GeneralConfirmationMsgForRecordSave(_msg,CtrlIDorClassName,event,IsClientSideCtrl);
    }
}