/**
 * @author Prashant Kankhara
 * @date April 2013
 */
 
(function ($) {
    $.fn.ValidationUI = function (options) {
        var settings = $.extend({}, $.fn.ValidationUI.defaults, options);
        methods.init(settings);
    }

    $.fn.ValidationUI.defaults = {
        validatorCssClassName: "error",
        ShowErrorMessage: false,
        ControlCssClassName: "checkvalidation",
        ControlHighLightClassName: "ErrorField",
        DropDownHighLightClassName: "dk_error",
        duplicateMessageCssClass: "dupmsg",
        left_pointer_spanCssClass: "error-left",
        empty_left_pointer_spanCssClass: "error-left-noback",
        PrimaryKeyIDClassName: "primarykeyIDclass",
        jointDuplicacyCheck: "jointDuplicacyCheck",
        DatePickerClassName: "setDatePicker",
        WrapSpanMessageCssClass: "wrapmsgbg",
        ApplyClassToParentLabel: false
    };

    $.fn.ValidationUI.ValidateMe = function (options, Ctrl) {
        var settings = $.extend({}, $.fn.ValidationUI.defaults, options);
        methods.ValidateFunction(Ctrl, settings);
    };
    
    
    $.fn.ValidationUI.Reset = function(options,Validator){
        var CurrCtrl = $("#" + Validator.controltovalidate);
        var settings = $.extend({}, $.fn.ValidationUI.defaults, options);
        methods.HideValidatorMessage($(Validator).siblings(".point"), CurrCtrl, settings);
    }
    
    this.methods =
        {
            init: function (settings) {

                var validatorArray = $("." + settings.validatorCssClassName);
                var duplicateSpanArray = $("." + settings.duplicateMessageCssClass);

                //if (settings.ShowErrorMessage) {
                    //Wrap Span to the Asp Validator 
                    this.WrapSpanTag(validatorArray, settings);
                    //Wrap span to the Duplicate Message 
                    this.WrapSpanTag(duplicateSpanArray, settings);
                //}
                //else 
                if (!settings.ShowErrorMessage) {
                    $("." + settings.duplicateMessageCssClass).hide();
                }

                $("." + settings.ControlCssClassName).filter(function () { if (this.type != undefined && (this.type == "text" || this.type == "password" || this.type == "textarea")) return this; }).bind("blur", function () {
                    methods.ValidateFunction(this, settings);
                });
                
                $("." + settings.DatePickerClassName).filter(function () { if (this.type != undefined && this.type == "text") return this; }).bind("change", function () {
                    methods.ValidateFunction(this, settings);
                });
                
                $("." + settings.ControlCssClassName).filter(function () { if (this.type != undefined && (this.type == "select-one" || this.type == "file")) return this; }).bind("change", function () {
                    methods.ValidateFunction(this, settings);
                });

                $(".submit").bind("click", function () {
//                    if(methods.CheckValidation(this, settings))
//                    {
//                        ShowDefaultLoader();
//                        return true;
//                    }
//                    else
//                        return false;
                    return methods.CheckValidation(this, settings);
                });
          
                $("." + settings.WrapSpanMessageCssClass).hide();
          
            },
            WrapSpanTag: function (objArray, settings) {

                if (settings.ShowErrorMessage) {
                    var left_pointer_msg_html = "<span class='point " + settings.empty_left_pointer_spanCssClass + "'></span>";
                    objArray.wrap('<span class="'+ settings.WrapSpanMessageCssClass + ' alignleft" />').before(left_pointer_msg_html);
                }
                else {
                    objArray.wrap('<span class="'+ settings.WrapSpanMessageCssClass + ' alignleft" />');
                }
            },
            ShowValidatorMessage: function (obj, Ctrl, settings,ValidatorObject) {
                if (settings.ShowErrorMessage) {
                    obj.parent("." + settings.WrapSpanMessageCssClass).siblings("." + settings.WrapSpanMessageCssClass).hide();
                    obj.addClass(settings.left_pointer_spanCssClass).removeClass(settings.empty_left_pointer_spanCssClass);
                    if ($(Ctrl).hasClass(settings.jointDuplicacyCheck)) {
                        $("." + settings.jointDuplicacyCheck).siblings().find("." + settings.duplicateMessageCssClass).parent("." + settings.WrapSpanMessageCssClass).hide();
                    }
                }
                else {
                    var DropDownControl = $(Ctrl).filter(function () { if (this.type != undefined && (this.type == "select" || this.type == "select-one")) return this; });
                    if($(DropDownControl).attr("id") != undefined)
                    {
                        //$(DropDownControl).siblings("div.dk_container").addClass(settings.DropDownHighLightClassName);
                        settings.ApplyClassToParentLabel = true;
                    }
                    else
                    {
                        settings.ApplyClassToParentLabel = false;
                    }
                    if(settings.ApplyClassToParentLabel)
                        $(Ctrl).parent().addClass(settings.ControlHighLightClassName);
                    else
                        $(Ctrl).addClass(settings.ControlHighLightClassName);
                        
                    $("." + settings.WrapSpanMessageCssClass).hide();
                    $("." + settings.validatorCssClassName).hide();
                    $(Ctrl).attr("title", $(ValidatorObject).html());
                }
            },
            HideValidatorMessage: function (obj, Ctrl, settings) {
                if (settings.ShowErrorMessage) {
                    obj.addClass(settings.empty_left_pointer_spanCssClass).removeClass(settings.left_pointer_spanCssClass);
                }
                else {
                    var DropDownControl = $(Ctrl).filter(function () { if (this.type != undefined && (this.type == "select" || this.type == "select-one")) return this; });
                    if($(DropDownControl).attr("id") != undefined)
                    {
                        //$(DropDownControl).siblings("div.dk_container").addClass(settings.DropDownHighLightClassName);
                        settings.ApplyClassToParentLabel = true;
                    }
                    else
                    {
                        settings.ApplyClassToParentLabel = false;
                    }
                        
                    if(settings.ApplyClassToParentLabel)
                        $(Ctrl).parent().removeClass(settings.ControlHighLightClassName);
                    else
                        $(Ctrl).removeClass(settings.ControlHighLightClassName);
                     $(Ctrl).removeAttr("title");
                }
            },
            // Function to set the Validation Message on Ctrl On blur event -------------Start
            ValidateFunction: function (CurrCtrl, settings) {
                var ValidatorArray = $("." + settings.validatorCssClassName).filter(function () {
                    return ((this.controltovalidate == $(CurrCtrl).attr("id") || this.controltocompare == $(CurrCtrl).attr("id")) && this.enabled != false);
                });
                if (ValidatorArray.length > 0)
                    methods.CheckValidator(ValidatorArray, settings);
            },
            // Function to set the Validation Message on Button Click  -------------Start
            CheckValidation: function (btnCtrl, settings) {
                $("." + settings.WrapSpanMessageCssClass).hide();
                var ButtonValidationGroup = $(btnCtrl).attr("call");
                if (typeof (Page_ClientValidate) == 'function') {
                    Page_ClientValidate(ButtonValidationGroup);
                }
             
                $("." + settings.ControlCssClassName).each(function () {
                    var CurrCtrlID = $(this).attr("id");
                    var ValidatorArray = $("." + settings.validatorCssClassName).filter(function () {
                        return ((this.controltovalidate == CurrCtrlID || this.controltocompare == CurrCtrlID) && this.validationGroup == ButtonValidationGroup && this.enabled != false);
                    });
                    if (ValidatorArray.length > 0)
                        methods.CheckValidator(ValidatorArray, settings);
                });

            },
            // Function to set the Validation Message on Ctrl On blur event -------------ENDS

            //All Validator Common Function-------------------------Start
            CheckValidator: function (ValidatorArray, settings) {
                var TotalValidatorCount = ValidatorArray.length;
                var ValidCount = 0;
                ValidatorArray.each(function () {
                    var ValidationGroup = this.validationGroup;
                    var CurrCtrl = $("#" + this.controltovalidate);
                    var ValidatorID = this.id;


                    if (typeof (this.evaluationfunction) == "function") {
                        this.isvalid = this.evaluationfunction(this);
                    }
                    if (this.isvalid) {
                        methods.HideValidatorMessage($(this).siblings(".point"), CurrCtrl, settings);
                        $(this).parent("." + settings.WrapSpanMessageCssClass).hide();
                        ValidCount++;
                        if (ValidCount == TotalValidatorCount) {
                            if ($(CurrCtrl).attr("call") != undefined) {
                                methods.CheckDuplicate($(CurrCtrl).attr("call"), $(CurrCtrl), settings.PrimaryKeyIDClassName, $(CurrCtrl).siblings("." + settings.WrapSpanMessageCssClass).find("." + settings.duplicateMessageCssClass).attr("id"), settings);
                            }
                        }
                    }
                    else {
                        ValidatorUpdateDisplay(this);
                        $(this).parent("." + settings.WrapSpanMessageCssClass).show();
                        methods.ShowValidatorMessage($(this).siblings(".point"), CurrCtrl, settings,$(this));
                        return false;
                    }
                });
            },
            //All Validator Common Function-------------------------Ends

            //Function to check/validate Duplicate Entries ----------------------------STARTS
            CheckDuplicate: function (URLWithFieldName, Ctrl, PrimaryKeyIDClassName, DuplicateMsgSpanID, settings) {
                var FieldList = "";
                var ValueList = "";
                var CheckDupStatus = true;

                var MethodURL = URLWithFieldName;
                var SplitURL = URLWithFieldName.split(";");
                if (SplitURL.length > 0) {
                    MethodURL = SplitURL[0];
                }

                if ($(Ctrl).hasClass(settings.jointDuplicacyCheck)) {
                    $("." + settings.jointDuplicacyCheck).each(function () {
                        var reqvalue = "";

                        if (this.type == "select-one")
                            reqvalue = "0";
                        if ($(this).val() != reqvalue) {
                            if ($(this).parent().find("." + settings.duplicateMessageCssClass).attr("id") != undefined) {
                                DuplicateMsgSpanID = $(this).parent().find("." + settings.duplicateMessageCssClass).attr("id");
                            }
                            if ($(this).attr("call") != undefined) {
                                SplitURL = $(this).attr("call").split(";");
                                if (SplitURL.length > 1) {
                                    if (FieldList != "")
                                        FieldList += ",";
                                    FieldList += SplitURL[1];
                                }


                                if (ValueList != "")
                                    ValueList += ",";
                                ValueList += $(this).val();
                            }
                        }
                        else {
                            CheckDupStatus = false;
                            return false;
                        }
                    });
                }
                else {
                    if ($(Ctrl).attr("call") != undefined) {
                        SplitURL = $(Ctrl).attr("call").split(";");
                        if (SplitURL.length > 1) {
                            if (FieldList != "")
                                FieldList += ",";
                            FieldList += SplitURL[1];
                        }
                    }
                    ValueList += $(Ctrl).val();
                }

                if ($("." + PrimaryKeyIDClassName).html() != "" && FieldList != "" && ValueList != "" && CheckDupStatus) {

                    $.ajax({
                        type: "POST",
                        url: MethodURL, //"AddStore.aspx/CheckDuplicateStoreCode",
                        data: "{'ID': '" + $("." + PrimaryKeyIDClassName).html() + "','Fields': '" + FieldList + "','Values': '" + ValueList + "'}",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            if (data.d == true || data.d == "true" || data.d == "True") {
                                $("#" + DuplicateMsgSpanID).parent().find(".point").addClass(settings.left_pointer_spanCssClass).removeClass(settings.empty_left_pointer_spanCssClass);
                                $("#" + DuplicateMsgSpanID).addClass("show");
                                $("#" + DuplicateMsgSpanID).parent("." + settings.WrapSpanMessageCssClass).show();
                                return false;

                            }
                            else {
                                $("#" + DuplicateMsgSpanID).parent().find(".point").addClass(settings.empty_left_pointer_spanCssClass).removeClass(settings.left_pointer_spanCssClass);
                                $("#" + DuplicateMsgSpanID).removeClass("show");
                                $("#" + DuplicateMsgSpanID).parent("." + settings.WrapSpanMessageCssClass).hide();
                                return true;
                            }
                        }, //end of success function
                        error: function (data) {
                        } //end of error function
                    });                             //end of ajax function
                }
                else {
                    $("#" + DuplicateMsgSpanID).parent().find(".point").addClass(settings.empty_left_pointer_spanCssClass).removeClass(settings.left_pointer_spanCssClass);
                    $("#" + DuplicateMsgSpanID).parent("." + settings.WrapSpanMessageCssClass).hide();
                }
            } //end of bind
            //Function to check/validate Duplicate Entries ----------------------------ENDS
        };
})(jQuery);
//window.onload = ValidationUI.init;